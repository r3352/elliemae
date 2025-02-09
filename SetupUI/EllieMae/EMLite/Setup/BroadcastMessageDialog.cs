// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.BroadcastMessageDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class BroadcastMessageDialog : Form
  {
    private string message = "";
    private Label label1;
    private Button sendBtn;
    private Button cancelBtn;
    private RichTextBox msgRichTextBox;
    private System.ComponentModel.Container components;

    public string Message => this.message;

    public BroadcastMessageDialog() => this.InitializeComponent();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.sendBtn = new Button();
      this.cancelBtn = new Button();
      this.msgRichTextBox = new RichTextBox();
      this.SuspendLayout();
      this.label1.Location = new Point(32, 16);
      this.label1.Name = "label1";
      this.label1.Size = new Size(216, 23);
      this.label1.TabIndex = 0;
      this.label1.Text = "Send/broadcast message to online users:";
      this.sendBtn.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.sendBtn.Location = new Point(288, 192);
      this.sendBtn.Name = "sendBtn";
      this.sendBtn.Size = new Size(75, 24);
      this.sendBtn.TabIndex = 2;
      this.sendBtn.Text = "&Send";
      this.sendBtn.Click += new EventHandler(this.sendBtn_Click);
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.cancelBtn.Location = new Point(374, 192);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 3;
      this.cancelBtn.Text = "Cancel";
      this.cancelBtn.Click += new EventHandler(this.cancelBtn_Click);
      this.msgRichTextBox.Location = new Point(32, 40);
      this.msgRichTextBox.Name = "msgRichTextBox";
      this.msgRichTextBox.Size = new Size(416, 144);
      this.msgRichTextBox.TabIndex = 4;
      this.msgRichTextBox.Text = "";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(482, 240);
      this.Controls.Add((Control) this.msgRichTextBox);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.sendBtn);
      this.Controls.Add((Control) this.label1);
      this.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (BroadcastMessageDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Broadcast Message";
      this.ResumeLayout(false);
    }

    private void sendBtn_Click(object sender, EventArgs e)
    {
      this.message = this.msgRichTextBox.Text;
      this.DialogResult = DialogResult.OK;
    }

    private void cancelBtn_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }
  }
}
