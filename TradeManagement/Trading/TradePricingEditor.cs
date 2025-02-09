// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradePricingEditor
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class TradePricingEditor : UserControl
  {
    private TradePricingItems pricingItems;
    private bool modified;
    private bool readOnly;
    private string decimalFormat = "0.000";
    private IContainer components;
    private GroupContainer groupContainer1;
    private GridView gvPricing;
    private StandardIconButton btnRemove;
    private StandardIconButton btnAdd;
    private FlowLayoutPanel flowLayoutPanel1;
    private ToolTip toolTip1;
    private FlowLayoutPanel flowLOPnlLeft;

    public TradePricingEditor()
    {
      this.InitializeComponent();
      this.gvPricing.Columns[2].Width = 0;
    }

    [Browsable(false)]
    public TradePricingItems PricingItems
    {
      get => this.pricingItems;
      set
      {
        this.pricingItems = value;
        if (value == null)
          return;
        this.loadPricingItems();
      }
    }

    public bool DataModified => this.modified && !this.readOnly;

    public bool ReadOnly
    {
      get => this.readOnly;
      set
      {
        if (this.readOnly == value)
          return;
        this.readOnly = value;
        this.gvPricing.SelectedItems.Clear();
        this.btnAdd.Enabled = !this.readOnly;
        this.btnRemove.Enabled = false;
      }
    }

    public bool ValidatePricing()
    {
      if (this.gvPricing.Items.Count < 2)
        return true;
      TradePricingItem tag1 = (TradePricingItem) this.gvPricing.Items[0].Tag;
      TradePricingItem tag2 = (TradePricingItem) this.gvPricing.Items[this.gvPricing.Items.Count - 1].Tag;
      TradePricingItem tradePricingItem = tag1;
      for (int nItemIndex = 1; nItemIndex < this.gvPricing.Items.Count; ++nItemIndex)
      {
        TradePricingItem tag3 = (TradePricingItem) this.gvPricing.Items[nItemIndex].Tag;
        if (tag3.Rate - tradePricingItem.Rate > 0.125M)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "You must specify pricing for every 1/8 point increment between " + tag1.Rate.ToString(this.decimalFormat) + " and " + tag2.Rate.ToString(this.decimalFormat), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return false;
        }
        tradePricingItem = tag3;
      }
      return true;
    }

    private void loadPricingItems()
    {
      this.gvPricing.Items.Clear();
      foreach (TradePricingItem pricingItem in this.pricingItems)
        this.gvPricing.Items.Add(this.createPricingGVItem(pricingItem));
      this.modified = false;
    }

    private GVItem createPricingGVItem(TradePricingItem pricingItem)
    {
      GVItem pricingGvItem = new GVItem();
      pricingGvItem.Text = pricingItem.Rate.ToString("0.000") + "%";
      pricingGvItem.SubItems.Add((object) pricingItem.Price.ToString(this.decimalFormat)).Tag = (object) "price";
      pricingGvItem.SubItems.Add((object) pricingItem.ServiceFee.ToString(this.decimalFormat)).Tag = (object) "servicefee";
      pricingGvItem.Tag = (object) new TradePricingItem(pricingItem);
      return pricingGvItem;
    }

    public void CommitChanges()
    {
      this.gvPricing.StopEditing();
      if (this.pricingItems != null)
      {
        this.pricingItems.Clear();
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvPricing.Items)
          this.pricingItems.Add(gvItem.Tag as TradePricingItem);
      }
      this.modified = false;
    }

    private void btnRemove_Click(object sender, EventArgs e)
    {
      this.gvPricing.CancelEditing();
      if (this.gvPricing.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select the items from the list to be removed.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        while (this.gvPricing.SelectedItems.Count > 0)
          this.gvPricing.Items.Remove(this.gvPricing.SelectedItems[0]);
        this.modified = true;
      }
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      AddPricingItemDialog pricingItemDialog = new AddPricingItemDialog(this.gvPricing.Columns[2].Width > 0);
      switch (pricingItemDialog.ShowDialog((IWin32Window) this))
      {
        case DialogResult.OK:
          this.addPricingItem(pricingItemDialog.PricingItem);
          this.modified = true;
          break;
        case DialogResult.Retry:
          this.addPricingItem(pricingItemDialog.PricingItem);
          this.modified = true;
          this.btnAdd_Click((object) null, (EventArgs) null);
          break;
      }
    }

    private void addPricingItem(TradePricingItem pricingItem)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvPricing.Items)
      {
        if (((TradePricingItem) gvItem.Tag).Rate == pricingItem.Rate)
        {
          this.gvPricing.Items.Remove(gvItem);
          break;
        }
      }
      for (int index = 0; index < this.gvPricing.Items.Count; ++index)
      {
        TradePricingItem tag = (TradePricingItem) this.gvPricing.Items[index].Tag;
        if (pricingItem.Rate < tag.Rate)
        {
          this.gvPricing.Items.Insert(index, this.createPricingGVItem(pricingItem));
          return;
        }
      }
      this.gvPricing.Items.Add(this.createPricingGVItem(pricingItem));
    }

    private void TradePricingEditor_Resize(object sender, EventArgs e)
    {
      this.btnAdd.Left = Math.Max(0, this.ClientSize.Width - this.btnAdd.Width);
      this.btnRemove.Left = this.btnAdd.Left;
      this.gvPricing.Height = this.ClientSize.Height;
      this.gvPricing.Width = Math.Max(0, this.ClientSize.Width - this.btnAdd.Width - 10);
    }

    private void gvPricing_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnRemove.Enabled = this.gvPricing.SelectedItems.Count > 0 && !this.readOnly;
    }

    private void gvPricing_EditorOpening(object source, GVSubItemEditingEventArgs e)
    {
      e.Cancel = this.readOnly;
      TextBoxFormatter.Attach(e.EditorControl as TextBox, TextBoxContentRule.Decimal, this.decimalFormat);
    }

    public void AddControlToHeader(Control c, bool toLeft)
    {
      if (toLeft)
      {
        if (this.flowLOPnlLeft.Controls.Count < 1)
        {
          this.flowLOPnlLeft.Controls.Add(c);
        }
        else
        {
          foreach (Control control in (ArrangedElementCollection) this.flowLOPnlLeft.Controls)
          {
            if (c.Name != control.Name)
            {
              this.flowLOPnlLeft.Controls.Add(c);
              break;
            }
          }
        }
      }
      else if (this.flowLayoutPanel1.Controls.Count < 1)
      {
        this.flowLayoutPanel1.Controls.Add(c);
      }
      else
      {
        foreach (Control control in (ArrangedElementCollection) this.flowLayoutPanel1.Controls)
        {
          if (c.Name != control.Name)
          {
            this.flowLayoutPanel1.Controls.Add(c);
            break;
          }
        }
      }
      c.BringToFront();
    }

    private void gvPricing_EditorClosing(object source, GVSubItemEditingEventArgs e)
    {
      TradePricingItem tag = (TradePricingItem) e.SubItem.Item.Tag;
      Decimal price = Utils.ParseDecimal((object) e.EditorControl.Text);
      if (Convert.ToString(e.SubItem.Tag) == "price")
      {
        if (price >= 1000M || price < 0M)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Price must be greater than or equal to 0 and less than 1000.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          price = tag.Price;
        }
        else if (price != tag.Price)
        {
          tag.Price = price;
          this.modified = true;
        }
        e.EditorControl.Text = price.ToString(this.decimalFormat);
      }
      if (!(Convert.ToString(e.SubItem.Tag) == "servicefee"))
        return;
      if (price >= 1000M || price < 0M)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Service Fee must be greater than or equal to 0 and less than 1000.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        price = tag.Price;
      }
      else if (price != tag.ServiceFee)
      {
        tag.ServiceFee = price;
        this.modified = true;
      }
      e.EditorControl.Text = price.ToString(this.decimalFormat);
    }

    public void ServiceFeeColumn(bool addremove)
    {
      if (addremove)
      {
        if (this.gvPricing.Columns.Count != 3)
          return;
        this.gvPricing.Columns[2].Width = 100;
      }
      else
      {
        if (this.gvPricing.Columns.Count != 3)
          return;
        this.gvPricing.Columns[2].Width = 0;
        if (this.pricingItems == null)
          return;
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvPricing.Items)
        {
          gvItem.SubItems[2].Value = (object) 0.ToString(this.decimalFormat);
          ((TradePricingItem) gvItem.Tag).ServiceFee = 0M;
        }
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
      this.groupContainer1 = new GroupContainer();
      this.gvPricing = new GridView();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.btnRemove = new StandardIconButton();
      this.btnAdd = new StandardIconButton();
      this.flowLOPnlLeft = new FlowLayoutPanel();
      this.toolTip1 = new ToolTip(this.components);
      this.groupContainer1.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      ((ISupportInitialize) this.btnRemove).BeginInit();
      ((ISupportInitialize) this.btnAdd).BeginInit();
      this.SuspendLayout();
      this.groupContainer1.Controls.Add((Control) this.gvPricing);
      this.groupContainer1.Controls.Add((Control) this.flowLayoutPanel1);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(305, 241);
      this.groupContainer1.TabIndex = 8;
      this.groupContainer1.Text = "Base Price";
      this.gvPricing.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SpringToFit = true;
      gvColumn1.Text = "Note Rate";
      gvColumn1.Width = 103;
      gvColumn2.ActivatedEditorType = GVActivatedEditorType.TextBox;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Tag = (object) "price";
      gvColumn2.Text = "Price";
      gvColumn2.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn2.Width = 100;
      gvColumn3.ActivatedEditorType = GVActivatedEditorType.TextBox;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Tag = (object) "servicefee";
      gvColumn3.Text = "Service Fee";
      gvColumn3.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn3.Width = 100;
      this.gvPricing.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gvPricing.Dock = DockStyle.Fill;
      this.gvPricing.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvPricing.Location = new Point(1, 26);
      this.gvPricing.Name = "gvPricing";
      this.gvPricing.Size = new Size(303, 214);
      this.gvPricing.SortOption = GVSortOption.None;
      this.gvPricing.TabIndex = 4;
      this.gvPricing.SelectedIndexChanged += new EventHandler(this.gvPricing_SelectedIndexChanged);
      this.gvPricing.EditorOpening += new GVSubItemEditingEventHandler(this.gvPricing_EditorOpening);
      this.gvPricing.EditorClosing += new GVSubItemEditingEventHandler(this.gvPricing_EditorClosing);
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnRemove);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnAdd);
      this.flowLayoutPanel1.Controls.Add((Control) this.flowLOPnlLeft);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(74, 2);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(226, 22);
      this.flowLayoutPanel1.TabIndex = 10;
      this.flowLayoutPanel1.WrapContents = false;
      this.btnRemove.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRemove.BackColor = Color.Transparent;
      this.btnRemove.Enabled = false;
      this.btnRemove.Location = new Point(210, 3);
      this.btnRemove.Margin = new Padding(3, 3, 0, 3);
      this.btnRemove.MouseDownImage = (Image) null;
      this.btnRemove.Name = "btnRemove";
      this.btnRemove.Size = new Size(16, 16);
      this.btnRemove.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnRemove.TabIndex = 9;
      this.btnRemove.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnRemove, "Remove Pricing");
      this.btnRemove.Click += new EventHandler(this.btnRemove_Click);
      this.btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAdd.BackColor = Color.Transparent;
      this.btnAdd.Location = new Point(188, 3);
      this.btnAdd.MouseDownImage = (Image) null;
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(16, 16);
      this.btnAdd.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAdd.TabIndex = 8;
      this.btnAdd.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnAdd, "Add Pricing");
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.flowLOPnlLeft.Dock = DockStyle.Fill;
      this.flowLOPnlLeft.FlowDirection = FlowDirection.RightToLeft;
      this.flowLOPnlLeft.Location = new Point(3, 0);
      this.flowLOPnlLeft.Margin = new Padding(0);
      this.flowLOPnlLeft.Name = "flowLOPnlLeft";
      this.flowLOPnlLeft.Size = new Size(182, 22);
      this.flowLOPnlLeft.TabIndex = 10;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupContainer1);
      this.Name = nameof (TradePricingEditor);
      this.Size = new Size(305, 241);
      this.Resize += new EventHandler(this.TradePricingEditor_Resize);
      this.groupContainer1.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      ((ISupportInitialize) this.btnRemove).EndInit();
      ((ISupportInitialize) this.btnAdd).EndInit();
      this.ResumeLayout(false);
    }
  }
}
