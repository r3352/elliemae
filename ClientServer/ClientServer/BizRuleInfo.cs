// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.BizRuleInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Customization;
using EllieMae.EMLite.FieldSearch;
using EllieMae.EMLite.Packages;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public abstract class BizRuleInfo : IXmlSerializable, IComparable, ICloneable, IHashableContents
  {
    private int ruleID;
    private string lastModifiedByUserId = "";
    private string lastModifiedByFullName = "";
    private string ruleName;
    public BizRule.Condition Condition;
    public string Condition2 = "0,1,2,3,4";
    public string ConditionState;
    public string ConditionState2;
    public string AdvancedCodeXML;
    public BizRule.RuleStatus Status;
    public DateTime LastModTime = DateTime.MinValue;
    public string CommentsTxt = string.Empty;

    protected BizRuleInfo(string ruleName)
      : this(0, ruleName)
    {
    }

    protected BizRuleInfo(int ruleID, string ruleName)
      : this(ruleID, ruleName, BizRule.Condition.Null, "0,1,2,3,4", "", "", (string) null)
    {
    }

    protected BizRuleInfo(
      string ruleName,
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2,
      string advancedCodeXml)
      : this(0, ruleName, condition, condition2, conditionState, conditionState2, advancedCodeXml)
    {
    }

    protected BizRuleInfo(
      int ruleID,
      string ruleName,
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2,
      string advancedCodeXml)
    {
      this.ruleID = ruleID;
      this.ruleName = ruleName;
      this.Condition = condition;
      this.Condition2 = condition2;
      this.ConditionState = conditionState;
      this.ConditionState2 = conditionState2;
      this.AdvancedCodeXML = advancedCodeXml;
    }

    protected BizRuleInfo(
      int ruleID,
      string ruleName,
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2,
      string advancedCodeXml,
      string comments)
      : this(ruleID, ruleName, condition, condition2, conditionState, conditionState2, advancedCodeXml)
    {
      this.CommentsTxt = comments;
    }

    protected BizRuleInfo(XmlSerializationInfo info)
    {
      this.ruleID = info.GetInteger(nameof (RuleID));
      this.ruleName = info.GetString(nameof (RuleName));
      this.Condition = info.GetEnum<BizRule.Condition>(nameof (Condition));
      this.Condition2 = info.GetString(nameof (Condition2));
      this.ConditionState = info.GetString(nameof (ConditionState));
      this.ConditionState2 = info.GetString(nameof (ConditionState2));
      if (this.Condition == BizRule.Condition.AdvancedCoding)
        this.ConditionState = info.GetString("AdvancedCode");
      this.Status = info.GetEnum<BizRule.RuleStatus>(nameof (Status));
      this.LastModTime = info.GetDateTime(nameof (LastModTime));
      this.AdvancedCodeXML = info.GetString("AdvancedCodeXml");
      this.CommentsTxt = info.GetString("Comments");
      this.LastModifiedByFullName = info.GetString(nameof (LastModifiedByFullName));
      this.LastModifiedByUserId = info.GetString(nameof (LastModifiedByUserId));
      if (!(info is PackageSerializationInfo serializationInfo))
        return;
      string name1 = info.GetString("Milestone", (string) null);
      if (name1 != null)
        this.ConditionState = serializationInfo.NameToMilestoneID(name1);
      string name2 = info.GetString("Role", (string) null);
      if (name2 == null)
        return;
      this.ConditionState2 = string.Concat((object) serializationInfo.NameToRoleID(name2));
    }

    protected BizRuleInfo(DataRow ruleInfo)
    {
      this.ruleID = (int) ruleInfo[nameof (ruleID)];
      this.ruleName = (string) ruleInfo[nameof (ruleName)];
      this.Condition = (BizRule.Condition) ruleInfo["condition"];
      this.Condition2 = string.Concat(ruleInfo["condition2"]);
      this.ConditionState = (string) ruleInfo["conditionState"];
      this.ConditionState2 = string.Concat(ruleInfo["conditionState2"]);
      if (this.Condition == BizRule.Condition.AdvancedCoding)
        this.ConditionState = string.Concat(ruleInfo["advancedCode"]);
      this.Status = (BizRule.RuleStatus) byte.Parse(ruleInfo["status"].ToString());
      this.LastModTime = (DateTime) ruleInfo["lastModTime"];
      this.AdvancedCodeXML = string.Concat(ruleInfo["advancedCodeXml"]);
      if (ruleInfo.Table.Columns.Contains(nameof (lastModifiedByFullName)))
        this.LastModifiedByFullName = ruleInfo[nameof (lastModifiedByFullName)] != DBNull.Value ? (string) ruleInfo[nameof (lastModifiedByFullName)] : "";
      if (ruleInfo.Table.Columns.Contains(nameof (lastModifiedByUserId)))
        this.LastModifiedByUserId = ruleInfo[nameof (lastModifiedByUserId)] != DBNull.Value ? (string) ruleInfo[nameof (lastModifiedByUserId)] : "";
      if (!ruleInfo.Table.Columns.Contains("Comments"))
        return;
      this.CommentsTxt = ruleInfo["Comments"] != DBNull.Value ? (string) ruleInfo["Comments"] : "";
    }

    public IEnumerable<KeyValuePair<RelationshipType, string>> GetBaseFields()
    {
      yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ConditionOf, "2626");
      if (this.Condition == BizRule.Condition.RateLock)
        yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ConditionOf, "2400");
      else if (this.Condition == BizRule.Condition.PropertyType)
        yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ConditionOf, "1041");
      else if (this.Condition == BizRule.Condition.FinishedMilestone)
        yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ConditionOf, "MS.STATUS");
      else if (this.Condition == BizRule.Condition.LoanDocType)
        yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ConditionOf, "MORNET.X67");
      else if (this.Condition == BizRule.Condition.LoanProgram)
        yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ConditionOf, "1401");
      else if (this.Condition == BizRule.Condition.LoanPurpose)
        yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ConditionOf, "19");
      else if (this.Condition == BizRule.Condition.LoanStatus)
        yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ConditionOf, "1393");
      else if (this.Condition == BizRule.Condition.LoanType)
        yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ConditionOf, "1172");
      else if (this.Condition == BizRule.Condition.PropertyState)
        yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ConditionOf, "14");
      else if (this.Condition == BizRule.Condition.AdvancedCoding)
      {
        string[] strArray = FieldReplacementRegex.ParseDependentFields(this.ConditionState);
        for (int index = 0; index < strArray.Length; ++index)
          yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ConditionOf, strArray[index]);
        strArray = (string[]) null;
      }
    }

    public abstract BizRuleType RuleType { get; }

    public int RuleID
    {
      get => this.ruleID;
      set => this.ruleID = value;
    }

    public string RuleName => this.ruleName;

    public string MilestoneID
    {
      get
      {
        return this.Condition == BizRule.Condition.CurrentLoanAssocMS || this.Condition == BizRule.Condition.FinishedMilestone ? this.ConditionState : (string) null;
      }
    }

    public int RoleID
    {
      get
      {
        if (this.Condition != BizRule.Condition.CurrentLoanAssocMS)
          return -1;
        try
        {
          return Convert.ToInt32(this.ConditionState2);
        }
        catch
        {
          return 0;
        }
      }
    }

    public int ConditionStateInt
    {
      get
      {
        if (this.Condition != BizRule.Condition.CurrentLoanAssocMS)
        {
          if (this.Condition != BizRule.Condition.FinishedMilestone)
          {
            try
            {
              return Convert.ToInt32(this.ConditionState);
            }
            catch
            {
              return 0;
            }
          }
        }
        return -1;
      }
    }

    public bool Inactive => this.Status == BizRule.RuleStatus.Inactive;

    public bool IsGeneralRule => this.Condition == BizRule.Condition.Null;

    public string LastModifiedByUserId
    {
      get => this.lastModifiedByUserId;
      set => this.lastModifiedByUserId = value;
    }

    public string LastModifiedByFullName
    {
      get => this.lastModifiedByFullName;
      set => this.lastModifiedByFullName = value;
    }

    public string LastModifiedByUserInfo
    {
      get
      {
        return !(this.lastModifiedByUserId == "") ? this.lastModifiedByUserId + " (" + this.lastModifiedByFullName + ")" : "";
      }
    }

    public override bool Equals(object obj)
    {
      if (!(obj is BizRuleInfo bizRuleInfo))
        return false;
      return this.ruleID <= 0 || bizRuleInfo.ruleID <= 0 ? this == bizRuleInfo : this.RuleID == bizRuleInfo.RuleID;
    }

    int IHashableContents.GetContentsHashCode()
    {
      return ObjectArrayHelpers.GetAggregateHash((object) this.RuleID, (object) this.LastModTime);
    }

    public override int GetHashCode() => this.ruleID.GetHashCode();

    public abstract object Clone();

    public BizRuleInfo Duplicate(string newName)
    {
      BizRuleInfo bizRuleInfo = (BizRuleInfo) this.Clone();
      bizRuleInfo.ruleID = 0;
      bizRuleInfo.ruleName = newName;
      return bizRuleInfo;
    }

    protected void copyInto(BizRuleInfo rule)
    {
      rule.Condition = this.Condition;
      rule.Condition2 = this.Condition2;
      rule.ConditionState = this.ConditionState;
      rule.ConditionState2 = this.ConditionState2;
      rule.AdvancedCodeXML = this.AdvancedCodeXML;
    }

    public virtual void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("RuleID", (object) this.ruleID);
      info.AddValue("RuleName", (object) this.ruleName);
      info.AddValue("Condition", (object) this.Condition);
      info.AddValue("Condition2", (object) this.Condition2);
      info.AddValue("ConditionState", (object) this.ConditionState);
      info.AddValue("ConditionState2", (object) this.ConditionState2);
      if (this.Condition == BizRule.Condition.AdvancedCoding)
        info.AddValue("AdvancedCode", (object) this.ConditionState);
      info.AddValue("LastModTime", (object) this.LastModTime);
      info.AddValue("AdvancedCodeXml", (object) this.AdvancedCodeXML);
      info.AddValue("Comments", (object) this.CommentsTxt);
      info.AddValue("Status", (object) this.Status);
      info.AddValue("LastModifiedByFullName", (object) this.LastModifiedByFullName);
      info.AddValue("LastModifiedByUserId", (object) this.LastModifiedByUserId);
      if (!(info is PackageSerializationInfo serializationInfo))
        return;
      if (this.MilestoneID != null)
        info.AddValue("Milestone", (object) serializationInfo.MilestoneIDToName(this.MilestoneID));
      if (this.RoleID <= 0)
        return;
      info.AddValue("Role", (object) serializationInfo.RoleIDToName(this.RoleID));
    }

    public int CompareTo(object obj)
    {
      if (!(obj is BizRuleInfo bizRuleInfo))
        throw new InvalidOperationException("The specified object type is invalid");
      if (this.Condition != bizRuleInfo.Condition)
        return this.Condition - bizRuleInfo.Condition;
      if ((this.Condition2 ?? "") != (bizRuleInfo.Condition2 ?? ""))
        return string.Compare(this.Condition2 ?? "", bizRuleInfo.Condition2 ?? "");
      if ((this.ConditionState ?? "") != (bizRuleInfo.ConditionState ?? ""))
        return string.Compare(this.ConditionState ?? "", bizRuleInfo.ConditionState ?? "");
      return (this.ConditionState2 ?? "") != (bizRuleInfo.ConditionState2 ?? "") ? string.Compare(this.ConditionState2 ?? "", bizRuleInfo.ConditionState2 ?? "") : 0;
    }

    public string ToDDMAdvanceCode()
    {
      string str1 = "";
      if ((this.Condition2 ?? "") != "" && this.Condition2 != "0,1,2,3,4")
      {
        if (this.Condition2.IndexOf("0") > -1)
          str1 = "[2626] = \"\"";
        if (this.Condition2.IndexOf("1") > -1)
          str1 = str1 + (str1 != "" ? " or " : "") + "[2626] = \"Banked - Retail\"";
        if (this.Condition2.IndexOf("2") > -1)
          str1 = str1 + (str1 != "" ? " or " : "") + "[2626] = \"Banked - Wholesale\"";
        if (this.Condition2.IndexOf("3") > -1)
          str1 = str1 + (str1 != "" ? " or " : "") + "[2626] = \"Brokered\"";
        if (this.Condition2.IndexOf("3") > -1)
          str1 = str1 + (str1 != "" ? " or " : "") + "[2626] = \"Correspondent\"";
      }
      string str2 = "";
      if (!this.IsGeneralRule)
      {
        if (this.Condition == BizRule.Condition.LoanPurpose)
          str2 = "[1172] = \"" + this.ConditionState + "\"";
        else if (this.Condition == BizRule.Condition.LoanType)
        {
          switch (this.ConditionState)
          {
            case "1":
              str2 = "[1172] = \"VA\"";
              break;
            case "2":
              str2 = "[1172] = \"Conventional\"";
              break;
            case "3":
              str2 = "[1172] = \"FHA\"";
              break;
            case "4":
              str2 = "[1172] = \"VA\"";
              break;
            case "5":
              str2 = "[1172] = \"FarmersHomeAdministration\"";
              break;
            case "6":
              str2 = "[1172] = \"HELOC\"";
              break;
            case "7":
              str2 = "[1172] = \"Other\"";
              break;
          }
        }
        else if (this.Condition == BizRule.Condition.LoanStatus)
          str2 = "[1172] = \"" + this.ConditionState + "\"";
        else if (this.Condition == BizRule.Condition.CurrentLoanAssocMS)
          str2 = "[DDM:MILESTONELOG:" + this.MilestoneID + ":" + (object) this.RoleID + "] = \"Y\"";
        else if (this.Condition == BizRule.Condition.RateLock)
          str2 = "[1172] = \"" + this.ConditionState + "\"";
        else if (this.Condition == BizRule.Condition.PropertyState)
          str2 = "[1172] = \"" + this.ConditionState + "\"";
        else if (this.Condition == BizRule.Condition.LoanDocType)
        {
          switch (this.ConditionState)
          {
            case "0":
              str2 = "[MORNET.X67] = \"\"";
              break;
            case "1":
              str2 = "[MORNET.X67] = \"Alternative\"";
              break;
            case "10":
              str2 = "[MORNET.X67] = \"NoIncomeOn1003\"";
              break;
            case "11":
              str2 = "[MORNET.X67] = \"NoVerificationOfStatedIncomeEmploymentOrAssets\"";
              break;
            case "12":
              str2 = "[MORNET.X67] = \"NoVerificationOfStatedIncomeOrAssets\"";
              break;
            case "13":
              str2 = "[MORNET.X67] = \"NoVerificationOfStatedAssets\"";
              break;
            case "14":
              str2 = "[MORNET.X67] = \"NoVerificationOfStatedIncomeOrEmployment\"";
              break;
            case "15":
              str2 = "[MORNET.X67] = \"NoVerificationOfStatedIncome\"";
              break;
            case "16":
              str2 = "[MORNET.X67] = \"VerbalVerificationOfEmployment\"";
              break;
            case "17":
              str2 = "[MORNET.X67] = \"OnePaystub\"";
              break;
            case "18":
              str2 = "[MORNET.X67] = \"Reduced\"";
              break;
            case "19":
              str2 = "[MORNET.X67] = \"OnePaystubAndVerbalVerificationOfEmployment\"";
              break;
            case "2":
              str2 = "[MORNET.X67] = \"StreamlineRefinance\"";
              break;
            case "20":
              str2 = "[MORNET.X67] = \"OnePaystubAndOneW2AndVerbalVerificationOfEmploymentOrOneYear1040\"";
              break;
            case "21":
              str2 = "[MORNET.X67] = \"NoIncomeNoEmploymentNoAssetsOn1003\"";
              break;
            case "3":
              str2 = "[MORNET.X67] = \"NoDocumentation\"";
              break;
            case "4":
              str2 = "[MORNET.X67] = \"NoRatio\"";
              break;
            case "5":
              str2 = "[MORNET.X67] = \"LimitedDocumentation\"";
              break;
            case "6":
              str2 = "[MORNET.X67] = \"FullDocumentation\"";
              break;
            case "7":
              str2 = "[MORNET.X67] = \"NoDepositVerificationEmploymentVerificationOrIncomeVerification\"";
              break;
            case "8":
              str2 = "[MORNET.X67] = \"NoDepositVerification\"";
              break;
            case "9":
              str2 = "[MORNET.X67] = \"NoEmploymentVerificationOrIncomeVerification\"";
              break;
          }
        }
        else if (this.Condition == BizRule.Condition.FinishedMilestone)
          str2 = "[DDM:FINISHEDMILESTONE:" + this.MilestoneID + "] = \"Y\"";
        else if (this.Condition == BizRule.Condition.AdvancedCoding)
          str2 = this.ConditionState;
        else if (this.Condition == BizRule.Condition.LoanProgram)
          str2 = "[1172] = \"" + this.ConditionState + "\"";
        else if (this.Condition == BizRule.Condition.PropertyType)
          str2 = "[1172] = \"" + this.ConditionState + "\"";
        else if (this.Condition == BizRule.Condition.Occupancy)
          str2 = "[1172] = \"" + this.ConditionState + "\"";
        else if (this.Condition == BizRule.Condition.TPOActions)
          str2 = "[1172] = \"" + this.ConditionState + "\"";
      }
      string str3 = "IF ([4000] = \"test\") THEN\r\n[4001] = [4000] + \"!!\"\r\nEND IF";
      string str4 = "";
      if (str1 != "")
        str4 = "IF (" + str1 + ") THEN\r\n";
      string ddmAdvanceCode;
      if (str2 != "")
        ddmAdvanceCode = str4 + (str1 != "" ? "    " : "") + "IF (" + str2 + ") THEN\r\n" + str3 + "\r\n" + (str1 != "" ? "    " : "") + "END IF\r\n";
      else
        ddmAdvanceCode = str4 + str3 + "\r\n";
      if (str1 != "")
        ddmAdvanceCode += "END IF";
      return ddmAdvanceCode;
    }
  }
}
