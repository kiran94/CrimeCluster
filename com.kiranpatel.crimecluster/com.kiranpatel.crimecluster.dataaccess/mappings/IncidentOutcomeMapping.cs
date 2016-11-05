namespace com.kiranpatel.crimecluster.dataaccess
{
	using System;
	using NHibernate.Mapping.ByCode;
	using NHibernate.Mapping.ByCode.Conformist;
	using com.kiranpatel.crimecluster.framework; 

	/// <summary>
	/// Mapping for the <see cref="IncidentOutcome"/> entity
	/// </summary>
	public class IncidentOutcomeMapping : ClassMapping<IncidentOutcome>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:com.kiranpatel.crimecluster.dataaccess.IncidentOutcomeMapping"/> class.
		/// </summary>
		public IncidentOutcomeMapping()
		{
			this.Table("IncidentOutcome");

			this.Id(o => o.ID);
			this.Property(o => o.Outcome);
			this.Property(o => o.DateCreated);
			this.Property(o => o.IsDeleted);

			this.ManyToOne(o => o.Incident, map => 
			{
				map.Class(typeof(Incident));
				map.Cascade(Cascade.All);
				map.Column("IncidentID");
				map.NotNullable(true); 
			});

			this.ManyToOne(o => o.Officer, map => 
			{
				map.Class(typeof(Officer));
				map.Column("OfficerID");
				map.Cascade(Cascade.All); 
			}); 

			this.Where("IsDeleted = 0");
		}
	}
}