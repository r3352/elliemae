// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.WaitDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class WaitDialog : Form
  {
    private Label lblDescription;
    private ProgressBar pbProgress;
    private Timer tmrTimer;
    private IContainer components;
    private WaitDialogFeedback feedbackFunction;
    private int timeout;
    private int elapsedTime;

    public WaitDialog(
      WaitDialogFeedback feedback,
      string description,
      int timeout,
      int timerInterval)
    {
      this.InitializeComponent();
      this.feedbackFunction = feedback;
      this.timeout = timeout;
      this.lblDescription.Text = description;
      this.tmrTimer.Interval = timerInterval;
    }

    public WaitDialog(WaitDialogFeedback feedback, string description, int timeout)
      : this(feedback, description, timeout, 1000)
    {
    }

    public WaitDialog(WaitDialogFeedback feedback, string description)
      : this(feedback, description, -1)
    {
    }

    public WaitDialog(WaitDialogFeedback feedback)
      : this(feedback, "Please wait...")
    {
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
      this.lblDescription = new Label();
      this.pbProgress = new ProgressBar();
      this.tmrTimer = new Timer(this.components);
      this.SuspendLayout();
      this.lblDescription.Location = new Point(16, 10);
      this.lblDescription.Name = "lblDescription";
      this.lblDescription.Size = new Size(324, 22);
      this.lblDescription.TabIndex = 3;
      this.lblDescription.Text = "Description Label";
      this.pbProgress.Location = new Point(14, 30);
      this.pbProgress.Name = "pbProgress";
      this.pbProgress.Size = new Size(326, 23);
      this.pbProgress.TabIndex = 2;
      this.tmrTimer.Interval = 1000;
      this.tmrTimer.Tick += new EventHandler(this.tmrTimer_Tick);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(358, 66);
      this.ControlBox = false;
      this.Controls.Add((Control) this.lblDescription);
      this.Controls.Add((Control) this.pbProgress);
      this.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Name = nameof (WaitDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Please wait...";
      this.ResumeLayout(false);
    }

    public DialogResult ShowWait()
    {
      this.elapsedTime = 0;
      this.tmrTimer.Enabled = true;
      return this.ShowDialog();
    }

    private void tmrTimer_Tick(object sender, EventArgs e)
    {
      if (!this.feedbackFunction())
      {
        this.tmrTimer.Enabled = false;
        this.DialogResult = DialogResult.OK;
        this.Close();
      }
      else if (this.timeout > 0)
      {
        this.elapsedTime += this.tmrTimer.Interval;
        this.pbProgress.Value = Math.Min(this.elapsedTime * 100 / this.timeout, 100);
        if (this.elapsedTime < this.timeout)
          return;
        this.tmrTimer.Enabled = false;
        this.DialogResult = DialogResult.Abort;
        this.Close();
      }
      else
      {
        this.elapsedTime = (this.elapsedTime + this.tmrTimer.Interval) % 20000;
        this.pbProgress.Value = (int) Math.Min((double) (this.elapsedTime * 100) / 20000.0, 100.0);
      }
    }
  }
}
