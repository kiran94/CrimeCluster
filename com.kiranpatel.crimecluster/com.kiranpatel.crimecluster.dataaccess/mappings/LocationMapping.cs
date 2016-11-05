namespace com.kiranpatel.crimecluster.dataaccess
{
	using System;
	using NHibernate.Mapping;
	using NHibernate.Mapping.ByCode.Conformist;
	using com.kiranpatel.crimecluster.framework; 

	/// <summary>
	/// Mapping for the <see cref="Location"/> entity
	/// </summary>
	public class LocationMapping : ClassMapping<Location>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:com.kiranpatel.crimecluster.dataaccess.LocationMapping"/> class.
		/// </summary>
		public LocationMapping()
		{
			this.Table("Location"); 

			this.Id(x => x.ID);
			this.Property(x => x.Latitude, map => { map.NotNullable(true); });
			this.Property(x => x.Longitude, map => { map.NotNullable(true); });
			this.Property(x => x.DateLogged, map => { map.NotNullable(true); });
			this.Property(x => x.IsDeleted, map => { map.NotNullable(true); }); 

			this.Where("IsDeleted = 0");
		}
	}
}