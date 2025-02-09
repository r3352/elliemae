// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.MilestoneHistoryChangeSheet
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class MilestoneHistoryChangeSheet : UserControl
  {
    private MilestoneHistoryLog previous;
    private MilestoneHistoryLog current;
    private IEnumerable<EllieMae.EMLite.Workflow.Milestone> allMilestones;
    private RoleInfo[] roles;
    private Font font;
    private IContainer components;
    private MilestoneHeader topPanel;
    private Label label1;
    private Panel panel1;
    private Label lblUser;
    private Label lblDate;
    private GroupContainer groupContainer1;
    private GroupContainer grpNew;
    private GroupContainer grpOld;
    private GridView gvPreviousMilestones;
    private GridView gvCurrentMilestones;
    private Label lblUserFeed;
    private Label lblDateFeed;
    private Label label2;
    private Label lblReasonFeed;
    private Panel panel2;

    public MilestoneHistoryChangeSheet(MilestoneHistoryLog log)
    {
      this.InitializeComponent();
      this.font = new Font(this.gvPreviousMilestones.Font, FontStyle.Bold);
      this.previous = log;
      this.allMilestones = ((MilestoneTemplatesBpmManager) Session.BPM.GetBpmManager(BpmCategory.Milestones)).GetAllMilestonesList();
      this.roles = Session.SessionObjects.BpmManager.GetAllRoleFunctions();
      LogRecordBase[] allRecordsOfType = Session.LoanData.GetLogList().GetAllRecordsOfType(typeof (MilestoneHistoryLog));
      int num = ((IEnumerable<LogRecordBase>) allRecordsOfType).ToList<LogRecordBase>().IndexOf((LogRecordBase) log);
      if (num == -1)
        return;
      this.current = num == ((IEnumerable<LogRecordBase>) allRecordsOfType).Count<LogRecordBase>() - 1 ? (MilestoneHistoryLog) null : (MilestoneHistoryLog) allRecordsOfType[num + 1];
      this.label1.Text = "Milestone Log Change";
      this.lblDateFeed.Text = this.previous.DateAddedUtc.ToString();
      this.lblUserFeed.Text = Session.OrganizationManager.GetUser(this.previous.AddedByUserId).FullName + " (" + this.previous.AddedByUserId + ")";
      this.lblReasonFeed.Text = this.previous.ChangeReason;
      this.grpOld.Text += this.previous.MilestoneTemplate;
      this.grpNew.Text = this.current == null ? this.grpNew.Text + Session.LoanData.GetLogList().MilestoneTemplate.Name : this.grpNew.Text + this.current.MilestoneTemplate;
      this.populatePreviousGrid();
      this.populateCurrentGrid();
      this.hightlightCommonMilestones();
      this.Dock = DockStyle.Fill;
    }

    private void populatePreviousGrid()
    {
      foreach (MilestoneLog milestone in this.previous.Milestones)
        this.gvPreviousMilestones.Items.Add(this.createGVItemForMilestoneLog(milestone, true));
    }

    private void populateCurrentGrid()
    {
      if (this.current != null)
      {
        foreach (MilestoneLog milestone in this.current.Milestones)
          this.gvCurrentMilestones.Items.Add(this.createGVItemForMilestoneLog(milestone, false));
      }
      else
      {
        foreach (MilestoneLog allMilestone in Session.LoanData.GetLogList().GetAllMilestones())
          this.gvCurrentMilestones.Items.Add(this.createGVItemForMilestoneLatest(allMilestone));
      }
    }

    private void hightlightCommonMilestones()
    {
      int num = 0;
      for (int nItemIndex = 0; nItemIndex < this.gvCurrentMilestones.Items.Count; ++nItemIndex)
      {
        if (((MilestoneLog) this.gvPreviousMilestones.Items[nItemIndex].Tag).MilestoneID == ((MilestoneLog) this.gvCurrentMilestones.Items[nItemIndex].Tag).MilestoneID)
        {
          this.gvPreviousMilestones.Items[nItemIndex].BackColor = Color.GreenYellow;
        }
        else
        {
          num = nItemIndex;
          break;
        }
      }
      for (int nItemIndex = num; nItemIndex < this.gvPreviousMilestones.Items.Count; ++nItemIndex)
        this.gvPreviousMilestones.Items[nItemIndex].ForeColor = Color.Gray;
    }

    private GVItem createGVItemForMilestoneLog(MilestoneLog ms, bool doBlue)
    {
      EllieMae.EMLite.Workflow.Milestone milestoneById = this.getMilestoneByID(ms.MilestoneID);
      GVItem itemForMilestoneLog = new GVItem();
      itemForMilestoneLog.SubItems[0].Value = (object) new MilestoneLabel(milestoneById);
      if (doBlue)
        itemForMilestoneLog.SubItems[0].ForeColor = Color.Blue;
      else
        itemForMilestoneLog.BackColor = Color.GreenYellow;
      itemForMilestoneLog.SubItems[1].Text = this.getRoleName(ms.RoleID);
      itemForMilestoneLog.SubItems[1].Tag = (object) ms.RoleID;
      if (ms.Date.Date != DateTime.MaxValue.Date)
        itemForMilestoneLog.SubItems[2].Value = (object) ms.Date;
      if (ms.Done)
        itemForMilestoneLog.SubItems.ToList<GVSubItem>().ForEach((Action<GVSubItem>) (em => em.Font = this.font));
      itemForMilestoneLog.Tag = (object) ms;
      return itemForMilestoneLog;
    }

    private GVItem createGVItemForMilestoneLatest(MilestoneLog milestones)
    {
      EllieMae.EMLite.Workflow.Milestone milestoneById = this.getMilestoneByID(milestones.MilestoneID);
      GVItem forMilestoneLatest = new GVItem();
      forMilestoneLatest.SubItems[0].Value = (object) new MilestoneLabel(milestoneById);
      forMilestoneLatest.SubItems[1].Text = this.getRoleName(milestones.RoleID);
      forMilestoneLatest.SubItems[1].Tag = (object) milestones.RoleID;
      DateTime dateTime = milestones.Date;
      DateTime date1 = dateTime.Date;
      dateTime = DateTime.MaxValue;
      DateTime date2 = dateTime.Date;
      if (date1 != date2)
        forMilestoneLatest.SubItems[2].Value = (object) milestones.Date;
      forMilestoneLatest.Tag = (object) milestones;
      forMilestoneLatest.BackColor = Color.GreenYellow;
      if (milestones.Done)
        forMilestoneLatest.SubItems.ToList<GVSubItem>().ForEach((Action<GVSubItem>) (em => em.Font = this.font));
      return forMilestoneLatest;
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

    private void gvPreviousMilestones_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      Session.Application.GetService<ILoanEditor>().OpenMilestoneLogReview((MilestoneLog) this.gvPreviousMilestones.Items[e.Item.Index].Tag, this.previous);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MilestoneHistoryChangeSheet));
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      this.panel1 = new Panel();
      this.lblReasonFeed = new Label();
      this.label2 = new Label();
      this.lblUserFeed = new Label();
      this.lblDateFeed = new Label();
      this.lblUser = new Label();
      this.lblDate = new Label();
      this.groupContainer1 = new GroupContainer();
      this.panel2 = new Panel();
      this.grpNew = new GroupContainer();
      this.gvCurrentMilestones = new GridView();
      this.grpOld = new GroupContainer();
      this.gvPreviousMilestones = new GridView();
      this.topPanel = new MilestoneHeader();
      this.label1 = new Label();
      this.panel1.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.grpNew.SuspendLayout();
      this.grpOld.SuspendLayout();
      this.topPanel.SuspendLayout();
      this.SuspendLayout();
      this.panel1.Controls.Add((Control) this.lblReasonFeed);
      this.panel1.Controls.Add((Control) this.label2);
      this.panel1.Controls.Add((Control) this.lblUserFeed);
      this.panel1.Controls.Add((Control) this.lblDateFeed);
      this.panel1.Controls.Add((Control) this.lblUser);
      this.panel1.Controls.Add((Control) this.lblDate);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(0, 26);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(900, 65);
      this.panel1.TabIndex = 1;
      this.lblReasonFeed.AutoSize = true;
      this.lblReasonFeed.Location = new Point(164, 46);
      this.lblReasonFeed.Name = "lblReasonFeed";
      this.lblReasonFeed.Size = new Size(0, 13);
      this.lblReasonFeed.TabIndex = 5;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(8, 46);
      this.label2.Name = "label2";
      this.label2.Size = new Size(90, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "Change Reason: ";
      this.lblUserFeed.AutoSize = true;
      this.lblUserFeed.Location = new Point(164, 28);
      this.lblUserFeed.Name = "lblUserFeed";
      this.lblUserFeed.Size = new Size(0, 13);
      this.lblUserFeed.TabIndex = 3;
      this.lblDateFeed.AutoSize = true;
      this.lblDateFeed.Location = new Point(164, 10);
      this.lblDateFeed.Name = "lblDateFeed";
      this.lblDateFeed.Size = new Size(0, 13);
      this.lblDateFeed.TabIndex = 2;
      this.lblUser.AutoSize = true;
      this.lblUser.Location = new Point(8, 28);
      this.lblUser.Name = "lblUser";
      this.lblUser.Size = new Size(70, 13);
      this.lblUser.TabIndex = 1;
      this.lblUser.Text = "Changed by: ";
      this.lblDate.AutoSize = true;
      this.lblDate.Location = new Point(8, 10);
      this.lblDate.Name = "lblDate";
      this.lblDate.Size = new Size(129, 13);
      this.lblDate.TabIndex = 0;
      this.lblDate.Text = "Date and Time Changed: ";
      this.groupContainer1.Controls.Add((Control) this.panel2);
      this.groupContainer1.Controls.Add((Control) this.grpNew);
      this.groupContainer1.Controls.Add((Control) this.grpOld);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 91);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(900, 505);
      this.groupContainer1.TabIndex = 2;
      this.groupContainer1.Text = "Milestone List Changes";
      this.panel2.BackgroundImage = (Image) componentResourceManager.GetObject("panel2.BackgroundImage");
      this.panel2.Location = new Point(428, 126);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(29, 155);
      this.panel2.TabIndex = 2;
      this.grpNew.Controls.Add((Control) this.gvCurrentMilestones);
      this.grpNew.HeaderForeColor = SystemColors.ControlText;
      this.grpNew.Location = new Point(464, 42);
      this.grpNew.Name = "grpNew";
      this.grpNew.Size = new Size(400, 355);
      this.grpNew.TabIndex = 1;
      this.grpNew.Text = "NEW - ";
      this.gvCurrentMilestones.AllowDrop = true;
      this.gvCurrentMilestones.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Milestone";
      gvColumn1.Width = 125;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Role";
      gvColumn2.Width = 125;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Date";
      gvColumn3.Width = 130;
      this.gvCurrentMilestones.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gvCurrentMilestones.Dock = DockStyle.Fill;
      this.gvCurrentMilestones.DropTarget = GVDropTarget.BetweenItems;
      this.gvCurrentMilestones.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.gvCurrentMilestones.Location = new Point(1, 26);
      this.gvCurrentMilestones.Name = "gvCurrentMilestones";
      this.gvCurrentMilestones.Size = new Size(398, 328);
      this.gvCurrentMilestones.SortOption = GVSortOption.None;
      this.gvCurrentMilestones.TabIndex = 2;
      this.grpOld.Controls.Add((Control) this.gvPreviousMilestones);
      this.grpOld.HeaderForeColor = SystemColors.ControlText;
      this.grpOld.Location = new Point(21, 42);
      this.grpOld.Name = "grpOld";
      this.grpOld.Size = new Size(400, 355);
      this.grpOld.TabIndex = 0;
      this.grpOld.Text = "OLD - ";
      this.gvPreviousMilestones.AllowDrop = true;
      this.gvPreviousMilestones.BorderStyle = BorderStyle.None;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column1";
      gvColumn4.Text = "Milestone";
      gvColumn4.Width = 125;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column2";
      gvColumn5.Text = "Role";
      gvColumn5.Width = 125;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column3";
      gvColumn6.Text = "Date";
      gvColumn6.Width = 130;
      this.gvPreviousMilestones.Columns.AddRange(new GVColumn[3]
      {
        gvColumn4,
        gvColumn5,
        gvColumn6
      });
      this.gvPreviousMilestones.Dock = DockStyle.Fill;
      this.gvPreviousMilestones.DropTarget = GVDropTarget.BetweenItems;
      this.gvPreviousMilestones.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.gvPreviousMilestones.Location = new Point(1, 26);
      this.gvPreviousMilestones.Name = "gvPreviousMilestones";
      this.gvPreviousMilestones.Size = new Size(398, 328);
      this.gvPreviousMilestones.SortOption = GVSortOption.None;
      this.gvPreviousMilestones.TabIndex = 2;
      this.gvPreviousMilestones.ItemDoubleClick += new GVItemEventHandler(this.gvPreviousMilestones_ItemDoubleClick);
      this.topPanel.BackColor = SystemColors.GradientInactiveCaption;
      this.topPanel.Controls.Add((Control) this.label1);
      this.topPanel.Dock = DockStyle.Top;
      this.topPanel.Location = new Point(0, 0);
      this.topPanel.Name = "topPanel";
      this.topPanel.Size = new Size(900, 26);
      this.topPanel.TabIndex = 0;
      this.label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label1.BackColor = Color.Transparent;
      this.label1.Font = new Font("Arial Black", 9f, FontStyle.Bold);
      this.label1.ForeColor = Color.Red;
      this.label1.Location = new Point(8, 6);
      this.label1.Name = "label1";
      this.label1.Size = new Size(743, 20);
      this.label1.TabIndex = 0;
      this.label1.Text = "label1";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.topPanel);
      this.Name = nameof (MilestoneHistoryChangeSheet);
      this.Size = new Size(900, 596);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.groupContainer1.ResumeLayout(false);
      this.grpNew.ResumeLayout(false);
      this.grpOld.ResumeLayout(false);
      this.topPanel.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
