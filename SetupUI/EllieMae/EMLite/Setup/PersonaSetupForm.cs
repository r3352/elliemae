// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PersonaSetupForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.DataDocs;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.RemotingServices.Acl;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class PersonaSetupForm : SettingsUserControl
  {
    private IContainer components;
    private Panel panelPersonaSettings;
    private ListView lvPersona;
    private PersonaSettingsMainForm personaSettingsForm;
    public ColumnHeader chPersonaName;
    private ContextMenu contextMenuPersona;
    private ToolTip toolTipPersona;
    private GroupContainer gcCreatePersona;
    private Splitter splitter1;
    private StandardIconButton stdIconBtnDown;
    private StandardIconButton stdIconBtnUp;
    private StandardIconButton stdIconBtnDuplicate;
    private StandardIconButton stdIconBtnAdd;
    private StandardIconButton stdIconBtnDelete;
    private MenuItem menuItemNew;
    private MenuItem menuItemRename;
    private MenuItem menuItemDuplicate;
    private MenuItem menuItemUp;
    private MenuItem menuItemDown;
    private MenuItem menuItemDelete;
    private Timer lvPersonaSelectedIndexChangedTimer;
    private Persona[] personas;
    private FeaturesAclManager aclMgr;
    private MenuItem menuItemMakeInternal;
    private MenuItem menuItemMakeExternal;
    private MenuItem menuItemMakeInternalExternal;
    private ColumnHeader chPersonaType;
    private Sessions.Session session;
    private bool isTPOMVP;
    private int currentPersonaId = -1;
    private string currentPersonaName = "";
    private bool lvPersonaSelectedIndexChangedTimerIsRunning;
    private int lvPersonaCurrSelectedIdx = -1;
    private bool checkSaveNeeded = true;

    public PersonaSetupForm(SetUpContainer setupContainer)
      : this(Session.DefaultInstance, setupContainer, false)
    {
    }

    public PersonaSetupForm(
      Sessions.Session session,
      SetUpContainer setupContainer,
      bool allowMultiSelect)
      : base(setupContainer)
    {
      this.session = session;
      this.InitializeComponent();
      this.isTPOMVP = session.ConfigurationManager.CheckIfAnyTPOSiteExists();
      this.aclMgr = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
      this.personas = this.getPersonas();
      this.personaSettingsForm = new PersonaSettingsMainForm((IWin32Window) this, this.session, this.personas[0].ID);
      this.personaSettingsForm.TopLevel = false;
      this.personaSettingsForm.Dock = DockStyle.Fill;
      this.personaSettingsForm.Visible = true;
      this.personaSettingsForm.FormBorderStyle = FormBorderStyle.None;
      this.panelPersonaSettings.Controls.Add((Control) this.personaSettingsForm);
      if (this.isTPOMVP)
        this.addPersonaTypeColumn();
      this.loadLVPersona(this.personas);
      if (this.session.EncompassEdition == EncompassEdition.Banker)
      {
        this.setTPOMVPContextMenu();
        this.lvPersona.ContextMenu = this.contextMenuPersona;
      }
      else
      {
        this.stdIconBtnAdd.Visible = false;
        this.stdIconBtnDelete.Visible = false;
        this.stdIconBtnDown.Visible = false;
        this.stdIconBtnDuplicate.Visible = false;
        this.stdIconBtnUp.Visible = false;
        this.lvPersona.LabelEdit = false;
        this.gcCreatePersona.Text = "1. Select a persona";
        this.gcCreatePersona.MinimumSize = new Size(this.gcCreatePersona.MinimumSize.Width / 2, this.gcCreatePersona.MinimumSize.Height);
        this.gcCreatePersona.Size = new Size(this.gcCreatePersona.Size.Width / 2, this.gcCreatePersona.Size.Height);
      }
      if (!this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_PersonasEdit))
      {
        this.lvPersona.ContextMenu = (ContextMenu) null;
        this.stdIconBtnAdd.Visible = false;
        this.stdIconBtnDelete.Visible = false;
        this.stdIconBtnDown.Visible = false;
        this.stdIconBtnDuplicate.Visible = false;
        this.stdIconBtnUp.Visible = false;
        this.lvPersona.LabelEdit = false;
        this.gcCreatePersona.Text = "1. Select a persona";
        GroupContainer gcCreatePersona = this.gcCreatePersona;
        Size minimumSize = this.gcCreatePersona.MinimumSize;
        int width = minimumSize.Width / 2;
        minimumSize = this.gcCreatePersona.MinimumSize;
        int height = minimumSize.Height;
        Size size = new Size(width, height);
        gcCreatePersona.MinimumSize = size;
        this.gcCreatePersona.Size = new Size(this.gcCreatePersona.Size.Width / 2, this.gcCreatePersona.Size.Height);
      }
      this.lvPersona.MultiSelect = allowMultiSelect;
    }

    private void setTPOMVPContextMenu()
    {
      if (this.isTPOMVP)
        return;
      this.menuItemMakeInternal.Visible = false;
      this.menuItemMakeExternal.Visible = false;
      this.menuItemMakeInternalExternal.Visible = false;
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
      this.panelPersonaSettings = new Panel();
      this.lvPersona = new ListView();
      this.chPersonaName = new ColumnHeader();
      this.chPersonaType = new ColumnHeader();
      this.contextMenuPersona = new ContextMenu();
      this.menuItemNew = new MenuItem();
      this.menuItemRename = new MenuItem();
      this.menuItemDuplicate = new MenuItem();
      this.menuItemUp = new MenuItem();
      this.menuItemDown = new MenuItem();
      this.menuItemMakeInternal = new MenuItem();
      this.menuItemMakeExternal = new MenuItem();
      this.menuItemMakeInternalExternal = new MenuItem();
      this.menuItemDelete = new MenuItem();
      this.toolTipPersona = new ToolTip(this.components);
      this.stdIconBtnDelete = new StandardIconButton();
      this.stdIconBtnAdd = new StandardIconButton();
      this.stdIconBtnDuplicate = new StandardIconButton();
      this.stdIconBtnUp = new StandardIconButton();
      this.stdIconBtnDown = new StandardIconButton();
      this.gcCreatePersona = new GroupContainer();
      this.splitter1 = new Splitter();
      this.lvPersonaSelectedIndexChangedTimer = new Timer(this.components);
      ((ISupportInitialize) this.stdIconBtnDelete).BeginInit();
      ((ISupportInitialize) this.stdIconBtnAdd).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDuplicate).BeginInit();
      ((ISupportInitialize) this.stdIconBtnUp).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDown).BeginInit();
      this.gcCreatePersona.SuspendLayout();
      this.SuspendLayout();
      this.panelPersonaSettings.Dock = DockStyle.Fill;
      this.panelPersonaSettings.Location = new Point(243, 0);
      this.panelPersonaSettings.Name = "panelPersonaSettings";
      this.panelPersonaSettings.Size = new Size(350, 371);
      this.panelPersonaSettings.TabIndex = 9;
      this.lvPersona.Alignment = ListViewAlignment.Default;
      this.lvPersona.AutoArrange = false;
      this.lvPersona.BorderStyle = BorderStyle.None;
      this.lvPersona.Columns.AddRange(new ColumnHeader[1]
      {
        this.chPersonaName
      });
      this.lvPersona.Dock = DockStyle.Fill;
      this.lvPersona.FullRowSelect = true;
      this.lvPersona.HeaderStyle = ColumnHeaderStyle.None;
      this.lvPersona.HideSelection = false;
      this.lvPersona.LabelEdit = true;
      this.lvPersona.Location = new Point(1, 26);
      this.lvPersona.MultiSelect = false;
      this.lvPersona.Name = "lvPersona";
      this.lvPersona.Size = new Size(238, 344);
      this.lvPersona.TabIndex = 11;
      this.lvPersona.UseCompatibleStateImageBehavior = false;
      this.lvPersona.View = View.Details;
      this.lvPersona.AfterLabelEdit += new LabelEditEventHandler(this.lvPersona_AfterLabelEdit);
      this.lvPersona.SelectedIndexChanged += new EventHandler(this.lvPersona_SelectedIndexChanged);
      this.chPersonaName.Text = "";
      this.chPersonaName.Width = 208;
      this.chPersonaType.Text = "Internal/External";
      this.chPersonaType.Width = 150;
      this.contextMenuPersona.MenuItems.AddRange(new MenuItem[9]
      {
        this.menuItemNew,
        this.menuItemRename,
        this.menuItemDuplicate,
        this.menuItemUp,
        this.menuItemDown,
        this.menuItemMakeInternal,
        this.menuItemMakeExternal,
        this.menuItemMakeInternalExternal,
        this.menuItemDelete
      });
      this.contextMenuPersona.Popup += new EventHandler(this.contextMenuPersona_Popup);
      this.menuItemNew.Index = 0;
      this.menuItemNew.Text = "New";
      this.menuItemNew.Click += new EventHandler(this.btnAddPersona_Click);
      this.menuItemRename.Index = 1;
      this.menuItemRename.Text = "Rename";
      this.menuItemRename.Click += new EventHandler(this.menuItemRename_Click);
      this.menuItemDuplicate.Index = 2;
      this.menuItemDuplicate.Text = "Duplicate";
      this.menuItemDuplicate.Click += new EventHandler(this.btnCopyPersona_Click);
      this.menuItemUp.Index = 3;
      this.menuItemUp.Text = "Move Up";
      this.menuItemUp.Click += new EventHandler(this.picBoxUp_Click);
      this.menuItemDown.Index = 4;
      this.menuItemDown.Text = "Move Down";
      this.menuItemDown.Click += new EventHandler(this.picBoxDown_Click);
      this.menuItemMakeInternal.Index = 5;
      this.menuItemMakeInternal.Text = "Make Internal";
      this.menuItemMakeInternal.Click += new EventHandler(this.menuItemMakeInternal_Click);
      this.menuItemMakeExternal.Index = 6;
      this.menuItemMakeExternal.Text = "Make External";
      this.menuItemMakeExternal.Click += new EventHandler(this.menuItemMakeExternal_Click);
      this.menuItemMakeInternalExternal.Index = 7;
      this.menuItemMakeInternalExternal.Text = "Make Both Internal && External";
      this.menuItemMakeInternalExternal.Click += new EventHandler(this.menuItemMakeInternalExternal_Click);
      this.menuItemDelete.Index = 8;
      this.menuItemDelete.Text = "Delete";
      this.menuItemDelete.Click += new EventHandler(this.btnDeletePersona_Click);
      this.toolTipPersona.AutoPopDelay = 5000;
      this.toolTipPersona.InitialDelay = 100;
      this.toolTipPersona.ReshowDelay = 100;
      this.stdIconBtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDelete.BackColor = Color.Transparent;
      this.stdIconBtnDelete.Location = new Point(219, 5);
      this.stdIconBtnDelete.MouseDownImage = (Image) null;
      this.stdIconBtnDelete.Name = "stdIconBtnDelete";
      this.stdIconBtnDelete.Size = new Size(16, 16);
      this.stdIconBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDelete.TabIndex = 12;
      this.stdIconBtnDelete.TabStop = false;
      this.toolTipPersona.SetToolTip((Control) this.stdIconBtnDelete, "Delete Persona");
      this.stdIconBtnDelete.Click += new EventHandler(this.btnDeletePersona_Click);
      this.stdIconBtnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnAdd.BackColor = Color.Transparent;
      this.stdIconBtnAdd.Location = new Point(135, 5);
      this.stdIconBtnAdd.MouseDownImage = (Image) null;
      this.stdIconBtnAdd.Name = "stdIconBtnAdd";
      this.stdIconBtnAdd.Size = new Size(16, 16);
      this.stdIconBtnAdd.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnAdd.TabIndex = 13;
      this.stdIconBtnAdd.TabStop = false;
      this.toolTipPersona.SetToolTip((Control) this.stdIconBtnAdd, "Add Persona");
      this.stdIconBtnAdd.Click += new EventHandler(this.btnAddPersona_Click);
      this.stdIconBtnDuplicate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDuplicate.BackColor = Color.Transparent;
      this.stdIconBtnDuplicate.Location = new Point(156, 5);
      this.stdIconBtnDuplicate.MouseDownImage = (Image) null;
      this.stdIconBtnDuplicate.Name = "stdIconBtnDuplicate";
      this.stdIconBtnDuplicate.Size = new Size(16, 16);
      this.stdIconBtnDuplicate.StandardButtonType = StandardIconButton.ButtonType.DuplicateButton;
      this.stdIconBtnDuplicate.TabIndex = 14;
      this.stdIconBtnDuplicate.TabStop = false;
      this.toolTipPersona.SetToolTip((Control) this.stdIconBtnDuplicate, "Duplicate Persona");
      this.stdIconBtnDuplicate.Click += new EventHandler(this.btnCopyPersona_Click);
      this.stdIconBtnUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnUp.BackColor = Color.Transparent;
      this.stdIconBtnUp.Location = new Point(177, 5);
      this.stdIconBtnUp.MouseDownImage = (Image) null;
      this.stdIconBtnUp.Name = "stdIconBtnUp";
      this.stdIconBtnUp.Size = new Size(16, 16);
      this.stdIconBtnUp.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.stdIconBtnUp.TabIndex = 15;
      this.stdIconBtnUp.TabStop = false;
      this.toolTipPersona.SetToolTip((Control) this.stdIconBtnUp, "Move Persona Up");
      this.stdIconBtnUp.Click += new EventHandler(this.picBoxUp_Click);
      this.stdIconBtnDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDown.BackColor = Color.Transparent;
      this.stdIconBtnDown.Location = new Point(198, 5);
      this.stdIconBtnDown.MouseDownImage = (Image) null;
      this.stdIconBtnDown.Name = "stdIconBtnDown";
      this.stdIconBtnDown.Size = new Size(16, 16);
      this.stdIconBtnDown.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.stdIconBtnDown.TabIndex = 16;
      this.stdIconBtnDown.TabStop = false;
      this.toolTipPersona.SetToolTip((Control) this.stdIconBtnDown, "Move Persona Down");
      this.stdIconBtnDown.Click += new EventHandler(this.picBoxDown_Click);
      this.gcCreatePersona.Controls.Add((Control) this.stdIconBtnDown);
      this.gcCreatePersona.Controls.Add((Control) this.stdIconBtnUp);
      this.gcCreatePersona.Controls.Add((Control) this.stdIconBtnDuplicate);
      this.gcCreatePersona.Controls.Add((Control) this.stdIconBtnAdd);
      this.gcCreatePersona.Controls.Add((Control) this.stdIconBtnDelete);
      this.gcCreatePersona.Controls.Add((Control) this.lvPersona);
      this.gcCreatePersona.Dock = DockStyle.Left;
      this.gcCreatePersona.HeaderForeColor = SystemColors.ControlText;
      this.gcCreatePersona.Location = new Point(0, 0);
      this.gcCreatePersona.MinimumSize = new Size(120, 50);
      this.gcCreatePersona.Name = "gcCreatePersona";
      this.gcCreatePersona.Size = new Size(240, 371);
      this.gcCreatePersona.TabIndex = 5;
      this.gcCreatePersona.Text = "1. Create a persona.";
      this.splitter1.Location = new Point(240, 0);
      this.splitter1.Name = "splitter1";
      this.splitter1.Size = new Size(3, 371);
      this.splitter1.TabIndex = 6;
      this.splitter1.TabStop = false;
      this.lvPersonaSelectedIndexChangedTimer.Tick += new EventHandler(this.lvGroupSelectedIndexChangedTimer_Tick);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.panelPersonaSettings);
      this.Controls.Add((Control) this.splitter1);
      this.Controls.Add((Control) this.gcCreatePersona);
      this.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (PersonaSetupForm);
      this.Size = new Size(593, 371);
      this.BackColorChanged += new EventHandler(this.PersonaSetupForm_BackColorChanged);
      ((ISupportInitialize) this.stdIconBtnDelete).EndInit();
      ((ISupportInitialize) this.stdIconBtnAdd).EndInit();
      ((ISupportInitialize) this.stdIconBtnDuplicate).EndInit();
      ((ISupportInitialize) this.stdIconBtnUp).EndInit();
      ((ISupportInitialize) this.stdIconBtnDown).EndInit();
      this.gcCreatePersona.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public override void Save() => this.personaSettingsForm.Save();

    public override void Reset() => this.personaSettingsForm.Reset();

    public override bool IsDirty => this.personaSettingsForm.IsDirty();

    public override bool IsValid => this.personaSettingsForm.IsValid();

    private int CurrentPersonaID
    {
      get => this.currentPersonaId;
      set
      {
        this.currentPersonaId = value;
        if (this.personaSettingsForm == null)
          return;
        this.personaSettingsForm.CurrentPersonaID = value;
        this.personaSettingsForm.CurrentPersonaName = this.currentPersonaName;
      }
    }

    private void loadLVPersona(Persona[] personas)
    {
      this.lvPersona.BeginUpdate();
      this.lvPersona.Items.Clear();
      for (int index = 0; index < personas.Length; ++index)
      {
        ListViewItem listViewItem = new ListViewItem(personas[index].Name);
        if (this.isTPOMVP)
          listViewItem.SubItems.Add(this.getPersonaTypeDescription(personas[index]));
        listViewItem.Tag = (object) personas[index];
        if (index == 0)
        {
          listViewItem.Selected = true;
          this.CurrentPersonaID = personas[index].ID;
        }
        this.lvPersona.Items.Add(listViewItem);
      }
      this.lvPersona.EndUpdate();
    }

    private void addPersonaTypeColumn()
    {
      this.lvPersona.HeaderStyle = ColumnHeaderStyle.Clickable;
      this.lvPersona.Columns.Add(this.chPersonaType);
      this.chPersonaName.Text = "Persona Name";
      this.chPersonaName.Width = 150;
    }

    private Persona[] getPersonas()
    {
      Persona[] allPersonas = this.session.PersonaManager.GetAllPersonas();
      ArrayList arrayList = new ArrayList();
      foreach (Persona persona in allPersonas)
      {
        if (persona.Name != "Administrator" && persona.ID != 1 && persona.Name != "Super Administrator" && persona.ID != 0)
          arrayList.Add((object) persona);
      }
      return (Persona[]) arrayList.ToArray(typeof (Persona));
    }

    private string[] GetProviderCompanyCodes()
    {
      List<string> stringList = new List<string>();
      stringList.AddRange(new DataDocsServiceHelper(this.session).GetInvestorsList().Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (s => s.Category.Equals(SelectInvestorsPage.InvestorCategory, StringComparison.InvariantCultureIgnoreCase))).Select<PartnerResponseBody, string>((Func<PartnerResponseBody, string>) (p => p.PartnerID + p.ProductName)).Distinct<string>());
      return stringList.ToArray();
    }

    private void btnAddPersona_Click(object sender, EventArgs e)
    {
      if (this.personaSettingsForm.CheckUnsavedData(true) == DialogResult.Cancel)
        return;
      PersonaNameDlg personaNameDlg = new PersonaNameDlg(this.session, false);
      if (personaNameDlg.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
        return;
      string personaName = personaNameDlg.PersonaName;
      bool settingsDefault = personaNameDlg.SettingsDefault;
      Persona persona = this.session.PersonaManager.CreatePersona(personaName, settingsDefault, personaNameDlg.IsInternal, personaNameDlg.IsExternal);
      ServicesAclManager aclManager1 = (ServicesAclManager) this.session.ACL.GetAclManager(AclCategory.Services);
      FieldAccessAclManager aclManager2 = (FieldAccessAclManager) this.session.ACL.GetAclManager(AclCategory.FieldAccess);
      ExportServicesAclManager aclManager3 = (ExportServicesAclManager) this.session.ACL.GetAclManager(AclCategory.ExportServices);
      InvestorServicesAclManager aclManager4 = (InvestorServicesAclManager) this.session.ACL.GetAclManager(AclCategory.InvestorServices);
      ((FeatureConfigsAclManager) this.session.ACL.GetAclManager(AclCategory.FeatureConfigs)).SetPermission(AclFeature.PlatForm_Access, persona.ID, 1);
      if (settingsDefault)
      {
        FeaturesAclManager aclManager5 = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
        aclManager5.DuplicateACLFeatures(1, persona.ID);
        Hashtable featureAccesses = new Hashtable();
        featureAccesses.Add((object) AclFeature.SettingsTab_HMDAAddLEI, (object) AclTriState.False);
        featureAccesses.Add((object) AclFeature.SettingsTab_HMDAEditLEI, (object) AclTriState.False);
        featureAccesses.Add((object) AclFeature.SettingsTab_HMDARemoveLEI, (object) AclTriState.False);
        featureAccesses.Add((object) AclFeature.TradeTab_BidTapeManagement, (object) AclTriState.True);
        featureAccesses.Add((object) AclFeature.TradeTab_EditBidTapeManagement, (object) AclTriState.True);
        if (!this.session.StartupInfo.AllowInsightsSetup)
          featureAccesses.Add((object) AclFeature.SettingsTab_InsightsSetup, (object) AclTriState.False);
        aclManager5.SetPermissions(featureAccesses, persona);
        ((InputFormsAclManager) this.session.ACL.GetAclManager(AclCategory.InputForms)).DuplicateACLInputForms(1, persona.ID);
        ((MilestonesAclManager) this.session.ACL.GetAclManager(AclCategory.Milestones)).DuplicateACLMilestones(1, persona.ID);
        ((StandardWebFormAclManager) this.session.ACL.GetAclManager(AclCategory.StandardWebforms)).DuplicateACLStandardWebForms(1, persona.ID, this.session.UserID);
        ((LoanFoldersAclManager) this.session.ACL.GetAclManager(AclCategory.LoanFolderMove)).DuplicateACLLoanFolder(1, persona.ID);
        ((LoanDuplicationAclManager) this.session.ACL.GetAclManager(AclCategory.LoanDuplicationTemplates)).DuplicateACLLoanDuplication(1, persona.ID);
        ((ToolsAclManager) this.session.ACL.GetAclManager(AclCategory.ToolsGrantWriteAccess)).DuplicateACLTools(1, persona.ID);
        aclManager2.DuplicateFieldAccess(1, persona.ID);
        aclManager1.DuplicateACLServices(1, persona.ID);
        aclManager3.DuplicateACLExportServices(1, persona.ID, ExportServiceAclInfo.GetExportServicesList(ServicesMapping.Categories.ToArray(), 229).ToArray());
        aclManager4.DuplicateACLInvestorServices(AclFeature.LoanMgmt_Investor_Service, 1, persona.ID, SelectInvestorsPage.InvestorCategory, InvestorServiceAclInfo.GetInvestorServicesList(this.GetProviderCompanyCodes(), 9007, SelectInvestorsPage.InvestorCategory).ToArray());
        if (new DataDocsServiceHelper(this.session).HasServiceAcccess(WarehouseLendersServicePage.InvestorCategory))
        {
          string[] array = new DataDocsServiceHelper(this.session).GetInvestorsList().Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (s => s.Category.Equals(WarehouseLendersServicePage.InvestorCategory, StringComparison.InvariantCultureIgnoreCase))).Select<PartnerResponseBody, string>((Func<PartnerResponseBody, string>) (p => p.PartnerID + p.ProductName)).Distinct<string>().ToArray<string>();
          aclManager4.DuplicateACLInvestorServices(AclFeature.LoanMgmt_Investor_Warehouse_Lenders, 1, persona.ID, WarehouseLendersServicePage.InvestorCategory, InvestorServiceAclInfo.GetInvestorServicesList(array, 9009, WarehouseLendersServicePage.InvestorCategory).ToArray());
        }
        if (new DataDocsServiceHelper(this.session).HasServiceAcccess(DueDiligenceServicePage.InvestorCategory))
        {
          string[] array = new DataDocsServiceHelper(this.session).GetInvestorsList().Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (s => s.Category.Equals(DueDiligenceServicePage.InvestorCategory, StringComparison.InvariantCultureIgnoreCase))).Select<PartnerResponseBody, string>((Func<PartnerResponseBody, string>) (p => p.PartnerID + p.ProductName)).Distinct<string>().ToArray<string>();
          aclManager4.DuplicateACLInvestorServices(AclFeature.LoanMgmt_Investor_Due_Diligence, 1, persona.ID, DueDiligenceServicePage.InvestorCategory, InvestorServiceAclInfo.GetInvestorServicesList(array, 9010, DueDiligenceServicePage.InvestorCategory).ToArray());
        }
        if (new DataDocsServiceHelper(this.session).HasServiceAcccess(HedgeAdvisoryServicePage.InvestorCategory))
        {
          string[] array = new DataDocsServiceHelper(this.session).GetInvestorsList().Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (s => s.Category.Equals(HedgeAdvisoryServicePage.InvestorCategory, StringComparison.InvariantCultureIgnoreCase))).Select<PartnerResponseBody, string>((Func<PartnerResponseBody, string>) (p => p.PartnerID + p.ProductName)).Distinct<string>().ToArray<string>();
          aclManager4.DuplicateACLInvestorServices(AclFeature.LoanMgmt_Investor_Hedge_Advisory, 1, persona.ID, HedgeAdvisoryServicePage.InvestorCategory, InvestorServiceAclInfo.GetInvestorServicesList(array, 9011, HedgeAdvisoryServicePage.InvestorCategory).ToArray());
        }
        if (new DataDocsServiceHelper(this.session).HasServiceAcccess(SubservicingServicePage.InvestorCategory))
        {
          string[] array = new DataDocsServiceHelper(this.session).GetInvestorsList().Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (s => s.Category.Equals(SubservicingServicePage.InvestorCategory, StringComparison.InvariantCultureIgnoreCase))).Select<PartnerResponseBody, string>((Func<PartnerResponseBody, string>) (p => p.PartnerID + p.ProductName)).Distinct<string>().ToArray<string>();
          aclManager4.DuplicateACLInvestorServices(AclFeature.LoanMgmt_Investor_Subservicing_Services, 1, persona.ID, SubservicingServicePage.InvestorCategory, InvestorServiceAclInfo.GetInvestorServicesList(array, 9012, SubservicingServicePage.InvestorCategory).ToArray());
        }
        if (new DataDocsServiceHelper(this.session).HasServiceAcccess(BidTapeServicePage.InvestorCategory))
        {
          string[] array = new DataDocsServiceHelper(this.session).GetInvestorsList().Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (s => s.Category.Equals(BidTapeServicePage.InvestorCategory, StringComparison.InvariantCultureIgnoreCase))).Select<PartnerResponseBody, string>((Func<PartnerResponseBody, string>) (p => p.PartnerID + p.ProductName)).Distinct<string>().ToArray<string>();
          aclManager4.DuplicateACLInvestorServices(AclFeature.LoanMgmt_Investor_Bid_Tape_Services, 1, persona.ID, BidTapeServicePage.InvestorCategory, InvestorServiceAclInfo.GetInvestorServicesList(array, 9013, BidTapeServicePage.InvestorCategory).ToArray());
        }
        if (new DataDocsServiceHelper(this.session).HasServiceAcccess(QCAuditServicesPage.InvestorCategory))
        {
          string[] array = new DataDocsServiceHelper(this.session).GetInvestorsList().Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (s => s.Category.Equals(QCAuditServicesPage.InvestorCategory, StringComparison.InvariantCultureIgnoreCase))).Select<PartnerResponseBody, string>((Func<PartnerResponseBody, string>) (p => p.PartnerID + p.ProductName)).Distinct<string>().ToArray<string>();
          aclManager4.DuplicateACLInvestorServices(AclFeature.LoanMgmt_Investor_QC_Audit_Services, 1, persona.ID, QCAuditServicesPage.InvestorCategory, InvestorServiceAclInfo.GetInvestorServicesList(array, 9014, QCAuditServicesPage.InvestorCategory).ToArray());
        }
        if (new DataDocsServiceHelper(this.session).HasServiceAcccess(WholesaleLenderServicePage.InvestorCategory))
        {
          string[] array = new DataDocsServiceHelper(this.session).GetInvestorsList().Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (s => s.Category.Equals(WholesaleLenderServicePage.InvestorCategory, StringComparison.InvariantCultureIgnoreCase))).Select<PartnerResponseBody, string>((Func<PartnerResponseBody, string>) (p => p.PartnerID + p.ProductName)).Distinct<string>().ToArray<string>();
          aclManager4.DuplicateACLInvestorServices(AclFeature.LoanMgmt_Investor_Wholesale_Lender_Services, 1, persona.ID, WholesaleLenderServicePage.InvestorCategory, InvestorServiceAclInfo.GetInvestorServicesList(array, 9017, WholesaleLenderServicePage.InvestorCategory).ToArray());
        }
        if (new DataDocsServiceHelper(this.session).HasServiceAcccess(ServicingServicePage.InvestorCategory))
        {
          string[] array = new DataDocsServiceHelper(this.session).GetInvestorsList().Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (s => s.Category.Equals(ServicingServicePage.InvestorCategory, StringComparison.InvariantCultureIgnoreCase))).Select<PartnerResponseBody, string>((Func<PartnerResponseBody, string>) (p => p.PartnerID + p.ProductName)).Distinct<string>().ToArray<string>();
          aclManager4.DuplicateACLInvestorServices(AclFeature.LoanMgmt_Investor_Servicing_Services, 1, persona.ID, ServicingServicePage.InvestorCategory, InvestorServiceAclInfo.GetInvestorServicesList(array, 9020, ServicingServicePage.InvestorCategory).ToArray());
        }
        ((PipelineViewAclManager) this.session.ACL.GetAclManager(AclCategory.PersonaPipelineView)).CreatePipelineView(new PersonaPipelineView(persona.ID, "New View")
        {
          Name = "Default View",
          Columns = {
            "Alerts.AlertCount",
            "Messages.MessageCount",
            "Loan.LoanNumber",
            {
              "Loan.BorrowerName",
              SortOrder.Ascending
            },
            "Loan.LoanRate",
            "Loan.LockAndRequestStatus",
            "Loan.LockExpirationDate",
            "Fields.Log.MS.LastCompleted",
            "NextMilestone.MilestoneName",
            "Loan.NextMilestoneDate",
            "Loan.DateOfEstimatedCompletion",
            "Loan.LoanType",
            "Loan.LoanPurpose",
            "Loan.Amortization",
            "Loan.Address1",
            "Loan.City",
            "Loan.State",
            "Loan.LoanOfficerName",
            "Loan.LoanProcessorName",
            "Fields.CurrentTeamMember"
          }
        });
      }
      else
      {
        aclManager2.InitialColumnSync(persona.ID, false);
        aclManager1.SetDefaultValue(AclFeature.LoanTab_Other_ePASS, persona.ID, ServiceAclInfo.ServicesDefaultSetting.None);
        aclManager3.SetDefaultValue(AclFeature.LoanMgmt_MgmtPipelineServices, persona.ID, ExportServiceAclInfo.ExportServicesDefaultSetting.None);
        aclManager4.SetDefaultValue(AclFeature.LoanMgmt_Investor_Service, persona.ID, SelectInvestorsPage.InvestorCategory, InvestorServiceAclInfo.InvestorServicesDefaultSetting.None);
        if (new DataDocsServiceHelper(this.session).HasServiceAcccess(WarehouseLendersServicePage.InvestorCategory))
          aclManager4.SetDefaultValue(AclFeature.LoanMgmt_Investor_Warehouse_Lenders, persona.ID, WarehouseLendersServicePage.InvestorCategory, InvestorServiceAclInfo.InvestorServicesDefaultSetting.None);
        if (new DataDocsServiceHelper(this.session).HasServiceAcccess(DueDiligenceServicePage.InvestorCategory))
          aclManager4.SetDefaultValue(AclFeature.LoanMgmt_Investor_Due_Diligence, persona.ID, DueDiligenceServicePage.InvestorCategory, InvestorServiceAclInfo.InvestorServicesDefaultSetting.None);
        if (new DataDocsServiceHelper(this.session).HasServiceAcccess(HedgeAdvisoryServicePage.InvestorCategory))
          aclManager4.SetDefaultValue(AclFeature.LoanMgmt_Investor_Hedge_Advisory, persona.ID, HedgeAdvisoryServicePage.InvestorCategory, InvestorServiceAclInfo.InvestorServicesDefaultSetting.None);
        if (new DataDocsServiceHelper(this.session).HasServiceAcccess(SubservicingServicePage.InvestorCategory))
          aclManager4.SetDefaultValue(AclFeature.LoanMgmt_Investor_Subservicing_Services, persona.ID, SubservicingServicePage.InvestorCategory, InvestorServiceAclInfo.InvestorServicesDefaultSetting.None);
        if (new DataDocsServiceHelper(this.session).HasServiceAcccess(BidTapeServicePage.InvestorCategory))
          aclManager4.SetDefaultValue(AclFeature.LoanMgmt_Investor_Bid_Tape_Services, persona.ID, BidTapeServicePage.InvestorCategory, InvestorServiceAclInfo.InvestorServicesDefaultSetting.None);
        if (new DataDocsServiceHelper(this.session).HasServiceAcccess(QCAuditServicesPage.InvestorCategory))
          aclManager4.SetDefaultValue(AclFeature.LoanMgmt_Investor_QC_Audit_Services, persona.ID, QCAuditServicesPage.InvestorCategory, InvestorServiceAclInfo.InvestorServicesDefaultSetting.None);
        if (new DataDocsServiceHelper(this.session).HasServiceAcccess(WholesaleLenderServicePage.InvestorCategory))
          aclManager4.SetDefaultValue(AclFeature.LoanMgmt_Investor_Wholesale_Lender_Services, persona.ID, WholesaleLenderServicePage.InvestorCategory, InvestorServiceAclInfo.InvestorServicesDefaultSetting.None);
        if (new DataDocsServiceHelper(this.session).HasServiceAcccess(ServicingServicePage.InvestorCategory))
          aclManager4.SetDefaultValue(AclFeature.LoanMgmt_Investor_Servicing_Services, persona.ID, ServicingServicePage.InvestorCategory, InvestorServiceAclInfo.InvestorServicesDefaultSetting.None);
        ((PipelineViewAclManager) this.session.ACL.GetAclManager(AclCategory.PersonaPipelineView)).CreatePipelineView(new PersonaPipelineView(persona.ID, "New View")
        {
          Name = "Default View"
        });
      }
      ListViewItem listViewItem = new ListViewItem(persona.Name);
      if (this.isTPOMVP)
        listViewItem.SubItems.Add(this.getPersonaTypeDescription(persona));
      listViewItem.Tag = (object) persona;
      listViewItem.Selected = true;
      this.lvPersona.BeginUpdate();
      if (this.lvPersona.SelectedItems.Count > 0)
        this.lvPersona.SelectedItems[0].Selected = false;
      this.lvPersona.Items.Add(listViewItem);
      this.lvPersona_SelectedIndexChanged((object) null, (EventArgs) null);
      this.lvPersona.EndUpdate();
      this.enableDisableMoveUpDownButtons();
    }

    private void btnDeletePersona_Click(object sender, EventArgs e)
    {
      if (this.lvPersona.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select the persona you want to delete.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        Persona tag = (Persona) this.lvPersona.SelectedItems[0].Tag;
        RoleInfo[] functionsByPersonaId = ((WorkflowManager) this.session.BPM.GetBpmManager(BpmCategory.Workflow)).GetAllRoleFunctionsByPersonaID(tag.ID);
        if (functionsByPersonaId != null && functionsByPersonaId.Length != 0)
        {
          bool flag = false;
          foreach (RoleInfo roleInfo in functionsByPersonaId)
          {
            if (roleInfo.PersonaIDs != null && roleInfo.PersonaIDs.Length == 1)
            {
              flag = true;
              break;
            }
          }
          if (flag)
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "This persona cannot be deleted because it is the only persona for certain Role.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return;
          }
        }
        UserInfo[] usersWithPersona = this.session.OrganizationManager.GetUsersWithPersona(tag.ID, true);
        if (usersWithPersona.Length != 0 && DialogResult.Cancel == new DeletePersonaDlg(tag, usersWithPersona).ShowDialog((IWin32Window) this) || DialogResult.No == Utils.Dialog((IWin32Window) this, "Are you sure you want to delete the persona?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
          return;
        this.personas = this.getPersonas();
        List<Persona> personaList = new List<Persona>();
        foreach (Persona persona in this.personas)
        {
          if (persona.ID != tag.ID)
            personaList.Add(persona);
        }
        this.personas = personaList.ToArray();
        this.loadLVPersona(this.personas);
        this.onLvPersonaSelectedIndexChanged();
        this.enableDisableMoveUpDownButtons();
        this.session.PersonaManager.DeletePersona(tag.ID);
      }
    }

    private void btnCopyPersona_Click(object sender, EventArgs e)
    {
      if (this.lvPersona.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a persona to copy from.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        if (this.personaSettingsForm.CheckUnsavedData(true) == DialogResult.Cancel)
          return;
        string str = "Copy of " + this.lvPersona.SelectedItems[0].Text;
        int num2 = 1;
        object[] objArray;
        for (; this.session.PersonaManager.PersonaNameExists(str); str = string.Concat(objArray))
        {
          ++num2;
          objArray = new object[4]
          {
            (object) "Copy of (",
            (object) num2,
            (object) ") ",
            (object) this.lvPersona.SelectedItems[0].Text
          };
        }
        if (str.Length > 64)
        {
          PersonaNameDlg personaNameDlg = new PersonaNameDlg(this.session, true);
          if (personaNameDlg.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          str = personaNameDlg.PersonaName;
        }
        int id = ((Persona) this.lvPersona.SelectedItems[0].Tag).ID;
        bool aclFeaturesDefault = ((Persona) this.lvPersona.SelectedItems[0].Tag).AclFeaturesDefault;
        bool isInternal = ((Persona) this.lvPersona.SelectedItems[0].Tag).IsInternal;
        bool isExternal = ((Persona) this.lvPersona.SelectedItems[0].Tag).IsExternal;
        Persona persona = this.session.PersonaManager.CreatePersona(str, aclFeaturesDefault, isInternal, isExternal);
        ((FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features)).DuplicateACLFeatures(id, persona.ID);
        ((InputFormsAclManager) this.session.ACL.GetAclManager(AclCategory.InputForms)).DuplicateACLInputForms(id, persona.ID);
        ((MilestonesAclManager) this.session.ACL.GetAclManager(AclCategory.Milestones)).DuplicateACLMilestones(id, persona.ID);
        ((MilestoneFreeRoleAclManager) this.session.ACL.GetAclManager(AclCategory.MilestonesFreeRole)).DuplicateACLMilestonesFreeRole(id, persona.ID);
        ((FeatureConfigsAclManager) this.session.ACL.GetAclManager(AclCategory.FeatureConfigs)).DuplicateACLFeatures(id, persona.ID);
        ((LoanFoldersAclManager) this.session.ACL.GetAclManager(AclCategory.LoanFolderMove)).DuplicateACLLoanFolder(id, persona.ID);
        ((LoanDuplicationAclManager) this.session.ACL.GetAclManager(AclCategory.LoanDuplicationTemplates)).DuplicateACLLoanDuplication(id, persona.ID);
        ((ToolsAclManager) this.session.ACL.GetAclManager(AclCategory.ToolsGrantWriteAccess)).DuplicateACLTools(id, persona.ID);
        ((ServicesAclManager) this.session.ACL.GetAclManager(AclCategory.Services)).DuplicateACLServices(id, persona.ID);
        ((ExportServicesAclManager) this.session.ACL.GetAclManager(AclCategory.ExportServices)).DuplicateACLExportServices(id, persona.ID, ExportServiceAclInfo.GetExportServicesList(ServicesMapping.Categories.ToArray(), 229).ToArray());
        InvestorServicesAclManager aclManager = (InvestorServicesAclManager) this.session.ACL.GetAclManager(AclCategory.InvestorServices);
        aclManager.DuplicateACLInvestorServices(AclFeature.LoanMgmt_Investor_Service, id, persona.ID, SelectInvestorsPage.InvestorCategory, InvestorServiceAclInfo.GetInvestorServicesList(this.GetProviderCompanyCodes(), 9007, SelectInvestorsPage.InvestorCategory).ToArray());
        if (new DataDocsServiceHelper(this.session).HasServiceAcccess(WarehouseLendersServicePage.InvestorCategory))
        {
          string[] array = new DataDocsServiceHelper(this.session).GetInvestorsList().Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (s => s.Category.Equals(WarehouseLendersServicePage.InvestorCategory, StringComparison.InvariantCultureIgnoreCase))).Select<PartnerResponseBody, string>((Func<PartnerResponseBody, string>) (p => p.PartnerID + p.ProductName)).Distinct<string>().ToArray<string>();
          aclManager.DuplicateACLInvestorServices(AclFeature.LoanMgmt_Investor_Warehouse_Lenders, id, persona.ID, WarehouseLendersServicePage.InvestorCategory, InvestorServiceAclInfo.GetInvestorServicesList(array, 9009, WarehouseLendersServicePage.InvestorCategory).ToArray());
        }
        if (new DataDocsServiceHelper(this.session).HasServiceAcccess(DueDiligenceServicePage.InvestorCategory))
        {
          string[] array = new DataDocsServiceHelper(this.session).GetInvestorsList().Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (s => s.Category.Equals(DueDiligenceServicePage.InvestorCategory, StringComparison.InvariantCultureIgnoreCase))).Select<PartnerResponseBody, string>((Func<PartnerResponseBody, string>) (p => p.PartnerID + p.ProductName)).Distinct<string>().ToArray<string>();
          aclManager.DuplicateACLInvestorServices(AclFeature.LoanMgmt_Investor_Due_Diligence, id, persona.ID, DueDiligenceServicePage.InvestorCategory, InvestorServiceAclInfo.GetInvestorServicesList(array, 9010, DueDiligenceServicePage.InvestorCategory).ToArray());
        }
        if (new DataDocsServiceHelper(this.session).HasServiceAcccess(HedgeAdvisoryServicePage.InvestorCategory))
        {
          string[] array = new DataDocsServiceHelper(this.session).GetInvestorsList().Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (s => s.Category.Equals(HedgeAdvisoryServicePage.InvestorCategory, StringComparison.InvariantCultureIgnoreCase))).Select<PartnerResponseBody, string>((Func<PartnerResponseBody, string>) (p => p.PartnerID + p.ProductName)).Distinct<string>().ToArray<string>();
          aclManager.DuplicateACLInvestorServices(AclFeature.LoanMgmt_Investor_Hedge_Advisory, id, persona.ID, HedgeAdvisoryServicePage.InvestorCategory, InvestorServiceAclInfo.GetInvestorServicesList(array, 9011, HedgeAdvisoryServicePage.InvestorCategory).ToArray());
        }
        if (new DataDocsServiceHelper(this.session).HasServiceAcccess(SubservicingServicePage.InvestorCategory))
        {
          string[] array = new DataDocsServiceHelper(this.session).GetInvestorsList().Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (s => s.Category.Equals(SubservicingServicePage.InvestorCategory, StringComparison.InvariantCultureIgnoreCase))).Select<PartnerResponseBody, string>((Func<PartnerResponseBody, string>) (p => p.PartnerID + p.ProductName)).Distinct<string>().ToArray<string>();
          aclManager.DuplicateACLInvestorServices(AclFeature.LoanMgmt_Investor_Subservicing_Services, id, persona.ID, SubservicingServicePage.InvestorCategory, InvestorServiceAclInfo.GetInvestorServicesList(array, 9012, SubservicingServicePage.InvestorCategory).ToArray());
        }
        if (new DataDocsServiceHelper(this.session).HasServiceAcccess(BidTapeServicePage.InvestorCategory))
        {
          string[] array = new DataDocsServiceHelper(this.session).GetInvestorsList().Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (s => s.Category.Equals(BidTapeServicePage.InvestorCategory, StringComparison.InvariantCultureIgnoreCase))).Select<PartnerResponseBody, string>((Func<PartnerResponseBody, string>) (p => p.PartnerID + p.ProductName)).Distinct<string>().ToArray<string>();
          aclManager.DuplicateACLInvestorServices(AclFeature.LoanMgmt_Investor_Bid_Tape_Services, id, persona.ID, BidTapeServicePage.InvestorCategory, InvestorServiceAclInfo.GetInvestorServicesList(array, 9013, BidTapeServicePage.InvestorCategory).ToArray());
        }
        if (new DataDocsServiceHelper(this.session).HasServiceAcccess(QCAuditServicesPage.InvestorCategory))
        {
          string[] array = new DataDocsServiceHelper(this.session).GetInvestorsList().Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (s => s.Category.Equals(QCAuditServicesPage.InvestorCategory, StringComparison.InvariantCultureIgnoreCase))).Select<PartnerResponseBody, string>((Func<PartnerResponseBody, string>) (p => p.PartnerID + p.ProductName)).Distinct<string>().ToArray<string>();
          aclManager.DuplicateACLInvestorServices(AclFeature.LoanMgmt_Investor_QC_Audit_Services, id, persona.ID, QCAuditServicesPage.InvestorCategory, InvestorServiceAclInfo.GetInvestorServicesList(array, 9014, QCAuditServicesPage.InvestorCategory).ToArray());
        }
        if (new DataDocsServiceHelper(this.session).HasServiceAcccess(WholesaleLenderServicePage.InvestorCategory))
        {
          string[] array = new DataDocsServiceHelper(this.session).GetInvestorsList().Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (s => s.Category.Equals(WholesaleLenderServicePage.InvestorCategory, StringComparison.InvariantCultureIgnoreCase))).Select<PartnerResponseBody, string>((Func<PartnerResponseBody, string>) (p => p.PartnerID + p.ProductName)).Distinct<string>().ToArray<string>();
          aclManager.DuplicateACLInvestorServices(AclFeature.LoanMgmt_Investor_Wholesale_Lender_Services, id, persona.ID, WholesaleLenderServicePage.InvestorCategory, InvestorServiceAclInfo.GetInvestorServicesList(array, 9017, WholesaleLenderServicePage.InvestorCategory).ToArray());
        }
        if (new DataDocsServiceHelper(this.session).HasServiceAcccess(ServicingServicePage.InvestorCategory))
        {
          string[] array = new DataDocsServiceHelper(this.session).GetInvestorsList().Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (s => s.Category.Equals(ServicingServicePage.InvestorCategory, StringComparison.InvariantCultureIgnoreCase))).Select<PartnerResponseBody, string>((Func<PartnerResponseBody, string>) (p => p.PartnerID + p.ProductName)).Distinct<string>().ToArray<string>();
          aclManager.DuplicateACLInvestorServices(AclFeature.LoanMgmt_Investor_Servicing_Services, id, persona.ID, ServicingServicePage.InvestorCategory, InvestorServiceAclInfo.GetInvestorServicesList(array, 9020, ServicingServicePage.InvestorCategory).ToArray());
        }
        ((EnhancedConditionsAclManager) this.session.ACL.GetAclManager(AclCategory.EnhancedConditions)).DuplicateEnhancedConditionTypeFeatures(id, persona.ID);
        ((FieldAccessAclManager) this.session.ACL.GetAclManager(AclCategory.FieldAccess)).DuplicateFieldAccess(id, persona.ID);
        ((PipelineViewAclManager) this.session.ACL.GetAclManager(AclCategory.PersonaPipelineView)).DuplicatePipelineViews(id, persona.ID);
        ListViewItem listViewItem = new ListViewItem(persona.Name);
        if (this.isTPOMVP)
          listViewItem.SubItems.Add(this.getPersonaTypeDescription(persona));
        listViewItem.Tag = (object) persona;
        listViewItem.Selected = true;
        this.lvPersona.BeginUpdate();
        if (this.lvPersona.SelectedItems.Count > 0)
          this.lvPersona.SelectedItems[0].Selected = false;
        this.lvPersona.Items.Add(listViewItem);
        this.lvPersona_SelectedIndexChanged((object) null, (EventArgs) null);
        this.lvPersona.EndUpdate();
        this.enableDisableMoveUpDownButtons();
      }
    }

    private void PersonaSetupForm_BackColorChanged(object sender, EventArgs e)
    {
      this.personaSettingsForm.BackColor = this.BackColor;
    }

    private void lvPersona_AfterLabelEdit(object sender, LabelEditEventArgs e)
    {
      bool flag = false;
      if (e.Label == null)
        e.CancelEdit = true;
      else if (e.Label.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please provide a persona name", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        e.CancelEdit = true;
      }
      else if (e.Label.Length > 64)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Persona name can not be longer than 64 characters.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        e.CancelEdit = true;
      }
      else if (e.Label.ToLower().Replace(" ", "") == "administrator" || e.Label.ToLower().Replace(" ", "") == "superadministrator")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The selected persona name is a reserved key word. Please enter a different name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        e.CancelEdit = true;
      }
      else
      {
        foreach (ListViewItem listViewItem in this.lvPersona.Items)
        {
          if (listViewItem.Index != e.Item && e.Label.ToLower().Trim() == listViewItem.Text.ToLower().Trim())
          {
            flag = true;
            break;
          }
        }
        if (flag)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Persona name already exist.  Please select another one.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          e.CancelEdit = true;
        }
        else
        {
          this.session.PersonaManager.RenamePersona((Persona) this.lvPersona.Items[e.Item].Tag, e.Label.Trim());
          Persona personaByName = this.session.PersonaManager.GetPersonaByName(e.Label.Trim());
          this.lvPersona.Items[e.Item].Tag = (object) personaByName;
          this.personaSettingsForm.Title = "2. Define access for the " + personaByName.Name + " persona.";
        }
      }
    }

    private void lvGroupSelectedIndexChangedTimer_Tick(object sender, EventArgs e)
    {
      this.lvPersonaSelectedIndexChangedTimer.Stop();
      this.lvPersonaSelectedIndexChangedTimerIsRunning = false;
      this.onLvPersonaSelectedIndexChanged();
    }

    private void lvPersona_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!this.lvPersonaSelectedIndexChangedTimerIsRunning && this.lvPersona.SelectedItems.Count == 0)
      {
        this.lvPersonaSelectedIndexChangedTimerIsRunning = true;
        this.lvPersonaSelectedIndexChangedTimer.Start();
      }
      else
      {
        if (!this.lvPersonaSelectedIndexChangedTimerIsRunning)
          this.onLvPersonaSelectedIndexChanged();
        this.personaSettingsForm.Enabled = this.lvPersona.SelectedItems.Count <= 1;
      }
    }

    public string[] SelectedPersonas
    {
      get
      {
        if (this.lvPersona.SelectedItems == null)
          return (string[]) null;
        List<string> stringList = new List<string>();
        foreach (ListViewItem selectedItem in this.lvPersona.SelectedItems)
          stringList.Add(selectedItem.Text);
        return stringList.ToArray();
      }
      set
      {
        if (value == null || value.Length == 0)
          return;
        List<string> stringList = new List<string>((IEnumerable<string>) value);
        foreach (ListViewItem listViewItem in this.lvPersona.Items)
          listViewItem.Selected = stringList.Contains(listViewItem.Text);
      }
    }

    private void onLvPersonaSelectedIndexChanged()
    {
      if (this.lvPersona.SelectedItems.Count == 0)
      {
        if (this.lvPersona.Items.Count == 0)
          return;
        if (this.lvPersonaCurrSelectedIdx >= this.lvPersona.Items.Count)
          this.lvPersonaCurrSelectedIdx = this.lvPersona.Items.Count - 1;
        if (this.lvPersonaCurrSelectedIdx < 0 || this.lvPersona.Items[this.lvPersonaCurrSelectedIdx].Selected)
          return;
        this.lvPersona.Items[this.lvPersonaCurrSelectedIdx].Selected = true;
      }
      else
      {
        if (this.lvPersonaCurrSelectedIdx == this.lvPersona.SelectedIndices[0])
          return;
        if (this.checkSaveNeeded && this.personaSettingsForm != null && !this.personaSettingsForm.SkipChecking && this.lvPersona.SelectedIndices[0] >= 0)
        {
          this.checkSaveNeeded = false;
          if (this.personaSettingsForm.CheckUnsavedData(true) == DialogResult.Cancel)
          {
            this.checkSaveNeeded = true;
            this.lvPersona.Items[this.lvPersonaCurrSelectedIdx].Selected = true;
            return;
          }
          this.checkSaveNeeded = true;
        }
        this.lvPersonaCurrSelectedIdx = this.lvPersona.SelectedIndices[0];
        this.menuItemRename.Enabled = this.stdIconBtnDelete.Enabled = this.stdIconBtnDuplicate.Enabled = this.lvPersona.SelectedItems.Count > 0;
        this.stdIconBtnUp.Enabled = this.stdIconBtnDown.Enabled = this.lvPersona.SelectedItems.Count > 0;
        this.menuItemDelete.Enabled = this.stdIconBtnDelete.Enabled;
        this.menuItemUp.Enabled = this.stdIconBtnUp.Enabled;
        this.menuItemDown.Enabled = this.stdIconBtnDown.Enabled;
        this.menuItemDuplicate.Enabled = this.stdIconBtnDuplicate.Enabled;
        if (this.lvPersona.SelectedItems.Count == 0)
          return;
        this.enableDisableMoveUpDownButtons();
        Persona tag = (Persona) this.lvPersona.SelectedItems[0].Tag;
        this.currentPersonaName = tag.Name;
        this.CurrentPersonaID = tag.ID;
        this.personaSettingsForm.Title = "2. Define access for the " + tag.Name + " persona.";
      }
    }

    private void enableDisableMoveUpDownButtons()
    {
      this.stdIconBtnUp.Enabled = this.menuItemUp.Enabled = this.lvPersona.SelectedIndices[0] != 0;
      this.stdIconBtnDown.Enabled = this.menuItemDown.Enabled = this.lvPersona.SelectedIndices[0] != this.lvPersona.Items.Count - 1;
    }

    private void picBoxUp_Click(object sender, EventArgs e)
    {
      this.lvPersona.SelectedIndexChanged -= new EventHandler(this.lvPersona_SelectedIndexChanged);
      try
      {
        if (this.lvPersona.SelectedItems.Count == 0)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Please select a persona.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return;
        }
        if (this.lvPersona.SelectedIndices[0] > 0)
        {
          if (this.personaSettingsForm.CheckUnsavedData(true) == DialogResult.Cancel)
            return;
          ListViewItem selectedItem = this.lvPersona.SelectedItems[0];
          int index = selectedItem.Index - 1;
          this.lvPersona.BeginUpdate();
          this.lvPersona.Items.Remove(selectedItem);
          this.lvPersona.Items.Insert(index, selectedItem);
          this.UpdatePersonaOrder();
          this.lvPersona.EndUpdate();
        }
      }
      finally
      {
        this.lvPersona.SelectedIndexChanged += new EventHandler(this.lvPersona_SelectedIndexChanged);
      }
      this.enableDisableMoveUpDownButtons();
    }

    private void picBoxDown_Click(object sender, EventArgs e)
    {
      this.lvPersona.SelectedIndexChanged -= new EventHandler(this.lvPersona_SelectedIndexChanged);
      try
      {
        if (this.lvPersona.SelectedItems.Count == 0)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Please select a persona.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return;
        }
        if (this.lvPersona.SelectedIndices[0] < this.lvPersona.Items.Count - 1)
        {
          if (this.personaSettingsForm.CheckUnsavedData(true) == DialogResult.Cancel)
            return;
          ListViewItem selectedItem = this.lvPersona.SelectedItems[0];
          int index = selectedItem.Index + 1;
          this.lvPersona.BeginUpdate();
          this.lvPersona.Items.Remove(selectedItem);
          this.lvPersona.Items.Insert(index, selectedItem);
          this.UpdatePersonaOrder();
          this.lvPersona.EndUpdate();
        }
      }
      finally
      {
        this.lvPersona.SelectedIndexChanged += new EventHandler(this.lvPersona_SelectedIndexChanged);
      }
      this.enableDisableMoveUpDownButtons();
    }

    private void UpdatePersonaOrder()
    {
      ArrayList arrayList = new ArrayList();
      foreach (ListViewItem listViewItem in this.lvPersona.Items)
      {
        Persona tag = (Persona) listViewItem.Tag;
        tag.DisplayOrder = listViewItem.Index;
        arrayList.Add((object) tag);
      }
      this.session.PersonaManager.UpdatePersonaOrder((Persona[]) arrayList.ToArray(typeof (Persona)));
    }

    private void menuItemRename_Click(object sender, EventArgs e)
    {
      this.lvPersona.SelectedItems[0].BeginEdit();
    }

    private void menuItemMakeInternal_Click(object sender, EventArgs e)
    {
      this.changePersonaType(true, false);
    }

    private void menuItemMakeExternal_Click(object sender, EventArgs e)
    {
      this.changePersonaType(false, true);
    }

    private void menuItemMakeInternalExternal_Click(object sender, EventArgs e)
    {
      this.changePersonaType(true, true);
    }

    private void contextMenuPersona_Popup(object sender, EventArgs e)
    {
      if (this.lvPersona.SelectedItems.Count == 0)
        return;
      this.setContextMenu((Persona) this.lvPersona.SelectedItems[0].Tag);
    }

    private void setContextMenu(Persona selectedPersona)
    {
      this.menuItemMakeInternal.Enabled = !selectedPersona.IsInternal || selectedPersona.IsExternal;
      this.menuItemMakeExternal.Enabled = selectedPersona.IsInternal || !selectedPersona.IsExternal;
      this.menuItemMakeInternalExternal.Enabled = !selectedPersona.IsInternal || !selectedPersona.IsExternal;
    }

    private void changePersonaType(bool isInternal, bool isExternal)
    {
      if (this.lvPersona.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a persona.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        if (this.personaSettingsForm.CheckUnsavedData(true) == DialogResult.Cancel)
          return;
        Persona tag = (Persona) this.lvPersona.SelectedItems[0].Tag;
        if (this.isAssociatedtoTPOContacts(tag, isInternal, isExternal))
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "This persona type cannot be changed as it is associated with one or more user accounts. Please select or create another persona with the proper persona type.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          this.session.PersonaManager.UpdatePersonaType(tag.ID, isInternal, isExternal);
          Persona personaByName = this.session.PersonaManager.GetPersonaByName(tag.Name);
          this.lvPersona.SelectedItems[0].SubItems[1].Text = this.getPersonaTypeDescription(personaByName);
          this.lvPersona.SelectedItems[0].Tag = (object) personaByName;
        }
      }
    }

    private string getPersonaTypeDescription(Persona objPersona)
    {
      return PersonaTypeNameProvider.GetDescriptionFromPersonaType(PersonaTypeNameProvider.ResolvePersonaType(objPersona.IsInternal, objPersona.IsExternal));
    }

    private bool isAssociatedtoTPOContacts(Persona objPersona, bool isInternal, bool isExternal)
    {
      bool flag = false;
      if (PersonaTypeNameProvider.ResolvePersonaType(isInternal, isExternal) == PersonaType.Internal && this.session.PersonaManager.GetAssociatedUsersCount(objPersona.ID) > 0)
        flag = true;
      return flag;
    }
  }
}
