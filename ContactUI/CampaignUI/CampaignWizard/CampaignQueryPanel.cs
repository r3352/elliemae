// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CampaignUI.CampaignWizard.CampaignQueryPanel
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.BizLayer;
using EllieMae.EMLite.ClientServer.Campaign;
using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.Common.UI.Wizard;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CampaignUI.CampaignWizard
{
  public class CampaignQueryPanel : CampaignWizardItem
  {
    private int stepNumber;
    private QueryEditMode queryEditMode;
    private CampaignData campaignData;
    private EllieMae.EMLite.Campaign.Campaign campaign;
    private IContainer components;

    public CampaignQueryPanel(int stepNumber, QueryEditMode queryEditMode)
    {
      this.stepNumber = stepNumber;
      this.queryEditMode = queryEditMode;
      this.InitializeComponent();
      this.campaignData = CampaignData.GetCampaignData();
      this.campaign = this.campaignData.WizardCampaign;
      this.populateControls();
    }

    protected void populateControls()
    {
      this.lblStep.Text = this.stepNumber.ToString();
      this.lblStep.Text += this.queryEditMode == QueryEditMode.AddQuery ? ". Create a Search for Adding Contacts" : ". Create a Search for Removing Contacts";
      WizardQueryControl wizardQueryControl = new WizardQueryControl(this.queryEditMode, true, this.campaign);
      wizardQueryControl.Dock = DockStyle.Fill;
      this.pnlMainContent.Controls.Add((Control) wizardQueryControl);
    }

    private bool processUserEntry()
    {
      if (this.queryEditMode == QueryEditMode.AddQuery && !this.campaign.AddQuery.IsValid)
      {
        this.DisplayBrokenRules((BusinessBase) this.campaign.AddQuery);
        return false;
      }
      if (QueryEditMode.DeleteQuery != this.queryEditMode || (CampaignType.AutoDeleteQuery & this.campaign.CampaignType) != CampaignType.AutoDeleteQuery || this.campaign.DeleteQuery.IsValid)
        return true;
      this.DisplayBrokenRules((BusinessBase) this.campaign.DeleteQuery);
      return false;
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
      if (!this.processUserEntry())
        return (WizardItem) null;
      return this.queryEditMode == QueryEditMode.AddQuery && CampaignType.AutoDeleteQuery == (CampaignType.AutoDeleteQuery & this.campaign.CampaignType) ? (WizardItem) new CampaignQueryPanel(this.stepNumber + 1, QueryEditMode.DeleteQuery) : (WizardItem) new CampaignSummaryPanel(this.stepNumber + 1);
    }

    public override WizardItem Back()
    {
      return QueryEditMode.DeleteQuery == this.queryEditMode ? (WizardItem) new CampaignQueryPanel(this.stepNumber - 1, QueryEditMode.AddQuery) : (WizardItem) new ManageContactsPanel(this.stepNumber - 1);
    }

    private void InitializeComponent()
    {
      this.pnlStepTitle.SuspendLayout();
      this.SuspendLayout();
      this.pnlStepTitle.TabIndex = 0;
      this.pnlMainContent.TabIndex = 1;
      this.lblStep.Text = "4. Create a Search for Adding Contacts";
      this.Location = new Point(0, 0);
      this.Name = nameof (CampaignQueryPanel);
      this.pnlStepTitle.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
