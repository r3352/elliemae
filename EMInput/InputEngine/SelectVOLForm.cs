// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.SelectVOLForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class SelectVOLForm : Form
  {
    private LoanData loan;
    private ArrayList selectedVOLs;
    private IContainer components;
    private Label label2;
    private Button okBtn;
    private Button cancelBtn;
    private CheckBox chkUseLoan;
    private GridView listViewLiabs;
    private GroupContainer groupContainer1;
    private StandardIconButton iconbtnDown;
    private StandardIconButton iconbtnUp;
    private GridView listViewPreview;
    private ToolTip toolTip1;

    public SelectVOLForm(LoanData loan, int location)
    {
      this.loan = loan;
      this.InitializeComponent();
      if (location == 1)
      {
        this.chkUseLoan.Visible = false;
        this.groupContainer1.Top = this.listViewLiabs.Top + this.listViewLiabs.Height - 1;
        this.okBtn.Top = this.cancelBtn.Top = this.groupContainer1.Top + this.groupContainer1.Height + 10;
        this.Height = this.okBtn.Top + this.okBtn.Height + 40;
      }
      this.initForm();
      this.listViewLiabs.SubItemCheck += new GVSubItemEventHandler(this.listViewLiabs_SubItemCheck);
      this.listViewPreview_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    private void initForm()
    {
      int exlcudingAlimonyJobExp = this.loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
      this.listViewLiabs.Items.Clear();
      this.listViewLiabs.BeginUpdate();
      for (int index = 1; index <= exlcudingAlimonyJobExp; ++index)
      {
        string str = "FL" + index.ToString("00");
        this.listViewLiabs.Items.Add(new GVItem(this.loan.GetSimpleField(str + "02"))
        {
          SubItems = {
            (object) this.loan.GetField(str + "08"),
            (object) this.loan.GetField(str + "13")
          },
          Tag = (object) Guid.NewGuid().ToString()
        });
      }
      this.listViewLiabs.EndUpdate();
      this.listViewLiabs.Sort(2, SortOrder.Descending);
    }

    public ArrayList SelectedVOLs => this.selectedVOLs;

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (this.getCheckedLiabilities() == 0 && !this.chkUseLoan.Checked)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You need to select at least one liability.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.selectedVOLs = new ArrayList();
        for (int nItemIndex = 0; nItemIndex < this.listViewPreview.Items.Count; ++nItemIndex)
          this.selectedVOLs.Add((object) new string[2]
          {
            this.listViewPreview.Items[nItemIndex].Text,
            this.listViewPreview.Items[nItemIndex].SubItems[1].Text
          });
        while (this.selectedVOLs.Count < 3)
          this.selectedVOLs.Add((object) new string[2]
          {
            "",
            ""
          });
        this.DialogResult = DialogResult.OK;
      }
    }

    private int getCheckedLiabilities()
    {
      int checkedLiabilities = 0;
      for (int nItemIndex = 0; nItemIndex < this.listViewLiabs.Items.Count; ++nItemIndex)
      {
        if (this.listViewLiabs.Items[nItemIndex].SubItems[0].Checked)
          ++checkedLiabilities;
      }
      return checkedLiabilities;
    }

    private void listViewLiabs_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      int checkedLiabilities = this.getCheckedLiabilities();
      if (checkedLiabilities > 3 || checkedLiabilities > 2 && this.chkUseLoan.Checked)
      {
        string text = "You only can select 3 liabilities.";
        if (this.chkUseLoan.Checked)
          text = "You only can select 2 liabilities if \"Map proposed loan to MLDS\" checkbox is checked.";
        int num = (int) Utils.Dialog((IWin32Window) this, text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        e.SubItem.Checked = false;
      }
      Hashtable hashtable = new Hashtable();
      for (int nItemIndex = 0; nItemIndex < this.listViewLiabs.Items.Count; ++nItemIndex)
      {
        if (this.listViewLiabs.Items[nItemIndex].SubItems[0].Checked && !hashtable.ContainsKey((object) this.listViewLiabs.Items[nItemIndex].Tag.ToString()))
          hashtable.Add((object) this.listViewLiabs.Items[nItemIndex].Tag.ToString(), (object) nItemIndex);
      }
      string empty = string.Empty;
      int count = this.listViewPreview.Items.Count;
      if (count > 0)
      {
        for (int nItemIndex = count - 1; nItemIndex >= 0; --nItemIndex)
        {
          string key = this.listViewPreview.Items[nItemIndex].Tag.ToString();
          if (!(key == "loan"))
          {
            if (!hashtable.ContainsKey((object) key))
              this.listViewPreview.Items.RemoveAt(nItemIndex);
            else
              hashtable.Remove((object) key);
          }
        }
      }
      if (hashtable != null && hashtable.Count > 0 && this.listViewPreview.Items.Count < 3)
      {
        foreach (DictionaryEntry dictionaryEntry in hashtable)
        {
          int nItemIndex = Utils.ParseInt((object) dictionaryEntry.Value.ToString());
          this.listViewPreview.Items.Add(new GVItem(this.listViewLiabs.Items[nItemIndex].SubItems[0].Text)
          {
            SubItems = {
              (object) this.listViewLiabs.Items[nItemIndex].SubItems[2].Text,
              (object) ""
            },
            Tag = this.listViewLiabs.Items[nItemIndex].Tag
          });
        }
      }
      this.resetOrder();
    }

    private void btnDown_Click(object sender, EventArgs e)
    {
      if (this.listViewPreview.SelectedItems.Count == 0)
        return;
      int index = this.listViewPreview.SelectedItems[0].Index;
      if (index + 1 >= this.listViewPreview.Items.Count)
        return;
      this.listViewPreview.BeginUpdate();
      GVItem gvItem = this.listViewPreview.Items[index];
      this.listViewPreview.Items.RemoveAt(index);
      this.listViewPreview.Items.Insert(index + 1, gvItem);
      gvItem.Selected = true;
      this.listViewPreview.EndUpdate();
      this.resetOrder();
    }

    private void btnUp_Click(object sender, EventArgs e)
    {
      if (this.listViewPreview.SelectedItems.Count == 0)
        return;
      int index = this.listViewPreview.SelectedItems[0].Index;
      if (index == 0)
        return;
      this.listViewPreview.BeginUpdate();
      GVItem gvItem = this.listViewPreview.Items[index];
      this.listViewPreview.Items.RemoveAt(index);
      this.listViewPreview.Items.Insert(index - 1, gvItem);
      gvItem.Selected = true;
      this.listViewPreview.EndUpdate();
      this.resetOrder();
    }

    private void chkUseLoan_Click(object sender, EventArgs e)
    {
      if (this.chkUseLoan.Checked)
      {
        int num1 = 0;
        for (int nItemIndex = 0; nItemIndex < this.listViewPreview.Items.Count; ++nItemIndex)
        {
          if (this.listViewPreview.Items[nItemIndex].Tag.ToString() == "loan")
            return;
          ++num1;
        }
        if (num1 >= 3)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "You only can select 2 liabilities if \"Map proposed loan to MLDS\" checkbox is checked. Please remove one liability before adding proposed loan to list.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.chkUseLoan.Checked = false;
          return;
        }
        this.listViewPreview.Items.Add(new GVItem(this.loan.GetField("1264"))
        {
          SubItems = {
            (object) Utils.ParseDouble((object) this.loan.GetSimpleField("1109")).ToString("N2"),
            (object) ""
          },
          Tag = (object) "loan"
        });
      }
      else
      {
        for (int nItemIndex = 0; nItemIndex < this.listViewPreview.Items.Count; ++nItemIndex)
        {
          if (this.listViewPreview.Items[nItemIndex].Tag.ToString() == "loan")
          {
            this.listViewPreview.Items.RemoveAt(nItemIndex);
            break;
          }
        }
      }
      this.resetOrder();
    }

    private void resetOrder()
    {
      for (int index = 1; index <= this.listViewPreview.Items.Count; ++index)
      {
        this.listViewPreview.Items[index - 1].SubItems[2].Text = index.ToString();
        if (index >= 3)
          break;
      }
    }

    private void listViewPreview_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.iconbtnUp.Enabled = this.iconbtnDown.Enabled = this.listViewPreview.SelectedItems.Count == 1;
    }

    private void SelectVOLForm_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\u001B')
        return;
      this.DialogResult = DialogResult.Cancel;
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
      this.label2 = new Label();
      this.okBtn = new Button();
      this.cancelBtn = new Button();
      this.chkUseLoan = new CheckBox();
      this.listViewLiabs = new GridView();
      this.groupContainer1 = new GroupContainer();
      this.iconbtnDown = new StandardIconButton();
      this.iconbtnUp = new StandardIconButton();
      this.listViewPreview = new GridView();
      this.toolTip1 = new ToolTip(this.components);
      this.groupContainer1.SuspendLayout();
      ((ISupportInitialize) this.iconbtnDown).BeginInit();
      ((ISupportInitialize) this.iconbtnUp).BeginInit();
      this.SuspendLayout();
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(10, 9);
      this.label2.Name = "label2";
      this.label2.Size = new Size(215, 14);
      this.label2.TabIndex = 13;
      this.label2.Text = "Please select liabilities to populate to MLDS:";
      this.okBtn.BackColor = SystemColors.Control;
      this.okBtn.Location = new Point(371, 374);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 23);
      this.okBtn.TabIndex = 14;
      this.okBtn.Text = "&OK";
      this.okBtn.UseVisualStyleBackColor = true;
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(452, 374);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 23);
      this.cancelBtn.TabIndex = 15;
      this.cancelBtn.Text = "&Cancel";
      this.chkUseLoan.AutoSize = true;
      this.chkUseLoan.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.chkUseLoan.Location = new Point(15, 240);
      this.chkUseLoan.Name = "chkUseLoan";
      this.chkUseLoan.Size = new Size(164, 18);
      this.chkUseLoan.TabIndex = 16;
      this.chkUseLoan.Text = "Map proposed loan to MLDS.";
      this.chkUseLoan.UseVisualStyleBackColor = true;
      this.chkUseLoan.Click += new EventHandler(this.chkUseLoan_Click);
      gvColumn1.CheckBoxes = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column2";
      gvColumn1.Text = "Creditor Name";
      gvColumn1.Width = 260;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column3";
      gvColumn2.Text = "Type";
      gvColumn2.Width = 125;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column4";
      gvColumn3.SortMethod = GVSortMethod.Numeric;
      gvColumn3.SpringToFit = true;
      gvColumn3.Text = "Balance";
      gvColumn3.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn3.Width = 128;
      this.listViewLiabs.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.listViewLiabs.Location = new Point(12, 25);
      this.listViewLiabs.Name = "listViewLiabs";
      this.listViewLiabs.Size = new Size(515, 204);
      this.listViewLiabs.TabIndex = 19;
      this.groupContainer1.Controls.Add((Control) this.iconbtnDown);
      this.groupContainer1.Controls.Add((Control) this.iconbtnUp);
      this.groupContainer1.Controls.Add((Control) this.listViewPreview);
      this.groupContainer1.Location = new Point(11, 263);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(516, 102);
      this.groupContainer1.TabIndex = 20;
      this.groupContainer1.Text = "Preview";
      this.iconbtnDown.BackColor = Color.Transparent;
      this.iconbtnDown.Location = new Point(494, 5);
      this.iconbtnDown.Name = "iconbtnDown";
      this.iconbtnDown.Size = new Size(16, 16);
      this.iconbtnDown.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.iconbtnDown.TabIndex = 2;
      this.iconbtnDown.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.iconbtnDown, "Move Down");
      this.iconbtnDown.Click += new EventHandler(this.btnDown_Click);
      this.iconbtnUp.BackColor = Color.Transparent;
      this.iconbtnUp.Location = new Point(474, 5);
      this.iconbtnUp.Name = "iconbtnUp";
      this.iconbtnUp.Size = new Size(16, 16);
      this.iconbtnUp.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.iconbtnUp.TabIndex = 1;
      this.iconbtnUp.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.iconbtnUp, "Move Up");
      this.iconbtnUp.Click += new EventHandler(this.btnUp_Click);
      this.listViewPreview.AllowMultiselect = false;
      this.listViewPreview.BorderStyle = BorderStyle.None;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column1";
      gvColumn4.Text = "Lienholder's Name";
      gvColumn4.Width = 239;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column2";
      gvColumn5.Text = "Amount Owing";
      gvColumn5.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn5.Width = 95;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column3";
      gvColumn6.SpringToFit = true;
      gvColumn6.Text = "Priority";
      gvColumn6.Width = 180;
      this.listViewPreview.Columns.AddRange(new GVColumn[3]
      {
        gvColumn4,
        gvColumn5,
        gvColumn6
      });
      this.listViewPreview.Dock = DockStyle.Fill;
      this.listViewPreview.Location = new Point(1, 26);
      this.listViewPreview.Name = "listViewPreview";
      this.listViewPreview.Size = new Size(514, 75);
      this.listViewPreview.SortOption = GVSortOption.None;
      this.listViewPreview.TabIndex = 0;
      this.listViewPreview.SelectedIndexChanged += new EventHandler(this.listViewPreview_SelectedIndexChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(539, 404);
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.listViewLiabs);
      this.Controls.Add((Control) this.chkUseLoan);
      this.Controls.Add((Control) this.okBtn);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.label2);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SelectVOLForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Select VOL";
      this.KeyPress += new KeyPressEventHandler(this.SelectVOLForm_KeyPress);
      this.groupContainer1.ResumeLayout(false);
      ((ISupportInitialize) this.iconbtnDown).EndInit();
      ((ISupportInitialize) this.iconbtnUp).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
