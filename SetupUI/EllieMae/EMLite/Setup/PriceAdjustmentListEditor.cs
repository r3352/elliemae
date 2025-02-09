// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PriceAdjustmentListEditor
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
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
  public class PriceAdjustmentListEditor : UserControl
  {
    private TradePriceAdjustments adjustments;
    private LoanReportFieldDefs fieldDefs;
    private bool modified;
    private bool readOnly;
    private Sessions.Session session;
    private IContainer components;
    private GridView gvAdjustments;
    private GroupContainer grpAdjustments;
    private StandardIconButton btnEdit;
    private StandardIconButton btnAdd;
    private StandardIconButton btnRemove;
    private FlowLayoutPanel flpControls;
    private ToolTip toolTip1;
    private CheckBox checkBox_AdjustmentfromPPE;

    public event EventHandler DataChange;

    public bool AdjustmentfromPPE
    {
      get => this.checkBox_AdjustmentfromPPE.Checked;
      set => this.checkBox_AdjustmentfromPPE.Checked = value;
    }

    public PriceAdjustmentListEditor()
      : this(Session.DefaultInstance)
    {
    }

    public PriceAdjustmentListEditor(Sessions.Session session)
    {
      this.session = session;
      this.InitializeComponent();
    }

    public void ShowPPEIndicator(TradeType tradeType, CorrespondentMasterDeliveryType deliveryType)
    {
      if (tradeType == TradeType.CorrespondentTrade && (deliveryType == CorrespondentMasterDeliveryType.AOT || deliveryType == CorrespondentMasterDeliveryType.Forwards || deliveryType == CorrespondentMasterDeliveryType.LiveTrade) && Utils.ParseBoolean(this.session.StartupInfo.TradeSettings[(object) "Trade.EnableTPOTradeManagement"]) && Utils.ParseBoolean(this.session.StartupInfo.TradeSettings[(object) "Trade.EPPSLoanProgEliPricing"]) && this.session.StartupInfo.ProductPricingPartner != null && (this.session.StartupInfo.ProductPricingPartner.PartnerID == "MPS" || this.session.StartupInfo.ProductPricingPartner.VendorPlatform.ToDescription() == "EPC2"))
      {
        this.checkBox_AdjustmentfromPPE.Visible = true;
      }
      else
      {
        this.checkBox_AdjustmentfromPPE.Checked = false;
        this.checkBox_AdjustmentfromPPE.Visible = false;
      }
    }

    [Browsable(false)]
    public TradePriceAdjustments Adjustments
    {
      get => this.adjustments;
      set
      {
        this.adjustments = value;
        if (value == null)
          return;
        this.loadAdjustments();
      }
    }

    public bool DataModified => this.modified;

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

    public void MakeBtnInvisible()
    {
      this.btnAdd.Visible = false;
      this.btnEdit.Visible = false;
      this.btnRemove.Visible = false;
      this.ReadOnly = true;
    }

    public void AddControlToHeader(Control c)
    {
      this.flpControls.Controls.Add(c);
      c.BringToFront();
    }

    public void ClearAdjustments()
    {
      if (this.gvAdjustments.Items.Count <= 0)
        return;
      this.gvAdjustments.Items.Clear();
      this.OnDataChange();
    }

    public void AddAdjustments(TradePriceAdjustments adjs)
    {
      foreach (TradePriceAdjustment adj in (List<TradePriceAdjustment>) adjs)
        this.gvAdjustments.Items.Add(this.createAdjustmentListViewItem(adj));
      if (adjs.Count <= 0)
        return;
      this.OnDataChange();
    }

    public void CommitChanges()
    {
      this.adjustments.Clear();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvAdjustments.Items)
        this.adjustments.Add((TradePriceAdjustment) gvItem.Tag);
      this.modified = false;
    }

    private void setReadOnly()
    {
      this.gvAdjustments.SelectedItems.Clear();
      this.btnAdd.Enabled = !this.readOnly;
      this.btnEdit.Enabled = false;
      this.btnRemove.Enabled = false;
      this.checkBox_AdjustmentfromPPE.Enabled = !this.readOnly;
    }

    private void loadAdjustments()
    {
      this.gvAdjustments.Items.Clear();
      foreach (TradePriceAdjustment adjustment in (List<TradePriceAdjustment>) this.adjustments)
        this.gvAdjustments.Items.Add(this.createAdjustmentListViewItem(adjustment));
      this.modified = false;
    }

    private GVItem createAdjustmentListViewItem(TradePriceAdjustment adj)
    {
      GVItem adjustmentListViewItem = new GVItem(adj.CriterionList.ToString());
      if (this.session.SessionObjects.Use10DecimalDigitInLockRequestSecondaryTradeAreas)
        adjustmentListViewItem.SubItems.Add((object) adj.PriceAdjustment.ToString("0.0000000000"));
      else
        adjustmentListViewItem.SubItems.Add((object) adj.PriceAdjustment.ToString("0.000"));
      adjustmentListViewItem.Tag = (object) new TradePriceAdjustment(adj);
      return adjustmentListViewItem;
    }

    private void refreshAdjustmentListViewItem(GVItem item)
    {
      TradePriceAdjustment tag = (TradePriceAdjustment) item.Tag;
      item.SubItems[0].Text = tag.CriterionList.ToString();
      if (this.session.SessionObjects.Use10DecimalDigitInLockRequestSecondaryTradeAreas)
        item.SubItems[1].Text = tag.PriceAdjustment.ToString("0.0000000000");
      else
        item.SubItems[1].Text = tag.PriceAdjustment.ToString("0.000");
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      using (PriceAdjustmentFilterDlg adjustmentFilterDlg = new PriceAdjustmentFilterDlg((TradePriceAdjustment) null, this.session))
      {
        if (adjustmentFilterDlg.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        TradePriceAdjustment adj = new TradePriceAdjustment(adjustmentFilterDlg.GetCurrentFilterList(), 0M);
        adj.PriceAdjustment = adjustmentFilterDlg.PriceAdjustment;
        if (adj.CriterionList.Count > 0)
          this.gvAdjustments.Items.Add(this.createAdjustmentListViewItem(adj));
        this.OnDataChange();
      }
    }

    private void btnEdit_Click(object sender, EventArgs e)
    {
      if (this.gvAdjustments.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select an adjustment from the list to edit.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
        this.editAdjustment(this.gvAdjustments.SelectedItems[0]);
    }

    private void editAdjustment(GVItem item)
    {
      TradePriceAdjustment tag = (TradePriceAdjustment) item.Tag;
      using (PriceAdjustmentFilterDlg adjustmentFilterDlg = new PriceAdjustmentFilterDlg(tag, this.session))
      {
        if (adjustmentFilterDlg.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        tag.CriterionList = adjustmentFilterDlg.GetCurrentFilterList();
        if (tag.CriterionList.Count == 0)
        {
          this.gvAdjustments.Items.Remove(this.gvAdjustments.SelectedItems[0]);
        }
        else
        {
          tag.PriceAdjustment = adjustmentFilterDlg.PriceAdjustment;
          item.Tag = (object) tag;
          this.refreshAdjustmentListViewItem(item);
        }
        this.OnDataChange();
      }
    }

    private void gvAdjustments_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.editSelectedItem();
    }

    private void editSelectedItem()
    {
      if (this.readOnly)
        return;
      GVHitTestInfo gvHitTestInfo = this.gvAdjustments.HitTest(this.gvAdjustments.PointToClient(Cursor.Position));
      if (gvHitTestInfo.RowIndex < 0)
        return;
      this.editAdjustment(this.gvAdjustments.Items[gvHitTestInfo.RowIndex]);
    }

    private void btnRemove_Click(object sender, EventArgs e)
    {
      this.gvAdjustments.CancelEditing();
      if (this.gvAdjustments.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select an adjustment from the list to edit.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        while (this.gvAdjustments.SelectedItems.Count > 0)
          this.gvAdjustments.Items.Remove(this.gvAdjustments.SelectedItems[0]);
        this.OnDataChange();
      }
    }

    protected virtual void OnDataChange()
    {
      this.modified = true;
      if (this.DataChange == null)
        return;
      this.DataChange((object) this, EventArgs.Empty);
    }

    private LoanReportFieldDefs getFieldDefinitions()
    {
      if (this.fieldDefs == null)
        this.fieldDefs = LoanReportFieldDefs.GetFieldDefs(this.session, LoanReportFieldFlags.LoanDataFieldsInDatabase);
      return this.fieldDefs;
    }

    private void PriceAdjustmentListEditor_Resize(object sender, EventArgs e)
    {
      this.gvAdjustments.Anchor = AnchorStyles.Top | AnchorStyles.Left;
      this.gvAdjustments.Height = this.ClientSize.Height;
      this.gvAdjustments.Width = Math.Max(0, this.ClientSize.Width - this.btnAdd.Width - 10);
    }

    private void lvwAdjustments_SelectedIndexChanged(object sender, EventArgs e)
    {
      int count = this.gvAdjustments.SelectedItems.Count;
      this.btnEdit.Enabled = count == 1 && !this.readOnly;
      this.btnRemove.Enabled = count > 0 && !this.readOnly;
    }

    private void gvAdjustments_EditorOpening(object source, GVSubItemEditingEventArgs e)
    {
      e.Cancel = this.readOnly;
      if (this.session.SessionObjects.Use10DecimalDigitInLockRequestSecondaryTradeAreas)
        TextBoxFormatter.Attach(e.EditorControl as TextBox, TextBoxContentRule.Decimal, "0.0000000000");
      else
        TextBoxFormatter.Attach(e.EditorControl as TextBox, TextBoxContentRule.Decimal, "0.000");
    }

    private void gvAdjustments_EditorClosing(object source, GVSubItemEditingEventArgs e)
    {
      TradePriceAdjustment tag = (TradePriceAdjustment) e.SubItem.Item.Tag;
      Decimal num = Utils.ParseDecimal((object) e.EditorControl.Text);
      if (num != tag.PriceAdjustment)
      {
        tag.PriceAdjustment = num;
        this.OnDataChange();
      }
      if (this.session.SessionObjects.Use10DecimalDigitInLockRequestSecondaryTradeAreas)
        e.EditorControl.Text = num.ToString("0.0000000000");
      else
        e.EditorControl.Text = num.ToString("0.000");
    }

    public void EnableAdjustmentfromPPE()
    {
      this.checkBox_AdjustmentfromPPE.Visible = true;
      this.checkBox_AdjustmentfromPPE.Enabled = true;
    }

    private void checkBox_AdjustmentfromPPE_CheckedChanged(object sender, EventArgs e)
    {
      this.modified = true;
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
      this.gvAdjustments = new GridView();
      this.grpAdjustments = new GroupContainer();
      this.flpControls = new FlowLayoutPanel();
      this.btnRemove = new StandardIconButton();
      this.btnEdit = new StandardIconButton();
      this.btnAdd = new StandardIconButton();
      this.checkBox_AdjustmentfromPPE = new CheckBox();
      this.toolTip1 = new ToolTip(this.components);
      this.grpAdjustments.SuspendLayout();
      this.flpControls.SuspendLayout();
      ((ISupportInitialize) this.btnRemove).BeginInit();
      ((ISupportInitialize) this.btnEdit).BeginInit();
      ((ISupportInitialize) this.btnAdd).BeginInit();
      this.SuspendLayout();
      this.gvAdjustments.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SpringToFit = true;
      gvColumn1.Text = "Filter";
      gvColumn1.Width = 436;
      gvColumn2.ActivatedEditorType = GVActivatedEditorType.TextBox;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column5";
      gvColumn2.Text = "Price Adjustment";
      gvColumn2.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn2.Width = 98;
      this.gvAdjustments.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvAdjustments.Dock = DockStyle.Fill;
      this.gvAdjustments.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvAdjustments.Location = new Point(1, 26);
      this.gvAdjustments.Name = "gvAdjustments";
      this.gvAdjustments.Size = new Size(534, 253);
      this.gvAdjustments.SortOption = GVSortOption.None;
      this.gvAdjustments.TabIndex = 0;
      this.gvAdjustments.TabStop = false;
      this.gvAdjustments.SelectedIndexChanged += new EventHandler(this.lvwAdjustments_SelectedIndexChanged);
      this.gvAdjustments.ItemDoubleClick += new GVItemEventHandler(this.gvAdjustments_ItemDoubleClick);
      this.gvAdjustments.EditorOpening += new GVSubItemEditingEventHandler(this.gvAdjustments_EditorOpening);
      this.gvAdjustments.EditorClosing += new GVSubItemEditingEventHandler(this.gvAdjustments_EditorClosing);
      this.grpAdjustments.Controls.Add((Control) this.flpControls);
      this.grpAdjustments.Controls.Add((Control) this.gvAdjustments);
      this.grpAdjustments.Dock = DockStyle.Fill;
      this.grpAdjustments.HeaderForeColor = SystemColors.ControlText;
      this.grpAdjustments.Location = new Point(0, 0);
      this.grpAdjustments.Name = "grpAdjustments";
      this.grpAdjustments.Size = new Size(536, 280);
      this.grpAdjustments.TabIndex = 5;
      this.grpAdjustments.Text = "Price Adjustments";
      this.flpControls.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.flpControls.BackColor = Color.Transparent;
      this.flpControls.Controls.Add((Control) this.btnRemove);
      this.flpControls.Controls.Add((Control) this.btnEdit);
      this.flpControls.Controls.Add((Control) this.btnAdd);
      this.flpControls.Controls.Add((Control) this.checkBox_AdjustmentfromPPE);
      this.flpControls.FlowDirection = FlowDirection.RightToLeft;
      this.flpControls.Location = new Point(216, 2);
      this.flpControls.Name = "flpControls";
      this.flpControls.Size = new Size(312, 22);
      this.flpControls.TabIndex = 8;
      this.flpControls.WrapContents = false;
      this.btnRemove.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRemove.BackColor = Color.Transparent;
      this.btnRemove.Enabled = false;
      this.btnRemove.Location = new Point(296, 3);
      this.btnRemove.Margin = new Padding(3, 3, 0, 3);
      this.btnRemove.MouseDownImage = (Image) null;
      this.btnRemove.Name = "btnRemove";
      this.btnRemove.Size = new Size(16, 16);
      this.btnRemove.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnRemove.TabIndex = 5;
      this.btnRemove.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnRemove, "Remove Adjustment");
      this.btnRemove.Click += new EventHandler(this.btnRemove_Click);
      this.btnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnEdit.BackColor = Color.Transparent;
      this.btnEdit.Enabled = false;
      this.btnEdit.Location = new Point(276, 3);
      this.btnEdit.Margin = new Padding(3, 3, 1, 3);
      this.btnEdit.MouseDownImage = (Image) null;
      this.btnEdit.Name = "btnEdit";
      this.btnEdit.Size = new Size(16, 16);
      this.btnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEdit.TabIndex = 7;
      this.btnEdit.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnEdit, "Edit Adjustment");
      this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
      this.btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAdd.BackColor = Color.Transparent;
      this.btnAdd.Location = new Point(256, 3);
      this.btnAdd.Margin = new Padding(3, 3, 1, 3);
      this.btnAdd.MouseDownImage = (Image) null;
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(16, 16);
      this.btnAdd.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAdd.TabIndex = 6;
      this.btnAdd.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnAdd, "Add Adjustment");
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.checkBox_AdjustmentfromPPE.AutoSize = true;
      this.checkBox_AdjustmentfromPPE.Location = new Point(120, 3);
      this.checkBox_AdjustmentfromPPE.Name = "checkBox_AdjustmentfromPPE";
      this.checkBox_AdjustmentfromPPE.Size = new Size(130, 17);
      this.checkBox_AdjustmentfromPPE.TabIndex = 8;
      this.checkBox_AdjustmentfromPPE.Text = "Adjustments from PPE";
      this.checkBox_AdjustmentfromPPE.UseVisualStyleBackColor = true;
      this.checkBox_AdjustmentfromPPE.Visible = false;
      this.checkBox_AdjustmentfromPPE.CheckedChanged += new EventHandler(this.checkBox_AdjustmentfromPPE_CheckedChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.grpAdjustments);
      this.Name = nameof (PriceAdjustmentListEditor);
      this.Size = new Size(536, 280);
      this.Resize += new EventHandler(this.PriceAdjustmentListEditor_Resize);
      this.grpAdjustments.ResumeLayout(false);
      this.flpControls.ResumeLayout(false);
      this.flpControls.PerformLayout();
      ((ISupportInitialize) this.btnRemove).EndInit();
      ((ISupportInitialize) this.btnEdit).EndInit();
      ((ISupportInitialize) this.btnAdd).EndInit();
      this.ResumeLayout(false);
    }
  }
}
