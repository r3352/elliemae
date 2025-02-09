// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CampaignUI.CampaignWizard.CampaignWizardItem
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.BizLayer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Wizard;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CampaignUI.CampaignWizard
{
  public class CampaignWizardItem : WizardItem
  {
    protected CampaignWizardForm frmCampaignWizard;
    private IContainer components;
    private Label lblStepSeparator;
    protected Panel pnlStepTitle;
    protected Panel pnlMainContent;
    protected Label lblStep;

    [DllImport("kernel32.dll")]
    public static extern bool Beep(int freq, int duration);

    public CampaignWizardForm CampaignWizardForm
    {
      get => this.frmCampaignWizard;
      set => this.frmCampaignWizard = value;
    }

    public CampaignWizardItem() => this.InitializeComponent();

    protected void DisplayBrokenRules(BusinessBase businessObject)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (BrokenRules.Rule brokenRules in (CollectionBase) businessObject.BrokenRulesCollection)
        stringBuilder.Append(brokenRules.Description + ".\n");
      int num = (int) Utils.Dialog((IWin32Window) this, stringBuilder.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }

    public virtual bool HelpLinkVisible => false;

    public virtual bool SaveAsTemplateVisible => false;

    public virtual bool SaveAndStartVisible => false;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblStep = new Label();
      this.lblStepSeparator = new Label();
      this.pnlStepTitle = new Panel();
      this.pnlMainContent = new Panel();
      this.pnlStepTitle.SuspendLayout();
      this.SuspendLayout();
      this.lblStep.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblStep.Font = new Font("Arial", 8.25f, FontStyle.Bold);
      this.lblStep.Location = new Point(10, 9);
      this.lblStep.Name = "lblStep";
      this.lblStep.Size = new Size(651, 14);
      this.lblStep.TabIndex = 0;
      this.lblStep.Text = "Step 1 - Enter Campaign Details.";
      this.lblStep.TextAlign = ContentAlignment.MiddleLeft;
      this.lblStepSeparator.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblStepSeparator.BorderStyle = BorderStyle.FixedSingle;
      this.lblStepSeparator.Location = new Point(7, 31);
      this.lblStepSeparator.Name = "lblStepSeparator";
      this.lblStepSeparator.Size = new Size(657, 1);
      this.lblStepSeparator.TabIndex = 1;
      this.lblStepSeparator.Text = "label1";
      this.lblStepSeparator.Visible = false;
      this.pnlStepTitle.Controls.Add((Control) this.lblStepSeparator);
      this.pnlStepTitle.Controls.Add((Control) this.lblStep);
      this.pnlStepTitle.Dock = DockStyle.Top;
      this.pnlStepTitle.Location = new Point(0, 0);
      this.pnlStepTitle.Name = "pnlStepTitle";
      this.pnlStepTitle.Size = new Size(671, 32);
      this.pnlStepTitle.TabIndex = 13;
      this.pnlMainContent.Dock = DockStyle.Fill;
      this.pnlMainContent.Location = new Point(0, 32);
      this.pnlMainContent.Name = "pnlMainContent";
      this.pnlMainContent.Size = new Size(671, 414);
      this.pnlMainContent.TabIndex = 14;
      this.Controls.Add((Control) this.pnlMainContent);
      this.Controls.Add((Control) this.pnlStepTitle);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Location = new Point(7, 7);
      this.Name = nameof (CampaignWizardItem);
      this.Size = new Size(671, 446);
      this.pnlStepTitle.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
