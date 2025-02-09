// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.LockRequest.LockRequestQAOverrideServerTimeDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine.LockRequest
{
  public class LockRequestQAOverrideServerTimeDialog : Form
  {
    public DateTime overrideTime = DateTime.MinValue;
    private IContainer components;
    private Button buttonContinue;
    private Label label1;
    private TextBox textBoxOverrideTime;
    private Label label2;

    public LockRequestQAOverrideServerTimeDialog(DateTime overrideval)
    {
      this.InitializeComponent();
      DateTime utcNow = DateTime.UtcNow;
      System.TimeZoneInfo timeZoneInfo;
      try
      {
        timeZoneInfo = System.TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
      }
      catch (Exception ex)
      {
        timeZoneInfo = System.TimeZoneInfo.Local;
      }
      this.overrideTime = overrideval;
      this.textBoxOverrideTime.Text = this.overrideTime.ToString("g");
    }

    private void buttonContinue_Click(object sender, EventArgs e)
    {
      DateTime result;
      if (DateTime.TryParse(this.textBoxOverrideTime.Text, out result))
      {
        this.overrideTime = result;
        this.DialogResult = DialogResult.OK;
      }
      else
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please enter a valid date/time string.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.DialogResult = DialogResult.None;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.buttonContinue = new Button();
      this.label1 = new Label();
      this.textBoxOverrideTime = new TextBox();
      this.label2 = new Label();
      this.SuspendLayout();
      this.buttonContinue.Location = new Point(317, 79);
      this.buttonContinue.Name = "buttonContinue";
      this.buttonContinue.Size = new Size(75, 23);
      this.buttonContinue.TabIndex = 0;
      this.buttonContinue.Text = "Continue";
      this.buttonContinue.UseVisualStyleBackColor = true;
      this.buttonContinue.Click += new EventHandler(this.buttonContinue_Click);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 29);
      this.label1.Name = "label1";
      this.label1.Size = new Size(211, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Server Date/Time for Lock Hours && ONRP:";
      this.textBoxOverrideTime.Location = new Point(232, 26);
      this.textBoxOverrideTime.Name = "textBoxOverrideTime";
      this.textBoxOverrideTime.Size = new Size(126, 20);
      this.textBoxOverrideTime.TabIndex = 2;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(365, 29);
      this.label2.Name = "label2";
      this.label2.Size = new Size(28, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "EST";
      this.AcceptButton = (IButtonControl) this.buttonContinue;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(404, 114);
      this.ControlBox = false;
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.textBoxOverrideTime);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.buttonContinue);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Name = nameof (LockRequestQAOverrideServerTimeDialog);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Override Server Time";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
