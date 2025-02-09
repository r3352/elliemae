// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.TradesSecurityHelper
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class TradesSecurityHelper : FeatureSecurityHelperBase
  {
    protected TreeNode tradesNode;
    protected TreeNode securityTradesNode;
    protected TreeNode editSecurityTradesNode;
    protected TreeNode loanSearchNode;
    protected TreeNode editLoanSearchNode;
    protected TreeNode loanTradesNode;
    protected TreeNode editLoanTradesNode;
    protected TreeNode masterCommitmentsNode;
    protected TreeNode editMasterCommitmentsNode;
    protected TreeNode mbsPoolsNode;
    protected TreeNode editMBSPoolsNode;
    protected TreeNode gseCommitmentsNode;
    protected TreeNode editGSECommitmentsNode;
    protected TreeNode correspondentTradesNode;
    protected TreeNode editCorrespondentTradesNode;
    protected TreeNode correspondentMastersNode;
    protected TreeNode editCorrespondentMastersNode;
    protected TreeNode bidTapeManagementNode;
    protected TreeNode editBidTapeManagementNode;
    private AclFeature[] aclFeatures = new AclFeature[20]
    {
      AclFeature.TradeTab_Trades,
      AclFeature.TradeTab_EditTrades,
      AclFeature.TradeTab_SecurityTrades,
      AclFeature.TradeTab_EditSecurityTrades,
      AclFeature.TradeTab_LoanSearch,
      AclFeature.TradeTab_EditLoanSearch,
      AclFeature.TradeTab_LoanTrades,
      AclFeature.TradeTab_EditLoanTrades,
      AclFeature.TradeTab_MasterCommitments,
      AclFeature.TradeTab_EditMasterContracts,
      AclFeature.TradeTab_MBSPools,
      AclFeature.TradeTab_EditMBSPools,
      AclFeature.TradeTab_GSECommitments,
      AclFeature.TradeTab_EditGSECommitments,
      AclFeature.TradeTab_CorrespondentTrades,
      AclFeature.TradeTab_EditCorrespondentTrades,
      AclFeature.TradeTab_CorrespondentMasters,
      AclFeature.TradeTab_EditCorrespondentMasters,
      AclFeature.TradeTab_BidTapeManagement,
      AclFeature.TradeTab_EditBidTapeManagement
    };

    public TradesSecurityHelper(Sessions.Session session, string userId, Persona[] personas)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new UserSecurityHelper(this.session, userId, personas, AclCategory.Features, this.aclFeatures);
    }

    public TradesSecurityHelper(Sessions.Session session, int personaId)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new PersonaSecurityHelper(this.session, personaId, AclCategory.Features, this.aclFeatures);
    }

    public override void BuildNodes(TreeView treeView)
    {
      this.tradesNode = new TreeNode("Access to Trades Tabs");
      this.nodeToFeature = new Hashtable();
      this.nodeToFeature.Add((object) this.tradesNode, (object) AclFeature.TradeTab_Trades);
      this.featureToNodeTbl = new Hashtable();
      this.featureToNodeTbl.Add((object) AclFeature.TradeTab_Trades, (object) this.tradesNode);
      this.nodeToUpdateStatus = new Hashtable();
      this.nodeToUpdateStatus.Add((object) this.tradesNode, (object) false);
      this.securityTradesNode = new TreeNode("Security Trades Tab");
      this.editSecurityTradesNode = new TreeNode("Edit Security Trades");
      this.securityTradesNode.Nodes.AddRange(new TreeNode[1]
      {
        this.editSecurityTradesNode
      });
      this.tradesNode.Nodes.AddRange(new TreeNode[1]
      {
        this.securityTradesNode
      });
      this.nodeToFeature.Add((object) this.securityTradesNode, (object) AclFeature.TradeTab_SecurityTrades);
      this.nodeToFeature.Add((object) this.editSecurityTradesNode, (object) AclFeature.TradeTab_EditSecurityTrades);
      this.featureToNodeTbl.Add((object) AclFeature.TradeTab_SecurityTrades, (object) this.securityTradesNode);
      this.featureToNodeTbl.Add((object) AclFeature.TradeTab_EditSecurityTrades, (object) this.editSecurityTradesNode);
      this.nodeToUpdateStatus.Add((object) this.securityTradesNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.editSecurityTradesNode, (object) false);
      this.loanSearchNode = new TreeNode("Loan Search Tab");
      this.editLoanSearchNode = new TreeNode("Edit Loan Search");
      this.loanSearchNode.Nodes.AddRange(new TreeNode[1]
      {
        this.editLoanSearchNode
      });
      this.tradesNode.Nodes.AddRange(new TreeNode[1]
      {
        this.loanSearchNode
      });
      this.nodeToFeature.Add((object) this.loanSearchNode, (object) AclFeature.TradeTab_LoanSearch);
      this.nodeToFeature.Add((object) this.editLoanSearchNode, (object) AclFeature.TradeTab_EditLoanSearch);
      this.featureToNodeTbl.Add((object) AclFeature.TradeTab_LoanSearch, (object) this.loanSearchNode);
      this.featureToNodeTbl.Add((object) AclFeature.TradeTab_EditLoanSearch, (object) this.editLoanSearchNode);
      this.nodeToUpdateStatus.Add((object) this.loanSearchNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.editLoanSearchNode, (object) false);
      this.loanTradesNode = new TreeNode("Loan Trades Tab");
      this.editLoanTradesNode = new TreeNode("Edit Loan Trades");
      this.loanTradesNode.Nodes.AddRange(new TreeNode[1]
      {
        this.editLoanTradesNode
      });
      this.tradesNode.Nodes.AddRange(new TreeNode[1]
      {
        this.loanTradesNode
      });
      this.nodeToFeature.Add((object) this.loanTradesNode, (object) AclFeature.TradeTab_LoanTrades);
      this.nodeToFeature.Add((object) this.editLoanTradesNode, (object) AclFeature.TradeTab_EditLoanTrades);
      this.featureToNodeTbl.Add((object) AclFeature.TradeTab_LoanTrades, (object) this.loanTradesNode);
      this.featureToNodeTbl.Add((object) AclFeature.TradeTab_EditLoanTrades, (object) this.editLoanTradesNode);
      this.nodeToUpdateStatus.Add((object) this.loanTradesNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.editLoanTradesNode, (object) false);
      this.masterCommitmentsNode = new TreeNode("Master Contracts Tab");
      this.editMasterCommitmentsNode = new TreeNode("Edit Master Contracts");
      this.masterCommitmentsNode.Nodes.AddRange(new TreeNode[1]
      {
        this.editMasterCommitmentsNode
      });
      this.tradesNode.Nodes.AddRange(new TreeNode[1]
      {
        this.masterCommitmentsNode
      });
      this.nodeToFeature.Add((object) this.masterCommitmentsNode, (object) AclFeature.TradeTab_MasterCommitments);
      this.nodeToFeature.Add((object) this.editMasterCommitmentsNode, (object) AclFeature.TradeTab_EditMasterContracts);
      this.featureToNodeTbl.Add((object) AclFeature.TradeTab_MasterCommitments, (object) this.masterCommitmentsNode);
      this.featureToNodeTbl.Add((object) AclFeature.TradeTab_EditMasterContracts, (object) this.editMasterCommitmentsNode);
      this.nodeToUpdateStatus.Add((object) this.masterCommitmentsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.editMasterCommitmentsNode, (object) false);
      this.mbsPoolsNode = new TreeNode("MBS Pools Tab");
      this.editMBSPoolsNode = new TreeNode("Edit MBS Pools");
      this.mbsPoolsNode.Nodes.AddRange(new TreeNode[1]
      {
        this.editMBSPoolsNode
      });
      this.tradesNode.Nodes.AddRange(new TreeNode[1]
      {
        this.mbsPoolsNode
      });
      this.nodeToFeature.Add((object) this.mbsPoolsNode, (object) AclFeature.TradeTab_MBSPools);
      this.nodeToFeature.Add((object) this.editMBSPoolsNode, (object) AclFeature.TradeTab_EditMBSPools);
      this.featureToNodeTbl.Add((object) AclFeature.TradeTab_MBSPools, (object) this.mbsPoolsNode);
      this.featureToNodeTbl.Add((object) AclFeature.TradeTab_EditMBSPools, (object) this.editMBSPoolsNode);
      this.nodeToUpdateStatus.Add((object) this.mbsPoolsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.editMBSPoolsNode, (object) false);
      this.gseCommitmentsNode = new TreeNode("GSE Commitments Tab");
      this.editGSECommitmentsNode = new TreeNode("Edit GSE Commitments");
      this.gseCommitmentsNode.Nodes.AddRange(new TreeNode[1]
      {
        this.editGSECommitmentsNode
      });
      this.tradesNode.Nodes.AddRange(new TreeNode[1]
      {
        this.gseCommitmentsNode
      });
      this.nodeToFeature.Add((object) this.gseCommitmentsNode, (object) AclFeature.TradeTab_GSECommitments);
      this.nodeToFeature.Add((object) this.editGSECommitmentsNode, (object) AclFeature.TradeTab_EditGSECommitments);
      this.featureToNodeTbl.Add((object) AclFeature.TradeTab_GSECommitments, (object) this.gseCommitmentsNode);
      this.featureToNodeTbl.Add((object) AclFeature.TradeTab_EditGSECommitments, (object) this.editGSECommitmentsNode);
      this.nodeToUpdateStatus.Add((object) this.gseCommitmentsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.editGSECommitmentsNode, (object) false);
      this.correspondentTradesNode = new TreeNode("Correspondent Trades Tab");
      this.editCorrespondentTradesNode = new TreeNode("Edit Correspondent Trades");
      this.correspondentTradesNode.Nodes.AddRange(new TreeNode[1]
      {
        this.editCorrespondentTradesNode
      });
      this.tradesNode.Nodes.AddRange(new TreeNode[1]
      {
        this.correspondentTradesNode
      });
      this.nodeToFeature.Add((object) this.correspondentTradesNode, (object) AclFeature.TradeTab_CorrespondentTrades);
      this.nodeToFeature.Add((object) this.editCorrespondentTradesNode, (object) AclFeature.TradeTab_EditCorrespondentTrades);
      this.featureToNodeTbl.Add((object) AclFeature.TradeTab_CorrespondentTrades, (object) this.correspondentTradesNode);
      this.featureToNodeTbl.Add((object) AclFeature.TradeTab_EditCorrespondentTrades, (object) this.editCorrespondentTradesNode);
      this.nodeToUpdateStatus.Add((object) this.correspondentTradesNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.editCorrespondentTradesNode, (object) false);
      this.correspondentMastersNode = new TreeNode("Correspondent Masters Tab");
      this.editCorrespondentMastersNode = new TreeNode("Edit Correspondent Masters");
      this.correspondentMastersNode.Nodes.AddRange(new TreeNode[1]
      {
        this.editCorrespondentMastersNode
      });
      this.tradesNode.Nodes.AddRange(new TreeNode[1]
      {
        this.correspondentMastersNode
      });
      this.nodeToFeature.Add((object) this.correspondentMastersNode, (object) AclFeature.TradeTab_CorrespondentMasters);
      this.nodeToFeature.Add((object) this.editCorrespondentMastersNode, (object) AclFeature.TradeTab_EditCorrespondentMasters);
      this.featureToNodeTbl.Add((object) AclFeature.TradeTab_CorrespondentMasters, (object) this.correspondentMastersNode);
      this.featureToNodeTbl.Add((object) AclFeature.TradeTab_EditCorrespondentMasters, (object) this.editCorrespondentMastersNode);
      this.nodeToUpdateStatus.Add((object) this.correspondentMastersNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.editCorrespondentMastersNode, (object) false);
      if (Convert.ToBoolean(this.session.ConfigurationManager.GetCompanySetting("POLICIES", "EnableBidTape")))
      {
        this.bidTapeManagementNode = new TreeNode("Bid Tape Management Tab");
        this.editBidTapeManagementNode = new TreeNode("Edit Bid Tape Management");
        this.bidTapeManagementNode.Nodes.AddRange(new TreeNode[1]
        {
          this.editBidTapeManagementNode
        });
        this.tradesNode.Nodes.Add(this.bidTapeManagementNode);
        this.nodeToFeature.Add((object) this.bidTapeManagementNode, (object) AclFeature.TradeTab_BidTapeManagement);
        this.nodeToFeature.Add((object) this.editBidTapeManagementNode, (object) AclFeature.TradeTab_EditBidTapeManagement);
        this.featureToNodeTbl.Add((object) AclFeature.TradeTab_BidTapeManagement, (object) this.bidTapeManagementNode);
        this.featureToNodeTbl.Add((object) AclFeature.TradeTab_EditBidTapeManagement, (object) this.editBidTapeManagementNode);
        this.nodeToUpdateStatus.Add((object) this.bidTapeManagementNode, (object) false);
        this.nodeToUpdateStatus.Add((object) this.editBidTapeManagementNode, (object) false);
      }
      treeView.Nodes.AddRange(new TreeNode[1]
      {
        this.tradesNode
      });
      this.securityHelper.SetTables(this.featureToNodeTbl, this.nodeToFeature, this.dependentNodes, this.nodeToUpdateStatus);
      treeView.ExpandAll();
    }
  }
}
