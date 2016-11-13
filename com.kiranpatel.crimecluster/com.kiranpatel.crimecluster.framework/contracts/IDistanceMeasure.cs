namespace com.kiranpatel.crimecluster.framework
{
	using System;

	/// <summary>
	/// Contract for a distance measure. 
	/// </summary>
	public interface IDistanceMeasure
	{
		/// <summary>
		/// Measures the distance between set1 and set2
		/// </summary>
		/// <param name="set1">Set1.</param>
		/// <param name="set2">Set2.</param>
		double measure(double[] set1, double[] set2);
	}
}