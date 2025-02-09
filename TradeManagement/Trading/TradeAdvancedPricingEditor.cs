// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradeAdvancedPricingEditor
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class TradeAdvancedPricingEditor : UserControl
  {
    private TradeAdvancedPricingInfo pricingInfo;
    private bool modified;
    private bool readOnly;
    private Decimal coupon;
    private Decimal securityPrice;
    private IContainer components;
    private GroupContainer groupContainer1;
    private FlowLayoutPanel flowLayoutPanel1;
    private StandardIconButton siBtnDelete;
    private StandardIconButton siBtnAdd;
    private FlowLayoutPanel flPnlLeft;
    private Panel panel1;
    private TextBox txtCoupon;
    private Label label1;
    private GradientPanel gradientPanel1;
    private Label label2;
    private GridView gvAdvancedPriceItem;
    private TextBox txtNegotiatedIncentive;
    private TextBox txtEarlyDeliveryCredit;
    private TextBox txtServiceFee;
    private TextBox txtGuaranteeFee;
    private Label label7;
    private Label lblServiceFee;
    private Label label5;
    private Label label4;
    private TextBox txtSecurityPrice;
    private Label label3;
    private ToolTip toolTip1;

    public TradeAdvancedPricingEditor()
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
      this.pricingInfo.GuaranteeFee = Utils.ParseDecimal((object) this.txtGuaranteeFee.Text, 0M);
      this.pricingInfo.ServiceFee = Utils.ParseDecimal((object) this.txtServiceFee.Text, 0M);
      this.pricingInfo.EarlyDeliveryCredit = Utils.ParseDecimal((object) this.txtEarlyDeliveryCredit.Text, 0M);
      this.pricingInfo.NegotiatedIncentive = Utils.ParseDecimal((object) this.txtNegotiatedIncentive.Text, 0M);
      this.pricingInfo.PricingItems.Clear();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvAdvancedPriceItem.Items)
        this.pricingInfo.PricingItems.Add(gvItem.Tag as TradeAdvancedPricingItem);
      this.modified = false;
    }

    public void ClearData()
    {
      this.txtGuaranteeFee.Text = "";
      this.txtServiceFee.Text = "";
      this.txtEarlyDeliveryCredit.Text = "";
      this.txtNegotiatedIncentive.Text = "";
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

    public bool ValidatePricing()
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
          int num = (int) Utils.Dialog((IWin32Window) this, text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
        this.txtCoupon.ReadOnly = this.txtGuaranteeFee.ReadOnly = this.txtServiceFee.ReadOnly = this.txtSecurityPrice.ReadOnly = this.txtEarlyDeliveryCredit.ReadOnly = this.txtNegotiatedIncentive.ReadOnly = this.readOnly;
      }
    }

    [Browsable(false)]
    public TradeAdvancedPricingInfo PricingInfo
    {
      get => this.pricingInfo;
      set
      {
        this.pricingInfo = value;
        if (value == null)
          return;
        this.loadPricingInfo();
      }
    }

    public Decimal SecurityTradeCoupon
    {
      set
      {
        this.coupon = value;
        this.txtCoupon.Text = this.coupon.ToString("0.00000");
      }
    }

    public Decimal SecurityTradeSecurityPrice
    {
      set
      {
        this.securityPrice = value;
        this.txtSecurityPrice.Text = this.securityPrice.ToString("0.0000000");
        this.loadPricingInfo();
        this.refreshPricingItems();
      }
    }

    public bool DataModified => this.modified && !this.readOnly;

    private void loadPricingInfo()
    {
      this.txtGuaranteeFee.Text = this.pricingInfo.GuaranteeFee.ToString("0.00000");
      this.txtServiceFee.Text = this.pricingInfo.ServiceFee.ToString("0.00000");
      this.txtEarlyDeliveryCredit.Text = this.pricingInfo.EarlyDeliveryCredit.ToString("0.00000");
      this.txtNegotiatedIncentive.Text = this.pricingInfo.NegotiatedIncentive.ToString("0.00000");
      this.gvAdvancedPriceItem.Items.Clear();
      if (this.pricingInfo.PricingItems != null)
      {
        foreach (TradeAdvancedPricingItem pricingItem in this.pricingInfo.PricingItems)
          this.gvAdvancedPriceItem.Items.Add(this.createPricingGVItem(pricingItem));
      }
      this.gvAdvancedPriceItem.ReSort();
      this.modified = false;
    }

    private GVItem createPricingGVItem(TradeAdvancedPricingItem pricingItem)
    {
      GVItem pricingGvItem = new GVItem();
      pricingGvItem.Text = pricingItem.NoteRate.ToString("0.000");
      pricingGvItem.SubItems.Add((object) pricingItem.BuyUp.ToString("0.00000"));
      pricingGvItem.SubItems.Add((object) pricingItem.BuyDown.ToString("0.00000"));
      pricingGvItem.SubItems.Add((object) pricingItem.GNMAIIExcess.ToString("0.00000"));
      pricingGvItem.SubItems.Add((object) pricingItem.MandAdj.ToString("0.00000"));
      Decimal totalPrice = TradeAdvancedPricingInfo.CalculateTotalPrice(pricingItem, Utils.ParseDecimal((object) this.txtSecurityPrice.Text, 0M), Utils.ParseDecimal((object) this.txtEarlyDeliveryCredit.Text, 0M), Utils.ParseDecimal((object) this.txtNegotiatedIncentive.Text, 0M));
      if (Session.SessionObjects.Use10DecimalDigitInLockRequestSecondaryTradeAreas)
        pricingGvItem.SubItems.Add((object) totalPrice.ToString("0.0000000000"));
      else
        pricingGvItem.SubItems.Add((object) totalPrice.ToString("0.000"));
      pricingGvItem.Tag = (object) new TradeAdvancedPricingItem(pricingItem);
      return pricingGvItem;
    }

    private void TradeAdvancedPricingEditor_Resize(object sender, EventArgs e)
    {
      this.siBtnAdd.Left = Math.Max(0, this.ClientSize.Width - this.siBtnAdd.Width);
      this.siBtnDelete.Left = this.siBtnAdd.Left;
      this.panel1.Height = this.ClientSize.Height - (this.lblServiceFee.Location.Y + this.lblServiceFee.Height + 20);
      this.panel1.Width = Math.Max(0, this.ClientSize.Width - 10);
    }

    private void siBtnAdd_Click(object sender, EventArgs e)
    {
      TradeAdvancedPricingItemDialog pricingItemDialog = new TradeAdvancedPricingItemDialog();
      if (pricingItemDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      this.gvAdvancedPriceItem.Items.Add(this.createPricingGVItem(pricingItemDialog.PricingItem));
      this.gvAdvancedPriceItem.ReSort();
      if (pricingItemDialog.IsCreatingAnother)
        this.siBtnAdd_Click((object) null, (EventArgs) null);
      this.modified = true;
    }

    private void txtGuaranteeFee_Validating(object sender, CancelEventArgs e)
    {
      if (this.txtGuaranteeFee.Text == string.Empty)
        return;
      if (!Utils.IsDecimal((object) this.txtGuaranteeFee.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Invalid data for Guarantee Fee.");
        e.Cancel = true;
      }
      else
      {
        Decimal num1 = Utils.ParseDecimal((object) this.txtGuaranteeFee.Text);
        if (num1 > 0M || num1 <= -100M)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "Guaranty Fee must between 0 and -100.");
          e.Cancel = true;
        }
        else
          this.txtGuaranteeFee.Text = num1.ToString("0.00000");
      }
    }

    private void txtServiceFee_Validating(object sender, CancelEventArgs e)
    {
      if (this.txtServiceFee.Text == string.Empty)
        return;
      if (!Utils.IsDecimal((object) this.txtServiceFee.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Invalid data for Service Fee.");
        e.Cancel = true;
      }
      else
      {
        Decimal num1 = Utils.ParseDecimal((object) this.txtServiceFee.Text);
        if (num1 < 0M || num1 >= 100M)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "Service Fee must between 0 and 100.");
          e.Cancel = true;
        }
        else
          this.txtServiceFee.Text = num1.ToString("0.00000");
      }
    }

    private void txtEarlyDeliveryCredit_Validating(object sender, CancelEventArgs e)
    {
      if (this.txtEarlyDeliveryCredit.Text == string.Empty)
        return;
      if (!Utils.IsDecimal((object) this.txtEarlyDeliveryCredit.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Invalid data for Early Delivery Credit.");
        e.Cancel = true;
      }
      else
      {
        Decimal num1 = Utils.ParseDecimal((object) this.txtEarlyDeliveryCredit.Text);
        if (num1 < 0M || num1 >= 10M)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "Early Delivery Credit must between 0 and 10.");
          e.Cancel = true;
        }
        else
          this.txtEarlyDeliveryCredit.Text = num1.ToString("0.00000");
      }
    }

    private void txtNegotiatedIncentive_Validating(object sender, CancelEventArgs e)
    {
      if (this.txtNegotiatedIncentive.Text == string.Empty)
        return;
      if (!Utils.IsDecimal((object) this.txtNegotiatedIncentive.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Invalid data for Negotiated Incentive.");
        e.Cancel = true;
      }
      else
      {
        Decimal num1 = Utils.ParseDecimal((object) this.txtNegotiatedIncentive.Text);
        if (num1 < 0M || num1 >= 10M)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "Negotiated Incentive must between 0 and 10.");
          e.Cancel = true;
        }
        else
          this.txtNegotiatedIncentive.Text = num1.ToString("0.00000");
      }
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
    }

    private void txt_TextChanged(object sender, EventArgs e)
    {
      this.modified = true;
      if (sender != this.txtEarlyDeliveryCredit && sender != this.txtNegotiatedIncentive)
        return;
      this.refreshPricingItems();
    }

    private void refreshPricingItems()
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvAdvancedPriceItem.Items)
      {
        TradeAdvancedPricingItem tag = (TradeAdvancedPricingItem) gvItem.Tag;
        Decimal num = Utils.ParseDecimal((object) this.txtSecurityPrice.Text, 0M) + Utils.ParseDecimal((object) this.txtEarlyDeliveryCredit.Text, 0M) + Utils.ParseDecimal((object) this.txtNegotiatedIncentive.Text, 0M) + tag.BuyUp + tag.BuyDown + tag.GNMAIIExcess + tag.MandAdj;
        gvItem.SubItems[5].Text = !Session.SessionObjects.Use10DecimalDigitInLockRequestSecondaryTradeAreas ? num.ToString("0.000") : num.ToString("0.0000000000");
      }
    }

    private void gvAdvancedPriceItem_EditorClosing(object source, GVSubItemEditingEventArgs e)
    {
      int index = e.SubItem.Index;
      string text = e.EditorControl.Text;
      string str = e.EditorControl.Text;
      this.modified = true;
      if (e.EditorControl.Text != "")
      {
        switch (index)
        {
          case 1:
            int num1 = !TradeAdvancedPricingItemDialog.CheckDataFormat("BuyUp", text) ? 1 : 0;
            bool flag1 = !TradeAdvancedPricingItemDialog.CheckDataRange("BuyUp", text);
            str = TradeAdvancedPricingItemDialog.FormatData("BuyUp", text);
            int num2 = flag1 ? 1 : 0;
            if ((num1 | num2) != 0)
            {
              int num3 = (int) Utils.Dialog((IWin32Window) this, "Invalid Data for Buy Up", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              e.Cancel = true;
              e.EditorControl.Text = e.SubItem.Text;
              return;
            }
            break;
          case 2:
            int num4 = !TradeAdvancedPricingItemDialog.CheckDataFormat("BuyDown", text) ? 1 : 0;
            bool flag2 = !TradeAdvancedPricingItemDialog.CheckDataRange("BuyDown", text);
            str = TradeAdvancedPricingItemDialog.FormatData("BuyDown", text);
            int num5 = flag2 ? 1 : 0;
            if ((num4 | num5) != 0)
            {
              int num6 = (int) Utils.Dialog((IWin32Window) this, "Invalid Data for Buy Down", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              e.Cancel = true;
              e.EditorControl.Text = e.SubItem.Text;
              return;
            }
            break;
          case 3:
            int num7 = !TradeAdvancedPricingItemDialog.CheckDataFormat("GNMA", text) ? 1 : 0;
            bool flag3 = !TradeAdvancedPricingItemDialog.CheckDataRange("GNMA", text);
            str = TradeAdvancedPricingItemDialog.FormatData("GNMA", text);
            int num8 = flag3 ? 1 : 0;
            if ((num7 | num8) != 0)
            {
              int num9 = (int) Utils.Dialog((IWin32Window) this, "Invalid Data for GNMA II Excess", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              e.Cancel = true;
              e.EditorControl.Text = e.SubItem.Text;
              return;
            }
            break;
          case 4:
            int num10 = !TradeAdvancedPricingItemDialog.CheckDataFormat("MandAdj", text) ? 1 : 0;
            bool flag4 = !TradeAdvancedPricingItemDialog.CheckDataRange("MandAdj", text);
            str = TradeAdvancedPricingItemDialog.FormatData("MandAdj", text);
            int num11 = flag4 ? 1 : 0;
            if ((num10 | num11) != 0)
            {
              int num12 = (int) Utils.Dialog((IWin32Window) this, "Invalid Data for Mand. Adj.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              e.Cancel = true;
              e.EditorControl.Text = e.SubItem.Text;
              return;
            }
            break;
        }
        e.EditorControl.Text = str;
      }
      TradeAdvancedPricingItem tag = (TradeAdvancedPricingItem) e.SubItem.Parent.Tag;
      switch (e.SubItem.Index)
      {
        case 1:
          tag.BuyUp = Utils.ParseDecimal((object) e.EditorControl.Text, 0M);
          break;
        case 2:
          tag.BuyDown = Utils.ParseDecimal((object) e.EditorControl.Text, 0M);
          break;
        case 3:
          tag.GNMAIIExcess = Utils.ParseDecimal((object) e.EditorControl.Text, 0M);
          break;
        case 4:
          tag.MandAdj = Utils.ParseDecimal((object) e.EditorControl.Text, 0M);
          break;
      }
      e.SubItem.Parent.Tag = (object) tag;
      Decimal num = Utils.ParseDecimal((object) this.txtSecurityPrice.Text, 0M) + Utils.ParseDecimal((object) this.txtEarlyDeliveryCredit.Text, 0M) + Utils.ParseDecimal((object) this.txtNegotiatedIncentive.Text, 0M) + tag.BuyUp + tag.BuyDown + tag.GNMAIIExcess + tag.MandAdj;
      if (Session.SessionObjects.Use10DecimalDigitInLockRequestSecondaryTradeAreas)
        e.SubItem.Parent.SubItems[5].Text = num.ToString("0.0000000000");
      else
        e.SubItem.Parent.SubItems[5].Text = num.ToString("0.000");
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
      this.groupContainer1 = new GroupContainer();
      this.txtNegotiatedIncentive = new TextBox();
      this.txtEarlyDeliveryCredit = new TextBox();
      this.txtServiceFee = new TextBox();
      this.txtGuaranteeFee = new TextBox();
      this.label7 = new Label();
      this.lblServiceFee = new Label();
      this.label5 = new Label();
      this.label4 = new Label();
      this.txtSecurityPrice = new TextBox();
      this.label3 = new Label();
      this.panel1 = new Panel();
      this.gvAdvancedPriceItem = new GridView();
      this.gradientPanel1 = new GradientPanel();
      this.label2 = new Label();
      this.txtCoupon = new TextBox();
      this.label1 = new Label();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.siBtnDelete = new StandardIconButton();
      this.siBtnAdd = new StandardIconButton();
      this.flPnlLeft = new FlowLayoutPanel();
      this.toolTip1 = new ToolTip(this.components);
      this.groupContainer1.SuspendLayout();
      this.panel1.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      ((ISupportInitialize) this.siBtnDelete).BeginInit();
      ((ISupportInitialize) this.siBtnAdd).BeginInit();
      this.SuspendLayout();
      this.groupContainer1.Controls.Add((Control) this.txtNegotiatedIncentive);
      this.groupContainer1.Controls.Add((Control) this.txtEarlyDeliveryCredit);
      this.groupContainer1.Controls.Add((Control) this.txtServiceFee);
      this.groupContainer1.Controls.Add((Control) this.txtGuaranteeFee);
      this.groupContainer1.Controls.Add((Control) this.label7);
      this.groupContainer1.Controls.Add((Control) this.lblServiceFee);
      this.groupContainer1.Controls.Add((Control) this.label5);
      this.groupContainer1.Controls.Add((Control) this.label4);
      this.groupContainer1.Controls.Add((Control) this.txtSecurityPrice);
      this.groupContainer1.Controls.Add((Control) this.label3);
      this.groupContainer1.Controls.Add((Control) this.panel1);
      this.groupContainer1.Controls.Add((Control) this.txtCoupon);
      this.groupContainer1.Controls.Add((Control) this.label1);
      this.groupContainer1.Controls.Add((Control) this.flowLayoutPanel1);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(448, 292);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "Detailed Pricing Grid";
      this.txtNegotiatedIncentive.Location = new Point(335, 77);
      this.txtNegotiatedIncentive.Name = "txtNegotiatedIncentive";
      this.txtNegotiatedIncentive.Size = new Size(100, 20);
      this.txtNegotiatedIncentive.TabIndex = 13;
      this.txtNegotiatedIncentive.TextChanged += new EventHandler(this.txt_TextChanged);
      this.txtNegotiatedIncentive.Validating += new CancelEventHandler(this.txtNegotiatedIncentive_Validating);
      this.txtEarlyDeliveryCredit.Location = new Point(335, 54);
      this.txtEarlyDeliveryCredit.Name = "txtEarlyDeliveryCredit";
      this.txtEarlyDeliveryCredit.Size = new Size(100, 20);
      this.txtEarlyDeliveryCredit.TabIndex = 12;
      this.txtEarlyDeliveryCredit.TextChanged += new EventHandler(this.txt_TextChanged);
      this.txtEarlyDeliveryCredit.Validating += new CancelEventHandler(this.txtEarlyDeliveryCredit_Validating);
      this.txtServiceFee.Location = new Point(92, 77);
      this.txtServiceFee.Name = "txtServiceFee";
      this.txtServiceFee.Size = new Size(100, 20);
      this.txtServiceFee.TabIndex = 11;
      this.txtServiceFee.TextChanged += new EventHandler(this.txt_TextChanged);
      this.txtServiceFee.Validating += new CancelEventHandler(this.txtServiceFee_Validating);
      this.txtGuaranteeFee.Location = new Point(92, 54);
      this.txtGuaranteeFee.Name = "txtGuaranteeFee";
      this.txtGuaranteeFee.Size = new Size(100, 20);
      this.txtGuaranteeFee.TabIndex = 10;
      this.txtGuaranteeFee.TextChanged += new EventHandler(this.txt_TextChanged);
      this.txtGuaranteeFee.Validating += new CancelEventHandler(this.txtGuaranteeFee_Validating);
      this.label7.AutoSize = true;
      this.label7.Location = new Point(221, 80);
      this.label7.Name = "label7";
      this.label7.Size = new Size(113, 13);
      this.label7.TabIndex = 9;
      this.label7.Text = "Negotiated Incentive *";
      this.lblServiceFee.AutoSize = true;
      this.lblServiceFee.Location = new Point(9, 80);
      this.lblServiceFee.Name = "lblServiceFee";
      this.lblServiceFee.Size = new Size(64, 13);
      this.lblServiceFee.TabIndex = 8;
      this.lblServiceFee.Text = "Service Fee";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(221, 57);
      this.label5.Name = "label5";
      this.label5.Size = new Size(108, 13);
      this.label5.TabIndex = 7;
      this.label5.Text = "Early Delivery Credit *";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(9, 57);
      this.label4.Name = "label4";
      this.label4.Size = new Size(78, 13);
      this.label4.TabIndex = 6;
      this.label4.Text = "Guaranty Fee";
      this.txtSecurityPrice.Location = new Point(335, 32);
      this.txtSecurityPrice.Name = "txtSecurityPrice";
      this.txtSecurityPrice.ReadOnly = true;
      this.txtSecurityPrice.Size = new Size(100, 20);
      this.txtSecurityPrice.TabIndex = 5;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(221, 35);
      this.label3.Name = "label3";
      this.label3.Size = new Size(79, 13);
      this.label3.TabIndex = 4;
      this.label3.Text = "Security Price *";
      this.panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panel1.Controls.Add((Control) this.gvAdvancedPriceItem);
      this.panel1.Controls.Add((Control) this.gradientPanel1);
      this.panel1.Location = new Point(4, 103);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(440, 174);
      this.panel1.TabIndex = 3;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SortMethod = GVSortMethod.Numeric;
      gvColumn1.Text = "Note Rate";
      gvColumn1.Width = 74;
      gvColumn2.ActivatedEditorType = GVActivatedEditorType.TextBox;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.SortMethod = GVSortMethod.None;
      gvColumn2.Text = "Buy Up";
      gvColumn2.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn2.Width = 65;
      gvColumn3.ActivatedEditorType = GVActivatedEditorType.TextBox;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.SortMethod = GVSortMethod.None;
      gvColumn3.Text = "Buy Down";
      gvColumn3.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn3.Width = 65;
      gvColumn4.ActivatedEditorType = GVActivatedEditorType.TextBox;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.SortMethod = GVSortMethod.None;
      gvColumn4.Text = "GNMA II Excess";
      gvColumn4.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn4.Width = 94;
      gvColumn5.ActivatedEditorType = GVActivatedEditorType.TextBox;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.SortMethod = GVSortMethod.None;
      gvColumn5.Text = "Mandatory Adjuster";
      gvColumn5.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn5.Width = 106;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column6";
      gvColumn6.SortMethod = GVSortMethod.None;
      gvColumn6.Text = "Total Price";
      gvColumn6.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn6.Width = 70;
      this.gvAdvancedPriceItem.Columns.AddRange(new GVColumn[6]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6
      });
      this.gvAdvancedPriceItem.Dock = DockStyle.Fill;
      this.gvAdvancedPriceItem.Location = new Point(0, 0);
      this.gvAdvancedPriceItem.Name = "gvAdvancedPriceItem";
      this.gvAdvancedPriceItem.Size = new Size(440, 149);
      this.gvAdvancedPriceItem.TabIndex = 1;
      this.gvAdvancedPriceItem.SelectedIndexChanged += new EventHandler(this.gvAdvancedPriceItem_SelectedIndexChanged);
      this.gvAdvancedPriceItem.EditorClosing += new GVSubItemEditingEventHandler(this.gvAdvancedPriceItem_EditorClosing);
      this.gradientPanel1.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gradientPanel1.Controls.Add((Control) this.label2);
      this.gradientPanel1.Dock = DockStyle.Bottom;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Location = new Point(0, 149);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(440, 25);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.TableFooter;
      this.gradientPanel1.TabIndex = 0;
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.Location = new Point(4, 7);
      this.label2.Name = "label2";
      this.label2.Size = new Size(133, 13);
      this.label2.TabIndex = 0;
      this.label2.Text = "* Contributes to Total Price";
      this.txtCoupon.Location = new Point(92, 32);
      this.txtCoupon.Name = "txtCoupon";
      this.txtCoupon.ReadOnly = true;
      this.txtCoupon.Size = new Size(100, 20);
      this.txtCoupon.TabIndex = 2;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(9, 35);
      this.label1.Name = "label1";
      this.label1.Size = new Size(44, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Coupon";
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.siBtnDelete);
      this.flowLayoutPanel1.Controls.Add((Control) this.siBtnAdd);
      this.flowLayoutPanel1.Controls.Add((Control) this.flPnlLeft);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(171, 2);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(276, 22);
      this.flowLayoutPanel1.TabIndex = 0;
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
      this.flPnlLeft.Dock = DockStyle.Fill;
      this.flPnlLeft.FlowDirection = FlowDirection.RightToLeft;
      this.flPnlLeft.Location = new Point(32, 0);
      this.flPnlLeft.Margin = new Padding(0);
      this.flPnlLeft.Name = "flPnlLeft";
      this.flPnlLeft.Size = new Size(200, 22);
      this.flPnlLeft.TabIndex = 2;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupContainer1);
      this.Name = nameof (TradeAdvancedPricingEditor);
      this.Size = new Size(448, 292);
      this.Resize += new EventHandler(this.TradeAdvancedPricingEditor_Resize);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.flowLayoutPanel1.ResumeLayout(false);
      ((ISupportInitialize) this.siBtnDelete).EndInit();
      ((ISupportInitialize) this.siBtnAdd).EndInit();
      this.ResumeLayout(false);
    }
  }
}
