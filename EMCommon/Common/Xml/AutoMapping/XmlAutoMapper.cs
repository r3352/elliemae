// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Xml.AutoMapping.XmlAutoMapper
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.Common.TimeZones;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

#nullable disable
namespace EllieMae.EMLite.Common.Xml.AutoMapping
{
  public class XmlAutoMapper
  {
    private static bool IsSimpleType(Type type)
    {
      Type underlyingType = Nullable.GetUnderlyingType(type);
      if (underlyingType != (Type) null)
        type = underlyingType;
      return type == typeof (string) || type == typeof (Decimal) || type == typeof (DateTime) || type == typeof (DateTimeWithZone) || type == typeof (Guid) || type.IsEnum || type.IsPrimitive;
    }

    private static T GetOrAddValue<T>(
      Type type,
      ConcurrentDictionary<Type, T> dict,
      Func<Type, T> defaultFactory)
    {
      return dict.GetOrAdd(type, (Func<Type, T>) (key =>
      {
        RuntimeHelpers.RunClassConstructor(type.TypeHandle);
        return dict.GetOrAdd(key, defaultFactory);
      }));
    }

    public static XmlAutoMapper.Profile<T> NewProfile<T>() => new XmlAutoMapper.Profile<T>();

    public static void AddProfile<T>(XmlAutoMapper.Profile<T> profile)
    {
      XmlAutoMapper.Reader.AddProfile(profile.ReaderProfile);
      XmlAutoMapper.Writer.AddProfile(profile.WriterProfile);
    }

    public static void ToXml(object obj, IXmlMapperContext element)
    {
      XmlAutoMapper.Writer.ToXml(obj, element, new HashSet<object>());
    }

    public static void FromXml(IXmlMapperContext element, object obj)
    {
      XmlAutoMapper.Reader.FromXml(element, obj);
    }

    public class Reader
    {
      private static readonly ConcurrentDictionary<Type, XmlAutoMapper.Reader> xmlReaders = new ConcurrentDictionary<Type, XmlAutoMapper.Reader>();

      public Action<object, IXmlMapperContext> Converter { get; private set; }

      public bool InheritParentConfiguration { get; private set; }

      private static XmlAutoMapper.Reader GetConverter(Type type)
      {
        return XmlAutoMapper.GetOrAddValue<XmlAutoMapper.Reader>(type, XmlAutoMapper.Reader.xmlReaders, (Func<Type, XmlAutoMapper.Reader>) (key => (Activator.CreateInstance(typeof (XmlAutoMapper.Reader.Profile<>).MakeGenericType(key)) as XmlAutoMapper.Reader.IProfile).Build()));
      }

      public static void FromXml(IXmlMapperContext element, object obj)
      {
        if (obj == null)
          return;
        XmlAutoMapper.Reader.FromXml(element, obj, obj.GetType());
      }

      private static void FromXml(IXmlMapperContext element, object obj, Type type)
      {
        XmlAutoMapper.Reader converter = XmlAutoMapper.Reader.GetConverter(type);
        if (converter.InheritParentConfiguration && type.BaseType != (Type) null && type.BaseType != typeof (object))
          XmlAutoMapper.Reader.FromXml(element, obj, type.BaseType);
        converter.Converter(obj, element);
      }

      public static void AddProfile(XmlAutoMapper.Reader.IProfile profile)
      {
        XmlAutoMapper.Reader.xmlReaders[profile.ForType()] = profile.Build();
      }

      public interface IFactoryProfile
      {
        void AddInstanceFactory(
          Dictionary<Type, Func<IXmlMapperContext, object, object>> factories);
      }

      public class FactoryProfile<T, TParent> : XmlAutoMapper.Reader.IFactoryProfile
      {
        private Func<IXmlMapperContext, TParent, T> _factory;

        public FactoryProfile(Func<IXmlMapperContext, TParent, T> factory)
        {
          this._factory = factory;
        }

        public void AddInstanceFactory(
          Dictionary<Type, Func<IXmlMapperContext, object, object>> factories)
        {
          factories[typeof (TParent)] = (Func<IXmlMapperContext, object, object>) ((xmlElement, parent) => (object) this._factory(xmlElement, (TParent) parent));
        }
      }

