// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.Controls.LoanSaveProgressDialog
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.Common.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI.Controls
{
  public class LoanSaveProgressDialog : Form
  {
    private readonly ProgressImage _progressImage;
    private readonly Color _defaultColor = Color.FromArgb((int) byte.MaxValue, 0, 106, 169);
    private IContainer components;
    private Panel panel1;
    private Panel panel2;
    private RegionProgressBar progressBar1;
    private Label lblStatus;
    private Panel pnlIcon;
    private Label lblSubStatus;

    public LoanSaveProgressDialog()
    {
      this.InitializeComponent();
      this._progressImage = new ProgressImage();
      this._progressImage.ImageIcon = Resources.appico;
      this._progressImage.Dock = DockStyle.Fill;
      this.Padding = new Padding(1, 1, 2, 2);
      this.pnlIcon.Controls.Add((Control) this._progressImage);
    }

    public void UpdateProgress(string activity, string detail, int progress = -1, Color? color = null)
    {
      this.lblStatus.Invoke((Delegate) (() => this.lblStatus.Text = activity));
      this.lblSubStatus.Invoke((Delegate) (() => this.lblSubStatus.Text = detail));
      if (progress != -1)
        this.progressBar1.SetProgress(progress, color ?? this._defaultColor);
      this.Invoke((Delegate) (() => this.Invalidate(true)));
      Application.DoEvents();
    }

    public void IncrementProgress(string activity, string detail, int progress, Color? color = null)
    {
      this.UpdateProgress(activity, detail, this.progressBar1.Value + progress, color);
    }

    public void SetProgressMax(int value) => this.progressBar1.Maximum = value;

    private void ProgressDialog_Load(object sender, EventArgs e)
    {
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);
      e.Graphics.DrawRectangle(new Pen(Color.Black, 2f), 0, 0, this.Width - 2, this.Height - 2);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panel1 = new Panel();
      this.panel2 = new Panel();
      this.lblSubStatus = new Label();
      this.progressBar1 = new RegionProgressBar();
      this.lblStatus = new Label();
      this.pnlIcon = new Panel();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.panel1.BackColor = SystemColors.InactiveCaption;
      this.panel1.Dock = DockStyle.Bottom;
      this.panel1.Location = new Point(0, 68);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(479, 20);
      this.panel1.TabIndex = 0;
      this.panel1.UseWaitCursor = true;
      this.panel2.BackColor = SystemColors.Control;
      this.panel2.Controls.Add((Control) this.lblSubStatus);
      this.panel2.Controls.Add((Control) this.progressBar1);
      this.panel2.Controls.Add((Control) this.lblStatus);
      this.panel2.Controls.Add((Control) this.pnlIcon);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(0, 0);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(479, 68);
      this.panel2.TabIndex = 1;
      this.panel2.UseWaitCursor = true;
      this.lblSubStatus.Location = new Point(276, 13);
      this.lblSubStatus.Name = "lblSubStatus";
      this.lblSubStatus.Size = new Size(178, 14);
      this.lblSubStatus.TabIndex = 3;
      this.lblSubStatus.Text = "Running plugins";
      this.lblSubStatus.TextAlign = ContentAlignment.TopRight;
      this.lblSubStatus.UseWaitCursor = true;
      this.progressBar1.Location = new Point(50, 31);
      this.progressBar1.Name = "progressBar1";
      this.progressBar1.Size = new Size(404, 20);
      this.progressBar1.Style = ProgressBarStyle.Continuous;
      this.progressBar1.TabIndex = 2;
      this.progressBar1.UseWaitCursor = true;
      this.lblStatus.AutoSize = true;
      this.lblStatus.Font = new Font("Arial Narrow", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblStatus.Location = new Point(46, 8);
      this.lblStatus.Name = "lblStatus";
      this.lblStatus.Size = new Size(87, 20);
      this.lblStatus.TabIndex = 1;
      this.lblStatus.Text = "Saving Loan";
      this.lblStatus.UseWaitCursor = true;
      this.pnlIcon.Location = new Point(3, 13);
      this.pnlIcon.Name = "pnlIcon";
      this.pnlIcon.Size = new Size(41, 38);
      this.pnlIcon.TabIndex = 0;
      this.pnlIcon.UseWaitCursor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(479, 88);
      this.ControlBox = false;
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.panel1);
      this.FormBorderStyle = FormBorderStyle.None;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.SizeGripStyle = SizeGripStyle.Hide;
      this.StartPosition = FormStartPosition.CenterParent;
      this.TopMost = true;
      this.UseWaitCursor = true;
      this.Text = nameof (LoanSaveProgressDialog);
      this.Load += new EventHandler(this.ProgressDialog_Load);
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
