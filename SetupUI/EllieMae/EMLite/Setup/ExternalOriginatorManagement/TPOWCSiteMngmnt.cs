// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.TPOWCSiteMngmnt
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.ExternalOriginatorManagement
{
  public class TPOWCSiteMngmnt : SettingsUserControl, IOnlineHelpTarget
  {
    private const string className = "TPOWCSiteMngmnt";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private Sessions.Session session;
    private string[] roles;
    private string[] userGroups;
    private string updateBrokerFile;
    private string importCreditLiabilities;
    private string registrationFileContact;
    private string registrationUserGroup;
    private string submittalFileContact;
    private string submittalUserGroup;
    private string lockRequestFileContact;
    private string lockDeskUserGroup;
    private string enableToCreateDocNames;
    private string enableUploadFiles;
    private string enableAddFilesToDocs;
    private string enableAddFilesToConditions;
    private string enableAssignDocsToConditions;
    private string enableMarkConditionsAsFulfilled;
    private IContainer components;
    private ToolTip toolTip1;
    private GroupContainer gcLoanSetup;
    private Panel pnlLoanSetup;
    private RadioButton rbNoUpgrdBrkrFile;
    private RadioButton rbYesUpgrdBrkrFile;
    private Label lblUpdateBrokerFile;
    private GroupContainer gcCreditSettings;
    private Panel pnlCreditSettings;
    private RadioButton rbNoImportCreditLiabilities;
    private RadioButton rbYesImportCreditLiabilities;
    private Label lblImportCreditLiabilities;
    private GroupContainer gvRegistrationSettings;
    private Panel pnlRegistrationSettings;
    private Label label1;
    private Label label2;
    private ComboBox cbRegistrationUserGroup;
    private ComboBox cbRegistrationFileContact;
    private GroupContainer gcSubmittalSettings;
    private Panel pnlSubmittalSettings;
    private ComboBox cbSubmittalUserGroup;
    private ComboBox cbSubmittalFileContact;
    private Label label3;
    private Label label4;
    private GroupContainer gcLockRequestSettings;
    private Panel pnlLockRequestSettings;
    private ComboBox cbLockDeskUserGroup;
    private ComboBox cbLockRequestFileContact;
    private Label label5;
    private Label label6;
    private GroupContainer gcDocumentConditionsSettings;
    private Panel pnlDocAndCondSettings;
    private CheckBox chkDocumentNames;
    private CheckBox chkUploadFiles;
    private CheckBox chkMarkConditionsFulfilled;
    private CheckBox chkAssignDocsToConditions;
    private CheckBox chkAddFilesToConditions;
    private CheckBox chkAddFilesToDocs;

    public string UpdateBrokerFile
    {
      get => this.updateBrokerFile;
      set => this.updateBrokerFile = value;
    }

    public string ImportCreditLiabilities
    {
      get => this.importCreditLiabilities;
      set => this.importCreditLiabilities = value;
    }

    public string RegistrationFileContact
    {
      get => this.registrationFileContact;
      set => this.registrationFileContact = value;
    }

    public string RegistrationUserGroup
    {
      get => this.registrationUserGroup;
      set => this.registrationUserGroup = value;
    }

    public string SubmittalFileContact
    {
      get => this.submittalFileContact;
      set => this.submittalFileContact = value;
    }

    public string SubmittalUserGroup
    {
      get => this.submittalUserGroup;
      set => this.submittalUserGroup = value;
    }

    public string LockRequestFileContact
    {
      get => this.lockRequestFileContact;
      set => this.lockRequestFileContact = value;
    }

    public string LockDeskUserGroup
    {
      get => this.lockDeskUserGroup;
      set => this.lockDeskUserGroup = value;
    }

    public string EnableToCreateDocNames
    {
      get => this.enableToCreateDocNames;
      set => this.enableToCreateDocNames = value;
    }

    public string EnableUploadFiles
    {
      get => this.enableUploadFiles;
      set => this.enableUploadFiles = value;
    }

    public string EnableAddFilesToDocs
    {
      get => this.enableAddFilesToDocs;
      set => this.enableAddFilesToDocs = value;
    }

    public string EnableAddFilesToConditions
    {
      get => this.enableAddFilesToConditions;
      set => this.enableAddFilesToConditions = value;
    }

    public string EnableAssignDocsToConditions
    {
      get => this.enableAssignDocsToConditions;
      set => this.enableAssignDocsToConditions = value;
    }

    public string EnableMarkConditionsAsFulfilled
    {
      get => this.enableMarkConditionsAsFulfilled;
      set => this.enableMarkConditionsAsFulfilled = value;
    }

    public TPOWCSiteMngmnt(SetUpContainer setupCont, Sessions.Session session)
      : base(setupCont)
    {
      this.InitializeComponent();
      this.session = session;
      this.fetchRoles();
      this.fetchUserGroups();
      this.populateRoles();
      this.populateUserGroups();
      this.initialPageValue();
    }

    private void fetchRoles()
    {
      RoleInfo[] allRoleFunctions = ((WorkflowManager) this.session.BPM.GetBpmManager(BpmCategory.Workflow)).GetAllRoleFunctions();
      this.roles = new string[allRoleFunctions.Length];
      for (int index = 0; index < allRoleFunctions.Length; ++index)
        this.roles[index] = allRoleFunctions[index].Name;
      Array.Sort<string>(this.roles);
    }

    private void fetchUserGroups()
    {
      AclGroup[] allGroups = this.session.AclGroupManager.GetAllGroups();
      this.userGroups = new string[allGroups.Length];
      for (int index = 0; index < allGroups.Length; ++index)
        this.userGroups[index] = allGroups[index].Name;
      Array.Sort<string>(this.userGroups);
    }

    private void populateRoles()
    {
      this.cbRegistrationFileContact.Items.AddRange((object[]) this.roles);
      this.cbSubmittalFileContact.Items.AddRange((object[]) this.roles);
      this.cbLockRequestFileContact.Items.AddRange((object[]) this.roles);
    }

    private void populateUserGroups()
    {
      this.cbRegistrationUserGroup.Items.AddRange((object[]) this.userGroups);
      this.cbSubmittalUserGroup.Items.AddRange((object[]) this.userGroups);
      this.cbLockDeskUserGroup.Items.AddRange((object[]) this.userGroups);
    }

    private void initialPageValue()
    {
      this.getCompanySettings();
      this.populateRegistrationSettings();
      this.populateSubmittalSettings();
      this.populateLockRequestSettings();
      this.selectRadiobuttonOption(this.rbYesUpgrdBrkrFile, this.rbNoUpgrdBrkrFile, this.updateBrokerFile);
      this.selectRadiobuttonOption(this.rbYesImportCreditLiabilities, this.rbNoImportCreditLiabilities, this.importCreditLiabilities);
      this.setCheckboxValue(this.chkDocumentNames, !string.IsNullOrEmpty(this.enableToCreateDocNames) && Convert.ToBoolean(this.enableToCreateDocNames));
      this.setCheckboxValue(this.chkUploadFiles, !string.IsNullOrEmpty(this.enableUploadFiles) && Convert.ToBoolean(this.enableUploadFiles));
      this.setCheckboxValue(this.chkAddFilesToDocs, !string.IsNullOrEmpty(this.enableAddFilesToDocs) && Convert.ToBoolean(this.enableAddFilesToDocs));
      this.setCheckboxValue(this.chkAddFilesToConditions, !string.IsNullOrEmpty(this.enableAddFilesToConditions) && Convert.ToBoolean(this.enableAddFilesToConditions));
      this.setCheckboxValue(this.chkAssignDocsToConditions, !string.IsNullOrEmpty(this.enableAssignDocsToConditions) && Convert.ToBoolean(this.enableAssignDocsToConditions));
      this.setCheckboxValue(this.chkMarkConditionsFulfilled, !string.IsNullOrEmpty(this.enableMarkConditionsAsFulfilled) && Convert.ToBoolean(this.enableMarkConditionsAsFulfilled));
      this.setDirtyFlag(false);
    }

    private void populateRegistrationSettings()
    {
      if (this.cbRegistrationFileContact.Items.Contains((object) this.registrationFileContact))
        this.cbRegistrationFileContact.SelectedItem = (object) this.registrationFileContact;
      else
        this.cbRegistrationFileContact.SelectedIndex = -1;
      if (this.cbRegistrationUserGroup.Items.Contains((object) this.registrationUserGroup))
        this.cbRegistrationUserGroup.SelectedItem = (object) this.registrationUserGroup;
      else
        this.cbRegistrationUserGroup.SelectedIndex = -1;
    }

    private void populateSubmittalSettings()
    {
      if (this.cbSubmittalFileContact.Items.Contains((object) this.submittalFileContact))
        this.cbSubmittalFileContact.SelectedItem = (object) this.submittalFileContact;
      else
        this.cbSubmittalFileContact.SelectedIndex = -1;
      if (this.cbSubmittalUserGroup.Items.Contains((object) this.submittalUserGroup))
        this.cbSubmittalUserGroup.SelectedItem = (object) this.submittalUserGroup;
      else
        this.cbSubmittalUserGroup.SelectedIndex = -1;
    }

    private void populateLockRequestSettings()
    {
      if (this.cbLockRequestFileContact.Items.Contains((object) this.lockRequestFileContact))
        this.cbLockRequestFileContact.SelectedItem = (object) this.lockRequestFileContact;
      else
        this.cbLockRequestFileContact.SelectedIndex = -1;
      if (this.cbLockDeskUserGroup.Items.Contains((object) this.lockDeskUserGroup))
        this.cbLockDeskUserGroup.SelectedItem = (object) this.lockDeskUserGroup;
      else
        this.cbLockDeskUserGroup.SelectedIndex = -1;
    }

    private void getCompanySettings()
    {
      this.updateBrokerFile = this.session.ConfigurationManager.GetCompanySetting("TPOWCSITEMNGMNT", "UpdateBrokerFile");
      this.importCreditLiabilities = this.session.ConfigurationManager.GetCompanySetting("TPOWCSITEMNGMNT", "ImportCreditLiabilities");
      this.registrationFileContact = this.session.ConfigurationManager.GetCompanySetting("TPOWCSITEMNGMNT", "RegistrationFileContact");
      this.registrationUserGroup = this.session.ConfigurationManager.GetCompanySetting("TPOWCSITEMNGMNT", "RegistrationUserGroup");
      this.submittalFileContact = this.session.ConfigurationManager.GetCompanySetting("TPOWCSITEMNGMNT", "SubmittalFileContact");
      this.submittalUserGroup = this.session.ConfigurationManager.GetCompanySetting("TPOWCSITEMNGMNT", "SubmittalUserGroup");
      this.lockRequestFileContact = this.session.ConfigurationManager.GetCompanySetting("TPOWCSITEMNGMNT", "LockRequestFileContact");
      this.lockDeskUserGroup = this.session.ConfigurationManager.GetCompanySetting("TPOWCSITEMNGMNT", "LockDeskUserGroup");
      this.enableToCreateDocNames = this.session.ConfigurationManager.GetCompanySetting("TPOWCSITEMNGMNT", "EnableToCreateDocNames");
      this.enableUploadFiles = this.session.ConfigurationManager.GetCompanySetting("TPOWCSITEMNGMNT", "EnableUploadFiles");
      this.enableAddFilesToDocs = this.session.ConfigurationManager.GetCompanySetting("TPOWCSITEMNGMNT", "EnableAddFilesToDocs");
      this.enableAddFilesToConditions = this.session.ConfigurationManager.GetCompanySetting("TPOWCSITEMNGMNT", "EnableAddFilesToConditions");
      this.enableAssignDocsToConditions = this.session.ConfigurationManager.GetCompanySetting("TPOWCSITEMNGMNT", "EnableAssignDocsToConditions");
      this.enableMarkConditionsAsFulfilled = this.session.ConfigurationManager.GetCompanySetting("TPOWCSITEMNGMNT", "EnableMarkConditionsAsFulfilled");
    }

    public override void Reset()
    {
      this.initialPageValue();
      this.setDirtyFlag(false);
    }

    public override void Save()
    {
      if (!this.validateData())
        return;
      this.session.ConfigurationManager.SetCompanySetting("TPOWCSITEMNGMNT", "UpdateBrokerFile", this.updateBrokerFile);
      this.session.ConfigurationManager.SetCompanySetting("TPOWCSITEMNGMNT", "ImportCreditLiabilities", this.importCreditLiabilities);
      this.session.ConfigurationManager.SetCompanySetting("TPOWCSITEMNGMNT", "RegistrationFileContact", this.registrationFileContact);
      this.session.ConfigurationManager.SetCompanySetting("TPOWCSITEMNGMNT", "RegistrationUserGroup", this.registrationUserGroup);
      this.session.ConfigurationManager.SetCompanySetting("TPOWCSITEMNGMNT", "SubmittalFileContact", this.submittalFileContact);
      this.session.ConfigurationManager.SetCompanySetting("TPOWCSITEMNGMNT", "SubmittalUserGroup", this.submittalUserGroup);
      this.session.ConfigurationManager.SetCompanySetting("TPOWCSITEMNGMNT", "LockRequestFileContact", this.lockRequestFileContact);
      this.session.ConfigurationManager.SetCompanySetting("TPOWCSITEMNGMNT", "LockDeskUserGroup", this.lockDeskUserGroup);
      this.session.ConfigurationManager.SetCompanySetting("TPOWCSITEMNGMNT", "EnableToCreateDocNames", this.enableToCreateDocNames);
      this.session.ConfigurationManager.SetCompanySetting("TPOWCSITEMNGMNT", "EnableUploadFiles", this.enableUploadFiles);
      this.session.ConfigurationManager.SetCompanySetting("TPOWCSITEMNGMNT", "EnableAddFilesToDocs", this.enableAddFilesToDocs);
      this.session.ConfigurationManager.SetCompanySetting("TPOWCSITEMNGMNT", "EnableAssignDocsToConditions", this.enableAssignDocsToConditions);
      this.session.ConfigurationManager.SetCompanySetting("TPOWCSITEMNGMNT", "EnableAddFilesToConditions", this.enableAddFilesToConditions);
      this.session.ConfigurationManager.SetCompanySetting("TPOWCSITEMNGMNT", "EnableMarkConditionsAsFulfilled", this.enableMarkConditionsAsFulfilled);
      this.setDirtyFlag(false);
    }

    private bool validateData() => true;

    private void selectRadiobuttonOption(RadioButton rbYes, RadioButton rbNo, string value)
    {
      if (value == "Yes")
        rbYes.Checked = true;
      else
        rbNo.Checked = true;
    }

    private void setCheckboxValue(CheckBox chkbx, bool value) => chkbx.Checked = value;

    private void rbYesUpgrdBrkrFile_CheckedChanged(object sender, EventArgs e)
    {
      this.updateBrokerFile = !this.rbYesUpgrdBrkrFile.Checked ? "No" : "Yes";
      this.setDirtyFlag(true);
    }

    private void rbYesImportCreditLiabilities_CheckedChanged(object sender, EventArgs e)
    {
      this.importCreditLiabilities = !this.rbYesImportCreditLiabilities.Checked ? "No" : "Yes";
      this.setDirtyFlag(true);
    }

    private void cbRegistrationFileContact_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cbRegistrationFileContact.SelectedIndex == -1)
        return;
      this.registrationFileContact = this.cbRegistrationFileContact.SelectedItem.ToString();
      this.setDirtyFlag(true);
    }

    private void cbRegistrationUserGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cbRegistrationUserGroup.SelectedIndex == -1)
        return;
      this.registrationUserGroup = this.cbRegistrationUserGroup.SelectedItem.ToString();
      this.setDirtyFlag(true);
    }

    private void cbSubmittalFileContact_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cbSubmittalFileContact.SelectedIndex == -1)
        return;
      this.submittalFileContact = this.cbSubmittalFileContact.SelectedItem.ToString();
      this.setDirtyFlag(true);
    }

    private void cbSubmittalUserGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cbSubmittalUserGroup.SelectedIndex == -1)
        return;
      this.submittalUserGroup = this.cbSubmittalUserGroup.SelectedItem.ToString();
      this.setDirtyFlag(true);
    }

    private void cbLockRequestFileContact_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cbLockRequestFileContact.SelectedIndex == -1)
        return;
      this.lockRequestFileContact = this.cbLockRequestFileContact.SelectedItem.ToString();
      this.setDirtyFlag(true);
    }

    private void cbLockDeskUserGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cbLockDeskUserGroup.SelectedIndex == -1)
        return;
      this.lockDeskUserGroup = this.cbLockDeskUserGroup.SelectedItem.ToString();
      this.setDirtyFlag(true);
    }

    private void chkbxDocumentandConditions_CheckedChanged(object sender, EventArgs e)
    {
      switch (((Control) sender).Name)
      {
        case "chkDocumentNames":
          this.enableToCreateDocNames = !this.chkDocumentNames.Checked ? "False" : "True";
          break;
        case "chkUploadFiles":
          this.enableUploadFiles = !this.chkUploadFiles.Checked ? "False" : "True";
          break;
        case "chkAddFilesToDocs":
          this.enableAddFilesToDocs = !this.chkAddFilesToDocs.Checked ? "False" : "True";
          break;
        case "chkAddFilesToConditions":
          this.enableAddFilesToConditions = !this.chkAddFilesToConditions.Checked ? "False" : "True";
          break;
        case "chkAssignDocsToConditions":
          this.enableAssignDocsToConditions = !this.chkAssignDocsToConditions.Checked ? "False" : "True";
          break;
        case "chkMarkConditionsFulfilled":
          this.enableMarkConditionsAsFulfilled = !this.chkMarkConditionsFulfilled.Checked ? "False" : "True";
          break;
      }
      this.setDirtyFlag(true);
    }

    string IOnlineHelpTarget.GetHelpTargetName() => "Setup\\TPO WebCenter Site Management";

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.toolTip1 = new ToolTip(this.components);
      this.gcLoanSetup = new GroupContainer();
      this.pnlLoanSetup = new Panel();
      this.rbNoUpgrdBrkrFile = new RadioButton();
      this.rbYesUpgrdBrkrFile = new RadioButton();
      this.lblUpdateBrokerFile = new Label();
      this.gcCreditSettings = new GroupContainer();
      this.pnlCreditSettings = new Panel();
      this.rbNoImportCreditLiabilities = new RadioButton();
      this.rbYesImportCreditLiabilities = new RadioButton();
      this.lblImportCreditLiabilities = new Label();
      this.gvRegistrationSettings = new GroupContainer();
      this.pnlRegistrationSettings = new Panel();
      this.cbRegistrationUserGroup = new ComboBox();
      this.cbRegistrationFileContact = new ComboBox();
      this.label2 = new Label();
      this.label1 = new Label();
      this.gcSubmittalSettings = new GroupContainer();
      this.pnlSubmittalSettings = new Panel();
      this.cbSubmittalUserGroup = new ComboBox();
      this.cbSubmittalFileContact = new ComboBox();
      this.label3 = new Label();
      this.label4 = new Label();
      this.gcLockRequestSettings = new GroupContainer();
      this.pnlLockRequestSettings = new Panel();
      this.cbLockDeskUserGroup = new ComboBox();
      this.cbLockRequestFileContact = new ComboBox();
      this.label5 = new Label();
      this.label6 = new Label();
      this.gcDocumentConditionsSettings = new GroupContainer();
      this.pnlDocAndCondSettings = new Panel();
      this.chkMarkConditionsFulfilled = new CheckBox();
      this.chkAssignDocsToConditions = new CheckBox();
      this.chkAddFilesToConditions = new CheckBox();
      this.chkAddFilesToDocs = new CheckBox();
      this.chkUploadFiles = new CheckBox();
      this.chkDocumentNames = new CheckBox();
      this.gcLoanSetup.SuspendLayout();
      this.pnlLoanSetup.SuspendLayout();
      this.gcCreditSettings.SuspendLayout();
      this.pnlCreditSettings.SuspendLayout();
      this.gvRegistrationSettings.SuspendLayout();
      this.pnlRegistrationSettings.SuspendLayout();
      this.gcSubmittalSettings.SuspendLayout();
      this.pnlSubmittalSettings.SuspendLayout();
      this.gcLockRequestSettings.SuspendLayout();
      this.pnlLockRequestSettings.SuspendLayout();
      this.gcDocumentConditionsSettings.SuspendLayout();
      this.pnlDocAndCondSettings.SuspendLayout();
      this.SuspendLayout();
      this.gcLoanSetup.Controls.Add((Control) this.pnlLoanSetup);
      this.gcLoanSetup.Dock = DockStyle.Top;
      this.gcLoanSetup.HeaderForeColor = SystemColors.ControlText;
      this.gcLoanSetup.Location = new Point(0, 0);
      this.gcLoanSetup.Name = "gcLoanSetup";
      this.gcLoanSetup.Size = new Size(731, 64);
      this.gcLoanSetup.TabIndex = 2;
      this.gcLoanSetup.Text = "Loan Setup";
      this.pnlLoanSetup.Controls.Add((Control) this.rbNoUpgrdBrkrFile);
      this.pnlLoanSetup.Controls.Add((Control) this.rbYesUpgrdBrkrFile);
      this.pnlLoanSetup.Controls.Add((Control) this.lblUpdateBrokerFile);
      this.pnlLoanSetup.Dock = DockStyle.Fill;
      this.pnlLoanSetup.Location = new Point(1, 26);
      this.pnlLoanSetup.Name = "pnlLoanSetup";
      this.pnlLoanSetup.Size = new Size(729, 37);
      this.pnlLoanSetup.TabIndex = 11;
      this.rbNoUpgrdBrkrFile.AutoSize = true;
      this.rbNoUpgrdBrkrFile.Location = new Point(339, 9);
      this.rbNoUpgrdBrkrFile.Name = "rbNoUpgrdBrkrFile";
      this.rbNoUpgrdBrkrFile.Size = new Size(39, 17);
      this.rbNoUpgrdBrkrFile.TabIndex = 15;
      this.rbNoUpgrdBrkrFile.TabStop = true;
      this.rbNoUpgrdBrkrFile.Text = "No";
      this.rbNoUpgrdBrkrFile.UseVisualStyleBackColor = true;
      this.rbNoUpgrdBrkrFile.CheckedChanged += new EventHandler(this.rbYesUpgrdBrkrFile_CheckedChanged);
      this.rbYesUpgrdBrkrFile.AutoSize = true;
      this.rbYesUpgrdBrkrFile.Location = new Point(274, 9);
      this.rbYesUpgrdBrkrFile.Name = "rbYesUpgrdBrkrFile";
      this.rbYesUpgrdBrkrFile.Size = new Size(43, 17);
      this.rbYesUpgrdBrkrFile.TabIndex = 14;
      this.rbYesUpgrdBrkrFile.TabStop = true;
      this.rbYesUpgrdBrkrFile.Text = "Yes";
      this.rbYesUpgrdBrkrFile.UseVisualStyleBackColor = true;
      this.rbYesUpgrdBrkrFile.CheckedChanged += new EventHandler(this.rbYesUpgrdBrkrFile_CheckedChanged);
      this.lblUpdateBrokerFile.AutoSize = true;
      this.lblUpdateBrokerFile.Location = new Point(5, 11);
      this.lblUpdateBrokerFile.Name = "lblUpdateBrokerFile";
      this.lblUpdateBrokerFile.Size = new Size(243, 13);
      this.lblUpdateBrokerFile.TabIndex = 13;
      this.lblUpdateBrokerFile.Text = "Update Broker File Contact with Originator Details:";
      this.gcCreditSettings.Controls.Add((Control) this.pnlCreditSettings);
      this.gcCreditSettings.Dock = DockStyle.Top;
      this.gcCreditSettings.HeaderForeColor = SystemColors.ControlText;
      this.gcCreditSettings.Location = new Point(0, 64);
      this.gcCreditSettings.Name = "gcCreditSettings";
      this.gcCreditSettings.Size = new Size(731, 66);
      this.gcCreditSettings.TabIndex = 12;
      this.gcCreditSettings.Text = "Credit Settings";
      this.pnlCreditSettings.Controls.Add((Control) this.rbNoImportCreditLiabilities);
      this.pnlCreditSettings.Controls.Add((Control) this.rbYesImportCreditLiabilities);
      this.pnlCreditSettings.Controls.Add((Control) this.lblImportCreditLiabilities);
      this.pnlCreditSettings.Dock = DockStyle.Fill;
      this.pnlCreditSettings.Location = new Point(1, 26);
      this.pnlCreditSettings.Name = "pnlCreditSettings";
      this.pnlCreditSettings.Size = new Size(729, 39);
      this.pnlCreditSettings.TabIndex = 11;
      this.rbNoImportCreditLiabilities.AutoSize = true;
      this.rbNoImportCreditLiabilities.Location = new Point(339, 13);
      this.rbNoImportCreditLiabilities.Name = "rbNoImportCreditLiabilities";
      this.rbNoImportCreditLiabilities.Size = new Size(39, 17);
      this.rbNoImportCreditLiabilities.TabIndex = 15;
      this.rbNoImportCreditLiabilities.TabStop = true;
      this.rbNoImportCreditLiabilities.Text = "No";
      this.rbNoImportCreditLiabilities.UseVisualStyleBackColor = true;
      this.rbNoImportCreditLiabilities.CheckedChanged += new EventHandler(this.rbYesImportCreditLiabilities_CheckedChanged);
      this.rbYesImportCreditLiabilities.AutoSize = true;
      this.rbYesImportCreditLiabilities.Location = new Point(274, 13);
      this.rbYesImportCreditLiabilities.Name = "rbYesImportCreditLiabilities";
      this.rbYesImportCreditLiabilities.Size = new Size(43, 17);
      this.rbYesImportCreditLiabilities.TabIndex = 14;
      this.rbYesImportCreditLiabilities.TabStop = true;
      this.rbYesImportCreditLiabilities.Text = "Yes";
      this.rbYesImportCreditLiabilities.UseVisualStyleBackColor = true;
      this.rbYesImportCreditLiabilities.CheckedChanged += new EventHandler(this.rbYesImportCreditLiabilities_CheckedChanged);
      this.lblImportCreditLiabilities.AutoSize = true;
      this.lblImportCreditLiabilities.Location = new Point(5, 13);
      this.lblImportCreditLiabilities.Name = "lblImportCreditLiabilities";
      this.lblImportCreditLiabilities.Size = new Size(166, 13);
      this.lblImportCreditLiabilities.TabIndex = 13;
      this.lblImportCreditLiabilities.Text = "Import Zero-Dollar Credit Liabilities";
      this.gvRegistrationSettings.Controls.Add((Control) this.pnlRegistrationSettings);
      this.gvRegistrationSettings.Dock = DockStyle.Top;
      this.gvRegistrationSettings.HeaderForeColor = SystemColors.ControlText;
      this.gvRegistrationSettings.Location = new Point(0, 130);
      this.gvRegistrationSettings.Name = "gvRegistrationSettings";
      this.gvRegistrationSettings.Size = new Size(731, 99);
      this.gvRegistrationSettings.TabIndex = 13;
      this.gvRegistrationSettings.Text = "Registration Settings";
      this.pnlRegistrationSettings.Controls.Add((Control) this.cbRegistrationUserGroup);
      this.pnlRegistrationSettings.Controls.Add((Control) this.cbRegistrationFileContact);
      this.pnlRegistrationSettings.Controls.Add((Control) this.label2);
      this.pnlRegistrationSettings.Controls.Add((Control) this.label1);
      this.pnlRegistrationSettings.Dock = DockStyle.Fill;
      this.pnlRegistrationSettings.Location = new Point(1, 26);
      this.pnlRegistrationSettings.Name = "pnlRegistrationSettings";
      this.pnlRegistrationSettings.Size = new Size(729, 72);
      this.pnlRegistrationSettings.TabIndex = 11;
      this.cbRegistrationUserGroup.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbRegistrationUserGroup.FormattingEnabled = true;
      this.cbRegistrationUserGroup.Location = new Point(274, 40);
      this.cbRegistrationUserGroup.Name = "cbRegistrationUserGroup";
      this.cbRegistrationUserGroup.Size = new Size(221, 21);
      this.cbRegistrationUserGroup.TabIndex = 17;
      this.cbRegistrationUserGroup.SelectedIndexChanged += new EventHandler(this.cbRegistrationUserGroup_SelectedIndexChanged);
      this.cbRegistrationFileContact.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbRegistrationFileContact.FormattingEnabled = true;
      this.cbRegistrationFileContact.Location = new Point(274, 10);
      this.cbRegistrationFileContact.Name = "cbRegistrationFileContact";
      this.cbRegistrationFileContact.Size = new Size(221, 21);
      this.cbRegistrationFileContact.TabIndex = 16;
      this.cbRegistrationFileContact.SelectedIndexChanged += new EventHandler(this.cbRegistrationFileContact_SelectedIndexChanged);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(5, 43);
      this.label2.Name = "label2";
      this.label2.Size = new Size(120, 13);
      this.label2.TabIndex = 14;
      this.label2.Text = "Registration User Group";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(5, 13);
      this.label1.Name = "label1";
      this.label1.Size = new Size(122, 13);
      this.label1.TabIndex = 13;
      this.label1.Text = "Registration File Contact";
      this.gcSubmittalSettings.Controls.Add((Control) this.pnlSubmittalSettings);
      this.gcSubmittalSettings.Dock = DockStyle.Top;
      this.gcSubmittalSettings.HeaderForeColor = SystemColors.ControlText;
      this.gcSubmittalSettings.Location = new Point(0, 229);
      this.gcSubmittalSettings.Name = "gcSubmittalSettings";
      this.gcSubmittalSettings.Size = new Size(731, 99);
      this.gcSubmittalSettings.TabIndex = 14;
      this.gcSubmittalSettings.Text = "Submittal Settings";
      this.pnlSubmittalSettings.Controls.Add((Control) this.cbSubmittalUserGroup);
      this.pnlSubmittalSettings.Controls.Add((Control) this.cbSubmittalFileContact);
      this.pnlSubmittalSettings.Controls.Add((Control) this.label3);
      this.pnlSubmittalSettings.Controls.Add((Control) this.label4);
      this.pnlSubmittalSettings.Dock = DockStyle.Fill;
      this.pnlSubmittalSettings.Location = new Point(1, 26);
      this.pnlSubmittalSettings.Name = "pnlSubmittalSettings";
      this.pnlSubmittalSettings.Size = new Size(729, 72);
      this.pnlSubmittalSettings.TabIndex = 11;
      this.cbSubmittalUserGroup.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbSubmittalUserGroup.FormattingEnabled = true;
      this.cbSubmittalUserGroup.Location = new Point(274, 40);
      this.cbSubmittalUserGroup.Name = "cbSubmittalUserGroup";
      this.cbSubmittalUserGroup.Size = new Size(221, 21);
      this.cbSubmittalUserGroup.TabIndex = 17;
      this.cbSubmittalUserGroup.SelectedIndexChanged += new EventHandler(this.cbSubmittalUserGroup_SelectedIndexChanged);
      this.cbSubmittalFileContact.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbSubmittalFileContact.FormattingEnabled = true;
      this.cbSubmittalFileContact.Location = new Point(274, 10);
      this.cbSubmittalFileContact.Name = "cbSubmittalFileContact";
      this.cbSubmittalFileContact.Size = new Size(221, 21);
      this.cbSubmittalFileContact.TabIndex = 16;
      this.cbSubmittalFileContact.SelectedIndexChanged += new EventHandler(this.cbSubmittalFileContact_SelectedIndexChanged);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(5, 43);
      this.label3.Name = "label3";
      this.label3.Size = new Size(107, 13);
      this.label3.TabIndex = 14;
      this.label3.Text = "Submittal User Group";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(5, 13);
      this.label4.Name = "label4";
      this.label4.Size = new Size(109, 13);
      this.label4.TabIndex = 13;
      this.label4.Text = "Submittal File Contact";
      this.gcLockRequestSettings.Controls.Add((Control) this.pnlLockRequestSettings);
      this.gcLockRequestSettings.Dock = DockStyle.Top;
      this.gcLockRequestSettings.HeaderForeColor = SystemColors.ControlText;
      this.gcLockRequestSettings.Location = new Point(0, 328);
      this.gcLockRequestSettings.Name = "gcLockRequestSettings";
      this.gcLockRequestSettings.Size = new Size(731, 99);
      this.gcLockRequestSettings.TabIndex = 15;
      this.gcLockRequestSettings.Text = "Lock Request Settings";
      this.pnlLockRequestSettings.Controls.Add((Control) this.cbLockDeskUserGroup);
      this.pnlLockRequestSettings.Controls.Add((Control) this.cbLockRequestFileContact);
      this.pnlLockRequestSettings.Controls.Add((Control) this.label5);
      this.pnlLockRequestSettings.Controls.Add((Control) this.label6);
      this.pnlLockRequestSettings.Dock = DockStyle.Fill;
      this.pnlLockRequestSettings.Location = new Point(1, 26);
      this.pnlLockRequestSettings.Name = "pnlLockRequestSettings";
      this.pnlLockRequestSettings.Size = new Size(729, 72);
      this.pnlLockRequestSettings.TabIndex = 11;
      this.cbLockDeskUserGroup.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbLockDeskUserGroup.FormattingEnabled = true;
      this.cbLockDeskUserGroup.Location = new Point(274, 40);
      this.cbLockDeskUserGroup.Name = "cbLockDeskUserGroup";
      this.cbLockDeskUserGroup.Size = new Size(221, 21);
      this.cbLockDeskUserGroup.TabIndex = 17;
      this.cbLockDeskUserGroup.SelectedIndexChanged += new EventHandler(this.cbLockDeskUserGroup_SelectedIndexChanged);
      this.cbLockRequestFileContact.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbLockRequestFileContact.FormattingEnabled = true;
      this.cbLockRequestFileContact.Location = new Point(274, 10);
      this.cbLockRequestFileContact.Name = "cbLockRequestFileContact";
      this.cbLockRequestFileContact.Size = new Size(221, 21);
      this.cbLockRequestFileContact.TabIndex = 16;
      this.cbLockRequestFileContact.SelectedIndexChanged += new EventHandler(this.cbLockRequestFileContact_SelectedIndexChanged);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(5, 43);
      this.label5.Name = "label5";
      this.label5.Size = new Size(116, 13);
      this.label5.TabIndex = 14;
      this.label5.Text = "Lock Desk User Group";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(5, 13);
      this.label6.Name = "label6";
      this.label6.Size = new Size(133, 13);
      this.label6.TabIndex = 13;
      this.label6.Text = "Lock Request File Contact";
      this.gcDocumentConditionsSettings.Controls.Add((Control) this.pnlDocAndCondSettings);
      this.gcDocumentConditionsSettings.Dock = DockStyle.Top;
      this.gcDocumentConditionsSettings.HeaderForeColor = SystemColors.ControlText;
      this.gcDocumentConditionsSettings.Location = new Point(0, 427);
      this.gcDocumentConditionsSettings.Name = "gcDocumentConditionsSettings";
      this.gcDocumentConditionsSettings.Size = new Size(731, 179);
      this.gcDocumentConditionsSettings.TabIndex = 16;
      this.gcDocumentConditionsSettings.Text = "Document and Conditions Settings";
      this.pnlDocAndCondSettings.Controls.Add((Control) this.chkMarkConditionsFulfilled);
      this.pnlDocAndCondSettings.Controls.Add((Control) this.chkAssignDocsToConditions);
      this.pnlDocAndCondSettings.Controls.Add((Control) this.chkAddFilesToConditions);
      this.pnlDocAndCondSettings.Controls.Add((Control) this.chkAddFilesToDocs);
      this.pnlDocAndCondSettings.Controls.Add((Control) this.chkUploadFiles);
      this.pnlDocAndCondSettings.Controls.Add((Control) this.chkDocumentNames);
      this.pnlDocAndCondSettings.Dock = DockStyle.Fill;
      this.pnlDocAndCondSettings.Location = new Point(1, 26);
      this.pnlDocAndCondSettings.Name = "pnlDocAndCondSettings";
      this.pnlDocAndCondSettings.Size = new Size(729, 152);
      this.pnlDocAndCondSettings.TabIndex = 11;
      this.chkMarkConditionsFulfilled.AutoSize = true;
      this.chkMarkConditionsFulfilled.Location = new Point(8, 128);
      this.chkMarkConditionsFulfilled.Name = "chkMarkConditionsFulfilled";
      this.chkMarkConditionsFulfilled.Size = new Size(246, 17);
      this.chkMarkConditionsFulfilled.TabIndex = 5;
      this.chkMarkConditionsFulfilled.Text = "Enable originator to mark conditions as Fulfilled";
      this.chkMarkConditionsFulfilled.TextAlign = ContentAlignment.BottomRight;
      this.chkMarkConditionsFulfilled.UseVisualStyleBackColor = true;
      this.chkMarkConditionsFulfilled.CheckedChanged += new EventHandler(this.chkbxDocumentandConditions_CheckedChanged);
      this.chkAssignDocsToConditions.AutoSize = true;
      this.chkAssignDocsToConditions.Location = new Point(8, 105);
      this.chkAssignDocsToConditions.Name = "chkAssignDocsToConditions";
      this.chkAssignDocsToConditions.Size = new Size(268, 17);
      this.chkAssignDocsToConditions.TabIndex = 4;
      this.chkAssignDocsToConditions.Text = "Enable originator to assign documents to conditions";
      this.chkAssignDocsToConditions.TextAlign = ContentAlignment.BottomRight;
      this.chkAssignDocsToConditions.UseVisualStyleBackColor = true;
      this.chkAssignDocsToConditions.CheckedChanged += new EventHandler(this.chkbxDocumentandConditions_CheckedChanged);
      this.chkAddFilesToConditions.AutoSize = true;
      this.chkAddFilesToConditions.Location = new Point(8, 82);
      this.chkAddFilesToConditions.Name = "chkAddFilesToConditions";
      this.chkAddFilesToConditions.Size = new Size(222, 17);
      this.chkAddFilesToConditions.TabIndex = 3;
      this.chkAddFilesToConditions.Text = "Enable originator to add files to conditions";
      this.chkAddFilesToConditions.TextAlign = ContentAlignment.BottomRight;
      this.chkAddFilesToConditions.UseVisualStyleBackColor = true;
      this.chkAddFilesToConditions.CheckedChanged += new EventHandler(this.chkbxDocumentandConditions_CheckedChanged);
      this.chkAddFilesToDocs.AutoSize = true;
      this.chkAddFilesToDocs.Location = new Point(8, 59);
      this.chkAddFilesToDocs.Name = "chkAddFilesToDocs";
      this.chkAddFilesToDocs.Size = new Size(226, 17);
      this.chkAddFilesToDocs.TabIndex = 2;
      this.chkAddFilesToDocs.Text = "Enable originator to add files to documents";
      this.chkAddFilesToDocs.TextAlign = ContentAlignment.BottomRight;
      this.chkAddFilesToDocs.UseVisualStyleBackColor = true;
      this.chkAddFilesToDocs.CheckedChanged += new EventHandler(this.chkbxDocumentandConditions_CheckedChanged);
      this.chkUploadFiles.AutoSize = true;
      this.chkUploadFiles.Location = new Point(8, 36);
      this.chkUploadFiles.Name = "chkUploadFiles";
      this.chkUploadFiles.Size = new Size(396, 17);
      this.chkUploadFiles.TabIndex = 1;
      this.chkUploadFiles.Text = "Enable originator to upload files without assigning to documents and conditions";
      this.chkUploadFiles.TextAlign = ContentAlignment.BottomRight;
      this.chkUploadFiles.UseVisualStyleBackColor = true;
      this.chkUploadFiles.CheckedChanged += new EventHandler(this.chkbxDocumentandConditions_CheckedChanged);
      this.chkDocumentNames.AutoSize = true;
      this.chkDocumentNames.Location = new Point(8, 13);
      this.chkDocumentNames.Name = "chkDocumentNames";
      this.chkDocumentNames.Size = new Size(234, 17);
      this.chkDocumentNames.TabIndex = 0;
      this.chkDocumentNames.Text = "Enable originator to create document names";
      this.chkDocumentNames.TextAlign = ContentAlignment.BottomRight;
      this.chkDocumentNames.UseVisualStyleBackColor = true;
      this.chkDocumentNames.CheckedChanged += new EventHandler(this.chkbxDocumentandConditions_CheckedChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoScroll = true;
      this.Controls.Add((Control) this.gcDocumentConditionsSettings);
      this.Controls.Add((Control) this.gcLockRequestSettings);
      this.Controls.Add((Control) this.gcSubmittalSettings);
      this.Controls.Add((Control) this.gvRegistrationSettings);
      this.Controls.Add((Control) this.gcCreditSettings);
      this.Controls.Add((Control) this.gcLoanSetup);
      this.Name = nameof (TPOWCSiteMngmnt);
      this.Size = new Size(731, 609);
      this.gcLoanSetup.ResumeLayout(false);
      this.pnlLoanSetup.ResumeLayout(false);
      this.pnlLoanSetup.PerformLayout();
      this.gcCreditSettings.ResumeLayout(false);
      this.pnlCreditSettings.ResumeLayout(false);
      this.pnlCreditSettings.PerformLayout();
      this.gvRegistrationSettings.ResumeLayout(false);
      this.pnlRegistrationSettings.ResumeLayout(false);
      this.pnlRegistrationSettings.PerformLayout();
      this.gcSubmittalSettings.ResumeLayout(false);
      this.pnlSubmittalSettings.ResumeLayout(false);
      this.pnlSubmittalSettings.PerformLayout();
      this.gcLockRequestSettings.ResumeLayout(false);
      this.pnlLockRequestSettings.ResumeLayout(false);
      this.pnlLockRequestSettings.PerformLayout();
      this.gcDocumentConditionsSettings.ResumeLayout(false);
      this.pnlDocAndCondSettings.ResumeLayout(false);
      this.pnlDocAndCondSettings.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
