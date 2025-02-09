// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Reporting.ReportingFieldDescriptor
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;

#nullable disable
namespace EllieMae.Encompass.Reporting
{
  /// <summary>
  /// Represents the metadata for a field in the Reporting Database.
  /// </summary>
  public class ReportingFieldDescriptor : IReportingFieldDescriptor
  {
    private LoanXDBField xdbfield;

    internal ReportingFieldDescriptor(LoanXDBField xdbfield) => this.xdbfield = xdbfield;

    /// <summary>Gets the Field ID for the field.</summary>
    public string FieldID => this.xdbfield.FieldID;

    /// <summary>Returns the Borrower Pair index for the field.</summary>
    public int BorrowerPair => this.xdbfield.ComortgagorPair;

    /// <summary>
    /// Returns the canonical name of the field to be used in queries.
    /// </summary>
    public string CanonicalName => "Fields." + this.xdbfield.FieldIDWithCoMortgagor;

    /// <summary>
    /// Returns the name of the underlying SQL table that stores this field
    /// </summary>
    public string TableName => this.xdbfield.TableName;

    /// <summary>
    /// Returns the name of the column in the SQL table in which this data is stored.
    /// </summary>
    public string ColumnName => this.xdbfield.ColumnName;

    /// <summary>
    /// Indicates if the Audit Trail is enabled for this field.
    /// </summary>
    public bool Auditable => this.xdbfield.Auditable;

    /// <summary>Gets the Description for the field.</summary>
    public string Description => this.xdbfield.Description;

    /// <summary>Get the FieldType for the field.</summary>
    public string FieldType => this.xdbfield.FieldType.ToString();

    /// <summary>Gets the FieldSize for the field.</summary>
    public int FieldSize => this.xdbfield.FieldSizeToInteger;
  }
}
