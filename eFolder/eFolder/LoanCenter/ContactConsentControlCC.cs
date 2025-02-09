// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.LoanCenter.ContactConsentControlCC
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.LoanCenter
{
  public class ContactConsentControlCC : UserControl
  {
    private string id;
    private IContainer components;
    private Panel pnlBorrowerPair;
    private Label lblBorrRejected;
    private Label lblBorrAccepted;
    private Label lblBorrConsentSent;
    private CheckBox chkContactType;
    private ToolTip toolTip;
    private TextBox txtBorrName;
    private TextBox txtBorrEmail;
    private TextBox txtBorrAuthentication;

    public object DataTag { get; set; }

    public event EventHandler BorrowerSelectedChanged;

    public ContactConsentControlCC()
    {
      this.InitializeComponent();
      this.txtBorrName.Text = string.Empty;
      this.txtBorrName.ReadOnly = true;
      this.txtBorrEmail.Text = string.Empty;
      this.txtBorrEmail.ReadOnly = true;
      this.lblBorrConsentSent.Text = string.Empty;
      this.lblBorrAccepted.Text = string.Empty;
      this.lblBorrRejected.Text = string.Empty;
      this.txtBorrAuthentication.Text = string.Empty;
      this.txtBorrAuthentication.ReadOnly = true;
    }

    public string BorrowerPairId
    {
      get => this.id;
      set => this.id = value;
    }

    public string ContactType
    {
      get => this.chkContactType.Text;
      set => this.chkContactType.Text = value;
    }

    public string ContactName
    {
      get => this.txtBorrName.Text;
      set
      {
        this.toolTip.SetToolTip((Control) this.txtBorrName, value);
        this.txtBorrName.Text = value;
        if (!string.IsNullOrEmpty(value.Trim()) || !this.chkContactType.Enabled)
          return;
        this.txtBorrName.ReadOnly = false;
      }
    }

    public bool ContactChecked
    {
      get => this.chkContactType.Checked;
      set => this.chkContactType.Checked = value;
    }

    public bool ContactEnabled
    {
      get => this.chkContactType.Enabled;
      set
      {
        this.chkContactType.Enabled = value;
        if (this.chkContactType.Enabled)
          return;
        this.txtBorrName.ReadOnly = true;
        this.txtBorrEmail.ReadOnly = true;
        this.txtBorrAuthentication.ReadOnly = true;
      }
    }

    public bool ContactAuthenticationEnabled => !this.txtBorrAuthentication.ReadOnly;

    public string ContactEmail
    {
      get => this.txtBorrEmail.Text;
      set
      {
        this.toolTip.SetToolTip((Control) this.txtBorrEmail, value);
        this.txtBorrEmail.Text = value;
        if (!string.IsNullOrEmpty(value.Trim()) || !this.chkContactType.Enabled)
          return;
        this.txtBorrEmail.ReadOnly = false;
      }
    }

    public bool ContactNameUpdated { get; private set; }

    public bool ContactEmailUpdated { get; private set; }

    public string ContactConsentSent
    {
      get => this.lblBorrConsentSent.Text;
      set => this.lblBorrConsentSent.Text = value;
    }

    public string ContactAccepted
    {
      get => this.lblBorrAccepted.Text;
      set => this.lblBorrAccepted.Text = value;
    }

    public string ContactRejected
    {
      get => this.lblBorrRejected.Text;
      set => this.lblBorrRejected.Text = value;
    }

    public string ContactAuthenticationCode
    {
      get => this.txtBorrAuthentication.Text;
      set
      {
        this.toolTip.SetToolTip((Control) this.txtBorrAuthentication, value);
        this.txtBorrAuthentication.Text = value;
        if (string.IsNullOrEmpty(value) && this.chkContactType.Enabled)
          this.txtBorrAuthentication.ReadOnly = false;
        else
          this.txtBorrAuthentication.ReadOnly = true;
      }
    }

    private void chkBorrower_CheckedChanged(object sender, EventArgs e)
    {
      if (this.BorrowerSelectedChanged == null)
        return;
      this.BorrowerSelectedChanged((object) this, e);
    }

    private void txtBorrName_TextChanged(object sender, EventArgs e)
    {
      if (!this.txtBorrName.Focused)
        return;
      this.ContactNameUpdated = true;
    }

    private void txtBorrEmail_TextChanged(object sender, EventArgs e)
    {
      if (!this.txtBorrEmail.Focused)
        return;
      this.ContactEmailUpdated = true;
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
      this.pnlBorrowerPair = new Panel();
      this.txtBorrAuthentication = new TextBox();
      this.txtBorrName = new TextBox();
      this.txtBorrEmail = new TextBox();
      this.lblBorrRejected = new Label();
      this.lblBorrAccepted = new Label();
      this.lblBorrConsentSent = new Label();
      this.chkContactType = new CheckBox();
      this.toolTip = new ToolTip(this.components);
      this.pnlBorrowerPair.SuspendLayout();
      this.SuspendLayout();
      this.pnlBorrowerPair.Controls.Add((Control) this.txtBorrAuthentication);
      this.pnlBorrowerPair.Controls.Add((Control) this.txtBorrName);
      this.pnlBorrowerPair.Controls.Add((Control) this.txtBorrEmail);
      this.pnlBorrowerPair.Controls.Add((Control) this.lblBorrRejected);
      this.pnlBorrowerPair.Controls.Add((Control) this.lblBorrAccepted);
      this.pnlBorrowerPair.Controls.Add((Control) this.lblBorrConsentSent);
      this.pnlBorrowerPair.Controls.Add((Control) this.chkContactType);
      this.pnlBorrowerPair.Dock = DockStyle.Fill;
      this.pnlBorrowerPair.Location = new Point(0, 0);
      this.pnlBorrowerPair.Margin = new Padding(0);
      this.pnlBorrowerPair.Name = "pnlBorrowerPair";
      this.pnlBorrowerPair.Size = new Size(674, 24);
      this.pnlBorrowerPair.TabIndex = 1;
      this.txtBorrAuthentication.Location = new Point(603, 2);
      this.txtBorrAuthentication.Name = "txtBorrAuthentication";
      this.txtBorrAuthentication.ReadOnly = true;
      this.txtBorrAuthentication.Size = new Size(70, 20);
      this.txtBorrAuthentication.TabIndex = 22;
      this.txtBorrName.Location = new Point(139, 2);
      this.txtBorrName.Name = "txtBorrName";
      this.txtBorrName.ReadOnly = true;
      this.txtBorrName.Size = new Size(100, 20);
      this.txtBorrName.TabIndex = 20;
      this.txtBorrName.TextChanged += new EventHandler(this.txtBorrName_TextChanged);
      this.txtBorrEmail.Location = new Point(245, 1);
      this.txtBorrEmail.Name = "txtBorrEmail";
      this.txtBorrEmail.ReadOnly = true;
      this.txtBorrEmail.Size = new Size(120, 20);
      this.txtBorrEmail.TabIndex = 18;
      this.txtBorrEmail.TextChanged += new EventHandler(this.txtBorrEmail_TextChanged);
      this.lblBorrRejected.AutoSize = true;
      this.lblBorrRejected.Location = new Point(524, 5);
      this.lblBorrRejected.Name = "lblBorrRejected";
      this.lblBorrRejected.Size = new Size(75, 14);
      this.lblBorrRejected.TabIndex = 14;
      this.lblBorrRejected.Text = "MM/DD/YYYY";
      this.lblBorrRejected.TextAlign = ContentAlignment.MiddleLeft;
      this.lblBorrAccepted.AutoSize = true;
      this.lblBorrAccepted.Location = new Point(453, 5);
      this.lblBorrAccepted.Name = "lblBorrAccepted";
      this.lblBorrAccepted.Size = new Size(65, 14);
      this.lblBorrAccepted.TabIndex = 12;
      this.lblBorrAccepted.Text = "mm/dd/yyyy";
      this.lblBorrAccepted.TextAlign = ContentAlignment.MiddleLeft;
      this.lblBorrConsentSent.AutoSize = true;
      this.lblBorrConsentSent.Location = new Point(371, 5);
      this.lblBorrConsentSent.Name = "lblBorrConsentSent";
      this.lblBorrConsentSent.Size = new Size(75, 14);
      this.lblBorrConsentSent.TabIndex = 10;
      this.lblBorrConsentSent.Text = "MM/DD/YYYY";
      this.lblBorrConsentSent.TextAlign = ContentAlignment.MiddleLeft;
      this.chkContactType.AutoSize = true;
      this.chkContactType.Enabled = false;
      this.chkContactType.Location = new Point(3, 4);
      this.chkContactType.Name = "chkContactType";
      this.chkContactType.Size = new Size(137, 18);
      this.chkContactType.TabIndex = 6;
      this.chkContactType.Text = "Non-Borrowing Owner";
      this.chkContactType.UseVisualStyleBackColor = true;
      this.chkContactType.CheckedChanged += new EventHandler(this.chkBorrower_CheckedChanged);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoScroll = true;
      this.BackColor = SystemColors.Control;
      this.Controls.Add((Control) this.pnlBorrowerPair);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (ContactConsentControlCC);
      this.Size = new Size(674, 24);
      this.pnlBorrowerPair.ResumeLayout(false);
      this.pnlBorrowerPair.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
