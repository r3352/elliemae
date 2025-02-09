// Decompiled with JetBrains decompiler
// Type: Elli.Common.Serialization.SerializableProperty
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using System;
using System.Reflection;

#nullable disable
namespace Elli.Common.Serialization
{
  public class SerializableProperty : SerializableMember
  {
    private PropertyInfo property;

    public SerializableProperty(PropertyInfo pi)
      : base((MemberInfo) pi)
    {
      this.property = pi;
    }

    public override bool CanSerialize => this.property.CanRead;

    public override bool CanDeserialize => this.property.CanRead && this.property.CanWrite;

    public override Type ValueType => this.property.PropertyType;

    public override object GetValue(object sourceObject)
    {
      return this.property.GetValue(sourceObject, (object[]) null);
    }

    public override void SetValue(object targetObject, object value)
    {
      this.property.SetValue(targetObject, value, (object[]) null);
    }

    public static bool IsSerializable(PropertyInfo property)
    {
      bool flag = property.GetGetMethod() != (MethodInfo) null && property.GetGetMethod().IsPublic;
      XmlSerializationAttribute serializationAttribute = SerializableMember.GetXmlSerializationAttribute((MemberInfo) property);
      return serializationAttribute == null ? flag : serializationAttribute.Serializable;
    }
  }
}
