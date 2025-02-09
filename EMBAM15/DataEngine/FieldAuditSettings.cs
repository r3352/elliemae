// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.FieldAuditSettings
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class FieldAuditSettings
  {
    private AuditData data;
    private string fieldId;

    public FieldAuditSettings(string fieldId, AuditData auditData)
    {
      this.data = auditData;
      this.fieldId = fieldId;
    }

    public string FieldID => this.fieldId;

    public AuditData AuditData => this.data;

    public string AuditDataAsString
    {
      get
      {
        if (this.data == AuditData.UserID)
          return "UserID";
        if (this.data == AuditData.UserName)
          return "UserName";
        if (this.data == AuditData.Timestamp)
          return "Timestamp";
        throw new Exception("Unhandled AuditData");
      }
    }

    public static AuditData StringToAuditData(string str)
    {
      str = (str ?? "").Trim().ToLower();
      switch (str)
      {
        case "userid":
          return AuditData.UserID;
        case "username":
          return AuditData.UserName;
        case "timestamp":
          return AuditData.Timestamp;
        default:
          throw new Exception(str + ": unrecognized AuditData string");
      }
    }
  }
}
