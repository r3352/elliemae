// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.SystemAuditTrail.BizContactGroupAuditRecord
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.SystemAuditTrail
{
  [Serializable]
  public class BizContactGroupAuditRecord : SystemAuditRecord
  {
    private int bizContactGroupID;
    private string bizContactGroupName = "";

    public BizContactGroupAuditRecord(
      string invokeUserID,
      string invokeUserFullName,
      ActionType action,
      DateTime loginTime,
      int bizContactGroupID,
      string bizContactGroupName)
      : base(invokeUserID, invokeUserFullName, action, loginTime, AuditObjectType.BusinessContactGroup)
    {
      this.bizContactGroupID = bizContactGroupID;
      this.bizContactGroupName = bizContactGroupName;
    }

    public int BizContactGroupID
    {
      get => this.bizContactGroupID;
      set => this.bizContactGroupID = value;
    }

    public string BizContactGroupName
    {
      get => this.bizContactGroupName;
      set => this.bizContactGroupName = value;
    }

    public override string[] ColumnHeaders
    {
      get
      {
        return new string[5]
        {
          "Performed by User",
          "Action Type",
          "DateTime",
          "Business Contact Group ID",
          "Business Contact Group Name"
        };
      }
    }

    public override string[] ColumnContents
    {
      get
      {
        return new string[5]
        {
          this.userID,
          this.actionType.ToString(),
          this.dateTime.ToString("MM/dd/yyyy HH:mm:ss"),
          this.BizContactGroupID.ToString(),
          this.BizContactGroupName
        };
      }
    }

    public override string ContentToCSV
    {
      get
      {
        return string.Join(",", this.CSVEncode(this.userID), this.CSVEncode(this.actionType.ToString()), this.CSVEncode(this.dateTime.ToString("MM/dd/yyyy HH:mm:ss")), this.CSVEncode(this.BizContactGroupID.ToString()), this.CSVEncode(this.BizContactGroupName));
      }
    }
  }
}
