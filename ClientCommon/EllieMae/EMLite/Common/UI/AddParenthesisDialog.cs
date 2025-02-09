// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.AddParenthesisDialog
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class AddParenthesisDialog : Form
  {
    private const string className = "AddParenthesisDialog";
    private static readonly string sw = Tracing.SwImportExport;
    private GridView gvFilters;
    private IContainer components;
    private int leftIndex = -1;
    private Label label1;
    private int rightIndex = -1;
    private int[] leftParenCounts;
    private TableContainer tableContainer1;
    private FlowLayoutPanel flowLayoutPanel1;
    private StandardIconButton btnRemove;
    private StandardIconButton btnAdd;
    private DialogButtons dlgButtons;
    private ToolTip toolTip1;
    private Label lblPreview;
    private Label label2;
    private int[] rightParentCounts;

    public AddParenthesisDialog(FieldFilterList filters)
    {
      this.InitializeComponent();
      this.leftParenCounts = new int[filters.Count];
      this.rightParentCounts = new int[filters.Count];
      for (int index = 0; index < filters.Count; ++index)
      {
        this.leftParenCounts[index] = filters[index].LeftParentheses;
        this.rightParentCounts[index] = filters[index].RightParentheses;
        this.gvFilters.Items.Add(this.fieldFilterToGVItem(index + 1, filters[index]));
      }
      this.btnAdd.Enabled = false;
      this.btnRemove.Enabled = false;
      this.displayFormula();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private GVItem fieldFilterToGVItem(int index, FieldFilter filter)
    {
      return new GVItem(index.ToString())
      {
        SubItems = {
          (object) filter.LeftParenthesesToString,
          (object) filter.FieldDescription,
          (object) filter.OperatorDescription,
          (object) filter.ValueDescription,
          (object) filter.RightParenthesesToString,
          (object) filter.JointTokenToString
        },
        Tag = (object) filter
      };
    }

    private void refreshGVItem(GVItem item)
    {
      FieldFilter tag = (FieldFilter) item.Tag;
      item.SubItems[1].Text = tag.LeftParenthesesToString;
      item.SubItems[5].Text = tag.RightParenthesesToString;
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
      this.gvFilters = new GridView();
      this.label1 = new Label();
      this.tableContainer1 = new TableContainer();
      this.lblPreview = new Label();
      this.label2 = new Label();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.btnRemove = new StandardIconButton();
      this.btnAdd = new StandardIconButton();
      this.dlgButtons = new DialogButtons();
      this.toolTip1 = new ToolTip(this.components);
      this.tableContainer1.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      ((ISupportInitialize) this.btnRemove).BeginInit();
      ((ISupportInitialize) this.btnAdd).BeginInit();
      this.SuspendLayout();
      this.gvFilters.AllowMultiselect = false;
      this.gvFilters.BorderStyle = BorderStyle.None;
      gvColumn1.CheckBoxes = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "";
      gvColumn1.Width = 40;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "(";
      gvColumn2.Width = 35;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Field";
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "Operator";
      gvColumn4.Width = 100;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.Text = "Value";
      gvColumn5.Width = 80;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column6";
      gvColumn6.Text = ")";
      gvColumn6.Width = 32;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column7";
      gvColumn7.Text = "Joint";
      gvColumn7.Width = 100;
      this.gvFilters.Columns.AddRange(new GVColumn[7]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7
      });
      this.gvFilters.Dock = DockStyle.Fill;
      this.gvFilters.Location = new Point(1, 26);
      this.gvFilters.Name = "gvFilters";
      this.gvFilters.Size = new Size(514, 279);
      this.gvFilters.SortOption = GVSortOption.None;
      this.gvFilters.TabIndex = 0;
      this.gvFilters.SubItemCheck += new GVSubItemEventHandler(this.gvFilters_ItemCheck);
      this.label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label1.Location = new Point(10, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(516, 20);
      this.label1.TabIndex = 47;
      this.label1.Text = "To group filters, select the first and the last filter for the group, and then click the New icon.";
      this.tableContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tableContainer1.Controls.Add((Control) this.lblPreview);
      this.tableContainer1.Controls.Add((Control) this.label2);
      this.tableContainer1.Controls.Add((Control) this.flowLayoutPanel1);
      this.tableContainer1.Controls.Add((Control) this.gvFilters);
      this.tableContainer1.Location = new Point(10, 31);
      this.tableContainer1.Name = "tableContainer1";
      this.tableContainer1.Size = new Size(516, 331);
      this.tableContainer1.TabIndex = 49;
      this.tableContainer1.Text = "Add/Edit Parentheses";
      this.lblPreview.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lblPreview.AutoEllipsis = true;
      this.lblPreview.BackColor = Color.Transparent;
      this.lblPreview.Location = new Point(58, 311);
      this.lblPreview.Name = "lblPreview";
      this.lblPreview.Size = new Size(452, 14);
      this.lblPreview.TabIndex = 3;
      this.label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.Location = new Point(8, 311);
      this.label2.Name = "label2";
      this.label2.Size = new Size(50, 14);
      this.label2.TabIndex = 2;
      this.label2.Text = "Preview:";
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnRemove);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnAdd);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(134, 2);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(376, 22);
      this.flowLayoutPanel1.TabIndex = 1;
      this.btnRemove.BackColor = Color.Transparent;
      this.btnRemove.Location = new Point(357, 3);
      this.btnRemove.Name = "btnRemove";
      this.btnRemove.Size = new Size(16, 16);
      this.btnRemove.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnRemove.TabIndex = 0;
      this.btnRemove.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnRemove, "Remove Parentheses");
      this.btnRemove.Click += new EventHandler(this.btnRemove_Click);
      this.btnAdd.BackColor = Color.Transparent;
      this.btnAdd.Location = new Point(335, 3);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(16, 16);
      this.btnAdd.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAdd.TabIndex = 1;
      this.btnAdd.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnAdd, "Add Parentheses");
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.dlgButtons.Dock = DockStyle.Bottom;
      this.dlgButtons.Location = new Point(0, 363);
      this.dlgButtons.Name = "dlgButtons";
      this.dlgButtons.Size = new Size(536, 44);
      this.dlgButtons.TabIndex = 50;
      this.dlgButtons.OK += new EventHandler(this.doneBtn_Click);
      this.AcceptButton = (IButtonControl) this.dlgButtons;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(536, 407);
      this.Controls.Add((Control) this.dlgButtons);
      this.Controls.Add((Control) this.tableContainer1);
      this.Controls.Add((Control) this.label1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MinimizeBox = false;
      this.Name = nameof (AddParenthesisDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Edit Parentheses";
      this.FormClosing += new FormClosingEventHandler(this.AddParenthesisDialog_FormClosing);
      this.tableContainer1.ResumeLayout(false);
      this.tableContainer1.PerformLayout();
      this.flowLayoutPanel1.ResumeLayout(false);
      ((ISupportInitialize) this.btnRemove).EndInit();
      ((ISupportInitialize) this.btnAdd).EndInit();
      this.ResumeLayout(false);
    }

    private void displayFormula()
    {
      string str = "";
      for (int index = 1; index <= this.gvFilters.Items.Count; ++index)
      {
        FieldFilter tag = (FieldFilter) this.gvFilters.Items[index - 1].Tag;
        if (tag != null)
        {
          if (tag.LeftParentheses > 0)
            str = str + tag.LeftParenthesesToString + " ";
          str = str + index.ToString() + " ";
          if (tag.RightParentheses > 0)
            str = str + tag.RightParenthesesToString + " ";
          if (index < this.gvFilters.Items.Count)
            str = str + tag.JointTokenToString + " ";
        }
      }
      this.lblPreview.Text = str.Trim();
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      if (this.leftIndex == -1)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a criterion first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.leftIndex == this.rightIndex)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "You can't add parenthesis to the same criterion.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.getParenthesisCountForCurrentPair() < 0)
      {
        int num3 = (int) Utils.Dialog((IWin32Window) this, "Parentheses cannot be added because the select items belong to different subexpressions.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        FieldFilter tag1 = (FieldFilter) this.gvFilters.Items[this.leftIndex].Tag;
        FieldFilter tag2 = (FieldFilter) this.gvFilters.Items[this.rightIndex].Tag;
        ++tag1.LeftParentheses;
        ++tag2.RightParentheses;
        this.refreshGVItem(this.gvFilters.Items[this.leftIndex]);
        this.refreshGVItem(this.gvFilters.Items[this.rightIndex]);
        this.displayFormula();
        this.clearOutChecks();
      }
    }

    private void btnRemove_Click(object sender, EventArgs e)
    {
      if (this.leftIndex == -1)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select an criterion first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        FieldFilter tag1 = (FieldFilter) this.gvFilters.Items[this.leftIndex].Tag;
        FieldFilter tag2 = (FieldFilter) this.gvFilters.Items[this.rightIndex].Tag;
        if (tag1.LeftParentheses < 1)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "There is no left parenthesis to remove.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else if (tag2.RightParentheses < 1)
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, "There is no right parenthesis to remove.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else if (this.getParenthesisCountForCurrentPair() < 1)
        {
          int num4 = (int) Utils.Dialog((IWin32Window) this, "The selected items do not have a pair of matching parentheses.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          --tag1.LeftParentheses;
          --tag2.RightParentheses;
          this.refreshGVItem(this.gvFilters.Items[this.leftIndex]);
          this.refreshGVItem(this.gvFilters.Items[this.rightIndex]);
          this.displayFormula();
          this.clearOutChecks();
        }
      }
    }

    private int getParenthesisCountForCurrentPair()
    {
      FieldFilter tag1 = (FieldFilter) this.gvFilters.Items[this.leftIndex].Tag;
      FieldFilter tag2 = (FieldFilter) this.gvFilters.Items[this.rightIndex].Tag;
      int val2_1 = 0;
      int val1 = tag1.LeftParentheses;
      int val2_2 = 0;
      int num = tag2.RightParentheses;
      for (int index = 0; index < this.rightIndex - this.leftIndex; ++index)
      {
        FieldFilter tag3 = (FieldFilter) this.gvFilters.Items[this.leftIndex + index].Tag;
        FieldFilter tag4 = (FieldFilter) this.gvFilters.Items[this.rightIndex - index].Tag;
        val2_1 += tag3.LeftParentheses - tag3.RightParentheses;
        val2_2 += tag4.RightParentheses - tag4.LeftParentheses;
        val1 = Math.Min(val1, val2_1);
        num = Math.Min(num, val2_2);
        if (val2_1 < 0)
          return val2_1;
        if (val2_2 < 0)
          return val2_2;
      }
      return Math.Min(val1, num);
    }

    private void clearOutChecks()
    {
      this.gvFilters.BeginUpdate();
      for (int nItemIndex = 0; nItemIndex < this.gvFilters.Items.Count; ++nItemIndex)
      {
        if (this.gvFilters.Items[nItemIndex].Checked)
          this.gvFilters.Items[nItemIndex].Checked = false;
      }
      this.gvFilters.EndUpdate();
      this.btnAdd.Enabled = false;
      this.btnRemove.Enabled = false;
    }

    private void gvFilters_ItemCheck(object sender, GVSubItemEventArgs e)
    {
      this.leftIndex = -1;
      this.rightIndex = -1;
      if (e.SubItem.Checked)
      {
        int num = 0;
        for (int nItemIndex = 0; nItemIndex < this.gvFilters.Items.Count; ++nItemIndex)
        {
          if (nItemIndex != e.SubItem.Item.Index)
          {
            if (this.gvFilters.Items[nItemIndex].Checked)
              ++num;
            if (num > 1)
              this.gvFilters.Items[nItemIndex].Checked = false;
          }
        }
      }
      for (int nItemIndex = 0; nItemIndex < this.gvFilters.Items.Count; ++nItemIndex)
      {
        if (this.gvFilters.Items[nItemIndex].Checked && nItemIndex != e.SubItem.Item.Index || nItemIndex == e.SubItem.Item.Index && e.SubItem.Checked)
        {
          if (this.leftIndex == -1)
            this.leftIndex = nItemIndex;
          else if (this.rightIndex == -1)
            this.rightIndex = nItemIndex;
        }
      }
      if (this.leftIndex == -1 || this.rightIndex == -1)
      {
        this.btnAdd.Enabled = false;
        this.btnRemove.Enabled = false;
      }
      else
      {
        this.btnAdd.Enabled = true;
        this.btnRemove.Enabled = true;
      }
    }

    private void doneBtn_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    private void AddParenthesisDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this.DialogResult != DialogResult.Cancel)
        return;
      if (Utils.Dialog((IWin32Window) this, "All changes made to the filter will be discarded.", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
      {
        this.DialogResult = DialogResult.None;
        e.Cancel = true;
      }
      else
      {
        for (int nItemIndex = 0; nItemIndex < this.gvFilters.Items.Count; ++nItemIndex)
        {
          FieldFilter tag = (FieldFilter) this.gvFilters.Items[nItemIndex].Tag;
          tag.LeftParentheses = this.leftParenCounts[nItemIndex];
          tag.RightParentheses = this.rightParentCounts[nItemIndex];
        }
      }
    }
  }
}
