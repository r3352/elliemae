// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.FieldSearchControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer.Bpm;
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
  public class FieldSearchControl : UserControl
  {
    private IContainer components;
    private GroupContainer gcRebuildFieldSearch;
    private GridView listRuleTypes;
    private Button rebuildBtn;

    public FieldSearchControl()
    {
      this.InitializeComponent();
      this.refreshRuleTypes();
    }

    private void refreshRuleTypes()
    {
      this.listRuleTypes.Items.Clear();
      foreach (SearchableRuleType searchableRuleType in Session.SessionObjects.BpmManager.GetSearchableRuleTypes())
      {
        if (searchableRuleType.Type != FieldSearchRuleType.None && (Session.SessionObjects.StartupInfo != null && Session.StartupInfo.AllowDDM || searchableRuleType.Type != FieldSearchRuleType.DDMDataTables && searchableRuleType.Type != FieldSearchRuleType.DDMFeeScenarios && searchableRuleType.Type != FieldSearchRuleType.DDMFieldScenarios))
          this.listRuleTypes.Items.Add(new GVItem(searchableRuleType.Name, (object) searchableRuleType.Type));
      }
    }

    private void rebuildBtn_Click(object sender, EventArgs e)
    {
      List<FieldSearchRuleType> fsRuleTypes = new List<FieldSearchRuleType>();
      foreach (GVItem selectedItem in this.listRuleTypes.SelectedItems)
      {
        FieldSearchRuleType result = FieldSearchRuleType.None;
        if (Enum.TryParse<FieldSearchRuleType>(selectedItem.Tag.ToString(), true, out result) && result != FieldSearchRuleType.None)
          fsRuleTypes.Add(result);
      }
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        Session.SessionObjects.BpmManager.UpdateFieldSearchInfo(fsRuleTypes, true);
        Cursor.Current = Cursors.Default;
        int num = (int) MessageBox.Show("Rebuild search data completed successfully.");
      }
      catch (Exception ex)
      {
        Cursor.Current = Cursors.Default;
        int num = (int) MessageBox.Show("Error happened while rebuilding field search data.");
      }
    }

    private void gvFolder_SizeChanged(object sender, EventArgs e)
    {
      this.listRuleTypes.Columns[0].Width = this.listRuleTypes.Width - 5;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn = new GVColumn();
      this.gcRebuildFieldSearch = new GroupContainer();
      this.listRuleTypes = new GridView();
      this.rebuildBtn = new Button();
      this.gcRebuildFieldSearch.SuspendLayout();
      this.SuspendLayout();
      this.gcRebuildFieldSearch.Controls.Add((Control) this.listRuleTypes);
      this.gcRebuildFieldSearch.Controls.Add((Control) this.rebuildBtn);
      this.gcRebuildFieldSearch.Dock = DockStyle.Fill;
      this.gcRebuildFieldSearch.HeaderForeColor = SystemColors.ControlText;
      this.gcRebuildFieldSearch.Location = new Point(0, 0);
      this.gcRebuildFieldSearch.Name = "gcRebuildFieldSearch";
      this.gcRebuildFieldSearch.Size = new Size(698, 424);
      this.gcRebuildFieldSearch.TabIndex = 7;
      this.gcRebuildFieldSearch.Text = "Searchable Business Rules";
      this.listRuleTypes.BorderStyle = BorderStyle.None;
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "colFolder";
      gvColumn.Text = "FolderName";
      gvColumn.Width = 500;
      this.listRuleTypes.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.listRuleTypes.Dock = DockStyle.Fill;
      this.listRuleTypes.HeaderHeight = 0;
      this.listRuleTypes.HeaderVisible = false;
      this.listRuleTypes.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listRuleTypes.Location = new Point(1, 26);
      this.listRuleTypes.Name = "listRuleTypes";
      this.listRuleTypes.Size = new Size(696, 397);
      this.listRuleTypes.TabIndex = 3;
      this.listRuleTypes.SizeChanged += new EventHandler(this.gvFolder_SizeChanged);
      this.rebuildBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.rebuildBtn.BackColor = SystemColors.Control;
      this.rebuildBtn.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.rebuildBtn.Location = new Point(591, 2);
      this.rebuildBtn.Name = "rebuildBtn";
      this.rebuildBtn.Size = new Size(103, 22);
      this.rebuildBtn.TabIndex = 1;
      this.rebuildBtn.Text = "Rebuild Data";
      this.rebuildBtn.UseVisualStyleBackColor = true;
      this.rebuildBtn.Click += new EventHandler(this.rebuildBtn_Click);
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.Controls.Add((Control) this.gcRebuildFieldSearch);
      this.Name = nameof (FieldSearchControl);
      this.Size = new Size(698, 424);
      this.gcRebuildFieldSearch.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
