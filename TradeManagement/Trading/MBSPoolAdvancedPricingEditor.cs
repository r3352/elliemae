// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.MBSPoolAdvancedPricingEditor
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class MBSPoolAdvancedPricingEditor : UserControl
  {
    private TradeAdvancedPricingInfo pricingInfo;
    private bool modified;
    private bool readOnly;
    public string gseContractNum = "";
    public string productName = "";
    private Decimal coupon;
    private Decimal weightedAvgPrice;
    private Decimal guaranteeFee;
    private Decimal serviceFee;
    private Decimal minServicingFee;
    private Decimal maxBU;
    private Decimal cpa;
    private IContainer components;
    private ToolTip toolTip1;
    private FlowLayoutPanel flowLayoutPanel1;
    private StandardIconButton siBtnDelete;
    private StandardIconButton siBtnAdd;
    private FlowLayoutPanel flPnlLeft;
    private GroupContainer groupContainer1;
    private GridView gvAdvancedPriceItem;
    private Panel panel1;
    private Panel pnlWeightedAvgPrice;
    private TextBox txtWeightedAvgPrice;
    private Label label5;
    private Panel pnlServicing;
    private TextBox txtMaxBU;
    private Label lblServiceFee;
    private TextBox txtMinServicingFee;
    private Label label3;
    private TextBox txtGuaranteeFee;
    private Label label4;
    private TextBox txtCoupon;
    private Label label1;
    private Process process1;
    private TextBox txtServiceFee;
    private Label label2;

    public MBSPoolAdvancedPricingEditor()
    {
      this.InitializeComponent();
      this.gvAdvancedPriceItem.Sort(0, SortOrder.Ascending);
    }

    public void AddControlToHeader(Control c, bool toLeft)
    {
      if (toLeft)
        this.flPnlLeft.Controls.Add(c);
      else
        this.flowLayoutPanel1.Controls.Add(c);
      c.BringToFront();
    }

    public void CommitChanges()
    {
      this.pricingInfo.PricingItems.Clear();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvAdvancedPriceItem.Items)
        this.pricingInfo.PricingItems.Add(gvItem.Tag as TradeAdvancedPricingItem);
      this.modified = false;
    }

    public void CommitChanges(string gseCommitmentNumber, string productName)
    {
      if (this.pricingInfo != null)
      {
        List<TradeAdvancedPricingItem> list = this.pricingInfo.PricingItems.ToList<TradeAdvancedPricingItem>();
        foreach (TradeAdvancedPricingItem advancedPricingItem in this.pricingInfo.PricingItems.ToArray<TradeAdvancedPricingItem>())
        {
          if (string.Compare(advancedPricingItem.GSEContractNumber, gseCommitmentNumber, StringComparison.OrdinalIgnoreCase) == 0 && string.Compare(advancedPricingItem.ProductName, this.productName, StringComparison.OrdinalIgnoreCase) == 0)
            list.Remove(advancedPricingItem);
        }
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvAdvancedPriceItem.Items)
          list.Add(gvItem.Tag as TradeAdvancedPricingItem);
        this.pricingInfo.PricingItems.Clear();
        foreach (TradeAdvancedPricingItem advancedPricingItem in list)
          this.pricingInfo.PricingItems.Add(advancedPricingItem);
      }
      this.productName = productName;
      this.gseContractNum = gseCommitmentNumber;
      this.modified = false;
    }

    public void ClearData()
    {
      this.gvAdvancedPriceItem.Items.Clear();
      this.modified = true;
    }

    private bool ValidateItems(List<TradeAdvancedPricingItem> items)
    {
      for (int index = 0; index < items.Count<TradeAdvancedPricingItem>(); ++index)
      {
        TradeAdvancedPricingItem advancedPricingItem1 = items[index];
        if (index < items.Count - 1)
        {
          TradeAdvancedPricingItem advancedPricingItem2 = items[index + 1];
          if (advancedPricingItem2.NoteRate == advancedPricingItem1.NoteRate)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "Detailed Pricing Grid: A price for the rate " + (object) advancedPricingItem2.NoteRate + " already exists.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return false;
          }
        }
      }
      return true;
    }

    public bool ValidatePricing(bool popupMsg = true)
    {
      if (this.gvAdvancedPriceItem.Items.Count < 2)
        return true;
      TradeAdvancedPricingItem tag1 = (TradeAdvancedPricingItem) this.gvAdvancedPriceItem.Items[0].Tag;
      TradeAdvancedPricingItem tag2 = (TradeAdvancedPricingItem) this.gvAdvancedPriceItem.Items[this.gvAdvancedPriceItem.Items.Count - 1].Tag;
      TradeAdvancedPricingItem advancedPricingItem = tag1;
      for (int nItemIndex = 1; nItemIndex < this.gvAdvancedPriceItem.Items.Count; ++nItemIndex)
      {
        TradeAdvancedPricingItem tag3 = (TradeAdvancedPricingItem) this.gvAdvancedPriceItem.Items[nItemIndex].Tag;
        if (tag3.NoteRate - advancedPricingItem.NoteRate > 0.125M)
        {
          string text = "You must specify pricing for every 1/8 point increment between " + tag1.NoteRate.ToString("0.000") + " and " + tag2.NoteRate.ToString("0.000");
          if (Session.SessionObjects.Use10DecimalDigitInLockRequestSecondaryTradeAreas)
            text = "You must specify pricing for every 1/8 point increment between " + tag1.NoteRate.ToString("0.0000000000") + " and " + tag2.NoteRate.ToString("0.0000000000");
          if (popupMsg)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          return false;
        }
        advancedPricingItem = tag3;
      }
      return this.ValidateItems(this.gvAdvancedPriceItem.Items.Select<GVItem, TradeAdvancedPricingItem>((Func<GVItem, TradeAdvancedPricingItem>) (i => i.Tag as TradeAdvancedPricingItem)).OrderBy<TradeAdvancedPricingItem, Decimal>((Func<TradeAdvancedPricingItem, Decimal>) (i => i.NoteRate)).ToList<TradeAdvancedPricingItem>());
    }

    public bool ReadOnly
    {
      get => this.readOnly;
      set
      {
        if (this.readOnly == value)
          return;
        this.readOnly = value;
        this.gvAdvancedPriceItem.SelectedItems.Clear();
        this.siBtnAdd.Enabled = !this.readOnly;
        this.siBtnDelete.Enabled = false;
        this.txtCoupon.ReadOnly = this.txtGuaranteeFee.ReadOnly = this.txtServiceFee.ReadOnly = this.txtWeightedAvgPrice.ReadOnly = this.readOnly;
      }
    }

    public void DisableButtons(bool state)
    {
      this.siBtnAdd.Enabled = !state;
      this.siBtnDelete.Enabled = !state && this.gvAdvancedPriceItem.SelectedItems.Count > 0;
    }

    public Decimal Coupon
    {
      set
      {
        this.coupon = value;
        this.txtCoupon.Text = value.ToString("0.00000");
      }
    }

    public string CouponRange
    {
      set => this.txtCoupon.Text = value;
    }

    public Decimal WeightedAvgPrice
    {
      set
      {
        this.weightedAvgPrice = value;
        this.txtWeightedAvgPrice.Text = value.ToString("0.00000");
      }
    }

    public Decimal GuaranteeFee
    {
      set
      {
        this.guaranteeFee = value;
        this.txtGuaranteeFee.Text = value.ToString("0.00000");
      }
    }

    public Decimal ServiceFee
    {
      set
      {
        this.serviceFee = value;
        this.txtServiceFee.Text = value.ToString("0.00000");
      }
    }

    public Decimal MinServicingFee
    {
      set
      {
        this.minServicingFee = value;
        this.txtMinServicingFee.Text = value.ToString("0.00000");
      }
    }

    public Decimal MaxBU
    {
      set
      {
        this.maxBU = value;
        this.txtMaxBU.Text = value.ToString("0.00000");
      }
    }

    public Decimal CPA
    {
      set => this.cpa = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public TradeAdvancedPricingInfo PricingInfo
    {
      get => this.pricingInfo;
      set
      {
        this.pricingInfo = value ?? new TradeAdvancedPricingInfo();
        this.pricingInfo.Coupon = this.coupon;
        this.pricingInfo.Price = this.weightedAvgPrice;
        this.pricingInfo.GuaranteeFee = this.guaranteeFee;
        this.pricingInfo.ServiceFee = this.serviceFee;
        this.pricingInfo.MinServicingFee = this.minServicingFee;
        this.pricingInfo.MaxBU = this.maxBU;
        this.loadPricingInfo();
      }
    }

    public MbsPoolBuyUpDownItems BuyUpDownItems { get; set; }

    public MbsPoolMortgageType PoolType { get; set; }

    public bool DataModified => this.modified && !this.readOnly;

    public void loadPricingInfo(string gseCommitmentNumber = "", string productName = "")
    {
      this.gseContractNum = gseCommitmentNumber;
      this.productName = productName;
      this.gvAdvancedPriceItem.Items.Clear();
      if (this.pricingInfo.PricingItems != null)
      {
        foreach (TradeAdvancedPricingItem pricingItem in this.pricingInfo.PricingItems)
        {
          if (string.Compare(pricingItem.GSEContractNumber, gseCommitmentNumber, StringComparison.OrdinalIgnoreCase) == 0 && string.Compare(pricingItem.ProductName, productName, StringComparison.OrdinalIgnoreCase) == 0)
            this.gvAdvancedPriceItem.Items.Add(this.createPricingGVItem(pricingItem));
        }
      }
      this.gvAdvancedPriceItem.ReSort();
      if (this.PoolType == MbsPoolMortgageType.GinnieMae)
      {
        GVColumn columnByName1 = this.gvAdvancedPriceItem.Columns.GetColumnByName("Column2");
        if (columnByName1 != null)
          columnByName1.Width = 0;
        GVColumn columnByName2 = this.gvAdvancedPriceItem.Columns.GetColumnByName("Column3");
        if (columnByName2 != null)
          columnByName2.Width = 0;
        GVColumn columnByName3 = this.gvAdvancedPriceItem.Columns.GetColumnByName("Column6");
        if (columnByName3 != null)
          columnByName3.Width = 0;
        GVColumn columnByName4 = this.gvAdvancedPriceItem.Columns.GetColumnByName("Column7");
        if (columnByName4 != null)
          columnByName4.Width = 0;
        GVColumn columnByName5 = this.gvAdvancedPriceItem.Columns.GetColumnByName("Column8");
        if (columnByName5 != null)
          columnByName5.Width = 0;
        GVColumn columnByName6 = this.gvAdvancedPriceItem.Columns.GetColumnByName("Column9");
        if (columnByName6 != null)
          columnByName6.Width = 0;
        GVColumn columnByName7 = this.gvAdvancedPriceItem.Columns.GetColumnByName("Column4");
        if (columnByName7 != null)
          columnByName7.Width = 100;
      }
      else
      {
        GVColumn columnByName8 = this.gvAdvancedPriceItem.Columns.GetColumnByName("Column2");
        if (columnByName8 != null)
          columnByName8.Width = 85;
        GVColumn columnByName9 = this.gvAdvancedPriceItem.Columns.GetColumnByName("Column3");
        if (columnByName9 != null)
          columnByName9.Width = 85;
        GVColumn columnByName10 = this.gvAdvancedPriceItem.Columns.GetColumnByName("Column6");
        if (columnByName10 != null)
          columnByName10.Width = 100;
        GVColumn columnByName11 = this.gvAdvancedPriceItem.Columns.GetColumnByName("Column7");
        if (columnByName11 != null)
          columnByName11.Width = 85;
        GVColumn columnByName12 = this.gvAdvancedPriceItem.Columns.GetColumnByName("Column8");
        if (columnByName12 != null)
          columnByName12.Width = 85;
        if (this.PoolType != MbsPoolMortgageType.FannieMaePE)
        {
          GVColumn columnByName13 = this.gvAdvancedPriceItem.Columns.GetColumnByName("Column9");
          if (columnByName13 != null)
            columnByName13.Width = 0;
        }
        GVColumn columnByName14 = this.gvAdvancedPriceItem.Columns.GetColumnByName("Column4");
        if (columnByName14 != null)
          columnByName14.Width = 0;
      }
      this.ToggleFannieMaePEMbsPoolFields();
      this.modified = false;
    }

    private GVItem createPricingGVItem(TradeAdvancedPricingItem pricingItem)
    {
      GVItem pricingGvItem = new GVItem();
      pricingGvItem.Text = pricingItem.NoteRate.ToString("0.000");
      pricingGvItem.SubItems.Add((object) pricingItem.ServicingFee.ToString("0.0000"));
      pricingGvItem.SubItems.Add((object) pricingItem.GuarantyFee.ToString("0.00000"));
      pricingGvItem.SubItems.Add((object) pricingItem.ServicingRetained.ToString("0.00000"));
      pricingGvItem.SubItems.Add((object) pricingItem.BuyUp.ToString("0.00000"));
      pricingGvItem.SubItems.Add((object) pricingItem.BuyDown.ToString("0.00000"));
      pricingGvItem.SubItems.Add((object) pricingItem.GNMAIIExcess.ToString("0.00000"));
      pricingGvItem.SubItems.Add((object) this.cpa.ToString("0.000"));
      pricingItem.ProductName = this.productName;
      pricingItem.GSEContractNumber = this.gseContractNum;
      Decimal totalPrice = MbsPoolCalculation.CalculateTotalPrice(this.pricingInfo.Price, pricingItem, this.cpa);
      if (Session.SessionObjects.Use10DecimalDigitInLockRequestSecondaryTradeAreas)
        pricingGvItem.SubItems.Add((object) totalPrice.ToString("0.0000000000"));
      else
        pricingGvItem.SubItems.Add((object) totalPrice.ToString("0.000"));
      pricingGvItem.Tag = (object) pricingItem;
      return pricingGvItem;
    }

    private void TradeAdvancedPricingEditor_Resize(object sender, EventArgs e)
    {
      this.siBtnAdd.Left = Math.Max(0, this.ClientSize.Width - this.siBtnAdd.Width);
      this.siBtnDelete.Left = this.siBtnAdd.Left;
      if (this.panel1.HorizontalScroll.Visible)
      {
        this.panel1.Height = 70;
        this.gvAdvancedPriceItem.Location = new Point(1, 99);
      }
      else
      {
        this.panel1.Height = 50;
        this.gvAdvancedPriceItem.Location = new Point(1, 90);
      }
      this.gvAdvancedPriceItem.Height = this.ClientSize.Height - (this.lblServiceFee.Location.Y + this.lblServiceFee.Height + 55);
      this.gvAdvancedPriceItem.Width = Math.Max(0, this.ClientSize.Width - 10);
    }

    private void siBtnAdd_Click(object sender, EventArgs e)
    {
      if (this.PoolType == MbsPoolMortgageType.FannieMae || this.PoolType == MbsPoolMortgageType.FannieMaePE || this.PoolType == MbsPoolMortgageType.FreddieMac)
        this.AddMbsFannieFreddiePriceItem();
      else
        this.AddPriceItem();
      this.modified = true;
      if (!(this.gseContractNum != "") || !("" != this.productName))
        return;
      this.CommitChanges(this.gseContractNum, this.productName);
    }

    private void AddMbsFannieFreddiePriceItem()
    {
      MbsFannieFreddiePricingItemDialog pricingItemDialog = new MbsFannieFreddiePricingItemDialog(this.pricingInfo.ServiceFee, this.pricingInfo.Coupon, this.guaranteeFee, this.maxBU, this.pricingInfo.MinServicingFee, this.PoolType, this.BuyUpDownItems, this.weightedAvgPrice, this.cpa);
      if (pricingItemDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      this.SaveAndRefreshPricingItem(pricingItemDialog.PricingItem);
      if (!pricingItemDialog.Recursive)
        return;
      this.siBtnAdd_Click((object) null, (EventArgs) null);
    }

    private void AddPriceItem()
    {
      TradeAdvancedPricingItemDialog pricingItemDialog = new TradeAdvancedPricingItemDialog();
      pricingItemDialog.PoolType = this.PoolType;
      pricingItemDialog.ForTradeType = TradeType.MbsPool;
      pricingItemDialog.Coupon = this.pricingInfo.Coupon;
      pricingItemDialog.GuaranteeFee = this.guaranteeFee;
      pricingItemDialog.ServiceFee = this.pricingInfo.ServiceFee;
      pricingItemDialog.BuyUpDownItems = this.BuyUpDownItems;
      pricingItemDialog.MinServicingFee = this.pricingInfo.MinServicingFee;
      pricingItemDialog.MaxBU = this.pricingInfo.MaxBU;
      if (pricingItemDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      this.SaveAndRefreshPricingItem(pricingItemDialog.PricingItem);
      if (!pricingItemDialog.IsCreatingAnother)
        return;
      this.siBtnAdd_Click((object) null, (EventArgs) null);
    }

    private void SaveAndRefreshPricingItem(TradeAdvancedPricingItem pricingItem)
    {
      this.gvAdvancedPriceItem.Items.Add(this.createPricingGVItem(pricingItem));
      this.gvAdvancedPriceItem.ReSort();
    }

    private void gvAdvancedPriceItem_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.gvAdvancedPriceItem.SelectedItems.Count == 0)
        this.siBtnDelete.Enabled = false;
      else
        this.siBtnDelete.Enabled = true;
    }

    private void siBtnDelete_Click(object sender, EventArgs e)
    {
      this.gvAdvancedPriceItem.CancelEditing();
      foreach (GVItem selectedItem in this.gvAdvancedPriceItem.SelectedItems)
        this.gvAdvancedPriceItem.Items.Remove(selectedItem);
      this.gvAdvancedPriceItem.ReSort();
      this.modified = true;
      if (!(this.gseContractNum != "") || !("" != this.productName))
        return;
      this.CommitChanges(this.gseContractNum, this.productName);
    }

    private void txt_TextChanged(object sender, EventArgs e) => this.modified = true;

    private void ToggleFannieMaePEMbsPoolFields()
    {
      if (this.PoolType == MbsPoolMortgageType.GinnieMae)
      {
        this.pnlServicing.Visible = false;
        this.pnlWeightedAvgPrice.Location = new Point(211, 1);
        this.label2.Visible = true;
        this.txtServiceFee.Visible = true;
        this.label4.Text = "Guaranty Fee";
      }
      else
      {
        this.pnlServicing.Visible = true;
        this.pnlWeightedAvgPrice.Location = new Point(406, 1);
        this.label2.Visible = false;
        this.txtServiceFee.Visible = false;
        this.label4.Text = "Base Guaranty Fee";
      }
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
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      GVColumn gvColumn8 = new GVColumn();
      GVColumn gvColumn9 = new GVColumn();
      this.toolTip1 = new ToolTip(this.components);
      this.siBtnDelete = new StandardIconButton();
      this.siBtnAdd = new StandardIconButton();
      this.groupContainer1 = new GroupContainer();
      this.panel1 = new Panel();
      this.pnlWeightedAvgPrice = new Panel();
      this.txtServiceFee = new TextBox();
      this.label2 = new Label();
      this.txtWeightedAvgPrice = new TextBox();
      this.label5 = new Label();
      this.pnlServicing = new Panel();
      this.txtMaxBU = new TextBox();
      this.lblServiceFee = new Label();
      this.txtMinServicingFee = new TextBox();
      this.label3 = new Label();
      this.txtGuaranteeFee = new TextBox();
      this.label4 = new Label();
      this.txtCoupon = new TextBox();
      this.label1 = new Label();
      this.gvAdvancedPriceItem = new GridView();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.flPnlLeft = new FlowLayoutPanel();
      this.process1 = new Process();
      ((ISupportInitialize) this.siBtnDelete).BeginInit();
      ((ISupportInitialize) this.siBtnAdd).BeginInit();
      this.groupContainer1.SuspendLayout();
      this.panel1.SuspendLayout();
      this.pnlWeightedAvgPrice.SuspendLayout();
      this.pnlServicing.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      this.siBtnDelete.BackColor = Color.Transparent;
      this.siBtnDelete.Enabled = false;
      this.siBtnDelete.Location = new Point(257, 3);
      this.siBtnDelete.MouseDownImage = (Image) null;
      this.siBtnDelete.Name = "siBtnDelete";
      this.siBtnDelete.Size = new Size(16, 16);
      this.siBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.siBtnDelete.TabIndex = 0;
      this.siBtnDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.siBtnDelete, "Remove Pricing");
      this.siBtnDelete.Click += new EventHandler(this.siBtnDelete_Click);
      this.siBtnAdd.BackColor = Color.Transparent;
      this.siBtnAdd.Location = new Point(235, 3);
      this.siBtnAdd.MouseDownImage = (Image) null;
      this.siBtnAdd.Name = "siBtnAdd";
      this.siBtnAdd.Size = new Size(16, 16);
      this.siBtnAdd.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.siBtnAdd.TabIndex = 1;
      this.siBtnAdd.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.siBtnAdd, "Add Pricing");
      this.siBtnAdd.Click += new EventHandler(this.siBtnAdd_Click);
      this.groupContainer1.Controls.Add((Control) this.panel1);
      this.groupContainer1.Controls.Add((Control) this.gvAdvancedPriceItem);
      this.groupContainer1.Controls.Add((Control) this.flowLayoutPanel1);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(638, 292);
      this.groupContainer1.TabIndex = 1;
      this.groupContainer1.Text = "Detailed Pricing Grid";
      this.panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.panel1.AutoScroll = true;
      this.panel1.Controls.Add((Control) this.pnlWeightedAvgPrice);
      this.panel1.Controls.Add((Control) this.pnlServicing);
      this.panel1.Controls.Add((Control) this.txtGuaranteeFee);
      this.panel1.Controls.Add((Control) this.label4);
      this.panel1.Controls.Add((Control) this.txtCoupon);
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Location = new Point(3, 27);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(632, 50);
      this.panel1.TabIndex = 18;
      this.pnlWeightedAvgPrice.Controls.Add((Control) this.txtServiceFee);
      this.pnlWeightedAvgPrice.Controls.Add((Control) this.label2);
      this.pnlWeightedAvgPrice.Controls.Add((Control) this.txtWeightedAvgPrice);
      this.pnlWeightedAvgPrice.Controls.Add((Control) this.label5);
      this.pnlWeightedAvgPrice.Location = new Point(406, 1);
      this.pnlWeightedAvgPrice.Name = "pnlWeightedAvgPrice";
      this.pnlWeightedAvgPrice.Size = new Size(222, 44);
      this.pnlWeightedAvgPrice.TabIndex = 23;
      this.txtServiceFee.Location = new Point(114, 23);
      this.txtServiceFee.Name = "txtServiceFee";
      this.txtServiceFee.ReadOnly = true;
      this.txtServiceFee.Size = new Size(100, 20);
      this.txtServiceFee.TabIndex = 23;
      this.txtServiceFee.TextAlign = HorizontalAlignment.Right;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(4, 26);
      this.label2.Name = "label2";
      this.label2.Size = new Size(64, 13);
      this.label2.TabIndex = 22;
      this.label2.Text = "Service Fee";
      this.txtWeightedAvgPrice.Location = new Point(114, 1);
      this.txtWeightedAvgPrice.Name = "txtWeightedAvgPrice";
      this.txtWeightedAvgPrice.ReadOnly = true;
      this.txtWeightedAvgPrice.Size = new Size(100, 20);
      this.txtWeightedAvgPrice.TabIndex = 21;
      this.txtWeightedAvgPrice.TextAlign = HorizontalAlignment.Right;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(4, 4);
      this.label5.Name = "label5";
      this.label5.Size = new Size(105, 13);
      this.label5.TabIndex = 20;
      this.label5.Text = "Weighted Avg. Price";
      this.pnlServicing.Controls.Add((Control) this.txtMaxBU);
      this.pnlServicing.Controls.Add((Control) this.lblServiceFee);
      this.pnlServicing.Controls.Add((Control) this.txtMinServicingFee);
      this.pnlServicing.Controls.Add((Control) this.label3);
      this.pnlServicing.Location = new Point(206, 1);
      this.pnlServicing.Name = "pnlServicing";
      this.pnlServicing.Size = new Size(211, 44);
      this.pnlServicing.TabIndex = 22;
      this.txtMaxBU.Location = new Point(102, 23);
      this.txtMaxBU.Name = "txtMaxBU";
      this.txtMaxBU.ReadOnly = true;
      this.txtMaxBU.Size = new Size(100, 20);
      this.txtMaxBU.TabIndex = 19;
      this.txtMaxBU.TextAlign = HorizontalAlignment.Right;
      this.lblServiceFee.AutoSize = true;
      this.lblServiceFee.Location = new Point(3, 26);
      this.lblServiceFee.Name = "lblServiceFee";
      this.lblServiceFee.Size = new Size(45, 13);
      this.lblServiceFee.TabIndex = 18;
      this.lblServiceFee.Text = "Max BU";
      this.txtMinServicingFee.Location = new Point(102, 1);
      this.txtMinServicingFee.Name = "txtMinServicingFee";
      this.txtMinServicingFee.ReadOnly = true;
      this.txtMinServicingFee.Size = new Size(100, 20);
      this.txtMinServicingFee.TabIndex = 17;
      this.txtMinServicingFee.TextAlign = HorizontalAlignment.Right;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(3, 4);
      this.label3.Name = "label3";
      this.label3.Size = new Size(92, 13);
      this.label3.TabIndex = 16;
      this.label3.Text = "Min Servicing Fee";
      this.txtGuaranteeFee.Location = new Point(101, 25);
      this.txtGuaranteeFee.Name = "txtGuaranteeFee";
      this.txtGuaranteeFee.ReadOnly = true;
      this.txtGuaranteeFee.Size = new Size(100, 20);
      this.txtGuaranteeFee.TabIndex = 21;
      this.txtGuaranteeFee.TextAlign = HorizontalAlignment.Right;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(3, 28);
      this.label4.Name = "label4";
      this.label4.Size = new Size(71, 13);
      this.label4.TabIndex = 20;
      this.label4.Text = "Guaranty Fee";
      this.txtCoupon.Location = new Point(101, 3);
      this.txtCoupon.Name = "txtCoupon";
      this.txtCoupon.ReadOnly = true;
      this.txtCoupon.Size = new Size(100, 20);
      this.txtCoupon.TabIndex = 19;
      this.txtCoupon.TextAlign = HorizontalAlignment.Right;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(3, 6);
      this.label1.Name = "label1";
      this.label1.Size = new Size(44, 13);
      this.label1.TabIndex = 17;
      this.label1.Text = "Coupon";
      this.gvAdvancedPriceItem.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SortMethod = GVSortMethod.Numeric;
      gvColumn1.Text = "Note Rate";
      gvColumn1.Width = 74;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column7";
      gvColumn2.Text = "Servicing Fee";
      gvColumn2.Width = 85;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column8";
      gvColumn3.Text = "Guaranty Fee";
      gvColumn3.Width = 85;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column6";
      gvColumn4.Text = "BU/BD bps";
      gvColumn4.Width = 100;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column2";
      gvColumn5.SortMethod = GVSortMethod.Numeric;
      gvColumn5.Text = "BU Execution";
      gvColumn5.Width = 85;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column3";
      gvColumn6.SortMethod = GVSortMethod.Numeric;
      gvColumn6.Text = "BD Execution";
      gvColumn6.Width = 85;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column4";
      gvColumn7.Text = "Servicing";
      gvColumn7.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn7.Width = 100;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Column9";
      gvColumn8.SortMethod = GVSortMethod.Numeric;
      gvColumn8.Text = "CPA";
      gvColumn8.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn8.Width = 70;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "Column5";
      gvColumn9.SortMethod = GVSortMethod.Numeric;
      gvColumn9.Text = "Total Price";
      gvColumn9.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn9.Width = 70;
      this.gvAdvancedPriceItem.Columns.AddRange(new GVColumn[9]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8,
        gvColumn9
      });
      this.gvAdvancedPriceItem.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvAdvancedPriceItem.Location = new Point(1, 79);
      this.gvAdvancedPriceItem.Name = "gvAdvancedPriceItem";
      this.gvAdvancedPriceItem.Size = new Size(637, 200);
      this.gvAdvancedPriceItem.TabIndex = 17;
      this.gvAdvancedPriceItem.SelectedIndexChanged += new EventHandler(this.gvAdvancedPriceItem_SelectedIndexChanged);
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.siBtnDelete);
      this.flowLayoutPanel1.Controls.Add((Control) this.siBtnAdd);
      this.flowLayoutPanel1.Controls.Add((Control) this.flPnlLeft);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(361, 2);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(276, 22);
      this.flowLayoutPanel1.TabIndex = 0;
      this.flPnlLeft.Dock = DockStyle.Fill;
      this.flPnlLeft.FlowDirection = FlowDirection.RightToLeft;
      this.flPnlLeft.Location = new Point(32, 0);
      this.flPnlLeft.Margin = new Padding(0);
      this.flPnlLeft.Name = "flPnlLeft";
      this.flPnlLeft.Size = new Size(200, 22);
      this.flPnlLeft.TabIndex = 2;
      this.process1.StartInfo.Domain = "";
      this.process1.StartInfo.LoadUserProfile = false;
      this.process1.StartInfo.Password = (SecureString) null;
      this.process1.StartInfo.StandardErrorEncoding = (Encoding) null;
      this.process1.StartInfo.StandardOutputEncoding = (Encoding) null;
      this.process1.StartInfo.UserName = "";
      this.process1.SynchronizingObject = (ISynchronizeInvoke) this;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupContainer1);
      this.Name = nameof (MBSPoolAdvancedPricingEditor);
      this.Size = new Size(638, 292);
      this.Resize += new EventHandler(this.TradeAdvancedPricingEditor_Resize);
      ((ISupportInitialize) this.siBtnDelete).EndInit();
      ((ISupportInitialize) this.siBtnAdd).EndInit();
      this.groupContainer1.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.pnlWeightedAvgPrice.ResumeLayout(false);
      this.pnlWeightedAvgPrice.PerformLayout();
      this.pnlServicing.ResumeLayout(false);
      this.pnlServicing.PerformLayout();
      this.flowLayoutPanel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
