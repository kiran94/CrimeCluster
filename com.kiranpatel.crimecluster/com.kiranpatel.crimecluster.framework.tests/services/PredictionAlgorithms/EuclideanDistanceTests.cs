namespace com.kiranpatel.crimecluster.framework.tests
{
	using System;
	using NUnit.Framework;
	using Moq; 

	/// <summary>
	/// Euclidean distance tests.
	/// </summary>
	[TestFixture]
	public class EuclideanDistanceTests
	{
		/// <summary>
		/// Ensures when the set1 is null, an argument null exception is thrown
		/// </summary>
		[Test]
		public void measure_NullSet1_ArgumentNullException()
		{
			double[] set1 = null;
			double[] set2 = new double[1];
			Assert.Throws<ArgumentNullException>(delegate
			{
				this.GetInstance().measure(set1, set2);
			}, "set1 is null");
		}

		/// <summary>
		/// Ensures when the set2 is null, an argument null exception is thrown
		/// </summary>
		[Test]
		public void measure_NullSet2_ArgumentNullException()
		{
			double[] set1 = new double[1];
			double[] set2 = null;
			Assert.Throws<ArgumentNullException>(delegate
			{
				this.GetInstance().measure(set1, set2);
			}, "set2 is null");
		}

		/// <summary>
		/// Ensures when the sets are different lengths, an arguemnt exception is thrown
		/// </summary>
		[Test]
		public void measure_DifferentLengthSets_ArgumentException()
		{
			double[] set1 = new double[1];
			double[] set2 = new double[2];

			Assert.Throws<ArgumentException>(delegate
			{
				this.GetInstance().measure(set1, set2);
			}, "set1 does not have the same length as set2");
		}

		/// <summary>
		/// Ensures when the stes are the same lengths, a value is returned
		/// </summary>
		[Test]
		public void measure_SameLengths_Calcualted()
		{
			double[] set1 = new double[3] { 1, 2, 3 };
			double[] set2 = new double[3] { 3, 2, 1 };

			var result = this.GetInstance().measure(set1, set2);

			Assert.AreEqual(2.83, Math.Round(result, 2)); 
		}

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <returns>The instance.</returns>
		public EuclideanDistance GetInstance()
		{
			return new EuclideanDistance(); 
		}
	}
}