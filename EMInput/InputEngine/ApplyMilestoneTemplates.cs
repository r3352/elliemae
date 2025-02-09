// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.ApplyMilestoneTemplates
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.InputEngine.MilestoneManagement;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.EMLite.Workflow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class ApplyMilestoneTemplates : Form
  {
    private static readonly string sw = Tracing.SwInputEngine;
    private Sessions.Session session;
    private IEnumerable<MilestoneTemplate> templates;
    private List<string> satisfiedTemplates;
    private MilestoneTemplate currentTemplate;
    private RoleInfo[] roles;
    private IEnumerable<EllieMae.EMLite.Workflow.Milestone> allMilestones;
    private List<LogRecordBase> logRecords;
    private DateTime previousDate;
    private Font font;
    private LogList list;
    private LoanConditions loanConditions;
    private LoanData loan;
    private bool manual;
    private MilestoneTemplate selectedTemplate;
    private bool MSlock;
    private int differentPoint;
    private Dictionary<UserInfo, List<string>> sendEmail = new Dictionary<UserInfo, List<string>>();
    private Dictionary<UserInfo, List<string>> dontSendEmail = new Dictionary<UserInfo, List<string>>();
    private bool hideMilestoneTemplate;
    private bool emailNotification;
    private const string className = "ApplyMilestoneTemplates";
    private string changeReason = "";
    private TriggerEmailTemplate template;
    private MilestoneTemplatesBpmManager milestoneTemplatesBpmManager;
    private IContainer components;
    private Label lblTitle;
    private Label lblDesc;
    private GroupContainer groupContainer1;
    private BorderPanel borderPanel1;
    private GridView gvMilestones;
    private GroupContainer grpTemplates;
    private GridView gvTemplates;
    private Label lblCurrentTemplate;
    private Button btnSelect;
    private Button btnCancel;
    private GroupContainer groupContainer2;
    private BorderPanel borderPanel2;
    private GridView gridView2;
    private EMHelpLink emHelpLink1;
    private PictureBox pictureBox1;
    private GradientPanel gradientPanel1;
    private Label lblTemplateName;
    private Label label1;
    private Panel panel1;
    private Label label3;
    private Panel panel2;

    public ApplyMilestoneTemplates(LoanConditions loanConditions, LoanData loan)
      : this(Session.DefaultInstance, loanConditions, loan)
    {
    }

    public ApplyMilestoneTemplates(LoanConditions loanConditions, LoanData loan, bool saveLoan)
    {
      this.manual = false;
      this.session = Session.DefaultInstance;
      this.loan = loan;
      this.loanConditions = loanConditions;
      this.init();
      foreach (MilestoneTemplate template in this.templates)
      {
        if (template.Active && this.satisfiedTemplates.Contains(template.Name))
        {
          if (template.Name != this.loan.GetLogList().MilestoneTemplate.Name)
          {
            this.SelectedTemplate = template;
            this.populateTemplateMilestones(this.SelectedTemplate);
            this.setUpView();
            if (saveLoan)
            {
              int num = (int) this.ShowDialog();
              if (this.DialogResult == DialogResult.OK || this.DialogResult == DialogResult.Cancel)
                break;
            }
            else
            {
              this.ReplaceTemplate(false);
              break;
            }
          }
          this.DialogResult = DialogResult.None;
          break;
        }
      }
    }

    public ApplyMilestoneTemplates(
      LoanConditions loanConditions,
      LoanData loan,
      MilestoneTemplate selectedTemplate,
      string changeReason)
    {
      this.manual = true;
      this.session = Session.DefaultInstance;
      this.loan = loan;
      this.loanConditions = loanConditions;
      this.SelectedTemplate = selectedTemplate;
      this.changeReason = changeReason;
      this.init();
      this.populateTemplateMilestones(selectedTemplate);
      this.setUpView();
      int num = (int) this.ShowDialog();
      if (this.DialogResult == DialogResult.OK || this.DialogResult == DialogResult.Cancel)
        return;
      this.DialogResult = DialogResult.None;
    }

    public ApplyMilestoneTemplates(
      Sessions.Session session,
      LoanConditions loanConditions,
      LoanData loan)
    {
      this.manual = true;
      this.session = session;
      this.loan = loan;
      this.loanConditions = loanConditions;
      this.init();
      this.lblDesc.Text = "Select a milestone template to apply to this loan file. The template that best matches the loan’s data is listed first, and so on.";
      this.lblDesc.Location = new Point(9, 13);
      this.lblDesc.Size = new Size(600, 22);
      this.lblTemplateName.Text = this.currentTemplate.Name;
      List<GVItem> gvItemList1 = new List<GVItem>();
      List<GVItem> gvItemList2 = new List<GVItem>();
      foreach (MilestoneTemplate template in this.templates)
      {
        if (template.Active)
        {
          GVItem gvItem = new GVItem(template.Name);
          gvItem.Tag = (object) template;
          if (template.Equals(this.currentTemplate))
            gvItem.SubItems[0].Text += " (Current)";
          if (this.satisfiedTemplates.Contains(template.Name))
          {
            gvItemList1.Add(gvItem);
            gvItem.SubItems[0].Font = this.font;
          }
          else
            gvItemList2.Add(gvItem);
        }
      }
      this.gvTemplates.Items.AddRange(gvItemList1.ToArray());
      if (!this.hideMilestoneTemplate)
      {
        if (gvItemList2.Count > 0)
          this.gvTemplates.Items.Add(new GVItem("       -----Non-matching Templates-----"));
        this.gvTemplates.Items.AddRange(gvItemList2.ToArray());
      }
      this.gvTemplates.Items[0].Selected = true;
    }

    private void init()
    {
      this.InitializeComponent();
      this.list = this.session.LoanData.GetLogList();
      this.currentTemplate = this.list.MilestoneTemplate;
      this.MSlock = this.list.MSLock;
      this.roles = this.session.SessionObjects.BpmManager.GetAllRoleFunctions();
      this.logRecords = ((IEnumerable<LogRecordBase>) Session.LoanData.GetLogList().GetAllDatedRecords()).ToList<LogRecordBase>();
      List<LogRecordBase> logRecordBaseList = new List<LogRecordBase>();
      foreach (LogRecordBase logRecord in this.logRecords)
      {
        if (!(logRecord is MilestoneLog))
          logRecordBaseList.Add(logRecord);
      }
      logRecordBaseList.ForEach((Action<LogRecordBase>) (item => this.logRecords.Remove(item)));
      this.milestoneTemplatesBpmManager = (MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones);
      this.allMilestones = this.milestoneTemplatesBpmManager.GetAllMilestonesList();
      this.satisfiedTemplates = this.milestoneTemplatesBpmManager.GetSatisfiedMilestoneTemplate(this.loanConditions, this.loan);
      this.templates = this.session.SessionObjects.BpmManager.GetMilestoneTemplates(true);
      this.hideMilestoneTemplate = !(bool) this.session.ServerManager.GetServerSetting("Policies.ShowNonMatchingMilestoneTemplate");
      this.emailNotification = string.IsNullOrWhiteSpace(this.session.ConfigurationManager.GetCompanySetting("POLICIES", "MilestoneTemplateChangeNotification")) ? (bool) this.session.ServerManager.GetServerSetting("Policies.MilestoneTemplateChangeNotification") : bool.Parse(this.session.ConfigurationManager.GetCompanySetting("POLICIES", "MilestoneTemplateChangeNotification"));
      this.font = new Font(this.gridView2.Font, FontStyle.Bold);
    }

    public MilestoneTemplate SelectedTemplate
    {
      get => this.selectedTemplate;
      set => this.selectedTemplate = value;
    }

    private void btnSelect_Click(object sender, EventArgs e)
    {
      if (this.btnSelect.Text == "Select and Continue")
      {
        if (this.gvTemplates.SelectedItems.Count == 0)
        {
          int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a template", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          this.SelectedTemplate = (MilestoneTemplate) this.gvTemplates.SelectedItems[0].Tag;
          if (this.SelectedTemplate == null)
            return;
          this.setUpView();
        }
      }
      else
      {
        if (!(this.btnSelect.Text == "Apply") && !(this.btnSelect.Text == "OK"))
          return;
        if (!this.showListOfUsersLosingAccess())
        {
          int num2 = (int) MessageBox.Show((IWin32Window) this, "The NEW milestone template was not applied.", "Milestone Template Not Applied", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          this.DialogResult = DialogResult.Cancel;
        }
        else
        {
          this.ReplaceTemplate(true);
          this.DialogResult = DialogResult.OK;
        }
      }
    }

    private void setUpView()
    {
      this.gradientPanel1.Visible = false;
      this.grpTemplates.Visible = false;
      if (!this.manual)
      {
        this.pictureBox1.Visible = true;
        this.Text = "Milestone Log Change";
        this.lblTitle.Text = "Milestone Log Change";
        this.lblDesc.Text = "Based on the changes made to the loan file, the current milestone list is no longer accessible. The loan file will be changed to use the new milestone list shown on the right table and all other milestones will be removed.";
        this.btnSelect.Text = "OK";
        this.lblTitle.Location = new Point(59, 13);
        this.lblDesc.Location = new Point(59, 30);
      }
      else
      {
        this.lblTitle.Text = "Preview and Apply";
        this.lblDesc.Text = "If you apply the NEW milestone template, the milestones highlighted in the CURRENT milestone list will not change. All other milestones in the CURRENT list will be removed from the loan file. Click the Apply button to apply the NEW milestone template.";
        this.btnSelect.Text = "Apply";
        this.lblTitle.Location = new Point(9, 13);
        this.lblDesc.Location = new Point(9, 30);
      }
      this.lblTitle.Visible = true;
      this.lblDesc.Size = new Size(750, 30);
      this.lblCurrentTemplate.Visible = false;
      this.gvMilestones.Columns[2].Text = "Date";
      this.groupContainer2.Visible = true;
      this.groupContainer2.Location = new Point(12, 64);
      this.groupContainer1.Location = new Point(430, 64);
      this.panel2.Location = new Point(400, 150);
      this.groupContainer2.Text = "CURRENT - " + this.currentTemplate.Name;
      this.groupContainer1.Text = "NEW - " + this.SelectedTemplate.Name;
      this.btnSelect.Size = new Size(75, 23);
      this.ClientSize = new Size(830, 475);
      this.btnSelect.Location = new Point(660, this.btnSelect.Location.Y);
      this.populateCurrentTemplateMilestone();
      this.highlightDifferentMilestones();
      this.findAllRolesLosingAccess();
      this.showRoles();
      this.panel1.Location = new Point(13, 442);
      this.ClientSize = new Size(830, 475 + this.panel1.Size.Height);
      this.btnSelect.Location = new Point(660, this.btnSelect.Location.Y);
    }

    private void ReplaceTemplate(bool createHistory)
    {
      MilestoneHistoryLog rec1 = (MilestoneHistoryLog) null;
      if (createHistory)
      {
        if (this.currentTemplate != null)
        {
          if (this.manual)
          {
            if (this.changeReason == "")
              this.changeReason = "Changed Manually";
          }
          else
            this.changeReason = "Changed Automatically";
        }
        rec1 = this.createMilestoneHistoryLog();
      }
      this.list.MilestoneTemplate = this.SelectedTemplate;
      MilestoneTemplate.TemplateMilestones sequentialMilestones = this.SelectedTemplate.SequentialMilestones;
      DateTime now = DateTime.Now;
      UserInfo userInfo = this.session.SessionObjects.UserInfo;
      this.list.RemoveMilestone(this.differentPoint);
      DateTime date = this.list.GetMilestone("Started").Date;
      foreach (MilestoneLog allMilestone in this.list.GetAllMilestones())
      {
        if (!allMilestone.Done)
          allMilestone.Date = date.AddDays((double) allMilestone.Days);
        date = allMilestone.Date;
      }
      for (int differentPoint = this.differentPoint; differentPoint < sequentialMilestones.Count(); ++differentPoint)
      {
        MilestoneLog milestoneById1 = this.list.GetMilestoneByID(sequentialMilestones[differentPoint].MilestoneID);
        EllieMae.EMLite.Workflow.Milestone milestoneById2 = this.milestoneTemplatesBpmManager.GetMilestoneByID(sequentialMilestones[differentPoint].MilestoneID, milestoneById1.Stage, false, milestoneById1.Days, milestoneById1.DoneText, milestoneById1.ExpText);
        MilestoneLog milestoneLog = this.list.AddMilestone(milestoneById2.Name, sequentialMilestones[differentPoint].DaysToComplete, sequentialMilestones[differentPoint].MilestoneID, sequentialMilestones[differentPoint].SortIndex, milestoneById2.TPOConnectStatus, milestoneById2.ConsumerStatus);
        milestoneLog.RoleID = milestoneById2.RoleID;
        milestoneLog.RoleName = this.getRoleName(milestoneLog.RoleID);
        milestoneLog.RoleRequired = milestoneById2.RoleRequired ? "Y" : "N";
        milestoneLog.Date = Convert.ToDateTime(this.gvMilestones.Items[differentPoint].SubItems[2].Value);
      }
      if (this.differentPoint > 1)
      {
        MilestoneLog logRecord = (MilestoneLog) this.logRecords[this.differentPoint - 1];
        EllieMae.EMLite.Workflow.Milestone milestoneById = this.getMilestoneByID(sequentialMilestones[this.differentPoint - 1].MilestoneID);
        if (logRecord.Done)
        {
          logRecord.Done = false;
          logRecord.ClearLoanAssociate();
          logRecord.RoleID = milestoneById.RoleID;
          logRecord.RoleName = this.getRoleName(milestoneById.RoleID);
          logRecord.RoleRequired = milestoneById.RoleRequired ? "Y" : "N";
          logRecord.Date = now;
        }
      }
      foreach (MilestoneFreeRoleLog milestoneFreeRole in this.list.GetAllMilestoneFreeRoles())
      {
        MilestoneFreeRoleLog existingLog = milestoneFreeRole;
        if (this.SelectedTemplate.FreeRoles.FirstOrDefault<TemplateFreeRole>((Func<TemplateFreeRole, bool>) (x => x.RoleID == existingLog.RoleID)) == null)
          this.loan.GetLogList().RemoveRecord((LogRecordBase) existingLog);
      }
      MilestoneFreeRoleLog[] milestoneFreeRoles = this.list.GetAllMilestoneFreeRoles();
      foreach (TemplateFreeRole freeRole1 in this.SelectedTemplate.FreeRoles)
      {
        TemplateFreeRole freeRole = freeRole1;
        if (((IEnumerable<MilestoneFreeRoleLog>) milestoneFreeRoles).FirstOrDefault<MilestoneFreeRoleLog>((Func<MilestoneFreeRoleLog, bool>) (x => x.RoleID == freeRole.RoleID)) == null)
        {
          RoleInfo roleInfo = ((IEnumerable<RoleInfo>) this.roles).FirstOrDefault<RoleInfo>((Func<RoleInfo, bool>) (y => y.RoleID == freeRole.RoleID));
          if (roleInfo != null)
          {
            MilestoneFreeRoleLog rec2 = new MilestoneFreeRoleLog();
            rec2.RoleID = roleInfo.RoleID;
            rec2.RoleName = roleInfo.RoleName;
            rec2.MarkAsClean();
            this.list.AddRecord((LogRecordBase) rec2, true);
          }
        }
      }
      if (!this.manual)
      {
        this.list.MSLock = false;
      }
      else
      {
        this.list.MSLock = true;
        int num = (int) MessageBox.Show((IWin32Window) this, "The new milestone template has been applied. The system will no longer try to apply the best matching template to your loan.", "Milestone Template Applied", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      if (rec1 != null)
        this.list.AddRecord((LogRecordBase) rec1);
      this.list.ReAssignCustomMileStones();
      this.list.ReAssignTasksMilestones();
      if (this.sendEmail.Count > 0 && this.emailNotification)
        this.sendEmailToUsersLosingAccess();
      Session.Application.GetService<ILoanEditor>().ClearMilestoneLogArea();
    }

    private void highlightDifferentMilestones()
    {
      int i = 0;
      ((IEnumerable<MilestoneLog>) this.list.GetAllMilestones()).TakeWhile<MilestoneLog>((Func<MilestoneLog, int, bool>) ((item, index) => item.MilestoneID.Equals(this.SelectedTemplate.SequentialMilestones[index].MilestoneID))).ToList<MilestoneLog>().ForEach((Action<MilestoneLog>) (item =>
      {
        this.gridView2.Items[i].BackColor = Color.GreenYellow;
        MilestoneLog logRecord = (MilestoneLog) this.logRecords[i];
        if (logRecord.Done)
        {
          this.previousDate = logRecord.Date;
          this.gridView2.Items[i].SubItems[2].Value = (object) logRecord.Date.ToString();
          this.gridView2.Items[i].SubItems.ToList<GVSubItem>().ForEach((Action<GVSubItem>) (em => em.Font = this.font));
        }
        else
        {
          this.previousDate = this.AddDays(this.previousDate, logRecord.Days);
          this.gridView2.Items[i].SubItems[2].Value = (object) this.previousDate.ToString();
        }
        this.gvMilestones.Items[i].ForeColor = Color.Gray;
        ++i;
      }));
      for (int index = 0; index < i; ++index)
      {
        if (index >= 1 && index == i - 1)
        {
          DateTime dateTime;
          if (((MilestoneLog) this.logRecords[index]).Done)
          {
            GVSubItem subItem = this.gvMilestones.Items[index].SubItems[2];
            this.previousDate = dateTime = DateTime.Now;
            // ISSUE: variable of a boxed type
            __Boxed<DateTime> local = (ValueType) dateTime;
            subItem.Value = (object) local;
          }
          else if (this.gridView2.Items[index].SubItems[2].Value != null)
          {
            this.gvMilestones.Items[index].SubItems[2].Value = (object) this.gridView2.Items[index].SubItems[2].Value.ToString();
          }
          else
          {
            GVSubItem subItem = this.gvMilestones.Items[index].SubItems[2];
            dateTime = this.logRecords[index].Date;
            string str = dateTime.ToString();
            subItem.Value = (object) str;
          }
          this.gvMilestones.Items[index].SubItems.ToList<GVSubItem>().ForEach((Action<GVSubItem>) (em => em.Font = this.gvMilestones.Font));
          this.gvMilestones.Items[index].BackColor = Color.GreenYellow;
          this.gvMilestones.Items[index].ForeColor = Color.Black;
        }
        else
          this.gvMilestones.Items[index] = this.gridView2.Items[index];
      }
      for (int index = i; index < this.SelectedTemplate.SequentialMilestones.Count(); ++index)
      {
        this.gvMilestones.Items[index].BackColor = Color.GreenYellow;
        this.previousDate = this.AddDays(this.previousDate, this.SelectedTemplate.SequentialMilestones[index].DaysToComplete);
        this.gvMilestones.Items[index].SubItems[2].Value = (object) this.previousDate.ToString();
        if (index < this.gridView2.Items.Count)
          this.gridView2.Items[index].ForeColor = Color.Gray;
      }
      for (int nItemIndex = this.SelectedTemplate.SequentialMilestones.Count(); nItemIndex < this.list.GetNumberOfMilestones(); ++nItemIndex)
        this.gridView2.Items[nItemIndex].ForeColor = Color.Gray;
      this.differentPoint = i;
      for (int index = i; index < this.list.GetNumberOfMilestones(); ++index)
        this.gridView2.Items[index].SubItems[2].Value = (object) this.list.GetMilestoneAt(index).Date;
    }

    private void findAllRolesLosingAccess()
    {
      for (int differentPoint = this.differentPoint; differentPoint < this.list.GetNumberOfMilestones(); ++differentPoint)
      {
        MilestoneLog milestoneAt = this.list.GetMilestoneAt(differentPoint);
        if (!(milestoneAt.LoanAssociateID == ""))
        {
          UserInfo user1 = Session.OrganizationManager.GetUser(milestoneAt.LoanAssociateID);
          if (user1 != (UserInfo) null)
          {
            if (this.dontSendEmail.ContainsKey(user1))
              this.dontSendEmail.Remove(user1);
            if (!user1.IsAdministrator())
            {
              if (!this.sendEmail.ContainsKey(user1))
              {
                this.sendEmail.Add(user1, new List<string>()
                {
                  this.getRoleName(milestoneAt.RoleID)
                });
              }
              else
              {
                List<string> stringList = this.sendEmail[user1];
                stringList.Add(this.getRoleName(milestoneAt.RoleID));
                this.sendEmail[user1] = stringList;
              }
            }
          }
          else
          {
            int result;
            if (int.TryParse(milestoneAt.LoanAssociateID, out result))
            {
              foreach (string userId in Session.AclGroupManager.GetUsersInGroup(result, true))
              {
                UserInfo user2 = Session.OrganizationManager.GetUser(userId);
                if (!user2.IsAdministrator())
                {
                  if (!this.sendEmail.ContainsKey(user2) && !this.dontSendEmail.ContainsKey(user2))
                    this.dontSendEmail.Add(user2, new List<string>()
                    {
                      this.getRoleName(milestoneAt.RoleID)
                    });
                  else if (this.dontSendEmail.ContainsKey(user2))
                  {
                    List<string> stringList = this.dontSendEmail[user2];
                    stringList.Add(this.getRoleName(milestoneAt.RoleID));
                    this.dontSendEmail[user1] = stringList;
                  }
                }
              }
            }
          }
        }
      }
      foreach (MilestoneFreeRoleLog milestoneFreeRole in this.list.GetAllMilestoneFreeRoles())
      {
        MilestoneFreeRoleLog freeRole = milestoneFreeRole;
        if (!(freeRole.LoanAssociateID == string.Empty) && this.SelectedTemplate.FreeRoles.FirstOrDefault<TemplateFreeRole>((Func<TemplateFreeRole, bool>) (x => x.RoleID == freeRole.RoleID)) == null)
        {
          UserInfo user = Session.OrganizationManager.GetUser(freeRole.LoanAssociateID);
          if (user != (UserInfo) null)
          {
            if (this.dontSendEmail.ContainsKey(user))
              this.dontSendEmail.Remove(user);
            if (!user.IsAdministrator())
            {
              if (!this.sendEmail.ContainsKey(user))
              {
                this.sendEmail.Add(user, new List<string>()
                {
                  freeRole.RoleName
                });
              }
              else
              {
                List<string> stringList = this.sendEmail[user];
                stringList.Add(freeRole.RoleName);
                this.sendEmail[user] = stringList;
              }
            }
          }
        }
      }
    }

    private bool showListOfUsersLosingAccess()
    {
      List<LogRecordBase> logRecords = new List<LogRecordBase>();
      foreach (MilestoneLog allMilestone in this.list.GetAllMilestones())
      {
        if (this.selectedTemplate.SequentialMilestones.GetMilestone(allMilestone.MilestoneID) == null)
          logRecords.Add((LogRecordBase) allMilestone);
      }
      if (this.sendEmail.Count == 0 && this.dontSendEmail.Count == 0 && logRecords.Count == 0)
        return true;
      LogChangeConfirmation changeConfirmation = new LogChangeConfirmation(this.sendEmail, this.dontSendEmail, logRecords, this.emailNotification);
      this.Hide();
      int num = (int) changeConfirmation.ShowDialog((IWin32Window) this);
      if (changeConfirmation.DialogResult != DialogResult.OK)
        return false;
      this.sendEmail = changeConfirmation.EmailList;
      return true;
    }

    private void showRoles()
    {
      Point point = new Point(9, 24);
      if (this.sendEmail.Count == 0 && this.dontSendEmail.Count == 0)
      {
        this.label3.Visible = false;
        this.panel1.Size = new Size(0, 0);
      }
      else
      {
        foreach (KeyValuePair<UserInfo, List<string>> user in this.sendEmail)
        {
          if (!user.Key.IsAdministrator())
          {
            string text = this.getText(user);
            Label label = new Label();
            label.AutoSize = true;
            label.Location = point;
            label.Name = user.Key.FullName;
            label.Size = new Size(61, 18);
            label.TabIndex = 10;
            label.Text = text;
            this.panel1.Controls.Add((Control) label);
            point.Y += 18;
          }
        }
        foreach (KeyValuePair<UserInfo, List<string>> user in this.dontSendEmail)
        {
          if ((object) user.Key != null && !user.Key.IsAdministrator())
          {
            string text = this.getText(user);
            Label label = new Label();
            label.AutoSize = true;
            label.Location = point;
            label.Name = user.Key.FullName;
            label.Size = new Size(61, 18);
            label.TabIndex = 10;
            label.Text = text;
            this.panel1.Controls.Add((Control) label);
            point.Y += 18;
          }
        }
        this.panel1.Size = new Size(600, point.Y + 10);
      }
    }

    private string getText(KeyValuePair<UserInfo, List<string>> user)
    {
      string str1 = user.Key.FullName + " (";
      string str2;
      if (user.Value.Count > 1)
      {
        foreach (string str3 in user.Value)
          str1 = str1 + str3 + ", ";
        str2 = str1.Substring(0, str1.Length - 2);
      }
      else
        str2 = str1 + user.Value[0];
      return str2 + ")";
    }

    private MilestoneHistoryLog createMilestoneHistoryLog()
    {
      List<LogRecordBase> historyLogs = new List<LogRecordBase>();
      historyLogs.AddRange((IEnumerable<LogRecordBase>) this.loan.GetLogList().GetAllMilestones());
      historyLogs.AddRange((IEnumerable<LogRecordBase>) this.loan.GetLogList().GetAllMilestoneTaskLogs());
      historyLogs.AddRange((IEnumerable<LogRecordBase>) this.loan.GetLogList().GetAllDocuments());
      return new MilestoneHistoryLog(this.list, historyLogs, this.session.UserID, this.changeReason, this.loan.GetLogList().MilestoneTemplate, this.loan.GetLogList().MSLock, this.loan.GetLogList().MSDateLock);
    }

    private void gvTemplates_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.gvTemplates.SelectedItems.Count <= 0)
        return;
      this.SelectedTemplate = (MilestoneTemplate) this.gvTemplates.SelectedItems[0].Tag;
      if (this.SelectedTemplate == null)
        return;
      this.populateTemplateMilestones(this.SelectedTemplate);
    }

    private void populateTemplateMilestones(MilestoneTemplate template)
    {
      this.gvMilestones.Items.Clear();
      this.groupContainer1.Text = template.Name + " Milestones";
      foreach (TemplateMilestone sequentialMilestone in template.SequentialMilestones)
        this.gvMilestones.Items.Add(this.createGVItemForTemplateMilestone(sequentialMilestone));
    }

    private void populateCurrentTemplateMilestone()
    {
      this.gridView2.Items.Clear();
      foreach (MilestoneLog allMilestone in this.list.GetAllMilestones())
        this.gridView2.Items.Add(this.createGVItemForMilestoneLog(allMilestone));
    }

    private GVItem createGVItemForTemplateMilestone(TemplateMilestone templateMs)
    {
      return this.createGVItemForMilestone(this.getMilestoneByID(templateMs.MilestoneID), templateMs.RoleID, templateMs.DaysToComplete);
    }

    private GVItem createGVItemForMilestoneLog(MilestoneLog log)
    {
      return this.createGVItemForMilestone(this.getMilestoneByID(log.MilestoneID), log.RoleID, log.Days);
    }

    private GVItem createGVItemForMilestone(EllieMae.EMLite.Workflow.Milestone ms, int RoleID, int DaysToComplete)
    {
      GVItem itemForMilestone = new GVItem();
      itemForMilestone.SubItems[0].Value = (object) new MilestoneLabel(ms);
      if (RoleID == 0)
      {
        itemForMilestone.SubItems[1].Text = this.getRoleName(ms.RoleID);
        itemForMilestone.SubItems[1].Tag = (object) ms.RoleID;
      }
      else
      {
        itemForMilestone.SubItems[1].Text = this.getRoleName(RoleID);
        itemForMilestone.SubItems[1].Tag = (object) RoleID;
      }
      itemForMilestone.SubItems[2].Value = (object) DaysToComplete;
      itemForMilestone.Tag = (object) ms;
      return itemForMilestone;
    }

    private EllieMae.EMLite.Workflow.Milestone getMilestoneByID(string milestoneId)
    {
      return this.allMilestones.FirstOrDefault<EllieMae.EMLite.Workflow.Milestone>((Func<EllieMae.EMLite.Workflow.Milestone, bool>) (x => string.Compare(x.MilestoneID, milestoneId, true) == 0));
    }

    private string getRoleName(int roleId)
    {
      RoleInfo roleInfo = ((IEnumerable<RoleInfo>) this.roles).FirstOrDefault<RoleInfo>((Func<RoleInfo, bool>) (x => x.RoleID == roleId));
      return roleInfo != null ? roleInfo.RoleName : "";
    }

    private DateTime AddDays(DateTime date, int dayCount)
    {
      DateTime date1 = date;
      if ((AutoDayCountSetting) this.session.StartupInfo.PolicySettings[(object) "Policies.MilestoneExpDayCount"] == AutoDayCountSetting.CalendarDays)
        date1 = dayCount == 0 ? date1.AddMinutes(1.0) : date1.AddDays((double) dayCount);
      else if ((AutoDayCountSetting) this.session.StartupInfo.PolicySettings[(object) "Policies.MilestoneExpDayCount"] == AutoDayCountSetting.CompanyDays)
      {
        try
        {
          date1 = this.session.SessionObjects.GetBusinessCalendar(CalendarType.Business).AddBusinessDays(date1, dayCount, true);
        }
        catch (ArgumentOutOfRangeException ex)
        {
          Tracing.Log(ApplyMilestoneTemplates.sw, nameof (ApplyMilestoneTemplates), TraceLevel.Error, ex.ToString());
        }
      }
      else if ((AutoDayCountSetting) this.session.StartupInfo.PolicySettings[(object) "Policies.MilestoneExpDayCount"] == AutoDayCountSetting.BusinessDays)
      {
        int num = dayCount;
        if (num == 0)
          date1 = date1.AddMinutes(1.0);
        while (num != 0)
        {
          date1 = date1.AddDays(1.0);
          if (date1.DayOfWeek < DayOfWeek.Saturday && date1.DayOfWeek > DayOfWeek.Sunday)
            --num;
        }
      }
      return date1;
    }

    private void gridView2_ItemDoubleClick(object sender, GVItemEventArgs e)
    {
      Session.Application.GetService<ILoanEditor>().OpenMilestoneLogReview((MilestoneLog) this.logRecords[e.Item.Index]);
    }

    private void sendEmailToUsersLosingAccess()
    {
      Borrower borrower = this.session.LoanData.CurrentBorrowerPair.Borrower;
      string str1 = "Loan access alert for ";
      if (borrower.LastName.Trim() != "")
        str1 = str1 + borrower.LastName + " ";
      string subject = str1 + "loan file";
      string str2 = "Dear <user>,\n\nA loan team member has applied a new milestone list to the loan ";
      if (borrower.ToString().Trim() != "")
        str2 = str2 + borrower.LastName + ", " + borrower.FirstName + " ";
      string str3 = str2 + "file. As a result, you are no longer assigned to any milestones for this loan which may result in you losing access to this loan file. You may have also lost access to this loan if you are not associated with the new milestone list.\n\nThe new milestone list being applied to this loan is: \n";
      foreach (MilestoneLog allMilestone in this.list.GetAllMilestones())
        str3 = str3 + allMilestone.Stage + "\n";
      string body = str3 + "\nSincerely, \nThe Encompass Team";
      List<string> userIDs = new List<string>();
      if (this.sendEmail.Count == 0)
        return;
      this.sendEmail.Keys.ToList<UserInfo>().ForEach((Action<UserInfo>) (item => userIDs.Add(item.Userid)));
      this.template = new TriggerEmailTemplate(subject, body, userIDs.ToArray(), new int[0], true);
    }

    public TriggerEmailTemplate EmailTemplate => this.template;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ApplyMilestoneTemplates));
      this.lblTitle = new Label();
      this.lblDesc = new Label();
      this.groupContainer1 = new GroupContainer();
      this.borderPanel1 = new BorderPanel();
      this.gvMilestones = new GridView();
      this.grpTemplates = new GroupContainer();
      this.gvTemplates = new GridView();
      this.lblCurrentTemplate = new Label();
      this.btnSelect = new Button();
      this.btnCancel = new Button();
      this.groupContainer2 = new GroupContainer();
      this.borderPanel2 = new BorderPanel();
      this.gridView2 = new GridView();
      this.emHelpLink1 = new EMHelpLink();
      this.gradientPanel1 = new GradientPanel();
      this.lblTemplateName = new Label();
      this.label1 = new Label();
      this.panel1 = new Panel();
      this.label3 = new Label();
      this.panel2 = new Panel();
      this.pictureBox1 = new PictureBox();
      this.groupContainer1.SuspendLayout();
      this.borderPanel1.SuspendLayout();
      this.grpTemplates.SuspendLayout();
      this.groupContainer2.SuspendLayout();
      this.borderPanel2.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      this.panel1.SuspendLayout();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
      this.lblTitle.AutoSize = true;
      this.lblTitle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblTitle.Location = new Point(59, 13);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Size = new Size(167, 13);
      this.lblTitle.TabIndex = 0;
      this.lblTitle.Text = "Select a milestone template.";
      this.lblTitle.Visible = false;
      this.lblDesc.Location = new Point(59, 30);
      this.lblDesc.Name = "lblDesc";
      this.lblDesc.Size = new Size(400, 22);
      this.lblDesc.TabIndex = 1;
      this.lblDesc.Text = "Current Template:  ";
      this.groupContainer1.Controls.Add((Control) this.borderPanel1);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(236, 73);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(390, 355);
      this.groupContainer1.TabIndex = 3;
      this.groupContainer1.Text = "Milestone Template";
      this.borderPanel1.Borders = AnchorStyles.Bottom;
      this.borderPanel1.Controls.Add((Control) this.gvMilestones);
      this.borderPanel1.Dock = DockStyle.Fill;
      this.borderPanel1.Location = new Point(1, 26);
      this.borderPanel1.Name = "borderPanel1";
      this.borderPanel1.Size = new Size(390, 328);
      this.borderPanel1.TabIndex = 1;
      this.gvMilestones.AllowDrop = true;
      this.gvMilestones.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Milestone";
      gvColumn1.Width = 125;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Role";
      gvColumn2.Width = 115;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Days";
      gvColumn3.Width = 131;
      this.gvMilestones.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gvMilestones.Dock = DockStyle.Fill;
      this.gvMilestones.DropTarget = GVDropTarget.BetweenItems;
      this.gvMilestones.Location = new Point(0, 0);
      this.gvMilestones.Name = "gvMilestones";
      this.gvMilestones.Size = new Size(383, 327);
      this.gvMilestones.SortOption = GVSortOption.None;
      this.gvMilestones.TabIndex = 1;
      this.grpTemplates.Controls.Add((Control) this.gvTemplates);
      this.grpTemplates.HeaderForeColor = SystemColors.ControlText;
      this.grpTemplates.Location = new Point(12, 73);
      this.grpTemplates.Name = "grpTemplates";
      this.grpTemplates.Size = new Size(219, 355);
      this.grpTemplates.TabIndex = 4;
      this.grpTemplates.Text = "Templates List";
      this.gvTemplates.AllowMultiselect = false;
      this.gvTemplates.BorderStyle = BorderStyle.None;
      this.gvTemplates.ClearSelectionsOnEmptyRowClick = false;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column2";
      gvColumn4.Text = "Name";
      gvColumn4.Width = 215;
      this.gvTemplates.Columns.AddRange(new GVColumn[1]
      {
        gvColumn4
      });
      this.gvTemplates.Dock = DockStyle.Fill;
      this.gvTemplates.Location = new Point(1, 26);
      this.gvTemplates.Name = "gvTemplates";
      this.gvTemplates.Size = new Size(217, 328);
      this.gvTemplates.SortOption = GVSortOption.None;
      this.gvTemplates.TabIndex = 1;
      this.gvTemplates.SelectedIndexChanged += new EventHandler(this.gvTemplates_SelectedIndexChanged);
      this.lblCurrentTemplate.AutoSize = true;
      this.lblCurrentTemplate.Location = new Point(105, 30);
      this.lblCurrentTemplate.Name = "lblCurrentTemplate";
      this.lblCurrentTemplate.Size = new Size(0, 13);
      this.lblCurrentTemplate.TabIndex = 5;
      this.btnSelect.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSelect.Location = new Point(425, 445);
      this.btnSelect.Name = "btnSelect";
      this.btnSelect.Size = new Size(116, 23);
      this.btnSelect.TabIndex = 6;
      this.btnSelect.Text = "Select and Continue";
      this.btnSelect.UseVisualStyleBackColor = true;
      this.btnSelect.Click += new EventHandler(this.btnSelect_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(547, 445);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 7;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.groupContainer2.Controls.Add((Control) this.borderPanel2);
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(81, 548);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(385, 355);
      this.groupContainer2.TabIndex = 4;
      this.groupContainer2.Text = "Current Log";
      this.groupContainer2.Visible = false;
      this.borderPanel2.Borders = AnchorStyles.Bottom;
      this.borderPanel2.Controls.Add((Control) this.gridView2);
      this.borderPanel2.Dock = DockStyle.Fill;
      this.borderPanel2.Location = new Point(1, 26);
      this.borderPanel2.Name = "borderPanel2";
      this.borderPanel2.Size = new Size(383, 328);
      this.borderPanel2.TabIndex = 1;
      this.gridView2.AllowDrop = true;
      this.gridView2.BorderStyle = BorderStyle.None;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column1";
      gvColumn5.Text = "Milestone";
      gvColumn5.Width = 125;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column2";
      gvColumn6.Text = "Role";
      gvColumn6.Width = 115;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column3";
      gvColumn7.Text = "Date";
      gvColumn7.Width = 133;
      this.gridView2.Columns.AddRange(new GVColumn[3]
      {
        gvColumn5,
        gvColumn6,
        gvColumn7
      });
      this.gridView2.Dock = DockStyle.Fill;
      this.gridView2.DropTarget = GVDropTarget.BetweenItems;
      this.gridView2.Location = new Point(0, 0);
      this.gridView2.Name = "gridView2";
      this.gridView2.Size = new Size(383, 327);
      this.gridView2.SortOption = GVSortOption.None;
      this.gridView2.TabIndex = 1;
      this.gridView2.ItemDoubleClick += new GVItemEventHandler(this.gridView2_ItemDoubleClick);
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "ApplyMilestoneTemplate";
      this.emHelpLink1.Location = new Point(12, 452);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 10;
      this.gradientPanel1.Controls.Add((Control) this.lblTemplateName);
      this.gradientPanel1.Controls.Add((Control) this.label1);
      this.gradientPanel1.GradientColor1 = Color.White;
      this.gradientPanel1.GradientColor2 = Color.White;
      this.gradientPanel1.Location = new Point(12, 50);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(614, 23);
      this.gradientPanel1.TabIndex = 12;
      this.lblTemplateName.AutoSize = true;
      this.lblTemplateName.BackColor = SystemColors.Window;
      this.lblTemplateName.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblTemplateName.Location = new Point(103, 4);
      this.lblTemplateName.Name = "lblTemplateName";
      this.lblTemplateName.Size = new Size(91, 13);
      this.lblTemplateName.TabIndex = 1;
      this.lblTemplateName.Text = "template Name";
      this.label1.AutoSize = true;
      this.label1.BackColor = SystemColors.Window;
      this.label1.Location = new Point(10, 4);
      this.label1.Name = "label1";
      this.label1.Size = new Size(94, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Current Template: ";
      this.panel1.Controls.Add((Control) this.label3);
      this.panel1.Location = new Point(277, 478);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(600, 133);
      this.panel1.TabIndex = 13;
      this.label3.AutoSize = false;
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(3, 4);
      this.label3.Name = "label3";
      this.label3.Size = new Size(600, 13);
      this.label3.TabIndex = 20;
      this.label3.Text = "The following users will no longer be assigned to a milestone and may lose access to this loan file:";
      this.panel2.BackgroundImage = (Image) componentResourceManager.GetObject("panel2.BackgroundImage");
      this.panel2.Location = new Point(118, 507);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(30, 150);
      this.panel2.TabIndex = 14;
      this.pictureBox1.Image = (Image) componentResourceManager.GetObject("pictureBox1.Image");
      this.pictureBox1.Location = new Point(12, 11);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(32, 32);
      this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 11;
      this.pictureBox1.TabStop = false;
      this.pictureBox1.Visible = false;
      this.AcceptButton = (IButtonControl) this.btnSelect;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(630, 477);
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.gradientPanel1);
      this.Controls.Add((Control) this.pictureBox1);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.groupContainer2);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSelect);
      this.Controls.Add((Control) this.lblCurrentTemplate);
      this.Controls.Add((Control) this.grpTemplates);
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.lblDesc);
      this.Controls.Add((Control) this.lblTitle);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ApplyMilestoneTemplates);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Apply Milestone Template";
      this.groupContainer1.ResumeLayout(false);
      this.borderPanel1.ResumeLayout(false);
      this.grpTemplates.ResumeLayout(false);
      this.groupContainer2.ResumeLayout(false);
      this.borderPanel2.ResumeLayout(false);
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
