namespace com.kiranpatel.crimecluster.framework
{
	using System;
	using System.ComponentModel;
	using System.Reflection;

	/// <summary>
	/// Extensions for <see cref="CrimeType"/> enums
	/// </summary>
	public static class CrimeTypeExtensions
	{
		/// <summary>
		/// Gets the description for the enum.
		/// </summary>
		/// <returns>The description.</returns>
		/// <param name="value">Crime Type.</param>
		public static String GetDescription(this CrimeType value)
		{
			var info = value.GetType().GetField(value.ToString());
			var attributes = (DescriptionAttribute[])info.GetCustomAttributes(typeof(DescriptionAttribute), false);

			if (attributes == null || attributes.Length <= 0)
			{
				return value.ToString();
			}

			return attributes[0].Description;
		}
	}
}