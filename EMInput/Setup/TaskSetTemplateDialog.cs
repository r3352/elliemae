// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.TaskSetTemplateDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class TaskSetTemplateDialog : Form, IHelp
  {
    private const string className = "TaskSetTemplateDialog";
    private static string sw = Tracing.SwOutsideLoan;
    private TaskSetTemplate taskTemplate;
    private MilestoneTaskDefinition[] predefinedTasks;
    private List<EllieMae.EMLite.Workflow.Milestone> msList;
    private string[] milestoneNames;
    private bool readOnly;
    private Sessions.Session session;
    private LogList list;
    private const string BADCHARS = "/:*?<>|.";
    private IContainer components;
    private Button removeBtn;
    private Button addBtn;
    private TextBox nameTxt;
    private Label label5;
    private Button cancelBtn;
    private Button saveBtn;
    private TextBox descTxt;
    private Label label4;
    private EMHelpLink emHelpLink1;
    private GroupContainer groupContainer1;
    private PanelEx panelEx2;
    private GridView gridPredefined;
    private PanelEx panelEx1;
    private Label label6;
    private GroupContainer groupContainer2;
    private GridView gridSelected;
    private ComboBoxEx cmbBoxMilestones;

    public TaskSetTemplateDialog(
      TaskSetTemplate taskTemplate,
      bool readOnly,
      Sessions.Session session)
      : this(taskTemplate, session)
    {
      this.readOnly = readOnly;
      if (!this.readOnly)
        return;
      this.nameTxt.ReadOnly = true;
      this.descTxt.ReadOnly = true;
      this.addBtn.Enabled = false;
      this.removeBtn.Enabled = false;
      this.saveBtn.Enabled = false;
      this.cancelBtn.Text = "Close";
      this.AcceptButton = (IButtonControl) this.cancelBtn;
      if (session.LoanData == null)
        return;
      this.list = session.LoanData.GetLogList();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gridSelected.Items)
      {
        if (!(gvItem.SubItems[1].Text == ""))
        {
          EllieMae.EMLite.Workflow.Milestone milestoneByName = ((MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).GetMilestoneByName(gvItem.SubItems[1].Text);
          if (milestoneByName != null && this.list.GetMilestoneByID(milestoneByName.MilestoneID) == null)
            gvItem.ForeColor = Color.Gray;
        }
      }
    }

    public TaskSetTemplateDialog(TaskSetTemplate taskTemplate, Sessions.Session session)
    {
      this.taskTemplate = taskTemplate;
      this.session = session;
      this.msList = this.session.StartupInfo.Milestones;
      this.InitializeComponent();
      this.emHelpLink1.AssignSession(this.session);
      this.predefinedTasks = this.session.ConfigurationManager.GetMilestoneTasks((string[]) null);
      this.milestoneNames = this.getAllMilestoneNames();
      this.cmbBoxMilestones.Items.AddRange((object[]) this.milestoneNames);
      this.showTrackedTaskList();
      this.cmbBoxMilestones.SelectedIndexChanged += new EventHandler(this.stageCombo_SelectedIndexChanged);
      this.cmbBoxMilestones.SelectedItem = (object) this.milestoneNames[1];
      this.nameTxt.Text = this.taskTemplate.TemplateName;
      this.descTxt.Text = this.taskTemplate.Description;
      this.gridPredefined_SelectedIndexChanged((object) null, (EventArgs) null);
      this.gridSelected_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    private string[] getAllMilestoneNames()
    {
      ArrayList arrayList = new ArrayList();
      arrayList.Add((object) string.Empty);
      for (int index = 0; index < this.msList.Count; ++index)
      {
        if (!this.msList[index].Archived && !(this.msList[index].Name == "Started"))
          arrayList.Add((object) this.msList[index].Name);
      }
      return (string[]) arrayList.ToArray(typeof (string));
    }

    private void addBtn_Click(object sender, EventArgs e)
    {
      ArrayList tasksByMilestone = this.taskTemplate.GetTasksByMilestone((string) this.cmbBoxMilestones.SelectedItem);
      ArrayList arrayList = new ArrayList();
      foreach (GVItem selectedItem in this.gridPredefined.SelectedItems)
        arrayList.Add((object) selectedItem);
      foreach (GVItem gvItem in arrayList)
      {
        this.gridPredefined.Items.Remove(gvItem);
        tasksByMilestone.Add((object) gvItem.Text);
      }
      this.showTrackedTaskList();
      this.refreshSaveButton();
    }

    private void removeBtn_Click(object sender, EventArgs e)
    {
      ArrayList arrayList = new ArrayList();
      foreach (object selectedItem in this.gridSelected.SelectedItems)
        arrayList.Add(selectedItem);
      foreach (GVItem gvItem in arrayList)
      {
        string text = gvItem.SubItems[0].Text;
        this.taskTemplate.GetTasksByMilestone(gvItem.SubItems[1].Text).Remove((object) text);
        this.gridSelected.Items.Remove(gvItem);
      }
      this.stageCombo_SelectedIndexChanged((object) null, (EventArgs) null);
      this.refreshSaveButton();
    }

    private void refreshSaveButton()
    {
      this.saveBtn.Enabled = !this.readOnly && this.gridSelected.Items.Count > 0;
    }

    private void stageCombo_SelectedIndexChanged(object sender, EventArgs e)
    {
      ArrayList tasksByMilestone = this.taskTemplate.GetTasksByMilestone((string) this.cmbBoxMilestones.SelectedItem);
      this.gridPredefined.Items.Clear();
      if (this.predefinedTasks == null || this.predefinedTasks.Length == 0)
        return;
      this.gridPredefined.BeginUpdate();
      for (int index = 0; index < this.predefinedTasks.Length; ++index)
      {
        if (!tasksByMilestone.Contains((object) this.predefinedTasks[index].TaskName))
          this.gridPredefined.Items.Add(new GVItem(this.predefinedTasks[index].TaskName));
      }
      this.gridPredefined.EndUpdate();
    }

    private void saveBtn_Click(object sender, EventArgs e)
    {
      this.taskTemplate.Description = this.descTxt.Text.Trim();
      this.DialogResult = DialogResult.OK;
    }

    private void showTrackedTaskList()
    {
      this.gridSelected.Items.Clear();
      foreach (string str in this.taskTemplate.GetTasksByMilestone(string.Empty))
        this.gridSelected.Items.Add(new GVItem(new string[2]
        {
          str,
          string.Empty
        }));
      for (int index = 1; index < this.msList.Count; ++index)
      {
        string name = this.msList[index].Name;
        if (!(name == "Completion"))
        {
          foreach (string text in this.taskTemplate.GetTasksByMilestone(name))
            this.gridSelected.Items.Add(new GVItem(text)
            {
              SubItems = {
                (object) name
              }
            });
        }
      }
      foreach (string text in this.taskTemplate.GetTasksByMilestone("Completion"))
        this.gridSelected.Items.Add(new GVItem(text)
        {
          SubItems = {
            (object) "Completion"
          }
        });
    }

    private void keypress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if ("/:*?<>|.".IndexOf(e.KeyChar) == -1)
      {
        char keyChar = e.KeyChar;
        if (!keyChar.Equals('\\'))
        {
          keyChar = e.KeyChar;
          if (!keyChar.Equals('"'))
          {
            e.Handled = false;
            return;
          }
        }
        e.Handled = true;
      }
      else
        e.Handled = true;
    }

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, "Task Sets");
    }

    private void gridPredefined_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.addBtn.Enabled = !this.readOnly && this.gridPredefined.SelectedItems.Count > 0;
    }

    private void gridSelected_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.removeBtn.Enabled = !this.readOnly && this.gridSelected.SelectedItems.Count > 0;
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
      this.removeBtn = new Button();
      this.addBtn = new Button();
      this.nameTxt = new TextBox();
      this.label5 = new Label();
      this.cancelBtn = new Button();
      this.saveBtn = new Button();
      this.descTxt = new TextBox();
      this.label4 = new Label();
      this.emHelpLink1 = new EMHelpLink();
      this.groupContainer2 = new GroupContainer();
      this.gridSelected = new GridView();
      this.groupContainer1 = new GroupContainer();
      this.panelEx2 = new PanelEx();
      this.gridPredefined = new GridView();
      this.panelEx1 = new PanelEx();
      this.label6 = new Label();
      this.cmbBoxMilestones = new ComboBoxEx();
      this.groupContainer2.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.panelEx2.SuspendLayout();
      this.panelEx1.SuspendLayout();
      this.SuspendLayout();
      this.removeBtn.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.removeBtn.Location = new Point(260, 282);
      this.removeBtn.Name = "removeBtn";
      this.removeBtn.Size = new Size(72, 23);
      this.removeBtn.TabIndex = 5;
      this.removeBtn.Text = "< Remove";
      this.removeBtn.Click += new EventHandler(this.removeBtn_Click);
      this.addBtn.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.addBtn.Location = new Point(260, 253);
      this.addBtn.Name = "addBtn";
      this.addBtn.Size = new Size(72, 23);
      this.addBtn.TabIndex = 4;
      this.addBtn.Text = "Add >";
      this.addBtn.Click += new EventHandler(this.addBtn_Click);
      this.nameTxt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.nameTxt.Location = new Point(94, 11);
      this.nameTxt.MaxLength = 256;
      this.nameTxt.Name = "nameTxt";
      this.nameTxt.ReadOnly = true;
      this.nameTxt.Size = new Size(489, 20);
      this.nameTxt.TabIndex = 27;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(6, 14);
      this.label5.Name = "label5";
      this.label5.Size = new Size(82, 13);
      this.label5.TabIndex = 31;
      this.label5.Text = "Template Name";
      this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(505, 511);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 29;
      this.cancelBtn.Text = "&Cancel";
      this.saveBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.saveBtn.Location = new Point(425, 511);
      this.saveBtn.Name = "saveBtn";
      this.saveBtn.Size = new Size(75, 24);
      this.saveBtn.TabIndex = 28;
      this.saveBtn.Text = "&Save";
      this.saveBtn.Click += new EventHandler(this.saveBtn_Click);
      this.descTxt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.descTxt.Location = new Point(94, 37);
      this.descTxt.Multiline = true;
      this.descTxt.Name = "descTxt";
      this.descTxt.ScrollBars = ScrollBars.Both;
      this.descTxt.Size = new Size(489, 66);
      this.descTxt.TabIndex = 26;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(6, 37);
      this.label4.Name = "label4";
      this.label4.Size = new Size(60, 13);
      this.label4.TabIndex = 32;
      this.label4.Text = "Description";
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.HelpTag = "Task Sets";
      this.emHelpLink1.Location = new Point(13, 515);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 33;
      this.groupContainer2.Controls.Add((Control) this.gridSelected);
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(337, 113);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(246, 392);
      this.groupContainer2.TabIndex = 35;
      this.groupContainer2.Text = "Tracked Tasks";
      this.gridSelected.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Task";
      gvColumn1.Width = 150;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Milestone";
      gvColumn2.Width = 100;
      this.gridSelected.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gridSelected.Dock = DockStyle.Fill;
      this.gridSelected.Location = new Point(1, 26);
      this.gridSelected.Name = "gridSelected";
      this.gridSelected.Size = new Size(244, 365);
      this.gridSelected.TabIndex = 1;
      this.gridSelected.SelectedIndexChanged += new EventHandler(this.gridSelected_SelectedIndexChanged);
      this.groupContainer1.Controls.Add((Control) this.panelEx2);
      this.groupContainer1.Controls.Add((Control) this.panelEx1);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(9, 113);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(246, 391);
      this.groupContainer1.TabIndex = 34;
      this.groupContainer1.Text = "Predefined Tasks";
      this.panelEx2.Controls.Add((Control) this.gridPredefined);
      this.panelEx2.Dock = DockStyle.Fill;
      this.panelEx2.Location = new Point(1, 54);
      this.panelEx2.Name = "panelEx2";
      this.panelEx2.Size = new Size(244, 336);
      this.panelEx2.TabIndex = 1;
      this.gridPredefined.BorderStyle = BorderStyle.None;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column1";
      gvColumn3.SpringToFit = true;
      gvColumn3.Text = "Name";
      gvColumn3.Width = 244;
      this.gridPredefined.Columns.AddRange(new GVColumn[1]
      {
        gvColumn3
      });
      this.gridPredefined.Dock = DockStyle.Fill;
      this.gridPredefined.Location = new Point(0, 0);
      this.gridPredefined.Name = "gridPredefined";
      this.gridPredefined.Size = new Size(244, 336);
      this.gridPredefined.TabIndex = 0;
      this.gridPredefined.SelectedIndexChanged += new EventHandler(this.gridPredefined_SelectedIndexChanged);
      this.panelEx1.Controls.Add((Control) this.cmbBoxMilestones);
      this.panelEx1.Controls.Add((Control) this.label6);
      this.panelEx1.Dock = DockStyle.Top;
      this.panelEx1.Location = new Point(1, 26);
      this.panelEx1.Name = "panelEx1";
      this.panelEx1.Size = new Size(244, 28);
      this.panelEx1.TabIndex = 0;
      this.label6.AutoSize = true;
      this.label6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label6.Location = new Point(3, 8);
      this.label6.Name = "label6";
      this.label6.Size = new Size(52, 13);
      this.label6.TabIndex = 18;
      this.label6.Text = "Milestone";
      this.label6.TextAlign = ContentAlignment.MiddleLeft;
      this.cmbBoxMilestones.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxMilestones.Location = new Point(57, 4);
      this.cmbBoxMilestones.Name = "cmbBoxMilestones";
      this.cmbBoxMilestones.SelectedBGColor = SystemColors.Highlight;
      this.cmbBoxMilestones.Size = new Size(184, 21);
      this.cmbBoxMilestones.TabIndex = 36;
      this.AcceptButton = (IButtonControl) this.saveBtn;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(594, 547);
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.groupContainer2);
      this.Controls.Add((Control) this.removeBtn);
      this.Controls.Add((Control) this.addBtn);
      this.Controls.Add((Control) this.nameTxt);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.saveBtn);
      this.Controls.Add((Control) this.descTxt);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.emHelpLink1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (TaskSetTemplateDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Task Set Template Details";
      this.groupContainer2.ResumeLayout(false);
      this.groupContainer1.ResumeLayout(false);
      this.panelEx2.ResumeLayout(false);
      this.panelEx1.ResumeLayout(false);
      this.panelEx1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
