using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core.Abstractions;
using Core.Models;
using System.Collections.Concurrent;
using System.Reflection;

namespace Core.Services
{
    public class ReflectionService: IReflectionService
    {
        private static readonly ConcurrentDictionary<string, Attribute> _attributesCache = new ConcurrentDictionary<string, Attribute>();
        private static readonly ConcurrentDictionary<Type, List<string>> _paramNameCache = new ConcurrentDictionary<Type, List<string>>();
        private static readonly ConcurrentDictionary<Type, List<PropertyInfo>> _propertyInfoCache = new ConcurrentDictionary<Type, List<PropertyInfo>>();

        private string GetAttributeKey<TAttribute>(object obj, string propertyName = "") where TAttribute : Attribute =>
                $"{obj}_{typeof(TAttribute)}_{propertyName}";
        
        public TAttribute GetEnumAttribute<TAttribute>(Enum EnumValue) where TAttribute : Attribute
        {
            var type = EnumValue.GetType();
            var memInfo = type.GetMember(EnumValue.ToString());
            if (!memInfo.Any()) return null;
            var member = memInfo[0];
            if (!_attributesCache.TryGetValue(GetAttributeKey<TAttribute>(member), out var _result))
            {
                var attr = (TAttribute)Attribute.GetCustomAttribute(member, typeof(TAttribute));
                if (attr == null)
                    return null;

                _attributesCache.TryAdd(GetAttributeKey<TAttribute>(member), attr);
                _result = attr;
            }
            return (TAttribute)_result;
        }

        public bool TryGetAttribute<TAttribute>(Type type, out TAttribute result) where TAttribute : Attribute
        {
            result = null;
            if (type == null) throw new NullReferenceException("Object parameter is null");

            if (!_attributesCache.TryGetValue(GetAttributeKey<TAttribute>(type), out var _result))
            {
                var attr = Attribute.GetCustomAttribute(type, typeof(TAttribute));
                if (attr == null)
                    return false;

                _attributesCache.TryAdd(GetAttributeKey<TAttribute>(type), attr);
                _result = attr;
            }
            result = (TAttribute)_result;
            return result != null;
        }

        public bool TryGetAttribute<TAttribute>(Type type, string propertyName, out TAttribute result) where TAttribute : Attribute
        {
            result = null;
            if (type == null) throw new NullReferenceException("Object parameter is null");

            if (!_attributesCache.TryGetValue(GetAttributeKey<TAttribute>(type, propertyName), out var _result))
            {
                Attribute attr = null;

                if (!string.IsNullOrWhiteSpace(propertyName))
                {
                    var member = type.GetMember(propertyName);
                    if (member.Any())
                        attr = Attribute.GetCustomAttribute(member.First(), typeof(TAttribute));
                }
                else
                    attr = Attribute.GetCustomAttribute(type, typeof(TAttribute));

                if (attr == null)
                    return false;

                _attributesCache.TryAdd(GetAttributeKey<TAttribute>(type), attr);
                _result = attr;
            }
            result = (TAttribute)_result;
            return result != null;
        }

        public List<string> GetMemberNames(object obj)
        {
            if (obj == null) throw new NullReferenceException("Object parameter is null");
            return GetMemberNames(obj.GetType());
        }

        public List<string> GetMemberNames(Type type)
        {
            if (!_paramNameCache.TryGetValue(type, out List<string> paramNames))
            {
                paramNames = new List<string>();
                foreach (var prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.GetGetMethod(false) != null))
                    paramNames.Add(prop.Name);

                _paramNameCache[type] = paramNames;
            }
            return paramNames;
        }

        public object GetMemberValue(object model, PropertyInfo propertyInfo) =>
             GetMemberValue<Object>(model, propertyInfo);

        public object GetMemberValue(object model, string propertyName) =>
             GetMemberValue<Object>(model, propertyName);

        public TResult GetMemberValue<TResult>(object model, string propertyName)
        {
            var propertyInfo = GetProperty(model.GetType(), propertyName);
            if (propertyInfo is null) return default(TResult);

            return GetMemberValue<TResult>(model, propertyInfo);
        }

        public TResult GetMemberValue<TResult>(object model, PropertyInfo propertyInfo) =>
            (TResult)propertyInfo.GetValue(model, null);

        public PropertyInfo GetProperty(Type type, string propertyName)
        {
            if (!_propertyInfoCache.TryGetValue(type, out List<PropertyInfo> propertyInfos))
            {
                _propertyInfoCache[type] = type.GetProperties().ToList();
                propertyInfos = _propertyInfoCache[type];
            }
            return propertyInfos.FirstOrDefault(e => e.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase));
        }

    }

}
