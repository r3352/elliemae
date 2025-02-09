// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Conditions.AddEnhancedConditionsDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder.ThinThick;
using EllieMae.EMLite.LoanUtils.EnhancedConditions;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.eFolder.Conditions
{
  public class AddEnhancedConditionsDialog : Form
  {
    private const string className = "AddEnhancedConditionsDialog";
    private static readonly string sw = Tracing.SwEFolder;
    private LoanDataMgr loanDataMgr;
    private eFolderAccessRights rights;
    private EnhancedConditionTemplate[] conditionTemplates;
    private EnhancedConditionType[] condTypes;
    private Dictionary<string, ConditionDefinitionContract> condContractDict = new Dictionary<string, ConditionDefinitionContract>();
    private DocumentLog[] docList;
    private bool canReviewAndImport;
    private bool canImportAll;
    private List<EnhancedConditionSet> condSets = new List<EnhancedConditionSet>();
    private List<EnhancedConditionLog> conditionsAddedList = new List<EnhancedConditionLog>();
    private string[] existingConditionKeysInLoan;
    private bool displayingInvestorDeliveryDialog;
    private bool isAdHocCondition;
    private string newConditionGUID;
    private EnhancedConditionLog newCondition;
    private Task<AddEnhancedConditionsDialog.AddAutomatedEnhancedConditionResult> addAutomatedCondtionsTask;
    private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
    private IContainer components;
    private Button btnOK;
    private Button btnCancel;
    private RadioButton rdoSet;
    private RadioButton rdoList;
    private Label lblFrom;
    private RadioButton rdoAuto;
    private Label lblImportFrom;
    private RadioButton rdoDelivery;
    private Panel pnlImportInvestorDelivery;
    private RadioButton rdoImportAll;
    private RadioButton rdoReviewAndImport;
    private RadioButton rdoBlank;
    private ProgressBar progressBar;
    private BackgroundWorker bgWorker;
    private Panel panelAddAutomatedConditions;
    private Label label2;
    private RadioButton rdoLpa;
    private RadioButton rdoDu;
    private RadioButton rdoEarlyCheck;

    public bool IsAdHocCondition => this.isAdHocCondition;

    public string NewConditionGUID => this.newConditionGUID;

    public EnhancedConditionLog NewCondition => this.newCondition;

    public AddEnhancedConditionsDialog(
      LoanDataMgr loanDataMgr,
      EnhancedConditionTemplate[] conditionTemplates,
      EnhancedConditionType[] condTypes)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
      this.conditionTemplates = conditionTemplates;
      this.condTypes = condTypes;
      this.condSets = Session.ConfigurationManager.GetEnhancedConditionSets();
      this.rdoSet.Enabled = this.condSets.Count > 0;
      this.rights = new eFolderAccessRights(this.loanDataMgr);
      this.rdoAuto.Enabled = this.enableAddAutomateCondition();
      this.rdoBlank.Enabled = this.enableAddBlankCondition();
      this.canReviewAndImport = this.rights.CanReviewAndImportEnhancedConditions();
      this.canImportAll = this.rights.CanImportAllEnhancedConditions();
      if (this.canReviewAndImport || this.canImportAll)
        return;
      this.lblImportFrom.Visible = false;
      this.rdoDelivery.Visible = false;
      this.pnlImportInvestorDelivery.Visible = false;
    }

    public AddEnhancedConditionsDialog(
      LoanDataMgr loanDataMgr,
      EnhancedConditionTemplate[] conditionTemplates,
      EnhancedConditionType[] condTypes,
      string[] conditionKeysInLoan)
      : this(loanDataMgr, conditionTemplates, condTypes)
    {
      this.existingConditionKeysInLoan = conditionKeysInLoan;
    }

    public ConditionLog[] Conditions => (ConditionLog[]) this.conditionsAddedList.ToArray();

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.rdoList.Checked)
        this.addConditionsFromList();
      else if (this.rdoSet.Checked)
        this.addConditionsFromSet();
      else if (this.rdoBlank.Checked)
        this.addblankCondition();
      else if (this.rdoAuto.Checked)
        this.addAutomatedConditions();
      else if (this.rdoDu.Checked)
        this.LaunchEvpConditionsDU();
      else if (this.rdoLpa.Checked)
        this.LaunchEvpConditionsLPA();
      else if (this.rdoEarlyCheck.Checked)
        this.LaunchEvpConditionsEarlyCheck();
      else if (this.rdoReviewAndImport.Checked)
      {
        this.reviewAndImportConditions();
      }
      else
      {
        if (!this.rdoImportAll.Checked)
          return;
        this.importAllConditions();
      }
    }

    private void LaunchEvpConditionsLPA()
    {
      IEPass service = Session.Application.GetService<IEPass>();
      if (service == null)
        return;
      string url = string.Format("{0};LPA;LPA+Conditions", (object) this.RetrieveConfigurationXml());
      service.ProcessURL(url);
    }

    private void LaunchEvpConditionsDU()
    {
      IEPass service = Session.Application.GetService<IEPass>();
      if (service == null)
        return;
      string url = string.Format("{0};DU;DU+Conditions", (object) this.RetrieveConfigurationXml());
      service.ProcessURL(url);
    }

    private void LaunchEvpConditionsEarlyCheck()
    {
      IEPass service = Session.Application.GetService<IEPass>();
      if (service == null)
        return;
      string url = string.Format("{0};EC;EC+Conditions", (object) this.RetrieveConfigurationXml());
      service.ProcessURL(url);
    }

    private string RetrieveConfigurationXml()
    {
      string path3 = "ImportConditions.CONFIG";
      string str1 = "Prod";
      string str2 = "EVP_SSFBOOTSTRAP_BAMDOTNETBROWSER";
      string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ePASS", path3);
      try
      {
        if (File.Exists(path))
        {
          string xml = File.ReadAllText(path);
          XmlDocument xmlDocument = new XmlDocument();
          xmlDocument.LoadXml(xml);
          XmlNode xmlNode = xmlDocument.SelectSingleNode("CONFIG/HOST");
          if (xmlNode != null)
          {
            if (xmlNode?.Attributes?["Environment"] != null)
            {
              if (!string.IsNullOrEmpty(xmlNode.Attributes["Environment"].Value))
                str1 = xmlNode.Attributes["Environment"].Value;
            }
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(AddEnhancedConditionsDialog.sw, nameof (AddEnhancedConditionsDialog), TraceLevel.Warning, "Import Conditions retrieve configure fails couldn't be loaded");
        str1 = "Prod";
      }
      return string.Format("https://ice.com/_EPASS_SIGNATURE;{0};{1};10000050;AUS-ImportConditions;VPDM.AUS.ImportFindings;22009001;1;none;990x650;importFindings", (object) str2, (object) str1);
    }

    private void addConditionsFromList()
    {
      using (AddEnhancedConditionsFromListDialog conditionsFromListDialog = new AddEnhancedConditionsFromListDialog(this.loanDataMgr, this.conditionTemplates, this.condTypes))
      {
        DialogResult dialogResult = conditionsFromListDialog.ShowDialog((IWin32Window) Form.ActiveForm);
        if (dialogResult != DialogResult.OK)
          return;
        this.addConditions(conditionsFromListDialog.TemplatesToAdd, conditionsFromListDialog.PairId, new SourceOfCondition?(SourceOfCondition.ConditionList));
        this.DialogResult = dialogResult;
      }
    }

    private void addConditionsFromSet()
    {
      using (EnhancedConditionSetsDialog conditionSetsDialog = new EnhancedConditionSetsDialog(this.loanDataMgr, this.condSets, this.conditionTemplates))
      {
        DialogResult dialogResult = conditionSetsDialog.ShowDialog((IWin32Window) this);
        if (dialogResult != DialogResult.OK)
          return;
        this.addConditions(conditionSetsDialog.TemplatesToAdd, conditionSetsDialog.PairId, new SourceOfCondition?(SourceOfCondition.ConditionList));
        this.DialogResult = dialogResult;
      }
    }

    private bool enableAddBlankCondition()
    {
      foreach (EnhancedConditionType condType in this.condTypes)
      {
        if (this.rights.CanAddBlankEnhancedCondition(condType.title))
          return true;
      }
      return false;
    }

    private void addblankCondition()
    {
      this.newCondition = (EnhancedConditionLog) null;
      this.newConditionGUID = (string) null;
      List<EnhancedConditionType> enhancedConditionTypeList = new List<EnhancedConditionType>();
      foreach (EnhancedConditionType condType in this.condTypes)
      {
        if (this.rights.CanAddBlankEnhancedCondition(condType.title))
          enhancedConditionTypeList.Add(condType);
      }
      using (AddBlankEnhancedConditionDialog enhancedConditionDialog = new AddBlankEnhancedConditionDialog(this.loanDataMgr, this.conditionTemplates, enhancedConditionTypeList.ToArray()))
      {
        DialogResult dialogResult = enhancedConditionDialog.ShowDialog((IWin32Window) Form.ActiveForm);
        if (dialogResult != DialogResult.OK)
          return;
        this.addConditions(enhancedConditionDialog.TemplatesToAdd, enhancedConditionDialog.PairId, new SourceOfCondition?(SourceOfCondition.Manual));
        if (this.newCondition != null)
        {
          this.newConditionGUID = this.newCondition.Guid;
          if (!enhancedConditionDialog.OpenEditDetailDialog)
            this.newCondition = (EnhancedConditionLog) null;
        }
        this.isAdHocCondition = enhancedConditionDialog.IsAdHocCondition;
        this.DialogResult = dialogResult;
      }
    }

    private bool enableAddAutomateCondition()
    {
      foreach (EnhancedConditionType condType in this.condTypes)
      {
        if (this.rights.CanAddAutomatedEnhancedCondition(condType.title))
          return true;
      }
      return false;
    }

    private void addAutomatedConditions()
    {
      this.panelAddAutomatedConditions.Visible = true;
      this.addAutomatedCondtionsTask = Task.Run<AddEnhancedConditionsDialog.AddAutomatedEnhancedConditionResult>((Func<AddEnhancedConditionsDialog.AddAutomatedEnhancedConditionResult>) (() => this.addAutomatedConditionAsync()), this.cancellationTokenSource.Token);
      AddEnhancedConditionsDialog.AddAutomatedEnhancedConditionResult result = this.addAutomatedCondtionsTask.Result;
      if (result.Errors.Count > 0)
      {
        StringBuilder errorMessageBuilder = new StringBuilder();
        result.Errors.ForEach((Action<string>) (e => errorMessageBuilder.AppendLine(e)));
        int num = (int) Utils.Dialog((IWin32Window) this, errorMessageBuilder.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (result.AutoAddConditions.Count > 0)
          this.addConditions(result.AutoAddConditions.ToArray(), "All", new SourceOfCondition?(SourceOfCondition.AutomatedByUser));
        if (result.DuplicateConditions.Count > 0)
        {
          using (SelectDuplicateAutomatedEnhancedConditonDialog enhancedConditonDialog = new SelectDuplicateAutomatedEnhancedConditonDialog(this.loanDataMgr, result.DuplicateConditions.ToArray(), result.AutoAddConditions.Count))
          {
            DialogResult dialogResult = enhancedConditonDialog.ShowDialog((IWin32Window) Form.ActiveForm);
            if (dialogResult == DialogResult.OK)
            {
              this.addConditions(enhancedConditonDialog.TemplatesToAdd, enhancedConditonDialog.PairId, new SourceOfCondition?(SourceOfCondition.AutomatedByUser));
              this.DialogResult = dialogResult;
            }
          }
        }
      }
      this.panelAddAutomatedConditions.Visible = false;
      this.Close();
    }

    private void reviewAndImportConditions()
    {
      if (this.displayingInvestorDeliveryDialog)
        return;
      try
      {
        this.displayingInvestorDeliveryDialog = true;
        new eFolderManager().LaunchInvestorDeliveryConditions(this.loanDataMgr, ThinThickType.ReviewAndImport);
        this.DialogResult = DialogResult.OK;
      }
      finally
      {
        this.displayingInvestorDeliveryDialog = false;
      }
    }

    private void importAllConditions()
    {
      if (this.displayingInvestorDeliveryDialog)
        return;
      try
      {
        this.displayingInvestorDeliveryDialog = true;
        new eFolderManager().LaunchInvestorDeliveryConditions(this.loanDataMgr, ThinThickType.ImportAll);
        this.DialogResult = DialogResult.OK;
      }
      finally
      {
        this.displayingInvestorDeliveryDialog = false;
      }
    }

    private void rdoDelivery_CheckedChanged(object sender, EventArgs e)
    {
      this.rdoReviewAndImport.Enabled = this.rdoDelivery.Checked && this.canReviewAndImport;
      this.rdoImportAll.Enabled = this.rdoDelivery.Checked && this.canImportAll;
    }

    private void addConditions(
      EnhancedConditionTemplate[] conditionTemplates,
      string pairId,
      SourceOfCondition? sourceOfCondition)
    {
      using (CursorActivator.Wait())
      {
        LogList logList = this.loanDataMgr.LoanData.GetLogList();
        foreach (EnhancedConditionTemplate conditionTemplate in conditionTemplates)
        {
          DateTime result1 = DateTime.MinValue;
          DateTime result2 = DateTime.MinValue;
          if (!string.IsNullOrEmpty(conditionTemplate.StartDate) && DateTime.TryParse(conditionTemplate.StartDate, out result1) && DateTime.Compare(result1, DateTime.Now.Date) > 0)
            Tracing.Log(AddEnhancedConditionsDialog.sw, nameof (AddEnhancedConditionsDialog), TraceLevel.Verbose, "Template.StartDate " + conditionTemplate.StartDate + " out of range. Condition not added.");
          else if (!string.IsNullOrEmpty(conditionTemplate.EndDate) && DateTime.TryParse(conditionTemplate.EndDate, out result2) && DateTime.Compare(result2, DateTime.Now.Date) < 0)
          {
            Tracing.Log(AddEnhancedConditionsDialog.sw, nameof (AddEnhancedConditionsDialog), TraceLevel.Verbose, "Template.EndDate " + conditionTemplate.EndDate + " out of range. Condition not added.");
          }
          else
          {
            EnhancedConditionLog enhancedConditionLog = new EnhancedConditionLog(conditionTemplate.ConditionType, conditionTemplate.Title, Session.UserID, pairId, this.getEnhancedConditionDefinition(conditionTemplate), (StatusTrackingList) null);
            logList.AddRecord((LogRecordBase) enhancedConditionLog);
            enhancedConditionLog.InternalDescription = conditionTemplate.InternalDescription;
            enhancedConditionLog.ExternalDescription = conditionTemplate.ExternalDescription;
            enhancedConditionLog.Source = conditionTemplate.Source;
            enhancedConditionLog.Recipient = conditionTemplate.Recipient;
            enhancedConditionLog.PriorTo = conditionTemplate.PriorTo;
            enhancedConditionLog.Category = conditionTemplate.Category;
            if (conditionTemplate.Owner != null)
              enhancedConditionLog.Owner = new int?(Convert.ToInt32(conditionTemplate.Owner.entityId));
            enhancedConditionLog.InternalId = conditionTemplate.InternalId;
            enhancedConditionLog.ExternalId = conditionTemplate.ExternalId;
            enhancedConditionLog.InternalPrint = new bool?(conditionTemplate.IsInternalPrint);
            enhancedConditionLog.ExternalPrint = new bool?(conditionTemplate.IsExternalPrint);
            enhancedConditionLog.DaysToReceive = conditionTemplate.DaysToReceive;
            enhancedConditionLog.SourceOfCondition = sourceOfCondition;
            if (result1 != DateTime.MinValue)
              enhancedConditionLog.StartDate = new DateTime?(result1);
            if (result2 != DateTime.MinValue)
              enhancedConditionLog.EndDate = new DateTime?(result2);
            this.assignDocuments(conditionTemplate, logList, enhancedConditionLog);
            this.conditionsAddedList.Add(enhancedConditionLog);
          }
        }
        SourceOfCondition? nullable = sourceOfCondition;
        SourceOfCondition sourceOfCondition1 = SourceOfCondition.Manual;
        if (!(nullable.GetValueOrDefault() == sourceOfCondition1 & nullable.HasValue))
        {
          nullable = sourceOfCondition;
          SourceOfCondition sourceOfCondition2 = SourceOfCondition.ConditionList;
          if (!(nullable.GetValueOrDefault() == sourceOfCondition2 & nullable.HasValue))
            return;
        }
        if (this.conditionsAddedList == null || this.conditionsAddedList.Count != 1)
          return;
        this.newCondition = this.conditionsAddedList[0];
      }
    }

    private void assignDocuments(
      EnhancedConditionTemplate template,
      LogList logList,
      EnhancedConditionLog cond)
    {
      if (template.AssignedTo == null)
        return;
      DocumentTrackingSetup documentTrackingSetup = Session.ConfigurationManager.GetDocumentTrackingSetup();
      if (this.docList == null)
        this.docList = logList.GetAllDocuments();
      foreach (EntityReferenceContract referenceContract in template.AssignedTo)
        this.assignDocumentToCondition(documentTrackingSetup.GetByID(referenceContract.entityId), this.docList, cond);
    }

    private void assignDocumentToCondition(
      DocumentTemplate docTemplate,
      DocumentLog[] docList,
      EnhancedConditionLog cond)
    {
      if (docTemplate == null)
        return;
      foreach (DocumentLog doc in docList)
      {
        if (doc.Title == docTemplate.Name)
          doc.Conditions.Add((ConditionLog) cond);
      }
    }

    private EnhancedConditionDefinition getEnhancedConditionDefinition(
      EnhancedConditionTemplate template)
    {
      ConditionDefinitionContract definitionContract = (ConditionDefinitionContract) null;
      if (template.CustomizeTypeDefinition.Value)
      {
        definitionContract = template.customDefinitions;
      }
      else
      {
        foreach (EnhancedConditionType condType in this.condTypes)
        {
          if (condType.title == template.ConditionType)
          {
            definitionContract = condType.definitions != null ? condType.definitions : this.getDefinitionContract(condType.id);
            break;
          }
        }
      }
      if (definitionContract == null)
        return (EnhancedConditionDefinition) null;
      IList<OptionDefinition> categoryDefinitions = (IList<OptionDefinition>) null;
      IList<OptionDefinition> priorToDefinitions = (IList<OptionDefinition>) null;
      IList<OptionDefinition> recipientDefinitions = (IList<OptionDefinition>) null;
      IList<OptionDefinition> sourceDefinitions = (IList<OptionDefinition>) null;
      IList<StatusTrackingDefinition> trackingDefinitions = (IList<StatusTrackingDefinition>) null;
      if (definitionContract.categoryDefinitions != null)
      {
        categoryDefinitions = (IList<OptionDefinition>) new List<OptionDefinition>();
        foreach (OptionDefinitionContract categoryDefinition in definitionContract.categoryDefinitions)
          categoryDefinitions.Add(new OptionDefinition(categoryDefinition.name));
      }
      if (definitionContract.priorToDefinitions != null)
      {
        priorToDefinitions = (IList<OptionDefinition>) new List<OptionDefinition>();
        foreach (OptionDefinitionContract priorToDefinition in definitionContract.priorToDefinitions)
          priorToDefinitions.Add(new OptionDefinition(priorToDefinition.name));
      }
      if (definitionContract.recipientDefinitions != null)
      {
        recipientDefinitions = (IList<OptionDefinition>) new List<OptionDefinition>();
        foreach (OptionDefinitionContract recipientDefinition in definitionContract.recipientDefinitions)
          recipientDefinitions.Add(new OptionDefinition(recipientDefinition.name));
      }
      if (definitionContract.sourceDefinitions != null)
      {
        sourceDefinitions = (IList<OptionDefinition>) new List<OptionDefinition>();
        foreach (OptionDefinitionContract sourceDefinition in definitionContract.sourceDefinitions)
          sourceDefinitions.Add(new OptionDefinition(sourceDefinition.name));
      }
      if (definitionContract.trackingDefinitions != null)
      {
        trackingDefinitions = (IList<StatusTrackingDefinition>) new List<StatusTrackingDefinition>();
        foreach (TrackingDefinitionContract trackingDefinition in definitionContract.trackingDefinitions)
        {
          if (trackingDefinition.roles != null)
          {
            List<int> intList = new List<int>();
            foreach (EntityReferenceContract role in trackingDefinition.roles)
              intList.Add(Convert.ToInt32(role.entityId));
            trackingDefinitions.Add(new StatusTrackingDefinition(trackingDefinition.name, trackingDefinition.open, intList.ToArray()));
          }
        }
      }
      return new EnhancedConditionDefinition(categoryDefinitions, priorToDefinitions, recipientDefinitions, sourceDefinitions, trackingDefinitions);
    }

    private ConditionDefinitionContract getDefinitionContract(string condTypeId)
    {
      if (this.condContractDict.ContainsKey(condTypeId))
        return this.condContractDict[condTypeId];
      EnhancedConditionType conditionTypeDetails = new EnhancedConditionsRestClient(this.loanDataMgr).GetEnhancedConditionTypeDetails(condTypeId);
      ConditionDefinitionContract definitionContract = (ConditionDefinitionContract) null;
      if (conditionTypeDetails != null)
        definitionContract = conditionTypeDetails.definitions;
      this.condContractDict.Add(condTypeId, definitionContract);
      return definitionContract;
    }

    private AddEnhancedConditionsDialog.AddAutomatedEnhancedConditionResult addAutomatedConditionAsync()
    {
      AddEnhancedConditionsDialog.AddAutomatedEnhancedConditionResult enhancedConditionResult = new AddEnhancedConditionsDialog.AddAutomatedEnhancedConditionResult();
      try
      {
        AutomatedEnhancedConditionBpmManager bpmManager = Session.BPM.GetBpmManager(BpmCategory.AutomatedEnhancedConditions) as AutomatedEnhancedConditionBpmManager;
        Dictionary<string, EnhancedConditionTemplate> dictionary1 = ((IEnumerable<EnhancedConditionTemplate>) this.conditionTemplates).ToDictionary<EnhancedConditionTemplate, string>((Func<EnhancedConditionTemplate, string>) (t => t.UniqueKey));
        Dictionary<string, string> dictionary2 = ((IEnumerable<string>) this.existingConditionKeysInLoan).ToDictionary<string, string>((Func<string, string>) (c => c));
        LoanData loanData = this.loanDataMgr.LoanData;
        LoanConditions loanConditions = new LoanBusinessRuleInfo(loanData).CurrentLoanForBusinessRule();
        if (loanConditions == null)
        {
          enhancedConditionResult.Errors.Add("No automated conditions rules matched.");
          return enhancedConditionResult;
        }
        foreach (string condition in bpmManager.GetConditions(loanConditions, loanData))
        {
          if (dictionary1.ContainsKey(condition))
          {
            EnhancedConditionTemplate conditionTemplate = dictionary1[condition];
            bool? nullable = conditionTemplate.Active;
            if (((int) nullable ?? 0) != 0)
            {
              DateTime now;
              if (!string.IsNullOrEmpty(conditionTemplate.StartDate))
              {
                DateTime t1 = DateTime.Parse(conditionTemplate.StartDate);
                now = DateTime.Now;
                DateTime date = now.Date;
                if (DateTime.Compare(t1, date) > 0)
                  continue;
              }
              if (!string.IsNullOrEmpty(conditionTemplate.EndDate))
              {
                DateTime t1 = DateTime.Parse(conditionTemplate.EndDate);
                now = DateTime.Now;
                DateTime date = now.Date;
                if (DateTime.Compare(t1, date) < 0)
                  continue;
              }
              if (this.rights.CanAddAutomatedEnhancedCondition(conditionTemplate.ConditionType))
              {
                if (!dictionary2.ContainsKey(condition))
                {
                  enhancedConditionResult.AutoAddConditions.Add(conditionTemplate);
                }
                else
                {
                  nullable = conditionTemplate.AllowDuplicate;
                  if (((int) nullable ?? 0) != 0)
                    enhancedConditionResult.DuplicateConditions.Add(conditionTemplate);
                }
              }
            }
          }
        }
        if (enhancedConditionResult.AutoAddConditions.Count == 0 && enhancedConditionResult.DuplicateConditions.Count == 0)
          enhancedConditionResult.Errors.Add("No automated conditions rules matched.");
        return enhancedConditionResult;
      }
      catch (Exception ex)
      {
        enhancedConditionResult.Errors.Add("Couldn't add automated conditions: " + ex.Message);
        return enhancedConditionResult;
      }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.verifyAndAbortAddAutomatedConditions();
    }

    private void verifyAndAbortAddAutomatedConditions()
    {
      if (this.addAutomatedCondtionsTask == null || this.addAutomatedCondtionsTask.Status == TaskStatus.RanToCompletion)
        return;
      this.cancellationTokenSource.Cancel();
    }

    private void AddEnhancedConditionsDialog_Load(object sender, EventArgs e)
    {
      this.verifyAndAbortAddAutomatedConditions();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.rdoSet = new RadioButton();
      this.rdoList = new RadioButton();
      this.lblFrom = new Label();
      this.rdoAuto = new RadioButton();
      this.lblImportFrom = new Label();
      this.rdoDelivery = new RadioButton();
      this.pnlImportInvestorDelivery = new Panel();
      this.rdoImportAll = new RadioButton();
      this.rdoReviewAndImport = new RadioButton();
      this.rdoBlank = new RadioButton();
      this.progressBar = new ProgressBar();
      this.bgWorker = new BackgroundWorker();
      this.panelAddAutomatedConditions = new Panel();
      this.label2 = new Label();
      this.rdoLpa = new RadioButton();
      this.rdoDu = new RadioButton();
      this.rdoEarlyCheck = new RadioButton();
      this.pnlImportInvestorDelivery.SuspendLayout();
      this.panelAddAutomatedConditions.SuspendLayout();
      this.SuspendLayout();
      this.btnOK.Location = new Point(86, 314);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 22);
      this.btnOK.TabIndex = 10;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(167, 314);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 11;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.rdoSet.AutoSize = true;
      this.rdoSet.Location = new Point(21, 62);
      this.rdoSet.Name = "rdoSet";
      this.rdoSet.Size = new Size(88, 18);
      this.rdoSet.TabIndex = 1;
      this.rdoSet.TabStop = true;
      this.rdoSet.Text = "Condition Set";
      this.rdoSet.UseVisualStyleBackColor = true;
      this.rdoList.AutoSize = true;
      this.rdoList.Checked = true;
      this.rdoList.Location = new Point(21, 38);
      this.rdoList.Name = "rdoList";
      this.rdoList.Size = new Size(95, 18);
      this.rdoList.TabIndex = 0;
      this.rdoList.TabStop = true;
      this.rdoList.Text = "Conditions List";
      this.rdoList.UseVisualStyleBackColor = true;
      this.lblFrom.AutoSize = true;
      this.lblFrom.Location = new Point(9, 11);
      this.lblFrom.Margin = new Padding(10);
      this.lblFrom.Name = "lblFrom";
      this.lblFrom.Size = new Size(54, 14);
      this.lblFrom.TabIndex = 8;
      this.lblFrom.Text = "Add From";
      this.rdoAuto.AutoSize = true;
      this.rdoAuto.Location = new Point(21, 86);
      this.rdoAuto.Name = "rdoAuto";
      this.rdoAuto.Size = new Size(130, 18);
      this.rdoAuto.TabIndex = 2;
      this.rdoAuto.TabStop = true;
      this.rdoAuto.Text = "Automated Conditions";
      this.rdoAuto.UseVisualStyleBackColor = true;
      this.lblImportFrom.AutoSize = true;
      this.lblImportFrom.Location = new Point(9, 141);
      this.lblImportFrom.Margin = new Padding(10);
      this.lblImportFrom.Name = "lblImportFrom";
      this.lblImportFrom.Size = new Size(63, 14);
      this.lblImportFrom.TabIndex = 10;
      this.lblImportFrom.Text = "Import From";
      this.rdoDelivery.AutoSize = true;
      this.rdoDelivery.Location = new Point(21, 244);
      this.rdoDelivery.Name = "rdoDelivery";
      this.rdoDelivery.Size = new Size(159, 18);
      this.rdoDelivery.TabIndex = 7;
      this.rdoDelivery.TabStop = true;
      this.rdoDelivery.Text = "Investor Delivery Conditions";
      this.rdoDelivery.UseVisualStyleBackColor = true;
      this.rdoDelivery.CheckedChanged += new EventHandler(this.rdoDelivery_CheckedChanged);
      this.pnlImportInvestorDelivery.Controls.Add((Control) this.rdoImportAll);
      this.pnlImportInvestorDelivery.Controls.Add((Control) this.rdoReviewAndImport);
      this.pnlImportInvestorDelivery.Location = new Point(35, 272);
      this.pnlImportInvestorDelivery.Name = "pnlImportInvestorDelivery";
      this.pnlImportInvestorDelivery.Size = new Size(194, 24);
      this.pnlImportInvestorDelivery.TabIndex = 25;
      this.rdoImportAll.AutoSize = true;
      this.rdoImportAll.Checked = true;
      this.rdoImportAll.Enabled = false;
      this.rdoImportAll.Location = new Point(3, 4);
      this.rdoImportAll.Name = "rdoImportAll";
      this.rdoImportAll.Size = new Size(68, 18);
      this.rdoImportAll.TabIndex = 8;
      this.rdoImportAll.TabStop = true;
      this.rdoImportAll.Text = "Import All";
      this.rdoImportAll.UseMnemonic = false;
      this.rdoImportAll.UseVisualStyleBackColor = true;
      this.rdoReviewAndImport.AutoSize = true;
      this.rdoReviewAndImport.Enabled = false;
      this.rdoReviewAndImport.Location = new Point(77, 4);
      this.rdoReviewAndImport.Name = "rdoReviewAndImport";
      this.rdoReviewAndImport.Size = new Size(115, 18);
      this.rdoReviewAndImport.TabIndex = 9;
      this.rdoReviewAndImport.TabStop = true;
      this.rdoReviewAndImport.Text = "Review and Import";
      this.rdoReviewAndImport.UseVisualStyleBackColor = true;
      this.rdoBlank.AutoSize = true;
      this.rdoBlank.Location = new Point(21, 110);
      this.rdoBlank.Name = "rdoBlank";
      this.rdoBlank.Size = new Size(98, 18);
      this.rdoBlank.TabIndex = 3;
      this.rdoBlank.TabStop = true;
      this.rdoBlank.Text = "Blank Condition";
      this.rdoBlank.UseVisualStyleBackColor = true;
      this.progressBar.Location = new Point(6, 25);
      this.progressBar.Name = "progressBar";
      this.progressBar.Size = new Size(230, 23);
      this.progressBar.Style = ProgressBarStyle.Marquee;
      this.progressBar.TabIndex = 27;
      this.panelAddAutomatedConditions.Controls.Add((Control) this.label2);
      this.panelAddAutomatedConditions.Controls.Add((Control) this.progressBar);
      this.panelAddAutomatedConditions.Location = new Point(12, 342);
      this.panelAddAutomatedConditions.Name = "panelAddAutomatedConditions";
      this.panelAddAutomatedConditions.Size = new Size(239, 51);
      this.panelAddAutomatedConditions.TabIndex = 28;
      this.panelAddAutomatedConditions.Visible = false;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(3, 8);
      this.label2.Name = "label2";
      this.label2.Size = new Size(208, 14);
      this.label2.TabIndex = 28;
      this.label2.Text = "Adding automated conditions to the loan...";
      this.rdoLpa.AutoSize = true;
      this.rdoLpa.Location = new Point(21, 192);
      this.rdoLpa.Name = "rdoLpa";
      this.rdoLpa.Size = new Size(145, 18);
      this.rdoLpa.TabIndex = 5;
      this.rdoLpa.TabStop = true;
      this.rdoLpa.Text = "LPA Feedback Certificate";
      this.rdoLpa.UseVisualStyleBackColor = true;
      this.rdoDu.AutoSize = true;
      this.rdoDu.Location = new Point(21, 166);
      this.rdoDu.Name = "rdoDu";
      this.rdoDu.Size = new Size(82, 18);
      this.rdoDu.TabIndex = 4;
      this.rdoDu.TabStop = true;
      this.rdoDu.Text = "DU Findings";
      this.rdoDu.UseVisualStyleBackColor = true;
      this.rdoEarlyCheck.AutoSize = true;
      this.rdoEarlyCheck.Location = new Point(21, 218);
      this.rdoEarlyCheck.Name = "rdoEarlyCheck";
      this.rdoEarlyCheck.Size = new Size(79, 18);
      this.rdoEarlyCheck.TabIndex = 6;
      this.rdoEarlyCheck.TabStop = true;
      this.rdoEarlyCheck.Text = "EarlyCheck";
      this.rdoEarlyCheck.UseVisualStyleBackColor = true;
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(263, 421);
      this.Controls.Add((Control) this.rdoEarlyCheck);
      this.Controls.Add((Control) this.rdoDu);
      this.Controls.Add((Control) this.rdoLpa);
      this.Controls.Add((Control) this.panelAddAutomatedConditions);
      this.Controls.Add((Control) this.rdoBlank);
      this.Controls.Add((Control) this.pnlImportInvestorDelivery);
      this.Controls.Add((Control) this.rdoDelivery);
      this.Controls.Add((Control) this.lblImportFrom);
      this.Controls.Add((Control) this.rdoAuto);
      this.Controls.Add((Control) this.lblFrom);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.rdoSet);
      this.Controls.Add((Control) this.rdoList);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddEnhancedConditionsDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add Condition";
      this.Load += new EventHandler(this.AddEnhancedConditionsDialog_Load);
      this.pnlImportInvestorDelivery.ResumeLayout(false);
      this.pnlImportInvestorDelivery.PerformLayout();
      this.panelAddAutomatedConditions.ResumeLayout(false);
      this.panelAddAutomatedConditions.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private class AddAutomatedEnhancedConditionResult
    {
      public AddAutomatedEnhancedConditionResult()
      {
        this.AutoAddConditions = new List<EnhancedConditionTemplate>();
        this.DuplicateConditions = new List<EnhancedConditionTemplate>();
        this.Errors = new List<string>();
      }

      public List<EnhancedConditionTemplate> AutoAddConditions { get; set; }

      public List<EnhancedConditionTemplate> DuplicateConditions { get; set; }

      public List<string> Errors { get; set; }
    }
  }
}
