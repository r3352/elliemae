// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.StatusReport
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class StatusReport : Form
  {
    private static StatusReport statusReportDlg;
    private IContainer components;
    private Label lblStatus;

    private StatusReport() => this.InitializeComponent();

    public StatusReport(string message)
    {
      this.InitializeComponent();
      this.lblStatus.Text = message;
    }

    private static void start()
    {
      if (StatusReport.statusReportDlg != null)
      {
        if (StatusReport.statusReportDlg.InvokeRequired)
        {
          MethodInvoker method = new MethodInvoker(StatusReport.start);
          StatusReport.statusReportDlg.Invoke((Delegate) method);
        }
        else
        {
          StatusReport.statusReportDlg.WindowState = FormWindowState.Minimized;
          StatusReport.statusReportDlg.WindowState = FormWindowState.Normal;
          StatusReport.statusReportDlg.TopMost = true;
          StatusReport.statusReportDlg.BringToFront();
          StatusReport.statusReportDlg.Select();
          StatusReport.statusReportDlg.TopMost = false;
        }
      }
      else
      {
        StatusReport.statusReportDlg = new StatusReport();
        StatusReport.statusReportDlg.TopMost = true;
        StatusReport.statusReportDlg.BringToFront();
        StatusReport.statusReportDlg.Select();
        StatusReport.statusReportDlg.Show();
      }
    }

    public void UpdateStatus(string message) => this.lblStatus.Text = message;

    public static void ProcessMessage(string message)
    {
      StatusReport.start();
      StatusReport.statusReportDlg.displayStatus(message);
    }

    private void displayStatus(string message)
    {
      if (this.InvokeRequired)
        this.Invoke((Delegate) new StatusReport.DisplayMessageDelegate(this.displayStatus), (object) message);
      else
        this.lblStatus.Text = message;
    }

    public static void CloseWindow()
    {
      StatusReport.start();
      StatusReport.statusReportDlg.Close();
      StatusReport.statusReportDlg = (StatusReport) null;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblStatus = new Label();
      this.SuspendLayout();
      this.lblStatus.AutoSize = true;
      this.lblStatus.Location = new Point(12, 21);
      this.lblStatus.Name = "lblStatus";
      this.lblStatus.Size = new Size(74, 13);
      this.lblStatus.TabIndex = 0;
      this.lblStatus.Text = "Current Status";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(307, 63);
      this.ControlBox = false;
      this.Controls.Add((Control) this.lblStatus);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Name = nameof (StatusReport);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Settings Synchronization";
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private delegate void DisplayMessageDelegate(string message);
  }
}
