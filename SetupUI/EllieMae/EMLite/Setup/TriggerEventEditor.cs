// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.TriggerEventEditor
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using Elli.ElliEnum.Triggers;
using EllieMae.EMLite.BusinessRules;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Compiler;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class TriggerEventEditor : Form
  {
    private const string className = "TriggerEventEditor";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private const string NewFieldPlaceholder = "<Enter Field ID>";
    private static TriggerConditionType[] dateNumConditions = new TriggerConditionType[4]
    {
      TriggerConditionType.ValueChange,
      TriggerConditionType.FixedValue,
      TriggerConditionType.Range,
      TriggerConditionType.ValueList
    };
    private static TriggerConditionType[] textConditions = new TriggerConditionType[3]
    {
      TriggerConditionType.ValueChange,
      TriggerConditionType.FixedValue,
      TriggerConditionType.ValueList
    };
    private FieldSettings fieldSettings;
    private TriggerEvent triggerEvent;
    private FieldDefinition triggerField;
    private Sessions.Session session;
    private IContainer components;
    private Label label1;
    private GroupBox groupBox1;
    private Panel panel2;
    private ComboBox cboCondType;
    private Label label2;
    private Panel pnlCondRange;
    private Label label5;
    private TextBox txtCondMax;
    private TextBox txtCondMin;
    private Label label4;
    private Panel pnlCondValue;
    private TextBox txtCondValue;
    private Label label3;
    private Panel pnlCondList;
    private Button btnAddValue;
    private Button btnRemoveValue;
    private Label label7;
    private Panel pnlActionCopy;
    private Label label8;
    private Panel pnlActionType;
    private ComboBox cboActionType;
    private Label label6;
    private Panel pnlActionAssign;
    private Label label9;
    private Panel pnlActionAdvanced;
    private TextBox txtAdvancedCode;
    private Label label12;
    private Button btnOK;
    private Button btnCancel;
    private ListView lvwCopyTo;
    private ColumnHeader columnHeader3;
    private ColumnHeader columnHeader4;
    private Button btnAddCopyTo;
    private Button btnRemoveCopyTo;
    private ListView lvwAssignments;
    private ColumnHeader columnHeader5;
    private ColumnHeader columnHeader6;
    private Button btnAddAssignment;
    private Button btnRemoveAssignment;
    private TextBox txtFieldID;
    private ListView lvwCondValues;
    private ColumnHeader columnHeader1;
    private GroupBox groupBox2;
    private Panel panel1;
    private Button btnFindField;
    private Button btnEditAssignment;
    private Button btnEditValue;
    private Button btnEditCopyTo;
    private TextBox txtDescription;
    private Label label10;
    private Panel pnlCondValueSelect;
    private ComboBox cboCondValue;
    private Label label11;
    private Panel pnlCondOptions;
    private Label label13;
    private CheckedListBox lstCondOptions;
    private Panel pnlActionTasks;
    private Label label14;
    private CheckedListBox lstTasks;
    private Panel pnlActionEmail;
    private Button btnEditEmail;
    private ListView lvwEmails;
    private ColumnHeader columnHeader2;
    private Button btnAddEmail;
    private Button btnRemoveEmail;
    private Label label15;
    private ComboBox cboActivationType;
    private Label label16;
    private Panel pnlFieldActivation;
    private Panel pnlMilestoneActivation;
    private MilestoneDropdownBox cboMilestone;
    private Label label17;
    private Panel pnlActionLoanTemplate;
    private Label lblSelectLoanTemplate;
    private StandardIconButton btnSelectLoanTemplate;
    private TextBox txtLoanTemplate;
    private Panel pnlActionMoveLoan;
    private ComboBox cboLoanFolder;
    private Label lblSelectLoanFolder;
    private ComboBox cboActions;
    private Label lblActionType;
    private Panel pnlActionSpecialFeatureCodes;
    private StandardIconButton btnSelectSpecialFeatureCode;
    private TextBox txtSpecialFeatureCodes;
    private Label label18;

    public TriggerEventEditor(
      FieldSettings fieldSettings,
      TriggerEvent triggerEvent,
      Sessions.Session session)
    {
      this.InitializeComponent();
      this.session = session;
      this.triggerEvent = triggerEvent;
      this.fieldSettings = fieldSettings;
      this.loadActivationTypes();
      this.loadLoanFolderList();
      this.loadTaskList();
      this.loadMilestoneList();
      if (triggerEvent != null)
        this.loadTriggerItem();
      else
        this.clearForm();
    }

    public TriggerEvent TriggerEvent => this.triggerEvent;

    private void clearForm()
    {
      this.cboActivationType.SelectedIndex = 0;
      this.txtFieldID.Text = "";
      this.cboCondType.SelectedIndex = 0;
      this.txtCondValue.Text = "";
      this.txtCondMin.Text = this.txtCondMax.Text = "";
      this.lvwCondValues.Items.Clear();
      this.cboActionType.SelectedIndex = 0;
      this.lvwCopyTo.Items.Clear();
      this.lvwAssignments.Items.Clear();
      this.txtAdvancedCode.Text = "";
      this.lvwEmails.Items.Clear();
    }

    private void loadActivationTypes()
    {
      List<string> activationTypes = new TriggerActivationNameProvider().GetActivationTypes();
      if (!this.session.ConfigurationManager.CheckIfAnyTPOSiteExists() && activationTypes.Contains("TPO actions"))
        activationTypes.Remove("TPO actions");
      this.cboActivationType.DataSource = (object) activationTypes;
    }

    private void loadActionTypes()
    {
      List<string> actionTypes = new TriggerEventActionNameProvider().GetActionTypes();
      this.cboActionType.Items.Clear();
      foreach (object obj in actionTypes)
        this.cboActionType.Items.Add(obj);
    }

    private void loadLoanFolderList()
    {
      try
      {
        LoanFolderInfo[] allLoanFolderInfos = this.session.LoanManager.GetAllLoanFolderInfos(false, true);
        Array.Sort<LoanFolderInfo>(allLoanFolderInfos);
        this.cboLoanFolder.Items.Clear();
        foreach (object obj in allLoanFolderInfos)
          this.cboLoanFolder.Items.Add(obj);
        this.cboLoanFolder.SelectedIndex = 0;
      }
      catch (Exception ex)
      {
        Tracing.Log(TriggerEventEditor.sw, nameof (TriggerEventEditor), TraceLevel.Error, "Error loading folder list: " + (object) ex);
        int num = (int) Utils.Dialog((IWin32Window) this, "Error loading folder list: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void loadTaskList()
    {
      foreach (MilestoneTaskDefinition milestoneTaskDefinition in (IEnumerable) this.session.ConfigurationManager.GetMilestoneTasks().Values)
        this.lstTasks.Items.Add((object) milestoneTaskDefinition.TaskName);
    }

    private void loadMilestoneList()
    {
      this.cboMilestone.PopulateAllMilestones(((MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).GetAllMilestonesList(), true, true);
    }

    private void loadTriggerItem()
    {
      TriggerCondition condition = this.triggerEvent.Conditions[0];
      this.cboActivationType.SelectedItem = (object) new TriggerActivationNameProvider().GetNameForConditionTypeToDisplay(condition.ConditionType);
      if (this.cboActivationType.SelectedItem.ToString().Equals("Rate Lock actions") || this.cboActivationType.SelectedItem.ToString().Equals("TPO actions"))
        this.cboActions.SelectedItem = (object) new TriggerActivationNameProvider().GetNameForConditionType(condition.ConditionType);
      switch (condition)
      {
        case TriggerFieldCondition _:
          this.loadFieldTriggerCondition(condition as TriggerFieldCondition);
          break;
        case TriggerMilestoneCompletionCondition _:
          this.cboMilestone.MilestoneID = ((TriggerMilestoneCompletionCondition) condition).MilestoneID;
          break;
      }
      this.cboActionType.SelectedItem = (object) new TriggerEventActionNameProvider().GetName((object) this.triggerEvent.Action.ActionType);
      if (this.triggerEvent.Action is TriggerCopyAction)
      {
        this.lvwCopyTo.Items.Clear();
        foreach (string targetFieldId in ((TriggerCopyAction) this.triggerEvent.Action).TargetFieldIDs)
          this.addFieldToListView(this.lvwCopyTo, targetFieldId);
      }
      else if (this.triggerEvent.Action is TriggerAssignmentAction)
      {
        this.lvwAssignments.Items.Clear();
        foreach (TriggerAssignment assignment in ((TriggerAssignmentAction) this.triggerEvent.Action).Assignments)
          this.lvwAssignments.Items.Add(this.createListItemForAssignment(assignment));
      }
      else if (this.triggerEvent.Action is TriggerAdvancedCodeAction)
        this.txtAdvancedCode.Text = ((TriggerAdvancedCodeAction) this.triggerEvent.Action).SourceCode;
      else if (this.triggerEvent.Action is TriggerEmailAction)
      {
        this.lvwEmails.Items.Clear();
        foreach (TriggerEmailTemplate template in ((TriggerEmailAction) this.triggerEvent.Action).Templates)
          this.lvwEmails.Items.Add(this.createListItemForEmailTemplate(template));
      }
      else if (this.triggerEvent.Action is TriggerCompleteTasksAction)
      {
        TriggerCompleteTasksAction action = (TriggerCompleteTasksAction) this.triggerEvent.Action;
        for (int index = 0; index < this.lstTasks.Items.Count; ++index)
          this.lstTasks.SetItemChecked(index, action.ContainsTask(this.lstTasks.Items[index].ToString()));
      }
      else if (this.triggerEvent.Action is TriggerApplyLoanTemplateAction)
      {
        TriggerApplyLoanTemplateAction action = (TriggerApplyLoanTemplateAction) this.triggerEvent.Action;
        this.txtLoanTemplate.Text = action.LoanTemplateName;
        if (string.IsNullOrEmpty(action.FilePath))
          return;
        this.txtLoanTemplate.Tag = (object) action.FilePath;
      }
      else if (this.triggerEvent.Action is TriggerSpecialFeatureCodesAction action1)
      {
        this.txtSpecialFeatureCodes.Tag = (object) action1.SpecialFeatureCodes;
        this.txtSpecialFeatureCodes.Text = action1.SpecialFeatureCodes.First<KeyValuePair<string, string>>().Value;
        foreach (KeyValuePair<string, string> keyValuePair in action1.SpecialFeatureCodes.Skip<KeyValuePair<string, string>>(1))
          this.txtSpecialFeatureCodes.Text = this.txtSpecialFeatureCodes.Text + " | " + keyValuePair.Value;
      }
      else
      {
        if (!(this.triggerEvent.Action is TriggerMoveLoanFolderAction))
          return;
        this.cboLoanFolder.SelectedIndex = this.cboLoanFolder.FindStringExact(((TriggerMoveLoanFolderAction) this.triggerEvent.Action).LoanFolderName);
      }
    }

    private void loadFieldTriggerCondition(TriggerFieldCondition cond)
    {
      this.triggerField = EncompassFields.GetField(cond.FieldID, this.fieldSettings);
      if (this.triggerField == null)
        this.triggerField = (FieldDefinition) new UndefinedField(cond.FieldID, "Unknown Field");
      this.updateUIForField();
      ClientCommonUtils.PopulateDropdown(this.cboCondType, (object) new TriggerEventConditionNameProvider().GetName((object) cond.ConditionType), false);
      switch (cond)
      {
        case TriggerFixedValueCondition _:
          string str = ((TriggerFixedValueCondition) cond).Value;
          if (this.triggerField.Options.RequireValueFromList && str != "")
          {
            this.cboCondValue.SelectedItem = (object) this.triggerField.Options.GetOptionByValue(str);
            break;
          }
          if (this.triggerField.Options.RequireValueFromList)
          {
            this.cboCondValue.SelectedItem = (object) "";
            break;
          }
          this.txtCondValue.Text = ((TriggerFixedValueCondition) cond).Value;
          break;
        case TriggerRangeCondition _:
          this.txtCondMin.Text = this.txtCondMax.Text = "";
          TriggerRangeCondition triggerRangeCondition = (TriggerRangeCondition) cond;
          this.txtCondMin.Text = triggerRangeCondition.Minimum.ToString();
          this.txtCondMax.Text = triggerRangeCondition.Maximum.ToString();
          break;
        case TriggerValueListCondition _:
          if (this.triggerField.Options.RequireValueFromList)
          {
            this.loadTriggerConditionOptions((TriggerValueListCondition) cond);
            break;
          }
          this.loadTriggerConditionValueList((TriggerValueListCondition) cond);
          break;
      }
    }

    private string fieldValueToText(string value)
    {
      return this.triggerField.Options.RequireValueFromList ? this.triggerField.Options.ValueToText(value) : value;
    }

    private string fieldTextToValue(string text)
    {
      return this.triggerField.Options.RequireValueFromList ? this.triggerField.Options.TextToValue(text) : text;
    }

    private void loadTriggerConditionValueList(TriggerValueListCondition listCond)
    {
      this.lvwCondValues.Items.Clear();
      foreach (string str in listCond.Values)
        this.lvwCondValues.Items.Add(this.fieldValueToText(str));
    }

    private void loadTriggerConditionOptions(TriggerValueListCondition listCond)
    {
      List<string> stringList = new List<string>((IEnumerable<string>) listCond.Values);
      for (int index = 0; index < this.lstCondOptions.Items.Count; ++index)
      {
        FieldOption fieldOption = (FieldOption) this.lstCondOptions.Items[index];
        this.lstCondOptions.SetItemChecked(index, stringList.Contains(fieldOption.Value));
      }
    }

    private ListViewItem createListItemForAssignment(TriggerAssignment assignment)
    {
      ListViewItem itemForAssignment = new ListViewItem(assignment.FieldID);
      itemForAssignment.SubItems.Add("");
      itemForAssignment.Tag = (object) assignment;
      this.updateListItemForAssignment(itemForAssignment);
      return itemForAssignment;
    }

    private void updateListItemForAssignment(ListViewItem item)
    {
      TriggerAssignment tag = (TriggerAssignment) item.Tag;
      item.Text = tag.FieldID;
      item.SubItems[1].Text = tag.Expression;
    }

    private ListViewItem createListItemForEmailTemplate(TriggerEmailTemplate template)
    {
      ListViewItem forEmailTemplate = new ListViewItem(template.Subject);
      forEmailTemplate.Tag = (object) template;
      this.updateListItemForEmailTemplate(forEmailTemplate);
      return forEmailTemplate;
    }

    private void updateListItemForEmailTemplate(ListViewItem item)
    {
      TriggerEmailTemplate tag = (TriggerEmailTemplate) item.Tag;
      item.Text = tag.Subject;
    }

    private bool addFieldToListView(ListView listView, string fieldId)
    {
      FieldDefinition fieldDefinition = EncompassFields.GetField(fieldId, this.fieldSettings) ?? (FieldDefinition) new UndefinedField(fieldId, "<Unknown Field>");
      foreach (ListViewItem listViewItem in listView.Items)
      {
        if (fieldDefinition.Equals(listViewItem.Tag))
          return false;
      }
      listView.Items.Add(new ListViewItem(fieldDefinition.FieldID)
      {
        SubItems = {
          fieldDefinition.Description
        },
        Tag = (object) fieldDefinition
      });
      return true;
    }

    private void removeSelectedListItems(ListView listView)
    {
      while (listView.SelectedIndices.Count > 0)
        listView.Items.RemoveAt(listView.SelectedIndices[0]);
    }

    private void btnAddCopyTo_Click(object sender, EventArgs e)
    {
      ListViewItem listViewItem = new ListViewItem("<Enter Field ID>");
      listViewItem.SubItems.Add("");
      this.lvwCopyTo.Items.Add(listViewItem);
      listViewItem.BeginEdit();
    }

    private void btnEditCopyTo_Click(object sender, EventArgs e)
    {
      if (this.lvwCopyTo.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Select the item from the list to edit.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else if (this.lvwCopyTo.SelectedItems.Count > 1)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "Only one item can be selected to edit.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
        this.lvwCopyTo.SelectedItems[0].BeginEdit();
    }

    private void onCopyToFieldsAdded(object source, EventArgs e)
    {
      foreach (string selectedValue in ((TriggerAddFieldDialog) source).GetSelectedValues())
        this.addFieldToListView(this.lvwCopyTo, selectedValue);
    }

    private void btnRemoveCopyTo_Click(object sender, EventArgs e)
    {
      if (this.lvwCopyTo.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select the fields(s) from the list to be removed.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
        this.removeSelectedListItems(this.lvwCopyTo);
    }

    private void cboCondType_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.pnlCondValue.Visible = this.pnlCondRange.Visible = this.pnlCondList.Visible = this.pnlCondValueSelect.Visible = this.pnlCondOptions.Visible = false;
      if (this.cboCondType.SelectedIndex < 0)
        return;
      switch (this.getSelectedConditionType())
      {
        case TriggerConditionType.FixedValue:
          if (this.triggerField.Options.RequireValueFromList)
          {
            this.pnlCondValueSelect.Visible = true;
            break;
          }
          this.pnlCondValue.Visible = true;
          break;
        case TriggerConditionType.Range:
          this.pnlCondRange.Visible = true;
          break;
        case TriggerConditionType.ValueList:
          if (this.triggerField.Options.RequireValueFromList)
          {
            this.pnlCondOptions.Visible = true;
            break;
          }
          this.pnlCondList.Visible = true;
          break;
      }
    }

    private void cboActionType_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.pnlActionAssign.Visible = this.pnlActionCopy.Visible = this.pnlActionAdvanced.Visible = this.pnlActionTasks.Visible = this.pnlActionEmail.Visible = this.pnlActionLoanTemplate.Visible = this.pnlActionSpecialFeatureCodes.Visible = this.pnlActionMoveLoan.Visible = false;
      switch (this.getSelectedActionType())
      {
        case TriggerActionType.Assign:
          this.pnlActionAssign.Visible = true;
          break;
        case TriggerActionType.Copy:
          this.pnlActionCopy.Visible = true;
          break;
        case TriggerActionType.AdvancedCode:
          this.pnlActionAdvanced.Visible = true;
          break;
        case TriggerActionType.CompleteTasks:
          this.pnlActionTasks.Visible = true;
          break;
        case TriggerActionType.Email:
          this.pnlActionEmail.Visible = true;
          break;
        case TriggerActionType.LoanMove:
          this.pnlActionMoveLoan.Visible = true;
          break;
        case TriggerActionType.ApplyLoanTemplate:
          this.pnlActionLoanTemplate.Visible = true;
          break;
        case TriggerActionType.AddSpecialFeatureCode:
          this.pnlActionSpecialFeatureCodes.Visible = true;
          break;
      }
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (!this.validateData())
        return;
      this.triggerEvent = new TriggerEvent(this.createCondition(), this.createAction());
      this.DialogResult = DialogResult.OK;
    }

    private bool validateData()
    {
      bool flag = true;
      if (this.pnlFieldActivation.Visible)
        flag = this.validateFieldActivatedCondition();
      else if (this.pnlMilestoneActivation.Visible)
        flag = this.validateMilestoneActivatedCondition();
      return flag && this.validateAction();
    }

    private bool validateMilestoneActivatedCondition()
    {
      if (this.cboMilestone.MilestoneID != null)
        return true;
      int num = (int) Utils.Dialog((IWin32Window) this, "You must select the milestone which will activate this trigger from the provided list.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      return false;
    }

    private bool validateFieldActivatedCondition()
    {
      if (this.triggerField == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "A valid Field ID must be specified as the trigger for this event.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.txtFieldID.Focus();
        return false;
      }
      switch (this.getSelectedConditionType())
      {
        case TriggerConditionType.Range:
          if (!this.validateRange())
            return false;
          break;
        case TriggerConditionType.ValueList:
          if (this.triggerField.Options.RequireValueFromList)
          {
            if (this.lstCondOptions.CheckedItems.Count == 0)
            {
              int num = (int) Utils.Dialog((IWin32Window) this, "One or more items must be selected within the Activation Values list.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
              return false;
            }
            break;
          }
          if (this.lvwCondValues.Items.Count == 0)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "One or more items must be added to the Activation Values list.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return false;
          }
          break;
      }
      return true;
    }

    private bool validateAction()
    {
      if (this.cboActionType.SelectedIndex < 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "An Action must be selected for this event.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      switch (this.getSelectedActionType())
      {
        case TriggerActionType.Assign:
          if (this.lvwAssignments.Items.Count == 0)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "One or more assignments must be made in the Assignments list.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return false;
          }
          break;
        case TriggerActionType.Copy:
          this.removeEmptyListItems(this.lvwCopyTo);
          if (this.lvwCopyTo.Items.Count == 0)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "One or more fields must be added to the Copy To list.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return false;
          }
          break;
        case TriggerActionType.CompleteTasks:
          if (this.lstTasks.CheckedItems.Count == 0)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "One or more tasks must be selected from the list provided.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return false;
          }
          break;
        case TriggerActionType.Email:
          if (this.lvwEmails.Items.Count == 0)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "One or more email templates must be created for this trigger.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return false;
          }
          break;
        case TriggerActionType.LoanMove:
          if (this.cboLoanFolder.SelectedItem == null || string.IsNullOrEmpty(this.cboLoanFolder.SelectedItem.ToString()))
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "A loan folder must be selected for this trigger.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return false;
          }
          break;
        case TriggerActionType.ApplyLoanTemplate:
          if (string.IsNullOrEmpty(this.txtLoanTemplate.Text))
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "A loan template must be selected for this trigger.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return false;
          }
          break;
        case TriggerActionType.AddSpecialFeatureCode:
          if (string.IsNullOrEmpty(this.txtSpecialFeatureCodes.Text))
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "A special code must be selected for this trigger.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return false;
          }
          break;
        default:
          if (!this.validateAdvancedCode(this.txtAdvancedCode.Text))
            return false;
          break;
      }
      return true;
    }

    private void removeEmptyListItems(ListView listView)
    {
      for (int index = listView.Items.Count - 1; index >= 0; --index)
      {
        if (listView.Items[index].Tag == null)
          listView.Items.RemoveAt(index);
      }
    }

    private TriggerConditionType getSelectedConditionType()
    {
      return (TriggerConditionType) new TriggerEventConditionNameProvider().GetValue(this.cboCondType.SelectedItem.ToString());
    }

    private TriggerActionType getSelectedActionType()
    {
      return (TriggerActionType) new TriggerEventActionNameProvider().GetValue(this.cboActionType.SelectedItem.ToString());
    }

    private bool validateAdvancedCode(string sourceCode)
    {
      if (sourceCode.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must provide your advanced code in the space provided.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        using (RuntimeContext context = RuntimeContext.Create())
          new TriggerEventEditor.TriggerCodeChecker(sourceCode).CreateImplementation(context);
        return true;
      }
      catch (CompileException ex)
      {
        int num = (int) new CompileExceptionDialog(ex).ShowDialog((IWin32Window) this);
        return false;
      }
      catch (Exception ex)
      {
        Tracing.Log(TriggerEventEditor.sw, nameof (TriggerEventEditor), TraceLevel.Error, "Error compiling advanced code: " + (object) ex);
        int num = (int) Utils.Dialog((IWin32Window) this, "Error validating advanced code: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private bool validateRange()
    {
      IComparable comparable1 = (IComparable) null;
      IComparable comparable2 = (IComparable) null;
      FieldDefinition field = EncompassFields.GetField(this.txtFieldID.Text.Trim(), this.fieldSettings);
      if (this.txtCondMin.Text.Trim() == "" && this.txtCondMax.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must specify and minimum and/or maximum value for the activation range.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      try
      {
        if (this.txtCondMin.Text.Trim() != "")
          comparable1 = (IComparable) Utils.ConvertToNativeValue(this.txtCondMin.Text.Trim(), field.Format, true);
      }
      catch
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The minimum value specified for the activation range is invalid for this field type.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      try
      {
        if (this.txtCondMax.Text.Trim() != "")
          comparable2 = (IComparable) Utils.ConvertToNativeValue(this.txtCondMax.Text.Trim(), field.Format, true);
      }
      catch
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The maximum value specified for the activation range is invalid for this field type.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      if (comparable1 == null || comparable2 == null || comparable1.CompareTo((object) comparable2) <= 0)
        return true;
      int num1 = (int) Utils.Dialog((IWin32Window) this, "The minimum activation value must be less than or equal to the maximum.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      return false;
    }

    private TriggerAction createAction()
    {
      switch (this.getSelectedActionType())
      {
        case TriggerActionType.Assign:
          return (TriggerAction) new TriggerAssignmentAction(this.createAssignmentList());
        case TriggerActionType.Copy:
          return (TriggerAction) new TriggerCopyAction(this.createCopyToFieldList());
        case TriggerActionType.CompleteTasks:
          return (TriggerAction) new TriggerCompleteTasksAction(this.createTaskList());
        case TriggerActionType.Email:
          return (TriggerAction) new TriggerEmailAction(this.createEmailTemplateList());
        case TriggerActionType.LoanMove:
          return (TriggerAction) new TriggerMoveLoanFolderAction(this.cboLoanFolder.SelectedItem.ToString());
        case TriggerActionType.ApplyLoanTemplate:
          string empty = string.Empty;
          if (this.txtLoanTemplate.Tag != null)
            empty = this.txtLoanTemplate.Tag.ToString();
          return (TriggerAction) new TriggerApplyLoanTemplateAction(empty.Replace("Public:", string.Empty));
        case TriggerActionType.AddSpecialFeatureCode:
          return (TriggerAction) new TriggerSpecialFeatureCodesAction((Dictionary<string, string>) this.txtSpecialFeatureCodes.Tag);
        default:
          return (TriggerAction) new TriggerAdvancedCodeAction(this.txtAdvancedCode.Text);
      }
    }

    private TriggerEmailTemplate[] createEmailTemplateList()
    {
      List<TriggerEmailTemplate> triggerEmailTemplateList = new List<TriggerEmailTemplate>();
      foreach (ListViewItem listViewItem in this.lvwEmails.Items)
        triggerEmailTemplateList.Add(listViewItem.Tag as TriggerEmailTemplate);
      return triggerEmailTemplateList.ToArray();
    }

    private string[] createTaskList()
    {
      List<string> stringList = new List<string>();
      foreach (string checkedItem in this.lstTasks.CheckedItems)
        stringList.Add(checkedItem);
      return stringList.ToArray();
    }

    private string[] createCopyToFieldList()
    {
      List<string> stringList = new List<string>();
      foreach (ListViewItem listViewItem in this.lvwCopyTo.Items)
        stringList.Add(((FieldDefinition) listViewItem.Tag).FieldID);
      return stringList.ToArray();
    }

    private TriggerAssignment[] createAssignmentList()
    {
      List<TriggerAssignment> triggerAssignmentList = new List<TriggerAssignment>();
      foreach (ListViewItem listViewItem in this.lvwAssignments.Items)
        triggerAssignmentList.Add((TriggerAssignment) listViewItem.Tag);
      return triggerAssignmentList.ToArray();
    }

    private TriggerCondition createCondition()
    {
      string name = this.cboActivationType.SelectedItem.ToString();
      if (this.cboActivationType.SelectedItem.ToString().Equals("Rate Lock actions") || this.cboActivationType.SelectedItem.ToString().Equals("TPO actions"))
        name = this.cboActions.SelectedItem.ToString();
      switch ((TriggerActivationType) new TriggerActivationNameProvider().GetValue(name))
      {
        case TriggerActivationType.FieldModified:
          return this.createFieldCondition();
        case TriggerActivationType.MilestoneCompleted:
          return this.createMilestoneCondition();
        case TriggerActivationType.LockRequested:
        case TriggerActivationType.LockConfirmed:
        case TriggerActivationType.LockDenied:
          return this.createRateLockCondition();
        default:
          return this.createGenericCondition();
      }
    }

    private TriggerCondition createMilestoneCondition()
    {
      return (TriggerCondition) new TriggerMilestoneCompletionCondition(this.cboMilestone.MilestoneID);
    }

    private TriggerCondition createRateLockCondition()
    {
      switch ((TriggerActivationType) new TriggerActivationNameProvider().GetValue(this.cboActions.SelectedItem.ToString()))
      {
        case TriggerActivationType.LockRequested:
          return (TriggerCondition) new TriggerRateLockCondition(TriggerLockAction.Requested);
        case TriggerActivationType.LockConfirmed:
          return (TriggerCondition) new TriggerRateLockCondition(TriggerLockAction.Confirmed);
        case TriggerActivationType.LockDenied:
          return (TriggerCondition) new TriggerRateLockCondition(TriggerLockAction.Denied);
        default:
          throw new Exception("Invalid rate lock condition specified");
      }
    }

    private TriggerCondition createFieldCondition()
    {
      string fieldId = this.txtFieldID.Text.Trim();
      switch ((TriggerConditionType) new TriggerEventConditionNameProvider().GetValue(this.cboCondType.SelectedItem.ToString()))
      {
        case TriggerConditionType.ValueChange:
          return (TriggerCondition) new TriggerValueChangeCondition(fieldId);
        case TriggerConditionType.FixedValue:
          return (TriggerCondition) new TriggerFixedValueCondition(fieldId, this.getConditionFixedValue());
        case TriggerConditionType.Range:
          return (TriggerCondition) new TriggerRangeCondition(fieldId, this.txtCondMin.Text, this.txtCondMax.Text);
        case TriggerConditionType.NonEmptyValue:
          return (TriggerCondition) new TriggerNonEmptyValueCondition(fieldId);
        default:
          return (TriggerCondition) new TriggerValueListCondition(fieldId, this.createConditionValueList());
      }
    }

    private TriggerCondition createGenericCondition()
    {
      TriggerActivationType triggerActivationType = (TriggerActivationType) new TriggerActivationNameProvider().GetValue(this.cboActions.SelectedItem.ToString());
      this.cboActions.SelectedItem.ToString();
      TriggerConditionType triggerConditionType = TriggerConditionType.FixedValue;
      switch (triggerActivationType)
      {
        case TriggerActivationType.RegisterLoan:
          triggerConditionType = TriggerConditionType.RegisterLoan;
          break;
        case TriggerActivationType.ImportAdditionalData:
          triggerConditionType = TriggerConditionType.ImportAdditionalData;
          break;
        case TriggerActivationType.OrderReissueCredit:
          triggerConditionType = TriggerConditionType.OrderReissueCredit;
          break;
        case TriggerActivationType.Disclosures:
          triggerConditionType = TriggerConditionType.Disclosures;
          break;
        case TriggerActivationType.SubmitLoan:
          triggerConditionType = TriggerConditionType.SubmitLoan;
          break;
        case TriggerActivationType.ChangedCircumstance:
          triggerConditionType = TriggerConditionType.ChangedCircumstance;
          break;
        case TriggerActivationType.LockRequest:
          triggerConditionType = TriggerConditionType.LockRequest;
          break;
        case TriggerActivationType.RunLPUnderwriting:
          triggerConditionType = TriggerConditionType.RunLPUnderwriting;
          break;
        case TriggerActivationType.RunDUUnderwriting:
          triggerConditionType = TriggerConditionType.RunDUUnderwriting;
          break;
        case TriggerActivationType.ReSubmitLoan:
          triggerConditionType = TriggerConditionType.ReSubmitLoan;
          break;
        case TriggerActivationType.ViewPurchaseAdvice:
          triggerConditionType = TriggerConditionType.ViewPurchaseAdvice;
          break;
        case TriggerActivationType.LockExtension:
          triggerConditionType = TriggerConditionType.LockExtension;
          break;
        case TriggerActivationType.SubmitPurchase:
          triggerConditionType = TriggerConditionType.SubmitPurchase;
          break;
        case TriggerActivationType.FloatLock:
          triggerConditionType = TriggerConditionType.FloatLock;
          break;
        case TriggerActivationType.CancelLock:
          triggerConditionType = TriggerConditionType.CancelLock;
          break;
        case TriggerActivationType.RePriceLock:
          triggerConditionType = TriggerConditionType.RePriceLock;
          break;
        case TriggerActivationType.ReLockLock:
          triggerConditionType = TriggerConditionType.ReLockLock;
          break;
        case TriggerActivationType.ChangeRequestOB:
          triggerConditionType = TriggerConditionType.ChangeRequestOB;
          break;
        case TriggerActivationType.Withdrawal:
          triggerConditionType = TriggerConditionType.Withdrawal;
          break;
        case TriggerActivationType.Cancel:
          triggerConditionType = TriggerConditionType.Cancel;
          break;
        case TriggerActivationType.RequestLoanEstimate:
          triggerConditionType = TriggerConditionType.RequestLoanEstimate;
          break;
        case TriggerActivationType.RequestTitleFees:
          triggerConditionType = TriggerConditionType.RequestTitleFees;
          break;
        case TriggerActivationType.GenerateLoanEstimateDisclosure:
          triggerConditionType = TriggerConditionType.GenerateLoanEstimateDisclosure;
          break;
        case TriggerActivationType.OrderAppraisalRequest:
          triggerConditionType = TriggerConditionType.OrderAppraisalRequest;
          break;
        case TriggerActivationType.OrderAUS:
          triggerConditionType = TriggerConditionType.OrderAUS;
          break;
        case TriggerActivationType.SaveLoan:
          triggerConditionType = TriggerConditionType.SaveLoan;
          break;
      }
      return (TriggerCondition) new TriggerGenericTPOCondition(triggerConditionType);
    }

    private Range<Decimal> createConditionRange()
    {
      return new Range<Decimal>(Utils.ParseDecimal((object) this.txtCondMin.Text, Decimal.MinValue), Utils.ParseDecimal((object) this.txtCondMax.Text, Decimal.MaxValue));
    }

    private string getConditionFixedValue()
    {
      return this.triggerField.Options.RequireValueFromList ? ((FieldOption) this.cboCondValue.SelectedItem).Value : this.txtCondValue.Text;
    }

    private string[] createConditionValueList()
    {
      List<string> stringList = new List<string>();
      if (this.triggerField.Options.RequireValueFromList)
      {
        foreach (FieldOption checkedItem in this.lstCondOptions.CheckedItems)
          stringList.Add(checkedItem.Value);
      }
      else
      {
        foreach (ListViewItem listViewItem in this.lvwCondValues.Items)
          stringList.Add(listViewItem.Text);
      }
      return stringList.ToArray();
    }

    private string[] getFieldIDsFromList(ListView listView)
    {
      List<string> stringList = new List<string>();
      foreach (ListViewItem listViewItem in listView.Items)
        stringList.Add(((FieldDefinition) listViewItem.Tag).FieldID);
      return stringList.ToArray();
    }

    private void btnAddAssignment_Click(object sender, EventArgs e)
    {
      using (TriggerAssignmentEditor assignmentEditor = new TriggerAssignmentEditor(this.fieldSettings, (TriggerAssignment) null))
      {
        if (assignmentEditor.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.lvwAssignments.Items.Add(this.createListItemForAssignment(assignmentEditor.TriggerAssignment));
      }
    }

    private void btnRemoveAssignment_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Remove the selected assignment(s) from the event?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      this.removeSelectedListItems(this.lvwAssignments);
    }

    private void btnEditAssignment_Click(object sender, EventArgs e)
    {
      if (this.lvwAssignments.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Select the assignment to be edited from the list.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else if (this.lvwAssignments.SelectedItems.Count > 1)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "You must first select a single assignment to be edited.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
        this.editAssignment();
    }

    private void btnAddValue_Click(object sender, EventArgs e)
    {
      this.lvwCondValues.Items.Add(new ListViewItem("<New Value>")).BeginEdit();
    }

    private void btnRemoveValue_Click(object sender, EventArgs e)
    {
      if (this.lvwCondValues.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select the value(s) from the list to be removed.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "Remove the selected values from the list?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
          return;
        this.removeSelectedListItems(this.lvwCondValues);
      }
    }

    private void btnEditValue_Click(object sender, EventArgs e)
    {
      if (this.lvwCondValues.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Select the item from the list to edit.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else if (this.lvwCondValues.SelectedItems.Count > 1)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "Only one item can be selected to edit.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
        this.lvwCondValues.SelectedItems[0].BeginEdit();
    }

    private void lvwCopyTo_AfterLabelEdit(object sender, LabelEditEventArgs e)
    {
      ListViewItem listViewItem = ((ListView) sender).Items[e.Item];
      if (listViewItem == null)
        return;
      string fieldId = (e.Label ?? listViewItem.Text).Trim();
      FieldDefinition field = EncompassFields.GetField(fieldId, this.fieldSettings);
      if (fieldId == "" || fieldId == "<Enter Field ID>")
        this.removeInvalidListItemsAsync((object) this.lvwCopyTo);
      else if (field == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The value '" + fieldId + "' is not a valid Field ID.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        e.CancelEdit = true;
        this.removeInvalidListItemsAsync((object) this.lvwCopyTo);
      }
      else if (!field.AllowEdit)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The field '" + fieldId + "' is read-only and cannot be used in this context.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        e.CancelEdit = true;
        this.removeInvalidListItemsAsync((object) this.lvwCopyTo);
      }
      else
      {
        listViewItem.SubItems[1].Text = field.Description;
        listViewItem.Tag = (object) field;
      }
    }

    private void removeInvalidListItemsAsync(object listView)
    {
      if (this.InvokeRequired)
        this.Invoke((Delegate) new WaitCallback(this.removeInvalidListItems), listView);
      else
        ThreadPool.QueueUserWorkItem(new WaitCallback(this.removeInvalidListItemsAsync), listView);
    }

    private void removeInvalidListItems(object listViewObj)
    {
      ListView listView = (ListView) listViewObj;
      for (int index = listView.Items.Count - 1; index >= 0; --index)
      {
        if (listView.Items[index].Text == "" || listView.Items[index].Text == "<Enter Field ID>")
          listView.Items.RemoveAt(index);
      }
    }

    private void btnFindField_Click(object sender, EventArgs e)
    {
      using (BusinessRuleFindFieldDialog ruleFindFieldDialog = new BusinessRuleFindFieldDialog(this.session, (string[]) null, true, string.Empty, true, true))
      {
        if (ruleFindFieldDialog.ShowDialog((IWin32Window) this) != DialogResult.OK || ruleFindFieldDialog.SelectedRequiredFields.Length == 0)
          return;
        this.selectTriggerField(ruleFindFieldDialog.SelectedRequiredFields[ruleFindFieldDialog.SelectedRequiredFields.Length - 1]);
        this.txtFieldID.Focus();
      }
    }

    private void lvwAssignments_DoubleClick(object sender, EventArgs e) => this.editAssignment();

    private void editAssignment()
    {
      if (this.lvwAssignments.SelectedItems.Count == 0)
        return;
      ListViewItem selectedItem = this.lvwAssignments.SelectedItems[0];
      using (TriggerAssignmentEditor assignmentEditor = new TriggerAssignmentEditor(this.fieldSettings, (TriggerAssignment) selectedItem.Tag))
      {
        if (assignmentEditor.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        selectedItem.Tag = (object) assignmentEditor.TriggerAssignment;
        this.updateListItemForAssignment(selectedItem);
      }
    }

    private void txtFieldID_Validating(object sender, CancelEventArgs e)
    {
      if (this.selectTriggerField(this.txtFieldID.Text.Trim()))
        return;
      e.Cancel = true;
    }

    private bool selectTriggerField(string fieldId)
    {
      if (this.triggerField != null && string.Compare(this.triggerField.FieldID, fieldId, true) == 0)
        return true;
      FieldDefinition fieldDefinition = (FieldDefinition) null;
      if (fieldId != "")
      {
        fieldDefinition = EncompassFields.GetField(fieldId, this.fieldSettings);
        if (fieldDefinition == null)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The value '" + fieldId + "' is not a valid field ID.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return false;
        }
        if (fieldDefinition is VirtualField)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The field '" + fieldId + "' is a virtual field and can only be used to create an Email Trigger.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          this.cboActionType.SelectedItem = (object) new TriggerEventActionNameProvider().GetName((object) TriggerActionType.Email);
          this.cboActionType.Enabled = false;
        }
        else
          this.cboActionType.Enabled = true;
      }
      this.triggerField = fieldDefinition;
      this.updateUIForField();
      return true;
    }

    private void updateUIForField()
    {
      if (this.triggerField == null)
      {
        this.txtFieldID.Text = "";
        this.txtDescription.Text = "";
        this.cboCondType.SelectedIndex = -1;
        this.cboCondType.Enabled = false;
      }
      else
      {
        this.txtFieldID.Text = this.triggerField.FieldID;
        this.txtDescription.Text = this.triggerField.Description;
        this.cboCondType.Enabled = true;
        if (this.triggerField.IsNumeric() || this.triggerField.IsDateValued())
        {
          this.populateConditionTypes(TriggerEventEditor.dateNumConditions);
          this.txtCondValue.MaxLength = 10;
        }
        else
        {
          this.populateConditionTypes(TriggerEventEditor.textConditions);
          this.txtCondValue.MaxLength = 200;
        }
        if (!this.triggerField.Options.RequireValueFromList)
          return;
        this.populateValueSelector();
        this.populateOptionsList();
      }
    }

    private void populateOptionsList()
    {
      this.lstCondOptions.Items.Clear();
      this.lstCondOptions.Items.Add((object) new FieldOption("<Empty>", ""));
      foreach (FieldOption option in this.triggerField.Options)
        this.lstCondOptions.Items.Add((object) option);
    }

    private void populateAllowedActions()
    {
      string name = this.cboActivationType.SelectedItem.ToString();
      if (this.cboActivationType.SelectedItem.ToString().Equals("Rate Lock actions") || this.cboActivationType.SelectedItem.ToString().Equals("TPO actions"))
        name = this.cboActions.SelectedItem.ToString();
      TriggerActionType[] triggerActionTypeArray;
      switch ((TriggerActivationType) new TriggerActivationNameProvider().GetValue(name))
      {
        case TriggerActivationType.FieldModified:
          triggerActionTypeArray = new TriggerActionType[7]
          {
            TriggerActionType.Assign,
            TriggerActionType.Copy,
            TriggerActionType.CompleteTasks,
            TriggerActionType.Email,
            TriggerActionType.AdvancedCode,
            TriggerActionType.ApplyLoanTemplate,
            TriggerActionType.AddSpecialFeatureCode
          };
          break;
        case TriggerActivationType.MilestoneCompleted:
        case TriggerActivationType.LockRequested:
        case TriggerActivationType.LockConfirmed:
        case TriggerActivationType.LockDenied:
          triggerActionTypeArray = new TriggerActionType[6]
          {
            TriggerActionType.Assign,
            TriggerActionType.CompleteTasks,
            TriggerActionType.Email,
            TriggerActionType.AdvancedCode,
            TriggerActionType.ApplyLoanTemplate,
            TriggerActionType.AddSpecialFeatureCode
          };
          break;
        default:
          triggerActionTypeArray = new TriggerActionType[8]
          {
            TriggerActionType.Assign,
            TriggerActionType.Copy,
            TriggerActionType.CompleteTasks,
            TriggerActionType.Email,
            TriggerActionType.AdvancedCode,
            TriggerActionType.LoanMove,
            TriggerActionType.ApplyLoanTemplate,
            TriggerActionType.AddSpecialFeatureCode
          };
          break;
      }
      TriggerEventActionNameProvider actionNameProvider = new TriggerEventActionNameProvider();
      object selectedItem = this.cboActionType.SelectedItem;
      this.cboActionType.Items.Clear();
      foreach (TriggerActionType triggerActionType in triggerActionTypeArray)
        this.cboActionType.Items.Add((object) actionNameProvider.GetName((object) triggerActionType));
      if (selectedItem != null)
      {
        try
        {
          this.cboActionType.SelectedItem = selectedItem;
        }
        catch
        {
        }
      }
      if (this.cboActionType.SelectedIndex >= 0)
        return;
      this.cboActionType.SelectedIndex = 0;
    }

    private void populateValueSelector()
    {
      this.cboCondValue.Items.Clear();
      this.cboCondValue.Items.Add((object) FieldOption.Empty);
      foreach (FieldOption option in this.triggerField.Options)
        this.cboCondValue.Items.Add((object) option);
      this.cboCondValue.SelectedIndex = 0;
    }

    private void populateConditionTypes(TriggerConditionType[] types)
    {
      TriggerEventConditionNameProvider conditionNameProvider = new TriggerEventConditionNameProvider();
      this.cboCondType.Items.Clear();
      foreach (TriggerConditionType type in types)
        this.cboCondType.Items.Add((object) conditionNameProvider.GetName((object) type));
      this.cboCondType.SelectedIndex = 0;
    }

    private void lvwCondValues_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnEditValue.Enabled = this.btnRemoveValue.Enabled = this.lvwCondValues.SelectedItems.Count > 0;
    }

    private void lvwCopyTo_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnEditCopyTo.Enabled = this.btnRemoveCopyTo.Enabled = this.lvwCopyTo.SelectedItems.Count > 0;
    }

    private void validateConditionTextValue(object sender, CancelEventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      if (this.validateFormat(textBox.Text))
        return;
      int num = (int) Utils.Dialog((IWin32Window) this, "The value '" + textBox.Text + "' is not valid for this field.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      e.Cancel = true;
    }

    private void lvwCondValues_AfterLabelEdit(object sender, LabelEditEventArgs e)
    {
      ThreadPool.QueueUserWorkItem(new WaitCallback(this.validateConditionListEdit), (object) e.Item);
    }

    private void validateConditionListEdit(object indexAsObj)
    {
      if (this.InvokeRequired)
      {
        this.Invoke((Delegate) new WaitCallback(this.validateConditionListEdit), indexAsObj);
      }
      else
      {
        int index = (int) indexAsObj;
        if (index >= this.lvwCondValues.Items.Count)
          return;
        ListViewItem listViewItem = this.lvwCondValues.Items[index];
        if (listViewItem.Text.Trim() == "")
        {
          listViewItem.Remove();
        }
        else
        {
          if (this.validateFormat(listViewItem.Text))
            return;
          int num = (int) Utils.Dialog((IWin32Window) this, "The value '" + listViewItem.Text + "' is not valid for this field.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          listViewItem.BeginEdit();
        }
      }
    }

    private bool validateFormat(string text)
    {
      if (text.Trim() == "")
        return true;
      try
      {
        if (this.triggerField.ValidateFormat(text) != "")
          return true;
      }
      catch
      {
      }
      return false;
    }

    private void btnAddEmail_Click(object sender, EventArgs e)
    {
      using (TriggerEmailDialog triggerEmailDialog = new TriggerEmailDialog(this.session))
      {
        if (triggerEmailDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.lvwEmails.Items.Add(this.createListItemForEmailTemplate(triggerEmailDialog.GetEmailTemplate()));
      }
    }

    private void btnEditEmail_Click(object sender, EventArgs e)
    {
      if (this.lvwEmails.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select an item from the list to edit.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
        this.editCurrentEmailTemplate();
    }

    private void editCurrentEmailTemplate()
    {
      ListViewItem selectedItem = this.lvwEmails.SelectedItems[0];
      using (TriggerEmailDialog triggerEmailDialog = new TriggerEmailDialog(selectedItem.Tag as TriggerEmailTemplate, this.session))
      {
        if (triggerEmailDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        selectedItem.Tag = (object) triggerEmailDialog.GetEmailTemplate();
        this.updateListItemForEmailTemplate(selectedItem);
      }
    }

    private void btnRemoveEmail_Click(object sender, EventArgs e)
    {
      if (this.lvwEmails.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select an item from the list to remove.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        while (this.lvwEmails.SelectedItems.Count > 0)
          this.lvwEmails.Items.Remove(this.lvwEmails.SelectedItems[0]);
      }
    }

    private void lvwEmails_DoubleClick(object sender, EventArgs e)
    {
      if (this.lvwEmails.SelectedItems.Count == 0)
        return;
      this.editCurrentEmailTemplate();
    }

    private void lvwEmails_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnEditEmail.Enabled = this.lvwEmails.SelectedItems.Count == 1;
      this.btnRemoveEmail.Enabled = this.lvwEmails.SelectedItems.Count > 0;
    }

    private void lvwAssignments_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnEditAssignment.Enabled = this.lvwAssignments.SelectedItems.Count == 1;
      this.btnRemoveAssignment.Enabled = this.lvwAssignments.SelectedItems.Count > 0;
    }

    private void cboActivationType_SelectedIndexChanged(object sender, EventArgs e)
    {
      TriggerActivationNameProvider activationNameProvider = new TriggerActivationNameProvider();
      this.pnlFieldActivation.Visible = this.pnlMilestoneActivation.Visible = this.lblActionType.Visible = this.cboActions.Visible = false;
      if (this.cboActivationType.SelectedItem.ToString().Equals("Rate Lock actions"))
      {
        this.lblActionType.Visible = this.cboActions.Visible = true;
        this.lblActionType.Text = "Rate Lock Action";
        this.cboActions.DataSource = (object) activationNameProvider.GetActivationTypesByParent("Rate Lock actions");
      }
      else if (this.cboActivationType.SelectedItem.ToString().Equals("TPO actions"))
      {
        this.lblActionType.Visible = this.cboActions.Visible = true;
        this.lblActionType.Text = "TPO Action";
        List<string> stringList = new List<string>((IEnumerable<string>) activationNameProvider.GetActivationTypesByParent("TPO actions"));
        string fromActivationType1 = activationNameProvider.GetDescriptionFromActivationType(TriggerActivationType.ViewPurchaseAdvice);
        string fromActivationType2 = activationNameProvider.GetDescriptionFromActivationType(TriggerActivationType.SubmitPurchase);
        string fromActivationType3 = activationNameProvider.GetDescriptionFromActivationType(TriggerActivationType.FloatLock);
        string fromActivationType4 = activationNameProvider.GetDescriptionFromActivationType(TriggerActivationType.ReLockLock);
        string fromActivationType5 = activationNameProvider.GetDescriptionFromActivationType(TriggerActivationType.ChangeRequestOB);
        string fromActivationType6 = activationNameProvider.GetDescriptionFromActivationType(TriggerActivationType.CancelLock);
        string fromActivationType7 = activationNameProvider.GetDescriptionFromActivationType(TriggerActivationType.RePriceLock);
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
        if (stringList.Contains(fromActivationType6))
          stringList.Remove(fromActivationType6);
        if (stringList.Contains(fromActivationType7))
          stringList.Remove(fromActivationType7);
        this.cboActions.DataSource = (object) stringList;
      }
      else
      {
        switch ((TriggerActivationType) activationNameProvider.GetValue(string.Concat(this.cboActivationType.SelectedItem)))
        {
          case TriggerActivationType.FieldModified:
            this.pnlFieldActivation.Visible = true;
            break;
          case TriggerActivationType.MilestoneCompleted:
            this.pnlMilestoneActivation.Visible = true;
            break;
        }
      }
      this.populateAllowedActions();
    }

    private void TriggerEventEditor_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Escape)
        return;
      this.btnCancel.PerformClick();
    }

    private void btnSelectLoanTemplate_Click(object sender, EventArgs e)
    {
      string text = this.txtLoanTemplate.Text;
      FileSystemEntry defaultFolder = (FileSystemEntry) null;
      if (text != string.Empty)
      {
        try
        {
          defaultFolder = FileSystemEntry.Parse(text, this.session.UserID).ParentFolder;
        }
        catch
        {
        }
      }
      TemplateSelectionDialog templateSelectionDialog = new TemplateSelectionDialog(this.session, EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanTemplate, defaultFolder, true);
      if (templateSelectionDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      FileSystemEntry selectedItem = templateSelectionDialog.SelectedItem;
      if (!selectedItem.IsPublic)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You can only select public loan template to apply.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.txtLoanTemplate.Text = selectedItem.ToDisplayString();
        this.txtLoanTemplate.Tag = (object) selectedItem.Path.Replace("Public:", string.Empty);
      }
    }

    private void btnSelectSpecialFeatureCode_Click(object sender, EventArgs e)
    {
      SpecialFeatureCodeSelectionDialog codeSelectionDialog = new SpecialFeatureCodeSelectionDialog(Session.DefaultInstance, (Dictionary<string, string>) this.txtSpecialFeatureCodes.Tag);
      if (codeSelectionDialog.ShowDialog() != DialogResult.OK)
        return;
      this.txtSpecialFeatureCodes.Clear();
      this.txtSpecialFeatureCodes.Tag = (object) null;
      if (codeSelectionDialog.listSpecialFeatureCodeDefinition.Count == 0)
        return;
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      this.txtSpecialFeatureCodes.Text = codeSelectionDialog.listSpecialFeatureCodeDefinition.First<SpecialFeatureCodeDefinition>().Code;
      dictionary.Add(codeSelectionDialog.listSpecialFeatureCodeDefinition.First<SpecialFeatureCodeDefinition>().ID, codeSelectionDialog.listSpecialFeatureCodeDefinition.First<SpecialFeatureCodeDefinition>().Code);
      if (codeSelectionDialog.listSpecialFeatureCodeDefinition.Count == 1)
      {
        this.txtSpecialFeatureCodes.Tag = (object) dictionary;
      }
      else
      {
        foreach (SpecialFeatureCodeDefinition featureCodeDefinition in codeSelectionDialog.listSpecialFeatureCodeDefinition.Skip<SpecialFeatureCodeDefinition>(1))
        {
          this.txtSpecialFeatureCodes.Text = this.txtSpecialFeatureCodes.Text + " | " + featureCodeDefinition.Code;
          dictionary.Add(featureCodeDefinition.ID, featureCodeDefinition.Code);
        }
        this.txtSpecialFeatureCodes.Tag = (object) dictionary;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.groupBox1 = new GroupBox();
      this.pnlActionSpecialFeatureCodes = new Panel();
      this.btnSelectSpecialFeatureCode = new StandardIconButton();
      this.txtSpecialFeatureCodes = new TextBox();
      this.label18 = new Label();
      this.pnlActionMoveLoan = new Panel();
      this.cboLoanFolder = new ComboBox();
      this.lblSelectLoanFolder = new Label();
      this.pnlActionLoanTemplate = new Panel();
      this.btnSelectLoanTemplate = new StandardIconButton();
      this.txtLoanTemplate = new TextBox();
      this.lblSelectLoanTemplate = new Label();
      this.pnlActionType = new Panel();
      this.cboActionType = new ComboBox();
      this.label6 = new Label();
      this.pnlActionTasks = new Panel();
      this.lstTasks = new CheckedListBox();
      this.label14 = new Label();
      this.pnlActionEmail = new Panel();
      this.btnEditEmail = new Button();
      this.lvwEmails = new ListView();
      this.columnHeader2 = new ColumnHeader();
      this.btnAddEmail = new Button();
      this.btnRemoveEmail = new Button();
      this.label15 = new Label();
      this.pnlActionCopy = new Panel();
      this.btnEditCopyTo = new Button();
      this.lvwCopyTo = new ListView();
      this.columnHeader3 = new ColumnHeader();
      this.columnHeader4 = new ColumnHeader();
      this.btnAddCopyTo = new Button();
      this.btnRemoveCopyTo = new Button();
      this.label8 = new Label();
      this.pnlActionAdvanced = new Panel();
      this.txtAdvancedCode = new TextBox();
      this.label12 = new Label();
      this.pnlActionAssign = new Panel();
      this.btnEditAssignment = new Button();
      this.lvwAssignments = new ListView();
      this.columnHeader5 = new ColumnHeader();
      this.columnHeader6 = new ColumnHeader();
      this.btnAddAssignment = new Button();
      this.btnRemoveAssignment = new Button();
      this.label9 = new Label();
      this.pnlCondList = new Panel();
      this.btnEditValue = new Button();
      this.lvwCondValues = new ListView();
      this.columnHeader1 = new ColumnHeader();
      this.btnAddValue = new Button();
      this.btnRemoveValue = new Button();
      this.label7 = new Label();
      this.pnlCondRange = new Panel();
      this.label5 = new Label();
      this.txtCondMax = new TextBox();
      this.txtCondMin = new TextBox();
      this.label4 = new Label();
      this.pnlCondValue = new Panel();
      this.txtCondValue = new TextBox();
      this.label3 = new Label();
      this.panel2 = new Panel();
      this.txtDescription = new TextBox();
      this.label10 = new Label();
      this.btnFindField = new Button();
      this.txtFieldID = new TextBox();
      this.cboCondType = new ComboBox();
      this.label2 = new Label();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.groupBox2 = new GroupBox();
      this.cboActions = new ComboBox();
      this.lblActionType = new Label();
      this.pnlFieldActivation = new Panel();
      this.pnlCondValueSelect = new Panel();
      this.cboCondValue = new ComboBox();
      this.label11 = new Label();
      this.pnlCondOptions = new Panel();
      this.lstCondOptions = new CheckedListBox();
      this.label13 = new Label();
      this.cboActivationType = new ComboBox();
      this.label16 = new Label();
      this.pnlMilestoneActivation = new Panel();
      this.cboMilestone = new MilestoneDropdownBox();
      this.label17 = new Label();
      this.panel1 = new Panel();
      this.groupBox1.SuspendLayout();
      this.pnlActionSpecialFeatureCodes.SuspendLayout();
      ((ISupportInitialize) this.btnSelectSpecialFeatureCode).BeginInit();
      this.pnlActionMoveLoan.SuspendLayout();
      this.pnlActionLoanTemplate.SuspendLayout();
      ((ISupportInitialize) this.btnSelectLoanTemplate).BeginInit();
      this.pnlActionType.SuspendLayout();
      this.pnlActionTasks.SuspendLayout();
      this.pnlActionEmail.SuspendLayout();
      this.pnlActionCopy.SuspendLayout();
      this.pnlActionAdvanced.SuspendLayout();
      this.pnlActionAssign.SuspendLayout();
      this.pnlCondList.SuspendLayout();
      this.pnlCondRange.SuspendLayout();
      this.pnlCondValue.SuspendLayout();
      this.panel2.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.pnlFieldActivation.SuspendLayout();
      this.pnlCondValueSelect.SuspendLayout();
      this.pnlCondOptions.SuspendLayout();
      this.pnlMilestoneActivation.SuspendLayout();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(9, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(160, 25);
      this.label1.TabIndex = 0;
      this.label1.Text = "Trigger Field ID";
      this.groupBox1.Controls.Add((Control) this.pnlActionSpecialFeatureCodes);
      this.groupBox1.Controls.Add((Control) this.pnlActionMoveLoan);
      this.groupBox1.Controls.Add((Control) this.pnlActionLoanTemplate);
      this.groupBox1.Controls.Add((Control) this.pnlActionType);
      this.groupBox1.Controls.Add((Control) this.pnlActionTasks);
      this.groupBox1.Controls.Add((Control) this.pnlActionEmail);
      this.groupBox1.Controls.Add((Control) this.pnlActionCopy);
      this.groupBox1.Controls.Add((Control) this.pnlActionAdvanced);
      this.groupBox1.Controls.Add((Control) this.pnlActionAssign);
      this.groupBox1.Dock = DockStyle.Top;
      this.groupBox1.Location = new Point(10, 297);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(504, 187);
      this.groupBox1.TabIndex = 2;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Action";
      this.pnlActionSpecialFeatureCodes.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlActionSpecialFeatureCodes.Controls.Add((Control) this.btnSelectSpecialFeatureCode);
      this.pnlActionSpecialFeatureCodes.Controls.Add((Control) this.txtSpecialFeatureCodes);
      this.pnlActionSpecialFeatureCodes.Controls.Add((Control) this.label18);
      this.pnlActionSpecialFeatureCodes.Location = new Point(3, 49);
      this.pnlActionSpecialFeatureCodes.Name = "pnlActionSpecialFeatureCodes";
      this.pnlActionSpecialFeatureCodes.Size = new Size(497, 132);
      this.pnlActionSpecialFeatureCodes.TabIndex = 24;
      this.btnSelectSpecialFeatureCode.BackColor = Color.Transparent;
      this.btnSelectSpecialFeatureCode.Location = new Point(429, 2);
      this.btnSelectSpecialFeatureCode.MouseDownImage = (Image) null;
      this.btnSelectSpecialFeatureCode.Name = "btnSelectSpecialFeatureCode";
      this.btnSelectSpecialFeatureCode.Size = new Size(16, 16);
      this.btnSelectSpecialFeatureCode.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnSelectSpecialFeatureCode.TabIndex = 23;
      this.btnSelectSpecialFeatureCode.TabStop = false;
      this.btnSelectSpecialFeatureCode.Click += new EventHandler(this.btnSelectSpecialFeatureCode_Click);
      this.txtSpecialFeatureCodes.Location = new Point(133, 2);
      this.txtSpecialFeatureCodes.Name = "txtSpecialFeatureCodes";
      this.txtSpecialFeatureCodes.ReadOnly = true;
      this.txtSpecialFeatureCodes.Size = new Size(287, 33);
      this.txtSpecialFeatureCodes.TabIndex = 22;
      this.txtSpecialFeatureCodes.TabStop = false;
      this.label18.AutoSize = true;
      this.label18.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label18.Location = new Point(10, 6);
      this.label18.Name = "label18";
      this.label18.Size = new Size(233, 26);
      this.label18.TabIndex = 2;
      this.label18.Text = "Special Feature Codes";
      this.pnlActionMoveLoan.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlActionMoveLoan.Controls.Add((Control) this.cboLoanFolder);
      this.pnlActionMoveLoan.Controls.Add((Control) this.lblSelectLoanFolder);
      this.pnlActionMoveLoan.Location = new Point(3, 49);
      this.pnlActionMoveLoan.Name = "pnlActionMoveLoan";
      this.pnlActionMoveLoan.Size = new Size(497, 132);
      this.pnlActionMoveLoan.TabIndex = 15;
      this.cboLoanFolder.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboLoanFolder.FormattingEnabled = true;
      this.cboLoanFolder.Location = new Point(132, 6);
      this.cboLoanFolder.Name = "cboLoanFolder";
      this.cboLoanFolder.Size = new Size(287, 22);
      this.cboLoanFolder.TabIndex = 24;
      this.lblSelectLoanFolder.AutoSize = true;
      this.lblSelectLoanFolder.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblSelectLoanFolder.Location = new Point(10, 6);
      this.lblSelectLoanFolder.Name = "lblSelectLoanFolder";
      this.lblSelectLoanFolder.Size = new Size(96, 13);
      this.lblSelectLoanFolder.TabIndex = 2;
      this.lblSelectLoanFolder.Text = "Select Folder";
      this.pnlActionLoanTemplate.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlActionLoanTemplate.Controls.Add((Control) this.btnSelectLoanTemplate);
      this.pnlActionLoanTemplate.Controls.Add((Control) this.txtLoanTemplate);
      this.pnlActionLoanTemplate.Controls.Add((Control) this.lblSelectLoanTemplate);
      this.pnlActionLoanTemplate.Location = new Point(3, 49);
      this.pnlActionLoanTemplate.Name = "pnlActionLoanTemplate";
      this.pnlActionLoanTemplate.Size = new Size(497, 132);
      this.pnlActionLoanTemplate.TabIndex = 14;
      this.btnSelectLoanTemplate.BackColor = Color.Transparent;
      this.btnSelectLoanTemplate.Location = new Point(429, 2);
      this.btnSelectLoanTemplate.MouseDownImage = (Image) null;
      this.btnSelectLoanTemplate.Name = "btnSelectLoanTemplate";
      this.btnSelectLoanTemplate.Size = new Size(16, 16);
      this.btnSelectLoanTemplate.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnSelectLoanTemplate.TabIndex = 23;
      this.btnSelectLoanTemplate.TabStop = false;
      this.btnSelectLoanTemplate.Click += new EventHandler(this.btnSelectLoanTemplate_Click);
      this.txtLoanTemplate.Location = new Point(133, 2);
      this.txtLoanTemplate.Name = "txtLoanTemplate";
      this.txtLoanTemplate.ReadOnly = true;
      this.txtLoanTemplate.Size = new Size(287, 20);
      this.txtLoanTemplate.TabIndex = 22;
      this.txtLoanTemplate.TabStop = false;
      this.lblSelectLoanTemplate.AutoSize = true;
      this.lblSelectLoanTemplate.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblSelectLoanTemplate.Location = new Point(10, 6);
      this.lblSelectLoanTemplate.Name = "lblSelectLoanTemplate";
      this.lblSelectLoanTemplate.Size = new Size(111, 13);
      this.lblSelectLoanTemplate.TabIndex = 2;
      this.lblSelectLoanTemplate.Text = "Select Loan Template";
      this.pnlActionType.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlActionType.Controls.Add((Control) this.cboActionType);
      this.pnlActionType.Controls.Add((Control) this.label6);
      this.pnlActionType.Location = new Point(3, 17);
      this.pnlActionType.Name = "pnlActionType";
      this.pnlActionType.Size = new Size(497, 30);
      this.pnlActionType.TabIndex = 1;
      this.cboActionType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboActionType.FormattingEnabled = true;
      this.cboActionType.Location = new Point(133, 4);
      this.cboActionType.Name = "cboActionType";
      this.cboActionType.Size = new Size(287, 22);
      this.cboActionType.TabIndex = 2;
      this.cboActionType.SelectedIndexChanged += new EventHandler(this.cboActionType_SelectedIndexChanged);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(10, 9);
      this.label6.Name = "label6";
      this.label6.Size = new Size(64, 14);
      this.label6.TabIndex = 1;
      this.label6.Text = "Action Type";
      this.pnlActionTasks.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlActionTasks.Controls.Add((Control) this.lstTasks);
      this.pnlActionTasks.Controls.Add((Control) this.label14);
      this.pnlActionTasks.Location = new Point(3, 49);
      this.pnlActionTasks.Name = "pnlActionTasks";
      this.pnlActionTasks.Size = new Size(497, 132);
      this.pnlActionTasks.TabIndex = 13;
      this.lstTasks.CheckOnClick = true;
      this.lstTasks.FormattingEnabled = true;
      this.lstTasks.Location = new Point(133, 3);
      this.lstTasks.Name = "lstTasks";
      this.lstTasks.Size = new Size(287, 109);
      this.lstTasks.TabIndex = 3;
      this.label14.AutoSize = true;
      this.label14.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label14.Location = new Point(10, 3);
      this.label14.Name = "label14";
      this.label14.Size = new Size(108, 13);
      this.label14.TabIndex = 2;
      this.label14.Text = "Complete these tasks";
      this.pnlActionEmail.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlActionEmail.Controls.Add((Control) this.btnEditEmail);
      this.pnlActionEmail.Controls.Add((Control) this.lvwEmails);
      this.pnlActionEmail.Controls.Add((Control) this.btnAddEmail);
      this.pnlActionEmail.Controls.Add((Control) this.btnRemoveEmail);
      this.pnlActionEmail.Controls.Add((Control) this.label15);
      this.pnlActionEmail.Location = new Point(3, 49);
      this.pnlActionEmail.Name = "pnlActionEmail";
      this.pnlActionEmail.Size = new Size(497, 132);
      this.pnlActionEmail.TabIndex = 14;
      this.btnEditEmail.Enabled = false;
      this.btnEditEmail.Location = new Point(426, 25);
      this.btnEditEmail.Name = "btnEditEmail";
      this.btnEditEmail.Size = new Size(62, 22);
      this.btnEditEmail.TabIndex = 3;
      this.btnEditEmail.Text = "&Edit";
      this.btnEditEmail.UseVisualStyleBackColor = true;
      this.btnEditEmail.Click += new EventHandler(this.btnEditEmail_Click);
      this.lvwEmails.Columns.AddRange(new ColumnHeader[1]
      {
        this.columnHeader2
      });
      this.lvwEmails.FullRowSelect = true;
      this.lvwEmails.HeaderStyle = ColumnHeaderStyle.None;
      this.lvwEmails.Location = new Point(133, 2);
      this.lvwEmails.MultiSelect = false;
      this.lvwEmails.Name = "lvwEmails";
      this.lvwEmails.Size = new Size(287, (int) sbyte.MaxValue);
      this.lvwEmails.TabIndex = 1;
      this.lvwEmails.UseCompatibleStateImageBehavior = false;
      this.lvwEmails.View = View.Details;
      this.lvwEmails.SelectedIndexChanged += new EventHandler(this.lvwEmails_SelectedIndexChanged);
      this.lvwEmails.DoubleClick += new EventHandler(this.lvwEmails_DoubleClick);
      this.columnHeader2.Text = "Subject";
      this.columnHeader2.Width = 259;
      this.btnAddEmail.Location = new Point(426, 2);
      this.btnAddEmail.Name = "btnAddEmail";
      this.btnAddEmail.Size = new Size(62, 22);
      this.btnAddEmail.TabIndex = 2;
      this.btnAddEmail.Text = "&Add";
      this.btnAddEmail.UseVisualStyleBackColor = true;
      this.btnAddEmail.Click += new EventHandler(this.btnAddEmail_Click);
      this.btnRemoveEmail.Enabled = false;
      this.btnRemoveEmail.Location = new Point(426, 48);
      this.btnRemoveEmail.Name = "btnRemoveEmail";
      this.btnRemoveEmail.Size = new Size(62, 22);
      this.btnRemoveEmail.TabIndex = 4;
      this.btnRemoveEmail.Text = "&Remove";
      this.btnRemoveEmail.UseVisualStyleBackColor = true;
      this.btnRemoveEmail.Click += new EventHandler(this.btnRemoveEmail_Click);
      this.label15.AutoSize = true;
      this.label15.Location = new Point(10, 6);
      this.label15.Name = "label15";
      this.label15.Size = new Size(73, 14);
      this.label15.TabIndex = 2;
      this.label15.Text = "Send Email(s)";
      this.pnlActionCopy.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlActionCopy.Controls.Add((Control) this.btnEditCopyTo);
      this.pnlActionCopy.Controls.Add((Control) this.lvwCopyTo);
      this.pnlActionCopy.Controls.Add((Control) this.btnAddCopyTo);
      this.pnlActionCopy.Controls.Add((Control) this.btnRemoveCopyTo);
      this.pnlActionCopy.Controls.Add((Control) this.label8);
      this.pnlActionCopy.Location = new Point(3, 49);
      this.pnlActionCopy.Name = "pnlActionCopy";
      this.pnlActionCopy.Size = new Size(497, 132);
      this.pnlActionCopy.TabIndex = 12;
      this.btnEditCopyTo.Enabled = false;
      this.btnEditCopyTo.Location = new Point(426, 25);
      this.btnEditCopyTo.Name = "btnEditCopyTo";
      this.btnEditCopyTo.Size = new Size(62, 22);
      this.btnEditCopyTo.TabIndex = 3;
      this.btnEditCopyTo.Text = "&Edit";
      this.btnEditCopyTo.UseVisualStyleBackColor = true;
      this.btnEditCopyTo.Click += new EventHandler(this.btnEditCopyTo_Click);
      this.lvwCopyTo.Columns.AddRange(new ColumnHeader[2]
      {
        this.columnHeader3,
        this.columnHeader4
      });
      this.lvwCopyTo.FullRowSelect = true;
      this.lvwCopyTo.HeaderStyle = ColumnHeaderStyle.Nonclickable;
      this.lvwCopyTo.LabelEdit = true;
      this.lvwCopyTo.Location = new Point(133, 2);
      this.lvwCopyTo.Name = "lvwCopyTo";
      this.lvwCopyTo.Size = new Size(287, (int) sbyte.MaxValue);
      this.lvwCopyTo.TabIndex = 1;
      this.lvwCopyTo.UseCompatibleStateImageBehavior = false;
      this.lvwCopyTo.View = View.Details;
      this.lvwCopyTo.AfterLabelEdit += new LabelEditEventHandler(this.lvwCopyTo_AfterLabelEdit);
      this.lvwCopyTo.SelectedIndexChanged += new EventHandler(this.lvwCopyTo_SelectedIndexChanged);
      this.columnHeader3.Text = "Field ID";
      this.columnHeader3.Width = 88;
      this.columnHeader4.Text = "Description";
      this.columnHeader4.Width = 163;
      this.btnAddCopyTo.Location = new Point(426, 2);
      this.btnAddCopyTo.Name = "btnAddCopyTo";
      this.btnAddCopyTo.Size = new Size(62, 22);
      this.btnAddCopyTo.TabIndex = 2;
      this.btnAddCopyTo.Text = "&Add";
      this.btnAddCopyTo.UseVisualStyleBackColor = true;
      this.btnAddCopyTo.Click += new EventHandler(this.btnAddCopyTo_Click);
      this.btnRemoveCopyTo.Enabled = false;
      this.btnRemoveCopyTo.Location = new Point(426, 48);
      this.btnRemoveCopyTo.Name = "btnRemoveCopyTo";
      this.btnRemoveCopyTo.Size = new Size(62, 22);
      this.btnRemoveCopyTo.TabIndex = 4;
      this.btnRemoveCopyTo.Text = "&Remove";
      this.btnRemoveCopyTo.UseVisualStyleBackColor = true;
      this.btnRemoveCopyTo.Click += new EventHandler(this.btnRemoveCopyTo_Click);
      this.label8.AutoSize = true;
      this.label8.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label8.Location = new Point(10, 6);
      this.label8.Name = "label8";
      this.label8.Size = new Size(47, 13);
      this.label8.TabIndex = 2;
      this.label8.Text = "Copy To";
      this.pnlActionAdvanced.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlActionAdvanced.Controls.Add((Control) this.txtAdvancedCode);
      this.pnlActionAdvanced.Controls.Add((Control) this.label12);
      this.pnlActionAdvanced.Location = new Point(3, 49);
      this.pnlActionAdvanced.Name = "pnlActionAdvanced";
      this.pnlActionAdvanced.Size = new Size(497, 132);
      this.pnlActionAdvanced.TabIndex = 2;
      this.txtAdvancedCode.AcceptsReturn = true;
      this.txtAdvancedCode.AcceptsTab = true;
      this.txtAdvancedCode.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtAdvancedCode.Location = new Point(133, 2);
      this.txtAdvancedCode.Multiline = true;
      this.txtAdvancedCode.Name = "txtAdvancedCode";
      this.txtAdvancedCode.ScrollBars = ScrollBars.Both;
      this.txtAdvancedCode.Size = new Size(355, 123);
      this.txtAdvancedCode.TabIndex = 3;
      this.txtAdvancedCode.WordWrap = false;
      this.label12.AutoSize = true;
      this.label12.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label12.Location = new Point(10, 6);
      this.label12.Name = "label12";
      this.label12.Size = new Size(84, 13);
      this.label12.TabIndex = 2;
      this.label12.Text = "Advanced Code";
      this.pnlActionAssign.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlActionAssign.Controls.Add((Control) this.btnEditAssignment);
      this.pnlActionAssign.Controls.Add((Control) this.lvwAssignments);
      this.pnlActionAssign.Controls.Add((Control) this.btnAddAssignment);
      this.pnlActionAssign.Controls.Add((Control) this.btnRemoveAssignment);
      this.pnlActionAssign.Controls.Add((Control) this.label9);
      this.pnlActionAssign.Location = new Point(3, 49);
      this.pnlActionAssign.Name = "pnlActionAssign";
      this.pnlActionAssign.Size = new Size(497, 132);
      this.pnlActionAssign.TabIndex = 2;
      this.btnEditAssignment.Enabled = false;
      this.btnEditAssignment.Location = new Point(426, 25);
      this.btnEditAssignment.Name = "btnEditAssignment";
      this.btnEditAssignment.Size = new Size(62, 22);
      this.btnEditAssignment.TabIndex = 3;
      this.btnEditAssignment.Text = "&Edit";
      this.btnEditAssignment.UseVisualStyleBackColor = true;
      this.btnEditAssignment.Click += new EventHandler(this.btnEditAssignment_Click);
      this.lvwAssignments.Columns.AddRange(new ColumnHeader[2]
      {
        this.columnHeader5,
        this.columnHeader6
      });
      this.lvwAssignments.FullRowSelect = true;
      this.lvwAssignments.HeaderStyle = ColumnHeaderStyle.Nonclickable;
      this.lvwAssignments.HideSelection = false;
      this.lvwAssignments.Location = new Point(133, 2);
      this.lvwAssignments.Name = "lvwAssignments";
      this.lvwAssignments.Size = new Size(287, (int) sbyte.MaxValue);
      this.lvwAssignments.TabIndex = 1;
      this.lvwAssignments.UseCompatibleStateImageBehavior = false;
      this.lvwAssignments.View = View.Details;
      this.lvwAssignments.SelectedIndexChanged += new EventHandler(this.lvwAssignments_SelectedIndexChanged);
      this.lvwAssignments.DoubleClick += new EventHandler(this.lvwAssignments_DoubleClick);
      this.columnHeader5.Text = "Field ID";
      this.columnHeader5.Width = 93;
      this.columnHeader6.Text = "Value";
      this.columnHeader6.Width = 146;
      this.btnAddAssignment.Location = new Point(426, 2);
      this.btnAddAssignment.Name = "btnAddAssignment";
      this.btnAddAssignment.Size = new Size(62, 22);
      this.btnAddAssignment.TabIndex = 2;
      this.btnAddAssignment.Text = "&Add";
      this.btnAddAssignment.UseVisualStyleBackColor = true;
      this.btnAddAssignment.Click += new EventHandler(this.btnAddAssignment_Click);
      this.btnRemoveAssignment.Enabled = false;
      this.btnRemoveAssignment.Location = new Point(426, 48);
      this.btnRemoveAssignment.Name = "btnRemoveAssignment";
      this.btnRemoveAssignment.Size = new Size(62, 22);
      this.btnRemoveAssignment.TabIndex = 4;
      this.btnRemoveAssignment.Text = "&Remove";
      this.btnRemoveAssignment.UseVisualStyleBackColor = true;
      this.btnRemoveAssignment.Click += new EventHandler(this.btnRemoveAssignment_Click);
      this.label9.AutoSize = true;
      this.label9.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label9.Location = new Point(10, 6);
      this.label9.Name = "label9";
      this.label9.Size = new Size(66, 13);
      this.label9.TabIndex = 2;
      this.label9.Text = "Assignments";
      this.pnlCondList.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlCondList.Controls.Add((Control) this.btnEditValue);
      this.pnlCondList.Controls.Add((Control) this.lvwCondValues);
      this.pnlCondList.Controls.Add((Control) this.btnAddValue);
      this.pnlCondList.Controls.Add((Control) this.btnRemoveValue);
      this.pnlCondList.Controls.Add((Control) this.label7);
      this.pnlCondList.Location = new Point(0, 85);
      this.pnlCondList.Name = "pnlCondList";
      this.pnlCondList.Size = new Size(496, 132);
      this.pnlCondList.TabIndex = 2;
      this.btnEditValue.Enabled = false;
      this.btnEditValue.Location = new Point(426, 28);
      this.btnEditValue.Name = "btnEditValue";
      this.btnEditValue.Size = new Size(62, 24);
      this.btnEditValue.TabIndex = 3;
      this.btnEditValue.Text = "&Edit";
      this.btnEditValue.UseVisualStyleBackColor = true;
      this.btnEditValue.Click += new EventHandler(this.btnEditValue_Click);
      this.lvwCondValues.Columns.AddRange(new ColumnHeader[1]
      {
        this.columnHeader1
      });
      this.lvwCondValues.FullRowSelect = true;
      this.lvwCondValues.HeaderStyle = ColumnHeaderStyle.None;
      this.lvwCondValues.HideSelection = false;
      this.lvwCondValues.LabelEdit = true;
      this.lvwCondValues.Location = new Point(133, 2);
      this.lvwCondValues.Name = "lvwCondValues";
      this.lvwCondValues.Size = new Size(287, (int) sbyte.MaxValue);
      this.lvwCondValues.TabIndex = 1;
      this.lvwCondValues.UseCompatibleStateImageBehavior = false;
      this.lvwCondValues.View = View.Details;
      this.lvwCondValues.AfterLabelEdit += new LabelEditEventHandler(this.lvwCondValues_AfterLabelEdit);
      this.lvwCondValues.SelectedIndexChanged += new EventHandler(this.lvwCondValues_SelectedIndexChanged);
      this.columnHeader1.Text = "Values";
      this.columnHeader1.Width = 258;
      this.btnAddValue.Location = new Point(426, 2);
      this.btnAddValue.Name = "btnAddValue";
      this.btnAddValue.Size = new Size(62, 24);
      this.btnAddValue.TabIndex = 2;
      this.btnAddValue.Text = "&Add";
      this.btnAddValue.UseVisualStyleBackColor = true;
      this.btnAddValue.Click += new EventHandler(this.btnAddValue_Click);
      this.btnRemoveValue.Enabled = false;
      this.btnRemoveValue.Location = new Point(426, 54);
      this.btnRemoveValue.Name = "btnRemoveValue";
      this.btnRemoveValue.Size = new Size(62, 24);
      this.btnRemoveValue.TabIndex = 4;
      this.btnRemoveValue.Text = "&Remove";
      this.btnRemoveValue.UseVisualStyleBackColor = true;
      this.btnRemoveValue.Click += new EventHandler(this.btnRemoveValue_Click);
      this.label7.AutoSize = true;
      this.label7.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label7.Location = new Point(10, 6);
      this.label7.Name = "label7";
      this.label7.Size = new Size(42, 13);
      this.label7.TabIndex = 2;
      this.label7.Text = "Values:";
      this.pnlCondRange.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlCondRange.Controls.Add((Control) this.label5);
      this.pnlCondRange.Controls.Add((Control) this.txtCondMax);
      this.pnlCondRange.Controls.Add((Control) this.txtCondMin);
      this.pnlCondRange.Controls.Add((Control) this.label4);
      this.pnlCondRange.Location = new Point(0, 85);
      this.pnlCondRange.Name = "pnlCondRange";
      this.pnlCondRange.Size = new Size(494, 132);
      this.pnlCondRange.TabIndex = 3;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(226, 6);
      this.label5.Name = "label5";
      this.label5.Size = new Size(11, 14);
      this.label5.TabIndex = 5;
      this.label5.Text = "-";
      this.txtCondMax.Location = new Point(238, 2);
      this.txtCondMax.Name = "txtCondMax";
      this.txtCondMax.Size = new Size(90, 20);
      this.txtCondMax.TabIndex = 2;
      this.txtCondMax.Validating += new CancelEventHandler(this.validateConditionTextValue);
      this.txtCondMin.Location = new Point(133, 2);
      this.txtCondMin.Name = "txtCondMin";
      this.txtCondMin.Size = new Size(91, 20);
      this.txtCondMin.TabIndex = 1;
      this.txtCondMin.Validating += new CancelEventHandler(this.validateConditionTextValue);
      this.label4.AutoSize = true;
      this.label4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(10, 6);
      this.label4.Name = "label4";
      this.label4.Size = new Size(42, 13);
      this.label4.TabIndex = 2;
      this.label4.Text = "Range:";
      this.pnlCondValue.Controls.Add((Control) this.txtCondValue);
      this.pnlCondValue.Controls.Add((Control) this.label3);
      this.pnlCondValue.Location = new Point(0, 83);
      this.pnlCondValue.Name = "pnlCondValue";
      this.pnlCondValue.Size = new Size(497, 134);
      this.pnlCondValue.TabIndex = 4;
      this.txtCondValue.Location = new Point(133, 2);
      this.txtCondValue.Name = "txtCondValue";
      this.txtCondValue.Size = new Size(287, 20);
      this.txtCondValue.TabIndex = 3;
      this.txtCondValue.Validating += new CancelEventHandler(this.validateConditionTextValue);
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(10, 6);
      this.label3.Name = "label3";
      this.label3.Size = new Size(34, 13);
      this.label3.TabIndex = 2;
      this.label3.Text = "Value";
      this.panel2.Controls.Add((Control) this.txtDescription);
      this.panel2.Controls.Add((Control) this.label10);
      this.panel2.Controls.Add((Control) this.btnFindField);
      this.panel2.Controls.Add((Control) this.txtFieldID);
      this.panel2.Controls.Add((Control) this.label1);
      this.panel2.Controls.Add((Control) this.cboCondType);
      this.panel2.Controls.Add((Control) this.label2);
      this.panel2.Dock = DockStyle.Top;
      this.panel2.Location = new Point(0, 0);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(496, 84);
      this.panel2.TabIndex = 1;
      this.txtDescription.Location = new Point(133, 29);
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.ReadOnly = true;
      this.txtDescription.Size = new Size(287, 20);
      this.txtDescription.TabIndex = 5;
      this.label10.AutoSize = true;
      this.label10.Location = new Point(9, 33);
      this.label10.Name = "label10";
      this.label10.Size = new Size(61, 14);
      this.label10.TabIndex = 4;
      this.label10.Text = "Description";
      this.btnFindField.Location = new Point(265, 2);
      this.btnFindField.Name = "btnFindField";
      this.btnFindField.Size = new Size(62, 22);
      this.btnFindField.TabIndex = 2;
      this.btnFindField.Text = "&Find";
      this.btnFindField.UseVisualStyleBackColor = true;
      this.btnFindField.Click += new EventHandler(this.btnFindField_Click);
      this.txtFieldID.Location = new Point(133, 3);
      this.txtFieldID.Name = "txtFieldID";
      this.txtFieldID.Size = new Size(128, 20);
      this.txtFieldID.TabIndex = 1;
      this.txtFieldID.Validating += new CancelEventHandler(this.txtFieldID_Validating);
      this.cboCondType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboCondType.Enabled = false;
      this.cboCondType.FormattingEnabled = true;
      this.cboCondType.Items.AddRange(new object[5]
      {
        (object) "Any change in field value",
        (object) "When field is set to a non-empty value",
        (object) "When field is set to a specific value",
        (object) "When field is set in a range of values",
        (object) "When field is set to an item from a list of values"
      });
      this.cboCondType.Location = new Point(133, 55);
      this.cboCondType.Name = "cboCondType";
      this.cboCondType.Size = new Size(287, 22);
      this.cboCondType.TabIndex = 3;
      this.cboCondType.SelectedIndexChanged += new EventHandler(this.cboCondType_SelectedIndexChanged);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(9, 59);
      this.label2.Name = "label2";
      this.label2.Size = new Size(47, 14);
      this.label2.TabIndex = 1;
      this.label2.Text = "Criterion";
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(358, 505);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 22);
      this.btnOK.TabIndex = 3;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.CausesValidation = false;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(437, 505);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.groupBox2.Controls.Add((Control) this.cboActions);
      this.groupBox2.Controls.Add((Control) this.lblActionType);
      this.groupBox2.Controls.Add((Control) this.pnlFieldActivation);
      this.groupBox2.Controls.Add((Control) this.cboActivationType);
      this.groupBox2.Controls.Add((Control) this.label16);
      this.groupBox2.Controls.Add((Control) this.pnlMilestoneActivation);
      this.groupBox2.Dock = DockStyle.Top;
      this.groupBox2.Location = new Point(10, 11);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(504, 277);
      this.groupBox2.TabIndex = 1;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Activation";
      this.cboActions.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboActions.FormattingEnabled = true;
      this.cboActions.Location = new Point(137, 49);
      this.cboActions.Name = "cboActions";
      this.cboActions.Size = new Size(287, 22);
      this.cboActions.TabIndex = 12;
      this.lblActionType.AutoSize = true;
      this.lblActionType.Location = new Point(13, 55);
      this.lblActionType.Name = "lblActionType";
      this.lblActionType.Size = new Size(60, 14);
      this.lblActionType.TabIndex = 11;
      this.lblActionType.Text = "TPO Action";
      this.pnlFieldActivation.Controls.Add((Control) this.panel2);
      this.pnlFieldActivation.Controls.Add((Control) this.pnlCondValueSelect);
      this.pnlFieldActivation.Controls.Add((Control) this.pnlCondRange);
      this.pnlFieldActivation.Controls.Add((Control) this.pnlCondOptions);
      this.pnlFieldActivation.Controls.Add((Control) this.pnlCondList);
      this.pnlFieldActivation.Controls.Add((Control) this.pnlCondValue);
      this.pnlFieldActivation.Location = new Point(4, 48);
      this.pnlFieldActivation.Name = "pnlFieldActivation";
      this.pnlFieldActivation.Size = new Size(496, 224);
      this.pnlFieldActivation.TabIndex = 9;
      this.pnlCondValueSelect.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlCondValueSelect.Controls.Add((Control) this.cboCondValue);
      this.pnlCondValueSelect.Controls.Add((Control) this.label11);
      this.pnlCondValueSelect.Location = new Point(0, 85);
      this.pnlCondValueSelect.Name = "pnlCondValueSelect";
      this.pnlCondValueSelect.Size = new Size(496, 132);
      this.pnlCondValueSelect.TabIndex = 5;
      this.cboCondValue.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboCondValue.FormattingEnabled = true;
      this.cboCondValue.Items.AddRange(new object[4]
      {
        (object) "Any change in field value",
        (object) "When field is set to a specific value",
        (object) "When field is set in a range of values",
        (object) "When field is set to an item from a list of values"
      });
      this.cboCondValue.Location = new Point(133, 2);
      this.cboCondValue.Name = "cboCondValue";
      this.cboCondValue.Size = new Size(287, 22);
      this.cboCondValue.TabIndex = 4;
      this.label11.AutoSize = true;
      this.label11.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label11.Location = new Point(10, 6);
      this.label11.Name = "label11";
      this.label11.Size = new Size(37, 13);
      this.label11.TabIndex = 2;
      this.label11.Text = "Value:";
      this.pnlCondOptions.Controls.Add((Control) this.lstCondOptions);
      this.pnlCondOptions.Controls.Add((Control) this.label13);
      this.pnlCondOptions.Location = new Point(0, 85);
      this.pnlCondOptions.Name = "pnlCondOptions";
      this.pnlCondOptions.Size = new Size(497, 141);
      this.pnlCondOptions.TabIndex = 6;
      this.lstCondOptions.CheckOnClick = true;
      this.lstCondOptions.FormattingEnabled = true;
      this.lstCondOptions.Location = new Point(133, 2);
      this.lstCondOptions.Name = "lstCondOptions";
      this.lstCondOptions.Size = new Size(287, 124);
      this.lstCondOptions.TabIndex = 3;
      this.label13.AutoSize = true;
      this.label13.Location = new Point(10, 2);
      this.label13.Name = "label13";
      this.label13.Size = new Size(43, 14);
      this.label13.TabIndex = 2;
      this.label13.Text = "Values:";
      this.cboActivationType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboActivationType.FormattingEnabled = true;
      this.cboActivationType.Location = new Point(137, 22);
      this.cboActivationType.Name = "cboActivationType";
      this.cboActivationType.Size = new Size(287, 22);
      this.cboActivationType.TabIndex = 8;
      this.cboActivationType.SelectedIndexChanged += new EventHandler(this.cboActivationType_SelectedIndexChanged);
      this.label16.AutoSize = true;
      this.label16.Location = new Point(13, 28);
      this.label16.Name = "label16";
      this.label16.Size = new Size(81, 14);
      this.label16.TabIndex = 7;
      this.label16.Text = "Activation Type";
      this.pnlMilestoneActivation.Controls.Add((Control) this.cboMilestone);
      this.pnlMilestoneActivation.Controls.Add((Control) this.label17);
      this.pnlMilestoneActivation.Location = new Point(4, 48);
      this.pnlMilestoneActivation.Name = "pnlMilestoneActivation";
      this.pnlMilestoneActivation.Size = new Size(496, 210);
      this.pnlMilestoneActivation.TabIndex = 10;
      this.cboMilestone.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboMilestone.Location = new Point(132, 4);
      this.cboMilestone.Name = "cboMilestone";
      this.cboMilestone.Size = new Size(287, 22);
      this.cboMilestone.TabIndex = 9;
      this.label17.AutoSize = true;
      this.label17.Location = new Point(8, 7);
      this.label17.Name = "label17";
      this.label17.Size = new Size(89, 14);
      this.label17.TabIndex = 8;
      this.label17.Text = "Trigger Milestone";
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(10, 288);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(504, 9);
      this.panel1.TabIndex = 8;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(524, 543);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.groupBox2);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (TriggerEventEditor);
      this.Padding = new Padding(10, 11, 10, 11);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add/Edit Field Event";
      this.KeyDown += new KeyEventHandler(this.TriggerEventEditor_KeyDown);
      this.groupBox1.ResumeLayout(false);
      this.pnlActionMoveLoan.ResumeLayout(false);
      this.pnlActionMoveLoan.PerformLayout();
      this.pnlActionLoanTemplate.ResumeLayout(false);
      this.pnlActionLoanTemplate.PerformLayout();
      ((ISupportInitialize) this.btnSelectLoanTemplate).EndInit();
      this.pnlActionType.ResumeLayout(false);
      this.pnlActionType.PerformLayout();
      this.pnlActionTasks.ResumeLayout(false);
      this.pnlActionTasks.PerformLayout();
      this.pnlActionEmail.ResumeLayout(false);
      this.pnlActionEmail.PerformLayout();
      this.pnlActionCopy.ResumeLayout(false);
      this.pnlActionCopy.PerformLayout();
      this.pnlActionAdvanced.ResumeLayout(false);
      this.pnlActionAdvanced.PerformLayout();
      this.pnlActionAssign.ResumeLayout(false);
      this.pnlActionAssign.PerformLayout();
      this.pnlCondList.ResumeLayout(false);
      this.pnlCondList.PerformLayout();
      this.pnlCondRange.ResumeLayout(false);
      this.pnlCondRange.PerformLayout();
      this.pnlCondValue.ResumeLayout(false);
      this.pnlCondValue.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.pnlFieldActivation.ResumeLayout(false);
      this.pnlCondValueSelect.ResumeLayout(false);
      this.pnlCondValueSelect.PerformLayout();
      this.pnlCondOptions.ResumeLayout(false);
      this.pnlCondOptions.PerformLayout();
      this.pnlMilestoneActivation.ResumeLayout(false);
      this.pnlMilestoneActivation.PerformLayout();
      this.pnlActionSpecialFeatureCodes.ResumeLayout(false);
      this.pnlActionSpecialFeatureCodes.PerformLayout();
      ((ISupportInitialize) this.btnSelectSpecialFeatureCode).EndInit();
      this.ResumeLayout(false);
    }

    private class TriggerCodeChecker : CodedTrigger
    {
      private string sourceCode;

      public TriggerCodeChecker(string sourceCode)
        : base("", new string[1]{ "36" }, (RuleCondition) PredefinedCondition.Empty)
      {
        this.sourceCode = sourceCode;
      }

      protected override string GetRuleDefinition() => this.sourceCode;
    }
  }
}
