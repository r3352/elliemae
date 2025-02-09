// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.PipelineSortOrder
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// Enumeration of the different possible pipeline sort orders.
  /// </summary>
  [Guid("ABAACEA9-9973-4669-BCF8-B91CFED095C5")]
  public enum PipelineSortOrder
  {
    /// <summary>No sort is applied to the pipeline elements.</summary>
    None,
    /// <summary>Items with current alerts are placed first in the sort order.</summary>
    Alert,
    /// <summary>Items are sorted based on the borrower's last name.</summary>
    LastName,
    /// <summary>Items are sorted based on the current milestone, with the earliest milestones first.</summary>
    Milestone,
    /// <summary>Items are sorted based on rate lock date.</summary>
    RateLock,
  }
}
