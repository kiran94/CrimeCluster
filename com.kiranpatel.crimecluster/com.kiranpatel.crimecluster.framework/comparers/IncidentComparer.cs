using System;
using System.Collections.Generic;

namespace com.kiranpatel.crimecluster.framework
{
	/// <summary>
	/// Defines the rules for comparing 2 <see cref="Incident"/> instances
	/// </summary>
	public class IncidentComparer : IComparer<Incident>
	{
		// <inheritdoc>
		public int Compare(Incident x, Incident y)
		{
			if (x == null || y == null)
			{
				return 0; 
			}

			// x has higher importance then y
			if (x.Grading.GradeValue < y.Grading.GradeValue)
			{
				return 1;
			}

			// y has greater importance then x
			if (x.Grading.GradeValue > y.Grading.GradeValue)
			{
				return -1;
			}

			return 0; 

		}
	}
}