// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CampaignUI.CampaignWizard.CampaignSummaryPanel
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer.Campaign;
using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Wizard;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CampaignUI.CampaignWizard
{
  public class CampaignSummaryPanel : CampaignWizardItem
  {
    private int stepNumber;
    private CampaignData campaignData;
    private EllieMae.EMLite.Campaign.Campaign campaign;
    private Label lblContactTypeHdr;
    private Label lblCampaignNameHdr;
    private Label lblDescriptionHdr;
    private Label lblDetails;
    private Label lblSteps;
    private Label lblDescription;
    private ToolTip tipCampaignSummary;
    private Label lblCampaignName;
    private Label lblContactType;
    private IContainer components;
    private GroupBox grpDetails;
    private FormattedLabel lblContactManagement;
    private GroupBox grpContactManagement;
    private Label lblManagementType;
    private Label lblRunSearch;
    private Label lblRunSearchHdr;
    private Label lblManagementTypeHdr;
    private GridView gvSteps;

    public CampaignSummaryPanel(int stepNumber)
    {
      this.stepNumber = stepNumber;
      this.InitializeComponent();
      this.campaignData = CampaignData.GetCampaignData();
      this.campaign = this.campaignData.WizardCampaign;
      this.populateControls();
    }

    protected void populateControls()
    {
      this.lblStep.Text = this.stepNumber.ToString() + ". Confirm Campaign Summary";
      this.fitLabelText(this.lblCampaignName, this.campaign.CampaignName);
      this.fitLabelText(this.lblDescription, this.campaign.CampaignDesc);
      this.lblContactType.Text = new ContactTypeNameProvider().GetName((object) this.campaign.ContactType) + " Contacts";
      if ((CampaignType.Manual & this.campaign.CampaignType) == CampaignType.Manual)
      {
        this.lblManagementType.Text = "Added and removed manually";
        this.lblRunSearchHdr.Visible = false;
        this.lblRunSearch.Visible = false;
      }
      else
      {
        if ((CampaignType.AutoDeleteQuery & this.campaign.CampaignType) == CampaignType.AutoDeleteQuery)
          this.lblManagementType.Text = "Added and removed automatically";
        else
          this.lblManagementType.Text = "Added automatically and removed manually";
        this.lblRunSearchHdr.Visible = true;
        this.lblRunSearch.Visible = true;
        if (CampaignFrequencyType.Custom == this.campaign.FrequencyType)
          this.lblRunSearch.Text = "Every " + (object) this.campaign.FrequencyInterval + " days";
        else
          this.lblRunSearch.Text = new CampaignFrequencyNameProvider().GetName((object) this.campaign.FrequencyType);
      }
      this.gvSteps.BeginUpdate();
      this.gvSteps.Items.Clear();
      if (this.campaign.CampaignSteps != null && this.campaign.CampaignSteps.Count != 0)
      {
        int num = 0;
        for (int index = 0; index < this.campaign.CampaignSteps.Count; ++index)
        {
          num += this.campaign.CampaignSteps[index].StepOffset;
          this.gvSteps.Items.Add(new GVItem(this.campaign.CampaignSteps[index].StepNumber.ToString())
          {
            SubItems = {
              (object) this.campaign.CampaignSteps[index].StepNumber.ToString(),
              (object) this.campaign.CampaignSteps[index].StepName,
              (object) this.campaign.CampaignSteps[index].StepDesc,
              (object) new ActivityTypeNameProvider().GetName((object) this.campaign.CampaignSteps[index].ActivityType),
              (object) this.campaign.CampaignSteps[index].StepOffset.ToString()
            },
            Tag = (object) this.campaign.CampaignSteps[index]
          });
        }
        this.gvSteps.Items[0].Selected = true;
        this.gvSteps.EnsureVisible(this.gvSteps.Items[0].Index);
        this.lblSteps.Text = "Steps (" + this.campaign.CampaignSteps.Count.ToString() + ")";
      }
      this.gvSteps.EndUpdate();
    }

    private void fitLabelText(Label label, string text)
    {
      using (Graphics graphics = label.CreateGraphics())
      {
        if (Utils.FitLabelText(graphics, label, text))
          this.tipCampaignSummary.SetToolTip((Control) label, string.Empty);
        else
          this.tipCampaignSummary.SetToolTip((Control) label, Utils.FitToolTipText(graphics, label.Font, 400f, text));
      }
    }

    private bool processUserEntry()
    {
      try
      {
        if (this.frmCampaignWizard.SaveAsTemplate && !this.saveTemplate())
          return false;
        if (!this.saveCampaign())
          return false;
      }
      finally
      {
        this.frmCampaignWizard.SaveAndStart = false;
      }
      return true;
    }

    private bool saveTemplate()
    {
      CampaignExplorerDialog campaignExplorerDialog = new CampaignExplorerDialog(FSExplorer.DialogMode.SaveFiles);
      campaignExplorerDialog.Campaign = this.campaign;
      if (this.campaignData.TemplateSourceEntry != null)
        campaignExplorerDialog.CurrentFolder = this.campaignData.TemplateSourceEntry;
      if (DialogResult.OK != campaignExplorerDialog.ShowDialog((IWin32Window) this))
        return false;
      this.campaignData.TemplateTargetEntry = campaignExplorerDialog.CurrentFolder;
      return true;
    }

    private void saveCampaignToTemplate(string templatePath)
    {
      try
      {
        using (TextWriter textWriter = (TextWriter) new StreamWriter(templatePath, false))
          new XmlSerializer(typeof (EllieMae.EMLite.Campaign.Campaign)).Serialize(textWriter, (object) this.campaign);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Exception: " + ex.Message, "Xml Deserialization Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    protected bool saveCampaign()
    {
      while (true)
      {
        try
        {
          Cursor.Current = Cursors.WaitCursor;
          this.campaign = (EllieMae.EMLite.Campaign.Campaign) this.campaign.Save();
          this.campaignData.WizardCampaign = this.campaign;
          if (this.frmCampaignWizard.SaveAndStart)
          {
            CampaignStartDialog campaignStartDialog = new CampaignStartDialog();
            int num = (int) campaignStartDialog.ShowDialog();
            if (CampaignStartDialog.SelectionTypes.Cancel == campaignStartDialog.UserSelection)
              return false;
            this.campaign.Start();
            break;
          }
          break;
        }
        catch (Exception ex)
        {
          if (0 <= ex.Message.IndexOf("Violation of UNIQUE KEY constraint 'UK_Campaign_UserId_CampaignName'"))
          {
            DuplicateCampaignNameDialog campaignNameDialog = new DuplicateCampaignNameDialog();
            campaignNameDialog.CampaignName = this.campaign.CampaignName;
            if (DialogResult.Cancel == campaignNameDialog.ShowDialog())
              return false;
            this.campaign.CampaignName = campaignNameDialog.CampaignName;
          }
          else
            throw;
        }
        finally
        {
          Cursor.Current = Cursors.Default;
        }
      }
      return true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public override bool NextEnabled => true;

    public override string NextLabel => "&Save";

    public override bool BackEnabled => true;

    public override bool CancelEnabled => true;

    public override WizardItem Next()
    {
      this.processUserEntry();
      return WizardItem.Finished;
    }

    public override WizardItem Back()
    {
      if ((CampaignType.AutoDeleteQuery & this.campaign.CampaignType) == CampaignType.AutoDeleteQuery)
        return (WizardItem) new CampaignQueryPanel(this.stepNumber - 1, QueryEditMode.DeleteQuery);
      return (CampaignType.AutoAddQuery & this.campaign.CampaignType) == CampaignType.AutoAddQuery ? (WizardItem) new CampaignQueryPanel(this.stepNumber - 1, QueryEditMode.AddQuery) : (WizardItem) new CampaignContactsPanel(this.stepNumber - 1);
    }

    public override bool SaveAsTemplateVisible => true;

    public override bool SaveAndStartVisible => true;

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      this.lblContactTypeHdr = new Label();
      this.lblCampaignNameHdr = new Label();
      this.lblDescriptionHdr = new Label();
      this.lblDetails = new Label();
      this.lblSteps = new Label();
      this.lblCampaignName = new Label();
      this.lblDescription = new Label();
      this.lblContactType = new Label();
      this.tipCampaignSummary = new ToolTip(this.components);
      this.grpDetails = new GroupBox();
      this.lblContactManagement = new FormattedLabel();
      this.grpContactManagement = new GroupBox();
      this.lblRunSearchHdr = new Label();
      this.lblManagementTypeHdr = new Label();
      this.lblRunSearch = new Label();
      this.lblManagementType = new Label();
      this.gvSteps = new GridView();
      this.pnlStepTitle.SuspendLayout();
      this.pnlMainContent.SuspendLayout();
      this.grpDetails.SuspendLayout();
      this.grpContactManagement.SuspendLayout();
      this.SuspendLayout();
      this.pnlStepTitle.TabIndex = 0;
      this.pnlMainContent.Controls.Add((Control) this.gvSteps);
      this.pnlMainContent.Controls.Add((Control) this.grpContactManagement);
      this.pnlMainContent.Controls.Add((Control) this.lblContactManagement);
      this.pnlMainContent.Controls.Add((Control) this.grpDetails);
      this.pnlMainContent.Controls.Add((Control) this.lblSteps);
      this.pnlMainContent.Controls.Add((Control) this.lblDetails);
      this.pnlMainContent.TabIndex = 1;
      this.lblStep.Text = "5. Confirm Campaign Summary";
      this.lblContactTypeHdr.AutoSize = true;
      this.lblContactTypeHdr.Font = new Font("Arial", 8.25f);
      this.lblContactTypeHdr.Location = new Point(10, 64);
      this.lblContactTypeHdr.Name = "lblContactTypeHdr";
      this.lblContactTypeHdr.Size = new Size(74, 14);
      this.lblContactTypeHdr.TabIndex = 2;
      this.lblContactTypeHdr.Text = "Contact Type:";
      this.lblContactTypeHdr.TextAlign = ContentAlignment.MiddleLeft;
      this.lblCampaignNameHdr.AutoSize = true;
      this.lblCampaignNameHdr.Font = new Font("Arial", 8.25f);
      this.lblCampaignNameHdr.Location = new Point(10, 16);
      this.lblCampaignNameHdr.Name = "lblCampaignNameHdr";
      this.lblCampaignNameHdr.Size = new Size(87, 14);
      this.lblCampaignNameHdr.TabIndex = 0;
      this.lblCampaignNameHdr.Text = "Campaign Name:";
      this.lblCampaignNameHdr.TextAlign = ContentAlignment.MiddleLeft;
      this.lblDescriptionHdr.AutoSize = true;
      this.lblDescriptionHdr.Font = new Font("Arial", 8.25f);
      this.lblDescriptionHdr.Location = new Point(10, 40);
      this.lblDescriptionHdr.Name = "lblDescriptionHdr";
      this.lblDescriptionHdr.Size = new Size(64, 14);
      this.lblDescriptionHdr.TabIndex = 1;
      this.lblDescriptionHdr.Text = "Description:";
      this.lblDescriptionHdr.TextAlign = ContentAlignment.MiddleLeft;
      this.lblDetails.AutoSize = true;
      this.lblDetails.Font = new Font("Arial", 8.25f, FontStyle.Bold);
      this.lblDetails.Location = new Point(22, 3);
      this.lblDetails.Name = "lblDetails";
      this.lblDetails.Size = new Size(44, 14);
      this.lblDetails.TabIndex = 4;
      this.lblDetails.Text = "Details";
      this.lblDetails.TextAlign = ContentAlignment.MiddleLeft;
      this.lblSteps.AutoSize = true;
      this.lblSteps.Font = new Font("Arial", 8.25f, FontStyle.Bold);
      this.lblSteps.Location = new Point(22, 118);
      this.lblSteps.Name = "lblSteps";
      this.lblSteps.Size = new Size(56, 14);
      this.lblSteps.TabIndex = 5;
      this.lblSteps.Text = "Steps (0)";
      this.lblSteps.TextAlign = ContentAlignment.MiddleLeft;
      this.lblCampaignName.Font = new Font("Arial", 8.25f);
      this.lblCampaignName.Location = new Point(105, 16);
      this.lblCampaignName.Name = "lblCampaignName";
      this.lblCampaignName.Size = new Size(510, 14);
      this.lblCampaignName.TabIndex = 10;
      this.lblCampaignName.Text = "New Campaign Name";
      this.lblCampaignName.TextAlign = ContentAlignment.MiddleLeft;
      this.lblDescription.Font = new Font("Arial", 8.25f);
      this.lblDescription.Location = new Point(105, 40);
      this.lblDescription.Name = "lblDescription";
      this.lblDescription.Size = new Size(510, 14);
      this.lblDescription.TabIndex = 11;
      this.lblDescription.Text = "New Campaign Description";
      this.lblDescription.TextAlign = ContentAlignment.MiddleLeft;
      this.lblContactType.AutoSize = true;
      this.lblContactType.Font = new Font("Arial", 8.25f);
      this.lblContactType.Location = new Point(105, 64);
      this.lblContactType.Name = "lblContactType";
      this.lblContactType.Size = new Size(54, 14);
      this.lblContactType.TabIndex = 12;
      this.lblContactType.Text = "Borrower";
      this.lblContactType.TextAlign = ContentAlignment.MiddleLeft;
      this.grpDetails.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.grpDetails.Controls.Add((Control) this.lblCampaignNameHdr);
      this.grpDetails.Controls.Add((Control) this.lblDescriptionHdr);
      this.grpDetails.Controls.Add((Control) this.lblContactTypeHdr);
      this.grpDetails.Controls.Add((Control) this.lblCampaignName);
      this.grpDetails.Controls.Add((Control) this.lblDescription);
      this.grpDetails.Controls.Add((Control) this.lblContactType);
      this.grpDetails.Location = new Point(25, 20);
      this.grpDetails.Name = "grpDetails";
      this.grpDetails.Size = new Size(621, 88);
      this.grpDetails.TabIndex = 25;
      this.grpDetails.TabStop = false;
      this.lblContactManagement.Location = new Point(25, 265);
      this.lblContactManagement.Name = "lblContactManagement";
      this.lblContactManagement.Size = new Size(436, 16);
      this.lblContactManagement.TabIndex = 26;
      this.lblContactManagement.Text = "<b>Contact Management</b> (You can change this information after you start the campaign)";
      this.grpContactManagement.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.grpContactManagement.Controls.Add((Control) this.lblRunSearchHdr);
      this.grpContactManagement.Controls.Add((Control) this.lblManagementTypeHdr);
      this.grpContactManagement.Controls.Add((Control) this.lblRunSearch);
      this.grpContactManagement.Controls.Add((Control) this.lblManagementType);
      this.grpContactManagement.Location = new Point(25, 282);
      this.grpContactManagement.Name = "grpContactManagement";
      this.grpContactManagement.Size = new Size(621, 64);
      this.grpContactManagement.TabIndex = 27;
      this.grpContactManagement.TabStop = false;
      this.lblRunSearchHdr.AutoSize = true;
      this.lblRunSearchHdr.Location = new Point(10, 40);
      this.lblRunSearchHdr.Name = "lblRunSearchHdr";
      this.lblRunSearchHdr.Size = new Size(67, 14);
      this.lblRunSearchHdr.TabIndex = 31;
      this.lblRunSearchHdr.Text = "Run Search:";
      this.lblManagementTypeHdr.AutoSize = true;
      this.lblManagementTypeHdr.Location = new Point(10, 16);
      this.lblManagementTypeHdr.Name = "lblManagementTypeHdr";
      this.lblManagementTypeHdr.Size = new Size(98, 14);
      this.lblManagementTypeHdr.TabIndex = 30;
      this.lblManagementTypeHdr.Text = "Management Type:";
      this.lblRunSearch.AutoSize = true;
      this.lblRunSearch.Location = new Point(105, 40);
      this.lblRunSearch.Name = "lblRunSearch";
      this.lblRunSearch.Size = new Size(30, 14);
      this.lblRunSearch.TabIndex = 29;
      this.lblRunSearch.Text = "Daily";
      this.lblManagementType.AutoSize = true;
      this.lblManagementType.Location = new Point(105, 16);
      this.lblManagementType.Name = "lblManagementType";
      this.lblManagementType.Size = new Size(150, 14);
      this.lblManagementType.TabIndex = 28;
      this.lblManagementType.Text = "Added and removed manually";
      this.gvSteps.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "gvcStepId";
      gvColumn1.Text = "Step Id";
      gvColumn1.Width = 0;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "gvcStepNumber";
      gvColumn2.Text = "Step";
      gvColumn2.Width = 41;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "gvcStepName";
      gvColumn3.Text = "Name";
      gvColumn3.Width = 184;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "gvcStepDescription";
      gvColumn4.Text = "Description";
      gvColumn4.Width = 248;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "gvcStepTaskType";
      gvColumn5.Text = "Task Type";
      gvColumn5.Width = 72;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "gvcStepInterval";
      gvColumn6.Text = "Interval";
      gvColumn6.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn6.Width = 52;
      this.gvSteps.Columns.AddRange(new GVColumn[6]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6
      });
      this.gvSteps.Location = new Point(25, 135);
      this.gvSteps.Name = "gvSteps";
      this.gvSteps.Size = new Size(621, 120);
      this.gvSteps.TabIndex = 28;
      this.Location = new Point(0, 0);
      this.Name = nameof (CampaignSummaryPanel);
      this.pnlStepTitle.ResumeLayout(false);
      this.pnlMainContent.ResumeLayout(false);
      this.pnlMainContent.PerformLayout();
      this.grpDetails.ResumeLayout(false);
      this.grpDetails.PerformLayout();
      this.grpContactManagement.ResumeLayout(false);
      this.grpContactManagement.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
