// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.eFolder.ConcurrentUpdateActionType
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System.ComponentModel;

#nullable disable
namespace EllieMae.EMLite.ClientServer.eFolder
{
  public enum ConcurrentUpdateActionType
  {
    [Description("CreateDocument")] CreateDocument,
    [Description("AssignAttachmentToDocument")] AssignAttachmentToDocument,
    [Description("UnAssignedAttachmentFromDocument")] UnAssignedAttachmentFromDocument,
    [Description("LinkDocumentToACondition")] LinkDocumentToACondition,
    [Description("UnlinkDocumentFromACondition")] UnlinkDocumentFromACondition,
    [Description("UpdateDocument")] UpdateDocument,
    [Description("CreateCondition")] CreateCondition,
    [Description("UpdateCondition")] UpdateCondition,
    [Description("UpdateAssignedAttachment")] UpdateAssignedAttachment,
    [Description("AddAllowedRole")] AddAllowedRole,
    [Description("RemoveAllowedRole")] RemoveAllowedRole,
    [Description("AddCommentsToRecord")] AddCommentsToRecord,
    [Description("UpdateCommentsInRecord")] UpdateCommentsInRecord,
    [Description("DeleteCommentsFromRecord")] DeleteCommentsFromRecord,
    [Description("AddEnhanceConditionTrackings")] AddEnhanceConditionTrackings,
    [Description("RemoveEnhanceConditionTrackings")] RemoveEnhanceConditionTrackings,
    [Description("SetReceivedStatus")] SetReceivedStatus,
    [Description("DeleteDocument")] DeleteDocument,
  }
}
