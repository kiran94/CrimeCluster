namespace com.kiranpatel.crimecluster.framework
{
	using KdTree;
	using System.Collections.Generic;

	/// <summary>
	/// Wrapper for the K-D Tree implementation.
	/// </summary>
	public sealed class KdTreeWrapper<TKey, TVal> : IKdTreeWrapper<TKey, TVal>
	{
		/// <summary>
		/// The kd tree instance.
		/// </summary>
		private readonly KdTree<TKey, TVal> _kdTree;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:com.kiranpatel.crimecluster.framework.KdTreeWrapper`2"/> class.
		/// </summary>
		/// <param name="mathType">Math type.</param>
		public KdTreeWrapper(ITypeMath<TKey> mathType)
		{
			this._kdTree = new KdTree<TKey, TVal>(2, mathType);
		}

		// <inheritdoc>
		public void Add(TKey[] point, TVal value)
		{
			this._kdTree.Add(point, value); 	
		}
			
		// <inheritdoc>
		public TKey[] GetNearestNeighbours(TKey[] point)
		{
			return this._kdTree.GetNearestNeighbours(point, 1)[0].Point;
		}

		// <inheritdoc>
		public void Balance()
		{
			this._kdTree.Balance();
		}

		// <inheritdoc>
		public IEnumerator<KdTreeNode<TKey, TVal>> GetEnumerator()
		{
			return this._kdTree.GetEnumerator();
		}

		// <inheritdoc>
		public int Count()
		{
			return this._kdTree.Count; 
		}
	}
}
