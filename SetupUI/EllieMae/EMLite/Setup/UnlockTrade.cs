// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.UnlockTrade
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer.TradeSynchronization;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Trading;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class UnlockTrade : Form
  {
    private SetUpContainer setupContainer;
    private IContainer components;
    private GridView lvwLoan;
    private ToolTip toolTip1;
    private StandardIconButton stdIconBtnRefresh;
    private GroupContainer gcUnlockLoan;
    private VerticalSeparator verticalSeparator1;
    private Button unlockButton;

    public UnlockTrade(SetUpContainer setupContainer)
    {
      this.setupContainer = setupContainer;
      this.InitializeComponent();
      this.LoadPendingTrades();
      this.lvwLoan.Sort(0, SortOrder.Ascending);
    }

    private void LoadPendingTrades()
    {
      GVColumnSort[] sortOrder = this.lvwLoan.Columns.GetSortOrder();
      this.lvwLoan.BeginUpdate();
      this.lvwLoan.Items.Clear();
      string s = Session.StartupInfo.TradeSettings[(object) "Trade.PendingTradesWaitTime"].ToString();
      List<TradeUnlockInfo> pendingTrades = Session.LoanTradeManager.GetPendingTrades(new List<TradeType>()
      {
        TradeType.CorrespondentTrade,
        TradeType.LoanTrade,
        TradeType.MbsPool
      }, string.IsNullOrEmpty(s) ? 20 : int.Parse(s));
      for (int index = 0; index < pendingTrades.Count; ++index)
      {
        TradeUnlockInfo tradeUnlockInfo = pendingTrades[index];
        if (tradeUnlockInfo != null)
        {
          tradeUnlockInfo.tradeInfo.TradeID.ToString();
          string name = tradeUnlockInfo.tradeInfo.Name;
          string description = tradeUnlockInfo.tradeInfo.TradeType.ToDescription();
          string pendingBy = tradeUnlockInfo.tradeInfo.PendingBy;
          string str = tradeUnlockInfo.batchJobId > 0 ? tradeUnlockInfo.batchJobId.ToString() : string.Empty;
          this.lvwLoan.Items.Add(new GVItem(name)
          {
            Tag = (object) tradeUnlockInfo,
            SubItems = {
              (object) description,
              (object) pendingBy,
              (object) str
            }
          });
        }
      }
      this.lvwLoan.EndUpdate();
      this.gcUnlockLoan.Text = "Pending Trades (" + (object) this.lvwLoan.Items.Count + ")";
      this.lvwLoan_SelectedIndexChanged((object) this, (EventArgs) null);
      this.lvwLoan.Sort(sortOrder);
    }

    private void unlockButton_Click(object sender, EventArgs e)
    {
      Dictionary<TradeType, List<int>> dictionary = new Dictionary<TradeType, List<int>>();
      List<TradeUnlockInfo> tradeBatchJobs = new List<TradeUnlockInfo>();
      foreach (GVItem selectedItem in this.lvwLoan.SelectedItems)
      {
        TradeUnlockInfo tag = (TradeUnlockInfo) selectedItem.Tag;
        if (tag.batchJobId <= 0)
        {
          if (!dictionary.ContainsKey(tag.tradeInfo.TradeType))
            dictionary.Add(tag.tradeInfo.TradeType, new List<int>());
          if (!dictionary[tag.tradeInfo.TradeType].Contains(tag.tradeInfo.TradeID))
            dictionary[tag.tradeInfo.TradeType].Add(tag.tradeInfo.TradeID);
        }
        else
          tradeBatchJobs.Add(tag);
      }
      if (dictionary.Count > 0)
      {
        if (dictionary.ContainsKey(TradeType.LoanTrade) && dictionary[TradeType.LoanTrade] != null && dictionary[TradeType.LoanTrade].Count > 0)
          Session.LoanTradeManager.SetTradeStatus(dictionary[TradeType.LoanTrade].ToArray(), TradeStatus.Open, TradeHistoryAction.UnlockPendingTrade, false);
        if (dictionary.ContainsKey(TradeType.MbsPool) && dictionary[TradeType.MbsPool] != null && dictionary[TradeType.MbsPool].Count > 0)
          Session.MbsPoolManager.SetTradeStatus(dictionary[TradeType.MbsPool].ToArray(), TradeStatus.Open, TradeHistoryAction.UnlockPendingTrade, false);
        if (dictionary.ContainsKey(TradeType.CorrespondentTrade) && dictionary[TradeType.CorrespondentTrade] != null && dictionary[TradeType.CorrespondentTrade].Count > 0)
          Session.CorrespondentTradeManager.SetTradeStatus(dictionary[TradeType.CorrespondentTrade].ToArray(), TradeStatus.Committed, TradeHistoryAction.UnlockPendingTrade, false);
      }
      if (tradeBatchJobs.Count > 0)
        TradeLoanUpdateUtils.UnlockTradesCancelBatchJobs(Session.SessionObjects, Session.UserInfo, tradeBatchJobs);
      this.LoadPendingTrades();
    }

    private void stdIconBtnRefresh_Click(object sender, EventArgs e) => this.LoadPendingTrades();

    private void lvwLoan_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.unlockButton.Enabled = this.lvwLoan.SelectedItems.Count > 0;
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
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      this.toolTip1 = new ToolTip(this.components);
      this.stdIconBtnRefresh = new StandardIconButton();
      this.gcUnlockLoan = new GroupContainer();
      this.verticalSeparator1 = new VerticalSeparator();
      this.lvwLoan = new GridView();
      this.unlockButton = new Button();
      ((ISupportInitialize) this.stdIconBtnRefresh).BeginInit();
      this.gcUnlockLoan.SuspendLayout();
      this.SuspendLayout();
      this.stdIconBtnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnRefresh.BackColor = Color.Transparent;
      this.stdIconBtnRefresh.Location = new Point(588, 5);
      this.stdIconBtnRefresh.MouseDownImage = (Image) null;
      this.stdIconBtnRefresh.Name = "stdIconBtnRefresh";
      this.stdIconBtnRefresh.Size = new Size(16, 17);
      this.stdIconBtnRefresh.StandardButtonType = StandardIconButton.ButtonType.RefreshButton;
      this.stdIconBtnRefresh.TabIndex = 7;
      this.stdIconBtnRefresh.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnRefresh, "Refresh");
      this.stdIconBtnRefresh.Click += new EventHandler(this.stdIconBtnRefresh_Click);
      this.gcUnlockLoan.Controls.Add((Control) this.verticalSeparator1);
      this.gcUnlockLoan.Controls.Add((Control) this.stdIconBtnRefresh);
      this.gcUnlockLoan.Controls.Add((Control) this.lvwLoan);
      this.gcUnlockLoan.Controls.Add((Control) this.unlockButton);
      this.gcUnlockLoan.Dock = DockStyle.Fill;
      this.gcUnlockLoan.HeaderForeColor = SystemColors.ControlText;
      this.gcUnlockLoan.Location = new Point(0, 0);
      this.gcUnlockLoan.Name = "gcUnlockLoan";
      this.gcUnlockLoan.Size = new Size(681, 426);
      this.gcUnlockLoan.TabIndex = 8;
      this.gcUnlockLoan.Text = "Locked Pending Trades (#)";
      this.verticalSeparator1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.verticalSeparator1.Location = new Point(610, 5);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 8;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.lvwLoan.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Trade ID / Pool ID / Commitment Number";
      gvColumn1.Width = 240;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column6";
      gvColumn2.Text = "Trade Type";
      gvColumn2.Width = 200;
      gvColumn3.ImageIndex = -1;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Locked By";
      gvColumn3.Width = 200;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "Batch Job ID";
      gvColumn4.Width = 206;
      this.lvwLoan.Columns.AddRange(new GVColumn[4]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4
      });
      this.lvwLoan.Dock = DockStyle.Fill;
      this.lvwLoan.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.lvwLoan.Location = new Point(1, 26);
      this.lvwLoan.Name = "lvwLoan";
      this.lvwLoan.Size = new Size(679, 399);
      this.lvwLoan.TabIndex = 4;
      this.lvwLoan.SelectedIndexChanged += new EventHandler(this.lvwLoan_SelectedIndexChanged);
      this.unlockButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.unlockButton.BackColor = SystemColors.Control;
      this.unlockButton.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.unlockButton.Location = new Point(615, 2);
      this.unlockButton.Name = "unlockButton";
      this.unlockButton.Size = new Size(62, 22);
      this.unlockButton.TabIndex = 6;
      this.unlockButton.Text = "&Unlock";
      this.unlockButton.UseVisualStyleBackColor = true;
      this.unlockButton.Click += new EventHandler(this.unlockButton_Click);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(681, 426);
      this.Controls.Add((Control) this.gcUnlockLoan);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (UnlockTrade);
      this.Text = "UnlockTradeForm";
      ((ISupportInitialize) this.stdIconBtnRefresh).EndInit();
      this.gcUnlockLoan.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
