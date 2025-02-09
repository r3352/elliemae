// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.eFolder.DocumentConversionSetupControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common.eFolder;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.eFolder.WebServices;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.eFolder
{
  public class DocumentConversionSetupControl : SettingsUserControl
  {
    private Sessions.Session session;
    private ImageAttachmentSettings imageAttachmentSettings;
    private const string BLACK_AND_WHITE = "Black & White";
    private const string COLOR = "Color";
    private const string AUTOMATIC = "Automatic";
    private IContainer components;
    private GroupContainer gcConversionPreferences;
    private Label lblText;
    private GroupContainer gcImageConversion;
    private CheckBox chkOriginalFormat;
    private Label lblImageFormat;
    private Label lblResoulutionX;
    private ComboBox cboImageFormat;
    private Label lblDocumentConversion1;
    private CheckBox chkEnableRapidViewer;
    private Label lblDocumentConversion1a;
    private ComboBox cboResolution;
    private Label lblDocumentConversion3;
    private Label lblDocumentConversion5;
    private EMHelpLink helpLinkDocuments;

    public DocumentConversionSetupControl(Sessions.Session session, SetUpContainer setupContainer)
      : base(setupContainer)
    {
      this.InitializeComponent();
      this.session = session;
      this.imageAttachmentSettings = this.session.ConfigurationManager.GetImageAttachmentSettings();
      this.initRapidViewerPreferences();
      this.gcImageConversion.Enabled = this.gcConversionPreferences.Enabled = false;
    }

    private void initRapidViewerPreferences()
    {
      this.chkEnableRapidViewer.Checked = this.imageAttachmentSettings.UseImageAttachments;
      this.chkOriginalFormat.Checked = this.imageAttachmentSettings.SaveOriginalFormat;
      this.initImageFormatSelection();
      int num = -1;
      if (this.cboResolution.Items.Count == 0)
        num = this.cboResolution.Items.Add((object) (this.imageAttachmentSettings.DpiX.ToString() + " DPI"));
      this.cboResolution.SelectedIndex = num;
      this.cboResolution.Visible = false;
      this.lblResoulutionX.Visible = false;
      this.gcConversionPreferences.Enabled = this.chkEnableRapidViewer.Checked;
      this.setDirtyFlag(false);
    }

    private void initImageFormatSelection()
    {
      if (this.cboImageFormat.Items.Count == 0)
      {
        this.cboImageFormat.Items.Add((object) new FieldOption("Color", "Automatic"));
        this.cboImageFormat.Items.Add((object) new FieldOption("Black & White", "Black & White"));
      }
      this.cboImageFormat.SelectedIndex = this.getImageFormatSelection();
    }

    private int getImageFormatSelection()
    {
      if (this.cboImageFormat.Items.Count == 0)
        return -1;
      string empty = string.Empty;
      string str = !this.imageAttachmentSettings.ConversionType.Equals((object) ImageConversionType.BlackAndWhite) ? "Automatic" : "Black & White";
      for (int index = 0; index < this.cboImageFormat.Items.Count; ++index)
      {
        if (((FieldOption) this.cboImageFormat.Items[index]).Value == str)
          return index;
      }
      return 0;
    }

    public override void Save()
    {
      bool imageAttachments = this.imageAttachmentSettings.UseImageAttachments;
      ImageConversionType imageConversionType = !((FieldOption) this.cboImageFormat.SelectedItem).Value.Equals("Black & White") ? ImageConversionType.Automatic : ImageConversionType.BlackAndWhite;
      if (this.imageAttachmentSettings.UseImageAttachments != this.chkEnableRapidViewer.Checked || this.imageAttachmentSettings.SaveOriginalFormat != this.chkOriginalFormat.Checked || this.imageAttachmentSettings.ConversionType != imageConversionType)
      {
        this.imageAttachmentSettings.UseImageAttachments = this.chkEnableRapidViewer.Checked;
        this.imageAttachmentSettings.SaveOriginalFormat = this.chkOriginalFormat.Checked;
        this.imageAttachmentSettings.ConversionType = imageConversionType;
        this.session.ConfigurationManager.SaveImageAttachmentSettings(this.imageAttachmentSettings);
        if (this.imageAttachmentSettings.UseImageAttachments != imageAttachments)
        {
          using (CompanySettingsService companySettingsService = new CompanySettingsService(Session.SessionObjects?.StartupInfo?.ServiceUrls?.CompanySettingsServiceUrl))
          {
            companySettingsService.CompanySettingsCredentialsValue = new CompanySettingsCredentials();
            companySettingsService.CompanySettingsCredentialsValue.ClientID = Session.CompanyInfo.ClientID;
            companySettingsService.CompanySettingsCredentialsValue.UserID = Session.UserID;
            companySettingsService.CompanySettingsCredentialsValue.Password = EpassLogin.LoginPassword;
            companySettingsService.SaveCompanySettings(new CompanySettings()
            {
              ClientID = Session.CompanyInfo.ClientID,
              SettingName = "DocumentConversion",
              SettingValue = this.imageAttachmentSettings.UseImageAttachments
            });
          }
        }
      }
      this.setDirtyFlag(false);
    }

    public override void Reset() => this.initRapidViewerPreferences();

    private void chkEnableRapidViewer_CheckedChanged(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
      this.gcConversionPreferences.Enabled = this.chkEnableRapidViewer.Checked;
    }

    private void chkOriginalFormat_CheckedChanged(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
    }

    private void cboImageFormat_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
    }

    private void DocumentConversionSetupControl_Resize(object sender, EventArgs e)
    {
      this.gcConversionPreferences.Height = this.ClientSize.Height / 2;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.gcConversionPreferences = new GroupContainer();
      this.cboResolution = new ComboBox();
      this.lblResoulutionX = new Label();
      this.cboImageFormat = new ComboBox();
      this.lblImageFormat = new Label();
      this.chkOriginalFormat = new CheckBox();
      this.lblText = new Label();
      this.gcImageConversion = new GroupContainer();
      this.helpLinkDocuments = new EMHelpLink();
      this.lblDocumentConversion5 = new Label();
      this.lblDocumentConversion3 = new Label();
      this.lblDocumentConversion1a = new Label();
      this.chkEnableRapidViewer = new CheckBox();
      this.lblDocumentConversion1 = new Label();
      this.gcConversionPreferences.SuspendLayout();
      this.gcImageConversion.SuspendLayout();
      this.SuspendLayout();
      this.gcConversionPreferences.Controls.Add((Control) this.cboResolution);
      this.gcConversionPreferences.Controls.Add((Control) this.lblResoulutionX);
      this.gcConversionPreferences.Controls.Add((Control) this.cboImageFormat);
      this.gcConversionPreferences.Controls.Add((Control) this.lblImageFormat);
      this.gcConversionPreferences.Controls.Add((Control) this.chkOriginalFormat);
      this.gcConversionPreferences.Controls.Add((Control) this.lblText);
      this.gcConversionPreferences.Dock = DockStyle.Bottom;
      this.gcConversionPreferences.HeaderForeColor = SystemColors.ControlText;
      this.gcConversionPreferences.Location = new Point(0, 317);
      this.gcConversionPreferences.Name = "gcConversionPreferences";
      this.gcConversionPreferences.Size = new Size(631, 193);
      this.gcConversionPreferences.TabIndex = 0;
      this.gcConversionPreferences.Text = "Unassigned Document Conversion Preferences";
      this.cboResolution.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboResolution.Enabled = false;
      this.cboResolution.FormattingEnabled = true;
      this.cboResolution.Location = new Point(81, 149);
      this.cboResolution.Name = "cboResolution";
      this.cboResolution.Size = new Size(134, 21);
      this.cboResolution.TabIndex = 5;
      this.lblResoulutionX.AutoSize = true;
      this.lblResoulutionX.Location = new Point(13, 152);
      this.lblResoulutionX.Name = "lblResoulutionX";
      this.lblResoulutionX.Size = new Size(57, 13);
      this.lblResoulutionX.TabIndex = 4;
      this.lblResoulutionX.Text = "Resolution";
      this.cboImageFormat.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboImageFormat.FormattingEnabled = true;
      this.cboImageFormat.Location = new Point(81, 116);
      this.cboImageFormat.Name = "cboImageFormat";
      this.cboImageFormat.Size = new Size(134, 21);
      this.cboImageFormat.TabIndex = 3;
      this.cboImageFormat.SelectionChangeCommitted += new EventHandler(this.cboImageFormat_SelectionChangeCommitted);
      this.lblImageFormat.AutoSize = true;
      this.lblImageFormat.Location = new Point(11, 121);
      this.lblImageFormat.Name = "lblImageFormat";
      this.lblImageFormat.Size = new Size(39, 13);
      this.lblImageFormat.TabIndex = 2;
      this.lblImageFormat.Text = "Format";
      this.chkOriginalFormat.AutoSize = true;
      this.chkOriginalFormat.Location = new Point(79, 84);
      this.chkOriginalFormat.Name = "chkOriginalFormat";
      this.chkOriginalFormat.Size = new Size(258, 17);
      this.chkOriginalFormat.TabIndex = 1;
      this.chkOriginalFormat.Text = "Keep a copy of the document in its original format";
      this.chkOriginalFormat.UseVisualStyleBackColor = true;
      this.chkOriginalFormat.CheckedChanged += new EventHandler(this.chkOriginalFormat_CheckedChanged);
      this.lblText.AutoSize = true;
      this.lblText.Location = new Point(11, 49);
      this.lblText.Name = "lblText";
      this.lblText.Size = new Size(538, 13);
      this.lblText.TabIndex = 0;
      this.lblText.Text = "Configure the image conversion options for files imported into the Unassigned section of the eFolder File Manager";
      this.gcImageConversion.Controls.Add((Control) this.helpLinkDocuments);
      this.gcImageConversion.Controls.Add((Control) this.lblDocumentConversion5);
      this.gcImageConversion.Controls.Add((Control) this.lblDocumentConversion3);
      this.gcImageConversion.Controls.Add((Control) this.lblDocumentConversion1a);
      this.gcImageConversion.Controls.Add((Control) this.chkEnableRapidViewer);
      this.gcImageConversion.Controls.Add((Control) this.lblDocumentConversion1);
      this.gcImageConversion.Dock = DockStyle.Fill;
      this.gcImageConversion.HeaderForeColor = SystemColors.ControlText;
      this.gcImageConversion.Location = new Point(0, 0);
      this.gcImageConversion.Name = "gcImageConversion";
      this.gcImageConversion.Size = new Size(631, 317);
      this.gcImageConversion.TabIndex = 1;
      this.gcImageConversion.Text = "Document Conversion";
      this.helpLinkDocuments.BackColor = Color.Transparent;
      this.helpLinkDocuments.Cursor = Cursors.Hand;
      this.helpLinkDocuments.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.helpLinkDocuments.HelpTag = "Setup\\Document Conversion";
      this.helpLinkDocuments.Location = new Point(78, 170);
      this.helpLinkDocuments.Name = "helpLinkDocuments";
      this.helpLinkDocuments.Size = new Size(90, 17);
      this.helpLinkDocuments.TabIndex = 23;
      this.lblDocumentConversion5.AutoSize = true;
      this.lblDocumentConversion5.Location = new Point(75, 136);
      this.lblDocumentConversion5.Name = "lblDocumentConversion5";
      this.lblDocumentConversion5.Size = new Size(757, 13);
      this.lblDocumentConversion5.TabIndex = 22;
      this.lblDocumentConversion5.Text = "When enabled, copies of the documents in their original file format are not saved unless you configure the save option in the document conversion preferences. \rDocument conversion is a memory and CPU intensive process that may require additional system resources for client machines converting documents.  ";
      this.lblDocumentConversion3.AutoSize = true;
      this.lblDocumentConversion3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblDocumentConversion3.Location = new Point(10, 136);
      this.lblDocumentConversion3.Name = "lblDocumentConversion3";
      this.lblDocumentConversion3.Size = new Size(64, 13);
      this.lblDocumentConversion3.TabIndex = 21;
      this.lblDocumentConversion3.Text = "Important:";
      this.lblDocumentConversion1a.AutoSize = true;
      this.lblDocumentConversion1a.Location = new Point(10, 95);
      this.lblDocumentConversion1a.Name = "lblDocumentConversion1a";
      this.lblDocumentConversion1a.Size = new Size(645, 13);
      this.lblDocumentConversion1a.TabIndex = 20;
      this.lblDocumentConversion1a.Text = "Changes in the setting apply only when new documents are added and do not affect the formatting of documents already in the eFolder.";
      this.chkEnableRapidViewer.AutoSize = true;
      this.chkEnableRapidViewer.Location = new Point(11, 46);
      this.chkEnableRapidViewer.Name = "chkEnableRapidViewer";
      this.chkEnableRapidViewer.Size = new Size(167, 17);
      this.chkEnableRapidViewer.TabIndex = 12;
      this.chkEnableRapidViewer.Text = "Enable Document Conversion";
      this.chkEnableRapidViewer.UseVisualStyleBackColor = true;
      this.chkEnableRapidViewer.CheckedChanged += new EventHandler(this.chkEnableRapidViewer_CheckedChanged);
      this.lblDocumentConversion1.AutoSize = true;
      this.lblDocumentConversion1.Location = new Point(10, 80);
      this.lblDocumentConversion1.Name = "lblDocumentConversion1";
      this.lblDocumentConversion1.Size = new Size(705, 13);
      this.lblDocumentConversion1.TabIndex = 11;
      this.lblDocumentConversion1.Text = "When enabled, documents are converted to image files at 200 DPI when added to the eFolder. When disabled, documents are added in their original file format.";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcImageConversion);
      this.Controls.Add((Control) this.gcConversionPreferences);
      this.Name = nameof (DocumentConversionSetupControl);
      this.Size = new Size(631, 510);
      this.Resize += new EventHandler(this.DocumentConversionSetupControl_Resize);
      this.gcConversionPreferences.ResumeLayout(false);
      this.gcConversionPreferences.PerformLayout();
      this.gcImageConversion.ResumeLayout(false);
      this.gcImageConversion.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
