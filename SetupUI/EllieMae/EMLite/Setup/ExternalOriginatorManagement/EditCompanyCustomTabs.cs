// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.EditCompanyCustomTabs
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.ExternalOriginatorManagement.RestApi;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.ExternalOriginatorManagement
{
  public class EditCompanyCustomTabs : UserControl
  {
    private TabPage[] dynamicTabPages = new TabPage[5];
    private Sessions.Session session;
    private int orgID = -1;
    private int parentId = -1;
    private int companyOrgId = -1;
    private ArrayList[] customFieldLists;
    private ArrayList customFieldsControls;
    private Color backColor = Color.Transparent;
    private bool isChkParentInfoUiDirty;
    private bool hasTab1Right = true;
    private bool hasTab2Right = true;
    private bool hasTab3Right = true;
    private bool hasTab4Right = true;
    private bool hasTab5Right = true;
    private bool hasAccess = true;
    private IContainer components;
    private TabControl tabControl1;
    private TabPage tabPageCustom1;
    private TabPage tabPageCustom2;
    private TabPage tabPageCustom3;
    private TabPage tabPageCustom4;
    private TabPage tabPageCustom5;
    private GradientPanel gradientPanel1;
    private GradientPanel gradientPanel2;
    private StandardIconButton saveButton;
    private StandardIconButton resetButton;
    private CheckBox chkUseParentInfo;
    private Label label1;

    public EditCompanyCustomTabs(
      Sessions.Session session,
      int orgID,
      int companyOrgId,
      int parentId,
      bool isTpoTool)
    {
      this.session = session;
      this.orgID = orgID;
      this.companyOrgId = companyOrgId;
      this.parentId = parentId;
      this.InitializeComponent();
      FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      this.hasTab1Right = isTpoTool ? aclManager.GetUserApplicationRight(AclFeature.ToolsTab_TPOCustomFieldsTab1Information) : aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_CustomFieldsTab1);
      this.hasTab2Right = isTpoTool ? aclManager.GetUserApplicationRight(AclFeature.ToolsTab_TPOcustomFieldsTab2Information) : aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_CustomFieldsTab2);
      this.hasTab3Right = isTpoTool ? aclManager.GetUserApplicationRight(AclFeature.ToolsTab_TPOcustomFieldsTab3Information) : aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_CustomFieldsTab3);
      this.hasTab4Right = isTpoTool ? aclManager.GetUserApplicationRight(AclFeature.ToolsTab_TPOcustomFieldsTab4Information) : aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_CustomFieldsTab4);
      this.hasTab5Right = isTpoTool ? aclManager.GetUserApplicationRight(AclFeature.ToolsTab_TPOcustomFieldsTab5Information) : aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_CustomFieldsTab5);
      if (parentId == 0 || parentId == -1)
      {
        this.chkUseParentInfo.Enabled = this.chkUseParentInfo.Checked = false;
      }
      else
      {
        this.chkUseParentInfo.Checked = this.session.ConfigurationManager.GetExternalOrganization(false, orgID).InheritCustomFields;
        this.chkUseParentInfo.Enabled = true;
      }
      this.saveButton.Enabled = false;
      this.resetButton.Enabled = false;
      this.IsChkParentInfoUiDirty = false;
      this.InitDynamicTabs();
      this.tabControl1.SelectedIndexChanged += new EventHandler(this.tabControl1_SelectedIndexChanged);
      this.tabControl1.SelectedTab = this.tabPageCustom1;
      this.setCustomFieldsState();
      new List<Control>()
      {
        this.Controls.Find(nameof (saveButton), true)[0]
      };
    }

    public bool getTabRight(int i)
    {
      switch (i)
      {
        case 0:
          return this.hasTab1Right;
        case 1:
          return this.hasTab2Right;
        case 2:
          return this.hasTab3Right;
        case 3:
          return this.hasTab4Right;
        case 4:
          return this.hasTab5Right;
        default:
          return true;
      }
    }

    public void TabControlEnforcement()
    {
      bool flag = true;
      if (!this.hasTab1Right)
        this.tabControl1.Controls.Remove((Control) this.tabPageCustom1);
      else if (this.tabControl1.Controls.Contains((Control) this.dynamicTabPages[0]))
      {
        this.tabControl1.SelectedTab = this.dynamicTabPages[0];
        flag = false;
      }
      if (!this.hasTab2Right)
        this.tabControl1.Controls.Remove((Control) this.tabPageCustom2);
      else if (flag && this.tabControl1.Controls.Contains((Control) this.dynamicTabPages[1]))
      {
        this.tabControl1.SelectedTab = this.dynamicTabPages[1];
        flag = false;
      }
      if (!this.hasTab3Right)
        this.tabControl1.Controls.Remove((Control) this.tabPageCustom3);
      else if (flag && this.tabControl1.Controls.Contains((Control) this.dynamicTabPages[2]))
      {
        this.tabControl1.SelectedTab = this.dynamicTabPages[2];
        flag = false;
      }
      if (!this.hasTab4Right)
        this.tabControl1.Controls.Remove((Control) this.tabPageCustom4);
      else if (flag && this.tabControl1.Controls.Contains((Control) this.dynamicTabPages[3]))
      {
        this.tabControl1.SelectedTab = this.dynamicTabPages[3];
        flag = false;
      }
      if (!this.hasTab5Right)
      {
        this.tabControl1.Controls.Remove((Control) this.tabPageCustom5);
      }
      else
      {
        if (!flag || !this.tabControl1.Controls.Contains((Control) this.dynamicTabPages[4]))
          return;
        this.tabControl1.SelectedTab = this.dynamicTabPages[4];
      }
    }

    public bool HasAccess
    {
      get => this.hasAccess;
      set => this.hasAccess = value;
    }

    private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.tabControl1.SelectedTab == null)
        return;
      if (this.tabControl1.SelectedTab.Controls.Count == 0)
        this.loadTabPageControl();
      if (this.customFieldsControls == null)
        return;
      for (int index = 0; index < this.customFieldsControls.Count; ++index)
        ((CustomFieldsControl) this.customFieldsControls[index]).IsReadOnly = this.chkUseParentInfo.Checked;
    }

    private ContactCustomFieldInfoCollection getCustomFields()
    {
      return this.session.ConfigurationManager.GetCustomFieldInfo();
    }

    public void InitDynamicTabs()
    {
      this.tabPageCustom1.Name = "Tab1";
      this.tabPageCustom2.Name = "Tab2";
      this.tabPageCustom3.Name = "Tab3";
      this.tabPageCustom4.Name = "Tab4";
      this.tabPageCustom5.Name = "Tab5";
      this.dynamicTabPages[0] = this.tabPageCustom1;
      this.dynamicTabPages[1] = this.tabPageCustom2;
      this.dynamicTabPages[2] = this.tabPageCustom3;
      this.dynamicTabPages[3] = this.tabPageCustom4;
      this.dynamicTabPages[4] = this.tabPageCustom5;
      ContactCustomFieldInfoCollection customFields = this.getCustomFields();
      ContactCustomFieldInfo[] items = customFields.Items;
      this.dynamicTabPages[0].Text = customFields.Page1Name != string.Empty ? customFields.Page1Name : "Page1";
      this.dynamicTabPages[1].Text = customFields.Page2Name != string.Empty ? customFields.Page2Name : "Page2";
      this.dynamicTabPages[2].Text = customFields.Page3Name != string.Empty ? customFields.Page3Name : "Page3";
      this.dynamicTabPages[3].Text = customFields.Page4Name != string.Empty ? customFields.Page4Name : "Page4";
      this.dynamicTabPages[4].Text = customFields.Page5Name != string.Empty ? customFields.Page5Name : "Page5";
      Array.Sort<ContactCustomFieldInfo>(items);
      this.customFieldLists = new ArrayList[5]
      {
        new ArrayList(),
        new ArrayList(),
        new ArrayList(),
        new ArrayList(),
        new ArrayList()
      };
      for (int index = 0; index < items.Length; ++index)
        this.customFieldLists[(items[index].LabelID - 1) / 20].Add((object) items[index]);
      foreach (TabPage dynamicTabPage in this.dynamicTabPages)
      {
        if (this.tabControl1.Controls.Contains((Control) dynamicTabPage))
          this.tabControl1.Controls.Remove((Control) dynamicTabPage);
      }
      this.customFieldsControls = new ArrayList();
      for (int index = 0; index < 5; ++index)
      {
        if (this.customFieldLists[index].Count > 0)
        {
          this.tabControl1.Controls.Add((Control) this.dynamicTabPages[index]);
          this.dynamicTabPages[index].Controls.Clear();
        }
      }
      this.TabControlEnforcement();
      int selectedIndex = this.tabControl1.SelectedIndex;
      this.loadTabPageControl();
      this.tabControl1.SelectedIndex = selectedIndex;
    }

    private void loadTabPageControl()
    {
      int index = 1;
      for (int i = 0; i < this.dynamicTabPages.Length; ++i)
      {
        EditCompanyCustomFields companyCustomFields = new EditCompanyCustomFields(this.session, this.orgID, this.parentId);
        if (this.chkUseParentInfo.Checked)
          companyCustomFields.SetContactID(this.orgID);
        else
          companyCustomFields.CurrentContactID = this.orgID;
        companyCustomFields.CustomFieldInfo = (ContactCustomFieldInfo[]) this.customFieldLists[i].ToArray(typeof (ContactCustomFieldInfo));
        companyCustomFields.Dock = DockStyle.Fill;
        companyCustomFields.DataChanged += new EventHandler(this.ctlCustomFields_DataChanged);
        companyCustomFields.populateTPOFieldValues(this.chkUseParentInfo.Checked);
        this.customFieldsControls.Add((object) companyCustomFields);
        UserControl userControl = (UserControl) companyCustomFields;
        if (this.tabControl1.SelectedTab == this.dynamicTabPages[i])
        {
          userControl.Visible = true;
          userControl.Dock = DockStyle.Fill;
          this.tabControl1.SelectedTab.Controls.Add((Control) userControl);
        }
        else if (this.tabControl1.SelectedTab != null && this.customFieldLists[i].Count > 0)
        {
          if (this.getTabRight(i))
          {
            try
            {
              this.tabControl1.TabPages[index].Controls.Add((Control) userControl);
              ++index;
            }
            catch (Exception ex)
            {
            }
          }
        }
      }
      this.Cursor = Cursors.Default;
      bool hierarchyAccess;
      Session.TpoHierarchyAccessCache.TryGetValue(this.orgID, out hierarchyAccess);
      TPOClientUtils.DisableControls((UserControl) this, hierarchyAccess);
    }

    private void ctlCustomFields_DataChanged(object sender, EventArgs e)
    {
      this.saveButton.Enabled = true;
      this.resetButton.Enabled = true;
    }

    public void saveButton_Click(object sender, EventArgs e)
    {
      try
      {
        for (int index = 0; index < this.customFieldsControls.Count; ++index)
        {
          EditCompanyCustomFields customFieldsControl = (EditCompanyCustomFields) this.customFieldsControls[index];
          customFieldsControl.SetIsDirty();
          customFieldsControl.SaveChanges();
        }
        this.saveButton.Enabled = false;
        this.resetButton.Enabled = false;
        this.session.ConfigurationManager.UpdateInheritCustomFieldsFlag(this.orgID, this.chkUseParentInfo.Checked);
        WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.Parent.Text, this.orgID);
        this.IsChkParentInfoUiDirty = false;
      }
      catch (FormatException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Custom Fields values cannot be saved, please contact your Administrator.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void resetButton_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to reset? All changes will be lost.", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
        return;
      if (this.parentId == 0 || this.parentId == -1)
      {
        this.chkUseParentInfo.Enabled = this.chkUseParentInfo.Checked = false;
      }
      else
      {
        this.chkUseParentInfo.Checked = this.session.ConfigurationManager.GetExternalOrganization(false, this.orgID).InheritCustomFields;
        this.chkUseParentInfo.Enabled = true;
      }
      this.InitDynamicTabs();
      this.setCustomFieldsState();
      this.saveButton.Enabled = false;
      this.resetButton.Enabled = false;
      this.IsChkParentInfoUiDirty = false;
    }

    public bool IsDirty
    {
      get
      {
        for (int index = 0; index < this.customFieldsControls.Count; ++index)
        {
          if (((CustomFieldsControl) this.customFieldsControls[index]).isDirty())
            return true;
        }
        return false;
      }
    }

    public bool IsChkParentInfoUiDirty
    {
      get => this.isChkParentInfoUiDirty;
      set => this.isChkParentInfoUiDirty = value;
    }

    private void setCustomFieldsState()
    {
      if (this.customFieldsControls == null)
        return;
      for (int index = 0; index < this.customFieldsControls.Count; ++index)
        ((CustomFieldsControl) this.customFieldsControls[index]).IsReadOnly = this.chkUseParentInfo.Checked;
    }

    private void chkUseParentInfo_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkUseParentInfo.Checked)
        this.InitDynamicTabs();
      this.saveButton.Enabled = true;
      this.resetButton.Enabled = true;
      this.setCustomFieldsState();
      this.IsChkParentInfoUiDirty = true;
    }

    public void DisableControls()
    {
      this.saveButton.Visible = this.resetButton.Visible = false;
      if (this.customFieldsControls == null)
        return;
      foreach (EditCompanyCustomFields customFieldsControl in this.customFieldsControls)
        customFieldsControl.DisableControls();
    }

    private void label1_Click(object sender, EventArgs e)
    {
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.tabControl1 = new TabControl();
      this.tabPageCustom1 = new TabPage();
      this.tabPageCustom2 = new TabPage();
      this.tabPageCustom3 = new TabPage();
      this.tabPageCustom4 = new TabPage();
      this.tabPageCustom5 = new TabPage();
      this.gradientPanel2 = new GradientPanel();
      this.label1 = new Label();
      this.gradientPanel1 = new GradientPanel();
      this.chkUseParentInfo = new CheckBox();
      this.resetButton = new StandardIconButton();
      this.saveButton = new StandardIconButton();
      this.tabControl1.SuspendLayout();
      this.gradientPanel2.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      ((ISupportInitialize) this.resetButton).BeginInit();
      ((ISupportInitialize) this.saveButton).BeginInit();
      this.SuspendLayout();
      this.tabControl1.Controls.Add((Control) this.tabPageCustom1);
      this.tabControl1.Controls.Add((Control) this.tabPageCustom2);
      this.tabControl1.Controls.Add((Control) this.tabPageCustom3);
      this.tabControl1.Controls.Add((Control) this.tabPageCustom4);
      this.tabControl1.Controls.Add((Control) this.tabPageCustom5);
      this.tabControl1.Dock = DockStyle.Fill;
      this.tabControl1.Location = new Point(5, 67);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new Size(768, 406);
      this.tabControl1.TabIndex = 0;
      this.tabPageCustom1.BackColor = Color.WhiteSmoke;
      this.tabPageCustom1.Location = new Point(4, 22);
      this.tabPageCustom1.Name = "tabPageCustom1";
      this.tabPageCustom1.Padding = new Padding(3);
      this.tabPageCustom1.Size = new Size(760, 380);
      this.tabPageCustom1.TabIndex = 0;
      this.tabPageCustom1.Text = "tabPage1";
      this.tabPageCustom2.BackColor = Color.WhiteSmoke;
      this.tabPageCustom2.Location = new Point(4, 22);
      this.tabPageCustom2.Name = "tabPageCustom2";
      this.tabPageCustom2.Padding = new Padding(3);
      this.tabPageCustom2.Size = new Size(770, 390);
      this.tabPageCustom2.TabIndex = 1;
      this.tabPageCustom2.Text = "tabPage2";
      this.tabPageCustom3.BackColor = Color.WhiteSmoke;
      this.tabPageCustom3.Location = new Point(4, 22);
      this.tabPageCustom3.Name = "tabPageCustom3";
      this.tabPageCustom3.Size = new Size(770, 390);
      this.tabPageCustom3.TabIndex = 2;
      this.tabPageCustom3.Text = "tabPage3";
      this.tabPageCustom4.BackColor = Color.WhiteSmoke;
      this.tabPageCustom4.Location = new Point(4, 22);
      this.tabPageCustom4.Name = "tabPageCustom4";
      this.tabPageCustom4.Size = new Size(770, 390);
      this.tabPageCustom4.TabIndex = 3;
      this.tabPageCustom4.Text = "tabPage4";
      this.tabPageCustom5.BackColor = Color.WhiteSmoke;
      this.tabPageCustom5.Location = new Point(4, 22);
      this.tabPageCustom5.Name = "tabPageCustom5";
      this.tabPageCustom5.Size = new Size(770, 390);
      this.tabPageCustom5.TabIndex = 4;
      this.tabPageCustom5.Text = "tabPage5";
      this.gradientPanel2.Borders = AnchorStyles.Top | AnchorStyles.Bottom;
      this.gradientPanel2.Controls.Add((Control) this.label1);
      this.gradientPanel2.Dock = DockStyle.Top;
      this.gradientPanel2.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel2.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel2.Location = new Point(5, 35);
      this.gradientPanel2.Name = "gradientPanel2";
      this.gradientPanel2.Size = new Size(768, 32);
      this.gradientPanel2.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel2.TabIndex = 1;
      this.label1.Dock = DockStyle.Fill;
      this.label1.Location = new Point(0, 0);
      this.label1.Name = "label1";
      this.label1.Padding = new Padding(4, 2, 0, 0);
      this.label1.Size = new Size(768, 32);
      this.label1.TabIndex = 0;
      this.label1.Text = "The fields below have been configured by your company to capture additional data for your business.";
      this.gradientPanel1.BackColorGlassyStyle = true;
      this.gradientPanel1.Borders = AnchorStyles.None;
      this.gradientPanel1.Controls.Add((Control) this.chkUseParentInfo);
      this.gradientPanel1.Controls.Add((Control) this.resetButton);
      this.gradientPanel1.Controls.Add((Control) this.saveButton);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(81, 123, 184);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(167, 201, 239);
      this.gradientPanel1.Location = new Point(5, 5);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(768, 30);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.PageHeader;
      this.gradientPanel1.TabIndex = 0;
      this.chkUseParentInfo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkUseParentInfo.AutoSize = true;
      this.chkUseParentInfo.BackColor = Color.Transparent;
      this.chkUseParentInfo.Location = new Point(620, 8);
      this.chkUseParentInfo.Name = "chkUseParentInfo";
      this.chkUseParentInfo.Size = new Size(100, 17);
      this.chkUseParentInfo.TabIndex = 2;
      this.chkUseParentInfo.Text = "Use Parent Info";
      this.chkUseParentInfo.UseVisualStyleBackColor = false;
      this.chkUseParentInfo.CheckedChanged += new EventHandler(this.chkUseParentInfo_CheckedChanged);
      this.resetButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.resetButton.BackColor = Color.Transparent;
      this.resetButton.Location = new Point(748, 8);
      this.resetButton.MouseDownImage = (Image) null;
      this.resetButton.Name = "resetButton";
      this.resetButton.Size = new Size(16, 16);
      this.resetButton.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.resetButton.TabIndex = 1;
      this.resetButton.TabStop = false;
      this.resetButton.Click += new EventHandler(this.resetButton_Click);
      this.saveButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.saveButton.BackColor = Color.Transparent;
      this.saveButton.Location = new Point(726, 8);
      this.saveButton.MouseDownImage = (Image) null;
      this.saveButton.Name = "saveButton";
      this.saveButton.Size = new Size(16, 16);
      this.saveButton.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.saveButton.TabIndex = 0;
      this.saveButton.TabStop = false;
      this.saveButton.Click += new EventHandler(this.saveButton_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.Controls.Add((Control) this.tabControl1);
      this.Controls.Add((Control) this.gradientPanel2);
      this.Controls.Add((Control) this.gradientPanel1);
      this.Margin = new Padding(0);
      this.Name = nameof (EditCompanyCustomTabs);
      this.Padding = new Padding(5);
      this.Size = new Size(778, 478);
      this.tabControl1.ResumeLayout(false);
      this.gradientPanel2.ResumeLayout(false);
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      ((ISupportInitialize) this.resetButton).EndInit();
      ((ISupportInitialize) this.saveButton).EndInit();
      this.ResumeLayout(false);
    }
  }
}
