// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.MigrateSecurityTradeDataDialog
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class MigrateSecurityTradeDataDialog : Form
  {
    private IContainer components;
    private Timer timer1;
    private GradientPanel gradPnlTop;
    private GradientPanel gradPnlBottom;
    private LabelEx lblExTitle;
    private LabelEx lblMessage;

    public MigrateSecurityTradeDataDialog() => this.InitializeComponent();

    public void MigrateSecurityTradeData()
    {
      try
      {
        Session.ConfigurationManager.SetCompanySetting("MIGRATION", "TradeSecurityTrade14.2.0.3", "0");
        SecurityTradeInfo[] activeTrades = Session.SecurityTradeManager.GetActiveTrades();
        if (activeTrades != null)
        {
          foreach (SecurityTradeInfo tradeInfo in activeTrades)
          {
            if (!(tradeInfo.OpenAmount != 0M))
            {
              SecurityTradeAssignment[] tradeAssigments = Session.SecurityTradeManager.GetTradeAssigments(tradeInfo.TradeID);
              MbsPoolAssignment[] assigmentsBySecurityTrade = Session.MbsPoolManager.GetTradeAssigmentsBySecurityTrade(tradeInfo.TradeID);
              tradeInfo.OpenAmount = tradeInfo.Calculation.CalculateOpenAmount(tradeAssigments, assigmentsBySecurityTrade);
              tradeInfo.TotalPairOffGainLoss = tradeInfo.Calculation.CalculateTotalGainLossAmount(tradeAssigments);
              Session.SecurityTradeManager.UpdateTrade(tradeInfo);
            }
          }
        }
        Session.ConfigurationManager.SetCompanySetting("MIGRATION", "TradeSecurityTrade14.2.0.3", "1");
      }
      catch (Exception ex)
      {
      }
    }

    private void Form_Shown(object sender, EventArgs e)
    {
      Application.DoEvents();
      this.MigrateSecurityTradeData();
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
      this.components = (IContainer) new System.ComponentModel.Container();
      this.timer1 = new Timer(this.components);
      this.gradPnlBottom = new GradientPanel();
      this.lblMessage = new LabelEx();
      this.gradPnlTop = new GradientPanel();
      this.lblExTitle = new LabelEx();
      this.gradPnlBottom.SuspendLayout();
      this.gradPnlTop.SuspendLayout();
      this.SuspendLayout();
      this.gradPnlBottom.Controls.Add((Control) this.lblMessage);
      this.gradPnlBottom.Dock = DockStyle.Fill;
      this.gradPnlBottom.Location = new Point(0, 26);
      this.gradPnlBottom.Name = "gradPnlBottom";
      this.gradPnlBottom.Size = new Size(430, 84);
      this.gradPnlBottom.TabIndex = 1;
      this.lblMessage.BackColor = Color.Transparent;
      this.lblMessage.Dock = DockStyle.Fill;
      this.lblMessage.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblMessage.ImageAlign = ContentAlignment.MiddleLeft;
      this.lblMessage.Location = new Point(0, 0);
      this.lblMessage.Name = "lblMessage";
      this.lblMessage.Size = new Size(430, 84);
      this.lblMessage.TabIndex = 0;
      this.lblMessage.Text = "Security Trade Data is Loading...";
      this.lblMessage.TextAlign = ContentAlignment.MiddleCenter;
      this.gradPnlTop.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gradPnlTop.Controls.Add((Control) this.lblExTitle);
      this.gradPnlTop.Dock = DockStyle.Top;
      this.gradPnlTop.Location = new Point(0, 0);
      this.gradPnlTop.Name = "gradPnlTop";
      this.gradPnlTop.Size = new Size(430, 26);
      this.gradPnlTop.TabIndex = 0;
      this.lblExTitle.BackColor = Color.Transparent;
      this.lblExTitle.Dock = DockStyle.Top;
      this.lblExTitle.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblExTitle.Location = new Point(0, 0);
      this.lblExTitle.Name = "lblExTitle";
      this.lblExTitle.Size = new Size(430, 26);
      this.lblExTitle.TabIndex = 0;
      this.lblExTitle.Text = "Encompass Message";
      this.lblExTitle.TextAlign = ContentAlignment.MiddleCenter;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = SystemColors.Control;
      this.ClientSize = new Size(430, 110);
      this.Controls.Add((Control) this.gradPnlBottom);
      this.Controls.Add((Control) this.gradPnlTop);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (MigrateSecurityTradeDataDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Save Confirmation";
      this.Shown += new EventHandler(this.Form_Shown);
      this.gradPnlBottom.ResumeLayout(false);
      this.gradPnlTop.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
