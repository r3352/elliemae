// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SRPTableEditor
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Trading;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class SRPTableEditor : UserControl
  {
    private SRPTable srpTable;
    private bool modified;
    private bool itemModified;
    private bool readOnly;
    private bool suspendEvents;
    private SRPTable.PricingItem currentItem;
    private Sessions.Session session;
    private IContainer components;
    private TextBox txtImpoundsPrice;
    private TextBox txtBasePrice;
    private TextBox txtMaxAmount;
    private TextBox txtMinAmount;
    private Label label6;
    private Label label5;
    private Label label4;
    private Label label3;
    private Label label2;
    private GridView gvStateAdjustments;
    private GridView gvPricing;
    private GroupContainer grpSRP;
    private StandardIconButton btnNew;
    private StandardIconButton btnDelete;
    private StandardIconButton btnDuplicate;
    private Panel panel1;
    private FlowLayoutPanel flpControls;
    private ToolTip toolTip1;
    private GroupContainer grpDetails;
    private FlowLayoutPanel flowLayoutPanel1;
    private StandardIconButton btnReset;
    private StandardIconButton btnAddUpdate;
    private CheckBox checkBoxSRPfromPPE;

    public event EventHandler DataChange;

    public bool SRPfromPPE
    {
      get => this.checkBoxSRPfromPPE.Checked;
      set => this.checkBoxSRPfromPPE.Checked = value;
    }

    public SRPTableEditor()
      : this(Session.DefaultInstance)
    {
    }

    public SRPTableEditor(Sessions.Session session)
    {
      this.session = session;
      this.InitializeComponent();
      TextBoxFormatter.Attach(this.txtMinAmount, TextBoxContentRule.NonNegativeInteger, "0");
      TextBoxFormatter.Attach(this.txtMaxAmount, TextBoxContentRule.NonNegativeInteger, "0");
      TextBoxFormatter.Attach(this.txtBasePrice, TextBoxContentRule.Decimal, "0.000");
      TextBoxFormatter.Attach(this.txtImpoundsPrice, TextBoxContentRule.Decimal, "0.000");
      this.loadGeographyList();
    }

    public void ShowPPEIndicator(TradeType tradeType, CorrespondentMasterDeliveryType deliveryType)
    {
      if (tradeType == TradeType.CorrespondentTrade && (deliveryType == CorrespondentMasterDeliveryType.AOT || deliveryType == CorrespondentMasterDeliveryType.Forwards || deliveryType == CorrespondentMasterDeliveryType.LiveTrade) && Utils.ParseBoolean(this.session.StartupInfo.TradeSettings[(object) "Trade.EnableTPOTradeManagement"]) && Utils.ParseBoolean(this.session.StartupInfo.TradeSettings[(object) "Trade.EPPSLoanProgEliPricing"]) && this.session.StartupInfo.ProductPricingPartner != null && (this.session.StartupInfo.ProductPricingPartner.PartnerID == "MPS" || this.session.StartupInfo.ProductPricingPartner.VendorPlatform.ToDescription() == "EPC2"))
      {
        this.checkBoxSRPfromPPE.Visible = true;
      }
      else
      {
        this.checkBoxSRPfromPPE.Checked = false;
        this.checkBoxSRPfromPPE.Visible = false;
      }
    }

    private void loadGeographyList()
    {
      this.gvStateAdjustments.Items.Clear();
      this.gvStateAdjustments.Items.Add(new GVItem("All States")
      {
        SubItems = {
          (object) ""
        },
        Tag = (object) "ALL"
      });
      foreach (string state in Utils.GetStates())
        this.gvStateAdjustments.Items.Add(new GVItem(Utils.GetFullStateName(state))
        {
          SubItems = {
            (object) ""
          },
          Tag = (object) state
        });
    }

    [Browsable(false)]
    public SRPTable SRPTable
    {
      get => this.srpTable;
      set
      {
        this.srpTable = value;
        if (value == null)
          return;
        this.loadSRPTableData();
      }
    }

    public bool DataModified => this.itemModified || this.modified;

    public bool ItemModified => this.itemModified;

    public bool ReadOnly
    {
      get => this.readOnly;
      set
      {
        if (this.readOnly == value)
          return;
        this.readOnly = value;
        this.setReadOnly();
      }
    }

    public void ApplySRPTableData(SRPTable data)
    {
      this.loadPricingData(data);
      this.onDataChange();
    }

    private void setReadOnly()
    {
      this.gvPricing.SelectedItems.Clear();
      this.btnNew.Enabled = !this.readOnly;
      this.btnDelete.Enabled = false;
      this.btnDuplicate.Enabled = false;
      this.txtMinAmount.ReadOnly = this.txtMaxAmount.ReadOnly = this.readOnly;
      this.txtBasePrice.ReadOnly = this.readOnly;
      this.txtImpoundsPrice.ReadOnly = this.readOnly;
      this.btnAddUpdate.Enabled = false;
      this.checkBoxSRPfromPPE.Enabled = !this.ReadOnly;
    }

    private void loadSRPTableData()
    {
      this.loadPricingData(this.srpTable);
      this.modified = false;
      this.setItemModified(false);
    }

    private void loadPricingData(SRPTable data)
    {
      this.gvPricing.Items.Clear();
      this.txtBasePrice.Text = string.Empty;
      this.txtImpoundsPrice.Text = string.Empty;
      this.txtMaxAmount.Text = string.Empty;
      this.txtMinAmount.Text = string.Empty;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvStateAdjustments.Items)
      {
        gvItem.SubItems[1].Text = "";
        gvItem.SubItems[2].Text = "";
      }
      foreach (SRPTable.PricingItem pricingItem in data.PricingItems)
        this.gvPricing.Items.Add(this.createPricingListItem(pricingItem));
    }

    private void loadAdjustmentData(SRPTable.PricingItem data)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvStateAdjustments.Items)
      {
        gvItem.SubItems[1].Text = "";
        gvItem.SubItems[2].Text = "";
      }
      foreach (SRPTable.StateAdjustment stateAdjustment in data.StateAdjustments)
      {
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvStateAdjustments.Items)
        {
          if (string.Concat(gvItem.Tag) == stateAdjustment.State)
          {
            if (stateAdjustment.Adjustment != 0M)
              gvItem.SubItems[1].Text = stateAdjustment.Adjustment.ToString("0.000");
            if (stateAdjustment.ImpoundAdjustment != 0M)
            {
              gvItem.SubItems[2].Text = stateAdjustment.ImpoundAdjustment.ToString("0.000");
              break;
            }
            break;
          }
        }
      }
    }

    private bool updatePricingItem(SRPTable.PricingItem priorItem, SRPTable.PricingItem newItem)
    {
      int nIndex = 0;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvPricing.Items)
      {
        SRPTable.PricingItem tag = (SRPTable.PricingItem) gvItem.Tag;
        if (priorItem == null || tag != priorItem)
        {
          if (!(newItem.LoanAmount.Maximum < tag.LoanAmount.Minimum))
          {
            if (newItem.LoanAmount.Minimum > tag.LoanAmount.Maximum)
            {
              ++nIndex;
            }
            else
            {
              int num = (int) Utils.Dialog((IWin32Window) this, "The specified range cannot be added because it overlaps an existing price range.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return false;
            }
          }
          else
            break;
        }
      }
      if (priorItem != null)
        this.removePricingItemFromList(priorItem);
      this.gvPricing.Items.Insert(nIndex, this.createPricingListItem(newItem));
      return true;
    }

    private void removePricingItemFromList(SRPTable.PricingItem pricingItem)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvPricing.Items)
      {
        if (gvItem.Tag == pricingItem)
        {
          this.gvPricing.Items.Remove(gvItem);
          break;
        }
      }
    }

    private GVItem createPricingListItem(SRPTable.PricingItem pricingItem)
    {
      Decimal num;
      string text;
      if (pricingItem.LoanAmount.Maximum == Decimal.MaxValue)
      {
        num = pricingItem.LoanAmount.Minimum;
        text = num.ToString("#,##0") + " and up";
      }
      else
      {
        num = pricingItem.LoanAmount.Minimum;
        string str1 = num.ToString("#,##0");
        num = pricingItem.LoanAmount.Maximum;
        string str2 = num.ToString("#,##0");
        text = str1 + " - " + str2;
      }
      GVItem pricingListItem = new GVItem(text);
      GVSubItemCollection subItems1 = pricingListItem.SubItems;
      num = pricingItem.BaseAdjustment;
      string str3 = num.ToString("0.000");
      subItems1.Add((object) str3);
      GVSubItemCollection subItems2 = pricingListItem.SubItems;
      num = pricingItem.ImpoundsAdjustment;
      string str4 = num.ToString("0.000");
      subItems2.Add((object) str4);
      pricingListItem.Tag = (object) new SRPTable.PricingItem(pricingItem);
      return pricingListItem;
    }

    private void btnAdd_Click(object sender, EventArgs e) => this.commitPricingItem();

    private void btnReset_Click(object sender, EventArgs e)
    {
      this.setCurrentItem(this.currentItem);
    }

    private SRPTable.PricingItem createPricingItem()
    {
      Decimal minValue = Utils.ParseDecimal((object) this.txtMinAmount.Text, true);
      Decimal maxValue = Utils.ParseDecimal((object) this.txtMaxAmount.Text, Decimal.MaxValue);
      Decimal baseAdjustment = Utils.ParseDecimal((object) this.txtBasePrice.Text, true);
      Decimal impoundsAdjustment = Utils.ParseDecimal((object) this.txtImpoundsPrice.Text, 0M);
      SRPTable.PricingItem pricingItem = this.currentItem != null ? new SRPTable.PricingItem(this.currentItem.Id, new Range<Decimal>(minValue, maxValue), baseAdjustment, impoundsAdjustment) : new SRPTable.PricingItem(new Range<Decimal>(minValue, maxValue), baseAdjustment, impoundsAdjustment);
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvStateAdjustments.Items)
      {
        GVItem item = gvItem;
        Guid id = Guid.Empty;
        if (this.currentItem != null)
          id = this.currentItem.StateAdjustments.Where<SRPTable.StateAdjustment>((Func<SRPTable.StateAdjustment, bool>) (t => t.State.Equals(item.Tag.ToString(), StringComparison.OrdinalIgnoreCase))).Select<SRPTable.StateAdjustment, Guid>((Func<SRPTable.StateAdjustment, Guid>) (t => t.Id)).FirstOrDefault<Guid>();
        if (id == Guid.Empty)
          pricingItem.StateAdjustments.Add(new SRPTable.StateAdjustment(item.Tag.ToString(), Utils.ParseDecimal((object) item.SubItems[1].Text), Utils.ParseDecimal((object) item.SubItems[2].Text)));
        else
          pricingItem.StateAdjustments.Add(new SRPTable.StateAdjustment(id, item.Tag.ToString(), Utils.ParseDecimal((object) item.SubItems[1].Text), Utils.ParseDecimal((object) item.SubItems[2].Text)));
      }
      return pricingItem;
    }

    private bool validatePricingFields()
    {
      if (this.txtMinAmount.Text == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "A minimum value for the price range must be specified.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      if (!(this.txtBasePrice.Text == ""))
        return true;
      int num1 = (int) Utils.Dialog((IWin32Window) this, "A base price for the range must be specified.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      return false;
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (this.gvPricing.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select the item(s) from the list to be deleted.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "Remove the selected item(s) from the pricing table?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
          return;
        for (int nItemIndex = this.gvPricing.Items.Count - 1; nItemIndex >= 0; --nItemIndex)
        {
          if (this.gvPricing.Items[nItemIndex].Selected)
            this.gvPricing.Items.RemoveAt(nItemIndex);
        }
        this.onDataChange();
        this.setCurrentItem((SRPTable.PricingItem) null);
      }
    }

    public void AddControlToHeader(Control c)
    {
      this.flpControls.Controls.Add(c);
      c.BringToFront();
    }

    public bool ValidateChanges() => this.queryCommitChanges();

    public void CommitChanges()
    {
      this.srpTable.PricingItems.Clear();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvPricing.Items)
        this.srpTable.PricingItems.Add(gvItem.Tag as SRPTable.PricingItem);
      this.modified = false;
    }

    private void btnDuplicate_Click(object sender, EventArgs e)
    {
      if (this.gvPricing.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select the line item to be duplicated from the list.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        SRPTable.PricingItem tag = (SRPTable.PricingItem) this.gvPricing.SelectedItems[0].Tag;
        this.gvPricing.SelectedItems.Clear();
        this.setCurrentItem(tag);
        this.txtMinAmount.Text = "";
        this.txtMaxAmount.Text = "";
        this.txtMinAmount.Focus();
        this.currentItem = (SRPTable.PricingItem) null;
        this.btnAddUpdate.Text = "&Add";
      }
    }

    private void onDataChange()
    {
      this.modified = true;
      if (this.DataChange == null)
        return;
      this.DataChange((object) this, EventArgs.Empty);
    }

    private void btnNew_Click(object sender, EventArgs e)
    {
      if (!this.queryCommitChanges())
        return;
      this.setCurrentItem((SRPTable.PricingItem) null);
    }

    private void clearAdjustmentTable()
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvStateAdjustments.Items)
      {
        gvItem.SubItems[1].Text = "";
        gvItem.SubItems[2].Text = "";
      }
    }

    private void lvwPricing_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!this.suspendEvents)
      {
        this.suspendEvents = true;
        int count = this.gvPricing.SelectedItems.Count;
        if (count > 1)
        {
          if (this.itemModified && this.queryCommitChanges())
            this.gvPricing.SelectedItems.Clear();
          this.setCurrentItem((SRPTable.PricingItem) null);
        }
        else if (count == 1)
        {
          SRPTable.PricingItem tag = (SRPTable.PricingItem) this.gvPricing.SelectedItems[0].Tag;
          if (tag != this.currentItem)
          {
            this.queryCommitChanges();
            this.setCurrentItem(tag);
          }
        }
        this.suspendEvents = false;
      }
      this.grpDetails.Enabled = this.gvPricing.SelectedItems.Count <= 1;
      this.btnDelete.Enabled = this.gvPricing.SelectedItems.Count > 0 && !this.readOnly;
      this.btnDuplicate.Enabled = this.gvPricing.SelectedItems.Count == 1 && !this.readOnly;
    }

    private void setCurrentItem(SRPTable.PricingItem pricingItem)
    {
      if (pricingItem != null)
      {
        this.txtMinAmount.Text = pricingItem.LoanAmount.Minimum.ToString("0");
        this.txtMaxAmount.Text = pricingItem.LoanAmount.Maximum == Decimal.MaxValue ? "" : pricingItem.LoanAmount.Maximum.ToString("0");
        this.txtBasePrice.Text = pricingItem.BaseAdjustment.ToString("0.000");
        this.txtImpoundsPrice.Text = pricingItem.ImpoundsAdjustment.ToString("0.000");
        this.loadAdjustmentData(pricingItem);
        this.btnAddUpdate.Text = "Upd&ate";
      }
      else
      {
        this.txtMinAmount.Text = "";
        this.txtMaxAmount.Text = "";
        this.txtBasePrice.Text = "";
        this.txtImpoundsPrice.Text = "";
        this.clearAdjustmentTable();
        this.btnAddUpdate.Text = "&Add";
      }
      this.currentItem = pricingItem;
      this.setItemModified(false);
      this.txtMinAmount.Focus();
    }

    private void setItemModified(bool modified)
    {
      this.itemModified = modified;
      this.btnAddUpdate.Enabled = modified;
      this.btnReset.Enabled = modified;
    }

    private void selectPricingItem(SRPTable.PricingItem pricingItem)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvPricing.Items)
      {
        if (gvItem.Tag == pricingItem)
        {
          gvItem.Selected = true;
          break;
        }
      }
    }

    private bool queryCommitChanges()
    {
      return !this.itemModified || Utils.Dialog((IWin32Window) this, "Save your changes to the current SRP pricing?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No || this.commitPricingItem();
    }

    private bool commitPricingItem()
    {
      this.txtMinAmount.Focus();
      if (!this.validatePricingFields())
        return false;
      try
      {
        bool flag = this.updatePricingItem(this.currentItem, this.createPricingItem());
        if (flag)
        {
          this.onDataChange();
          this.setCurrentItem((SRPTable.PricingItem) null);
        }
        return flag;
      }
      catch (ArgumentException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Unexpected error while adding pricing information: " + (object) ex, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
    }

    private void onFieldValueChanged(object sender, EventArgs e) => this.setItemModified(true);

    private void gvPricing_EditorOpening(object source, GVSubItemEditingEventArgs e)
    {
      e.Cancel = this.readOnly;
      TextBoxFormatter.Attach(e.EditorControl as TextBox, TextBoxContentRule.Decimal, "0.000");
    }

    private void gvPricing_EditorClosing(object source, GVSubItemEditingEventArgs e)
    {
      SRPTable.PricingItem tag = (SRPTable.PricingItem) e.SubItem.Item.Tag;
      Decimal num = Utils.ParseDecimal((object) e.EditorControl.Text.Trim(), 0M);
      e.EditorControl.Text = num.ToString("0.000");
      if (e.SubItem.Index == 1)
      {
        if (!(num != tag.BaseAdjustment))
          return;
        tag.BaseAdjustment = num;
        this.onDataChange();
      }
      else
      {
        if (!(num != tag.ImpoundsAdjustment))
          return;
        tag.ImpoundsAdjustment = num;
        this.onDataChange();
      }
    }

    private void gvStateAdjustments_EditorOpening(object source, GVSubItemEditingEventArgs e)
    {
      e.Cancel = this.readOnly;
      TextBoxFormatter.Attach(e.EditorControl as TextBox, TextBoxContentRule.Decimal, "0.000");
    }

    private void gvStateAdjustments_EditorClosing(object source, GVSubItemEditingEventArgs e)
    {
      string text = e.SubItem.Text;
      if (e.EditorControl.Text.Trim() == "")
      {
        e.EditorControl.Text = "";
      }
      else
      {
        Decimal num = Utils.ParseDecimal((object) e.EditorControl.Text.Trim(), 0M);
        e.EditorControl.Text = !(num == 0M) ? num.ToString("0.000") : "";
      }
      if (!(e.EditorControl.Text != text))
        return;
      this.setItemModified(true);
    }

    private void checkBoxSRPfromPPE_CheckedChanged(object sender, EventArgs e)
    {
      this.modified = true;
    }

    public void EnableSRPfromPPE()
    {
      this.checkBoxSRPfromPPE.Visible = true;
      this.checkBoxSRPfromPPE.Enabled = true;
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
      this.toolTip1 = new ToolTip(this.components);
      this.btnReset = new StandardIconButton();
      this.btnAddUpdate = new StandardIconButton();
      this.btnDelete = new StandardIconButton();
      this.btnDuplicate = new StandardIconButton();
      this.btnNew = new StandardIconButton();
      this.grpDetails = new GroupContainer();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.panel1 = new Panel();
      this.gvStateAdjustments = new GridView();
      this.label2 = new Label();
      this.txtMaxAmount = new TextBox();
      this.txtMinAmount = new TextBox();
      this.label6 = new Label();
      this.label3 = new Label();
      this.txtBasePrice = new TextBox();
      this.label4 = new Label();
      this.label5 = new Label();
      this.txtImpoundsPrice = new TextBox();
      this.grpSRP = new GroupContainer();
      this.gvPricing = new GridView();
      this.flpControls = new FlowLayoutPanel();
      this.checkBoxSRPfromPPE = new CheckBox();
      ((ISupportInitialize) this.btnReset).BeginInit();
      ((ISupportInitialize) this.btnAddUpdate).BeginInit();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      ((ISupportInitialize) this.btnDuplicate).BeginInit();
      ((ISupportInitialize) this.btnNew).BeginInit();
      this.grpDetails.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.panel1.SuspendLayout();
      this.grpSRP.SuspendLayout();
      this.flpControls.SuspendLayout();
      this.SuspendLayout();
      this.btnReset.BackColor = Color.Transparent;
      this.btnReset.Location = new Point(34, 3);
      this.btnReset.Margin = new Padding(3, 3, 0, 3);
      this.btnReset.MouseDownImage = (Image) null;
      this.btnReset.Name = "btnReset";
      this.btnReset.Size = new Size(16, 16);
      this.btnReset.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.btnReset.TabIndex = 11;
      this.btnReset.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnReset, "Refresh SRP Details");
      this.btnReset.Click += new EventHandler(this.btnReset_Click);
      this.btnAddUpdate.BackColor = Color.Transparent;
      this.btnAddUpdate.Location = new Point(14, 3);
      this.btnAddUpdate.Margin = new Padding(3, 3, 1, 3);
      this.btnAddUpdate.MouseDownImage = (Image) null;
      this.btnAddUpdate.Name = "btnAddUpdate";
      this.btnAddUpdate.Size = new Size(16, 16);
      this.btnAddUpdate.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.btnAddUpdate.TabIndex = 12;
      this.btnAddUpdate.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnAddUpdate, "Add/Update SRP Details");
      this.btnAddUpdate.Click += new EventHandler(this.btnAdd_Click);
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Enabled = false;
      this.btnDelete.Location = new Point(282, 3);
      this.btnDelete.Margin = new Padding(3, 3, 0, 3);
      this.btnDelete.MouseDownImage = (Image) null;
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(16, 16);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabIndex = 9;
      this.btnDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnDelete, "Delete Item");
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.btnDuplicate.BackColor = Color.Transparent;
      this.btnDuplicate.Enabled = false;
      this.btnDuplicate.Location = new Point(262, 3);
      this.btnDuplicate.Margin = new Padding(3, 3, 1, 3);
      this.btnDuplicate.MouseDownImage = (Image) null;
      this.btnDuplicate.Name = "btnDuplicate";
      this.btnDuplicate.Size = new Size(16, 16);
      this.btnDuplicate.StandardButtonType = StandardIconButton.ButtonType.DuplicateButton;
      this.btnDuplicate.TabIndex = 8;
      this.btnDuplicate.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnDuplicate, "Duplicate Item");
      this.btnDuplicate.Click += new EventHandler(this.btnDuplicate_Click);
      this.btnNew.BackColor = Color.Transparent;
      this.btnNew.Location = new Point(242, 3);
      this.btnNew.Margin = new Padding(3, 3, 1, 3);
      this.btnNew.MouseDownImage = (Image) null;
      this.btnNew.Name = "btnNew";
      this.btnNew.Size = new Size(16, 16);
      this.btnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnNew.TabIndex = 7;
      this.btnNew.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnNew, "Add Item");
      this.btnNew.Click += new EventHandler(this.btnNew_Click);
      this.grpDetails.Controls.Add((Control) this.flowLayoutPanel1);
      this.grpDetails.Controls.Add((Control) this.panel1);
      this.grpDetails.Dock = DockStyle.Fill;
      this.grpDetails.HeaderForeColor = SystemColors.ControlText;
      this.grpDetails.Location = new Point(0, 184);
      this.grpDetails.Name = "grpDetails";
      this.grpDetails.Size = new Size(434, 257);
      this.grpDetails.TabIndex = 8;
      this.grpDetails.Text = "SRP Details";
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnReset);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnAddUpdate);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(378, 2);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(50, 22);
      this.flowLayoutPanel1.TabIndex = 12;
      this.panel1.BackColor = Color.Transparent;
      this.panel1.Controls.Add((Control) this.gvStateAdjustments);
      this.panel1.Controls.Add((Control) this.label2);
      this.panel1.Controls.Add((Control) this.txtMaxAmount);
      this.panel1.Controls.Add((Control) this.txtMinAmount);
      this.panel1.Controls.Add((Control) this.label6);
      this.panel1.Controls.Add((Control) this.label3);
      this.panel1.Controls.Add((Control) this.txtBasePrice);
      this.panel1.Controls.Add((Control) this.label4);
      this.panel1.Controls.Add((Control) this.label5);
      this.panel1.Controls.Add((Control) this.txtImpoundsPrice);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(1, 26);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(432, 230);
      this.panel1.TabIndex = 10;
      this.gvStateAdjustments.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "State";
      gvColumn1.Width = 101;
      gvColumn2.ActivatedEditorType = GVActivatedEditorType.TextBox;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "SRP Adjustment";
      gvColumn2.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn2.Width = 124;
      gvColumn3.ActivatedEditorType = GVActivatedEditorType.TextBox;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "SRP Adjustment, If Impounds are Waived";
      gvColumn3.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn3.Width = 185;
      this.gvStateAdjustments.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gvStateAdjustments.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvStateAdjustments.Location = new Point(5, 99);
      this.gvStateAdjustments.Name = "gvStateAdjustments";
      this.gvStateAdjustments.Size = new Size(421, 126);
      this.gvStateAdjustments.SortOption = GVSortOption.None;
      this.gvStateAdjustments.TabIndex = 18;
      this.gvStateAdjustments.TabStop = false;
      this.gvStateAdjustments.EditorOpening += new GVSubItemEditingEventHandler(this.gvStateAdjustments_EditorOpening);
      this.gvStateAdjustments.EditorClosing += new GVSubItemEditingEventHandler(this.gvStateAdjustments_EditorClosing);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(6, 12);
      this.label2.Name = "label2";
      this.label2.Size = new Size(70, 13);
      this.label2.TabIndex = 19;
      this.label2.Text = "Loan Amount";
      this.txtMaxAmount.Location = new Point(217, 9);
      this.txtMaxAmount.MaxLength = 9;
      this.txtMaxAmount.Name = "txtMaxAmount";
      this.txtMaxAmount.Size = new Size(92, 20);
      this.txtMaxAmount.TabIndex = 2;
      this.txtMaxAmount.TextAlign = HorizontalAlignment.Right;
      this.txtMaxAmount.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.txtMinAmount.Location = new Point(109, 9);
      this.txtMinAmount.MaxLength = 9;
      this.txtMinAmount.Name = "txtMinAmount";
      this.txtMinAmount.Size = new Size(92, 20);
      this.txtMinAmount.TabIndex = 1;
      this.txtMinAmount.TextAlign = HorizontalAlignment.Right;
      this.txtMinAmount.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(6, 80);
      this.label6.Name = "label6";
      this.label6.Size = new Size(187, 13);
      this.label6.TabIndex = 23;
      this.label6.Text = "SRP Adjustment Based on Geography";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(6, 34);
      this.label3.Name = "label3";
      this.label3.Size = new Size(56, 13);
      this.label3.TabIndex = 20;
      this.label3.Text = "Base SRP";
      this.txtBasePrice.Location = new Point(109, 31);
      this.txtBasePrice.MaxLength = 7;
      this.txtBasePrice.Name = "txtBasePrice";
      this.txtBasePrice.Size = new Size(92, 20);
      this.txtBasePrice.TabIndex = 3;
      this.txtBasePrice.TextAlign = HorizontalAlignment.Right;
      this.txtBasePrice.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(204, 12);
      this.label4.Name = "label4";
      this.label4.Size = new Size(10, 13);
      this.label4.TabIndex = 21;
      this.label4.Text = "-";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(6, 56);
      this.label5.Name = "label5";
      this.label5.Size = new Size(230, 13);
      this.label5.TabIndex = 22;
      this.label5.Text = "Base SRP Adjustment, If Impounds are Waived";
      this.txtImpoundsPrice.Location = new Point(237, 52);
      this.txtImpoundsPrice.MaxLength = 7;
      this.txtImpoundsPrice.Name = "txtImpoundsPrice";
      this.txtImpoundsPrice.Size = new Size(92, 20);
      this.txtImpoundsPrice.TabIndex = 4;
      this.txtImpoundsPrice.TextAlign = HorizontalAlignment.Right;
      this.txtImpoundsPrice.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.grpSRP.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.grpSRP.Controls.Add((Control) this.gvPricing);
      this.grpSRP.Controls.Add((Control) this.flpControls);
      this.grpSRP.Dock = DockStyle.Top;
      this.grpSRP.HeaderForeColor = SystemColors.ControlText;
      this.grpSRP.Location = new Point(0, 0);
      this.grpSRP.Name = "grpSRP";
      this.grpSRP.Size = new Size(434, 184);
      this.grpSRP.TabIndex = 7;
      this.grpSRP.Text = "SRP Table";
      this.gvPricing.BorderStyle = BorderStyle.None;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column1";
      gvColumn4.Text = "Loan Amount";
      gvColumn4.Width = 249;
      gvColumn5.ActivatedEditorType = GVActivatedEditorType.TextBox;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column2";
      gvColumn5.Text = "Base SRP";
      gvColumn5.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn5.Width = 67;
      gvColumn6.ActivatedEditorType = GVActivatedEditorType.TextBox;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column3";
      gvColumn6.Text = "Base SRP Adjustment, If Impounds are Waived";
      gvColumn6.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn6.Width = 116;
      this.gvPricing.Columns.AddRange(new GVColumn[3]
      {
        gvColumn4,
        gvColumn5,
        gvColumn6
      });
      this.gvPricing.Dock = DockStyle.Fill;
      this.gvPricing.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvPricing.Location = new Point(1, 26);
      this.gvPricing.Name = "gvPricing";
      this.gvPricing.Size = new Size(432, 158);
      this.gvPricing.SortOption = GVSortOption.None;
      this.gvPricing.TabIndex = 6;
      this.gvPricing.TabStop = false;
      this.gvPricing.SelectedIndexChanged += new EventHandler(this.lvwPricing_SelectedIndexChanged);
      this.gvPricing.EditorOpening += new GVSubItemEditingEventHandler(this.gvPricing_EditorOpening);
      this.gvPricing.EditorClosing += new GVSubItemEditingEventHandler(this.gvPricing_EditorClosing);
      this.flpControls.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flpControls.BackColor = Color.Transparent;
      this.flpControls.Controls.Add((Control) this.btnDelete);
      this.flpControls.Controls.Add((Control) this.btnDuplicate);
      this.flpControls.Controls.Add((Control) this.btnNew);
      this.flpControls.Controls.Add((Control) this.checkBoxSRPfromPPE);
      this.flpControls.FlowDirection = FlowDirection.RightToLeft;
      this.flpControls.Location = new Point(134, 2);
      this.flpControls.Name = "flpControls";
      this.flpControls.Size = new Size(298, 22);
      this.flpControls.TabIndex = 11;
      this.flpControls.WrapContents = false;
      this.checkBoxSRPfromPPE.AutoSize = true;
      this.checkBoxSRPfromPPE.Location = new Point(141, 3);
      this.checkBoxSRPfromPPE.Name = "checkBoxSRPfromPPE";
      this.checkBoxSRPfromPPE.Size = new Size(95, 17);
      this.checkBoxSRPfromPPE.TabIndex = 10;
      this.checkBoxSRPfromPPE.Text = "SRP from PPE";
      this.checkBoxSRPfromPPE.UseVisualStyleBackColor = true;
      this.checkBoxSRPfromPPE.Visible = false;
      this.checkBoxSRPfromPPE.CheckedChanged += new EventHandler(this.checkBoxSRPfromPPE_CheckedChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.grpDetails);
      this.Controls.Add((Control) this.grpSRP);
      this.Name = nameof (SRPTableEditor);
      this.Size = new Size(434, 441);
      ((ISupportInitialize) this.btnReset).EndInit();
      ((ISupportInitialize) this.btnAddUpdate).EndInit();
      ((ISupportInitialize) this.btnDelete).EndInit();
      ((ISupportInitialize) this.btnDuplicate).EndInit();
      ((ISupportInitialize) this.btnNew).EndInit();
      this.grpDetails.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.grpSRP.ResumeLayout(false);
      this.flpControls.ResumeLayout(false);
      this.flpControls.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
