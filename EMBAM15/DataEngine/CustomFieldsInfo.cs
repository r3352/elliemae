// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.CustomFieldsInfo
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Cache;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.XPath;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class CustomFieldsInfo : FastSerializable, IEnumerable, ISerializable
  {
    private const string className = "CustomFieldsInfo";
    private static readonly string sw = Tracing.SwDataEngine;
    private string xmlData;
    private Hashtable map;
    public const int MinStandardFieldIndex = 1;
    public const int MaxStandardFieldIndex = 100;
    private const string FastSerializeVersion = "{v1}";

    public CustomFieldsInfo(bool bLoadStandardFields)
      : base("{v1}")
    {
      this.map = CollectionsUtil.CreateCaseInsensitiveHashtable();
      if (!bLoadStandardFields)
        return;
      this.loadStandardFields();
    }

    public CustomFieldsInfo(string xmlData)
      : base("{v1}")
    {
      this.xmlData = xmlData;
    }

    public CustomFieldsInfo(SerializationInfo info, StreamingContext context)
      : base("{v1}", info, context)
    {
    }

    public string ToXML()
    {
      this.parseXml();
      return this.ToString();
    }

    private void parseXml()
    {
      lock (this)
      {
        if (this.xmlData == null)
          return;
        this.map = CollectionsUtil.CreateCaseInsensitiveHashtable();
        try
        {
          XPathNodeIterator xpathNodeIterator = new XPathDocument((TextReader) new StringReader(this.xmlData)).CreateNavigator().Select("/CustomFieldList/Field");
          while (xpathNodeIterator.MoveNext())
          {
            string attribute1 = xpathNodeIterator.Current.GetAttribute("id", string.Empty);
            string attribute2 = xpathNodeIterator.Current.GetAttribute("desc", string.Empty);
            string attribute3 = xpathNodeIterator.Current.GetAttribute("type", string.Empty);
            FieldFormat fieldFormat;
            try
            {
              fieldFormat = FieldFormatEnumUtil.NameToValue(attribute3);
            }
            catch
            {
              continue;
            }
            string[] options = (string[]) null;
            FieldAuditSettings auditSettings = (FieldAuditSettings) null;
            switch (fieldFormat)
            {
              case FieldFormat.DROPDOWNLIST:
              case FieldFormat.DROPDOWN:
                options = CustomFieldsInfo.parseOptions(xpathNodeIterator.Current);
                break;
              case FieldFormat.AUDIT:
                auditSettings = CustomFieldsInfo.parseAuditSettings(xpathNodeIterator.Current);
                break;
            }
            string calculation = CustomFieldsInfo.parseCalculation(xpathNodeIterator.Current);
            int maxLength = CustomFieldsInfo.parseInt(xpathNodeIterator.Current.GetAttribute("maxlength", ""), 0);
            this.map[(object) attribute1] = (object) new CustomFieldInfo(attribute1, attribute2, fieldFormat, options, maxLength, auditSettings, calculation);
          }
        }
        catch (Exception ex)
        {
          Tracing.Log(CustomFieldsInfo.sw, nameof (CustomFieldsInfo), TraceLevel.Warning, "Cannot parse Custom Field XML file: " + (object) ex);
        }
        this.xmlData = (string) null;
        this.loadStandardFields();
      }
    }

    private static int parseInt(string value, int defaultValue)
    {
      try
      {
        return int.Parse(value, NumberStyles.Any, (IFormatProvider) null);
      }
      catch
      {
        return defaultValue;
      }
    }

    private static Decimal parseDecimal(string value, Decimal defaultValue)
    {
      try
      {
        return Decimal.Parse(value, NumberStyles.Any, (IFormatProvider) null);
      }
      catch
      {
        return defaultValue;
      }
    }

    private static string parseCalculation(XPathNavigator nav)
    {
      XPathNodeIterator xpathNodeIterator = nav.Select("Calculation");
      if (xpathNodeIterator.Count <= 0)
        return "";
      xpathNodeIterator.MoveNext();
      return xpathNodeIterator.Current.Value;
    }

    private static FieldAuditSettings parseAuditSettings(XPathNavigator nav)
    {
      XPathNodeIterator xpathNodeIterator = nav.Select("Audit");
      if (xpathNodeIterator.Count > 0)
      {
        xpathNodeIterator.MoveNext();
        try
        {
          return new FieldAuditSettings(xpathNodeIterator.Current.GetAttribute("fieldid", ""), (AuditData) Enum.Parse(typeof (AuditData), xpathNodeIterator.Current.GetAttribute("data", ""), true));
        }
        catch
        {
        }
      }
      return (FieldAuditSettings) null;
    }

    private static string[] parseOptions(XPathNavigator nav)
    {
      XPathNodeIterator xpathNodeIterator = nav.Select("Option");
      string[] options = new string[xpathNodeIterator.Count];
      int num = 0;
      while (xpathNodeIterator.MoveNext())
        options[num++] = xpathNodeIterator.Current.Value;
      return options;
    }

    private void loadStandardFields()
    {
      for (int index = 1; index <= 100; ++index)
      {
        string standardFieldId = this.getStandardFieldId(index);
        if (!this.map.Contains((object) standardFieldId))
          this.map[(object) standardFieldId] = (object) new CustomFieldInfo(standardFieldId);
      }
    }

    public CustomFieldInfo GetStandardField(int index)
    {
      return this.GetField(this.getStandardFieldId(index));
    }

    public SortedList GetSortedFields()
    {
      this.parseXml();
      SortedList sortedFields = new SortedList();
      foreach (DictionaryEntry dictionaryEntry in this.map)
        sortedFields.Add((object) dictionaryEntry.Key.ToString(), (object) (CustomFieldInfo) dictionaryEntry.Value);
      return sortedFields;
    }

    public CustomFieldInfo GetField(string fieldID)
    {
      this.parseXml();
      return !this.map.ContainsKey((object) fieldID) ? (CustomFieldInfo) null : (CustomFieldInfo) this.map[(object) fieldID];
    }

    public int GetNonEmptyCount()
    {
      int nonEmptyCount = 0;
      foreach (CustomFieldInfo customFieldInfo in this)
      {
        if (!customFieldInfo.IsEmpty())
          ++nonEmptyCount;
      }
      return nonEmptyCount;
    }

    public override string ToString()
    {
      if (this.xmlData != null)
        return this.xmlData;
      XmlDocument xmlDocument = new XmlDocument();
      XmlElement xmlElement1 = (XmlElement) xmlDocument.AppendChild((XmlNode) xmlDocument.CreateElement("CustomFieldList"));
      foreach (CustomFieldInfo customFieldInfo in (IEnumerable) this.map.Values)
      {
        if (customFieldInfo.IsExtendedField() || !customFieldInfo.IsEmpty())
        {
          XmlElement xmlElement2 = (XmlElement) xmlElement1.AppendChild((XmlNode) xmlDocument.CreateElement("Field"));
          xmlElement2.SetAttribute("id", customFieldInfo.FieldID);
          xmlElement2.SetAttribute("desc", customFieldInfo.Description);
          xmlElement2.SetAttribute("type", FieldFormatEnumUtil.ValueToName(customFieldInfo.Format));
          if (customFieldInfo.MaxLength > 0 && customFieldInfo.Format != FieldFormat.DROPDOWNLIST)
            xmlElement2.SetAttribute("maxlength", customFieldInfo.MaxLength.ToString());
          if (customFieldInfo.IsCalculationAllowed() && (customFieldInfo.Calculation ?? "") != "")
            xmlElement2.AppendChild((XmlNode) xmlDocument.CreateElement("Calculation")).InnerText = customFieldInfo.Calculation;
          if (customFieldInfo.Format == FieldFormat.DROPDOWN || customFieldInfo.Format == FieldFormat.DROPDOWNLIST)
          {
            if (customFieldInfo.Options != null)
            {
              for (int index = 0; index < customFieldInfo.Options.Length; ++index)
                xmlElement2.AppendChild((XmlNode) xmlDocument.CreateElement("Option")).InnerText = customFieldInfo.Options[index];
            }
          }
          else if (customFieldInfo.Format == FieldFormat.AUDIT && customFieldInfo.AuditSettings != null)
          {
            XmlElement xmlElement3 = (XmlElement) xmlElement2.AppendChild((XmlNode) xmlDocument.CreateElement("Audit"));
            xmlElement3.SetAttribute("fieldid", customFieldInfo.AuditSettings.FieldID);
            xmlElement3.SetAttribute("data", customFieldInfo.AuditSettings.AuditData.ToString());
          }
        }
      }
      return xmlDocument.OuterXml;
    }

    public void Add(CustomFieldInfo fieldInfo)
    {
      this.parseXml();
      if (this.map.ContainsKey((object) fieldInfo.FieldID))
        this.map.Remove((object) fieldInfo.FieldID);
      this.map[(object) fieldInfo.FieldID] = (object) fieldInfo;
    }

    public void Remove(CustomFieldInfo fieldInfo)
    {
      this.parseXml();
      if (!fieldInfo.IsExtendedField())
        throw new ArgumentException("Standard custom fields cannot be deleted");
      this.map.Remove((object) fieldInfo.FieldID);
    }

    private string getStandardFieldId(int index) => "CUST" + index.ToString("#00") + "FV";

    protected override void Initialize(BinaryReader br)
    {
      this.map = CollectionsUtil.CreateCaseInsensitiveHashtable();
      while (br.PeekChar() != -1)
      {
        CustomFieldInfo customFieldInfo = new CustomFieldInfo(br);
        this.map[(object) customFieldInfo.FieldID] = (object) customFieldInfo;
      }
    }

    protected override void WriteBytes(BinaryWriter bw)
    {
      this.parseXml();
      foreach (CustomFieldInfo customFieldInfo in (IEnumerable) this.map.Values)
        customFieldInfo.WriteBytes(bw);
    }

    public IEnumerator GetEnumerator()
    {
      this.parseXml();
      return this.map.Values.GetEnumerator();
    }
  }
}
