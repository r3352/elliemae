// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LoanTemplatePanelExplorer
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class LoanTemplatePanelExplorer : UserControl
  {
    private const string className = "LoanTemplatePanelExplorer";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private FSExplorer tempExplorer;
    private Button btnGo;
    private IContainer components;
    private ListView lvwDefault;
    private ImageList imageListSmall;
    private const string publicBase = "\\\\Public Loan Template";
    private GradientPanel gradientPanel1;
    private Label label1;
    private const string privateBase = "\\\\Personal Loan Template";
    private Sessions.Session session;

    public event EventHandler SelectedEntryChanged;

    public event EventHandler AfterFileRenamed;

    public event EventHandler BeforeFileDeleted;

    public event EventHandler AfterFileDeleted;

    public event EventHandler BeforeFileMoved;

    public event EventHandler AfterFileMoved;

    public LoanTemplatePanelExplorer(Sessions.Session session)
      : this(session, false)
    {
    }

    public LoanTemplatePanelExplorer(Sessions.Session session, bool showPublicOnly)
    {
      this.session = session;
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      this.tempExplorer.SetAsDefaultButtonClick += new EventHandler(this.btnSet_Click);
      this.tempExplorer.SelectedEntryChanged += new EventHandler(this.tempExplorer_SelectedEntryChanged);
      this.tempExplorer.AfterFileRenamed += new EventHandler(this.tempExplorer_AfterFileRenamed);
      this.tempExplorer.BeforeFileDeleted += new EventHandler(this.tempExplorer_BeforeFileDeleted);
      this.tempExplorer.AfterFileDeleted += new EventHandler(this.tempExplorer_AfterFileDeleted);
      this.tempExplorer.BeforeFileMoved += new EventHandler(this.tempExplorer_BeforeFileMoved);
      this.tempExplorer.AfterFileMoved += new EventHandler(this.tempExplorer_AfterFileMoved);
      bool menuItemImportVisible = false;
      TemplateIFSExplorer ifsExplorer = new TemplateIFSExplorer(this.session, EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanTemplate);
      ifsExplorer.DefaultLoanTemplateChanged += new EventHandler(this.refreshDefaultLoanTemplate);
      this.tempExplorer.FileType = FSExplorer.FileTypes.LoanTemplates;
      FileSystemEntry fileSystemEntry = FileSystemEntry.PublicRoot;
      if (!this.session.AclGroupManager.CheckPublicAccessPermission(AclFileType.LoanTemplate))
      {
        this.tempExplorer.HasPublicRight = false;
        fileSystemEntry = FileSystemEntry.PrivateRoot(this.session.UserInfo.Userid);
      }
      this.tempExplorer.SetProperties(false, menuItemImportVisible, false, 5, true);
      FileSystemEntry publicRoot;
      try
      {
        publicRoot = FileSystemEntry.Parse(this.session.GetPrivateProfileString("LoanTemplate", "LastFolderViewed") ?? "");
        if (!ifsExplorer.EntryExists(publicRoot))
          publicRoot = FileSystemEntry.PublicRoot;
      }
      catch
      {
        publicRoot = FileSystemEntry.PublicRoot;
      }
      if (!showPublicOnly)
        showPublicOnly = !this.getPrivateRight();
      if (showPublicOnly && !publicRoot.IsPublic)
        publicRoot = FileSystemEntry.PublicRoot;
      this.tempExplorer.Init((IFSExplorerBase) ifsExplorer, publicRoot, showPublicOnly);
      this.getCurrentDefaultTemplate();
    }

    private void tempExplorer_BeforeFileMoved(object sender, EventArgs e)
    {
      if (this.BeforeFileMoved == null)
        return;
      this.BeforeFileMoved(sender, e);
    }

    private void tempExplorer_BeforeFileDeleted(object sender, EventArgs e)
    {
      if (this.BeforeFileDeleted == null)
        return;
      this.BeforeFileDeleted(sender, e);
    }

    private void tempExplorer_AfterFileMoved(object sender, EventArgs e)
    {
      if (this.AfterFileMoved != null)
        this.AfterFileMoved(sender, e);
      if (sender == null || this.lvwDefault.Items.Count <= 0 || this.lvwDefault.Items[0].Tag == null)
        return;
      string tag = (string) this.lvwDefault.Items[0].Tag;
      FileSystemEntry[] fileSystemEntryArray = (FileSystemEntry[]) sender;
      if (fileSystemEntryArray[0].Type == FileSystemEntry.Types.File)
      {
        if (!(fileSystemEntryArray[0].ToDisplayString() == tag))
          return;
        this.session.WritePrivateProfileString("LoanTemplate", "Default", fileSystemEntryArray[1].ToDisplayString());
        this.getCurrentDefaultTemplate();
      }
      else
      {
        if (!tag.StartsWith(fileSystemEntryArray[0].ToDisplayString()))
          return;
        this.session.WritePrivateProfileString("LoanTemplate", "Default", fileSystemEntryArray[1].ToDisplayString() + tag.Substring(fileSystemEntryArray[0].ToDisplayString().Length));
        this.getCurrentDefaultTemplate();
      }
    }

    private void tempExplorer_AfterFileDeleted(object sender, EventArgs e)
    {
      if (this.AfterFileDeleted != null)
        this.AfterFileDeleted(sender, e);
      if (sender == null || !(sender is FileSystemEntry) || this.lvwDefault.Items.Count <= 0 || this.lvwDefault.Items[0].Tag == null)
        return;
      FileSystemEntry fileSystemEntry = (FileSystemEntry) sender;
      if (fileSystemEntry.ToDisplayString() == (string) this.lvwDefault.Items[0].Tag)
      {
        this.session.WritePrivateProfileString("LoanTemplate", "Default", "");
        this.lvwDefault.Items.Clear();
        this.lvwDefault.Items.Add("");
        this.lvwDefault.Items[0].Tag = (object) "";
      }
      this.session.DeleteFavoriteLoanTemplateSet(fileSystemEntry.ToDisplayString());
    }

    private void tempExplorer_AfterFileRenamed(object sender, EventArgs e)
    {
      if (this.AfterFileRenamed != null)
        this.AfterFileRenamed(sender, e);
      if (sender == null || this.lvwDefault.Items.Count <= 0 || this.lvwDefault.Items[0].Tag == null)
        return;
      string tag = (string) this.lvwDefault.Items[0].Tag;
      FileSystemEntry[] fileSystemEntryArray = (FileSystemEntry[]) sender;
      if (fileSystemEntryArray[0].Type == FileSystemEntry.Types.File)
      {
        if (fileSystemEntryArray[0].ToDisplayString() == tag)
        {
          this.session.WritePrivateProfileString("LoanTemplate", "Default", fileSystemEntryArray[1].ToDisplayString());
          this.getCurrentDefaultTemplate();
        }
        this.session.RenameFavoriteLoanTemplateSet(fileSystemEntryArray[0].ToDisplayString(), fileSystemEntryArray[1].ToDisplayString(), true);
      }
      else
      {
        if (tag.StartsWith(fileSystemEntryArray[0].ToDisplayString()))
        {
          this.session.WritePrivateProfileString("LoanTemplate", "Default", fileSystemEntryArray[1].ToDisplayString() + tag.Substring(fileSystemEntryArray[0].ToDisplayString().Length));
          this.getCurrentDefaultTemplate();
        }
        this.session.RenameFavoriteLoanTemplateSet(fileSystemEntryArray[0].ToDisplayString(), fileSystemEntryArray[1].ToDisplayString(), false);
      }
    }

    private void tempExplorer_SelectedEntryChanged(object sender, EventArgs e)
    {
      if (this.SelectedEntryChanged == null)
        return;
      this.SelectedEntryChanged(sender, e);
    }

    public FileSystemEntry SelectedFolder
    {
      get => this.tempExplorer.CurrentFolder;
      set => this.tempExplorer.SetFolder(value);
    }

    public void SetSelectedItems(string[] fileTypePathList)
    {
      this.tempExplorer.MakeItemSelected(AclFileType.LoanTemplate, fileTypePathList);
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

    private bool getPrivateRight()
    {
      return ((FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.SettingsTab_Personal_LoanTemplateSets);
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (LoanTemplatePanelExplorer));
      this.tempExplorer = new FSExplorer(this.session);
      this.btnGo = new Button();
      this.gradientPanel1 = new GradientPanel();
      this.label1 = new Label();
      this.lvwDefault = new ListView();
      this.imageListSmall = new ImageList(this.components);
      this.gradientPanel1.SuspendLayout();
      this.SuspendLayout();
      this.tempExplorer.Dock = DockStyle.Fill;
      this.tempExplorer.FolderComboSelectedIndex = -1;
      this.tempExplorer.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.tempExplorer.HasPublicRight = true;
      this.tempExplorer.Location = new Point(0, 33);
      this.tempExplorer.Name = "tempExplorer";
      this.tempExplorer.RenameButtonSize = new Size(62, 22);
      this.tempExplorer.setContactType = EllieMae.EMLite.ContactUI.ContactType.BizPartner;
      this.tempExplorer.Size = new Size(625, 447);
      this.tempExplorer.TabIndex = 1;
      this.tempExplorer.Leave += new EventHandler(this.tempExplorer_Leave);
      this.btnGo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnGo.Location = new Point(566, 5);
      this.btnGo.Name = "btnGo";
      this.btnGo.Size = new Size(56, 22);
      this.btnGo.TabIndex = 31;
      this.btnGo.Text = "&Go To";
      this.btnGo.Click += new EventHandler(this.btnGo_Click);
      this.gradientPanel1.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gradientPanel1.Controls.Add((Control) this.label1);
      this.gradientPanel1.Controls.Add((Control) this.lvwDefault);
      this.gradientPanel1.Controls.Add((Control) this.btnGo);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Location = new Point(0, 0);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(625, 33);
      this.gradientPanel1.TabIndex = 34;
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Location = new Point(3, 10);
      this.label1.Name = "label1";
      this.label1.Size = new Size(41, 14);
      this.label1.TabIndex = 33;
      this.label1.Text = "Default";
      this.lvwDefault.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lvwDefault.BackColor = SystemColors.Window;
      this.lvwDefault.Location = new Point(46, 7);
      this.lvwDefault.Name = "lvwDefault";
      this.lvwDefault.Size = new Size(516, 20);
      this.lvwDefault.SmallImageList = this.imageListSmall;
      this.lvwDefault.TabIndex = 32;
      this.lvwDefault.UseCompatibleStateImageBehavior = false;
      this.lvwDefault.View = View.List;
      this.imageListSmall.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageListSmall.ImageStream");
      this.imageListSmall.TransparentColor = Color.Transparent;
      this.imageListSmall.Images.SetKeyName(0, "");
      this.imageListSmall.Images.SetKeyName(1, "");
      this.imageListSmall.Images.SetKeyName(2, "");
      this.imageListSmall.Images.SetKeyName(3, "");
      this.imageListSmall.Images.SetKeyName(4, "template.bmp");
      this.Controls.Add((Control) this.tempExplorer);
      this.Controls.Add((Control) this.gradientPanel1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (LoanTemplatePanelExplorer);
      this.Size = new Size(625, 480);
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.ResumeLayout(false);
    }

    private void tempExplorer_Leave(object sender, EventArgs e)
    {
      try
      {
        this.session.WritePrivateProfileString("LoanTemplate", "LastFolderViewed", this.tempExplorer.CurrentFolder.ToString());
      }
      catch (Exception ex)
      {
      }
    }

    private void btnSet_Click(object sender, EventArgs e)
    {
      if (this.tempExplorer.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "To setup a default loan template, you have to select a loan template first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.tempExplorer.SelectedItems[0].Tag.ToString() == "")
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "To setup a default loan template, you have to select a loan template first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        FileSystemEntry tag = (FileSystemEntry) this.tempExplorer.SelectedItems[0].Tag;
        if (tag.Type == FileSystemEntry.Types.Folder)
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, "To setup a default loan template, you have to select a loan template first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          this.session.WritePrivateProfileString("LoanTemplate", "Default", tag.ToString());
          this.lvwDefault.Items.Clear();
          this.lvwDefault.Items.Add(tag.ToDisplayString().Replace("Public:", "Public Loan Templates").Replace("Personal:", "Personal Loan Templates"));
          this.lvwDefault.Items[0].ImageIndex = 4;
          this.lvwDefault.Items[0].Tag = (object) tag.ToDisplayString();
        }
      }
    }

    private void refreshDefaultLoanTemplate(object sender, EventArgs e)
    {
      this.getCurrentDefaultTemplate();
    }

    private void btnGo_Click(object sender, EventArgs e)
    {
      if (this.lvwDefault.Items.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You don't have default loan template.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        string tag = (string) this.lvwDefault.Items[0].Tag;
        FileSystemEntry fileSystemEntry;
        try
        {
          fileSystemEntry = FileSystemEntry.Parse(tag, this.session.UserID);
        }
        catch
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "The default loan template cannot be found.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        if (fileSystemEntry.ParentFolder != null && !this.session.ConfigurationManager.TemplateSettingsObjectExists(EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanTemplate, fileSystemEntry.ParentFolder))
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, "The default loan template cannot be found.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
          this.tempExplorer.SetFolder(fileSystemEntry.ParentFolder);
      }
    }

    private void getCurrentDefaultTemplate()
    {
      string privateProfileString = this.session.GetPrivateProfileString("LoanTemplate", "Default");
      try
      {
        FileSystemEntry fileSystemEntry = FileSystemEntry.Parse(privateProfileString);
        this.lvwDefault.Items.Clear();
        if (!((FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features)).CheckPermission(AclFeature.SettingsTab_Personal_LoanTemplateSets, this.session.UserInfo) && fileSystemEntry.ToDisplayString().IndexOf("Personal:") > -1)
          return;
        this.lvwDefault.Items.Add(fileSystemEntry.ToDisplayString().Replace("Public:", "Public Loan Templates").Replace("Personal:", "Personal Loan Templates"));
        this.lvwDefault.Items[0].ImageIndex = 4;
        this.lvwDefault.Items[0].Tag = (object) fileSystemEntry.ToDisplayString();
      }
      catch
      {
        this.lvwDefault.Items.Clear();
        this.lvwDefault.Items.Add("");
        this.lvwDefault.Items[0].Tag = (object) "";
      }
    }
  }
}
