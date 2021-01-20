using System.ComponentModel;
using System.Reflection;

namespace System
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets the description for an enumeration value
        /// </summary>
        /// <param name="enumValue">Enumeration value</param>
        /// <param name="defaultDescription">Default description to use when not annotated</param>
        public static string GetDescription(this Enum enumValue, string defaultDescription = null)
        {
            Type type = enumValue.GetType();
            FieldInfo info = type.GetField(enumValue.ToString());
            DescriptionAttribute[] descriptionAttributes = (DescriptionAttribute[])(info.GetCustomAttributes(typeof(DescriptionAttribute), false));

            if (descriptionAttributes.Length > 0)
            {
                return descriptionAttributes[0].Description;
            }

            return defaultDescription ?? string.Empty;
        }
    }
}
