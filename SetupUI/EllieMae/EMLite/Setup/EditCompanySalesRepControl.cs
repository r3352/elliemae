// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.EditCompanySalesRepControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.ExternalOriginatorManagement;
using EllieMae.EMLite.Setup.ExternalOriginatorManagement.RestApi;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class EditCompanySalesRepControl : UserControl
  {
    private Sessions.Session session;
    private IOrganizationManager rOrg;
    private UserInfo[] allInternalUsers;
    private Hashtable orgLookup;
    private int orgID = -1;
    private int companyOrgId = -1;
    private string currentOrg = "";
    private string primarySalesRepUserId;
    private bool isUsedByTPOInputHandler;
    private IContainer components;
    private GroupContainer groupContainer1;
    private StandardIconButton btnDelete;
    private StandardIconButton btnAdd;
    private GridView gvwSalesReps;
    private ComboBox cboViewMode;
    private GroupContainer groupContainer2;
    private Panel panelHeader;
    private Label label33;
    private Button btnSetPrimary;

    public EditCompanySalesRepControl(Sessions.Session session, int orgID, int companyOrgId)
    {
      this.InitializeComponent();
      this.RefreshCompanySalesRepControl(session, orgID, companyOrgId);
      this.Dock = DockStyle.Fill;
      this.gvwSalesReps.Dock = DockStyle.Fill;
      bool hierarchyAccess;
      Session.TpoHierarchyAccessCache.TryGetValue(orgID, out hierarchyAccess);
      TPOClientUtils.DisableControls((UserControl) this, hierarchyAccess);
    }

    public EditCompanySalesRepControl(SessionObjects session, int orgID, int companyOrgId)
    {
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      this.gvwSalesReps.Dock = DockStyle.Fill;
      bool hierarchyAccess;
      Session.TpoHierarchyAccessCache.TryGetValue(orgID, out hierarchyAccess);
      TPOClientUtils.DisableControls((UserControl) this, hierarchyAccess);
    }

    public void RefreshCompanySalesRepControl(
      Sessions.Session session,
      int orgID,
      int companyOrgId)
    {
      if (orgID == -1)
        return;
      this.session = session;
      this.orgID = orgID;
      this.companyOrgId = companyOrgId == -1 || companyOrgId == 0 ? orgID : companyOrgId;
      this.currentOrg = this.session.ConfigurationManager.GetOrgDetails(false, this.orgID).OrganizationName;
      this.loadOrgLookUp();
      this.loadAllInternalUsers();
      this.cboViewMode.SelectedIndex = 0;
      this.initForm(this.cboViewMode.SelectedIndex);
      this.gvwSalesReps.Visible = true;
    }

    private void initForm(int intViewMode)
    {
      Cursor.Current = Cursors.WaitCursor;
      List<ExternalOrgSalesRep> inner = (List<ExternalOrgSalesRep>) null;
      Hashtable hashtable = new Hashtable();
      this.gvwSalesReps.BeginUpdate();
      bool applicationRight = ((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.ExternalSettings_EditSalesReps);
      this.gvwSalesReps.Items.Clear();
      if (intViewMode == 0)
      {
        List<ExternalOrgSalesRep> repsForCurrentOrg = this.session.ConfigurationManager.GetExternalOrgSalesRepsForCurrentOrg(this.orgID);
        List<ExternalOrgSalesRep> externalOrgSalesRepList = new List<ExternalOrgSalesRep>();
        this.primarySalesRepUserId = this.session.ConfigurationManager.GetPrimarySalesRep(this.orgID);
        if (repsForCurrentOrg != null && repsForCurrentOrg.Count > 0)
        {
          foreach (ExternalOrgSalesRep externalOrgSalesRep in repsForCurrentOrg)
          {
            if (externalOrgSalesRep.externalOrgId == this.orgID)
            {
              if (inner == null)
                inner = new List<ExternalOrgSalesRep>();
              inner.Add(externalOrgSalesRep);
            }
            else
              externalOrgSalesRepList.Add(externalOrgSalesRep);
          }
          if (inner == null)
            inner = externalOrgSalesRepList;
          else if (externalOrgSalesRepList != null && externalOrgSalesRepList.Count > 0)
          {
            foreach (ExternalOrgSalesRep externalOrgSalesRep in externalOrgSalesRepList)
              inner.Add(externalOrgSalesRep);
          }
        }
      }
      else
        inner = this.session.ConfigurationManager.GetExternalOrgSalesRepsForCompany(this.companyOrgId);
      if (inner != null && inner.Count > 0)
      {
        foreach (ExternalOrgSalesRep externalOrgSalesRep in inner)
        {
          if (hashtable.ContainsKey((object) externalOrgSalesRep.userId.ToLower()))
          {
            if (externalOrgSalesRep.organizationName != "")
              hashtable[(object) externalOrgSalesRep.userId.ToLower()] = (object) (hashtable[(object) externalOrgSalesRep.userId.ToLower()].ToString() + " ; " + externalOrgSalesRep.organizationName);
          }
          else if (externalOrgSalesRep.organizationName != "")
            hashtable.Add((object) externalOrgSalesRep.userId.ToLower(), (object) externalOrgSalesRep.organizationName);
        }
        foreach (var data in ((IEnumerable<UserInfo>) this.allInternalUsers).Join((IEnumerable<ExternalOrgSalesRep>) inner, (Func<UserInfo, string>) (u => u.Userid.ToLower()), (Func<ExternalOrgSalesRep, string>) (salesRep => salesRep.userId.ToLower()), (u, salesRep) => new
        {
          salesRepId = salesRep.salesRepId,
          UserId = u.Userid,
          FullName = u.FullName,
          Title = salesRep.title,
          UserPersonas = u.UserPersonas,
          Phone = u.Phone,
          Email = u.Email,
          ExternalOrgID = salesRep.externalOrgId,
          CompanyLegalName = salesRep.companyLegalName,
          OrganizationName = salesRep.organizationName,
          isPrimarySalesRep = salesRep.isPrimarySalesRep,
          isWholesaleChannelEnabled = salesRep.isWholesaleChannelEnabled,
          isNonDelegatedChannelEnabled = salesRep.isNonDelegatedChannelEnabled,
          isDelegatedChannelEnabled = salesRep.isDelegatedChannelEnabled
        }).GroupBy(sr => new{ UserId = sr.UserId }).Select(s => s.FirstOrDefault()))
        {
          GVItem gvItem = new GVItem();
          if (this.cboViewMode.SelectedIndex == 0)
          {
            gvItem.SubItems.Add((object) "");
            gvItem.SubItems[0].Checked = data.isWholesaleChannelEnabled;
            gvItem.SubItems[0].CheckBoxEnabled = applicationRight;
            gvItem.SubItems.Add((object) "");
            gvItem.SubItems[1].Checked = data.isNonDelegatedChannelEnabled;
            gvItem.SubItems[1].CheckBoxEnabled = applicationRight;
            gvItem.SubItems.Add((object) "");
            gvItem.SubItems[2].Checked = data.isDelegatedChannelEnabled;
            gvItem.SubItems[2].CheckBoxEnabled = applicationRight;
          }
          gvItem.SubItems.Add((object) data.FullName);
          gvItem.SubItems.Add((object) Persona.ToString(data.UserPersonas));
          gvItem.SubItems.Add((object) data.Phone);
          gvItem.SubItems.Add((object) data.Email);
          gvItem.SubItems.Add(hashtable[(object) data.UserId.ToLower()]);
          if (intViewMode == 0)
          {
            if (data.UserId == this.primarySalesRepUserId)
              gvItem.SubItems.Add((object) "Primary");
            else
              gvItem.SubItems.Add((object) "");
          }
          else if (data.OrganizationName == "")
            gvItem.SubItems.Add((object) "Yes");
          else
            gvItem.SubItems.Add((object) "");
          ExternalOrgSalesRep externalOrgSalesRep = new ExternalOrgSalesRep(data.salesRepId, data.ExternalOrgID, data.UserId, data.CompanyLegalName, data.OrganizationName, data.FullName, data.Title, data.Phone, data.Email, data.isPrimarySalesRep, data.isWholesaleChannelEnabled, data.isDelegatedChannelEnabled, data.isNonDelegatedChannelEnabled);
          gvItem.Tag = (object) externalOrgSalesRep;
          this.gvwSalesReps.Items.Add(gvItem);
        }
      }
      this.gvwSalesReps.EndUpdate();
      this.btnDelete.Enabled = false;
      this.btnSetPrimary.Enabled = false;
      Cursor.Current = Cursors.Default;
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      this.loadAllInternalUsers();
      List<string> existingUserIDs = new List<string>();
      for (int nItemIndex = 0; nItemIndex < this.gvwSalesReps.Items.Count; ++nItemIndex)
      {
        string userId = ((ExternalOrgSalesRep) this.gvwSalesReps.Items[nItemIndex].Tag).userId;
        existingUserIDs.Add(userId);
      }
      Cursor.Current = Cursors.Default;
      using (AddSalesRepForm addSalesRepForm = new AddSalesRepForm(this.allInternalUsers, existingUserIDs))
      {
        if (addSalesRepForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.gvwSalesReps.BeginUpdate();
        this.gvwSalesReps.SelectedItems.Clear();
        ExternalOrgSalesRep[] newReps = new ExternalOrgSalesRep[addSalesRepForm.SelectedUsers.Length];
        for (int index = 0; index < addSalesRepForm.SelectedUsers.Length; ++index)
          newReps[index] = new ExternalOrgSalesRep(0, this.orgID, addSalesRepForm.SelectedUsers[index].Userid, "", "", "", "", "", "");
        try
        {
          if (!this.session.ConfigurationManager.AddExternalOrganizationSalesReps(newReps))
            return;
          WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.Parent.Text, this.orgID);
          this.initForm(this.cboViewMode.SelectedIndex);
        }
        catch (Exception ex)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, ex.Message);
        }
        finally
        {
          this.gvwSalesReps.EndUpdate();
        }
      }
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (this.gvwSalesReps.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select sales rep which you would like to delete.");
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "Are you sure you want to delete the selected sales rep?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
          return;
        string[] strArray = new string[this.gvwSalesReps.SelectedItems.Count];
        GVItem[] gvItemArray = new GVItem[this.gvwSalesReps.SelectedItems.Count];
        int index1 = this.gvwSalesReps.SelectedItems[0].Index;
        string userId = (string) null;
        for (int index2 = 0; index2 < this.gvwSalesReps.SelectedItems.Count; ++index2)
        {
          userId = ((ExternalOrgSalesRep) this.gvwSalesReps.SelectedItems[index2].Tag).userId;
          strArray[index2] = userId;
          gvItemArray[index2] = this.gvwSalesReps.SelectedItems[index2];
        }
        try
        {
          if (((IEnumerable<string>) strArray).ToList<string>().Contains(Session.UserID) && Utils.Dialog((IWin32Window) this, "Are you sure you want to delete yourself as a Sales Rep? You will loose access to this Organization later.", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            return;
          if (this.session.ConfigurationManager.CheckIfSalesRepIsPrimary(userId, this.orgID))
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "Selected sales rep has been listed as primary for sub organizations. Please change primary sales rep for sub organizations before deleting sales rep.");
          }
          else
          {
            if (!this.session.ConfigurationManager.DeleteExternalOrganizationSalesReps(this.orgID, strArray))
              return;
            for (int index3 = 0; index3 < gvItemArray.Length; ++index3)
              this.gvwSalesReps.Items.Remove(gvItemArray[index3]);
            WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.Parent.Text, this.orgID);
            this.initForm(this.cboViewMode.SelectedIndex);
          }
        }
        catch (Exception ex)
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, ex.Message);
        }
        finally
        {
          this.gvwSalesReps.EndUpdate();
        }
      }
    }

    private void cboViewMode_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.gvwSalesReps.Items.Clear();
      this.gvwSalesReps.SelectedItems.Clear();
      switch (this.cboViewMode.SelectedIndex)
      {
        case 0:
          if (!this.isUsedByTPOInputHandler)
            this.btnAdd.Visible = this.btnDelete.Visible = this.btnSetPrimary.Visible = true;
          if (!this.gvwSalesReps.Columns[0].Name.Equals("Whlse"))
          {
            this.gvwSalesReps.Columns.Insert(0, new GVColumn()
            {
              Name = "Whlse",
              Text = "Wholesale",
              CheckBoxes = true
            });
            this.gvwSalesReps.Columns.Insert(1, new GVColumn()
            {
              Name = "Non-Del",
              Text = "Non-Delegated",
              CheckBoxes = true
            });
            this.gvwSalesReps.Columns.Insert(2, new GVColumn()
            {
              Name = "Del",
              Text = "Delegated",
              CheckBoxes = true
            });
            this.gvwSalesReps.Columns[0].Width = 100;
            this.gvwSalesReps.Columns[1].Width = 100;
            this.gvwSalesReps.Columns[2].Width = 100;
          }
          this.gvwSalesReps.Columns[7].Width = 150;
          this.gvwSalesReps.Columns[8].Width = 150;
          this.gvwSalesReps.Columns[8].Text = "Sales Rep";
          break;
        case 1:
          for (int index = 0; index <= 2; ++index)
          {
            if (this.gvwSalesReps.Columns[0].Name.Equals("Whlse") || this.gvwSalesReps.Columns[0].Name.Equals("Non-Del") || this.gvwSalesReps.Columns[0].Name.Equals("Del"))
              this.gvwSalesReps.Columns.RemoveAt(0);
          }
          this.btnAdd.Visible = this.btnDelete.Visible = this.btnSetPrimary.Visible = false;
          this.gvwSalesReps.Columns[4].Width = 150;
          this.gvwSalesReps.Columns[5].Width = 150;
          this.gvwSalesReps.Columns[5].Text = "Specific Contacts";
          break;
      }
      this.initForm(this.cboViewMode.SelectedIndex);
    }

    private void loadAllInternalUsers()
    {
      this.loadOrgLookUp();
      if (this.allInternalUsers != null)
        return;
      this.allInternalUsers = this.rOrg.GetAllAccessibleSalesRepUsers();
    }

    private void loadOrgLookUp()
    {
      if (this.rOrg == null)
        this.rOrg = this.session.OrganizationManager;
      if (this.orgLookup != null)
        return;
      OrgInfo[] allOrganizations = this.rOrg.GetAllOrganizations();
      this.orgLookup = new Hashtable(allOrganizations.Length);
      for (int index = 0; index < allOrganizations.Length; ++index)
        this.orgLookup.Add((object) allOrganizations[index].Oid, (object) allOrganizations[index].OrgName);
    }

    private void gvwSalesReps_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnDelete.Enabled = this.btnSetPrimary.Enabled = this.gvwSalesReps.SelectedItems.Count == 1;
      if (this.gvwSalesReps.SelectedItems.Count == 0 || this.gvwSalesReps.SelectedItems.Count != 1)
        return;
      if (this.gvwSalesReps.SelectedItems[0].SubItems[4].ToString().Split(';').Length > 1 || ((ExternalOrgSalesRep) this.gvwSalesReps.SelectedItems[0].Tag).externalOrgId != this.orgID)
        this.btnDelete.Enabled = false;
      else
        this.btnDelete.Enabled = true;
      if (this.cboViewMode.SelectedIndex == 0 && this.gvwSalesReps.SelectedItems[0].SubItems[8].ToString() == "Primary")
      {
        this.btnSetPrimary.Enabled = false;
        this.btnDelete.Enabled = false;
      }
      else
        this.btnSetPrimary.Enabled = true;
    }

    private void btnSetPrimary_Click(object sender, EventArgs e)
    {
      if (this.gvwSalesReps.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select sales rep which you would like to set as Primary.");
      }
      else
      {
        this.gvwSalesReps.BeginUpdate();
        string userId = ((ExternalOrgSalesRep) this.gvwSalesReps.SelectedItems[0].Tag).userId;
        if (this.session.ConfigurationManager.CheckIfSalesRepHasAnyContacts(this.primarySalesRepUserId, this.orgID) && Utils.Dialog((IWin32Window) this, "Would you like to change all the contacts/sub organizations to use this sales rep?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button3) == DialogResult.Yes)
          this.session.ConfigurationManager.ChangeSalesRepForContacts(this.primarySalesRepUserId, userId, this.orgID, DateTime.Now);
        this.session.ConfigurationManager.SetSalesRepAsPrimary(userId, this.orgID, DateTime.Now);
        this.primarySalesRepUserId = userId;
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvwSalesReps.Items)
          gvItem.SubItems[8].Text = "";
        this.gvwSalesReps.SelectedItems[0].SubItems[8].Text = "Primary";
        this.btnDelete.Enabled = false;
        WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.Parent.Text, this.orgID);
        this.gvwSalesReps.EndUpdate();
      }
    }

    public void DisableControls()
    {
      this.isUsedByTPOInputHandler = true;
      this.btnSetPrimary.Visible = this.btnAdd.Visible = this.btnDelete.Visible = false;
      this.gvwSalesReps.Enabled = false;
      this.disableControl(this.Controls);
      this.cboViewMode.Enabled = true;
    }

    private void disableControl(Control.ControlCollection controls)
    {
      foreach (Control control in (ArrangedElementCollection) controls)
      {
        switch (control)
        {
          case TextBox _:
          case CheckBox _:
          case DatePicker _:
            control.Enabled = false;
            break;
        }
        if (control.Controls != null && control.Controls.Count > 0)
          this.disableControl(control.Controls);
      }
    }

    private void gvwSalesReps_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      if (this.cboViewMode.SelectedIndex != 0 || !(e.SubItem.Item.Tag is ExternalOrgSalesRep tag))
        return;
      switch (e.SubItem.Index)
      {
        case 0:
          tag.isWholesaleChannelEnabled = e.SubItem.Checked;
          break;
        case 1:
          tag.isNonDelegatedChannelEnabled = e.SubItem.Checked;
          break;
        case 2:
          tag.isDelegatedChannelEnabled = e.SubItem.Checked;
          break;
        default:
          return;
      }
      this.session.ConfigurationManager.UpdateExternalOrganizationSalesRep(tag);
      WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.Parent.Text, this.companyOrgId);
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
      this.groupContainer1 = new GroupContainer();
      this.btnSetPrimary = new Button();
      this.cboViewMode = new ComboBox();
      this.btnDelete = new StandardIconButton();
      this.btnAdd = new StandardIconButton();
      this.gvwSalesReps = new GridView();
      this.groupContainer2 = new GroupContainer();
      this.panelHeader = new Panel();
      this.label33 = new Label();
      this.groupContainer1.SuspendLayout();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      ((ISupportInitialize) this.btnAdd).BeginInit();
      this.groupContainer2.SuspendLayout();
      this.panelHeader.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer1.Borders = AnchorStyles.Top;
      this.groupContainer1.Controls.Add((Control) this.btnSetPrimary);
      this.groupContainer1.Controls.Add((Control) this.cboViewMode);
      this.groupContainer1.Controls.Add((Control) this.btnDelete);
      this.groupContainer1.Controls.Add((Control) this.btnAdd);
      this.groupContainer1.Controls.Add((Control) this.gvwSalesReps);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(1, 52);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(860, 557);
      this.groupContainer1.TabIndex = 10;
      this.groupContainer1.Text = "View";
      this.btnSetPrimary.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSetPrimary.Location = new Point(696, 2);
      this.btnSetPrimary.Name = "btnSetPrimary";
      this.btnSetPrimary.Size = new Size(106, 23);
      this.btnSetPrimary.TabIndex = 13;
      this.btnSetPrimary.Text = "&Set As Primary";
      this.btnSetPrimary.UseVisualStyleBackColor = true;
      this.btnSetPrimary.Click += new EventHandler(this.btnSetPrimary_Click);
      this.cboViewMode.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboViewMode.DropDownWidth = 175;
      this.cboViewMode.FormattingEnabled = true;
      this.cboViewMode.Items.AddRange(new object[2]
      {
        (object) "Current Org. TPO Sales Reps",
        (object) "Company TPO Sales Reps"
      });
      this.cboViewMode.Location = new Point(46, 2);
      this.cboViewMode.Name = "cboViewMode";
      this.cboViewMode.Size = new Size(176, 21);
      this.cboViewMode.TabIndex = 12;
      this.cboViewMode.SelectedIndexChanged += new EventHandler(this.cboViewMode_SelectedIndexChanged);
      this.btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Location = new Point(840, 4);
      this.btnDelete.MouseDownImage = (Image) null;
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(16, 16);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabIndex = 11;
      this.btnDelete.TabStop = false;
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAdd.BackColor = Color.Transparent;
      this.btnAdd.Location = new Point(818, 4);
      this.btnAdd.MouseDownImage = (Image) null;
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(16, 16);
      this.btnAdd.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAdd.TabIndex = 9;
      this.btnAdd.TabStop = false;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.gvwSalesReps.AllowMultiselect = false;
      this.gvwSalesReps.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "ColumnName";
      gvColumn1.Text = "Name";
      gvColumn1.Width = 150;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "ColumnPersonas";
      gvColumn2.Text = "Persona(s)";
      gvColumn2.Width = 180;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "ColumnPhone";
      gvColumn3.Text = "Phone #";
      gvColumn3.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "ColumnEmail";
      gvColumn4.Text = "Email";
      gvColumn4.Width = 130;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "ColumnOrg";
      gvColumn5.Text = "Org. Assignment";
      gvColumn5.Width = 200;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "ColumnSetAsPrimary";
      gvColumn6.Text = "Sales Rep";
      gvColumn6.Width = 84;
      this.gvwSalesReps.Columns.AddRange(new GVColumn[6]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6
      });
      this.gvwSalesReps.Dock = DockStyle.Top;
      this.gvwSalesReps.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvwSalesReps.Location = new Point(0, 26);
      this.gvwSalesReps.Name = "gvwSalesReps";
      this.gvwSalesReps.Size = new Size(860, 193);
      this.gvwSalesReps.TabIndex = 8;
      this.gvwSalesReps.SelectedIndexChanged += new EventHandler(this.gvwSalesReps_SelectedIndexChanged);
      this.gvwSalesReps.SubItemCheck += new GVSubItemEventHandler(this.gvwSalesReps_SubItemCheck);
      this.groupContainer2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer2.Controls.Add((Control) this.groupContainer1);
      this.groupContainer2.Controls.Add((Control) this.panelHeader);
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(5, 5);
      this.groupContainer2.Margin = new Padding(0);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(862, 610);
      this.groupContainer2.TabIndex = 11;
      this.groupContainer2.Text = "Sales Reps / Account Executives";
      this.panelHeader.Controls.Add((Control) this.label33);
      this.panelHeader.Dock = DockStyle.Top;
      this.panelHeader.Location = new Point(1, 26);
      this.panelHeader.Name = "panelHeader";
      this.panelHeader.Size = new Size(860, 26);
      this.panelHeader.TabIndex = 0;
      this.label33.AutoSize = true;
      this.label33.Location = new Point(6, 6);
      this.label33.Name = "label33";
      this.label33.Size = new Size(585, 13);
      this.label33.TabIndex = 36;
      this.label33.Text = "Add your company’s sales reps or account executives who manage this Third Party Originator company or branch account.";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupContainer2);
      this.Margin = new Padding(0);
      this.Name = nameof (EditCompanySalesRepControl);
      this.Padding = new Padding(5);
      this.Size = new Size(872, 620);
      this.groupContainer1.ResumeLayout(false);
      ((ISupportInitialize) this.btnDelete).EndInit();
      ((ISupportInitialize) this.btnAdd).EndInit();
      this.groupContainer2.ResumeLayout(false);
      this.panelHeader.ResumeLayout(false);
      this.panelHeader.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
