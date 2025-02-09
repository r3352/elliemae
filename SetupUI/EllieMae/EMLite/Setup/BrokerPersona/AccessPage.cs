// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.BrokerPersona.AccessPage
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.RemotingServices.Acl;
using EllieMae.EMLite.Setup.PersonaSetup.SecurityPage;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.BrokerPersona
{
  public class AccessPage : UserControl, IPersonaSecurityPage
  {
    private int personaID = -1;
    private bool dirty;
    private Persona[] personas;
    private string userID = "";
    private Dictionary<string, AclTriState> accessColList = new Dictionary<string, AclTriState>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
    private Dictionary<string, AclTriState> accessColList_User = new Dictionary<string, AclTriState>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
    private Dictionary<string, AclTriState> accessColList_Personas = new Dictionary<string, AclTriState>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
    private PipelineViewAclManager pipelineViewMgr;
    private FieldAccessAclManager fieldAccessMgr;
    private FeaturesAclManager aclMgr;
    private ServicesAclManager serviceMgr;
    private LoanReportFieldDefs loanFieldDefs;
    private bool forcedReadOnly;
    private FeaturesAclManager featureMgr;
    private List<string> viewDBColumns = new List<string>();
    private Hashtable personaAccessSetting = new Hashtable();
    private Hashtable userAccessSetting = new Hashtable();
    private bool suspendEvent;
    private int broken;
    private int linked = 1;
    private TreeNode currentlySelectedNode;
    private ImportLoanDlg impDlg;
    private int selectOption = 2;
    private List<int> personaIDs = new List<int>();
    private Hashtable cachedData = new Hashtable();
    private List<AclFeature> contactTopFeatureList = new List<AclFeature>((IEnumerable<AclFeature>) new AclFeature[21]
    {
      AclFeature.GlobalTab_Contacts,
      AclFeature.Cnt_Borrower_Access,
      AclFeature.Cnt_Borrower_CreateNew,
      AclFeature.Cnt_Borrower_Copy,
      AclFeature.Cnt_Borrower_Delete,
      AclFeature.Cnt_Borrower_Reassign,
      AclFeature.Cnt_Borrower_MailMerge,
      AclFeature.Cnt_Borrower_Import,
      AclFeature.Cnt_Borrower_Print,
      AclFeature.Cnt_Borrower_CreatLoanFrmTemplate,
      AclFeature.Cnt_Borrower_CreatBlankLoan,
      AclFeature.Cnt_Borrower_LoansTab,
      AclFeature.Cnt_Biz_Access,
      AclFeature.Cnt_Biz_CreateNew,
      AclFeature.Cnt_Biz_Copy,
      AclFeature.Cnt_Biz_Delete,
      AclFeature.Cnt_Biz_MailMerge,
      AclFeature.Cnt_Biz_Import,
      AclFeature.Cnt_Biz_Print,
      AclFeature.Cnt_Biz_LoansTab,
      AclFeature.Cnt_Contacts_Update
    });
    private Sessions.Session session;
    private int currentPlatformAccessSelection;
    private bool loanSoftArchivalEnabled;
    private IContainer components;
    private PanelEx pnlExLeft;
    private Splitter splitter1;
    private PanelEx pnlExRight;
    private GroupContainer gcPipelineView;
    private PanelEx pnlExContacts;
    private Splitter splitter2;
    private PanelEx pnlExLoans;
    private GroupContainer gcLoans;
    private GroupContainer gcContacts;
    private PanelEx pnlExSettings;
    private GroupContainer gcSettings;
    private PanelEx pnlPipelineTasks;
    private GroupContainer gcPipelineTasks;
    private Splitter splitter6;
    private TreeView tvPipelineTasks;
    private PanelEx pnlExDashboardReport;
    private GroupContainer gcDashboardReport;
    private TreeView tvSettings;
    private TreeView tvDashabordReport;
    private TreeView tvContact;
    private TreeView tvLoan;
    private ImageList imgListLink;
    private ContextMenuStrip contextMenuTree;
    private ToolStripMenuItem linkWithPersonaRightsToolStripMenuItem;
    private ToolStripMenuItem disconnectFromPersonaRightsToolStripMenuItem;
    private GridView gvPipelineViews;
    private StandardIconButton siBtnMoveViewDown;
    private StandardIconButton siBtnMoveViewUp;
    private StandardIconButton siBtnEditPipelineView;
    private StandardIconButton siBtnDeleteView;
    private StandardIconButton siBtnAddPipelineView;
    private ToolTip toolTip1;
    private PanelEx pnlExMbl;
    private GroupContainer groupContainerPlatformAccess;
    private Label lblPlatforms;
    private RadioButton rdoDesktop;
    private RadioButton rdoBoth;
    private PanelEx pnlExNew;
    private Splitter splitter7;
    private Splitter splitter3;
    private Splitter splitter4;
    private PanelEx pnlExInsideMobileAccess;
    private PanelEx panelExDevConnect;
    private GroupContainer gcDeveloperConnect;
    private TreeView tvDeveloperConnect;

    public event EventHandler DirtyFlagChanged;

    public AccessPage(Sessions.Session session, int personaId, EventHandler dirtyFlagChanged)
    {
      this.suspendEvent = true;
      this.session = session;
      this.featureMgr = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
      this.personaID = personaId;
      this.init(dirtyFlagChanged);
      this.suspendEvent = false;
    }

    public AccessPage(
      Sessions.Session session,
      string userID,
      Persona[] personas,
      EventHandler dirtyFlagChanged)
    {
      this.suspendEvent = true;
      this.session = session;
      this.featureMgr = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
      this.userID = userID;
      this.personas = personas;
      foreach (Persona persona in this.personas)
        this.personaIDs.Add(persona.ID);
      this.init(dirtyFlagChanged);
      this.MakeReadOnly(true, AclFeature.PlatForm_Access);
      this.suspendEvent = false;
    }

    private void init(EventHandler dirtyFlagChanged)
    {
      this.loanSoftArchivalEnabled = string.Equals(this.session.ConfigurationManager.GetCompanySetting("POLICIES", "EnableLoanSoftArchival"), "true", StringComparison.OrdinalIgnoreCase);
      this.InitializeComponent();
      this.tvContact.ExpandAll();
      this.tvDashabordReport.ExpandAll();
      this.tvLoan.ExpandAll();
      this.tvSettings.ExpandAll();
      this.tvPipelineTasks.ExpandAll();
      this.tvDeveloperConnect.ExpandAll();
      this.setupTreeNode();
      this.DirtyFlagChanged += dirtyFlagChanged;
      this.aclMgr = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
      this.serviceMgr = (ServicesAclManager) this.session.ACL.GetAclManager(AclCategory.Services);
      this.fieldAccessMgr = (FieldAccessAclManager) this.session.ACL.GetAclManager(AclCategory.FieldAccess);
      this.pipelineViewMgr = (PipelineViewAclManager) this.session.ACL.GetAclManager(AclCategory.PersonaPipelineView);
      this.loanFieldDefs = LoanReportFieldDefs.GetLoanReportFieldDefs(this.session).GetFieldDefsI(LoanReportFieldFlags.AllDatabaseFields, false);
      this.loadPipelineData(true);
      this.loadSettings();
    }

    public void MakeReadOnly(bool makeReadOnly, AclFeature aclFeature)
    {
      if (aclFeature != AclFeature.PlatForm_Access)
        return;
      this.rdoBoth.Enabled = this.rdoDesktop.Enabled = !makeReadOnly;
    }

    private void setupTreeNode()
    {
      this.tvPipelineTasks.Nodes[0].Tag = (object) new AclFeature[1]
      {
        AclFeature.LoanMgmt_Export_ULAD_ForDu
      };
      this.tvPipelineTasks.Nodes[1].Tag = (object) new AclFeature[1]
      {
        AclFeature.LoanMgmt_Export_ILAD
      };
      this.tvPipelineTasks.Nodes[2].Tag = (object) new AclFeature[1]
      {
        AclFeature.LoanMgmt_Export_FannieMae_FormattedFile
      };
      if (this.loanSoftArchivalEnabled)
      {
        this.tvPipelineTasks.Nodes[3].Tag = (object) new AclFeature[1]
        {
          AclFeature.LoanMgmt_AccessToArchiveFolders
        };
        this.tvPipelineTasks.Nodes[4].Tag = (object) new AclFeature[1]
        {
          AclFeature.LoanMgmt_AccessToArchiveLoans
        };
      }
      this.tvLoan.Nodes[0].Tag = (object) AclFeature.LoanMgmt_Import;
      this.tvLoan.Nodes[0].ForeColor = Color.Blue;
      this.tvLoan.Nodes[1].Tag = (object) new AclFeature[1]
      {
        AclFeature.LoanMgmt_Transfer
      };
      this.tvLoan.Nodes[2].Tag = (object) new AclFeature[1]
      {
        AclFeature.LoanTab_Other_ePASS
      };
      this.tvLoan.Nodes[3].Tag = (object) new AclFeature[1]
      {
        AclFeature.LoanTab_Other_PlanCode
      };
      this.tvLoan.Nodes[4].Tag = (object) new AclFeature[1]
      {
        AclFeature.LoanTab_Other_AltLender
      };
      this.tvLoan.Nodes[5].Tag = (object) new AclFeature[1]
      {
        AclFeature.eFolder_Other_PackagesTab
      };
      this.tvLoan.Nodes[5].Nodes[0].Tag = (object) new AclFeature[1]
      {
        AclFeature.eFolder_Other_eSignPackages
      };
      this.tvContact.Nodes[0].Tag = (object) this.contactTopFeatureList.ToArray();
      this.tvContact.Nodes[0].Nodes[0].Tag = (object) new AclFeature[2]
      {
        AclFeature.Cnt_Borrower_Export,
        AclFeature.Cnt_Biz_Export
      };
      this.tvContact.Nodes[0].Nodes[1].Tag = (object) new AclFeature[1]
      {
        AclFeature.Cnt_Synchronization
      };
      this.tvContact.Nodes[0].Nodes[2].Tag = (object) new AclFeature[1]
      {
        AclFeature.Cnt_Campaign_Access
      };
      this.tvContact.Nodes[0].Nodes[2].Nodes[0].Tag = (object) new AclFeature[1]
      {
        AclFeature.Cnt_Campaign_AssignTaskToOther
      };
      this.tvContact.Nodes[0].Nodes[3].Tag = (object) new AclFeature[3]
      {
        AclFeature.Borrower_Contacts_Personal_CustomLetters,
        AclFeature.Business_Contacts_Personal_CustomLetters,
        AclFeature.Cnt_Campaign_PersonalTemplates
      };
      this.tvDashabordReport.Nodes[0].Tag = (object) new AclFeature[3]
      {
        AclFeature.ReportTab_LoanReport,
        AclFeature.ReportTab_BorrowerContactReport,
        AclFeature.ReportTab_BusinessContactReport
      };
      this.tvDashabordReport.Nodes[0].Nodes[0].Tag = (object) new AclFeature[1]
      {
        AclFeature.ReportTab_ReportingDB
      };
      this.tvDashabordReport.Nodes[0].Nodes[1].Tag = (object) new AclFeature[1]
      {
        AclFeature.ReportTab_PersonalTemplate
      };
      this.tvDashabordReport.Nodes[1].Tag = (object) new AclFeature[1]
      {
        AclFeature.DashboardTab_Dashboard
      };
      this.tvDashabordReport.Nodes[1].Nodes[0].Tag = (object) new AclFeature[2]
      {
        AclFeature.DashboardTab_ManagePersonalTemplate,
        AclFeature.DashboardTab_ManagePersonalViewTemplate
      };
      this.tvSettings.Nodes[0].Tag = (object) new AclFeature[2]
      {
        AclFeature.SettingsTab_Personal_CustomPrintForms,
        AclFeature.SettingsTab_Personal_PrintGroups
      };
      this.tvSettings.Nodes[1].Tag = (object) new AclFeature[1]
      {
        AclFeature.SettingsTab_Company_LoanCustomFields
      };
      this.tvSettings.Nodes[2].Tag = (object) new AclFeature[1]
      {
        AclFeature.SettingsTab_Company_DocumentSetup
      };
      this.tvSettings.Nodes[3].Tag = (object) new AclFeature[9]
      {
        AclFeature.SettingsTab_Personal_LoanPrograms,
        AclFeature.SettingsTab_Personal_ClosingCosts,
        AclFeature.SettingsTab_Personal_DocumentSets,
        AclFeature.SettingsTab_Personal_InputFormSets,
        AclFeature.SettingsTab_Personal_SettlementServiceProvider,
        AclFeature.SettingsTab_Personal_Affiliate,
        AclFeature.SettingsTab_Personal_MiscDataTemplates,
        AclFeature.SettingsTab_Personal_LoanTemplateSets,
        AclFeature.SettingsTab_Personal_TaskSets
      };
      this.tvSettings.Nodes[4].Tag = (object) new AclFeature[1]
      {
        AclFeature.SettingsTab_Company_TableFee
      };
      this.tvSettings.Nodes[4].Nodes[0].Tag = (object) new AclFeature[1]
      {
        AclFeature.SettingsTab_Company_TF_2010ItemizationFeeMgmt
      };
      this.tvDeveloperConnect.Nodes[0].Tag = (object) new AclFeature[1]
      {
        AclFeature.DeveloperConnectTab_SubscribetoWebhooks
      };
      this.tvDeveloperConnect.Nodes[0].Nodes[0].Tag = (object) new AclFeature[1]
      {
        AclFeature.DeveloperConnectTab_SubscribetoWebhooks_EnhancedFieldChange
      };
    }

    private void setUserControlOption(int accessRight, AclFeature aclFeature)
    {
      if (aclFeature != AclFeature.PlatForm_Access)
        return;
      switch (accessRight)
      {
        case -1:
        case 0:
        case int.MaxValue:
          this.rdoDesktop.Checked = true;
          break;
        default:
          this.rdoBoth.Checked = true;
          break;
      }
      this.currentPlatformAccessSelection = accessRight;
    }

    private void loadSettings()
    {
      if (this.userID == "")
        this.loadPersonaSetting();
      else
        this.loadUserSetting();
    }

    private void loadPersonaSetting()
    {
      this.personaAccessSetting = this.aclMgr.GetPermissions(FeatureSets.BrokerFeatures, this.personaID);
      if (this.serviceMgr.GetServicesDefaultSetting(AclFeature.LoanTab_Other_ePASS, this.personaID) == ServiceAclInfo.ServicesDefaultSetting.All)
        this.personaAccessSetting.Add((object) AclFeature.LoanTab_Other_ePASS, (object) true);
      else
        this.personaAccessSetting.Add((object) AclFeature.LoanTab_Other_ePASS, (object) false);
      this.impDlg = new ImportLoanDlg(this.session, this.personaID, this.forcedReadOnly, this.selectOption);
      if (this.impDlg.HasSomethingChecked())
        this.personaAccessSetting.Add((object) AclFeature.LoanMgmt_Import, (object) true);
      this.populateTreeValue(this.personaAccessSetting);
    }

    private void loadUserSetting()
    {
      this.personaAccessSetting = this.aclMgr.GetPermissions(FeatureSets.BrokerFeatures, this.personaIDs.ToArray());
      if (this.serviceMgr.GetServicesDefaultSetting(AclFeature.LoanTab_Other_ePASS, this.userID, this.personas) == ServiceAclInfo.ServicesDefaultSetting.All)
        this.personaAccessSetting.Add((object) AclFeature.LoanTab_Other_ePASS, (object) true);
      else
        this.personaAccessSetting.Add((object) AclFeature.LoanTab_Other_ePASS, (object) false);
      this.userAccessSetting = this.aclMgr.GetPermissions(FeatureSets.BrokerFeatures, this.userID);
      switch (this.serviceMgr.GetUserSpecificServicesDefaultSetting(AclFeature.LoanTab_Other_ePASS, this.userID, this.personas))
      {
        case ServiceAclInfo.ServicesDefaultSetting.All:
          this.userAccessSetting.Add((object) AclFeature.LoanTab_Other_ePASS, (object) AclTriState.True);
          goto case ServiceAclInfo.ServicesDefaultSetting.NotSpecified;
        case ServiceAclInfo.ServicesDefaultSetting.NotSpecified:
          this.populateUserTreeValue(this.personaAccessSetting, this.userAccessSetting);
          break;
        default:
          this.userAccessSetting.Add((object) AclFeature.LoanTab_Other_ePASS, (object) AclTriState.False);
          goto case ServiceAclInfo.ServicesDefaultSetting.NotSpecified;
      }
    }

    private void populateUserTreeValue(Hashtable accessTable, Hashtable userAccessTable)
    {
      this.suspendEvent = true;
      this.registerTreeNodeCheckEvent(false);
      this.impDlg = new ImportLoanDlg(this.session, this.userID, this.personas, this.forcedReadOnly, this.selectOption);
      this.tvLoan.Nodes[0].Checked = this.impDlg.HasSomethingChecked();
      this.tvLoan.Nodes[0].ImageIndex = this.impDlg.GetImageIndex() != 1 ? (this.tvLoan.Nodes[0].SelectedImageIndex = this.tvLoan.Nodes[0].StateImageIndex = this.linked) : (this.tvLoan.Nodes[0].SelectedImageIndex = this.tvLoan.Nodes[0].StateImageIndex = this.broken);
      if (userAccessTable.ContainsKey((object) AclFeature.LoanMgmt_Transfer))
      {
        this.tvLoan.Nodes[1].ImageIndex = this.tvLoan.Nodes[1].SelectedImageIndex = this.tvLoan.Nodes[1].StateImageIndex = this.broken;
        this.tvLoan.Nodes[1].Checked = (AclTriState) userAccessTable[(object) AclFeature.LoanMgmt_Transfer] == AclTriState.True;
      }
      else
      {
        this.tvLoan.Nodes[1].ImageIndex = this.tvLoan.Nodes[1].SelectedImageIndex = this.tvLoan.Nodes[1].StateImageIndex = this.linked;
        this.tvLoan.Nodes[1].Checked = accessTable.ContainsKey((object) AclFeature.LoanMgmt_Transfer) && (bool) accessTable[(object) AclFeature.LoanMgmt_Transfer];
      }
      if (userAccessTable.ContainsKey((object) AclFeature.LoanTab_Other_ePASS))
      {
        this.tvLoan.Nodes[2].ImageIndex = this.tvLoan.Nodes[2].SelectedImageIndex = this.tvLoan.Nodes[2].StateImageIndex = this.broken;
        this.tvLoan.Nodes[2].Checked = (AclTriState) userAccessTable[(object) AclFeature.LoanTab_Other_ePASS] == AclTriState.True;
      }
      else
      {
        this.tvLoan.Nodes[2].ImageIndex = this.tvLoan.Nodes[2].SelectedImageIndex = this.tvLoan.Nodes[2].StateImageIndex = this.linked;
        this.tvLoan.Nodes[2].Checked = accessTable.ContainsKey((object) AclFeature.LoanTab_Other_ePASS) && (bool) accessTable[(object) AclFeature.LoanTab_Other_ePASS];
      }
      if (userAccessTable.ContainsKey((object) AclFeature.LoanTab_Other_PlanCode))
      {
        this.tvLoan.Nodes[3].ImageIndex = this.tvLoan.Nodes[3].SelectedImageIndex = this.tvLoan.Nodes[3].StateImageIndex = this.broken;
        this.tvLoan.Nodes[3].Checked = (AclTriState) userAccessTable[(object) AclFeature.LoanTab_Other_PlanCode] == AclTriState.True;
      }
      else
      {
        this.tvLoan.Nodes[3].ImageIndex = this.tvLoan.Nodes[3].SelectedImageIndex = this.tvLoan.Nodes[3].StateImageIndex = this.linked;
        this.tvLoan.Nodes[3].Checked = accessTable.ContainsKey((object) AclFeature.LoanTab_Other_PlanCode) && (bool) accessTable[(object) AclFeature.LoanTab_Other_PlanCode];
      }
      if (userAccessTable.ContainsKey((object) AclFeature.LoanTab_Other_AltLender))
      {
        this.tvLoan.Nodes[4].ImageIndex = this.tvLoan.Nodes[4].SelectedImageIndex = this.tvLoan.Nodes[4].StateImageIndex = this.broken;
        this.tvLoan.Nodes[4].Checked = (AclTriState) userAccessTable[(object) AclFeature.LoanTab_Other_AltLender] == AclTriState.True;
      }
      else
      {
        this.tvLoan.Nodes[4].ImageIndex = this.tvLoan.Nodes[4].SelectedImageIndex = this.tvLoan.Nodes[4].StateImageIndex = this.linked;
        this.tvLoan.Nodes[4].Checked = accessTable.ContainsKey((object) AclFeature.LoanTab_Other_AltLender) && (bool) accessTable[(object) AclFeature.LoanTab_Other_AltLender];
      }
      if (userAccessTable.ContainsKey((object) AclFeature.eFolder_Other_PackagesTab))
      {
        this.tvLoan.Nodes[5].ImageIndex = this.tvLoan.Nodes[5].SelectedImageIndex = this.tvLoan.Nodes[5].StateImageIndex = this.broken;
        this.tvLoan.Nodes[5].Checked = (AclTriState) userAccessTable[(object) AclFeature.eFolder_Other_PackagesTab] == AclTriState.True;
      }
      else
      {
        this.tvLoan.Nodes[5].ImageIndex = this.tvLoan.Nodes[5].SelectedImageIndex = this.tvLoan.Nodes[5].StateImageIndex = this.linked;
        this.tvLoan.Nodes[5].Checked = accessTable.ContainsKey((object) AclFeature.eFolder_Other_PackagesTab) && (bool) accessTable[(object) AclFeature.eFolder_Other_PackagesTab];
      }
      if (userAccessTable.ContainsKey((object) AclFeature.eFolder_Other_eSignPackages))
      {
        this.tvLoan.Nodes[5].Nodes[0].ImageIndex = this.tvLoan.Nodes[5].Nodes[0].SelectedImageIndex = this.tvLoan.Nodes[5].Nodes[0].StateImageIndex = this.broken;
        this.tvLoan.Nodes[5].Nodes[0].Checked = (AclTriState) userAccessTable[(object) AclFeature.eFolder_Other_eSignPackages] == AclTriState.True;
      }
      else
      {
        this.tvLoan.Nodes[5].Nodes[0].ImageIndex = this.tvLoan.Nodes[5].Nodes[0].SelectedImageIndex = this.tvLoan.Nodes[5].Nodes[0].StateImageIndex = this.linked;
        this.tvLoan.Nodes[5].Nodes[0].Checked = accessTable.ContainsKey((object) AclFeature.eFolder_Other_eSignPackages) && (bool) accessTable[(object) AclFeature.eFolder_Other_eSignPackages];
      }
      if (userAccessTable.ContainsKey((object) AclFeature.LoanTab_SearchAllRegs))
      {
        this.tvLoan.Nodes[6].ImageIndex = this.tvLoan.Nodes[6].SelectedImageIndex = this.tvLoan.Nodes[6].StateImageIndex = this.broken;
        this.tvLoan.Nodes[6].Checked = (AclTriState) userAccessTable[(object) AclFeature.LoanTab_SearchAllRegs] == AclTriState.True;
      }
      else
      {
        this.tvLoan.Nodes[6].ImageIndex = this.tvLoan.Nodes[6].SelectedImageIndex = this.tvLoan.Nodes[6].StateImageIndex = this.linked;
        this.tvLoan.Nodes[6].Checked = accessTable.ContainsKey((object) AclFeature.LoanTab_SearchAllRegs) && (bool) accessTable[(object) AclFeature.LoanTab_SearchAllRegs];
      }
      bool flag1 = false;
      bool flag2 = true;
      foreach (AclFeature key in this.contactTopFeatureList.ToArray())
      {
        if (userAccessTable.ContainsKey((object) key))
        {
          flag1 = true;
          break;
        }
      }
      if (flag1)
      {
        this.tvContact.Nodes[0].ImageIndex = this.tvContact.Nodes[0].SelectedImageIndex = this.tvContact.Nodes[0].StateImageIndex = this.broken;
        foreach (AclFeature key in this.contactTopFeatureList.ToArray())
        {
          if (!userAccessTable.ContainsKey((object) key) || (AclTriState) userAccessTable[(object) key] != AclTriState.True)
          {
            flag2 = false;
            break;
          }
        }
      }
      else
      {
        this.tvContact.Nodes[0].ImageIndex = this.tvContact.Nodes[0].SelectedImageIndex = this.tvContact.Nodes[0].StateImageIndex = this.linked;
        foreach (AclFeature key in this.contactTopFeatureList.ToArray())
        {
          if (!accessTable.ContainsKey((object) key) || !(bool) accessTable[(object) key])
          {
            flag2 = false;
            break;
          }
        }
      }
      this.tvContact.Nodes[0].Checked = flag2;
      if (userAccessTable.ContainsKey((object) AclFeature.Cnt_Borrower_Export) || userAccessTable.ContainsKey((object) AclFeature.Cnt_Biz_Export))
      {
        this.tvContact.Nodes[0].Nodes[0].ImageIndex = this.tvContact.Nodes[0].SelectedImageIndex = this.tvContact.Nodes[0].Nodes[0].StateImageIndex = this.broken;
        this.tvContact.Nodes[0].Nodes[0].Checked = userAccessTable.ContainsKey((object) AclFeature.Cnt_Borrower_Export) && (AclTriState) userAccessTable[(object) AclFeature.Cnt_Borrower_Export] == AclTriState.True && userAccessTable.ContainsKey((object) AclFeature.Cnt_Biz_Export) && (AclTriState) userAccessTable[(object) AclFeature.Cnt_Biz_Export] == AclTriState.True;
      }
      else
      {
        this.tvContact.Nodes[0].Nodes[0].ImageIndex = this.tvContact.Nodes[0].Nodes[0].SelectedImageIndex = this.tvContact.Nodes[0].Nodes[0].StateImageIndex = this.linked;
        this.tvContact.Nodes[0].Nodes[0].Checked = accessTable.ContainsKey((object) AclFeature.Cnt_Borrower_Export) && (bool) accessTable[(object) AclFeature.Cnt_Borrower_Export] && accessTable.ContainsKey((object) AclFeature.Cnt_Biz_Export) && (bool) accessTable[(object) AclFeature.Cnt_Biz_Export];
      }
      if (userAccessTable.ContainsKey((object) AclFeature.Cnt_Synchronization))
      {
        this.tvContact.Nodes[0].Nodes[1].ImageIndex = this.tvContact.Nodes[0].Nodes[1].SelectedImageIndex = this.tvContact.Nodes[0].Nodes[1].StateImageIndex = this.broken;
        this.tvContact.Nodes[0].Nodes[1].Checked = (AclTriState) userAccessTable[(object) AclFeature.Cnt_Synchronization] == AclTriState.True;
      }
      else
      {
        this.tvContact.Nodes[0].Nodes[1].ImageIndex = this.tvContact.Nodes[0].Nodes[1].SelectedImageIndex = this.tvContact.Nodes[0].Nodes[1].StateImageIndex = this.linked;
        this.tvContact.Nodes[0].Nodes[1].Checked = accessTable.ContainsKey((object) AclFeature.Cnt_Synchronization) && (bool) accessTable[(object) AclFeature.Cnt_Synchronization];
      }
      if (userAccessTable.ContainsKey((object) AclFeature.Cnt_Campaign_Access))
      {
        this.tvContact.Nodes[0].Nodes[2].ImageIndex = this.tvContact.Nodes[0].Nodes[2].SelectedImageIndex = this.tvContact.Nodes[0].Nodes[2].StateImageIndex = this.broken;
        this.tvContact.Nodes[0].Nodes[2].Checked = (AclTriState) userAccessTable[(object) AclFeature.Cnt_Campaign_Access] == AclTriState.True;
      }
      else
      {
        this.tvContact.Nodes[0].Nodes[2].ImageIndex = this.tvContact.Nodes[0].Nodes[2].SelectedImageIndex = this.tvContact.Nodes[0].Nodes[2].StateImageIndex = this.linked;
        this.tvContact.Nodes[0].Nodes[2].Checked = accessTable.ContainsKey((object) AclFeature.Cnt_Campaign_Access) && (bool) accessTable[(object) AclFeature.Cnt_Campaign_Access];
      }
      if (userAccessTable.ContainsKey((object) AclFeature.Cnt_Campaign_AssignTaskToOther))
      {
        this.tvContact.Nodes[0].Nodes[2].Nodes[0].ImageIndex = this.tvContact.Nodes[0].Nodes[2].Nodes[0].SelectedImageIndex = this.tvContact.Nodes[0].Nodes[2].Nodes[0].StateImageIndex = this.broken;
        this.tvContact.Nodes[0].Nodes[2].Nodes[0].Checked = (AclTriState) userAccessTable[(object) AclFeature.Cnt_Campaign_AssignTaskToOther] == AclTriState.True;
      }
      else
      {
        this.tvContact.Nodes[0].Nodes[2].Nodes[0].ImageIndex = this.tvContact.Nodes[0].Nodes[2].Nodes[0].SelectedImageIndex = this.tvContact.Nodes[0].Nodes[2].Nodes[0].StateImageIndex = this.linked;
        this.tvContact.Nodes[0].Nodes[2].Nodes[0].Checked = accessTable.ContainsKey((object) AclFeature.Cnt_Campaign_AssignTaskToOther) && (bool) accessTable[(object) AclFeature.Cnt_Campaign_AssignTaskToOther];
      }
      if (userAccessTable.ContainsKey((object) AclFeature.Borrower_Contacts_Personal_CustomLetters) || userAccessTable.ContainsKey((object) AclFeature.Business_Contacts_Personal_CustomLetters) || userAccessTable.ContainsKey((object) AclFeature.Cnt_Campaign_PersonalTemplates))
      {
        this.tvContact.Nodes[0].Nodes[3].ImageIndex = this.tvContact.Nodes[0].Nodes[3].SelectedImageIndex = this.tvContact.Nodes[0].Nodes[3].StateImageIndex = this.broken;
        this.tvContact.Nodes[0].Nodes[3].Checked = userAccessTable.ContainsKey((object) AclFeature.Borrower_Contacts_Personal_CustomLetters) && (AclTriState) userAccessTable[(object) AclFeature.Borrower_Contacts_Personal_CustomLetters] == AclTriState.True && userAccessTable.ContainsKey((object) AclFeature.Business_Contacts_Personal_CustomLetters) && (AclTriState) userAccessTable[(object) AclFeature.Business_Contacts_Personal_CustomLetters] == AclTriState.True && userAccessTable.ContainsKey((object) AclFeature.Cnt_Campaign_PersonalTemplates) && (AclTriState) userAccessTable[(object) AclFeature.Cnt_Campaign_PersonalTemplates] == AclTriState.True;
      }
      else
      {
        this.tvContact.Nodes[0].Nodes[3].ImageIndex = this.tvContact.Nodes[0].Nodes[3].SelectedImageIndex = this.tvContact.Nodes[0].Nodes[3].StateImageIndex = this.linked;
        this.tvContact.Nodes[0].Nodes[3].Checked = accessTable.ContainsKey((object) AclFeature.Borrower_Contacts_Personal_CustomLetters) && (bool) accessTable[(object) AclFeature.Borrower_Contacts_Personal_CustomLetters] && accessTable.ContainsKey((object) AclFeature.Business_Contacts_Personal_CustomLetters) && (bool) accessTable[(object) AclFeature.Business_Contacts_Personal_CustomLetters] && accessTable.ContainsKey((object) AclFeature.Cnt_Campaign_PersonalTemplates) && (bool) accessTable[(object) AclFeature.Cnt_Campaign_PersonalTemplates];
      }
      if (userAccessTable.ContainsKey((object) AclFeature.ReportTab_LoanReport) || userAccessTable.ContainsKey((object) AclFeature.ReportTab_BorrowerContactReport) || userAccessTable.ContainsKey((object) AclFeature.ReportTab_BusinessContactReport))
      {
        this.tvDashabordReport.Nodes[0].ImageIndex = this.tvDashabordReport.Nodes[0].SelectedImageIndex = this.tvDashabordReport.Nodes[0].StateImageIndex = this.broken;
        this.tvDashabordReport.Nodes[0].Checked = userAccessTable.ContainsKey((object) AclFeature.ReportTab_LoanReport) && (AclTriState) userAccessTable[(object) AclFeature.ReportTab_LoanReport] == AclTriState.True && userAccessTable.ContainsKey((object) AclFeature.ReportTab_BorrowerContactReport) && (AclTriState) userAccessTable[(object) AclFeature.ReportTab_BorrowerContactReport] == AclTriState.True && userAccessTable.ContainsKey((object) AclFeature.ReportTab_BusinessContactReport) && (AclTriState) userAccessTable[(object) AclFeature.ReportTab_BusinessContactReport] == AclTriState.True;
      }
      else
      {
        this.tvDashabordReport.Nodes[0].ImageIndex = this.tvDashabordReport.Nodes[0].SelectedImageIndex = this.tvDashabordReport.Nodes[0].StateImageIndex = this.linked;
        this.tvDashabordReport.Nodes[0].Checked = accessTable.ContainsKey((object) AclFeature.ReportTab_LoanReport) && (bool) accessTable[(object) AclFeature.ReportTab_LoanReport] && accessTable.ContainsKey((object) AclFeature.ReportTab_BorrowerContactReport) && (bool) accessTable[(object) AclFeature.ReportTab_BorrowerContactReport] && accessTable.ContainsKey((object) AclFeature.ReportTab_BusinessContactReport) && (bool) accessTable[(object) AclFeature.ReportTab_BusinessContactReport];
      }
      if (userAccessTable.ContainsKey((object) AclFeature.ReportTab_ReportingDB))
      {
        this.tvDashabordReport.Nodes[0].Nodes[0].ImageIndex = this.tvDashabordReport.Nodes[0].Nodes[0].SelectedImageIndex = this.tvDashabordReport.Nodes[0].Nodes[0].StateImageIndex = this.broken;
        this.tvDashabordReport.Nodes[0].Nodes[0].Checked = (AclTriState) userAccessTable[(object) AclFeature.ReportTab_ReportingDB] == AclTriState.True;
      }
      else
      {
        this.tvDashabordReport.Nodes[0].Nodes[0].ImageIndex = this.tvDashabordReport.Nodes[0].Nodes[0].SelectedImageIndex = this.tvDashabordReport.Nodes[0].Nodes[0].StateImageIndex = this.linked;
        this.tvDashabordReport.Nodes[0].Nodes[0].Checked = accessTable.ContainsKey((object) AclFeature.ReportTab_ReportingDB) && (bool) accessTable[(object) AclFeature.ReportTab_ReportingDB];
      }
      if (userAccessTable.ContainsKey((object) AclFeature.ReportTab_PersonalTemplate))
      {
        this.tvDashabordReport.Nodes[0].Nodes[1].ImageIndex = this.tvDashabordReport.Nodes[0].Nodes[1].SelectedImageIndex = this.tvDashabordReport.Nodes[0].Nodes[1].StateImageIndex = this.broken;
        this.tvDashabordReport.Nodes[0].Nodes[1].Checked = (AclTriState) userAccessTable[(object) AclFeature.ReportTab_PersonalTemplate] == AclTriState.True;
      }
      else
      {
        this.tvDashabordReport.Nodes[0].Nodes[1].ImageIndex = this.tvDashabordReport.Nodes[0].Nodes[1].SelectedImageIndex = this.tvDashabordReport.Nodes[0].Nodes[1].StateImageIndex = this.linked;
        this.tvDashabordReport.Nodes[0].Nodes[1].Checked = accessTable.ContainsKey((object) AclFeature.ReportTab_PersonalTemplate) && (bool) accessTable[(object) AclFeature.ReportTab_PersonalTemplate];
      }
      if (userAccessTable.ContainsKey((object) AclFeature.DashboardTab_Dashboard))
      {
        this.tvDashabordReport.Nodes[1].ImageIndex = this.tvDashabordReport.Nodes[1].SelectedImageIndex = this.tvDashabordReport.Nodes[1].StateImageIndex = this.broken;
        this.tvDashabordReport.Nodes[1].Checked = (AclTriState) userAccessTable[(object) AclFeature.DashboardTab_Dashboard] == AclTriState.True;
      }
      else
      {
        this.tvDashabordReport.Nodes[1].ImageIndex = this.tvDashabordReport.Nodes[1].SelectedImageIndex = this.tvDashabordReport.Nodes[1].StateImageIndex = this.linked;
        this.tvDashabordReport.Nodes[1].Checked = accessTable.ContainsKey((object) AclFeature.DashboardTab_Dashboard) && (bool) accessTable[(object) AclFeature.DashboardTab_Dashboard];
      }
      if (userAccessTable.ContainsKey((object) AclFeature.DashboardTab_ManagePersonalTemplate) || userAccessTable.ContainsKey((object) AclFeature.DashboardTab_ManagePersonalViewTemplate))
      {
        this.tvDashabordReport.Nodes[1].Nodes[0].ImageIndex = this.tvDashabordReport.Nodes[1].Nodes[0].SelectedImageIndex = this.tvDashabordReport.Nodes[1].Nodes[0].StateImageIndex = this.broken;
        this.tvDashabordReport.Nodes[1].Nodes[0].Checked = userAccessTable.ContainsKey((object) AclFeature.DashboardTab_ManagePersonalTemplate) && (AclTriState) userAccessTable[(object) AclFeature.DashboardTab_ManagePersonalTemplate] == AclTriState.True && userAccessTable.ContainsKey((object) AclFeature.DashboardTab_ManagePersonalViewTemplate) && (AclTriState) userAccessTable[(object) AclFeature.DashboardTab_ManagePersonalViewTemplate] == AclTriState.True;
      }
      else
      {
        this.tvDashabordReport.Nodes[1].Nodes[0].ImageIndex = this.tvDashabordReport.Nodes[1].Nodes[0].SelectedImageIndex = this.tvDashabordReport.Nodes[1].Nodes[0].StateImageIndex = this.linked;
        this.tvDashabordReport.Nodes[1].Nodes[0].Checked = accessTable.ContainsKey((object) AclFeature.DashboardTab_ManagePersonalTemplate) && (bool) accessTable[(object) AclFeature.DashboardTab_ManagePersonalTemplate] && accessTable.ContainsKey((object) AclFeature.DashboardTab_ManagePersonalViewTemplate) && (bool) accessTable[(object) AclFeature.DashboardTab_ManagePersonalViewTemplate];
      }
      if (userAccessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_CustomPrintForms) || userAccessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_PrintGroups))
      {
        this.tvSettings.Nodes[0].ImageIndex = this.tvSettings.Nodes[0].SelectedImageIndex = this.tvSettings.Nodes[0].StateImageIndex = this.broken;
        this.tvSettings.Nodes[0].Checked = userAccessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_CustomPrintForms) && (AclTriState) userAccessTable[(object) AclFeature.SettingsTab_Personal_CustomPrintForms] == AclTriState.True && userAccessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_PrintGroups) && (AclTriState) userAccessTable[(object) AclFeature.SettingsTab_Personal_PrintGroups] == AclTriState.True;
      }
      else
      {
        this.tvSettings.Nodes[0].ImageIndex = this.tvSettings.Nodes[0].SelectedImageIndex = this.tvSettings.Nodes[0].StateImageIndex = this.linked;
        this.tvSettings.Nodes[0].Checked = accessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_CustomPrintForms) && (bool) accessTable[(object) AclFeature.SettingsTab_Personal_CustomPrintForms] && accessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_PrintGroups) && (bool) accessTable[(object) AclFeature.SettingsTab_Personal_PrintGroups];
      }
      if (userAccessTable.ContainsKey((object) AclFeature.SettingsTab_Company_LoanCustomFields))
      {
        this.tvSettings.Nodes[1].ImageIndex = this.tvSettings.Nodes[1].SelectedImageIndex = this.tvSettings.Nodes[1].StateImageIndex = this.broken;
        this.tvSettings.Nodes[1].Checked = (AclTriState) userAccessTable[(object) AclFeature.SettingsTab_Company_LoanCustomFields] == AclTriState.True;
      }
      else
      {
        this.tvSettings.Nodes[1].ImageIndex = this.tvSettings.Nodes[1].SelectedImageIndex = this.tvSettings.Nodes[1].StateImageIndex = this.linked;
        this.tvSettings.Nodes[1].Checked = accessTable.ContainsKey((object) AclFeature.SettingsTab_Company_LoanCustomFields) && (bool) accessTable[(object) AclFeature.SettingsTab_Company_LoanCustomFields];
      }
      if (userAccessTable.ContainsKey((object) AclFeature.SettingsTab_Company_DocumentSetup))
      {
        this.tvSettings.Nodes[2].ImageIndex = this.tvSettings.Nodes[2].SelectedImageIndex = this.tvSettings.Nodes[2].StateImageIndex = this.broken;
        this.tvSettings.Nodes[2].Checked = (AclTriState) userAccessTable[(object) AclFeature.SettingsTab_Company_DocumentSetup] == AclTriState.True;
      }
      else
      {
        this.tvSettings.Nodes[2].ImageIndex = this.tvSettings.Nodes[2].SelectedImageIndex = this.tvSettings.Nodes[2].StateImageIndex = this.linked;
        this.tvSettings.Nodes[2].Checked = accessTable.ContainsKey((object) AclFeature.SettingsTab_Company_DocumentSetup) && (bool) accessTable[(object) AclFeature.SettingsTab_Company_DocumentSetup];
      }
      if (userAccessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_LoanPrograms) || userAccessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_ClosingCosts) || userAccessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_DocumentSets) || userAccessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_InputFormSets) || userAccessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_SettlementServiceProvider) || userAccessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_Affiliate) || userAccessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_MiscDataTemplates) || userAccessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_LoanTemplateSets) || userAccessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_TaskSets))
      {
        this.tvSettings.Nodes[3].ImageIndex = this.tvSettings.Nodes[3].SelectedImageIndex = this.tvSettings.Nodes[3].StateImageIndex = this.broken;
        this.tvSettings.Nodes[3].Checked = userAccessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_LoanPrograms) && (AclTriState) userAccessTable[(object) AclFeature.SettingsTab_Personal_LoanPrograms] == AclTriState.True && userAccessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_ClosingCosts) && (AclTriState) userAccessTable[(object) AclFeature.SettingsTab_Personal_ClosingCosts] == AclTriState.True && userAccessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_DocumentSets) && (AclTriState) userAccessTable[(object) AclFeature.SettingsTab_Personal_DocumentSets] == AclTriState.True && userAccessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_InputFormSets) && (AclTriState) userAccessTable[(object) AclFeature.SettingsTab_Personal_InputFormSets] == AclTriState.True && userAccessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_SettlementServiceProvider) && (AclTriState) userAccessTable[(object) AclFeature.SettingsTab_Personal_SettlementServiceProvider] == AclTriState.True && userAccessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_Affiliate) && (AclTriState) userAccessTable[(object) AclFeature.SettingsTab_Personal_Affiliate] == AclTriState.True && userAccessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_MiscDataTemplates) && (AclTriState) userAccessTable[(object) AclFeature.SettingsTab_Personal_MiscDataTemplates] == AclTriState.True && userAccessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_LoanTemplateSets) && (AclTriState) userAccessTable[(object) AclFeature.SettingsTab_Personal_LoanTemplateSets] == AclTriState.True && userAccessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_TaskSets) && (AclTriState) userAccessTable[(object) AclFeature.SettingsTab_Personal_TaskSets] == AclTriState.True;
      }
      else
      {
        this.tvSettings.Nodes[3].ImageIndex = this.tvSettings.Nodes[3].SelectedImageIndex = this.tvSettings.Nodes[3].StateImageIndex = this.linked;
        this.tvSettings.Nodes[3].Checked = accessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_LoanPrograms) && (bool) accessTable[(object) AclFeature.SettingsTab_Personal_LoanPrograms] && accessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_ClosingCosts) && (bool) accessTable[(object) AclFeature.SettingsTab_Personal_ClosingCosts] && accessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_DocumentSets) && (bool) accessTable[(object) AclFeature.SettingsTab_Personal_DocumentSets] && accessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_InputFormSets) && (bool) accessTable[(object) AclFeature.SettingsTab_Personal_InputFormSets] && accessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_SettlementServiceProvider) && (bool) accessTable[(object) AclFeature.SettingsTab_Personal_SettlementServiceProvider] && accessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_Affiliate) && (bool) accessTable[(object) AclFeature.SettingsTab_Personal_Affiliate] && accessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_MiscDataTemplates) && (bool) accessTable[(object) AclFeature.SettingsTab_Personal_MiscDataTemplates] && accessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_LoanTemplateSets) && (bool) accessTable[(object) AclFeature.SettingsTab_Personal_LoanTemplateSets] && accessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_TaskSets) && (bool) accessTable[(object) AclFeature.SettingsTab_Personal_TaskSets];
      }
      if (userAccessTable.ContainsKey((object) AclFeature.SettingsTab_Company_TableFee))
      {
        this.tvSettings.Nodes[4].ImageIndex = this.tvSettings.Nodes[4].SelectedImageIndex = this.tvSettings.Nodes[4].StateImageIndex = this.broken;
        this.tvSettings.Nodes[4].Checked = (AclTriState) userAccessTable[(object) AclFeature.SettingsTab_Company_TableFee] == AclTriState.True;
      }
      else
      {
        this.tvSettings.Nodes[4].ImageIndex = this.tvSettings.Nodes[4].SelectedImageIndex = this.tvSettings.Nodes[4].StateImageIndex = this.linked;
        this.tvSettings.Nodes[4].Checked = accessTable.ContainsKey((object) AclFeature.SettingsTab_Company_TableFee) && (bool) accessTable[(object) AclFeature.SettingsTab_Company_TableFee];
      }
      if (userAccessTable.ContainsKey((object) AclFeature.SettingsTab_Company_TF_2010ItemizationFeeMgmt))
      {
        this.tvSettings.Nodes[4].Nodes[0].ImageIndex = this.tvSettings.Nodes[4].Nodes[0].SelectedImageIndex = this.tvSettings.Nodes[4].Nodes[0].StateImageIndex = this.broken;
        this.tvSettings.Nodes[4].Nodes[0].Checked = (AclTriState) userAccessTable[(object) AclFeature.SettingsTab_Company_TF_2010ItemizationFeeMgmt] == AclTriState.True;
      }
      else
      {
        this.tvSettings.Nodes[4].Nodes[0].ImageIndex = this.tvSettings.Nodes[4].Nodes[0].SelectedImageIndex = this.tvSettings.Nodes[4].Nodes[0].StateImageIndex = this.linked;
        this.tvSettings.Nodes[4].Nodes[0].Checked = accessTable.ContainsKey((object) AclFeature.SettingsTab_Company_TF_2010ItemizationFeeMgmt) && (bool) accessTable[(object) AclFeature.SettingsTab_Company_TF_2010ItemizationFeeMgmt];
      }
      if (userAccessTable.ContainsKey((object) AclFeature.LoanMgmt_Export_ULAD_ForDu))
      {
        this.tvPipelineTasks.Nodes[0].ImageIndex = this.tvPipelineTasks.Nodes[0].SelectedImageIndex = this.tvPipelineTasks.Nodes[0].StateImageIndex = this.broken;
        this.tvPipelineTasks.Nodes[0].Checked = (AclTriState) userAccessTable[(object) AclFeature.LoanMgmt_Export_ULAD_ForDu] == AclTriState.True;
      }
      else
      {
        this.tvPipelineTasks.Nodes[0].ImageIndex = this.tvPipelineTasks.Nodes[0].SelectedImageIndex = this.tvPipelineTasks.Nodes[0].StateImageIndex = this.linked;
        this.tvPipelineTasks.Nodes[0].Checked = accessTable.ContainsKey((object) AclFeature.LoanMgmt_Export_ULAD_ForDu) && (bool) accessTable[(object) AclFeature.LoanMgmt_Export_ULAD_ForDu];
      }
      if (userAccessTable.ContainsKey((object) AclFeature.LoanMgmt_Export_ILAD))
      {
        this.tvPipelineTasks.Nodes[1].ImageIndex = this.tvPipelineTasks.Nodes[1].SelectedImageIndex = this.tvPipelineTasks.Nodes[1].StateImageIndex = this.broken;
        this.tvPipelineTasks.Nodes[1].Checked = (AclTriState) userAccessTable[(object) AclFeature.LoanMgmt_Export_ILAD] == AclTriState.True;
      }
      else
      {
        this.tvPipelineTasks.Nodes[1].ImageIndex = this.tvPipelineTasks.Nodes[1].SelectedImageIndex = this.tvPipelineTasks.Nodes[1].StateImageIndex = this.linked;
        this.tvPipelineTasks.Nodes[1].Checked = accessTable.ContainsKey((object) AclFeature.LoanMgmt_Export_ILAD) && (bool) accessTable[(object) AclFeature.LoanMgmt_Export_ILAD];
      }
      if (userAccessTable.ContainsKey((object) AclFeature.LoanMgmt_Export_FannieMae_FormattedFile))
      {
        this.tvPipelineTasks.Nodes[2].ImageIndex = this.tvPipelineTasks.Nodes[2].SelectedImageIndex = this.tvPipelineTasks.Nodes[2].StateImageIndex = this.broken;
        this.tvPipelineTasks.Nodes[2].Checked = (AclTriState) userAccessTable[(object) AclFeature.LoanMgmt_Export_FannieMae_FormattedFile] == AclTriState.True;
      }
      else
      {
        this.tvPipelineTasks.Nodes[2].ImageIndex = this.tvPipelineTasks.Nodes[2].SelectedImageIndex = this.tvPipelineTasks.Nodes[2].StateImageIndex = this.linked;
        this.tvPipelineTasks.Nodes[2].Checked = accessTable.ContainsKey((object) AclFeature.LoanMgmt_Export_FannieMae_FormattedFile) && (bool) accessTable[(object) AclFeature.LoanMgmt_Export_FannieMae_FormattedFile];
      }
      if (this.loanSoftArchivalEnabled)
      {
        if (userAccessTable.ContainsKey((object) AclFeature.LoanMgmt_AccessToArchiveFolders))
        {
          this.tvPipelineTasks.Nodes[3].ImageIndex = this.tvPipelineTasks.Nodes[3].SelectedImageIndex = this.tvPipelineTasks.Nodes[3].StateImageIndex = this.broken;
          this.tvPipelineTasks.Nodes[3].Checked = (AclTriState) userAccessTable[(object) AclFeature.LoanMgmt_AccessToArchiveFolders] == AclTriState.True;
        }
        else
        {
          this.tvPipelineTasks.Nodes[3].ImageIndex = this.tvPipelineTasks.Nodes[3].SelectedImageIndex = this.tvPipelineTasks.Nodes[3].StateImageIndex = this.linked;
          this.tvPipelineTasks.Nodes[3].Checked = accessTable.ContainsKey((object) AclFeature.LoanMgmt_AccessToArchiveFolders) && (bool) accessTable[(object) AclFeature.LoanMgmt_AccessToArchiveFolders];
        }
        if (userAccessTable.ContainsKey((object) AclFeature.LoanMgmt_AccessToArchiveLoans))
        {
          this.tvPipelineTasks.Nodes[4].ImageIndex = this.tvPipelineTasks.Nodes[4].SelectedImageIndex = this.tvPipelineTasks.Nodes[4].StateImageIndex = this.broken;
          this.tvPipelineTasks.Nodes[4].Checked = (AclTriState) userAccessTable[(object) AclFeature.LoanMgmt_AccessToArchiveLoans] == AclTriState.True;
        }
        else
        {
          this.tvPipelineTasks.Nodes[4].ImageIndex = this.tvPipelineTasks.Nodes[4].SelectedImageIndex = this.tvPipelineTasks.Nodes[4].StateImageIndex = this.linked;
          this.tvPipelineTasks.Nodes[4].Checked = accessTable.ContainsKey((object) AclFeature.LoanMgmt_AccessToArchiveLoans) && (bool) accessTable[(object) AclFeature.LoanMgmt_AccessToArchiveLoans];
        }
      }
      if (this.userAccessSetting.ContainsKey((object) AclFeature.PlatForm_Access))
      {
        if (this.userAccessSetting.ContainsKey((object) AclFeature.PlatForm_Access) && Convert.ToBoolean(this.userAccessSetting[(object) AclFeature.PlatForm_Access]))
        {
          this.rdoBoth.Checked = true;
          this.rdoDesktop.Checked = false;
        }
        else
        {
          this.rdoDesktop.Checked = true;
          this.rdoBoth.Checked = false;
        }
      }
      else if (accessTable.ContainsKey((object) AclFeature.PlatForm_Access) && Convert.ToBoolean(accessTable[(object) AclFeature.PlatForm_Access]))
      {
        this.rdoBoth.Checked = true;
        this.rdoDesktop.Checked = false;
      }
      else
      {
        this.rdoDesktop.Checked = true;
        this.rdoBoth.Checked = false;
      }
      if (userAccessTable.ContainsKey((object) AclFeature.DeveloperConnectTab_SubscribetoWebhooks))
      {
        this.tvDeveloperConnect.Nodes[0].ImageIndex = this.tvDeveloperConnect.Nodes[0].SelectedImageIndex = this.tvDeveloperConnect.Nodes[0].StateImageIndex = this.broken;
        this.tvDeveloperConnect.Nodes[0].Checked = (AclTriState) userAccessTable[(object) AclFeature.DeveloperConnectTab_SubscribetoWebhooks] == AclTriState.True;
      }
      else
      {
        this.tvDeveloperConnect.Nodes[0].ImageIndex = this.tvDeveloperConnect.Nodes[0].SelectedImageIndex = this.tvDeveloperConnect.Nodes[0].StateImageIndex = this.linked;
        this.tvDeveloperConnect.Nodes[0].Checked = accessTable.ContainsKey((object) AclFeature.DeveloperConnectTab_SubscribetoWebhooks) && (bool) accessTable[(object) AclFeature.DeveloperConnectTab_SubscribetoWebhooks];
      }
      if (userAccessTable.ContainsKey((object) AclFeature.DeveloperConnectTab_SubscribetoWebhooks_EnhancedFieldChange))
      {
        this.tvDeveloperConnect.Nodes[0].Nodes[0].ImageIndex = this.tvDeveloperConnect.Nodes[0].Nodes[0].SelectedImageIndex = this.tvDeveloperConnect.Nodes[0].Nodes[0].StateImageIndex = this.broken;
        this.tvDeveloperConnect.Nodes[0].Nodes[0].Checked = (AclTriState) userAccessTable[(object) AclFeature.DeveloperConnectTab_SubscribetoWebhooks_EnhancedFieldChange] == AclTriState.True;
      }
      else
      {
        this.tvDeveloperConnect.Nodes[0].Nodes[0].ImageIndex = this.tvDeveloperConnect.Nodes[0].Nodes[0].SelectedImageIndex = this.tvDeveloperConnect.Nodes[0].Nodes[0].StateImageIndex = this.linked;
        this.tvDeveloperConnect.Nodes[0].Nodes[0].Checked = accessTable.ContainsKey((object) AclFeature.DeveloperConnectTab_SubscribetoWebhooks_EnhancedFieldChange) && (bool) accessTable[(object) AclFeature.DeveloperConnectTab_SubscribetoWebhooks_EnhancedFieldChange];
      }
      this.suspendEvent = false;
      this.registerTreeNodeCheckEvent(true);
    }

    private void populateTreeValue(Hashtable accessTable)
    {
      this.suspendEvent = true;
      this.tvLoan.Nodes[0].Checked = accessTable.ContainsKey((object) AclFeature.LoanMgmt_Import) && (bool) accessTable[(object) AclFeature.LoanMgmt_Import];
      this.tvLoan.Nodes[1].Checked = accessTable.ContainsKey((object) AclFeature.LoanMgmt_Transfer) && (bool) accessTable[(object) AclFeature.LoanMgmt_Transfer];
      this.tvLoan.Nodes[2].Checked = accessTable.ContainsKey((object) AclFeature.LoanTab_Other_ePASS) && (bool) accessTable[(object) AclFeature.LoanTab_Other_ePASS];
      this.tvLoan.Nodes[3].Checked = accessTable.ContainsKey((object) AclFeature.LoanTab_Other_PlanCode) && (bool) accessTable[(object) AclFeature.LoanTab_Other_PlanCode];
      this.tvLoan.Nodes[4].Checked = accessTable.ContainsKey((object) AclFeature.LoanTab_Other_AltLender) && (bool) accessTable[(object) AclFeature.LoanTab_Other_AltLender];
      this.tvLoan.Nodes[5].Checked = accessTable.ContainsKey((object) AclFeature.eFolder_Other_PackagesTab) && (bool) accessTable[(object) AclFeature.eFolder_Other_PackagesTab];
      this.tvLoan.Nodes[5].Nodes[0].Checked = accessTable.ContainsKey((object) AclFeature.eFolder_Other_eSignPackages) && (bool) accessTable[(object) AclFeature.eFolder_Other_eSignPackages];
      this.tvLoan.Nodes[6].Checked = accessTable.ContainsKey((object) AclFeature.LoanTab_SearchAllRegs) && (bool) accessTable[(object) AclFeature.LoanTab_SearchAllRegs];
      this.tvPipelineTasks.Nodes[0].Checked = accessTable.ContainsKey((object) AclFeature.LoanMgmt_Export_ULAD_ForDu) && (bool) accessTable[(object) AclFeature.LoanMgmt_Export_ULAD_ForDu];
      this.tvPipelineTasks.Nodes[1].Checked = accessTable.ContainsKey((object) AclFeature.LoanMgmt_Export_ILAD) && (bool) accessTable[(object) AclFeature.LoanMgmt_Export_ILAD];
      this.tvPipelineTasks.Nodes[2].Checked = accessTable.ContainsKey((object) AclFeature.LoanMgmt_Export_FannieMae_FormattedFile) && (bool) accessTable[(object) AclFeature.LoanMgmt_Export_FannieMae_FormattedFile];
      if (this.loanSoftArchivalEnabled)
      {
        this.tvPipelineTasks.Nodes[3].Checked = accessTable.ContainsKey((object) AclFeature.LoanMgmt_AccessToArchiveFolders) && (bool) accessTable[(object) AclFeature.LoanMgmt_AccessToArchiveFolders];
        this.tvPipelineTasks.Nodes[4].Checked = accessTable.ContainsKey((object) AclFeature.LoanMgmt_AccessToArchiveLoans) && (bool) accessTable[(object) AclFeature.LoanMgmt_AccessToArchiveLoans];
      }
      this.tvDeveloperConnect.Nodes[0].Checked = accessTable.ContainsKey((object) AclFeature.DeveloperConnectTab_SubscribetoWebhooks) && (bool) accessTable[(object) AclFeature.DeveloperConnectTab_SubscribetoWebhooks];
      this.tvDeveloperConnect.Nodes[0].Nodes[0].Checked = accessTable.ContainsKey((object) AclFeature.DeveloperConnectTab_SubscribetoWebhooks_EnhancedFieldChange) && (bool) accessTable[(object) AclFeature.DeveloperConnectTab_SubscribetoWebhooks_EnhancedFieldChange];
      bool flag = true;
      foreach (AclFeature key in this.contactTopFeatureList.ToArray())
      {
        if (!accessTable.ContainsKey((object) key) || !(bool) accessTable[(object) key])
        {
          flag = false;
          break;
        }
      }
      this.tvContact.Nodes[0].Checked = flag;
      this.tvContact.Nodes[0].Nodes[0].Checked = accessTable.ContainsKey((object) AclFeature.Cnt_Borrower_Export) && (bool) accessTable[(object) AclFeature.Cnt_Biz_Export];
      this.tvContact.Nodes[0].Nodes[1].Checked = accessTable.ContainsKey((object) AclFeature.Cnt_Synchronization) && (bool) accessTable[(object) AclFeature.Cnt_Synchronization];
      this.tvContact.Nodes[0].Nodes[2].Checked = accessTable.ContainsKey((object) AclFeature.Cnt_Campaign_Access) && (bool) accessTable[(object) AclFeature.Cnt_Campaign_Access];
      this.tvContact.Nodes[0].Nodes[2].Nodes[0].Checked = accessTable.ContainsKey((object) AclFeature.Cnt_Campaign_AssignTaskToOther) && (bool) accessTable[(object) AclFeature.Cnt_Campaign_AssignTaskToOther];
      this.tvContact.Nodes[0].Nodes[3].Checked = accessTable.ContainsKey((object) AclFeature.Borrower_Contacts_Personal_CustomLetters) && (bool) accessTable[(object) AclFeature.Borrower_Contacts_Personal_CustomLetters] && accessTable.ContainsKey((object) AclFeature.Business_Contacts_Personal_CustomLetters) && (bool) accessTable[(object) AclFeature.Business_Contacts_Personal_CustomLetters] && accessTable.ContainsKey((object) AclFeature.Cnt_Campaign_PersonalTemplates) && (bool) accessTable[(object) AclFeature.Cnt_Campaign_PersonalTemplates];
      this.tvDashabordReport.Nodes[0].Checked = accessTable.ContainsKey((object) AclFeature.ReportTab_LoanReport) && (bool) accessTable[(object) AclFeature.ReportTab_LoanReport] && accessTable.ContainsKey((object) AclFeature.ReportTab_BorrowerContactReport) && (bool) accessTable[(object) AclFeature.ReportTab_BorrowerContactReport] && accessTable.ContainsKey((object) AclFeature.ReportTab_BusinessContactReport) && (bool) accessTable[(object) AclFeature.ReportTab_BusinessContactReport];
      this.tvDashabordReport.Nodes[0].Nodes[0].Checked = accessTable.ContainsKey((object) AclFeature.ReportTab_ReportingDB) && (bool) accessTable[(object) AclFeature.ReportTab_ReportingDB];
      this.tvDashabordReport.Nodes[0].Nodes[1].Checked = accessTable.ContainsKey((object) AclFeature.ReportTab_PersonalTemplate) && (bool) accessTable[(object) AclFeature.ReportTab_PersonalTemplate];
      this.tvDashabordReport.Nodes[1].Checked = accessTable.ContainsKey((object) AclFeature.DashboardTab_Dashboard) && (bool) accessTable[(object) AclFeature.DashboardTab_Dashboard];
      this.tvDashabordReport.Nodes[1].Nodes[0].Checked = accessTable.ContainsKey((object) AclFeature.DashboardTab_ManagePersonalTemplate) && (bool) accessTable[(object) AclFeature.DashboardTab_ManagePersonalTemplate] && accessTable.ContainsKey((object) AclFeature.DashboardTab_ManagePersonalViewTemplate) && (bool) accessTable[(object) AclFeature.DashboardTab_ManagePersonalViewTemplate];
      this.tvSettings.Nodes[0].Checked = accessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_CustomPrintForms) && (bool) accessTable[(object) AclFeature.SettingsTab_Personal_CustomPrintForms] && accessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_PrintGroups) && (bool) accessTable[(object) AclFeature.SettingsTab_Personal_PrintGroups];
      this.tvSettings.Nodes[1].Checked = accessTable.ContainsKey((object) AclFeature.SettingsTab_Company_LoanCustomFields) && (bool) accessTable[(object) AclFeature.SettingsTab_Company_LoanCustomFields];
      this.tvSettings.Nodes[2].Checked = accessTable.ContainsKey((object) AclFeature.SettingsTab_Company_DocumentSetup) && (bool) accessTable[(object) AclFeature.SettingsTab_Company_DocumentSetup];
      this.tvSettings.Nodes[3].Checked = accessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_LoanPrograms) && (bool) accessTable[(object) AclFeature.SettingsTab_Personal_LoanPrograms] && accessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_ClosingCosts) && (bool) accessTable[(object) AclFeature.SettingsTab_Personal_ClosingCosts] && accessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_DocumentSets) && (bool) accessTable[(object) AclFeature.SettingsTab_Personal_DocumentSets] && accessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_InputFormSets) && (bool) accessTable[(object) AclFeature.SettingsTab_Personal_InputFormSets] && accessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_SettlementServiceProvider) && (bool) accessTable[(object) AclFeature.SettingsTab_Personal_SettlementServiceProvider] && accessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_Affiliate) && (bool) accessTable[(object) AclFeature.SettingsTab_Personal_Affiliate] && accessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_MiscDataTemplates) && (bool) accessTable[(object) AclFeature.SettingsTab_Personal_MiscDataTemplates] && accessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_LoanTemplateSets) && (bool) accessTable[(object) AclFeature.SettingsTab_Personal_LoanTemplateSets] && accessTable.ContainsKey((object) AclFeature.SettingsTab_Personal_TaskSets) && (bool) accessTable[(object) AclFeature.SettingsTab_Personal_TaskSets];
      this.tvSettings.Nodes[4].Checked = accessTable.ContainsKey((object) AclFeature.SettingsTab_Company_TableFee) && (bool) accessTable[(object) AclFeature.SettingsTab_Company_TableFee];
      this.tvSettings.Nodes[4].Nodes[0].Checked = accessTable.ContainsKey((object) AclFeature.SettingsTab_Company_TF_2010ItemizationFeeMgmt) && (bool) accessTable[(object) AclFeature.SettingsTab_Company_TF_2010ItemizationFeeMgmt];
      if (accessTable.ContainsKey((object) AclFeature.PlatForm_Access) && (bool) accessTable[(object) AclFeature.PlatForm_Access])
      {
        this.rdoDesktop.Checked = false;
        this.rdoBoth.Checked = true;
      }
      else
      {
        this.rdoDesktop.Checked = true;
        this.rdoBoth.Checked = false;
      }
      this.suspendEvent = false;
    }

    private Hashtable getAccessTreeFromUI()
    {
      Hashtable accessTreeFromUi = new Hashtable();
      if (this.tvPipelineTasks.Nodes[0].Checked)
      {
        if (this.userID == "")
          accessTreeFromUi.Add((object) AclFeature.LoanMgmt_Export_ULAD_ForDu, (object) AclTriState.True);
        else if (this.tvPipelineTasks.Nodes[0].ImageIndex == this.broken)
          accessTreeFromUi.Add((object) AclFeature.LoanMgmt_Export_ULAD_ForDu, (object) AclTriState.True);
        else
          accessTreeFromUi.Add((object) AclFeature.LoanMgmt_Export_ULAD_ForDu, (object) AclTriState.Unspecified);
      }
      else if (this.userID == "")
        accessTreeFromUi.Add((object) AclFeature.LoanMgmt_Export_ULAD_ForDu, (object) AclTriState.False);
      else if (this.tvPipelineTasks.Nodes[0].ImageIndex == this.broken)
        accessTreeFromUi.Add((object) AclFeature.LoanMgmt_Export_ULAD_ForDu, (object) AclTriState.False);
      else
        accessTreeFromUi.Add((object) AclFeature.LoanMgmt_Export_ULAD_ForDu, (object) AclTriState.Unspecified);
      if (this.tvPipelineTasks.Nodes[1].Checked)
      {
        if (this.userID == "")
          accessTreeFromUi.Add((object) AclFeature.LoanMgmt_Export_ILAD, (object) AclTriState.True);
        else if (this.tvPipelineTasks.Nodes[1].ImageIndex == this.broken)
          accessTreeFromUi.Add((object) AclFeature.LoanMgmt_Export_ILAD, (object) AclTriState.True);
        else
          accessTreeFromUi.Add((object) AclFeature.LoanMgmt_Export_ILAD, (object) AclTriState.Unspecified);
      }
      else if (this.userID == "")
        accessTreeFromUi.Add((object) AclFeature.LoanMgmt_Export_ILAD, (object) AclTriState.False);
      else if (this.tvPipelineTasks.Nodes[1].ImageIndex == this.broken)
        accessTreeFromUi.Add((object) AclFeature.LoanMgmt_Export_ILAD, (object) AclTriState.False);
      else
        accessTreeFromUi.Add((object) AclFeature.LoanMgmt_Export_ILAD, (object) AclTriState.Unspecified);
      if (this.tvPipelineTasks.Nodes[2].Checked)
      {
        if (this.userID == "")
          accessTreeFromUi.Add((object) AclFeature.LoanMgmt_Export_FannieMae_FormattedFile, (object) AclTriState.True);
        else if (this.tvPipelineTasks.Nodes[2].ImageIndex == this.broken)
          accessTreeFromUi.Add((object) AclFeature.LoanMgmt_Export_FannieMae_FormattedFile, (object) AclTriState.True);
        else
          accessTreeFromUi.Add((object) AclFeature.LoanMgmt_Export_FannieMae_FormattedFile, (object) AclTriState.Unspecified);
      }
      else if (this.userID == "")
        accessTreeFromUi.Add((object) AclFeature.LoanMgmt_Export_FannieMae_FormattedFile, (object) AclTriState.False);
      else if (this.tvPipelineTasks.Nodes[2].ImageIndex == this.broken)
        accessTreeFromUi.Add((object) AclFeature.LoanMgmt_Export_FannieMae_FormattedFile, (object) AclTriState.False);
      else
        accessTreeFromUi.Add((object) AclFeature.LoanMgmt_Export_FannieMae_FormattedFile, (object) AclTriState.Unspecified);
      if (this.loanSoftArchivalEnabled)
      {
        if (this.tvPipelineTasks.Nodes[3].Checked)
        {
          if (this.userID == "")
            accessTreeFromUi.Add((object) AclFeature.LoanMgmt_AccessToArchiveFolders, (object) AclTriState.True);
          else if (this.tvPipelineTasks.Nodes[3].ImageIndex == this.broken)
            accessTreeFromUi.Add((object) AclFeature.LoanMgmt_AccessToArchiveFolders, (object) AclTriState.True);
          else
            accessTreeFromUi.Add((object) AclFeature.LoanMgmt_AccessToArchiveFolders, (object) AclTriState.Unspecified);
        }
        else if (this.userID == "")
          accessTreeFromUi.Add((object) AclFeature.LoanMgmt_AccessToArchiveFolders, (object) AclTriState.False);
        else if (this.tvPipelineTasks.Nodes[3].ImageIndex == this.broken)
          accessTreeFromUi.Add((object) AclFeature.LoanMgmt_AccessToArchiveFolders, (object) AclTriState.False);
        else
          accessTreeFromUi.Add((object) AclFeature.LoanMgmt_AccessToArchiveFolders, (object) AclTriState.Unspecified);
        if (this.tvPipelineTasks.Nodes[4].Checked)
        {
          if (this.userID == "")
            accessTreeFromUi.Add((object) AclFeature.LoanMgmt_AccessToArchiveLoans, (object) AclTriState.True);
          else if (this.tvPipelineTasks.Nodes[4].ImageIndex == this.broken)
            accessTreeFromUi.Add((object) AclFeature.LoanMgmt_AccessToArchiveLoans, (object) AclTriState.True);
          else
            accessTreeFromUi.Add((object) AclFeature.LoanMgmt_AccessToArchiveLoans, (object) AclTriState.Unspecified);
        }
        else if (this.userID == "")
          accessTreeFromUi.Add((object) AclFeature.LoanMgmt_AccessToArchiveLoans, (object) AclTriState.False);
        else if (this.tvPipelineTasks.Nodes[4].ImageIndex == this.broken)
          accessTreeFromUi.Add((object) AclFeature.LoanMgmt_AccessToArchiveLoans, (object) AclTriState.False);
        else
          accessTreeFromUi.Add((object) AclFeature.LoanMgmt_AccessToArchiveLoans, (object) AclTriState.Unspecified);
      }
      if (this.tvLoan.Nodes[1].Checked)
      {
        if (this.userID == "")
          accessTreeFromUi.Add((object) AclFeature.LoanMgmt_Transfer, (object) AclTriState.True);
        else if (this.tvLoan.Nodes[1].ImageIndex == this.broken)
          accessTreeFromUi.Add((object) AclFeature.LoanMgmt_Transfer, (object) AclTriState.True);
        else
          accessTreeFromUi.Add((object) AclFeature.LoanMgmt_Transfer, (object) AclTriState.Unspecified);
      }
      else if (this.userID == "")
        accessTreeFromUi.Add((object) AclFeature.LoanMgmt_Transfer, (object) AclTriState.False);
      else if (this.tvLoan.Nodes[1].ImageIndex == this.broken)
        accessTreeFromUi.Add((object) AclFeature.LoanMgmt_Transfer, (object) AclTriState.False);
      else
        accessTreeFromUi.Add((object) AclFeature.LoanMgmt_Transfer, (object) AclTriState.Unspecified);
      if (this.tvLoan.Nodes[2].Checked)
      {
        if (this.userID == "")
          accessTreeFromUi.Add((object) AclFeature.LoanTab_Other_ePASS, (object) AclTriState.True);
        else if (this.tvLoan.Nodes[2].ImageIndex == this.broken)
          accessTreeFromUi.Add((object) AclFeature.LoanTab_Other_ePASS, (object) AclTriState.True);
        else
          accessTreeFromUi.Add((object) AclFeature.LoanTab_Other_ePASS, (object) AclTriState.Unspecified);
      }
      else if (this.userID == "")
        accessTreeFromUi.Add((object) AclFeature.LoanTab_Other_ePASS, (object) AclTriState.False);
      else if (this.tvLoan.Nodes[2].ImageIndex == this.broken)
        accessTreeFromUi.Add((object) AclFeature.LoanTab_Other_ePASS, (object) AclTriState.False);
      else
        accessTreeFromUi.Add((object) AclFeature.LoanTab_Other_ePASS, (object) AclTriState.Unspecified);
      if (this.tvLoan.Nodes[3].Checked)
      {
        if (this.userID == "")
          accessTreeFromUi.Add((object) AclFeature.LoanTab_Other_PlanCode, (object) AclTriState.True);
        else if (this.tvLoan.Nodes[3].ImageIndex == this.broken)
          accessTreeFromUi.Add((object) AclFeature.LoanTab_Other_PlanCode, (object) AclTriState.True);
        else
          accessTreeFromUi.Add((object) AclFeature.LoanTab_Other_PlanCode, (object) AclTriState.Unspecified);
      }
      else if (this.userID == "")
        accessTreeFromUi.Add((object) AclFeature.LoanTab_Other_PlanCode, (object) AclTriState.False);
      else if (this.tvLoan.Nodes[3].ImageIndex == this.broken)
        accessTreeFromUi.Add((object) AclFeature.LoanTab_Other_PlanCode, (object) AclTriState.False);
      else
        accessTreeFromUi.Add((object) AclFeature.LoanTab_Other_PlanCode, (object) AclTriState.Unspecified);
      if (this.tvLoan.Nodes[4].Checked)
      {
        if (this.userID == "")
          accessTreeFromUi.Add((object) AclFeature.LoanTab_Other_AltLender, (object) AclTriState.True);
        else if (this.tvLoan.Nodes[4].ImageIndex == this.broken)
          accessTreeFromUi.Add((object) AclFeature.LoanTab_Other_AltLender, (object) AclTriState.True);
        else
          accessTreeFromUi.Add((object) AclFeature.LoanTab_Other_AltLender, (object) AclTriState.Unspecified);
      }
      else if (this.userID == "")
        accessTreeFromUi.Add((object) AclFeature.LoanTab_Other_AltLender, (object) AclTriState.False);
      else if (this.tvLoan.Nodes[4].ImageIndex == this.broken)
        accessTreeFromUi.Add((object) AclFeature.LoanTab_Other_AltLender, (object) AclTriState.False);
      else
        accessTreeFromUi.Add((object) AclFeature.LoanTab_Other_AltLender, (object) AclTriState.Unspecified);
      if (this.tvLoan.Nodes[5].Checked)
      {
        if (this.userID == "")
          accessTreeFromUi.Add((object) AclFeature.eFolder_Other_PackagesTab, (object) AclTriState.True);
        else if (this.tvLoan.Nodes[5].ImageIndex == this.broken)
          accessTreeFromUi.Add((object) AclFeature.eFolder_Other_PackagesTab, (object) AclTriState.True);
        else
          accessTreeFromUi.Add((object) AclFeature.eFolder_Other_PackagesTab, (object) AclTriState.Unspecified);
        if (this.tvLoan.Nodes[5].Nodes[0].Checked)
        {
          if (this.userID == "")
            accessTreeFromUi.Add((object) AclFeature.eFolder_Other_eSignPackages, (object) AclTriState.True);
          else if (this.tvLoan.Nodes[5].Nodes[0].ImageIndex == this.broken)
            accessTreeFromUi.Add((object) AclFeature.eFolder_Other_eSignPackages, (object) AclTriState.True);
          else
            accessTreeFromUi.Add((object) AclFeature.eFolder_Other_eSignPackages, (object) AclTriState.Unspecified);
        }
        else if (this.userID == "")
          accessTreeFromUi.Add((object) AclFeature.eFolder_Other_eSignPackages, (object) AclTriState.False);
        else if (this.tvLoan.Nodes[5].Nodes[0].ImageIndex == this.broken)
          accessTreeFromUi.Add((object) AclFeature.eFolder_Other_eSignPackages, (object) AclTriState.False);
        else
          accessTreeFromUi.Add((object) AclFeature.eFolder_Other_eSignPackages, (object) AclTriState.Unspecified);
      }
      else if (this.userID == "")
      {
        accessTreeFromUi.Add((object) AclFeature.eFolder_Other_PackagesTab, (object) AclTriState.False);
        accessTreeFromUi.Add((object) AclFeature.eFolder_Other_eSignPackages, (object) AclTriState.False);
      }
      else if (this.tvLoan.Nodes[5].ImageIndex == this.broken)
      {
        accessTreeFromUi.Add((object) AclFeature.eFolder_Other_PackagesTab, (object) AclTriState.False);
        accessTreeFromUi.Add((object) AclFeature.eFolder_Other_eSignPackages, (object) AclTriState.False);
      }
      else
      {
        accessTreeFromUi.Add((object) AclFeature.eFolder_Other_PackagesTab, (object) AclTriState.Unspecified);
        accessTreeFromUi.Add((object) AclFeature.eFolder_Other_eSignPackages, (object) AclTriState.Unspecified);
      }
      if (this.tvLoan.Nodes[6].Checked)
      {
        if (this.userID == "")
          accessTreeFromUi.Add((object) AclFeature.LoanTab_SearchAllRegs, (object) AclTriState.True);
        else if (this.tvLoan.Nodes[6].ImageIndex == this.broken)
          accessTreeFromUi.Add((object) AclFeature.LoanTab_SearchAllRegs, (object) AclTriState.True);
        else
          accessTreeFromUi.Add((object) AclFeature.LoanTab_SearchAllRegs, (object) AclTriState.Unspecified);
      }
      else if (this.userID == "")
        accessTreeFromUi.Add((object) AclFeature.LoanTab_SearchAllRegs, (object) AclTriState.False);
      else if (this.tvLoan.Nodes[6].ImageIndex == this.broken)
        accessTreeFromUi.Add((object) AclFeature.LoanTab_SearchAllRegs, (object) AclTriState.False);
      else
        accessTreeFromUi.Add((object) AclFeature.LoanTab_SearchAllRegs, (object) AclTriState.Unspecified);
      if (this.tvContact.Nodes[0].Checked)
      {
        if (this.userID == "")
        {
          foreach (AclFeature key in this.contactTopFeatureList.ToArray())
            accessTreeFromUi.Add((object) key, (object) AclTriState.True);
        }
        else if (this.tvContact.Nodes[0].ImageIndex == this.broken)
        {
          foreach (AclFeature key in this.contactTopFeatureList.ToArray())
            accessTreeFromUi.Add((object) key, (object) AclTriState.True);
        }
        else
        {
          foreach (AclFeature key in this.contactTopFeatureList.ToArray())
            accessTreeFromUi.Add((object) key, (object) AclTriState.Unspecified);
        }
      }
      else if (this.userID == "")
      {
        foreach (AclFeature key in this.contactTopFeatureList.ToArray())
          accessTreeFromUi.Add((object) key, (object) AclTriState.False);
      }
      else if (this.tvContact.Nodes[0].ImageIndex == this.broken)
      {
        foreach (AclFeature key in this.contactTopFeatureList.ToArray())
          accessTreeFromUi.Add((object) key, (object) AclTriState.False);
      }
      else
      {
        foreach (AclFeature key in this.contactTopFeatureList.ToArray())
          accessTreeFromUi.Add((object) key, (object) AclTriState.Unspecified);
      }
      if (this.tvContact.Nodes[0].Nodes[0].Checked)
      {
        if (this.userID == "")
        {
          accessTreeFromUi.Add((object) AclFeature.Cnt_Biz_Export, (object) AclTriState.True);
          accessTreeFromUi.Add((object) AclFeature.Cnt_Borrower_Export, (object) AclTriState.True);
        }
        else if (this.tvContact.Nodes[0].Nodes[0].ImageIndex == this.broken)
        {
          accessTreeFromUi.Add((object) AclFeature.Cnt_Biz_Export, (object) AclTriState.True);
          accessTreeFromUi.Add((object) AclFeature.Cnt_Borrower_Export, (object) AclTriState.True);
        }
        else
        {
          accessTreeFromUi.Add((object) AclFeature.Cnt_Biz_Export, (object) AclTriState.Unspecified);
          accessTreeFromUi.Add((object) AclFeature.Cnt_Borrower_Export, (object) AclTriState.Unspecified);
        }
      }
      else if (this.userID == "")
      {
        accessTreeFromUi.Add((object) AclFeature.Cnt_Biz_Export, (object) AclTriState.False);
        accessTreeFromUi.Add((object) AclFeature.Cnt_Borrower_Export, (object) AclTriState.False);
      }
      else if (this.tvContact.Nodes[0].Nodes[0].ImageIndex == this.broken)
      {
        accessTreeFromUi.Add((object) AclFeature.Cnt_Biz_Export, (object) AclTriState.False);
        accessTreeFromUi.Add((object) AclFeature.Cnt_Borrower_Export, (object) AclTriState.False);
      }
      else
      {
        accessTreeFromUi.Add((object) AclFeature.Cnt_Biz_Export, (object) AclTriState.Unspecified);
        accessTreeFromUi.Add((object) AclFeature.Cnt_Borrower_Export, (object) AclTriState.Unspecified);
      }
      if (this.tvContact.Nodes[0].Nodes[1].Checked)
      {
        if (this.userID == "")
          accessTreeFromUi.Add((object) AclFeature.Cnt_Synchronization, (object) AclTriState.True);
        else if (this.tvContact.Nodes[0].Nodes[1].ImageIndex == this.broken)
          accessTreeFromUi.Add((object) AclFeature.Cnt_Synchronization, (object) AclTriState.True);
        else
          accessTreeFromUi.Add((object) AclFeature.Cnt_Synchronization, (object) AclTriState.Unspecified);
      }
      else if (this.userID == "")
        accessTreeFromUi.Add((object) AclFeature.Cnt_Synchronization, (object) AclTriState.False);
      else if (this.tvContact.Nodes[0].Nodes[1].ImageIndex == this.broken)
        accessTreeFromUi.Add((object) AclFeature.Cnt_Synchronization, (object) AclTriState.False);
      else
        accessTreeFromUi.Add((object) AclFeature.Cnt_Synchronization, (object) AclTriState.Unspecified);
      if (this.tvContact.Nodes[0].Nodes[2].Checked)
      {
        if (this.userID == "")
          accessTreeFromUi.Add((object) AclFeature.Cnt_Campaign_Access, (object) AclTriState.True);
        else if (this.tvContact.Nodes[0].Nodes[2].ImageIndex == this.broken)
          accessTreeFromUi.Add((object) AclFeature.Cnt_Campaign_Access, (object) AclTriState.True);
        else
          accessTreeFromUi.Add((object) AclFeature.Cnt_Campaign_Access, (object) AclTriState.Unspecified);
        if (this.tvContact.Nodes[0].Nodes[2].Nodes[0].Checked)
        {
          if (this.userID == "")
            accessTreeFromUi.Add((object) AclFeature.Cnt_Campaign_AssignTaskToOther, (object) AclTriState.True);
          else if (this.tvContact.Nodes[0].Nodes[2].Nodes[0].ImageIndex == this.broken)
            accessTreeFromUi.Add((object) AclFeature.Cnt_Campaign_AssignTaskToOther, (object) AclTriState.True);
          else
            accessTreeFromUi.Add((object) AclFeature.Cnt_Campaign_AssignTaskToOther, (object) AclTriState.Unspecified);
        }
        else if (this.userID == "")
          accessTreeFromUi.Add((object) AclFeature.Cnt_Campaign_AssignTaskToOther, (object) AclTriState.False);
        else if (this.tvContact.Nodes[0].Nodes[2].Nodes[0].ImageIndex == this.broken)
          accessTreeFromUi.Add((object) AclFeature.Cnt_Campaign_AssignTaskToOther, (object) AclTriState.False);
        else
          accessTreeFromUi.Add((object) AclFeature.Cnt_Campaign_AssignTaskToOther, (object) AclTriState.Unspecified);
      }
      else if (this.userID == "")
      {
        accessTreeFromUi.Add((object) AclFeature.Cnt_Campaign_Access, (object) AclTriState.False);
        accessTreeFromUi.Add((object) AclFeature.Cnt_Campaign_AssignTaskToOther, (object) AclTriState.False);
      }
      else if (this.tvContact.Nodes[0].Nodes[2].ImageIndex == this.broken)
      {
        accessTreeFromUi.Add((object) AclFeature.Cnt_Campaign_Access, (object) AclTriState.False);
        accessTreeFromUi.Add((object) AclFeature.Cnt_Campaign_AssignTaskToOther, (object) AclTriState.False);
      }
      else
      {
        accessTreeFromUi.Add((object) AclFeature.Cnt_Campaign_Access, (object) AclTriState.Unspecified);
        accessTreeFromUi.Add((object) AclFeature.Cnt_Campaign_AssignTaskToOther, (object) AclTriState.Unspecified);
      }
      if (this.tvContact.Nodes[0].Nodes[3].Checked)
      {
        if (this.userID == "")
        {
          accessTreeFromUi.Add((object) AclFeature.Borrower_Contacts_Personal_CustomLetters, (object) AclTriState.True);
          accessTreeFromUi.Add((object) AclFeature.Business_Contacts_Personal_CustomLetters, (object) AclTriState.True);
          accessTreeFromUi.Add((object) AclFeature.Cnt_Campaign_PersonalTemplates, (object) AclTriState.True);
        }
        else if (this.tvContact.Nodes[0].Nodes[3].ImageIndex == this.broken)
        {
          accessTreeFromUi.Add((object) AclFeature.Borrower_Contacts_Personal_CustomLetters, (object) AclTriState.True);
          accessTreeFromUi.Add((object) AclFeature.Business_Contacts_Personal_CustomLetters, (object) AclTriState.True);
          accessTreeFromUi.Add((object) AclFeature.Cnt_Campaign_PersonalTemplates, (object) AclTriState.True);
        }
        else
        {
          accessTreeFromUi.Add((object) AclFeature.Borrower_Contacts_Personal_CustomLetters, (object) AclTriState.Unspecified);
          accessTreeFromUi.Add((object) AclFeature.Business_Contacts_Personal_CustomLetters, (object) AclTriState.Unspecified);
          accessTreeFromUi.Add((object) AclFeature.Cnt_Campaign_PersonalTemplates, (object) AclTriState.Unspecified);
        }
      }
      else if (this.userID == "")
      {
        accessTreeFromUi.Add((object) AclFeature.Borrower_Contacts_Personal_CustomLetters, (object) AclTriState.False);
        accessTreeFromUi.Add((object) AclFeature.Business_Contacts_Personal_CustomLetters, (object) AclTriState.False);
        accessTreeFromUi.Add((object) AclFeature.Cnt_Campaign_PersonalTemplates, (object) AclTriState.False);
      }
      else if (this.tvContact.Nodes[0].Nodes[3].ImageIndex == this.broken)
      {
        accessTreeFromUi.Add((object) AclFeature.Borrower_Contacts_Personal_CustomLetters, (object) AclTriState.False);
        accessTreeFromUi.Add((object) AclFeature.Business_Contacts_Personal_CustomLetters, (object) AclTriState.False);
        accessTreeFromUi.Add((object) AclFeature.Cnt_Campaign_PersonalTemplates, (object) AclTriState.False);
      }
      else
      {
        accessTreeFromUi.Add((object) AclFeature.Borrower_Contacts_Personal_CustomLetters, (object) AclTriState.Unspecified);
        accessTreeFromUi.Add((object) AclFeature.Business_Contacts_Personal_CustomLetters, (object) AclTriState.Unspecified);
        accessTreeFromUi.Add((object) AclFeature.Cnt_Campaign_PersonalTemplates, (object) AclTriState.Unspecified);
      }
      if (this.tvDashabordReport.Nodes[0].Checked)
      {
        if (this.userID == "")
        {
          accessTreeFromUi.Add((object) AclFeature.ReportTab_LoanReport, (object) AclTriState.True);
          accessTreeFromUi.Add((object) AclFeature.ReportTab_BorrowerContactReport, (object) AclTriState.True);
          accessTreeFromUi.Add((object) AclFeature.ReportTab_BusinessContactReport, (object) AclTriState.True);
        }
        else if (this.tvDashabordReport.Nodes[0].ImageIndex == this.broken)
        {
          accessTreeFromUi.Add((object) AclFeature.ReportTab_LoanReport, (object) AclTriState.True);
          accessTreeFromUi.Add((object) AclFeature.ReportTab_BorrowerContactReport, (object) AclTriState.True);
          accessTreeFromUi.Add((object) AclFeature.ReportTab_BusinessContactReport, (object) AclTriState.True);
        }
        else
        {
          accessTreeFromUi.Add((object) AclFeature.ReportTab_LoanReport, (object) AclTriState.Unspecified);
          accessTreeFromUi.Add((object) AclFeature.ReportTab_BorrowerContactReport, (object) AclTriState.Unspecified);
          accessTreeFromUi.Add((object) AclFeature.ReportTab_BusinessContactReport, (object) AclTriState.Unspecified);
        }
        if (this.tvDashabordReport.Nodes[0].Nodes[0].Checked)
        {
          if (this.userID == "")
            accessTreeFromUi.Add((object) AclFeature.ReportTab_ReportingDB, (object) AclTriState.True);
          else if (this.tvDashabordReport.Nodes[0].Nodes[0].ImageIndex == this.broken)
            accessTreeFromUi.Add((object) AclFeature.ReportTab_ReportingDB, (object) AclTriState.True);
          else
            accessTreeFromUi.Add((object) AclFeature.ReportTab_ReportingDB, (object) AclTriState.Unspecified);
        }
        else if (this.userID == "")
          accessTreeFromUi.Add((object) AclFeature.ReportTab_ReportingDB, (object) AclTriState.False);
        else if (this.tvDashabordReport.Nodes[0].Nodes[0].ImageIndex == this.broken)
          accessTreeFromUi.Add((object) AclFeature.ReportTab_ReportingDB, (object) AclTriState.False);
        else
          accessTreeFromUi.Add((object) AclFeature.ReportTab_ReportingDB, (object) AclTriState.Unspecified);
        if (this.tvDashabordReport.Nodes[0].Nodes[1].Checked)
        {
          if (this.userID == "")
            accessTreeFromUi.Add((object) AclFeature.ReportTab_PersonalTemplate, (object) AclTriState.True);
          else if (this.tvDashabordReport.Nodes[0].Nodes[1].ImageIndex == this.broken)
            accessTreeFromUi.Add((object) AclFeature.ReportTab_PersonalTemplate, (object) AclTriState.True);
          else
            accessTreeFromUi.Add((object) AclFeature.ReportTab_PersonalTemplate, (object) AclTriState.Unspecified);
        }
        else if (this.userID == "")
          accessTreeFromUi.Add((object) AclFeature.ReportTab_PersonalTemplate, (object) AclTriState.False);
        else if (this.tvDashabordReport.Nodes[0].Nodes[1].ImageIndex == this.broken)
          accessTreeFromUi.Add((object) AclFeature.ReportTab_PersonalTemplate, (object) AclTriState.False);
        else
          accessTreeFromUi.Add((object) AclFeature.ReportTab_PersonalTemplate, (object) AclTriState.Unspecified);
      }
      else if (this.userID == "")
      {
        accessTreeFromUi.Add((object) AclFeature.ReportTab_LoanReport, (object) AclTriState.False);
        accessTreeFromUi.Add((object) AclFeature.ReportTab_BorrowerContactReport, (object) AclTriState.False);
        accessTreeFromUi.Add((object) AclFeature.ReportTab_BusinessContactReport, (object) AclTriState.False);
        accessTreeFromUi.Add((object) AclFeature.ReportTab_ReportingDB, (object) AclTriState.False);
        accessTreeFromUi.Add((object) AclFeature.ReportTab_PersonalTemplate, (object) AclTriState.False);
      }
      else if (this.tvDashabordReport.Nodes[0].ImageIndex == this.broken)
      {
        accessTreeFromUi.Add((object) AclFeature.ReportTab_LoanReport, (object) AclTriState.False);
        accessTreeFromUi.Add((object) AclFeature.ReportTab_BorrowerContactReport, (object) AclTriState.False);
        accessTreeFromUi.Add((object) AclFeature.ReportTab_BusinessContactReport, (object) AclTriState.False);
        accessTreeFromUi.Add((object) AclFeature.ReportTab_ReportingDB, (object) AclTriState.False);
        accessTreeFromUi.Add((object) AclFeature.ReportTab_PersonalTemplate, (object) AclTriState.False);
      }
      else
      {
        accessTreeFromUi.Add((object) AclFeature.ReportTab_LoanReport, (object) AclTriState.Unspecified);
        accessTreeFromUi.Add((object) AclFeature.ReportTab_BorrowerContactReport, (object) AclTriState.Unspecified);
        accessTreeFromUi.Add((object) AclFeature.ReportTab_BusinessContactReport, (object) AclTriState.Unspecified);
        accessTreeFromUi.Add((object) AclFeature.ReportTab_ReportingDB, (object) AclTriState.Unspecified);
        accessTreeFromUi.Add((object) AclFeature.ReportTab_PersonalTemplate, (object) AclTriState.Unspecified);
      }
      if (this.tvDashabordReport.Nodes[1].Checked)
      {
        if (this.userID == "")
          accessTreeFromUi.Add((object) AclFeature.DashboardTab_Dashboard, (object) AclTriState.True);
        else if (this.tvDashabordReport.Nodes[1].ImageIndex == this.broken)
          accessTreeFromUi.Add((object) AclFeature.DashboardTab_Dashboard, (object) AclTriState.True);
        else
          accessTreeFromUi.Add((object) AclFeature.DashboardTab_Dashboard, (object) AclTriState.Unspecified);
        if (this.tvDashabordReport.Nodes[1].Nodes[0].Checked)
        {
          if (this.userID == "")
          {
            accessTreeFromUi.Add((object) AclFeature.DashboardTab_ManagePersonalTemplate, (object) AclTriState.True);
            accessTreeFromUi.Add((object) AclFeature.DashboardTab_ManagePersonalViewTemplate, (object) AclTriState.True);
          }
          else if (this.tvDashabordReport.Nodes[1].Nodes[0].ImageIndex == this.broken)
          {
            accessTreeFromUi.Add((object) AclFeature.DashboardTab_ManagePersonalTemplate, (object) AclTriState.True);
            accessTreeFromUi.Add((object) AclFeature.DashboardTab_ManagePersonalViewTemplate, (object) AclTriState.True);
          }
          else
          {
            accessTreeFromUi.Add((object) AclFeature.DashboardTab_ManagePersonalTemplate, (object) AclTriState.Unspecified);
            accessTreeFromUi.Add((object) AclFeature.DashboardTab_ManagePersonalViewTemplate, (object) AclTriState.Unspecified);
          }
        }
        else if (this.userID == "")
        {
          accessTreeFromUi.Add((object) AclFeature.DashboardTab_ManagePersonalTemplate, (object) AclTriState.False);
          accessTreeFromUi.Add((object) AclFeature.DashboardTab_ManagePersonalViewTemplate, (object) AclTriState.False);
        }
        else if (this.tvDashabordReport.Nodes[1].Nodes[0].ImageIndex == this.broken)
        {
          accessTreeFromUi.Add((object) AclFeature.DashboardTab_ManagePersonalTemplate, (object) AclTriState.False);
          accessTreeFromUi.Add((object) AclFeature.DashboardTab_ManagePersonalViewTemplate, (object) AclTriState.False);
        }
        else
        {
          accessTreeFromUi.Add((object) AclFeature.DashboardTab_ManagePersonalTemplate, (object) AclTriState.Unspecified);
          accessTreeFromUi.Add((object) AclFeature.DashboardTab_ManagePersonalViewTemplate, (object) AclTriState.Unspecified);
        }
      }
      else if (this.userID == "")
      {
        accessTreeFromUi.Add((object) AclFeature.DashboardTab_Dashboard, (object) AclTriState.False);
        accessTreeFromUi.Add((object) AclFeature.DashboardTab_ManagePersonalTemplate, (object) AclTriState.False);
        accessTreeFromUi.Add((object) AclFeature.DashboardTab_ManagePersonalViewTemplate, (object) AclTriState.False);
      }
      else if (this.tvDashabordReport.Nodes[1].ImageIndex == this.broken)
      {
        accessTreeFromUi.Add((object) AclFeature.DashboardTab_Dashboard, (object) AclTriState.False);
        accessTreeFromUi.Add((object) AclFeature.DashboardTab_ManagePersonalTemplate, (object) AclTriState.False);
        accessTreeFromUi.Add((object) AclFeature.DashboardTab_ManagePersonalViewTemplate, (object) AclTriState.False);
      }
      else
      {
        accessTreeFromUi.Add((object) AclFeature.DashboardTab_Dashboard, (object) AclTriState.Unspecified);
        accessTreeFromUi.Add((object) AclFeature.DashboardTab_ManagePersonalTemplate, (object) AclTriState.Unspecified);
        accessTreeFromUi.Add((object) AclFeature.DashboardTab_ManagePersonalViewTemplate, (object) AclTriState.Unspecified);
      }
      if (this.tvSettings.Nodes[0].Checked)
      {
        if (this.userID == "")
        {
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_CustomPrintForms, (object) AclTriState.True);
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_PrintGroups, (object) AclTriState.True);
        }
        else if (this.tvSettings.Nodes[0].ImageIndex == this.broken)
        {
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_CustomPrintForms, (object) AclTriState.True);
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_PrintGroups, (object) AclTriState.True);
        }
        else
        {
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_CustomPrintForms, (object) AclTriState.Unspecified);
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_PrintGroups, (object) AclTriState.Unspecified);
        }
      }
      else if (this.userID == "")
      {
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_CustomPrintForms, (object) AclTriState.False);
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_PrintGroups, (object) AclTriState.False);
      }
      else if (this.tvSettings.Nodes[0].ImageIndex == this.broken)
      {
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_CustomPrintForms, (object) AclTriState.False);
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_PrintGroups, (object) AclTriState.False);
      }
      else
      {
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_CustomPrintForms, (object) AclTriState.Unspecified);
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_PrintGroups, (object) AclTriState.Unspecified);
      }
      if (this.tvSettings.Nodes[1].Checked)
      {
        if (this.userID == "")
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Company_LoanCustomFields, (object) AclTriState.True);
        else if (this.tvSettings.Nodes[1].ImageIndex == this.broken)
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Company_LoanCustomFields, (object) AclTriState.True);
        else
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Company_LoanCustomFields, (object) AclTriState.Unspecified);
      }
      else if (this.userID == "")
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Company_LoanCustomFields, (object) AclTriState.False);
      else if (this.tvSettings.Nodes[1].ImageIndex == this.broken)
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Company_LoanCustomFields, (object) AclTriState.False);
      else
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Company_LoanCustomFields, (object) AclTriState.Unspecified);
      if (this.tvSettings.Nodes[2].Checked)
      {
        if (this.userID == "")
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Company_DocumentSetup, (object) AclTriState.True);
        else if (this.tvSettings.Nodes[2].ImageIndex == this.broken)
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Company_DocumentSetup, (object) AclTriState.True);
        else
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Company_DocumentSetup, (object) AclTriState.Unspecified);
      }
      else if (this.userID == "")
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Company_DocumentSetup, (object) AclTriState.False);
      else if (this.tvSettings.Nodes[2].ImageIndex == this.broken)
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Company_DocumentSetup, (object) AclTriState.False);
      else
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Company_DocumentSetup, (object) AclTriState.Unspecified);
      if (this.tvSettings.Nodes[3].Checked)
      {
        if (this.userID == "")
        {
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_LoanPrograms, (object) AclTriState.True);
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_ClosingCosts, (object) AclTriState.True);
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_DocumentSets, (object) AclTriState.True);
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_InputFormSets, (object) AclTriState.True);
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_SettlementServiceProvider, (object) AclTriState.True);
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_Affiliate, (object) AclTriState.True);
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_MiscDataTemplates, (object) AclTriState.True);
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_LoanTemplateSets, (object) AclTriState.True);
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_TaskSets, (object) AclTriState.True);
        }
        else if (this.tvSettings.Nodes[3].ImageIndex == this.broken)
        {
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_LoanPrograms, (object) AclTriState.True);
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_ClosingCosts, (object) AclTriState.True);
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_DocumentSets, (object) AclTriState.True);
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_InputFormSets, (object) AclTriState.True);
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_SettlementServiceProvider, (object) AclTriState.True);
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_Affiliate, (object) AclTriState.True);
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_MiscDataTemplates, (object) AclTriState.True);
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_LoanTemplateSets, (object) AclTriState.True);
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_TaskSets, (object) AclTriState.True);
        }
        else
        {
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_LoanPrograms, (object) AclTriState.Unspecified);
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_ClosingCosts, (object) AclTriState.Unspecified);
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_DocumentSets, (object) AclTriState.Unspecified);
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_InputFormSets, (object) AclTriState.Unspecified);
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_SettlementServiceProvider, (object) AclTriState.Unspecified);
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_Affiliate, (object) AclTriState.Unspecified);
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_MiscDataTemplates, (object) AclTriState.Unspecified);
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_LoanTemplateSets, (object) AclTriState.Unspecified);
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_TaskSets, (object) AclTriState.Unspecified);
        }
      }
      else if (this.userID == "")
      {
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_LoanPrograms, (object) AclTriState.False);
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_ClosingCosts, (object) AclTriState.False);
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_DocumentSets, (object) AclTriState.False);
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_InputFormSets, (object) AclTriState.False);
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_SettlementServiceProvider, (object) AclTriState.False);
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_Affiliate, (object) AclTriState.False);
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_MiscDataTemplates, (object) AclTriState.False);
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_LoanTemplateSets, (object) AclTriState.False);
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_TaskSets, (object) AclTriState.False);
      }
      else if (this.tvSettings.Nodes[3].ImageIndex == this.broken)
      {
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_LoanPrograms, (object) AclTriState.False);
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_ClosingCosts, (object) AclTriState.False);
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_DocumentSets, (object) AclTriState.False);
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_InputFormSets, (object) AclTriState.False);
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_SettlementServiceProvider, (object) AclTriState.False);
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_Affiliate, (object) AclTriState.False);
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_MiscDataTemplates, (object) AclTriState.False);
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_LoanTemplateSets, (object) AclTriState.False);
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_TaskSets, (object) AclTriState.False);
      }
      else
      {
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_LoanPrograms, (object) AclTriState.Unspecified);
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_ClosingCosts, (object) AclTriState.Unspecified);
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_DocumentSets, (object) AclTriState.Unspecified);
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_InputFormSets, (object) AclTriState.Unspecified);
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_SettlementServiceProvider, (object) AclTriState.Unspecified);
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_Affiliate, (object) AclTriState.Unspecified);
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_MiscDataTemplates, (object) AclTriState.Unspecified);
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_LoanTemplateSets, (object) AclTriState.Unspecified);
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Personal_TaskSets, (object) AclTriState.Unspecified);
      }
      if (this.tvSettings.Nodes[4].Checked)
      {
        if (this.userID == "")
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Company_TableFee, (object) AclTriState.True);
        else if (this.tvSettings.Nodes[4].ImageIndex == this.broken)
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Company_TableFee, (object) AclTriState.True);
        else
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Company_TableFee, (object) AclTriState.Unspecified);
        if (this.tvSettings.Nodes[4].Nodes[0].Checked)
        {
          if (this.userID == "")
            accessTreeFromUi.Add((object) AclFeature.SettingsTab_Company_TF_2010ItemizationFeeMgmt, (object) AclTriState.True);
          else if (this.tvSettings.Nodes[4].Nodes[0].ImageIndex == this.broken)
            accessTreeFromUi.Add((object) AclFeature.SettingsTab_Company_TF_2010ItemizationFeeMgmt, (object) AclTriState.True);
          else
            accessTreeFromUi.Add((object) AclFeature.SettingsTab_Company_TF_2010ItemizationFeeMgmt, (object) AclTriState.Unspecified);
        }
        else if (this.userID == "")
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Company_TF_2010ItemizationFeeMgmt, (object) AclTriState.False);
        else if (this.tvSettings.Nodes[4].Nodes[0].ImageIndex == this.broken)
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Company_TF_2010ItemizationFeeMgmt, (object) AclTriState.False);
        else
          accessTreeFromUi.Add((object) AclFeature.SettingsTab_Company_TF_2010ItemizationFeeMgmt, (object) AclTriState.Unspecified);
      }
      else if (this.userID == "")
      {
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Company_TableFee, (object) AclTriState.False);
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Company_TF_2010ItemizationFeeMgmt, (object) AclTriState.False);
      }
      else if (this.tvSettings.Nodes[4].ImageIndex == this.broken)
      {
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Company_TableFee, (object) AclTriState.False);
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Company_TF_2010ItemizationFeeMgmt, (object) AclTriState.False);
      }
      else
      {
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Company_TableFee, (object) AclTriState.Unspecified);
        accessTreeFromUi.Add((object) AclFeature.SettingsTab_Company_TF_2010ItemizationFeeMgmt, (object) AclTriState.Unspecified);
      }
      if (this.tvDeveloperConnect.Nodes[0].Checked)
      {
        if (this.userID == "")
          accessTreeFromUi.Add((object) AclFeature.DeveloperConnectTab_SubscribetoWebhooks, (object) AclTriState.True);
        else if (this.tvDeveloperConnect.Nodes[0].ImageIndex == this.broken)
          accessTreeFromUi.Add((object) AclFeature.DeveloperConnectTab_SubscribetoWebhooks, (object) AclTriState.True);
        else
          accessTreeFromUi.Add((object) AclFeature.DeveloperConnectTab_SubscribetoWebhooks, (object) AclTriState.Unspecified);
      }
      else if (this.userID == "")
        accessTreeFromUi.Add((object) AclFeature.DeveloperConnectTab_SubscribetoWebhooks, (object) AclTriState.False);
      else if (this.tvDeveloperConnect.Nodes[0].ImageIndex == this.broken)
        accessTreeFromUi.Add((object) AclFeature.DeveloperConnectTab_SubscribetoWebhooks, (object) AclTriState.False);
      else
        accessTreeFromUi.Add((object) AclFeature.DeveloperConnectTab_SubscribetoWebhooks, (object) AclTriState.Unspecified);
      if (this.tvDeveloperConnect.Nodes[0].Nodes[0].Checked)
      {
        if (this.userID == "")
          accessTreeFromUi.Add((object) AclFeature.DeveloperConnectTab_SubscribetoWebhooks_EnhancedFieldChange, (object) AclTriState.True);
        else if (this.tvDeveloperConnect.Nodes[0].Nodes[0].ImageIndex == this.broken)
          accessTreeFromUi.Add((object) AclFeature.DeveloperConnectTab_SubscribetoWebhooks_EnhancedFieldChange, (object) AclTriState.True);
        else
          accessTreeFromUi.Add((object) AclFeature.DeveloperConnectTab_SubscribetoWebhooks_EnhancedFieldChange, (object) AclTriState.Unspecified);
      }
      else if (this.userID == "")
        accessTreeFromUi.Add((object) AclFeature.DeveloperConnectTab_SubscribetoWebhooks_EnhancedFieldChange, (object) AclTriState.False);
      else if (this.tvDeveloperConnect.Nodes[0].Nodes[0].ImageIndex == this.broken)
        accessTreeFromUi.Add((object) AclFeature.DeveloperConnectTab_SubscribetoWebhooks_EnhancedFieldChange, (object) AclTriState.False);
      else
        accessTreeFromUi.Add((object) AclFeature.DeveloperConnectTab_SubscribetoWebhooks_EnhancedFieldChange, (object) AclTriState.Unspecified);
      accessTreeFromUi.Add((object) AclFeature.PlatForm_Access, (object) this.selectedPlatformAccess);
      return accessTreeFromUi;
    }

    public void SetPersona(int personaId)
    {
      if (this.personaID == personaId && !this.IsDirty)
        return;
      this.personaID = personaId;
      this.setStatus(false);
      this.loadPipelineData(false);
      this.loadPersonaSetting();
    }

    private void setStatus(bool dirty)
    {
      this.dirty = dirty;
      if (this.DirtyFlagChanged == null)
        return;
      this.DirtyFlagChanged((object) this, (EventArgs) null);
    }

    public void MakeReadOnly(bool makeReadOnly) => this.disableAccess(makeReadOnly);

    private void disableAccess(bool makeReadOnly)
    {
      this.forcedReadOnly = makeReadOnly;
      this.siBtnDeleteView.Enabled = !makeReadOnly;
      this.siBtnDeleteView.Visible = this.userID == "";
      this.siBtnEditPipelineView.Enabled = !makeReadOnly;
      this.siBtnEditPipelineView.Visible = this.userID == "";
      this.siBtnAddPipelineView.Enabled = !makeReadOnly;
      this.siBtnAddPipelineView.Visible = this.userID == "";
      this.siBtnMoveViewDown.Enabled = !makeReadOnly;
      this.siBtnMoveViewDown.Visible = this.userID == "";
      this.siBtnMoveViewUp.Enabled = !makeReadOnly;
      this.siBtnMoveViewUp.Visible = this.userID == "";
      this.gvPipelineViews.Enabled = !makeReadOnly;
      this.linkWithPersonaRightsToolStripMenuItem.Enabled = this.disconnectFromPersonaRightsToolStripMenuItem.Enabled = !makeReadOnly;
      if (this.impDlg == null)
        return;
      this.impDlg.IsReadOnly = makeReadOnly;
    }

    public void SaveData()
    {
      if (!this.IsDirty)
        return;
      Hashtable accessTreeFromUi = this.getAccessTreeFromUI();
      if (this.userID == "")
      {
        this.pipelineViewMgr.ReplacePersonaPipelineViews(this.personaID, this.getPipelineViews());
        if (accessTreeFromUi.ContainsKey((object) AclFeature.LoanTab_Other_ePASS))
        {
          this.serviceMgr.SetDefaultValue(AclFeature.LoanTab_Other_ePASS, this.personaID, (AclTriState) accessTreeFromUi[(object) AclFeature.LoanTab_Other_ePASS] == AclTriState.True ? ServiceAclInfo.ServicesDefaultSetting.All : ServiceAclInfo.ServicesDefaultSetting.None);
          accessTreeFromUi.Remove((object) AclFeature.LoanTab_Other_ePASS);
        }
        this.aclMgr.SetPermissions(accessTreeFromUi, this.personaID);
      }
      else
      {
        if (accessTreeFromUi.ContainsKey((object) AclFeature.LoanTab_Other_ePASS))
        {
          this.serviceMgr.SetDefaultValue(AclFeature.LoanTab_Other_ePASS, this.userID, (AclTriState) accessTreeFromUi[(object) AclFeature.LoanTab_Other_ePASS] == AclTriState.True ? ServiceAclInfo.ServicesDefaultSetting.All : ((AclTriState) accessTreeFromUi[(object) AclFeature.LoanTab_Other_ePASS] == AclTriState.Unspecified ? ServiceAclInfo.ServicesDefaultSetting.NotSpecified : ServiceAclInfo.ServicesDefaultSetting.None));
          accessTreeFromUi.Remove((object) AclFeature.LoanTab_Other_ePASS);
        }
        this.aclMgr.SetPermissions(accessTreeFromUi, this.userID);
      }
      if (this.impDlg != null)
        this.impDlg.SaveData();
      this.setStatus(false);
    }

    public void ResetData()
    {
      this.loadPipelineData(false);
      this.impDlg = (ImportLoanDlg) null;
      this.loadSettings();
      this.setStatus(false);
    }

    public bool IsDirty => this.dirty;

    public bool IsValid => true;

    private void registerTreeNodeCheckEvent(bool register)
    {
      if (register)
      {
        this.tvContact.AfterCheck += new TreeViewEventHandler(this.tv_AfterCheck);
        this.tvDashabordReport.AfterCheck += new TreeViewEventHandler(this.tv_AfterCheck);
        this.tvLoan.AfterCheck += new TreeViewEventHandler(this.tv_AfterCheck);
        this.tvSettings.AfterCheck += new TreeViewEventHandler(this.tv_AfterCheck);
        this.tvPipelineTasks.AfterCheck += new TreeViewEventHandler(this.tv_AfterCheck);
        this.tvDeveloperConnect.AfterCheck += new TreeViewEventHandler(this.tv_AfterCheck);
      }
      else
      {
        this.tvContact.AfterCheck -= new TreeViewEventHandler(this.tv_AfterCheck);
        this.tvDashabordReport.AfterCheck -= new TreeViewEventHandler(this.tv_AfterCheck);
        this.tvLoan.AfterCheck -= new TreeViewEventHandler(this.tv_AfterCheck);
        this.tvSettings.AfterCheck -= new TreeViewEventHandler(this.tv_AfterCheck);
        this.tvPipelineTasks.AfterCheck -= new TreeViewEventHandler(this.tv_AfterCheck);
        this.tvDeveloperConnect.AfterCheck -= new TreeViewEventHandler(this.tv_AfterCheck);
      }
    }

    private void tv_BeforeCollapse(object sender, TreeViewCancelEventArgs e) => e.Cancel = true;

    private void tv_AfterCheck(object sender, TreeViewEventArgs e)
    {
      e.Node.ImageIndex = e.Node.SelectedImageIndex = e.Node.StateImageIndex = this.broken;
      if (!this.suspendEvent)
        this.setStatus(true);
      this.registerTreeNodeCheckEvent(false);
      if (e.Node.Name == "loan_ILNodes")
      {
        if (!this.suspendEvent)
        {
          this.selectOption = !e.Node.Checked ? 0 : 1;
          this.tvLoan_NodeMouseClick(sender, new TreeNodeMouseClickEventArgs(e.Node, MouseButtons.Left, 1, 0, 0));
        }
      }
      else if (e.Node.Name == "nodeAccessContact")
      {
        if (e.Node.Checked)
        {
          if (!this.tvContact.Nodes[0].Nodes[0].Checked)
          {
            this.tvContact.Nodes[0].Nodes[0].Checked = true;
            this.tvContact.Nodes[0].Nodes[0].ImageIndex = this.tvContact.Nodes[0].Nodes[0].StateImageIndex = this.tvContact.Nodes[0].Nodes[0].SelectedImageIndex = this.broken;
          }
          if (!this.tvContact.Nodes[0].Nodes[1].Checked)
          {
            this.tvContact.Nodes[0].Nodes[1].Checked = true;
            this.tvContact.Nodes[0].Nodes[1].ImageIndex = this.tvContact.Nodes[0].Nodes[1].StateImageIndex = this.tvContact.Nodes[0].Nodes[1].SelectedImageIndex = this.broken;
          }
          if (!this.tvContact.Nodes[0].Nodes[2].Checked)
          {
            this.tvContact.Nodes[0].Nodes[2].Checked = true;
            this.tvContact.Nodes[0].Nodes[2].ImageIndex = this.tvContact.Nodes[0].Nodes[2].StateImageIndex = this.tvContact.Nodes[0].Nodes[2].SelectedImageIndex = this.broken;
            if (!this.tvContact.Nodes[0].Nodes[2].Nodes[0].Checked)
            {
              this.tvContact.Nodes[0].Nodes[2].Nodes[0].Checked = true;
              this.tvContact.Nodes[0].Nodes[2].Nodes[0].ImageIndex = this.tvContact.Nodes[0].Nodes[2].Nodes[0].StateImageIndex = this.tvContact.Nodes[0].Nodes[2].Nodes[0].SelectedImageIndex = this.broken;
            }
          }
          if (!this.tvContact.Nodes[0].Nodes[3].Checked)
          {
            this.tvContact.Nodes[0].Nodes[3].Checked = true;
            this.tvContact.Nodes[0].Nodes[3].ImageIndex = this.tvContact.Nodes[0].Nodes[3].StateImageIndex = this.tvContact.Nodes[0].Nodes[3].SelectedImageIndex = this.broken;
          }
        }
        else
        {
          if (this.tvContact.Nodes[0].Nodes[0].Checked)
          {
            this.tvContact.Nodes[0].Nodes[0].Checked = false;
            this.tvContact.Nodes[0].Nodes[0].ImageIndex = this.tvContact.Nodes[0].Nodes[0].StateImageIndex = this.tvContact.Nodes[0].Nodes[0].SelectedImageIndex = this.broken;
          }
          if (this.tvContact.Nodes[0].Nodes[1].Checked)
          {
            this.tvContact.Nodes[0].Nodes[1].Checked = false;
            this.tvContact.Nodes[0].Nodes[1].ImageIndex = this.tvContact.Nodes[0].Nodes[1].StateImageIndex = this.tvContact.Nodes[0].Nodes[1].SelectedImageIndex = this.broken;
          }
          if (this.tvContact.Nodes[0].Nodes[2].Checked)
          {
            this.tvContact.Nodes[0].Nodes[2].Checked = false;
            this.tvContact.Nodes[0].Nodes[2].ImageIndex = this.tvContact.Nodes[0].Nodes[2].StateImageIndex = this.tvContact.Nodes[0].Nodes[2].SelectedImageIndex = this.broken;
            if (this.tvContact.Nodes[0].Nodes[2].Nodes[0].Checked)
            {
              this.tvContact.Nodes[0].Nodes[2].Nodes[0].Checked = false;
              this.tvContact.Nodes[0].Nodes[2].Nodes[0].ImageIndex = this.tvContact.Nodes[0].Nodes[2].Nodes[0].StateImageIndex = this.tvContact.Nodes[0].Nodes[2].Nodes[0].SelectedImageIndex = this.broken;
            }
          }
          if (this.tvContact.Nodes[0].Nodes[3].Checked)
          {
            this.tvContact.Nodes[0].Nodes[3].Checked = false;
            this.tvContact.Nodes[0].Nodes[3].ImageIndex = this.tvContact.Nodes[0].Nodes[3].StateImageIndex = this.tvContact.Nodes[0].Nodes[3].SelectedImageIndex = this.broken;
          }
        }
      }
      else if (e.Node.Name == "contact_ACM_ATONode")
      {
        if (e.Node.Checked)
        {
          if (!this.tvContact.Nodes[0].Nodes[2].Checked)
          {
            this.tvContact.Nodes[0].Nodes[2].Checked = true;
            this.tvContact.Nodes[0].Nodes[2].ImageIndex = this.tvContact.Nodes[0].Nodes[2].StateImageIndex = this.tvContact.Nodes[0].Nodes[2].SelectedImageIndex = this.broken;
          }
          if (!this.tvContact.Nodes[0].Checked)
          {
            this.tvContact.Nodes[0].Checked = true;
            this.tvContact.Nodes[0].ImageIndex = this.tvContact.Nodes[0].StateImageIndex = this.tvContact.Nodes[0].SelectedImageIndex = this.broken;
          }
        }
      }
      else if (e.Node.Name == "contact_ACMNode")
      {
        if (e.Node.Checked)
        {
          if (!this.tvContact.Nodes[0].Nodes[2].Nodes[0].Checked)
          {
            this.tvContact.Nodes[0].Nodes[2].Nodes[0].Checked = true;
            this.tvContact.Nodes[0].Nodes[2].Nodes[0].ImageIndex = this.tvContact.Nodes[0].Nodes[2].Nodes[0].StateImageIndex = this.tvContact.Nodes[0].Nodes[2].Nodes[0].SelectedImageIndex = this.broken;
          }
          if (!this.tvContact.Nodes[0].Checked)
          {
            this.tvContact.Nodes[0].Checked = true;
            this.tvContact.Nodes[0].ImageIndex = this.tvContact.Nodes[0].StateImageIndex = this.tvContact.Nodes[0].SelectedImageIndex = this.broken;
          }
        }
        else if (this.tvContact.Nodes[0].Nodes[2].Nodes[0].Checked)
        {
          this.tvContact.Nodes[0].Nodes[2].Nodes[0].Checked = false;
          this.tvContact.Nodes[0].Nodes[2].Nodes[0].ImageIndex = this.tvContact.Nodes[0].Nodes[2].Nodes[0].StateImageIndex = this.tvContact.Nodes[0].Nodes[2].Nodes[0].SelectedImageIndex = this.broken;
        }
      }
      else if (e.Node.Name == "contact_ECNode")
      {
        if (e.Node.Checked && !this.tvContact.Nodes[0].Checked)
        {
          this.tvContact.Nodes[0].Checked = true;
          this.tvContact.Nodes[0].ImageIndex = this.tvContact.Nodes[0].StateImageIndex = this.tvContact.Nodes[0].SelectedImageIndex = this.broken;
        }
      }
      else if (e.Node.Name == "contact_SCNode")
      {
        if (e.Node.Checked && !this.tvContact.Nodes[0].Checked)
        {
          this.tvContact.Nodes[0].Checked = true;
          this.tvContact.Nodes[0].ImageIndex = this.tvContact.Nodes[0].StateImageIndex = this.tvContact.Nodes[0].SelectedImageIndex = this.broken;
        }
      }
      else if (e.Node.Name == "contact_MPCLCTNode")
      {
        if (e.Node.Checked && !this.tvContact.Nodes[0].Checked)
        {
          this.tvContact.Nodes[0].Checked = true;
          this.tvContact.Nodes[0].ImageIndex = this.tvContact.Nodes[0].StateImageIndex = this.tvContact.Nodes[0].SelectedImageIndex = this.broken;
        }
      }
      else if (e.Node.Name == "dashReport_ARDBNode")
      {
        if (e.Node.Checked && !this.tvDashabordReport.Nodes[0].Checked)
        {
          this.tvDashabordReport.Nodes[0].Checked = true;
          this.tvDashabordReport.Nodes[0].ImageIndex = this.tvDashabordReport.Nodes[0].StateImageIndex = this.tvDashabordReport.Nodes[0].SelectedImageIndex = this.broken;
        }
      }
      else if (e.Node.Name == "reportTemplate_Node")
      {
        if (e.Node.Checked && !this.tvDashabordReport.Nodes[0].Checked)
        {
          this.tvDashabordReport.Nodes[0].Checked = true;
          this.tvDashabordReport.Nodes[0].ImageIndex = this.tvDashabordReport.Nodes[0].StateImageIndex = this.tvDashabordReport.Nodes[0].SelectedImageIndex = this.broken;
        }
      }
      else if (e.Node.Name == "dashReport_ARNode")
      {
        if (e.Node.Checked)
        {
          if (!this.tvDashabordReport.Nodes[0].Nodes[0].Checked)
          {
            this.tvDashabordReport.Nodes[0].Nodes[0].Checked = true;
            this.tvDashabordReport.Nodes[0].Nodes[0].ImageIndex = this.tvDashabordReport.Nodes[0].Nodes[0].StateImageIndex = this.tvDashabordReport.Nodes[0].Nodes[0].SelectedImageIndex = this.broken;
          }
          if (!this.tvDashabordReport.Nodes[0].Nodes[1].Checked)
          {
            this.tvDashabordReport.Nodes[0].Nodes[1].Checked = true;
            this.tvDashabordReport.Nodes[0].Nodes[1].ImageIndex = this.tvDashabordReport.Nodes[0].Nodes[1].StateImageIndex = this.tvDashabordReport.Nodes[0].Nodes[1].SelectedImageIndex = this.broken;
          }
        }
        else
        {
          if (this.tvDashabordReport.Nodes[0].Nodes[0].Checked)
          {
            this.tvDashabordReport.Nodes[0].Nodes[0].Checked = false;
            this.tvDashabordReport.Nodes[0].Nodes[0].ImageIndex = this.tvDashabordReport.Nodes[0].Nodes[0].StateImageIndex = this.tvDashabordReport.Nodes[0].Nodes[0].SelectedImageIndex = this.broken;
          }
          if (this.tvDashabordReport.Nodes[0].Nodes[1].Checked)
          {
            this.tvDashabordReport.Nodes[0].Nodes[1].Checked = false;
            this.tvDashabordReport.Nodes[0].Nodes[1].ImageIndex = this.tvDashabordReport.Nodes[0].Nodes[1].StateImageIndex = this.tvDashabordReport.Nodes[0].Nodes[1].SelectedImageIndex = this.broken;
          }
        }
      }
      else if (e.Node.Name == "dashboardTemp_Node")
      {
        if (e.Node.Checked && !this.tvDashabordReport.Nodes[1].Checked)
        {
          this.tvDashabordReport.Nodes[1].Checked = true;
          this.tvDashabordReport.Nodes[1].ImageIndex = this.tvDashabordReport.Nodes[1].StateImageIndex = this.tvDashabordReport.Nodes[1].SelectedImageIndex = this.broken;
        }
      }
      else if (e.Node.Name == "dashboard_Node")
      {
        if (e.Node.Checked && !this.tvDashabordReport.Nodes[1].Nodes[0].Checked)
        {
          this.tvDashabordReport.Nodes[1].Nodes[0].Checked = true;
          this.tvDashabordReport.Nodes[1].Nodes[0].ImageIndex = this.tvDashabordReport.Nodes[1].Nodes[0].StateImageIndex = this.tvDashabordReport.Nodes[1].Nodes[0].SelectedImageIndex = this.broken;
        }
        else if (this.tvDashabordReport.Nodes[1].Nodes[0].Checked)
        {
          this.tvDashabordReport.Nodes[1].Nodes[0].Checked = false;
          this.tvDashabordReport.Nodes[1].Nodes[0].ImageIndex = this.tvDashabordReport.Nodes[1].Nodes[0].StateImageIndex = this.tvDashabordReport.Nodes[1].Nodes[0].SelectedImageIndex = this.broken;
        }
      }
      else if (e.Node.Name == "setting_2010ItemFeeMgmt")
      {
        if (e.Node.Checked && !this.tvSettings.Nodes[4].Checked)
        {
          this.tvSettings.Nodes[4].Checked = true;
          this.tvSettings.Nodes[4].ImageIndex = this.tvSettings.Nodes[4].StateImageIndex = this.tvSettings.Nodes[4].SelectedImageIndex = this.broken;
        }
      }
      else if (e.Node.Name == "setting_TableFeeNode")
      {
        if (e.Node.Checked && !this.tvSettings.Nodes[4].Nodes[0].Checked)
        {
          this.tvSettings.Nodes[4].Nodes[0].Checked = true;
          this.tvSettings.Nodes[4].Nodes[0].ImageIndex = this.tvSettings.Nodes[4].Nodes[0].StateImageIndex = this.tvSettings.Nodes[4].Nodes[0].SelectedImageIndex = this.broken;
        }
        else if (this.tvSettings.Nodes[4].Nodes[0].Checked)
        {
          this.tvSettings.Nodes[4].Nodes[0].Checked = false;
          this.tvSettings.Nodes[4].Nodes[0].ImageIndex = this.tvSettings.Nodes[4].Nodes[0].StateImageIndex = this.tvSettings.Nodes[4].Nodes[0].SelectedImageIndex = this.broken;
        }
      }
      else if (e.Node.Name == "loan_eSignPackagesNode")
      {
        if (e.Node.Checked && !this.tvLoan.Nodes[5].Checked)
        {
          this.tvLoan.Nodes[5].Checked = true;
          this.tvLoan.Nodes[5].ImageIndex = this.tvLoan.Nodes[5].StateImageIndex = this.tvLoan.Nodes[5].SelectedImageIndex = this.broken;
        }
      }
      else if (e.Node.Name == "loan_PackagesNode")
      {
        if (e.Node.Checked && !this.tvLoan.Nodes[5].Nodes[0].Checked)
        {
          this.tvLoan.Nodes[5].Nodes[0].Checked = true;
          this.tvLoan.Nodes[5].Nodes[0].ImageIndex = this.tvLoan.Nodes[5].Nodes[0].StateImageIndex = this.tvLoan.Nodes[5].Nodes[0].SelectedImageIndex = this.broken;
        }
        else if (this.tvLoan.Nodes[5].Nodes[0].Checked)
        {
          this.tvLoan.Nodes[5].Nodes[0].Checked = false;
          this.tvLoan.Nodes[5].Nodes[0].ImageIndex = this.tvLoan.Nodes[5].Nodes[0].StateImageIndex = this.tvLoan.Nodes[5].Nodes[0].SelectedImageIndex = this.broken;
        }
      }
      else if (e.Node.Name == "EnhancedFieldChange_Node" && e.Node.Checked && !this.tvDeveloperConnect.Nodes[0].Checked)
      {
        this.tvDeveloperConnect.Nodes[0].Checked = true;
        this.tvDeveloperConnect.Nodes[0].ImageIndex = this.tvDeveloperConnect.Nodes[0].StateImageIndex = this.tvDeveloperConnect.Nodes[0].SelectedImageIndex = this.broken;
      }
      this.registerTreeNodeCheckEvent(true);
    }

    private void loadPipelineData(bool syncFields)
    {
      if (this.userID == "")
      {
        this.accessColList = this.fieldAccessMgr.GetFieldIDsPermission(this.personaID, true);
      }
      else
      {
        this.accessColList_User = this.fieldAccessMgr.GetFieldIDsPermission(this.userID);
        this.accessColList_Personas = this.fieldAccessMgr.GetFieldIDsPermission(this.personas, true);
      }
      if (syncFields)
        this.syncNewFields();
      this.loadPipelineViews(!(this.userID == "") ? this.pipelineViewMgr.GetUserPipelineViews(this.userID) : this.pipelineViewMgr.GetPersonaPipelineViews(this.personaID));
    }

    private void syncNewFields()
    {
      Dictionary<string, string> dictionary1 = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      Dictionary<string, string> dictionary2 = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      Dictionary<string, AclTriState> dictionary3 = this.userID == "" ? this.accessColList : this.accessColList_Personas;
      foreach (ReportFieldDef loanFieldDef in (ReportFieldDefContainer) this.loanFieldDefs)
      {
        if (!dictionary3.ContainsKey(loanFieldDef.FieldID))
          dictionary1[loanFieldDef.FieldID] = loanFieldDef.FieldID;
      }
      foreach (string key in dictionary3.Keys)
      {
        if (this.loanFieldDefs.GetFieldByID(key) == null)
          dictionary2[key] = key;
      }
      if (dictionary1.Count <= 0 && dictionary2.Count <= 0)
        return;
      List<string> stringList1 = new List<string>((IEnumerable<string>) dictionary1.Keys);
      List<string> stringList2 = new List<string>((IEnumerable<string>) dictionary2.Keys);
      this.fieldAccessMgr.SyncFieldIDList(stringList1.ToArray(), stringList2.ToArray());
      if (this.userID == "")
      {
        this.accessColList = this.fieldAccessMgr.GetFieldIDsPermission(this.personaID);
      }
      else
      {
        this.accessColList_User = this.fieldAccessMgr.GetFieldIDsPermission(this.userID);
        this.accessColList_Personas = this.fieldAccessMgr.GetFieldIDsPermission(this.personas);
      }
    }

    private PersonaPipelineView[] getPipelineViews()
    {
      List<PersonaPipelineView> personaPipelineViewList = new List<PersonaPipelineView>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvPipelineViews.Items)
        personaPipelineViewList.Add((PersonaPipelineView) gvItem.Tag);
      return personaPipelineViewList.ToArray();
    }

    private void loadPipelineViews(PersonaPipelineView[] views)
    {
      this.gvPipelineViews.Items.Clear();
      foreach (PersonaPipelineView view in views)
        this.gvPipelineViews.Items.Add(this.createGVItemForPipelineView(view));
    }

    private GVItem createGVItemForPipelineView(PersonaPipelineView view)
    {
      GVItem itemForPipelineView = new GVItem();
      itemForPipelineView.Tag = (object) view;
      this.populatePipelineViewGVItem(itemForPipelineView);
      return itemForPipelineView;
    }

    private void populatePipelineViewGVItem(GVItem item)
    {
      PersonaPipelineView tag = (PersonaPipelineView) item.Tag;
      item.SubItems[0].Text = tag.Name;
    }

    private void siBtnEditPipelineView_Click(object sender, EventArgs e)
    {
      this.editPipelineView(this.gvPipelineViews.SelectedItems[0]);
    }

    private void editPipelineView(GVItem viewItem)
    {
      using (PipelineViewEditor pipelineViewEditor = new PipelineViewEditor(this.session, (PersonaPipelineView) viewItem.Tag, this.loanFieldDefs, this.getInUseViewNames()))
      {
        if (pipelineViewEditor.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.populatePipelineViewGVItem(viewItem);
        this.setStatus(true);
      }
    }

    private void siBtnAddPipelineView_Click(object sender, EventArgs e)
    {
      PersonaPipelineView view = (PersonaPipelineView) null;
      using (NewPipelineViewDialog pipelineViewDialog = new NewPipelineViewDialog(this.session, this.personaID))
      {
        if (pipelineViewDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        view = pipelineViewDialog.GetPipelineView();
      }
      using (PipelineViewEditor pipelineViewEditor = new PipelineViewEditor(this.session, view, this.loanFieldDefs, this.getInUseViewNames()))
      {
        if (pipelineViewEditor.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        int nItemIndex = this.gvPipelineViews.Items.Add(this.createGVItemForPipelineView(view));
        this.gvPipelineViews.SelectedItems.Clear();
        this.gvPipelineViews.Items[nItemIndex].Selected = true;
        this.setStatus(true);
      }
    }

    private void siBtnDeleteView_Click(object sender, EventArgs e)
    {
      if (this.gvPipelineViews.SelectedItems.Count == this.gvPipelineViews.Items.Count)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Each persona must have at least one Pipeline View.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        for (int nItemIndex = this.gvPipelineViews.Items.Count - 1; nItemIndex >= 0; --nItemIndex)
        {
          if (this.gvPipelineViews.Items[nItemIndex].Selected)
            this.gvPipelineViews.Items.RemoveAt(nItemIndex);
        }
        this.setStatus(true);
      }
    }

    private string[] getInUseViewNames()
    {
      List<string> stringList = new List<string>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvPipelineViews.Items)
        stringList.Add(((PersonaPipelineView) gvItem.Tag).Name);
      return stringList.ToArray();
    }

    private void gvPipelineView_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!(this.userID == ""))
        return;
      int count = this.gvPipelineViews.SelectedItems.Count;
      this.siBtnDeleteView.Enabled = count > 0;
      this.siBtnEditPipelineView.Enabled = count == 1;
      this.siBtnMoveViewDown.Enabled = count > 0;
      this.siBtnMoveViewUp.Enabled = count > 0;
    }

    private void gvPipelineViews_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      if (!(this.userID == ""))
        return;
      this.editPipelineView(e.Item);
    }

    private void siBtnMoveViewUp_Click(object sender, EventArgs e)
    {
      for (int nItemIndex = 1; nItemIndex < this.gvPipelineViews.Items.Count; ++nItemIndex)
      {
        if (this.gvPipelineViews.Items[nItemIndex].Selected && !this.gvPipelineViews.Items[nItemIndex - 1].Selected)
        {
          GVItem gvItem = this.gvPipelineViews.Items[nItemIndex];
          this.gvPipelineViews.Items.RemoveAt(nItemIndex);
          this.gvPipelineViews.Items.Insert(nItemIndex - 1, gvItem);
          gvItem.Selected = true;
          this.setStatus(true);
        }
      }
    }

    private void siBtnMoveViewDown_Click(object sender, EventArgs e)
    {
      for (int nItemIndex = this.gvPipelineViews.Items.Count - 2; nItemIndex >= 0; --nItemIndex)
      {
        if (this.gvPipelineViews.Items[nItemIndex].Selected && !this.gvPipelineViews.Items[nItemIndex + 1].Selected)
        {
          GVItem gvItem = this.gvPipelineViews.Items[nItemIndex];
          this.gvPipelineViews.Items.RemoveAt(nItemIndex);
          this.gvPipelineViews.Items.Insert(nItemIndex + 1, gvItem);
          gvItem.Selected = true;
          this.setStatus(true);
        }
      }
    }

    private void linkWithPersonaRightsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.currentlySelectedNode == null)
        return;
      AclFeature[] tag = (AclFeature[]) this.currentlySelectedNode.Tag;
      bool flag = false;
      foreach (AclFeature key in tag)
      {
        if (this.personaAccessSetting.ContainsKey((object) key))
        {
          if (!(bool) this.personaAccessSetting[(object) key])
          {
            flag = false;
            break;
          }
          flag = true;
        }
      }
      this.currentlySelectedNode.Checked = flag;
      this.currentlySelectedNode.ImageIndex = this.currentlySelectedNode.SelectedImageIndex = this.currentlySelectedNode.StateImageIndex = this.linked;
    }

    private void disconnectFromPersonaRightsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.currentlySelectedNode == null)
        return;
      this.currentlySelectedNode.ImageIndex = this.currentlySelectedNode.SelectedImageIndex = this.currentlySelectedNode.StateImageIndex = this.broken;
    }

    private void tv_AfterSelect(object sender, TreeViewEventArgs e)
    {
    }

    private void tv_BeforeCheck(object sender, TreeViewCancelEventArgs e)
    {
      if (this.suspendEvent || !this.forcedReadOnly)
        return;
      e.Cancel = true;
    }

    private void tvLoan_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
    {
      if (e.Node.Text != "Import Loans")
        return;
      if (this.personaID > 0)
      {
        if (this.impDlg == null || this.selectOption < 2)
        {
          if (this.selectOption < 2 && !this.cachedData.ContainsKey((object) AclFeature.LoanMgmt_Import))
          {
            this.impDlg = new ImportLoanDlg(this.session, this.personaID, this.forcedReadOnly, 2);
            this.cachedData[(object) AclFeature.LoanMgmt_Import] = (object) this.impDlg.DataView;
          }
          this.impDlg = new ImportLoanDlg(this.session, this.personaID, this.forcedReadOnly, this.selectOption);
          this.selectOption = 2;
        }
      }
      else if (this.impDlg == null || this.selectOption < 2)
      {
        if (this.selectOption < 2 && !this.cachedData.ContainsKey((object) AclFeature.LoanMgmt_Import))
        {
          this.impDlg = new ImportLoanDlg(this.session, this.userID, this.personas, this.forcedReadOnly, 2);
          this.cachedData[(object) AclFeature.LoanMgmt_Import] = (object) this.impDlg.DataView;
        }
        this.impDlg = new ImportLoanDlg(this.session, this.userID, this.personas, this.forcedReadOnly, this.selectOption);
        this.selectOption = 2;
      }
      if (this.cachedData[(object) AclFeature.LoanMgmt_Import] != null)
        this.impDlg.DataView = (ArrayList) this.cachedData[(object) AclFeature.LoanMgmt_Import];
      else
        this.cachedData[(object) AclFeature.LoanMgmt_Import] = (object) this.impDlg.DataView;
      this.suspendEvent = true;
      this.impDlg.IsReadOnly = this.forcedReadOnly;
      if (DialogResult.OK == this.impDlg.ShowDialog((IWin32Window) this))
      {
        this.cachedData[(object) AclFeature.LoanMgmt_Import] = (object) this.impDlg.DataView;
        if (this.impDlg.HasBeenModified())
          this.setStatus(true);
        if (this.userID != "")
          e.Node.ImageIndex = this.impDlg.GetImageIndex() != 1 ? (e.Node.StateImageIndex = e.Node.SelectedImageIndex = this.linked) : (e.Node.StateImageIndex = e.Node.SelectedImageIndex = this.broken);
      }
      if (this.impDlg.HasSomethingChecked() && !e.Node.Checked)
        e.Node.Checked = true;
      else if (!this.impDlg.HasSomethingChecked() && e.Node.Checked)
        e.Node.Checked = false;
      this.suspendEvent = false;
    }

    private void tvLoan_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Right)
        return;
      TreeNode nodeAt = this.tvLoan.GetNodeAt(e.X, e.Y);
      if (nodeAt == null)
        return;
      this.currentlySelectedNode = nodeAt;
      this.tvLoan.SelectedNode = nodeAt;
    }

    private void tvContact_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Right)
        return;
      TreeNode nodeAt = this.tvContact.GetNodeAt(e.X, e.Y);
      if (nodeAt == null)
        return;
      this.currentlySelectedNode = nodeAt;
      this.tvContact.SelectedNode = nodeAt;
    }

    private void tvDashabordReport_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Right)
        return;
      TreeNode nodeAt = this.tvDashabordReport.GetNodeAt(e.X, e.Y);
      if (nodeAt == null)
        return;
      this.currentlySelectedNode = nodeAt;
      this.tvDashabordReport.SelectedNode = nodeAt;
    }

    private void tvSettings_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Right)
        return;
      TreeNode nodeAt = this.tvSettings.GetNodeAt(e.X, e.Y);
      if (nodeAt == null)
        return;
      this.currentlySelectedNode = nodeAt;
      this.tvSettings.SelectedNode = nodeAt;
    }

    private void rdo_PlatformAccessCheckedChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent)
        return;
      this.setStatus(true);
      if (this.userID != "")
        this.lblPlatforms.ImageIndex = 0;
      if ((this.currentPlatformAccessSelection == 0 || this.currentPlatformAccessSelection == -1) && this.rdoBoth.Checked)
      {
        this.currentPlatformAccessSelection = 1;
      }
      else
      {
        if (!this.rdoDesktop.Checked)
          return;
        this.currentPlatformAccessSelection = 0;
      }
    }

    private int selectedPlatformAccess => this.rdoDesktop.Checked ? 0 : 1;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AccessPage));
      TreeNode treeNode1 = new TreeNode("Export Contacts");
      TreeNode treeNode2 = new TreeNode("Synchronize Contacts");
      TreeNode treeNode3 = new TreeNode("Assign Tasks to Others");
      TreeNode treeNode4 = new TreeNode("Access to Campaign Management", new TreeNode[1]
      {
        treeNode3
      });
      TreeNode treeNode5 = new TreeNode("Manage Personal Custom Letters and Campaign Template");
      TreeNode treeNode6 = new TreeNode("Access to Contacts Tab", new TreeNode[4]
      {
        treeNode1,
        treeNode2,
        treeNode4,
        treeNode5
      });
      TreeNode treeNode7 = new TreeNode("Import Loans");
      TreeNode treeNode8 = new TreeNode("Transfer Loans");
      TreeNode treeNode9 = new TreeNode("Manage Service Provider's List");
      TreeNode treeNode10 = new TreeNode("Manage Plan Code");
      TreeNode treeNode11 = new TreeNode("Manage Alt Lender");
      TreeNode treeNode12 = new TreeNode("eSign Packages");
      TreeNode treeNode13 = new TreeNode("Packages Tab", new TreeNode[1]
      {
        treeNode12
      });
      TreeNode treeNode14 = new TreeNode("Search AllRegs");
      TreeNode treeNode15 = new TreeNode("Export ULAD");
      TreeNode treeNode16 = new TreeNode("Export iLAD");
      TreeNode treeNode17 = new TreeNode("Export Fannie Mae Formatted File (3.2)");
      TreeNode treeNode18 = new TreeNode("Manage Personal Custom Print Forms and Form Groups");
      TreeNode treeNode19 = new TreeNode("Loan Custom Fields");
      TreeNode treeNode20 = new TreeNode("eFolder Setup");
      TreeNode treeNode21 = new TreeNode("Manage Personal Loan Templates");
      TreeNode treeNode22 = new TreeNode("Itemization Fee Management");
      TreeNode treeNode23 = new TreeNode("Tables and Fees", new TreeNode[1]
      {
        treeNode22
      });
      TreeNode treeNode24 = new TreeNode("Allow loan files to be opened for data (slower performance)");
      TreeNode treeNode25 = new TreeNode("Manage Personal Report Templates");
      TreeNode treeNode26 = new TreeNode("Access to Reports tab", new TreeNode[2]
      {
        treeNode24,
        treeNode25
      });
      TreeNode treeNode27 = new TreeNode("Manage Personal Dashboard Templates");
      TreeNode treeNode28 = new TreeNode("Access to Dashboard tab", new TreeNode[1]
      {
        treeNode27
      });
      GVColumn gvColumn = new GVColumn();
      TreeNode treeNode29 = new TreeNode("Enhanced Field Change");
      TreeNode treeNode30 = new TreeNode("Subscribe to Webhooks", new TreeNode[1]
      {
        treeNode29
      });
      this.splitter1 = new Splitter();
      this.imgListLink = new ImageList(this.components);
      this.pnlExRight = new PanelEx();
      this.pnlExContacts = new PanelEx();
      this.gcContacts = new GroupContainer();
      this.tvContact = new TreeView();
      this.splitter2 = new Splitter();
      this.pnlExLoans = new PanelEx();
      this.gcLoans = new GroupContainer();
      this.tvLoan = new TreeView();
      this.splitter6 = new Splitter();
      this.pnlPipelineTasks = new PanelEx();
      this.gcPipelineTasks = new GroupContainer();
      this.tvPipelineTasks = new TreeView();
      this.pnlExMbl = new PanelEx();
      this.groupContainerPlatformAccess = new GroupContainer();
      this.pnlExInsideMobileAccess = new PanelEx();
      this.rdoBoth = new RadioButton();
      this.lblPlatforms = new Label();
      this.rdoDesktop = new RadioButton();
      this.splitter4 = new Splitter();
      this.pnlExSettings = new PanelEx();
      this.gcSettings = new GroupContainer();
      this.tvSettings = new TreeView();
      this.pnlExDashboardReport = new PanelEx();
      this.gcDashboardReport = new GroupContainer();
      this.tvDashabordReport = new TreeView();
      this.pnlExLeft = new PanelEx();
      this.gcPipelineView = new GroupContainer();
      this.siBtnMoveViewDown = new StandardIconButton();
      this.siBtnMoveViewUp = new StandardIconButton();
      this.siBtnEditPipelineView = new StandardIconButton();
      this.siBtnDeleteView = new StandardIconButton();
      this.siBtnAddPipelineView = new StandardIconButton();
      this.gvPipelineViews = new GridView();
      this.contextMenuTree = new ContextMenuStrip(this.components);
      this.linkWithPersonaRightsToolStripMenuItem = new ToolStripMenuItem();
      this.disconnectFromPersonaRightsToolStripMenuItem = new ToolStripMenuItem();
      this.toolTip1 = new ToolTip(this.components);
      this.panelExDevConnect = new PanelEx();
      this.gcDeveloperConnect = new GroupContainer();
      this.tvDeveloperConnect = new TreeView();
      this.pnlExNew = new PanelEx();
      this.splitter3 = new Splitter();
      this.splitter7 = new Splitter();
      this.pnlExRight.SuspendLayout();
      this.pnlExContacts.SuspendLayout();
      this.gcContacts.SuspendLayout();
      this.pnlExLoans.SuspendLayout();
      this.gcLoans.SuspendLayout();
      this.pnlPipelineTasks.SuspendLayout();
      this.gcPipelineTasks.SuspendLayout();
      this.pnlExMbl.SuspendLayout();
      this.groupContainerPlatformAccess.SuspendLayout();
      this.pnlExInsideMobileAccess.SuspendLayout();
      this.pnlExSettings.SuspendLayout();
      this.gcSettings.SuspendLayout();
      this.pnlExDashboardReport.SuspendLayout();
      this.gcDashboardReport.SuspendLayout();
      this.pnlExLeft.SuspendLayout();
      this.gcPipelineView.SuspendLayout();
      ((ISupportInitialize) this.siBtnMoveViewDown).BeginInit();
      ((ISupportInitialize) this.siBtnMoveViewUp).BeginInit();
      ((ISupportInitialize) this.siBtnEditPipelineView).BeginInit();
      ((ISupportInitialize) this.siBtnDeleteView).BeginInit();
      ((ISupportInitialize) this.siBtnAddPipelineView).BeginInit();
      this.contextMenuTree.SuspendLayout();
      this.panelExDevConnect.SuspendLayout();
      this.gcDeveloperConnect.SuspendLayout();
      this.pnlExNew.SuspendLayout();
      this.SuspendLayout();
      this.splitter1.Location = new Point(232, 0);
      this.splitter1.Name = "splitter1";
      this.splitter1.Size = new Size(3, 642);
      this.splitter1.TabIndex = 1;
      this.splitter1.TabStop = false;
      this.imgListLink.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgListLink.ImageStream");
      this.imgListLink.TransparentColor = Color.Transparent;
      this.imgListLink.Images.SetKeyName(0, "link-broken.png");
      this.imgListLink.Images.SetKeyName(1, "link.png");
      this.pnlExRight.Controls.Add((Control) this.pnlExContacts);
      this.pnlExRight.Controls.Add((Control) this.splitter2);
      this.pnlExRight.Controls.Add((Control) this.pnlExLoans);
      this.pnlExRight.Controls.Add((Control) this.splitter6);
      this.pnlExRight.Controls.Add((Control) this.pnlPipelineTasks);
      this.pnlExRight.Dock = DockStyle.Left;
      this.pnlExRight.Location = new Point(235, 0);
      this.pnlExRight.Name = "pnlExRight";
      this.pnlExRight.Size = new Size(250, 642);
      this.pnlExRight.TabIndex = 2;
      this.pnlExContacts.Controls.Add((Control) this.gcContacts);
      this.pnlExContacts.Dock = DockStyle.Fill;
      this.pnlExContacts.Location = new Point(0, 221);
      this.pnlExContacts.Name = "pnlExContacts";
      this.pnlExContacts.Size = new Size(250, 421);
      this.pnlExContacts.TabIndex = 2;
      this.gcContacts.Controls.Add((Control) this.tvContact);
      this.gcContacts.Dock = DockStyle.Fill;
      this.gcContacts.HeaderForeColor = SystemColors.ControlText;
      this.gcContacts.Location = new Point(0, 0);
      this.gcContacts.Name = "gcContacts";
      this.gcContacts.Size = new Size(250, 421);
      this.gcContacts.TabIndex = 0;
      this.gcContacts.Text = "Contacts";
      this.tvContact.BorderStyle = BorderStyle.None;
      this.tvContact.CheckBoxes = true;
      this.tvContact.Dock = DockStyle.Fill;
      this.tvContact.Location = new Point(1, 26);
      this.tvContact.Name = "tvContact";
      treeNode1.Name = "contact_ECNode";
      treeNode1.Text = "Export Contacts";
      treeNode2.Name = "contact_SCNode";
      treeNode2.Text = "Synchronize Contacts";
      treeNode3.Name = "contact_ACM_ATONode";
      treeNode3.Text = "Assign Tasks to Others";
      treeNode4.Name = "contact_ACMNode";
      treeNode4.Text = "Access to Campaign Management";
      treeNode5.Name = "contact_MPCLCTNode";
      treeNode5.Text = "Manage Personal Custom Letters and Campaign Template";
      treeNode6.Name = "nodeAccessContact";
      treeNode6.Text = "Access to Contacts Tab";
      this.tvContact.Nodes.AddRange(new TreeNode[1]
      {
        treeNode6
      });
      this.tvContact.ShowLines = false;
      this.tvContact.ShowPlusMinus = false;
      this.tvContact.Size = new Size(248, 394);
      this.tvContact.TabIndex = 0;
      this.tvContact.BeforeCheck += new TreeViewCancelEventHandler(this.tv_BeforeCheck);
      this.tvContact.AfterCheck += new TreeViewEventHandler(this.tv_AfterCheck);
      this.tvContact.BeforeCollapse += new TreeViewCancelEventHandler(this.tv_BeforeCollapse);
      this.tvContact.MouseDown += new MouseEventHandler(this.tvContact_MouseDown);
      this.splitter2.Dock = DockStyle.Top;
      this.splitter2.Location = new Point(0, 218);
      this.splitter2.Name = "splitter2";
      this.splitter2.Size = new Size(250, 3);
      this.splitter2.TabIndex = 1;
      this.splitter2.TabStop = false;
      this.pnlExLoans.Controls.Add((Control) this.gcLoans);
      this.pnlExLoans.Dock = DockStyle.Top;
      this.pnlExLoans.Location = new Point(0, 103);
      this.pnlExLoans.Name = "pnlExLoans";
      this.pnlExLoans.Size = new Size(250, 115);
      this.pnlExLoans.TabIndex = 0;
      this.gcLoans.Controls.Add((Control) this.tvLoan);
      this.gcLoans.Dock = DockStyle.Fill;
      this.gcLoans.HeaderForeColor = SystemColors.ControlText;
      this.gcLoans.Location = new Point(0, 0);
      this.gcLoans.Name = "gcLoans";
      this.gcLoans.Size = new Size(250, 115);
      this.gcLoans.TabIndex = 0;
      this.gcLoans.Text = "Loans";
      this.tvLoan.BorderStyle = BorderStyle.None;
      this.tvLoan.CheckBoxes = true;
      this.tvLoan.Dock = DockStyle.Fill;
      this.tvLoan.Indent = 19;
      this.tvLoan.Location = new Point(1, 26);
      this.tvLoan.Name = "tvLoan";
      treeNode7.Name = "loan_ILNodes";
      treeNode7.Text = "Import Loans";
      treeNode8.Name = "loan_TLNode";
      treeNode8.Text = "Transfer Loans";
      treeNode9.Name = "loan_MSPLNode";
      treeNode9.Text = "Manage Service Provider's List";
      treeNode10.Name = "loan_MPCNode";
      treeNode10.Text = "Manage Plan Code";
      treeNode11.Name = "loan_MALNode";
      treeNode11.Text = "Manage Alt Lender";
      treeNode12.Name = "loan_eSignPackagesNode";
      treeNode12.Text = "eSign Packages";
      treeNode13.Name = "loan_PackagesNode";
      treeNode13.Text = "Packages Tab";
      treeNode14.Name = "loan_searchAllRegs";
      treeNode14.Text = "Search AllRegs";
      this.tvLoan.Nodes.AddRange(new TreeNode[7]
      {
        treeNode7,
        treeNode8,
        treeNode9,
        treeNode10,
        treeNode11,
        treeNode13,
        treeNode14
      });
      this.tvLoan.ShowLines = false;
      this.tvLoan.ShowPlusMinus = false;
      this.tvLoan.Size = new Size(248, 88);
      this.tvLoan.TabIndex = 0;
      this.tvLoan.BeforeCheck += new TreeViewCancelEventHandler(this.tv_BeforeCheck);
      this.tvLoan.AfterCheck += new TreeViewEventHandler(this.tv_AfterCheck);
      this.tvLoan.BeforeCollapse += new TreeViewCancelEventHandler(this.tv_BeforeCollapse);
      this.tvLoan.NodeMouseClick += new TreeNodeMouseClickEventHandler(this.tvLoan_NodeMouseClick);
      this.tvLoan.MouseDown += new MouseEventHandler(this.tvLoan_MouseDown);
      this.splitter6.Dock = DockStyle.Top;
      this.splitter6.Location = new Point(0, 100);
      this.splitter6.Name = "splitter6";
      this.splitter6.Size = new Size(250, 3);
      this.splitter6.TabIndex = 1;
      this.splitter6.TabStop = false;
      this.pnlPipelineTasks.Controls.Add((Control) this.gcPipelineTasks);
      this.pnlPipelineTasks.Dock = DockStyle.Top;
      this.pnlPipelineTasks.Location = new Point(0, 0);
      this.pnlPipelineTasks.Name = "pnlPipelineTasks";
      this.pnlPipelineTasks.Size = new Size(250, 100);
      this.pnlPipelineTasks.TabIndex = 0;
      this.gcPipelineTasks.Controls.Add((Control) this.tvPipelineTasks);
      this.gcPipelineTasks.Dock = DockStyle.Fill;
      this.gcPipelineTasks.HeaderForeColor = SystemColors.ControlText;
      this.gcPipelineTasks.Location = new Point(0, 0);
      this.gcPipelineTasks.Name = "gcPipelineTasks";
      this.gcPipelineTasks.Size = new Size(250, 100);
      this.gcPipelineTasks.TabIndex = 0;
      this.gcPipelineTasks.Text = "Pipeline Tasks";
      this.tvPipelineTasks.BorderStyle = BorderStyle.None;
      this.tvPipelineTasks.CheckBoxes = true;
      this.tvPipelineTasks.Dock = DockStyle.Fill;
      this.tvPipelineTasks.Indent = 19;
      this.tvPipelineTasks.Location = new Point(1, 26);
      this.tvPipelineTasks.Name = "tvPipelineTasks";
      treeNode15.Name = "Export_ULAD_Node";
      treeNode15.Text = "Export ULAD";
      treeNode16.Name = "Export_ILAD_Node";
      treeNode16.Text = "Export iLAD";
      treeNode17.Name = "Export_Fannie_Node";
      treeNode17.Text = "Export Fannie Mae Formatted File (3.2)";
      this.tvPipelineTasks.Nodes.AddRange(new TreeNode[3]
      {
        treeNode15,
        treeNode16,
        treeNode17
      });
      if (string.Equals(this.session.ConfigurationManager.GetCompanySetting("POLICIES", "EnableLoanSoftArchival"), "true", StringComparison.OrdinalIgnoreCase))
      {
        this.tvPipelineTasks.Nodes.AddRange(new TreeNode[1]
        {
          new TreeNode("Access to Archive Folders")
          {
            Name = "AccessToArchiveFolders_Node",
            Text = "Access to Archive Folders"
          }
        });
        this.tvPipelineTasks.Nodes.AddRange(new TreeNode[1]
        {
          new TreeNode("Access to Archive Loans")
          {
            Name = "AccessToArchiveLoans_Node",
            Text = "Access to Archive Loans"
          }
        });
      }
      this.tvPipelineTasks.ShowLines = false;
      this.tvPipelineTasks.ShowPlusMinus = false;
      this.tvPipelineTasks.Size = new Size(248, 73);
      this.tvPipelineTasks.TabIndex = 0;
      this.pnlExMbl.Controls.Add((Control) this.groupContainerPlatformAccess);
      this.pnlExMbl.Dock = DockStyle.Top;
      this.pnlExMbl.Location = new Point(0, 221);
      this.pnlExMbl.Name = "pnlExMbl";
      this.pnlExMbl.Size = new Size(312, 122);
      this.pnlExMbl.TabIndex = 8;
      this.groupContainerPlatformAccess.Controls.Add((Control) this.pnlExInsideMobileAccess);
      this.groupContainerPlatformAccess.Dock = DockStyle.Fill;
      this.groupContainerPlatformAccess.HeaderForeColor = SystemColors.ControlText;
      this.groupContainerPlatformAccess.Location = new Point(0, 0);
      this.groupContainerPlatformAccess.Name = "groupContainerPlatformAccess";
      this.groupContainerPlatformAccess.Size = new Size(312, 122);
      this.groupContainerPlatformAccess.TabIndex = 9;
      this.groupContainerPlatformAccess.Text = "Encompass Access";
      this.pnlExInsideMobileAccess.AutoScroll = true;
      this.pnlExInsideMobileAccess.Controls.Add((Control) this.rdoBoth);
      this.pnlExInsideMobileAccess.Controls.Add((Control) this.lblPlatforms);
      this.pnlExInsideMobileAccess.Controls.Add((Control) this.rdoDesktop);
      this.pnlExInsideMobileAccess.Dock = DockStyle.Fill;
      this.pnlExInsideMobileAccess.Location = new Point(1, 26);
      this.pnlExInsideMobileAccess.Name = "pnlExInsideMobileAccess";
      this.pnlExInsideMobileAccess.Size = new Size(310, 95);
      this.pnlExInsideMobileAccess.TabIndex = 0;
      this.rdoBoth.AutoSize = true;
      this.rdoBoth.Location = new Point(8, 48);
      this.rdoBoth.Name = "rdoBoth";
      this.rdoBoth.Size = new Size(341, 30);
      this.rdoBoth.TabIndex = 6;
      this.rdoBoth.TabStop = true;
      this.rdoBoth.Text = "Both desktop and web versions of Encompass (web version allows\r\n browser based access across desktop, tablet, and mobile devices)";
      this.rdoBoth.UseVisualStyleBackColor = true;
      this.rdoBoth.CheckedChanged += new EventHandler(this.rdo_PlatformAccessCheckedChanged);
      this.lblPlatforms.AutoSize = true;
      this.lblPlatforms.ImageAlign = ContentAlignment.MiddleRight;
      this.lblPlatforms.Location = new Point(5, 9);
      this.lblPlatforms.Name = "lblPlatforms";
      this.lblPlatforms.Size = new Size(82, 13);
      this.lblPlatforms.TabIndex = 7;
      this.lblPlatforms.Text = "Select one:       ";
      this.rdoDesktop.AutoSize = true;
      this.rdoDesktop.Location = new Point(8, 25);
      this.rdoDesktop.Name = "rdoDesktop";
      this.rdoDesktop.Size = new Size(172, 17);
      this.rdoDesktop.TabIndex = 5;
      this.rdoDesktop.TabStop = true;
      this.rdoDesktop.Text = "Desktop version of Encompass";
      this.rdoDesktop.UseVisualStyleBackColor = true;
      this.rdoDesktop.CheckedChanged += new EventHandler(this.rdo_PlatformAccessCheckedChanged);
      this.splitter4.Cursor = Cursors.HSplit;
      this.splitter4.Dock = DockStyle.Top;
      this.splitter4.Location = new Point(0, 218);
      this.splitter4.Name = "splitter4";
      this.splitter4.Size = new Size(312, 3);
      this.splitter4.TabIndex = 8;
      this.splitter4.TabStop = false;
      this.pnlExSettings.Controls.Add((Control) this.gcSettings);
      this.pnlExSettings.Dock = DockStyle.Top;
      this.pnlExSettings.Location = new Point(0, 103);
      this.pnlExSettings.Name = "pnlExSettings";
      this.pnlExSettings.Size = new Size(312, 115);
      this.pnlExSettings.TabIndex = 6;
      this.gcSettings.Controls.Add((Control) this.tvSettings);
      this.gcSettings.Dock = DockStyle.Fill;
      this.gcSettings.HeaderForeColor = SystemColors.ControlText;
      this.gcSettings.Location = new Point(0, 0);
      this.gcSettings.Name = "gcSettings";
      this.gcSettings.Size = new Size(312, 115);
      this.gcSettings.TabIndex = 0;
      this.gcSettings.Text = "Settings";
      this.tvSettings.BorderStyle = BorderStyle.None;
      this.tvSettings.CheckBoxes = true;
      this.tvSettings.Dock = DockStyle.Fill;
      this.tvSettings.Location = new Point(1, 26);
      this.tvSettings.Name = "tvSettings";
      treeNode18.Name = "setting_CusPrintNode";
      treeNode18.Text = "Manage Personal Custom Print Forms and Form Groups";
      treeNode19.Name = "setting_LCFNode";
      treeNode19.Text = "Loan Custom Fields";
      treeNode20.Name = "setting_eFolderNode";
      treeNode20.Text = "eFolder Setup";
      treeNode21.Name = "setting_LoanTemplateNode";
      treeNode21.Text = "Manage Personal Loan Templates";
      treeNode22.Name = "setting_2010ItemFeeMgmt";
      treeNode22.Text = "Itemization Fee Management";
      treeNode23.Name = "setting_TableFeeNode";
      treeNode23.Text = "Tables and Fees";
      this.tvSettings.Nodes.AddRange(new TreeNode[5]
      {
        treeNode18,
        treeNode19,
        treeNode20,
        treeNode21,
        treeNode23
      });
      this.tvSettings.ShowLines = false;
      this.tvSettings.ShowPlusMinus = false;
      this.tvSettings.Size = new Size(310, 88);
      this.tvSettings.TabIndex = 0;
      this.tvSettings.BeforeCheck += new TreeViewCancelEventHandler(this.tv_BeforeCheck);
      this.tvSettings.AfterCheck += new TreeViewEventHandler(this.tv_AfterCheck);
      this.tvSettings.BeforeCollapse += new TreeViewCancelEventHandler(this.tv_BeforeCollapse);
      this.tvSettings.MouseDown += new MouseEventHandler(this.tvSettings_MouseDown);
      this.pnlExDashboardReport.Controls.Add((Control) this.gcDashboardReport);
      this.pnlExDashboardReport.Dock = DockStyle.Top;
      this.pnlExDashboardReport.Location = new Point(0, 0);
      this.pnlExDashboardReport.Name = "pnlExDashboardReport";
      this.pnlExDashboardReport.Size = new Size(312, 100);
      this.pnlExDashboardReport.TabIndex = 4;
      this.gcDashboardReport.Controls.Add((Control) this.tvDashabordReport);
      this.gcDashboardReport.Dock = DockStyle.Fill;
      this.gcDashboardReport.HeaderForeColor = SystemColors.ControlText;
      this.gcDashboardReport.Location = new Point(0, 0);
      this.gcDashboardReport.Name = "gcDashboardReport";
      this.gcDashboardReport.Size = new Size(312, 100);
      this.gcDashboardReport.TabIndex = 0;
      this.gcDashboardReport.Text = "Dashboard/Reports";
      this.tvDashabordReport.BorderStyle = BorderStyle.None;
      this.tvDashabordReport.CheckBoxes = true;
      this.tvDashabordReport.Dock = DockStyle.Fill;
      this.tvDashabordReport.Location = new Point(1, 26);
      this.tvDashabordReport.Name = "tvDashabordReport";
      treeNode24.Name = "dashReport_ARDBNode";
      treeNode24.Text = "Allow loan files to be opened for data (slower performance)";
      treeNode25.Name = "reportTemplate_Node";
      treeNode25.Text = "Manage Personal Report Templates";
      treeNode26.Name = "dashReport_ARNode";
      treeNode26.Text = "Access to Reports tab";
      treeNode27.Name = "dashboardTemp_Node";
      treeNode27.Text = "Manage Personal Dashboard Templates";
      treeNode28.Name = "dashboard_Node";
      treeNode28.Text = "Access to Dashboard tab";
      this.tvDashabordReport.Nodes.AddRange(new TreeNode[2]
      {
        treeNode26,
        treeNode28
      });
      this.tvDashabordReport.ShowLines = false;
      this.tvDashabordReport.ShowPlusMinus = false;
      this.tvDashabordReport.Size = new Size(310, 73);
      this.tvDashabordReport.TabIndex = 0;
      this.tvDashabordReport.BeforeCheck += new TreeViewCancelEventHandler(this.tv_BeforeCheck);
      this.tvDashabordReport.AfterCheck += new TreeViewEventHandler(this.tv_AfterCheck);
      this.tvDashabordReport.BeforeCollapse += new TreeViewCancelEventHandler(this.tv_BeforeCollapse);
      this.tvDashabordReport.MouseDown += new MouseEventHandler(this.tvDashabordReport_MouseDown);
      this.pnlExLeft.Controls.Add((Control) this.gcPipelineView);
      this.pnlExLeft.Dock = DockStyle.Left;
      this.pnlExLeft.Location = new Point(0, 0);
      this.pnlExLeft.Name = "pnlExLeft";
      this.pnlExLeft.Size = new Size(232, 642);
      this.pnlExLeft.TabIndex = 0;
      this.gcPipelineView.Controls.Add((Control) this.siBtnMoveViewDown);
      this.gcPipelineView.Controls.Add((Control) this.siBtnMoveViewUp);
      this.gcPipelineView.Controls.Add((Control) this.siBtnEditPipelineView);
      this.gcPipelineView.Controls.Add((Control) this.siBtnDeleteView);
      this.gcPipelineView.Controls.Add((Control) this.siBtnAddPipelineView);
      this.gcPipelineView.Controls.Add((Control) this.gvPipelineViews);
      this.gcPipelineView.Dock = DockStyle.Fill;
      this.gcPipelineView.HeaderForeColor = SystemColors.ControlText;
      this.gcPipelineView.Location = new Point(0, 0);
      this.gcPipelineView.Name = "gcPipelineView";
      this.gcPipelineView.Size = new Size(232, 642);
      this.gcPipelineView.TabIndex = 1;
      this.gcPipelineView.Text = "Pipeline Views";
      this.siBtnMoveViewDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.siBtnMoveViewDown.BackColor = Color.Transparent;
      this.siBtnMoveViewDown.Enabled = false;
      this.siBtnMoveViewDown.Location = new Point(191, 4);
      this.siBtnMoveViewDown.MouseDownImage = (Image) null;
      this.siBtnMoveViewDown.Name = "siBtnMoveViewDown";
      this.siBtnMoveViewDown.Size = new Size(16, 16);
      this.siBtnMoveViewDown.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.siBtnMoveViewDown.TabIndex = 14;
      this.siBtnMoveViewDown.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.siBtnMoveViewDown, "Move View Down");
      this.siBtnMoveViewDown.Click += new EventHandler(this.siBtnMoveViewDown_Click);
      this.siBtnMoveViewUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.siBtnMoveViewUp.BackColor = Color.Transparent;
      this.siBtnMoveViewUp.Enabled = false;
      this.siBtnMoveViewUp.Location = new Point(170, 4);
      this.siBtnMoveViewUp.MouseDownImage = (Image) null;
      this.siBtnMoveViewUp.Name = "siBtnMoveViewUp";
      this.siBtnMoveViewUp.Size = new Size(16, 16);
      this.siBtnMoveViewUp.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.siBtnMoveViewUp.TabIndex = 13;
      this.siBtnMoveViewUp.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.siBtnMoveViewUp, "Move View Up");
      this.siBtnMoveViewUp.Click += new EventHandler(this.siBtnMoveViewUp_Click);
      this.siBtnEditPipelineView.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.siBtnEditPipelineView.BackColor = Color.Transparent;
      this.siBtnEditPipelineView.Enabled = false;
      this.siBtnEditPipelineView.Location = new Point(150, 4);
      this.siBtnEditPipelineView.MouseDownImage = (Image) null;
      this.siBtnEditPipelineView.Name = "siBtnEditPipelineView";
      this.siBtnEditPipelineView.Size = new Size(16, 16);
      this.siBtnEditPipelineView.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.siBtnEditPipelineView.TabIndex = 12;
      this.siBtnEditPipelineView.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.siBtnEditPipelineView, "Edit View");
      this.siBtnEditPipelineView.Click += new EventHandler(this.siBtnEditPipelineView_Click);
      this.siBtnDeleteView.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.siBtnDeleteView.BackColor = Color.Transparent;
      this.siBtnDeleteView.Enabled = false;
      this.siBtnDeleteView.Location = new Point(211, 4);
      this.siBtnDeleteView.MouseDownImage = (Image) null;
      this.siBtnDeleteView.Name = "siBtnDeleteView";
      this.siBtnDeleteView.Size = new Size(16, 16);
      this.siBtnDeleteView.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.siBtnDeleteView.TabIndex = 11;
      this.siBtnDeleteView.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.siBtnDeleteView, "Delete View");
      this.siBtnDeleteView.Click += new EventHandler(this.siBtnDeleteView_Click);
      this.siBtnAddPipelineView.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.siBtnAddPipelineView.BackColor = Color.Transparent;
      this.siBtnAddPipelineView.Location = new Point(129, 4);
      this.siBtnAddPipelineView.MouseDownImage = (Image) null;
      this.siBtnAddPipelineView.Name = "siBtnAddPipelineView";
      this.siBtnAddPipelineView.Size = new Size(16, 16);
      this.siBtnAddPipelineView.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.siBtnAddPipelineView.TabIndex = 10;
      this.siBtnAddPipelineView.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.siBtnAddPipelineView, "Add View");
      this.siBtnAddPipelineView.Click += new EventHandler(this.siBtnAddPipelineView_Click);
      this.gvPipelineViews.BorderStyle = BorderStyle.None;
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "Column1";
      gvColumn.SpringToFit = true;
      gvColumn.Text = "Name";
      gvColumn.Width = 230;
      this.gvPipelineViews.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.gvPipelineViews.Dock = DockStyle.Fill;
      this.gvPipelineViews.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvPipelineViews.Location = new Point(1, 26);
      this.gvPipelineViews.Name = "gvPipelineViews";
      this.gvPipelineViews.Size = new Size(230, 615);
      this.gvPipelineViews.SortOption = GVSortOption.None;
      this.gvPipelineViews.TabIndex = 7;
      this.gvPipelineViews.SelectedIndexChanged += new EventHandler(this.gvPipelineView_SelectedIndexChanged);
      this.gvPipelineViews.ItemDoubleClick += new GVItemEventHandler(this.gvPipelineViews_ItemDoubleClick);
      this.contextMenuTree.ImageScalingSize = new Size(24, 24);
      this.contextMenuTree.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.linkWithPersonaRightsToolStripMenuItem,
        (ToolStripItem) this.disconnectFromPersonaRightsToolStripMenuItem
      });
      this.contextMenuTree.Name = "contextMenuTree";
      this.contextMenuTree.ShowImageMargin = false;
      this.contextMenuTree.Size = new Size(219, 48);
      this.linkWithPersonaRightsToolStripMenuItem.Name = "linkWithPersonaRightsToolStripMenuItem";
      this.linkWithPersonaRightsToolStripMenuItem.Size = new Size(218, 22);
      this.linkWithPersonaRightsToolStripMenuItem.Text = "Link with Persona Rights";
      this.linkWithPersonaRightsToolStripMenuItem.Click += new EventHandler(this.linkWithPersonaRightsToolStripMenuItem_Click);
      this.disconnectFromPersonaRightsToolStripMenuItem.Name = "disconnectFromPersonaRightsToolStripMenuItem";
      this.disconnectFromPersonaRightsToolStripMenuItem.Size = new Size(218, 22);
      this.disconnectFromPersonaRightsToolStripMenuItem.Text = "Disconnect from Persona Rights";
      this.disconnectFromPersonaRightsToolStripMenuItem.Click += new EventHandler(this.disconnectFromPersonaRightsToolStripMenuItem_Click);
      this.panelExDevConnect.Controls.Add((Control) this.gcDeveloperConnect);
      this.panelExDevConnect.Dock = DockStyle.Fill;
      this.panelExDevConnect.Location = new Point(0, 343);
      this.panelExDevConnect.Margin = new Padding(2);
      this.panelExDevConnect.Name = "panelExDevConnect";
      this.panelExDevConnect.Size = new Size(312, 299);
      this.panelExDevConnect.TabIndex = 9;
      this.toolTip1.SetToolTip((Control) this.panelExDevConnect, "Developer Connect");
      this.gcDeveloperConnect.Controls.Add((Control) this.tvDeveloperConnect);
      this.gcDeveloperConnect.Dock = DockStyle.Fill;
      this.gcDeveloperConnect.HeaderForeColor = SystemColors.ControlText;
      this.gcDeveloperConnect.Location = new Point(0, 0);
      this.gcDeveloperConnect.Name = "gcDeveloperConnect";
      this.gcDeveloperConnect.Size = new Size(312, 299);
      this.gcDeveloperConnect.TabIndex = 1;
      this.gcDeveloperConnect.Text = "Developer Connect";
      this.tvDeveloperConnect.BorderStyle = BorderStyle.None;
      this.tvDeveloperConnect.CheckBoxes = true;
      this.tvDeveloperConnect.Dock = DockStyle.Fill;
      this.tvDeveloperConnect.Location = new Point(1, 26);
      this.tvDeveloperConnect.Name = "tvDeveloperConnect";
      treeNode29.Name = "EnhancedFieldChange_Node";
      treeNode29.Text = "Enhanced Field Change";
      treeNode30.Name = "SubscribetoWebhooks_Node";
      treeNode30.Text = "Subscribe to Webhooks";
      this.tvDeveloperConnect.Nodes.AddRange(new TreeNode[1]
      {
        treeNode30
      });
      this.tvDeveloperConnect.ShowLines = false;
      this.tvDeveloperConnect.ShowPlusMinus = false;
      this.tvDeveloperConnect.Size = new Size(310, 272);
      this.tvDeveloperConnect.TabIndex = 0;
      this.pnlExNew.Controls.Add((Control) this.panelExDevConnect);
      this.pnlExNew.Controls.Add((Control) this.pnlExMbl);
      this.pnlExNew.Controls.Add((Control) this.splitter4);
      this.pnlExNew.Controls.Add((Control) this.pnlExSettings);
      this.pnlExNew.Controls.Add((Control) this.splitter3);
      this.pnlExNew.Controls.Add((Control) this.pnlExDashboardReport);
      this.pnlExNew.Dock = DockStyle.Fill;
      this.pnlExNew.Location = new Point(488, 0);
      this.pnlExNew.Name = "pnlExNew";
      this.pnlExNew.Size = new Size(312, 642);
      this.pnlExNew.TabIndex = 4;
      this.splitter3.Cursor = Cursors.HSplit;
      this.splitter3.Dock = DockStyle.Top;
      this.splitter3.Location = new Point(0, 100);
      this.splitter3.Name = "splitter3";
      this.splitter3.Size = new Size(312, 3);
      this.splitter3.TabIndex = 8;
      this.splitter3.TabStop = false;
      this.splitter7.Location = new Point(485, 0);
      this.splitter7.Name = "splitter7";
      this.splitter7.Size = new Size(3, 642);
      this.splitter7.TabIndex = 2;
      this.splitter7.TabStop = false;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.pnlExNew);
      this.Controls.Add((Control) this.splitter7);
      this.Controls.Add((Control) this.pnlExRight);
      this.Controls.Add((Control) this.splitter1);
      this.Controls.Add((Control) this.pnlExLeft);
      this.Name = nameof (AccessPage);
      this.Size = new Size(800, 642);
      this.pnlExRight.ResumeLayout(false);
      this.pnlExContacts.ResumeLayout(false);
      this.gcContacts.ResumeLayout(false);
      this.pnlExLoans.ResumeLayout(false);
      this.gcLoans.ResumeLayout(false);
      this.pnlPipelineTasks.ResumeLayout(false);
      this.gcPipelineTasks.ResumeLayout(false);
      this.pnlExMbl.ResumeLayout(false);
      this.groupContainerPlatformAccess.ResumeLayout(false);
      this.pnlExInsideMobileAccess.ResumeLayout(false);
      this.pnlExInsideMobileAccess.PerformLayout();
      this.pnlExSettings.ResumeLayout(false);
      this.gcSettings.ResumeLayout(false);
      this.pnlExDashboardReport.ResumeLayout(false);
      this.gcDashboardReport.ResumeLayout(false);
      this.pnlExLeft.ResumeLayout(false);
      this.gcPipelineView.ResumeLayout(false);
      ((ISupportInitialize) this.siBtnMoveViewDown).EndInit();
      ((ISupportInitialize) this.siBtnMoveViewUp).EndInit();
      ((ISupportInitialize) this.siBtnEditPipelineView).EndInit();
      ((ISupportInitialize) this.siBtnDeleteView).EndInit();
      ((ISupportInitialize) this.siBtnAddPipelineView).EndInit();
      this.contextMenuTree.ResumeLayout(false);
      this.panelExDevConnect.ResumeLayout(false);
      this.gcDeveloperConnect.ResumeLayout(false);
      this.pnlExNew.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
