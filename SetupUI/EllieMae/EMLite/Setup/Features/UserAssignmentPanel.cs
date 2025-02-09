// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.Features.UserAssignmentPanel
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.EMLite.WebServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Web.Services.Protocols;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.Features
{
  public class UserAssignmentPanel : UserControl
  {
    private bool suspendItemCheckEventHandler;
    private Hashtable moduleUsers = CollectionsUtil.CreateCaseInsensitiveHashtable();
    private bool isDirty;
    private string numInUse;
    private ModuleLicense license;
    private EncompassModule module;
    private int companyLicenseCount;
    private int companyInUseCount;
    private int personalLicenseCount;
    private int personalInUseCount;
    private LabelEx lblDesc;
    private GridView lvwUsers;
    private GroupContainer gcLicMgnt;
    private Button btnDeselectAll;
    private VerticalSeparator verticalSeparator1;
    private StandardIconButton stdIconBtnSave;
    private StandardIconButton stdIconBtnReset;
    private Button btnSelectAll;
    private System.ComponentModel.Container components;

    public UserAssignmentPanel(EncompassModule module, ModuleLicense license)
    {
      this.InitializeComponent();
      this.module = module;
      this.license = license;
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
      this.lblDesc = new LabelEx();
      this.lvwUsers = new GridView();
      this.gcLicMgnt = new GroupContainer();
      this.btnSelectAll = new Button();
      this.btnDeselectAll = new Button();
      this.verticalSeparator1 = new VerticalSeparator();
      this.stdIconBtnSave = new StandardIconButton();
      this.stdIconBtnReset = new StandardIconButton();
      this.gcLicMgnt.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnSave).BeginInit();
      ((ISupportInitialize) this.stdIconBtnReset).BeginInit();
      this.SuspendLayout();
      this.lblDesc.Dock = DockStyle.Top;
      this.lblDesc.Location = new Point(1, 26);
      this.lblDesc.Name = "lblDesc";
      this.lblDesc.Size = new Size(642, 32);
      this.lblDesc.TabIndex = 3;
      this.lblDesc.Text = "  Enable/Disable this add-on for the following user(s).";
      this.lblDesc.TextAlign = ContentAlignment.MiddleLeft;
      this.lvwUsers.AllowColumnReorder = true;
      this.lvwUsers.AllowMultiselect = false;
      this.lvwUsers.BorderStyle = BorderStyle.None;
      gvColumn1.CheckBoxes = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "User ID";
      gvColumn1.Width = 98;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Last Name";
      gvColumn2.Width = 89;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "First Name";
      gvColumn3.Width = 95;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "Organization";
      gvColumn4.Width = 172;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.Text = "Self-Paid";
      gvColumn5.Width = 100;
      this.lvwUsers.Columns.AddRange(new GVColumn[5]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5
      });
      this.lvwUsers.Dock = DockStyle.Fill;
      this.lvwUsers.Location = new Point(1, 58);
      this.lvwUsers.Name = "lvwUsers";
      this.lvwUsers.Size = new Size(642, 358);
      this.lvwUsers.TabIndex = 4;
      this.lvwUsers.SubItemCheck += new GVSubItemEventHandler(this.lvwUsers_SubItemCheck);
      this.gcLicMgnt.Controls.Add((Control) this.btnSelectAll);
      this.gcLicMgnt.Controls.Add((Control) this.btnDeselectAll);
      this.gcLicMgnt.Controls.Add((Control) this.verticalSeparator1);
      this.gcLicMgnt.Controls.Add((Control) this.stdIconBtnSave);
      this.gcLicMgnt.Controls.Add((Control) this.stdIconBtnReset);
      this.gcLicMgnt.Controls.Add((Control) this.lvwUsers);
      this.gcLicMgnt.Controls.Add((Control) this.lblDesc);
      this.gcLicMgnt.Dock = DockStyle.Fill;
      this.gcLicMgnt.Location = new Point(0, 0);
      this.gcLicMgnt.Name = "gcLicMgnt";
      this.gcLicMgnt.Size = new Size(644, 417);
      this.gcLicMgnt.TabIndex = 18;
      this.btnSelectAll.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSelectAll.Location = new Point(437, 3);
      this.btnSelectAll.Name = "btnSelectAll";
      this.btnSelectAll.Size = new Size(75, 21);
      this.btnSelectAll.TabIndex = 9;
      this.btnSelectAll.Text = "Select All";
      this.btnSelectAll.UseVisualStyleBackColor = true;
      this.btnSelectAll.Click += new EventHandler(this.btnSelectAll_Click);
      this.btnDeselectAll.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDeselectAll.Location = new Point(514, 3);
      this.btnDeselectAll.Name = "btnDeselectAll";
      this.btnDeselectAll.Size = new Size(75, 21);
      this.btnDeselectAll.TabIndex = 8;
      this.btnDeselectAll.Text = "Deselect All";
      this.btnDeselectAll.UseVisualStyleBackColor = true;
      this.btnDeselectAll.Click += new EventHandler(this.btnDeselectAll_Click);
      this.verticalSeparator1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.verticalSeparator1.Location = new Point(593, 5);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 7;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.stdIconBtnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnSave.BackColor = Color.Transparent;
      this.stdIconBtnSave.Location = new Point(600, 5);
      this.stdIconBtnSave.Name = "stdIconBtnSave";
      this.stdIconBtnSave.Size = new Size(16, 16);
      this.stdIconBtnSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.stdIconBtnSave.TabIndex = 6;
      this.stdIconBtnSave.TabStop = false;
      this.stdIconBtnSave.Click += new EventHandler(this.stdIconBtnSave_Click);
      this.stdIconBtnReset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnReset.BackColor = Color.Transparent;
      this.stdIconBtnReset.Location = new Point(622, 5);
      this.stdIconBtnReset.Name = "stdIconBtnReset";
      this.stdIconBtnReset.Size = new Size(16, 16);
      this.stdIconBtnReset.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.stdIconBtnReset.TabIndex = 5;
      this.stdIconBtnReset.TabStop = false;
      this.stdIconBtnReset.Click += new EventHandler(this.stdIconBtnReset_Click);
      this.Controls.Add((Control) this.gcLicMgnt);
      this.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (UserAssignmentPanel);
      this.Size = new Size(644, 417);
      this.Load += new EventHandler(this.UserAssignmentPanel_Load);
      this.gcLicMgnt.ResumeLayout(false);
      ((ISupportInitialize) this.stdIconBtnSave).EndInit();
      ((ISupportInitialize) this.stdIconBtnReset).EndInit();
      this.ResumeLayout(false);
    }

    public bool IsDirty => this.isDirty;

    private void enableDisableButtons()
    {
      this.stdIconBtnSave.Enabled = this.stdIconBtnReset.Enabled = this.IsDirty;
    }

    private void initUserList(ModuleLicense license)
    {
      if (license == null)
        return;
      this.moduleUsers.Clear();
      this.lvwUsers.SelectedItems.Clear();
      this.lvwUsers.Items.Clear();
      OrgInfo[] allOrganizations = Session.OrganizationManager.GetAllOrganizations();
      Hashtable hashtable = new Hashtable();
      foreach (OrgInfo orgInfo in allOrganizations)
        hashtable.Add((object) orgInfo.Oid, (object) orgInfo.OrgName);
      Hashtable insensitiveHashtable1 = CollectionsUtil.CreateCaseInsensitiveHashtable();
      Hashtable insensitiveHashtable2 = CollectionsUtil.CreateCaseInsensitiveHashtable();
      this.personalLicenseCount = 0;
      this.personalInUseCount = 0;
      this.companyLicenseCount = 0;
      this.companyInUseCount = 0;
      if (license.UserList != null)
      {
        for (int index = 0; index < license.UserList.Length; ++index)
        {
          ModuleUser user = license.UserList[index];
          if (user.PersonalLicense)
            ++this.personalLicenseCount;
          if (!user.Disabled && user.PersonalLicense)
            ++this.personalInUseCount;
          else if (!user.Disabled)
            ++this.companyInUseCount;
          insensitiveHashtable1.Add((object) user.UserID, (object) user);
        }
      }
      foreach (UserInfo allUser in Session.OrganizationManager.GetAllUsers())
      {
        GVItem gvItem = new GVItem(new string[5]
        {
          allUser.Userid,
          allUser.LastName,
          allUser.FirstName,
          string.Concat(hashtable[(object) allUser.OrgId]),
          "No"
        });
        gvItem.Tag = (object) allUser;
        if (insensitiveHashtable1.Contains((object) allUser.Userid))
        {
          ModuleUser moduleUser = (ModuleUser) insensitiveHashtable1[(object) allUser.Userid];
          gvItem.SubItems[4].Text = moduleUser.PersonalLicense ? "Yes" : "No";
          gvItem.Checked = !moduleUser.Disabled;
        }
        this.moduleUsers.Add((object) allUser.Userid, (object) gvItem.Checked);
        this.lvwUsers.Items.Add(gvItem);
        insensitiveHashtable2.Add((object) allUser.Userid, (object) null);
      }
      if (license.UserList != null)
      {
        foreach (ModuleUser user in license.UserList)
        {
          if ((!user.Disabled || user.PersonalLicense) && !insensitiveHashtable2.Contains((object) user.UserID))
          {
            GVItem gvItem = new GVItem(new string[5]
            {
              "(" + user.UserID + ")",
              "",
              "",
              "(Not Recognized)",
              user.PersonalLicense ? "Yes" : "No"
            });
            gvItem.Tag = (object) user.UserID;
            gvItem.Checked = !user.Disabled;
            gvItem.ForeColor = Color.Red;
            this.lvwUsers.Items.Add(gvItem);
            this.moduleUsers.Add((object) user.UserID, (object) gvItem.Checked);
          }
        }
      }
      this.companyLicenseCount = license.UserLimit != 0 ? Math.Max(license.UserLimit - this.personalLicenseCount, 0) : -1;
      this.updateInUseCount();
      this.lvwUsers.Sort(0, SortOrder.Ascending);
      this.isDirty = false;
      this.enableDisableButtons();
    }

    private void changeBackgroundColor()
    {
      for (int nItemIndex = 0; nItemIndex < this.lvwUsers.Items.Count; ++nItemIndex)
        this.lvwUsers.Items[nItemIndex].BackColor = nItemIndex % 2 != 0 ? EncompassColors.Neutral3 : SystemColors.Window;
    }

    private void lvwUsers_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      if (this.suspendItemCheckEventHandler)
        return;
      this.processSubItemCheckedChanged(e.SubItem);
    }

    private bool processSubItemCheckedChanged(GVSubItem subItem)
    {
      string str = string.Empty;
      str = (object) (subItem.Item.Tag as UserInfo) == null ? subItem.Item.Tag.ToString() : ((UserInfo) subItem.Item.Tag).Userid;
      bool flag = false;
      if (subItem.Checked)
      {
        if (subItem.Item.SubItems[4].Text == "Yes")
          ++this.personalInUseCount;
        else if (this.companyLicenseCount >= 0 && this.companyInUseCount >= this.companyLicenseCount)
        {
          int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "Company license limit exceeded", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          bool checkEventHandler = this.suspendItemCheckEventHandler;
          this.suspendItemCheckEventHandler = true;
          subItem.Checked = false;
          this.suspendItemCheckEventHandler = checkEventHandler;
          flag = true;
        }
        else
          ++this.companyInUseCount;
      }
      else if (subItem.Item.SubItems[4].Text == "Yes")
        --this.personalInUseCount;
      else
        --this.companyInUseCount;
      this.updateInUseCount();
      if (!flag)
        this.isDirty = true;
      this.enableDisableButtons();
      return flag;
    }

    public void Reset()
    {
      ModuleLicense license = this.license ?? Modules.GetModuleLicense(this.module);
      try
      {
        this.suspendItemCheckEventHandler = true;
        this.initUserList(license);
        this.isDirty = false;
      }
      finally
      {
        this.suspendItemCheckEventHandler = false;
      }
    }

    public void Save()
    {
      if (!this.isDirty)
        return;
      try
      {
        List<string> stringList1 = new List<string>();
        List<string> stringList2 = new List<string>();
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.lvwUsers.Items)
        {
          string key = (object) (gvItem.Tag as UserInfo) == null ? gvItem.Tag.ToString() : ((UserInfo) gvItem.Tag).Userid;
          bool flag = gvItem.SubItems[0].Checked;
          if ((bool) this.moduleUsers[(object) key] != flag)
          {
            if (flag)
              stringList1.Add(key);
            else
              stringList2.Add(key);
            this.moduleUsers[(object) key] = (object) flag;
          }
        }
        Modules.UpdateModuleUsers(this.module, stringList1.ToArray(), stringList2.ToArray());
        this.isDirty = false;
        this.enableDisableButtons();
      }
      catch (SoapException ex)
      {
        string text = ex.Message;
        if (ex.Message.IndexOf("--> ") > 0)
          text = ex.Message.Substring(ex.Message.IndexOf("--> ") + 4);
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.Reset();
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "Operation failed due to error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.Reset();
      }
    }

    private void updateInUseCount()
    {
      this.numInUse = this.companyInUseCount.ToString();
      this.gcLicMgnt.Text = "Enabled Users (" + this.numInUse + ")          Allowed Licenses (" + (this.companyLicenseCount >= 0 ? string.Concat((object) this.companyLicenseCount) : "Unlimited") + ")";
    }

    private void UserAssignmentPanel_Load(object sender, EventArgs e)
    {
      this.Reset();
      this.license = (ModuleLicense) null;
    }

    private void stdIconBtnSave_Click(object sender, EventArgs e)
    {
      using (CursorActivator.Wait())
        this.Save();
    }

    private void stdIconBtnReset_Click(object sender, EventArgs e)
    {
      if (ResetConfirmDialog.ShowDialog((IWin32Window) this, (string) null) == DialogResult.No)
        return;
      using (CursorActivator.Wait())
        this.Reset();
    }

    private void btnSelectAll_Click(object sender, EventArgs e)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.lvwUsers.Items)
      {
        if (!gvItem.SubItems[0].Checked)
        {
          bool checkEventHandler = this.suspendItemCheckEventHandler;
          this.suspendItemCheckEventHandler = true;
          gvItem.SubItems[0].Checked = true;
          this.suspendItemCheckEventHandler = checkEventHandler;
          if (this.processSubItemCheckedChanged(gvItem.SubItems[0]))
            break;
        }
      }
    }

    private void btnDeselectAll_Click(object sender, EventArgs e)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.lvwUsers.Items)
      {
        if (gvItem.SubItems[0].Checked)
          gvItem.SubItems[0].Checked = false;
      }
    }
  }
}
