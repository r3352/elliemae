// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.CorrespondentTradeCommitmentType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.ComponentModel;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  /// <summary>Defines possible Correspondent Trade Commitment Types</summary>
  public enum CorrespondentTradeCommitmentType
  {
    /// <summary>None</summary>
    [Description("None")] None,
    /// <summary>Best Efforts status</summary>
    [Description("Best Efforts")] BestEfforts,
    /// <summary>Mandatory status</summary>
    [Description("Mandatory")] Mandatory,
  }
}
