// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.FeeScenarioRules
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class FeeScenarioRules : Form, IHelp
  {
    private string _className = nameof (FeeScenarioRules);
    private string _gcScenariosForLineTextFormat = "Scenarios for {0} {1}";
    private string _gcScenarioRuleDetailsTextFormat = "{0} Details";
    private string _gcScenariosForLineText = string.Empty;
    private static readonly string _sw = Tracing.SwOutsideLoan;
    private static Dictionary<string, Dictionary<string, int>> fieldOrderCache = new Dictionary<string, Dictionary<string, int>>();
    private Sessions.Session _session;
    private DDMFeeRuleScenariosBpmManager _feeScenariosBpmManager;
    private DDMFeeRulesBpmManager _feeRulesBpmMgr;
    private List<EllieMae.EMLite.Workflow.Milestone> _msList;
    private RoleInfo[] _roleInfos;
    private DDMFeeRuleScenario _currentFeeRuleScenario;
    private ChannelConditionControl channelControl;
    private RuleConditionControl ruleConditonControl;
    private EffectiveDateControl effectiveDateControl;
    private int _maxOrder;
    private GVItem _selectedGridRow;
    private DDMFeeRule _feeRule;
    private bool _activatedFromLandingPage;
    private int activeCount;
    private bool isReset;
    private bool fromChkFilter;
    private bool isGlobalSetting;
    private bool anyChangeToActivateOrDeactivateScenarios;
    private DDMFeeRule _previousFeeRuleBeforeGlobalSettingChange;
    private const int STATUSCOLUMN = 6;
    private const int GLOBALCONDITIONCOLUMN = 4;
    private const int FIELDVALUETYPE = 2;
    private const int FIELDVALUE = 3;
    private string _oapiGatewayPrefix;
    private int activeDeActiveScenarioId;
    private IContainer components;
    private GroupContainer gcScenariosForLine;
    private StandardIconButton stdButtonNew;
    private StandardIconButton stdButtonUp;
    private StandardIconButton stdButtonDown;
    private StandardIconButton stdButtonCopy;
    private StandardIconButton stdButtonDelete;
    private ButtonEx btnGlobalRuleSettings;
    private ButtonEx btnDeactivate;
    private FormattedLabel lblHeaderText;
    private GridView gvLineScenarios;
    private GroupContainer gcScenarioRuleDetails;
    private TabControlEx tabControlRuleDetails;
    private TabPageEx tpDetails;
    private TabPageEx tpValues;
    private FormattedLabel lblRuleName;
    private TextBox tbxScenarioName;
    private FormattedLabel lblChannels;
    private FormattedLabel formattedLabel1;
    private FormattedLabel lblNotesComments;
    private TextBox tbxNotesComments;
    private Panel panelChannel;
    private Panel panelCondition;
    private EMHelpLink emHelpLink1;
    private StandardIconButton stdButtonSave;
    private StandardIconButton stdButtonReset;
    private Button btnClose;
    private ButtonEx btnClearValue;
    private ButtonEx btnSetValue;
    private GridView gvScenarioFeeValues;
    private ToolTip ttipMessageAltText;
    private Label label1;
    private CheckBox chkFeeNotAllowed;
    private FormattedLabel formattedLabel2;
    private Panel panelEffectiveDate;
    private CheckBox chkShowFieldsValue;
    private StandardIconButton middleSaveButton;
    private StandardIconButton middleResetButton;
    private Button bottomSaveButton;
    private VerticalSeparator verticalSeparator1;

    private event EventHandler OnScenarioDeleted;

    public FeeScenarioRules()
    {
      if (this.DesignMode)
        return;
      int num = (int) MessageBox.Show("Do not use the default constructor FeeScenarioRules()");
    }

    public FeeScenarioRules(
      DDMFeeRule feeRule,
      Sessions.Session session,
      bool activatedFromLandingPage)
    {
      this.InitializeComponent();
      this._session = session;
      this._oapiGatewayPrefix = string.IsNullOrEmpty(session.StartupInfo.OAPIGatewayBaseUri) ? EnConfigurationSettings.AppSettings["oAuth.Url"] : session.StartupInfo.OAPIGatewayBaseUri;
      this._msList = new List<EllieMae.EMLite.Workflow.Milestone>(((BpmManager) this._session.BPM.GetBpmManager(BpmCategory.Milestones)).GetAllMilestones());
      this._roleInfos = ((WorkflowManager) this._session.BPM.GetBpmManager(BpmCategory.Workflow)).GetAllRoleFunctions();
      this._feeScenariosBpmManager = (DDMFeeRuleScenariosBpmManager) this._session.BPM.GetBpmManager(BpmCategory.DDMFeeScanarioRules);
      this._feeRulesBpmMgr = (DDMFeeRulesBpmManager) this._session.BPM.GetBpmManager(BpmCategory.DDMFeeRules);
      this._feeRule = feeRule;
      this._activatedFromLandingPage = activatedFromLandingPage;
      this.InitForm();
      this.OnScenarioDeleted += new EventHandler(this.FeeScenarioRules_ScenarioDeleted);
      foreach (BizRuleInfo scenario in this._feeRule.Scenarios)
      {
        if (scenario.Status == BizRule.RuleStatus.Active)
          ++this.activeCount;
      }
    }

    public DDMFeeRule FeeRule { get; set; }

    public int ActiveScenarioCount => this.activeCount;

    public int ActiveDeActiveScenarioId
    {
      get => this.activeDeActiveScenarioId;
      set => this.activeDeActiveScenarioId = value;
    }

    private void InitForm()
    {
      this._gcScenariosForLineText = string.Format(this._gcScenariosForLineTextFormat, this._feeRule.FeeType == type.line ? (object) "Line" : (object) "Line Group", (object) this._feeRule.FeeLine);
      this.gcScenariosForLine.Text = this._gcScenariosForLineText.Length > 70 ? this._gcScenariosForLineText.Substring(0, 67) + "..." : this._gcScenariosForLineText;
      this.ttipMessageAltText.Show(this._gcScenariosForLineText, (IWin32Window) this.gcScenariosForLine);
      this.gcScenarioRuleDetails.Text = string.Format(this._gcScenarioRuleDetailsTextFormat, (object) "");
      this.GetRulesFromDatabase();
    }

    private void FeeScenarioRules_Load(object sender, EventArgs e)
    {
      if (this._feeRule.Scenarios == null)
        this._feeRule.Scenarios = new List<DDMFeeRuleScenario>();
      if (this._feeRule.Scenarios.Count != 0)
        return;
      this.createNewEmptyScenario();
    }

    private void createNewEmptyScenario()
    {
      this.gvLineScenarios.SelectedIndexChanged -= new EventHandler(this.gvLineScenarios_SelectedIndexChanged);
      this.CreateNewFeeScenario(this._feeRule.RuleID);
      this.SetupUIForSelectedScenario();
      this.btnGlobalRuleSettings.Enabled = true;
      this.gvLineScenarios.SelectedIndexChanged += new EventHandler(this.gvLineScenarios_SelectedIndexChanged);
    }

    private DDMFeeRuleScenario CreateNewFeeScenario(int feeRuleID)
    {
      ++this._maxOrder;
      int num = 0;
      foreach (GVItem selectedItem in this.gvLineScenarios.SelectedItems)
        num = selectedItem.Index + 1;
      DDMFeeRuleScenario newFeeRuleScenario = new DDMFeeRuleScenario(this.GetNewFeeRuleScenarioName());
      newFeeRuleScenario.Order = num;
      newFeeRuleScenario.FeeRuleID = feeRuleID;
      newFeeRuleScenario.ParentRuleName = this._feeRule.RuleName;
      GVItem newItem = new GVItem((object) 1);
      this.PopulateFeeRuleValueFields(newFeeRuleScenario);
      this.BindScenarioToGridView(newFeeRuleScenario, newItem, index: num);
      newFeeRuleScenario.OrderChanged = newFeeRuleScenario.ContentChanged = true;
      this._feeRule.Scenarios.Add(newFeeRuleScenario);
      newItem.Selected = true;
      this.ReorderRules(num);
      this.BindScenarioValuesToGridView(newFeeRuleScenario);
      return newFeeRuleScenario;
    }

    private string GetNewFeeRuleScenarioName()
    {
      if (this._feeRule.Scenarios.Count<DDMFeeRuleScenario>((Func<DDMFeeRuleScenario, bool>) (scenario => !scenario.Deleted && string.Compare(scenario.RuleName, "New Fee Scenario Name", true) == 0)) == 0)
        return "New Fee Scenario Name";
      IEnumerable<DDMFeeRuleScenario> ddmFeeRuleScenarios = this._feeRule.Scenarios.Where<DDMFeeRuleScenario>((Func<DDMFeeRuleScenario, bool>) (scenario => !scenario.Deleted && scenario.RuleName.StartsWith("New Fee Scenario Name (")));
      int num = 0;
      foreach (DDMFeeRuleScenario ddmFeeRuleScenario in ddmFeeRuleScenarios)
      {
        int startIndex = ddmFeeRuleScenario.RuleName.LastIndexOf('(') + 1;
        int length = ddmFeeRuleScenario.RuleName.LastIndexOf(')') - startIndex;
        if (num < Convert.ToInt32(ddmFeeRuleScenario.RuleName.Substring(startIndex, length)))
          num = Convert.ToInt32(ddmFeeRuleScenario.RuleName.Substring(startIndex, length));
      }
      return num == 0 ? "New Fee Scenario Name (1)" : string.Format("New Fee Scenario Name ({0})", (object) (num + 1));
    }

    private string ChangeNameIfAlreadyExists(string scenarioName)
    {
      if (this._feeRule.Scenarios.Count<DDMFeeRuleScenario>((Func<DDMFeeRuleScenario, bool>) (scenario => !scenario.Deleted && string.Compare(scenario.RuleName, scenarioName, true) == 0)) == 0)
        return scenarioName;
      int length1 = scenarioName.IndexOf('(');
      string scenarioNameSubstring = scenarioName.Substring(0, length1).Trim();
      IEnumerable<DDMFeeRuleScenario> ddmFeeRuleScenarios = this._feeRule.Scenarios.Where<DDMFeeRuleScenario>((Func<DDMFeeRuleScenario, bool>) (scenario => !scenario.Deleted && scenario.RuleName.StartsWith(scenarioNameSubstring + "(")));
      int num = 0;
      foreach (DDMFeeRuleScenario ddmFeeRuleScenario in ddmFeeRuleScenarios)
      {
        int startIndex = ddmFeeRuleScenario.RuleName.LastIndexOf('(') + 1;
        int length2 = ddmFeeRuleScenario.RuleName.LastIndexOf(')') - startIndex;
        if (num < Convert.ToInt32(ddmFeeRuleScenario.RuleName.Substring(startIndex, length2)))
          num = Convert.ToInt32(ddmFeeRuleScenario.RuleName.Substring(startIndex, length2));
      }
      return num == 0 ? scenarioNameSubstring + " (1)" : string.Format("{0} ({1})", (object) scenarioNameSubstring, (object) (num + 1));
    }

    private void PopulateFeeRuleValueFields(DDMFeeRuleScenario newFeeRuleScenario)
    {
      if (this._feeRule != null && string.IsNullOrEmpty(this._feeRule.FeeLine))
        return;
      if (newFeeRuleScenario.FeeRuleValues == null)
        newFeeRuleScenario.FeeRuleValues = new List<DDMFeeRuleValue>();
      List<string[]> strArrayList = new List<string[]>();
      strArrayList.AddRange((IEnumerable<string[]>) DDM_FieldAccess_Utils.GetDDMNonCalculatedFields(this._feeRule.FeeLine));
      foreach (string[] strArray in strArrayList)
      {
        foreach (string fieldid in strArray)
          this.PopulateFeeRuleValue(newFeeRuleScenario, fieldid);
      }
    }

    private void PopulateFeeRuleValue(DDMFeeRuleScenario newFeeRuleScenario, string fieldid)
    {
      FieldDefinition field = EncompassFields.GetField(fieldid);
      FieldFormat format = field.Format;
      newFeeRuleScenario.FeeRuleValues.Add(new DDMFeeRuleValue()
      {
        Field_Name = field == null || field.Description == null ? "" : field.Description,
        Field_Type = format,
        Field_Value = string.Empty,
        Field_Value_Type = fieldValueType.ValueNotSet,
        FieldID = fieldid,
        RuleScenarioID = newFeeRuleScenario.FeeRuleID
      });
    }

    private void ToggleTabsEditable()
    {
      if (this._currentFeeRuleScenario == null)
      {
        this.ToggleTabEditable(this.tpDetails, false);
        this.ToggleTabEditable(this.tpValues, false);
      }
      else
      {
        this.ToggleTabEditable(this.tpDetails, this._currentFeeRuleScenario.EditMode);
        this.ToggleTabEditable(this.tpValues, this._currentFeeRuleScenario.EditMode);
      }
    }

    private void ToggleTabEditable(TabPageEx tabPage, bool editable)
    {
      foreach (Control control in (ArrangedElementCollection) tabPage.Controls)
      {
        if (control is TextBox)
          ((TextBoxBase) control).ReadOnly = !editable;
        else
          control.Enabled = editable;
      }
    }

    private string BuildChannelString(BizRuleInfo bizInfo)
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

    private string BuildConditionString(BizRuleInfo bizInfo)
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
          for (int index4 = 0; index4 < this._roleInfos.Length; ++index4)
          {
            if (string.Compare(this._roleInfos[index4].RoleID.ToString(), bizInfo.ConditionState2, true) == 0)
            {
              str5 = this._roleInfos[index4].RoleName;
              break;
            }
          }
          str1 = str5 + " Assigned on " + this.GetMilestoneName(bizInfo.ConditionState);
          break;
        case BizRule.Condition.RateLock:
          string str6 = "Rate is ";
          int index5 = Utils.ParseInt((object) bizInfo.ConditionState);
          str1 = index5 <= BizRule.LockDateStrings.Length - 1 ? str6 + BizRule.LockDateStrings[index5] : str6 + BizRule.LockDateStrings[0];
          break;
        case BizRule.Condition.PropertyState:
          string str7 = "Property State is ";
          USPS.StateCode key = (USPS.StateCode) this.ToEnum(Utils.ParseInt((object) bizInfo.ConditionState).ToString(), typeof (USPS.StateCode));
          str1 = !USPS.StateNames.ContainsKey((object) key) ? str7 + USPS.StateNames[(object) USPS.StateCode.Unknown] : str7 + USPS.StateNames[(object) key];
          break;
        case BizRule.Condition.LoanDocType:
          string str8 = "Loan Doc Type is ";
          int index6 = Utils.ParseInt((object) bizInfo.ConditionState);
          str1 = index6 + 1 > LoanDocTypeMap.Descriptions.Length || index6 == 0 ? str8 + "Empty" : str8 + LoanDocTypeMap.Descriptions[index6];
          break;
        case BizRule.Condition.FinishedMilestone:
          str1 = "Completed milestone is " + this.GetMilestoneName(bizInfo.ConditionState);
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

    private object ToEnum(string value, System.Type enumType)
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

    private string GetMilestoneName(string id)
    {
      string milestoneName = string.Empty;
      EllieMae.EMLite.Workflow.Milestone milestone = this._msList.Find((Predicate<EllieMae.EMLite.Workflow.Milestone>) (item => item.MilestoneID == id));
      if (milestone != null)
        milestoneName = milestone.Name;
      return milestoneName;
    }

    private void gvLineScenarios_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.chkFeeNotAllowed.CheckedChanged -= new EventHandler(this.chkFeeNotAllowed_CheckedChanged);
      this.gvLineScenarios.SelectedIndexChanged -= new EventHandler(this.gvLineScenarios_SelectedIndexChanged);
      if (this._currentFeeRuleScenario != null)
        this.SyncUIDetailsToScenario();
      if (this.ValidateForm())
      {
        if (this._selectedGridRow != null && this.gvLineScenarios.SelectedItems.Count != 0 && this.gvLineScenarios.SelectedItems[0].Index == this._selectedGridRow.Index)
          this._selectedGridRow.Selected = true;
        else if (this.gvLineScenarios.SelectedItems.Count == 0 && this.gvLineScenarios.Items.Count > 0 && this._currentFeeRuleScenario != null)
        {
          this._selectedGridRow = this.gvLineScenarios.Items[this.gvLineScenarios.Items.Count - 1];
          this._selectedGridRow.Selected = true;
        }
        this.SetupUIForSelectedScenario();
      }
      else
      {
        this.gvLineScenarios.SelectedItems.Clear();
        this.gvLineScenarios.Items[this._selectedGridRow.Index].Selected = true;
      }
      this.chkFeeNotAllowed.CheckedChanged += new EventHandler(this.chkFeeNotAllowed_CheckedChanged);
      this.gvLineScenarios.SelectedIndexChanged += new EventHandler(this.gvLineScenarios_SelectedIndexChanged);
    }

    private void SetupUIForSelectedScenario()
    {
      if (this.gvLineScenarios.SelectedItems.Count != 0 && this.gvLineScenarios.SelectedItems[0].SubItems[6].Text == BizRule.RuleStatusStrings[1])
      {
        this.btnDeactivate.Enabled = true;
        this.btnDeactivate.Text = "Deactivate";
      }
      else if (this.gvLineScenarios.SelectedItems.Count != 0 && this.gvLineScenarios.SelectedItems[0].SubItems[6].Text == BizRule.RuleStatusStrings[0])
      {
        this.btnDeactivate.Enabled = true;
        this.btnDeactivate.Text = "Activate";
      }
      else
      {
        this.btnDeactivate.Enabled = false;
        this._currentFeeRuleScenario = (DDMFeeRuleScenario) null;
      }
      if (this.gvLineScenarios.SelectedItems != null && this.gvLineScenarios.SelectedItems.Count != 0)
      {
        this._currentFeeRuleScenario = (DDMFeeRuleScenario) this.gvLineScenarios.SelectedItems[0].Tag;
        this._selectedGridRow = this.gvLineScenarios.SelectedItems[0];
      }
      this.SetDetailsTabPage();
      this.SetValueTabPage();
      this.HandleStandardButtons();
    }

    private bool SaveOrDiscardPendingChanges()
    {
      int pendingContentChangesCount = this.PendingContentChanges();
      return (pendingContentChangesCount <= 0 || this._currentFeeRuleScenario == null) && !this.AnyNewScenarioToPersist() || this.PromptUserToSaveOrDiscard(pendingContentChangesCount);
    }

    private bool AnyNewScenarioToPersist()
    {
      return this._feeRule.Scenarios.Count<DDMFeeRuleScenario>((Func<DDMFeeRuleScenario, bool>) (scenario => !scenario.Deleted && scenario.RuleID == 0)) > 0;
    }

    private bool PromptUserToSaveOrDiscard(int pendingContentChangesCount)
    {
      if (pendingContentChangesCount == 0)
        return true;
      switch (MessageBox.Show((IWin32Window) this, "There are unsaved changes to one or more scenarios. Do you want to save them?", "Save Changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
      {
        case DialogResult.Yes:
          return this.SaveMultipleScenarios();
        case DialogResult.No:
          this.stdButtonReset_Click((object) this, (EventArgs) null);
          break;
      }
      return false;
    }

    private void ResetScenarioHeader()
    {
      this._gcScenariosForLineText = string.Format(this._gcScenariosForLineTextFormat, this._feeRule.FeeType == type.line ? (object) "Line" : (object) "Line Group", (object) this._feeRule.FeeLine);
      this.gcScenariosForLine.Text = this._gcScenariosForLineText.Length > 70 ? this._gcScenariosForLineText.Substring(0, 67) + "..." : this._gcScenariosForLineText;
      this.ttipMessageAltText.Show(this._gcScenariosForLineText, (IWin32Window) this.gcScenariosForLine);
    }

    private void ResetMaxOrder()
    {
      this._maxOrder = this._feeRule.Scenarios.Count == 0 ? 0 : this._feeRule.Scenarios.Max<DDMFeeRuleScenario>((Func<DDMFeeRuleScenario, int>) (scenario => scenario.Order));
    }

    private void HandleStandardButtons()
    {
      this.HandleSaveResetDeleteButtons();
      this.HandleUpDownButtons();
      this.HandleNewCopyEditButtons();
      this.HandleDeactiveAndGlobalRuleSettings();
    }

    private void HandleDeactiveAndGlobalRuleSettings()
    {
      if (!this._activatedFromLandingPage)
        this.btnGlobalRuleSettings.Enabled = true;
      else
        this.btnGlobalRuleSettings.Enabled = this._activatedFromLandingPage && this._currentFeeRuleScenario != null && this.PendingChanges() == 0;
      this.btnDeactivate.Enabled = this._currentFeeRuleScenario != null && this.gvLineScenarios.Items.Count != 0 && this.PendingContentChanges() == 0;
    }

    private void HandleNewCopyEditButtons()
    {
      this.stdButtonNew.Enabled = true;
      this.stdButtonCopy.Enabled = this._currentFeeRuleScenario != null && this._currentFeeRuleScenario.RuleID > 0;
    }

    private void HandleSaveResetDeleteButtons()
    {
      this.stdButtonDelete.Enabled = this.gvLineScenarios.SelectedItems.Count != 0;
      bool flag = this.PendingChanges() > 0;
      this.middleSaveButton.Enabled = this.bottomSaveButton.Enabled = this.stdButtonSave.Enabled = flag;
      this.stdButtonReset.Enabled = this.middleResetButton.Enabled = flag;
    }

    private void HandleUpDownButtons()
    {
      if (this.gvLineScenarios.SelectedItems.Count == 0 || this.gvLineScenarios.Items.Count == 1)
        this.stdButtonDown.Enabled = this.stdButtonUp.Enabled = false;
      else if (this._selectedGridRow.Index == 0)
      {
        this.stdButtonDown.Enabled = true;
        this.stdButtonUp.Enabled = false;
      }
      else if (this._selectedGridRow.Index == this.gvLineScenarios.Items.Count - 1)
      {
        this.stdButtonUp.Enabled = true;
        this.stdButtonDown.Enabled = false;
      }
      else
        this.stdButtonDown.Enabled = this.stdButtonUp.Enabled = true;
    }

    private void SetValueTabPage()
    {
      this.gvScenarioFeeValues.Items.Clear();
      if (this._currentFeeRuleScenario == null || this._currentFeeRuleScenario.FeeRuleValues == null || this._currentFeeRuleScenario.FeeRuleValues.Count <= 0)
        return;
      this.BindScenarioValuesToGridView(this._currentFeeRuleScenario);
    }

    private void SetDetailsTabPage()
    {
      this.tbxScenarioName.TextChanged -= new EventHandler(this.tbxScenarioName_TextChanged);
      this.tbxNotesComments.TextChanged -= new EventHandler(this.tbxNotesComments_TextChanged);
      if (this._currentFeeRuleScenario == null)
      {
        this.tbxScenarioName.Text = string.Empty;
        this.tbxNotesComments.Text = string.Empty;
        foreach (Control control in (ArrangedElementCollection) this.tpDetails.Controls)
          control.Enabled = false;
      }
      else
      {
        if (!this.tbxScenarioName.Enabled)
        {
          foreach (Control control in (ArrangedElementCollection) this.tpDetails.Controls)
            control.Enabled = true;
        }
        this.tbxScenarioName.Text = this._currentFeeRuleScenario.RuleName;
        this.tbxNotesComments.Text = this._currentFeeRuleScenario.CommentsTxt;
        this.panelChannel.Controls.Remove((Control) this.channelControl);
        this.channelControl = new ChannelConditionControl();
        this.channelControl.ChannelValue = this._currentFeeRuleScenario.Condition2;
        this.channelControl.ChangesMadeToChannel += new EventHandler(this.channelControl_ChangesMadeToChannel);
        this.panelChannel.Controls.Add((Control) this.channelControl);
        this.panelCondition.Controls.Remove((Control) this.ruleConditonControl);
        this.ruleConditonControl = new RuleConditionControl(this._session, true);
        this.ruleConditonControl.InitControl(BpmCategory.DDMFeeScanarioRules);
        this.ruleConditonControl.SetCondition((BizRuleInfo) this._currentFeeRuleScenario);
        this.ruleConditonControl.ChangesMadeToConditions += new EventHandler(this.ruleConditonControl_ChangesMadeToConditions);
        this.panelCondition.Controls.Add((Control) this.ruleConditonControl);
        this.panelEffectiveDate.Controls.Remove((Control) this.effectiveDateControl);
        this.effectiveDateControl = new EffectiveDateControl(this._session);
        this.effectiveDateControl.EffectiveDateConcatenatedInfo = this._currentFeeRuleScenario.EffectiveDateInfo;
        this.effectiveDateControl.ChangesMadeToEffectiveDate += new EventHandler(this.effectiveDateControl_ChangesMadeToEffectiveDate);
        this.panelEffectiveDate.Controls.Add((Control) this.effectiveDateControl);
      }
      this.SetDetailsSectionTitle();
      this.ToggleTabsEditable();
      this.tbxScenarioName.TextChanged += new EventHandler(this.tbxScenarioName_TextChanged);
      this.tbxNotesComments.TextChanged += new EventHandler(this.tbxNotesComments_TextChanged);
    }

    private void effectiveDateControl_ChangesMadeToEffectiveDate(object sender, EventArgs e)
    {
      if (this.effectiveDateControl != null && string.IsNullOrEmpty(this.effectiveDateControl.EffectiveDateFieldId))
        return;
      this.MarkDirty();
    }

    private void SetDetailsSectionTitle()
    {
      if (this._currentFeeRuleScenario == null)
        this.gcScenarioRuleDetails.Text = string.Format(this._gcScenarioRuleDetailsTextFormat, (object) string.Empty);
      else
        this.gcScenarioRuleDetails.Text = string.Format(this._gcScenarioRuleDetailsTextFormat, (object) this._currentFeeRuleScenario.RuleName);
    }

    private void channelControl_ChangesMadeToChannel(object sender, EventArgs e)
    {
      this.MarkDirty();
    }

    private void ruleConditonControl_ChangesMadeToConditions(object sender, EventArgs e)
    {
      if (this.ruleConditonControl.GetRuleCondition() == BizRule.Condition.CurrentLoanAssocMS)
        this.ruleConditonControl.ResizeComboCondition(new Size(200, 21));
      else
        this.ruleConditonControl.ResizeComboCondition(new Size(300, 21));
      this.ruleConditonControl.ApplyCondition((BizRuleInfo) this._currentFeeRuleScenario);
      this.MarkDirty();
    }

    private void stdButtonUp_Click(object sender, EventArgs e)
    {
      this.SyncUIDetailsToScenario();
      if (!this.ValidateForm())
        return;
      int index = this._selectedGridRow.Index;
      if (index == 0)
        return;
      GVItem newItem = this.gvLineScenarios.Items[index - 1];
      DDMFeeRuleScenario tag = (DDMFeeRuleScenario) newItem.Tag;
      int order1 = tag.Order;
      int order2 = this._currentFeeRuleScenario.Order;
      tag.Order = order2;
      this._currentFeeRuleScenario.Order = order1;
      tag.OrderChanged = this._currentFeeRuleScenario.OrderChanged = true;
      this._selectedGridRow.Tag = (object) tag;
      this.BindScenarioToGridView(tag, this._selectedGridRow, false);
      newItem.Tag = (object) this._currentFeeRuleScenario;
      this.BindScenarioToGridView(this._currentFeeRuleScenario, newItem, false);
      this._selectedGridRow = newItem;
      this._selectedGridRow.Selected = true;
      this.HandleStandardButtons();
    }

    private void stdButtonDown_Click(object sender, EventArgs e)
    {
      this.SyncUIDetailsToScenario();
      if (!this.ValidateForm())
        return;
      int index = this._selectedGridRow.Index;
      if (index == this.gvLineScenarios.Items.Count - 1)
        return;
      GVItem newItem = this.gvLineScenarios.Items[index + 1];
      DDMFeeRuleScenario tag = (DDMFeeRuleScenario) newItem.Tag;
      int order1 = tag.Order;
      int order2 = this._currentFeeRuleScenario.Order;
      tag.Order = order2;
      this._currentFeeRuleScenario.Order = order1;
      tag.OrderChanged = this._currentFeeRuleScenario.OrderChanged = true;
      this._selectedGridRow.Tag = (object) tag;
      this.BindScenarioToGridView(tag, this._selectedGridRow, false);
      newItem.Tag = (object) this._currentFeeRuleScenario;
      this.BindScenarioToGridView(this._currentFeeRuleScenario, newItem, false);
      this._selectedGridRow = newItem;
      this._selectedGridRow.Selected = true;
      this.HandleStandardButtons();
    }

    private void stdButtonDelete_Click(object sender, EventArgs e)
    {
      if (!this._currentFeeRuleScenario.Inactive)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, string.Format("Active scenario cannot be deleted."), "Delete Scenario?", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (MessageBox.Show((IWin32Window) this, string.Format("Are you sure you want to delete the scenario '{0}'?", (object) this._currentFeeRuleScenario.RuleName), "Delete Scenario?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
          return;
        int sender1 = this._currentFeeRuleScenario == null ? -1 : this._selectedGridRow.Index;
        this.DeleteRoutine();
        this.OnScenarioDeleted((object) sender1, (EventArgs) null);
      }
    }

    private void FeeScenarioRules_ScenarioDeleted(object sender, EventArgs e)
    {
      this.ResetMaxOrder();
      this.SetNextScenarioAsSelected((int) sender);
      this.HandleStandardButtons();
      this.SetDetailsTabPage();
      this.SetValueTabPage();
    }

    private void SetNextScenarioAsSelected(int indexOfDeletedScenario)
    {
      if (indexOfDeletedScenario == -1 || this.gvLineScenarios.Items.Count == 0)
        return;
      this._selectedGridRow = indexOfDeletedScenario != this.gvLineScenarios.Items.Count ? this.gvLineScenarios.Items[indexOfDeletedScenario] : this.gvLineScenarios.Items[this.gvLineScenarios.Items.Count - 1];
      this._selectedGridRow.Selected = true;
    }

    private void DeleteRoutine()
    {
      if (this._currentFeeRuleScenario == null)
        return;
      if (this._currentFeeRuleScenario.RuleID == 0)
      {
        this._feeRule.Scenarios.Remove(this._currentFeeRuleScenario);
        this._currentFeeRuleScenario = (DDMFeeRuleScenario) null;
        int index = this._selectedGridRow.Index;
        this.gvLineScenarios.Items.Remove(this._selectedGridRow);
        this._selectedGridRow = (GVItem) null;
        this.ReorderRules(index);
      }
      else if (this._currentFeeRuleScenario.RuleID != 0 && this.gvLineScenarios.Items.Count == 1)
      {
        this._currentFeeRuleScenario.Deleted = true;
        this._currentFeeRuleScenario = (DDMFeeRuleScenario) null;
        this.gvLineScenarios.Items.Remove(this._selectedGridRow);
        this.gvScenarioFeeValues.Items.Clear();
        this._selectedGridRow = (GVItem) null;
      }
      else
      {
        this.gvLineScenarios.SelectedIndexChanged -= new EventHandler(this.gvLineScenarios_SelectedIndexChanged);
        this._currentFeeRuleScenario.Deleted = true;
        this._currentFeeRuleScenario = (DDMFeeRuleScenario) null;
        int index = this._selectedGridRow.Index;
        this.gvLineScenarios.Items.Remove(this._selectedGridRow);
        this.ReorderRules(index);
        this.gvLineScenarios.SelectedIndexChanged += new EventHandler(this.gvLineScenarios_SelectedIndexChanged);
      }
    }

    private void RebindScenariosToGrid()
    {
      if (this._feeRule.Scenarios.Count == 0)
        return;
      this._feeRule.Scenarios.Sort();
      this.gvLineScenarios.Items.Clear();
      this._feeRule.Scenarios.ForEach((Action<DDMFeeRuleScenario>) (scenario =>
      {
        scenario.MarkNotDirty();
        GVItem newItem = new GVItem((object) scenario.Order);
        this.BindScenarioToGridView(scenario, newItem);
      }));
    }

    private void ReorderRules(int beginReorderIndex)
    {
      for (int nItemIndex = beginReorderIndex; nItemIndex < this.gvLineScenarios.Items.Count; ++nItemIndex)
      {
        DDMFeeRuleScenario tag = (DDMFeeRuleScenario) this.gvLineScenarios.Items[nItemIndex].Tag;
        tag.Order = nItemIndex + 1;
        tag.OrderChanged = true;
        this.BindScenarioToGridView(tag, this.gvLineScenarios.Items[nItemIndex], false);
      }
    }

    private void stdButtonCopy_Click(object sender, EventArgs e)
    {
      if (this.gvLineScenarios.SelectedItems.Count == 0 || !this.ValidateForm())
        return;
      if (this._currentFeeRuleScenario.Dirty && !this.SaveOrDiscardPendingChanges())
      {
        this.HandleStandardButtons();
        this.SetDetailsTabPage();
        this.SetValueTabPage();
      }
      else
      {
        this.gvLineScenarios.SelectedIndexChanged -= new EventHandler(this.gvLineScenarios_SelectedIndexChanged);
        int num = 1;
        foreach (GVItem selectedItem in this.gvLineScenarios.SelectedItems)
          num = selectedItem.Index + 1;
        DDMFeeRuleScenario newFeeRuleScenario = (DDMFeeRuleScenario) this._currentFeeRuleScenario.Clone();
        ++this._maxOrder;
        newFeeRuleScenario.Order = num;
        newFeeRuleScenario.OrderChanged = newFeeRuleScenario.ContentChanged = true;
        GVItem newItem = new GVItem((object) newFeeRuleScenario.Order);
        this.BindScenarioToGridView(newFeeRuleScenario, newItem, index: num);
        this._feeRule.Scenarios.Add(newFeeRuleScenario);
        this.ReorderRules(num);
        newItem.Selected = true;
        this.SetupUIForSelectedScenario();
        this.tbxScenarioName.Focus();
        this.tbxScenarioName.SelectAll();
        this.gvLineScenarios.SelectedIndexChanged += new EventHandler(this.gvLineScenarios_SelectedIndexChanged);
      }
    }

    private IEnumerable<DDMFeeRuleValue> orderFields(List<DDMFeeRuleValue> feeRuleValues)
    {
      Dictionary<string, int> fieldOrder = new Dictionary<string, int>();
      string key1 = this._feeRule.FeeLine;
      if (key1 == "803x" || key1 == "1101x")
        key1 = key1.Substring(0, key1.Length - 1);
      if (FeeScenarioRules.fieldOrderCache.ContainsKey(key1))
      {
        fieldOrder = FeeScenarioRules.fieldOrderCache[key1];
        return (IEnumerable<DDMFeeRuleValue>) feeRuleValues.OrderBy<DDMFeeRuleValue, int>((Func<DDMFeeRuleValue, int>) (x => fieldOrder.ContainsKey(x.FieldID) ? fieldOrder[x.FieldID] : fieldOrder.Count<KeyValuePair<string, int>>()));
      }
      try
      {
        Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("EllieMae.EMLite.Resources.DDMFeeLineOrder.feeline_" + key1 + ".csv");
        int num = 0;
        using (StreamReader streamReader = new StreamReader(manifestResourceStream))
        {
          streamReader.ReadLine();
          while (!streamReader.EndOfStream)
          {
            string key2 = streamReader.ReadLine().Trim();
            if (!(key2 == "") && !(key2 == "NA") && !fieldOrder.ContainsKey(key2))
            {
              fieldOrder[key2] = num;
              ++num;
            }
          }
        }
        FeeScenarioRules.fieldOrderCache[key1] = fieldOrder;
        return (IEnumerable<DDMFeeRuleValue>) feeRuleValues.OrderBy<DDMFeeRuleValue, int>((Func<DDMFeeRuleValue, int>) (x => fieldOrder.ContainsKey(x.FieldID) ? fieldOrder[x.FieldID] : fieldOrder.Count<KeyValuePair<string, int>>()));
      }
      catch (Exception ex)
      {
        Tracing.Log(FeeScenarioRules._sw, this._className, TraceLevel.Error, ex.ToString());
        return (IEnumerable<DDMFeeRuleValue>) feeRuleValues;
      }
    }

    private void BindScenarioValuesToGridView(DDMFeeRuleScenario newFeeRuleScenario)
    {
      this.gvScenarioFeeValues.Items.Clear();
      if (newFeeRuleScenario != null && newFeeRuleScenario.FeeRuleValues != null)
      {
        if (!this.fromChkFilter)
          this.chkFeeNotAllowed.Checked = newFeeRuleScenario.FeeNotAllowed;
        this.EnableDisableScenarioFeeValuesGridView();
        foreach (DDMFeeRuleValue orderField in this.orderFields(new HashSet<DDMFeeRuleValue>((IEnumerable<DDMFeeRuleValue>) this.GetFilteredFeeRuleValues(newFeeRuleScenario, this.chkShowFieldsValue.Checked), (IEqualityComparer<DDMFeeRuleValue>) new FeeScenarioRules.FieldIDComparer()).ToList<DDMFeeRuleValue>()))
        {
          GVItem gvItem = new GVItem(orderField.Field_Name.ToString());
          gvItem.SubItems.Add((object) orderField.FieldID.ToString());
          gvItem.SubItems.Add((object) this.GetFieldValueTypefromEnum(orderField.Field_Value_Type));
          string fieldValue = orderField.Field_Value;
          if (!string.IsNullOrEmpty(fieldValue) && (orderField.Field_Value_Type == fieldValueType.Table || orderField.Field_Value_Type == fieldValueType.SystemTable))
          {
            string[] strArray = fieldValue.Split('|');
            if (strArray.Length > 1)
              fieldValue = strArray[1];
          }
          gvItem.SubItems.Add((object) fieldValue);
          gvItem.Tag = (object) orderField;
          this.gvScenarioFeeValues.Items.Add(gvItem);
        }
      }
      else
        this.gvScenarioFeeValues.Items.Clear();
    }

    private List<DDMFeeRuleValue> GetFilteredFeeRuleValues(
      DDMFeeRuleScenario newFeeRuleScenario,
      bool isValueType)
    {
      return isValueType ? newFeeRuleScenario.FeeRuleValues.Where<DDMFeeRuleValue>((Func<DDMFeeRuleValue, bool>) (x => x.Field_Value_Type != fieldValueType.ValueNotSet)).ToList<DDMFeeRuleValue>() : newFeeRuleScenario.FeeRuleValues;
    }

    private string GetFieldValueTypefromEnum(fieldValueType val)
    {
      switch (val)
      {
        case fieldValueType.ValueNotSet:
          return "No Value Set";
        case fieldValueType.SpecificValue:
        case fieldValueType.FeeManagement:
          return "Specific Value";
        case fieldValueType.Table:
        case fieldValueType.SystemTable:
          return "Table";
        case fieldValueType.Calculation:
          return "Calculation";
        case fieldValueType.ClearValueInLoanFile:
          return "Clear value in loan file";
        case fieldValueType.UseCalculatedValue:
          return "Use Calculated Value";
        default:
          return "";
      }
    }

    private void stdButtonNew_Click(object sender, EventArgs e)
    {
      if (this._currentFeeRuleScenario != null)
        this.SyncUIDetailsToScenario();
      if (!this.ValidateForm())
        return;
      this.gvLineScenarios.SelectedIndexChanged -= new EventHandler(this.gvLineScenarios_SelectedIndexChanged);
      this.chkFeeNotAllowed.CheckedChanged -= new EventHandler(this.chkFeeNotAllowed_CheckedChanged);
      this.CreateNewFeeScenario(this._feeRule.RuleID);
      this.SetupUIForSelectedScenario();
      this.tbxScenarioName.Focus();
      this.tbxScenarioName.SelectAll();
      this.chkFeeNotAllowed.CheckedChanged += new EventHandler(this.chkFeeNotAllowed_CheckedChanged);
      this.gvLineScenarios.SelectedIndexChanged += new EventHandler(this.gvLineScenarios_SelectedIndexChanged);
    }

    private string BuildGlobalConditionString()
    {
      string str = "";
      if (this._feeRule.InitLESent)
        str += "Initial LE Sent";
      if (this._feeRule.Condition)
      {
        if (str != "")
          str += " | ";
        str += this._feeRule.ConditionState.Trim();
      }
      return str == "" ? "No Condition" : str.Trim();
    }

    private void BindScenarioToGridView(
      DDMFeeRuleScenario newFeeRuleScenario,
      GVItem newItem,
      bool newScenario = true,
      int index = 0)
    {
      if (newScenario)
      {
        newItem.SubItems.Add((object) newFeeRuleScenario.RuleName);
        string str1 = this.BuildConditionString((BizRuleInfo) newFeeRuleScenario);
        newItem.SubItems.Add((object) EffectiveDateControl.GetDisplayString(newFeeRuleScenario.EffectiveDateInfo));
        newItem.SubItems.Add(string.IsNullOrEmpty(str1) ? (object) "No Condition" : (object) str1);
        newItem.SubItems.Add((object) this.BuildGlobalConditionString());
        string str2 = this.BuildChannelString((BizRuleInfo) newFeeRuleScenario);
        newItem.SubItems.Add(string.IsNullOrEmpty(str2) ? (object) "No Channel" : (object) str2);
        newItem.SubItems.Add((object) newFeeRuleScenario.Status);
        newItem.SubItems.Add((object) newFeeRuleScenario.LastModifiedByUserInfo);
        newItem.SubItems.Add(newFeeRuleScenario.LastModTime == DateTime.MinValue ? (object) "" : (object) newFeeRuleScenario.LastModTime.ToString("MM/dd/yyyy hh:mm tt"));
        newItem.Tag = (object) newFeeRuleScenario;
        if (newFeeRuleScenario.RuleID == 0)
          this.gvLineScenarios.Items.Insert(index, newItem);
        else
          this.gvLineScenarios.Items.Add(newItem);
      }
      else
      {
        newItem.SubItems[0].Text = Convert.ToString(newFeeRuleScenario.Order);
        newItem.SubItems[1].Text = newFeeRuleScenario.RuleName;
        newItem.SubItems[2].Text = EffectiveDateControl.GetDisplayString(newFeeRuleScenario.EffectiveDateInfo);
        string str3 = this.BuildConditionString((BizRuleInfo) newFeeRuleScenario);
        newItem.SubItems[3].Text = string.IsNullOrEmpty(str3) ? "No Condition" : str3;
        newItem.SubItems[4].Text = this.BuildGlobalConditionString();
        string str4 = this.BuildChannelString((BizRuleInfo) newFeeRuleScenario);
        newItem.SubItems[5].Text = string.IsNullOrEmpty(str4) ? "No Channel" : str4;
        newItem.SubItems[6].Text = Convert.ToString((object) newFeeRuleScenario.Status);
        newItem.SubItems[7].Text = newFeeRuleScenario.LastModifiedByUserInfo;
        newItem.SubItems[8].Text = newFeeRuleScenario.LastModTime == DateTime.MinValue ? "" : newFeeRuleScenario.LastModTime.ToString("MM/dd/yyyy hh:mm tt");
        newItem.Tag = (object) newFeeRuleScenario;
      }
      this.BindScenarioValuesToGridView(newFeeRuleScenario);
    }

    private void btnDeactivate_Click(object sender, EventArgs e)
    {
      if (this.gvLineScenarios.SelectedItems.Count == 0 || !this.SaveOrDiscardPendingChanges())
        return;
      if (this._currentFeeRuleScenario.Status == BizRule.RuleStatus.Active)
      {
        if (this._feeRule.Scenarios.Where<DDMFeeRuleScenario>((Func<DDMFeeRuleScenario, bool>) (s => s.Status == BizRule.RuleStatus.Active)).Count<DDMFeeRuleScenario>() == 1)
          this.activeDeActiveScenarioId = this._currentFeeRuleScenario.RuleID;
        if (this._feeScenariosBpmManager.DeactivateRule(this._currentFeeRuleScenario.RuleID, this.isGlobalSetting, true) != BizRule.ActivationReturnCode.Succeed)
          throw new Exception("Persisting business rule failed");
        this._currentFeeRuleScenario.Status = BizRule.RuleStatus.Inactive;
        this.btnDeactivate.Text = "Activate";
        --this.activeCount;
      }
      else
      {
        if (this._feeRule.Scenarios.Where<DDMFeeRuleScenario>((Func<DDMFeeRuleScenario, bool>) (s => s.Status == BizRule.RuleStatus.Active)).Count<DDMFeeRuleScenario>() == 0)
          this.activeDeActiveScenarioId = this._currentFeeRuleScenario.RuleID;
        if (this._feeScenariosBpmManager.ActivateRule(this._currentFeeRuleScenario.RuleID, this.isGlobalSetting, true) != BizRule.ActivationReturnCode.Succeed)
          throw new Exception("Persisting business rule failed");
        this._currentFeeRuleScenario.Status = BizRule.RuleStatus.Active;
        this.btnDeactivate.Text = "Deactivate";
        ++this.activeCount;
      }
      this.MarkChangeToActivateOrDeactivateScenarios();
      this._currentFeeRuleScenario.MarkNotDirty();
      this.HandleStandardButtons();
    }

    private void MarkChangeToActivateOrDeactivateScenarios()
    {
      this.anyChangeToActivateOrDeactivateScenarios = true;
      this._currentFeeRuleScenario.LastModTime = DateTime.Now;
      this._currentFeeRuleScenario.LastModifiedByFullName = this._session.UserInfo.FullName;
      this._currentFeeRuleScenario.LastModifiedByUserId = this._session.UserInfo.Userid;
      this.SyncUIDetailsToScenario();
      this.updateModificationInfoForParentRule();
    }

    private void stdButtonSave_Click(object sender, EventArgs e)
    {
      int nItemIndex = -1;
      if (this.gvLineScenarios.SelectedItems != null && this.gvLineScenarios.SelectedItems.Count > 0)
        nItemIndex = this.gvLineScenarios.SelectedItems[0].Index;
      if (this.PendingChanges() == 0 || !this.SaveMultipleScenarios())
        return;
      if (nItemIndex > 0 && this.gvLineScenarios.Items.Count > 0)
      {
        if (nItemIndex >= this.gvLineScenarios.Items.Count)
          this.gvLineScenarios.Items[this.gvLineScenarios.Items.Count - 1].Selected = true;
        else
          this.gvLineScenarios.Items[nItemIndex].Selected = true;
      }
      if (this.gvLineScenarios.Items.Count != 0)
        return;
      this.stdButtonNew_Click((object) null, (EventArgs) null);
    }

    private bool SaveMultipleScenarios()
    {
      if (this._currentFeeRuleScenario != null)
        this.SyncUIDetailsToScenario();
      if (!this.ValidateForm())
        return false;
      this.invalidatePreviousGlobalSettingState();
      foreach (DDMFeeRuleScenario ddmFeeRuleScenario in this._feeRule.Scenarios.Where<DDMFeeRuleScenario>((Func<DDMFeeRuleScenario, bool>) (scenario => scenario.Dirty)))
      {
        ddmFeeRuleScenario.LastModifiedByFullName = this._session.UserInfo.FullName;
        ddmFeeRuleScenario.LastModifiedByUserId = this._session.UserInfo.Userid;
      }
      this._feeScenariosBpmManager.UpdateRules(this._feeRule.Scenarios.Where<DDMFeeRuleScenario>((Func<DDMFeeRuleScenario, bool>) (scenario => scenario.Dirty)).ToList<DDMFeeRuleScenario>(), this.isGlobalSetting, true);
      this.updateModificationInfoForParentRule();
      this._feeRulesBpmMgr.UpdateDDMFeeRuleByID(this._feeRule.RuleID, this._feeRule, isGlobalSetting: this.isGlobalSetting);
      this.isGlobalSetting = false;
      this.GetRulesFromDatabase();
      return true;
    }

    private void updateModificationInfoForParentRule()
    {
      this._feeRule.LastModByFullName = this._session.UserInfo.FullName;
      this._feeRule.LastModByUserID = this._session.UserInfo.Userid;
      this._feeRule.UpdateDt = DateTime.Now.ToString();
    }

    private bool CheckDuplicateRules(List<DDMFeeRuleScenario> scenarios)
    {
      if (scenarios == null)
        return true;
      HashSet<string> stringSet = new HashSet<string>();
      foreach (DDMFeeRuleScenario scenario in scenarios)
      {
        if (!scenario.Deleted)
        {
          if (stringSet.Contains(scenario.RuleName))
            return false;
          stringSet.Add(scenario.RuleName);
        }
      }
      return true;
    }

    private void SyncUIDetailsToScenario()
    {
      DDMFeeRuleScenario ddmFeeRuleScenario = new DDMFeeRuleScenario(this.tbxScenarioName.Text.Trim());
      ddmFeeRuleScenario.RuleID = this._currentFeeRuleScenario.RuleID;
      ddmFeeRuleScenario.Order = this._currentFeeRuleScenario.Order;
      ddmFeeRuleScenario.Status = this._currentFeeRuleScenario.Status;
      ddmFeeRuleScenario.EffectiveDateInfo = this.effectiveDateControl.ToString();
      ddmFeeRuleScenario.FeeRuleID = this._currentFeeRuleScenario.FeeRuleID;
      ddmFeeRuleScenario.FeeRuleValues = this._currentFeeRuleScenario.FeeRuleValues;
      ddmFeeRuleScenario.LastModifiedByFullName = this._currentFeeRuleScenario.LastModifiedByFullName;
      ddmFeeRuleScenario.LastModifiedByUserId = this._currentFeeRuleScenario.LastModifiedByUserId;
      ddmFeeRuleScenario.CommentsTxt = this.tbxNotesComments.Text;
      this.ruleConditonControl.ApplyCondition((BizRuleInfo) ddmFeeRuleScenario);
      ddmFeeRuleScenario.Condition2 = this.channelControl.ChannelValue;
      ddmFeeRuleScenario.FeeRuleValues = this._currentFeeRuleScenario.FeeRuleValues;
      ddmFeeRuleScenario.ContentChanged = this._currentFeeRuleScenario.ContentChanged;
      ddmFeeRuleScenario.OrderChanged = this._currentFeeRuleScenario.OrderChanged;
      ddmFeeRuleScenario.FeeNotAllowed = this.chkFeeNotAllowed.Checked;
      ddmFeeRuleScenario.ParentRuleName = this._feeRule.RuleName;
      ddmFeeRuleScenario.LastModTime = this._currentFeeRuleScenario.LastModTime;
      this.BindScenarioToGridView(ddmFeeRuleScenario, this._selectedGridRow, false);
      this._feeRule.Scenarios.Remove(this._currentFeeRuleScenario);
      this._currentFeeRuleScenario = ddmFeeRuleScenario;
      this._feeRule.Scenarios.Add(this._currentFeeRuleScenario);
    }

    private bool ValidateForm()
    {
      if (this._currentFeeRuleScenario == null)
        return true;
      if (!this.ruleConditonControl.ValidateCondition())
        return false;
      if (!this.CheckDuplicateRules(this._feeRule.Scenarios))
      {
        int num = (int) MessageBox.Show((IWin32Window) this, "A scenario with same name exists. Please use a different name", "Duplicate Scenario Name", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (string.IsNullOrEmpty(this.tbxScenarioName.Text.Trim()))
      {
        int num = (int) MessageBox.Show((IWin32Window) this, "Please enter a name for the scenario name", "Scenario Name Required", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      bool flag = false;
      if (this.effectiveDateControl.EffectiveDateFieldId == "Please select" && this.effectiveDateControl.Operator == "Please select")
      {
        if (this.effectiveDateControl.StartDate != DateTime.MinValue || this.effectiveDateControl.EndDate != DateTime.MinValue)
          flag = true;
      }
      else if (!(this.effectiveDateControl.EffectiveDateFieldId != "Please select") || !(this.effectiveDateControl.Operator != "Please select"))
        flag = true;
      else if (!this.effectiveDateControl.Operator.Equals("Blank") && !this.effectiveDateControl.Operator.Equals("Blank>="))
      {
        if (this.effectiveDateControl.StartDate == DateTime.MinValue)
          flag = true;
        if (!flag && this.effectiveDateControl.Operator.Equals("Between"))
        {
          if (this.effectiveDateControl.StartDate == DateTime.MinValue || this.effectiveDateControl.EndDate == DateTime.MinValue)
            flag = true;
          if (this.effectiveDateControl.StartDate > this.effectiveDateControl.EndDate)
            flag = true;
        }
      }
      if (!flag)
        return true;
      int num1 = (int) Utils.Dialog((IWin32Window) this, "Effective Date criteria is incomplete or Start Date is greater than End Date. Please review.");
      return false;
    }

    private int PendingChanges()
    {
      return this._feeRule.Scenarios.Where<DDMFeeRuleScenario>((Func<DDMFeeRuleScenario, bool>) (scenario => scenario.Dirty)).Count<DDMFeeRuleScenario>();
    }

    private int PendingContentChanges()
    {
      return this._feeRule.Scenarios.Where<DDMFeeRuleScenario>((Func<DDMFeeRuleScenario, bool>) (scenario => scenario.ContentChanged)).Count<DDMFeeRuleScenario>();
    }

    private int PendingDeletes()
    {
      return this._feeRule.Scenarios.Where<DDMFeeRuleScenario>((Func<DDMFeeRuleScenario, bool>) (scenario => scenario.Deleted)).Count<DDMFeeRuleScenario>();
    }

    private void stdButtonReset_Click(object sender, EventArgs e)
    {
      this.isReset = true;
      if (this.PendingChanges() == 0)
        return;
      this.restorePreviousGlobalSettingState();
      this.GetRulesFromDatabase();
      if (this._feeRule.Scenarios.Count == 0)
        this.createNewEmptyScenario();
      this.ResetScenarioHeader();
      this.ResetMaxOrder();
      this.isReset = false;
    }

    private void GetRulesFromDatabase()
    {
      this.gvLineScenarios.SelectedIndexChanged -= new EventHandler(this.gvLineScenarios_SelectedIndexChanged);
      this.chkFeeNotAllowed.CheckedChanged -= new EventHandler(this.chkFeeNotAllowed_CheckedChanged);
      this._feeRule.Scenarios = this._feeScenariosBpmManager.GetAllRules(this._feeRule.RuleID, true);
      this.gvLineScenarios.Items.Clear();
      this.gvLineScenarios.SelectedItems.Clear();
      this._feeRule.Scenarios.ForEach((Action<DDMFeeRuleScenario>) (scenario =>
      {
        scenario.ParentRuleName = this._feeRule.RuleName;
        GVItem newItem = new GVItem((object) scenario.Order);
        this.BindScenarioToGridView(scenario, newItem);
      }));
      if (this.gvLineScenarios.Items.Count != 0)
      {
        this.gvLineScenarios.Items[0].Selected = true;
        this._currentFeeRuleScenario = (DDMFeeRuleScenario) this.gvLineScenarios.Items[0].Tag;
        this._selectedGridRow = this.gvLineScenarios.Items[0];
      }
      this.gvLineScenarios.SelectedIndexChanged += new EventHandler(this.gvLineScenarios_SelectedIndexChanged);
      this.SetupUIForSelectedScenario();
      this.chkFeeNotAllowed.CheckedChanged += new EventHandler(this.chkFeeNotAllowed_CheckedChanged);
    }

    private void tbxScenarioName_TextChanged(object sender, EventArgs e) => this.MarkDirty();

    private void MarkDirty()
    {
      if (this.gvLineScenarios.SelectedItems.Count == 0)
        return;
      this._currentFeeRuleScenario.ContentChanged = true;
      this.HandleStandardButtons();
    }

    private void MarkAllDirty()
    {
      foreach (DDMFeeRuleScenario scenario in this._feeRule.Scenarios)
        scenario.ContentChanged = true;
      this.HandleStandardButtons();
    }

    private void tbxNotesComments_TextChanged(object sender, EventArgs e) => this.MarkDirty();

    private void FeeScenarioRules_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this.PendingChanges() > 0)
      {
        switch (MessageBox.Show((IWin32Window) this, "There are unsaved changes to one or more scenarios. Do you want to save them?", "Save Changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
        {
          case DialogResult.Cancel:
            e.Cancel = true;
            break;
          case DialogResult.Yes:
            if (this.SaveMultipleScenarios())
              break;
            e.Cancel = true;
            break;
        }
      }
      else
      {
        if (!this.anyChangeToActivateOrDeactivateScenarios)
          return;
        this._feeRulesBpmMgr.UpdateDDMFeeRuleByID(this._feeRule.RuleID, this._feeRule);
      }
    }

    private void btnCancel_Click(object sender, EventArgs e) => this.Close();

    private void handleAffectedFields(string affectingField)
    {
      Dictionary<string, object> sysTableWithAction = DDM_FieldAccess_Utils.GetAffectedFieldsDictBySysTableWithAction(affectingField);
      if (sysTableWithAction == null || sysTableWithAction.Count <= 0)
        return;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvScenarioFeeValues.Items)
      {
        DDMFeeRuleValue tag = (DDMFeeRuleValue) gvItem.Tag;
        if (sysTableWithAction.Keys.Contains<string>(tag.FieldID) && (!(sysTableWithAction[tag.FieldID] is string str) || !str.Equals("R")))
        {
          if (str != null && str.Equals("C"))
          {
            tag.Field_Value_Type = fieldValueType.ClearValueInLoanFile;
            gvItem.SubItems[2].Text = this.GetFieldValueTypefromEnum(fieldValueType.ClearValueInLoanFile);
          }
          else
          {
            tag.Field_Value_Type = fieldValueType.ValueNotSet;
            gvItem.SubItems[2].Text = this.GetFieldValueTypefromEnum(fieldValueType.ValueNotSet);
          }
          tag.Field_Value = string.Empty;
          gvItem.SubItems[3].Text = string.Empty;
        }
      }
    }

    private void btnSetValue_Click(object sender, EventArgs e)
    {
      if (this.gvScenarioFeeValues != null && this.gvScenarioFeeValues.SelectedItems.Count <= 0)
        return;
      DDMFeeRuleValue tag1 = (DDMFeeRuleValue) this.gvScenarioFeeValues.SelectedItems[0].Tag;
      DDMFeeRuleValue ddmFeeRuleValue1 = new DDMFeeRuleValue()
      {
        Field_Name = tag1.Field_Name,
        Field_Value_Type = tag1.Field_Value_Type,
        Field_Value = tag1.Field_Value
      };
      using (FeeValueDlg dlg = new FeeValueDlg(tag1, this._feeRule.FeeLine, this._session))
      {
        dlg.ShowInTaskbar = false;
        int num1 = (int) dlg.ShowDialog((IWin32Window) this);
        if (dlg.DialogResult == DialogResult.OK)
        {
          try
          {
            string str1 = Convert.ToString(dlg.feeRuleValue.Field_Value);
            if (!string.IsNullOrEmpty(str1) && (dlg.feeRuleValue.Field_Value_Type == fieldValueType.Table || dlg.feeRuleValue.Field_Value_Type == fieldValueType.SystemTable))
            {
              string[] strArray = str1.Split('|');
              if (strArray.Length > 1)
                str1 = strArray[1];
            }
            DDMFeeRuleValue ddmFeeRuleValue2 = this._currentFeeRuleScenario.FeeRuleValues.Where<DDMFeeRuleValue>((Func<DDMFeeRuleValue, bool>) (x => x.FieldID == dlg.feeRuleValue.FieldID)).FirstOrDefault<DDMFeeRuleValue>();
            if (ddmFeeRuleValue2 != null)
            {
              if (dlg.feeRuleValue.Field_Value_Type == fieldValueType.SystemTable)
                this.handleAffectedFields(dlg.feeRuleValue.FieldID);
              else if (this._feeRule.FeeLine == "902" && dlg.feeRuleValue.FieldID == "3533")
              {
                if (dlg.feeRuleValue.Field_Value_Type == fieldValueType.SpecificValue && dlg.feeRuleValue.Field_Value == "Y")
                  this.handleAffectedFields("3533");
              }
              else if (dlg.feeRuleValue.Field_Value_Type != fieldValueType.ValueNotSet && dlg.feeRuleValue.Field_Value_Type != fieldValueType.UseCalculatedValue && dlg.feeRuleValue.Field_Value_Type != fieldValueType.none)
              {
                bool flag = false;
                if (DDM_FieldAccess_Utils.isSpecialFeeLine(this._feeRule.FeeLine))
                {
                  flag = true;
                  string str2 = this.affectedFieldHasSystemTableAssigned(dlg.feeRuleValue.FieldID);
                  if (str2 != null)
                  {
                    int num2 = (int) Utils.Dialog((IWin32Window) this, string.Format("System table is associated to field ID {0}. Please try setting a value for field ID {1} after you change the value setting for field ID {0}.", (object) str2, (object) dlg.feeRuleValue.FieldID), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    ddmFeeRuleValue2.Field_Value_Type = ddmFeeRuleValue1.Field_Value_Type;
                    ddmFeeRuleValue2.Field_Value = ddmFeeRuleValue1.Field_Value;
                    return;
                  }
                }
                if (!flag)
                  this.handleAffectedFields(dlg.feeRuleValue.FieldID);
                Dictionary<string, SysTblInfluenceAction> dictByAnyWithAction = DDM_FieldAccess_Utils.GetAffectingFieldsDictByAnyWithAction(ddmFeeRuleValue2.FieldID);
                if (!flag && dictByAnyWithAction != null && dictByAnyWithAction.Count > 0)
                {
                  foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvScenarioFeeValues.Items)
                  {
                    DDMFeeRuleValue tag2 = (DDMFeeRuleValue) gvItem.Tag;
                    if (dictByAnyWithAction.Keys.Contains<string>(tag2.FieldID) && tag2.Field_Value_Type != fieldValueType.ValueNotSet && tag2.Field_Value_Type != fieldValueType.none)
                    {
                      SysTblInfluenceAction tblInfluenceAction = dictByAnyWithAction[tag2.FieldID];
                      string empty = string.Empty;
                      MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                      string text;
                      if (tblInfluenceAction != null)
                      {
                        if (tblInfluenceAction.Action.Equals("C"))
                        {
                          if (tag2.Field_Value_Type == fieldValueType.SystemTable)
                            text = string.Format("You have chosen to set Field {0} with a system table which conflict with field {1} and will be reset.", (object) tag2.FieldID, (object) ddmFeeRuleValue2.FieldID);
                          else if (tblInfluenceAction.IsNonSystemTableType)
                            text = string.Format("You have chosen to set Field {0} which conflict with field {1} and will be reset. Do you still want to set this setting?", (object) tag2.FieldID, (object) ddmFeeRuleValue2.FieldID);
                          else
                            break;
                        }
                        else if (tblInfluenceAction.Action.Equals("R"))
                        {
                          if (tag2.Field_Value_Type != fieldValueType.UseCalculatedValue)
                          {
                            if (tblInfluenceAction.IsNonSystemTableType)
                              text = string.Format("You have chosen to set Field {0} which may override field {1}. Do you still want to set this setting?", (object) tag2.FieldID, (object) ddmFeeRuleValue2.FieldID);
                            else
                              break;
                          }
                          else
                            break;
                        }
                        else
                          break;
                      }
                      else if (tag2.Field_Value_Type == fieldValueType.SystemTable)
                        text = string.Format("You have chosen to set Field {0} with a system table which could result in this value being overwritten.", (object) tag2.FieldID);
                      else if (tblInfluenceAction.IsNonSystemTableType)
                        text = string.Format("You have chosen to set Field {0} which may overwrite field {1}. Do you still want to set this setting?", (object) tag2.FieldID, (object) ddmFeeRuleValue2.FieldID);
                      else
                        break;
                      if (tag2.Field_Value_Type == fieldValueType.SystemTable)
                        buttons = MessageBoxButtons.OK;
                      if (Utils.Dialog((IWin32Window) this, text, buttons, MessageBoxIcon.Asterisk) != DialogResult.Yes)
                      {
                        ddmFeeRuleValue2.Field_Value_Type = ddmFeeRuleValue1.Field_Value_Type;
                        ddmFeeRuleValue2.Field_Value = ddmFeeRuleValue1.Field_Value;
                        return;
                      }
                      if (tblInfluenceAction != null && tblInfluenceAction.Equals((object) "C"))
                      {
                        tag2.Field_Value_Type = fieldValueType.ValueNotSet;
                        tag2.Field_Value = string.Empty;
                        gvItem.SubItems[2].Text = this.GetFieldValueTypefromEnum(fieldValueType.ValueNotSet);
                        gvItem.SubItems[3].Text = string.Empty;
                      }
                    }
                  }
                }
              }
              string[] strArray = DDM_FieldAccess_Utils.ValidateSellerObligated(this._feeRule.FeeLine, dlg.feeRuleValue.FieldID, this._currentFeeRuleScenario);
              string str3 = DDM_FieldAccess_Utils.HandleTaxes(this._feeRule.FeeLine, dlg.feeRuleValue.FieldID, this._currentFeeRuleScenario);
              bool flag1 = false;
              if (strArray != null || str3 != string.Empty)
              {
                foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvScenarioFeeValues.Items)
                {
                  DDMFeeRuleValue tag3 = (DDMFeeRuleValue) gvItem.Tag;
                  if (strArray[1] == tag3.FieldID && strArray[0] == HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT.ToString())
                  {
                    int num3 = (int) Utils.Dialog((IWin32Window) this, string.Format("Setting {0} Seller Obligated Checkbox value to 'N' will set {1} to 'Clear value in loan file'", (object) strArray[2], (object) strArray[1]), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    tag3.Field_Value_Type = fieldValueType.ClearValueInLoanFile;
                    tag3.Field_Value = string.Empty;
                    gvItem.SubItems[2].Text = this.GetFieldValueTypefromEnum(fieldValueType.ClearValueInLoanFile);
                    gvItem.SubItems[3].Text = string.Empty;
                    break;
                  }
                  if (strArray[1] == tag3.FieldID && strArray[0] == HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATED.ToString())
                  {
                    int num4 = (int) Utils.Dialog((IWin32Window) this, string.Format("Setting a value for {0} Seller Obligated Amount will set the {1} Seller Obligated Checkbox to 'Y'", (object) strArray[2], (object) strArray[1]), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    tag3.Field_Value_Type = fieldValueType.SpecificValue;
                    tag3.Field_Value = "Y";
                    gvItem.SubItems[2].Text = this.GetFieldValueTypefromEnum(fieldValueType.SpecificValue);
                    gvItem.SubItems[3].Text = "Y";
                    break;
                  }
                  if (str3 != string.Empty)
                  {
                    flag1 = true;
                    DDMFeeRuleValue tag4 = (DDMFeeRuleValue) gvItem.Tag;
                    if (str3 == tag4.FieldID)
                    {
                      tag4.Field_Value_Type = fieldValueType.ClearValueInLoanFile;
                      tag4.Field_Value = string.Empty;
                      gvItem.SubItems[2].Text = this.GetFieldValueTypefromEnum(fieldValueType.ClearValueInLoanFile);
                      gvItem.SubItems[3].Text = string.Empty;
                    }
                  }
                }
              }
              if (flag1)
              {
                int num5 = (int) Utils.Dialog((IWin32Window) this, string.Format("Setting 'Y' value for {0} will set {1} to 'Clear value in loan file", (object) dlg.feeRuleValue.FieldID, (object) str3), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
              }
              this.gvScenarioFeeValues.SelectedItems[0].Tag = (object) dlg.feeRuleValue;
              if (strArray != null && strArray[1] != dlg.feeRuleValue.FieldID && (str3 == null || !str3.Contains(dlg.feeRuleValue.FieldID)))
              {
                this.gvScenarioFeeValues.SelectedItems[0].SubItems[2].Text = this.GetFieldValueTypefromEnum(dlg.feeRuleValue.Field_Value_Type);
                this.gvScenarioFeeValues.SelectedItems[0].SubItems[3].Text = str1;
              }
              DDMFeeRuleValue feeRuleValue = dlg.feeRuleValue;
              this._currentFeeRuleScenario.ContentChanged = true;
              this.RemoveCurrentRowBasedOnFilter(dlg.feeRuleValue.Field_Value_Type, this.chkShowFieldsValue.Checked);
              this.HandleStandardButtons();
            }
          }
          catch (Exception ex)
          {
          }
        }
        dlg.Dispose();
      }
    }

    private string affectedFieldHasSystemTableAssigned(string fieldId)
    {
      if (fieldId == null)
        return (string) null;
      List<string> fieldsListBySysTable = DDM_FieldAccess_Utils.GetAffectedFieldsListBySysTable(fieldId);
      if (fieldsListBySysTable == null || fieldsListBySysTable.Count == 0)
        return (string) null;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvScenarioFeeValues.Items)
      {
        DDMFeeRuleValue tag = (DDMFeeRuleValue) gvItem.Tag;
        if (fieldsListBySysTable.Contains(tag.FieldID) && tag.Field_Value_Type == fieldValueType.SystemTable)
          return tag.FieldID;
      }
      return (string) null;
    }

    private void RemoveCurrentRowBasedOnFilter(fieldValueType fieldValueType, bool isOnlyValues)
    {
      if (!(fieldValueType == fieldValueType.ValueNotSet & isOnlyValues))
        return;
      this.gvScenarioFeeValues.Items.Remove(this.gvScenarioFeeValues.SelectedItems[0]);
    }

    private void tabControlRuleDetails_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.tabControlRuleDetails.SelectedPage == this.tpDetails)
      {
        this._gcScenarioRuleDetailsTextFormat = "{0} Details";
        if (this._currentFeeRuleScenario != null && this._currentFeeRuleScenario.RuleID == 0)
        {
          this.tbxScenarioName.Focus();
          this.tbxScenarioName.SelectAll();
        }
      }
      if (this.tabControlRuleDetails.SelectedPage == this.tpValues)
        this._gcScenarioRuleDetailsTextFormat = "{0} Values";
      this.SetDetailsSectionTitle();
    }

    private void btnClearValue_Click(object sender, EventArgs e)
    {
      if (this.gvScenarioFeeValues != null && this.gvScenarioFeeValues.SelectedItems.Count <= 0)
        return;
      DDMFeeRuleValue feerule = (DDMFeeRuleValue) this.gvScenarioFeeValues.SelectedItems[0].Tag;
      feerule.Field_Value_Type = fieldValueType.ValueNotSet;
      feerule.Field_Value = "";
      this.gvScenarioFeeValues.SelectedItems[0].SubItems[2].Text = DDMFeeRuleValue.typeToValueTable[feerule.Field_Value_Type];
      this.gvScenarioFeeValues.SelectedItems[0].SubItems[3].Text = feerule.Field_Value;
      if (this._currentFeeRuleScenario.FeeRuleValues.Where<DDMFeeRuleValue>((Func<DDMFeeRuleValue, bool>) (x => x.FieldID == feerule.FieldID)).FirstOrDefault<DDMFeeRuleValue>() == null)
        return;
      DDMFeeRuleValue ddmFeeRuleValue = feerule;
      this._currentFeeRuleScenario.ContentChanged = true;
      this.HandleStandardButtons();
    }

    private void btnGlobalRuleSettings_Click(object sender, EventArgs e)
    {
      this.cachePreviousGlobalSettingState();
      using (FeeRulesDlg feeRulesDlg = new FeeRulesDlg(this._currentFeeRuleScenario.FeeRuleID, this._session))
      {
        feeRulesDlg.ShowInTaskbar = false;
        int num = (int) feeRulesDlg.ShowDialog((IWin32Window) this);
        if (feeRulesDlg.DialogResult == DialogResult.OK)
        {
          this.isGlobalSetting = true;
          try
          {
            this.Text = "Modify Fee Scenario";
            this._feeRule.RuleName = feeRulesDlg.DDMFeeRule.RuleName;
            this._feeRule.Condition = feeRulesDlg.DDMFeeRule.Condition;
            this._feeRule.ConditionState = feeRulesDlg.DDMFeeRule.ConditionState;
            this._feeRule.InitLESent = feeRulesDlg.DDMFeeRule.InitLESent;
            this._feeRule.AdvCodeConditionXML = feeRulesDlg.DDMFeeRule.AdvCodeConditionXML;
            if (this._feeRule.FeeLine != feeRulesDlg.DDMFeeRule.FeeLine)
            {
              this._feeRule.FeeLine = feeRulesDlg.DDMFeeRule.FeeLine;
              this._gcScenariosForLineText = string.Format(this._gcScenariosForLineTextFormat, this._feeRule.FeeType == type.line ? (object) "Line" : (object) "Line Group", (object) this._feeRule.FeeLine);
              this.gcScenariosForLine.Text = this._gcScenariosForLineText.Length > 70 ? this._gcScenariosForLineText.Substring(0, 67) + "..." : this._gcScenariosForLineText;
              this.ttipMessageAltText.Show(this._gcScenariosForLineText, (IWin32Window) this.gcScenariosForLine);
              foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvLineScenarios.Items)
              {
                DDMFeeRuleScenario tag = gvItem.Tag as DDMFeeRuleScenario;
                tag.FeeRuleValues.Clear();
                this.PopulateFeeRuleValueFields(tag);
                this.SetValueTabPage();
              }
            }
            string str = this.BuildGlobalConditionString();
            foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvLineScenarios.Items)
              gvItem.SubItems[4].Text = str;
            this.MarkAllDirty();
            this.FeeRule = this._feeRule;
          }
          catch (Exception ex)
          {
          }
        }
        feeRulesDlg.Dispose();
      }
    }

    private void chkFeeNotAllowed_CheckedChanged(object sender, EventArgs e)
    {
      if (this._currentFeeRuleScenario == null || this._currentFeeRuleScenario.FeeRuleValues == null)
        return;
      this.resetGridValue(this._currentFeeRuleScenario);
      if (this.chkFeeNotAllowed.Checked)
      {
        for (int index = 0; index < this.gvScenarioFeeValues.Items.Count; ++index)
        {
          this.gvScenarioFeeValues.Items[index].SubItems[2].Text = this.GetFieldValueTypefromEnum(fieldValueType.ClearValueInLoanFile);
          this.gvScenarioFeeValues.Items[index].SubItems[3].Text = string.Empty;
          this._currentFeeRuleScenario.FeeRuleValues[index].Field_Value_Type = fieldValueType.ClearValueInLoanFile;
          this._currentFeeRuleScenario.FeeRuleValues[index].Field_Value = string.Empty;
        }
      }
      else
      {
        for (int index = 0; index < this.gvScenarioFeeValues.Items.Count; ++index)
        {
          this.gvScenarioFeeValues.Items[index].SubItems[2].Text = this.GetFieldValueTypefromEnum(fieldValueType.ValueNotSet);
          this.gvScenarioFeeValues.Items[index].SubItems[3].Text = string.Empty;
          this._currentFeeRuleScenario.FeeRuleValues[index].Field_Value_Type = fieldValueType.ValueNotSet;
          this._currentFeeRuleScenario.FeeRuleValues[index].Field_Value = string.Empty;
        }
      }
      this.EnableDisableScenarioFeeValuesGridView();
      if (!this.isReset)
        this._currentFeeRuleScenario.ContentChanged = true;
      this.HandleStandardButtons();
      this.FilterBasedOnShowValueType();
    }

    private void resetGridValue(DDMFeeRuleScenario newFeeRuleScenario)
    {
      this.gvScenarioFeeValues.Items.Clear();
      if (newFeeRuleScenario == null || newFeeRuleScenario.FeeRuleValues == null)
        return;
      foreach (DDMFeeRuleValue feeRuleValue in newFeeRuleScenario.FeeRuleValues)
      {
        GVItem gvItem = new GVItem(feeRuleValue.Field_Name.ToString());
        gvItem.SubItems.Add((object) feeRuleValue.FieldID.ToString());
        gvItem.SubItems.Add((object) this.GetFieldValueTypefromEnum(feeRuleValue.Field_Value_Type));
        string fieldValue = feeRuleValue.Field_Value;
        if (!string.IsNullOrEmpty(fieldValue) && (feeRuleValue.Field_Value_Type == fieldValueType.Table || feeRuleValue.Field_Value_Type == fieldValueType.SystemTable))
        {
          string[] strArray = fieldValue.Split('|');
          if (strArray.Length > 1)
            fieldValue = strArray[1];
        }
        gvItem.SubItems.Add((object) fieldValue);
        gvItem.Tag = (object) feeRuleValue;
        this.gvScenarioFeeValues.Items.Add(gvItem);
      }
    }

    private void FilterBasedOnShowValueType()
    {
      this.fromChkFilter = true;
      this.SetValueTabPage();
      this.fromChkFilter = false;
    }

    private void EnableDisableScenarioFeeValuesGridView()
    {
      if (this.gvScenarioFeeValues.SelectedItems.Count > 0)
        this.gvScenarioFeeValues.SelectedItems[0].Selected = false;
      this.gvScenarioFeeValues.Enabled = !this.chkFeeNotAllowed.Checked;
      if (this.chkFeeNotAllowed.Checked)
      {
        this.gvScenarioFeeValues.BackColor = SystemColors.Control;
        this.gvScenarioFeeValues.ForeColor = SystemColors.GrayText;
      }
      else
      {
        this.gvScenarioFeeValues.BackColor = SystemColors.Window;
        this.gvScenarioFeeValues.ForeColor = SystemColors.ControlText;
      }
    }

    private void chkShowFieldsValue_CheckedChanged(object sender, EventArgs e)
    {
      this.fromChkFilter = true;
      this.SetValueTabPage();
      this.fromChkFilter = false;
    }

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, "Setup\\Fee Rule Scenarios");
    }

    private void middleSaveButton_Click(object sender, EventArgs e)
    {
      this.stdButtonSave_Click(sender, e);
    }

    private void middleResetButton_Click(object sender, EventArgs e)
    {
      this.stdButtonReset_Click(sender, e);
    }

    private void bottomSaveButton_Click(object sender, EventArgs e)
    {
      this.stdButtonSave_Click(sender, e);
    }

    private void cachePreviousGlobalSettingState()
    {
      this._previousFeeRuleBeforeGlobalSettingChange = this._feeRule.shallowClone();
    }

    private void invalidatePreviousGlobalSettingState()
    {
      this._previousFeeRuleBeforeGlobalSettingChange = (DDMFeeRule) null;
    }

    private void restorePreviousGlobalSettingState()
    {
      if (this._previousFeeRuleBeforeGlobalSettingChange == null)
        return;
      this._feeRule.RuleName = this._previousFeeRuleBeforeGlobalSettingChange.RuleName;
      this._feeRule.FeeLine = this._previousFeeRuleBeforeGlobalSettingChange.FeeLine;
      this._feeRule.Condition = this._previousFeeRuleBeforeGlobalSettingChange.Condition;
      this._feeRule.InitLESent = this._previousFeeRuleBeforeGlobalSettingChange.InitLESent;
      this._feeRule.ConditionState = this._previousFeeRuleBeforeGlobalSettingChange.ConditionState;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

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
      GVColumn gvColumn9 = new GVColumn();
      GVColumn gvColumn10 = new GVColumn();
      GVColumn gvColumn11 = new GVColumn();
      GVColumn gvColumn12 = new GVColumn();
      GVColumn gvColumn13 = new GVColumn();
      this.gcScenariosForLine = new GroupContainer();
      this.verticalSeparator1 = new VerticalSeparator();
      this.stdButtonSave = new StandardIconButton();
      this.stdButtonReset = new StandardIconButton();
      this.gvLineScenarios = new GridView();
      this.btnGlobalRuleSettings = new ButtonEx();
      this.btnDeactivate = new ButtonEx();
      this.stdButtonDelete = new StandardIconButton();
      this.stdButtonUp = new StandardIconButton();
      this.stdButtonDown = new StandardIconButton();
      this.stdButtonCopy = new StandardIconButton();
      this.stdButtonNew = new StandardIconButton();
      this.lblHeaderText = new FormattedLabel();
      this.gcScenarioRuleDetails = new GroupContainer();
      this.middleSaveButton = new StandardIconButton();
      this.tabControlRuleDetails = new TabControlEx();
      this.tpDetails = new TabPageEx();
      this.panelEffectiveDate = new Panel();
      this.formattedLabel2 = new FormattedLabel();
      this.panelCondition = new Panel();
      this.panelChannel = new Panel();
      this.tbxNotesComments = new TextBox();
      this.lblNotesComments = new FormattedLabel();
      this.formattedLabel1 = new FormattedLabel();
      this.lblChannels = new FormattedLabel();
      this.tbxScenarioName = new TextBox();
      this.lblRuleName = new FormattedLabel();
      this.tpValues = new TabPageEx();
      this.chkShowFieldsValue = new CheckBox();
      this.label1 = new Label();
      this.chkFeeNotAllowed = new CheckBox();
      this.btnClearValue = new ButtonEx();
      this.btnSetValue = new ButtonEx();
      this.gvScenarioFeeValues = new GridView();
      this.middleResetButton = new StandardIconButton();
      this.btnClose = new Button();
      this.ttipMessageAltText = new ToolTip(this.components);
      this.emHelpLink1 = new EMHelpLink();
      this.bottomSaveButton = new Button();
      this.gcScenariosForLine.SuspendLayout();
      ((ISupportInitialize) this.stdButtonSave).BeginInit();
      ((ISupportInitialize) this.stdButtonReset).BeginInit();
      ((ISupportInitialize) this.stdButtonDelete).BeginInit();
      ((ISupportInitialize) this.stdButtonUp).BeginInit();
      ((ISupportInitialize) this.stdButtonDown).BeginInit();
      ((ISupportInitialize) this.stdButtonCopy).BeginInit();
      ((ISupportInitialize) this.stdButtonNew).BeginInit();
      this.gcScenarioRuleDetails.SuspendLayout();
      ((ISupportInitialize) this.middleSaveButton).BeginInit();
      this.tabControlRuleDetails.SuspendLayout();
      this.tpDetails.SuspendLayout();
      this.tpValues.SuspendLayout();
      ((ISupportInitialize) this.middleResetButton).BeginInit();
      this.SuspendLayout();
      this.gcScenariosForLine.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gcScenariosForLine.Controls.Add((Control) this.verticalSeparator1);
      this.gcScenariosForLine.Controls.Add((Control) this.stdButtonSave);
      this.gcScenariosForLine.Controls.Add((Control) this.stdButtonReset);
      this.gcScenariosForLine.Controls.Add((Control) this.gvLineScenarios);
      this.gcScenariosForLine.Controls.Add((Control) this.btnGlobalRuleSettings);
      this.gcScenariosForLine.Controls.Add((Control) this.btnDeactivate);
      this.gcScenariosForLine.Controls.Add((Control) this.stdButtonDelete);
      this.gcScenariosForLine.Controls.Add((Control) this.stdButtonUp);
      this.gcScenariosForLine.Controls.Add((Control) this.stdButtonDown);
      this.gcScenariosForLine.Controls.Add((Control) this.stdButtonCopy);
      this.gcScenariosForLine.Controls.Add((Control) this.stdButtonNew);
      this.gcScenariosForLine.HeaderForeColor = SystemColors.ControlText;
      this.gcScenariosForLine.Location = new Point(0, 24);
      this.gcScenariosForLine.Name = "gcScenariosForLine";
      this.gcScenariosForLine.Size = new Size(989, 219);
      this.gcScenariosForLine.TabIndex = 1;
      this.gcScenariosForLine.TabStop = true;
      this.verticalSeparator1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.verticalSeparator1.Location = new Point(773, 4);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 19;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.stdButtonSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdButtonSave.BackColor = Color.Transparent;
      this.stdButtonSave.Enabled = false;
      this.stdButtonSave.Location = new Point(732, 4);
      this.stdButtonSave.MouseDownImage = (Image) null;
      this.stdButtonSave.Name = "stdButtonSave";
      this.stdButtonSave.Size = new Size(16, 16);
      this.stdButtonSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.stdButtonSave.TabIndex = 10;
      this.stdButtonSave.TabStop = false;
      this.ttipMessageAltText.SetToolTip((Control) this.stdButtonSave, "Save");
      this.stdButtonSave.Click += new EventHandler(this.stdButtonSave_Click);
      this.stdButtonReset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdButtonReset.BackColor = Color.Transparent;
      this.stdButtonReset.Enabled = false;
      this.stdButtonReset.Location = new Point(751, 4);
      this.stdButtonReset.MouseDownImage = (Image) null;
      this.stdButtonReset.Name = "stdButtonReset";
      this.stdButtonReset.Size = new Size(16, 16);
      this.stdButtonReset.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.stdButtonReset.TabIndex = 9;
      this.stdButtonReset.TabStop = false;
      this.ttipMessageAltText.SetToolTip((Control) this.stdButtonReset, "Reset");
      this.stdButtonReset.Click += new EventHandler(this.stdButtonReset_Click);
      this.gvLineScenarios.AllowMultiselect = false;
      this.gvLineScenarios.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gvLineScenarios.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Order";
      gvColumn1.Text = "Order";
      gvColumn1.Width = 50;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "ScenarioName";
      gvColumn2.Text = "Fee Scenario Name";
      gvColumn2.Width = 150;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Effective Date";
      gvColumn3.Text = "Effective Date";
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Condition";
      gvColumn4.Text = "Condition";
      gvColumn4.Width = 100;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Stop Condition";
      gvColumn5.Text = "Stop Condition";
      gvColumn5.Width = 100;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Channel";
      gvColumn6.Text = "Channel";
      gvColumn6.Width = 100;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Status";
      gvColumn7.Text = "Status";
      gvColumn7.Width = 100;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "LastModifiedBy";
      gvColumn8.Text = "Last Modified By";
      gvColumn8.Width = 130;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "LastModifiedDate";
      gvColumn9.Text = "Last Modified Date & Time";
      gvColumn9.Width = 150;
      this.gvLineScenarios.Columns.AddRange(new GVColumn[9]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8,
        gvColumn9
      });
      this.gvLineScenarios.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvLineScenarios.Location = new Point(0, 26);
      this.gvLineScenarios.Name = "gvLineScenarios";
      this.gvLineScenarios.Size = new Size(989, 193);
      this.gvLineScenarios.SortOption = GVSortOption.None;
      this.gvLineScenarios.TabIndex = 8;
      this.gvLineScenarios.SelectedIndexChanged += new EventHandler(this.gvLineScenarios_SelectedIndexChanged);
      this.btnGlobalRuleSettings.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnGlobalRuleSettings.Location = new Point(862, 2);
      this.btnGlobalRuleSettings.Name = "btnGlobalRuleSettings";
      this.btnGlobalRuleSettings.Size = new Size(117, 23);
      this.btnGlobalRuleSettings.TabIndex = 7;
      this.btnGlobalRuleSettings.Text = "Rule Settings";
      this.btnGlobalRuleSettings.UseVisualStyleBackColor = true;
      this.btnGlobalRuleSettings.Click += new EventHandler(this.btnGlobalRuleSettings_Click);
      this.btnDeactivate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDeactivate.Location = new Point(780, 2);
      this.btnDeactivate.Name = "btnDeactivate";
      this.btnDeactivate.Size = new Size(75, 23);
      this.btnDeactivate.TabIndex = 6;
      this.btnDeactivate.Text = "Deactivate";
      this.btnDeactivate.UseVisualStyleBackColor = true;
      this.btnDeactivate.Click += new EventHandler(this.btnDeactivate_Click);
      this.stdButtonDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdButtonDelete.BackColor = Color.Transparent;
      this.stdButtonDelete.Location = new Point(713, 4);
      this.stdButtonDelete.MouseDownImage = (Image) null;
      this.stdButtonDelete.Name = "stdButtonDelete";
      this.stdButtonDelete.Size = new Size(16, 16);
      this.stdButtonDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdButtonDelete.TabIndex = 5;
      this.stdButtonDelete.TabStop = false;
      this.ttipMessageAltText.SetToolTip((Control) this.stdButtonDelete, "Delete");
      this.stdButtonDelete.Click += new EventHandler(this.stdButtonDelete_Click);
      this.stdButtonUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdButtonUp.BackColor = Color.Transparent;
      this.stdButtonUp.Location = new Point(694, 4);
      this.stdButtonUp.MouseDownImage = (Image) null;
      this.stdButtonUp.Name = "stdButtonUp";
      this.stdButtonUp.Size = new Size(16, 16);
      this.stdButtonUp.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.stdButtonUp.TabIndex = 4;
      this.stdButtonUp.TabStop = false;
      this.ttipMessageAltText.SetToolTip((Control) this.stdButtonUp, "Move Up");
      this.stdButtonUp.Click += new EventHandler(this.stdButtonUp_Click);
      this.stdButtonDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdButtonDown.BackColor = Color.Transparent;
      this.stdButtonDown.Location = new Point(675, 4);
      this.stdButtonDown.MouseDownImage = (Image) null;
      this.stdButtonDown.Name = "stdButtonDown";
      this.stdButtonDown.Size = new Size(16, 16);
      this.stdButtonDown.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.stdButtonDown.TabIndex = 3;
      this.stdButtonDown.TabStop = false;
      this.ttipMessageAltText.SetToolTip((Control) this.stdButtonDown, "Move Down");
      this.stdButtonDown.Click += new EventHandler(this.stdButtonDown_Click);
      this.stdButtonCopy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdButtonCopy.BackColor = Color.Transparent;
      this.stdButtonCopy.Location = new Point(656, 4);
      this.stdButtonCopy.MouseDownImage = (Image) null;
      this.stdButtonCopy.Name = "stdButtonCopy";
      this.stdButtonCopy.Size = new Size(16, 16);
      this.stdButtonCopy.StandardButtonType = StandardIconButton.ButtonType.DuplicateButton;
      this.stdButtonCopy.TabIndex = 1;
      this.stdButtonCopy.TabStop = false;
      this.ttipMessageAltText.SetToolTip((Control) this.stdButtonCopy, "Duplicate");
      this.stdButtonCopy.Click += new EventHandler(this.stdButtonCopy_Click);
      this.stdButtonNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdButtonNew.BackColor = Color.Transparent;
      this.stdButtonNew.Location = new Point(637, 4);
      this.stdButtonNew.MouseDownImage = (Image) null;
      this.stdButtonNew.Name = "stdButtonNew";
      this.stdButtonNew.Size = new Size(16, 16);
      this.stdButtonNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdButtonNew.TabIndex = 0;
      this.stdButtonNew.TabStop = false;
      this.ttipMessageAltText.SetToolTip((Control) this.stdButtonNew, "New");
      this.stdButtonNew.Click += new EventHandler(this.stdButtonNew_Click);
      this.lblHeaderText.Location = new Point(4, 3);
      this.lblHeaderText.Name = "lblHeaderText";
      this.lblHeaderText.Size = new Size(878, 16);
      this.lblHeaderText.TabIndex = 1;
      this.lblHeaderText.TabStop = false;
      this.lblHeaderText.Text = "Fee scenario order is important. The system applies the first fee scenario rule it finds in this list that best matches the data in the loan. Use the arrows to manage the list order.";
      this.gcScenarioRuleDetails.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gcScenarioRuleDetails.Controls.Add((Control) this.middleSaveButton);
      this.gcScenarioRuleDetails.Controls.Add((Control) this.tabControlRuleDetails);
      this.gcScenarioRuleDetails.Controls.Add((Control) this.middleResetButton);
      this.gcScenarioRuleDetails.HeaderForeColor = SystemColors.ControlText;
      this.gcScenarioRuleDetails.Location = new Point(0, 248);
      this.gcScenarioRuleDetails.Name = "gcScenarioRuleDetails";
      this.gcScenarioRuleDetails.Size = new Size(989, 356);
      this.gcScenarioRuleDetails.TabIndex = 0;
      this.middleSaveButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.middleSaveButton.BackColor = Color.Transparent;
      this.middleSaveButton.Enabled = false;
      this.middleSaveButton.Location = new Point(948, 4);
      this.middleSaveButton.MouseDownImage = (Image) null;
      this.middleSaveButton.Name = "middleSaveButton";
      this.middleSaveButton.Size = new Size(16, 16);
      this.middleSaveButton.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.middleSaveButton.TabIndex = 12;
      this.middleSaveButton.TabStop = false;
      this.ttipMessageAltText.SetToolTip((Control) this.middleSaveButton, "Save");
      this.middleSaveButton.Click += new EventHandler(this.middleSaveButton_Click);
      this.tabControlRuleDetails.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tabControlRuleDetails.BackColor = SystemColors.Control;
      this.tabControlRuleDetails.Location = new Point(5, 30);
      this.tabControlRuleDetails.Name = "tabControlRuleDetails";
      this.tabControlRuleDetails.Size = new Size(980, 322);
      this.tabControlRuleDetails.TabHeight = 20;
      this.tabControlRuleDetails.TabIndex = 0;
      this.tabControlRuleDetails.TabPages.Add(this.tpDetails);
      this.tabControlRuleDetails.TabPages.Add(this.tpValues);
      this.tabControlRuleDetails.SelectedIndexChanged += new EventHandler(this.tabControlRuleDetails_SelectedIndexChanged);
      this.tpDetails.BackColor = Color.Transparent;
      this.tpDetails.Controls.Add((Control) this.panelEffectiveDate);
      this.tpDetails.Controls.Add((Control) this.formattedLabel2);
      this.tpDetails.Controls.Add((Control) this.panelCondition);
      this.tpDetails.Controls.Add((Control) this.panelChannel);
      this.tpDetails.Controls.Add((Control) this.tbxNotesComments);
      this.tpDetails.Controls.Add((Control) this.lblNotesComments);
      this.tpDetails.Controls.Add((Control) this.formattedLabel1);
      this.tpDetails.Controls.Add((Control) this.lblChannels);
      this.tpDetails.Controls.Add((Control) this.tbxScenarioName);
      this.tpDetails.Controls.Add((Control) this.lblRuleName);
      this.tpDetails.Location = new Point(1, 23);
      this.tpDetails.Name = "tpDetails";
      this.tpDetails.TabIndex = 0;
      this.tpDetails.TabStop = true;
      this.tpDetails.TabWidth = 100;
      this.tpDetails.Text = "Details";
      this.tpDetails.Value = (object) "Details";
      this.panelEffectiveDate.Location = new Point(571, 27);
      this.panelEffectiveDate.Name = "panelEffectiveDate";
      this.panelEffectiveDate.Size = new Size(392, 79);
      this.panelEffectiveDate.TabIndex = 3;
      this.formattedLabel2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.formattedLabel2.Location = new Point(571, 3);
      this.formattedLabel2.Name = "formattedLabel2";
      this.formattedLabel2.Size = new Size(185, 16);
      this.formattedLabel2.TabIndex = 37;
      this.formattedLabel2.TabStop = false;
      this.formattedLabel2.Text = "Effective Date for the fee scenario";
      this.panelCondition.Location = new Point(7, 188);
      this.panelCondition.Name = "panelCondition";
      this.panelCondition.Size = new Size(528, 92);
      this.panelCondition.TabIndex = 2;
      this.panelChannel.Location = new Point(7, 75);
      this.panelChannel.Name = "panelChannel";
      this.panelChannel.Size = new Size(528, 84);
      this.panelChannel.TabIndex = 1;
      this.tbxNotesComments.Location = new Point(571, 134);
      this.tbxNotesComments.Multiline = true;
      this.tbxNotesComments.Name = "tbxNotesComments";
      this.tbxNotesComments.ScrollBars = ScrollBars.Vertical;
      this.tbxNotesComments.Size = new Size(392, 146);
      this.tbxNotesComments.TabIndex = 4;
      this.tbxNotesComments.TextChanged += new EventHandler(this.tbxNotesComments_TextChanged);
      this.lblNotesComments.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblNotesComments.Location = new Point(571, 112);
      this.lblNotesComments.Name = "lblNotesComments";
      this.lblNotesComments.Size = new Size(99, 16);
      this.lblNotesComments.TabIndex = 14;
      this.lblNotesComments.TabStop = false;
      this.lblNotesComments.Text = "Notes/Comments";
      this.formattedLabel1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.formattedLabel1.Location = new Point(7, 166);
      this.formattedLabel1.Name = "formattedLabel1";
      this.formattedLabel1.Size = new Size(168, 16);
      this.formattedLabel1.TabIndex = 8;
      this.formattedLabel1.TabStop = false;
      this.formattedLabel1.Text = "Conditions for the fee scenario";
      this.lblChannels.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblChannels.Location = new Point(7, 53);
      this.lblChannels.Name = "lblChannels";
      this.lblChannels.Size = new Size(200, 16);
      this.lblChannels.TabIndex = 2;
      this.lblChannels.TabStop = false;
      this.lblChannels.Text = "Channels this fee scenario applies to";
      this.tbxScenarioName.Location = new Point(7, 27);
      this.tbxScenarioName.MaxLength = 64;
      this.tbxScenarioName.Name = "tbxScenarioName";
      this.tbxScenarioName.Size = new Size(528, 20);
      this.tbxScenarioName.TabIndex = 0;
      this.tbxScenarioName.TextChanged += new EventHandler(this.tbxScenarioName_TextChanged);
      this.lblRuleName.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblRuleName.Location = new Point(7, 4);
      this.lblRuleName.Name = "lblRuleName";
      this.lblRuleName.Size = new Size(112, 16);
      this.lblRuleName.TabIndex = 0;
      this.lblRuleName.TabStop = false;
      this.lblRuleName.Text = "Fee Scenario Name";
      this.tpValues.BackColor = Color.Transparent;
      this.tpValues.Controls.Add((Control) this.chkShowFieldsValue);
      this.tpValues.Controls.Add((Control) this.label1);
      this.tpValues.Controls.Add((Control) this.chkFeeNotAllowed);
      this.tpValues.Controls.Add((Control) this.btnClearValue);
      this.tpValues.Controls.Add((Control) this.btnSetValue);
      this.tpValues.Controls.Add((Control) this.gvScenarioFeeValues);
      this.tpValues.Location = new Point(1, 23);
      this.tpValues.Name = "tpValues";
      this.tpValues.TabIndex = 0;
      this.tpValues.TabWidth = 100;
      this.tpValues.Text = "Values";
      this.tpValues.Value = (object) "Values";
      this.chkShowFieldsValue.AutoSize = true;
      this.chkShowFieldsValue.Location = new Point(809, 27);
      this.chkShowFieldsValue.Name = "chkShowFieldsValue";
      this.chkShowFieldsValue.Size = new Size(162, 17);
      this.chkShowFieldsValue.TabIndex = 7;
      this.chkShowFieldsValue.Text = "Show Fields with Value Type";
      this.chkShowFieldsValue.UseVisualStyleBackColor = true;
      this.chkShowFieldsValue.CheckedChanged += new EventHandler(this.chkShowFieldsValue_CheckedChanged);
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Italic, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(826, 138);
      this.label1.Name = "label1";
      this.label1.Size = new Size(114, 13);
      this.label1.TabIndex = 4;
      this.label1.Text = "(Clear fields in loan file)";
      this.chkFeeNotAllowed.AutoSize = true;
      this.chkFeeNotAllowed.Location = new Point(809, 121);
      this.chkFeeNotAllowed.Name = "chkFeeNotAllowed";
      this.chkFeeNotAllowed.Size = new Size(101, 17);
      this.chkFeeNotAllowed.TabIndex = 0;
      this.chkFeeNotAllowed.Text = "Fee not allowed";
      this.chkFeeNotAllowed.UseVisualStyleBackColor = true;
      this.chkFeeNotAllowed.CheckedChanged += new EventHandler(this.chkFeeNotAllowed_CheckedChanged);
      this.btnClearValue.Location = new Point(807, 85);
      this.btnClearValue.Name = "btnClearValue";
      this.btnClearValue.Size = new Size(100, 23);
      this.btnClearValue.TabIndex = 3;
      this.btnClearValue.Text = "Clear Field Value";
      this.btnClearValue.UseVisualStyleBackColor = true;
      this.btnClearValue.Click += new EventHandler(this.btnClearValue_Click);
      this.btnSetValue.Location = new Point(807, 56);
      this.btnSetValue.Name = "btnSetValue";
      this.btnSetValue.Size = new Size(100, 23);
      this.btnSetValue.TabIndex = 2;
      this.btnSetValue.Text = "Set Field Value";
      this.btnSetValue.UseVisualStyleBackColor = true;
      this.btnSetValue.Click += new EventHandler(this.btnSetValue_Click);
      this.gvScenarioFeeValues.AllowMultiselect = false;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "Field";
      gvColumn10.Text = "Field Description";
      gvColumn10.Width = 300;
      gvColumn11.ImageIndex = -1;
      gvColumn11.Name = "fieldID";
      gvColumn11.Text = "Field ID";
      gvColumn11.Width = 150;
      gvColumn12.ImageIndex = -1;
      gvColumn12.Name = "valType";
      gvColumn12.Text = "Value Type";
      gvColumn12.Width = 150;
      gvColumn13.ImageIndex = -1;
      gvColumn13.Name = "Value";
      gvColumn13.SpringToFit = true;
      gvColumn13.Text = "Value";
      gvColumn13.Width = 178;
      this.gvScenarioFeeValues.Columns.AddRange(new GVColumn[4]
      {
        gvColumn10,
        gvColumn11,
        gvColumn12,
        gvColumn13
      });
      this.gvScenarioFeeValues.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvScenarioFeeValues.Location = new Point(17, 25);
      this.gvScenarioFeeValues.Name = "gvScenarioFeeValues";
      this.gvScenarioFeeValues.Size = new Size(780, 252);
      this.gvScenarioFeeValues.TabIndex = 1;
      this.gvScenarioFeeValues.DoubleClick += new EventHandler(this.btnSetValue_Click);
      this.middleResetButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.middleResetButton.BackColor = Color.Transparent;
      this.middleResetButton.Enabled = false;
      this.middleResetButton.Location = new Point(967, 4);
      this.middleResetButton.MouseDownImage = (Image) null;
      this.middleResetButton.Name = "middleResetButton";
      this.middleResetButton.Size = new Size(16, 16);
      this.middleResetButton.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.middleResetButton.TabIndex = 11;
      this.middleResetButton.TabStop = false;
      this.ttipMessageAltText.SetToolTip((Control) this.middleResetButton, "Reset");
      this.middleResetButton.Click += new EventHandler(this.middleResetButton_Click);
      this.btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnClose.DialogResult = DialogResult.Cancel;
      this.btnClose.Location = new Point(905, 611);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 23);
      this.btnClose.TabIndex = 2;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new EventHandler(this.btnCancel_Click);
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "Setup\\Fee Rule Scenarios";
      this.emHelpLink1.Location = new Point(12, 613);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 17;
      this.emHelpLink1.TabStop = false;
      this.bottomSaveButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.bottomSaveButton.Location = new Point(824, 611);
      this.bottomSaveButton.Name = "bottomSaveButton";
      this.bottomSaveButton.Size = new Size(75, 23);
      this.bottomSaveButton.TabIndex = 18;
      this.bottomSaveButton.Text = "Save";
      this.bottomSaveButton.UseVisualStyleBackColor = true;
      this.bottomSaveButton.Click += new EventHandler(this.bottomSaveButton_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnClose;
      this.ClientSize = new Size(994, 641);
      this.Controls.Add((Control) this.bottomSaveButton);
      this.Controls.Add((Control) this.btnClose);
      this.Controls.Add((Control) this.gcScenarioRuleDetails);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.lblHeaderText);
      this.Controls.Add((Control) this.gcScenariosForLine);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (FeeScenarioRules);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add/Edit Fee Scenario";
      this.FormClosing += new FormClosingEventHandler(this.FeeScenarioRules_FormClosing);
      this.Load += new EventHandler(this.FeeScenarioRules_Load);
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.gcScenariosForLine.ResumeLayout(false);
      ((ISupportInitialize) this.stdButtonSave).EndInit();
      ((ISupportInitialize) this.stdButtonReset).EndInit();
      ((ISupportInitialize) this.stdButtonDelete).EndInit();
      ((ISupportInitialize) this.stdButtonUp).EndInit();
      ((ISupportInitialize) this.stdButtonDown).EndInit();
      ((ISupportInitialize) this.stdButtonCopy).EndInit();
      ((ISupportInitialize) this.stdButtonNew).EndInit();
      this.gcScenarioRuleDetails.ResumeLayout(false);
      ((ISupportInitialize) this.middleSaveButton).EndInit();
      this.tabControlRuleDetails.ResumeLayout(false);
      this.tpDetails.ResumeLayout(false);
      this.tpDetails.PerformLayout();
      this.tpValues.ResumeLayout(false);
      this.tpValues.PerformLayout();
      ((ISupportInitialize) this.middleResetButton).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private class FieldIDComparer : IEqualityComparer<DDMFeeRuleValue>
    {
      public bool Equals(DDMFeeRuleValue val1, DDMFeeRuleValue val2)
      {
        return val1.FieldID == val2.FieldID;
      }

      public int GetHashCode(DDMFeeRuleValue val) => val.FieldID.GetHashCode();
    }
  }
}
