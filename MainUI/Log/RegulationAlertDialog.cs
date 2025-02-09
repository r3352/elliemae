// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.RegulationAlertDialog
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class RegulationAlertDialog : Form
  {
    private AlertDialogPanel lastPanel;
    private IContainer components;
    private Panel pnlButtons;
    private Button btnClose;
    private Button btnDetails;
    private EMHelpLink emHelpLink1;

    public RegulationAlertDialog(PipelineInfo.Alert[] alerts)
    {
      this.InitializeComponent();
      int height = this.pnlButtons.Height;
      for (int index = alerts.Length - 1; index >= 0; --index)
      {
        AlertDialogPanel alertDialogPanel = new AlertDialogPanel(alerts[index]);
        alertDialogPanel.Dock = DockStyle.Top;
        alertDialogPanel.ViewDetails += new EventHandler(this.onViewAlertDetails);
        if (index == alerts.Length - 1)
        {
          this.lastPanel = alertDialogPanel;
          alertDialogPanel.DetailsButtonVisible = false;
        }
        this.Controls.Add((Control) alertDialogPanel);
        height += alertDialogPanel.Height;
      }
      this.ClientSize = new Size(this.ClientSize.Width, height);
    }

    private void onViewAlertDetails(object sender, EventArgs e)
    {
      AlertDialogPanel alertDialogPanel = (AlertDialogPanel) sender;
      Session.Application.GetService<ILoanEditor>().AddToWorkArea(AlertPanels.GetAlertPanel(alertDialogPanel.Alert));
      Session.Application.GetService<ILoanEditor>().RefreshLogPanel();
      this.DialogResult = DialogResult.Cancel;
    }

    public static DialogResult DisplayAlerts(IWin32Window parent)
    {
      PipelineInfo pipelineInfo = Session.LoanData.ToPipelineInfo();
      pipelineInfo.UpdateAlerts(Session.UserInfo, Session.LoanDataMgr.SystemConfiguration.UserAclGroups, Session.LoanDataMgr.SystemConfiguration.AlertSetupData);
      List<PipelineInfo.Alert> alertList = new List<PipelineInfo.Alert>();
      foreach (PipelineInfo.Alert alert in pipelineInfo.Alerts)
      {
        if (Alerts.IsRegulationAlert(alert.AlertID) && Session.LoanDataMgr.SystemConfiguration.AlertSetupData.GetAlertConfig(alert.AlertID) != null)
          alertList.Add(alert);
      }
      if (alertList.Count == 0)
        return DialogResult.OK;
      using (RegulationAlertDialog regulationAlertDialog = new RegulationAlertDialog(alertList.ToArray()))
        return regulationAlertDialog.ShowDialog(parent);
    }

    private void btnClose_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    private void RegulationAlertDialog_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Escape)
        return;
      this.DialogResult = DialogResult.OK;
    }

    private void btnDetails_Click(object sender, EventArgs e)
    {
      this.onViewAlertDetails((object) this.lastPanel, EventArgs.Empty);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pnlButtons = new Panel();
      this.btnDetails = new Button();
      this.btnClose = new Button();
      this.emHelpLink1 = new EMHelpLink();
      this.pnlButtons.SuspendLayout();
      this.SuspendLayout();
      this.pnlButtons.Controls.Add((Control) this.emHelpLink1);
      this.pnlButtons.Controls.Add((Control) this.btnDetails);
      this.pnlButtons.Controls.Add((Control) this.btnClose);
      this.pnlButtons.Dock = DockStyle.Bottom;
      this.pnlButtons.Location = new Point(0, 288);
      this.pnlButtons.Name = "pnlButtons";
      this.pnlButtons.Size = new Size(499, 32);
      this.pnlButtons.TabIndex = 0;
      this.btnDetails.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnDetails.Location = new Point(323, 0);
      this.btnDetails.Name = "btnDetails";
      this.btnDetails.Size = new Size(84, 22);
      this.btnDetails.TabIndex = 4;
      this.btnDetails.Text = "View Details";
      this.btnDetails.UseVisualStyleBackColor = true;
      this.btnDetails.Click += new EventHandler(this.btnDetails_Click);
      this.btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnClose.Location = new Point(415, 0);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 23);
      this.btnClose.TabIndex = 0;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.HelpTag = "Regulation Alert";
      this.emHelpLink1.Location = new Point(5, 1);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(121, 19);
      this.emHelpLink1.TabIndex = 44;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(499, 320);
      this.Controls.Add((Control) this.pnlButtons);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (RegulationAlertDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Compliance Alerts";
      this.KeyDown += new KeyEventHandler(this.RegulationAlertDialog_KeyDown);
      this.pnlButtons.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
