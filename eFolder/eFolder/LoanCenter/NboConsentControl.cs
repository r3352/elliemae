// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.LoanCenter.NboConsentControl
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.DataEngine;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.LoanCenter
{
  public class NboConsentControl : UserControl
  {
    private string id;
    private IContainer components;
    private Panel pnlNbo;
    private Label lblNboRejected;
    private Label lblNboAccepted;
    private Label lblNboConsentSent;
    private Label lblNboEmail;
    private CheckBox chkNboName;
    private Label lblNonBorrowingOwner;
    private ToolTip toolTip;

    public event EventHandler NboSelectedChanged;

    public NonBorrowingOwner DataTag { get; set; }

    public NboConsentControl()
    {
      this.InitializeComponent();
      this.chkNboName.Text = string.Empty;
      this.lblNboEmail.Text = string.Empty;
      this.lblNboConsentSent.Text = string.Empty;
      this.lblNboAccepted.Text = string.Empty;
      this.lblNboRejected.Text = string.Empty;
    }

    public string BorrowerPairId
    {
      get => this.id;
      set => this.id = value;
    }

    public string NboName
    {
      get => this.chkNboName.Text;
      set
      {
        this.toolTip.SetToolTip((Control) this.chkNboName, value);
        this.chkNboName.Text = value;
      }
    }

    public bool NboChecked
    {
      get => this.chkNboName.Checked;
      set => this.chkNboName.Checked = value;
    }

    public bool NboEnabled
    {
      get => this.chkNboName.Enabled;
      set => this.chkNboName.Enabled = value;
    }

    public string NboEmail
    {
      get => this.lblNboEmail.Text;
      set
      {
        this.chkNboName.Enabled = !string.IsNullOrEmpty(value);
        this.toolTip.SetToolTip((Control) this.lblNboEmail, value);
        this.lblNboEmail.Text = value;
      }
    }

    public string NboConsentSent
    {
      get => this.lblNboConsentSent.Text;
      set => this.lblNboConsentSent.Text = value;
    }

    public string NboAccepted
    {
      get => this.lblNboAccepted.Text;
      set => this.lblNboAccepted.Text = value;
    }

    public string NboRejected
    {
      get => this.lblNboRejected.Text;
      set => this.lblNboRejected.Text = value;
    }

    private void chkNbo_CheckedChanged(object sender, EventArgs e)
    {
      if (this.NboSelectedChanged == null)
        return;
      this.NboSelectedChanged((object) this, e);
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
      this.pnlNbo = new Panel();
      this.lblNboRejected = new Label();
      this.lblNboAccepted = new Label();
      this.lblNboConsentSent = new Label();
      this.lblNboEmail = new Label();
      this.chkNboName = new CheckBox();
      this.lblNonBorrowingOwner = new Label();
      this.toolTip = new ToolTip(this.components);
      this.pnlNbo.SuspendLayout();
      this.SuspendLayout();
      this.pnlNbo.Controls.Add((Control) this.lblNboRejected);
      this.pnlNbo.Controls.Add((Control) this.lblNboAccepted);
      this.pnlNbo.Controls.Add((Control) this.lblNboConsentSent);
      this.pnlNbo.Controls.Add((Control) this.lblNboEmail);
      this.pnlNbo.Controls.Add((Control) this.chkNboName);
      this.pnlNbo.Controls.Add((Control) this.lblNonBorrowingOwner);
      this.pnlNbo.Dock = DockStyle.Fill;
      this.pnlNbo.Location = new Point(0, 0);
      this.pnlNbo.Name = "pnlNbo";
      this.pnlNbo.Size = new Size(568, 24);
      this.pnlNbo.TabIndex = 1;
      this.lblNboRejected.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblNboRejected.AutoSize = true;
      this.lblNboRejected.Location = new Point(491, 4);
      this.lblNboRejected.Name = "lblNboRejected";
      this.lblNboRejected.Size = new Size(75, 14);
      this.lblNboRejected.TabIndex = 14;
      this.lblNboRejected.Text = "MM/DD/YYYY";
      this.lblNboRejected.TextAlign = ContentAlignment.MiddleLeft;
      this.lblNboAccepted.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblNboAccepted.AutoSize = true;
      this.lblNboAccepted.Location = new Point(420, 3);
      this.lblNboAccepted.Name = "lblNboAccepted";
      this.lblNboAccepted.Size = new Size(75, 14);
      this.lblNboAccepted.TabIndex = 12;
      this.lblNboAccepted.Text = "MM/DD/YYYY";
      this.lblNboAccepted.TextAlign = ContentAlignment.MiddleLeft;
      this.lblNboConsentSent.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblNboConsentSent.AutoSize = true;
      this.lblNboConsentSent.Location = new Point(349, 3);
      this.lblNboConsentSent.Name = "lblNboConsentSent";
      this.lblNboConsentSent.Size = new Size(75, 14);
      this.lblNboConsentSent.TabIndex = 10;
      this.lblNboConsentSent.Text = "MM/DD/YYYY";
      this.lblNboConsentSent.TextAlign = ContentAlignment.MiddleLeft;
      this.lblNboEmail.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblNboEmail.Location = new Point(233, 5);
      this.lblNboEmail.Name = "lblNboEmail";
      this.lblNboEmail.Size = new Size(106, 14);
      this.lblNboEmail.TabIndex = 8;
      this.lblNboEmail.Text = "NboEmail";
      this.lblNboEmail.TextAlign = ContentAlignment.MiddleLeft;
      this.chkNboName.Enabled = false;
      this.chkNboName.Location = new Point((int) sbyte.MaxValue, 3);
      this.chkNboName.Name = "chkNboName";
      this.chkNboName.Size = new Size(100, 18);
      this.chkNboName.TabIndex = 6;
      this.chkNboName.Text = "None";
      this.chkNboName.UseVisualStyleBackColor = true;
      this.chkNboName.CheckedChanged += new EventHandler(this.chkNbo_CheckedChanged);
      this.lblNonBorrowingOwner.AutoSize = true;
      this.lblNonBorrowingOwner.Location = new Point(3, 4);
      this.lblNonBorrowingOwner.Name = "lblNonBorrowingOwner";
      this.lblNonBorrowingOwner.Size = new Size(118, 14);
      this.lblNonBorrowingOwner.TabIndex = 3;
      this.lblNonBorrowingOwner.Text = "Non-Borrowing Owner";
      this.lblNonBorrowingOwner.TextAlign = ContentAlignment.MiddleLeft;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoScroll = true;
      this.BackColor = SystemColors.Control;
      this.Controls.Add((Control) this.pnlNbo);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (NboConsentControl);
      this.Size = new Size(568, 24);
      this.pnlNbo.ResumeLayout(false);
      this.pnlNbo.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
