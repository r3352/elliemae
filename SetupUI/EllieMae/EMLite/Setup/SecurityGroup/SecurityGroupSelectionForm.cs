// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SecurityGroup.SecurityGroupSelectionForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.SecurityGroup
{
  public class SecurityGroupSelectionForm : Form
  {
    private AclGroup[] availableGroups;
    private AclGroup[] currentAssignedGroups;
    private string userID = "";
    private ListBox lsbAvaGroups;
    private ListBox lsbCurrentGroups;
    private Button btnAdd;
    private Button btnRemove;
    private Button btnSave;
    private Button btnCancel;
    private System.ComponentModel.Container components;

    public SecurityGroupSelectionForm(string userID)
    {
      this.InitializeComponent();
      this.userID = userID;
      this.InitialFormValue();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lsbAvaGroups = new ListBox();
      this.lsbCurrentGroups = new ListBox();
      this.btnAdd = new Button();
      this.btnRemove = new Button();
      this.btnSave = new Button();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.lsbAvaGroups.Location = new Point(12, 16);
      this.lsbAvaGroups.Name = "lsbAvaGroups";
      this.lsbAvaGroups.SelectionMode = SelectionMode.MultiSimple;
      this.lsbAvaGroups.Size = new Size(148, 290);
      this.lsbAvaGroups.TabIndex = 0;
      this.lsbCurrentGroups.Location = new Point(252, 12);
      this.lsbCurrentGroups.Name = "lsbCurrentGroups";
      this.lsbCurrentGroups.SelectionMode = SelectionMode.MultiSimple;
      this.lsbCurrentGroups.Size = new Size(152, 290);
      this.lsbCurrentGroups.TabIndex = 1;
      this.btnAdd.Location = new Point(168, 100);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(75, 23);
      this.btnAdd.TabIndex = 2;
      this.btnAdd.Text = "Add >>";
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.btnRemove.Location = new Point(168, 144);
      this.btnRemove.Name = "btnRemove";
      this.btnRemove.Size = new Size(75, 23);
      this.btnRemove.TabIndex = 3;
      this.btnRemove.Text = "<< Remove";
      this.btnRemove.Click += new EventHandler(this.btnRemove_Click);
      this.btnSave.Location = new Point(252, 316);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 23);
      this.btnSave.TabIndex = 4;
      this.btnSave.Text = "Save";
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(332, 316);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "Cancel";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(416, 353);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.btnRemove);
      this.Controls.Add((Control) this.btnAdd);
      this.Controls.Add((Control) this.lsbCurrentGroups);
      this.Controls.Add((Control) this.lsbAvaGroups);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Name = nameof (SecurityGroupSelectionForm);
      this.ShowInTaskbar = false;
      this.Text = "Security groups selection";
      this.ResumeLayout(false);
    }

    private void InitialFormValue()
    {
      this.availableGroups = Session.AclGroupManager.GetAllGroups();
      this.currentAssignedGroups = Session.AclGroupManager.GetGroupsOfUser(this.userID);
      this.loadCurrentAssignedGroupList();
      this.loadAvailableGroupList();
    }

    private void loadAvailableGroupList()
    {
      foreach (AclGroup availableGroup in this.availableGroups)
      {
        if (!this.lsbCurrentGroups.Items.Contains((object) availableGroup))
          this.lsbAvaGroups.Items.Add((object) availableGroup);
      }
    }

    private void loadCurrentAssignedGroupList()
    {
      this.lsbCurrentGroups.Items.AddRange((object[]) this.currentAssignedGroups);
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      if (this.lsbAvaGroups.SelectedItems == null || this.lsbAvaGroups.SelectedItems.Count <= 0)
        return;
      AclGroup[] aclGroupArray = new AclGroup[this.lsbAvaGroups.SelectedItems.Count];
      this.lsbAvaGroups.SelectedItems.CopyTo((Array) aclGroupArray, 0);
      this.lsbCurrentGroups.Items.AddRange((object[]) aclGroupArray);
      for (int index = this.lsbAvaGroups.SelectedItems.Count - 1; index >= 0; --index)
        this.lsbAvaGroups.Items.Remove(this.lsbAvaGroups.SelectedItems[index]);
    }

    private void btnRemove_Click(object sender, EventArgs e)
    {
      if (this.lsbCurrentGroups.SelectedItems == null || this.lsbCurrentGroups.SelectedItems.Count <= 0)
        return;
      AclGroup[] aclGroupArray = new AclGroup[this.lsbCurrentGroups.SelectedItems.Count];
      this.lsbCurrentGroups.SelectedItems.CopyTo((Array) aclGroupArray, 0);
      this.lsbAvaGroups.Items.AddRange((object[]) aclGroupArray);
      for (int index = this.lsbCurrentGroups.SelectedItems.Count - 1; index >= 0; --index)
        this.lsbCurrentGroups.Items.Remove(this.lsbCurrentGroups.SelectedItems[index]);
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
    }
  }
}
