// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.CorrespondentTradeLoanStatus
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.ComponentModel;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  public enum CorrespondentTradeLoanStatus
  {
    [Description("None")] None,
    [Description("Unassigned")] Unassigned,
    [Description("Assigned")] Assigned,
    [Description("Shipped")] Shipped,
    [Description("Purchased")] Purchased,
  }
}
