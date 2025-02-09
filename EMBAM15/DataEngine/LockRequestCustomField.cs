// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.LockRequestCustomField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class LockRequestCustomField : PersistentField
  {
    public const string FieldPrefix = "LR.";
    private FieldDefinition baseField;
    private string rolodex;

    public LockRequestCustomField(FieldDefinition baseField)
      : base("LR." + baseField.FieldID)
    {
      this.baseField = baseField;
    }

    public override FieldFormat Format
    {
      get => this.baseField.Format;
      set => this.baseField.Format = value;
    }

    public override bool AllowInReportingDatabase => this.baseField.AllowInReportingDatabase;

    public override bool AllowEdit => this.baseField.AllowEdit;

    public override ReportingDatabaseColumnType ReportingDatabaseColumnType
    {
      get => this.baseField.ReportingDatabaseColumnType;
    }

    public override int ReportingDatabaseColumnSize => this.baseField.ReportingDatabaseColumnSize;

    public override string Description => "Lock Request - " + this.baseField.Description;

    public override int MaxLength => this.baseField.MaxLength;

    public override FieldOptionCollection Options => this.baseField.Options;

    internal override string XPath
    {
      get => "EllieMae/CUSTOM_FIELDS/FIELD[@FieldID='" + this.FieldID + "']/@FieldValue";
    }

    public override string Rolodex
    {
      get => this.rolodex;
      set => this.rolodex = value;
    }

    public override string GetValue(LoanData loan) => this.GetValue(loan, this.FieldID);

    public override string GetValue(LoanData loan, string id) => loan.GetField(id);

    public override void SetValue(LoanData loan, string value)
    {
      loan.SetField(this.FieldID, value);
    }

    public static bool IsLockRequestCustomField(string fieldId)
    {
      return fieldId.StartsWith("LR.", StringComparison.CurrentCultureIgnoreCase);
    }

    public static string GetBaseFieldIDForCustomField(string lrCustomFieldId)
    {
      return LockRequestCustomField.IsLockRequestCustomField(lrCustomFieldId) ? lrCustomFieldId.Substring(3) : throw new ArgumentException("The specified field ID '" + lrCustomFieldId + "' is not in the proper format");
    }

    public static string GenerateCustomFieldID(string baseFieldId) => "LR." + baseFieldId;
  }
}
