// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.LoanDuplicationDefaultTemplate
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class LoanDuplicationDefaultTemplate : BinaryConvertibleObject
  {
    private List<string[]> borFields = new List<string[]>();
    private List<string[]> borEmployerFields = new List<string[]>();
    private List<string[]> borEmployerFieldsUrla2020 = new List<string[]>();
    private List<string[]> borAddress = new List<string[]>();
    private List<string[]> borAddressUrla2020 = new List<string[]>();
    private List<string[]> borPriorAddress = new List<string[]>();
    private List<string[]> borPriorAddressUrla2020 = new List<string[]>();
    private List<string[]> cobFields = new List<string[]>();
    private List<string[]> cobEmployerFields = new List<string[]>();
    private List<string[]> cobEmployerFieldsUrla2020 = new List<string[]>();
    private List<string[]> cobAddress = new List<string[]>();
    private List<string[]> cobAddressUrla2020 = new List<string[]>();
    private List<string[]> cobPriorAddress = new List<string[]>();
    private List<string[]> cobPriorAddressUrla2020 = new List<string[]>();
    private List<string[]> propertyAddress = new List<string[]>();
    private List<string[]> propertyAddressUrla2020 = new List<string[]>();

    public LoanDuplicationDefaultTemplate(XmlSerializationInfo info)
    {
      if (info == null)
        return;
      foreach (string name in info)
      {
        XmlStringTable xmlStringTable = (XmlStringTable) info.GetValue(name, typeof (XmlStringTable));
        string empty = string.Empty;
        foreach (string key in xmlStringTable.Keys)
        {
          string str = xmlStringTable[key].ToString();
          switch (name)
          {
            case "Borrower Employer Information":
              this.borEmployerFields.Add(new string[2]
              {
                key,
                str
              });
              continue;
            case "Borrower Employer Information URLA2020":
              this.borEmployerFieldsUrla2020.Add(new string[2]
              {
                key,
                str
              });
              continue;
            case "Borrower Information":
              this.borFields.Add(new string[2]{ key, str });
              continue;
            case "Borrower Present Address":
              this.borAddress.Add(new string[2]{ key, str });
              continue;
            case "Borrower Present Address URLA2020":
              this.borAddressUrla2020.Add(new string[2]
              {
                key,
                str
              });
              continue;
            case "Borrower Prior Address":
              this.borPriorAddress.Add(new string[2]
              {
                key,
                str
              });
              continue;
            case "Borrower Prior Address URLA2020":
              this.borPriorAddressUrla2020.Add(new string[2]
              {
                key,
                str
              });
              continue;
            case "Co-Borrower Employer Information":
              this.cobEmployerFields.Add(new string[2]
              {
                key,
                str
              });
              continue;
            case "Co-Borrower Employer Information URLA2020":
              this.cobEmployerFieldsUrla2020.Add(new string[2]
              {
                key,
                str
              });
              continue;
            case "Co-Borrower Information":
              this.cobFields.Add(new string[2]{ key, str });
              continue;
            case "Co-Borrower Present Address":
              this.cobAddress.Add(new string[2]{ key, str });
              continue;
            case "Co-Borrower Present Address URLA2020":
              this.cobAddressUrla2020.Add(new string[2]
              {
                key,
                str
              });
              continue;
            case "Co-Borrower Prior Address":
              this.cobPriorAddress.Add(new string[2]
              {
                key,
                str
              });
              continue;
            case "Co-Borrower Prior Address URLA2020":
              this.cobPriorAddressUrla2020.Add(new string[2]
              {
                key,
                str
              });
              continue;
            case "Property Address":
              this.propertyAddress.Add(new string[2]
              {
                key,
                str
              });
              continue;
            case "Property Address URLA2020":
              this.propertyAddressUrla2020.Add(new string[2]
              {
                key,
                str
              });
              continue;
            default:
              continue;
          }
        }
      }
    }

    public string[] GetFieldIDs(
      LoanDuplicationDefaultTemplate.FieldTypes fieldType)
    {
      return this.GetFieldIDs(fieldType, false);
    }

    public string[] GetFieldIDs(
      LoanDuplicationDefaultTemplate.FieldTypes fieldType,
      bool isURLA2020)
    {
      List<string[]> fields = this.GetFields(fieldType, isURLA2020);
      if (fields == null)
        return (string[]) null;
      string[] fieldIds = new string[fields.Count];
      for (int index = 0; index < fields.Count; ++index)
        fieldIds[index] = fields[index][0];
      return fieldIds;
    }

    public List<string[]> GetFields(
      LoanDuplicationDefaultTemplate.FieldTypes fieldType)
    {
      return this.GetFields(fieldType, false);
    }

    public List<string[]> GetFields(
      LoanDuplicationDefaultTemplate.FieldTypes fieldType,
      bool isURLA2020)
    {
      switch (fieldType)
      {
        case LoanDuplicationDefaultTemplate.FieldTypes.BorrowerInformation:
          return this.borFields;
        case LoanDuplicationDefaultTemplate.FieldTypes.BorrowerEmployerInformation:
          return isURLA2020 ? this.borEmployerFieldsUrla2020 : this.borEmployerFields;
        case LoanDuplicationDefaultTemplate.FieldTypes.BorrowerPresentAddress:
          return isURLA2020 ? this.borAddressUrla2020 : this.borAddress;
        case LoanDuplicationDefaultTemplate.FieldTypes.BorrowerPriorAddress:
          return isURLA2020 ? this.borPriorAddressUrla2020 : this.borPriorAddress;
        case LoanDuplicationDefaultTemplate.FieldTypes.CoBorrowerInformation:
          return this.cobFields;
        case LoanDuplicationDefaultTemplate.FieldTypes.CoBorrowerEmployerInformation:
          return isURLA2020 ? this.cobEmployerFieldsUrla2020 : this.cobEmployerFields;
        case LoanDuplicationDefaultTemplate.FieldTypes.CoBorrowerPresentAddress:
          return isURLA2020 ? this.cobAddressUrla2020 : this.cobAddress;
        case LoanDuplicationDefaultTemplate.FieldTypes.CoBorrowerPriorAddress:
          return isURLA2020 ? this.cobPriorAddressUrla2020 : this.cobPriorAddress;
        case LoanDuplicationDefaultTemplate.FieldTypes.PropertyAddress:
          return isURLA2020 ? this.propertyAddressUrla2020 : this.propertyAddress;
        case LoanDuplicationDefaultTemplate.FieldTypes.BorrowerPresentAddressUrla2020:
          return this.borAddressUrla2020;
        case LoanDuplicationDefaultTemplate.FieldTypes.CoBorrowerPresentAddressUrla2020:
          return this.cobAddressUrla2020;
        case LoanDuplicationDefaultTemplate.FieldTypes.BorrowerEmployerInformationUrla2020:
          return this.borEmployerFieldsUrla2020;
        case LoanDuplicationDefaultTemplate.FieldTypes.CoBorrowerEmployerInformationUrla2020:
          return this.cobEmployerFieldsUrla2020;
        case LoanDuplicationDefaultTemplate.FieldTypes.BorrowerPriorAddressUrla2020:
          return this.borPriorAddressUrla2020;
        case LoanDuplicationDefaultTemplate.FieldTypes.CoBorrowerPriorAddressUrla2020:
          return this.cobPriorAddressUrla2020;
        case LoanDuplicationDefaultTemplate.FieldTypes.PropertyAddressUrla2020:
          return this.propertyAddressUrla2020;
        default:
          return (List<string[]>) null;
      }
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
    }

    public static explicit operator LoanDuplicationDefaultTemplate(BinaryObject obj)
    {
      return (LoanDuplicationDefaultTemplate) BinaryConvertibleObject.Parse(obj, typeof (LoanDuplicationDefaultTemplate));
    }

    public enum FieldTypes
    {
      BorrowerInformation,
      BorrowerEmployerInformation,
      BorrowerPresentAddress,
      BorrowerPriorAddress,
      CoBorrowerInformation,
      CoBorrowerEmployerInformation,
      CoBorrowerPresentAddress,
      CoBorrowerPriorAddress,
      PropertyAddress,
      BorrowerPresentAddressUrla2020,
      CoBorrowerPresentAddressUrla2020,
      BorrowerEmployerInformationUrla2020,
      CoBorrowerEmployerInformationUrla2020,
      BorrowerPriorAddressUrla2020,
      CoBorrowerPriorAddressUrla2020,
      PropertyAddressUrla2020,
    }
  }
}
