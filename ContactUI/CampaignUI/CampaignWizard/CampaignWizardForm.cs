// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CampaignUI.CampaignWizard.CampaignWizardForm
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.Common.UI.Wizard;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CampaignUI.CampaignWizard
{
  public class CampaignWizardForm : WizardBase
  {
    protected static CampaignWizardForm campaignWizardForm;
    private CampaignData campaignData;
    private bool saveAndStart;
    private IContainer components;
    private Label lblButtonSeparator;
    private EMHelpLink emHelpLink1;
    private Button btnSaveAndStart;
    private CheckBox chkSaveAsTemplate;

    public static CampaignWizardForm GetCampaignWizardForm()
    {
      return CampaignWizardForm.campaignWizardForm;
    }

    public static void DisableNavigation(Button btnAccept, Button btnCancel)
    {
      CampaignWizardForm.campaignWizardForm.AcceptButton = (IButtonControl) btnAccept;
      CampaignWizardForm.campaignWizardForm.CancelButton = (IButtonControl) null;
      CampaignWizardForm.campaignWizardForm.pnlFooter.Enabled = false;
    }

    public static void EnableNavigation()
    {
      CampaignWizardForm.campaignWizardForm.AcceptButton = (IButtonControl) CampaignWizardForm.campaignWizardForm.btnNext;
      CampaignWizardForm.campaignWizardForm.CancelButton = (IButtonControl) CampaignWizardForm.campaignWizardForm.btnCancel;
      CampaignWizardForm.campaignWizardForm.pnlFooter.Enabled = true;
    }

    public bool SaveAsTemplate => this.chkSaveAsTemplate.Checked;

    public bool SaveAndStart
    {
      get => this.saveAndStart;
      set => this.saveAndStart = value;
    }

    private CampaignWizardForm()
    {
      this.InitializeComponent();
      CampaignWizardForm.campaignWizardForm = this;
      this.chkSaveAsTemplate.Left = this.emHelpLink1.Left + 15;
      this.campaignData = CampaignData.GetCampaignData();
    }

    public CampaignWizardForm(bool isNewCampaign)
      : this(isNewCampaign, (FileSystemEntry) null)
    {
    }

    public CampaignWizardForm(bool isNewCampaign, FileSystemEntry templateEntry)
      : this()
    {
      this.campaignData.IsNewCampaign = isNewCampaign;
      this.campaignData.TemplateSourceEntry = templateEntry;
      if (isNewCampaign)
      {
        this.Text = "New Campaign Wizard";
        this.campaignData.WizardCampaign = EllieMae.EMLite.Campaign.Campaign.NewCampaign(Session.SessionObjects);
        if (this.campaignData.TemplateSourceEntry != null)
          this.campaignData.WizardCampaign.CampaignName = this.campaignData.TemplateSourceEntry.Name;
      }
      else
      {
        this.Text = "Edit Campaign Wizard";
        this.campaignData.WizardCampaign = EllieMae.EMLite.Campaign.Campaign.GetCampaign(this.campaignData.ActiveCampaign.CampaignId, Session.SessionObjects);
      }
      this.Current = (WizardItem) new CampaignDetailsPanel(1);
    }

    public CampaignWizardForm(EllieMae.EMLite.Campaign.Campaign campaign)
      : this(campaign, (FileSystemEntry) null)
    {
    }

    public CampaignWizardForm(EllieMae.EMLite.Campaign.Campaign campaign, FileSystemEntry templateEntry)
      : this()
    {
      this.campaignData.WizardCampaign = campaign;
      this.campaignData.TemplateSourceEntry = templateEntry;
      this.Text = "Edit Campaign Wizard";
      this.Current = (WizardItem) new CampaignDetailsPanel(1);
    }

    protected override void ChangePanel(WizardItem newPanel)
    {
      base.ChangePanel(newPanel);
      if (!(newPanel is CampaignWizardItem campaignWizardItem))
        return;
      campaignWizardItem.CampaignWizardForm = this;
    }

    protected override void OnItemChanged()
    {
      base.OnItemChanged();
      if (!(this.Current is CampaignWizardItem current))
        return;
      this.emHelpLink1.Visible = current.HelpLinkVisible;
      this.chkSaveAsTemplate.Visible = current.SaveAsTemplateVisible;
      if (this.btnSaveAndStart.Visible == current.SaveAndStartVisible)
        return;
      this.btnSaveAndStart.Visible = current.SaveAndStartVisible;
      if (this.btnSaveAndStart.Visible)
      {
        this.btnSaveAndStart.Left = this.btnCancel.Left - this.btnSaveAndStart.Width - 10;
        this.btnNext.Left = this.btnSaveAndStart.Left - this.btnNext.Width - 10;
        this.btnBack.Left = this.btnNext.Left - this.btnBack.Width - 10;
      }
      else
      {
        this.btnNext.Left = this.btnCancel.Left - this.btnNext.Width - 10;
        this.btnBack.Left = this.btnNext.Left - this.btnBack.Width - 10;
        this.btnSaveAndStart.Left = this.btnBack.Left - this.btnSaveAndStart.Width - 10;
      }
    }

    protected override void OnCancelling(CancelEventArgs e)
    {
      base.OnCancelling(e);
      if (!this.campaignData.WizardCampaign.IsDirty || DialogResult.No != Utils.Dialog((IWin32Window) this, "Your changes will be discarded.\nDo you want to cancel?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2))
        return;
      e.Cancel = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void btnSaveAndStart_Click(object sender, EventArgs e)
    {
      this.saveAndStart = true;
      this.Next();
    }

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      JedHelp.ShowHelp(this.emHelpLink1.HelpTag);
    }

    private void InitializeComponent()
    {
      this.lblButtonSeparator = new Label();
      this.emHelpLink1 = new EMHelpLink();
      this.chkSaveAsTemplate = new CheckBox();
      this.btnSaveAndStart = new Button();
      this.pnlFooter.SuspendLayout();
      this.SuspendLayout();
      this.btnNext.Location = new Point(501, 5);
      this.btnNext.Size = new Size(75, 22);
      this.btnNext.TabIndex = 100;
      this.btnCancel.Location = new Point(586, 5);
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 102;
      this.pnlFooter.Controls.Add((Control) this.btnSaveAndStart);
      this.pnlFooter.Controls.Add((Control) this.chkSaveAsTemplate);
      this.pnlFooter.Controls.Add((Control) this.emHelpLink1);
      this.pnlFooter.Controls.Add((Control) this.lblButtonSeparator);
      this.pnlFooter.Location = new Point(0, 448);
      this.pnlFooter.Size = new Size(671, 32);
      this.pnlFooter.Controls.SetChildIndex((Control) this.btnBack, 0);
      this.pnlFooter.Controls.SetChildIndex((Control) this.btnNext, 0);
      this.pnlFooter.Controls.SetChildIndex((Control) this.btnCancel, 0);
      this.pnlFooter.Controls.SetChildIndex((Control) this.lblButtonSeparator, 0);
      this.pnlFooter.Controls.SetChildIndex((Control) this.emHelpLink1, 0);
      this.pnlFooter.Controls.SetChildIndex((Control) this.chkSaveAsTemplate, 0);
      this.pnlFooter.Controls.SetChildIndex((Control) this.btnSaveAndStart, 0);
      this.btnBack.Location = new Point(416, 5);
      this.btnBack.Size = new Size(75, 22);
      this.btnBack.TabIndex = 101;
      this.groupBox1.Location = new Point(0, 446);
      this.groupBox1.Size = new Size(671, 2);
      this.groupBox1.Visible = false;
      this.lblButtonSeparator.BorderStyle = BorderStyle.FixedSingle;
      this.lblButtonSeparator.Location = new Point(7, 0);
      this.lblButtonSeparator.Name = "lblButtonSeparator";
      this.lblButtonSeparator.Size = new Size(657, 1);
      this.lblButtonSeparator.TabIndex = 103;
      this.lblButtonSeparator.Visible = false;
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.HelpTag = "Campaign Details";
      this.emHelpLink1.Location = new Point(10, 7);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 17);
      this.emHelpLink1.TabIndex = 104;
      this.emHelpLink1.Visible = false;
      this.chkSaveAsTemplate.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.chkSaveAsTemplate.AutoSize = true;
      this.chkSaveAsTemplate.Location = new Point(100, 7);
      this.chkSaveAsTemplate.Name = "chkSaveAsTemplate";
      this.chkSaveAsTemplate.Size = new Size(162, 18);
      this.chkSaveAsTemplate.TabIndex = 105;
      this.chkSaveAsTemplate.Text = "Save as Campaign Template";
      this.chkSaveAsTemplate.UseVisualStyleBackColor = true;
      this.chkSaveAsTemplate.Visible = false;
      this.btnSaveAndStart.Location = new Point(262, 5);
      this.btnSaveAndStart.Name = "btnSaveAndStart";
      this.btnSaveAndStart.Size = new Size(145, 22);
      this.btnSaveAndStart.TabIndex = 106;
      this.btnSaveAndStart.Text = "Sa&ve and Start Campaign";
      this.btnSaveAndStart.UseVisualStyleBackColor = true;
      this.btnSaveAndStart.Visible = false;
      this.btnSaveAndStart.Click += new EventHandler(this.btnSaveAndStart_Click);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.BackButtonVisible = true;
      this.ClientSize = new Size(671, 480);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.KeyPreview = true;
      this.MaximumSize = new Size(677, 512);
      this.MinimizeBox = false;
      this.Name = nameof (CampaignWizardForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.pnlFooter.ResumeLayout(false);
      this.pnlFooter.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
