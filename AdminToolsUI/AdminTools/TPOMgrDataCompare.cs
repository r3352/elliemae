// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.TPOMgrDataCompare
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.UI;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools
{
  public class TPOMgrDataCompare : UserControl
  {
    private DataTable oriDT;
    private DataTable newDT;
    private string tableName;
    private IContainer components;
    private Panel panel1;
    private Splitter splitter1;
    private GroupContainer groupContainer2;
    private GroupContainer groupContainer1;
    private GridView gvUpdated;
    private GridView gvOri;

    public TPOMgrDataCompare(DataSet oriDS, DataSet newDS, string tableName)
    {
      this.InitializeComponent();
      this.tableName = tableName;
      if (oriDS != null && oriDS.Tables.Contains(this.tableName))
        this.oriDT = oriDS.Tables[this.tableName];
      if (newDS != null && newDS.Tables.Contains(this.tableName))
        this.newDT = newDS.Tables[this.tableName];
      this.initialPage();
    }

    public void initialPage()
    {
      if (this.oriDT != null)
      {
        this.gvOri.Columns.Add("#", 20);
        foreach (DataColumn column in (InternalDataCollectionBase) this.oriDT.Columns)
          this.gvOri.Columns.Add(column.ColumnName, 80);
        for (int index = 0; index < this.oriDT.Rows.Count; ++index)
        {
          DataRow row = this.oriDT.Rows[index];
          GVItem gvItem = new GVItem((object) index);
          for (int columnIndex = 0; columnIndex < this.oriDT.Columns.Count; ++columnIndex)
            gvItem.SubItems.Add(row[columnIndex]);
          this.gvOri.Items.Add(gvItem);
        }
      }
      if (this.newDT == null)
        return;
      this.gvUpdated.Columns.Add("#", 20);
      foreach (DataColumn column in (InternalDataCollectionBase) this.newDT.Columns)
        this.gvUpdated.Columns.Add(column.ColumnName, 80);
      for (int index = 0; index < this.newDT.Rows.Count; ++index)
      {
        DataRow row = this.newDT.Rows[index];
        GVItem gvItem = new GVItem((object) index);
        for (int columnIndex = 0; columnIndex < this.newDT.Columns.Count; ++columnIndex)
          gvItem.SubItems.Add(row[columnIndex]);
        this.gvUpdated.Items.Add(gvItem);
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
      this.panel1 = new Panel();
      this.groupContainer1 = new GroupContainer();
      this.groupContainer2 = new GroupContainer();
      this.splitter1 = new Splitter();
      this.gvOri = new GridView();
      this.gvUpdated = new GridView();
      this.panel1.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.groupContainer2.SuspendLayout();
      this.SuspendLayout();
      this.panel1.Controls.Add((Control) this.splitter1);
      this.panel1.Controls.Add((Control) this.groupContainer2);
      this.panel1.Controls.Add((Control) this.groupContainer1);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(781, 448);
      this.panel1.TabIndex = 0;
      this.groupContainer1.Controls.Add((Control) this.gvOri);
      this.groupContainer1.Dock = DockStyle.Top;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(781, 216);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "Original Data";
      this.groupContainer2.Controls.Add((Control) this.gvUpdated);
      this.groupContainer2.Dock = DockStyle.Fill;
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(0, 216);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(781, 232);
      this.groupContainer2.TabIndex = 1;
      this.groupContainer2.Text = "Updated Data";
      this.splitter1.Dock = DockStyle.Top;
      this.splitter1.Location = new Point(0, 216);
      this.splitter1.Name = "splitter1";
      this.splitter1.Size = new Size(781, 3);
      this.splitter1.TabIndex = 2;
      this.splitter1.TabStop = false;
      this.gvOri.Dock = DockStyle.Fill;
      this.gvOri.Location = new Point(1, 26);
      this.gvOri.Name = "gvOri";
      this.gvOri.Size = new Size(779, 189);
      this.gvOri.TabIndex = 0;
      this.gvUpdated.Dock = DockStyle.Fill;
      this.gvUpdated.Location = new Point(1, 26);
      this.gvUpdated.Name = "gvUpdated";
      this.gvUpdated.Size = new Size(779, 205);
      this.gvUpdated.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.panel1);
      this.Name = nameof (TPOMgrDataCompare);
      this.Size = new Size(781, 448);
      this.panel1.ResumeLayout(false);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer2.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
