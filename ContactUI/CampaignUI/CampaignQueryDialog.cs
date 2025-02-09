// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CampaignUI.CampaignQueryDialog
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.BizLayer;
using EllieMae.EMLite.ClientServer.Campaign;
using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.ContactGroup;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CampaignUI
{
  public class CampaignQueryDialog : Form
  {
    private EllieMae.EMLite.Campaign.Campaign campaign;
    private ContactQuery contactQuery;
    private QueryEditMode queryEditMode;
    private IContainer components;
    private Panel pnlQueryControl;
    private Panel pnlButtons;
    private Label lblSeparator;
    private Button btnCancel;
    private Button btnOK;
    private ErrorProvider errBrokenRules;

    public CampaignQueryDialog(QueryEditMode queryEditMode, EllieMae.EMLite.Campaign.Campaign campaign)
    {
      this.queryEditMode = queryEditMode;
      this.campaign = campaign;
      this.InitializeComponent();
      if (queryEditMode == QueryEditMode.AddQuery)
      {
        campaign.CampaignType |= CampaignType.AutoAddQuery;
        this.contactQuery = campaign.AddQuery;
      }
      else
      {
        campaign.CampaignType |= CampaignType.AutoDeleteQuery;
        this.contactQuery = campaign.DeleteQuery;
      }
      this.populateControls();
    }

    private void populateControls()
    {
      this.Text = this.queryEditMode == QueryEditMode.AddQuery ? "Specify Add Contact Query" : "Specify Delete Contact Query";
      WizardQueryControl wizardQueryControl = new WizardQueryControl(this.queryEditMode, true, this.campaign);
      wizardQueryControl.Dock = DockStyle.Fill;
      this.pnlQueryControl.Controls.Add((Control) wizardQueryControl);
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (!this.processUserEntry())
        return;
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private bool processUserEntry()
    {
      if (this.contactQuery.IsValid)
        return true;
      this.displayBrokenRules((BusinessBase) this.contactQuery);
      return false;
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    protected void displayBrokenRules(BusinessBase businessObject)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (BrokenRules.Rule brokenRules in (CollectionBase) businessObject.BrokenRulesCollection)
        stringBuilder.Append(brokenRules.Description + ".\n");
      int num = (int) Utils.Dialog((IWin32Window) this, stringBuilder.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
      this.pnlQueryControl = new Panel();
      this.pnlButtons = new Panel();
      this.lblSeparator = new Label();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.errBrokenRules = new ErrorProvider(this.components);
      this.pnlButtons.SuspendLayout();
      ((ISupportInitialize) this.errBrokenRules).BeginInit();
      this.SuspendLayout();
      this.pnlQueryControl.Dock = DockStyle.Fill;
      this.pnlQueryControl.Location = new Point(0, 0);
      this.pnlQueryControl.Name = "pnlQueryControl";
      this.pnlQueryControl.Size = new Size(671, 486);
      this.pnlQueryControl.TabIndex = 3;
      this.pnlButtons.Controls.Add((Control) this.lblSeparator);
      this.pnlButtons.Controls.Add((Control) this.btnCancel);
      this.pnlButtons.Controls.Add((Control) this.btnOK);
      this.pnlButtons.Dock = DockStyle.Bottom;
      this.pnlButtons.Location = new Point(0, 486);
      this.pnlButtons.Name = "pnlButtons";
      this.pnlButtons.Size = new Size(671, 32);
      this.pnlButtons.TabIndex = 2;
      this.lblSeparator.BorderStyle = BorderStyle.FixedSingle;
      this.lblSeparator.Location = new Point(7, 0);
      this.lblSeparator.Name = "lblSeparator";
      this.lblSeparator.Size = new Size(657, 1);
      this.lblSeparator.TabIndex = 2;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(589, 4);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnOK.Location = new Point(510, 4);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 0;
      this.btnOK.Text = "OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.errBrokenRules.ContainerControl = (ContainerControl) this;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(671, 518);
      this.Controls.Add((Control) this.pnlQueryControl);
      this.Controls.Add((Control) this.pnlButtons);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MaximumSize = new Size(677, 550);
      this.MinimizeBox = false;
      this.Name = nameof (CampaignQueryDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Campaign Query Maintenance";
      this.KeyUp += new KeyEventHandler(this.CampaignQueryDialog_KeyUp);
      this.pnlButtons.ResumeLayout(false);
      ((ISupportInitialize) this.errBrokenRules).EndInit();
      this.ResumeLayout(false);
    }

    private void CampaignQueryDialog_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Escape)
        return;
      this.btnCancel.PerformClick();
    }
  }
}
