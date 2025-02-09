// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SelectInvestorsPage
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.DataDocs;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.RemotingServices.Acl;
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
  public class SelectInvestorsPage : Form
  {
    private static string investorCategory = "Investor";
    private InvestorServicesAclManager investorServiceMgr;
    private int personaID;
    private AclFeature feature;
    private bool initial = true;
    private bool dirty;
    private ArrayList previousView;
    private InvestorServiceAclInfo.InvestorServicesDefaultSetting previousSettings = InvestorServiceAclInfo.InvestorServicesDefaultSetting.NotSpecified;
    private int selectOption = 2;
    private string userID = "";
    private Persona[] personaList;
    private bool isPersonal;
    private bool readOnly;
    private Sessions.Session session;
    private List<InvestorServiceAclInfo> availableServices = new List<InvestorServiceAclInfo>();
    private Dictionary<string, InvestorServiceUI> investors = new Dictionary<string, InvestorServiceUI>();
    private IContainer components;
    private ListView lsvInvestors;
    private ColumnHeader columnHeader1;
    private Button btnOk;
    private Button btnCancel;
    private ImageList imgListTv;
    private ContextMenu contMenu;
    private MenuItem miLinkTo;
    private MenuItem miDisconTo;
    private Label lblDisconnected;
    private Label lblLinked;
    private RadioButton rdbAll;
    private RadioButton rdbCustom;
    private RadioButton rdbNone;
    private Label lblDefaultSetting;
    private ContextMenu contMenuDefault;
    private MenuItem miDefaultLinkTo;
    private MenuItem miDefaultDisconFrom;

    public static string WellsFargoPartnerCompanyCode => "#WellsFargo#";

    public static string InvestorCategory => SelectInvestorsPage.investorCategory;

    public SelectInvestorsPage(
      Sessions.Session session,
      AclFeature feature,
      int personaId,
      bool readOnly,
      int option)
    {
      this.session = session;
      this.InitializeComponent();
      this.investorServiceMgr = (InvestorServicesAclManager) this.session.ACL.GetAclManager(AclCategory.InvestorServices);
      this.PopulateAvailableInvestors();
      this.previousView = new ArrayList();
      this.feature = feature;
      this.personaID = personaId;
      this.selectOption = option;
      this.loadInvestorServicesForPersona();
      this.readOnly = readOnly;
      this.MakeReadOnly(this.readOnly, false);
      if (!this.readOnly)
      {
        if (this.selectOption == 1)
        {
          this.selectAll(false);
          this.rdbCustom.Checked = true;
        }
        else
        {
          if (this.selectOption != 0)
            return;
          this.deselectAll(false);
          this.rdbCustom.Checked = true;
        }
      }
      else
        this.setView();
    }

    public SelectInvestorsPage(
      Sessions.Session session,
      AclFeature feature,
      string userID,
      Persona[] personaList,
      bool readOnly,
      int option)
    {
      this.session = session;
      this.InitializeComponent();
      this.userID = userID;
      this.PopulateAvailableInvestors();
      this.personaList = personaList;
      this.isPersonal = true;
      this.lblDisconnected.Visible = true;
      this.lblLinked.Visible = true;
      this.readOnly = readOnly;
      this.feature = feature;
      this.previousView = new ArrayList();
      this.selectOption = option;
      this.lsvInvestors.SmallImageList = this.imgListTv;
      this.lsvInvestors.ContextMenu = this.contMenu;
      this.MakeReadOnly(this.readOnly, false);
      this.investorServiceMgr = (InvestorServicesAclManager) this.session.ACL.GetAclManager(AclCategory.InvestorServices);
      this.loadInvestorServicesForUser();
      if (!this.readOnly)
      {
        if (this.selectOption == 1)
        {
          this.selectAll(true);
          this.rdbCustom.Checked = true;
        }
        else if (this.selectOption == 0)
        {
          this.deselectAll(true);
          this.rdbCustom.Checked = true;
        }
      }
      if (this.selectOption != 2)
        return;
      this.setView();
    }

    public bool IsReadOnly
    {
      set => this.MakeReadOnly(value, false);
    }

    private void MakeReadOnly(bool makeReadOnly, bool makeInvestorReadOnly)
    {
      if (makeReadOnly)
      {
        this.btnOk.Enabled = false;
        this.rdbAll.Enabled = false;
        this.rdbCustom.Enabled = false;
        this.rdbNone.Enabled = false;
        this.lblDefaultSetting.ContextMenu = (ContextMenu) null;
      }
      else
      {
        this.btnOk.Enabled = true;
        this.lsvInvestors.ContextMenu = this.contMenu;
        this.lblDefaultSetting.ContextMenu = this.contMenuDefault;
        this.rdbAll.Enabled = true;
        this.rdbCustom.Enabled = true;
        this.rdbNone.Enabled = true;
      }
      if (!makeInvestorReadOnly)
        return;
      this.lsvInvestors.ContextMenu = (ContextMenu) null;
    }

    private void selectAll(bool custom)
    {
      this.initial = false;
      foreach (ListViewItem listViewItem in this.lsvInvestors.Items)
      {
        listViewItem.Checked = true;
        if (custom)
        {
          ((InvestorServiceUI) listViewItem.Tag).InvestorServiceAclInfo.CustomAccess = AclResourceAccess.ReadWrite;
          listViewItem.ImageIndex = 0;
        }
        else
          ((InvestorServiceUI) listViewItem.Tag).InvestorServiceAclInfo.PersonaAccess = AclResourceAccess.ReadWrite;
      }
      this.dirty = true;
    }

    private void deselectAll(bool custom)
    {
      this.initial = false;
      foreach (ListViewItem listViewItem in this.lsvInvestors.Items)
      {
        listViewItem.Checked = false;
        if (custom)
        {
          ((InvestorServiceUI) listViewItem.Tag).InvestorServiceAclInfo.CustomAccess = AclResourceAccess.ReadOnly;
          listViewItem.ImageIndex = 0;
        }
        else
          ((InvestorServiceUI) listViewItem.Tag).InvestorServiceAclInfo.PersonaAccess = AclResourceAccess.ReadOnly;
      }
      this.dirty = true;
    }

    private void setView()
    {
      int length1 = this.lsvInvestors.Columns[0].Text.Length;
      this.previousView = new ArrayList();
      int num = 0;
      for (int index = 0; index < this.lsvInvestors.Items.Count; ++index)
      {
        ListViewItem listViewItem = this.lsvInvestors.Items[index];
        int length2 = listViewItem.Text.Length;
        if (length2 > num)
          num = length2;
        this.previousView.Add(listViewItem.Clone());
      }
      if (length1 < num)
        this.lsvInvestors.Columns[0].Width = -1;
      else
        this.lsvInvestors.Columns[0].Width = -2;
    }

    private void PopulateAvailableInvestors()
    {
      this.investors = new Dictionary<string, InvestorServiceUI>();
      int num = 0;
      foreach (PartnerResponseBody partnerResponseBody in new DataDocsServiceHelper(this.session).GetInvestorsList().Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (provider => provider.Category.Equals(SelectInvestorsPage.investorCategory, StringComparison.InvariantCultureIgnoreCase))))
      {
        string str = partnerResponseBody.PartnerID + partnerResponseBody.ProductName;
        if (!this.investors.ContainsKey(str))
        {
          InvestorServiceAclInfo investorServiceAclInfo = new InvestorServiceAclInfo(str, 9007, SelectInvestorsPage.investorCategory);
          InvestorServiceUI investorServiceUi = new InvestorServiceUI(investorServiceAclInfo, partnerResponseBody.DisplayName, ++num);
          this.availableServices.Add(investorServiceAclInfo);
          this.investors.Add(str, investorServiceUi);
        }
      }
    }

    private void loadInvestorServicesForPersona()
    {
      this.initial = true;
      this.dirty = false;
      InvestorServiceAclInfo.InvestorServicesDefaultSetting servicesDefaultSetting = this.investorServiceMgr.GetInvestorServicesDefaultSetting(this.feature, this.personaID, SelectInvestorsPage.investorCategory);
      this.previousSettings = servicesDefaultSetting;
      InvestorServiceAclInfo[] permissions = this.investorServiceMgr.GetPermissions(this.feature, this.personaID, SelectInvestorsPage.investorCategory, this.availableServices.ToArray());
      this.lsvInvestors.Items.Clear();
      if (this.investors != null && this.investors.Count > 0)
        this.lsvInvestors.ListViewItemSorter = (IComparer) this.investors.Values.First<InvestorServiceUI>();
      foreach (InvestorServiceAclInfo investorServiceAclInfo in permissions)
      {
        InvestorServiceUI investorServiceUi = (InvestorServiceUI) null;
        this.investors.TryGetValue(investorServiceAclInfo.ProviderCompanyCode, out investorServiceUi);
        if (investorServiceUi != null)
        {
          investorServiceUi.InvestorServiceAclInfo = investorServiceAclInfo;
          ListViewItem listViewItem = new ListViewItem(investorServiceUi.DisplayName);
          listViewItem.Tag = (object) investorServiceUi;
          listViewItem.Checked = investorServiceAclInfo.PersonaAccess == AclResourceAccess.ReadWrite;
          this.lsvInvestors.Items.Add(listViewItem);
          switch (servicesDefaultSetting)
          {
            case InvestorServiceAclInfo.InvestorServicesDefaultSetting.None:
              listViewItem.Checked = false;
              listViewItem.ForeColor = Color.Gray;
              continue;
            case InvestorServiceAclInfo.InvestorServicesDefaultSetting.Custom:
              listViewItem.ForeColor = Color.Black;
              continue;
            case InvestorServiceAclInfo.InvestorServicesDefaultSetting.All:
              listViewItem.Checked = true;
              listViewItem.ForeColor = Color.Gray;
              continue;
            default:
              continue;
          }
        }
      }
      switch (servicesDefaultSetting)
      {
        case InvestorServiceAclInfo.InvestorServicesDefaultSetting.None:
          this.rdbNone.Checked = true;
          break;
        case InvestorServiceAclInfo.InvestorServicesDefaultSetting.Custom:
          this.rdbCustom.Checked = true;
          break;
        case InvestorServiceAclInfo.InvestorServicesDefaultSetting.All:
          this.rdbAll.Checked = true;
          break;
      }
      this.setView();
    }

    private void loadInvestorServicesForUser()
    {
      this.initial = true;
      this.dirty = false;
      InvestorServiceAclInfo.InvestorServicesDefaultSetting servicesDefaultSetting = this.investorServiceMgr.GetUserSpecificInvestorServicesDefaultSetting(this.feature, this.userID, SelectInvestorsPage.investorCategory, this.personaList);
      InvestorServiceAclInfo[] permissions = this.investorServiceMgr.GetPermissions(this.feature, this.userID, SelectInvestorsPage.investorCategory, this.personaList, this.availableServices.ToArray());
      this.lsvInvestors.Items.Clear();
      if (this.investors != null && this.investors.Count > 0)
        this.lsvInvestors.ListViewItemSorter = (IComparer) this.investors.Values.First<InvestorServiceUI>();
      foreach (InvestorServiceAclInfo investorServiceAclInfo in permissions)
      {
        InvestorServiceUI investorServiceUi = (InvestorServiceUI) null;
        this.investors.TryGetValue(investorServiceAclInfo.ProviderCompanyCode, out investorServiceUi);
        if (investorServiceUi != null)
        {
          investorServiceUi.InvestorServiceAclInfo = investorServiceAclInfo;
          ListViewItem listViewItem = new ListViewItem(investorServiceUi.DisplayName);
          listViewItem.Tag = (object) investorServiceUi;
          if (investorServiceAclInfo.CustomAccess != AclResourceAccess.None)
          {
            listViewItem.Checked = investorServiceAclInfo.CustomAccess == AclResourceAccess.ReadWrite;
            listViewItem.ImageIndex = 0;
          }
          else
          {
            listViewItem.Checked = investorServiceAclInfo.PersonaAccess == AclResourceAccess.ReadWrite;
            listViewItem.ImageIndex = 1;
          }
          this.lsvInvestors.Items.Add(listViewItem);
        }
      }
      if (servicesDefaultSetting == InvestorServiceAclInfo.InvestorServicesDefaultSetting.NotSpecified)
      {
        this.lblDefaultSetting.ImageIndex = 1;
        servicesDefaultSetting = this.investorServiceMgr.GetInvestorServicesDefaultSetting(this.feature, this.userID, SelectInvestorsPage.investorCategory, this.personaList);
      }
      else
        this.lblDefaultSetting.ImageIndex = 0;
      switch (servicesDefaultSetting)
      {
        case InvestorServiceAclInfo.InvestorServicesDefaultSetting.None:
          this.lsvInvestors.ContextMenu = (ContextMenu) null;
          this.rdbNone.Checked = true;
          break;
        case InvestorServiceAclInfo.InvestorServicesDefaultSetting.Custom:
          this.rdbCustom.Checked = true;
          break;
        case InvestorServiceAclInfo.InvestorServicesDefaultSetting.All:
          this.lsvInvestors.ContextMenu = (ContextMenu) null;
          this.rdbAll.Checked = true;
          break;
      }
      this.setView();
    }

    private void lsvInvestors_Click(object sender, EventArgs e) => this.initial = false;

    public int GetImageIndex()
    {
      int imageIndex = 0;
      if (this.lblDefaultSetting.ImageIndex == 0)
      {
        imageIndex = 1;
      }
      else
      {
        foreach (ListViewItem listViewItem in this.lsvInvestors.Items)
        {
          if (listViewItem.ImageIndex == 0)
          {
            imageIndex = 1;
            break;
          }
        }
      }
      return imageIndex;
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      if (this.rdbAll.Checked)
        this.previousSettings = InvestorServiceAclInfo.InvestorServicesDefaultSetting.All;
      else if (this.rdbCustom.Checked)
        this.previousSettings = InvestorServiceAclInfo.InvestorServicesDefaultSetting.Custom;
      else if (this.rdbNone.Checked)
        this.previousSettings = InvestorServiceAclInfo.InvestorServicesDefaultSetting.None;
      this.DialogResult = DialogResult.OK;
      this.setView();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.initial = true;
      this.lsvInvestors.Items.Clear();
      if (this.previousSettings != InvestorServiceAclInfo.InvestorServicesDefaultSetting.NotSpecified)
      {
        switch (this.previousSettings)
        {
          case InvestorServiceAclInfo.InvestorServicesDefaultSetting.None:
            this.rdbNone.Checked = true;
            break;
          case InvestorServiceAclInfo.InvestorServicesDefaultSetting.Custom:
            this.rdbCustom.Checked = true;
            break;
          case InvestorServiceAclInfo.InvestorServicesDefaultSetting.All:
            this.rdbAll.Checked = true;
            break;
        }
      }
      foreach (ListViewItem listViewItem in this.previousView)
        this.lsvInvestors.Items.Add((ListViewItem) listViewItem.Clone());
      this.dirty = false;
      this.DialogResult = DialogResult.Cancel;
    }

    public bool HasBeenModified => this.dirty;

    public void SaveData()
    {
      List<InvestorServiceAclInfo> investorServiceAclInfoList = new List<InvestorServiceAclInfo>();
      foreach (ListViewItem listViewItem in this.lsvInvestors.Items)
      {
        InvestorServiceAclInfo investorServiceAclInfo = ((InvestorServiceUI) listViewItem.Tag).InvestorServiceAclInfo;
        investorServiceAclInfoList.Add(investorServiceAclInfo);
      }
      InvestorServiceAclInfo.InvestorServicesDefaultSetting defaultValue = InvestorServiceAclInfo.InvestorServicesDefaultSetting.All;
      if (this.rdbCustom.Checked)
        defaultValue = InvestorServiceAclInfo.InvestorServicesDefaultSetting.Custom;
      else if (this.rdbNone.Checked)
      {
        investorServiceAclInfoList = new List<InvestorServiceAclInfo>();
        defaultValue = InvestorServiceAclInfo.InvestorServicesDefaultSetting.None;
      }
      if (!this.isPersonal)
      {
        this.investorServiceMgr.SetPermissions(this.feature, investorServiceAclInfoList.ToArray(), this.personaID, SelectInvestorsPage.investorCategory, defaultValue);
        this.investorServiceMgr.SetDefaultValue(this.feature, this.personaID, SelectInvestorsPage.investorCategory, defaultValue);
      }
      else
      {
        if (this.lblDefaultSetting.ImageIndex == 1)
          defaultValue = InvestorServiceAclInfo.InvestorServicesDefaultSetting.NotSpecified;
        this.investorServiceMgr.SetPermissions(this.feature, investorServiceAclInfoList.ToArray(), this.userID, SelectInvestorsPage.investorCategory, defaultValue);
        this.investorServiceMgr.SetDefaultValue(this.feature, this.userID, SelectInvestorsPage.investorCategory, defaultValue);
      }
      this.dirty = false;
    }

    public bool HasSomethingChecked()
    {
      bool flag = false;
      foreach (ListViewItem listViewItem in this.lsvInvestors.Items)
      {
        if (listViewItem.Checked)
        {
          flag = true;
          break;
        }
      }
      return flag;
    }

    public ArrayList DataView
    {
      get => this.previousView;
      set => this.previousView = value;
    }

    private void lsvInvestors_ItemChecked(object sender, ItemCheckedEventArgs e)
    {
      if (this.initial || this.readOnly)
        return;
      this.dirty = true;
      InvestorServiceUI tag = (InvestorServiceUI) e.Item.Tag;
      InvestorServiceAclInfo investorServiceAclInfo = tag.InvestorServiceAclInfo;
      if (!this.isPersonal)
      {
        investorServiceAclInfo.PersonaAccess = !e.Item.Checked ? AclResourceAccess.ReadOnly : AclResourceAccess.ReadWrite;
      }
      else
      {
        investorServiceAclInfo.CustomAccess = !e.Item.Checked ? AclResourceAccess.ReadOnly : AclResourceAccess.ReadWrite;
        e.Item.ImageIndex = 0;
      }
      e.Item.Tag = (object) tag;
    }

    private void miLinkTo_Click(object sender, EventArgs e)
    {
      ListViewItem selectedItem = this.lsvInvestors.SelectedItems[0];
      this.initial = true;
      if (selectedItem != null && selectedItem.ImageIndex < 1)
      {
        InvestorServiceUI tag = (InvestorServiceUI) selectedItem.Tag;
        selectedItem.ImageIndex = 1;
        InvestorServiceAclInfo investorServiceAclInfo = tag.InvestorServiceAclInfo;
        if (investorServiceAclInfo != null)
          selectedItem.Checked = investorServiceAclInfo.PersonaAccess == AclResourceAccess.ReadWrite;
        investorServiceAclInfo.CustomAccess = AclResourceAccess.None;
        selectedItem.Tag = (object) tag;
        this.dirty = true;
      }
      this.initial = false;
    }

    private void miDisconTo_Click(object sender, EventArgs e)
    {
      ListViewItem selectedItem = this.lsvInvestors.SelectedItems[0];
      this.initial = true;
      if (selectedItem != null && selectedItem.ImageIndex == 1)
      {
        InvestorServiceUI tag = (InvestorServiceUI) selectedItem.Tag;
        selectedItem.ImageIndex = 0;
        InvestorServiceAclInfo investorServiceAclInfo = tag.InvestorServiceAclInfo;
        if (investorServiceAclInfo != null)
          investorServiceAclInfo.CustomAccess = !selectedItem.Checked ? AclResourceAccess.ReadOnly : AclResourceAccess.ReadWrite;
        selectedItem.Tag = (object) tag;
        this.dirty = true;
      }
      this.initial = false;
    }

    private void lsvInvestors_ItemCheck(object sender, ItemCheckEventArgs e)
    {
      if (this.initial)
        return;
      if (this.readOnly)
      {
        this.dirty = false;
        e.NewValue = e.CurrentValue;
      }
      else if (this.rdbAll.Checked)
      {
        e.NewValue = CheckState.Checked;
      }
      else
      {
        if (!this.rdbNone.Checked)
          return;
        e.NewValue = CheckState.Unchecked;
      }
    }

    private void rdb_CheckedChanged(object sender, EventArgs e)
    {
      if (this.initial)
        return;
      this.dirty = true;
      if (this.rdbAll.Checked)
      {
        foreach (ListViewItem listViewItem in this.lsvInvestors.Items)
        {
          listViewItem.Checked = true;
          listViewItem.ForeColor = Color.Gray;
        }
        this.lsvInvestors.ContextMenu = (ContextMenu) null;
      }
      else if (this.rdbCustom.Checked)
      {
        foreach (ListViewItem listViewItem in this.lsvInvestors.Items)
          listViewItem.ForeColor = Color.Black;
        if (this.isPersonal)
          this.lsvInvestors.ContextMenu = this.contMenu;
      }
      else
      {
        foreach (ListViewItem listViewItem in this.lsvInvestors.Items)
        {
          listViewItem.Checked = false;
          listViewItem.ForeColor = Color.Gray;
        }
        this.lsvInvestors.ContextMenu = (ContextMenu) null;
      }
      if (!this.isPersonal)
        return;
      this.lblDefaultSetting.ImageIndex = 0;
    }

    private void rdb_MouseEnter(object sender, EventArgs e) => this.initial = false;

    private void miDefaultDisconFrom_Click(object sender, EventArgs e)
    {
      this.dirty = true;
      this.lblDefaultSetting.ImageIndex = 0;
    }

    private void miDefaultLinkTo_Click(object sender, EventArgs e)
    {
      this.dirty = true;
      this.initial = true;
      switch (this.investorServiceMgr.GetPersonasInvestorServicesDefaultSetting(this.feature, this.userID, SelectInvestorsPage.investorCategory, this.personaList))
      {
        case InvestorServiceAclInfo.InvestorServicesDefaultSetting.None:
          this.rdbNone.Checked = true;
          break;
        case InvestorServiceAclInfo.InvestorServicesDefaultSetting.Custom:
          this.rdbCustom.Checked = true;
          break;
        case InvestorServiceAclInfo.InvestorServicesDefaultSetting.All:
          this.rdbAll.Checked = true;
          break;
      }
      foreach (ListViewItem listViewItem in this.lsvInvestors.Items)
      {
        listViewItem.ImageIndex = 1;
        InvestorServiceUI tag = (InvestorServiceUI) listViewItem.Tag;
        InvestorServiceAclInfo investorServiceAclInfo = tag.InvestorServiceAclInfo;
        if (investorServiceAclInfo != null)
          listViewItem.Checked = investorServiceAclInfo.PersonaAccess == AclResourceAccess.ReadWrite;
        investorServiceAclInfo.CustomAccess = AclResourceAccess.None;
        listViewItem.Tag = (object) tag;
      }
      this.lblDefaultSetting.ImageIndex = 1;
      this.initial = false;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SelectInvestorsPage));
      this.lsvInvestors = new ListView();
      this.columnHeader1 = new ColumnHeader();
      this.btnOk = new Button();
      this.btnCancel = new Button();
      this.imgListTv = new ImageList(this.components);
      this.contMenu = new ContextMenu();
      this.miLinkTo = new MenuItem();
      this.miDisconTo = new MenuItem();
      this.lblDisconnected = new Label();
      this.lblLinked = new Label();
      this.rdbAll = new RadioButton();
      this.rdbCustom = new RadioButton();
      this.rdbNone = new RadioButton();
      this.lblDefaultSetting = new Label();
      this.contMenuDefault = new ContextMenu();
      this.miDefaultLinkTo = new MenuItem();
      this.miDefaultDisconFrom = new MenuItem();
      this.SuspendLayout();
      this.lsvInvestors.CheckBoxes = true;
      this.lsvInvestors.Columns.AddRange(new ColumnHeader[1]
      {
        this.columnHeader1
      });
      this.lsvInvestors.FullRowSelect = true;
      this.lsvInvestors.GridLines = true;
      this.lsvInvestors.HeaderStyle = ColumnHeaderStyle.Nonclickable;
      this.lsvInvestors.Location = new Point(12, 48);
      this.lsvInvestors.MultiSelect = false;
      this.lsvInvestors.Name = "lsvInvestors";
      this.lsvInvestors.Size = new Size(241, 227);
      this.lsvInvestors.Sorting = SortOrder.Ascending;
      this.lsvInvestors.TabIndex = 0;
      this.lsvInvestors.UseCompatibleStateImageBehavior = false;
      this.lsvInvestors.View = View.Details;
      this.lsvInvestors.ItemCheck += new ItemCheckEventHandler(this.lsvInvestors_ItemCheck);
      this.lsvInvestors.ItemChecked += new ItemCheckedEventHandler(this.lsvInvestors_ItemChecked);
      this.lsvInvestors.Click += new EventHandler(this.lsvInvestors_Click);
      this.columnHeader1.Text = "Selected will display in Pipeline";
      this.columnHeader1.Width = 237;
      this.btnOk.Location = new Point(97, 310);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(75, 23);
      this.btnOk.TabIndex = 1;
      this.btnOk.Text = "OK";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.btnCancel.Location = new Point(178, 310);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.imgListTv.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgListTv.ImageStream");
      this.imgListTv.TransparentColor = Color.White;
      this.imgListTv.Images.SetKeyName(0, "");
      this.imgListTv.Images.SetKeyName(1, "");
      this.contMenu.MenuItems.AddRange(new MenuItem[2]
      {
        this.miLinkTo,
        this.miDisconTo
      });
      this.miLinkTo.Index = 0;
      this.miLinkTo.Text = "Link with Persona Rights";
      this.miLinkTo.Click += new EventHandler(this.miLinkTo_Click);
      this.miDisconTo.Index = 1;
      this.miDisconTo.Text = "Disconnect from Persona Rights";
      this.miDisconTo.Click += new EventHandler(this.miDisconTo_Click);
      this.lblDisconnected.ImageAlign = ContentAlignment.MiddleLeft;
      this.lblDisconnected.ImageIndex = 0;
      this.lblDisconnected.ImageList = this.imgListTv;
      this.lblDisconnected.Location = new Point(9, 291);
      this.lblDisconnected.Name = "lblDisconnected";
      this.lblDisconnected.Size = new Size(197, 16);
      this.lblDisconnected.TabIndex = 8;
      this.lblDisconnected.Text = "      Disconnected from Persona Rights";
      this.lblDisconnected.TextAlign = ContentAlignment.MiddleLeft;
      this.lblDisconnected.Visible = false;
      this.lblLinked.ImageAlign = ContentAlignment.MiddleLeft;
      this.lblLinked.ImageIndex = 1;
      this.lblLinked.ImageList = this.imgListTv;
      this.lblLinked.Location = new Point(9, 275);
      this.lblLinked.Name = "lblLinked";
      this.lblLinked.Size = new Size(197, 16);
      this.lblLinked.TabIndex = 7;
      this.lblLinked.Text = "      Linked with Persona Rights";
      this.lblLinked.TextAlign = ContentAlignment.MiddleLeft;
      this.lblLinked.Visible = false;
      this.rdbAll.AutoSize = true;
      this.rdbAll.Location = new Point(11, 25);
      this.rdbAll.Name = "rdbAll";
      this.rdbAll.Size = new Size(36, 17);
      this.rdbAll.TabIndex = 9;
      this.rdbAll.TabStop = true;
      this.rdbAll.Text = "All";
      this.rdbAll.UseVisualStyleBackColor = true;
      this.rdbAll.MouseEnter += new EventHandler(this.rdb_MouseEnter);
      this.rdbCustom.AutoSize = true;
      this.rdbCustom.Location = new Point(54, 25);
      this.rdbCustom.Name = "rdbCustom";
      this.rdbCustom.Size = new Size(60, 17);
      this.rdbCustom.TabIndex = 10;
      this.rdbCustom.TabStop = true;
      this.rdbCustom.Text = "Custom";
      this.rdbCustom.UseVisualStyleBackColor = true;
      this.rdbCustom.CheckedChanged += new EventHandler(this.rdb_CheckedChanged);
      this.rdbCustom.MouseEnter += new EventHandler(this.rdb_MouseEnter);
      this.rdbNone.AutoSize = true;
      this.rdbNone.Location = new Point(120, 25);
      this.rdbNone.Name = "rdbNone";
      this.rdbNone.Size = new Size(51, 17);
      this.rdbNone.TabIndex = 11;
      this.rdbNone.TabStop = true;
      this.rdbNone.Text = "None";
      this.rdbNone.UseVisualStyleBackColor = true;
      this.rdbNone.CheckedChanged += new EventHandler(this.rdb_CheckedChanged);
      this.rdbNone.MouseEnter += new EventHandler(this.rdb_MouseEnter);
      this.lblDefaultSetting.AutoSize = true;
      this.lblDefaultSetting.ImageAlign = ContentAlignment.MiddleRight;
      this.lblDefaultSetting.ImageList = this.imgListTv;
      this.lblDefaultSetting.Location = new Point(9, 9);
      this.lblDefaultSetting.Name = "lblDefaultSetting";
      this.lblDefaultSetting.Size = new Size(101, 13);
      this.lblDefaultSetting.TabIndex = 12;
      this.lblDefaultSetting.Text = "Default Setting        ";
      this.contMenuDefault.MenuItems.AddRange(new MenuItem[2]
      {
        this.miDefaultLinkTo,
        this.miDefaultDisconFrom
      });
      this.miDefaultLinkTo.Index = 0;
      this.miDefaultLinkTo.Text = "Link with Persona Rights";
      this.miDefaultLinkTo.Click += new EventHandler(this.miDefaultLinkTo_Click);
      this.miDefaultDisconFrom.Index = 1;
      this.miDefaultDisconFrom.Text = "Disconnect from Persona Rights";
      this.miDefaultDisconFrom.Click += new EventHandler(this.miDefaultDisconFrom_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(266, 340);
      this.ControlBox = false;
      this.Controls.Add((Control) this.lblDefaultSetting);
      this.Controls.Add((Control) this.rdbNone);
      this.Controls.Add((Control) this.rdbCustom);
      this.Controls.Add((Control) this.rdbAll);
      this.Controls.Add((Control) this.lblDisconnected);
      this.Controls.Add((Control) this.lblLinked);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOk);
      this.Controls.Add((Control) this.lsvInvestors);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SelectInvestorsPage);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Select Investors";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
