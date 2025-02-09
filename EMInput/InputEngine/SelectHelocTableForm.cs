// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.SelectHelocTableForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class SelectHelocTableForm : Form
  {
    private ColumnHeader nameHeader;
    private Button cancelBtn;
    private Button okBtn;
    private ListView listViewHeloc;
    private System.ComponentModel.Container components;
    private string tableName = "";

    public SelectHelocTableForm(bool showOldAndNewHelocTables)
    {
      this.InitializeComponent();
      this.listViewHeloc.Items.Clear();
      FileSystemEntry[] helocTableDirEntries = Session.ConfigurationManager.GetHelocTableDirEntries();
      FileSystemEntry[] fileSystemEntryArray = (FileSystemEntry[]) null;
      if (showOldAndNewHelocTables)
        fileSystemEntryArray = Session.ConfigurationManager.GetHelocTableDirEntries(true);
      if (helocTableDirEntries != null)
      {
        for (int index = 0; index < helocTableDirEntries.Length; ++index)
          this.listViewHeloc.Items.Add(new ListViewItem(FileSystem.DecodeFilename(helocTableDirEntries[index].Name)));
      }
      if (!showOldAndNewHelocTables || fileSystemEntryArray == null)
        return;
      for (int index = 0; index < fileSystemEntryArray.Length; ++index)
        this.listViewHeloc.Items.Add(new ListViewItem(FileSystem.DecodeFilename(fileSystemEntryArray[index].Name)));
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public string TableName => this.tableName;

    private void InitializeComponent()
    {
      this.listViewHeloc = new ListView();
      this.nameHeader = new ColumnHeader();
      this.cancelBtn = new Button();
      this.okBtn = new Button();
      this.SuspendLayout();
      this.listViewHeloc.AllowColumnReorder = true;
      this.listViewHeloc.AllowDrop = true;
      this.listViewHeloc.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.listViewHeloc.Columns.AddRange(new ColumnHeader[1]
      {
        this.nameHeader
      });
      this.listViewHeloc.FullRowSelect = true;
      this.listViewHeloc.GridLines = true;
      this.listViewHeloc.HideSelection = false;
      this.listViewHeloc.Location = new Point(12, 12);
      this.listViewHeloc.MultiSelect = false;
      this.listViewHeloc.Name = "listViewHeloc";
      this.listViewHeloc.Size = new Size(420, 420);
      this.listViewHeloc.TabIndex = 2;
      this.listViewHeloc.View = View.Details;
      this.listViewHeloc.DoubleClick += new EventHandler(this.listViewHeloc_DoubleClick);
      this.nameHeader.Text = "Table Name";
      this.nameHeader.Width = 399;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.cancelBtn.Location = new Point(444, 44);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 8;
      this.cancelBtn.Text = "&Cancel";
      this.okBtn.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.okBtn.Location = new Point(444, 12);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 24);
      this.okBtn.TabIndex = 7;
      this.okBtn.Text = "&Select";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(532, 446);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.okBtn);
      this.Controls.Add((Control) this.listViewHeloc);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SelectHelocTableForm);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Select Heloc Table";
      this.ResumeLayout(false);
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (this.listViewHeloc.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a table first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.tableName = this.listViewHeloc.SelectedItems[0].Text;
        this.DialogResult = DialogResult.OK;
      }
    }

    private void listViewHeloc_DoubleClick(object sender, EventArgs e)
    {
      this.okBtn_Click((object) null, (EventArgs) null);
    }
  }
}
