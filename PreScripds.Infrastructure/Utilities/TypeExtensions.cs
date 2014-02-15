using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace PreScripds.Infrastructure
{
    public static class TypeExtensions
    {
        public static T GetAttribute<T>(this Enum enumValue)
        where T : Attribute
        {
            return enumValue
                .GetType()
                .GetTypeInfo()
                .GetDeclaredField(enumValue.ToString())
                .GetCustomAttribute<T>();
        }

        public static string GetDisplayName(this Enum enumValue)        
        {
            var displayAttribute =
                enumValue
                .GetType()
                .GetTypeInfo()
                .GetDeclaredField(enumValue.ToString())
                .GetCustomAttribute(typeof(DisplayAttribute));

            return displayAttribute != null ? ((DisplayAttribute)displayAttribute).GetName() : enumValue.ToString();                
        }

        public static bool IsNullable(this Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }
    }
}
