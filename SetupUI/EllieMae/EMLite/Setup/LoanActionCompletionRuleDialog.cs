// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LoanActionCompletionRuleDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class LoanActionCompletionRuleDialog : Form
  {
    private const string className = "LoanActionCompletionRuleDialog";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private Sessions.Session session;
    private Button cancelBtn;
    private Button removeDocBtn;
    private Button addDocBtn;
    private Button removeFieldBtn;
    private Button addFieldBtn;
    private System.ComponentModel.Container components;
    private Button okBtn;
    private TextBox textBoxName;
    private RuleConditionControl ruleCondForm;
    private DocLoanActionPair[] docsForRule;
    private FieldLoanActionPair[] fieldsForRule;
    private TaskLoanActionPair[] tasksForRule;
    private MilestoneLoanActionPair[] milestonesForRule;
    private AdvancedCodeLoanActionPair[] advCodesForRule;
    private Dictionary<string, string> milestoneList;
    private const string ARCHIVED = " (Archived)";
    private Label label1;
    private Label label2;
    private Label label3;
    private Panel panelCondition;
    private Button findBtn;
    private LoanActionCompletionRulePanel container;
    private string[] loanActionList;
    private List<string> filteredLoanActionList;
    private EMHelpLink emHelpLink1;
    private Label label6;
    private Panel panelChannel;
    private FieldSettings fieldSettings;
    private Button btnRemoveTask;
    private Button btnAddTask;
    private ChannelConditionControl channelControl;
    private string[] REQOPTIONS = new string[2]
    {
      MilestoneTaskLog.TASKREQUIRED,
      MilestoneTaskLog.TASKOPTIONAL
    };
    private Button removeCodeBtn;
    private Button addCodeBtn;
    private Button editCodeBtn;
    private TabControlEx tabControl1;
    private TabPageEx tabPageEx1;
    private TabPageEx tabPageEx2;
    private TabPageEx tabPageEx3;
    private TabPageEx tabPageEx4;
    private GridView emListViewCode;
    private GridView emListViewFields;
    private GridView emListViewTasks;
    private TabPageEx tabPageEx5;
    private GridView emListViewDocs;
    private GridView emListViewMilestones;
    private Button addMilestoneBtn;
    private Button removeMilestoneBtn;
    private const string FOR_LOAN_ACTION_LABEL = "For Loan Action";
    private Panel panel1;
    private List<string> selectedTPORuleConditionValue;
    private TextBox commentsTxt;
    private Label label4;
    private bool hasNoAccess;
    private List<string> mustHaveLoanActionList;
    private LoanActionCompletionRuleInfo loanActionCompletionRule;

    public LoanActionCompletionRuleDialog(
      Sessions.Session session,
      LoanActionCompletionRuleInfo loanActionCompletionRule,
      LoanActionCompletionRulePanel container)
    {
      this.session = session;
      this.container = container;
      this.loanActionCompletionRule = loanActionCompletionRule;
      if (loanActionCompletionRule != null)
      {
        this.docsForRule = loanActionCompletionRule.Docs;
        this.fieldsForRule = loanActionCompletionRule.Fields;
        this.tasksForRule = loanActionCompletionRule.Tasks;
        this.milestonesForRule = loanActionCompletionRule.Milestones;
        this.advCodesForRule = loanActionCompletionRule.AdvancedCodes;
      }
      this.InitializeComponent();
      this.emHelpLink1.AssignSession(this.session);
      this.fieldSettings = this.session.LoanManager.GetFieldSettings();
      this.channelControl = new ChannelConditionControl();
      if (this.loanActionCompletionRule != null)
        this.channelControl.ChannelValue = this.loanActionCompletionRule.Condition2;
      this.panelChannel.Controls.Add((Control) this.channelControl);
      this.ruleCondForm = new RuleConditionControl(this.session);
      this.ruleCondForm.InitControl(BpmCategory.LoanActionCompletionRules);
      this.panelCondition.Controls.Add((Control) this.ruleCondForm);
      this.ruleCondForm.ChangesMadeToConditions += new EventHandler(this.ruleCondForm_ChangesMadeToConditions);
      this.initForm();
      this.emListViewFields.SelectedIndexChanged += new EventHandler(this.emListViewFields_SelectedIndexChanged);
      this.emListViewDocs.SelectedIndexChanged += new EventHandler(this.emListViewDocs_SelectedIndexChanged);
      this.emListViewTasks.SelectedIndexChanged += new EventHandler(this.emListViewTasks_SelectedIndexChanged);
      this.emListViewMilestones.SelectedIndexChanged += new EventHandler(this.emListViewMilestones_SelectedIndexChanged);
      this.emListViewTasks_SelectedIndexChanged((object) null, (EventArgs) null);
      this.emListViewDocs_SelectedIndexChanged((object) null, (EventArgs) null);
      this.emListViewFields_SelectedIndexChanged((object) null, (EventArgs) null);
      this.emListViewCode_SelectedIndexChanged((object) null, (EventArgs) null);
      this.emListViewMilestones_SelectedIndexChanged((object) null, (EventArgs) null);
      this.hasNoAccess = !((FeaturesAclManager) session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.SettingsTab_LoanActionCompletion);
      this.addCodeBtn.Enabled = this.editCodeBtn.Enabled = this.removeCodeBtn.Enabled = this.addMilestoneBtn.Enabled = this.removeMilestoneBtn.Enabled = this.addCodeBtn.Enabled = this.editCodeBtn.Enabled = this.removeCodeBtn.Enabled = this.addDocBtn.Enabled = this.removeDocBtn.Enabled = this.btnAddTask.Enabled = this.btnRemoveTask.Enabled = this.addFieldBtn.Enabled = this.removeFieldBtn.Enabled = this.findBtn.Enabled = this.okBtn.Enabled = !this.hasNoAccess;
      if (!this.hasNoAccess)
        return;
      this.textBoxName.Enabled = false;
      this.ruleCondForm.DisableControls();
      this.channelControl.DisableControls();
    }

    private void ruleCondForm_ChangesMadeToConditions(object sender, EventArgs e)
    {
      this.mustHaveLoanActionList = this.getMustHaveLoanActionsList();
      this.selectedTPORuleConditionValue = this.ruleCondForm.SelectedTPORuleCondition;
      this.filterLoanActions();
    }

    private void emListViewTasks_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.hasNoAccess)
        return;
      this.btnRemoveTask.Enabled = this.emListViewTasks.SelectedItems.Count > 0;
    }

    private void emListViewDocs_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.hasNoAccess)
        return;
      this.removeDocBtn.Enabled = this.emListViewDocs.SelectedItems.Count > 0;
    }

    private void emListViewFields_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.hasNoAccess)
        return;
      this.removeFieldBtn.Enabled = this.emListViewFields.SelectedItems.Count > 0;
    }

    private void emListViewMilestones_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.hasNoAccess)
        return;
      this.removeMilestoneBtn.Enabled = this.emListViewMilestones.SelectedItems.Count > 0;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public LoanActionCompletionRuleInfo LoanActionCompletionRule => this.loanActionCompletionRule;

    private void InitializeComponent()
    {
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      GVColumn gvColumn8 = new GVColumn();
      GVColumn gvColumn9 = new GVColumn();
      GVColumn gvColumn10 = new GVColumn();
      GVColumn gvColumn11 = new GVColumn();
      GVColumn gvColumn12 = new GVColumn();
      this.cancelBtn = new Button();
      this.textBoxName = new TextBox();
      this.okBtn = new Button();
      this.label1 = new Label();
      this.label2 = new Label();
      this.panelCondition = new Panel();
      this.label3 = new Label();
      this.label6 = new Label();
      this.panelChannel = new Panel();
      this.tabControl1 = new TabControlEx();
      this.tabPageEx1 = new TabPageEx();
      this.emListViewDocs = new GridView();
      this.addDocBtn = new Button();
      this.removeDocBtn = new Button();
      this.tabPageEx2 = new TabPageEx();
      this.emListViewTasks = new GridView();
      this.btnRemoveTask = new Button();
      this.btnAddTask = new Button();
      this.tabPageEx3 = new TabPageEx();
      this.emListViewFields = new GridView();
      this.findBtn = new Button();
      this.addFieldBtn = new Button();
      this.removeFieldBtn = new Button();
      this.tabPageEx5 = new TabPageEx();
      this.emListViewMilestones = new GridView();
      this.addMilestoneBtn = new Button();
      this.removeMilestoneBtn = new Button();
      this.tabPageEx4 = new TabPageEx();
      this.emListViewCode = new GridView();
      this.editCodeBtn = new Button();
      this.addCodeBtn = new Button();
      this.removeCodeBtn = new Button();
      this.emHelpLink1 = new EMHelpLink();
      this.panel1 = new Panel();
      this.commentsTxt = new TextBox();
      this.label4 = new Label();
      this.tabControl1.SuspendLayout();
      this.tabPageEx1.SuspendLayout();
      this.tabPageEx2.SuspendLayout();
      this.tabPageEx3.SuspendLayout();
      this.tabPageEx5.SuspendLayout();
      this.tabPageEx4.SuspendLayout();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(978, 529);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 23);
      this.cancelBtn.TabIndex = 14;
      this.cancelBtn.Text = "Cancel";
      this.textBoxName.Location = new Point(32, 30);
      this.textBoxName.MaxLength = 64;
      this.textBoxName.Name = "textBoxName";
      this.textBoxName.Size = new Size(653, 20);
      this.textBoxName.TabIndex = 1;
      this.okBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.okBtn.Location = new Point(898, 529);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 23);
      this.okBtn.TabIndex = 13;
      this.okBtn.Text = "&Save";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(16, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(164, 16);
      this.label1.TabIndex = 0;
      this.label1.Text = "1. Create a Rule Name";
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(16, 170);
      this.label2.Name = "label2";
      this.label2.Size = new Size(264, 16);
      this.label2.TabIndex = 4;
      this.label2.Text = "3. Is there a condition for this rule";
      this.panelCondition.Location = new Point(34, 189);
      this.panelCondition.Name = "panelCondition";
      this.panelCondition.Size = new Size(653, 94);
      this.panelCondition.TabIndex = 5;
      this.label3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(16, 298);
      this.label3.Name = "label3";
      this.label3.Size = new Size(551, 16);
      this.label3.TabIndex = 6;
      this.label3.Text = "4. Select required documents, tasks, fields, and milestones for loan action completion.";
      this.label6.AutoSize = true;
      this.label6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label6.Location = new Point(16, 53);
      this.label6.Name = "label6";
      this.label6.Size = new Size(239, 13);
      this.label6.TabIndex = 2;
      this.label6.Text = "2. Select all Channels this rule applies to";
      this.panelChannel.Location = new Point(34, 79);
      this.panelChannel.Name = "panelChannel";
      this.panelChannel.Size = new Size(653, 79);
      this.panelChannel.TabIndex = 3;
      this.tabControl1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.tabControl1.Location = new Point(35, 317);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.Size = new Size(652, 218);
      this.tabControl1.TabHeight = 20;
      this.tabControl1.TabIndex = 7;
      this.tabControl1.TabPages.Add(this.tabPageEx1);
      this.tabControl1.TabPages.Add(this.tabPageEx2);
      this.tabControl1.TabPages.Add(this.tabPageEx3);
      this.tabControl1.TabPages.Add(this.tabPageEx5);
      this.tabControl1.TabPages.Add(this.tabPageEx4);
      this.tabControl1.Text = "tabControlEx1";
      this.tabPageEx1.BackColor = Color.Transparent;
      this.tabPageEx1.Controls.Add((Control) this.emListViewDocs);
      this.tabPageEx1.Controls.Add((Control) this.addDocBtn);
      this.tabPageEx1.Controls.Add((Control) this.removeDocBtn);
      this.tabPageEx1.Location = new Point(1, 23);
      this.tabPageEx1.Name = "tabPageEx1";
      this.tabPageEx1.TabIndex = 0;
      this.tabPageEx1.TabWidth = 100;
      this.tabPageEx1.Text = "Required Docs";
      this.tabPageEx1.Value = (object) "Required Docs";
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Name";
      gvColumn1.Width = 260;
      gvColumn2.ActivatedEditorType = GVActivatedEditorType.ComboBox;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "For Loan Action";
      gvColumn2.Width = 155;
      gvColumn3.ActivatedEditorType = GVActivatedEditorType.ComboBox;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.SpringToFit = true;
      gvColumn3.Text = "Attachment Req.";
      gvColumn3.Width = 119;
      this.emListViewDocs.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.emListViewDocs.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.emListViewDocs.Location = new Point(18, 16);
      this.emListViewDocs.Name = "emListViewDocs";
      this.emListViewDocs.Size = new Size(536, 166);
      this.emListViewDocs.TabIndex = 29;
      this.emListViewDocs.EditorOpening += new GVSubItemEditingEventHandler(this.emListViewDocs_EditorOpening);
      this.emListViewDocs.EditorClosing += new GVSubItemEditingEventHandler(this.gridView_EditorClosing);
      this.addDocBtn.Location = new Point(564, 16);
      this.addDocBtn.Name = "addDocBtn";
      this.addDocBtn.Size = new Size(75, 23);
      this.addDocBtn.TabIndex = 8;
      this.addDocBtn.Text = "&Add";
      this.addDocBtn.Click += new EventHandler(this.addDocBtn_Click);
      this.removeDocBtn.Location = new Point(564, 45);
      this.removeDocBtn.Name = "removeDocBtn";
      this.removeDocBtn.Size = new Size(75, 23);
      this.removeDocBtn.TabIndex = 9;
      this.removeDocBtn.Text = "&Remove";
      this.removeDocBtn.Click += new EventHandler(this.removeBtn_Click);
      this.tabPageEx2.BackColor = Color.Transparent;
      this.tabPageEx2.Controls.Add((Control) this.emListViewTasks);
      this.tabPageEx2.Controls.Add((Control) this.btnRemoveTask);
      this.tabPageEx2.Controls.Add((Control) this.btnAddTask);
      this.tabPageEx2.Location = new Point(1, 23);
      this.tabPageEx2.Name = "tabPageEx2";
      this.tabPageEx2.TabIndex = 0;
      this.tabPageEx2.TabWidth = 100;
      this.tabPageEx2.Text = "Required Tasks";
      this.tabPageEx2.Value = (object) "Required Tasks";
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column1";
      gvColumn4.Text = "Name";
      gvColumn4.Width = 355;
      gvColumn5.ActivatedEditorType = GVActivatedEditorType.ComboBox;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column2";
      gvColumn5.SpringToFit = true;
      gvColumn5.Text = "For Loan Action";
      gvColumn5.Width = 183;
      this.emListViewTasks.Columns.AddRange(new GVColumn[2]
      {
        gvColumn4,
        gvColumn5
      });
      this.emListViewTasks.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.emListViewTasks.Location = new Point(18, 16);
      this.emListViewTasks.Name = "emListViewTasks";
      this.emListViewTasks.Size = new Size(540, 166);
      this.emListViewTasks.TabIndex = 29;
      this.emListViewTasks.EditorOpening += new GVSubItemEditingEventHandler(this.emListViewTasks_EditorOpening);
      this.emListViewTasks.EditorClosing += new GVSubItemEditingEventHandler(this.gridView_EditorClosing);
      this.btnRemoveTask.Location = new Point(564, 45);
      this.btnRemoveTask.Name = "btnRemoveTask";
      this.btnRemoveTask.Size = new Size(75, 23);
      this.btnRemoveTask.TabIndex = 23;
      this.btnRemoveTask.Text = "&Remove";
      this.btnRemoveTask.Click += new EventHandler(this.removeBtn_Click);
      this.btnAddTask.Location = new Point(564, 16);
      this.btnAddTask.Name = "btnAddTask";
      this.btnAddTask.Size = new Size(75, 23);
      this.btnAddTask.TabIndex = 22;
      this.btnAddTask.Text = "&Add";
      this.btnAddTask.Click += new EventHandler(this.btnAddTask_Click);
      this.tabPageEx3.BackColor = Color.Transparent;
      this.tabPageEx3.Controls.Add((Control) this.emListViewFields);
      this.tabPageEx3.Controls.Add((Control) this.findBtn);
      this.tabPageEx3.Controls.Add((Control) this.addFieldBtn);
      this.tabPageEx3.Controls.Add((Control) this.removeFieldBtn);
      this.tabPageEx3.Location = new Point(1, 23);
      this.tabPageEx3.Name = "tabPageEx3";
      this.tabPageEx3.TabIndex = 0;
      this.tabPageEx3.TabWidth = 100;
      this.tabPageEx3.Text = "Required Fields";
      this.tabPageEx3.Value = (object) "Required Fields";
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column1";
      gvColumn6.Text = "ID";
      gvColumn6.Width = 114;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column2";
      gvColumn7.Text = "Description";
      gvColumn7.Width = 244;
      gvColumn8.ActivatedEditorType = GVActivatedEditorType.ComboBox;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Column3";
      gvColumn8.Text = "For Loan Action";
      gvColumn8.Width = 155;
      this.emListViewFields.Columns.AddRange(new GVColumn[3]
      {
        gvColumn6,
        gvColumn7,
        gvColumn8
      });
      this.emListViewFields.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.emListViewFields.Location = new Point(18, 16);
      this.emListViewFields.Name = "emListViewFields";
      this.emListViewFields.Size = new Size(540, 166);
      this.emListViewFields.TabIndex = 29;
      this.emListViewFields.EditorOpening += new GVSubItemEditingEventHandler(this.emListViewFields_EditorOpening);
      this.emListViewFields.EditorClosing += new GVSubItemEditingEventHandler(this.gridView_EditorClosing);
      this.findBtn.Location = new Point(564, 42);
      this.findBtn.Name = "findBtn";
      this.findBtn.Size = new Size(75, 23);
      this.findBtn.TabIndex = 22;
      this.findBtn.Text = "&Find";
      this.findBtn.Click += new EventHandler(this.findBtn_Click);
      this.addFieldBtn.Location = new Point(564, 16);
      this.addFieldBtn.Name = "addFieldBtn";
      this.addFieldBtn.Size = new Size(75, 23);
      this.addFieldBtn.TabIndex = 11;
      this.addFieldBtn.Text = "&Add";
      this.addFieldBtn.Click += new EventHandler(this.addFieldBtn_Click);
      this.removeFieldBtn.Location = new Point(564, 68);
      this.removeFieldBtn.Name = "removeFieldBtn";
      this.removeFieldBtn.Size = new Size(75, 23);
      this.removeFieldBtn.TabIndex = 12;
      this.removeFieldBtn.Text = "&Remove";
      this.removeFieldBtn.Click += new EventHandler(this.removeBtn_Click);
      this.tabPageEx5.BackColor = Color.Transparent;
      this.tabPageEx5.Controls.Add((Control) this.emListViewMilestones);
      this.tabPageEx5.Controls.Add((Control) this.addMilestoneBtn);
      this.tabPageEx5.Controls.Add((Control) this.removeMilestoneBtn);
      this.tabPageEx5.Location = new Point(0, 0);
      this.tabPageEx5.Name = "tabPageEx5";
      this.tabPageEx5.TabIndex = 0;
      this.tabPageEx5.TabWidth = 128;
      this.tabPageEx5.Text = "Required Milestones";
      this.tabPageEx5.Value = (object) "Required Milestones";
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "Column1";
      gvColumn9.Text = "Milestone Name";
      gvColumn9.Width = 260;
      gvColumn10.ActivatedEditorType = GVActivatedEditorType.ComboBox;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "Column2";
      gvColumn10.Text = "For Loan Action";
      gvColumn10.Width = 155;
      this.emListViewMilestones.Columns.AddRange(new GVColumn[2]
      {
        gvColumn9,
        gvColumn10
      });
      this.emListViewMilestones.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.emListViewMilestones.Location = new Point(18, 16);
      this.emListViewMilestones.Name = "emListViewMilestones";
      this.emListViewMilestones.Size = new Size(536, 166);
      this.emListViewMilestones.TabIndex = 29;
      this.emListViewMilestones.EditorOpening += new GVSubItemEditingEventHandler(this.emListViewMilestones_EditorOpening);
      this.emListViewMilestones.EditorClosing += new GVSubItemEditingEventHandler(this.gridView_EditorClosing);
      this.addMilestoneBtn.Location = new Point(564, 16);
      this.addMilestoneBtn.Name = "addMilestoneBtn";
      this.addMilestoneBtn.Size = new Size(75, 23);
      this.addMilestoneBtn.TabIndex = 5;
      this.addMilestoneBtn.Text = "&Add";
      this.addMilestoneBtn.Click += new EventHandler(this.addMilestoneBtn_Click);
      this.removeMilestoneBtn.Location = new Point(564, 45);
      this.removeMilestoneBtn.Name = "removeMilestoneBtn";
      this.removeMilestoneBtn.Size = new Size(75, 23);
      this.removeMilestoneBtn.TabIndex = 6;
      this.removeMilestoneBtn.Text = "&Remove";
      this.removeMilestoneBtn.Click += new EventHandler(this.removeBtn_Click);
      this.tabPageEx4.BackColor = Color.Transparent;
      this.tabPageEx4.Controls.Add((Control) this.emListViewCode);
      this.tabPageEx4.Controls.Add((Control) this.editCodeBtn);
      this.tabPageEx4.Controls.Add((Control) this.addCodeBtn);
      this.tabPageEx4.Controls.Add((Control) this.removeCodeBtn);
      this.tabPageEx4.Location = new Point(1, 23);
      this.tabPageEx4.Name = "tabPageEx4";
      this.tabPageEx4.TabIndex = 0;
      this.tabPageEx4.TabWidth = 128;
      this.tabPageEx4.Text = "Advanced Conditions";
      this.tabPageEx4.Value = (object) "Advanced Conditions";
      gvColumn11.ImageIndex = -1;
      gvColumn11.Name = "Column1";
      gvColumn11.Text = "Code";
      gvColumn11.Width = 355;
      gvColumn12.ActivatedEditorType = GVActivatedEditorType.ComboBox;
      gvColumn12.ImageIndex = -1;
      gvColumn12.Name = "Column2";
      gvColumn12.Text = "For Loan Action";
      gvColumn12.Width = 155;
      this.emListViewCode.Columns.AddRange(new GVColumn[2]
      {
        gvColumn11,
        gvColumn12
      });
      this.emListViewCode.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.emListViewCode.Location = new Point(18, 16);
      this.emListViewCode.Name = "emListViewCode";
      this.emListViewCode.Size = new Size(540, 166);
      this.emListViewCode.TabIndex = 28;
      this.emListViewCode.SelectedIndexChanged += new EventHandler(this.emListViewCode_SelectedIndexChanged);
      this.emListViewCode.EditorOpening += new GVSubItemEditingEventHandler(this.emListViewCode_EditorOpening);
      this.emListViewCode.EditorClosing += new GVSubItemEditingEventHandler(this.gridView_EditorClosing);
      this.emListViewCode.DoubleClick += new EventHandler(this.emListViewCode_DoubleClick);
      this.editCodeBtn.Location = new Point(564, 45);
      this.editCodeBtn.Name = "editCodeBtn";
      this.editCodeBtn.Size = new Size(75, 23);
      this.editCodeBtn.TabIndex = 27;
      this.editCodeBtn.Text = "&Edit";
      this.editCodeBtn.Click += new EventHandler(this.editCodeBtn_Click);
      this.addCodeBtn.Location = new Point(564, 16);
      this.addCodeBtn.Name = "addCodeBtn";
      this.addCodeBtn.Size = new Size(75, 23);
      this.addCodeBtn.TabIndex = 24;
      this.addCodeBtn.Text = "&Add";
      this.addCodeBtn.Click += new EventHandler(this.addCodeBtn_Click);
      this.removeCodeBtn.Location = new Point(564, 74);
      this.removeCodeBtn.Name = "removeCodeBtn";
      this.removeCodeBtn.Size = new Size(75, 23);
      this.removeCodeBtn.TabIndex = 25;
      this.removeCodeBtn.Text = "&Remove";
      this.removeCodeBtn.Click += new EventHandler(this.removeBtn_Click);
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "Setup\\Loan Action Completion";
      this.emHelpLink1.Location = new Point(10, 538);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 12;
      this.panel1.Controls.Add((Control) this.commentsTxt);
      this.panel1.Controls.Add((Control) this.label4);
      this.panel1.Controls.Add((Control) this.panelCondition);
      this.panel1.Controls.Add((Control) this.emHelpLink1);
      this.panel1.Controls.Add((Control) this.okBtn);
      this.panel1.Controls.Add((Control) this.tabControl1);
      this.panel1.Controls.Add((Control) this.cancelBtn);
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Controls.Add((Control) this.panelChannel);
      this.panel1.Controls.Add((Control) this.label2);
      this.panel1.Controls.Add((Control) this.label6);
      this.panel1.Controls.Add((Control) this.label3);
      this.panel1.Controls.Add((Control) this.textBoxName);
      this.panel1.Location = new Point(2, 2);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(1103, 555);
      this.panel1.TabIndex = 33;
      this.commentsTxt.Location = new Point(719, 30);
      this.commentsTxt.Multiline = true;
      this.commentsTxt.Name = "commentsTxt";
      this.commentsTxt.ScrollBars = ScrollBars.Vertical;
      this.commentsTxt.Size = new Size(334, 145);
      this.commentsTxt.TabIndex = 11;
      this.label4.AutoSize = true;
      this.label4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(716, 9);
      this.label4.Name = "label4";
      this.label4.Size = new Size(103, 13);
      this.label4.TabIndex = 10;
      this.label4.Text = "Notes/Comments";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(1103, 559);
      this.Controls.Add((Control) this.panel1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LoanActionCompletionRuleDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Loan Action Completion Rule";
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.KeyPress += new KeyPressEventHandler(this.LoanActionCompletionRuleDialog_KeyPress);
      this.tabControl1.ResumeLayout(false);
      this.tabPageEx1.ResumeLayout(false);
      this.tabPageEx2.ResumeLayout(false);
      this.tabPageEx3.ResumeLayout(false);
      this.tabPageEx5.ResumeLayout(false);
      this.tabPageEx4.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
    }

    private void initForm()
    {
      Hashtable hashtable = new Hashtable();
      this.milestoneList = new Dictionary<string, string>();
      foreach (DictionaryEntry dictionaryEntry in this.container.MSNameTable)
      {
        if (this.container.ArchivedMilestone.Contains(dictionaryEntry.Value))
          hashtable.Add(dictionaryEntry.Key, (object) (dictionaryEntry.Value.ToString() + " (Archived)"));
        else
          this.milestoneList.Add(dictionaryEntry.Key.ToString(), dictionaryEntry.Value.ToString());
      }
      foreach (DictionaryEntry dictionaryEntry in hashtable)
        this.milestoneList.Add(dictionaryEntry.Key.ToString(), dictionaryEntry.Value.ToString());
      TriggerActivationNameProvider activationNameProvider = new TriggerActivationNameProvider();
      this.loanActionList = new TriggerActivationNameProvider().GetActivationTypesByParent("TPO actions");
      List<string> stringList = new List<string>((IEnumerable<string>) this.loanActionList);
      stringList.Remove(activationNameProvider.GetDescriptionFromActivationType(TriggerActivationType.ViewPurchaseAdvice));
      stringList.Remove(activationNameProvider.GetDescriptionFromActivationType(TriggerActivationType.SubmitPurchase));
      string fromActivationType1 = activationNameProvider.GetDescriptionFromActivationType(TriggerActivationType.FloatLock);
      string fromActivationType2 = activationNameProvider.GetDescriptionFromActivationType(TriggerActivationType.ReLockLock);
      string fromActivationType3 = activationNameProvider.GetDescriptionFromActivationType(TriggerActivationType.ChangeRequestOB);
      string fromActivationType4 = activationNameProvider.GetDescriptionFromActivationType(TriggerActivationType.CancelLock);
      string fromActivationType5 = activationNameProvider.GetDescriptionFromActivationType(TriggerActivationType.RePriceLock);
      if (stringList.Contains(fromActivationType1))
        stringList.Remove(fromActivationType1);
      if (stringList.Contains(fromActivationType2))
        stringList.Remove(fromActivationType2);
      if (stringList.Contains(fromActivationType3))
        stringList.Remove(fromActivationType3);
      if (stringList.Contains(fromActivationType4))
        stringList.Remove(fromActivationType4);
      if (stringList.Contains(fromActivationType5))
        stringList.Remove(fromActivationType5);
      this.loanActionList = stringList.ToArray();
      this.emListViewDocs.Items.Clear();
      this.emListViewFields.Items.Clear();
      this.emListViewMilestones.Items.Clear();
      if (this.loanActionCompletionRule == null)
        return;
      GVItem gvItem1 = (GVItem) null;
      if (this.docsForRule != null)
      {
        for (int index = 0; index < this.docsForRule.Length; ++index)
        {
          DocumentTemplate byId = this.container.DocsInforInSettings.GetByID(this.docsForRule[index].DocGuid);
          if (byId == null)
          {
            Tracing.Log(LoanActionCompletionRuleDialog.sw, TraceLevel.Error, nameof (LoanActionCompletionRuleDialog), "initForm: can't find document '" + this.docsForRule[index].DocGuid + "' in CondList.xml.");
          }
          else
          {
            TriggerActivationType triggerActivationType = (TriggerActivationType) Enum.Parse(typeof (TriggerActivationType), this.docsForRule[index].LoanActionID);
            string fromActivationType6 = activationNameProvider.GetDescriptionFromActivationType(triggerActivationType);
            this.emListViewDocs.Items.Add(new GVItem(byId.Name)
            {
              SubItems = {
                (object) fromActivationType6,
                this.docsForRule[index].AttachedRequired ? (object) "Yes" : (object) "No"
              },
              Tag = (object) this.docsForRule[index].LoanActionID
            });
          }
        }
      }
      if (this.tasksForRule != null)
      {
        for (int index = 0; index < this.tasksForRule.Length; ++index)
        {
          if (!this.container.TasksInforInSettings.ContainsKey((object) this.tasksForRule[index].TaskGuid))
          {
            Tracing.Log(LoanActionCompletionRuleDialog.sw, TraceLevel.Error, nameof (LoanActionCompletionRuleDialog), "initForm: can't find task '" + this.tasksForRule[index].TaskGuid + "' in task setting.");
          }
          else
          {
            TriggerActivationType triggerActivationType = (TriggerActivationType) Enum.Parse(typeof (TriggerActivationType), this.tasksForRule[index].LoanActionID);
            string fromActivationType7 = activationNameProvider.GetDescriptionFromActivationType(triggerActivationType);
            this.emListViewTasks.Items.Add(new GVItem(((MilestoneTaskDefinition) this.container.TasksInforInSettings[(object) this.tasksForRule[index].TaskGuid]).TaskName)
            {
              SubItems = {
                (object) fromActivationType7
              }
            });
          }
        }
      }
      if (this.milestonesForRule != null)
      {
        for (int index = 0; index < this.milestonesForRule.Length; ++index)
        {
          TriggerActivationType triggerActivationType = (TriggerActivationType) Enum.Parse(typeof (TriggerActivationType), this.milestonesForRule[index].LoanActionID);
          string fromActivationType8 = activationNameProvider.GetDescriptionFromActivationType(triggerActivationType);
          this.addMilestoneGridViewItem(this.milestonesForRule[index].MilestoneID, fromActivationType8);
        }
      }
      if (this.fieldsForRule != null)
      {
        gvItem1 = (GVItem) null;
        string empty = string.Empty;
        for (int index = 0; index < this.fieldsForRule.Length; ++index)
        {
          TriggerActivationType triggerActivationType = (TriggerActivationType) Enum.Parse(typeof (TriggerActivationType), this.fieldsForRule[index].LoanActionID);
          string fromActivationType9 = activationNameProvider.GetDescriptionFromActivationType(triggerActivationType);
          bool flag = false;
          string fieldId = this.fieldsForRule[index].FieldID;
          if (fieldId.StartsWith("BE") || fieldId.StartsWith("CE") || fieldId.StartsWith("BR") || fieldId.StartsWith("CR") && !fieldId.StartsWith("CRED") || fieldId.StartsWith("DD"))
            flag = true;
          if (flag || EncompassFields.IsReportable(fieldId, this.fieldSettings))
            this.emListViewFields.Items.Add(new GVItem(this.fieldsForRule[index].FieldID)
            {
              SubItems = {
                (object) this.getFieldDescription(fieldId),
                (object) fromActivationType9
              }
            });
        }
      }
      if (this.advCodesForRule != null)
      {
        for (int index = 0; index < this.advCodesForRule.Length; ++index)
        {
          TriggerActivationType triggerActivationType = (TriggerActivationType) Enum.Parse(typeof (TriggerActivationType), this.advCodesForRule[index].LoanActionID);
          string fromActivationType10 = activationNameProvider.GetDescriptionFromActivationType(triggerActivationType);
          GVItem gvItem2 = new GVItem(this.getSourceCodeDescription(this.advCodesForRule[index].SourceCode));
          gvItem2.SubItems.Add((object) fromActivationType10);
          gvItem2.SubItems.Add((object) this.advCodesForRule[index].SourceCode);
          this.emListViewCode.Items.Add(gvItem2);
          gvItem2.Tag = (object) this.advCodesForRule[index].SourceCode;
        }
      }
      this.ruleCondForm.SetCondition((BizRuleInfo) this.loanActionCompletionRule);
      this.ruleCondForm_ChangesMadeToConditions((object) null, (EventArgs) null);
      this.textBoxName.Text = this.loanActionCompletionRule.RuleName;
      if (this.loanActionCompletionRule.CommentsTxt.Contains("\n") && !this.loanActionCompletionRule.CommentsTxt.Contains(Environment.NewLine))
        this.commentsTxt.Text = this.loanActionCompletionRule.CommentsTxt.Replace("\n", Environment.NewLine);
      else
        this.commentsTxt.Text = this.loanActionCompletionRule.CommentsTxt;
    }

    private void addDocBtn_Click(object sender, EventArgs e)
    {
      Hashtable existDocs = new Hashtable();
      string empty = string.Empty;
      string str = string.Empty;
      for (int nItemIndex = 0; nItemIndex < this.emListViewDocs.Items.Count; ++nItemIndex)
      {
        string lower = this.emListViewDocs.Items[nItemIndex].SubItems[0].Text.Trim().ToLower();
        str = this.emListViewDocs.Items[nItemIndex].SubItems[1].Text;
        if (!existDocs.ContainsKey((object) lower))
          existDocs.Add((object) lower, (object) lower);
      }
      using (AddDocsForLoanActionCompletion actionCompletion = new AddDocsForLoanActionCompletion(this.session, this.getLoanActionList(true), existDocs, this.container))
      {
        if (actionCompletion.ShowDialog((IWin32Window) this) == DialogResult.OK)
        {
          DocLoanActionPair[] docsForRule = actionCompletion.DocsForRule;
          if (docsForRule.Length != 0)
          {
            this.emListViewDocs.BeginUpdate();
            for (int index = 0; index < docsForRule.Length; ++index)
            {
              DocumentTemplate byId = this.container.DocsInforInSettings.GetByID(docsForRule[index].DocGuid);
              if (byId == null)
              {
                int num = (int) Utils.Dialog((IWin32Window) this, "The document '" + docsForRule[index].DocGuid + "' is invalid. It might be removed by another user.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              }
              else
                this.emListViewDocs.Items.Add(new GVItem(byId.Name)
                {
                  SubItems = {
                    (object) new TriggerActivationNameProvider().GetDescriptionFromActivationType((TriggerActivationType) Enum.Parse(typeof (TriggerActivationType), docsForRule[index].LoanActionID)).ToString(),
                    actionCompletion.NeedAttachment ? (object) "Yes" : (object) "No"
                  },
                  Tag = (object) docsForRule[index].LoanActionID
                });
            }
            this.emListViewDocs.EndUpdate();
          }
        }
        actionCompletion.Dispose();
      }
    }

    private void removeBtn_Click(object sender, EventArgs e)
    {
      Button button = (Button) sender;
      string str = "document";
      GridView gridView;
      if (button.Name == "removeFieldBtn")
      {
        gridView = this.emListViewFields;
        str = "field";
      }
      else if (button.Name == "btnRemoveTask")
      {
        gridView = this.emListViewTasks;
        str = "task";
      }
      else if (button.Name == "removeCodeBtn")
      {
        gridView = this.emListViewCode;
        str = "validation rule";
      }
      else if (button.Name == "removeMilestoneBtn")
      {
        gridView = this.emListViewMilestones;
        str = "milestone";
      }
      else
        gridView = this.emListViewDocs;
      if (gridView.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You have to select a " + str + " first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        int num2 = gridView.Items.Count - 1;
        int index = gridView.SelectedItems[0].Index;
        for (int nItemIndex = num2; nItemIndex >= 0; --nItemIndex)
        {
          if (gridView.Items[nItemIndex].Selected)
            gridView.Items.RemoveAt(nItemIndex);
        }
        if (gridView.Items.Count == 0)
          return;
        if (index > gridView.Items.Count - 1)
          gridView.Items[gridView.Items.Count - 1].Selected = true;
        else
          gridView.Items[index].Selected = true;
      }
    }

    private void addMilestoneBtn_Click(object sender, EventArgs e)
    {
      using (AddMilestonesForLoanActionCompletion actionCompletion = new AddMilestonesForLoanActionCompletion(this.session, this.getLoanActionList(true), this.milestoneList))
      {
        if (actionCompletion.ShowDialog((IWin32Window) this) == DialogResult.OK)
        {
          MilestoneLoanActionPair[] milestonesForRule = actionCompletion.MilestonesForRule;
          if (milestonesForRule.Length != 0)
          {
            this.emListViewMilestones.BeginUpdate();
            for (int index = 0; index < milestonesForRule.Length; ++index)
              this.addMilestoneGridViewItem(milestonesForRule[index].MilestoneID, actionCompletion.SelectedLoanAction);
            this.emListViewMilestones.EndUpdate();
          }
        }
        actionCompletion.Dispose();
      }
    }

    private void addMilestoneGridViewItem(string milestoneID, string loanActionName)
    {
      this.emListViewMilestones.Items.Add(new GVItem(this.milestoneList[milestoneID].ToString())
      {
        SubItems = {
          (object) loanActionName
        },
        Tag = (object) milestoneID
      });
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (this.textBoxName.Text == "")
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please enter a rule name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        BizRuleInfo[] allRules = ((BpmManager) this.session.BPM.GetBpmManager(BpmCategory.LoanActionCompletionRules)).GetAllRules();
        for (int index = 0; index < allRules.Length; ++index)
        {
          if (string.Compare(this.textBoxName.Text.Trim(), allRules[index].RuleName, StringComparison.OrdinalIgnoreCase) == 0)
          {
            bool flag = false;
            if (this.loanActionCompletionRule == null)
              flag = true;
            else if (this.loanActionCompletionRule.RuleID != allRules[index].RuleID || this.loanActionCompletionRule.RuleID == 0)
              flag = true;
            if (flag)
            {
              int num2 = (int) Utils.Dialog((IWin32Window) this, "The rule name that you entered is already in use. Please try a different rule name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              this.textBoxName.Focus();
              return;
            }
          }
        }
        if (!this.ruleCondForm.ValidateCondition())
          return;
        string channelValue = this.channelControl.ChannelValue;
        if (this.emListViewDocs.Items.Count == 0 && this.emListViewFields.Items.Count == 0 && this.emListViewTasks.Items.Count == 0 && this.emListViewMilestones.Items.Count == 0 && this.emListViewCode.Items.Count == 0)
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, "Please add a document, task, field, milestone or advanced code element.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          for (int nItemIndex = 0; nItemIndex < this.emListViewDocs.Items.Count; ++nItemIndex)
          {
            if (!this.validateTPOActionRuleCondition(this.emListViewDocs.Items[nItemIndex].SubItems[1].Text))
            {
              int num4 = (int) Utils.Dialog((IWin32Window) this, "You need to select/change a loan action for document '" + this.emListViewDocs.Items[nItemIndex].Text + "'.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              this.tabControl1.SelectedPage = this.tabPageEx1;
              this.emListViewDocs.Items[nItemIndex].Selected = true;
              return;
            }
          }
          for (int nItemIndex = 0; nItemIndex < this.emListViewTasks.Items.Count; ++nItemIndex)
          {
            if (!this.validateTPOActionRuleCondition(this.emListViewTasks.Items[nItemIndex].SubItems[1].Text))
            {
              int num5 = (int) Utils.Dialog((IWin32Window) this, "You need to select/change a loan action for task " + this.emListViewTasks.Items[nItemIndex].Text + ".", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              this.tabControl1.SelectedPage = this.tabPageEx2;
              this.emListViewTasks.Items[nItemIndex].Selected = true;
              return;
            }
          }
          for (int nItemIndex = 0; nItemIndex < this.emListViewFields.Items.Count; ++nItemIndex)
          {
            if (!this.validateTPOActionRuleCondition(this.emListViewFields.Items[nItemIndex].SubItems[2].Text))
            {
              int num6 = (int) Utils.Dialog((IWin32Window) this, "You need to select/change a loan action for field " + this.emListViewFields.Items[nItemIndex].Text + ".", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              this.tabControl1.SelectedPage = this.tabPageEx3;
              this.emListViewFields.Items[nItemIndex].Selected = true;
              return;
            }
          }
          for (int nItemIndex = 0; nItemIndex < this.emListViewMilestones.Items.Count; ++nItemIndex)
          {
            if (!this.validateTPOActionRuleCondition(this.emListViewMilestones.Items[nItemIndex].SubItems[1].Text))
            {
              int num7 = (int) Utils.Dialog((IWin32Window) this, "You need to select/change a loan action for milestone " + this.emListViewMilestones.Items[nItemIndex].Text + ".", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              this.tabControl1.SelectedPage = this.tabPageEx5;
              this.emListViewMilestones.Items[nItemIndex].Selected = true;
              return;
            }
          }
          for (int nItemIndex = 0; nItemIndex < this.emListViewCode.Items.Count; ++nItemIndex)
          {
            if (!this.validateTPOActionRuleCondition(this.emListViewCode.Items[nItemIndex].SubItems[1].Text))
            {
              int num8 = (int) Utils.Dialog((IWin32Window) this, "You need to select/change a loan action for all of your advanced coding rules.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              this.tabControl1.SelectedPage = this.tabPageEx4;
              this.emListViewCode.Items[nItemIndex].Selected = true;
              return;
            }
          }
          ArrayList arrayList1 = new ArrayList();
          for (int nItemIndex = this.emListViewDocs.Items.Count - 1; nItemIndex >= 0; --nItemIndex)
          {
            string str = this.emListViewDocs.Items[nItemIndex].Text.Trim().ToLower() + "|" + this.emListViewDocs.Items[nItemIndex].SubItems[1].Text.Trim().ToLower();
            if (arrayList1.Contains((object) str))
              this.emListViewDocs.Items.Remove(this.emListViewDocs.Items[nItemIndex]);
            else
              arrayList1.Add((object) str);
          }
          int num9 = this.emListViewTasks.Items.Count - 1;
          arrayList1.Clear();
          for (int nItemIndex = num9; nItemIndex >= 0; --nItemIndex)
          {
            string str = this.emListViewTasks.Items[nItemIndex].Text.Trim().ToLower() + "|" + this.emListViewTasks.Items[nItemIndex].SubItems[1].Text.Trim().ToLower();
            if (arrayList1.Contains((object) str))
              this.emListViewTasks.Items.Remove(this.emListViewTasks.Items[nItemIndex]);
            else
              arrayList1.Add((object) str);
          }
          int num10 = this.emListViewFields.Items.Count - 1;
          arrayList1.Clear();
          for (int nItemIndex = num10; nItemIndex >= 0; --nItemIndex)
          {
            string str = "|" + this.emListViewFields.Items[nItemIndex].Text.Trim().ToLower() + "|" + this.emListViewFields.Items[nItemIndex].SubItems[2].Text.Trim().ToLower() + "|";
            if (arrayList1.Contains((object) str))
              this.emListViewFields.Items.Remove(this.emListViewFields.Items[nItemIndex]);
            else
              arrayList1.Add((object) str);
          }
          int num11 = this.emListViewMilestones.Items.Count - 1;
          arrayList1.Clear();
          for (int nItemIndex = num11; nItemIndex >= 0; --nItemIndex)
          {
            string str = "|" + this.emListViewMilestones.Items[nItemIndex].Text.Trim().ToLower() + "|" + this.emListViewMilestones.Items[nItemIndex].SubItems[1].Text.Trim().ToLower() + "|";
            if (arrayList1.Contains((object) str))
              this.emListViewMilestones.Items.Remove(this.emListViewMilestones.Items[nItemIndex]);
            else
              arrayList1.Add((object) str);
          }
          ArrayList arrayList2 = new ArrayList();
          TriggerActivationType triggerActivationType;
          for (int nItemIndex = 0; nItemIndex < this.emListViewDocs.Items.Count; ++nItemIndex)
          {
            string name = this.emListViewDocs.Items[nItemIndex].Text.Trim();
            triggerActivationType = (TriggerActivationType) new TriggerActivationNameProvider().GetValue(this.emListViewDocs.Items[nItemIndex].SubItems[1].Text);
            string loanActionID = triggerActivationType.ToString();
            DocumentTemplate byName = this.container.DocsInforInSettings.GetByName(name);
            if (byName != null)
            {
              bool attachedRequired = this.emListViewDocs.Items[nItemIndex].SubItems[2].Text == "Yes";
              arrayList2.Add((object) new DocLoanActionPair(byName.Guid, loanActionID, attachedRequired));
            }
          }
          this.docsForRule = (DocLoanActionPair[]) arrayList2.ToArray(typeof (DocLoanActionPair));
          ArrayList arrayList3 = new ArrayList();
          for (int nItemIndex = 0; nItemIndex < this.emListViewTasks.Items.Count; ++nItemIndex)
          {
            string key = this.emListViewTasks.Items[nItemIndex].SubItems[1].Tag.ToString();
            triggerActivationType = (TriggerActivationType) new TriggerActivationNameProvider().GetValue(this.emListViewTasks.Items[nItemIndex].SubItems[1].Text);
            string loanActionID = triggerActivationType.ToString();
            MilestoneTaskDefinition milestoneTaskDefinition = (MilestoneTaskDefinition) null;
            if (this.container.TasksInforInSettings.ContainsKey((object) key))
              milestoneTaskDefinition = (MilestoneTaskDefinition) this.container.TasksInforInSettings[(object) key];
            if (milestoneTaskDefinition != null)
              arrayList3.Add((object) new TaskLoanActionPair(milestoneTaskDefinition.TaskGUID, loanActionID));
          }
          this.tasksForRule = (TaskLoanActionPair[]) arrayList3.ToArray(typeof (TaskLoanActionPair));
          ArrayList arrayList4 = new ArrayList();
          for (int nItemIndex = 0; nItemIndex < this.emListViewFields.Items.Count; ++nItemIndex)
          {
            string fieldID = this.emListViewFields.Items[nItemIndex].Text.Trim();
            triggerActivationType = (TriggerActivationType) new TriggerActivationNameProvider().GetValue(this.emListViewFields.Items[nItemIndex].SubItems[2].Text);
            string loanActionID = triggerActivationType.ToString();
            arrayList4.Add((object) new FieldLoanActionPair(fieldID, loanActionID));
          }
          this.fieldsForRule = (FieldLoanActionPair[]) arrayList4.ToArray(typeof (FieldLoanActionPair));
          ArrayList arrayList5 = new ArrayList();
          for (int nItemIndex = 0; nItemIndex < this.emListViewMilestones.Items.Count; ++nItemIndex)
          {
            string milestoneID = this.emListViewMilestones.Items[nItemIndex].Tag.ToString().Trim();
            triggerActivationType = (TriggerActivationType) new TriggerActivationNameProvider().GetValue(this.emListViewMilestones.Items[nItemIndex].SubItems[1].Text);
            string loanActionID = triggerActivationType.ToString();
            arrayList5.Add((object) new MilestoneLoanActionPair(milestoneID, loanActionID));
          }
          this.milestonesForRule = (MilestoneLoanActionPair[]) arrayList5.ToArray(typeof (MilestoneLoanActionPair));
          ArrayList arrayList6 = new ArrayList();
          for (int nItemIndex = 0; nItemIndex < this.emListViewCode.Items.Count; ++nItemIndex)
          {
            string text = this.emListViewCode.Items[nItemIndex].SubItems[2].Text;
            triggerActivationType = (TriggerActivationType) new TriggerActivationNameProvider().GetValue(this.emListViewCode.Items[nItemIndex].SubItems[1].Text);
            string loanActionId = triggerActivationType.ToString();
            arrayList6.Add((object) new AdvancedCodeLoanActionPair(loanActionId, text));
          }
          this.advCodesForRule = (AdvancedCodeLoanActionPair[]) arrayList6.ToArray(typeof (AdvancedCodeLoanActionPair));
          this.loanActionCompletionRule = this.loanActionCompletionRule == null ? new LoanActionCompletionRuleInfo(this.textBoxName.Text.Trim()) : new LoanActionCompletionRuleInfo(this.loanActionCompletionRule.RuleID, this.textBoxName.Text.Trim());
          this.loanActionCompletionRule.Fields = this.fieldsForRule;
          this.loanActionCompletionRule.Docs = this.docsForRule;
          this.loanActionCompletionRule.Tasks = this.tasksForRule;
          this.loanActionCompletionRule.Milestones = this.milestonesForRule;
          this.loanActionCompletionRule.AdvancedCodes = this.advCodesForRule;
          this.ruleCondForm.ApplyCondition((BizRuleInfo) this.loanActionCompletionRule);
          this.loanActionCompletionRule.Condition2 = this.channelControl.ChannelValue;
          this.loanActionCompletionRule.CommentsTxt = this.commentsTxt.Text;
          this.DialogResult = DialogResult.OK;
        }
      }
    }

    private bool validateTPOActionRuleCondition(string selectedTPOLoanAction)
    {
      return !(selectedTPOLoanAction == "") && (this.selectedTPORuleConditionValue == null || !this.selectedTPORuleConditionValue.Contains(selectedTPOLoanAction)) || this.mustHaveLoanActionList.Contains(selectedTPOLoanAction);
    }

    private void addFieldBtn_Click(object sender, EventArgs e)
    {
      using (AddFields addFields = new AddFields(this.session, (string) null, AddFieldOptions.AllowCustomFields | AddFieldOptions.AllowVirtualFields | AddFieldOptions.AllowButtons))
      {
        addFields.SetMilestoneSelection(this.getLoanActionList(true));
        addFields.SetComboLabelText = "For Loan Action";
        addFields.SetComboIndex(0);
        addFields.OnAddMoreButtonClick += new EventHandler(this.addFieldDlg_OnAddMoreButtonClick);
        if (addFields.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.addFields(addFields.SelectedFieldIDs, addFields.SelectedRuleValue);
      }
    }

    private void addFields(string[] ids, string loanActionName)
    {
      if (ids.Length == 0)
        return;
      this.emListViewFields.BeginUpdate();
      for (int index = 0; index < ids.Length; ++index)
      {
        bool flag = false;
        for (int nItemIndex = 0; nItemIndex < this.emListViewFields.Items.Count; ++nItemIndex)
        {
          if (string.Compare(this.emListViewFields.Items[nItemIndex].Text, ids[index], true) == 0)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The field list already contains field '" + ids[index] + "'. This field will be ignored.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            flag = true;
            break;
          }
        }
        if (!flag)
          this.emListViewFields.Items.Add(new GVItem(ids[index])
          {
            SubItems = {
              (object) this.getFieldDescription(ids[index]),
              (object) loanActionName
            },
            Tag = (object) ""
          });
      }
      this.emListViewFields.EndUpdate();
    }

    private void findBtn_Click(object sender, EventArgs e)
    {
      ArrayList arrayList = new ArrayList();
      for (int nItemIndex = 0; nItemIndex < this.emListViewFields.Items.Count; ++nItemIndex)
        arrayList.Add((object) this.emListViewFields.Items[nItemIndex].Text);
      string empty = string.Empty;
      using (BusinessRuleFindFieldDialog ruleFindFieldDialog = new BusinessRuleFindFieldDialog(this.session, (string[]) arrayList.ToArray(typeof (string)), true, string.Empty, false, false))
      {
        ruleFindFieldDialog.SetMilestoneSelection(this.getLoanActionList(true));
        ruleFindFieldDialog.SetComboLabelText = "For Loan Action";
        ruleFindFieldDialog.SetComboIndex(0);
        if (ruleFindFieldDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        Cursor.Current = Cursors.WaitCursor;
        this.emListViewFields.BeginUpdate();
        for (int index = 0; index < ruleFindFieldDialog.SelectedRequiredFields.Length; ++index)
        {
          if (!(ruleFindFieldDialog.SelectedRequiredFields[index] == "") && !arrayList.Contains((object) ruleFindFieldDialog.SelectedRequiredFields[index]) && EncompassFields.IsReportable(ruleFindFieldDialog.SelectedRequiredFields[index], this.fieldSettings))
            this.emListViewFields.Items.Add(new GVItem(ruleFindFieldDialog.SelectedRequiredFields[index])
            {
              SubItems = {
                (object) this.getFieldDescription(ruleFindFieldDialog.SelectedRequiredFields[index]),
                (object) ruleFindFieldDialog.SelectedRuleValue
              },
              Tag = (object) ""
            });
        }
        this.emListViewFields.EndUpdate();
        Cursor.Current = Cursors.Default;
      }
    }

    private string getFieldDescription(string fieldID)
    {
      return EncompassFields.GetDescription(fieldID, this.fieldSettings);
    }

    private void addFieldDlg_OnAddMoreButtonClick(object sender, EventArgs e)
    {
      AddFields addFields = (AddFields) sender;
      if (addFields == null)
        return;
      this.addFields(addFields.SelectedFieldIDs, addFields.SelectedRuleValue);
    }

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.F1)
      {
        this.ShowHelp();
      }
      else
      {
        if (e.KeyCode != Keys.Escape)
          return;
        this.cancelBtn.PerformClick();
      }
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, nameof (LoanActionCompletionRuleDialog));
    }

    private void btnAddTask_Click(object sender, EventArgs e)
    {
      Hashtable existDocs = new Hashtable();
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      for (int nItemIndex = 0; nItemIndex < this.emListViewTasks.Items.Count; ++nItemIndex)
      {
        string lower = this.emListViewTasks.Items[nItemIndex].SubItems[0].Text.Trim().ToLower();
        if (!existDocs.ContainsKey((object) lower))
          existDocs.Add((object) lower, (object) lower);
      }
      using (AddTasksForLoanActionCompletion actionCompletion = new AddTasksForLoanActionCompletion(this.session, this.getLoanActionList(true), existDocs))
      {
        if (actionCompletion.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        TaskLoanActionPair[] tasksForRule = actionCompletion.TasksForRule;
        if (tasksForRule.Length == 0)
          return;
        this.emListViewTasks.BeginUpdate();
        TriggerActivationNameProvider activationNameProvider = new TriggerActivationNameProvider();
        for (int index = 0; index < tasksForRule.Length; ++index)
        {
          if (this.container.TasksInforInSettings.ContainsKey((object) tasksForRule[index].TaskGuid))
          {
            GVItem gvItem = new GVItem(((MilestoneTaskDefinition) this.container.TasksInforInSettings[(object) tasksForRule[index].TaskGuid]).TaskName);
            TriggerActivationType triggerActivationType = (TriggerActivationType) Enum.Parse(typeof (TriggerActivationType), tasksForRule[index].LoanActionID);
            string fromActivationType = activationNameProvider.GetDescriptionFromActivationType(triggerActivationType);
            gvItem.SubItems.Add((object) fromActivationType);
            gvItem.Tag = (object) tasksForRule[index].LoanActionID;
            gvItem.SubItems[1].Tag = (object) tasksForRule[index].TaskGuid;
            this.emListViewTasks.Items.Add(gvItem);
          }
          else
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The task '" + tasksForRule[index].TaskGuid + "' is invalid. It might be removed by another user.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
        this.emListViewTasks.EndUpdate();
      }
    }

    private void addCodeBtn_Click(object sender, EventArgs e)
    {
      using (AddAdvCodeForLoanActionCompletion actionCompletion = new AddAdvCodeForLoanActionCompletion())
      {
        if (actionCompletion.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        GVItem gvItem = new GVItem(this.getSourceCodeDescription(actionCompletion.SourceCode));
        gvItem.SubItems.Add((object) "");
        gvItem.SubItems.Add((object) actionCompletion.SourceCode);
        this.emListViewCode.Items.Add(gvItem);
        gvItem.Tag = (object) actionCompletion.SourceCode;
      }
    }

    private void emListViewCode_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.hasNoAccess)
        return;
      this.enableDisableButtons();
    }

    private void enableDisableButtons()
    {
      this.editCodeBtn.Enabled = this.emListViewCode.SelectedItems.Count == 1;
      this.removeCodeBtn.Enabled = this.emListViewCode.SelectedItems.Count > 0;
    }

    private void emListViewCode_DoubleClick(object sender, EventArgs e)
    {
    }

    private void editCodeBtn_Click(object sender, EventArgs e)
    {
      if (this.emListViewCode.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select an item to edit.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
        this.editAdvancedCode(this.emListViewCode.SelectedItems[0]);
    }

    private void editAdvancedCode(GVItem listItem)
    {
      using (AddAdvCodeForLoanActionCompletion actionCompletion = new AddAdvCodeForLoanActionCompletion(string.Concat(listItem.Tag)))
      {
        if (actionCompletion.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        listItem.SubItems[0].Text = this.getSourceCodeDescription(actionCompletion.SourceCode);
        listItem.SubItems[2].Text = actionCompletion.SourceCode;
      }
    }

    private string getSourceCodeDescription(string sourceCode)
    {
      string[] strArray = sourceCode.Replace(Environment.NewLine, "\r").Split('\r');
      if (strArray.Length == 1)
        return sourceCode.Trim();
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (strArray[index].Trim() != "")
          return strArray[index].Trim() + "... (" + (object) strArray.Length + " lines)";
      }
      return "<Source Code>";
    }

    private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.enableDisableButtons();
    }

    private void emListViewDocs_EditorOpening(object source, GVSubItemEditingEventArgs e)
    {
      if (e.SubItem.Index == 1)
      {
        this.loanActionEditorOpening(e);
      }
      else
      {
        if (e.SubItem.Index != 2)
          return;
        ComboBox editorControl = (ComboBox) e.EditorControl;
        editorControl.Items.Clear();
        editorControl.DropDownStyle = ComboBoxStyle.DropDownList;
        editorControl.Items.AddRange((object[]) new string[2]
        {
          "Yes",
          "No"
        });
        editorControl.Text = (e.SubItem.Text ?? "") == "" ? "No" : e.SubItem.Text;
      }
    }

    private void emListViewMilestones_EditorOpening(object source, GVSubItemEditingEventArgs e)
    {
      if (e.SubItem.Index != 1)
        return;
      this.loanActionEditorOpening(e);
    }

    private void gridView_EditorClosing(object source, GVSubItemEditingEventArgs e)
    {
      this.loanActionEditorClosing(e);
    }

    private void loanActionEditorOpening(GVSubItemEditingEventArgs e)
    {
      ComboBox editorControl = (ComboBox) e.EditorControl;
      editorControl.Items.Clear();
      editorControl.DropDownStyle = ComboBoxStyle.DropDownList;
      editorControl.Items.AddRange((object[]) this.getLoanActionList(true));
      editorControl.Text = e.SubItem.Text;
    }

    private void loanActionEditorClosing(GVSubItemEditingEventArgs e)
    {
      ComboBox editorControl = (ComboBox) e.EditorControl;
      e.SubItem.Text = editorControl.SelectedText;
    }

    private void emListViewTasks_EditorOpening(object source, GVSubItemEditingEventArgs e)
    {
      if (e.SubItem.Index != 1)
        return;
      this.loanActionEditorOpening(e);
    }

    private void emListViewFields_EditorOpening(object source, GVSubItemEditingEventArgs e)
    {
      if (e.SubItem.Index != 2)
        return;
      this.loanActionEditorOpening(e);
    }

    private void emListViewCode_EditorOpening(object source, GVSubItemEditingEventArgs e)
    {
      if (e.SubItem.Index != 1)
        return;
      this.loanActionEditorOpening(e);
    }

    private void LoanActionCompletionRuleDialog_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\u001B')
        return;
      this.Close();
    }

    private void filterLoanActions()
    {
      if (this.selectedTPORuleConditionValue != null && this.selectedTPORuleConditionValue.Count != 0)
      {
        this.filteredLoanActionList = new List<string>();
        foreach (string loanAction in this.loanActionList)
        {
          if (!this.selectedTPORuleConditionValue.Contains(loanAction) || this.mustHaveLoanActionList.Contains(loanAction))
            this.filteredLoanActionList.Add(loanAction);
        }
      }
      else
        this.filteredLoanActionList = (List<string>) null;
    }

    private string[] getLoanActionList(bool forTPORuleCondition)
    {
      return forTPORuleCondition && this.filteredLoanActionList != null ? this.filteredLoanActionList.ToArray() : this.loanActionList;
    }

    private List<string> getMustHaveLoanActionsList()
    {
      return new List<string>()
      {
        new TriggerActivationNameProvider().GetDescriptionFromActivationType(TriggerActivationType.RequestTitleFees),
        new TriggerActivationNameProvider().GetDescriptionFromActivationType(TriggerActivationType.GenerateLoanEstimateDisclosure),
        new TriggerActivationNameProvider().GetDescriptionFromActivationType(TriggerActivationType.OrderAppraisalRequest)
      };
    }
  }
}
