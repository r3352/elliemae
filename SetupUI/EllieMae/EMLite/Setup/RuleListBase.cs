// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.RuleListBase
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.BusinessRuleBase;
using EllieMae.EMLite.UI;
using EllieMae.EMLite.Workflow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class RuleListBase : UserControl
  {
    private const string className = "RuleListBase";
    private static readonly string sw = Tracing.SwOutsideLoan;
    public const int STATUSCOLUMN = 3;
    protected Sessions.Session session;
    private string header;
    protected ListViewSortManager sortMngr;
    protected object objectRuleManager;
    protected List<EllieMae.EMLite.Workflow.Milestone> msList;
    private BpmManager bpmManager;
    private RoleInfo[] roleInfos;
    private IContainer components;
    private ContextMenu ctxMenuLstView1;
    protected Button activeBtn;
    protected Button deactiveBtn;
    protected GridView listViewRule;
    protected MenuItem menuItemNew;
    protected MenuItem menuItemDuplicate;
    protected MenuItem menuItemDelete;
    protected MenuItem menuItemEdit;
    protected MenuItem menuItemActive;
    protected MenuItem menuItemDeactive;
    protected MenuItem menuItemImport;
    protected MenuItem menuItemExport;
    protected GroupContainer gContainer;
    private ToolTip toolTip1;
    private StandardIconButton stdIconBtnDelete;
    private StandardIconButton stdIconBtnEdit;
    private StandardIconButton stdIconBtnDuplicate;
    private StandardIconButton stdIconBtnNew;
    private StandardIconButton stdIconBtnImport;
    private VerticalSeparator verticalSeparator1;
    private StandardIconButton stdIconBtnExport;
    private SaveFileDialog exportFileDialog;
    private VerticalSeparator verticalSeparator2;
    private string[] preConfiguredRuleNames = new string[1]
    {
      "Manner in Which Title will be Held"
    };
    private string data = "";
    private string fileName = string.Empty;

    public RuleListBase()
      : this(Session.DefaultInstance, "", false)
    {
      if (this.DesignMode)
        return;
      int num = (int) MessageBox.Show("Do not use constructor RuleListBase().");
    }

    protected RuleListBase(Sessions.Session session, string header, bool allowMultiSelect)
    {
      this.session = session;
      this.header = header;
      this.InitializeComponent();
      this.listViewRule.AllowMultiselect = allowMultiSelect;
      this.msList = new List<EllieMae.EMLite.Workflow.Milestone>(((BpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).GetAllMilestones());
      this.roleInfos = ((WorkflowManager) this.session.BPM.GetBpmManager(BpmCategory.Workflow)).GetAllRoleFunctions();
      this.listViewRule.SelectedIndexChanged += new EventHandler(this.listViewRule_SelectedIndexChanged);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    protected void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      this.ctxMenuLstView1 = new ContextMenu();
      this.menuItemNew = new MenuItem();
      this.menuItemImport = new MenuItem();
      this.menuItemExport = new MenuItem();
      this.menuItemDuplicate = new MenuItem();
      this.menuItemEdit = new MenuItem();
      this.menuItemActive = new MenuItem();
      this.menuItemDeactive = new MenuItem();
      this.menuItemDelete = new MenuItem();
      this.activeBtn = new Button();
      this.deactiveBtn = new Button();
      this.listViewRule = new GridView();
      this.gContainer = new GroupContainer();
      this.stdIconBtnExport = new StandardIconButton();
      this.stdIconBtnImport = new StandardIconButton();
      this.verticalSeparator1 = new VerticalSeparator();
      this.stdIconBtnEdit = new StandardIconButton();
      this.stdIconBtnDuplicate = new StandardIconButton();
      this.stdIconBtnNew = new StandardIconButton();
      this.stdIconBtnDelete = new StandardIconButton();
      this.toolTip1 = new ToolTip(this.components);
      this.exportFileDialog = new SaveFileDialog();
      this.verticalSeparator2 = new VerticalSeparator();
      this.gContainer.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnExport).BeginInit();
      ((ISupportInitialize) this.stdIconBtnImport).BeginInit();
      ((ISupportInitialize) this.stdIconBtnEdit).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDuplicate).BeginInit();
      ((ISupportInitialize) this.stdIconBtnNew).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDelete).BeginInit();
      this.SuspendLayout();
      this.ctxMenuLstView1.MenuItems.AddRange(new MenuItem[8]
      {
        this.menuItemNew,
        this.menuItemImport,
        this.menuItemExport,
        this.menuItemDuplicate,
        this.menuItemEdit,
        this.menuItemActive,
        this.menuItemDeactive,
        this.menuItemDelete
      });
      this.ctxMenuLstView1.Popup += new EventHandler(this.ctxMenuLstView1_Popup);
      this.menuItemNew.Index = 0;
      this.menuItemNew.Text = "New";
      this.menuItemNew.Click += new EventHandler(this.menuItem_Click);
      this.menuItemImport.Index = 1;
      this.menuItemImport.Text = "Import";
      this.menuItemImport.Click += new EventHandler(this.menuItem_Click);
      this.menuItemExport.Index = 2;
      this.menuItemExport.Text = "Export";
      this.menuItemExport.Click += new EventHandler(this.menuItem_Click);
      this.menuItemDuplicate.Index = 3;
      this.menuItemDuplicate.Text = "Duplicate";
      this.menuItemDuplicate.Click += new EventHandler(this.menuItem_Click);
      this.menuItemEdit.Index = 4;
      this.menuItemEdit.Text = "Edit";
      this.menuItemEdit.Click += new EventHandler(this.menuItem_Click);
      this.menuItemActive.Index = 5;
      this.menuItemActive.Text = "Activate";
      this.menuItemActive.Click += new EventHandler(this.menuItem_Click);
      this.menuItemDeactive.Index = 6;
      this.menuItemDeactive.Text = "Deactivate";
      this.menuItemDeactive.Click += new EventHandler(this.menuItem_Click);
      this.menuItemDelete.Index = 7;
      this.menuItemDelete.Text = "Delete";
      this.menuItemDelete.Click += new EventHandler(this.menuItem_Click);
      this.activeBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.activeBtn.BackColor = SystemColors.Control;
      this.activeBtn.Location = new Point(404, 2);
      this.activeBtn.Name = "activeBtn";
      this.activeBtn.Size = new Size(66, 22);
      this.activeBtn.TabIndex = 2;
      this.activeBtn.Text = "&Activate";
      this.activeBtn.UseVisualStyleBackColor = true;
      this.activeBtn.Click += new EventHandler(this.activeBtn_Click);
      this.deactiveBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.deactiveBtn.BackColor = SystemColors.Control;
      this.deactiveBtn.Location = new Point(472, 2);
      this.deactiveBtn.Name = "deactiveBtn";
      this.deactiveBtn.Size = new Size(80, 22);
      this.deactiveBtn.TabIndex = 2;
      this.deactiveBtn.Text = "Deac&tivate";
      this.deactiveBtn.UseVisualStyleBackColor = true;
      this.deactiveBtn.Click += new EventHandler(this.deactiveBtn_Click);
      this.listViewRule.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "colName";
      gvColumn1.Text = "Name";
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "colChannel";
      gvColumn2.Text = "Channel";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "colCondition";
      gvColumn3.Text = "Condition";
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "colStatus";
      gvColumn4.Text = "Status";
      gvColumn4.Width = 100;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "colLastModifiedBy";
      gvColumn5.Text = "Last Modified By";
      gvColumn5.Width = 100;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "colLastModified";
      gvColumn6.SortMethod = GVSortMethod.DateTime;
      gvColumn6.Text = "Last Modified Date & Time";
      gvColumn6.Width = 150;
      this.listViewRule.Columns.AddRange(new GVColumn[6]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6
      });
      this.listViewRule.ContextMenu = this.ctxMenuLstView1;
      this.listViewRule.Dock = DockStyle.Fill;
      this.listViewRule.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewRule.Location = new Point(1, 26);
      this.listViewRule.Name = "listViewRule";
      this.listViewRule.Size = new Size(555, 378);
      this.listViewRule.TabIndex = 7;
      this.listViewRule.SelectedIndexChanged += new EventHandler(this.listViewRule_SelectedIndexChanged_1);
      this.listViewRule.ItemDoubleClick += new GVItemEventHandler(this.listViewRule_ItemDoubleClick);
      this.listViewRule.Resize += new EventHandler(this.listView_Resize);
      this.gContainer.Controls.Add((Control) this.verticalSeparator2);
      this.gContainer.Controls.Add((Control) this.stdIconBtnExport);
      this.gContainer.Controls.Add((Control) this.stdIconBtnImport);
      this.gContainer.Controls.Add((Control) this.verticalSeparator1);
      this.gContainer.Controls.Add((Control) this.stdIconBtnEdit);
      this.gContainer.Controls.Add((Control) this.stdIconBtnDuplicate);
      this.gContainer.Controls.Add((Control) this.stdIconBtnNew);
      this.gContainer.Controls.Add((Control) this.stdIconBtnDelete);
      this.gContainer.Controls.Add((Control) this.listViewRule);
      this.gContainer.Controls.Add((Control) this.activeBtn);
      this.gContainer.Controls.Add((Control) this.deactiveBtn);
      this.gContainer.Dock = DockStyle.Fill;
      this.gContainer.HeaderForeColor = SystemColors.ControlText;
      this.gContainer.Location = new Point(0, 0);
      this.gContainer.Name = "gContainer";
      this.gContainer.Size = new Size(557, 405);
      this.gContainer.TabIndex = 8;
      this.stdIconBtnExport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnExport.BackColor = Color.Transparent;
      this.stdIconBtnExport.Enabled = false;
      this.stdIconBtnExport.Location = new Point(355, 5);
      this.stdIconBtnExport.MouseDownImage = (Image) null;
      this.stdIconBtnExport.Name = "stdIconBtnExport";
      this.stdIconBtnExport.Size = new Size(16, 17);
      this.stdIconBtnExport.StandardButtonType = StandardIconButton.ButtonType.ExportDataToFileButton;
      this.stdIconBtnExport.TabIndex = 13;
      this.stdIconBtnExport.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnExport, "Export");
      this.stdIconBtnExport.Click += new EventHandler(this.exportBtn_Click);
      this.stdIconBtnImport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnImport.BackColor = Color.Transparent;
      this.stdIconBtnImport.Location = new Point(377, 5);
      this.stdIconBtnImport.MouseDownImage = (Image) null;
      this.stdIconBtnImport.Name = "stdIconBtnImport";
      this.stdIconBtnImport.Size = new Size(16, 16);
      this.stdIconBtnImport.StandardButtonType = StandardIconButton.ButtonType.ImportLoanButton;
      this.stdIconBtnImport.TabIndex = 13;
      this.stdIconBtnImport.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnImport, "Import");
      this.stdIconBtnImport.Click += new EventHandler(this.importBtn_Click);
      this.verticalSeparator1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.verticalSeparator1.Location = new Point(398, 5);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 12;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.stdIconBtnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnEdit.BackColor = Color.Transparent;
      this.stdIconBtnEdit.Location = new Point(307, 5);
      this.stdIconBtnEdit.MouseDownImage = (Image) null;
      this.stdIconBtnEdit.Name = "stdIconBtnEdit";
      this.stdIconBtnEdit.Size = new Size(16, 17);
      this.stdIconBtnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdIconBtnEdit.TabIndex = 11;
      this.stdIconBtnEdit.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnEdit, "Edit");
      this.stdIconBtnEdit.Click += new EventHandler(this.editBtn_Click);
      this.stdIconBtnDuplicate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDuplicate.BackColor = Color.Transparent;
      this.stdIconBtnDuplicate.Location = new Point(286, 5);
      this.stdIconBtnDuplicate.MouseDownImage = (Image) null;
      this.stdIconBtnDuplicate.Name = "stdIconBtnDuplicate";
      this.stdIconBtnDuplicate.Size = new Size(16, 17);
      this.stdIconBtnDuplicate.StandardButtonType = StandardIconButton.ButtonType.DuplicateButton;
      this.stdIconBtnDuplicate.TabIndex = 10;
      this.stdIconBtnDuplicate.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDuplicate, "Duplicate");
      this.stdIconBtnDuplicate.Click += new EventHandler(this.dupBtn_Click);
      this.stdIconBtnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnNew.BackColor = Color.Transparent;
      this.stdIconBtnNew.Location = new Point(266, 5);
      this.stdIconBtnNew.MouseDownImage = (Image) null;
      this.stdIconBtnNew.Name = "stdIconBtnNew";
      this.stdIconBtnNew.Size = new Size(16, 17);
      this.stdIconBtnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnNew.TabIndex = 9;
      this.stdIconBtnNew.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnNew, "New");
      this.stdIconBtnNew.Click += new EventHandler(this.newBtn_Click);
      this.stdIconBtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDelete.BackColor = Color.Transparent;
      this.stdIconBtnDelete.Location = new Point(328, 5);
      this.stdIconBtnDelete.MouseDownImage = (Image) null;
      this.stdIconBtnDelete.Name = "stdIconBtnDelete";
      this.stdIconBtnDelete.Size = new Size(16, 17);
      this.stdIconBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDelete.TabIndex = 8;
      this.stdIconBtnDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDelete, "Delete");
      this.stdIconBtnDelete.Click += new EventHandler(this.deleteBtn_Click);
      this.exportFileDialog.FileOk += new CancelEventHandler(this.exportFileDialog_FileOk);
      this.verticalSeparator2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.verticalSeparator2.Location = new Point(349, 5);
      this.verticalSeparator2.MaximumSize = new Size(2, 16);
      this.verticalSeparator2.MinimumSize = new Size(2, 16);
      this.verticalSeparator2.Name = "verticalSeparator2";
      this.verticalSeparator2.Size = new Size(2, 16);
      this.verticalSeparator2.TabIndex = 14;
      this.verticalSeparator2.Text = "verticalSeparator2";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gContainer);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (RuleListBase);
      this.Size = new Size(557, 405);
      this.gContainer.ResumeLayout(false);
      ((ISupportInitialize) this.stdIconBtnExport).EndInit();
      ((ISupportInitialize) this.stdIconBtnImport).EndInit();
      ((ISupportInitialize) this.stdIconBtnEdit).EndInit();
      ((ISupportInitialize) this.stdIconBtnDuplicate).EndInit();
      ((ISupportInitialize) this.stdIconBtnNew).EndInit();
      ((ISupportInitialize) this.stdIconBtnDelete).EndInit();
      this.ResumeLayout(false);
    }

    public string[] SelectedRules
    {
      get
      {
        if (this.listViewRule.SelectedItems == null)
          return (string[]) null;
        List<string> stringList = new List<string>();
        foreach (GVItem selectedItem in this.listViewRule.SelectedItems)
          stringList.Add(selectedItem.Text);
        return stringList.ToArray();
      }
      set
      {
        if (value == null || value.Length == 0)
          return;
        List<string> stringList = new List<string>((IEnumerable<string>) value);
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.listViewRule.Items)
          gvItem.Selected = stringList.Contains(gvItem.Text);
      }
    }

    protected void setHeader()
    {
      this.gContainer.Text = this.header + " (" + (object) this.listViewRule.Items.Count + ")";
    }

    protected virtual void initForm()
    {
      this.bpmManager = (BpmManager) this.objectRuleManager;
      this.listViewRule.Items.Clear();
      BizRuleInfo[] bizRuleInfoArray = (BizRuleInfo[]) null;
      try
      {
        if (this.objectRuleManager is MilestoneRulesBpmManager)
          bizRuleInfoArray = ((BpmManager) this.objectRuleManager).GetRulesFromDatabase(true);
        if (this.objectRuleManager is LoanActionCompletionRulesBpmManager)
          bizRuleInfoArray = ((BpmManager) this.objectRuleManager).GetRulesFromDatabase(true);
        else if (this.objectRuleManager is InputFormsBpmManager)
          bizRuleInfoArray = ((BpmManager) this.objectRuleManager).GetAllRulesFromDatabase();
        else if (this.objectRuleManager is FieldAccessBpmManager)
          bizRuleInfoArray = ((BpmManager) this.objectRuleManager).GetAllRulesFromDatabase();
        else if (this.objectRuleManager is LoanAccessBpmManager)
          bizRuleInfoArray = ((BpmManager) this.objectRuleManager).GetAllRulesFromDatabase();
        else if (this.objectRuleManager is LoanActionAccessBpmManager)
          bizRuleInfoArray = ((BpmManager) this.objectRuleManager).GetAllRulesFromDatabase();
        else if (this.objectRuleManager is FieldRulesBpmManager)
          bizRuleInfoArray = ((BpmManager) this.objectRuleManager).GetRulesFromDatabase(true);
        else if (this.objectRuleManager is TriggersBpmManager)
          bizRuleInfoArray = ((BpmManager) this.objectRuleManager).GetAllRulesFromDatabase();
        else if (this.objectRuleManager is PrintFormsBpmManager)
          bizRuleInfoArray = ((BpmManager) this.objectRuleManager).GetAllRulesFromDatabase();
        else if (this.objectRuleManager is PrintSelectionBpmManager)
          bizRuleInfoArray = ((BpmManager) this.objectRuleManager).GetAllRulesFromDatabase();
        else if (this.objectRuleManager is AutomatedConditionBpmManager)
          bizRuleInfoArray = ((BpmManager) this.objectRuleManager).GetAllRulesFromDatabase();
        else if (this.objectRuleManager is MilestoneTemplatesBpmManager)
          bizRuleInfoArray = ((BpmManager) this.objectRuleManager).GetAllRulesFromDatabase();
        else if (this.objectRuleManager is AutoLockExclusionRulesBpmManager)
          bizRuleInfoArray = ((BpmManager) this.objectRuleManager).GetAllRulesFromDatabase();
        else if (this.objectRuleManager is AutomatedEnhancedConditionBpmManager)
          bizRuleInfoArray = ((BpmManager) this.objectRuleManager).GetAllRulesFromDatabase();
      }
      catch (Exception ex)
      {
        Tracing.Log(RuleListBase.sw, TraceLevel.Error, nameof (RuleListBase), "initForm: Can't access business rule. Error: " + ex.Message);
        return;
      }
      if (bizRuleInfoArray != null)
      {
        for (int index = 0; index < bizRuleInfoArray.Length; ++index)
        {
          if (bizRuleInfoArray[index].Inactive)
            this.addViewItemToList(bizRuleInfoArray[index].RuleName, this.BuildChannelString(bizRuleInfoArray[index]), this.BuildConditionString(bizRuleInfoArray[index]), BizRule.RuleStatusStrings[0], bizRuleInfoArray[index].LastModifiedByUserInfo, bizRuleInfoArray[index].LastModTime.ToString("MM/dd/yyyy hh:mm tt"), (object) bizRuleInfoArray[index]);
          else
            this.addViewItemToList(bizRuleInfoArray[index].RuleName, this.BuildChannelString(bizRuleInfoArray[index]), this.BuildConditionString(bizRuleInfoArray[index]), BizRule.RuleStatusStrings[1], bizRuleInfoArray[index].LastModifiedByUserInfo, bizRuleInfoArray[index].LastModTime.ToString("MM/dd/yyyy hh:mm tt"), (object) bizRuleInfoArray[index]);
        }
      }
      BizRuleInfo[] rules;
      if (this.objectRuleManager is MilestoneRulesBpmManager)
        rules = ((BpmManager) this.objectRuleManager).GetRules(false);
      else if (this.objectRuleManager is LoanActionCompletionRulesBpmManager)
      {
        rules = ((BpmManager) this.objectRuleManager).GetRules(false);
      }
      else
      {
        if (!(this.objectRuleManager is FieldRulesBpmManager))
          return;
        rules = ((BpmManager) this.objectRuleManager).GetRules(false);
      }
      if (rules == null)
        return;
      for (int index = 0; index < rules.Length; ++index)
      {
        if (rules[index].Inactive)
          this.addViewItemToList(rules[index].RuleName, this.BuildChannelString(rules[index]), this.BuildConditionString(rules[index]), BizRule.RuleStatusStrings[0], rules[index].LastModifiedByUserInfo, rules[index].LastModTime.ToString("MM/dd/yyyy hh:mm tt"), (object) rules[index]);
        else
          this.addViewItemToList(rules[index].RuleName, this.BuildChannelString(rules[index]), this.BuildConditionString(rules[index]), BizRule.RuleStatusStrings[1], rules[index].LastModifiedByUserInfo, rules[index].LastModTime.ToString("MM/dd/yyyy hh:mm tt"), (object) rules[index]);
      }
    }

    public virtual void newBtn_Click(object sender, EventArgs e) => this.setHeader();

    protected virtual void ctxMenuLstView1_Popup(object sender, EventArgs e)
    {
      if (this.listViewRule.SelectedItems.Count == 0)
      {
        this.menuItemDuplicate.Visible = false;
        this.menuItemEdit.Visible = false;
        this.menuItemActive.Visible = false;
        this.menuItemDeactive.Visible = false;
        this.menuItemDelete.Visible = false;
      }
      else
      {
        this.menuItemDuplicate.Visible = true;
        this.menuItemEdit.Visible = true;
        this.menuItemActive.Visible = this.activeBtn.Enabled;
        this.menuItemDeactive.Visible = this.deactiveBtn.Enabled;
        if (this.bpmManager is FieldRulesBpmManager && ((IEnumerable<string>) this.preConfiguredRuleNames).Contains<string>(((BizRuleInfo) this.listViewRule.SelectedItems[0].Tag).RuleName))
          this.menuItemDelete.Visible = false;
        else
          this.menuItemDelete.Visible = true;
      }
    }

    protected virtual void menuItem_Click(object sender, EventArgs e)
    {
      string text = ((MenuItem) sender).Text;
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(text))
      {
        case 999227792:
          if (!(text == "Activate"))
            break;
          this.activeBtn_Click((object) null, (EventArgs) null);
          break;
        case 1463683828:
          if (!(text == "Import"))
            break;
          this.importBtn_Click((object) null, (EventArgs) null);
          break;
        case 1469573738:
          if (!(text == "Delete"))
            break;
          this.deleteBtn_Click((object) null, (EventArgs) null);
          break;
        case 2144318963:
          if (!(text == "Deactivate"))
            break;
          this.deactiveBtn_Click((object) null, (EventArgs) null);
          break;
        case 2334404017:
          if (!(text == "New"))
            break;
          this.newBtn_Click((object) null, (EventArgs) null);
          break;
        case 3267849393:
          if (!(text == "Edit"))
            break;
          this.editBtn_Click((object) null, (EventArgs) null);
          break;
        case 3898821075:
          if (!(text == "Export"))
            break;
          this.exportBtn_Click((object) null, (EventArgs) null);
          break;
        case 4231201590:
          if (!(text == "Duplicate"))
            break;
          this.dupBtn_Click((object) null, (EventArgs) null);
          break;
      }
    }

    private void editSelectedItem()
    {
      if (this.listViewRule.SelectedItems.Count != 0)
        return;
      int num = (int) Utils.Dialog((IWin32Window) this, "You have to select a Business Rule first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }

    protected virtual void listViewRule_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.editSelectedItem();
    }

    protected virtual void editBtn_Click(object sender, EventArgs e) => this.editSelectedItem();

    protected virtual void deleteBtn_Click(object sender, EventArgs e)
    {
      if (this.listViewRule.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You have to select a Business Rule first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        BizRuleInfo tag = (BizRuleInfo) this.listViewRule.SelectedItems[0].Tag;
        if (tag == null)
        {
          Tracing.Log(RuleListBase.sw, TraceLevel.Error, nameof (RuleListBase), "deleteBtn_Click: Can't cast to business rule.");
        }
        else
        {
          if (Utils.Dialog((IWin32Window) this, "Are you sure you want to delete the selected Business Rule?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
          {
            try
            {
              this.bpmManager.DeleteRule(tag.RuleID, forceToPrimaryDb: true);
            }
            catch (Exception ex)
            {
              Tracing.Log(RuleListBase.sw, TraceLevel.Error, nameof (RuleListBase), "deleteBtn_Click: Can't delete selected business rule. Error: " + ex.Message);
            }
            this.removeViewItemFromList(this.listViewRule.SelectedItems[0].Index);
          }
          this.setHeader();
        }
      }
    }

    protected virtual string GetCurrentRuleStatus(
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2,
      int ruleID)
    {
      int num = (int) this.bpmManager.ActivateRule(ruleID, forceToPrimaryDb: true);
      return BizRule.RuleStatusStrings[1];
    }

    protected virtual void activeBtn_Click(object sender, EventArgs e)
    {
      if (this.listViewRule.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You have to select a Business Rule first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        BizRuleInfo tag = (BizRuleInfo) this.listViewRule.SelectedItems[0].Tag;
        if (tag == null)
        {
          Tracing.Log(RuleListBase.sw, TraceLevel.Error, nameof (RuleListBase), "activeBtn_Click: Can't cast to business rule.");
        }
        else
        {
          try
          {
            if (this.listViewRule.SelectedItems[0].SubItems[3].Text == BizRule.RuleStatusStrings[0])
            {
              BizRule.ActivationReturnCode activationReturnCode = this.bpmManager.ActivateRule(tag.RuleID, forceToPrimaryDb: true);
              if (this.bpmManager is FieldRulesBpmManager && activationReturnCode == BizRule.ActivationReturnCode.InconsistentFieldRules)
                ((FieldRulesBpmManager) this.bpmManager).GetInconsistentFields(tag.RuleID);
              if (activationReturnCode != BizRule.ActivationReturnCode.Succeed && activationReturnCode != BizRule.ActivationReturnCode.NoOp)
              {
                int num2 = (int) Utils.Dialog((IWin32Window) this, "This business rule can not be activated. Error code: " + (object) (int) activationReturnCode, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
              }
            }
            this.listViewRule.SelectedItems[0].SubItems[3].Text = BizRule.RuleStatusStrings[1];
            this.listViewRule.SelectedItems[0].SubItems[4].Text = this.session.UserID + " (" + this.session.UserInfo.FullName + ")";
            this.listViewRule.SelectedItems[0].SubItems[5].Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
            this.deactiveBtn.Enabled = true;
            this.activeBtn.Enabled = false;
          }
          catch (Exception ex)
          {
            Tracing.Log(RuleListBase.sw, TraceLevel.Error, nameof (RuleListBase), "activeBtn_Click: Can't activate business rule. Error: " + ex.Message);
          }
        }
      }
    }

    protected virtual void deactiveBtn_Click(object sender, EventArgs e)
    {
      if (this.listViewRule.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You have to select a Business Rule first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        BizRuleInfo tag = (BizRuleInfo) this.listViewRule.SelectedItems[0].Tag;
        if (tag == null)
        {
          Tracing.Log(RuleListBase.sw, TraceLevel.Error, nameof (RuleListBase), "deactiveBtn_Click: Can't cast to business rule.");
        }
        else
        {
          try
          {
            if (this.listViewRule.SelectedItems[0].SubItems[3].Text == BizRule.RuleStatusStrings[1])
            {
              int num2 = (int) this.bpmManager.DeactivateRule(tag.RuleID, forceToPrimaryDb: true);
            }
            this.listViewRule.SelectedItems[0].SubItems[3].Text = BizRule.RuleStatusStrings[0];
            this.listViewRule.SelectedItems[0].SubItems[4].Text = this.session.UserID + " (" + this.session.UserInfo.FullName + ")";
            this.listViewRule.SelectedItems[0].SubItems[5].Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
            this.deactiveBtn.Enabled = false;
            this.activeBtn.Enabled = true;
          }
          catch (Exception ex)
          {
            Tracing.Log(RuleListBase.sw, TraceLevel.Error, nameof (RuleListBase), "deactiveBtn_Click: Can't deactivate business rule. Error: " + ex.Message);
          }
        }
      }
    }

    protected virtual void dupBtn_Click(object sender, EventArgs e)
    {
      if (this.listViewRule.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You have to select a Business Rule first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
        this.setHeader();
    }

    protected virtual void listView_Resize(object sender, EventArgs e)
    {
      int width = this.listViewRule.ClientSize.Width;
      if (width <= 0)
        return;
      this.listViewRule.Columns.GetColumnByName("colName").Width = (int) ((double) width * 0.2);
      this.listViewRule.Columns.GetColumnByName("colChannel").Width = (int) ((double) width * 0.1);
      this.listViewRule.Columns.GetColumnByName("colCondition").Width = (int) ((double) width * 0.4);
      this.listViewRule.Columns.GetColumnByName("colStatus").Width = (int) ((double) width * 0.1);
      this.listViewRule.Columns.GetColumnByName("colLastModifiedBy").Width = (int) ((double) width * 0.08);
      this.listViewRule.Columns.GetColumnByName("colLastModified").Width = (int) ((double) width * 0.11);
    }

    protected virtual void listView_DoubleClick(object sender, EventArgs e)
    {
      this.editBtn_Click((object) null, (EventArgs) null);
    }

    protected virtual void addViewItemToList(
      string column1,
      string column2,
      string column3,
      string column4,
      string column5,
      string column6,
      object o)
    {
      this.addViewItemToList(column1, column2, column3, column4, column5, column6, o, false, false);
    }

    protected virtual void addViewItemToList(
      string column1,
      string column2,
      string column3,
      string column4,
      string column5,
      string column6,
      object o,
      bool selected,
      bool ensureVisible)
    {
      GVItem gvItem1 = new GVItem(column1);
      if ((column2 ?? "") == string.Empty)
        gvItem1.SubItems.Add((object) "No Channel");
      else
        gvItem1.SubItems.Add((object) column2);
      if ((column3 ?? "") == string.Empty)
        gvItem1.SubItems.Add((object) "No Condition");
      else
        gvItem1.SubItems.Add((object) column3);
      gvItem1.SubItems.Add((object) column4);
      gvItem1.SubItems.Add((object) column5);
      gvItem1.SubItems.Add((object) column6);
      gvItem1.Tag = o;
      if (selected)
      {
        foreach (GVItem gvItem2 in (IEnumerable<GVItem>) this.listViewRule.Items)
          gvItem2.Selected = false;
        gvItem1.Selected = true;
      }
      this.listViewRule.Items.Add(gvItem1);
    }

    protected virtual void editViewItemOnList(
      string column1,
      string column2,
      string column3,
      string column4,
      object o)
    {
      this.listViewRule.SelectedItems[0].Text = column1;
      this.listViewRule.SelectedItems[0].SubItems[1].Text = !((column2 ?? "") == string.Empty) ? column2 : "No Channel";
      this.listViewRule.SelectedItems[0].SubItems[2].Text = !((column3 ?? "") == string.Empty) ? column3 : "No Condition";
      this.listViewRule.SelectedItems[0].SubItems[3].Text = column4;
      this.listViewRule.SelectedItems[0].SubItems[4].Text = this.session.UserID + " (" + this.session.UserInfo.FullName + ")";
      this.listViewRule.SelectedItems[0].SubItems[5].Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
      this.listViewRule.SelectedItems[0].Tag = o;
    }

    protected virtual void removeViewItemFromList(int selected)
    {
      this.listViewRule.Items.Remove(this.listViewRule.Items[selected]);
      if (this.listViewRule.Items.Count == 0)
        return;
      if (selected + 1 > this.listViewRule.Items.Count)
        this.listViewRule.Items[this.listViewRule.Items.Count - 1].Selected = true;
      else
        this.listViewRule.Items[selected].Selected = true;
    }

    protected virtual string BuildConditionString(BizRuleInfo bizInfo)
    {
      string str1 = "";
      switch (bizInfo.Condition)
      {
        case BizRule.Condition.LoanPurpose:
          string str2 = "Loan Purpose is ";
          int index1 = Utils.ParseInt((object) bizInfo.ConditionState);
          str1 = index1 <= BizRule.LoanPurposeStrings.Length - 1 ? str2 + BizRule.LoanPurposeStrings[index1] : str2 + "Null";
          break;
        case BizRule.Condition.LoanType:
          string str3 = "Loan Type is ";
          int index2 = Utils.ParseInt((object) bizInfo.ConditionState);
          str1 = index2 <= BizRule.LoanTypeStrings.Length - 1 ? str3 + BizRule.LoanTypeStrings[index2] : str3 + "Null";
          break;
        case BizRule.Condition.LoanStatus:
          string str4 = "Loan Status is ";
          int index3 = Utils.ParseInt((object) bizInfo.ConditionState);
          str1 = index3 + 1 <= BizRule.LoanStatusStrings.Length ? str4 + BizRule.LoanStatusStrings[index3] : str4 + "Null";
          break;
        case BizRule.Condition.CurrentLoanAssocMS:
          string str5 = "Unknown role ";
          for (int index4 = 0; index4 < this.roleInfos.Length; ++index4)
          {
            if (string.Compare(this.roleInfos[index4].RoleID.ToString(), bizInfo.ConditionState2, true) == 0)
            {
              str5 = this.roleInfos[index4].RoleName;
              break;
            }
          }
          str1 = str5 + " Assigned on " + this.getMilestoneName(bizInfo.ConditionState);
          break;
        case BizRule.Condition.RateLock:
          string str6 = "Rate is ";
          int index5 = Utils.ParseInt((object) bizInfo.ConditionState);
          str1 = index5 <= BizRule.LockDateStrings.Length - 1 ? str6 + BizRule.LockDateStrings[index5] : str6 + BizRule.LockDateStrings[0];
          break;
        case BizRule.Condition.PropertyState:
          string str7 = "Property State is ";
          USPS.StateCode key = (USPS.StateCode) this.toEnum(Utils.ParseInt((object) bizInfo.ConditionState).ToString(), typeof (USPS.StateCode));
          str1 = !USPS.StateNames.ContainsKey((object) key) ? str7 + USPS.StateNames[(object) USPS.StateCode.Unknown] : str7 + USPS.StateNames[(object) key];
          break;
        case BizRule.Condition.LoanDocType:
          string str8 = "Loan Doc Type is ";
          int index6 = Utils.ParseInt((object) bizInfo.ConditionState);
          str1 = index6 + 1 > LoanDocTypeMap.Descriptions.Length || index6 == 0 ? str8 + "Empty" : str8 + LoanDocTypeMap.Descriptions[index6];
          break;
        case BizRule.Condition.FinishedMilestone:
          str1 = "Completed milestone is " + this.getMilestoneName(bizInfo.ConditionState);
          break;
        case BizRule.Condition.AdvancedCoding:
          str1 = bizInfo.ConditionState.Replace(Environment.NewLine, " ");
          if (str1.Length > 100)
          {
            str1 = str1.Substring(0, 100) + "...";
            break;
          }
          break;
        case BizRule.Condition.LoanProgram:
          str1 = "Loan Program is " + bizInfo.ConditionState;
          break;
        case BizRule.Condition.PropertyType:
          str1 = "Property Type is " + BizRule.PropertyTypeStrings[Utils.ParseInt((object) bizInfo.ConditionState)];
          break;
        case BizRule.Condition.Occupancy:
          str1 = "Occupancy is " + BizRule.PropertyOccupancyStrings[Utils.ParseInt((object) bizInfo.ConditionState)];
          break;
        case BizRule.Condition.TPOActions:
          TriggerActivationNameProvider activationNameProvider = new TriggerActivationNameProvider();
          if (bizInfo.RuleType == BizRuleType.LoanActionAccess || bizInfo.RuleType == BizRuleType.LoanActionCompletionRules || bizInfo.RuleType == BizRuleType.FieldAccess || bizInfo.RuleType == BizRuleType.LoanAccess || bizInfo.RuleType == BizRuleType.FieldRules)
          {
            string[] strArray1 = bizInfo.ConditionState.Split('|');
            string[] strArray2 = strArray1[1].Split(',');
            string str9 = strArray2.Length != 1 ? (!(strArray1[0].Trim().ToLower() == "all") ? "TPO actions are any of " : "TPO actions are all of ") : "TPO action is ";
            string empty = string.Empty;
            for (int index7 = 0; index7 < strArray2.Length; ++index7)
            {
              if (empty != string.Empty)
                empty += ", ";
              empty += activationNameProvider.GetDescriptionFromActivationType((TriggerActivationType) Enum.Parse(typeof (TriggerActivationType), strArray2[index7].Trim()));
            }
            str1 = str9 + empty;
            break;
          }
          TriggerActivationType triggerActivationType = (TriggerActivationType) Enum.Parse(typeof (TriggerActivationType), bizInfo.ConditionState);
          str1 = "TPO Action is " + activationNameProvider.GetDescriptionFromActivationType(triggerActivationType);
          break;
      }
      return str1;
    }

    protected virtual string BuildChannelString(BizRuleInfo bizInfo)
    {
      string empty = string.Empty;
      if (bizInfo.Condition2 == "0,1,2,3,4")
        return "All Channels";
      if (bizInfo.Condition2 == "1,2,3,4")
        return "Any Selected Channel";
      if (bizInfo.Condition2 != string.Empty)
      {
        string[] strArray = bizInfo.Condition2.Split(',');
        for (int index = 0; index < strArray.Length; ++index)
        {
          if (Utils.ParseInt((object) strArray[index]) >= 0)
          {
            if (empty != string.Empty)
              empty += ", ";
            empty += BizRule.ChannelStatusString[Utils.ParseInt((object) strArray[index])];
          }
        }
      }
      return empty;
    }

    private string getMilestoneName(string id)
    {
      string milestoneName = string.Empty;
      EllieMae.EMLite.Workflow.Milestone milestone = this.msList.Find((Predicate<EllieMae.EMLite.Workflow.Milestone>) (item => item.MilestoneID == id));
      if (milestone != null)
        milestoneName = milestone.Name;
      return milestoneName;
    }

    protected virtual void listViewRule_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.stdIconBtnDuplicate.Enabled = this.stdIconBtnEdit.Enabled = this.stdIconBtnDelete.Enabled = this.listViewRule.SelectedItems.Count > 0;
      if (this.listViewRule.SelectedItems.Count == 0)
      {
        this.activeBtn.Enabled = this.menuItemActive.Enabled = this.deactiveBtn.Enabled = this.menuItemDeactive.Enabled = false;
        this.stdIconBtnNew.Enabled = this.menuItemNew.Enabled = true;
      }
      else
      {
        if (this.listViewRule.SelectedItems[0].SubItems[3].Text == BizRule.RuleStatusStrings[1])
        {
          this.deactiveBtn.Enabled = this.menuItemDeactive.Enabled = true;
          this.activeBtn.Enabled = this.menuItemActive.Enabled = false;
        }
        else
        {
          this.deactiveBtn.Enabled = this.menuItemDeactive.Enabled = false;
          this.activeBtn.Enabled = this.menuItemActive.Enabled = true;
        }
        if (this.listViewRule.SelectedItems.Count > 1)
        {
          this.stdIconBtnNew.Enabled = this.menuItemNew.Enabled = false;
          this.stdIconBtnDuplicate.Enabled = this.menuItemDuplicate.Enabled = this.stdIconBtnEdit.Enabled = this.menuItemEdit.Enabled = this.stdIconBtnDelete.Enabled = this.menuItemDelete.Enabled = false;
          this.activeBtn.Enabled = this.menuItemActive.Enabled = this.menuItemDeactive.Enabled = this.deactiveBtn.Enabled = false;
        }
        else
          this.stdIconBtnNew.Enabled = this.menuItemNew.Enabled = true;
        if (!(this.bpmManager is FieldRulesBpmManager) || !((IEnumerable<string>) this.preConfiguredRuleNames).Contains<string>(((BizRuleInfo) this.listViewRule.SelectedItems[0].Tag).RuleName))
          return;
        this.stdIconBtnDelete.Enabled = false;
      }
    }

    private object toEnum(string value, System.Type enumType)
    {
      try
      {
        return Enum.Parse(enumType, value, true);
      }
      catch
      {
        return (object) 0;
      }
    }

    protected virtual string FindDuplicateName(string sourceName, BizRuleInfo[] busiRules)
    {
      string lower = sourceName.Trim().ToLower();
      string str = sourceName;
      if (lower.StartsWith("copy of "))
      {
        int num = lower.IndexOf("copy of ");
        str = sourceName.Substring(num + 8);
      }
      else if (lower.StartsWith("copy (") && lower.IndexOf(") of ") > -1)
      {
        int num = lower.IndexOf(") of ");
        str = sourceName.Substring(num + 5);
      }
      int num1 = 0;
      string duplicateName = string.Empty;
      string empty = string.Empty;
      while (duplicateName == string.Empty)
      {
        string strA = num1 != 0 ? "Copy (" + num1.ToString() + ") of " + str : "Copy of " + str;
        bool flag = false;
        for (int index = 0; index < busiRules.Length; ++index)
        {
          if (string.Compare(strA, busiRules[index].RuleName, true) == 0)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          duplicateName = strA;
        ++num1;
      }
      if (duplicateName.Length > 64)
        duplicateName = duplicateName.Substring(0, 64);
      return duplicateName;
    }

    private void exportBtn_Click(object sender, EventArgs e)
    {
      try
      {
        this.data = XElement.Load((XmlReader) JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(BusinessRuleRestApiHelper.ExportRule(this.GetExportURL())), new XmlDictionaryReaderQuotas())).Value;
        XDocument doc = XDocument.Parse(this.data);
        if (this.GetBizRuleType() == BizRuleType.MilestoneRules)
          this.populateWorkflowTasksToXML(doc);
        this.fileName = "BR_" + doc.Root.Attribute((XName) "RuleType").Value + "_" + doc.Root.Attribute((XName) "Name").Value;
        foreach (char ch in new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars()))
          this.fileName = this.fileName.Replace(ch.ToString(), "");
        if (File.Exists(SystemSettings.TempFolderRoot + this.fileName))
          File.Delete(SystemSettings.TempFolderRoot + this.fileName);
        doc.Save(SystemSettings.TempFolderRoot + this.fileName + ".Xml");
        this.exportFileDialog.Filter = "zip files (*.zip)|*.zip";
        this.exportFileDialog.FileName = this.fileName;
        this.exportFileDialog.FilterIndex = 1;
        int num = (int) this.exportFileDialog.ShowDialog();
      }
      catch (AggregateException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Flatten().InnerExceptions[0].Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void populateWorkflowTasksToXML(XDocument doc)
    {
      IEnumerable<XElement> source = doc.Descendants((XName) "MilestoneRequirements").Descendants<XElement>((XName) "RequiredTasks").Descendants<XElement>((XName) "AffectedTask").Descendants<XElement>((XName) "XRef").Where<XElement>((Func<XElement, bool>) (wft => wft.Attribute((XName) "EntityType").Value == "WorkflowTasks"));
      if (!source.Any<XElement>())
        return;
      foreach (XElement xelement in source)
      {
        string key = xelement.Attribute((XName) "EntityID").Value;
        if (this.session.SessionObjects.CachedWorkflowTaskTemplates.Contains((object) key))
          xelement.Attribute((XName) "EntityUID").Value = ((TaskTemplate) this.session.SessionObjects.CachedWorkflowTaskTemplates[(object) key]).Name;
        else
          xelement.Attribute((XName) "EntityUID").Value = "";
      }
    }

    protected virtual void importBtn_Click(object sender, EventArgs e)
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Filter = "zip files (*.zip)|*.zip";
      string str1 = Guid.NewGuid().ToString();
      if (DialogResult.OK != openFileDialog.ShowDialog())
        return;
      string str2 = SystemSettings.TempFolderRoot + "BusinessRule_" + str1 + "\\";
      if (Directory.Exists(str2))
        Directory.Delete(str2, true);
      FileCompressor.Instance.Unzip(openFileDialog.FileName, str2);
      string[] files = Directory.GetFiles(str2);
      using (ImportBusinessRuleDialog businessRuleDialog = new ImportBusinessRuleDialog(this.session, this.GetImportURL(), files[0], this.GetBizRuleType()))
      {
        int num = (int) businessRuleDialog.ShowDialog();
        this.initForm();
        this.setHeader();
        this.listViewRule_SelectedIndexChanged((object) null, (EventArgs) null);
      }
    }

    protected virtual string GetExportURL() => throw new NotImplementedException();

    protected virtual string GetImportURL() => throw new NotImplementedException();

    protected virtual BizRuleType GetBizRuleType() => throw new NotImplementedException();

    private void exportFileDialog_FileOk(object sender, CancelEventArgs e)
    {
      FileCompressor.Instance.ZipFile(SystemSettings.TempFolderRoot + this.fileName + ".Xml", this.exportFileDialog.FileName);
    }

    private void listViewRule_SelectedIndexChanged_1(object sender, EventArgs e)
    {
      if (this.listViewRule.SelectedItems.Count == 0)
        this.stdIconBtnExport.Enabled = false;
      else
        this.stdIconBtnExport.Enabled = true;
    }

    public void HideImportExport()
    {
      this.stdIconBtnExport.Visible = false;
      this.stdIconBtnImport.Visible = false;
    }
  }
}
