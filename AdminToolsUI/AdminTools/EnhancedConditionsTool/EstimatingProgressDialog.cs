// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.EnhancedConditionsTool.EstimatingProgressDialog
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools.EnhancedConditionsTool
{
  public class EstimatingProgressDialog : Form
  {
    private DateTime lastUpdate;
    private EstimatingProgressDialog.SimpleRateCalc rateCalc;
    private bool verifyCancel = true;
    private IContainer components;
    private Label lblOverallDescription;
    private ProgressBar progressBar;
    private Button btnCancel;
    private Label lblEstimate;
    private Label lblCurrentDesc;

    public event EventHandler OnCancelRequested;

    public string Title
    {
      set => this.Text = value;
    }

    public string OverallDescription
    {
      set => this.lblOverallDescription.Text = value;
    }

    public string CurrentDescription
    {
      set => this.lblCurrentDesc.Text = value;
    }

    public int CurrentValue
    {
      get => this.progressBar.Value;
      set
      {
        if (value == this.progressBar.Value)
          return;
        DateTime now = DateTime.Now;
        long ticks = (now - this.lastUpdate).Ticks;
        float num = ticks > 0L ? (float) ticks / (float) (value - this.progressBar.Value) : 0.0f;
        this.progressBar.Value = value;
        if (this.lastUpdate > DateTime.MinValue)
          this.rateCalc.Push(num);
        this.lastUpdate = now;
      }
    }

    public int MaxValue
    {
      get => this.progressBar.Maximum;
      set => this.progressBar.Maximum = value;
    }

    public EstimatingProgressDialog() => this.InitializeComponent();

    public void StartEstimating(CancellationToken cancellationToken)
    {
      this.lastUpdate = DateTime.Now;
      this.rateCalc = new EstimatingProgressDialog.SimpleRateCalc();
      int updateDelay = 1000;
      string format = "m\\:ss";
      double ticksPerMsec = 10000.0;
      this.lblEstimate.Text = this.lblCurrentDesc.Text = "";
      Action<string> setEstimateText = (Action<string>) (text => this.lblEstimate.Text = text);
      Task.Run((Func<Task>) (async () =>
      {
        while (!cancellationToken.IsCancellationRequested)
        {
          string str = "";
          if (this.MaxValue > 0 && this.CurrentValue > 0 && this.rateCalc.IsValid)
            str = "Est. remaining: " + new TimeSpan((long) (this.rateCalc.Rate * (double) (this.MaxValue - this.CurrentValue) + 500.0 * ticksPerMsec)).ToString(format);
          if (this.IsHandleCreated && !this.IsDisposed)
            this.Invoke((Delegate) setEstimateText, (object) str);
          await Task.Delay(updateDelay).ConfigureAwait(false);
        }
      })).ConfigureAwait(false);
    }

    public void DoCancel()
    {
      EventHandler onCancelRequested = this.OnCancelRequested;
      if (onCancelRequested == null)
        return;
      onCancelRequested((object) this, new EventArgs());
    }

    public void DoClose()
    {
      this.verifyCancel = false;
      this.Close();
    }

    private void EstimatingProgressDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (e.CloseReason != CloseReason.UserClosing || !this.verifyCancel)
        return;
      this.DoCancel();
    }

    private void btnCancel_Click(object sender, EventArgs e) => this.DoCancel();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblOverallDescription = new Label();
      this.progressBar = new ProgressBar();
      this.btnCancel = new Button();
      this.lblEstimate = new Label();
      this.lblCurrentDesc = new Label();
      this.SuspendLayout();
      this.lblOverallDescription.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblOverallDescription.Location = new Point(12, 17);
      this.lblOverallDescription.Name = "lblOverallDescription";
      this.lblOverallDescription.Size = new Size(550, 52);
      this.lblOverallDescription.TabIndex = 0;
      this.lblOverallDescription.Text = "The overall description of the operation goes here\\nIt can be multi-line";
      this.progressBar.Location = new Point(12, 83);
      this.progressBar.Maximum = 0;
      this.progressBar.Name = "progressBar";
      this.progressBar.Size = new Size(554, 31);
      this.progressBar.TabIndex = 1;
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.Location = new Point(438, 162);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(128, 36);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.lblEstimate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblEstimate.Location = new Point(386, 117);
      this.lblEstimate.Name = "lblEstimate";
      this.lblEstimate.Size = new Size(180, 32);
      this.lblEstimate.TabIndex = 3;
      this.lblEstimate.Text = "Remaining: 0:00";
      this.lblEstimate.TextAlign = ContentAlignment.TopRight;
      this.lblCurrentDesc.Location = new Point(12, 117);
      this.lblCurrentDesc.Name = "lblCurrentDesc";
      this.lblCurrentDesc.Size = new Size(351, 32);
      this.lblCurrentDesc.TabIndex = 4;
      this.lblCurrentDesc.Text = "Processing";
      this.AutoScaleDimensions = new SizeF(9f, 20f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(578, 210);
      this.Controls.Add((Control) this.lblCurrentDesc);
      this.Controls.Add((Control) this.lblEstimate);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.progressBar);
      this.Controls.Add((Control) this.lblOverallDescription);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.Name = nameof (EstimatingProgressDialog);
      this.ShowIcon = false;
      this.SizeGripStyle = SizeGripStyle.Hide;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = nameof (EstimatingProgressDialog);
      this.FormClosing += new FormClosingEventHandler(this.EstimatingProgressDialog_FormClosing);
      this.ResumeLayout(false);
    }

    private class SimpleRateCalc
    {
      public SimpleRateCalc()
      {
        this.Count = 0;
        this.Sum = 0.0;
      }

      public int Push(float value)
      {
        this.Sum += (double) value;
        ++this.Count;
        return this.Count;
      }

      private int Count { get; set; }

      private double Sum { get; set; }

      public double Rate => !this.IsValid ? 0.0 : this.Sum / (double) this.Count;

      public bool IsValid => this.Count > 0;
    }
  }
}
