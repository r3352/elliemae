// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExportTemplateSetupControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class ExportTemplateSetupControl : SettingsUserControl
  {
    private Sessions.Session session;
    private TemplateIFSExplorer ifsExplorer;
    private DocumentExportTemplate currentTemplate;
    private FileSystemEntry currentEntry;
    private string defaultTemplateName;
    private bool readOnly;
    private const string DEFAULT_SELECTED_VALUE = "Yes";
    private IContainer components;
    private GroupContainer grpTemplates;
    private GridView gvTemplates;
    private FlowLayoutPanel pnlTemplateControls;
    private StandardIconButton btnDeleteTemplate;
    private StandardIconButton btnDuplicateTemplate;
    private StandardIconButton btnAddTemplate;
    private ToolTip toolTip1;
    private VerticalSeparator verticalSeparator1;
    private Button btnSetAsDefault;
    private SaveFileDialog sfdExport;
    private OpenFileDialog ofdImport;
    private GroupContainer grpTemplateDetails;
    private FlowLayoutPanel pnlDetailControls;
    private StandardIconButton btnResetDetails;
    private StandardIconButton btnSaveDetails;
    private VerticalSeparator verticalSeparator2;
    private StandardIconButton btnRemoveDocument;
    private StandardIconButton btnMoveDocumentDown;
    private StandardIconButton btnMoveDocumentUp;
    private StandardIconButton btnAddDocuments;
    private StandardIconButton btnExportTemplate;
    private StandardIconButton btnImportTemplate;
    private Panel pnlDetails;
    private TextBox txtConfirmPass;
    private Label newLbl;
    private TextBox txtPass;
    private Label confirmLbl;
    private RadioButton rdoZip;
    private RadioButton rdoPDF;
    private CheckBox chkPassword;
    private Label label2;
    private ComboBox cboAnnotations;
    private Label label1;
    private ComboBox cboStackingOrder;
    private CheckBox chkExportLocation;
    private Label label3;
    private TextBox txtExportLocation;
    private Button btnBrowse;
    private Label label4;
    private ComboBox cboFileName3;
    private ComboBox cboFileName2;
    private ComboBox cboFileName1;
    private Label label5;
    private TextBox txtFileName3;
    private TextBox txtFileName2;
    private TextBox txtFileName1;
    private Label lblFileName;
    private Label label6;

    public string[] SelectedExportTemplateNames
    {
      get
      {
        return this.gvTemplates.SelectedItems.Count == 0 ? (string[]) null : this.gvTemplates.SelectedItems.Select<GVItem, string>((Func<GVItem, string>) (item => item.Text)).ToArray<string>();
      }
      set
      {
        if (value == null || ((IEnumerable<string>) value).Count<string>() == 0)
          return;
        foreach (GVItem gvItem in this.gvTemplates.Items.Where<GVItem>((Func<GVItem, bool>) (item => ((IEnumerable<string>) value).Contains<string>(item.SubItems[0].Text))))
          gvItem.Selected = true;
      }
    }

    public string[] SelectedStackingTemplateNames
    {
      get
      {
        return this.SelectedExportTemplateNames.Length == 0 ? (string[]) null : this.session.ConfigurationManager.GetStackingOrderTemplateNamesByExportTemplates(this.SelectedExportTemplateNames);
      }
    }

    public ExportTemplateSetupControl(SetUpContainer container, Sessions.Session session)
      : base(container)
    {
      this.session = session;
      this.InitializeComponent();
      if (this.DesignMode)
        return;
      this.clearTemplate();
      FileSystemEntry[] templateDirEntries = this.session.ConfigurationManager.GetFilteredTemplateDirEntries(TemplateSettingsType.StackingOrder, FileSystemEntry.PublicRoot);
      List<StackingOrderSetTemplate> orderSetTemplateList = new List<StackingOrderSetTemplate>();
      foreach (FileSystemEntry fileEntry in templateDirEntries)
      {
        StackingOrderSetTemplate templateSettings = (StackingOrderSetTemplate) this.session.ConfigurationManager.GetTemplateSettings(TemplateSettingsType.StackingOrder, fileEntry);
        if (templateSettings != null)
          orderSetTemplateList.Add(templateSettings);
      }
      this.cboStackingOrder.ValueMember = "DocumentStackingTemplateID";
      this.cboStackingOrder.DisplayMember = "TemplateName";
      this.cboStackingOrder.DataSource = (object) orderSetTemplateList;
      this.cboStackingOrder.SelectedIndex = -1;
      this.gvTemplates.Sort(0, SortOrder.Ascending);
      if (container == null)
        this.gvTemplates.AllowMultiselect = true;
      this.ifsExplorer = new TemplateIFSExplorer(this.session, TemplateSettingsType.DocumentExportTemplate);
      this.ifsExplorer.Init(FileSystemEntry.PublicRoot, true);
      this.getCurrentDefaultTemplate();
      this.loadTemplates();
      this.setDirtyFlag(false);
    }

    public bool ReadOnly
    {
      get => this.readOnly;
      set
      {
        this.readOnly = value;
        this.setEditControlStates();
      }
    }

    private void setEditControlStates()
    {
      this.pnlDetailControls.Visible = !this.readOnly;
      this.pnlTemplateControls.Visible = !this.readOnly;
    }

    private void loadTemplates()
    {
      FileSystemEntry[] fileSystemEntries = this.ifsExplorer.CurrentFileSystemEntries;
      this.gvTemplates.Items.Clear();
      foreach (FileSystemEntry entry in fileSystemEntries)
        this.gvTemplates.Items.Add(this.createGVItemForTemplate(entry));
      this.gvTemplates.ReSort();
      this.refreshTemplateCount();
    }

    private void refreshTemplateCount()
    {
      this.grpTemplates.Text = "Document Export Templates (" + (object) this.gvTemplates.Items.Count + ")";
      if (this.gvTemplates.Items.Count != 0)
        return;
      this.clearTemplate();
    }

    private GVItem createGVItemForTemplate(FileSystemEntry entry)
    {
      GVItem gvItemForTemplate = new GVItem(this.ifsExplorer.GetDisplayName(entry, true));
      if (entry.Name == this.defaultTemplateName)
        gvItemForTemplate.SubItems.Add((object) "Yes");
      gvItemForTemplate.ImageIndex = this.ifsExplorer.GetFileImageIcon();
      gvItemForTemplate.Tag = (object) entry;
      return gvItemForTemplate;
    }

    private void gvTemplates_SelectedIndexChanged(object sender, EventArgs e)
    {
      DocumentExportTemplate documentExportTemplate = (DocumentExportTemplate) null;
      FileSystemEntry fileSystemEntry = (FileSystemEntry) null;
      string str = (string) null;
      string templateName = this.currentTemplate == null ? (string) null : this.currentTemplate.TemplateName;
      if (this.gvTemplates.SelectedItems.Count > 0)
      {
        fileSystemEntry = (FileSystemEntry) this.gvTemplates.SelectedItems[0].Tag;
        documentExportTemplate = (DocumentExportTemplate) this.session.ConfigurationManager.GetTemplateSettings(TemplateSettingsType.DocumentExportTemplate, fileSystemEntry);
        if (documentExportTemplate != null)
          str = documentExportTemplate.TemplateName;
      }
      this.btnDeleteTemplate.Enabled = this.btnDuplicateTemplate.Enabled = this.gvTemplates.SelectedItems.Count == 1;
      this.btnSetAsDefault.Enabled = documentExportTemplate != null && !documentExportTemplate.IsDefault && this.gvTemplates.SelectedItems.Count == 1;
      if (str == templateName)
        return;
      if (!this.PromptToCommit())
      {
        this.selectTemplate(templateName);
      }
      else
      {
        if (this.gvTemplates.SelectedItems.Count > 0 && !this.setCurrentTemplate(fileSystemEntry))
          this.selectTemplate(templateName);
        if (this.gvTemplates.SelectedItems.Count != 0)
          return;
        this.clearTemplate();
      }
    }

    public override void Save()
    {
      this.commitChanges();
      base.Save();
    }

    public bool PromptToCommit()
    {
      if (this.currentTemplate == null || !this.btnSaveDetails.Enabled)
        return true;
      switch (Utils.Dialog((IWin32Window) this, "Save the changes to the current export template?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
      {
        case DialogResult.Cancel:
          return false;
        case DialogResult.No:
          this.setCurrentTemplate(this.currentEntry, this.currentTemplate);
          return true;
        default:
          if (!this.validateTemplate())
            return false;
          this.commitChanges();
          return true;
      }
    }

    private void selectTemplate(string templateName)
    {
      if (templateName == null)
      {
        this.clearTemplate();
      }
      else
      {
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvTemplates.Items)
        {
          if (((DocumentExportTemplate) this.session.ConfigurationManager.GetTemplateSettings(TemplateSettingsType.DocumentExportTemplate, (FileSystemEntry) gvItem.Tag)).TemplateName == templateName)
          {
            gvItem.Selected = true;
            break;
          }
        }
      }
    }

    private void clearTemplate()
    {
      this.currentTemplate = (DocumentExportTemplate) null;
      this.currentEntry = (FileSystemEntry) null;
      this.grpTemplateDetails.Enabled = false;
    }

    private bool setCurrentTemplate(FileSystemEntry entry)
    {
      DocumentExportTemplate templateSettings = (DocumentExportTemplate) this.session.ConfigurationManager.GetTemplateSettings(TemplateSettingsType.DocumentExportTemplate, entry);
      if (templateSettings == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The selected export template has been deleted or is no longer accessible.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      this.setCurrentTemplate(entry, templateSettings);
      return true;
    }

    private void setCurrentTemplate(FileSystemEntry entry, DocumentExportTemplate template)
    {
      this.grpTemplateDetails.Enabled = true;
      this.currentEntry = entry;
      this.currentTemplate = template;
      this.reloadTemplateDetails();
      this.btnSaveDetails.Enabled = false;
      this.btnResetDetails.Enabled = false;
      this.setDirtyFlag(false);
    }

    private void reloadTemplateDetails()
    {
      this.rdoZip.Checked = this.currentTemplate.ExportAsZip;
      this.rdoPDF.Checked = !this.rdoZip.Checked;
      bool flag = false;
      if (this.currentTemplate.DocumentStackingTemplateID > 0)
      {
        StackingOrderSetTemplate stackingOrderSetTemplate = (StackingOrderSetTemplate) this.session.ConfigurationManager.GetTemplateSettings(TemplateSettingsType.StackingOrder, this.currentTemplate.DocumentStackingTemplateID);
        if (stackingOrderSetTemplate != null)
        {
          IEnumerable<StackingOrderSetTemplate> source = this.cboStackingOrder.Items.Cast<StackingOrderSetTemplate>().Where<StackingOrderSetTemplate>((Func<StackingOrderSetTemplate, bool>) (item => item.DocumentStackingTemplateID.Equals(stackingOrderSetTemplate.DocumentStackingTemplateID)));
          if (source.Count<StackingOrderSetTemplate>() > 0)
          {
            ClientCommonUtils.PopulateDropdown(this.cboStackingOrder, (object) source.First<StackingOrderSetTemplate>(), false);
            flag = true;
          }
        }
      }
      if (!flag)
        this.cboStackingOrder.SelectedIndex = -1;
      ClientCommonUtils.PopulateDropdown(this.cboAnnotations, (object) this.currentTemplate.AnnotationExportType.ToString(), false);
      this.chkPassword.Checked = this.currentTemplate.PasswordProtect;
      this.txtPass.Text = this.currentTemplate.Password;
      this.txtConfirmPass.Text = this.txtPass.Text;
      this.chkExportLocation.Checked = this.currentTemplate.ExportLocationSet;
      this.txtExportLocation.Text = this.currentTemplate.ExportLocation;
      ClientCommonUtils.PopulateDropdown(this.cboFileName1, (object) this.getExportFieldDisplayName(this.currentTemplate.FileNameField1), false);
      this.txtFileName1.Text = this.currentTemplate.FileNameText1;
      ClientCommonUtils.PopulateDropdown(this.cboFileName2, (object) this.getExportFieldDisplayName(this.currentTemplate.FileNameField2), false);
      this.txtFileName2.Text = this.currentTemplate.FileNameText2;
      ClientCommonUtils.PopulateDropdown(this.cboFileName3, (object) this.getExportFieldDisplayName(this.currentTemplate.FileNameField3), false);
      this.txtFileName3.Text = this.currentTemplate.FileNameText3;
      this.refreshFileName();
    }

    private void refreshFileName()
    {
      this.lblFileName.Text = "File Name: ";
      string fileNamePart1 = this.getFileNamePart(this.cboFileName1, this.txtFileName1);
      if (fileNamePart1 != string.Empty)
      {
        Label lblFileName = this.lblFileName;
        lblFileName.Text = lblFileName.Text + " " + fileNamePart1;
      }
      string fileNamePart2 = this.getFileNamePart(this.cboFileName2, this.txtFileName2);
      if (fileNamePart2 != string.Empty)
      {
        Label lblFileName = this.lblFileName;
        lblFileName.Text = lblFileName.Text + "_" + fileNamePart2;
      }
      string fileNamePart3 = this.getFileNamePart(this.cboFileName3, this.txtFileName3);
      if (!(fileNamePart3 != string.Empty))
        return;
      Label lblFileName1 = this.lblFileName;
      lblFileName1.Text = lblFileName1.Text + "_" + fileNamePart3;
    }

    private string getFileNamePart(ComboBox cboFileName, TextBox txtFileName)
    {
      string empty = string.Empty;
      if (cboFileName.SelectedItem != null && cboFileName.SelectedItem.ToString() != "None")
      {
        if (cboFileName.SelectedItem.ToString() == "Other")
        {
          if (!string.IsNullOrEmpty(txtFileName.Text))
            empty += txtFileName.Text;
        }
        else
          empty += cboFileName.SelectedItem.ToString();
      }
      return empty;
    }

    private string getExportFieldDisplayName(ExportFileNameFieldType fieldType)
    {
      switch (fieldType)
      {
        case ExportFileNameFieldType.None:
          return "None";
        case ExportFileNameFieldType.TodaysDate:
          return "Today's Date";
        case ExportFileNameFieldType.OrigLoanAmount:
          return "Orig. Loan Amount";
        case ExportFileNameFieldType.SubjectPropertyAddress:
          return "Subject Property Address";
        case ExportFileNameFieldType.LoanProgram:
          return "Loan Program";
        case ExportFileNameFieldType.PrimarySSN:
          return "Primary SSN";
        case ExportFileNameFieldType.InvestorLoanNumber:
          return "Investor Loan Number";
        case ExportFileNameFieldType.LoanNumber:
          return "Loan Number";
        case ExportFileNameFieldType.FirstPaymentDate:
          return "First Payment Date";
        case ExportFileNameFieldType.ActualPrincipal:
          return "Actual Principal";
        case ExportFileNameFieldType.LoanType:
          return "Loan Type";
        case ExportFileNameFieldType.InvestorFirstPaymentDate:
          return "Investor First Payment Date";
        case ExportFileNameFieldType.PrimaryFirstName:
          return "Primary First Name";
        case ExportFileNameFieldType.PrimaryLastName:
          return "Primary Last Name";
        case ExportFileNameFieldType.Other:
          return "Other";
        default:
          return string.Empty;
      }
    }

    private void onTemplatePropertyChanged(object sender, EventArgs e)
    {
      this.btnSaveDetails.Enabled = true;
      this.btnResetDetails.Enabled = true;
      this.setDirtyFlag(true);
    }

    private void btnSaveDetails_Click(object sender, EventArgs e) => this.commitChanges();

    private bool validateTemplate()
    {
      List<string> stringList = new List<string>();
      if (this.chkPassword.Checked && this.txtPass.Text == string.Empty)
        stringList.Add("You must enter a password if Protect File with Password is checked.");
      if (this.chkPassword.Checked && this.txtPass.Text != this.txtConfirmPass.Text)
        stringList.Add("Password and Confirmation do not match.");
      if (this.chkExportLocation.Checked)
      {
        if (this.txtExportLocation.Text == string.Empty)
        {
          stringList.Add("Location can not be blank if Set and define Document Export Location is checked.");
        }
        else
        {
          string str = this.txtExportLocation.Text;
          foreach (char invalidPathChar in Path.GetInvalidPathChars())
            str = str.Replace(invalidPathChar.ToString(), " ");
          if (this.txtExportLocation.Text != str)
            stringList.Add("Document Export Location contains invalid characters.");
          try
          {
            if (new DirectoryInfo(this.txtExportLocation.Text).FullName != this.txtExportLocation.Text)
              stringList.Add("Document Export Location is not a full path.");
          }
          catch (Exception ex)
          {
            stringList.Add("Document Export Location is invalid:\n\n" + ex.Message);
          }
        }
      }
      if (this.lblFileName.Text.Trim() == "File Name:")
        stringList.Add("A File Name is required.");
      if (this.txtFileName1.Visible)
      {
        string str = this.txtFileName1.Text;
        foreach (char invalidFileNameChar in Path.GetInvalidFileNameChars())
          str = str.Replace(invalidFileNameChar.ToString(), " ");
        if (str != this.txtFileName1.Text)
          stringList.Add("File Name contains invalid characters.");
      }
      if (this.txtFileName2.Visible)
      {
        string str = this.txtFileName2.Text;
        foreach (char invalidFileNameChar in Path.GetInvalidFileNameChars())
          str = str.Replace(invalidFileNameChar.ToString(), " ");
        if (str != this.txtFileName2.Text)
          stringList.Add("File Name contains invalid characters.");
      }
      if (this.txtFileName3.Visible)
      {
        string str = this.txtFileName3.Text;
        foreach (char invalidFileNameChar in Path.GetInvalidFileNameChars())
          str = str.Replace(invalidFileNameChar.ToString(), " ");
        if (str != this.txtFileName3.Text)
          stringList.Add("File Name contains invalid characters.");
      }
      if (stringList.Count == 0)
        return true;
      string text = "You must fix the following items in order to save:";
      foreach (string str in stringList)
        text = text + "\n\n" + str;
      int num = (int) Utils.Dialog((IWin32Window) this, text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      return false;
    }

    private bool templateExists(FileSystemEntry entry)
    {
      return this.ifsExplorer.EntryExistsOfAnyType(entry);
    }

    private void commitChanges()
    {
      if (!this.validateTemplate())
        return;
      this.commitDetailsToTemplate();
      this.session.ConfigurationManager.SaveTemplateSettings(TemplateSettingsType.DocumentExportTemplate, this.currentEntry, (BinaryObject) (BinaryConvertibleObject) this.currentTemplate);
      this.btnSaveDetails.Enabled = false;
      this.btnResetDetails.Enabled = false;
      this.setDirtyFlag(false);
    }

    private void commitDetailsToTemplate()
    {
      this.currentTemplate.ExportAsZip = this.rdoZip.Checked;
      this.currentTemplate.StackingOrderName = this.cboStackingOrder.Text;
      this.currentTemplate.DocumentStackingTemplateID = this.cboStackingOrder.SelectedValue == null ? 0 : Convert.ToInt32(this.cboStackingOrder.SelectedValue);
      this.currentTemplate.AnnotationExportType = (AnnotationExportType) Enum.Parse(typeof (AnnotationExportType), this.cboAnnotations.Text);
      this.currentTemplate.PasswordProtect = this.chkPassword.Checked;
      this.currentTemplate.ExportLocationSet = this.chkExportLocation.Checked;
      this.currentTemplate.ExportLocation = this.txtExportLocation.Text;
      this.currentTemplate.Password = this.txtPass.Text;
      this.currentTemplate.FileNameField1 = this.parseExportFieldDisplayValue(this.cboFileName1);
      this.currentTemplate.FileNameText1 = this.txtFileName1.Text;
      this.currentTemplate.FileNameField2 = this.parseExportFieldDisplayValue(this.cboFileName2);
      this.currentTemplate.FileNameText2 = this.txtFileName2.Text;
      this.currentTemplate.FileNameField3 = this.parseExportFieldDisplayValue(this.cboFileName3);
      this.currentTemplate.FileNameText3 = this.txtFileName3.Text;
    }

    private ExportFileNameFieldType parseExportFieldDisplayValue(ComboBox cboFileName)
    {
      return (ExportFileNameFieldType) Enum.Parse(typeof (ExportFileNameFieldType), cboFileName.SelectedItem.ToString().Replace(".", "").Replace("'", "").Replace(" ", ""));
    }

    private void btnResetDetails_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Discard your changes to the current export template?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      this.setCurrentTemplate((FileSystemEntry) this.gvTemplates.SelectedItems[0].Tag);
    }

    private void btnAddTemplate_Click(object sender, EventArgs e)
    {
      if (this.cboStackingOrder.Items.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "To create a Document Export Template, you first must create a Document Stacking Template.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        if (!this.PromptToCommit())
          return;
        this.gvTemplates.SelectedItems.Clear();
        FileSystemEntry entry = this.ifsExplorer.AddEntry(true);
        if (entry != null)
        {
          GVItem gvItemForTemplate = this.createGVItemForTemplate(entry);
          this.gvTemplates.Items.Add(gvItemForTemplate);
          gvItemForTemplate.Selected = true;
        }
        this.refreshTemplateCount();
        this.rdoPDF.Checked = true;
        string companySetting = this.session.ConfigurationManager.GetCompanySetting("StackingOrder", "Default");
        try
        {
          FileSystemEntry fileSystemEntry = FileSystemEntry.Parse(companySetting);
          IEnumerable<StackingOrderSetTemplate> source = this.cboStackingOrder.Items.Cast<StackingOrderSetTemplate>().Where<StackingOrderSetTemplate>((Func<StackingOrderSetTemplate, bool>) (item => item.TemplateName.Equals(fileSystemEntry.Name, StringComparison.OrdinalIgnoreCase)));
          if (source.Count<StackingOrderSetTemplate>() > 0)
            ClientCommonUtils.PopulateDropdown(this.cboStackingOrder, (object) source.First<StackingOrderSetTemplate>(), false);
        }
        catch
        {
        }
        this.cboAnnotations.SelectedIndex = 0;
        this.chkExportLocation.Checked = false;
        this.txtExportLocation.Text = string.Empty;
        this.chkPassword.Checked = false;
        this.txtPass.Text = string.Empty;
        this.txtConfirmPass.Text = string.Empty;
        this.cboFileName1.SelectedIndex = 1;
        this.btnResetDetails.Enabled = false;
      }
    }

    private void btnDuplicateTemplate_Click(object sender, EventArgs e)
    {
      if (!this.PromptToCommit() || this.currentEntry == null)
        return;
      if (!this.templateExists(this.currentEntry))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Cannot find the source file " + this.currentEntry.Name + ".", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (!this.ifsExplorer.DuplicateEntry(this.currentEntry))
          return;
        this.loadTemplates();
      }
    }

    private void btnDeleteTemplate_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to permanently delete the '" + this.currentTemplate.TemplateName + "' export template?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        return;
      this.ifsExplorer.DeleteEntry(this.currentEntry);
      this.currentTemplate = (DocumentExportTemplate) null;
      this.gvTemplates.Items.Remove(this.gvTemplates.SelectedItems[0]);
      this.refreshTemplateCount();
      this.clearTemplate();
    }

    private void btnSetAsDefault_Click(object sender, EventArgs e)
    {
      if (this.gvTemplates.SelectedItems.Count != 1)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "To setup a default export template, you have to select a single template first.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        this.session.ConfigurationManager.SetCompanySetting("ExportTemplate", "Default", ((FileSystemEntry) this.gvTemplates.SelectedItems[0].Tag).ToString());
        this.getCurrentDefaultTemplate();
        this.loadTemplates();
      }
    }

    private void getCurrentDefaultTemplate()
    {
      string companySetting = this.session.ConfigurationManager.GetCompanySetting("ExportTemplate", "Default");
      try
      {
        this.defaultTemplateName = FileSystemEntry.Parse(companySetting).Name;
      }
      catch
      {
      }
    }

    private void gvTemplates_EditorOpening(object sender, GVSubItemEditingEventArgs e)
    {
      if (this.gvTemplates.SelectedItems.Count <= 0)
        return;
      FileSystemEntry tag = (FileSystemEntry) this.gvTemplates.SelectedItems[0].Tag;
      if (tag == null || tag.Access != AclResourceAccess.ReadOnly)
        return;
      int num = (int) Utils.Dialog((IWin32Window) this, "You do not have the necessary access rights to rename this file.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      e.Cancel = true;
    }

    private void gvTemplates_EditorClosing(object sender, GVSubItemEditingEventArgs e)
    {
      GVItem parent = e.SubItem.Parent;
      FileSystemEntry tag = (FileSystemEntry) parent.Tag;
      if (e.EditorControl.Text == e.SubItem.Text)
        return;
      if (e.EditorControl.Text.IndexOf("\\") > -1)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "A file or folder name cannot contain the \"\\\" character.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        e.Cancel = true;
        e.EditorControl.Text = tag.Name;
      }
      else
      {
        int length = e.EditorControl.Text.IndexOf(".");
        if (length > -1 && e.EditorControl.Text.Substring(0, length).Trim() == "")
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "You must type a file name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          e.Cancel = true;
          e.EditorControl.Text = tag.Name;
        }
        else
        {
          int num1 = 260;
          if (FileSystemEntry.Types.File == ((FileSystemEntry) parent.Tag).Type)
          {
            string str = e.EditorControl.Text.Trim();
            if (num1 < str.Length)
            {
              int num2 = (int) Utils.Dialog((IWin32Window) this, "File name is limited to " + (object) num1 + " characters.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              e.Cancel = true;
              e.EditorControl.Text = tag.Name;
              return;
            }
          }
          string name = tag.Name;
          string newName = e.EditorControl.Text.Trim();
          if (newName == "")
          {
            int num3 = (int) Utils.Dialog((IWin32Window) this, "A file or folder name cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            e.Cancel = true;
            e.EditorControl.Text = tag.Name;
          }
          else
          {
            string lower = e.EditorControl.Text.Trim().ToLower();
            FileSystemEntry[] fileSystemEntries = this.ifsExplorer.CurrentFileSystemEntries;
            for (int index = 0; index < fileSystemEntries.Length; ++index)
            {
              if (fileSystemEntries[index] != tag && this.ifsExplorer.GetDisplayName(fileSystemEntries[index], true).ToLower() == lower)
              {
                int num4 = (int) Utils.Dialog((IWin32Window) this, "The selected item cannot be renamed because an item in this folder already exists with the specified name.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                e.Cancel = true;
                return;
              }
            }
            FileSystemEntry fileSystemEntry = this.ifsExplorer.Rename(tag, name, newName);
            if (fileSystemEntry == null)
            {
              e.Cancel = true;
            }
            else
            {
              e.SubItem.Item.Tag = (object) fileSystemEntry;
              this.currentEntry = fileSystemEntry;
              if (this.currentTemplate == null)
                return;
              this.currentTemplate.TemplateName = newName;
            }
          }
        }
      }
    }

    private void rdoExport_Click(object sender, EventArgs e)
    {
      this.onTemplatePropertyChanged(sender, e);
    }

    private void cboDetail_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.onTemplatePropertyChanged(sender, e);
    }

    private void chkPassword_CheckChanged(object sender, EventArgs e)
    {
      this.txtPass.Enabled = this.chkPassword.Checked;
      this.txtConfirmPass.Enabled = this.chkPassword.Checked;
      this.onTemplatePropertyChanged(sender, e);
    }

    private void txtPass_TextChanged(object sender, EventArgs e)
    {
      this.onTemplatePropertyChanged(sender, e);
    }

    private void chkExportLocation_CheckChanged(object sender, EventArgs e)
    {
      this.txtExportLocation.Enabled = this.chkExportLocation.Checked;
      this.btnBrowse.Enabled = this.chkExportLocation.Checked;
      this.onTemplatePropertyChanged(sender, e);
    }

    private void txtExportLocation_TextChanged(object sender, EventArgs e)
    {
      this.onTemplatePropertyChanged(sender, e);
    }

    private void btnBrowse_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
      if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
        return;
      this.txtExportLocation.Text = folderBrowserDialog.SelectedPath;
    }

    private void cboFileName_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cboFileName1.SelectedItem != null)
        this.txtFileName1.Visible = this.cboFileName1.SelectedItem.ToString() == "Other";
      if (this.cboFileName2.SelectedItem != null)
        this.txtFileName2.Visible = this.cboFileName2.SelectedItem.ToString() == "Other";
      if (this.cboFileName3.SelectedItem != null)
        this.txtFileName3.Visible = this.cboFileName3.SelectedItem.ToString() == "Other";
      this.refreshFileName();
      this.onTemplatePropertyChanged(sender, e);
    }

    private void txtFileName_TextChanged(object sender, EventArgs e)
    {
      this.refreshFileName();
      this.onTemplatePropertyChanged(sender, e);
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
      this.toolTip1 = new ToolTip(this.components);
      this.btnResetDetails = new StandardIconButton();
      this.btnSaveDetails = new StandardIconButton();
      this.btnRemoveDocument = new StandardIconButton();
      this.btnMoveDocumentDown = new StandardIconButton();
      this.btnMoveDocumentUp = new StandardIconButton();
      this.btnAddDocuments = new StandardIconButton();
      this.btnDeleteTemplate = new StandardIconButton();
      this.btnExportTemplate = new StandardIconButton();
      this.btnImportTemplate = new StandardIconButton();
      this.btnDuplicateTemplate = new StandardIconButton();
      this.btnAddTemplate = new StandardIconButton();
      this.sfdExport = new SaveFileDialog();
      this.ofdImport = new OpenFileDialog();
      this.grpTemplateDetails = new GroupContainer();
      this.pnlDetails = new Panel();
      this.label6 = new Label();
      this.lblFileName = new Label();
      this.txtFileName3 = new TextBox();
      this.txtFileName2 = new TextBox();
      this.txtFileName1 = new TextBox();
      this.cboFileName3 = new ComboBox();
      this.cboFileName2 = new ComboBox();
      this.cboFileName1 = new ComboBox();
      this.label5 = new Label();
      this.label4 = new Label();
      this.btnBrowse = new Button();
      this.txtExportLocation = new TextBox();
      this.label3 = new Label();
      this.chkExportLocation = new CheckBox();
      this.txtConfirmPass = new TextBox();
      this.newLbl = new Label();
      this.txtPass = new TextBox();
      this.confirmLbl = new Label();
      this.rdoZip = new RadioButton();
      this.rdoPDF = new RadioButton();
      this.chkPassword = new CheckBox();
      this.label2 = new Label();
      this.cboAnnotations = new ComboBox();
      this.label1 = new Label();
      this.cboStackingOrder = new ComboBox();
      this.pnlDetailControls = new FlowLayoutPanel();
      this.verticalSeparator2 = new VerticalSeparator();
      this.grpTemplates = new GroupContainer();
      this.gvTemplates = new GridView();
      this.pnlTemplateControls = new FlowLayoutPanel();
      this.btnSetAsDefault = new Button();
      this.verticalSeparator1 = new VerticalSeparator();
      ((ISupportInitialize) this.btnResetDetails).BeginInit();
      ((ISupportInitialize) this.btnSaveDetails).BeginInit();
      ((ISupportInitialize) this.btnRemoveDocument).BeginInit();
      ((ISupportInitialize) this.btnMoveDocumentDown).BeginInit();
      ((ISupportInitialize) this.btnMoveDocumentUp).BeginInit();
      ((ISupportInitialize) this.btnAddDocuments).BeginInit();
      ((ISupportInitialize) this.btnDeleteTemplate).BeginInit();
      ((ISupportInitialize) this.btnExportTemplate).BeginInit();
      ((ISupportInitialize) this.btnImportTemplate).BeginInit();
      ((ISupportInitialize) this.btnDuplicateTemplate).BeginInit();
      ((ISupportInitialize) this.btnAddTemplate).BeginInit();
      this.grpTemplateDetails.SuspendLayout();
      this.pnlDetails.SuspendLayout();
      this.pnlDetailControls.SuspendLayout();
      this.grpTemplates.SuspendLayout();
      this.pnlTemplateControls.SuspendLayout();
      this.SuspendLayout();
      this.btnResetDetails.BackColor = Color.Transparent;
      this.btnResetDetails.Location = new Point(114, 3);
      this.btnResetDetails.Margin = new Padding(5, 3, 0, 3);
      this.btnResetDetails.MouseDownImage = (Image) null;
      this.btnResetDetails.Name = "btnResetDetails";
      this.btnResetDetails.Size = new Size(16, 16);
      this.btnResetDetails.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.btnResetDetails.TabIndex = 0;
      this.btnResetDetails.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnResetDetails, "Reset Stacking Order");
      this.btnResetDetails.Click += new EventHandler(this.btnResetDetails_Click);
      this.btnSaveDetails.BackColor = Color.Transparent;
      this.btnSaveDetails.Location = new Point(93, 3);
      this.btnSaveDetails.Margin = new Padding(2, 3, 0, 3);
      this.btnSaveDetails.MouseDownImage = (Image) null;
      this.btnSaveDetails.Name = "btnSaveDetails";
      this.btnSaveDetails.Size = new Size(16, 16);
      this.btnSaveDetails.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.btnSaveDetails.TabIndex = 1;
      this.btnSaveDetails.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnSaveDetails, "Save Stacking Order");
      this.btnSaveDetails.Click += new EventHandler(this.btnSaveDetails_Click);
      this.btnRemoveDocument.BackColor = Color.Transparent;
      this.btnRemoveDocument.Enabled = false;
      this.btnRemoveDocument.Location = new Point(68, 3);
      this.btnRemoveDocument.Margin = new Padding(5, 3, 0, 3);
      this.btnRemoveDocument.MouseDownImage = (Image) null;
      this.btnRemoveDocument.Name = "btnRemoveDocument";
      this.btnRemoveDocument.Size = new Size(16, 16);
      this.btnRemoveDocument.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnRemoveDocument.TabIndex = 0;
      this.btnRemoveDocument.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnRemoveDocument, "Remove Document(s) from Stacking Order");
      this.btnRemoveDocument.Visible = false;
      this.btnMoveDocumentDown.BackColor = Color.Transparent;
      this.btnMoveDocumentDown.Enabled = false;
      this.btnMoveDocumentDown.Location = new Point(47, 3);
      this.btnMoveDocumentDown.Margin = new Padding(5, 3, 0, 3);
      this.btnMoveDocumentDown.MouseDownImage = (Image) null;
      this.btnMoveDocumentDown.Name = "btnMoveDocumentDown";
      this.btnMoveDocumentDown.Size = new Size(16, 16);
      this.btnMoveDocumentDown.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.btnMoveDocumentDown.TabIndex = 1;
      this.btnMoveDocumentDown.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnMoveDocumentDown, "Move Document(s) down in Stacking Order");
      this.btnMoveDocumentDown.Visible = false;
      this.btnMoveDocumentUp.BackColor = Color.Transparent;
      this.btnMoveDocumentUp.Enabled = false;
      this.btnMoveDocumentUp.Location = new Point(26, 3);
      this.btnMoveDocumentUp.Margin = new Padding(5, 3, 0, 3);
      this.btnMoveDocumentUp.MouseDownImage = (Image) null;
      this.btnMoveDocumentUp.Name = "btnMoveDocumentUp";
      this.btnMoveDocumentUp.Size = new Size(16, 16);
      this.btnMoveDocumentUp.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.btnMoveDocumentUp.TabIndex = 2;
      this.btnMoveDocumentUp.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnMoveDocumentUp, "Move Document(s) up in Stacking Order");
      this.btnMoveDocumentUp.Visible = false;
      this.btnAddDocuments.BackColor = Color.Transparent;
      this.btnAddDocuments.Location = new Point(5, 3);
      this.btnAddDocuments.Margin = new Padding(5, 3, 0, 3);
      this.btnAddDocuments.MouseDownImage = (Image) null;
      this.btnAddDocuments.Name = "btnAddDocuments";
      this.btnAddDocuments.Size = new Size(16, 16);
      this.btnAddDocuments.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddDocuments.TabIndex = 3;
      this.btnAddDocuments.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnAddDocuments, "Add Document(s) to Stacking Order");
      this.btnAddDocuments.Visible = false;
      this.btnDeleteTemplate.BackColor = Color.Transparent;
      this.btnDeleteTemplate.Enabled = false;
      this.btnDeleteTemplate.Location = new Point(89, 3);
      this.btnDeleteTemplate.Margin = new Padding(5, 3, 0, 3);
      this.btnDeleteTemplate.MouseDownImage = (Image) null;
      this.btnDeleteTemplate.Name = "btnDeleteTemplate";
      this.btnDeleteTemplate.Size = new Size(16, 16);
      this.btnDeleteTemplate.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDeleteTemplate.TabIndex = 0;
      this.btnDeleteTemplate.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnDeleteTemplate, "Delete Export Template");
      this.btnDeleteTemplate.Click += new EventHandler(this.btnDeleteTemplate_Click);
      this.btnExportTemplate.BackColor = Color.Transparent;
      this.btnExportTemplate.Enabled = false;
      this.btnExportTemplate.Location = new Point(68, 3);
      this.btnExportTemplate.Margin = new Padding(5, 3, 0, 3);
      this.btnExportTemplate.MouseDownImage = (Image) null;
      this.btnExportTemplate.Name = "btnExportTemplate";
      this.btnExportTemplate.Size = new Size(16, 16);
      this.btnExportTemplate.StandardButtonType = StandardIconButton.ButtonType.ExportDataToFileButton;
      this.btnExportTemplate.TabIndex = 5;
      this.btnExportTemplate.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnExportTemplate, "Export Template");
      this.btnExportTemplate.Visible = false;
      this.btnImportTemplate.BackColor = Color.Transparent;
      this.btnImportTemplate.Location = new Point(47, 3);
      this.btnImportTemplate.Margin = new Padding(5, 3, 0, 3);
      this.btnImportTemplate.MouseDownImage = (Image) null;
      this.btnImportTemplate.Name = "btnImportTemplate";
      this.btnImportTemplate.Size = new Size(16, 16);
      this.btnImportTemplate.StandardButtonType = StandardIconButton.ButtonType.ImportLoanButton;
      this.btnImportTemplate.TabIndex = 6;
      this.btnImportTemplate.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnImportTemplate, "Import Export Template");
      this.btnImportTemplate.Visible = false;
      this.btnDuplicateTemplate.BackColor = Color.Transparent;
      this.btnDuplicateTemplate.Enabled = false;
      this.btnDuplicateTemplate.Location = new Point(26, 3);
      this.btnDuplicateTemplate.Margin = new Padding(5, 3, 0, 3);
      this.btnDuplicateTemplate.MouseDownImage = (Image) null;
      this.btnDuplicateTemplate.Name = "btnDuplicateTemplate";
      this.btnDuplicateTemplate.Size = new Size(16, 16);
      this.btnDuplicateTemplate.StandardButtonType = StandardIconButton.ButtonType.DuplicateButton;
      this.btnDuplicateTemplate.TabIndex = 1;
      this.btnDuplicateTemplate.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnDuplicateTemplate, "Duplicate Export Template");
      this.btnDuplicateTemplate.Click += new EventHandler(this.btnDuplicateTemplate_Click);
      this.btnAddTemplate.BackColor = Color.Transparent;
      this.btnAddTemplate.Location = new Point(5, 3);
      this.btnAddTemplate.Margin = new Padding(5, 3, 0, 3);
      this.btnAddTemplate.MouseDownImage = (Image) null;
      this.btnAddTemplate.Name = "btnAddTemplate";
      this.btnAddTemplate.Size = new Size(16, 16);
      this.btnAddTemplate.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddTemplate.TabIndex = 2;
      this.btnAddTemplate.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnAddTemplate, "New Export Template");
      this.btnAddTemplate.Click += new EventHandler(this.btnAddTemplate_Click);
      this.sfdExport.DefaultExt = "xml";
      this.sfdExport.Filter = "XML Files|*.xml|All Files|*.*";
      this.ofdImport.DefaultExt = "xml";
      this.ofdImport.Filter = "XML Files|*.xml|All Files|*.*";
      this.grpTemplateDetails.Controls.Add((Control) this.pnlDetails);
      this.grpTemplateDetails.Controls.Add((Control) this.pnlDetailControls);
      this.grpTemplateDetails.HeaderForeColor = SystemColors.ControlText;
      this.grpTemplateDetails.Location = new Point(359, 0);
      this.grpTemplateDetails.Name = "grpTemplateDetails";
      this.grpTemplateDetails.Size = new Size(474, 496);
      this.grpTemplateDetails.TabIndex = 3;
      this.grpTemplateDetails.Text = "Template Details";
      this.pnlDetails.Controls.Add((Control) this.label6);
      this.pnlDetails.Controls.Add((Control) this.lblFileName);
      this.pnlDetails.Controls.Add((Control) this.txtFileName3);
      this.pnlDetails.Controls.Add((Control) this.txtFileName2);
      this.pnlDetails.Controls.Add((Control) this.txtFileName1);
      this.pnlDetails.Controls.Add((Control) this.cboFileName3);
      this.pnlDetails.Controls.Add((Control) this.cboFileName2);
      this.pnlDetails.Controls.Add((Control) this.cboFileName1);
      this.pnlDetails.Controls.Add((Control) this.label5);
      this.pnlDetails.Controls.Add((Control) this.label4);
      this.pnlDetails.Controls.Add((Control) this.btnBrowse);
      this.pnlDetails.Controls.Add((Control) this.txtExportLocation);
      this.pnlDetails.Controls.Add((Control) this.label3);
      this.pnlDetails.Controls.Add((Control) this.chkExportLocation);
      this.pnlDetails.Controls.Add((Control) this.txtConfirmPass);
      this.pnlDetails.Controls.Add((Control) this.newLbl);
      this.pnlDetails.Controls.Add((Control) this.txtPass);
      this.pnlDetails.Controls.Add((Control) this.confirmLbl);
      this.pnlDetails.Controls.Add((Control) this.rdoZip);
      this.pnlDetails.Controls.Add((Control) this.rdoPDF);
      this.pnlDetails.Controls.Add((Control) this.chkPassword);
      this.pnlDetails.Controls.Add((Control) this.label2);
      this.pnlDetails.Controls.Add((Control) this.cboAnnotations);
      this.pnlDetails.Controls.Add((Control) this.label1);
      this.pnlDetails.Controls.Add((Control) this.cboStackingOrder);
      this.pnlDetails.Dock = DockStyle.Fill;
      this.pnlDetails.Location = new Point(1, 26);
      this.pnlDetails.Name = "pnlDetails";
      this.pnlDetails.Size = new Size(472, 469);
      this.pnlDetails.TabIndex = 19;
      this.label6.AutoSize = true;
      this.label6.Location = new Point(10, 37);
      this.label6.Name = "label6";
      this.label6.Size = new Size(327, 14);
      this.label6.TabIndex = 43;
      this.label6.Text = "Only documents specified by the document stack will be exported.";
      this.lblFileName.AutoSize = true;
      this.lblFileName.Location = new Point(145, 372);
      this.lblFileName.Name = "lblFileName";
      this.lblFileName.Size = new Size(56, 14);
      this.lblFileName.TabIndex = 42;
      this.lblFileName.Text = "File Name:";
      this.txtFileName3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtFileName3.Location = new Point(309, 338);
      this.txtFileName3.MaxLength = 50;
      this.txtFileName3.Name = "txtFileName3";
      this.txtFileName3.Size = new Size(150, 20);
      this.txtFileName3.TabIndex = 41;
      this.txtFileName3.Visible = false;
      this.txtFileName3.TextChanged += new EventHandler(this.txtFileName_TextChanged);
      this.txtFileName2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtFileName2.Location = new Point(309, 312);
      this.txtFileName2.MaxLength = 50;
      this.txtFileName2.Name = "txtFileName2";
      this.txtFileName2.Size = new Size(150, 20);
      this.txtFileName2.TabIndex = 40;
      this.txtFileName2.Visible = false;
      this.txtFileName2.TextChanged += new EventHandler(this.txtFileName_TextChanged);
      this.txtFileName1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtFileName1.Location = new Point(309, 285);
      this.txtFileName1.MaxLength = 50;
      this.txtFileName1.Name = "txtFileName1";
      this.txtFileName1.Size = new Size(150, 20);
      this.txtFileName1.TabIndex = 39;
      this.txtFileName1.Visible = false;
      this.txtFileName1.TextChanged += new EventHandler(this.txtFileName_TextChanged);
      this.cboFileName3.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFileName3.FormattingEnabled = true;
      this.cboFileName3.Items.AddRange(new object[15]
      {
        (object) "None",
        (object) "Loan Number",
        (object) "Investor Loan Number",
        (object) "Primary First Name",
        (object) "Primary Last Name",
        (object) "Primary SSN",
        (object) "Subject Property Address",
        (object) "Loan Type",
        (object) "Orig. Loan Amount",
        (object) "Loan Program",
        (object) "Actual Principal",
        (object) "First Payment Date",
        (object) "Investor First Payment Date",
        (object) "Today's Date",
        (object) "Other"
      });
      this.cboFileName3.Location = new Point(148, 338);
      this.cboFileName3.Name = "cboFileName3";
      this.cboFileName3.Size = new Size(155, 22);
      this.cboFileName3.TabIndex = 38;
      this.cboFileName3.SelectedIndexChanged += new EventHandler(this.cboFileName_SelectedIndexChanged);
      this.cboFileName2.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFileName2.FormattingEnabled = true;
      this.cboFileName2.Items.AddRange(new object[15]
      {
        (object) "None",
        (object) "Loan Number",
        (object) "Investor Loan Number",
        (object) "Primary First Name",
        (object) "Primary Last Name",
        (object) "Primary SSN",
        (object) "Subject Property Address",
        (object) "Loan Type",
        (object) "Orig. Loan Amount",
        (object) "Loan Program",
        (object) "Actual Principal",
        (object) "First Payment Date",
        (object) "Investor First Payment Date",
        (object) "Today's Date",
        (object) "Other"
      });
      this.cboFileName2.Location = new Point(148, 310);
      this.cboFileName2.Name = "cboFileName2";
      this.cboFileName2.Size = new Size(155, 22);
      this.cboFileName2.TabIndex = 37;
      this.cboFileName2.SelectedIndexChanged += new EventHandler(this.cboFileName_SelectedIndexChanged);
      this.cboFileName1.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFileName1.FormattingEnabled = true;
      this.cboFileName1.Items.AddRange(new object[15]
      {
        (object) "None",
        (object) "Loan Number",
        (object) "Investor Loan Number",
        (object) "Primary First Name",
        (object) "Primary Last Name",
        (object) "Primary SSN",
        (object) "Subject Property Address",
        (object) "Loan Type",
        (object) "Orig. Loan Amount",
        (object) "Loan Program",
        (object) "Actual Principal",
        (object) "First Payment Date",
        (object) "Investor First Payment Date",
        (object) "Today's Date",
        (object) "Other"
      });
      this.cboFileName1.Location = new Point(148, 282);
      this.cboFileName1.Name = "cboFileName1";
      this.cboFileName1.Size = new Size(155, 22);
      this.cboFileName1.TabIndex = 36;
      this.cboFileName1.SelectedIndexChanged += new EventHandler(this.cboFileName_SelectedIndexChanged);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(9, 282);
      this.label5.Name = "label5";
      this.label5.Size = new Size(89, 14);
      this.label5.TabIndex = 35;
      this.label5.Text = "File Name Builder";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(145, 240);
      this.label4.Name = "label4";
      this.label4.Size = new Size(122, 14);
      this.label4.TabIndex = 34;
      this.label4.Text = "Example: \\\\server\\share";
      this.btnBrowse.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.btnBrowse.AutoSize = true;
      this.btnBrowse.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.btnBrowse.BackColor = SystemColors.Control;
      this.btnBrowse.Enabled = false;
      this.btnBrowse.Location = new Point(394, 235);
      this.btnBrowse.Name = "btnBrowse";
      this.btnBrowse.Size = new Size(65, 24);
      this.btnBrowse.TabIndex = 33;
      this.btnBrowse.Text = "Browse...";
      this.btnBrowse.UseVisualStyleBackColor = true;
      this.btnBrowse.Click += new EventHandler(this.btnBrowse_Click);
      this.txtExportLocation.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtExportLocation.Enabled = false;
      this.txtExportLocation.Location = new Point(148, 214);
      this.txtExportLocation.MaxLength = 50;
      this.txtExportLocation.Name = "txtExportLocation";
      this.txtExportLocation.Size = new Size(311, 20);
      this.txtExportLocation.TabIndex = 32;
      this.txtExportLocation.TextChanged += new EventHandler(this.txtExportLocation_TextChanged);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(9, 217);
      this.label3.Name = "label3";
      this.label3.Size = new Size(133, 14);
      this.label3.TabIndex = 31;
      this.label3.Text = "Document Export Location";
      this.chkExportLocation.AutoSize = true;
      this.chkExportLocation.Location = new Point(148, 190);
      this.chkExportLocation.Name = "chkExportLocation";
      this.chkExportLocation.Size = new Size(174, 18);
      this.chkExportLocation.TabIndex = 30;
      this.chkExportLocation.Text = "Set and define Export Location";
      this.chkExportLocation.CheckedChanged += new EventHandler(this.chkExportLocation_CheckChanged);
      this.txtConfirmPass.Enabled = false;
      this.txtConfirmPass.Location = new Point(309, 119);
      this.txtConfirmPass.Name = "txtConfirmPass";
      this.txtConfirmPass.PasswordChar = '*';
      this.txtConfirmPass.Size = new Size(150, 20);
      this.txtConfirmPass.TabIndex = 29;
      this.txtConfirmPass.TextChanged += new EventHandler(this.txtPass_TextChanged);
      this.newLbl.Location = new Point(207, 92);
      this.newLbl.Name = "newLbl";
      this.newLbl.Size = new Size(100, 16);
      this.newLbl.TabIndex = 26;
      this.newLbl.Text = "Set Password";
      this.newLbl.TextAlign = ContentAlignment.MiddleLeft;
      this.txtPass.Enabled = false;
      this.txtPass.Location = new Point(309, 91);
      this.txtPass.Name = "txtPass";
      this.txtPass.PasswordChar = '*';
      this.txtPass.Size = new Size(150, 20);
      this.txtPass.TabIndex = 28;
      this.txtPass.TextChanged += new EventHandler(this.txtPass_TextChanged);
      this.confirmLbl.Location = new Point(207, 120);
      this.confirmLbl.Name = "confirmLbl";
      this.confirmLbl.Size = new Size(100, 16);
      this.confirmLbl.TabIndex = 27;
      this.confirmLbl.Text = "Confirm Password";
      this.confirmLbl.TextAlign = ContentAlignment.MiddleLeft;
      this.rdoZip.AutoSize = true;
      this.rdoZip.Location = new Point(122, 64);
      this.rdoZip.Name = "rdoZip";
      this.rdoZip.Size = new Size(89, 18);
      this.rdoZip.TabIndex = 25;
      this.rdoZip.TabStop = true;
      this.rdoZip.Text = "Export as ZIP";
      this.rdoZip.UseVisualStyleBackColor = true;
      this.rdoZip.Click += new EventHandler(this.rdoExport_Click);
      this.rdoPDF.AutoSize = true;
      this.rdoPDF.Location = new Point(13, 64);
      this.rdoPDF.Name = "rdoPDF";
      this.rdoPDF.Size = new Size(93, 18);
      this.rdoPDF.TabIndex = 24;
      this.rdoPDF.TabStop = true;
      this.rdoPDF.Text = "Export as PDF";
      this.rdoPDF.UseVisualStyleBackColor = true;
      this.rdoPDF.Click += new EventHandler(this.rdoExport_Click);
      this.chkPassword.AutoSize = true;
      this.chkPassword.Location = new Point(309, 64);
      this.chkPassword.Name = "chkPassword";
      this.chkPassword.Size = new Size(156, 18);
      this.chkPassword.TabIndex = 23;
      this.chkPassword.Text = "Protect File with Password";
      this.chkPassword.CheckedChanged += new EventHandler(this.chkPassword_CheckChanged);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(10, 153);
      this.label2.Name = "label2";
      this.label2.Size = new Size(99, 14);
      this.label2.TabIndex = 22;
      this.label2.Text = "Annotations Export";
      this.cboAnnotations.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboAnnotations.FormattingEnabled = true;
      this.cboAnnotations.Items.AddRange(new object[4]
      {
        (object) "None",
        (object) "All",
        (object) "Personal",
        (object) "Public"
      });
      this.cboAnnotations.Location = new Point(148, 150);
      this.cboAnnotations.Name = "cboAnnotations";
      this.cboAnnotations.Size = new Size(155, 22);
      this.cboAnnotations.TabIndex = 21;
      this.cboAnnotations.SelectedIndexChanged += new EventHandler(this.cboDetail_SelectedIndexChanged);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(10, 15);
      this.label1.Name = "label1";
      this.label1.Size = new Size(131, 14);
      this.label1.TabIndex = 20;
      this.label1.Text = "Document Stack to Export";
      this.cboStackingOrder.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboStackingOrder.FormattingEnabled = true;
      this.cboStackingOrder.Location = new Point(148, 12);
      this.cboStackingOrder.Name = "cboStackingOrder";
      this.cboStackingOrder.Size = new Size(155, 22);
      this.cboStackingOrder.TabIndex = 19;
      this.cboStackingOrder.SelectedIndexChanged += new EventHandler(this.cboDetail_SelectedIndexChanged);
      this.pnlDetailControls.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlDetailControls.AutoSize = true;
      this.pnlDetailControls.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.pnlDetailControls.BackColor = Color.Transparent;
      this.pnlDetailControls.Controls.Add((Control) this.btnResetDetails);
      this.pnlDetailControls.Controls.Add((Control) this.btnSaveDetails);
      this.pnlDetailControls.Controls.Add((Control) this.verticalSeparator2);
      this.pnlDetailControls.Controls.Add((Control) this.btnRemoveDocument);
      this.pnlDetailControls.Controls.Add((Control) this.btnMoveDocumentDown);
      this.pnlDetailControls.Controls.Add((Control) this.btnMoveDocumentUp);
      this.pnlDetailControls.Controls.Add((Control) this.btnAddDocuments);
      this.pnlDetailControls.FlowDirection = FlowDirection.RightToLeft;
      this.pnlDetailControls.Location = new Point(338, 2);
      this.pnlDetailControls.Name = "pnlDetailControls";
      this.pnlDetailControls.Size = new Size(130, 22);
      this.pnlDetailControls.TabIndex = 2;
      this.verticalSeparator2.Location = new Point(87, 3);
      this.verticalSeparator2.Margin = new Padding(3, 3, 2, 3);
      this.verticalSeparator2.MaximumSize = new Size(2, 16);
      this.verticalSeparator2.MinimumSize = new Size(2, 16);
      this.verticalSeparator2.Name = "verticalSeparator2";
      this.verticalSeparator2.Size = new Size(2, 16);
      this.verticalSeparator2.TabIndex = 4;
      this.verticalSeparator2.Text = "verticalSeparator2";
      this.grpTemplates.Controls.Add((Control) this.gvTemplates);
      this.grpTemplates.Controls.Add((Control) this.pnlTemplateControls);
      this.grpTemplates.HeaderForeColor = SystemColors.ControlText;
      this.grpTemplates.Location = new Point(0, 0);
      this.grpTemplates.Name = "grpTemplates";
      this.grpTemplates.Size = new Size(353, 496);
      this.grpTemplates.TabIndex = 0;
      this.grpTemplates.Text = "Document Export Templates";
      this.gvTemplates.AllowMultiselect = false;
      this.gvTemplates.BorderStyle = BorderStyle.None;
      this.gvTemplates.ClearSelectionsOnEmptyRowClick = false;
      gvColumn1.ActivatedEditorType = GVActivatedEditorType.TextBox;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SpringToFit = true;
      gvColumn1.Text = "Name";
      gvColumn1.Width = 291;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Default";
      gvColumn2.Width = 60;
      this.gvTemplates.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvTemplates.Dock = DockStyle.Fill;
      this.gvTemplates.Location = new Point(1, 26);
      this.gvTemplates.Name = "gvTemplates";
      this.gvTemplates.Size = new Size(351, 469);
      this.gvTemplates.TabIndex = 0;
      this.gvTemplates.SelectedIndexChanged += new EventHandler(this.gvTemplates_SelectedIndexChanged);
      this.gvTemplates.EditorOpening += new GVSubItemEditingEventHandler(this.gvTemplates_EditorOpening);
      this.gvTemplates.EditorClosing += new GVSubItemEditingEventHandler(this.gvTemplates_EditorClosing);
      this.pnlTemplateControls.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlTemplateControls.AutoSize = true;
      this.pnlTemplateControls.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.pnlTemplateControls.BackColor = Color.Transparent;
      this.pnlTemplateControls.Controls.Add((Control) this.btnSetAsDefault);
      this.pnlTemplateControls.Controls.Add((Control) this.verticalSeparator1);
      this.pnlTemplateControls.Controls.Add((Control) this.btnDeleteTemplate);
      this.pnlTemplateControls.Controls.Add((Control) this.btnExportTemplate);
      this.pnlTemplateControls.Controls.Add((Control) this.btnImportTemplate);
      this.pnlTemplateControls.Controls.Add((Control) this.btnDuplicateTemplate);
      this.pnlTemplateControls.Controls.Add((Control) this.btnAddTemplate);
      this.pnlTemplateControls.FlowDirection = FlowDirection.RightToLeft;
      this.pnlTemplateControls.Location = new Point(147, 2);
      this.pnlTemplateControls.Name = "pnlTemplateControls";
      this.pnlTemplateControls.Size = new Size(200, 22);
      this.pnlTemplateControls.TabIndex = 1;
      this.btnSetAsDefault.Enabled = false;
      this.btnSetAsDefault.Location = new Point(112, 0);
      this.btnSetAsDefault.Margin = new Padding(0);
      this.btnSetAsDefault.Name = "btnSetAsDefault";
      this.btnSetAsDefault.Size = new Size(88, 22);
      this.btnSetAsDefault.TabIndex = 4;
      this.btnSetAsDefault.Text = "Set as Default";
      this.btnSetAsDefault.UseVisualStyleBackColor = true;
      this.btnSetAsDefault.Click += new EventHandler(this.btnSetAsDefault_Click);
      this.verticalSeparator1.Location = new Point(108, 3);
      this.verticalSeparator1.Margin = new Padding(3, 3, 2, 3);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 3;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.Controls.Add((Control) this.grpTemplateDetails);
      this.Controls.Add((Control) this.grpTemplates);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (ExportTemplateSetupControl);
      this.Size = new Size(837, 496);
      ((ISupportInitialize) this.btnResetDetails).EndInit();
      ((ISupportInitialize) this.btnSaveDetails).EndInit();
      ((ISupportInitialize) this.btnRemoveDocument).EndInit();
      ((ISupportInitialize) this.btnMoveDocumentDown).EndInit();
      ((ISupportInitialize) this.btnMoveDocumentUp).EndInit();
      ((ISupportInitialize) this.btnAddDocuments).EndInit();
      ((ISupportInitialize) this.btnDeleteTemplate).EndInit();
      ((ISupportInitialize) this.btnExportTemplate).EndInit();
      ((ISupportInitialize) this.btnImportTemplate).EndInit();
      ((ISupportInitialize) this.btnDuplicateTemplate).EndInit();
      ((ISupportInitialize) this.btnAddTemplate).EndInit();
      this.grpTemplateDetails.ResumeLayout(false);
      this.grpTemplateDetails.PerformLayout();
      this.pnlDetails.ResumeLayout(false);
      this.pnlDetails.PerformLayout();
      this.pnlDetailControls.ResumeLayout(false);
      this.grpTemplates.ResumeLayout(false);
      this.grpTemplates.PerformLayout();
      this.pnlTemplateControls.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
