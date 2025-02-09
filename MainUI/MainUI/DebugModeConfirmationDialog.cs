// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.DebugModeConfirmationDialog
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Diagnostics;
using EllieMae.EMLite.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public class DebugModeConfirmationDialog : Form
  {
    private IContainer components;
    private Label label1;
    private Button btnyes;
    private Button btnno;
    private Label label2;
    private CheckBox checkBox1;
    private PictureBox pictureBox1;

    public DebugModeConfirmationDialog() => this.InitializeComponent();

    private void button_Click(object sender, EventArgs e)
    {
      string name = ((Control) sender).Name;
      try
      {
        if (name == "btnyes")
        {
          EnConfigurationSettings.GlobalSettings.Debug = false;
          DiagnosticSession.DiagnosticsMode = DiagnosticsMode.Disabled;
          this.DialogResult = DialogResult.Yes;
        }
        if (this.checkBox1.Checked)
          EnConfigurationSettings.GlobalSettings.DebugDoNotAskIndicator = true;
      }
      catch
      {
      }
      this.Close();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.btnyes = new Button();
      this.btnno = new Button();
      this.label2 = new Label();
      this.checkBox1 = new CheckBox();
      this.pictureBox1 = new PictureBox();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(72, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(315, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "This instance of Encompass is currently running in debug mode.";
      this.btnyes.Location = new Point(86, 76);
      this.btnyes.Name = "btnyes";
      this.btnyes.Size = new Size(141, 29);
      this.btnyes.TabIndex = 1;
      this.btnyes.Text = "Turn Off Debug Mode";
      this.btnyes.UseVisualStyleBackColor = true;
      this.btnyes.Click += new EventHandler(this.button_Click);
      this.btnno.Location = new Point(241, 76);
      this.btnno.Name = "btnno";
      this.btnno.Size = new Size(141, 29);
      this.btnno.TabIndex = 2;
      this.btnno.Text = "Ignore";
      this.btnno.UseVisualStyleBackColor = true;
      this.btnno.Click += new EventHandler(this.button_Click);
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(70, 24);
      this.label2.Name = "label2";
      this.label2.Size = new Size(393, 14);
      this.label2.TabIndex = 4;
      this.label2.Text = " Performance decreases significantly when you run Encompass in debug mode.";
      this.checkBox1.AutoSize = true;
      this.checkBox1.Location = new Point(76, 47);
      this.checkBox1.Name = "checkBox1";
      this.checkBox1.Size = new Size(133, 18);
      this.checkBox1.TabIndex = 5;
      this.checkBox1.Text = "Don't ask me next time";
      this.checkBox1.UseVisualStyleBackColor = true;
      this.pictureBox1.Image = (Image) Resources.Warning;
      this.pictureBox1.Location = new Point(12, 9);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(46, 41);
      this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
      this.pictureBox1.TabIndex = 6;
      this.pictureBox1.TabStop = false;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = SystemColors.ButtonHighlight;
      this.ClientSize = new Size(478, 119);
      this.Controls.Add((Control) this.pictureBox1);
      this.Controls.Add((Control) this.checkBox1);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.btnno);
      this.Controls.Add((Control) this.btnyes);
      this.Controls.Add((Control) this.label1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Margin = new Padding(2, 3, 2, 3);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (DebugModeConfirmationDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Warning!";
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
