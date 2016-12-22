namespace com.kiranpatel.crimecluster.framework.tests
{
	using System;
	using NUnit.Framework; 

	/// <summary>
	/// Location distance tests.
	/// </summary>
	[TestFixture]
	public class LocationDistanceTests
	{
		/// <summary>
		/// Ensures when both locations are given, the difference is returned within a given tolerance (5%)
		/// </summary>
		[Test]
		public void measure_BothLocationsGiven_DifferenceReturned()
		{
			double[] set1 = { 36.12, -86.67 };
			double[] set2 = { 33.94, -118.4 };

			var result = this.GetInstance().measure(set1, set2);

			Assert.That(result, Is.EqualTo(2888D).Within(5D).Percent);
		}

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <returns>The instance.</returns>
		private LocationDistance GetInstance()
		{
			return new LocationDistance(); 
		}
	}
}
