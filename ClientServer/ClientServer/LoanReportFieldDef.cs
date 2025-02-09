// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.LoanReportFieldDef
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.DataEngine;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public class LoanReportFieldDef : ReportFieldDef
  {
    public const string ReportingDatabaseCategory = "Database�";
    public const string AuditTrailCategory = "Audit Trail�";
    public const string LoanTeamMemberFieldSource = "LoanTeamMember�";
    private int borrowerPairIndex = -1;

    public LoanReportFieldDef(
      string category,
      string fieldId,
      string name,
      string description,
      FieldFormat fieldType,
      string criFieldName)
      : base(category, fieldId, name, description, fieldType, criFieldName)
    {
      FieldPairInfo fieldPairInfo = FieldPairParser.ParseFieldPairInfo(fieldId);
      FieldDefinition field = EncompassFields.GetField(fieldPairInfo.FieldID);
      if (field == null)
        return;
      this.fieldDefinition = field;
      if (field.Category != FieldCategory.Borrower && field.Category != FieldCategory.Coborrower)
        return;
      this.borrowerPairIndex = fieldPairInfo.PairIndex;
    }

    private LoanReportFieldDef(string category, FieldDefinition fieldDef)
      : base(category, fieldDef)
    {
    }

    public LoanReportFieldDef(FieldDefinition fieldDef)
      : base("Loan", fieldDef)
    {
    }

    public LoanReportFieldDef(
      string category,
      FieldDefinition fieldDef,
      string dbName,
      FieldDisplayType displayType,
      string[] relatedFields)
      : base(category, fieldDef, dbName, displayType, relatedFields)
    {
    }

    public LoanReportFieldDef(
      string category,
      FieldDefinition fieldDef,
      string dbName,
      FieldDisplayType displayType)
      : base(category, fieldDef, dbName, displayType, new string[0])
    {
    }

    public LoanReportFieldDef(string category, XmlElement fieldElement)
      : base(category, fieldElement)
    {
    }

    public LoanReportFieldDef(CustomFieldInfo fieldDef)
      : this((FieldDefinition) new CustomField(fieldDef))
    {
      this.category = "Custom";
      if (!fieldDef.IsAuditField())
        return;
      this.fieldType = fieldDef.IsDateValued() ? EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsDate : EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsString;
    }

    public LoanReportFieldDef(FieldDefinition fieldDef, LoanXDBField xdbField)
      : this(fieldDef)
    {
      this.category = "Database";
      this.name = xdbField.Description;
      this.description = xdbField.Description;
      this.dbName = xdbField.ReportingCriterionName;
      if (!fieldDef.RequiresBorrowerPredicate)
        return;
      this.borrowerPairIndex = xdbField.ComortgagorPair;
      if (this.borrowerPairIndex == 0)
        this.borrowerPairIndex = 1;
      if (this.borrowerPairIndex <= 1)
        return;
      this.description = this.description + " - " + FieldPairInfo.GetPairDescription(this.borrowerPairIndex);
    }

    public LoanReportFieldDef(CustomFieldInfo fieldDef, LoanXDBField xdbField)
      : this(fieldDef)
    {
      this.category = "Database";
      this.name = xdbField.Description;
      this.description = xdbField.Description;
      this.dbName = xdbField.ReportingCriterionName;
    }

    public LoanReportFieldDef(LoanXDBField xdbField)
      : base("Database", (FieldDefinition) ReportingFieldDefinition.FromXDBField(xdbField), xdbField.ReportingCriterionName)
    {
      this.borrowerPairIndex = xdbField.ComortgagorPair;
    }

    public LoanReportFieldDef(LoanXDBAuditField auditField)
      : base("Audit Trail", (FieldDefinition) ReportingFieldDefinition.FromAuditField(auditField), auditField.ReportingCriterionName)
    {
      this.borrowerPairIndex = auditField.DatabaseField.ComortgagorPair;
    }

    public static LoanReportFieldDef RateLockFieldSelector()
    {
      return (LoanReportFieldDef) new LoanReportFieldDef.RateLockFieldSelectorDef();
    }

    public static LoanReportFieldDef InterimFieldSelector()
    {
      return (LoanReportFieldDef) new LoanReportFieldDef.InterimFieldSelectorDef();
    }

    public static LoanReportFieldDef GFEDisclosedFieldSelector()
    {
      return (LoanReportFieldDef) new LoanReportFieldDef.GFEDisclosureFieldSelectorDef();
    }

    public static LoanReportFieldDef LEDisclosedFieldSelector()
    {
      return (LoanReportFieldDef) new LoanReportFieldDef.LEDisclosureFieldSelectorDef();
    }

    public static LoanReportFieldDef AuditTrailFieldSelector()
    {
      return (LoanReportFieldDef) new LoanReportFieldDef.AuditTrailFieldSelectorDef();
    }

    public int BorrowerPair => this.borrowerPairIndex;

    public override string FieldID
    {
      get
      {
        return this.borrowerPairIndex <= 1 ? base.FieldID : FieldPairParser.GetFieldIDForBorrowerPair(base.FieldID, this.borrowerPairIndex);
      }
    }

    public override FilterDataSource DataSource => FilterDataSource.Loan;

    public ReportingDatabaseColumnType ReportingDatabaseColumnType
    {
      get
      {
        switch (this.fieldType)
        {
          case EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsString:
          case EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsPhone:
          case EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsOptionList:
          case EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsYesNo:
          case EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsCheckbox:
            return ReportingDatabaseColumnType.Text;
          case EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsNumeric:
            return ReportingDatabaseColumnType.Numeric;
          case EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsDate:
          case EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsMonthDay:
            return ReportingDatabaseColumnType.Date;
          case EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsDateTime:
            return ReportingDatabaseColumnType.DateTime;
          default:
            return ReportingDatabaseColumnType.Unknown;
        }
      }
    }

    public virtual bool IsLoanDataField => !(this.fieldDefinition is ReportingFieldDefinition);

    protected override FieldDefinition GetFieldDefinition(string fieldId, XmlElement fieldElement)
    {
      return EncompassFields.GetField(fieldId) ?? base.GetFieldDefinition(fieldId, fieldElement);
    }

    private class RateLockFieldSelectorDef : LoanReportFieldDef
    {
      public RateLockFieldSelectorDef()
        : base("Rate Lock", (FieldDefinition) new ReportingFieldDefinition("RATELOCK...", "Select Other Rate Lock Fields...", FieldFormat.STRING, EncompassEdition.Banker))
      {
      }

      public override bool IsLoanDataField => true;
    }

    private class InterimFieldSelectorDef : LoanReportFieldDef
    {
      public InterimFieldSelectorDef()
        : base("Interim Servicing", (FieldDefinition) new ReportingFieldDefinition("ISPAY...", "Select Interim Servicing Fields...", FieldFormat.STRING, EncompassEdition.Banker))
      {
      }

      public override bool IsLoanDataField => true;
    }

    private class GFEDisclosureFieldSelectorDef : LoanReportFieldDef
    {
      public GFEDisclosureFieldSelectorDef()
        : base("GFE Disclosure Tracking", (FieldDefinition) new ReportingFieldDefinition("DISCLOSEDGFE.Snapshot...", "Select Other GFE Disclosed Fields...", FieldFormat.STRING, EncompassEdition.None))
      {
      }

      public override bool IsLoanDataField => true;
    }

    private class LEDisclosureFieldSelectorDef : LoanReportFieldDef
    {
      public LEDisclosureFieldSelectorDef()
        : base("LE Disclosure Tracking", (FieldDefinition) new ReportingFieldDefinition("DISCLOSEDLE.Snapshot...", "Select Other LE Disclosed Fields...", FieldFormat.STRING, EncompassEdition.None))
      {
      }

      public override bool IsLoanDataField => true;
    }

    private class AuditTrailFieldSelectorDef : LoanReportFieldDef
    {
      public AuditTrailFieldSelectorDef()
        : base("Audit Trail", (FieldDefinition) new ReportingFieldDefinition("AuditTrail...", "Select Data from the Audit Trail...", FieldFormat.STRING, EncompassEdition.None), "AuditTrail...", FieldDisplayType.Normal)
      {
      }

      public override bool IsLoanDataField => false;
    }
  }
}
