// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LoanTemplateSelectDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class LoanTemplateSelectDialog : Form, IHelp
  {
    private FSExplorer tempExplorer;
    private GradientPanel gradPnlDefault;
    private ListView lvwDefault;
    private Button cancelBtn;
    private ImageList imageListSmall;
    private Button btnDefault;
    private Button btnDetail;
    private Button btnSelect;
    private Button btnNew;
    private IContainer components;
    private const string publicBase = "\\\\Public Loan Template";
    private Button btnGo;
    private const string privateBase = "\\\\Personal Loan Template";
    private CheckBox checkBoxAppend;
    private bool insideLoan;
    private bool allowBlankLoan;
    private EMHelpLink emHelpLink1;
    private Label label1;
    private Label lblDefault;
    private bool allowTemplate;
    private Sessions.Session session;
    private bool forLoanImportRequirement;
    private FileSystemEntry selectedItem;

    private void doSecurityCheck()
    {
      if (this.insideLoan)
        return;
      this.btnNew.Enabled = this.allowBlankLoan;
      this.btnSelect.Enabled = this.allowTemplate;
      this.btnDefault.Enabled = this.allowTemplate;
    }

    public LoanTemplateSelectDialog(
      Sessions.Session session,
      bool insideLoan,
      bool allowBlankLoan,
      bool allowTemplate)
      : this(session, insideLoan, allowBlankLoan, allowTemplate, false)
    {
    }

    public LoanTemplateSelectDialog(
      Sessions.Session session,
      bool insideLoan,
      bool allowBlankLoan,
      bool allowTemplate,
      bool forLoanImportRequirement)
    {
      this.session = session;
      this.allowBlankLoan = allowBlankLoan;
      this.allowTemplate = allowTemplate;
      this.forLoanImportRequirement = forLoanImportRequirement;
      this.insideLoan = insideLoan;
      this.InitializeComponent();
      this.doSecurityCheck();
      if (this.insideLoan)
      {
        this.btnNew.Visible = false;
        this.btnDefault.Left = this.btnSelect.Left;
        this.btnSelect.Left = this.btnNew.Left;
        this.emHelpLink1.HelpTag = "Apply Loan Template";
        this.Text = "Apply Loan Template";
      }
      this.getCurrentDefaultTemplate();
      TemplateIFSExplorer ifsExplorer = new TemplateIFSExplorer(Session.DefaultInstance, EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanTemplate, false);
      ifsExplorer.SelectedCurrentFile += new EventHandler(this.tempExplorer_SelectedCurrentFile);
      this.tempExplorer.FileType = FSExplorer.FileTypes.LoanTemplates;
      this.tempExplorer.HideAllButtons = true;
      this.tempExplorer.SingleSelection = true;
      this.tempExplorer.SetProperties(false, false, true, 6, true);
      FileSystemEntry fileSystemEntry;
      try
      {
        fileSystemEntry = FileSystemEntry.Parse(this.getPreference("LoanTemplate", "LastFolderViewed"));
        if (!ifsExplorer.EntryExists(fileSystemEntry))
          fileSystemEntry = (FileSystemEntry) null;
      }
      catch
      {
        fileSystemEntry = (FileSystemEntry) null;
      }
      if (this.forLoanImportRequirement && fileSystemEntry != null && !fileSystemEntry.IsPublic)
        fileSystemEntry = (FileSystemEntry) null;
      if (fileSystemEntry == null)
        fileSystemEntry = FileSystemEntry.PublicRoot;
      this.tempExplorer.Init((IFSExplorerBase) ifsExplorer, fileSystemEntry, this.forLoanImportRequirement || !this.getPersonalRight());
      this.checkBoxAppend.Checked = (bool) this.session.StartupInfo.PolicySettings[(object) "Policies.AppendNewLoanTemplate"];
      if (!this.forLoanImportRequirement)
        return;
      this.checkBoxAppend.Visible = false;
      this.label1.Visible = false;
      this.Text = "Loan Import Requirements";
      if (this.lvwDefault.Items.Count <= 0)
        return;
      string tag = (string) this.lvwDefault.Items[0].Tag;
      this.btnGo.Enabled = this.btnDefault.Enabled = tag != string.Empty && !tag.ToLower().StartsWith("personal");
    }

    private bool getPersonalRight()
    {
      return UserInfo.IsSuperAdministrator(this.session.UserID, this.session.UserInfo.UserPersonas) || ((FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.SettingsTab_Personal_LoanTemplateSets);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public FileSystemEntry SelectedItem => this.selectedItem;

    public bool AppendData => this.checkBoxAppend.Checked;

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (LoanTemplateSelectDialog));
      this.tempExplorer = new FSExplorer();
      this.gradPnlDefault = new GradientPanel();
      this.lblDefault = new Label();
      this.btnGo = new Button();
      this.lvwDefault = new ListView();
      this.imageListSmall = new ImageList(this.components);
      this.btnDefault = new Button();
      this.cancelBtn = new Button();
      this.btnDetail = new Button();
      this.btnSelect = new Button();
      this.btnNew = new Button();
      this.checkBoxAppend = new CheckBox();
      this.emHelpLink1 = new EMHelpLink();
      this.label1 = new Label();
      this.gradPnlDefault.SuspendLayout();
      this.SuspendLayout();
      this.tempExplorer.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.tempExplorer.FolderComboSelectedIndex = -1;
      this.tempExplorer.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.tempExplorer.HasPublicRight = true;
      this.tempExplorer.Location = new Point(17, 44);
      this.tempExplorer.Name = "tempExplorer";
      this.tempExplorer.RenameButtonSize = new Size(62, 22);
      this.tempExplorer.RESPAMode = FSExplorer.RESPAFilter.All;
      this.tempExplorer.setContactType = EllieMae.EMLite.ContactUI.ContactType.BizPartner;
      this.tempExplorer.Size = new Size(568, 370);
      this.tempExplorer.TabIndex = 2;
      this.tempExplorer.SelectedCurrentFile += new EventHandler(this.tempExplorer_SelectedCurrentFile);
      this.gradPnlDefault.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gradPnlDefault.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gradPnlDefault.Controls.Add((Control) this.lblDefault);
      this.gradPnlDefault.Controls.Add((Control) this.btnGo);
      this.gradPnlDefault.Controls.Add((Control) this.lvwDefault);
      this.gradPnlDefault.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradPnlDefault.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradPnlDefault.Location = new Point(17, 12);
      this.gradPnlDefault.Name = "gradPnlDefault";
      this.gradPnlDefault.Size = new Size(568, 32);
      this.gradPnlDefault.TabIndex = 34;
      this.lblDefault.AutoSize = true;
      this.lblDefault.BackColor = Color.Transparent;
      this.lblDefault.Location = new Point(4, 11);
      this.lblDefault.Name = "lblDefault";
      this.lblDefault.Size = new Size(41, 13);
      this.lblDefault.TabIndex = 2;
      this.lblDefault.Text = "Default";
      this.btnGo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnGo.Location = new Point(502, 5);
      this.btnGo.Name = "btnGo";
      this.btnGo.Size = new Size(56, 24);
      this.btnGo.TabIndex = 1;
      this.btnGo.Text = "&Go to";
      this.btnGo.Click += new EventHandler(this.btnGo_Click);
      this.lvwDefault.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lvwDefault.BackColor = SystemColors.Window;
      this.lvwDefault.Location = new Point(45, 7);
      this.lvwDefault.Name = "lvwDefault";
      this.lvwDefault.Size = new Size(449, 20);
      this.lvwDefault.SmallImageList = this.imageListSmall;
      this.lvwDefault.TabIndex = 0;
      this.lvwDefault.UseCompatibleStateImageBehavior = false;
      this.lvwDefault.View = View.List;
      this.imageListSmall.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageListSmall.ImageStream");
      this.imageListSmall.TransparentColor = Color.Transparent;
      this.imageListSmall.Images.SetKeyName(0, "");
      this.imageListSmall.Images.SetKeyName(1, "");
      this.imageListSmall.Images.SetKeyName(2, "");
      this.imageListSmall.Images.SetKeyName(3, "");
      this.imageListSmall.Images.SetKeyName(4, "template.bmp");
      this.btnDefault.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDefault.Location = new Point(72, 458);
      this.btnDefault.Name = "btnDefault";
      this.btnDefault.Size = new Size(112, 24);
      this.btnDefault.TabIndex = 3;
      this.btnDefault.Text = "Default Template";
      this.btnDefault.Click += new EventHandler(this.btnDefault_Click);
      this.cancelBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(504, 458);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(80, 24);
      this.cancelBtn.TabIndex = 7;
      this.cancelBtn.Text = "Cancel";
      this.btnDetail.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDetail.Location = new Point(424, 458);
      this.btnDetail.Name = "btnDetail";
      this.btnDetail.Size = new Size(75, 24);
      this.btnDetail.TabIndex = 6;
      this.btnDetail.Text = "Detail";
      this.btnDetail.Click += new EventHandler(this.btnDetail_Click);
      this.btnSelect.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSelect.Location = new Point(190, 458);
      this.btnSelect.Name = "btnSelect";
      this.btnSelect.Size = new Size(112, 24);
      this.btnSelect.TabIndex = 4;
      this.btnSelect.Text = "Select Template";
      this.btnSelect.Click += new EventHandler(this.btnSelect_Click);
      this.btnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnNew.Location = new Point(307, 458);
      this.btnNew.Name = "btnNew";
      this.btnNew.Size = new Size(112, 24);
      this.btnNew.TabIndex = 5;
      this.btnNew.Text = "New Blank Loan";
      this.btnNew.Click += new EventHandler(this.btnNew_Click);
      this.checkBoxAppend.BackColor = Color.Transparent;
      this.checkBoxAppend.Location = new Point(17, 420);
      this.checkBoxAppend.Name = "checkBoxAppend";
      this.checkBoxAppend.Size = new Size(548, 22);
      this.checkBoxAppend.TabIndex = 35;
      this.checkBoxAppend.Text = "Append template data. If selected, only non-blank field values in the template are written to the loan.";
      this.checkBoxAppend.UseVisualStyleBackColor = false;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.HelpTag = "Select Loan Template";
      this.emHelpLink1.Location = new Point(16, 488);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 36;
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Location = new Point(32, 438);
      this.label1.Name = "label1";
      this.label1.Size = new Size(411, 13);
      this.label1.TabIndex = 37;
      this.label1.Text = "If not selected, all field values in the template (including blanks) are written to the loan.";
      this.label1.Click += new EventHandler(this.label1_Click);
      this.AcceptButton = (IButtonControl) this.btnDefault;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(600, 512);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.checkBoxAppend);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.btnNew);
      this.Controls.Add((Control) this.btnDefault);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.btnDetail);
      this.Controls.Add((Control) this.btnSelect);
      this.Controls.Add((Control) this.gradPnlDefault);
      this.Controls.Add((Control) this.tempExplorer);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LoanTemplateSelectDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "New Loan";
      this.Closed += new EventHandler(this.LoanTemplateSelectDialog_Closed);
      this.KeyDown += new KeyEventHandler(this.LoanTemplateSelectDialog_KeyDown);
      this.gradPnlDefault.ResumeLayout(false);
      this.gradPnlDefault.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private string getPreference(string id, string item)
    {
      return this.session.GetPrivateProfileString(id, item) ?? "";
    }

    private void setPreference(string id, string item, string val)
    {
    }

    private void btnDetail_Click(object sender, EventArgs e)
    {
      if (this.tempExplorer.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must select a loan template.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.tempExplorer.SelectedItems[0].Tag.ToString() == "")
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "You must select a loan template first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        FileSystemEntry tag = (FileSystemEntry) this.tempExplorer.SelectedItems[0].Tag;
        if (tag.Type != FileSystemEntry.Types.File)
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, "You must select a loan template file.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else if (!this.session.ConfigurationManager.TemplateSettingsObjectExists(EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanTemplate, tag))
        {
          int num4 = (int) Utils.Dialog((IWin32Window) this, "The template not found. Please use 'Refresh' to update the list.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          BinaryObject templateSettings = this.session.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanTemplate, tag);
          if (templateSettings == null)
          {
            int num5 = (int) Utils.Dialog((IWin32Window) this, "The template file is not found.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
          else
          {
            Cursor.Current = Cursors.WaitCursor;
            using (LoanTemplateDialog loanTemplateDialog = new LoanTemplateDialog(this.session, (LoanTemplate) templateSettings, true, true))
            {
              int num6 = (int) loanTemplateDialog.ShowDialog((IWin32Window) this);
            }
            Cursor.Current = Cursors.Default;
          }
        }
      }
    }

    private void btnNew_Click(object sender, EventArgs e)
    {
      this.selectedItem = (FileSystemEntry) null;
      this.DialogResult = DialogResult.OK;
    }

    private void btnSelect_Click(object sender, EventArgs e)
    {
      if (this.tempExplorer.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must select a loan template first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.tempExplorer.SelectedItems[0].Tag.ToString() == "")
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "You must select a loan template first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (!this.btnSelect.Enabled)
      {
        int num3 = (int) Utils.Dialog((IWin32Window) this, "You do not have security access right to a loan template.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        FileSystemEntry tag = (FileSystemEntry) this.tempExplorer.SelectedItems[0].Tag;
        if (tag.Type != FileSystemEntry.Types.File)
        {
          int num4 = (int) Utils.Dialog((IWin32Window) this, "You cannot select a folder.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else if (!this.session.ConfigurationManager.TemplateSettingsObjectExists(EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanTemplate, tag))
        {
          int num5 = (int) Utils.Dialog((IWin32Window) this, "The loan template cannot be found.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          this.selectedItem = tag;
          this.DialogResult = DialogResult.OK;
        }
      }
    }

    private void btnDefault_Click(object sender, EventArgs e)
    {
      if (this.lvwDefault.Items.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You have not defined a default template yet.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        string tag = (string) this.lvwDefault.Items[0].Tag;
        FileSystemEntry fileEntry;
        try
        {
          fileEntry = FileSystemEntry.Parse(tag, this.session.UserID);
        }
        catch
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "The default loan template cannot be found.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        if (!this.session.ConfigurationManager.TemplateSettingsObjectExists(EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanTemplate, fileEntry))
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, "The default loan template cannot be found.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          this.selectedItem = fileEntry;
          this.DialogResult = DialogResult.OK;
        }
      }
    }

    private FileSystemEntry getCurrentDefaultTemplate()
    {
      string privateProfileString = this.session.GetPrivateProfileString("LoanTemplate", "Default");
      try
      {
        FileSystemEntry fileSystemEntry = FileSystemEntry.Parse(privateProfileString);
        this.lvwDefault.Items.Clear();
        if (!((FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features)).CheckPermission(AclFeature.SettingsTab_Personal_LoanTemplateSets, this.session.UserInfo) && fileSystemEntry.ToDisplayString().IndexOf("Personal:") > -1)
          return (FileSystemEntry) null;
        this.lvwDefault.Items.Add(fileSystemEntry.ToDisplayString().Replace("Public:", "Public Loan Templates").Replace("Personal:", "Personal Loan Templates"));
        this.lvwDefault.Items[0].ImageIndex = 4;
        this.lvwDefault.Items[0].Tag = (object) fileSystemEntry.ToDisplayString();
        return fileSystemEntry.ParentFolder;
      }
      catch
      {
        return (FileSystemEntry) null;
      }
    }

    private void tempExplorer_SelectedCurrentFile(object sender, EventArgs e)
    {
      this.btnSelect_Click((object) null, (EventArgs) null);
    }

    private void btnGo_Click(object sender, EventArgs e)
    {
      if (this.lvwDefault.Items.Count == 0)
      {
        if (sender == null)
          return;
        int num = (int) Utils.Dialog((IWin32Window) this, "You don't have default loan template.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        string tag = (string) this.lvwDefault.Items[0].Tag;
        FileSystemEntry target;
        try
        {
          target = FileSystemEntry.Parse(tag, this.session.UserID);
        }
        catch
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The default loan template cannot be found.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        if (target.ParentFolder != null && !this.session.ConfigurationManager.TemplateSettingsObjectExists(EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanTemplate, target.ParentFolder))
        {
          if (sender == null)
            return;
          int num = (int) Utils.Dialog((IWin32Window) this, "The default loan template cannot be found.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
          this.tempExplorer.SetFolder(target);
      }
    }

    private void LoanTemplateSelectDialog_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, "Select Loan Template");
    }

    private void LoanTemplateSelectDialog_Closed(object sender, EventArgs e)
    {
      try
      {
        this.session.WritePrivateProfileString("LoanTemplate", "LastFolderViewed", this.tempExplorer.CurrentFolder.ToString());
      }
      catch (Exception ex)
      {
      }
    }

    private void label1_Click(object sender, EventArgs e)
    {
      this.checkBoxAppend.Checked = !this.checkBoxAppend.Checked;
    }

    public string DialogTitle
    {
      set => this.Text = value;
    }

    public void DisableAppendDataCheckbox()
    {
      this.checkBoxAppend.Visible = false;
      this.label1.Visible = false;
      this.checkBoxAppend.Checked = true;
      this.btnNew.Enabled = false;
    }
  }
}
