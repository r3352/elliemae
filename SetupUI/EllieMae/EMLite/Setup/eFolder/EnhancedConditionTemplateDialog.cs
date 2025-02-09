// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.eFolder.EnhancedConditionTemplateDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.eFolder
{
  public class EnhancedConditionTemplateDialog : Form
  {
    private const string className = "EnhancedConditionTemplateDialog";
    private DocumentTrackingSetup docSetup;
    private DocumentTemplate[] docList;
    private string[] selectedDocList;
    private Sessions.Session session;
    private DocumentTemplate tpoDefaultDoc;
    private bool isFormChanged;
    private EnhancedConditionTemplate enhancedConditionTemplate;
    private bool isEdit;
    private bool isDuplicate;
    private bool hasPersonaEditPermission;
    private string defaultDropDownValue = "Please select";
    private bool isCustomizedTypeDefinition;
    private ConditionDefinitionContract customizedTypeDefinitions;
    private RoleInfo[] owners;
    private const string investorType = "investor delivery";
    private string currentSelectedType = string.Empty;
    private IContainer components;
    private GroupBox groupBoxMain;
    private Button btnSave;
    private Button btnCancel;
    private EMHelpLink helpLinkDocuments;
    private ComboBox cmbConditionType;
    private Label label4;
    private Label lblExternalID;
    private Label lblInternalID;
    private Label lblType;
    private TextBox txtInternalId;
    private TextBox txtConditionStatus;
    private TextBox txtExternalId;
    private TextBox txtName;
    private Label lblName;
    private CheckBox chkAllowDuplicate;
    private Label label6;
    private TextBox txtExternalDesc;
    private Label label7;
    private ListBox lstDocuments;
    private Button button2;
    private CheckBox chkCustomizeCondition;
    private GroupBox groupBox1;
    private TextBox txtInternalDesc;
    private RadioButton rdTpoDocSameName;
    private RadioButton rdTpoDefaultDoc;
    private Button btnSaveTpoDoc;
    private GroupBox grpDates;
    private GroupBox grpCustomizeSettings;
    private ComboBox cmbCategory;
    private Label label11;
    private Label label10;
    private Label label9;
    private Label label8;
    private Label label14;
    private Label label13;
    private Label label12;
    private ComboBox cmbPriorTo;
    private ComboBox cmbRecipient;
    private ComboBox cmbSource;
    private TextBox txtDaysToReceive;
    private Label label16;
    private Label label15;
    private CheckBox chkPrintExternal;
    private CheckBox chkPrintInternal;
    private Label defaultDocLabel;
    private TextBox txtEffectiveStartDate;
    private TextBox txtEffectiveEndDate;
    private Label label52;
    private Label label5;
    private StandardIconButton btnEditCustomizeCondition;
    private CalendarButton calEndDate;
    private CalendarButton calStartDate;
    private ToolTip toolTip1;
    private ComboBox cmbowner;
    private Label lblowner;

    public EnhancedConditionTemplateDialog(
      Sessions.Session currentSession,
      EnhancedConditionTemplate enhancedConditionTemplateref)
    {
      this.InitializeComponent();
      this.session = currentSession;
      this.helpLinkDocuments.AssignSession(this.session);
      this.enhancedConditionTemplate = (EnhancedConditionTemplate) enhancedConditionTemplateref.Clone();
      if (enhancedConditionTemplateref.Id.HasValue && this.enhancedConditionTemplate != null)
        this.enhancedConditionTemplate.Id = enhancedConditionTemplateref.Id;
      if ((ValueType) this.enhancedConditionTemplate.Id != null)
        this.isEdit = true;
      else if (this.enhancedConditionTemplate.Title != null)
      {
        this.isDuplicate = true;
        this.enhancedConditionTemplate.Active = new bool?(false);
      }
      this.hasPersonaEditPermission = ((FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.SettingsTab_AddEditCopyConditions);
      this.populateConditionTypes();
      this.GetAllRoles();
      this.setControlsDefaultVisibility();
      this.initDocumentList();
      this.loadTemplateInfo();
      this.initForm();
    }

    private void initForm()
    {
      this.txtDaysToReceive.Text = Convert.ToString((object) this.enhancedConditionTemplate.DaysToReceive);
      this.chkPrintInternal.Checked = this.enhancedConditionTemplate.IsInternalPrint;
      this.chkPrintExternal.Checked = this.enhancedConditionTemplate.IsExternalPrint;
      if (this.isEdit || this.isDuplicate)
      {
        this.cmbConditionType.SelectedIndex = this.cmbConditionType.FindStringExact(this.enhancedConditionTemplate.ConditionType);
        if ((ValueType) this.enhancedConditionTemplate.Active != null)
        {
          TextBox txtConditionStatus = this.txtConditionStatus;
          bool? active = this.enhancedConditionTemplate.Active;
          bool flag = true;
          string str = active.GetValueOrDefault() == flag & active.HasValue ? EnhancedConditionTemplateDialog.ConditionStatus.Active.ToString() : EnhancedConditionTemplateDialog.ConditionStatus.Inactive.ToString();
          txtConditionStatus.Text = str;
        }
        this.txtInternalId.Text = this.enhancedConditionTemplate.InternalId;
        this.txtExternalId.Text = this.enhancedConditionTemplate.ExternalId;
        if ((ValueType) this.enhancedConditionTemplate.AllowDuplicate != null)
          this.chkAllowDuplicate.Checked = !Convert.ToBoolean((object) this.enhancedConditionTemplate.AllowDuplicate);
        this.txtName.Text = this.enhancedConditionTemplate.Title;
        this.txtInternalDesc.Text = this.enhancedConditionTemplate.InternalDescription;
        this.txtExternalDesc.Text = this.enhancedConditionTemplate.ExternalDescription;
        if (this.enhancedConditionTemplate.AssignedTo != null)
          this.loadDocumentList();
        this.setCustomizeTypeValues(Convert.ToBoolean((object) this.enhancedConditionTemplate.CustomizeTypeDefinition), this.enhancedConditionTemplate.customDefinitions);
        if (this.isCustomizedTypeDefinition)
          this.populateConditionTypeDefinition();
        if (this.enhancedConditionTemplate.Owner != null && this.owners != null && ((IEnumerable<RoleInfo>) this.owners).Count<RoleInfo>() > 0)
          this.cmbowner.SelectedIndex = this.cmbowner.FindStringExact(((IEnumerable<RoleInfo>) this.owners).Where<RoleInfo>((Func<RoleInfo, bool>) (r => r.RoleID == Convert.ToInt32(this.enhancedConditionTemplate.Owner.entityId))).Select<RoleInfo, string>((Func<RoleInfo, string>) (r => r.RoleName)).FirstOrDefault<string>());
        if (!string.IsNullOrEmpty(this.enhancedConditionTemplate.Category))
          this.cmbCategory.SelectedIndex = this.cmbCategory.FindStringExact(Convert.ToString(this.enhancedConditionTemplate.Category));
        if (!string.IsNullOrEmpty(this.enhancedConditionTemplate.Source))
          this.cmbSource.SelectedIndex = this.cmbSource.FindStringExact(Convert.ToString(this.enhancedConditionTemplate.Source));
        if (!string.IsNullOrEmpty(this.enhancedConditionTemplate.PriorTo))
          this.cmbPriorTo.SelectedIndex = this.cmbPriorTo.FindStringExact(Convert.ToString(this.enhancedConditionTemplate.PriorTo));
        if (!string.IsNullOrEmpty(this.enhancedConditionTemplate.Recipient))
          this.cmbRecipient.SelectedIndex = this.cmbRecipient.FindStringExact(Convert.ToString(this.enhancedConditionTemplate.Recipient));
        this.txtEffectiveStartDate.Text = string.IsNullOrEmpty(this.enhancedConditionTemplate.StartDate) ? "" : Convert.ToDateTime(this.enhancedConditionTemplate.StartDate).ToShortDateString();
        this.txtEffectiveEndDate.Text = string.IsNullOrEmpty(this.enhancedConditionTemplate.EndDate) ? "" : Convert.ToDateTime(this.enhancedConditionTemplate.EndDate).ToShortDateString();
        if (this.enhancedConditionTemplate.ConnectSettings == null)
          return;
        this.initTPOCondDocType(this.enhancedConditionTemplate.ConnectSettings);
      }
      else
        this.cmbConditionType.SelectedIndex = 0;
    }

    private void setControlsDefaultVisibility()
    {
      this.rdTpoDocSameName.Checked = true;
      this.toggleTpoSaveDocument();
      this.btnSave.Enabled = false;
      this.isFormChanged = false;
    }

    private void loadTemplateInfo()
    {
      if (this.selectedDocList == null)
      {
        this.docList = new DocumentTemplate[0];
      }
      else
      {
        ArrayList arrayList = new ArrayList();
        foreach (string selectedDoc in this.selectedDocList)
        {
          DocumentTemplate byId = this.docSetup.GetByID(selectedDoc);
          if (byId != null)
            arrayList.Add((object) byId);
        }
        this.docList = (DocumentTemplate[]) arrayList.ToArray(typeof (DocumentTemplate));
      }
    }

    private void loadDocumentList()
    {
      this.lstDocuments.Items.Clear();
      foreach (object doc in this.docList)
        this.lstDocuments.Items.Add(doc);
    }

    private void populateConditionTypes()
    {
      this.cmbConditionType.Items.Clear();
      Func<EnhancedConditionType, bool> predicate = (Func<EnhancedConditionType, bool>) (o => o.title.ToLower() != "investor delivery");
      foreach (object obj in ((IEnumerable<EnhancedConditionType>) EnhancedConditionRestApiHelper.GetEnhancedConditionTypes()).Where<EnhancedConditionType>(predicate).OrderBy<EnhancedConditionType, string>((Func<EnhancedConditionType, string>) (e => e.title)).ToList<EnhancedConditionType>())
        this.cmbConditionType.Items.Add(obj);
      this.cmbConditionType.DisplayMember = "title";
      this.cmbConditionType.ValueMember = "id";
      this.cmbConditionType.Items.Insert(0, (object) "Please select");
    }

    private void populateConditionTypeDefinition()
    {
      string category = this.cmbCategory.Text;
      string priorto = this.cmbPriorTo.Text;
      string recipient = this.cmbRecipient.Text;
      string source = this.cmbSource.Text;
      string owner = this.cmbowner.Text;
      this.cmbCategory.Items.Clear();
      this.cmbPriorTo.Items.Clear();
      this.cmbRecipient.Items.Clear();
      this.cmbSource.Items.Clear();
      if (this.cmbConditionType.SelectedIndex > 0 && !this.cmbConditionType.SelectedText.Equals("Please select"))
      {
        EnhancedConditionType enhancedConditionType;
        if (this.isCustomizedTypeDefinition)
        {
          enhancedConditionType = new EnhancedConditionType()
          {
            definitions = this.customizedTypeDefinitions
          };
          this.setCustomizeTypeControls(new bool?(true), true);
        }
        else
        {
          enhancedConditionType = EnhancedConditionRestApiHelper.GetConditionTypeById(((EnhancedConditionType) this.cmbConditionType.SelectedItem)?.id);
          this.setCustomizeTypeControls(new bool?(true));
        }
        if (enhancedConditionType.definitions == null)
          return;
        if (enhancedConditionType.definitions.categoryDefinitions != null && enhancedConditionType.definitions.categoryDefinitions.Length != 0)
        {
          this.populateOptionDropdown(enhancedConditionType.definitions.categoryDefinitions, this.cmbCategory);
          if (((IEnumerable<OptionDefinitionContract>) enhancedConditionType.definitions.categoryDefinitions).Where<OptionDefinitionContract>((Func<OptionDefinitionContract, bool>) (a => a.name == category)).Count<OptionDefinitionContract>() > 0)
            this.cmbCategory.Text = category;
        }
        if (enhancedConditionType.definitions.priorToDefinitions != null && enhancedConditionType.definitions.priorToDefinitions.Length != 0)
        {
          this.populateOptionDropdown(enhancedConditionType.definitions.priorToDefinitions, this.cmbPriorTo);
          if (((IEnumerable<OptionDefinitionContract>) enhancedConditionType.definitions.priorToDefinitions).Where<OptionDefinitionContract>((Func<OptionDefinitionContract, bool>) (a => a.name == priorto)).Count<OptionDefinitionContract>() > 0)
            this.cmbPriorTo.Text = priorto;
        }
        if (enhancedConditionType.definitions.recipientDefinitions != null && enhancedConditionType.definitions.recipientDefinitions.Length != 0)
        {
          this.populateOptionDropdown(enhancedConditionType.definitions.recipientDefinitions, this.cmbRecipient);
          if (((IEnumerable<OptionDefinitionContract>) enhancedConditionType.definitions.recipientDefinitions).Where<OptionDefinitionContract>((Func<OptionDefinitionContract, bool>) (a => a.name == recipient)).Count<OptionDefinitionContract>() > 0)
            this.cmbRecipient.Text = recipient;
        }
        if (enhancedConditionType.definitions.sourceDefinitions != null && enhancedConditionType.definitions.sourceDefinitions.Length != 0)
        {
          this.populateOptionDropdown(enhancedConditionType.definitions.sourceDefinitions, this.cmbSource);
          if (((IEnumerable<OptionDefinitionContract>) enhancedConditionType.definitions.sourceDefinitions).Where<OptionDefinitionContract>((Func<OptionDefinitionContract, bool>) (a => a.name == source)).Count<OptionDefinitionContract>() > 0)
            this.cmbSource.Text = source;
        }
        if (this.owners == null || this.owners.Length == 0)
          return;
        this.populateOwnersDropdown();
        if (((IEnumerable<RoleInfo>) this.owners).Where<RoleInfo>((Func<RoleInfo, bool>) (r => r.RoleName == owner)).Count<RoleInfo>() <= 0)
          return;
        this.cmbowner.Text = owner;
      }
      else
        this.setCustomizeTypeControls(new bool?(false));
    }

    private void populateOptionDropdown(
      OptionDefinitionContract[] optionDefinitions,
      ComboBox comboBox)
    {
      ((IEnumerable<OptionDefinitionContract>) optionDefinitions).ToList<OptionDefinitionContract>().ForEach((Action<OptionDefinitionContract>) (item => comboBox.Items.Add((object) item)));
      if (optionDefinitions.Length > 1)
        comboBox.Items.Insert(0, (object) this.defaultDropDownValue);
      comboBox.DisplayMember = "name";
      comboBox.ValueMember = "name";
      comboBox.SelectedIndex = 0;
    }

    private void initDocumentList()
    {
      this.docSetup = this.session.ConfigurationManager.GetDocumentTrackingSetup();
      if (this.enhancedConditionTemplate.AssignedTo == null)
        return;
      this.selectedDocList = this.enhancedConditionTemplate.AssignedTo.Select<EntityReferenceContract, string>((Func<EntityReferenceContract, string>) (dl => dl.entityId.ToString())).ToArray<string>();
    }

    private bool isValidInputs()
    {
      List<string> stringList = new List<string>();
      if (!string.IsNullOrWhiteSpace(this.txtDaysToReceive.Text) && Utils.ParseInt((object) this.txtDaysToReceive.Text, -1) < 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Invalid Days to Receive", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      if (this.cmbConditionType.SelectedItem == null || this.cmbConditionType.SelectedItem.ToString().Equals("Please select", StringComparison.CurrentCultureIgnoreCase))
        stringList.Add(this.lblType.Text);
      if (string.IsNullOrWhiteSpace(this.txtName.Text.Trim()))
        stringList.Add(this.lblName.Text);
      if (stringList.Count > 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, string.Format("Please provide details for {0}.", (object) string.Join(", ", stringList.ToArray())), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      if (!string.IsNullOrWhiteSpace(this.txtEffectiveStartDate.Text) && !Utils.IsDate((object) this.txtEffectiveStartDate.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Invalid start date format", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.txtEffectiveStartDate.Text = string.Empty;
        return false;
      }
      if (!string.IsNullOrWhiteSpace(this.txtEffectiveEndDate.Text) && !Utils.IsDate((object) this.txtEffectiveEndDate.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Invalid end date format", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.txtEffectiveEndDate.Text = string.Empty;
        return false;
      }
      if (!string.IsNullOrWhiteSpace(this.txtEffectiveStartDate.Text) && !string.IsNullOrWhiteSpace(this.txtEffectiveEndDate.Text) && !(Utils.ParseDate((object) this.txtEffectiveEndDate.Text) >= Utils.ParseDate((object) this.txtEffectiveStartDate.Text)))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "End date should be greater than or equal to Start Date.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.txtEffectiveEndDate.Text = string.Empty;
        return false;
      }
      if (this.rdTpoDefaultDoc.Checked && string.IsNullOrWhiteSpace(this.defaultDocLabel.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select the Default Document to upload TPO attachments.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      return this.validateAlphanumeric((object) this.txtInternalId) && this.validateAlphanumeric((object) this.txtExternalId);
    }

    private void toggleTpoSaveDocument()
    {
      if (this.rdTpoDocSameName.Checked)
        this.btnSaveTpoDoc.Enabled = false;
      if (!this.rdTpoDefaultDoc.Checked)
        return;
      this.btnSaveTpoDoc.Enabled = true;
    }

    private void initTPOCondDocType(ConnectSettingsContract connectSettingsContract)
    {
      if (connectSettingsContract.DocumentOption.ToLower() == ConnectSettingsDocumentOptions.MatchConditionName.ToString().ToLower())
      {
        this.rdTpoDocSameName.Checked = true;
      }
      else
      {
        if (!(connectSettingsContract.DocumentOption.ToLower() == ConnectSettingsDocumentOptions.DefaultDocument.ToString().ToLower()))
          return;
        this.tpoDefaultDoc = this.getTPODefaultDoc(this.docSetup, this.enhancedConditionTemplate.ConnectSettings.DocumentTemplate.entityId.ToString());
        if (this.tpoDefaultDoc != null)
          this.defaultDocLabel.Text = this.tpoDefaultDoc.Name;
        this.rdTpoDefaultDoc.Checked = true;
        this.btnSaveTpoDoc.Enabled = true;
      }
    }

    private DocumentTemplate getTPODefaultDoc(DocumentTrackingSetup docSetup, string TPOCondDocGuid)
    {
      ArrayList arrayList = new ArrayList();
      return TPOCondDocGuid != "" ? docSetup.GetByID(TPOCondDocGuid) ?? (DocumentTemplate) null : (DocumentTemplate) null;
    }

    private static bool IsTemplateDocsModified(
      IReadOnlyCollection<string> existingDocsList,
      IReadOnlyCollection<string> modifiedDocsList)
    {
      if ((existingDocsList != null ? existingDocsList.Count : 0) != (modifiedDocsList != null ? modifiedDocsList.Count : 0))
        return true;
      IEnumerable<string> source1 = existingDocsList != null ? existingDocsList.Except<string>((IEnumerable<string>) (modifiedDocsList ?? (IReadOnlyCollection<string>) new List<string>())) : (IEnumerable<string>) null;
      if ((source1 != null ? source1.Count<string>() : 0) > 0)
        return true;
      IEnumerable<string> source2 = modifiedDocsList != null ? modifiedDocsList.Except<string>((IEnumerable<string>) (existingDocsList ?? (IReadOnlyCollection<string>) new List<string>())) : (IEnumerable<string>) null;
      return (source2 != null ? source2.Count<string>() : 0) > 0;
    }

    public void getModifiedTemplateBaseContract()
    {
      if (this.isEdit)
      {
        if (this.txtInternalDesc.Text != this.enhancedConditionTemplate.InternalDescription)
          this.enhancedConditionTemplate.InternalDescription = this.txtInternalDesc.Text;
        if (this.txtExternalDesc.Text != this.enhancedConditionTemplate.ExternalDescription)
          this.enhancedConditionTemplate.ExternalDescription = this.txtExternalDesc.Text;
        if (this.txtInternalId.Text != this.enhancedConditionTemplate.InternalId)
          this.enhancedConditionTemplate.InternalId = this.txtInternalId.Text;
        if (this.txtExternalId.Text != this.enhancedConditionTemplate.ExternalId)
          this.enhancedConditionTemplate.ExternalId = this.txtExternalId.Text;
        if (this.chkPrintExternal.Checked != this.enhancedConditionTemplate.IsExternalPrint)
          this.enhancedConditionTemplate.IsExternalPrint = this.chkPrintExternal.Checked;
        if (this.chkPrintInternal.Checked != this.enhancedConditionTemplate.IsInternalPrint)
          this.enhancedConditionTemplate.IsInternalPrint = this.chkPrintInternal.Checked;
        List<string> list = this.lstDocuments.Items.OfType<DocumentTemplate>().Select<DocumentTemplate, string>((Func<DocumentTemplate, string>) (s1 => s1.Guid)).ToList<string>();
        List<EntityReferenceContract> assignedTo = this.enhancedConditionTemplate.AssignedTo;
        if (EnhancedConditionTemplateDialog.IsTemplateDocsModified(assignedTo != null ? (IReadOnlyCollection<string>) assignedTo.Select<EntityReferenceContract, string>((Func<EntityReferenceContract, string>) (x => x.entityId)).ToList<string>() : (IReadOnlyCollection<string>) null, (IReadOnlyCollection<string>) list))
        {
          this.enhancedConditionTemplate.AssignedTo = (List<EntityReferenceContract>) null;
          this.enhancedConditionTemplate.AssignedTo = new List<EntityReferenceContract>();
          foreach (DocumentTemplate documentTemplate in this.lstDocuments.Items)
            this.enhancedConditionTemplate.AssignedTo.Add(new EntityReferenceContract()
            {
              entityId = documentTemplate.Guid
            });
        }
        try
        {
          DateTime dateTime;
          if (this.txtEffectiveStartDate.Text != this.enhancedConditionTemplate.StartDate)
          {
            EnhancedConditionTemplate conditionTemplate = this.enhancedConditionTemplate;
            string text;
            if (!string.IsNullOrEmpty(this.txtEffectiveStartDate.Text))
            {
              dateTime = Convert.ToDateTime(this.txtEffectiveStartDate.Text);
              text = dateTime.ToString("yyyy-MM-dd");
            }
            else
              text = this.txtEffectiveStartDate.Text;
            conditionTemplate.StartDate = text;
          }
          if (this.txtEffectiveEndDate.Text != this.enhancedConditionTemplate.EndDate)
          {
            EnhancedConditionTemplate conditionTemplate = this.enhancedConditionTemplate;
            string text;
            if (!string.IsNullOrEmpty(this.txtEffectiveEndDate.Text))
            {
              dateTime = Convert.ToDateTime(this.txtEffectiveEndDate.Text);
              text = dateTime.ToString("yyyy-MM-dd");
            }
            else
              text = this.txtEffectiveEndDate.Text;
            conditionTemplate.EndDate = text;
          }
        }
        catch (Exception ex)
        {
          RemoteLogger.Write(TraceLevel.Error, nameof (EnhancedConditionTemplateDialog), "Error in parsing StartDate or End date", ex);
          int num = (int) Utils.Dialog((IWin32Window) this, "Error in parsing StartDate or End date : " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        int? nullable1;
        if (string.IsNullOrWhiteSpace(this.txtDaysToReceive.Text))
        {
          nullable1 = this.enhancedConditionTemplate.DaysToReceive;
          if (nullable1.HasValue)
          {
            EnhancedConditionTemplate conditionTemplate = this.enhancedConditionTemplate;
            nullable1 = new int?();
            int? nullable2 = nullable1;
            conditionTemplate.DaysToReceive = nullable2;
            goto label_61;
          }
        }
        if (!string.IsNullOrWhiteSpace(this.txtDaysToReceive.Text))
        {
          nullable1 = this.enhancedConditionTemplate.DaysToReceive;
          if (!nullable1.HasValue)
            goto label_39;
        }
        int num1 = Utils.ParseInt((object) this.txtDaysToReceive.Text);
        nullable1 = this.enhancedConditionTemplate.DaysToReceive;
        int valueOrDefault = nullable1.GetValueOrDefault();
        if (num1 == valueOrDefault & nullable1.HasValue)
          goto label_61;
label_39:
        EnhancedConditionTemplate conditionTemplate1 = this.enhancedConditionTemplate;
        int? nullable3;
        if (Utils.ParseInt((object) this.txtDaysToReceive.Text) != -1)
        {
          nullable3 = new int?(Convert.ToInt32(this.txtDaysToReceive.Text));
        }
        else
        {
          nullable1 = new int?();
          nullable3 = nullable1;
        }
        conditionTemplate1.DaysToReceive = nullable3;
      }
      else
      {
        this.enhancedConditionTemplate.InternalDescription = string.IsNullOrWhiteSpace(this.txtInternalDesc.Text) ? (string) null : this.txtInternalDesc.Text;
        this.enhancedConditionTemplate.ExternalDescription = string.IsNullOrWhiteSpace(this.txtExternalDesc.Text) ? (string) null : this.txtExternalDesc.Text;
        this.enhancedConditionTemplate.InternalId = string.IsNullOrWhiteSpace(this.txtInternalId.Text) ? (string) null : this.txtInternalId.Text;
        this.enhancedConditionTemplate.ExternalId = string.IsNullOrWhiteSpace(this.txtExternalId.Text) ? (string) null : this.txtExternalId.Text;
        this.enhancedConditionTemplate.PrintDefinitions = (List<string>) null;
        if (this.chkPrintExternal.Checked || this.chkPrintInternal.Checked)
        {
          this.enhancedConditionTemplate.IsExternalPrint = this.chkPrintExternal.Checked;
          this.enhancedConditionTemplate.IsInternalPrint = this.chkPrintInternal.Checked;
        }
        this.enhancedConditionTemplate.AssignedTo = (List<EntityReferenceContract>) null;
        if (this.lstDocuments.Items != null && this.lstDocuments.Items.Count > 0)
        {
          this.enhancedConditionTemplate.AssignedTo = new List<EntityReferenceContract>();
          foreach (DocumentTemplate documentTemplate in this.lstDocuments.Items)
            this.enhancedConditionTemplate.AssignedTo.Add(new EntityReferenceContract()
            {
              entityId = documentTemplate.Guid
            });
        }
        try
        {
          if (!string.IsNullOrWhiteSpace(this.txtEffectiveStartDate.Text) && Convert.ToDateTime(this.txtEffectiveStartDate.Text) != DateTime.MinValue)
            this.enhancedConditionTemplate.StartDate = Convert.ToDateTime(this.txtEffectiveStartDate.Text).ToString("yyyy-MM-dd");
          if (!string.IsNullOrWhiteSpace(this.txtEffectiveEndDate.Text))
          {
            if (Convert.ToDateTime(this.txtEffectiveEndDate.Text) != DateTime.MinValue)
              this.enhancedConditionTemplate.EndDate = Convert.ToDateTime(this.txtEffectiveEndDate.Text).ToString("yyyy-MM-dd");
          }
        }
        catch (Exception ex)
        {
          RemoteLogger.Write(TraceLevel.Error, nameof (EnhancedConditionTemplateDialog), "Error in parsing StartDate or End date", ex);
          int num = (int) Utils.Dialog((IWin32Window) this, "Error in parsing StartDate or End date : " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        this.enhancedConditionTemplate.DaysToReceive = string.IsNullOrWhiteSpace(this.txtDaysToReceive.Text) ? new int?() : new int?(Convert.ToInt32(this.txtDaysToReceive.Text));
      }
label_61:
      this.enhancedConditionTemplate.ConditionType = this.cmbConditionType.SelectedItem is EnhancedConditionType selectedItem1 ? selectedItem1.title : (string) null;
      this.enhancedConditionTemplate.Category = this.cmbCategory.Text == this.defaultDropDownValue ? string.Empty : this.cmbCategory.Text;
      this.enhancedConditionTemplate.Source = this.cmbSource.Text == this.defaultDropDownValue ? string.Empty : this.cmbSource.Text;
      this.enhancedConditionTemplate.PriorTo = this.cmbPriorTo.Text == this.defaultDropDownValue ? string.Empty : this.cmbPriorTo.Text;
      this.enhancedConditionTemplate.Recipient = this.cmbRecipient.Text == this.defaultDropDownValue ? string.Empty : this.cmbRecipient.Text;
      EnhancedConditionTemplate conditionTemplate2 = this.enhancedConditionTemplate;
      EntityReferenceContract referenceContract;
      if (!(this.cmbowner.Text == this.defaultDropDownValue))
        referenceContract = new EntityReferenceContract()
        {
          entityId = this.cmbowner.SelectedItem is RoleInfo selectedItem2 ? selectedItem2.RoleID.ToString() : (string) null
        };
      else
        referenceContract = (EntityReferenceContract) null;
      conditionTemplate2.Owner = referenceContract;
      this.enhancedConditionTemplate.CustomizeTypeDefinition = new bool?(this.isCustomizedTypeDefinition);
      this.enhancedConditionTemplate.customDefinitions = this.customizedTypeDefinitions;
      this.enhancedConditionTemplate.AllowDuplicate = new bool?(!this.chkAllowDuplicate.Checked);
      this.enhancedConditionTemplate.Title = this.txtName.Text;
      if (this.rdTpoDefaultDoc.Checked)
        this.enhancedConditionTemplate.ConnectSettings = new ConnectSettingsContract()
        {
          DocumentOption = ConnectSettingsDocumentOptions.DefaultDocument.ToString(),
          DocumentTemplate = new EntityReferenceContract()
          {
            entityId = this.tpoDefaultDoc.Guid.ToString()
          }
        };
      else if (this.rdTpoDocSameName.Checked)
        this.enhancedConditionTemplate.ConnectSettings = new ConnectSettingsContract()
        {
          DocumentOption = ConnectSettingsDocumentOptions.MatchConditionName.ToString(),
          DocumentTemplate = (EntityReferenceContract) null
        };
      this.enhancedConditionTemplate.Active = new bool?();
    }

    private void populateOwnersDropdown()
    {
      this.cmbowner.Items.Clear();
      if (this.owners.Length != 0)
      {
        foreach (object owner in this.owners)
          this.cmbowner.Items.Add(owner);
        this.cmbowner.DisplayMember = "RoleName";
        this.cmbowner.ValueMember = "RoleID";
      }
      this.cmbowner.Items.Insert(0, (object) this.defaultDropDownValue);
      this.cmbowner.SelectedIndex = 0;
    }

    private void GetAllRoles()
    {
      this.owners = ((IEnumerable<RoleInfo>) ((WorkflowManager) this.session.BPM.GetBpmManager(BpmCategory.Workflow)).GetAllRoleFunctions()).OrderBy<RoleInfo, string>((Func<RoleInfo, string>) (r => r.RoleName)).ToArray<RoleInfo>();
    }

    private void btnDocuments_Click(object sender, EventArgs e)
    {
      using (ConditionDocumentsDialog conditionDocumentsDialog = new ConditionDocumentsDialog(this.session, this.docSetup, this.docList))
      {
        if (conditionDocumentsDialog.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
          return;
        this.docList = conditionDocumentsDialog.Documents;
        this.loadDocumentList();
      }
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (!this.hasPersonaEditPermission)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You do not have permission to add/edit condition template. Please contact your administrator.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        if (!this.isValidInputs())
          return;
        bool? active = this.enhancedConditionTemplate.Active;
        this.getModifiedTemplateBaseContract();
        try
        {
          int num2;
          if (!this.isEdit)
          {
            num2 = 0;
          }
          else
          {
            ConditionAPIActions conditionApiActions = (ConditionAPIActions) (num2 = 1);
          }
          string[] strArray = EnhancedConditionRestApiHelper.AddConditionTemplates(new EnhancedConditionTemplate[1]
          {
            this.enhancedConditionTemplate
          }, false, ((ConditionAPIActions) num2).ToString());
          if (this.isEdit)
            this.enhancedConditionTemplate.Active = active;
          if ((!this.isEdit || strArray == null) && (this.isEdit || strArray == null || strArray.Length == 0))
            return;
          RemoteLogger.Write(TraceLevel.Info, "Template updated successfully with the id:" + strArray[0]);
          this.DialogResult = DialogResult.OK;
        }
        catch (HttpException ex)
        {
          RemoteLogger.Write(TraceLevel.Error, nameof (EnhancedConditionTemplateDialog), "Error in in adding condition templates", (Exception) ex);
          if (ex.GetHttpCode() == 409)
          {
            int num3 = (int) Utils.Dialog((IWin32Window) this, "Name already exists for the Condition Type. Please provide a unique name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
          else
          {
            int num4 = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
        catch (Exception ex)
        {
          RemoteLogger.Write(TraceLevel.Error, nameof (EnhancedConditionTemplateDialog), "Error in in adding condition templates", ex);
          int num5 = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
      }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      if (this.isFormChanged)
      {
        if (Utils.Dialog((IWin32Window) this, "All unsaved changes will be discarded.\n\rDo you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
          return;
        this.isFormChanged = false;
        this.DialogResult = DialogResult.Cancel;
      }
      else
        this.DialogResult = DialogResult.Cancel;
    }

    private void txtChanged_validation(object sender, EventArgs e) => this.setDirty(true);

    private void txtformat_validation(object sender, KeyPressEventArgs e)
    {
    }

    private void txtformat_validation_nospace(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if (char.IsLetterOrDigit(e.KeyChar) || char.IsNumber(e.KeyChar))
        e.Handled = false;
      else
        e.Handled = true;
    }

    private void rdTpoDefaultDoc_Click(object sender, EventArgs e)
    {
      this.toggleTpoSaveDocument();
      this.btnSave.Enabled = true;
      this.isFormChanged = true;
    }

    private void rdTpoDocSameName_Click(object sender, EventArgs e)
    {
      this.toggleTpoSaveDocument();
      this.btnSave.Enabled = true;
      this.isFormChanged = true;
    }

    private void btnSaveTpoDoc_Click(object sender, EventArgs e)
    {
      DocumentTemplate documentTemplate = (DocumentTemplate) null;
      if (this.tpoDefaultDoc != null)
        documentTemplate = this.tpoDefaultDoc;
      else if (this.docList.Length != 0)
        documentTemplate = this.docList[0];
      using (ConditionDocumentsDialog conditionDocumentsDialog = new ConditionDocumentsDialog(this.session, this.docSetup, this.docList, false, this.tpoDefaultDoc))
      {
        if (conditionDocumentsDialog.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
          return;
        DocumentTemplate[] documents = conditionDocumentsDialog.Documents;
        if (documents.Length == 0)
          return;
        this.tpoDefaultDoc = documents[0];
        this.defaultDocLabel.Text = this.tpoDefaultDoc.Name;
      }
    }

    private void dateChanged_validation(object sender, MouseEventArgs e)
    {
      this.btnSave.Enabled = true;
      this.isFormChanged = true;
    }

    private void ConditionTemplateDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (!this.isFormChanged || Utils.Dialog((IWin32Window) this, "All unsaved changes will be discarded,  Do you want to Continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
        return;
      e.Cancel = true;
    }

    private void cmbConditionType_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.chkCustomizeCondition.Checked && Utils.Dialog((IWin32Window) this, "Selecting a different Type will clear the Customized Conditions Settings. Do you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
      {
        this.cmbConditionType.SelectedIndexChanged -= new EventHandler(this.cmbConditionType_SelectedIndexChanged);
        this.cmbConditionType.Text = this.currentSelectedType;
        this.cmbConditionType.SelectedIndexChanged += new EventHandler(this.cmbConditionType_SelectedIndexChanged);
      }
      else
      {
        this.currentSelectedType = this.cmbConditionType.Text;
        this.setCustomizeTypeValues(false, (ConditionDefinitionContract) null);
        this.populateConditionTypeDefinition();
      }
    }

    private void textboxNumericOnlyValidation(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if (!char.IsNumber(e.KeyChar))
        e.Handled = true;
      else
        e.Handled = false;
    }

    private void btnEditCustomizeCondition_Click(object sender, EventArgs e)
    {
      this.showCustomizeTypeDialog();
    }

    private void chkCustomizeCondition_Click(object sender, EventArgs e)
    {
      if (this.chkCustomizeCondition.Checked)
        this.showCustomizeTypeDialog();
      else if (Utils.Dialog((IWin32Window) this, "Do you want to remove all the Customized details?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
      {
        this.setCustomizeTypeValues(false, (ConditionDefinitionContract) null);
        this.populateConditionTypeDefinition();
      }
      else
        this.setCustomizeTypeControls(new bool?(true), true);
    }

    private void showCustomizeTypeDialog()
    {
      EnhancedConditionType selectedItem = (EnhancedConditionType) this.cmbConditionType.SelectedItem;
      EnhancedConditionType conditionTypeDetails = new EnhancedConditionType()
      {
        title = selectedItem.title,
        id = selectedItem.id,
        definitions = selectedItem.definitions
      };
      if (this.isCustomizedTypeDefinition)
        conditionTypeDetails.definitions = this.customizedTypeDefinitions;
      ConditionTypeSettingsAddEditDialog settingsAddEditDialog = new ConditionTypeSettingsAddEditDialog(this.session, conditionTypeDetails, true);
      if (settingsAddEditDialog.ShowDialog((IWin32Window) this) == DialogResult.OK)
      {
        this.setCustomizeTypeValues(true, settingsAddEditDialog.CustomizedConditionDefinition);
        this.populateConditionTypeDefinition();
      }
      else
      {
        if (this.isCustomizedTypeDefinition)
          return;
        this.setCustomizeTypeControls(new bool?(true));
      }
    }

    private void setDirty(bool isDirty)
    {
      this.btnSave.Enabled = isDirty;
      this.isFormChanged = isDirty;
    }

    private void setCustomizeTypeControls(bool? isEnabled = null, bool isChecked = false)
    {
      if (isEnabled.HasValue)
      {
        this.chkCustomizeCondition.Enabled = isEnabled.Value;
        this.chkCustomizeCondition.Checked = isChecked;
      }
      this.btnEditCustomizeCondition.Enabled = isChecked;
    }

    private void setCustomizeTypeValues(bool isCustomized, ConditionDefinitionContract definitions)
    {
      this.isCustomizedTypeDefinition = isCustomized;
      this.customizedTypeDefinitions = definitions;
    }

    private bool validateAlphanumeric(object control)
    {
      bool flag = false;
      string str = string.Empty;
      string empty = string.Empty;
      if (control is TextBox)
      {
        // ISSUE: reference to a compiler-generated field
        if (EnhancedConditionTemplateDialog.\u003C\u003Eo__54.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          EnhancedConditionTemplateDialog.\u003C\u003Eo__54.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (EnhancedConditionTemplateDialog)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, string> target1 = EnhancedConditionTemplateDialog.\u003C\u003Eo__54.\u003C\u003Ep__1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, string>> p1 = EnhancedConditionTemplateDialog.\u003C\u003Eo__54.\u003C\u003Ep__1;
        // ISSUE: reference to a compiler-generated field
        if (EnhancedConditionTemplateDialog.\u003C\u003Eo__54.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          EnhancedConditionTemplateDialog.\u003C\u003Eo__54.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Text", typeof (EnhancedConditionTemplateDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj1 = EnhancedConditionTemplateDialog.\u003C\u003Eo__54.\u003C\u003Ep__0.Target((CallSite) EnhancedConditionTemplateDialog.\u003C\u003Eo__54.\u003C\u003Ep__0, control);
        flag = this.isAlphanumeric(target1((CallSite) p1, obj1));
        if (!flag)
        {
          // ISSUE: reference to a compiler-generated field
          if (EnhancedConditionTemplateDialog.\u003C\u003Eo__54.\u003C\u003Ep__4 == null)
          {
            // ISSUE: reference to a compiler-generated field
            EnhancedConditionTemplateDialog.\u003C\u003Eo__54.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (EnhancedConditionTemplateDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, bool> target2 = EnhancedConditionTemplateDialog.\u003C\u003Eo__54.\u003C\u003Ep__4.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, bool>> p4 = EnhancedConditionTemplateDialog.\u003C\u003Eo__54.\u003C\u003Ep__4;
          // ISSUE: reference to a compiler-generated field
          if (EnhancedConditionTemplateDialog.\u003C\u003Eo__54.\u003C\u003Ep__3 == null)
          {
            // ISSUE: reference to a compiler-generated field
            EnhancedConditionTemplateDialog.\u003C\u003Eo__54.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (EnhancedConditionTemplateDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, string, object> target3 = EnhancedConditionTemplateDialog.\u003C\u003Eo__54.\u003C\u003Ep__3.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, string, object>> p3 = EnhancedConditionTemplateDialog.\u003C\u003Eo__54.\u003C\u003Ep__3;
          // ISSUE: reference to a compiler-generated field
          if (EnhancedConditionTemplateDialog.\u003C\u003Eo__54.\u003C\u003Ep__2 == null)
          {
            // ISSUE: reference to a compiler-generated field
            EnhancedConditionTemplateDialog.\u003C\u003Eo__54.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Name", typeof (EnhancedConditionTemplateDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj2 = EnhancedConditionTemplateDialog.\u003C\u003Eo__54.\u003C\u003Ep__2.Target((CallSite) EnhancedConditionTemplateDialog.\u003C\u003Eo__54.\u003C\u003Ep__2, control);
          string name1 = this.txtInternalId.Name;
          object obj3 = target3((CallSite) p3, obj2, name1);
          if (target2((CallSite) p4, obj3))
          {
            str = this.lblInternalID.Text;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (EnhancedConditionTemplateDialog.\u003C\u003Eo__54.\u003C\u003Ep__7 == null)
            {
              // ISSUE: reference to a compiler-generated field
              EnhancedConditionTemplateDialog.\u003C\u003Eo__54.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (EnhancedConditionTemplateDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, bool> target4 = EnhancedConditionTemplateDialog.\u003C\u003Eo__54.\u003C\u003Ep__7.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, bool>> p7 = EnhancedConditionTemplateDialog.\u003C\u003Eo__54.\u003C\u003Ep__7;
            // ISSUE: reference to a compiler-generated field
            if (EnhancedConditionTemplateDialog.\u003C\u003Eo__54.\u003C\u003Ep__6 == null)
            {
              // ISSUE: reference to a compiler-generated field
              EnhancedConditionTemplateDialog.\u003C\u003Eo__54.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (EnhancedConditionTemplateDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, string, object> target5 = EnhancedConditionTemplateDialog.\u003C\u003Eo__54.\u003C\u003Ep__6.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, string, object>> p6 = EnhancedConditionTemplateDialog.\u003C\u003Eo__54.\u003C\u003Ep__6;
            // ISSUE: reference to a compiler-generated field
            if (EnhancedConditionTemplateDialog.\u003C\u003Eo__54.\u003C\u003Ep__5 == null)
            {
              // ISSUE: reference to a compiler-generated field
              EnhancedConditionTemplateDialog.\u003C\u003Eo__54.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Name", typeof (EnhancedConditionTemplateDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj4 = EnhancedConditionTemplateDialog.\u003C\u003Eo__54.\u003C\u003Ep__5.Target((CallSite) EnhancedConditionTemplateDialog.\u003C\u003Eo__54.\u003C\u003Ep__5, control);
            string name2 = this.txtExternalId.Name;
            object obj5 = target5((CallSite) p6, obj4, name2);
            if (target4((CallSite) p7, obj5))
              str = this.lblExternalID.Text;
          }
        }
      }
      if (flag)
        return true;
      int num = (int) Utils.Dialog((IWin32Window) this, "Please provide valid input to " + str);
      // ISSUE: reference to a compiler-generated field
      if (EnhancedConditionTemplateDialog.\u003C\u003Eo__54.\u003C\u003Ep__8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        EnhancedConditionTemplateDialog.\u003C\u003Eo__54.\u003C\u003Ep__8 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Focus", (IEnumerable<System.Type>) null, typeof (EnhancedConditionTemplateDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      EnhancedConditionTemplateDialog.\u003C\u003Eo__54.\u003C\u003Ep__8.Target((CallSite) EnhancedConditionTemplateDialog.\u003C\u003Eo__54.\u003C\u003Ep__8, control);
      return false;
    }

    private bool isAlphanumeric(string str) => new Regex("^[a-zA-Z0-9]*$").IsMatch(str);

    private void txtValidateAlphanumeric_Leave(object sender, EventArgs e)
    {
      this.validateAlphanumeric(sender);
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (EnhancedConditionTemplateDialog));
      this.groupBoxMain = new GroupBox();
      this.cmbowner = new ComboBox();
      this.lblowner = new Label();
      this.btnEditCustomizeCondition = new StandardIconButton();
      this.helpLinkDocuments = new EMHelpLink();
      this.label5 = new Label();
      this.btnCancel = new Button();
      this.label52 = new Label();
      this.btnSave = new Button();
      this.chkPrintExternal = new CheckBox();
      this.chkPrintInternal = new CheckBox();
      this.txtDaysToReceive = new TextBox();
      this.label16 = new Label();
      this.label15 = new Label();
      this.grpDates = new GroupBox();
      this.calEndDate = new CalendarButton();
      this.txtEffectiveEndDate = new TextBox();
      this.calStartDate = new CalendarButton();
      this.txtEffectiveStartDate = new TextBox();
      this.label14 = new Label();
      this.label13 = new Label();
      this.label12 = new Label();
      this.grpCustomizeSettings = new GroupBox();
      this.cmbPriorTo = new ComboBox();
      this.cmbRecipient = new ComboBox();
      this.cmbSource = new ComboBox();
      this.cmbCategory = new ComboBox();
      this.label11 = new Label();
      this.label10 = new Label();
      this.label9 = new Label();
      this.label8 = new Label();
      this.txtInternalDesc = new TextBox();
      this.groupBox1 = new GroupBox();
      this.btnSaveTpoDoc = new Button();
      this.rdTpoDocSameName = new RadioButton();
      this.rdTpoDefaultDoc = new RadioButton();
      this.defaultDocLabel = new Label();
      this.chkCustomizeCondition = new CheckBox();
      this.lstDocuments = new ListBox();
      this.button2 = new Button();
      this.txtExternalDesc = new TextBox();
      this.label7 = new Label();
      this.label6 = new Label();
      this.txtName = new TextBox();
      this.lblName = new Label();
      this.chkAllowDuplicate = new CheckBox();
      this.txtConditionStatus = new TextBox();
      this.txtExternalId = new TextBox();
      this.txtInternalId = new TextBox();
      this.cmbConditionType = new ComboBox();
      this.label4 = new Label();
      this.lblExternalID = new Label();
      this.lblInternalID = new Label();
      this.lblType = new Label();
      this.toolTip1 = new ToolTip(this.components);
      this.groupBoxMain.SuspendLayout();
      ((ISupportInitialize) this.btnEditCustomizeCondition).BeginInit();
      this.grpDates.SuspendLayout();
      ((ISupportInitialize) this.calEndDate).BeginInit();
      ((ISupportInitialize) this.calStartDate).BeginInit();
      this.grpCustomizeSettings.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      this.groupBoxMain.Controls.Add((Control) this.cmbowner);
      this.groupBoxMain.Controls.Add((Control) this.lblowner);
      this.groupBoxMain.Controls.Add((Control) this.btnEditCustomizeCondition);
      this.groupBoxMain.Controls.Add((Control) this.helpLinkDocuments);
      this.groupBoxMain.Controls.Add((Control) this.label5);
      this.groupBoxMain.Controls.Add((Control) this.btnCancel);
      this.groupBoxMain.Controls.Add((Control) this.label52);
      this.groupBoxMain.Controls.Add((Control) this.btnSave);
      this.groupBoxMain.Controls.Add((Control) this.chkPrintExternal);
      this.groupBoxMain.Controls.Add((Control) this.chkPrintInternal);
      this.groupBoxMain.Controls.Add((Control) this.txtDaysToReceive);
      this.groupBoxMain.Controls.Add((Control) this.label16);
      this.groupBoxMain.Controls.Add((Control) this.label15);
      this.groupBoxMain.Controls.Add((Control) this.grpDates);
      this.groupBoxMain.Controls.Add((Control) this.grpCustomizeSettings);
      this.groupBoxMain.Controls.Add((Control) this.txtInternalDesc);
      this.groupBoxMain.Controls.Add((Control) this.groupBox1);
      this.groupBoxMain.Controls.Add((Control) this.chkCustomizeCondition);
      this.groupBoxMain.Controls.Add((Control) this.lstDocuments);
      this.groupBoxMain.Controls.Add((Control) this.button2);
      this.groupBoxMain.Controls.Add((Control) this.txtExternalDesc);
      this.groupBoxMain.Controls.Add((Control) this.label7);
      this.groupBoxMain.Controls.Add((Control) this.label6);
      this.groupBoxMain.Controls.Add((Control) this.txtName);
      this.groupBoxMain.Controls.Add((Control) this.lblName);
      this.groupBoxMain.Controls.Add((Control) this.chkAllowDuplicate);
      this.groupBoxMain.Controls.Add((Control) this.txtConditionStatus);
      this.groupBoxMain.Controls.Add((Control) this.txtExternalId);
      this.groupBoxMain.Controls.Add((Control) this.txtInternalId);
      this.groupBoxMain.Controls.Add((Control) this.cmbConditionType);
      this.groupBoxMain.Controls.Add((Control) this.label4);
      this.groupBoxMain.Controls.Add((Control) this.lblExternalID);
      this.groupBoxMain.Controls.Add((Control) this.lblInternalID);
      this.groupBoxMain.Controls.Add((Control) this.lblType);
      this.groupBoxMain.Location = new Point(22, 18);
      this.groupBoxMain.Margin = new Padding(14);
      this.groupBoxMain.Name = "groupBoxMain";
      this.groupBoxMain.Padding = new Padding(4, 5, 4, 5);
      this.groupBoxMain.Size = new Size(868, 1016);
      this.groupBoxMain.TabIndex = 0;
      this.groupBoxMain.TabStop = false;
      this.cmbowner.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbowner.FormattingEnabled = true;
      this.cmbowner.Location = new Point(180, 558);
      this.cmbowner.Margin = new Padding(15);
      this.cmbowner.Name = "cmbowner";
      this.cmbowner.Size = new Size(193, 28);
      this.cmbowner.TabIndex = 47;
      this.lblowner.AutoSize = true;
      this.lblowner.Location = new Point(27, 560);
      this.lblowner.Margin = new Padding(15);
      this.lblowner.Name = "lblowner";
      this.lblowner.Size = new Size(55, 20);
      this.lblowner.TabIndex = 46;
      this.lblowner.Text = "Owner";
      this.btnEditCustomizeCondition.BackColor = Color.Transparent;
      this.btnEditCustomizeCondition.Location = new Point(268, 595);
      this.btnEditCustomizeCondition.Margin = new Padding(4, 5, 4, 5);
      this.btnEditCustomizeCondition.MouseDownImage = (Image) null;
      this.btnEditCustomizeCondition.Name = "btnEditCustomizeCondition";
      this.btnEditCustomizeCondition.Size = new Size(24, 25);
      this.btnEditCustomizeCondition.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEditCustomizeCondition.TabIndex = 45;
      this.btnEditCustomizeCondition.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnEditCustomizeCondition, "Edit Customized Condition Settings");
      this.btnEditCustomizeCondition.Click += new EventHandler(this.btnEditCustomizeCondition_Click);
      this.helpLinkDocuments.BackColor = Color.Transparent;
      this.helpLinkDocuments.Cursor = Cursors.Hand;
      this.helpLinkDocuments.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.helpLinkDocuments.HelpTag = "EnhancedConditionDialog";
      this.helpLinkDocuments.Location = new Point(26, 963);
      this.helpLinkDocuments.Margin = new Padding(4, 5, 4, 5);
      this.helpLinkDocuments.Name = "helpLinkDocuments";
      this.helpLinkDocuments.Size = new Size(135, 26);
      this.helpLinkDocuments.TabIndex = 26;
      this.label5.AutoSize = true;
      this.label5.BackColor = Color.Transparent;
      this.label5.Font = new Font("Arial", 9.75f, FontStyle.Bold);
      this.label5.ForeColor = Color.Red;
      this.label5.Location = new Point(68, 155);
      this.label5.Margin = new Padding(4, 0, 4, 0);
      this.label5.Name = "label5";
      this.label5.Size = new Size(18, 24);
      this.label5.TabIndex = 44;
      this.label5.Text = "*";
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.Location = new Point(728, 970);
      this.btnCancel.Margin = new Padding(4, 5, 4, 5);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(112, 37);
      this.btnCancel.TabIndex = 28;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.label52.AutoSize = true;
      this.label52.BackColor = Color.Transparent;
      this.label52.Font = new Font("Arial", 9.75f, FontStyle.Bold);
      this.label52.ForeColor = Color.Red;
      this.label52.Location = new Point(62, 31);
      this.label52.Margin = new Padding(4, 0, 4, 0);
      this.label52.Name = "label52";
      this.label52.Size = new Size(18, 24);
      this.label52.TabIndex = 43;
      this.label52.Text = "*";
      this.btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSave.Location = new Point(592, 970);
      this.btnSave.Margin = new Padding(4, 5, 4, 5);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(112, 37);
      this.btnSave.TabIndex = 27;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.chkPrintExternal.AutoSize = true;
      this.chkPrintExternal.Location = new Point(732, 791);
      this.chkPrintExternal.Margin = new Padding(4, 5, 4, 5);
      this.chkPrintExternal.Name = "chkPrintExternal";
      this.chkPrintExternal.Size = new Size(93, 24);
      this.chkPrintExternal.TabIndex = 21;
      this.chkPrintExternal.Text = "External";
      this.chkPrintExternal.UseVisualStyleBackColor = true;
      this.chkPrintExternal.CheckedChanged += new EventHandler(this.txtChanged_validation);
      this.chkPrintInternal.AutoSize = true;
      this.chkPrintInternal.Location = new Point(615, 791);
      this.chkPrintInternal.Margin = new Padding(4, 5, 4, 5);
      this.chkPrintInternal.Name = "chkPrintInternal";
      this.chkPrintInternal.Size = new Size(89, 24);
      this.chkPrintInternal.TabIndex = 20;
      this.chkPrintInternal.Text = "Internal";
      this.chkPrintInternal.UseVisualStyleBackColor = true;
      this.chkPrintInternal.CheckedChanged += new EventHandler(this.txtChanged_validation);
      this.txtDaysToReceive.Location = new Point(615, 752);
      this.txtDaysToReceive.Margin = new Padding(4, 5, 4, 5);
      this.txtDaysToReceive.MaxLength = 3;
      this.txtDaysToReceive.Name = "txtDaysToReceive";
      this.txtDaysToReceive.Size = new Size(112, 26);
      this.txtDaysToReceive.TabIndex = 19;
      this.txtDaysToReceive.TextChanged += new EventHandler(this.txtChanged_validation);
      this.txtDaysToReceive.KeyPress += new KeyPressEventHandler(this.textboxNumericOnlyValidation);
      this.label16.AutoSize = true;
      this.label16.Location = new Point(483, 792);
      this.label16.Margin = new Padding(15);
      this.label16.Name = "label16";
      this.label16.Size = new Size(41, 20);
      this.label16.TabIndex = 30;
      this.label16.Text = "Print";
      this.label15.AutoSize = true;
      this.label15.Location = new Point(483, 757);
      this.label15.Margin = new Padding(15);
      this.label15.Name = "label15";
      this.label15.Size = new Size(124, 20);
      this.label15.TabIndex = 16;
      this.label15.Text = "Days to Receive";
      this.grpDates.Controls.Add((Control) this.calEndDate);
      this.grpDates.Controls.Add((Control) this.calStartDate);
      this.grpDates.Controls.Add((Control) this.txtEffectiveEndDate);
      this.grpDates.Controls.Add((Control) this.txtEffectiveStartDate);
      this.grpDates.Controls.Add((Control) this.label14);
      this.grpDates.Controls.Add((Control) this.label13);
      this.grpDates.Controls.Add((Control) this.label12);
      this.grpDates.Location = new Point(477, 622);
      this.grpDates.Margin = new Padding(4, 5, 4, 5);
      this.grpDates.Name = "grpDates";
      this.grpDates.Padding = new Padding(4, 5, 4, 5);
      this.grpDates.Size = new Size(366, 126);
      this.grpDates.TabIndex = 16;
      this.grpDates.TabStop = false;
      this.calEndDate.DateControl = (Control) this.txtEffectiveEndDate;
      ((IconButton) this.calEndDate).Image = (Image) componentResourceManager.GetObject("calEndDate.Image");
      this.calEndDate.Location = new Point(286, 85);
      this.calEndDate.Margin = new Padding(4, 5, 4, 5);
      this.calEndDate.MouseDownImage = (Image) null;
      this.calEndDate.Name = "calEndDate";
      this.calEndDate.Size = new Size(16, 16);
      this.calEndDate.SizeMode = PictureBoxSizeMode.AutoSize;
      this.calEndDate.TabIndex = 37;
      this.calEndDate.TabStop = false;
      this.txtEffectiveEndDate.Location = new Point(138, 80);
      this.txtEffectiveEndDate.Margin = new Padding(4, 5, 4, 5);
      this.txtEffectiveEndDate.Name = "txtEffectiveEndDate";
      this.txtEffectiveEndDate.Size = new Size(139, 26);
      this.txtEffectiveEndDate.TabIndex = 18;
      this.txtEffectiveEndDate.TextChanged += new EventHandler(this.txtChanged_validation);
      this.calStartDate.DateControl = (Control) this.txtEffectiveStartDate;
      ((IconButton) this.calStartDate).Image = (Image) componentResourceManager.GetObject("calStartDate.Image");
      this.calStartDate.Location = new Point(286, 45);
      this.calStartDate.Margin = new Padding(4, 5, 4, 5);
      this.calStartDate.MouseDownImage = (Image) null;
      this.calStartDate.Name = "calStartDate";
      this.calStartDate.Size = new Size(16, 16);
      this.calStartDate.SizeMode = PictureBoxSizeMode.AutoSize;
      this.calStartDate.TabIndex = 36;
      this.calStartDate.TabStop = false;
      this.txtEffectiveStartDate.Location = new Point(138, 43);
      this.txtEffectiveStartDate.Margin = new Padding(4, 5, 4, 5);
      this.txtEffectiveStartDate.Name = "txtEffectiveStartDate";
      this.txtEffectiveStartDate.Size = new Size(139, 26);
      this.txtEffectiveStartDate.TabIndex = 17;
      this.txtEffectiveStartDate.TextChanged += new EventHandler(this.txtChanged_validation);
      this.label14.AutoSize = true;
      this.label14.Location = new Point(6, 85);
      this.label14.Margin = new Padding(15);
      this.label14.Name = "label14";
      this.label14.Size = new Size(77, 20);
      this.label14.TabIndex = 13;
      this.label14.Text = "End Date";
      this.label13.AutoSize = true;
      this.label13.Location = new Point(6, 48);
      this.label13.Margin = new Padding(15);
      this.label13.Name = "label13";
      this.label13.Size = new Size(83, 20);
      this.label13.TabIndex = 12;
      this.label13.Text = "Start Date";
      this.label12.AutoSize = true;
      this.label12.Location = new Point(6, 17);
      this.label12.Margin = new Padding(15);
      this.label12.Name = "label12";
      this.label12.Size = new Size(118, 20);
      this.label12.TabIndex = 11;
      this.label12.Text = "Effective Dates";
      this.grpCustomizeSettings.Controls.Add((Control) this.cmbPriorTo);
      this.grpCustomizeSettings.Controls.Add((Control) this.cmbRecipient);
      this.grpCustomizeSettings.Controls.Add((Control) this.cmbSource);
      this.grpCustomizeSettings.Controls.Add((Control) this.cmbCategory);
      this.grpCustomizeSettings.Controls.Add((Control) this.label11);
      this.grpCustomizeSettings.Controls.Add((Control) this.label10);
      this.grpCustomizeSettings.Controls.Add((Control) this.label9);
      this.grpCustomizeSettings.Controls.Add((Control) this.label8);
      this.grpCustomizeSettings.FlatStyle = FlatStyle.Popup;
      this.grpCustomizeSettings.Location = new Point(27, 622);
      this.grpCustomizeSettings.Margin = new Padding(4, 5, 4, 5);
      this.grpCustomizeSettings.Name = "grpCustomizeSettings";
      this.grpCustomizeSettings.Padding = new Padding(4, 5, 4, 5);
      this.grpCustomizeSettings.Size = new Size(387, 192);
      this.grpCustomizeSettings.TabIndex = 11;
      this.grpCustomizeSettings.TabStop = false;
      this.cmbPriorTo.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbPriorTo.FormattingEnabled = true;
      this.cmbPriorTo.Location = new Point(158, 146);
      this.cmbPriorTo.Margin = new Padding(15);
      this.cmbPriorTo.Name = "cmbPriorTo";
      this.cmbPriorTo.Size = new Size(210, 28);
      this.cmbPriorTo.TabIndex = 15;
      this.cmbPriorTo.TextChanged += new EventHandler(this.txtChanged_validation);
      this.cmbRecipient.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbRecipient.FormattingEnabled = true;
      this.cmbRecipient.Location = new Point(158, 103);
      this.cmbRecipient.Margin = new Padding(15);
      this.cmbRecipient.Name = "cmbRecipient";
      this.cmbRecipient.Size = new Size(210, 28);
      this.cmbRecipient.TabIndex = 14;
      this.cmbRecipient.TextChanged += new EventHandler(this.txtChanged_validation);
      this.cmbSource.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbSource.FormattingEnabled = true;
      this.cmbSource.Location = new Point(158, 62);
      this.cmbSource.Margin = new Padding(15);
      this.cmbSource.Name = "cmbSource";
      this.cmbSource.Size = new Size(210, 28);
      this.cmbSource.TabIndex = 13;
      this.cmbSource.TextChanged += new EventHandler(this.txtChanged_validation);
      this.cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbCategory.FormattingEnabled = true;
      this.cmbCategory.Location = new Point(158, 20);
      this.cmbCategory.Margin = new Padding(15);
      this.cmbCategory.Name = "cmbCategory";
      this.cmbCategory.Size = new Size(210, 28);
      this.cmbCategory.TabIndex = 12;
      this.cmbCategory.TextChanged += new EventHandler(this.txtChanged_validation);
      this.label11.AutoSize = true;
      this.label11.Location = new Point(8, 63);
      this.label11.Margin = new Padding(15);
      this.label11.Name = "label11";
      this.label11.Size = new Size(60, 20);
      this.label11.TabIndex = 13;
      this.label11.Text = "Source";
      this.label10.AutoSize = true;
      this.label10.Location = new Point(8, 106);
      this.label10.Margin = new Padding(15);
      this.label10.Name = "label10";
      this.label10.Size = new Size(76, 20);
      this.label10.TabIndex = 12;
      this.label10.Text = "Recipient";
      this.label9.AutoSize = true;
      this.label9.Location = new Point(8, 151);
      this.label9.Margin = new Padding(15);
      this.label9.Name = "label9";
      this.label9.Size = new Size(63, 20);
      this.label9.TabIndex = 11;
      this.label9.Text = "Prior To";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(8, 25);
      this.label8.Margin = new Padding(15);
      this.label8.Name = "label8";
      this.label8.Size = new Size(73, 20);
      this.label8.TabIndex = 10;
      this.label8.Text = "Category";
      this.txtInternalDesc.Location = new Point(182, 192);
      this.txtInternalDesc.Margin = new Padding(4, 5, 4, 5);
      this.txtInternalDesc.Multiline = true;
      this.txtInternalDesc.Name = "txtInternalDesc";
      this.txtInternalDesc.ScrollBars = ScrollBars.Vertical;
      this.txtInternalDesc.Size = new Size(660, 113);
      this.txtInternalDesc.TabIndex = 6;
      this.txtInternalDesc.TextChanged += new EventHandler(this.txtChanged_validation);
      this.groupBox1.Controls.Add((Control) this.btnSaveTpoDoc);
      this.groupBox1.Controls.Add((Control) this.rdTpoDocSameName);
      this.groupBox1.Controls.Add((Control) this.rdTpoDefaultDoc);
      this.groupBox1.Controls.Add((Control) this.defaultDocLabel);
      this.groupBox1.Location = new Point(27, 823);
      this.groupBox1.Margin = new Padding(4, 5, 4, 5);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Padding = new Padding(4, 5, 4, 5);
      this.groupBox1.Size = new Size(819, 134);
      this.groupBox1.TabIndex = 22;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Automatically upload TPO attachments to";
      this.btnSaveTpoDoc.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSaveTpoDoc.BackColor = Color.WhiteSmoke;
      this.btnSaveTpoDoc.Location = new Point(600, 66);
      this.btnSaveTpoDoc.Margin = new Padding(4, 5, 4, 5);
      this.btnSaveTpoDoc.Name = "btnSaveTpoDoc";
      this.btnSaveTpoDoc.Size = new Size(196, 37);
      this.btnSaveTpoDoc.TabIndex = 25;
      this.btnSaveTpoDoc.Text = "Select Documents";
      this.btnSaveTpoDoc.UseVisualStyleBackColor = false;
      this.btnSaveTpoDoc.Click += new EventHandler(this.btnSaveTpoDoc_Click);
      this.rdTpoDocSameName.AutoSize = true;
      this.rdTpoDocSameName.Location = new Point(15, 32);
      this.rdTpoDocSameName.Margin = new Padding(4, 5, 4, 5);
      this.rdTpoDocSameName.Name = "rdTpoDocSameName";
      this.rdTpoDocSameName.Size = new Size(316, 24);
      this.rdTpoDocSameName.TabIndex = 23;
      this.rdTpoDocSameName.TabStop = true;
      this.rdTpoDocSameName.Text = "Document with same name as condition";
      this.rdTpoDocSameName.UseMnemonic = false;
      this.rdTpoDocSameName.UseVisualStyleBackColor = true;
      this.rdTpoDocSameName.CheckedChanged += new EventHandler(this.rdTpoDocSameName_Click);
      this.rdTpoDefaultDoc.AutoSize = true;
      this.rdTpoDefaultDoc.Location = new Point(15, 66);
      this.rdTpoDefaultDoc.Margin = new Padding(4, 5, 4, 5);
      this.rdTpoDefaultDoc.Name = "rdTpoDefaultDoc";
      this.rdTpoDefaultDoc.Size = new Size(164, 24);
      this.rdTpoDefaultDoc.TabIndex = 24;
      this.rdTpoDefaultDoc.TabStop = true;
      this.rdTpoDefaultDoc.Text = "Default Document";
      this.rdTpoDefaultDoc.UseVisualStyleBackColor = true;
      this.rdTpoDefaultDoc.Click += new EventHandler(this.rdTpoDefaultDoc_Click);
      this.defaultDocLabel.AutoSize = true;
      this.defaultDocLabel.ForeColor = SystemColors.InactiveCaptionText;
      this.defaultDocLabel.Location = new Point(190, 69);
      this.defaultDocLabel.Margin = new Padding(4, 0, 4, 0);
      this.defaultDocLabel.Name = "defaultDocLabel";
      this.defaultDocLabel.Size = new Size(0, 20);
      this.defaultDocLabel.TabIndex = 24;
      this.chkCustomizeCondition.AutoSize = true;
      this.chkCustomizeCondition.Location = new Point(27, 595);
      this.chkCustomizeCondition.Margin = new Padding(15);
      this.chkCustomizeCondition.Name = "chkCustomizeCondition";
      this.chkCustomizeCondition.Size = new Size(244, 24);
      this.chkCustomizeCondition.TabIndex = 10;
      this.chkCustomizeCondition.Text = "Customize Condition Settings";
      this.chkCustomizeCondition.UseVisualStyleBackColor = true;
      this.chkCustomizeCondition.CheckedChanged += new EventHandler(this.txtChanged_validation);
      this.chkCustomizeCondition.Click += new EventHandler(this.chkCustomizeCondition_Click);
      this.lstDocuments.FormattingEnabled = true;
      this.lstDocuments.ItemHeight = 20;
      this.lstDocuments.Location = new Point(180, 442);
      this.lstDocuments.Margin = new Padding(4, 5, 4, 5);
      this.lstDocuments.Name = "lstDocuments";
      this.lstDocuments.SelectionMode = SelectionMode.None;
      this.lstDocuments.Size = new Size(660, 104);
      this.lstDocuments.TabIndex = 9;
      this.lstDocuments.ValueMemberChanged += new EventHandler(this.txtChanged_validation);
      this.button2.BackColor = Color.WhiteSmoke;
      this.button2.Location = new Point(27, 442);
      this.button2.Margin = new Padding(4, 5, 4, 5);
      this.button2.Name = "button2";
      this.button2.Size = new Size(112, 37);
      this.button2.TabIndex = 8;
      this.button2.Text = "Documents";
      this.button2.UseVisualStyleBackColor = false;
      this.button2.Click += new EventHandler(this.btnDocuments_Click);
      this.txtExternalDesc.Location = new Point(180, 317);
      this.txtExternalDesc.Margin = new Padding(4, 5, 4, 5);
      this.txtExternalDesc.Multiline = true;
      this.txtExternalDesc.Name = "txtExternalDesc";
      this.txtExternalDesc.ScrollBars = ScrollBars.Vertical;
      this.txtExternalDesc.Size = new Size(660, 113);
      this.txtExternalDesc.TabIndex = 7;
      this.txtExternalDesc.TextChanged += new EventHandler(this.txtChanged_validation);
      this.label7.AutoSize = true;
      this.label7.Location = new Point(22, 317);
      this.label7.Margin = new Padding(15);
      this.label7.MaximumSize = new Size(90, 0);
      this.label7.Name = "label7";
      this.label7.Size = new Size(89, 40);
      this.label7.TabIndex = 11;
      this.label7.Text = "External Description";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(22, 192);
      this.label6.Margin = new Padding(15);
      this.label6.MaximumSize = new Size(90, 0);
      this.label6.Name = "label6";
      this.label6.Size = new Size(89, 40);
      this.label6.TabIndex = 10;
      this.label6.Text = "Internal Description";
      this.txtName.Location = new Point(182, 154);
      this.txtName.Margin = new Padding(15);
      this.txtName.MaxLength = (int) byte.MaxValue;
      this.txtName.Name = "txtName";
      this.txtName.Size = new Size(660, 26);
      this.txtName.TabIndex = 5;
      this.txtName.TextChanged += new EventHandler(this.txtChanged_validation);
      this.lblName.AutoSize = true;
      this.lblName.Location = new Point(22, 158);
      this.lblName.Margin = new Padding(15);
      this.lblName.Name = "lblName";
      this.lblName.Size = new Size(51, 20);
      this.lblName.TabIndex = 9;
      this.lblName.Text = "Name";
      this.chkAllowDuplicate.AutoSize = true;
      this.chkAllowDuplicate.Location = new Point(26, 111);
      this.chkAllowDuplicate.Margin = new Padding(15);
      this.chkAllowDuplicate.Name = "chkAllowDuplicate";
      this.chkAllowDuplicate.Size = new Size(325, 24);
      this.chkAllowDuplicate.TabIndex = 4;
      this.chkAllowDuplicate.Text = "Conditions cannot be duplicated in a loan";
      this.chkAllowDuplicate.UseVisualStyleBackColor = true;
      this.chkAllowDuplicate.CheckedChanged += new EventHandler(this.txtChanged_validation);
      this.chkAllowDuplicate.TextChanged += new EventHandler(this.txtChanged_validation);
      this.txtConditionStatus.Location = new Point(633, 29);
      this.txtConditionStatus.Margin = new Padding(4, 5, 4, 5);
      this.txtConditionStatus.Name = "txtConditionStatus";
      this.txtConditionStatus.ReadOnly = true;
      this.txtConditionStatus.Size = new Size(193, 26);
      this.txtConditionStatus.TabIndex = 1;
      this.txtConditionStatus.TabStop = false;
      this.txtExternalId.Location = new Point(633, 71);
      this.txtExternalId.Margin = new Padding(4, 5, 4, 5);
      this.txtExternalId.Name = "txtExternalId";
      this.txtExternalId.Size = new Size(193, 26);
      this.txtExternalId.TabIndex = 3;
      this.txtExternalId.TextChanged += new EventHandler(this.txtChanged_validation);
      this.txtExternalId.KeyPress += new KeyPressEventHandler(this.txtformat_validation_nospace);
      this.txtExternalId.Leave += new EventHandler(this.txtValidateAlphanumeric_Leave);
      this.txtInternalId.Location = new Point(182, 71);
      this.txtInternalId.Margin = new Padding(4, 5, 4, 5);
      this.txtInternalId.Name = "txtInternalId";
      this.txtInternalId.Size = new Size(193, 26);
      this.txtInternalId.TabIndex = 2;
      this.txtInternalId.TextChanged += new EventHandler(this.txtChanged_validation);
      this.txtInternalId.KeyPress += new KeyPressEventHandler(this.txtformat_validation_nospace);
      this.txtInternalId.Leave += new EventHandler(this.txtValidateAlphanumeric_Leave);
      this.cmbConditionType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbConditionType.FormattingEnabled = true;
      this.cmbConditionType.Location = new Point(182, 28);
      this.cmbConditionType.Margin = new Padding(15);
      this.cmbConditionType.Name = "cmbConditionType";
      this.cmbConditionType.Size = new Size(193, 28);
      this.cmbConditionType.TabIndex = 0;
      this.cmbConditionType.SelectedIndexChanged += new EventHandler(this.cmbConditionType_SelectedIndexChanged);
      this.cmbConditionType.TextChanged += new EventHandler(this.txtChanged_validation);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(488, 32);
      this.label4.Margin = new Padding(15);
      this.label4.Name = "label4";
      this.label4.Size = new Size((int) sbyte.MaxValue, 20);
      this.label4.TabIndex = 3;
      this.label4.Text = "Condition Status";
      this.lblExternalID.AutoSize = true;
      this.lblExternalID.Location = new Point(488, 71);
      this.lblExternalID.Margin = new Padding(4, 0, 4, 0);
      this.lblExternalID.Name = "lblExternalID";
      this.lblExternalID.Size = new Size(88, 20);
      this.lblExternalID.TabIndex = 2;
      this.lblExternalID.Text = "External ID";
      this.lblInternalID.AutoSize = true;
      this.lblInternalID.Location = new Point(22, 71);
      this.lblInternalID.Margin = new Padding(15);
      this.lblInternalID.Name = "lblInternalID";
      this.lblInternalID.Size = new Size(84, 20);
      this.lblInternalID.TabIndex = 1;
      this.lblInternalID.Text = "Internal ID";
      this.lblType.AutoSize = true;
      this.lblType.Location = new Point(22, 32);
      this.lblType.Margin = new Padding(15);
      this.lblType.Name = "lblType";
      this.lblType.Size = new Size(43, 20);
      this.lblType.TabIndex = 0;
      this.lblType.Text = "Type";
      this.AccessibleDescription = "Enhanced Conditions";
      this.AutoScaleDimensions = new SizeF(9f, 20f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoScroll = true;
      this.ClientSize = new Size(915, 1042);
      this.Controls.Add((Control) this.groupBoxMain);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Margin = new Padding(4, 5, 4, 5);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (EnhancedConditionTemplateDialog);
      this.ShowIcon = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Enhanced Conditions";
      this.groupBoxMain.ResumeLayout(false);
      this.groupBoxMain.PerformLayout();
      ((ISupportInitialize) this.btnEditCustomizeCondition).EndInit();
      this.grpDates.ResumeLayout(false);
      this.grpDates.PerformLayout();
      ((ISupportInitialize) this.calEndDate).EndInit();
      ((ISupportInitialize) this.calStartDate).EndInit();
      this.grpCustomizeSettings.ResumeLayout(false);
      this.grpCustomizeSettings.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
    }

    private enum ConditionStatus
    {
      Active,
      Inactive,
    }
  }
}
