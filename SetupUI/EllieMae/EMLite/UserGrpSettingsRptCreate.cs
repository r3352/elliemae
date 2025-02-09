// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.UserGrpSettingsRptCreate
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

#nullable disable
namespace EllieMae.EMLite
{
  public class UserGrpSettingsRptCreate : UserControl
  {
    private Sessions.Session session;
    private Panel panel1;
    private Panel panel2;
    private GroupContainer gcUserGroup;
    private ListViewEx listViewUserGroups;
    private ColumnHeader columnCheckBox;
    private ColumnHeader columnPersonaName;
    private System.Windows.Forms.CheckBox internalPersonaChkBx;
    private GroupContainer gcUserGroupRpt;
    private Label label2;
    private System.Windows.Forms.CheckBox selectAll;
    private System.Windows.Forms.CheckBox roleList_chkbx;
    private System.Windows.Forms.CheckBox loanTemplates_chkbx;
    private System.Windows.Forms.CheckBox borrowerContacts_chkbx;
    private System.Windows.Forms.CheckBox resources_chkbx;
    private System.Windows.Forms.CheckBox members_chkbx;
    private System.Windows.Forms.CheckBox loanaccess_chkbx;
    private Label label4;
    private System.Windows.Forms.TextBox reportName;
    private Label label1;
    private AclGroup[] userGroups;
    private IContainer components;

    public List<string> SelectedUserGroup => this.getSelectedUserGroup();

    public string ReportName => this.reportName.Text;

    public bool Members => this.members_chkbx.Checked;

    public bool LoanAccess => this.loanaccess_chkbx.Checked;

    public bool BorrowerContacts => this.borrowerContacts_chkbx.Checked;

    public bool LoanTemplates => this.loanTemplates_chkbx.Checked;

    public bool Resources => this.resources_chkbx.Checked;

    public bool RoleList => this.roleList_chkbx.Checked;

    public UserGrpSettingsRptCreate(
      Sessions.Session session,
      string userid,
      SettingsRptJobInfo jobinfo)
    {
      this.session = session;
      this.InitializeComponent();
      this.populateUserGroups();
      if (jobinfo == null)
        return;
      foreach (ListViewItem listViewItem in this.listViewUserGroups.Items)
      {
        if (jobinfo.reportFilters.Contains(listViewItem.SubItems[1].Text))
          listViewItem.Checked = true;
      }
      this.reportName.Text = "CopyOf_" + jobinfo.ReportName;
      this.members_chkbx.Checked = jobinfo.reportParameters.ContainsKey(nameof (Members)) && Convert.ToBoolean(jobinfo.reportParameters[nameof (Members)]);
      this.loanaccess_chkbx.Checked = jobinfo.reportParameters.ContainsKey(nameof (LoanAccess)) && Convert.ToBoolean(jobinfo.reportParameters[nameof (LoanAccess)]);
      this.borrowerContacts_chkbx.Checked = jobinfo.reportParameters.ContainsKey("BorrowContacts") && Convert.ToBoolean(jobinfo.reportParameters["BorrowContacts"]);
      this.loanTemplates_chkbx.Checked = jobinfo.reportParameters.ContainsKey(nameof (LoanTemplates)) && Convert.ToBoolean(jobinfo.reportParameters[nameof (LoanTemplates)]);
      this.resources_chkbx.Checked = jobinfo.reportParameters.ContainsKey(nameof (Resources)) && Convert.ToBoolean(jobinfo.reportParameters[nameof (Resources)]);
      this.roleList_chkbx.Checked = jobinfo.reportParameters.ContainsKey("RolesList") && Convert.ToBoolean(jobinfo.reportParameters["RolesList"]);
    }

    private void populateUserGroups()
    {
      this.userGroups = this.getUserGroups();
      this.listViewUserGroups.Items.Clear();
      foreach (AclGroup userGroup in this.userGroups)
        this.listViewUserGroups.Items.Add(new ListViewItem(new string[2]
        {
          "",
          userGroup.Name
        }));
    }

    private AclGroup[] getUserGroups()
    {
      AclGroup[] allGroups = this.session.AclGroupManager.GetAllGroups();
      Array.Sort<AclGroup>(allGroups);
      return allGroups;
    }