      public interface IProfile
      {
        XmlAutoMapper.Reader Build();

        Type ForType();
      }

      public class Profile<T> : XmlAutoMapper.Reader.IProfile
      {
        private readonly HashSet<PropertyInfo> _ignoredProperties = new HashSet<PropertyInfo>(XmlAutoMapper.PropertyInfoEqualityComparer.Instance);
        private readonly Dictionary<PropertyInfo, Func<string, object>> _attributeMappers = new Dictionary<PropertyInfo, Func<string, object>>(XmlAutoMapper.PropertyInfoEqualityComparer.Instance);
        private readonly Dictionary<PropertyInfo, Func<IXmlMapperContext, T, object>> _propertyFactories = new Dictionary<PropertyInfo, Func<IXmlMapperContext, T, object>>(XmlAutoMapper.PropertyInfoEqualityComparer.Instance);
        private readonly Dictionary<PropertyInfo, string> _renames = new Dictionary<PropertyInfo, string>(XmlAutoMapper.PropertyInfoEqualityComparer.Instance);
        private bool _inheritParentConfiguration;

        public void InheritParentConfiguration() => this._inheritParentConfiguration = true;

        public void IgnoreProperty(PropertyInfo propertyInfo)
        {
          this._ignoredProperties.Add(propertyInfo);
        }

        public void ReadAttributeUsing(PropertyInfo propertyInfo, Func<string, object> mapper)
        {
          this._attributeMappers[propertyInfo] = mapper;
        }

        public void CreatePropertyUsing(
          PropertyInfo propertyInfo,
          Func<IXmlMapperContext, T, object> mapper)
        {
          this._propertyFactories[propertyInfo] = mapper;
        }

        public void Rename(PropertyInfo propertyInfo, string name)
        {
          this._renames[propertyInfo] = name;
        }

        private string GetPropertyName(PropertyInfo p)
        {
          string str;
          return this._renames.TryGetValue(p, out str) ? str : p.Name;
        }

        private static void SetSimpleTypeProperty<TProperty>(
          IXmlMapperContext element,
          T obj,
          string propertyName,
          Action<T, TProperty> setProperty)
        {
          if (element == null || (object) obj == null)
            return;
          if (!element.HasAttribute(propertyName))
            return;
          try
          {
            setProperty(obj, element.GetAttributeValue<TProperty>(propertyName));
          }
          catch
          {
          }
        }

        private static void SetSimpleTypePropertyUsingCustomMapping<TProperty>(
          IXmlMapperContext element,
          T obj,
          string propertyName,
          Action<T, TProperty> setProperty,
          Func<string, object> mapper)
        {
          if (element == null || (object) obj == null)
            return;
          if (!element.HasAttribute(propertyName))
            return;
          try
          {
            setProperty(obj, (TProperty) mapper(element.GetAttribute(propertyName)));
          }
          catch
          {
          }
        }

        private static void SetSimpleListTypeProperty<TProperty>(
          IXmlMapperContext element,
          T obj,
          string propertyName,
          Action<T, IList<TProperty>> setProperty)
        {
          if (element == null || (object) obj == null)
            return;
          if (!element.HasElement(propertyName))
            return;
          try
          {
            IXmlMapperContext element1 = element.GetElement(propertyName);
            List<TProperty> propertyList = new List<TProperty>();
            foreach (IXmlMapperContext element2 in element1.GetElements("Item"))
            {
              if (element2.HasAttribute("Value"))
                propertyList.Add(element2.GetAttributeValue<TProperty>("Value"));
            }
            setProperty(obj, (IList<TProperty>) propertyList);
          }
          catch
          {
          }
        }

