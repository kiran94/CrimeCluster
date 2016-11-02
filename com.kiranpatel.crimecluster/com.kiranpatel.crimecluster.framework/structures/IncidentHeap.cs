namespace com.kiranpatel.crimecluster.framework
{
	using System;

	/// <summary>
	/// Represents the heap incidents are stored in (singleton) 
	/// </summary>
	public static class IncidentHeap
	{
		/// <summary>
		/// The heap instance.
		/// </summary>
		private static Heap<Incident> heapInstance; 

		/// <summary>
		/// Gets the heap instance or if null, creates one
		/// </summary>
		/// <value>The heap.</value>
		public static Heap<Incident> Heap
		{
			get
			{
				if (heapInstance == null)
				{
					initHeap();
				}

				return heapInstance; 
			}
		}

		/// <summary>
		/// Initialises the heap
		/// </summary>
		public static void initHeap()
		{
			heapInstance = new Heap<Incident>(new IncidentComparer()); 
		}
	}
}