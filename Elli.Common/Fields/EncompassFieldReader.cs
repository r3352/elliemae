// Decompiled with JetBrains decompiler
// Type: Elli.Common.Fields.EncompassFieldReader
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

#nullable disable
namespace Elli.Common.Fields
{
  public class EncompassFieldReader
  {
    public static void Read(Stream stream, IDictionary<string, EncompassField> fields)
    {
      foreach (XElement element in XDocument.Load(stream).Root.Elements((XName) "Field"))
      {
        EncompassField encompassField = EncompassFieldReader.ReadField(element);
        if (!encompassField.ID.StartsWith("LP1") || !(encompassField.Category.ToLower() != "none"))
        {
          if (fields.ContainsKey(encompassField.ID))
            throw new Exception("Error processing EncompassFields.xml file.  Field ID exists multiple times in the file:  " + encompassField.ID);
          fields.Add(encompassField.ID, encompassField);
        }
      }
    }

    public static IList<EncompassField> Read(string uriFieldsDocument)
    {
      List<EncompassField> encompassFieldList = new List<EncompassField>();
      foreach (XElement element in XDocument.Load(uriFieldsDocument).Root.Elements((XName) "Field"))
      {
        EncompassField encompassField = EncompassFieldReader.ReadField(element);
        encompassFieldList.Add(encompassField);
      }
      return (IList<EncompassField>) encompassFieldList;
    }

    private static EncompassField ReadField(XElement fieldElement)
    {
      EncompassField encompassField = new EncompassField();
      encompassField.AllowEdit = XmlUtil.ReadNumericBool(fieldElement, "AllowEdit");
      encompassField.AllowReporting = XmlUtil.ReadNumericBool(fieldElement, "AllowReporting");
      encompassField.Category = XmlUtil.ReadString(fieldElement, "Category");
      encompassField.DbField = XmlUtil.ReadString(fieldElement, "DbField");
      encompassField.Format = XmlUtil.ReadString(fieldElement, "Format");
      encompassField.ID = XmlUtil.ReadString(fieldElement, "ID");
      encompassField.MultiInstance = XmlUtil.ReadNumericBool(fieldElement, "MultiInstance");
      encompassField.MaxLength = XmlUtil.ReadInt(fieldElement, "MaxLength");
      XElement xelement1 = fieldElement.Element((XName) "Description");
      if (xelement1 != null)
        encompassField.Description = xelement1.Value;
      XElement xelement2 = fieldElement.Element((XName) "XPath");
      if (xelement2 != null)
        encompassField.XPath = xelement2.Value;
      XElement element1 = fieldElement.Element((XName) "Sql");
      if (element1 != null)
        encompassField.DataType = XmlUtil.ReadString(element1, "DataType");
      XElement xelement3 = fieldElement.Element((XName) "ModelPath");
      if (xelement3 != null)
        encompassField.ModelPath = xelement3.Value;
      XElement element2 = fieldElement.Element((XName) "Options");
      if (element2 != null)
      {
        encompassField.OptionsRequired = XmlUtil.ReadNumericBool(element2, "Required");
        foreach (XElement element3 in element2.Elements((XName) "Option"))
          encompassField.Options.Add(new EncompassFieldOption()
          {
            Description = element3.Value,
            Value = XmlUtil.ReadString(element3, "Value"),
            BooleanValue = XmlUtil.ReadNullableBool(element3, "BooleanValue")
          });
      }
      return encompassField;
    }
  }
}
