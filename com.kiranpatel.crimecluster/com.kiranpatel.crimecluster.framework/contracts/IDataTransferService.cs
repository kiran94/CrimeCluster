namespace com.kiranpatel.crimecluster.framework
{
	using System;

	/// <summary>
	/// Contract for transferring between domain objects and DTO objects
	/// </summary>
	/// <typeparam name="TDomain">Domain Object Type</typeparam>  
	/// <typeparam name="TDataTransfer">DTO Type</typeparam>  
	public interface IDataTransferService<TDomain, TDataTransfer> where TDomain : EntityBase
	{
		/// <summary>
		/// Transfers a {TDomain} to {TDataTransfer} object
		/// </summary>
		/// <returns>The dto.</returns>
		/// <param name="domainEntity">Domain entity.</param>
		TDataTransfer toDTO(TDomain domainEntity); 
	}
}