        private static void SetDictionaryTypeProperty<TKey, TValue>(
          IXmlMapperContext element,
          T obj,
          string propertyName,
          Action<T, Dictionary<TKey, TValue>> setProperty)
        {
          if (element == null || (object) obj == null)
            return;
          if (!element.HasElement(propertyName))
            return;
          try
          {
            IXmlMapperContext element1 = element.GetElement(propertyName);
            Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();
            foreach (IXmlMapperContext element2 in element1.GetElements("Item"))
            {
              if (element2.HasAttribute("Key") && element2.HasAttribute("Value"))
              {
                TKey attributeValue1 = element2.GetAttributeValue<TKey>("Key");
                TValue attributeValue2 = element2.GetAttributeValue<TValue>("Value");
                dictionary[attributeValue1] = attributeValue2;
              }
            }
            setProperty(obj, dictionary);
          }
          catch
          {
          }
        }

        private static void SetComplexListTypeProperty<TProperty>(
          IXmlMapperContext element,
          T obj,
          string propertyName,
          Func<IXmlMapperContext, T, object> factory,
          Action<T, IList<TProperty>> setProperty)
        {
          if (element == null || (object) obj == null)
            return;
          if (!element.HasElement(propertyName))
            return;
          try
          {
            IXmlMapperContext element1 = element.GetElement(propertyName);
            List<TProperty> propertyList = new List<TProperty>();
            foreach (IXmlMapperContext element2 in element1.GetElements("Item"))
            {
              TProperty property = (TProperty) factory(element2, obj);
              if ((object) property != null)
              {
                propertyList.Add(property);
                XmlAutoMapper.Reader.FromXml(element2, (object) property);
              }
            }
            setProperty(obj, (IList<TProperty>) propertyList);
          }
          catch
          {
          }
        }

        private static void SetComplexTypeProperty<TProperty>(
          IXmlMapperContext element,
          T obj,
          string propertyName,
          Func<IXmlMapperContext, T, object> factory,
          Action<T, TProperty> setProperty)
        {
          if (element == null || (object) obj == null)
            return;
          if (!element.HasElement(propertyName))
            return;
          try
          {
            IXmlMapperContext element1 = element.GetElement(propertyName);
            TProperty property = (TProperty) factory(element1, obj);
            setProperty(obj, property);
            if ((object) property == null)
              return;
            XmlAutoMapper.Reader.FromXml(element1, (object) property);
          }
          catch
          {
          }
        }

        private LambdaExpression SetPropertyLambda(Type type, PropertyInfo propertyInfo)
        {
          ParameterExpression instance = Expression.Parameter(type);
          ParameterExpression parameterExpression = Expression.Parameter(propertyInfo.PropertyType);
          return Expression.Lambda((Expression) Expression.Call((Expression) instance, propertyInfo.SetMethod, (Expression) parameterExpression), instance, parameterExpression);
        }

