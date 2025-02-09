// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CampaignUI.CampaignExplorerDialog
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Campaign;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.ContactUI.CampaignUI.CampaignWizard;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CampaignUI
{
  public class CampaignExplorerDialog : Form
  {
    private FSExplorer fsExplorer;
    private CampaignIFSExplorer ifsExplorer;
    private FSExplorer.DialogMode dialogMode;
    private CampaignData campaignData;
    private EllieMae.EMLite.Campaign.Campaign campaign;
    private IContainer components;
    private Panel pnlExplorer;
    private Panel pnlButtons;
    private Label label1;
    private Button btnAction;
    private Button btnCancel;
    private Panel pnlTemplateName;
    private TextBox txtTemplateName;
    private TextBox txtTemplateDescription;
    private Label lblTemplateDescription;
    private Label lblTemplateName;
    private EMHelpLink emHelpLink1;

    public EllieMae.EMLite.Campaign.Campaign Campaign
    {
      get => this.campaign;
      set => this.campaign = value;
    }

    public FileSystemEntry CurrentFolder
    {
      get => this.fsExplorer.CurrentFolder;
      set => this.fsExplorer.SetFolder(value);
    }

    public CampaignExplorerDialog(FSExplorer.DialogMode dialogMode)
    {
      this.dialogMode = dialogMode == FSExplorer.DialogMode.Unspecified ? FSExplorer.DialogMode.ManageFiles : dialogMode;
      this.InitializeComponent();
      this.createFSExplorerControl();
      this.campaignData = CampaignData.GetCampaignData();
      if (FSExplorer.DialogMode.SelectFiles == dialogMode)
      {
        this.Text = "Select Campaign Template";
        this.btnAction.Text = "Select";
        this.pnlTemplateName.Visible = false;
      }
      else if (FSExplorer.DialogMode.SaveFiles == dialogMode)
      {
        this.Text = "Save Campaign Template";
        this.btnAction.Text = "Save";
      }
      else
      {
        this.Text = "Manage Campaign Templates";
        this.btnAction.Visible = false;
        this.btnCancel.Text = "Close";
        this.pnlTemplateName.Visible = false;
      }
      this.fsExplorer.SetCampaignTemplateProperties(dialogMode);
      this.ifsExplorer = new CampaignIFSExplorer(this.fsExplorer);
      FileSystemEntry defaultFolder = FileSystemEntry.PublicRoot;
      bool flag1 = this.hasPrivateCampaignTemplateRight();
      bool flag2 = this.hasPublicCampaignTemplateRight();
      if (flag1)
        defaultFolder = FileSystemEntry.PrivateRoot(Session.UserID);
      string privateProfileString = Session.GetPrivateProfileString("CampaignTemplate", "LastCampaignTemplateFolder");
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
                  goto label_14;
              }
              defaultFolder = entry;
            }
          }
          catch (Exception ex)
          {
          }
        }
      }
