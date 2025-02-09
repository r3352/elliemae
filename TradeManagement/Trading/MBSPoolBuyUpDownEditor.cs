// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.MBSPoolBuyUpDownEditor
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.Common;
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
  public class MBSPoolBuyUpDownEditor : UserControl
  {
    private MbsPoolBuyUpDownItems buyUpDownItems;
    private bool modified;
    private bool readOnly;
    private IContainer components;
    private GridView gvBuyUp;
    private ToolTip toolTip1;
    private GroupContainer gcBuyUp;
    private Panel panel1;
    private FlowLayoutPanel flowLayoutPanel1;
    private StandardIconButton siBtnDeleteUp;
    private StandardIconButton siBtnAddUp;
    private GroupContainer gcBuyDown;
    private Panel panel2;
    private GridView gvBuyDown;
    private FlowLayoutPanel flowLayoutPanel2;
    private StandardIconButton siBtnDeleteDown;
    private StandardIconButton siBtnAddDown;

    public event EventHandler BuyUpDownModified;

    private void OnBuyUpDownModified()
    {
      if (this.ValidateBuyUpDown())
      {
        this.CommitChanges();
        if (this.BuyUpDownModified != null)
          this.BuyUpDownModified((object) this, EventArgs.Empty);
      }
      this.modified = true;
    }

    public MBSPoolBuyUpDownEditor()
    {
      this.InitializeComponent();
      this.gvBuyUp.Sort(0, SortOrder.Ascending);
      this.gvBuyDown.Sort(0, SortOrder.Ascending);
    }

    public void CommitChanges()
    {
      this.gvBuyUp.StopEditing();
      this.gvBuyDown.StopEditing();
      if (!this.modified)
        return;
      this.buyUpDownItems.Clear();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvBuyUp.Items)
        this.buyUpDownItems.Add(gvItem.Tag as MbsPoolBuyUpDownItem, true);
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvBuyDown.Items)
        this.buyUpDownItems.Add(gvItem.Tag as MbsPoolBuyUpDownItem, false);
      this.modified = false;
    }

    public void ClearData()
    {
      this.buyUpDownItems.Clear();
      this.modified = true;
    }

    private bool ValidateItems(List<MbsPoolBuyUpDownItem> items)
    {
      for (int index = 0; index < items.Count<MbsPoolBuyUpDownItem>(); ++index)
      {
        MbsPoolBuyUpDownItem poolBuyUpDownItem = items[index];
        string text1 = poolBuyUpDownItem.ValidateDataFormat();
        if (text1 != string.Empty)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, text1, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        string text2 = poolBuyUpDownItem.ValidateDataRange();
        if (text2 != string.Empty)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, text2, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        if (index < items.Count - 1 && items[index + 1].GnrMin <= poolBuyUpDownItem.GnrMax)
        {
          if (poolBuyUpDownItem.IsBuyUp)
          {
            int num1 = (int) Utils.Dialog((IWin32Window) this, "Buy Up: The minimum and maximum note range cannot overlap existing note ranges.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
          else
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "Buy Down: The minimum and maximum note range cannot overlap existing note ranges.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
          return false;
        }
      }
      return true;
    }

    public bool ValidateBuyUpDown(bool popupMsg = true)
    {
      return this.gvBuyUp.Items.Count == 0 && this.gvBuyDown.Items.Count == 0 || this.ValidateItems(this.gvBuyUp.Items.Select<GVItem, MbsPoolBuyUpDownItem>((Func<GVItem, MbsPoolBuyUpDownItem>) (i => i.Tag as MbsPoolBuyUpDownItem)).OrderBy<MbsPoolBuyUpDownItem, Decimal>((Func<MbsPoolBuyUpDownItem, Decimal>) (i => i.GnrMin)).ToList<MbsPoolBuyUpDownItem>()) && this.ValidateItems(this.gvBuyDown.Items.Select<GVItem, MbsPoolBuyUpDownItem>((Func<GVItem, MbsPoolBuyUpDownItem>) (i => i.Tag as MbsPoolBuyUpDownItem)).OrderBy<MbsPoolBuyUpDownItem, Decimal>((Func<MbsPoolBuyUpDownItem, Decimal>) (i => i.GnrMin)).ToList<MbsPoolBuyUpDownItem>());
    }

    public bool ReadOnly
    {
      get => this.readOnly;
      set
      {
        if (this.readOnly == value)
          return;
        this.readOnly = value;
        this.gvBuyUp.SelectedItems.Clear();
        this.gvBuyDown.SelectedItems.Clear();
        this.siBtnAddUp.Enabled = !this.readOnly;
        this.siBtnAddDown.Enabled = !this.readOnly;
        this.siBtnDeleteUp.Enabled = false;
        this.siBtnDeleteDown.Enabled = false;
        this.gvBuyUp.Enabled = !this.readOnly;
        this.gvBuyDown.Enabled = !this.readOnly;
      }
    }

    public void MakeBtnInvisible()
    {
      this.siBtnAddUp.Visible = false;
      this.siBtnAddDown.Visible = false;
      this.siBtnDeleteUp.Visible = false;
      this.siBtnDeleteDown.Visible = false;
      this.ReadOnly = true;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public MbsPoolBuyUpDownItems BuyUpDownItems
    {
      get => this.buyUpDownItems;
      set
      {
        this.buyUpDownItems = value;
        if (value != null)
          this.loadBuyUpDownItems();
        else
          this.buyUpDownItems = new MbsPoolBuyUpDownItems();
      }
    }

    public bool DataModified => this.modified && !this.readOnly;

    private void loadBuyUpDownItems()
    {
      this.gvBuyUp.Items.Clear();
      this.gvBuyDown.Items.Clear();
      if (this.buyUpDownItems != null)
      {
        foreach (MbsPoolBuyUpDownItem buyUpDown in this.buyUpDownItems.Where<MbsPoolBuyUpDownItem>((Func<MbsPoolBuyUpDownItem, bool>) (i => i.IsBuyUp)))
          this.gvBuyUp.Items.Add(this.CreateGVItem(buyUpDown));
        foreach (MbsPoolBuyUpDownItem buyUpDown in this.buyUpDownItems.Where<MbsPoolBuyUpDownItem>((Func<MbsPoolBuyUpDownItem, bool>) (i => !i.IsBuyUp)))
          this.gvBuyDown.Items.Add(this.CreateGVItem(buyUpDown));
      }
      this.gvBuyUp.ReSort();
      this.gvBuyDown.ReSort();
      this.modified = false;
    }

    private GVItem CreateGVItem(MbsPoolBuyUpDownItem buyUpDown)
    {
      return new GVItem()
      {
        SubItems = {
          (object) buyUpDown.GnrMin.ToString("0.00000"),
          (object) buyUpDown.GnrMax.ToString("0.00000"),
          (object) buyUpDown.Ratio.ToString("0.00000")
        },
        Tag = (object) new MbsPoolBuyUpDownItem(buyUpDown)
      };
    }

    private void MBSPoolBuyUpDownEditor_Resize(object sender, EventArgs e)
    {
      this.siBtnAddUp.Left = Math.Max(0, this.ClientSize.Width - this.siBtnAddUp.Width);
      this.siBtnDeleteUp.Left = this.siBtnAddUp.Right;
      this.siBtnDeleteDown.Left = this.siBtnAddDown.Right;
      GroupContainer gcBuyUp1 = this.gcBuyUp;
      GroupContainer gcBuyDown = this.gcBuyDown;
      Size clientSize = this.ClientSize;
      int num1;
      int num2 = num1 = clientSize.Height / 2;
      gcBuyDown.Height = num1;
      int num3 = num2;
      gcBuyUp1.Height = num3;
      GroupContainer gcBuyUp2 = this.gcBuyUp;
      GroupContainer gcBuyUp3 = this.gcBuyUp;
      int val2 = this.ClientSize.Width - 10;
      int num4;
      int num5 = num4 = Math.Max(0, val2);
      gcBuyUp3.Width = num4;
      int num6 = num5;
      gcBuyUp2.Width = num6;
    }

    private void siBtnAdd_Click(object sender, EventArgs e)
    {
      bool flag1 = ((Control) sender).Name == "siBtnAddUp";
      MBSPoolBuyUpDownDialog poolBuyUpDownDialog = new MBSPoolBuyUpDownDialog();
      poolBuyUpDownDialog.IsBuyUp = flag1;
      int num1 = (int) poolBuyUpDownDialog.ShowDialog((IWin32Window) this);
      bool flag2 = false;
      if (num1 != 1)
        return;
      if (flag1)
      {
        string text = poolBuyUpDownDialog.BuyUpDownItem.ValidateDataRange();
        if (text != string.Empty)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          flag2 = true;
        }
      }
      else
      {
        string text = poolBuyUpDownDialog.BuyUpDownItem.ValidateDataRange();
        if (text != string.Empty)
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          flag2 = true;
        }
      }
      if (flag2)
        return;
      if (flag1)
      {
        this.gvBuyUp.Items.Add(this.CreateGVItem(poolBuyUpDownDialog.BuyUpDownItem));
        this.gvBuyUp.ReSort();
      }
      else
      {
        this.gvBuyDown.Items.Add(this.CreateGVItem(poolBuyUpDownDialog.BuyUpDownItem));
        this.gvBuyDown.ReSort();
      }
      this.modified = true;
      this.OnBuyUpDownModified();
      if (!poolBuyUpDownDialog.IsCreatingAnother)
        return;
      this.siBtnAdd_Click(sender, e);
    }

    private void gv_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (((Control) sender).Name == "gvBuyUp")
        this.siBtnDeleteUp.Enabled = this.gvBuyUp.SelectedItems.Count != 0;
      else
        this.siBtnDeleteDown.Enabled = this.gvBuyDown.SelectedItems.Count != 0;
    }

    private void siBtnDelete_Click(object sender, EventArgs e)
    {
      if (((Control) sender).Name == "siBtnDeleteUp")
      {
        this.gvBuyUp.CancelEditing();
        foreach (GVItem selectedItem in this.gvBuyUp.SelectedItems)
          this.gvBuyUp.Items.Remove(selectedItem);
        this.gvBuyUp.ReSort();
      }
      else
      {
        this.gvBuyDown.CancelEditing();
        foreach (GVItem selectedItem in this.gvBuyDown.SelectedItems)
          this.gvBuyDown.Items.Remove(selectedItem);
        this.gvBuyDown.ReSort();
      }
      this.modified = true;
      this.OnBuyUpDownModified();
    }

    private void subItemClick(object source, GVSubItemEventArgs e)
    {
    }

    private void gv_EditorClosing(object source, GVSubItemEditingEventArgs e)
    {
      if (e == null)
        return;
      int index = e.SubItem.Index;
      string text1 = e.EditorControl.Text;
      this.modified = true;
      string text2 = string.Empty;
      if (!(text1 != ""))
        return;
      MbsPoolBuyUpDownItem tag = (MbsPoolBuyUpDownItem) e.SubItem.Parent.Tag;
      tag.IsBuyUp = ((Control) source).Name == "gvBuyUp";
      switch (index)
      {
        case 0:
          tag.GnrMin = Utils.ParseDecimal((object) text1);
          text2 = tag.ValidateDataFormat("GnrMin", text1);
          break;
        case 1:
          tag.GnrMax = Utils.ParseDecimal((object) text1);
          text2 = tag.ValidateDataFormat("GnrMax", text1);
          break;
        case 2:
          tag.Ratio = Utils.ParseDecimal((object) text1);
          text2 = tag.ValidateDataFormat("Ratio", text1);
          break;
      }
      if (text2 != string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, text2, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        e.Cancel = true;
        e.EditorControl.Text = e.SubItem.Text;
      }
      else
      {
        string text3 = tag.ValidateDataRange();
        if (text3 != string.Empty)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, text3, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          e.Cancel = true;
          e.EditorControl.Text = e.SubItem.Text;
        }
        else
        {
          e.SubItem.Parent.Tag = (object) tag;
          ((GridView) source).CancelEditing();
          this.OnBuyUpDownModified();
        }
      }
    }

    private void gvBuy_SubItemEnter(object source, GVSubItemEventArgs e)
    {
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
      this.gvBuyUp = new GridView();
      this.toolTip1 = new ToolTip(this.components);
      this.siBtnDeleteUp = new StandardIconButton();
      this.siBtnAddUp = new StandardIconButton();
      this.siBtnDeleteDown = new StandardIconButton();
      this.siBtnAddDown = new StandardIconButton();
      this.gcBuyUp = new GroupContainer();
      this.panel1 = new Panel();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.gcBuyDown = new GroupContainer();
      this.panel2 = new Panel();
      this.gvBuyDown = new GridView();
      this.flowLayoutPanel2 = new FlowLayoutPanel();
      ((ISupportInitialize) this.siBtnDeleteUp).BeginInit();
      ((ISupportInitialize) this.siBtnAddUp).BeginInit();
      ((ISupportInitialize) this.siBtnDeleteDown).BeginInit();
      ((ISupportInitialize) this.siBtnAddDown).BeginInit();
      this.gcBuyUp.SuspendLayout();
      this.panel1.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.gcBuyDown.SuspendLayout();
      this.panel2.SuspendLayout();
      this.flowLayoutPanel2.SuspendLayout();
      this.SuspendLayout();
      gvColumn1.ActivatedEditorType = GVActivatedEditorType.TextBox;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SortMethod = GVSortMethod.Numeric;
      gvColumn1.Text = "GNR Min(%)";
      gvColumn1.TextAlignment = ContentAlignment.BottomCenter;
      gvColumn1.Width = 145;
      gvColumn2.ActivatedEditorType = GVActivatedEditorType.TextBox;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.SortMethod = GVSortMethod.Numeric;
      gvColumn2.Text = "GNR Max(%)";
      gvColumn2.TextAlignment = ContentAlignment.BottomCenter;
      gvColumn2.Width = 145;
      gvColumn3.ActivatedEditorType = GVActivatedEditorType.TextBox;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.SortMethod = GVSortMethod.Numeric;
      gvColumn3.Text = "Buy Up Ratio";
      gvColumn3.TextAlignment = ContentAlignment.BottomCenter;
      gvColumn3.Width = 145;
      this.gvBuyUp.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gvBuyUp.Dock = DockStyle.Fill;
      this.gvBuyUp.Location = new Point(0, 0);
      this.gvBuyUp.Name = "gvBuyUp";
      this.gvBuyUp.Size = new Size(446, 191);
      this.gvBuyUp.TabIndex = 1;
      this.gvBuyUp.SelectedIndexChanged += new EventHandler(this.gv_SelectedIndexChanged);
      this.gvBuyUp.EditorClosing += new GVSubItemEditingEventHandler(this.gv_EditorClosing);
      this.siBtnDeleteUp.BackColor = Color.Transparent;
      this.siBtnDeleteUp.Enabled = false;
      this.siBtnDeleteUp.Location = new Point(257, 3);
      this.siBtnDeleteUp.MouseDownImage = (Image) null;
      this.siBtnDeleteUp.Name = "siBtnDeleteUp";
      this.siBtnDeleteUp.Size = new Size(16, 16);
      this.siBtnDeleteUp.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.siBtnDeleteUp.TabIndex = 0;
      this.siBtnDeleteUp.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.siBtnDeleteUp, "Remove Buy Up");
      this.siBtnDeleteUp.Click += new EventHandler(this.siBtnDelete_Click);
      this.siBtnAddUp.BackColor = Color.Transparent;
      this.siBtnAddUp.Location = new Point(235, 3);
      this.siBtnAddUp.MouseDownImage = (Image) null;
      this.siBtnAddUp.Name = "siBtnAddUp";
      this.siBtnAddUp.Size = new Size(16, 16);
      this.siBtnAddUp.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.siBtnAddUp.TabIndex = 1;
      this.siBtnAddUp.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.siBtnAddUp, "Add Buy Up");
      this.siBtnAddUp.Click += new EventHandler(this.siBtnAdd_Click);
      this.siBtnDeleteDown.BackColor = Color.Transparent;
      this.siBtnDeleteDown.Enabled = false;
      this.siBtnDeleteDown.Location = new Point(257, 3);
      this.siBtnDeleteDown.MouseDownImage = (Image) null;
      this.siBtnDeleteDown.Name = "siBtnDeleteDown";
      this.siBtnDeleteDown.Size = new Size(16, 16);
      this.siBtnDeleteDown.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.siBtnDeleteDown.TabIndex = 0;
      this.siBtnDeleteDown.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.siBtnDeleteDown, "Remove Buy Down");
      this.siBtnDeleteDown.Click += new EventHandler(this.siBtnDelete_Click);
      this.siBtnAddDown.BackColor = Color.Transparent;
      this.siBtnAddDown.Location = new Point(235, 3);
      this.siBtnAddDown.MouseDownImage = (Image) null;
      this.siBtnAddDown.Name = "siBtnAddDown";
      this.siBtnAddDown.Size = new Size(16, 16);
      this.siBtnAddDown.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.siBtnAddDown.TabIndex = 1;
      this.siBtnAddDown.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.siBtnAddDown, "Add Buy Down");
      this.siBtnAddDown.Click += new EventHandler(this.siBtnAdd_Click);
      this.gcBuyUp.Controls.Add((Control) this.panel1);
      this.gcBuyUp.Controls.Add((Control) this.flowLayoutPanel1);
      this.gcBuyUp.Dock = DockStyle.Top;
      this.gcBuyUp.HeaderForeColor = SystemColors.ControlText;
      this.gcBuyUp.Location = new Point(0, 0);
      this.gcBuyUp.Name = "gcBuyUp";
      this.gcBuyUp.Size = new Size(448, 220);
      this.gcBuyUp.TabIndex = 2;
      this.gcBuyUp.Text = "Buy Up";
      this.panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panel1.Controls.Add((Control) this.gvBuyUp);
      this.panel1.Location = new Point(1, 25);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(446, 191);
      this.panel1.TabIndex = 3;
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.siBtnDeleteUp);
      this.flowLayoutPanel1.Controls.Add((Control) this.siBtnAddUp);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(171, 2);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(276, 22);
      this.flowLayoutPanel1.TabIndex = 0;
      this.gcBuyDown.Controls.Add((Control) this.panel2);
      this.gcBuyDown.Controls.Add((Control) this.flowLayoutPanel2);
      this.gcBuyDown.Dock = DockStyle.Top;
      this.gcBuyDown.HeaderForeColor = SystemColors.ControlText;
      this.gcBuyDown.Location = new Point(0, 220);
      this.gcBuyDown.Name = "gcBuyDown";
      this.gcBuyDown.Size = new Size(448, 220);
      this.gcBuyDown.TabIndex = 3;
      this.gcBuyDown.Text = "Buy Down";
      this.panel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panel2.Controls.Add((Control) this.gvBuyDown);
      this.panel2.Location = new Point(1, 25);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(446, 191);
      this.panel2.TabIndex = 3;
      gvColumn4.ActivatedEditorType = GVActivatedEditorType.TextBox;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column1";
      gvColumn4.SortMethod = GVSortMethod.Numeric;
      gvColumn4.Text = "GNR Min(%)";
      gvColumn4.TextAlignment = ContentAlignment.BottomCenter;
      gvColumn4.Width = 145;
      gvColumn5.ActivatedEditorType = GVActivatedEditorType.TextBox;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column2";
      gvColumn5.SortMethod = GVSortMethod.Numeric;
      gvColumn5.Text = "GNR Max(%)";
      gvColumn5.TextAlignment = ContentAlignment.BottomCenter;
      gvColumn5.Width = 145;
      gvColumn6.ActivatedEditorType = GVActivatedEditorType.TextBox;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column3";
      gvColumn6.SortMethod = GVSortMethod.Numeric;
      gvColumn6.Text = "Buy Down Ratio";
      gvColumn6.TextAlignment = ContentAlignment.BottomCenter;
      gvColumn6.Width = 145;
      this.gvBuyDown.Columns.AddRange(new GVColumn[3]
      {
        gvColumn4,
        gvColumn5,
        gvColumn6
      });
      this.gvBuyDown.Dock = DockStyle.Fill;
      this.gvBuyDown.Location = new Point(0, 0);
      this.gvBuyDown.Name = "gvBuyDown";
      this.gvBuyDown.Size = new Size(446, 191);
      this.gvBuyDown.TabIndex = 1;
      this.gvBuyDown.SelectedIndexChanged += new EventHandler(this.gv_SelectedIndexChanged);
      this.gvBuyDown.EditorClosing += new GVSubItemEditingEventHandler(this.gv_EditorClosing);
      this.flowLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel2.BackColor = Color.Transparent;
      this.flowLayoutPanel2.Controls.Add((Control) this.siBtnDeleteDown);
      this.flowLayoutPanel2.Controls.Add((Control) this.siBtnAddDown);
      this.flowLayoutPanel2.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel2.Location = new Point(171, 2);
      this.flowLayoutPanel2.Name = "flowLayoutPanel2";
      this.flowLayoutPanel2.Size = new Size(276, 22);
      this.flowLayoutPanel2.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcBuyDown);
      this.Controls.Add((Control) this.gcBuyUp);
      this.Name = nameof (MBSPoolBuyUpDownEditor);
      this.Size = new Size(448, 444);
      this.Resize += new EventHandler(this.MBSPoolBuyUpDownEditor_Resize);
      ((ISupportInitialize) this.siBtnDeleteUp).EndInit();
      ((ISupportInitialize) this.siBtnAddUp).EndInit();
      ((ISupportInitialize) this.siBtnDeleteDown).EndInit();
      ((ISupportInitialize) this.siBtnAddDown).EndInit();
      this.gcBuyUp.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      this.gcBuyDown.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.flowLayoutPanel2.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
