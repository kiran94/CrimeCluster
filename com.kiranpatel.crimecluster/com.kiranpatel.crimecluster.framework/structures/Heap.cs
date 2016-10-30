namespace com.kiranpatel.crimecluster.framework
{
	using System;
	using System.Collections.Generic; 

	/// <summary>
	/// Minimum Heap Representation (all child nodes are larger then any particular node) 
	/// </summary>
	/// <typeparam name="T">Type of object to store in the heap</typeparam> 
	public class Heap<T> : IHeap<T>
	{
		/// <summary>
		/// The List based representation of the Heap
		/// </summary>
		private IList<T> heap;

		/// <summary>
		/// Comparer function on comparing type {T}
		/// </summary>
		private IComparer<T> comparer; 

		/// <summary>
		/// Initializes a new instance of the <see cref="T:com.kiranpatel.crimecluster.framework.MinHeap`1"/> class.
		/// </summary>
		/// <param name="comparer">Comparer.</param>
		public Heap(IComparer<T> comparer)
		{
			this.heap = new List<T>();
			this.comparer = comparer; 
		}

		// <inheritdoc>
		public void add(T toAdd)
		{
			if (this.heap.Count == 0)
			{
				this.heap.Add(toAdd);
				return; 
			}

			this.heap.Add(toAdd);
			int currentIndex = this.heap.Count - 1;
			int parentIndex = this.getParent(currentIndex);

			while (currentIndex > 0 
			       && this.comparer.Compare(this.heap[currentIndex], this.heap[parentIndex]) == 1)
			{
				this.swap(parentIndex, currentIndex); 
				currentIndex = parentIndex;
				parentIndex = this.getParent(currentIndex); 
			}
		}

		// <inheritdoc>
		public void remove(T toRemove)
		{
			throw new NotSupportedException("Incidents should not be removed from the backlog");
		}

		// <inheritdoc>
		public T getRoot()
		{
			if (this.heap.Count == 0)
			{
				return default(T); 
			}

			// get the root value 
			// get the the end of the list and place in the root (first) 
			// perform sink down

			T root = this.heap[0];
			this.heap[0] = this.heap[this.heap.Count - 1];

			this.sinkDown(0);

			return root; 
		}

		/// <summary>
		/// Starting the StartingIndex, if the left or right child is less than the startingIndex,
		/// then swap and perform the same operation on the next sub tree
		/// </summary>
		/// <param name="startingIndex">Starting Index</param>
		private void sinkDown(int startingIndex)
		{
			int smallest = startingIndex;
			int leftChildIndex = this.getLeftChild(startingIndex);
			int rightChildIndex = this.getRightChild(startingIndex); 

			if (leftChildIndex < this.heap.Count-1 && this.comparer.Compare(this.heap[smallest], this.heap[leftChildIndex]) == -1)
			{
				smallest = leftChildIndex; 
			}

			if (rightChildIndex < this.heap.Count - 1 && this.comparer.Compare(this.heap[smallest], this.heap[rightChildIndex]) == -1)
			{
				smallest = rightChildIndex; 
			}

			if (smallest != startingIndex)
			{
				this.swap(startingIndex, smallest);
				this.sinkDown(smallest); 
			}
		}

		/// <summary>
		/// Gets the left child index of passed node index
		/// </summary>
		/// <returns>The left child.</returns>
		/// <param name="index">Index.</param>
		private int getLeftChild(int index)
		{
			if (index == 0) return 1; 
			return 2 * index; 
		}

		/// <summary>
		/// Gets the right child index of the passed node index
		/// </summary>
		/// <returns>The right child.</returns>
		/// <param name="index">Index.</param>
		private int getRightChild(int index)
		{
			if (index == 0) return 2; 
			return this.getLeftChild(index) + 1;
		}

		/// <summary>
		/// Gets the Parent of the passed node index
		/// </summary>
		/// <returns>The parent.</returns>
		/// <param name="index">Index.</param>
		private int getParent(int index)
		{
			return index / 2; 
		}

		/// <summary>
		/// Swaps the heap values of the firstIndex and secondIndex
		/// </summary>
		/// <param name="firstIndex">First index.</param>
		/// <param name="secondIndex">Second index.</param>
		private void swap(int firstIndex, int secondIndex)
		{
			T temp = this.heap[firstIndex];
			this.heap[firstIndex] = this.heap[secondIndex];
			this.heap[secondIndex] = temp; 
		}
	}
}