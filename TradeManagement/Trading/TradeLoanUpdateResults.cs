// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradeLoanUpdateResults
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class TradeLoanUpdateResults : UserControl
  {
    private int tradeId;
    private TradeType tradeType;
    private Sessions.Session session;
    private GridView gvTrades;
    private GVItem item;
    private string jobGuid;
    private IContainer components;
    private System.Windows.Forms.LinkLabel lnkLblDetails;
    private System.Windows.Forms.LinkLabel lnkLblClear;
    private VerticalSeparator verticalSeparator1;

    public TradeLoanUpdateResults() => this.InitializeComponent();

    public TradeLoanUpdateResults(
      Sessions.Session session,
      string jobGuid,
      int tradeId,
      TradeType tradeType,
      GridView gvTrades,
      GVItem item)
    {
      this.InitializeComponent();
      this.tradeId = tradeId;
      this.session = session;
      this.tradeType = tradeType;
      this.jobGuid = jobGuid;
      this.gvTrades = gvTrades;
      this.item = item;
    }

    private void lnkLblDetails_Click(object sender, EventArgs e)
    {
      this.lnkLblDetails.LinkVisited = true;
      this.session.MainScreen.NavigateToTradesTab(this.tradeId);
      if (this.tradeType == TradeType.LoanTrade)
        TradeManagementConsole.Instance.OpenTrade(this.tradeId, true);
      if (this.tradeType == TradeType.MbsPool)
        TradeManagementConsole.Instance.OpenMbsPool(this.tradeId, true);
      if (this.tradeType != TradeType.CorrespondentTrade)
        return;
      TradeManagementConsole.Instance.OpenCorrespondentTrade(this.tradeId, true);
    }

    private void lnkLblClear_Click(object sender, EventArgs e)
    {
      Session.TradeLoanUpdateQueueManager.DeleteQueue(this.jobGuid);
      this.gvTrades.Items.Remove(this.item);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lnkLblDetails = new System.Windows.Forms.LinkLabel();
      this.lnkLblClear = new System.Windows.Forms.LinkLabel();
      this.verticalSeparator1 = new VerticalSeparator();
      this.SuspendLayout();
      this.lnkLblDetails.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lnkLblDetails.AutoSize = true;
      this.lnkLblDetails.LinkBehavior = LinkBehavior.AlwaysUnderline;
      this.lnkLblDetails.Location = new Point(54, 8);
      this.lnkLblDetails.Name = "lnkLblDetails";
      this.lnkLblDetails.Size = new Size(39, 13);
      this.lnkLblDetails.TabIndex = 1;
      this.lnkLblDetails.TabStop = true;
      this.lnkLblDetails.Text = "Details";
      this.lnkLblDetails.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnkLblDetails_Click);
      this.lnkLblDetails.Click += new EventHandler(this.lnkLblDetails_Click);
      this.lnkLblClear.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lnkLblClear.AutoSize = true;
      this.lnkLblClear.LinkBehavior = LinkBehavior.AlwaysUnderline;
      this.lnkLblClear.Location = new Point(1, 8);
      this.lnkLblClear.Name = "lnkLblClear";
      this.lnkLblClear.Size = new Size(31, 13);
      this.lnkLblClear.TabIndex = 2;
      this.lnkLblClear.TabStop = true;
      this.lnkLblClear.Text = "Clear";
      this.lnkLblClear.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnkLblClear_Click);
      this.verticalSeparator1.Location = new Point(42, 1);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 4;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = SystemColors.Control;
      this.Controls.Add((Control) this.verticalSeparator1);
      this.Controls.Add((Control) this.lnkLblClear);
      this.Controls.Add((Control) this.lnkLblDetails);
      this.Name = nameof (TradeLoanUpdateResults);
      this.Size = new Size(119, 22);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
