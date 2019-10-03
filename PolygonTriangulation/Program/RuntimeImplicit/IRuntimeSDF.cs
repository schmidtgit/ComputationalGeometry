using PolygonTriangulation.ImplicitObjects;

namespace Program.RuntimeImplicit {
	/// <summary>
	/// Used to make it easier for the user to instantiate SDFs at runtime.
	/// </summary>
	public interface IRuntimeSDF {
		/// <summary>
		/// Expects the parameters needed to initialize the SDF.
		/// </summary>
		bool Setup(params float[] f);
		/// <summary>
		/// Returns a list of parameter descriptions.
		/// </summary>
		string[] Parameters();
		/// <summary>
		/// Returns the RuntimeSDF as a SDF
		/// </summary>
		SDF Instance();
	}
}