// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.CorrespondentMasterDeliveryType
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System.ComponentModel;

#nullable disable
namespace EllieMae.EMLite.Trading
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
