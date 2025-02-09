// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.FolderSettingsForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.RemotingServices.Acl;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class FolderSettingsForm : Form
  {
    private SetUpContainer setupContainer;
    private IContainer components;
    private GroupContainer gcLoanFolders;
    private StandardIconButton stdIconBtnNew;
    private StandardIconButton stdIconBtnDelete;
    private ToolTip toolTip1;
    private ContextMenuStrip ctxMenuStrip;
    private ToolStripMenuItem createFolderToolStripMenuItem;
    private GridView gvLoanFolders;
    private StandardIconButton stdIconBtnEdit;
    private ToolStripMenuItem deleteFolderToolStripMenuItem;

    public FolderSettingsForm(SetUpContainer setupContainer)
    {
      this.setupContainer = setupContainer;
      this.InitializeComponent();
      this.refreshFolderList();
      this.gvLoanFolders_SelectedIndexChanged((object) this, (EventArgs) null);
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
      this.gcLoanFolders = new GroupContainer();
      this.stdIconBtnEdit = new StandardIconButton();
      this.gvLoanFolders = new GridView();
      this.stdIconBtnNew = new StandardIconButton();
      this.stdIconBtnDelete = new StandardIconButton();
      this.toolTip1 = new ToolTip(this.components);
      this.ctxMenuStrip = new ContextMenuStrip(this.components);
      this.createFolderToolStripMenuItem = new ToolStripMenuItem();
      this.deleteFolderToolStripMenuItem = new ToolStripMenuItem();
      this.gcLoanFolders.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnEdit).BeginInit();
      ((ISupportInitialize) this.stdIconBtnNew).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDelete).BeginInit();
      this.ctxMenuStrip.SuspendLayout();
      this.SuspendLayout();
      this.gcLoanFolders.Controls.Add((Control) this.stdIconBtnEdit);
      this.gcLoanFolders.Controls.Add((Control) this.gvLoanFolders);
      this.gcLoanFolders.Controls.Add((Control) this.stdIconBtnNew);
      this.gcLoanFolders.Controls.Add((Control) this.stdIconBtnDelete);
      this.gcLoanFolders.Dock = DockStyle.Fill;
      this.gcLoanFolders.HeaderForeColor = SystemColors.ControlText;
      this.gcLoanFolders.Location = new Point(0, 0);
      this.gcLoanFolders.Name = "gcLoanFolders";
      this.gcLoanFolders.Size = new Size(631, 440);
      this.gcLoanFolders.TabIndex = 12;
      this.gcLoanFolders.Text = "Loan Folders (#)";
      this.stdIconBtnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnEdit.BackColor = Color.Transparent;
      this.stdIconBtnEdit.Location = new Point(586, 5);
      this.stdIconBtnEdit.MouseDownImage = (Image) null;
      this.stdIconBtnEdit.Name = "stdIconBtnEdit";
      this.stdIconBtnEdit.Size = new Size(16, 16);
      this.stdIconBtnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdIconBtnEdit.TabIndex = 15;
      this.stdIconBtnEdit.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnEdit, "Edit Folder Property");
      this.stdIconBtnEdit.Click += new EventHandler(this.stdIconBtnEdit_Click);
      this.gvLoanFolders.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "colFolderName";
      gvColumn1.Text = "Folder Name";
      gvColumn1.Width = 300;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "colArchiveFolder";
      gvColumn2.Text = "Archive Folder";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "colDuplicateLoan";
      gvColumn3.Text = "Duplicate Loan Check";
      gvColumn3.Width = 140;
      this.gvLoanFolders.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gvLoanFolders.Dock = DockStyle.Fill;
      this.gvLoanFolders.Location = new Point(1, 26);
      this.gvLoanFolders.Name = "gvLoanFolders";
      this.gvLoanFolders.Size = new Size(629, 413);
      this.gvLoanFolders.TabIndex = 14;
      this.gvLoanFolders.SelectedIndexChanged += new EventHandler(this.gvLoanFolders_SelectedIndexChanged);
      this.gvLoanFolders.ItemDoubleClick += new GVItemEventHandler(this.gvLoanFolders_ItemDoubleClick);
      this.stdIconBtnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnNew.BackColor = Color.Transparent;
      this.stdIconBtnNew.Location = new Point(564, 5);
      this.stdIconBtnNew.MouseDownImage = (Image) null;
      this.stdIconBtnNew.Name = "stdIconBtnNew";
      this.stdIconBtnNew.Size = new Size(16, 16);
      this.stdIconBtnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnNew.TabIndex = 13;
      this.stdIconBtnNew.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnNew, "Create New Folder");
      this.stdIconBtnNew.Click += new EventHandler(this.stdIconBtnNew_Click);
      this.stdIconBtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDelete.BackColor = Color.Transparent;
      this.stdIconBtnDelete.Location = new Point(608, 5);
      this.stdIconBtnDelete.MouseDownImage = (Image) null;
      this.stdIconBtnDelete.Name = "stdIconBtnDelete";
      this.stdIconBtnDelete.Size = new Size(16, 16);
      this.stdIconBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDelete.TabIndex = 12;
      this.stdIconBtnDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDelete, "Delete Folder");
      this.stdIconBtnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.ctxMenuStrip.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.createFolderToolStripMenuItem,
        (ToolStripItem) this.deleteFolderToolStripMenuItem
      });
      this.ctxMenuStrip.Name = "ctxMenuStrip";
      this.ctxMenuStrip.Size = new Size(145, 48);
      this.createFolderToolStripMenuItem.Name = "createFolderToolStripMenuItem";
      this.createFolderToolStripMenuItem.Size = new Size(144, 22);
      this.createFolderToolStripMenuItem.Text = "Create Folder";
      this.createFolderToolStripMenuItem.Click += new EventHandler(this.stdIconBtnNew_Click);
      this.deleteFolderToolStripMenuItem.Name = "deleteFolderToolStripMenuItem";
      this.deleteFolderToolStripMenuItem.Size = new Size(144, 22);
      this.deleteFolderToolStripMenuItem.Text = "Delete Folder";
      this.deleteFolderToolStripMenuItem.Click += new EventHandler(this.btnDelete_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(631, 440);
      this.Controls.Add((Control) this.gcLoanFolders);
      this.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (FolderSettingsForm);
      this.Text = nameof (FolderSettingsForm);
      this.gcLoanFolders.ResumeLayout(false);
      ((ISupportInitialize) this.stdIconBtnEdit).EndInit();
      ((ISupportInitialize) this.stdIconBtnNew).EndInit();
      ((ISupportInitialize) this.stdIconBtnDelete).EndInit();
      this.ctxMenuStrip.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void createLoanFolder(string name, bool isArchiveFolder, bool dupLoanCheck)
    {
      name = name.Trim();
      try
      {
        Session.LoanManager.CreateLoanFolder(name);
        AclGroup groupByName = Session.AclGroupManager.GetGroupByName("All Users");
        if (groupByName != (AclGroup) null)
          Session.AclGroupManager.UpdateAclGroupLoanFolder(Session.EncompassEdition != EncompassEdition.Broker ? new LoanFolderInGroup(groupByName.ID, name, false) : new LoanFolderInGroup(groupByName.ID, name, true));
        if (Session.EncompassEdition == EncompassEdition.Broker)
        {
          Persona[] allPersonas = Session.PersonaManager.GetAllPersonas();
          LoanFoldersAclManager aclManager = (LoanFoldersAclManager) Session.ACL.GetAclManager(AclCategory.LoanFolderMove);
          foreach (Persona persona in allPersonas)
          {
            LoanFolderAclInfo loanFolderAclInfo = new LoanFolderAclInfo(211, persona.ID, name, 1, 1);
            aclManager.SetPermission(AclFeature.LoanMgmt_Move, loanFolderAclInfo, persona.ID);
          }
        }
        this.updateArchiveFolderSetting(name, isArchiveFolder, dupLoanCheck, true);
        this.refreshFolderList();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, "An error occurred during creation of the new loan folder, " + name + ". The error message is: " + ex.Message, "Loan Folder Creation Failure", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void refreshFolderList()
    {
      this.gvLoanFolders.Items.Clear();
      LoanFolderInfo[] allLoanFolderInfos = Session.LoanManager.GetAllLoanFolderInfos(true, false);
      for (int index = 0; index < allLoanFolderInfos.Length; ++index)
        this.gvLoanFolders.Items.Add(new GVItem(new string[3]
        {
          allLoanFolderInfos[index].DisplayName,
          allLoanFolderInfos[index].Type == LoanFolderInfo.LoanFolderType.Archive ? "Yes" : "No",
          allLoanFolderInfos[index].IncludeInDuplicateLoanCheck ? "Yes" : "No"
        })
        {
          Tag = (object) allLoanFolderInfos[index].Name
        });
      this.stdIconBtnDelete.Enabled = false;
      this.setLoanFolderCount();
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (this.gvLoanFolders.SelectedItems.Count == 0)
        return;
      for (int index = 0; index < this.gvLoanFolders.SelectedItems.Count; ++index)
      {
        string tag = (string) this.gvLoanFolders.SelectedItems[index].Tag;
        if (SystemSettings.TrashFolder.Equals(tag))
        {
          int num1 = (int) Utils.Dialog((IWin32Window) this, "You cannot delete the system trash folder '" + SystemSettings.TrashFolder + "'.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          try
          {
            if (Session.LoanManager.GetLoanFolderPhysicalSize(tag) > 0)
            {
              int num2 = (int) MessageBox.Show((IWin32Window) this, "The selected folder '" + tag + "' is not empty.\r\nYou can only delete empty folders.", "Non-Empty Folder", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (MessageBox.Show((IWin32Window) this, "Are you sure you want to delete loan folder '" + tag + "'?", "Delete Confirmation", MessageBoxButtons.YesNo) != DialogResult.No)
            {
              Session.LoanManager.DeleteLoanFolder(tag, false);
              ((LoanFolderRuleManager) Session.BPM.GetBpmManager(BpmCategory.LoanFolder)).DeleteRule(tag);
            }
          }
          catch (Exception ex)
          {
            int num3 = (int) MessageBox.Show((IWin32Window) this, ex.Message, "Error deleting loan folder '" + tag + "'", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
        }
      }
      this.refreshFolderList();
    }

    private void setLoanFolderCount()
    {
      this.gcLoanFolders.Text = "Loan Folders (" + (object) this.gvLoanFolders.Items.Count + ")";
    }

    private void stdIconBtnNew_Click(object sender, EventArgs e)
    {
      string[] folders = new string[this.gvLoanFolders.Items.Count];
      for (int nItemIndex = 0; nItemIndex < this.gvLoanFolders.Items.Count; ++nItemIndex)
        folders[nItemIndex] = (string) this.gvLoanFolders.Items[nItemIndex].Tag;
      FolderSettingsDialog folderSettingsDialog = new FolderSettingsDialog(folders, (string) null, false, true);
      if (folderSettingsDialog.ShowDialog((IWin32Window) this.setupContainer) == DialogResult.Cancel)
        return;
      this.createLoanFolder(folderSettingsDialog.FolderName, folderSettingsDialog.IsArchiveFolder, folderSettingsDialog.IncludeInDuplicateLoanCheck);
    }

    private void gvLoanFolders_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.editSelectedItem();
    }

    private void stdIconBtnEdit_Click(object sender, EventArgs e) => this.editSelectedItem();

    private void editSelectedItem()
    {
      if (this.gvLoanFolders.SelectedItems.Count != 1)
        return;
      string tag = (string) this.gvLoanFolders.SelectedItems[0].Tag;
      FolderSettingsDialog folderSettingsDialog = new FolderSettingsDialog((string[]) null, tag, this.gvLoanFolders.SelectedItems[0].SubItems[1].Text == "Yes", this.gvLoanFolders.SelectedItems[0].SubItems[2].Text == "Yes");
      if (folderSettingsDialog.ShowDialog((IWin32Window) this.setupContainer) == DialogResult.Cancel)
        return;
      this.updateArchiveFolderSetting(tag, folderSettingsDialog.IsArchiveFolder, folderSettingsDialog.IncludeInDuplicateLoanCheck, folderSettingsDialog.IsFolderTypeChanged);
    }

    private void updateArchiveFolderSetting(
      string folderName,
      bool isArchiveFolder,
      bool dupLoanCheck,
      bool folderTypeChanged)
    {
      using (CursorActivator.Wait())
      {
        if (folderTypeChanged)
        {
          LoanFolderInfo.LoanFolderType folderType = isArchiveFolder ? LoanFolderInfo.LoanFolderType.Archive : LoanFolderInfo.LoanFolderType.Regular;
          Session.LoanManager.SetLoanFolderType(folderName, folderType);
        }
        Session.LoanManager.SetIncludeInDuplicateLoanCheck(folderName, dupLoanCheck);
        this.refreshFolderList();
      }
    }

    private void gvLoanFolders_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.stdIconBtnDelete.Enabled = this.gvLoanFolders.SelectedItems.Count > 0;
      this.stdIconBtnEdit.Enabled = this.gvLoanFolders.SelectedItems.Count == 1;
    }
  }
}
