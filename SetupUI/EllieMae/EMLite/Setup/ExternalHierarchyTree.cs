// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalHierarchyTree
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class ExternalHierarchyTree : TreeView
  {
    public const int IDXFolderAndBelowSelected = 0;
    public const int IDXFolderSelected = 1;
    public const int IDXUserSelected = 2;
    public const int IDXFolderIncluded = 3;
    public const int IDXUserIncluded = 4;
    public const int IDXFolderNotInGroup = 5;
    public const int IDXUserNotInGroup = 6;
    public const string DUMMY_NODE = "<DUMMY NODE>";
    public const int BatchLoadNum = 10;
    private List<HierarchySummary>[] externalCompanyList;
    private List<HierarchySummary> brokerCompanyList;
    private List<HierarchySummary> brokerOrganizationList;
    private Sessions.Session session;

    public List<HierarchySummary>[] ExternalCompanyList
    {
      get => this.externalCompanyList;
      set
      {
        this.externalCompanyList = value;
        this.brokerCompanyList = (List<HierarchySummary>) null;
        this.brokerOrganizationList = (List<HierarchySummary>) null;
      }
    }

    public bool IsTpoAdmin { get; set; }

    public List<ExternalOriginatorManagementData> BrokerRootOrgsList { get; set; }

    public List<int> ExternalOrgsList { get; set; }

    public List<HierarchySummary> LenderOrganizations
    {
      get
      {
        return this.ExternalCompanyList == null || this.ExternalCompanyList.Length != 3 ? new List<HierarchySummary>() : this.ExternalCompanyList[0];
      }
    }

    public List<HierarchySummary> BrokerOrganizations
    {
      get
      {
        return this.ExternalCompanyList == null || this.ExternalCompanyList.Length != 3 ? new List<HierarchySummary>() : this.ExternalCompanyList[1];
      }
    }

    public List<HierarchySummary> BrokerCompanyList
    {
      get
      {
        if (this.brokerCompanyList == null)
          this.brokerCompanyList = this.BrokerOrganizations.Where<HierarchySummary>((Func<HierarchySummary, bool>) (x => x.Depth == 1)).ToList<HierarchySummary>();
        return this.brokerCompanyList;
      }
    }

    public List<HierarchySummary> BrokerOrganizationList
    {
      get
      {
        if (this.brokerOrganizationList == null)
          this.brokerOrganizationList = this.BrokerOrganizations.Where<HierarchySummary>((Func<HierarchySummary, bool>) (x => x.Depth != 1)).ToList<HierarchySummary>();
        return this.brokerOrganizationList;
      }
    }

    public Sessions.Session CurrentSession => this.session;

    public ExternalHierarchyTree()
      : this(Session.DefaultInstance)
    {
    }

    public ExternalHierarchyTree(Sessions.Session session) => this.session = session;

    public void SetSession(Sessions.Session session) => this.session = session;

    public void CallBeforeExpand(TreeViewCancelEventArgs e) => this.OnBeforeExpand(e);

    protected override void OnBeforeExpand(TreeViewCancelEventArgs e)
    {
      base.OnBeforeExpand(e);
      this.BeginUpdate();
      this.addBranch(e.Node);
      this.EndUpdate();
    }

    private void addBranch(TreeNode tn)
    {
      if (tn.Nodes.Count > 1 || tn.Nodes.Count == 1 && tn.Nodes[0].Text != "<DUMMY NODE>")
        return;
      tn.Nodes.Clear();
      IConfigurationManager configurationManager = this.session.ConfigurationManager;
      ExtNodeTag tag = (ExtNodeTag) tn.Tag;
      List<HierarchySummary> list = this.BrokerOrganizations.Where<HierarchySummary>((Func<HierarchySummary, bool>) (x => x.Parent == tag.Oid)).ToList<HierarchySummary>();
      if (list.Count == 0)
        return;
      foreach (HierarchySummary hierarchySummary in list.ToList<HierarchySummary>())
      {
        HierarchySummary o = hierarchySummary;
        if (this.BrokerOrganizationList.FirstOrDefault<HierarchySummary>((Func<HierarchySummary, bool>) (e1 => e1.ExternalID.Equals(o.ExternalID))) != null || this.IsTpoAdmin)
        {
          TreeNode node = new TreeNode(o.OrganizationName, 0, 1);
          ExtNodeTag extNodeTag = new ExtNodeTag(o.oid, o.OrganizationName, "Tpo");
          if (this.ExternalOrgsList != null && (this.ExternalOrgsList.Contains(o.oid) || tag.AllowAccess || this.IsTpoAdmin))
          {
            Session.AddTpoHierarchyAccessCache(o.oid, true);
            extNodeTag.AllowAccess = true;
          }
          node.Tag = (object) extNodeTag;
          tn.Nodes.Add(node);
          if (this.BrokerOrganizations.FirstOrDefault<HierarchySummary>((Func<HierarchySummary, bool>) (x => x.Parent == o.oid)) != null)
            node.Nodes.Add(new TreeNode("<DUMMY NODE>", 0, 1));
        }
      }
    }
  }
}