        public XmlAutoMapper.Reader Build()
        {
          Type type = typeof (T);
          ParameterExpression parameterExpression3 = Expression.Parameter(typeof (object), "obj");
          ParameterExpression parameterExpression4 = Expression.Parameter(typeof (IXmlMapperContext), "element");
          List<Expression> source = new List<Expression>();
          BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty;
          if (this._inheritParentConfiguration)
            bindingAttr |= BindingFlags.DeclaredOnly;
          foreach (PropertyInfo property in type.GetProperties(bindingAttr))
          {
            PropertyInfo p = property;
            if (!(p.SetMethod == (MethodInfo) null) && !this._ignoredProperties.Contains(p))
            {
              if (this._attributeMappers.ContainsKey(p))
                source.Add((Expression) Expression.Call(typeof (XmlAutoMapper.Reader.Profile<T>).GetMethod("SetSimpleTypePropertyUsingCustomMapping", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(p.PropertyType), (Expression) parameterExpression4, (Expression) Expression.Convert((Expression) parameterExpression3, type), (Expression) Expression.Constant((object) this.GetPropertyName(p), typeof (string)), (Expression) this.SetPropertyLambda(type, p), (Expression) Expression.Constant((object) this._attributeMappers[p], typeof (Func<string, object>))));
              else if (XmlAutoMapper.IsSimpleType(p.PropertyType))
                source.Add((Expression) Expression.Call(typeof (XmlAutoMapper.Reader.Profile<T>).GetMethod("SetSimpleTypeProperty", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(p.PropertyType), (Expression) parameterExpression4, (Expression) Expression.Convert((Expression) parameterExpression3, type), (Expression) Expression.Constant((object) this.GetPropertyName(p), typeof (string)), (Expression) this.SetPropertyLambda(type, p)));
              else if (typeof (IEnumerable).IsAssignableFrom(p.PropertyType))
              {
                if (p.PropertyType.IsGenericType && p.PropertyType.GetGenericArguments().Length == 1)
                {
                  if (XmlAutoMapper.IsSimpleType(p.PropertyType.GetGenericArguments()[0]))
                  {
                    source.Add((Expression) Expression.Call(typeof (XmlAutoMapper.Reader.Profile<T>).GetMethod("SetSimpleListTypeProperty", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(p.PropertyType.GetGenericArguments()[0]), (Expression) parameterExpression4, (Expression) Expression.Convert((Expression) parameterExpression3, type), (Expression) Expression.Constant((object) this.GetPropertyName(p), typeof (string)), (Expression) this.SetPropertyLambda(type, p)));
                  }
                  else
                  {
                    Func<IXmlMapperContext, T, object> func;
                    if (!this._propertyFactories.TryGetValue(p, out func))
                      func = (Func<IXmlMapperContext, T, object>) ((xmlMapperContext, parent) => Activator.CreateInstance(p.PropertyType.GetGenericArguments()[0]));
                    source.Add((Expression) Expression.Call(typeof (XmlAutoMapper.Reader.Profile<T>).GetMethod("SetComplexListTypeProperty", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(p.PropertyType.GetGenericArguments()[0]), (Expression) parameterExpression4, (Expression) Expression.Convert((Expression) parameterExpression3, type), (Expression) Expression.Constant((object) this.GetPropertyName(p), typeof (string)), (Expression) Expression.Constant((object) func, typeof (Func<IXmlMapperContext, T, object>)), (Expression) this.SetPropertyLambda(type, p)));
                  }
                }
                else if (p.PropertyType.IsGenericType && p.PropertyType.GetGenericArguments().Length == 2)
                  source.Add((Expression) Expression.Call(typeof (XmlAutoMapper.Reader.Profile<T>).GetMethod("SetDictionaryTypeProperty", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(p.PropertyType.GetGenericArguments()[0], p.PropertyType.GetGenericArguments()[1]), (Expression) parameterExpression4, (Expression) Expression.Convert((Expression) parameterExpression3, type), (Expression) Expression.Constant((object) this.GetPropertyName(p), typeof (string)), (Expression) this.SetPropertyLambda(type, p)));
              }
              else
              {
                Func<IXmlMapperContext, T, object> func;
                if (!this._propertyFactories.TryGetValue(p, out func))
                  func = (Func<IXmlMapperContext, T, object>) ((xmlMapperContext, parent) => Activator.CreateInstance(p.PropertyType));
                source.Add((Expression) Expression.Call(typeof (XmlAutoMapper.Reader.Profile<T>).GetMethod("SetComplexTypeProperty", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(p.PropertyType), (Expression) parameterExpression4, (Expression) Expression.Convert((Expression) parameterExpression3, type), (Expression) Expression.Constant((object) this.GetPropertyName(p), typeof (string)), (Expression) Expression.Constant((object) func, typeof (Func<IXmlMapperContext, T, object>)), (Expression) this.SetPropertyLambda(type, p)));
              }
            }
          }
          Action<object, IXmlMapperContext> action;
          if (source.Any<Expression>())
            action = ((Expression<Action<object, IXmlMapperContext>>) ((parameterExpression1, parameterExpression2) => Expression.Block((IEnumerable<Expression>) source))).Compile();
          else
            action = (Action<object, IXmlMapperContext>) ((o, e) => { });
          return new XmlAutoMapper.Reader()
          {
            Converter = action,
            InheritParentConfiguration = this._inheritParentConfiguration
          };
        }

        public Type ForType() => typeof (T);
      }
    }

    public class Writer
    {
      private static readonly ConcurrentDictionary<Type, XmlAutoMapper.Writer> xmlWritetrs = new ConcurrentDictionary<Type, XmlAutoMapper.Writer>();

      public Action<object, IXmlMapperContext, HashSet<object>> Converter { get; private set; }

      public bool InheritParentConfiguration { get; private set; }

      private static XmlAutoMapper.Writer GetWriter(Type type)
      {
        return XmlAutoMapper.GetOrAddValue<XmlAutoMapper.Writer>(type, XmlAutoMapper.Writer.xmlWritetrs, (Func<Type, XmlAutoMapper.Writer>) (key => (Activator.CreateInstance(typeof (XmlAutoMapper.Writer.Profile<>).MakeGenericType(key)) as XmlAutoMapper.Writer.IProfile).Build()));
      }

      public static void ToXml(object obj, IXmlMapperContext element, HashSet<object> visited)
      {
        if (obj == null || visited.Contains(obj))
          return;
        visited.Add(obj);
        XmlAutoMapper.Writer.ToXml(obj, element, visited, obj.GetType());
      }

      private static void ToXml(
        object obj,
        IXmlMapperContext element,
        HashSet<object> visited,
        Type type)
      {
        XmlAutoMapper.Writer writer = XmlAutoMapper.Writer.GetWriter(type);
        if (writer.InheritParentConfiguration && type.BaseType != (Type) null && type.BaseType != typeof (object))
          XmlAutoMapper.Writer.ToXml(obj, element, visited, type.BaseType);
        writer.Converter(obj, element, visited);
      }

      public static void AddProfile(XmlAutoMapper.Writer.IProfile profile)
      {
        XmlAutoMapper.Writer.xmlWritetrs[profile.ForType()] = profile.Build();
      }

      public interface IProfile
      {
        XmlAutoMapper.Writer Build();

        Type ForType();
      }

      public class Profile<T> : XmlAutoMapper.Writer.IProfile
      {
        private readonly HashSet<PropertyInfo> _ignoredProperties = new HashSet<PropertyInfo>(XmlAutoMapper.PropertyInfoEqualityComparer.Instance);
        private readonly Dictionary<PropertyInfo, Func<object, string>> _attributeMappers = new Dictionary<PropertyInfo, Func<object, string>>(XmlAutoMapper.PropertyInfoEqualityComparer.Instance);
        private readonly Dictionary<PropertyInfo, string> _renames = new Dictionary<PropertyInfo, string>(XmlAutoMapper.PropertyInfoEqualityComparer.Instance);
        private bool _inheritParentConfiguration;

        public void InheritParentConfiguration() => this._inheritParentConfiguration = true;

        public void IgnoreProperty(PropertyInfo propertyInfo)
        {
          this._ignoredProperties.Add(propertyInfo);
        }

        public void WriteAttributeUsing(PropertyInfo propertyInfo, Func<object, string> mapper)
        {
          this._attributeMappers[propertyInfo] = mapper;
        }

        public void Rename(PropertyInfo propertyInfo, string name)
        {
          this._renames[propertyInfo] = name;
        }

        private static void AddSimpleTypeAttributeUsingCustomMapping(
          IXmlMapperContext element,
          string name,
          object obj,
          Func<object, string> mapper)
        {
          string str = mapper(obj);
          if (str == null)
            return;
          element.SetAttribute(name, (object) str.ToString());
        }

        private static void AddSimpleTypeAttribute(
          IXmlMapperContext writer,
          string name,
          object value)
        {
          if (value == null)
            return;
          writer.SetAttribute(name, value);
        }

        private static void AddSimpleListElements(
          IXmlMapperContext parentElement,
          string listElementName,
          IEnumerable items)
        {
          IXmlMapperContext xmlMapperContext = parentElement.NewElement(listElementName);
          foreach (object obj in items)
            xmlMapperContext.NewElement("Item").SetAttribute("Value", obj);
        }

        private static void AddComplexListElements(
          IXmlMapperContext parentElement,
          string listElementName,
          IEnumerable items,
          HashSet<object> visited)
        {
          IXmlMapperContext xmlMapperContext = parentElement.NewElement(listElementName);
          foreach (object obj in items)
            XmlAutoMapper.Writer.ToXml(obj, xmlMapperContext.NewElement("Item"), visited);
        }

        private static void AddComplexTypeElement(
          IXmlMapperContext parentElement,
          string elementName,
          object item,
          HashSet<object> visited)
        {
          IXmlMapperContext element = parentElement.NewElement(elementName);
          XmlAutoMapper.Writer.ToXml(item, element, visited);
        }

        private string GetPropertyName(PropertyInfo p)
        {
          string str;
          return this._renames.TryGetValue(p, out str) ? str : p.Name;
        }

        public XmlAutoMapper.Writer Build()
        {
          Type type = typeof (T);
          ParameterExpression parameterExpression4 = Expression.Parameter(typeof (object), "obj");
          ParameterExpression parameterExpression5 = Expression.Parameter(typeof (IXmlMapperContext), "element");
          ParameterExpression parameterExpression6 = Expression.Parameter(typeof (HashSet<object>), "visited");
          List<Expression> source = new List<Expression>();
          BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty;
          if (this._inheritParentConfiguration)
            bindingAttr |= BindingFlags.DeclaredOnly;
          foreach (PropertyInfo property in type.GetProperties(bindingAttr))
          {
            if (!this._ignoredProperties.Contains(property))
            {
              if (this._attributeMappers.ContainsKey(property))
                source.Add((Expression) Expression.Call(typeof (XmlAutoMapper.Writer.Profile<T>).GetMethod("AddSimpleTypeAttributeUsingCustomMapping", BindingFlags.Static | BindingFlags.NonPublic), (Expression) parameterExpression5, (Expression) Expression.Constant((object) this.GetPropertyName(property), typeof (string)), (Expression) parameterExpression4, (Expression) Expression.Constant((object) this._attributeMappers[property], typeof (Func<object, string>))));
              else if (XmlAutoMapper.IsSimpleType(property.PropertyType))
                source.Add((Expression) Expression.Call(typeof (XmlAutoMapper.Writer.Profile<T>).GetMethod("AddSimpleTypeAttribute", BindingFlags.Static | BindingFlags.NonPublic), (Expression) parameterExpression5, (Expression) Expression.Constant((object) this.GetPropertyName(property), typeof (string)), (Expression) Expression.Convert((Expression) Expression.Property((Expression) Expression.Convert((Expression) parameterExpression4, type), property), typeof (object))));
              else if (typeof (IEnumerable).IsAssignableFrom(property.PropertyType))
              {
                if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericArguments().Length == 1)
                {
                  if (XmlAutoMapper.IsSimpleType(property.PropertyType.GetGenericArguments()[0]))
                    source.Add((Expression) Expression.Call(typeof (XmlAutoMapper.Writer.Profile<T>).GetMethod("AddSimpleListElements", BindingFlags.Static | BindingFlags.NonPublic), (Expression) parameterExpression5, (Expression) Expression.Constant((object) this.GetPropertyName(property), typeof (string)), (Expression) Expression.Convert((Expression) Expression.Property((Expression) Expression.Convert((Expression) parameterExpression4, type), property), typeof (IEnumerable))));
                  else
                    source.Add((Expression) Expression.Call(typeof (XmlAutoMapper.Writer.Profile<T>).GetMethod("AddComplexListElements", BindingFlags.Static | BindingFlags.NonPublic), (Expression) parameterExpression5, (Expression) Expression.Constant((object) this.GetPropertyName(property), typeof (string)), (Expression) Expression.Convert((Expression) Expression.Property((Expression) Expression.Convert((Expression) parameterExpression4, type), property), typeof (IEnumerable)), (Expression) parameterExpression6));
                }
                else if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericArguments().Length == 2)
                  source.Add((Expression) Expression.Call(typeof (XmlAutoMapper.Writer.Profile<T>).GetMethod("AddComplexListElements", BindingFlags.Static | BindingFlags.NonPublic), (Expression) parameterExpression5, (Expression) Expression.Constant((object) this.GetPropertyName(property), typeof (string)), (Expression) Expression.Convert((Expression) Expression.Property((Expression) Expression.Convert((Expression) parameterExpression4, type), property), typeof (IEnumerable)), (Expression) parameterExpression6));
              }
              else
                source.Add((Expression) Expression.Call(typeof (XmlAutoMapper.Writer.Profile<T>).GetMethod("AddComplexTypeElement", BindingFlags.Static | BindingFlags.NonPublic), (Expression) parameterExpression5, (Expression) Expression.Constant((object) this.GetPropertyName(property), typeof (string)), (Expression) Expression.Convert((Expression) Expression.Property((Expression) Expression.Convert((Expression) parameterExpression4, type), property), typeof (object)), (Expression) parameterExpression6));
            }
          }
          Action<object, IXmlMapperContext, HashSet<object>> action;
          if (source.Any<Expression>())
            action = ((Expression<Action<object, IXmlMapperContext, HashSet<object>>>) ((parameterExpression1, parameterExpression2, parameterExpression3) => Expression.Block((IEnumerable<Expression>) source))).Compile();
          else
            action = (Action<object, IXmlMapperContext, HashSet<object>>) ((o, e, v) => { });
          return new XmlAutoMapper.Writer()
          {
            Converter = action,
            InheritParentConfiguration = this._inheritParentConfiguration
          };
        }

        public Type ForType() => typeof (T);
      }
    }

    public class PropertyInfoEqualityComparer : IEqualityComparer<PropertyInfo>
    {
      public static readonly IEqualityComparer<PropertyInfo> Instance = (IEqualityComparer<PropertyInfo>) new XmlAutoMapper.PropertyInfoEqualityComparer();

      private PropertyInfoEqualityComparer()
      {
      }

      public bool Equals(PropertyInfo x, PropertyInfo y)
      {
        if (x == (PropertyInfo) null && y == (PropertyInfo) null)
          return true;
        return !(x == (PropertyInfo) null) && !(y == (PropertyInfo) null) && object.Equals((object) x.PropertyType, (object) y.PropertyType) && object.Equals((object) x.Name, (object) y.Name);
      }

      public int GetHashCode(PropertyInfo obj)
      {
        return (17 * 23 + obj.PropertyType.GetHashCode()) * 23 + obj.Name.GetHashCode();
      }
    }

    public class Profile<T>
    {
      protected readonly XmlAutoMapper.Reader.Profile<T> _readerProfile = new XmlAutoMapper.Reader.Profile<T>();
      private readonly XmlAutoMapper.Writer.Profile<T> _writerProfile = new XmlAutoMapper.Writer.Profile<T>();

      public XmlAutoMapper.Profile<T> ForMember<TMember>(
        Expression<Func<T, TMember>> exp,
        Action<XmlAutoMapper.Profile<T>.ProfileOptions<TMember>> options)
      {
        XmlAutoMapper.Profile<T>.ProfileOptions<TMember> profileOptions = new XmlAutoMapper.Profile<T>.ProfileOptions<TMember>(this, exp);
        options(profileOptions);
        return this;
      }

      public XmlAutoMapper.Profile<T> ForCollectionMember<TItem>(
        Expression<Func<T, IList<TItem>>> exp,
        Action<XmlAutoMapper.Profile<T>.ProfileOptions<IList<TItem>, TItem>> options)
      {
        XmlAutoMapper.Profile<T>.ProfileOptions<IList<TItem>, TItem> profileOptions = new XmlAutoMapper.Profile<T>.ProfileOptions<IList<TItem>, TItem>(this, exp);
        options(profileOptions);
        return this;
      }

      public XmlAutoMapper.Profile<T> InheritParentConfiguration()
      {
        this._readerProfile.InheritParentConfiguration();
        this._writerProfile.InheritParentConfiguration();
        return this;
      }

      public XmlAutoMapper.Reader.IProfile ReaderProfile
      {
        get => (XmlAutoMapper.Reader.IProfile) this._readerProfile;
      }

      public XmlAutoMapper.Writer.IProfile WriterProfile
      {
        get => (XmlAutoMapper.Writer.IProfile) this._writerProfile;
      }

      public class ProfileOptions<TMember>
      {
        protected readonly XmlAutoMapper.Profile<T> _parent;
        protected readonly Expression<Func<T, TMember>> _exp;

        public ProfileOptions(XmlAutoMapper.Profile<T> parent, Expression<Func<T, TMember>> exp)
        {
          if (exp == null || exp.Body == null || !(exp.Body is MemberExpression) || (object) ((exp.Body as MemberExpression).Member as PropertyInfo) == null)
            throw new ArgumentException("exp should refer to a property in type " + typeof (T).Name);
          this._parent = parent;
          this._exp = exp;
        }

        public XmlAutoMapper.Profile<T>.ProfileOptions<TMember> Ignore()
        {
          PropertyInfo member = (this._exp.Body as MemberExpression).Member as PropertyInfo;
          this._parent._readerProfile.IgnoreProperty(member);
          this._parent._writerProfile.IgnoreProperty(member);
          return this;
        }

        public XmlAutoMapper.Profile<T>.ProfileOptions<TMember> IgnoreInReader()
        {
          this._parent._readerProfile.IgnoreProperty((this._exp.Body as MemberExpression).Member as PropertyInfo);
          return this;
        }

        public XmlAutoMapper.Profile<T>.ProfileOptions<TMember> IgnoreInWriter()
        {
          this._parent._writerProfile.IgnoreProperty((this._exp.Body as MemberExpression).Member as PropertyInfo);
          return this;
        }

        public XmlAutoMapper.Profile<T>.ProfileOptions<TMember> WriteAttributeUsing(
          Func<TMember, string> mapper)
        {
          Func<T, TMember> getMember = this._exp.Compile();
          this._parent._writerProfile.WriteAttributeUsing((this._exp.Body as MemberExpression).Member as PropertyInfo, (Func<object, string>) (obj =>
          {
            if (obj == null || !(obj is T obj2))
              return string.Empty;
            TMember member = getMember(obj2);
            return (object) member == null ? string.Empty : mapper(member);
          }));
          return this;
        }

        public XmlAutoMapper.Profile<T>.ProfileOptions<TMember> ReadAttributeUsing(
          Func<string, object> mapper)
        {
          this._parent._readerProfile.ReadAttributeUsing((this._exp.Body as MemberExpression).Member as PropertyInfo, (Func<string, object>) (attrValue => mapper(attrValue)));
          return this;
        }

        public XmlAutoMapper.Profile<T>.ProfileOptions<TMember> CreateUsing(
          Func<IXmlMapperContext, T, TMember> mapper)
        {
          this._parent._readerProfile.CreatePropertyUsing((this._exp.Body as MemberExpression).Member as PropertyInfo, (Func<IXmlMapperContext, T, object>) ((xmlContext, parent) => (object) mapper(xmlContext, parent)));
          return this;
        }

        public XmlAutoMapper.Profile<T>.ProfileOptions<TMember> Rename(string newName)
        {
          PropertyInfo member = (this._exp.Body as MemberExpression).Member as PropertyInfo;
          this._parent._readerProfile.Rename(member, newName);
          this._parent._writerProfile.Rename(member, newName);
          return this;
        }
      }

      public class ProfileOptions<TEnumerableMember, TItem>(
        XmlAutoMapper.Profile<T> parent,
        Expression<Func<T, TEnumerableMember>> exp) : 
        XmlAutoMapper.Profile<T>.ProfileOptions<TEnumerableMember>(parent, exp)
        where TEnumerableMember : IEnumerable<TItem>
      {
        public XmlAutoMapper.Profile<T>.ProfileOptions<TEnumerableMember, TItem> CreateItemUsing(
          Func<IXmlMapperContext, T, TItem> mapper)
        {
          this._parent._readerProfile.CreatePropertyUsing((this._exp.Body as MemberExpression).Member as PropertyInfo, (Func<IXmlMapperContext, T, object>) ((xmlContext, parent) => (object) mapper(xmlContext, parent)));
          return this;
        }
      }
    }
  }
}
