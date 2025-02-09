// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.eFolder.ImportWeb
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.Import;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ePass.eFolder
{
  public class ImportWeb : Form
  {
    private ComboBox folderCombo;
    private Button cancelBtn;
    private Button importBtn;
    private IContainer components;
    private Label importLbl;
    private TextBox txtTemplate;
    private Label label1;
    private StandardIconButton iconBtnBrowse;
    private StandardIconButton iconBtnClear;
    private LoanTemplateSelection loanTemplate;
    private ToolTip toolTip1;
    private LoanImportRequirement loanImportRequirement;
    private string webFile = string.Empty;

    public ImportWeb(string webFile)
    {
      this.InitializeComponent();
      this.webFile = webFile;
      foreach (object obj in ((LoanFolderRuleManager) Session.BPM.GetBpmManager(BpmCategory.LoanFolder)).GetLoanFoldersForAction(LoanFolderAction.Import))
        this.folderCombo.Items.Add(obj);
      this.folderCombo.SelectedItem = (object) Session.UserInfo.WorkingFolder;
      this.loanImportRequirement = Session.ConfigurationManager.GetLoanImportRequirements();
      if (this.loanImportRequirement.WebCenterImportRequirementType == LoanImportRequirement.LoanImportRequirementType.TemplateIsRequiredByCompany && this.loanImportRequirement.TemplateForWebCenterImport != string.Empty)
      {
        this.loanTemplate = new LoanTemplateSelection(FileSystemEntry.Parse(this.loanImportRequirement.TemplateForWebCenterImport), true);
        this.txtTemplate.Text = this.loanTemplate.TemplateEntry.Name;
      }
      else
        this.txtTemplate_TextChanged((object) null, (EventArgs) null);
      this.iconBtnBrowse.Enabled = this.loanImportRequirement.WebCenterImportRequirementType != LoanImportRequirement.LoanImportRequirementType.TemplateIsRequiredByCompany;
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
      this.importLbl = new Label();
      this.folderCombo = new ComboBox();
      this.cancelBtn = new Button();
      this.importBtn = new Button();
      this.txtTemplate = new TextBox();
      this.label1 = new Label();
      this.iconBtnBrowse = new StandardIconButton();
      this.iconBtnClear = new StandardIconButton();
      this.toolTip1 = new ToolTip(this.components);
      ((ISupportInitialize) this.iconBtnBrowse).BeginInit();
      ((ISupportInitialize) this.iconBtnClear).BeginInit();
      this.SuspendLayout();
      this.importLbl.AutoSize = true;
      this.importLbl.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.importLbl.Location = new Point(12, 12);
      this.importLbl.Name = "importLbl";
      this.importLbl.Size = new Size(310, 14);
      this.importLbl.TabIndex = 0;
      this.importLbl.Text = "Select the loan folder where you want to import the Application";
      this.folderCombo.DropDownStyle = ComboBoxStyle.DropDownList;
      this.folderCombo.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.folderCombo.Location = new Point(15, 29);
      this.folderCombo.Name = "folderCombo";
      this.folderCombo.Size = new Size(332, 22);
      this.folderCombo.TabIndex = 1;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cancelBtn.Location = new Point(272, 116);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 3;
      this.cancelBtn.Text = "Cancel";
      this.importBtn.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.importBtn.Location = new Point(192, 116);
      this.importBtn.Name = "importBtn";
      this.importBtn.Size = new Size(75, 24);
      this.importBtn.TabIndex = 2;
      this.importBtn.Text = "Import";
      this.importBtn.Click += new EventHandler(this.importBtn_Click);
      this.txtTemplate.Location = new Point(15, 80);
      this.txtTemplate.Name = "txtTemplate";
      this.txtTemplate.ReadOnly = true;
      this.txtTemplate.Size = new Size(292, 20);
      this.txtTemplate.TabIndex = 20;
      this.txtTemplate.Tag = (object) "1862";
      this.txtTemplate.TextChanged += new EventHandler(this.txtTemplate_TextChanged);
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(12, 63);
      this.label1.Name = "label1";
      this.label1.Size = new Size(74, 14);
      this.label1.TabIndex = 23;
      this.label1.Text = "Loan template";
      this.iconBtnBrowse.BackColor = Color.Transparent;
      this.iconBtnBrowse.Location = new Point(310, 82);
      this.iconBtnBrowse.MouseDownImage = (Image) null;
      this.iconBtnBrowse.Name = "iconBtnBrowse";
      this.iconBtnBrowse.Size = new Size(16, 16);
      this.iconBtnBrowse.StandardButtonType = StandardIconButton.ButtonType.ZoomInButton;
      this.iconBtnBrowse.TabIndex = 24;
      this.iconBtnBrowse.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.iconBtnBrowse, "Select Template");
      this.iconBtnBrowse.Click += new EventHandler(this.iconBtnBrowse_Click);
      this.iconBtnClear.BackColor = Color.Transparent;
      this.iconBtnClear.Location = new Point(331, 82);
      this.iconBtnClear.MouseDownImage = (Image) null;
      this.iconBtnClear.Name = "iconBtnClear";
      this.iconBtnClear.Size = new Size(16, 16);
      this.iconBtnClear.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.iconBtnClear.TabIndex = 25;
      this.iconBtnClear.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.iconBtnClear, "Remove Template");
      this.iconBtnClear.Click += new EventHandler(this.iconBtnClear_Click);
      this.AcceptButton = (IButtonControl) this.importBtn;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(358, 149);
      this.Controls.Add((Control) this.iconBtnClear);
      this.Controls.Add((Control) this.iconBtnBrowse);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.txtTemplate);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.importBtn);
      this.Controls.Add((Control) this.folderCombo);
      this.Controls.Add((Control) this.importLbl);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ImportWeb);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Import Web Application";
      ((ISupportInitialize) this.iconBtnBrowse).EndInit();
      ((ISupportInitialize) this.iconBtnClear).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void importBtn_Click(object sender, EventArgs e)
    {
      Session.ISession.LoanImportInProgress = true;
      if (this.folderCombo.SelectedItem == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You have to choose a destination folder.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        string loanFolder = this.folderCombo.SelectedItem.ToString();
        byte[] numArray = (byte[]) null;
        string empty = string.Empty;
        string str1;
        try
        {
          using (FileStream fileStream = File.OpenRead(this.webFile))
          {
            numArray = new byte[fileStream.Length];
            fileStream.Read(numArray, 0, numArray.Length);
          }
          str1 = Encoding.UTF8.GetString(numArray).Trim();
        }
        catch (FormatException ex)
        {
          throw new FormatException("Unable to find Webapp file '" + this.webFile + "'");
        }
        bool enforceApplicationDate = (EnableDisableSetting) Session.ServerManager.GetServerSetting("Import.EnforceApplicationDate") == EnableDisableSetting.Enabled;
        string str2 = string.Empty;
        string emSiteID = string.Empty;
        string str3 = string.Empty;
        bool flag1 = false;
        LoanDataMgr loanDataMgr;
        if (str1.StartsWith("<Fields>"))
        {
          WebsitesImport websitesImport;
          if (this.loanTemplate != null)
          {
            websitesImport = new WebsitesImport(this.loanTemplate);
            loanDataMgr = websitesImport.Convert(this.webFile, Session.SessionObjects, string.Empty, enforceApplicationDate);
          }
          else
          {
            websitesImport = new WebsitesImport(Session.SessionObjects);
            loanDataMgr = websitesImport.Convert(this.webFile, Session.SessionObjects, string.Empty, enforceApplicationDate);
          }
          str2 = websitesImport.LoanImportId;
          emSiteID = websitesImport.EMSiteID;
          str3 = websitesImport.LoanGuid;
          flag1 = websitesImport.OverwriteExistingWebLoanFileWithGuid;
        }
        else
        {
          bool flag2 = (EnableDisableSetting) Session.ServerManager.GetServerSetting("Import.LoanNumbering") == EnableDisableSetting.Enabled;
          FannieImport fannieImport = this.loanTemplate == null ? new FannieImport(Session.SessionObjects) : new FannieImport(this.loanTemplate);
          fannieImport.UseEMLoanNumbering = flag2;
          fannieImport.EnforceApplicationDate = enforceApplicationDate;
          loanDataMgr = fannieImport.Convert(this.webFile, Session.SessionObjects, string.Empty);
        }
        if (loanDataMgr != null)
        {
          loanDataMgr.FromLoanImport = true;
          if (!flag1)
            loanDataMgr.Create(loanFolder, "");
          string str4 = str3 != "" ? str3 : loanDataMgr.LoanData.GUID;
          loanDataMgr.Close();
          if (str2 != string.Empty)
          {
            DateTime now = DateTime.Now;
            if (!loanDataMgr.SubmitTPOLoanImportStatus(str2, emSiteID, str4, now))
              Session.LoanManager.AddWebCenterImportID(str2, emSiteID, str4, now, Session.UserID);
          }
          DialogResult dialogResult = Utils.Dialog((IWin32Window) this, "Your web application has been successfully imported. Would you like to open this loan now?", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
          this.DialogResult = DialogResult.OK;
          if (dialogResult == DialogResult.Yes)
            this.openLoan(str4);
        }
        else
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "Your web application failed to be imported.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.DialogResult = DialogResult.OK;
        }
        Session.ISession.LoanImportInProgress = false;
      }
    }

    private void openLoan(string guid)
    {
      Session.Application.GetService<ILoanConsole>().OpenLoan(guid, true);
      Session.Application.GetService<IEncompassApplication>().CloseModalDialogs();
    }

    private void iconBtnBrowse_Click(object sender, EventArgs e)
    {
      FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      using (LoanTemplateSelectDialog templateSelectDialog = new LoanTemplateSelectDialog(Session.DefaultInstance, false, aclManager.GetUserApplicationRight(AclFeature.LoanMgmt_CreateBlank), aclManager.GetUserApplicationRight(AclFeature.LoanMgmt_CreateFromTmpl)))
      {
        templateSelectDialog.DisableAppendDataCheckbox();
        if (templateSelectDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        FileSystemEntry selectedItem = templateSelectDialog.SelectedItem;
        if (selectedItem != null)
        {
          this.loanTemplate = new LoanTemplateSelection(selectedItem, templateSelectDialog.AppendData);
          this.txtTemplate.Text = this.loanTemplate.TemplateEntry.Name;
        }
        else
        {
          this.loanTemplate = (LoanTemplateSelection) null;
          this.txtTemplate.Text = "";
        }
      }
    }

    private void iconBtnClear_Click(object sender, EventArgs e)
    {
      this.txtTemplate.Text = string.Empty;
      this.loanTemplate = (LoanTemplateSelection) null;
    }

    private void txtTemplate_TextChanged(object sender, EventArgs e)
    {
      this.importBtn.Enabled = this.loanImportRequirement.WebCenterImportRequirementType == LoanImportRequirement.LoanImportRequirementType.TemplateIsNotRequired || this.loanImportRequirement.WebCenterImportRequirementType != LoanImportRequirement.LoanImportRequirementType.TemplateIsNotRequired && this.txtTemplate.Text.Trim() != string.Empty;
      this.iconBtnClear.Enabled = this.txtTemplate.Text.Trim() != string.Empty && this.loanImportRequirement.WebCenterImportRequirementType == LoanImportRequirement.LoanImportRequirementType.TemplateIsNotRequired;
    }
  }
}
