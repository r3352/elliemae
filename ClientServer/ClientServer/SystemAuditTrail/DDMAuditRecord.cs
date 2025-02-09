// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.SystemAuditTrail.DDMAuditRecord
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.SystemAuditTrail
{
  [Serializable]
  public class DDMAuditRecord : SystemAuditRecord
  {
    private string orderID;
    private string ruleName = "";
    private string scenarioName = "";
    private string additionalInfo = "";

    public DDMAuditRecord(
      string invokeUserID,
      string invokeUserFullName,
      ActionType action,
      DateTime loginTime,
      string orderID,
      string ruleName,
      AuditObjectType type,
      string scenarioName = "�",
      string additionalInfo = "�")
      : base(invokeUserID, invokeUserFullName, action, loginTime, type)
    {
      this.orderID = orderID;
      this.ruleName = ruleName;
      this.scenarioName = scenarioName;
      this.additionalInfo = additionalInfo;
    }

    public string OrderID
    {
      get => this.orderID;
      set => this.orderID = value;
    }

    public string RuleName
    {
      get => this.ruleName;
      set => this.ruleName = value;
    }

    public string ScenarioName
    {
      get => this.scenarioName;
      set => this.scenarioName = value;
    }

    public string AddtionalInfo
    {
      get => this.additionalInfo;
      set => this.additionalInfo = value;
    }

    public override string[] ColumnHeaders
    {
      get
      {
        return new string[7]
        {
          "Performed by User",
          "Action Type",
          "DateTime",
          "Order ID",
          "Setting / Rule Name",
          "Scenario Name",
          "Addtional Information"
        };
      }
    }

    public override string[] ColumnContents
    {
      get
      {
        return new string[7]
        {
          this.userID,
          this.actionType.ToString(),
          this.dateTime.ToString("MM/dd/yyyy HH:mm:ss"),
          this.OrderID.ToString(),
          this.RuleName,
          this.scenarioName,
          this.additionalInfo
        };
      }
    }

    public override string ContentToCSV
    {
      get
      {
        return string.Join(",", this.CSVEncode(this.userID), this.CSVEncode(this.actionType.ToString()), this.CSVEncode(this.dateTime.ToString("MM/dd/yyyy HH:mm:ss")), this.CSVEncode(this.OrderID.ToString()), this.CSVEncode(this.RuleName), this.CSVEncode(this.scenarioName), this.CSVEncode(this.additionalInfo));
      }
    }
  }
}
