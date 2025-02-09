// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.TemplateSelectionDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Workflow;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class TemplateSelectionDialog : Form
  {
    private const string className = "TemplateSelectionDialog";
    private static string sw = Tracing.SwOutsideLoan;
    private System.ComponentModel.Container components;
    private EllieMae.EMLite.ClientServer.TemplateSettingsType templateType;
    private Button selectBtn;
    private Button detailBtn;
    private FSExplorer tempExplorer;
    private Button cancelBtn;
    private bool selectPublicOnly;
    private FileSystemEntry selectedItem;
    private MilestoneTemplate selectedMilestoneTemplate;
    private SyncTemplate selectedSyncTemplate;
    private Sessions.Session session;

    public FileSystemEntry SelectedItem => this.selectedItem;

    public MilestoneTemplate SelectedMilestoneTemplate => this.selectedMilestoneTemplate;

    public SyncTemplate SelectedSyncTemplate => this.selectedSyncTemplate;

    public TemplateSelectionDialog(Sessions.Session session, List<SyncTemplate> syncTemplates)
    {
      this.session = session;
      this.InitializeComponent();
      this.Text = "Select Sync Template";
      this.templateType = EllieMae.EMLite.ClientServer.TemplateSettingsType.SyncTemplates;
      TemplateIFSExplorer templateIfsExplorer = new TemplateIFSExplorer(this.session, EllieMae.EMLite.ClientServer.TemplateSettingsType.SyncTemplates);
      this.tempExplorer.RefreshedClicked += new EventHandler(this.tempExplorer_RefreshedClicked);
      this.tempExplorer.HideAllButtons = true;
      this.tempExplorer.FileType = FSExplorer.FileTypes.SyncTemplates;
      this.tempExplorer.PopulateSyncTemplate(syncTemplates);
      this.detailBtn.Visible = false;
      this.selectBtn.Left = this.detailBtn.Left;
    }

    private void tempExplorer_RefreshedClicked(object sender, EventArgs e)
    {
      this.tempExplorer.PopulateSyncTemplate(this.session.ConfigurationManager.GetAllSyncTemplates());
    }

    public TemplateSelectionDialog(
      Sessions.Session session,
      EllieMae.EMLite.ClientServer.TemplateSettingsType templateType,
      FileSystemEntry defaultFolder)
      : this(session, templateType, defaultFolder, false)
    {
    }

    public TemplateSelectionDialog(
      Sessions.Session session,
      EllieMae.EMLite.ClientServer.TemplateSettingsType templateType,
      FileSystemEntry defaultFolder,
      bool selectPublicOnly)
      : this(session, templateType, defaultFolder, selectPublicOnly, FSExplorer.RESPAFilter.All)
    {
    }

    public TemplateSelectionDialog(
      Sessions.Session session,
      EllieMae.EMLite.ClientServer.TemplateSettingsType templateType,
      FileSystemEntry defaultFolder,
      bool selectPublicOnly,
      FSExplorer.RESPAFilter respaMode)
    {
      this.session = session;
      this.InitializeComponent();
      if (this.tempExplorer.GetSession() == null)
        this.tempExplorer.SetSession(this.session);
      this.selectPublicOnly = selectPublicOnly;
      if (respaMode != FSExplorer.RESPAFilter.All)
        this.tempExplorer.RESPAMode = respaMode;
      switch (templateType)
      {
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.CustomLetter:
          this.Text = "Select Custom Print Form";
          this.detailBtn.Visible = false;
          this.selectBtn.Left = this.detailBtn.Left;
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanProgram:
          this.Text = "Select Loan Program Template";
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.ClosingCost:
          this.Text = "Select Closing Cost Template";
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.MiscData:
          this.Text = "Select Data Template";
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.FormList:
          this.Text = "Select Input Form Set Template";
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentSet:
          this.Text = "Select Document Set Template";
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanTemplate:
          this.Text = "Select Loan Template";
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.TaskSet:
          this.Text = "Select Task Set Template";
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.SettlementServiceProviders:
          this.Text = "Select Settlement Service Provider Template";
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.MilestoneTemplate:
          this.Text = "Select Milestone Template";
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.AffiliatedBusinessArrangements:
          this.Text = "Select Affiliated Business Arrangement Template";
          break;
      }
      this.templateType = templateType;
      string section = "";
      if (templateType <= EllieMae.EMLite.ClientServer.TemplateSettingsType.TaskSet)
      {
        switch (templateType)
        {
          case EllieMae.EMLite.ClientServer.TemplateSettingsType.CustomLetter:
            section = "CustomForms";
            break;
          case EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanProgram:
            section = "LoanProgramTemplate";
            break;
          case EllieMae.EMLite.ClientServer.TemplateSettingsType.ClosingCost:
            section = "ClosingCostTemplate";
            break;
          case EllieMae.EMLite.ClientServer.TemplateSettingsType.MiscData:
            section = "DataTemplate";
            break;
          case EllieMae.EMLite.ClientServer.TemplateSettingsType.FormList:
            section = "FormListTemplate";
            break;
          case EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentSet:
            section = "DocumentSetTemplate";
            break;
          case EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanTemplate:
            section = "LoanTemplate";
            break;
          default:
            if (templateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.TaskSet)
            {
              section = "TaskSetTemplate";
              break;
            }
            break;
        }
      }
      else if (templateType != EllieMae.EMLite.ClientServer.TemplateSettingsType.SettlementServiceProviders)
      {
        if (templateType != EllieMae.EMLite.ClientServer.TemplateSettingsType.MilestoneTemplate)
        {
          if (templateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.AffiliatedBusinessArrangements)
            section = "AffiliateTemplate";
        }
        else
          section = "MilestoneTemplate";
      }
      else
        section = "SettlementServiceProviderTemplate";
      string uri = this.session.GetPrivateProfileString(section, "LastFolderViewed") ?? "";
      TemplateIFSExplorer ifsExplorer;
      switch (templateType)
      {
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.MiscData:
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.FormList:
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentSet:
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.TaskSet:
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.SettlementServiceProviders:
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.AffiliatedBusinessArrangements:
          ifsExplorer = new TemplateIFSExplorer(this.session, templateType, false);
          ifsExplorer.SelectedCurrentFile += new EventHandler(this.selectBtn_Click);
          break;
        default:
          ifsExplorer = new TemplateIFSExplorer(this.session, templateType);
          break;
      }
      switch (templateType)
      {
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.CustomLetter:
          this.tempExplorer.FileType = FSExplorer.FileTypes.CustomForms;
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanProgram:
          this.tempExplorer.FileType = FSExplorer.FileTypes.LoanPrograms;
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.ClosingCost:
          this.tempExplorer.FileType = FSExplorer.FileTypes.ClosingCosts;
          this.tempExplorer.AddNewHUDColumn();
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.MiscData:
          this.tempExplorer.FileType = FSExplorer.FileTypes.DataTemplates;
          this.tempExplorer.AddNewHUDColumn();
          this.tempExplorer.RESPAMode = this.session.LoanData == null || this.session.LoanData.GetLogList().GetAllIDisclosureTracking2015Log(false).Length == 0 && this.session.LoanData.GetLogList().GetAllIDisclosureTracking2015Log(false).Length == 0 ? FSExplorer.RESPAFilter.All : (this.session.LoanData.Use2015RESPA ? FSExplorer.RESPAFilter.Respa2015 : (this.session.LoanData.Use2010RESPA ? FSExplorer.RESPAFilter.Respa2010 : FSExplorer.RESPAFilter.Respa2009));
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.FormList:
          this.tempExplorer.FileType = FSExplorer.FileTypes.FormLists;
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentSet:
          this.tempExplorer.FileType = FSExplorer.FileTypes.DocumentSets;
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanTemplate:
          this.tempExplorer.FileType = FSExplorer.FileTypes.LoanTemplates;
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.TaskSet:
          this.tempExplorer.FileType = FSExplorer.FileTypes.TaskSets;
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.SettlementServiceProviders:
          this.tempExplorer.FileType = FSExplorer.FileTypes.SettlementServiceProviders;
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.MilestoneTemplate:
          this.tempExplorer.FileType = FSExplorer.FileTypes.MilestoneTemplate;
          this.tempExplorer.PopulateMilestoneTemplate(this.session.SessionObjects.BpmManager.GetMilestoneTemplates(true));
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.AffiliatedBusinessArrangements:
          this.tempExplorer.FileType = FSExplorer.FileTypes.AffiliatedBusinessArrangements;
          break;
      }
      this.tempExplorer.HideAllButtons = true;
      if (templateType != EllieMae.EMLite.ClientServer.TemplateSettingsType.MilestoneTemplate)
        this.tempExplorer.SetProperties(false, false, true, (int) templateType, templateType != EllieMae.EMLite.ClientServer.TemplateSettingsType.CustomLetter);
      if (templateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.CustomLetter)
        this.tempExplorer.HideDescription = true;
      this.tempExplorer.SingleSelection = true;
      this.tempExplorer.EntryDoubleClicked += new EventHandler(this.tempExplorer_EntryDoubleClicked);
      try
      {
        if (defaultFolder == null)
          defaultFolder = FileSystemEntry.Parse(uri);
        if (!ifsExplorer.EntryExists(defaultFolder))
          defaultFolder = FileSystemEntry.PublicRoot;
        if (selectPublicOnly && !defaultFolder.IsPublic)
          defaultFolder = FileSystemEntry.PublicRoot;
        if (!this.selectPublicOnly)
          this.selectPublicOnly = !this.getPersonalRight(templateType);
      }
      catch
      {
        defaultFolder = FileSystemEntry.PublicRoot;
      }
      if (templateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.MilestoneTemplate)
        return;
      this.tempExplorer.Init((IFSExplorerBase) ifsExplorer, defaultFolder, this.selectPublicOnly);
    }

    private void tempExplorer_EntryDoubleClicked(object sender, EventArgs e)
    {
      this.selectBtn_Click((object) null, (EventArgs) null);
    }

    public void SetRESPAMode(FSExplorer.RESPAFilter filter) => this.tempExplorer.RESPAMode = filter;

    private bool getPersonalRight(EllieMae.EMLite.ClientServer.TemplateSettingsType type)
    {
      if (UserInfo.IsSuperAdministrator(this.session.UserID, this.session.UserInfo.UserPersonas))
        return true;
      FeaturesAclManager aclManager = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
      AclFeature feature = AclFeature.SettingsTab_Personal_LoanPrograms;
      switch (type)
      {
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.CustomLetter:
          feature = AclFeature.SettingsTab_Personal_CustomPrintForms;
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanProgram:
          feature = AclFeature.SettingsTab_Personal_LoanPrograms;
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.ClosingCost:
          feature = AclFeature.SettingsTab_Personal_ClosingCosts;
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.MiscData:
          feature = AclFeature.SettingsTab_Personal_MiscDataTemplates;
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.FormList:
          feature = AclFeature.SettingsTab_Personal_InputFormSets;
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentSet:
          feature = AclFeature.SettingsTab_Personal_DocumentSets;
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanTemplate:
          feature = AclFeature.SettingsTab_Personal_LoanTemplateSets;
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.TaskSet:
          feature = AclFeature.SettingsTab_Personal_DocumentSets;
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.SettlementServiceProviders:
          feature = AclFeature.SettingsTab_Personal_SettlementServiceProvider;
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.AffiliatedBusinessArrangements:
          feature = AclFeature.SettingsTab_Personal_Affiliate;
          break;
      }
      return aclManager.GetUserApplicationRight(feature);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.selectBtn = new Button();
      this.detailBtn = new Button();
      this.tempExplorer = new FSExplorer();
      this.cancelBtn = new Button();
      this.SuspendLayout();
      this.selectBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.selectBtn.Location = new Point(406, 407);
      this.selectBtn.Name = "selectBtn";
      this.selectBtn.Size = new Size(75, 24);
      this.selectBtn.TabIndex = 10;
      this.selectBtn.Text = "Select";
      this.selectBtn.Click += new EventHandler(this.selectBtn_Click);
      this.detailBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.detailBtn.Location = new Point(489, 407);
      this.detailBtn.Name = "detailBtn";
      this.detailBtn.Size = new Size(75, 24);
      this.detailBtn.TabIndex = 13;
      this.detailBtn.Text = "Details";
      this.detailBtn.Click += new EventHandler(this.detailBtn_Click);
      this.tempExplorer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tempExplorer.FolderComboSelectedIndex = -1;
      this.tempExplorer.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.tempExplorer.HasPublicRight = true;
      this.tempExplorer.Location = new Point(8, 8);
      this.tempExplorer.Name = "tempExplorer";
      this.tempExplorer.RenameButtonSize = new Size(62, 22);
      this.tempExplorer.RESPAMode = FSExplorer.RESPAFilter.All;
      this.tempExplorer.setContactType = ContactType.BizPartner;
      this.tempExplorer.Size = new Size(640, 388);
      this.tempExplorer.TabIndex = 14;
      this.tempExplorer.SelectedCurrentFile += new EventHandler(this.tempExplorer_SelectedCurrentFile);
      this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(572, 407);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(80, 24);
      this.cancelBtn.TabIndex = 15;
      this.cancelBtn.Text = "Cancel";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(660, 444);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.tempExplorer);
      this.Controls.Add((Control) this.detailBtn);
      this.Controls.Add((Control) this.selectBtn);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (TemplateSelectionDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Templates";
      this.Closed += new EventHandler(this.TemplateSelectionDialog_Closed);
      this.ResumeLayout(false);
    }

    private void selectBtn_Click(object sender, EventArgs e)
    {
      if (this.tempExplorer.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must select a template.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (this.templateType != EllieMae.EMLite.ClientServer.TemplateSettingsType.SyncTemplates)
        {
          if (this.tempExplorer.SelectedItems[0].Tag.ToString() == "")
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "You must select a template.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
          if (!this.tempExplorer.IsRootPublic && this.selectPublicOnly)
          {
            int num3 = (int) Utils.Dialog((IWin32Window) this, "You can only select public template for public loan template.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
        }
        if (this.templateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.MilestoneTemplate)
          this.selectedMilestoneTemplate = (MilestoneTemplate) this.tempExplorer.SelectedItems[0].Tag;
        else if (this.templateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.SyncTemplates)
        {
          this.selectedSyncTemplate = (SyncTemplate) this.tempExplorer.SelectedItems[0].Tag;
        }
        else
        {
          FileSystemEntry tag = (FileSystemEntry) this.tempExplorer.SelectedItems[0].Tag;
          if (tag.Type != FileSystemEntry.Types.File)
          {
            int num4 = (int) Utils.Dialog((IWin32Window) this, "You must select a template file.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
          if (this.templateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.MiscData && this.session.LoanData != null)
          {
            string str = tag.Properties.ContainsKey((object) "RESPAVERSION") ? tag.Properties[(object) "RESPAVERSION"].ToString() : "";
            if (str == "2015" && !this.session.LoanData.Use2015RESPA || str == "2010" && !this.session.LoanData.Use2010RESPA || str == "2009" && (this.session.LoanData.Use2010RESPA || this.session.LoanData.Use2015RESPA))
            {
              if (this.session.LoanData != null && (this.session.LoanData.GetLogList().GetAllDisclosureTrackingLog(false).Length != 0 || this.session.LoanData.GetLogList().GetAllIDisclosureTracking2015Log(false).Length != 0))
              {
                int num5 = (int) Utils.Dialog((IWin32Window) this, "The current loan has been disclosed. Applying this template will change the RESPA-TILA Form Version currently being used for this loan.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
              }
              if (Utils.Dialog((IWin32Window) this, "Applying this template will change the RESPA-TILA Form Version currently being used for this loan. Do you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
                return;
            }
            BinaryObject templateSettings = this.session.ConfigurationManager.GetTemplateSettings(this.templateType, tag);
            if (templateSettings != null && !new BusinessRuleCheck().SkipReadOnlyFields((object) (DataTemplate) templateSettings, this.session.LoanData))
              return;
          }
          this.selectedItem = tag;
        }
        this.DialogResult = DialogResult.OK;
      }
    }

    private void detailBtn_Click(object sender, EventArgs e)
    {
      if (this.tempExplorer.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must select a template.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.tempExplorer.SelectedItems[0].Tag.ToString() == "")
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "You must select a template.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.templateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.MilestoneTemplate)
      {
        Cursor.Current = Cursors.WaitCursor;
        using (MilestoneTemplateDialog milestoneTemplateDialog = new MilestoneTemplateDialog(this.session, (MilestoneTemplate) this.tempExplorer.SelectedItems[0].Tag))
        {
          int num3 = (int) milestoneTemplateDialog.ShowDialog((IWin32Window) this);
        }
        Cursor.Current = Cursors.Default;
      }
      else
      {
        FileSystemEntry tag = (FileSystemEntry) this.tempExplorer.SelectedItems[0].Tag;
        if (tag.Type != FileSystemEntry.Types.File)
        {
          int num4 = (int) Utils.Dialog((IWin32Window) this, "You must select a template file.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else if (!this.session.ConfigurationManager.TemplateSettingsObjectExists(this.templateType, tag))
        {
          int num5 = (int) Utils.Dialog((IWin32Window) this, "The template not found. Please use 'Refresh' to update the list.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          BinaryObject templateSettings = this.session.ConfigurationManager.GetTemplateSettings(this.templateType, tag);
          if (templateSettings == null)
          {
            int num6 = (int) Utils.Dialog((IWin32Window) this, "The template file not found.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
          else
          {
            Cursor.Current = Cursors.WaitCursor;
            Form form = (Form) null;
            switch (this.templateType)
            {
              case EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanProgram:
                TemplateDialog templateDialog1 = new TemplateDialog((FieldDataTemplate) (LoanProgram) templateSettings, this.templateType, this.tempExplorer.IsRootPublic, true, this.session);
                templateDialog1.LoadForm("Loan Program", "LOANPROG");
                form = (Form) templateDialog1;
                break;
              case EllieMae.EMLite.ClientServer.TemplateSettingsType.ClosingCost:
                TemplateDialog templateDialog2 = new TemplateDialog((FieldDataTemplate) (ClosingCost) templateSettings, this.templateType, this.tempExplorer.IsRootPublic, true, this.session);
                if (((ClosingCost) templateSettings).RESPAVersion == "2015")
                  templateDialog2.LoadForm("Closing Cost", "REGZGFE_2015");
                else if (((ClosingCost) templateSettings).For2010GFE)
                  templateDialog2.LoadForm("Closing Cost", "REGZGFE_2010");
                else
                  templateDialog2.LoadForm("Closing Cost", "CCOSTPROG");
                form = (Form) templateDialog2;
                break;
              case EllieMae.EMLite.ClientServer.TemplateSettingsType.MiscData:
                form = (Form) new DataTemplateDialog(this.session, (DataTemplate) templateSettings, true);
                break;
              case EllieMae.EMLite.ClientServer.TemplateSettingsType.FormList:
                form = (Form) new FormListDialog((FormTemplate) templateSettings, true);
                break;
              case EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentSet:
                form = (Form) new DocumentSetTemplateDialog((DocumentSetTemplate) templateSettings, true, this.session);
                break;
              case EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanTemplate:
                form = (Form) new LoanTemplateDialog(this.session, (LoanTemplate) templateSettings, true, true);
                break;
              case EllieMae.EMLite.ClientServer.TemplateSettingsType.FundingTemplate:
                FundingTemplate template = (FundingTemplate) templateSettings;
                TemplateDialog templateDialog3 = new TemplateDialog((FieldDataTemplate) template, this.templateType, this.tempExplorer.IsRootPublic, true, this.session);
                if (template.RESPAVersion == "2015")
                  templateDialog3.LoadForm("Funding Template", "FundingTemplate2015");
                else if (template.RESPAVersion == "2010" || template.For2010GFE)
                  templateDialog3.LoadForm("Funding Template", "FundingTemplate2010");
                else
                  templateDialog3.LoadForm("Funding Template", "FundingTemplateForm");
                form = (Form) templateDialog3;
                break;
              case EllieMae.EMLite.ClientServer.TemplateSettingsType.TaskSet:
                form = (Form) new TaskSetTemplateDialog((TaskSetTemplate) templateSettings, true, this.session);
                break;
              case EllieMae.EMLite.ClientServer.TemplateSettingsType.SettlementServiceProviders:
                form = (Form) new SettlementServiceTemplateDialog((SettlementServiceTemplate) templateSettings, tag.IsPublic, true, this.session);
                break;
              case EllieMae.EMLite.ClientServer.TemplateSettingsType.AffiliatedBusinessArrangements:
                form = (Form) new AffiliateTemplateDialog((AffiliateTemplate) templateSettings, tag.IsPublic, true, this.session);
                break;
            }
            if (form != null)
            {
              try
              {
                int num7 = (int) form.ShowDialog((IWin32Window) this);
              }
              finally
              {
                form.Dispose();
              }
            }
            Cursor.Current = Cursors.Default;
          }
        }
      }
    }

    private void tempExplorer_SelectedCurrentFile(object sender, EventArgs e)
    {
      if (this.tempExplorer.SelectedItems.Count == 0)
        return;
      this.selectBtn_Click((object) null, (EventArgs) null);
    }

    private void TemplateSelectionDialog_Closed(object sender, EventArgs e)
    {
      string section = "";
      switch (this.templateType)
      {
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.CustomLetter:
          section = "CustomForms";
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanProgram:
          section = "LoanProgramTemplate";
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.ClosingCost:
          section = "ClosingCostTemplate";
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.MiscData:
          section = "DataTemplate";
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.FormList:
          section = "FormListTemplate";
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentSet:
          section = "DocumentSetTemplate";
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanTemplate:
          section = "LoanTemplate";
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.TaskSet:
          section = "TaskSetTemplate";
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.SettlementServiceProviders:
          section = "SettlementServiceProviderTemplate";
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.MilestoneTemplate:
          section = "MilestoneTemplate";
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.AffiliatedBusinessArrangements:
          section = "AffiliateTemplate";
          break;
      }
      try
      {
        this.session.WritePrivateProfileString(section, "LastFolderViewed", this.tempExplorer.CurrentFolder.ToString());
      }
      catch (Exception ex)
      {
      }
    }
  }
}
