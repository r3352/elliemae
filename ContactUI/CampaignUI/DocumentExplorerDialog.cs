// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CampaignUI.DocumentExplorerDialog
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Campaign;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.CustomLetters;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CampaignUI
{
  public class DocumentExplorerDialog : Form
  {
    private ActivityType _activityType;
    private EllieMae.EMLite.ContactUI.ContactType _contactType;
    private string _documentId = string.Empty;
    private ArrayList _contacts;
    private FileSystemEntry _fsEntry;
    private System.ComponentModel.Container components;
    private Panel pnlButtons;
    private Button btnCancel;
    private Button btnDoAction;
    private Button btnPreview;
    private Panel pnlExplorerControl;
    private Label lblSeparator;
    private FSExplorer fsExplorer1;

    public string DocumentId
    {
      get
      {
        if (this._fsEntry == null)
          return string.Empty;
        this._fsEntry.GetEncodedPath();
        return this.fsExplorer1.CurrentFolder.ToString() + this._fsEntry.Name;
      }
    }

    public DocumentExplorerDialog(
      ActivityType activityType,
      EllieMae.EMLite.ContactUI.ContactType contactType,
      string documentId,
      ArrayList contacts)
    {
      this.InitializeComponent();
      this._activityType = activityType;
      this._contactType = contactType;
      this._documentId = documentId == null ? string.Empty : documentId;
      this._contacts = contacts;
      if (this._contacts == null)
      {
        this.btnPreview.Visible = false;
        this.btnDoAction.Text = "Select";
        this.Text = "Select Document Template";
      }
      else
      {
        this.btnPreview.Visible = true;
        this.btnDoAction.Text = this._activityType == ActivityType.Email ? "Send" : "Print";
        this.Text = this.btnDoAction.Text + " " + new ActivityTypeNameProvider().GetName((object) this._activityType);
      }
      string uri1 = string.Empty;
      if (string.Empty != this._documentId && 0 < this._documentId.IndexOf("\\"))
        uri1 = documentId.Substring(0, documentId.LastIndexOf("\\") + 1);
      string uri2 = Session.GetPrivateProfileString(this._contactType == EllieMae.EMLite.ContactUI.ContactType.Borrower ? "BorContactLetterPanel" : "BizContactLetterPanel", "LastFolderViewed") ?? "";
      CustomLetterIFSExplorer ifsExplorer = new CustomLetterIFSExplorer(Session.DefaultInstance, this._contactType);
      this.fsExplorer1.FileType = FSExplorer.FileTypes.CustomLetters;
      if (!((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).CheckPermission(AclFeature.SettingsTab_Personal_CustomPrintForms, Session.UserInfo))
        this.fsExplorer1.HasPublicRight = false;
      FileSystemEntry publicRoot = FileSystemEntry.PublicRoot;
      try
      {
        if (string.Empty != uri1)
        {
          publicRoot = FileSystemEntry.Parse(uri1);
          if (!ifsExplorer.EntryExists(publicRoot))
          {
            publicRoot = FileSystemEntry.Parse(uri2);
            if (!ifsExplorer.EntryExists(publicRoot))
              publicRoot = FileSystemEntry.PublicRoot;
          }
        }
        else
        {
          publicRoot = FileSystemEntry.Parse(uri2);
          if (!ifsExplorer.EntryExists(publicRoot))
            publicRoot = FileSystemEntry.PublicRoot;
        }
      }
      catch
      {
      }
      this.fsExplorer1.SetProperties(false, true, true, false);
      this.fsExplorer1.HideAllButtons = true;
      this.fsExplorer1.Init((IFSExplorerBase) ifsExplorer, publicRoot);
      this.fsExplorer1.SelectedCurrentFile += new EventHandler(this.fsExplorer1_SelectedCurrentFile);
    }

    private bool processUserEntry()
    {
      GVSelectedItemCollection selectedItems = this.fsExplorer1.SelectedItems;
      if (selectedItems.Count != 1 || FileSystemEntry.Types.File != ((FileSystemEntry) selectedItems[0].Tag).Type)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a single document.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      this._fsEntry = (FileSystemEntry) selectedItems[0].Tag;
      this.setPreference();
      return true;
    }

    private void setPreference()
    {
      string str = this.fsExplorer1.IsTopFolder ? string.Empty : this.fsExplorer1.CurrentFolder.ToString();
      string section = this._contactType == EllieMae.EMLite.ContactUI.ContactType.Borrower ? "BorContactLetterPanel" : "BizContactLetterPanel";
      try
      {
        Session.WritePrivateProfileString(section, "LastFolderViewed", str);
      }
      catch (Exception ex)
      {
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void fsExplorer1_SelectedCurrentFile(object sender, EventArgs e)
    {
      this.btnDoAction_Click(sender, e);
    }

    private void btnPreview_Click(object sender, EventArgs e)
    {
      throw new NotImplementedException("DocumentExplorerDialog: 'Preview activity' not implemented");
    }

    private void btnDoAction_Click(object sender, EventArgs e)
    {
      if (!this.processUserEntry())
        return;
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void InitializeComponent()
    {
      this.pnlButtons = new Panel();
      this.lblSeparator = new Label();
      this.btnPreview = new Button();
      this.btnDoAction = new Button();
      this.btnCancel = new Button();
      this.pnlExplorerControl = new Panel();
      this.fsExplorer1 = new FSExplorer();
      this.pnlButtons.SuspendLayout();
      this.pnlExplorerControl.SuspendLayout();
      this.SuspendLayout();
      this.pnlButtons.Controls.Add((Control) this.lblSeparator);
      this.pnlButtons.Controls.Add((Control) this.btnPreview);
      this.pnlButtons.Controls.Add((Control) this.btnDoAction);
      this.pnlButtons.Controls.Add((Control) this.btnCancel);
      this.pnlButtons.Dock = DockStyle.Bottom;
      this.pnlButtons.Location = new Point(0, 500);
      this.pnlButtons.Name = "pnlButtons";
      this.pnlButtons.Size = new Size(574, 39);
      this.pnlButtons.TabIndex = 0;
      this.lblSeparator.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblSeparator.BorderStyle = BorderStyle.FixedSingle;
      this.lblSeparator.Location = new Point(7, 5);
      this.lblSeparator.Name = "lblSeparator";
      this.lblSeparator.Size = new Size(560, 1);
      this.lblSeparator.TabIndex = 3;
      this.btnPreview.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnPreview.Location = new Point(325, 9);
      this.btnPreview.Name = "btnPreview";
      this.btnPreview.Size = new Size(78, 23);
      this.btnPreview.TabIndex = 2;
      this.btnPreview.Text = "Preview";
      this.btnPreview.Click += new EventHandler(this.btnPreview_Click);
      this.btnDoAction.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnDoAction.Location = new Point(407, 9);
      this.btnDoAction.Name = "btnDoAction";
      this.btnDoAction.Size = new Size(78, 23);
      this.btnDoAction.TabIndex = 1;
      this.btnDoAction.Text = "Do Action";
      this.btnDoAction.Click += new EventHandler(this.btnDoAction_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(489, 9);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(78, 23);
      this.btnCancel.TabIndex = 0;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.pnlExplorerControl.Controls.Add((Control) this.fsExplorer1);
      this.pnlExplorerControl.Dock = DockStyle.Fill;
      this.pnlExplorerControl.Location = new Point(0, 0);
      this.pnlExplorerControl.Name = "pnlExplorerControl";
      this.pnlExplorerControl.Size = new Size(574, 500);
      this.pnlExplorerControl.TabIndex = 1;
      this.fsExplorer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.fsExplorer1.HasPublicRight = true;
      this.fsExplorer1.Location = new Point(7, 7);
      this.fsExplorer1.Name = "fsExplorer1";
      this.fsExplorer1.Size = new Size(560, 496);
      this.fsExplorer1.TabIndex = 0;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(574, 539);
      this.Controls.Add((Control) this.pnlExplorerControl);
      this.Controls.Add((Control) this.pnlButtons);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (DocumentExplorerDialog);
      this.ShowInTaskbar = false;
      this.Text = "Document Explorer";
      this.pnlButtons.ResumeLayout(false);
      this.pnlExplorerControl.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
