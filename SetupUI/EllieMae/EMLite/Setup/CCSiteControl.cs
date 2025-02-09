// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.CCSiteControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.LoanUtils.ConsumerConnect;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class CCSiteControl : UserControl
  {
    private Sessions.Session session;
    public CCSiteInfo ccSiteSettings;
    private IOrganizationManager rOrg;
    private int parentID = -1;
    private int oid = -1;
    private string userid;
    private bool isOrg;
    private bool newOrg;
    private bool initParentInfo;
    private bool userCCSiteIsNull;
    public bool invalidCCSiteURL;
    public bool userChangedURLText;
    private IContainer components;
    private GroupContainer groupContainer5;
    private CheckBox useParentInfoChkBox;
    private Label ccSiteUrlLabel;
    private TextBox urlTextBox;
    private StandardIconButton searchButton;

    public CCSiteControl(Sessions.Session session, OrgInfo orgInfo, bool newOrg = false)
    {
      this.session = session;
      this.rOrg = this.session.OrganizationManager;
      this.InitializeComponent();
      if (!newOrg)
        this.ccSiteSettings = orgInfo.CCSiteSettings;
      this.parentID = orgInfo.Parent;
      this.oid = orgInfo.Oid;
      this.isOrg = true;
      if (this.ccSiteSettings != null)
        this.setCCSiteInfo();
      else
        this.ccSiteSettings = new CCSiteInfo();
      if (this.oid == this.parentID && !newOrg)
        this.useParentInfoChkBox.Enabled = false;
      this.newOrg = newOrg;
      this.initParentInfo = true;
    }

    public CCSiteControl(Sessions.Session session, UserInfo userInfo, int oid = -1)
    {
      this.session = session;
      this.rOrg = this.session.OrganizationManager;
      this.InitializeComponent();
      if (userInfo != (UserInfo) null)
      {
        this.userid = userInfo.Userid;
        this.ccSiteSettings = this.rOrg.GetUserCCSiteInfo(userInfo.Userid);
      }
      if (this.ccSiteSettings != null)
        this.setCCSiteInfo();
      else if (userInfo != (UserInfo) null)
      {
        this.ccSiteSettings = new CCSiteInfo();
        this.ccSiteSettings.UseParentInfo = userInfo.InheritParentCCSite;
        if (this.ccSiteSettings.UseParentInfo)
        {
          this.toggleCCSite_UI(false);
          this.useParentInfoChkBox.Checked = true;
          this.userCCSiteIsNull = true;
        }
        else
        {
          this.toggleCCSite_UI(true);
          this.useParentInfoChkBox.Checked = false;
        }
      }
      else if (oid != -1)
      {
        OrgInfo organization = this.rOrg.GetOrganization(oid);
        this.ccSiteSettings = new CCSiteInfo();
        if (organization.CCSiteSettings != null && organization.CCSiteSettings.SiteId.Length > 0)
          this.setCCSiteInfo(organization.CCSiteSettings);
        this.ccSiteSettings.UseParentInfo = true;
        this.toggleCCSite_UI(false);
        this.useParentInfoChkBox.Checked = true;
      }
      else
      {
        this.ccSiteSettings = new CCSiteInfo();
        this.ccSiteSettings.UseParentInfo = true;
        this.toggleCCSite_UI(false);
        this.useParentInfoChkBox.Checked = true;
      }
      this.initParentInfo = true;
    }

    private void setCCSiteInfo()
    {
      if (this.ccSiteSettings.SiteId.Length > 0)
      {
        EllieMae.EMLite.LoanUtils.ConsumerConnect.Site siteForSiteId = new SiteAccessor().GetSiteForSiteID(this.session.SessionObjects, this.ccSiteSettings.SiteId);
        if (siteForSiteId == null)
          this.invalidCCSiteURL = true;
        else
          this.setCCUrl(CCSiteControl.getCCSiteURL(siteForSiteId));
      }
      if (!this.ccSiteSettings.UseParentInfo)
        return;
      this.useParentInfoChkBox.Checked = this.ccSiteSettings.UseParentInfo;
      this.toggleCCSite_UI(false);
    }

    private void setCCSiteInfo(CCSiteInfo ccsiteInfo)
    {
      if (ccsiteInfo.SiteId.Length <= 0)
        return;
      EllieMae.EMLite.LoanUtils.ConsumerConnect.Site siteForSiteId = new SiteAccessor().GetSiteForSiteID(this.session.SessionObjects, ccsiteInfo.SiteId);
      if (siteForSiteId == null)
        return;
      this.setCCUrl(CCSiteControl.getCCSiteURL(siteForSiteId));
      this.ccSiteSettings.SiteId = ccsiteInfo.SiteId;
    }

    public static string getCCSiteURL(EllieMae.EMLite.LoanUtils.ConsumerConnect.Site site)
    {
      string ccSiteUrl = (string) null;
      foreach (string domain in site.domains)
      {
        if (domain.ToLower().EndsWith("mymortgage-app.net") || domain.ToLower().EndsWith("mymortgage-online.com"))
        {
          if (ccSiteUrl == null)
            ccSiteUrl = "https://" + domain;
        }
        else
        {
          ccSiteUrl = "https://" + domain;
          break;
        }
      }
      return ccSiteUrl;
    }

    private void useParentInfo_CheckedChanged(object sender, EventArgs e)
    {
      if (this.useParentInfoChkBox.Checked)
      {
        CCSiteInfo ccSiteInfo = (CCSiteInfo) null;
        if (this.userid == null)
        {
          if (this.oid != -1)
            ccSiteInfo = this.rOrg.getCCSiteInfo((this.newOrg || this.parentID == -1 ? this.rOrg.GetOrganization(this.oid) : this.rOrg.GetOrganization(this.parentID)).Oid);
        }
        else
        {
          UserInfo user = this.rOrg.GetUser(this.userid);
          if (!this.userCCSiteIsNull)
            ccSiteInfo = this.rOrg.getCCSiteInfo(user.OrgId);
          this.userCCSiteIsNull = false;
        }
        if (ccSiteInfo == null)
        {
          if (this.initParentInfo)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "Currently there is no Consumer Connect Site URL assigned to the parent organization. When a Consumer Connect Site URL is assigned to the parent organization in the future, it will then be applied to this organization and to it's users who have opted to use information from parent organization.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            this.urlTextBox.Clear();
          }
        }
        else
        {
          if (ccSiteInfo.Url.Length > 0)
          {
            if (this.isOrg && this.initParentInfo)
            {
              int num = (int) Utils.Dialog((IWin32Window) this, "Consumer Connect Site URL from the parent organization will be applied to this Organization and its users who have opted for 'Use Parent Info' associated to this Organization. " + Environment.NewLine + Environment.NewLine + "Please note that other Organizations and its Users, directly under this Organization's hierarchy and opted for 'Use Parent info' will be applied with this Consumer Connect Site URL", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            this.setCCUrl(ccSiteInfo.Url);
          }
          else
          {
            this.urlTextBox.Clear();
            if (this.initParentInfo)
            {
              int num = (int) Utils.Dialog((IWin32Window) this, "Currently there is no Consumer Connect Site URL assigned to the parent organization. When a Consumer Connect Site URL is assigned to the parent organization in the future, it will then be applied to this organization and to it's users who have opted to use information from parent organization.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
          }
          if (!this.userCCSiteIsNull)
          {
            this.ccSiteSettings.SiteId = ccSiteInfo.SiteId;
            this.setCCUrl(ccSiteInfo.Url);
          }
          this.userCCSiteIsNull = false;
        }
        this.toggleCCSite_UI(false);
        this.ccSiteSettings.UseParentInfo = true;
      }
      else
      {
        this.toggleCCSite_UI(true);
        this.ccSiteSettings.UseParentInfo = false;
      }
    }

    private void toggleCCSite_UI(bool show)
    {
      this.urlTextBox.Enabled = show;
      this.searchButton.Enabled = show;
    }

    private void setCCUrl(string url)
    {
      this.urlTextBox.Text = url;
      if (this.ccSiteSettings == null)
        return;
      this.ccSiteSettings.Url = url;
    }

    private void searchButton_Click(object sender, EventArgs e) => this.doSearch(true);

    public bool doSearch(bool throughSearchButton = false)
    {
      if (this.useParentInfoChkBox.Checked)
        return true;
      if (!throughSearchButton && this.urlTextBox.Text.Trim().Length == 0)
      {
        this.ccSiteSettings.SiteId = "";
        this.setCCUrl("");
        this.userChangedURLText = false;
        return true;
      }
      ConsumerConnectSiteSearch connectSiteSearch = new ConsumerConnectSiteSearch(this.session, this.urlTextBox.Text.Trim());
      int numQueryResults = connectSiteSearch.getNumQueryResults();
      if (numQueryResults == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "There are no Consumer Connect Site URLs available matching the entered text.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      if (numQueryResults > 1)
      {
        int num = (int) connectSiteSearch.ShowDialog();
        if (connectSiteSearch.userClickedCancel())
          return false;
      }
      EllieMae.EMLite.LoanUtils.ConsumerConnect.Site selectedSite = connectSiteSearch.getSelectedSite();
      this.ccSiteSettings.SiteId = string.Concat((object) selectedSite.id);
      this.setCCUrl(CCSiteControl.getCCSiteURL(selectedSite));
      this.userChangedURLText = false;
      return true;
    }

    private void urlTextBox_TextChanged(object sender, EventArgs e)
    {
      this.urlTextBox.TextChanged -= new EventHandler(this.urlTextBox_TextChanged);
      this.urlTextBox.Text = this.urlTextBox.Text.Trim();
      this.userChangedURLText = true;
      this.urlTextBox.TextChanged += new EventHandler(this.urlTextBox_TextChanged);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.groupContainer5 = new GroupContainer();
      this.searchButton = new StandardIconButton();
      this.urlTextBox = new TextBox();
      this.useParentInfoChkBox = new CheckBox();
      this.ccSiteUrlLabel = new Label();
      this.groupContainer5.SuspendLayout();
      ((ISupportInitialize) this.searchButton).BeginInit();
      this.SuspendLayout();
      this.groupContainer5.Controls.Add((Control) this.searchButton);
      this.groupContainer5.Controls.Add((Control) this.urlTextBox);
      this.groupContainer5.Controls.Add((Control) this.useParentInfoChkBox);
      this.groupContainer5.Controls.Add((Control) this.ccSiteUrlLabel);
      this.groupContainer5.Dock = DockStyle.Fill;
      this.groupContainer5.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer5.Location = new Point(0, 0);
      this.groupContainer5.Name = "groupContainer5";
      this.groupContainer5.Size = new Size(650, 80);
      this.groupContainer5.TabIndex = 0;
      this.groupContainer5.Text = "Consumer Connect Site";
      this.searchButton.BackColor = Color.Transparent;
      this.searchButton.Location = new Point(425, 35);
      this.searchButton.MouseDownImage = (Image) null;
      this.searchButton.Name = "searchButton";
      this.searchButton.Padding = new Padding(0, 2, 0, 0);
      this.searchButton.Size = new Size(16, 20);
      this.searchButton.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.searchButton.TabIndex = 19;
      this.searchButton.TabStop = false;
      this.searchButton.Click += new EventHandler(this.searchButton_Click);
      this.urlTextBox.Font = new Font("Arial", 8.25f);
      this.urlTextBox.Location = new Point(66, 35);
      this.urlTextBox.Name = "urlTextBox";
      this.urlTextBox.Size = new Size(353, 20);
      this.urlTextBox.TabIndex = 18;
      this.urlTextBox.TextChanged += new EventHandler(this.urlTextBox_TextChanged);
      this.useParentInfoChkBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.useParentInfoChkBox.AutoSize = true;
      this.useParentInfoChkBox.BackColor = Color.Transparent;
      this.useParentInfoChkBox.Location = new Point(531, 3);
      this.useParentInfoChkBox.Name = "useParentInfoChkBox";
      this.useParentInfoChkBox.Size = new Size(100, 17);
      this.useParentInfoChkBox.TabIndex = 0;
      this.useParentInfoChkBox.Text = "Use Parent Info";
      this.useParentInfoChkBox.UseVisualStyleBackColor = false;
      this.useParentInfoChkBox.CheckedChanged += new EventHandler(this.useParentInfo_CheckedChanged);
      this.ccSiteUrlLabel.AutoSize = true;
      this.ccSiteUrlLabel.Location = new Point(10, 38);
      this.ccSiteUrlLabel.Name = "ccSiteUrlLabel";
      this.ccSiteUrlLabel.Size = new Size(50, 13);
      this.ccSiteUrlLabel.TabIndex = 1;
      this.ccSiteUrlLabel.Text = "Site URL";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupContainer5);
      this.Name = nameof (CCSiteControl);
      this.Size = new Size(650, 80);
      this.groupContainer5.ResumeLayout(false);
      this.groupContainer5.PerformLayout();
      ((ISupportInitialize) this.searchButton).EndInit();
      this.ResumeLayout(false);
    }
  }
}
