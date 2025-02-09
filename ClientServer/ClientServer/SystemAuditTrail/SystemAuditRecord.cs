// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.SystemAuditTrail.SystemAuditRecord
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.SystemAuditTrail
{
  [Serializable]
  public abstract class SystemAuditRecord
  {
    protected string userID;
    protected string userFullName;
    protected ActionType actionType;
    protected DateTime dateTime;
    protected AuditObjectType objectType;
    protected string impersonatedUserID;
    protected string impersonatedUserFullName;

    public SystemAuditRecord(
      string userID,
      string userFullName,
      ActionType actionType,
      DateTime dateTime,
      AuditObjectType objectType)
    {
      this.userID = userID;
      this.userFullName = userFullName;
      this.actionType = actionType;
      this.dateTime = dateTime;
      this.objectType = objectType;
    }

    public SystemAuditRecord(
      string userID,
      string userFullName,
      ActionType actionType,
      AuditObjectType objectType)
    {
      this.userID = userID;
      this.userFullName = userFullName;
      this.actionType = actionType;
      this.objectType = objectType;
    }

    [CLSCompliant(false)]
    public string UserID => this.userID;

    [CLSCompliant(false)]
    public string UserFullName => this.userFullName;

    [CLSCompliant(false)]
    public ActionType ActionType => this.actionType;

    [CLSCompliant(false)]
    public DateTime DateTime => this.dateTime;

    [CLSCompliant(false)]
    public AuditObjectType ObjectType => this.objectType;

    [CLSCompliant(false)]
    public string ImpersonatedUserID => this.impersonatedUserID;

    [CLSCompliant(false)]
    public string ImpersonatedUserFullName => this.impersonatedUserFullName;

    public abstract string[] ColumnHeaders { get; }

    public virtual string[] ColumnContents
    {
      get
      {
        return new string[3]
        {
          this.userID,
          this.actionType.ToString(),
          this.dateTime.ToString("MM/dd/yyyy HH:mm:ss")
        };
      }
    }

    public virtual string ContentToCSV
    {
      get
      {
        return string.Join(",", this.CSVEncode(this.userID), this.CSVEncode(this.actionType.ToString()), this.CSVEncode(this.dateTime.ToString("MM/dd/yyyy HH:mm:ss")));
      }
    }

    public string HeaderToCSV
    {
      get
      {
        string[] columnHeaders = this.ColumnHeaders;
        string headerToCsv = "";
        foreach (string rawData in columnHeaders)
          headerToCsv = !(headerToCsv == "") ? headerToCsv + "," + this.CSVEncode(rawData) : this.CSVEncode(rawData);
        return headerToCsv;
      }
    }

    protected string CSVEncode(string rawData)
    {
      return rawData == null ? "" : "\"" + rawData.Replace("\"", "\"\"") + "\"";
    }
  }
}
