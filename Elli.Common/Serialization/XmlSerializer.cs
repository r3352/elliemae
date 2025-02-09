// Decompiled with JetBrains decompiler
// Type: Elli.Common.Serialization.XmlSerializer
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

#nullable disable
namespace Elli.Common.Serialization
{
  public class XmlSerializer
  {
    private static Dictionary<Type, XmlSerializer> serializerCache = new Dictionary<Type, XmlSerializer>();
    private SerializableMember[] serializableMembers;

    private XmlSerializer(Type type)
    {
      this.SerializableType = type;
      this.serializableMembers = XmlSerializer.GetSerializableMembers(type, (string) null);
    }

    public Type SerializableType { get; private set; }

    public void Serialize(object sourceObj, XElement targetElement, string contextName = null)
    {
      if (sourceObj != null && !object.Equals((object) sourceObj.GetType(), (object) this.SerializableType) && !sourceObj.GetType().IsSubclassOf(this.SerializableType))
        throw new ArgumentException("The specified object is not of the correct type for this serializer");
      if (sourceObj is IXSerializable)
      {
        ((IXSerializable) sourceObj).ToXml(targetElement, contextName);
      }
      else
      {
        if (sourceObj == null)
          return;
        this.SerializeMembers(sourceObj, targetElement, contextName);
      }
    }

    public void SerializeMembers(object sourceObj, XElement targetElement, string contextName = null)
    {
      if (sourceObj != null && !object.Equals((object) sourceObj.GetType(), (object) this.SerializableType) && !sourceObj.GetType().IsSubclassOf(this.SerializableType))
        throw new ArgumentException("The specified object is not of the correct type for this serializer");
      if (sourceObj == null)
        return;
      foreach (SerializableMember serializableMember in this.serializableMembers)
      {
        if (serializableMember.CanSerialize && serializableMember.AppliesToContext(contextName))
          XmlSerializer.serializeMember(sourceObj, serializableMember, targetElement, contextName);
      }
    }

    public void Deserialize(object targetObj, XElement sourceElement, string contextName = null)
    {
      if (targetObj == null)
        throw new ArgumentNullException("Cannot deserialize into null object reference");
      if (!object.Equals((object) targetObj.GetType(), (object) this.SerializableType) && !targetObj.GetType().IsSubclassOf(this.SerializableType))
        throw new ArgumentException("The specified object is not of the correct type for this serializer");
      if (targetObj is IXSerializable)
        ((IXSerializable) targetObj).FromXml(sourceElement, contextName);
      else
        this.DeserializeMembers(targetObj, sourceElement, contextName);
    }

    public void DeserializeMembers(object targetObj, XElement sourceElement, string contextName = null)
    {
      if (targetObj == null)
        throw new ArgumentNullException("Cannot deserialize into null object reference");
      if (!object.Equals((object) targetObj.GetType(), (object) this.SerializableType) && !targetObj.GetType().IsSubclassOf(this.SerializableType))
        throw new ArgumentException("The specified object is not of the correct type for this serializer");
      foreach (SerializableMember serializableMember in XmlSerializer.GetSerializableMembers(targetObj.GetType(), contextName))
      {
        if (serializableMember.CanDeserialize && serializableMember.AppliesToContext(contextName))
          XmlSerializer.deserializeMember(targetObj, serializableMember, sourceElement, contextName);
      }
    }

    private static void serializeMember(
      object sourceObject,
      SerializableMember member,
      XElement targetElement,
      string contextName)
    {
      object val = member.GetValue(sourceObject);
      if (val == null)
        return;
      if (member.IsSimpleType())
        new AttributeWriter(targetElement).Write(member.XmlName, val);
      else if (val is IXSerializable)
      {
        XmlSerializer.serializeXSerializable(val, member.XmlName, targetElement);
      }
      else
      {
        if (!member.IsListValued())
          return;
        XmlSerializer.serializeList(val, member, targetElement, contextName);
      }
    }

    private static void serializeXSerializable(object val, string name, XElement targetElement)
    {
      XElement xelement = new XElement((XName) name);
      ((IXSerializable) val).ToXml(xelement);
      targetElement.Add((object) xelement);
    }

    private static void serializeList(
      object val,
      SerializableMember member,
      XElement targetElement,
      string contextName)
    {
      XElement content = targetElement;
      if (member.AlwaysEmitGroupElement || content.Name != (XName) member.XmlName)
        content = new XElement((XName) member.XmlName);
      XmlSerializer xmlSerializer = XmlSerializer.Create(member.ListItemType);
      foreach (object sourceObj in (IEnumerable) val)
      {
        XElement xelement = new XElement((XName) member.XmlListItemName);
        xmlSerializer.Serialize(sourceObj, xelement, contextName);
        content.Add((object) xelement);
      }
      if (content == targetElement || !content.HasElements)
        return;
      targetElement.Add((object) content);
    }

