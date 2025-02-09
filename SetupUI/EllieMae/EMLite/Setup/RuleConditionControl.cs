// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.RuleConditionControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.BusinessRules;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Compiler;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class RuleConditionControl : UserControl
  {
    private const string className = "RuleConditionControl";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private Sessions.Session session;
    private ComboBox comboType;
    private IContainer components;
    private BpmCategory ruleCategory;
    private Hashtable msNameTable;
    private ArrayList bmpMap;
    private ArrayList msNameList;
    private System.Windows.Forms.RadioButton radioGeneral;
    private System.Windows.Forms.RadioButton radioCondition;
    private ComboBox comboCondition2;
    private Hashtable msGUIDTable;
    private System.Windows.Forms.Label labelAssigned;
    private RoleInfo[] roleInfos;
    private System.Windows.Forms.TextBox textConditionCode;
    private System.Windows.Forms.Label labelIs;
    private StandardIconButton btnSelect;
    private ResourceManager UIresources;
    private BizRule.Condition ruleCondition;
    private ToolTip toolTip1;
    private System.Windows.Forms.Label lblWorksheet;
    private ComboBox comboCondition;
    private System.Windows.Forms.Panel panelConditions;
    private CheckedListBox chkdListTPOActions;
    private string advancedCodeXml;
    private bool ddmSetting;

    public event EventHandler ChangesMadeToConditions;

    public List<string> SelectedTPORuleCondition
    {
      get
      {
        if (this.ruleCondition != BizRule.Condition.TPOActions)
          return new List<string>();
        List<string> tpoRuleCondition = new List<string>();
        if (this.ruleCategory == BpmCategory.LoanActionCompletionRules || this.ruleCategory == BpmCategory.FieldRules || this.ruleCategory == BpmCategory.FieldAccess)
        {
          for (int index = 0; index < this.chkdListTPOActions.Items.Count; ++index)
          {
            if (this.chkdListTPOActions.GetItemCheckState(index) == CheckState.Checked)
              tpoRuleCondition.Add(this.chkdListTPOActions.GetItemText(this.chkdListTPOActions.Items[index]).ToString());
          }
        }
        else if (this.comboCondition.SelectedValue != null)
          tpoRuleCondition.Add(this.comboCondition.Text.ToString());
        return tpoRuleCondition;
      }
    }

    public RuleConditionControl(Sessions.Session session)
    {
      this.Dock = DockStyle.Fill;
      this.InitializeComponent();
      this.session = session;
      this.UIresources = new ResourceManager(typeof (RuleConditionControl));
      this.msGUIDTable = ((MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).GetCompleteMilestoneGUID();
      this.comboCondition2.Items.Clear();
      this.roleInfos = ((WorkflowManager) this.session.BPM.GetBpmManager(BpmCategory.Workflow)).GetAllRoleFunctions();
      if (this.roleInfos != null)
      {
        for (int index = 0; index < this.roleInfos.Length; ++index)
          this.comboCondition2.Items.Add((object) this.roleInfos[index].RoleName);
      }
      this.radioGeneral.Checked = true;
    }

    public RuleConditionControl(Sessions.Session session, bool isDDM)
      : this(session)
    {
      this.ddmSetting = isDDM;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public BizRule.Condition GetRuleCondition()
    {
      return this.radioGeneral.Checked ? BizRule.Condition.Null : this.ruleCondition;
    }

    public void SetCondition(BizRuleInfo rule)
    {
      this.setConditionType(rule.Condition);
      this.setConditionValue(rule.ConditionState);
      this.setConditionValue2(rule.ConditionState2);
      this.advancedCodeXml = rule.AdvancedCodeXML;
    }

    public void ApplyCondition(BizRuleInfo rule)
    {
      rule.Condition = this.getConditionType();
      rule.ConditionState = this.getConditionValue();
      rule.ConditionState2 = this.getConditionValue2();
      if (rule.Condition == BizRule.Condition.AdvancedCoding)
        rule.AdvancedCodeXML = this.advancedCodeXml;
      else
        rule.AdvancedCodeXML = (string) null;
    }

    public void DisableControls()
    {
      this.textConditionCode.Enabled = false;
      this.btnSelect.Enabled = false;
      this.comboType.Enabled = false;
      this.comboCondition.Enabled = false;
      this.comboCondition2.Enabled = false;
      this.radioCondition.Enabled = false;
      this.radioGeneral.Enabled = false;
    }

    public bool IsGeneralRule => this.radioGeneral.Checked;

    private string getConditionValue()
    {
      switch (this.ruleCondition)
      {
        case BizRule.Condition.LoanPurpose:
        case BizRule.Condition.LoanType:
        case BizRule.Condition.LoanStatus:
        case BizRule.Condition.RateLock:
        case BizRule.Condition.PropertyType:
        case BizRule.Condition.Occupancy:
          return this.comboCondition.Items.Count > 0 && this.bmpMap != null ? this.bmpMap[this.comboCondition.SelectedIndex].ToString() : "";
        case BizRule.Condition.CurrentLoanAssocMS:
        case BizRule.Condition.FinishedMilestone:
          string strB = this.comboCondition.Text;
          if (strB.IndexOf("(Archived)") > -1)
            strB = strB.Replace(" (Archived)", "");
          foreach (DictionaryEntry dictionaryEntry in this.msNameTable)
          {
            if (string.Compare(dictionaryEntry.Value.ToString(), strB, true) == 0)
              return dictionaryEntry.Key.ToString();
          }
          return "";
        case BizRule.Condition.PropertyState:
          return this.comboCondition.SelectedIndex.ToString();
        case BizRule.Condition.LoanDocType:
          return this.comboCondition.SelectedIndex == 0 ? "100" : this.comboCondition.SelectedIndex.ToString();
        case BizRule.Condition.AdvancedCoding:
          return this.textConditionCode.Text;
        case BizRule.Condition.LoanProgram:
          return this.textConditionCode.Text;
        case BizRule.Condition.TPOActions:
          string empty = string.Empty;
          string conditionValue;
          if (this.isTPOActionCheckBoxListDisplay(this.ruleCategory))
          {
            TriggerActivationNameProvider activationNameProvider = new TriggerActivationNameProvider();
            foreach (object checkedItem in this.chkdListTPOActions.CheckedItems)
            {
              if (empty != string.Empty)
                empty += ",";
              empty += activationNameProvider.GetTriggerActivationTypeFromDescription(checkedItem.ToString()).ToString();
            }
            conditionValue = this.comboCondition.SelectedIndex != 0 ? "ALL | " + empty : "ANY | " + empty;
          }
          else
            conditionValue = this.comboCondition.SelectedValue.ToString();
          return conditionValue;
        default:
          return string.Empty;
      }
    }

    private void setConditionValue(string conditionValue)
    {
      switch (this.ruleCondition)
      {
        case BizRule.Condition.LoanPurpose:
        case BizRule.Condition.LoanType:
        case BizRule.Condition.LoanStatus:
        case BizRule.Condition.RateLock:
        case BizRule.Condition.PropertyType:
        case BizRule.Condition.Occupancy:
          if (this.comboCondition.Items.Count <= 0 || this.bmpMap == null)
            break;
          for (int index = 0; index < this.bmpMap.Count; ++index)
          {
            if (conditionValue == this.bmpMap[index].ToString())
            {
              this.comboCondition.SelectedIndex = index;
              break;
            }
          }
          break;
        case BizRule.Condition.CurrentLoanAssocMS:
        case BizRule.Condition.FinishedMilestone:
          IDictionaryEnumerator enumerator = this.msNameTable.GetEnumerator();
          try
          {
            while (enumerator.MoveNext())
            {
              DictionaryEntry current = (DictionaryEntry) enumerator.Current;
              if (current.Key.ToString() == conditionValue)
              {
                bool flag = false;
                if (this.comboCondition.Items.Contains((object) current.Value.ToString()))
                  flag = true;
                if (this.comboCondition.Items.Contains((object) (current.Value.ToString() + " (Archived)")))
                  flag = true;
                if (flag)
                {
                  for (int index = 0; index < this.comboCondition.Items.Count; ++index)
                  {
                    string str = this.comboCondition.Items[index].ToString();
                    if (str.IndexOf("(Archived)") > -1)
                      str = str.Replace(" (Archived)", "");
                    if (str == current.Value.ToString())
                    {
                      this.comboCondition.SelectedIndex = index;
                      break;
                    }
                  }
                }
                else
                {
                  this.comboCondition.Items.Add((object) (current.Value.ToString() + " (Archived)"));
                  this.comboCondition.Text = current.Value.ToString() + " (Archived)";
                }
              }
            }
            break;
          }
          finally
          {
            if (enumerator is IDisposable disposable)
              disposable.Dispose();
          }
        case BizRule.Condition.PropertyState:
        case BizRule.Condition.LoanDocType:
          int num = Utils.ParseInt((object) conditionValue);
          if (num == 100)
            num = 0;
          if (num + 1 > this.comboCondition.Items.Count)
          {
            this.comboCondition.SelectedIndex = 0;
            break;
          }
          this.comboCondition.SelectedIndex = num;
          break;
        case BizRule.Condition.AdvancedCoding:
          this.textConditionCode.Text = conditionValue;
          break;
        case BizRule.Condition.LoanProgram:
          this.textConditionCode.Text = conditionValue;
          break;
        case BizRule.Condition.TPOActions:
          if (this.isTPOActionCheckBoxListDisplay(this.ruleCategory))
          {
            this.toggleTPOActionCheckboxlist(true);
            string[] strArray = conditionValue.Split('|');
            if (strArray[0].Trim().ToLower() == "all")
              this.comboCondition.SelectedIndex = 1;
            else
              this.comboCondition.SelectedIndex = 0;
            TriggerActivationNameProvider activationNameProvider = new TriggerActivationNameProvider();
            string str = strArray[1];
            char[] chArray = new char[1]{ ',' };
            foreach (object obj in str.Split(chArray))
              this.chkdListTPOActions.SetItemChecked(this.chkdListTPOActions.Items.IndexOf((object) activationNameProvider.GetDescriptionFromActivationType((TriggerActivationType) Enum.Parse(typeof (TriggerActivationType), obj.ToString().Trim()))), true);
            break;
          }
          this.comboCondition.SelectedValue = (object) (TriggerActivationType) Enum.Parse(typeof (TriggerActivationType), conditionValue);
          break;
        default:
          this.toggleTPOActionCheckboxlist(false);
          this.comboCondition.Text = "";
          break;
      }
    }

    private string getConditionValue2()
    {
      if (this.ruleCondition == BizRule.Condition.CurrentLoanAssocMS)
      {
        for (int index = 0; index < this.roleInfos.Length; ++index)
        {
          if (string.Compare(this.roleInfos[index].RoleName, this.comboCondition2.Text, true) == 0)
            return this.roleInfos[index].RoleID.ToString();
        }
      }
      return string.Empty;
    }

    private void setConditionValue2(string conditionValue2)
    {
      string strB = string.Empty;
      if (this.ruleCondition == BizRule.Condition.CurrentLoanAssocMS)
      {
        for (int index = 0; index < this.roleInfos.Length; ++index)
        {
          if (string.Compare(this.roleInfos[index].RoleID.ToString(), conditionValue2, true) == 0)
            strB = this.roleInfos[index].RoleName;
        }
      }
      if (!(strB != string.Empty))
        return;
      for (int index = 0; index < this.comboCondition2.Items.Count; ++index)
      {
        if (string.Compare(this.comboCondition2.Items[index].ToString(), strB, true) == 0)
        {
          this.comboCondition2.SelectedIndex = index;
          break;
        }
      }
    }

    private void setConditionType(BizRule.Condition cond)
    {
      this.ruleCondition = cond;
      if (cond == BizRule.Condition.Null)
      {
        this.radioGeneral.Checked = true;
        this.comboType.SelectedIndex = -1;
      }
      else
      {
        this.radioCondition.Checked = true;
        ClientCommonUtils.PopulateDropdown(this.comboType, (object) BizRule.ConditionStrings[(int) cond], false);
      }
    }

    private BizRule.Condition getConditionType()
    {
      return this.IsGeneralRule ? BizRule.Condition.Null : this.ruleCondition;
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.comboType = new ComboBox();
      this.radioGeneral = new System.Windows.Forms.RadioButton();
      this.radioCondition = new System.Windows.Forms.RadioButton();
      this.comboCondition2 = new ComboBox();
      this.labelAssigned = new System.Windows.Forms.Label();
      this.textConditionCode = new System.Windows.Forms.TextBox();
      this.labelIs = new System.Windows.Forms.Label();
      this.toolTip1 = new ToolTip(this.components);
      this.lblWorksheet = new System.Windows.Forms.Label();
      this.btnSelect = new StandardIconButton();
      this.comboCondition = new ComboBox();
      this.panelConditions = new System.Windows.Forms.Panel();
      this.chkdListTPOActions = new CheckedListBox();
      ((ISupportInitialize) this.btnSelect).BeginInit();
      this.panelConditions.SuspendLayout();
      this.SuspendLayout();
      this.comboType.DropDownHeight = 120;
      this.comboType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboType.IntegralHeight = false;
      this.comboType.ItemHeight = 13;
      this.comboType.Location = new Point(1, 43);
      this.comboType.Name = "comboType";
      this.comboType.Size = new Size(176, 21);
      this.comboType.TabIndex = 3;
      this.comboType.SelectedIndexChanged += new EventHandler(this.comboType_SelectedIndexChanged);
      this.radioGeneral.Location = new Point(0, 0);
      this.radioGeneral.Name = "radioGeneral";
      this.radioGeneral.Size = new Size(180, 20);
      this.radioGeneral.TabIndex = 1;
      this.radioGeneral.Text = "No - Always apply this rule";
      this.radioGeneral.CheckedChanged += new EventHandler(this.radioGeneral_CheckedChanged);
      this.radioCondition.Location = new Point(0, 20);
      this.radioCondition.Name = "radioCondition";
      this.radioCondition.Size = new Size(180, 20);
      this.radioCondition.TabIndex = 2;
      this.radioCondition.Text = "Yes - Apply this rule only if";
      this.radioCondition.CheckedChanged += new EventHandler(this.radioCondition_CheckedChanged);
      this.comboCondition2.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      this.comboCondition2.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboCondition2.Location = new Point(201, 40);
      this.comboCondition2.Name = "comboCondition2";
      this.comboCondition2.Size = new Size(267, 21);
      this.comboCondition2.TabIndex = 5;
      this.comboCondition2.SelectedIndexChanged += new EventHandler(this.changesMadeToConditions);
      this.labelAssigned.AutoSize = true;
      this.labelAssigned.Location = new Point(97, 72);
      this.labelAssigned.Name = "labelAssigned";
      this.labelAssigned.Size = new Size(104, 13);
      this.labelAssigned.TabIndex = 8;
      this.labelAssigned.Text = "who has finished the";
      this.textConditionCode.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      this.textConditionCode.Location = new Point(201, 40);
      this.textConditionCode.Multiline = true;
      this.textConditionCode.Name = "textConditionCode";
      this.textConditionCode.Size = new Size(267, 45);
      this.textConditionCode.TabIndex = 30;
      this.textConditionCode.Visible = false;
      this.textConditionCode.TextChanged += new EventHandler(this.changesMadeToConditions);
      this.labelIs.AutoSize = true;
      this.labelIs.Location = new Point(181, 46);
      this.labelIs.Name = "labelIs";
      this.labelIs.Size = new Size(14, 13);
      this.labelIs.TabIndex = 31;
      this.labelIs.Text = "is";
      this.lblWorksheet.Anchor = AnchorStyles.Right;
      this.lblWorksheet.AutoSize = true;
      this.lblWorksheet.Location = new Point(367, 69);
      this.lblWorksheet.Name = "lblWorksheet";
      this.lblWorksheet.Size = new Size(103, 13);
      this.lblWorksheet.TabIndex = 33;
      this.lblWorksheet.Text = "milestone worksheet";
      this.btnSelect.Anchor = AnchorStyles.Right;
      this.btnSelect.BackColor = Color.Transparent;
      this.btnSelect.Location = new Point(472, 41);
      this.btnSelect.MouseDownImage = (System.Drawing.Image) null;
      this.btnSelect.Name = "btnSelect";
      this.btnSelect.Size = new Size(16, 16);
      this.btnSelect.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnSelect.TabIndex = 32;
      this.btnSelect.TabStop = false;
      this.btnSelect.Click += new EventHandler(this.btnSelect_Click);
      this.comboCondition.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      this.comboCondition.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboCondition.Location = new Point(201, 64);
      this.comboCondition.Name = "comboCondition";
      this.comboCondition.Size = new Size(160, 21);
      this.comboCondition.TabIndex = 4;
      this.comboCondition.SelectedIndexChanged += new EventHandler(this.changesMadeToConditions);
      this.panelConditions.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.panelConditions.Controls.Add((Control) this.radioGeneral);
      this.panelConditions.Controls.Add((Control) this.lblWorksheet);
      this.panelConditions.Controls.Add((Control) this.btnSelect);
      this.panelConditions.Controls.Add((Control) this.comboType);
      this.panelConditions.Controls.Add((Control) this.labelIs);
      this.panelConditions.Controls.Add((Control) this.radioCondition);
      this.panelConditions.Controls.Add((Control) this.comboCondition);
      this.panelConditions.Controls.Add((Control) this.comboCondition2);
      this.panelConditions.Controls.Add((Control) this.labelAssigned);
      this.panelConditions.Controls.Add((Control) this.textConditionCode);
      this.panelConditions.Location = new Point(0, 0);
      this.panelConditions.Name = "panelConditions";
      this.panelConditions.Size = new Size(492, 94);
      this.panelConditions.TabIndex = 34;
      this.chkdListTPOActions.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.chkdListTPOActions.BackColor = SystemColors.ButtonFace;
      this.chkdListTPOActions.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.chkdListTPOActions.CheckOnClick = true;
      this.chkdListTPOActions.ColumnWidth = 215;
      this.chkdListTPOActions.FormattingEnabled = true;
      this.chkdListTPOActions.Location = new Point(3, 74);
      this.chkdListTPOActions.MultiColumn = true;
      this.chkdListTPOActions.Name = "chkdListTPOActions";
      this.chkdListTPOActions.Size = new Size(485, 105);
      this.chkdListTPOActions.TabIndex = 0;
      this.chkdListTPOActions.SelectedIndexChanged += new EventHandler(this.chkdListTPOActions_SelectedIndexChanged);
      this.Controls.Add((Control) this.chkdListTPOActions);
      this.Controls.Add((Control) this.panelConditions);
      this.Name = nameof (RuleConditionControl);
      this.Size = new Size(492, 94);
      ((ISupportInitialize) this.btnSelect).EndInit();
      this.panelConditions.ResumeLayout(false);
      this.panelConditions.PerformLayout();
      this.ResumeLayout(false);
    }

    public void InitControl(BpmCategory ruleCategory)
    {
      this.loadMilestoneData();
      this.ruleCategory = ruleCategory;
      this.comboType.Items.Clear();
      switch (ruleCategory)
      {
        case BpmCategory.MilestoneRules:
        case BpmCategory.LoanAccess:
        case BpmCategory.FieldAccess:
        case BpmCategory.FieldRules:
        case BpmCategory.InputForms:
        case BpmCategory.Triggers:
        case BpmCategory.PrintForms:
        case BpmCategory.PrintSelection:
        case BpmCategory.AutomatedConditions:
        case BpmCategory.LoanActionAccess:
        case BpmCategory.LoanActionCompletionRules:
        case BpmCategory.DDMFeeScanarioRules:
        case BpmCategory.DDMFieldScenarioRules:
        case BpmCategory.AutomatedEnhancedConditions:
          this.comboType.Items.Add((object) BizRule.ConditionStrings[9]);
          this.comboType.Items.Add((object) BizRule.ConditionStrings[4]);
          this.comboType.Items.Add((object) BizRule.ConditionStrings[7]);
          if (ruleCategory == BpmCategory.FieldAccess || ruleCategory == BpmCategory.LoanActionAccess || ruleCategory == BpmCategory.LoanAccess)
          {
            this.comboType.Items.Add((object) BizRule.ConditionStrings[8]);
            this.comboCondition.Top = this.comboType.Top;
          }
          else if (ruleCategory == BpmCategory.InputForms)
          {
            this.comboType.Top = this.comboCondition.Top = this.radioGeneral.Top;
            this.radioGeneral.Visible = false;
            this.radioCondition.Visible = false;
            this.labelIs.Top = this.comboCondition.Top + 3;
            this.comboType.Enabled = this.comboCondition.Enabled = true;
            this.radioCondition.Checked = true;
            this.lblWorksheet.Top = this.labelAssigned.Top = this.comboCondition2.Top + 10;
          }
          this.comboType.Items.Add((object) BizRule.ConditionStrings[10]);
          this.comboType.Items.Add((object) BizRule.ConditionStrings[1]);
          this.comboType.Items.Add((object) BizRule.ConditionStrings[3]);
          this.comboType.Items.Add((object) BizRule.ConditionStrings[2]);
          this.comboType.Items.Add((object) BizRule.ConditionStrings[6]);
          if (ruleCategory == BpmCategory.AutomatedConditions || ruleCategory == BpmCategory.DDMFeeScanarioRules || ruleCategory == BpmCategory.DDMFieldScenarioRules || ruleCategory == BpmCategory.AutomatedEnhancedConditions)
          {
            this.comboType.Items.Add((object) BizRule.ConditionStrings[11]);
            this.comboType.Items.Add((object) BizRule.ConditionStrings[12]);
          }
          this.comboType.Items.Add((object) BizRule.ConditionStrings[5]);
          if (!this.session.ConfigurationManager.CheckIfAnyTPOSiteExists() || ruleCategory != BpmCategory.FieldAccess && ruleCategory != BpmCategory.LoanActionCompletionRules && ruleCategory != BpmCategory.LoanActionAccess && ruleCategory != BpmCategory.LoanAccess && ruleCategory != BpmCategory.FieldRules)
            break;
          this.comboType.Items.Add((object) BizRule.ConditionStrings[13]);
          break;
        case BpmCategory.Milestones:
          this.comboType.Items.Add((object) BizRule.ConditionStrings[9]);
          this.comboType.Items.Add((object) BizRule.ConditionStrings[2]);
          this.comboType.Items.Add((object) BizRule.ConditionStrings[1]);
          break;
      }
    }

    public bool ValidateCondition()
    {
      if (this.IsGeneralRule)
        return true;
      if (this.ruleCondition == BizRule.Condition.Null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this.ParentForm, "Please select a condition.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (this.ruleCondition == BizRule.Condition.AdvancedCoding)
        return this.validateAdvancedCode();
      if (this.ruleCondition == BizRule.Condition.TPOActions)
        return !this.isTPOActionCheckBoxListDisplay(this.ruleCategory) || this.validateTPOActions();
      if (this.getConditionValue() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this.ParentForm, "Please select a condition value.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (this.ruleCondition != BizRule.Condition.CurrentLoanAssocMS || !(this.getConditionValue2() == string.Empty))
        return true;
      int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a role value.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return false;
    }

    private void loadMilestoneData()
    {
      this.msNameTable = new Hashtable();
      this.msNameList = new ArrayList();
      List<EllieMae.EMLite.Workflow.Milestone> milestoneList = new List<EllieMae.EMLite.Workflow.Milestone>();
      foreach (EllieMae.EMLite.Workflow.Milestone milestone in new List<EllieMae.EMLite.Workflow.Milestone>(((BpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).GetAllMilestones()))
      {
        if (milestone.Archived)
        {
          milestoneList.Add(milestone);
        }
        else
        {
          this.msNameTable.Add((object) milestone.MilestoneID, (object) milestone.Name);
          this.msNameList.Add((object) milestone.Name);
        }
      }
      foreach (EllieMae.EMLite.Workflow.Milestone milestone in milestoneList)
      {
        this.msNameTable.Add((object) milestone.MilestoneID, (object) milestone.Name);
        this.msNameList.Add((object) milestone.Name);
      }
    }

    private void comboType_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.comboCondition.Visible = true;
      this.comboCondition2.Visible = false;
      this.btnSelect.Visible = false;
      this.textConditionCode.Visible = false;
      this.labelAssigned.Visible = false;
      this.lblWorksheet.Visible = false;
      this.chkdListTPOActions.Visible = false;
      this.comboCondition.Top = this.comboType.Top;
      this.toggleTPOActionCheckboxlist(false);
      this.bmpMap = new ArrayList();
      this.comboCondition.DataSource = (object) null;
      this.comboCondition.Items.Clear();
      this.chkdListTPOActions.Items.Clear();
      this.ruleCondition = this.comboType.SelectedItem != null ? BizRule.GetConditionEnum(this.comboType.SelectedItem.ToString()) : BizRule.Condition.Null;
      this.toolTip1.SetToolTip((Control) this.btnSelect, "");
      this.labelIs.Text = "is";
      switch (this.ruleCondition)
      {
        case BizRule.Condition.LoanPurpose:
          this.comboCondition.Items.Add((object) BizRule.LoanPurposeStrings[2]);
          this.bmpMap.Add((object) 2);
          this.comboCondition.Items.Add((object) BizRule.LoanPurposeStrings[3]);
          this.bmpMap.Add((object) 3);
          this.comboCondition.Items.Add((object) BizRule.LoanPurposeStrings[4]);
          this.bmpMap.Add((object) 4);
          this.comboCondition.Items.Add((object) BizRule.LoanPurposeStrings[5]);
          this.bmpMap.Add((object) 5);
          this.comboCondition.Items.Add((object) BizRule.LoanPurposeStrings[6]);
          this.bmpMap.Add((object) 6);
          this.comboCondition.Items.Add((object) BizRule.LoanPurposeStrings[1]);
          this.bmpMap.Add((object) 1);
          this.comboCondition.Items.Add((object) BizRule.LoanPurposeStrings[0]);
          this.bmpMap.Add((object) 0);
          break;
        case BizRule.Condition.LoanType:
          this.comboCondition.Items.Add((object) BizRule.LoanTypeStrings[2]);
          this.bmpMap.Add((object) 2);
          this.comboCondition.Items.Add((object) BizRule.LoanTypeStrings[3]);
          this.bmpMap.Add((object) 3);
          this.comboCondition.Items.Add((object) BizRule.LoanTypeStrings[4]);
          this.bmpMap.Add((object) 4);
          this.comboCondition.Items.Add((object) BizRule.LoanTypeStrings[5]);
          this.bmpMap.Add((object) 5);
          this.comboCondition.Items.Add((object) BizRule.LoanTypeStrings[6]);
          this.bmpMap.Add((object) 6);
          this.comboCondition.Items.Add((object) BizRule.LoanTypeStrings[1]);
          this.bmpMap.Add((object) 1);
          this.comboCondition.Items.Add((object) BizRule.LoanTypeStrings[0]);
          this.bmpMap.Add((object) 0);
          break;
        case BizRule.Condition.LoanStatus:
          this.comboCondition.Items.Add((object) BizRule.LoanStatusStrings[0]);
          this.bmpMap.Add((object) 0);
          this.comboCondition.Items.Add((object) BizRule.LoanStatusStrings[1]);
          this.bmpMap.Add((object) 1);
          this.comboCondition.Items.Add((object) BizRule.LoanStatusStrings[2]);
          this.bmpMap.Add((object) 2);
          break;
        case BizRule.Condition.CurrentLoanAssocMS:
          this.comboCondition2.Visible = true;
          this.labelIs.Text = " is assigned to the next milestone by the ";
          this.comboCondition2.Location = new Point(378, 43);
          this.comboCondition2.Size = new Size(this.ClientSize.Width - 400, 21);
          this.labelAssigned.Visible = true;
          this.lblWorksheet.Visible = true;
          this.btnSelect.Visible = true;
          if (this.ruleCategory == BpmCategory.InputForms)
            this.comboCondition2.Top = this.radioGeneral.Top;
          else
            this.comboCondition2.Top = this.comboType.Top;
          this.labelIs.Top = this.comboCondition2.Top + 3;
          this.comboCondition.Top = this.comboCondition2.Top + this.comboCondition2.Height + 4;
          this.labelAssigned.Top = this.comboCondition.Top + 4;
          this.lblWorksheet.Top = this.labelAssigned.Top;
          this.btnSelect.Top = this.comboCondition2.Top;
          List<string> stringList1 = new List<string>();
          foreach (EllieMae.EMLite.Workflow.Milestone allMilestones in ((MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).GetAllMilestonesList())
          {
            if (allMilestones.Archived)
              stringList1.Add(allMilestones.Name);
            else
              this.comboCondition.Items.Add((object) allMilestones.Name);
          }
          foreach (string str in stringList1)
            this.comboCondition.Items.Add((object) (str + " (Archived)"));
          this.toolTip1.SetToolTip((Control) this.btnSelect, "Select Current Role");
          break;
        case BizRule.Condition.RateLock:
          this.comboCondition.Items.Add((object) BizRule.LockDateStrings[1]);
          this.bmpMap.Add((object) 1);
          this.comboCondition.Items.Add((object) BizRule.LockDateStrings[0]);
          this.bmpMap.Add((object) 0);
          break;
        case BizRule.Condition.PropertyState:
          for (int index = 0; index <= 59; ++index)
          {
            USPS.StateCode key = (USPS.StateCode) this.toEnum(index.ToString(), typeof (USPS.StateCode));
            this.comboCondition.Items.Add(USPS.StateNames[(object) key]);
          }
          break;
        case BizRule.Condition.LoanDocType:
          this.comboCondition.Items.AddRange((object[]) LoanDocTypeMap.Descriptions);
          break;
        case BizRule.Condition.FinishedMilestone:
          List<string> stringList2 = new List<string>();
          foreach (EllieMae.EMLite.Workflow.Milestone allMilestones in ((MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).GetAllMilestonesList())
          {
            if (allMilestones.Archived)
              stringList2.Add(allMilestones.Name);
            else
              this.comboCondition.Items.Add((object) allMilestones.Name);
          }
          using (List<string>.Enumerator enumerator = stringList2.GetEnumerator())
          {
            while (enumerator.MoveNext())
              this.comboCondition.Items.Add((object) (enumerator.Current + " (Archived)"));
            break;
          }
        case BizRule.Condition.AdvancedCoding:
          this.textConditionCode.Top = this.comboType.Top;
          this.comboCondition.Visible = false;
          this.textConditionCode.Visible = true;
          this.textConditionCode.Multiline = true;
          this.btnSelect.Visible = true;
          this.btnSelect.Top = this.textConditionCode.Top;
          this.toolTip1.SetToolTip((Control) this.btnSelect, "Edit Condition");
          break;
        case BizRule.Condition.LoanProgram:
          this.textConditionCode.Top = this.comboType.Top;
          this.comboCondition.Visible = false;
          this.textConditionCode.Visible = true;
          this.textConditionCode.Multiline = false;
          this.btnSelect.Visible = true;
          this.btnSelect.Top = this.textConditionCode.Top;
          this.toolTip1.SetToolTip((Control) this.btnSelect, "Select a Loan Program");
          break;
        case BizRule.Condition.PropertyType:
          this.comboCondition.Items.Add((object) BizRule.PropertyTypeStrings[0]);
          this.bmpMap.Add((object) 0);
          this.comboCondition.Items.Add((object) BizRule.PropertyTypeStrings[1]);
          this.bmpMap.Add((object) 1);
          this.comboCondition.Items.Add((object) BizRule.PropertyTypeStrings[2]);
          this.bmpMap.Add((object) 2);
          this.comboCondition.Items.Add((object) BizRule.PropertyTypeStrings[4]);
          this.bmpMap.Add((object) 4);
          this.comboCondition.Items.Add((object) BizRule.PropertyTypeStrings[3]);
          this.bmpMap.Add((object) 3);
          this.comboCondition.Items.Add((object) BizRule.PropertyTypeStrings[6]);
          this.bmpMap.Add((object) 6);
          this.comboCondition.Items.Add((object) BizRule.PropertyTypeStrings[5]);
          this.bmpMap.Add((object) 5);
          this.comboCondition.Items.Add((object) BizRule.PropertyTypeStrings[8]);
          this.bmpMap.Add((object) 8);
          break;
        case BizRule.Condition.Occupancy:
          this.comboCondition.Items.Add((object) BizRule.PropertyOccupancyStrings[0]);
          this.bmpMap.Add((object) 0);
          this.comboCondition.Items.Add((object) BizRule.PropertyOccupancyStrings[2]);
          this.bmpMap.Add((object) 2);
          this.comboCondition.Items.Add((object) BizRule.PropertyOccupancyStrings[1]);
          this.bmpMap.Add((object) 1);
          break;
        case BizRule.Condition.TPOActions:
          TriggerActivationNameProvider activationNameProvider = new TriggerActivationNameProvider();
          string[] activationTypesByParent = activationNameProvider.GetActivationTypesByParent("TPO actions");
          Dictionary<TriggerActivationType, string> dataSource = new Dictionary<TriggerActivationType, string>();
          foreach (string activationDescription in activationTypesByParent)
          {
            if ((!this.isTPOActionCheckBoxListDisplay(this.ruleCategory) || !(activationDescription == activationNameProvider.GetDescriptionFromActivationType(TriggerActivationType.ViewPurchaseAdvice))) && (!this.isTPOActionCheckBoxListDisplay(this.ruleCategory) || this.ruleCategory != BpmCategory.LoanAccess && this.ruleCategory != BpmCategory.LoanActionAccess && this.ruleCategory != BpmCategory.LoanActionCompletionRules && this.ruleCategory != BpmCategory.FieldRules && this.ruleCategory != BpmCategory.Triggers && this.ruleCategory != BpmCategory.FieldAccess || !(activationDescription == activationNameProvider.GetDescriptionFromActivationType(TriggerActivationType.SubmitPurchase))))
            {
              bool loanActions = this.isTPOPersonaAccessToLoanActions(this.ruleCategory);
              if ((loanActions || !(activationDescription == activationNameProvider.GetDescriptionFromActivationType(TriggerActivationType.FloatLock))) && (loanActions || !(activationDescription == activationNameProvider.GetDescriptionFromActivationType(TriggerActivationType.CancelLock))) && (loanActions || !(activationDescription == activationNameProvider.GetDescriptionFromActivationType(TriggerActivationType.ReLockLock))) && (loanActions || !(activationDescription == activationNameProvider.GetDescriptionFromActivationType(TriggerActivationType.RePriceLock))) && (loanActions || !(activationDescription == activationNameProvider.GetDescriptionFromActivationType(TriggerActivationType.ChangeRequestOB))))
              {
                if (this.isTPOActionCheckBoxListDisplay(this.ruleCategory))
                  this.chkdListTPOActions.Items.Add((object) activationDescription, false);
                else
                  dataSource.Add(activationNameProvider.GetTriggerActivationTypeFromDescription(activationDescription), activationDescription);
              }
            }
          }
          if (this.isTPOActionCheckBoxListDisplay(this.ruleCategory))
          {
            this.toggleTPOActionCheckboxlist(true);
            this.comboCondition.Items.Add((object) new System.Web.UI.WebControls.ListItem("Any of", "any"));
            this.comboCondition.Items.Add((object) new System.Web.UI.WebControls.ListItem("All of", "all"));
            break;
          }
          this.comboCondition.DisplayMember = "Value";
          this.comboCondition.ValueMember = "Key";
          this.comboCondition.DataSource = (object) new BindingSource((object) dataSource, (string) null);
          break;
      }
      if (this.comboCondition.Items.Count > 0)
        this.comboCondition.SelectedIndex = 0;
      this.changesMadeToConditions(sender, e);
    }

    public void ResizeComboCondition()
    {
      this.comboCondition.BringToFront();
      this.comboCondition.Size = new Size(230, 21);
    }

    public void ResizeComboCondition(Size newSize)
    {
      this.comboCondition.BringToFront();
      this.comboCondition.Size = newSize;
    }

    private void radioCondition_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.radioCondition.Checked)
        return;
      this.comboType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboCondition.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboType.SelectedIndex = 0;
      this.comboType.Enabled = true;
      this.comboCondition.Enabled = true;
    }

    private void radioGeneral_CheckedChanged(object sender, EventArgs e)
    {
      if (this.radioGeneral.Checked)
      {
        this.comboType.DropDownStyle = ComboBoxStyle.DropDown;
        this.comboCondition.DropDownStyle = ComboBoxStyle.DropDown;
        this.comboType.SelectedIndex = -1;
        this.comboCondition.SelectedIndex = -1;
        this.textConditionCode.Text = "";
        this.textConditionCode.Visible = false;
        this.comboCondition.Visible = true;
        this.comboType.SelectedIndex = -1;
        this.comboCondition.SelectedIndex = -1;
        this.comboType.Enabled = false;
        this.comboCondition.Enabled = false;
        this.comboCondition2.Enabled = false;
        this.btnSelect.Visible = false;
        this.comboType_SelectedIndexChanged((object) null, (EventArgs) null);
      }
      this.changesMadeToConditions(sender, e);
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

    private void btnSelect_Click(object sender, EventArgs e)
    {
      this.ruleCondition = this.comboType.SelectedItem != null ? BizRule.GetConditionEnum(this.comboType.SelectedItem.ToString()) : BizRule.Condition.Null;
      switch (this.ruleCondition)
      {
        case BizRule.Condition.CurrentLoanAssocMS:
          using (SelectCurrentRoleForm selectCurrentRoleForm = new SelectCurrentRoleForm(this.session))
          {
            if (selectCurrentRoleForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
              break;
            this.setConditionValue(selectCurrentRoleForm.MilestoneID);
            this.setConditionValue2(selectCurrentRoleForm.RoleID.ToString());
            break;
          }
        case BizRule.Condition.AdvancedCoding:
          using (AdvConditionEditor advConditionEditor = new AdvConditionEditor(this.session, this.advancedCodeXml, this.ddmSetting))
          {
            if (advConditionEditor.GetConditionScript() != this.textConditionCode.Text)
              advConditionEditor.ClearFilters();
            if (advConditionEditor.ShowDialog((IWin32Window) this) != DialogResult.OK)
              break;
            this.textConditionCode.Text = advConditionEditor.GetConditionScript();
            this.advancedCodeXml = advConditionEditor.GetConditionXml();
            break;
          }
        case BizRule.Condition.LoanProgram:
          using (TemplateSelectionDialog templateSelectionDialog = new TemplateSelectionDialog(this.session, EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanProgram, (FileSystemEntry) null, true))
          {
            if (templateSelectionDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
              break;
            this.textConditionCode.Text = templateSelectionDialog.SelectedItem.Name;
            break;
          }
      }
    }

    private bool validateAdvancedCode()
    {
      if (this.textConditionCode.Text == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this.ParentForm, "You must provide code to determine the conditions under which this rule applies.");
        return false;
      }
      try
      {
        string str = this.textConditionCode.Text.Trim();
        if (this.ruleCategory == BpmCategory.LoanActionAccess)
        {
          if (!this.validateTPOActionNames(str))
            throw new CompileException("", str, new System.CodeDom.Compiler.CompilerErrorCollection());
        }
        else if (str.IndexOf("LoanActions.IsComplete(\"") > -1)
          throw new CompileException("", str, new System.CodeDom.Compiler.CompilerErrorCollection());
        using (RuntimeContext context = RuntimeContext.Create())
          new AdvancedCodeCondition(str).CreateImplementation(context);
        return true;
      }
      catch (CompileException ex)
      {
        if (EnConfigurationSettings.GlobalSettings.Debug)
        {
          ErrorDialog.Display((Exception) ex);
          return false;
        }
        int num = (int) Utils.Dialog((IWin32Window) this.ParentForm, "Validation failed: the condition contains errors or is not a valid expression.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      catch (Exception ex)
      {
        if (EnConfigurationSettings.GlobalSettings.Debug)
        {
          ErrorDialog.Display(ex);
          return false;
        }
        int num = (int) Utils.Dialog((IWin32Window) this.ParentForm, "Error validating expression: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
    }

    private void changesMadeToConditions(object sender, EventArgs e)
    {
      if (this.ChangesMadeToConditions == null)
        return;
      this.ChangesMadeToConditions((object) this, EventArgs.Empty);
    }

    private void toggleTPOActionCheckboxlist(bool isVisible)
    {
      if (!this.isTPOActionCheckBoxListDisplay(this.ruleCategory))
        return;
      int num1;
      int num2;
      int num3;
      if (!isVisible)
      {
        this.comboCondition.Width = 163;
        num1 = 92;
        num2 = 555;
        num3 = 600;
      }
      else
      {
        this.comboCondition.Width = 85;
        num1 = 205;
        num2 = 665;
        num3 = 705;
      }
      this.SuspendLayout();
      this.chkdListTPOActions.Visible = isVisible;
      this.Height = num1;
      this.ResumeLayout();
      if (this.Parent == null)
        return;
      this.Parent.SuspendLayout();
      this.Parent.Height = num1;
      this.Parent.ResumeLayout();
      this.Parent.Parent.SuspendLayout();
      this.Parent.Parent.Height = num2;
      this.Parent.Parent.Parent.Height = num3;
      if (Convert.ToString(this.comboType.SelectedItem) == BizRule.ConditionStrings[13])
        this.Parent.Parent.Parent.Location = this.alignParentDialogtoCenteroftheScreen();
      this.Parent.Parent.ResumeLayout();
    }

    private bool validateTPOActions()
    {
      if (this.chkdListTPOActions.CheckedItems.Count > 0)
        return true;
      int num = (int) Utils.Dialog((IWin32Window) this.ParentForm, "Please add at least one TPO Action.");
      return false;
    }

    private void chkdListTPOActions_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.ruleCategory != BpmCategory.LoanActionCompletionRules)
        return;
      CheckedListBox checkedListBox = (CheckedListBox) sender;
      if (this.chkdListTPOActions.CheckedItems.Count == this.chkdListTPOActions.Items.Count)
      {
        this.chkdListTPOActions.SetItemChecked(checkedListBox.SelectedIndex, false);
        int num = (int) Utils.Dialog((IWin32Window) this.ParentForm, "Not all TPO actions can be selected at one time in this section.");
      }
      else
        this.changesMadeToConditions(sender, e);
    }

    private bool isTPOActionCheckBoxListDisplay(BpmCategory bpmCategory)
    {
      return bpmCategory == BpmCategory.LoanActionAccess || bpmCategory == BpmCategory.LoanActionCompletionRules || bpmCategory == BpmCategory.FieldAccess || bpmCategory == BpmCategory.LoanAccess || bpmCategory == BpmCategory.FieldRules;
    }

    private bool isTPOPersonaAccessToLoanActions(BpmCategory bpmCategory)
    {
      return bpmCategory == BpmCategory.LoanActionAccess;
    }

    private Point alignParentDialogtoCenteroftheScreen()
    {
      return new Point((Screen.PrimaryScreen.Bounds.Width - this.Parent.Parent.Parent.Width) / 2, (Screen.PrimaryScreen.Bounds.Height - this.Parent.Parent.Parent.Height) / 2);
    }

    private List<int> getPositions(string source, string searchString)
    {
      List<int> positions = new List<int>();
      int length = searchString.Length;
      int num = -length;
      while (true)
      {
        num = source.IndexOf(searchString, num + length);
        if (num != -1)
          positions.Add(num);
        else
          break;
      }
      return positions;
    }

    private bool validateTPOActionNames(string advancedCode)
    {
      bool flag = true;
      string searchString = "LoanActions.IsComplete(\"";
      List<int> positions = this.getPositions(advancedCode, searchString);
      List<string> stringList = new List<string>();
      foreach (int num1 in positions)
      {
        int startIndex = num1 + searchString.Length;
        int num2 = advancedCode.IndexOf("\"", startIndex + 1);
        stringList.Add(advancedCode.Substring(startIndex, num2 - startIndex).Trim());
      }
      foreach (string loanActionName in stringList)
      {
        if (!TriggerActivationNameProvider.IsLoanActionValid(loanActionName))
          flag = false;
        if (!flag)
          break;
      }
      return flag;
    }
  }
}
