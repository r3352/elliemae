// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.CorrespondentMasterDeliveryType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.ComponentModel;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  public enum CorrespondentMasterDeliveryType
  {
    [Description("None")] None = 0,
    [Description("AOT")] AOT = 5,
    [Description("Forwards")] Forwards = 10, // 0x0000000A
    [Description("Individual Best Efforts")] IndividualBestEfforts = 15, // 0x0000000F
    [Description("Individual Mandatory")] IndividualMandatory = 20, // 0x00000014
    [Description("Direct Trade")] LiveTrade = 25, // 0x00000019
    [Description("Bulk")] Bulk = 30, // 0x0000001E
    [Description("Bulk AOT")] BulkAOT = 35, // 0x00000023
    [Description("Co-Issue")] CoIssue = 40, // 0x00000028
  }
}
