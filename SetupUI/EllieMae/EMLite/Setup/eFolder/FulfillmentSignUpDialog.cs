// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.eFolder.FulfillmentSignUpDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.eFolder
{
  public class FulfillmentSignUpDialog : Form
  {
    private IContainer components;
    private GroupContainer gcTerms;
    private WebBrowser browser;
    private Panel panel1;
    private CheckBox chkConsent;
    private Button btnCancel;
    private Button btnFinish;

    public FulfillmentSignUpDialog()
    {
      this.InitializeComponent();
      this.gcTerms.Text = "ICE Mortgage Technology eDisclosure Fulfillment Service Terms & Conditions";
      this.chkConsent.Text = "I Agree to ICE Mortgage Technology eDisclosure Fulfillment Service Terms && Conditions";
      this.browser.Navigate("https://www.elliemae.com/go/encompass/fulfillment/products/docs/elliemaeagreement.pdf");
    }

    private void browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
    {
      this.chkConsent.Enabled = true;
      this.btnFinish.Enabled = true;
    }

    private void btnFinish_Click(object sender, EventArgs e)
    {
      if (!this.chkConsent.Checked)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, "Please checkmark the 'I Agree' checkbox before continuing.", "Accept Agreement", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
        this.DialogResult = DialogResult.OK;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FulfillmentSignUpDialog));
      this.gcTerms = new GroupContainer();
      this.browser = new WebBrowser();
      this.panel1 = new Panel();
      this.btnCancel = new Button();
      this.btnFinish = new Button();
      this.chkConsent = new CheckBox();
      this.gcTerms.SuspendLayout();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.gcTerms.Controls.Add((Control) this.browser);
      this.gcTerms.Dock = DockStyle.Fill;
      this.gcTerms.HeaderForeColor = SystemColors.ControlText;
      this.gcTerms.Location = new Point(0, 0);
      this.gcTerms.Name = "gcTerms";
      this.gcTerms.Size = new Size(755, 428);
      this.gcTerms.TabIndex = 0;
      this.browser.Dock = DockStyle.Fill;
      this.browser.Location = new Point(1, 26);
      this.browser.MinimumSize = new Size(20, 22);
      this.browser.Name = "browser";
      this.browser.Size = new Size(753, 401);
      this.browser.TabIndex = 0;
      this.browser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(this.browser_DocumentCompleted);
      this.panel1.Controls.Add((Control) this.btnCancel);
      this.panel1.Controls.Add((Control) this.btnFinish);
      this.panel1.Controls.Add((Control) this.chkConsent);
      this.panel1.Dock = DockStyle.Bottom;
      this.panel1.Location = new Point(0, 428);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(755, 39);
      this.panel1.TabIndex = 1;
      this.btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(671, 8);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnFinish.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnFinish.Enabled = false;
      this.btnFinish.Location = new Point(591, 8);
      this.btnFinish.Name = "btnFinish";
      this.btnFinish.Size = new Size(75, 22);
      this.btnFinish.TabIndex = 1;
      this.btnFinish.Text = "&Finish";
      this.btnFinish.UseVisualStyleBackColor = true;
      this.btnFinish.Click += new EventHandler(this.btnFinish_Click);
      this.chkConsent.AutoSize = true;
      this.chkConsent.Enabled = false;
      this.chkConsent.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkConsent.Location = new Point(8, 12);
      this.chkConsent.Name = "chkConsent";
      this.chkConsent.Size = new Size(78, 18);
      this.chkConsent.TabIndex = 0;
      this.chkConsent.Text = "I Agree....";
      this.chkConsent.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(755, 467);
      this.ControlBox = false;
      this.Controls.Add((Control) this.gcTerms);
      this.Controls.Add((Control) this.panel1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (FulfillmentSignUpDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Fulfillment Service";
      this.gcTerms.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
