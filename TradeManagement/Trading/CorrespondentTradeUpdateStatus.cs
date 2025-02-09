// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.CorrespondentTradeUpdateStatus
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.Common;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class CorrespondentTradeUpdateStatus : Form
  {
    public TradeStatus result;
    private TradeStatus oldStatus;
    private IContainer components;
    private Label label1;
    private RadioButton rdoCommitted;
    private RadioButton rdoDelivered;
    private RadioButton rdoSettled;
    private Button btnOk;
    private Button btnCancel;

    public CorrespondentTradeUpdateStatus() => this.InitializeComponent();

    public CorrespondentTradeUpdateStatus(TradeStatus st)
    {
      this.InitializeComponent();
      this.oldStatus = st;
      switch (this.oldStatus)
      {
        case TradeStatus.Committed:
          this.rdoCommitted.Enabled = false;
          break;
        case TradeStatus.Delivered:
          this.rdoDelivered.Enabled = false;
          break;
        case TradeStatus.Settled:
          this.rdoSettled.Enabled = false;
          break;
      }
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      if (this.rdoCommitted.Checked)
        this.result = TradeStatus.Committed;
      if (this.rdoDelivered.Checked)
        this.result = TradeStatus.Delivered;
      if (this.rdoSettled.Checked)
        this.result = TradeStatus.Settled;
      if (this.oldStatus == this.result)
        return;
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to change the status of the trade?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
      {
        this.result = this.oldStatus;
        this.DialogResult = DialogResult.No;
      }
      else
        this.DialogResult = DialogResult.Yes;
    }

    private void btnCancel_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.No;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.rdoCommitted = new RadioButton();
      this.rdoDelivered = new RadioButton();
      this.rdoSettled = new RadioButton();
      this.btnOk = new Button();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 20);
      this.label1.Name = "label1";
      this.label1.Size = new Size(100, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Update trade status";
      this.rdoCommitted.AutoSize = true;
      this.rdoCommitted.Location = new Point(115, 36);
      this.rdoCommitted.Name = "rdoCommitted";
      this.rdoCommitted.Size = new Size(74, 17);
      this.rdoCommitted.TabIndex = 1;
      this.rdoCommitted.TabStop = true;
      this.rdoCommitted.Text = "Committed";
      this.rdoCommitted.UseVisualStyleBackColor = true;
      this.rdoDelivered.AutoSize = true;
      this.rdoDelivered.Location = new Point(115, 59);
      this.rdoDelivered.Name = "rdoDelivered";
      this.rdoDelivered.Size = new Size(70, 17);
      this.rdoDelivered.TabIndex = 2;
      this.rdoDelivered.TabStop = true;
      this.rdoDelivered.Text = "Delivered";
      this.rdoDelivered.UseVisualStyleBackColor = true;
      this.rdoSettled.AutoSize = true;
      this.rdoSettled.Location = new Point(115, 82);
      this.rdoSettled.Name = "rdoSettled";
      this.rdoSettled.Size = new Size(58, 17);
      this.rdoSettled.TabIndex = 3;
      this.rdoSettled.TabStop = true;
      this.rdoSettled.Text = "Settled";
      this.rdoSettled.UseVisualStyleBackColor = true;
      this.btnOk.Location = new Point(137, 118);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(87, 23);
      this.btnOk.TabIndex = 4;
      this.btnOk.Text = "OK";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.btnCancel.Location = new Point(230, 118);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(79, 23);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(324, 153);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOk);
      this.Controls.Add((Control) this.rdoSettled);
      this.Controls.Add((Control) this.rdoDelivered);
      this.Controls.Add((Control) this.rdoCommitted);
      this.Controls.Add((Control) this.label1);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (CorrespondentTradeUpdateStatus);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Update Status";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
