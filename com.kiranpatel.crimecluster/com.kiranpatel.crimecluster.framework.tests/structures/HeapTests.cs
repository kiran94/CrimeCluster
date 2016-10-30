namespace com.kiranpatel.crimecluster.framework.tests
{
	using System;
	using System.Linq; 
	using NUnit.Framework;
	using com.kiranpatel.crimecluster.framework;
	using System.Collections.Generic;

	/// <summary>
	/// Tests for the <see cref="Heap"/> class
	/// </summary>
	[TestFixture]
	public class HeapTests
	{
		/// <summary>
		/// The comparer. for incident objects
		/// </summary>
		private IncidentComparer comparer = new IncidentComparer();

		/// <summary>
		/// Ensures when add is called and no items are added yet, the item is added as the root
		/// </summary>
		[Test]
		public void add_NoItemsYet_AddedAsRoot()
		{
			Incident incident = new Incident();

			var heap = this.GetInstance();
			heap.add(incident);

			var root = heap.getRoot();

			Assert.AreSame(incident, root); 
		}

		/// <summary>
		/// Ensures when add is called and items are already added, the item is added to the correct place 
		/// in accordance to the comparion function 
		/// </summary>
		[Test]
		public void add_ItemsAlreadyAdded_ItemAddedInCorrectPlace()
		{
			var incidents = new List<Incident>();
			incidents.Add(new Incident() { Grading = new IncidentGrading() { GradeValue = 4 } });
			incidents.Add(new Incident() { Grading = new IncidentGrading() { GradeValue = 4 } });
			incidents.Add(new Incident() { Grading = new IncidentGrading() { GradeValue = 6 } });
			incidents.Add(new Incident() { Grading = new IncidentGrading() { GradeValue = 2 } });
			incidents.Add(new Incident() { Grading = new IncidentGrading() { GradeValue = 3 } });
			incidents.Add(new Incident() { Grading = new IncidentGrading() { GradeValue = 7 } });
			incidents.Add(new Incident() { Grading = new IncidentGrading() { GradeValue = 3 } });

			var heap = this.GetInstance();
			incidents.ToList().ForEach(o => heap.add(o));

			var result = heap.getRoot();

			Assert.AreEqual(2, result.Grading.GradeValue); 
		}

		/// <summary>
		/// Ensures when get root is called and no items are in the heap, null is returned
		/// </summary>
		[Test]
		public void getRoot_NoItems_Null()
		{
			Assert.IsNull(this.GetInstance().getRoot()); 
		}

		/// <summary>
		/// Ensures when get root is called and items are already added to the heap, 
		/// the highest prority item is returned
		/// </summary>
		[Test]
		public void getRoot_ItemsAlreadyAdded_HighestProrityReturned()
		{
			var incidents = new List<Incident>(); 
			var heap = this.GetInstance();

			for (int i = 5; i > 0; i--)
			{
				var currentIncident = new Incident() { Grading = new IncidentGrading() { GradeValue = i } }; 
				incidents.Add(currentIncident);
				heap.add(currentIncident); 
			}

			for (int i = 1; i <= 5; i++)
			{
				Incident result = heap.getRoot();
				Assert.AreEqual(i, result.Grading.GradeValue);
			}
		}

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <returns>The instance.</returns>
		private IHeap<Incident> GetInstance()
		{
			return new Heap<Incident>(comparer); 
		}
	}
}