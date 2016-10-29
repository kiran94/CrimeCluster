namespace com.kiranpatel.crimecluster.framework
{
	using System;

	/// <summary>
	/// Contract for the Logger Service
	/// </summary>
	public interface ILogger
	{
		/// <summary>
		/// Debug the specified message.
		/// </summary>
		/// <param name="message">Message.</param>
		void debug(String message);

		/// <summary>
		/// Info the specified message.
		/// </summary>
		/// <param name="message">Message.</param>
		void info(String message);

		/// <summary>
		/// Warn the specified message.
		/// </summary>
		/// <param name="message">Message.</param>
		void warn(String message);

		/// <summary>
		/// Error the specified message.
		/// </summary>
		/// <param name="message">Message.</param>
		void error(String message);

		/// <summary>
		/// Error the specified message and e.
		/// </summary>
		/// <param name="message">Message.</param>
		/// <param name="e">E.</param>
		void error(String message, Exception e);

		/// <summary>
		/// Fatal the specified message.
		/// </summary>
		/// <param name="message">Message.</param>
		void fatal(String message);

		/// <summary>
		/// Fatal the specified message and e.
		/// </summary>
		/// <param name="message">Message.</param>
		/// <param name="e">E.</param>
		void fatal(String message, Exception e);
	}
}