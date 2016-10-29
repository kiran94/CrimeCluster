namespace com.kiranpatel.crimecluster.dataaccess
{
	using System;
	using NHibernate.Mapping.ByCode;
	using NHibernate.Mapping.ByCode.Conformist;
	using com.kiranpatel.crimecluster.framework; 

	/// <summary>
	/// Mapping for the Incident Entity
	/// </summary>
	public class IncidentMapping : ClassMapping<Incident>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:com.kiranpatel.crimecluster.dataaccess.IncidentMapping"/> class.
		/// </summary>
		public IncidentMapping()
		{
			this.Table("Incident");

			this.Id(o => o.ID);
			this.Property(o => o.Summary); 
			this.Property(o => o.DateCreated);
			this.Property(o => o.IsDeleted);

			this.ManyToOne(o => o.Location, 
			               map =>
			{
				map.Class(typeof(Location));
				map.Column("LocationID");
				map.Cascade(Cascade.All); 
			});

			this.ManyToOne(o => o.Grading, 
			               map => 
			{
				map.Class(typeof(IncidentGrading));
				map.Column("IncidentGradingID");
				map.Cascade(Cascade.All); 
			});

			this.Set(o => o.Outcome,
					 map =>
			{
				map.Key(k => k.Column("IncidentID"));
				map.Inverse(true);
			}, x => x.OneToMany(y => y.Class(typeof(IncidentOutcome)))); 
		}
	}
}
