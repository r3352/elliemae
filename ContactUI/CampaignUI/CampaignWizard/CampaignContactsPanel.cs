// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CampaignUI.CampaignWizard.CampaignContactsPanel
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.BizLayer;
using EllieMae.EMLite.Common.UI.Wizard;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CampaignUI.CampaignWizard
{
  public class CampaignContactsPanel : CampaignWizardItem
  {
    private int stepNumber;
    private CampaignData campaignData;
    private EllieMae.EMLite.Campaign.Campaign campaign;
    private IContainer components;

    public CampaignContactsPanel(int stepNumber)
    {
      this.stepNumber = stepNumber;
      this.InitializeComponent();
      this.campaignData = CampaignData.GetCampaignData();
      this.campaign = this.campaignData.WizardCampaign;
      this.populateControls();
    }

    protected void populateControls()
    {
      this.lblStep.Text = this.stepNumber.ToString() + ". Add Contacts (Optional)";
      CampaignGroupControl campaignGroupControl = new CampaignGroupControl(this.campaign);
      campaignGroupControl.Dock = DockStyle.Fill;
      this.pnlMainContent.Controls.Add((Control) campaignGroupControl);
    }

    private bool processUserEntry()
    {
      if (!this.campaign.IsValid)
        this.DisplayBrokenRules((BusinessBase) this.campaign);
      return this.campaign.IsValid;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public override bool NextEnabled => true;

    public override bool BackEnabled => true;

    public override bool CancelEnabled => true;

    public override WizardItem Next()
    {
      return !this.processUserEntry() ? (WizardItem) null : (WizardItem) new CampaignSummaryPanel(this.stepNumber + 1);
    }

    public override WizardItem Back()
    {
      if (this.campaign.ContactGroupCount == 0)
        this.campaign.ContactGroup = (EllieMae.EMLite.ContactGroup.ContactGroup) null;
      return !this.processUserEntry() ? (WizardItem) null : (WizardItem) new ManageContactsPanel(this.stepNumber - 1);
    }

    private void InitializeComponent()
    {
      this.pnlStepTitle.SuspendLayout();
      this.SuspendLayout();
      this.pnlStepTitle.TabIndex = 0;
      this.pnlMainContent.TabIndex = 1;
      this.lblStep.Text = "4. Add Contacts (Optional)";
      this.Location = new Point(0, 0);
      this.Name = nameof (CampaignContactsPanel);
      this.pnlStepTitle.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
