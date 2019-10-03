using PolygonTriangulation.ImplicitObjects;

namespace Program.RuntimeOperator {
	public interface ISDFOperator {
		/// <summary>
		/// Expects the parameters needed to initialize the SDF.
		/// </summary>
		bool Setup(float[] f, SDF[] s);
		/// <summary>
		/// Returns a list of SDF parameter descriptions.
		/// </summary>
		string[] SDFParameters();
		/// <summary>
		/// Returns a list of float parameter descriptions.
		/// </summary>
		string[] FloatParameters();
		/// <summary>
		/// Returns the ISDFOperator as a SDF
		/// </summary>
		SDF Instance();
	}
}
