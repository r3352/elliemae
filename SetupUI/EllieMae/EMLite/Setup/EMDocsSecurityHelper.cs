// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.EMDocsSecurityHelper
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DocEngine;
using EllieMae.EMLite.RemotingServices;
using System.Collections;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class EMDocsSecurityHelper : FeatureSecurityHelperBase
  {
    protected TreeNode orderDocsNode;
    protected TreeNode orderPreClosingDocsNode;
    protected TreeNode orderDocsComplianceFeatureNode;
    protected TreeNode orderAdditionalDocsNode;
    protected TreeNode orderMoveDocsNode;
    protected TreeNode orderDeselectNode;
    protected TreeNode orderDocsWithAuditFailuresNode;
    protected TreeNode rearrangeDocsNode;
    protected TreeNode viewDocDataNode;
    protected TreeNode overrideDocDataNode;
    protected TreeNode manageAltLendersNode;
    protected TreeNode addingclosingdocsNode;
    protected TreeNode deselectingclosingdocsNode;
    protected TreeNode encompasseCloseNode;
    protected TreeNode configureClosingOptionsNode;
    protected TreeNode orderDocsdigitalClosingNode;
    protected TreeNode orderDocsWithAuditFailuresdigitalClosingNode;
    protected TreeNode addingClosingDocsdigitalClosingNode;
    protected TreeNode rearrangeDocsdigitalClosingNode;
    protected TreeNode deselectingClosingDocsdigitalClosingNode;
    protected TreeNode closingPkgMgtdigitalClosingNode;
    protected TreeNode approveForSigndigitalClosingNode;
    protected TreeNode packagePreviewdigitalClosingNode;
    protected TreeNode packageExirationdigitalClosingNode;
    protected TreeNode packageSigningDateClosingNode;
    protected TreeNode eNoteTabNode;
    protected TreeNode reverseRegistrationNode;
    protected TreeNode transferNode;
    protected TreeNode deactivateNode;
    protected TreeNode reverseDeactivateNode;

    public EMDocsSecurityHelper(Sessions.Session session, string userId, Persona[] personas)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new UserSecurityHelper(this.session, userId, personas, AclCategory.Features, FeatureSets.EMClosingDocsFeatures);
    }

    public EMDocsSecurityHelper(Sessions.Session session, int personaId)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new PersonaSecurityHelper(this.session, personaId, AclCategory.Features, FeatureSets.EMClosingDocsFeatures);
    }

    public Hashtable SpecialDepTreeNodes
    {
      get => this.securityHelper.getSpecialDepTreeNodes();
      set => this.securityHelper.setSpecialDepTreeNodes(value);
    }

    public override void BuildNodes(TreeView treeView)
    {
      this.orderDocsNode = new TreeNode("Order Closing Docs");
      this.orderPreClosingDocsNode = new TreeNode("Web Order Pre-Closing Docs");
      this.orderDocsComplianceFeatureNode = new TreeNode("Order Docs even with Compliance Review Failures");
      this.orderAdditionalDocsNode = new TreeNode("Add Additional Docs");
      this.orderMoveDocsNode = new TreeNode("Move Docs Up/Down");
      this.orderDeselectNode = new TreeNode("Deselect Docs");
      this.orderDocsWithAuditFailuresNode = new TreeNode("Order Docs even with Compliance Review Failures");
      this.addingclosingdocsNode = new TreeNode("Add Additional Docs");
      this.rearrangeDocsNode = new TreeNode("Move Docs Up/Down");
      this.deselectingclosingdocsNode = new TreeNode("Deselect Docs");
      this.viewDocDataNode = new TreeNode("View Closing Document Data");
      this.overrideDocDataNode = new TreeNode("Override Closing Document Data fields");
      this.manageAltLendersNode = new TreeNode("Manage Alt Lenders");
      this.encompasseCloseNode = new TreeNode("Encompass eClose");
      this.configureClosingOptionsNode = new TreeNode("Configure Closing Options");
      this.orderDocsdigitalClosingNode = new TreeNode("Order Closing Docs");
      this.orderDocsWithAuditFailuresdigitalClosingNode = new TreeNode("Order Docs even with Compliance Review Failures");
      this.addingClosingDocsdigitalClosingNode = new TreeNode("Add Additional Docs");
      this.rearrangeDocsdigitalClosingNode = new TreeNode("Move Docs Up/Down");
      this.deselectingClosingDocsdigitalClosingNode = new TreeNode("Deselect Docs");
      this.closingPkgMgtdigitalClosingNode = new TreeNode("Closing Package Managment");
      this.approveForSigndigitalClosingNode = new TreeNode("Approve for Signing");
      this.packagePreviewdigitalClosingNode = new TreeNode("Package Preview");
      this.packageExirationdigitalClosingNode = new TreeNode("Package Expiration");
      this.packageSigningDateClosingNode = new TreeNode("Package Signing Date");
      this.eNoteTabNode = new TreeNode("eNote");
      this.reverseRegistrationNode = new TreeNode("Reverse Registration");
      this.transferNode = new TreeNode("Transfer");
      this.deactivateNode = new TreeNode("Deactivate an eNote");
      this.reverseDeactivateNode = new TreeNode("Reverse Deactivation of eNote");
      this.approveForSigndigitalClosingNode.Nodes.AddRange(new TreeNode[3]
      {
        this.packageSigningDateClosingNode,
        this.packagePreviewdigitalClosingNode,
        this.packageExirationdigitalClosingNode
      });
      this.closingPkgMgtdigitalClosingNode.Nodes.AddRange(new TreeNode[1]
      {
        this.approveForSigndigitalClosingNode
      });
      this.orderDocsdigitalClosingNode.Nodes.AddRange(new TreeNode[4]
      {
        this.orderDocsWithAuditFailuresdigitalClosingNode,
        this.addingClosingDocsdigitalClosingNode,
        this.rearrangeDocsdigitalClosingNode,
        this.deselectingClosingDocsdigitalClosingNode
      });
      this.eNoteTabNode.Nodes.AddRange(new TreeNode[4]
      {
        this.reverseRegistrationNode,
        this.transferNode,
        this.deactivateNode,
        this.reverseDeactivateNode
      });
      this.encompasseCloseNode.Nodes.AddRange(new TreeNode[4]
      {
        this.configureClosingOptionsNode,
        this.orderDocsdigitalClosingNode,
        this.closingPkgMgtdigitalClosingNode,
        this.eNoteTabNode
      });
      this.viewDocDataNode.Nodes.AddRange(new TreeNode[1]
      {
        this.overrideDocDataNode
      });
      if (this.session.Application.GetService<ILoanServices>().IsEncompassDocServiceAvailable(DocumentOrderType.Closing))
      {
        treeView.Nodes.AddRange(new TreeNode[5]
        {
          this.orderPreClosingDocsNode,
          this.orderDocsNode,
          this.encompasseCloseNode,
          this.viewDocDataNode,
          this.manageAltLendersNode
        });
        this.orderDocsNode.Nodes.AddRange(new TreeNode[4]
        {
          this.orderDocsWithAuditFailuresNode,
          this.addingclosingdocsNode,
          this.rearrangeDocsNode,
          this.deselectingclosingdocsNode
        });
      }
      else
        treeView.Nodes.AddRange(new TreeNode[4]
        {
          this.orderPreClosingDocsNode,
          this.orderDocsNode,
          this.encompasseCloseNode,
          this.manageAltLendersNode
        });
      this.orderPreClosingDocsNode.Nodes.AddRange(new TreeNode[4]
      {
        this.orderDocsComplianceFeatureNode,
        this.orderAdditionalDocsNode,
        this.orderMoveDocsNode,
        this.orderDeselectNode
      });
      this.nodeToFeature = new Hashtable();
      this.nodeToFeature.Add((object) this.orderDocsNode, (object) AclFeature.LoanTab_EMClosingDocs_OrderDocs);
      this.nodeToFeature.Add((object) this.orderPreClosingDocsNode, (object) AclFeature.LoanTab_EMClosingDocs_PreCloseDocs);
      this.nodeToFeature.Add((object) this.orderDocsWithAuditFailuresNode, (object) AclFeature.LoanTab_EMClosingDocs_OrderDocsWithAuditFailures);
      this.nodeToFeature.Add((object) this.orderDocsComplianceFeatureNode, (object) AclFeature.LoanTab_EMClosingDocs_PreClosingComplianceDocs);
      this.nodeToFeature.Add((object) this.orderAdditionalDocsNode, (object) AclFeature.LoanTab_EMClosingDocs_PreClosingAdditionalDocs);
      this.nodeToFeature.Add((object) this.orderMoveDocsNode, (object) AclFeature.LoanTab_EMClosingDocs_PreClosingMoveDocs);
      this.nodeToFeature.Add((object) this.orderDeselectNode, (object) AclFeature.LoanTab_EMClosingDocs_PreClosingDeselectDocs);
      this.nodeToFeature.Add((object) this.rearrangeDocsNode, (object) AclFeature.LoanTab_EMClosingDocs_RearrangeDocs);
      this.nodeToFeature.Add((object) this.viewDocDataNode, (object) AclFeature.LoanTab_EMClosingDocs_ViewDocData);
      this.nodeToFeature.Add((object) this.overrideDocDataNode, (object) AclFeature.LoanTab_EMClosingDocs_OverrideDocData);
      this.nodeToFeature.Add((object) this.manageAltLendersNode, (object) AclFeature.LoanTab_Other_AltLender);
      this.nodeToFeature.Add((object) this.addingclosingdocsNode, (object) AclFeature.LoanTab_EMClosingDocs_AddClosingDocs);
      this.nodeToFeature.Add((object) this.deselectingclosingdocsNode, (object) AclFeature.LoanTab_EMClosingDocs_DeselectClosingDocs);
      this.featureToNodeTbl = new Hashtable();
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_EMClosingDocs_OrderDocs, (object) this.orderDocsNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_EMClosingDocs_PreCloseDocs, (object) this.orderPreClosingDocsNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_EMClosingDocs_PreClosingComplianceDocs, (object) this.orderDocsComplianceFeatureNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_EMClosingDocs_PreClosingAdditionalDocs, (object) this.orderAdditionalDocsNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_EMClosingDocs_PreClosingMoveDocs, (object) this.orderMoveDocsNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_EMClosingDocs_PreClosingDeselectDocs, (object) this.orderDeselectNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_EMClosingDocs_OrderDocsWithAuditFailures, (object) this.orderDocsWithAuditFailuresNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_EMClosingDocs_RearrangeDocs, (object) this.rearrangeDocsNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_EMClosingDocs_ViewDocData, (object) this.viewDocDataNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_EMClosingDocs_OverrideDocData, (object) this.overrideDocDataNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_Other_AltLender, (object) this.manageAltLendersNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_EMClosingDocs_AddClosingDocs, (object) this.addingclosingdocsNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_EMClosingDocs_DeselectClosingDocs, (object) this.deselectingclosingdocsNode);
      this.nodeToUpdateStatus = new Hashtable();
      this.nodeToUpdateStatus.Add((object) this.orderDocsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.orderDocsWithAuditFailuresNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.rearrangeDocsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.viewDocDataNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.overrideDocDataNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.manageAltLendersNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.addingclosingdocsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.deselectingclosingdocsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.orderPreClosingDocsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.orderDocsComplianceFeatureNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.orderAdditionalDocsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.orderMoveDocsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.orderDeselectNode, (object) false);
      this.dependentNodes.Add((object) this.encompasseCloseNode);
      this.nodeToFeature.Add((object) this.configureClosingOptionsNode, (object) AclFeature.LoanTab_EMClosingDocs_ConfigureClosingOptionsDigitalClosing);
      this.nodeToFeature.Add((object) this.orderDocsdigitalClosingNode, (object) AclFeature.LoanTab_EMClosingDocs_OrderDocsDigitalClosing);
      this.nodeToFeature.Add((object) this.orderDocsWithAuditFailuresdigitalClosingNode, (object) AclFeature.LoanTab_EMClosingDocs_OrderDocsWithAuditFailuresDigitalClosing);
      this.nodeToFeature.Add((object) this.addingClosingDocsdigitalClosingNode, (object) AclFeature.LoanTab_EMClosingDocs_AddClosingDocsDigitalClosing);
      this.nodeToFeature.Add((object) this.rearrangeDocsdigitalClosingNode, (object) AclFeature.LoanTab_EMClosingDocs_RearrangeDocsDigitalClosing);
      this.nodeToFeature.Add((object) this.deselectingClosingDocsdigitalClosingNode, (object) AclFeature.LoanTab_EMClosingDocs_DeselectClosingDocsDigitalClosing);
      this.nodeToFeature.Add((object) this.closingPkgMgtdigitalClosingNode, (object) AclFeature.LoanTab_EMClosingDocs_ClosingPackageManagmentDigitalClosing);
      this.nodeToFeature.Add((object) this.approveForSigndigitalClosingNode, (object) AclFeature.LoanTab_EMClosingDocs_ApproveForSigningDigitalClosing);
      this.nodeToFeature.Add((object) this.packagePreviewdigitalClosingNode, (object) AclFeature.LoanTab_EMClosingDocs_PackagePreviewDigitalClosing);
      this.nodeToFeature.Add((object) this.packageExirationdigitalClosingNode, (object) AclFeature.LoanTab_EMClosingDocs_PackageExpirationDigitalClosing);
      this.nodeToFeature.Add((object) this.packageSigningDateClosingNode, (object) AclFeature.LoanTab_EMClosingDocs_PackageSigningDateDigitalClosing);
      this.nodeToFeature.Add((object) this.eNoteTabNode, (object) AclFeature.LoanTab_EMClosingDocs_eNoteTab);
      this.nodeToFeature.Add((object) this.reverseRegistrationNode, (object) AclFeature.LoanTab_EMClosingDocs_ENoteTab_ReverseRegistration);
      this.nodeToFeature.Add((object) this.transferNode, (object) AclFeature.LoanTab_EMClosingDocs_ENoteTab_Transfer);
      this.nodeToFeature.Add((object) this.deactivateNode, (object) AclFeature.LoanTab_EMClosingDocs_ENoteTab_Deactivate);
      this.nodeToFeature.Add((object) this.reverseDeactivateNode, (object) AclFeature.LoanTab_EMClosingDocs_ENoteTab_ReverseDeactivation);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_EMClosingDocs_ConfigureClosingOptionsDigitalClosing, (object) this.configureClosingOptionsNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_EMClosingDocs_OrderDocsDigitalClosing, (object) this.orderDocsdigitalClosingNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_EMClosingDocs_OrderDocsWithAuditFailuresDigitalClosing, (object) this.orderDocsWithAuditFailuresdigitalClosingNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_EMClosingDocs_AddClosingDocsDigitalClosing, (object) this.addingClosingDocsdigitalClosingNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_EMClosingDocs_RearrangeDocsDigitalClosing, (object) this.rearrangeDocsdigitalClosingNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_EMClosingDocs_DeselectClosingDocsDigitalClosing, (object) this.deselectingClosingDocsdigitalClosingNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_EMClosingDocs_ClosingPackageManagmentDigitalClosing, (object) this.closingPkgMgtdigitalClosingNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_EMClosingDocs_ApproveForSigningDigitalClosing, (object) this.approveForSigndigitalClosingNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_EMClosingDocs_PackagePreviewDigitalClosing, (object) this.packagePreviewdigitalClosingNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_EMClosingDocs_PackageExpirationDigitalClosing, (object) this.packageExirationdigitalClosingNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_EMClosingDocs_PackageSigningDateDigitalClosing, (object) this.packageSigningDateClosingNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_EMClosingDocs_eNoteTab, (object) this.eNoteTabNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_EMClosingDocs_ENoteTab_ReverseRegistration, (object) this.reverseRegistrationNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_EMClosingDocs_ENoteTab_Transfer, (object) this.transferNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_EMClosingDocs_ENoteTab_Deactivate, (object) this.deactivateNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_EMClosingDocs_ENoteTab_ReverseDeactivation, (object) this.reverseDeactivateNode);
      this.nodeToUpdateStatus.Add((object) this.configureClosingOptionsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.orderDocsdigitalClosingNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.orderDocsWithAuditFailuresdigitalClosingNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.addingClosingDocsdigitalClosingNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.rearrangeDocsdigitalClosingNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.deselectingClosingDocsdigitalClosingNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.closingPkgMgtdigitalClosingNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.approveForSigndigitalClosingNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.packagePreviewdigitalClosingNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.packageExirationdigitalClosingNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.packageSigningDateClosingNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.eNoteTabNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.reverseRegistrationNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.transferNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.deactivateNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.reverseDeactivateNode, (object) false);
      this.securityHelper.SetTables(this.featureToNodeTbl, this.nodeToFeature, this.dependentNodes, this.nodeToUpdateStatus);
      treeView.ExpandAll();
    }
  }
}
