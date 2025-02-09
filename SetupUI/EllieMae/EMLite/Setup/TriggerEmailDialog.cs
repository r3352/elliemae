// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.TriggerEmailDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class TriggerEmailDialog : Form
  {
    private Sessions.Session session;
    private IContainer components;
    private Label label1;
    private TextBox txtSubject;
    private TextBox txtBody;
    private Label label2;
    private Label label3;
    private Label label4;
    private ListBox lstUsers;
    private ListBox lstRoles;
    private Label label5;
    private DialogButtons dlgButtons;
    private Label label6;
    private Button btnAddUser;
    private Button btnRemoveUser;
    private Button btnRemoveRole;
    private Button btnAddRole;
    private CheckBox chkDisplayInLog;

    public TriggerEmailDialog(Sessions.Session session)
    {
      this.session = session;
      this.InitializeComponent();
    }

    public TriggerEmailDialog(TriggerEmailTemplate template, Sessions.Session session)
      : this(session)
    {
      this.loadTemplate(template);
    }

    private void loadTemplate(TriggerEmailTemplate template)
    {
      this.txtSubject.Text = template.Subject;
      this.txtBody.Text = template.Body;
      this.lstUsers.Items.Clear();
      if (template.RecipientUsers.Length != 0)
      {
        Hashtable users = this.session.OrganizationManager.GetUsers(template.RecipientUsers, true);
        foreach (string recipientUser in template.RecipientUsers)
        {
          if (users.ContainsKey((object) recipientUser) && users[(object) recipientUser] != null)
            this.lstUsers.Items.Add(users[(object) recipientUser]);
        }
      }
      this.lstRoles.Items.Clear();
      if (template.RecipientRoles.Length != 0)
      {
        Hashtable roleLookupTable = this.getRoleLookupTable();
        foreach (int recipientRole in template.RecipientRoles)
        {
          if (roleLookupTable.ContainsKey((object) recipientRole) && roleLookupTable[(object) recipientRole] != null)
            this.lstRoles.Items.Add(roleLookupTable[(object) recipientRole]);
        }
      }
      this.chkDisplayInLog.Checked = template.DisplayInLog;
    }

    private Hashtable getRoleLookupTable()
    {
      RoleInfo[] allRoleFunctions = ((WorkflowManager) this.session.BPM.GetBpmManager(BpmCategory.Workflow)).GetAllRoleFunctions();
      Hashtable roleLookupTable = new Hashtable();
      roleLookupTable[(object) RoleInfo.FileStarter.RoleID] = (object) RoleInfo.FileStarter;
      foreach (RoleInfo roleInfo in allRoleFunctions)
        roleLookupTable[(object) roleInfo.ID] = (object) roleInfo;
      return roleLookupTable;
    }

    public TriggerEmailTemplate GetEmailTemplate()
    {
      List<string> stringList = new List<string>();
      foreach (UserInfoSummary userInfoSummary in this.lstUsers.Items)
        stringList.Add(userInfoSummary.UserID);
      List<int> intList = new List<int>();
      foreach (RoleInfo roleInfo in this.lstRoles.Items)
        intList.Add(roleInfo.ID);
      return new TriggerEmailTemplate(this.txtSubject.Text, this.txtBody.Text, stringList.ToArray(), intList.ToArray(), this.chkDisplayInLog.Checked);
    }

    private void btnAddUser_Click(object sender, EventArgs e)
    {
      using (UserPicker userPicker = new UserPicker(this.session.OrganizationManager.GetAllUserInfoSummary(), true))
      {
        if (userPicker.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        foreach (UserInfoSummary selectedUserInfoSummary in userPicker.GetSelectedUserInfoSummaries())
        {
          if (!this.lstUsers.Items.Contains((object) selectedUserInfoSummary))
            this.lstUsers.Items.Add((object) selectedUserInfoSummary);
        }
      }
    }

    private void btnRemoveUser_Click(object sender, EventArgs e)
    {
      if (this.lstUsers.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select at least one item to be removed from the list.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        while (this.lstUsers.SelectedItems.Count > 0)
          this.lstUsers.Items.Remove(this.lstUsers.SelectedItems[0]);
      }
    }

    private void btnAddRole_Click(object sender, EventArgs e)
    {
      WorkflowManager bpmManager = (WorkflowManager) this.session.BPM.GetBpmManager(BpmCategory.Workflow);
      List<RoleInfo> roleInfoList = new List<RoleInfo>();
      roleInfoList.Add(RoleInfo.FileStarter);
      roleInfoList.AddRange((IEnumerable<RoleInfo>) bpmManager.GetAllRoleFunctions());
      using (RolePicker rolePicker = new RolePicker((RoleSummaryInfo[]) roleInfoList.ToArray(), true))
      {
        if (rolePicker.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        foreach (RoleInfo selectedRole in rolePicker.GetSelectedRoles())
        {
          if (!this.lstRoles.Items.Contains((object) selectedRole))
            this.lstRoles.Items.Add((object) selectedRole);
        }
      }
    }

    private void btnRemoveRole_Click(object sender, EventArgs e)
    {
      if (this.lstRoles.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select at least one item to be removed from the list.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        while (this.lstRoles.SelectedItems.Count > 0)
          this.lstRoles.Items.Remove(this.lstRoles.SelectedItems[0]);
      }
    }

    private void dlgButtons_OK(object sender, EventArgs e)
    {
      if (this.txtSubject.Text == "")
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must provide a subject for the email message to be sent.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else if (this.lstUsers.Items.Count == 0 && this.lstRoles.Items.Count == 0)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "You must select at least one user or one role as the recipient for this email.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
        this.DialogResult = DialogResult.OK;
    }

    private void TriggerEmailDialog_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Escape)
        return;
      this.dlgButtons.CancelButton.PerformClick();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.txtSubject = new TextBox();
      this.txtBody = new TextBox();
      this.label2 = new Label();
      this.label3 = new Label();
      this.label4 = new Label();
      this.lstUsers = new ListBox();
      this.lstRoles = new ListBox();
      this.label5 = new Label();
      this.dlgButtons = new DialogButtons();
      this.label6 = new Label();
      this.btnAddUser = new Button();
      this.btnRemoveUser = new Button();
      this.btnRemoveRole = new Button();
      this.btnAddRole = new Button();
      this.chkDisplayInLog = new CheckBox();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(6, 49);
      this.label1.Name = "label1";
      this.label1.Size = new Size(43, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "Subject";
      this.txtSubject.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtSubject.Location = new Point(58, 46);
      this.txtSubject.MaxLength = 250;
      this.txtSubject.Name = "txtSubject";
      this.txtSubject.Size = new Size(412, 20);
      this.txtSubject.TabIndex = 1;
      this.txtBody.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBody.Location = new Point(58, 70);
      this.txtBody.Multiline = true;
      this.txtBody.Name = "txtBody";
      this.txtBody.ScrollBars = ScrollBars.Both;
      this.txtBody.Size = new Size(412, 141);
      this.txtBody.TabIndex = 3;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(6, 72);
      this.label2.Name = "label2";
      this.label2.Size = new Size(32, 14);
      this.label2.TabIndex = 2;
      this.label2.Text = "Body";
      this.label3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(6, 237);
      this.label3.Name = "label3";
      this.label3.Size = new Size(312, 14);
      this.label3.TabIndex = 4;
      this.label3.Text = "Select the user(s) and/or role(s) that will receive this message:";
      this.label4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(6, 257);
      this.label4.Name = "label4";
      this.label4.Size = new Size(36, 14);
      this.label4.TabIndex = 5;
      this.label4.Text = "Users";
      this.lstUsers.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lstUsers.FormattingEnabled = true;
      this.lstUsers.ItemHeight = 14;
      this.lstUsers.Location = new Point(58, 256);
      this.lstUsers.Name = "lstUsers";
      this.lstUsers.SelectionMode = SelectionMode.MultiExtended;
      this.lstUsers.Size = new Size(150, 88);
      this.lstUsers.TabIndex = 6;
      this.lstRoles.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.lstRoles.FormattingEnabled = true;
      this.lstRoles.ItemHeight = 14;
      this.lstRoles.Location = new Point(292, 256);
      this.lstRoles.Name = "lstRoles";
      this.lstRoles.SelectionMode = SelectionMode.MultiExtended;
      this.lstRoles.Size = new Size(156, 88);
      this.lstRoles.TabIndex = 8;
      this.label5.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(249, 257);
      this.label5.Name = "label5";
      this.label5.Size = new Size(34, 14);
      this.label5.TabIndex = 7;
      this.label5.Text = "Roles";
      this.dlgButtons.Dock = DockStyle.Bottom;
      this.dlgButtons.Location = new Point(0, 357);
      this.dlgButtons.Name = "dlgButtons";
      this.dlgButtons.Size = new Size(480, 47);
      this.dlgButtons.TabIndex = 9;
      this.dlgButtons.OK += new EventHandler(this.dlgButtons_OK);
      this.label6.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label6.Location = new Point(6, 7);
      this.label6.Name = "label6";
      this.label6.Size = new Size(464, 32);
      this.label6.TabIndex = 10;
      this.label6.Text = "The Subject and Body can include loan data by specifying the Field ID within square brackets (e.g. [1109]).";
      this.btnAddUser.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnAddUser.Location = new Point(208, (int) byte.MaxValue);
      this.btnAddUser.Name = "btnAddUser";
      this.btnAddUser.Size = new Size(20, 22);
      this.btnAddUser.TabIndex = 11;
      this.btnAddUser.Text = "+";
      this.btnAddUser.UseVisualStyleBackColor = true;
      this.btnAddUser.Click += new EventHandler(this.btnAddUser_Click);
      this.btnRemoveUser.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnRemoveUser.Location = new Point(208, 277);
      this.btnRemoveUser.Name = "btnRemoveUser";
      this.btnRemoveUser.Size = new Size(20, 22);
      this.btnRemoveUser.TabIndex = 12;
      this.btnRemoveUser.Text = "-";
      this.btnRemoveUser.UseVisualStyleBackColor = true;
      this.btnRemoveUser.Click += new EventHandler(this.btnRemoveUser_Click);
      this.btnRemoveRole.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnRemoveRole.Location = new Point(450, 277);
      this.btnRemoveRole.Name = "btnRemoveRole";
      this.btnRemoveRole.Size = new Size(20, 22);
      this.btnRemoveRole.TabIndex = 14;
      this.btnRemoveRole.Text = "-";
      this.btnRemoveRole.UseVisualStyleBackColor = true;
      this.btnRemoveRole.Click += new EventHandler(this.btnRemoveRole_Click);
      this.btnAddRole.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnAddRole.Location = new Point(450, (int) byte.MaxValue);
      this.btnAddRole.Name = "btnAddRole";
      this.btnAddRole.Size = new Size(20, 22);
      this.btnAddRole.TabIndex = 13;
      this.btnAddRole.Text = "+";
      this.btnAddRole.UseVisualStyleBackColor = true;
      this.btnAddRole.Click += new EventHandler(this.btnAddRole_Click);
      this.chkDisplayInLog.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.chkDisplayInLog.AutoSize = true;
      this.chkDisplayInLog.Location = new Point(58, 213);
      this.chkDisplayInLog.Name = "chkDisplayInLog";
      this.chkDisplayInLog.Size = new Size(203, 18);
      this.chkDisplayInLog.TabIndex = 15;
      this.chkDisplayInLog.Text = "Display email information in Loan Log";
      this.chkDisplayInLog.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(480, 404);
      this.Controls.Add((Control) this.chkDisplayInLog);
      this.Controls.Add((Control) this.btnRemoveRole);
      this.Controls.Add((Control) this.btnAddRole);
      this.Controls.Add((Control) this.btnRemoveUser);
      this.Controls.Add((Control) this.btnAddUser);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.dlgButtons);
      this.Controls.Add((Control) this.lstRoles);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.lstUsers);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.txtBody);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.txtSubject);
      this.Controls.Add((Control) this.label1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.KeyPreview = true;
      this.MinimizeBox = false;
      this.Name = nameof (TriggerEmailDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Email Trigger Settings";
      this.KeyDown += new KeyEventHandler(this.TriggerEmailDialog_KeyDown);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
