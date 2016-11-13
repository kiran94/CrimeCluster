namespace com.kiranpatel.crimecluster.framework
{
	using System;

	/// <summary>
	/// Euclidean Distance Class 
	/// </summary>
	public class EuclideanDistance : IDistanceMeasure
	{
		// <inheritdoc>
		public double measure(double[] set1, double[] set2)
		{
			if (set1 == null)
			{
				throw new ArgumentNullException(nameof(set1)); 
			}

			if (set2 == null)
			{
				throw new ArgumentNullException(nameof(set2));
			}

			if (set1.Length != set2.Length)
			{
				throw new ArgumentException(String.Format("{0} does not have the same lenght as {1}", nameof(set1), nameof(set2)));
			}

			double total = 0; 
			for (int i = 0; i < set1.Length; i++)
			{
				var current = set2[i] - set1[i];
				current = Math.Pow(current, 2);
				total += current; 
			}

			return Math.Sqrt(total); 
		}
	}
}