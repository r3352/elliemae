// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.FieldValidationUtil
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public static class FieldValidationUtil
  {
    public static bool ValidateFormat(
      string fieldID,
      string text,
      FieldFormat format,
      Sessions.Session session)
    {
      FieldDefinition fieldDefinition = !fieldID.ToLower().StartsWith("cx.") ? EncompassFields.GetField(fieldID) : EncompassFields.GetField(fieldID, session.LoanManager.GetFieldSettings());
      if (text.Trim() == "")
      {
        int num = (int) MessageBox.Show("Value cannot be empty.", "Encompass");
        return false;
      }
      if (fieldDefinition.ReportingDatabaseColumnType == ReportingDatabaseColumnType.Numeric && !FieldValidationUtil.IsNumericOnly(text))
      {
        int num = (int) MessageBox.Show("Please enter numeric value for field.", "Encompass");
        return false;
      }
      if (format == FieldFormat.SSN && !FieldValidationUtil.IsSSNType(text) || format == FieldFormat.PHONE && !FieldValidationUtil.IsPhoneNumberType(text))
        return false;
      try
      {
        if (fieldDefinition.ValidateFormat(text) != "")
          return true;
        int num = (int) MessageBox.Show("Its not a valid format", "Encompass");
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Its not a valid format :" + ex.Message, "Encompass");
      }
      return false;
    }

    private static bool IsSSNType(string text)
    {
      if (text.All<char>((Func<char, bool>) (c => c >= '0' && c <= '9' || c == '-')))
      {
        if (new Regex("^\\d{3}-\\d{2}-\\d{4}$").Match(text).Success)
          return true;
        int num = (int) MessageBox.Show("Please enter SSN in XXX-XX-XXXX format", "Encompass");
      }
      else
      {
        int num1 = (int) MessageBox.Show("Please enter valid SSN number.", "Encompass");
      }
      return false;
    }

    private static bool IsPhoneNumberType(string text)
    {
      if (Utils.ValidatePhoneNumber(text))
        return true;
      int num = (int) MessageBox.Show("Please enter valid Phone number format.", "Encompass");
      return false;
    }

    public static bool IsNumericOnly(string str)
    {
      return str.All<char>((Func<char, bool>) (c => c >= '0' && c <= '9' || c == '.'));
    }

    public static bool ValidateCustomFields(string value, FieldFormat format)
    {
      try
      {
        Utils.ConvertToLoanInternalValue(value, format);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Its not a valid format :" + ex.Message, "Encompass");
        return false;
      }
      return true;
    }

    public static bool validateRange(
      string fieldID,
      string minCond,
      string maxCond,
      FieldFormat format,
      Sessions.Session session)
    {
      IComparable comparable1 = (IComparable) null;
      IComparable comparable2 = (IComparable) null;
      FieldDefinition fieldDefinition = !fieldID.ToLower().StartsWith("cx.") ? EncompassFields.GetField(fieldID) : EncompassFields.GetField(fieldID, session.LoanManager.GetFieldSettings());
      if (minCond.Trim() == "" && maxCond.Trim() == "")
      {
        int num = (int) MessageBox.Show("You must specify and minimum and/or maximum value for the activation range.", "Encompass");
        return false;
      }
      if (fieldDefinition.ReportingDatabaseColumnType == ReportingDatabaseColumnType.Numeric && (!FieldValidationUtil.IsNumericOnly(minCond) || !FieldValidationUtil.IsNumericOnly(maxCond)))
      {
        int num = (int) MessageBox.Show("Please enter numeric value for field.", "Encompass");
        return false;
      }
      try
      {
        if (minCond.Trim() != "")
          comparable1 = (IComparable) Utils.ConvertToNativeValue(minCond.Trim(), fieldDefinition.Format, true);
      }
      catch
      {
        int num = (int) MessageBox.Show("The minimum value specified for the activation range is invalid for this field type.", "Encompass");
        return false;
      }
      try
      {
        if (maxCond.Trim() != "")
          comparable2 = (IComparable) Utils.ConvertToNativeValue(maxCond.Trim(), fieldDefinition.Format, true);
      }
      catch
      {
        int num = (int) MessageBox.Show("The maximum value specified for the activation range is invalid for this field type.", "Encompass");
        return false;
      }
      if (comparable1 == null || comparable2 == null || comparable1.CompareTo((object) comparable2) <= 0)
        return true;
      int num1 = (int) MessageBox.Show("The minimum activation value must be less than or equal to the maximum.", "Encompass");
      return false;
    }

    public static bool HasOptions(string fieldID, Sessions.Session session)
    {
      FieldDefinition fieldDefinition = !fieldID.ToLower().StartsWith("cx.") ? EncompassFields.GetField(fieldID) : EncompassFields.GetField(fieldID, session.LoanManager.GetFieldSettings());
      return fieldDefinition != null && fieldDefinition.HasOptions();
    }
  }
}
