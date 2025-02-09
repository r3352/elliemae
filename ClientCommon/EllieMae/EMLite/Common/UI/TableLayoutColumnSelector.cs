// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.TableLayoutColumnSelector
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class TableLayoutColumnSelector : Form
  {
    public const int MaxColumns = 50;
    private int requiredColumnCount = 1;
    private TableLayout selectedLayout;
    private string lastFind;
    private IContainer components;
    private GridView gvColumns;
    private TextBox txtFind;
    private Button btnFind;
    private DialogButtons dlgButtons;
    private GroupContainer groupContainer1;
    private FlowLayoutPanel flowLayoutPanel1;
    private StandardIconButton btnMoveDown;
    private StandardIconButton btnMoveUp;
    private ToolTip toolTip1;

    public bool IsGlobalSearchOn { get; set; }

    public TableLayoutColumnSelector(TableLayout fullTableLayout, TableLayout currentLayout)
      : this(fullTableLayout, currentLayout, (TableLayout) null)
    {
    }

    public TableLayoutColumnSelector(
      TableLayout fullTableLayout,
      TableLayout currentLayout,
      TableLayout nonCustomizableColumnLayout,
      bool isGlobalSearchOn = false)
    {
      this.InitializeComponent();
      this.IsGlobalSearchOn = isGlobalSearchOn;
      this.gvColumns.Items.Clear();
      if (currentLayout != null)
      {
        foreach (TableLayout.Column column in currentLayout)
        {
          if (!this.isNonCustomizableColumn(column, nonCustomizableColumnLayout))
            this.gvColumns.Items.Add(new GVItem((object) column)
            {
              Checked = true
            });
        }
      }
      foreach (TableLayout.Column column in fullTableLayout)
      {
        if (!this.isNonCustomizableColumn(column, nonCustomizableColumnLayout) && (currentLayout == null || currentLayout.GetColumnByID(column.ColumnID) == null))
          this.gvColumns.Items.Add(new GVItem((object) column));
      }
    }

    public int RequiredColumnCount
    {
      get => this.requiredColumnCount;
      set => this.requiredColumnCount = value;
    }

    public TableLayout SelectedLayout => this.selectedLayout;

    private void btnMoveUp_Click(object sender, EventArgs e)
    {
      int index = this.gvColumns.SelectedItems[0].Index;
      if (index <= 0)
        return;
      this.moveItemUp(index);
    }

    private void moveItemUp(int index)
    {
      TableLayout.Column column = (TableLayout.Column) this.gvColumns.Items[index].SubItems[0].Value;
      bool flag = this.gvColumns.Items[index].Checked;
      int nIndex = index - 1;
      if (flag)
      {
        while (nIndex > 0 && !this.gvColumns.Items[nIndex - 1].Checked)
          --nIndex;
      }
      GVItem gvItem = new GVItem((object) column);
      this.gvColumns.Items.RemoveAt(index);
      this.gvColumns.Items.Insert(nIndex, gvItem);
      gvItem.Checked = flag;
      gvItem.Selected = true;
      this.gvColumns.EnsureVisible(gvItem.Index);
      this.btnMoveUp.Focus();
    }

    private void btnMoveDown_Click(object sender, EventArgs e)
    {
      int index = this.gvColumns.SelectedItems[0].Index;
      if (index >= this.gvColumns.Items.Count - 1)
        return;
      this.moveItemDown(index);
    }

    private void moveItemDown(int index)
    {
      TableLayout.Column column = (TableLayout.Column) this.gvColumns.Items[index].SubItems[0].Value;
      bool flag = this.gvColumns.Items[index].Checked;
      int nIndex = index + 1;
      if (!flag)
      {
        while (nIndex < this.gvColumns.Items.Count - 1 && this.gvColumns.Items[nIndex + 1].Checked)
          ++nIndex;
      }
      GVItem gvItem = new GVItem((object) column);
      this.gvColumns.Items.RemoveAt(index);
      this.gvColumns.Items.Insert(nIndex, gvItem);
      gvItem.Checked = flag;
      gvItem.Selected = true;
      this.gvColumns.EnsureVisible(gvItem.Index);
      this.btnMoveUp.Focus();
    }

    private void gvColumns_SelectedIndexChanged(object sender, EventArgs e)
    {
      int count = this.gvColumns.SelectedItems.Count;
      this.btnMoveUp.Enabled = count > 0 && this.gvColumns.SelectedItems[0].Index > 0;
      this.btnMoveDown.Enabled = count > 0 && this.gvColumns.SelectedItems[0].Index < this.gvColumns.Items.Count - 1;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.IsGlobalSearchOn && Utils.GlobalSearchFields.Intersect<string>(this.gvColumns.Items.Where<GVItem>((Func<GVItem, bool>) (q => q.Checked)).Select<GVItem, string>((Func<GVItem, string>) (n => n.Text))).Count<string>() <= 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "To use Global Search, one of the following columns must be present in your view : Loan Number, Borrower First Name, Loan Name, Borrower Last Name, Subject Property Address", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        GVItem[] checkedItems = this.gvColumns.GetCheckedItems(0);
        if (checkedItems.Length < this.requiredColumnCount)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "You must select at least " + (object) this.requiredColumnCount + " column(s) before saving your changes.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
        {
          this.buildSelectedLayout(checkedItems);
          this.DialogResult = DialogResult.OK;
        }
      }
    }

    private void buildSelectedLayout(GVItem[] checkedItems)
    {
      List<TableLayout.Column> columnList = new List<TableLayout.Column>();
      for (int index = 0; index < checkedItems.Length; ++index)
      {
        TableLayout.Column column = (TableLayout.Column) ((TableLayout.Column) checkedItems[index].SubItems[0].Value).Clone();
        if (column.ColumnID == "Loan.LinkGUID")
          column.Width = 50;
        column.DisplayOrder = -1;
        columnList.Add(column);
      }
      this.selectedLayout = new TableLayout(columnList.ToArray());
    }

    private void gvColumns_ItemCheck(object sender, GVSubItemEventArgs e)
    {
      if (e.SubItem.Checked)
      {
        if (this.gvColumns.GetCheckedItems(0).Length <= 50)
          return;
        int num = (int) Utils.Dialog((IWin32Window) this, "You have reached the maximum number of columns permitted (" + (object) 50 + "). To add this column, you must first deselect a column from the list.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        e.SubItem.Checked = false;
      }
      else
      {
        if (!((TableLayout.Column) e.SubItem.Value).Required)
          return;
        int num = (int) Utils.Dialog((IWin32Window) this, "This field is required and cannot be removed.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        e.SubItem.Checked = true;
      }
    }

    private void btnFind_Click(object sender, EventArgs e)
    {
      if (this.txtFind.Text == "")
        return;
      this.findNext(this.txtFind.Text);
    }

    private void findNext(string text)
    {
      int num1 = 0;
      int num2 = -1;
      if (this.gvColumns.SelectedItems.Count > 0)
        num2 = this.gvColumns.SelectedItems[0].Index;
      if (text == this.lastFind && num2 >= 0)
        num1 = num2 + 1;
      for (int index = num1; index < this.gvColumns.Items.Count; ++index)
      {
        if (this.gvColumns.Items[index].Text.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) >= 0)
        {
          this.lastFind = text;
          this.gvColumns.Items[index].Selected = true;
          this.gvColumns.EnsureVisible(index);
          this.gvColumns.Focus();
          return;
        }
      }
      if (this.lastFind == text)
      {
        int num3 = (int) Utils.Dialog((IWin32Window) this, "No more matches were found for '" + text + "'.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        int num4 = (int) Utils.Dialog((IWin32Window) this, "No matches were found for '" + text + "'.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      this.lastFind = (string) null;
    }

    private bool isNonCustomizableColumn(
      TableLayout.Column column,
      TableLayout nonCustomizableColumnLayout)
    {
      return nonCustomizableColumnLayout != null && nonCustomizableColumnLayout.Where<TableLayout.Column>((Func<TableLayout.Column, bool>) (c => c.ColumnID == column.ColumnID || c.Description == column.Description)).FirstOrDefault<TableLayout.Column>() != null;
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
      GVColumn gvColumn = new GVColumn();
      this.toolTip1 = new ToolTip(this.components);
      this.btnMoveDown = new StandardIconButton();
      this.btnMoveUp = new StandardIconButton();
      this.btnFind = new Button();
      this.groupContainer1 = new GroupContainer();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.txtFind = new TextBox();
      this.gvColumns = new GridView();
      this.dlgButtons = new DialogButtons();
      ((ISupportInitialize) this.btnMoveDown).BeginInit();
      ((ISupportInitialize) this.btnMoveUp).BeginInit();
      this.groupContainer1.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      this.btnMoveDown.BackColor = Color.Transparent;
      this.btnMoveDown.Enabled = false;
      this.btnMoveDown.Location = new Point(33, 3);
      this.btnMoveDown.Margin = new Padding(2, 3, 3, 3);
      this.btnMoveDown.Name = "btnMoveDown";
      this.btnMoveDown.Size = new Size(16, 16);
      this.btnMoveDown.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.btnMoveDown.TabIndex = 0;
      this.btnMoveDown.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnMoveDown, "Move Down");
      this.btnMoveDown.Click += new EventHandler(this.btnMoveDown_Click);
      this.btnMoveUp.BackColor = Color.Transparent;
      this.btnMoveUp.Enabled = false;
      this.btnMoveUp.Location = new Point(12, 3);
      this.btnMoveUp.Name = "btnMoveUp";
      this.btnMoveUp.Size = new Size(16, 16);
      this.btnMoveUp.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.btnMoveUp.TabIndex = 1;
      this.btnMoveUp.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnMoveUp, "Move Up");
      this.btnMoveUp.Click += new EventHandler(this.btnMoveUp_Click);
      this.btnFind.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnFind.BackColor = SystemColors.Control;
      this.btnFind.Location = new Point(279, 32);
      this.btnFind.Name = "btnFind";
      this.btnFind.Size = new Size(49, 22);
      this.btnFind.TabIndex = 2;
      this.btnFind.Text = "&Find";
      this.btnFind.UseVisualStyleBackColor = true;
      this.btnFind.Click += new EventHandler(this.btnFind_Click);
      this.groupContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer1.Controls.Add((Control) this.flowLayoutPanel1);
      this.groupContainer1.Controls.Add((Control) this.txtFind);
      this.groupContainer1.Controls.Add((Control) this.gvColumns);
      this.groupContainer1.Controls.Add((Control) this.btnFind);
      this.groupContainer1.Location = new Point(8, 6);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(338, 297);
      this.groupContainer1.TabIndex = 7;
      this.groupContainer1.Text = "Selected Columns";
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnMoveDown);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnMoveUp);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(282, 2);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(52, 22);
      this.flowLayoutPanel1.TabIndex = 4;
      this.txtFind.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtFind.Location = new Point(10, 33);
      this.txtFind.MaxLength = 200;
      this.txtFind.Name = "txtFind";
      this.txtFind.Size = new Size(266, 20);
      this.txtFind.TabIndex = 1;
      this.gvColumns.AllowMultiselect = false;
      this.gvColumns.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      gvColumn.CheckBoxes = true;
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "Column1";
      gvColumn.SpringToFit = true;
      gvColumn.Text = "Column";
      gvColumn.Width = 316;
      this.gvColumns.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.gvColumns.HeaderHeight = 0;
      this.gvColumns.HeaderVisible = false;
      this.gvColumns.Location = new Point(10, 57);
      this.gvColumns.Name = "gvColumns";
      this.gvColumns.Size = new Size(318, 231);
      this.gvColumns.TabIndex = 3;
      this.gvColumns.SubItemCheck += new GVSubItemEventHandler(this.gvColumns_ItemCheck);
      this.gvColumns.SelectedIndexChanged += new EventHandler(this.gvColumns_SelectedIndexChanged);
      this.dlgButtons.Dock = DockStyle.Bottom;
      this.dlgButtons.Location = new Point(0, 304);
      this.dlgButtons.Name = "dlgButtons";
      this.dlgButtons.Size = new Size(355, 41);
      this.dlgButtons.TabIndex = 6;
      this.dlgButtons.OK += new EventHandler(this.btnOK_Click);
      this.AcceptButton = (IButtonControl) this.btnFind;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(355, 345);
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.dlgButtons);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (TableLayoutColumnSelector);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Customize Columns";
      ((ISupportInitialize) this.btnMoveDown).EndInit();
      ((ISupportInitialize) this.btnMoveUp).EndInit();
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.flowLayoutPanel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
