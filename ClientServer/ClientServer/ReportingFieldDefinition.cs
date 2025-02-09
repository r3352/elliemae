// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ReportingFieldDefinition
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.DataEngine;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public class ReportingFieldDefinition : FieldDefinition
  {
    private FieldFormat format;
    private string description;
    private FieldOptionCollection options;
    private EncompassEdition allowedEditions;
    private string rolodex;

    public ReportingFieldDefinition(string fieldId, XmlElement e)
      : base(fieldId)
    {
      try
      {
        this.format = (FieldFormat) Enum.Parse(typeof (FieldFormat), e.GetAttribute(nameof (Format)), true);
      }
      catch
      {
        throw new ArgumentException("Missing or invalid Format specified for field '" + fieldId + "'");
      }
      this.description = e.GetAttribute("desc") ?? "";
      string str = e.GetAttribute("Edition") ?? "";
      if (str != "")
        this.allowedEditions = (EncompassEdition) Enum.Parse(typeof (EncompassEdition), str, true);
      this.options = new FieldOptionCollection((FieldDefinition) this, (XmlElement) e.SelectSingleNode(nameof (Options)));
    }

    public ReportingFieldDefinition(string fieldId, string description, FieldFormat format)
      : this(fieldId, description, format, EncompassEdition.None)
    {
    }

    public ReportingFieldDefinition(
      string fieldId,
      string description,
      FieldFormat format,
      EncompassEdition allowedEditions)
      : base(fieldId)
    {
      this.format = format;
      this.description = description;
      this.options = new FieldOptionCollection((FieldDefinition) this, (XmlElement) null);
      this.allowedEditions = allowedEditions;
    }

    public override FieldFormat Format
    {
      get => this.format;
      set => this.format = value;
    }

    public override string Description => this.description;

    public override FieldOptionCollection Options => this.options;

    public override string Rolodex
    {
      get => this.rolodex;
      set => this.rolodex = value;
    }

    public override string GetValue(LoanData loan) => throw new InvalidOperationException();

    public override string GetValue(LoanData loan, string id)
    {
      throw new InvalidOperationException();
    }

    public override void SetValue(LoanData loan, string value)
    {
      throw new InvalidOperationException();
    }

    public override bool AppliesToEdition(EncompassEdition edition)
    {
      return this.allowedEditions == EncompassEdition.None || this.allowedEditions == edition;
    }

    public static ReportingFieldDefinition FromXDBField(LoanXDBField xdbField)
    {
      return new ReportingFieldDefinition(xdbField.FieldID, xdbField.Description, LoanXDBTable.TableTypeToFieldFormat(xdbField.FieldType));
    }

    public static ReportingFieldDefinition FromAuditField(LoanXDBAuditField auditField)
    {
      return new ReportingFieldDefinition(auditField.ReportingCriterionName, auditField.Description, LoanXDBTable.TableTypeToFieldFormat(auditField.FieldType));
    }

    public static ReportingFieldDefinition FromAlertDefinition(AlertDefinition alertDef)
    {
      return new ReportingFieldDefinition(alertDef.GetCriterionName(), "Alert - " + alertDef.Name, FieldFormat.INTEGER);
    }

    public static ReportingFieldDefinition FromLoamExternalDataDefinition(
      LoanExternalFieldConfig loanExternalFieldData)
    {
      return new ReportingFieldDefinition(loanExternalFieldData.FieldID, loanExternalFieldData.Description, loanExternalFieldData.FieldType);
    }
  }
}
