// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.AlternateNamesDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class AlternateNamesDialog : Form
  {
    private LoanData loan;
    private IMainScreen mainScreen;
    private bool isborrower;
    private bool isDirty;
    private Color nonFocusColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
    private Color disabledColor = Color.FromArgb(240, 240, 240);
    private IStatusDisplay statusDisplay;
    private BizRule.FieldAccessRight firstName_rightsPAF = BizRule.FieldAccessRight.DoesNotApply;
    private BizRule.FieldAccessRight lastName_rightsPAF = BizRule.FieldAccessRight.DoesNotApply;
    private BizRule.FieldAccessRight middleName_rightsPAF = BizRule.FieldAccessRight.DoesNotApply;
    private BizRule.FieldAccessRight suffix_rightsPAF = BizRule.FieldAccessRight.DoesNotApply;
    private IContainer components;
    private Button okBtn;
    private Button cancelBtn;
    private TableContainer tableContainer1;
    private StandardIconButton btnDelete;
    private GridView listViewAltNames;
    private StandardIconButton btnAdd;
    private GroupContainer groupContainer1;
    private TextBox txtSuffix;
    private TextBox txtLastName;
    private TextBox txtMiddleName;
    private Label label4;
    private Label label3;
    private Label label2;
    private Label label1;
    private TextBox txtFirstName;
    private StandardIconButton btnUp;
    private StandardIconButton btnDown;

    public AlternateNamesDialog(LoanData loan, IMainScreen mainScreen, bool isborrower)
    {
      this.loan = loan;
      this.mainScreen = mainScreen;
      this.isborrower = isborrower;
      this.InitializeComponent();
      this.Text += isborrower ? " - Borrower" : " - Coborrower";
      this.txtFirstName.Tag = isborrower ? (object) "URLABAKA0001" : (object) "URLACAKA0001";
      this.txtLastName.Tag = isborrower ? (object) "URLABAKA0003" : (object) "URLACAKA0003";
      this.txtMiddleName.Tag = isborrower ? (object) "URLABAKA0002" : (object) "URLACAKA0002";
      this.txtSuffix.Tag = isborrower ? (object) "URLABAKA0004" : (object) "URLACAKA0004";
      this.statusDisplay = Session.Application.GetService<IStatusDisplay>();
      if (Session.LoanDataMgr != null)
      {
        this.firstName_rightsPAF = Session.LoanDataMgr.GetFieldAccessRights(this.txtFirstName.Tag.ToString());
        this.lastName_rightsPAF = Session.LoanDataMgr.GetFieldAccessRights(this.txtLastName.Tag.ToString());
        this.middleName_rightsPAF = Session.LoanDataMgr.GetFieldAccessRights(this.txtMiddleName.Tag.ToString());
        this.suffix_rightsPAF = Session.LoanDataMgr.GetFieldAccessRights(this.txtSuffix.Tag.ToString());
      }
      if (this.areAllButtonsDisabledPerPAFs())
      {
        this.btnAdd.Enabled = false;
        this.btnDelete.Enabled = false;
        this.btnUp.Enabled = false;
        this.btnDown.Enabled = false;
      }
      this.initForm();
      this.isDirty = false;
    }

    private bool areAllButtonsDisabledPerPAFs()
    {
      return (this.firstName_rightsPAF == BizRule.FieldAccessRight.Hide || this.firstName_rightsPAF == BizRule.FieldAccessRight.ViewOnly) && (this.lastName_rightsPAF == BizRule.FieldAccessRight.Hide || this.lastName_rightsPAF == BizRule.FieldAccessRight.ViewOnly) && (this.middleName_rightsPAF == BizRule.FieldAccessRight.Hide || this.middleName_rightsPAF == BizRule.FieldAccessRight.ViewOnly) && (this.suffix_rightsPAF == BizRule.FieldAccessRight.Hide || this.suffix_rightsPAF == BizRule.FieldAccessRight.ViewOnly);
    }

    private void initForm()
    {
      IList<URLAAlternateName> urlaAlternateNameList = (IList<URLAAlternateName>) null;
      if (this.loan.GetNumberOfURLAAlternateNames(this.isborrower) > 0)
        urlaAlternateNameList = this.loan.GetURLAAlternames(this.isborrower);
      if (urlaAlternateNameList == null || urlaAlternateNameList.Count == 0)
      {
        this.listViewAltNames.Items.Clear();
        this.listViewAltNames_SelectedIndexChanged((object) null, (EventArgs) null);
      }
      else
      {
        this.listViewAltNames.BeginUpdate();
        this.listViewAltNames.Items.Clear();
        foreach (URLAAlternateName urlaAlternateName in (IEnumerable<URLAAlternateName>) urlaAlternateNameList)
          this.populateListView(urlaAlternateName, false);
        this.listViewAltNames.EndUpdate();
        if (this.listViewAltNames.Items.Count > 0)
          this.listViewAltNames.Items[0].Selected = true;
        else
          this.listViewAltNames_SelectedIndexChanged((object) null, (EventArgs) null);
      }
    }

    private void populateListView(URLAAlternateName urlaAlternateName, bool selected)
    {
      GVItem gvItem = new GVItem();
      gvItem.SubItems.Add((object) urlaAlternateName.FirstName);
      gvItem.SubItems.Add((object) urlaAlternateName.MiddleName);
      gvItem.SubItems.Add((object) urlaAlternateName.LastName);
      gvItem.SubItems.Add((object) urlaAlternateName.Suffix);
      gvItem.SubItems.Add((object) urlaAlternateName.FullName);
      gvItem.Tag = (object) urlaAlternateName;
      if (selected)
        gvItem.Selected = true;
      this.listViewAltNames.Items.Add(gvItem);
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (this.isDirty)
      {
        IList<URLAAlternateName> akaNameList = (IList<URLAAlternateName>) new List<URLAAlternateName>();
        for (int nItemIndex = 0; nItemIndex < this.listViewAltNames.Items.Count; ++nItemIndex)
        {
          URLAAlternateName tag = (URLAAlternateName) this.listViewAltNames.Items[nItemIndex].Tag;
          tag.FirstName = this.listViewAltNames.Items[nItemIndex].SubItems[0].Text.Trim();
          tag.MiddleName = this.listViewAltNames.Items[nItemIndex].SubItems[1].Text.Trim();
          tag.LastName = this.listViewAltNames.Items[nItemIndex].SubItems[2].Text.Trim();
          tag.Suffix = this.listViewAltNames.Items[nItemIndex].SubItems[3].Text.Trim();
          tag.FullName = this.listViewAltNames.Items[nItemIndex].SubItems[4].Text.Trim();
          if (!tag.IsBlank)
            akaNameList.Add(tag);
        }
        Cursor.Current = Cursors.WaitCursor;
        this.loan.UpdateURLAAlternateNames(this.isborrower, akaNameList);
        this.loan.Calculator.FormCalculation(this.isborrower ? "URLABAKA0101" : "URLACAKA0101");
        Cursor.Current = Cursors.Default;
      }
      this.DialogResult = DialogResult.OK;
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      if (this.listViewAltNames.SelectedItems != null && this.listViewAltNames.SelectedItems.Count > 0)
        this.listViewAltNames.SelectedItems.Clear();
      this.populateListView(new URLAAlternateName((string) null, "", "", "", "", ""), true);
      this.txtFirstName.Focus();
      this.isDirty = true;
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (this.listViewAltNames.SelectedItems == null)
        return;
      this.isDirty = true;
      int index = this.listViewAltNames.SelectedItems[0].Index;
      foreach (GVItem selectedItem in this.listViewAltNames.SelectedItems)
        this.listViewAltNames.Items.Remove(selectedItem);
      if (this.listViewAltNames.Items.Count == 0)
        return;
      this.listViewAltNames.Items[this.listViewAltNames.Items.Count <= index ? this.listViewAltNames.Items.Count - 1 : index].Selected = true;
    }

    private void listViewAltNames_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!this.areAllButtonsDisabledPerPAFs())
      {
        this.btnDelete.Enabled = this.listViewAltNames.SelectedItems != null && this.listViewAltNames.SelectedItems.Count > 0;
        this.btnUp.Enabled = this.btnDown.Enabled = this.listViewAltNames.SelectedItems != null && this.listViewAltNames.SelectedItems.Count > 0;
      }
      if (this.listViewAltNames.SelectedItems != null && this.listViewAltNames.SelectedItems.Count == 1)
      {
        this.txtFirstName.Text = this.txtMiddleName.Text = this.txtLastName.Text = this.txtSuffix.Text = "*";
        if (this.firstName_rightsPAF != BizRule.FieldAccessRight.Hide)
          this.txtFirstName.Text = this.listViewAltNames.SelectedItems[0].SubItems[0].Text.Trim();
        if (this.middleName_rightsPAF != BizRule.FieldAccessRight.Hide)
          this.txtMiddleName.Text = this.listViewAltNames.SelectedItems[0].SubItems[1].Text.Trim();
        if (this.lastName_rightsPAF != BizRule.FieldAccessRight.Hide)
          this.txtLastName.Text = this.listViewAltNames.SelectedItems[0].SubItems[2].Text.Trim();
        if (this.suffix_rightsPAF != BizRule.FieldAccessRight.Hide)
          this.txtSuffix.Text = this.listViewAltNames.SelectedItems[0].SubItems[3].Text.Trim();
      }
      else
        this.txtFirstName.Text = this.txtMiddleName.Text = this.txtLastName.Text = this.txtSuffix.Text = "";
      TextBox txtFirstName = this.txtFirstName;
      TextBox txtMiddleName = this.txtMiddleName;
      TextBox txtLastName = this.txtLastName;
      bool flag1;
      this.txtSuffix.ReadOnly = flag1 = this.listViewAltNames.SelectedItems == null || this.listViewAltNames.SelectedItems.Count != 1;
      int num1;
      bool flag2 = (num1 = flag1 ? 1 : 0) != 0;
      txtLastName.ReadOnly = num1 != 0;
      int num2;
      bool flag3 = (num2 = flag2 ? 1 : 0) != 0;
      txtMiddleName.ReadOnly = num2 != 0;
      int num3 = flag3 ? 1 : 0;
      txtFirstName.ReadOnly = num3 != 0;
      if (!this.txtFirstName.ReadOnly && (this.firstName_rightsPAF == BizRule.FieldAccessRight.Hide || this.firstName_rightsPAF == BizRule.FieldAccessRight.ViewOnly))
        this.txtFirstName.ReadOnly = true;
      if (!this.txtMiddleName.ReadOnly && (this.middleName_rightsPAF == BizRule.FieldAccessRight.Hide || this.middleName_rightsPAF == BizRule.FieldAccessRight.ViewOnly))
        this.txtMiddleName.ReadOnly = true;
      if (!this.txtLastName.ReadOnly && (this.lastName_rightsPAF == BizRule.FieldAccessRight.Hide || this.lastName_rightsPAF == BizRule.FieldAccessRight.ViewOnly))
        this.txtLastName.ReadOnly = true;
      if (!this.txtSuffix.ReadOnly && (this.suffix_rightsPAF == BizRule.FieldAccessRight.Hide || this.suffix_rightsPAF == BizRule.FieldAccessRight.ViewOnly))
        this.txtSuffix.ReadOnly = true;
      this.txtFirstName.Enabled = !this.txtFirstName.ReadOnly;
      this.txtMiddleName.Enabled = !this.txtMiddleName.ReadOnly;
      this.txtLastName.Enabled = !this.txtLastName.ReadOnly;
      this.txtSuffix.Enabled = !this.txtSuffix.ReadOnly;
      this.txtFirstName.BackColor = this.txtFirstName.Enabled ? this.nonFocusColor : this.disabledColor;
      this.txtMiddleName.BackColor = this.txtMiddleName.Enabled ? this.nonFocusColor : this.disabledColor;
      this.txtLastName.BackColor = this.txtLastName.Enabled ? this.nonFocusColor : this.disabledColor;
      this.txtSuffix.BackColor = this.txtSuffix.Enabled ? this.nonFocusColor : this.disabledColor;
    }

    private void nameField_KeyUp(object sender, KeyEventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      switch (textBox.Name)
      {
        case "txtFirstName":
          this.listViewAltNames.SelectedItems[0].SubItems[0].Text = textBox.Text;
          break;
        case "txtMiddleName":
          this.listViewAltNames.SelectedItems[0].SubItems[1].Text = textBox.Text;
          break;
        case "txtLastName":
          this.listViewAltNames.SelectedItems[0].SubItems[2].Text = textBox.Text;
          break;
        case "txtSuffix":
          this.listViewAltNames.SelectedItems[0].SubItems[3].Text = textBox.Text;
          break;
      }
      this.updateFullNameInColumn(this.listViewAltNames.SelectedItems[0]);
      this.isDirty = true;
    }

    private void nameField_Leave(object sender, EventArgs e)
    {
      ((Control) sender).BackColor = this.nonFocusColor;
      if (this.statusDisplay == null)
        return;
      this.statusDisplay.DisplayFieldID("");
    }

    private void nameField_Enter(object sender, EventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      textBox.BackColor = Color.LightGoldenrodYellow;
      string tag = (string) textBox.Tag;
      string id;
      if (this.listViewAltNames.SelectedItems != null && this.listViewAltNames.SelectedItems.Any<GVItem>())
      {
        int num = this.listViewAltNames.SelectedItems[0].DisplayIndex + 1;
        id = (this.isborrower ? "URLABAKA" : "URLACAKA") + num.ToString("00") + tag.Substring(tag.Length - 2);
      }
      else
        id = string.Empty;
      if (this.statusDisplay == null)
        return;
      this.statusDisplay.DisplayFieldID(id);
    }

    private void updateFullNameInColumn(GVItem gvItem)
    {
      string str1 = "";
      for (int nItemIndex = 0; nItemIndex < gvItem.SubItems.Count - 1; ++nItemIndex)
      {
        string str2 = gvItem.SubItems[nItemIndex].Text.Trim();
        str1 = str1 + (!(str1 != "") || !(str2 != "") ? "" : " ") + str2;
      }
      gvItem.SubItems[4].Text = str1;
    }

    private void cancelBtn_Click(object sender, EventArgs e)
    {
      if (this.isDirty && Utils.Dialog((IWin32Window) this, "You have modified borrower's alternate name(s). Do you want to continue to cancel your change?", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes)
        return;
      this.DialogResult = DialogResult.Cancel;
    }

    private void btnUp_Click(object sender, EventArgs e)
    {
      this.listViewAltNames.SelectedIndexChanged -= new EventHandler(this.listViewAltNames_SelectedIndexChanged);
      this.listViewAltNames.BeginUpdate();
      for (int index = 0; index < this.listViewAltNames.SelectedItems.Count; ++index)
      {
        GVItem selectedItem = this.listViewAltNames.SelectedItems[index];
        if (selectedItem.Index != 0 && !this.listViewAltNames.Items[selectedItem.Index - 1].Selected)
        {
          this.listViewAltNames.Items.RemoveAt(selectedItem.Index);
          this.listViewAltNames.Items.Insert(selectedItem.Index - 1, selectedItem);
          selectedItem.Selected = true;
          if (!this.isDirty)
            this.isDirty = true;
        }
      }
      this.listViewAltNames.EndUpdate();
      this.listViewAltNames.SelectedIndexChanged += new EventHandler(this.listViewAltNames_SelectedIndexChanged);
    }

    private void btnDown_Click(object sender, EventArgs e)
    {
      this.listViewAltNames.SelectedIndexChanged -= new EventHandler(this.listViewAltNames_SelectedIndexChanged);
      this.listViewAltNames.BeginUpdate();
      for (int index = this.listViewAltNames.SelectedItems.Count - 1; index >= 0; --index)
      {
        GVItem selectedItem = this.listViewAltNames.SelectedItems[index];
        if (selectedItem.Index != this.listViewAltNames.Items.Count - 1 && !this.listViewAltNames.Items[selectedItem.Index + 1].Selected)
        {
          this.listViewAltNames.Items.RemoveAt(selectedItem.Index);
          this.listViewAltNames.Items.Insert(selectedItem.Index + 1, selectedItem);
          selectedItem.Selected = true;
          if (!this.isDirty)
            this.isDirty = true;
        }
      }
      this.listViewAltNames.EndUpdate();
      this.listViewAltNames.SelectedIndexChanged += new EventHandler(this.listViewAltNames_SelectedIndexChanged);
    }

    private void txtFirstName_KeyPress(object sender, KeyPressEventArgs e)
    {
      this.removeSemiColon(e);
    }

    private void txtMiddleName_KeyPress(object sender, KeyPressEventArgs e)
    {
      this.removeSemiColon(e);
    }

    private void removeSemiColon(KeyPressEventArgs e)
    {
      if (e.KeyChar == ';')
        e.Handled = true;
      else
        e.Handled = false;
    }

    private void txtLastName_KeyPress(object sender, KeyPressEventArgs e)
    {
      this.removeSemiColon(e);
    }

    private void txtSuffix_KeyPress(object sender, KeyPressEventArgs e) => this.removeSemiColon(e);

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
      GVItem gvItem = new GVItem();
      GVSubItem gvSubItem1 = new GVSubItem();
      GVSubItem gvSubItem2 = new GVSubItem();
      GVSubItem gvSubItem3 = new GVSubItem();
      GVSubItem gvSubItem4 = new GVSubItem();
      GVSubItem gvSubItem5 = new GVSubItem();
      this.okBtn = new Button();
      this.cancelBtn = new Button();
      this.tableContainer1 = new TableContainer();
      this.btnUp = new StandardIconButton();
      this.btnDown = new StandardIconButton();
      this.btnAdd = new StandardIconButton();
      this.listViewAltNames = new GridView();
      this.btnDelete = new StandardIconButton();
      this.groupContainer1 = new GroupContainer();
      this.txtSuffix = new TextBox();
      this.txtLastName = new TextBox();
      this.txtMiddleName = new TextBox();
      this.label4 = new Label();
      this.label3 = new Label();
      this.label2 = new Label();
      this.label1 = new Label();
      this.txtFirstName = new TextBox();
      this.tableContainer1.SuspendLayout();
      ((ISupportInitialize) this.btnUp).BeginInit();
      ((ISupportInitialize) this.btnDown).BeginInit();
      ((ISupportInitialize) this.btnAdd).BeginInit();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.okBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.okBtn.BackColor = SystemColors.Control;
      this.okBtn.Location = new Point(806, 477);
      this.okBtn.Margin = new Padding(4, 5, 4, 5);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(112, 37);
      this.okBtn.TabIndex = 4;
      this.okBtn.Text = "&OK";
      this.okBtn.UseVisualStyleBackColor = true;
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.cancelBtn.Location = new Point(926, 477);
      this.cancelBtn.Margin = new Padding(4, 5, 4, 5);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(112, 37);
      this.cancelBtn.TabIndex = 5;
      this.cancelBtn.Text = "&Cancel";
      this.cancelBtn.Click += new EventHandler(this.cancelBtn_Click);
      this.tableContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tableContainer1.Controls.Add((Control) this.btnUp);
      this.tableContainer1.Controls.Add((Control) this.btnDown);
      this.tableContainer1.Controls.Add((Control) this.btnAdd);
      this.tableContainer1.Controls.Add((Control) this.listViewAltNames);
      this.tableContainer1.Controls.Add((Control) this.btnDelete);
      this.tableContainer1.Location = new Point(14, 18);
      this.tableContainer1.Margin = new Padding(4, 5, 4, 5);
      this.tableContainer1.Name = "tableContainer1";
      this.tableContainer1.Size = new Size(1022, 291);
      this.tableContainer1.Style = TableContainer.ContainerStyle.HeaderOnly;
      this.tableContainer1.TabIndex = 74;
      this.tableContainer1.Text = "Alternate Names";
      this.btnUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnUp.BackColor = Color.Transparent;
      this.btnUp.Location = new Point(956, 6);
      this.btnUp.Margin = new Padding(4, 5, 4, 5);
      this.btnUp.MouseDownImage = (Image) null;
      this.btnUp.Name = "btnUp";
      this.btnUp.Size = new Size(24, 25);
      this.btnUp.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.btnUp.TabIndex = 39;
      this.btnUp.TabStop = false;
      this.btnUp.Click += new EventHandler(this.btnUp_Click);
      this.btnDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDown.BackColor = Color.Transparent;
      this.btnDown.Location = new Point(986, 6);
      this.btnDown.Margin = new Padding(4, 5, 4, 5);
      this.btnDown.MouseDownImage = (Image) null;
      this.btnDown.Name = "btnDown";
      this.btnDown.Size = new Size(24, 25);
      this.btnDown.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.btnDown.TabIndex = 38;
      this.btnDown.TabStop = false;
      this.btnDown.Click += new EventHandler(this.btnDown_Click);
      this.btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAdd.BackColor = Color.Transparent;
      this.btnAdd.Location = new Point(894, 6);
      this.btnAdd.Margin = new Padding(4, 5, 4, 5);
      this.btnAdd.MouseDownImage = (Image) null;
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(24, 25);
      this.btnAdd.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAdd.TabIndex = 37;
      this.btnAdd.TabStop = false;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.listViewAltNames.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column2";
      gvColumn1.Text = "First Name";
      gvColumn1.Width = 150;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column5";
      gvColumn2.Text = "Middle Name";
      gvColumn2.Width = 150;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Last Name";
      gvColumn3.Width = 150;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.SortMethod = GVSortMethod.None;
      gvColumn4.Text = "Suffix";
      gvColumn4.TextAlignment = ContentAlignment.TopLeft;
      gvColumn4.Width = 100;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column7";
      gvColumn5.SpringToFit = true;
      gvColumn5.Text = "Full Name";
      gvColumn5.Width = 470;
      this.listViewAltNames.Columns.AddRange(new GVColumn[5]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5
      });
      this.listViewAltNames.Dock = DockStyle.Fill;
      this.listViewAltNames.HotTrackingColor = Color.FromArgb(250, 248, 188);
      gvItem.BackColor = Color.Empty;
      gvItem.ForeColor = Color.Empty;
      gvSubItem1.BackColor = Color.Empty;
      gvSubItem1.ForeColor = Color.Empty;
      gvSubItem2.BackColor = Color.Empty;
      gvSubItem2.ForeColor = Color.Empty;
      gvSubItem3.BackColor = Color.Empty;
      gvSubItem3.ForeColor = Color.Empty;
      gvSubItem4.BackColor = Color.Empty;
      gvSubItem4.ForeColor = Color.Empty;
      gvSubItem5.BackColor = Color.Empty;
      gvSubItem5.ForeColor = Color.Empty;
      gvItem.SubItems.AddRange(new GVSubItem[5]
      {
        gvSubItem1,
        gvSubItem2,
        gvSubItem3,
        gvSubItem4,
        gvSubItem5
      });
      this.listViewAltNames.Items.AddRange(new GVItem[1]
      {
        gvItem
      });
      this.listViewAltNames.Location = new Point(1, 26);
      this.listViewAltNames.Margin = new Padding(4, 5, 4, 5);
      this.listViewAltNames.Name = "listViewAltNames";
      this.listViewAltNames.ShowFocusRect = true;
      this.listViewAltNames.Size = new Size(1020, 264);
      this.listViewAltNames.SortOption = GVSortOption.None;
      this.listViewAltNames.TabIndex = 36;
      this.listViewAltNames.SelectedIndexChanged += new EventHandler(this.listViewAltNames_SelectedIndexChanged);
      this.btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Location = new Point(926, 6);
      this.btnDelete.Margin = new Padding(4, 5, 4, 5);
      this.btnDelete.MouseDownImage = (Image) null;
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(24, 25);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabIndex = 34;
      this.btnDelete.TabStop = false;
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.groupContainer1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer1.Controls.Add((Control) this.txtSuffix);
      this.groupContainer1.Controls.Add((Control) this.txtLastName);
      this.groupContainer1.Controls.Add((Control) this.txtMiddleName);
      this.groupContainer1.Controls.Add((Control) this.label4);
      this.groupContainer1.Controls.Add((Control) this.label3);
      this.groupContainer1.Controls.Add((Control) this.label2);
      this.groupContainer1.Controls.Add((Control) this.label1);
      this.groupContainer1.Controls.Add((Control) this.txtFirstName);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(14, 318);
      this.groupContainer1.Margin = new Padding(4, 5, 4, 5);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(1020, 143);
      this.groupContainer1.TabIndex = 75;
      this.groupContainer1.Text = "Detail";
      this.txtSuffix.Location = new Point(675, 89);
      this.txtSuffix.Margin = new Padding(4, 5, 4, 5);
      this.txtSuffix.Name = "txtSuffix";
      this.txtSuffix.Size = new Size(332, 26);
      this.txtSuffix.TabIndex = 3;
      this.txtSuffix.Enter += new EventHandler(this.nameField_Enter);
      this.txtSuffix.KeyPress += new KeyPressEventHandler(this.txtSuffix_KeyPress);
      this.txtSuffix.KeyUp += new KeyEventHandler(this.nameField_KeyUp);
      this.txtSuffix.Leave += new EventHandler(this.nameField_Leave);
      this.txtLastName.Location = new Point(142, 89);
      this.txtLastName.Margin = new Padding(4, 5, 4, 5);
      this.txtLastName.Name = "txtLastName";
      this.txtLastName.Size = new Size(368, 26);
      this.txtLastName.TabIndex = 2;
      this.txtLastName.Enter += new EventHandler(this.nameField_Enter);
      this.txtLastName.KeyPress += new KeyPressEventHandler(this.txtLastName_KeyPress);
      this.txtLastName.KeyUp += new KeyEventHandler(this.nameField_KeyUp);
      this.txtLastName.Leave += new EventHandler(this.nameField_Leave);
      this.txtMiddleName.Location = new Point(675, 52);
      this.txtMiddleName.Margin = new Padding(4, 5, 4, 5);
      this.txtMiddleName.Name = "txtMiddleName";
      this.txtMiddleName.Size = new Size(332, 26);
      this.txtMiddleName.TabIndex = 1;
      this.txtMiddleName.Enter += new EventHandler(this.nameField_Enter);
      this.txtMiddleName.KeyPress += new KeyPressEventHandler(this.txtMiddleName_KeyPress);
      this.txtMiddleName.KeyUp += new KeyEventHandler(this.nameField_KeyUp);
      this.txtMiddleName.Leave += new EventHandler(this.nameField_Leave);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(562, 94);
      this.label4.Margin = new Padding(4, 0, 4, 0);
      this.label4.Name = "label4";
      this.label4.Size = new Size(49, 20);
      this.label4.TabIndex = 4;
      this.label4.Text = "Suffix";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(9, 94);
      this.label3.Margin = new Padding(4, 0, 4, 0);
      this.label3.Name = "label3";
      this.label3.Size = new Size(86, 20);
      this.label3.TabIndex = 3;
      this.label3.Text = "Last Name";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(562, 57);
      this.label2.Margin = new Padding(4, 0, 4, 0);
      this.label2.Name = "label2";
      this.label2.Size = new Size(101, 20);
      this.label2.TabIndex = 2;
      this.label2.Text = "Middle Name";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(9, 57);
      this.label1.Margin = new Padding(4, 0, 4, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(86, 20);
      this.label1.TabIndex = 1;
      this.label1.Text = "First Name";
      this.txtFirstName.Location = new Point(142, 52);
      this.txtFirstName.Margin = new Padding(4, 5, 4, 5);
      this.txtFirstName.Name = "txtFirstName";
      this.txtFirstName.Size = new Size(368, 26);
      this.txtFirstName.TabIndex = 0;
      this.txtFirstName.Enter += new EventHandler(this.nameField_Enter);
      this.txtFirstName.KeyPress += new KeyPressEventHandler(this.txtFirstName_KeyPress);
      this.txtFirstName.KeyUp += new KeyEventHandler(this.nameField_KeyUp);
      this.txtFirstName.Leave += new EventHandler(this.nameField_Leave);
      this.AutoScaleDimensions = new SizeF(9f, 20f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1052, 528);
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.tableContainer1);
      this.Controls.Add((Control) this.okBtn);
      this.Controls.Add((Control) this.cancelBtn);
      this.Margin = new Padding(4, 5, 4, 5);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AlternateNamesDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Alternate Names";
      this.tableContainer1.ResumeLayout(false);
      ((ISupportInitialize) this.btnUp).EndInit();
      ((ISupportInitialize) this.btnDown).EndInit();
      ((ISupportInitialize) this.btnAdd).EndInit();
      ((ISupportInitialize) this.btnDelete).EndInit();
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
