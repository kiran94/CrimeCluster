namespace com.kiranpatel.crimecluster.webservice.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;
	using System.Web.Mvc;

	using com.kiranpatel.crimecluster.framework;

	/// <summary>
	/// Officer controller.
	/// </summary>
	public class OfficerController : BaseController
    {
		/// <summary>
		/// The officer service.
		/// </summary>
		private readonly IOfficerService officerService; 

		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="T:com.kiranpatel.crimecluster.webservice.Controllers.OfficerController"/> class.
		/// </summary>
		/// <param name="repository">Repository.</param>
		/// <param name="configService">Config service.</param>
		/// <param name="logger">Logger.</param>
		/// <param name="serialisationService">Serialisation service.</param>
		/// <param name="officerService">Officer service.</param>
		public OfficerController(
			IRepository repository,
			IConfigurationService configService,
			ILogger logger,
			ISerialisationService serialisationService,
			IOfficerService officerService) 
			: base(repository, configService, logger, serialisationService)
		{
			this.officerService = officerService; 
		}

		///// <summary>
		///// Gets data for officer with the identifier
		///// </summary>
		///// <param name="ID">Identifier.</param>
		//public String Get(String ID)
		//{
		//	ResponseResultModel response = new ResponseResultModel();
		//	Guid officerID; 

		//	if (ID == null || ID.Equals(String.Empty))
		//	{
		//		var model = this.logError("Officer: ID was null or Empty");
		//		return this.serialisationService.serialise(model); 
		//	}

		//	if (Guid.TryParse(ID, out officerID))
		//	{
		//		var model = this.logError("Officer: Failed to parse Guid");
		//		return this.serialisationService.serialise(model);
		//	}

		//	Officer officer = this.officerService.Get(officerID);

		//	if (officer == null)
		//	{
		//		var model = this.logError("Officer: Officer not found");
		//		return this.serialisationService.serialise(model);
		//	}

		//	var model = new ResponseResultModel() 
		//	{ 
		//		Status = ResponseResultType.OK, 
		//		Message = "Officer Retrieved", 
		//		Response = this.serialisationService.serialise(officer)
		//	};

		//	return this.serialisationService.serialise(model); 
		//}

		///// <summary>
		///// Saves a new Officer 
		///// </summary>
		///// <param name="serialisedOfficer">Serialised officer object to save</param>
		//public String Save(String serialisedOfficer)
		//{
		//	if (serialisedOfficer == null || serialisedOfficer.Equals(string.Empty))
		//	{
		//		var model = this.logError("Officer: Officer was null or empty");
		//		return this.serialisationService.serialise(model);
		//	}

		//	Officer officer = this.serialisationService.deserialise<Officer>(serialisedOfficer);
		//	if (officer == null)
		//	{
		//		var model = this.logError("Officer: Officer could not be deserialised");
		//		return this.serialisationService.serialise(model);
		//	}




		//}
    }
}