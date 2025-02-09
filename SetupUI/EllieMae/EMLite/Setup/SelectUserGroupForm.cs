// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SelectUserGroupForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class SelectUserGroupForm : Form
  {
    private RoleDetailForm parentForm;
    private IContainer components;
    private GridView gvUserGroups;
    private Button buttonCancel;
    private Button buttonSelect;

    public SelectUserGroupForm(int[] groupIds, RoleDetailForm parent)
    {
      this.InitializeComponent();
      this.parentForm = parent;
      this.initForm(groupIds);
    }

    private void initForm(int[] groupIds)
    {
      this.gvUserGroups.Items.Clear();
      foreach (int groupId in groupIds)
      {
        if (this.parentForm.UserGroupSettings.ContainsKey((object) groupId))
          this.gvUserGroups.Items.Add(new GVItem(this.parentForm.UserGroupSettings[(object) groupId].ToString())
          {
            Tag = (object) groupId
          });
      }
      this.gvUserGroups.Sort(0, SortOrder.Ascending);
    }

    public int[] GetSelectedGroups()
    {
      List<int> intList = new List<int>();
      foreach (GVItem selectedItem in this.gvUserGroups.SelectedItems)
        intList.Add((int) selectedItem.Tag);
      return intList.ToArray();
    }

    private void buttonSelect_Click(object sender, EventArgs e)
    {
      if (this.gvUserGroups.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select one or more groups from the list provided.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
        this.DialogResult = DialogResult.OK;
    }

    private void gvUserGroups_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      if (!e.Item.Selected)
        return;
      this.DialogResult = DialogResult.OK;
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
      this.gvUserGroups = new GridView();
      this.buttonCancel = new Button();
      this.buttonSelect = new Button();
      this.SuspendLayout();
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "Column1";
      gvColumn.SpringToFit = true;
      gvColumn.Text = "User Groups";
      gvColumn.Width = 254;
      this.gvUserGroups.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.gvUserGroups.Location = new Point(10, 10);
      this.gvUserGroups.Name = "gvUserGroups";
      this.gvUserGroups.Size = new Size(256, 324);
      this.gvUserGroups.TabIndex = 32;
      this.gvUserGroups.ItemDoubleClick += new GVItemEventHandler(this.gvUserGroups_ItemDoubleClick);
      this.buttonCancel.DialogResult = DialogResult.Cancel;
      this.buttonCancel.Location = new Point(278, 41);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new Size(75, 23);
      this.buttonCancel.TabIndex = 31;
      this.buttonCancel.Text = "&Cancel";
      this.buttonSelect.Location = new Point(278, 10);
      this.buttonSelect.Name = "buttonSelect";
      this.buttonSelect.Size = new Size(75, 23);
      this.buttonSelect.TabIndex = 30;
      this.buttonSelect.Text = "&Select";
      this.buttonSelect.Click += new EventHandler(this.buttonSelect_Click);
      this.AcceptButton = (IButtonControl) this.buttonSelect;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.buttonCancel;
      this.ClientSize = new Size(364, 346);
      this.Controls.Add((Control) this.gvUserGroups);
      this.Controls.Add((Control) this.buttonCancel);
      this.Controls.Add((Control) this.buttonSelect);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SelectUserGroupForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Select User Group";
      this.ResumeLayout(false);
    }
  }
}
