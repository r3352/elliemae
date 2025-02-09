// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.SavePipelineViewDialog
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public class SavePipelineViewDialog : Form
  {
    private PipelineView currentView;
    private string[] namesInUse;
    private FileSystemEntry selectedEntry;
    private PipelineView selectedUserEntry;
    private IContainer components;
    private Label label1;
    private RadioButton radUpdate;
    private RadioButton radNew;
    private TextBox txtName;
    private DialogButtons dlgButtons;
    private CheckBox chkDefault;

    public SavePipelineViewDialog(PipelineView view, string[] namesInUse, bool allowOverwrite)
    {
      this.InitializeComponent();
      this.currentView = view;
      this.namesInUse = namesInUse;
      if (allowOverwrite || this.editSuperAdminView())
        return;
      this.radNew.Checked = true;
      this.radUpdate.Enabled = false;
    }

    public FileSystemEntry SelectedEntry => this.selectedEntry;

    public PipelineView SelectedUserEntry => this.selectedUserEntry;

    private void radNew_CheckedChanged(object sender, EventArgs e)
    {
      this.txtName.Enabled = this.radNew.Checked;
    }

    private bool editSuperAdminView()
    {
      if (!(Session.UserID == "admin"))
        return false;
      return this.currentView.Name == "Super Administrator - Default View" || this.currentView.Name == "Administrator - Default View";
    }

    private void saveSuperAdminView()
    {
      if (Utils.Dialog((IWin32Window) this, "Both Administrator and Super Administrator Views will be updated.  Do you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
        return;
      PipelineViewAclManager aclManager = (PipelineViewAclManager) Session.ACL.GetAclManager(AclCategory.PersonaPipelineView);
      PersonaPipelineView personaPipelineView = new PersonaPipelineView(Persona.SuperAdministrator.ID, "New View");
      personaPipelineView.Name = "Default View";
      personaPipelineView.LoanFolders = this.currentView.LoanFolder;
      foreach (TableLayout.Column column in this.currentView.Layout)
        personaPipelineView.Columns.Add(column.ColumnID, column.SortPriority == 1 ? column.SortOrder : SortOrder.None, column.Width);
      aclManager.ReplacePersonaPipelineViews(Persona.SuperAdministrator.ID, new PersonaPipelineView[1]
      {
        personaPipelineView.Duplicate(Persona.SuperAdministrator.ID)
      });
      aclManager.ReplacePersonaPipelineViews(Persona.Administrator.ID, new PersonaPipelineView[1]
      {
        personaPipelineView.Duplicate(Persona.Administrator.ID)
      });
      if (!this.chkDefault.Checked)
        return;
      Session.WritePrivateProfileString("Pipeline.DefaultView", this.currentView.Name);
    }

    private void saveUserView()
    {
      PipelineViewAclManager aclManager = (PipelineViewAclManager) Session.ACL.GetAclManager(AclCategory.PersonaPipelineView);
      UserPipelineView pipelineView = new UserPipelineView(Session.UserID, this.updateUserViewEntry(), this.currentView.ViewID);
      pipelineView.LoanFolders = this.currentView.LoanFolder;
      pipelineView.ExternalOrgId = this.currentView.ExternalOrgId;
      pipelineView.Filter = this.currentView.Filter;
      pipelineView.Ownership = this.currentView.LoanOwnership;
      pipelineView.OrgType = this.currentView.LoanOrgType;
      foreach (TableLayout.Column column in this.currentView.Layout)
      {
        UserPipelineViewColumn newColumn = new UserPipelineViewColumn();
        newColumn.ColumnDBName = column.ColumnID;
        newColumn.Width = column.Width;
        newColumn.Alignment = column.Alignment.ToString();
        if (column.SortPriority != -1)
        {
          newColumn.SortOrder = column.SortOrder;
          newColumn.SortPriority = column.SortPriority;
        }
        pipelineView.Columns.Add(newColumn);
      }
      if (this.radNew.Checked)
      {
        UserPipelineView pipelineUserView = aclManager.CreatePipelineUserView(pipelineView.Duplicate(Session.UserID), Session.UserID);
        this.currentView.ViewID = pipelineUserView != null ? pipelineUserView.ViewID : -1;
      }
      else
        aclManager.UpdatePipelineUserView(pipelineView, Session.UserID, this.currentView.Name);
      this.currentView.IsUserView = true;
      this.selectedUserEntry = this.currentView;
      if (!this.chkDefault.Checked)
        return;
      Session.WritePrivateProfileString("Pipeline.DefaultView", "Personal:\\" + Session.UserID + "\\" + this.currentView.Name);
    }

    private void dlgButtons_OK(object sender, EventArgs e)
    {
      if (this.radUpdate.Checked && this.editSuperAdminView())
      {
        this.saveSuperAdminView();
        this.DialogResult = DialogResult.Ignore;
      }
      else
      {
        try
        {
          if (this.radNew.Checked)
          {
            string name = this.txtName.Text.Trim();
            if (name.Length == 0)
            {
              int num = (int) Utils.Dialog((IWin32Window) this, "The specified view name is invalid. The name must be non-empty.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
              return;
            }
            if (this.isNameInUse(name))
            {
              int num = (int) Utils.Dialog((IWin32Window) this, "A view with the name '" + name + "' already exists. You must provide a unique name for this view.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return;
            }
          }
          this.saveUserView();
        }
        catch (Exception ex)
        {
          ErrorDialog.Display(ex);
        }
        this.DialogResult = DialogResult.OK;
      }
    }

    private string updateUserViewEntry()
    {
      if (this.radUpdate.Checked)
        return this.currentView.Name;
      string str = this.txtName.Text.Trim();
      this.currentView.Name = str;
      return str;
    }

    private FileSystemEntry getViewEntry()
    {
      if (this.radUpdate.Checked)
        return new FileSystemEntry("\\" + this.currentView.Name, FileSystemEntry.Types.File, Session.UserID);
      string str = this.txtName.Text.Trim();
      if (!FileSystem.IsValidFilename(str))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The specified view name is invalid. The name must be non-empty and cannot contain the backslash (\\) character.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return (FileSystemEntry) null;
      }
      if (this.isNameInUse(str))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "A view with the name '" + str + "' already exists. You must provide a unique name for this view.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return (FileSystemEntry) null;
      }
      this.currentView.Name = str;
      return new FileSystemEntry("\\" + this.currentView.Name, FileSystemEntry.Types.File, Session.UserID);
    }

    private bool isNameInUse(string name)
    {
      foreach (string strB in this.namesInUse)
      {
        if (string.Compare(name, strB, true) == 0)
          return true;
      }
      return false;
    }

    private void txtName_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\\')
        return;
      e.Handled = true;
    }

    private void txtName_TextChanged(object sender, EventArgs e)
    {
      if (!this.txtName.Text.Contains("\\"))
        return;
      this.txtName.Text = this.txtName.Text.Replace("\\", "");
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.radUpdate = new RadioButton();
      this.radNew = new RadioButton();
      this.txtName = new TextBox();
      this.dlgButtons = new DialogButtons();
      this.chkDefault = new CheckBox();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 14);
      this.label1.Name = "label1";
      this.label1.Size = new Size(277, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "Search filters, columns and sorting are saved in a view.";
      this.radUpdate.AutoSize = true;
      this.radUpdate.Checked = true;
      this.radUpdate.Location = new Point(14, 40);
      this.radUpdate.Name = "radUpdate";
      this.radUpdate.Size = new Size(142, 18);
      this.radUpdate.TabIndex = 1;
      this.radUpdate.TabStop = true;
      this.radUpdate.Text = "Update the current view";
      this.radUpdate.UseVisualStyleBackColor = true;
      this.radNew.AutoSize = true;
      this.radNew.Location = new Point(14, 62);
      this.radNew.Name = "radNew";
      this.radNew.Size = new Size(65, 18);
      this.radNew.TabIndex = 2;
      this.radNew.Text = "Save as";
      this.radNew.UseVisualStyleBackColor = true;
      this.radNew.CheckedChanged += new EventHandler(this.radNew_CheckedChanged);
      this.txtName.Enabled = false;
      this.txtName.Location = new Point(78, 60);
      this.txtName.MaxLength = 100;
      this.txtName.Name = "txtName";
      this.txtName.Size = new Size(210, 20);
      this.txtName.TabIndex = 3;
      this.txtName.TextChanged += new EventHandler(this.txtName_TextChanged);
      this.txtName.KeyPress += new KeyPressEventHandler(this.txtName_KeyPress);
      this.dlgButtons.Dock = DockStyle.Bottom;
      this.dlgButtons.Location = new Point(0, 126);
      this.dlgButtons.Name = "dlgButtons";
      this.dlgButtons.OKText = "&Save";
      this.dlgButtons.Size = new Size(306, 44);
      this.dlgButtons.TabIndex = 4;
      this.dlgButtons.OK += new EventHandler(this.dlgButtons_OK);
      this.chkDefault.AutoSize = true;
      this.chkDefault.Checked = true;
      this.chkDefault.CheckState = CheckState.Checked;
      this.chkDefault.Location = new Point(14, 93);
      this.chkDefault.Name = "chkDefault";
      this.chkDefault.Size = new Size(137, 18);
      this.chkDefault.TabIndex = 5;
      this.chkDefault.Text = "Set as my default view";
      this.chkDefault.UseVisualStyleBackColor = true;
      this.AcceptButton = (IButtonControl) this.dlgButtons;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(306, 170);
      this.Controls.Add((Control) this.chkDefault);
      this.Controls.Add((Control) this.dlgButtons);
      this.Controls.Add((Control) this.txtName);
      this.Controls.Add((Control) this.radNew);
      this.Controls.Add((Control) this.radUpdate);
      this.Controls.Add((Control) this.label1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SavePipelineViewDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Save View";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
