namespace com.kiranpatel.crimecluster.framework.tests
{
	using System;
	using NUnit.Framework;

	/// <summary>
	/// Crime type extensions tests.
	/// </summary>
	public class CrimeTypeExtensionsTests
	{
		/// <summary>
		/// Ensures when other theft enum is passed, the description is returned
		/// </summary>
		[Test]
		public void GetDescription_OtherTheft_DescriptionReturned()
		{
			var type = CrimeType.OtherTheft;
			var result = type.GetDescription();
			StringAssert.AreEqualIgnoringCase("Other theft", result);
		}
	}
}