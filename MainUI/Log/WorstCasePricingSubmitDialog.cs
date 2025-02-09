// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.WorstCasePricingSubmitDialog
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class WorstCasePricingSubmitDialog : Form
  {
    private IContainer components;
    private Label lblmsg;
    private Button btnSubmit;
    private Button btnCancel;

    public WorstCasePricingSubmitDialog(string message)
    {
      this.InitializeComponent();
      this.lblmsg.Text = message;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblmsg = new Label();
      this.btnSubmit = new Button();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.lblmsg.AutoSize = true;
      this.lblmsg.Location = new Point(33, 29);
      this.lblmsg.Name = "lblmsg";
      this.lblmsg.Size = new Size(50, 13);
      this.lblmsg.TabIndex = 0;
      this.lblmsg.Text = "Message";
      this.btnSubmit.Anchor = AnchorStyles.None;
      this.btnSubmit.DialogResult = DialogResult.OK;
      this.btnSubmit.Location = new Point(207, 64);
      this.btnSubmit.Name = "btnSubmit";
      this.btnSubmit.Size = new Size(75, 23);
      this.btnSubmit.TabIndex = 3;
      this.btnSubmit.Text = "&Submit";
      this.btnCancel.Anchor = AnchorStyles.None;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(302, 64);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "&Cancel";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(406, 114);
      this.Controls.Add((Control) this.btnSubmit);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.lblmsg);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (WorstCasePricingSubmitDialog);
      this.ShowIcon = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Lock Submittal Confirmation";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
