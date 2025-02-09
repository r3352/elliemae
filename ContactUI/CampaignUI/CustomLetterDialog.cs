// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CampaignUI.CustomLetterDialog
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.CustomLetters;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CampaignUI
{
  public class CustomLetterDialog : Form
  {
    private ContactType contactType;
    private FSExplorer fsExplorer;
    private CustomLetterIFSExplorer ifsExplorer;
    private FSExplorer.DialogMode dialogMode = FSExplorer.DialogMode.SaveFiles;
    private IContainer components;
    private Panel pnlExplorer;
    private Panel pnlButtons;
    private Button btnSelect;
    private Button btnCancel;
    private Label label1;

    public FileSystemEntry SelectedFolder => this.fsExplorer.CurrentFolder;

    public CustomLetterDialog(ContactType contactType)
    {
      this.contactType = contactType;
      this.InitializeComponent();
      this.createFSExplorerControl();
      this.fsExplorer.SetCustomLetterProperties(this.dialogMode);
      this.ifsExplorer = new CustomLetterIFSExplorer(Session.DefaultInstance, contactType);
      FileSystemEntry defaultFolder = (FileSystemEntry) null;
      bool flag1 = this.hasPrivateCustomLetterRight(contactType);
      bool flag2 = this.hasPublicCustomLetterRight();
      if (flag1)
        defaultFolder = FileSystemEntry.PrivateRoot(Session.UserID);
      else if (flag2)
        defaultFolder = FileSystemEntry.PublicRoot;
      string privateProfileString = Session.GetPrivateProfileString("CampaignTemplate", "LastCustomLetterFolder");
      if (privateProfileString != null)
      {
        if (string.Empty != privateProfileString)
        {
          try
          {
            FileSystemEntry entry = FileSystemEntry.Parse(privateProfileString);
            if (this.ifsExplorer.EntryExists(entry))
            {
              if (!(entry.IsPublic & flag2))
              {
                if (!(!entry.IsPublic & flag1))
                  goto label_11;
              }
              defaultFolder = entry;
            }
          }
          catch (Exception ex)
          {
          }
        }
      }
label_11:
      this.fsExplorer.HasPublicRight = flag2;
      this.fsExplorer.HideDescription = true;
      this.fsExplorer.Init((IFSExplorerBase) this.ifsExplorer, defaultFolder, !flag1);
      this.fsExplorer.FolderChanged += new EventHandler(this.fsExplorer_FolderChanged);
      this.setActionButton();
    }

    public FileSystemEntry Import(string sourceFilePath, FileSystemEntry fsTargetFile)
    {
      return this.ifsExplorer.ImportForm(sourceFilePath, fsTargetFile);
    }

    private void createFSExplorerControl()
    {
      int num = 7;
      this.fsExplorer = new FSExplorer();
      this.fsExplorer.Name = "fsExplorer";
      this.pnlExplorer.Controls.Add((Control) this.fsExplorer);
      this.fsExplorer.Size = new Size(this.pnlExplorer.Width - 2 * num, this.pnlExplorer.Height - 2 * num);
      this.fsExplorer.Location = new Point(num, num);
      this.fsExplorer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.fsExplorer.TabIndex = 1;
    }

    private bool hasPublicCustomLetterRight()
    {
      return UserInfo.IsSuperAdministrator(Session.UserID, Session.UserInfo.UserPersonas) || Session.AclGroupManager.CheckPublicAccessPermission(AclFileType.CustomPrintForms);
    }

    private bool hasPrivateCustomLetterRight(ContactType cType)
    {
      AclFeature feature;
      if (cType != ContactType.Borrower)
      {
        if (cType != ContactType.BizPartner)
          throw new Exception("Invalid contact type.");
        feature = AclFeature.Business_Contacts_Personal_CustomLetters;
      }
      else
        feature = AclFeature.Borrower_Contacts_Personal_CustomLetters;
      return ((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(feature);
    }

    private void setActionButton()
    {
      this.btnSelect.Enabled = this.SelectedFolder != null && (!this.SelectedFolder.IsPublic || this.hasPublicCustomLetterRight()) && (this.SelectedFolder.IsPublic || this.hasPrivateCustomLetterRight(this.contactType));
    }

    private void fsExplorer_FolderChanged(object sender, EventArgs e) => this.setActionButton();

    private void btnSelect_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pnlExplorer = new Panel();
      this.pnlButtons = new Panel();
      this.btnSelect = new Button();
      this.btnCancel = new Button();
      this.label1 = new Label();
      this.pnlButtons.SuspendLayout();
      this.SuspendLayout();
      this.pnlExplorer.Dock = DockStyle.Fill;
      this.pnlExplorer.Location = new Point(0, 0);
      this.pnlExplorer.Name = "pnlExplorer";
      this.pnlExplorer.Size = new Size(538, 344);
      this.pnlExplorer.TabIndex = 0;
      this.pnlButtons.Controls.Add((Control) this.btnSelect);
      this.pnlButtons.Controls.Add((Control) this.btnCancel);
      this.pnlButtons.Controls.Add((Control) this.label1);
      this.pnlButtons.Dock = DockStyle.Bottom;
      this.pnlButtons.Location = new Point(0, 305);
      this.pnlButtons.Name = "pnlButtons";
      this.pnlButtons.Size = new Size(538, 39);
      this.pnlButtons.TabIndex = 2;
      this.btnSelect.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSelect.Enabled = false;
      this.btnSelect.Location = new Point(366, 9);
      this.btnSelect.Name = "btnSelect";
      this.btnSelect.Size = new Size(78, 24);
      this.btnSelect.TabIndex = 3;
      this.btnSelect.Text = "Select";
      this.btnSelect.Click += new EventHandler(this.btnSelect_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(453, 9);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(78, 24);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.label1.Location = new Point(7, 5);
      this.label1.Name = "label1";
      this.label1.Size = new Size(560, 1);
      this.label1.TabIndex = 0;
      this.label1.Text = "label1";
      this.AcceptButton = (IButtonControl) this.btnSelect;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(538, 344);
      this.Controls.Add((Control) this.pnlButtons);
      this.Controls.Add((Control) this.pnlExplorer);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Name = nameof (CustomLetterDialog);
      this.Text = "Select Custom Letter Folder";
      this.pnlButtons.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
