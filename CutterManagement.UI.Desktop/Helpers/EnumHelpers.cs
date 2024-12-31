using System.ComponentModel;
using System.Reflection;

namespace CutterManagement.UI.Desktop
{
    public static class EnumHelpers
    {
        public static T? GetEnumAttribute<T>(Enum source) where T : Attribute
        {
            Type type = source.GetType();
            string? sourceName = Enum.GetName(type, source);
            FieldInfo? fieldInfo = type.GetField(sourceName ?? string.Empty);

            if(fieldInfo is not null)
            {
                object[] attributes = fieldInfo.GetCustomAttributes(typeof(T), false);

                foreach (object attribute in attributes)
                {
                    if (attribute is T) 
                    {
                        return (T)attribute;
                    }

                }
            }

            return null;
        }

        public static string GetDescription(Enum source)
        {
            DescriptionAttribute? descriptionAttribute = GetEnumAttribute<DescriptionAttribute>(source);
            if (descriptionAttribute is null)
                return string.Empty;    

            return descriptionAttribute.Description;
        }
    }
}
