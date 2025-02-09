// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.FieldRulesDlg
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.ContactUI.Import;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.Properties;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public class FieldRulesDlg : Form, IHelp
  {
    private Sessions.Session session;
    private string userid;
    private string advancedCodeXml;
    private ArrayList curSelection;
    private FieldSettings fieldSettings;
    private StandardFields standardFields;
    private bool newRule = true;
    public DDMFieldRule _DDMFieldRule;
    private List<string> existingFields;
    public Dictionary<string, string> importIDValuePair = new Dictionary<string, string>();
    public ImportSource dataImportSource;
    private bool RemoveActionPerformed;
    private IContainer components;
    private GroupContainer groupContainer2;
    private Label label11;
    private GroupContainer groupContainer1;
    private Label label6;
    private Label label5;
    private GridView gridViewFields;
    private StandardIconButton btnSelect;
    private TextBox conditionTextBox;
    private CheckBox conditionchkBx;
    private CheckBox afterLESentChkBx;
    private Button cancelBtn;
    private Button okBtn;
    private PictureBox pictureBox1;
    private System.Windows.Forms.LinkLabel lnkLblLearnMore;
    private Panel panelRuleName;
    private Panel panelBottom;
    private Panel panelMiddleLeft;
    private Panel panelMiddleRight;
    private Label label1;
    private Label label4;
    private TextBox ruleNameTxtBx;
    private Label label3;
    private ToolTip ttFieldRulesDlg;
    private StandardIconButton stdButtonDelete;
    private StandardIconButton stdButtonNew;
    private StandardIconButton stdButtonFind;
    private StandardIconButton stdIconBtnImport;
    private VerticalSeparator verticalSeparator1;

    public FieldRulesDlg(Sessions.Session session, string userid, DDMFieldRule currentFieldRule = null)
    {
      this.session = session;
      this.userid = userid;
      this.InitializeComponent();
      this.fieldSettings = this.session.LoanManager.GetFieldSettings();
      this.standardFields = this.session.LoanManager.GetStandardFields();
      if (currentFieldRule != null)
      {
        this.newRule = false;
        this._DDMFieldRule = currentFieldRule;
        this.populateFieldRule();
        this.Text = "Rule Settings";
        this.okBtn.Enabled = true;
      }
      this.gridViewFields_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    private void populateFieldRule()
    {
      this.ruleNameTxtBx.Text = this._DDMFieldRule.RuleName;
      this.afterLESentChkBx.Checked = this._DDMFieldRule.InitLESent;
      this.conditionchkBx.Checked = this._DDMFieldRule.Condition;
      this.conditionTextBox.Text = this._DDMFieldRule.ConditionState;
      this.advancedCodeXml = this._DDMFieldRule.AdvCodeConditionXML;
      string[] strArray = this._DDMFieldRule.Fields.Split(',');
      this.existingFields = ((IEnumerable<string>) strArray).ToList<string>();
      this.addFields(strArray);
    }

    private void btnSelect_Click(object sender, EventArgs e)
    {
      using (AdvConditionEditor advConditionEditor = new AdvConditionEditor(this.session, this.advancedCodeXml, true))
      {
        if (advConditionEditor.GetConditionScript() != this.conditionTextBox.Text)
          advConditionEditor.ClearFilters();
        if (advConditionEditor.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.conditionTextBox.Text = advConditionEditor.GetConditionScript();
        this.advancedCodeXml = advConditionEditor.GetConditionXml();
      }
    }

    private void conditionchkBx_CheckedChanged(object sender, EventArgs e)
    {
      if (this.conditionchkBx.Checked)
      {
        this.btnSelect.Enabled = true;
        this.conditionTextBox.Enabled = true;
      }
      else
      {
        this.btnSelect.Enabled = false;
        this.conditionTextBox.Enabled = false;
      }
    }

    private void addFieldDlg_OnAddMoreButtonClick(object sender, EventArgs e)
    {
      AddFields addFields = (AddFields) sender;
      if (addFields == null)
        return;
      this.addFields(addFields.SelectedFieldIDs);
    }

    private void addFields(string[] ids)
    {
      if (ids.Length == 0)
        return;
      this.gridViewFields.BeginUpdate();
      this.curSelection = new ArrayList();
      for (int index = 0; index < ids.Length; ++index)
      {
        bool flag = false;
        for (int nItemIndex = 0; nItemIndex < this.gridViewFields.Items.Count; ++nItemIndex)
        {
          if (string.Compare(this.gridViewFields.Items[nItemIndex].Text, ids[index], true) == 0)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The field list already contains field '" + ids[index] + "'. This field will be ignored.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          GVItem gvItem = new GVItem(ids[index]);
          gvItem.SubItems.Add((object) this.getFieldDescription(ids[index]));
          gvItem.Selected = true;
          this.gridViewFields.Items.Add(gvItem);
          this.curSelection.Add((object) gvItem.Index);
        }
      }
      this.gridViewFields.EndUpdate();
    }

    private string getFieldDescription(string fieldID)
    {
      return EncompassFields.GetDescription(fieldID, this.fieldSettings);
    }

    private bool isChanged()
    {
      return this.RemoveActionPerformed && (this.checkFieldsChanged() || this._DDMFieldRule != null && (this._DDMFieldRule.RuleName != this.ruleNameTxtBx.Text || this._DDMFieldRule.InitLESent != this.afterLESentChkBx.Checked || this._DDMFieldRule.Condition != this.conditionchkBx.Checked || this._DDMFieldRule.ConditionState != this.conditionTextBox.Text));
    }

    private bool checkFieldsChanged()
    {
      if (this._DDMFieldRule == null)
        return false;
      string selectedFields = this.getSelectedFields();
      if (selectedFields.Length != this._DDMFieldRule.Fields.Length)
        return true;
      List<string> curFields = ((IEnumerable<string>) selectedFields.Split(',')).ToList<string>();
      List<string> oldFields = ((IEnumerable<string>) this._DDMFieldRule.Fields.Split(',')).ToList<string>();
      return curFields.Count != oldFields.Count || curFields.Where<string>((Func<string, bool>) (x => !oldFields.Contains(x))).Count<string>() > 0 || oldFields.Where<string>((Func<string, bool>) (x => !curFields.Contains(x))).Count<string>() > 0;
    }

    private string CommaSeparatedToSQLStringFormat(string fields)
    {
      string sqlStringFormat = string.Empty;
      if (string.IsNullOrEmpty(fields))
        return string.Empty;
      string str1 = fields;
      char[] chArray = new char[1]{ ',' };
      foreach (string str2 in ((IEnumerable<string>) str1.Split(chArray)).Select<string, string>((Func<string, string>) (sValue => sValue.Trim())).ToArray<string>())
        sqlStringFormat = sqlStringFormat + "'" + str2 + "',";
      if (!string.IsNullOrEmpty(sqlStringFormat))
        sqlStringFormat = sqlStringFormat.Substring(0, sqlStringFormat.Length - 1);
      return sqlStringFormat;
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      string selectedFields = this.getSelectedFields();
      if (selectedFields.Length == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select Fields for Field Rule.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.ruleNameTxtBx.Text.Length == 0)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "Please enter Field Rule Name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.ruleNameTxtBx.Text.Length > 100)
      {
        int num3 = (int) Utils.Dialog((IWin32Window) this, "Field Rule Name can not be over 100 characters.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        DDMFieldRulesBpmManager bpmManager = (DDMFieldRulesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.DDMFieldRules);
        if (this.conditionchkBx.Checked && this.conditionTextBox.Text.Length == 0)
        {
          int num4 = (int) Utils.Dialog((IWin32Window) this, "You must provide code to determine the conditions under which this rule applies.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else if (this.ruleNameTxtBx.Text.Trim().Length == 0)
        {
          int num5 = (int) Utils.Dialog((IWin32Window) this, "Rule name can not be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          if (this.newRule)
          {
            if (bpmManager.DDMFieldRuleExist(this.ruleNameTxtBx.Text, true))
            {
              int num6 = (int) Utils.Dialog((IWin32Window) this, "The Field rule name that you entered already exists.  Please try a different rule name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return;
            }
            if (bpmManager.DDMFieldsExistInFieldRules(this.CommaSeparatedToSQLStringFormat(selectedFields), true))
            {
              int num7 = (int) Utils.Dialog((IWin32Window) this, "A Field rule already exists for one or more fields you are attempting to add. The new field rule created with the duplicated fields will be in 'Inactive' status.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            this.Hide();
            string text1 = this.ruleNameTxtBx.Text;
            string fields = selectedFields;
            int num8 = this.afterLESentChkBx.Checked ? 1 : 0;
            string text2 = this.conditionTextBox.Text;
            int num9 = this.conditionchkBx.Checked ? 1 : 0;
            string userid = this.userid;
            string fullName = this.session.UserInfo.FullName;
            DateTime now = DateTime.Now;
            string createDt = now.ToString();
            now = DateTime.Now;
            string updateDt = now.ToString();
            string advancedCodeXml = this.advancedCodeXml;
            DDMFieldRule fieldRule = new DDMFieldRule(text1, fields, num8 != 0, ruleStatus.Inactive, text2, num9 != 0, userid, fullName, createDt, updateDt, advancedCodeXml);
            fieldRule.RuleID = bpmManager.CreateFieldRule(fieldRule);
            this._DDMFieldRule = fieldRule;
            this.Close();
          }
          else
          {
            this.getNewFields();
            if (this.ruleNameTxtBx.Text != this._DDMFieldRule.RuleName && bpmManager.DDMFieldRuleExist(this.ruleNameTxtBx.Text, true))
            {
              int num10 = (int) Utils.Dialog((IWin32Window) this, "The field rule name that you entered already exists.  Please try a different rule name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return;
            }
            if (this.isChanged() && Utils.Dialog((IWin32Window) this, "Please note that removing Fields would remove its Scenario Value Settings. " + Environment.NewLine + Environment.NewLine + "Do you want to continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
              return;
            this._DDMFieldRule.RuleName = this.ruleNameTxtBx.Text;
            this._DDMFieldRule.InitLESent = this.afterLESentChkBx.Checked;
            this._DDMFieldRule.Condition = this.conditionchkBx.Checked;
            this._DDMFieldRule.ConditionState = this.conditionTextBox.Text;
            this._DDMFieldRule.Fields = selectedFields;
            this._DDMFieldRule.UpdateDt = DateTime.Now.ToString();
            this._DDMFieldRule.AdvCodeConditionXML = this.advancedCodeXml;
            this.updateFieldRuleValues(this._DDMFieldRule, selectedFields);
          }
          this.Close();
          this.DialogResult = DialogResult.OK;
        }
      }
    }

    private void PopulateFieldRuleValue(DDMFieldRuleScenario newFieldRuleScenario, string fieldid)
    {
      FieldFormat fieldFormat = FieldFormat.NONE;
      FieldDefinition fieldDefinition = DDM_FieldAccess_Utils.GetFieldDefinition(fieldid, this.session.LoanManager);
      if (fieldDefinition != null)
        fieldFormat = fieldDefinition.Format;
      newFieldRuleScenario.FieldRuleValues.Add(new DDMFeeRuleValue()
      {
        Field_Name = fieldDefinition == null || fieldDefinition.Description == null ? "" : fieldDefinition.Description,
        Field_Type = fieldFormat,
        Field_Value = string.Empty,
        Field_Value_Type = fieldValueType.ValueNotSet,
        FieldID = fieldid,
        RuleScenarioID = newFieldRuleScenario.FieldRuleID
      });
    }

    private void updateFieldRuleValues(DDMFieldRule ddmFR, string newFields)
    {
      foreach (DDMFieldRuleScenario scenario in ddmFR.Scenarios)
      {
        List<string> list = ((IEnumerable<string>) newFields.Split(',')).ToList<string>();
        scenario.ContentChanged = true;
        foreach (DDMFeeRuleValue ddmFeeRuleValue in scenario.FieldRuleValues.ToList<DDMFeeRuleValue>())
        {
          if (list.Contains(ddmFeeRuleValue.FieldID))
            list.Remove(ddmFeeRuleValue.FieldID);
          else
            scenario.FieldRuleValues.Remove(ddmFeeRuleValue);
        }
        foreach (string fieldid in list)
          this.PopulateFieldRuleValue(scenario, fieldid);
      }
    }

    private string getSelectedFields()
    {
      string empty = string.Empty;
      GVItemCollection items = this.gridViewFields.Items;
      if (items.Count == 0)
        return empty;
      List<string> values = new List<string>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) items)
        values.Add(gvItem.Value.ToString());
      return string.Join(",", (IEnumerable<string>) values);
    }

    private string getNewFields()
    {
      string empty = string.Empty;
      GVItemCollection items = this.gridViewFields.Items;
      if (items.Count == 0)
        return empty;
      List<string> values = new List<string>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) items)
      {
        if (!this.existingFields.Contains(gvItem.Value.ToString()))
          values.Add(gvItem.Value.ToString());
      }
      return string.Join(",", (IEnumerable<string>) values);
    }

    private void cancelBtn_Click(object sender, EventArgs e) => this.Close();

    private void ruleNameTxtBx_TextChanged(object sender, EventArgs e)
    {
      if (this.ruleNameTxtBx.Text.Length > 0)
        this.okBtn.Enabled = true;
      else
        this.okBtn.Enabled = false;
    }

    private void gridViewFields_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.stdButtonDelete.Enabled = this.gridViewFields.SelectedItems.Count > 0;
    }

    private void LearnMore_Click(object sender, EventArgs e) => this.ShowHelp();

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    public void ShowHelp()
    {
      if (this.newRule)
        JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, "Setup\\Field Rules");
      else
        JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, "Setup\\Rule Settings");
    }

    private void btnNew_Click(object sender, EventArgs e)
    {
      using (AddFields addFields = new AddFields(this.session, (string) null, AddFieldOptions.AllowCustomFields | AddFieldOptions.AllowHiddenFields, new List<string>((IEnumerable<string>) new string[2]
      {
        "LOANFOLDER",
        "LOANLASTMODIFIED"
      })))
      {
        addFields.OnAddMoreButtonClick += new EventHandler(this.addFieldDlg_OnAddMoreButtonClick);
        if (addFields.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.addFields(addFields.SelectedFieldIDs);
      }
    }

    private void btnFind_Click(object sender, EventArgs e)
    {
      ArrayList arrayList = new ArrayList();
      for (int nItemIndex = 0; nItemIndex < this.gridViewFields.Items.Count; ++nItemIndex)
        arrayList.Add((object) this.gridViewFields.Items[nItemIndex].Text);
      using (BusinessRuleFindFieldDialog ruleFindFieldDialog = new BusinessRuleFindFieldDialog(this.session, (string[]) arrayList.ToArray(typeof (string)), false, string.Empty, false, true))
      {
        if (ruleFindFieldDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        Cursor.Current = Cursors.WaitCursor;
        this.gridViewFields.BeginUpdate();
        for (int index = 0; index < ruleFindFieldDialog.SelectedRequiredFields.Length; ++index)
        {
          if (!(ruleFindFieldDialog.SelectedRequiredFields[index] == "") && !arrayList.Contains((object) ruleFindFieldDialog.SelectedRequiredFields[index]))
            this.gridViewFields.Items.Add(new GVItem(ruleFindFieldDialog.SelectedRequiredFields[index])
            {
              SubItems = {
                (object) this.getFieldDescription(ruleFindFieldDialog.SelectedRequiredFields[index])
              },
              Selected = true
            });
        }
        this.gridViewFields.EndUpdate();
        Cursor.Current = Cursors.Default;
      }
    }

    private void btnRemove_Click(object sender, EventArgs e)
    {
      this.RemoveActionPerformed = true;
      if (this.gridViewFields.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You have to select a field first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.gridViewFields.BeginUpdate();
        int index = this.gridViewFields.SelectedItems[0].Index;
        while (this.gridViewFields.SelectedItems.Count > 0)
          this.gridViewFields.Items.Remove(this.gridViewFields.SelectedItems[0]);
        if (this.gridViewFields.Items.Count > 0)
          this.gridViewFields.Items[index >= this.gridViewFields.Items.Count ? this.gridViewFields.Items.Count - 1 : index].Selected = true;
        this.gridViewFields.EndUpdate();
      }
    }

    private void btnImport_Click(object sender, EventArgs e)
    {
      using (ImportFormatSelector importFormatSelector = new ImportFormatSelector())
      {
        if (importFormatSelector.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
          return;
        this.dataImportSource = importFormatSelector.ImportSource;
      }
      switch (this.dataImportSource)
      {
        case ImportSource.DataTemplate:
          this.ImportFromDataTemplate();
          break;
        case ImportSource.Csv:
          this.ImportFromCsv();
          break;
      }
    }

    private void ImportFromCsv()
    {
      if (this.gridViewFields.Items.Count > 0 && Utils.Dialog((IWin32Window) this, "Import process will replace existing scenarios with a single default scenario. You can add, find, remove fields once the import is complete. Do you want to continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
        return;
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Filter = "Comma Separated Value Files (*.csv)|*.csv";
      openFileDialog.Multiselect = false;
      switch (openFileDialog.ShowDialog((IWin32Window) this))
      {
        case DialogResult.OK:
          string fileName = openFileDialog.FileName;
          string[][] parserResult = (string[][]) null;
          try
          {
            using (TextReader dataReader = (TextReader) File.OpenText(fileName))
              parserResult = new CsvParserForDdm(dataReader, true).RemainingRows();
          }
          catch (Exception ex)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, string.Format("Unexpected error parsing the input file. \r\nTechnical Details {0}", (object) ex.StackTrace), MessageBoxButtons.OK, MessageBoxIcon.Hand);
            break;
          }
          switch (this.ValidateImportCsvFile(parserResult))
          {
            case ImportFileParserStatus.EmptyInputFile:
              int num1 = (int) Utils.Dialog((IWin32Window) this, "The input file is empty. Please provide a valid file.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return;
            case ImportFileParserStatus.UnequalColumnsDetected:
              int num2 = (int) Utils.Dialog((IWin32Window) this, "Input file contains one or more invalid rows. Please make sure the input file has two comma-separated values on each row and try again.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return;
            case ImportFileParserStatus.DuplicateFieldIDsDetected:
              int num3 = (int) Utils.Dialog((IWin32Window) this, "One or more duplicate Field IDs identified in the import file. Please verify.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return;
            case ImportFileParserStatus.InvalidField:
              int num4 = (int) Utils.Dialog((IWin32Window) this, "Input file contains invalid field IDs. Please verify.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return;
            default:
              Dictionary<string, List<string>> rawFieldRuleData = new Dictionary<string, List<string>>();
              foreach (string[] source in parserResult)
              {
                string upperInvariant = source[0].ToUpperInvariant();
                List<string> list = ((IEnumerable<string>) source).Skip<string>(1).ToList<string>();
                rawFieldRuleData.Add(upperInvariant, list);
              }
              FieldRuleImportRawData importData = new FieldRuleImportRawData(rawFieldRuleData, this.standardFields, this.fieldSettings);
              ImportProcessResult importResult = new ImportProcessor((IImportErrorProvider) new FieldRuleImportErrorProvider()).Process((IRawImportData) importData);
              if (importResult.Result == ImportResult.Error)
              {
                using (ImportErrorForm importErrorForm = new ImportErrorForm("Field Rule Import Errors", "The field rule cannot be imported because the row(s) have invalid data. Please refer to the Error column to correct data entries.", new ImportErrorDataProvider((IRawImportData) importData, importResult).GetErrorDataForReport(), this.standardFields, this.fieldSettings, ImportType.FieldRule))
                {
                  int num5 = (int) importErrorForm.ShowDialog();
                }
                this.dataImportSource = ImportSource.None;
                return;
              }
              this.importIDValuePair = new Dictionary<string, string>();
              foreach (KeyValuePair<string, List<string>> keyValuePair in rawFieldRuleData)
                this.importIDValuePair.Add(keyValuePair.Key, keyValuePair.Value[0]);
              string[] array = this.importIDValuePair.Keys.ToArray<string>();
              if (((IEnumerable<string>) array).Count<string>() <= 0)
                return;
              this.gridViewFields.Items.Clear();
              this.addFields(array);
              return;
          }
        case DialogResult.Cancel:
          this.dataImportSource = ImportSource.None;
          break;
      }
    }

    private ImportFileParserStatus ValidateImportCsvFile(string[][] parserResult)
    {
      if (parserResult == null || parserResult.Length == 0)
        return ImportFileParserStatus.EmptyInputFile;
      SortedSet sortedSet = new SortedSet();
      foreach (string[] strArray in parserResult)
      {
        if (strArray.Length != 2)
          return ImportFileParserStatus.UnequalColumnsDetected;
        if (sortedSet.Contains((object) strArray[0].ToUpperInvariant()))
          return ImportFileParserStatus.DuplicateFieldIDsDetected;
        sortedSet.Add((object) strArray[0].ToUpperInvariant());
        if (string.IsNullOrWhiteSpace(this.getFieldDescription(strArray[0].ToUpperInvariant())))
          return ImportFileParserStatus.InvalidField;
      }
      return ImportFileParserStatus.OK;
    }

    private void ImportFromDataTemplate()
    {
      if (this.gridViewFields.Items.Count > 0 && Utils.Dialog((IWin32Window) this, "Import process will replace existing scenarios with a single default scenario. You can add, find, remove fields once the import is complete. Do you want to continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
        return;
      using (TemplateSelectionDialog templateSelectionDialog = new TemplateSelectionDialog(this.session, TemplateSettingsType.MiscData, (FileSystemEntry) null, false))
      {
        templateSelectionDialog.Text = "Import Data Template";
        switch (templateSelectionDialog.ShowDialog((IWin32Window) this))
        {
          case DialogResult.OK:
            Cursor.Current = Cursors.WaitCursor;
            FileSystemEntry selectedItem = templateSelectionDialog.SelectedItem;
            if (!this.session.ConfigurationManager.TemplateSettingsObjectExists(TemplateSettingsType.MiscData, selectedItem))
            {
              int num = (int) Utils.Dialog((IWin32Window) this, "The data template '" + selectedItem.Name + "' has been deleted.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              break;
            }
            try
            {
              XmlStringTable fieldDataWithValues = ((DataTemplate) this.session.ConfigurationManager.GetTemplateSettings(TemplateSettingsType.MiscData, selectedItem)).FieldDataWithValues;
              this.importIDValuePair.Clear();
              foreach (KeyValuePair<string, object> keyValuePair in (Dictionary<string, object>) fieldDataWithValues)
                this.importIDValuePair.Add(keyValuePair.Key, keyValuePair.Value.ToString());
              string[] array = this.importIDValuePair.Keys.Where<string>((Func<string, bool>) (x => x != "3969")).ToArray<string>();
              if (((IEnumerable<string>) array).Count<string>() <= 0)
                break;
              this.gridViewFields.Items.Clear();
              this.addFields(array);
              break;
            }
            catch (Exception ex)
            {
              int num = (int) Utils.Dialog((IWin32Window) this, "Can't open " + selectedItem.Name + " data template file. Message: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              break;
            }
          case DialogResult.Cancel:
            this.dataImportSource = ImportSource.None;
            break;
        }
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
      this.components = (IContainer) new System.ComponentModel.Container();
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      this.groupContainer2 = new GroupContainer();
      this.stdIconBtnImport = new StandardIconButton();
      this.stdButtonFind = new StandardIconButton();
      this.stdButtonNew = new StandardIconButton();
      this.stdButtonDelete = new StandardIconButton();
      this.label1 = new Label();
      this.gridViewFields = new GridView();
      this.label5 = new Label();
      this.label11 = new Label();
      this.groupContainer1 = new GroupContainer();
      this.btnSelect = new StandardIconButton();
      this.conditionTextBox = new TextBox();
      this.conditionchkBx = new CheckBox();
      this.afterLESentChkBx = new CheckBox();
      this.label6 = new Label();
      this.cancelBtn = new Button();
      this.okBtn = new Button();
      this.pictureBox1 = new PictureBox();
      this.lnkLblLearnMore = new System.Windows.Forms.LinkLabel();
      this.panelRuleName = new Panel();
      this.label4 = new Label();
      this.ruleNameTxtBx = new TextBox();
      this.label3 = new Label();
      this.panelBottom = new Panel();
      this.panelMiddleLeft = new Panel();
      this.panelMiddleRight = new Panel();
      this.ttFieldRulesDlg = new ToolTip(this.components);
      this.verticalSeparator1 = new VerticalSeparator();
      this.groupContainer2.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnImport).BeginInit();
      ((ISupportInitialize) this.stdButtonFind).BeginInit();
      ((ISupportInitialize) this.stdButtonNew).BeginInit();
      ((ISupportInitialize) this.stdButtonDelete).BeginInit();
      this.groupContainer1.SuspendLayout();
      ((ISupportInitialize) this.btnSelect).BeginInit();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.panelRuleName.SuspendLayout();
      this.panelBottom.SuspendLayout();
      this.panelMiddleLeft.SuspendLayout();
      this.panelMiddleRight.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer2.AutoSize = true;
      this.groupContainer2.BackColor = Color.Transparent;
      this.groupContainer2.Borders = AnchorStyles.Top | AnchorStyles.Bottom;
      this.groupContainer2.Controls.Add((Control) this.verticalSeparator1);
      this.groupContainer2.Controls.Add((Control) this.stdIconBtnImport);
      this.groupContainer2.Controls.Add((Control) this.stdButtonFind);
      this.groupContainer2.Controls.Add((Control) this.stdButtonNew);
      this.groupContainer2.Controls.Add((Control) this.stdButtonDelete);
      this.groupContainer2.Controls.Add((Control) this.label1);
      this.groupContainer2.Controls.Add((Control) this.gridViewFields);
      this.groupContainer2.Controls.Add((Control) this.label5);
      this.groupContainer2.Controls.Add((Control) this.label11);
      this.groupContainer2.Dock = DockStyle.Fill;
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(0, 0);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(435, 318);
      this.groupContainer2.TabIndex = 0;
      this.groupContainer2.Text = " Fields Managed";
      this.stdIconBtnImport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnImport.BackColor = Color.Transparent;
      this.stdIconBtnImport.Location = new Point(416, 3);
      this.stdIconBtnImport.Margin = new Padding(2, 2, 2, 2);
      this.stdIconBtnImport.MouseDownImage = (Image) null;
      this.stdIconBtnImport.Name = "stdIconBtnImport";
      this.stdIconBtnImport.Size = new Size(20, 16);
      this.stdIconBtnImport.StandardButtonType = StandardIconButton.ButtonType.ImportLoanButton;
      this.stdIconBtnImport.TabIndex = 30;
      this.stdIconBtnImport.TabStop = false;
      this.ttFieldRulesDlg.SetToolTip((Control) this.stdIconBtnImport, "Import from Data template");
      this.stdIconBtnImport.Click += new EventHandler(this.btnImport_Click);
      this.stdButtonFind.BackColor = Color.Transparent;
      this.stdButtonFind.Location = new Point(364, 3);
      this.stdButtonFind.MouseDownImage = (Image) null;
      this.stdButtonFind.Name = "stdButtonFind";
      this.stdButtonFind.Size = new Size(16, 16);
      this.stdButtonFind.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.stdButtonFind.TabIndex = 29;
      this.stdButtonFind.TabStop = false;
      this.ttFieldRulesDlg.SetToolTip((Control) this.stdButtonFind, "Find a field");
      this.stdButtonFind.Click += new EventHandler(this.btnFind_Click);
      this.stdButtonNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdButtonNew.BackColor = Color.Transparent;
      this.stdButtonNew.Location = new Point(344, 3);
      this.stdButtonNew.MouseDownImage = (Image) null;
      this.stdButtonNew.Name = "stdButtonNew";
      this.stdButtonNew.Size = new Size(16, 16);
      this.stdButtonNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdButtonNew.TabIndex = 19;
      this.stdButtonNew.TabStop = false;
      this.ttFieldRulesDlg.SetToolTip((Control) this.stdButtonNew, "Add a field");
      this.stdButtonNew.Click += new EventHandler(this.btnNew_Click);
      this.stdButtonDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdButtonDelete.BackColor = Color.Transparent;
      this.stdButtonDelete.Location = new Point(385, 3);
      this.stdButtonDelete.MouseDownImage = (Image) null;
      this.stdButtonDelete.Name = "stdButtonDelete";
      this.stdButtonDelete.Size = new Size(17, 16);
      this.stdButtonDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdButtonDelete.TabIndex = 18;
      this.stdButtonDelete.TabStop = false;
      this.ttFieldRulesDlg.SetToolTip((Control) this.stdButtonDelete, "Remove a field");
      this.stdButtonDelete.Click += new EventHandler(this.btnRemove_Click);
      this.label1.AutoSize = true;
      this.label1.ForeColor = Color.Red;
      this.label1.Location = new Point(145, 35);
      this.label1.Name = "label1";
      this.label1.Size = new Size(11, 13);
      this.label1.TabIndex = 9;
      this.label1.Text = "*";
      this.gridViewFields.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "ID";
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Description";
      gvColumn2.Width = 200;
      this.gridViewFields.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gridViewFields.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridViewFields.Location = new Point(8, 59);
      this.gridViewFields.Margin = new Padding(2, 2, 2, 2);
      this.gridViewFields.Name = "gridViewFields";
      this.gridViewFields.Size = new Size(413, 253);
      this.gridViewFields.TabIndex = 0;
      this.gridViewFields.SelectedIndexChanged += new EventHandler(this.gridViewFields_SelectedIndexChanged);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(8, 35);
      this.label5.Margin = new Padding(2, 0, 2, 0);
      this.label5.Name = "label5";
      this.label5.Size = new Size(136, 13);
      this.label5.TabIndex = 4;
      this.label5.Text = "Manage Fields for Field rule";
      this.label11.AutoSize = true;
      this.label11.Location = new Point(-12, -12);
      this.label11.Margin = new Padding(0);
      this.label11.Name = "label11";
      this.label11.Size = new Size(41, 13);
      this.label11.TabIndex = 1;
      this.label11.Text = "label11";
      this.groupContainer1.AutoSize = true;
      this.groupContainer1.BackColor = Color.Transparent;
      this.groupContainer1.Borders = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      this.groupContainer1.Controls.Add((Control) this.btnSelect);
      this.groupContainer1.Controls.Add((Control) this.conditionTextBox);
      this.groupContainer1.Controls.Add((Control) this.conditionchkBx);
      this.groupContainer1.Controls.Add((Control) this.afterLESentChkBx);
      this.groupContainer1.Controls.Add((Control) this.label6);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(406, 318);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "Stop Automatically Populating Data";
      this.btnSelect.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSelect.BackColor = Color.Transparent;
      this.btnSelect.Enabled = false;
      this.btnSelect.Location = new Point(378, 70);
      this.btnSelect.Margin = new Padding(2, 2, 2, 2);
      this.btnSelect.MouseDownImage = (Image) null;
      this.btnSelect.Name = "btnSelect";
      this.btnSelect.Size = new Size(20, 20);
      this.btnSelect.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnSelect.TabIndex = 37;
      this.btnSelect.TabStop = false;
      this.btnSelect.Tag = (object) "Edit Condition";
      this.ttFieldRulesDlg.SetToolTip((Control) this.btnSelect, "Edit Condition");
      this.btnSelect.Click += new EventHandler(this.btnSelect_Click);
      this.conditionTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.conditionTextBox.Enabled = false;
      this.conditionTextBox.Location = new Point(12, 70);
      this.conditionTextBox.Margin = new Padding(2, 2, 2, 2);
      this.conditionTextBox.Multiline = true;
      this.conditionTextBox.Name = "conditionTextBox";
      this.conditionTextBox.Size = new Size(362, 242);
      this.conditionTextBox.TabIndex = 3;
      this.conditionchkBx.AutoSize = true;
      this.conditionchkBx.Location = new Point(12, 49);
      this.conditionchkBx.Margin = new Padding(2, 2, 2, 2);
      this.conditionchkBx.Name = "conditionchkBx";
      this.conditionchkBx.Size = new Size(162, 17);
      this.conditionchkBx.TabIndex = 2;
      this.conditionchkBx.Text = "When condition below is met";
      this.conditionchkBx.UseVisualStyleBackColor = true;
      this.conditionchkBx.CheckedChanged += new EventHandler(this.conditionchkBx_CheckedChanged);
      this.afterLESentChkBx.AutoSize = true;
      this.afterLESentChkBx.Location = new Point(12, 31);
      this.afterLESentChkBx.Margin = new Padding(2, 2, 2, 2);
      this.afterLESentChkBx.Name = "afterLESentChkBx";
      this.afterLESentChkBx.Size = new Size(177, 17);
      this.afterLESentChkBx.TabIndex = 1;
      this.afterLESentChkBx.Text = "After initial Loan Estimate is sent";
      this.afterLESentChkBx.UseVisualStyleBackColor = true;
      this.label6.AutoSize = true;
      this.label6.Location = new Point(-11, -12);
      this.label6.Margin = new Padding(2, 0, 2, 0);
      this.label6.Name = "label6";
      this.label6.Size = new Size(35, 13);
      this.label6.TabIndex = 1;
      this.label6.Text = "label6";
      this.cancelBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.cancelBtn.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cancelBtn.Location = new Point(755, 15);
      this.cancelBtn.Margin = new Padding(2, 2, 2, 2);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 23);
      this.cancelBtn.TabIndex = 1;
      this.cancelBtn.Text = "Cancel";
      this.cancelBtn.UseVisualStyleBackColor = true;
      this.cancelBtn.Click += new EventHandler(this.cancelBtn_Click);
      this.okBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.okBtn.Enabled = false;
      this.okBtn.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.okBtn.Location = new Point(676, 15);
      this.okBtn.Margin = new Padding(2, 2, 2, 2);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 23);
      this.okBtn.TabIndex = 0;
      this.okBtn.Text = "OK";
      this.okBtn.UseVisualStyleBackColor = true;
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.pictureBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.pictureBox1.BackColor = Color.Transparent;
      this.pictureBox1.Image = (Image) Resources.help;
      this.pictureBox1.Location = new Point(7, 18);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(16, 16);
      this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 17;
      this.pictureBox1.TabStop = false;
      this.lnkLblLearnMore.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lnkLblLearnMore.AutoSize = true;
      this.lnkLblLearnMore.ImageAlign = ContentAlignment.MiddleLeft;
      this.lnkLblLearnMore.LinkBehavior = LinkBehavior.NeverUnderline;
      this.lnkLblLearnMore.LinkColor = Color.SteelBlue;
      this.lnkLblLearnMore.Location = new Point(28, 20);
      this.lnkLblLearnMore.Name = "lnkLblLearnMore";
      this.lnkLblLearnMore.Size = new Size(70, 13);
      this.lnkLblLearnMore.TabIndex = 2;
      this.lnkLblLearnMore.TabStop = true;
      this.lnkLblLearnMore.Text = "Learn More...";
      this.lnkLblLearnMore.Click += new EventHandler(this.LearnMore_Click);
      this.panelRuleName.Controls.Add((Control) this.label4);
      this.panelRuleName.Controls.Add((Control) this.ruleNameTxtBx);
      this.panelRuleName.Controls.Add((Control) this.label3);
      this.panelRuleName.Dock = DockStyle.Top;
      this.panelRuleName.Location = new Point(0, 0);
      this.panelRuleName.Name = "panelRuleName";
      this.panelRuleName.Size = new Size(841, 39);
      this.panelRuleName.TabIndex = 1;
      this.label4.AutoSize = true;
      this.label4.ForeColor = Color.Red;
      this.label4.Location = new Point(92, 10);
      this.label4.Name = "label4";
      this.label4.Size = new Size(11, 13);
      this.label4.TabIndex = 8;
      this.label4.Text = "*";
      this.ruleNameTxtBx.ForeColor = SystemColors.WindowText;
      this.ruleNameTxtBx.Location = new Point(108, 10);
      this.ruleNameTxtBx.Margin = new Padding(2, 2, 2, 2);
      this.ruleNameTxtBx.MaxLength = 64;
      this.ruleNameTxtBx.Name = "ruleNameTxtBx";
      this.ruleNameTxtBx.Size = new Size(327, 20);
      this.ruleNameTxtBx.TabIndex = 1;
      this.ruleNameTxtBx.TextChanged += new EventHandler(this.ruleNameTxtBx_TextChanged);
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(8, 10);
      this.label3.Margin = new Padding(2, 0, 2, 0);
      this.label3.Name = "label3";
      this.label3.Size = new Size(85, 13);
      this.label3.TabIndex = 7;
      this.label3.Text = "Field Rule Name";
      this.panelBottom.Controls.Add((Control) this.cancelBtn);
      this.panelBottom.Controls.Add((Control) this.okBtn);
      this.panelBottom.Controls.Add((Control) this.lnkLblLearnMore);
      this.panelBottom.Controls.Add((Control) this.pictureBox1);
      this.panelBottom.Dock = DockStyle.Bottom;
      this.panelBottom.Location = new Point(0, 357);
      this.panelBottom.Name = "panelBottom";
      this.panelBottom.Size = new Size(841, 50);
      this.panelBottom.TabIndex = 4;
      this.panelMiddleLeft.Controls.Add((Control) this.groupContainer2);
      this.panelMiddleLeft.Dock = DockStyle.Left;
      this.panelMiddleLeft.Location = new Point(0, 39);
      this.panelMiddleLeft.Name = "panelMiddleLeft";
      this.panelMiddleLeft.Size = new Size(435, 318);
      this.panelMiddleLeft.TabIndex = 2;
      this.panelMiddleRight.Controls.Add((Control) this.groupContainer1);
      this.panelMiddleRight.Dock = DockStyle.Fill;
      this.panelMiddleRight.Location = new Point(435, 39);
      this.panelMiddleRight.Name = "panelMiddleRight";
      this.panelMiddleRight.Size = new Size(406, 318);
      this.panelMiddleRight.TabIndex = 3;
      this.verticalSeparator1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.verticalSeparator1.Location = new Point(408, 4);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 31;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(841, 407);
      this.Controls.Add((Control) this.panelMiddleRight);
      this.Controls.Add((Control) this.panelMiddleLeft);
      this.Controls.Add((Control) this.panelBottom);
      this.Controls.Add((Control) this.panelRuleName);
      this.KeyPreview = true;
      this.Margin = new Padding(2, 2, 2, 2);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (FieldRulesDlg);
      this.ShowIcon = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add New Field Rule";
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.groupContainer2.ResumeLayout(false);
      this.groupContainer2.PerformLayout();
      ((ISupportInitialize) this.stdIconBtnImport).EndInit();
      ((ISupportInitialize) this.stdButtonFind).EndInit();
      ((ISupportInitialize) this.stdButtonNew).EndInit();
      ((ISupportInitialize) this.stdButtonDelete).EndInit();
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      ((ISupportInitialize) this.btnSelect).EndInit();
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.panelRuleName.ResumeLayout(false);
      this.panelRuleName.PerformLayout();
      this.panelBottom.ResumeLayout(false);
      this.panelBottom.PerformLayout();
      this.panelMiddleLeft.ResumeLayout(false);
      this.panelMiddleLeft.PerformLayout();
      this.panelMiddleRight.ResumeLayout(false);
      this.panelMiddleRight.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
