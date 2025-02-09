// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ConditionalLetterExplorer
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class ConditionalLetterExplorer : UserControl
  {
    private TemplateIFSExplorer ifsExplorer;
    private FileSystemEntry currentFolder = FileSystemEntry.PublicRoot;
    private Sessions.Session session;
    private IContainer components;
    private FSExplorer tempExplorer;

    public ConditionalLetterExplorer(Sessions.Session session)
    {
      this.session = session;
      this.InitializeComponent();
      this.initForm();
    }

    private void initForm()
    {
      this.ifsExplorer = new TemplateIFSExplorer(this.session, EllieMae.EMLite.ClientServer.TemplateSettingsType.ConditionalLetter);
      this.tempExplorer.FileType = FSExplorer.FileTypes.ConditionalLetter;
      this.tempExplorer.HasPublicRight = true;
      this.tempExplorer.SetProperties(false, false, false, 25, true);
      this.tempExplorer.Init((IFSExplorerBase) this.ifsExplorer, FileSystemEntry.PublicRoot, true);
      bool canCreateEdit = true;
      FeaturesAclManager aclManager = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
      if (!this.session.UserInfo.IsSuperAdministrator() && !aclManager.GetUserApplicationRight(AclFeature.SettingsTab_Company_ConditionalApprovalLetter))
        canCreateEdit = false;
      this.tempExplorer.SetupForConditionalLetter(canCreateEdit);
    }

    public FileSystemEntry SelectedFolder
    {
      get => this.tempExplorer.CurrentFolder;
      set => this.tempExplorer.SetFolder(value);
    }

    public void SetSelectedItems(string[] fileTypePathList)
    {
      this.tempExplorer.MakeItemSelected(AclFileType.ConditionalApprovalLetter, fileTypePathList);
    }

    public List<FileSystemEntry> SelectedFileSystemEntries
    {
      get
      {
        List<FileSystemEntry> fileSystemEntries = new List<FileSystemEntry>();
        foreach (GVItem selectedItem in this.tempExplorer.SelectedItems)
          fileSystemEntries.Add((FileSystemEntry) selectedItem.Tag);
        return fileSystemEntries;
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
      this.tempExplorer = new FSExplorer(this.session);
      this.SuspendLayout();
      this.tempExplorer.Dock = DockStyle.Fill;
      this.tempExplorer.HasPublicRight = true;
      this.tempExplorer.Location = new Point(0, 0);
      this.tempExplorer.Name = "tempExplorer";
      this.tempExplorer.setContactType = EllieMae.EMLite.ContactUI.ContactType.BizPartner;
      this.tempExplorer.Size = new Size(722, 477);
      this.tempExplorer.TabIndex = 69;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.tempExplorer);
      this.Name = nameof (ConditionalLetterExplorer);
      this.Size = new Size(722, 477);
      this.ResumeLayout(false);
    }
  }
}