    private static void deserializeMember(
      object targetObj,
      SerializableMember member,
      XElement sourceElement,
      string contextName)
    {
      object obj = (object) null;
      if (member.IsSimpleType())
        obj = new AttributeReader(sourceElement).GetValue(member.ValueType, member.XmlName);
      else if (typeof (IXSerializable).IsAssignableFrom(member.ValueType))
        obj = XmlSerializer.deserializeXSerializable(member.ValueType, member.XmlName, sourceElement);
      else if (member.IsListValued())
        obj = XmlSerializer.deserializeList(member, sourceElement, contextName);
      if (obj == null)
        return;
      member.SetValue(targetObj, obj);
    }

    private static object deserializeXSerializable(
      Type valType,
      string name,
      XElement sourceElement)
    {
      IXSerializable instance = (IXSerializable) XmlSerializer.createInstance(valType);
      instance.FromXml(sourceElement);
      return (object) instance;
    }

    private static object deserializeList(
      SerializableMember member,
      XElement targetElement,
      string contextName)
    {
      XElement xelement = targetElement;
      if (member.AlwaysEmitGroupElement || xelement.Name != (XName) member.XmlName)
        xelement = targetElement.Element((XName) member.XmlName);
      object instance1 = XmlSerializer.createInstance(member.ValueType);
      XmlSerializer xmlSerializer = XmlSerializer.Create(member.ListItemType);
      if (xelement != null)
      {
        foreach (XElement element in xelement.Elements((XName) member.XmlListItemName))
        {
          object instance2 = XmlSerializer.createInstance(member.ListItemType);
          xmlSerializer.Deserialize(instance2, element, contextName);
          XmlSerializer.addItemToList(instance1, instance2);
        }
      }
      return instance1;
    }

    private static void addItemToList(object listObj, object listItem)
    {
      listObj.GetType().GetMethod("Add", BindingFlags.Instance | BindingFlags.Public).Invoke(listObj, new object[1]
      {
        listItem
      });
    }

    private static bool isListType(Type t)
    {
      if (!t.IsGenericType)
        return false;
      return t.GetGenericTypeDefinition() == typeof (List<>) || t.GetGenericTypeDefinition() == typeof (IList<>);
    }

    private static object createInstance(Type t)
    {
      return XmlSerializer.isListType(t) ? Activator.CreateInstance(typeof (List<>).MakeGenericType(t.GetGenericArguments())) : Activator.CreateInstance(t);
    }

    public static SerializableMember[] GetSerializableMembers(Type type, string context)
    {
      List<SerializableMember> serializableMemberList = new List<SerializableMember>();
      for (Type type1 = type; type1 != (Type) null && type1.FullName != "Elli.Domain.Entity"; type1 = type1.BaseType)
      {
        foreach (PropertyInfo propertyInfo in (IEnumerable<PropertyInfo>) ((IEnumerable<PropertyInfo>) type1.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)).OrderBy<PropertyInfo, string>((Func<PropertyInfo, string>) (x => x.Name)))
        {
          if (SerializableProperty.IsSerializable(propertyInfo))
          {
            SerializableProperty serializableProperty = new SerializableProperty(propertyInfo);
            if (serializableProperty.AppliesToContext(context))
              serializableMemberList.Add((SerializableMember) serializableProperty);
          }
        }
        foreach (FieldInfo fieldInfo in (IEnumerable<FieldInfo>) ((IEnumerable<FieldInfo>) type1.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)).OrderBy<FieldInfo, string>((Func<FieldInfo, string>) (x => x.Name)))
        {
          if (SerializableField.IsSerializable(fieldInfo))
          {
            SerializableField serializableField = new SerializableField(fieldInfo);
            if (serializableField.AppliesToContext(context))
              serializableMemberList.Add((SerializableMember) serializableField);
          }
        }
      }
      return serializableMemberList.ToArray();
    }

    public static XmlSerializer Create(Type type)
    {
      lock (XmlSerializer.serializerCache)
      {
        if (XmlSerializer.serializerCache.ContainsKey(type))
          return XmlSerializer.serializerCache[type];
      }
      XmlSerializer xmlSerializer = new XmlSerializer(type);
      lock (XmlSerializer.serializerCache)
        XmlSerializer.serializerCache[type] = xmlSerializer;
      return xmlSerializer;
    }
  }
}
