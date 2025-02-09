// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.ARMTypeDetails
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class ARMTypeDetails : Form
  {
    private System.ComponentModel.Container components;
    private Button cancelBtn;
    private Button okBtn;
    private ListView armListView;
    private ColumnHeader columnHeader1;
    private ColumnHeader columnHeader2;
    private ListViewSortManager sortMngr;
    private string armTypeID;

    public ARMTypeDetails(string fieldID, string fieldValue)
    {
      this.InitializeComponent();
      this.sortMngr = new ListViewSortManager(this.armListView, new System.Type[2]
      {
        typeof (ListViewTextSort),
        typeof (ListViewTextSort)
      });
      this.sortMngr.Sort(0);
      this.armListView.Items.Clear();
      if (!(fieldID == "995") && !(fieldID == "LP86"))
        return;
      this.Initialize995(fieldValue);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.cancelBtn = new Button();
      this.okBtn = new Button();
      this.armListView = new ListView();
      this.columnHeader1 = new ColumnHeader();
      this.columnHeader2 = new ColumnHeader();
      this.SuspendLayout();
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(484, 40);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 9;
      this.cancelBtn.Text = "&Cancel";
      this.okBtn.Location = new Point(484, 8);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 24);
      this.okBtn.TabIndex = 8;
      this.okBtn.Text = "&Select";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.armListView.Columns.AddRange(new ColumnHeader[2]
      {
        this.columnHeader1,
        this.columnHeader2
      });
      this.armListView.FullRowSelect = true;
      this.armListView.GridLines = true;
      this.armListView.HideSelection = false;
      this.armListView.Location = new Point(8, 8);
      this.armListView.MultiSelect = false;
      this.armListView.Name = "armListView";
      this.armListView.Size = new Size(464, 276);
      this.armListView.TabIndex = 7;
      this.armListView.UseCompatibleStateImageBehavior = false;
      this.armListView.View = View.Details;
      this.armListView.DoubleClick += new EventHandler(this.okBtn_Click);
      this.columnHeader1.Text = "ARM Type";
      this.columnHeader1.Width = 360;
      this.columnHeader2.Text = "Type ID";
      this.columnHeader2.TextAlign = HorizontalAlignment.Right;
      this.columnHeader2.Width = 85;
      this.AcceptButton = (IButtonControl) this.okBtn;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(566, 295);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.okBtn);
      this.Controls.Add((Control) this.armListView);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ARMTypeDetails);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "ARM Type";
      this.KeyPress += new KeyPressEventHandler(this.ARMTypeDetails_KeyPress);
      this.ResumeLayout(false);
    }

    internal string ArmTypeID => this.armTypeID;

    private void Initialize995(string fieldValue)
    {
      this.armListView.Items.Clear();
      int index = -1;
      foreach (ARMType armType in (IEnumerable<ARMType>) ARMTypeList.ARMTypes)
      {
        ListViewItem listViewItem = new ListViewItem(armType.Description);
        listViewItem.SubItems.Add(armType.TypeID);
        if (armType.TypeID == fieldValue)
        {
          listViewItem.Selected = true;
          index = this.armListView.Items.Count;
        }
        this.armListView.Items.Add(listViewItem);
      }
      if (index == -1)
        return;
      this.armListView.Items[index].EnsureVisible();
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (this.armListView.SelectedItems.Count > 0)
      {
        this.armTypeID = this.armListView.SelectedItems[0].SubItems[1].Text;
        this.DialogResult = DialogResult.OK;
      }
      else
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must select an ARM type.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void ARMTypeDetails_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\u001B')
        return;
      this.Close();
    }
  }
}
