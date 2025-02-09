// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AddFeeForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AddFeeForm : Form
  {
    private IContainer components;
    private Label label1;
    private TextBox txtFeeName;
    private DialogButtons dialogButtons1;

    public event EventHandler OkButtonClick;

    public AddFeeForm()
    {
      this.InitializeComponent();
      this.txtFeeName_TextChanged((object) null, (EventArgs) null);
    }

    public string NewFeeName => this.txtFeeName.Text.Trim();

    private void dialogButtons1_OK(object sender, EventArgs e)
    {
      try
      {
        if (this.OkButtonClick != null)
          this.OkButtonClick((object) this, e);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return;
      }
      this.DialogResult = DialogResult.OK;
    }

    private void txtFeeName_TextChanged(object sender, EventArgs e)
    {
      this.dialogButtons1.OKButton.Enabled = this.txtFeeName.Text.Trim().Length > 0;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AddFeeForm));
      this.label1 = new Label();
      this.txtFeeName = new TextBox();
      this.dialogButtons1 = new DialogButtons();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 20);
      this.label1.Name = "label1";
      this.label1.Size = new Size(114, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Encompass Fee Name";
      this.txtFeeName.Location = new Point(132, 17);
      this.txtFeeName.MaxLength = 100;
      this.txtFeeName.Name = "txtFeeName";
      this.txtFeeName.Size = new Size(348, 20);
      this.txtFeeName.TabIndex = 0;
      this.txtFeeName.TextChanged += new EventHandler(this.txtFeeName_TextChanged);
      this.dialogButtons1.Dock = DockStyle.Bottom;
      this.dialogButtons1.Location = new Point(0, 43);
      this.dialogButtons1.Name = "dialogButtons1";
      this.dialogButtons1.Size = new Size(492, 35);
      this.dialogButtons1.TabIndex = 2;
      this.dialogButtons1.OK += new EventHandler(this.dialogButtons1_OK);
      this.AcceptButton = (IButtonControl) this.dialogButtons1;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(492, 78);
      this.Controls.Add((Control) this.dialogButtons1);
      this.Controls.Add((Control) this.txtFeeName);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddFeeForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Add New Fee";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
