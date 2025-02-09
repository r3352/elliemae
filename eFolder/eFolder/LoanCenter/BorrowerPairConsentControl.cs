// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.LoanCenter.BorrowerPairConsentControl
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
  public class BorrowerPairConsentControl : UserControl
  {
    private string id;
    private IContainer components;
    private Panel pnlBorrowerPair;
    private Label lblCoBorrRejected;
    private Label lblBorrRejected;
    private Label lblCoBorrAccepted;
    private Label lblBorrAccepted;
    private Label lblCoBorrConsentSent;
    private Label lblBorrConsentSent;
    private Label lblCoBorrEmail;
    private Label lblBorrEmail;
    private CheckBox chkCoBorrName;
    private CheckBox chkBorrName;
    private Label lblCoborrower;
    private Label lblBorrower;
    private ToolTip toolTip;

    public event EventHandler BorrowerSelectedChanged;

    public BorrowerPairConsentControl()
    {
      this.InitializeComponent();
      this.chkBorrName.Text = string.Empty;
      this.lblBorrEmail.Text = string.Empty;
      this.lblBorrConsentSent.Text = string.Empty;
      this.lblBorrAccepted.Text = string.Empty;
      this.lblBorrRejected.Text = string.Empty;
      this.chkCoBorrName.Text = string.Empty;
      this.lblCoBorrEmail.Text = string.Empty;
      this.lblCoBorrConsentSent.Text = string.Empty;
      this.lblCoBorrAccepted.Text = string.Empty;
      this.lblCoBorrRejected.Text = string.Empty;
    }

    public string BorrowerPairId
    {
      get => this.id;
      set => this.id = value;
    }

    public string BorrowerName
    {
      get => this.chkBorrName.Text;
      set
      {
        this.toolTip.SetToolTip((Control) this.chkBorrName, value);
        this.chkBorrName.Text = value;
      }
    }

    public string CoBorrowerName
    {
      get => this.chkCoBorrName.Text;
      set
      {
        this.toolTip.SetToolTip((Control) this.chkCoBorrName, value);
        this.chkCoBorrName.Text = value;
      }
    }

    public bool BorrowerChecked
    {
      get => this.chkBorrName.Checked;
      set => this.chkBorrName.Checked = value;
    }

    public bool CoBorrowerChecked
    {
      get => this.chkCoBorrName.Checked;
      set => this.chkCoBorrName.Checked = value;
    }

    public bool BorrowerEnabled
    {
      get => this.chkBorrName.Enabled;
      set => this.chkBorrName.Enabled = value;
    }

    public bool CoBorrowerEnabled
    {
      get => this.chkCoBorrName.Enabled;
      set => this.chkCoBorrName.Enabled = value;
    }

    public string BorrowerEmail
    {
      get => this.lblBorrEmail.Text;
      set
      {
        this.chkBorrName.Enabled = !string.IsNullOrEmpty(value);
        this.toolTip.SetToolTip((Control) this.lblBorrEmail, value);
        this.lblBorrEmail.Text = value;
      }
    }

    public string CoBorrowerEmail
    {
      get => this.lblCoBorrEmail.Text;
      set
      {
        this.chkCoBorrName.Enabled = false;
        if (!string.IsNullOrEmpty(this.chkCoBorrName.Text.Trim()))
        {
          if (!string.IsNullOrEmpty(value))
            this.chkCoBorrName.Enabled = true;
          else if (!string.IsNullOrEmpty(this.BorrowerEmail))
            this.chkCoBorrName.Enabled = true;
        }
        this.toolTip.SetToolTip((Control) this.lblCoBorrEmail, value);
        this.lblCoBorrEmail.Text = value;
      }
    }

    public string BorrowerConsentSent
    {
      get => this.lblBorrConsentSent.Text;
      set => this.lblBorrConsentSent.Text = value;
    }

    public string CoBorrowerConsentSent
    {
      get => this.lblCoBorrConsentSent.Text;
      set => this.lblCoBorrConsentSent.Text = value;
    }

    public string BorrowerAccepted
    {
      get => this.lblBorrAccepted.Text;
      set => this.lblBorrAccepted.Text = value;
    }

    public string CoBorrowerAccepted
    {
      get => this.lblCoBorrAccepted.Text;
      set => this.lblCoBorrAccepted.Text = value;
    }

    public string BorrowerRejected
    {
      get => this.lblBorrRejected.Text;
      set => this.lblBorrRejected.Text = value;
    }

    public string CoBorrowerRejected
    {
      get => this.lblCoBorrRejected.Text;
      set => this.lblCoBorrRejected.Text = value;
    }

    private void chkBorrower_CheckedChanged(object sender, EventArgs e)
    {
      if (this.BorrowerSelectedChanged == null)
        return;
      this.BorrowerSelectedChanged((object) this, e);
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
      this.lblCoBorrRejected = new Label();
      this.lblBorrRejected = new Label();
      this.lblCoBorrAccepted = new Label();
      this.lblBorrAccepted = new Label();
      this.lblCoBorrConsentSent = new Label();
      this.lblBorrConsentSent = new Label();
      this.lblCoBorrEmail = new Label();
      this.lblBorrEmail = new Label();
      this.chkCoBorrName = new CheckBox();
      this.chkBorrName = new CheckBox();
      this.lblCoborrower = new Label();
      this.lblBorrower = new Label();
      this.toolTip = new ToolTip(this.components);
      this.pnlBorrowerPair.SuspendLayout();
      this.SuspendLayout();
      this.pnlBorrowerPair.Controls.Add((Control) this.lblCoBorrRejected);
      this.pnlBorrowerPair.Controls.Add((Control) this.lblBorrRejected);
      this.pnlBorrowerPair.Controls.Add((Control) this.lblCoBorrAccepted);
      this.pnlBorrowerPair.Controls.Add((Control) this.lblBorrAccepted);
      this.pnlBorrowerPair.Controls.Add((Control) this.lblCoBorrConsentSent);
      this.pnlBorrowerPair.Controls.Add((Control) this.lblBorrConsentSent);
      this.pnlBorrowerPair.Controls.Add((Control) this.lblCoBorrEmail);
      this.pnlBorrowerPair.Controls.Add((Control) this.lblBorrEmail);
      this.pnlBorrowerPair.Controls.Add((Control) this.chkCoBorrName);
      this.pnlBorrowerPair.Controls.Add((Control) this.chkBorrName);
      this.pnlBorrowerPair.Controls.Add((Control) this.lblCoborrower);
      this.pnlBorrowerPair.Controls.Add((Control) this.lblBorrower);
      this.pnlBorrowerPair.Dock = DockStyle.Fill;
      this.pnlBorrowerPair.Location = new Point(0, 0);
      this.pnlBorrowerPair.Name = "pnlBorrowerPair";
      this.pnlBorrowerPair.Size = new Size(568, 48);
      this.pnlBorrowerPair.TabIndex = 1;
      this.lblCoBorrRejected.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblCoBorrRejected.AutoSize = true;
      this.lblCoBorrRejected.Location = new Point(491, 29);
      this.lblCoBorrRejected.Name = "lblCoBorrRejected";
      this.lblCoBorrRejected.Size = new Size(75, 14);
      this.lblCoBorrRejected.TabIndex = 15;
      this.lblCoBorrRejected.Text = "MM/DD/YYYY";
      this.lblCoBorrRejected.TextAlign = ContentAlignment.MiddleLeft;
      this.lblBorrRejected.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblBorrRejected.AutoSize = true;
      this.lblBorrRejected.Location = new Point(491, 5);
      this.lblBorrRejected.Name = "lblBorrRejected";
      this.lblBorrRejected.Size = new Size(75, 14);
      this.lblBorrRejected.TabIndex = 14;
      this.lblBorrRejected.Text = "MM/DD/YYYY";
      this.lblBorrRejected.TextAlign = ContentAlignment.MiddleLeft;
      this.lblCoBorrAccepted.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblCoBorrAccepted.AutoSize = true;
      this.lblCoBorrAccepted.Location = new Point(420, 28);
      this.lblCoBorrAccepted.Name = "lblCoBorrAccepted";
      this.lblCoBorrAccepted.Size = new Size(75, 14);
      this.lblCoBorrAccepted.TabIndex = 13;
      this.lblCoBorrAccepted.Text = "MM/DD/YYYY";
      this.lblCoBorrAccepted.TextAlign = ContentAlignment.MiddleLeft;
      this.lblBorrAccepted.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblBorrAccepted.AutoSize = true;
      this.lblBorrAccepted.Location = new Point(420, 4);
      this.lblBorrAccepted.Name = "lblBorrAccepted";
      this.lblBorrAccepted.Size = new Size(75, 14);
      this.lblBorrAccepted.TabIndex = 12;
      this.lblBorrAccepted.Text = "MM/DD/YYYY";
      this.lblBorrAccepted.TextAlign = ContentAlignment.MiddleLeft;
      this.lblCoBorrConsentSent.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblCoBorrConsentSent.AutoSize = true;
      this.lblCoBorrConsentSent.Location = new Point(349, 28);
      this.lblCoBorrConsentSent.Name = "lblCoBorrConsentSent";
      this.lblCoBorrConsentSent.Size = new Size(75, 14);
      this.lblCoBorrConsentSent.TabIndex = 11;
      this.lblCoBorrConsentSent.Text = "MM/DD/YYYY";
      this.lblCoBorrConsentSent.TextAlign = ContentAlignment.MiddleLeft;
      this.lblBorrConsentSent.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblBorrConsentSent.AutoSize = true;
      this.lblBorrConsentSent.Location = new Point(349, 4);
      this.lblBorrConsentSent.Name = "lblBorrConsentSent";
      this.lblBorrConsentSent.Size = new Size(75, 14);
      this.lblBorrConsentSent.TabIndex = 10;
      this.lblBorrConsentSent.Text = "MM/DD/YYYY";
      this.lblBorrConsentSent.TextAlign = ContentAlignment.MiddleLeft;
      this.lblCoBorrEmail.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblCoBorrEmail.Location = new Point(233, 29);
      this.lblCoBorrEmail.Name = "lblCoBorrEmail";
      this.lblCoBorrEmail.Size = new Size(107, 14);
      this.lblCoBorrEmail.TabIndex = 9;
      this.lblCoBorrEmail.Text = "Co-BorrowerEmail";
      this.lblCoBorrEmail.TextAlign = ContentAlignment.MiddleLeft;
      this.lblBorrEmail.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblBorrEmail.Location = new Point(233, 5);
      this.lblBorrEmail.Name = "lblBorrEmail";
      this.lblBorrEmail.Size = new Size(107, 14);
      this.lblBorrEmail.TabIndex = 8;
      this.lblBorrEmail.Text = "BorrowerEmail";
      this.lblBorrEmail.TextAlign = ContentAlignment.MiddleLeft;
      this.chkCoBorrName.Enabled = false;
      this.chkCoBorrName.Location = new Point(128, 28);
      this.chkCoBorrName.Name = "chkCoBorrName";
      this.chkCoBorrName.Size = new Size(103, 18);
      this.chkCoBorrName.TabIndex = 7;
      this.chkCoBorrName.Text = "None";
      this.chkCoBorrName.UseVisualStyleBackColor = true;
      this.chkCoBorrName.CheckedChanged += new EventHandler(this.chkBorrower_CheckedChanged);
      this.chkBorrName.Enabled = false;
      this.chkBorrName.Location = new Point(128, 4);
      this.chkBorrName.Name = "chkBorrName";
      this.chkBorrName.Size = new Size(103, 18);
      this.chkBorrName.TabIndex = 6;
      this.chkBorrName.Text = "None";
      this.chkBorrName.UseVisualStyleBackColor = true;
      this.chkBorrName.CheckedChanged += new EventHandler(this.chkBorrower_CheckedChanged);
      this.lblCoborrower.AutoSize = true;
      this.lblCoborrower.Location = new Point(3, 28);
      this.lblCoborrower.Name = "lblCoborrower";
      this.lblCoborrower.Size = new Size(71, 14);
      this.lblCoborrower.TabIndex = 5;
      this.lblCoborrower.Text = "Co-Borrower";
      this.lblCoborrower.TextAlign = ContentAlignment.MiddleLeft;
      this.lblBorrower.AutoSize = true;
      this.lblBorrower.Location = new Point(3, 4);
      this.lblBorrower.Name = "lblBorrower";
      this.lblBorrower.Size = new Size(54, 14);
      this.lblBorrower.TabIndex = 3;
      this.lblBorrower.Text = "Borrower";
      this.lblBorrower.TextAlign = ContentAlignment.MiddleLeft;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoScroll = true;
      this.BackColor = SystemColors.Control;
      this.Controls.Add((Control) this.pnlBorrowerPair);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (BorrowerPairConsentControl);
      this.Size = new Size(568, 48);
      this.pnlBorrowerPair.ResumeLayout(false);
      this.pnlBorrowerPair.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
