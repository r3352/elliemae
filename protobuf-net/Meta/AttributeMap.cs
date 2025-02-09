// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Meta.AttributeMap
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using System;
using System.Reflection;

#nullable disable
namespace ProtoBuf.Meta
{
  internal abstract class AttributeMap
  {
    public override string ToString() => this.AttributeType?.FullName ?? "";

    public abstract bool TryGet(string key, bool publicOnly, out object value);

    public bool TryGet(string key, out object value) => this.TryGet(key, true, out value);

    public abstract Type AttributeType { get; }

    public static AttributeMap[] Create(TypeModel model, Type type, bool inherit)
    {
      object[] customAttributes = type.GetCustomAttributes(inherit);
      AttributeMap[] attributeMapArray = new AttributeMap[customAttributes.Length];
      for (int index = 0; index < customAttributes.Length; ++index)
        attributeMapArray[index] = (AttributeMap) new AttributeMap.ReflectionAttributeMap((Attribute) customAttributes[index]);
      return attributeMapArray;
    }

    public static AttributeMap[] Create(TypeModel model, MemberInfo member, bool inherit)
    {
      object[] customAttributes = member.GetCustomAttributes(inherit);
      AttributeMap[] attributeMapArray = new AttributeMap[customAttributes.Length];
      for (int index = 0; index < customAttributes.Length; ++index)
        attributeMapArray[index] = (AttributeMap) new AttributeMap.ReflectionAttributeMap((Attribute) customAttributes[index]);
      return attributeMapArray;
    }

    public static AttributeMap[] Create(TypeModel model, Assembly assembly)
    {
      object[] customAttributes = assembly.GetCustomAttributes(false);
      AttributeMap[] attributeMapArray = new AttributeMap[customAttributes.Length];
      for (int index = 0; index < customAttributes.Length; ++index)
        attributeMapArray[index] = (AttributeMap) new AttributeMap.ReflectionAttributeMap((Attribute) customAttributes[index]);
      return attributeMapArray;
    }

    public abstract object Target { get; }

    private sealed class ReflectionAttributeMap : AttributeMap
    {
      private readonly Attribute attribute;

      public ReflectionAttributeMap(Attribute attribute) => this.attribute = attribute;

      public override object Target => (object) this.attribute;

      public override Type AttributeType => this.attribute.GetType();

      public override bool TryGet(string key, bool publicOnly, out object value)
      {
        foreach (MemberInfo fieldsAndProperty in Helpers.GetInstanceFieldsAndProperties(this.attribute.GetType(), publicOnly))
        {
          if (string.Equals(fieldsAndProperty.Name, key, StringComparison.OrdinalIgnoreCase))
          {
            switch (fieldsAndProperty)
            {
              case PropertyInfo propertyInfo:
                value = propertyInfo.GetValue((object) this.attribute, (object[]) null);
                return true;
              case FieldInfo fieldInfo:
                value = fieldInfo.GetValue((object) this.attribute);
                return true;
              default:
                throw new NotSupportedException(fieldsAndProperty.GetType().Name);
            }
          }
        }
        value = (object) null;
        return false;
      }
    }
  }
}
