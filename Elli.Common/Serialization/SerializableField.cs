// Decompiled with JetBrains decompiler
// Type: Elli.Common.Serialization.SerializableField
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using System;
using System.Reflection;

#nullable disable
namespace Elli.Common.Serialization
{
  public class SerializableField : SerializableMember
  {
    private FieldInfo field;

    public SerializableField(FieldInfo fi)
      : base((MemberInfo) fi)
    {
      this.field = fi;
    }

    public override bool CanSerialize => true;

    public override bool CanDeserialize => true;

    public override Type ValueType => this.field.FieldType;

    public override object GetValue(object sourceObject) => this.field.GetValue(sourceObject);

    public override void SetValue(object targetObject, object value)
    {
      this.field.SetValue(targetObject, value);
    }

    public static bool IsSerializable(FieldInfo field)
    {
      XmlSerializationAttribute serializationAttribute = SerializableMember.GetXmlSerializationAttribute((MemberInfo) field);
      return serializationAttribute == null ? field.IsPublic : serializationAttribute.Serializable;
    }
  }
}
