// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DashBoard.TimeFrameControl
// Assembly: DashBoard, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 99BFBD49-67F8-470C-81BC-FC4FAEA6C933
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DashBoard.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.DashBoard
{
  public class TimeFrameControl : UserControl
  {
    private LoanReportFieldDefs fieldDefinitions;
    private LoanReportFieldDef origFieldDefinition;
    private LoanReportFieldDef currFieldDefinition;
    private IContainer components;
    private RadioButton rbOtherDate;
    private RadioButton rbCompletedDate;
    private RadioButton rbStartedDate;
    private PictureBox picSearch;
    private TextBox txtOtherDate;
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
    public LoanReportFieldDef TimeFrameFieldDefinition
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

    public TimeFrameControl()
    {
      this.InitializeComponent();
      int num = this.DesignMode ? 1 : 0;
    }

    private void setFieldSelection()
    {
      if (this.currFieldDefinition != null && "Loan.DateFileOpened" == this.currFieldDefinition.CriterionFieldName)
      {
        this.rbStartedDate.Checked = true;
        this.txtOtherDate.Text = string.Empty;
        this.setIconButton(this.picSearch, false);
      }
      else if (this.currFieldDefinition != null && "Loan.DateCompleted" == this.currFieldDefinition.CriterionFieldName)
      {
        this.rbCompletedDate.Checked = true;
        this.txtOtherDate.Text = string.Empty;
        this.setIconButton(this.picSearch, false);
      }
      else
      {
        this.rbOtherDate.Checked = true;
        if (this.currFieldDefinition != null)
          this.txtOtherDate.Text = this.currFieldDefinition.Description;
        this.setIconButton(this.picSearch, true);
      }
      if (this.origFieldDefinition != null && !(this.origFieldDefinition.CriterionFieldName != this.currFieldDefinition.CriterionFieldName))
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

    private void rbStartedDate_Click(object sender, EventArgs e)
    {
      if (this.currFieldDefinition != null && !(this.currFieldDefinition.CriterionFieldName != "Loan.DateFileOpened"))
        return;
      this.currFieldDefinition = this.fieldDefinitions.GetFieldByCriterionName("Loan.DateFileOpened");
      this.setFieldSelection();
    }

    private void rbCompletedDate_Click(object sender, EventArgs e)
    {
      if (this.currFieldDefinition != null && !(this.currFieldDefinition.CriterionFieldName != "Loan.DateCompleted"))
        return;
      this.currFieldDefinition = this.fieldDefinitions.GetFieldByCriterionName("Loan.DateCompleted");
      this.setFieldSelection();
    }

    private void rbOtherDate_Click(object sender, EventArgs e) => this.picSearch_Click(sender, e);

    private void picSearch_Click(object sender, EventArgs e)
    {
      using (FindLoanFieldDialog findLoanFieldDialog = new FindLoanFieldDialog(this.fieldDefinitions, ReportingDatabaseColumnType.Date))
      {
        if (DialogResult.OK == findLoanFieldDialog.ShowDialog((IWin32Window) this))
          this.currFieldDefinition = findLoanFieldDialog.GetSelectedField();
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

    public event TimeFrameControl.DataChangedEventHandler DataChangedEvent;

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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (TimeFrameControl));
      this.rbOtherDate = new RadioButton();
      this.rbCompletedDate = new RadioButton();
      this.rbStartedDate = new RadioButton();
      this.picSearch = new PictureBox();
      this.txtOtherDate = new TextBox();
      this.imgList = new ImageList(this.components);
      this.toolTip1 = new ToolTip(this.components);
      ((ISupportInitialize) this.picSearch).BeginInit();
      this.SuspendLayout();
      this.rbOtherDate.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.rbOtherDate.AutoSize = true;
      this.rbOtherDate.Location = new Point(0, 44);
      this.rbOtherDate.Name = "rbOtherDate";
      this.rbOtherDate.Size = new Size(14, 13);
      this.rbOtherDate.TabIndex = 426;
      this.rbOtherDate.TabStop = true;
      this.rbOtherDate.UseVisualStyleBackColor = true;
      this.rbOtherDate.Click += new EventHandler(this.rbOtherDate_Click);
      this.rbCompletedDate.AutoSize = true;
      this.rbCompletedDate.Location = new Point(0, 21);
      this.rbCompletedDate.Name = "rbCompletedDate";
      this.rbCompletedDate.Size = new Size(101, 17);
      this.rbCompletedDate.TabIndex = 425;
      this.rbCompletedDate.TabStop = true;
      this.rbCompletedDate.Text = "Completed Date";
      this.rbCompletedDate.UseVisualStyleBackColor = true;
      this.rbCompletedDate.Click += new EventHandler(this.rbCompletedDate_Click);
      this.rbStartedDate.AutoSize = true;
      this.rbStartedDate.Location = new Point(0, 0);
      this.rbStartedDate.Name = "rbStartedDate";
      this.rbStartedDate.Size = new Size(104, 17);
      this.rbStartedDate.TabIndex = 424;
      this.rbStartedDate.TabStop = true;
      this.rbStartedDate.Text = "File Started Date";
      this.rbStartedDate.UseVisualStyleBackColor = true;
      this.rbStartedDate.Click += new EventHandler(this.rbStartedDate_Click);
      this.picSearch.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.picSearch.Image = (Image) componentResourceManager.GetObject("picSearch.Image");
      this.picSearch.Location = new Point(212, 42);
      this.picSearch.Name = "picSearch";
      this.picSearch.Size = new Size(16, 16);
      this.picSearch.TabIndex = 423;
      this.picSearch.TabStop = false;
      this.picSearch.Tag = (object) "0";
      this.toolTip1.SetToolTip((Control) this.picSearch, "Search Field");
      this.picSearch.Click += new EventHandler(this.picSearch_Click);
      this.picSearch.MouseEnter += new EventHandler(this.picSearch_MouseEnter);
      this.picSearch.MouseLeave += new EventHandler(this.picSearch_MouseLeave);
      this.txtOtherDate.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtOtherDate.BackColor = SystemColors.Control;
      this.txtOtherDate.Location = new Point(20, 40);
      this.txtOtherDate.Name = "txtOtherDate";
      this.txtOtherDate.ReadOnly = true;
      this.txtOtherDate.Size = new Size(186, 20);
      this.txtOtherDate.TabIndex = 422;
      this.imgList.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgList.ImageStream");
      this.imgList.TransparentColor = Color.Transparent;
      this.imgList.Images.SetKeyName(0, "picSearch");
      this.imgList.Images.SetKeyName(1, "picSearchMouseOver");
      this.imgList.Images.SetKeyName(2, "picSearchDisabled");
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.rbOtherDate);
      this.Controls.Add((Control) this.rbCompletedDate);
      this.Controls.Add((Control) this.rbStartedDate);
      this.Controls.Add((Control) this.picSearch);
      this.Controls.Add((Control) this.txtOtherDate);
      this.Name = nameof (TimeFrameControl);
      this.Size = new Size(228, 60);
      ((ISupportInitialize) this.picSearch).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public delegate void DataChangedEventHandler(object sender, EventArgs e);
  }
}
