// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradeLoanUpdateNotificationDialog
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class TradeLoanUpdateNotificationDialog : Form
  {
    private const int edgePadding = 10;
    private IContainer components;
    private FormFader formFader;
    private Label lblUpdateResults;

    public TradeLoanUpdateNotificationDialog(string tradeName, string status)
    {
      this.InitializeComponent();
      this.formFader.AttachToForm((Form) this);
      this.StartPosition = FormStartPosition.Manual;
      this.Left = Screen.PrimaryScreen.WorkingArea.Width - this.Width - 10;
      this.Top = Screen.PrimaryScreen.WorkingArea.Height - this.Height - 10;
      if (status == "Cancelled")
        this.lblUpdateResults.Text = "Trade Loan Update job for " + tradeName + " has been cancelled";
      else
        this.lblUpdateResults.Text = "Trade Loan Update job for " + tradeName + " has been updated";
    }

    public TradeLoanUpdateNotificationDialog(List<string> tradeNames, string status)
    {
      this.InitializeComponent();
      this.formFader.AttachToForm((Form) this);
      this.StartPosition = FormStartPosition.Manual;
      this.Left = Screen.PrimaryScreen.WorkingArea.Width - this.Width - 10;
      this.Top = Screen.PrimaryScreen.WorkingArea.Height - this.Height - 10;
      if (status == "Cancelled")
        this.lblUpdateResults.Text = "Trade Loan Update job for " + string.Join(", ", (IEnumerable<string>) tradeNames) + " " + (tradeNames.Count == 1 ? "has" : "have") + " been cancelled";
      else
        this.lblUpdateResults.Text = "Trade Loan Update job for " + string.Join(", ", (IEnumerable<string>) tradeNames) + " " + (tradeNames.Count == 1 ? "has" : "have") + " been updated";
    }

    private void onMouseEnterForm(object sender, EventArgs e) => this.formFader.EnsureOpaque();

    private void onMouseLeaveForm(object sender, EventArgs e)
    {
      if (this.RectangleToScreen(this.ClientRectangle).Contains(Cursor.Position))
        return;
      this.formFader.Resume();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.formFader = new FormFader();
      this.lblUpdateResults = new Label();
      this.SuspendLayout();
      this.formFader.DisplayDuration = 4000;
      this.formFader.OpacityIncrement = 8;
      this.lblUpdateResults.AutoSize = true;
      this.lblUpdateResults.Location = new Point(26, 24);
      this.lblUpdateResults.Name = "lblUpdateResults";
      this.lblUpdateResults.Size = new Size(35, 13);
      this.lblUpdateResults.TabIndex = 0;
      this.lblUpdateResults.Text = "label1";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(409, 63);
      this.Controls.Add((Control) this.lblUpdateResults);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (TradeLoanUpdateNotificationDialog);
      this.ShowIcon = false;
      this.Text = "Trade Loan Update Notification";
      this.MouseLeave += new EventHandler(this.onMouseLeaveForm);
      this.MouseEnter += new EventHandler(this.onMouseEnterForm);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
