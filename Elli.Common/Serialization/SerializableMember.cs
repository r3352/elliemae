// Decompiled with JetBrains decompiler
// Type: Elli.Common.Serialization.SerializableMember
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using Elli.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace Elli.Common.Serialization
{
  public abstract class SerializableMember
  {
    private Type listItemType;

    protected SerializableMember(MemberInfo mi)
    {
      this.Member = mi;
      this.SerializationAttribute = SerializableMember.GetXmlSerializationAttribute(mi);
      this.SerializationDefaults = SerializableMember.GetXmlSerializationDefaultsAttribute(mi);
    }

    public MemberInfo Member { get; private set; }

    public abstract Type ValueType { get; }

    public abstract bool CanSerialize { get; }

    public abstract bool CanDeserialize { get; }

    public bool AlwaysEmitGroupElement
    {
      get
      {
        return this.SerializationAttribute != null && this.SerializationAttribute.AlwaysEmitGroupElement;
      }
    }

    protected XmlSerializationAttribute SerializationAttribute { get; private set; }

    protected XmlSerializationDefaultsAttribute SerializationDefaults { get; private set; }

    public string XmlName
    {
      get
      {
        return this.SerializationAttribute != null && !string.IsNullOrEmpty(this.SerializationAttribute.XmlName) ? this.SerializationAttribute.XmlName : this.Member.Name;
      }
    }

    public string XmlListItemName
    {
      get
      {
        return this.SerializationAttribute != null && !string.IsNullOrEmpty(this.SerializationAttribute.XmlListItemName) ? this.SerializationAttribute.XmlListItemName : this.ListItemType.Name;
      }
    }

    public bool AppliesToContext(string context)
    {
      if (context == null)
        return true;
      if (this.SerializationAttribute != null)
        return this.SerializationAttribute.AppliesToContext(context);
      return this.SerializationDefaults == null || this.SerializationDefaults.AppliesToContext(context);
    }

    public abstract object GetValue(object sourceObject);

    public abstract void SetValue(object targetObject, object value);

    public bool IsSimpleType() => SerializableMember.isSimpleType(this.ValueType);

    public bool IsListValued() => SerializableMember.isListType(this.ValueType);

    public Type ListItemType
    {
      get
      {
        lock (this)
        {
          if (this.listItemType == (Type) null)
            this.listItemType = SerializableMember.getListItemType(this.ValueType);
          return this.listItemType;
        }
      }
    }

    private static bool isSimpleType(Type t)
    {
      return t.IsPrimitive || t == typeof (string) || t == typeof (Guid) || t == typeof (DateTime) || t == typeof (Decimal) || t.IsNullableType() && SerializableMember.isSimpleType(t.GetNullableValueType()) || t.IsEnum;
    }

    private static bool isListType(Type t)
    {
      if (!t.IsGenericType)
        return false;
      return t.GetGenericTypeDefinition() == typeof (List<>) || t.GetGenericTypeDefinition() == typeof (IList<>);
    }

    public static XmlSerializationAttribute GetXmlSerializationAttribute(MemberInfo mi)
    {
      XmlSerializationAttribute[] customAttributes = (XmlSerializationAttribute[]) mi.GetCustomAttributes(typeof (XmlSerializationAttribute), true);
      return customAttributes.Length != 0 ? customAttributes[0] : (XmlSerializationAttribute) null;
    }

    public static XmlSerializationDefaultsAttribute GetXmlSerializationDefaultsAttribute(
      MemberInfo mi)
    {
      XmlSerializationDefaultsAttribute[] customAttributes = (XmlSerializationDefaultsAttribute[]) mi.ReflectedType.GetCustomAttributes(typeof (XmlSerializationDefaultsAttribute), true);
      return customAttributes.Length != 0 ? customAttributes[0] : (XmlSerializationDefaultsAttribute) null;
    }

    private static Type getListItemType(Type listType)
    {
      return SerializableMember.isListType(listType) ? listType.GetGenericArguments()[0] : throw new ArgumentException("Return type of " + listType.Name + " is not a List<> type");
    }
  }
}
