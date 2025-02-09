// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.FeeRulesMain
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.DynamicDataManagement;
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
namespace EllieMae.EMLite.Setup
{
  public class FeeRulesMain : SettingsUserControl, IOnlineHelpTarget
  {
    private Sessions.Session session;
    private DDMFeeRulesBpmManager feeRulesBpmMgr;
    private ToolTip ttFeeRulesMain;
    private const int STATUS = 2;
    protected Button deactiveBtn;
    private VerticalSeparator verticalSeparator2;
    private StandardIconButton stdBtnExport;
    private VerticalSeparator verticalSeparator1;
    private SaveFileDialog exportFileDialog;
    private StandardIconButton stdBtnImport;
    private IContainer components;
    private GroupContainer gcFeeRules;
    private StandardIconButton stdButtonDelete;
    private StandardIconButton stdButtonEdit;
    private StandardIconButton stdButtonNew;
    private GridView gvFeeRulesList;
    private string fileNameForXML;

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
      this.gcFeeRules = new GroupContainer();
      this.stdBtnImport = new StandardIconButton();
      this.verticalSeparator1 = new VerticalSeparator();
      this.stdBtnExport = new StandardIconButton();
      this.verticalSeparator2 = new VerticalSeparator();
      this.deactiveBtn = new Button();
      this.stdButtonDelete = new StandardIconButton();
      this.stdButtonEdit = new StandardIconButton();
      this.stdButtonNew = new StandardIconButton();
      this.gvFeeRulesList = new GridView();
      this.ttFeeRulesMain = new ToolTip(this.components);
      this.exportFileDialog = new SaveFileDialog();
      this.gcFeeRules.SuspendLayout();
      ((ISupportInitialize) this.stdBtnImport).BeginInit();
      ((ISupportInitialize) this.stdBtnExport).BeginInit();
      ((ISupportInitialize) this.stdButtonDelete).BeginInit();
      ((ISupportInitialize) this.stdButtonEdit).BeginInit();
      ((ISupportInitialize) this.stdButtonNew).BeginInit();
      this.SuspendLayout();
      this.gcFeeRules.Controls.Add((Control) this.stdBtnImport);
      this.gcFeeRules.Controls.Add((Control) this.verticalSeparator1);
      this.gcFeeRules.Controls.Add((Control) this.stdBtnExport);
      this.gcFeeRules.Controls.Add((Control) this.verticalSeparator2);
      this.gcFeeRules.Controls.Add((Control) this.deactiveBtn);
      this.gcFeeRules.Controls.Add((Control) this.stdButtonDelete);
      this.gcFeeRules.Controls.Add((Control) this.stdButtonEdit);
      this.gcFeeRules.Controls.Add((Control) this.stdButtonNew);
      this.gcFeeRules.Controls.Add((Control) this.gvFeeRulesList);
      this.gcFeeRules.Dock = DockStyle.Fill;
      this.gcFeeRules.HeaderForeColor = SystemColors.ControlText;
      this.gcFeeRules.Location = new Point(0, 0);
      this.gcFeeRules.Name = "gcFeeRules";
      this.gcFeeRules.Size = new Size(700, 495);
      this.gcFeeRules.TabIndex = 1;
      this.stdBtnImport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdBtnImport.BackColor = Color.Transparent;
      this.stdBtnImport.Location = new Point(591, 5);
      this.stdBtnImport.MouseDownImage = (Image) null;
      this.stdBtnImport.Name = "stdBtnImport";
      this.stdBtnImport.Size = new Size(16, 16);
      this.stdBtnImport.StandardButtonType = StandardIconButton.ButtonType.ImportLoanButton;
      this.stdBtnImport.TabIndex = 19;
      this.stdBtnImport.TabStop = false;
      this.ttFeeRulesMain.SetToolTip((Control) this.stdBtnImport, "Import");
      this.stdBtnImport.Click += new EventHandler(this.stdBtnImport_Click);
      this.verticalSeparator1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.verticalSeparator1.Location = new Point(609, 5);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 18;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.stdBtnExport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdBtnExport.BackColor = Color.Transparent;
      this.stdBtnExport.Location = new Point(571, 4);
      this.stdBtnExport.MouseDownImage = (Image) null;
      this.stdBtnExport.Name = "stdBtnExport";
      this.stdBtnExport.Size = new Size(16, 16);
      this.stdBtnExport.StandardButtonType = StandardIconButton.ButtonType.ExportDataToFileButton;
      this.stdBtnExport.TabIndex = 16;
      this.stdBtnExport.TabStop = false;
      this.ttFeeRulesMain.SetToolTip((Control) this.stdBtnExport, "Export");
      this.stdBtnExport.Click += new EventHandler(this.stdBtnExport_Click);
      this.verticalSeparator2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.verticalSeparator2.Location = new Point(565, 5);
      this.verticalSeparator2.MaximumSize = new Size(2, 16);
      this.verticalSeparator2.MinimumSize = new Size(2, 16);
      this.verticalSeparator2.Name = "verticalSeparator2";
      this.verticalSeparator2.Size = new Size(2, 16);
      this.verticalSeparator2.TabIndex = 15;
      this.verticalSeparator2.Text = "verticalSeparator2";
      this.verticalSeparator2.Click += new EventHandler(this.verticalSeparator2_Click);
      this.deactiveBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.deactiveBtn.BackColor = SystemColors.Control;
      this.deactiveBtn.Location = new Point(614, 1);
      this.deactiveBtn.Name = "deactiveBtn";
      this.deactiveBtn.Size = new Size(80, 22);
      this.deactiveBtn.TabIndex = 5;
      this.deactiveBtn.Text = "Deac&tivate";
      this.deactiveBtn.UseVisualStyleBackColor = true;
      this.deactiveBtn.Click += new EventHandler(this.deactiveBtn_Click);
      this.stdButtonDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdButtonDelete.BackColor = Color.Transparent;
      this.stdButtonDelete.Location = new Point(544, 4);
      this.stdButtonDelete.MouseDownImage = (Image) null;
      this.stdButtonDelete.Name = "stdButtonDelete";
      this.stdButtonDelete.Size = new Size(16, 16);
      this.stdButtonDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdButtonDelete.TabIndex = 4;
      this.stdButtonDelete.TabStop = false;
      this.ttFeeRulesMain.SetToolTip((Control) this.stdButtonDelete, "Delete");
      this.stdButtonDelete.Click += new EventHandler(this.stdButtonDelete_Click);
      this.stdButtonEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdButtonEdit.BackColor = Color.Transparent;
      this.stdButtonEdit.Location = new Point(523, 4);
      this.stdButtonEdit.MouseDownImage = (Image) null;
      this.stdButtonEdit.Name = "stdButtonEdit";
      this.stdButtonEdit.Size = new Size(16, 16);
      this.stdButtonEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdButtonEdit.TabIndex = 3;
      this.stdButtonEdit.TabStop = false;
      this.ttFeeRulesMain.SetToolTip((Control) this.stdButtonEdit, "Edit");
      this.stdButtonEdit.Click += new EventHandler(this.stdButtonEdit_Click);
      this.stdButtonNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdButtonNew.BackColor = Color.Transparent;
      this.stdButtonNew.Location = new Point(502, 4);
      this.stdButtonNew.MouseDownImage = (Image) null;
      this.stdButtonNew.Name = "stdButtonNew";
      this.stdButtonNew.Size = new Size(16, 16);
      this.stdButtonNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdButtonNew.TabIndex = 1;
      this.stdButtonNew.TabStop = false;
      this.ttFeeRulesMain.SetToolTip((Control) this.stdButtonNew, "New");
      this.stdButtonNew.Click += new EventHandler(this.stdButtonNew_Click);
      this.gvFeeRulesList.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Tag = (object) "Name";
      gvColumn1.Text = "Name";
      gvColumn1.Width = 250;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Tag = (object) "LineNumGroup";
      gvColumn2.Text = "Line #";
      gvColumn2.Width = 200;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Tag = (object) "Status";
      gvColumn3.Text = "Status";
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Tag = (object) "LastModBy";
      gvColumn4.Text = "Last Modified By";
      gvColumn4.Width = 200;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.SortMethod = GVSortMethod.DateTime;
      gvColumn5.Tag = (object) "LastModDateTime";
      gvColumn5.Text = "Last Modified Date & Time";
      gvColumn5.Width = 200;
      this.gvFeeRulesList.Columns.AddRange(new GVColumn[5]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5
      });
      this.gvFeeRulesList.Dock = DockStyle.Fill;
      this.gvFeeRulesList.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvFeeRulesList.Location = new Point(1, 26);
      this.gvFeeRulesList.Name = "gvFeeRulesList";
      this.gvFeeRulesList.Size = new Size(698, 468);
      this.gvFeeRulesList.SortingType = SortingType.AlphaNumeric;
      this.gvFeeRulesList.TabIndex = 0;
      this.gvFeeRulesList.SelectedIndexChanged += new EventHandler(this.gvFeeRulesList_SelectedIndexChanged);
      this.gvFeeRulesList.Click += new EventHandler(this.gvFeeRulesList_Click);
      this.gvFeeRulesList.DoubleClick += new EventHandler(this.gvFeeRulesList_DoubleClick);
      this.exportFileDialog.FileOk += new CancelEventHandler(this.exportFileDialog_FileOk);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcFeeRules);
      this.Margin = new Padding(2);
      this.Name = nameof (FeeRulesMain);
      this.Size = new Size(700, 495);
      this.gcFeeRules.ResumeLayout(false);
      ((ISupportInitialize) this.stdBtnImport).EndInit();
      ((ISupportInitialize) this.stdBtnExport).EndInit();
      ((ISupportInitialize) this.stdButtonDelete).EndInit();
      ((ISupportInitialize) this.stdButtonEdit).EndInit();
      ((ISupportInitialize) this.stdButtonNew).EndInit();
      this.ResumeLayout(false);
    }

    public FeeRulesMain(SetUpContainer setupContainer, Sessions.Session session)
      : base(setupContainer)
    {
      this.setupContainer = setupContainer;
      this.session = session;
      this.InitializeComponent();
      this.feeRulesBpmMgr = (DDMFeeRulesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.DDMFeeRules);
      this.refresh();
      this.HandleStandardButtons();
    }

    private void HandleStandardButtons()
    {
      this.stdButtonDelete.Enabled = this.stdButtonEdit.Enabled = this.stdBtnExport.Enabled = this.gvFeeRulesList.SelectedItems.Count > 0;
      this.deactiveBtn.Enabled = false;
      using (IEnumerator<DDMFeeRule> enumerator = this.gvFeeRulesList.SelectedItems.Select<GVItem, DDMFeeRule>((Func<GVItem, DDMFeeRule>) (item => (DDMFeeRule) item.Tag)).Where<DDMFeeRule>((Func<DDMFeeRule, bool>) (feeRule => feeRule.Status == ruleStatus.Active)).GetEnumerator())
      {
        if (!enumerator.MoveNext())
          return;
        DDMFeeRule current = enumerator.Current;
        this.deactiveBtn.Enabled = true;
      }
    }

    private void UpdateListCount()
    {
      this.gcFeeRules.Text = "Fee Rules List (" + (object) this.gvFeeRulesList.Items.Count + ")";
    }

    private void refresh()
    {
      this.gvFeeRulesList.Items.Clear();
      DDMFeeRule[] allDdmFeeRules = this.feeRulesBpmMgr.GetAllDDMFeeRules(true);
      if (allDdmFeeRules != null)
      {
        foreach (DDMFeeRule ddmFeeRule in allDdmFeeRules)
        {
          if (ddmFeeRule != null)
          {
            string str = Session.UserInfo.ToString() + " (" + Session.UserID + ")";
            this.gvFeeRulesList.Items.Add(new GVItem(ddmFeeRule.RuleName.ToString())
            {
              SubItems = {
                (object) ddmFeeRule.FeeLine,
                (object) ddmFeeRule.Status.ToString(),
                (object) (ddmFeeRule.LastModByUserID + " (" + ddmFeeRule.LastModByFullName + ")"),
                (object) ddmFeeRule.UpdateDt.ToString()
              },
              Tag = (object) ddmFeeRule
            });
          }
          else
            break;
        }
      }
      using (IEnumerator<GVColumn> enumerator = this.gvFeeRulesList.Columns.Where<GVColumn>((Func<GVColumn, bool>) (col => col.SortPriority == 0)).GetEnumerator())
      {
        if (enumerator.MoveNext())
        {
          GVColumn current = enumerator.Current;
          this.gvFeeRulesList.Sort(current.Index, current.SortOrder);
        }
      }
      this.UpdateListCount();
    }

    private void stdButtonNew_Click(object sender, EventArgs e)
    {
      using (FeeRulesDlg feeRulesDlg = new FeeRulesDlg(this.session, Session.UserID))
      {
        feeRulesDlg.ShowInTaskbar = false;
        int num1 = (int) feeRulesDlg.ShowDialog((IWin32Window) this);
        if (feeRulesDlg.DialogResult == DialogResult.OK)
        {
          try
          {
            DDMFeeRule matchingFeeRuleItem = this.FindMatchingFeeRuleItem(feeRulesDlg.DDMFeeRule);
            FeeScenarioRules feeScenarioRules = new FeeScenarioRules(matchingFeeRuleItem, this.session, false);
            int num2 = (int) feeScenarioRules.ShowDialog((IWin32Window) this);
            if (feeScenarioRules.ActiveScenarioCount > 0)
            {
              matchingFeeRuleItem.Status = ruleStatus.Active;
              this.feeRulesBpmMgr.UpdateDDMFeeRuleByID(matchingFeeRuleItem.RuleID, matchingFeeRuleItem, true);
              if (this.gvFeeRulesList.SelectedItems.Count > 0)
                this.gvFeeRulesList.SelectedItems[0].SubItems[2].Text = ruleStatus.Active.ToString();
            }
          }
          catch (Exception ex)
          {
          }
        }
        feeRulesDlg.Dispose();
        this.refresh();
      }
    }

    private DDMFeeRule FindMatchingFeeRuleItem(DDMFeeRule ddmFeeRule)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvFeeRulesList.Items)
      {
        if (((DDMFeeRule) gvItem.Tag).RuleName == ddmFeeRule.RuleName)
        {
          gvItem.Selected = true;
          return (DDMFeeRule) gvItem.Tag;
        }
      }
      return ddmFeeRule;
    }

    private void stdButtonEdit_Click(object sender, EventArgs e)
    {
      bool statusUpdate = false;
      if (this.gvFeeRulesList.SelectedItems.Count == 0)
        return;
      DDMFeeRule tag = (DDMFeeRule) this.gvFeeRulesList.SelectedItems[0].Tag;
      FeeScenarioRules feeScenarioRules = new FeeScenarioRules(tag, this.session, true);
      feeScenarioRules.ShowDialog((IWin32Window) this);
      if (tag.Status == ruleStatus.Active && feeScenarioRules.ActiveScenarioCount == 0)
      {
        tag.Status = ruleStatus.Inactive;
        statusUpdate = true;
      }
      else if (tag.Status == ruleStatus.Inactive && feeScenarioRules.ActiveScenarioCount > 0)
      {
        tag.Status = ruleStatus.Active;
        statusUpdate = true;
      }
      if (statusUpdate)
        this.UpdateDDMFeeRuleByID(tag.RuleID, tag, statusUpdate, feeScenarioRules.ActiveDeActiveScenarioId);
      this.refresh();
    }

    private void UpdateDDMFeeRuleByID(
      int ruleID,
      DDMFeeRule feeRule,
      bool statusUpdate = false,
      int activeDeActiveScenarioId = 0)
    {
      ((DDMFeeRulesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.DDMFeeRules)).UpdateDDMFeeRuleByID(ruleID, feeRule, statusUpdate, activeDeActiveScenarioId: activeDeActiveScenarioId);
    }

    private void gvFeeRulesList_DoubleClick(object sender, EventArgs e)
    {
      this.stdButtonEdit_Click(sender, e);
    }

    private void stdButtonDelete_Click(object sender, EventArgs e)
    {
      if (this.gvFeeRulesList.SelectedItems.Count == 0)
        return;
      DDMFeeRule tag = (DDMFeeRule) this.gvFeeRulesList.SelectedItems[0].Tag;
      if (tag.Status == ruleStatus.Active)
      {
        int num = (int) MessageBox.Show("You have at least one active scenario under this rule. Please deactivate the scenarios to delete the fee rule.", "Encompass");
      }
      else
      {
        if (MessageBox.Show("Are you sure you want to delete this fee rule?", "Encompass", MessageBoxButtons.YesNo) != DialogResult.Yes)
          return;
        DDMFeeRulesBpmManager bpmManager1 = (DDMFeeRulesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.DDMFeeRules);
        DDMFeeRuleScenariosBpmManager bpmManager2 = (DDMFeeRuleScenariosBpmManager) this.session.BPM.GetBpmManager(BpmCategory.DDMFeeScanarioRules);
        foreach (DDMFeeRuleScenario allRule in bpmManager2.GetAllRules(tag.RuleID, true))
          bpmManager2.DeleteRule(allRule.RuleID, forceToPrimaryDb: true);
        bpmManager1.DeleteDDMFeeRuleByID(tag.RuleID, true);
        this.gvFeeRulesList.Items.Remove(this.gvFeeRulesList.SelectedItems[0]);
        this.refresh();
      }
    }

    private void gvFeeRulesList_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.HandleStandardButtons();
    }

    string IOnlineHelpTarget.GetHelpTargetName() => "Fee Rules";

    private void deactiveBtn_Click(object sender, EventArgs e)
    {
      foreach (GVItem selectedItem in this.gvFeeRulesList.SelectedItems)
      {
        DDMFeeRule tag = (DDMFeeRule) selectedItem.Tag;
        DDMFeeRulesBpmManager bpmManager = (DDMFeeRulesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.DDMFeeRules);
        DDMFeeRuleScenariosBpmManager feeScenariosBpmManager = (DDMFeeRuleScenariosBpmManager) this.session.BPM.GetBpmManager(BpmCategory.DDMFeeScanarioRules);
        List<DDMFeeRuleScenario> allRules = feeScenariosBpmManager.GetAllRules(tag.RuleID, true);
        int num;
        Parallel.ForEach<DDMFeeRuleScenario>((IEnumerable<DDMFeeRuleScenario>) allRules, (Action<DDMFeeRuleScenario>) (scen => num = (int) feeScenariosBpmManager.DeactivateRule(scen.RuleID, forceToPrimaryDb: true)));
        tag.Scenarios = allRules;
        tag.Status = ruleStatus.Inactive;
        bpmManager.UpdateDDMFeeRuleByID(tag.RuleID, tag, true);
      }
      this.refresh();
    }

    private void verticalSeparator2_Click(object sender, EventArgs e)
    {
    }

    private void gvFeeRulesList_Click(object sender, EventArgs e)
    {
    }

    private void stdBtnExport_Click(object sender, EventArgs e)
    {
      if (this.gvFeeRulesList.SelectedItems.Count == 0)
        return;
      if (this.gvFeeRulesList.SelectedItems.Count > 1)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select one fee rule at a time", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        XDocument xdocument = XDocument.Parse(XElement.Load((XmlReader) JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(DDMRestApiHelper.ExportFeeRule(((DDMFeeRule) this.gvFeeRulesList.SelectedItems[0].Tag).RuleID, "xml")), new XmlDictionaryReaderQuotas())).Value);
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
        string str = SystemSettings.TempFolderRoot + "FeeRule_" + Guid.NewGuid().ToString() + "\\";
        if (Directory.Exists(str))
          Directory.Delete(str, true);
        FileCompressor.Instance.Unzip(openFileDialog.FileName, str);
        string[] files = Directory.GetFiles(str);
        if (((IEnumerable<string>) files).Count<string>() < 1)
          throw new Exception("No files in this zip file, please check.");
        parsedXml.Load(files[0]);
        HttpResponseMessage httpResponseMessage = DDMRestApiHelper.ValidateFeeRule(parsedXml.OuterXml);
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
        string innerText = documentElement.GetElementsByTagName("GlobalSettings").Item(0).SelectNodes("FeeLine").Item(0).InnerText;
        string attribute2 = documentElement.GetAttribute("LastModifiedBy");
        string dateTime = documentElement.GetAttribute("LastModifiedDateTime") == "" ? "" : Utils.ParseUTCDateTime(documentElement.GetAttribute("LastModifiedDateTime")).ToString("G");
        if (exEntities.RuleAlreadyExists.Equals("True"))
        {
          int num1 = (int) Utils.Dialog((IWin32Window) this, string.Format("{0} already exists. Please rename the rule and try importing it again", (object) attribute1), MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else if (exEntities.FeeRuleLineAlreadyExist != null && exEntities.FeeRuleLineAlreadyExist.Equals("True"))
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, string.Format("Fee rule for {0} already exists. Please delete the existing rule for this fee line to import the new rule. Alternatively, you can add additional scenarios to the existing rule.", (object) innerText), MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          DDMRuleValidationResult ui = new DDMRuleValidator(this.session.SessionObjects, BizRuleType.DDMFeeRules, parsedXml, exEntities).mapExEntitiesToUI();
          if (new DDMRuleImportInfoDlg(ImportType.FeeRule, attribute1, innerText, dateTime, attribute2, ui).ShowDialog() != DialogResult.OK)
            return;
          string input = DDMRestApiHelper.ImportFeeRule(parsedXml.OuterXml);
          ImportResultResponse importResultResponse = scriptSerializer.Deserialize<ImportResultResponse>(input);
          if (!string.IsNullOrEmpty(importResultResponse.RuleId))
          {
            int num3 = (int) Utils.Dialog((IWin32Window) this, string.Format("{0} has been successfully imported.", (object) attribute1), MessageBoxButtons.OK, MessageBoxIcon.None);
            this.session.BPM.GetBpmManager(BizRuleType.DDMFeeScenarios).BPMManager.InvalidateCache(BizRuleType.DDMFeeScenarios);
            this.refresh();
          }
          else
          {
            int num4 = (int) Utils.Dialog((IWin32Window) this, "Import failed: " + importResultResponse.ErrorMessage, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Fee rule import fails: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }
  }
}
