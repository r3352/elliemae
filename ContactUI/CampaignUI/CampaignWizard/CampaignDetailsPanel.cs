// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CampaignUI.CampaignWizard.CampaignDetailsPanel
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.BizLayer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Wizard;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CampaignUI.CampaignWizard
{
  public class CampaignDetailsPanel : CampaignWizardItem
  {
    private int stepNumber;
    private CampaignData campaignData;
    private EllieMae.EMLite.Campaign.Campaign campaign;
    private ToolTip toolTip1;
    private ContactTypeNameProvider contactTypeNames = new ContactTypeNameProvider();
    private IContainer components;
    private TextBox txtCampaignName;
    private Label lblDescription;
    private TextBox txtDescription;
    private Label lblContactType;
    private Label lblStartFrom;
    private FormattedLabel lblRequiredField;
    private FormattedLabel lblCampaignName;
    private RadioButton rbStartFromTemplate;
    private RadioButton rbStartFromBlank;
    private StandardIconButton icnSelectTemplate;
    private TextBox txtTemplateName;
    private Panel pnlStartFrom;
    private RadioButton rbBusiness;
    private RadioButton rbBorrower;
    private Panel pnlContactType;

    public CampaignDetailsPanel(int stepNumber)
    {
      this.stepNumber = stepNumber;
      this.InitializeComponent();
      this.campaignData = CampaignData.GetCampaignData();
      this.campaign = this.campaignData.WizardCampaign;
      this.populateControls();
    }

    protected void populateControls()
    {
      this.lblStep.Text = this.stepNumber.ToString() + ". Enter Campaign Details";
      this.icnSelectTemplate.Enabled = false;
      this.txtCampaignName.Text = this.campaign.CampaignName;
      this.txtDescription.Text = this.campaign.CampaignDesc;
      if (this.campaign.ContactType == EllieMae.EMLite.ContactUI.ContactType.Borrower)
        this.rbBorrower.Checked = true;
      else if (this.campaign.ContactType == EllieMae.EMLite.ContactUI.ContactType.BizPartner)
        this.rbBusiness.Checked = true;
      this.pnlContactType.Enabled = this.campaignData.IsNewCampaign;
    }

    private bool processUserEntry()
    {
      this.campaign.ContactType = this.rbBorrower.Checked ? EllieMae.EMLite.ContactUI.ContactType.Borrower : EllieMae.EMLite.ContactUI.ContactType.BizPartner;
      this.campaign.CampaignName = this.txtCampaignName.Text;
      this.campaign.CampaignDesc = this.txtDescription.Text;
      if (!this.campaign.IsValid)
        this.DisplayBrokenRules((BusinessBase) this.campaign);
      return this.campaign.IsValid;
    }

    private void loadCampaignFromTemplate(string templatePath)
    {
      try
      {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof (EllieMae.EMLite.Campaign.Campaign));
        xmlSerializer.UnknownNode += new XmlNodeEventHandler(this.xmlSerializer_UnknownNode);
        xmlSerializer.UnknownAttribute += new XmlAttributeEventHandler(this.xmlSerializer_UnknownAttribute);
        FileStream fileStream = new FileStream(templatePath, FileMode.Open);
        this.campaign = (EllieMae.EMLite.Campaign.Campaign) xmlSerializer.Deserialize((Stream) fileStream);
        this.campaignData.WizardCampaign = this.campaign;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Exception: " + ex.Message, "Xml Deserialization Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    protected void xmlSerializer_UnknownNode(object sender, XmlNodeEventArgs e)
    {
      int num = (int) MessageBox.Show("Unknown Node: " + e.Name + " - " + e.Text, "Xml Deserialization Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }

    protected void xmlSerializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
    {
      XmlAttribute attr = e.Attr;
      int num = (int) MessageBox.Show("Unknown Attribute: " + attr.Name + " = '" + attr.Value + "'", "Xml Deserialization Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void rbStartFromBlank_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.rbStartFromBlank.Checked || string.Empty == this.txtTemplateName.Text)
        return;
      if (DialogResult.No == Utils.Dialog((IWin32Window) this, "Selecting this option will clear all campaign details. Are you sure you want to start from a blank campaign?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2))
      {
        this.rbStartFromTemplate.Checked = true;
      }
      else
      {
        this.icnSelectTemplate.Enabled = false;
        this.txtTemplateName.Text = string.Empty;
        this.campaignData.WizardCampaign = EllieMae.EMLite.Campaign.Campaign.NewCampaign(Session.SessionObjects);
        this.campaignData.IsNewCampaign = true;
        this.campaign = this.campaignData.WizardCampaign;
        this.populateControls();
      }
    }

    private void rbStartFromTemplate_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.rbStartFromTemplate.Checked || string.Empty != this.txtTemplateName.Text)
        return;
      CampaignExplorerDialog campaignExplorerDialog = new CampaignExplorerDialog(FSExplorer.DialogMode.SelectFiles);
      if (DialogResult.OK == campaignExplorerDialog.ShowDialog((IWin32Window) this))
      {
        this.campaign = campaignExplorerDialog.Campaign;
        this.txtTemplateName.Text = this.campaign.CampaignName;
        this.campaignData.WizardCampaign = this.campaign;
        this.campaignData.IsNewCampaign = false;
        this.campaignData.TemplateSourceEntry = campaignExplorerDialog.CurrentFolder;
        this.populateControls();
        this.icnSelectTemplate.Enabled = true;
      }
      else
        this.rbStartFromBlank.Checked = true;
    }

    private void icnSelectTemplate_Click(object sender, EventArgs e)
    {
      CampaignExplorerDialog campaignExplorerDialog = new CampaignExplorerDialog(FSExplorer.DialogMode.SelectFiles);
      if (DialogResult.OK != campaignExplorerDialog.ShowDialog((IWin32Window) this))
        return;
      this.campaign = campaignExplorerDialog.Campaign;
      this.txtTemplateName.Text = this.campaign.CampaignName;
      this.campaignData.WizardCampaign = this.campaign;
      this.campaignData.IsNewCampaign = false;
      this.campaignData.TemplateSourceEntry = campaignExplorerDialog.CurrentFolder;
      this.populateControls();
    }

    public override bool BackVisible => false;

    public override bool NextEnabled => true;

    public override bool CancelEnabled => true;

    public override WizardItem Next()
    {
      return !this.processUserEntry() ? (WizardItem) null : (WizardItem) new CampaignStepsPanel(this.stepNumber + 1);
    }

    public override bool HelpLinkVisible => true;

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.txtCampaignName = new TextBox();
      this.lblDescription = new Label();
      this.txtDescription = new TextBox();
      this.lblContactType = new Label();
      this.lblStartFrom = new Label();
      this.lblRequiredField = new FormattedLabel();
      this.lblCampaignName = new FormattedLabel();
      this.rbStartFromBlank = new RadioButton();
      this.rbStartFromTemplate = new RadioButton();
      this.icnSelectTemplate = new StandardIconButton();
      this.txtTemplateName = new TextBox();
      this.rbBorrower = new RadioButton();
      this.rbBusiness = new RadioButton();
      this.pnlStartFrom = new Panel();
      this.pnlContactType = new Panel();
      this.toolTip1 = new ToolTip(this.components);
      this.pnlStepTitle.SuspendLayout();
      this.pnlMainContent.SuspendLayout();
      ((ISupportInitialize) this.icnSelectTemplate).BeginInit();
      this.pnlStartFrom.SuspendLayout();
      this.pnlContactType.SuspendLayout();
      this.SuspendLayout();
      this.pnlStepTitle.TabIndex = 0;
      this.pnlMainContent.Controls.Add((Control) this.pnlContactType);
      this.pnlMainContent.Controls.Add((Control) this.pnlStartFrom);
      this.pnlMainContent.Controls.Add((Control) this.txtTemplateName);
      this.pnlMainContent.Controls.Add((Control) this.icnSelectTemplate);
      this.pnlMainContent.Controls.Add((Control) this.lblCampaignName);
      this.pnlMainContent.Controls.Add((Control) this.lblRequiredField);
      this.pnlMainContent.Controls.Add((Control) this.lblStartFrom);
      this.pnlMainContent.Controls.Add((Control) this.lblContactType);
      this.pnlMainContent.Controls.Add((Control) this.txtCampaignName);
      this.pnlMainContent.Controls.Add((Control) this.lblDescription);
      this.pnlMainContent.Controls.Add((Control) this.txtDescription);
      this.pnlMainContent.TabIndex = 1;
      this.lblStep.TabIndex = 1;
      this.lblStep.Text = "1. Enter Campaign Details";
      this.txtCampaignName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtCampaignName.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtCampaignName.Location = new Point((int) sbyte.MaxValue, 57);
      this.txtCampaignName.MaxLength = 64;
      this.txtCampaignName.Name = "txtCampaignName";
      this.txtCampaignName.Size = new Size(522, 20);
      this.txtCampaignName.TabIndex = 1;
      this.lblDescription.AutoSize = true;
      this.lblDescription.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblDescription.Location = new Point(22, 89);
      this.lblDescription.Name = "lblDescription";
      this.lblDescription.Size = new Size(60, 13);
      this.lblDescription.TabIndex = 2;
      this.lblDescription.Text = "Description";
      this.lblDescription.TextAlign = ContentAlignment.MiddleLeft;
      this.txtDescription.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDescription.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtDescription.Location = new Point((int) sbyte.MaxValue, 87);
      this.txtDescription.MaxLength = 250;
      this.txtDescription.Multiline = true;
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.Size = new Size(522, 62);
      this.txtDescription.TabIndex = 3;
      this.lblContactType.AutoSize = true;
      this.lblContactType.Location = new Point(22, 161);
      this.lblContactType.Name = "lblContactType";
      this.lblContactType.Size = new Size(71, 14);
      this.lblContactType.TabIndex = 4;
      this.lblContactType.Text = "Contact Type";
      this.lblContactType.TextAlign = ContentAlignment.MiddleLeft;
      this.lblStartFrom.AutoSize = true;
      this.lblStartFrom.Location = new Point(22, 30);
      this.lblStartFrom.Name = "lblStartFrom";
      this.lblStartFrom.Size = new Size(57, 14);
      this.lblStartFrom.TabIndex = 10;
      this.lblStartFrom.Text = "Start From";
      this.lblStartFrom.TextAlign = ContentAlignment.MiddleLeft;
      this.lblRequiredField.Location = new Point(22, 3);
      this.lblRequiredField.Name = "lblRequiredField";
      this.lblRequiredField.Size = new Size(90, 16);
      this.lblRequiredField.TabIndex = 12;
      this.lblRequiredField.Text = "<c value=\"Red\">*</c> Required Field";
      this.lblCampaignName.Location = new Point(22, 59);
      this.lblCampaignName.Name = "lblCampaignName";
      this.lblCampaignName.Size = new Size(98, 16);
      this.lblCampaignName.TabIndex = 13;
      this.lblCampaignName.Text = "Campaign Name<c value=\"Red\">*</c>";
      this.rbStartFromBlank.AutoSize = true;
      this.rbStartFromBlank.Checked = true;
      this.rbStartFromBlank.Location = new Point(0, 1);
      this.rbStartFromBlank.Name = "rbStartFromBlank";
      this.rbStartFromBlank.Size = new Size(51, 18);
      this.rbStartFromBlank.TabIndex = 14;
      this.rbStartFromBlank.TabStop = true;
      this.rbStartFromBlank.Text = "Blank";
      this.rbStartFromBlank.UseVisualStyleBackColor = true;
      this.rbStartFromBlank.CheckedChanged += new EventHandler(this.rbStartFromBlank_CheckedChanged);
      this.rbStartFromTemplate.AutoSize = true;
      this.rbStartFromTemplate.Location = new Point(58, 1);
      this.rbStartFromTemplate.Name = "rbStartFromTemplate";
      this.rbStartFromTemplate.Size = new Size(118, 18);
      this.rbStartFromTemplate.TabIndex = 15;
      this.rbStartFromTemplate.TabStop = true;
      this.rbStartFromTemplate.Text = "Campaign Template";
      this.rbStartFromTemplate.UseVisualStyleBackColor = true;
      this.rbStartFromTemplate.CheckedChanged += new EventHandler(this.rbStartFromTemplate_CheckedChanged);
      this.icnSelectTemplate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.icnSelectTemplate.BackColor = Color.Transparent;
      this.icnSelectTemplate.Enabled = false;
      this.icnSelectTemplate.Location = new Point(633, 29);
      this.icnSelectTemplate.Name = "icnSelectTemplate";
      this.icnSelectTemplate.Size = new Size(16, 16);
      this.icnSelectTemplate.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.icnSelectTemplate.TabIndex = 16;
      this.icnSelectTemplate.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.icnSelectTemplate, "Select a Template");
      this.icnSelectTemplate.Click += new EventHandler(this.icnSelectTemplate_Click);
      this.txtTemplateName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtTemplateName.Location = new Point(309, 27);
      this.txtTemplateName.Name = "txtTemplateName";
      this.txtTemplateName.ReadOnly = true;
      this.txtTemplateName.Size = new Size(318, 20);
      this.txtTemplateName.TabIndex = 71;
      this.txtTemplateName.Tag = (object) "";
      this.rbBorrower.AutoSize = true;
      this.rbBorrower.Checked = true;
      this.rbBorrower.Location = new Point(0, 1);
      this.rbBorrower.Name = "rbBorrower";
      this.rbBorrower.Size = new Size(72, 18);
      this.rbBorrower.TabIndex = 72;
      this.rbBorrower.TabStop = true;
      this.rbBorrower.Text = "Borrower";
      this.rbBorrower.UseVisualStyleBackColor = true;
      this.rbBusiness.AutoSize = true;
      this.rbBusiness.Location = new Point(73, 1);
      this.rbBusiness.Name = "rbBusiness";
      this.rbBusiness.Size = new Size(70, 18);
      this.rbBusiness.TabIndex = 73;
      this.rbBusiness.TabStop = true;
      this.rbBusiness.Text = "Business";
      this.rbBusiness.UseVisualStyleBackColor = true;
      this.pnlStartFrom.Controls.Add((Control) this.rbStartFromTemplate);
      this.pnlStartFrom.Controls.Add((Control) this.rbStartFromBlank);
      this.pnlStartFrom.Location = new Point((int) sbyte.MaxValue, 27);
      this.pnlStartFrom.Name = "pnlStartFrom";
      this.pnlStartFrom.Size = new Size(176, 20);
      this.pnlStartFrom.TabIndex = 74;
      this.pnlContactType.Controls.Add((Control) this.rbBorrower);
      this.pnlContactType.Controls.Add((Control) this.rbBusiness);
      this.pnlContactType.Location = new Point((int) sbyte.MaxValue, 158);
      this.pnlContactType.Name = "pnlContactType";
      this.pnlContactType.Size = new Size(140, 20);
      this.pnlContactType.TabIndex = 75;
      this.Location = new Point(0, 0);
      this.Name = nameof (CampaignDetailsPanel);
      this.pnlStepTitle.ResumeLayout(false);
      this.pnlMainContent.ResumeLayout(false);
      this.pnlMainContent.PerformLayout();
      ((ISupportInitialize) this.icnSelectTemplate).EndInit();
      this.pnlStartFrom.ResumeLayout(false);
      this.pnlStartFrom.PerformLayout();
      this.pnlContactType.ResumeLayout(false);
      this.pnlContactType.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
