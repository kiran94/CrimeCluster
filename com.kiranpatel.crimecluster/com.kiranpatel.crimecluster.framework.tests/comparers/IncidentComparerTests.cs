namespace com.kiranpatel.crimecluster.framework.tests
{
	using System;
	using NUnit.Framework; 

	/// <summary>
	/// Tests for the <see cref="IncidentComparer"/> class
	/// </summary>
	[TestFixture]
	public class IncidentComparerTests
	{
		/// <summary>
		/// Ensures when Incident 1 has a higher importance (lower grading) then Incident 2, 
		/// A one is returned to indicate paramter one has higher importance then two
		/// </summary>
		[Test]
		public void compare_Incident1HigherImportance_Incident1Grading1AndIncident2Grading3_One()
		{
			Incident incident1 = new Incident() { Grading = new IncidentGrading() { GradeValue = 1 } };
			Incident incident2 = new Incident() { Grading = new IncidentGrading() { GradeValue = 3 } };

			int result = this.GetInstance().Compare(incident1, incident2);

			Assert.AreEqual(1, result); 
		}

		/// <summary>
		/// Ensures when Incident 2 has higher importance (lower grading) then Incident 1, 
		/// A Minus One is returned to indicate parameter two has higher importance then one
		/// </summary>
		[Test]
		public void compare_Incident2HigherImportance_Incident1Grading2AndInciden2Grading1_MinusOne()
		{
			Incident incident1 = new Incident() { Grading = new IncidentGrading() { GradeValue = 3 } };
			Incident incident2 = new Incident() { Grading = new IncidentGrading() { GradeValue = 1 } };

			int result = this.GetInstance().Compare(incident1, incident2);

			Assert.AreEqual(-1, result);
		}

		/// <summary>
		/// Ensures when both incidents have the same grading, a zero is returned 
		//  to indicate they have equal importance
		/// </summary>
		[Test]
		public void compare_Incident1And2Equal_Zero()
		{
			Incident incident1 = new Incident() { Grading = new IncidentGrading() { GradeValue = 1 } };
			Incident incident2 = new Incident() { Grading = new IncidentGrading() { GradeValue = 1 } };

			int result = this.GetInstance().Compare(incident1, incident2);

			Assert.AreEqual(0, result);
		}

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <returns>The instance.</returns>
		public IncidentComparer GetInstance()
		{
			return new IncidentComparer(); 
		}
	}
}
