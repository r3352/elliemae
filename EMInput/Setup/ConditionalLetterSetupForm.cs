// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ConditionalLetterSetupForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class ConditionalLetterSetupForm : Form
  {
    private static string[] conditionData = new string[11]
    {
      "Condition Due (PTA, PTD, PTF, PTC)",
      "Category (Assets, Liabilities, etc)",
      "Condition Owner's Name",
      "Condition Status",
      "Condition Status - Added",
      "Condition Status - Fulfilled",
      "Condition Status - Received",
      "Condition Status - Reviewed",
      "Condition Status - Rejected",
      "Condition Status - Cleared",
      "Condition Status - Waived"
    };
    private ConditionalLetterPrintOption letterPrintOption;
    private Sessions.Session session;
    private IContainer components;
    private DialogButtons dialogButtons1;
    private GroupContainer groupContainer1;
    private GroupBox groupBox1;
    private Label label1;
    private TextBox txtName;
    private Label label2;
    private Label label3;
    private StandardIconButton btnAddStarting;
    private StandardIconButton btnDeleteEnding;
    private Label label5;
    private ComboBox cboConditionOption;
    private Label label4;
    private GridView gridViewData;
    private GroupBox groupBox2;
    private RadioButton rdoSortByDue;
    private CheckBox chkUseStartingPage;
    private Label label9;
    private Label label8;
    private Label label7;
    private Label label6;
    private Panel panelGroupCondition;
    private RadioButton rdoGroupByDue;
    private RadioButton rdoGroupByCategory;
    private RadioButton rdoGroupByOwner;
    private CheckBox chkUseGroup;
    private ComboBox cboPageNo;
    private Panel panel1;
    private RadioButton rdoSortByStatus;
    private RadioButton rdoSortByCategory;
    private RadioButton rdoSortByOwner;
    private Panel panelGroupingPage;
    private RadioButton rdoPageMultiple;
    private RadioButton rdoPageOne;
    private CheckBox chkIncludeDesc;
    private StandardIconButton btnEdit;
    private TextBox txtStartingPages;
    private TextBox txtEndingPages;
    private StandardIconButton btnDeleteStarting;
    private StandardIconButton btnAddEnding;
    private Label label10;
    private ComboBox cboLetterType;
    private ToolTip toolTip1;
    private ComboBox cboPaperSize;
    private ComboBox cboSpacing;
    private Label label12;
    private Label label11;

    public ConditionalLetterSetupForm(
      Sessions.Session session,
      ConditionalLetterPrintOption letterPrintOption)
    {
      this.session = session;
      this.letterPrintOption = letterPrintOption;
      this.InitializeComponent();
      this.initForm();
      this.chkUseGroup_Click((object) null, (EventArgs) null);
      this.gridViewData_SelectedIndexChanged((object) null, (EventArgs) null);
      this.gridViewData.DoubleClick += new EventHandler(this.gridViewData_DoubleClick);
    }

    private void gridViewData_DoubleClick(object sender, EventArgs e)
    {
      if (this.gridViewData.SelectedItems.Count == 0 || this.gridViewData.SelectedItems[0].Index <= 2)
        return;
      this.btnEdit_Click((object) null, (EventArgs) null);
    }

    private void initForm()
    {
      this.btnDeleteStarting.Enabled = false;
      this.btnDeleteEnding.Enabled = false;
      this.cboConditionOption.SelectedIndex = 0;
      this.gridViewData.BeginUpdate();
      for (int index = 0; index < ConditionalLetterSetupForm.conditionData.Length; ++index)
      {
        GVItem gvItem = new GVItem(ConditionalLetterSetupForm.conditionData[index]);
        if (index <= 2)
          gvItem.SubItems.Add((object) "N/A");
        else if (this.letterPrintOption == null && index == 3)
          gvItem.SubItems.Add((object) "Date");
        else if (this.letterPrintOption == null)
        {
          gvItem.SubItems.Add((object) "Date and Name");
        }
        else
        {
          switch (index)
          {
            case 3:
              gvItem.SubItems.Add(this.letterPrintOption.StatusCurrentType == 0 ? (object) "" : (this.letterPrintOption.StatusCurrentType == 1 ? (object) this.getStatusType(this.letterPrintOption.StatusCurrentType) : (object) ""));
              break;
            case 4:
              gvItem.SubItems.Add((object) this.getStatusType(this.letterPrintOption.StatusAddedType));
              break;
            case 5:
              gvItem.SubItems.Add((object) this.getStatusType(this.letterPrintOption.StatusFulfilledType));
              break;
            case 6:
              gvItem.SubItems.Add((object) this.getStatusType(this.letterPrintOption.StatusReceivedType));
              break;
            case 7:
              gvItem.SubItems.Add((object) this.getStatusType(this.letterPrintOption.StatusReviewedType));
              break;
            case 8:
              gvItem.SubItems.Add((object) this.getStatusType(this.letterPrintOption.StatusRejectedType));
              break;
            case 9:
              gvItem.SubItems.Add((object) this.getStatusType(this.letterPrintOption.StatusClearedType));
              break;
            case 10:
              gvItem.SubItems.Add((object) this.getStatusType(this.letterPrintOption.StatusWaivedType));
              break;
          }
        }
        if (this.letterPrintOption != null)
        {
          switch (index)
          {
            case 0:
              gvItem.Checked = this.letterPrintOption.UseDue;
              break;
            case 1:
              gvItem.Checked = this.letterPrintOption.UseCategory;
              break;
            case 2:
              gvItem.Checked = this.letterPrintOption.UseOwnerName;
              break;
            case 3:
              gvItem.Checked = this.letterPrintOption.UseCurrentStatus;
              break;
            case 4:
              gvItem.Checked = this.letterPrintOption.UseStatusAdded;
              break;
            case 5:
              gvItem.Checked = this.letterPrintOption.UseStatusFulfilled;
              break;
            case 6:
              gvItem.Checked = this.letterPrintOption.UseStatusReceived;
              break;
            case 7:
              gvItem.Checked = this.letterPrintOption.UseStatusReviewed;
              break;
            case 8:
              gvItem.Checked = this.letterPrintOption.UseStatusRejected;
              break;
            case 9:
              gvItem.Checked = this.letterPrintOption.UseStatusCleared;
              break;
            case 10:
              gvItem.Checked = this.letterPrintOption.UseStatusWaived;
              break;
          }
        }
        this.gridViewData.Items.Add(gvItem);
      }
      this.gridViewData.EndUpdate();
      if (this.letterPrintOption != null)
      {
        this.txtName.Text = this.letterPrintOption.TemplateName;
        this.txtStartingPages.Text = this.removeExtension(this.letterPrintOption.StartingPages);
        this.txtStartingPages.Tag = (object) this.letterPrintOption.StartingPages;
        this.txtEndingPages.Text = this.removeExtension(this.letterPrintOption.EndingPages);
        this.txtEndingPages.Tag = (object) this.letterPrintOption.EndingPages;
        this.cboConditionOption.SelectedIndex = this.letterPrintOption.ConditionOption;
        this.chkIncludeDesc.Checked = this.letterPrintOption.IncludeDescription;
        this.rdoSortByDue.Checked = this.letterPrintOption.SortBy == 1;
        this.rdoSortByCategory.Checked = this.letterPrintOption.SortBy == 2;
        this.rdoSortByOwner.Checked = this.letterPrintOption.SortBy == 3;
        this.rdoSortByStatus.Checked = this.letterPrintOption.SortBy == 4;
        this.chkUseStartingPage.Checked = this.letterPrintOption.ShowPageNumber;
        this.cboPageNo.SelectedIndex = this.letterPrintOption.StartingPageNumber > 0 ? this.letterPrintOption.StartingPageNumber - 1 : this.letterPrintOption.StartingPageNumber;
        this.chkUseGroup.Checked = this.letterPrintOption.NeedGroup;
        this.rdoGroupByDue.Checked = this.letterPrintOption.GroupBy == 1;
        this.rdoGroupByCategory.Checked = this.letterPrintOption.GroupBy == 2;
        this.rdoGroupByOwner.Checked = this.letterPrintOption.GroupBy == 3;
        this.rdoPageMultiple.Checked = this.letterPrintOption.GroupingPage == 1;
        this.rdoPageOne.Checked = this.letterPrintOption.GroupingPage == 2;
        if (this.letterPrintOption.LetterType >= this.cboLetterType.Items.Count)
          this.cboLetterType.SelectedIndex = 0;
        else
          this.cboLetterType.SelectedIndex = this.letterPrintOption.LetterType;
        this.cboSpacing.SelectedIndex = this.letterPrintOption.SpaceCondensed ? 1 : 0;
        this.cboPaperSize.SelectedIndex = this.letterPrintOption.UseLegalSize ? 1 : 0;
      }
      this.chkUseStartingPage_Click((object) null, (EventArgs) null);
    }

    private string getStatusType(int t)
    {
      if (t == 1)
        return "Date";
      return t == 2 ? "Name" : "Date and Name";
    }

    private int getStatusTypeInteger(string t)
    {
      switch (t)
      {
        case "Date":
          return 1;
        case "Name":
          return 2;
        default:
          return 0;
      }
    }

    private string removeExtension(string name)
    {
      if (name.ToLower().EndsWith(".doc") || name.ToLower().EndsWith(".rtf"))
        return name.Substring(0, name.Length - 4);
      return name.ToLower().EndsWith(".docx") ? name.Substring(0, name.Length - 5) : name;
    }

    private void dialogButtons1_OK(object sender, EventArgs e)
    {
      if (this.txtName.Text.Trim() == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please enter a letter name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (this.letterPrintOption == null)
          this.letterPrintOption = new ConditionalLetterPrintOption();
        this.letterPrintOption.StartingPages = this.txtStartingPages.Tag.ToString();
        this.letterPrintOption.EndingPages = this.txtEndingPages.Tag.ToString();
        this.letterPrintOption.ConditionOption = this.cboConditionOption.SelectedIndex;
        this.letterPrintOption.IncludeDescription = this.chkIncludeDesc.Checked;
        this.letterPrintOption.SortBy = this.rdoSortByDue.Checked ? 1 : (this.rdoSortByCategory.Checked ? 2 : (this.rdoSortByOwner.Checked ? 3 : (this.rdoSortByStatus.Checked ? 4 : 1)));
        this.letterPrintOption.ShowPageNumber = this.chkUseStartingPage.Checked;
        this.letterPrintOption.StartingPageNumber = this.cboPageNo.SelectedIndex + 1;
        this.letterPrintOption.NeedGroup = this.chkUseGroup.Checked;
        this.letterPrintOption.GroupBy = this.rdoGroupByDue.Checked ? 1 : (this.rdoGroupByCategory.Checked ? 2 : (this.rdoGroupByOwner.Checked ? 3 : 1));
        this.letterPrintOption.GroupingPage = this.rdoPageMultiple.Checked ? 1 : (this.rdoPageOne.Checked ? 2 : 1);
        this.letterPrintOption.LetterType = this.cboLetterType.SelectedIndex;
        this.letterPrintOption.SpaceCondensed = this.cboSpacing.SelectedIndex == 1;
        this.letterPrintOption.UseLegalSize = this.cboPaperSize.SelectedIndex == 1;
        for (int nItemIndex = 0; nItemIndex < this.gridViewData.Items.Count; ++nItemIndex)
        {
          switch (nItemIndex)
          {
            case 0:
              this.letterPrintOption.UseDue = this.gridViewData.Items[nItemIndex].Checked;
              break;
            case 1:
              this.letterPrintOption.UseCategory = this.gridViewData.Items[nItemIndex].Checked;
              break;
            case 2:
              this.letterPrintOption.UseOwnerName = this.gridViewData.Items[nItemIndex].Checked;
              break;
            case 3:
              this.letterPrintOption.UseCurrentStatus = this.gridViewData.Items[nItemIndex].Checked;
              this.letterPrintOption.StatusCurrentType = this.gridViewData.Items[nItemIndex].SubItems[1].Text != string.Empty ? 1 : 0;
              break;
            case 4:
              this.letterPrintOption.UseStatusAdded = this.gridViewData.Items[nItemIndex].Checked;
              this.letterPrintOption.StatusAddedType = this.getStatusTypeInteger(this.gridViewData.Items[nItemIndex].SubItems[1].Text);
              break;
            case 5:
              this.letterPrintOption.UseStatusFulfilled = this.gridViewData.Items[nItemIndex].Checked;
              this.letterPrintOption.StatusFulfilledType = this.getStatusTypeInteger(this.gridViewData.Items[nItemIndex].SubItems[1].Text);
              break;
            case 6:
              this.letterPrintOption.UseStatusReceived = this.gridViewData.Items[nItemIndex].Checked;
              this.letterPrintOption.StatusReceivedType = this.getStatusTypeInteger(this.gridViewData.Items[nItemIndex].SubItems[1].Text);
              break;
            case 7:
              this.letterPrintOption.UseStatusReviewed = this.gridViewData.Items[nItemIndex].Checked;
              this.letterPrintOption.StatusReviewedType = this.getStatusTypeInteger(this.gridViewData.Items[nItemIndex].SubItems[1].Text);
              break;
            case 8:
              this.letterPrintOption.UseStatusRejected = this.gridViewData.Items[nItemIndex].Checked;
              this.letterPrintOption.StatusRejectedType = this.getStatusTypeInteger(this.gridViewData.Items[nItemIndex].SubItems[1].Text);
              break;
            case 9:
              this.letterPrintOption.UseStatusCleared = this.gridViewData.Items[nItemIndex].Checked;
              this.letterPrintOption.StatusClearedType = this.getStatusTypeInteger(this.gridViewData.Items[nItemIndex].SubItems[1].Text);
              break;
            case 10:
              this.letterPrintOption.UseStatusWaived = this.gridViewData.Items[nItemIndex].Checked;
              this.letterPrintOption.StatusWaivedType = this.getStatusTypeInteger(this.gridViewData.Items[nItemIndex].SubItems[1].Text);
              break;
          }
        }
        this.DialogResult = DialogResult.OK;
      }
    }

    private void btnEdit_Click(object sender, EventArgs e)
    {
      if (this.gridViewData.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a condition status to edit.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        using (StatusPrintOptionForm statusPrintOptionForm = new StatusPrintOptionForm(this.gridViewData.SelectedItems[0].Index == 3, this.gridViewData.SelectedItems[0].SubItems[1].Text))
        {
          if (statusPrintOptionForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          if (this.gridViewData.SelectedItems[0].Index == 3)
            this.gridViewData.SelectedItems[0].SubItems[1].Text = statusPrintOptionForm.PrintOption == 1 ? this.getStatusType(statusPrintOptionForm.PrintOption) : "";
          else
            this.gridViewData.SelectedItems[0].SubItems[1].Text = this.getStatusType(statusPrintOptionForm.PrintOption);
        }
      }
    }

    private void gridViewData_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnEdit.Enabled = true;
      if (this.gridViewData.SelectedItems.Count == 0 || this.gridViewData.SelectedItems.Count > 1)
      {
        this.btnEdit.Enabled = false;
      }
      else
      {
        if (this.gridViewData.SelectedItems[0].Index > 2)
          return;
        this.btnEdit.Enabled = false;
      }
    }

    private void btnDeleteEnding_Click(object sender, EventArgs e)
    {
      this.txtEndingPages.Text = string.Empty;
      this.txtEndingPages.Tag = (object) string.Empty;
    }

    private void btnDeleteStarting_Click(object sender, EventArgs e)
    {
      this.txtStartingPages.Text = string.Empty;
      this.txtStartingPages.Tag = (object) string.Empty;
    }

    private void addCustomPage(TextBox box)
    {
      using (TemplateSelectionDialog templateSelectionDialog = new TemplateSelectionDialog(this.session, TemplateSettingsType.CustomLetter, (FileSystemEntry) null, true))
      {
        if (templateSelectionDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        FileSystemEntry selectedItem = templateSelectionDialog.SelectedItem;
        box.Text = this.removeExtension(selectedItem.ToString());
        box.Tag = (object) selectedItem.ToString();
      }
    }

    private void btnAddStarting_Click(object sender, EventArgs e)
    {
      this.addCustomPage(this.txtStartingPages);
    }

    private void btnAddEnding_Click(object sender, EventArgs e)
    {
      this.addCustomPage(this.txtEndingPages);
    }

    private void chkUseStartingPage_Click(object sender, EventArgs e)
    {
      if (this.chkUseStartingPage.Checked)
      {
        if (this.cboPageNo.SelectedIndex == -1)
          this.cboPageNo.SelectedIndex = 0;
        this.cboPageNo.Enabled = true;
      }
      else
      {
        this.cboPageNo.SelectedIndex = -1;
        this.cboPageNo.Enabled = false;
      }
    }

    private void chkUseGroup_Click(object sender, EventArgs e)
    {
      this.panelGroupCondition.Enabled = this.chkUseGroup.Checked;
      this.panelGroupingPage.Enabled = this.chkUseGroup.Checked;
    }

    private void txtStartingPages_TextChanged(object sender, EventArgs e)
    {
      this.btnDeleteStarting.Enabled = this.txtStartingPages.Text != string.Empty;
    }

    private void txtEndingPages_TextChanged(object sender, EventArgs e)
    {
      this.btnDeleteEnding.Enabled = this.txtEndingPages.Text != string.Empty;
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
      this.dialogButtons1 = new DialogButtons();
      this.groupContainer1 = new GroupContainer();
      this.btnEdit = new StandardIconButton();
      this.gridViewData = new GridView();
      this.groupBox1 = new GroupBox();
      this.chkIncludeDesc = new CheckBox();
      this.label5 = new Label();
      this.cboConditionOption = new ComboBox();
      this.label4 = new Label();
      this.label1 = new Label();
      this.txtName = new TextBox();
      this.label2 = new Label();
      this.label3 = new Label();
      this.btnAddStarting = new StandardIconButton();
      this.btnDeleteEnding = new StandardIconButton();
      this.groupBox2 = new GroupBox();
      this.panelGroupingPage = new Panel();
      this.rdoPageMultiple = new RadioButton();
      this.rdoPageOne = new RadioButton();
      this.panelGroupCondition = new Panel();
      this.rdoGroupByDue = new RadioButton();
      this.rdoGroupByCategory = new RadioButton();
      this.rdoGroupByOwner = new RadioButton();
      this.chkUseGroup = new CheckBox();
      this.cboPageNo = new ComboBox();
      this.panel1 = new Panel();
      this.rdoSortByDue = new RadioButton();
      this.rdoSortByStatus = new RadioButton();
      this.rdoSortByCategory = new RadioButton();
      this.rdoSortByOwner = new RadioButton();
      this.chkUseStartingPage = new CheckBox();
      this.label9 = new Label();
      this.label8 = new Label();
      this.label7 = new Label();
      this.label6 = new Label();
      this.txtStartingPages = new TextBox();
      this.txtEndingPages = new TextBox();
      this.btnDeleteStarting = new StandardIconButton();
      this.btnAddEnding = new StandardIconButton();
      this.label10 = new Label();
      this.cboLetterType = new ComboBox();
      this.toolTip1 = new ToolTip(this.components);
      this.label11 = new Label();
      this.label12 = new Label();
      this.cboSpacing = new ComboBox();
      this.cboPaperSize = new ComboBox();
      this.groupContainer1.SuspendLayout();
      ((ISupportInitialize) this.btnEdit).BeginInit();
      this.groupBox1.SuspendLayout();
      ((ISupportInitialize) this.btnAddStarting).BeginInit();
      ((ISupportInitialize) this.btnDeleteEnding).BeginInit();
      this.groupBox2.SuspendLayout();
      this.panelGroupingPage.SuspendLayout();
      this.panelGroupCondition.SuspendLayout();
      this.panel1.SuspendLayout();
      ((ISupportInitialize) this.btnDeleteStarting).BeginInit();
      ((ISupportInitialize) this.btnAddEnding).BeginInit();
      this.SuspendLayout();
      this.dialogButtons1.Dock = DockStyle.Bottom;
      this.dialogButtons1.Location = new Point(0, 648);
      this.dialogButtons1.Name = "dialogButtons1";
      this.dialogButtons1.OKText = "&Save";
      this.dialogButtons1.Size = new Size(599, 44);
      this.dialogButtons1.TabIndex = 22;
      this.dialogButtons1.OK += new EventHandler(this.dialogButtons1_OK);
      this.groupContainer1.Controls.Add((Control) this.btnEdit);
      this.groupContainer1.Controls.Add((Control) this.gridViewData);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(11, 57);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(544, 245);
      this.groupContainer1.TabIndex = 1;
      this.groupContainer1.Text = "Available Condition Data";
      this.btnEdit.BackColor = Color.Transparent;
      this.btnEdit.Location = new Point(520, 4);
      this.btnEdit.Name = "btnEdit";
      this.btnEdit.Size = new Size(16, 16);
      this.btnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEdit.TabIndex = 1;
      this.btnEdit.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnEdit, "Edit Condition Data");
      this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
      this.gridViewData.BorderStyle = BorderStyle.None;
      gvColumn1.CheckBoxes = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Name";
      gvColumn1.Width = 350;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "Status Print Options";
      gvColumn2.Width = 192;
      this.gridViewData.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gridViewData.Dock = DockStyle.Fill;
      this.gridViewData.Location = new Point(1, 26);
      this.gridViewData.Name = "gridViewData";
      this.gridViewData.Size = new Size(542, 218);
      this.gridViewData.TabIndex = 0;
      this.gridViewData.SelectedIndexChanged += new EventHandler(this.gridViewData_SelectedIndexChanged);
      this.groupBox1.Controls.Add((Control) this.chkIncludeDesc);
      this.groupBox1.Controls.Add((Control) this.label5);
      this.groupBox1.Controls.Add((Control) this.cboConditionOption);
      this.groupBox1.Controls.Add((Control) this.label4);
      this.groupBox1.Controls.Add((Control) this.groupContainer1);
      this.groupBox1.Location = new Point(15, 107);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(570, 327);
      this.groupBox1.TabIndex = 3;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Content";
      this.chkIncludeDesc.AutoSize = true;
      this.chkIncludeDesc.Location = new Point(12, 308);
      this.chkIncludeDesc.Name = "chkIncludeDesc";
      this.chkIncludeDesc.Size = new Size(179, 17);
      this.chkIncludeDesc.TabIndex = 5;
      this.chkIncludeDesc.Text = "Include the condition description";
      this.chkIncludeDesc.UseVisualStyleBackColor = true;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(8, 41);
      this.label5.Name = "label5";
      this.label5.Size = new Size(88, 13);
      this.label5.TabIndex = 10;
      this.label5.Text = "Include this data:";
      this.cboConditionOption.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboConditionOption.FormattingEnabled = true;
      this.cboConditionOption.Items.AddRange(new object[3]
      {
        (object) "All Conditions",
        (object) "Open Conditions",
        (object) "Prompt to choose conditions when printing"
      });
      this.cboConditionOption.Location = new Point(152, 17);
      this.cboConditionOption.Name = "cboConditionOption";
      this.cboConditionOption.Size = new Size(403, 21);
      this.cboConditionOption.TabIndex = 4;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(8, 20);
      this.label4.Name = "label4";
      this.label4.Size = new Size(122, 13);
      this.label4.TabIndex = 4;
      this.label4.Text = "Include these conditions";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 13);
      this.label1.Name = "label1";
      this.label1.Size = new Size(65, 13);
      this.label1.TabIndex = 3;
      this.label1.Text = "Letter Name";
      this.txtName.Location = new Point(102, 10);
      this.txtName.Name = "txtName";
      this.txtName.ReadOnly = true;
      this.txtName.Size = new Size(483, 20);
      this.txtName.TabIndex = 0;
      this.txtName.TabStop = false;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(12, 59);
      this.label2.Name = "label2";
      this.label2.Size = new Size(76, 13);
      this.label2.TabIndex = 6;
      this.label2.Text = "Starting Pages";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(12, 82);
      this.label3.Name = "label3";
      this.label3.Size = new Size(73, 13);
      this.label3.TabIndex = 7;
      this.label3.Text = "Ending Pages";
      this.btnAddStarting.BackColor = Color.Transparent;
      this.btnAddStarting.Location = new Point(549, 59);
      this.btnAddStarting.Name = "btnAddStarting";
      this.btnAddStarting.Size = new Size(16, 16);
      this.btnAddStarting.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddStarting.TabIndex = 9;
      this.btnAddStarting.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnAddStarting, "Add Starting Page");
      this.btnAddStarting.Click += new EventHandler(this.btnAddStarting_Click);
      this.btnDeleteEnding.BackColor = Color.Transparent;
      this.btnDeleteEnding.Location = new Point(568, 82);
      this.btnDeleteEnding.Name = "btnDeleteEnding";
      this.btnDeleteEnding.Size = new Size(16, 16);
      this.btnDeleteEnding.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDeleteEnding.TabIndex = 10;
      this.btnDeleteEnding.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnDeleteEnding, "Delete Ending Page");
      this.btnDeleteEnding.Click += new EventHandler(this.btnDeleteEnding_Click);
      this.groupBox2.Controls.Add((Control) this.cboPaperSize);
      this.groupBox2.Controls.Add((Control) this.cboSpacing);
      this.groupBox2.Controls.Add((Control) this.label12);
      this.groupBox2.Controls.Add((Control) this.label11);
      this.groupBox2.Controls.Add((Control) this.panelGroupingPage);
      this.groupBox2.Controls.Add((Control) this.panelGroupCondition);
      this.groupBox2.Controls.Add((Control) this.chkUseGroup);
      this.groupBox2.Controls.Add((Control) this.cboPageNo);
      this.groupBox2.Controls.Add((Control) this.panel1);
      this.groupBox2.Controls.Add((Control) this.chkUseStartingPage);
      this.groupBox2.Controls.Add((Control) this.label9);
      this.groupBox2.Controls.Add((Control) this.label8);
      this.groupBox2.Controls.Add((Control) this.label7);
      this.groupBox2.Controls.Add((Control) this.label6);
      this.groupBox2.Location = new Point(15, 439);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(570, 214);
      this.groupBox2.TabIndex = 6;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Presentation";
      this.panelGroupingPage.Controls.Add((Control) this.rdoPageMultiple);
      this.panelGroupingPage.Controls.Add((Control) this.rdoPageOne);
      this.panelGroupingPage.Location = new Point(325, 141);
      this.panelGroupingPage.Name = "panelGroupingPage";
      this.panelGroupingPage.Size = new Size(230, 44);
      this.panelGroupingPage.TabIndex = 21;
      this.rdoPageMultiple.AutoSize = true;
      this.rdoPageMultiple.Location = new Point(3, 3);
      this.rdoPageMultiple.Name = "rdoPageMultiple";
      this.rdoPageMultiple.Size = new Size(170, 17);
      this.rdoPageMultiple.TabIndex = 22;
      this.rdoPageMultiple.TabStop = true;
      this.rdoPageMultiple.Text = "Show multiple groups per page";
      this.rdoPageMultiple.UseVisualStyleBackColor = true;
      this.rdoPageOne.AutoSize = true;
      this.rdoPageOne.Location = new Point(3, 21);
      this.rdoPageOne.Name = "rdoPageOne";
      this.rdoPageOne.Size = new Size(170, 17);
      this.rdoPageOne.TabIndex = 23;
      this.rdoPageOne.TabStop = true;
      this.rdoPageOne.Text = "Show only one group per page";
      this.rdoPageOne.UseVisualStyleBackColor = true;
      this.panelGroupCondition.Controls.Add((Control) this.rdoGroupByDue);
      this.panelGroupCondition.Controls.Add((Control) this.rdoGroupByCategory);
      this.panelGroupCondition.Controls.Add((Control) this.rdoGroupByOwner);
      this.panelGroupCondition.Location = new Point(325, 55);
      this.panelGroupCondition.Name = "panelGroupCondition";
      this.panelGroupCondition.Size = new Size(230, 58);
      this.panelGroupCondition.TabIndex = 17;
      this.rdoGroupByDue.AutoSize = true;
      this.rdoGroupByDue.Location = new Point(3, 3);
      this.rdoGroupByDue.Name = "rdoGroupByDue";
      this.rdoGroupByDue.Size = new Size(203, 17);
      this.rdoGroupByDue.TabIndex = 18;
      this.rdoGroupByDue.TabStop = true;
      this.rdoGroupByDue.Text = "When the condition is due (PTA, etc.)";
      this.rdoGroupByDue.UseVisualStyleBackColor = true;
      this.rdoGroupByCategory.AutoSize = true;
      this.rdoGroupByCategory.Location = new Point(3, 21);
      this.rdoGroupByCategory.Name = "rdoGroupByCategory";
      this.rdoGroupByCategory.Size = new Size(179, 17);
      this.rdoGroupByCategory.TabIndex = 19;
      this.rdoGroupByCategory.TabStop = true;
      this.rdoGroupByCategory.Text = "Category (Assets, Liabilities, etc.)";
      this.rdoGroupByCategory.UseVisualStyleBackColor = true;
      this.rdoGroupByOwner.AutoSize = true;
      this.rdoGroupByOwner.Location = new Point(3, 39);
      this.rdoGroupByOwner.Name = "rdoGroupByOwner";
      this.rdoGroupByOwner.Size = new Size(141, 17);
      this.rdoGroupByOwner.TabIndex = 20;
      this.rdoGroupByOwner.TabStop = true;
      this.rdoGroupByOwner.Text = "Condition Owner's Name";
      this.rdoGroupByOwner.UseVisualStyleBackColor = true;
      this.chkUseGroup.AutoSize = true;
      this.chkUseGroup.Location = new Point(313, 20);
      this.chkUseGroup.Name = "chkUseGroup";
      this.chkUseGroup.Size = new Size(148, 17);
      this.chkUseGroup.TabIndex = 16;
      this.chkUseGroup.Text = "Group conditions together";
      this.chkUseGroup.UseVisualStyleBackColor = true;
      this.chkUseGroup.Click += new EventHandler(this.chkUseGroup_Click);
      this.cboPageNo.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboPageNo.FormattingEnabled = true;
      this.cboPageNo.Items.AddRange(new object[9]
      {
        (object) "1",
        (object) "2",
        (object) "3",
        (object) "4",
        (object) "5",
        (object) "6",
        (object) "7",
        (object) "8",
        (object) "9"
      });
      this.cboPageNo.Location = new Point(171, 139);
      this.cboPageNo.Name = "cboPageNo";
      this.cboPageNo.Size = new Size(58, 21);
      this.cboPageNo.TabIndex = 13;
      this.panel1.Controls.Add((Control) this.rdoSortByDue);
      this.panel1.Controls.Add((Control) this.rdoSortByStatus);
      this.panel1.Controls.Add((Control) this.rdoSortByCategory);
      this.panel1.Controls.Add((Control) this.rdoSortByOwner);
      this.panel1.Location = new Point(23, 36);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(230, 77);
      this.panel1.TabIndex = 7;
      this.rdoSortByDue.AutoSize = true;
      this.rdoSortByDue.Location = new Point(3, 3);
      this.rdoSortByDue.Name = "rdoSortByDue";
      this.rdoSortByDue.Size = new Size(203, 17);
      this.rdoSortByDue.TabIndex = 8;
      this.rdoSortByDue.TabStop = true;
      this.rdoSortByDue.Text = "When the condition is due (PTA, etc.)";
      this.rdoSortByDue.UseVisualStyleBackColor = true;
      this.rdoSortByStatus.AutoSize = true;
      this.rdoSortByStatus.Location = new Point(3, 57);
      this.rdoSortByStatus.Name = "rdoSortByStatus";
      this.rdoSortByStatus.Size = new Size(102, 17);
      this.rdoSortByStatus.TabIndex = 11;
      this.rdoSortByStatus.TabStop = true;
      this.rdoSortByStatus.Text = "Condition Status";
      this.rdoSortByStatus.UseVisualStyleBackColor = true;
      this.rdoSortByCategory.AutoSize = true;
      this.rdoSortByCategory.Location = new Point(3, 21);
      this.rdoSortByCategory.Name = "rdoSortByCategory";
      this.rdoSortByCategory.Size = new Size(179, 17);
      this.rdoSortByCategory.TabIndex = 9;
      this.rdoSortByCategory.TabStop = true;
      this.rdoSortByCategory.Text = "Category (Assets, Liabilities, etc.)";
      this.rdoSortByCategory.UseVisualStyleBackColor = true;
      this.rdoSortByOwner.AutoSize = true;
      this.rdoSortByOwner.Location = new Point(3, 39);
      this.rdoSortByOwner.Name = "rdoSortByOwner";
      this.rdoSortByOwner.Size = new Size(141, 17);
      this.rdoSortByOwner.TabIndex = 10;
      this.rdoSortByOwner.TabStop = true;
      this.rdoSortByOwner.Text = "Condition Owner's Name";
      this.rdoSortByOwner.UseVisualStyleBackColor = true;
      this.chkUseStartingPage.AutoSize = true;
      this.chkUseStartingPage.Location = new Point(23, 141);
      this.chkUseStartingPage.Name = "chkUseStartingPage";
      this.chkUseStartingPage.Size = new Size(142, 17);
      this.chkUseStartingPage.TabIndex = 12;
      this.chkUseStartingPage.Text = "Show, starting with page";
      this.chkUseStartingPage.UseVisualStyleBackColor = true;
      this.chkUseStartingPage.Click += new EventHandler(this.chkUseStartingPage_Click);
      this.label9.AutoSize = true;
      this.label9.Location = new Point(310, 122);
      this.label9.Name = "label9";
      this.label9.Size = new Size(98, 13);
      this.label9.TabIndex = 14;
      this.label9.Text = "Grouping per page:";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(310, 39);
      this.label8.Name = "label8";
      this.label8.Size = new Size(104, 13);
      this.label8.TabIndex = 13;
      this.label8.Text = "Group conditions by:";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(9, 122);
      this.label7.Name = "label7";
      this.label7.Size = new Size(269, 13);
      this.label7.TabIndex = 12;
      this.label7.Text = "Page Numbers for the conditional approval letter pages:";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(9, 20);
      this.label6.Name = "label6";
      this.label6.Size = new Size(95, 13);
      this.label6.TabIndex = 11;
      this.label6.Text = "Sort Conditions by:";
      this.txtStartingPages.Location = new Point(102, 57);
      this.txtStartingPages.Name = "txtStartingPages";
      this.txtStartingPages.ReadOnly = true;
      this.txtStartingPages.Size = new Size(444, 20);
      this.txtStartingPages.TabIndex = 23;
      this.txtStartingPages.TabStop = false;
      this.txtStartingPages.TextChanged += new EventHandler(this.txtStartingPages_TextChanged);
      this.txtEndingPages.Location = new Point(102, 80);
      this.txtEndingPages.Name = "txtEndingPages";
      this.txtEndingPages.ReadOnly = true;
      this.txtEndingPages.Size = new Size(444, 20);
      this.txtEndingPages.TabIndex = 24;
      this.txtEndingPages.TabStop = false;
      this.txtEndingPages.TextChanged += new EventHandler(this.txtEndingPages_TextChanged);
      this.btnDeleteStarting.BackColor = Color.Transparent;
      this.btnDeleteStarting.Location = new Point(568, 59);
      this.btnDeleteStarting.Name = "btnDeleteStarting";
      this.btnDeleteStarting.Size = new Size(16, 16);
      this.btnDeleteStarting.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDeleteStarting.TabIndex = 25;
      this.btnDeleteStarting.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnDeleteStarting, "Delete Starting Page");
      this.btnDeleteStarting.Click += new EventHandler(this.btnDeleteStarting_Click);
      this.btnAddEnding.BackColor = Color.Transparent;
      this.btnAddEnding.Location = new Point(549, 82);
      this.btnAddEnding.Name = "btnAddEnding";
      this.btnAddEnding.Size = new Size(16, 16);
      this.btnAddEnding.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddEnding.TabIndex = 26;
      this.btnAddEnding.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnAddEnding, "Add Ending Page");
      this.btnAddEnding.Click += new EventHandler(this.btnAddEnding_Click);
      this.label10.AutoSize = true;
      this.label10.Location = new Point(12, 36);
      this.label10.Name = "label10";
      this.label10.Size = new Size(61, 13);
      this.label10.TabIndex = 27;
      this.label10.Text = "Letter Type";
      this.cboLetterType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboLetterType.FormattingEnabled = true;
      this.cboLetterType.Items.AddRange(new object[2]
      {
        (object) "External",
        (object) "Internal"
      });
      this.cboLetterType.Location = new Point(102, 33);
      this.cboLetterType.Name = "cboLetterType";
      this.cboLetterType.Size = new Size(142, 21);
      this.cboLetterType.TabIndex = 1;
      this.label11.AutoSize = true;
      this.label11.Location = new Point(9, 166);
      this.label11.Name = "label11";
      this.label11.Size = new Size(69, 13);
      this.label11.TabIndex = 20;
      this.label11.Text = "Line Spacing";
      this.label12.AutoSize = true;
      this.label12.Location = new Point(9, 193);
      this.label12.Name = "label12";
      this.label12.Size = new Size(58, 13);
      this.label12.TabIndex = 21;
      this.label12.Text = "Paper Size";
      this.cboSpacing.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboSpacing.FormattingEnabled = true;
      this.cboSpacing.Items.AddRange(new object[2]
      {
        (object) "Normal",
        (object) "Condensed"
      });
      this.cboSpacing.Location = new Point(87, 163);
      this.cboSpacing.Name = "cboSpacing";
      this.cboSpacing.Size = new Size(104, 21);
      this.cboSpacing.TabIndex = 14;
      this.cboPaperSize.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboPaperSize.FormattingEnabled = true;
      this.cboPaperSize.Items.AddRange(new object[2]
      {
        (object) "Letter",
        (object) "Legal"
      });
      this.cboPaperSize.Location = new Point(87, 187);
      this.cboPaperSize.Name = "cboPaperSize";
      this.cboPaperSize.Size = new Size(104, 21);
      this.cboPaperSize.TabIndex = 15;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(599, 692);
      this.Controls.Add((Control) this.cboLetterType);
      this.Controls.Add((Control) this.label10);
      this.Controls.Add((Control) this.btnAddEnding);
      this.Controls.Add((Control) this.btnDeleteStarting);
      this.Controls.Add((Control) this.txtEndingPages);
      this.Controls.Add((Control) this.txtStartingPages);
      this.Controls.Add((Control) this.groupBox2);
      this.Controls.Add((Control) this.btnDeleteEnding);
      this.Controls.Add((Control) this.btnAddStarting);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.txtName);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.dialogButtons1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ConditionalLetterSetupForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Condition Form Details";
      this.groupContainer1.ResumeLayout(false);
      ((ISupportInitialize) this.btnEdit).EndInit();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      ((ISupportInitialize) this.btnAddStarting).EndInit();
      ((ISupportInitialize) this.btnDeleteEnding).EndInit();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.panelGroupingPage.ResumeLayout(false);
      this.panelGroupingPage.PerformLayout();
      this.panelGroupCondition.ResumeLayout(false);
      this.panelGroupCondition.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      ((ISupportInitialize) this.btnDeleteStarting).EndInit();
      ((ISupportInitialize) this.btnAddEnding).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
