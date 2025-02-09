// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.FileContactForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using mshtml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class FileContactForm : CustomUserControl, IRefreshContents
  {
    private const string className = "FileContactForm";
    private static readonly string sw = Tracing.SwInputEngine;
    public EventHandler FileContactSelectedIndexChanged;
    private Panel panel1;
    private ToolTip fieldToolTip;
    private IContainer components;
    private LoanData loan;
    private IMainScreen mainScreen;
    private FileContactRecord contactData;
    private LoanScreen freeScreen;
    private Panel panelDetail;
    private IWin32Window parentWindows;
    private Hashtable roleTables = new Hashtable();
    private CollapsibleSplitter collapsibleSplitter1;
    private GridView gridViewContacts;
    private BorderPanel borderPanelTop;
    private Sessions.Session session;
    private LoanAssociateDetail loanAssociateDetail;
    private const string companyIDs = ",1264,617,610,411,56,VEND.X122,VEND.X133,VEND.X144,VEND.X424,713,L252,L248,VEND.X34,1500,624,REGZGFE.X8,VEND.X178,395,VEND.X200,VEND.X44,VEND.X263,VEND.X278,VEND.X293,VEND.X310,VEND.X655,VEND.X54,VEND.X64,VEND.X74,VEND.X1,";
    private const string nameIDs = ",1256,618,611,416,VEND.X117,VEND.X128,VEND.X139,VEND.X150,638,VEND.X412,Seller3.Name,Seller4.Name,VEND.X429,714,VEND.X162,707,VEND.X35,VEND.X13,625,984,VEND.X184,VEND.X195,VEND.X206,VEND.X45,VEND.X271,VEND.X286,VEND.X302,VEND.X317,VEND.X668,VEND.X55,VEND.X65,VEND.X75,VEND.X2,";
    private const string phoneIDs = ",FE0117,FE0217,1262,622,615,417,VEND.X118,VEND.X129,VEND.X140,VEND.X151,VEND.X220,VEND.X421,Seller3.BusPh,Seller4.BusPh,VEND.X430,718,VEND.X163,711,VEND.X41,VEND.X19,629,1410,VEND.X185,VEND.X196,VEND.X207,VEND.X51,VEND.X272,VEND.X287,VEND.X303,VEND.X318,VEND.X669,VEND.X61,VEND.X71,VEND.X81,VEND.X8,";
    private const string emailIDs = ",1240,1268,95,89,87,88,VEND.X119,VEND.X130,VEND.X141,VEND.X152,92,VEND.X419,Seller3.Email,Seller4.Email,VEND.X432,94,VEND.X164,93,VEND.X43,VEND.X21,90,1411,VEND.X186,VEND.X197,VEND.X208,VEND.X53,VEND.X273,VEND.X288,VEND.X305,VEND.X319,VEND.X670,VEND.X63,VEND.X73,VEND.X83,VEND.X10,";
    private const string categoryIDs = ",VEND.X84,VEND.X85,VEND.X86,VEND.X11,";
    private const string borNameIDs = ",4000,4001,4002,4003,4004,4005,4006,4007,";

    public FileContactForm(Sessions.Session session, LoanData loan, IWin32Window parentWindows)
    {
      this.session = session;
      this.loan = loan;
      this.parentWindows = parentWindows;
      this.Dock = DockStyle.Fill;
      if (!this.loan.IsTemplate && !this.loan.IsInFindFieldForm)
        this.loadRoleSettings();
      this.InitializeComponent();
      this.initForm();
    }

    public FileContactForm(Sessions.Session session, LoanData loan, IMainScreen mainScreen)
    {
      this.session = session;
      this.loan = loan;
      this.mainScreen = mainScreen;
      this.Dock = DockStyle.Fill;
      this.loadRoleSettings();
      this.InitializeComponent();
      this.initForm();
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
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      this.panel1 = new Panel();
      this.borderPanelTop = new BorderPanel();
      this.gridViewContacts = new GridView();
      this.collapsibleSplitter1 = new CollapsibleSplitter();
      this.panelDetail = new Panel();
      this.fieldToolTip = new ToolTip(this.components);
      this.panel1.SuspendLayout();
      this.borderPanelTop.SuspendLayout();
      this.SuspendLayout();
      this.panel1.BackColor = SystemColors.ControlLight;
      this.panel1.Controls.Add((Control) this.borderPanelTop);
      this.panel1.Controls.Add((Control) this.collapsibleSplitter1);
      this.panel1.Controls.Add((Control) this.panelDetail);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(654, 545);
      this.panel1.TabIndex = 29;
      this.borderPanelTop.Borders = AnchorStyles.Bottom;
      this.borderPanelTop.Controls.Add((Control) this.gridViewContacts);
      this.borderPanelTop.Dock = DockStyle.Fill;
      this.borderPanelTop.Location = new Point(0, 0);
      this.borderPanelTop.Name = "borderPanelTop";
      this.borderPanelTop.Size = new Size(654, 222);
      this.borderPanelTop.TabIndex = 0;
      this.gridViewContacts.AllowMultiselect = false;
      this.gridViewContacts.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SortMethod = GVSortMethod.Numeric;
      gvColumn1.Text = "";
      gvColumn1.Width = 30;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Category/Role";
      gvColumn2.Width = 150;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column7";
      gvColumn3.Text = "Disc. on CD";
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column3";
      gvColumn4.Text = "Company";
      gvColumn4.Width = 180;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column4";
      gvColumn5.Text = "Name";
      gvColumn5.Width = 160;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column5";
      gvColumn6.Text = "Work Phone";
      gvColumn6.Width = 100;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column6";
      gvColumn7.Text = "Email";
      gvColumn7.Width = 160;
      this.gridViewContacts.Columns.AddRange(new GVColumn[7]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7
      });
      this.gridViewContacts.Dock = DockStyle.Fill;
      this.gridViewContacts.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridViewContacts.Location = new Point(0, 0);
      this.gridViewContacts.Name = "gridViewContacts";
      this.gridViewContacts.Size = new Size(654, 221);
      this.gridViewContacts.TabIndex = 10;
      this.gridViewContacts.SelectedIndexChanged += new EventHandler(this.gridViewContacts_SelectedIndexChanged);
      this.gridViewContacts.MouseUp += new MouseEventHandler(this.gridViewContacts_MouseUp);
      this.collapsibleSplitter1.AnimationDelay = 20;
      this.collapsibleSplitter1.AnimationStep = 20;
      this.collapsibleSplitter1.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter1.ControlToHide = (Control) this.panelDetail;
      this.collapsibleSplitter1.Dock = DockStyle.Bottom;
      this.collapsibleSplitter1.ExpandParentForm = false;
      this.collapsibleSplitter1.Location = new Point(0, 222);
      this.collapsibleSplitter1.Name = "collapsibleSplitter1";
      this.collapsibleSplitter1.TabIndex = 13;
      this.collapsibleSplitter1.TabStop = false;
      this.collapsibleSplitter1.UseAnimations = false;
      this.collapsibleSplitter1.VisualStyle = VisualStyles.Encompass;
      this.panelDetail.Dock = DockStyle.Bottom;
      this.panelDetail.Location = new Point(0, 229);
      this.panelDetail.Name = "panelDetail";
      this.panelDetail.Size = new Size(654, 316);
      this.panelDetail.TabIndex = 10;
      this.Controls.Add((Control) this.panel1);
      this.Name = nameof (FileContactForm);
      this.Size = new Size(654, 545);
      this.panel1.ResumeLayout(false);
      this.borderPanelTop.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void loadRoleSettings()
    {
      this.roleTables.Clear();
      RoleInfo[] allRoles = this.session.LoanDataMgr.SystemConfiguration.AllRoles;
      for (int index = 0; index < allRoles.Length; ++index)
      {
        if (!this.roleTables.ContainsKey((object) allRoles[index].RoleID))
          this.roleTables.Add((object) allRoles[index].RoleID, (object) allRoles[index]);
      }
    }

    private void initForm()
    {
      this.contactData = new FileContactRecord(this.session, this.loan);
      this.gridViewContacts.BorderStyle = BorderStyle.None;
      this.gridViewContacts.Items.Clear();
      FileContactRecord.ContactFields contactFields = (FileContactRecord.ContactFields) null;
      int order = 0;
      for (int index = 1; index <= 37; ++index)
      {
        if (!this.loan.IsTemplate || index >= 4)
        {
          switch (index)
          {
            case 1:
              contactFields = this.contactData.GetBorrower(false);
              break;
            case 2:
              contactFields = this.contactData.GetBorrower(true);
              break;
            case 3:
              this.addRoles(ref order);
              continue;
            case 4:
              contactFields = this.contactData.GetVendors(FileContactRecord.ContactTypes.Lender);
              break;
            case 5:
              contactFields = this.contactData.GetVendors(FileContactRecord.ContactTypes.Appraiser);
              break;
            case 6:
              contactFields = this.contactData.GetVendors(FileContactRecord.ContactTypes.Escrow);
              break;
            case 7:
              contactFields = this.contactData.GetVendors(FileContactRecord.ContactTypes.Title);
              break;
            case 8:
              contactFields = this.contactData.GetVendors(FileContactRecord.ContactTypes.BuyerAttorney);
              break;
            case 9:
              contactFields = this.contactData.GetVendors(FileContactRecord.ContactTypes.SellerAttorney);
              break;
            case 10:
              contactFields = this.contactData.GetVendors(FileContactRecord.ContactTypes.BuyerAgent);
              break;
            case 11:
              contactFields = this.contactData.GetVendors(FileContactRecord.ContactTypes.SellerAgent);
              break;
            case 12:
              contactFields = this.contactData.GetVendors(FileContactRecord.ContactTypes.Seller);
              break;
            case 13:
              contactFields = this.contactData.GetVendors(FileContactRecord.ContactTypes.Seller2);
              break;
            case 14:
              contactFields = this.contactData.GetVendors(FileContactRecord.ContactTypes.Seller3);
              break;
            case 15:
              contactFields = this.contactData.GetVendors(FileContactRecord.ContactTypes.Seller4);
              break;
            case 16:
              contactFields = this.contactData.GetVendors(FileContactRecord.ContactTypes.Notary);
              break;
            case 17:
              contactFields = this.contactData.GetVendors(FileContactRecord.ContactTypes.Builder);
              break;
            case 18:
              contactFields = this.contactData.GetVendors(FileContactRecord.ContactTypes.HazardInsurance);
              break;
            case 19:
              contactFields = this.contactData.GetVendors(FileContactRecord.ContactTypes.MortgageInsurance);
              break;
            case 20:
              contactFields = this.contactData.GetVendors(FileContactRecord.ContactTypes.Surveyor);
              break;
            case 21:
              contactFields = this.contactData.GetVendors(FileContactRecord.ContactTypes.FloodInsurance);
              break;
            case 22:
              contactFields = this.contactData.GetVendors(FileContactRecord.ContactTypes.CreditCompany);
              break;
            case 23:
              contactFields = this.contactData.GetVendors(FileContactRecord.ContactTypes.Underwriter);
              break;
            case 24:
              contactFields = this.contactData.GetVendors(FileContactRecord.ContactTypes.Servicing);
              break;
            case 25:
              contactFields = this.contactData.GetVendors(FileContactRecord.ContactTypes.DocSigning);
              break;
            case 26:
              contactFields = this.contactData.GetVendors(FileContactRecord.ContactTypes.Warehouse);
              break;
            case 27:
              contactFields = this.contactData.GetVendors(FileContactRecord.ContactTypes.FinancialPlanner);
              break;
            case 28:
              contactFields = this.contactData.GetVendors(FileContactRecord.ContactTypes.Investor);
              break;
            case 29:
              contactFields = this.contactData.GetVendors(FileContactRecord.ContactTypes.AssignTo);
              break;
            case 30:
              contactFields = this.contactData.GetVendors(FileContactRecord.ContactTypes.Broker);
              break;
            case 31:
              contactFields = this.contactData.GetVendors(FileContactRecord.ContactTypes.DocsPrepared);
              break;
            case 32:
              contactFields = this.contactData.GetVendors(FileContactRecord.ContactTypes.SettlementAgent);
              break;
            case 33:
              contactFields = this.contactData.GetVendors(FileContactRecord.ContactTypes.SellerCorporationOfficer);
              break;
            case 34:
              contactFields = this.contactData.GetVendors(FileContactRecord.ContactTypes.Custom1);
              break;
            case 35:
              contactFields = this.contactData.GetVendors(FileContactRecord.ContactTypes.Custom2);
              break;
            case 36:
              contactFields = this.contactData.GetVendors(FileContactRecord.ContactTypes.Custom3);
              break;
            case 37:
              contactFields = this.contactData.GetVendors(FileContactRecord.ContactTypes.Custom4);
              break;
          }
          ++order;
          if (contactFields != null)
            this.addContactToListForm(order, contactFields);
        }
      }
      int borrowingOwnerContact = this.loan.GetNumberOfNonBorrowingOwnerContact();
      if (borrowingOwnerContact > 0)
      {
        for (int i = 1; i <= borrowingOwnerContact; ++i)
        {
          FileContactRecord.ContactFields nonBorrowingOwner = this.contactData.GetNonBorrowingOwner(i);
          this.addContactToListForm(++order, nonBorrowingOwner);
        }
      }
      this.freeScreen = new LoanScreen(this.session, this.parentWindows, (IHtmlInput) this.loan);
      this.freeScreen.FieldOnKeyUp += new EventHandler(this.freeScreen_FieldOnKeyUp);
      if (this.gridViewContacts.Items.Count <= 0)
        return;
      this.gridViewContacts.Items[0].Selected = true;
    }

    private void addContactToListForm(int order, FileContactRecord.ContactFields contactFields)
    {
      this.gridViewContacts.Items.Add(new GVItem(order.ToString())
      {
        SubItems = {
          (object) contactFields.Category,
          (object) contactFields.DiscToCD,
          (object) contactFields.Company,
          (object) contactFields.FullName,
          (object) contactFields.BizPhone,
          (object) contactFields.Email
        },
        Tag = (object) contactFields
      });
    }

    private void addRoles(ref int order)
    {
      LogList logList = this.loan.GetLogList();
      if (logList == null)
        return;
      Hashtable hashtable = new Hashtable();
      MilestoneLog[] allMilestones = logList.GetAllMilestones();
      foreach (MilestoneLog milestoneLog in allMilestones)
      {
        if (milestoneLog.RoleID != -1 || !((milestoneLog.RoleName ?? "") == ""))
        {
          ++order;
          hashtable[(object) milestoneLog.RoleID] = (object) milestoneLog.RoleName;
          GVItem gvItem = new GVItem(order.ToString());
          if (string.Compare(milestoneLog.Stage, "Started", true) == 0)
            gvItem.SubItems.Add((object) "Role - File Starter");
          else if (this.roleTables.ContainsKey((object) milestoneLog.RoleID))
            gvItem.SubItems.Add((object) ("Role - " + ((RoleSummaryInfo) this.roleTables[(object) milestoneLog.RoleID]).RoleName + " (" + milestoneLog.Stage + ")"));
          else
            gvItem.SubItems.Add((object) ("Role - " + milestoneLog.RoleName + " (" + milestoneLog.Stage + ")"));
          gvItem.SubItems.Add((object) "");
          gvItem.SubItems.Add((object) "");
          gvItem.SubItems.Add((object) milestoneLog.LoanAssociateName);
          gvItem.SubItems.Add((object) milestoneLog.LoanAssociatePhone);
          gvItem.SubItems.Add((object) milestoneLog.LoanAssociateEmail);
          gvItem.Tag = (object) milestoneLog;
          this.gridViewContacts.Items.Add(gvItem);
        }
      }
      foreach (MilestoneFreeRoleLog milestoneFreeRole in this.loan.GetLogList().GetAllMilestoneFreeRoles())
      {
        ++order;
        GVItem gvItem = new GVItem(order.ToString());
        if (string.IsNullOrWhiteSpace(milestoneFreeRole.RoleName) && this.roleTables.ContainsKey((object) milestoneFreeRole.RoleID))
        {
          this.loan.Dirty = true;
          milestoneFreeRole.RoleName = ((RoleSummaryInfo) this.roleTables[(object) milestoneFreeRole.RoleID]).RoleName;
        }
        gvItem.SubItems.Add((object) ("Role - " + milestoneFreeRole.RoleName));
        gvItem.SubItems.Add((object) "");
        gvItem.SubItems.Add((object) "");
        LoanAssociateLog loanAssociateLog = (LoanAssociateLog) milestoneFreeRole;
        gvItem.SubItems.Add((object) loanAssociateLog.LoanAssociateName);
        gvItem.SubItems.Add((object) loanAssociateLog.LoanAssociatePhone);
        gvItem.SubItems.Add((object) loanAssociateLog.LoanAssociateEmail);
        gvItem.Tag = (object) loanAssociateLog;
        this.gridViewContacts.Items.Add(gvItem);
      }
    }

    private void gridViewContacts_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.gridViewContacts.SelectedItems.Count == 0)
        return;
      object tag = this.gridViewContacts.SelectedItems[0].Tag;
      switch (tag)
      {
        case FileContactRecord.ContactFields _:
          this.freeScreen.ContactFieldsChanged -= new EventHandler(this.freeScreen_ContactFieldsChanged);
          FileContactRecord.ContactFields contactFields = (FileContactRecord.ContactFields) tag;
          string str = "FC_" + (contactFields.ContactType == FileContactRecord.ContactTypes.NonBorrowingOwnerContact ? "NonBorrowingOwnerContact" : contactFields.ContactType.ToString());
          if (this.loanAssociateDetail != null)
            this.loanAssociateDetail.Dispose();
          this.panelDetail.Controls.Clear();
          this.freeScreen.RemoveTitle();
          this.freeScreen.RemoveBorder();
          this.freeScreen.LoadForm(new InputFormInfo(str, str));
          this.freeScreen.BrwHandler.Property = (object) contactFields.NBOCIndex;
          this.freeScreen.BringToFront();
          this.freeScreen.Focus();
          this.freeScreen.ContactFieldsChanged += new EventHandler(this.freeScreen_ContactFieldsChanged);
          FileContactVendorForm contactVendorForm = new FileContactVendorForm(this.freeScreen, contactFields, this.loan);
          if (contactFields.ContactType != FileContactRecord.ContactTypes.NonBorrowingOwnerContact)
          {
            contactVendorForm.CopyProviderButtonClick += new EventHandler(this.vendorDetailForm_CopyProviderButtonClick);
            contactVendorForm.CopySettlementAgentButtonClick += new EventHandler(this.vendorDetailForm_CopySettlementAgentButtonClick);
          }
          this.panelDetail.Controls.Add((Control) contactVendorForm);
          break;
        case MilestoneLog _:
          MilestoneLog ml = (MilestoneLog) tag;
          MilestoneLog[] allMilestones = this.loan.GetLogList().GetAllMilestones();
          MilestoneLog precedingMl = (MilestoneLog) null;
          foreach (MilestoneLog milestoneLog in allMilestones)
          {
            if (string.Compare(milestoneLog.MilestoneID, ml.MilestoneID, true) != 0)
              precedingMl = milestoneLog;
            else
              break;
          }
          this.loanAssociateDetail = new LoanAssociateDetail(ml, precedingMl, this.gridViewContacts.SelectedItems[0].SubItems[1].Text, this.loan, false);
          this.loanAssociateDetail.FieldOnKeyUp += new EventHandler(this.loanAssociateDetail_FieldOnKeyUp);
          this.loanAssociateDetail.FieldsRefresh += new EventHandler(this.loanAssociateDetail_FieldsRefresh);
          this.panelDetail.Controls.Clear();
          this.panelDetail.Controls.Add((Control) this.loanAssociateDetail);
          this.loanAssociateDetail.BringToFront();
          this.loanAssociateDetail.Focus();
          break;
        case MilestoneFreeRoleLog _:
          MilestoneFreeRoleLog msfl = (MilestoneFreeRoleLog) tag;
          if (this.loanAssociateDetail != null)
            this.loanAssociateDetail.Dispose();
          this.panelDetail.Controls.Clear();
          this.loanAssociateDetail = new LoanAssociateDetail(msfl, this.gridViewContacts.SelectedItems[0].SubItems[1].Text, this.loan, false);
          this.loanAssociateDetail.FieldOnKeyUp += new EventHandler(this.loanAssociateDetail_FieldOnKeyUp);
          this.loanAssociateDetail.FieldsRefresh += new EventHandler(this.loanAssociateDetail_FieldsRefresh);
          this.panelDetail.Controls.Remove((Control) this.freeScreen);
          this.panelDetail.Controls.Add((Control) this.loanAssociateDetail);
          this.loanAssociateDetail.BringToFront();
          this.loanAssociateDetail.Focus();
          break;
      }
      if (this.FileContactSelectedIndexChanged == null)
        return;
      this.FileContactSelectedIndexChanged(tag, new EventArgs());
    }

    private Dictionary<string, string> getSettlementAgentMapping(
      FileContactRecord.ContactTypes contactType)
    {
      Dictionary<string, string> settlementAgentMapping = new Dictionary<string, string>();
      switch (contactType)
      {
        case FileContactRecord.ContactTypes.Escrow:
          settlementAgentMapping.Add("610", "VEND.X655");
          settlementAgentMapping.Add("612", "VEND.X656");
          settlementAgentMapping.Add("613", "VEND.X657");
          settlementAgentMapping.Add("1175", "VEND.X658");
          settlementAgentMapping.Add("614", "VEND.X659");
          settlementAgentMapping.Add("VEND.X216", "VEND.X660");
          settlementAgentMapping.Add("VEND.X218", "VEND.X661");
          settlementAgentMapping.Add("186", "VEND.X673");
          settlementAgentMapping.Add("611", "VEND.X668");
          settlementAgentMapping.Add("615", "VEND.X669");
          settlementAgentMapping.Add("87", "VEND.X670");
          settlementAgentMapping.Add("1011", "VEND.X671");
          settlementAgentMapping.Add("VEND.X518", "VEND.X672");
          settlementAgentMapping.Add("616", "VEND.X674");
          settlementAgentMapping.Add("VEND.X986", "VEND.X663");
          settlementAgentMapping.Add("VEND.X699", "VEND.X664");
          settlementAgentMapping.Add("VEND.X700", "VEND.X665");
          settlementAgentMapping.Add("VEND.X701", "VEND.X666");
          settlementAgentMapping.Add("VEND.X702", "VEND.X667");
          settlementAgentMapping.Add("VEND.X703", "VEND.X676");
          settlementAgentMapping.Add("VEND.X704", "VEND.X677");
          settlementAgentMapping.Add("VEND.X705", "VEND.X678");
          settlementAgentMapping.Add("VEND.X706", "VEND.X679");
          settlementAgentMapping.Add("VEND.X707", "VEND.X680");
          break;
        case FileContactRecord.ContactTypes.Title:
          settlementAgentMapping.Add("411", "VEND.X655");
          settlementAgentMapping.Add("412", "VEND.X656");
          settlementAgentMapping.Add("413", "VEND.X657");
          settlementAgentMapping.Add("1174", "VEND.X658");
          settlementAgentMapping.Add("414", "VEND.X659");
          settlementAgentMapping.Add("VEND.X155", "VEND.X660");
          settlementAgentMapping.Add("VEND.X156", "VEND.X661");
          settlementAgentMapping.Add("187", "VEND.X673");
          settlementAgentMapping.Add("416", "VEND.X668");
          settlementAgentMapping.Add("417", "VEND.X669");
          settlementAgentMapping.Add("88", "VEND.X670");
          settlementAgentMapping.Add("1243", "VEND.X671");
          settlementAgentMapping.Add("VEND.X519", "VEND.X672");
          settlementAgentMapping.Add("419", "VEND.X674");
          settlementAgentMapping.Add("VEND.X979", "VEND.X663");
          settlementAgentMapping.Add("VEND.X708", "VEND.X664");
          settlementAgentMapping.Add("VEND.X709", "VEND.X665");
          settlementAgentMapping.Add("VEND.X710", "VEND.X666");
          settlementAgentMapping.Add("VEND.X711", "VEND.X667");
          settlementAgentMapping.Add("VEND.X712", "VEND.X676");
          settlementAgentMapping.Add("VEND.X713", "VEND.X677");
          settlementAgentMapping.Add("VEND.X714", "VEND.X678");
          settlementAgentMapping.Add("VEND.X715", "VEND.X679");
          settlementAgentMapping.Add("VEND.X716", "VEND.X680");
          break;
        case FileContactRecord.ContactTypes.BuyerAttorney:
          settlementAgentMapping.Add("56", "VEND.X655");
          settlementAgentMapping.Add("VEND.X112", "VEND.X656");
          settlementAgentMapping.Add("VEND.X113", "VEND.X657");
          settlementAgentMapping.Add("VEND.X114", "VEND.X658");
          settlementAgentMapping.Add("VEND.X115", "VEND.X659");
          settlementAgentMapping.Add("VEND.X233", "VEND.X660");
          settlementAgentMapping.Add("VEND.X234", "VEND.X661");
          settlementAgentMapping.Add("VEND.X121", "VEND.X673");
          settlementAgentMapping.Add("VEND.X117", "VEND.X668");
          settlementAgentMapping.Add("VEND.X118", "VEND.X669");
          settlementAgentMapping.Add("VEND.X119", "VEND.X670");
          settlementAgentMapping.Add("VEND.X120", "VEND.X671");
          settlementAgentMapping.Add("VEND.X498", "VEND.X672");
          settlementAgentMapping.Add("VEND.X116", "VEND.X674");
          settlementAgentMapping.Add("VEND.X982", "VEND.X663");
          settlementAgentMapping.Add("VEND.X717", "VEND.X664");
          settlementAgentMapping.Add("VEND.X718", "VEND.X665");
          settlementAgentMapping.Add("VEND.X719", "VEND.X666");
          settlementAgentMapping.Add("VEND.X720", "VEND.X667");
          settlementAgentMapping.Add("VEND.X721", "VEND.X676");
          settlementAgentMapping.Add("VEND.X722", "VEND.X677");
          settlementAgentMapping.Add("VEND.X723", "VEND.X678");
          settlementAgentMapping.Add("VEND.X724", "VEND.X679");
          settlementAgentMapping.Add("VEND.X725", "VEND.X680");
          break;
      }
      return settlementAgentMapping;
    }

    private void vendorDetailForm_CopySettlementAgentButtonClick(object sender, EventArgs e)
    {
      FileContactRecord.ContactTypes contactType = (FileContactRecord.ContactTypes) sender;
      Dictionary<string, string> settlementAgentMapping = this.getSettlementAgentMapping(contactType);
      string str = "VEND.X";
      for (int index = 655; index <= 680; ++index)
        this.loan.SetCurrentField(str + index.ToString(), string.Empty);
      foreach (KeyValuePair<string, string> keyValuePair in settlementAgentMapping)
        this.loan.SetCurrentField(keyValuePair.Value, this.loan.GetSimpleField(keyValuePair.Key));
      if ((contactType == FileContactRecord.ContactTypes.Escrow || contactType == FileContactRecord.ContactTypes.Title || contactType == FileContactRecord.ContactTypes.BuyerAttorney) && this.loan.GetField("VEND.X654") == "Y")
        this.loan.Calculator.FormCalculation("VEND.X654", "VEND.X654", this.loan.GetField("VEND.X654"));
      for (int nItemIndex = 0; nItemIndex < this.gridViewContacts.Items.Count; ++nItemIndex)
      {
        if (this.gridViewContacts.Items[nItemIndex].Tag is FileContactRecord.ContactFields && ((FileContactRecord.ContactFields) this.gridViewContacts.Items[nItemIndex].Tag).ContactType == FileContactRecord.ContactTypes.SettlementAgent)
        {
          this.contactData.GetVendors(FileContactRecord.ContactTypes.SettlementAgent);
          this.gridViewContacts.Items[nItemIndex].SubItems[3].Text = this.loan.GetField("VEND.X655");
          this.gridViewContacts.Items[nItemIndex].SubItems[4].Text = this.loan.GetField("VEND.X668");
          this.gridViewContacts.Items[nItemIndex].SubItems[5].Text = this.loan.GetField("VEND.X669");
          this.gridViewContacts.Items[nItemIndex].SubItems[6].Text = this.loan.GetField("VEND.X670");
          break;
        }
      }
      int num = (int) Utils.Dialog((IWin32Window) this, "The selected contact was copied to Settlement Agent successfully.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    private void vendorDetailForm_CopyProviderButtonClick(object sender, EventArgs e)
    {
      ContactFieldRef contactFields = ContactGroup.GetContactFields((FileContactRecord.ContactTypes) sender);
      if (contactFields == null)
        return;
      string empty = string.Empty;
      int serviceProviders = this.loan.GetNumberOfSettlementServiceProviders();
      for (int index = 1; index <= serviceProviders; ++index)
      {
        string str = "SP" + (index > 99 ? index.ToString("000") : index.ToString("00"));
        if (string.Compare(this.loan.GetSimpleField(str + "01"), contactFields.CategoryName.StartsWith("VEND.X") ? this.loan.GetSimpleField(contactFields.CategoryName) : contactFields.CategoryName, true) == 0 && string.Compare(this.loan.GetSimpleField(str + "02"), this.loan.GetSimpleField(contactFields.CompanyField), true) == 0 && string.Compare(this.loan.GetSimpleField(str + "03"), this.loan.GetSimpleField(contactFields.AddressField), true) == 0 && string.Compare(this.loan.GetSimpleField(str + "04"), this.loan.GetSimpleField(contactFields.CityField), true) == 0 && string.Compare(this.loan.GetSimpleField(str + "05"), this.loan.GetSimpleField(contactFields.StateField), true) == 0 && string.Compare(this.loan.GetSimpleField(str + "06"), this.loan.GetSimpleField(contactFields.ZipField), true) == 0 && string.Compare(this.loan.GetSimpleField(str + "07"), this.loan.GetSimpleField(contactFields.PhoneField), true) == 0 && string.Compare(this.loan.GetSimpleField(str + "08"), this.loan.GetSimpleField(contactFields.RelationshipField), true) == 0 && string.Compare(this.loan.GetSimpleField(str + "09"), this.loan.GetSimpleField(contactFields.LineItemField), true) == 0 && string.Compare(this.loan.GetSimpleField(str + "14"), this.loan.GetSimpleField(contactFields.ContactNameField), true) == 0 && string.Compare(this.loan.GetSimpleField(str + "15"), this.loan.GetSimpleField(contactFields.EmailField), true) == 0 && string.Compare(this.loan.GetSimpleField(str + "16"), this.loan.GetSimpleField(contactFields.FaxField), true) == 0)
        {
          if (Utils.Dialog((IWin32Window) this, "The Settlement Service Provider list already contains this contact. Do you want to copy it again?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
            return;
          break;
        }
      }
      int num1 = this.loan.NewSettlementServiceProvider();
      string str1 = "SP" + (num1 > 99 ? num1.ToString("000") : num1.ToString("00"));
      this.loan.SetCurrentField(str1 + "01", contactFields.CategoryName.StartsWith("VEND.X") ? this.loan.GetSimpleField(contactFields.CategoryName) : contactFields.CategoryName);
      this.loan.SetCurrentField(str1 + "02", this.loan.GetSimpleField(contactFields.CompanyField));
      this.loan.SetCurrentField(str1 + "03", this.loan.GetSimpleField(contactFields.AddressField));
      this.loan.SetCurrentField(str1 + "04", this.loan.GetSimpleField(contactFields.CityField));
      this.loan.SetCurrentField(str1 + "05", this.loan.GetSimpleField(contactFields.StateField));
      this.loan.SetCurrentField(str1 + "06", this.loan.GetSimpleField(contactFields.ZipField));
      this.loan.SetCurrentField(str1 + "07", this.loan.GetSimpleField(contactFields.PhoneField));
      this.loan.SetCurrentField(str1 + "08", this.loan.GetSimpleField(contactFields.RelationshipField));
      this.loan.SetCurrentField(str1 + "09", this.loan.GetSimpleField(contactFields.LineItemField));
      this.loan.SetCurrentField(str1 + "14", this.loan.GetSimpleField(contactFields.ContactNameField));
      this.loan.SetCurrentField(str1 + "15", this.loan.GetSimpleField(contactFields.EmailField));
      this.loan.SetCurrentField(str1 + "16", this.loan.GetSimpleField(contactFields.FaxField));
      int num2 = (int) Utils.Dialog((IWin32Window) this, "The selected contact was copied to the Settlement Service Provider List successfully.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    private void loanAssociateDetail_FieldOnKeyUp(object sender, EventArgs e)
    {
      if (sender == null)
        return;
      TextBox textBox = (TextBox) sender;
      if (textBox == null)
        return;
      switch ((string) textBox.Tag)
      {
        case "phone":
          this.gridViewContacts.SelectedItems[0].SubItems[5].Text = textBox.Text.Trim();
          break;
        case "email":
          if (textBox.Text.Trim() != "" && Utils.ValidateEmail(textBox.Text.Trim()))
          {
            this.gridViewContacts.SelectedItems[0].SubItems[6].Text = textBox.Text.Trim();
            break;
          }
          this.gridViewContacts.SelectedItems[0].SubItems[6].Text = "";
          break;
      }
    }

    private void loanAssociateDetail_FieldsRefresh(object sender, EventArgs e)
    {
      if (this.gridViewContacts.SelectedItems.Count == 0)
        return;
      MilestoneFreeRoleLog milestoneFreeRoleLog = (MilestoneFreeRoleLog) sender;
      if (milestoneFreeRoleLog == null)
        return;
      this.gridViewContacts.SelectedItems[0].SubItems[4].Text = milestoneFreeRoleLog.LoanAssociateName;
      this.gridViewContacts.SelectedItems[0].SubItems[5].Text = milestoneFreeRoleLog.LoanAssociatePhone;
      this.gridViewContacts.SelectedItems[0].SubItems[6].Text = milestoneFreeRoleLog.LoanAssociateEmail;
    }

    private void gridViewContacts_MouseUp(object sender, MouseEventArgs e)
    {
      this.panelDetail.Focus();
    }

    private void freeScreen_ContactFieldsChanged(object sender, EventArgs e)
    {
      if (this.gridViewContacts.SelectedItems.Count == 0)
        return;
      FileContactRecord.ContactFields tag = (FileContactRecord.ContactFields) this.gridViewContacts.SelectedItems[0].Tag;
      FileContactRecord.ContactFields contactFields = tag.ContactType != FileContactRecord.ContactTypes.NonBorrowingOwnerContact ? this.contactData.GetVendors(tag.ContactType) : this.contactData.GetNonBorrowingOwner(tag.NBOCIndex);
      this.gridViewContacts.SelectedItems[0].SubItems[2].Text = contactFields.DiscToCD;
      this.gridViewContacts.SelectedItems[0].SubItems[3].Text = contactFields.Company;
      this.gridViewContacts.SelectedItems[0].SubItems[4].Text = contactFields.ContactType != FileContactRecord.ContactTypes.NonBorrowingOwnerContact ? contactFields.FullName : (contactFields.FirstName + " " + contactFields.LastName).Trim();
      this.gridViewContacts.SelectedItems[0].SubItems[5].Text = contactFields.BizPhone;
      this.gridViewContacts.SelectedItems[0].SubItems[6].Text = contactFields.Email;
      this.gridViewContacts.SelectedItems[0].Tag = (object) contactFields;
    }

    private void freeScreen_FieldOnKeyUp(object sender, EventArgs e)
    {
      if (this.gridViewContacts.SelectedItems.Count == 0)
        return;
      mshtml.IHTMLEventObj htmlEventObj = (mshtml.IHTMLEventObj) sender;
      string attribute = (string) htmlEventObj.srcElement.getAttribute("emid");
      if (",1264,617,610,411,56,VEND.X122,VEND.X133,VEND.X144,VEND.X424,713,L252,L248,VEND.X34,1500,624,REGZGFE.X8,VEND.X178,395,VEND.X200,VEND.X44,VEND.X263,VEND.X278,VEND.X293,VEND.X310,VEND.X655,VEND.X54,VEND.X64,VEND.X74,VEND.X1,".IndexOf("," + attribute + ",") > -1)
        this.gridViewContacts.SelectedItems[0].SubItems[3].Text = ((IHTMLInputElement) htmlEventObj.srcElement).value;
      else if (",1256,618,611,416,VEND.X117,VEND.X128,VEND.X139,VEND.X150,638,VEND.X412,Seller3.Name,Seller4.Name,VEND.X429,714,VEND.X162,707,VEND.X35,VEND.X13,625,984,VEND.X184,VEND.X195,VEND.X206,VEND.X45,VEND.X271,VEND.X286,VEND.X302,VEND.X317,VEND.X668,VEND.X55,VEND.X65,VEND.X75,VEND.X2,".IndexOf("," + attribute + ",") > -1)
        this.gridViewContacts.SelectedItems[0].SubItems[4].Text = ((IHTMLInputElement) htmlEventObj.srcElement).value;
      else if (",FE0117,FE0217,1262,622,615,417,VEND.X118,VEND.X129,VEND.X140,VEND.X151,VEND.X220,VEND.X421,Seller3.BusPh,Seller4.BusPh,VEND.X430,718,VEND.X163,711,VEND.X41,VEND.X19,629,1410,VEND.X185,VEND.X196,VEND.X207,VEND.X51,VEND.X272,VEND.X287,VEND.X303,VEND.X318,VEND.X669,VEND.X61,VEND.X71,VEND.X81,VEND.X8,".IndexOf("," + attribute + ",") > -1)
        this.gridViewContacts.SelectedItems[0].SubItems[5].Text = ((IHTMLInputElement) htmlEventObj.srcElement).value;
      else if (",1240,1268,95,89,87,88,VEND.X119,VEND.X130,VEND.X141,VEND.X152,92,VEND.X419,Seller3.Email,Seller4.Email,VEND.X432,94,VEND.X164,93,VEND.X43,VEND.X21,90,1411,VEND.X186,VEND.X197,VEND.X208,VEND.X53,VEND.X273,VEND.X288,VEND.X305,VEND.X319,VEND.X670,VEND.X63,VEND.X73,VEND.X83,VEND.X10,".IndexOf("," + attribute + ",") > -1 || attribute.StartsWith("NBOC") && attribute.EndsWith("11"))
      {
        string emailAddresses = ((IHTMLInputElement) htmlEventObj.srcElement).value;
        if (emailAddresses != null && emailAddresses != string.Empty && Utils.ValidateEmail(emailAddresses))
          this.gridViewContacts.SelectedItems[0].SubItems[6].Text = emailAddresses;
        else
          this.gridViewContacts.SelectedItems[0].SubItems[6].Text = "";
      }
      else if (",VEND.X84,VEND.X85,VEND.X86,VEND.X11,".IndexOf("," + attribute + ",") > -1)
      {
        string str = ((IHTMLInputElement) htmlEventObj.srcElement).value;
        if ((str ?? "") == "")
        {
          str = "Custom Category #";
          switch (attribute)
          {
            case "VEND.X84":
              str += "1";
              break;
            case "VEND.X85":
              str += "2";
              break;
            case "VEND.X86":
              str += "3";
              break;
            case "VEND.X11":
              str += "4";
              break;
          }
        }
        this.gridViewContacts.SelectedItems[0].SubItems[1].Text = str;
      }
      else if (",4000,4001,4002,4003,4004,4005,4006,4007,".IndexOf("," + attribute + ",") > -1)
      {
        string str1 = ((IHTMLInputElement) htmlEventObj.srcElement).value;
        string str2 = "";
        int num1 = 4000;
        int num2 = 4003;
        if (attribute == "4004" || attribute == "4005" || attribute == "4006" || attribute == "4007")
        {
          num1 = 4004;
          num2 = 4007;
        }
        string empty = string.Empty;
        for (int index = num1; index <= num2; ++index)
        {
          string str3 = !(index.ToString() == attribute) ? this.loan.GetField(index.ToString()) : str1;
          if (str3 != null && !(str3.Trim() == string.Empty))
            str2 = str2 + " " + str3;
        }
        this.gridViewContacts.SelectedItems[0].SubItems[4].Text = str2.Trim().Trim();
      }
      else
      {
        if (!attribute.StartsWith("NBOC") || !attribute.EndsWith("01") && !attribute.EndsWith("02") && !attribute.EndsWith("03") && !attribute.EndsWith("04") && !attribute.EndsWith("11") && !attribute.EndsWith("13"))
          return;
        string str = ((IHTMLInputElement) htmlEventObj.srcElement).value;
        FileContactRecord.ContactFields tag = (FileContactRecord.ContactFields) this.gridViewContacts.SelectedItems[0].Tag;
        if (attribute.EndsWith("01") || attribute.EndsWith("02") || attribute.EndsWith("03") || attribute.EndsWith("04"))
        {
          if (attribute.EndsWith("01"))
            tag.FirstName = str + " " + this.loan.GetField("NBOC" + tag.NBOCIndex.ToString("00") + "02");
          else if (attribute.EndsWith("03"))
            tag.LastName = str + " " + this.loan.GetField("NBOC" + tag.NBOCIndex.ToString("00") + "04");
          this.gridViewContacts.SelectedItems[0].SubItems[4].Text = (tag.FirstName + " " + tag.LastName).Trim();
        }
        else if (attribute.EndsWith("11"))
        {
          tag.Email = str ?? "";
          this.gridViewContacts.SelectedItems[0].SubItems[6].Text = tag.Email;
        }
        else
        {
          if (!attribute.EndsWith("13"))
            return;
          tag.BizPhone = str ?? "";
          this.gridViewContacts.SelectedItems[0].SubItems[5].Text = tag.BizPhone;
        }
      }
    }

    public void RefreshContents()
    {
      int nItemIndex1 = -1;
      if (this.gridViewContacts.SelectedItems.Count > 0)
        nItemIndex1 = this.gridViewContacts.SelectedItems[0].Index;
      this.initForm();
      if (this.gridViewContacts.Items.Count <= nItemIndex1)
        return;
      for (int nItemIndex2 = 0; nItemIndex2 < this.gridViewContacts.Items.Count; ++nItemIndex2)
        this.gridViewContacts.Items[nItemIndex2].Selected = false;
      this.gridViewContacts.Items[nItemIndex1].Selected = true;
    }

    public void RefreshLoanContents()
    {
      int nItemIndex1 = -1;
      if (this.gridViewContacts.SelectedItems.Count > 0)
        nItemIndex1 = this.gridViewContacts.SelectedItems[0].Index;
      this.loadRoleSettings();
      this.initForm();
      if (this.gridViewContacts.Items.Count <= nItemIndex1)
        return;
      for (int nItemIndex2 = 0; nItemIndex2 < this.gridViewContacts.Items.Count; ++nItemIndex2)
        this.gridViewContacts.Items[nItemIndex2].Selected = false;
      this.gridViewContacts.Items[nItemIndex1].Selected = true;
    }

    public void AddNewNonBorrowingOwnerContact()
    {
      int num = this.loan.NewNonBorrowingOwnerContact();
      if (num == -1)
        return;
      int i = num + 1;
      this.gridViewContacts.SelectedItems.Clear();
      this.addContactToListForm(this.gridViewContacts.Items.Count + 1, this.contactData.GetNonBorrowingOwner(i));
      this.gridViewContacts.Items[this.gridViewContacts.Items.Count - 1].Selected = true;
      this.gridViewContacts.EnsureVisible(this.gridViewContacts.Items.Count - 1);
    }

    public void RemoveNewNonBorrowingOwnerContact()
    {
      if (this.gridViewContacts.SelectedItems.Count == 0)
        return;
      object tag1 = this.gridViewContacts.SelectedItems[0].Tag;
      if (!(tag1 is FileContactRecord.ContactFields))
        return;
      FileContactRecord.ContactFields contactFields1 = (FileContactRecord.ContactFields) tag1;
      if (contactFields1.ContactType != FileContactRecord.ContactTypes.NonBorrowingOwnerContact || this.loan.GetField("NBOC" + contactFields1.NBOCIndex.ToString("00") + "99") != "" && Utils.Dialog((IWin32Window) this, this.loan.Use2015RESPA ? "You are about to delete the vesting information for " + contactFields1.FullName + ", a Non-Borrowing Owner. Once completed, you should review or re-generate any disclosures, documents, Closing Disclosures, beneficiary and final vesting statements to reflect this change. \r\n\r\nWould you like to proceed?" : "You are about to delete the vesting information for " + contactFields1.FullName + ", a Non-Borrowing Owner. Once completed, you should review or re-generate any disclosures, documents, HUD-1 Settlement Statements, beneficiary and final vesting statements to reflect this change. \r\n\r\nWould you like to proceed?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes || !this.loan.RemoveNonBorrowingOwnerContactAt(contactFields1.NBOCIndex))
        return;
      for (int nItemIndex = 0; nItemIndex < this.gridViewContacts.Items.Count; ++nItemIndex)
      {
        object tag2 = this.gridViewContacts.Items[nItemIndex].Tag;
        if (tag2 is FileContactRecord.ContactFields)
        {
          FileContactRecord.ContactFields contactFields2 = (FileContactRecord.ContactFields) tag2;
          if (contactFields2.ContactType == FileContactRecord.ContactTypes.NonBorrowingOwnerContact && contactFields2.NBOCIndex > contactFields1.NBOCIndex)
          {
            --contactFields2.NBOCIndex;
            int num = Utils.ParseInt((object) this.gridViewContacts.Items[nItemIndex].Text) - 1;
            this.gridViewContacts.Items[nItemIndex].Text = string.Concat((object) num);
          }
        }
      }
      int index = this.gridViewContacts.SelectedItems[0].Index;
      this.gridViewContacts.Items.Remove(this.gridViewContacts.SelectedItems[0]);
      if (this.gridViewContacts.Items.Count <= index)
        this.gridViewContacts.Items[this.gridViewContacts.Items.Count - 1].Selected = true;
      else
        this.gridViewContacts.Items[index].Selected = true;
    }
  }
}
