// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.LoanMetricsDialog
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common.Metrics;
using EllieMae.Encompass.Client;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Definitions.Series;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public class LoanMetricsDialog : Form
  {
    private readonly System.Drawing.Color ElliColor = System.Drawing.Color.FromArgb((int) byte.MaxValue, 0, 106, 169);
    private readonly System.Drawing.Color PluginColor = System.Drawing.Color.Orange;
    private readonly System.Windows.Media.Brush ElliBrush = (System.Windows.Media.Brush) new SolidColorBrush(System.Windows.Media.Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 106, (byte) 169));
    private readonly System.Windows.Media.Brush PluginBrush = (System.Windows.Media.Brush) System.Windows.Media.Brushes.Orange;
    private IContainer components;
    private TabControl tabControl;
    private TabPage tpMetrics;
    private TabPage tpConsole;
    private TextBox txtConsole;
    private LiveCharts.WinForms.PieChart pcOverview;
    private Label lblOvTTime;
    private LiveCharts.WinForms.CartesianChart cartesianChart1;
    private LiveCharts.WinForms.CartesianChart ctBreakdown;
    private ListView lvPlugins;

    public LoanMetricsDialog() => this.InitializeComponent();

    public LoanMetricsDialog(LoanActivityMetricData metrics)
      : this()
    {
      this.LoanMetrics = metrics;
    }

    public LoanActivityMetricData LoanMetrics { get; set; }

    private void LoanInsightsDialog_Load(object sender, EventArgs e)
    {
      if (ApplicationLog.DebugEnabled)
      {
        if (!this.tabControl.TabPages.Contains(this.tpConsole))
          this.tabControl.TabPages.Add(this.tpConsole);
        if (this.LoanMetrics?.LogOutput != null)
          this.txtConsole.Text = this.LoanMetrics.LogOutput.ToString();
      }
      else
        this.tabControl.TabPages.Remove(this.tpConsole);
      this.pcOverview.InnerRadius = 50.0;
      this.pcOverview.LegendLocation = LegendLocation.Right;
      this.pcOverview.Series = new SeriesCollection();
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      int num4 = 0;
      int num5 = 0;
      foreach (LoanActivityMetric activityMetric in this.LoanMetrics.ActivityMetrics)
      {
        num1 += activityMetric.ExecutionTimeInMs;
        if (activityMetric.IsExtension)
          num2 += activityMetric.ExecutionTimeInMs;
        else
          num3 += activityMetric.ExecutionTimeInMs;
        if (activityMetric.Name == "CalcOnDemand" || activityMetric.Name == "CalcAll")
          num4 += activityMetric.ExecutionTimeInMs;
        else if (activityMetric.Name == "LoanCommit")
          num5 = activityMetric.ExecutionTimeInMs;
      }
      Func<ChartPoint, string> func = (Func<ChartPoint, string>) (chartPoint => string.Format("{0}ms", (object) chartPoint.Y));
      SeriesCollection series1 = this.pcOverview.Series;
      PieSeries pieSeries1 = new PieSeries();
      pieSeries1.Title = "Business Rules";
      ChartValues<double> chartValues1 = new ChartValues<double>();
      chartValues1.Add((double) num4);
      pieSeries1.Values = (IChartValues) chartValues1;
      pieSeries1.DataLabels = true;
      pieSeries1.Fill = this.ElliBrush;
      pieSeries1.LabelPoint = func;
      series1.Add((ISeriesView) pieSeries1);
      SeriesCollection series2 = this.pcOverview.Series;
      PieSeries pieSeries2 = new PieSeries();
      pieSeries2.Title = "Plugins";
      ChartValues<double> chartValues2 = new ChartValues<double>();
      chartValues2.Add((double) num2);
      pieSeries2.Values = (IChartValues) chartValues2;
      pieSeries2.DataLabels = true;
      pieSeries2.Fill = this.PluginBrush;
      pieSeries2.LabelPoint = func;
      series2.Add((ISeriesView) pieSeries2);
      SeriesCollection series3 = this.pcOverview.Series;
      PieSeries pieSeries3 = new PieSeries();
      pieSeries3.Title = "Loan Commit";
      ChartValues<double> chartValues3 = new ChartValues<double>();
      chartValues3.Add((double) num5);
      pieSeries3.Values = (IChartValues) chartValues3;
      pieSeries3.DataLabels = true;
      pieSeries3.Fill = this.ElliBrush;
      pieSeries3.LabelPoint = func;
      series3.Add((ISeriesView) pieSeries3);
      this.lblOvTTime.Text = num1 < 10000 ? num1.ToString() + "ms" : (num1 / 1000).ToString() + "s";
      this.lvPlugins.Columns.AddRange(new ColumnHeader[3]
      {
        new ColumnHeader() { Text = "Name", Width = 410 },
        new ColumnHeader() { Text = "Time (ms)", Width = 100 },
        new ColumnHeader() { Text = "Cost (%)", Width = 100 }
      });
      foreach (LoanActivityMetric loanActivityMetric in this.LoanMetrics.ActivityMetrics.Where<LoanActivityMetric>((Func<LoanActivityMetric, bool>) (p => p.IsExtension)))
      {
        ListViewGroup group = this.lvPlugins.Groups[loanActivityMetric.Plugin.FullName];
        if (group == null)
        {
          group = new ListViewGroup(loanActivityMetric.Plugin.FullName, loanActivityMetric.Plugin.Name);
          this.lvPlugins.Groups.Add(group);
        }
        this.lvPlugins.Items.Add(new ListViewItem(new string[3]
        {
          loanActivityMetric.ActivityName.ToString(),
          loanActivityMetric.ExecutionTimeInMs.ToString() + "ms",
          ((double) loanActivityMetric.ExecutionTimeInMs / (double) num1 * 100.0).ToString("0.##") + "%"
        }, group));
      }
      this.ctBreakdown.Series = new SeriesCollection();
      CartesianMapper<double> configuration = new CartesianMapper<double>().X((Func<double, int, double>) ((value, index) => (double) (index + 1))).Y((Func<double, int, double>) ((value, index) => value)).Fill((Func<double, int, object>) ((value, index) => index >= 3 || index <= 0 ? (object) this.ElliBrush : (object) this.PluginBrush));
      SeriesCollection series4 = this.ctBreakdown.Series;
      ColumnSeries columnSeries = new ColumnSeries((object) configuration);
      columnSeries.Title = "Time (ms): ";
      ChartValues<double> chartValues4 = new ChartValues<double>();
      chartValues4.Add((double) num4);
      chartValues4.Add(14.0);
      chartValues4.Add((double) num2);
      chartValues4.Add((double) num5);
      columnSeries.Values = (IChartValues) chartValues4;
      columnSeries.Fill = this.ElliBrush;
      series4.Add((ISeriesView) columnSeries);
      AxesCollection axisX = this.ctBreakdown.AxisX;
      Axis axis1 = new Axis();
      axis1.Labels = (IList<string>) new string[5]
      {
        "Calc",
        "Calc",
        "Custom Calc",
        "Plugins",
        "Commit"
      };
      axis1.Title = "Breakdown";
      Axis axis2 = axis1;
      Separator separator = new Separator();
      separator.Step = 1.0;
      separator.IsEnabled = false;
      axis2.Separator = separator;
      Axis axis3 = axis1;
      axisX.Add(axis3);
    }

    private void tpMetrics_Click(object sender, EventArgs e)
    {
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (LoanMetricsDialog));
      this.tabControl = new TabControl();
      this.tpMetrics = new TabPage();
      this.lvPlugins = new ListView();
      this.ctBreakdown = new LiveCharts.WinForms.CartesianChart();
      this.lblOvTTime = new Label();
      this.pcOverview = new LiveCharts.WinForms.PieChart();
      this.tpConsole = new TabPage();
      this.txtConsole = new TextBox();
      this.cartesianChart1 = new LiveCharts.WinForms.CartesianChart();
      this.tabControl.SuspendLayout();
      this.tpMetrics.SuspendLayout();
      this.tpConsole.SuspendLayout();
      this.SuspendLayout();
      this.tabControl.Controls.Add((Control) this.tpMetrics);
      this.tabControl.Controls.Add((Control) this.tpConsole);
      this.tabControl.Dock = DockStyle.Fill;
      this.tabControl.Location = new Point(0, 0);
      this.tabControl.Name = "tabControl";
      this.tabControl.SelectedIndex = 0;
      this.tabControl.Size = new Size(975, 820);
      this.tabControl.TabIndex = 0;
      this.tpMetrics.Controls.Add((Control) this.lvPlugins);
      this.tpMetrics.Controls.Add((Control) this.ctBreakdown);
      this.tpMetrics.Controls.Add((Control) this.lblOvTTime);
      this.tpMetrics.Controls.Add((Control) this.pcOverview);
      this.tpMetrics.Location = new Point(4, 29);
      this.tpMetrics.Name = "tpMetrics";
      this.tpMetrics.Padding = new Padding(3, 3, 3, 3);
      this.tpMetrics.Size = new Size(967, 787);
      this.tpMetrics.TabIndex = 0;
      this.tpMetrics.Text = "Metrics";
      this.tpMetrics.UseVisualStyleBackColor = true;
      this.tpMetrics.Click += new EventHandler(this.tpMetrics_Click);
      this.lvPlugins.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      this.lvPlugins.FullRowSelect = true;
      this.lvPlugins.HideSelection = false;
      this.lvPlugins.Location = new Point(9, 440);
      this.lvPlugins.Name = "lvPlugins";
      this.lvPlugins.Size = new Size(950, 340);
      this.lvPlugins.TabIndex = 3;
      this.lvPlugins.UseCompatibleStateImageBehavior = false;
      this.lvPlugins.View = View.Details;
      this.ctBreakdown.Location = new Point(489, 18);
      this.ctBreakdown.Name = "ctBreakdown";
      this.ctBreakdown.Size = new Size(448, 392);
      this.ctBreakdown.TabIndex = 2;
      this.lblOvTTime.Font = new Font("Microsoft Sans Serif", 16.2f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblOvTTime.Location = new Point(110, 145);
      this.lblOvTTime.Name = "lblOvTTime";
      this.lblOvTTime.Size = new Size(112, 40);
      this.lblOvTTime.TabIndex = 1;
      this.lblOvTTime.Text = "0ms";
      this.lblOvTTime.TextAlign = ContentAlignment.MiddleCenter;
      this.pcOverview.Location = new Point(9, 8);
      this.pcOverview.Name = "pcOverview";
      this.pcOverview.Size = new Size(474, 315);
      this.pcOverview.TabIndex = 0;
      this.pcOverview.Text = "Time Spent";
      this.tpConsole.Controls.Add((Control) this.txtConsole);
      this.tpConsole.Location = new Point(4, 29);
      this.tpConsole.Name = "tpConsole";
      this.tpConsole.Padding = new Padding(3, 3, 3, 3);
      this.tpConsole.Size = new Size(967, 787);
      this.tpConsole.TabIndex = 1;
      this.tpConsole.Text = "Console";
      this.tpConsole.UseVisualStyleBackColor = true;
      this.txtConsole.Dock = DockStyle.Fill;
      this.txtConsole.Font = new Font("Courier New", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtConsole.Location = new Point(3, 3);
      this.txtConsole.MaxLength = 4096000;
      this.txtConsole.Multiline = true;
      this.txtConsole.Name = "txtConsole";
      this.txtConsole.ReadOnly = true;
      this.txtConsole.ScrollBars = ScrollBars.Both;
      this.txtConsole.Size = new Size(961, 781);
      this.txtConsole.TabIndex = 0;
      this.txtConsole.WordWrap = false;
      this.cartesianChart1.Location = new Point(30, 325);
      this.cartesianChart1.Name = "cartesianChart1";
      this.cartesianChart1.Size = new Size(799, 405);
      this.cartesianChart1.TabIndex = 2;
      this.cartesianChart1.Text = "cartesianChart1";
      this.AutoScaleDimensions = new SizeF(9f, 20f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(975, 820);
      this.Controls.Add((Control) this.tabControl);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LoanMetricsDialog);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Loan Metrics - Save Loan";
      this.Load += new EventHandler(this.LoanInsightsDialog_Load);
      this.tabControl.ResumeLayout(false);
      this.tpMetrics.ResumeLayout(false);
      this.tpConsole.ResumeLayout(false);
      this.tpConsole.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
