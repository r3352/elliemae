// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.CorrespondentMasterDeliveryType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.ComponentModel;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  /// <summary>Defines possible Correspondent Master Delivery types</summary>
  public enum CorrespondentMasterDeliveryType
  {
    /// <summary>None</summary>
    [Description("None")] None = 0,
    /// <summary>AOT Delivery type</summary>
    [Description("AOT")] AOT = 5,
    /// <summary>Forwards type</summary>
    [Description("Forwards")] Forwards = 10, // 0x0000000A
    /// <summary>Individual Best Efforts type</summary>
    [Description("Individual Best Efforts")] IndividualBestEfforts = 15, // 0x0000000F
    /// <summary>Individual Mandatory type</summary>
    [Description("Individual Mandatory")] IndividualMandatory = 20, // 0x00000014
    /// <summary>Direct Trade type</summary>
    [Description("Direct Trade")] LiveTrade = 25, // 0x00000019
    /// <summary>Bulk type</summary>
    [Description("Bulk")] Bulk = 30, // 0x0000001E
    /// <summary>Bulk AOT type</summary>
    [Description("Bulk AOT")] BulkAOT = 35, // 0x00000023
    /// <summary>Co-Issue type</summary>
    [Description("Co-Issue")] CoIssue = 40, // 0x00000028
  }
}
