// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PersonaSetup.FeatureSecurity.ePass.ePassOtherHelper
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DocEngine;
using EllieMae.EMLite.RemotingServices;
using System.Collections;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.PersonaSetup.FeatureSecurity.ePass
{
  public class ePassOtherHelper : FeatureSecurityHelperBase
  {
    private TreeNode newDuplicateDocNode;
    private TreeNode manageAccessDocNode;
    private TreeNode removeAccessFromProtectedRoleNode;
    private TreeNode sendConsentNode;
    private TreeNode requestBorrowerDocumentsNode;
    private TreeNode requestEllieMaeNetworkServicesNode;
    private TreeNode eDisclosureNode;
    private TreeNode addingAdditionalDocsNode;
    private TreeNode orderDocsWithCompliance;
    private TreeNode deselectingDocsNode;
    private TreeNode rearrangeeDisclosuresNode;
    private TreeNode retrieveBorrowerDocumentsNode;
    private TreeNode retrieveEllieMaeNetworkServicesNode;
    private TreeNode sendFilesNode;
    private TreeNode sendFilestoLenderNode;
    private TreeNode archiveDocsNode;
    private TreeNode viewAllAnnotationsNode;
    private TreeNode packagesTabNode;
    private TreeNode eSignPackagesNode;

    public ePassOtherHelper(Sessions.Session session, string userId, Persona[] personas)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new UserSecurityHelper(this.session, userId, personas, AclCategory.Features, FeatureSets.LoanEFolderFeatures);
    }

    public ePassOtherHelper(Sessions.Session session, int personaId)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new PersonaSecurityHelper(this.session, personaId, AclCategory.Features, FeatureSets.LoanEFolderFeatures);
    }

    public override void BuildNodes(TreeView treeView)
    {
      ILoanServices service = this.session.Application.GetService<ILoanServices>();
      this.newDuplicateDocNode = new TreeNode("Create/Duplicate Documents");
      this.manageAccessDocNode = new TreeNode("Manage Access to Documents");
      this.removeAccessFromProtectedRoleNode = new TreeNode("Remove Access From Protected Roles");
      this.manageAccessDocNode.Nodes.AddRange(new TreeNode[1]
      {
        this.removeAccessFromProtectedRoleNode
      });
      this.sendConsentNode = new TreeNode("Send Consent");
      this.requestBorrowerDocumentsNode = new TreeNode("Request Borrower Documents");
      this.requestEllieMaeNetworkServicesNode = new TreeNode("Request ICE Mortgage Technology Network Services");
      this.eDisclosureNode = new TreeNode("eDisclosures");
      this.addingAdditionalDocsNode = new TreeNode("Add Additional Docs");
      this.orderDocsWithCompliance = new TreeNode("Web Order Disclosures even with Compliance Review Failures");
      this.deselectingDocsNode = new TreeNode("Deselect Docs");
      this.rearrangeeDisclosuresNode = new TreeNode("Move Docs Up/Down");
      this.retrieveBorrowerDocumentsNode = new TreeNode("Retrieve Borrower Documents");
      this.retrieveEllieMaeNetworkServicesNode = new TreeNode("Retrieve ICE Mortgage Technology Network Services");
      this.sendFilesNode = new TreeNode("Send Files");
      this.sendFilestoLenderNode = new TreeNode("Send Files to Lender");
      this.archiveDocsNode = new TreeNode("Archive Documents");
      this.viewAllAnnotationsNode = new TreeNode("View All Annotations");
      this.packagesTabNode = new TreeNode("Packages Tab");
      this.eSignPackagesNode = new TreeNode("eSign Packages");
      bool result = false;
      bool.TryParse(this.session.ConfigurationManager.GetCompanySetting("eDisclosures", "IncludeComplianceReportWithAudit"), out result);
      if (this.session.EncompassEdition == EncompassEdition.Banker & result)
        this.eDisclosureNode.Nodes.Add(this.orderDocsWithCompliance);
      if (service.IsEncompassDocServiceAvailable(DocumentOrderType.Opening))
      {
        this.eDisclosureNode.Nodes.AddRange(new TreeNode[3]
        {
          this.addingAdditionalDocsNode,
          this.rearrangeeDisclosuresNode,
          this.deselectingDocsNode
        });
      }
      else
      {
        this.eDisclosureNode.Nodes.AddRange(new TreeNode[1]
        {
          this.addingAdditionalDocsNode
        });
        this.addingAdditionalDocsNode.Text = "Add/Remove eDisclosures to Send";
      }
      this.packagesTabNode.Nodes.AddRange(new TreeNode[1]
      {
        this.eSignPackagesNode
      });
      treeView.Nodes.AddRange(new TreeNode[13]
      {
        this.newDuplicateDocNode,
        this.manageAccessDocNode,
        this.sendConsentNode,
        this.requestBorrowerDocumentsNode,
        this.requestEllieMaeNetworkServicesNode,
        this.eDisclosureNode,
        this.retrieveBorrowerDocumentsNode,
        this.retrieveEllieMaeNetworkServicesNode,
        this.sendFilesNode,
        this.sendFilestoLenderNode,
        this.archiveDocsNode,
        this.viewAllAnnotationsNode,
        this.packagesTabNode
      });
      this.nodeToFeature = new Hashtable();
      this.nodeToFeature.Add((object) this.newDuplicateDocNode, (object) AclFeature.eFolder_Other_NewDuplicateDoc);
      this.nodeToFeature.Add((object) this.manageAccessDocNode, (object) AclFeature.eFolder_Other_ManageAccessToDocs);
      this.nodeToFeature.Add((object) this.removeAccessFromProtectedRoleNode, (object) AclFeature.eFolder_Other_MAD_RemoveAccessFromProtectedRoles);
      this.nodeToFeature.Add((object) this.sendConsentNode, (object) AclFeature.eFolder_Other_SendConsent);
      this.nodeToFeature.Add((object) this.requestBorrowerDocumentsNode, (object) AclFeature.eFolder_Other_RequestBorrowerDocuments);
      this.nodeToFeature.Add((object) this.requestEllieMaeNetworkServicesNode, (object) AclFeature.eFolder_Other_RequestEllieMaeNetworkServices);
      this.nodeToFeature.Add((object) this.eDisclosureNode, (object) AclFeature.eFolder_Other_eDisclosure);
      this.nodeToFeature.Add((object) this.addingAdditionalDocsNode, (object) AclFeature.eFolder_Other_eDisclosure_AddAdditionalDocs);
      this.nodeToFeature.Add((object) this.orderDocsWithCompliance, (object) AclFeature.eFolder_orderdocs_compliance_failure);
      this.nodeToFeature.Add((object) this.deselectingDocsNode, (object) AclFeature.eFolder_Other_eDisclosure_DeselectDocs);
      this.nodeToFeature.Add((object) this.rearrangeeDisclosuresNode, (object) AclFeature.eFolder_Other_eDisclosure_RearrangeeDisclosures);
      this.nodeToFeature.Add((object) this.retrieveBorrowerDocumentsNode, (object) AclFeature.eFolder_Other_RetrieveBorrowerDocuments);
      this.nodeToFeature.Add((object) this.retrieveEllieMaeNetworkServicesNode, (object) AclFeature.eFolder_Other_RetrieveEllieMaeNetworkServices);
      this.nodeToFeature.Add((object) this.sendFilesNode, (object) AclFeature.eFolder_Other_SendFiles);
      this.nodeToFeature.Add((object) this.sendFilestoLenderNode, (object) AclFeature.eFolder_Other_SendFilesToLender);
      this.nodeToFeature.Add((object) this.archiveDocsNode, (object) AclFeature.eFolder_Other_ArchiveDocs);
      this.nodeToFeature.Add((object) this.viewAllAnnotationsNode, (object) AclFeature.eFolder_Other_ViewAllAnnotations);
      this.nodeToFeature.Add((object) this.packagesTabNode, (object) AclFeature.eFolder_Other_PackagesTab);
      this.nodeToFeature.Add((object) this.eSignPackagesNode, (object) AclFeature.eFolder_Other_eSignPackages);
      this.featureToNodeTbl = new Hashtable(FeatureSets.Features.Length);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Other_NewDuplicateDoc, (object) this.newDuplicateDocNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Other_ManageAccessToDocs, (object) this.manageAccessDocNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Other_MAD_RemoveAccessFromProtectedRoles, (object) this.removeAccessFromProtectedRoleNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Other_SendConsent, (object) this.sendConsentNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Other_RequestBorrowerDocuments, (object) this.requestBorrowerDocumentsNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Other_RequestEllieMaeNetworkServices, (object) this.requestEllieMaeNetworkServicesNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Other_eDisclosure, (object) this.eDisclosureNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Other_eDisclosure_AddAdditionalDocs, (object) this.addingAdditionalDocsNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_orderdocs_compliance_failure, (object) this.orderDocsWithCompliance);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Other_eDisclosure_DeselectDocs, (object) this.deselectingDocsNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Other_eDisclosure_RearrangeeDisclosures, (object) this.rearrangeeDisclosuresNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Other_RetrieveBorrowerDocuments, (object) this.retrieveBorrowerDocumentsNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Other_RetrieveEllieMaeNetworkServices, (object) this.retrieveEllieMaeNetworkServicesNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Other_SendFiles, (object) this.sendFilesNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Other_SendFilesToLender, (object) this.sendFilestoLenderNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Other_ArchiveDocs, (object) this.archiveDocsNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Other_ViewAllAnnotations, (object) this.viewAllAnnotationsNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Other_PackagesTab, (object) this.packagesTabNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Other_eSignPackages, (object) this.eSignPackagesNode);
      this.nodeToUpdateStatus = new Hashtable();
      this.nodeToUpdateStatus.Add((object) this.newDuplicateDocNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.manageAccessDocNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.removeAccessFromProtectedRoleNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.sendConsentNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.requestBorrowerDocumentsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.requestEllieMaeNetworkServicesNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.orderDocsWithCompliance, (object) false);
      this.nodeToUpdateStatus.Add((object) this.eDisclosureNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.addingAdditionalDocsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.deselectingDocsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.rearrangeeDisclosuresNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.retrieveBorrowerDocumentsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.retrieveEllieMaeNetworkServicesNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.sendFilesNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.sendFilestoLenderNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.archiveDocsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.viewAllAnnotationsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.packagesTabNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.eSignPackagesNode, (object) false);
      this.securityHelper.SetTables(this.featureToNodeTbl, this.nodeToFeature, this.dependentNodes, this.nodeToUpdateStatus);
      treeView.ExpandAll();
    }
  }
}
