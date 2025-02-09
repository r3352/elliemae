// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.StatusOnline.StatusOnlineUsersControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer.StatusOnline;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.EMLite.WebServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.StatusOnline
{
  public class StatusOnlineUsersControl : UserControl
  {
    private Sessions.Session session;
    private StatusOnlineSetup setup;
    private ModuleLicense license;
    private bool isDirty;
    private IContainer components;
    private GradientPanel pnlApplyTemplate;
    private RadioButton rdoLoanOfficer;
    private Label lblApplyTemplate;
    private RadioButton rdoFileStarter;
    private GroupContainer gcUsers;
    private StandardIconButton btnSave;
    private StandardIconButton btnReset;
    private GridView gvUsers;
    private GradientPanel pnlTitle;
    private Label lblTitle;

    public StatusOnlineUsersControl(Sessions.Session session, StatusOnlineSetup setup)
    {
      this.InitializeComponent();
      this.session = session;
      this.setup = setup;
      this.initUserList();
      this.Reset();
    }

    private void initUserList() => this.gvUsers.Sort(0, SortOrder.Ascending);

    private void loadUserList()
    {
      this.gvUsers.Items.Clear();
      this.license = Modules.GetModuleLicense(EncompassModule.StatusOnline);
      if (this.license == null)
        return;
      Persona[] allPersonas = Session.PersonaManager.GetAllPersonas();
      Dictionary<int, string> dictionary = new Dictionary<int, string>();
      foreach (Persona persona in allPersonas)
        dictionary.Add(persona.ID, persona.Name);
      Hashtable insensitiveHashtable1 = CollectionsUtil.CreateCaseInsensitiveHashtable();
      if (this.license.UserList != null)
      {
        foreach (ModuleUser user in this.license.UserList)
        {
          if (!user.Disabled)
            insensitiveHashtable1.Add((object) user.UserID, (object) null);
        }
      }
      UserInfo[] userInfoArray = this.session.OrganizationManager.GetAllUsers();
      if (this.license.UserLimit > 0)
      {
        Hashtable insensitiveHashtable2 = CollectionsUtil.CreateCaseInsensitiveHashtable();
        foreach (UserInfo userInfo in userInfoArray)
          insensitiveHashtable2.Add((object) userInfo.Userid, (object) userInfo);
        List<UserInfo> userInfoList = new List<UserInfo>();
        if (this.license.UserList != null)
        {
          foreach (ModuleUser user in this.license.UserList)
          {
            if (user.PersonalLicense && insensitiveHashtable2.ContainsKey((object) user.UserID))
              userInfoList.Add((UserInfo) insensitiveHashtable2[(object) user.UserID]);
          }
        }
        userInfoArray = userInfoList.ToArray();
      }
      this.gvUsers.BeginUpdate();
      foreach (UserInfo userInfo in userInfoArray)
      {
        List<string> stringList = new List<string>();
        foreach (int personaId in userInfo.GetPersonaIDs())
        {
          if (dictionary.ContainsKey(personaId))
            stringList.Add(dictionary[personaId]);
        }
        GVItem gvItem = this.gvUsers.Items.Add(userInfo.Userid);
        gvItem.SubItems[1].Value = (object) userInfo.LastName;
        gvItem.SubItems[2].Value = (object) userInfo.FirstName;
        gvItem.SubItems[3].Value = (object) string.Join(", ", stringList.ToArray());
        gvItem.SubItems[4].Checked = insensitiveHashtable1.ContainsKey((object) userInfo.Userid);
        gvItem.SubItems[5].Checked = userInfo.PersonalStatusOnline;
        gvItem.SubItems[5].CheckBoxVisible = gvItem.SubItems[4].Checked;
        gvItem.Tag = (object) userInfo;
      }
      this.gvUsers.ReSort();
      this.gvUsers.EndUpdate();
    }

    private void saveUserList()
    {
      if (this.license == null)
        return;
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>();
      List<string> stringList3 = new List<string>();
      List<string> stringList4 = new List<string>();
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      if (this.license.UserList != null)
      {
        foreach (ModuleUser user in this.license.UserList)
        {
          if (!user.Disabled)
            insensitiveHashtable.Add((object) user.UserID, (object) null);
        }
      }
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvUsers.Items)
      {
        UserInfo tag = gvItem.Tag as UserInfo;
        if (gvItem.SubItems[4].Checked && !insensitiveHashtable.ContainsKey((object) tag.Userid))
          stringList1.Add(tag.Userid);
        else if (!gvItem.SubItems[4].Checked && insensitiveHashtable.ContainsKey((object) tag.Userid))
          stringList2.Add(tag.Userid);
        if (gvItem.SubItems[5].Checked && !tag.PersonalStatusOnline)
          stringList3.Add(tag.Userid);
        else if (!gvItem.SubItems[5].Checked && tag.PersonalStatusOnline)
          stringList4.Add(tag.Userid);
      }
      if (stringList1.Count > 0 || stringList2.Count > 0)
        Modules.UpdateModuleUsers(EncompassModule.StatusOnline, stringList1.ToArray(), stringList2.ToArray());
      if (stringList3.Count > 0 || stringList4.Count > 0)
      {
        this.session.OrganizationManager.UpdatePersonalStatusOnlineUsers(stringList3.ToArray(), stringList4.ToArray());
        this.session.RecacheUserInfo();
      }
      this.loadUserList();
    }

    private void gvUsers_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      if (e.SubItem.Index == 4)
      {
        GVSubItem subItem = e.SubItem.Item.SubItems[5];
        subItem.Checked = e.SubItem.Checked;
        subItem.CheckBoxVisible = e.SubItem.Checked;
      }
      this.setDirtyFlag(true);
    }

    private void loadApplyTemplateSetting()
    {
      switch (this.setup.PersonalTriggersType)
      {
        case ApplyPersonalTriggersType.FileStarter:
          this.rdoFileStarter.Checked = true;
          break;
        case ApplyPersonalTriggersType.LoanOfficer:
          this.rdoLoanOfficer.Checked = true;
          break;
      }
    }

    private void saveApplyTemplateSetting()
    {
      ApplyPersonalTriggersType personalTriggersType = ApplyPersonalTriggersType.None;
      if (this.rdoFileStarter.Checked)
        personalTriggersType = ApplyPersonalTriggersType.FileStarter;
      else if (this.rdoLoanOfficer.Checked)
        personalTriggersType = ApplyPersonalTriggersType.LoanOfficer;
      if (this.setup.PersonalTriggersType == personalTriggersType)
        return;
      this.setup.PersonalTriggersType = personalTriggersType;
      this.session.ConfigurationManager.SaveStatusOnlineSetup((string) null, this.setup);
    }

    private void rdoApplyTemplate_Click(object sender, EventArgs e) => this.setDirtyFlag(true);

    private void setDirtyFlag(bool isDirty)
    {
      this.isDirty = isDirty;
      this.btnReset.Enabled = isDirty;
      this.btnSave.Enabled = isDirty;
    }

    public bool IsDirty => this.isDirty;

    public void Reset()
    {
      this.loadUserList();
      this.loadApplyTemplateSetting();
      this.setDirtyFlag(false);
    }

    public void Save()
    {
      this.saveUserList();
      this.saveApplyTemplateSetting();
      this.setDirtyFlag(false);
    }

    private void btnReset_Click(object sender, EventArgs e) => this.Reset();

    private void btnSave_Click(object sender, EventArgs e) => this.Save();

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
      this.pnlApplyTemplate = new GradientPanel();
      this.rdoLoanOfficer = new RadioButton();
      this.lblApplyTemplate = new Label();
      this.rdoFileStarter = new RadioButton();
      this.gcUsers = new GroupContainer();
      this.btnSave = new StandardIconButton();
      this.btnReset = new StandardIconButton();
      this.gvUsers = new GridView();
      this.pnlTitle = new GradientPanel();
      this.lblTitle = new Label();
      this.pnlApplyTemplate.SuspendLayout();
      this.gcUsers.SuspendLayout();
      ((ISupportInitialize) this.btnSave).BeginInit();
      ((ISupportInitialize) this.btnReset).BeginInit();
      this.pnlTitle.SuspendLayout();
      this.SuspendLayout();
      this.pnlApplyTemplate.Borders = AnchorStyles.Top;
      this.pnlApplyTemplate.Controls.Add((Control) this.rdoLoanOfficer);
      this.pnlApplyTemplate.Controls.Add((Control) this.lblApplyTemplate);
      this.pnlApplyTemplate.Controls.Add((Control) this.rdoFileStarter);
      this.pnlApplyTemplate.Dock = DockStyle.Bottom;
      this.pnlApplyTemplate.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.pnlApplyTemplate.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.pnlApplyTemplate.Location = new Point(1, 286);
      this.pnlApplyTemplate.Name = "pnlApplyTemplate";
      this.pnlApplyTemplate.Size = new Size(753, 31);
      this.pnlApplyTemplate.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.pnlApplyTemplate.TabIndex = 2;
      this.rdoLoanOfficer.AutoSize = true;
      this.rdoLoanOfficer.BackColor = Color.Transparent;
      this.rdoLoanOfficer.Location = new Point(438, 6);
      this.rdoLoanOfficer.Name = "rdoLoanOfficer";
      this.rdoLoanOfficer.Size = new Size(86, 18);
      this.rdoLoanOfficer.TabIndex = 2;
      this.rdoLoanOfficer.TabStop = true;
      this.rdoLoanOfficer.Text = "Loan Officer";
      this.rdoLoanOfficer.UseVisualStyleBackColor = false;
      this.rdoLoanOfficer.Click += new EventHandler(this.rdoApplyTemplate_Click);
      this.lblApplyTemplate.AutoSize = true;
      this.lblApplyTemplate.BackColor = Color.Transparent;
      this.lblApplyTemplate.Location = new Point(8, 8);
      this.lblApplyTemplate.Name = "lblApplyTemplate";
      this.lblApplyTemplate.Size = new Size(336, 14);
      this.lblApplyTemplate.TabIndex = 0;
      this.lblApplyTemplate.Text = "For a new loan, apply company templates and personal templates of";
      this.rdoFileStarter.AutoSize = true;
      this.rdoFileStarter.BackColor = Color.Transparent;
      this.rdoFileStarter.Location = new Point(355, 6);
      this.rdoFileStarter.Name = "rdoFileStarter";
      this.rdoFileStarter.Size = new Size(77, 18);
      this.rdoFileStarter.TabIndex = 1;
      this.rdoFileStarter.TabStop = true;
      this.rdoFileStarter.Text = "File Starter";
      this.rdoFileStarter.UseVisualStyleBackColor = false;
      this.rdoFileStarter.Click += new EventHandler(this.rdoApplyTemplate_Click);
      this.gcUsers.Controls.Add((Control) this.btnSave);
      this.gcUsers.Controls.Add((Control) this.btnReset);
      this.gcUsers.Controls.Add((Control) this.gvUsers);
      this.gcUsers.Controls.Add((Control) this.pnlTitle);
      this.gcUsers.Controls.Add((Control) this.pnlApplyTemplate);
      this.gcUsers.Dock = DockStyle.Fill;
      this.gcUsers.HeaderForeColor = SystemColors.ControlText;
      this.gcUsers.Location = new Point(0, 0);
      this.gcUsers.Name = "gcUsers";
      this.gcUsers.Size = new Size(755, 318);
      this.gcUsers.TabIndex = 0;
      this.gcUsers.Text = "Control user access to Status Online features";
      this.btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSave.BackColor = Color.Transparent;
      this.btnSave.Location = new Point(711, 5);
      this.btnSave.MouseDownImage = (Image) null;
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(16, 16);
      this.btnSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.btnSave.TabIndex = 6;
      this.btnSave.TabStop = false;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnReset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnReset.BackColor = Color.Transparent;
      this.btnReset.Location = new Point(733, 5);
      this.btnReset.MouseDownImage = (Image) null;
      this.btnReset.Name = "btnReset";
      this.btnReset.Size = new Size(16, 16);
      this.btnReset.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.btnReset.TabIndex = 5;
      this.btnReset.TabStop = false;
      this.btnReset.Click += new EventHandler(this.btnReset_Click);
      this.gvUsers.AllowMultiselect = false;
      this.gvUsers.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "colUserID";
      gvColumn1.Text = "User ID";
      gvColumn1.Width = 98;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "colLastName";
      gvColumn2.Text = "Last Name";
      gvColumn2.Width = 89;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "colFirstName";
      gvColumn3.Text = "First Name";
      gvColumn3.Width = 95;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "colPersona";
      gvColumn4.Text = "Persona";
      gvColumn4.Width = 172;
      gvColumn5.CheckBoxes = true;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "colPublish";
      gvColumn5.Text = "Publish Updates";
      gvColumn5.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn5.Width = 94;
      gvColumn6.CheckBoxes = true;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "colPersonal";
      gvColumn6.Text = "Configure Personal Status Online";
      gvColumn6.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn6.Width = 180;
      this.gvUsers.Columns.AddRange(new GVColumn[6]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6
      });
      this.gvUsers.Dock = DockStyle.Fill;
      this.gvUsers.Location = new Point(1, 57);
      this.gvUsers.Name = "gvUsers";
      this.gvUsers.Size = new Size(753, 229);
      this.gvUsers.TabIndex = 1;
      this.gvUsers.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvUsers.SubItemCheck += new GVSubItemEventHandler(this.gvUsers_SubItemCheck);
      this.pnlTitle.Borders = AnchorStyles.Bottom;
      this.pnlTitle.Controls.Add((Control) this.lblTitle);
      this.pnlTitle.Dock = DockStyle.Top;
      this.pnlTitle.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.pnlTitle.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.pnlTitle.Location = new Point(1, 26);
      this.pnlTitle.Name = "pnlTitle";
      this.pnlTitle.Size = new Size(753, 31);
      this.pnlTitle.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.pnlTitle.TabIndex = 0;
      this.lblTitle.AutoSize = true;
      this.lblTitle.BackColor = Color.Transparent;
      this.lblTitle.Location = new Point(8, 8);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Size = new Size(562, 14);
      this.lblTitle.TabIndex = 0;
      this.lblTitle.Text = "Select users that can publish Status Online updates and configure Personal Status Online in their Personal settings.";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcUsers);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (StatusOnlineUsersControl);
      this.Size = new Size(755, 318);
      this.pnlApplyTemplate.ResumeLayout(false);
      this.pnlApplyTemplate.PerformLayout();
      this.gcUsers.ResumeLayout(false);
      ((ISupportInitialize) this.btnSave).EndInit();
      ((ISupportInitialize) this.btnReset).EndInit();
      this.pnlTitle.ResumeLayout(false);
      this.pnlTitle.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
