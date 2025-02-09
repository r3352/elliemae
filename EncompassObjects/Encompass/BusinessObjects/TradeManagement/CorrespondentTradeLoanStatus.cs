// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.CorrespondentTradeLoanStatus
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.ComponentModel;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  /// <summary>Defines possible Correspondent Trade Loan statuses</summary>
  public enum CorrespondentTradeLoanStatus
  {
    /// <summary>None</summary>
    [Description("None")] None,
    /// <summary>Unassigned status</summary>
    [Description("Unassigned")] Unassigned,
    /// <summary>Assigned status</summary>
    [Description("Assigned")] Assigned,
    /// <summary>Shipped status</summary>
    [Description("Shipped")] Shipped,
    /// <summary>Purchased status</summary>
    [Description("Purchased")] Purchased,
  }
}
