// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PrintSelectionEventEditor
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.BusinessRules;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.Properties;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class PrintSelectionEventEditor : Form
  {
    private const string className = "PrintSelectionEventEditor";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private const string NewFieldPlaceholder = "<Enter Field ID>";
    private static PrintSelectionConditionType[] dateNumConditions = new PrintSelectionConditionType[3]
    {
      PrintSelectionConditionType.FixedValue,
      PrintSelectionConditionType.Range,
      PrintSelectionConditionType.ValueList
    };
    private static PrintSelectionConditionType[] textConditions = new PrintSelectionConditionType[2]
    {
      PrintSelectionConditionType.FixedValue,
      PrintSelectionConditionType.ValueList
    };
    private Sessions.Session session;
    private FieldSettings fieldSettings;
    private PrintSelectionEvent printSelectionEvent;
    private FieldDefinition printField;
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
    private Button btnOK;
    private Button btnCancel;
    private TextBox txtFieldID;
    private ListView lvwCondValues;
    private ColumnHeader columnHeader1;
    private GroupBox groupBox2;
    private Panel panel1;
    private Button btnFindField;
    private Button btnEditValue;
    private TextBox txtDescription;
    private Label label10;
    private Panel pnlCondValueSelect;
    private ComboBox cboCondValue;
    private Label label11;
    private Panel pnlCondOptions;
    private Label label13;
    private CheckedListBox lstCondOptions;
    private Button btnAddGroup;
    private Button btnAddForm;
    private GridView gridViewForms;
    private Button btnRemove;

    public PrintSelectionEventEditor(
      Sessions.Session session,
      FieldSettings fieldSettings,
      PrintSelectionEvent printSelectionEvent)
    {
      this.InitializeComponent();
      this.session = session;
      this.printSelectionEvent = printSelectionEvent;
      this.fieldSettings = fieldSettings;
      if (printSelectionEvent != null)
        this.loadPrintSelectionItem();
      else
        this.clearForm();
    }

    public PrintSelectionEvent PrintSelectionEvent => this.printSelectionEvent;

    private void clearForm()
    {
      this.txtFieldID.Text = "";
      this.cboCondType.SelectedIndex = 0;
      this.txtCondValue.Text = "";
      this.txtCondMin.Text = this.txtCondMax.Text = "";
      this.lvwCondValues.Items.Clear();
      this.gridViewForms.Items.Clear();
    }

    private void loadPrintSelectionItem()
    {
      PrintSelectionCondition condition = this.printSelectionEvent.Conditions[0];
      this.printField = EncompassFields.GetField(condition.FieldID, this.fieldSettings);
      if (this.printField == null)
        this.printField = (FieldDefinition) new UndefinedField(condition.FieldID, "Unknown Field");
      this.updateUIForField();
      ClientCommonUtils.PopulateDropdown(this.cboCondType, (object) new PrintSelectionEventEditor.PrintSelectionEventConditionNameProvider().GetName((object) condition.ConditionType), false);
      switch (condition)
      {
        case PrintSelectionFixedValueCondition _:
          if (this.printField.Options.RequireValueFromList)
          {
            this.cboCondValue.SelectedItem = (object) this.printField.Options.GetOptionByValue(((PrintSelectionFixedValueCondition) condition).Value);
            break;
          }
          this.txtCondValue.Text = ((PrintSelectionFixedValueCondition) condition).Value;
          break;
        case PrintSelectionRangeCondition _:
          this.txtCondMin.Text = this.txtCondMax.Text = "";
          PrintSelectionRangeCondition selectionRangeCondition = (PrintSelectionRangeCondition) condition;
          this.txtCondMin.Text = selectionRangeCondition.Minimum.ToString();
          this.txtCondMax.Text = selectionRangeCondition.Maximum.ToString();
          break;
        case PrintSelectionValueListCondition _:
          if (this.printField.Options.RequireValueFromList)
          {
            this.loadPrintSelectionConditionOptions((PrintSelectionValueListCondition) condition);
            break;
          }
          this.loadPrintSelectionConditionValueList((PrintSelectionValueListCondition) condition);
          break;
      }
      this.gridViewForms.Items.Clear();
      this.gridViewForms.BeginUpdate();
      FormInfo[] selectedForms = this.printSelectionEvent.SelectedForms;
      if (selectedForms != null)
      {
        for (int index = 0; index < selectedForms.Length; ++index)
          this.addFormObjectToList(selectedForms[index]);
      }
      this.gridViewForms.EndUpdate();
    }

    private string fieldValueToText(string value)
    {
      return this.printField.Options.RequireValueFromList ? this.printField.Options.ValueToText(value) : value;
    }

    private string fieldTextToValue(string text)
    {
      return this.printField.Options.RequireValueFromList ? this.printField.Options.TextToValue(text) : text;
    }

    private void loadPrintSelectionConditionValueList(PrintSelectionValueListCondition listCond)
    {
      this.lvwCondValues.Items.Clear();
      foreach (string str in listCond.Values)
        this.lvwCondValues.Items.Add(this.fieldValueToText(str));
    }

    private void loadPrintSelectionConditionOptions(PrintSelectionValueListCondition listCond)
    {
      List<string> stringList = new List<string>((IEnumerable<string>) listCond.Values);
      for (int index = 0; index < this.lstCondOptions.Items.Count; ++index)
      {
        FieldOption fieldOption = (FieldOption) this.lstCondOptions.Items[index];
        this.lstCondOptions.SetItemChecked(index, stringList.Contains(fieldOption.Value));
      }
    }

    private void removeSelectedListItems(ListView listView)
    {
      while (listView.SelectedIndices.Count > 0)
        listView.Items.RemoveAt(listView.SelectedIndices[0]);
    }

    private void cboCondType_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.pnlCondValue.Visible = this.pnlCondRange.Visible = this.pnlCondList.Visible = this.pnlCondValueSelect.Visible = this.pnlCondOptions.Visible = false;
      if (this.cboCondType.SelectedIndex < 0)
        return;
      switch (this.getSelectedConditionType())
      {
        case PrintSelectionConditionType.FixedValue:
          if (this.printField.Options.RequireValueFromList)
          {
            this.pnlCondValueSelect.Visible = true;
            break;
          }
          this.pnlCondValue.Visible = true;
          break;
        case PrintSelectionConditionType.Range:
          this.pnlCondRange.Visible = true;
          break;
        case PrintSelectionConditionType.ValueList:
          if (this.printField.Options.RequireValueFromList)
          {
            this.pnlCondOptions.Visible = true;
            break;
          }
          this.pnlCondList.Visible = true;
          this.lvwCondValues_SelectedIndexChanged((object) null, (EventArgs) null);
          break;
      }
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (!this.validateData())
        return;
      PrintSelectionCondition condition = this.createCondition();
      FormInfo[] selectedForms = new FormInfo[this.gridViewForms.Items.Count];
      for (int nItemIndex = 0; nItemIndex < this.gridViewForms.Items.Count; ++nItemIndex)
        selectedForms[nItemIndex] = (FormInfo) this.gridViewForms.Items[nItemIndex].Tag;
      this.printSelectionEvent = new PrintSelectionEvent(condition, selectedForms);
      this.DialogResult = DialogResult.OK;
    }

    private bool validateData()
    {
      if (!this.selectPrintSelectionField(this.txtFieldID.Text.Trim(), true))
      {
        this.txtFieldID.Focus();
        return false;
      }
      if (this.printField == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "A valid Field ID must be specified as the Print Selection for this event.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.txtFieldID.Focus();
        return false;
      }
      switch (this.getSelectedConditionType())
      {
        case PrintSelectionConditionType.Range:
          if (!this.validateRange())
            return false;
          break;
        case PrintSelectionConditionType.ValueList:
          if (this.printField.Options.RequireValueFromList)
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
      if (this.gridViewForms.Items.Count != 0)
        return true;
      int num1 = (int) Utils.Dialog((IWin32Window) this, "Must specify a valid form for Print Auto Selection event.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      this.btnAddForm.Focus();
      return false;
    }

    private void removeEmptyListItems(ListView listView)
    {
      for (int index = listView.Items.Count - 1; index >= 0; --index)
      {
        if (listView.Items[index].Tag == null)
          listView.Items.RemoveAt(index);
      }
    }

    private PrintSelectionConditionType getSelectedConditionType()
    {
      return (PrintSelectionConditionType) new PrintSelectionEventEditor.PrintSelectionEventConditionNameProvider().GetValue(this.cboCondType.SelectedItem.ToString());
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

    private PrintSelectionCondition createCondition()
    {
      string fieldId = this.txtFieldID.Text.Trim();
      switch ((PrintSelectionConditionType) new PrintSelectionEventEditor.PrintSelectionEventConditionNameProvider().GetValue(this.cboCondType.SelectedItem.ToString()))
      {
        case PrintSelectionConditionType.FixedValue:
          return (PrintSelectionCondition) new PrintSelectionFixedValueCondition(fieldId, this.getConditionFixedValue());
        case PrintSelectionConditionType.Range:
          return (PrintSelectionCondition) new PrintSelectionRangeCondition(fieldId, this.txtCondMin.Text, this.txtCondMax.Text);
        case PrintSelectionConditionType.NonEmptyValue:
          return (PrintSelectionCondition) new PrintSelectionNonEmptyValueCondition(fieldId);
        default:
          return (PrintSelectionCondition) new PrintSelectionValueListCondition(fieldId, this.createConditionValueList());
      }
    }

    private Range<Decimal> createConditionRange()
    {
      return new Range<Decimal>(Utils.ParseDecimal((object) this.txtCondMin.Text, Decimal.MinValue), Utils.ParseDecimal((object) this.txtCondMax.Text, Decimal.MaxValue));
    }

    private string getConditionFixedValue()
    {
      return this.printField.Options.RequireValueFromList ? ((FieldOption) this.cboCondValue.SelectedItem).Value : this.txtCondValue.Text;
    }

    private string[] createConditionValueList()
    {
      List<string> stringList = new List<string>();
      if (this.printField.Options.RequireValueFromList)
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
        this.selectPrintSelectionField(ruleFindFieldDialog.SelectedRequiredFields[ruleFindFieldDialog.SelectedRequiredFields.Length - 1], true);
        this.txtFieldID.Focus();
      }
    }

    private bool selectPrintSelectionField(string fieldId, bool throwException)
    {
      if (this.printField != null && string.Compare(this.printField.FieldID, fieldId, true) == 0)
        return true;
      FieldDefinition fieldDefinition = (FieldDefinition) null;
      if (fieldId != "")
      {
        fieldDefinition = EncompassFields.GetField(fieldId, this.fieldSettings);
        if (fieldDefinition == null)
        {
          if (throwException)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The value '" + fieldId + "' is not a valid field ID.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          return false;
        }
        if (fieldDefinition is VirtualField)
        {
          if (throwException)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The field '" + fieldId + "' is a virtual field and cannot be used with a Print Selection.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          return false;
        }
      }
      this.printField = fieldDefinition;
      this.updateUIForField();
      return true;
    }

    private void updateUIForField()
    {
      if (this.printField == null)
      {
        this.txtFieldID.Text = "";
        this.txtDescription.Text = "";
        this.cboCondType.SelectedIndex = -1;
        this.cboCondType.Enabled = false;
      }
      else
      {
        this.txtFieldID.Text = this.printField.FieldID;
        this.txtDescription.Text = this.printField.Description;
        this.cboCondType.Enabled = true;
        if (this.printField.IsNumeric() || this.printField.IsDateValued())
        {
          this.populateConditionTypes(PrintSelectionEventEditor.dateNumConditions);
          this.txtCondValue.MaxLength = 10;
        }
        else
        {
          this.populateConditionTypes(PrintSelectionEventEditor.textConditions);
          this.txtCondValue.MaxLength = 200;
        }
        if (!this.printField.Options.RequireValueFromList)
          return;
        this.populateValueSelector();
        this.populateOptionsList();
      }
    }

    private void populateOptionsList()
    {
      this.lstCondOptions.Items.Clear();
      this.lstCondOptions.Items.Add((object) new FieldOption("<Empty>", ""));
      foreach (FieldOption option in this.printField.Options)
        this.lstCondOptions.Items.Add((object) option);
    }

    private void populateValueSelector()
    {
      this.cboCondValue.Items.Clear();
      this.cboCondValue.Items.Add((object) FieldOption.Empty);
      foreach (FieldOption option in this.printField.Options)
        this.cboCondValue.Items.Add((object) option);
      this.cboCondValue.SelectedIndex = 0;
    }

    private void populateConditionTypes(PrintSelectionConditionType[] types)
    {
      PrintSelectionEventEditor.PrintSelectionEventConditionNameProvider conditionNameProvider = new PrintSelectionEventEditor.PrintSelectionEventConditionNameProvider();
      this.cboCondType.Items.Clear();
      foreach (PrintSelectionConditionType type in types)
        this.cboCondType.Items.Add((object) conditionNameProvider.GetName((object) type));
      this.cboCondType.SelectedIndex = 0;
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
        if (this.printField.ValidateFormat(text) != "")
          return true;
      }
      catch
      {
        if (this.printField.Format == FieldFormat.DATE)
        {
          if (text.ToLower() == "[today]")
            return true;
        }
      }
      return false;
    }

    private void btnAddForm_Click(object sender, EventArgs e)
    {
      using (AddPrintFormContainer printFormContainer = new AddPrintFormContainer(this.session))
      {
        if (printFormContainer.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        Hashtable hashtable = this.collectExistingForms();
        for (int index = 0; index < printFormContainer.SelectedForms.Length; ++index)
        {
          if (!hashtable.ContainsKey((object) printFormContainer.SelectedForms[index].Name))
            this.addFormObjectToList(printFormContainer.SelectedForms[index]);
        }
      }
    }

    private void btnAddGroup_Click(object sender, EventArgs e)
    {
      using (AddFormGroupContainer formGroupContainer = new AddFormGroupContainer(this.session))
      {
        if (formGroupContainer.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        FormInfo[] selectedFormGroups = formGroupContainer.SelectedFormGroups;
        Hashtable hashtable = this.collectExistingForms();
        for (int index = 0; index < selectedFormGroups.Length; ++index)
        {
          if (!hashtable.ContainsKey((object) selectedFormGroups[index].Name))
            this.addFormObjectToList(selectedFormGroups[index]);
        }
      }
    }

    private Hashtable collectExistingForms()
    {
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      for (int nItemIndex = 0; nItemIndex < this.gridViewForms.Items.Count; ++nItemIndex)
      {
        FormInfo tag = (FormInfo) this.gridViewForms.Items[nItemIndex].Tag;
        if (!insensitiveHashtable.ContainsKey((object) tag.Name))
          insensitiveHashtable.Add((object) tag.Name, (object) tag);
      }
      return insensitiveHashtable;
    }

    private void addFormObjectToList(FormInfo formItem)
    {
      string name = formItem.Name;
      GVItem gvItem = formItem.Type != OutputFormType.FormGroup ? new GVItem((object) new ObjectWithImage(this.buildUIFormName(formItem), (Image) Resources.doc_single)) : new GVItem((object) new ObjectWithImage(this.buildUIFormName(formItem), (Image) Resources.document_group_public));
      gvItem.Tag = (object) formItem;
      this.gridViewForms.Items.Add(gvItem);
    }

    private string buildUIFormName(FormInfo formItem)
    {
      string strA = formItem.Name;
      int num = strA.LastIndexOf("\\");
      if (num > -1)
        strA = formItem.Name.Substring(num + 1);
      if (formItem.Type == OutputFormType.CustomLetters)
      {
        if (strA.ToLower().EndsWith(".doc") || strA.ToLower().EndsWith(".rtf"))
          strA = strA.Substring(0, strA.Length - 4);
        else if (strA.ToLower().EndsWith(".docx"))
          strA = strA.Substring(0, strA.Length - 5);
      }
      else if (formItem.Type == OutputFormType.PdfForms)
      {
        if (string.Compare(strA, "Loans Where Credit Score is Not Available", true) == 0)
          strA += " Model H5";
        else if (string.Compare(strA, "Risk-Based Pricing Notice", true) == 0)
          strA += " Model H1";
      }
      return strA;
    }

    private void btnRemove_Click(object sender, EventArgs e)
    {
      if (this.gridViewForms.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a form or a form group.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        int num2 = this.gridViewForms.Items.Count - 1;
        int index = this.gridViewForms.SelectedItems[0].Index;
        for (int nItemIndex = num2; nItemIndex >= 0; --nItemIndex)
        {
          if (this.gridViewForms.Items[nItemIndex].Selected)
            this.gridViewForms.Items.RemoveAt(nItemIndex);
        }
        if (this.gridViewForms.Items.Count == 0)
          return;
        if (index + 1 >= this.gridViewForms.Items.Count)
          this.gridViewForms.Items[this.gridViewForms.Items.Count - 1].Selected = true;
        else
          this.gridViewForms.Items[index].Selected = true;
      }
    }

    private void lvwCondValues_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnRemoveValue.Enabled = this.lvwCondValues.SelectedItems.Count > 0;
      this.btnEditValue.Enabled = this.lvwCondValues.SelectedItems.Count == 1;
    }

    private void txtFieldID_Leave(object sender, EventArgs e)
    {
      if (this.selectPrintSelectionField(this.txtFieldID.Text, false))
        return;
      string str = this.txtFieldID.Text.Trim();
      this.printField = (FieldDefinition) null;
      this.updateUIForField();
      this.txtFieldID.Text = str;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn = new GVColumn();
      this.label1 = new Label();
      this.groupBox1 = new GroupBox();
      this.btnRemove = new Button();
      this.btnAddGroup = new Button();
      this.btnAddForm = new Button();
      this.gridViewForms = new GridView();
      this.pnlCondList = new Panel();
      this.pnlCondOptions = new Panel();
      this.lstCondOptions = new CheckedListBox();
      this.label13 = new Label();
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
      this.pnlCondValueSelect = new Panel();
      this.cboCondValue = new ComboBox();
      this.label11 = new Label();
      this.panel1 = new Panel();
      this.groupBox1.SuspendLayout();
      this.pnlCondList.SuspendLayout();
      this.pnlCondOptions.SuspendLayout();
      this.pnlCondRange.SuspendLayout();
      this.pnlCondValue.SuspendLayout();
      this.panel2.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.pnlCondValueSelect.SuspendLayout();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(9, 7);
      this.label1.Name = "label1";
      this.label1.Size = new Size(43, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Field ID";
      this.groupBox1.Controls.Add((Control) this.btnRemove);
      this.groupBox1.Controls.Add((Control) this.btnAddGroup);
      this.groupBox1.Controls.Add((Control) this.btnAddForm);
      this.groupBox1.Controls.Add((Control) this.gridViewForms);
      this.groupBox1.Dock = DockStyle.Top;
      this.groupBox1.Location = new Point(10, 254);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(504, 173);
      this.groupBox1.TabIndex = 2;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Action";
      this.btnRemove.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnRemove.Location = new Point(392, 77);
      this.btnRemove.Name = "btnRemove";
      this.btnRemove.Size = new Size(106, 23);
      this.btnRemove.TabIndex = 6;
      this.btnRemove.Text = "&Remove Forms";
      this.btnRemove.UseVisualStyleBackColor = true;
      this.btnRemove.Click += new EventHandler(this.btnRemove_Click);
      this.btnAddGroup.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnAddGroup.Location = new Point(392, 48);
      this.btnAddGroup.Name = "btnAddGroup";
      this.btnAddGroup.Size = new Size(106, 23);
      this.btnAddGroup.TabIndex = 5;
      this.btnAddGroup.Text = "&Add Form Groups";
      this.btnAddGroup.UseVisualStyleBackColor = true;
      this.btnAddGroup.Click += new EventHandler(this.btnAddGroup_Click);
      this.btnAddForm.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnAddForm.Location = new Point(392, 19);
      this.btnAddForm.Name = "btnAddForm";
      this.btnAddForm.Size = new Size(106, 23);
      this.btnAddForm.TabIndex = 4;
      this.btnAddForm.Text = "&Add Forms";
      this.btnAddForm.UseVisualStyleBackColor = true;
      this.btnAddForm.Click += new EventHandler(this.btnAddForm_Click);
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "Column1";
      gvColumn.Text = "Auto Selected Forms or Form Groups";
      gvColumn.Width = 375;
      this.gridViewForms.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.gridViewForms.Location = new Point(6, 19);
      this.gridViewForms.Name = "gridViewForms";
      this.gridViewForms.Size = new Size(380, 148);
      this.gridViewForms.TabIndex = 0;
      this.pnlCondList.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlCondList.Controls.Add((Control) this.btnEditValue);
      this.pnlCondList.Controls.Add((Control) this.lvwCondValues);
      this.pnlCondList.Controls.Add((Control) this.btnAddValue);
      this.pnlCondList.Controls.Add((Control) this.btnRemoveValue);
      this.pnlCondList.Controls.Add((Control) this.label7);
      this.pnlCondList.Location = new Point(3, 93);
      this.pnlCondList.Name = "pnlCondList";
      this.pnlCondList.Size = new Size(497, 123);
      this.pnlCondList.TabIndex = 2;
      this.pnlCondOptions.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlCondOptions.Controls.Add((Control) this.lstCondOptions);
      this.pnlCondOptions.Controls.Add((Control) this.label13);
      this.pnlCondOptions.Location = new Point(3, 93);
      this.pnlCondOptions.Name = "pnlCondOptions";
      this.pnlCondOptions.Size = new Size(497, 131);
      this.pnlCondOptions.TabIndex = 6;
      this.lstCondOptions.CheckOnClick = true;
      this.lstCondOptions.FormattingEnabled = true;
      this.lstCondOptions.Location = new Point(133, 2);
      this.lstCondOptions.Name = "lstCondOptions";
      this.lstCondOptions.Size = new Size(355, 109);
      this.lstCondOptions.TabIndex = 3;
      this.label13.AutoSize = true;
      this.label13.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label13.Location = new Point(10, 2);
      this.label13.Name = "label13";
      this.label13.Size = new Size(39, 13);
      this.label13.TabIndex = 2;
      this.label13.Text = "Values";
      this.btnEditValue.Location = new Point(426, 26);
      this.btnEditValue.Name = "btnEditValue";
      this.btnEditValue.Size = new Size(62, 22);
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
      this.lvwCondValues.Size = new Size(287, 118);
      this.lvwCondValues.TabIndex = 1;
      this.lvwCondValues.UseCompatibleStateImageBehavior = false;
      this.lvwCondValues.View = View.Details;
      this.lvwCondValues.AfterLabelEdit += new LabelEditEventHandler(this.lvwCondValues_AfterLabelEdit);
      this.lvwCondValues.SelectedIndexChanged += new EventHandler(this.lvwCondValues_SelectedIndexChanged);
      this.columnHeader1.Text = "Values";
      this.columnHeader1.Width = 258;
      this.btnAddValue.Location = new Point(426, 2);
      this.btnAddValue.Name = "btnAddValue";
      this.btnAddValue.Size = new Size(62, 22);
      this.btnAddValue.TabIndex = 2;
      this.btnAddValue.Text = "&Add";
      this.btnAddValue.UseVisualStyleBackColor = true;
      this.btnAddValue.Click += new EventHandler(this.btnAddValue_Click);
      this.btnRemoveValue.Location = new Point(426, 50);
      this.btnRemoveValue.Name = "btnRemoveValue";
      this.btnRemoveValue.Size = new Size(62, 22);
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
      this.pnlCondRange.Location = new Point(3, 93);
      this.pnlCondRange.Name = "pnlCondRange";
      this.pnlCondRange.Size = new Size(497, 123);
      this.pnlCondRange.TabIndex = 3;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(226, 6);
      this.label5.Name = "label5";
      this.label5.Size = new Size(10, 13);
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
      this.pnlCondValue.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlCondValue.Controls.Add((Control) this.txtCondValue);
      this.pnlCondValue.Controls.Add((Control) this.label3);
      this.pnlCondValue.Location = new Point(3, 93);
      this.pnlCondValue.Name = "pnlCondValue";
      this.pnlCondValue.Size = new Size(497, 123);
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
      this.label3.Size = new Size(37, 13);
      this.label3.TabIndex = 2;
      this.label3.Text = "Value:";
      this.panel2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.panel2.Controls.Add((Control) this.txtDescription);
      this.panel2.Controls.Add((Control) this.label10);
      this.panel2.Controls.Add((Control) this.btnFindField);
      this.panel2.Controls.Add((Control) this.txtFieldID);
      this.panel2.Controls.Add((Control) this.label1);
      this.panel2.Controls.Add((Control) this.cboCondType);
      this.panel2.Controls.Add((Control) this.label2);
      this.panel2.Location = new Point(3, 16);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(497, 78);
      this.panel2.TabIndex = 1;
      this.txtDescription.Location = new Point(133, 27);
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.ReadOnly = true;
      this.txtDescription.Size = new Size(355, 20);
      this.txtDescription.TabIndex = 5;
      this.label10.AutoSize = true;
      this.label10.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label10.Location = new Point(9, 31);
      this.label10.Name = "label10";
      this.label10.Size = new Size(60, 13);
      this.label10.TabIndex = 4;
      this.label10.Text = "Description";
      this.btnFindField.Location = new Point(426, 3);
      this.btnFindField.Name = "btnFindField";
      this.btnFindField.Size = new Size(62, 22);
      this.btnFindField.TabIndex = 2;
      this.btnFindField.Text = "&Find";
      this.btnFindField.UseVisualStyleBackColor = true;
      this.btnFindField.Click += new EventHandler(this.btnFindField_Click);
      this.txtFieldID.Location = new Point(133, 3);
      this.txtFieldID.Name = "txtFieldID";
      this.txtFieldID.Size = new Size(287, 20);
      this.txtFieldID.TabIndex = 1;
      this.txtFieldID.Leave += new EventHandler(this.txtFieldID_Leave);
      this.cboCondType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboCondType.Enabled = false;
      this.cboCondType.FormattingEnabled = true;
      this.cboCondType.Items.AddRange(new object[4]
      {
        (object) "Value Is Non-Empty Value",
        (object) "Value Is",
        (object) "Value Is Between",
        (object) "Value Is an Item From a List Below"
      });
      this.cboCondType.Location = new Point(133, 51);
      this.cboCondType.Name = "cboCondType";
      this.cboCondType.Size = new Size(355, 21);
      this.cboCondType.TabIndex = 3;
      this.cboCondType.SelectedIndexChanged += new EventHandler(this.cboCondType_SelectedIndexChanged);
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(9, 55);
      this.label2.Name = "label2";
      this.label2.Size = new Size(13, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "If";
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(358, 437);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 3;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(437, 437);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.groupBox2.Controls.Add((Control) this.pnlCondOptions);
      this.groupBox2.Controls.Add((Control) this.panel2);
      this.groupBox2.Controls.Add((Control) this.pnlCondList);
      this.groupBox2.Controls.Add((Control) this.pnlCondRange);
      this.groupBox2.Controls.Add((Control) this.pnlCondValueSelect);
      this.groupBox2.Controls.Add((Control) this.pnlCondValue);
      this.groupBox2.Dock = DockStyle.Top;
      this.groupBox2.Location = new Point(10, 10);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(504, 236);
      this.groupBox2.TabIndex = 1;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Condition";
      this.pnlCondValueSelect.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlCondValueSelect.Controls.Add((Control) this.cboCondValue);
      this.pnlCondValueSelect.Controls.Add((Control) this.label11);
      this.pnlCondValueSelect.Location = new Point(3, 93);
      this.pnlCondValueSelect.Name = "pnlCondValueSelect";
      this.pnlCondValueSelect.Size = new Size(497, 123);
      this.pnlCondValueSelect.TabIndex = 5;
      this.cboCondValue.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboCondValue.FormattingEnabled = true;
      this.cboCondValue.Items.AddRange(new object[4]
      {
        (object) "Value Is Non-Empty Value",
        (object) "Value Is",
        (object) "Value Is Between",
        (object) "Value Is an Item From a List Below"
      });
      this.cboCondValue.Location = new Point(133, 2);
      this.cboCondValue.Name = "cboCondValue";
      this.cboCondValue.Size = new Size(287, 21);
      this.cboCondValue.TabIndex = 4;
      this.label11.AutoSize = true;
      this.label11.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label11.Location = new Point(10, 6);
      this.label11.Name = "label11";
      this.label11.Size = new Size(37, 13);
      this.label11.TabIndex = 2;
      this.label11.Text = "Value:";
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(10, 246);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(504, 8);
      this.panel1.TabIndex = 8;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(524, 472);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.groupBox2);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (PrintSelectionEventEditor);
      this.Padding = new Padding(10);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add/Edit Field Event";
      this.groupBox1.ResumeLayout(false);
      this.pnlCondList.ResumeLayout(false);
      this.pnlCondList.PerformLayout();
      this.pnlCondOptions.ResumeLayout(false);
      this.pnlCondOptions.PerformLayout();
      this.pnlCondRange.ResumeLayout(false);
      this.pnlCondRange.PerformLayout();
      this.pnlCondValue.ResumeLayout(false);
      this.pnlCondValue.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.pnlCondValueSelect.ResumeLayout(false);
      this.pnlCondValueSelect.PerformLayout();
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

    private class PrintSelectionEventConditionNameProvider : CustomEnumNameProvider
    {
      private static Hashtable nameMap = CollectionsUtil.CreateCaseInsensitiveHashtable();

      static PrintSelectionEventConditionNameProvider()
      {
        PrintSelectionEventEditor.PrintSelectionEventConditionNameProvider.nameMap.Add((object) PrintSelectionConditionType.NonEmptyValue, (object) "Value Is Non-Empty Value");
        PrintSelectionEventEditor.PrintSelectionEventConditionNameProvider.nameMap.Add((object) PrintSelectionConditionType.FixedValue, (object) "Value Is");
        PrintSelectionEventEditor.PrintSelectionEventConditionNameProvider.nameMap.Add((object) PrintSelectionConditionType.Range, (object) "Value Is Between");
        PrintSelectionEventEditor.PrintSelectionEventConditionNameProvider.nameMap.Add((object) PrintSelectionConditionType.ValueList, (object) "Value Is an Item From a List Below");
      }

      public PrintSelectionEventConditionNameProvider()
        : base(typeof (PrintSelectionEventEditor.PrintSelectionEventConditionNameProvider), PrintSelectionEventEditor.PrintSelectionEventConditionNameProvider.nameMap)
      {
      }
    }
  }
}
