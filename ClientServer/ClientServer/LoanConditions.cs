// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.LoanConditions
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class LoanConditions
  {
    private BizRule.LoanType loanTypeValue;
    private BizRule.LoanPurpose loanPurposeValue;
    private BizRule.RateLock rateLockValue;
    private BizRule.LoanStatus loanStatusValue;
    private USPS.StateCode stateCodeValue;
    private LoanDocTypeMap.Code docTypeCodeValue;
    private string[] milestonesToCheck;
    private string[] finishedMilestones;
    private string[] finishedRoleIDs;
    private string currentMilestoneID;
    private string currentRoleID;
    private string loanProgramName;
    private bool dataFromPlinth;
    private string channelValue;
    private bool checkRoleOnly;

    public LoanConditions()
    {
    }

    public LoanConditions(
      BizRule.LoanType loanType,
      BizRule.LoanPurpose loanPurpose,
      BizRule.RateLock rateLock,
      BizRule.LoanStatus loanStatus,
      USPS.StateCode stateCode,
      LoanDocTypeMap.Code docTypeCode,
      string[] milestonesToCheck,
      string currentMilestoneID,
      string currentRoleID,
      string loanProgramName,
      string channelValue)
    {
      this.loanTypeValue = loanType;
      this.loanPurposeValue = loanPurpose;
      this.rateLockValue = rateLock;
      this.loanStatusValue = loanStatus;
      this.stateCodeValue = stateCode;
      this.docTypeCodeValue = docTypeCode;
      this.milestonesToCheck = milestonesToCheck;
      this.currentMilestoneID = currentMilestoneID;
      this.finishedMilestones = milestonesToCheck;
      this.checkRoleOnly = false;
      this.dataFromPlinth = false;
      this.currentRoleID = currentRoleID;
      this.loanProgramName = loanProgramName;
      this.channelValue = channelValue;
    }

    public BizRule.LoanType LoanTypeValue
    {
      get => this.loanTypeValue;
      set => this.loanTypeValue = value;
    }

    public BizRule.LoanPurpose LoanPurposeValue
    {
      get => this.loanPurposeValue;
      set => this.loanPurposeValue = value;
    }

    public BizRule.RateLock RateLockValue
    {
      get => this.rateLockValue;
      set => this.rateLockValue = value;
    }

    public BizRule.LoanStatus LoanStatusValue
    {
      get => this.loanStatusValue;
      set => this.loanStatusValue = value;
    }

    public USPS.StateCode StateCodeValue
    {
      get => this.stateCodeValue;
      set => this.stateCodeValue = value;
    }

    public LoanDocTypeMap.Code DocTypeCodeValue
    {
      get => this.docTypeCodeValue;
      set => this.docTypeCodeValue = value;
    }

    public string[] MilestonesToCheck
    {
      get => this.milestonesToCheck;
      set => this.milestonesToCheck = value;
    }

    public string[] FinishedMilestones
    {
      get => this.finishedMilestones;
      set => this.finishedMilestones = value;
    }

    public string[] FinishedRoleIDs
    {
      get => this.finishedRoleIDs;
      set => this.finishedRoleIDs = value;
    }

    public string CurrentMilestoneID
    {
      get => this.currentMilestoneID;
      set => this.currentMilestoneID = value;
    }

    public string CurrentRoleID
    {
      get => this.currentRoleID;
      set => this.currentRoleID = value;
    }

    public string LoanProgramName
    {
      get => this.loanProgramName;
      set => this.loanProgramName = value;
    }

    public bool DataFromPlinth
    {
      get => this.dataFromPlinth;
      set => this.dataFromPlinth = value;
    }

    public string ChannelValue
    {
      get
      {
        return (this.channelValue ?? "") == "" || this.channelValue.Trim() == "" ? "0" : this.channelValue;
      }
      set => this.channelValue = value;
    }

    public bool ContainMilestone(string milestoneID)
    {
      if (this.milestonesToCheck == null)
        return false;
      for (int index = 0; index < this.milestonesToCheck.Length; ++index)
      {
        if (milestoneID == this.milestonesToCheck[index])
          return true;
      }
      return false;
    }

    public bool CheckRoleOnly
    {
      get => this.checkRoleOnly;
      set => this.checkRoleOnly = value;
    }

    public string ToCacheKey()
    {
      string cacheKey = this.ChannelValue + ":" + this.loanTypeValue.ToString() + ":" + this.loanPurposeValue.ToString() + ":" + this.rateLockValue.ToString() + ":" + this.loanStatusValue.ToString() + ":" + this.stateCodeValue.ToString() + ":" + this.docTypeCodeValue.ToString() + ":" + this.currentMilestoneID + ":" + this.currentRoleID + ":" + this.loanProgramName;
      for (int index = 0; index < this.milestonesToCheck.Length; ++index)
        cacheKey = cacheKey + ":" + this.milestonesToCheck[index];
      return cacheKey;
    }

    public override bool Equals(object obj)
    {
      return obj is LoanConditions loanConditions && loanConditions.ToCacheKey() == this.ToCacheKey();
    }

    public override int GetHashCode() => this.ToCacheKey().GetHashCode();
  }
}
