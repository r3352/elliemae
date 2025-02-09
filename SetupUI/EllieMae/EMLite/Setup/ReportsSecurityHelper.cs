// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ReportsSecurityHelper
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System.Collections;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class ReportsSecurityHelper : FeatureSecurityHelperBase
  {
    protected TreeNode reportsNode;
    protected TreeNode managePersonalReportsNode;
    protected TreeNode loanReportsNode;
    protected TreeNode reportDBNode;
    protected TreeNode borrowerContactReportNode;
    protected TreeNode bizContactReportNode;
    protected TreeNode dynamicReportNode;
    protected TreeNode tpoReportNode;
    protected TreeNode tradeReportNode;

    public ReportsSecurityHelper(Sessions.Session session, string userId, Persona[] personas)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new UserSecurityHelper(this.session, userId, personas, AclCategory.Features, FeatureSets.ReportFeatures);
    }

    public ReportsSecurityHelper(Sessions.Session session, int personaId)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new PersonaSecurityHelper(this.session, personaId, AclCategory.Features, FeatureSets.ReportFeatures);
    }

    public override void BuildNodes(TreeView treeView)
    {
      this.managePersonalReportsNode = new TreeNode("Manage Personal Reports");
      this.loanReportsNode = new TreeNode("Loan Reports");
      this.reportDBNode = new TreeNode("Allow loan files to be opened for data (slower performance)");
      this.borrowerContactReportNode = new TreeNode("Borrower Contact Reports");
      this.bizContactReportNode = new TreeNode("Business Contact Reports");
      this.dynamicReportNode = new TreeNode("Dynamic Reporting");
      this.tpoReportNode = new TreeNode("TPO Settings Reports");
      this.tradeReportNode = new TreeNode("Trade Reports");
      this.reportsNode = new TreeNode("Reports");
      this.loanReportsNode.Nodes.Add(this.reportDBNode);
      this.reportsNode.Nodes.AddRange(new TreeNode[7]
      {
        this.loanReportsNode,
        this.borrowerContactReportNode,
        this.bizContactReportNode,
        this.tpoReportNode,
        this.tradeReportNode,
        this.managePersonalReportsNode,
        this.dynamicReportNode
      });
      treeView.Nodes.Add(this.reportsNode);
      this.dependentNodes.Add((object) this.reportsNode);
      this.nodeToFeature = new Hashtable();
      this.nodeToFeature.Add((object) this.managePersonalReportsNode, (object) AclFeature.ReportTab_PersonalTemplate);
      this.nodeToFeature.Add((object) this.loanReportsNode, (object) AclFeature.ReportTab_LoanReport);
      this.nodeToFeature.Add((object) this.borrowerContactReportNode, (object) AclFeature.ReportTab_BorrowerContactReport);
      this.nodeToFeature.Add((object) this.bizContactReportNode, (object) AclFeature.ReportTab_BusinessContactReport);
      this.nodeToFeature.Add((object) this.reportDBNode, (object) AclFeature.ReportTab_ReportingDB);
      this.nodeToFeature.Add((object) this.dynamicReportNode, (object) AclFeature.ReportTab_DynamicReporting);
      this.nodeToFeature.Add((object) this.tpoReportNode, (object) AclFeature.ReportTab_TpoSettingsReport);
      this.nodeToFeature.Add((object) this.tradeReportNode, (object) AclFeature.ReportTab_TradeReport);
      this.featureToNodeTbl = new Hashtable();
      this.featureToNodeTbl.Add((object) AclFeature.ReportTab_PersonalTemplate, (object) this.managePersonalReportsNode);
      this.featureToNodeTbl.Add((object) AclFeature.ReportTab_BorrowerContactReport, (object) this.borrowerContactReportNode);
      this.featureToNodeTbl.Add((object) AclFeature.ReportTab_LoanReport, (object) this.loanReportsNode);
      this.featureToNodeTbl.Add((object) AclFeature.ReportTab_BusinessContactReport, (object) this.bizContactReportNode);
      this.featureToNodeTbl.Add((object) AclFeature.ReportTab_ReportingDB, (object) this.reportDBNode);
      this.featureToNodeTbl.Add((object) AclFeature.ReportTab_DynamicReporting, (object) this.dynamicReportNode);
      this.featureToNodeTbl.Add((object) AclFeature.ReportTab_TpoSettingsReport, (object) this.tpoReportNode);
      this.featureToNodeTbl.Add((object) AclFeature.ReportTab_TradeReport, (object) this.tradeReportNode);
      this.nodeToUpdateStatus = new Hashtable();
      this.nodeToUpdateStatus.Add((object) this.managePersonalReportsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.loanReportsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.borrowerContactReportNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.bizContactReportNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.reportDBNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.dynamicReportNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.tpoReportNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.tradeReportNode, (object) false);
      this.securityHelper.SetTables(this.featureToNodeTbl, this.nodeToFeature, this.dependentNodes, this.nodeToUpdateStatus);
      treeView.ExpandAll();
    }

    public void AfterChekcedEventHandler(TreeNode node)
    {
      if (node == this.managePersonalReportsNode)
      {
        if (!node.Checked || this.loanReportsNode.Checked || this.borrowerContactReportNode.Checked || this.bizContactReportNode.Checked)
          return;
        this.loanReportsNode.Checked = this.borrowerContactReportNode.Checked = this.bizContactReportNode.Checked = true;
      }
      else
      {
        if (node != this.loanReportsNode && node != this.borrowerContactReportNode && node != this.bizContactReportNode || this.loanReportsNode.Checked || this.borrowerContactReportNode.Checked || this.bizContactReportNode.Checked)
          return;
        this.managePersonalReportsNode.Checked = false;
      }
    }
  }
}
