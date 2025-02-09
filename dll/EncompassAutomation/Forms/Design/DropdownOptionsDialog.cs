// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Design.DropdownOptionsDialog
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.Encompass.Forms.Design
{
  public class DropdownOptionsDialog : System.Windows.Forms.Form
  {
    private DropdownOptionCollection options;
    private bool copyToValue;
    private ListViewEx lvwOptions;
    private ColumnHeader columnHeader1;
    private ColumnHeader columnHeader2;
    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.Button btnUp;
    private System.Windows.Forms.Button btnDown;
    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.TextBox txtText;
    private System.Windows.Forms.TextBox txtValue;
    private System.Windows.Forms.Button btnAdd;
    private System.Windows.Forms.TextBox txtValueEditor;
    private System.Windows.Forms.CheckBox chkEmpty;
    private System.Windows.Forms.Label lblText;
    private System.Windows.Forms.Label lblValue;
    private System.Windows.Forms.Label lblInstructions;
    private System.ComponentModel.Container components;

    public DropdownOptionsDialog(DropdownOptionCollection options)
    {
      this.InitializeComponent();
      this.options = options;
      this.chkEmpty.Visible = options.AllowEmptyValues;
      if (!options.AllowEditValues)
      {
        this.lblText.Visible = false;
        this.lblValue.Visible = false;
        this.txtValue.Visible = false;
        this.txtText.Visible = false;
        this.btnAdd.Visible = false;
        this.btnDelete.Visible = false;
        ((System.Windows.Forms.Control) this.lvwOptions).Height = ((System.Windows.Forms.Control) this.lvwOptions).Bottom - this.txtValue.Top;
        ((System.Windows.Forms.Control) this.lvwOptions).Top = this.txtValue.Top;
        this.lblInstructions.Text = "Double-click the Text of an existing item to edit.";
      }
      if (!options.AllowRearrangeValues)
      {
        this.btnUp.Visible = false;
        this.btnDown.Visible = false;
      }
      if (!options.AllowEmptyValues && !options.AllowEditValues && !options.AllowRearrangeValues)
      {
        this.btnOK.Enabled = false;
        this.btnOK.Visible = false;
        this.btnCancel.Text = "&Close";
      }
      foreach (DropdownOption option in options)
      {
        if (option.Value == "" && option.Text == "")
          this.chkEmpty.Checked = true;
        else
          ((ListView) this.lvwOptions).Items.Add(new ListViewItem(new string[2]
          {
            option.Text,
            option.Value
          }));
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
      this.lvwOptions = new ListViewEx();
      this.columnHeader1 = new ColumnHeader();
      this.columnHeader2 = new ColumnHeader();
      this.btnDelete = new System.Windows.Forms.Button();
      this.btnUp = new System.Windows.Forms.Button();
      this.btnDown = new System.Windows.Forms.Button();
      this.btnOK = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.txtText = new System.Windows.Forms.TextBox();
      this.lblText = new System.Windows.Forms.Label();
      this.lblValue = new System.Windows.Forms.Label();
      this.txtValue = new System.Windows.Forms.TextBox();
      this.btnAdd = new System.Windows.Forms.Button();
      this.txtValueEditor = new System.Windows.Forms.TextBox();
      this.lblInstructions = new System.Windows.Forms.Label();
      this.chkEmpty = new System.Windows.Forms.CheckBox();
      this.SuspendLayout();
      ((ListView) this.lvwOptions).AllowColumnReorder = true;
      ((System.Windows.Forms.Control) this.lvwOptions).Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      ((ListView) this.lvwOptions).Columns.AddRange(new ColumnHeader[2]
      {
        this.columnHeader1,
        this.columnHeader2
      });
      this.lvwOptions.DoubleClickActivation = true;
      ((ListView) this.lvwOptions).FullRowSelect = true;
      ((ListView) this.lvwOptions).HeaderStyle = ColumnHeaderStyle.Nonclickable;
      ((ListView) this.lvwOptions).HideSelection = false;
      ((System.Windows.Forms.Control) this.lvwOptions).Location = new Point(14, 60);
      ((ListView) this.lvwOptions).MultiSelect = false;
      ((System.Windows.Forms.Control) this.lvwOptions).Name = "lvwOptions";
      ((System.Windows.Forms.Control) this.lvwOptions).Size = new Size(326, 148);
      ((System.Windows.Forms.Control) this.lvwOptions).TabIndex = 4;
      ((ListView) this.lvwOptions).View = View.Details;
      this.lvwOptions.SubItemBeginEditing += new SubItemEventHandler(this.lvwOptions_SubItemBeginEditing);
      this.lvwOptions.SubItemEndEditing += new SubItemEndEditingEventHandler(this.lvwOptions_SubItemEndEditing);
      this.lvwOptions.SubItemClicked += new SubItemEventHandler(this.lvwOptions_SubItemClicked);
      this.columnHeader1.Text = "Text";
      this.columnHeader1.Width = 161;
      this.columnHeader2.Text = "Value";
      this.columnHeader2.Width = 141;
      this.btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDelete.Location = new Point(350, 60);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(60, 23);
      this.btnDelete.TabIndex = 5;
      this.btnDelete.Text = "&Delete";
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.btnUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnUp.Location = new Point(350, 87);
      this.btnUp.Name = "btnUp";
      this.btnUp.Size = new Size(60, 23);
      this.btnUp.TabIndex = 3;
      this.btnUp.Text = "&Up";
      this.btnUp.Click += new EventHandler(this.btnUp_Click);
      this.btnDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDown.Location = new Point(350, 115);
      this.btnDown.Name = "btnDown";
      this.btnDown.Size = new Size(60, 23);
      this.btnDown.TabIndex = 4;
      this.btnDown.Text = "Dow&n";
      this.btnDown.Click += new EventHandler(this.btnDown_Click);
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point((int) byte.MaxValue, 240);
      this.btnOK.Name = "btnOK";
      this.btnOK.TabIndex = 6;
      this.btnOK.Text = "&OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(335, 240);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.TabIndex = 7;
      this.btnCancel.Text = "&Cancel";
      this.txtText.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtText.Location = new Point(14, 34);
      this.txtText.Name = "txtText";
      this.txtText.Size = new Size(162, 20);
      this.txtText.TabIndex = 0;
      this.txtText.Text = "";
      this.txtText.KeyPress += new KeyPressEventHandler(this.txtText_KeyPress);
      this.txtText.TextChanged += new EventHandler(this.txtText_TextChanged);
      this.lblText.Location = new Point(14, 18);
      this.lblText.Name = "lblText";
      this.lblText.Size = new Size(100, 16);
      this.lblText.TabIndex = 7;
      this.lblText.Text = "Text:";
      this.lblValue.Location = new Point(178, 18);
      this.lblValue.Name = "lblValue";
      this.lblValue.Size = new Size(100, 16);
      this.lblValue.TabIndex = 9;
      this.lblValue.Text = "Value:";
      this.txtValue.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtValue.Location = new Point(178, 34);
      this.txtValue.Name = "txtValue";
      this.txtValue.Size = new Size(162, 20);
      this.txtValue.TabIndex = 1;
      this.txtValue.Text = "";
      this.btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAdd.Location = new Point(350, 32);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(60, 22);
      this.btnAdd.TabIndex = 2;
      this.btnAdd.Text = "&Add";
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.txtValueEditor.Location = new Point(308, 6);
      this.txtValueEditor.Name = "txtValueEditor";
      this.txtValueEditor.TabIndex = 10;
      this.txtValueEditor.Text = "";
      this.txtValueEditor.Visible = false;
      this.lblInstructions.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblInstructions.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Italic, GraphicsUnit.Point, (byte) 0);
      this.lblInstructions.Location = new Point(14, 209);
      this.lblInstructions.Name = "lblInstructions";
      this.lblInstructions.Size = new Size(326, 18);
      this.lblInstructions.TabIndex = 11;
      this.lblInstructions.Text = "Double-click the Text or Value of an existing item to edit.";
      this.chkEmpty.Location = new Point(14, 240);
      this.chkEmpty.Name = "chkEmpty";
      this.chkEmpty.Size = new Size(204, 23);
      this.chkEmpty.TabIndex = 12;
      this.chkEmpty.Text = "Include empty option at top of list";
      this.AcceptButton = (IButtonControl) this.btnAdd;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(428, 275);
      this.Controls.Add((System.Windows.Forms.Control) this.chkEmpty);
      this.Controls.Add((System.Windows.Forms.Control) this.lblInstructions);
      this.Controls.Add((System.Windows.Forms.Control) this.txtValueEditor);
      this.Controls.Add((System.Windows.Forms.Control) this.txtValue);
      this.Controls.Add((System.Windows.Forms.Control) this.txtText);
      this.Controls.Add((System.Windows.Forms.Control) this.btnAdd);
      this.Controls.Add((System.Windows.Forms.Control) this.lblValue);
      this.Controls.Add((System.Windows.Forms.Control) this.lblText);
      this.Controls.Add((System.Windows.Forms.Control) this.btnCancel);
      this.Controls.Add((System.Windows.Forms.Control) this.btnOK);
      this.Controls.Add((System.Windows.Forms.Control) this.btnDown);
      this.Controls.Add((System.Windows.Forms.Control) this.btnUp);
      this.Controls.Add((System.Windows.Forms.Control) this.btnDelete);
      this.Controls.Add((System.Windows.Forms.Control) this.lvwOptions);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new Size(434, 300);
      this.Name = nameof (DropdownOptionsDialog);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Dropdown Options Editor";
      this.ResumeLayout(false);
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      if (this.txtText.Text.Trim() == "")
      {
        int num1 = (int) MessageBox.Show((IWin32Window) this, "Please enter the text for the option.", "Option Editor", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else if (this.txtValue.Text.Trim() == "" && !this.options.AllowEmptyValues)
      {
        int num2 = (int) MessageBox.Show((IWin32Window) this, "The current control requires all options have a non-empty value.", "Option Editor", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        ListViewItem orFindItem = this.createOrFindItem(this.txtText.Text.Trim());
        orFindItem.SubItems[0].Text = this.txtText.Text.Trim();
        orFindItem.SubItems[1].Text = this.txtValue.Text.Trim();
        this.txtText.Text = "";
        this.txtValue.Text = "";
        this.txtText.Focus();
      }
    }

    private ListViewItem createOrFindItem(string text)
    {
      foreach (ListViewItem orFindItem in ((ListView) this.lvwOptions).Items)
      {
        if (string.Compare(orFindItem.Text, text, true) == 0)
          return orFindItem;
      }
      ListViewItem orFindItem1 = new ListViewItem(new string[2]
      {
        text,
        text
      });
      ((ListView) this.lvwOptions).Items.Add(orFindItem1);
      return orFindItem1;
    }

    private void txtText_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (!(this.txtText.Text == this.txtValue.Text))
        return;
      this.copyToValue = true;
    }

    private void txtText_TextChanged(object sender, EventArgs e)
    {
      if (this.copyToValue)
        this.txtValue.Text = this.txtText.Text;
      this.copyToValue = false;
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      ArrayList arrayList = new ArrayList();
      foreach (ListViewItem selectedItem in ((ListView) this.lvwOptions).SelectedItems)
        arrayList.Add((object) selectedItem);
      foreach (ListViewItem listViewItem in arrayList)
        ((ListView) this.lvwOptions).Items.Remove(listViewItem);
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      this.options.Clear();
      ArrayList optionList = new ArrayList();
      if (this.chkEmpty.Checked && this.options.AllowEmptyValues)
        optionList.Add((object) new DropdownOption(""));
      foreach (ListViewItem listViewItem in ((ListView) this.lvwOptions).Items)
        optionList.Add((object) new DropdownOption(listViewItem.SubItems[0].Text, listViewItem.SubItems[1].Text));
      this.options.AddRange((ICollection) optionList);
      this.DialogResult = DialogResult.OK;
    }

    private void btnUp_Click(object sender, EventArgs e)
    {
      if (((ListView) this.lvwOptions).SelectedItems == null || ((ListView) this.lvwOptions).SelectedItems.Count <= 0)
        return;
      int index = ((ListView) this.lvwOptions).Items.IndexOf(((ListView) this.lvwOptions).SelectedItems[0]);
      if (index <= 0)
        return;
      ListViewItem listViewItem = ((ListView) this.lvwOptions).Items[index - 1];
      ((ListView) this.lvwOptions).Items.Remove(listViewItem);
      ((ListView) this.lvwOptions).Items.Insert(index, listViewItem);
    }

    private void btnDown_Click(object sender, EventArgs e)
    {
      if (((ListView) this.lvwOptions).SelectedItems == null || ((ListView) this.lvwOptions).SelectedItems.Count <= 0)
        return;
      int index = ((ListView) this.lvwOptions).Items.IndexOf(((ListView) this.lvwOptions).SelectedItems[0]);
      if (index + 1 >= ((ListView) this.lvwOptions).Items.Count)
        return;
      ListViewItem listViewItem = ((ListView) this.lvwOptions).Items[index + 1];
      ((ListView) this.lvwOptions).Items.Remove(listViewItem);
      ((ListView) this.lvwOptions).Items.Insert(index, listViewItem);
    }

    private void lvwOptions_SubItemClicked(object sender, SubItemEventArgs e)
    {
      this.lvwOptions.StartEditing((System.Windows.Forms.Control) this.txtValueEditor, e.Item, e.SubItem);
    }

    private void lvwOptions_SubItemEndEditing(object sender, SubItemEndEditingEventArgs e)
    {
      e.DisplayText = e.DisplayText.Trim();
      if (((SubItemEventArgs) e).SubItem == 0 && e.DisplayText == "")
      {
        int num = (int) MessageBox.Show((IWin32Window) this, "The text for this option cannot be blank.", "Option Editor", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        e.DisplayText = ((SubItemEventArgs) e).Item.SubItems[0].Text;
      }
      if (((SubItemEventArgs) e).SubItem != 1 || !(e.DisplayText == "") || this.options.AllowEmptyValues)
        return;
      int num1 = (int) MessageBox.Show((IWin32Window) this, "The current control requires all options have a non-empty value.", "Option Editor", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      e.DisplayText = ((SubItemEventArgs) e).Item.SubItems[1].Text;
    }

    private void lvwOptions_SubItemBeginEditing(object sender, SubItemEventArgs e)
    {
      if (e.SubItem != 1 || this.options.AllowEditValues)
        return;
      e.Cancel = true;
    }
  }
}
