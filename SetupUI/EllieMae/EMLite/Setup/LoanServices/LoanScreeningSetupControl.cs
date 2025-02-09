// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LoanServices.LoanScreeningSetupControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.LoanServices;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.LoanServices
{
  public class LoanScreeningSetupControl : SettingsUserControl
  {
    private const string className = "LoanScreeningSetupControl";
    private static string sw = Tracing.SwOutsideLoan;
    private OfferSettings originalSettings;
    private OfferSettings offerSettings;
    private bool isLoading = true;
    private Persona selectedPersona;
    private UserInfo selectedUser;
    private IContainer components;
    private GradientPanel gradientPanel1;
    private Panel pnlTabs;
    private TabControl tabsOfferSettings;
    private TabPage tpPersonas;
    private Panel pnlCompany;
    private TabPage tpUsers;
    private CollapsibleSplitter collapsibleSplitter1;
    private GroupContainer grpPersonaOptOuts;
    private GroupContainer grpPersonas;
    private GradientPanel gradientPanel2;
    private GridView gvPersonaOptOuts;
    private GridView gvPersonas;
    private Label label1;
    private Label label2;
    private CheckBox chkPersonaOptIn;
    private GroupContainer grpUserOptOuts;
    private CheckBox chkUserOptIn;
    private GridView gvUserOptOuts;
    private GradientPanel gradientPanel3;
    private Label label3;
    private CollapsibleSplitter collapsibleSplitter2;
    private GroupContainer grpUsers;
    private GridView gvUsers;
    private ImageList ilIcons;
    private CheckBox chkCompanyOptIn;
    private BorderPanel pnlCompanyOptOutOld;
    private Label label4;
    private Panel pnlPersonaOptOut;
    private Label lblPersonaOptOut;
    private Panel pnlPersonaOptIn;
    private Panel pnlUserOptIn;
    private Panel pnlUserOptOut;
    private Label lblUserOptOut;
    private TabPage tpCompany;
    private GroupContainer grpCompanyOptOuts;
    private Panel pnlCompanyOptIn;
    private GridView gvCompanyOptOuts;
    private GradientPanel gradientPanel4;
    private Label label5;
    private Panel pnlCompanyOptOut;
    private Label label6;

    public LoanScreeningSetupControl(SetUpContainer setupContainer)
      : base(setupContainer)
    {
      this.InitializeComponent();
      this.originalSettings = LoanScreeningManager.GetOfferSettings();
      this.offerSettings = this.originalSettings;
      this.loadPersonas();
      this.loadUsers();
      this.loadOffers(this.gvCompanyOptOuts);
      this.loadOffers(this.gvPersonaOptOuts);
      this.loadOffers(this.gvUserOptOuts);
      this.Reset();
    }

    private void loadPersonas()
    {
      Persona[] allPersonas = Session.PersonaManager.GetAllPersonas();
      this.gvPersonas.Items.Clear();
      foreach (Persona persona in allPersonas)
        this.gvPersonas.Items.Add(new GVItem(persona.Name)
        {
          Tag = (object) persona
        });
    }

    private void loadUsers()
    {
      UserInfo[] allUsers = Session.OrganizationManager.GetAllUsers();
      this.gvUsers.Items.Clear();
      foreach (UserInfo userInfo in allUsers)
        this.gvUsers.Items.Add(new GVItem()
        {
          SubItems = {
            [1] = {
              Text = userInfo.LastName + ", " + userInfo.FirstName
            },
            [2] = {
              Text = this.getPersonaNames(userInfo.UserPersonas)
            }
          },
          Tag = (object) userInfo
        });
    }

    private void refreshUserOverrideIcons()
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvUsers.Items)
        this.refreshUserOverrideIcon(gvItem);
    }

    private void refreshUserOverrideIcon(GVItem item)
    {
      UserInfo tag = (UserInfo) item.Tag;
      if (this.offerSettings.OptOutSettings.ContainsUserOverride(this.offerSettings.Catalog, tag.Userid, tag.GetPersonaIDs()))
        item.SubItems[0].ImageIndex = 1;
      else
        item.SubItems[0].ImageIndex = -1;
    }

    private string getPersonaNames(Persona[] personas)
    {
      List<string> stringList = new List<string>();
      foreach (Persona persona in personas)
        stringList.Add(persona.Name);
      return string.Join(", ", stringList.ToArray());
    }

    private void loadOffers(GridView gridView)
    {
      foreach (OfferCategory category in this.offerSettings.Catalog.Categories)
      {
        GVItem gvItem1 = new GVItem(category.Name);
        gvItem1.SubItems[1].Text = category.Description;
        gvItem1.Tag = (object) category;
        gridView.Items.Add(gvItem1);
        foreach (OfferProvider offerProvider in this.offerSettings.Catalog.GetProvidersByCategoryID(category.ObjectID))
        {
          GVItem gvItem2 = new GVItem(offerProvider.Name);
          gvItem2.SubItems[1].Text = offerProvider.Description;
          gvItem2.Tag = (object) offerProvider;
          gvItem1.GroupItems.Add(gvItem2);
          foreach (Offer offer in this.offerSettings.Catalog.GetOffersByProviderID(offerProvider.ObjectID))
            gvItem2.GroupItems.Add(new GVItem(offer.Name)
            {
              SubItems = {
                [1] = {
                  Text = offer.Description
                }
              },
              Tag = (object) offer
            });
        }
      }
    }

    private void applyCompanyOptOutSetting(GVItem item)
    {
      bool flag1 = this.offerSettings.OptOutSettings.IsClientObjectEnablable(item.Tag as LoanScreeningObject);
      bool flag2 = true;
      if (flag1)
        flag2 = this.offerSettings.OptOutSettings.IsClientOptedOut(item.Tag as LoanScreeningObject);
      item.SubItems[0].CheckBoxEnabled = true;
      item.SubItems[0].Checked = flag1 && !flag2;
      item.SubItems[0].CheckBoxEnabled = flag1;
    }

    private void applyPersonaOptOutSetting(int personaId, GVItem item)
    {
      bool flag1 = this.offerSettings.OptOutSettings.IsPersonaObjectEnablable(personaId, item.Tag as LoanScreeningObject);
      bool flag2 = true;
      if (flag1)
        flag2 = this.offerSettings.OptOutSettings.IsPersonaOptedOut(personaId, item.Tag as LoanScreeningObject);
      item.SubItems[0].CheckBoxEnabled = true;
      item.SubItems[0].Checked = flag1 && !flag2;
      item.SubItems[0].CheckBoxEnabled = flag1;
    }

    private void applyUserOptOutSetting(string userId, int[] personaIds, GVItem item)
    {
      bool flag1 = this.offerSettings.OptOutSettings.IsUserObjectEnablable(userId, personaIds, item.Tag as LoanScreeningObject);
      bool flag2 = true;
      if (flag1)
        flag2 = this.offerSettings.OptOutSettings.IsUserOptedOut(userId, item.Tag as LoanScreeningObject);
      item.SubItems[0].CheckBoxEnabled = true;
      item.SubItems[0].Checked = flag1 && !flag2;
      item.SubItems[0].CheckBoxEnabled = flag1;
    }

    private void gvPersonas_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.gvPersonas.SelectedItems.Count == 0)
        this.clearPersonaOfferSettings();
      else
        this.loadPersonaOfferSettings(this.gvPersonas.SelectedItems[0].Tag as Persona);
    }

    private void clearPersonaOfferSettings()
    {
      this.selectedPersona = (Persona) null;
      this.clearOfferSettings(this.gvPersonaOptOuts.Items);
      this.chkPersonaOptIn.Checked = false;
      this.chkPersonaOptIn.Enabled = false;
      this.setPersonaLayout();
    }

    private void clearOfferSettings(GVItemCollection items)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) items)
      {
        gvItem.SubItems[0].Checked = false;
        gvItem.SubItems[0].CheckBoxEnabled = false;
        if (gvItem.GroupItems.Count > 0)
          this.clearOfferSettings(gvItem.GroupItems);
      }
    }

    private void loadCompanyOfferSettings()
    {
      this.chkCompanyOptIn.Checked = !this.offerSettings.OptOutSettings.IsClientOptedOutOfFeature();
      this.loadCompanyOfferSettings(this.gvCompanyOptOuts.Items);
      this.setCompanyLayout();
    }

    private void loadPersonaOfferSettings(Persona p)
    {
      this.selectedPersona = (Persona) null;
      this.chkPersonaOptIn.Enabled = !this.offerSettings.OptOutSettings.IsClientOptedOutOfFeature();
      this.chkPersonaOptIn.Checked = this.chkPersonaOptIn.Enabled && !this.offerSettings.OptOutSettings.IsPersonaOptedOutOfFeature(p.ID);
      this.loadPersonaOfferSettings(p, this.gvPersonaOptOuts.Items);
      this.setPersonaLayout();
      this.selectedPersona = p;
    }

    private void loadCompanyOfferSettings(GVItemCollection items)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) items)
      {
        this.applyCompanyOptOutSetting(gvItem);
        if (gvItem.GroupItems.Count > 0)
          this.loadCompanyOfferSettings(gvItem.GroupItems);
      }
    }

    private void loadPersonaOfferSettings(Persona p, GVItemCollection items)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) items)
      {
        this.applyPersonaOptOutSetting(p.ID, gvItem);
        if (gvItem.GroupItems.Count > 0)
          this.loadPersonaOfferSettings(p, gvItem.GroupItems);
      }
    }

    private void gvCompanyOptOuts_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      if (this.isLoading)
        return;
      LoanScreeningOptOut clientOptOutElement = ((LoanScreeningObject) e.SubItem.Item.Tag).ToClientOptOutElement();
      if (e.SubItem.Checked)
        this.offerSettings.OptOutSettings.Remove(clientOptOutElement);
      else
        this.offerSettings.OptOutSettings.Add(clientOptOutElement);
      this.loadCompanyOfferSettings();
      this.setDirtyFlag(true);
    }

    private void gvPersonaOptOuts_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      if (this.selectedPersona == (Persona) null)
        return;
      LoanScreeningOptOut personaOptOutElement = ((LoanScreeningObject) e.SubItem.Item.Tag).ToPersonaOptOutElement(this.selectedPersona.ID);
      if (e.SubItem.Checked)
        this.offerSettings.OptOutSettings.Remove(personaOptOutElement);
      else
        this.offerSettings.OptOutSettings.Add(personaOptOutElement);
      this.loadPersonaOfferSettings(this.selectedPersona);
      this.setDirtyFlag(true);
    }

    private void chkPersonaOptIn_CheckedChanged(object sender, EventArgs e)
    {
      if (!(this.selectedPersona != (Persona) null))
        return;
      if (!this.isLoading && !this.chkPersonaOptIn.Checked && Utils.Dialog((IWin32Window) this, "You are about to disable all e360Select programs for the " + this.selectedPersona.Name + " persona. Would you like to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
      {
        this.chkPersonaOptIn.Checked = true;
      }
      else
      {
        LoanScreeningOptOut loanScreeningOptOut = new LoanScreeningOptOut(this.selectedPersona.ID, (string) null, LoanScreeningObjectType.Feature);
        bool flag = this.offerSettings.OptOutSettings.IsPersonaOptedOutOfFeature(this.selectedPersona.ID);
        if (this.chkPersonaOptIn.Checked & flag)
        {
          this.offerSettings.OptOutSettings.Remove(loanScreeningOptOut);
        }
        else
        {
          if (this.chkPersonaOptIn.Checked || flag)
            return;
          this.offerSettings.OptOutSettings.Add(loanScreeningOptOut);
        }
        this.loadPersonaOfferSettings(this.selectedPersona);
        this.setDirtyFlag(true);
      }
    }

    private void setCompanyLayout()
    {
      if (!this.chkCompanyOptIn.Checked)
      {
        this.pnlCompanyOptIn.Visible = false;
        this.pnlCompanyOptOut.Visible = true;
        this.pnlCompanyOptOut.Dock = DockStyle.Fill;
      }
      else
      {
        this.pnlCompanyOptOut.Visible = false;
        this.pnlCompanyOptIn.Visible = true;
        this.pnlCompanyOptIn.Dock = DockStyle.Fill;
      }
    }

    private void setPersonaLayout()
    {
      if (this.chkPersonaOptIn.Enabled && !this.chkPersonaOptIn.Checked)
      {
        this.pnlPersonaOptIn.Visible = false;
        this.pnlPersonaOptOut.Visible = true;
        this.pnlPersonaOptOut.Dock = DockStyle.Fill;
      }
      else
      {
        this.pnlPersonaOptOut.Visible = false;
        this.pnlPersonaOptIn.Visible = true;
        this.pnlPersonaOptIn.Dock = DockStyle.Fill;
      }
    }

    private void gvUsers_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.gvUsers.SelectedItems.Count == 0)
        this.clearUserOfferSettings();
      else
        this.loadUserOfferSettings(this.gvUsers.SelectedItems[0].Tag as UserInfo);
    }

    private void clearUserOfferSettings()
    {
      this.selectedUser = (UserInfo) null;
      this.clearOfferSettings(this.gvUserOptOuts.Items);
      this.chkUserOptIn.Checked = false;
      this.chkUserOptIn.Enabled = false;
      this.setUserLayout();
    }

    private void loadUserOfferSettings(UserInfo user)
    {
      this.selectedUser = (UserInfo) null;
      this.chkUserOptIn.Enabled = !this.offerSettings.OptOutSettings.IsClientOptedOutOfFeature();
      this.chkUserOptIn.Checked = this.chkUserOptIn.Enabled && !this.offerSettings.OptOutSettings.IsUserOptedOutOfFeature(user.Userid);
      this.loadUserOfferSettings(user.Userid, user.GetPersonaIDs(), this.gvUserOptOuts.Items);
      this.setUserLayout();
      this.selectedUser = user;
    }

    private void loadUserOfferSettings(string userId, int[] personaIds, GVItemCollection items)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) items)
      {
        this.applyUserOptOutSetting(userId, personaIds, gvItem);
        if (gvItem.GroupItems.Count > 0)
          this.loadUserOfferSettings(userId, personaIds, gvItem.GroupItems);
      }
    }

    private void gvUserOptOuts_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      if (this.selectedUser == (UserInfo) null)
        return;
      LoanScreeningOptOut userOptOutElement = ((LoanScreeningObject) e.SubItem.Item.Tag).ToUserOptOutElement(this.selectedUser.Userid);
      if (e.SubItem.Checked)
        this.offerSettings.OptOutSettings.Remove(userOptOutElement);
      else
        this.offerSettings.OptOutSettings.Add(userOptOutElement);
      this.loadUserOfferSettings(this.selectedUser);
      this.refreshUserOverrideIcon(this.gvUsers.SelectedItems[0]);
      this.setDirtyFlag(true);
    }

    private void chkUserOptIn_CheckedChanged(object sender, EventArgs e)
    {
      if (!(this.selectedUser != (UserInfo) null))
        return;
      if (!this.isLoading && !this.chkUserOptIn.Checked && Utils.Dialog((IWin32Window) this, "You are about to disable all e360Select programs for " + this.selectedUser.FullName + ". Would you like to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
      {
        this.chkUserOptIn.Checked = true;
      }
      else
      {
        LoanScreeningOptOut loanScreeningOptOut = new LoanScreeningOptOut(this.selectedUser.Userid, (string) null, LoanScreeningObjectType.Feature);
        bool flag = this.offerSettings.OptOutSettings.IsUserOptedOutOfFeature(this.selectedUser.Userid);
        if (this.chkUserOptIn.Checked & flag)
        {
          this.offerSettings.OptOutSettings.Remove(loanScreeningOptOut);
        }
        else
        {
          if (this.chkUserOptIn.Checked || flag)
            return;
          this.offerSettings.OptOutSettings.Add(loanScreeningOptOut);
        }
        this.loadUserOfferSettings(this.selectedUser);
        this.refreshUserOverrideIcon(this.gvUsers.SelectedItems[0]);
        this.setDirtyFlag(true);
      }
    }

    private void setUserLayout()
    {
      if (this.chkUserOptIn.Enabled && !this.chkUserOptIn.Checked)
      {
        this.pnlUserOptIn.Visible = false;
        this.pnlUserOptOut.Visible = true;
        this.pnlUserOptOut.Dock = DockStyle.Fill;
      }
      else
      {
        this.pnlUserOptOut.Visible = false;
        this.pnlUserOptIn.Visible = true;
        this.pnlUserOptIn.Dock = DockStyle.Fill;
      }
    }

    private void tabsOfferSettings_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.tabsOfferSettings.SelectedTab == this.tpPersonas && this.selectedPersona != (Persona) null)
      {
        this.loadPersonaOfferSettings(this.selectedPersona);
      }
      else
      {
        if (this.tabsOfferSettings.SelectedTab != this.tpUsers || !(this.selectedUser != (UserInfo) null))
          return;
        this.loadUserOfferSettings(this.selectedUser);
        this.refreshUserOverrideIcons();
      }
    }

    public override void Reset()
    {
      this.offerSettings = (OfferSettings) this.originalSettings.Clone();
      this.isLoading = true;
      this.loadCompanyOfferSettings();
      if (this.selectedPersona != (Persona) null)
        this.loadPersonaOfferSettings(this.selectedPersona);
      if (this.selectedUser != (UserInfo) null)
        this.loadUserOfferSettings(this.selectedUser);
      this.refreshUserOverrideIcons();
      if (this.selectedPersona == (Persona) null)
        this.gvPersonas.Items[0].Selected = true;
      if (this.selectedUser == (UserInfo) null)
        this.gvUsers.Items[0].Selected = true;
      base.Reset();
      this.isLoading = false;
    }

    public override void Save()
    {
      try
      {
        LoanScreeningManager.SaveOfferSettings(this.offerSettings);
        base.Save();
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanScreeningSetupControl.sw, nameof (LoanScreeningSetupControl), TraceLevel.Error, "Error saving opt-out settings: " + (object) ex);
        int num = (int) Utils.Dialog((IWin32Window) this, "An error occurred while attempting to save your settings: " + ex.Message + ". Your changes have not beed saved.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void chkCompanyOptIn_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.isLoading && !this.chkCompanyOptIn.Checked && Utils.Dialog((IWin32Window) this, "You are about to disable all e360Select programs for your company. Are you sure you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
      {
        this.chkCompanyOptIn.Checked = true;
      }
      else
      {
        bool flag = this.offerSettings.OptOutSettings.IsClientOptedOutOfFeature();
        if (this.chkCompanyOptIn.Checked & flag)
        {
          this.offerSettings.OptOutSettings.Remove(new LoanScreeningOptOut((string) null, LoanScreeningObjectType.Feature));
        }
        else
        {
          if (this.chkCompanyOptIn.Checked || flag)
            return;
          this.offerSettings.OptOutSettings.Add(new LoanScreeningOptOut((string) null, LoanScreeningObjectType.Feature));
        }
        this.isLoading = true;
        this.setCompanyLayout();
        this.loadCompanyOfferSettings();
        if (this.selectedPersona != (Persona) null)
          this.loadPersonaOfferSettings(this.selectedPersona);
        if (this.selectedUser != (UserInfo) null)
          this.loadUserOfferSettings(this.selectedUser);
        this.setDirtyFlag(true);
        this.isLoading = false;
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
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      GVColumn gvColumn8 = new GVColumn();
      GVColumn gvColumn9 = new GVColumn();
      GVColumn gvColumn10 = new GVColumn();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (LoanScreeningSetupControl));
      this.pnlTabs = new Panel();
      this.tabsOfferSettings = new TabControl();
      this.tpCompany = new TabPage();
      this.grpCompanyOptOuts = new GroupContainer();
      this.pnlCompanyOptOut = new Panel();
      this.label6 = new Label();
      this.chkCompanyOptIn = new CheckBox();
      this.pnlCompanyOptIn = new Panel();
      this.gvCompanyOptOuts = new GridView();
      this.gradientPanel4 = new GradientPanel();
      this.label5 = new Label();
      this.tpPersonas = new TabPage();
      this.pnlCompany = new Panel();
      this.grpPersonaOptOuts = new GroupContainer();
      this.pnlPersonaOptIn = new Panel();
      this.gvPersonaOptOuts = new GridView();
      this.gradientPanel2 = new GradientPanel();
      this.label1 = new Label();
      this.chkPersonaOptIn = new CheckBox();
      this.pnlPersonaOptOut = new Panel();
      this.lblPersonaOptOut = new Label();
      this.collapsibleSplitter1 = new CollapsibleSplitter();
      this.grpPersonas = new GroupContainer();
      this.gvPersonas = new GridView();
      this.tpUsers = new TabPage();
      this.grpUserOptOuts = new GroupContainer();
      this.pnlUserOptOut = new Panel();
      this.lblUserOptOut = new Label();
      this.pnlUserOptIn = new Panel();
      this.gvUserOptOuts = new GridView();
      this.gradientPanel3 = new GradientPanel();
      this.label3 = new Label();
      this.chkUserOptIn = new CheckBox();
      this.collapsibleSplitter2 = new CollapsibleSplitter();
      this.grpUsers = new GroupContainer();
      this.gvUsers = new GridView();
      this.ilIcons = new ImageList(this.components);
      this.pnlCompanyOptOutOld = new BorderPanel();
      this.label4 = new Label();
      this.gradientPanel1 = new GradientPanel();
      this.label2 = new Label();
      this.pnlTabs.SuspendLayout();
      this.tabsOfferSettings.SuspendLayout();
      this.tpCompany.SuspendLayout();
      this.grpCompanyOptOuts.SuspendLayout();
      this.pnlCompanyOptOut.SuspendLayout();
      this.pnlCompanyOptIn.SuspendLayout();
      this.gradientPanel4.SuspendLayout();
      this.tpPersonas.SuspendLayout();
      this.pnlCompany.SuspendLayout();
      this.grpPersonaOptOuts.SuspendLayout();
      this.pnlPersonaOptIn.SuspendLayout();
      this.gradientPanel2.SuspendLayout();
      this.pnlPersonaOptOut.SuspendLayout();
      this.grpPersonas.SuspendLayout();
      this.tpUsers.SuspendLayout();
      this.grpUserOptOuts.SuspendLayout();
      this.pnlUserOptOut.SuspendLayout();
      this.pnlUserOptIn.SuspendLayout();
      this.gradientPanel3.SuspendLayout();
      this.grpUsers.SuspendLayout();
      this.pnlCompanyOptOutOld.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      this.SuspendLayout();
      this.pnlTabs.Controls.Add((Control) this.tabsOfferSettings);
      this.pnlTabs.Dock = DockStyle.Fill;
      this.pnlTabs.Location = new Point(0, 26);
      this.pnlTabs.Name = "pnlTabs";
      this.pnlTabs.Padding = new Padding(2, 2, 0, 0);
      this.pnlTabs.Size = new Size(741, 548);
      this.pnlTabs.TabIndex = 1;
      this.tabsOfferSettings.Controls.Add((Control) this.tpCompany);
      this.tabsOfferSettings.Controls.Add((Control) this.tpPersonas);
      this.tabsOfferSettings.Controls.Add((Control) this.tpUsers);
      this.tabsOfferSettings.Dock = DockStyle.Fill;
      this.tabsOfferSettings.Location = new Point(2, 2);
      this.tabsOfferSettings.Name = "tabsOfferSettings";
      this.tabsOfferSettings.SelectedIndex = 0;
      this.tabsOfferSettings.Size = new Size(739, 546);
      this.tabsOfferSettings.TabIndex = 0;
      this.tabsOfferSettings.SelectedIndexChanged += new EventHandler(this.tabsOfferSettings_SelectedIndexChanged);
      this.tpCompany.Controls.Add((Control) this.grpCompanyOptOuts);
      this.tpCompany.Location = new Point(4, 23);
      this.tpCompany.Margin = new Padding(1, 3, 3, 3);
      this.tpCompany.Name = "tpCompany";
      this.tpCompany.Size = new Size(731, 519);
      this.tpCompany.TabIndex = 2;
      this.tpCompany.Text = "Company";
      this.tpCompany.UseVisualStyleBackColor = true;
      this.grpCompanyOptOuts.Controls.Add((Control) this.chkCompanyOptIn);
      this.grpCompanyOptOuts.Controls.Add((Control) this.pnlCompanyOptIn);
      this.grpCompanyOptOuts.Controls.Add((Control) this.pnlCompanyOptOut);
      this.grpCompanyOptOuts.Dock = DockStyle.Fill;
      this.grpCompanyOptOuts.HeaderForeColor = SystemColors.ControlText;
      this.grpCompanyOptOuts.Location = new Point(0, 0);
      this.grpCompanyOptOuts.Name = "grpCompanyOptOuts";
      this.grpCompanyOptOuts.Size = new Size(731, 519);
      this.grpCompanyOptOuts.TabIndex = 2;
      this.grpCompanyOptOuts.Text = "Company Opt-In Settings";
      this.pnlCompanyOptOut.Controls.Add((Control) this.label6);
      this.pnlCompanyOptOut.Location = new Point(50, 176);
      this.pnlCompanyOptOut.Name = "pnlCompanyOptOut";
      this.pnlCompanyOptOut.Size = new Size(252, 136);
      this.pnlCompanyOptOut.TabIndex = 4;
      this.label6.Anchor = AnchorStyles.None;
      this.label6.Location = new Point(-75, 53);
      this.label6.Name = "label6";
      this.label6.Size = new Size(402, 31);
      this.label6.TabIndex = 0;
      this.label6.Text = "Your company is not participating in the e360Select program. To activate this program, select the Company Opt In box above.";
      this.label6.TextAlign = ContentAlignment.TopCenter;
      this.chkCompanyOptIn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkCompanyOptIn.AutoSize = true;
      this.chkCompanyOptIn.BackColor = Color.Transparent;
      this.chkCompanyOptIn.Location = new Point(626, 4);
      this.chkCompanyOptIn.Name = "chkCompanyOptIn";
      this.chkCompanyOptIn.Size = new Size(102, 18);
      this.chkCompanyOptIn.TabIndex = 1;
      this.chkCompanyOptIn.Text = "Company Opt In";
      this.chkCompanyOptIn.UseVisualStyleBackColor = false;
      this.chkCompanyOptIn.CheckedChanged += new EventHandler(this.chkCompanyOptIn_CheckedChanged);
      this.pnlCompanyOptIn.Controls.Add((Control) this.gvCompanyOptOuts);
      this.pnlCompanyOptIn.Controls.Add((Control) this.gradientPanel4);
      this.pnlCompanyOptIn.Dock = DockStyle.Fill;
      this.pnlCompanyOptIn.Location = new Point(1, 26);
      this.pnlCompanyOptIn.Name = "pnlCompanyOptIn";
      this.pnlCompanyOptIn.Size = new Size(729, 492);
      this.pnlCompanyOptIn.TabIndex = 3;
      this.gvCompanyOptOuts.BorderStyle = BorderStyle.None;
      gvColumn1.CheckBoxes = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Name";
      gvColumn1.Width = 200;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Description";
      gvColumn2.Width = 350;
      this.gvCompanyOptOuts.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvCompanyOptOuts.Dock = DockStyle.Fill;
      this.gvCompanyOptOuts.ItemGrouping = true;
      this.gvCompanyOptOuts.Location = new Point(0, 25);
      this.gvCompanyOptOuts.Name = "gvCompanyOptOuts";
      this.gvCompanyOptOuts.Size = new Size(729, 467);
      this.gvCompanyOptOuts.TabIndex = 0;
      this.gvCompanyOptOuts.SubItemCheck += new GVSubItemEventHandler(this.gvCompanyOptOuts_SubItemCheck);
      this.gradientPanel4.Borders = AnchorStyles.Bottom;
      this.gradientPanel4.Controls.Add((Control) this.label5);
      this.gradientPanel4.Dock = DockStyle.Top;
      this.gradientPanel4.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel4.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel4.Location = new Point(0, 0);
      this.gradientPanel4.Name = "gradientPanel4";
      this.gradientPanel4.Size = new Size(729, 25);
      this.gradientPanel4.Style = GradientPanel.PanelStyle.TableFooter;
      this.gradientPanel4.TabIndex = 1;
      this.label5.AutoSize = true;
      this.label5.BackColor = Color.Transparent;
      this.label5.Location = new Point(6, 6);
      this.label5.Name = "label5";
      this.label5.Size = new Size(460, 14);
      this.label5.TabIndex = 0;
      this.label5.Text = "Select categories, providers, or programs to make programs visible throughout your company.";
      this.tpPersonas.Controls.Add((Control) this.pnlCompany);
      this.tpPersonas.Location = new Point(4, 23);
      this.tpPersonas.Name = "tpPersonas";
      this.tpPersonas.Padding = new Padding(1, 3, 3, 3);
      this.tpPersonas.Size = new Size(731, 519);
      this.tpPersonas.TabIndex = 0;
      this.tpPersonas.Text = "Personas";
      this.tpPersonas.UseVisualStyleBackColor = true;
      this.pnlCompany.Controls.Add((Control) this.grpPersonaOptOuts);
      this.pnlCompany.Controls.Add((Control) this.collapsibleSplitter1);
      this.pnlCompany.Controls.Add((Control) this.grpPersonas);
      this.pnlCompany.Dock = DockStyle.Fill;
      this.pnlCompany.Location = new Point(1, 3);
      this.pnlCompany.Name = "pnlCompany";
      this.pnlCompany.Size = new Size(727, 513);
      this.pnlCompany.TabIndex = 0;
      this.grpPersonaOptOuts.Controls.Add((Control) this.pnlPersonaOptIn);
      this.grpPersonaOptOuts.Controls.Add((Control) this.chkPersonaOptIn);
      this.grpPersonaOptOuts.Controls.Add((Control) this.pnlPersonaOptOut);
      this.grpPersonaOptOuts.Dock = DockStyle.Fill;
      this.grpPersonaOptOuts.HeaderForeColor = SystemColors.ControlText;
      this.grpPersonaOptOuts.Location = new Point(207, 0);
      this.grpPersonaOptOuts.Name = "grpPersonaOptOuts";
      this.grpPersonaOptOuts.Size = new Size(520, 513);
      this.grpPersonaOptOuts.TabIndex = 1;
      this.grpPersonaOptOuts.Text = "Persona Opt-In Settings";
      this.pnlPersonaOptIn.Controls.Add((Control) this.gvPersonaOptOuts);
      this.pnlPersonaOptIn.Controls.Add((Control) this.gradientPanel2);
      this.pnlPersonaOptIn.Dock = DockStyle.Fill;
      this.pnlPersonaOptIn.Location = new Point(1, 26);
      this.pnlPersonaOptIn.Name = "pnlPersonaOptIn";
      this.pnlPersonaOptIn.Size = new Size(518, 486);
      this.pnlPersonaOptIn.TabIndex = 3;
      this.gvPersonaOptOuts.BorderStyle = BorderStyle.None;
      gvColumn3.CheckBoxes = true;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column1";
      gvColumn3.Text = "Name";
      gvColumn3.Width = 200;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column2";
      gvColumn4.Text = "Description";
      gvColumn4.Width = 350;
      this.gvPersonaOptOuts.Columns.AddRange(new GVColumn[2]
      {
        gvColumn3,
        gvColumn4
      });
      this.gvPersonaOptOuts.Dock = DockStyle.Fill;
      this.gvPersonaOptOuts.ItemGrouping = true;
      this.gvPersonaOptOuts.Location = new Point(0, 25);
      this.gvPersonaOptOuts.Name = "gvPersonaOptOuts";
      this.gvPersonaOptOuts.Size = new Size(518, 461);
      this.gvPersonaOptOuts.TabIndex = 0;
      this.gvPersonaOptOuts.SubItemCheck += new GVSubItemEventHandler(this.gvPersonaOptOuts_SubItemCheck);
      this.gradientPanel2.Borders = AnchorStyles.Bottom;
      this.gradientPanel2.Controls.Add((Control) this.label1);
      this.gradientPanel2.Dock = DockStyle.Top;
      this.gradientPanel2.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel2.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel2.Location = new Point(0, 0);
      this.gradientPanel2.Name = "gradientPanel2";
      this.gradientPanel2.Size = new Size(518, 25);
      this.gradientPanel2.Style = GradientPanel.PanelStyle.TableFooter;
      this.gradientPanel2.TabIndex = 1;
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Location = new Point(6, 6);
      this.label1.Name = "label1";
      this.label1.Size = new Size(415, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "Select categories, providers, or programs to make programs visible to each persona.";
      this.chkPersonaOptIn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkPersonaOptIn.AutoSize = true;
      this.chkPersonaOptIn.BackColor = Color.Transparent;
      this.chkPersonaOptIn.Location = new Point(419, 4);
      this.chkPersonaOptIn.Name = "chkPersonaOptIn";
      this.chkPersonaOptIn.Size = new Size(97, 18);
      this.chkPersonaOptIn.TabIndex = 2;
      this.chkPersonaOptIn.Text = "Persona Opt In";
      this.chkPersonaOptIn.UseVisualStyleBackColor = false;
      this.chkPersonaOptIn.CheckedChanged += new EventHandler(this.chkPersonaOptIn_CheckedChanged);
      this.pnlPersonaOptOut.Controls.Add((Control) this.lblPersonaOptOut);
      this.pnlPersonaOptOut.Location = new Point(50, 176);
      this.pnlPersonaOptOut.Name = "pnlPersonaOptOut";
      this.pnlPersonaOptOut.Size = new Size(252, 136);
      this.pnlPersonaOptOut.TabIndex = 4;
      this.lblPersonaOptOut.Anchor = AnchorStyles.None;
      this.lblPersonaOptOut.Location = new Point(-75, 53);
      this.lblPersonaOptOut.Name = "lblPersonaOptOut";
      this.lblPersonaOptOut.Size = new Size(402, 31);
      this.lblPersonaOptOut.TabIndex = 0;
      this.lblPersonaOptOut.Text = "This persona has been opted out of the e360Select program. To opt in, select the Persona Opt In box above.";
      this.lblPersonaOptOut.TextAlign = ContentAlignment.TopCenter;
      this.collapsibleSplitter1.AnimationDelay = 20;
      this.collapsibleSplitter1.AnimationStep = 20;
      this.collapsibleSplitter1.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter1.ControlToHide = (Control) this.grpPersonas;
      this.collapsibleSplitter1.ExpandParentForm = false;
      this.collapsibleSplitter1.Location = new Point(200, 0);
      this.collapsibleSplitter1.Name = "collapsibleSplitter1";
      this.collapsibleSplitter1.TabIndex = 2;
      this.collapsibleSplitter1.TabStop = false;
      this.collapsibleSplitter1.UseAnimations = false;
      this.collapsibleSplitter1.VisualStyle = VisualStyles.Encompass;
      this.grpPersonas.Controls.Add((Control) this.gvPersonas);
      this.grpPersonas.Dock = DockStyle.Left;
      this.grpPersonas.HeaderForeColor = SystemColors.ControlText;
      this.grpPersonas.Location = new Point(0, 0);
      this.grpPersonas.Name = "grpPersonas";
      this.grpPersonas.Size = new Size(200, 513);
      this.grpPersonas.TabIndex = 0;
      this.grpPersonas.Text = "Personas";
      this.gvPersonas.AllowMultiselect = false;
      this.gvPersonas.BorderStyle = BorderStyle.None;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column1";
      gvColumn5.SpringToFit = true;
      gvColumn5.Text = "Column";
      gvColumn5.Width = 198;
      this.gvPersonas.Columns.AddRange(new GVColumn[1]
      {
        gvColumn5
      });
      this.gvPersonas.Dock = DockStyle.Fill;
      this.gvPersonas.HeaderHeight = 0;
      this.gvPersonas.HeaderVisible = false;
      this.gvPersonas.Location = new Point(1, 26);
      this.gvPersonas.Name = "gvPersonas";
      this.gvPersonas.Size = new Size(198, 486);
      this.gvPersonas.TabIndex = 1;
      this.gvPersonas.SelectedIndexChanged += new EventHandler(this.gvPersonas_SelectedIndexChanged);
      this.tpUsers.Controls.Add((Control) this.grpUserOptOuts);
      this.tpUsers.Controls.Add((Control) this.collapsibleSplitter2);
      this.tpUsers.Controls.Add((Control) this.grpUsers);
      this.tpUsers.Location = new Point(4, 23);
      this.tpUsers.Name = "tpUsers";
      this.tpUsers.Padding = new Padding(1, 3, 3, 3);
      this.tpUsers.Size = new Size(731, 519);
      this.tpUsers.TabIndex = 1;
      this.tpUsers.Text = "Users";
      this.tpUsers.UseVisualStyleBackColor = true;
      this.grpUserOptOuts.Controls.Add((Control) this.pnlUserOptOut);
      this.grpUserOptOuts.Controls.Add((Control) this.pnlUserOptIn);
      this.grpUserOptOuts.Controls.Add((Control) this.chkUserOptIn);
      this.grpUserOptOuts.Dock = DockStyle.Fill;
      this.grpUserOptOuts.HeaderForeColor = SystemColors.ControlText;
      this.grpUserOptOuts.Location = new Point(204, 3);
      this.grpUserOptOuts.Name = "grpUserOptOuts";
      this.grpUserOptOuts.Size = new Size(524, 513);
      this.grpUserOptOuts.TabIndex = 4;
      this.grpUserOptOuts.Text = "User Opt-In Settings";
      this.pnlUserOptOut.Controls.Add((Control) this.lblUserOptOut);
      this.pnlUserOptOut.Location = new Point(106, 168);
      this.pnlUserOptOut.Name = "pnlUserOptOut";
      this.pnlUserOptOut.Size = new Size(200, 128);
      this.pnlUserOptOut.TabIndex = 5;
      this.lblUserOptOut.Anchor = AnchorStyles.None;
      this.lblUserOptOut.Location = new Point(-101, 49);
      this.lblUserOptOut.Name = "lblUserOptOut";
      this.lblUserOptOut.Size = new Size(402, 31);
      this.lblUserOptOut.TabIndex = 0;
      this.lblUserOptOut.Text = "This user has been opted out of the e360Select program. To opt in, select the User Opt In box above.";
      this.lblUserOptOut.TextAlign = ContentAlignment.TopCenter;
      this.pnlUserOptIn.Controls.Add((Control) this.gvUserOptOuts);
      this.pnlUserOptIn.Controls.Add((Control) this.gradientPanel3);
      this.pnlUserOptIn.Dock = DockStyle.Fill;
      this.pnlUserOptIn.Location = new Point(1, 26);
      this.pnlUserOptIn.Name = "pnlUserOptIn";
      this.pnlUserOptIn.Size = new Size(522, 486);
      this.pnlUserOptIn.TabIndex = 3;
      this.gvUserOptOuts.BorderStyle = BorderStyle.None;
      gvColumn6.CheckBoxes = true;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column1";
      gvColumn6.Text = "Name";
      gvColumn6.Width = 200;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column2";
      gvColumn7.Text = "Description";
      gvColumn7.Width = 350;
      this.gvUserOptOuts.Columns.AddRange(new GVColumn[2]
      {
        gvColumn6,
        gvColumn7
      });
      this.gvUserOptOuts.Dock = DockStyle.Fill;
      this.gvUserOptOuts.ItemGrouping = true;
      this.gvUserOptOuts.Location = new Point(0, 25);
      this.gvUserOptOuts.Name = "gvUserOptOuts";
      this.gvUserOptOuts.Size = new Size(522, 461);
      this.gvUserOptOuts.TabIndex = 0;
      this.gvUserOptOuts.SubItemCheck += new GVSubItemEventHandler(this.gvUserOptOuts_SubItemCheck);
      this.gradientPanel3.Borders = AnchorStyles.Bottom;
      this.gradientPanel3.Controls.Add((Control) this.label3);
      this.gradientPanel3.Dock = DockStyle.Top;
      this.gradientPanel3.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel3.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel3.Location = new Point(0, 0);
      this.gradientPanel3.Name = "gradientPanel3";
      this.gradientPanel3.Size = new Size(522, 25);
      this.gradientPanel3.Style = GradientPanel.PanelStyle.TableFooter;
      this.gradientPanel3.TabIndex = 1;
      this.label3.AutoSize = true;
      this.label3.BackColor = Color.Transparent;
      this.label3.Location = new Point(6, 6);
      this.label3.Name = "label3";
      this.label3.Size = new Size(397, 14);
      this.label3.TabIndex = 0;
      this.label3.Text = "Select categories, providers, or programs to make programs visible to each user.";
      this.chkUserOptIn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkUserOptIn.AutoSize = true;
      this.chkUserOptIn.BackColor = Color.Transparent;
      this.chkUserOptIn.Location = new Point(440, 4);
      this.chkUserOptIn.Name = "chkUserOptIn";
      this.chkUserOptIn.Size = new Size(80, 18);
      this.chkUserOptIn.TabIndex = 2;
      this.chkUserOptIn.Text = "User Opt In";
      this.chkUserOptIn.UseVisualStyleBackColor = false;
      this.chkUserOptIn.CheckedChanged += new EventHandler(this.chkUserOptIn_CheckedChanged);
      this.collapsibleSplitter2.AnimationDelay = 20;
      this.collapsibleSplitter2.AnimationStep = 20;
      this.collapsibleSplitter2.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter2.ControlToHide = (Control) this.grpUsers;
      this.collapsibleSplitter2.ExpandParentForm = false;
      this.collapsibleSplitter2.Location = new Point(201, 3);
      this.collapsibleSplitter2.Name = "collapsibleSplitter1";
      this.collapsibleSplitter2.TabIndex = 5;
      this.collapsibleSplitter2.TabStop = false;
      this.collapsibleSplitter2.UseAnimations = false;
      this.collapsibleSplitter2.VisualStyle = VisualStyles.Encompass;
      this.grpUsers.Controls.Add((Control) this.gvUsers);
      this.grpUsers.Dock = DockStyle.Left;
      this.grpUsers.HeaderForeColor = SystemColors.ControlText;
      this.grpUsers.Location = new Point(1, 3);
      this.grpUsers.Name = "grpUsers";
      this.grpUsers.Size = new Size(200, 513);
      this.grpUsers.TabIndex = 3;
      this.grpUsers.Text = "Users";
      this.gvUsers.AllowMultiselect = false;
      this.gvUsers.BorderStyle = BorderStyle.None;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Column1";
      gvColumn8.Text = "";
      gvColumn8.Width = 25;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "Column2";
      gvColumn9.Text = "User";
      gvColumn9.Width = 100;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "Column3";
      gvColumn10.Text = "Persona(s)";
      gvColumn10.Width = 150;
      this.gvUsers.Columns.AddRange(new GVColumn[3]
      {
        gvColumn8,
        gvColumn9,
        gvColumn10
      });
      this.gvUsers.Dock = DockStyle.Fill;
      this.gvUsers.ImageList = this.ilIcons;
      this.gvUsers.Location = new Point(1, 26);
      this.gvUsers.Name = "gvUsers";
      this.gvUsers.Size = new Size(198, 486);
      this.gvUsers.TabIndex = 1;
      this.gvUsers.SelectedIndexChanged += new EventHandler(this.gvUsers_SelectedIndexChanged);
      this.ilIcons.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("ilIcons.ImageStream");
      this.ilIcons.TransparentColor = Color.Transparent;
      this.ilIcons.Images.SetKeyName(0, "link.png");
      this.ilIcons.Images.SetKeyName(1, "link-broken.png");
      this.pnlCompanyOptOutOld.BackColor = Color.WhiteSmoke;
      this.pnlCompanyOptOutOld.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlCompanyOptOutOld.Controls.Add((Control) this.label4);
      this.pnlCompanyOptOutOld.Location = new Point(144, 32);
      this.pnlCompanyOptOutOld.Name = "pnlCompanyOptOutOld";
      this.pnlCompanyOptOutOld.Size = new Size(592, 250);
      this.pnlCompanyOptOutOld.TabIndex = 1;
      this.label4.Anchor = AnchorStyles.None;
      this.label4.Location = new Point(78, 107);
      this.label4.Name = "label4";
      this.label4.Size = new Size(437, 31);
      this.label4.TabIndex = 0;
      this.label4.Text = "Your company is not participating in the e360Select program. To activate this program, select the Company Opt In box above.";
      this.label4.TextAlign = ContentAlignment.MiddleCenter;
      this.gradientPanel1.Controls.Add((Control) this.label2);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.Location = new Point(0, 0);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(741, 26);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.TableHeader;
      this.gradientPanel1.TabIndex = 0;
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(8, 6);
      this.label2.Name = "label2";
      this.label2.Size = new Size(151, 14);
      this.label2.TabIndex = 0;
      this.label2.Text = "e360Select Opt-In Settings";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.pnlTabs);
      this.Controls.Add((Control) this.gradientPanel1);
      this.Controls.Add((Control) this.pnlCompanyOptOutOld);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (LoanScreeningSetupControl);
      this.Size = new Size(741, 574);
      this.pnlTabs.ResumeLayout(false);
      this.tabsOfferSettings.ResumeLayout(false);
      this.tpCompany.ResumeLayout(false);
      this.grpCompanyOptOuts.ResumeLayout(false);
      this.grpCompanyOptOuts.PerformLayout();
      this.pnlCompanyOptOut.ResumeLayout(false);
      this.pnlCompanyOptIn.ResumeLayout(false);
      this.gradientPanel4.ResumeLayout(false);
      this.gradientPanel4.PerformLayout();
      this.tpPersonas.ResumeLayout(false);
      this.pnlCompany.ResumeLayout(false);
      this.grpPersonaOptOuts.ResumeLayout(false);
      this.grpPersonaOptOuts.PerformLayout();
      this.pnlPersonaOptIn.ResumeLayout(false);
      this.gradientPanel2.ResumeLayout(false);
      this.gradientPanel2.PerformLayout();
      this.pnlPersonaOptOut.ResumeLayout(false);
      this.grpPersonas.ResumeLayout(false);
      this.tpUsers.ResumeLayout(false);
      this.grpUserOptOuts.ResumeLayout(false);
      this.grpUserOptOuts.PerformLayout();
      this.pnlUserOptOut.ResumeLayout(false);
      this.pnlUserOptIn.ResumeLayout(false);
      this.gradientPanel3.ResumeLayout(false);
      this.gradientPanel3.PerformLayout();
      this.grpUsers.ResumeLayout(false);
      this.pnlCompanyOptOutOld.ResumeLayout(false);
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