    private List<string> getSelectedUserGroup()
    {
      List<string> selectedUserGroup = new List<string>();
      foreach (ListViewItem listViewItem in this.listViewUserGroups.Items)
      {
        if (listViewItem.Checked)
          selectedUserGroup.Add(listViewItem.SubItems[1].Text);
      }
      return selectedUserGroup;
    }

    private void listView1_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
    {
      if (e.ColumnIndex == 0)
      {
        e.DrawBackground();
        bool flag = false;
        try
        {
          flag = Convert.ToBoolean(e.Header.Tag);
        }
        catch (Exception ex)
        {
        }
        CheckBoxRenderer.DrawCheckBox(e.Graphics, new Point(e.Bounds.Left + 5, e.Bounds.Top + 2), flag ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal);
      }
      else
        e.DrawDefault = true;
    }

    private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
    {
      if (e.Column != 0)
        return;
      bool flag = false;
      try
      {
        flag = Convert.ToBoolean(this.listViewUserGroups.Columns[e.Column].Tag);
      }
      catch (Exception ex)
      {
      }
      this.listViewUserGroups.Columns[e.Column].Tag = (object) !flag;
      foreach (ListViewItem listViewItem in this.listViewUserGroups.Items)
        listViewItem.Checked = !flag;
      this.listViewUserGroups.Invalidate();
    }

    private void listView1_DrawItem(object sender, DrawListViewItemEventArgs e)
    {
      e.DrawDefault = true;
    }

    private void listView1_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
    {
      e.DrawDefault = true;
    }

