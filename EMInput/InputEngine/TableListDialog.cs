// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.TableListDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class TableListDialog : Form
  {
    private Button cancelBtn;
    private Button okBtn;
    private System.ComponentModel.Container components;
    private TableFeeListBase tableList;
    private string tableType = string.Empty;
    private string titleType = string.Empty;
    private GroupContainer groupContainer1;
    private GridView gridTables;
    private Sessions.Session session = Session.DefaultInstance;

    public TableListDialog(string tableType, string titleType, Sessions.Session session)
    {
      this.tableType = tableType;
      this.titleType = titleType;
      this.session = session;
      this.InitializeComponent();
      if (this.titleType == "Owner")
        this.Text = "Owner's Title Fee";
      else if (this.titleType == "Lender")
        this.Text = "Lender's Title Fee";
      else
        this.Text = "Select Escrow Fee";
      this.groupContainer1.Text = this.tableType;
      try
      {
        switch (tableType)
        {
          case "Escrow Fee (Purchase)":
            this.tableList = (TableFeeListBase) this.session.GetSystemSettings(typeof (TblEscrowPurList));
            break;
          case "Escrow Fee (Refinance)":
            this.tableList = (TableFeeListBase) this.session.GetSystemSettings(typeof (TblEscrowRefiList));
            break;
          case "Title Fee (Purchase)":
            this.tableList = (TableFeeListBase) this.session.GetSystemSettings(typeof (TblTitlePurList));
            break;
          default:
            this.tableList = (TableFeeListBase) this.session.GetSystemSettings(typeof (TblTitleRefiList));
            break;
        }
        this.refreshList();
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      this.gridTables_SelectedIndexChanged((object) null, (EventArgs) null);
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
      this.cancelBtn = new Button();
      this.okBtn = new Button();
      this.gridTables = new GridView();
      this.groupContainer1 = new GroupContainer();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(349, 330);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 5;
      this.cancelBtn.Text = "&Cancel";
      this.okBtn.Location = new Point(268, 330);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 24);
      this.okBtn.TabIndex = 4;
      this.okBtn.Text = "&OK";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.gridTables.BorderStyle = BorderStyle.None;
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "Column1";
      gvColumn.SpringToFit = true;
      gvColumn.Text = "Table Name";
      gvColumn.Width = 410;
      this.gridTables.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.gridTables.Dock = DockStyle.Fill;
      this.gridTables.Location = new Point(1, 26);
      this.gridTables.Name = "gridTables";
      this.gridTables.Size = new Size(410, 285);
      this.gridTables.TabIndex = 7;
      this.gridTables.SelectedIndexChanged += new EventHandler(this.gridTables_SelectedIndexChanged);
      this.gridTables.DoubleClick += new EventHandler(this.listView_DoubleClick);
      this.groupContainer1.Controls.Add((Control) this.gridTables);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(12, 12);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(412, 312);
      this.groupContainer1.TabIndex = 8;
      this.groupContainer1.Text = "Escrow Fee (Purchase)";
      this.AcceptButton = (IButtonControl) this.okBtn;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(436, 366);
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.okBtn);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (TableListDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Table list :";
      this.KeyUp += new KeyEventHandler(this.TableListDialog_KeyUp);
      this.groupContainer1.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void refreshList()
    {
      this.gridTables.Items.Clear();
      if (this.tableList == null)
        return;
      this.gridTables.BeginUpdate();
      for (int i = 0; i < this.tableList.Count; ++i)
      {
        TableFeeListBase.FeeTable tableAt = this.tableList.GetTableAt(i);
        if (!this.tableType.StartsWith("Title") || !(this.titleType != tableAt.FeeType))
          this.gridTables.Items.Add(new GVItem()
          {
            SubItems = {
              (object) tableAt.TableName
            },
            Tag = (object) tableAt
          });
      }
      this.gridTables.EndUpdate();
    }

    private void okBtn_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    internal TableFeeListBase.FeeTable SelectedFeeTable
    {
      get => (TableFeeListBase.FeeTable) this.gridTables.SelectedItems[0].Tag;
    }

    internal string SelectedTableName
    {
      get => ((TableFeeListBase.FeeTable) this.gridTables.SelectedItems[0].Tag).TableName;
    }

    private void listView_DoubleClick(object sender, EventArgs e)
    {
      this.okBtn_Click((object) null, (EventArgs) null);
    }

    private void TableListDialog_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, "Escrow Fee (Purchase)");
    }

    private void gridTables_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.okBtn.Enabled = this.gridTables.SelectedItems.Count > 0;
    }
  }
}
