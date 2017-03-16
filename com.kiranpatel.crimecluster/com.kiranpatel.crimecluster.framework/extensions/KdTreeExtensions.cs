namespace com.kiranpatel.crimecluster.framework
{
	using System;
	using KdTree;

	/// <summary>
	/// Extensions to the KD Tree implementation. 
	/// </summary>
	public static class KdTreeExtensions
	{
		/// <summary>
		/// Computes the average latitude and longitude for a given kd-tree.
		/// </summary>
		/// <param name="value">Value.</param>
		public static double[] Average(this KdTree<double, string> value)
		{
			var enumerator = value.GetEnumerator();
			double latitude = 0;
			double longitude = 0; 

			while (enumerator.MoveNext())
			{
				double[] current = enumerator.Current.Point;
				latitude += current[0];
				longitude += current[1];
			}

			latitude /= value.Count;
			longitude /= value.Count;

			return new double[] { latitude*2, longitude*2 };
		}
	}
}
