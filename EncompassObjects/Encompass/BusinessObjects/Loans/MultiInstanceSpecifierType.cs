// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.MultiInstanceSpecifierType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// Indicates the type of object used to specify an instance of a multi-instance field.
  /// </summary>
  public enum MultiInstanceSpecifierType
  {
    /// <summary>Indictes the field is not multi-instance.</summary>
    None = 0,
    /// <summary>Instances are sepcified using a numeric index.</summary>
    Index = 1,
    /// <summary>Instances are sepcified using a role name, e.g. "Loan Officer".</summary>
    Role = 2,
    /// <summary>Instances are sepcified using a milestone name, e.g. "Started".</summary>
    Milestone = 3,
    /// <summary>Instances are sepcified using a tracked document name, e.g. "Credit Report".</summary>
    Document = 4,
    /// <summary>Instances are sepcified using an underwriting condition name, e.g. "Pay Stubs".</summary>
    UnderwritingCondition = 5,
    /// <summary>Instances are sepcified using a post-closing condition name, e.g. "HMDA Report".</summary>
    PostClosingCondition = 6,
    /// <summary>Instances are sepcified using a milestone task name, e.g. "Pull Credit".</summary>
    MilestoneTask = 8,
  }
}
