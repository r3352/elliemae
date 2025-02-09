// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.MilestoneTaskListPanel
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class MilestoneTaskListPanel : UserControl
  {
    private Sessions.Session session;
    private bool isReadOnly;
    private IContainer components;
    private GridView lvwTasks;
    private GroupContainer gcTasks;
    private StandardIconButton stdIconBtnNew;
    private StandardIconButton stdIconBtnEdit;
    private StandardIconButton stdIconBtnDelete;
    private ToolTip toolTip1;

    public MilestoneTaskListPanel(Sessions.Session session, bool isReadOnly)
    {
      this.isReadOnly = isReadOnly;
      this.InitializeComponent();
      this.session = session;
      if (this.isReadOnly)
      {
        this.stdIconBtnNew.Enabled = false;
        this.stdIconBtnEdit.Enabled = false;
        this.stdIconBtnDelete.Enabled = false;
        this.lvwTasks.AllowMultiselect = true;
      }
      this.initForm();
      this.lvwTasks_SelectedIndexChanged((object) this, (EventArgs) null);
    }

    private void initForm()
    {
      this.lvwTasks.Items.Clear();
      MilestoneTaskDefinition[] milestoneTasks = this.session.ConfigurationManager.GetMilestoneTasks((string[]) null);
      if (milestoneTasks == null || milestoneTasks.Length == 0)
        return;
      Cursor.Current = Cursors.WaitCursor;
      this.lvwTasks.BeginUpdate();
      for (int index = 0; index < milestoneTasks.Length; ++index)
        this.lvwTasks.Items.Add(this.buildGVItem(milestoneTasks[index], false));
      this.lvwTasks.EndUpdate();
      this.setTitle();
      Cursor.Current = Cursors.Default;
    }

    private void setTitle()
    {
      this.gcTasks.Text = "Tasks (" + (object) this.lvwTasks.Items.Count + ")";
    }

    private GVItem buildGVItem(MilestoneTaskDefinition task, bool selected)
    {
      return new GVItem(task.TaskName)
      {
        SubItems = {
          (object) task.TaskDescription,
          task.DaysToComplete > 0 ? (object) task.DaysToComplete.ToString() : (object) "",
          (object) task.TaskPriority.ToString()
        },
        Tag = (object) task,
        Selected = selected
      };
    }

    public string[] SelectedTaskList
    {
      get
      {
        List<string> stringList = new List<string>();
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.lvwTasks.Items)
        {
          if (gvItem.Selected)
            stringList.Add(((MilestoneTaskDefinition) gvItem.Tag).TaskName);
        }
        return stringList.ToArray();
      }
      set
      {
        foreach (string str in value)
        {
          foreach (GVItem gvItem in (IEnumerable<GVItem>) this.lvwTasks.Items)
            gvItem.Selected = ((MilestoneTaskDefinition) gvItem.Tag).TaskName == str;
        }
      }
    }

    private void newBtn_Click(object sender, EventArgs e)
    {
      this.lvwTasks.SelectedItems.Clear();
      using (MilestoneTaskDetailSetupForm taskDetailSetupForm = new MilestoneTaskDetailSetupForm(this.session, (MilestoneTaskDefinition) null))
      {
        if (taskDetailSetupForm.ShowDialog((IWin32Window) this) == DialogResult.OK)
        {
          try
          {
            MilestoneTaskDefinition task = taskDetailSetupForm.Task;
            task.TaskGUID = this.session.ConfigurationManager.AddMilestoneTask(task);
            this.lvwTasks.Items.Add(this.buildGVItem(task, true));
          }
          catch (Exception ex)
          {
          }
        }
      }
      this.setTitle();
    }

    private void deleteBtn_Click(object sender, EventArgs e)
    {
      if (this.lvwTasks.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a task.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "Are you sure you want to delete selected " + (this.lvwTasks.SelectedItems.Count > 1 ? "tasks" : "task") + "?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
          return;
        int index = this.lvwTasks.SelectedItems[0].Index;
        int num2 = this.lvwTasks.Items.Count - 1;
        this.lvwTasks.BeginUpdate();
        List<string> stringList = new List<string>();
        for (int nItemIndex = num2; nItemIndex >= 0; --nItemIndex)
        {
          if (this.lvwTasks.Items[nItemIndex].Selected)
          {
            MilestoneTaskDefinition tag = (MilestoneTaskDefinition) this.lvwTasks.Items[nItemIndex].Tag;
            stringList.Add(tag.TaskGUID);
            this.lvwTasks.Items.Remove(this.lvwTasks.Items[nItemIndex]);
          }
        }
        this.session.ConfigurationManager.DeleteMilestoneTasks(stringList.ToArray());
        this.lvwTasks.EndUpdate();
        this.setTitle();
        if (this.lvwTasks.Items.Count == 0)
          return;
        if (index + 1 >= this.lvwTasks.Items.Count)
          this.lvwTasks.Items[this.lvwTasks.Items.Count - 1].Selected = true;
        else
          this.lvwTasks.Items[index].Selected = true;
      }
    }

    private void lvwTasks_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.editSelectedItem();
    }

    private void editBtn_Click(object sender, EventArgs e) => this.editSelectedItem();

    private void editSelectedItem()
    {
      if (this.isReadOnly)
        return;
      if (this.lvwTasks.SelectedItems.Count != 1)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a task.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        MilestoneTaskDefinition tag = (MilestoneTaskDefinition) this.lvwTasks.SelectedItems[0].Tag;
        using (MilestoneTaskDetailSetupForm taskDetailSetupForm = new MilestoneTaskDetailSetupForm(this.session, tag))
        {
          if (taskDetailSetupForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          try
          {
            this.session.ConfigurationManager.UpdateMilestoneTask(tag);
            this.lvwTasks.SelectedItems[0].Text = tag.TaskName;
            this.lvwTasks.SelectedItems[0].SubItems[1].Text = tag.TaskDescription;
            this.lvwTasks.SelectedItems[0].SubItems[2].Text = tag.DaysToComplete > 0 ? string.Concat((object) tag.DaysToComplete) : "";
            this.lvwTasks.SelectedItems[0].SubItems[3].Text = tag.TaskPriority.ToString();
          }
          catch (Exception ex)
          {
          }
        }
      }
    }

    public MilestoneTaskDefinition[] SelectedTasks
    {
      get
      {
        MilestoneTaskDefinition[] selectedTasks = new MilestoneTaskDefinition[this.lvwTasks.SelectedItems.Count];
        for (int index = 0; index < this.lvwTasks.SelectedItems.Count; ++index)
          selectedTasks[index] = (MilestoneTaskDefinition) this.lvwTasks.SelectedItems[index].Tag;
        return selectedTasks;
      }
    }

    private void lvwTasks_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.isReadOnly)
      {
        this.stdIconBtnDelete.Enabled = this.stdIconBtnEdit.Enabled = this.stdIconBtnNew.Enabled = false;
      }
      else
      {
        this.stdIconBtnDelete.Enabled = this.lvwTasks.SelectedItems.Count > 0;
        this.stdIconBtnEdit.Enabled = this.lvwTasks.SelectedItems.Count == 1;
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
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      this.lvwTasks = new GridView();
      this.gcTasks = new GroupContainer();
      this.stdIconBtnNew = new StandardIconButton();
      this.stdIconBtnEdit = new StandardIconButton();
      this.stdIconBtnDelete = new StandardIconButton();
      this.toolTip1 = new ToolTip(this.components);
      this.gcTasks.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnNew).BeginInit();
      ((ISupportInitialize) this.stdIconBtnEdit).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDelete).BeginInit();
      this.SuspendLayout();
      this.lvwTasks.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Name";
      gvColumn1.Width = 284;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Description";
      gvColumn2.Width = 200;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.SortMethod = GVSortMethod.Numeric;
      gvColumn3.Text = "Days to Complete";
      gvColumn3.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.SpringToFit = true;
      gvColumn4.Text = "Priority";
      gvColumn4.Width = 114;
      this.lvwTasks.Columns.AddRange(new GVColumn[4]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4
      });
      this.lvwTasks.Dock = DockStyle.Fill;
      this.lvwTasks.Location = new Point(1, 26);
      this.lvwTasks.Name = "lvwTasks";
      this.lvwTasks.Size = new Size(698, 415);
      this.lvwTasks.TabIndex = 9;
      this.lvwTasks.TextTrimming = StringTrimming.EllipsisWord;
      this.lvwTasks.SelectedIndexChanged += new EventHandler(this.lvwTasks_SelectedIndexChanged);
      this.lvwTasks.ItemDoubleClick += new GVItemEventHandler(this.lvwTasks_ItemDoubleClick);
      this.gcTasks.Controls.Add((Control) this.stdIconBtnNew);
      this.gcTasks.Controls.Add((Control) this.stdIconBtnEdit);
      this.gcTasks.Controls.Add((Control) this.stdIconBtnDelete);
      this.gcTasks.Controls.Add((Control) this.lvwTasks);
      this.gcTasks.Dock = DockStyle.Fill;
      this.gcTasks.Location = new Point(0, 0);
      this.gcTasks.Name = "gcTasks";
      this.gcTasks.Size = new Size(700, 442);
      this.gcTasks.TabIndex = 10;
      this.gcTasks.Text = "Tasks (0)";
      this.stdIconBtnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnNew.BackColor = Color.Transparent;
      this.stdIconBtnNew.Location = new Point(635, 5);
      this.stdIconBtnNew.Name = "stdIconBtnNew";
      this.stdIconBtnNew.Size = new Size(16, 16);
      this.stdIconBtnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnNew.TabIndex = 12;
      this.stdIconBtnNew.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnNew, "New");
      this.stdIconBtnNew.Click += new EventHandler(this.newBtn_Click);
      this.stdIconBtnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnEdit.BackColor = Color.Transparent;
      this.stdIconBtnEdit.Location = new Point(657, 5);
      this.stdIconBtnEdit.Name = "stdIconBtnEdit";
      this.stdIconBtnEdit.Size = new Size(16, 16);
      this.stdIconBtnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdIconBtnEdit.TabIndex = 11;
      this.stdIconBtnEdit.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnEdit, "Edit");
      this.stdIconBtnEdit.Click += new EventHandler(this.editBtn_Click);
      this.stdIconBtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDelete.BackColor = Color.Transparent;
      this.stdIconBtnDelete.Location = new Point(679, 5);
      this.stdIconBtnDelete.Name = "stdIconBtnDelete";
      this.stdIconBtnDelete.Size = new Size(16, 16);
      this.stdIconBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDelete.TabIndex = 10;
      this.stdIconBtnDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDelete, "Delete");
      this.stdIconBtnDelete.Click += new EventHandler(this.deleteBtn_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcTasks);
      this.Name = nameof (MilestoneTaskListPanel);
      this.Size = new Size(700, 442);
      this.gcTasks.ResumeLayout(false);
      ((ISupportInitialize) this.stdIconBtnNew).EndInit();
      ((ISupportInitialize) this.stdIconBtnEdit).EndInit();
      ((ISupportInitialize) this.stdIconBtnDelete).EndInit();
      this.ResumeLayout(false);
    }
  }
}
