// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.eFolder.DocumentTemplateDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.eFolder;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.eFolder
{
  public class DocumentTemplateDialog : Form
  {
    private DocumentTrackingSetup docSetup;
    private DocumentTemplate template;
    private OpeningCriteria openingCriteria;
    private DocumentCriteria closingCriteria;
    private PreClosingCriteria preClosingCriteria;
    private List<string> standardList;
    private List<string> customAllList;
    private List<string> customBorrowerList;
    private List<string> customCoborrowerList;
    private Sessions.Session session;
    private bool useImageAttachments;
    private const string BLACK_AND_WHITE = "Black & White";
    private const string COLOR = "Color";
    private const string AUTOMATIC = "Automatic";
    private IContainer components;
    private ComboBox cboType;
    private Label lblSource;
    private Label lblType;
    private TextBox txtName;
    private Label lblDaysTillExpire;
    private Label lblDaysTillDue;
    private Label lblName;
    private TextBox txtDaysTillExpire;
    private TextBox txtDaysTillDue;
    private ComboBox cboSource;
    private Button btnClosingCriteria;
    private CheckBox chkClosingDoc;
    private EMHelpLink helpLink;
    private Button btnCancel;
    private Button btnOK;
    private CheckBox chkOpeningDoc;
    private Button btnOpeningCriteria;
    private CheckBox chkWebCenter;
    private Label signatureTypeLbl;
    private ComboBox signatureTypeCbo;
    private GroupBox grpDocs;
    private GroupBox grpConversion;
    private ComboBox cboImageFormat;
    private Label lblConversionFormat;
    private CheckBox chkOriginalFormat;
    private Label lblDescription;
    private TextBox txtDescription;
    private CheckBox chkTPOWebcenterPortal;
    private CheckBox chkThirdParty;
    private Label lblConversionOff;
    private GroupBox grpTracking;
    private GroupBox grpAvailable;
    private Label lblSourceCoborrower;
    private ComboBox cboSourceCoborrower;
    private CheckBox chkPreClosingDoc;
    private Button btnPreClosingCriteria;

    public DocumentTemplateDialog(
      DocumentTrackingSetup docSetup,
      DocumentTemplate template,
      Sessions.Session session)
    {
      this.InitializeComponent();
      this.helpLink.AssignSession(session);
      this.docSetup = docSetup;
      this.template = template;
      this.session = session;
      this.loadTemplateInfo();
      this.loadDocConversionInfo();
      this.PromptUserIfSourceContainsArchivedForm();
    }

    private void loadTemplateInfo()
    {
      DocumentTemplate documentTemplate = this.template ?? (this.template = new DocumentTemplate(""));
      this.txtName.Text = documentTemplate.Name;
      this.txtDescription.Text = documentTemplate.Description;
      int num;
      if (documentTemplate.DaysTillDue > 0)
      {
        TextBox txtDaysTillDue = this.txtDaysTillDue;
        num = documentTemplate.DaysTillDue;
        string str = num.ToString();
        txtDaysTillDue.Text = str;
      }
      if (documentTemplate.DaysTillExpire > 0)
      {
        TextBox txtDaysTillExpire = this.txtDaysTillExpire;
        num = documentTemplate.DaysTillExpire;
        string str = num.ToString();
        txtDaysTillExpire.Text = str;
      }
      this.chkWebCenter.Checked = documentTemplate.IsWebcenter;
      this.chkTPOWebcenterPortal.Checked = documentTemplate.IsTPOWebcenterPortal;
      this.chkThirdParty.Checked = documentTemplate.IsThirdPartyDoc;
      if (Epass.IsEpassDoc(documentTemplate.Name))
      {
        this.txtName.ReadOnly = true;
        this.cboType.Items.Add((object) "Settlement Service");
        this.cboType.Text = "Settlement Service";
      }
      else
      {
        this.cboType.Items.Add((object) "Standard Form");
        this.cboType.Items.Add((object) "Custom Form");
        this.cboType.Items.Add((object) "Needed");
        this.signatureTypeCbo.Items.Add((object) "eSignable");
        this.signatureTypeCbo.Items.Add((object) "Wet Sign Only");
        this.signatureTypeCbo.Items.Add((object) "Informational");
        this.cboType.Text = documentTemplate.SourceType;
        if (!(documentTemplate.SourceType != "Needed"))
          return;
        if (documentTemplate.SourceType != "Borrower Specific Custom Form")
        {
          this.cboSource.Text = documentTemplate.Source;
        }
        else
        {
          this.cboSource.Text = documentTemplate.SourceBorrower;
          this.cboSourceCoborrower.Text = documentTemplate.SourceCoborrower;
        }
        this.chkOpeningDoc.Checked = documentTemplate.OpeningDocument;
        this.chkClosingDoc.Checked = documentTemplate.ClosingDocument;
        this.chkPreClosingDoc.Checked = documentTemplate.PreClosingDocument;
        this.openingCriteria = documentTemplate.OpeningCriteria;
        this.closingCriteria = documentTemplate.ClosingCriteria;
        this.preClosingCriteria = documentTemplate.PreClosingCriteria;
        this.signatureTypeCbo.Text = documentTemplate.SignatureType;
      }
    }

    private void loadDocConversionInfo()
    {
      this.useImageAttachments = this.session.ConfigurationManager.GetImageAttachmentSettings().UseImageAttachments;
      this.chkOriginalFormat.Checked = this.template.SaveOriginalFormat;
      if (this.cboImageFormat.Items.Count == 0)
      {
        this.cboImageFormat.Items.Add((object) new FieldOption("Color", "Automatic"));
        this.cboImageFormat.Items.Add((object) new FieldOption("Black & White", "Black & White"));
      }
      this.cboImageFormat.SelectedIndex = this.getImageFormatSelection();
      this.lblConversionOff.Visible = !this.useImageAttachments;
    }

    private int getImageFormatSelection()
    {
      if (this.cboImageFormat.Items.Count == 0)
        return -1;
      string empty = string.Empty;
      string str = !this.template.ConversionType.Equals((object) ImageConversionType.BlackAndWhite) ? "Automatic" : "Black & White";
      for (int index = 0; index < this.cboImageFormat.Items.Count; ++index)
      {
        if (((FieldOption) this.cboImageFormat.Items[index]).Value == str)
          return index;
      }
      return 0;
    }

    private bool saveTemplateInfo()
    {
      DocumentTrackingSetup docSetup = this.docSetup;
      DocumentTemplate template = this.template;
      string name = this.txtName.Text.Trim();
      if (name == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Invalid Document Name", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      List<string> stringList = new List<string>()
      {
        "VOL",
        "VOR",
        "VOE",
        "VOM",
        "VOD"
      };
      bool flag = template.Name != name && docSetup.Contains(name);
      if (stringList.Contains(name) | flag)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The '" + name + "' name has been used already. Please enter a different one.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      if (template.Name != name)
        template = this.template = new DocumentTemplate(template.Guid, name);
      string text = this.cboType.Text;
      if (text == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Invalid Document Type", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      string str1 = string.Empty;
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      if (text == "Standard Form" || text == "Custom Form")
      {
        str1 = this.cboSource.Text;
        if (str1 == string.Empty)
        {
          if (!this.PromptUserIfSourceContainsArchivedForm())
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "Invalid Document Source", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          return false;
        }
      }
      int num1 = Utils.ParseInt((object) this.txtDaysTillDue.Text, 0);
      if (num1 < 0)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "Invalid Days to Receive", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      int num3 = Utils.ParseInt((object) this.txtDaysTillExpire.Text, 0);
      if (num3 < 0)
      {
        int num4 = (int) Utils.Dialog((IWin32Window) this, "Invalid Days to Expire", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      string str2 = ((FieldOption) this.cboImageFormat.SelectedItem).Value;
      if (this.useImageAttachments && str2 == "Color" && Utils.Dialog((IWin32Window) this, "Color conversion will be slower and consume more space. Do you wish to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
        return false;
      template.Description = this.txtDescription.Text;
      template.SourceType = text;
      template.Source = str1;
      template.SourceBorrower = empty1;
      template.SourceCoborrower = empty2;
      template.DaysTillDue = num1;
      template.DaysTillExpire = num3;
      template.IsWebcenter = this.chkWebCenter.Checked;
      template.IsTPOWebcenterPortal = this.chkTPOWebcenterPortal.Checked;
      template.IsThirdPartyDoc = this.chkThirdParty.Checked;
      template.OpeningDocument = this.chkOpeningDoc.Checked;
      template.OpeningCriteria = this.openingCriteria;
      template.ClosingDocument = this.chkClosingDoc.Checked;
      template.ClosingCriteria = this.closingCriteria;
      template.PreClosingDocument = this.chkPreClosingDoc.Checked;
      template.PreClosingCriteria = this.preClosingCriteria;
      template.SignatureType = this.signatureTypeCbo.Text;
      template.ConversionType = !str2.Equals("Black & White") ? ImageConversionType.Automatic : ImageConversionType.BlackAndWhite;
      template.SaveOriginalFormat = this.chkOriginalFormat.Checked;
      if (!docSetup.Contains(template))
      {
        DocumentTemplate byId = docSetup.GetByID(template.Guid);
        if (byId != null)
          docSetup.Remove(byId);
        docSetup.Add(template);
      }
      this.session.ConfigurationManager.UpsertDocumentTrackingTemplate(this.docSetup, template.Guid);
      return true;
    }

    private void cboType_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.cboSource.Items.Clear();
      this.cboSourceCoborrower.Items.Clear();
      switch ((string) this.cboType.SelectedItem)
      {
        case "Standard Form":
          this.loadStandardFormSources();
          break;
        case "Custom Form":
          this.loadCustomFormSources();
          break;
        case "Needed":
          this.loadNeededSources();
          break;
        case "Settlement Service":
          this.loadSettlementServiceSources();
          break;
      }
    }

    private void loadStandardFormSources()
    {
      if (this.standardList == null)
      {
        PrintFormList printFormList = PrintFormList.Parse(this.session.GetFormConfigFile(FormConfigFile.OutFormAndFileMapping), this.session.EncompassEdition);
        PrintGroupList printGroupList = PrintGroupList.Parse(this.session.GetFormConfigFile(FormConfigFile.FormGroupList), this.session.EncompassEdition);
        if (printGroupList != null)
        {
          PrintGroup groupByName = printGroupList.GetGroupByName("Archived Forms");
          this.standardList = new List<string>();
          foreach (PrintForm printForm in printFormList)
          {
            if (!string.IsNullOrEmpty(printForm.Source) && !this.standardList.Contains(printForm.Source))
            {
              if (groupByName != null && groupByName.archivedForms != null)
              {
                if (!groupByName.archivedForms.ContainsKey(printForm.FormID))
                  this.standardList.Add(printForm.Source);
                else if (groupByName.archivedForms[printForm.FormID] != null && groupByName.archivedForms[printForm.FormID].SuppressArchivedPrompt)
                  this.standardList.Add(printForm.Source);
              }
              else
                this.standardList.Add(printForm.Source);
            }
          }
        }
      }
      this.lblSource.Text = "Source";
      this.lblSource.Visible = true;
      this.cboSource.Items.AddRange((object[]) this.standardList.ToArray());
      this.cboSource.Visible = true;
      this.lblSourceCoborrower.Visible = false;
      this.cboSourceCoborrower.Visible = false;
      this.grpDocs.Enabled = true;
      this.signatureTypeCbo.Text = "eSignable";
    }

    private bool PromptUserIfSourceContainsArchivedForm()
    {
      bool flag = false;
      if (this.template.SourceType == "Standard Form" && !string.IsNullOrEmpty(this.template.Source))
      {
        PrintGroupList printGroupList = PrintGroupList.Parse(this.session.GetFormConfigFile(FormConfigFile.FormGroupList), this.session.EncompassEdition);
        if (printGroupList == null)
          return flag;
        PrintGroup groupByName = printGroupList.GetGroupByName("Archived Forms");
        if (groupByName == null)
          return flag;
        PrintForm formBySourceName = this.GetFormBySourceName(this.template.Source);
        if (formBySourceName != null && groupByName.archivedForms != null && groupByName.archivedForms.ContainsKey(formBySourceName.FormID))
        {
          ArchivedFormDetails archivedFormDetails;
          groupByName.archivedForms.TryGetValue(formBySourceName.FormID, out archivedFormDetails);
          if (archivedFormDetails != null)
          {
            List<string> replacementFormNames = archivedFormDetails.ReplacementFormNames;
            if (!archivedFormDetails.SuppressArchivedPrompt && replacementFormNames != null)
            {
              if (replacementFormNames.Count == 0)
              {
                int num = (int) Utils.Dialog((IWin32Window) this, "This document is utilizing a source form titled '" + formBySourceName.Source + "', this source form has been archived. There is no replacement for this form.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                flag = true;
              }
              else if (replacementFormNames.Count == 1)
              {
                PrintForm formByFormId = this.GetFormByFormId(replacementFormNames[0]);
                int num = (int) Utils.Dialog((IWin32Window) this, "This document is utilizing a source form titled '" + formBySourceName.Source + "', this source form has been archived. The updated version of this form is titled '" + formByFormId.Source + "', please update your document's source.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                flag = true;
              }
              else if (replacementFormNames.Count > 1)
              {
                string str = (string) null;
                for (int index = 0; index < replacementFormNames.Count; ++index)
                {
                  PrintForm formByFormId = this.GetFormByFormId(replacementFormNames[index]);
                  if (formByFormId != null)
                    str = index != replacementFormNames.Count - 1 ? str + formByFormId.Source + "' OR '" : str + formByFormId.Source;
                }
                if (str != null)
                {
                  int num = (int) Utils.Dialog((IWin32Window) this, "This document is utilizing a source form titled '" + formBySourceName.Source + "', this source form has been archived. The updated versions of this form are titled '" + str + "', please update your document's source.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  flag = true;
                }
              }
            }
          }
        }
      }
      return flag;
    }

    private PrintForm GetFormByFormId(string formName)
    {
      PrintFormList printFormList = PrintFormList.Parse(this.session.GetFormConfigFile(FormConfigFile.OutFormAndFileMapping), this.session.EncompassEdition);
      PrintForm formByFormId = (PrintForm) null;
      foreach (PrintForm printForm in printFormList)
      {
        if (printForm.FormID == formName)
        {
          formByFormId = printForm;
          break;
        }
      }
      return formByFormId;
    }

    private PrintForm GetFormBySourceName(string source)
    {
      PrintFormList printFormList = PrintFormList.Parse(this.session.GetFormConfigFile(FormConfigFile.OutFormAndFileMapping), this.session.EncompassEdition);
      PrintForm formBySourceName = (PrintForm) null;
      if (!string.IsNullOrEmpty(source))
      {
        foreach (PrintForm printForm in printFormList)
        {
          if (printForm.Source == source)
          {
            formBySourceName = printForm;
            break;
          }
        }
      }
      return formBySourceName;
    }

    private void loadCustomFormLists()
    {
      FileSystemEntry[] lettersRecursively = this.session.ConfigurationManager.GetCustomLettersRecursively(CustomLetterType.Generic, FileSystemEntry.PublicRoot);
      CustomFormDetail[] customFormDetails = this.session.ConfigurationManager.GetCustomFormDetails();
      this.customAllList = new List<string>();
      this.customBorrowerList = new List<string>();
      this.customCoborrowerList = new List<string>();
      foreach (FileSystemEntry fileSystemEntry in lettersRecursively)
      {
        if (fileSystemEntry.Type == FileSystemEntry.Types.File && (fileSystemEntry.Name.ToLower().EndsWith(".doc") || fileSystemEntry.Name.ToLower().EndsWith(".rtf") || fileSystemEntry.Name.ToLower().EndsWith(".docx")))
        {
          string str = fileSystemEntry.ToString();
          ForBorrowerType forBorrowerType = ForBorrowerType.All;
          foreach (CustomFormDetail customFormDetail in customFormDetails)
          {
            if (customFormDetail.Source == str)
              forBorrowerType = customFormDetail.IntendedFor;
          }
          switch (forBorrowerType)
          {
            case ForBorrowerType.Borrower:
              this.customBorrowerList.Add(str);
              continue;
            case ForBorrowerType.CoBorrower:
              this.customCoborrowerList.Add(str);
              continue;
            case ForBorrowerType.All:
              this.customAllList.Add(str);
              continue;
            default:
              continue;
          }
        }
      }
    }

    private void loadCustomFormSources()
    {
      if (this.customAllList == null)
        this.loadCustomFormLists();
      this.lblSource.Text = "Source";
      this.lblSource.Visible = true;
      this.cboSource.Items.AddRange((object[]) this.customAllList.ToArray());
      this.cboSource.Visible = true;
      this.lblSourceCoborrower.Visible = false;
      this.cboSourceCoborrower.Visible = false;
      this.grpDocs.Enabled = true;
      this.signatureTypeCbo.Text = "eSignable";
    }

    private void loadNeededSources()
    {
      this.lblSource.Visible = false;
      this.cboSource.Visible = false;
      this.lblSourceCoborrower.Visible = false;
      this.cboSourceCoborrower.Visible = false;
      this.grpDocs.Enabled = false;
      this.chkOpeningDoc.Checked = false;
      this.chkClosingDoc.Checked = false;
      this.chkPreClosingDoc.Checked = false;
      this.signatureTypeCbo.SelectedIndex = -1;
    }

    private void loadSettlementServiceSources()
    {
      this.lblSource.Visible = false;
      this.cboSource.Visible = false;
      this.lblSourceCoborrower.Visible = false;
      this.cboSourceCoborrower.Visible = false;
      this.grpDocs.Enabled = false;
      this.chkOpeningDoc.Checked = false;
      this.chkClosingDoc.Checked = false;
      this.chkPreClosingDoc.Checked = false;
      this.signatureTypeCbo.SelectedIndex = -1;
    }

    private void chkOpeningDoc_CheckedChanged(object sender, EventArgs e)
    {
      this.btnOpeningCriteria.Enabled = this.chkOpeningDoc.Checked;
    }

    private void btnOpeningCriteria_Click(object sender, EventArgs e)
    {
      using (OpeningCriteriaDialog openingCriteriaDialog = new OpeningCriteriaDialog(this.openingCriteria, this.session))
      {
        if (openingCriteriaDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.openingCriteria = openingCriteriaDialog.Criteria;
      }
    }

    private void chkClosingDoc_CheckedChanged(object sender, EventArgs e)
    {
      this.btnClosingCriteria.Enabled = this.chkClosingDoc.Checked;
    }

    private void btnClosingCriteria_Click(object sender, EventArgs e)
    {
      using (DocumentCriteriaDialog documentCriteriaDialog = new DocumentCriteriaDialog(this.closingCriteria, this.session))
      {
        if (documentCriteriaDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.closingCriteria = documentCriteriaDialog.Criteria;
      }
    }

    private void chkPreClosingDoc_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkPreClosingDoc.Checked)
      {
        if (string.IsNullOrEmpty(this.txtDaysTillDue.Text))
          this.txtDaysTillDue.Text = "7";
        if (string.IsNullOrEmpty(this.txtDaysTillExpire.Text))
          this.txtDaysTillExpire.Text = "30";
      }
      this.btnPreClosingCriteria.Enabled = this.chkPreClosingDoc.Checked;
    }

    private void btnPreClosingCriteria_Click(object sender, EventArgs e)
    {
      using (PreClosingCriteriaDialog closingCriteriaDialog = new PreClosingCriteriaDialog(this.preClosingCriteria, this.session))
      {
        if (closingCriteriaDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.preClosingCriteria = closingCriteriaDialog.Criteria;
      }
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (!this.saveTemplateInfo())
        return;
      this.DialogResult = DialogResult.OK;
    }

    private void setSignatureType()
    {
      string text = this.cboSource.Text;
      switch (this.cboType.Text)
      {
        case "Custom Form":
        case "Borrower Specific Custom Form":
          this.signatureTypeCbo.Text = "eSignable";
          break;
        case "Standard Form":
          switch (text)
          {
            case "IRS4506 - Copy Request":
            case "IRS4506T - Trans Request":
            case "IRS8821 - Tax Info Auth":
              this.signatureTypeCbo.Text = "Wet Sign Only";
              return;
            case "HUD Settlement Cost Booklet":
              this.signatureTypeCbo.Text = "Informational";
              return;
            default:
              this.signatureTypeCbo.Text = "eSignable";
              return;
          }
      }
    }

    private void cboSource_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.setSignatureType();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.cboType = new ComboBox();
      this.lblSource = new Label();
      this.lblType = new Label();
      this.txtName = new TextBox();
      this.lblDaysTillExpire = new Label();
      this.lblDaysTillDue = new Label();
      this.lblName = new Label();
      this.txtDaysTillExpire = new TextBox();
      this.txtDaysTillDue = new TextBox();
      this.cboSource = new ComboBox();
      this.btnClosingCriteria = new Button();
      this.chkClosingDoc = new CheckBox();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.chkOpeningDoc = new CheckBox();
      this.btnOpeningCriteria = new Button();
      this.chkWebCenter = new CheckBox();
      this.signatureTypeLbl = new Label();
      this.signatureTypeCbo = new ComboBox();
      this.grpDocs = new GroupBox();
      this.grpConversion = new GroupBox();
      this.lblConversionOff = new Label();
      this.cboImageFormat = new ComboBox();
      this.lblConversionFormat = new Label();
      this.chkOriginalFormat = new CheckBox();
      this.lblDescription = new Label();
      this.txtDescription = new TextBox();
      this.chkTPOWebcenterPortal = new CheckBox();
      this.chkThirdParty = new CheckBox();
      this.helpLink = new EMHelpLink();
      this.grpTracking = new GroupBox();
      this.grpAvailable = new GroupBox();
      this.lblSourceCoborrower = new Label();
      this.cboSourceCoborrower = new ComboBox();
      this.chkPreClosingDoc = new CheckBox();
      this.btnPreClosingCriteria = new Button();
      this.grpDocs.SuspendLayout();
      this.grpConversion.SuspendLayout();
      this.grpTracking.SuspendLayout();
      this.grpAvailable.SuspendLayout();
      this.SuspendLayout();
      this.cboType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboType.Location = new Point(124, 96);
      this.cboType.MaxDropDownItems = 15;
      this.cboType.Name = "cboType";
      this.cboType.Size = new Size(400, 22);
      this.cboType.TabIndex = 5;
      this.cboType.SelectedIndexChanged += new EventHandler(this.cboType_SelectedIndexChanged);
      this.lblSource.AutoSize = true;
      this.lblSource.Location = new Point(12, 128);
      this.lblSource.Name = "lblSource";
      this.lblSource.Size = new Size(42, 14);
      this.lblSource.TabIndex = 6;
      this.lblSource.Text = "Source";
      this.lblType.AutoSize = true;
      this.lblType.Location = new Point(12, 100);
      this.lblType.Name = "lblType";
      this.lblType.Size = new Size(30, 14);
      this.lblType.TabIndex = 4;
      this.lblType.Text = "Type";
      this.txtName.Location = new Point(124, 9);
      this.txtName.Name = "txtName";
      this.txtName.Size = new Size(400, 20);
      this.txtName.TabIndex = 1;
      this.lblDaysTillExpire.AutoSize = true;
      this.lblDaysTillExpire.Location = new Point(12, 49);
      this.lblDaysTillExpire.Name = "lblDaysTillExpire";
      this.lblDaysTillExpire.Size = new Size(77, 14);
      this.lblDaysTillExpire.TabIndex = 2;
      this.lblDaysTillExpire.Text = "Days to Expire";
      this.lblDaysTillDue.AutoSize = true;
      this.lblDaysTillDue.Location = new Point(12, 24);
      this.lblDaysTillDue.Name = "lblDaysTillDue";
      this.lblDaysTillDue.Size = new Size(86, 14);
      this.lblDaysTillDue.TabIndex = 0;
      this.lblDaysTillDue.Text = "Days to Receive";
      this.lblName.AutoSize = true;
      this.lblName.Location = new Point(12, 12);
      this.lblName.Name = "lblName";
      this.lblName.Size = new Size(34, 14);
      this.lblName.TabIndex = 0;
      this.lblName.Text = "Name";
      this.txtDaysTillExpire.Location = new Point(104, 46);
      this.txtDaysTillExpire.Name = "txtDaysTillExpire";
      this.txtDaysTillExpire.Size = new Size(80, 20);
      this.txtDaysTillExpire.TabIndex = 3;
      this.txtDaysTillDue.Location = new Point(104, 21);
      this.txtDaysTillDue.Name = "txtDaysTillDue";
      this.txtDaysTillDue.Size = new Size(80, 20);
      this.txtDaysTillDue.TabIndex = 1;
      this.cboSource.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboSource.Location = new Point(124, 124);
      this.cboSource.MaxDropDownItems = 15;
      this.cboSource.Name = "cboSource";
      this.cboSource.Size = new Size(400, 22);
      this.cboSource.Sorted = true;
      this.cboSource.TabIndex = 7;
      this.cboSource.SelectedIndexChanged += new EventHandler(this.cboSource_SelectedIndexChanged);
      this.btnClosingCriteria.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnClosingCriteria.Enabled = false;
      this.btnClosingCriteria.Location = new Point(432, 44);
      this.btnClosingCriteria.Name = "btnClosingCriteria";
      this.btnClosingCriteria.Size = new Size(70, 22);
      this.btnClosingCriteria.TabIndex = 3;
      this.btnClosingCriteria.Text = "Criteria...";
      this.btnClosingCriteria.Click += new EventHandler(this.btnClosingCriteria_Click);
      this.chkClosingDoc.AutoSize = true;
      this.chkClosingDoc.Location = new Point(12, 48);
      this.chkClosingDoc.Name = "chkClosingDoc";
      this.chkClosingDoc.Size = new Size(271, 18);
      this.chkClosingDoc.TabIndex = 2;
      this.chkClosingDoc.Text = "Add this document to Encompass Closer packages";
      this.chkClosingDoc.CheckedChanged += new EventHandler(this.chkClosingDoc_CheckedChanged);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(452, 553);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 16;
      this.btnCancel.Text = "Cancel";
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(372, 553);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 22);
      this.btnOK.TabIndex = 15;
      this.btnOK.Text = "OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.chkOpeningDoc.AutoSize = true;
      this.chkOpeningDoc.Location = new Point(12, 24);
      this.chkOpeningDoc.Name = "chkOpeningDoc";
      this.chkOpeningDoc.Size = new Size(238, 18);
      this.chkOpeningDoc.TabIndex = 0;
      this.chkOpeningDoc.Text = "Add this document to eDisclosure packages";
      this.chkOpeningDoc.CheckedChanged += new EventHandler(this.chkOpeningDoc_CheckedChanged);
      this.btnOpeningCriteria.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnOpeningCriteria.Enabled = false;
      this.btnOpeningCriteria.Location = new Point(432, 20);
      this.btnOpeningCriteria.Name = "btnOpeningCriteria";
      this.btnOpeningCriteria.Size = new Size(70, 22);
      this.btnOpeningCriteria.TabIndex = 1;
      this.btnOpeningCriteria.Text = "Criteria...";
      this.btnOpeningCriteria.Click += new EventHandler(this.btnOpeningCriteria_Click);
      this.chkWebCenter.AutoSize = true;
      this.chkWebCenter.Location = new Point(12, 24);
      this.chkWebCenter.Name = "chkWebCenter";
      this.chkWebCenter.Size = new Size(79, 18);
      this.chkWebCenter.TabIndex = 0;
      this.chkWebCenter.Text = "Webcenter";
      this.signatureTypeLbl.AutoSize = true;
      this.signatureTypeLbl.Location = new Point(12, 104);
      this.signatureTypeLbl.Name = "signatureTypeLbl";
      this.signatureTypeLbl.Size = new Size(79, 14);
      this.signatureTypeLbl.TabIndex = 4;
      this.signatureTypeLbl.Text = "Signature Type";
      this.signatureTypeCbo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.signatureTypeCbo.DropDownStyle = ComboBoxStyle.DropDownList;
      this.signatureTypeCbo.Location = new Point(104, 100);
      this.signatureTypeCbo.MaxDropDownItems = 15;
      this.signatureTypeCbo.Name = "signatureTypeCbo";
      this.signatureTypeCbo.Size = new Size(247, 22);
      this.signatureTypeCbo.TabIndex = 5;
      this.grpDocs.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.grpDocs.Controls.Add((Control) this.chkPreClosingDoc);
      this.grpDocs.Controls.Add((Control) this.btnPreClosingCriteria);
      this.grpDocs.Controls.Add((Control) this.chkOpeningDoc);
      this.grpDocs.Controls.Add((Control) this.signatureTypeLbl);
      this.grpDocs.Controls.Add((Control) this.chkClosingDoc);
      this.grpDocs.Controls.Add((Control) this.signatureTypeCbo);
      this.grpDocs.Controls.Add((Control) this.btnClosingCriteria);
      this.grpDocs.Controls.Add((Control) this.btnOpeningCriteria);
      this.grpDocs.Location = new Point(12, 409);
      this.grpDocs.Name = "grpDocs";
      this.grpDocs.Size = new Size(512, 138);
      this.grpDocs.TabIndex = 13;
      this.grpDocs.TabStop = false;
      this.grpDocs.Text = "Encompass Docs Service";
      this.grpConversion.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.grpConversion.Controls.Add((Control) this.lblConversionOff);
      this.grpConversion.Controls.Add((Control) this.cboImageFormat);
      this.grpConversion.Controls.Add((Control) this.lblConversionFormat);
      this.grpConversion.Controls.Add((Control) this.chkOriginalFormat);
      this.grpConversion.Location = new Point(12, 297);
      this.grpConversion.Name = "grpConversion";
      this.grpConversion.Size = new Size(512, 100);
      this.grpConversion.TabIndex = 12;
      this.grpConversion.TabStop = false;
      this.grpConversion.Text = "Conversion Preferences";
      this.lblConversionOff.AutoSize = true;
      this.lblConversionOff.Location = new Point(11, 21);
      this.lblConversionOff.Name = "lblConversionOff";
      this.lblConversionOff.Size = new Size(358, 14);
      this.lblConversionOff.TabIndex = 0;
      this.lblConversionOff.Text = "Document Conversion Off - Settings only apply with conversion enabled.";
      this.cboImageFormat.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboImageFormat.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboImageFormat.FormattingEnabled = true;
      this.cboImageFormat.Location = new Point(116, 68);
      this.cboImageFormat.Name = "cboImageFormat";
      this.cboImageFormat.Size = new Size(159, 22);
      this.cboImageFormat.TabIndex = 3;
      this.lblConversionFormat.AutoSize = true;
      this.lblConversionFormat.Location = new Point(12, 72);
      this.lblConversionFormat.Name = "lblConversionFormat";
      this.lblConversionFormat.Size = new Size(98, 14);
      this.lblConversionFormat.TabIndex = 2;
      this.lblConversionFormat.Text = "Conversion Format";
      this.chkOriginalFormat.AutoSize = true;
      this.chkOriginalFormat.Location = new Point(12, 44);
      this.chkOriginalFormat.Name = "chkOriginalFormat";
      this.chkOriginalFormat.Size = new Size(162, 18);
      this.chkOriginalFormat.TabIndex = 1;
      this.chkOriginalFormat.Text = "Keep copy of original format";
      this.chkOriginalFormat.UseVisualStyleBackColor = true;
      this.lblDescription.AutoSize = true;
      this.lblDescription.Location = new Point(13, 40);
      this.lblDescription.Name = "lblDescription";
      this.lblDescription.Size = new Size(61, 14);
      this.lblDescription.TabIndex = 2;
      this.lblDescription.Text = "Description";
      this.txtDescription.Location = new Point(124, 37);
      this.txtDescription.Multiline = true;
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.ScrollBars = ScrollBars.Vertical;
      this.txtDescription.Size = new Size(400, 51);
      this.txtDescription.TabIndex = 3;
      this.chkTPOWebcenterPortal.AutoSize = true;
      this.chkTPOWebcenterPortal.Location = new Point(12, 47);
      this.chkTPOWebcenterPortal.Name = "chkTPOWebcenterPortal";
      this.chkTPOWebcenterPortal.Size = new Size(133, 18);
      this.chkTPOWebcenterPortal.TabIndex = 1;
      this.chkTPOWebcenterPortal.Text = "TPO Portal";
      this.chkThirdParty.AutoSize = true;
      this.chkThirdParty.Location = new Point(12, 71);
      this.chkThirdParty.Name = "chkThirdParty";
      this.chkThirdParty.Size = new Size(200, 18);
      this.chkThirdParty.TabIndex = 2;
      this.chkThirdParty.Text = "EDM Lenders (Send Files to Lender)";
      this.helpLink.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.helpLink.BackColor = Color.Transparent;
      this.helpLink.Cursor = Cursors.Hand;
      this.helpLink.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.helpLink.HelpTag = "Documents";
      this.helpLink.Location = new Point(12, 557);
      this.helpLink.Name = "helpLink";
      this.helpLink.Size = new Size(90, 17);
      this.helpLink.TabIndex = 14;
      this.grpTracking.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.grpTracking.Controls.Add((Control) this.lblDaysTillDue);
      this.grpTracking.Controls.Add((Control) this.txtDaysTillDue);
      this.grpTracking.Controls.Add((Control) this.txtDaysTillExpire);
      this.grpTracking.Controls.Add((Control) this.lblDaysTillExpire);
      this.grpTracking.Location = new Point(12, 189);
      this.grpTracking.Name = "grpTracking";
      this.grpTracking.Size = new Size(248, 96);
      this.grpTracking.TabIndex = 10;
      this.grpTracking.TabStop = false;
      this.grpTracking.Text = "Tracking";
      this.grpAvailable.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.grpAvailable.Controls.Add((Control) this.chkWebCenter);
      this.grpAvailable.Controls.Add((Control) this.chkTPOWebcenterPortal);
      this.grpAvailable.Controls.Add((Control) this.chkThirdParty);
      this.grpAvailable.Location = new Point(276, 189);
      this.grpAvailable.Name = "grpAvailable";
      this.grpAvailable.Size = new Size(248, 96);
      this.grpAvailable.TabIndex = 11;
      this.grpAvailable.TabStop = false;
      this.grpAvailable.Text = "Available";
      this.lblSourceCoborrower.AutoSize = true;
      this.lblSourceCoborrower.Location = new Point(12, 156);
      this.lblSourceCoborrower.Name = "lblSourceCoborrower";
      this.lblSourceCoborrower.Size = new Size(109, 14);
      this.lblSourceCoborrower.TabIndex = 8;
      this.lblSourceCoborrower.Text = "Co-Borrower Source";
      this.lblSourceCoborrower.Visible = false;
      this.cboSourceCoborrower.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboSourceCoborrower.Location = new Point(124, 152);
      this.cboSourceCoborrower.MaxDropDownItems = 15;
      this.cboSourceCoborrower.Name = "cboSourceCoborrower";
      this.cboSourceCoborrower.Size = new Size(400, 22);
      this.cboSourceCoborrower.Sorted = true;
      this.cboSourceCoborrower.TabIndex = 9;
      this.cboSourceCoborrower.Visible = false;
      this.chkPreClosingDoc.AutoSize = true;
      this.chkPreClosingDoc.Location = new Point(12, 72);
      this.chkPreClosingDoc.Name = "chkPreClosingDoc";
      this.chkPreClosingDoc.Size = new Size(236, 18);
      this.chkPreClosingDoc.TabIndex = 6;
      this.chkPreClosingDoc.Text = "Add this document to Pre-Closing packages";
      this.chkPreClosingDoc.CheckedChanged += new EventHandler(this.chkPreClosingDoc_CheckedChanged);
      this.btnPreClosingCriteria.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnPreClosingCriteria.Enabled = false;
      this.btnPreClosingCriteria.Location = new Point(432, 68);
      this.btnPreClosingCriteria.Name = "btnPreClosingCriteria";
      this.btnPreClosingCriteria.Size = new Size(70, 22);
      this.btnPreClosingCriteria.TabIndex = 7;
      this.btnPreClosingCriteria.Text = "Criteria...";
      this.btnPreClosingCriteria.Click += new EventHandler(this.btnPreClosingCriteria_Click);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(535, 582);
      this.Controls.Add((Control) this.lblSourceCoborrower);
      this.Controls.Add((Control) this.cboSourceCoborrower);
      this.Controls.Add((Control) this.grpAvailable);
      this.Controls.Add((Control) this.grpTracking);
      this.Controls.Add((Control) this.txtDescription);
      this.Controls.Add((Control) this.lblDescription);
      this.Controls.Add((Control) this.grpConversion);
      this.Controls.Add((Control) this.grpDocs);
      this.Controls.Add((Control) this.helpLink);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.cboType);
      this.Controls.Add((Control) this.lblSource);
      this.Controls.Add((Control) this.lblType);
      this.Controls.Add((Control) this.txtName);
      this.Controls.Add((Control) this.lblName);
      this.Controls.Add((Control) this.cboSource);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (DocumentTemplateDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Document";
      this.grpDocs.ResumeLayout(false);
      this.grpDocs.PerformLayout();
      this.grpConversion.ResumeLayout(false);
      this.grpConversion.PerformLayout();
      this.grpTracking.ResumeLayout(false);
      this.grpTracking.PerformLayout();
      this.grpAvailable.ResumeLayout(false);
      this.grpAvailable.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
