// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DashBoard.GroupByControl
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
  public class GroupByControl : UserControl
  {
    private LoanReportFieldDefs fieldDefinitions;
    private LoanReportFieldDef origFieldDefinition;
    private LoanReportFieldDef currFieldDefinition;
    private IContainer components;
    private PictureBox picSearch;
    private TextBox txtGroupBy;
    private ImageList imgList;
    private RadioButton rbGroupBy;
    private RadioButton rbNoGroupBy;
    private RadioButton rbCurrentMilestone;
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
    public LoanReportFieldDef GroupByFieldDefinition
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

    public GroupByControl()
    {
      this.InitializeComponent();
      int num = this.DesignMode ? 1 : 0;
    }

    private void setFieldSelection()
    {
      if (this.currFieldDefinition != null && "Dashboard.NoGroupBy" == this.currFieldDefinition.CriterionFieldName)
      {
        this.rbNoGroupBy.Checked = true;
        this.txtGroupBy.Text = string.Empty;
        this.setIconButton(this.picSearch, false);
      }
      else if (this.currFieldDefinition != null && "Loan.CurrentMilestoneName" == this.currFieldDefinition.CriterionFieldName)
      {
        this.rbCurrentMilestone.Checked = true;
        this.txtGroupBy.Text = string.Empty;
        this.setIconButton(this.picSearch, false);
      }
      else if (this.currFieldDefinition != null)
      {
        this.rbGroupBy.Checked = true;
        this.txtGroupBy.Text = this.currFieldDefinition.Description;
        this.setIconButton(this.picSearch, true);
      }
      if (this.currFieldDefinition != null && this.origFieldDefinition != null && !(this.origFieldDefinition.CriterionFieldName != this.currFieldDefinition.CriterionFieldName))
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

    private void rbNoGroupBy_Click(object sender, EventArgs e)
    {
      if (this.currFieldDefinition != null && !("Dashboard.NoGroupBy" != this.currFieldDefinition.CriterionFieldName))
        return;
      this.currFieldDefinition = this.fieldDefinitions.GetFieldByCriterionName("Dashboard.NoGroupBy");
      this.setFieldSelection();
    }

    private void rbCurrentMilestone_Click(object sender, EventArgs e)
    {
      if (this.currFieldDefinition != null && !("Loan.CurrentMilestoneName" != this.currFieldDefinition.CriterionFieldName))
        return;
      this.currFieldDefinition = this.fieldDefinitions.GetFieldByCriterionName("Loan.CurrentMilestoneName");
      this.setFieldSelection();
    }

    private void rbGroupBy_Click(object sender, EventArgs e) => this.picSearch_Click(sender, e);

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

    public event GroupByControl.DataChangedEventHandler DataChangedEvent;

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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (GroupByControl));
      this.picSearch = new PictureBox();
      this.txtGroupBy = new TextBox();
      this.imgList = new ImageList(this.components);
      this.rbGroupBy = new RadioButton();
      this.rbNoGroupBy = new RadioButton();
      this.rbCurrentMilestone = new RadioButton();
      this.toolTip1 = new ToolTip(this.components);
      ((ISupportInitialize) this.picSearch).BeginInit();
      this.SuspendLayout();
      this.picSearch.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.picSearch.Image = (Image) componentResourceManager.GetObject("picSearch.Image");
      this.picSearch.InitialImage = (Image) componentResourceManager.GetObject("picSearch.InitialImage");
      this.picSearch.Location = new Point(212, 42);
      this.picSearch.Name = "picSearch";
      this.picSearch.Size = new Size(16, 16);
      this.picSearch.TabIndex = 460;
      this.picSearch.TabStop = false;
      this.picSearch.Tag = (object) "0";
      this.toolTip1.SetToolTip((Control) this.picSearch, "Search Field");
      this.picSearch.MouseLeave += new EventHandler(this.picSearch_MouseLeave);
      this.picSearch.Click += new EventHandler(this.picSearch_Click);
      this.picSearch.MouseEnter += new EventHandler(this.picSearch_MouseEnter);
      this.txtGroupBy.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtGroupBy.BackColor = SystemColors.Control;
      this.txtGroupBy.Location = new Point(20, 40);
      this.txtGroupBy.Name = "txtGroupBy";
      this.txtGroupBy.ReadOnly = true;
      this.txtGroupBy.Size = new Size(186, 20);
      this.txtGroupBy.TabIndex = 459;
      this.imgList.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgList.ImageStream");
      this.imgList.TransparentColor = Color.Transparent;
      this.imgList.Images.SetKeyName(0, "picSearch");
      this.imgList.Images.SetKeyName(1, "picSearchMouseOver");
      this.imgList.Images.SetKeyName(2, "picSearchDisabled");
      this.rbGroupBy.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.rbGroupBy.AutoSize = true;
      this.rbGroupBy.Location = new Point(0, 43);
      this.rbGroupBy.Name = "rbGroupBy";
      this.rbGroupBy.Size = new Size(14, 13);
      this.rbGroupBy.TabIndex = 462;
      this.rbGroupBy.TabStop = true;
      this.rbGroupBy.UseVisualStyleBackColor = true;
      this.rbGroupBy.Click += new EventHandler(this.rbGroupBy_Click);
      this.rbNoGroupBy.AutoSize = true;
      this.rbNoGroupBy.Location = new Point(0, 0);
      this.rbNoGroupBy.Name = "rbNoGroupBy";
      this.rbNoGroupBy.Size = new Size(111, 17);
      this.rbNoGroupBy.TabIndex = 461;
      this.rbNoGroupBy.TabStop = true;
      this.rbNoGroupBy.Text = "No Group By Field";
      this.rbNoGroupBy.UseVisualStyleBackColor = true;
      this.rbNoGroupBy.Click += new EventHandler(this.rbNoGroupBy_Click);
      this.rbCurrentMilestone.AutoSize = true;
      this.rbCurrentMilestone.Location = new Point(0, 21);
      this.rbCurrentMilestone.Name = "rbCurrentMilestone";
      this.rbCurrentMilestone.Size = new Size(135, 17);
      this.rbCurrentMilestone.TabIndex = 463;
      this.rbCurrentMilestone.TabStop = true;
      this.rbCurrentMilestone.Text = "Last Finished Milestone";
      this.rbCurrentMilestone.UseVisualStyleBackColor = true;
      this.rbCurrentMilestone.Click += new EventHandler(this.rbCurrentMilestone_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.rbCurrentMilestone);
      this.Controls.Add((Control) this.rbGroupBy);
      this.Controls.Add((Control) this.rbNoGroupBy);
      this.Controls.Add((Control) this.picSearch);
      this.Controls.Add((Control) this.txtGroupBy);
      this.Name = nameof (GroupByControl);
      this.Size = new Size(228, 60);
      ((ISupportInitialize) this.picSearch).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public delegate void DataChangedEventHandler(object sender, EventArgs e);
  }
}
