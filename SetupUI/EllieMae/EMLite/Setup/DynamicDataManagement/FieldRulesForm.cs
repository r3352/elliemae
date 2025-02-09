// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.FieldRulesForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.DynamicDataManagement.ImportExport;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public class FieldRulesForm : UserControl, IOnlineHelpTarget
  {
    private Sessions.Session session;
    private DDMFieldRulesBpmManager fieldRulesBpmMgr;
    private const int STATUS = 2;
    private string fileNameForXML;
    private IContainer components;
    private GroupContainer gcBaseRate;
    private GridView listViewOptions;
    private StandardIconButton stdIconBtnNew;
    private StandardIconButton stdIconBtnDelete;
    private StandardIconButton stdIconBtnEdit;
    private StandardIconButton stdIconBtnDuplicate;
    private ToolTip ttFieldRuleMain;
    private VerticalSeparator verticalSeparator2;
    protected Button deactiveBtn;
    private VerticalSeparator verticalSeparator1;
    private StandardIconButton stdBtnExport;
    private SaveFileDialog exportFileDialog;
    private StandardIconButton stdBtnImport;

    public FieldRulesForm(SetUpContainer setupContainer, Sessions.Session session)
    {
      this.InitializeComponent();
      this.session = session;
      this.fieldRulesBpmMgr = (DDMFieldRulesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.DDMFieldRules);
      this.refresh();
      this.HandleStandardButtons();
    }

    private void HandleStandardButtons()
    {
      this.stdIconBtnDelete.Enabled = this.stdIconBtnDuplicate.Enabled = this.stdIconBtnEdit.Enabled = this.stdBtnExport.Enabled = this.listViewOptions.SelectedItems.Count > 0;
      this.deactiveBtn.Enabled = false;
      using (IEnumerator<DDMFieldRule> enumerator = this.listViewOptions.SelectedItems.Select<GVItem, DDMFieldRule>((Func<GVItem, DDMFieldRule>) (item => (DDMFieldRule) item.Tag)).Where<DDMFieldRule>((Func<DDMFieldRule, bool>) (fieldRule => fieldRule.Status == ruleStatus.Active)).GetEnumerator())
      {
        if (!enumerator.MoveNext())
          return;
        DDMFieldRule current = enumerator.Current;
        this.deactiveBtn.Enabled = true;
      }
    }

    private void refresh()
    {
      this.listViewOptions.Items.Clear();
      DDMFieldRule[] allDdmFieldRules = this.fieldRulesBpmMgr.GetAllDDMFieldRules(true);
      if (allDdmFieldRules == null)
        return;
      this.gcBaseRate.Text = "Field Rules List(" + (object) allDdmFieldRules.Length + ")";
      foreach (DDMFieldRule ddmFieldRule in allDdmFieldRules)
      {
        if (ddmFieldRule != null)
        {
          string str = Session.UserInfo.ToString() + " (" + Session.UserID + ")";
          GVItem gvItem = new GVItem(ddmFieldRule.RuleName.ToString());
          char[] chArray = new char[1]{ ',' };
          gvItem.SubItems.Add((object) ddmFieldRule.Fields.Trim(chArray));
          gvItem.SubItems.Add((object) ddmFieldRule.Status.ToString());
          gvItem.SubItems.Add((object) (ddmFieldRule.LastModByUserID + " (" + ddmFieldRule.LastModByFullName + ")"));
          gvItem.SubItems.Add((object) DateTime.Parse(ddmFieldRule.UpdateDt).ToString("MM/dd/yyyy hh:mm tt"));
          gvItem.Tag = (object) ddmFieldRule;
          this.listViewOptions.Items.Add(gvItem);
        }
        else
          break;
      }
      using (IEnumerator<GVColumn> enumerator = this.listViewOptions.Columns.Where<GVColumn>((Func<GVColumn, bool>) (col => col.SortPriority == 0)).GetEnumerator())
      {
        if (!enumerator.MoveNext())
          return;
        GVColumn current = enumerator.Current;
        this.listViewOptions.Sort(current.Index, current.SortOrder);
      }
    }

    private void stdIconBtnNew_Click(object sender, EventArgs e)
    {
      using (FieldRulesDlg fieldRulesDlg = new FieldRulesDlg(this.session, Session.UserID))
      {
        int num1 = (int) fieldRulesDlg.ShowDialog((IWin32Window) this);
        if (fieldRulesDlg.DialogResult == DialogResult.OK)
        {
          try
          {
            DDMFieldRule matchingFeeRuleItem = this.FindMatchingFeeRuleItem(fieldRulesDlg._DDMFieldRule);
            FieldScenarioRules fieldScenarioRules = new FieldScenarioRules(matchingFeeRuleItem, this.session, true, fieldRulesDlg.importIDValuePair, fieldRulesDlg.dataImportSource);
            int num2 = (int) fieldScenarioRules.ShowDialog((IWin32Window) this);
            if (fieldScenarioRules.ActiveScenarioCount > 0)
            {
              matchingFeeRuleItem.Status = ruleStatus.Active;
              this.fieldRulesBpmMgr.UpdateFieldRule(matchingFeeRuleItem, true);
              if (this.listViewOptions.SelectedItems.Count > 0)
                this.listViewOptions.SelectedItems[0].SubItems[2].Text = ruleStatus.Active.ToString();
            }
          }
          catch (Exception ex)
          {
          }
        }
        fieldRulesDlg.Dispose();
        this.refresh();
      }
    }

    private DDMFieldRule FindMatchingFeeRuleItem(DDMFieldRule ddmFieldRule)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.listViewOptions.Items)
      {
        if (((DDMFieldRule) gvItem.Tag).RuleName == ddmFieldRule.RuleName)
        {
          gvItem.Selected = true;
          return (DDMFieldRule) gvItem.Tag;
        }
      }
      return ddmFieldRule;
    }

    private void listViewOptions_Click(object sender, EventArgs e)
    {
    }

    private void stdIconBtnEdit_Click(object sender, EventArgs e)
    {
      if (this.listViewOptions.SelectedItems.Count == 0)
        return;
      DDMFieldRule tag = (DDMFieldRule) this.listViewOptions.SelectedItems[0].Tag;
      FieldScenarioRules fieldScenarioRules = new FieldScenarioRules(tag, this.session, true);
      fieldScenarioRules.ShowDialog((IWin32Window) this);
      if (fieldScenarioRules.ActiveScenarioCount > 0 && tag.Status == ruleStatus.Inactive)
      {
        tag.Status = ruleStatus.Active;
        ((DDMFieldRulesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.DDMFieldRules)).UpdateFieldRule(tag, true, activeDeActiveScenarioId: fieldScenarioRules.ActiveDeActiveScenarioId);
      }
      else if (fieldScenarioRules.ActiveScenarioCount == 0 && tag.Status == ruleStatus.Active)
      {
        tag.Status = ruleStatus.Inactive;
        ((DDMFieldRulesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.DDMFieldRules)).UpdateFieldRule(tag, true, activeDeActiveScenarioId: fieldScenarioRules.ActiveDeActiveScenarioId);
      }
      this.refresh();
    }

    private void stdIconBtnDuplicate_Click(object sender, EventArgs e)
    {
      if (this.listViewOptions.SelectedItems.Count == 0)
        return;
      DDMFieldRule tag = (DDMFieldRule) this.listViewOptions.SelectedItems[0].Tag;
      if (tag.Status == ruleStatus.Active)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You are attempting to duplicate an active field rule. The duplicated field rule created will be in 'Inactive' status. ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "The duplicated field rule will be created in 'Inactive' status. ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      DDMFieldRuleScenariosBpmManager bpmManager1 = (DDMFieldRuleScenariosBpmManager) this.session.BPM.GetBpmManager(BpmCategory.DDMFieldScenarioRules);
      List<DDMFieldRuleScenario> allRules = bpmManager1.GetAllRules(tag.RuleID, true);
      DDMFieldRule ddmFieldRule = (DDMFieldRule) tag.Clone();
      DDMFieldRulesBpmManager bpmManager2 = (DDMFieldRulesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.DDMFieldRules);
      this.UpdateUserInfo(ddmFieldRule);
      ddmFieldRule.RuleID = bpmManager2.CreateFieldRule(ddmFieldRule);
      foreach (DDMFieldRuleScenario fieldRuleScenario in allRules)
        ddmFieldRule.Scenarios.Add(fieldRuleScenario.CloneForDuplicate(ddmFieldRule.RuleID));
      bpmManager1.UpdateRules(ddmFieldRule.Scenarios);
      this.refresh();
    }

    private void UpdateUserInfo(DDMFieldRule rule)
    {
      string str = DateTime.Now.ToString();
      rule.LastModByUserID = Session.UserID;
      rule.LastModByFullName = Session.UserInfo.FullName;
      rule.CreateDt = str;
      rule.UpdateDt = str;
    }

    private void listViewOptions_DoubleClick(object sender, EventArgs e)
    {
      this.stdIconBtnEdit_Click(sender, e);
    }

    private void stdIconBtnDelete_Click(object sender, EventArgs e)
    {
      if (this.listViewOptions.SelectedItems.Count == 0)
        return;
      DDMFieldRule tag = (DDMFieldRule) this.listViewOptions.SelectedItems[0].Tag;
      if (tag.Status == ruleStatus.Active)
      {
        int num = (int) MessageBox.Show("You have at least one active scenario under this rule. Please deactivate the scenarios to delete the field rule.", "Encompass");
      }
      else
      {
        if (MessageBox.Show("Are you sure you want to delete this field rule?", "Encompass", MessageBoxButtons.YesNo) != DialogResult.Yes)
          return;
        DDMFieldRulesBpmManager bpmManager1 = (DDMFieldRulesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.DDMFieldRules);
        DDMFieldRuleScenariosBpmManager bpmManager2 = (DDMFieldRuleScenariosBpmManager) this.session.BPM.GetBpmManager(BpmCategory.DDMFieldScenarioRules);
        foreach (DDMFieldRuleScenario allRule in bpmManager2.GetAllRules(tag.RuleID, true))
          bpmManager2.DeleteRule(allRule.RuleID, forceToPrimaryDb: true);
        bpmManager1.DeleteDDMFieldRuleByID(tag.RuleID, true);
        this.listViewOptions.Items.Remove(this.listViewOptions.SelectedItems[0]);
        this.refresh();
      }
    }

    private void listViewOptions_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.HandleStandardButtons();
    }

    string IOnlineHelpTarget.GetHelpTargetName() => "Field Rules";

    private void deactiveBtn_Click(object sender, EventArgs e)
    {
      foreach (GVItem selectedItem in this.listViewOptions.SelectedItems)
      {
        DDMFieldRule tag = (DDMFieldRule) selectedItem.Tag;
        DDMFieldRulesBpmManager bpmManager = (DDMFieldRulesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.DDMFieldRules);
        DDMFieldRuleScenariosBpmManager fieldScenariosBpmManager = (DDMFieldRuleScenariosBpmManager) this.session.BPM.GetBpmManager(BpmCategory.DDMFieldScenarioRules);
        List<DDMFieldRuleScenario> allRules = fieldScenariosBpmManager.GetAllRules(tag.RuleID, true);
        int num;
        Parallel.ForEach<DDMFieldRuleScenario>((IEnumerable<DDMFieldRuleScenario>) allRules, (Action<DDMFieldRuleScenario>) (scen => num = (int) fieldScenariosBpmManager.DeactivateRule(scen.RuleID, forceToPrimaryDb: true)));
        tag.Scenarios = allRules;
        tag.Status = ruleStatus.Inactive;
        bpmManager.UpdateFieldRule(tag, true);
      }
      this.refresh();
    }

    private void stdBtnExport_Click(object sender, EventArgs e)
    {
      if (this.listViewOptions.SelectedItems.Count == 0)
        return;
      if (this.listViewOptions.SelectedItems.Count > 1)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select one field rule at a time", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        XDocument xdocument = XDocument.Parse(XElement.Load((XmlReader) JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(DDMRestApiHelper.ExportFieldRule(((DDMFieldRule) this.listViewOptions.SelectedItems[0].Tag).RuleID, "xml")), new XmlDictionaryReaderQuotas())).Value);
        string str = "DDM_" + xdocument.Root.Attribute((XName) "RuleType").Value + "_" + xdocument.Root.Attribute((XName) "Name").Value;
        foreach (char ch in new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars()))
          str = str.Replace(ch.ToString(), "");
        this.fileNameForXML = str + ".xml";
        if (System.IO.File.Exists(SystemSettings.TempFolderRoot + this.fileNameForXML))
          System.IO.File.Delete(SystemSettings.TempFolderRoot + this.fileNameForXML);
        xdocument.Save(SystemSettings.TempFolderRoot + this.fileNameForXML);
        this.exportFileDialog.Filter = "zip files (*.zip)|*.zip";
        this.exportFileDialog.FileName = str + ".zip";
        int num2 = (int) this.exportFileDialog.ShowDialog();
      }
    }

    private void exportFileDialog_FileOk(object sender, CancelEventArgs e)
    {
      FileCompressor.Instance.ZipFile(SystemSettings.TempFolderRoot + this.fileNameForXML, this.exportFileDialog.FileName);
    }

    private void stdBtnImport_Click(object sender, EventArgs e)
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Filter = "zip files (*.zip)|*.zip";
      openFileDialog.Multiselect = false;
      XmlDocument parsedXml = new XmlDocument();
      if (DialogResult.OK != openFileDialog.ShowDialog())
        return;
      try
      {
        string fileName = openFileDialog.FileName;
        string str = SystemSettings.TempFolderRoot + "FieldRule_" + Guid.NewGuid().ToString() + "\\";
        if (Directory.Exists(str))
          Directory.Delete(str, true);
        FileCompressor.Instance.Unzip(openFileDialog.FileName, str);
        string[] files = Directory.GetFiles(str);
        if (((IEnumerable<string>) files).Count<string>() < 1)
          throw new Exception("No files in this zip file, please check.");
        parsedXml.Load(files[0]);
        HttpResponseMessage httpResponseMessage = DDMRestApiHelper.ValidateFieldRule(parsedXml.OuterXml);
        string result = httpResponseMessage.Content.ReadAsStringAsync().Result;
        JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
        EllieMae.EMLite.Setup.DynamicDataManagement.ImportExport.ExternalEntities exEntities = scriptSerializer.Deserialize<EllieMae.EMLite.Setup.DynamicDataManagement.ImportExport.ExternalEntities>(result);
        if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
        {
          if (exEntities != null && exEntities.details != null)
            throw new Exception(exEntities.details);
          throw new Exception("Not able to get a validation result from EBS. Request Status: " + httpResponseMessage.StatusCode.ToString());
        }
        XmlElement documentElement = parsedXml.DocumentElement;
        string attribute1 = documentElement.GetAttribute("Name");
        string attribute2 = documentElement.GetAttribute("LastModifiedBy");
        string dateTime = documentElement.GetAttribute("LastModifiedDateTime") == "" ? "" : Utils.ParseUTCDateTime(documentElement.GetAttribute("LastModifiedDateTime")).ToString("G");
        if (exEntities.RuleAlreadyExists.Equals("True"))
        {
          int num1 = (int) Utils.Dialog((IWin32Window) this, string.Format("{0} already exists. Please rename the rule and try importing it again", (object) attribute1), MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          DDMRuleValidationResult ui = new DDMRuleValidator(this.session.SessionObjects, BizRuleType.DDMFieldRules, parsedXml, exEntities).mapExEntitiesToUI();
          if (new DDMRuleImportInfoDlg(ImportType.FieldRule, attribute1, string.Empty, dateTime, attribute2, ui).ShowDialog() != DialogResult.OK)
            return;
          string input = DDMRestApiHelper.ImportFieldRule(parsedXml.OuterXml);
          ImportResultResponse importResultResponse = scriptSerializer.Deserialize<ImportResultResponse>(input);
          if (!string.IsNullOrEmpty(importResultResponse.RuleId))
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, string.Format("{0} has been successfully imported.", (object) attribute1), MessageBoxButtons.OK, MessageBoxIcon.None);
            this.session.BPM.GetBpmManager(BizRuleType.DDMFeeScenarios).BPMManager.InvalidateCache(BizRuleType.DDMFieldScenarios);
            this.refresh();
          }
          else
          {
            int num3 = (int) Utils.Dialog((IWin32Window) this, "Import failed: " + importResultResponse.ErrorMessage, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Field rule import fails: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      this.gcBaseRate = new GroupContainer();
      this.stdBtnImport = new StandardIconButton();
      this.stdBtnExport = new StandardIconButton();
      this.verticalSeparator1 = new VerticalSeparator();
      this.verticalSeparator2 = new VerticalSeparator();
      this.deactiveBtn = new Button();
      this.stdIconBtnDelete = new StandardIconButton();
      this.stdIconBtnEdit = new StandardIconButton();
      this.stdIconBtnDuplicate = new StandardIconButton();
      this.stdIconBtnNew = new StandardIconButton();
      this.listViewOptions = new GridView();
      this.ttFieldRuleMain = new ToolTip(this.components);
      this.exportFileDialog = new SaveFileDialog();
      this.gcBaseRate.SuspendLayout();
      ((ISupportInitialize) this.stdBtnImport).BeginInit();
      ((ISupportInitialize) this.stdBtnExport).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDelete).BeginInit();
      ((ISupportInitialize) this.stdIconBtnEdit).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDuplicate).BeginInit();
      ((ISupportInitialize) this.stdIconBtnNew).BeginInit();
      this.SuspendLayout();
      this.gcBaseRate.Controls.Add((Control) this.stdBtnImport);
      this.gcBaseRate.Controls.Add((Control) this.stdBtnExport);
      this.gcBaseRate.Controls.Add((Control) this.verticalSeparator1);
      this.gcBaseRate.Controls.Add((Control) this.verticalSeparator2);
      this.gcBaseRate.Controls.Add((Control) this.deactiveBtn);
      this.gcBaseRate.Controls.Add((Control) this.stdIconBtnDelete);
      this.gcBaseRate.Controls.Add((Control) this.stdIconBtnEdit);
      this.gcBaseRate.Controls.Add((Control) this.stdIconBtnDuplicate);
      this.gcBaseRate.Controls.Add((Control) this.stdIconBtnNew);
      this.gcBaseRate.Controls.Add((Control) this.listViewOptions);
      this.gcBaseRate.Dock = DockStyle.Fill;
      this.gcBaseRate.HeaderForeColor = SystemColors.ControlText;
      this.gcBaseRate.Location = new Point(0, 0);
      this.gcBaseRate.Name = "gcBaseRate";
      this.gcBaseRate.Size = new Size(1024, 517);
      this.gcBaseRate.TabIndex = 0;
      this.gcBaseRate.Text = "Field Rules List (0)";
      this.stdBtnImport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdBtnImport.BackColor = Color.Transparent;
      this.stdBtnImport.Location = new Point(917, 5);
      this.stdBtnImport.MouseDownImage = (Image) null;
      this.stdBtnImport.Name = "stdBtnImport";
      this.stdBtnImport.Size = new Size(16, 16);
      this.stdBtnImport.StandardButtonType = StandardIconButton.ButtonType.ImportLoanButton;
      this.stdBtnImport.TabIndex = 29;
      this.stdBtnImport.TabStop = false;
      this.ttFieldRuleMain.SetToolTip((Control) this.stdBtnImport, "Import");
      this.stdBtnImport.Click += new EventHandler(this.stdBtnImport_Click);
      this.stdBtnExport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdBtnExport.BackColor = Color.Transparent;
      this.stdBtnExport.Location = new Point(896, 5);
      this.stdBtnExport.MouseDownImage = (Image) null;
      this.stdBtnExport.Name = "stdBtnExport";
      this.stdBtnExport.Size = new Size(16, 16);
      this.stdBtnExport.StandardButtonType = StandardIconButton.ButtonType.ExportDataToFileButton;
      this.stdBtnExport.TabIndex = 28;
      this.stdBtnExport.TabStop = false;
      this.ttFieldRuleMain.SetToolTip((Control) this.stdBtnExport, "Export");
      this.stdBtnExport.Click += new EventHandler(this.stdBtnExport_Click);
      this.verticalSeparator1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.verticalSeparator1.Location = new Point(890, 6);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 27;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.verticalSeparator2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.verticalSeparator2.Location = new Point(936, 6);
      this.verticalSeparator2.MaximumSize = new Size(2, 16);
      this.verticalSeparator2.MinimumSize = new Size(2, 16);
      this.verticalSeparator2.Name = "verticalSeparator2";
      this.verticalSeparator2.Size = new Size(2, 16);
      this.verticalSeparator2.TabIndex = 26;
      this.verticalSeparator2.Text = "verticalSeparator2";
      this.deactiveBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.deactiveBtn.BackColor = SystemColors.Control;
      this.deactiveBtn.Location = new Point(941, 1);
      this.deactiveBtn.Name = "deactiveBtn";
      this.deactiveBtn.Size = new Size(80, 22);
      this.deactiveBtn.TabIndex = 25;
      this.deactiveBtn.Text = "Deac&tivate";
      this.deactiveBtn.UseVisualStyleBackColor = true;
      this.deactiveBtn.Click += new EventHandler(this.deactiveBtn_Click);
      this.stdIconBtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDelete.BackColor = Color.Transparent;
      this.stdIconBtnDelete.Location = new Point(868, 5);
      this.stdIconBtnDelete.MouseDownImage = (Image) null;
      this.stdIconBtnDelete.Name = "stdIconBtnDelete";
      this.stdIconBtnDelete.Size = new Size(16, 16);
      this.stdIconBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDelete.TabIndex = 24;
      this.stdIconBtnDelete.TabStop = false;
      this.ttFieldRuleMain.SetToolTip((Control) this.stdIconBtnDelete, "Delete");
      this.stdIconBtnDelete.Click += new EventHandler(this.stdIconBtnDelete_Click);
      this.stdIconBtnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnEdit.BackColor = Color.Transparent;
      this.stdIconBtnEdit.Location = new Point(845, 5);
      this.stdIconBtnEdit.MouseDownImage = (Image) null;
      this.stdIconBtnEdit.Name = "stdIconBtnEdit";
      this.stdIconBtnEdit.Size = new Size(16, 16);
      this.stdIconBtnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdIconBtnEdit.TabIndex = 23;
      this.stdIconBtnEdit.TabStop = false;
      this.ttFieldRuleMain.SetToolTip((Control) this.stdIconBtnEdit, "Edit");
      this.stdIconBtnEdit.Click += new EventHandler(this.stdIconBtnEdit_Click);
      this.stdIconBtnDuplicate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDuplicate.BackColor = Color.Transparent;
      this.stdIconBtnDuplicate.Location = new Point(822, 5);
      this.stdIconBtnDuplicate.MouseDownImage = (Image) null;
      this.stdIconBtnDuplicate.Name = "stdIconBtnDuplicate";
      this.stdIconBtnDuplicate.Size = new Size(16, 16);
      this.stdIconBtnDuplicate.StandardButtonType = StandardIconButton.ButtonType.DuplicateButton;
      this.stdIconBtnDuplicate.TabIndex = 22;
      this.stdIconBtnDuplicate.TabStop = false;
      this.ttFieldRuleMain.SetToolTip((Control) this.stdIconBtnDuplicate, "Duplicate");
      this.stdIconBtnDuplicate.Click += new EventHandler(this.stdIconBtnDuplicate_Click);
      this.stdIconBtnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnNew.BackColor = Color.Transparent;
      this.stdIconBtnNew.Location = new Point(799, 5);
      this.stdIconBtnNew.MouseDownImage = (Image) null;
      this.stdIconBtnNew.Name = "stdIconBtnNew";
      this.stdIconBtnNew.Size = new Size(16, 16);
      this.stdIconBtnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnNew.TabIndex = 21;
      this.stdIconBtnNew.TabStop = false;
      this.ttFieldRuleMain.SetToolTip((Control) this.stdIconBtnNew, "New");
      this.stdIconBtnNew.Click += new EventHandler(this.stdIconBtnNew_Click);
      this.listViewOptions.AutoHeight = true;
      this.listViewOptions.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Name";
      gvColumn1.Width = 175;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Field IDs";
      gvColumn2.Width = 350;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Status";
      gvColumn3.Width = 175;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "Last Modified By";
      gvColumn4.Width = 175;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.SortMethod = GVSortMethod.DateTime;
      gvColumn5.Text = "Last Modified Date & Time";
      gvColumn5.Width = 175;
      this.listViewOptions.Columns.AddRange(new GVColumn[5]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5
      });
      this.listViewOptions.Dock = DockStyle.Fill;
      this.listViewOptions.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewOptions.Location = new Point(1, 26);
      this.listViewOptions.Name = "listViewOptions";
      this.listViewOptions.Size = new Size(1022, 490);
      this.listViewOptions.SortingType = SortingType.AlphaNumeric;
      this.listViewOptions.TabIndex = 0;
      this.listViewOptions.SelectedIndexChanged += new EventHandler(this.listViewOptions_SelectedIndexChanged);
      this.listViewOptions.DoubleClick += new EventHandler(this.listViewOptions_DoubleClick);
      this.exportFileDialog.FileOk += new CancelEventHandler(this.exportFileDialog_FileOk);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcBaseRate);
      this.Name = nameof (FieldRulesForm);
      this.Size = new Size(1024, 517);
      this.gcBaseRate.ResumeLayout(false);
      ((ISupportInitialize) this.stdBtnImport).EndInit();
      ((ISupportInitialize) this.stdBtnExport).EndInit();
      ((ISupportInitialize) this.stdIconBtnDelete).EndInit();
      ((ISupportInitialize) this.stdIconBtnEdit).EndInit();
      ((ISupportInitialize) this.stdIconBtnDuplicate).EndInit();
      ((ISupportInitialize) this.stdIconBtnNew).EndInit();
      this.ResumeLayout(false);
    }
  }
}
