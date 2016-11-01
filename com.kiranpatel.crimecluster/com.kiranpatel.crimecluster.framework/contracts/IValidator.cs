namespace com.kiranpatel.crimecluster.framework
{
	using System;

	/// <summary>
	/// Contract for validating Entities
	/// </summary>
	public interface IValidator<T> where T : EntityBase
	{
		/// <summary>
		/// Validates that passed T obeys the rules of the application
		/// </summary>
		/// <param name="toValidate">object go validate.</param>
		bool validate(T toValidate);
	}
}