using PolygonTriangulation.Builder;
using PolygonTriangulation.ImplicitObjects;
using PolygonTriangulation.Model;
using PolygonTriangulation.Polygon;
using System;

namespace PolygonTriangulation.Triangulation {
	public abstract class GridBased : ITriangulator {
		//Settings
		protected bool useRefinement, useLazyCube, resizeGrid;

		//Dependencies
		protected SDF _obj;
		protected virtual IPolygonBuilder _builder { get; set; }

		//Overall grid values
		protected int _gridX = 128, _gridY = 128, _gridZ = 128;
		protected float _step = 1;

		//Lazy evaluation values, used for lazy cube evaluation
		protected float _lazy = 0;
		protected Vec3 _halfStep;

		//Refinement values, used for grid refinement
		protected int[] jumpValues;
		protected float[] lazyValue;
		protected Vec3[] halfstepValues;

		//Logging values, used for visualisation logging
		protected bool _useLogging;
		protected GridLog _log;

		/// <summary>
		/// Checks whether it is possible to use grid refinement and lazy cube evalutions.
		/// Will resize the triangulation grid if possible.
		/// </summary>
		protected virtual void Optimize(SDF obj) {
			useRefinement = obj.Precise();
			useLazyCube = obj.Precise();
			if (obj.RequiredGridSize() != 0) {
				int result = 1;
				if (useRefinement)
					while (result < (obj.RequiredGridSize()))
						result *= 2; // results will differ from non-refinement version if the grid can not be divided by 2.
				else
					result = obj.RequiredGridSize() % 2 == 0 ? obj.RequiredGridSize() : obj.RequiredGridSize() + 1;
				_gridX = _gridY = _gridZ = result;
			}
		}

		/// <summary>
		/// Triangulates the given object and parses the result to the builder.
		/// This will optimize the current ITriangulator to use grid refinement,
		/// lazy cube evaluation and a correct sized grid - if possible.
		/// </summary>
		public virtual IPolygon Run(SDF obj, IPolygonBuilder builder) {
			return Run(obj, builder, true);
		}

		/// <summary>
		/// Triangulates the given object and parses the result to the builder.
		/// </summary>
		/// <param name="autoOptimize">Determine weather or not to use grid refinement, lazy cube evaluation and another sized grid - based on the given SDF.</param>
		/// <returns></returns>
		public virtual IPolygon Run(SDF obj, IPolygonBuilder builder, bool autoOptimize) {
			/// Setup!
			if (autoOptimize)
				Optimize(obj);

			_obj = obj;
			_builder = builder;

			if(builder is GridLog) {
				_log = builder as GridLog;
				_useLogging = true;
			}

			if(useRefinement) {
				GridRefinement();
			} else {
				_halfStep = new Vec3(_step / 2, _step / 2, _step / 2);
				_lazy = _halfStep.Magnitude() * 1.001f;
				Initialize();
				GridLoop();
			}

			return _builder.Build();
		}

        /// <summary>
        /// Used to initialize any values needed for the algorithm.
        /// </summary>
		protected virtual void Initialize() { }

        /// <summary>
        /// Loops over the entire grid.
        /// </summary>
		protected virtual void GridLoop() {
			int xstep = _gridX / 2;
			int ystep = _gridY / 2;
			int zstep = _gridZ / 2;
			for(int x = -xstep; x < xstep; x++) {
				for(int y = -ystep; y < ystep; y++) {
					for(int z = -zstep; z < zstep; z++) {
						Step(x, y, z);
					}
				}
			}
		}

        /// <summary>
        /// Initializes refinement values and starts the gridloop.
        /// </summary>
		protected virtual void GridRefinement() {
			var steps = (int)Math.Floor(Math.Log(_gridX / _step, 2));
			var totalsteps = new int[steps];
			var stepValues = new float[steps];
			halfstepValues = new Vec3[steps];
			jumpValues = new int[steps];
			lazyValue = new float[steps];

			for(int i = 0; i < jumpValues.Length; i++) {
				jumpValues[i] = i == 0 ? 1 : jumpValues[i - 1] * 2;
			}
			for(int i = totalsteps.Length - 1; i >= 0; i--) {
				totalsteps[i] = i == totalsteps.Length - 1 ? 2 : totalsteps[i + 1] * 2;
			}
            _gridX = totalsteps[0]; _gridY = totalsteps[0]; _gridZ = totalsteps[0];
			for(int i = 0; i < stepValues.Length; i++) {
				stepValues[i] = (float)_gridX / totalsteps[i];
				halfstepValues[i] = new Vec3(stepValues[i]/2, stepValues[i]/2, stepValues[i]/2);
				lazyValue[i] = halfstepValues[i].Magnitude() * 1.001f;
			}
			_step = stepValues[0];
			Initialize();
			RefinementLoop(-jumpValues[steps-1], -jumpValues[steps - 1], -jumpValues[steps - 1], steps-1);
		}

        /// <summary>
        /// Loops over the grid using binary refinement for optimization.
        /// </summary>
		protected virtual void RefinementLoop(int minx, int miny, int minz, int looplevel) {
			var step = jumpValues[looplevel];
			for(int x = 0; x < 2; x++) {
				for(int y = 0; y < 2; y++) {
					for(int z = 0; z < 2; z++) {
						RefinementStep(minx + x * step, miny + y * step, minz + z * step, looplevel);
					}
				}
			}
		}

        /// <summary>
        /// Evaluates a single step in the refinementloop.
        /// </summary>
		protected virtual void RefinementStep(int x, int y, int z, int looplevel) {
			Vec3 origin = new Vec3(x * _step, y * _step, z * _step) + halfstepValues[looplevel];
			if(_useLogging) {
				_log.NewCube(origin, halfstepValues[looplevel].X);
			}
			if(Math.Abs(_obj.Distance(origin)) < lazyValue[looplevel]) {
				if(looplevel == 0) {
					PostStep(x, y, z, origin);
				} else {
					RefinementLoop(x, y, z, looplevel - 1);
				}
			}
		}

        /// <summary>
        /// Evaluates a single step in the grid loop.
        /// </summary>
		protected virtual void Step(int x, int y, int z) {
			/* 
			 *    o - - - o
			 *   /|      / |
			 *  o - - - o  |
			 *  | |     |  |
			 *  | o - - |- o
			 *  |/      | /
			 *  o - - - o
			 *  
			 * Origin corresponds to the center of the current cube, optimized for "lazy-cube"
			 */
			Vec3 origin = new Vec3(x * _step, y * _step, z * _step) + _halfStep;
			if(_useLogging)
				_log.NewCube(origin, _halfStep.X);
			if (!useLazyCube || Math.Abs(_obj.Distance(origin)) < _lazy)
				PostStep(x, y, z, origin);
		}

        /// <summary>
        /// Evaluates the current step and adds triangles to the builder.
        /// </summary>
		protected virtual void PostStep(int x, int y, int z, Vec3 origin) { }
	}
}
