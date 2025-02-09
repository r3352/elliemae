// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.PipelineScreen
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using Elli.Metrics.Client;
using EllieMae.EMLite.BusinessRules;
using EllieMae.EMLite.ClientCommon;
using EllieMae.EMLite.ClientCommon.AIQCapsilon;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.DataDocs;
using EllieMae.EMLite.Common.DeepLinking;
using EllieMae.EMLite.Common.DeepLinking.Context;
using EllieMae.EMLite.Common.DeepLinking.Context.Contract;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.eFolder.Documents;
using EllieMae.EMLite.eFolder.Files;
using EllieMae.EMLite.eFolder.LoanCenter;
using EllieMae.EMLite.eFolder.ThinThick;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.ePass.Messaging;
using EllieMae.EMLite.Export;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.Import;
using EllieMae.EMLite.JedScriptEngine;
using EllieMae.EMLite.LoanServices;
using EllieMae.EMLite.LoanUtils.Workflow;
using EllieMae.EMLite.Properties;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.RemotingServices.Acl;
using EllieMae.EMLite.Reporting;
using EllieMae.EMLite.Setup;
using EllieMae.EMLite.Setup.ExternalOriginatorManagement;
using EllieMae.EMLite.UI;
using EllieMae.EMLite.UI.Controls;
using EllieMae.Encompass.AsmResolver;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public class PipelineScreen : UserControl, IPipeline, IMenuProvider, IOnlineHelpTarget
  {
    private const string className = "PipelineScreen";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private static bool alreadyDisplayedAlertWarningMessage = false;
    private static readonly string[] requiredFields = new string[12]
    {
      "Loan.Guid",
      "Loan.LoanNumber",
      "Loan.LoanName",
      "Loan.LoanFolder",
      "Loan.BorrowerLastName",
      "Loan.BorrowerFirstName",
      "Loan.LoanStatus",
      "Loan.Adverse",
      "Loan.CurrentMilestoneName",
      "Loan.ActionTaken",
      "Loan.NextMilestoneName",
      "Loan.LinkGuid"
    };
    private List<PipelineView> pipelineViews;
    private List<PipelineView> personaPipelineViews;
    private LoanFolderInfo[] loanFolderInfos;
    private PipelineView currentView;
    private ICursor pipelineCursor;
    private uint _majorOsVersion = 6;
    private GridViewLayoutManager layoutManager;
    private GridViewReportFilterManager filterManager;
    private FieldFilterList advFilter;
    private FileSystemEntry fsViewEntry;
    private bool suspendEvents;
    private bool suspendRefresh;
    private LoanReportFieldDefs fieldDefs;
    private DateTime lastRefreshTime = DateTime.MinValue;
    private bool invalidated;
    private string[] externalUsersIds;
    private string[] externalAndInternalUsersIds;
    private List<ExternalOriginatorManagementData> externalOrgsList;
    private bool isAdmin;
    private PipeLineExtOrgInfo pipeLineExtOrgInfo;
    private ExternalOriginatorManagementData selectedOrg;
    public ToolStripMenuItem exportFannieMae;
    private FeaturesAclManager aclMgr;
    private string enableCustomLoanDelieveryFlow;
    private static bool canSearchArchiveFolders = Session.UserInfo.IsAdministrator() || !Session.ACL.IsAuthorizedForFeature(AclFeature.LoanMgmt_SearchArchiveFolders);
    private static bool canAccessArchiveFolders = Session.UserInfo.IsAdministrator() || Session.ACL.IsAuthorizedForFeature(AclFeature.LoanMgmt_AccessToArchiveFolders);
    private bool canUserSearchInArchiveFolders = !Session.StartupInfo.EnableLoanSoftArchival && PipelineScreen.canSearchArchiveFolders;
    private bool canIncludeArchiveLoans;
    private bool clearCurrentFolder = true;
    private bool displayingSubmissionStatus;
    private QueryCriterion q_Filter;
    private SortField[] p_sortField;
    private bool hasManagePipelineViewRights = Session.ACL.IsAuthorizedForFeature(AclFeature.LoanMgmt_CreatePipelineViews);
    private AIQButtonHelper aiqHelper;
    private int[] personaIDs;
    private IList<LoanFolderInfo> activeFolders;
    private IList<LoanFolderInfo> archiveFolders;
    private IList<LoanFolderInfo> trashFolders;
    private IDictionary<string, int> map;
    private const string allActive = "<Select All Active Folders>";
    private const string allArchive = "<Select All Archive Folders>";
    private ToolTip buttonToolTip;
    private string currentViewFolders = "";
    private bool folderSelectedChanged;
    private int folderCounts;
    private const int MOUSEEVENT_LEFTDOWN = 2;
    private const int MOUSEEVENT_LEFTUP = 4;
    private bool errorMsgDisplayed;
    private int loanTypeselected = -1;
    private bool selectLoanOnly;
    private IContainer components;
    private GradientPanel gradientPanel1;
    private ComboBoxEx cboView;
    private Label label1;
    private VerticalSeparator verticalSeparator1;
    private StandardIconButton btnPrint;
    private StandardIconButton btnExport;
    private StandardIconButton btnRefreshView;
    private StandardIconButton btnManageViews;
    private StandardIconButton btnSaveView;
    private GradientPanel gradientPanel2;
    private GradientPanel gradientPanel3;
    private GroupContainer groupContainer1;
    private Label label4;
    private Button btnAdvSearch;
    private PageListNavigator navPipeline;
    private Button btnTransfer;
    private Button btnMove;
    private StandardIconButton btnDelete;
    private StandardIconButton btnDuplicate;
    private StandardIconButton btnEdit;
    private StandardIconButton btnNew;
    private GridView gvLoans;
    private ToolTip toolTips;
    private FlowLayoutPanel flowLayoutPanel1;
    private Button btnClearSearch;
    private ContextMenuStrip mnuLoans;
    private ToolStripMenuItem mnuOpen;
    private ToolStripMenuItem mnuDuplicate;
    private ToolStripMenuItem mnuDelete;
    private ToolStripMenuItem mnuPrint;
    private ToolStripMenuItem mnuManageAlerts;
    private ToolStripMenuItem mnuMove;
    private ToolStripMenuItem mnuTransfer;
    private ToolStripSeparator mnuDivPrint;
    private ToolStripSeparator mnuDivServices;
    private ToolStripMenuItem mnuLoanProperties;
    private StandardIconButton btnRefresh;
    private ElementControl elmMailbox;
    private Label lblFilter;
    private ToolStripMenuItem mnuNew;
    private ToolStripSeparator mnuDivNewEdit;
    private ToolStripSeparator mnuDivMoveDelete;
    private ToolStripMenuItem mnuExport;
    private ToolStripMenuItem mnuSelectAll;
    private Timer tmrRefresh;
    private IconButton btnLoanMailbox;
    private ToolStripMenuItem mnuRefresh;
    private ToolStripMenuItem mnuExportSelected;
    private ToolStripMenuItem mnuExportAll;
    private ToolStripMenuItem mnuEFolderDocuments;
    private ToolStripMenuItem mnuSendFiles;
    private ToolStripMenuItem mnuSendFilesToLender;
    private ToolStripMenuItem mnuExportDocuments;
    private ToolStripMenuItem mnuExportDocsSelectedLoans;
    private ToolStripMenuItem mnuEFolderexportToExcel;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripMenuItem mnuExportDocsAllLoans;
    private ToolStripMenuItem mnuMenuItemOpenWebView;
    private ToolStripMenuItem mnuOpenWebViewLoan;
    private ToolStripMenuItem mnuOpportunities;
    private ToolStripMenuItem mnuProspects;
    private ToolStripSeparator toolStripSeparatorOpenWebView;
    private Button btnNotifyUsers;
    private ToolStripMenuItem toolStripMenuItem1;
    private Button btnSubmissionStatus;
    private Button btn_LaunchAIQ;
    private Button btn_eSignPackages;
    private FlowLayoutPanel pnlAllFolderSelection;
    private Panel panel1;
    private CheckedComboBox cboFolder;
    private Panel pnlglobalsearch;
    private PictureBox pictureBox1;
    private RadioButton rbGlobalSearchOff;
    private RadioButton rbGlobalSearchOn;
    private Label label5;
    private Panel panel3;
    private ComboBox cboLoanType;
    private Label lblCompany;
    private ComboBox cboCompany;
    private TextBox textBox1;
    private StandardIconButton popUpButton;
    private Panel panel2;
    private CheckBox chkarchive;
    private Label label2;
    private Label label3;

    public bool EnableCustomLoanDeliveryFlow
    {
      get
      {
        if (string.IsNullOrWhiteSpace(this.enableCustomLoanDelieveryFlow))
          this.enableCustomLoanDelieveryFlow = SmartClientUtils.GetAttribute(Session.CompanyInfo.ClientID, "Encompass.exe", "IC_CustomLoanDeliveryFlow");
        return !string.IsNullOrWhiteSpace(this.enableCustomLoanDelieveryFlow) && !(this.enableCustomLoanDelieveryFlow == "0");
      }
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    public static extern void mouse_event(
      uint dwFlags,
      uint dx,
      uint dy,
      uint cButtons,
      uint dwExtraInfo);

    private void DoMouseClick()
    {
      PipelineScreen.mouse_event(6U, (uint) (Cursor.Position.X - 25), (uint) (Cursor.Position.Y + 25), 0U, 0U);
    }

    private void setAllFoldersCheckState(
      IList<LoanFolderInfo> folders,
      CheckState state,
      string folderCategory = "")
    {
      if (!string.IsNullOrEmpty(folderCategory) && this.map.ContainsKey(folderCategory))
        ClientCommonUtils.CheckLoanFolder(this.cboFolder, folderCategory, state);
      foreach (LoanFolderInfo folder in (IEnumerable<LoanFolderInfo>) folders)
        ClientCommonUtils.CheckLoanFolder(this.cboFolder, folder.Name, state);
    }

    private bool areAllTrashfolderChecked()
    {
      foreach (LoanFolderInfo trashFolder in (IEnumerable<LoanFolderInfo>) this.trashFolders)
      {
        if (this.cboFolder.ItemList.GetItemCheckState(this.map[trashFolder.Name]) == CheckState.Unchecked)
          return false;
      }
      return true;
    }

    private bool isRegularFolderSelected()
    {
      foreach (ComboBoxItem checkedItem in this.cboFolder.ItemList.CheckedItems)
      {
        LoanFolderInfo tag = (LoanFolderInfo) checkedItem.Tag;
        if (tag.Type == LoanFolderInfo.LoanFolderType.Regular || tag.Type == LoanFolderInfo.LoanFolderType.Archive)
          return true;
      }
      return false;
    }

    private bool isTrashFolderSelected()
    {
      foreach (ComboBoxItem checkedItem in this.cboFolder.ItemList.CheckedItems)
      {
        if (((LoanFolderInfo) checkedItem.Tag).Type == LoanFolderInfo.LoanFolderType.Trash)
          return true;
      }
      return false;
    }

    private bool isArchiveFolderSelected()
    {
      foreach (ComboBoxItem checkedItem in this.cboFolder.ItemList.CheckedItems)
      {
        if (((LoanFolderInfo) checkedItem.Tag).Type == LoanFolderInfo.LoanFolderType.Archive)
          return true;
      }
      return false;
    }

    private void classifyFolders(LoanFolderInfo[] folders)
    {
      this.activeFolders.Clear();
      this.archiveFolders.Clear();
      this.trashFolders.Clear();
      foreach (LoanFolderInfo folder in folders)
      {
        if (folder.Type == LoanFolderInfo.LoanFolderType.Regular)
          this.activeFolders.Add(folder);
        else if (folder.Type == LoanFolderInfo.LoanFolderType.Archive)
          this.archiveFolders.Add(folder);
        else if (folder.Type == LoanFolderInfo.LoanFolderType.Trash)
          this.trashFolders.Add(folder);
      }
    }

    private void loadAllFolders(LoanFolderInfo[] folders)
    {
      int num = 0;
      this.cboFolder.Items.Clear();
      if (this.activeFolders.Count > 0 || this.canUserSearchInArchiveFolders && this.archiveFolders.Count > 0)
      {
        LoanFolderInfo tag = new LoanFolderInfo(SystemSettings.AllFolders);
        ComboBoxItem comboBoxItem = new ComboBoxItem(tag.Name, tag.DisplayName, (object) tag);
        this.cboFolder.Items.Add((object) comboBoxItem);
        if (!this.map.ContainsKey(comboBoxItem.Name))
          this.map.Add(comboBoxItem.Name, num);
        ++num;
      }
      if (this.activeFolders.Count > 0)
      {
        LoanFolderInfo tag = new LoanFolderInfo("<Select All Active Folders>");
        ComboBoxItem comboBoxItem = new ComboBoxItem(tag.Name, tag.DisplayName, (object) tag);
        this.cboFolder.Items.Add((object) comboBoxItem);
        if (!this.map.ContainsKey(comboBoxItem.Name))
          this.map.Add(comboBoxItem.Name, num);
        ++num;
      }
      if (this.canUserSearchInArchiveFolders && this.archiveFolders.Count > 0)
      {
        LoanFolderInfo tag = new LoanFolderInfo("<Select All Archive Folders>");
        ComboBoxItem comboBoxItem = new ComboBoxItem(tag.Name, tag.DisplayName, (object) tag);
        this.cboFolder.Items.Add((object) comboBoxItem);
        if (!this.map.ContainsKey(comboBoxItem.Name))
          this.map.Add(comboBoxItem.Name, num);
        ++num;
      }
      foreach (LoanFolderInfo folder in folders)
      {
        ComboBoxItem comboBoxItem = new ComboBoxItem(folder.Name, folder.DisplayName, (object) folder);
        if (folder.Type == LoanFolderInfo.LoanFolderType.Trash || folder.Type == LoanFolderInfo.LoanFolderType.Regular || folder.Type == LoanFolderInfo.LoanFolderType.Archive && this.canUserSearchInArchiveFolders)
        {
          this.cboFolder.Items.Add((object) comboBoxItem);
          if (!this.map.ContainsKey(comboBoxItem.Name))
            this.map.Add(folder.Name, num);
          ++num;
        }
      }
    }

    private void selectAllFolders()
    {
      if (this.activeFolders.Count > 0 || this.archiveFolders.Count > 0)
        ClientCommonUtils.CheckLoanFolder(this.cboFolder, SystemSettings.AllFolders, CheckState.Checked);
      if (this.activeFolders.Count > 0)
        this.setAllFoldersCheckState(this.activeFolders, CheckState.Checked, "<Select All Active Folders>");
      if (this.archiveFolders.Count > 0)
        this.setAllFoldersCheckState(this.archiveFolders, CheckState.Checked, "<Select All Archive Folders>");
      this.setAllFoldersCheckState(this.trashFolders, CheckState.Unchecked);
      int num = this.activeFolders.Count + (this.canUserSearchInArchiveFolders ? this.archiveFolders.Count : 0);
      if (num == 1)
        this.cboFolder.ComboBoxText = this.activeFolders.Count == 0 ? this.archiveFolders[0].Name : this.activeFolders[0].Name;
      else
        this.cboFolder.ComboBoxText = num.ToString() + " folders are selected";
    }

    private void selectFoldersFromFolderList(string[] folderList)
    {
      int num = 0;
      int index = 0;
      ClientCommonUtils.UncheckLoanFolders(this.cboFolder, this.getSelectedFolderListItem());
      foreach (string folder in folderList)
      {
        if (this.map.ContainsKey(folder))
        {
          ClientCommonUtils.CheckLoanFolder(this.cboFolder, folder, CheckState.Checked);
          if (!folder.Equals(SystemSettings.AllFolders) && !folder.Equals("<Select All Active Folders>") && !folder.Equals("<Select All Archive Folders>"))
            ++num;
          else
            ++index;
        }
        else
          ++index;
      }
      if (num <= 0)
        return;
      this.cboFolder.ComboBoxText = num == 1 ? folderList[index] : num.ToString() + " folders are selected";
    }

    private string getSelectedFolderList()
    {
      if (string.IsNullOrEmpty(this.cboFolder.ComboBoxText) && this.cboFolder.ItemList.CheckedItems.Count == 0)
        return "";
      StringBuilder stringBuilder = new StringBuilder();
      foreach (object checkedItem in this.cboFolder.ItemList.CheckedItems)
      {
        stringBuilder.Append(((ComboBoxItem) checkedItem).Name);
        stringBuilder.Append("|");
      }
      if (stringBuilder.Length > 0)
        stringBuilder.Remove(stringBuilder.Length - 1, 1);
      return stringBuilder.ToString();
    }

    private List<ComboBoxItem> getSelectedFolderListItem()
    {
      List<ComboBoxItem> selectedFolderListItem = new List<ComboBoxItem>();
      if (string.IsNullOrEmpty(this.cboFolder.ComboBoxText) && this.cboFolder.ItemList.CheckedItems.Count == 0)
        return selectedFolderListItem;
      foreach (object checkedItem in this.cboFolder.ItemList.CheckedItems)
        selectedFolderListItem.Add((ComboBoxItem) checkedItem);
      return selectedFolderListItem;
    }

    private string getFolderList()
    {
      if (this.cboFolder.ItemList.Items.Count == 0)
        return "";
      StringBuilder stringBuilder = new StringBuilder();
      foreach (object obj in (ListBox.ObjectCollection) this.cboFolder.ItemList.Items)
      {
        stringBuilder.Append(((ComboBoxItem) obj).Name);
        stringBuilder.Append("|");
      }
      if (stringBuilder.Length > 0)
        stringBuilder.Remove(stringBuilder.Length - 1, 1);
      return stringBuilder.ToString();
    }

    private void loadViewFolders(string folderName)
    {
      if (string.IsNullOrEmpty(folderName))
        return;
      string[] folderList = folderName.Split('|');
      if (folderList[0].Equals(SystemSettings.AllFolders))
      {
        this.selectAllFolders();
      }
      else
      {
        this.selectFoldersFromFolderList(folderList);
        if (this.cboFolder.ItemList.CheckedItems.Count != 0)
          return;
        this.selectAllFolders();
      }
    }

    private void cboFolder_DropDown(object sender, EventArgs e)
    {
    }

    [DllImport("ntdll.dll", SetLastError = true)]
    internal static extern uint RtlGetVersion(
      out PipelineScreen.OsVersionInfo versionInformation);

    private void cboFolder_DropDownClosed(object sender, EventArgs e)
    {
      if (this.cboFolder.Display && !this.cboFolder.ForceRefresh)
        return;
      if (!(e is CCBoxEventArgs ccBoxEventArgs))
        ccBoxEventArgs = new CCBoxEventArgs(false, e);
      if (ccBoxEventArgs.AssignValues)
      {
        switch (this.cboFolder.ItemList.CheckedItems.Count)
        {
          case 0:
            int num = (int) Utils.Dialog((IWin32Window) this, "Please select at least one item from the list.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            this.cboFolder.Display = true;
            return;
          case 1:
            this.cboFolder.Text = ((LoanFolderInfo) ((ComboBoxItem) this.cboFolder.ItemList.CheckedItems[0]).Tag).Name;
            break;
        }
        this.currentViewFolders = this.getSelectedFolderList();
        this.loadViewFolders(this.currentViewFolders);
        this.btnMove.Text = this.isRegularFolderSelected() || !this.isTrashFolderSelected() ? "&Move to Folder" : "&Restore to Folder";
        this.btnNew.Enabled = this.CanOriginateLoans();
        this.applySpecialFolderSecurity();
        this.setButtonEnabledStates();
        this.showAlertPerformanceMessage();
        this.RefreshPipeline(false, 1);
        this.setViewChanged(this.folderSelectedChanged);
        this.folderSelectedChanged = false;
      }
      else
      {
        foreach (KeyValuePair<string, int> keyValuePair in (IEnumerable<KeyValuePair<string, int>>) this.map)
          ClientCommonUtils.CheckLoanFolder(this.cboFolder, keyValuePair.Key, CheckState.Unchecked);
        this.loadViewFolders(this.currentViewFolders);
      }
    }

    public bool CanOriginateLoans()
    {
      bool flag = false;
      string name = string.Empty;
      if (this.cboFolder.Items.Count > 0)
        name = this.cboFolder.ComboBoxText;
      if (!this.isMultiFolderSelected() && !string.IsNullOrEmpty(name) && !name.Equals(SystemSettings.TrashFolder))
        flag = this.getLoanFolderRule(new LoanFolderInfo(name)).CanOriginateLoans;
      return flag;
    }

    private void showAlertPerformanceMessage()
    {
      if (PipelineScreen.alreadyDisplayedAlertWarningMessage)
        return;
      try
      {
        if ((this.cboFolder.ItemList.CheckedItems.Count <= 0 || !this.cboFolder.ItemList.GetItemChecked(this.map[SystemSettings.AllFolders])) && !this.isMultiFolderSelected() && !this.isArchiveFolderSelected() && !this.isTrashFolderSelected())
          return;
        PipelineScreen.alreadyDisplayedAlertWarningMessage = true;
        int num = (int) Utils.Dialog((IWin32Window) this, "To help ensure optimum Pipeline performance, loan alert details are not provided for multiple folders, Archive folders, or Trash folder views.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      catch
      {
      }
    }

    private void cboFolder_ItemCheck(object sender, ItemCheckEventArgs e)
    {
      if (!this.cboFolder.Display)
        return;
      bool flag1 = !this.map.ContainsKey("<Select All Active Folders>") || this.cboFolder.ItemList.GetItemChecked(this.map["<Select All Active Folders>"]);
      bool flag2 = !this.map.ContainsKey("<Select All Archive Folders>") || this.cboFolder.ItemList.GetItemChecked(this.map["<Select All Archive Folders>"]);
      bool flag3 = this.areAllTrashfolderChecked();
      bool flag4 = true;
      this.cboFolder.ItemList.ItemCheck -= new ItemCheckEventHandler(this.cboFolder_ItemCheck);
      string name = ((ComboBoxItem) this.cboFolder.Items[e.Index]).Name;
      if (name.Equals(SystemSettings.AllFolders))
      {
        if (e.NewValue == CheckState.Checked)
        {
          if (this.activeFolders.Count > 0)
            this.setAllFoldersCheckState(this.activeFolders, CheckState.Checked, "<Select All Active Folders>");
          if (this.canUserSearchInArchiveFolders && this.archiveFolders.Count > 0)
            this.setAllFoldersCheckState(this.archiveFolders, CheckState.Checked, "<Select All Archive Folders>");
          if (this.trashFolders.Count > 0)
            this.setAllFoldersCheckState(this.trashFolders, CheckState.Unchecked);
          flag1 = true;
          flag2 = true;
          flag4 = true;
        }
        else if (e.NewValue == CheckState.Unchecked)
        {
          if (this.activeFolders.Count > 0)
            this.setAllFoldersCheckState(this.activeFolders, CheckState.Unchecked, "<Select All Active Folders>");
          if (this.canUserSearchInArchiveFolders && this.archiveFolders.Count > 0)
            this.setAllFoldersCheckState(this.archiveFolders, CheckState.Unchecked, "<Select All Archive Folders>");
          flag1 = false;
          flag2 = false;
          flag4 = false;
        }
      }
      else
      {
        switch (name)
        {
          case "<Select All Active Folders>":
            if (e.NewValue == CheckState.Checked)
            {
              this.setAllFoldersCheckState(this.activeFolders, CheckState.Checked);
              flag1 = true;
              flag4 = true;
              break;
            }
            if (e.NewValue == CheckState.Unchecked)
            {
              this.setAllFoldersCheckState(this.activeFolders, CheckState.Unchecked);
              flag1 = false;
              flag4 = false;
              foreach (LoanFolderInfo trashFolder in (IEnumerable<LoanFolderInfo>) this.trashFolders)
              {
                if (this.map.ContainsKey(trashFolder.Name) && this.cboFolder.ItemList.GetItemChecked(this.map[trashFolder.Name]))
                {
                  flag4 = true;
                  break;
                }
              }
              using (IEnumerator<LoanFolderInfo> enumerator = this.archiveFolders.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  LoanFolderInfo current = enumerator.Current;
                  if (this.map.ContainsKey(current.Name) && this.cboFolder.ItemList.GetItemChecked(this.map[current.Name]))
                  {
                    flag4 = true;
                    break;
                  }
                }
                break;
              }
            }
            else
              break;
          case "<Select All Archive Folders>":
            if (e.NewValue == CheckState.Checked)
            {
              this.setAllFoldersCheckState(this.archiveFolders, CheckState.Checked);
              flag2 = true;
              flag4 = true;
              break;
            }
            if (e.NewValue == CheckState.Unchecked)
            {
              this.setAllFoldersCheckState(this.archiveFolders, CheckState.Unchecked);
              flag2 = false;
              flag4 = false;
              foreach (LoanFolderInfo trashFolder in (IEnumerable<LoanFolderInfo>) this.trashFolders)
              {
                if (this.map.ContainsKey(trashFolder.Name) && this.cboFolder.ItemList.GetItemChecked(this.map[trashFolder.Name]))
                {
                  flag4 = true;
                  break;
                }
              }
              using (IEnumerator<LoanFolderInfo> enumerator = this.activeFolders.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  LoanFolderInfo current = enumerator.Current;
                  if (this.map.ContainsKey(current.Name) && this.cboFolder.ItemList.GetItemChecked(this.map[current.Name]))
                  {
                    flag4 = true;
                    break;
                  }
                }
                break;
              }
            }
            else
              break;
          default:
            LoanFolderInfo tag = (LoanFolderInfo) ((ComboBoxItem) this.cboFolder.Items[e.Index]).Tag;
            if (tag.Type == LoanFolderInfo.LoanFolderType.Regular)
            {
              if (e.NewValue == CheckState.Unchecked)
              {
                ClientCommonUtils.CheckLoanFolder(this.cboFolder, "<Select All Active Folders>", CheckState.Unchecked);
                flag1 = false;
                flag4 = false;
                foreach (LoanFolderInfo archiveFolder in (IEnumerable<LoanFolderInfo>) this.archiveFolders)
                {
                  if (this.map.ContainsKey(archiveFolder.Name) && this.cboFolder.ItemList.GetItemChecked(this.map[archiveFolder.Name]))
                  {
                    flag4 = true;
                    break;
                  }
                }
                foreach (LoanFolderInfo activeFolder in (IEnumerable<LoanFolderInfo>) this.activeFolders)
                {
                  if (!activeFolder.Name.Equals(tag.Name) && this.cboFolder.ItemList.GetItemChecked(this.map[activeFolder.Name]))
                  {
                    flag4 = true;
                    break;
                  }
                }
                using (IEnumerator<LoanFolderInfo> enumerator = this.trashFolders.GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    LoanFolderInfo current = enumerator.Current;
                    if (this.map.ContainsKey(current.Name) && this.cboFolder.ItemList.GetItemChecked(this.map[current.Name]))
                    {
                      flag4 = true;
                      break;
                    }
                  }
                  break;
                }
              }
              else if (e.NewValue == CheckState.Checked)
              {
                flag4 = true;
                using (IEnumerator<LoanFolderInfo> enumerator = this.activeFolders.GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    LoanFolderInfo current = enumerator.Current;
                    ClientCommonUtils.CheckLoanFolder(this.cboFolder, "<Select All Active Folders>", CheckState.Checked);
                    flag1 = true;
                    if (!current.Name.Equals(tag.Name) && !this.cboFolder.ItemList.GetItemChecked(this.map[current.Name]))
                    {
                      ClientCommonUtils.CheckLoanFolder(this.cboFolder, "<Select All Active Folders>", CheckState.Unchecked);
                      flag1 = false;
                      break;
                    }
                  }
                  break;
                }
              }
              else
                break;
            }
            else if (tag.Type == LoanFolderInfo.LoanFolderType.Archive)
            {
              if (e.NewValue == CheckState.Unchecked)
              {
                ClientCommonUtils.CheckLoanFolder(this.cboFolder, "<Select All Archive Folders>", CheckState.Unchecked);
                flag2 = false;
                flag4 = false;
                foreach (LoanFolderInfo archiveFolder in (IEnumerable<LoanFolderInfo>) this.archiveFolders)
                {
                  if (!archiveFolder.Name.Equals(tag.Name) && this.cboFolder.ItemList.GetItemChecked(this.map[archiveFolder.Name]))
                  {
                    flag4 = true;
                    break;
                  }
                }
                foreach (LoanFolderInfo activeFolder in (IEnumerable<LoanFolderInfo>) this.activeFolders)
                {
                  if (this.map.ContainsKey(activeFolder.Name) && this.cboFolder.ItemList.GetItemChecked(this.map[activeFolder.Name]))
                  {
                    flag4 = true;
                    break;
                  }
                }
                using (IEnumerator<LoanFolderInfo> enumerator = this.trashFolders.GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    LoanFolderInfo current = enumerator.Current;
                    if (this.map.ContainsKey(current.Name) && this.cboFolder.ItemList.GetItemChecked(this.map[current.Name]))
                    {
                      flag4 = true;
                      break;
                    }
                  }
                  break;
                }
              }
              else if (e.NewValue == CheckState.Checked)
              {
                flag4 = true;
                using (IEnumerator<LoanFolderInfo> enumerator = this.archiveFolders.GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    LoanFolderInfo current = enumerator.Current;
                    ClientCommonUtils.CheckLoanFolder(this.cboFolder, "<Select All Archive Folders>", CheckState.Checked);
                    flag2 = true;
                    if (!current.Name.Equals(tag.Name) && !this.cboFolder.ItemList.GetItemChecked(this.map[current.Name]))
                    {
                      ClientCommonUtils.CheckLoanFolder(this.cboFolder, "<Select All Archive Folders>", CheckState.Unchecked);
                      flag2 = false;
                      break;
                    }
                  }
                  break;
                }
              }
              else
                break;
            }
            else if (tag.Type == LoanFolderInfo.LoanFolderType.Trash)
            {
              if (e.NewValue == CheckState.Unchecked)
              {
                flag3 = false;
                flag4 = false;
                foreach (LoanFolderInfo archiveFolder in (IEnumerable<LoanFolderInfo>) this.archiveFolders)
                {
                  if (this.map.ContainsKey(archiveFolder.Name) && this.cboFolder.ItemList.GetItemChecked(this.map[archiveFolder.Name]))
                  {
                    flag4 = true;
                    break;
                  }
                }
                using (IEnumerator<LoanFolderInfo> enumerator = this.activeFolders.GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    LoanFolderInfo current = enumerator.Current;
                    if (this.map.ContainsKey(current.Name) && this.cboFolder.ItemList.GetItemChecked(this.map[current.Name]))
                    {
                      flag4 = true;
                      break;
                    }
                  }
                  break;
                }
              }
              else if (e.NewValue == CheckState.Checked)
              {
                flag3 = true;
                flag4 = true;
                using (IEnumerator<LoanFolderInfo> enumerator = this.trashFolders.GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    LoanFolderInfo current = enumerator.Current;
                    if (!current.Name.Equals(tag.Name) && !this.cboFolder.ItemList.GetItemChecked(this.map[current.Name]))
                    {
                      flag3 = false;
                      break;
                    }
                  }
                  break;
                }
              }
              else
                break;
            }
            else
              break;
        }
      }
      CheckState checkState = flag1 & flag2 ? CheckState.Checked : CheckState.Unchecked;
      ClientCommonUtils.CheckLoanFolder(this.cboFolder, SystemSettings.AllFolders, checkState);
      this.cboFolder.OKBtn.Enabled = flag4;
      this.cboFolder.ItemList.ItemCheck += new ItemCheckEventHandler(this.cboFolder_ItemCheck);
      this.folderSelectedChanged = true;
    }

    private void cboFolder_MouseHover(object sender, EventArgs e)
    {
      this.buttonToolTip.ToolTipTitle = "Selected Folders:";
      this.buttonToolTip.SetToolTip((Control) this.cboFolder, this.getSelectedFolderList());
    }

    private bool isMultiFolderSelected()
    {
      int num = 0;
      for (int index = 0; index < this.cboFolder.Items.Count; ++index)
      {
        if (num > 1)
          return true;
        LoanFolderInfo tag = (LoanFolderInfo) ((ComboBoxItem) this.cboFolder.Items[index]).Tag;
        if (!tag.Name.Equals(SystemSettings.AllFolders) && !tag.Name.Equals("<Select All Active Folders>") && !tag.Name.Equals("<Select All Archive Folders>") && this.cboFolder.ItemList.GetItemCheckState(index) == CheckState.Checked)
          ++num;
      }
      return num > 1;
    }

    private bool GridHasAnyGlobalSearchFields
    {
      get
      {
        return this.gvLoans.Columns.Select<GVColumn, string>((Func<GVColumn, string>) (n => n.Text)).Any<string>((Func<string, bool>) (f => Utils.GlobalSearchFields.Contains(f)));
      }
    }

    public PipelineScreen()
    {
      this.InitializeComponent();
      try
      {
        PipelineScreen.OsVersionInfo versionInformation = new PipelineScreen.OsVersionInfo();
        if (PipelineScreen.RtlGetVersion(out versionInformation) == 0U)
          this._majorOsVersion = versionInformation.MajorVersion;
      }
      catch
      {
      }
      using (MetricsFactory.GetIncrementalTimer("PipelineRefreshIncTimer", (SFxTag) new SFxUiTag()))
      {
        using (PerformanceMeter.StartNew("Pipeline.Load.CheckPoint1", "Pipeline screen constructor", false, 826, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\PipelineScreen.cs"))
        {
          this.aclMgr = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
          Session.Application.RegisterService((object) this, typeof (IPipeline));
          if (!Session.SessionObjects.StartupInfo.FastLoanLoad)
            Task.Run<CompiledTriggers>((Func<CompiledTriggers>) (() => TriggerCache.GetTriggers(Session.SessionObjects)));
          using (PerformanceMeter.StartNew("Pipeline.Load.CheckPoint2", "Investor Connect Initialization", false, 836, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\PipelineScreen.cs"))
          {
            if (MainScreen.initializeInvestorServicesTask != null)
              MainScreen.InitializeInvesterServicesSettings = MainScreen.initializeInvestorServicesTask.Result;
          }
          EPassMessages.MessageActivity += new EPassMessageEventHandler(this.onEPassMessageActivity);
          this.elmMailbox.Element = (object) new AlertMessageLink(AlertMessageLabel.AlertMessageStyle.Message, "", new EventHandler(this.onOpenLoanMailbox));
          this.populateServicesMenuItem();
          this.cboCompany.Visible = Session.EncompassEdition == EncompassEdition.Banker;
          this.textBox1.Visible = Session.EncompassEdition == EncompassEdition.Banker;
          this.popUpButton.Visible = Session.EncompassEdition == EncompassEdition.Banker;
          this.lblCompany.Visible = Session.EncompassEdition == EncompassEdition.Banker;
          FeatureConfigsAclManager aclManager = (FeatureConfigsAclManager) Session.ACL.GetAclManager(AclCategory.FeatureConfigs);
          this.mnuMenuItemOpenWebView.Visible = this.toolStripSeparatorOpenWebView.Visible = Session.EncompassEdition == EncompassEdition.Banker && aclManager.GetUserApplicationRight(AclFeature.PlatForm_Access) > 0;
          if (!Session.UserInfo.IsSuperAdministrator() && !this.aclMgr.GetUserApplicationRight(AclFeature.LoanMgmt_Export_FannieMae_FormattedFile))
          {
            if (this.exportFannieMae != null)
              this.exportFannieMae.Visible = false;
            this.toolStripMenuItem1.Visible = false;
          }
          this.btn_eSignPackages.Visible = this.aclMgr.GetUserApplicationRight(AclFeature.eFolder_Other_PackagesTab) && this.aclMgr.GetUserApplicationRight(AclFeature.eFolder_Other_eSignPackages);
          if (this.btn_eSignPackages.Visible)
            this.btn_LaunchAIQ.Left -= this.btn_eSignPackages.Width;
          this.btnSubmissionStatus.Visible = MainScreen.AllowDataAndDocs && this.aclMgr.GetUserApplicationRight(AclFeature.LoanMgmt_Investor_Service) || MainScreen.AllowWarehouseLenders && this.aclMgr.GetUserApplicationRight(AclFeature.LoanMgmt_Investor_Warehouse_Lenders) || MainScreen.AllowDueDiligenceServices && this.aclMgr.GetUserApplicationRight(AclFeature.LoanMgmt_Investor_Due_Diligence) || MainScreen.AllowHedgeAdvisoryServices && this.aclMgr.GetUserApplicationRight(AclFeature.LoanMgmt_Investor_Hedge_Advisory) || MainScreen.AllowSubservicingServices && this.aclMgr.GetUserApplicationRight(AclFeature.LoanMgmt_Investor_Subservicing_Services) || MainScreen.AllowBidTapServices && this.aclMgr.GetUserApplicationRight(AclFeature.LoanMgmt_Investor_Bid_Tape_Services) || MainScreen.AllowQCAuditServices && this.aclMgr.GetUserApplicationRight(AclFeature.LoanMgmt_Investor_QC_Audit_Services) || MainScreen.AllowWholesaleLenderServices && this.aclMgr.GetUserApplicationRight(AclFeature.LoanMgmt_Investor_Wholesale_Lender_Services) || MainScreen.AllowServicingServices && this.aclMgr.GetUserApplicationRight(AclFeature.LoanMgmt_Investor_Servicing_Services);
          if (!this.btnSubmissionStatus.Visible)
          {
            this.btn_LaunchAIQ.Left += this.btnSubmissionStatus.Width;
            this.btn_eSignPackages.Left += this.btnSubmissionStatus.Width;
          }
          this.activeFolders = (IList<LoanFolderInfo>) new List<LoanFolderInfo>();
          this.archiveFolders = (IList<LoanFolderInfo>) new List<LoanFolderInfo>();
          this.trashFolders = (IList<LoanFolderInfo>) new List<LoanFolderInfo>();
          this.map = (IDictionary<string, int>) new Dictionary<string, int>();
          this.buttonToolTip = new ToolTip();
          this.personaIDs = Session.UserInfo.GetPersonaIDs();
          this.canIncludeArchiveLoans = Session.UserInfo.IsAdministrator() || this.aclMgr.GetUserApplicationRight(AclFeature.LoanMgmt_AccessToArchiveLoans);
          this.chkarchive.Enabled = Session.StartupInfo.EnableLoanSoftArchival && this.canIncludeArchiveLoans;
          this.aiqHelper = new AIQButtonHelper((IWin32Window) this, this.btn_LaunchAIQ);
          if (Session.StartupInfo.EnableLoanSoftArchival)
            return;
          this.panel2.Visible = false;
        }
      }
    }

    private void loadTPOSettings()
    {
      bool flag1 = false;
      bool flag2 = false;
      if (Session.StartupInfo.UserAclFeatureRights.Contains((object) AclFeature.ExternalSettings_ContactSalesRep) && Session.StartupInfo.UserAclFeatureRights.Contains((object) AclFeature.ExternalSettings_OrganizationSettings))
        flag1 = !(bool) Session.StartupInfo.UserAclFeatureRights[(object) AclFeature.ExternalSettings_ContactSalesRep] && (bool) Session.StartupInfo.UserAclFeatureRights[(object) AclFeature.ExternalSettings_OrganizationSettings];
      AclGroup[] groupsOfUser = Session.AclGroupManager.GetGroupsOfUser(Session.UserID);
      if (groupsOfUser != null && groupsOfUser.Length != 0)
        flag2 = ((IEnumerable<AclGroup>) groupsOfUser).Any<AclGroup>((Func<AclGroup, bool>) (group => group.Name.ToLower() == "TPO Admins".ToLower()));
      if (Session.UserInfo.IsAdministrator() | flag1 | flag2)
      {
        this.isAdmin = true;
        this.externalOrgsList = Session.ConfigurationManager.GetAllExternalParentOrganizations(false);
        this.externalUsersIds = new string[0];
        this.externalAndInternalUsersIds = new string[0];
      }
      else
      {
        ArrayList andOrgBySalesRep = Session.ConfigurationManager.GetExternalAndInternalUserAndOrgBySalesRep(Session.UserID, Session.UserInfo.OrgId);
        this.externalOrgsList = (List<ExternalOriginatorManagementData>) andOrgBySalesRep[1];
        this.externalAndInternalUsersIds = ((List<string>) andOrgBySalesRep[4]).ToArray();
        this.externalUsersIds = new string[0];
      }
    }

    public void Initialize() => this.Initialize(0);

    public void Initialize(int sqlRead)
    {
      PipelineScreenData pipelineScreenData = (PipelineScreenData) null;
      LoanFolderInfo[] loanFolderInfoArray = (LoanFolderInfo[]) null;
      using (MetricsFactory.GetIncrementalTimer("PipelineRefreshIncTimer", (SFxTag) new SFxUiTag()))
      {
        using (PerformanceMeter.StartNew("Pipeline.Load.CheckPoint4", "Get Field Definitions", false, 957, nameof (Initialize), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\PipelineScreen.cs"))
          this.fieldDefs = LoanReportFieldDefs.GetFieldDefs(Session.DefaultInstance, LoanReportFieldFlags.DatabaseFieldsNoAudit);
      }
      using (MetricsFactory.GetIncrementalTimer("PipelineRefreshIncTimer", (SFxTag) new SFxUiTag()))
      {
        using (PerformanceMeter.StartNew("Pipeline.Load.CheckPoint3", "Get Pipeline Screen Data", false, 965, nameof (Initialize), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\PipelineScreen.cs"))
        {
          this.pipelineViews = PipelineViewListItem.GetPipelineViewForUserPipelineView(Session.LoanManager.GetPipelineData(), (ReportFieldDefContainer) this.fieldDefs, new ReportFieldCommonExtension.GetFieldDefWidth(ReportFieldClientExtension.GetDefaultColumnWidth));
          this.personaPipelineViews = PipelineViewListItem.GetPipelineViewForPersonaPipelineView(Session.LoanManager.GetPersonaPipelineData(), (ReportFieldDefContainer) this.fieldDefs, new ReportFieldCommonExtension.GetFieldDefWidth(ReportFieldClientExtension.GetDefaultColumnWidth));
          this.pipelineViews.AddRange((IEnumerable<PipelineView>) this.personaPipelineViews);
          loanFolderInfoArray = Session.LoanManager.GetAllLoanFolderInfos(true);
          Session.SessionObjects.LoanFolderInfos = loanFolderInfoArray != null ? ((IEnumerable<LoanFolderInfo>) loanFolderInfoArray).ToList<LoanFolderInfo>() : (List<LoanFolderInfo>) null;
        }
      }
      this.loadViewList(pipelineScreenData?.CustomViews);
      this.classifyFolders(loanFolderInfoArray);
      this.loanFolderInfos = loanFolderInfoArray;
      this.folderCounts = ((IEnumerable<LoanFolderInfo>) loanFolderInfoArray).Count<LoanFolderInfo>();
      this.suspendEvents = this.suspendRefresh = true;
      string str = Session.GetPrivateProfileString("Pipeline.DefaultView") ?? "";
      this.suspendEvents = false;
      if (str != "")
      {
        foreach (PipelineViewList pipelineViewList in this.cboView.Items)
        {
          bool flag = false;
          if (pipelineViewList.PipelineView != null)
          {
            if (pipelineViewList.PipelineView.IsUserView)
            {
              int num = str.LastIndexOf('\\');
              if (num > 0 && pipelineViewList.PipelineView.Name == str.Substring(num + 1))
                flag = true;
            }
            else if (pipelineViewList.PipelineView.Name == str)
              flag = true;
            if (flag)
            {
              this.cboView.SelectedItem = (object) pipelineViewList;
              break;
            }
          }
        }
      }
      if (this.cboView.SelectedIndex < 0 && this.cboView.Items.Count > 0)
        this.cboView.SelectedIndex = 0;
      using (MetricsFactory.GetIncrementalTimer("PipelineRefreshIncTimer", (SFxTag) new SFxUiTag()))
      {
        using (PerformanceMeter.StartNew("Pipeline.Load.CheckPoint5", "Load Folders and Views", false, 1019, nameof (Initialize), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\PipelineScreen.cs"))
        {
          this.loadAllFolders(loanFolderInfoArray);
          this.selectFolders(loanFolderInfoArray);
        }
      }
      this.applyUserAccessSecurity();
      this.refreshLoanMailboxNotification();
      this.suspendRefresh = false;
      this.RefreshPipeline(false, sqlRead);
      this.UpdateGlobalSearchControls();
      this.rbGlobalSearchOff.Checked = true;
      if (this.folderCounts > 0)
        return;
      this.btnNew.Enabled = false;
    }

    private void UpdateGlobalSearchControls()
    {
      bool result;
      bool flag = bool.TryParse(Session.SessionObjects.GetCompanySettingFromCache("POLICIES", "UseGlobalSearch"), out result) & result;
      this.pnlglobalsearch.Visible = flag;
      if (!flag)
        return;
      if (this.GridHasAnyGlobalSearchFields)
        this.rbGlobalSearchOn.Enabled = this.rbGlobalSearchOff.Checked = true;
      else
        this.rbGlobalSearchOn.Enabled = false;
    }

    public void RefreshViews(bool keepCurrentSelection)
    {
      PipelineViewList pipelineViewList = (PipelineViewList) null;
      if (keepCurrentSelection)
        pipelineViewList = (PipelineViewList) this.cboView.SelectedItem;
      this.pipelineViews = PipelineViewListItem.GetPipelineViewForUserPipelineView(Session.LoanManager.GetPipelineData(), (ReportFieldDefContainer) this.fieldDefs, new ReportFieldCommonExtension.GetFieldDefWidth(ReportFieldClientExtension.GetDefaultColumnWidth));
      this.pipelineViews.AddRange((IEnumerable<PipelineView>) this.personaPipelineViews);
      this.loadViewList(Session.ConfigurationManager.GetUserAccessibleViews(Session.UserID));
      if (this.cboView.Items.Count <= 0)
        return;
      if (pipelineViewList != null)
      {
        foreach (object obj in this.cboView.Items)
        {
          if (obj.ToString() == pipelineViewList.PipelineView.Name)
          {
            this.cboView.SelectedItem = obj;
            break;
          }
        }
      }
      if (this.cboView.SelectedItem != null || this.cboView.SelectedIndex >= 0)
        return;
      this.cboView.SelectedIndex = 0;
    }

    private void loadViewList(FileSystemEntry[] fsEntries)
    {
      this.suspendEvents = true;
      try
      {
        this.cboView.Items.Clear();
        this.cboView.Dividers.Clear();
        List<PipelineViewList> pipelineViewListList = new List<PipelineViewList>();
        foreach (PipelineView pipelineView in this.pipelineViews)
        {
          if (pipelineView.IsUserView)
            pipelineViewListList.Add(new PipelineViewList(pipelineView));
        }
        pipelineViewListList.Sort();
        foreach (PipelineView pipelineView in this.pipelineViews)
        {
          if (!pipelineView.IsUserView)
            pipelineViewListList.Add(new PipelineViewList(pipelineView));
        }
        foreach (PipelineViewList pipelineViewList in pipelineViewListList)
        {
          if (pipelineViewList.PipelineView.IsUserView)
            this.cboView.Items.Add((object) pipelineViewList);
        }
        this.cboView.Dividers.Add(this.cboView.Items.Count);
        foreach (PipelineViewList pipelineViewList in pipelineViewListList)
        {
          if (!pipelineViewList.PipelineView.IsUserView)
            this.cboView.Items.Add((object) pipelineViewList);
        }
        if (this.currentView == null)
          return;
        foreach (object obj in this.cboView.Items)
        {
          if (obj.ToString() == this.currentView.Name)
          {
            this.cboView.SelectedItem = obj;
            break;
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(PipelineScreen.sw, nameof (PipelineScreen), TraceLevel.Error, "Error loading view list: " + (object) ex);
        ErrorDialog.Display(ex);
      }
      finally
      {
        this.suspendEvents = false;
      }
    }

    private void loadCompanyDropDown()
    {
      if (this.cboCompany.Items.Count != 0)
        return;
      this.cboCompany.Items.Add((object) "Internal");
      if (Session.EncompassEdition == EncompassEdition.Broker)
        return;
      this.cboCompany.Items.Add((object) "TPO");
    }

    public void InvalidatePipeline()
    {
      if (this.tmrRefresh.Enabled)
        this.RefreshPipeline(true);
      else
        this.invalidated = true;
    }

    public void RefreshPipeline() => this.RefreshPipeline(true);

    public void RefreshPipeline(bool preserveSelection)
    {
      this.RefreshPipeline(preserveSelection, false, 0);
    }

    public void RefreshPipeline(bool preserveSelection, int sqlRead)
    {
      this.RefreshPipeline(preserveSelection, false, sqlRead);
    }

    public void RefreshPipeline(bool preserveSelection, bool autoRefresh, int sqlRead)
    {
      if (this.suspendRefresh)
        return;
      using (CursorActivator.Wait())
      {
        using (MetricsFactory.GetIncrementalTimer("PipelineRefreshIncTimer", (SFxTag) new SFxUiTag()))
        {
          using (PerformanceMeter performanceMeter = PerformanceMeter.StartNew("Pipeline.Refresh", "Refresh the pipeline screen data", true, 1229, nameof (RefreshPipeline), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\PipelineScreen.cs"))
          {
            if (this.retrievePipelineData((QueryCriterion) null, (SortField[]) null, autoRefresh, sqlRead))
            {
              this.displayCurrentPage(preserveSelection, sqlRead);
              this.refreshFilterDescription();
            }
            performanceMeter.AddVariable("LoanCount", (object) this.navPipeline.NumberOfItems);
            performanceMeter.AddVariable("Folders", (object) this.getSelectedFolderList());
            performanceMeter.AddVariable("Columns", (object) this.gvLoans.Columns.Count);
            performanceMeter.AddVariable("Filter", (object) this.lblFilter.Text);
            GVColumnSort[] sortOrder = this.gvLoans.Columns.GetSortOrder();
            if (sortOrder.Length != 0)
              performanceMeter.AddVariable("Sort", this.gvLoans.Columns[sortOrder[0].Column].Tag);
            performanceMeter.Stop();
            MetricsFactory.RecordIncrementalTimerSample("EMA_Pipeline_Refresh", Convert.ToInt64(performanceMeter.Duration.TotalMilliseconds), (SFxTag) new SFxUiTag());
          }
        }
      }
    }

    public void RefreshViewableColumns()
    {
      if (this.layoutManager == null)
        return;
      this.layoutManager.AllColumns = this.getFullTableLayout();
    }

    public void RefreshFolders()
    {
      if (this.rbGlobalSearchOn.Checked)
        return;
      this.loanFolderInfos = Session.LoanManager.GetAllLoanFolderInfos(true, true);
      SessionObjects sessionObjects = Session.SessionObjects;
      LoanFolderInfo[] loanFolderInfos = this.loanFolderInfos;
      List<LoanFolderInfo> list = loanFolderInfos != null ? ((IEnumerable<LoanFolderInfo>) loanFolderInfos).ToList<LoanFolderInfo>() : (List<LoanFolderInfo>) null;
      sessionObjects.LoanFolderInfos = list;
      this.selectFolders(this.loanFolderInfos);
      if (this.cboFolder.CheckedItems.Count != 0)
        return;
      string workingFolder = Session.WorkingFolder;
      if (string.IsNullOrEmpty(workingFolder))
        workingFolder = Session.UserInfo.WorkingFolder;
      if (this.map.ContainsKey(workingFolder))
        ClientCommonUtils.CheckLoanFolder(this.cboFolder, workingFolder, CheckState.Checked);
      else
        this.selectAllFolders();
    }

    public void SetCurrentFilter(FieldFilterList filter, int sqlRead)
    {
      this.advFilter = filter;
      this.filterManager.ClearColumnFilters();
      this.refreshFilterDescription();
      this.RefreshPipeline(false, sqlRead);
      this.setViewChanged(true);
    }

    public void DisableRefreshTimer() => this.tmrRefresh.Enabled = false;

    public void EnableRefreshTimer()
    {
      if (this.invalidated)
        this.RefreshPipeline(true, 1);
      this.tmrRefresh.Enabled = true;
    }

    private void selectFolders(LoanFolderInfo[] folderInfos)
    {
      this.suspendEvents = true;
      try
      {
        Array.Sort<LoanFolderInfo>(folderInfos);
        if (this.folderListNeedsModification(this.getFolderList(), folderInfos))
        {
          this.classifyFolders(folderInfos);
          this.map.Clear();
          this.loadAllFolders(folderInfos);
        }
        if (!string.IsNullOrEmpty(this.currentViewFolders))
        {
          string[] folderList = this.currentViewFolders.Split('|');
          if (folderList[0].Equals(SystemSettings.AllFolders))
            this.selectAllFolders();
          else
            this.selectFoldersFromFolderList(folderList);
        }
        else
        {
          for (int index = 0; index < this.cboFolder.Items.Count; ++index)
            ClientCommonUtils.CheckLoanFolder(this.cboFolder, ((LoanFolderInfo) ((ComboBoxItem) this.cboFolder.Items[index]).Tag).Name, CheckState.Unchecked);
          if (this.cboView.SelectedItem == null)
            return;
          string str = this.currentView.LoanFolder;
          this.currentViewFolders = str;
          if (string.IsNullOrEmpty(str))
          {
            str = Session.WorkingFolder == null ? SystemSettings.AllFolders : Session.WorkingFolder;
            this.currentViewFolders = str;
          }
          string[] folderList = str.Split('|');
          if (folderList[0].Equals(SystemSettings.AllFolders))
          {
            this.selectAllFolders();
          }
          else
          {
            this.selectFoldersFromFolderList(folderList);
            if (this.cboFolder.ItemList.CheckedItems.Count == 0)
              this.selectAllFolders();
          }
          this.btnMove.Text = this.isRegularFolderSelected() || !this.isTrashFolderSelected() ? "&Move to Folder" : "&Restore to Folder";
          this.btnNew.Enabled = this.CanOriginateLoans();
          this.applySpecialFolderSecurity();
          this.setButtonEnabledStates();
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(PipelineScreen.sw, nameof (PipelineScreen), TraceLevel.Error, "Error loading folder list: " + (object) ex);
        ErrorDialog.Display(ex);
      }
      finally
      {
        this.suspendEvents = false;
      }
    }

    private bool folderListNeedsModification(
      string currentViewFolders,
      LoanFolderInfo[] folderInfos)
    {
      if (this.folderCounts != ((IEnumerable<LoanFolderInfo>) folderInfos).Count<LoanFolderInfo>())
      {
        this.folderCounts = ((IEnumerable<LoanFolderInfo>) folderInfos).Count<LoanFolderInfo>();
        return true;
      }
      Dictionary<string, string> dictionary = ((IEnumerable<string>) currentViewFolders.Split('|')).ToDictionary<string, string>((Func<string, string>) (folder => folder));
      foreach (LoanFolderInfo folderInfo in folderInfos)
      {
        if (!dictionary.TryGetValue(folderInfo.Name, out string _) || ((LoanFolderInfo) ((ComboBoxItem) this.cboFolder.Items[this.map[folderInfo.Name]]).Tag).Type != folderInfo.Type)
          return true;
      }
      return false;
    }

    private void displayCurrentPage(bool preserveSelections)
    {
      this.displayCurrentPage(preserveSelections, 0);
    }

    private void displayCurrentPage(bool preserveSelections, int sqlRead)
    {
      int currentPageItemIndex = this.navPipeline.CurrentPageItemIndex;
      int currentPageItemCount = this.navPipeline.CurrentPageItemCount;
      PipelineInfo[] pinfos = new PipelineInfo[0];
      if (currentPageItemCount > 0)
        pinfos = (PipelineInfo[]) this.pipelineCursor.GetItems(currentPageItemIndex, currentPageItemCount, false, sqlRead);
      Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
      if (preserveSelections)
      {
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvLoans.Items)
        {
          if (gvItem.Selected && gvItem.Tag != null)
            dictionary[((PipelineInfo) gvItem.Tag).GUID] = true;
        }
      }
      LoanInfo.Right[] effectiveRightsForLoans = ((LoanAccessBpmManager) Session.BPM.GetBpmManager(BizRuleType.LoanAccess)).GetEffectiveRightsForLoans(pinfos);
      this.gvLoans.Items.Clear();
      this.gvLoans.Selectable = true;
      for (int index = 0; index < pinfos.Length; ++index)
      {
        GVItem itemForPipelineInfo = this.createGVItemForPipelineInfo(pinfos[index], effectiveRightsForLoans[index]);
        this.gvLoans.Items.Add(itemForPipelineInfo);
        if (dictionary.ContainsKey(pinfos[index].GUID))
          itemForPipelineInfo.Selected = true;
      }
      if (this.gvLoans.Items.Count <= 0)
        this.gvLoans.Selectable = false;
      if (this.gvLoans.Items.Count > 0 && this.gvLoans.SelectedItems.Count == 0)
        this.gvLoans.Items[0].Selected = true;
      this.setButtonEnabledStates();
    }

    private GVItem createGVItemForPipelineInfo(PipelineInfo pinfo, LoanInfo.Right effectiveRights)
    {
      GVItem itemForPipelineInfo = new GVItem();
      for (int index = 0; index < this.gvLoans.Columns.Count; ++index)
      {
        string columnId = ((TableLayout.Column) this.gvLoans.Columns[index].Tag).ColumnID;
        LoanReportFieldDef fieldByCriterionName = this.fieldDefs.GetFieldByCriterionName(columnId);
        object displayElement = pinfo.Info[(object) columnId];
        if (fieldByCriterionName != null)
          displayElement = fieldByCriterionName.ToDisplayElement(columnId, pinfo, (Control) this.gvLoans);
        itemForPipelineInfo.SubItems[index].Value = this.selectLoanOnly ? (object) displayElement.ToString() : displayElement;
        itemForPipelineInfo.Tag = (object) pinfo;
        if (pinfo.LockInfo.IsLocked || this.isLoanReadOnly(pinfo, effectiveRights))
        {
          itemForPipelineInfo.BackColor = EncompassColors.Secondary2;
          itemForPipelineInfo.ForeColor = EncompassColors.Secondary5;
        }
        if (string.Concat(pinfo.GetField("Loan.Adverse")) == "Y")
          itemForPipelineInfo.BackColor = EncompassColors.Alert3;
      }
      return itemForPipelineInfo;
    }

    private bool isLoanReadOnly(PipelineInfo pinfo, LoanInfo.Right effectiveRights)
    {
      if (Session.UserInfo.IsSuperAdministrator())
        return false;
      return effectiveRights == LoanInfo.Right.Read || ((LoanAccessBpmManager) Session.BPM.GetBpmManager(BizRuleType.LoanAccess)).GetLoanContentAccess(pinfo) == LoanContentAccess.None;
    }

    public void SetCurrentFolder(string folderName)
    {
      if (string.IsNullOrEmpty(folderName))
        return;
      string str1 = folderName;
      char[] chArray = new char[1]{ '|' };
      foreach (string str2 in str1.Split(chArray))
      {
        if (this.map.ContainsKey(str2))
          ClientCommonUtils.CheckLoanFolder(this.cboFolder, str2, CheckState.Checked);
      }
    }

    public void SetCurrentView(FileSystemEntry fsEntry) => this.SetCurrentView(fsEntry, 0);

    public void SetCurrentView(FileSystemEntry fsEntry, int sqlRead)
    {
      try
      {
        this.SetCurrentView((PipelineView) Session.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.PipelineView, fsEntry) ?? throw new ArgumentException(), fsEntry, sqlRead);
      }
      catch (Exception ex)
      {
        Tracing.Log(PipelineScreen.sw, nameof (PipelineScreen), TraceLevel.Error, "Error opening view: " + (object) ex);
        ErrorDialog.Display(ex);
        this.RefreshViews(false);
      }
    }

    public void SetCurrentView(PipelineView view, FileSystemEntry e)
    {
      this.SetCurrentView(view, e, 0);
    }

    public void SetCurrentView(
      PipelineView view,
      FileSystemEntry e,
      int sqlRead,
      PipelineViewList userPipeline = null)
    {
      this.currentView = view;
      this.suspendEvents = true;
      this.applyTableLayout(view.Layout);
      this.advFilter = view.Filter;
      this.filterManager.ClearColumnFilters();
      this.selectFolders(this.loanFolderInfos);
      this.suspendEvents = true;
      if (view.LoanFolder != null && this.currentViewFolders == null)
        this.SetCurrentFolder(view.LoanFolder);
      if (view.LoanOwnership == PipelineViewLoanOwnership.All)
        this.cboLoanType.SelectedIndex = 0;
      else
        this.cboLoanType.SelectedIndex = 1;
      if (view.ExternalOrgId != null && view.ExternalOrgId != string.Empty && view.ExternalOrgId != "0")
      {
        if (this.externalOrgsList == null)
          this.loadTPOSettings();
        ExternalOriginatorManagementData originatorManagementData = this.externalOrgsList.FirstOrDefault<ExternalOriginatorManagementData>((Func<ExternalOriginatorManagementData, bool>) (e1 => e1.ExternalID == view.ExternalOrgId));
        if (originatorManagementData != null)
        {
          this.textBox1.Text = originatorManagementData.OrganizationName;
          this.textBox1.Tag = (object) originatorManagementData.ExternalID;
        }
        else
        {
          this.textBox1.Text = "All";
          this.textBox1.Tag = (object) "-1";
        }
      }
      else
      {
        this.textBox1.Text = "All";
        this.textBox1.Tag = (object) "-1";
      }
      if (view.LoanOrgType == PipelineViewLoanOrgType.Internal)
      {
        this.popUpButton.Enabled = false;
        this.cboCompany.SelectedIndex = 0;
      }
      else
        this.cboCompany.SelectedIndex = 1;
      this.setViewChanged(false);
      this.suspendEvents = false;
      this.btnNew.Enabled = this.CanOriginateLoans();
      this.RefreshPipeline(false, sqlRead);
    }

    private void clearPipeline()
    {
      this.gvLoans.Items.Clear();
      this.navPipeline.NumberOfItems = 0;
    }

    private bool retrievePipelineData()
    {
      return this.retrievePipelineData((QueryCriterion) null, (SortField[]) null);
    }

    private bool retrievePipelineData(QueryCriterion filter, SortField[] sortFields)
    {
      return this.retrievePipelineData(filter, sortFields, false, 0);
    }

    private bool retrievePipelineData(QueryCriterion filter, SortField[] sortFields, int sqlRead)
    {
      return this.retrievePipelineData(filter, sortFields, false, sqlRead);
    }

    private bool retrievePipelineData(
      QueryCriterion filter,
      SortField[] sortFields,
      bool autoRefresh,
      int sqlRead)
    {
      string[] fieldList = this.generateFieldList();
      string str = this.getSelectedFolderList().Length == 0 ? string.Empty : this.getSelectedFolderList();
      if (filter == null)
        filter = this.generateQueryCriteria();
      this.q_Filter = filter;
      if (sortFields == null)
        sortFields = this.getCurrentSortFields();
      this.p_sortField = sortFields;
      try
      {
        this.suspendEvents = true;
        if (this.pipelineCursor != null)
        {
          try
          {
            this.pipelineCursor.Dispose();
          }
          catch
          {
          }
          this.pipelineCursor = (ICursor) null;
        }
        bool isExternalOrganization = false;
        if (this.cboCompany.SelectedItem.Equals((object) "TPO"))
          isExternalOrganization = true;
        MetricsFactory.IncrementCounter("PipelineRefreshIncCounter", (SFxTag) new SFxInternalTag());
        using (MetricsFactory.GetIncrementalTimer("PipelineRefreshIncTimer", (SFxTag) new SFxInternalTag()))
        {
          string[] strArray1;
          if (!string.IsNullOrEmpty(str))
            strArray1 = str.Split('|');
          else
            strArray1 = (string[]) null;
          string[] strArray2 = strArray1;
          if (strArray2 != null && ((IEnumerable<string>) strArray2).Contains<string>("<All Folders>"))
            strArray2 = (string[]) null;
          this.pipelineCursor = Session.LoanManager.OpenPipeline(strArray2, LoanInfo.Right.Access, fieldList, PipelineData.Lock | PipelineData.Milestones | PipelineData.LoanAssociates | PipelineData.CurrentUserAccessRightsOnly, sortFields, filter, isExternalOrganization, sqlRead, this.rbGlobalSearchOn.Checked, !this.chkarchive.Checked);
        }
        this.navPipeline.NumberOfItems = this.pipelineCursor.GetItemCount();
        this.lastRefreshTime = DateTime.Now;
        this.invalidated = false;
        this.errorMsgDisplayed = false;
        return true;
      }
      catch (Exception ex)
      {
        if (autoRefresh)
        {
          MetricsFactory.IncrementErrorCounter(ex, "Error auto refreshing pipeline", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\PipelineScreen.cs", nameof (retrievePipelineData), 1789);
          if (!this.errorMsgDisplayed)
          {
            this.errorMsgDisplayed = true;
            Tracing.Log(PipelineScreen.sw, nameof (PipelineScreen), TraceLevel.Error, ex.ToString());
            int num = (int) Utils.Dialog((IWin32Window) this, "Error auto refreshing pipeline.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
        }
        else
        {
          MetricsFactory.IncrementErrorCounter(ex, "Error loading pipeline", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\PipelineScreen.cs", nameof (retrievePipelineData), 1799);
          Tracing.Log(PipelineScreen.sw, nameof (PipelineScreen), TraceLevel.Error, ex.ToString());
          int num = (int) Utils.Dialog((IWin32Window) this, "Error loading pipeline: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        return false;
      }
      finally
      {
        this.suspendEvents = false;
      }
    }

    private JObject ConstructDeliveryParameter()
    {
      JObject jobject1 = (JObject) null;
      if (this.q_Filter != null)
        jobject1 = !(this.q_Filter is StringValueCriterion) || !((this.q_Filter as StringValueCriterion).FieldName == "Loan.LoanFolder") || !((this.q_Filter as StringValueCriterion).Value == "(Trash)") ? this.q_Filter.ToJson() : (JObject) null;
      if (!string.IsNullOrEmpty(this.cboFolder.ComboBoxText) && this.cboFolder.ItemList.CheckedItems.Count > 0)
      {
        List<string> source = new List<string>();
        bool flag = false;
        foreach (object checkedItem in this.cboFolder.ItemList.CheckedItems)
        {
          if (((ComboBoxItem) checkedItem).Name == SystemSettings.AllFolders)
          {
            flag = true;
            break;
          }
          source.Add(((ComboBoxItem) checkedItem).Name);
        }
        JObject jobject2 = new JObject();
        if (!flag)
        {
          if (source.Count > 1)
          {
            IEnumerable<\u003C\u003Ef__AnonymousType2<string, string, string>> o = source.Select(l => new
            {
              canonicalName = "Loan.LoanFolder",
              value = l,
              matchType = "exact"
            });
            jobject2.Add("operator", (JToken) "OR");
            jobject2.Add("terms", (JToken) JArray.FromObject((object) o));
          }
          else
          {
            var o = new
            {
              canonicalName = "Loan.LoanFolder",
              value = source.FirstOrDefault<string>(),
              matchType = "exact"
            };
            jobject2 = JObject.FromObject((object) o);
          }
        }
        else
        {
          IEnumerable<\u003C\u003Ef__AnonymousType2<string, string, string>> o = ((IEnumerable<string>) new string[2]
          {
            "LoanFolder.Trash",
            "LoanFolder.Archive"
          }).Select(foldername => new
          {
            canonicalName = foldername,
            value = "N",
            matchType = "exact"
          });
          jobject2.Add("operator", (JToken) "OR");
          jobject2.Add("terms", (JToken) JArray.FromObject((object) o));
        }
        if (jobject1 != null)
        {
          jobject1 = new JObject()
          {
            {
              "operator",
              (JToken) "AND"
            },
            {
              "terms",
              (JToken) new JArray()
              {
                (JToken) jobject1,
                (JToken) jobject2
              }
            }
          };
        }
        else
        {
          JObject jobject3 = new JObject();
          jobject1 = jobject2;
        }
      }
      return jobject1;
    }

    private IList<LoanFolderInfo> getCurrentLoanFolders()
    {
      IList<LoanFolderInfo> currentLoanFolders = (IList<LoanFolderInfo>) new List<LoanFolderInfo>();
      foreach (ComboBoxItem checkedItem in this.cboFolder.ItemList.CheckedItems)
      {
        LoanFolderInfo tag = (LoanFolderInfo) ((ComboBoxItem) this.cboFolder.ItemList.CheckedItems[0]).Tag;
        currentLoanFolders.Add(tag);
      }
      return currentLoanFolders;
    }

    private SortField[] getCurrentSortFields()
    {
      return this.getSortFieldsForColumnSort(this.gvLoans.Columns.GetSortOrder());
    }

    private SortField[] getSortFieldsForColumnSort(GVColumnSort[] sortOrder)
    {
      List<SortField> sortFieldList = new List<SortField>();
      IList<LoanFolderInfo> currentLoanFolders = this.getCurrentLoanFolders();
      bool flag1 = false;
      bool flag2 = false;
      foreach (LoanFolderInfo loanFolderInfo in (IEnumerable<LoanFolderInfo>) currentLoanFolders)
      {
        if (loanFolderInfo.Name.Equals("(Archive)"))
          flag1 = true;
        if (loanFolderInfo.Name.Equals(SystemSettings.AllFolders))
          flag2 = true;
      }
      foreach (GVColumnSort gvColumnSort in sortOrder)
      {
        TableLayout.Column tag = (TableLayout.Column) this.gvLoans.Columns[gvColumnSort.Column].Tag;
        if (!tag.ColumnID.Equals("Alerts.AlertCount") || ((flag2 ? 1 : (currentLoanFolders.Count > 1 ? 1 : 0)) | (flag1 ? 1 : 0)) == 0)
          sortFieldList.Add(this.getSortFieldForColumn(tag, gvColumnSort.SortOrder));
      }
      return sortFieldList.ToArray();
    }

    private SortField getSortFieldForColumn(TableLayout.Column colInfo, SortOrder sortOrder)
    {
      LoanReportFieldDef fieldByCriterionName = this.fieldDefs.GetFieldByCriterionName(colInfo.ColumnID);
      return new SortField(fieldByCriterionName.SortTerm, sortOrder == SortOrder.Ascending ? FieldSortOrder.Ascending : FieldSortOrder.Descending, fieldByCriterionName.DataConversion);
    }

    private QueryCriterion generateQueryCriteria()
    {
      QueryCriterion criterion1 = (QueryCriterion) null;
      if (this.cboFolder.Items.Count > 0 && this.cboFolder.ItemList.GetItemChecked(0))
        criterion1 = (QueryCriterion) new StringValueCriterion("Loan.LoanFolder", SystemSettings.TrashFolder, StringMatchType.Exact, false);
      QueryCriterion criterion2 = (QueryCriterion) null;
      QueryCriterion criterion3 = (QueryCriterion) null;
      if (this.cboCompany.SelectedItem != null && this.cboCompany.SelectedItem.Equals((object) "TPO"))
      {
        if (this.cboLoanType.SelectedItem != null)
        {
          if (this.cboLoanType.SelectedItem.Equals((object) "My Loans"))
            criterion2 = (QueryCriterion) new StringValueCriterion("Loan.TPOLOID", "-1", StringMatchType.NotEquals);
          else if (this.cboLoanType.SelectedItem.Equals((object) "All Loans") && this.isAdmin)
            criterion2 = new StringValueCriterion("Loan.TPOLOID", (string) null, StringMatchType.NotEquals).And((QueryCriterion) new StringValueCriterion("Loan.TPOLOID", "", StringMatchType.NotEquals)).Or(new StringValueCriterion("Loan.TPOLPID", (string) null, StringMatchType.NotEquals).And((QueryCriterion) new StringValueCriterion("Loan.TPOLPID", "", StringMatchType.NotEquals)));
        }
        if (this.isAdmin)
        {
          QueryCriterion criterion4 = new StringValueCriterion("Loan.TPOCompanyID", (string) null, StringMatchType.NotEquals).And((QueryCriterion) new StringValueCriterion("Loan.TPOCompanyID", "", StringMatchType.NotEquals));
          criterion2 = criterion2.And(criterion4);
        }
        if (this.textBox1.Text != "" && this.textBox1.Text != "All")
          criterion3 = (QueryCriterion) new StringValueCriterion("Loan.TPOCompanyID", (string) this.textBox1.Tag, StringMatchType.Exact);
      }
      else if (this.cboLoanType.SelectedItem != null && this.cboLoanType.SelectedItem.Equals((object) "My Loans"))
        criterion2 = (QueryCriterion) new StringValueCriterion("LoanAssociateUser.UserID", Session.UserID, StringMatchType.Exact);
      QueryCriterion criterion5 = (QueryCriterion) null;
      if (this.advFilter != null)
        criterion5 = this.advFilter.CreateEvaluator().ToQueryCriterion();
      QueryCriterion queryCriterion = this.filterManager.ToQueryCriterion();
      QueryCriterion queryCriteria = (QueryCriterion) null;
      if (criterion1 != null)
        queryCriteria = queryCriteria == null ? criterion1 : queryCriteria.And(criterion1);
      if (criterion2 != null)
        queryCriteria = queryCriteria == null ? criterion2 : queryCriteria.And(criterion2);
      if (criterion3 != null)
        queryCriteria = queryCriteria == null ? criterion3 : queryCriteria.And(criterion3);
      if (criterion5 != null)
        queryCriteria = queryCriteria == null ? criterion5 : queryCriteria.And(criterion5);
      if (queryCriterion != null)
        queryCriteria = queryCriteria == null ? queryCriterion : queryCriteria.And(queryCriterion);
      return queryCriteria;
    }

    private QueryCriterion addArchiveFoldersCriteria(QueryCriterion filter)
    {
      IEnumerable<LoanFolderInfo> source = ((IEnumerable<LoanFolderInfo>) this.loanFolderInfos).Where<LoanFolderInfo>((Func<LoanFolderInfo, bool>) (loanFolder => loanFolder.Type != LoanFolderInfo.LoanFolderType.Archive));
      if (!source.Any<LoanFolderInfo>())
        return filter;
      QueryCriterion criterion = (QueryCriterion) null;
      foreach (LoanFolderInfo loanFolderInfo in source)
        criterion = criterion != null ? criterion.Or((QueryCriterion) new StringValueCriterion("Loan.LoanFolder", loanFolderInfo.Name, StringMatchType.Exact)) : (QueryCriterion) new StringValueCriterion("Loan.LoanFolder", loanFolderInfo.Name, StringMatchType.Exact);
      if (criterion != null)
        filter = filter == null ? criterion : filter.And(criterion);
      return filter;
    }

    private string[] generateFieldList()
    {
      List<string> stringList = new List<string>((IEnumerable<string>) PipelineScreen.requiredFields);
      IList<LoanFolderInfo> currentLoanFolders = this.getCurrentLoanFolders();
      foreach (string ruleField in LoanBusinessRuleInfo.RuleFields)
      {
        if (!stringList.Contains(ruleField))
          stringList.Add(ruleField);
      }
      bool flag1 = false;
      bool flag2 = false;
      foreach (LoanFolderInfo loanFolderInfo in (IEnumerable<LoanFolderInfo>) currentLoanFolders)
      {
        if (loanFolderInfo.Name.Equals("(Archive)"))
          flag1 = true;
        if (loanFolderInfo.Name.Equals(SystemSettings.AllFolders))
          flag2 = true;
      }
      foreach (TableLayout.Column column in this.layoutManager?.GetCurrentLayout())
      {
        if (!column.ColumnID.Equals("Alerts.AlertCount") || ((flag2 ? 1 : (currentLoanFolders.Count > 1 ? 1 : 0)) | (flag1 ? 1 : 0)) == 0)
        {
          LoanReportFieldDef fieldByCriterionName = this.fieldDefs.GetFieldByCriterionName(column.ColumnID);
          if (!stringList.Contains(column.ColumnID))
            stringList.Add(column.ColumnID);
          if (fieldByCriterionName != null)
          {
            foreach (string relatedField in fieldByCriterionName.RelatedFields)
            {
              if (!stringList.Contains(relatedField))
                stringList.Add(relatedField);
            }
          }
        }
      }
      return stringList.ToArray();
    }

    private void validateTableLayout(TableLayout layout)
    {
      List<TableLayout.Column> columnList = new List<TableLayout.Column>();
      foreach (TableLayout.Column column in layout)
      {
        ReportFieldDef fieldByCriterionName = (ReportFieldDef) this.fieldDefs.GetFieldByCriterionName(column.ColumnID);
        if (fieldByCriterionName != null)
        {
          column.Title = fieldByCriterionName.Description;
          if (column.Width < 0)
            column.Width = fieldByCriterionName.GetDefaultColumnWidth();
        }
        else
          columnList.Add(column);
      }
      foreach (TableLayout.Column column in columnList)
        layout.Remove(column);
    }

    private void applyTableLayout(TableLayout layout)
    {
      if (this.layoutManager == null)
        this.layoutManager = this.createLayoutManager();
      this.validateTableLayout(layout);
      this.layoutManager.ApplyLayout(layout, false);
      if (this.filterManager == null)
      {
        this.filterManager = new GridViewReportFilterManager(Session.DefaultInstance, this.gvLoans);
        this.filterManager.FilterChanged += new EventHandler(this.filterManager_FilterChanged);
      }
      this.filterManager.CreateColumnFilters((ReportFieldDefs) this.fieldDefs);
    }

    private GridViewLayoutManager createLayoutManager()
    {
      GridViewLayoutManager layoutManager = new GridViewLayoutManager(this.gvLoans, this.getFullTableLayout());
      layoutManager.LayoutChanged += new EventHandler(this.onLayoutChanged);
      return layoutManager;
    }

    private void onLayoutChanged(object sender, EventArgs e)
    {
      if (this.suspendEvents)
        return;
      if (this.filterManager != null)
        this.filterManager.CreateColumnFilters((ReportFieldDefs) this.fieldDefs);
      this.setViewChanged(true);
      this.RefreshPipeline(true);
      this.rbGlobalSearchOn.Enabled = this.GridHasAnyGlobalSearchFields;
      if (!this.rbGlobalSearchOn.Checked)
        return;
      this.filterManager.DisableFilterControl(this.disableFilter());
    }

    private void filterManager_FilterChanged(object sender, EventArgs e)
    {
      if (this.suspendEvents)
        return;
      this.refreshFilterDescription();
      this.setViewChanged(true);
      this.RefreshPipeline(false, 1);
    }

    private void refreshFilterDescription()
    {
      FieldFilterList currentFilter = this.GetCurrentFilter();
      if (currentFilter.Count == 0)
      {
        this.lblFilter.Text = "None";
        this.btnClearSearch.Enabled = false;
      }
      else
      {
        this.lblFilter.Text = currentFilter.ToString(true);
        this.btnClearSearch.Enabled = true;
      }
    }

    private TableLayout getFullTableLayout()
    {
      TableLayout fullTableLayout = new TableLayout();
      FileStream fileStream = (FileStream) null;
      if ("1" == (Session.SessionObjects.GetCompanySettingFromCache("DebugTrace", "PipelineReportFields") ?? ""))
      {
        try
        {
          string path1 = "c:\\temp";
          string path2 = path1 + "\\getcolumns_ui_" + Session.UserID + ".csv";
          Directory.CreateDirectory(path1);
          fileStream = File.Open(path2, FileMode.Create);
          byte[] bytes = new UTF8Encoding(true).GetBytes("# CriterionFieldName,Description,FieldID,DisplayType\n");
          fileStream.Write(bytes, 0, bytes.Length);
        }
        catch (Exception ex)
        {
          Tracing.Log(PipelineScreen.sw, nameof (PipelineScreen), TraceLevel.Error, ex.ToString());
          fileStream = (FileStream) null;
        }
      }
      foreach (LoanReportFieldDef fieldDef in (ReportFieldDefContainer) this.fieldDefs)
      {
        if (fieldDef.Selectable && fullTableLayout.GetColumnByID(fieldDef.CriterionFieldName) == null)
        {
          fullTableLayout.AddColumn(ReportFieldClientExtension.ToTableLayoutColumn(fieldDef));
          if (fileStream != null)
          {
            byte[] bytes = new UTF8Encoding(true).GetBytes(fieldDef.CriterionFieldName + "," + fieldDef.Description + "," + fieldDef.FieldDefinition.FieldID + "," + fieldDef.DisplayType.ToString() + "\n");
            fileStream.Write(bytes, 0, bytes.Length);
          }
        }
      }
      fileStream?.Close();
      fullTableLayout.SortByDescription();
      return fullTableLayout;
    }

    private void cboView_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.suspendEvents)
        return;
      if (this.clearCurrentFolder)
        this.currentViewFolders = string.Empty;
      if (this.cboView.SelectedIndex < 0)
      {
        this.clearPipeline();
      }
      else
      {
        this.RefreshCurrentView(1);
        this.UpdateGlobalSearchControls();
      }
    }

    private void RefreshCurrentView(int sqlRead)
    {
      PipelineViewList selectedItem = (PipelineViewList) this.cboView.SelectedItem;
      this.SetCurrentView(selectedItem.PipelineView, (FileSystemEntry) null, sqlRead, selectedItem);
    }

    private void navPipeline_PageChangedEvent(object sender, PageChangedEventArgs e)
    {
      if (this.suspendEvents)
        return;
      using (CursorActivator.Wait())
        this.displayCurrentPage(false, 1);
    }

    private LoanFolderRuleInfo getLoanFolderRule(LoanFolderInfo loanFolder)
    {
      if (loanFolder.Name == SystemSettings.AllFolders)
        return new LoanFolderRuleInfo(loanFolder.Name, false);
      return loanFolder.Type == LoanFolderInfo.LoanFolderType.Trash ? new LoanFolderRuleInfo(loanFolder.Name, false) : ((LoanFolderRuleManager) Session.BPM.GetBpmManager(BpmCategory.LoanFolder)).GetRule(loanFolder.Name);
    }

    private void btnSaveView_Click(object sender, EventArgs e) => this.saveCurrentView();

    private void saveCurrentView()
    {
      PipelineView view = new PipelineView(this.currentView.Name);
      view.ViewID = this.currentView.ViewID;
      view.LoanFolder = this.getSelectedFolderList();
      view.Layout = this.layoutManager.GetCurrentLayout();
      view.Filter = this.GetCurrentFilter();
      view.IsUserView = this.currentView.IsUserView;
      view.LoanOwnership = this.cboLoanType.SelectedItem.Equals((object) "All Loans") ? PipelineViewLoanOwnership.All : PipelineViewLoanOwnership.User;
      view.LoanOrgType = this.cboCompany.SelectedItem.Equals((object) "Internal Organization") ? PipelineViewLoanOrgType.Internal : PipelineViewLoanOrgType.TPO;
      if (this.textBox1.Text != "" && this.textBox1.Text != "All")
        view.ExternalOrgId = (string) this.textBox1.Tag;
      using (SavePipelineViewDialog pipelineViewDialog = new SavePipelineViewDialog(view, this.getViewNameList(), this.currentView != null && this.currentView.IsUserView))
      {
        switch (pipelineViewDialog.ShowDialog((IWin32Window) this))
        {
          case DialogResult.OK:
            this.updateCurrentView(view, pipelineViewDialog.SelectedEntry);
            break;
          case DialogResult.Cancel:
            return;
        }
      }
      this.setViewChanged(false);
    }

    private string[] getViewNameList()
    {
      List<string> stringList = new List<string>();
      foreach (PipelineViewList pipelineViewList in this.cboView.Items)
        stringList.Add(pipelineViewList.ToString());
      return stringList.ToArray();
    }

    private void updateCurrentView(
      PipelineView view,
      FileSystemEntry e,
      UserPipelineView userPipeline = null)
    {
      this.currentView = view;
      this.pipelineViews = PipelineViewListItem.GetPipelineViewForUserPipelineView(Session.LoanManager.GetPipelineData(), (ReportFieldDefContainer) this.fieldDefs, new ReportFieldCommonExtension.GetFieldDefWidth(ReportFieldClientExtension.GetDefaultColumnWidth));
      this.pipelineViews.AddRange((IEnumerable<PipelineView>) this.personaPipelineViews);
      this.loadViewList(Session.ConfigurationManager.GetUserAccessibleViews(Session.UserID));
    }

    private void gvLoans_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      using (MetricsFactory.GetIncrementalTimer("LoanOpenIncTimer", (SFxTag) new SFxUiTag()))
      {
        MainForm.Instance.HideInsightsOnStatusBar();
        if (e.Item.Selected)
        {
          PipelineInfo tag = (PipelineInfo) e.Item.Tag;
          if (tag == null)
            return;
          this.openLoan(tag.GUID);
        }
      }
      MetricsFactory.IncrementCounter("LoanOpenIncCounter", (SFxTag) new SFxUiTag());
    }

    private void openLoan(string guid)
    {
      try
      {
        Session.Application.GetService<ILoanConsole>().OpenLoan(guid);
      }
      catch (Exception ex)
      {
        Tracing.Log(PipelineScreen.sw, nameof (PipelineScreen), TraceLevel.Error, "Error opening loan: " + (object) ex);
        ErrorDialog.Display(ex);
      }
    }

    public void OpenLoan(string guid) => this.openLoan(guid);

    private void btnAdvSearch_Click(object sender, EventArgs e)
    {
      using (PipelineAdvSearchDialog pipelineAdvSearchDialog = new PipelineAdvSearchDialog(this.fieldDefs, this.GetCurrentFilter()))
      {
        if (pipelineAdvSearchDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.SetCurrentFilter(pipelineAdvSearchDialog.GetSelectedFilter(), 1);
      }
    }

    public FieldFilterList GetCurrentFilter()
    {
      FieldFilterList fieldFilterList = this.filterManager.ToFieldFilterList();
      if (this.advFilter != null)
        fieldFilterList.Merge(this.advFilter);
      return fieldFilterList;
    }

    private void btnClearSearch_Click(object sender, EventArgs e)
    {
      this.SetCurrentFilter((FieldFilterList) null, 1);
    }

    private TableLayout getDemoTableLayout()
    {
      TableLayout demoTableLayout = new TableLayout();
      demoTableLayout.AddColumn(new TableLayout.Column("Alerts.AlertCount", "Alert", HorizontalAlignment.Center, 40));
      demoTableLayout.AddColumn(new TableLayout.Column("Loan.LockExpirationDate", "Lock Status", HorizontalAlignment.Center, 103));
      demoTableLayout.AddColumn(new TableLayout.Column("Loan.LoanNumber", "Loan Number", HorizontalAlignment.Left, 103));
      demoTableLayout.AddColumn(new TableLayout.Column("Loan.BorrowerName", "Borrower Name", HorizontalAlignment.Left, 180));
      demoTableLayout.AddColumn(new TableLayout.Column("Loan.LoanAmount", "Loan Amount", HorizontalAlignment.Right, 103));
      demoTableLayout.AddColumn(new TableLayout.Column("Loan.LoanType", "Loan Type", HorizontalAlignment.Left, 103));
      demoTableLayout.AddColumn(new TableLayout.Column("Loan.Address1", "Property Address", HorizontalAlignment.Left, 180));
      demoTableLayout.AddColumn(new TableLayout.Column("Loan.City", "City", HorizontalAlignment.Left, 103));
      demoTableLayout.AddColumn(new TableLayout.Column("Loan.State", "State", HorizontalAlignment.Left, 40));
      demoTableLayout.AddColumn(new TableLayout.Column("Loan.LoanOfficerName", "Loan Officer", HorizontalAlignment.Left, 180));
      demoTableLayout.AddColumn(new TableLayout.Column("Loan.LoanProcessorName", "Loan Processor", HorizontalAlignment.Left, 180));
      demoTableLayout.AddColumn(new TableLayout.Column("NextMilestone.MilestoneName", "Next Milestone", HorizontalAlignment.Left, 103));
      return demoTableLayout;
    }

    private void btnNew_Click(object sender, EventArgs e)
    {
      if (!this.btnNew.Enabled)
        return;
      this.createNewLoan();
    }

    private void createNewLoan()
    {
      LoanFolderRuleManager bpmManager = (LoanFolderRuleManager) Session.BPM.GetBpmManager(BpmCategory.LoanFolder);
      LoanFolderInfo loanFolderInfo = new LoanFolderInfo(this.cboFolder.ComboBoxText);
      if (!bpmManager.GetRule(loanFolderInfo.Name).CanOriginateLoans)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You are not authorized to originate loans in the current folder.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
        Session.Application.GetService<ILoanConsole>().StartNewLoan(loanFolderInfo.Name, true);
    }

    private void btnEdit_Click(object sender, EventArgs e)
    {
      if (this.gvLoans == null || this.gvLoans.SelectedItems.Count != 1 || !(this.gvLoans.SelectedItems[0].Tag is PipelineInfo tag))
        return;
      this.openLoan(tag.GUID);
    }

    private void btnDuplicate_Click(object sender, EventArgs e)
    {
      if (this.gvLoans.SelectedItems.Count != 1)
        return;
      this.duplicateSelectedLoan();
    }

    private List<int> disableFilter()
    {
      List<int> intList = new List<int>();
      foreach (GVColumn column in this.gvLoans.Columns)
      {
        if (Utils.GlobalSearchFields.Contains(column.Text))
          intList.Add(column.Index);
      }
      return intList;
    }

    private void EnableDisableAllFolderControls(bool flag)
    {
      this.cboFolder.Enabled = this.cboCompany.Enabled = this.textBox1.Enabled = this.popUpButton.Enabled = this.cboLoanType.Enabled = flag;
    }

    private void rbGlobalSearch_Changed(object sender, EventArgs e)
    {
      this.EnableDisableAllFolderControls(!this.rbGlobalSearchOn.Checked);
      this.btnRefreshView.Enabled = this.btnManageViews.Enabled = this.cboView.Enabled = !this.rbGlobalSearchOn.Checked;
      this.btnSaveView.Enabled = this.hasManagePipelineViewRights && !this.rbGlobalSearchOn.Checked;
      this.layoutManager.IsGlobalSearch = this.rbGlobalSearchOn.Checked;
      if (this.rbGlobalSearchOn.Checked)
      {
        this.advFilter = (FieldFilterList) null;
        this.selectAllFolders();
        this.filterManager.IsGlobalSearchOn = true;
        if (this.cboLoanType.SelectedIndex != 0)
        {
          this.loanTypeselected = this.cboLoanType.SelectedIndex;
          this.cboLoanType.SelectedIndex = 0;
        }
        else
          this.cboLoanType_SelectedIndexChanged((object) null, (EventArgs) null);
        this.setViewChanged(this.folderSelectedChanged);
        this.filterManager.IsGlobalSearchOn = false;
        this.filterManager.DisableFilterControl(this.disableFilter());
        this.btnNew.Enabled = false;
        this.RefreshPipeline(true, 1);
      }
      else
      {
        if (this.loanTypeselected >= 0 && this.cboLoanType.SelectedIndex != 0)
        {
          this.cboLoanType.SelectedIndexChanged -= new EventHandler(this.cboLoanType_SelectedIndexChanged);
          this.cboLoanType.SelectedIndex = this.loanTypeselected;
          this.cboLoanType.SelectedIndexChanged += new EventHandler(this.cboLoanType_SelectedIndexChanged);
        }
        this.RefreshCurrentView(1);
        this.filterManager.EnableFilterControl();
        if (this.cboFolder.ItemList.CheckedItems.Count > 1 && this.btnNew.Enabled)
          this.btnNew.Enabled = false;
      }
      this.rbGlobalSearchOn.Enabled = this.GridHasAnyGlobalSearchFields;
    }

    private void duplicateSelectedLoan()
    {
      List<string> stringList = new List<string>((IEnumerable<string>) ((LoanFolderRuleManager) Session.BPM.GetBpmManager(BpmCategory.LoanFolder)).GetLoanFoldersForAction(LoanFolderAction.DuplicateInto));
      if (stringList.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You are not authorized to duplicate loans into any folder.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        PipelineInfo tag = this.gvLoans.SelectedItems[0].Tag as PipelineInfo;
        bool isSecond = false;
        bool isPiggyback = false;
        string str = string.Concat(tag.GetField("Loan.LoanFolder"));
        string templateToUse = string.Empty;
        int selectedIndex = -1;
        LoanFolderInfo[] allowedFolders = LoanFolderUtil.LoanFolderNames2LoanFolderInfos(stringList.ToArray(), str, out selectedIndex);
        LoanFolderInfo defaultFolder = (LoanFolderInfo) null;
        if (selectedIndex >= 0)
          defaultFolder = allowedFolders[selectedIndex];
        using (DuplicateLoanDialog duplicateLoanDialog = new DuplicateLoanDialog(allowedFolders, defaultFolder))
        {
          if (duplicateLoanDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          isSecond = duplicateLoanDialog.DuplicateAsSecond;
          isPiggyback = duplicateLoanDialog.DuplicateAsPiggyback;
          str = duplicateLoanDialog.SelectedFolder;
          templateToUse = duplicateLoanDialog.SelectedTemplate;
        }
        this.duplicateLoan(tag.GUID, isSecond, isPiggyback, str, templateToUse);
      }
    }

    private void duplicateLoan(
      string guid,
      bool isSecond,
      bool isPiggyback,
      string destinationFolder,
      string templateToUse)
    {
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        LoanDataMgr sourceMgr = LoanDataMgr.OpenLoan(Session.SessionObjects, guid, false);
        if (isPiggyback)
        {
          if (sourceMgr.IsLoanLocked())
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The selected loan is locked by another user. Linking to a Piggyback requires full access rights to the selected loan.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
          if (sourceMgr.LinkedLoan != null && Utils.Dialog((IWin32Window) this, "This loan currently is linking to another loan. Do you want to remove current link?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
            return;
        }
        LoanData loanData = sourceMgr.LoanData;
        if (sourceMgr.LoanName.Replace(".", "") == "")
        {
          int num1 = (int) Utils.Dialog((IWin32Window) this, "An error occurred while attempting to copy the loan '" + loanData.ToPipelineInfo().LoanDisplayString + "'. Contact ICE Mortgage Technology Customer Support at 800-777-1718 for assistance.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          if (isSecond)
            loanData.ChangeLoanToSecond();
          LoanDataMgr loanDataMgr = LoanDataMgr.CopyLoan(Session.SessionObjects, loanData, destinationFolder, sourceMgr.LoanName, templateToUse);
          LoanServiceManager.DuplicateLoan(loanDataMgr, templateToUse);
          LoanServiceManager.DuplicateLoan(loanDataMgr);
          bool flag = true;
          string empty = string.Empty;
          string guid1;
          if (isPiggyback)
          {
            if (!this.linkToPiggybackLoan(sourceMgr, loanDataMgr))
              return;
            guid1 = loanDataMgr.LoanData.GUID;
          }
          else
          {
            sourceMgr.Close();
            flag = loanDataMgr.Save(false);
            guid1 = loanDataMgr.LoanData.GUID;
          }
          sourceMgr.Close();
          loanDataMgr.Close();
          if (flag)
          {
            this.RefreshPipeline(true, 0);
            if (Utils.Dialog((IWin32Window) this, "The loan has been successfully duplicated. Would you like to open the loan now?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
              return;
            this.openLoan(guid1);
          }
          else
            Utils.Dialog((IWin32Window) this, "Loan duplication has failed. Please try again.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
      }
      catch (Exception ex)
      {
        MetricsFactory.IncrementErrorCounter(ex, "Error duplicating loan", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\PipelineScreen.cs", nameof (duplicateLoan), 2820);
        Tracing.Log(PipelineScreen.sw, TraceLevel.Error, nameof (PipelineScreen), "Error duplicating loan: " + (object) ex);
        if (MainScreen.Instance.HandleFileSizeLimitException(ex))
          return;
        int num = (int) Utils.Dialog((IWin32Window) this, "Error duplicating loan: " + ex.Message);
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private bool linkToPiggybackLoan(LoanDataMgr sourceMgr, LoanDataMgr targetMgr)
    {
      Cursor.Current = Cursors.WaitCursor;
      try
      {
        sourceMgr.Lock(LoanInfo.LockReason.OpenForWork, LockInfo.ExclusiveLock.ExclusiveA);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) Session.MainForm, "The selected loan cannot be linked due to this error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (sourceMgr.LinkedLoan != null)
      {
        try
        {
          sourceMgr.LinkedLoan.LoanData.SetCurrentField("LINKGUID", "");
          try
          {
            sourceMgr.LinkedLoan.Save();
            sourceMgr.Unlink();
          }
          catch (Exception ex)
          {
            int num = (int) Utils.Dialog((IWin32Window) Session.MainForm, "The existing piggyback loan information in source file cannot be removed due to this error: " + ex.Message);
            return false;
          }
        }
        catch (Exception ex)
        {
          int num = (int) Utils.Dialog((IWin32Window) Session.MainForm, "The existing piggyback loan information in source file cannot be removed due to this error: " + ex.Message);
          return false;
        }
      }
      targetMgr.LoanData.SetCurrentField("LINKGUID", sourceMgr.LoanData.GUID);
      Tracing.Log(PipelineScreen.sw, TraceLevel.Info, nameof (PipelineScreen), "trying to link the newly created linked loan.");
      sourceMgr.LoanData.SetCurrentField("LINKGUID", targetMgr.LoanData.GUID);
      try
      {
        targetMgr.LinkTo(sourceMgr);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) Session.MainForm, "Can not link the new Piggyback loan.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        Tracing.Log(PipelineScreen.sw, TraceLevel.Info, nameof (PipelineScreen), "trying to link the newly created linked loan. Error: " + ex.Message);
        return false;
      }
      if (sourceMgr.LoanData.Calculator != null)
      {
        sourceMgr.LoanData.Calculator.SkipLockRequestSync = true;
        if (sourceMgr.LoanData.LinkedData != null && sourceMgr.LoanData.LinkedData.Calculator != null)
          sourceMgr.LoanData.LinkedData.Calculator.SkipLockRequestSync = true;
      }
      sourceMgr.LoanData.SyncPiggyBackFiles((string[]) null, true, true, (string) null, (string) null, false);
      sourceMgr.LoanData.Calculator.FormCalculation("4487");
      sourceMgr.LoanData.Calculator.FormCalculation("LinkVoal");
      sourceMgr.SaveLoan(false, (ILoanMilestoneTemplateOrchestrator) null, false);
      sourceMgr.ReleaseExclusiveALock();
      Cursor.Current = Cursors.Default;
      return true;
    }

    private void btnRefresh_Click(object sender, EventArgs e) => this.RefreshPipeline(true, 0);

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (this.gvLoans.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must first select one or more loans from the list.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        bool confirmAllDelete = false;
        bool confirmAllTrash = false;
        bool deleteIt = false;
        foreach (GVItem selectedItem in this.gvLoans.SelectedItems)
        {
          PipelineInfo tag = (PipelineInfo) selectedItem.Tag;
          if (tag != null && tag.AssignedTrade == null)
          {
            if (tag.LoanName.Replace(".", "") == "")
            {
              int num2 = (int) Utils.Dialog((IWin32Window) this, "An error occurred while attempting to delete the loan '" + tag.LoanDisplayString + "'. Contact ICE Mortgage Technology Customer Support at 800-777-1718 for assistance.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              break;
            }
            if (tag.LoanFolder.Equals(SystemSettings.TrashFolder))
              this.deleteLoanFolder(tag, ref confirmAllDelete, ref deleteIt);
            else
              this.trashLoanFolder(tag, ref confirmAllTrash, ref deleteIt);
          }
        }
        this.RefreshPipeline(true, 0);
      }
    }

    private void deleteLoanFolder(PipelineInfo pinfo, ref bool confirmAllDelete, ref bool deleteIt)
    {
      if (!this.notifyUserOfLoansInTrade())
        return;
      try
      {
        bool isLinkedLoan = !string.IsNullOrEmpty(pinfo.LinkGuid);
        string str = "";
        if (!confirmAllDelete)
        {
          if (isLinkedLoan)
            str += "The selected loan has a loan linked to it.\n";
          using (ConfirmDialog confirmDialog = new ConfirmDialog("Delete Loan", str + Messages.GetMessage("PermanentlyDeleteLoanConfirmation", (object) pinfo.LoanDisplayString), this.gvLoans.SelectedItems.Count > 1))
          {
            deleteIt = confirmDialog.ShowDialog() == DialogResult.Yes;
            confirmAllDelete = confirmDialog.ApplyToAll;
          }
        }
        if (!deleteIt)
          return;
        this.deleteLoan(pinfo, isLinkedLoan);
      }
      catch (LockException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The loan '" + pinfo.LoanDisplayString + "' is open for work by someone and cannot be deleted.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      catch (SecurityException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You do not have the necessary permissions to delete the loan '" + pinfo.LoanDisplayString + "'", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      catch (Exception ex)
      {
        MetricsFactory.IncrementErrorCounter(ex, "Unexpected error during delete operation", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\PipelineScreen.cs", nameof (deleteLoanFolder), 3002);
        int num = (int) Utils.Dialog((IWin32Window) this, "Unexpected error during delete operation: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void trashLoanFolder(PipelineInfo pinfo, ref bool confirmAllTrash, ref bool deleteIt)
    {
      string trashFolder = SystemSettings.TrashFolder;
      try
      {
        bool flag = !string.IsNullOrEmpty(pinfo.LinkGuid);
        string str = "";
        if (!confirmAllTrash)
        {
          if (flag)
            str += "The selected loan has a loan linked to it.\n";
          using (ConfirmDialog confirmDialog = new ConfirmDialog("Delete Loan", str + Messages.GetMessage("DeleteLoanConfirmation", (object) pinfo.LoanDisplayString), this.gvLoans.SelectedItems.Count > 1))
          {
            deleteIt = confirmDialog.ShowDialog((IWin32Window) this) == DialogResult.Yes;
            confirmAllTrash = confirmDialog.ApplyToAll;
          }
        }
        if (!deleteIt)
          return;
        Cursor.Current = Cursors.WaitCursor;
        if (flag)
        {
          LoanDataMgr loanDataMgr = LoanDataMgr.OpenLoan(Session.SessionObjects, pinfo.GUID, false);
          if (loanDataMgr != null)
          {
            loanDataMgr.Lock(LoanInfo.LockReason.OpenForWork, LockInfo.ExclusiveLock.ExclusiveA);
            if (loanDataMgr.LinkedLoan != null && loanDataMgr.Calculator != null && loanDataMgr.LinkedLoan.Calculator != null && loanDataMgr.LoanData.GetField("LINKGUID") != "")
            {
              LoanDataMgr linkedLoan = loanDataMgr.LinkedLoan;
              linkedLoan.Lock(LoanInfo.LockReason.OpenForWork, LockInfo.ExclusiveLock.ExclusiveA);
              if (loanDataMgr.LoanData.GetField("420") == "SecondLien")
              {
                linkedLoan.Calculator.FormCalculation("REMOVELINK", (string) null, (string) null);
                loanDataMgr.Calculator.FormCalculation("REMOVELINK", (string) null, (string) null);
              }
              else
              {
                loanDataMgr.Calculator.FormCalculation("REMOVELINK", (string) null, (string) null);
                linkedLoan.Calculator.FormCalculation("REMOVELINK", (string) null, (string) null);
              }
              linkedLoan.LoanData.LinkedData = (LoanData) null;
              linkedLoan.Calculator.FormCalculation("428");
              linkedLoan.SaveLoan(false, (ILoanMilestoneTemplateOrchestrator) null, false);
              loanDataMgr.Unlink();
              loanDataMgr.LoanData.SetCurrentField("LINKGUID", "");
              loanDataMgr.SaveLoan(false, (ILoanMilestoneTemplateOrchestrator) null, false);
              linkedLoan.Unlock();
            }
            loanDataMgr.Unlock();
          }
        }
        Session.LoanManager.MoveLoan(pinfo.Identity, trashFolder, DuplicateLoanAction.Rename);
      }
      catch (Exception ex)
      {
        MetricsFactory.IncrementErrorCounter(ex, "Error during delete operation", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\PipelineScreen.cs", nameof (trashLoanFolder), 3077);
        int num = (int) Utils.Dialog((IWin32Window) this, "The loan '" + pinfo.LoanDisplayString + "' could not be deleted for the following reason: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private void deleteSelectedLoans()
    {
      bool flag = false;
      DialogResult dialogResult = DialogResult.No;
      if (!this.notifyUserOfLoansInTrade())
        return;
      foreach (GVItem selectedItem in this.gvLoans.SelectedItems)
      {
        PipelineInfo tag = (PipelineInfo) selectedItem.Tag;
        if (tag.AssignedTrade == null)
        {
          if (tag.LoanName.Replace(".", "") == "")
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "An error occurred while attempting to delete the loan '" + tag.LoanDisplayString + "'. Contact ICE Mortgage Technology Customer Support at 800-777-1718 for assistance.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            break;
          }
          try
          {
            if (!flag)
            {
              using (ConfirmDialog confirmDialog = new ConfirmDialog("Delete Loan", Messages.GetMessage("PermanentlyDeleteLoanConfirmation", (object) tag.LoanDisplayString), (this.gvLoans.SelectedItems.Count > 1 ? 1 : 0) != 0))
              {
                dialogResult = confirmDialog.ShowDialog();
                flag = confirmDialog.ApplyToAll;
              }
            }
            if (dialogResult == DialogResult.Yes | flag)
              this.deleteLoan(tag);
          }
          catch (LockException ex)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The loan '" + tag.LoanDisplayString + "' is open for work by someone and cannot be deleted.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          }
          catch (SecurityException ex)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "You do not have the necessary permissions to delete the loan '" + tag.LoanDisplayString + "'", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          }
          catch (Exception ex)
          {
            MetricsFactory.IncrementErrorCounter(ex, "Unexpected error during delete operation", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\PipelineScreen.cs", nameof (deleteSelectedLoans), 3140);
            int num = (int) Utils.Dialog((IWin32Window) this, "Unexpected error during delete operation: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            break;
          }
        }
      }
      this.RefreshPipeline(true, 0);
    }

    private bool notifyUserOfLoansInTrade()
    {
      List<PipelineInfo> pipelineInfoList = new List<PipelineInfo>();
      foreach (GVItem selectedItem in this.gvLoans.SelectedItems)
      {
        PipelineInfo tag = (PipelineInfo) selectedItem.Tag;
        if (tag.AssignedTrade != null)
          pipelineInfoList.Add(tag);
      }
      if (pipelineInfoList.Count == 0)
        return true;
      if (this.gvLoans.SelectedItems.Count == 1)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The specified loan is assigned to a trade and cannot be deleted. You must remove the loan from the trade before deleting it.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      int num1 = (int) Utils.Dialog((IWin32Window) this, "The following loans cannot be deleted because they are assigned to a trade." + Environment.NewLine + Environment.NewLine + this.createLoanInfoString(pipelineInfoList.ToArray()) + Environment.NewLine + "You must remove the loans from the trade before deleting them.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      return true;
    }

    private string createLoanInfoString(PipelineInfo[] pinfos)
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < pinfos.Length; ++index)
      {
        string str = pinfos[index].GetField("BorrowerLastName").ToString() + ", " + pinfos[index].GetField("BorrowerFirstName");
        string loanNumber = pinfos[index].LoanNumber;
        if (str != "")
          stringBuilder.Append(str);
        else
          stringBuilder.Append("Borrower Not Specified");
        if (loanNumber != "")
          stringBuilder.Append(" (" + loanNumber + ")");
        stringBuilder.Append(Environment.NewLine);
      }
      return stringBuilder.ToString();
    }

    private void deleteLoan(PipelineInfo pinfo, bool isLinkedLoan = false)
    {
      if (pinfo.LoanName.Replace(".", "") == "")
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "An error occurred while attempting to delete this loan. Contact ICE Mortgage Technology Customer Support at 800-777-1718 for assistance.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        LoanData loanData = (LoanData) null;
        try
        {
          using (ILoan loan = Session.LoanManager.OpenLoan(pinfo.GUID))
            loanData = loan.GetLoanData(false);
          if (isLinkedLoan)
          {
            LoanDataMgr loanDataMgr = LoanDataMgr.OpenLoan(Session.SessionObjects, pinfo.LinkGuid, false);
            if (loanDataMgr != null)
            {
              loanDataMgr.Lock(LoanInfo.LockReason.OpenForWork, LockInfo.ExclusiveLock.ExclusiveA);
              if (loanDataMgr.LoanData.GetField("LINKGUID") != "")
              {
                if (loanDataMgr.LinkedLoan != null)
                {
                  if (loanDataMgr.Calculator != null)
                  {
                    if (loanDataMgr.LinkedLoan.Calculator != null)
                    {
                      if (loanDataMgr.LoanData.GetField("LINKGUID") != "")
                      {
                        if (loanDataMgr.LoanData.GetField("420") == "SecondLien")
                        {
                          loanDataMgr.LinkedLoan.Calculator.FormCalculation("REMOVELINK", (string) null, (string) null);
                          loanDataMgr.Calculator.FormCalculation("REMOVELINK", (string) null, (string) null);
                        }
                        else
                        {
                          loanDataMgr.Calculator.FormCalculation("REMOVELINK", (string) null, (string) null);
                          loanDataMgr.LinkedLoan.Calculator.FormCalculation("REMOVELINK", (string) null, (string) null);
                        }
                        loanDataMgr.Unlink();
                        loanDataMgr.LoanData.SetCurrentField("LINKGUID", "");
                        loanDataMgr.SaveLoan(false, (ILoanMilestoneTemplateOrchestrator) null, false);
                        loanDataMgr.Unlock();
                      }
                    }
                  }
                }
              }
            }
          }
        }
        catch (Exception ex)
        {
        }
        if (loanData == null)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "The selected loan has been deleted or is not longer accessible.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
        {
          PipelineInfo pipelineInfo = Session.LoanManager.GetPipeline(new string[1]
          {
            pinfo.GUID
          }, false, 1)[0];
          if (pipelineInfo == null)
          {
            int num3 = (int) Utils.Dialog((IWin32Window) this, "The selected loan has been deleted or is not longer accessible.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          }
          else
          {
            LoanAccessBpmManager bpmManager = (LoanAccessBpmManager) Session.BPM.GetBpmManager(BpmCategory.LoanAccess);
            if (bpmManager.GetEffectiveRightsForLoan(pipelineInfo) == LoanInfo.Right.Read || bpmManager.GetLoanContentAccess(pipelineInfo, loanData) == LoanContentAccess.None)
            {
              int num4 = (int) Utils.Dialog((IWin32Window) this, "You do not have the necessary rights to delete this loan file. Contact your system administrator if you require further access to this loan.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
              Session.LoanManager.DeleteLoan(pinfo.GUID);
          }
        }
      }
    }

    private void btnPrint_Click(object sender, EventArgs e)
    {
      if (this.gvLoans.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select one or more loans from the list.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
        this.printSelectedLoans();
    }

    private void printSelectedLoans()
    {
      List<string> stringList = new List<string>();
      string[] strArray = (string[]) null;
      foreach (GVItem selectedItem in this.gvLoans.SelectedItems)
      {
        string guid = ((PipelineInfo) selectedItem.Tag).GUID;
        stringList.Add(guid);
        if (strArray == null)
          strArray = new EllieMae.EMLite.LoanServices.Bam(LoanDataMgr.OpenLoan(Session.SessionObjects, guid, false)).GetCompanyDisclosureEntities();
      }
      using (FormSelectorDialog formSelectorDialog = new FormSelectorDialog(Session.DefaultInstance, stringList.ToArray()))
      {
        formSelectorDialog.EntityList = strArray;
        int num = (int) formSelectorDialog.ShowDialog((IWin32Window) this);
      }
    }

    private void btnMove_Click(object sender, EventArgs e)
    {
      if (this.gvLoans.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select one or more loans from the list.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
        this.moveSelectedLoans();
    }

    private void moveSelectedLoans()
    {
      List<string> fromList = (List<string>) null;
      List<string> toList = (List<string>) null;
      bool restoreLoanFromTrashFolder = !this.isRegularFolderSelected() && this.isTrashFolderSelected();
      ((LoanFoldersAclManager) Session.ACL.GetAclManager(AclCategory.LoanFolderMove)).GetAccessibleFoldersForMove(out fromList, out toList);
      toList.Remove(SystemSettings.TrashFolder);
      for (int index = 0; index < this.gvLoans.SelectedItems.Count; ++index)
      {
        PipelineInfo tag = this.gvLoans.SelectedItems[index].Tag as PipelineInfo;
        toList.Remove(tag.LoanFolder);
      }
      if (fromList.Count == 0 || toList.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You are not authorized to move loans between folders.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        string targetFolder = (string) null;
        using (MoveDialog moveDialog = new MoveDialog(toList.ToArray(), restoreLoanFromTrashFolder))
        {
          if (moveDialog.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
            return;
          targetFolder = moveDialog.Destination;
        }
        this.moveSelectedLoans(targetFolder, fromList);
      }
    }

    private void moveSelectedLoans(string targetFolder, List<string> moveLoanAccessibleFoldersFrom)
    {
      LoanFoldersAclManager aclManager = (LoanFoldersAclManager) Session.ACL.GetAclManager(AclCategory.LoanFolderMove);
      LoanFolderRuleManager bpmManager = (LoanFolderRuleManager) Session.BPM.GetBpmManager(BpmCategory.LoanFolder);
      bool flag = false;
      foreach (GVItem selectedItem in this.gvLoans.SelectedItems)
      {
        PipelineInfo tag = (PipelineInfo) selectedItem.Tag;
        if (string.Compare(tag.LoanFolder, targetFolder, true) != 0)
        {
          if (string.Compare(tag.LoanFolder, SystemSettings.TrashFolder, true) == 0)
          {
            if (!Session.ACL.IsAuthorizedForFeature(AclFeature.LoanMgmt_TF_Restore))
            {
              int num = (int) Utils.Dialog((IWin32Window) this, "You do not have the necessary access rights to restore the loan '" + tag.LoanDisplayString + ".'", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              continue;
            }
          }
          else if (moveLoanAccessibleFoldersFrom.FindIndex(new Predicate<string>(new StringPredicate(tag.LoanFolder, true).Compare)) < 0)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The loan '" + tag.LoanDisplayString + "' cannot be moved from folder " + tag.LoanFolder, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            continue;
          }
          if (tag.LoanName.Replace(".", "") == "")
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "An error occurred while attempting to move the loan '" + tag.LoanDisplayString + "'. Contact ICE Mortgage Technology Customer Support at 800-777-1718 for assistance.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            break;
          }
          if (!flag)
          {
            using (ConfirmDialog confirmDialog = new ConfirmDialog("Move Loan", Messages.GetMessage("MoveLoanConfirmation", (object) tag.LoanDisplayString), (this.gvLoans.SelectedItems.Count > 1 ? 1 : 0) != 0))
            {
              DialogResult dialogResult = confirmDialog.ShowDialog((IWin32Window) this);
              flag = confirmDialog.ApplyToAll;
              if (!(dialogResult != DialogResult.Yes & flag))
              {
                if (dialogResult != DialogResult.Yes)
                  continue;
              }
              else
                break;
            }
          }
          try
          {
            if (!(targetFolder == tag.LoanFolder))
            {
              if (!bpmManager.CanMoveToFolder(targetFolder, tag))
              {
                int num1 = (int) Utils.Dialog((IWin32Window) this, "Loan '" + tag.LoanDisplayString + "' is not allowed to be moved to folder '" + targetFolder + "'.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              }
              else
              {
                try
                {
                  Cursor.Current = Cursors.WaitCursor;
                  Session.LoanManager.MoveLoan(tag.Identity, targetFolder, DuplicateLoanAction.Rename);
                }
                catch (Exception ex)
                {
                  MetricsFactory.IncrementErrorCounter(ex, "Unexpected error during loan move", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\PipelineScreen.cs", nameof (moveSelectedLoans), 3499);
                  int num2 = (int) Utils.Dialog((IWin32Window) this, "The loan '" + tag.LoanDisplayString + "' could not be moved for the following reason: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                finally
                {
                  Cursor.Current = Cursors.Default;
                }
              }
            }
          }
          catch (Exception ex)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The loan '" + tag.LoanDisplayString + "' could not be moved for the following reason: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
        }
      }
      this.RefreshPipeline(true, 0);
    }

    private void trashSelectedLoans()
    {
      string trashFolder = SystemSettings.TrashFolder;
      bool flag = false;
      foreach (GVItem selectedItem in this.gvLoans.SelectedItems)
      {
        PipelineInfo tag = selectedItem.Tag as PipelineInfo;
        if (tag.LoanName.Replace(".", "") == "")
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "An error occurred while attempting to delete the loan '" + tag.LoanDisplayString + "'. Contact ICE Mortgage Technology Customer Support at 800-777-1718 for assistance.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          break;
        }
        if (!flag)
        {
          using (ConfirmDialog confirmDialog = new ConfirmDialog("Delete Loan", Messages.GetMessage("DeleteLoanConfirmation", (object) tag.LoanDisplayString), (this.gvLoans.SelectedItems.Count > 1 ? 1 : 0) != 0))
          {
            DialogResult dialogResult = confirmDialog.ShowDialog((IWin32Window) this);
            flag = confirmDialog.ApplyToAll;
            if (!(dialogResult != DialogResult.Yes & flag))
            {
              if (dialogResult != DialogResult.Yes)
                continue;
            }
            else
              break;
          }
        }
        if (!(trashFolder == tag.LoanFolder))
        {
          try
          {
            Cursor.Current = Cursors.WaitCursor;
            Session.LoanManager.MoveLoan(tag.Identity, trashFolder, DuplicateLoanAction.Rename);
          }
          catch (Exception ex)
          {
            MetricsFactory.IncrementErrorCounter(ex, "Error during delete operation", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\PipelineScreen.cs", nameof (trashSelectedLoans), 3568);
            int num = (int) Utils.Dialog((IWin32Window) this, "The loan '" + tag.LoanDisplayString + "' could not be deleted for the following reason: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          finally
          {
            Cursor.Current = Cursors.Default;
          }
        }
      }
      this.RefreshPipeline(true, 0);
    }

    private void btnTransfer_Click(object sender, EventArgs e)
    {
      if (this.gvLoans.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select one or more loans from the list.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
        this.transferSelectedLoans();
    }

    private void transferSelectedLoans()
    {
      string fileList = "";
      string guidList = "";
      foreach (GVItem selectedItem in this.gvLoans.SelectedItems)
      {
        PipelineInfo tag = (PipelineInfo) selectedItem.Tag;
        if (tag != null)
        {
          if (fileList == "")
          {
            fileList = tag.LoanName;
            guidList = tag.GUID;
          }
          else
          {
            fileList = fileList + "|" + tag.LoanName;
            guidList = guidList + "|" + tag.GUID;
          }
        }
      }
      using (TransferDialog transferDialog = new TransferDialog(fileList, guidList))
      {
        int num = (int) transferDialog.ShowDialog((IWin32Window) this);
      }
    }

    private void btnExport_Click(object sender, EventArgs e) => this.exportPipeline(false);

    private void mnuExportAll_Click(object sender, EventArgs e) => this.exportPipeline(true);

    private bool exportPipeline(bool exportAll)
    {
      if (this.gvLoans.Columns.Count > ExcelHandler.GetMaximumColumnCount())
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You pipeline cannot be exported because the number of columns exceeds the limit supported by Excel (" + (object) ExcelHandler.GetMaximumColumnCount() + ")", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      if ((!exportAll ? this.gvLoans.SelectedItems.Count : this.pipelineCursor.GetItemCount()) > ExcelHandler.GetMaximumRowCount() - 1)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You pipeline cannot be exported because the number of rows exceeds the limit supported by Excel (" + (object) ExcelHandler.GetMaximumRowCount() + ")", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      if (exportAll)
        this.exportAllLoansToExcel();
      else
        this.exportSelectedRowsToExcel();
      return true;
    }

    private void mnuExportLEF_Click(object sender, EventArgs e) => this.exportLEFPipeline(false);

    private void mnuExportLEFAll_Click(object sender, EventArgs e) => this.exportLEFPipeline(true);

    private bool exportLEFPipeline(bool exportAll)
    {
      using (CursorActivator.Wait())
      {
        List<string> stringList = new List<string>();
        if (exportAll)
        {
          foreach (PipelineInfo pipelineInfo in new EnumerableCursor(this.pipelineCursor, false))
            stringList.Add(pipelineInfo.GUID);
        }
        else
        {
          foreach (GVItem selectedItem in this.gvLoans.SelectedItems)
          {
            PipelineInfo tag = (PipelineInfo) selectedItem.Tag;
            stringList.Add(tag.GUID);
          }
        }
        if (stringList.Count != 0)
          return new ExportLEFData().Export(Session.LoanDataMgr, stringList.ToArray());
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a loan to export.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
    }

    private bool investorStandardExportPipeline(ServiceSetting setting)
    {
      using (CursorActivator.Wait())
      {
        List<string> stringList = new List<string>();
        if (string.Concat(setting.Tag) == "All")
        {
          foreach (PipelineInfo pipelineInfo in new EnumerableCursor(this.pipelineCursor, false))
            stringList.Add(pipelineInfo.GUID);
        }
        else
        {
          foreach (GVItem selectedItem in this.gvLoans.SelectedItems)
          {
            PipelineInfo tag = (PipelineInfo) selectedItem.Tag;
            stringList.Add(tag.GUID);
          }
        }
        if (stringList.Count == 0)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Please select a loan to export.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return false;
        }
        if (Session.LoanData != null)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Please exit all loan files before exporting.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return false;
        }
        ILoanServices service = Session.Application.GetService<ILoanServices>();
        bool flag;
        if (setting.UseStandardValidationGrid)
        {
          InvestorExportDialog.Instance.Initialize(this.fieldDefs, stringList.ToArray(), this.layoutManager.GetCurrentLayout(), setting);
          if (!InvestorExportDialog.PassDataCheck && DialogResult.Cancel == InvestorExportDialog.Instance.ShowDialog((IWin32Window) this))
            return false;
          flag = service.ExportServiceExportData(Session.LoanDataMgr, setting, stringList.ToArray());
        }
        else
          flag = !string.IsNullOrEmpty(setting.FilePath) ? service.ExportServiceProcessLoans(Session.LoanDataMgr, setting, stringList.ToArray()) : service.ExportServiceExportData(Session.LoanDataMgr, setting, stringList.ToArray());
        return flag;
      }
    }

    private void exportSelectedRowsToExcel()
    {
      ExcelHandler excelHandler = new ExcelHandler();
      excelHandler.AddDataTable(this.gvLoans, (ReportFieldDefs) this.fieldDefs, true);
      excelHandler.CreateExcel();
    }

    private void exportAllLoansToExcel()
    {
      using (CursorActivator.Wait())
      {
        ExcelHandler excelHandler = new ExcelHandler();
        foreach (GVColumn c in this.gvLoans.Columns.DisplaySequence)
          excelHandler.AddHeaderColumn(c.Text, excelHandler.GetColumnFormat(c, (ReportFieldDefs) this.fieldDefs));
        foreach (PipelineInfo pinfo in new EnumerableCursor(this.pipelineCursor, false))
        {
          GVItem itemForPipelineInfo = this.createGVItemForPipelineInfo(pinfo, LoanInfo.Right.Read);
          string[] data = new string[this.gvLoans.Columns.Count];
          for (int index = 0; index < this.gvLoans.Columns.Count; ++index)
            data[index] = itemForPipelineInfo.SubItems[this.gvLoans.Columns.DisplaySequence[index].Index].Text;
          excelHandler.AddDataRow(data);
        }
        excelHandler.CreateExcel();
      }
    }

    private void gvLoans_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.setButtonEnabledStates();
      if (this.gvLoans.SelectedItems.Count == 1 && this.gvLoans.SelectedItems[0].Tag is PipelineInfo tag && !string.IsNullOrWhiteSpace(tag.LoanNumber))
        this.aiqHelper.EnableAIQLaunchButton(tag.GUID);
      if (this.gvLoans.SelectedItems.Count != 0)
        return;
      this.btn_LaunchAIQ.Visible = false;
    }

    private void setButtonEnabledStates()
    {
      bool flag1 = this.isRegularFolderSelected() && !this.isTrashFolderSelected();
      bool flag2 = !this.isRegularFolderSelected() && this.isTrashFolderSelected();
      this.btnEdit.Enabled = this.gvLoans.SelectedItems.Count == 1;
      this.btnDelete.Enabled = this.gvLoans.SelectedItems.Count > 0;
      this.btnPrint.Enabled = this.gvLoans.SelectedItems.Count > 0;
      this.btnMove.Enabled = this.gvLoans.SelectedItems.Count > 0 && flag1 | flag2;
      this.btnExport.Enabled = this.gvLoans.SelectedItems.Count > 0;
      this.btnTransfer.Enabled = this.gvLoans.SelectedItems.Count > 0 & flag1;
      this.btnNotifyUsers.Enabled = this.gvLoans.SelectedItems.Count > 0 & flag1;
      this.mnuManageAlerts.Enabled = this.gvLoans.SelectedItems.Count == 1 & flag1;
      this.mnuExportDocsSelectedLoans.Enabled = this.gvLoans.SelectedItems.Count > 0 & flag1;
      this.mnuOpenWebViewLoan.Enabled = this.gvLoans.SelectedItems.Count == 1;
      ExportServicesAclManager aclManager = (ExportServicesAclManager) Session.ACL.GetAclManager(AclCategory.ExportServices);
      this.btnDuplicate.Enabled = false;
      this.toolStripMenuItem1.Enabled = false;
      if (this.exportFannieMae != null)
        this.exportFannieMae.Enabled = false;
      if (this.gvLoans.SelectedItems.Count == 1)
      {
        LoanFolderRuleManager bpmManager = (LoanFolderRuleManager) Session.BPM.GetBpmManager(BpmCategory.LoanFolder);
        if (this.gvLoans.SelectedItems[0].Tag is PipelineInfo tag && bpmManager.IsActionPermitted(tag.LoanFolder, LoanFolderAction.DuplicateFrom))
          this.btnDuplicate.Enabled = true;
        if (Session.UserInfo.IsSuperAdministrator() || this.aclMgr.GetUserApplicationRight(AclFeature.LoanMgmt_Export_FannieMae_FormattedFile) && aclManager.GetUserApplicationRightForPipelineServices("GSE Services"))
        {
          this.toolStripMenuItem1.Enabled = true;
          if (this.exportFannieMae != null)
            this.exportFannieMae.Enabled = true;
        }
        else if (this.exportFannieMae != null)
          this.exportFannieMae.Visible = false;
      }
      if (this.gvLoans.SelectedItems.Count == 0)
        this.btn_LaunchAIQ.Visible = false;
      if (!this.selectLoanOnly)
        return;
      this.DisabledButtons();
    }

    private void applySpecialFolderSecurity()
    {
      FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      if (this.cboFolder.ItemList.CheckedItems.Count > 0 && this.isTrashFolderSelected())
      {
        this.btnDelete.Visible = aclManager.GetUserApplicationRight(AclFeature.LoanMgmt_TF_Delete);
        this.btnMove.Visible = aclManager.GetUserApplicationRight(AclFeature.LoanMgmt_TF_Restore);
      }
      else
      {
        this.btnDelete.Visible = aclManager.GetUserApplicationRight(AclFeature.LoanMgmt_Delete);
        this.btnMove.Visible = this.canMoveLoans();
      }
    }

    private void applyUserAccessSecurity()
    {
      UserInfo userInfo = Session.UserInfo;
      FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      if (!aclManager.GetUserApplicationRight(AclFeature.LoanMgmt_CreateBlank) && !aclManager.GetUserApplicationRight(AclFeature.LoanMgmt_CreateFromTmpl))
        this.btnNew.Visible = false;
      if (!aclManager.GetUserApplicationRight(AclFeature.LoanMgmt_Duplicate) && !aclManager.GetUserApplicationRight(AclFeature.LoanMgmt_Duplicate_For_Second))
        this.btnDuplicate.Visible = false;
      if (!aclManager.GetUserApplicationRight(AclFeature.LoansTab_Print_PrintButton))
        this.btnPrint.Visible = false;
      this.applySpecialFolderSecurity();
      if (!aclManager.GetUserApplicationRight(AclFeature.LoanMgmt_Transfer))
        this.btnTransfer.Visible = false;
      if (aclManager.GetUserApplicationRight(AclFeature.LoanMgmt_ExportToExcel))
        return;
      this.btnExport.Visible = false;
    }

    private bool canMoveLoans()
    {
      if (Session.UserInfo.IsSuperAdministrator())
        return true;
      LoanFolderAclInfo[] accessibleFoldersForMove = Session.StartupInfo.AccessibleFoldersForMove;
      if (accessibleFoldersForMove == null)
        return false;
      foreach (LoanFolderAclInfo loanFolderAclInfo in accessibleFoldersForMove)
      {
        if (loanFolderAclInfo.MoveFromAccess == 1 || loanFolderAclInfo.MoveToAccess == 1)
          return true;
      }
      return false;
    }

    private void gvLoans_SortItems(object source, GVColumnSortEventArgs e)
    {
      if (this.suspendEvents)
        return;
      using (CursorActivator.Wait())
      {
        if (!this.retrievePipelineData((QueryCriterion) null, this.getSortFieldsForColumnSort(e.ColumnSorts)))
        {
          e.Cancel = true;
        }
        else
        {
          this.displayCurrentPage(true);
          this.setViewChanged(true);
        }
      }
    }

    private void btnRefreshView_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to reset the selected view?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      this.SetCurrentView(this.currentView, this.fsViewEntry);
    }

    public void DisplayPipeline(string folderName, string viewName)
    {
      this.DisplayPipeline(folderName, viewName, 0);
    }

    public void DisplayPipeline(string folderName, string viewName, int sqlRead)
    {
      this.DisplayPipeline(folderName, viewName, sqlRead, false);
    }

    public void DisplayPipeline(
      string folderName,
      string viewName,
      int sqlRead,
      bool fromHomePage)
    {
      this.suspendEvents = true;
      this.advFilter = (FieldFilterList) null;
      this.filterManager.ClearColumnFilters();
      this.suspendEvents = false;
      if ((folderName ?? "") == "")
        folderName = SystemSettings.AllFolders;
      foreach (ComboBoxItem comboBoxItem in (ListBox.ObjectCollection) this.cboFolder.Items)
      {
        LoanFolderInfo tag = (LoanFolderInfo) comboBoxItem.Tag;
        if (string.Compare(tag.Name, folderName, true) == 0 || string.Compare(tag.DisplayName, folderName, true) == 0)
        {
          ClientCommonUtils.CheckLoanFolder(this.cboFolder, folderName, CheckState.Checked);
          this.currentViewFolders = folderName;
          break;
        }
      }
      foreach (PipelineViewList pipelineViewList in this.cboView.Items)
      {
        if (string.Compare(viewName, pipelineViewList.ToString(), true) == 0 && this.cboView.SelectedItem != pipelineViewList)
        {
          if (fromHomePage)
            this.clearCurrentFolder = false;
          this.cboView.SelectedItem = (object) pipelineViewList;
          this.clearCurrentFolder = true;
          return;
        }
      }
      this.SetCurrentView(this.currentView, this.fsViewEntry, sqlRead);
    }

    public void DisplayAlertMessage() => this.showAlertPerformanceMessage();

    public void DisplayPipeline(
      string loanFolder,
      bool usersLoansOnly,
      FieldFilterList filters,
      SortField[] sortFields)
    {
      this.DisplayPipeline(loanFolder, usersLoansOnly, filters, sortFields, 0);
    }

    public void DisplayPipeline(
      bool usersLoansOnly,
      FieldFilterList filters,
      SortField[] sortFields,
      int sqlRead)
    {
      this.DisplayPipeline(SystemSettings.AllFolders, usersLoansOnly, filters, sortFields, sqlRead);
    }

    public void DisplayPipeline(
      bool usersLoansOnly,
      FieldFilterList filters,
      SortField[] sortFields)
    {
      this.DisplayPipeline(SystemSettings.AllFolders, usersLoansOnly, filters, sortFields, 0);
    }

    public void DisplayPipeline(
      string loanFolder,
      bool usersLoansOnly,
      FieldFilterList filters,
      SortField[] sortFields,
      int sqlRead)
    {
      this.suspendEvents = true;
      using (CursorActivator.Wait())
      {
        ClientCommonUtils.PopulateLoanFolderDropdown((ComboBox) this.cboFolder, new LoanFolderInfo(loanFolder), false);
        if (!usersLoansOnly)
          this.cboLoanType.SelectedIndex = 0;
        else
          this.cboLoanType.SelectedIndex = 1;
        this.filterManager.ClearColumnFilters();
        this.advFilter = filters;
        this.refreshFilterDescription();
        this.gvLoans.ClearSort();
        if (!this.retrievePipelineData((QueryCriterion) null, sortFields, sqlRead))
          return;
        this.displayCurrentPage(false, sqlRead);
      }
    }

    private void mnuLoans_Opening(object sender, CancelEventArgs e)
    {
      this.mnuNew.Visible = this.btnNew.Visible;
      this.mnuNew.Enabled = this.btnNew.Enabled;
      this.mnuOpen.Visible = this.btnEdit.Visible;
      this.mnuOpen.Enabled = this.btnEdit.Enabled;
      this.mnuDuplicate.Visible = this.btnDuplicate.Visible;
      this.mnuDuplicate.Enabled = this.btnDuplicate.Enabled;
      this.mnuMove.Visible = this.btnMove.Visible;
      this.mnuMove.Enabled = this.btnMove.Enabled;
      this.mnuTransfer.Visible = this.btnTransfer.Visible;
      this.mnuTransfer.Enabled = this.btnTransfer.Enabled;
      this.mnuDelete.Visible = this.btnDelete.Visible;
      this.mnuDelete.Enabled = this.btnDelete.Enabled;
      this.mnuPrint.Visible = this.btnPrint.Visible;
      this.mnuPrint.Enabled = this.btnPrint.Enabled;
      this.mnuExport.Visible = this.btnExport.Visible;
      this.mnuExportSelected.Enabled = this.btnExport.Enabled;
      this.mnuExportAll.Enabled = this.gvLoans.Items.Count > 0;
      this.mnuManageAlerts.Enabled = this.gvLoans.SelectedItems.Count == 1;
      this.mnuExportDocsSelectedLoans.Enabled = this.gvLoans.SelectedItems.Count > 0;
      this.mnuLoanProperties.Enabled = this.gvLoans.SelectedItems.Count == 1;
      this.mnuSelectAll.Enabled = this.gvLoans.Items.Count > 0;
      this.mnuDelete.Text = "&Delete Loan" + (this.gvLoans.SelectedItems.Count > 1 ? "s" : "");
      if (this.mnuMove.Visible)
        this.mnuMove.Text = this.btnMove.Text;
      if (this.mnuMove.Enabled)
      {
        List<string> fromList;
        List<string> toList;
        ((LoanFoldersAclManager) Session.ACL.GetAclManager(AclCategory.LoanFolderMove)).GetAccessibleFoldersForMove(out fromList, out toList);
        toList.Remove(SystemSettings.TrashFolder);
        for (int index = 0; index < this.gvLoans.SelectedItems.Count; ++index)
        {
          PipelineInfo tag = this.gvLoans.SelectedItems[index].Tag as PipelineInfo;
          toList.Remove(tag == null ? "" : tag.LoanFolder);
        }
        this.mnuMove.Tag = (object) fromList;
        this.mnuMove.DropDownItems.Clear();
        ToolStripItem[] toolStripItems = new ToolStripItem[toList.Count];
        int index1 = 0;
        foreach (string text in toList)
        {
          ToolStripItem toolStripItem = (ToolStripItem) new ToolStripMenuItem(text, (Image) null, new EventHandler(this.onMoveFolderMenuClick));
          toolStripItem.Tag = (object) text;
          toolStripItems[index1] = toolStripItem;
          ++index1;
        }
        this.mnuMove.DropDownItems.AddRange(toolStripItems);
        ((ToolStripDropDownMenu) this.mnuMove.DropDown).ShowImageMargin = false;
      }
      this.mnuDivNewEdit.Visible = this.btnNew.Visible || this.btnEdit.Visible || this.btnDuplicate.Visible;
      this.mnuDivMoveDelete.Visible = this.btnMove.Visible || this.btnTransfer.Visible || this.btnDelete.Visible;
      this.mnuDivPrint.Visible = true;
      this.disableServiceMenuItemsForMultipleLoanSelection(this.mnuLoans.Items);
    }

    private void disableServiceMenuItemsForMultipleLoanSelection(
      ToolStripItemCollection itemsToLookIn)
    {
      if (itemsToLookIn == null || itemsToLookIn.Count <= 0)
        return;
      for (int index = 0; index < itemsToLookIn.Count; ++index)
      {
        if (itemsToLookIn[index] is ToolStripMenuItem toolStripMenuItem)
        {
          if (toolStripMenuItem.Tag is ServiceSetting tag && tag.DisableMenuItemForMultipleLoanSelection)
            toolStripMenuItem.Enabled = this.gvLoans.SelectedItems.Count == 1;
          if (toolStripMenuItem.Enabled && toolStripMenuItem.HasDropDownItems)
            this.disableServiceMenuItemsForMultipleLoanSelection(toolStripMenuItem.DropDownItems);
        }
      }
    }

    private void onMoveFolderMenuClick(object sender, EventArgs e)
    {
      this.moveSelectedLoans((string) ((ToolStripItem) sender).Tag, (List<string>) this.mnuMove.Tag);
    }

    private void mnuDuplicate_Click(object sender, EventArgs e) => this.duplicateSelectedLoan();

    private void mnuManageAlerts_Click(object sender, EventArgs e)
    {
      this.manageAlerts(this.gvLoans.SelectedItems[0].Tag as PipelineInfo);
    }

    private void manageAlerts(PipelineInfo pinfo)
    {
      using (AlertDialog alertDialog = new AlertDialog(pinfo))
      {
        int num = (int) alertDialog.ShowDialog((IWin32Window) this);
      }
    }

    private void mnuExportDocsSelectedLoans_Click(object sender, EventArgs e)
    {
      this.exportDocuments(false);
    }

    private void mnuExportDocsAllLoans_Click(object sender, EventArgs e)
    {
      this.exportDocuments(true);
    }

    private void exportDocuments(bool exportAll)
    {
      try
      {
        if (!Modules.IsModuleAvailableForUser(EncompassModule.EDM, true))
          return;
        List<string> stringList = new List<string>();
        if (exportAll)
        {
          foreach (PipelineInfo pipelineInfo in new EnumerableCursor(this.pipelineCursor, false))
            stringList.Add(pipelineInfo.GUID);
        }
        else
        {
          foreach (GVItem selectedItem in this.gvLoans.SelectedItems)
          {
            PipelineInfo tag = (PipelineInfo) selectedItem.Tag;
            stringList.Add(tag.GUID);
          }
        }
        if (stringList.Count == 0)
        {
          int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a loan to export documents.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else if (Session.ConfigurationManager.GetTemplateDirEntries(EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentExportTemplate, FileSystemEntry.PublicRoot).Length == 0)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "At least one Document Export Template must be set up before you can export documents.");
        }
        else
        {
          string companySetting = Session.ConfigurationManager.GetCompanySetting("ExportTemplate", "Default");
          FileSystemEntry defaultEntry;
          try
          {
            defaultEntry = FileSystemEntry.Parse(companySetting);
          }
          catch (Exception ex)
          {
            int num3 = (int) Utils.Dialog((IWin32Window) this, "One Document Export Template must be designated as Default before you can export documents.");
            Tracing.Log(PipelineScreen.sw, nameof (PipelineScreen), TraceLevel.Error, "Error retrieving Default Export Template: " + (object) ex);
            return;
          }
          if (BackgroundAttachmentDialog.IsProcessing(stringList.ToArray()))
          {
            int num4 = (int) Utils.Dialog((IWin32Window) this, "Selected loans have files waiting to be attached.\r\nYou cannot export until the files have been attached.");
          }
          else
          {
            using (SelectDocumentsDialog selectDocumentsDialog = new SelectDocumentsDialog(LoanDataMgr.OpenLoan(Session.SessionObjects, stringList[0], false), stringList.ToArray(), defaultEntry))
            {
              if (selectDocumentsDialog.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.Cancel)
                ;
            }
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(PipelineScreen.sw, nameof (PipelineScreen), TraceLevel.Error, "Error exporting documents: " + (object) ex);
        ErrorDialog.Display(ex);
      }
    }

    private void setViewChanged(bool modified)
    {
      this.btnSaveView.Enabled = modified && this.hasManagePipelineViewRights;
      this.btnRefreshView.Enabled = modified;
    }

    private void gvLoans_ColumnReorder(object source, GVColumnEventArgs e)
    {
      this.setViewChanged(true);
    }

    private void gvLoans_ColumnResize(object source, GVColumnEventArgs e)
    {
      this.setViewChanged(true);
    }

    private void mnuLoanProperties_Click(object sender, EventArgs e)
    {
      LoanProperties loanProperties = Session.LoanManager.GetLoanProperties((this.gvLoans.SelectedItems[0].Tag as PipelineInfo).GUID);
      if (loanProperties == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "The specified loan no longer exists or cannot be accessed.");
      }
      else
      {
        using (LoanPropertiesDialog propertiesDialog = new LoanPropertiesDialog(loanProperties))
        {
          int num2 = (int) propertiesDialog.ShowDialog((IWin32Window) this);
        }
      }
    }

    private void btnManageViews_Click(object sender, EventArgs e)
    {
      using (ViewManagementDialog managementDialog = new ViewManagementDialog(EllieMae.EMLite.ClientServer.TemplateSettingsType.PipelineView, false, "Pipeline.DefaultView"))
      {
        foreach (PipelineViewList view in this.cboView.Items)
          managementDialog.AddUserView(view);
        int num = (int) managementDialog.ShowDialog((IWin32Window) this);
      }
      this.RefreshViews(true);
      this.RefreshCurrentView(1);
    }

    private void onEPassMessageActivity(object sender, EPassMessageEventArgs eventArgs)
    {
      if (eventArgs.EventType == EPassMessageEventType.MessagesSynced)
      {
        this.refreshLoanMailboxNotification();
      }
      else
      {
        if (eventArgs.Message == null || !eventArgs.Message.IsLoanMailboxMessage)
          return;
        this.refreshLoanMailboxNotification();
      }
    }

    private void refreshLoanMailboxNotification()
    {
      if (this.InvokeRequired)
      {
        this.Invoke((Delegate) new MethodInvoker(this.refreshLoanMailboxNotification));
      }
      else
      {
        int messageCountForUser = Session.ConfigurationManager.GetLoanMailboxMessageCountForUser(Session.UserID);
        if (messageCountForUser == 0)
        {
          this.elmMailbox.Visible = false;
        }
        else
        {
          ((AlertMessageLabel) this.elmMailbox.Element).Text = messageCountForUser.ToString();
          this.elmMailbox.Visible = true;
          this.elmMailbox.Invalidate();
        }
      }
    }

    private void onOpenLoanMailbox(object sender, EventArgs e)
    {
      Session.Application.GetService<IEncompassApplication>().OpenLoanMailbox();
    }

    private void mnuSelectAll_Click(object sender, EventArgs e) => this.gvLoans.Items.SelectAll();

    private void tmrRefresh_Tick(object sender, EventArgs e)
    {
      int num = -1;
      try
      {
        num = PipelineSettings.RefreshInterval;
      }
      catch
      {
      }
      if (num <= 0 || MainScreen.Instance.IsModalDialogOpen())
        return;
      TimeSpan timeSpan = DateTime.Now - this.lastRefreshTime;
      if (timeSpan.TotalSeconds >= (double) num)
      {
        this.RefreshPipeline(true, true, 1);
        this.tmrRefresh.Interval = num * 1000;
        Console.WriteLine(DateTime.Now.ToString() + " -> Pipeline Refreshed");
      }
      else
        this.tmrRefresh.Interval = (num - (int) timeSpan.TotalSeconds) * 1000;
    }

    private void importLoans()
    {
      using (ImportMain importMain = new ImportMain())
      {
        importMain.LoanImported += new EventHandler(this.importMainForm_LoanImported);
        int num = (int) importMain.ShowDialog((IWin32Window) MainScreen.Instance);
      }
      this.RefreshPipeline(true, 0);
    }

    private void importMainForm_LoanImported(object sender, EventArgs e)
    {
      LoanDataMgr loanDataMgr = (LoanDataMgr) sender;
      if (loanDataMgr == null || loanDataMgr.LoanData == null)
        return;
      BorrowerPair[] borrowerPairs = loanDataMgr.LoanData.GetBorrowerPairs();
      if (borrowerPairs == null || borrowerPairs.Length == 0)
        return;
      for (int index = 0; index < borrowerPairs.Length; ++index)
        LoanServiceManager.NewBorrowerPairCreated(borrowerPairs[index].Id, loanDataMgr);
    }

    private void generateNMLSReport()
    {
      if (!NMLSReportForm.ValidateRequiredFields(NMLSReportType.Standard))
        return;
      using (NMLSReportForm nmlsReportForm = new NMLSReportForm())
      {
        int num = (int) nmlsReportForm.ShowDialog((IWin32Window) this);
      }
    }

    private void generateNCMLDReport()
    {
      if (!NCMLDReportForm.ValidateRequiredFields())
        return;
      using (NCMLDReportForm ncmldReportForm = new NCMLDReportForm())
      {
        int num = (int) ncmldReportForm.ShowDialog((IWin32Window) this);
      }
    }

    private void generateHMDAReport(ServiceSetting setting)
    {
      Session.Application.GetService<ILoanServices>().ExportServiceProcessLoans(Session.LoanDataMgr, setting, new string[0]);
    }

    private async void btnOpportunities_ClickAsync(object sender, EventArgs e)
    {
      await DeepLinkLauncher.LaunchWebAppInBrowserAsync(DeepLinkType.Opportunities, (IDeepLinkContext) new PipelineContext());
    }

    private async void btnProspects_ClickAsync(object sender, EventArgs e)
    {
      await DeepLinkLauncher.LaunchWebAppInBrowserAsync(DeepLinkType.Prospects, (IDeepLinkContext) new PipelineContext());
    }

    private async void btnOpenWebViewLoan_ClickAsync(object sender, EventArgs e)
    {
      if (this.gvLoans == null || this.gvLoans.SelectedItems.Count != 1 || !(this.gvLoans.SelectedItems[0].Tag is PipelineInfo tag))
        return;
      await DeepLinkLauncher.LaunchWebAppInBrowserAsync(DeepLinkType.LoanDefaultPage, (IDeepLinkContext) new PipelineLoanContext(tag.GUID));
    }

    public void MenuClicked(ToolStripItem menuItem)
    {
      if (menuItem.Tag is ServiceSetting)
      {
        ServiceSetting tag = (ServiceSetting) menuItem.Tag;
        switch (tag.ID.ToLower())
        {
          case "lef":
            if (string.Concat(tag.Tag) != "All")
            {
              this.mnuExportLEF_Click((object) null, (EventArgs) null);
              break;
            }
            this.mnuExportLEFAll_Click((object) null, (EventArgs) null);
            break;
          case "nmls":
            this.generateNMLSReport();
            break;
          case "ncarcompliancereport":
            this.generateNCMLDReport();
            break;
          case "hmda":
            this.generateHMDAReport(tag);
            break;
          default:
            this.investorStandardExportPipeline(tag);
            break;
        }
      }
      else
      {
        string s = string.Concat(menuItem.Tag);
        // ISSUE: reference to a compiler-generated method
        switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(s))
        {
          case 43775930:
            if (!(s == "PI_Refresh"))
              break;
            this.btnRefresh_Click((object) null, (EventArgs) null);
            break;
          case 192027707:
            if (!(s == "PI_Edit"))
              break;
            this.btnEdit_Click((object) null, (EventArgs) null);
            break;
          case 229759188:
            if (!(s == "PI_ExportSelected"))
              break;
            this.btnExport_Click((object) null, (EventArgs) null);
            break;
          case 816557528:
            if (!(s == "PI_OpenWebViewLoan"))
              break;
            this.btnOpenWebViewLoan_ClickAsync((object) null, (EventArgs) null);
            break;
          case 958330119:
            if (!(s == "PI_ResetView"))
              break;
            this.btnRefreshView_Click((object) null, (EventArgs) null);
            break;
          case 1164481484:
            if (!(s == "PI_Transfer"))
              break;
            this.btnTransfer_Click((object) null, (EventArgs) null);
            break;
          case 1874144292:
            if (!(s == "PI_Opportunities"))
              break;
            this.btnOpportunities_ClickAsync((object) null, (EventArgs) null);
            break;
          case 2078888238:
            if (!(s == "PI_Print"))
              break;
            this.btnPrint_Click((object) null, (EventArgs) null);
            break;
          case 2278446596:
            if (!(s == "PI_Delete"))
              break;
            this.btnDelete_Click((object) null, (EventArgs) null);
            break;
          case 2498446807:
            if (!(s == "PI_New"))
              break;
            this.btnNew_Click((object) null, (EventArgs) null);
            break;
          case 3257414178:
            if (!(s == "PI_Columns"))
              break;
            this.layoutManager.CustomizeColumns();
            break;
          case 3431196638:
            if (!(s == "PI_ManageViews"))
              break;
            this.btnManageViews_Click((object) null, (EventArgs) null);
            break;
          case 3578892022:
            if (!(s == "PI_Import"))
              break;
            this.importLoans();
            break;
          case 3799046446:
            if (!(s == "PI_Move"))
              break;
            this.btnMove_Click((object) null, (EventArgs) null);
            break;
          case 4092998664:
            if (!(s == "PI_Duplicate"))
              break;
            this.btnDuplicate_Click((object) null, (EventArgs) null);
            break;
          case 4202687132:
            if (!(s == "PI_Prospects"))
              break;
            this.btnProspects_ClickAsync((object) null, (EventArgs) null);
            break;
          case 4210065643:
            if (!(s == "PI_SaveView"))
              break;
            this.btnSaveView_Click((object) null, (EventArgs) null);
            break;
          case 4279509156:
            if (!(s == "PI_ExportAll"))
              break;
            this.mnuExportAll_Click((object) null, (EventArgs) null);
            break;
          case 4281834431:
            if (!(s == "PI_ManageAlerts"))
              break;
            this.mnuManageAlerts_Click((object) null, (EventArgs) null);
            break;
        }
      }
    }

    public bool SetMenuItemState(ToolStripItem menuItem)
    {
      Control stateControl = (Control) null;
      switch (string.Concat(menuItem.Tag))
      {
        case "PI_Delete":
          stateControl = (Control) this.btnDelete;
          break;
        case "PI_Duplicate":
          stateControl = (Control) this.btnDuplicate;
          break;
        case "PI_Edit":
          stateControl = (Control) this.btnEdit;
          break;
        case "PI_Export":
          return Session.ACL.IsAuthorizedForFeature(AclFeature.LoanMgmt_ExportToExcel);
        case "PI_ExportAll":
          menuItem.Enabled = this.gvLoans.Items.Count > 0;
          return true;
        case "PI_ExportLoan":
          menuItem.Enabled = this.isClientEnabledToExportFNMFRE();
          return this.isClientEnabledToExportFNMFRE();
        case "PI_ExportSelected":
          stateControl = (Control) this.btnExport;
          break;
        case "PI_Import":
          bool flag = false;
          LoanFolderAclInfo[] foldersForImport = Session.StartupInfo.AccessibleFoldersForImport;
          if (foldersForImport != null && foldersForImport.Length != 0)
          {
            foreach (LoanFolderAclInfo loanFolderAclInfo in foldersForImport)
            {
              if (loanFolderAclInfo.MoveFromAccess == 1 || loanFolderAclInfo.MoveToAccess == 1)
              {
                flag = true;
                break;
              }
            }
          }
          return flag;
        case "PI_ManageAlerts":
          return Session.ACL.IsAuthorizedForFeature(AclFeature.LoanMgmt_Pipeline_Alert) && this.gvLoans.SelectedItems.Count == 1;
        case "PI_ManageViews":
          stateControl = (Control) this.btnManageViews;
          break;
        case "PI_Move":
          stateControl = (Control) this.btnMove;
          break;
        case "PI_New":
          stateControl = (Control) this.btnNew;
          break;
        case "PI_OpenWebView":
          FeatureConfigsAclManager aclManager = (FeatureConfigsAclManager) Session.ACL.GetAclManager(AclCategory.FeatureConfigs);
          return Session.EncompassEdition != EncompassEdition.Broker && aclManager.GetUserApplicationRight(AclFeature.PlatForm_Access) > 0;
        case "PI_OpenWebViewLoan":
          menuItem.Enabled = this.gvLoans.SelectedItems.Count == 1;
          return true;
        case "PI_Print":
          stateControl = (Control) this.btnPrint;
          break;
        case "PI_Refresh":
          stateControl = (Control) this.btnRefresh;
          break;
        case "PI_ResetView":
          stateControl = (Control) this.btnRefreshView;
          break;
        case "PI_SaveView":
          stateControl = (Control) this.btnSaveView;
          break;
        case "PI_Transfer":
          stateControl = (Control) this.btnTransfer;
          break;
        default:
          if (menuItem.Tag is ServiceSetting)
          {
            ServiceSetting tag = (ServiceSetting) menuItem.Tag;
            if (!Session.Application.GetService<ILoanServices>().IsExportServiceAccessible(Session.LoanDataMgr, tag))
              return false;
            if (tag.DisableMenuItemForMultipleLoanSelection)
              menuItem.Enabled = this.gvLoans.SelectedItems.Count == 1;
            if (string.Concat(tag.Tag) == "Selected")
              menuItem.Enabled = this.gvLoans.SelectedItems.Count > 0;
            return true;
          }
          break;
      }
      if (stateControl == null)
        return true;
      ClientCommonUtils.ApplyControlStateToMenu(menuItem, stateControl);
      return stateControl.Visible;
    }

    public string GetHelpTargetName() => "PipelinePage";

    private bool isClientEnabledToExportFNMFRE()
    {
      return Session.MainScreen.IsClientEnabledToExportFNMFRE;
    }

    public void DisabledButtons()
    {
      this.btnExport.Enabled = this.btnPrint.Enabled = this.btnDuplicate.Enabled = this.btnDelete.Enabled = this.btnEdit.Enabled = this.btnNew.Enabled = this.btnTransfer.Enabled = this.btnMove.Enabled = this.btnLoanMailbox.Enabled = false;
      this.elmMailbox.Visible = false;
      this.gvLoans.ItemDoubleClick -= new GVItemEventHandler(this.gvLoans_ItemDoubleClick);
      this.gvLoans.ContextMenuStrip = (ContextMenuStrip) null;
      this.selectLoanOnly = true;
    }

    public PipelineInfo GetSelectedPipelineInfo()
    {
      return this.gvLoans.SelectedItems.Count == 0 ? (PipelineInfo) null : this.gvLoans.SelectedItems[0].Tag as PipelineInfo;
    }

    private bool hasRights(ServiceSetting menu)
    {
      if (menu.ID.ToLower().Contains("fannie"))
        return this.aclMgr.GetUserApplicationRight(AclFeature.LoanMgmt_Export_ULDD_FannieMae);
      if (menu.ID.ToLower().Contains("freddie"))
        return this.aclMgr.GetUserApplicationRight(AclFeature.LoanMgmt_Export_ULDD_FreddieMac);
      if (menu.ID.ToLower().Contains("ginniemae"))
        return this.aclMgr.GetUserApplicationRight(AclFeature.LoanMgmt_Export_PDD_GinnieMae);
      if (menu.DisplayName == "Export Fannie Mae Formatted File (3.2)")
        return this.aclMgr.GetUserApplicationRight(AclFeature.LoanMgmt_Export_FannieMae_FormattedFile);
      if (menu.ID.ToLowerInvariant().Contains("frdmaccac"))
        return this.aclMgr.GetUserApplicationRight(AclFeature.Freddie_Mac_CAC);
      if (menu.ID.ToLowerInvariant().Contains("fnmaucdtransfer"))
        return this.aclMgr.GetUserApplicationRight(AclFeature.Fannie_Mae_UCD_Transfer);
      return !menu.ID.ToLowerInvariant().Contains("frdmaclpa") || this.aclMgr.GetUserApplicationRight(AclFeature.Freddie_Mac_LPA_Batch);
    }

    private void populateServicesMenuItem()
    {
      List<string> categories = ServicesMapping.Categories;
      ILoanServices service = Session.Application.GetService<ILoanServices>();
      List<ToolStripMenuItem> toolStripMenuItemList = new List<ToolStripMenuItem>();
      ExportServicesAclManager aclManager = (ExportServicesAclManager) Session.ACL.GetAclManager(AclCategory.ExportServices);
      ServiceSetting wellsFargoServiceSetting = (ServiceSetting) null;
      foreach (string str in categories)
      {
        List<ServiceSetting> source = new List<ServiceSetting>((IEnumerable<ServiceSetting>) ServicesMapping.GetServiceSetting(str));
        if (str == "GSE Services")
        {
          wellsFargoServiceSetting = source.Where<ServiceSetting>((Func<ServiceSetting, bool>) (setting => setting.ID == "WellsFargo")).FirstOrDefault<ServiceSetting>();
          source.Remove(wellsFargoServiceSetting);
          bool flag = true;
          if (!Session.UserInfo.IsSuperAdministrator() && !Session.UserInfo.IsAdministrator() && Session.EncompassEdition == EncompassEdition.Broker && (this.aclMgr.GetUserApplicationRight(AclFeature.LoanMgmt_Export_ULAD_ForDu) || this.aclMgr.GetUserApplicationRight(AclFeature.LoanMgmt_Export_ILAD) || this.aclMgr.GetUserApplicationRight(AclFeature.LoanMgmt_Export_FannieMae_FormattedFile)))
            flag = false;
          if (((!this.aclMgr.GetUserApplicationRight(AclFeature.LoanMgmt_GSE_Services) ? 1 : (!aclManager.GetUserApplicationRightForPipelineServices("GSE Services") ? 1 : 0)) & (flag ? 1 : 0)) != 0)
            continue;
        }
        ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(str);
        foreach (ServiceSetting serviceSetting in source)
        {
          if (serviceSetting.SupportedInCurrentVersion() && service.IsExportServiceAccessible(Session.LoanDataMgr, serviceSetting))
          {
            if (serviceSetting.ToolStripSeparator)
              toolStripMenuItem.DropDownItems.Add((ToolStripItem) new ToolStripSeparator());
            if (aclManager.GetUserApplicationRightForPipelineServices(serviceSetting.CategoryName) && (!(str == "GSE Services") || this.hasRights(serviceSetting)))
            {
              ToolStripMenuItem serviceMenuItem = this.createServiceMenuItem(serviceSetting);
              toolStripMenuItem.DropDownItems.Add((ToolStripItem) serviceMenuItem);
            }
          }
        }
        if (str == "GSE Services")
        {
          this.toolStripMenuItem1.Click += new EventHandler(this.toolStripMenuItem1_Click);
          toolStripMenuItem.DropDownItems.Add((ToolStripItem) this.toolStripMenuItem1);
        }
        if (toolStripMenuItem.DropDownItems.Count > 0)
          toolStripMenuItemList.Add(toolStripMenuItem);
      }
      toolStripMenuItemList.AddRange((IEnumerable<ToolStripMenuItem>) MainForm.Instance.ShowDataDocsMenus(this.aclMgr, wellsFargoServiceSetting));
      if (toolStripMenuItemList.Count <= 0)
        return;
      int index1 = this.mnuLoans.Items.IndexOf((ToolStripItem) this.mnuLoanProperties);
      this.mnuLoans.Items.Insert(index1, (ToolStripItem) this.mnuDivServices);
      for (int index2 = toolStripMenuItemList.Count - 1; index2 >= 0; --index2)
        this.mnuLoans.Items.Insert(index1, (ToolStripItem) toolStripMenuItemList[index2]);
    }

    public void ShowLoanDeliveryServices()
    {
      if (this.gvLoans == null || this.gvLoans.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select one or more loans to deliver.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        List<Loan> lstLoans = new List<Loan>();
        EllieMae.EMLite.Common.DataDocs.EntityRef entityRef = new EllieMae.EMLite.Common.DataDocs.EntityRef();
        foreach (GVItem selectedItem in this.gvLoans.SelectedItems)
        {
          if (selectedItem.Tag is PipelineInfo tag)
            lstLoans.Add(new Loan()
            {
              entityRef = new EntityRefs()
              {
                EntityId = tag.GUID.Replace("{", "").Replace("}", ""),
                EntityType = "Loan",
                LoanNumber = tag.LoanNumber
              }
            });
        }
        if (lstLoans.Count == 0)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "Please select one or more loans to deliver.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
        {
          JObject filterParam = this.ConstructDeliveryParameter();
          if (Session.LoanData != null && Session.LoanData.Dirty && !Session.Application.GetService<ILoanConsole>().SaveLoan())
            return;
          using (Form deliveryServicesForm = new LoanDeliveryFactory(LoanDeliveryFactory.FormType.LoanDeliveryServices).GetLoanDeliveryServicesForm(lstLoans, this.Size, filterParam, this.p_sortField, this.navPipeline.NumberOfItems))
          {
            int num3 = (int) deliveryServicesForm.ShowDialog();
          }
        }
      }
    }

    private ToolStripMenuItem createServiceMenuItem(ServiceSetting service)
    {
      ToolStripMenuItem serviceMenuItem = new ToolStripMenuItem(service.DisplayName);
      if (service.LoanFileSpecific)
      {
        ToolStripMenuItem toolStripMenuItem1 = new ToolStripMenuItem("&Selected Loans Only...");
        toolStripMenuItem1.Click += new EventHandler(this.serviceItem_Click);
        ServiceSetting serviceSetting1 = service.Clone();
        serviceSetting1.Tag = (object) "Selected";
        toolStripMenuItem1.Tag = (object) serviceSetting1;
        ToolStripMenuItem toolStripMenuItem2 = new ToolStripMenuItem("&All Loans on All Pages...");
        toolStripMenuItem2.Click += new EventHandler(this.serviceItem_Click);
        ServiceSetting serviceSetting2 = service.Clone();
        serviceSetting2.Tag = (object) "All";
        toolStripMenuItem2.Tag = (object) serviceSetting2;
        serviceMenuItem.DropDownItems.AddRange(new ToolStripItem[2]
        {
          (ToolStripItem) toolStripMenuItem1,
          (ToolStripItem) toolStripMenuItem2
        });
      }
      else
        serviceMenuItem.Click += new EventHandler(this.serviceItem_Click);
      serviceMenuItem.Tag = (object) service;
      return serviceMenuItem;
    }

    private void serviceItem_Click(object sender, EventArgs e)
    {
      this.MenuClicked((ToolStripItem) sender);
    }

    private void popUpButton_Click(object sender, EventArgs e)
    {
      if (this.externalOrgsList == null)
        this.loadTPOSettings();
      if (this.externalOrgsList.Count > 0)
      {
        this.pipeLineExtOrgInfo = new PipeLineExtOrgInfo(this.externalOrgsList);
        this.pipeLineExtOrgInfo.StartPosition = FormStartPosition.CenterParent;
        if (this.pipeLineExtOrgInfo.ShowDialog((IWin32Window) this) == DialogResult.OK)
        {
          this.selectedOrg = this.pipeLineExtOrgInfo.selectedOrg;
          if (this.selectedOrg != null)
          {
            this.textBox1.Text = this.selectedOrg.OrganizationName;
            this.textBox1.Tag = (object) this.selectedOrg.ExternalID;
            this.setViewChanged(true);
            this.RefreshPipeline(false);
          }
          this.pipeLineExtOrgInfo.Close();
        }
        else
          this.pipeLineExtOrgInfo.Close();
      }
      else
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "No External Org found!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }

    private void cboCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.suspendEvents)
        return;
      if (this.cboCompany.SelectedItem.Equals((object) "Internal Organization"))
      {
        this.popUpButton.Enabled = false;
        this.textBox1.Text = "";
        this.textBox1.Tag = (object) "";
      }
      else
      {
        if (this.externalOrgsList == null)
          this.loadTPOSettings();
        if (this.textBox1.Text.Equals(""))
        {
          this.textBox1.Text = "All";
          this.textBox1.Tag = (object) "-1";
        }
        this.popUpButton.Enabled = true;
      }
      this.setViewChanged(true);
      this.RefreshPipeline(false, 1);
    }

    private void cboLoanType_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.suspendEvents)
        return;
      this.setViewChanged(true);
      this.RefreshPipeline(false, 1);
    }

    private void btnNotifyUsers_Click(object sender, EventArgs e)
    {
      if (this.gvLoans.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must first select one or more loans from the list.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        List<LoanDisplayInfo> loanInfo = new List<LoanDisplayInfo>();
        foreach (GVItem selectedItem in this.gvLoans.SelectedItems)
        {
          PipelineInfo tag = (PipelineInfo) selectedItem.Tag;
          if (tag != null)
          {
            LoanDisplayInfo loanDisplayInfo = new LoanDisplayInfo();
            loanDisplayInfo.LoanNumber = tag.LoanNumber;
            loanDisplayInfo.LoanGuid = new Guid(tag.GUID);
            if (tag.Info[(object) "Loan.BorrowerName"] != null)
              loanDisplayInfo.BorrowerName = tag.Info[(object) "Loan.BorrowerName"].ToString();
            if (tag.Info[(object) "Loan.LoanAmount"] != null)
              loanDisplayInfo.LoanAmount = Convert.ToDecimal(tag.Info[(object) "Loan.LoanAmount"]);
            loanInfo.Add(loanDisplayInfo);
          }
        }
        using (NotifyUsersDialog notifyUsersDialog = new NotifyUsersDialog(loanInfo))
        {
          int num2 = (int) notifyUsersDialog.ShowDialog((IWin32Window) this);
        }
      }
    }

    public void toolStripMenuItem1_Click(object sender, EventArgs e)
    {
      try
      {
        string path1 = string.Empty;
        string str1 = string.Empty;
        using (ExportToLocal exportToLocal = new ExportToLocal())
        {
          if (exportToLocal.ShowDialog((IWin32Window) Session.MainScreen) != DialogResult.OK)
            return;
          path1 = exportToLocal.folder;
          str1 = exportToLocal.fileName;
        }
        if (str1.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 || str1 == string.Empty)
        {
          int num1 = (int) MessageBox.Show("Invalid file name.", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          string str2 = str1 + ".fnm";
          if (!Directory.Exists(path1))
          {
            int num2 = (int) MessageBox.Show("Invalid folder path.", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
          else
          {
            foreach (GVItem selectedItem in this.gvLoans.SelectedItems)
            {
              PipelineInfo tag = (PipelineInfo) selectedItem.Tag;
              LoanDisplayInfo loanDisplayInfo = new LoanDisplayInfo()
              {
                LoanNumber = tag.LoanNumber,
                LoanGuid = new Guid(tag.GUID)
              };
              LoanDataMgr LoanDataMgr = LoanDataMgr.OpenLoan(Session.SessionObjects, tag.GUID, false);
              LoanData loanData = LoanDataMgr.LoanData;
              if (!this.Validate("FNMA32", true, loanData))
                break;
              string s = this.Export(LoanDataMgr, loanData, "FNMA32", false);
              if (s != "")
              {
                string path2 = path1 + "\\" + str2;
                if (File.Exists(path2) && MessageBox.Show("File already exists. Would you like to replace it?", "Encompass", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.Cancel)
                  break;
                FileStream fileStream = new FileStream(path2, FileMode.Create, FileAccess.Write, FileShare.None);
                byte[] bytes = Encoding.ASCII.GetBytes(s);
                fileStream.Write(bytes, 0, bytes.Length);
                fileStream.Close();
                int num3 = (int) MessageBox.Show("File exported. The file " + path2 + " has been created.", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    public bool Validate(string format, bool allowContinue, LoanData loanData)
    {
      try
      {
        string empty = string.Empty;
        string str = !(format == "LEF") ? SystemSettings.EpassDataDir + "Export.Validate.dll" : SystemSettings.EpassDataDir + "Export.LEF.dll";
        if (!File.Exists(str))
        {
          str = AssemblyResolver.GetResourceFileFolderPath("EMN\\Export.Validate.dll");
          if (!File.Exists(str))
            return false;
        }
        string fullName1 = AssemblyName.GetAssemblyName(str).FullName;
        Assembly assembly1 = (Assembly) null;
        foreach (Assembly assembly2 in AppDomain.CurrentDomain.GetAssemblies())
        {
          if (assembly2.FullName == fullName1)
            assembly1 = assembly2;
        }
        if (assembly1 == (Assembly) null)
        {
          FileStream fileStream = File.OpenRead(str);
          byte[] numArray = new byte[fileStream.Length];
          fileStream.Read(numArray, 0, numArray.Length);
          fileStream.Close();
          assembly1 = Assembly.Load(numArray);
        }
        string fullName2 = typeof (IValidate).FullName;
        string typeName = (string) null;
        foreach (System.Type type in assembly1.GetTypes())
        {
          if (type.GetInterface(fullName2) != (System.Type) null)
            typeName = type.FullName;
        }
        IValidate instance = (IValidate) assembly1.CreateInstance(typeName);
        instance.Bam = (EllieMae.EMLite.Export.IBam) new EllieMae.EMLite.Export.Bam(loanData);
        return instance.ValidateData(format, allowContinue);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message + ".", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
    }

    public string Export(
      LoanDataMgr LoanDataMgr,
      LoanData loanData,
      string format,
      bool currentBorPairOnly)
    {
      try
      {
        ExportData exportData = new ExportData(LoanDataMgr, loanData);
        if (string.Compare(format, "EDRS", true) == 0)
          return loanData.ToXml(true);
        string str1 = PipelineScreen.GetExportAssemblyPath(format);
        Tracing.Log(Tracing.SwImportExport, "ExportData", TraceLevel.Verbose, "Export assembly file: " + str1);
        if (!File.Exists(str1))
        {
          str1 = AssemblyResolver.GetResourceFileFolderPath("EMN\\Export.Fannie32.dll");
          if (!File.Exists(str1))
            return string.Empty;
        }
        string fullName1 = AssemblyName.GetAssemblyName(str1).FullName;
        Assembly assembly1 = (Assembly) null;
        foreach (Assembly assembly2 in AppDomain.CurrentDomain.GetAssemblies())
        {
          if (assembly2.FullName == fullName1)
            assembly1 = assembly2;
        }
        if (assembly1 == (Assembly) null)
        {
          FileStream fileStream = File.OpenRead(str1);
          byte[] numArray = new byte[fileStream.Length];
          fileStream.Read(numArray, 0, numArray.Length);
          fileStream.Close();
          assembly1 = Assembly.Load(numArray);
        }
        string fullName2 = typeof (IExport).FullName;
        string typeName = (string) null;
        foreach (System.Type type in assembly1.GetTypes())
        {
          if (type.GetInterface(fullName2) != (System.Type) null)
            typeName = type.FullName;
        }
        IExport instance = (IExport) assembly1.CreateInstance(typeName);
        instance.Bam = (EllieMae.EMLite.Export.IBam) new EllieMae.EMLite.Export.Bam(loanData);
        string empty = string.Empty;
        string str2;
        if (currentBorPairOnly)
        {
          MethodInfo method = instance.GetType().GetMethod("ExportData", new System.Type[1]
          {
            typeof (bool)
          });
          str2 = !(method == (MethodInfo) null) ? string.Concat(method.Invoke((object) instance, new object[1]
          {
            (object) true
          })) : throw new NotSupportedException("The export format '" + format + "' does not support export of the current borrower pair only");
        }
        else
          str2 = this.exportDataMethodInvoke(instance);
        return str2;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message + ".", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return "";
      }
    }

    public static string GetExportAssemblyPath(string format)
    {
      if (string.Compare(format, "EDRS", true) == 0)
        return (string) null;
      switch (format.ToUpper())
      {
        case "EMXML":
          format = "EMXML10";
          break;
        case "FNMA30":
          format = "FANNIE30";
          break;
        case "FNMA32":
          format = "FANNIE32";
          break;
        case "FANNIE":
          format = "LOANDELIVERY";
          break;
        case "FREDDIE":
          format = "LOANDELIVERY";
          break;
        case "GINNIE":
          format = "GnmaPdd12";
          break;
      }
      return SystemSettings.EpassDataDir + "Export." + format + ".dll";
    }

    protected string exportDataMethodInvoke(IExport export) => export.ExportData();

    private void btnSubmissionStatus_Click(object sender, EventArgs e)
    {
      if (this.displayingSubmissionStatus)
        return;
      try
      {
        this.displayingSubmissionStatus = true;
        using (Form submissionStatusForm = new LoanDeliveryFactory(LoanDeliveryFactory.FormType.LoanDeliveryStatus).GetSubmissionStatusForm(this.Size))
        {
          int num = (int) submissionStatusForm.ShowDialog((IWin32Window) this);
        }
      }
      catch
      {
        throw;
      }
      finally
      {
        this.displayingSubmissionStatus = false;
      }
    }

    private void btn_eSignPackages_Click(object sender, EventArgs e)
    {
      try
      {
        using (PerformanceMeter.StartNew(nameof (btn_eSignPackages_Click), "Launch eSignPackages app", false, 5739, nameof (btn_eSignPackages_Click), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\PipelineScreen.cs"))
        {
          using (ThinThickDialog thinThickDialog = new ThinThickDialog((LoanDataMgr) null, Session.SessionObjects.StartupInfo.eSignPackagesUrl, ThinThickType.eSignPackages))
          {
            PerformanceMeter.Current.AddCheckpoint("BEFORE ThinThickDialog.ShowDialog", 5745, nameof (btn_eSignPackages_Click), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\PipelineScreen.cs");
            PerformanceMeter.Current.AddCheckpoint("AFTER ThinThickDialog.ShowDialog: " + thinThickDialog.ShowDialog((IWin32Window) Form.ActiveForm).ToString(), 5747, nameof (btn_eSignPackages_Click), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\PipelineScreen.cs");
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(PipelineScreen.sw, nameof (PipelineScreen), TraceLevel.Error, "eSignPackages failure. Ex: " + ex.ToString());
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "An unexpected error occurred during the eSignPackages workflow.\n\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void btn_LaunchAIQ_Click(object sender, EventArgs e)
    {
      if (this.gvLoans.SelectedItems.Count != 1)
        return;
      this.aiqHelper.btnClick_action((this.gvLoans.SelectedItems[0].Tag as PipelineInfo).GUID);
    }

    private void loadFolder()
    {
      LoanFolderInfo[] allLoanFolderInfos = Session.LoanManager.GetAllLoanFolderInfos(true);
      Session.SessionObjects.LoanFolderInfos = allLoanFolderInfos != null ? ((IEnumerable<LoanFolderInfo>) allLoanFolderInfos).ToList<LoanFolderInfo>() : (List<LoanFolderInfo>) null;
      this.loadAllFolders(allLoanFolderInfos);
      this.selectFolders(allLoanFolderInfos);
      this.cboFolder_DropDownClosed((object) null, (EventArgs) null);
    }

    private void chkarchive_CheckedChanged(object sender, EventArgs e)
    {
      this.canUserSearchInArchiveFolders = (Session.StartupInfo.EnableLoanSoftArchival && PipelineScreen.canAccessArchiveFolders || !Session.StartupInfo.EnableLoanSoftArchival && PipelineScreen.canSearchArchiveFolders) && this.chkarchive.Checked;
      if (this.rbGlobalSearchOn.Checked)
      {
        if (!this.canUserSearchInArchiveFolders)
          this.loadFolder();
        this.rbGlobalSearch_Changed(sender, e);
      }
      else
      {
        this.loadFolder();
        this.RefreshPipeline();
      }
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
      this.toolTips = new ToolTip(this.components);
      this.btnPrint = new StandardIconButton();
      this.btnExport = new StandardIconButton();
      this.btnRefresh = new StandardIconButton();
      this.btnDelete = new StandardIconButton();
      this.btnDuplicate = new StandardIconButton();
      this.btnEdit = new StandardIconButton();
      this.btnNew = new StandardIconButton();
      this.btnRefreshView = new StandardIconButton();
      this.btnManageViews = new StandardIconButton();
      this.btnSaveView = new StandardIconButton();
      this.pictureBox1 = new PictureBox();
      this.mnuLoans = new ContextMenuStrip(this.components);
      this.mnuNew = new ToolStripMenuItem();
      this.mnuOpen = new ToolStripMenuItem();
      this.mnuDuplicate = new ToolStripMenuItem();
      this.mnuDivNewEdit = new ToolStripSeparator();
      this.mnuMove = new ToolStripMenuItem();
      this.mnuTransfer = new ToolStripMenuItem();
      this.mnuDelete = new ToolStripMenuItem();
      this.mnuDivMoveDelete = new ToolStripSeparator();
      this.mnuRefresh = new ToolStripMenuItem();
      this.mnuExport = new ToolStripMenuItem();
      this.mnuExportSelected = new ToolStripMenuItem();
      this.mnuExportAll = new ToolStripMenuItem();
      this.mnuPrint = new ToolStripMenuItem();
      this.mnuManageAlerts = new ToolStripMenuItem();
      this.mnuDivPrint = new ToolStripSeparator();
      this.mnuEFolderDocuments = new ToolStripMenuItem();
      this.mnuSendFiles = new ToolStripMenuItem();
      this.mnuSendFilesToLender = new ToolStripMenuItem();
      this.mnuExportDocuments = new ToolStripMenuItem();
      this.mnuExportDocsSelectedLoans = new ToolStripMenuItem();
      this.mnuExportDocsAllLoans = new ToolStripMenuItem();
      this.mnuEFolderexportToExcel = new ToolStripMenuItem();
      this.toolStripSeparator1 = new ToolStripSeparator();
      this.mnuLoanProperties = new ToolStripMenuItem();
      this.mnuSelectAll = new ToolStripMenuItem();
      this.toolStripMenuItem1 = new ToolStripMenuItem();
      this.mnuDivServices = new ToolStripSeparator();
      this.mnuMenuItemOpenWebView = new ToolStripMenuItem();
      this.mnuOpenWebViewLoan = new ToolStripMenuItem();
      this.mnuOpportunities = new ToolStripMenuItem();
      this.mnuProspects = new ToolStripMenuItem();
      this.toolStripSeparatorOpenWebView = new ToolStripSeparator();
      this.tmrRefresh = new Timer(this.components);
      this.groupContainer1 = new GroupContainer();
      this.navPipeline = new PageListNavigator();
      this.gvLoans = new GridView();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.btnTransfer = new Button();
      this.btnMove = new Button();
      this.verticalSeparator1 = new VerticalSeparator();
      this.gradientPanel3 = new GradientPanel();
      this.btn_LaunchAIQ = new Button();
      this.btn_eSignPackages = new Button();
      this.btnSubmissionStatus = new Button();
      this.lblFilter = new Label();
      this.btnClearSearch = new Button();
      this.btnAdvSearch = new Button();
      this.label4 = new Label();
      this.btnNotifyUsers = new Button();
      this.gradientPanel2 = new GradientPanel();
      this.pnlAllFolderSelection = new FlowLayoutPanel();
      this.panel1 = new Panel();
      this.cboFolder = new CheckedComboBox(this.components);
      this.panel2 = new Panel();
      this.chkarchive = new CheckBox();
      this.panel3 = new Panel();
      this.cboLoanType = new ComboBox();
      this.lblCompany = new Label();
      this.cboCompany = new ComboBox();
      this.textBox1 = new TextBox();
      this.popUpButton = new StandardIconButton();
      this.pnlglobalsearch = new Panel();
      this.label5 = new Label();
      this.rbGlobalSearchOff = new RadioButton();
      this.rbGlobalSearchOn = new RadioButton();
      this.gradientPanel1 = new GradientPanel();
      this.btnLoanMailbox = new IconButton();
      this.cboView = new ComboBoxEx();
      this.label1 = new Label();
      this.elmMailbox = new ElementControl();
      this.label3 = new Label();
      this.label2 = new Label();
      ((ISupportInitialize) this.btnPrint).BeginInit();
      ((ISupportInitialize) this.btnExport).BeginInit();
      ((ISupportInitialize) this.btnRefresh).BeginInit();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      ((ISupportInitialize) this.btnDuplicate).BeginInit();
      ((ISupportInitialize) this.btnEdit).BeginInit();
      ((ISupportInitialize) this.btnNew).BeginInit();
      ((ISupportInitialize) this.btnRefreshView).BeginInit();
      ((ISupportInitialize) this.btnManageViews).BeginInit();
      ((ISupportInitialize) this.btnSaveView).BeginInit();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.mnuLoans.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.gradientPanel3.SuspendLayout();
      this.gradientPanel2.SuspendLayout();
      this.pnlAllFolderSelection.SuspendLayout();
      this.panel1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.panel3.SuspendLayout();
      ((ISupportInitialize) this.popUpButton).BeginInit();
      this.pnlglobalsearch.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      ((ISupportInitialize) this.btnLoanMailbox).BeginInit();
      this.SuspendLayout();
      this.btnPrint.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnPrint.BackColor = Color.Transparent;
      this.btnPrint.Location = new Point(283, 3);
      this.btnPrint.Margin = new Padding(3, 3, 2, 3);
      this.btnPrint.MouseDownImage = (Image) null;
      this.btnPrint.Name = "btnPrint";
      this.btnPrint.Size = new Size(16, 16);
      this.btnPrint.StandardButtonType = StandardIconButton.ButtonType.PrintButton;
      this.btnPrint.TabIndex = 6;
      this.btnPrint.TabStop = false;
      this.toolTips.SetToolTip((Control) this.btnPrint, "Print Forms");
      this.btnPrint.Click += new EventHandler(this.btnPrint_Click);
      this.btnExport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExport.BackColor = Color.Transparent;
      this.btnExport.Location = new Point(262, 3);
      this.btnExport.Margin = new Padding(3, 3, 2, 3);
      this.btnExport.MouseDownImage = (Image) null;
      this.btnExport.Name = "btnExport";
      this.btnExport.Size = new Size(16, 16);
      this.btnExport.StandardButtonType = StandardIconButton.ButtonType.ExcelButton;
      this.btnExport.TabIndex = 5;
      this.btnExport.TabStop = false;
      this.toolTips.SetToolTip((Control) this.btnExport, "Export to Excel");
      this.btnExport.Click += new EventHandler(this.btnExport_Click);
      this.btnRefresh.BackColor = Color.Transparent;
      this.btnRefresh.Location = new Point(241, 3);
      this.btnRefresh.Margin = new Padding(3, 3, 2, 3);
      this.btnRefresh.MouseDownImage = (Image) null;
      this.btnRefresh.Name = "btnRefresh";
      this.btnRefresh.Size = new Size(16, 16);
      this.btnRefresh.StandardButtonType = StandardIconButton.ButtonType.RefreshButton;
      this.btnRefresh.TabIndex = 17;
      this.btnRefresh.TabStop = false;
      this.toolTips.SetToolTip((Control) this.btnRefresh, "Refresh");
      this.btnRefresh.Click += new EventHandler(this.btnRefresh_Click);
      this.btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Location = new Point(220, 3);
      this.btnDelete.Margin = new Padding(3, 3, 2, 3);
      this.btnDelete.MouseDownImage = (Image) null;
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(16, 16);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabIndex = 13;
      this.btnDelete.TabStop = false;
      this.toolTips.SetToolTip((Control) this.btnDelete, "Delete Loan");
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.btnDuplicate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDuplicate.BackColor = Color.Transparent;
      this.btnDuplicate.Location = new Point(199, 3);
      this.btnDuplicate.Margin = new Padding(3, 3, 2, 3);
      this.btnDuplicate.MouseDownImage = (Image) null;
      this.btnDuplicate.Name = "btnDuplicate";
      this.btnDuplicate.Size = new Size(16, 16);
      this.btnDuplicate.StandardButtonType = StandardIconButton.ButtonType.DuplicateButton;
      this.btnDuplicate.TabIndex = 15;
      this.btnDuplicate.TabStop = false;
      this.toolTips.SetToolTip((Control) this.btnDuplicate, "Duplicate Loan");
      this.btnDuplicate.Click += new EventHandler(this.btnDuplicate_Click);
      this.btnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnEdit.BackColor = Color.Transparent;
      this.btnEdit.Location = new Point(178, 3);
      this.btnEdit.Margin = new Padding(3, 3, 2, 3);
      this.btnEdit.MouseDownImage = (Image) null;
      this.btnEdit.Name = "btnEdit";
      this.btnEdit.Size = new Size(16, 16);
      this.btnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEdit.TabIndex = 14;
      this.btnEdit.TabStop = false;
      this.toolTips.SetToolTip((Control) this.btnEdit, "Edit Loan");
      this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
      this.btnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnNew.BackColor = Color.Transparent;
      this.btnNew.Location = new Point(157, 3);
      this.btnNew.Margin = new Padding(3, 3, 2, 3);
      this.btnNew.MouseDownImage = (Image) null;
      this.btnNew.Name = "btnNew";
      this.btnNew.Size = new Size(16, 16);
      this.btnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnNew.TabIndex = 16;
      this.btnNew.TabStop = false;
      this.toolTips.SetToolTip((Control) this.btnNew, "New Loan");
      this.btnNew.Click += new EventHandler(this.btnNew_Click);
      this.btnRefreshView.BackColor = Color.Transparent;
      this.btnRefreshView.Location = new Point(362, 8);
      this.btnRefreshView.MouseDownImage = (Image) null;
      this.btnRefreshView.Name = "btnRefreshView";
      this.btnRefreshView.Size = new Size(16, 16);
      this.btnRefreshView.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.btnRefreshView.TabIndex = 4;
      this.btnRefreshView.TabStop = false;
      this.toolTips.SetToolTip((Control) this.btnRefreshView, "Reset View");
      this.btnRefreshView.Click += new EventHandler(this.btnRefreshView_Click);
      this.btnManageViews.BackColor = Color.Transparent;
      this.btnManageViews.Location = new Point(384, 8);
      this.btnManageViews.MouseDownImage = (Image) null;
      this.btnManageViews.Name = "btnManageViews";
      this.btnManageViews.Size = new Size(16, 16);
      this.btnManageViews.StandardButtonType = StandardIconButton.ButtonType.ManageButton;
      this.btnManageViews.TabIndex = 3;
      this.btnManageViews.TabStop = false;
      this.toolTips.SetToolTip((Control) this.btnManageViews, "Manage Views");
      this.btnManageViews.Click += new EventHandler(this.btnManageViews_Click);
      this.btnSaveView.BackColor = Color.Transparent;
      this.btnSaveView.Location = new Point(340, 8);
      this.btnSaveView.MouseDownImage = (Image) null;
      this.btnSaveView.Name = "btnSaveView";
      this.btnSaveView.Size = new Size(16, 16);
      this.btnSaveView.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.btnSaveView.TabIndex = 2;
      this.btnSaveView.TabStop = false;
      this.toolTips.SetToolTip((Control) this.btnSaveView, "Save View");
      this.btnSaveView.Click += new EventHandler(this.btnSaveView_Click);
      this.pictureBox1.Image = (Image) Resources.help;
      this.pictureBox1.Location = new Point(175, 1);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(18, 18);
      this.pictureBox1.TabIndex = 3;
      this.pictureBox1.TabStop = false;
      this.toolTips.SetToolTip((Control) this.pictureBox1, "To use Global Search, one of the following columns must be present: Loan Number, Borrower First Name, Loan Name, Borrower Last Name, Subject Property Address");
      this.mnuLoans.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.mnuLoans.Items.AddRange(new ToolStripItem[20]
      {
        (ToolStripItem) this.mnuNew,
        (ToolStripItem) this.mnuOpen,
        (ToolStripItem) this.mnuDuplicate,
        (ToolStripItem) this.mnuDivNewEdit,
        (ToolStripItem) this.mnuMove,
        (ToolStripItem) this.mnuTransfer,
        (ToolStripItem) this.mnuDelete,
        (ToolStripItem) this.mnuDivMoveDelete,
        (ToolStripItem) this.mnuRefresh,
        (ToolStripItem) this.mnuExport,
        (ToolStripItem) this.mnuPrint,
        (ToolStripItem) this.mnuManageAlerts,
        (ToolStripItem) this.mnuDivPrint,
        (ToolStripItem) this.mnuEFolderDocuments,
        (ToolStripItem) this.toolStripSeparator1,
        (ToolStripItem) this.mnuMenuItemOpenWebView,
        (ToolStripItem) this.toolStripSeparatorOpenWebView,
        (ToolStripItem) this.mnuLoanProperties,
        (ToolStripItem) this.mnuSelectAll,
        (ToolStripItem) this.toolStripMenuItem1
      });
      this.mnuLoans.Name = "mnuLoans";
      this.mnuLoans.ShowImageMargin = false;
      this.mnuLoans.Size = new Size(235, 336);
      this.mnuLoans.Opening += new CancelEventHandler(this.mnuLoans_Opening);
      this.mnuNew.Name = "mnuNew";
      this.mnuNew.Size = new Size(234, 22);
      this.mnuNew.Text = "&New Loan...";
      this.mnuNew.Click += new EventHandler(this.btnNew_Click);
      this.mnuOpen.Name = "mnuOpen";
      this.mnuOpen.Size = new Size(234, 22);
      this.mnuOpen.Text = "&Edit Loan...";
      this.mnuOpen.Click += new EventHandler(this.btnEdit_Click);
      this.mnuDuplicate.Name = "mnuDuplicate";
      this.mnuDuplicate.Size = new Size(234, 22);
      this.mnuDuplicate.Text = "&Duplicate Loan...";
      this.mnuDuplicate.Click += new EventHandler(this.mnuDuplicate_Click);
      this.mnuDivNewEdit.Name = "mnuDivNewEdit";
      this.mnuDivNewEdit.Size = new Size(231, 6);
      this.mnuMove.Name = "mnuMove";
      this.mnuMove.Size = new Size(234, 22);
      this.mnuMove.Text = "Move to Folder";
      this.mnuTransfer.Name = "mnuTransfer";
      this.mnuTransfer.Size = new Size(234, 22);
      this.mnuTransfer.Text = "Transfer...";
      this.mnuTransfer.Click += new EventHandler(this.btnTransfer_Click);
      this.mnuDelete.Name = "mnuDelete";
      this.mnuDelete.Size = new Size(234, 22);
      this.mnuDelete.Text = "Delete Loan";
      this.mnuDelete.Click += new EventHandler(this.btnDelete_Click);
      this.mnuDivMoveDelete.Name = "mnuDivMoveDelete";
      this.mnuDivMoveDelete.Size = new Size(231, 6);
      this.mnuRefresh.Name = "mnuRefresh";
      this.mnuRefresh.Size = new Size(234, 22);
      this.mnuRefresh.Text = "Re&fresh";
      this.mnuRefresh.Click += new EventHandler(this.btnRefresh_Click);
      this.mnuExport.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.mnuExportSelected,
        (ToolStripItem) this.mnuExportAll
      });
      this.mnuExport.Name = "mnuExport";
      this.mnuExport.Size = new Size(234, 22);
      this.mnuExport.Text = "Export to Excel";
      this.mnuExportSelected.Name = "mnuExportSelected";
      this.mnuExportSelected.Size = new Size(190, 22);
      this.mnuExportSelected.Text = "Selected Loans Only...";
      this.mnuExportSelected.Click += new EventHandler(this.btnExport_Click);
      this.mnuExportAll.Name = "mnuExportAll";
      this.mnuExportAll.Size = new Size(190, 22);
      this.mnuExportAll.Text = "All Loans on All Pages...";
      this.mnuExportAll.Click += new EventHandler(this.mnuExportAll_Click);
      this.mnuPrint.Name = "mnuPrint";
      this.mnuPrint.Size = new Size(234, 22);
      this.mnuPrint.Text = "Print Forms...";
      this.mnuPrint.Click += new EventHandler(this.btnPrint_Click);
      this.mnuManageAlerts.Name = "mnuManageAlerts";
      this.mnuManageAlerts.Size = new Size(234, 22);
      this.mnuManageAlerts.Text = "Manage Alerts...";
      this.mnuManageAlerts.Click += new EventHandler(this.mnuManageAlerts_Click);
      this.mnuDivPrint.Name = "mnuDivPrint";
      this.mnuDivPrint.Size = new Size(231, 6);
      this.mnuEFolderDocuments.DropDownItems.AddRange(new ToolStripItem[4]
      {
        (ToolStripItem) this.mnuSendFiles,
        (ToolStripItem) this.mnuSendFilesToLender,
        (ToolStripItem) this.mnuExportDocuments,
        (ToolStripItem) this.mnuEFolderexportToExcel
      });
      this.mnuEFolderDocuments.Name = "mnuEFolderDocuments";
      this.mnuEFolderDocuments.Size = new Size(234, 22);
      this.mnuEFolderDocuments.Text = "eFolder Documents";
      this.mnuSendFiles.Enabled = false;
      this.mnuSendFiles.Name = "mnuSendFiles";
      this.mnuSendFiles.Size = new Size(173, 22);
      this.mnuSendFiles.Text = "Send Files";
      this.mnuSendFiles.Visible = false;
      this.mnuSendFilesToLender.Enabled = false;
      this.mnuSendFilesToLender.Name = "mnuSendFilesToLender";
      this.mnuSendFilesToLender.Size = new Size(173, 22);
      this.mnuSendFilesToLender.Text = "Send Files to Lender";
      this.mnuSendFilesToLender.Visible = false;
      this.mnuExportDocuments.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.mnuExportDocsSelectedLoans,
        (ToolStripItem) this.mnuExportDocsAllLoans
      });
      this.mnuExportDocuments.Name = "mnuExportDocuments";
      this.mnuExportDocuments.Size = new Size(173, 22);
      this.mnuExportDocuments.Text = "Export Document(s)";
      this.mnuExportDocsSelectedLoans.Name = "mnuExportDocsSelectedLoans";
      this.mnuExportDocsSelectedLoans.Size = new Size(190, 22);
      this.mnuExportDocsSelectedLoans.Text = "&Selected Loans Only...";
      this.mnuExportDocsSelectedLoans.Click += new EventHandler(this.mnuExportDocsSelectedLoans_Click);
      this.mnuExportDocsAllLoans.Name = "mnuExportDocsAllLoans";
      this.mnuExportDocsAllLoans.Size = new Size(190, 22);
      this.mnuExportDocsAllLoans.Text = "All Loans on All Pages...";
      this.mnuExportDocsAllLoans.Click += new EventHandler(this.mnuExportDocsAllLoans_Click);
      this.mnuEFolderexportToExcel.Enabled = false;
      this.mnuEFolderexportToExcel.Name = "mnuEFolderexportToExcel";
      this.mnuEFolderexportToExcel.Size = new Size(173, 22);
      this.mnuEFolderexportToExcel.Text = "Export to Excel";
      this.mnuEFolderexportToExcel.Visible = false;
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new Size(231, 6);
      this.mnuMenuItemOpenWebView.DropDownItems.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.mnuOpenWebViewLoan,
        (ToolStripItem) this.mnuOpportunities,
        (ToolStripItem) this.mnuProspects
      });
      this.mnuMenuItemOpenWebView.Name = "mnuMenuItemOpenWebView";
      this.mnuMenuItemOpenWebView.Size = new Size(234, 22);
      this.mnuMenuItemOpenWebView.Tag = (object) "PI_OpenWebView";
      this.mnuMenuItemOpenWebView.Text = "Open Web View";
      this.mnuOpenWebViewLoan.Name = "mnuOpenWebViewLoan";
      this.mnuOpenWebViewLoan.Size = new Size(173, 22);
      this.mnuOpenWebViewLoan.Tag = (object) "PI_OpenWebViewLoan";
      this.mnuOpenWebViewLoan.Text = "Loan";
      this.mnuOpenWebViewLoan.Click += new EventHandler(this.btnOpenWebViewLoan_ClickAsync);
      this.mnuOpportunities.Name = "mnuOpportunities";
      this.mnuOpportunities.Size = new Size(173, 22);
      this.mnuOpportunities.Tag = (object) "PI_Opportunities";
      this.mnuOpportunities.Text = "Opportunities";
      this.mnuOpportunities.Click += new EventHandler(this.btnOpportunities_ClickAsync);
      this.mnuProspects.Name = "mnuProspects";
      this.mnuProspects.Size = new Size(173, 22);
      this.mnuProspects.Tag = (object) "PI_Prospects";
      this.mnuProspects.Text = "Prospects";
      this.mnuProspects.Click += new EventHandler(this.btnProspects_ClickAsync);
      this.toolStripSeparatorOpenWebView.Name = "toolStripSeparatorOpenWebView";
      this.toolStripSeparatorOpenWebView.Size = new Size(231, 6);
      this.mnuLoanProperties.Name = "mnuLoanProperties";
      this.mnuLoanProperties.Size = new Size(234, 22);
      this.mnuLoanProperties.Text = "Properties...";
      this.mnuLoanProperties.Click += new EventHandler(this.mnuLoanProperties_Click);
      this.mnuSelectAll.Name = "mnuSelectAll";
      this.mnuSelectAll.Size = new Size(234, 22);
      this.mnuSelectAll.Text = "Select All on This Page";
      this.mnuSelectAll.Click += new EventHandler(this.mnuSelectAll_Click);
      this.toolStripMenuItem1.Name = "toolStripMenuItem1";
      this.toolStripMenuItem1.Size = new Size(234, 22);
      this.toolStripMenuItem1.Tag = (object) "SRV_FannieMaeFormattedFile";
      this.toolStripMenuItem1.Text = "Export Fannie Mae Formatted File (3.2)";
      this.mnuDivServices.Name = "mnuDivServices";
      this.mnuDivServices.Size = new Size(155, 6);
      this.tmrRefresh.Interval = 1000;
      this.tmrRefresh.Tick += new EventHandler(this.tmrRefresh_Tick);
      this.groupContainer1.Controls.Add((Control) this.navPipeline);
      this.groupContainer1.Controls.Add((Control) this.gvLoans);
      this.groupContainer1.Controls.Add((Control) this.flowLayoutPanel1);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 93);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(1237, 440);
      this.groupContainer1.TabIndex = 3;
      this.navPipeline.BackColor = Color.Transparent;
      this.navPipeline.Font = new Font("Arial", 8f);
      this.navPipeline.Location = new Point(0, 2);
      this.navPipeline.Name = "navPipeline";
      this.navPipeline.NumberOfItems = 0;
      this.navPipeline.Size = new Size(254, 22);
      this.navPipeline.TabIndex = 1;
      this.navPipeline.PageChangedEvent += new PageListNavigator.PageChangedEventHandler(this.navPipeline_PageChangedEvent);
      this.gvLoans.AllowColumnReorder = true;
      this.gvLoans.BorderStyle = BorderStyle.None;
      this.gvLoans.ContextMenuStrip = this.mnuLoans;
      this.gvLoans.Dock = DockStyle.Fill;
      this.gvLoans.FilterVisible = true;
      this.gvLoans.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvLoans.Location = new Point(1, 26);
      this.gvLoans.Name = "gvLoans";
      this.gvLoans.Size = new Size(1235, 413);
      this.gvLoans.SortOption = GVSortOption.Owner;
      this.gvLoans.TabIndex = 3;
      this.gvLoans.SelectedIndexChanged += new EventHandler(this.gvLoans_SelectedIndexChanged);
      this.gvLoans.ColumnReorder += new GVColumnEventHandler(this.gvLoans_ColumnReorder);
      this.gvLoans.ColumnResize += new GVColumnEventHandler(this.gvLoans_ColumnResize);
      this.gvLoans.SortItems += new GVColumnSortEventHandler(this.gvLoans_SortItems);
      this.gvLoans.ItemDoubleClick += new GVItemEventHandler(this.gvLoans_ItemDoubleClick);
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnTransfer);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnMove);
      this.flowLayoutPanel1.Controls.Add((Control) this.verticalSeparator1);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnPrint);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnExport);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnRefresh);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnDelete);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnDuplicate);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnEdit);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnNew);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(753, 2);
      this.flowLayoutPanel1.Margin = new Padding(0);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(479, 22);
      this.flowLayoutPanel1.TabIndex = 2;
      this.btnTransfer.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnTransfer.BackColor = SystemColors.Control;
      this.btnTransfer.Location = new Point(411, 0);
      this.btnTransfer.Margin = new Padding(0);
      this.btnTransfer.Name = "btnTransfer";
      this.btnTransfer.Padding = new Padding(1, 0, 0, 0);
      this.btnTransfer.Size = new Size(68, 22);
      this.btnTransfer.TabIndex = 2;
      this.btnTransfer.Text = "&Transfer";
      this.btnTransfer.UseVisualStyleBackColor = true;
      this.btnTransfer.Click += new EventHandler(this.btnTransfer_Click);
      this.btnMove.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnMove.BackColor = SystemColors.Control;
      this.btnMove.Location = new Point(310, 0);
      this.btnMove.Margin = new Padding(0);
      this.btnMove.Name = "btnMove";
      this.btnMove.Padding = new Padding(2, 0, 0, 0);
      this.btnMove.Size = new Size(101, 22);
      this.btnMove.TabIndex = 1;
      this.btnMove.Text = "&Move to Folder";
      this.btnMove.UseVisualStyleBackColor = true;
      this.btnMove.Click += new EventHandler(this.btnMove_Click);
      this.verticalSeparator1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.verticalSeparator1.Location = new Point(304, 3);
      this.verticalSeparator1.Margin = new Padding(3, 3, 4, 3);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 7;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.gradientPanel3.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gradientPanel3.Controls.Add((Control) this.btn_LaunchAIQ);
      this.gradientPanel3.Controls.Add((Control) this.btn_eSignPackages);
      this.gradientPanel3.Controls.Add((Control) this.btnSubmissionStatus);
      this.gradientPanel3.Controls.Add((Control) this.lblFilter);
      this.gradientPanel3.Controls.Add((Control) this.btnClearSearch);
      this.gradientPanel3.Controls.Add((Control) this.btnAdvSearch);
      this.gradientPanel3.Controls.Add((Control) this.label4);
      this.gradientPanel3.Controls.Add((Control) this.btnNotifyUsers);
      this.gradientPanel3.Dock = DockStyle.Top;
      this.gradientPanel3.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel3.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel3.Location = new Point(0, 62);
      this.gradientPanel3.Name = "gradientPanel3";
      this.gradientPanel3.Size = new Size(1237, 31);
      this.gradientPanel3.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel3.TabIndex = 2;
      this.btn_LaunchAIQ.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btn_LaunchAIQ.Location = new Point(772, 5);
      this.btn_LaunchAIQ.Margin = new Padding(0);
      this.btn_LaunchAIQ.Name = "btn_LaunchAIQ";
      this.btn_LaunchAIQ.Padding = new Padding(1, 0, 0, 0);
      this.btn_LaunchAIQ.Size = new Size(56, 22);
      this.btn_LaunchAIQ.TabIndex = 20;
      this.btn_LaunchAIQ.Text = "DDA";
      this.btn_LaunchAIQ.UseVisualStyleBackColor = true;
      this.btn_LaunchAIQ.Visible = false;
      this.btn_LaunchAIQ.Click += new EventHandler(this.btn_LaunchAIQ_Click);
      this.btn_eSignPackages.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btn_eSignPackages.Location = new Point(728, 5);
      this.btn_eSignPackages.Name = "btn_eSignPackages";
      this.btn_eSignPackages.Size = new Size(101, 22);
      this.btn_eSignPackages.TabIndex = 20;
      this.btn_eSignPackages.Text = "eSign Packages";
      this.btn_eSignPackages.UseVisualStyleBackColor = true;
      this.btn_eSignPackages.Visible = false;
      this.btn_eSignPackages.Click += new EventHandler(this.btn_eSignPackages_Click);
      this.btnSubmissionStatus.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSubmissionStatus.BackColor = SystemColors.Control;
      this.btnSubmissionStatus.Location = new Point(829, 5);
      this.btnSubmissionStatus.Margin = new Padding(0);
      this.btnSubmissionStatus.Name = "btnSubmissionStatus";
      this.btnSubmissionStatus.Padding = new Padding(1, 0, 0, 0);
      this.btnSubmissionStatus.Size = new Size(149, 22);
      this.btnSubmissionStatus.TabIndex = 19;
      this.btnSubmissionStatus.Text = "&Loan Delivery Status";
      this.btnSubmissionStatus.UseVisualStyleBackColor = true;
      this.btnSubmissionStatus.Click += new EventHandler(this.btnSubmissionStatus_Click);
      this.lblFilter.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblFilter.AutoEllipsis = true;
      this.lblFilter.BackColor = Color.Transparent;
      this.lblFilter.Location = new Point(38, 9);
      this.lblFilter.Name = "lblFilter";
      this.lblFilter.Size = new Size(794, 14);
      this.lblFilter.TabIndex = 7;
      this.btnClearSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnClearSearch.Enabled = false;
      this.btnClearSearch.Location = new Point(1176, 5);
      this.btnClearSearch.Name = "btnClearSearch";
      this.btnClearSearch.Padding = new Padding(2, 0, 0, 0);
      this.btnClearSearch.Size = new Size(56, 22);
      this.btnClearSearch.TabIndex = 6;
      this.btnClearSearch.Text = "&Clear";
      this.btnClearSearch.UseVisualStyleBackColor = true;
      this.btnClearSearch.Click += new EventHandler(this.btnClearSearch_Click);
      this.btnAdvSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAdvSearch.Location = new Point(1062, 5);
      this.btnAdvSearch.Name = "btnAdvSearch";
      this.btnAdvSearch.Size = new Size(114, 22);
      this.btnAdvSearch.TabIndex = 4;
      this.btnAdvSearch.Text = "Advanced &Search";
      this.btnAdvSearch.UseVisualStyleBackColor = true;
      this.btnAdvSearch.Click += new EventHandler(this.btnAdvSearch_Click);
      this.label4.AutoSize = true;
      this.label4.BackColor = Color.Transparent;
      this.label4.Location = new Point(7, 9);
      this.label4.Name = "label4";
      this.label4.Size = new Size(33, 14);
      this.label4.TabIndex = 3;
      this.label4.Text = "Filter:";
      this.btnNotifyUsers.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnNotifyUsers.BackColor = SystemColors.Control;
      this.btnNotifyUsers.Location = new Point(978, 5);
      this.btnNotifyUsers.Margin = new Padding(0);
      this.btnNotifyUsers.Name = "btnNotifyUsers";
      this.btnNotifyUsers.Padding = new Padding(1, 0, 0, 0);
      this.btnNotifyUsers.Size = new Size(84, 22);
      this.btnNotifyUsers.TabIndex = 18;
      this.btnNotifyUsers.Text = "Notify &Users";
      this.btnNotifyUsers.UseVisualStyleBackColor = true;
      this.btnNotifyUsers.Click += new EventHandler(this.btnNotifyUsers_Click);
      this.gradientPanel2.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gradientPanel2.Controls.Add((Control) this.pnlAllFolderSelection);
      this.gradientPanel2.Dock = DockStyle.Top;
      this.gradientPanel2.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel2.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel2.Location = new Point(0, 31);
      this.gradientPanel2.Name = "gradientPanel2";
      this.gradientPanel2.Size = new Size(1237, 31);
      this.gradientPanel2.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel2.TabIndex = 1;
      this.pnlAllFolderSelection.Controls.Add((Control) this.panel1);
      this.pnlAllFolderSelection.Controls.Add((Control) this.panel2);
      this.pnlAllFolderSelection.Controls.Add((Control) this.panel3);
      this.pnlAllFolderSelection.Controls.Add((Control) this.pnlglobalsearch);
      this.pnlAllFolderSelection.Location = new Point(3, 3);
      this.pnlAllFolderSelection.Name = "pnlAllFolderSelection";
      this.pnlAllFolderSelection.Size = new Size(1228, 25);
      this.pnlAllFolderSelection.TabIndex = 20;
      this.panel1.Controls.Add((Control) this.label2);
      this.panel1.Controls.Add((Control) this.cboFolder);
      this.panel1.Location = new Point(3, 3);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(258, 22);
      this.panel1.TabIndex = 0;
      this.cboFolder.CheckOnClick = true;
      this.cboFolder.ComboBoxText = "";
      this.cboFolder.Display = false;
      this.cboFolder.DisplayMember = "DisplayName";
      this.cboFolder.DropDownHeight = 1;
      this.cboFolder.ForceRefresh = false;
      this.cboFolder.FormattingEnabled = true;
      this.cboFolder.IntegralHeight = false;
      this.cboFolder.Location = new Point(76, 0);
      this.cboFolder.MaxDropDownItems = 16;
      this.cboFolder.Name = "cboFolder";
      this.cboFolder.Size = new Size(178, 22);
      this.cboFolder.TabIndex = 22;
      this.cboFolder.DropDown += new EventHandler(this.cboFolder_DropDown);
      this.cboFolder.DropDownClosed += new EventHandler(this.cboFolder_DropDownClosed);
      this.cboFolder.MouseHover += new EventHandler(this.cboFolder_MouseHover);
      this.cboFolder.ItemList.ItemCheck += new ItemCheckEventHandler(this.cboFolder_ItemCheck);
      this.panel2.Controls.Add((Control) this.chkarchive);
      this.panel2.Location = new Point(267, 3);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(132, 21);
      this.panel2.TabIndex = 44;
      this.chkarchive.AutoSize = true;
      this.chkarchive.Enabled = false;
      this.chkarchive.Location = new Point(0, 2);
      this.chkarchive.Name = "chkarchive";
      this.chkarchive.Size = new Size(133, 18);
      this.chkarchive.TabIndex = 43;
      this.chkarchive.Text = "Include Archive Loans";
      this.chkarchive.UseVisualStyleBackColor = true;
      this.chkarchive.CheckedChanged += new EventHandler(this.chkarchive_CheckedChanged);
      this.panel3.Controls.Add((Control) this.label3);
      this.panel3.Controls.Add((Control) this.cboLoanType);
      this.panel3.Controls.Add((Control) this.lblCompany);
      this.panel3.Controls.Add((Control) this.cboCompany);
      this.panel3.Controls.Add((Control) this.textBox1);
      this.panel3.Controls.Add((Control) this.popUpButton);
      this.panel3.Location = new Point(405, 3);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(501, 23);
      this.panel3.TabIndex = 43;
      this.cboLoanType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboLoanType.FormattingEnabled = true;
      this.cboLoanType.Items.AddRange(new object[2]
      {
        (object) "All Loans",
        (object) "My Loans"
      });
      this.cboLoanType.Location = new Point(40, 0);
      this.cboLoanType.Name = "cboLoanType";
      this.cboLoanType.Size = new Size(121, 22);
      this.cboLoanType.TabIndex = 34;
      this.cboLoanType.SelectedIndexChanged += new EventHandler(this.cboLoanType_SelectedIndexChanged);
      this.lblCompany.AutoSize = true;
      this.lblCompany.Location = new Point(167, 2);
      this.lblCompany.Name = "lblCompany";
      this.lblCompany.Size = new Size(52, 14);
      this.lblCompany.TabIndex = 30;
      this.lblCompany.Text = "Company";
      this.cboCompany.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboCompany.DropDownWidth = 133;
      this.cboCompany.FormattingEnabled = true;
      this.cboCompany.Items.AddRange(new object[2]
      {
        (object) "Internal Organization",
        (object) "TPO"
      });
      this.cboCompany.Location = new Point(225, 0);
      this.cboCompany.Name = "cboCompany";
      this.cboCompany.Size = new Size(133, 22);
      this.cboCompany.TabIndex = 31;
      this.cboCompany.SelectedIndexChanged += new EventHandler(this.cboCompany_SelectedIndexChanged);
      this.textBox1.Location = new Point(364, 0);
      this.textBox1.Name = "textBox1";
      this.textBox1.ReadOnly = true;
      this.textBox1.Size = new Size(111, 20);
      this.textBox1.TabIndex = 32;
      this.popUpButton.BackColor = Color.Transparent;
      this.popUpButton.Location = new Point(481, 1);
      this.popUpButton.MouseDownImage = (Image) null;
      this.popUpButton.Name = "popUpButton";
      this.popUpButton.Size = new Size(16, 16);
      this.popUpButton.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.popUpButton.TabIndex = 33;
      this.popUpButton.TabStop = false;
      this.popUpButton.Click += new EventHandler(this.popUpButton_Click);
      this.pnlglobalsearch.Controls.Add((Control) this.pictureBox1);
      this.pnlglobalsearch.Controls.Add((Control) this.label5);
      this.pnlglobalsearch.Controls.Add((Control) this.rbGlobalSearchOff);
      this.pnlglobalsearch.Controls.Add((Control) this.rbGlobalSearchOn);
      this.pnlglobalsearch.Location = new Point(912, 3);
      this.pnlglobalsearch.Name = "pnlglobalsearch";
      this.pnlglobalsearch.Size = new Size(227, 21);
      this.pnlglobalsearch.TabIndex = 21;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(6, 3);
      this.label5.Name = "label5";
      this.label5.Size = new Size(75, 14);
      this.label5.TabIndex = 2;
      this.label5.Text = "Global Search";
      this.rbGlobalSearchOff.AutoSize = true;
      this.rbGlobalSearchOff.Location = new Point(128, 1);
      this.rbGlobalSearchOff.Name = "rbGlobalSearchOff";
      this.rbGlobalSearchOff.Size = new Size(41, 18);
      this.rbGlobalSearchOff.TabIndex = 1;
      this.rbGlobalSearchOff.TabStop = true;
      this.rbGlobalSearchOff.Text = "Off";
      this.rbGlobalSearchOff.UseVisualStyleBackColor = true;
      this.rbGlobalSearchOn.AutoSize = true;
      this.rbGlobalSearchOn.Location = new Point(85, 1);
      this.rbGlobalSearchOn.Name = "rbGlobalSearchOn";
      this.rbGlobalSearchOn.Size = new Size(39, 18);
      this.rbGlobalSearchOn.TabIndex = 0;
      this.rbGlobalSearchOn.TabStop = true;
      this.rbGlobalSearchOn.Text = "On";
      this.rbGlobalSearchOn.UseVisualStyleBackColor = true;
      this.rbGlobalSearchOn.CheckedChanged += new EventHandler(this.rbGlobalSearch_Changed);
      this.gradientPanel1.BackColorGlassyStyle = true;
      this.gradientPanel1.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gradientPanel1.Controls.Add((Control) this.btnLoanMailbox);
      this.gradientPanel1.Controls.Add((Control) this.btnRefreshView);
      this.gradientPanel1.Controls.Add((Control) this.btnManageViews);
      this.gradientPanel1.Controls.Add((Control) this.btnSaveView);
      this.gradientPanel1.Controls.Add((Control) this.cboView);
      this.gradientPanel1.Controls.Add((Control) this.label1);
      this.gradientPanel1.Controls.Add((Control) this.elmMailbox);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(81, 123, 184);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(167, 201, 239);
      this.gradientPanel1.GradientPaddingColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      this.gradientPanel1.Location = new Point(0, 0);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(1237, 31);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.PageHeader;
      this.gradientPanel1.TabIndex = 0;
      this.btnLoanMailbox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnLoanMailbox.BackColor = Color.Transparent;
      this.btnLoanMailbox.DisabledImage = (Image) null;
      this.btnLoanMailbox.Image = (Image) Resources.btn_loan_mailbox;
      this.btnLoanMailbox.Location = new Point(1148, 6);
      this.btnLoanMailbox.MouseDownImage = (Image) null;
      this.btnLoanMailbox.MouseOverImage = (Image) Resources.btn_loan_mailbox_over;
      this.btnLoanMailbox.Name = "btnLoanMailbox";
      this.btnLoanMailbox.Size = new Size(83, 20);
      this.btnLoanMailbox.SizeMode = PictureBoxSizeMode.AutoSize;
      this.btnLoanMailbox.TabIndex = 6;
      this.btnLoanMailbox.TabStop = false;
      this.btnLoanMailbox.Click += new EventHandler(this.onOpenLoanMailbox);
      this.cboView.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboView.FormattingEnabled = true;
      this.cboView.Location = new Point(111, 5);
      this.cboView.Name = "cboView";
      this.cboView.SelectedBGColor = SystemColors.Highlight;
      this.cboView.Size = new Size(219, 21);
      this.cboView.TabIndex = 1;
      this.cboView.SelectedIndexChanged += new EventHandler(this.cboView_SelectedIndexChanged);
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(7, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(97, 16);
      this.label1.TabIndex = 0;
      this.label1.Text = "Pipeline View";
      this.elmMailbox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.elmMailbox.BackColor = Color.Transparent;
      this.elmMailbox.Element = (object) null;
      this.elmMailbox.ElementAlignment = ContentAlignment.MiddleRight;
      this.elmMailbox.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.elmMailbox.ForeColor = Color.FromArgb(29, 110, 174);
      this.elmMailbox.Location = new Point(1124, 6);
      this.elmMailbox.Name = "elmMailbox";
      this.elmMailbox.Size = new Size(24, 20);
      this.elmMailbox.TabIndex = 5;
      this.elmMailbox.Text = "elementControl1";
      this.label3.AutoSize = true;
      this.label3.BackColor = Color.Transparent;
      this.label3.Location = new Point(3, 3);
      this.label3.Name = "label3";
      this.label3.Size = new Size(33, 14);
      this.label3.TabIndex = 35;
      this.label3.Text = "View";
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.Location = new Point(6, 4);
      this.label2.Name = "label2";
      this.label2.Size = new Size(64, 14);
      this.label2.TabIndex = 30;
      this.label2.Text = "Loan Folder";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.gradientPanel3);
      this.Controls.Add((Control) this.gradientPanel2);
      this.Controls.Add((Control) this.gradientPanel1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (PipelineScreen);
      this.Size = new Size(1237, 533);
      ((ISupportInitialize) this.btnPrint).EndInit();
      ((ISupportInitialize) this.btnExport).EndInit();
      ((ISupportInitialize) this.btnRefresh).EndInit();
      ((ISupportInitialize) this.btnDelete).EndInit();
      ((ISupportInitialize) this.btnDuplicate).EndInit();
      ((ISupportInitialize) this.btnEdit).EndInit();
      ((ISupportInitialize) this.btnNew).EndInit();
      ((ISupportInitialize) this.btnRefreshView).EndInit();
      ((ISupportInitialize) this.btnManageViews).EndInit();
      ((ISupportInitialize) this.btnSaveView).EndInit();
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.mnuLoans.ResumeLayout(false);
      this.groupContainer1.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      this.gradientPanel3.ResumeLayout(false);
      this.gradientPanel3.PerformLayout();
      this.gradientPanel2.ResumeLayout(false);
      this.pnlAllFolderSelection.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.panel3.ResumeLayout(false);
      this.panel3.PerformLayout();
      ((ISupportInitialize) this.popUpButton).EndInit();
      this.pnlglobalsearch.ResumeLayout(false);
      this.pnlglobalsearch.PerformLayout();
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      ((ISupportInitialize) this.btnLoanMailbox).EndInit();
      this.ResumeLayout(false);
    }

    internal struct OsVersionInfo
    {
      private readonly uint OsVersionInfoSize;
      internal readonly uint MajorVersion;
      internal readonly uint MinorVersion;
      private readonly uint BuildNumber;
      private readonly uint PlatformId;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
      private readonly string CSDVersion;
    }
  }
}
