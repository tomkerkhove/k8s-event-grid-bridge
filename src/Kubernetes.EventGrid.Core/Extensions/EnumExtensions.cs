using System;
using System.ComponentModel;

namespace Kubernetes.EventGrid.Core.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        ///     Gets the description for an enumeration value
        /// </summary>
        /// <remarks>Provides the raw enum value when no default description is specified</remarks>
        /// <param name="enumValue">Enumeration value</param>
        /// <param name="defaultDescription">Default description to use when not annotated</param>
        public static string GetDescription(this Enum enumValue, string defaultDescription = null)
        {
            var type = enumValue.GetType();

            var rawEnumValue = enumValue.ToString();
            var info = type.GetField(rawEnumValue);
            var descriptionAttributes = (DescriptionAttribute[]) info.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (descriptionAttributes.Length > 0)
            {
                return descriptionAttributes[0].Description;
            }

            return defaultDescription ?? rawEnumValue;
        }
    }
}