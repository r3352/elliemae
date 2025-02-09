// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.MilestoneLogDiff
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine.Log;
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
  public class MilestoneLogDiff : Form
  {
    private RoleInfo[] roles;
    private IEnumerable<EllieMae.EMLite.Workflow.Milestone> allMilestones;
    private List<LogRecordBase> logRecords;
    private DateTime previousDate;
    private Font font;
    private bool manual;
    private MilestoneTemplate selectedTemplate;
    private int differentPoint;
    private Dictionary<UserInfo, List<string>> sendEmail = new Dictionary<UserInfo, List<string>>();
    private Dictionary<UserInfo, List<string>> dontSendEmail = new Dictionary<UserInfo, List<string>>();
    private static readonly string sw = Tracing.SwInputEngine;
    private const string className = "MilestoneLogDiff";
    private MilestoneLog[] currentMilestoneList;
    private SessionObjects sessionObjects;
    private string currentTemplateName;
    private IContainer components;
    private Label lblCurrentTemplate;
    private Button btnSelect;
    private Button btnCancel;
    private GroupContainer groupContainer2;
    private BorderPanel borderPanel2;
    private GridView gridView2;
    private EMHelpLink emHelpLink1;
    private Panel panel1;
    private Label label3;
    private Panel panel2;
    private GroupContainer groupContainer1;
    private BorderPanel borderPanel1;
    private GridView gvMilestones;
    private Panel panel3;
    private Label lblDesc;
    private Label lblTitle;
    private PictureBox pictureBox1;
    private Panel panel4;
    private Label label1;
    private Label label2;
    private Label label4;

    public MilestoneLogDiff(
      MilestoneTemplate newTemplate,
      MilestoneLog[] currentMilestoneList,
      int startingMilestoneToBeReplaced,
      RoleInfo[] roles,
      IEnumerable<EllieMae.EMLite.Workflow.Milestone> allMilestones,
      Dictionary<UserInfo, List<string>> sendEmail,
      Dictionary<UserInfo, List<string>> dontSendEmail,
      List<LogRecordBase> logRecords,
      SessionObjects sessionObjects,
      string currentTemplateName,
      bool manual)
    {
      this.manual = manual;
      this.selectedTemplate = newTemplate;
      this.currentMilestoneList = currentMilestoneList;
      this.sessionObjects = sessionObjects;
      this.roles = roles;
      this.allMilestones = allMilestones;
      this.sendEmail = sendEmail;
      this.dontSendEmail = dontSendEmail;
      this.logRecords = logRecords;
      this.currentTemplateName = currentTemplateName;
      this.init();
      this.populateTemplateMilestones(this.selectedTemplate);
      this.setUpView();
    }

    private void init()
    {
      this.InitializeComponent();
      this.font = new Font(this.gridView2.Font, FontStyle.Bold);
    }

    private void btnSelect_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    private void setUpView()
    {
      if (!this.manual)
      {
        this.pictureBox1.Visible = true;
        this.Text = "Milestone Log Change";
        this.lblTitle.Text = "Milestone Log Change";
        this.lblDesc.Text = "Based on the changes made to the loan file, the current milestone list is no longer accessible. The loan file will be changed to use the new milestone list shown on the right table and all other milestones will be removed.";
        this.btnSelect.Text = "OK";
      }
      else
      {
        this.lblTitle.Text = "Preview and Apply";
        this.lblDesc.Text = "If you apply the NEW milestone template, the milestones highlighted in the CURRENT milestone list will not change. All other milestones in the CURRENT list will be removed from the loan file. Click the Apply button to apply the NEW milestone template.";
        this.btnSelect.Text = "Apply";
      }
      this.lblTitle.Visible = true;
      this.lblCurrentTemplate.Visible = false;
      this.gvMilestones.Columns[2].Text = "Date";
      this.groupContainer2.Visible = true;
      this.groupContainer2.Text = "CURRENT - " + this.currentTemplateName;
      this.populateCurrentTemplateMilestone();
      this.highlightDifferentMilestones();
      this.showRoles();
      this.ClientSize = new Size(872, 500 + this.panel1.Size.Height);
      this.btnSelect.Location = new Point(708, this.btnSelect.Location.Y);
    }

    private void highlightDifferentMilestones()
    {
      int i = 0;
      AutoDayCountSetting calendarType = (AutoDayCountSetting) this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.MilestoneExpDayCount"];
      ((IEnumerable<MilestoneLog>) this.currentMilestoneList).TakeWhile<MilestoneLog>((Func<MilestoneLog, int, bool>) ((item, index) => item.MilestoneID.Equals(this.selectedTemplate.SequentialMilestones[index].MilestoneID))).ToList<MilestoneLog>().ForEach((Action<MilestoneLog>) (item =>
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
          DateTime dateTime1 = item.Date;
          DateTime date1 = dateTime1.Date;
          dateTime1 = DateTime.MaxValue;
          DateTime date2 = dateTime1.Date;
          if (date1 != date2)
          {
            DateTime dateTime2 = item.Date;
            DateTime date3 = dateTime2.Date;
            dateTime2 = DateTime.MinValue;
            DateTime date4 = dateTime2.Date;
            if (date3 != date4)
            {
              this.previousDate = item.Date;
              goto label_6;
            }
          }
          this.previousDate = this.AddDays(this.previousDate, logRecord.Days, calendarType, this.sessionObjects);
label_6:
          this.gridView2.Items[i].SubItems[2].Value = (object) this.previousDate.ToString();
        }
        this.gvMilestones.Items[i].ForeColor = Color.Gray;
        ++i;
      }));
      for (int index = 0; index < i; ++index)
      {
        if (index >= 1 && index == i - 1)
        {
          this.gvMilestones.Items[index].SubItems[2].Value = !((MilestoneLog) this.logRecords[index]).Done ? (this.gridView2.Items[index].SubItems[2].Value == null ? (object) this.logRecords[index].Date.ToString() : (object) this.gridView2.Items[index].SubItems[2].Value.ToString()) : (object) (this.previousDate = DateTime.Now);
          this.gvMilestones.Items[index].SubItems.ToList<GVSubItem>().ForEach((Action<GVSubItem>) (em => em.Font = this.gvMilestones.Font));
          this.gvMilestones.Items[index].BackColor = Color.GreenYellow;
          this.gvMilestones.Items[index].ForeColor = Color.Black;
        }
        else
          this.gvMilestones.Items[index] = this.gridView2.Items[index];
      }
      for (int index = i; index < this.selectedTemplate.SequentialMilestones.Count(); ++index)
      {
        this.gvMilestones.Items[index].BackColor = Color.GreenYellow;
        this.previousDate = this.AddDays(this.previousDate, this.selectedTemplate.SequentialMilestones[index].DaysToComplete, calendarType, this.sessionObjects);
        this.gvMilestones.Items[index].SubItems[2].Value = (object) this.previousDate.ToString();
        if (index < this.gridView2.Items.Count)
          this.gridView2.Items[index].ForeColor = Color.Gray;
      }
      for (int nItemIndex = this.selectedTemplate.SequentialMilestones.Count(); nItemIndex < ((IEnumerable<MilestoneLog>) this.currentMilestoneList).Count<MilestoneLog>(); ++nItemIndex)
        this.gridView2.Items[nItemIndex].ForeColor = Color.Gray;
      this.differentPoint = i;
      this.previousDate = ((IEnumerable<MilestoneLog>) this.currentMilestoneList).ElementAt<MilestoneLog>(i - 1).Date;
      if (this.previousDate.Date == DateTime.MaxValue.Date || this.previousDate.Date == DateTime.MinValue.Date)
        this.previousDate = Convert.ToDateTime(this.gridView2.Items[i - 1].SubItems[2].Value);
      for (int index = i; index < ((IEnumerable<MilestoneLog>) this.currentMilestoneList).Count<MilestoneLog>(); ++index)
      {
        MilestoneLog milestoneLog = ((IEnumerable<MilestoneLog>) this.currentMilestoneList).ElementAt<MilestoneLog>(index);
        if (milestoneLog.Done)
        {
          this.previousDate = milestoneLog.Date;
          this.gridView2.Items[index].SubItems.ToList<GVSubItem>().ForEach((Action<GVSubItem>) (em => em.Font = this.font));
        }
        else
        {
          DateTime dateTime = milestoneLog.Date;
          DateTime date5 = dateTime.Date;
          dateTime = DateTime.MaxValue;
          DateTime date6 = dateTime.Date;
          if (date5 != date6)
          {
            dateTime = milestoneLog.Date;
            DateTime date7 = dateTime.Date;
            dateTime = DateTime.MinValue;
            DateTime date8 = dateTime.Date;
            if (date7 != date8)
            {
              this.previousDate = milestoneLog.Date;
              goto label_23;
            }
          }
          this.previousDate = this.AddDays(this.previousDate, milestoneLog.Days, calendarType, this.sessionObjects);
        }
label_23:
        this.gridView2.Items[index].SubItems[2].Value = (object) this.previousDate;
      }
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

    private void populateTemplateMilestones(MilestoneTemplate template)
    {
      this.gvMilestones.Items.Clear();
      this.groupContainer1.Text = "NEW - " + template.Name;
      foreach (TemplateMilestone sequentialMilestone in template.SequentialMilestones)
        this.gvMilestones.Items.Add(this.createGVItemForTemplateMilestone(sequentialMilestone));
    }

    private void populateCurrentTemplateMilestone()
    {
      this.gridView2.Items.Clear();
      foreach (MilestoneLog currentMilestone in this.currentMilestoneList)
        this.gridView2.Items.Add(this.createGVItemForMilestoneLog(currentMilestone));
    }

    private GVItem createGVItemForTemplateMilestone(TemplateMilestone templateMs)
    {
      return this.createGVItemForMilestone(this.getMilestoneByID(templateMs.MilestoneID), templateMs.RoleID, templateMs.DaysToComplete);
    }

    private GVItem createGVItemForMilestoneLog(MilestoneLog log)
    {
      return this.createGVItemForMilestone(this.getMilestoneByMilestoneLog(log), log.RoleID, log.Days);
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

    private EllieMae.EMLite.Workflow.Milestone getMilestoneByID(string milestoneID)
    {
      return this.allMilestones.FirstOrDefault<EllieMae.EMLite.Workflow.Milestone>((Func<EllieMae.EMLite.Workflow.Milestone, bool>) (x => string.Compare(x.MilestoneID, milestoneID, true) == 0));
    }

    private EllieMae.EMLite.Workflow.Milestone getMilestoneByMilestoneLog(MilestoneLog log)
    {
      EllieMae.EMLite.Workflow.Milestone milestoneByMilestoneLog = this.allMilestones.FirstOrDefault<EllieMae.EMLite.Workflow.Milestone>((Func<EllieMae.EMLite.Workflow.Milestone, bool>) (x => string.Compare(x.MilestoneID, log.MilestoneID, true) == 0));
      if (milestoneByMilestoneLog == null)
        milestoneByMilestoneLog = new EllieMae.EMLite.Workflow.Milestone(log.MilestoneID, log.SortIndex, log.RoleID)
        {
          Name = log.Stage,
          Archived = false,
          DefaultDays = log.Days,
          DescTextAfter = log.DoneText,
          DescTextBefore = log.ExpText,
          DisplayColor = Color.WhiteSmoke,
          RoleRequired = log.RoleRequired == "Y"
        };
      return milestoneByMilestoneLog;
    }

    private string getRoleName(int roleId)
    {
      RoleInfo roleInfo = ((IEnumerable<RoleInfo>) this.roles).FirstOrDefault<RoleInfo>((Func<RoleInfo, bool>) (x => x.RoleID == roleId));
      return roleInfo != null ? roleInfo.RoleName : "";
    }

    private DateTime AddDays(
      DateTime date,
      int dayCount,
      AutoDayCountSetting calenderType,
      SessionObjects sessionObjects)
    {
      DateTime date1 = date;
      switch (calenderType)
      {
        case AutoDayCountSetting.CalendarDays:
          date1 = dayCount == 0 ? date1.AddMinutes(1.0) : date1.AddDays((double) dayCount);
          break;
        case AutoDayCountSetting.CompanyDays:
          try
          {
            date1 = sessionObjects.GetBusinessCalendar(CalendarType.Business).AddBusinessDays(date1, dayCount, false);
            break;
          }
          catch (ArgumentOutOfRangeException ex)
          {
            Tracing.Log(MilestoneLogDiff.sw, nameof (MilestoneLogDiff), TraceLevel.Error, ex.ToString());
            break;
          }
        default:
          int num = dayCount;
          if (num == 0)
            date1 = date1.AddMinutes(1.0);
          while (num != 0)
          {
            date1 = date1.AddDays(1.0);
            if (date1.DayOfWeek < DayOfWeek.Saturday && date1.DayOfWeek > DayOfWeek.Sunday)
              --num;
          }
          break;
      }
      return date1;
    }

    private void gridView2_ItemDoubleClick(object sender, GVItemEventArgs e)
    {
      Session.Application.GetService<ILoanEditor>().OpenMilestoneLogReview((MilestoneLog) this.logRecords[e.Item.Index]);
    }

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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MilestoneLogDiff));
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      this.lblCurrentTemplate = new Label();
      this.btnSelect = new Button();
      this.btnCancel = new Button();
      this.groupContainer2 = new GroupContainer();
      this.borderPanel2 = new BorderPanel();
      this.gridView2 = new GridView();
      this.emHelpLink1 = new EMHelpLink();
      this.panel1 = new Panel();
      this.label3 = new Label();
      this.panel2 = new Panel();
      this.groupContainer1 = new GroupContainer();
      this.borderPanel1 = new BorderPanel();
      this.gvMilestones = new GridView();
      this.panel3 = new Panel();
      this.lblDesc = new Label();
      this.lblTitle = new Label();
      this.pictureBox1 = new PictureBox();
      this.panel4 = new Panel();
      this.label1 = new Label();
      this.label2 = new Label();
      this.label4 = new Label();
      this.groupContainer2.SuspendLayout();
      this.borderPanel2.SuspendLayout();
      this.panel1.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.borderPanel1.SuspendLayout();
      this.panel3.SuspendLayout();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
      this.lblCurrentTemplate.AutoSize = true;
      this.lblCurrentTemplate.Location = new Point(105, 30);
      this.lblCurrentTemplate.Name = "lblCurrentTemplate";
      this.lblCurrentTemplate.Size = new Size(0, 13);
      this.lblCurrentTemplate.TabIndex = 5;
      this.btnSelect.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSelect.Location = new Point(708, 612);
      this.btnSelect.Name = "btnSelect";
      this.btnSelect.Size = new Size(75, 23);
      this.btnSelect.TabIndex = 6;
      this.btnSelect.Text = "Apply";
      this.btnSelect.UseVisualStyleBackColor = true;
      this.btnSelect.Click += new EventHandler(this.btnSelect_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(789, 612);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 7;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.groupContainer2.Controls.Add((Control) this.borderPanel2);
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(12, 70);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(408, 355);
      this.groupContainer2.TabIndex = 4;
      this.groupContainer2.Text = "Current Log";
      this.groupContainer2.Visible = false;
      this.borderPanel2.Borders = AnchorStyles.Bottom;
      this.borderPanel2.Controls.Add((Control) this.gridView2);
      this.borderPanel2.Dock = DockStyle.Fill;
      this.borderPanel2.Location = new Point(1, 26);
      this.borderPanel2.Name = "borderPanel2";
      this.borderPanel2.Size = new Size(406, 328);
      this.borderPanel2.TabIndex = 1;
      this.gridView2.AllowDrop = true;
      this.gridView2.BorderStyle = BorderStyle.None;
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
      gvColumn3.Text = "Date";
      gvColumn3.Width = 133;
      this.gridView2.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gridView2.Dock = DockStyle.Fill;
      this.gridView2.DropTarget = GVDropTarget.BetweenItems;
      this.gridView2.Location = new Point(0, 0);
      this.gridView2.Name = "gridView2";
      this.gridView2.Size = new Size(406, 327);
      this.gridView2.SortOption = GVSortOption.None;
      this.gridView2.TabIndex = 1;
      this.gridView2.ItemDoubleClick += new GVItemEventHandler(this.gridView2_ItemDoubleClick);
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "ApplyMilestoneTemplate";
      this.emHelpLink1.Location = new Point(12, 619);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 10;
      this.panel1.Controls.Add((Control) this.label3);
      this.panel1.Location = new Point(13, 435);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(847, 133);
      this.panel1.TabIndex = 13;
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(3, 4);
      this.label3.Name = "label3";
      this.label3.Size = new Size(600, 13);
      this.label3.TabIndex = 20;
      this.label3.Text = "The following users will no longer be assigned to a milestone and may lose access to this loan file:";
      this.panel2.BackgroundImage = (Image) componentResourceManager.GetObject("panel2.BackgroundImage");
      this.panel2.Location = new Point(422, 155);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(30, 150);
      this.panel2.TabIndex = 14;
      this.groupContainer1.Controls.Add((Control) this.borderPanel1);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(454, 70);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(406, 355);
      this.groupContainer1.TabIndex = 3;
      this.groupContainer1.Text = "Milestone Template";
      this.borderPanel1.Borders = AnchorStyles.Bottom;
      this.borderPanel1.Controls.Add((Control) this.gvMilestones);
      this.borderPanel1.Dock = DockStyle.Fill;
      this.borderPanel1.Location = new Point(1, 26);
      this.borderPanel1.Name = "borderPanel1";
      this.borderPanel1.Size = new Size(404, 328);
      this.borderPanel1.TabIndex = 1;
      this.gvMilestones.AllowDrop = true;
      this.gvMilestones.BorderStyle = BorderStyle.None;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column1";
      gvColumn4.Text = "Milestone";
      gvColumn4.Width = 125;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column2";
      gvColumn5.Text = "Role";
      gvColumn5.Width = 115;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column3";
      gvColumn6.Text = "Days";
      gvColumn6.Width = 131;
      this.gvMilestones.Columns.AddRange(new GVColumn[3]
      {
        gvColumn4,
        gvColumn5,
        gvColumn6
      });
      this.gvMilestones.Dock = DockStyle.Fill;
      this.gvMilestones.DropTarget = GVDropTarget.BetweenItems;
      this.gvMilestones.Location = new Point(0, 0);
      this.gvMilestones.Name = "gvMilestones";
      this.gvMilestones.Size = new Size(404, 327);
      this.gvMilestones.SortOption = GVSortOption.None;
      this.gvMilestones.TabIndex = 1;
      this.panel3.Controls.Add((Control) this.lblDesc);
      this.panel3.Controls.Add((Control) this.lblTitle);
      this.panel3.Controls.Add((Control) this.pictureBox1);
      this.panel3.Location = new Point(13, 12);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(846, 39);
      this.panel3.TabIndex = 16;
      this.lblDesc.Dock = DockStyle.Top;
      this.lblDesc.Location = new Point(32, 13);
      this.lblDesc.Name = "lblDesc";
      this.lblDesc.Size = new Size(814, 50);
      this.lblDesc.TabIndex = 14;
      this.lblDesc.Text = "Current Template:  ";
      this.lblTitle.AutoSize = true;
      this.lblTitle.Dock = DockStyle.Top;
      this.lblTitle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblTitle.Location = new Point(32, 0);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Size = new Size(167, 13);
      this.lblTitle.TabIndex = 0;
      this.lblTitle.Text = "Select a milestone template.";
      this.lblTitle.Visible = false;
      this.pictureBox1.Dock = DockStyle.Left;
      this.pictureBox1.Image = (Image) componentResourceManager.GetObject("pictureBox1.Image");
      this.pictureBox1.Location = new Point(0, 0);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(32, 39);
      this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 11;
      this.pictureBox1.TabStop = false;
      this.pictureBox1.Visible = false;
      this.panel4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.panel4.BackColor = Color.GreenYellow;
      this.panel4.Location = new Point(14, 578);
      this.panel4.Name = "panel4";
      this.panel4.Size = new Size(15, 15);
      this.panel4.TabIndex = 17;
      this.label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(32, 579);
      this.label1.Name = "label1";
      this.label1.Size = new Size(177, 13);
      this.label1.TabIndex = 18;
      this.label1.Text = "Green highlighted rows are retained.";
      this.label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(31, 597);
      this.label2.Name = "label2";
      this.label2.Size = new Size(32, 13);
      this.label2.TabIndex = 19;
      this.label2.Text = "Bold";
      this.label4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(59, 597);
      this.label4.Name = "label4";
      this.label4.Size = new Size(129, 13);
      this.label4.TabIndex = 20;
      this.label4.Text = "are completed milestones.";
      this.AcceptButton = (IButtonControl) this.btnSelect;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(872, 644);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.panel4);
      this.Controls.Add((Control) this.panel3);
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.groupContainer2);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSelect);
      this.Controls.Add((Control) this.lblCurrentTemplate);
      this.Controls.Add((Control) this.groupContainer1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (MilestoneLogDiff);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Apply Milestone Template";
      this.groupContainer2.ResumeLayout(false);
      this.borderPanel2.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.groupContainer1.ResumeLayout(false);
      this.borderPanel1.ResumeLayout(false);
      this.panel3.ResumeLayout(false);
      this.panel3.PerformLayout();
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
