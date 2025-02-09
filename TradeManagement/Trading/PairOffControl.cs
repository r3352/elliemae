// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.PairOffControl
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Reporting;
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
  public class PairOffControl : UserControl
  {
    private bool readOnly = true;
    public List<object> SelectedPairOffs;
    public string DialogHeadTitle = string.Empty;
    public string DeleteItemText = string.Empty;
    public PairOffType PairOffType;
    private IContainer components;
    private TableContainer tcPairOffs;
    private FlowLayoutPanel flowLayoutPanel2;
    private StandardIconButton btnDelPairOffs;
    private StandardIconButton btnExport;
    private StandardIconButton btnEditPairOffs;
    private StandardIconButton btnAddPairOffs;
    private GridView gvPairOffs;

    public bool ReadOnly
    {
      get => this.readOnly;
      set
      {
        this.readOnly = value;
        this.SetReadOnly(!this.readOnly);
      }
    }

    public bool Locked { get; set; }

    public bool Modified { get; set; }

    public LoanReportFieldDefs FieldDefs { get; set; }

    public event EventHandler DeleteButtonClicked;

    public ITradeEditor TradeEditor { get; set; }

    protected virtual void OnDeleteButtonClicked(EventArgs e)
    {
      EventHandler deleteButtonClicked = this.DeleteButtonClicked;
      if (deleteButtonClicked == null)
        return;
      deleteButtonClicked((object) this, e);
    }

    public event EventHandler EditButtonClicked;

    protected virtual void OnEditButtonClicked(EventArgs e)
    {
      EventHandler editButtonClicked = this.EditButtonClicked;
      if (editButtonClicked == null)
        return;
      editButtonClicked((object) this, e);
    }

    public PairOffControl(PairOffType pairOffType)
    {
      this.InitializeComponent();
      this.PairOffType = pairOffType;
      if (this.PairOffType == PairOffType.CorrespondentTrades)
        return;
      GVColumn column1 = this.gvPairOffs.Columns[1];
      GVColumn column2 = this.gvPairOffs.Columns[6];
      this.gvPairOffs.Columns.Remove(column1);
      this.gvPairOffs.Columns.Remove(column2);
    }

    private void SetReadOnly(bool enabled)
    {
      this.btnAddPairOffs.Enabled = enabled;
      this.btnDelPairOffs.Enabled = enabled;
      this.btnEditPairOffs.Enabled = enabled;
      this.setButtonState(enabled);
    }

    private void setButtonState(bool enabled)
    {
      if (!enabled || this.PairOffType != PairOffType.CorrespondentTrades)
        return;
      bool flag = this.gvPairOffs.SelectedItems.Count != 0;
      this.btnEditPairOffs.Enabled = flag;
      this.btnDelPairOffs.Enabled = flag;
      this.btnExport.Enabled = flag;
    }

    private void btnAddPairOffs_Click(object sender, EventArgs e)
    {
      this.EditPairOff((CommonPairOff) null);
    }

    private void btnEditPairOff_Click(object sender, EventArgs e)
    {
      if (this.gvPairOffs.SelectedItems.Count < 1)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a pair off to edit.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else if (this.gvPairOffs.SelectedItems.Count > 1)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "Multiple Pair-offs cannot be edited at the same time.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
        this.EditPairOff((CommonPairOff) this.gvPairOffs.SelectedItems[0].Tag);
    }

    private void btnDelPairOffs_Click(object sender, EventArgs e)
    {
      if (this.gvPairOffs.SelectedItems.Count < 1)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a pair off to remove.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, string.Format("Are you sure you want to remove the selected pair-offs from the {0}?", (object) this.DeleteItemText), MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
          return;
        this.SelectedPairOffs = new List<object>();
        if (this.PairOffType == PairOffType.CorrespondentTrades)
        {
          bool positiveEntryExists = false;
          CommonPairOff commonPairOff = (CommonPairOff) null;
          foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvPairOffs.Items)
          {
            CommonPairOff tag = (CommonPairOff) gvItem.Tag;
            if ((commonPairOff == null || commonPairOff.Index != tag.Index) && tag.TradeAmount > 0M)
              positiveEntryExists = true;
          }
          Decimal openAmount = this.TradeEditor != null ? this.TradeEditor.GetOpenAmount() : 0M;
          Decimal tradeAmount = this.TradeEditor != null ? this.TradeEditor.TradeAmount : 0M;
          foreach (GVItem selectedItem in this.gvPairOffs.SelectedItems)
          {
            this.SelectedPairOffs.Add(selectedItem.Tag);
            CommonPairOff tag = (CommonPairOff) selectedItem.Tag;
            using (CommonPairOffDialog commonPairOffDialog = new CommonPairOffDialog(tag, this.PairOffType, positiveEntryExists, openAmount, tradeAmount, this.getTotalPairOffAmount()))
            {
              if (!commonPairOffDialog.ValidatePairOff(tag.TradeAmount, (IWin32Window) this, true))
                break;
              this.Modified = true;
              this.OnDeleteButtonClicked(e);
            }
          }
        }
        else
        {
          Decimal num2 = 0M;
          foreach (GVItem selectedItem in this.gvPairOffs.SelectedItems)
          {
            this.SelectedPairOffs.Add(selectedItem.Tag);
            num2 += ((CommonPairOff) selectedItem.Tag).TradeAmount;
          }
          if (this.TradeEditor != null && this.TradeEditor.GetOpenAmount() + num2 < 0M)
          {
            int num3 = (int) Utils.Dialog((IWin32Window) this, "The trade amount of the pair-off cannot be greater than the open amount.");
          }
          else
          {
            this.Modified = true;
            this.OnDeleteButtonClicked(e);
          }
        }
      }
    }

    private void btnExport_Click(object sender, EventArgs e) => this.ExportPairOff();

    private void gvPairOffs_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.EditPairOff((CommonPairOff) this.gvPairOffs.SelectedItems[0].Tag);
    }

    private void EditPairOff(CommonPairOff pairOff)
    {
      bool positiveEntryExists = false;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvPairOffs.Items)
      {
        CommonPairOff tag = (CommonPairOff) gvItem.Tag;
        if ((pairOff == null || pairOff.Index != tag.Index) && tag.TradeAmount > 0M)
          positiveEntryExists = true;
      }
      Decimal openAmount = this.TradeEditor != null ? this.TradeEditor.GetOpenAmount() : 0M;
      Decimal tradeAmount = this.TradeEditor != null ? this.TradeEditor.TradeAmount : 0M;
      using (CommonPairOffDialog commonPairOffDialog = new CommonPairOffDialog(pairOff, this.PairOffType, positiveEntryExists, openAmount, tradeAmount, this.getTotalPairOffAmount()))
      {
        commonPairOffDialog.Text = this.DialogHeadTitle;
        commonPairOffDialog.ReadOnly = this.ReadOnly;
        if (!this.ReadOnly && this.Locked)
          commonPairOffDialog.ReadOnly = true;
        if (commonPairOffDialog.ShowDialog((IWin32Window) this) == DialogResult.Cancel || !commonPairOffDialog.DataModified)
          return;
        commonPairOffDialog.CommitChanges();
        this.SelectedPairOffs = new List<object>();
        if (pairOff != null)
          commonPairOffDialog.PairOff.Index = this.gvPairOffs.SelectedItems[0].Index + 1;
        this.SelectedPairOffs.Add((object) commonPairOffDialog.PairOff);
        this.Modified = true;
        this.OnEditButtonClicked((EventArgs) null);
      }
    }

    private bool ExportPairOff()
    {
      if (this.gvPairOffs.SelectedItems.Count < 1)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a pair-off to export to Excel.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      if (this.gvPairOffs.Columns.Count > ExcelHandler.GetMaximumColumnCount())
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You pair-off list cannot be exported because the number of columns exceeds the limit supported by Excel (" + (object) ExcelHandler.GetMaximumColumnCount() + ")", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      if (this.gvPairOffs.SelectedItems.Count > ExcelHandler.GetMaximumRowCount() - 1)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You pair-off list cannot be exported because the number of rows exceeds the limit supported by Excel (" + (object) ExcelHandler.GetMaximumRowCount() + ")", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      using (CursorActivator.Wait())
      {
        ExcelHandler excelHandler = new ExcelHandler();
        excelHandler.AddDataTable(this.gvPairOffs, (ReportFieldDefs) this.FieldDefs, true);
        excelHandler.CreateExcel();
      }
      return true;
    }

    private Decimal getTotalPairOffAmount()
    {
      return this.gvPairOffs.Items.Sum<GVItem>((Func<GVItem, Decimal>) (item => ((CommonPairOff) item.Tag).TradeAmount));
    }

    public void LoadPairOffs(CommonPairOff[] pairOffs)
    {
      List<GVItem> gvItemList = new List<GVItem>();
      this.gvPairOffs.Items.Clear();
      for (int index = 0; index < pairOffs.Length; ++index)
      {
        CommonPairOff pairOff = pairOffs[index];
        GVItem gvItem = new GVItem("Pair-Off " + (object) (index + 1));
        if (this.PairOffType == PairOffType.CorrespondentTrades)
          gvItem.SubItems.Add((object) pairOff.RequestedBy.ToString());
        if (pairOff.Date != DateTime.MinValue)
          gvItem.SubItems.Add((object) pairOff.Date.ToString("MM/dd/yyyy"));
        else
          gvItem.SubItems.Add((object) "");
        Decimal num;
        if (pairOff.TradeAmount > 0M || this.PairOffType == PairOffType.CorrespondentTrades)
        {
          GVSubItemCollection subItems = gvItem.SubItems;
          num = pairOff.DisplayedTradeAmount;
          string str = num.ToString("#,0.00#");
          subItems.Add((object) str);
        }
        else
          gvItem.SubItems.Add((object) "");
        if (pairOff.PairOffFeePercentage != 0M)
        {
          GVSubItemCollection subItems = gvItem.SubItems;
          num = pairOff.PairOffFeePercentage;
          string str = num.ToString("N5");
          subItems.Add((object) str);
        }
        else
          gvItem.SubItems.Add((object) "");
        if (pairOff.CalculatedPairOffFee != 0M)
        {
          GVSubItemCollection subItems = gvItem.SubItems;
          num = pairOff.DisplayCalculatedPairOffFee;
          string str = num.ToString("#,0.00#");
          subItems.Add((object) str);
        }
        else
          gvItem.SubItems.Add((object) "");
        if (this.PairOffType == PairOffType.CorrespondentTrades)
          gvItem.SubItems.Add((object) pairOff.Comments.Replace(Environment.NewLine, " "));
        gvItem.Tag = (object) pairOff;
        this.gvPairOffs.Items.Add(gvItem);
      }
      this.setButtonState(!this.readOnly);
    }

    private void gvPairOffs_ItemClick(object source, GVItemEventArgs e)
    {
      this.btnEditPairOffs.Enabled = true;
      if (this.PairOffType != PairOffType.CorrespondentTrades)
        return;
      if (this.gvPairOffs.SelectedItems.Count > 1 || this.gvPairOffs.SelectedItems.Count == 0)
      {
        this.btnDelPairOffs.Enabled = false;
        this.btnEditPairOffs.Enabled = false;
      }
      else if (!this.readOnly)
      {
        this.btnDelPairOffs.Enabled = true;
        this.btnExport.Enabled = true;
      }
      else
        this.btnDelPairOffs.Enabled = false;
    }

    private void gvPairOffs_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.gvPairOffs_ItemClick(sender, (GVItemEventArgs) null);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      this.tcPairOffs = new TableContainer();
      this.flowLayoutPanel2 = new FlowLayoutPanel();
      this.btnDelPairOffs = new StandardIconButton();
      this.btnExport = new StandardIconButton();
      this.btnEditPairOffs = new StandardIconButton();
      this.btnAddPairOffs = new StandardIconButton();
      this.gvPairOffs = new GridView();
      this.tcPairOffs.SuspendLayout();
      this.flowLayoutPanel2.SuspendLayout();
      ((ISupportInitialize) this.btnDelPairOffs).BeginInit();
      ((ISupportInitialize) this.btnExport).BeginInit();
      ((ISupportInitialize) this.btnEditPairOffs).BeginInit();
      ((ISupportInitialize) this.btnAddPairOffs).BeginInit();
      this.SuspendLayout();
      this.tcPairOffs.Controls.Add((Control) this.flowLayoutPanel2);
      this.tcPairOffs.Controls.Add((Control) this.gvPairOffs);
      this.tcPairOffs.Dock = DockStyle.Fill;
      this.tcPairOffs.Location = new Point(0, 0);
      this.tcPairOffs.Margin = new Padding(3, 3, 3, 15);
      this.tcPairOffs.Name = "tcPairOffs";
      this.tcPairOffs.Size = new Size(733, 194);
      this.tcPairOffs.Style = TableContainer.ContainerStyle.HeaderOnly;
      this.tcPairOffs.TabIndex = 1;
      this.tcPairOffs.Text = "Pair-Offs";
      this.flowLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.flowLayoutPanel2.BackColor = Color.Transparent;
      this.flowLayoutPanel2.Controls.Add((Control) this.btnDelPairOffs);
      this.flowLayoutPanel2.Controls.Add((Control) this.btnExport);
      this.flowLayoutPanel2.Controls.Add((Control) this.btnEditPairOffs);
      this.flowLayoutPanel2.Controls.Add((Control) this.btnAddPairOffs);
      this.flowLayoutPanel2.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel2.Location = new Point(566, 2);
      this.flowLayoutPanel2.Name = "flowLayoutPanel2";
      this.flowLayoutPanel2.Size = new Size(164, 22);
      this.flowLayoutPanel2.TabIndex = 115;
      this.btnDelPairOffs.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDelPairOffs.BackColor = Color.Transparent;
      this.btnDelPairOffs.Location = new Point(145, 3);
      this.btnDelPairOffs.MouseDownImage = (Image) null;
      this.btnDelPairOffs.Name = "btnDelPairOffs";
      this.btnDelPairOffs.Size = new Size(16, 16);
      this.btnDelPairOffs.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelPairOffs.TabIndex = 3;
      this.btnDelPairOffs.TabStop = false;
      this.btnDelPairOffs.Click += new EventHandler(this.btnDelPairOffs_Click);
      this.btnExport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExport.BackColor = Color.Transparent;
      this.btnExport.Location = new Point(123, 3);
      this.btnExport.MouseDownImage = (Image) null;
      this.btnExport.Name = "btnExport";
      this.btnExport.Size = new Size(16, 16);
      this.btnExport.StandardButtonType = StandardIconButton.ButtonType.ExcelButton;
      this.btnExport.TabIndex = 50;
      this.btnExport.TabStop = false;
      this.btnExport.Click += new EventHandler(this.btnExport_Click);
      this.btnEditPairOffs.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnEditPairOffs.BackColor = Color.Transparent;
      this.btnEditPairOffs.Location = new Point(101, 3);
      this.btnEditPairOffs.MouseDownImage = (Image) null;
      this.btnEditPairOffs.Name = "btnEditPairOffs";
      this.btnEditPairOffs.Size = new Size(16, 16);
      this.btnEditPairOffs.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEditPairOffs.TabIndex = 82;
      this.btnEditPairOffs.TabStop = false;
      this.btnEditPairOffs.Click += new EventHandler(this.btnEditPairOff_Click);
      this.btnAddPairOffs.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddPairOffs.BackColor = Color.Transparent;
      this.btnAddPairOffs.Location = new Point(79, 3);
      this.btnAddPairOffs.MouseDownImage = (Image) null;
      this.btnAddPairOffs.Name = "btnAddPairOffs";
      this.btnAddPairOffs.Size = new Size(16, 16);
      this.btnAddPairOffs.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddPairOffs.TabIndex = 4;
      this.btnAddPairOffs.TabStop = false;
      this.btnAddPairOffs.Click += new EventHandler(this.btnAddPairOffs_Click);
      this.gvPairOffs.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Pair-Off #";
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "colRequestedBy";
      gvColumn2.Text = "Requested By";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column2";
      gvColumn3.Text = "Pair-Off Date";
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column3";
      gvColumn4.Text = "Pair-Off Amount";
      gvColumn4.Width = 100;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column4";
      gvColumn5.Text = "Pair-Off Fee";
      gvColumn5.Width = 100;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column5";
      gvColumn6.Text = "Gain/Loss";
      gvColumn6.Width = 100;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "colComments";
      gvColumn7.Text = "Comments";
      gvColumn7.Width = 100;
      this.gvPairOffs.Columns.AddRange(new GVColumn[7]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7
      });
      this.gvPairOffs.Dock = DockStyle.Fill;
      this.gvPairOffs.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvPairOffs.Location = new Point(1, 26);
      this.gvPairOffs.Name = "gvPairOffs";
      this.gvPairOffs.Size = new Size(731, 167);
      this.gvPairOffs.SortOption = GVSortOption.None;
      this.gvPairOffs.TabIndex = 0;
      this.gvPairOffs.SelectedIndexChanged += new EventHandler(this.gvPairOffs_SelectedIndexChanged);
      this.gvPairOffs.ItemClick += new GVItemEventHandler(this.gvPairOffs_ItemClick);
      this.gvPairOffs.ItemDoubleClick += new GVItemEventHandler(this.gvPairOffs_ItemDoubleClick);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.tcPairOffs);
      this.Name = nameof (PairOffControl);
      this.Size = new Size(733, 194);
      this.tcPairOffs.ResumeLayout(false);
      this.flowLayoutPanel2.ResumeLayout(false);
      ((ISupportInitialize) this.btnDelPairOffs).EndInit();
      ((ISupportInitialize) this.btnExport).EndInit();
      ((ISupportInitialize) this.btnEditPairOffs).EndInit();
      ((ISupportInitialize) this.btnAddPairOffs).EndInit();
      this.ResumeLayout(false);
    }
  }
}
