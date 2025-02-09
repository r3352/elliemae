// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DashBoard.SummaryFieldControl
// Assembly: DashBoard, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 99BFBD49-67F8-470C-81BC-FC4FAEA6C933
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DashBoard.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.DashBoard
{
  public class SummaryFieldControl : UserControl
  {
    private SummaryFieldControl.FieldSelectionOption[] fieldSelectionOptions1 = new SummaryFieldControl.FieldSelectionOption[3]
    {
      new SummaryFieldControl.FieldSelectionOption("Dashboard.LoanCount", "Number of Loans"),
      new SummaryFieldControl.FieldSelectionOption("Loan.TotalLoanAmount", "Total Loan Amount"),
      new SummaryFieldControl.FieldSelectionOption("Dashboard.Other", "Other")
    };
    private SummaryFieldControl.FieldSelectionOption[] fieldSelectionOptions2 = new SummaryFieldControl.FieldSelectionOption[4]
    {
      new SummaryFieldControl.FieldSelectionOption("Dashboard.NoSummary", ""),
      new SummaryFieldControl.FieldSelectionOption("Dashboard.LoanCount", "Number of Loans"),
      new SummaryFieldControl.FieldSelectionOption("Loan.TotalLoanAmount", "Total Loan Amount"),
      new SummaryFieldControl.FieldSelectionOption("Dashboard.Other", "Other")
    };
    private SummaryFieldControl.SummarySelectionOption[] summarySelectionOptions = new SummaryFieldControl.SummarySelectionOption[2]
    {
      new SummaryFieldControl.SummarySelectionOption(4, "Average"),
      new SummaryFieldControl.SummarySelectionOption(3, "Total")
    };
    private LoanReportFieldDefs fieldDefinitions;
    private LoanReportFieldDef origFieldDefinition1;
    private ColumnSummaryType origSummaryType1;
    private LoanReportFieldDef currFieldDefinition1;
    private ColumnSummaryType currSummaryType1;
    private LoanReportFieldDef origFieldDefinition2;
    private ColumnSummaryType origSummaryType2;
    private LoanReportFieldDef currFieldDefinition2;
    private ColumnSummaryType currSummaryType2;
    private LoanReportFieldDef origFieldDefinition3;
    private ColumnSummaryType origSummaryType3;
    private LoanReportFieldDef currFieldDefinition3;
    private ColumnSummaryType currSummaryType3;
    private IContainer components;
    private ComboBox cboField3;
    private ComboBox cboField2;
    private ComboBox cboField1;
    private PictureBox picField1;
    private PictureBox picField2;
    private PictureBox picField3;
    private TextBox txtField1;
    private TextBox txtField2;
    private TextBox txtField3;
    private ComboBox cboSummaryType1;
    private ComboBox cboSummaryType2;
    private ComboBox cboSummaryType3;
    private ImageList imgList;
    private ToolTip toolTip1;

    [Browsable(false)]
    public LoanReportFieldDefs FieldDefinitions
    {
      get => this.fieldDefinitions;
      set
      {
        if (this.DesignMode)
          return;
        this.fieldDefinitions = value;
      }
    }

    [Browsable(false)]
    public LoanReportFieldDef SummaryFieldDefinition1
    {
      get => this.currFieldDefinition1;
      set
      {
        if (this.DesignMode || value == null)
          return;
        this.origFieldDefinition1 = value;
        this.currFieldDefinition1 = value;
        this.setFieldSelection1();
      }
    }

    [Browsable(false)]
    public LoanReportFieldDef SummaryFieldDefinition2
    {
      get => this.currFieldDefinition2;
      set
      {
        if (this.DesignMode || value == null)
          return;
        this.origFieldDefinition2 = value;
        this.currFieldDefinition2 = value;
        this.setFieldSelection2();
      }
    }

    [Browsable(false)]
    public LoanReportFieldDef SummaryFieldDefinition3
    {
      get => this.currFieldDefinition3;
      set
      {
        if (this.DesignMode || value == null)
          return;
        this.origFieldDefinition3 = value;
        this.currFieldDefinition3 = value;
        this.setFieldSelection3();
      }
    }

    [Browsable(false)]
    public ColumnSummaryType SummaryType1
    {
      get => this.currSummaryType1;
      set
      {
        if (this.DesignMode || value == ColumnSummaryType.None)
          return;
        this.origSummaryType1 = value;
        this.currSummaryType1 = value;
        this.setFieldSelection1();
      }
    }

    [Browsable(false)]
    public ColumnSummaryType SummaryType2
    {
      get => this.currSummaryType2;
      set
      {
        if (this.DesignMode || value == ColumnSummaryType.None)
          return;
        this.origSummaryType2 = value;
        this.currSummaryType2 = value;
        this.setFieldSelection2();
      }
    }

    [Browsable(false)]
    public ColumnSummaryType SummaryType3
    {
      get => this.currSummaryType3;
      set
      {
        if (this.DesignMode || value == ColumnSummaryType.None)
          return;
        this.origSummaryType3 = value;
        this.currSummaryType3 = value;
        this.setFieldSelection3();
      }
    }

    public SummaryFieldControl()
    {
      this.InitializeComponent();
      if (this.DesignMode)
        return;
      this.txtField3.Visible = false;
      this.setIconButton(this.picField3, false);
      this.cboSummaryType3.Visible = false;
      this.cboField1.DataSource = this.fieldSelectionOptions1.Clone();
      this.cboField1.DisplayMember = "Description";
      this.cboField1.ValueMember = "CriterionName";
      this.cboField2.DataSource = this.fieldSelectionOptions2.Clone();
      this.cboField2.DisplayMember = "Description";
      this.cboField2.ValueMember = "CriterionName";
      this.cboField3.DataSource = this.fieldSelectionOptions2.Clone();
      this.cboField3.DisplayMember = "Description";
      this.cboField3.ValueMember = "CriterionName";
      this.cboSummaryType1.DataSource = this.summarySelectionOptions.Clone();
      this.cboSummaryType1.DisplayMember = "Name";
      this.cboSummaryType1.ValueMember = "Id";
      this.cboSummaryType2.DataSource = this.summarySelectionOptions.Clone();
      this.cboSummaryType2.DisplayMember = "Name";
      this.cboSummaryType2.ValueMember = "Id";
      this.cboSummaryType3.DataSource = this.summarySelectionOptions.Clone();
      this.cboSummaryType3.DisplayMember = "Name";
      this.cboSummaryType3.ValueMember = "Id";
    }

    private void setFieldSelection1()
    {
      if (this.origFieldDefinition1 == null)
        return;
      if ("Dashboard.LoanCount" == this.currFieldDefinition1.CriterionFieldName || "Loan.TotalLoanAmount" == this.currFieldDefinition1.CriterionFieldName)
      {
        this.currSummaryType1 = "Dashboard.LoanCount" == this.currFieldDefinition1.CriterionFieldName ? ColumnSummaryType.Count : ColumnSummaryType.Total;
        this.cboField1.SelectedValue = (object) this.currFieldDefinition1.CriterionFieldName;
        this.txtField1.Visible = false;
        this.setIconButton(this.picField1, false);
        this.cboSummaryType1.Visible = false;
      }
      else
      {
        this.cboField1.SelectedValue = (object) "Dashboard.Other";
        this.txtField1.Text = this.currFieldDefinition1.Description;
        this.cboSummaryType1.SelectedValue = (object) (int) this.currSummaryType1;
        this.txtField1.Visible = true;
        this.setIconButton(this.picField1, true);
        this.cboSummaryType1.Visible = true;
      }
      if (!(this.origFieldDefinition1.CriterionFieldName != this.currFieldDefinition1.CriterionFieldName))
        return;
      this.OnDataChanged(EventArgs.Empty);
    }

    private void setFieldSelection2()
    {
      if (this.origFieldDefinition2 == null)
        return;
      if ("Dashboard.LoanCount" == this.currFieldDefinition2.CriterionFieldName || "Loan.TotalLoanAmount" == this.currFieldDefinition2.CriterionFieldName)
      {
        this.currSummaryType2 = "Dashboard.LoanCount" == this.currFieldDefinition2.CriterionFieldName ? ColumnSummaryType.Count : ColumnSummaryType.Total;
        this.cboField2.SelectedValue = (object) this.currFieldDefinition2.CriterionFieldName;
        this.txtField2.Visible = false;
        this.setIconButton(this.picField2, false);
        this.cboSummaryType2.Visible = false;
      }
      else if ("Dashboard.NoSummary" != this.currFieldDefinition2.CriterionFieldName)
      {
        this.cboField2.SelectedValue = (object) "Dashboard.Other";
        this.txtField2.Text = this.currFieldDefinition2.Description;
        this.cboSummaryType2.SelectedValue = (object) (int) this.currSummaryType2;
        this.txtField2.Visible = true;
        this.setIconButton(this.picField2, true);
        this.cboSummaryType2.Visible = true;
      }
      else
      {
        this.cboField2.SelectedValue = (object) "Dashboard.NoSummary";
        this.txtField2.Text = string.Empty;
        this.cboSummaryType2.SelectedValue = (object) 0;
        this.txtField2.Visible = false;
        this.setIconButton(this.picField2, false);
        this.cboSummaryType2.Visible = false;
      }
      if (!(this.origFieldDefinition2.CriterionFieldName != this.currFieldDefinition2.CriterionFieldName))
        return;
      this.OnDataChanged(EventArgs.Empty);
    }

    private void setFieldSelection3()
    {
      if (this.origFieldDefinition3 == null)
        return;
      if ("Dashboard.LoanCount" == this.currFieldDefinition3.CriterionFieldName || "Loan.TotalLoanAmount" == this.currFieldDefinition3.CriterionFieldName)
      {
        this.currSummaryType3 = "Dashboard.LoanCount" == this.currFieldDefinition3.CriterionFieldName ? ColumnSummaryType.Count : ColumnSummaryType.Total;
        this.cboField3.SelectedValue = (object) this.currFieldDefinition3.CriterionFieldName;
        this.txtField3.Visible = false;
        this.setIconButton(this.picField3, false);
        this.cboSummaryType3.Visible = false;
      }
      else if ("Dashboard.NoSummary" != this.currFieldDefinition3.CriterionFieldName)
      {
        this.cboField3.SelectedValue = (object) "Dashboard.Other";
        this.txtField3.Text = this.currFieldDefinition3.Description;
        this.cboSummaryType3.SelectedValue = (object) (int) this.currSummaryType3;
        this.txtField3.Visible = true;
        this.setIconButton(this.picField3, true);
        this.cboSummaryType3.Visible = true;
      }
      else
      {
        this.cboField3.SelectedValue = (object) "Dashboard.NoSummary";
        this.txtField3.Text = string.Empty;
        this.cboSummaryType3.SelectedValue = (object) 0;
        this.txtField3.Visible = false;
        this.setIconButton(this.picField3, false);
        this.cboSummaryType3.Visible = false;
      }
      if (!(this.origFieldDefinition3.CriterionFieldName != this.currFieldDefinition3.CriterionFieldName))
        return;
      this.OnDataChanged(EventArgs.Empty);
    }

    private void setIconButton(PictureBox pictureBox, bool visible)
    {
      pictureBox.Visible = visible;
      if (!visible)
        return;
      if (this.Parent.Enabled)
      {
        pictureBox.Image = this.imgList.Images["imgSearch"];
        pictureBox.Enabled = true;
      }
      else
      {
        pictureBox.Image = this.imgList.Images["imgSearchDisabled"];
        pictureBox.Enabled = false;
      }
    }

    private void cboField1_SelectionChangeCommitted(object sender, EventArgs e)
    {
      if (this.currFieldDefinition1.CriterionFieldName == (string) this.cboField1.SelectedValue)
        return;
      if ("Dashboard.Other" == (string) this.cboField1.SelectedValue)
      {
        this.picField1_Click(sender, e);
      }
      else
      {
        this.currFieldDefinition1 = this.fieldDefinitions.GetFieldByCriterionName((string) this.cboField1.SelectedValue);
        this.setFieldSelection1();
      }
    }

    private void cboField2_SelectionChangeCommitted(object sender, EventArgs e)
    {
      if (this.currFieldDefinition2.CriterionFieldName == (string) this.cboField2.SelectedValue)
        return;
      if ("Dashboard.Other" == (string) this.cboField2.SelectedValue)
      {
        this.picField2_Click(sender, e);
      }
      else
      {
        this.currFieldDefinition2 = this.fieldDefinitions.GetFieldByCriterionName((string) this.cboField2.SelectedValue);
        this.setFieldSelection2();
      }
    }

    private void cboField3_SelectionChangeCommitted(object sender, EventArgs e)
    {
      if (this.currFieldDefinition3.CriterionFieldName == (string) this.cboField3.SelectedValue)
        return;
      if ("Dashboard.Other" == (string) this.cboField3.SelectedValue)
      {
        this.picField3_Click(sender, e);
      }
      else
      {
        this.currFieldDefinition3 = this.fieldDefinitions.GetFieldByCriterionName((string) this.cboField3.SelectedValue);
        this.setFieldSelection3();
      }
    }

    private void cboSummaryType1_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.currSummaryType1 = (ColumnSummaryType) this.cboSummaryType1.SelectedValue;
      if (this.origSummaryType1 == this.currSummaryType1)
        return;
      this.OnDataChanged(EventArgs.Empty);
    }

    private void cboSummaryType2_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.currSummaryType2 = (ColumnSummaryType) this.cboSummaryType2.SelectedValue;
      if (this.origSummaryType2 == this.currSummaryType2)
        return;
      this.OnDataChanged(EventArgs.Empty);
    }

    private void cboSummaryType3_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.currSummaryType3 = (ColumnSummaryType) this.cboSummaryType3.SelectedValue;
      if (this.origSummaryType3 == this.currSummaryType3)
        return;
      this.OnDataChanged(EventArgs.Empty);
    }

    private void picField1_Click(object sender, EventArgs e)
    {
      using (FindLoanFieldDialog findLoanFieldDialog = new FindLoanFieldDialog(this.fieldDefinitions, ReportingDatabaseColumnType.Numeric))
      {
        if (DialogResult.OK == findLoanFieldDialog.ShowDialog((IWin32Window) this))
        {
          this.currFieldDefinition1 = findLoanFieldDialog.GetSelectedField();
          this.currSummaryType1 = ColumnSummaryType.Total;
        }
        this.setFieldSelection1();
      }
    }

    private void picField2_Click(object sender, EventArgs e)
    {
      using (FindLoanFieldDialog findLoanFieldDialog = new FindLoanFieldDialog(this.fieldDefinitions, ReportingDatabaseColumnType.Numeric))
      {
        if (DialogResult.OK == findLoanFieldDialog.ShowDialog((IWin32Window) this))
        {
          this.currFieldDefinition2 = findLoanFieldDialog.GetSelectedField();
          this.currSummaryType2 = ColumnSummaryType.Total;
        }
        this.setFieldSelection2();
      }
    }

    private void picField3_Click(object sender, EventArgs e)
    {
      using (FindLoanFieldDialog findLoanFieldDialog = new FindLoanFieldDialog(this.fieldDefinitions, ReportingDatabaseColumnType.Numeric))
      {
        if (DialogResult.OK == findLoanFieldDialog.ShowDialog((IWin32Window) this))
        {
          this.currFieldDefinition3 = findLoanFieldDialog.GetSelectedField();
          this.currSummaryType3 = ColumnSummaryType.Total;
        }
        this.setFieldSelection3();
      }
    }

    private void picField_MouseEnter(object sender, EventArgs e)
    {
      ((PictureBox) sender).Image = this.imgList.Images["imgSearchMouseOver"];
    }

    private void picField_MouseLeave(object sender, EventArgs e)
    {
      ((PictureBox) sender).Image = this.imgList.Images["imgSearch"];
    }

    public event SummaryFieldControl.DataChangedEventHandler DataChangedEvent;

    protected virtual void OnDataChanged(EventArgs e)
    {
      if (this.DataChangedEvent == null)
        return;
      this.DataChangedEvent((object) this, e);
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SummaryFieldControl));
      this.cboField3 = new ComboBox();
      this.cboField2 = new ComboBox();
      this.cboField1 = new ComboBox();
      this.picField1 = new PictureBox();
      this.picField2 = new PictureBox();
      this.picField3 = new PictureBox();
      this.txtField1 = new TextBox();
      this.txtField2 = new TextBox();
      this.txtField3 = new TextBox();
      this.cboSummaryType1 = new ComboBox();
      this.cboSummaryType2 = new ComboBox();
      this.cboSummaryType3 = new ComboBox();
      this.imgList = new ImageList(this.components);
      this.toolTip1 = new ToolTip(this.components);
      ((ISupportInitialize) this.picField1).BeginInit();
      ((ISupportInitialize) this.picField2).BeginInit();
      ((ISupportInitialize) this.picField3).BeginInit();
      this.SuspendLayout();
      this.cboField3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.cboField3.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboField3.Font = new Font("Arial", 8.25f);
      this.cboField3.FormattingEnabled = true;
      this.cboField3.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "Number of Loans",
        (object) "Total Loan Amount",
        (object) "Other Numeric Field"
      });
      this.cboField3.Location = new Point(0, 48);
      this.cboField3.Name = "cboField3";
      this.cboField3.Size = new Size(121, 22);
      this.cboField3.TabIndex = 465;
      this.cboField3.SelectionChangeCommitted += new EventHandler(this.cboField3_SelectionChangeCommitted);
      this.cboField2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.cboField2.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboField2.Font = new Font("Arial", 8.25f);
      this.cboField2.FormattingEnabled = true;
      this.cboField2.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "Number of Loans",
        (object) "Total Loan Amount",
        (object) "Other Numeric Field"
      });
      this.cboField2.Location = new Point(0, 24);
      this.cboField2.Name = "cboField2";
      this.cboField2.Size = new Size(121, 22);
      this.cboField2.TabIndex = 464;
      this.cboField2.SelectionChangeCommitted += new EventHandler(this.cboField2_SelectionChangeCommitted);
      this.cboField1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.cboField1.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboField1.Font = new Font("Arial", 8.25f);
      this.cboField1.FormattingEnabled = true;
      this.cboField1.Items.AddRange(new object[3]
      {
        (object) "Number of Loans",
        (object) "Total Loan Amount",
        (object) "Other Numeric Field"
      });
      this.cboField1.Location = new Point(0, 0);
      this.cboField1.Name = "cboField1";
      this.cboField1.Size = new Size(121, 22);
      this.cboField1.TabIndex = 463;
      this.cboField1.SelectionChangeCommitted += new EventHandler(this.cboField1_SelectionChangeCommitted);
      this.picField1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.picField1.Image = (Image) componentResourceManager.GetObject("picField1.Image");
      this.picField1.Location = new Point(316, 3);
      this.picField1.Name = "picField1";
      this.picField1.Size = new Size(16, 16);
      this.picField1.TabIndex = 466;
      this.picField1.TabStop = false;
      this.picField1.Tag = (object) "0";
      this.toolTip1.SetToolTip((Control) this.picField1, "Search Field");
      this.picField1.Visible = false;
      this.picField1.MouseLeave += new EventHandler(this.picField_MouseLeave);
      this.picField1.Click += new EventHandler(this.picField1_Click);
      this.picField1.MouseEnter += new EventHandler(this.picField_MouseEnter);
      this.picField2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.picField2.Image = (Image) componentResourceManager.GetObject("picField2.Image");
      this.picField2.Location = new Point(316, 27);
      this.picField2.Name = "picField2";
      this.picField2.Size = new Size(16, 16);
      this.picField2.TabIndex = 467;
      this.picField2.TabStop = false;
      this.picField2.Tag = (object) "0";
      this.toolTip1.SetToolTip((Control) this.picField2, "Search Field");
      this.picField2.Visible = false;
      this.picField2.MouseLeave += new EventHandler(this.picField_MouseLeave);
      this.picField2.Click += new EventHandler(this.picField2_Click);
      this.picField2.MouseEnter += new EventHandler(this.picField_MouseEnter);
      this.picField3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.picField3.Image = (Image) componentResourceManager.GetObject("picField3.Image");
      this.picField3.Location = new Point(316, 51);
      this.picField3.Name = "picField3";
      this.picField3.Size = new Size(16, 16);
      this.picField3.TabIndex = 468;
      this.picField3.TabStop = false;
      this.picField3.Tag = (object) "0";
      this.toolTip1.SetToolTip((Control) this.picField3, "Search Field");
      this.picField3.MouseLeave += new EventHandler(this.picField_MouseLeave);
      this.picField3.Click += new EventHandler(this.picField3_Click);
      this.picField3.MouseEnter += new EventHandler(this.picField_MouseEnter);
      this.txtField1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtField1.BackColor = SystemColors.Control;
      this.txtField1.Font = new Font("Arial", 8.25f);
      this.txtField1.Location = new Point(126, 1);
      this.txtField1.Name = "txtField1";
      this.txtField1.ReadOnly = true;
      this.txtField1.Size = new Size(184, 20);
      this.txtField1.TabIndex = 469;
      this.txtField1.Visible = false;
      this.txtField2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtField2.BackColor = SystemColors.Control;
      this.txtField2.Font = new Font("Arial", 8.25f);
      this.txtField2.Location = new Point(126, 25);
      this.txtField2.Name = "txtField2";
      this.txtField2.ReadOnly = true;
      this.txtField2.Size = new Size(184, 20);
      this.txtField2.TabIndex = 470;
      this.txtField2.Visible = false;
      this.txtField3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtField3.BackColor = SystemColors.Control;
      this.txtField3.Font = new Font("Arial", 8.25f);
      this.txtField3.Location = new Point(126, 49);
      this.txtField3.Name = "txtField3";
      this.txtField3.ReadOnly = true;
      this.txtField3.Size = new Size(184, 20);
      this.txtField3.TabIndex = 471;
      this.cboSummaryType1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.cboSummaryType1.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboSummaryType1.Font = new Font("Arial", 8.25f);
      this.cboSummaryType1.FormattingEnabled = true;
      this.cboSummaryType1.Location = new Point(338, 0);
      this.cboSummaryType1.Name = "cboSummaryType1";
      this.cboSummaryType1.Size = new Size(81, 22);
      this.cboSummaryType1.TabIndex = 472;
      this.cboSummaryType1.Visible = false;
      this.cboSummaryType1.SelectionChangeCommitted += new EventHandler(this.cboSummaryType1_SelectionChangeCommitted);
      this.cboSummaryType2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.cboSummaryType2.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboSummaryType2.Font = new Font("Arial", 8.25f);
      this.cboSummaryType2.FormattingEnabled = true;
      this.cboSummaryType2.Location = new Point(338, 24);
      this.cboSummaryType2.Name = "cboSummaryType2";
      this.cboSummaryType2.Size = new Size(81, 22);
      this.cboSummaryType2.TabIndex = 473;
      this.cboSummaryType2.Visible = false;
      this.cboSummaryType2.SelectionChangeCommitted += new EventHandler(this.cboSummaryType2_SelectionChangeCommitted);
      this.cboSummaryType3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.cboSummaryType3.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboSummaryType3.Font = new Font("Arial", 8.25f);
      this.cboSummaryType3.FormattingEnabled = true;
      this.cboSummaryType3.Location = new Point(338, 48);
      this.cboSummaryType3.Name = "cboSummaryType3";
      this.cboSummaryType3.Size = new Size(81, 22);
      this.cboSummaryType3.TabIndex = 474;
      this.cboSummaryType3.SelectionChangeCommitted += new EventHandler(this.cboSummaryType3_SelectionChangeCommitted);
      this.imgList.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgList.ImageStream");
      this.imgList.TransparentColor = Color.Transparent;
      this.imgList.Images.SetKeyName(0, "imgSearch");
      this.imgList.Images.SetKeyName(1, "imgSearchMouseOver");
      this.imgList.Images.SetKeyName(2, "imgSearchDisabled");
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.cboSummaryType3);
      this.Controls.Add((Control) this.cboSummaryType2);
      this.Controls.Add((Control) this.cboSummaryType1);
      this.Controls.Add((Control) this.txtField3);
      this.Controls.Add((Control) this.txtField2);
      this.Controls.Add((Control) this.txtField1);
      this.Controls.Add((Control) this.picField3);
      this.Controls.Add((Control) this.picField2);
      this.Controls.Add((Control) this.picField1);
      this.Controls.Add((Control) this.cboField3);
      this.Controls.Add((Control) this.cboField2);
      this.Controls.Add((Control) this.cboField1);
      this.Name = nameof (SummaryFieldControl);
      this.Size = new Size(419, 72);
      ((ISupportInitialize) this.picField1).EndInit();
      ((ISupportInitialize) this.picField2).EndInit();
      ((ISupportInitialize) this.picField3).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public delegate void DataChangedEventHandler(object sender, EventArgs e);

    public class FieldSelectionOption
    {
      private string criterionName;
      private string description;

      public string CriterionName => this.criterionName;

      public string Description => this.description;

      public FieldSelectionOption(string criterionName, string description)
      {
        this.criterionName = criterionName;
        this.description = description;
      }
    }

    public class SummarySelectionOption
    {
      private int id;
      private string name;

      public int Id => this.id;

      public string Name => this.name;

      public SummarySelectionOption(int id, string name)
      {
        this.id = id;
        this.name = name;
      }

      public override string ToString() => this.name;
    }
  }
}
