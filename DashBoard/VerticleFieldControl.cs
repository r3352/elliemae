// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DashBoard.VerticleFieldControl
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
  public class VerticleFieldControl : UserControl
  {
    private LoanReportFieldDefs fieldDefinitions;
    private LoanReportFieldDef origFieldDefinition;
    private LoanReportFieldDef currFieldDefinition;
    private ColumnSummaryType origSummaryType;
    private ColumnSummaryType currSummaryType;
    private IContainer components;
    private ComboBox cboSummaryType;
    private RadioButton rbOtherNumeric;
    private RadioButton rbLoanCount;
    private PictureBox picSearch;
    private TextBox txtOtherNumeric;
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
    public LoanReportFieldDef VerticleFieldDefinition
    {
      get => this.currFieldDefinition;
      set
      {
        if (this.DesignMode || value == null)
          return;
        this.origFieldDefinition = value;
        this.currFieldDefinition = value;
        this.setFieldSelection();
      }
    }

    [Browsable(false)]
    public ColumnSummaryType SummaryType
    {
      get => this.currSummaryType;
      set
      {
        if (this.DesignMode)
          return;
        this.origSummaryType = value;
        this.currSummaryType = value;
        this.setFieldSelection();
      }
    }

    public VerticleFieldControl()
    {
      this.InitializeComponent();
      if (this.DesignMode)
        return;
      this.cboSummaryType.DataSource = SelectionOptions.SummaryTypeOptions2.Clone();
      this.cboSummaryType.DisplayMember = "Name";
      this.cboSummaryType.ValueMember = "Id";
    }

    private void setFieldSelection()
    {
      if (this.currFieldDefinition == null)
        return;
      if ("Dashboard.LoanCount" == this.currFieldDefinition.CriterionFieldName)
      {
        this.rbLoanCount.Checked = true;
        this.currSummaryType = ColumnSummaryType.Count;
        this.txtOtherNumeric.Text = string.Empty;
        this.setIconButton(this.picSearch, false);
        this.cboSummaryType.Enabled = false;
      }
      else
      {
        this.rbOtherNumeric.Checked = true;
        this.txtOtherNumeric.Text = this.currFieldDefinition.Description;
        this.setIconButton(this.picSearch, true);
        this.cboSummaryType.Enabled = true;
      }
      if (!this.cboSummaryType.Enabled)
        this.cboSummaryType.SelectedIndex = -1;
      else if (ColumnSummaryType.Average == this.currSummaryType)
        this.cboSummaryType.SelectedValue = (object) 4;
      else
        this.cboSummaryType.SelectedValue = (object) 3;
      if (!(this.origFieldDefinition.CriterionFieldName != this.currFieldDefinition.CriterionFieldName) && this.origSummaryType == this.currSummaryType)
        return;
      this.OnDataChanged(EventArgs.Empty);
    }

    private void setIconButton(PictureBox pictureBox, bool enable)
    {
      if (enable && this.Parent.Enabled)
      {
        pictureBox.Image = this.imgList.Images[pictureBox.Name];
        pictureBox.Enabled = true;
      }
      else
      {
        pictureBox.Image = this.imgList.Images[pictureBox.Name + "Disabled"];
        pictureBox.Enabled = false;
      }
    }

    private void rbLoanCount_Click(object sender, EventArgs e)
    {
      if (!("Dashboard.LoanCount" != this.currFieldDefinition.CriterionFieldName))
        return;
      this.currFieldDefinition = this.fieldDefinitions.GetFieldByCriterionName("Dashboard.LoanCount");
      this.currSummaryType = ColumnSummaryType.Count;
      this.setFieldSelection();
    }

    private void rbOtherNumeric_Click(object sender, EventArgs e)
    {
      this.picSearch_Click(sender, e);
    }

    private void picSearch_Click(object sender, EventArgs e)
    {
      using (FindLoanFieldDialog findLoanFieldDialog = new FindLoanFieldDialog(this.fieldDefinitions, ReportingDatabaseColumnType.Numeric))
      {
        if (DialogResult.OK == findLoanFieldDialog.ShowDialog((IWin32Window) this))
        {
          this.currFieldDefinition = findLoanFieldDialog.GetSelectedField();
          this.currSummaryType = ColumnSummaryType.Total;
        }
        this.setFieldSelection();
      }
    }

    private void picSearch_MouseEnter(object sender, EventArgs e)
    {
      PictureBox pictureBox = (PictureBox) sender;
      pictureBox.Image = this.imgList.Images[pictureBox.Name + "MouseOver"];
    }

    private void picSearch_MouseLeave(object sender, EventArgs e)
    {
      PictureBox pictureBox = (PictureBox) sender;
      if (!pictureBox.Enabled)
        return;
      pictureBox.Image = this.imgList.Images[pictureBox.Name];
    }

    private void cboSummaryType_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.currSummaryType = (ColumnSummaryType) this.cboSummaryType.SelectedValue;
      this.setFieldSelection();
    }

    public event VerticleFieldControl.DataChangedEventHandler DataChangedEvent;

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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (VerticleFieldControl));
      this.cboSummaryType = new ComboBox();
      this.rbOtherNumeric = new RadioButton();
      this.rbLoanCount = new RadioButton();
      this.picSearch = new PictureBox();
      this.txtOtherNumeric = new TextBox();
      this.imgList = new ImageList(this.components);
      this.toolTip1 = new ToolTip(this.components);
      ((ISupportInitialize) this.picSearch).BeginInit();
      this.SuspendLayout();
      this.cboSummaryType.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.cboSummaryType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboSummaryType.FormattingEnabled = true;
      this.cboSummaryType.Location = new Point(234, 18);
      this.cboSummaryType.Name = "cboSummaryType";
      this.cboSummaryType.Size = new Size(81, 21);
      this.cboSummaryType.TabIndex = 452;
      this.cboSummaryType.SelectionChangeCommitted += new EventHandler(this.cboSummaryType_SelectionChangeCommitted);
      this.rbOtherNumeric.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.rbOtherNumeric.AutoSize = true;
      this.rbOtherNumeric.Location = new Point(0, 22);
      this.rbOtherNumeric.Name = "rbOtherNumeric";
      this.rbOtherNumeric.Size = new Size(14, 13);
      this.rbOtherNumeric.TabIndex = 451;
      this.rbOtherNumeric.TabStop = true;
      this.rbOtherNumeric.UseVisualStyleBackColor = true;
      this.rbOtherNumeric.Click += new EventHandler(this.rbOtherNumeric_Click);
      this.rbLoanCount.AutoSize = true;
      this.rbLoanCount.Location = new Point(0, 0);
      this.rbLoanCount.Name = "rbLoanCount";
      this.rbLoanCount.Size = new Size(106, 17);
      this.rbLoanCount.TabIndex = 450;
      this.rbLoanCount.TabStop = true;
      this.rbLoanCount.Text = "Number of Loans";
      this.rbLoanCount.UseVisualStyleBackColor = true;
      this.rbLoanCount.Click += new EventHandler(this.rbLoanCount_Click);
      this.picSearch.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.picSearch.Image = (Image) componentResourceManager.GetObject("picSearch.Image");
      this.picSearch.Location = new Point(212, 20);
      this.picSearch.Name = "picSearch";
      this.picSearch.Size = new Size(16, 16);
      this.picSearch.TabIndex = 449;
      this.picSearch.TabStop = false;
      this.picSearch.Tag = (object) "0";
      this.toolTip1.SetToolTip((Control) this.picSearch, "Search Field");
      this.picSearch.MouseLeave += new EventHandler(this.picSearch_MouseLeave);
      this.picSearch.Click += new EventHandler(this.picSearch_Click);
      this.picSearch.MouseEnter += new EventHandler(this.picSearch_MouseEnter);
      this.txtOtherNumeric.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtOtherNumeric.BackColor = SystemColors.Control;
      this.txtOtherNumeric.Location = new Point(20, 19);
      this.txtOtherNumeric.Name = "txtOtherNumeric";
      this.txtOtherNumeric.ReadOnly = true;
      this.txtOtherNumeric.Size = new Size(186, 20);
      this.txtOtherNumeric.TabIndex = 448;
      this.imgList.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgList.ImageStream");
      this.imgList.TransparentColor = Color.Transparent;
      this.imgList.Images.SetKeyName(0, "picSearch");
      this.imgList.Images.SetKeyName(1, "picSearchMouseOver");
      this.imgList.Images.SetKeyName(2, "picSearchDisabled");
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.cboSummaryType);
      this.Controls.Add((Control) this.rbOtherNumeric);
      this.Controls.Add((Control) this.rbLoanCount);
      this.Controls.Add((Control) this.picSearch);
      this.Controls.Add((Control) this.txtOtherNumeric);
      this.Name = nameof (VerticleFieldControl);
      this.Size = new Size(315, 39);
      ((ISupportInitialize) this.picSearch).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public delegate void DataChangedEventHandler(object sender, EventArgs e);
  }
}
