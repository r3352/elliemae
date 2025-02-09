// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DashBoard.HorizontalFieldControl
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
  public class HorizontalFieldControl : UserControl
  {
    private LoanReportFieldDefs fieldDefinitions;
    private LoanReportFieldDef origFieldDefinition;
    private LoanReportFieldDef currFieldDefinition;
    private IContainer components;
    private RadioButton rbOtherString;
    private RadioButton rbCurrentMilestone;
    private PictureBox picSearch;
    private TextBox txtOtherString;
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
    public LoanReportFieldDef HorizontalFieldDefinition
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

    public HorizontalFieldControl()
    {
      this.InitializeComponent();
      int num = this.DesignMode ? 1 : 0;
    }

    private void setFieldSelection()
    {
      if ("Loan.CurrentMilestoneName" == this.currFieldDefinition.CriterionFieldName)
      {
        this.rbCurrentMilestone.Checked = true;
        this.txtOtherString.Text = string.Empty;
        this.setIconButton(this.picSearch, false);
      }
      else
      {
        this.rbOtherString.Checked = true;
        this.txtOtherString.Text = this.currFieldDefinition.Description;
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

    private void rbCurrentMilestone_Click(object sender, EventArgs e)
    {
      if (!(this.currFieldDefinition.CriterionFieldName != "Loan.CurrentMilestoneName"))
        return;
      this.currFieldDefinition = this.fieldDefinitions.GetFieldByCriterionName("Loan.CurrentMilestoneName");
      this.setFieldSelection();
    }

    private void rbOtherString_Click(object sender, EventArgs e) => this.picSearch_Click(sender, e);

    private void picSearch_Click(object sender, EventArgs e)
    {
      using (FindLoanFieldDialog findLoanFieldDialog = new FindLoanFieldDialog(this.fieldDefinitions, ReportingDatabaseColumnType.Text))
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

    public event HorizontalFieldControl.DataChangedEventHandler DataChangedEvent;

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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (HorizontalFieldControl));
      this.rbOtherString = new RadioButton();
      this.rbCurrentMilestone = new RadioButton();
      this.picSearch = new PictureBox();
      this.txtOtherString = new TextBox();
      this.imgList = new ImageList(this.components);
      this.toolTip1 = new ToolTip(this.components);
      ((ISupportInitialize) this.picSearch).BeginInit();
      this.SuspendLayout();
      this.rbOtherString.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.rbOtherString.AutoSize = true;
      this.rbOtherString.Location = new Point(0, 23);
      this.rbOtherString.Name = "rbOtherString";
      this.rbOtherString.Size = new Size(14, 13);
      this.rbOtherString.TabIndex = 456;
      this.rbOtherString.TabStop = true;
      this.rbOtherString.UseVisualStyleBackColor = true;
      this.rbOtherString.Click += new EventHandler(this.rbOtherString_Click);
      this.rbCurrentMilestone.AutoSize = true;
      this.rbCurrentMilestone.Location = new Point(0, 0);
      this.rbCurrentMilestone.Name = "rbCurrentMilestone";
      this.rbCurrentMilestone.Size = new Size(135, 17);
      this.rbCurrentMilestone.TabIndex = 455;
      this.rbCurrentMilestone.TabStop = true;
      this.rbCurrentMilestone.Text = "Last Finished Milestone";
      this.rbCurrentMilestone.UseVisualStyleBackColor = true;
      this.rbCurrentMilestone.Click += new EventHandler(this.rbCurrentMilestone_Click);
      this.picSearch.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.picSearch.Image = (Image) componentResourceManager.GetObject("picSearch.Image");
      this.picSearch.Location = new Point(212, 21);
      this.picSearch.Name = "picSearch";
      this.picSearch.Size = new Size(16, 16);
      this.picSearch.TabIndex = 454;
      this.picSearch.TabStop = false;
      this.picSearch.Tag = (object) "0";
      this.toolTip1.SetToolTip((Control) this.picSearch, "Search Field");
      this.picSearch.MouseLeave += new EventHandler(this.picSearch_MouseLeave);
      this.picSearch.Click += new EventHandler(this.picSearch_Click);
      this.picSearch.MouseEnter += new EventHandler(this.picSearch_MouseEnter);
      this.txtOtherString.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtOtherString.BackColor = SystemColors.Control;
      this.txtOtherString.Location = new Point(20, 19);
      this.txtOtherString.Name = "txtOtherString";
      this.txtOtherString.ReadOnly = true;
      this.txtOtherString.Size = new Size(186, 20);
      this.txtOtherString.TabIndex = 453;
      this.imgList.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgList.ImageStream");
      this.imgList.TransparentColor = Color.Transparent;
      this.imgList.Images.SetKeyName(0, "picSearch");
      this.imgList.Images.SetKeyName(1, "picSearchMouseOver");
      this.imgList.Images.SetKeyName(2, "picSearchDisabled");
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.rbOtherString);
      this.Controls.Add((Control) this.rbCurrentMilestone);
      this.Controls.Add((Control) this.picSearch);
      this.Controls.Add((Control) this.txtOtherString);
      this.Name = nameof (HorizontalFieldControl);
      this.Size = new Size(228, 39);
      ((ISupportInitialize) this.picSearch).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public delegate void DataChangedEventHandler(object sender, EventArgs e);
  }
}
