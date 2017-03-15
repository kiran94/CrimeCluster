namespace com.kiranpatel.crimecluster.framework
{
	using System;
	using System.ComponentModel;

	/// <summary>
	/// Crime Types 
	/// </summary>
	public enum CrimeType
	{
		/// <summary>
		/// The default.
		/// </summary>
		Default,

		/// <summary>
		/// Represents anti social behaviour.
		/// </summary>
		[Description("Anti-social behaviour")]
		AntiSocialBehaviour,

		/// <summary>
		/// Represents bicycle theft.
		/// </summary>
		[Description("Bicycle theft")]
		BicycleTheft,

		/// <summary>
		/// Represents burglary.
		/// </summary>
		[Description("Burglary")]
		Burglary,

		/// <summary>
		/// Represents criminal damage and arson.
		/// </summary>
		[Description("Criminal damage and arson")]
		CriminalDamageandArson,

		/// <summary>
		/// Represents drug crime.
		/// </summary>
		[Description("Drugs")]
		Drugs,

		/// <summary>
		/// Represents other crime.
		/// </summary>
		[Description("Other crime")]
		OtherCrime,

		/// <summary>
		/// Represents other theft.
		/// </summary>
		[Description("Other theft")]
		OtherTheft,

		/// <summary>
		/// Represents the possession of weapons.
		/// </summary>
		[Description("Possession of weapons")]
		PossessionOfWeapons,

		/// <summary>
		/// Represents the public disorder and weapons.
		/// </summary>
		[Description("Public disorder and weapons")]
		PublicDisorderAndWeapons,

		/// <summary>
		/// Represents public order crime.
		/// </summary>
		[Description("Public order")]
		PublicOrder,

		/// <summary>
		/// Represents robbery.
		/// </summary>
		[Description("Robbery")]
		Robbery,

		/// <summary>
		/// Represents shoplifting.
		/// </summary>
		[Description("Shoplifting")]
		Shoplifting,

		/// <summary>
		/// Represents theft from the person.
		/// </summary>
		[Description("Theft from the person")]
		TheftFromThePerson,

		/// <summary>
		/// Represents vehicle crime.
		/// </summary>
		[Description("Vehicle crime")]
		VehicleCrime,

		/// <summary>
		/// Represents violence and sexual offences.
		/// </summary>
		[Description("Violence and sexual offences")]
		ViolenceAndSexualOffences,

		/// <summary>
		/// Represents Violent crime.
		/// </summary>
		[Description("Violent crime")]
		ViolentCrime
	}
}