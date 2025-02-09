// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.AclEnhancedConditionType
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public enum AclEnhancedConditionType
  {
    AccessConditions = 1,
    AddConditions = 2,
    AddAutomatedConditions = 3,
    EditConditions = 4,
    ChangePriorTo = 5,
    AddComments = 6,
    RemoveComments = 7,
    MarkComments = 8,
    AssignDocuments = 9,
    UnassignDocuments = 10, // 0x0000000A
    DeleteConditions = 11, // 0x0000000B
    CreateBlankCondition = 12, // 0x0000000C
    ImportAllConditions = 13, // 0x0000000D
    ReviewAndImportConditions = 14, // 0x0000000E
    DeliveryConditionResponses = 15, // 0x0000000F
    ViewConditionDeliveryStatus = 16, // 0x00000010
    InternalDescription = 17, // 0x00000011
    ExternalDescription = 18, // 0x00000012
    PrintInternally = 19, // 0x00000013
    PrintExternally = 20, // 0x00000014
    ImportAUSFindings = 21, // 0x00000015
    ImportLoanQualityFindings = 22, // 0x00000016
  }
}