    private void InitializeComponent()
    {
      this.panel1 = new Panel();
      this.gcUserGroup = new GroupContainer();
      this.listViewUserGroups = new ListViewEx();
      this.columnCheckBox = new ColumnHeader();
      this.columnPersonaName = new ColumnHeader();
      this.internalPersonaChkBx = new System.Windows.Forms.CheckBox();
      this.panel2 = new Panel();
      this.gcUserGroupRpt = new GroupContainer();
      this.label2 = new Label();
      this.selectAll = new System.Windows.Forms.CheckBox();
      this.roleList_chkbx = new System.Windows.Forms.CheckBox();
      this.loanTemplates_chkbx = new System.Windows.Forms.CheckBox();
      this.borrowerContacts_chkbx = new System.Windows.Forms.CheckBox();
      this.resources_chkbx = new System.Windows.Forms.CheckBox();
      this.members_chkbx = new System.Windows.Forms.CheckBox();
      this.loanaccess_chkbx = new System.Windows.Forms.CheckBox();
      this.label4 = new Label();
      this.reportName = new System.Windows.Forms.TextBox();
      this.label1 = new Label();
      this.panel1.SuspendLayout();
      this.gcUserGroup.SuspendLayout();
      this.panel2.SuspendLayout();
      this.gcUserGroupRpt.SuspendLayout();
      this.SuspendLayout();
      this.panel1.Controls.Add((Control) this.gcUserGroup);
      this.panel1.Location = new Point(3, 3);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(523, 550);
      this.panel1.TabIndex = 0;
      this.gcUserGroup.AutoScroll = true;
      this.gcUserGroup.Controls.Add((Control) this.listViewUserGroups);
      this.gcUserGroup.Controls.Add((Control) this.internalPersonaChkBx);
      this.gcUserGroup.Dock = DockStyle.Fill;
      this.gcUserGroup.HeaderForeColor = SystemColors.ControlText;
      this.gcUserGroup.Location = new Point(0, 0);
      this.gcUserGroup.Name = "gcUserGroup";
      this.gcUserGroup.Size = new Size(523, 550);
      this.gcUserGroup.TabIndex = 10;
      this.gcUserGroup.Text = "1. Choose User Group(s)";
      this.listViewUserGroups.AllowColumnReorder = true;
      this.listViewUserGroups.BorderStyle = BorderStyle.None;
      this.listViewUserGroups.CheckBoxes = true;
      this.listViewUserGroups.Columns.AddRange(new ColumnHeader[2]
      {
        this.columnCheckBox,
        this.columnPersonaName
      });
      this.listViewUserGroups.Dock = DockStyle.Fill;
      this.listViewUserGroups.DoubleClickActivation = false;
      this.listViewUserGroups.FullRowSelect = true;
      this.listViewUserGroups.GridLines = true;
      this.listViewUserGroups.Location = new Point(1, 26);
      this.listViewUserGroups.Name = "listViewUserGroups";
      this.listViewUserGroups.OwnerDraw = true;
      this.listViewUserGroups.Size = new Size(521, 523);
      this.listViewUserGroups.TabIndex = 5;
      this.listViewUserGroups.UseCompatibleStateImageBehavior = false;
      this.listViewUserGroups.View = View.Details;
      this.listViewUserGroups.ColumnClick += new ColumnClickEventHandler(this.listView1_ColumnClick);
      this.listViewUserGroups.DrawColumnHeader += new DrawListViewColumnHeaderEventHandler(this.listView1_DrawColumnHeader);
      this.listViewUserGroups.DrawItem += new DrawListViewItemEventHandler(this.listView1_DrawItem);
      this.listViewUserGroups.DrawSubItem += new DrawListViewSubItemEventHandler(this.listView1_DrawSubItem);
      this.columnCheckBox.Text = "";
      this.columnPersonaName.Text = "User Group Name";
      this.columnPersonaName.Width = 216;
      this.internalPersonaChkBx.AutoSize = true;
      this.internalPersonaChkBx.BackColor = Color.Transparent;
      this.internalPersonaChkBx.Checked = true;
      this.internalPersonaChkBx.CheckState = CheckState.Checked;
      this.internalPersonaChkBx.Location = new Point(183, 5);
      this.internalPersonaChkBx.Name = "internalPersonaChkBx";
      this.internalPersonaChkBx.Size = new Size(256, 21);
      this.internalPersonaChkBx.TabIndex = 0;
      this.internalPersonaChkBx.Text = "Show Personas with Internal Access";
      this.internalPersonaChkBx.UseVisualStyleBackColor = false;
      this.internalPersonaChkBx.Visible = false;
      this.panel2.Controls.Add((Control) this.gcUserGroupRpt);
      this.panel2.Location = new Point(532, 3);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(380, 550);
      this.panel2.TabIndex = 1;
      this.gcUserGroupRpt.BackColor = Color.White;
      this.gcUserGroupRpt.Controls.Add((Control) this.label2);
      this.gcUserGroupRpt.Controls.Add((Control) this.selectAll);
      this.gcUserGroupRpt.Controls.Add((Control) this.roleList_chkbx);
      this.gcUserGroupRpt.Controls.Add((Control) this.loanTemplates_chkbx);
      this.gcUserGroupRpt.Controls.Add((Control) this.borrowerContacts_chkbx);
      this.gcUserGroupRpt.Controls.Add((Control) this.resources_chkbx);
      this.gcUserGroupRpt.Controls.Add((Control) this.members_chkbx);
      this.gcUserGroupRpt.Controls.Add((Control) this.loanaccess_chkbx);
      this.gcUserGroupRpt.Controls.Add((Control) this.label4);
      this.gcUserGroupRpt.Controls.Add((Control) this.reportName);
      this.gcUserGroupRpt.Controls.Add((Control) this.label1);
      this.gcUserGroupRpt.Dock = DockStyle.Fill;
      this.gcUserGroupRpt.HeaderForeColor = SystemColors.ControlText;
      this.gcUserGroupRpt.Location = new Point(0, 0);
      this.gcUserGroupRpt.Name = "gcUserGroupRpt";
      this.gcUserGroupRpt.Size = new Size(380, 550);
      this.gcUserGroupRpt.TabIndex = 11;
      this.gcUserGroupRpt.Text = "2. Select Report Options";
      this.label2.BorderStyle = BorderStyle.Fixed3D;
      this.label2.ForeColor = SystemColors.ControlDark;
      this.label2.Location = new Point(21, 157);
      this.label2.Name = "label2";
      this.label2.Size = new Size(331, 2);
      this.label2.TabIndex = 23;
      this.selectAll.AutoSize = true;
      this.selectAll.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.selectAll.Location = new Point(21, (int) sbyte.MaxValue);
      this.selectAll.Name = "selectAll";
      this.selectAll.Size = new Size(88, 21);
      this.selectAll.TabIndex = 22;
      this.selectAll.Text = "Select All";
      this.selectAll.UseVisualStyleBackColor = true;
      this.selectAll.CheckedChanged += new EventHandler(this.selectAll_CheckedChanged);
      this.roleList_chkbx.AutoSize = true;
      this.roleList_chkbx.Location = new Point(21, 304);
      this.roleList_chkbx.Name = "roleList_chkbx";
      this.roleList_chkbx.Size = new Size(118, 21);
      this.roleList_chkbx.TabIndex = 17;
      this.roleList_chkbx.Text = "Role List View";
      this.roleList_chkbx.UseVisualStyleBackColor = true;
      this.loanTemplates_chkbx.AutoSize = true;
      this.loanTemplates_chkbx.Location = new Point(21, 250);
      this.loanTemplates_chkbx.Name = "loanTemplates_chkbx";
      this.loanTemplates_chkbx.Size = new Size(132, 21);
      this.loanTemplates_chkbx.TabIndex = 13;
      this.loanTemplates_chkbx.Text = "Loan Templates";
      this.loanTemplates_chkbx.UseVisualStyleBackColor = true;
      this.borrowerContacts_chkbx.AutoSize = true;
      this.borrowerContacts_chkbx.Location = new Point(21, 223);
      this.borrowerContacts_chkbx.Name = "borrowerContacts_chkbx";
      this.borrowerContacts_chkbx.Size = new Size(146, 21);
      this.borrowerContacts_chkbx.TabIndex = 12;
      this.borrowerContacts_chkbx.Text = "Borrower Contacts";
      this.borrowerContacts_chkbx.UseVisualStyleBackColor = true;
      this.resources_chkbx.AutoSize = true;
      this.resources_chkbx.Location = new Point(21, 277);
      this.resources_chkbx.Name = "resources_chkbx";
      this.resources_chkbx.Size = new Size(98, 21);
      this.resources_chkbx.TabIndex = 14;
      this.resources_chkbx.Text = "Resources";
      this.resources_chkbx.UseVisualStyleBackColor = true;
      this.members_chkbx.AutoSize = true;
      this.members_chkbx.Checked = true;
      this.members_chkbx.CheckState = CheckState.Checked;
      this.members_chkbx.Location = new Point(21, 169);
      this.members_chkbx.Name = "members_chkbx";
      this.members_chkbx.Size = new Size(88, 21);
      this.members_chkbx.TabIndex = 8;
      this.members_chkbx.Text = "Members";
      this.members_chkbx.UseVisualStyleBackColor = true;
      this.loanaccess_chkbx.AutoSize = true;
      this.loanaccess_chkbx.Location = new Point(21, 196);
      this.loanaccess_chkbx.Name = "loanaccess_chkbx";
      this.loanaccess_chkbx.Size = new Size(111, 21);
      this.loanaccess_chkbx.TabIndex = 9;
      this.loanaccess_chkbx.Text = "Loan Access";
      this.loanaccess_chkbx.UseVisualStyleBackColor = true;
      this.label4.AutoSize = true;
      this.label4.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(18, 107);
      this.label4.Name = "label4";
      this.label4.Size = new Size(161, 17);
      this.label4.TabIndex = 7;
      this.label4.Text = "Include the following:";
      this.reportName.Location = new Point(21, 61);
      this.reportName.Name = "reportName";
      this.reportName.Size = new Size(325, 22);
      this.reportName.TabIndex = 2;
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(18, 41);
      this.label1.Name = "label1";
      this.label1.Size = new Size(103, 17);
      this.label1.TabIndex = 1;
      this.label1.Text = "Report Name";
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.panel1);
      this.Name = nameof (UserGrpSettingsRptCreate);
      this.Size = new Size(917, 556);
      this.panel1.ResumeLayout(false);
      this.gcUserGroup.ResumeLayout(false);
      this.gcUserGroup.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.gcUserGroupRpt.ResumeLayout(false);
      this.gcUserGroupRpt.PerformLayout();
      this.ResumeLayout(false);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void selectAll_CheckedChanged(object sender, EventArgs e)
    {
      if (this.selectAll.Checked)
      {
        this.members_chkbx.Checked = true;
        this.loanaccess_chkbx.Checked = true;
        this.borrowerContacts_chkbx.Checked = true;
        this.roleList_chkbx.Checked = true;
        this.resources_chkbx.Checked = true;
        this.loanTemplates_chkbx.Checked = true;
      }
      else
      {
        this.members_chkbx.Checked = false;
        this.loanaccess_chkbx.Checked = false;
        this.borrowerContacts_chkbx.Checked = false;
        this.roleList_chkbx.Checked = false;
        this.resources_chkbx.Checked = false;
        this.loanTemplates_chkbx.Checked = false;
      }
    }
  }
}
