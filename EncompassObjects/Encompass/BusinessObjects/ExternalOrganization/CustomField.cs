// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.CustomField
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  /// <summary>
  /// Represents a single CustomField for an
  /// <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization">ExternalOrganization</see>
  /// </summary>
  public class CustomField
  {
    private ContactCustomField contactCustomField;
    private ExternalOriginatorManagementData extOrg;
    private string[] options;
    private FieldFormat type;

    internal CustomField(
      ContactCustomField contactCustomField,
      string fieldName,
      ExternalOriginatorManagementData extOrg,
      string[] options,
      FieldFormat type)
    {
      this.contactCustomField = contactCustomField;
      this.FieldName = fieldName;
      this.extOrg = extOrg;
      this.options = options;
      this.type = type;
    }

    /// <summary>
    /// Returns all possible values for a drop down box field.
    /// </summary>
    /// <remarks>The array will be empty unless the field is a drop down field.</remarks>
    public string[] FieldOptions => this.options;

    /// <summary>Gets the name of the custom field.</summary>
    public string FieldName { get; private set; }

    /// <summary>Gets and sets value of the custom field.</summary>
    /// <exception cref="T:System.Exception">Throws exception if setting a value when using
    /// parent info.</exception>
    /// <exception cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.FieldValidationException">Throws excpetion if the new value
    /// isn't valid for the field type.</exception>
    public string FieldValue
    {
      get => this.contactCustomField.FieldValue;
      set
      {
        if (this.extOrg.InheritCustomFields)
          throw new Exception("Field cannot be set when inheriting from parent.");
        this.contactCustomField.FieldValue = this.ValidateInput(value);
      }
    }

    private string ValidateInput(string value)
    {
      value = value.Trim();
      if (string.IsNullOrEmpty(value))
        return value;
      switch (this.type)
      {
        case FieldFormat.STRING:
          return value;
        case FieldFormat.YN:
          string upper = value.ToUpper();
          if (upper != "Y" && upper != "N")
            throw new FieldValidationException(this.FieldName, value, "Value is not a valid Yes/No value. Must be 'Y' or 'N'");
          return value;
        case FieldFormat.X:
          return !(value.ToUpper() != "X") ? value : throw new FieldValidationException(this.FieldName, value, "Value is not a valid Check Box value. Must be 'X' or empty.");
        case FieldFormat.ZIPCODE:
          string[] strArray1 = value.Split(new string[1]
          {
            "-"
          }, StringSplitOptions.RemoveEmptyEntries);
          bool flag1 = int.TryParse(strArray1[0], out int _) && strArray1[0].Length == 5;
          bool flag2 = strArray1.Length == 1 || int.TryParse(strArray1[1], out int _) && strArray1[1].Length == 4;
          if (!flag1 || !flag2 || strArray1.Length > 2)
            throw new FieldValidationException(this.FieldName, value, "Value is not a valid zip code. Format is XXXXX or XXXXX-XXXX");
          return value;
        case FieldFormat.STATE:
          if (value.Where<char>((Func<char, int, bool>) ((t, idx) => !char.IsLetter(t) || idx > 1)).Any<char>())
            throw new FieldValidationException(this.FieldName, value, "Value is not a valid state.");
          return value;
        case FieldFormat.PHONE:
          string[] strArray2 = value.Split(new string[1]
          {
            "-"
          }, StringSplitOptions.RemoveEmptyEntries);
          long result1;
          bool flag3 = strArray2.Length == 1 && strArray2[0].Length >= 10 && strArray2[0].Length <= 14 && long.TryParse(strArray2[0], out result1);
          if (!flag3 && strArray2.Length == 3 && strArray2[0].Length == 3 && strArray2[1].Length == 3 && (strArray2[2].Length == 4 || strArray2[2].Length == 9) && long.TryParse(strArray2[0], out result1) && long.TryParse(strArray2[1], out result1))
          {
            string[] strArray3 = strArray2[2].Split(new string[1]
            {
              " "
            }, StringSplitOptions.RemoveEmptyEntries);
            if (strArray3.Length == 1 && long.TryParse(strArray3[0], out result1))
              flag3 = true;
            if (strArray3.Length == 2 && long.TryParse(strArray3[0], out result1) && long.TryParse(strArray3[1], out result1))
              flag3 = true;
          }
          if (!flag3)
            throw new FieldValidationException(this.FieldName, value, "Value is not a valid phone number. Format is XXXXXXXXXX or XXX-XXX-XXXX or XXXXXXXXXXXXXX or XXX-XXX-XXXX XXXX");
          bool needsUpdate1 = false;
          return Utils.FormatInput(value, this.type, ref needsUpdate1);
        case FieldFormat.SSN:
          string[] strArray4 = value.Split(new string[1]
          {
            "-"
          }, StringSplitOptions.RemoveEmptyEntries);
          int result2;
          bool flag4 = strArray4.Length == 1 && strArray4[0].Length == 9 && int.TryParse(strArray4[0], out result2);
          if (!flag4)
            flag4 = strArray4.Length == 3 && strArray4[0].Length == 3 && strArray4[1].Length == 2 && strArray4[2].Length == 4 && int.TryParse(strArray4[0], out result2) && int.TryParse(strArray4[1], out result2) && int.TryParse(strArray4[2], out result2);
          if (!flag4)
            throw new FieldValidationException(this.FieldName, value, "Value is not a valid SSN. Format is XXXXXXXXX or XXX-XX-XXXX");
          bool needsUpdate2 = false;
          return Utils.FormatInput(value, this.type, ref needsUpdate2);
        case FieldFormat.INTEGER:
          return int.TryParse(value, out int _) ? value : throw new FieldValidationException(this.FieldName, value, "Value is not a valid integer.");
        case FieldFormat.DECIMAL_1:
        case FieldFormat.DECIMAL_2:
        case FieldFormat.DECIMAL_3:
        case FieldFormat.DECIMAL_4:
        case FieldFormat.DECIMAL_6:
        case FieldFormat.DECIMAL_5:
        case FieldFormat.DECIMAL_7:
        case FieldFormat.DECIMAL:
        case FieldFormat.DECIMAL_10:
          string[] strArray5 = this.type.ToString().Split(new string[1]
          {
            "_"
          }, StringSplitOptions.None);
          int result3;
          if (!int.TryParse(strArray5.Length == 1 ? string.Empty : strArray5[1], out result3))
            result3 = -1;
          foreach (char c in value)
          {
            if (!char.IsDigit(c) && c != '.')
              throw new FieldValidationException(this.FieldName, value, "Value is not a valid decimal.");
          }
          int num = value.IndexOf('.');
          if (num > -1 && result3 > -1 && value.Substring(num + 1).Length > result3)
            throw new FieldValidationException(this.FieldName, value, "Decimal value has too many decimal places. Max decimal places is " + result3.ToString());
          return value;
        case FieldFormat.DATE:
          DateTime result4;
          if (!DateTime.TryParse(value, out result4))
            throw new FieldValidationException(this.FieldName, value, "Value is not a valid Date.");
          return result4.ToShortDateString();
        case FieldFormat.MONTHDAY:
          string str = value + "/2012";
          return DateTime.TryParse(value, out DateTime _) ? value : throw new FieldValidationException(this.FieldName, value, "Value is not a valid Month/Day.");
        case FieldFormat.DROPDOWNLIST:
        case FieldFormat.DROPDOWN:
          return ((IEnumerable<string>) this.options).Contains<string>(value) ? value : throw new FieldValidationException(this.FieldName, value, "Value is not in list of FieldOptions.");
        default:
          throw new Exception("Invalid field type.");
      }
    }
  }
}
