
namespace com.kiranpatel.crimecluster.framework
{
	using System;

	/// <summary>
	/// Contract for the Incident Grading Service
	/// </summary>
	public interface IIncidentGradingService : IEntityService<IncidentGrading>
	{
		/// <summary>
		/// Gets the import incident grading.
		/// </summary>
		/// <returns>The import incident grading.</returns>
		IncidentGrading GetImportIncidentGrading(); 
	}
}