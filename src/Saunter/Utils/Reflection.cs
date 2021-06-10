using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Namotion.Reflection;

namespace Saunter.Utils
{
    internal static class Reflection
    {
        public static bool HasCustomAttribute<T>(this TypeInfo typeInfo) where T : Attribute
        {
            return typeInfo.GetCustomAttribute<T>() != null;
        }
        
        public static T GetCustomAttribute<T>(this Enum enumValue) where T : Attribute
        {
            var type = enumValue.GetType();
            var memberInfo = type.GetMember(enumValue.ToString()).Single();
            var attribute = memberInfo.GetCustomAttributes<T>().FirstOrDefault();
            return attribute;
        }

        public static string GetTitle(this MemberInfo memberInfo)
        {
            var displayAttribute = memberInfo.GetCustomAttribute<DisplayAttribute>();
            return displayAttribute?.Name;
        }

        public static string GetDescription(this MemberInfo memberInfo)
        {
            var descriptionAttribute = memberInfo.GetCustomAttribute<DescriptionAttribute>();
            if (descriptionAttribute != null)
            {
                return descriptionAttribute.Description;
            }

            var displayAttribute = memberInfo.GetCustomAttribute<DisplayAttribute>();
            if (displayAttribute?.Description != null)
            {
                return displayAttribute.Description;
            }

            var xmlSummary = memberInfo.GetXmlDocsSummary();
            if (!string.IsNullOrEmpty(xmlSummary))
            {
                return xmlSummary;
            }

            return null;
        }

        public static decimal? GetMinimum(this MemberInfo memberInfo)
        {
            var rangeAttribute = memberInfo.GetCustomAttribute<RangeAttribute>();
            if (rangeAttribute?.Minimum != null && decimal.TryParse(rangeAttribute.Minimum.ToString(), out var minimum))
            {
                return minimum;
            }

            return null;
        }

        public static decimal? GetMaximum(this MemberInfo memberInfo)
        {
            var rangeAttribute = memberInfo.GetCustomAttribute<RangeAttribute>();
            if (rangeAttribute?.Maximum != null && decimal.TryParse(rangeAttribute.Maximum.ToString(), out var maximum))
            {
                return maximum;
            }

            return null;
        }

        public static int? GetMinItems(this MemberInfo memberInfo)
        {
            var minLengthAttribute = memberInfo.GetCustomAttribute<MinLengthAttribute>();
            return minLengthAttribute?.Length;
        }

        public static int? GetMaxItems(this MemberInfo memberInfo)
        {
            var maxLengthAttribute = memberInfo.GetCustomAttribute<MaxLengthAttribute>();
            return maxLengthAttribute?.Length;
        }

        public static int? GetMinLength(this MemberInfo memberInfo)
        {
            var minLengthAttribute = memberInfo.GetCustomAttribute<MinLengthAttribute>();
            if (minLengthAttribute != null)
            {
                return minLengthAttribute.Length;
            }

            var stringLengthAttribute = memberInfo.GetCustomAttribute<StringLengthAttribute>();
            return stringLengthAttribute?.MinimumLength;
        }

        public static int? GetMaxLength(this MemberInfo memberInfo)
        {
            var maxLengthAttribute = memberInfo.GetCustomAttribute<MaxLengthAttribute>();
            if (maxLengthAttribute != null)
            {
                return maxLengthAttribute.Length;
            }

            var stringLengthAttribute = memberInfo.GetCustomAttribute<StringLengthAttribute>();
            return stringLengthAttribute?.MaximumLength;
        }

        public static bool? GetIsUniqueItems(this PropertyInfo prop)
        {
            return GetIsUniqueItems(prop as MemberInfo);
        }

        public static bool? GetIsUniqueItems(this MemberInfo memberInfo)
        {
            var type = GetUnderlyingType(memberInfo);

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ISet<>))
            {
                return true;
            }

            if (type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISet<>)))
            {
                return true;
            }

            return false;
        }

        public static string GetPattern(this MemberInfo memberInfo)
        {
            var regexAttribute = memberInfo.GetCustomAttribute<RegularExpressionAttribute>();
            return regexAttribute?.Pattern;
        }

        public static bool GetIsRequired(this MemberInfo memberInfo)
        {
            var requiredAttribute = memberInfo.GetCustomAttribute<RequiredAttribute>();
            return requiredAttribute != null;
        }

        public static string GetExample(this MemberInfo memberInfo)
        {
            var xmlExample = memberInfo.GetXmlDocsTag("example");
            if (!String.IsNullOrEmpty(xmlExample))
            {
                return xmlExample;
            }

            return null;
        }

        public static Type GetUnderlyingType(MemberInfo memberInfo)
        {
            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)memberInfo).FieldType;
                case MemberTypes.Property:
                    return ((PropertyInfo)memberInfo).PropertyType;
                default:
                    throw new ArgumentException
                    (
                        "Input MemberInfo must be if type FieldInfo or PropertyInfo"
                    );
            }
        }
    }
}