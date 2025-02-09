// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.LoanCenter.BorrowerPairConsentControlCC
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.DataEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.LoanCenter
{
  public class BorrowerPairConsentControlCC : UserControl
  {
    private string id;
    private bool _borrNameUpdated;
    private bool _borrEmailUpdated;
    private bool _coBorrNameUpdated;
    private bool _coBorrEmailUpdated;
    private IContainer components;
    private Panel pnlBorrowerPair;
    private Label lblCoBorrRejected;
    private Label lblBorrRejected;
    private Label lblCoBorrAccepted;
    private Label lblBorrAccepted;
    private Label lblCoBorrConsentSent;
    private Label lblBorrConsentSent;
    private CheckBox chkCoBorr;
    private CheckBox chkBorr;
    private ToolTip toolTip;
    private TextBox txtCoBorrName;
    private TextBox txtBorrName;
    private TextBox txtCoBorrEmail;
    private TextBox txtBorrEmail;
    private TextBox txtCoBorrAuthentication;
    private TextBox txtBorrAuthentication;

    public event EventHandler BorrowerSelectedChanged;

    public List<NonBorrowingOwner> NboContacts { get; set; }

    public BorrowerPairConsentControlCC()
    {
      this.InitializeComponent();
      this.txtBorrName.Text = string.Empty;
      this.txtBorrName.ReadOnly = true;
      this.txtBorrEmail.Text = string.Empty;
      this.txtBorrEmail.ReadOnly = true;
      this.lblBorrConsentSent.Text = string.Empty;
      this.lblBorrAccepted.Text = string.Empty;
      this.lblBorrRejected.Text = string.Empty;
      this.txtCoBorrName.Text = string.Empty;
      this.txtCoBorrName.ReadOnly = true;
      this.txtCoBorrEmail.ReadOnly = true;
      this.txtCoBorrEmail.Text = string.Empty;
      this.lblCoBorrConsentSent.Text = string.Empty;
      this.lblCoBorrAccepted.Text = string.Empty;
      this.lblCoBorrRejected.Text = string.Empty;
      this.txtBorrAuthentication.Text = string.Empty;
      this.txtBorrAuthentication.ReadOnly = true;
      this.txtCoBorrAuthentication.Text = string.Empty;
      this.txtCoBorrAuthentication.ReadOnly = true;
    }

    public string BorrowerPairId
    {
      get => this.id;
      set => this.id = value;
    }

    public string BorrowerName
    {
      get => this.txtBorrName.Text;
      set
      {
        this.toolTip.SetToolTip((Control) this.txtBorrName, value);
        this.txtBorrName.Text = value;
        if (!string.IsNullOrEmpty(value.Trim()) || !this.chkBorr.Enabled)
          return;
        this.txtBorrName.ReadOnly = false;
      }
    }

    public string CoBorrowerName
    {
      get => this.txtCoBorrName.Text;
      set
      {
        this.toolTip.SetToolTip((Control) this.txtCoBorrName, value);
        this.txtCoBorrName.Text = value;
        if (!string.IsNullOrEmpty(value.Trim()) || !this.chkCoBorr.Enabled)
          return;
        this.txtCoBorrName.ReadOnly = false;
      }
    }

    public bool BorrowerChecked
    {
      get => this.chkBorr.Checked;
      set => this.chkBorr.Checked = value;
    }

    public bool CoBorrowerChecked
    {
      get => this.chkCoBorr.Checked;
      set => this.chkCoBorr.Checked = value;
    }

    public bool BorrowerEnabled
    {
      get => this.chkBorr.Enabled;
      set
      {
        this.chkBorr.Enabled = value;
        if (this.chkBorr.Enabled)
          return;
        this.txtBorrName.ReadOnly = true;
        this.txtBorrEmail.ReadOnly = true;
        this.txtBorrAuthentication.ReadOnly = true;
      }
    }

    public bool CoBorrowerEnabled
    {
      get => this.chkCoBorr.Enabled;
      set
      {
        this.chkCoBorr.Enabled = value;
        if (this.chkCoBorr.Enabled)
          return;
        this.txtCoBorrName.ReadOnly = true;
        this.txtCoBorrEmail.ReadOnly = true;
        this.txtCoBorrAuthentication.ReadOnly = true;
      }
    }

    public bool CoBorrAuthenticationEnabled => !this.txtCoBorrAuthentication.ReadOnly;

    public bool BorrAuthenticationEnabled => !this.txtBorrAuthentication.ReadOnly;

    public string BorrowerEmail
    {
      get => this.txtBorrEmail.Text;
      set
      {
        this.toolTip.SetToolTip((Control) this.txtBorrEmail, value);
        this.txtBorrEmail.Text = value;
        if (!string.IsNullOrEmpty(value.Trim()) || !this.chkBorr.Enabled)
          return;
        this.txtBorrEmail.ReadOnly = false;
      }
    }

    public string CoBorrowerEmail
    {
      get => this.txtCoBorrEmail.Text;
      set
      {
        this.chkCoBorr.Enabled = false;
        if (!string.IsNullOrEmpty(this.chkCoBorr.Text.Trim()))
        {
          if (!string.IsNullOrEmpty(value.Trim()))
            this.chkCoBorr.Enabled = true;
          else if (!string.IsNullOrEmpty(this.BorrowerEmail.Trim()))
            this.chkCoBorr.Enabled = true;
        }
        this.toolTip.SetToolTip((Control) this.txtCoBorrEmail, value);
        this.txtCoBorrEmail.Text = value;
        if (!string.IsNullOrEmpty(value.Trim()) || !this.chkCoBorr.Enabled)
          return;
        this.txtCoBorrEmail.ReadOnly = false;
      }
    }

    public bool BorrowerNameUpdated => this._borrNameUpdated;

    public bool BorrowerEmailUpdated => this._borrEmailUpdated;

    public bool CoBorrowerNameUpdated => this._coBorrNameUpdated;

    public bool CoBorrowerEmailUpdated => this._coBorrEmailUpdated;

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

    public string BorrowerAuthenticationCode
    {
      get => this.txtBorrAuthentication.Text;
      set
      {
        this.toolTip.SetToolTip((Control) this.txtBorrAuthentication, value);
        this.txtBorrAuthentication.Text = value;
        if (string.IsNullOrEmpty(value) && this.chkBorr.Enabled)
          this.txtBorrAuthentication.ReadOnly = false;
        else
          this.txtBorrAuthentication.ReadOnly = true;
      }
    }

    public string CoBorrowerAuthenticationCode
    {
      get => this.txtCoBorrAuthentication.Text;
      set
      {
        this.toolTip.SetToolTip((Control) this.txtCoBorrAuthentication, value);
        this.txtCoBorrAuthentication.Text = value;
        if (string.IsNullOrEmpty(value) && this.chkCoBorr.Enabled)
          this.txtCoBorrAuthentication.ReadOnly = false;
        else
          this.txtCoBorrAuthentication.ReadOnly = true;
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
      this._borrNameUpdated = true;
    }

    private void txtBorrEmail_TextChanged(object sender, EventArgs e)
    {
      if (!this.txtBorrEmail.Focused)
        return;
      this._borrEmailUpdated = true;
    }

    private void txtCoBorrName_TextChanged(object sender, EventArgs e)
    {
      if (!this.txtCoBorrName.Focused)
        return;
      this._coBorrNameUpdated = true;
    }

    private void txtCoBorrEmail_TextChanged(object sender, EventArgs e)
    {
      if (!this.txtCoBorrEmail.Focused)
        return;
      this._coBorrEmailUpdated = true;
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
      this.txtCoBorrAuthentication = new TextBox();
      this.txtBorrAuthentication = new TextBox();
      this.txtCoBorrName = new TextBox();
      this.txtBorrName = new TextBox();
      this.txtCoBorrEmail = new TextBox();
      this.txtBorrEmail = new TextBox();
      this.lblCoBorrRejected = new Label();
      this.lblBorrRejected = new Label();
      this.lblCoBorrAccepted = new Label();
      this.lblBorrAccepted = new Label();
      this.lblCoBorrConsentSent = new Label();
      this.lblBorrConsentSent = new Label();
      this.chkCoBorr = new CheckBox();
      this.chkBorr = new CheckBox();
      this.toolTip = new ToolTip(this.components);
      this.pnlBorrowerPair.SuspendLayout();
      this.SuspendLayout();
      this.pnlBorrowerPair.Controls.Add((Control) this.txtCoBorrAuthentication);
      this.pnlBorrowerPair.Controls.Add((Control) this.txtBorrAuthentication);
      this.pnlBorrowerPair.Controls.Add((Control) this.txtCoBorrName);
      this.pnlBorrowerPair.Controls.Add((Control) this.txtBorrName);
      this.pnlBorrowerPair.Controls.Add((Control) this.txtCoBorrEmail);
      this.pnlBorrowerPair.Controls.Add((Control) this.txtBorrEmail);
      this.pnlBorrowerPair.Controls.Add((Control) this.lblCoBorrRejected);
      this.pnlBorrowerPair.Controls.Add((Control) this.lblBorrRejected);
      this.pnlBorrowerPair.Controls.Add((Control) this.lblCoBorrAccepted);
      this.pnlBorrowerPair.Controls.Add((Control) this.lblBorrAccepted);
      this.pnlBorrowerPair.Controls.Add((Control) this.lblCoBorrConsentSent);
      this.pnlBorrowerPair.Controls.Add((Control) this.lblBorrConsentSent);
      this.pnlBorrowerPair.Controls.Add((Control) this.chkCoBorr);
      this.pnlBorrowerPair.Controls.Add((Control) this.chkBorr);
      this.pnlBorrowerPair.Dock = DockStyle.Fill;
      this.pnlBorrowerPair.Location = new Point(0, 0);
      this.pnlBorrowerPair.Name = "pnlBorrowerPair";
      this.pnlBorrowerPair.Size = new Size(674, 48);
      this.pnlBorrowerPair.TabIndex = 1;
      this.txtCoBorrAuthentication.Location = new Point(603, 26);
      this.txtCoBorrAuthentication.Name = "txtCoBorrAuthentication";
      this.txtCoBorrAuthentication.ReadOnly = true;
      this.txtCoBorrAuthentication.Size = new Size(70, 20);
      this.txtCoBorrAuthentication.TabIndex = 23;
      this.txtBorrAuthentication.Location = new Point(603, 2);
      this.txtBorrAuthentication.Name = "txtBorrAuthentication";
      this.txtBorrAuthentication.ReadOnly = true;
      this.txtBorrAuthentication.Size = new Size(70, 20);
      this.txtBorrAuthentication.TabIndex = 22;
      this.txtCoBorrName.Location = new Point(139, 26);
      this.txtCoBorrName.Name = "txtCoBorrName";
      this.txtCoBorrName.ReadOnly = true;
      this.txtCoBorrName.Size = new Size(100, 20);
      this.txtCoBorrName.TabIndex = 21;
      this.txtCoBorrName.TextChanged += new EventHandler(this.txtCoBorrName_TextChanged);
      this.txtBorrName.Location = new Point(139, 2);
      this.txtBorrName.Name = "txtBorrName";
      this.txtBorrName.ReadOnly = true;
      this.txtBorrName.Size = new Size(100, 20);
      this.txtBorrName.TabIndex = 20;
      this.txtBorrName.TextChanged += new EventHandler(this.txtBorrName_TextChanged);
      this.txtCoBorrEmail.Location = new Point(245, 26);
      this.txtCoBorrEmail.Name = "txtCoBorrEmail";
      this.txtCoBorrEmail.ReadOnly = true;
      this.txtCoBorrEmail.Size = new Size(120, 20);
      this.txtCoBorrEmail.TabIndex = 19;
      this.txtCoBorrEmail.TextChanged += new EventHandler(this.txtCoBorrEmail_TextChanged);
      this.txtBorrEmail.Location = new Point(245, 2);
      this.txtBorrEmail.Name = "txtBorrEmail";
      this.txtBorrEmail.ReadOnly = true;
      this.txtBorrEmail.Size = new Size(120, 20);
      this.txtBorrEmail.TabIndex = 18;
      this.txtBorrEmail.TextChanged += new EventHandler(this.txtBorrEmail_TextChanged);
      this.lblCoBorrRejected.AutoSize = true;
      this.lblCoBorrRejected.Location = new Point(524, 30);
      this.lblCoBorrRejected.Name = "lblCoBorrRejected";
      this.lblCoBorrRejected.Size = new Size(75, 14);
      this.lblCoBorrRejected.TabIndex = 15;
      this.lblCoBorrRejected.Text = "MM/DD/YYYY";
      this.lblCoBorrRejected.TextAlign = ContentAlignment.MiddleLeft;
      this.lblBorrRejected.AutoSize = true;
      this.lblBorrRejected.Location = new Point(524, 6);
      this.lblBorrRejected.Name = "lblBorrRejected";
      this.lblBorrRejected.Size = new Size(75, 14);
      this.lblBorrRejected.TabIndex = 14;
      this.lblBorrRejected.Text = "MM/DD/YYYY";
      this.lblBorrRejected.TextAlign = ContentAlignment.MiddleLeft;
      this.lblCoBorrAccepted.AutoSize = true;
      this.lblCoBorrAccepted.Location = new Point(453, 30);
      this.lblCoBorrAccepted.Name = "lblCoBorrAccepted";
      this.lblCoBorrAccepted.Size = new Size(65, 14);
      this.lblCoBorrAccepted.TabIndex = 13;
      this.lblCoBorrAccepted.Text = "mm/dd/yyyy";
      this.lblCoBorrAccepted.TextAlign = ContentAlignment.MiddleLeft;
      this.lblBorrAccepted.AutoSize = true;
      this.lblBorrAccepted.Location = new Point(453, 6);
      this.lblBorrAccepted.Name = "lblBorrAccepted";
      this.lblBorrAccepted.Size = new Size(65, 14);
      this.lblBorrAccepted.TabIndex = 12;
      this.lblBorrAccepted.Text = "mm/dd/yyyy";
      this.lblBorrAccepted.TextAlign = ContentAlignment.MiddleLeft;
      this.lblCoBorrConsentSent.AutoSize = true;
      this.lblCoBorrConsentSent.Location = new Point(371, 30);
      this.lblCoBorrConsentSent.Name = "lblCoBorrConsentSent";
      this.lblCoBorrConsentSent.Size = new Size(75, 14);
      this.lblCoBorrConsentSent.TabIndex = 11;
      this.lblCoBorrConsentSent.Text = "MM/DD/YYYY";
      this.lblCoBorrConsentSent.TextAlign = ContentAlignment.MiddleLeft;
      this.lblBorrConsentSent.AutoSize = true;
      this.lblBorrConsentSent.Location = new Point(371, 6);
      this.lblBorrConsentSent.Name = "lblBorrConsentSent";
      this.lblBorrConsentSent.Size = new Size(75, 14);
      this.lblBorrConsentSent.TabIndex = 10;
      this.lblBorrConsentSent.Text = "MM/DD/YYYY";
      this.lblBorrConsentSent.TextAlign = ContentAlignment.MiddleLeft;
      this.chkCoBorr.Enabled = false;
      this.chkCoBorr.Location = new Point(3, 28);
      this.chkCoBorr.Name = "chkCoBorr";
      this.chkCoBorr.Size = new Size(90, 18);
      this.chkCoBorr.TabIndex = 7;
      this.chkCoBorr.Text = "Co-Borrower";
      this.chkCoBorr.UseVisualStyleBackColor = true;
      this.chkCoBorr.CheckedChanged += new EventHandler(this.chkBorrower_CheckedChanged);
      this.chkBorr.Enabled = false;
      this.chkBorr.Location = new Point(3, 4);
      this.chkBorr.Name = "chkBorr";
      this.chkBorr.Size = new Size(90, 18);
      this.chkBorr.TabIndex = 6;
      this.chkBorr.Text = "Borrower";
      this.chkBorr.UseVisualStyleBackColor = true;
      this.chkBorr.CheckedChanged += new EventHandler(this.chkBorrower_CheckedChanged);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoScroll = true;
      this.BackColor = SystemColors.Control;
      this.Controls.Add((Control) this.pnlBorrowerPair);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (BorrowerPairConsentControlCC);
      this.Size = new Size(674, 48);
      this.pnlBorrowerPair.ResumeLayout(false);
      this.pnlBorrowerPair.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
