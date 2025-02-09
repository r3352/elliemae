// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PipelineViewEditor
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.EMLite.UI.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class PipelineViewEditor : Form
  {
    private Sessions.Session session;
    private PersonaPipelineView view;
    private LoanReportFieldDefs fieldDefs;
    private GridViewReportFilterManager filterMgr;
    private GridViewLayoutManager layoutMgr;
    private FieldFilterList advFilter;
    private string[] inUseViewNames;
    private IList<LoanFolderInfo> activeFolders;
    private IList<LoanFolderInfo> archiveFolders;
    private IDictionary<string, int> map;
    private const string allActive = "<Select all Active Folders>";
    private const string allArchive = "<Select all Archive Folders>";
    private FeaturesAclManager aclMgr;
    private bool hasAccess;
    private ToolTip buttonToolTip;
    private const int MOUSEEVENT_LEFTDOWN = 2;
    private const int MOUSEEVENT_LEFTUP = 4;
    private IContainer components;
    private DialogButtons dlgButtons;
    private TextBox txtName;
    private Label label1;
    private BorderPanel borderPanel1;
    private GradientPanel gradientPanel3;
    private Label lblFilter;
    private Button btnClearSearch;
    private Button btnAdvSearch;
    private Label label4;
    private GradientPanel gradientPanel2;
    private RadioButton radViewMyLoans;
    private RadioButton radViewAllLoans;
    private Label label3;
    private CheckedComboBox cboFolder;
    private Label label2;
    private GridView gvLoans;
    private StandardIconButton btnCustomizeColumns;
    private ToolTip toolTip1;

    public PipelineViewEditor(
      Sessions.Session session,
      PersonaPipelineView view,
      LoanReportFieldDefs fieldDefs,
      string[] inUseViewNames)
    {
      this.session = session;
      this.fieldDefs = fieldDefs;
      this.view = view;
      this.inUseViewNames = inUseViewNames;
      this.activeFolders = (IList<LoanFolderInfo>) new List<LoanFolderInfo>();
      this.archiveFolders = (IList<LoanFolderInfo>) new List<LoanFolderInfo>();
      this.map = (IDictionary<string, int>) new Dictionary<string, int>();
      this.aclMgr = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      this.buttonToolTip = new ToolTip();
      bool flag = !this.aclMgr.GetPermission(AclFeature.LoanMgmt_SearchArchiveFolders, view.PersonaID);
      bool permission = this.aclMgr.GetPermission(AclFeature.LoanMgmt_AccessToArchiveFolders, view.PersonaID);
      this.hasAccess = Session.StartupInfo.EnableLoanSoftArchival ? permission : flag;
      this.InitializeComponent();
      this.loadFolders();
      this.loadView();
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
      PipelineViewEditor.mouse_event(6U, (uint) (Cursor.Position.X - 25), (uint) (Cursor.Position.Y + 25), 0U, 0U);
    }

    private void checkAllActiveFolders()
    {
      this.cboFolder.ItemList.SetItemChecked(this.map["<Select all Active Folders>"], true);
      foreach (LoanFolderInfo activeFolder in (IEnumerable<LoanFolderInfo>) this.activeFolders)
        this.cboFolder.ItemList.SetItemChecked(this.map[activeFolder.Name], true);
    }

    private void checkAllArchiveFolders()
    {
      this.cboFolder.ItemList.SetItemChecked(this.map["<Select all Archive Folders>"], true);
      foreach (LoanFolderInfo archiveFolder in (IEnumerable<LoanFolderInfo>) this.archiveFolders)
        this.cboFolder.ItemList.SetItemChecked(this.map[archiveFolder.Name], true);
    }

    private void findAllArchiveFolders(LoanFolderInfo[] folders)
    {
      foreach (LoanFolderInfo folder in folders)
      {
        if (folder.Type == LoanFolderInfo.LoanFolderType.Archive)
          this.archiveFolders.Add(folder);
      }
    }

    private void findAllActiveeFolders(LoanFolderInfo[] folders)
    {
      foreach (LoanFolderInfo folder in folders)
      {
        if (folder.Type == LoanFolderInfo.LoanFolderType.Regular)
          this.activeFolders.Add(folder);
      }
    }

    private void loadActiveArchiveFolders(LoanFolderInfo[] folders)
    {
      int num = 0;
      if (this.activeFolders.Count > 0 || this.hasAccess && this.archiveFolders.Count > 0)
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
        LoanFolderInfo tag = new LoanFolderInfo("<Select all Active Folders>");
        ComboBoxItem comboBoxItem = new ComboBoxItem(tag.Name, tag.DisplayName, (object) tag);
        this.cboFolder.Items.Add((object) comboBoxItem);
        if (!this.map.ContainsKey(comboBoxItem.Name))
          this.map.Add(comboBoxItem.Name, num);
        ++num;
      }
      if (this.hasAccess && this.archiveFolders.Count > 0)
      {
        LoanFolderInfo tag = new LoanFolderInfo("<Select all Archive Folders>");
        ComboBoxItem comboBoxItem = new ComboBoxItem(tag.Name, tag.DisplayName, (object) tag);
        this.cboFolder.Items.Add((object) comboBoxItem);
        if (!this.map.ContainsKey(comboBoxItem.Name))
          this.map.Add(comboBoxItem.Name, num);
        ++num;
      }
      foreach (LoanFolderInfo folder in folders)
      {
        ComboBoxItem comboBoxItem = new ComboBoxItem(folder.Name, folder.DisplayName, (object) folder);
        if (folder.Type == LoanFolderInfo.LoanFolderType.Regular || folder.Type == LoanFolderInfo.LoanFolderType.Archive && this.hasAccess)
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
      if (this.activeFolders.Count > 0 || this.hasAccess && this.archiveFolders.Count > 0)
        this.cboFolder.ItemList.SetItemChecked(this.map[SystemSettings.AllFolders], true);
      if (this.activeFolders.Count > 0)
        this.checkAllActiveFolders();
      if (this.hasAccess && this.archiveFolders.Count > 0)
        this.checkAllArchiveFolders();
      int num = this.hasAccess ? this.activeFolders.Count + this.archiveFolders.Count : this.activeFolders.Count;
      switch (num)
      {
        case 0:
          this.cboFolder.ComboBoxText = string.Empty;
          break;
        case 1:
          this.cboFolder.ComboBoxText = this.activeFolders.Count == 0 ? this.archiveFolders[0].Name : this.activeFolders[0].Name;
          break;
        default:
          this.cboFolder.ComboBoxText = num.ToString() + " folders are selected";
          break;
      }
    }

    private void selectFoldersFromFolderList(string[] folderList)
    {
      int num = 0;
      foreach (string folder in folderList)
      {
        if (this.map.ContainsKey(folder))
        {
          this.cboFolder.ItemList.SetItemChecked(this.map[folder], true);
          ++num;
        }
      }
      if (num == 0)
        this.cboFolder.ComboBoxText = string.Empty;
      else
        this.cboFolder.ComboBoxText = num == 1 ? folderList[0] : num.ToString() + " folders are selected";
    }

    private string getFolderList()
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

    private void loadFolders()
    {
      LoanFolderInfo[] allLoanFolderInfos = this.session.LoanManager.GetAllLoanFolderInfos(false);
      this.cboFolder.Items.Clear();
      Array.Sort<LoanFolderInfo>(allLoanFolderInfos);
      this.findAllActiveeFolders(allLoanFolderInfos);
      this.findAllArchiveFolders(allLoanFolderInfos);
      this.loadActiveArchiveFolders(allLoanFolderInfos);
    }

    private void loadViewFolders()
    {
      string loanFolders = this.view.LoanFolders;
      if (string.IsNullOrEmpty(loanFolders))
        return;
      string[] folderList = loanFolders.Split('|');
      if (folderList[0].Equals(SystemSettings.AllFolders))
        this.selectAllFolders();
      else
        this.selectFoldersFromFolderList(folderList);
    }

    private void loadView()
    {
      this.txtName.Text = this.view.Name;
      this.loadViewFolders();
      if (this.view.Ownership == PipelineViewLoanOwnership.All)
        this.radViewAllLoans.Checked = true;
      else
        this.radViewMyLoans.Checked = true;
      this.layoutMgr = new GridViewLayoutManager(this.gvLoans, this.getFullTableLayout(), this.getDefaultTableLayout());
      this.layoutMgr.LayoutChanged += new EventHandler(this.onLayoutChanged);
      this.filterMgr = new GridViewReportFilterManager(this.session, this.gvLoans);
      this.filterMgr.CreateColumnFilters((ReportFieldDefs) this.fieldDefs);
      this.filterMgr.FilterChanged += new EventHandler(this.onColumnFilterChanged);
      this.advFilter = this.view.Filter;
      this.refreshFilterDescription();
    }

    private void onColumnFilterChanged(object sender, EventArgs e)
    {
      this.refreshFilterDescription();
    }

    private TableLayout getFullTableLayout()
    {
      TableLayout fullTableLayout = new TableLayout();
      foreach (LoanReportFieldDef fieldDef in (ReportFieldDefContainer) this.fieldDefs)
      {
        if (fullTableLayout.GetColumnByID(fieldDef.CriterionFieldName) == null)
          fullTableLayout.AddColumn(ReportFieldClientExtension.ToTableLayoutColumn(fieldDef));
      }
      fullTableLayout.SortByDescription();
      return fullTableLayout;
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

    public FieldFilterList GetCurrentFilter()
    {
      FieldFilterList fieldFilterList = this.filterMgr.ToFieldFilterList();
      if (this.advFilter != null)
        fieldFilterList.Merge(this.advFilter);
      return fieldFilterList;
    }

    private void onLayoutChanged(object sender, EventArgs e)
    {
      this.filterMgr.CreateColumnFilters((ReportFieldDefs) this.fieldDefs);
    }

    private TableLayout getDefaultTableLayout()
    {
      TableLayout defaultTableLayout = new TableLayout();
      foreach (PersonaPipelineViewColumn column in this.view.Columns)
      {
        LoanReportFieldDef fieldByCriterionName = this.fieldDefs.GetFieldByCriterionName(column.ColumnDBName);
        if (fieldByCriterionName != null)
        {
          TableLayout.Column tableLayoutColumn = ReportFieldClientExtension.ToTableLayoutColumn(fieldByCriterionName);
          tableLayoutColumn.SortOrder = column.SortOrder;
          if (column.Width >= 0)
            tableLayoutColumn.Width = column.Width;
          defaultTableLayout.AddColumn(tableLayoutColumn);
        }
      }
      return defaultTableLayout;
    }

    private void btnAdvSearch_Click(object sender, EventArgs e)
    {
      using (PipelineViewAdvSearchDialog viewAdvSearchDialog = new PipelineViewAdvSearchDialog(this.fieldDefs, this.GetCurrentFilter()))
      {
        if (viewAdvSearchDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.advFilter = viewAdvSearchDialog.GetSelectedFilter();
        this.filterMgr.ClearColumnFilters();
        this.refreshFilterDescription();
      }
    }

    private void btnClearSearch_Click(object sender, EventArgs e)
    {
      this.advFilter = (FieldFilterList) null;
      this.filterMgr.ClearColumnFilters();
      this.refreshFilterDescription();
    }

    private void dlgButtons_OK(object sender, EventArgs e)
    {
      if (!this.validateView())
        return;
      this.view.Name = this.txtName.Text.Trim();
      this.view.Filter = this.GetCurrentFilter();
      this.view.LoanFolders = this.getFolderList();
      this.view.Ownership = this.radViewAllLoans.Checked ? PipelineViewLoanOwnership.All : PipelineViewLoanOwnership.User;
      this.view.Columns.Clear();
      foreach (TableLayout.Column column in this.layoutMgr.GetCurrentLayout())
        this.view.Columns.Add(column.ColumnID, column.SortPriority == 0 ? column.SortOrder : SortOrder.None, column.Width);
      this.DialogResult = DialogResult.OK;
    }

    private bool validateView()
    {
      string strA = this.txtName.Text.Trim();
      if (strA == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must provide a name for this view.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      for (int index = 0; index < this.inUseViewNames.Length; ++index)
      {
        if (string.Compare(strA, this.inUseViewNames[index], true) == 0)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The name '" + strA + "' is already in use by another view. You must select a new name for the view.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return false;
        }
      }
      if (this.gvLoans.Columns.Count != 0)
        return true;
      int num1 = (int) Utils.Dialog((IWin32Window) this, "You must include at least one column in this Pipeline View.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      return false;
    }

    private void btnCustomizeColumns_Click(object sender, EventArgs e)
    {
      this.layoutMgr.CustomizeColumns();
    }

    private void cboFolder_Dropdown(object sender, EventArgs e)
    {
    }

    private void cboFolder_DropdownClosed(object sender, EventArgs e)
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
            this.cboFolder.ComboBoxText = "";
            this.view.LoanFolders = "";
            return;
          case 1:
            this.cboFolder.Text = ((LoanFolderInfo) ((ComboBoxItem) this.cboFolder.ItemList.CheckedItems[0]).Tag).Name;
            break;
        }
        this.view.LoanFolders = this.getFolderList();
        this.loadViewFolders();
      }
      else
      {
        foreach (KeyValuePair<string, int> keyValuePair in (IEnumerable<KeyValuePair<string, int>>) this.map)
          this.cboFolder.ItemList.SetItemChecked(keyValuePair.Value, false);
        this.loadViewFolders();
      }
    }

    private void cboFolder_ItemCheck(object sender, ItemCheckEventArgs e)
    {
      if (!this.cboFolder.Display)
        return;
      bool flag1 = !this.map.ContainsKey("<Select all Active Folders>") || this.cboFolder.ItemList.GetItemChecked(this.map["<Select all Active Folders>"]);
      bool flag2 = !this.map.ContainsKey("<Select all Archive Folders>") || this.cboFolder.ItemList.GetItemChecked(this.map["<Select all Archive Folders>"]);
      this.cboFolder.ItemList.ItemCheck -= new ItemCheckEventHandler(this.cboFolder_ItemCheck);
      string name = ((ComboBoxItem) this.cboFolder.Items[e.Index]).Name;
      if (name.Equals(SystemSettings.AllFolders))
      {
        if (e.NewValue == CheckState.Checked)
        {
          if (this.activeFolders.Count > 0)
          {
            this.cboFolder.ItemList.SetItemChecked(this.map["<Select all Active Folders>"], true);
            flag1 = true;
          }
          if (this.hasAccess && this.archiveFolders.Count > 0)
          {
            this.cboFolder.ItemList.SetItemChecked(this.map["<Select all Archive Folders>"], true);
            flag2 = true;
          }
        }
        else if (e.NewValue == CheckState.Unchecked)
        {
          if (this.activeFolders.Count > 0)
          {
            this.cboFolder.ItemList.SetItemChecked(this.map["<Select all Active Folders>"], false);
            flag1 = false;
          }
          if (this.hasAccess && this.archiveFolders.Count > 0)
          {
            this.cboFolder.ItemList.SetItemChecked(this.map["<Select all Archive Folders>"], false);
            flag2 = false;
          }
        }
      }
      if (name.Equals(SystemSettings.AllFolders) || name.Equals("<Select all Active Folders>"))
      {
        if (e.NewValue == CheckState.Checked)
          flag1 = true;
        else if (e.NewValue == CheckState.Unchecked)
          flag1 = false;
        foreach (LoanFolderInfo activeFolder in (IEnumerable<LoanFolderInfo>) this.activeFolders)
        {
          int index = this.map[activeFolder.Name];
          if (e.NewValue == CheckState.Checked)
            this.cboFolder.ItemList.SetItemChecked(index, true);
          else if (e.NewValue == CheckState.Unchecked)
            this.cboFolder.ItemList.SetItemChecked(index, false);
        }
      }
      if (name.Equals(SystemSettings.AllFolders) || name.Equals("<Select all Archive Folders>"))
      {
        if (e.NewValue == CheckState.Checked)
          flag2 = true;
        else if (e.NewValue == CheckState.Unchecked)
          flag2 = false;
        if (this.hasAccess && this.archiveFolders.Count > 0)
        {
          foreach (LoanFolderInfo archiveFolder in (IEnumerable<LoanFolderInfo>) this.archiveFolders)
          {
            int index = this.map[archiveFolder.Name];
            if (e.NewValue == CheckState.Checked)
              this.cboFolder.ItemList.SetItemChecked(index, true);
            else if (e.NewValue == CheckState.Unchecked)
              this.cboFolder.ItemList.SetItemChecked(index, false);
          }
        }
      }
      else
      {
        LoanFolderInfo tag = (LoanFolderInfo) ((ComboBoxItem) this.cboFolder.Items[e.Index]).Tag;
        if (tag.Type == LoanFolderInfo.LoanFolderType.Regular)
        {
          if (e.NewValue == CheckState.Unchecked)
          {
            this.cboFolder.ItemList.SetItemChecked(this.map["<Select all Active Folders>"], false);
            flag1 = false;
          }
          else if (e.NewValue == CheckState.Checked)
          {
            foreach (LoanFolderInfo activeFolder in (IEnumerable<LoanFolderInfo>) this.activeFolders)
            {
              this.cboFolder.ItemList.SetItemChecked(this.map["<Select all Active Folders>"], true);
              flag1 = true;
              if (!activeFolder.Name.Equals(tag.Name) && !this.cboFolder.ItemList.GetItemChecked(this.map[activeFolder.Name]))
              {
                this.cboFolder.ItemList.SetItemChecked(this.map["<Select all Active Folders>"], false);
                flag1 = false;
                break;
              }
            }
          }
        }
        else if (tag.Type == LoanFolderInfo.LoanFolderType.Archive)
        {
          if (e.NewValue == CheckState.Unchecked)
          {
            this.cboFolder.ItemList.SetItemChecked(this.map["<Select all Archive Folders>"], false);
            flag2 = false;
          }
          else if (e.NewValue == CheckState.Checked)
          {
            foreach (LoanFolderInfo archiveFolder in (IEnumerable<LoanFolderInfo>) this.archiveFolders)
            {
              this.cboFolder.ItemList.SetItemChecked(this.map["<Select all Archive Folders>"], true);
              flag2 = true;
              if (!archiveFolder.Name.Equals(tag.Name) && !this.cboFolder.ItemList.GetItemChecked(this.map[archiveFolder.Name]))
              {
                this.cboFolder.ItemList.SetItemChecked(this.map["<Select all Archive Folders>"], false);
                flag2 = false;
                break;
              }
            }
          }
        }
      }
      bool flag3 = flag1 & flag2;
      this.cboFolder.ItemList.SetItemChecked(this.map[SystemSettings.AllFolders], flag3);
      this.cboFolder.ItemList.ItemCheck += new ItemCheckEventHandler(this.cboFolder_ItemCheck);
    }

    private void cboFolder_MouseHover(object sender, EventArgs e)
    {
      this.buttonToolTip.ToolTipTitle = "Selected Folders:";
      this.buttonToolTip.SetToolTip((Control) this.cboFolder, this.getFolderList());
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
      this.dlgButtons = new DialogButtons();
      this.txtName = new TextBox();
      this.label1 = new Label();
      this.borderPanel1 = new BorderPanel();
      this.gvLoans = new GridView();
      this.gradientPanel3 = new GradientPanel();
      this.lblFilter = new Label();
      this.btnClearSearch = new Button();
      this.btnAdvSearch = new Button();
      this.label4 = new Label();
      this.gradientPanel2 = new GradientPanel();
      this.btnCustomizeColumns = new StandardIconButton();
      this.radViewMyLoans = new RadioButton();
      this.radViewAllLoans = new RadioButton();
      this.label3 = new Label();
      this.cboFolder = new CheckedComboBox(this.components);
      this.label2 = new Label();
      this.toolTip1 = new ToolTip(this.components);
      this.borderPanel1.SuspendLayout();
      this.gradientPanel3.SuspendLayout();
      this.gradientPanel2.SuspendLayout();
      ((ISupportInitialize) this.btnCustomizeColumns).BeginInit();
      this.SuspendLayout();
      this.dlgButtons.Dock = DockStyle.Bottom;
      this.dlgButtons.Location = new Point(0, 230);
      this.dlgButtons.Name = "dlgButtons";
      this.dlgButtons.Size = new Size(685, 44);
      this.dlgButtons.TabIndex = 0;
      this.dlgButtons.OK += new EventHandler(this.dlgButtons_OK);
      this.txtName.Location = new Point(46, 10);
      this.txtName.Name = "txtName";
      this.txtName.Size = new Size(245, 20);
      this.txtName.TabIndex = 1;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(7, 14);
      this.label1.Name = "label1";
      this.label1.Size = new Size(34, 14);
      this.label1.TabIndex = 2;
      this.label1.Text = "Name";
      this.borderPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.borderPanel1.Controls.Add((Control) this.gvLoans);
      this.borderPanel1.Controls.Add((Control) this.gradientPanel3);
      this.borderPanel1.Controls.Add((Control) this.gradientPanel2);
      this.borderPanel1.Location = new Point(10, 42);
      this.borderPanel1.Name = "borderPanel1";
      this.borderPanel1.Size = new Size(665, 188);
      this.borderPanel1.TabIndex = 3;
      this.gvLoans.AllowColumnReorder = true;
      this.gvLoans.BorderStyle = BorderStyle.None;
      this.gvLoans.Dock = DockStyle.Fill;
      this.gvLoans.FilterVisible = true;
      this.gvLoans.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvLoans.Location = new Point(1, 62);
      this.gvLoans.Name = "gvLoans";
      this.gvLoans.Size = new Size(663, 125);
      this.gvLoans.SortHistory = 0;
      this.gvLoans.TabIndex = 5;
      this.gradientPanel3.Borders = AnchorStyles.Left | AnchorStyles.Right;
      this.gradientPanel3.Controls.Add((Control) this.lblFilter);
      this.gradientPanel3.Controls.Add((Control) this.btnClearSearch);
      this.gradientPanel3.Controls.Add((Control) this.btnAdvSearch);
      this.gradientPanel3.Controls.Add((Control) this.label4);
      this.gradientPanel3.Dock = DockStyle.Top;
      this.gradientPanel3.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel3.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel3.Location = new Point(1, 32);
      this.gradientPanel3.Name = "gradientPanel3";
      this.gradientPanel3.Size = new Size(663, 30);
      this.gradientPanel3.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel3.TabIndex = 4;
      this.lblFilter.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblFilter.AutoEllipsis = true;
      this.lblFilter.BackColor = Color.Transparent;
      this.lblFilter.Location = new Point(38, 9);
      this.lblFilter.Name = "lblFilter";
      this.lblFilter.Size = new Size(445, 14);
      this.lblFilter.TabIndex = 7;
      this.btnClearSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnClearSearch.Enabled = false;
      this.btnClearSearch.Location = new Point(602, 5);
      this.btnClearSearch.Name = "btnClearSearch";
      this.btnClearSearch.Padding = new Padding(2, 0, 0, 0);
      this.btnClearSearch.Size = new Size(56, 22);
      this.btnClearSearch.TabIndex = 6;
      this.btnClearSearch.Text = "&Clear";
      this.btnClearSearch.UseVisualStyleBackColor = true;
      this.btnClearSearch.Click += new EventHandler(this.btnClearSearch_Click);
      this.btnAdvSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAdvSearch.Location = new Point(488, 5);
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
      this.gradientPanel2.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gradientPanel2.Controls.Add((Control) this.btnCustomizeColumns);
      this.gradientPanel2.Controls.Add((Control) this.radViewMyLoans);
      this.gradientPanel2.Controls.Add((Control) this.radViewAllLoans);
      this.gradientPanel2.Controls.Add((Control) this.label3);
      this.gradientPanel2.Controls.Add((Control) this.cboFolder);
      this.gradientPanel2.Controls.Add((Control) this.label2);
      this.gradientPanel2.Dock = DockStyle.Top;
      this.gradientPanel2.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel2.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel2.Location = new Point(1, 1);
      this.gradientPanel2.Name = "gradientPanel2";
      this.gradientPanel2.Size = new Size(663, 31);
      this.gradientPanel2.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel2.TabIndex = 3;
      this.btnCustomizeColumns.BackColor = Color.Transparent;
      this.btnCustomizeColumns.Location = new Point(446, 7);
      this.btnCustomizeColumns.MouseDownImage = (Image) null;
      this.btnCustomizeColumns.Name = "btnCustomizeColumns";
      this.btnCustomizeColumns.Size = new Size(16, 16);
      this.btnCustomizeColumns.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnCustomizeColumns.TabIndex = 7;
      this.btnCustomizeColumns.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnCustomizeColumns, "Customize Columns");
      this.btnCustomizeColumns.Click += new EventHandler(this.btnCustomizeColumns_Click);
      this.radViewMyLoans.AutoSize = true;
      this.radViewMyLoans.BackColor = Color.Transparent;
      this.radViewMyLoans.Location = new Point(368, 7);
      this.radViewMyLoans.Name = "radViewMyLoans";
      this.radViewMyLoans.Size = new Size(72, 18);
      this.radViewMyLoans.TabIndex = 6;
      this.radViewMyLoans.Text = "My Loans";
      this.radViewMyLoans.UseVisualStyleBackColor = false;
      this.radViewAllLoans.AutoSize = true;
      this.radViewAllLoans.BackColor = Color.Transparent;
      this.radViewAllLoans.Checked = true;
      this.radViewAllLoans.Location = new Point(295, 7);
      this.radViewAllLoans.Name = "radViewAllLoans";
      this.radViewAllLoans.Size = new Size(70, 18);
      this.radViewAllLoans.TabIndex = 5;
      this.radViewAllLoans.TabStop = true;
      this.radViewAllLoans.Text = "All Loans";
      this.radViewAllLoans.UseVisualStyleBackColor = false;
      this.label3.AutoSize = true;
      this.label3.BackColor = Color.Transparent;
      this.label3.Location = new Point(256, 9);
      this.label3.Name = "label3";
      this.label3.Size = new Size(33, 14);
      this.label3.TabIndex = 4;
      this.label3.Text = "View";
      this.cboFolder.CheckOnClick = true;
      this.cboFolder.ComboBoxText = "";
      this.cboFolder.Display = false;
      this.cboFolder.DisplayMember = "DisplayName";
      this.cboFolder.FormattingEnabled = true;
      this.cboFolder.Location = new Point(78, 5);
      this.cboFolder.Name = "cboFolder";
      this.cboFolder.Size = new Size(160, 22);
      this.cboFolder.TabIndex = 3;
      this.cboFolder.ItemList.ItemCheck += new ItemCheckEventHandler(this.cboFolder_ItemCheck);
      this.cboFolder.DropDownClosed += new EventHandler(this.cboFolder_DropdownClosed);
      this.cboFolder.MouseHover += new EventHandler(this.cboFolder_MouseHover);
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.Location = new Point(7, 9);
      this.label2.Name = "label2";
      this.label2.Size = new Size(64, 14);
      this.label2.TabIndex = 2;
      this.label2.Text = "Loan Folder";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(685, 274);
      this.Controls.Add((Control) this.borderPanel1);
      this.Controls.Add((Control) this.txtName);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.dlgButtons);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (PipelineViewEditor);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "New / Edit Pipeline View";
      this.borderPanel1.ResumeLayout(false);
      this.gradientPanel3.ResumeLayout(false);
      this.gradientPanel3.PerformLayout();
      this.gradientPanel2.ResumeLayout(false);
      this.gradientPanel2.PerformLayout();
      ((ISupportInitialize) this.btnCustomizeColumns).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