label_14:
      this.fsExplorer.HasPublicRight = flag2;
      this.fsExplorer.InitCampaignTemplate((IFSExplorerBase) this.ifsExplorer, defaultFolder);
      this.fsExplorer.SelectedCurrentFile += new EventHandler(this.fsExplorer_SelectedCurrentFile);
      this.fsExplorer.SelectedEntryChanged += new EventHandler(this.fsExplorer_SelectedEntryChanged);
      this.fsExplorer.FolderChanged += new EventHandler(this.fsExplorer_FolderChanged);
      this.ifsExplorer.CreateNewEvent += new CampaignIFSExplorer.CreateNewEventHandler(this.ifsExplorer_CreateNewEvent);
      this.ifsExplorer.OpenFileEvent += new CampaignIFSExplorer.OpenFileEventHandler(this.ifsExplorer_OpenFileEvent);
      this.ifsExplorer.DeployEvent += new CampaignIFSExplorer.DeployEventHandler(this.ifsExplorer_DeployEvent);
      this.ifsExplorer.ImportEvent += new CampaignIFSExplorer.ImportEventHandler(this.ifsExplorer_ImportEvent);
      this.ifsExplorer.ExportEvent += new CampaignIFSExplorer.ExportEventHandler(this.ifsExplorer_ExportEvent);
      this.setActionButton();
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

    private void setActionButton()
    {
      bool flag = false;
      if (this.fsExplorer != null)
      {
        if (FSExplorer.DialogMode.SaveFiles == this.dialogMode)
        {
          if (this.fsExplorer.CurrentFolder != null && (!this.fsExplorer.CurrentFolder.IsPublic || this.fsExplorer.CurrentFolder.Access == AclResourceAccess.ReadWrite))
            flag = true;
        }
        else if (FSExplorer.DialogMode.SelectFiles == this.dialogMode)
        {
          GVSelectedItemCollection selectedItems = this.fsExplorer.SelectedItems;
          if (selectedItems.Count == 1 && FileSystemEntry.Types.File == ((FileSystemEntry) selectedItems[0].Tag).Type)
            flag = true;
        }
      }
      this.btnAction.Enabled = flag;
    }

    private bool hasPublicCampaignTemplateRight()
    {
      return UserInfo.IsSuperAdministrator(Session.UserID, Session.UserInfo.UserPersonas) || Session.AclGroupManager.CheckPublicAccessPermission(AclFileType.CampaignTemplate);
    }

    private bool hasPrivateCampaignTemplateRight()
    {
      return ((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.Cnt_Campaign_PersonalTemplates);
    }

    private void processUserEntry()
    {
      if (FSExplorer.DialogMode.SaveFiles == this.dialogMode)
      {
        FileSystemEntry fileSystemEntry = this.fsExplorer.CurrentFolder;
        if (fileSystemEntry != null)
        {
          GVSelectedItemCollection selectedItems = this.fsExplorer.SelectedItems;
          if (1 == selectedItems.Count && FileSystemEntry.Types.Folder == ((FileSystemEntry) selectedItems[0].Tag).Type)
            fileSystemEntry = (FileSystemEntry) selectedItems[0].Tag;
        }
        if (fileSystemEntry == null)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Please select a folder.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return;
        }
        string entryName = this.txtTemplateName.Text.Trim();
        if (string.Empty == entryName)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Please specify a template name.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return;
        }
        if (!this.createCampaignTemplate(new FileSystemEntry(fileSystemEntry.Path, entryName, FileSystemEntry.Types.File, fileSystemEntry.IsPublic ? (string) null : Session.UserID)))
          return;
      }
      else if (FSExplorer.DialogMode.SelectFiles == this.dialogMode)
      {
        GVSelectedItemCollection selectedItems = this.fsExplorer.SelectedItems;
        if (selectedItems.Count != 1 || FileSystemEntry.Types.File != ((FileSystemEntry) selectedItems[0].Tag).Type)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Please select a file.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return;
        }
        this.campaign = this.createCampaign((FileSystemEntry) selectedItems[0].Tag);
        if (this.campaign == null)
          return;
      }
      try
      {
        Session.WritePrivateProfileString("CampaignTemplate", "LastFolderViewed", this.fsExplorer.IsTopFolder ? string.Empty : this.fsExplorer.CurrentFolder.ToString());
      }
      catch (Exception ex)
      {
      }
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private bool createCampaignTemplate(FileSystemEntry fsFile)
    {
      try
      {
        CampaignTemplate campaignTemplate = this.campaign.GetCampaignTemplate();
        campaignTemplate.TemplateName = this.txtTemplateName.Text.Trim();
        campaignTemplate.Description = this.txtTemplateDescription.Text.Trim();
        return this.ifsExplorer.SaveCampaignTemplate(fsFile, campaignTemplate);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The selected campaign could not be processed.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
    }

    private EllieMae.EMLite.Campaign.Campaign createCampaign(FileSystemEntry fsFile)
    {
      EllieMae.EMLite.Campaign.Campaign campaign;
      try
      {
        campaign = EllieMae.EMLite.Campaign.Campaign.NewCampaign(this.ifsExplorer.LoadCampaignTemplate(fsFile), Session.SessionObjects);
      }
      catch (Exception ex)
      {
        campaign = (EllieMae.EMLite.Campaign.Campaign) null;
      }
      if (campaign == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The selected campaign template could not be processed.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      return campaign;
    }

    private void deployCampaign(EllieMae.EMLite.Campaign.Campaign newCampaign, string userId, bool startCampaign)
    {
      while (true)
      {
        try
        {
          newCampaign = newCampaign.Save(userId);
          if (startCampaign)
            newCampaign.Start();
          int num = (int) Utils.Dialog((IWin32Window) this, "The campaign has been deployed for user '" + userId + "'.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          break;
        }
        catch (Exception ex)
        {
          if (0 <= ex.Message.IndexOf("Violation of UNIQUE KEY constraint 'UK_Campaign_UserId_CampaignName'"))
          {
            DuplicateCampaignNameDialog campaignNameDialog = new DuplicateCampaignNameDialog();
            campaignNameDialog.CampaignName = newCampaign.CampaignName;
            UserInfo user = Session.OrganizationManager.GetUser(userId);
            if ((UserInfo) null != user)
              campaignNameDialog.UserName = user.FullName;
            if (DialogResult.Cancel == campaignNameDialog.ShowDialog())
              break;
            newCampaign.CampaignName = campaignNameDialog.CampaignName;
          }
          else
            throw;
        }
      }
    }

    private void CampaignExplorerDialog_Load(object sender, EventArgs e)
    {
      if (FSExplorer.DialogMode.SaveFiles != this.dialogMode || this.campaign == null)
        return;
      this.txtTemplateName.Text = this.campaign.CampaignName;
      this.txtTemplateDescription.Text = this.campaign.CampaignDesc;
    }

    private void fsExplorer_SelectedCurrentFile(object sender, EventArgs e)
    {
      this.processUserEntry();
    }

    private void fsExplorer_SelectedEntryChanged(object sender, EventArgs e)
    {
      this.setActionButton();
    }

    private void fsExplorer_FolderChanged(object sender, EventArgs e) => this.setActionButton();

    private void ifsExplorer_CreateNewEvent(object sender, SelectedFileEventArgs e)
    {
      int num = (int) new CampaignWizardForm(true, e.FSEntry).ShowDialog((IWin32Window) this.ParentForm);
      FileSystemEntry templateTargetEntry = this.campaignData.TemplateTargetEntry;
      if (templateTargetEntry == null)
        return;
      this.fsExplorer.SetFolder(templateTargetEntry);
    }

    private void ifsExplorer_OpenFileEvent(object sender, SelectedFileEventArgs e)
    {
      if (FSExplorer.DialogMode.ManageFiles != this.dialogMode)
        return;
      EllieMae.EMLite.Campaign.Campaign campaign = this.createCampaign(e.FSEntry);
      if (campaign == null)
        return;
      int num = (int) new CampaignWizardForm(campaign, e.FSEntry).ShowDialog((IWin32Window) this.ParentForm);
      FileSystemEntry templateTargetEntry = this.campaignData.TemplateTargetEntry;
      if (templateTargetEntry == null)
        return;
      this.fsExplorer.SetFolder(templateTargetEntry);
    }

    private void ifsExplorer_DeployEvent(object sender, SelectedFileEventArgs e)
    {
      DeployUserSelectionDialog userSelectionDialog = new DeployUserSelectionDialog();
      if (DialogResult.OK != userSelectionDialog.ShowDialog())
        return;
      Cursor.Current = Cursors.WaitCursor;
      foreach (string selectedUserId in userSelectionDialog.SelectedUserIds)
      {
        EllieMae.EMLite.Campaign.Campaign campaign = this.createCampaign(e.FSEntry);
        if (campaign != null)
          this.deployCampaign(campaign, selectedUserId, true);
      }
      Cursor.Current = Cursors.Default;
      this.btnCancel.PerformClick();
    }

    private void ifsExplorer_ImportEvent(object sender, SelectedFileEventArgs e)
    {
      new ImportCampaignDialog(this.ifsExplorer, e.FSEntry).ShowDialog();
    }

    private void ifsExplorer_ExportEvent(object sender, SelectedFileListEventArgs e)
    {
      new ExportCampaignDialog(this.ifsExplorer, e.FSEntryList).ShowDialog();
    }

    private void btnAction_Click(object sender, EventArgs e) => this.processUserEntry();

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
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
      this.emHelpLink1 = new EMHelpLink();
      this.btnAction = new Button();
      this.btnCancel = new Button();
      this.label1 = new Label();
      this.pnlTemplateName = new Panel();
      this.txtTemplateName = new TextBox();
      this.txtTemplateDescription = new TextBox();
      this.lblTemplateDescription = new Label();
      this.lblTemplateName = new Label();
      this.pnlButtons.SuspendLayout();
      this.pnlTemplateName.SuspendLayout();
      this.SuspendLayout();
      this.pnlExplorer.Dock = DockStyle.Fill;
      this.pnlExplorer.Location = new Point(0, 0);
      this.pnlExplorer.Name = "pnlExplorer";
      this.pnlExplorer.Size = new Size(622, 447);
      this.pnlExplorer.TabIndex = 0;
      this.pnlButtons.Controls.Add((Control) this.emHelpLink1);
      this.pnlButtons.Controls.Add((Control) this.btnAction);
      this.pnlButtons.Controls.Add((Control) this.btnCancel);
      this.pnlButtons.Controls.Add((Control) this.label1);
      this.pnlButtons.Dock = DockStyle.Bottom;
      this.pnlButtons.Location = new Point(0, 500);
      this.pnlButtons.Name = "pnlButtons";
      this.pnlButtons.Size = new Size(622, 39);
      this.pnlButtons.TabIndex = 1;
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.HelpTag = "Campaign Explorer";
      this.emHelpLink1.Location = new Point(7, 13);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 4;
      this.btnAction.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnAction.Location = new Point(450, 9);
      this.btnAction.Name = "btnAction";
      this.btnAction.Size = new Size(78, 24);
      this.btnAction.TabIndex = 3;
      this.btnAction.Text = "Action";
      this.btnAction.Click += new EventHandler(this.btnAction_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(537, 9);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(78, 24);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.label1.Location = new Point(7, 5);
      this.label1.Name = "label1";
      this.label1.Size = new Size(560, 1);
      this.label1.TabIndex = 0;
      this.label1.Text = "label1";
      this.pnlTemplateName.Controls.Add((Control) this.txtTemplateName);
      this.pnlTemplateName.Controls.Add((Control) this.txtTemplateDescription);
      this.pnlTemplateName.Controls.Add((Control) this.lblTemplateDescription);
      this.pnlTemplateName.Controls.Add((Control) this.lblTemplateName);
      this.pnlTemplateName.Dock = DockStyle.Bottom;
      this.pnlTemplateName.Location = new Point(0, 447);
      this.pnlTemplateName.Name = "pnlTemplateName";
      this.pnlTemplateName.Size = new Size(622, 53);
      this.pnlTemplateName.TabIndex = 2;
      this.txtTemplateName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtTemplateName.Location = new Point(173, 7);
      this.txtTemplateName.Name = "txtTemplateName";
      this.txtTemplateName.Size = new Size(355, 20);
      this.txtTemplateName.TabIndex = 3;
      this.txtTemplateDescription.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtTemplateDescription.Location = new Point(173, 33);
      this.txtTemplateDescription.Name = "txtTemplateDescription";
      this.txtTemplateDescription.Size = new Size(355, 20);
      this.txtTemplateDescription.TabIndex = 2;
      this.lblTemplateDescription.AutoSize = true;
      this.lblTemplateDescription.Location = new Point(7, 37);
      this.lblTemplateDescription.Name = "lblTemplateDescription";
      this.lblTemplateDescription.Size = new Size(160, 13);
      this.lblTemplateDescription.TabIndex = 1;
      this.lblTemplateDescription.Text = "Campaign Template Description:";
      this.lblTemplateDescription.TextAlign = ContentAlignment.MiddleLeft;
      this.lblTemplateName.AutoSize = true;
      this.lblTemplateName.Location = new Point(7, 11);
      this.lblTemplateName.Name = "lblTemplateName";
      this.lblTemplateName.Size = new Size(135, 13);
      this.lblTemplateName.TabIndex = 0;
      this.lblTemplateName.Text = "Campaign Template Name:";
      this.lblTemplateName.TextAlign = ContentAlignment.MiddleLeft;
      this.AcceptButton = (IButtonControl) this.btnAction;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(622, 539);
      this.Controls.Add((Control) this.pnlExplorer);
      this.Controls.Add((Control) this.pnlTemplateName);
      this.Controls.Add((Control) this.pnlButtons);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (CampaignExplorerDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Campaign Explorer";
      this.Load += new EventHandler(this.CampaignExplorerDialog_Load);
      this.pnlButtons.ResumeLayout(false);
      this.pnlTemplateName.ResumeLayout(false);
      this.pnlTemplateName.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
