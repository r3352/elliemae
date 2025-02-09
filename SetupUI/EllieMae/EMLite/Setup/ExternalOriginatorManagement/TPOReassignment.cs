// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.TPOReassignment
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.ExternalOriginatorManagement
{
  public class TPOReassignment : UserControl, IOnlineHelpTarget
  {
    private Sessions.Session session;
    private SetUpContainer setupContainer;
    private UserInfo[] userList;
    private int[] selectedExternalOrg;
    private string[] selectedExternalUser;
    private AnimationProgress an;
    private Task<bool> t;
    private SalesRepInformation salesRepInfo;
    private GVItem selectedUser;
    private Dictionary<int, List<int>> orgParentChildMap = new Dictionary<int, List<int>>();
    private Dictionary<int, List<string>> orgChildContact = new Dictionary<int, List<string>>();
    private const string TPO = "Third Party Originators";
    private IContainer components;
    private PanelEx panelEx1;
    private PanelEx panelEx2;
    private GroupContainer grpContTPOOrgAndContacts;
    private GridView grdViewTPOOrgAndContact;
    private Label lblAssignedTo;
    private Splitter splitter1;
    private GroupContainer grpCntTPOReassign;
    private Button btnReassign;
    private GridView grdViewReassignSalesRep;
    private StandardIconButton popUpButton;
    private TextBox textBox1;

    public TPOReassignment(Sessions.Session session, SetUpContainer setupContainer)
    {
      this.InitializeComponent();
      this.session = session;
      this.setupContainer = setupContainer;
      this.userList = this.session.OrganizationManager.GetAllAccessibleSalesRepUsers();
    }

    private void PopulateSalesRep(UserInfo[] userList)
    {
      this.grdViewReassignSalesRep.Items.Clear();
      if (this.grdViewTPOOrgAndContact.Items.Any<GVItem>())
      {
        foreach (UserInfo user in userList)
        {
          string str = string.Join(",", ((IEnumerable<Persona>) user.UserPersonas).Select<Persona, string>((Func<Persona, string>) (p => p.Name)).ToArray<string>());
          this.grdViewReassignSalesRep.Items.Add(new GVItem(new string[3]
          {
            user.FullName + "(" + user.Userid + ")",
            str,
            user.Userid
          }));
        }
      }
      this.grdViewReassignSalesRep.Refresh();
      this.btnReassign.Enabled = false;
    }

    private void gcTPOOrganisationAndContacts_Paint(object sender, PaintEventArgs e)
    {
    }

    private void cmbAssignedTo_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.PopulateTPOOrgAndContactGridView();
    }

    private void PopulateTPOOrgAndContactGridView()
    {
      UserInfo[] array = ((IEnumerable<UserInfo>) this.userList).Where<UserInfo>((Func<UserInfo, bool>) (u => u.Userid != this.selectedUser.SubItems[1].Text)).ToArray<UserInfo>();
      this.orgParentChildMap = new Dictionary<int, List<int>>();
      this.orgChildContact = new Dictionary<int, List<string>>();
      List<ExternalOriginatorManagementData> withPrimarySalesRep = this.session.ConfigurationManager.GetExternalOrganizationBySalesRepWithPrimarySalesRep(this.selectedUser.SubItems[1].Text);
      this.selectedExternalOrg = withPrimarySalesRep.Select<ExternalOriginatorManagementData, int>((Func<ExternalOriginatorManagementData, int>) (a => a.oid)).ToArray<int>();
      List<ExternalUserInfo> userInfoBySalesRep = this.session.ConfigurationManager.GetExternalUserInfoBySalesRep(this.selectedUser.SubItems[1].Text);
      this.selectedExternalUser = userInfoBySalesRep.Select<ExternalUserInfo, string>((Func<ExternalUserInfo, string>) (a => a.ExternalUserID)).ToArray<string>();
      this.grdViewTPOOrgAndContact.Items.Clear();
      HashSet<int> intSet = new HashSet<int>();
      List<int> intList = new List<int>();
      foreach (ExternalOriginatorManagementData originatorManagementData in withPrimarySalesRep)
      {
        List<int> organizationDesendents = this.session.ConfigurationManager.GetExternalOrganizationDesendents(originatorManagementData.oid);
        if (!this.orgParentChildMap.ContainsKey(originatorManagementData.oid))
          this.orgParentChildMap.Add(originatorManagementData.oid, new List<int>());
        foreach (int num in organizationDesendents)
        {
          int d = num;
          List<ExternalOriginatorManagementData> list = withPrimarySalesRep.Where<ExternalOriginatorManagementData>((Func<ExternalOriginatorManagementData, bool>) (a => a.oid == d)).ToList<ExternalOriginatorManagementData>();
          if (!this.orgParentChildMap.ContainsKey(d))
            this.orgParentChildMap.Add(d, new List<int>());
          if (list.Any<ExternalOriginatorManagementData>())
          {
            if (this.orgParentChildMap.ContainsKey(originatorManagementData.oid))
              this.orgParentChildMap[originatorManagementData.oid].Add(d);
            else
              this.orgParentChildMap.Add(originatorManagementData.oid, new List<int>()
              {
                d
              });
          }
          intSet.Add(d);
        }
      }
      foreach (ExternalOriginatorManagementData originatorManagementData in withPrimarySalesRep)
      {
        if (!intSet.Contains(originatorManagementData.oid))
        {
          if (originatorManagementData.HierarchyPath == "Third Party Originators")
            originatorManagementData.HierarchyPath = "Third Party Originators\\" + originatorManagementData.OrganizationName;
          this.grdViewTPOOrgAndContact.Items.Add(new GVItem(new string[6]
          {
            originatorManagementData.HierarchyPath.Replace("Third Party Originators\\", ""),
            "",
            "",
            "",
            originatorManagementData.oid.ToString(),
            "1"
          })
          {
            Tag = (object) originatorManagementData
          });
        }
      }
      foreach (ExternalUserInfo externalUserInfo in userInfoBySalesRep)
      {
        ExternalOriginatorManagementData externalOrganization = this.session.ConfigurationManager.GetExternalOrganization(false, externalUserInfo.ExternalOrgID);
        if (externalOrganization.HierarchyPath == "Third Party Originators")
          externalOrganization.HierarchyPath = "Third Party Originators\\" + externalOrganization.OrganizationName;
        if (this.orgChildContact.ContainsKey(externalUserInfo.ExternalOrgID))
          this.orgChildContact[externalUserInfo.ExternalOrgID].Add(externalUserInfo.ExternalUserID);
        else
          this.orgChildContact.Add(externalUserInfo.ExternalOrgID, new List<string>()
          {
            externalUserInfo.ExternalUserID
          });
        if (!this.orgParentChildMap.ContainsKey(externalUserInfo.ExternalOrgID))
          this.grdViewTPOOrgAndContact.Items.Add(new GVItem(new string[6]
          {
            externalOrganization.HierarchyPath.Replace("Third Party Originators\\", ""),
            externalUserInfo.FirstName,
            externalUserInfo.LastName,
            externalUserInfo.State,
            externalUserInfo.ExternalUserID,
            "2"
          })
          {
            Tag = (object) externalUserInfo
          });
      }
      this.grdViewTPOOrgAndContact.Refresh();
      this.PopulateSalesRep(array);
    }

    private void btnReassign_Click(object sender, EventArgs e)
    {
      this.btnReassign.Enabled = false;
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      if (this.grdViewTPOOrgAndContact.Items == null || !this.grdViewTPOOrgAndContact.Items.Any<GVItem>())
      {
        int num = (int) MessageBox.Show("No TPO Organization or external contact assigned to the sales rep", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.btnReassign.Enabled = true;
      }
      else if (this.grdViewReassignSalesRep.SelectedItems == null || !this.grdViewReassignSalesRep.SelectedItems.Any<GVItem>())
      {
        int num = (int) MessageBox.Show("Select a sales rep to assign", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.btnReassign.Enabled = true;
      }
      else if (!this.grdViewTPOOrgAndContact.SelectedItems.Any<GVItem>())
      {
        int num = (int) MessageBox.Show("Select a TPO organization or contact", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.btnReassign.Enabled = true;
      }
      else
      {
        this.an = new AnimationProgress();
        List<string> selectedExtUser = new List<string>();
        List<int> selectedExtOrg = new List<int>();
        string salesRepId = string.Empty;
        this.ReassignSalesRep(selectedExtUser, selectedExtOrg, ref salesRepId);
        this.t = Task<bool>.Factory.StartNew((Func<bool>) (() => this.ProcessReassignment(selectedExtUser, selectedExtOrg, salesRepId)));
        this.t.ContinueWith((Action<Task<bool>>) (task =>
        {
          if (!task.IsCompleted)
            return;
          this.an.Dispose();
          this.an.Close();
          if (task.IsCanceled || task.IsFaulted || !task.Result)
          {
            int num1 = (int) MessageBox.Show("Reassignment unsuccessful!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
          else
          {
            int num2 = (int) MessageBox.Show((IWin32Window) this, "Reassignment of sales rep has been successful!", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          }
          this.btnReassign.Enabled = true;
          this.PopulateTPOOrgAndContactGridView();
        }), scheduler);
        this.an.StartPosition = FormStartPosition.CenterParent;
        int num = (int) this.an.ShowDialog((IWin32Window) this);
      }
    }

    private void ReassignSalesRep(
      List<string> selectedExtUser,
      List<int> selectedExtOrg,
      ref string salesRepId)
    {
      GVItem selectedItem1 = this.grdViewReassignSalesRep.SelectedItems[0];
      salesRepId = selectedItem1.SubItems[2].Text;
      if (this.grdViewTPOOrgAndContact.SelectedItems.Any<GVItem>())
      {
        foreach (GVItem selectedItem2 in this.grdViewTPOOrgAndContact.SelectedItems)
        {
          if (selectedItem2.SubItems[5].Text == "1")
            selectedExtOrg.Add(Convert.ToInt32(selectedItem2.SubItems[4].Text));
          else
            selectedExtUser.Add(selectedItem2.SubItems[4].Text);
        }
      }
      else
      {
        int num = (int) MessageBox.Show("Select a TPO organization or contact", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      List<int> collection1 = new List<int>();
      List<string> collection2 = new List<string>();
      bool flag = true;
      foreach (int key1 in selectedExtOrg)
      {
        if (this.orgParentChildMap.ContainsKey(key1) && this.orgParentChildMap[key1].Any<int>() || this.orgChildContact.ContainsKey(key1) && this.orgChildContact[key1].Any<string>())
        {
          DialogResult dialogResult = DialogResult.None;
          if (flag)
            dialogResult = MessageBox.Show("Would you like to change all the contacts/sub organizations to use this sales rep?", "Encompass", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
          if (!flag || dialogResult == DialogResult.Yes)
          {
            if (this.orgParentChildMap.ContainsKey(key1))
              collection1.AddRange((IEnumerable<int>) this.orgParentChildMap[key1]);
            if (this.orgChildContact.ContainsKey(key1))
              collection2.AddRange((IEnumerable<string>) this.orgChildContact[key1]);
            foreach (int key2 in this.orgParentChildMap[key1])
            {
              if (key2 != key1 && this.orgChildContact.ContainsKey(key2))
                collection2.AddRange((IEnumerable<string>) this.orgChildContact[key2]);
            }
          }
          flag = false;
        }
      }
      selectedExtUser.AddRange((IEnumerable<string>) collection2);
      selectedExtOrg.AddRange((IEnumerable<int>) collection1);
    }

    private bool ProcessReassignment(
      List<string> selectedExtUser,
      List<int> selectedExtOrg,
      string salesRepId)
    {
      bool flag = this.session.ConfigurationManager.ReassignSalesRep(selectedExtUser, selectedExtOrg, salesRepId, this.selectedUser.SubItems[1].Text);
      if (Utils.Dialog((IWin32Window) this, "Would you like to update the loans with new sales rep?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
        this.session.ConfigurationManager.ReassignTPOLoanSalesRep(selectedExtOrg, selectedExtUser);
      return flag;
    }

    private void PopUpButton_Click(object sender, EventArgs e)
    {
      if (((IEnumerable<UserInfo>) this.userList).Any<UserInfo>())
      {
        this.salesRepInfo = new SalesRepInformation(this.userList);
        this.salesRepInfo.StartPosition = FormStartPosition.CenterParent;
        if (this.salesRepInfo.ShowDialog((IWin32Window) this) == DialogResult.OK)
        {
          this.selectedUser = this.salesRepInfo.selectedUser;
          if (this.selectedUser != null)
            this.textBox1.Text = this.selectedUser.SubItems[0].Text;
          this.salesRepInfo.Close();
        }
        else
          this.salesRepInfo.Close();
      }
      else
      {
        int num = (int) MessageBox.Show("No sales rep found!", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
      if (this.textBox1.Text == null && !(this.textBox1.Text != ""))
        return;
      this.PopulateTPOOrgAndContactGridView();
    }

    private void grdViewReassignSalesRep_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.grdViewReassignSalesRep.Items.Any<GVItem>() && this.grdViewReassignSalesRep.SelectedItems.Any<GVItem>() && this.grdViewTPOOrgAndContact.SelectedItems.Any<GVItem>())
        this.btnReassign.Enabled = true;
      else
        this.btnReassign.Enabled = false;
    }

    private void grdViewTPOOrgAndContact_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.grdViewReassignSalesRep.Items.Any<GVItem>() && this.grdViewReassignSalesRep.SelectedItems.Any<GVItem>() && this.grdViewTPOOrgAndContact.SelectedItems.Any<GVItem>())
        this.btnReassign.Enabled = true;
      else
        this.btnReassign.Enabled = false;
    }

    string IOnlineHelpTarget.GetHelpTargetName() => "Setup\\TPO Reassignment";

    private void grdViewReassignSalesRep_KeyPress(object sender, KeyPressEventArgs e)
    {
      char keyChar = e.KeyChar;
    }

    private void grdViewReassignSalesRep_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
    {
      if (!e.KeyData.HasFlag((Enum) Keys.Control) || !e.KeyData.HasFlag((Enum) Keys.A))
        return;
      this.grdViewReassignSalesRep.SelectedItems.Clear();
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
      this.splitter1 = new Splitter();
      this.grpContTPOOrgAndContacts = new GroupContainer();
      this.textBox1 = new TextBox();
      this.popUpButton = new StandardIconButton();
      this.grdViewTPOOrgAndContact = new GridView();
      this.lblAssignedTo = new Label();
      this.panelEx2 = new PanelEx();
      this.grpCntTPOReassign = new GroupContainer();
      this.grdViewReassignSalesRep = new GridView();
      this.btnReassign = new Button();
      this.panelEx1 = new PanelEx();
      this.grpContTPOOrgAndContacts.SuspendLayout();
      ((ISupportInitialize) this.popUpButton).BeginInit();
      this.panelEx2.SuspendLayout();
      this.grpCntTPOReassign.SuspendLayout();
      this.SuspendLayout();
      this.splitter1.Dock = DockStyle.Right;
      this.splitter1.Location = new Point(577, 0);
      this.splitter1.Name = "splitter1";
      this.splitter1.Size = new Size(3, 459);
      this.splitter1.TabIndex = 1;
      this.splitter1.TabStop = false;
      this.grpContTPOOrgAndContacts.AutoSize = true;
      this.grpContTPOOrgAndContacts.Controls.Add((Control) this.textBox1);
      this.grpContTPOOrgAndContacts.Controls.Add((Control) this.popUpButton);
      this.grpContTPOOrgAndContacts.Controls.Add((Control) this.grdViewTPOOrgAndContact);
      this.grpContTPOOrgAndContacts.Controls.Add((Control) this.lblAssignedTo);
      this.grpContTPOOrgAndContacts.Dock = DockStyle.Fill;
      this.grpContTPOOrgAndContacts.HeaderForeColor = SystemColors.ControlText;
      this.grpContTPOOrgAndContacts.Location = new Point(0, 0);
      this.grpContTPOOrgAndContacts.Name = "grpContTPOOrgAndContacts";
      this.grpContTPOOrgAndContacts.Size = new Size(577, 459);
      this.grpContTPOOrgAndContacts.TabIndex = 2;
      this.grpContTPOOrgAndContacts.Text = "1. TPO Organizations and Contacts";
      this.textBox1.Location = new Point(275, 3);
      this.textBox1.Name = "textBox1";
      this.textBox1.ReadOnly = true;
      this.textBox1.Size = new Size(173, 20);
      this.textBox1.TabIndex = 4;
      this.textBox1.TextChanged += new EventHandler(this.textBox1_TextChanged);
      this.popUpButton.BackColor = Color.Transparent;
      this.popUpButton.Location = new Point(450, 4);
      this.popUpButton.MouseDownImage = (Image) null;
      this.popUpButton.Name = "popUpButton";
      this.popUpButton.Size = new Size(17, 21);
      this.popUpButton.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.popUpButton.TabIndex = 3;
      this.popUpButton.TabStop = false;
      this.popUpButton.Click += new EventHandler(this.PopUpButton_Click);
      this.grdViewTPOOrgAndContact.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "clmnTPOOrgPath";
      gvColumn1.Text = "TPO Org. Path";
      gvColumn1.Width = 250;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "clmnFirstName";
      gvColumn2.Text = "First Name";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "clmnLastName";
      gvColumn3.Text = "Last Name";
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "clmnState";
      gvColumn4.Text = "State";
      gvColumn4.Width = 100;
      this.grdViewTPOOrgAndContact.Columns.AddRange(new GVColumn[4]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4
      });
      this.grdViewTPOOrgAndContact.Dock = DockStyle.Fill;
      this.grdViewTPOOrgAndContact.Location = new Point(1, 26);
      this.grdViewTPOOrgAndContact.Name = "grdViewTPOOrgAndContact";
      this.grdViewTPOOrgAndContact.Size = new Size(575, 432);
      this.grdViewTPOOrgAndContact.TabIndex = 2;
      this.grdViewTPOOrgAndContact.SelectedIndexChanged += new EventHandler(this.grdViewTPOOrgAndContact_SelectedIndexChanged);
      this.lblAssignedTo.AutoSize = true;
      this.lblAssignedTo.BackColor = Color.Transparent;
      this.lblAssignedTo.Location = new Point(206, 7);
      this.lblAssignedTo.Name = "lblAssignedTo";
      this.lblAssignedTo.Size = new Size(63, 14);
      this.lblAssignedTo.TabIndex = 0;
      this.lblAssignedTo.Text = "assigned to";
      this.panelEx2.Controls.Add((Control) this.grpCntTPOReassign);
      this.panelEx2.Dock = DockStyle.Right;
      this.panelEx2.Location = new Point(580, 0);
      this.panelEx2.Name = "panelEx2";
      this.panelEx2.Size = new Size(253, 459);
      this.panelEx2.TabIndex = 0;
      this.grpCntTPOReassign.AutoSize = true;
      this.grpCntTPOReassign.Controls.Add((Control) this.grdViewReassignSalesRep);
      this.grpCntTPOReassign.Controls.Add((Control) this.btnReassign);
      this.grpCntTPOReassign.Dock = DockStyle.Fill;
      this.grpCntTPOReassign.HeaderForeColor = SystemColors.ControlText;
      this.grpCntTPOReassign.Location = new Point(0, 0);
      this.grpCntTPOReassign.Name = "grpCntTPOReassign";
      this.grpCntTPOReassign.Size = new Size(253, 459);
      this.grpCntTPOReassign.TabIndex = 0;
      this.grpCntTPOReassign.Text = "2. Reassign to a Sales Rep";
      this.grdViewReassignSalesRep.AllowMultiselect = false;
      this.grdViewReassignSalesRep.BorderStyle = BorderStyle.None;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "clmnName";
      gvColumn5.Text = "Name";
      gvColumn5.Width = 100;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "clmnPersona";
      gvColumn6.Text = "Persona";
      gvColumn6.Width = 100;
      this.grdViewReassignSalesRep.Columns.AddRange(new GVColumn[2]
      {
        gvColumn5,
        gvColumn6
      });
      this.grdViewReassignSalesRep.Dock = DockStyle.Fill;
      this.grdViewReassignSalesRep.Location = new Point(1, 26);
      this.grdViewReassignSalesRep.Name = "grdViewReassignSalesRep";
      this.grdViewReassignSalesRep.Size = new Size(251, 432);
      this.grdViewReassignSalesRep.TabIndex = 1;
      this.grdViewReassignSalesRep.SelectedIndexChanged += new EventHandler(this.grdViewReassignSalesRep_SelectedIndexChanged);
      this.grdViewReassignSalesRep.PreviewKeyDown += new PreviewKeyDownEventHandler(this.grdViewReassignSalesRep_PreviewKeyDown);
      this.btnReassign.Enabled = false;
      this.btnReassign.Location = new Point(161, 0);
      this.btnReassign.Name = "btnReassign";
      this.btnReassign.Size = new Size(75, 25);
      this.btnReassign.TabIndex = 0;
      this.btnReassign.Text = "Reassign";
      this.btnReassign.UseMnemonic = false;
      this.btnReassign.UseVisualStyleBackColor = true;
      this.btnReassign.Click += new EventHandler(this.btnReassign_Click);
      this.panelEx1.Dock = DockStyle.Left;
      this.panelEx1.Location = new Point(0, 0);
      this.panelEx1.Name = "panelEx1";
      this.panelEx1.Size = new Size(827, 459);
      this.panelEx1.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.grpContTPOOrgAndContacts);
      this.Controls.Add((Control) this.splitter1);
      this.Controls.Add((Control) this.panelEx2);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (TPOReassignment);
      this.Size = new Size(833, 459);
      this.grpContTPOOrgAndContacts.ResumeLayout(false);
      this.grpContTPOOrgAndContacts.PerformLayout();
      ((ISupportInitialize) this.popUpButton).EndInit();
      this.panelEx2.ResumeLayout(false);
      this.panelEx2.PerformLayout();
      this.grpCntTPOReassign.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
