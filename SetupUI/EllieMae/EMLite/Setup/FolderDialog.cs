// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.FolderDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System.Windows.Forms;
using System.Windows.Forms.Design;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class FolderDialog : FolderNameEditor
  {
    private FolderNameEditor.FolderBrowser folderDialog;

    public FolderDialog()
    {
      this.folderDialog = new FolderNameEditor.FolderBrowser();
      this.InitializeDialog(this.folderDialog);
      this.folderDialog.StartLocation = FolderNameEditor.FolderBrowserFolder.MyComputer;
    }

    public string SelectedDirectory => this.folderDialog.DirectoryPath;

    public DialogResult ShowDialog(IWin32Window w) => this.folderDialog.ShowDialog(w);
  }
}
