// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.RoleDetailForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.HelpAPI;
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
  public class RoleDetailForm : Form
  {
    private Label label9;
    private Label label1;
    private IContainer components;
    private GridView gvPersonas;
    private Label label2;
    private Button cancelBtn;
    private Button saveBtn;
    private Label label3;
    private Label label4;
    private TextBox boxAbbr;
    private EMHelpLink emHelpLink1;
    private GroupContainer groupContainer1;
    private FlowLayoutPanel flowLayoutPanel1;
    private StandardIconButton btnRemovePersona;
    private ToolTip toolTip1;
    private StandardIconButton btnAddPersona;
    private GroupContainer groupContainer2;
    private FlowLayoutPanel flowLayoutPanel2;
    private StandardIconButton btnRemoveGroup;
    private StandardIconButton btnAddGroup;
    private GridView gvUserGroups;
    private Label label5;
    private TextBox boxRoleName;
    private RoleInfo role;
    private RoleFunctionPanel parentForm;
    private Sessions.Session session;

    public RoleDetailForm(Sessions.Session session, RoleInfo role, RoleFunctionPanel parent)
    {
      this.session = session;
      this.role = role;
      this.parentForm = parent;
      this.InitializeComponent();
      this.emHelpLink1.AssignSession(this.session);
      if (this.role == null)
        return;
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
      this.boxAbbr = new TextBox();
      this.boxRoleName = new TextBox();
      this.label9 = new Label();
      this.label1 = new Label();
      this.gvPersonas = new GridView();
      this.label4 = new Label();
      this.label3 = new Label();
      this.label2 = new Label();
      this.cancelBtn = new Button();
      this.saveBtn = new Button();
      this.groupContainer1 = new GroupContainer();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.btnRemovePersona = new StandardIconButton();
      this.btnAddPersona = new StandardIconButton();
      this.toolTip1 = new ToolTip(this.components);
      this.btnRemoveGroup = new StandardIconButton();
      this.btnAddGroup = new StandardIconButton();
      this.groupContainer2 = new GroupContainer();
      this.flowLayoutPanel2 = new FlowLayoutPanel();
      this.gvUserGroups = new GridView();
      this.label5 = new Label();
      this.emHelpLink1 = new EMHelpLink();
      this.groupContainer1.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      ((ISupportInitialize) this.btnRemovePersona).BeginInit();
      ((ISupportInitialize) this.btnAddPersona).BeginInit();
      ((ISupportInitialize) this.btnRemoveGroup).BeginInit();
      ((ISupportInitialize) this.btnAddGroup).BeginInit();
      this.groupContainer2.SuspendLayout();
      this.flowLayoutPanel2.SuspendLayout();
      this.SuspendLayout();
      this.boxAbbr.Location = new Point(107, 32);
      this.boxAbbr.MaxLength = 2;
      this.boxAbbr.Name = "boxAbbr";
      this.boxAbbr.Size = new Size(52, 20);
      this.boxAbbr.TabIndex = 2;
      this.boxRoleName.Location = new Point(107, 10);
      this.boxRoleName.MaxLength = 20;
      this.boxRoleName.Name = "boxRoleName";
      this.boxRoleName.Size = new Size(160, 20);
      this.boxRoleName.TabIndex = 1;
      this.label9.AutoSize = true;
      this.label9.Location = new Point(8, 35);
      this.label9.Name = "label9";
      this.label9.Size = new Size(92, 14);
      this.label9.TabIndex = 56;
      this.label9.Text = "Role Abbreviation";
      this.label9.TextAlign = ContentAlignment.MiddleLeft;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(8, 13);
      this.label1.Name = "label1";
      this.label1.Size = new Size(58, 14);
      this.label1.TabIndex = 55;
      this.label1.Text = "Role Name";
      this.label1.TextAlign = ContentAlignment.MiddleLeft;
      this.gvPersonas.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SpringToFit = true;
      gvColumn1.Text = "Personas";
      gvColumn1.Width = 379;
      this.gvPersonas.Columns.AddRange(new GVColumn[1]
      {
        gvColumn1
      });
      this.gvPersonas.Dock = DockStyle.Fill;
      this.gvPersonas.HeaderHeight = 0;
      this.gvPersonas.HeaderVisible = false;
      this.gvPersonas.Location = new Point(1, 26);
      this.gvPersonas.Name = "gvPersonas";
      this.gvPersonas.Size = new Size(379, 126);
      this.gvPersonas.TabIndex = 3;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(165, 36);
      this.label4.Name = "label4";
      this.label4.Size = new Size(77, 14);
      this.label4.TabIndex = 63;
      this.label4.Text = "(2 characters)";
      this.label4.TextAlign = ContentAlignment.MiddleLeft;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(275, 14);
      this.label3.Name = "label3";
      this.label3.Size = new Size(109, 14);
      this.label3.TabIndex = 62;
      this.label3.Text = "(20 characters max.)";
      this.label3.TextAlign = ContentAlignment.MiddleLeft;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(8, 62);
      this.label2.Name = "label2";
      this.label2.Size = new Size(217, 14);
      this.label2.TabIndex = 61;
      this.label2.Text = "Select the users who can perform this role.";
      this.label2.TextAlign = ContentAlignment.MiddleLeft;
      this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(316, 417);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(76, 22);
      this.cancelBtn.TabIndex = 7;
      this.cancelBtn.Text = "&Cancel";
      this.saveBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.saveBtn.Location = new Point(232, 417);
      this.saveBtn.Name = "saveBtn";
      this.saveBtn.Size = new Size(76, 22);
      this.saveBtn.TabIndex = 6;
      this.saveBtn.Text = "&Save";
      this.saveBtn.Click += new EventHandler(this.saveBtn_Click);
      this.groupContainer1.Controls.Add((Control) this.flowLayoutPanel1);
      this.groupContainer1.Controls.Add((Control) this.gvPersonas);
      this.groupContainer1.Location = new Point(10, 79);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(381, 153);
      this.groupContainer1.TabIndex = 65;
      this.groupContainer1.Text = "All Users in These Personas";
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnRemovePersona);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnAddPersona);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(320, 3);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(58, 22);
      this.flowLayoutPanel1.TabIndex = 4;
      this.btnRemovePersona.BackColor = Color.Transparent;
      this.btnRemovePersona.Location = new Point(40, 3);
      this.btnRemovePersona.Margin = new Padding(3, 3, 2, 3);
      this.btnRemovePersona.Name = "btnRemovePersona";
      this.btnRemovePersona.Size = new Size(16, 16);
      this.btnRemovePersona.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnRemovePersona.TabIndex = 0;
      this.btnRemovePersona.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnRemovePersona, "Remove Persona");
      this.btnRemovePersona.Click += new EventHandler(this.btnRemovePersona_Click);
      this.btnAddPersona.BackColor = Color.Transparent;
      this.btnAddPersona.Location = new Point(19, 3);
      this.btnAddPersona.Margin = new Padding(3, 3, 2, 3);
      this.btnAddPersona.Name = "btnAddPersona";
      this.btnAddPersona.Size = new Size(16, 16);
      this.btnAddPersona.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddPersona.TabIndex = 1;
      this.btnAddPersona.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnAddPersona, "Add Persona");
      this.btnAddPersona.Click += new EventHandler(this.btnAddPersona_Click);
      this.btnRemoveGroup.BackColor = Color.Transparent;
      this.btnRemoveGroup.Location = new Point(40, 3);
      this.btnRemoveGroup.Margin = new Padding(3, 3, 2, 3);
      this.btnRemoveGroup.Name = "btnRemoveGroup";
      this.btnRemoveGroup.Size = new Size(16, 16);
      this.btnRemoveGroup.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnRemoveGroup.TabIndex = 0;
      this.btnRemoveGroup.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnRemoveGroup, "Remove User Group");
      this.btnRemoveGroup.Click += new EventHandler(this.btnRemoveGroup_Click);
      this.btnAddGroup.BackColor = Color.Transparent;
      this.btnAddGroup.Location = new Point(19, 3);
      this.btnAddGroup.Margin = new Padding(3, 3, 2, 3);
      this.btnAddGroup.Name = "btnAddGroup";
      this.btnAddGroup.Size = new Size(16, 16);
      this.btnAddGroup.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddGroup.TabIndex = 1;
      this.btnAddGroup.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnAddGroup, "Add User Group");
      this.btnAddGroup.Click += new EventHandler(this.btnAddGroup_Click);
      this.groupContainer2.Controls.Add((Control) this.flowLayoutPanel2);
      this.groupContainer2.Controls.Add((Control) this.gvUserGroups);
      this.groupContainer2.Location = new Point(10, 254);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(381, 153);
      this.groupContainer2.TabIndex = 67;
      this.groupContainer2.Text = "User Groups";
      this.flowLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel2.BackColor = Color.Transparent;
      this.flowLayoutPanel2.Controls.Add((Control) this.btnRemoveGroup);
      this.flowLayoutPanel2.Controls.Add((Control) this.btnAddGroup);
      this.flowLayoutPanel2.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel2.Location = new Point(320, 3);
      this.flowLayoutPanel2.Name = "flowLayoutPanel2";
      this.flowLayoutPanel2.Size = new Size(58, 22);
      this.flowLayoutPanel2.TabIndex = 4;
      this.gvUserGroups.BorderStyle = BorderStyle.None;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column1";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "Personas";
      gvColumn2.Width = 379;
      this.gvUserGroups.Columns.AddRange(new GVColumn[1]
      {
        gvColumn2
      });
      this.gvUserGroups.Dock = DockStyle.Fill;
      this.gvUserGroups.HeaderHeight = 0;
      this.gvUserGroups.HeaderVisible = false;
      this.gvUserGroups.Location = new Point(1, 26);
      this.gvUserGroups.Name = "gvUserGroups";
      this.gvUserGroups.Size = new Size(379, 126);
      this.gvUserGroups.TabIndex = 3;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(8, 237);
      this.label5.Name = "label5";
      this.label5.Size = new Size(248, 14);
      this.label5.TabIndex = 66;
      this.label5.Text = "Select the user groups who can perform this role.";
      this.label5.TextAlign = ContentAlignment.MiddleLeft;
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.HelpTag = "RoleDialog";
      this.emHelpLink1.Location = new Point(10, 420);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 64;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(401, 448);
      this.Controls.Add((Control) this.groupContainer2);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.saveBtn);
      this.Controls.Add((Control) this.label9);
      this.Controls.Add((Control) this.boxRoleName);
      this.Controls.Add((Control) this.boxAbbr);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (RoleDetailForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Create/Edit Role";
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.groupContainer1.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      ((ISupportInitialize) this.btnRemovePersona).EndInit();
      ((ISupportInitialize) this.btnAddPersona).EndInit();
      ((ISupportInitialize) this.btnRemoveGroup).EndInit();
      ((ISupportInitialize) this.btnAddGroup).EndInit();
      this.groupContainer2.ResumeLayout(false);
      this.flowLayoutPanel2.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public RoleInfo Role => this.role;

    private void initForm()
    {
      this.boxRoleName.Text = this.role.RoleName;
      this.boxAbbr.Text = this.role.RoleAbbr;
      this.gvPersonas.Items.Clear();
      for (int index = 0; index < this.role.PersonaIDs.Length; ++index)
      {
        if (this.parentForm.PersonaSettings.ContainsKey((object) this.role.PersonaIDs[index]))
          this.gvPersonas.Items.Add(new GVItem(this.parentForm.PersonaSettings[(object) this.role.PersonaIDs[index]].ToString())
          {
            Tag = (object) this.role.PersonaIDs[index]
          });
      }
      this.gvUserGroups.Items.Clear();
      for (int index = 0; index < this.role.UserGroupIDs.Length; ++index)
      {
        if (this.parentForm.UserGroupSettings.ContainsKey((object) this.role.UserGroupIDs[index]))
          this.gvUserGroups.Items.Add(new GVItem(this.parentForm.UserGroupSettings[(object) this.role.UserGroupIDs[index]].ToString())
          {
            Tag = (object) this.role.UserGroupIDs[index]
          });
      }
    }

    private void btnAddPersona_Click(object sender, EventArgs e)
    {
      int[] availablePersonaList = this.getAvailablePersonaList();
      if (availablePersonaList.Length == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "All personas have been added for this role.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        using (SelectPersonaForm selectPersonaForm = new SelectPersonaForm(availablePersonaList, this))
        {
          if (selectPersonaForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          int[] selectedPersonas = selectPersonaForm.GetSelectedPersonas();
          for (int index = 0; index < selectedPersonas.Length; ++index)
            this.gvPersonas.Items.Add(new GVItem(this.parentForm.PersonaSettings[(object) selectedPersonas[index]].ToString())
            {
              Tag = (object) selectedPersonas[index]
            });
        }
      }
    }

    private int[] getAvailablePersonaList()
    {
      List<int> intList = new List<int>();
      foreach (int key in (IEnumerable) this.parentForm.PersonaSettings.Keys)
      {
        bool flag = false;
        for (int nItemIndex = 0; nItemIndex < this.gvPersonas.Items.Count; ++nItemIndex)
        {
          if ((int) this.gvPersonas.Items[nItemIndex].Tag == key)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          intList.Add(key);
      }
      return intList.ToArray();
    }

    private void btnAddGroup_Click(object sender, EventArgs e)
    {
      int[] availableUserGroupList = this.getAvailableUserGroupList();
      if (availableUserGroupList.Length == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "All User Groups have been added for this role.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        using (SelectUserGroupForm selectUserGroupForm = new SelectUserGroupForm(availableUserGroupList, this))
        {
          if (selectUserGroupForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          int[] selectedGroups = selectUserGroupForm.GetSelectedGroups();
          for (int index = 0; index < selectedGroups.Length; ++index)
            this.gvUserGroups.Items.Add(new GVItem(this.parentForm.UserGroupSettings[(object) selectedGroups[index]].ToString())
            {
              Tag = (object) selectedGroups[index]
            });
        }
      }
    }

    private int[] getAvailableUserGroupList()
    {
      List<int> intList = new List<int>();
      foreach (int key in (IEnumerable) this.parentForm.UserGroupSettings.Keys)
      {
        bool flag = false;
        for (int nItemIndex = 0; nItemIndex < this.gvUserGroups.Items.Count; ++nItemIndex)
        {
          if ((int) this.gvUserGroups.Items[nItemIndex].Tag == key)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          intList.Add(key);
      }
      return intList.ToArray();
    }

    private void btnRemovePersona_Click(object sender, EventArgs e)
    {
      if (this.gvPersonas.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select one or more Personas to be removed.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "Are you sure you want to remove selected Persona(s)?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
          return;
        while (this.gvPersonas.SelectedItems.Count > 0)
          this.gvPersonas.Items.Remove(this.gvPersonas.SelectedItems[0]);
      }
    }

    private void btnRemoveGroup_Click(object sender, EventArgs e)
    {
      if (this.gvUserGroups.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select one or more User Groups to be removed.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "Are you sure you want to remove selected User Group(s)?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
          return;
        while (this.gvUserGroups.SelectedItems.Count > 0)
          this.gvUserGroups.Items.Remove(this.gvUserGroups.SelectedItems[0]);
      }
    }

    private void saveBtn_Click(object sender, EventArgs e)
    {
      string str = this.boxRoleName.Text.Trim();
      string abbreviation = this.boxAbbr.Text.Trim();
      if (str == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The role name cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.boxRoleName.Focus();
      }
      else if (abbreviation == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The role abbreviation cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.boxAbbr.Focus();
      }
      else if (abbreviation.Length < 2)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The role abbreviation should be two characters.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.boxAbbr.Focus();
      }
      else if (this.gvPersonas.Items.Count == 0 && this.gvUserGroups.Items.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You have to select at least one Persona or User Group for the Role.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        RoleInfo[] allRoleFunctions = ((WorkflowManager) this.session.BPM.GetBpmManager(BpmCategory.Workflow)).GetAllRoleFunctions();
        for (int index = 0; index < allRoleFunctions.Length; ++index)
        {
          if (string.Compare(allRoleFunctions[index].RoleName, str, true) == 0 && (this.role == null || this.role.RoleID != allRoleFunctions[index].RoleID))
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "The role name already exists. Please use a different role name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            this.boxRoleName.Focus();
            return;
          }
        }
        int[] personaIDs = new int[this.gvPersonas.Items.Count];
        for (int nItemIndex = 0; nItemIndex < this.gvPersonas.Items.Count; ++nItemIndex)
          personaIDs[nItemIndex] = (int) this.gvPersonas.Items[nItemIndex].Tag;
        int[] groupIDs = new int[this.gvUserGroups.Items.Count];
        for (int nItemIndex = 0; nItemIndex < this.gvUserGroups.Items.Count; ++nItemIndex)
          groupIDs[nItemIndex] = (int) this.gvUserGroups.Items[nItemIndex].Tag;
        this.role = this.role != null ? new RoleInfo(this.role.RoleID, str, abbreviation, this.role.Protected, personaIDs, groupIDs) : new RoleInfo(str, abbreviation, false, personaIDs, groupIDs);
        this.DialogResult = DialogResult.OK;
      }
    }

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, "RoleDialog");
    }

    public Hashtable PersonaSettings => this.parentForm.PersonaSettings;

    public Hashtable UserGroupSettings => this.parentForm.UserGroupSettings;
  }
}
