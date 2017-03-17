namespace com.kiranpatel.crimecluster.framework
{
	using KdTree;
	using System.Collections.Generic;

	/// <summary>
	/// Contract for the wrapper for the K-D Tree implementation.
	/// </summary>
	public interface IKdTreeWrapper<TKey, TVal>
	{
		/// <summary>
		/// Adds the point and value to the K-D Tree.
		/// </summary>
		/// <param name="point">Point to add.</param>
		/// <param name="value">Related Value to add.</param>
		void Add(TKey[] point, TVal value);

		/// <summary>
		/// Gets the single nearest neighbours to the passed point.
		/// </summary>
		/// <returns>The nearest neighbour to the point.</returns>
		/// <param name="point">Point to find nearest neighbour on.</param>
		TKey[] GetNearestNeighbours(TKey[] point); 

		/// <summary>
		/// Balances the K-D Tree.
		/// </summary>
		void Balance();

		/// <summary>
		/// Gets an Enumerator of kdTreeNodes from the kd tree.
		/// </summary>
		/// <returns>The enumerator.</returns>
		IEnumerator<KdTreeNode<TKey, TVal>> GetEnumerator();

		/// <summary>
		/// Counts the number of nodes in the tree.
		/// </summary>
		int Count(); 
	}
}