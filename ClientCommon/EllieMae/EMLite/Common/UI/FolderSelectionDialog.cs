// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.FolderSelectionDialog
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class FolderSelectionDialog : Form
  {
    private FileSystemExplorer explorer;
    private FileSystemEntry selectedFolder;
    private bool requireWriteAccess;
    private Sessions.Session session;
    private IContainer components;
    private Panel pnlBrowser;
    private DialogButtons dlgButtons;

    public FolderSelectionDialog(IFileSystem fileSystem, bool requireWriteAccess)
    {
      this.session = Session.DefaultInstance;
    }

    public FolderSelectionDialog(
      IFileSystem fileSystem,
      bool requireWriteAccess,
      Sessions.Session session)
    {
      this.InitializeComponent();
      this.requireWriteAccess = requireWriteAccess;
      this.session = session;
      this.explorer = new FileSystemExplorer();
      this.explorer.AllowMultiselect = false;
      this.explorer.HideAllActions(true);
      this.explorer.ShowAction(FileFolderAction.CreateFolder);
      this.explorer.ShowAction(FileFolderAction.RenameFolderOrFile);
      this.explorer.DisplayFoldersOnly = true;
      this.explorer.AttachFileSystem(this.session, fileSystem);
      this.explorer.Dock = DockStyle.Fill;
      this.pnlBrowser.Controls.Add((Control) this.explorer);
    }

    public FileSystemEntry SelectedFolder => this.selectedFolder;

    private void dlgButtons_OK(object sender, EventArgs e)
    {
      FileSystemEntry fileSystemEntry = this.explorer.CurrentFolder;
      ExplorerListItem[] selectedItems = this.explorer.GetSelectedItems();
      if (selectedItems.Length != 0)
        fileSystemEntry = selectedItems[0].FileFolderEntry;
      if (this.requireWriteAccess && fileSystemEntry.Access != AclResourceAccess.ReadWrite)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You do not have the necessary permissions to modify the contents of the folder '" + fileSystemEntry.ToDisplayString() + "'.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        this.selectedFolder = fileSystemEntry;
        this.DialogResult = DialogResult.OK;
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
      this.pnlBrowser = new Panel();
      this.dlgButtons = new DialogButtons();
      this.SuspendLayout();
      this.pnlBrowser.Dock = DockStyle.Fill;
      this.pnlBrowser.Location = new Point(0, 0);
      this.pnlBrowser.Name = "pnlBrowser";
      this.pnlBrowser.Size = new Size(382, 373);
      this.pnlBrowser.TabIndex = 0;
      this.dlgButtons.Dock = DockStyle.Bottom;
      this.dlgButtons.Location = new Point(0, 373);
      this.dlgButtons.Name = "dlgButtons";
      this.dlgButtons.Size = new Size(382, 47);
      this.dlgButtons.TabIndex = 1;
      this.dlgButtons.OK += new EventHandler(this.dlgButtons_OK);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(382, 420);
      this.Controls.Add((Control) this.pnlBrowser);
      this.Controls.Add((Control) this.dlgButtons);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (FolderSelectionDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Select Folder";
      this.ResumeLayout(false);
    }
  }
}
