// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.ClientIDInputForm
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.Encompass.AsmResolver
{
  public class ClientIDInputForm : Form
  {
    private IContainer components;
    private Button btnCancel;
    private Button btnOK;
    private Label label1;
    private TextBox textBoxClientID;

    public ClientIDInputForm() => this.InitializeComponent();

    public string ClientID => this.textBoxClientID.Text;

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (!((this.textBoxClientID.Text ?? "").Trim() == ""))
        return;
      int num = (int) MessageBox.Show("Please enter a client ID.", "Encompass SmartClient");
      this.DialogResult = DialogResult.None;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.label1 = new Label();
      this.textBoxClientID = new TextBox();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(191, 37);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(110, 37);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(13, 13);
      this.label1.Name = "label1";
      this.label1.Size = new Size(47, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Client ID";
      this.textBoxClientID.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.textBoxClientID.Location = new Point(66, 10);
      this.textBoxClientID.Name = "textBoxClientID";
      this.textBoxClientID.Size = new Size(200, 20);
      this.textBoxClientID.TabIndex = 0;
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(281, 68);
      this.Controls.Add((Control) this.textBoxClientID);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Name = nameof (ClientIDInputForm);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "SmartClient Client ID";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
