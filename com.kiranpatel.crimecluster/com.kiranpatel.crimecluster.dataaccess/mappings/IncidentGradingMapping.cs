namespace com.kiranpatel.crimecluster.dataaccess
{
	using System;
	using NHibernate.Mapping.ByCode.Conformist;
	using com.kiranpatel.crimecluster.framework;

	/// <summary>
	/// Mapping for the <see cref="IncidentGrading"/> entity
	/// </summary>
	public class IncidentGradingMapping : ClassMapping<IncidentGrading>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:com.kiranpatel.crimecluster.dataaccess.IncidentGradingMapping"/> class.
		/// </summary>
		public IncidentGradingMapping() 
		{
			this.Table("IncidentGrading");

			this.Id(o => o.ID);
			this.Property(o => o.GradeValue);
			this.Property(o => o.Description);
			this.Property(o => o.IsDeleted); 

			this.Where("IsDeleted = 0");
		}
	}
}