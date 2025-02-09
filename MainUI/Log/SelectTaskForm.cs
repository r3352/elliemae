// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.SelectTaskForm
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class SelectTaskForm : Form
  {
    private IContainer components;
    private DialogButtons dialogButtons1;
    private GridView lvwTasks;

    public SelectTaskForm(bool isSingleSelection)
    {
      this.InitializeComponent();
      this.initForm();
      this.lvwTasks.AllowMultiselect = !isSingleSelection;
    }

    private void initForm()
    {
      this.lvwTasks.Items.Clear();
      MilestoneTaskDefinition[] milestoneTasks = Session.ConfigurationManager.GetMilestoneTasks((string[]) null);
      if (milestoneTasks == null || milestoneTasks.Length == 0)
        return;
      Cursor.Current = Cursors.WaitCursor;
      this.lvwTasks.BeginUpdate();
      for (int index = 0; index < milestoneTasks.Length; ++index)
        this.lvwTasks.Items.Add(this.buildListViewItem(milestoneTasks[index], false));
      this.lvwTasks.EndUpdate();
      Cursor.Current = Cursors.Default;
    }

    private GVItem buildListViewItem(MilestoneTaskDefinition task, bool selected)
    {
      return new GVItem(task.TaskName)
      {
        SubItems = {
          (object) task.TaskDescription
        },
        Tag = (object) task,
        Selected = selected
      };
    }

    private void dialogButtons1_OK(object sender, EventArgs e)
    {
      if (this.lvwTasks.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a task.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
        this.DialogResult = DialogResult.OK;
    }

    public MilestoneTaskDefinition SelectedTask
    {
      get => (MilestoneTaskDefinition) this.lvwTasks.SelectedItems[0].Tag;
    }

    private void lvwTasks_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.dialogButtons1_OK((object) null, (EventArgs) null);
    }

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
      this.dialogButtons1 = new DialogButtons();
      this.lvwTasks = new GridView();
      this.SuspendLayout();
      this.dialogButtons1.Dock = DockStyle.Bottom;
      this.dialogButtons1.Location = new Point(0, 337);
      this.dialogButtons1.Name = "dialogButtons1";
      this.dialogButtons1.OKText = "&Select";
      this.dialogButtons1.Size = new Size(680, 44);
      this.dialogButtons1.TabIndex = 11;
      this.dialogButtons1.OK += new EventHandler(this.dialogButtons1_OK);
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Name";
      gvColumn1.Width = 238;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "Description";
      gvColumn2.Width = 424;
      this.lvwTasks.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.lvwTasks.Location = new Point(7, 12);
      this.lvwTasks.Name = "lvwTasks";
      this.lvwTasks.Size = new Size(664, 319);
      this.lvwTasks.TabIndex = 13;
      this.lvwTasks.ItemDoubleClick += new GVItemEventHandler(this.lvwTasks_ItemDoubleClick);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(680, 381);
      this.Controls.Add((Control) this.lvwTasks);
      this.Controls.Add((Control) this.dialogButtons1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SelectTaskForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Select Task";
      this.ResumeLayout(false);
    }
  }
}
