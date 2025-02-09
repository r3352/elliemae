// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.MSWorksheet.MilestoneSelectionDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.MSWorksheet
{
  public class MilestoneSelectionDialog : Form
  {
    private Sessions.Session session;
    private WorkflowManager workflowMgr;
    private List<EllieMae.EMLite.Workflow.Milestone> msList;
    private AlertConfig alertConfig;
    private List<string> milestoneGuidList;
    private Dictionary<WorksheetInfo, string> alertMsgsToUpdate = new Dictionary<WorksheetInfo, string>();
    private bool dirty;
    private bool suspendEventHandler = true;
    private IContainer components;
    private GridView lsvMilestone;
    private Label lblSubTitle;
    private Button btnOK;
    private Button btnCancel;
    private CheckBox chbCheckAll;
    private GroupContainer groupContainer1;

    public MilestoneSelectionDialog(Sessions.Session session, AlertConfig alertConfig)
    {
      this.InitializeComponent();
      this.session = session;
      this.alertConfig = alertConfig;
      this.initPageSetting();
    }

    private void initPageSetting()
    {
      this.suspendEventHandler = true;
      if (this.alertConfig.AlertID == 0)
      {
        this.workflowMgr = (WorkflowManager) this.session.BPM.GetBpmManager(BpmCategory.Workflow);
        this.Text = "Select Milestones and Configure Alert Messages";
        this.lblSubTitle.Text = "Select the milestones to enable the alert and configure the alert message for each enabled milestone.";
      }
      else
      {
        this.Width -= this.lsvMilestone.Columns[1].Width;
        this.lsvMilestone.Columns.RemoveAt(1);
        this.Text = "Select Milestones";
        this.lblSubTitle.Text = "Select the milestones to enable the alert.";
      }
      this.msList = this.session.StartupInfo.Milestones;
      foreach (EllieMae.EMLite.Workflow.Milestone ms in this.msList)
      {
        if (!(ms.Name == "Completion") && !ms.Archived)
          this.populateGridMilestone(ms);
      }
      this.suspendEventHandler = false;
    }

    private void populateGridMilestone(EllieMae.EMLite.Workflow.Milestone ms)
    {
      GVItem currItem = new GVItem((object) new MilestoneLabel(ms));
      bool msSelected = this.alertConfig.MilestoneGuidList.Contains(ms.MilestoneID);
      if (msSelected)
        currItem.SubItems[0].Checked = true;
      if (this.workflowMgr != null)
      {
        TextBox txtBox = new TextBox();
        txtBox.BorderStyle = BorderStyle.None;
        txtBox.GotFocus += new EventHandler(this.txtBox_GotFocus);
        txtBox.LostFocus += new EventHandler(this.txtBox_LostFocus);
        txtBox.TextChanged += new EventHandler(this.txtBoxAlertMsg_TextChanged);
        this.setAlertMsgTextBoxProperty(ms.MilestoneID, msSelected, txtBox, currItem);
        currItem.SubItems[1] = new GVSubItem((object) txtBox);
        txtBox.Tag = (object) currItem.SubItems[1];
        currItem.SubItems[1].Tag = (object) ms.MilestoneID;
      }
      currItem.Tag = (object) ms;
      this.lsvMilestone.Items.Add(currItem);
    }

    private void txtBox_GotFocus(object sender, EventArgs e)
    {
      GVSubItem tag = (GVSubItem) ((Control) sender).Tag;
      tag.BackColor = Color.Blue;
      tag.ForeColor = Color.White;
    }

    private void txtBox_LostFocus(object sender, EventArgs e)
    {
      GVSubItem tag = (GVSubItem) ((Control) sender).Tag;
      tag.BackColor = tag.Item.Index % 2 == 0 ? this.lsvMilestone.BackColor : this.lsvMilestone.AlternateBackground;
      tag.ForeColor = this.lsvMilestone.ForeColor;
    }

    private void txtBoxAlertMsg_TextChanged(object sender, EventArgs e)
    {
      if (this.suspendEventHandler)
        return;
      TextBox textBox = (TextBox) sender;
      string tag = (string) ((GVSubItem) textBox.Tag).Tag;
      EllieMae.EMLite.Workflow.Milestone milestoneById = ((MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).GetMilestoneByID(tag);
      RoleInfo roleFunction = ((WorkflowManager) Session.BPM.GetBpmManager(BpmCategory.Workflow)).GetRoleFunction(milestoneById.RoleID);
      WorksheetInfo key = new WorksheetInfo(milestoneById.MilestoneID, (RoleSummaryInfo) roleFunction, milestoneById.RoleRequired, textBox.Text, milestoneById.Archived, (InputFormInfo) null);
      this.dirty = true;
      if (this.alertMsgsToUpdate.ContainsKey(key))
        this.alertMsgsToUpdate[key] = textBox.Text;
      else
        this.alertMsgsToUpdate.Add(key, textBox.Text);
    }

    private void lsvMilestone_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      if (this.suspendEventHandler)
        return;
      this.dirty = true;
      if (this.workflowMgr == null)
        return;
      GVItem currItem = e.SubItem.Item;
      TextBox txtBox = (TextBox) currItem.SubItems[1].Value;
      this.setAlertMsgTextBoxProperty(((EllieMae.EMLite.Workflow.Milestone) currItem.Tag).MilestoneID, e.SubItem.Checked, txtBox, currItem);
    }

    private void setAlertMsgTextBoxProperty(
      string msGuid,
      bool msSelected,
      TextBox txtBox,
      GVItem currItem)
    {
      WorksheetInfo msWorksheetInfo = this.workflowMgr.GetMsWorksheetInfo(msGuid);
      EllieMae.EMLite.Workflow.Milestone milestoneById = ((MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).GetMilestoneByID(msGuid);
      string milestoneId = (string) null;
      List<EllieMae.EMLite.Workflow.Milestone> list = ((MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).GetAllActiveMilestonesList().ToList<EllieMae.EMLite.Workflow.Milestone>();
      foreach (EllieMae.EMLite.Workflow.Milestone milestone in list)
      {
        if (milestone.MilestoneID == msGuid)
        {
          if (milestone.Name != "Started")
          {
            milestoneId = list[list.IndexOf(milestone) - 1].MilestoneID;
            break;
          }
          break;
        }
      }
      bool flag = false;
      if (milestoneId != null)
      {
        ((MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).GetMilestoneByID(milestoneId);
        flag = milestoneById.RoleID != -1;
      }
      txtBox.Enabled = false;
      string str1 = msGuid;
      int num = 1;
      string str2 = num.ToString();
      if (str1 != str2 & msSelected & flag)
      {
        if (msWorksheetInfo != null)
          txtBox.Text = msWorksheetInfo.AlertMessage;
        if (txtBox.Text == "")
          txtBox.Text = milestoneById.DescTextAfter;
        txtBox.Enabled = true;
      }
      else
      {
        txtBox.Enabled = false;
        bool suspendEventHandler = this.suspendEventHandler;
        this.suspendEventHandler = true;
        string str3 = msGuid;
        num = 1;
        string str4 = num.ToString();
        if (str3 == str4)
        {
          if (msSelected)
          {
            if (msWorksheetInfo != null)
              txtBox.Text = msWorksheetInfo.AlertMessage;
            if (txtBox.Text == "")
              txtBox.Text = milestoneById.DescTextAfter;
            if ((txtBox.Text ?? "").Trim() == "")
              txtBox.Text = "Loan has been started.";
            txtBox.Enabled = true;
          }
          else
            txtBox.Text = "N/A (Alert Not Enabled)";
        }
        else if (!flag)
        {
          txtBox.Text = "N/A (No Role Change)";
          currItem.SubItems[0].Checked = false;
          currItem.SubItems[0].CheckBoxEnabled = false;
        }
        else if (!msSelected)
          txtBox.Text = "N/A (Alert Not Enabled)";
        this.suspendEventHandler = suspendEventHandler;
      }
    }

    public Dictionary<WorksheetInfo, string> AlertMsgsToUpdate
    {
      get => this.dirty ? this.alertMsgsToUpdate : (Dictionary<WorksheetInfo, string>) null;
    }

    public List<string> GetSelectedMilestones()
    {
      if (this.milestoneGuidList == null)
      {
        this.milestoneGuidList = new List<string>();
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.lsvMilestone.Items)
        {
          if (gvItem.SubItems[0].Checked)
            this.milestoneGuidList.Add(((EllieMae.EMLite.Workflow.Milestone) gvItem.Tag).MilestoneID);
        }
      }
      return this.milestoneGuidList;
    }

    public bool IsDirty => this.dirty;

    private void lsvMilestone_Click(object sender, EventArgs e) => this.suspendEventHandler = false;

    private void chbCheckAll_CheckedChanged(object sender, EventArgs e)
    {
      this.suspendEventHandler = false;
      foreach (GVItem currItem in (IEnumerable<GVItem>) this.lsvMilestone.Items)
      {
        currItem.SubItems[0].Checked = this.chbCheckAll.Checked;
        if (this.workflowMgr != null)
        {
          TextBox txtBox = (TextBox) currItem.SubItems[1].Value;
          this.setAlertMsgTextBoxProperty(((EllieMae.EMLite.Workflow.Milestone) currItem.Tag).MilestoneID, currItem.SubItems[0].Checked, txtBox, currItem);
        }
      }
    }

    private void btnCancel_Click(object sender, EventArgs e) => this.dirty = false;

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.workflowMgr != null)
      {
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.lsvMilestone.Items)
        {
          bool flag = gvItem.SubItems[0].Checked;
          TextBox textBox = (TextBox) gvItem.SubItems[1].Value;
          if (flag && textBox.Text.Trim() == "")
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "A message must be specified for all milestones that are configured to display an alert.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return;
          }
        }
      }
      this.DialogResult = DialogResult.OK;
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
      this.lsvMilestone = new GridView();
      this.lblSubTitle = new Label();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.chbCheckAll = new CheckBox();
      this.groupContainer1 = new GroupContainer();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.lsvMilestone.AllowMultiselect = false;
      this.lsvMilestone.BorderStyle = BorderStyle.None;
      gvColumn1.CheckBoxes = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "colMilestone";
      gvColumn1.SpringToFit = true;
      gvColumn1.Text = "      Milestone";
      gvColumn1.Width = 234;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "colAlertMsg";
      gvColumn2.Text = "Alert Message";
      gvColumn2.Width = 350;
      this.lsvMilestone.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.lsvMilestone.Dock = DockStyle.Fill;
      this.lsvMilestone.HotItemTracking = false;
      this.lsvMilestone.Location = new Point(1, 26);
      this.lsvMilestone.Name = "lsvMilestone";
      this.lsvMilestone.Size = new Size(584, 335);
      this.lsvMilestone.SortOption = GVSortOption.None;
      this.lsvMilestone.TabIndex = 2;
      this.lsvMilestone.SubItemCheck += new GVSubItemEventHandler(this.lsvMilestone_SubItemCheck);
      this.lsvMilestone.Click += new EventHandler(this.lsvMilestone_Click);
      this.lblSubTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblSubTitle.AutoSize = true;
      this.lblSubTitle.BackColor = Color.Transparent;
      this.lblSubTitle.Location = new Point(6, 6);
      this.lblSubTitle.Name = "lblSubTitle";
      this.lblSubTitle.Size = new Size(493, 14);
      this.lblSubTitle.TabIndex = 3;
      this.lblSubTitle.Text = "Select the milestones to enable the alert and configure the alert message for each enabled milestone.";
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(440, 381);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 22);
      this.btnOK.TabIndex = 4;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(523, 381);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.chbCheckAll.AutoSize = true;
      this.chbCheckAll.Location = new Point(6, 29);
      this.chbCheckAll.Name = "chbCheckAll";
      this.chbCheckAll.Size = new Size(15, 14);
      this.chbCheckAll.TabIndex = 6;
      this.chbCheckAll.UseVisualStyleBackColor = true;
      this.chbCheckAll.CheckedChanged += new EventHandler(this.chbCheckAll_CheckedChanged);
      this.groupContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer1.Controls.Add((Control) this.chbCheckAll);
      this.groupContainer1.Controls.Add((Control) this.lblSubTitle);
      this.groupContainer1.Controls.Add((Control) this.lsvMilestone);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(10, 10);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(586, 362);
      this.groupContainer1.TabIndex = 7;
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(607, 412);
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.MinimizeBox = false;
      this.Name = nameof (MilestoneSelectionDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.SizeGripStyle = SizeGripStyle.Hide;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = " Select Milestones and Configure Alert Messages";
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
