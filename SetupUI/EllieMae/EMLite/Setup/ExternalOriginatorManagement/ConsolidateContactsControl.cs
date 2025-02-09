// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.ConsolidateContactsControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.ExternalOriginatorManagement
{
  public class ConsolidateContactsControl : Form
  {
    private Sessions.Session session;
    private TreeNode treeTopNodeBroker = new TreeNode("Third Party Originators", 0, 1);
    private const string tpoCompanyType = "Tpo";
    private List<ExternalUserInfo> listExternalUsersToMove = new List<ExternalUserInfo>();
    private List<string> selectedUrlsForAdding = new List<string>();
    private int destOid;
    private List<ExternalOrgURL> destinationOrgUrls = new List<ExternalOrgURL>();
    private IContainer components;
    private ExternalHierarchyTree trvOrgStructure;
    private GroupContainer grpContainerOptions;
    private GroupContainer grpContainerURLSelections;
    private RadioButton rdoAdd;
    private RadioButton rdoKeep;
    private RadioButton rdoReplace;
    private Button btnMove;
    private ImageList imgListTv;
    private Button btnClose;
    private CheckedListBox chkListUrls;
    private SplitContainer splitContainer1;

    public ConsolidateContactsControl(Sessions.Session session, List<ExternalUserInfo> userList)
    {
      this.session = session;
      this.treeTopNodeBroker.Tag = (object) new ExtNodeTag(0, "Third Parth Originators", "Tpo");
      foreach (ExternalUserInfo user in userList)
        this.listExternalUsersToMove.Add(this.session.ConfigurationManager.GetExternalUserInfo(user.ExternalUserID));
      this.InitializeComponent();
      this.trvOrgStructure.Nodes.Add(this.treeTopNodeBroker);
      this.trvOrgStructure.ImageList = this.imgListTv;
      this.trvOrgStructure.SelectedNode = this.treeTopNodeBroker;
      this.chkListUrls.Enabled = false;
      this.initHierarchy();
      this.btnMove.Enabled = false;
    }

    private void initHierarchy()
    {
      Cursor.Current = Cursors.WaitCursor;
      List<HierarchySummary>[] allHierarchy = this.session.ConfigurationManager.GetAllHierarchy();
      List<HierarchySummary> hierarchySummaryList = allHierarchy == null || allHierarchy.Length != 3 ? (List<HierarchySummary>) null : allHierarchy[0];
      List<HierarchySummary> list = allHierarchy == null || allHierarchy.Length != 3 ? (List<HierarchySummary>) null : allHierarchy[1];
      this.trvOrgStructure.ExternalCompanyList = allHierarchy;
      this.trvOrgStructure.BeginUpdate();
      if (list != null)
        this.buildHierarchyTree(list, false);
      this.trvOrgStructure.EndUpdate();
      this.trvOrgStructure.SelectedNode = this.treeTopNodeBroker;
      Cursor.Current = Cursors.Default;
    }

    private void buildHierarchyTree(List<HierarchySummary> list, bool access)
    {
      if (list.Count == 0)
        return;
      this.buildHierarchyTree(this.treeTopNodeBroker, list.Where<HierarchySummary>((Func<HierarchySummary, bool>) (x => x.Depth == 1)).ToList<HierarchySummary>(), false);
      this.treeTopNodeBroker.Expand();
    }

    private void buildHierarchyTree(TreeNode parentNode, List<HierarchySummary> list, bool access)
    {
      if (list.Count == 0)
        return;
      TreeNode newTreeNode = (TreeNode) null;
      list.ForEach((Action<HierarchySummary>) (company =>
      {
        newTreeNode = new TreeNode(company.OrganizationName, 0, 1);
        newTreeNode.Tag = (object) new ExtNodeTag(company.oid, company.OrganizationName, "Tpo");
        parentNode.Nodes.Add(newTreeNode);
        if (this.trvOrgStructure.BrokerOrganizationList.FirstOrDefault<HierarchySummary>((Func<HierarchySummary, bool>) (x => x.Parent == company.oid)) == null)
          return;
        newTreeNode.Nodes.Add("<DUMMY NODE>");
      }));
    }

    private void trvOrgStructure_AfterSelect(object sender, TreeViewEventArgs e)
    {
      ExtNodeTag tag = (ExtNodeTag) this.trvOrgStructure.SelectedNode.Tag;
      this.chkListUrls.Items.Clear();
      this.LoadExternalOrgUrls(tag.Oid);
      this.destOid = ((ExtNodeTag) this.trvOrgStructure.SelectedNode.Tag).Oid;
      if (this.destOid == 0 || this.destOid == this.listExternalUsersToMove[0].ExternalOrgID)
        this.btnMove.Enabled = false;
      else if (this.rdoKeep.Checked || this.rdoAdd.Checked && this.chkListUrls.CheckedItems.Count > 0 || this.rdoReplace.Checked && this.chkListUrls.CheckedItems.Count > 0)
        this.btnMove.Enabled = true;
      else
        this.btnMove.Enabled = false;
    }

    private void LoadExternalOrgUrls(int oid)
    {
      foreach (ExternalOrgURL selectedOrgUrl in this.session.ConfigurationManager.GetSelectedOrgUrls(oid))
        this.chkListUrls.Items.Add((object) selectedOrgUrl.URL);
    }

    private void btnMove_Click(object sender, EventArgs e)
    {
      this.btnClose.Enabled = false;
      this.btnMove.Enabled = false;
      if (this.destOid == this.listExternalUsersToMove[0].ExternalOrgID)
      {
        MessageBox.Show("Please select a destination organization different from source organization.", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Hand);
      }
      else
      {
        this.selectedUrlsForAdding.Clear();
        foreach (object checkedItem in this.chkListUrls.CheckedItems)
          this.selectedUrlsForAdding.Add(checkedItem.ToString());
        List<int> intList = new List<int>();
        this.destinationOrgUrls = this.session.ConfigurationManager.GetSelectedOrgUrls(this.destOid);
        foreach (string str in this.selectedUrlsForAdding)
        {
          string surl = str;
          intList.AddRange((IEnumerable<int>) this.destinationOrgUrls.Where<ExternalOrgURL>((Func<ExternalOrgURL, bool>) (a => a.URL == surl)).Select<ExternalOrgURL, int>((Func<ExternalOrgURL, int>) (a => a.URLID)).ToList<int>());
        }
        List<ExternalUserInfo> source = new List<ExternalUserInfo>();
        List<ExternalUserInfo> externalUserInfoList = new List<ExternalUserInfo>();
        List<ExternalUserInfoURL> externalUserInfoUrlList = new List<ExternalUserInfoURL>();
        foreach (ExternalUserInfo externalUserInfo in this.listExternalUsersToMove)
        {
          ExternalUserInfo u = externalUserInfo;
          List<ExternalUserInfo> contactsByLoginEmailId = this.session.ConfigurationManager.GetAllContactsByLoginEmailId(u.EmailForLogin, "");
          if (contactsByLoginEmailId != null)
          {
            List<ExternalUserInfoURL> urlsByContactIds = this.session.ConfigurationManager.GetExternalUserInfoUrlsByContactIds(string.Join("','", contactsByLoginEmailId.Select<ExternalUserInfo, string>((Func<ExternalUserInfo, string>) (eu => eu.ExternalUserID)).ToArray<string>()));
            if (urlsByContactIds != null)
            {
              foreach (int num in intList)
              {
                int urlId = num;
                if (urlsByContactIds.FirstOrDefault<ExternalUserInfoURL>((Func<ExternalUserInfoURL, bool>) (item => item.URLID == urlId && item.ExternalUserID != u.ExternalUserID)) != null)
                {
                  source.Add(u);
                  break;
                }
              }
            }
          }
        }
        if (source.Any<ExternalUserInfo>())
        {
          string text = "The following contact(s) cannot be moved with the selected TPO WebCenter URL assigned to them because existing contacts have the same login email address and URL combination.";
          foreach (ExternalUserInfo externalUserInfo in source)
            text = text + "\n" + externalUserInfo.FirstName + " " + externalUserInfo.LastName;
          if (MessageBox.Show(text, "Encompass", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
          {
            this.btnClose.Enabled = true;
            return;
          }
        }
        foreach (ExternalUserInfo externalUserInfo in source)
          this.listExternalUsersToMove.Remove(externalUserInfo);
        if (this.listExternalUsersToMove.Any<ExternalUserInfo>())
        {
          bool flag = false;
          if (this.rdoAdd.Checked)
            flag = this.MoveWithAddURL();
          else if (this.rdoKeep.Checked)
            flag = this.MoveWithKeepURL();
          else if (this.rdoReplace.Checked)
            flag = this.MoveWithReplaceURL();
          if (flag)
          {
            foreach (ExternalUserInfo externalUserInfo1 in this.listExternalUsersToMove)
            {
              ExternalUserInfo newUser = new ExternalUserInfo();
              ExternalUserInfo externalUserInfo2 = this.session.ConfigurationManager.GetExternalUserInfo(externalUserInfo1.ExternalUserID);
              newUser.ExternalUserID = externalUserInfo1.ExternalUserID;
              newUser.ExternalOrgID = this.destOid;
              newUser.ContactID = externalUserInfo1.ContactID;
              newUser.Roles = externalUserInfo1.Roles;
              this.session.ConfigurationManager.ReassignTPOLoans(externalUserInfo2, newUser);
            }
          }
        }
        this.Close();
      }
    }

    private bool Validation(int destinationOid, bool validateUrl)
    {
      List<int> intList = new List<int>();
      bool flag1 = true;
      this.session.ConfigurationManager.GetPrimarySalesRep(destinationOid);
      List<ExternalOrgSalesRep> repsForCurrentOrg = this.session.ConfigurationManager.GetExternalOrgSalesRepsForCurrentOrg(destinationOid);
      bool flag2 = true;
      bool flag3 = true;
      foreach (ExternalUserInfo externalUserInfo in this.listExternalUsersToMove)
      {
        ExternalUserInfo user = externalUserInfo;
        if (validateUrl)
        {
          foreach (ExternalUserURL externalUserInfoUrL in this.session.ConfigurationManager.GetExternalUserInfoURLs(user.ExternalUserID))
          {
            ExternalUserURL url = externalUserInfoUrL;
            if (this.destinationOrgUrls.FirstOrDefault<ExternalOrgURL>((Func<ExternalOrgURL, bool>) (a => a.URLID == url.URLID)) == null && flag1)
            {
              if (MessageBox.Show("One or more URLs for one or more TPO contacts do not exist in the new company.These URLs will be deleted.", "Encompass", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
                return false;
              flag1 = false;
            }
          }
        }
        intList.Clear();
        if (repsForCurrentOrg.Where<ExternalOrgSalesRep>((Func<ExternalOrgSalesRep, bool>) (a => a.userId == user.SalesRepID)).FirstOrDefault<ExternalOrgSalesRep>() == null && flag2)
        {
          if (MessageBox.Show("Sales Rep for one or more TPO contacts do not exist in the new company, Sales Rep will be changed to the destination company's Primary Sales Rep.", "Encompass", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
            return false;
          flag2 = false;
        }
        ExternalOriginatorManagementData externalOrganization = this.session.ConfigurationManager.GetExternalOrganization(false, this.listExternalUsersToMove[0].ExternalOrgID);
        if (externalOrganization.Manager == user.ExternalUserID)
        {
          if (flag3)
          {
            if (MessageBox.Show("Primary Manager is being moved from current organization.", "Encompass", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
              return false;
            flag3 = false;
          }
          externalOrganization.Manager = (string) null;
        }
      }
      return true;
    }

    private bool MoveWithReplaceURL()
    {
      List<int> intList = new List<int>();
      bool flag1 = true;
      bool flag2 = this.Validation(this.destOid, false);
      if (flag2)
      {
        foreach (string str in this.selectedUrlsForAdding)
        {
          string surl = str;
          intList.AddRange((IEnumerable<int>) this.destinationOrgUrls.Where<ExternalOrgURL>((Func<ExternalOrgURL, bool>) (a => a.URL == surl)).Select<ExternalOrgURL, int>((Func<ExternalOrgURL, int>) (a => a.URLID)).ToList<int>());
        }
        foreach (ExternalUserInfo externalUserInfo in this.listExternalUsersToMove)
          this.session.ConfigurationManager.SaveExternalUserInfoURLs(externalUserInfo.ExternalUserID, intList.ToArray());
        flag1 = this.UpdateSalesRep(this.destOid);
        flag2 = this.MoveExternalUsers(this.destOid);
      }
      return flag2;
    }

    private bool MoveWithKeepURL()
    {
      List<int> intList = new List<int>();
      bool flag1 = true;
      bool flag2 = this.Validation(this.destOid, true);
      if (flag2)
      {
        foreach (ExternalUserInfo externalUserInfo in this.listExternalUsersToMove)
        {
          foreach (ExternalUserURL externalUserInfoUrL in this.session.ConfigurationManager.GetExternalUserInfoURLs(externalUserInfo.ExternalUserID))
          {
            ExternalUserURL url = externalUserInfoUrL;
            if (this.destinationOrgUrls.FirstOrDefault<ExternalOrgURL>((Func<ExternalOrgURL, bool>) (a => a.URLID == url.URLID)) != null)
              intList.Add(url.URLID);
          }
          this.session.ConfigurationManager.SaveExternalUserInfoURLs(externalUserInfo.ExternalUserID, intList.ToArray());
          intList.Clear();
        }
        flag1 = this.UpdateSalesRep(this.destOid);
        flag2 = this.MoveExternalUsers(this.destOid);
      }
      return flag2;
    }

    private bool MoveWithAddURL()
    {
      List<int> source = new List<int>();
      List<int> collection = new List<int>();
      bool flag1 = true;
      bool flag2 = this.Validation(this.destOid, true);
      if (flag2)
      {
        foreach (string str in this.selectedUrlsForAdding)
        {
          string surl = str;
          collection.AddRange((IEnumerable<int>) this.destinationOrgUrls.Where<ExternalOrgURL>((Func<ExternalOrgURL, bool>) (a => a.URL == surl)).Select<ExternalOrgURL, int>((Func<ExternalOrgURL, int>) (a => a.URLID)).ToList<int>());
        }
        foreach (ExternalUserInfo externalUserInfo in this.listExternalUsersToMove)
        {
          foreach (ExternalUserURL externalUserInfoUrL in this.session.ConfigurationManager.GetExternalUserInfoURLs(externalUserInfo.ExternalUserID))
          {
            ExternalUserURL url = externalUserInfoUrL;
            if (this.destinationOrgUrls.FirstOrDefault<ExternalOrgURL>((Func<ExternalOrgURL, bool>) (a => a.URLID == url.URLID)) != null)
              source.Add(url.URLID);
          }
          source.AddRange((IEnumerable<int>) collection);
          source = source.Distinct<int>().ToList<int>();
          this.session.ConfigurationManager.SaveExternalUserInfoURLs(externalUserInfo.ExternalUserID, source.ToArray());
          source.Clear();
        }
        flag1 = this.UpdateSalesRep(this.destOid);
        flag2 = this.MoveExternalUsers(this.destOid);
      }
      return flag2;
    }

    private bool MoveExternalUsers(int destinationOid)
    {
      foreach (ExternalUserInfo externalUserInfo in this.listExternalUsersToMove)
      {
        foreach (int oid in this.session.ConfigurationManager.GetExternalOrganizationsForManagerID(externalUserInfo.ExternalUserID))
          this.session.ConfigurationManager.UpdateExternalOrgManager(oid, "");
      }
      if (this.listExternalUsersToMove.Any<ExternalUserInfo>())
        this.listExternalUsersToMove[0].UpdatedByExternalAdmin = "";
      this.session.ConfigurationManager.MoveExternalUser(this.listExternalUsersToMove, destinationOid);
      return true;
    }

    private bool UpdateSalesRep(int destinationOid)
    {
      List<ExternalOrgSalesRep> repsForCurrentOrg = this.session.ConfigurationManager.GetExternalOrgSalesRepsForCurrentOrg(destinationOid);
      string primarySalesRep = this.session.ConfigurationManager.GetPrimarySalesRep(destinationOid);
      foreach (ExternalUserInfo externalUserInfo in this.listExternalUsersToMove)
      {
        ExternalUserInfo u = externalUserInfo;
        if (repsForCurrentOrg.Where<ExternalOrgSalesRep>((Func<ExternalOrgSalesRep, bool>) (a => a.userId == u.SalesRepID)).FirstOrDefault<ExternalOrgSalesRep>() == null)
        {
          u.SalesRepID = primarySalesRep;
          this.session.ConfigurationManager.SaveExternalUserInfo(u);
        }
      }
      return true;
    }

    private void rdoKeep_CheckedChanged(object sender, EventArgs e)
    {
      if (this.rdoKeep.Checked)
      {
        this.chkListUrls.Enabled = false;
        foreach (int checkedIndex in this.chkListUrls.CheckedIndices)
          this.chkListUrls.SetItemCheckState(checkedIndex, CheckState.Unchecked);
        if (this.destOid == this.listExternalUsersToMove[0].ExternalOrgID)
          this.btnMove.Enabled = false;
        else
          this.btnMove.Enabled = true;
      }
      else
      {
        this.chkListUrls.Enabled = true;
        this.btnMove.Enabled = false;
      }
      if (this.destOid != 0 && this.destOid != this.listExternalUsersToMove[0].ExternalOrgID)
        return;
      this.btnMove.Enabled = false;
    }

    private void btnClose_Click(object sender, EventArgs e) => this.Close();

    private void chkListUrls_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!this.rdoKeep.Checked && this.chkListUrls.CheckedItems.Count > 0)
        this.btnMove.Enabled = true;
      else
        this.btnMove.Enabled = false;
      if (this.destOid != 0 && this.destOid != this.listExternalUsersToMove[0].ExternalOrgID)
        return;
      this.btnMove.Enabled = false;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ConsolidateContactsControl));
      this.trvOrgStructure = new ExternalHierarchyTree();
      this.grpContainerOptions = new GroupContainer();
      this.rdoAdd = new RadioButton();
      this.rdoKeep = new RadioButton();
      this.rdoReplace = new RadioButton();
      this.grpContainerURLSelections = new GroupContainer();
      this.chkListUrls = new CheckedListBox();
      this.btnMove = new Button();
      this.imgListTv = new ImageList(this.components);
      this.btnClose = new Button();
      this.splitContainer1 = new SplitContainer();
      this.grpContainerOptions.SuspendLayout();
      this.grpContainerURLSelections.SuspendLayout();
      this.splitContainer1.BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.SuspendLayout();
      this.trvOrgStructure.BrokerRootOrgsList = (List<ExternalOriginatorManagementData>) null;
      this.trvOrgStructure.Dock = DockStyle.Fill;
      this.trvOrgStructure.ExternalCompanyList = (List<HierarchySummary>[]) null;
      this.trvOrgStructure.ExternalOrgsList = (List<int>) null;
      this.trvOrgStructure.HideSelection = false;
      this.trvOrgStructure.IsTpoAdmin = false;
      this.trvOrgStructure.Location = new Point(0, 0);
      this.trvOrgStructure.Name = "trvOrgStructure";
      this.trvOrgStructure.Size = new Size(329, 523);
      this.trvOrgStructure.Sorted = true;
      this.trvOrgStructure.TabIndex = 0;
      this.trvOrgStructure.AfterSelect += new TreeViewEventHandler(this.trvOrgStructure_AfterSelect);
      this.grpContainerOptions.Controls.Add((Control) this.chkListUrls);
      this.grpContainerOptions.Dock = DockStyle.Fill;
      this.grpContainerOptions.HeaderForeColor = SystemColors.ControlText;
      this.grpContainerOptions.Location = new Point(0, 77);
      this.grpContainerOptions.Name = "grpContainerOptions";
      this.grpContainerOptions.Size = new Size(777, 446);
      this.grpContainerOptions.TabIndex = 4;
      this.grpContainerOptions.Text = "Select URLs";
      this.rdoAdd.AutoSize = true;
      this.rdoAdd.Location = new Point(389, 41);
      this.rdoAdd.Name = "rdoAdd";
      this.rdoAdd.Size = new Size(236, 17);
      this.rdoAdd.TabIndex = 2;
      this.rdoAdd.Text = "Add one or more URLs to their existing URLs";
      this.rdoAdd.UseVisualStyleBackColor = true;
      this.rdoKeep.AutoSize = true;
      this.rdoKeep.Checked = true;
      this.rdoKeep.Location = new Point(9, 41);
      this.rdoKeep.Name = "rdoKeep";
      this.rdoKeep.Size = new Size(118, 17);
      this.rdoKeep.TabIndex = 0;
      this.rdoKeep.TabStop = true;
      this.rdoKeep.Text = "Keep existing URLs";
      this.rdoKeep.UseVisualStyleBackColor = true;
      this.rdoKeep.CheckedChanged += new EventHandler(this.rdoKeep_CheckedChanged);
      this.rdoReplace.AutoSize = true;
      this.rdoReplace.Location = new Point(134, 41);
      this.rdoReplace.Name = "rdoReplace";
      this.rdoReplace.Size = new Size(249, 17);
      this.rdoReplace.TabIndex = 1;
      this.rdoReplace.Text = "Replace URLs with one or more selected URLs";
      this.rdoReplace.UseVisualStyleBackColor = true;
      this.grpContainerURLSelections.Controls.Add((Control) this.rdoAdd);
      this.grpContainerURLSelections.Controls.Add((Control) this.rdoKeep);
      this.grpContainerURLSelections.Controls.Add((Control) this.rdoReplace);
      this.grpContainerURLSelections.Dock = DockStyle.Top;
      this.grpContainerURLSelections.HeaderForeColor = SystemColors.ControlText;
      this.grpContainerURLSelections.Location = new Point(0, 0);
      this.grpContainerURLSelections.Name = "grpContainerURLSelections";
      this.grpContainerURLSelections.Size = new Size(777, 77);
      this.grpContainerURLSelections.TabIndex = 5;
      this.chkListUrls.CheckOnClick = true;
      this.chkListUrls.Dock = DockStyle.Fill;
      this.chkListUrls.FormattingEnabled = true;
      this.chkListUrls.Location = new Point(1, 26);
      this.chkListUrls.Name = "chkListUrls";
      this.chkListUrls.Size = new Size(775, 419);
      this.chkListUrls.TabIndex = 0;
      this.chkListUrls.SelectedIndexChanged += new EventHandler(this.chkListUrls_SelectedIndexChanged);
      this.btnMove.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnMove.Location = new Point(955, 529);
      this.btnMove.Name = "btnMove";
      this.btnMove.Size = new Size(75, 23);
      this.btnMove.TabIndex = 2;
      this.btnMove.Text = "Move";
      this.btnMove.UseVisualStyleBackColor = true;
      this.btnMove.Click += new EventHandler(this.btnMove_Click);
      this.imgListTv.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgListTv.ImageStream");
      this.imgListTv.TransparentColor = Color.Transparent;
      this.imgListTv.Images.SetKeyName(0, "folder.bmp");
      this.imgListTv.Images.SetKeyName(1, "folder-open.bmp");
      this.btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnClose.Location = new Point(1036, 529);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 23);
      this.btnClose.TabIndex = 3;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.splitContainer1.Location = new Point(1, 0);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Panel1.Controls.Add((Control) this.trvOrgStructure);
      this.splitContainer1.Panel2.Controls.Add((Control) this.grpContainerOptions);
      this.splitContainer1.Panel2.Controls.Add((Control) this.grpContainerURLSelections);
      this.splitContainer1.Size = new Size(1110, 523);
      this.splitContainer1.SplitterDistance = 329;
      this.splitContainer1.TabIndex = 4;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1114, 559);
      this.Controls.Add((Control) this.splitContainer1);
      this.Controls.Add((Control) this.btnClose);
      this.Controls.Add((Control) this.btnMove);
      this.Name = nameof (ConsolidateContactsControl);
      this.Text = "Consolidate Contacts";
      this.grpContainerOptions.ResumeLayout(false);
      this.grpContainerURLSelections.ResumeLayout(false);
      this.grpContainerURLSelections.PerformLayout();
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.EndInit();
      this.splitContainer1.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public enum UrlOption
    {
      Keep,
      Replace,
      Add,
    }
  }
}
