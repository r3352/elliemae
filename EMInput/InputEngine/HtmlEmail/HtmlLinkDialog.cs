// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.HtmlEmail.HtmlLinkDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine.HtmlEmail
{
  public class HtmlLinkDialog : Form
  {
    private string linkUrl;
    private IContainer components;
    private Label lblLinkTo;
    private RadioButton rdoWebCenter;
    private RadioButton rdoUrl;
    private RadioButton rdoEmail;
    private TextBox txtUrl;
    private TextBox txtEmail;
    private Button btnCancel;
    private Button btnCreate;

    public HtmlLinkDialog()
    {
      this.InitializeComponent();
      this.rdoWebCenter.Checked = true;
    }

    public string LinkUrl => this.linkUrl;

    private void btnCreate_Click(object sender, EventArgs e)
    {
      string empty = string.Empty;
      string str1;
      if (this.rdoWebCenter.Checked)
        str1 = "https://webcenter/";
      else if (this.rdoUrl.Checked)
      {
        string uriString = this.txtUrl.Text.Trim();
        if (!Uri.TryCreate(uriString, UriKind.RelativeOrAbsolute, out Uri _))
        {
          int num = (int) MessageBox.Show((IWin32Window) this, "You must enter a valid url.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return;
        }
        str1 = uriString;
      }
      else if (this.rdoEmail.Checked)
      {
        string str2 = this.txtEmail.Text.Trim();
        if (str2 == string.Empty)
        {
          int num = (int) MessageBox.Show((IWin32Window) this, "You must enter a valid email address.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return;
        }
        str1 = "mailto:" + str2;
      }
      else
      {
        int num = (int) MessageBox.Show((IWin32Window) this, "You must select a link to type.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return;
      }
      this.linkUrl = str1;
      this.DialogResult = DialogResult.OK;
    }

    private void rdoButtons_CheckedChanged(object sender, EventArgs e)
    {
      this.txtEmail.Enabled = this.rdoEmail.Checked;
      this.txtUrl.Enabled = this.rdoUrl.Checked;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblLinkTo = new Label();
      this.rdoWebCenter = new RadioButton();
      this.rdoUrl = new RadioButton();
      this.rdoEmail = new RadioButton();
      this.txtUrl = new TextBox();
      this.txtEmail = new TextBox();
      this.btnCancel = new Button();
      this.btnCreate = new Button();
      this.SuspendLayout();
      this.lblLinkTo.AutoSize = true;
      this.lblLinkTo.Location = new Point(12, 12);
      this.lblLinkTo.Name = "lblLinkTo";
      this.lblLinkTo.Size = new Size(38, 14);
      this.lblLinkTo.TabIndex = 0;
      this.lblLinkTo.Text = "Link to";
      this.rdoWebCenter.AutoSize = true;
      this.rdoWebCenter.Location = new Point(68, 12);
      this.rdoWebCenter.Name = "rdoWebCenter";
      this.rdoWebCenter.Size = new Size(165, 18);
      this.rdoWebCenter.TabIndex = 1;
      this.rdoWebCenter.TabStop = true;
      this.rdoWebCenter.Text = "The WebCenter/Loan Center";
      this.rdoWebCenter.UseVisualStyleBackColor = true;
      this.rdoWebCenter.CheckedChanged += new EventHandler(this.rdoButtons_CheckedChanged);
      this.rdoUrl.AutoSize = true;
      this.rdoUrl.Location = new Point(68, 36);
      this.rdoUrl.Name = "rdoUrl";
      this.rdoUrl.Size = new Size(131, 18);
      this.rdoUrl.TabIndex = 2;
      this.rdoUrl.Text = "General Web address";
      this.rdoUrl.UseVisualStyleBackColor = true;
      this.rdoUrl.CheckedChanged += new EventHandler(this.rdoButtons_CheckedChanged);
      this.rdoEmail.AutoSize = true;
      this.rdoEmail.Location = new Point(68, 60);
      this.rdoEmail.Name = "rdoEmail";
      this.rdoEmail.Size = new Size(92, 18);
      this.rdoEmail.TabIndex = 4;
      this.rdoEmail.Text = "Email address";
      this.rdoEmail.UseVisualStyleBackColor = true;
      this.rdoEmail.CheckedChanged += new EventHandler(this.rdoButtons_CheckedChanged);
      this.txtUrl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtUrl.Location = new Point(200, 36);
      this.txtUrl.Name = "txtUrl";
      this.txtUrl.Size = new Size(288, 20);
      this.txtUrl.TabIndex = 3;
      this.txtEmail.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtEmail.Location = new Point(200, 60);
      this.txtEmail.Name = "txtEmail";
      this.txtEmail.Size = new Size(288, 20);
      this.txtEmail.TabIndex = 5;
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(414, 108);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 7;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCreate.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCreate.Location = new Point(336, 108);
      this.btnCreate.Name = "btnCreate";
      this.btnCreate.Size = new Size(75, 22);
      this.btnCreate.TabIndex = 6;
      this.btnCreate.Text = "Create";
      this.btnCreate.UseVisualStyleBackColor = true;
      this.btnCreate.Click += new EventHandler(this.btnCreate_Click);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(500, 140);
      this.Controls.Add((Control) this.btnCreate);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.txtEmail);
      this.Controls.Add((Control) this.txtUrl);
      this.Controls.Add((Control) this.rdoEmail);
      this.Controls.Add((Control) this.rdoUrl);
      this.Controls.Add((Control) this.rdoWebCenter);
      this.Controls.Add((Control) this.lblLinkTo);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (HtmlLinkDialog);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Create Hyperlink";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
