// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.LoanFolderRuleManager
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class LoanFolderRuleManager : ManagerBase
  {
    private IBpmManager loanFolderRuleMgr;

    internal static LoanFolderRuleManager Instance => Session.DefaultInstance.LoanFolderRuleManager;

    internal static void RefreshInstance()
    {
      if (Session.DefaultInstance == null)
        return;
      Session.DefaultInstance.ResetLoanFolderRuleManager();
    }

    internal LoanFolderRuleManager(Sessions.Session session)
      : base(session, ClientSessionCacheID.LoanFolderRule)
    {
      this.loanFolderRuleMgr = this.session.SessionObjects.BpmManager;
    }

    public LoanFolderRuleInfo[] GetFolderAccessRules()
    {
      LoanFolderRuleInfo[] subject = (LoanFolderRuleInfo[]) this.GetSubjectFromCache("rules");
      if (subject == null)
      {
        subject = this.session.SessionObjects.BpmManager.GetLoanFolderAccessRules();
        this.SetSubjectCache("rules", (object) subject);
      }
      return subject;
    }

    public void SetRule(LoanFolderRuleInfo rule)
    {
      this.ClearCache();
      this.loanFolderRuleMgr.SetLoanFolderAccessRule(rule);
    }

    public void DeleteRule(string loanFolder)
    {
      this.ClearCache();
      loanFolder = (loanFolder ?? "").Trim();
      this.loanFolderRuleMgr.DeleteLoanFolderAccessRule(loanFolder);
    }

    public LoanFolderRuleInfo GetRule(string loanFolder)
    {
      if (string.Compare(loanFolder, SystemSettings.TrashFolder, true) == 0)
        return new LoanFolderRuleInfo(SystemSettings.TrashFolder, false);
      foreach (LoanFolderRuleInfo folderAccessRule in this.GetFolderAccessRules())
      {
        if (string.Compare(folderAccessRule.LoanFolder, loanFolder, true) == 0)
          return folderAccessRule;
      }
      return new LoanFolderRuleInfo(loanFolder, true);
    }

    public string[] GetLoanFoldersForAction(LoanFolderAction action)
    {
      LoanFolderRuleInfo[] folderAccessRules = this.GetFolderAccessRules();
      Dictionary<string, bool> dictionary = new Dictionary<string, bool>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      foreach (LoanFolderRuleInfo loanFolderRuleInfo in folderAccessRules)
      {
        if (!loanFolderRuleInfo.IsActionAllowed(action))
          dictionary[loanFolderRuleInfo.LoanFolder] = false;
      }
      string[] allLoanFolderNames = this.session.LoanManager.GetAllLoanFolderNames(false);
      List<string> stringList = new List<string>();
      foreach (string key in allLoanFolderNames)
      {
        if (!dictionary.ContainsKey(key))
          stringList.Add(key);
      }
      return stringList.ToArray();
    }

    public bool CanMoveToFolder(
      string loanFolder,
      string[] finishedMilestones,
      LoanStatusMap.LoanStatus loanStatus)
    {
      LoanFolderRuleInfo rule = this.GetRule(loanFolder);
      if (rule.MoveRuleOption == BizRule.LoanFolderMoveRuleOption.None)
        return true;
      if (rule.MoveRuleOption == BizRule.LoanFolderMoveRuleOption.LoanStatus)
        return rule.GetLoanStatusSetting(loanStatus);
      for (int index = 0; index < finishedMilestones.Length; ++index)
      {
        if (finishedMilestones[index] == rule.MilestoneID)
          return true;
      }
      return false;
    }

    public bool CanMoveToFolder(string loanFolder, PipelineInfo pinfo)
    {
      if (this.session.UserInfo.IsSuperAdministrator())
        return true;
      ArrayList arrayList = new ArrayList();
      foreach (PipelineInfo.MilestoneInfo milestone in pinfo.Milestones)
      {
        if (milestone.Finished)
          arrayList.Add((object) milestone.MilestoneID);
      }
      LoanStatusMap.LoanStatus loanStatus = LoanStatusMap.LoanStatus.ActiveLoan;
      string str = string.Concat(pinfo.GetField("loanStatus"));
      if (str.Trim() != "")
      {
        try
        {
          loanStatus = (LoanStatusMap.LoanStatus) Convert.ToInt32(str);
        }
        catch
        {
        }
      }
      return this.CanMoveToFolder(loanFolder, (string[]) arrayList.ToArray(typeof (string)), loanStatus);
    }

    public bool IsActionPermitted(string folderName, LoanFolderAction action)
    {
      return this.session.UserInfo.IsSuperAdministrator() || this.GetRule(folderName).IsActionAllowed(action);
    }
  }
}
