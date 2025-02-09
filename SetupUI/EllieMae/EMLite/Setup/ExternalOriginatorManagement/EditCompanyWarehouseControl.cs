// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.EditCompanyWarehouseControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.ExternalOriginatorManagement.RestApi;
using EllieMae.EMLite.UI;
using EllieMae.Encompass.AsmResolver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.ExternalOriginatorManagement
{
  public class EditCompanyWarehouseControl : UserControl
  {
    private Sessions.Session session;
    private int externalOrgId;
    private int parentId;
    private List<ExternalBank> externalBanks;
    private List<ExternalOrgWarehouse> externalOrgWarehouses;
    private ExternalOrgWarehouse selectedWarehouse;
    private InputEventHelper inputEventHelper;
    private bool useParentInfo;
    private bool hasWarehouseEditRight;
    private bool isTPOTool;
    private IContainer components;
    private GroupContainer grpAll;
    private Panel panel1;
    private Panel panel2;
    private Panel panelMarylandKansas;
    private GroupContainer grpMaryland;
    private GroupContainer grpKansas;
    private Panel panelLicenseList;
    private GroupContainer grpLicenseList;
    private StandardIconButton btnDelete;
    private StandardIconButton btnAdd;
    private GridView gridView;
    private StandardIconButton btnReset;
    private StandardIconButton btnSave;
    private Panel panelHeader;
    private Label label33;
    private GroupContainer groupContainer1;
    private CheckBox chkUseParentInfo;
    private Label label8;
    private Label label7;
    private Label label6;
    private Label label5;
    private Label label4;
    private Label label3;
    private Label label2;
    private Label label1;
    private Label label10;
    private TextBox txtDesc;
    private TextBox txtAccount;
    private TextBox txtABA;
    private TextBox txtCity;
    private TextBox txtAddress1;
    private TextBox txtAddress;
    private TextBox txtBankName;
    private TextBox txtZip;
    private Label label11;
    private Label label13;
    private Label label12;
    private Label label14;
    private ComboBox cmbTri;
    private ComboBox cmbBailee;
    private ComboBox cmbSelf;
    private TextBox txtFax;
    private TextBox txtPhone;
    private TextBox txtEmail;
    private TextBox txtName;
    private Label label18;
    private Label label17;
    private Label label16;
    private Label label15;
    private GroupContainer grpLicensingIssues;
    private TextBox txtNotes;
    private TextBox txtState;
    private CheckBox chkUseBank;
    private DatePicker dtExpiration;
    private StandardIconButton btReset;
    private StandardIconButton btSave;
    private Panel panel3;
    private Label label21;
    private TextBox txtCreditAcctName;
    private Label label22;
    private Label label20;
    private TextBox txtCreditAcctNum;
    private Label label19;
    private TextBox txtAcctName;
    private Label label9;
    private CheckBox cboxApproved;
    private Label label23;
    private ComboBox cmbTimeZone;
    private Label label24;
    private DatePicker dpStatusDate;
    private Label label25;

    public EditCompanyWarehouseControl(
      Sessions.Session session,
      int externalOrgId,
      int parent,
      bool edit,
      bool isTPOTool)
    {
      this.InitializeComponent();
      this.inputEventHelper = new InputEventHelper();
      this.session = session;
      this.externalOrgId = externalOrgId;
      this.parentId = parent;
      this.externalBanks = this.session.ConfigurationManager.GetExternalBanks();
      this.isTPOTool = isTPOTool;
      if (parent == 0)
        this.chkUseParentInfo.Enabled = false;
      else
        this.chkUseParentInfo.Checked = this.useParentInfo = this.session.ConfigurationManager.GetInheritWarehouses(this.externalOrgId);
      if (this.chkUseParentInfo.Checked)
        this.refreshGrid(this.parentId);
      else
        this.refreshGrid(externalOrgId);
      this.initFieldEvents();
      bool hierarchyAccess;
      Session.TpoHierarchyAccessCache.TryGetValue(externalOrgId, out hierarchyAccess);
      TPOClientUtils.DisableControls((UserControl) this, hierarchyAccess);
      this.hasWarehouseEditRight = ((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.ExternalSettings_EditWarehouseTab);
      this.gridView.SelectedIndexChanged += new EventHandler(this.gridView1_SelectedIndexChanged);
      if (isTPOTool)
        this.DisableControls();
      ImageList imageList = new ImageList();
      Image image = Image.FromFile(AssemblyResolver.GetResourceFileFullPath(Path.Combine(SystemSettings.ImageRelDir, "check-mark-green.png"), SystemSettings.LocalAppDir));
      imageList.Images.Add(image);
      this.gridView.ImageList = imageList;
    }

    private void initFieldEvents()
    {
      this.inputEventHelper.AddPhoneFieldHelper(this.txtPhone);
      this.inputEventHelper.AddPhoneFieldHelper(this.txtFax);
    }

    public void DisableControls()
    {
      this.btnAdd.Visible = false;
      this.btnDelete.Visible = false;
      this.btnReset.Visible = false;
      this.btnSave.Visible = false;
      this.btSave.Visible = false;
      this.btReset.Visible = false;
    }

    public void btnSave_Click(object sender, EventArgs e)
    {
      this.session.ConfigurationManager.UpdateInheritWarehouses(this.externalOrgId, this.chkUseParentInfo.Checked);
      WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.Parent.Text, this.externalOrgId);
      this.refreshGrid(this.externalOrgId);
    }

    private void btnReset_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to reset? All changes will be lost.", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
        return;
      this.chkUseParentInfo.Checked = this.useParentInfo = this.session.ConfigurationManager.GetInheritWarehouses(this.externalOrgId);
      this.refreshGrid(this.externalOrgId);
    }

    private void textField_Changed(object sender, EventArgs e)
    {
      this.setGridButtonStatus(false);
      this.SetWarehouseDetailButtonStatus(true);
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      if (!this.semiSave())
        return;
      ExternalbankList externalbankList = new ExternalbankList(this.externalBanks);
      if (externalbankList.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      ExternalBank selectedbank = externalbankList.selectedbank;
      ExternalOrgWarehouse externalOrgWarehouse = this.session.ConfigurationManager.AddExternalOrgWarehouse(new ExternalOrgWarehouse()
      {
        ExternalOrgID = this.externalOrgId,
        BankID = selectedbank.BankID,
        UseBankContact = true,
        BankName = selectedbank.BankName,
        Address = selectedbank.Address,
        Address1 = selectedbank.Address1,
        City = selectedbank.City,
        State = selectedbank.State,
        Zip = selectedbank.Zip,
        ABANumber = selectedbank.ABANumber,
        BankContactName = selectedbank.ContactName,
        BankContactEmail = selectedbank.ContactEmail,
        BankContactPhone = selectedbank.ContactPhone,
        BankContactFax = selectedbank.ContactFax,
        TimeZone = selectedbank.TimeZone
      });
      WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.Parent.Text, this.externalOrgId);
      this.gridView.SelectedItems.Clear();
      GVItem gvItem = new GVItem(new string[8]
      {
        externalOrgWarehouse.BankName,
        externalOrgWarehouse.City,
        externalOrgWarehouse.State,
        externalOrgWarehouse.Zip,
        externalOrgWarehouse.BankContactName,
        externalOrgWarehouse.BankContactPhone,
        externalOrgWarehouse.ABANumber,
        selectedbank.DateAdded.ToString("d")
      });
      gvItem.Tag = (object) externalOrgWarehouse;
      this.gridView.Items.Add(gvItem);
      this.SetWarehouseDetailButtonStatus(false);
      this.setGridButtonStatus(true);
      this.panelMarylandKansas.Enabled = false;
      gvItem.Selected = true;
      this.gridView.EnsureVisible(gvItem.Index);
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (!this.gridView.SelectedItems.Any<GVItem>() || !this.semiSave())
        return;
      this.selectedWarehouse = (ExternalOrgWarehouse) this.gridView.SelectedItems[0].Tag;
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to delete warehouse for " + this.selectedWarehouse.BankName + "?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
        return;
      this.session.ConfigurationManager.DeleteExternalOrgWarehouse(this.selectedWarehouse.WarehouseID, this.selectedWarehouse.ExternalOrgID);
      WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.Parent.Text, this.externalOrgId);
      this.refreshGrid(this.externalOrgId);
      this.populateWarehouseDetails();
      this.SetWarehouseDetailButtonStatus(false);
      this.setGridButtonStatus(true);
    }

    public bool validate()
    {
      if (!(this.txtEmail.Text.Trim() != "") || Utils.ValidateEmail(this.txtEmail.Text.Trim()))
        return true;
      int num = (int) Utils.Dialog((IWin32Window) this, "The email address format is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      this.txtEmail.Focus();
      return false;
    }

    public void btSave_Click(object sender, EventArgs e)
    {
      if (!this.semiSave())
      {
        this.SetWarehouseDetailButtonStatus(false);
        this.setGridButtonStatus(true);
      }
      else
      {
        if (!this.validate())
          return;
        this.txt_Leave((object) null, (EventArgs) null);
        if (!this.chkUseBank.Checked)
        {
          this.selectedWarehouse.ContactName = this.txtName.Text;
          this.selectedWarehouse.ContactEmail = this.txtEmail.Text;
          this.selectedWarehouse.ContactPhone = this.txtPhone.Text;
          this.selectedWarehouse.ContactFax = this.txtFax.Text;
        }
        else
          this.selectedWarehouse.ContactEmail = this.selectedWarehouse.ContactName = this.selectedWarehouse.ContactPhone = this.selectedWarehouse.ContactFax = "";
        this.selectedWarehouse.Notes = this.txtNotes.Text;
        this.selectedWarehouse.SelfFunder = this.cmbSelf.SelectedIndex;
        this.selectedWarehouse.BaileeReq = this.cmbBailee.SelectedIndex;
        this.selectedWarehouse.TriParty = this.cmbTri.SelectedIndex;
        this.selectedWarehouse.TimeZone = Convert.ToString(this.cmbTimeZone.SelectedItem);
        this.selectedWarehouse.AcctNumber = this.txtAccount.Text;
        this.selectedWarehouse.Description = this.txtDesc.Text;
        this.selectedWarehouse.Expiration = Convert.ToDateTime(this.dtExpiration.Value);
        this.selectedWarehouse.UseBankContact = this.chkUseBank.Checked;
        this.selectedWarehouse.Approved = this.cboxApproved.Checked;
        this.selectedWarehouse.AcctName = this.txtAcctName.Text;
        this.selectedWarehouse.CreditAcctName = this.txtCreditAcctName.Text;
        this.selectedWarehouse.CreditAcctNumber = this.txtCreditAcctNum.Text;
        this.selectedWarehouse.StatusDate = this.dpStatusDate.Value;
        this.session.ConfigurationManager.UpdateExternalOrgWarehouse(this.selectedWarehouse.WarehouseID, this.selectedWarehouse);
        WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.Parent.Text, this.externalOrgId);
        GVItem gvItem = this.createitem(this.selectedWarehouse);
        this.SetApprovedImage(this.selectedWarehouse.Approved, this.gridView.SelectedItems[0]);
        this.gridView.SelectedItems[0].SubItems[4].Text = gvItem.SubItems[4].Text;
        this.gridView.SelectedItems[0].SubItems[5].Text = gvItem.SubItems[5].Text;
        this.gridView.Refresh();
        this.SetWarehouseDetailButtonStatus(false);
        this.setGridButtonStatus(true);
      }
    }

    private void btReset_Click(object sender, EventArgs e)
    {
      if (!this.semiSave())
      {
        this.SetWarehouseDetailButtonStatus(false);
        this.setGridButtonStatus(true);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "Are you sure you want to reset? All changes will be lost.", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
          return;
        this.populateWarehouseDetails();
        this.SetWarehouseDetailButtonStatus(false);
        this.setGridButtonStatus(true);
      }
    }

    private void gridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.gridView.SelectedItems.Any<GVItem>())
      {
        if (!this.chkUseParentInfo.Checked && this.hasWarehouseEditRight && !this.isTPOTool)
          this.panelMarylandKansas.Enabled = this.btnDelete.Enabled = true;
        if (this.gridView.SelectedItems.Count<GVItem>() == 1)
        {
          this.populateWarehouseDetails();
          this.SetWarehouseDetailButtonStatus(false);
          this.setGridButtonStatus(true);
          this.txtAccount.Focus();
        }
      }
      else
      {
        this.populateWarehouseDetails();
        this.SetWarehouseDetailButtonStatus(false);
        this.setGridButtonStatus(true);
        this.panelMarylandKansas.Enabled = this.btnDelete.Enabled = false;
      }
      if (!this.chkUseParentInfo.Checked && this.hasWarehouseEditRight && !this.isTPOTool)
        return;
      this.btnAdd.Enabled = false;
    }

    private void chkUseBank_CheckedChanged(object sender, EventArgs e)
    {
      this.setContactDetails(false);
    }

    private void chkUseParentInfo_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkUseParentInfo.Checked)
        this.refreshGrid(this.parentId);
      this.SetReadOnly(this.chkUseParentInfo.Checked);
      this.SetMainButtonStatus(true);
    }

    private void refreshGrid(int id)
    {
      this.externalOrgWarehouses = this.session.ConfigurationManager.GetExternalOrgWarehouses(id);
      this.gridView.Items.Clear();
      foreach (ExternalOrgWarehouse externalOrgWarehouse in this.externalOrgWarehouses)
      {
        GVItem gvItem = this.createitem(externalOrgWarehouse);
        this.SetApprovedImage(externalOrgWarehouse.Approved, gvItem);
        this.gridView.Items.Add(gvItem);
      }
      this.gridView.Sort(0, SortOrder.Ascending);
      this.gridView.Refresh();
      this.panelMarylandKansas.Enabled = false;
      this.SetMainButtonStatus(false);
      this.SetWarehouseDetailButtonStatus(false);
      this.btnDelete.Enabled = false;
    }

    private void SetApprovedImage(bool approved, GVItem item)
    {
      item.SubItems[8].ImageAlignment = HorizontalAlignment.Center;
      if (approved)
        item.SubItems[8].ImageIndex = 0;
      else
        item.SubItems[8].ImageIndex = -1;
    }

    private GVItem createitem(ExternalOrgWarehouse wareHouse)
    {
      GVItem gvItem;
      if (!wareHouse.UseBankContact)
      {
        gvItem = new GVItem(new string[8]
        {
          wareHouse.BankName,
          wareHouse.City,
          wareHouse.State,
          wareHouse.Zip,
          wareHouse.ContactName,
          wareHouse.ContactPhone,
          wareHouse.ABANumber,
          wareHouse.DateAdded.ToString("d")
        });
        gvItem.Tag = (object) wareHouse;
      }
      else
      {
        gvItem = new GVItem(new string[8]
        {
          wareHouse.BankName,
          wareHouse.City,
          wareHouse.State,
          wareHouse.Zip,
          wareHouse.BankContactName,
          wareHouse.BankContactPhone,
          wareHouse.ABANumber,
          wareHouse.DateAdded.ToString("d")
        });
        gvItem.Tag = (object) wareHouse;
      }
      return gvItem;
    }

    private void populateWarehouseDetails()
    {
      this.selectedWarehouse = this.gridView.SelectedItems.Count <= 0 ? new ExternalOrgWarehouse() : (ExternalOrgWarehouse) this.gridView.SelectedItems[0].Tag;
      this.txtBankName.Text = this.selectedWarehouse.BankName;
      this.txtAddress.Text = this.selectedWarehouse.Address;
      this.txtAddress1.Text = this.selectedWarehouse.Address1;
      this.txtCity.Text = this.selectedWarehouse.City;
      this.txtState.Text = this.selectedWarehouse.State;
      this.txtZip.Text = this.selectedWarehouse.Zip;
      this.txtABA.Text = this.selectedWarehouse.ABANumber;
      this.chkUseBank.Checked = this.selectedWarehouse.UseBankContact;
      this.setContactDetails(true);
      this.txtNotes.Text = this.selectedWarehouse.Notes;
      this.cmbSelf.SelectedIndex = this.selectedWarehouse.SelfFunder;
      this.cmbBailee.SelectedIndex = this.selectedWarehouse.BaileeReq;
      this.cmbTri.SelectedIndex = this.selectedWarehouse.TriParty;
      this.cmbTimeZone.SelectedItem = string.IsNullOrEmpty(this.selectedWarehouse.TimeZone) ? (object) (string) null : (object) this.selectedWarehouse.TimeZone;
      this.txtAccount.Text = this.selectedWarehouse.AcctNumber;
      this.txtDesc.Text = this.selectedWarehouse.Description;
      this.cboxApproved.Checked = this.selectedWarehouse.Approved;
      this.txtAcctName.Text = this.selectedWarehouse.AcctName;
      this.txtCreditAcctName.Text = this.selectedWarehouse.CreditAcctName;
      this.txtCreditAcctNum.Text = this.selectedWarehouse.CreditAcctNumber;
      this.dpStatusDate.Value = this.selectedWarehouse.StatusDate;
      if (this.selectedWarehouse.Expiration != DateTime.MinValue)
        this.dtExpiration.Value = this.selectedWarehouse.Expiration;
      else
        this.dtExpiration.Value = new DateTime();
    }

    private void setContactDetails(bool flag)
    {
      if (this.chkUseBank.Checked)
      {
        if (this.selectedWarehouse != null)
        {
          this.txtName.Text = this.selectedWarehouse.BankContactName;
          this.txtEmail.Text = this.selectedWarehouse.BankContactEmail;
          this.txtPhone.Text = this.selectedWarehouse.BankContactPhone;
          this.txtFax.Text = this.selectedWarehouse.BankContactFax;
        }
        this.txtName.Enabled = this.txtPhone.Enabled = this.txtFax.Enabled = this.txtEmail.Enabled = false;
      }
      else
      {
        if (this.selectedWarehouse != null)
        {
          if (((this.selectedWarehouse.ContactName != null && !string.Empty.Equals(this.selectedWarehouse.ContactName) || this.selectedWarehouse.ContactEmail != null && !string.Empty.Equals(this.selectedWarehouse.ContactEmail) || this.selectedWarehouse.ContactPhone != null && !string.Empty.Equals(this.selectedWarehouse.ContactPhone) ? 1 : (this.selectedWarehouse.ContactFax == null ? 0 : (!string.Empty.Equals(this.selectedWarehouse.ContactFax) ? 1 : 0))) | (flag ? 1 : 0)) != 0)
          {
            this.txtName.Text = this.selectedWarehouse.ContactName;
            this.txtEmail.Text = this.selectedWarehouse.ContactEmail;
            this.txtPhone.Text = this.selectedWarehouse.ContactPhone;
            this.txtFax.Text = this.selectedWarehouse.ContactFax;
          }
          else
          {
            this.txtName.Text = this.selectedWarehouse.BankContactName;
            this.txtEmail.Text = this.selectedWarehouse.BankContactEmail;
            this.txtPhone.Text = this.selectedWarehouse.BankContactPhone;
            this.txtFax.Text = this.selectedWarehouse.BankContactFax;
          }
        }
        this.txtName.Enabled = this.txtPhone.Enabled = this.txtFax.Enabled = this.txtEmail.Enabled = true;
      }
    }

    private void SetReadOnly(bool flag) => this.btnAdd.Enabled = this.btnDelete.Enabled = !flag;

    private void setGridButtonStatus(bool status)
    {
      this.gridView.Enabled = this.btnAdd.Enabled = status;
    }

    public void SetWarehouseDetailButtonStatus(bool enabled)
    {
      this.btReset.Enabled = this.btSave.Enabled = enabled;
    }

    public void SetMainButtonStatus(bool enabled)
    {
      this.btnReset.Enabled = this.btnSave.Enabled = enabled;
    }

    public bool IsDirty => this.btSave.Enabled;

    public bool IsChkParentInfoDirty => this.btnSave.Enabled;

    private bool semiSave()
    {
      if (!this.IsChkParentInfoDirty)
        return true;
      if (Utils.Dialog((IWin32Window) this, "You must save changes before updating details. Do you want to save now?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        this.btnSave_Click((object) null, (EventArgs) null);
      return false;
    }

    private void txt_Leave(object sender, EventArgs e)
    {
      if (this.txtPhone.Text.Trim() != "" && this.txtPhone.TextLength > 30)
        this.txtPhone.Text = string.Empty;
      if (!(this.txtFax.Text.Trim() != "") || this.txtFax.TextLength <= 30)
        return;
      this.txtFax.Text = string.Empty;
    }

    private void gridView_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
    {
      if (!e.KeyData.HasFlag((Enum) Keys.Control) || !e.KeyData.HasFlag((Enum) Keys.A))
        return;
      this.gridView.SelectedItems.Clear();
    }

    private void cboxApproved_CheckedChanged(object sender, EventArgs e)
    {
      this.dpStatusDate.Value = DateTime.Now;
      this.textField_Changed((object) null, (EventArgs) null);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      GVColumn gvColumn8 = new GVColumn();
      GVColumn gvColumn9 = new GVColumn();
      this.grpAll = new GroupContainer();
      this.panel1 = new Panel();
      this.panelMarylandKansas = new Panel();
      this.panel3 = new Panel();
      this.grpLicensingIssues = new GroupContainer();
      this.txtNotes = new TextBox();
      this.grpKansas = new GroupContainer();
      this.chkUseBank = new CheckBox();
      this.txtFax = new TextBox();
      this.txtPhone = new TextBox();
      this.txtEmail = new TextBox();
      this.txtName = new TextBox();
      this.label18 = new Label();
      this.label17 = new Label();
      this.label16 = new Label();
      this.label15 = new Label();
      this.grpMaryland = new GroupContainer();
      this.cmbTimeZone = new ComboBox();
      this.label24 = new Label();
      this.cboxApproved = new CheckBox();
      this.label23 = new Label();
      this.label21 = new Label();
      this.txtCreditAcctName = new TextBox();
      this.label22 = new Label();
      this.label20 = new Label();
      this.txtCreditAcctNum = new TextBox();
      this.label19 = new Label();
      this.txtAcctName = new TextBox();
      this.label9 = new Label();
      this.dtExpiration = new DatePicker();
      this.txtState = new TextBox();
      this.cmbTri = new ComboBox();
      this.cmbBailee = new ComboBox();
      this.cmbSelf = new ComboBox();
      this.label14 = new Label();
      this.label13 = new Label();
      this.label12 = new Label();
      this.txtZip = new TextBox();
      this.label11 = new Label();
      this.txtDesc = new TextBox();
      this.txtAccount = new TextBox();
      this.txtABA = new TextBox();
      this.txtCity = new TextBox();
      this.txtAddress1 = new TextBox();
      this.txtAddress = new TextBox();
      this.txtBankName = new TextBox();
      this.label10 = new Label();
      this.label8 = new Label();
      this.label7 = new Label();
      this.label6 = new Label();
      this.label5 = new Label();
      this.label4 = new Label();
      this.label3 = new Label();
      this.label2 = new Label();
      this.label1 = new Label();
      this.panel2 = new Panel();
      this.groupContainer1 = new GroupContainer();
      this.btReset = new StandardIconButton();
      this.btSave = new StandardIconButton();
      this.panelLicenseList = new Panel();
      this.grpLicenseList = new GroupContainer();
      this.chkUseParentInfo = new CheckBox();
      this.btnDelete = new StandardIconButton();
      this.btnAdd = new StandardIconButton();
      this.gridView = new GridView();
      this.btnReset = new StandardIconButton();
      this.btnSave = new StandardIconButton();
      this.panelHeader = new Panel();
      this.label33 = new Label();
      this.dpStatusDate = new DatePicker();
      this.label25 = new Label();
      this.grpAll.SuspendLayout();
      this.panel1.SuspendLayout();
      this.panelMarylandKansas.SuspendLayout();
      this.panel3.SuspendLayout();
      this.grpLicensingIssues.SuspendLayout();
      this.grpKansas.SuspendLayout();
      this.grpMaryland.SuspendLayout();
      this.panel2.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      ((ISupportInitialize) this.btReset).BeginInit();
      ((ISupportInitialize) this.btSave).BeginInit();
      this.panelLicenseList.SuspendLayout();
      this.grpLicenseList.SuspendLayout();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      ((ISupportInitialize) this.btnAdd).BeginInit();
      ((ISupportInitialize) this.btnReset).BeginInit();
      ((ISupportInitialize) this.btnSave).BeginInit();
      this.panelHeader.SuspendLayout();
      this.SuspendLayout();
      this.grpAll.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.grpAll.Controls.Add((Control) this.panel1);
      this.grpAll.Controls.Add((Control) this.btnReset);
      this.grpAll.Controls.Add((Control) this.btnSave);
      this.grpAll.Controls.Add((Control) this.panelHeader);
      this.grpAll.HeaderForeColor = SystemColors.ControlText;
      this.grpAll.Location = new Point(0, 0);
      this.grpAll.Margin = new Padding(0);
      this.grpAll.Name = "grpAll";
      this.grpAll.Size = new Size(862, 730);
      this.grpAll.TabIndex = 2;
      this.grpAll.Text = "TPO Warehouse Banks";
      this.panel1.AutoScroll = true;
      this.panel1.Controls.Add((Control) this.panelMarylandKansas);
      this.panel1.Controls.Add((Control) this.panel2);
      this.panel1.Controls.Add((Control) this.panelLicenseList);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(1, 52);
      this.panel1.Margin = new Padding(5);
      this.panel1.Name = "panel1";
      this.panel1.Padding = new Padding(5);
      this.panel1.Size = new Size(860, 677);
      this.panel1.TabIndex = 32;
      this.panelMarylandKansas.Controls.Add((Control) this.panel3);
      this.panelMarylandKansas.Controls.Add((Control) this.grpMaryland);
      this.panelMarylandKansas.Dock = DockStyle.Top;
      this.panelMarylandKansas.Location = new Point(5, 192);
      this.panelMarylandKansas.Margin = new Padding(5);
      this.panelMarylandKansas.Name = "panelMarylandKansas";
      this.panelMarylandKansas.Size = new Size(833, 510);
      this.panelMarylandKansas.TabIndex = 5;
      this.panel3.Controls.Add((Control) this.grpLicensingIssues);
      this.panel3.Controls.Add((Control) this.grpKansas);
      this.panel3.Dock = DockStyle.Fill;
      this.panel3.Location = new Point(406, 0);
      this.panel3.Name = "panel3";
      this.panel3.Padding = new Padding(10, 0, 0, 0);
      this.panel3.Size = new Size(427, 510);
      this.panel3.TabIndex = 6;
      this.grpLicensingIssues.Controls.Add((Control) this.txtNotes);
      this.grpLicensingIssues.Dock = DockStyle.Fill;
      this.grpLicensingIssues.HeaderForeColor = SystemColors.ControlText;
      this.grpLicensingIssues.Location = new Point(10, 156);
      this.grpLicensingIssues.Name = "grpLicensingIssues";
      this.grpLicensingIssues.Size = new Size(417, 354);
      this.grpLicensingIssues.TabIndex = 5;
      this.grpLicensingIssues.Text = "Notes";
      this.txtNotes.AcceptsReturn = true;
      this.txtNotes.Dock = DockStyle.Fill;
      this.txtNotes.Location = new Point(1, 26);
      this.txtNotes.MaxLength = 5000;
      this.txtNotes.Multiline = true;
      this.txtNotes.Name = "txtNotes";
      this.txtNotes.ScrollBars = ScrollBars.Vertical;
      this.txtNotes.Size = new Size(415, 327);
      this.txtNotes.TabIndex = 8;
      this.txtNotes.TextChanged += new EventHandler(this.textField_Changed);
      this.grpKansas.Controls.Add((Control) this.chkUseBank);
      this.grpKansas.Controls.Add((Control) this.txtFax);
      this.grpKansas.Controls.Add((Control) this.txtPhone);
      this.grpKansas.Controls.Add((Control) this.txtEmail);
      this.grpKansas.Controls.Add((Control) this.txtName);
      this.grpKansas.Controls.Add((Control) this.label18);
      this.grpKansas.Controls.Add((Control) this.label17);
      this.grpKansas.Controls.Add((Control) this.label16);
      this.grpKansas.Controls.Add((Control) this.label15);
      this.grpKansas.Dock = DockStyle.Top;
      this.grpKansas.HeaderForeColor = SystemColors.ControlText;
      this.grpKansas.Location = new Point(10, 0);
      this.grpKansas.Name = "grpKansas";
      this.grpKansas.Size = new Size(417, 156);
      this.grpKansas.TabIndex = 4;
      this.grpKansas.Text = "Contact Information";
      this.chkUseBank.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkUseBank.AutoSize = true;
      this.chkUseBank.BackColor = Color.Transparent;
      this.chkUseBank.Location = new Point(286, 6);
      this.chkUseBank.Name = "chkUseBank";
      this.chkUseBank.Size = new Size(122, 17);
      this.chkUseBank.TabIndex = 8;
      this.chkUseBank.Text = "Use Default Contact";
      this.chkUseBank.UseVisualStyleBackColor = false;
      this.chkUseBank.CheckedChanged += new EventHandler(this.chkUseBank_CheckedChanged);
      this.chkUseBank.CheckStateChanged += new EventHandler(this.textField_Changed);
      this.txtFax.Location = new Point(86, 106);
      this.txtFax.MaxLength = 30;
      this.txtFax.Name = "txtFax";
      this.txtFax.ShortcutsEnabled = false;
      this.txtFax.Size = new Size(186, 20);
      this.txtFax.TabIndex = 7;
      this.txtFax.TextChanged += new EventHandler(this.textField_Changed);
      this.txtFax.Leave += new EventHandler(this.txt_Leave);
      this.txtPhone.Location = new Point(86, 80);
      this.txtPhone.MaxLength = 30;
      this.txtPhone.Name = "txtPhone";
      this.txtPhone.ShortcutsEnabled = false;
      this.txtPhone.Size = new Size(186, 20);
      this.txtPhone.TabIndex = 6;
      this.txtPhone.TextChanged += new EventHandler(this.textField_Changed);
      this.txtPhone.Leave += new EventHandler(this.txt_Leave);
      this.txtEmail.Location = new Point(86, 55);
      this.txtEmail.MaxLength = 64;
      this.txtEmail.Name = "txtEmail";
      this.txtEmail.Size = new Size(186, 20);
      this.txtEmail.TabIndex = 5;
      this.txtEmail.TextChanged += new EventHandler(this.textField_Changed);
      this.txtName.Location = new Point(86, 30);
      this.txtName.MaxLength = 64;
      this.txtName.Name = "txtName";
      this.txtName.Size = new Size(186, 20);
      this.txtName.TabIndex = 4;
      this.txtName.TextChanged += new EventHandler(this.textField_Changed);
      this.label18.AutoSize = true;
      this.label18.Location = new Point(15, 113);
      this.label18.Name = "label18";
      this.label18.Size = new Size(24, 13);
      this.label18.TabIndex = 3;
      this.label18.Text = "Fax";
      this.label17.AutoSize = true;
      this.label17.Location = new Point(15, 88);
      this.label17.Name = "label17";
      this.label17.Size = new Size(38, 13);
      this.label17.TabIndex = 2;
      this.label17.Text = "Phone";
      this.label16.AutoSize = true;
      this.label16.Location = new Point(15, 63);
      this.label16.Name = "label16";
      this.label16.Size = new Size(32, 13);
      this.label16.TabIndex = 1;
      this.label16.Text = "Email";
      this.label15.AutoSize = true;
      this.label15.Location = new Point(15, 37);
      this.label15.Name = "label15";
      this.label15.Size = new Size(35, 13);
      this.label15.TabIndex = 0;
      this.label15.Text = "Name";
      this.grpMaryland.Controls.Add((Control) this.dpStatusDate);
      this.grpMaryland.Controls.Add((Control) this.label25);
      this.grpMaryland.Controls.Add((Control) this.cmbTimeZone);
      this.grpMaryland.Controls.Add((Control) this.label24);
      this.grpMaryland.Controls.Add((Control) this.cboxApproved);
      this.grpMaryland.Controls.Add((Control) this.label23);
      this.grpMaryland.Controls.Add((Control) this.label21);
      this.grpMaryland.Controls.Add((Control) this.txtCreditAcctName);
      this.grpMaryland.Controls.Add((Control) this.label22);
      this.grpMaryland.Controls.Add((Control) this.label20);
      this.grpMaryland.Controls.Add((Control) this.txtCreditAcctNum);
      this.grpMaryland.Controls.Add((Control) this.label19);
      this.grpMaryland.Controls.Add((Control) this.txtAcctName);
      this.grpMaryland.Controls.Add((Control) this.label9);
      this.grpMaryland.Controls.Add((Control) this.dtExpiration);
      this.grpMaryland.Controls.Add((Control) this.txtState);
      this.grpMaryland.Controls.Add((Control) this.cmbTri);
      this.grpMaryland.Controls.Add((Control) this.cmbBailee);
      this.grpMaryland.Controls.Add((Control) this.cmbSelf);
      this.grpMaryland.Controls.Add((Control) this.label14);
      this.grpMaryland.Controls.Add((Control) this.label13);
      this.grpMaryland.Controls.Add((Control) this.label12);
      this.grpMaryland.Controls.Add((Control) this.txtZip);
      this.grpMaryland.Controls.Add((Control) this.label11);
      this.grpMaryland.Controls.Add((Control) this.txtDesc);
      this.grpMaryland.Controls.Add((Control) this.txtAccount);
      this.grpMaryland.Controls.Add((Control) this.txtABA);
      this.grpMaryland.Controls.Add((Control) this.txtCity);
      this.grpMaryland.Controls.Add((Control) this.txtAddress1);
      this.grpMaryland.Controls.Add((Control) this.txtAddress);
      this.grpMaryland.Controls.Add((Control) this.txtBankName);
      this.grpMaryland.Controls.Add((Control) this.label10);
      this.grpMaryland.Controls.Add((Control) this.label8);
      this.grpMaryland.Controls.Add((Control) this.label7);
      this.grpMaryland.Controls.Add((Control) this.label6);
      this.grpMaryland.Controls.Add((Control) this.label5);
      this.grpMaryland.Controls.Add((Control) this.label4);
      this.grpMaryland.Controls.Add((Control) this.label3);
      this.grpMaryland.Controls.Add((Control) this.label2);
      this.grpMaryland.Controls.Add((Control) this.label1);
      this.grpMaryland.Dock = DockStyle.Left;
      this.grpMaryland.HeaderForeColor = SystemColors.ControlText;
      this.grpMaryland.Location = new Point(0, 0);
      this.grpMaryland.Margin = new Padding(0, 3, 3, 3);
      this.grpMaryland.Name = "grpMaryland";
      this.grpMaryland.Size = new Size(406, 510);
      this.grpMaryland.TabIndex = 3;
      this.grpMaryland.Text = "Business Information";
      this.cmbTimeZone.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbTimeZone.FormattingEnabled = true;
      this.cmbTimeZone.Items.AddRange(new object[7]
      {
        (object) "(UTC -10:00) Hawaii Time",
        (object) "(UTC -09:00) Alaska Time",
        (object) "(UTC -08:00) Pacific Time",
        (object) "(UTC -07:00) Arizona Time",
        (object) "(UTC -07:00) Mountain Time",
        (object) "(UTC -06:00) Central Time",
        (object) "(UTC -05:00) Eastern Time"
      });
      this.cmbTimeZone.Location = new Point(100, 482);
      this.cmbTimeZone.Name = "cmbTimeZone";
      this.cmbTimeZone.Size = new Size(161, 21);
      this.cmbTimeZone.TabIndex = 26;
      this.cmbTimeZone.Enabled = false;
      this.cmbTimeZone.SelectedIndexChanged += new EventHandler(this.textField_Changed);
      this.label24.AutoSize = true;
      this.label24.Location = new Point(11, 485);
      this.label24.Name = "label24";
      this.label24.Size = new Size(55, 13);
      this.label24.TabIndex = 41;
      this.label24.Text = "TimeZone";
      this.cboxApproved.AutoSize = true;
      this.cboxApproved.Location = new Point(100, 36);
      this.cboxApproved.Name = "cboxApproved";
      this.cboxApproved.Size = new Size(15, 14);
      this.cboxApproved.TabIndex = 9;
      this.cboxApproved.UseVisualStyleBackColor = true;
      this.cboxApproved.CheckedChanged += new EventHandler(this.cboxApproved_CheckedChanged);
      this.label23.AutoSize = true;
      this.label23.Location = new Point(11, 35);
      this.label23.Name = "label23";
      this.label23.Size = new Size(53, 13);
      this.label23.TabIndex = 39;
      this.label23.Text = "Approved";
      this.label21.AutoSize = true;
      this.label21.Location = new Point(11, 306);
      this.label21.Name = "label21";
      this.label21.Size = new Size(78, 13);
      this.label21.TabIndex = 38;
      this.label21.Text = "Account Name";
      this.txtCreditAcctName.Location = new Point(100, 297);
      this.txtCreditAcctName.MaxLength = 100;
      this.txtCreditAcctName.Name = "txtCreditAcctName";
      this.txtCreditAcctName.Size = new Size(275, 20);
      this.txtCreditAcctName.TabIndex = 20;
      this.txtCreditAcctName.TextChanged += new EventHandler(this.textField_Changed);
      this.label22.AutoSize = true;
      this.label22.Location = new Point(11, 290);
      this.label22.Name = "label22";
      this.label22.Size = new Size(70, 13);
      this.label22.TabIndex = 36;
      this.label22.Text = "Further Credit";
      this.label20.AutoSize = true;
      this.label20.Location = new Point(11, 274);
      this.label20.Name = "label20";
      this.label20.Size = new Size(57, 13);
      this.label20.TabIndex = 35;
      this.label20.Text = "Account #";
      this.txtCreditAcctNum.Location = new Point(100, 267);
      this.txtCreditAcctNum.MaxLength = 64;
      this.txtCreditAcctNum.Name = "txtCreditAcctNum";
      this.txtCreditAcctNum.Size = new Size(206, 20);
      this.txtCreditAcctNum.TabIndex = 19;
      this.txtCreditAcctNum.TextChanged += new EventHandler(this.textField_Changed);
      this.label19.AutoSize = true;
      this.label19.Location = new Point(11, 261);
      this.label19.Name = "label19";
      this.label19.Size = new Size(70, 13);
      this.label19.TabIndex = 33;
      this.label19.Text = "Further Credit";
      this.txtAcctName.Location = new Point(100, 235);
      this.txtAcctName.MaxLength = 100;
      this.txtAcctName.Name = "txtAcctName";
      this.txtAcctName.Size = new Size(275, 20);
      this.txtAcctName.TabIndex = 18;
      this.txtAcctName.TextChanged += new EventHandler(this.textField_Changed);
      this.label9.AutoSize = true;
      this.label9.Location = new Point(11, 238);
      this.label9.Name = "label9";
      this.label9.Size = new Size(78, 13);
      this.label9.TabIndex = 31;
      this.label9.Text = "Account Name";
      this.dtExpiration.BackColor = SystemColors.Window;
      this.dtExpiration.Location = new Point(100, 373);
      this.dtExpiration.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dtExpiration.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtExpiration.Name = "dtExpiration";
      this.dtExpiration.Size = new Size(114, 21);
      this.dtExpiration.TabIndex = 22;
      this.dtExpiration.ToolTip = "";
      this.dtExpiration.Value = new DateTime(0L);
      this.dtExpiration.ValueChanged += new EventHandler(this.textField_Changed);
      this.txtState.Enabled = false;
      this.txtState.Location = new Point(100, 155);
      this.txtState.Name = "txtState";
      this.txtState.Size = new Size(42, 20);
      this.txtState.TabIndex = 14;
      this.cmbTri.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbTri.FormattingEnabled = true;
      this.cmbTri.Items.AddRange(new object[3]
      {
        (object) "",
        (object) "Yes",
        (object) "No"
      });
      this.cmbTri.Location = new Point(100, 455);
      this.cmbTri.Name = "cmbTri";
      this.cmbTri.Size = new Size(114, 21);
      this.cmbTri.TabIndex = 25;
      this.cmbTri.SelectedIndexChanged += new EventHandler(this.textField_Changed);
      this.cmbTri.TextChanged += new EventHandler(this.textField_Changed);
      this.cmbBailee.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBailee.FormattingEnabled = true;
      this.cmbBailee.Items.AddRange(new object[3]
      {
        (object) "",
        (object) "Yes",
        (object) "No"
      });
      this.cmbBailee.Location = new Point(100, 428);
      this.cmbBailee.Name = "cmbBailee";
      this.cmbBailee.Size = new Size(114, 21);
      this.cmbBailee.TabIndex = 24;
      this.cmbBailee.SelectedIndexChanged += new EventHandler(this.textField_Changed);
      this.cmbBailee.TextChanged += new EventHandler(this.textField_Changed);
      this.cmbSelf.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbSelf.FormattingEnabled = true;
      this.cmbSelf.Items.AddRange(new object[3]
      {
        (object) "",
        (object) "Yes",
        (object) "No"
      });
      this.cmbSelf.Location = new Point(100, 401);
      this.cmbSelf.Name = "cmbSelf";
      this.cmbSelf.Size = new Size(114, 21);
      this.cmbSelf.TabIndex = 23;
      this.cmbSelf.SelectedIndexChanged += new EventHandler(this.textField_Changed);
      this.cmbSelf.TextChanged += new EventHandler(this.textField_Changed);
      this.label14.AutoSize = true;
      this.label14.Location = new Point(11, 458);
      this.label14.Name = "label14";
      this.label14.Size = new Size(89, 13);
      this.label14.TabIndex = 25;
      this.label14.Text = "Tri-Party Contract";
      this.label13.AutoSize = true;
      this.label13.Location = new Point(11, 436);
      this.label13.Name = "label13";
      this.label13.Size = new Size(89, 13);
      this.label13.TabIndex = 24;
      this.label13.Text = "Bailee Letter Req";
      this.label12.AutoSize = true;
      this.label12.Location = new Point(11, 409);
      this.label12.Name = "label12";
      this.label12.Size = new Size(61, 13);
      this.label12.TabIndex = 23;
      this.label12.Text = "Self-Funder";
      this.txtZip.Enabled = false;
      this.txtZip.Location = new Point(189, 156);
      this.txtZip.Name = "txtZip";
      this.txtZip.Size = new Size(117, 20);
      this.txtZip.TabIndex = 15;
      this.label11.AutoSize = true;
      this.label11.Location = new Point(161, 159);
      this.label11.Name = "label11";
      this.label11.Size = new Size(22, 13);
      this.label11.TabIndex = 21;
      this.label11.Text = "Zip";
      this.txtDesc.AcceptsReturn = true;
      this.txtDesc.Location = new Point(100, 322);
      this.txtDesc.MaxLength = 100;
      this.txtDesc.Multiline = true;
      this.txtDesc.Name = "txtDesc";
      this.txtDesc.ScrollBars = ScrollBars.Vertical;
      this.txtDesc.Size = new Size(206, 45);
      this.txtDesc.TabIndex = 21;
      this.txtDesc.TextChanged += new EventHandler(this.textField_Changed);
      this.txtAccount.Location = new Point(100, 209);
      this.txtAccount.MaxLength = 64;
      this.txtAccount.Name = "txtAccount";
      this.txtAccount.Size = new Size(206, 20);
      this.txtAccount.TabIndex = 17;
      this.txtAccount.TextChanged += new EventHandler(this.textField_Changed);
      this.txtABA.Enabled = false;
      this.txtABA.Location = new Point(100, 183);
      this.txtABA.Name = "txtABA";
      this.txtABA.Size = new Size(206, 20);
      this.txtABA.TabIndex = 16;
      this.txtCity.Enabled = false;
      this.txtCity.Location = new Point(100, 130);
      this.txtCity.Name = "txtCity";
      this.txtCity.Size = new Size(206, 20);
      this.txtCity.TabIndex = 13;
      this.txtAddress1.Enabled = false;
      this.txtAddress1.Location = new Point(100, 106);
      this.txtAddress1.Name = "txtAddress1";
      this.txtAddress1.Size = new Size(206, 20);
      this.txtAddress1.TabIndex = 12;
      this.txtAddress.Enabled = false;
      this.txtAddress.Location = new Point(100, 82);
      this.txtAddress.Name = "txtAddress";
      this.txtAddress.Size = new Size(206, 20);
      this.txtAddress.TabIndex = 11;
      this.txtBankName.Enabled = false;
      this.txtBankName.Location = new Point(100, 56);
      this.txtBankName.Name = "txtBankName";
      this.txtBankName.Size = new Size(206, 20);
      this.txtBankName.TabIndex = 10;
      this.label10.AutoSize = true;
      this.label10.Location = new Point(11, 381);
      this.label10.Name = "label10";
      this.label10.Size = new Size(85, 13);
      this.label10.TabIndex = 9;
      this.label10.Text = "Bailee Expiration";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(11, 325);
      this.label8.Name = "label8";
      this.label8.Size = new Size(60, 13);
      this.label8.TabIndex = 7;
      this.label8.Text = "Description";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(11, 214);
      this.label7.Name = "label7";
      this.label7.Size = new Size(57, 13);
      this.label7.TabIndex = 6;
      this.label7.Text = "Account #";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(11, 190);
      this.label6.Name = "label6";
      this.label6.Size = new Size(38, 13);
      this.label6.TabIndex = 5;
      this.label6.Text = "ABA #";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(11, 163);
      this.label5.Name = "label5";
      this.label5.Size = new Size(32, 13);
      this.label5.TabIndex = 4;
      this.label5.Text = "State";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(11, 137);
      this.label4.Name = "label4";
      this.label4.Size = new Size(24, 13);
      this.label4.TabIndex = 3;
      this.label4.Text = "City";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(11, 113);
      this.label3.Name = "label3";
      this.label3.Size = new Size(51, 13);
      this.label3.TabIndex = 2;
      this.label3.Text = "Address2";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(11, 88);
      this.label2.Name = "label2";
      this.label2.Size = new Size(51, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "Address1";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(11, 63);
      this.label1.Name = "label1";
      this.label1.Size = new Size(63, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Bank Name";
      this.panel2.Controls.Add((Control) this.groupContainer1);
      this.panel2.Dock = DockStyle.Top;
      this.panel2.Location = new Point(5, 163);
      this.panel2.Margin = new Padding(5);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(833, 29);
      this.panel2.TabIndex = 6;
      this.groupContainer1.Controls.Add((Control) this.btReset);
      this.groupContainer1.Controls.Add((Control) this.btSave);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(833, 29);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "Warehouse Bank Details";
      this.btReset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btReset.BackColor = Color.Transparent;
      this.btReset.Location = new Point(808, 4);
      this.btReset.MouseDownImage = (Image) null;
      this.btReset.Name = "btReset";
      this.btReset.Size = new Size(16, 16);
      this.btReset.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.btReset.TabIndex = 33;
      this.btReset.TabStop = false;
      this.btReset.Click += new EventHandler(this.btReset_Click);
      this.btSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btSave.BackColor = Color.Transparent;
      this.btSave.Location = new Point(786, 4);
      this.btSave.MouseDownImage = (Image) null;
      this.btSave.Name = "btSave";
      this.btSave.Size = new Size(16, 16);
      this.btSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.btSave.TabIndex = 32;
      this.btSave.TabStop = false;
      this.btSave.Click += new EventHandler(this.btSave_Click);
      this.panelLicenseList.Controls.Add((Control) this.grpLicenseList);
      this.panelLicenseList.Dock = DockStyle.Top;
      this.panelLicenseList.Location = new Point(5, 5);
      this.panelLicenseList.Margin = new Padding(5);
      this.panelLicenseList.Name = "panelLicenseList";
      this.panelLicenseList.Size = new Size(833, 158);
      this.panelLicenseList.TabIndex = 2;
      this.grpLicenseList.Controls.Add((Control) this.chkUseParentInfo);
      this.grpLicenseList.Controls.Add((Control) this.btnDelete);
      this.grpLicenseList.Controls.Add((Control) this.btnAdd);
      this.grpLicenseList.Controls.Add((Control) this.gridView);
      this.grpLicenseList.Dock = DockStyle.Fill;
      this.grpLicenseList.HeaderForeColor = SystemColors.ControlText;
      this.grpLicenseList.Location = new Point(0, 0);
      this.grpLicenseList.Name = "grpLicenseList";
      this.grpLicenseList.Size = new Size(833, 158);
      this.grpLicenseList.TabIndex = 2;
      this.grpLicenseList.Text = "Warehouse Banks";
      this.chkUseParentInfo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkUseParentInfo.AutoSize = true;
      this.chkUseParentInfo.BackColor = Color.Transparent;
      this.chkUseParentInfo.Location = new Point(683, 6);
      this.chkUseParentInfo.Name = "chkUseParentInfo";
      this.chkUseParentInfo.Size = new Size(100, 17);
      this.chkUseParentInfo.TabIndex = 0;
      this.chkUseParentInfo.Text = "Use Parent Info";
      this.chkUseParentInfo.UseVisualStyleBackColor = false;
      this.chkUseParentInfo.CheckedChanged += new EventHandler(this.chkUseParentInfo_CheckedChanged);
      this.btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Location = new Point(808, 6);
      this.btnDelete.MouseDownImage = (Image) null;
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(16, 17);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabIndex = 37;
      this.btnDelete.TabStop = false;
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAdd.BackColor = Color.Transparent;
      this.btnAdd.Location = new Point(786, 6);
      this.btnAdd.MouseDownImage = (Image) null;
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(16, 17);
      this.btnAdd.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAdd.TabIndex = 36;
      this.btnAdd.TabStop = false;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.gridView.AllowMultiselect = false;
      this.gridView.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "ColumnName";
      gvColumn1.Text = "Bank Name";
      gvColumn1.Width = 150;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "ColumnCity";
      gvColumn2.Text = "City";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "ColumnState";
      gvColumn3.Text = "State";
      gvColumn3.Width = 50;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "ColumnZip";
      gvColumn4.Text = "Zip";
      gvColumn4.Width = 70;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "ColumnContactName";
      gvColumn5.Text = "Contact Name";
      gvColumn5.Width = 150;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "ColumnPhone";
      gvColumn6.Text = "Phone #";
      gvColumn6.Width = 150;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "ColumnABA";
      gvColumn7.Text = "ABA #";
      gvColumn7.Width = 100;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "ColumnDate";
      gvColumn8.Text = "Date Added";
      gvColumn8.Width = 100;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "ColumnApproved";
      gvColumn9.Text = "Approved";
      gvColumn9.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn9.Width = 100;
      this.gridView.Columns.AddRange(new GVColumn[9]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8,
        gvColumn9
      });
      this.gridView.Dock = DockStyle.Fill;
      this.gridView.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridView.Location = new Point(1, 26);
      this.gridView.Name = "gridView";
      this.gridView.Size = new Size(831, 131);
      this.gridView.TabIndex = 2;
      this.gridView.SelectedIndexChanged += new EventHandler(this.gridView1_SelectedIndexChanged);
      this.gridView.PreviewKeyDown += new PreviewKeyDownEventHandler(this.gridView_PreviewKeyDown);
      this.btnReset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnReset.BackColor = Color.Transparent;
      this.btnReset.Location = new Point(837, 4);
      this.btnReset.MouseDownImage = (Image) null;
      this.btnReset.Name = "btnReset";
      this.btnReset.Size = new Size(16, 16);
      this.btnReset.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.btnReset.TabIndex = 31;
      this.btnReset.TabStop = false;
      this.btnReset.Click += new EventHandler(this.btnReset_Click);
      this.btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSave.BackColor = Color.Transparent;
      this.btnSave.Location = new Point(815, 4);
      this.btnSave.MouseDownImage = (Image) null;
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(16, 16);
      this.btnSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.btnSave.TabIndex = 30;
      this.btnSave.TabStop = false;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.panelHeader.Controls.Add((Control) this.label33);
      this.panelHeader.Dock = DockStyle.Top;
      this.panelHeader.Location = new Point(1, 26);
      this.panelHeader.Name = "panelHeader";
      this.panelHeader.Size = new Size(860, 26);
      this.panelHeader.TabIndex = 0;
      this.label33.AutoSize = true;
      this.label33.Location = new Point(6, 6);
      this.label33.Name = "label33";
      this.label33.Size = new Size(278, 13);
      this.label33.TabIndex = 37;
      this.label33.Text = "Maintain the Warehouse banks associated with this TPO.";
      this.dpStatusDate.BackColor = SystemColors.Window;
      this.dpStatusDate.Location = new Point(199, 30);
      this.dpStatusDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpStatusDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpStatusDate.Name = "dpStatusDate";
      this.dpStatusDate.Size = new Size(107, 21);
      this.dpStatusDate.TabIndex = 10;
      this.dpStatusDate.Tag = (object) "";
      this.dpStatusDate.ToolTip = "";
      this.dpStatusDate.Value = new DateTime(0L);
      this.dpStatusDate.ValueChanged += new EventHandler(this.textField_Changed);
      this.label25.AutoSize = true;
      this.label25.Location = new Point(130, 35);
      this.label25.Name = "label25";
      this.label25.Size = new Size(63, 13);
      this.label25.TabIndex = 55;
      this.label25.Text = "Status Date";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.grpAll);
      this.Name = nameof (EditCompanyWarehouseControl);
      this.Size = new Size(862, 730);
      this.grpAll.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panelMarylandKansas.ResumeLayout(false);
      this.panel3.ResumeLayout(false);
      this.grpLicensingIssues.ResumeLayout(false);
      this.grpLicensingIssues.PerformLayout();
      this.grpKansas.ResumeLayout(false);
      this.grpKansas.PerformLayout();
      this.grpMaryland.ResumeLayout(false);
      this.grpMaryland.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.groupContainer1.ResumeLayout(false);
      ((ISupportInitialize) this.btReset).EndInit();
      ((ISupportInitialize) this.btSave).EndInit();
      this.panelLicenseList.ResumeLayout(false);
      this.grpLicenseList.ResumeLayout(false);
      this.grpLicenseList.PerformLayout();
      ((ISupportInitialize) this.btnDelete).EndInit();
      ((ISupportInitialize) this.btnAdd).EndInit();
      ((ISupportInitialize) this.btnReset).EndInit();
      ((ISupportInitialize) this.btnSave).EndInit();
      this.panelHeader.ResumeLayout(false);
      this.panelHeader.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
