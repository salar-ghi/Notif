using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;

namespace Core.Abstractions
{
    public interface IReflectionService
    {
        TAttribute GetEnumAttribute<TAttribute>(Enum EnumValue) where TAttribute : Attribute;
        bool TryGetAttribute<TAttribute>(Type type, out TAttribute result) where TAttribute : Attribute;
        bool TryGetAttribute<TAttribute>(Type type, string propertyName, out TAttribute result) where TAttribute : Attribute;
        List<string> GetMemberNames(object obj);
        List<string> GetMemberNames(Type type);
        object GetMemberValue(object model, PropertyInfo propertyInfo);
        object GetMemberValue(object model, string propertyName);
        TResult GetMemberValue<TResult>(object model, string propertyName);
        TResult GetMemberValue<TResult>(object model, PropertyInfo propertyInfo);
        PropertyInfo GetProperty(Type type, string propertyName);

    }
}
