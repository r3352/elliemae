// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.MilestoneWS
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.BusinessRules;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Customization;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.LoanServices;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class MilestoneWS : UserControl, IOnlineHelpTarget, IRefreshContents
  {
    private const string className = "MilestoneWS";
    private static readonly string sw = Tracing.SwDataEngine;
    private static Font bFont = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
    private static Font rFont = new Font("Arial", 7.8f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
    private MilestoneLog currentMilestoneLog;
    private int stageIndex;
    private int nextStageIndex;
    private MilestoneLog previousML;
    private DateTime milestoneUIDate = DateTime.Today;
    private MilestonesAclManager aclMgr;
    private WorkflowManager workflowMgr;
    private MilestoneLog[] mslogs;
    private int finishCheckPos;
    private int dateFieldPos;
    private bool hasRightsToAccept = true;
    private Panel panelRequiredFields;
    private Panel panelTasks;
    private Panel panelComments;
    private Panel panelDocumentTracking;
    private RequiredFieldsControl reqFieldControl;
    private RequiredTasksControl reqTasksControl;
    private RequiredDocumentsControl reqDocsControl;
    private ILoanEditor editor;
    private Label label1;
    private GroupContainer groupContainerComments;
    private CollapsibleSplitter collapsibleSplitter1;
    private TextBox boxCurrentLA;
    private BorderPanel panelAllComments;
    private BorderPanel borderPanel1;
    private StandardIconButton pictureBoxPreviousLA;
    private StandardIconButton pictureBoxNextLA;
    private StandardIconButton pictureBoxCurrentLA;
    private LogList logList;
    private Button dataTracBtn;
    private bool msRollback = true;
    private Label label2;
    private EllieMae.EMLite.Workflow.Milestone ms;
    private static Dictionary<string, Hashtable> cachedSecurityPermissions = new Dictionary<string, Hashtable>();
    private Sessions.Session session;
    private static Dictionary<string, Hashtable> cachedAssignMemberPermissions = new Dictionary<string, Hashtable>();
    private System.ComponentModel.Container components;
    private TextBox commentsBox;
    private MilestoneHeader topPanel;
    private Panel panel1;
    private CheckBox checkBoxFinished;
    private Panel panelDocList;
    private TextBox boxPreviousLA;
    private TextBox boxNextLA;
    private DateTimePicker dtPickerFinish;
    private Button showAllBtn;
    private Label labelDaysToFinish;
    private TextBox boxDays;
    private Button dataStampBtn;
    private Label labelPreviousLA;
    private Label labelCurrentLA;
    private Label labelNextLA;
    private Button acceptBtn;
    private Button returnBtn;
    private TextBox allCommentsBox;
    private PictureBox pictureBoxFindOver;
    private Panel panelDate;
    private Panel panelPrevious;
    private Panel panelCurrent;
    private Panel panelNext;
    private Panel panelLeft;
    private Panel panelRight;

    private Dictionary<string, Hashtable> cachedMilestonePermissions
    {
      get
      {
        return this.session.SessionObjects.CachedMilestonePermissions.ToDictionary<KeyValuePair<string, Hashtable>, string, Hashtable>((Func<KeyValuePair<string, Hashtable>, string>) (entry => entry.Key), (Func<KeyValuePair<string, Hashtable>, Hashtable>) (entry => (Hashtable) entry.Value.Clone()));
      }
    }

    private void enforceSecurity()
    {
      if (this.session.EncompassEdition == EncompassEdition.Banker)
      {
        if (this.session.StartupInfo.FastLoanLoad && MilestoneWS.cachedSecurityPermissions.Count == 0)
        {
          MilestoneWS.cachedSecurityPermissions = this.cachedMilestonePermissions;
          Tracing.Log(MilestoneWS.sw, TraceLevel.Info, nameof (MilestoneWS), "enforceSecurity check use fastloanload flag");
        }
        if (!MilestoneWS.cachedSecurityPermissions.ContainsKey(this.currentMilestoneLog.MilestoneID))
          MilestoneWS.cachedSecurityPermissions[this.currentMilestoneLog.MilestoneID] = this.aclMgr.CheckPermissions(new AclMilestone[6]
          {
            AclMilestone.AcceptFiles,
            AclMilestone.FinishMilestone,
            AclMilestone.ChangeExpectedDate,
            AclMilestone.ReturnFiles,
            AclMilestone.AssignLoanTeamMembers,
            AclMilestone.EditMilestoneComments
          }, this.currentMilestoneLog.MilestoneID);
        Hashtable securityPermission = MilestoneWS.cachedSecurityPermissions[this.currentMilestoneLog.MilestoneID];
        if (!(bool) securityPermission[(object) AclMilestone.AcceptFiles])
        {
          this.acceptBtn.Enabled = false;
          this.hasRightsToAccept = false;
        }
        if (!(bool) securityPermission[(object) AclMilestone.FinishMilestone])
          this.checkBoxFinished.Enabled = false;
        if (!(bool) securityPermission[(object) AclMilestone.ChangeExpectedDate])
        {
          this.dtPickerFinish.Enabled = false;
          this.boxDays.ReadOnly = true;
        }
        if (!(bool) securityPermission[(object) AclMilestone.ReturnFiles])
          this.returnBtn.Enabled = false;
        if (!(bool) securityPermission[(object) AclMilestone.EditMilestoneComments])
        {
          this.commentsBox.Enabled = false;
          this.dataStampBtn.Enabled = false;
        }
      }
      FeaturesAclManager aclManager = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
      if (aclManager.GetUserApplicationRight(AclFeature.eFolder_AccessToDocumentTab))
        this.reqDocsControl.SetButtonStatus(true);
      else
        this.reqDocsControl.SetButtonStatus(false);
      if (!aclManager.GetUserApplicationRight(AclFeature.ToolsTab_Task))
        this.reqTasksControl.Editable = false;
      else if ((this.session.LoanData.ContentAccess & LoanContentAccess.Task) != LoanContentAccess.Task && this.session.LoanData.ContentAccess != LoanContentAccess.FullAccess)
        this.reqTasksControl.Editable = false;
      else if (!aclManager.GetUserApplicationRight(AclFeature.ToolsTab_Task_Edit))
        this.reqTasksControl.Editable = false;
      if (this.session.EncompassEdition == EncompassEdition.Banker && !this.msRollback && this.currentMilestoneLog.Done && this.checkBoxFinished.Enabled)
      {
        for (int nextStageIndex = this.nextStageIndex; nextStageIndex < this.logList.GetNumberOfMilestones(); ++nextStageIndex)
        {
          MilestoneLog milestoneAt = this.logList.GetMilestoneAt(nextStageIndex);
          if (milestoneAt.Done)
          {
            Hashtable hashtable = new Hashtable();
            bool flag = this.session.StartupInfo.FastLoanLoad;
            if (flag)
            {
              if (MilestoneWS.cachedSecurityPermissions.ContainsKey(milestoneAt.MilestoneID))
                hashtable = MilestoneWS.cachedSecurityPermissions[milestoneAt.MilestoneID];
              else
                flag = false;
            }
            if (!flag)
              hashtable = this.aclMgr.CheckPermissions(new AclMilestone[1], milestoneAt.MilestoneID);
            if (!(bool) hashtable[(object) AclMilestone.FinishMilestone])
            {
              this.checkBoxFinished.Enabled = false;
              break;
            }
          }
          else
            break;
        }
      }
      List<string> stringList = new List<string>();
      foreach (MilestoneLog allMilestone in this.logList.GetAllMilestones())
        stringList.Add(allMilestone.MilestoneID);
      Dictionary<string, AclTriState> dictionary = new Dictionary<string, AclTriState>();
      if (this.session.StartupInfo.FastLoanLoad)
      {
        foreach (string key in stringList)
        {
          if (MilestoneWS.cachedSecurityPermissions.ContainsKey(key))
          {
            Hashtable hashtable = (Hashtable) null;
            if (MilestoneWS.cachedAssignMemberPermissions.ContainsKey(key))
              hashtable = MilestoneWS.cachedAssignMemberPermissions[key];
            if (hashtable != null && hashtable.ContainsKey((object) AclMilestone.ChangeExpectedDate))
            {
              dictionary.Add(key, (bool) hashtable[(object) AclMilestone.ChangeExpectedDate] ? AclTriState.True : AclTriState.False);
            }
            else
            {
              Dictionary<string, AclTriState> permissionFromUser = ChangeMilestoneDates.GetPermissionFromUser(AclMilestone.ChangeExpectedDate, this.session, new string[1]
              {
                key
              });
              if (permissionFromUser != null && permissionFromUser.Count > 0)
                dictionary.Add(key, permissionFromUser[key]);
            }
          }
          else
            break;
        }
      }
      else
        dictionary = ChangeMilestoneDates.GetPermissionFromUser(AclMilestone.ChangeExpectedDate, this.session, stringList.ToArray());
      this.label2.Visible = dictionary.ContainsValue(AclTriState.True);
      if (!this.label2.Visible)
        return;
      this.label2.Location = new Point(this.dtPickerFinish.Location.X + 145, this.dtPickerFinish.Location.Y + 2);
    }

    public MilestoneWS(Sessions.Session session, MilestoneLog currentMilestoneLog)
    {
      this.session = session;
      this.ms = ((MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).GetMilestoneByID(currentMilestoneLog.MilestoneID, currentMilestoneLog.Stage, false, currentMilestoneLog.Days, currentMilestoneLog.DoneText, currentMilestoneLog.ExpText);
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      this.dtPickerFinish.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      AutoDayCountSetting policySetting = (AutoDayCountSetting) this.session.StartupInfo.PolicySettings[(object) "Policies.MilestoneExpDayCount"];
      if (this.session.IsBankerEdition() && (EnableDisableSetting) this.session.StartupInfo.PolicySettings[(object) "Policies.MilestoneRollback"] != EnableDisableSetting.Enabled)
        this.msRollback = false;
      this.finishCheckPos = this.checkBoxFinished.Top;
      this.dateFieldPos = this.panelDate.Top;
      this.editor = Session.Application.GetService<ILoanEditor>();
      this.workflowMgr = (WorkflowManager) this.session.BPM.GetBpmManager(BpmCategory.Workflow);
      this.aclMgr = (MilestonesAclManager) this.session.ACL.GetAclManager(AclCategory.Milestones);
      this.logList = this.session.LoanData.GetLogList();
      this.currentMilestoneLog = currentMilestoneLog;
      this.loadContent();
    }

    public void UnRegisterEvent()
    {
    }

    public MilestoneWS(
      Sessions.Session session,
      MilestoneLog currentMilestoneLog,
      MilestoneHistoryLog milestoneHistoryLog)
    {
      this.session = session;
      this.ms = ((MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).GetMilestoneByID(currentMilestoneLog.MilestoneID, currentMilestoneLog.Stage, false, currentMilestoneLog.Days, currentMilestoneLog.DoneText, currentMilestoneLog.ExpText);
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      this.dtPickerFinish.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      AutoDayCountSetting policySetting = (AutoDayCountSetting) this.session.StartupInfo.PolicySettings[(object) "Policies.MilestoneExpDayCount"];
      if (this.session.IsBankerEdition() && (EnableDisableSetting) this.session.StartupInfo.PolicySettings[(object) "Policies.MilestoneRollback"] != EnableDisableSetting.Enabled)
        this.msRollback = false;
      this.finishCheckPos = this.checkBoxFinished.Top;
      this.dateFieldPos = this.panelDate.Top;
      this.editor = Session.Application.GetService<ILoanEditor>();
      this.workflowMgr = (WorkflowManager) this.session.BPM.GetBpmManager(BpmCategory.Workflow);
      this.aclMgr = (MilestonesAclManager) this.session.ACL.GetAclManager(AclCategory.Milestones);
      this.logList = this.session.LoanData.GetLogList();
      this.currentMilestoneLog = currentMilestoneLog;
      this.loadContentHistory(milestoneHistoryLog);
    }

    private void loadContentHistory(MilestoneHistoryLog milestoneHistoryLog)
    {
      this.refreshAllComments(false, true);
      this.refreshHeader();
      this.checkBoxFinished.Checked = this.currentMilestoneLog.Done;
      this.commentsBox.Text = this.currentMilestoneLog.Comments;
      this.stageIndex = milestoneHistoryLog.Milestones.IndexOf(this.currentMilestoneLog);
      this.nextStageIndex = this.stageIndex == milestoneHistoryLog.Milestones.Count - 1 ? this.stageIndex : this.stageIndex + 1;
      this.previousML = (MilestoneLog) null;
      if (this.stageIndex > 0)
        this.previousML = milestoneHistoryLog.Milestones[this.stageIndex - 1];
      this.mslogs = milestoneHistoryLog.Milestones.ToArray();
      this.setMilestoneUIDate();
      if (this.currentMilestoneLog.Stage == "Started")
      {
        this.panelLeft.Visible = false;
        this.collapsibleSplitter1.Visible = false;
        this.panelTasks.Visible = false;
        this.labelDaysToFinish.Visible = false;
        this.boxDays.Visible = false;
        this.dtPickerFinish.Left = 0;
        this.checkBoxFinished.Visible = false;
        this.labelPreviousLA.Visible = false;
        this.boxPreviousLA.Visible = false;
        this.pictureBoxPreviousLA.Visible = false;
        this.labelNextLA.Top = this.labelCurrentLA.Top;
        this.boxNextLA.Top = this.boxCurrentLA.Top;
        this.pictureBoxNextLA.Top = this.pictureBoxCurrentLA.Top;
        this.labelCurrentLA.Top = this.labelPreviousLA.Top;
        this.boxCurrentLA.Top = this.boxPreviousLA.Top;
        this.pictureBoxCurrentLA.Visible = false;
        this.panelComments.Dock = DockStyle.Fill;
      }
      else
      {
        if (this.currentMilestoneLog.Stage == "Completion")
        {
          this.labelNextLA.Visible = false;
          this.boxNextLA.Visible = false;
          this.pictureBoxNextLA.Visible = false;
        }
        this.checkBoxFinished.Checked = this.currentMilestoneLog.Done;
      }
      if (this.session.EncompassEdition == EncompassEdition.Broker)
        this.panelRequiredFields.Visible = false;
      this.refreshRoles();
      this.moveRoleFields();
      Hashtable loanMilestones = new Hashtable();
      for (int index = 0; index < this.mslogs.Length; ++index)
        loanMilestones.Add((object) this.mslogs[index].MilestoneID, (object) this.mslogs[index].Stage);
      this.panelDocumentTracking.Controls.Clear();
      this.reqDocsControl = new RequiredDocumentsControl(this.currentMilestoneLog);
      this.panelDocumentTracking.Controls.Add((Control) this.reqDocsControl);
      this.panelRequiredFields.Controls.Clear();
      this.reqFieldControl = new RequiredFieldsControl(this.session, this.currentMilestoneLog, ((MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).GetMilestoneByID(this.currentMilestoneLog.MilestoneID), loanMilestones);
      this.panelRequiredFields.Controls.Add((Control) this.reqFieldControl);
      this.panelTasks.Controls.Clear();
      this.reqTasksControl = new RequiredTasksControl();
      this.panelTasks.Controls.Add((Control) this.reqTasksControl);
      this.refreshRequiredDocumentsAndFieldsHistory(milestoneHistoryLog, false);
      this.enforceSecurity();
      bool up = false;
      if (this.currentMilestoneLog.Stage == "Started" || !this.acceptBtn.Enabled && !this.returnBtn.Enabled)
        up = true;
      if (this.currentMilestoneLog.Done)
        up = true;
      else if (this.previousML != null && !this.previousML.Done)
        up = true;
      if (this.previousML != null && this.currentMilestoneLog.LoanAssociateID == this.previousML.LoanAssociateID)
      {
        this.acceptBtn.Enabled = this.returnBtn.Enabled = false;
        up = true;
      }
      this.moveComponents(up);
    }

    private void loadContent()
    {
      this.checkBoxFinished.CheckedChanged -= new EventHandler(this.checkBoxFinished_CheckedChanged);
      this.commentsBox.TextChanged -= new EventHandler(this.commentsBox_TextChanged);
      this.boxDays.TextChanged -= new EventHandler(this.boxDays_TextChanged);
      this.dtPickerFinish.Validated -= new EventHandler(this.dtPickerFinish_Validated);
      this.refreshAllComments(false, true);
      this.refreshHeader();
      this.checkBoxFinished.Checked = this.currentMilestoneLog.Done;
      this.commentsBox.Text = this.currentMilestoneLog.Comments;
      this.stageIndex = this.logList.GetMilestoneIndex(this.currentMilestoneLog.Stage);
      this.nextStageIndex = this.stageIndex == this.logList.GetNumberOfMilestones() - 1 ? this.stageIndex : this.stageIndex + 1;
      this.previousML = (MilestoneLog) null;
      if (this.stageIndex > 0)
        this.previousML = this.logList.GetMilestoneAt(this.stageIndex - 1);
      this.mslogs = this.session.LoanDataMgr.LoanData.GetLogList().GetAllMilestones();
      this.setMilestoneUIDate();
      if (this.currentMilestoneLog.Stage == "Started")
      {
        this.panelLeft.Visible = false;
        this.collapsibleSplitter1.Visible = false;
        this.panelTasks.Visible = false;
        this.labelDaysToFinish.Visible = false;
        this.boxDays.Visible = false;
        this.dtPickerFinish.Left = 0;
        this.checkBoxFinished.Visible = false;
        this.labelPreviousLA.Visible = false;
        this.boxPreviousLA.Visible = false;
        this.pictureBoxPreviousLA.Visible = false;
        this.labelNextLA.Top = this.labelCurrentLA.Top;
        this.boxNextLA.Top = this.boxCurrentLA.Top;
        this.pictureBoxNextLA.Top = this.pictureBoxCurrentLA.Top;
        this.labelCurrentLA.Top = this.labelPreviousLA.Top;
        this.boxCurrentLA.Top = this.boxPreviousLA.Top;
        this.pictureBoxCurrentLA.Visible = false;
        this.panelComments.Dock = DockStyle.Fill;
      }
      else
      {
        if (this.currentMilestoneLog.Stage == "Completion")
        {
          this.labelNextLA.Visible = false;
          this.boxNextLA.Visible = false;
          this.pictureBoxNextLA.Visible = false;
        }
        this.checkBoxFinished.Checked = this.currentMilestoneLog.Done;
      }
      if (this.session.EncompassEdition == EncompassEdition.Broker)
        this.panelRequiredFields.Visible = false;
      this.refreshRoles();
      if (this.panelPrevious.Visible)
        this.setPermission(this.stageIndex - 1, (PictureBox) this.pictureBoxPreviousLA);
      if (this.panelCurrent.Visible)
      {
        if (this.currentMilestoneLog.Stage == "Started")
          this.setPermission(this.stageIndex + 1, (PictureBox) this.pictureBoxCurrentLA);
        else
          this.setPermission(this.stageIndex, (PictureBox) this.pictureBoxCurrentLA);
      }
      if (this.panelNext.Visible)
        this.setPermission(this.stageIndex + 1, (PictureBox) this.pictureBoxNextLA);
      this.moveRoleFields();
      Hashtable loanMilestones = new Hashtable();
      for (int index = 0; index < this.mslogs.Length; ++index)
        loanMilestones.Add((object) this.mslogs[index].MilestoneID, (object) this.mslogs[index].Stage);
      this.panelDocumentTracking.Controls.Clear();
      this.reqDocsControl = new RequiredDocumentsControl(this.currentMilestoneLog, loanMilestones);
      this.panelDocumentTracking.Controls.Add((Control) this.reqDocsControl);
      this.panelRequiredFields.Controls.Clear();
      this.reqFieldControl = new RequiredFieldsControl(this.session, this.currentMilestoneLog, ((MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).GetMilestoneByID(this.currentMilestoneLog.MilestoneID), loanMilestones);
      this.panelRequiredFields.Controls.Add((Control) this.reqFieldControl);
      this.panelTasks.Controls.Clear();
      this.reqTasksControl = new RequiredTasksControl(this.logList, this.currentMilestoneLog, loanMilestones);
      this.panelTasks.Controls.Add((Control) this.reqTasksControl);
      this.refreshRequiredDocumentsAndFields(false);
      this.enforceSecurity();
      bool up = false;
      if (this.currentMilestoneLog.Stage == "Started" || !this.acceptBtn.Enabled && !this.returnBtn.Enabled)
        up = true;
      if (this.currentMilestoneLog.Done)
        up = true;
      else if (this.previousML != null && !this.previousML.Done)
        up = true;
      if (this.previousML != null && this.currentMilestoneLog.LoanAssociateID == this.previousML.LoanAssociateID)
      {
        this.acceptBtn.Enabled = this.returnBtn.Enabled = false;
        up = true;
      }
      this.moveComponents(up);
      this.checkBoxFinished.CheckedChanged += new EventHandler(this.checkBoxFinished_CheckedChanged);
      this.commentsBox.TextChanged += new EventHandler(this.commentsBox_TextChanged);
      this.boxDays.TextChanged += new EventHandler(this.boxDays_TextChanged);
      this.dtPickerFinish.Validated += new EventHandler(this.dtPickerFinish_Validated);
    }

    private void refreshRoles()
    {
      this.setPreviousRole();
      this.setCurrentRole();
      this.setNextRole();
    }

    private void setMilestoneUIDate()
    {
      if (this.currentMilestoneLog.Date != DateTime.MinValue && this.currentMilestoneLog.Date != DateTime.MaxValue)
      {
        this.dtPickerFinish.Value = this.currentMilestoneLog.Date;
        this.boxDays.Text = this.getMilestoneCountdown(this.currentMilestoneLog.Date);
      }
      else if (this.previousML != null)
      {
        int dayCount = 0;
        for (int index = 1; index <= this.stageIndex; ++index)
          dayCount += this.mslogs[index].Days;
        this.dtPickerFinish.Value = Session.Application.GetService<ILoanEditor>().AddDays(DateTime.Today, dayCount);
        this.boxDays.Text = this.getMilestoneCountdown(this.dtPickerFinish.Value);
      }
      this.milestoneUIDate = this.dtPickerFinish.Value;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void moveComponents(bool up)
    {
      this.checkBoxFinished.Top = this.finishCheckPos;
      this.panelDate.Top = this.dateFieldPos;
      if (up)
      {
        this.acceptBtn.Visible = false;
        this.returnBtn.Visible = false;
        this.checkBoxFinished.Top = this.panelDate.Top;
        this.panelDate.Top = this.acceptBtn.Top;
      }
      else
      {
        this.acceptBtn.Visible = true;
        this.returnBtn.Visible = true;
        this.checkBoxFinished.Top = this.finishCheckPos;
        this.panelDate.Top = this.dateFieldPos;
      }
    }

    private void setPreviousRole()
    {
      int previousMilestone = this.getPreviousMilestone(this.stageIndex);
      if (previousMilestone == -1)
      {
        this.panelPrevious.Visible = false;
      }
      else
      {
        MilestoneLog mslog = this.mslogs[previousMilestone];
        RoleInfo roleInfo = (RoleInfo) null;
        if (string.Compare(mslog.RoleName, "File Starter", true) == 0)
        {
          this.pictureBoxPreviousLA.Visible = false;
          this.labelPreviousLA.Text = "File Started By";
        }
        else
        {
          roleInfo = this.session.LoanDataMgr.SystemConfiguration.GetRoleFunction(mslog.RoleID);
          if (roleInfo != null)
            this.labelPreviousLA.Text = roleInfo.Name;
          else
            this.labelPreviousLA.Text = mslog.RoleName;
        }
        if (mslog.LoanAssociateType == LoanAssociateType.User)
          this.boxPreviousLA.Text = string.Format("{0} ({1})", (object) mslog.LoanAssociateName, (object) mslog.LoanAssociateID);
        else if (mslog.LoanAssociateType == LoanAssociateType.Group)
          this.boxPreviousLA.Text = string.Format("{0} (Group)", (object) mslog.LoanAssociateName);
        else
          this.boxPreviousLA.Text = string.Empty;
        this.boxPreviousLA.Tag = (object) this.mslogs[previousMilestone];
        this.pictureBoxPreviousLA.Tag = (object) roleInfo;
      }
    }

    private void setCurrentRole()
    {
      if (this.mslogs[this.stageIndex].RoleID == -1)
      {
        this.panelCurrent.Visible = false;
      }
      else
      {
        RoleInfo roleFunction = this.session.LoanDataMgr.SystemConfiguration.GetRoleFunction(this.mslogs[this.stageIndex].RoleID);
        if (roleFunction != null)
          this.labelCurrentLA.Text = roleFunction.Name;
        else
          this.labelCurrentLA.Text = this.mslogs[this.stageIndex].RoleName;
        if (this.mslogs[this.stageIndex].LoanAssociateType == LoanAssociateType.User)
          this.boxCurrentLA.Text = string.Format("{0} ({1})", (object) this.mslogs[this.stageIndex].LoanAssociateName, (object) this.mslogs[this.stageIndex].LoanAssociateID);
        else if (this.mslogs[this.stageIndex].LoanAssociateType == LoanAssociateType.Group)
          this.boxCurrentLA.Text = string.Format("{0} (Group)", (object) this.mslogs[this.stageIndex].LoanAssociateName);
        else
          this.boxCurrentLA.Text = string.Empty;
        this.boxCurrentLA.Tag = (object) this.mslogs[this.stageIndex];
        this.pictureBoxCurrentLA.Tag = (object) roleFunction;
      }
    }

    private void setNextRole()
    {
      if (this.stageIndex + 1 >= this.mslogs.Length)
      {
        this.boxNextLA.Visible = false;
        this.pictureBoxNextLA.Visible = false;
      }
      else
      {
        RoleInfo roleFunction = this.session.LoanDataMgr.SystemConfiguration.GetRoleFunction(this.mslogs[this.stageIndex + 1].RoleID);
        if (roleFunction != null)
        {
          this.labelNextLA.Text = roleFunction.Name;
        }
        else
        {
          if (this.mslogs[this.stageIndex + 1].RoleName == string.Empty)
          {
            this.panelNext.Visible = false;
            return;
          }
          this.labelNextLA.Text = this.mslogs[this.stageIndex + 1].RoleName;
        }
        if (this.mslogs[this.stageIndex + 1].LoanAssociateType == LoanAssociateType.User)
          this.boxNextLA.Text = string.Format("{0} ({1})", (object) this.mslogs[this.stageIndex + 1].LoanAssociateName, (object) this.mslogs[this.stageIndex + 1].LoanAssociateID);
        else if (this.mslogs[this.stageIndex + 1].LoanAssociateType == LoanAssociateType.Group)
          this.boxNextLA.Text = string.Format("{0} (Group)", (object) this.mslogs[this.stageIndex + 1].LoanAssociateName);
        else
          this.boxNextLA.Text = string.Empty;
        this.boxNextLA.Tag = (object) this.mslogs[this.stageIndex + 1];
        this.pictureBoxNextLA.Tag = (object) roleFunction;
      }
    }

    private int getPreviousMilestone(int current)
    {
      if (current <= 0)
        return -1;
      for (int previousMilestone = current - 1; previousMilestone >= 0; --previousMilestone)
      {
        if (this.mslogs[previousMilestone].RoleID != -1)
          return previousMilestone;
      }
      return -1;
    }

    private void moveRoleFields()
    {
      if (!this.panelPrevious.Visible)
      {
        this.panelNext.Top = this.panelCurrent.Top;
        this.panelCurrent.Top = this.panelPrevious.Top;
      }
      if (this.panelCurrent.Visible)
        return;
      this.panelNext.Top = this.panelCurrent.Top;
    }

    private void setPermission(int msIndex, PictureBox picButton)
    {
      if (this.session.StartupInfo.FastLoanLoad && MilestoneWS.cachedAssignMemberPermissions.Count == 0)
      {
        MilestoneWS.cachedAssignMemberPermissions = this.cachedMilestonePermissions;
        Tracing.Log(MilestoneWS.sw, TraceLevel.Info, nameof (MilestoneWS), "setPermission using FastLoanLoad flag.");
      }
      if (msIndex < 0 || msIndex >= this.mslogs.Length)
        return;
      MilestoneLog mslog = this.mslogs[msIndex];
      if (!MilestoneWS.cachedAssignMemberPermissions.ContainsKey(mslog.MilestoneID))
        MilestoneWS.cachedAssignMemberPermissions[mslog.MilestoneID] = this.aclMgr.CheckPermissions(new AclMilestone[1]
        {
          AclMilestone.AssignLoanTeamMembers
        }, mslog.MilestoneID);
      Hashtable memberPermission = MilestoneWS.cachedAssignMemberPermissions[mslog.MilestoneID];
      if (memberPermission.ContainsKey((object) AclMilestone.AssignLoanTeamMembers) && (bool) memberPermission[(object) AclMilestone.AssignLoanTeamMembers])
        picButton.Enabled = true;
      else
        picButton.Enabled = false;
    }

    private void commentsBox_TextChanged(object sender, EventArgs e)
    {
      this.currentMilestoneLog.Comments = this.commentsBox.Text;
    }

    private void refreshHeader()
    {
      DateTime dateTime = this.currentMilestoneLog.Date;
      DateTime date1 = dateTime.Date;
      dateTime = DateTime.MaxValue;
      DateTime date2 = dateTime.Date;
      string str1;
      if (date1 != date2)
      {
        DateTime date3 = this.currentMilestoneLog.Date;
        if (date3.Date == DateTime.Today)
        {
          str1 = this.currentMilestoneLog.Done ? " today" : " today!";
        }
        else
        {
          date3 = this.currentMilestoneLog.Date;
          date3 = date3.Date;
          str1 = " on " + date3.ToString("MM/dd/yy");
        }
        DateTime today = DateTime.Today;
        date3 = this.currentMilestoneLog.Date;
        DateTime date4 = date3.Date;
        int days = (today - date4).Days;
        if (!this.currentMilestoneLog.Done)
        {
          if (days > 0)
            str1 = str1 + " (" + (object) days + " day(s) ago)!";
          else if (days < 0)
          {
            int num = -days;
            str1 = str1 + " (in " + (object) num + " day(s))";
          }
        }
        else if (this.dtPickerFinish.Value != this.currentMilestoneLog.Date)
        {
          this.dtPickerFinish.Value = this.currentMilestoneLog.Date;
          this.boxDays.Text = this.getMilestoneCountdown(this.dtPickerFinish.Value);
        }
      }
      else
        str1 = string.Empty;
      string str2;
      if (this.currentMilestoneLog.Done)
      {
        str2 = this.currentMilestoneLog.DoneText + str1;
      }
      else
      {
        str2 = this.currentMilestoneLog.Stage + " Worksheet ";
        if ((this.currentMilestoneLog.LoanAssociateName ?? "") != string.Empty)
          str2 = str2 + " for " + this.currentMilestoneLog.LoanAssociateName;
      }
      if (this.isMilestoneInAlertState())
        this.topPanel.BackColor = AppColors.AlertRed;
      else
        this.topPanel.BackColor = this.ms.DisplayColor;
      this.label1.ForeColor = this.ContrastColor(this.topPanel.BackColor);
      this.label1.Text = str2;
      this.acceptBtn.Enabled = !this.currentMilestoneLog.Reviewed && this.isAllowedToAccept();
      if (!(this.currentMilestoneLog.Stage == "submittal"))
        return;
      this.dataTracBtn.Visible = LoanServiceManager.ShowExportToDataTracButton();
    }

    private Color ContrastColor(Color color)
    {
      int maxValue = 1.0 - (0.299 * (double) color.R + 0.587 * (double) color.G + 0.114 * (double) color.B) / (double) byte.MaxValue >= 0.5 ? (int) byte.MaxValue : 0;
      return Color.FromArgb(maxValue, maxValue, maxValue);
    }

    private bool isMilestoneInAlertState()
    {
      foreach (PipelineInfo.Alert usersPipelineAlert in this.session.LoanDataMgr.AccessRules.GetUsersPipelineAlerts())
      {
        if (usersPipelineAlert.LogRecordID == this.currentMilestoneLog.Guid)
          return true;
      }
      return false;
    }

    private bool isAllowedToAccept() => this.hasRightsToAccept;

    private void checkBoxFinished_CheckedChanged(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      if (this.checkBoxFinished.Checked)
      {
        if (sender != null)
        {
          if (this.nextStageIndex < this.mslogs.Length && this.mslogs[this.nextStageIndex].RoleID > -1)
          {
            bool flag = false;
            if ((this.mslogs[this.nextStageIndex].RoleRequired ?? "") == string.Empty)
            {
              EllieMae.EMLite.Workflow.Milestone milestoneById = ((MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).GetMilestoneByID(this.currentMilestoneLog.MilestoneID, this.currentMilestoneLog.Stage, false, this.currentMilestoneLog.Days, this.currentMilestoneLog.DoneText, this.currentMilestoneLog.ExpText);
              if (milestoneById != null && milestoneById.RoleRequired)
                flag = true;
            }
            else if (this.mslogs[this.nextStageIndex].RoleRequired == "Y")
              flag = true;
            if (flag && this.boxNextLA.Text.Trim() == string.Empty && this.boxNextLA.Visible)
            {
              this.uncheckFinishMilestone();
              int num = (int) Utils.Dialog((IWin32Window) this, "Assign a loan team member for the next milestone prior to finishing this one.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return;
            }
          }
          if (this.hasBlankPreviousRole())
          {
            this.uncheckFinishMilestone();
            return;
          }
          if (!this.hasPermissionToFinishMilestones())
          {
            this.uncheckFinishMilestone();
            return;
          }
          if (!this.businessRulesPassed())
          {
            this.uncheckFinishMilestone();
            this.RefreshTitle(true);
            return;
          }
        }
        try
        {
          if (this.logList.MSDateLock)
          {
            this.currentMilestoneLog.AdjustCorrectionDatesOnly(DateTime.Now);
            RemoteLogger.Write(TraceLevel.Info, string.Format("MilestoneWS.cs: Getting the completed date for the {0}. logList.MSDateLock={1}, AdjustCorrectionDatesOnly({2}), Milestone.Date={3}, ClientID={4}, ServerInstanceName={5}", (object) this.currentMilestoneLog.Stage, (object) this.logList.MSDateLock, (object) DateTime.Now, (object) this.currentMilestoneLog.Date, (object) this.session.SessionObjects.StartupInfo.CompanyInfo.ClientID, this.session.ServerIdentity == null ? (object) "Local" : (object) this.session.ServerIdentity.InstanceName));
          }
          else
          {
            DateTime adjustedCompletionDate = this.getAdjustedCompletionDate(DateTime.Now);
            this.currentMilestoneLog.AdjustDate(adjustedCompletionDate, true, true);
            RemoteLogger.Write(TraceLevel.Info, string.Format("MilestoneWS.cs: Getting the completed date for the {0}. logList.MSDateLock={1}, getAdjustedCompletionDate={2}, Milestone.Date={3}, ClientID={4}, ServerInstanceName={5}", (object) this.currentMilestoneLog.Stage, (object) this.logList.MSDateLock, (object) adjustedCompletionDate, (object) this.currentMilestoneLog.Date, (object) this.session.SessionObjects.StartupInfo.CompanyInfo.ClientID, this.session.ServerIdentity == null ? (object) "Local" : (object) this.session.ServerIdentity.InstanceName));
          }
          this.editor.SetMilestoneStatus(this.currentMilestoneLog, this.stageIndex, true);
          this.session.LoanDataMgr.LoanData.Calculator.UpdateLenderRepresentative((LoanAssociateLog) this.mslogs[this.nextStageIndex], "all");
        }
        catch (ActionCanceledException ex)
        {
          RemoteLogger.Write(TraceLevel.Error, nameof (MilestoneWS), string.Format("ActionCanceledException thrown in MilestoneWS.cs from the ClientID={0} on Encompass ServerInstanceName={1}. Current Milestone={2}. Next Stage={3}. Logged in user={4}.", (object) this.session.SessionObjects.StartupInfo.CompanyInfo.ClientID, this.session.ServerIdentity == null ? (object) "Local" : (object) this.session.ServerIdentity.InstanceName, (object) this.currentMilestoneLog.Stage, (object) this.mslogs[this.nextStageIndex].Stage, (object) this.session.UserInfo.Userid), (Exception) ex);
          this.uncheckFinishMilestone();
          this.RefreshTitle(true);
          return;
        }
        catch (ValidationException ex)
        {
          RemoteLogger.Write(TraceLevel.Error, nameof (MilestoneWS), string.Format("ValidationException thrown in MilestoneWS.cs from the ClientID={0} on Encompass ServerInstanceName={1}. Current Milestone={2}. Next Stage={3}. Logged in user={4}.", (object) this.session.SessionObjects.StartupInfo.CompanyInfo.ClientID, this.session.ServerIdentity == null ? (object) "Local" : (object) this.session.ServerIdentity.InstanceName, (object) this.currentMilestoneLog.Stage, (object) this.mslogs[this.nextStageIndex].Stage, (object) this.session.UserInfo.Userid), (Exception) ex);
          int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.uncheckFinishMilestone();
          this.RefreshTitle(true);
          return;
        }
        catch (Exception ex)
        {
          RemoteLogger.Write(TraceLevel.Error, nameof (MilestoneWS), string.Format("Exception thrown in MilestoneWS.cs from the ClientID={0} on Encompass ServerInstanceName={1}. Current Milestone={2}. Next Stage={3}. Logged in user={4}.", (object) this.session.SessionObjects.StartupInfo.CompanyInfo.ClientID, this.session.ServerIdentity == null ? (object) "Local" : (object) this.session.ServerIdentity.InstanceName, (object) this.currentMilestoneLog.Stage, (object) this.mslogs[this.nextStageIndex].Stage, (object) this.session.UserInfo.Userid), ex);
          this.uncheckFinishMilestone();
          this.RefreshTitle(true);
          return;
        }
        if (this.currentMilestoneLog.Date != DateTime.MinValue && this.currentMilestoneLog.Date != DateTime.MaxValue)
          this.setMilestoneUIDate();
        this.moveComponents(true);
      }
      else
      {
        try
        {
          this.editor.SetMilestoneStatus(this.currentMilestoneLog, this.stageIndex, false);
          this.session.LoanDataMgr.LoanData.Calculator.UpdateLenderRepresentative((LoanAssociateLog) this.currentMilestoneLog, "all");
        }
        catch (ActionCanceledException ex)
        {
          Tracing.Log(MilestoneWS.sw, TraceLevel.Error, nameof (MilestoneWS), "Milestone ActionCancelledException " + ex.Message);
          this.checkFinishMilestone();
          this.RefreshTitle(true);
          return;
        }
        catch (ValidationException ex)
        {
          Tracing.Log(MilestoneWS.sw, TraceLevel.Error, nameof (MilestoneWS), "Milestone ValidationException " + ex.Message);
          int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.checkFinishMilestone();
          this.RefreshTitle(true);
          return;
        }
        catch (Exception ex)
        {
          Tracing.Log(MilestoneWS.sw, TraceLevel.Error, nameof (MilestoneWS), "Milestone Exception " + ex.Message);
          this.checkFinishMilestone();
          this.RefreshTitle(true);
          return;
        }
        this.moveComponents(!this.returnBtn.Enabled && !this.acceptBtn.Enabled);
      }
      this.topPanel.Invalidate();
      Cursor.Current = Cursors.Arrow;
    }

    private DateTime getAdjustedCompletionDate(DateTime startTime)
    {
      DateTime adjustedCompletionDate = startTime;
      for (int i = 0; i < this.stageIndex; ++i)
      {
        MilestoneLog milestoneAt = this.logList.GetMilestoneAt(i);
        if (milestoneAt.Done && milestoneAt.Date > adjustedCompletionDate)
          adjustedCompletionDate = milestoneAt.Date;
      }
      return adjustedCompletionDate;
    }

    private bool hasPermissionToFinishMilestones()
    {
      if (this.session.UserInfo.IsSuperAdministrator())
        return true;
      bool finishMilestones = true;
      int[] numArray = (int[]) null;
      Persona[] userPersonas = this.session.UserInfo.UserPersonas;
      if (userPersonas != null && userPersonas.Length != 0)
      {
        numArray = new int[userPersonas.Length];
        for (int index = 0; index < userPersonas.Length; ++index)
          numArray[index] = userPersonas[index].ID;
      }
      if (numArray == null)
        return false;
      string str = "previous";
      for (int index = 0; index <= this.stageIndex; ++index)
      {
        MilestoneLog mslog = this.mslogs[index];
        if (!mslog.Done)
        {
          if (!this.aclMgr.CheckPermission(AclMilestone.FinishMilestone, mslog.MilestoneID))
            finishMilestones = false;
          if (!finishMilestones)
          {
            str = mslog.Stage;
            break;
          }
        }
      }
      if (!finishMilestones)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You don't have permission to finish " + str + " milestone.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      return finishMilestones;
    }

    private void uncheckFinishMilestone()
    {
      this.checkBoxFinished.CheckedChanged -= new EventHandler(this.checkBoxFinished_CheckedChanged);
      this.checkBoxFinished.Checked = false;
      this.checkBoxFinished.CheckedChanged += new EventHandler(this.checkBoxFinished_CheckedChanged);
      Cursor.Current = Cursors.Default;
    }

    private void checkFinishMilestone()
    {
      this.checkBoxFinished.CheckedChanged -= new EventHandler(this.checkBoxFinished_CheckedChanged);
      this.checkBoxFinished.Checked = true;
      this.checkBoxFinished.CheckedChanged += new EventHandler(this.checkBoxFinished_CheckedChanged);
      Cursor.Current = Cursors.Default;
    }

    private RolesMappingInfo getRoleMapping(RealWorldRoleID roleId)
    {
      foreach (RolesMappingInfo roleMapping in this.session.LoanDataMgr.SystemConfiguration.RoleMappings)
      {
        if (roleMapping.RealWorldRoleID == roleId)
          return roleMapping;
      }
      return (RolesMappingInfo) null;
    }

    private bool hasBlankPreviousRole()
    {
      for (int index = 1; index <= this.stageIndex; ++index)
      {
        if (!this.mslogs[index].Done && this.mslogs[index].RoleID != -1)
        {
          if ((this.mslogs[index].RoleRequired ?? "") == string.Empty)
          {
            EllieMae.EMLite.Workflow.Milestone milestoneById = ((MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).GetMilestoneByID(this.mslogs[index - 1].MilestoneID, this.mslogs[index - 1].Stage, false, this.mslogs[index - 1].Days, this.mslogs[index - 1].DoneText, this.mslogs[index - 1].ExpText, this.mslogs[index - 1].RoleRequired == "Y", this.mslogs[index - 1].RoleID);
            if (milestoneById == null || !milestoneById.RoleRequired)
              continue;
          }
          else if ((this.mslogs[index].RoleRequired ?? "") == "N")
            continue;
          if ((this.mslogs[index].LoanAssociateID ?? "") == string.Empty)
          {
            string str = this.mslogs[index].Stage;
            if (string.Compare(this.mslogs[index].Stage, "Processing", true) == 0)
              str = this.mslogs[index].ExpText;
            int num = (int) Utils.Dialog((IWin32Window) this, "Please assign a loan team member for the " + str + " milestone prior to finishing this one.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return true;
          }
        }
      }
      return false;
    }

    public void RefreshContents()
    {
      if (this.currentMilestoneLog.Done)
      {
        this.checkBoxFinished.CheckedChanged -= new EventHandler(this.checkBoxFinished_CheckedChanged);
        this.checkBoxFinished.Checked = true;
        this.moveComponents(true);
        this.checkBoxFinished.CheckedChanged += new EventHandler(this.checkBoxFinished_CheckedChanged);
      }
      this.RefreshTitle(false);
    }

    public void RefreshLoanContents()
    {
      this.logList = this.session.LoanDataMgr.LoanData.GetLogList();
      this.currentMilestoneLog = this.logList.GetMilestoneByID(this.currentMilestoneLog.MilestoneID);
      this.loadContent();
      eFolderDialog.RefreshLoanContents();
    }

    internal void RefreshTitle(bool includePrevious)
    {
      this.checkBoxFinished.CheckedChanged -= new EventHandler(this.checkBoxFinished_CheckedChanged);
      this.checkBoxFinished.Checked = this.currentMilestoneLog.Done;
      this.refreshRequiredDocumentsAndFields(includePrevious);
      this.checkBoxFinished.CheckedChanged += new EventHandler(this.checkBoxFinished_CheckedChanged);
      this.refreshHeader();
      this.setMilestoneUIDate();
    }

    private void refreshRequiredDocumentsAndFields(bool includePrevious)
    {
      try
      {
        BusinessRuleCheck businessRuleCheck = new BusinessRuleCheck();
        businessRuleCheck.HasRequirement(this.session.LoanData, this.currentMilestoneLog);
        this.reqDocsControl.RefreshDocumentList(businessRuleCheck.RequiredDocs, includePrevious);
        this.reqFieldControl.RefreshFieldList(businessRuleCheck.RequiredFields, includePrevious);
        this.reqTasksControl.RefreshTaskList(businessRuleCheck.RequiredTasks, includePrevious);
      }
      catch (Exception ex)
      {
        Tracing.Log(MilestoneWS.sw, TraceLevel.Error, nameof (MilestoneWS), "Cannot check business rule: Error: " + ex.Message);
      }
    }

    private void refreshRequiredDocumentsAndFieldsHistory(
      MilestoneHistoryLog historyLog,
      bool includePrevious)
    {
      try
      {
        BusinessRuleCheck businessRuleCheck = new BusinessRuleCheck();
        businessRuleCheck.HasRequirement(this.session.LoanData, this.currentMilestoneLog);
        List<DocumentLog> docs = new List<DocumentLog>();
        List<MilestoneTaskLog> tasks = new List<MilestoneTaskLog>();
        foreach (DocumentLog document in historyLog.Documents)
        {
          if (document.Stage == this.currentMilestoneLog.Stage)
            docs.Add(document);
        }
        foreach (MilestoneTaskLog task in historyLog.Tasks)
        {
          if (task.Stage == this.currentMilestoneLog.Stage)
            tasks.Add(task);
        }
        this.reqDocsControl.RefreshHistoryDocumentList(docs);
        this.reqTasksControl.RefreshHistoryTaskList(tasks);
        this.reqFieldControl.RefreshFieldList(businessRuleCheck.RequiredFields, includePrevious);
      }
      catch (Exception ex)
      {
        Tracing.Log(MilestoneWS.sw, TraceLevel.Error, nameof (MilestoneWS), "Cannot check business rule: Error: " + ex.Message);
      }
    }

    public void RefreshTask(MilestoneTaskLog taskLog) => this.reqTasksControl.RefreshTask(taskLog);

    public string GetHelpTargetName()
    {
      return this.currentMilestoneLog.Stage == "Started" ? "StartedWS" : nameof (MilestoneWS);
    }

    private bool businessRulesPassed()
    {
      if (this.session.UserInfo.IsSuperAdministrator())
        return true;
      this.RefreshTitle(true);
      if (this.reqDocsControl.VerifyRequiredDocuments() && this.reqFieldControl.VerifyRequiredFields())
      {
        if (this.reqTasksControl.VerifyRequiredTasks())
        {
          try
          {
            using (ExecutionContext context = new ExecutionContext(this.session.UserInfo, this.session.LoanData, (IServerDataProvider) new CustomCodeSessionDataProvider(this.session.SessionObjects)))
              MilestoneValidatorCache.GetMilestoneValidators(this.session.SessionObjects, this.session.LoanDataMgr.SystemConfiguration).ValidateMilestone(this.currentMilestoneLog.MilestoneID, context);
          }
          catch (ValidationException ex)
          {
            int num = (int) Utils.Dialog((IWin32Window) null, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return false;
          }
          catch (Exception ex)
          {
            Tracing.Log(MilestoneWS.sw, nameof (MilestoneWS), TraceLevel.Error, "An error occurred while executing the custom validation code for this miletone: " + (object) ex);
            ErrorDialog.Display("An error occurred while executing the custom validation code for this miletone.", ex);
            return false;
          }
          return true;
        }
      }
      int num1 = (int) Utils.Dialog((IWin32Window) this, "Complete required items prior to finishing the milestone.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return false;
    }

    private void invalidMilestoneDateCallback(object notUsed)
    {
      this.BeginInvoke((Delegate) new MethodInvoker(this.invalidMilestoneDate));
    }

    private void invalidMilestoneDate()
    {
      int num = (int) Utils.Dialog((IWin32Window) this, "The milestone date must be later than all previous, completed milestones dates.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      this.setMilestoneUIDate();
    }

    private void dueDateChanged()
    {
      DateTime dateTime1 = this.dtPickerFinish.Value;
      if (this.milestoneUIDate == dateTime1)
        return;
      DateTime milestoneCompletionDate = this.logList.GetAdjustedMilestoneCompletionDate(this.stageIndex, dateTime1);
      if (milestoneCompletionDate.Date > dateTime1.Date)
      {
        int num = (int) new ChangeMilestoneDates(this.session, this.stageIndex, dateTime1).ShowDialog();
        this.setMilestoneUIDate();
      }
      else
      {
        this.dtPickerFinish.Validated -= new EventHandler(this.dtPickerFinish_Validated);
        this.boxDays.TextChanged -= new EventHandler(this.boxDays_TextChanged);
        try
        {
          DateTime dateTime2;
          for (int i = 0; i < this.stageIndex; ++i)
          {
            MilestoneLog milestoneAt = this.logList.GetMilestoneAt(i);
            dateTime2 = milestoneAt.Date;
            DateTime date1 = dateTime2.Date;
            dateTime2 = DateTime.MaxValue;
            DateTime date2 = dateTime2.Date;
            if (date1 == date2)
              milestoneAt.Date = milestoneCompletionDate;
          }
          if (this.stageIndex != 0)
          {
            DateTime date3 = milestoneCompletionDate.Date;
            dateTime2 = this.logList.GetMilestoneAt(this.stageIndex - 1).Date;
            DateTime date4 = dateTime2.Date;
            if (date3 < date4)
            {
              int num = (int) new ChangeMilestoneDates(this.session, this.stageIndex, milestoneCompletionDate.Date).ShowDialog();
              goto label_20;
            }
          }
          if (this.stageIndex != this.logList.GetNumberOfMilestones() - 1)
          {
            DateTime date5 = milestoneCompletionDate.Date;
            dateTime2 = this.logList.GetMilestoneAt(this.stageIndex + 1).Date;
            DateTime date6 = dateTime2.Date;
            if (date5 > date6)
            {
              if (this.logList.MSDateLock)
              {
                int num = (int) new ChangeMilestoneDates(this.session, this.stageIndex, milestoneCompletionDate.Date).ShowDialog();
                goto label_20;
              }
              else
              {
                this.currentMilestoneLog.AdjustDate(milestoneCompletionDate, false, true);
                goto label_20;
              }
            }
          }
          if (this.logList.MSDateLock)
            this.currentMilestoneLog.AdjustDate(milestoneCompletionDate, false, false);
          else
            this.currentMilestoneLog.AdjustDate(milestoneCompletionDate, false, true);
label_20:
          Session.Application.GetService<ILoanEditor>().RefreshLogPanel();
          this.setMilestoneUIDate();
        }
        catch (Exception ex)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        if (!this.checkBoxFinished.Checked)
          this.boxDays.Text = this.getMilestoneCountdown(this.dtPickerFinish.Value);
        this.dtPickerFinish.Validated += new EventHandler(this.dtPickerFinish_Validated);
        this.boxDays.TextChanged += new EventHandler(this.boxDays_TextChanged);
        this.refreshHeader();
      }
    }

    private string getMilestoneCountdown(DateTime fromNow)
    {
      return this.currentMilestoneLog.Done ? "0" : fromNow.Subtract(DateTime.Today).Days.ToString();
    }

    private void dataStampBtn_Click(object sender, EventArgs e)
    {
      this.commentsBox.TextChanged -= new EventHandler(this.commentsBox_TextChanged);
      if (this.currentMilestoneLog.Comments != string.Empty && !this.currentMilestoneLog.Comments.EndsWith("\r\n"))
        this.currentMilestoneLog.Comments += "\r\n";
      MilestoneLog currentMilestoneLog = this.currentMilestoneLog;
      currentMilestoneLog.Comments = currentMilestoneLog.Comments + DateTime.Now.ToString("MM/dd/yy h:mm tt ") + "(" + Utils.CurrentTimeZoneName + ") " + this.session.UserInfo.FullName + " > ";
      this.commentsBox.Text = this.currentMilestoneLog.Comments;
      this.commentsBox.SelectionStart = this.commentsBox.Text.Length;
      this.commentsBox.TextChanged += new EventHandler(this.commentsBox_TextChanged);
      this.commentsBox.Focus();
    }

    private void boxDays_TextChanged(object sender, EventArgs e) => this.dueDateChanged();

    private void returnBtn_Click(object sender, EventArgs e)
    {
      if (!this.session.LoanDataMgr.LockLoanWithExclusiveA())
        return;
      string str = string.Empty;
      if (this.pictureBoxPreviousLA.Tag != null)
      {
        RoleInfo tag = (RoleInfo) this.pictureBoxPreviousLA.Tag;
        if (tag != null)
          str = tag.RoleName;
      }
      if (str == string.Empty)
      {
        str = "previous loan team member";
        if (this.boxPreviousLA.Tag != null)
          str = ((LoanAssociateLog) this.boxPreviousLA.Tag).RoleName;
      }
      if (Utils.Dialog((IWin32Window) this, "This loan will return to the previous milestone with access restored to the previous loan team member." + (this.session.UserInfo.IsAdministrator() ? string.Empty : Environment.NewLine + "You will no longer have access to this loan.") + Environment.NewLine + Environment.NewLine + "Do you want to return this loan to the " + str + "?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
      {
        for (int stageIndex = this.stageIndex; stageIndex < this.mslogs.Length; ++stageIndex)
        {
          this.mslogs[stageIndex].Done = false;
          this.mslogs[stageIndex].Reviewed = false;
          this.session.LoanDataMgr.ClearLoanAssociate((LoanAssociateLog) this.mslogs[stageIndex]);
        }
        if (this.stageIndex > 1)
        {
          this.mslogs[this.stageIndex - 1].Done = false;
          this.mslogs[this.stageIndex - 1].Reviewed = false;
        }
        this.boxCurrentLA.Text = string.Empty;
        this.checkBoxFinished.Checked = false;
        this.boxNextLA.Text = string.Empty;
        this.moveComponents(true);
      }
      Session.Application.GetService<ILoanEditor>().RefreshLogPanel();
    }

    private void showAllBtn_Click(object sender, EventArgs e)
    {
      if (this.panelAllComments.Visible)
        this.refreshAllComments(false, true);
      else
        this.refreshAllComments(true, true);
    }

    private void refreshAllComments(bool enabled, bool reloadAllComments)
    {
      this.panelAllComments.Visible = enabled;
      if (enabled)
      {
        this.showAllBtn.Text = "&Hide All";
        if (reloadAllComments)
        {
          string str = string.Empty;
          foreach (MilestoneLog mslog in this.mslogs)
            str = str + mslog.Stage + "\r\n" + "--------------------------\r\n" + mslog.Comments + "\r\n\r\n";
          this.allCommentsBox.Text = str;
        }
        this.groupContainerComments.Height = this.panelComments.Height / 2;
      }
      else
      {
        this.showAllBtn.Text = "&Show All";
        this.groupContainerComments.Height = this.panelComments.Height;
      }
    }

    private void pictureBoxLoanAssociate_Click(object sender, EventArgs e)
    {
      switch (((Control) sender).Name)
      {
        case "pictureBoxPreviousLA":
          this.setLoanTeamMember(this.boxPreviousLA, (PictureBox) this.pictureBoxPreviousLA);
          break;
        case "pictureBoxCurrentLA":
          this.setLoanTeamMember(this.boxCurrentLA, (PictureBox) this.pictureBoxCurrentLA);
          break;
        case "pictureBoxNextLA":
          this.setLoanTeamMember(this.boxNextLA, (PictureBox) this.pictureBoxNextLA);
          break;
      }
      Session.Application.GetService<ILoanEditor>().RefreshLoanTeamMembers();
    }

    private bool setLoanTeamMember(TextBox box, PictureBox picBox)
    {
      MilestoneLog tag = (MilestoneLog) box.Tag;
      bool clearUser = false;
      if (string.Compare(picBox.Name, "pictureBoxNextLA", true) == 0 && !this.checkBoxFinished.Checked)
        clearUser = true;
      using (ProcessorSelectionDialog processorSelectionDialog = new ProcessorSelectionDialog((LoanAssociateLog) tag, clearUser))
      {
        if (processorSelectionDialog.ShowDialog((IWin32Window) this) == DialogResult.OK)
        {
          this.refreshRoles();
          this.editor.RefreshLoanTeamMembers();
          this.editor.ApplyBusinessRules();
          this.session.LoanDataMgr.LoanData.Calculator.SetVerificationTitle(tag);
          this.session.LoanDataMgr.LoanData.AccessRules.Refresh();
        }
      }
      return true;
    }

    private void acceptBtn_Click(object sender, EventArgs e)
    {
      this.currentMilestoneLog.Reviewed = true;
      this.acceptBtn.Enabled = false;
      this.refreshHeader();
      int num = (int) Utils.Dialog((IWin32Window) this, "The milestone alert has been cleared.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    private void MilestoneWS_Resize(object sender, EventArgs e)
    {
      this.panelLeft.Width = (this.Width - this.collapsibleSplitter1.Width) / 2;
      this.panelRequiredFields.Height = this.panelLeft.Height / 2;
      this.panelTasks.Height = this.panelDocList.Height / 2;
      this.refreshAllComments(this.allCommentsBox.Visible, false);
    }

    private void dtPickerFinish_Validated(object sender, EventArgs e) => this.dueDateChanged();

    private void dtPickerFinish_CloseUp(object sender, EventArgs e) => this.dueDateChanged();

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MilestoneWS));
      this.panelNext = new Panel();
      this.boxNextLA = new TextBox();
      this.labelNextLA = new Label();
      this.pictureBoxNextLA = new StandardIconButton();
      this.panelCurrent = new Panel();
      this.boxCurrentLA = new TextBox();
      this.labelCurrentLA = new Label();
      this.pictureBoxCurrentLA = new StandardIconButton();
      this.panelPrevious = new Panel();
      this.labelPreviousLA = new Label();
      this.boxPreviousLA = new TextBox();
      this.pictureBoxPreviousLA = new StandardIconButton();
      this.panelDate = new Panel();
      this.label2 = new Label();
      this.boxDays = new TextBox();
      this.labelDaysToFinish = new Label();
      this.dtPickerFinish = new DateTimePicker();
      this.pictureBoxFindOver = new PictureBox();
      this.returnBtn = new Button();
      this.acceptBtn = new Button();
      this.checkBoxFinished = new CheckBox();
      this.commentsBox = new TextBox();
      this.panel1 = new Panel();
      this.panelDocList = new Panel();
      this.panelRight = new Panel();
      this.panelComments = new Panel();
      this.panelAllComments = new BorderPanel();
      this.allCommentsBox = new TextBox();
      this.groupContainerComments = new GroupContainer();
      this.showAllBtn = new Button();
      this.dataStampBtn = new Button();
      this.panelTasks = new Panel();
      this.collapsibleSplitter1 = new CollapsibleSplitter();
      this.panelLeft = new Panel();
      this.panelDocumentTracking = new Panel();
      this.panelRequiredFields = new Panel();
      this.borderPanel1 = new BorderPanel();
      this.topPanel = new MilestoneHeader();
      this.dataTracBtn = new Button();
      this.label1 = new Label();
      this.panelNext.SuspendLayout();
      ((ISupportInitialize) this.pictureBoxNextLA).BeginInit();
      this.panelCurrent.SuspendLayout();
      ((ISupportInitialize) this.pictureBoxCurrentLA).BeginInit();
      this.panelPrevious.SuspendLayout();
      ((ISupportInitialize) this.pictureBoxPreviousLA).BeginInit();
      this.panelDate.SuspendLayout();
      ((ISupportInitialize) this.pictureBoxFindOver).BeginInit();
      this.panel1.SuspendLayout();
      this.panelDocList.SuspendLayout();
      this.panelRight.SuspendLayout();
      this.panelComments.SuspendLayout();
      this.panelAllComments.SuspendLayout();
      this.groupContainerComments.SuspendLayout();
      this.panelLeft.SuspendLayout();
      this.borderPanel1.SuspendLayout();
      this.topPanel.SuspendLayout();
      this.SuspendLayout();
      this.panelNext.Controls.Add((Control) this.boxNextLA);
      this.panelNext.Controls.Add((Control) this.labelNextLA);
      this.panelNext.Controls.Add((Control) this.pictureBoxNextLA);
      this.panelNext.Location = new Point(7, 54);
      this.panelNext.Name = "panelNext";
      this.panelNext.Size = new Size(312, 21);
      this.panelNext.TabIndex = 31;
      this.boxNextLA.Location = new Point(120, 0);
      this.boxNextLA.Name = "boxNextLA";
      this.boxNextLA.ReadOnly = true;
      this.boxNextLA.Size = new Size(156, 20);
      this.boxNextLA.TabIndex = 20;
      this.boxNextLA.TabStop = false;
      this.labelNextLA.AutoSize = true;
      this.labelNextLA.Location = new Point(0, 3);
      this.labelNextLA.Name = "labelNextLA";
      this.labelNextLA.Size = new Size(80, 14);
      this.labelNextLA.TabIndex = 19;
      this.labelNextLA.Text = "Next Associate";
      this.labelNextLA.TextAlign = ContentAlignment.MiddleLeft;
      this.pictureBoxNextLA.BackColor = Color.Transparent;
      this.pictureBoxNextLA.Location = new Point(279, 2);
      this.pictureBoxNextLA.MouseDownImage = (Image) null;
      this.pictureBoxNextLA.Name = "pictureBoxNextLA";
      this.pictureBoxNextLA.Size = new Size(16, 16);
      this.pictureBoxNextLA.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.pictureBoxNextLA.TabIndex = 27;
      this.pictureBoxNextLA.TabStop = false;
      this.pictureBoxNextLA.Click += new EventHandler(this.pictureBoxLoanAssociate_Click);
      this.panelCurrent.Controls.Add((Control) this.boxCurrentLA);
      this.panelCurrent.Controls.Add((Control) this.labelCurrentLA);
      this.panelCurrent.Controls.Add((Control) this.pictureBoxCurrentLA);
      this.panelCurrent.Location = new Point(7, 32);
      this.panelCurrent.Name = "panelCurrent";
      this.panelCurrent.Size = new Size(312, 21);
      this.panelCurrent.TabIndex = 30;
      this.boxCurrentLA.Location = new Point(120, 0);
      this.boxCurrentLA.Name = "boxCurrentLA";
      this.boxCurrentLA.ReadOnly = true;
      this.boxCurrentLA.Size = new Size(156, 20);
      this.boxCurrentLA.TabIndex = 14;
      this.boxCurrentLA.TabStop = false;
      this.labelCurrentLA.AutoSize = true;
      this.labelCurrentLA.Location = new Point(0, 3);
      this.labelCurrentLA.Name = "labelCurrentLA";
      this.labelCurrentLA.Size = new Size(94, 14);
      this.labelCurrentLA.TabIndex = 16;
      this.labelCurrentLA.Text = "Current Associate";
      this.labelCurrentLA.TextAlign = ContentAlignment.MiddleLeft;
      this.pictureBoxCurrentLA.BackColor = Color.Transparent;
      this.pictureBoxCurrentLA.Location = new Point(279, 2);
      this.pictureBoxCurrentLA.MouseDownImage = (Image) null;
      this.pictureBoxCurrentLA.Name = "pictureBoxCurrentLA";
      this.pictureBoxCurrentLA.Size = new Size(16, 16);
      this.pictureBoxCurrentLA.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.pictureBoxCurrentLA.TabIndex = 26;
      this.pictureBoxCurrentLA.TabStop = false;
      this.pictureBoxCurrentLA.Click += new EventHandler(this.pictureBoxLoanAssociate_Click);
      this.panelPrevious.Controls.Add((Control) this.labelPreviousLA);
      this.panelPrevious.Controls.Add((Control) this.boxPreviousLA);
      this.panelPrevious.Controls.Add((Control) this.pictureBoxPreviousLA);
      this.panelPrevious.Location = new Point(7, 10);
      this.panelPrevious.Name = "panelPrevious";
      this.panelPrevious.Size = new Size(312, 21);
      this.panelPrevious.TabIndex = 29;
      this.labelPreviousLA.AutoSize = true;
      this.labelPreviousLA.Location = new Point(0, 3);
      this.labelPreviousLA.Name = "labelPreviousLA";
      this.labelPreviousLA.Size = new Size(100, 14);
      this.labelPreviousLA.TabIndex = 13;
      this.labelPreviousLA.Text = "Previous Associate";
      this.labelPreviousLA.TextAlign = ContentAlignment.MiddleLeft;
      this.boxPreviousLA.Location = new Point(120, 0);
      this.boxPreviousLA.Name = "boxPreviousLA";
      this.boxPreviousLA.ReadOnly = true;
      this.boxPreviousLA.Size = new Size(156, 20);
      this.boxPreviousLA.TabIndex = 17;
      this.boxPreviousLA.TabStop = false;
      this.pictureBoxPreviousLA.BackColor = Color.Transparent;
      this.pictureBoxPreviousLA.Location = new Point(279, 2);
      this.pictureBoxPreviousLA.MouseDownImage = (Image) null;
      this.pictureBoxPreviousLA.Name = "pictureBoxPreviousLA";
      this.pictureBoxPreviousLA.Size = new Size(16, 16);
      this.pictureBoxPreviousLA.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.pictureBoxPreviousLA.TabIndex = 25;
      this.pictureBoxPreviousLA.TabStop = false;
      this.pictureBoxPreviousLA.Click += new EventHandler(this.pictureBoxLoanAssociate_Click);
      this.panelDate.Controls.Add((Control) this.label2);
      this.panelDate.Controls.Add((Control) this.boxDays);
      this.panelDate.Controls.Add((Control) this.labelDaysToFinish);
      this.panelDate.Controls.Add((Control) this.dtPickerFinish);
      this.panelDate.Location = new Point(316, 31);
      this.panelDate.Name = "panelDate";
      this.panelDate.Size = new Size(393, 21);
      this.panelDate.TabIndex = 28;
      this.label2.AutoSize = true;
      this.label2.ForeColor = Color.Blue;
      this.label2.Location = new Point(269, 4);
      this.label2.Name = "label2";
      this.label2.Size = new Size(117, 14);
      this.label2.TabIndex = 32;
      this.label2.Text = "Change Milestone Date";
      this.label2.Click += new EventHandler(this.label2_Click);
      this.boxDays.Location = new Point(78, 1);
      this.boxDays.Name = "boxDays";
      this.boxDays.ReadOnly = true;
      this.boxDays.Size = new Size(42, 20);
      this.boxDays.TabIndex = 2;
      this.boxDays.TabStop = false;
      this.labelDaysToFinish.AutoSize = true;
      this.labelDaysToFinish.Location = new Point(2, 4);
      this.labelDaysToFinish.Name = "labelDaysToFinish";
      this.labelDaysToFinish.Size = new Size(75, 14);
      this.labelDaysToFinish.TabIndex = 23;
      this.labelDaysToFinish.Text = "Days to Finish";
      this.dtPickerFinish.CalendarMonthBackground = Color.WhiteSmoke;
      this.dtPickerFinish.CalendarTitleBackColor = SystemColors.GradientInactiveCaption;
      this.dtPickerFinish.CustomFormat = "MM/dd/yyyy hh:mm tt";
      this.dtPickerFinish.Format = DateTimePickerFormat.Custom;
      this.dtPickerFinish.Location = new Point(126, 1);
      this.dtPickerFinish.Name = "dtPickerFinish";
      this.dtPickerFinish.Size = new Size(138, 20);
      this.dtPickerFinish.TabIndex = 3;
      this.dtPickerFinish.CloseUp += new EventHandler(this.dtPickerFinish_CloseUp);
      this.pictureBoxFindOver.Image = (Image) componentResourceManager.GetObject("pictureBoxFindOver.Image");
      this.pictureBoxFindOver.Location = new Point(606, 33);
      this.pictureBoxFindOver.Name = "pictureBoxFindOver";
      this.pictureBoxFindOver.Size = new Size(16, 16);
      this.pictureBoxFindOver.TabIndex = 27;
      this.pictureBoxFindOver.TabStop = false;
      this.pictureBoxFindOver.Visible = false;
      this.returnBtn.BackColor = SystemColors.Control;
      this.returnBtn.Image = (Image) componentResourceManager.GetObject("returnBtn.Image");
      this.returnBtn.ImageAlign = ContentAlignment.MiddleLeft;
      this.returnBtn.Location = new Point(480, 9);
      this.returnBtn.Name = "returnBtn";
      this.returnBtn.Padding = new Padding(7, 0, 0, 0);
      this.returnBtn.Size = new Size(145, 22);
      this.returnBtn.TabIndex = 1;
      this.returnBtn.Text = "     Return File to Sender";
      this.returnBtn.UseVisualStyleBackColor = true;
      this.returnBtn.Click += new EventHandler(this.returnBtn_Click);
      this.acceptBtn.BackColor = SystemColors.Control;
      this.acceptBtn.Image = (Image) componentResourceManager.GetObject("acceptBtn.Image");
      this.acceptBtn.ImageAlign = ContentAlignment.MiddleLeft;
      this.acceptBtn.Location = new Point(320, 9);
      this.acceptBtn.Name = "acceptBtn";
      this.acceptBtn.Padding = new Padding(7, 0, 0, 0);
      this.acceptBtn.Size = new Size(157, 22);
      this.acceptBtn.TabIndex = 0;
      this.acceptBtn.Text = "      Accept File (Clear Alert)";
      this.acceptBtn.UseVisualStyleBackColor = true;
      this.acceptBtn.Click += new EventHandler(this.acceptBtn_Click);
      this.checkBoxFinished.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.checkBoxFinished.ForeColor = SystemColors.ControlText;
      this.checkBoxFinished.Location = new Point(321, 55);
      this.checkBoxFinished.Name = "checkBoxFinished";
      this.checkBoxFinished.Size = new Size(77, 20);
      this.checkBoxFinished.TabIndex = 4;
      this.checkBoxFinished.Text = "Finished";
      this.commentsBox.BorderStyle = BorderStyle.None;
      this.commentsBox.Dock = DockStyle.Fill;
      this.commentsBox.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.commentsBox.Location = new Point(1, 26);
      this.commentsBox.Multiline = true;
      this.commentsBox.Name = "commentsBox";
      this.commentsBox.ScrollBars = ScrollBars.Both;
      this.commentsBox.Size = new Size(381, 74);
      this.commentsBox.TabIndex = 1;
      this.panel1.AutoScroll = true;
      this.panel1.Controls.Add((Control) this.panelDocList);
      this.panel1.Controls.Add((Control) this.borderPanel1);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(0, 26);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(731, 474);
      this.panel1.TabIndex = 22;
      this.panelDocList.Controls.Add((Control) this.panelRight);
      this.panelDocList.Controls.Add((Control) this.collapsibleSplitter1);
      this.panelDocList.Controls.Add((Control) this.panelLeft);
      this.panelDocList.Dock = DockStyle.Fill;
      this.panelDocList.Location = new Point(0, 84);
      this.panelDocList.Name = "panelDocList";
      this.panelDocList.Size = new Size(731, 390);
      this.panelDocList.TabIndex = 11;
      this.panelRight.Controls.Add((Control) this.panelComments);
      this.panelRight.Controls.Add((Control) this.panelTasks);
      this.panelRight.Dock = DockStyle.Fill;
      this.panelRight.Location = new Point(348, 0);
      this.panelRight.Name = "panelRight";
      this.panelRight.Size = new Size(383, 390);
      this.panelRight.TabIndex = 17;
      this.panelComments.Controls.Add((Control) this.panelAllComments);
      this.panelComments.Controls.Add((Control) this.groupContainerComments);
      this.panelComments.Dock = DockStyle.Fill;
      this.panelComments.Location = new Point(0, 191);
      this.panelComments.Name = "panelComments";
      this.panelComments.Size = new Size(383, 199);
      this.panelComments.TabIndex = 15;
      this.panelAllComments.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panelAllComments.Controls.Add((Control) this.allCommentsBox);
      this.panelAllComments.Dock = DockStyle.Fill;
      this.panelAllComments.ForeColor = SystemColors.GradientActiveCaption;
      this.panelAllComments.Location = new Point(0, 101);
      this.panelAllComments.Name = "panelAllComments";
      this.panelAllComments.Size = new Size(383, 98);
      this.panelAllComments.TabIndex = 0;
      this.allCommentsBox.BackColor = Color.WhiteSmoke;
      this.allCommentsBox.BorderStyle = BorderStyle.None;
      this.allCommentsBox.Dock = DockStyle.Fill;
      this.allCommentsBox.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.allCommentsBox.Location = new Point(1, 0);
      this.allCommentsBox.Multiline = true;
      this.allCommentsBox.Name = "allCommentsBox";
      this.allCommentsBox.ReadOnly = true;
      this.allCommentsBox.ScrollBars = ScrollBars.Both;
      this.allCommentsBox.Size = new Size(381, 97);
      this.allCommentsBox.TabIndex = 14;
      this.groupContainerComments.Controls.Add((Control) this.showAllBtn);
      this.groupContainerComments.Controls.Add((Control) this.dataStampBtn);
      this.groupContainerComments.Controls.Add((Control) this.commentsBox);
      this.groupContainerComments.Dock = DockStyle.Top;
      this.groupContainerComments.HeaderForeColor = SystemColors.ControlText;
      this.groupContainerComments.Location = new Point(0, 0);
      this.groupContainerComments.Name = "groupContainerComments";
      this.groupContainerComments.Size = new Size(383, 101);
      this.groupContainerComments.TabIndex = 16;
      this.groupContainerComments.Text = "Milestone Comments";
      this.showAllBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.showAllBtn.BackColor = SystemColors.Control;
      this.showAllBtn.Location = new Point(305, 2);
      this.showAllBtn.Name = "showAllBtn";
      this.showAllBtn.Size = new Size(73, 22);
      this.showAllBtn.TabIndex = 11;
      this.showAllBtn.Text = "&Show All";
      this.showAllBtn.UseVisualStyleBackColor = true;
      this.showAllBtn.Click += new EventHandler(this.showAllBtn_Click);
      this.dataStampBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.dataStampBtn.BackColor = SystemColors.Control;
      this.dataStampBtn.Location = new Point(225, 2);
      this.dataStampBtn.Name = "dataStampBtn";
      this.dataStampBtn.Size = new Size(80, 22);
      this.dataStampBtn.TabIndex = 13;
      this.dataStampBtn.Text = "Date Stamp";
      this.dataStampBtn.UseVisualStyleBackColor = true;
      this.dataStampBtn.Click += new EventHandler(this.dataStampBtn_Click);
      this.panelTasks.Dock = DockStyle.Top;
      this.panelTasks.Location = new Point(0, 0);
      this.panelTasks.Name = "panelTasks";
      this.panelTasks.Size = new Size(383, 191);
      this.panelTasks.TabIndex = 16;
      this.collapsibleSplitter1.AnimationDelay = 20;
      this.collapsibleSplitter1.AnimationStep = 20;
      this.collapsibleSplitter1.BackColor = Color.WhiteSmoke;
      this.collapsibleSplitter1.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter1.ControlToHide = (Control) this.panelLeft;
      this.collapsibleSplitter1.ExpandParentForm = false;
      this.collapsibleSplitter1.Location = new Point(341, 0);
      this.collapsibleSplitter1.Name = "collapsibleSplitter1";
      this.collapsibleSplitter1.TabIndex = 18;
      this.collapsibleSplitter1.TabStop = false;
      this.collapsibleSplitter1.UseAnimations = false;
      this.collapsibleSplitter1.VisualStyle = VisualStyles.Encompass;
      this.panelLeft.Controls.Add((Control) this.panelDocumentTracking);
      this.panelLeft.Controls.Add((Control) this.panelRequiredFields);
      this.panelLeft.Dock = DockStyle.Left;
      this.panelLeft.Location = new Point(0, 0);
      this.panelLeft.Name = "panelLeft";
      this.panelLeft.Size = new Size(341, 390);
      this.panelLeft.TabIndex = 15;
      this.panelDocumentTracking.Dock = DockStyle.Fill;
      this.panelDocumentTracking.Location = new Point(0, 0);
      this.panelDocumentTracking.Name = "panelDocumentTracking";
      this.panelDocumentTracking.Size = new Size(341, 191);
      this.panelDocumentTracking.TabIndex = 13;
      this.panelRequiredFields.Dock = DockStyle.Bottom;
      this.panelRequiredFields.Location = new Point(0, 191);
      this.panelRequiredFields.Name = "panelRequiredFields";
      this.panelRequiredFields.Size = new Size(341, 199);
      this.panelRequiredFields.TabIndex = 12;
      this.borderPanel1.Borders = AnchorStyles.Left | AnchorStyles.Right;
      this.borderPanel1.Controls.Add((Control) this.panelDate);
      this.borderPanel1.Controls.Add((Control) this.panelNext);
      this.borderPanel1.Controls.Add((Control) this.panelPrevious);
      this.borderPanel1.Controls.Add((Control) this.panelCurrent);
      this.borderPanel1.Controls.Add((Control) this.checkBoxFinished);
      this.borderPanel1.Controls.Add((Control) this.acceptBtn);
      this.borderPanel1.Controls.Add((Control) this.returnBtn);
      this.borderPanel1.Controls.Add((Control) this.pictureBoxFindOver);
      this.borderPanel1.Dock = DockStyle.Top;
      this.borderPanel1.Location = new Point(0, 0);
      this.borderPanel1.Name = "borderPanel1";
      this.borderPanel1.Size = new Size(731, 84);
      this.borderPanel1.TabIndex = 12;
      this.topPanel.Controls.Add((Control) this.dataTracBtn);
      this.topPanel.Controls.Add((Control) this.label1);
      this.topPanel.Dock = DockStyle.Top;
      this.topPanel.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.topPanel.Location = new Point(0, 0);
      this.topPanel.Name = "topPanel";
      this.topPanel.Size = new Size(731, 26);
      this.topPanel.TabIndex = 24;
      this.dataTracBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.dataTracBtn.BackColor = SystemColors.Control;
      this.dataTracBtn.Location = new Point(606, 2);
      this.dataTracBtn.Name = "dataTracBtn";
      this.dataTracBtn.Size = new Size(120, 22);
      this.dataTracBtn.TabIndex = 14;
      this.dataTracBtn.Text = "Submit to DataTrac";
      this.dataTracBtn.UseVisualStyleBackColor = true;
      this.dataTracBtn.Visible = false;
      this.dataTracBtn.Click += new EventHandler(this.dataTracBtn_Click);
      this.label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label1.BackColor = Color.Transparent;
      this.label1.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.ForeColor = Color.White;
      this.label1.Location = new Point(8, 6);
      this.label1.Name = "label1";
      this.label1.Size = new Size(592, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "label1";
      this.AutoScroll = true;
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.topPanel);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (MilestoneWS);
      this.Size = new Size(731, 500);
      this.Resize += new EventHandler(this.MilestoneWS_Resize);
      this.panelNext.ResumeLayout(false);
      this.panelNext.PerformLayout();
      ((ISupportInitialize) this.pictureBoxNextLA).EndInit();
      this.panelCurrent.ResumeLayout(false);
      this.panelCurrent.PerformLayout();
      ((ISupportInitialize) this.pictureBoxCurrentLA).EndInit();
      this.panelPrevious.ResumeLayout(false);
      this.panelPrevious.PerformLayout();
      ((ISupportInitialize) this.pictureBoxPreviousLA).EndInit();
      this.panelDate.ResumeLayout(false);
      this.panelDate.PerformLayout();
      ((ISupportInitialize) this.pictureBoxFindOver).EndInit();
      this.panel1.ResumeLayout(false);
      this.panelDocList.ResumeLayout(false);
      this.panelRight.ResumeLayout(false);
      this.panelComments.ResumeLayout(false);
      this.panelAllComments.ResumeLayout(false);
      this.panelAllComments.PerformLayout();
      this.groupContainerComments.ResumeLayout(false);
      this.groupContainerComments.PerformLayout();
      this.panelLeft.ResumeLayout(false);
      this.borderPanel1.ResumeLayout(false);
      this.topPanel.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void dataTracBtn_Click(object sender, EventArgs e)
    {
      Session.Application.GetService<ILoanServices>()?.OrderDataTrac();
    }

    private void label2_Click(object sender, EventArgs e)
    {
      if (new ChangeMilestoneDates(this.session, false).ShowDialog((IWin32Window) this) != DialogResult.OK)
      {
        this.RefreshTitle(false);
      }
      else
      {
        this.refreshHeader();
        this.setMilestoneUIDate();
      }
    }
  }
}
