// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.TaskLabel
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.Properties;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class TaskLabel : UserControl
  {
    private MilestoneTaskLog taskLog;
    private ListViewItem viewItem;
    private RequiredTasksControl reqControl;
    private bool editable = true;
    private IContainer components;
    private Label labelStatus;
    private Label labelTaskName;
    private PictureBox picIsRequired;
    private ToolTip toolTip1;

    public TaskLabel(
      MilestoneTaskLog taskLog,
      ListViewItem viewItem,
      RequiredTasksControl reqControl,
      bool editable)
    {
      this.taskLog = taskLog;
      this.viewItem = viewItem;
      this.reqControl = reqControl;
      this.editable = editable;
      this.InitializeComponent();
      this.RefreshTaskStatus();
    }

    public void RefreshTaskStatus(MilestoneTaskLog taskLog)
    {
      this.taskLog = taskLog;
      this.RefreshTaskStatus();
    }

    public void RefreshTaskStatus()
    {
      this.picIsRequired.Visible = this.taskLog.IsRequired;
      this.labelTaskName.Text = this.taskLog.TaskName;
      if (this.taskLog.CompletedDate != DateTime.MinValue)
        this.labelStatus.Text = " completed on " + this.taskLog.CompletedDate.ToString("MM/dd/yyyy");
      else
        this.labelStatus.Text = string.Empty;
      this.labelStatus.Left = this.labelTaskName.Left + this.labelTaskName.Width;
    }

    private void labelTaskName_Click(object sender, EventArgs e)
    {
      if (this.taskLog == null)
        return;
      using (MilestoneTaskWorksheetContainer worksheetContainer = new MilestoneTaskWorksheetContainer(this.taskLog, this.editable))
      {
        if (worksheetContainer.ShowDialog() != DialogResult.OK)
          return;
        this.viewItem.Checked = this.taskLog.CompletedDate != DateTime.MinValue;
        Session.Application.GetService<ILoanEditor>().RefreshLogPanel();
        this.RefreshTaskStatus();
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
      this.components = (IContainer) new System.ComponentModel.Container();
      this.labelStatus = new Label();
      this.labelTaskName = new Label();
      this.picIsRequired = new PictureBox();
      this.toolTip1 = new ToolTip(this.components);
      ((ISupportInitialize) this.picIsRequired).BeginInit();
      this.SuspendLayout();
      this.labelStatus.AutoSize = true;
      this.labelStatus.Location = new Point(86, 1);
      this.labelStatus.Name = "labelStatus";
      this.labelStatus.Size = new Size(70, 13);
      this.labelStatus.TabIndex = 3;
      this.labelStatus.Text = "(Task Status)";
      this.labelTaskName.AutoSize = true;
      this.labelTaskName.Cursor = Cursors.Hand;
      this.labelTaskName.ForeColor = SystemColors.HotTrack;
      this.labelTaskName.Location = new Point(12, 1);
      this.labelTaskName.Name = "labelTaskName";
      this.labelTaskName.Size = new Size(68, 13);
      this.labelTaskName.TabIndex = 2;
      this.labelTaskName.Text = "(Task Name)";
      this.labelTaskName.Click += new EventHandler(this.labelTaskName_Click);
      this.picIsRequired.Image = (Image) Resources.required;
      this.picIsRequired.Location = new Point(0, 0);
      this.picIsRequired.Name = "picIsRequired";
      this.picIsRequired.Size = new Size(16, 16);
      this.picIsRequired.SizeMode = PictureBoxSizeMode.AutoSize;
      this.picIsRequired.TabIndex = 5;
      this.picIsRequired.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.picIsRequired, "Task is required");
      this.picIsRequired.Visible = false;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.Transparent;
      this.Controls.Add((Control) this.picIsRequired);
      this.Controls.Add((Control) this.labelStatus);
      this.Controls.Add((Control) this.labelTaskName);
      this.Name = nameof (TaskLabel);
      this.Size = new Size(330, 14);
      ((ISupportInitialize) this.picIsRequired).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
