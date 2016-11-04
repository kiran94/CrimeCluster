namespace com.kiranpatel.crimecluster.dataaccess
{
	using System;
	using NHibernate.Mapping.ByCode.Conformist;
	using NHibernate.Mapping.ByCode; 
	using com.kiranpatel.crimecluster.framework;

	/// <summary>
	/// Mapping for the <see cref="Officer"/> entity
	/// </summary>
	public class OfficerMapping : ClassMapping<Officer>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:com.kiranpatel.crimecluster.dataaccess.OfficerMapping"/> class.
		/// </summary>
		public OfficerMapping()
		{
			this.Table("Person");

			this.Id(o => o.ID);
			this.Property(o => o.Title); 
			this.Property(o => o.FirstName); 
			this.Property(o => o.LastName);
			this.Property(o => o.DOB);
			this.Property(o => o.DateRegistered);
			this.Property(o => o.BadgeNumber);
			this.Property(o => o.Status);

			this.ManyToOne(o => o.Location, map => 
			{
				map.Class(typeof(Location));
				map.Column("LocationID");
				map.Cascade(Cascade.All);
			});

			//this.ManyToOne(o => o.Incident, map =>
			//{
			//	map.Class(typeof(Incident));
			//	map.Column("IncidentID");
			//	map.Cascade(Cascade.All);
			//});
		}
	}
}