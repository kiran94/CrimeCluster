namespace com.kiranpatel.crimecluster.framework
{
	using System;

	/// <summary>
	/// Logic for calcuting difference between 2 lat/lng coordinates
	/// </summary>
	public class LocationDistance : IDistanceMeasure
	{
		// <inheritdoc>
		public double measure(double[] set1, double[] set2)
		{
			return this.calculate(set1[0], set1[1], set2[0], set2[1]); 
		}

		/// <summary>
		/// Calculate the difference between lat1, lon1, lat2 and lon2. https://rosettacode.org/wiki/Haversine_formula#C.23
		/// </summary>
		/// <param name="lat1">Lat1.</param>
		/// <param name="lon1">Lon1.</param>
		/// <param name="lat2">Lat2.</param>
		/// <param name="lon2">Lon2.</param>
		private double calculate(double lat1, double lon1, double lat2, double lon2)
		{
			var R = 6372.8; // In kilometers
			var dLat = toRadians(lat2 - lat1);
			var dLon = toRadians(lon2 - lon1);
			lat1 = toRadians(lat1);
			lat2 = toRadians(lat2);

			var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
			return R * 2 * Math.Asin(Math.Sqrt(a));
		}

		/// <summary>
		/// To radians. https://rosettacode.org/wiki/Haversine_formula#C.23
		/// </summary>
		/// <returns>The radians.</returns>
		/// <param name="angle">Angle.</param>
		private double toRadians(double angle)
		{
			return Math.PI * angle / 180.0;
		}
	}
}
