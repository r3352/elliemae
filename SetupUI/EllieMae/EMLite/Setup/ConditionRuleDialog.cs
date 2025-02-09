// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ConditionRuleDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using Elli.ElliEnum;
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
using EllieMae.EMLite.Workflow;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class ConditionRuleDialog : Form
  {
    private const string className = "ConditionRuleDialog";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private Sessions.Session session;
    private bool hasNoAccess;
    private Button cancelBtn;
    private Button removeDocBtn;
    private Button addDocBtn;
    private Button removeFieldBtn;
    private Button addFieldBtn;
    private IContainer components;
    private Button okBtn;
    private TextBox textBoxName;
    private RuleConditionControl ruleCondForm;
    private DocMilestonePair[] docsForRule;
    private FieldMilestonePair[] fieldsForRule;
    private TaskMilestonePair[] tasksForRule;
    private AdvancedCodeMilestonePair[] advCodesForRule;
    private const string ARCHIVED = " (Archived)";
    private Label label1;
    private Label label2;
    private Label label3;
    private Panel panelCondition;
    private Button findBtn;
    private ConditionRulePanel container;
    private string[] msList;
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
    private Label label4;
    private TextBox commentsTxt;
    private ImageList imageList1;
    private ToolTip toolTip1;
    private GridView emListViewDocs;
    private MilestoneRuleInfo milestoneRule;

    public ConditionRuleDialog(
      Sessions.Session session,
      MilestoneRuleInfo milestoneRule,
      ConditionRulePanel container)
    {
      this.session = session;
      this.container = container;
      this.milestoneRule = milestoneRule;
      if (milestoneRule != null)
      {
        this.docsForRule = milestoneRule.Docs;
        this.fieldsForRule = milestoneRule.Fields;
        this.tasksForRule = milestoneRule.Tasks;
        this.advCodesForRule = milestoneRule.AdvancedCodes;
      }
      this.InitializeComponent();
      this.emHelpLink1.AssignSession(this.session);
      this.fieldSettings = this.session.LoanManager.GetFieldSettings();
      this.channelControl = new ChannelConditionControl();
      if (this.milestoneRule != null)
        this.channelControl.ChannelValue = this.milestoneRule.Condition2;
      this.panelChannel.Controls.Add((Control) this.channelControl);
      this.ruleCondForm = new RuleConditionControl(this.session);
      this.ruleCondForm.InitControl(BpmCategory.MilestoneRules);
      this.panelCondition.Controls.Add((Control) this.ruleCondForm);
      this.initForm();
      this.emListViewFields.SelectedIndexChanged += new EventHandler(this.emListViewFields_SelectedIndexChanged);
      this.emListViewDocs.SelectedIndexChanged += new EventHandler(this.emListViewDocs_SelectedIndexChanged);
      this.emListViewTasks.SelectedIndexChanged += new EventHandler(this.emListViewTasks_SelectedIndexChanged);
      this.emListViewTasks.SubItemEnter += new GVSubItemEventHandler(this.emListViewTasks_SubItemEnter);
      this.emListViewTasks.SubItemLeave += new GVSubItemEventHandler(this.emListViewTasks_SubItemLeave);
      this.emListViewTasks_SelectedIndexChanged((object) null, (EventArgs) null);
      this.emListViewDocs_SelectedIndexChanged((object) null, (EventArgs) null);
      this.emListViewFields_SelectedIndexChanged((object) null, (EventArgs) null);
      this.emListViewCode_SelectedIndexChanged((object) null, (EventArgs) null);
      this.hasNoAccess = !((FeaturesAclManager) session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.SettingsTab_MilestoneCompletion);
      this.addFieldBtn.Enabled = this.removeFieldBtn.Enabled = this.findBtn.Enabled = this.addDocBtn.Enabled = this.removeDocBtn.Enabled = this.btnAddTask.Enabled = this.btnRemoveTask.Enabled = this.addCodeBtn.Enabled = this.removeCodeBtn.Enabled = this.editCodeBtn.Enabled = this.okBtn.Enabled = !this.hasNoAccess;
      if (!this.hasNoAccess)
        return;
      this.textBoxName.Enabled = false;
      this.ruleCondForm.DisableControls();
      this.channelControl.DisableControls();
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

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public MilestoneRuleInfo MilestoneRule => this.milestoneRule;

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      GVColumn gvColumn8 = new GVColumn();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ConditionRuleDialog));
      GVColumn gvColumn9 = new GVColumn();
      GVColumn gvColumn10 = new GVColumn();
      GVColumn gvColumn11 = new GVColumn();
      GVColumn gvColumn12 = new GVColumn();
      GVColumn gvColumn13 = new GVColumn();
      this.cancelBtn = new Button();
      this.textBoxName = new TextBox();
      this.removeDocBtn = new Button();
      this.addDocBtn = new Button();
      this.btnRemoveTask = new Button();
      this.btnAddTask = new Button();
      this.findBtn = new Button();
      this.removeFieldBtn = new Button();
      this.addFieldBtn = new Button();
      this.editCodeBtn = new Button();
      this.removeCodeBtn = new Button();
      this.addCodeBtn = new Button();
      this.okBtn = new Button();
      this.label1 = new Label();
      this.label2 = new Label();
      this.panelCondition = new Panel();
      this.label3 = new Label();
      this.emHelpLink1 = new EMHelpLink();
      this.label6 = new Label();
      this.panelChannel = new Panel();
      this.tabControl1 = new TabControlEx();
      this.tabPageEx1 = new TabPageEx();
      this.emListViewDocs = new GridView();
      this.tabPageEx2 = new TabPageEx();
      this.emListViewTasks = new GridView();
      this.imageList1 = new ImageList(this.components);
      this.tabPageEx3 = new TabPageEx();
      this.emListViewFields = new GridView();
      this.tabPageEx4 = new TabPageEx();
      this.emListViewCode = new GridView();
      this.label4 = new Label();
      this.commentsTxt = new TextBox();
      this.toolTip1 = new ToolTip(this.components);
      this.tabControl1.SuspendLayout();
      this.tabPageEx1.SuspendLayout();
      this.tabPageEx2.SuspendLayout();
      this.tabPageEx3.SuspendLayout();
      this.tabPageEx4.SuspendLayout();
      this.SuspendLayout();
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(969, 603);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 23);
      this.cancelBtn.TabIndex = 14;
      this.cancelBtn.Text = "Cancel";
      this.textBoxName.Location = new Point(32, 30);
      this.textBoxName.MaxLength = 64;
      this.textBoxName.Name = "textBoxName";
      this.textBoxName.Size = new Size(653, 20);
      this.textBoxName.TabIndex = 1;
      this.removeDocBtn.Location = new Point(564, 45);
      this.removeDocBtn.Name = "removeDocBtn";
      this.removeDocBtn.Size = new Size(75, 23);
      this.removeDocBtn.TabIndex = 9;
      this.removeDocBtn.Text = "&Remove";
      this.removeDocBtn.Click += new EventHandler(this.removeBtn_Click);
      this.addDocBtn.Location = new Point(564, 16);
      this.addDocBtn.Name = "addDocBtn";
      this.addDocBtn.Size = new Size(75, 23);
      this.addDocBtn.TabIndex = 8;
      this.addDocBtn.Text = "&Add";
      this.addDocBtn.Click += new EventHandler(this.addDocBtn_Click);
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
      this.findBtn.Location = new Point(564, 42);
      this.findBtn.Name = "findBtn";
      this.findBtn.Size = new Size(75, 23);
      this.findBtn.TabIndex = 22;
      this.findBtn.Text = "&Find";
      this.findBtn.Click += new EventHandler(this.findBtn_Click);
      this.removeFieldBtn.Location = new Point(564, 68);
      this.removeFieldBtn.Name = "removeFieldBtn";
      this.removeFieldBtn.Size = new Size(75, 23);
      this.removeFieldBtn.TabIndex = 12;
      this.removeFieldBtn.Text = "&Remove";
      this.removeFieldBtn.Click += new EventHandler(this.removeBtn_Click);
      this.addFieldBtn.Location = new Point(564, 16);
      this.addFieldBtn.Name = "addFieldBtn";
      this.addFieldBtn.Size = new Size(75, 23);
      this.addFieldBtn.TabIndex = 11;
      this.addFieldBtn.Text = "&Add";
      this.addFieldBtn.Click += new EventHandler(this.addFieldBtn_Click);
      this.editCodeBtn.Location = new Point(564, 45);
      this.editCodeBtn.Name = "editCodeBtn";
      this.editCodeBtn.Size = new Size(75, 23);
      this.editCodeBtn.TabIndex = 27;
      this.editCodeBtn.Text = "&Edit";
      this.editCodeBtn.Click += new EventHandler(this.editCodeBtn_Click);
      this.removeCodeBtn.Location = new Point(564, 74);
      this.removeCodeBtn.Name = "removeCodeBtn";
      this.removeCodeBtn.Size = new Size(75, 23);
      this.removeCodeBtn.TabIndex = 25;
      this.removeCodeBtn.Text = "&Remove";
      this.removeCodeBtn.Click += new EventHandler(this.removeBtn_Click);
      this.addCodeBtn.Location = new Point(564, 16);
      this.addCodeBtn.Name = "addCodeBtn";
      this.addCodeBtn.Size = new Size(75, 23);
      this.addCodeBtn.TabIndex = 24;
      this.addCodeBtn.Text = "&Add";
      this.addCodeBtn.Click += new EventHandler(this.addCodeBtn_Click);
      this.okBtn.Location = new Point(884, 603);
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
      this.label2.Location = new Point(16, 194);
      this.label2.Name = "label2";
      this.label2.Size = new Size(264, 16);
      this.label2.TabIndex = 4;
      this.label2.Text = "3. Is there a condition for this rule";
      this.panelCondition.Location = new Point(32, 222);
      this.panelCondition.Name = "panelCondition";
      this.panelCondition.Size = new Size(653, 92);
      this.panelCondition.TabIndex = 5;
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(16, 332);
      this.label3.Name = "label3";
      this.label3.Size = new Size(551, 16);
      this.label3.TabIndex = 6;
      this.label3.Text = "4. Select required documents, fields and tasks for each milestone";
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "Setup\\Milestone Completion";
      this.emHelpLink1.Location = new Point(19, 603);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 12;
      this.label6.AutoSize = true;
      this.label6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label6.Location = new Point(16, 70);
      this.label6.Name = "label6";
      this.label6.Size = new Size(239, 13);
      this.label6.TabIndex = 2;
      this.label6.Text = "2. Select all Channels this rule applies to";
      this.panelChannel.Location = new Point(32, 98);
      this.panelChannel.Name = "panelChannel";
      this.panelChannel.Size = new Size(653, 79);
      this.panelChannel.TabIndex = 3;
      this.tabControl1.Location = new Point(32, 363);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.Size = new Size(652, 221);
      this.tabControl1.TabHeight = 20;
      this.tabControl1.TabIndex = 7;
      this.tabControl1.TabPages.Add(this.tabPageEx1);
      this.tabControl1.TabPages.Add(this.tabPageEx2);
      this.tabControl1.TabPages.Add(this.tabPageEx3);
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
      gvColumn2.Text = "For Milestone";
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
      gvColumn4.Name = "clWarning";
      gvColumn4.SortMethod = GVSortMethod.None;
      gvColumn4.Text = "";
      gvColumn4.Width = 25;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "clTypeID";
      gvColumn5.Text = "Type ID";
      gvColumn5.Width = 100;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "clName";
      gvColumn6.Text = "Name";
      gvColumn6.Width = 150;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "clTaskGroup";
      gvColumn7.Text = "Task Group";
      gvColumn7.Width = 150;
      gvColumn8.ActivatedEditorType = GVActivatedEditorType.ComboBox;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "clMilestone";
      gvColumn8.SpringToFit = true;
      gvColumn8.Text = "For Milestone";
      gvColumn8.Width = 155;
      this.emListViewTasks.Columns.AddRange(new GVColumn[5]
      {
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8
      });
      this.emListViewTasks.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.emListViewTasks.ImageList = this.imageList1;
      this.emListViewTasks.Location = new Point(18, 16);
      this.emListViewTasks.Name = "emListViewTasks";
      this.emListViewTasks.Size = new Size(540, 166);
      this.emListViewTasks.TabIndex = 29;
      this.emListViewTasks.EditorOpening += new GVSubItemEditingEventHandler(this.emListViewTasks_EditorOpening);
      this.emListViewTasks.EditorClosing += new GVSubItemEditingEventHandler(this.gridView_EditorClosing);
      this.imageList1.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageList1.ImageStream");
      this.imageList1.TransparentColor = Color.Transparent;
      this.imageList1.Images.SetKeyName(0, "tasks.png");
      this.imageList1.Images.SetKeyName(1, "status-warning.png");
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
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "Column1";
      gvColumn9.Text = "ID";
      gvColumn9.Width = 114;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "Column2";
      gvColumn10.Text = "Description";
      gvColumn10.Width = 244;
      gvColumn11.ActivatedEditorType = GVActivatedEditorType.ComboBox;
      gvColumn11.ImageIndex = -1;
      gvColumn11.Name = "Column3";
      gvColumn11.Text = "For Milestone";
      gvColumn11.Width = 155;
      this.emListViewFields.Columns.AddRange(new GVColumn[3]
      {
        gvColumn9,
        gvColumn10,
        gvColumn11
      });
      this.emListViewFields.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.emListViewFields.Location = new Point(18, 16);
      this.emListViewFields.Name = "emListViewFields";
      this.emListViewFields.Size = new Size(540, 166);
      this.emListViewFields.TabIndex = 29;
      this.emListViewFields.EditorOpening += new GVSubItemEditingEventHandler(this.emListViewFields_EditorOpening);
      this.emListViewFields.EditorClosing += new GVSubItemEditingEventHandler(this.gridView_EditorClosing);
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
      gvColumn12.ImageIndex = -1;
      gvColumn12.Name = "Column1";
      gvColumn12.Text = "Code";
      gvColumn12.Width = 355;
      gvColumn13.ActivatedEditorType = GVActivatedEditorType.ComboBox;
      gvColumn13.ImageIndex = -1;
      gvColumn13.Name = "Column2";
      gvColumn13.Text = "For Milestone";
      gvColumn13.Width = 155;
      this.emListViewCode.Columns.AddRange(new GVColumn[2]
      {
        gvColumn12,
        gvColumn13
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
      this.label4.AutoSize = true;
      this.label4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(715, 9);
      this.label4.Name = "label4";
      this.label4.Size = new Size(103, 13);
      this.label4.TabIndex = 10;
      this.label4.Text = "Notes/Comments";
      this.commentsTxt.Location = new Point(718, 30);
      this.commentsTxt.Multiline = true;
      this.commentsTxt.Name = "commentsTxt";
      this.commentsTxt.ScrollBars = ScrollBars.Vertical;
      this.commentsTxt.Size = new Size(311, 147);
      this.commentsTxt.TabIndex = 11;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(1056, 651);
      this.Controls.Add((Control) this.commentsTxt);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.tabControl1);
      this.Controls.Add((Control) this.panelChannel);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.textBoxName);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.panelCondition);
      this.Controls.Add((Control) this.okBtn);
      this.Controls.Add((Control) this.cancelBtn);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ConditionRuleDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Milestone Completion Rule";
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.KeyPress += new KeyPressEventHandler(this.ConditionRuleDialog_KeyPress);
      this.tabControl1.ResumeLayout(false);
      this.tabPageEx1.ResumeLayout(false);
      this.tabPageEx2.ResumeLayout(false);
      this.tabPageEx3.ResumeLayout(false);
      this.tabPageEx4.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void initForm()
    {
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < this.container.MilestoneList.Length; ++index)
      {
        if (!this.container.ArchiveMilestone.Contains((object) this.container.MilestoneList[index]))
        {
          string milestone = this.container.MilestoneList[index];
          arrayList.Add((object) milestone);
        }
      }
      if (this.container.ArchiveMilestone != null)
      {
        for (int index = 0; index < this.container.ArchiveMilestone.Count; ++index)
          arrayList.Add((object) (this.container.ArchiveMilestone[index].ToString() + " (Archived)"));
      }
      this.msList = (string[]) arrayList.ToArray(typeof (string));
      this.emListViewDocs.Items.Clear();
      this.emListViewFields.Items.Clear();
      if (!this.container.IsWorkflowTaskAccessible)
      {
        this.emListViewTasks.Columns[0].Width = 0;
        this.emListViewTasks.Columns[1].Width = 0;
        this.emListViewTasks.Columns[3].Width = 0;
      }
      if (this.milestoneRule == null)
        return;
      GVItem gvItem1 = (GVItem) null;
      if (this.docsForRule != null)
      {
        for (int index = 0; index < this.docsForRule.Length; ++index)
        {
          DocumentTemplate byId = this.container.DocsInforInSettings.GetByID(this.docsForRule[index].DocGuid);
          if (byId == null)
          {
            Tracing.Log(ConditionRuleDialog.sw, TraceLevel.Error, nameof (ConditionRuleDialog), "initForm: can't find document '" + this.docsForRule[index].DocGuid + "' in CondList.xml.");
          }
          else
          {
            string str = "";
            if (this.container.MSNameTable.ContainsKey((object) this.docsForRule[index].MilestoneID))
            {
              str = this.container.MSNameTable[(object) this.docsForRule[index].MilestoneID].ToString();
              if (this.container.ArchiveMilestone.Contains((object) str))
                str += " (Archived)";
            }
            this.emListViewDocs.Items.Add(new GVItem(byId.Name)
            {
              SubItems = {
                (object) str,
                this.docsForRule[index].AttachedRequired ? (object) "Yes" : (object) "No"
              },
              Tag = (object) this.docsForRule[index].MilestoneID
            });
          }
        }
      }
      if (this.tasksForRule != null)
      {
        foreach (TaskMilestonePair taskMilestonePair in this.tasksForRule)
        {
          if (taskMilestonePair.TaskType != MilestoneTaskType.MilestoneWorkflow || this.container.IsWorkflowTaskAccessible)
          {
            bool flag = true;
            if (!this.existsMilestoneTaskInSettings(taskMilestonePair.TaskGuid, taskMilestonePair.TaskType))
            {
              Tracing.Log(ConditionRuleDialog.sw, TraceLevel.Error, nameof (ConditionRuleDialog), "initForm: can't find task '" + taskMilestonePair.TaskGuid + "' in task setting.");
              if (taskMilestonePair.TaskType == MilestoneTaskType.MilestoneWorkflow)
                flag = false;
              else
                continue;
            }
            string str = "";
            if (this.container.MSNameTable.ContainsKey((object) taskMilestonePair.MilestoneID))
            {
              str = this.container.MSNameTable[(object) taskMilestonePair.MilestoneID].ToString();
              if (this.container.ArchiveMilestone.Contains((object) str))
                str += " (Archived)";
            }
            ConditionRuleDialog.TaskLog taskLog = (ConditionRuleDialog.TaskLog) null;
            if (flag)
              taskLog = this.getMilestoneTaskInSettings(taskMilestonePair.TaskGuid, taskMilestonePair.TaskType);
            GVItem gvItem2 = new GVItem();
            if (!flag)
            {
              gvItem2.Text = string.Empty;
              gvItem2.ImageIndex = 1;
              gvItem2.SubItems.Add((object) taskMilestonePair.TaskGuid);
              gvItem2.SubItems.Add((object) taskMilestonePair.TaskName);
              gvItem2.SubItems[2].ImageIndex = 0;
              gvItem2.SubItems.Add((object) string.Empty);
              gvItem2.SubItems[3].Tag = (object) (int) taskMilestonePair.TaskType;
              gvItem2.SubItems.Add((object) str);
              gvItem2.SubItems[4].Tag = (object) taskMilestonePair.TaskGuid;
              gvItem2.ForeColor = Color.Gray;
            }
            else
            {
              gvItem2.Text = string.Empty;
              if (taskMilestonePair.TaskType == MilestoneTaskType.MilestoneWorkflow)
                gvItem2.SubItems.Add((object) taskLog.TaskGUID);
              else
                gvItem2.SubItems.Add((object) string.Empty);
              gvItem2.SubItems.Add((object) taskLog.TaskName);
              if (taskMilestonePair.TaskType == MilestoneTaskType.MilestoneWorkflow)
                gvItem2.SubItems[2].ImageIndex = 0;
              gvItem2.SubItems.Add((object) this.getWorkflowTaskGroupName(taskLog.TaskParentID));
              gvItem2.SubItems[3].Tag = (object) (int) taskMilestonePair.TaskType;
              gvItem2.SubItems.Add((object) str);
              gvItem2.SubItems[4].Tag = (object) taskLog.TaskGUID;
            }
            this.emListViewTasks.Items.Add(gvItem2);
          }
        }
      }
      if (this.fieldsForRule != null)
      {
        gvItem1 = (GVItem) null;
        string empty = string.Empty;
        for (int index = 0; index < this.fieldsForRule.Length; ++index)
        {
          string str = "";
          if (this.container.MSNameTable.ContainsKey((object) this.fieldsForRule[index].MilestoneID))
          {
            str = this.container.MSNameTable[(object) this.fieldsForRule[index].MilestoneID].ToString();
            if (this.container.ArchiveMilestone.Contains((object) str))
              str += " (Archived)";
          }
          bool flag = false;
          string fieldId = this.fieldsForRule[index].FieldID;
          if (fieldId.StartsWith("BE") || fieldId.StartsWith("CE") || fieldId.StartsWith("BR") || fieldId.StartsWith("CR") && !fieldId.StartsWith("CRED") || fieldId.StartsWith("DD"))
            flag = true;
          if (flag || EncompassFields.IsReportable(fieldId, this.fieldSettings))
            this.emListViewFields.Items.Add(new GVItem(this.fieldsForRule[index].FieldID)
            {
              SubItems = {
                (object) this.getFieldDescription(fieldId),
                (object) str
              }
            });
        }
      }
      if (this.advCodesForRule != null)
      {
        for (int index = 0; index < this.advCodesForRule.Length; ++index)
        {
          string str = "";
          if (this.container.MSNameTable.ContainsKey((object) this.advCodesForRule[index].MilestoneID))
          {
            str = this.container.MSNameTable[(object) this.advCodesForRule[index].MilestoneID].ToString();
            if (this.container.ArchiveMilestone.Contains((object) str))
              str += " (Archived)";
          }
          GVItem gvItem3 = new GVItem(this.getSourceCodeDescription(this.advCodesForRule[index].SourceCode));
          gvItem3.SubItems.Add((object) str);
          gvItem3.SubItems.Add((object) this.advCodesForRule[index].SourceCode);
          this.emListViewCode.Items.Add(gvItem3);
          gvItem3.Tag = (object) this.advCodesForRule[index].SourceCode;
        }
      }
      this.ruleCondForm.SetCondition((BizRuleInfo) this.milestoneRule);
      this.textBoxName.Text = this.milestoneRule.RuleName;
      if (this.milestoneRule.CommentsTxt.Contains("\n") && !this.milestoneRule.CommentsTxt.Contains(Environment.NewLine))
        this.commentsTxt.Text = this.milestoneRule.CommentsTxt.Replace("\n", Environment.NewLine);
      else
        this.commentsTxt.Text = this.milestoneRule.CommentsTxt;
    }

    private void addDocBtn_Click(object sender, EventArgs e)
    {
      Hashtable existDocs = new Hashtable();
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      for (int nItemIndex = 0; nItemIndex < this.emListViewDocs.Items.Count; ++nItemIndex)
      {
        string lower = this.emListViewDocs.Items[nItemIndex].SubItems[0].Text.Trim().ToLower();
        if (this.emListViewDocs.Items[nItemIndex].SubItems[1].Text.IndexOf("(Archived)") <= -1 && !existDocs.ContainsKey((object) lower))
          existDocs.Add((object) lower, (object) lower);
      }
      using (AddDocs addDocs = new AddDocs(this.session, this.msList, existDocs, this.container))
      {
        if (addDocs.ShowDialog((IWin32Window) this) == DialogResult.OK)
        {
          DocMilestonePair[] docsForRule = addDocs.DocsForRule;
          if (docsForRule.Length != 0)
          {
            this.emListViewDocs.BeginUpdate();
            for (int index = 0; index < docsForRule.Length; ++index)
            {
              string str = !this.container.ArchiveMilestone.Contains((object) docsForRule[index].MilestoneID) ? docsForRule[index].MilestoneID : docsForRule[index].MilestoneID + " (Archived)";
              this.emListViewDocs.Items.Add(new GVItem(docsForRule[index].DocGuid)
              {
                SubItems = {
                  (object) str,
                  addDocs.NeedAttachment ? (object) "Yes" : (object) "No"
                },
                Tag = (object) docsForRule[index].MilestoneID
              });
            }
            this.emListViewDocs.EndUpdate();
          }
        }
        addDocs.Dispose();
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

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (this.textBoxName.Text == "")
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please enter a rule name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        BizRuleInfo[] allRules = ((BpmManager) this.session.BPM.GetBpmManager(BpmCategory.MilestoneRules)).GetAllRules();
        for (int index = 0; index < allRules.Length; ++index)
        {
          if (string.Compare(this.textBoxName.Text.Trim(), allRules[index].RuleName, StringComparison.OrdinalIgnoreCase) == 0)
          {
            bool flag = false;
            if (this.milestoneRule == null)
              flag = true;
            else if (this.milestoneRule.RuleID != allRules[index].RuleID || this.milestoneRule.RuleID == 0)
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
        if (this.emListViewDocs.Items.Count == 0 && this.emListViewFields.Items.Count == 0 && this.emListViewTasks.Items.Count == 0 && this.emListViewCode.Items.Count == 0)
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, "Please add a document, task, field or advanced code element.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          for (int nItemIndex = 0; nItemIndex < this.emListViewDocs.Items.Count; ++nItemIndex)
          {
            if (this.emListViewDocs.Items[nItemIndex].SubItems[1].Text == "")
            {
              int num4 = (int) Utils.Dialog((IWin32Window) this, "You need to select a milestone for document '" + this.emListViewDocs.Items[nItemIndex].SubItems[1].Text + "'.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              this.tabControl1.SelectedPage = this.tabPageEx1;
              this.emListViewDocs.Items[nItemIndex].Selected = true;
              return;
            }
          }
          for (int nItemIndex = 0; nItemIndex < this.emListViewTasks.Items.Count; ++nItemIndex)
          {
            if (this.emListViewTasks.Items[nItemIndex].SubItems[4].Text == "")
            {
              int num5 = (int) Utils.Dialog((IWin32Window) this, "You need to select a milestone for task " + this.emListViewTasks.Items[nItemIndex].SubItems[2].Text + ".", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              this.tabControl1.SelectedPage = this.tabPageEx2;
              this.emListViewTasks.Items[nItemIndex].Selected = true;
              return;
            }
          }
          for (int nItemIndex = 0; nItemIndex < this.emListViewFields.Items.Count; ++nItemIndex)
          {
            if (this.emListViewFields.Items[nItemIndex].SubItems[2].Text == "")
            {
              int num6 = (int) Utils.Dialog((IWin32Window) this, "You need to select a milestone for field " + this.emListViewFields.Items[nItemIndex].Text + ".", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              this.tabControl1.SelectedPage = this.tabPageEx3;
              this.emListViewFields.Items[nItemIndex].Selected = true;
              return;
            }
          }
          for (int nItemIndex = 0; nItemIndex < this.emListViewCode.Items.Count; ++nItemIndex)
          {
            if (this.emListViewCode.Items[nItemIndex].SubItems[1].Text == "")
            {
              int num7 = (int) Utils.Dialog((IWin32Window) this, "You need to select a milestone for all of your advanced coding rules.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
          int num8 = this.emListViewTasks.Items.Count - 1;
          arrayList1.Clear();
          for (int nItemIndex = num8; nItemIndex >= 0; --nItemIndex)
          {
            string str = this.emListViewTasks.Items[nItemIndex].SubItems[2].Text.Trim().ToLower() + "|" + this.emListViewTasks.Items[nItemIndex].SubItems[4].Text.Trim().ToLower();
            if (arrayList1.Contains((object) str))
              this.emListViewTasks.Items.Remove(this.emListViewTasks.Items[nItemIndex]);
            else
              arrayList1.Add((object) str);
          }
          int num9 = this.emListViewFields.Items.Count - 1;
          arrayList1.Clear();
          for (int nItemIndex = num9; nItemIndex >= 0; --nItemIndex)
          {
            string str = "|" + this.emListViewFields.Items[nItemIndex].Text.Trim().ToLower() + "|" + this.emListViewFields.Items[nItemIndex].SubItems[2].Text.Trim().ToLower() + "|";
            if (arrayList1.Contains((object) str))
              this.emListViewFields.Items.Remove(this.emListViewFields.Items[nItemIndex]);
            else
              arrayList1.Add((object) str);
          }
          ArrayList arrayList2 = new ArrayList();
          for (int nItemIndex = 0; nItemIndex < this.emListViewDocs.Items.Count; ++nItemIndex)
          {
            string name = this.emListViewDocs.Items[nItemIndex].Text.Trim();
            string key = this.emListViewDocs.Items[nItemIndex].SubItems[1].Text.Replace(" (Archived)", "");
            if (this.container.MSGUIDTable.ContainsKey((object) key))
            {
              DocumentTemplate byName = this.container.DocsInforInSettings.GetByName(name);
              if (byName != null)
              {
                bool attachedRequired = this.emListViewDocs.Items[nItemIndex].SubItems[2].Text == "Yes";
                arrayList2.Add((object) new DocMilestonePair(byName.Guid, this.container.MSGUIDTable[(object) key].ToString(), attachedRequired));
              }
            }
          }
          this.docsForRule = (DocMilestonePair[]) arrayList2.ToArray(typeof (DocMilestonePair));
          ArrayList arrayList3 = new ArrayList();
          for (int nItemIndex = 0; nItemIndex < this.emListViewTasks.Items.Count; ++nItemIndex)
          {
            string taskGuid = this.emListViewTasks.Items[nItemIndex].SubItems[4].Tag.ToString();
            string key = this.emListViewTasks.Items[nItemIndex].SubItems[4].Text.Replace(" (Archived)", "");
            MilestoneTaskType tag = (MilestoneTaskType) this.emListViewTasks.Items[nItemIndex].SubItems[3].Tag;
            if (this.container.MSGUIDTable.ContainsKey((object) key))
            {
              ConditionRuleDialog.TaskLog taskLog = (ConditionRuleDialog.TaskLog) null;
              if (this.existsMilestoneTaskInSettings(taskGuid, tag))
                taskLog = this.getMilestoneTaskInSettings(taskGuid, tag);
              if (taskLog != null || tag != MilestoneTaskType.MilestoneRequired)
                arrayList3.Add((object) new TaskMilestonePair(taskLog?.TaskGUID ?? taskGuid, this.container.MSGUIDTable[(object) key].ToString(), true, tag, new DateTime?(DateTime.Now)));
            }
          }
          this.tasksForRule = (TaskMilestonePair[]) arrayList3.ToArray(typeof (TaskMilestonePair));
          ArrayList arrayList4 = new ArrayList();
          for (int nItemIndex = 0; nItemIndex < this.emListViewFields.Items.Count; ++nItemIndex)
          {
            string fieldID = this.emListViewFields.Items[nItemIndex].Text.Trim();
            string key = this.emListViewFields.Items[nItemIndex].SubItems[2].Text.Replace(" (Archived)", "");
            if (this.container.MSGUIDTable.ContainsKey((object) key))
              arrayList4.Add((object) new FieldMilestonePair(fieldID, this.container.MSGUIDTable[(object) key].ToString()));
          }
          this.fieldsForRule = (FieldMilestonePair[]) arrayList4.ToArray(typeof (FieldMilestonePair));
          ArrayList arrayList5 = new ArrayList();
          for (int nItemIndex = 0; nItemIndex < this.emListViewCode.Items.Count; ++nItemIndex)
          {
            string text = this.emListViewCode.Items[nItemIndex].SubItems[2].Text;
            string key = this.emListViewCode.Items[nItemIndex].SubItems[1].Text.Replace(" (Archived)", "");
            if (this.container.MSGUIDTable.ContainsKey((object) key))
              arrayList5.Add((object) new AdvancedCodeMilestonePair(this.container.MSGUIDTable[(object) key].ToString(), text));
          }
          this.advCodesForRule = (AdvancedCodeMilestonePair[]) arrayList5.ToArray(typeof (AdvancedCodeMilestonePair));
          this.milestoneRule = this.milestoneRule == null ? new MilestoneRuleInfo(this.textBoxName.Text.Trim()) : new MilestoneRuleInfo(this.milestoneRule.RuleID, this.textBoxName.Text.Trim());
          this.milestoneRule.Docs = this.docsForRule;
          this.milestoneRule.Fields = this.fieldsForRule;
          this.milestoneRule.Tasks = this.tasksForRule;
          this.milestoneRule.AdvancedCodes = this.advCodesForRule;
          this.ruleCondForm.ApplyCondition((BizRuleInfo) this.milestoneRule);
          this.milestoneRule.Condition2 = this.channelControl.ChannelValue;
          this.milestoneRule.CommentsTxt = this.commentsTxt.Text;
          this.DialogResult = DialogResult.OK;
        }
      }
    }

    private void addFieldBtn_Click(object sender, EventArgs e)
    {
      using (AddFields addFields = new AddFields(this.session, (string) null, AddFieldOptions.AllowCustomFields | AddFieldOptions.AllowVirtualFields | AddFieldOptions.AllowButtons))
      {
        addFields.SetMilestoneSelection(this.msList);
        addFields.OnAddMoreButtonClick += new EventHandler(this.addFieldDlg_OnAddMoreButtonClick);
        if (addFields.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.addFields(addFields.SelectedFieldIDs, addFields.SelectedRuleValue);
      }
    }

    private void addFields(string[] ids, string milestoneName)
    {
      if (ids.Length == 0)
        return;
      string empty = string.Empty;
      string str = !this.container.ArchiveMilestone.Contains((object) milestoneName) ? milestoneName : milestoneName + " (Archived)";
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
              (object) str
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
        ruleFindFieldDialog.SetMilestoneSelection(this.msList);
        if (ruleFindFieldDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        Cursor.Current = Cursors.WaitCursor;
        this.emListViewFields.BeginUpdate();
        for (int index = 0; index < ruleFindFieldDialog.SelectedRequiredFields.Length; ++index)
        {
          if (!(ruleFindFieldDialog.SelectedRequiredFields[index] == "") && !arrayList.Contains((object) ruleFindFieldDialog.SelectedRequiredFields[index]) && EncompassFields.IsReportable(ruleFindFieldDialog.SelectedRequiredFields[index], this.fieldSettings))
          {
            string str = !this.container.ArchiveMilestone.Contains((object) ruleFindFieldDialog.SelectedRuleValue) ? ruleFindFieldDialog.SelectedRuleValue : ruleFindFieldDialog.SelectedRuleValue + " (Archived)";
            this.emListViewFields.Items.Add(new GVItem(ruleFindFieldDialog.SelectedRequiredFields[index])
            {
              SubItems = {
                (object) this.getFieldDescription(ruleFindFieldDialog.SelectedRequiredFields[index]),
                (object) str
              },
              Tag = (object) ""
            });
          }
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
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, nameof (ConditionRuleDialog));
    }

    private void btnAddTask_Click(object sender, EventArgs e)
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      using (AddTasks addTasks = new AddTasks(this.session, this.msList, this.container))
      {
        if (addTasks.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        TaskMilestonePair[] tasksForRule = addTasks.TasksForRule;
        if (tasksForRule.Length == 0)
          return;
        this.emListViewTasks.BeginUpdate();
        foreach (TaskMilestonePair taskMilestonePair in tasksForRule)
        {
          if (this.existsMilestoneTaskInSettings(taskMilestonePair.TaskGuid, taskMilestonePair.TaskType))
          {
            ConditionRuleDialog.TaskLog milestoneTaskInSettings = this.getMilestoneTaskInSettings(taskMilestonePair.TaskGuid, taskMilestonePair.TaskType);
            string str = !this.container.ArchiveMilestone.Contains((object) taskMilestonePair.MilestoneID) ? taskMilestonePair.MilestoneID : taskMilestonePair.MilestoneID + " (Archived)";
            GVItem gvItem = new GVItem(string.Empty);
            if (taskMilestonePair.TaskType == MilestoneTaskType.MilestoneWorkflow)
              gvItem.SubItems.Add((object) milestoneTaskInSettings.TaskGUID);
            else
              gvItem.SubItems.Add((object) string.Empty);
            gvItem.SubItems.Add((object) milestoneTaskInSettings.TaskName);
            if (taskMilestonePair.TaskType == MilestoneTaskType.MilestoneWorkflow)
              gvItem.SubItems[2].ImageIndex = 0;
            gvItem.SubItems.Add((object) this.getWorkflowTaskGroupName(milestoneTaskInSettings.TaskParentID));
            gvItem.SubItems[3].Tag = (object) (int) taskMilestonePair.TaskType;
            gvItem.SubItems.Add((object) str);
            gvItem.Tag = (object) taskMilestonePair.MilestoneID;
            gvItem.SubItems[4].Tag = (object) taskMilestonePair.TaskGuid;
            this.emListViewTasks.Items.Add(gvItem);
          }
          else
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The task '" + taskMilestonePair.TaskGuid + "' is invalid. It might be removed by another user.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
        this.emListViewTasks.EndUpdate();
      }
    }

    private void addCodeBtn_Click(object sender, EventArgs e)
    {
      using (AddAdvCode addAdvCode = new AddAdvCode())
      {
        if (addAdvCode.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        GVItem gvItem = new GVItem(this.getSourceCodeDescription(addAdvCode.SourceCode));
        gvItem.SubItems.Add((object) "");
        gvItem.SubItems.Add((object) addAdvCode.SourceCode);
        this.emListViewCode.Items.Add(gvItem);
        gvItem.Tag = (object) addAdvCode.SourceCode;
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
      using (AddAdvCode addAdvCode = new AddAdvCode(string.Concat(listItem.Tag)))
      {
        if (addAdvCode.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        listItem.SubItems[0].Text = this.getSourceCodeDescription(addAdvCode.SourceCode);
        listItem.SubItems[2].Text = addAdvCode.SourceCode;
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
        this.milestoneEditorOpening(e);
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

    private void gridView_EditorClosing(object source, GVSubItemEditingEventArgs e)
    {
      this.milestoneEditorClosing(e);
    }

    private void milestoneEditorOpening(GVSubItemEditingEventArgs e)
    {
      ComboBox editorControl = (ComboBox) e.EditorControl;
      editorControl.Items.Clear();
      editorControl.DropDownStyle = ComboBoxStyle.DropDownList;
      editorControl.Items.AddRange((object[]) this.msList);
      editorControl.Text = e.SubItem.Text;
    }

    private void milestoneEditorClosing(GVSubItemEditingEventArgs e)
    {
      ComboBox editorControl = (ComboBox) e.EditorControl;
      e.SubItem.Text = editorControl.SelectedText;
    }

    private void emListViewTasks_EditorOpening(object source, GVSubItemEditingEventArgs e)
    {
      if (e.SubItem.Index != 4)
        return;
      this.milestoneEditorOpening(e);
    }

    private void emListViewFields_EditorOpening(object source, GVSubItemEditingEventArgs e)
    {
      if (e.SubItem.Index != 2)
        return;
      this.milestoneEditorOpening(e);
    }

    private void emListViewCode_EditorOpening(object source, GVSubItemEditingEventArgs e)
    {
      if (e.SubItem.Index != 1)
        return;
      this.milestoneEditorOpening(e);
    }

    private void ConditionRuleDialog_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\u001B')
        return;
      this.Close();
    }

    private void emListViewTasks_SubItemEnter(object source, GVSubItemEventArgs e)
    {
      if (e.SubItem.Index != 0 || e.SubItem.ImageIndex != 1)
        return;
      this.toolTip1.Show("The selected Workflow Task no longer exists.", (IWin32Window) this.emListViewTasks);
    }

    private void emListViewTasks_SubItemLeave(object source, GVSubItemEventArgs e)
    {
      if (e.SubItem.Index != 0)
        return;
      this.toolTip1.RemoveAll();
    }

    private bool existsMilestoneTaskInSettings(string taskGuid, MilestoneTaskType taskType)
    {
      bool flag = false;
      switch (taskType)
      {
        case MilestoneTaskType.MilestoneRequired:
          flag = this.container.TasksInforInSettings.ContainsKey((object) taskGuid);
          break;
        case MilestoneTaskType.MilestoneWorkflow:
          flag = this.container.WorkflowTaskTemplatesInfoInSettings.Value.ContainsKey((object) taskGuid);
          break;
      }
      return flag;
    }

    private ConditionRuleDialog.TaskLog getMilestoneTaskInSettings(
      string taskGuid,
      MilestoneTaskType taskType)
    {
      ConditionRuleDialog.TaskLog milestoneTaskInSettings = new ConditionRuleDialog.TaskLog();
      switch (taskType)
      {
        case MilestoneTaskType.MilestoneRequired:
          MilestoneTaskDefinition tasksInforInSetting = (MilestoneTaskDefinition) this.container.TasksInforInSettings[(object) taskGuid];
          milestoneTaskInSettings.TaskGUID = tasksInforInSetting.TaskGUID;
          milestoneTaskInSettings.TaskName = tasksInforInSetting.TaskName;
          break;
        case MilestoneTaskType.MilestoneWorkflow:
          TaskTemplate taskTemplate = (TaskTemplate) this.container.WorkflowTaskTemplatesInfoInSettings.Value[(object) taskGuid];
          milestoneTaskInSettings.TaskGUID = taskTemplate.TypeID;
          milestoneTaskInSettings.TaskName = taskTemplate.Name;
          milestoneTaskInSettings.TaskParentID = taskTemplate.TaskGroupTemplateId;
          break;
      }
      return milestoneTaskInSettings;
    }

    private string getWorkflowTaskGroupName(string groupID)
    {
      string empty = string.Empty;
      return string.IsNullOrWhiteSpace(groupID) ? empty : ((TaskGroupTemplate) this.container.WorkflowTaskGroupTemplatesInfoInSettings.Value[(object) groupID])?.Name;
    }

    private class TaskLog
    {
      public string TaskGUID;
      public string TaskName;
      public string TaskParentID;
    }
  }
}
