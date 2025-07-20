using System;
using System.Reflection;
using System.Runtime.Serialization;

public static class EnumExtensions
{
    public static string GetEnumMemberValue(this Enum enumValue)
    {
        var type = enumValue.GetType();
        var member = type.GetMember(enumValue.ToString());
        if (member != null && member.Length > 0)
        {
            var attr = member[0].GetCustomAttribute<EnumMemberAttribute>(false);
            if (attr != null)
                return attr.Value;
        }
        return enumValue.ToString();
    }
}
