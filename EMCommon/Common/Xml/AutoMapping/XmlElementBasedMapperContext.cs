// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Xml.AutoMapping.XmlElementBasedMapperContext
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.Common.TimeZones;
using EllieMae.EMLite.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Common.Xml.AutoMapping
{
  public class XmlElementBasedMapperContext : IXmlMapperContext
  {
    private readonly XmlElement _xmlElement;
    private readonly AttributeReader _attributeReader;
    private readonly AttributeWriter _attributeWriter;
    private readonly System.TimeZoneInfo _timeZoneInfo;

    public XmlElementBasedMapperContext(XmlElement xmlElement)
    {
      this._xmlElement = xmlElement;
      this._attributeReader = new AttributeReader(this._xmlElement);
      this._attributeWriter = new AttributeWriter(this._xmlElement);
    }

    public XmlElementBasedMapperContext(XmlElement xmlElement, System.TimeZoneInfo timeZoneInfo)
      : this(xmlElement)
    {
      this._timeZoneInfo = timeZoneInfo;
    }

    public string GetAttribute(string attributeName)
    {
      return this._xmlElement.GetAttribute(attributeName);
    }

    private TValue GetEnumValue<TValue>(string attributeName)
    {
      string attribute = this._xmlElement.GetAttribute(attributeName);
      if (string.IsNullOrWhiteSpace(attribute))
        return default (TValue);
      Type type = Nullable.GetUnderlyingType(typeof (TValue));
      if ((object) type == null)
        type = typeof (TValue);
      Type enumType = type;
      try
      {
        return (TValue) Enum.Parse(enumType, attribute, true);
      }
      catch
      {
        return default (TValue);
      }
    }

    public TValue GetAttributeValue<TValue>(string attributeName)
    {
      if (typeof (TValue).IsEnum)
        return this.GetEnumValue<TValue>(attributeName);
      if (typeof (TValue) == typeof (DateTime))
        return (TValue) (ValueType) this._attributeReader.GetDate(attributeName);
      if (typeof (TValue) == typeof (DateTimeWithZone))
        return (TValue) (ValueType) this._attributeReader.GetDateTimeWithZone(attributeName, this._timeZoneInfo);
      return typeof (TValue) == typeof (bool) ? (TValue) (ValueType) this._attributeReader.GetBoolean(attributeName) : (TValue) this._attributeReader.GetValueWithDefault(attributeName, typeof (TValue), (object) default (TValue));
    }

    public IXmlMapperContext GetElement(string elementName)
    {
      return (IXmlMapperContext) new XmlElementBasedMapperContext(this._xmlElement.SelectSingleNode(elementName) as XmlElement, this._timeZoneInfo);
    }

    public IEnumerable<IXmlMapperContext> GetElements(string elementName)
    {
      return (IEnumerable<IXmlMapperContext>) this._xmlElement.SelectNodes(elementName).Cast<XmlElement>().Select<XmlElement, XmlElementBasedMapperContext>((Func<XmlElement, XmlElementBasedMapperContext>) (e => new XmlElementBasedMapperContext(e, this._timeZoneInfo)));
    }

    public bool HasAttribute(string attributeName) => this._xmlElement.HasAttribute(attributeName);

    public bool HasElement(string elementName)
    {
      return this._xmlElement.SelectSingleNode(elementName) != null;
    }

    public IXmlMapperContext NewElement(string elementName)
    {
      XmlElement element = this._xmlElement.OwnerDocument.CreateElement(elementName);
      this._xmlElement.AppendChild((XmlNode) element);
      return (IXmlMapperContext) new XmlElementBasedMapperContext(element, this._timeZoneInfo);
    }

    public void SetAttribute(string attributeName, object value)
    {
      this._attributeWriter.Write(attributeName, value);
    }
  }
}
