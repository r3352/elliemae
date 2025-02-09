// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.eDelivery.RecipientControl
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.eDelivery
{
  public class RecipientControl : UserControl
  {
    private IContainer components;
    private Panel pnlRecipient;
    public TextBox txtName;
    public TextBox txtEmail;
    public CheckBox cbSelect;
    public TextBox txtAuthenticationCode;

    public RecipientControl() => this.InitializeComponent();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pnlRecipient = new Panel();
      this.txtName = new TextBox();
      this.txtEmail = new TextBox();
      this.cbSelect = new CheckBox();
      this.txtAuthenticationCode = new TextBox();
      this.pnlRecipient.SuspendLayout();
      this.SuspendLayout();
      this.pnlRecipient.Controls.Add((Control) this.txtName);
      this.pnlRecipient.Controls.Add((Control) this.txtEmail);
      this.pnlRecipient.Controls.Add((Control) this.cbSelect);
      this.pnlRecipient.Controls.Add((Control) this.txtAuthenticationCode);
      this.pnlRecipient.Dock = DockStyle.Fill;
      this.pnlRecipient.Location = new Point(0, 0);
      this.pnlRecipient.Name = "pnlRecipient";
      this.pnlRecipient.Size = new Size(540, 30);
      this.pnlRecipient.TabIndex = 1;
      this.txtName.Location = new Point(149, 5);
      this.txtName.Margin = new Padding(3, 2, 3, 2);
      this.txtName.Name = "txtName";
      this.txtName.ReadOnly = true;
      this.txtName.Size = new Size(129, 20);
      this.txtName.TabIndex = 3;
      this.txtEmail.Location = new Point(284, 5);
      this.txtEmail.Margin = new Padding(3, 2, 3, 2);
      this.txtEmail.Name = "txtEmail";
      this.txtEmail.ReadOnly = true;
      this.txtEmail.Size = new Size(161, 20);
      this.txtEmail.TabIndex = 4;
      this.cbSelect.AutoSize = true;
      this.cbSelect.Checked = true;
      this.cbSelect.CheckState = CheckState.Checked;
      this.cbSelect.Location = new Point(3, 5);
      this.cbSelect.Margin = new Padding(3, 2, 3, 2);
      this.cbSelect.Name = "cbSelect";
      this.cbSelect.Size = new Size(93, 18);
      this.cbSelect.TabIndex = 2;
      this.cbSelect.Text = "RecipientType";
      this.cbSelect.UseVisualStyleBackColor = true;
      this.txtAuthenticationCode.Location = new Point(451, 5);
      this.txtAuthenticationCode.Margin = new Padding(3, 2, 3, 2);
      this.txtAuthenticationCode.Name = "txtAuthenticationCode";
      this.txtAuthenticationCode.ReadOnly = true;
      this.txtAuthenticationCode.Size = new Size(81, 20);
      this.txtAuthenticationCode.TabIndex = 5;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoScroll = true;
      this.BackColor = Color.WhiteSmoke;
      this.Controls.Add((Control) this.pnlRecipient);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (RecipientControl);
      this.Size = new Size(540, 30);
      this.pnlRecipient.ResumeLayout(false);
      this.pnlRecipient.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
