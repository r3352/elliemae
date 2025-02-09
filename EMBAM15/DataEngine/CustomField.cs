// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.CustomField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class CustomField : PersistentField
  {
    private CustomFieldInfo fieldInfo;
    private FieldOptionCollection options;
    private string rolodex;

    public CustomField(CustomFieldInfo fieldInfo)
      : base(fieldInfo.FieldID)
    {
      this.fieldInfo = fieldInfo;
    }

    public override FieldFormat Format
    {
      get => this.fieldInfo.Format;
      set => this.fieldInfo.Format = value;
    }

    public override bool AllowInReportingDatabase => true;

    public override bool AllowEdit => !this.fieldInfo.IsAuditField();

    public override ReportingDatabaseColumnType ReportingDatabaseColumnType
    {
      get
      {
        return this.fieldInfo.Format == FieldFormat.AUDIT && this.fieldInfo.AuditSettings.AuditData == AuditData.Timestamp ? ReportingDatabaseColumnType.Date : base.ReportingDatabaseColumnType;
      }
    }

    public override string Description => this.fieldInfo.Description;

    public override int MaxLength
    {
      get
      {
        switch (this.Format)
        {
          case FieldFormat.STRING:
          case FieldFormat.DROPDOWN:
            return this.fieldInfo.MaxLength;
          default:
            return base.MaxLength;
        }
      }
    }

    public override FieldOptionCollection Options
    {
      get
      {
        if (this.options == null)
          this.options = new FieldOptionCollection(this.fieldInfo);
        return this.options;
      }
    }

    internal override string XPath
    {
      get => "EllieMae/CUSTOM_FIELDS/FIELD[@FieldID='" + this.fieldInfo.FieldID + "']/@FieldValue";
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
  }
}
