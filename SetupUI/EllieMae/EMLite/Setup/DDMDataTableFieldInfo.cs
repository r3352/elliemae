// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DDMDataTableFieldInfo
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class DDMDataTableFieldInfo
  {
    public string FieldId { get; set; }

    public string Description { get; set; }

    public ReportingDatabaseColumnType Type { get; set; }

    public FieldFormat Format { get; set; }

    public FieldCategory Category { get; set; }

    public int ComortgagorPair { get; set; }

    public bool IsOutput { get; set; }

    public DDMDataTableFieldInfo()
    {
    }

    private ReportingDatabaseColumnType getReportingDatabaseColumnType(
      FieldFormat overrideFieldFormat)
    {
      switch (overrideFieldFormat)
      {
        case FieldFormat.INTEGER:
        case FieldFormat.DECIMAL_1:
        case FieldFormat.DECIMAL_2:
        case FieldFormat.DECIMAL_3:
        case FieldFormat.DECIMAL_4:
        case FieldFormat.DECIMAL_6:
        case FieldFormat.DECIMAL_5:
        case FieldFormat.DECIMAL_7:
        case FieldFormat.DECIMAL:
        case FieldFormat.DECIMAL_10:
          return ReportingDatabaseColumnType.Numeric;
        case FieldFormat.DATE:
        case FieldFormat.MONTHDAY:
          return ReportingDatabaseColumnType.Date;
        case FieldFormat.DATETIME:
          return ReportingDatabaseColumnType.DateTime;
        default:
          return ReportingDatabaseColumnType.Text;
      }
    }

    private FieldFormat getFieldFormat(string fieldId)
    {
      FieldDefinition fieldDefinition = DDM_FieldAccess_Utils.GetFieldDefinition(fieldId, Session.LoanManager);
      return fieldDefinition == null ? FieldFormat.NONE : fieldDefinition.Format;
    }

    public DDMDataTableFieldInfo(FieldDefinition fieldDefinition)
    {
      this.FieldId = fieldDefinition.FieldID;
      this.Description = fieldDefinition.Description;
      this.Format = this.getFieldFormat(fieldDefinition.FieldID);
      this.Type = this.getReportingDatabaseColumnType(this.Format);
      this.Category = fieldDefinition.Category;
      if (fieldDefinition.Category == FieldCategory.Common)
        return;
      this.ComortgagorPair = 1;
    }

    public DDMDataTableFieldInfo(LoanXDBField xdbField, FieldDefinition fieldDefinition)
    {
      this.FieldId = xdbField.FieldIDWithCoMortgagor;
      this.Description = xdbField.Description;
      this.Format = this.getFieldFormat(fieldDefinition.FieldID);
      this.Type = this.getReportingDatabaseColumnType(this.Format);
      this.Category = fieldDefinition.Category;
      this.ComortgagorPair = xdbField.ComortgagorPair;
    }

    public DDMDataTableFieldInfo(
      string FieldId,
      string Description,
      ReportingDatabaseColumnType Type)
    {
      this.FieldId = FieldId;
      this.Description = Description;
      this.Format = this.getFieldFormat(FieldId);
      this.Type = this.getReportingDatabaseColumnType(this.Format);
    }

    public DDMDataTableFieldInfo(
      string FieldId,
      string Description,
      ReportingDatabaseColumnType Type,
      FieldCategory Category,
      int pairIndex,
      FieldFormat format)
    {
      this.FieldId = FieldId;
      this.Description = Description;
      this.Category = Category;
      this.ComortgagorPair = pairIndex;
      this.Format = this.getFieldFormat(FieldId);
      this.Type = this.getReportingDatabaseColumnType(this.Format);
    }

    public DDMDataTableFieldInfo(string fieldId, bool isOutput = true, int outputIdx = -1)
    {
      this.FieldId = fieldId;
      this.Description = string.Format("Output {0}", outputIdx < 0 ? (object) string.Empty : (object) string.Concat((object) (outputIdx + 1)));
      this.Format = FieldFormat.NONE;
      this.Type = ReportingDatabaseColumnType.Text;
      this.IsOutput = isOutput;
    }
  }
}
