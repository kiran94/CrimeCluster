namespace com.kiranpatel.crimecluster.framework
{
	/// <summary>
	/// Contract for Heap Data Structures
	/// </summary>
	public interface IHeap<T> 
	{
		/// <summary>
		/// Adds a Node to the Heap in the Heap Order
		/// </summary>
		/// <param name="toAdd">To add.</param>
		void add(T toAdd);

		/// <summary>
		/// Removes the passed node from the heap
		/// </summary>
		/// <param name="toRemove">To remove.</param>
		void remove(T toRemove);

		/// <summary>
		/// Gets the Min or Max depending on the Comparison function and removes root from the heap
		/// </summary>
		/// <returns>The minimum.</returns>
		T getRoot();

		/// <summary>
		/// Gets the number of nodes in the heap
		/// </summary>
		int getSize(); 
	}
}