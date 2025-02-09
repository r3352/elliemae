// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.SystemAuditTrail.BizRuleAuditRecord
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.SystemAuditTrail
{
  [Serializable]
  public class BizRuleAuditRecord : SystemAuditRecord
  {
    private string ruleID;
    private string ruleName = "";

    public BizRuleAuditRecord(
      string invokeUserID,
      string invokeUserFullName,
      ActionType action,
      DateTime loginTime,
      string ruleID,
      string ruleName,
      AuditObjectType type)
      : base(invokeUserID, invokeUserFullName, action, loginTime, type)
    {
      this.ruleID = ruleID;
      this.ruleName = ruleName;
    }

    public string RuleID
    {
      get => this.ruleID;
      set => this.ruleID = value;
    }

    public string RuleName
    {
      get => this.ruleName;
      set => this.ruleName = value;
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
          "Rule ID",
          "Rule Name"
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
          this.RuleID.ToString(),
          this.RuleName
        };
      }
    }

    public override string ContentToCSV
    {
      get
      {
        return string.Join(",", this.CSVEncode(this.userID), this.CSVEncode(this.actionType.ToString()), this.CSVEncode(this.dateTime.ToString("MM/dd/yyyy HH:mm:ss")), this.CSVEncode(this.RuleID.ToString()), this.CSVEncode(this.RuleName));
      }
    }
  }
}
