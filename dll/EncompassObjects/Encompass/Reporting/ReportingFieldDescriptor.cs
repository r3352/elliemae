// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Reporting.ReportingFieldDescriptor
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;

#nullable disable
namespace EllieMae.Encompass.Reporting
{
  public class ReportingFieldDescriptor : IReportingFieldDescriptor
  {
    private LoanXDBField xdbfield;

    internal ReportingFieldDescriptor(LoanXDBField xdbfield) => this.xdbfield = xdbfield;

    public string FieldID => this.xdbfield.FieldID;

    public int BorrowerPair => this.xdbfield.ComortgagorPair;

    public string CanonicalName => "Fields." + this.xdbfield.FieldIDWithCoMortgagor;

    public string TableName => this.xdbfield.TableName;

    public string ColumnName => this.xdbfield.ColumnName;

    public bool Auditable => this.xdbfield.Auditable;

    public string Description => this.xdbfield.Description;

    public string FieldType => this.xdbfield.FieldType.ToString();

    public int FieldSize => this.xdbfield.FieldSizeToInteger;
  }
}
