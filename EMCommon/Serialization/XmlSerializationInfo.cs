// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Serialization.XmlSerializationInfo
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.Xml;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Serialization
{
  public class XmlSerializationInfo : IEnumerable
  {
    private readonly XmlDocument xml = new XmlDocument()
    {
      XmlResolver = (XmlResolver) null
    };
    private XmlSerializationInfo.XmlElementHolder currentElementHolder;
    private Hashtable contextData = new Hashtable();
    private const char DT_DELIMITER = '|';
    private const string DT_DELIMITER_1 = "|1";
    private const string DT_DELIMITER_0 = "|0";
    private static ConcurrentDictionary<Type, ConstructorInfo> s_constructors = new ConcurrentDictionary<Type, ConstructorInfo>();

    protected internal XmlSerializationInfo()
    {
      this.currentElementHolder = new XmlSerializationInfo.XmlElementHolder((XmlElement) this.xml.AppendChild((XmlNode) this.xml.CreateElement("objdata")));
    }

    protected internal XmlSerializationInfo(Stream stream)
    {
      using (TextReader reader = XmlHelper.CreateReader(stream))
        this.xml.Load(reader);
      this.currentElementHolder = new XmlSerializationInfo.XmlElementHolder((XmlElement) this.xml.SelectSingleNode("objdata"));
      if (this.currentElementHolder.CurrentElement == null)
        throw new InvalidOperationException("Invalid XML document format");
    }

    protected internal XmlSerializationInfo(string xmldata)
    {
      this.xml.LoadXml(xmldata);
      this.currentElementHolder = new XmlSerializationInfo.XmlElementHolder((XmlElement) this.xml.SelectSingleNode("objdata"));
      if (this.currentElementHolder.CurrentElement == null)
        throw new InvalidOperationException("Invalid XML document format");
    }

    protected internal XmlSerializationInfo(XmlElement containerElement)
    {
      this.xml = containerElement.OwnerDocument;
      this.currentElementHolder = new XmlSerializationInfo.XmlElementHolder(containerElement);
    }

    public override string ToString() => this.xml.OuterXml;

    public virtual void AddValue(string name, object value)
    {
      XmlElement objectElement = this.createObjectElement(name);
      switch (value)
      {
        case null:
          objectElement.SetAttribute("null", "1");
          break;
        case IXmlSerializable _:
          XmlSerializationInfo.XmlElementHolder currentElementHolder1 = this.currentElementHolder;
          this.currentElementHolder = new XmlSerializationInfo.XmlElementHolder(objectElement);
          Hashtable contextData = this.contextData;
          this.contextData = (Hashtable) this.contextData.Clone();
          ((IXmlSerializable) value).GetXmlObjectData(this);
          this.currentElementHolder = currentElementHolder1;
          this.contextData = contextData;
          break;
        case Enum _:
          objectElement.InnerText = value.ToString();
          break;
        case DateTime dateTime:
          objectElement.InnerText = dateTime.ToString("O");
          break;
        case IConvertible _:
          string str = value.ToString();
          string[] strArray = str.Split('|');
          if (strArray.Length == 2 && (str.EndsWith("|0") || str.EndsWith("|1")))
          {
            objectElement.InnerText = strArray[0];
            if (strArray[1] == "1")
            {
              objectElement.SetAttribute("isUserModified", "Yes");
              break;
            }
            objectElement.SetAttribute("isUserModified", "No");
            break;
          }
          objectElement.InnerText = str;
          break;
        case Guid _:
          objectElement.InnerText = value.ToString();
          break;
        case Array _:
          XmlArrayList xmlArrayList = new XmlArrayList((ICollection) value);
          XmlSerializationInfo.XmlElementHolder currentElementHolder2 = this.currentElementHolder;
          this.currentElementHolder = new XmlSerializationInfo.XmlElementHolder(objectElement);
          xmlArrayList.GetXmlObjectData(this);
          this.currentElementHolder = currentElementHolder2;
          break;
        case IDictionary _:
          XmlDictionary<string> xmlDictionary = new XmlDictionary<string>((IDictionary<string, string>) value);
          XmlSerializationInfo.XmlElementHolder currentElementHolder3 = this.currentElementHolder;
          this.currentElementHolder = new XmlSerializationInfo.XmlElementHolder(objectElement);
          xmlDictionary.GetXmlObjectData(this);
          this.currentElementHolder = currentElementHolder3;
          break;
        default:
          throw new Exception("Type cannot be serialized");
      }
    }

    public object GetValue(string name, Type valueType, object defaultValue, bool isSubElement = false)
    {
      object retVal;
      return this.TryGetValue(name, valueType, out retVal, isSubElement) ? retVal : defaultValue;
    }

    public object GetValue(string name, Type valueType)
    {
      object retVal;
      if (this.TryGetValue(name, valueType, out retVal))
        return retVal;
      throw new ApplicationException("Serialization info does not contain a value with the name \"" + name + "\"");
    }

    public virtual bool TryGetValue(
      string name,
      Type valueType,
      out object retVal,
      bool isSubElement = false)
    {
      retVal = (object) null;
      XmlElement e = this.currentElementHolder.SelectElementByName(name);
      if (e == null)
      {
        if (!(name == "id") || !(valueType.Name.ToUpper() == "GUID"))
          return false;
        retVal = (object) Guid.Empty;
        return true;
      }
      if (isSubElement)
      {
        retVal = (object) e;
        return true;
      }
      if (e.GetAttribute("null") != "1")
      {
        if (typeof (IXmlSerializable).IsAssignableFrom(valueType))
        {
          XmlSerializationInfo.XmlElementHolder currentElementHolder = this.currentElementHolder;
          this.currentElementHolder = new XmlSerializationInfo.XmlElementHolder(e);
          Hashtable contextData = this.contextData;
          this.contextData = (Hashtable) this.contextData.Clone();
          try
          {
            ConstructorInfo constructor;
            if (!XmlSerializationInfo.s_constructors.TryGetValue(valueType, out constructor))
            {
              constructor = valueType.GetConstructor(new Type[1]
              {
                this.GetType()
              });
              XmlSerializationInfo.s_constructors.TryAdd(valueType, constructor);
            }
            ref object local = ref retVal;
            object obj;
            if ((object) constructor == null)
              obj = (object) null;
            else
              obj = constructor.Invoke(new object[1]
              {
                (object) this
              });
            local = obj;
          }
          catch (Exception ex)
          {
          }
          this.currentElementHolder = currentElementHolder;
          this.contextData = contextData;
        }
        else if (valueType.IsEnum)
        {
          try
          {
            retVal = Enum.Parse(valueType, e.InnerText, true);
          }
          catch
          {
            retVal = Enum.ToObject(valueType, Convert.ChangeType((object) e.InnerText, Enum.GetUnderlyingType(valueType)));
          }
        }
        else if (typeof (IConvertible).IsAssignableFrom(valueType))
        {
          try
          {
            retVal = Convert.ChangeType((object) e.InnerText, valueType);
            if (valueType.Name != "Boolean")
            {
              if (e.Attributes["isUserModified"] != null && e.Attributes["isUserModified"].Value.ToLower() == "yes")
                retVal = (object) (retVal.ToString() + "|1");
              if (e.Attributes["isUserModified"] != null)
              {
                if (e.Attributes["isUserModified"].Value.ToLower() == "no")
                  retVal = (object) (retVal.ToString() + "|0");
              }
            }
          }
          catch
          {
          }
        }
        else if (valueType.Name.ToUpper() == "GUID")
        {
          try
          {
            retVal = (object) Guid.Parse(e.InnerText);
          }
          catch
          {
          }
        }
        else if (valueType.IsArray)
        {
          XmlSerializationInfo.XmlElementHolder currentElementHolder = this.currentElementHolder;
          this.currentElementHolder = new XmlSerializationInfo.XmlElementHolder(e);
          XmlArrayList xmlArrayList = new XmlArrayList(this, valueType.GetElementType());
          retVal = (object) xmlArrayList.ToArray(valueType.GetElementType());
          this.currentElementHolder = currentElementHolder;
        }
        else
        {
          if (!valueType.IsGenericType || !(valueType.GetGenericTypeDefinition() == typeof (Dictionary<,>)))
            throw new Exception("Type cannot be deserialized");
          XmlSerializationInfo.XmlElementHolder currentElementHolder = this.currentElementHolder;
          this.currentElementHolder = new XmlSerializationInfo.XmlElementHolder(e);
          XmlDictionary<string> xmlDictionary = new XmlDictionary<string>(this);
          retVal = (object) xmlDictionary;
          this.currentElementHolder = currentElementHolder;
        }
      }
      return true;
    }

    public T GetValue<T>(string name) => (T) this.GetValue(name, typeof (T));

    public T GetValue<T>(string name, T defaultValue)
    {
      try
      {
        object retVal;
        return this.TryGetValue(name, typeof (T), out retVal) ? (T) retVal : defaultValue;
      }
      catch
      {
        return defaultValue;
      }
    }

    public string GetString(string name) => (string) this.GetValue(name, typeof (string));

    public string GetString(string name, string defaultValue)
    {
      return (string) this.GetValue(name, typeof (string), (object) defaultValue);
    }

    public DateTime GetDateTime(string name) => (DateTime) this.GetValue(name, typeof (DateTime));

    public DateTime GetDateTime(string name, DateTime defaultValue)
    {
      return (DateTime) this.GetValue(name, typeof (DateTime), (object) defaultValue);
    }

    public int GetInteger(string name) => (int) this.GetValue(name, typeof (int));

    public int GetInteger(string name, int defaultValue)
    {
      return (int) this.GetValue(name, typeof (int), (object) defaultValue);
    }

    public float GetSingle(string name) => (float) this.GetValue(name, typeof (float));

    public float GetSingle(string name, float defaultValue)
    {
      return (float) this.GetValue(name, typeof (float), (object) defaultValue);
    }

    public double GetDouble(string name) => (double) this.GetValue(name, typeof (double));

    public double GetDouble(string name, double defaultValue)
    {
      return (double) this.GetValue(name, typeof (double), (object) defaultValue);
    }

    public Decimal GetDecimal(string name) => (Decimal) this.GetValue(name, typeof (Decimal));

    public Decimal GetDecimal(string name, Decimal defaultValue)
    {
      return (Decimal) this.GetValue(name, typeof (Decimal), (object) defaultValue);
    }

    public bool GetBoolean(string name) => (bool) this.GetValue(name, typeof (bool));

    public bool GetBoolean(string name, bool defaultValue)
    {
      return (bool) this.GetValue(name, typeof (bool), (object) defaultValue);
    }

    public Array GetArray(string name, Type elementType)
    {
      return (Array) this.GetValue(name, Array.CreateInstance(elementType, 0).GetType());
    }

    public T GetEnum<T>(string name) => (T) this.GetValue(name, typeof (T));

    public T GetEnum<T>(string name, T defaultValue)
    {
      return (T) this.GetValue(name, typeof (T), (object) defaultValue);
    }

    [CLSCompliant(false)]
    public IDictionary ContextData => (IDictionary) this.contextData;

    public IEnumerator GetEnumerator()
    {
      return (IEnumerator) new XmlSerializationInfo.XmlEnumerator(this.currentElementHolder.CurrentElement);
    }

    protected XmlElement createObjectElement(string name)
    {
      XmlElement objectElement = (XmlElement) this.currentElementHolder.CurrentElement.AppendChild((XmlNode) this.xml.CreateElement("element"));
      objectElement.SetAttribute(nameof (name), name);
      return objectElement;
    }

    protected static string GetElementName(XmlElement e) => e.GetAttribute("name");

    protected class XmlEnumerator : IEnumerator
    {
      private XmlElement firstElement;
      private XmlElement currentElement;
      private bool bof = true;

      public XmlEnumerator(XmlElement parentElement)
      {
        this.firstElement = (XmlElement) parentElement.FirstChild;
      }

      public void Reset()
      {
        this.currentElement = (XmlElement) null;
        this.bof = true;
      }

      public object Current => (object) XmlSerializationInfo.GetElementName(this.currentElement);

      public bool MoveNext()
      {
        this.currentElement = !this.bof ? (XmlElement) this.currentElement.NextSibling : this.firstElement;
        this.bof = false;
        return this.currentElement != null;
      }
    }

    protected class XmlElementHolder
    {
      private XmlElement currentElement;
      private Dictionary<string, XmlElement> elements = new Dictionary<string, XmlElement>();

      public XmlElementHolder(XmlElement e)
      {
        this.currentElement = e;
        foreach (XmlNode childNode in e.ChildNodes)
        {
          if (childNode is XmlElement e1)
          {
            string elementName = XmlSerializationInfo.GetElementName(e1);
            if (!string.IsNullOrEmpty(elementName))
              this.elements[elementName] = e1;
          }
        }
      }

      public XmlElement CurrentElement => this.currentElement;

      public XmlElement SelectElementByName(string name)
      {
        XmlElement xmlElement = (XmlElement) null;
        return !this.elements.TryGetValue(name, out xmlElement) ? (XmlElement) null : xmlElement;
      }
    }
  }
}
