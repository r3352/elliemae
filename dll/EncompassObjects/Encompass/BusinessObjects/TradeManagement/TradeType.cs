// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.TradeType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.ComponentModel;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  public enum TradeType
  {
    None,
    SecurityTrade,
    [Description("Loan Trade")] LoanTrade,
    [Description("Mbs Pool")] MbsPool,
    [Description("Correspondent Trade")] CorrespondentTrade,
    [Description("Correspondent Master")] CorrespondentMaster,
    [Description("Master Contract")] MasterContract,
    [Description("GSE Commitment")] GSECommitment,
  }
}
