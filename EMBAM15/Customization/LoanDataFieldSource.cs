// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Customization.LoanDataFieldSource
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;

#nullable disable
namespace EllieMae.EMLite.Customization
{
  public class LoanDataFieldSource(LoanData loan, UserInfo currentUser, bool readOnly) : 
    LoanDataSource(loan, currentUser, readOnly),
    IFieldSource,
    IReadOnlyFieldSource
  {
    public object this[string fieldId]
    {
      get
      {
        if (fieldId.StartsWith("#"))
          return this.GetNumeric(fieldId.Substring(1));
        if (fieldId.StartsWith("+"))
          return this.GetFormatted(fieldId.Substring(1));
        if (fieldId.StartsWith("-"))
          return this.GetUnformatted(fieldId.Substring(1));
        return fieldId.StartsWith("@") ? this.GetDate(fieldId.Substring(1)) : this.GetNative(fieldId);
      }
      set
      {
        this.EnsureWritable();
        if (fieldId == "2025")
          return;
        string str = (string) null;
        if (fieldId.IndexOf("#") > -1)
        {
          int num = Utils.ParseInt((object) fieldId.Substring(fieldId.IndexOf("#") + 1), 0);
          BorrowerPair[] borrowerPairs = this.Loan.GetBorrowerPairs();
          if (num <= borrowerPairs.Length && this.Loan.CurrentBorrowerPair.Id != borrowerPairs[num <= 0 ? 0 : num - 1].Id)
          {
            str = this.Loan.CurrentBorrowerPair.Id;
            this.Loan.SetBorrowerPair(borrowerPairs[num <= 0 ? 0 : num - 1]);
          }
          else if (num > borrowerPairs.Length)
            return;
          fieldId = fieldId.Substring(0, fieldId.IndexOf("#"));
        }
        FieldFormat format = this.Loan.GetFormat(fieldId);
        DateTime returnValue;
        string val = format != FieldFormat.DATETIME || value == null || !Utils.TryParseDate(value, out returnValue, true) ? this.Loan.FormatValue(string.Concat(value), format) : (returnValue.Kind != DateTimeKind.Utc ? this.Loan.FormatValue(string.Concat(value), format) : Utils.DateTimeToUTCString(returnValue));
        if (val != this.Loan.GetField(fieldId))
          this.Loan.SetField(fieldId, val);
        if (str == null)
          return;
        BorrowerPair[] borrowerPairs1 = this.Loan.GetBorrowerPairs();
        for (int index = 0; index < borrowerPairs1.Length; ++index)
        {
          if (borrowerPairs1[index].Id == str)
          {
            this.Loan.SetBorrowerPair(borrowerPairs1[index]);
            break;
          }
        }
      }
    }

    public object GetNumeric(string fieldId)
    {
      string simpleField = this.Loan.GetSimpleField(fieldId);
      return this.Loan.GetFormat(fieldId) == FieldFormat.INTEGER ? (object) Utils.ParseInt((object) simpleField, 0) : (object) Utils.ParseDecimal((object) simpleField, 0M);
    }

    public object GetDate(string fieldId)
    {
      string simpleField = this.Loan.GetSimpleField(fieldId);
      return this.Loan.GetFormat(fieldId) == FieldFormat.MONTHDAY ? (object) Utils.ParseMonthDay((object) simpleField, false) : (object) Utils.ParseDate((object) simpleField, false);
    }

    public object GetNative(string fieldId)
    {
      return Utils.ConvertToNativeValue(this.Loan.GetSimpleField(fieldId), this.Loan.GetFormat(fieldId), (object) "");
    }

    public object GetFormatted(string fieldId) => (object) this.Loan.GetField(fieldId);

    public object GetUnformatted(string fieldId)
    {
      return (object) Utils.UnformatValue(this.Loan.GetSimpleField(fieldId), this.Loan.GetFormat(fieldId));
    }

    public object XType(object value, string fieldId)
    {
      FieldFormat format = !((fieldId ?? "") == "") ? this.Loan.GetFormat(fieldId) : throw new ArgumentException(nameof (fieldId));
      return Utils.ConvertToNativeValue(string.Concat(value), format, (object) null);
    }

    public void DisableRules()
    {
      this.EnsureWritable();
      this.Loan.Validator.Enabled = false;
    }

    public void EnableRules()
    {
      this.EnsureWritable();
      this.Loan.Validator.Enabled = true;
    }

    public void Lock(string fieldId)
    {
      this.EnsureWritable();
      this.Loan.AddLock(fieldId);
    }

    public void Unlock(string fieldId)
    {
      this.EnsureWritable();
      this.Loan.RemoveLock(fieldId);
    }

    public void Recalculate()
    {
      this.EnsureWritable();
      this.Loan.Calculator.CalculateAll();
    }

    public void ExecuteCalculation(string calcName)
    {
      this.EnsureWritable();
      this.Loan.Calculator.FormCalculation(calcName, (string) null, (string) null);
    }

    public void SetDocumentOverride(string fieldId, bool overide)
    {
      this.EnsureWritable();
      this.Loan.SetDocFieldUserOverride(fieldId, overide);
    }
  }
}
