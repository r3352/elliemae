// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.MilestoneTemplatesBpmManager
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.LoanUtils.Workflow;
using EllieMae.EMLite.Workflow;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class MilestoneTemplatesBpmManager : BpmManager
  {
    private MilestoneTemplatesManager mtMgr = new MilestoneTemplatesManager();

    internal static MilestoneTemplatesBpmManager Instance
    {
      get => Session.DefaultInstance.MilestoneTemplatesBpmManager;
    }

    internal static void RefreshInstance()
    {
      if (Session.DefaultInstance == null)
        return;
      Session.DefaultInstance.ResetMilestoneTemplatesBpmManager();
    }

    internal MilestoneTemplatesBpmManager(Sessions.Session session)
      : base(session, BizRuleType.MilestoneTemplateConditions, ClientSessionCacheID.MilestoneTemplateConditions)
    {
    }

    public List<string> GetSatisfiedMilestoneTemplate(LoanConditions loanConditions, LoanData loan)
    {
      return this.mtMgr.GetSatisfiedMilestoneTemplate(loanConditions, loan, this.session.UserInfo, this.session.SessionObjects);
    }

    public FieldRuleInfo[][] GetMilestoneTemplateConditions(
      LoanConditions loanConditions,
      LoanData loan)
    {
      return this.mtMgr.GetMilestoneTemplateConditions(loanConditions, loan, this.session.UserInfo, this.session.SessionObjects);
    }

    public IEnumerable<Milestone> GetAllMilestonesList() => this.GetAllMilestones();

    public IEnumerable<Milestone> GetAllActiveMilestonesList()
    {
      IEnumerable<Milestone> allMilestonesList = this.GetAllMilestonesList();
      List<Milestone> milestones = new List<Milestone>();
      allMilestonesList.ToList<Milestone>().ForEach((Action<Milestone>) (item =>
      {
        if (item.Archived)
          return;
        milestones.Add(item);
      }));
      return (IEnumerable<Milestone>) milestones.ToArray();
    }

    public Hashtable GetCompleteMilestoneGUID()
    {
      List<Milestone> list = this.GetAllMilestonesList().ToList<Milestone>();
      Hashtable completeMilestoneGuid = new Hashtable();
      for (int index = 0; index < list.Count; ++index)
      {
        if (!completeMilestoneGuid.ContainsKey((object) list[index].Name))
          completeMilestoneGuid.Add((object) list[index].Name, (object) list[index].MilestoneID);
      }
      return completeMilestoneGuid;
    }

    public Milestone GetMilestoneByID(
      string milestoneId,
      string milestoneName = "",
      bool archived = true,
      int defaultdays = 0,
      string descTextAfter = "",
      string descTextBefore = "",
      bool roleRequired = false,
      int roleID = -1)
    {
      Milestone milestoneById = this.GetAllMilestones().FirstOrDefault<Milestone>((Func<Milestone, bool>) (item => item.MilestoneID == milestoneId));
      if (milestoneById == null && milestoneName != "")
        milestoneById = new Milestone(milestoneId, -1, -1)
        {
          Archived = archived,
          DefaultDays = defaultdays,
          DescTextAfter = descTextAfter,
          DescTextBefore = descTextBefore,
          DisplayColor = Color.WhiteSmoke,
          Name = milestoneName,
          RoleRequired = roleRequired,
          RoleID = roleID
        };
      return milestoneById;
    }

    public Milestone GetMilestoneByName(string milestoneName)
    {
      return this.GetAllMilestones().FirstOrDefault<Milestone>((Func<Milestone, bool>) (item => item.Name == milestoneName));
    }

    public void ResetMilestones() => this.ResetMilestonesCache();
  }
}
