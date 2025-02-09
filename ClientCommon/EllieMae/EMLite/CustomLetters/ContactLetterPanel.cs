// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CustomLetters.ContactLetterPanel
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.CustomLetters
{
  public class ContactLetterPanel : UserControl
  {
    private const string className = "ContactLetterPanel";
    private static readonly string sw = Tracing.SwCustomLetters;
    private System.ComponentModel.Container components;
    private Button btnPreview;
    private Button btnPrint;
    private Button btnSend;
    private Button btnCancel;
    private bool _isPublic;
    private string _LetterToSend = "";
    private string _Action = "";
    private FSExplorer fsExplorer1;
    private Panel bottomPanel;
    private Panel topPanel;
    private EllieMae.EMLite.ContactUI.ContactType _contactType;
    private EMHelpLink emHelpLink1;
    private Sessions.Session session;
    private bool _IsPrintPreview;

    public event ContactLetterPanel.LetterSelectedEventHandler LetterSelected;

    public bool IsPublic => this._isPublic;

    public string LetterToSend
    {
      get => this._LetterToSend;
      set => this._LetterToSend = value;
    }

    public bool IsPrintPreview
    {
      get => this._IsPrintPreview;
      set => this._IsPrintPreview = value;
    }

    protected virtual void OnLetterSelected(LetterSelectedEventArg e) => this.LetterSelected(e);

    public ContactLetterPanel(Sessions.Session session)
      : this(session, (IMainScreen) null, false, EllieMae.EMLite.ContactUI.ContactType.Borrower, false)
    {
    }

    public ContactLetterPanel(
      Sessions.Session session,
      bool bEmailMerge,
      EllieMae.EMLite.ContactUI.ContactType contactType,
      bool isSettingsSync = false)
      : this(session, (IMainScreen) null, bEmailMerge, contactType, false, isSettingsSync)
    {
    }

    public ContactLetterPanel(
      Sessions.Session session,
      IMainScreen mainScreen,
      bool bEmailMerge,
      EllieMae.EMLite.ContactUI.ContactType contactType,
      bool docMgmtOnly,
      bool isSettingsSync = false)
    {
      this.session = session;
      this._contactType = contactType;
      this.InitializeComponent();
      if (this.DesignMode)
        return;
      string empty = string.Empty;
      string uri = this.session.GetPrivateProfileString(contactType != EllieMae.EMLite.ContactUI.ContactType.BizPartner ? "BorContactLetterPanel" : "BizContactLetterPanel", "LastFolderViewed") ?? "";
      CustomLetterIFSExplorer ifsExplorer = new CustomLetterIFSExplorer(this.session, this._contactType);
      this.fsExplorer1.FileType = FSExplorer.FileTypes.CustomLetters;
      this.fsExplorer1.HasPublicRight = this.GetPublicRight(contactType);
      FileSystemEntry publicRoot = FileSystemEntry.PublicRoot;
      try
      {
        publicRoot = FileSystemEntry.Parse(uri);
        if (!ifsExplorer.EntryExists(publicRoot))
          publicRoot = FileSystemEntry.PublicRoot;
      }
      catch
      {
      }
      this.fsExplorer1.setContactType = contactType;
      this.fsExplorer1.SetProperties(false, true, false, false);
      bool publicOnly = !this.GetPrivateRight(contactType);
      this.fsExplorer1.Init((IFSExplorerBase) ifsExplorer, publicRoot, publicOnly);
      if (docMgmtOnly)
      {
        this.btnSend.Enabled = false;
        this.btnPrint.Enabled = false;
        this.btnPreview.Enabled = false;
      }
      if (isSettingsSync)
      {
        this.btnSend.Visible = false;
        this.btnPrint.Visible = false;
        this.btnPreview.Visible = false;
        this.btnCancel.Visible = false;
      }
      this.emHelpLink1.AssignSession(session);
    }

    private bool GetPrivateRight(EllieMae.EMLite.ContactUI.ContactType cType)
    {
      AclFeature feature;
      if (cType != EllieMae.EMLite.ContactUI.ContactType.Borrower)
      {
        if (cType != EllieMae.EMLite.ContactUI.ContactType.BizPartner)
          throw new Exception("Invalid contact type.");
        feature = AclFeature.Business_Contacts_Personal_CustomLetters;
      }
      else
        feature = AclFeature.Borrower_Contacts_Personal_CustomLetters;
      return ((FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(feature);
    }

    public void PerformCancel() => this.btnCancel.PerformClick();

    private bool GetPublicRight(EllieMae.EMLite.ContactUI.ContactType cType)
    {
      if (UserInfo.IsSuperAdministrator(this.session.UserID, this.session.UserInfo.UserPersonas))
        return true;
      AclFileType fileType = AclFileType.BizCustomLetters;
      switch (cType)
      {
        case EllieMae.EMLite.ContactUI.ContactType.Borrower:
          fileType = AclFileType.BorrowerCustomLetters;
          break;
        case EllieMae.EMLite.ContactUI.ContactType.BizPartner:
          fileType = AclFileType.BizCustomLetters;
          break;
      }
      return this.session.AclGroupManager.CheckPublicAccessPermission(fileType);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnPreview = new Button();
      this.btnPrint = new Button();
      this.btnSend = new Button();
      this.btnCancel = new Button();
      this.fsExplorer1 = new FSExplorer(this.session);
      this.bottomPanel = new Panel();
      this.emHelpLink1 = new EMHelpLink();
      this.topPanel = new Panel();
      this.bottomPanel.SuspendLayout();
      this.topPanel.SuspendLayout();
      this.SuspendLayout();
      this.btnPreview.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnPreview.Location = new Point(422, 10);
      this.btnPreview.Name = "btnPreview";
      this.btnPreview.Size = new Size(75, 24);
      this.btnPreview.TabIndex = 6;
      this.btnPreview.Text = "Preview";
      this.btnPreview.Click += new EventHandler(this.btnPreview_Click);
      this.btnPrint.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnPrint.Location = new Point(340, 10);
      this.btnPrint.Name = "btnPrint";
      this.btnPrint.Size = new Size(75, 24);
      this.btnPrint.TabIndex = 7;
      this.btnPrint.Text = "Print";
      this.btnPrint.Click += new EventHandler(this.btnPrint_Click);
      this.btnSend.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSend.Location = new Point(258, 10);
      this.btnSend.Name = "btnSend";
      this.btnSend.Size = new Size(75, 24);
      this.btnSend.TabIndex = 8;
      this.btnSend.Text = "Send";
      this.btnSend.Click += new EventHandler(this.btnSend_Click);
      this.btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCancel.Location = new Point(504, 10);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 24);
      this.btnCancel.TabIndex = 9;
      this.btnCancel.Text = "Close";
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.fsExplorer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.fsExplorer1.HasPublicRight = true;
      this.fsExplorer1.Location = new Point(12, 3);
      this.fsExplorer1.Name = "fsExplorer1";
      this.fsExplorer1.setContactType = EllieMae.EMLite.ContactUI.ContactType.BizPartner;
      this.fsExplorer1.Size = new Size(567, 411);
      this.fsExplorer1.TabIndex = 12;
      this.fsExplorer1.Leave += new EventHandler(this.fsExplorer1_Leave);
      this.bottomPanel.Controls.Add((Control) this.emHelpLink1);
      this.bottomPanel.Controls.Add((Control) this.btnCancel);
      this.bottomPanel.Controls.Add((Control) this.btnSend);
      this.bottomPanel.Controls.Add((Control) this.btnPrint);
      this.bottomPanel.Controls.Add((Control) this.btnPreview);
      this.bottomPanel.Dock = DockStyle.Bottom;
      this.bottomPanel.Location = new Point(0, 414);
      this.bottomPanel.Name = "bottomPanel";
      this.bottomPanel.Size = new Size(592, 42);
      this.bottomPanel.TabIndex = 13;
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.HelpTag = "Mail Merge";
      this.emHelpLink1.Location = new Point(12, 10);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 10;
      this.topPanel.Controls.Add((Control) this.fsExplorer1);
      this.topPanel.Dock = DockStyle.Fill;
      this.topPanel.Location = new Point(0, 0);
      this.topPanel.Name = "topPanel";
      this.topPanel.Size = new Size(592, 414);
      this.topPanel.TabIndex = 14;
      this.Controls.Add((Control) this.topPanel);
      this.Controls.Add((Control) this.bottomPanel);
      this.Name = nameof (ContactLetterPanel);
      this.Size = new Size(592, 456);
      this.bottomPanel.ResumeLayout(false);
      this.topPanel.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void TriggerLetterSelectedEvent(bool bPrintPreview)
    {
      GVSelectedItemCollection selectedItems = this.fsExplorer1.SelectedItems;
      if (selectedItems.Count != 1)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a single letter to preview, print, or send.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        FileSystemEntry tag = (FileSystemEntry) selectedItems[0].Tag;
        if (tag.Type == FileSystemEntry.Types.Folder)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "Please select a letter to preview, print, or send.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          this._IsPrintPreview = bPrintPreview;
          this.OnLetterSelected(new LetterSelectedEventArg(tag, this._IsPrintPreview, this._Action));
        }
      }
    }

    private void btnPreview_Click(object sender, EventArgs e)
    {
      this._Action = "MailMerge";
      this.TriggerLetterSelectedEvent(true);
    }

    private void btnPrint_Click(object sender, EventArgs e)
    {
      this._Action = "MailMerge";
      this.TriggerLetterSelectedEvent(false);
    }

    private void btnSend_Click(object sender, EventArgs e)
    {
      this._Action = "EmailMerge";
      this.TriggerLetterSelectedEvent(false);
    }

    private void btnCancel_Click(object sender, EventArgs e) => ((Form) this.Parent).Close();

    private void setPreference()
    {
      string str = !this.fsExplorer1.IsTopFolder ? this.fsExplorer1.CurrentFolder.ToString() : string.Empty;
      string empty = string.Empty;
      string section = this._contactType != EllieMae.EMLite.ContactUI.ContactType.BizPartner ? "BorContactLetterPanel" : "BizContactLetterPanel";
      try
      {
        this.session.WritePrivateProfileString(section, "LastFolderViewed", str);
      }
      catch (Exception ex)
      {
      }
    }

    private void fsExplorer1_Leave(object sender, EventArgs e) => this.setPreference();

    public FileSystemEntry SelectedFolder
    {
      get => this.fsExplorer1.CurrentFolder;
      set => this.fsExplorer1.SetFolder(value);
    }

    public void SetSelectedItems(AclFileType type, string[] fileTypePathList)
    {
      this.fsExplorer1.MakeItemSelected(type, fileTypePathList);
    }

    public List<FileSystemEntry> SelectedFileSystemEntries
    {
      get
      {
        List<FileSystemEntry> fileSystemEntries = new List<FileSystemEntry>();
        foreach (GVItem selectedItem in this.fsExplorer1.SelectedItems)
          fileSystemEntries.Add((FileSystemEntry) selectedItem.Tag);
        return fileSystemEntries;
      }
    }

    public delegate void LetterSelectedEventHandler(LetterSelectedEventArg e);
  }
}
