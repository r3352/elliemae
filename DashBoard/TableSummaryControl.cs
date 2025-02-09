// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DashBoard.TableSummaryControl
// Assembly: DashBoard, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 99BFBD49-67F8-470C-81BC-FC4FAEA6C933
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DashBoard.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.DashBoard
{
  public class TableSummaryControl : UserControl
  {
    private bool isMinSelected;
    private bool isMaxSelected;
    private bool isAverageSelected;
    private bool isTotalSelected;
    private IContainer components;
    private CheckBox chkMin;
    private CheckBox chkMax;
    private CheckBox chkAverage;
    private CheckBox chkTotal;

    public TableSummaryControl()
    {
      this.InitializeComponent();
      int num = this.DesignMode ? 1 : 0;
    }

    [Browsable(false)]
    public bool IncludeMin
    {
      get => this.isMinSelected;
      set
      {
        this.isMinSelected = value;
        this.chkMin.CheckedChanged -= new EventHandler(this.chk_CheckedChanged);
        this.updateUI(this.chkMin, value);
        this.chkMin.CheckedChanged += new EventHandler(this.chk_CheckedChanged);
      }
    }

    [Browsable(false)]
    public bool IncludeMax
    {
      get => this.isMaxSelected;
      set
      {
        this.isMaxSelected = value;
        this.chkMax.CheckedChanged -= new EventHandler(this.chk_CheckedChanged);
        this.updateUI(this.chkMax, value);
        this.chkMax.CheckedChanged += new EventHandler(this.chk_CheckedChanged);
      }
    }

    [Browsable(false)]
    public bool IncludeAverage
    {
      get => this.isAverageSelected;
      set
      {
        this.isAverageSelected = value;
        this.chkAverage.CheckedChanged -= new EventHandler(this.chk_CheckedChanged);
        this.updateUI(this.chkAverage, value);
        this.chkAverage.CheckedChanged += new EventHandler(this.chk_CheckedChanged);
      }
    }

    [Browsable(false)]
    public bool IncludeTotal
    {
      get => this.isTotalSelected;
      set
      {
        this.isTotalSelected = value;
        this.chkTotal.CheckedChanged -= new EventHandler(this.chk_CheckedChanged);
        this.updateUI(this.chkTotal, value);
        this.chkTotal.CheckedChanged += new EventHandler(this.chk_CheckedChanged);
      }
    }

    public bool HideTotalOption
    {
      set => this.chkTotal.Visible = !value;
    }

    private void updateUI(CheckBox control, bool isChecked) => control.Checked = isChecked;

    public event TableSummaryControl.DataChangedEventHandler DataChangedEvent;

    protected virtual void OnDataChanged(EventArgs e)
    {
      if (this.DataChangedEvent == null)
        return;
      this.DataChangedEvent((object) this, e);
    }

    private void chk_CheckedChanged(object sender, EventArgs e)
    {
      this.isMinSelected = this.chkMin.Checked;
      this.isMaxSelected = this.chkMax.Checked;
      this.isAverageSelected = this.chkAverage.Checked;
      this.isTotalSelected = this.chkTotal.Checked;
      this.OnDataChanged(e);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.chkMin = new CheckBox();
      this.chkMax = new CheckBox();
      this.chkAverage = new CheckBox();
      this.chkTotal = new CheckBox();
      this.SuspendLayout();
      this.chkMin.AutoSize = true;
      this.chkMin.Location = new Point(0, 0);
      this.chkMin.Name = "chkMin";
      this.chkMin.Size = new Size(43, 17);
      this.chkMin.TabIndex = 0;
      this.chkMin.Text = "Min";
      this.chkMin.UseVisualStyleBackColor = true;
      this.chkMin.CheckedChanged += new EventHandler(this.chk_CheckedChanged);
      this.chkMax.AutoSize = true;
      this.chkMax.Location = new Point(0, 23);
      this.chkMax.Name = "chkMax";
      this.chkMax.Size = new Size(46, 17);
      this.chkMax.TabIndex = 1;
      this.chkMax.Text = "Max";
      this.chkMax.UseVisualStyleBackColor = true;
      this.chkMax.CheckedChanged += new EventHandler(this.chk_CheckedChanged);
      this.chkAverage.AutoSize = true;
      this.chkAverage.Location = new Point(0, 46);
      this.chkAverage.Name = "chkAverage";
      this.chkAverage.Size = new Size(66, 17);
      this.chkAverage.TabIndex = 2;
      this.chkAverage.Text = "Average";
      this.chkAverage.UseVisualStyleBackColor = true;
      this.chkAverage.CheckedChanged += new EventHandler(this.chk_CheckedChanged);
      this.chkTotal.AutoSize = true;
      this.chkTotal.Location = new Point(0, 69);
      this.chkTotal.Name = "chkTotal";
      this.chkTotal.Size = new Size(50, 17);
      this.chkTotal.TabIndex = 3;
      this.chkTotal.Text = "Total";
      this.chkTotal.UseVisualStyleBackColor = true;
      this.chkTotal.CheckedChanged += new EventHandler(this.chk_CheckedChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.chkTotal);
      this.Controls.Add((Control) this.chkAverage);
      this.Controls.Add((Control) this.chkMax);
      this.Controls.Add((Control) this.chkMin);
      this.Name = nameof (TableSummaryControl);
      this.Size = new Size(66, 91);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public delegate void DataChangedEventHandler(object sender, EventArgs e);
  }
}
