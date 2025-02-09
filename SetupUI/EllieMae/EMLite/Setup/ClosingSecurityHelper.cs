// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ClosingSecurityHelper
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.RemotingServices;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class ClosingSecurityHelper : FeatureSecurityHelperBase
  {
    protected TreeNode applyLoanTemplateNode;
    protected TreeNode ePASSNode;
    protected TreeNode deleteBorrowerNode;
    protected TreeNode manualExclusiveLockNode;
    protected TreeNode moveBorrowerNode;
    protected TreeNode manageBorrowerNode;
    protected TreeNode importBorrowerNode;
    protected TreeNode importBorrowerNodefromContacts;
    protected TreeNode importBorrowerNodefromLoan;
    protected TreeNode importBorrowerNodefromFNMAFile;
    protected TreeNode importBorrowerNodefromMISMOFile;
    protected TreeNode manuallyApplyMilestoneTemplateNode;
    protected TreeNode lockUnlockMilestonesList;
    protected TreeNode lockMilestoneDates;
    protected TreeNode displayMilestoneChangeScreen;
    protected TreeNode allowEmailChange;
    protected TreeNode respaTileFomrmVersion;
    protected TreeNode urlaFormVersion;
    protected TreeNode searchAllRegs;

    public ClosingSecurityHelper(Sessions.Session session, string userId, Persona[] personas)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new UserSecurityHelper(this.session, userId, personas, AclCategory.Features, FeatureSets.LoanOtherFeatures);
    }

    public ClosingSecurityHelper(Sessions.Session session, int personaId)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new PersonaSecurityHelper(this.session, personaId, AclCategory.Features, FeatureSets.LoanOtherFeatures);
    }

    public Hashtable SpecialDepTreeNodes
    {
      get => this.securityHelper.getSpecialDepTreeNodes();
      set => this.securityHelper.setSpecialDepTreeNodes(value);
    }

    public override void BuildNodes(TreeView treeView)
    {
      this.applyLoanTemplateNode = new TreeNode("Apply Loan Templates");
      this.manageBorrowerNode = new TreeNode("Manage Borrowers");
      this.moveBorrowerNode = new TreeNode("Move Borrowers");
      this.importBorrowerNode = new TreeNode("Import Borrowers");
      this.importBorrowerNodefromContacts = new TreeNode("From Contacts");
      this.importBorrowerNodefromLoan = new TreeNode("From another Loan");
      this.importBorrowerNodefromFNMAFile = new TreeNode("From a FNMA 3.2 File");
      this.importBorrowerNodefromMISMOFile = new TreeNode("From a ULAD / iLAD (MISMO 3.4)");
      this.ePASSNode = new TreeNode("Manage Service Providers List");
      this.deleteBorrowerNode = new TreeNode("Delete Borrowers");
      this.manualExclusiveLockNode = new TreeNode("Manually Block Multi-User Editing");
      this.manuallyApplyMilestoneTemplateNode = new TreeNode("Manually Apply Milestone Template");
      this.lockUnlockMilestonesList = new TreeNode("Manage Milestone Templates Mode");
      this.lockMilestoneDates = new TreeNode("Manage Milestone Dates Mode");
      this.displayMilestoneChangeScreen = new TreeNode("Display Milestone List Change Screen");
      this.allowEmailChange = new TreeNode("Modify who receives notification of access loss");
      this.respaTileFomrmVersion = new TreeNode("Change RESPA-TILA Form Version");
      this.urlaFormVersion = new TreeNode("Change URLA Form Version");
      this.searchAllRegs = new TreeNode("Search AllRegs");
      this.ePASSNode.ForeColor = Color.Blue;
      this.ePASSNode.Tag = (object) AclFeature.LoanTab_Other_ePASS;
      if (this.session.SessionObjects.AllowConcurrentEditing)
      {
        this.manageBorrowerNode.Nodes.AddRange(new TreeNode[3]
        {
          this.deleteBorrowerNode,
          this.moveBorrowerNode,
          this.importBorrowerNode
        });
        this.displayMilestoneChangeScreen.Nodes.AddRange(new TreeNode[1]
        {
          this.allowEmailChange
        });
        treeView.Nodes.AddRange(new TreeNode[9]
        {
          this.manageBorrowerNode,
          this.applyLoanTemplateNode,
          this.ePASSNode,
          this.manualExclusiveLockNode,
          this.manuallyApplyMilestoneTemplateNode,
          this.lockUnlockMilestonesList,
          this.lockMilestoneDates,
          this.displayMilestoneChangeScreen,
          this.respaTileFomrmVersion
        });
      }
      else
      {
        this.manageBorrowerNode.Nodes.AddRange(new TreeNode[3]
        {
          this.deleteBorrowerNode,
          this.moveBorrowerNode,
          this.importBorrowerNode
        });
        this.displayMilestoneChangeScreen.Nodes.AddRange(new TreeNode[1]
        {
          this.allowEmailChange
        });
        treeView.Nodes.AddRange(new TreeNode[8]
        {
          this.manageBorrowerNode,
          this.applyLoanTemplateNode,
          this.ePASSNode,
          this.manuallyApplyMilestoneTemplateNode,
          this.lockUnlockMilestonesList,
          this.lockMilestoneDates,
          this.displayMilestoneChangeScreen,
          this.respaTileFomrmVersion
        });
      }
      if (this.session.EncompassEdition == EncompassEdition.Banker & this.session.StartupInfo.AllowURLA2020)
        treeView.Nodes.Add(this.urlaFormVersion);
      treeView.Nodes.Add(this.searchAllRegs);
      this.importBorrowerNode.Nodes.AddRange(new TreeNode[4]
      {
        this.importBorrowerNodefromContacts,
        this.importBorrowerNodefromLoan,
        this.importBorrowerNodefromFNMAFile,
        this.importBorrowerNodefromMISMOFile
      });
      this.dependentNodes.AddRange((ICollection) new TreeNode[1]
      {
        this.ePASSNode
      });
      this.nodeToFeature = new Hashtable();
      this.nodeToFeature.Add((object) this.manageBorrowerNode, (object) AclFeature.LoanTab_Other_ManageBorrowers);
      this.nodeToFeature.Add((object) this.deleteBorrowerNode, (object) AclFeature.LoanTab_Other_DeleteBorrowers);
      this.nodeToFeature.Add((object) this.applyLoanTemplateNode, (object) AclFeature.LoanTab_Other_ApplyLoanTemplate);
      this.nodeToFeature.Add((object) this.moveBorrowerNode, (object) AclFeature.LoanTab_Other_MoveBorrowers);
      this.nodeToFeature.Add((object) this.importBorrowerNode, (object) AclFeature.LoanTab_Other_ImportBorrowers);
      this.nodeToFeature.Add((object) this.importBorrowerNodefromContacts, (object) AclFeature.LoanTab_Other_ImportBorrowersfromContacts);
      this.nodeToFeature.Add((object) this.importBorrowerNodefromLoan, (object) AclFeature.LoanTab_Other_ImportBorrowersfromLoan);
      this.nodeToFeature.Add((object) this.importBorrowerNodefromFNMAFile, (object) AclFeature.LoanTab_Other_ImportBorrowersfromENMA);
      this.nodeToFeature.Add((object) this.importBorrowerNodefromMISMOFile, (object) AclFeature.LoanTab_Other_ImportBorrowersfromMISMO);
      this.nodeToFeature.Add((object) this.manuallyApplyMilestoneTemplateNode, (object) AclFeature.LoanTab_ManuallyApplyMilestoneTemplate);
      this.nodeToFeature.Add((object) this.lockUnlockMilestonesList, (object) AclFeature.LoanTab_LockUnlockMilestonesList);
      this.nodeToFeature.Add((object) this.lockMilestoneDates, (object) AclFeature.LoanTab_LockMilestoneDates);
      this.nodeToFeature.Add((object) this.displayMilestoneChangeScreen, (object) AclFeature.LoanTab_DisplayMilestoneChangeScreen);
      this.nodeToFeature.Add((object) this.allowEmailChange, (object) AclFeature.LoanTab_AllowEmailChange);
      this.nodeToFeature.Add((object) this.respaTileFomrmVersion, (object) AclFeature.LoanTab_RespaTileFomrmVersion);
      this.nodeToFeature.Add((object) this.urlaFormVersion, (object) AclFeature.LoanTab_ChangeURLAFormVersion);
      this.nodeToFeature.Add((object) this.searchAllRegs, (object) AclFeature.LoanTab_SearchAllRegs);
      this.featureToNodeTbl = new Hashtable();
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_Other_ManageBorrowers, (object) this.manageBorrowerNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_Other_DeleteBorrowers, (object) this.deleteBorrowerNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_Other_ApplyLoanTemplate, (object) this.applyLoanTemplateNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_Other_MoveBorrowers, (object) this.moveBorrowerNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_Other_ImportBorrowers, (object) this.importBorrowerNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_Other_ImportBorrowersfromContacts, (object) this.importBorrowerNodefromContacts);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_Other_ImportBorrowersfromLoan, (object) this.importBorrowerNodefromLoan);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_Other_ImportBorrowersfromENMA, (object) this.importBorrowerNodefromFNMAFile);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_Other_ImportBorrowersfromMISMO, (object) this.importBorrowerNodefromMISMOFile);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_ManuallyApplyMilestoneTemplate, (object) this.manuallyApplyMilestoneTemplateNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_LockUnlockMilestonesList, (object) this.lockUnlockMilestonesList);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_LockMilestoneDates, (object) this.lockMilestoneDates);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_DisplayMilestoneChangeScreen, (object) this.displayMilestoneChangeScreen);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_AllowEmailChange, (object) this.allowEmailChange);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_RespaTileFomrmVersion, (object) this.respaTileFomrmVersion);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_ChangeURLAFormVersion, (object) this.urlaFormVersion);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_SearchAllRegs, (object) this.searchAllRegs);
      this.nodeToUpdateStatus = new Hashtable();
      this.nodeToUpdateStatus.Add((object) this.manageBorrowerNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.deleteBorrowerNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.applyLoanTemplateNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.moveBorrowerNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.importBorrowerNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.importBorrowerNodefromContacts, (object) false);
      this.nodeToUpdateStatus.Add((object) this.importBorrowerNodefromLoan, (object) false);
      this.nodeToUpdateStatus.Add((object) this.importBorrowerNodefromFNMAFile, (object) false);
      this.nodeToUpdateStatus.Add((object) this.importBorrowerNodefromMISMOFile, (object) false);
      this.nodeToUpdateStatus.Add((object) this.manuallyApplyMilestoneTemplateNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.lockUnlockMilestonesList, (object) false);
      this.nodeToUpdateStatus.Add((object) this.lockMilestoneDates, (object) false);
      this.nodeToUpdateStatus.Add((object) this.displayMilestoneChangeScreen, (object) false);
      this.nodeToUpdateStatus.Add((object) this.allowEmailChange, (object) false);
      this.nodeToUpdateStatus.Add((object) this.respaTileFomrmVersion, (object) false);
      this.nodeToUpdateStatus.Add((object) this.urlaFormVersion, (object) false);
      this.nodeToUpdateStatus.Add((object) this.searchAllRegs, (object) false);
      if (this.session.SessionObjects.AllowConcurrentEditing)
      {
        this.nodeToFeature.Add((object) this.manualExclusiveLockNode, (object) AclFeature.LoanTab_Other_ManualExclusiveLock);
        this.featureToNodeTbl.Add((object) AclFeature.LoanTab_Other_ManualExclusiveLock, (object) this.manualExclusiveLockNode);
        this.nodeToUpdateStatus.Add((object) this.manualExclusiveLockNode, (object) false);
      }
      this.securityHelper.SetTables(this.featureToNodeTbl, this.nodeToFeature, this.dependentNodes, this.nodeToUpdateStatus);
      treeView.ExpandAll();
    }
  }
}
