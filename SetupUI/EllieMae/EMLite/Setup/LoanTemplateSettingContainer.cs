// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LoanTemplateSettingContainer
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class LoanTemplateSettingContainer : UserControl
  {
    private Sessions.Session session;
    private LoanTemplatePanelExplorer loanTemplatePanelExplorer;
    private LoanImportRequirement loanImportRequirement;
    private bool inLoadingMode = true;
    private IContainer components;
    private Label label2;
    private Label label1;
    private ComboBox cboWebCenter;
    private ComboBox cboFannieMae;
    private Panel panelBottom;
    private CollapsibleSplitter collapsibleSplitter1;
    private GroupContainer groupContainer1;
    private Panel panelTop;
    private StandardIconButton btnWeb;
    private StandardIconButton btnFannie;
    private TextBox txtWeb;
    private TextBox txtFannie;

    public LoanTemplateSettingContainer(Sessions.Session session)
    {
      this.session = session;
      this.InitializeComponent();
      this.cboFannieMae.Items.AddRange((object[]) LoanImportRequirement.GetImportOptions());
      this.cboWebCenter.Items.AddRange((object[]) LoanImportRequirement.GetImportOptions());
      this.loanTemplatePanelExplorer = new LoanTemplatePanelExplorer(session);
      this.loanTemplatePanelExplorer.AfterFileRenamed += new EventHandler(this.ifsExplorer_TemplateNamedChanged);
      this.loanTemplatePanelExplorer.BeforeFileMoved += new EventHandler(this.ifsExplorer_BeforeFileMoved);
      this.loanTemplatePanelExplorer.AfterFileMoved += new EventHandler(this.ifsExplorer_TemplateMoved);
      this.loanTemplatePanelExplorer.BeforeFileDeleted += new EventHandler(this.ifsExplorer_BeforeFileDeleted);
      this.loanTemplatePanelExplorer.AfterFileDeleted += new EventHandler(this.ifsExplorer_TemplateDeleted);
      this.panelTop.Controls.Add((Control) this.loanTemplatePanelExplorer);
      this.initForm();
      this.inLoadingMode = false;
      if (!(this.session.UserID != "admin"))
        return;
      this.panelBottom.Visible = false;
      this.collapsibleSplitter1.Visible = false;
    }

    private void initForm()
    {
      this.loanImportRequirement = this.session.ConfigurationManager.GetLoanImportRequirements();
      if (this.loanImportRequirement == null)
        return;
      this.cboFannieMae.Text = this.loanImportRequirement.FannieMaeImportRequirementTypeToString;
      this.txtFannie.Text = this.loanImportRequirement.TemplateForFannieMaeImport;
      this.cboWebCenter.Text = this.loanImportRequirement.WebCenterImportRequirementTypeToString;
      this.txtWeb.Text = this.loanImportRequirement.TemplateForWebCenterImport;
    }

    private void ifsExplorer_BeforeFileMoved(object sender, EventArgs e)
    {
      object[] objArray = (object[]) sender;
      FileSystemEntry sourceTemplate = (FileSystemEntry) objArray[1];
      FileSystemEntry targetTemplate = (FileSystemEntry) objArray[2];
      if (!sourceTemplate.IsPublic)
        return;
      bool fannieChanged = false;
      bool webChanged = false;
      if (sourceTemplate.Type == FileSystemEntry.Types.Folder)
      {
        if (this.txtFannie.Text.Trim() != string.Empty && this.txtFannie.Text.Trim().ToLower().StartsWith(sourceTemplate.ToDisplayString().ToLower()))
          fannieChanged = true;
      }
      else if (this.txtFannie.Text.Trim() != string.Empty && string.Compare(sourceTemplate.ToDisplayString(), this.txtFannie.Text.Trim(), true) == 0)
        fannieChanged = true;
      if (sourceTemplate.Type == FileSystemEntry.Types.Folder)
      {
        if (this.txtWeb.Text.Trim() != string.Empty && this.txtWeb.Text.Trim().ToLower().StartsWith(sourceTemplate.ToDisplayString().ToLower()))
          webChanged = true;
      }
      else if (this.txtWeb.Text.Trim() != string.Empty && string.Compare(sourceTemplate.ToDisplayString(), this.txtWeb.Text.Trim(), true) == 0)
        webChanged = true;
      if (!(fannieChanged | webChanged) || Utils.Dialog((IWin32Window) this, this.buildMessageForTemplateChangeEvents(sourceTemplate, targetTemplate, fannieChanged, webChanged, "BeforeMoved"), MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
        return;
      ((FSExplorer) objArray[0]).FileActionConfirmed = false;
    }

    private void ifsExplorer_BeforeFileDeleted(object sender, EventArgs e)
    {
      object[] objArray = (object[]) sender;
      FileSystemEntry sourceTemplate = (FileSystemEntry) objArray[1];
      if (sourceTemplate.Type != FileSystemEntry.Types.File || !sourceTemplate.IsPublic)
        return;
      bool fannieChanged = false;
      bool webChanged = false;
      if (this.txtFannie.Text.Trim() != string.Empty && string.Compare(sourceTemplate.ToDisplayString(), this.txtFannie.Text.Trim(), true) == 0)
        fannieChanged = true;
      if (this.txtWeb.Text.Trim() != string.Empty && string.Compare(sourceTemplate.ToDisplayString(), this.txtWeb.Text.Trim(), true) == 0)
        webChanged = true;
      if (!(fannieChanged | webChanged) || Utils.Dialog((IWin32Window) this, this.buildMessageForTemplateChangeEvents(sourceTemplate, (FileSystemEntry) null, fannieChanged, webChanged, "BeforeDeleted"), MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
        return;
      ((FSExplorer) objArray[0]).FileActionConfirmed = false;
    }

    private void ifsExplorer_TemplateDeleted(object sender, EventArgs e)
    {
      FileSystemEntry fileSystemEntry = (FileSystemEntry) sender;
      if (fileSystemEntry.Type != FileSystemEntry.Types.File || !fileSystemEntry.IsPublic)
        return;
      bool flag1 = false;
      bool flag2 = false;
      if (this.txtFannie.Text.Trim() != string.Empty && string.Compare(fileSystemEntry.ToDisplayString(), this.txtFannie.Text.Trim(), true) == 0)
      {
        this.cboFannieMae.SelectedIndex = 0;
        flag1 = true;
      }
      if (this.txtWeb.Text.Trim() != string.Empty && string.Compare(fileSystemEntry.ToDisplayString(), this.txtWeb.Text.Trim(), true) == 0)
      {
        this.cboWebCenter.SelectedIndex = 0;
        flag2 = true;
      }
      if (!(flag1 | flag2))
        return;
      this.saveSettings();
    }

    private void ifsExplorer_TemplateMoved(object sender, EventArgs e)
    {
      FileSystemEntry[] fileSystemEntryArray = (FileSystemEntry[]) sender;
      bool flag1 = false;
      bool flag2 = false;
      if (fileSystemEntryArray[0].Type == FileSystemEntry.Types.Folder)
      {
        if (this.txtFannie.Text.Trim() != string.Empty && this.txtFannie.Text.Trim().ToLower().StartsWith(fileSystemEntryArray[0].ToDisplayString().ToLower()))
          flag1 = true;
      }
      else if (this.txtFannie.Text.Trim() != string.Empty && string.Compare(fileSystemEntryArray[0].ToDisplayString(), this.txtFannie.Text.Trim(), true) == 0)
        flag1 = true;
      if (flag1)
      {
        if (fileSystemEntryArray[1].IsPublic)
        {
          if (fileSystemEntryArray[1].Type == FileSystemEntry.Types.Folder)
            this.txtFannie.Text = fileSystemEntryArray[1].ToDisplayString() + this.txtFannie.Text.Trim().Substring(fileSystemEntryArray[0].ToDisplayString().Length);
          else
            this.txtFannie.Text = fileSystemEntryArray[1].ToDisplayString();
        }
        else
        {
          this.txtFannie.Text = string.Empty;
          this.cboFannieMae.SelectedIndexChanged -= new EventHandler(this.importTypeField_SelectedIndexChanged);
          this.cboFannieMae.SelectedIndex = 0;
          this.cboFannieMae.SelectedIndexChanged += new EventHandler(this.importTypeField_SelectedIndexChanged);
          this.btnFannie.Enabled = false;
        }
      }
      if (fileSystemEntryArray[0].Type == FileSystemEntry.Types.Folder)
      {
        if (this.txtWeb.Text.Trim() != string.Empty && this.txtWeb.Text.Trim().ToLower().StartsWith(fileSystemEntryArray[0].ToDisplayString().ToLower()))
          flag2 = true;
      }
      else if (this.txtWeb.Text.Trim() != string.Empty && string.Compare(fileSystemEntryArray[0].ToDisplayString(), this.txtWeb.Text.Trim(), true) == 0)
        flag2 = true;
      if (flag2)
      {
        if (fileSystemEntryArray[1].IsPublic)
        {
          if (fileSystemEntryArray[1].Type == FileSystemEntry.Types.Folder)
            this.txtWeb.Text = fileSystemEntryArray[1].ToDisplayString() + this.txtWeb.Text.Trim().Substring(fileSystemEntryArray[0].ToDisplayString().Length);
          else
            this.txtWeb.Text = fileSystemEntryArray[1].ToDisplayString();
        }
        else
        {
          this.txtWeb.Text = string.Empty;
          this.cboWebCenter.SelectedIndexChanged -= new EventHandler(this.importTypeField_SelectedIndexChanged);
          this.cboWebCenter.SelectedIndex = 0;
          this.cboWebCenter.SelectedIndexChanged += new EventHandler(this.importTypeField_SelectedIndexChanged);
          this.btnWeb.Enabled = false;
        }
      }
      if (!(flag1 | flag2))
        return;
      this.saveSettings();
    }

    private void ifsExplorer_TemplateNamedChanged(object sender, EventArgs e)
    {
      object[] objArray = (object[]) sender;
      FileSystemEntry sourceTemplate = (FileSystemEntry) objArray[0];
      FileSystemEntry targetTemplate = (FileSystemEntry) objArray[1];
      if (!sourceTemplate.IsPublic)
        return;
      bool fannieChanged = false;
      bool webChanged = false;
      if (sourceTemplate.Type == FileSystemEntry.Types.Folder)
      {
        if (this.txtFannie.Text.Trim() != string.Empty && this.txtFannie.Text.Trim().ToLower().StartsWith(sourceTemplate.ToDisplayString().ToLower()))
          fannieChanged = true;
        if (this.txtWeb.Text.Trim() != string.Empty && this.txtWeb.Text.Trim().ToLower().StartsWith(sourceTemplate.ToDisplayString().ToLower()))
          webChanged = true;
      }
      else
      {
        if (string.Compare(sourceTemplate.ToDisplayString(), this.txtFannie.Text.Trim(), true) == 0)
          fannieChanged = true;
        if (string.Compare(sourceTemplate.ToDisplayString(), this.txtWeb.Text.Trim(), true) == 0)
          webChanged = true;
      }
      if (fannieChanged)
        this.txtFannie.Text = targetTemplate.ToDisplayString() + this.txtFannie.Text.Trim().Substring(sourceTemplate.ToDisplayString().Length);
      if (webChanged)
        this.txtWeb.Text = targetTemplate.ToDisplayString() + this.txtWeb.Text.Trim().Substring(sourceTemplate.ToDisplayString().Length);
      if (!(fannieChanged | webChanged))
        return;
      int num = (int) Utils.Dialog((IWin32Window) this, this.buildMessageForTemplateChangeEvents(sourceTemplate, targetTemplate, fannieChanged, webChanged, "NameChanged"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      this.saveSettings();
    }

    private string buildMessageForTemplateChangeEvents(
      FileSystemEntry sourceTemplate,
      FileSystemEntry targetTemplate,
      bool fannieChanged,
      bool webChanged,
      string changeType)
    {
      string str1 = "WebCenter Application imports";
      if (fannieChanged & webChanged)
        str1 = "Fannie Mae file imports and WebCenter Application imports";
      else if (fannieChanged)
        str1 = "Fannie Mae file imports";
      string empty = string.Empty;
      string str2;
      if (changeType == "NameChanged")
      {
        if (sourceTemplate.Type == FileSystemEntry.Types.Folder)
          str2 = "The '" + sourceTemplate.ToDisplayString() + "' folder you are renaming contains " + (fannieChanged & webChanged ? "loan templates that are" : "a loan template that is") + " currently designated as the required template for " + str1 + ". ";
        else
          str2 = "The loan template '" + this.getTemplateName(sourceTemplate.ToDisplayString()) + "' has been renamed to '" + this.getTemplateName(targetTemplate.ToDisplayString()) + "'. ";
      }
      else if (sourceTemplate.Type == FileSystemEntry.Types.Folder)
        str2 = "The ‘" + sourceTemplate.ToDisplayString() + "' folder you are moving contains " + (fannieChanged & webChanged ? "loan templates that are" : "a loan template that is") + " currently designated as the required template for " + str1 + ". ";
      else
        str2 = "The loan template ‘" + sourceTemplate.ToDisplayString() + "' is currently designated as the required template for " + str1 + ". ";
      switch (changeType)
      {
        case "BeforeDeleted":
          str2 += "If you continue, the template will be deleted and ";
          break;
        case "BeforeMoved":
          str2 = str2 + "If you continue to move this " + (sourceTemplate.Type == FileSystemEntry.Types.Folder ? "folder" : "template") + ", ";
          break;
      }
      string str3 = str2 + (changeType == "NameChanged" ? "The" : "the") + " Loan Import Requirements setting for " + str1 + " will be updated ";
      switch (changeType)
      {
        case "NameChanged":
          return sourceTemplate.Type != FileSystemEntry.Types.Folder ? str3 + "with the new template name: '" + this.getTemplateName(targetTemplate.ToDisplayString()) + "'." : str3 + "with the new template location.";
        case "BeforeDeleted":
          str3 += "to \"No template required\".";
          break;
        case "BeforeMoved":
          str3 = sourceTemplate.Type != FileSystemEntry.Types.Folder ? (!targetTemplate.IsPublic ? str3 + "to \"No template required\". (Templates stored in a Personal Loan Templates folder cannot be designated as a required template.)" : str3 + "to '" + targetTemplate.ToDisplayString() + "'.") : (!targetTemplate.IsPublic ? str3 + "to \"No template required\"." : str3 + "with the new template location.");
          break;
      }
      return str3 + " Do you want to continue?";
    }

    private string getTemplateName(string templateName)
    {
      int num = templateName.LastIndexOf('\\');
      return templateName.Substring(num + 1);
    }

    private void importTypeField_SelectedIndexChanged(object sender, EventArgs e)
    {
      ComboBox dropBox = (ComboBox) sender;
      if (dropBox == null)
        return;
      TextBox textBox = dropBox.Name == "cboFannieMae" ? this.txtFannie : this.txtWeb;
      StandardIconButton standardIconButton = dropBox.Name == "cboFannieMae" ? this.btnFannie : this.btnWeb;
      if (dropBox.SelectedIndex == 2 && string.Compare(dropBox.Text, dropBox.Name == "cboFannieMae" ? this.loanImportRequirement.FannieMaeImportRequirementTypeToString : this.loanImportRequirement.WebCenterImportRequirementTypeToString, true) != 0)
      {
        standardIconButton.Enabled = true;
        this.setLoanTemplate(textBox, dropBox);
      }
      if (dropBox.SelectedIndex != 2)
      {
        textBox.Text = string.Empty;
        standardIconButton.Enabled = false;
      }
      this.saveSettings();
    }

    private void saveSettings()
    {
      if (this.inLoadingMode)
        return;
      this.loanImportRequirement = new LoanImportRequirement();
      this.loanImportRequirement.FannieMaeImportRequirementTypeToEnum(this.cboFannieMae.Text);
      this.loanImportRequirement.TemplateForFannieMaeImport = this.txtFannie.Text.Trim();
      this.loanImportRequirement.WebCenterImportRequirementTypeToEnum(this.cboWebCenter.Text);
      this.loanImportRequirement.TemplateForWebCenterImport = this.txtWeb.Text.Trim();
      try
      {
        this.session.ConfigurationManager.SetLoanImportRequirements(this.loanImportRequirement);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Loan Import Requirement settings cannot be saved due to this error: " + ex.Message + ". All the changes you made will be lost.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void btnFannie_Click(object sender, EventArgs e)
    {
      this.setLoanTemplate(this.txtFannie, this.cboFannieMae);
      this.saveSettings();
    }

    private void btnWeb_Click(object sender, EventArgs e)
    {
      this.setLoanTemplate(this.txtWeb, this.cboWebCenter);
      this.saveSettings();
    }

    private void setLoanTemplate(TextBox textBox, ComboBox dropBox)
    {
      if (this.inLoadingMode)
        return;
      using (LoanTemplateSelectDialog templateSelectDialog = new LoanTemplateSelectDialog(this.session, false, false, true, true))
      {
        if (templateSelectDialog.ShowDialog((IWin32Window) this) == DialogResult.OK)
        {
          textBox.Text = templateSelectDialog.SelectedItem.ToDisplayString();
        }
        else
        {
          this.cboFannieMae.SelectedIndexChanged -= new EventHandler(this.importTypeField_SelectedIndexChanged);
          this.cboWebCenter.SelectedIndexChanged -= new EventHandler(this.importTypeField_SelectedIndexChanged);
          if (dropBox.Name == "cboFannieMae")
            dropBox.Text = this.loanImportRequirement.FannieMaeImportRequirementTypeToString;
          else
            dropBox.Text = this.loanImportRequirement.WebCenterImportRequirementTypeToString;
          this.cboWebCenter.SelectedIndexChanged += new EventHandler(this.importTypeField_SelectedIndexChanged);
          this.cboFannieMae.SelectedIndexChanged += new EventHandler(this.importTypeField_SelectedIndexChanged);
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
      this.panelBottom = new Panel();
      this.groupContainer1 = new GroupContainer();
      this.btnWeb = new StandardIconButton();
      this.btnFannie = new StandardIconButton();
      this.txtWeb = new TextBox();
      this.txtFannie = new TextBox();
      this.cboFannieMae = new ComboBox();
      this.label2 = new Label();
      this.cboWebCenter = new ComboBox();
      this.label1 = new Label();
      this.panelTop = new Panel();
      this.collapsibleSplitter1 = new CollapsibleSplitter();
      this.panelBottom.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      ((ISupportInitialize) this.btnWeb).BeginInit();
      ((ISupportInitialize) this.btnFannie).BeginInit();
      this.SuspendLayout();
      this.panelBottom.Controls.Add((Control) this.groupContainer1);
      this.panelBottom.Dock = DockStyle.Bottom;
      this.panelBottom.Location = new Point(0, 517);
      this.panelBottom.Name = "panelBottom";
      this.panelBottom.Size = new Size(838, 103);
      this.panelBottom.TabIndex = 4;
      this.groupContainer1.Controls.Add((Control) this.btnWeb);
      this.groupContainer1.Controls.Add((Control) this.btnFannie);
      this.groupContainer1.Controls.Add((Control) this.txtWeb);
      this.groupContainer1.Controls.Add((Control) this.txtFannie);
      this.groupContainer1.Controls.Add((Control) this.cboFannieMae);
      this.groupContainer1.Controls.Add((Control) this.label2);
      this.groupContainer1.Controls.Add((Control) this.cboWebCenter);
      this.groupContainer1.Controls.Add((Control) this.label1);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(838, 103);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "Loan Import Requirements";
      this.btnWeb.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnWeb.BackColor = Color.Transparent;
      this.btnWeb.Location = new Point(811, 68);
      this.btnWeb.MouseDownImage = (Image) null;
      this.btnWeb.Name = "btnWeb";
      this.btnWeb.Size = new Size(16, 16);
      this.btnWeb.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnWeb.TabIndex = 41;
      this.btnWeb.TabStop = false;
      this.btnWeb.Click += new EventHandler(this.btnWeb_Click);
      this.btnFannie.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnFannie.BackColor = Color.Transparent;
      this.btnFannie.Location = new Point(811, 39);
      this.btnFannie.MouseDownImage = (Image) null;
      this.btnFannie.Name = "btnFannie";
      this.btnFannie.Size = new Size(16, 16);
      this.btnFannie.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnFannie.TabIndex = 40;
      this.btnFannie.TabStop = false;
      this.btnFannie.Click += new EventHandler(this.btnFannie_Click);
      this.txtWeb.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtWeb.Location = new Point(414, 66);
      this.txtWeb.Name = "txtWeb";
      this.txtWeb.ReadOnly = true;
      this.txtWeb.Size = new Size(391, 20);
      this.txtWeb.TabIndex = 39;
      this.txtWeb.TabStop = false;
      this.txtFannie.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtFannie.Location = new Point(414, 37);
      this.txtFannie.Name = "txtFannie";
      this.txtFannie.ReadOnly = true;
      this.txtFannie.Size = new Size(391, 20);
      this.txtFannie.TabIndex = 38;
      this.txtFannie.TabStop = false;
      this.cboFannieMae.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFannieMae.FormattingEnabled = true;
      this.cboFannieMae.Location = new Point(161, 36);
      this.cboFannieMae.Name = "cboFannieMae";
      this.cboFannieMae.Size = new Size(247, 21);
      this.cboFannieMae.TabIndex = 0;
      this.cboFannieMae.SelectedIndexChanged += new EventHandler(this.importTypeField_SelectedIndexChanged);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(6, 68);
      this.label2.Name = "label2";
      this.label2.Size = new Size(146, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "WebCenter application import\r\n";
      this.cboWebCenter.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboWebCenter.FormattingEnabled = true;
      this.cboWebCenter.Location = new Point(161, 65);
      this.cboWebCenter.Name = "cboWebCenter";
      this.cboWebCenter.Size = new Size(247, 21);
      this.cboWebCenter.TabIndex = 2;
      this.cboWebCenter.SelectedIndexChanged += new EventHandler(this.importTypeField_SelectedIndexChanged);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(6, 39);
      this.label1.Name = "label1";
      this.label1.Size = new Size(110, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Fannie Mae file import";
      this.panelTop.Dock = DockStyle.Fill;
      this.panelTop.Location = new Point(0, 0);
      this.panelTop.Name = "panelTop";
      this.panelTop.Size = new Size(838, 510);
      this.panelTop.TabIndex = 6;
      this.collapsibleSplitter1.AnimationDelay = 20;
      this.collapsibleSplitter1.AnimationStep = 20;
      this.collapsibleSplitter1.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter1.ControlToHide = (Control) this.panelBottom;
      this.collapsibleSplitter1.Dock = DockStyle.Bottom;
      this.collapsibleSplitter1.ExpandParentForm = false;
      this.collapsibleSplitter1.Location = new Point(0, 510);
      this.collapsibleSplitter1.Name = "collapsibleSplitter1";
      this.collapsibleSplitter1.TabIndex = 5;
      this.collapsibleSplitter1.TabStop = false;
      this.collapsibleSplitter1.UseAnimations = false;
      this.collapsibleSplitter1.VisualStyle = VisualStyles.Encompass;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.panelTop);
      this.Controls.Add((Control) this.collapsibleSplitter1);
      this.Controls.Add((Control) this.panelBottom);
      this.Name = nameof (LoanTemplateSettingContainer);
      this.Size = new Size(838, 620);
      this.panelBottom.ResumeLayout(false);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      ((ISupportInitialize) this.btnWeb).EndInit();
      ((ISupportInitialize) this.btnFannie).EndInit();
      this.ResumeLayout(false);
    }
  }
}
