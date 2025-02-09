// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CampaignUI.CampaignWizard.ManageContactsPanel
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.BizLayer;
using EllieMae.EMLite.ClientServer.Campaign;
using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.Common.UI.Wizard;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CampaignUI.CampaignWizard
{
  public class ManageContactsPanel : CampaignWizardItem
  {
    private int stepNumber;
    private CampaignData campaignData;
    private EllieMae.EMLite.Campaign.Campaign campaign;
    private CampaignFrequencyNameProvider frequencyNames = new CampaignFrequencyNameProvider();
    private IContainer components;
    private Label lblManageContacts;
    private RadioButton rbAutomatic;
    private RadioButton rbManual;
    private Panel pnlSearch;
    private Label lblFrequencyType;
    private CheckBox chkDeleteContacts;
    private CheckBox chkAddContacts;
    private ComboBox cboFrequencyType;
    private NumericUpDown nudFrequencyDays;
    private Label lblDays;
    private Panel pnlDays;

    public ManageContactsPanel(int stepNumber)
    {
      this.stepNumber = stepNumber;
      this.InitializeComponent();
      this.campaignData = CampaignData.GetCampaignData();
      this.campaign = this.campaignData.WizardCampaign;
      this.cboFrequencyType.Items.Clear();
      this.cboFrequencyType.Items.AddRange((object[]) this.frequencyNames.GetNames());
      this.populateControls();
    }

    protected void populateControls()
    {
      this.lblStep.Text = this.stepNumber.ToString() + ". Manage Contacts for the Campaign";
      if ((CampaignType.AutoAddQuery & this.campaign.CampaignType) == CampaignType.AutoAddQuery)
      {
        this.rbAutomatic.Checked = true;
        this.rbManual.Checked = false;
      }
      else
      {
        this.rbManual.Checked = true;
        this.rbAutomatic.Checked = false;
      }
    }

    private bool processUserEntry()
    {
      if (this.rbAutomatic.Checked)
      {
        this.campaign.CampaignType |= CampaignType.AutoAddQuery;
        this.campaign.CampaignType &= ~CampaignType.Manual;
        if (this.chkDeleteContacts.Checked)
          this.campaign.CampaignType |= CampaignType.AutoDeleteQuery;
        else
          this.campaign.CampaignType &= ~CampaignType.AutoDeleteQuery;
        this.campaign.FrequencyType = (CampaignFrequencyType) this.frequencyNames.GetValue(this.cboFrequencyType.SelectedItem.ToString());
        this.campaign.FrequencyInterval = CampaignFrequencyType.Custom == this.campaign.FrequencyType ? (int) this.nudFrequencyDays.Value : 0;
      }
      else
        this.campaign.CampaignType = CampaignType.Manual;
      if (!this.campaign.IsValid)
        this.DisplayBrokenRules((BusinessBase) this.campaign);
      return this.campaign.IsValid;
    }

    private void rbManual_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.rbManual.Checked)
        return;
      this.pnlSearch.Visible = false;
    }

    private void rbAutomatic_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.rbAutomatic.Checked)
        return;
      this.pnlSearch.Visible = true;
      if ((CampaignType.AutoDeleteQuery & this.campaign.CampaignType) == CampaignType.AutoDeleteQuery)
        this.chkDeleteContacts.Checked = true;
      this.cboFrequencyType.SelectedIndex = this.cboFrequencyType.Items.IndexOf((object) this.frequencyNames.GetName((object) this.campaign.FrequencyType));
    }

    private void cboFrequencyType_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (CampaignFrequencyType.Custom == (CampaignFrequencyType) this.frequencyNames.GetValue(this.cboFrequencyType.SelectedItem.ToString()))
      {
        this.nudFrequencyDays.Minimum = 1M;
        this.pnlDays.Visible = true;
      }
      else
      {
        this.nudFrequencyDays.Minimum = 0M;
        this.nudFrequencyDays.Value = 0M;
        this.pnlDays.Visible = false;
      }
    }

    public override bool NextEnabled => true;

    public override bool BackEnabled => true;

    public override bool CancelEnabled => true;

    public override WizardItem Next()
    {
      if (!this.processUserEntry())
        return (WizardItem) null;
      return (CampaignType.Manual & this.campaign.CampaignType) == CampaignType.Manual ? (WizardItem) new CampaignContactsPanel(this.stepNumber + 1) : (WizardItem) new CampaignQueryPanel(this.stepNumber + 1, QueryEditMode.AddQuery);
    }

    public override WizardItem Back()
    {
      return !this.processUserEntry() ? (WizardItem) null : (WizardItem) new CampaignStepsPanel(this.stepNumber - 1);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblManageContacts = new Label();
      this.rbAutomatic = new RadioButton();
      this.rbManual = new RadioButton();
      this.pnlSearch = new Panel();
      this.lblFrequencyType = new Label();
      this.cboFrequencyType = new ComboBox();
      this.pnlDays = new Panel();
      this.nudFrequencyDays = new NumericUpDown();
      this.lblDays = new Label();
      this.chkDeleteContacts = new CheckBox();
      this.chkAddContacts = new CheckBox();
      this.pnlStepTitle.SuspendLayout();
      this.pnlMainContent.SuspendLayout();
      this.pnlSearch.SuspendLayout();
      this.pnlDays.SuspendLayout();
      this.nudFrequencyDays.BeginInit();
      this.SuspendLayout();
      this.pnlMainContent.Controls.Add((Control) this.pnlSearch);
      this.pnlMainContent.Controls.Add((Control) this.rbAutomatic);
      this.pnlMainContent.Controls.Add((Control) this.rbManual);
      this.pnlMainContent.Controls.Add((Control) this.lblManageContacts);
      this.lblStep.Text = "3. Manage Contacts for the Campaign";
      this.lblManageContacts.AutoSize = true;
      this.lblManageContacts.Location = new Point(22, 3);
      this.lblManageContacts.Name = "lblManageContacts";
      this.lblManageContacts.Size = new Size(181, 14);
      this.lblManageContacts.TabIndex = 0;
      this.lblManageContacts.Text = "Select how to manage the contacts.";
      this.rbAutomatic.AutoSize = true;
      this.rbAutomatic.Font = new Font("Arial", 8.25f);
      this.rbAutomatic.Location = new Point(25, 50);
      this.rbAutomatic.Name = "rbAutomatic";
      this.rbAutomatic.Size = new Size(250, 18);
      this.rbAutomatic.TabIndex = 8;
      this.rbAutomatic.Text = "Automatically manage contacts using a search";
      this.rbAutomatic.CheckedChanged += new EventHandler(this.rbAutomatic_CheckedChanged);
      this.rbManual.AutoSize = true;
      this.rbManual.Checked = true;
      this.rbManual.Font = new Font("Arial", 8.25f);
      this.rbManual.Location = new Point(25, 27);
      this.rbManual.Name = "rbManual";
      this.rbManual.Size = new Size(153, 18);
      this.rbManual.TabIndex = 9;
      this.rbManual.TabStop = true;
      this.rbManual.Text = "Manually manage contacts";
      this.rbManual.CheckedChanged += new EventHandler(this.rbManual_CheckedChanged);
      this.pnlSearch.Controls.Add((Control) this.lblFrequencyType);
      this.pnlSearch.Controls.Add((Control) this.cboFrequencyType);
      this.pnlSearch.Controls.Add((Control) this.pnlDays);
      this.pnlSearch.Controls.Add((Control) this.chkDeleteContacts);
      this.pnlSearch.Controls.Add((Control) this.chkAddContacts);
      this.pnlSearch.Location = new Point(25, 73);
      this.pnlSearch.Name = "pnlSearch";
      this.pnlSearch.Size = new Size(450, 70);
      this.pnlSearch.TabIndex = 10;
      this.pnlSearch.Visible = false;
      this.lblFrequencyType.AutoSize = true;
      this.lblFrequencyType.Location = new Point(19, 49);
      this.lblFrequencyType.Name = "lblFrequencyType";
      this.lblFrequencyType.Size = new Size(214, 14);
      this.lblFrequencyType.TabIndex = 2;
      this.lblFrequencyType.Text = "How often do you want to run the search?";
      this.cboFrequencyType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFrequencyType.Location = new Point(239, 45);
      this.cboFrequencyType.Name = "cboFrequencyType";
      this.cboFrequencyType.Size = new Size(123, 22);
      this.cboFrequencyType.TabIndex = 3;
      this.cboFrequencyType.SelectedIndexChanged += new EventHandler(this.cboFrequencyType_SelectedIndexChanged);
      this.pnlDays.Controls.Add((Control) this.nudFrequencyDays);
      this.pnlDays.Controls.Add((Control) this.lblDays);
      this.pnlDays.Location = new Point(368, 45);
      this.pnlDays.Name = "pnlDays";
      this.pnlDays.Size = new Size(79, 22);
      this.pnlDays.TabIndex = 7;
      this.nudFrequencyDays.Location = new Point(0, 1);
      this.nudFrequencyDays.Maximum = new Decimal(new int[4]
      {
        999,
        0,
        0,
        0
      });
      this.nudFrequencyDays.Name = "nudFrequencyDays";
      this.nudFrequencyDays.Size = new Size(48, 20);
      this.nudFrequencyDays.TabIndex = 4;
      this.nudFrequencyDays.Value = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      this.lblDays.AutoSize = true;
      this.lblDays.Location = new Point(48, 4);
      this.lblDays.Name = "lblDays";
      this.lblDays.Size = new Size(31, 14);
      this.lblDays.TabIndex = 5;
      this.lblDays.Text = "days";
      this.lblDays.TextAlign = ContentAlignment.MiddleLeft;
      this.chkDeleteContacts.AutoSize = true;
      this.chkDeleteContacts.Location = new Point(22, 24);
      this.chkDeleteContacts.Name = "chkDeleteContacts";
      this.chkDeleteContacts.Size = new Size(175, 18);
      this.chkDeleteContacts.TabIndex = 1;
      this.chkDeleteContacts.Text = "Remove contacts automatically";
      this.chkDeleteContacts.UseVisualStyleBackColor = true;
      this.chkAddContacts.AutoSize = true;
      this.chkAddContacts.Checked = true;
      this.chkAddContacts.CheckState = CheckState.Checked;
      this.chkAddContacts.Enabled = false;
      this.chkAddContacts.Location = new Point(22, 0);
      this.chkAddContacts.Name = "chkAddContacts";
      this.chkAddContacts.Size = new Size(156, 18);
      this.chkAddContacts.TabIndex = 0;
      this.chkAddContacts.Text = "Add contacts automatically";
      this.chkAddContacts.UseVisualStyleBackColor = true;
      this.Location = new Point(0, 0);
      this.Name = nameof (ManageContactsPanel);
      this.pnlStepTitle.ResumeLayout(false);
      this.pnlMainContent.ResumeLayout(false);
      this.pnlMainContent.PerformLayout();
      this.pnlSearch.ResumeLayout(false);
      this.pnlSearch.PerformLayout();
      this.pnlDays.ResumeLayout(false);
      this.pnlDays.PerformLayout();
      this.nudFrequencyDays.EndInit();
      this.ResumeLayout(false);
    }
  }
}
