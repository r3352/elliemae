// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PersonaSetup.SecurityPage.EnhancedConditionSecurityHelper
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.PersonaSetup.SecurityPage
{
  internal class EnhancedConditionSecurityHelper : FeatureSecurityHelperBase
  {
    protected new Hashtable featureToNodeTbl;
    protected new Hashtable nodeToFeature;
    protected new Hashtable nodeToUpdateStatus;
    protected TreeNode accessConditionsNode;
    protected TreeNode addConditionsNode;
    protected TreeNode createBlankConditionsNode;
    protected TreeNode addAutomatedConditionsNode;
    protected TreeNode importConditionsNode;
    protected TreeNode editConditionsNode;
    protected TreeNode changePriorToNode;
    protected TreeNode internalDescriptionNode;
    protected TreeNode externalDescriptionNode;
    protected TreeNode printInternallyNode;
    protected TreeNode printExternallyNode;
    protected TreeNode addCommentsNode;
    protected TreeNode removeCommentsNode;
    protected TreeNode markCommentsNode;
    protected TreeNode assignDocumentsNode;
    protected TreeNode unassignDocumentsNode;
    protected TreeNode deleteConditionsNode;
    protected TreeNode importAllConditionsNode;
    protected TreeNode reviewAndImportConditionsNode;
    protected TreeNode deliveryConditionResponses;
    protected TreeNode ViewConditionDeliveryStatus;
    protected TreeNode importAUSFindingsNode;
    protected TreeNode importLoanQualityFindingsNode;
    private AclEnhancedConditionType[] enhancedConditionTypeFeatures = (AclEnhancedConditionType[]) Enum.GetValues(typeof (AclEnhancedConditionType));
    private Guid currentEnhancedConditionType;
    private int personaID;
    private string userId;
    private bool bIsUserSetup;
    private bool bIsInvestorDelivery;
    private Persona[] personas;

    public EnhancedConditionSecurityHelper(
      Sessions.Session session,
      int personaId,
      Guid enhancedConditionType,
      bool isInvestorDelivery = false)
      : base(session)
    {
      this.currentEnhancedConditionType = enhancedConditionType;
      this.personaID = personaId;
      this.bIsUserSetup = false;
      this.bIsInvestorDelivery = isInvestorDelivery;
      this.securityHelper = (ILevelSecurityHelper) new PersonaSecurityHelper(this.session, personaId, this.enhancedConditionTypeFeatures);
    }

    public EnhancedConditionSecurityHelper(
      Sessions.Session session,
      string userId,
      Persona[] personas,
      Guid enhancedConditionType,
      bool isInvestorDelivery = false)
      : base(session)
    {
      this.currentEnhancedConditionType = enhancedConditionType;
      this.personas = personas;
      this.userId = userId;
      this.currentEnhancedConditionType = enhancedConditionType;
      this.bIsUserSetup = true;
      this.bIsInvestorDelivery = isInvestorDelivery;
      this.securityHelper = (ILevelSecurityHelper) new UserSecurityHelper(this.session, userId, personas, this.enhancedConditionTypeFeatures);
    }

    public override void SetNodeStates()
    {
      if (!this.bIsUserSetup)
      {
        Hashtable personaPermissions = this.GetPersonaPermissions();
        if (personaPermissions == null)
          throw new ApplicationException("Feature rights from a call to GetPersonaPermissions is null.");
        for (int index = 0; index < this.enhancedConditionTypeFeatures.Length; ++index)
        {
          if (this.featureToNodeTbl.Contains((object) this.enhancedConditionTypeFeatures[index]))
          {
            TreeNode treeNode = (TreeNode) this.featureToNodeTbl[(object) this.enhancedConditionTypeFeatures[index]];
            int conditionTypeFeature = (int) this.enhancedConditionTypeFeatures[index];
            treeNode.Checked = personaPermissions.ContainsKey((object) conditionTypeFeature.ToString()) && (string) personaPermissions[(object) conditionTypeFeature.ToString()] == "1";
          }
        }
      }
      else
      {
        EnhancedConditionsAclManager aclManager = (EnhancedConditionsAclManager) this.session.ACL.GetAclManager(AclCategory.EnhancedConditions);
        Hashtable permissions1 = aclManager.GetPermissions(this.enhancedConditionTypeFeatures, this.currentEnhancedConditionType, this.userId);
        Hashtable permissions2 = aclManager.GetPermissions(this.enhancedConditionTypeFeatures, this.currentEnhancedConditionType, this.personas);
        if (permissions1 == null)
          throw new ApplicationException("User feature rights from a call to GetPersonaPermissions is null.");
        if (permissions2 == null)
          throw new ApplicationException("Persona feature rights from a call to GetPersonaPermissions is null.");
        Hashtable hashtable = new Hashtable(this.enhancedConditionTypeFeatures.Length);
        for (int index = 0; index < this.enhancedConditionTypeFeatures.Length; ++index)
        {
          if (this.featureToNodeTbl.Contains((object) this.enhancedConditionTypeFeatures[index]))
          {
            TreeNode node = (TreeNode) this.featureToNodeTbl[(object) this.enhancedConditionTypeFeatures[index]];
            bool flag1;
            bool flag2;
            if (permissions1.Contains((object) this.enhancedConditionTypeFeatures[index]))
            {
              AclTriState aclTriState = (AclTriState) permissions1[(object) this.enhancedConditionTypeFeatures[index]];
              if (aclTriState == AclTriState.Unspecified)
              {
                flag1 = true;
                flag2 = (bool) permissions2[(object) this.enhancedConditionTypeFeatures[index]];
              }
              else
              {
                flag1 = false;
                flag2 = aclTriState == AclTriState.True;
              }
            }
            else
            {
              flag1 = true;
              flag2 = (bool) permissions2[(object) this.enhancedConditionTypeFeatures[index]];
            }
            if (flag1)
              this.SetNodeImageIndex(node, 0);
            else
              this.SetNodeImageIndex(node, 1);
            node.Checked = flag2;
          }
        }
      }
    }

    public override void SetNodeUpdateStatus(TreeNode node, bool status)
    {
      if (this.nodeToUpdateStatus == null || !this.nodeToUpdateStatus.Contains((object) node))
        return;
      this.nodeToUpdateStatus[(object) node] = (object) status;
    }

    public override Hashtable GetPersonaPermissions()
    {
      return ((EnhancedConditionsAclManager) this.session.ACL.GetAclManager(AclCategory.EnhancedConditions)).GetPersonaPermissions(this.currentEnhancedConditionType, this.personaID);
    }

    public override bool GetNodeUpdateStatus(TreeNode node)
    {
      bool nodeUpdateStatus = false;
      if (this.nodeToUpdateStatus != null && this.nodeToUpdateStatus.Contains((object) node))
        nodeUpdateStatus = (bool) this.nodeToUpdateStatus[(object) node];
      return nodeUpdateStatus;
    }

    public override Dictionary<AclEnhancedConditionType, bool> GetEnhancedConditionsUpdatedFeatures(
      bool isUserUpdate)
    {
      Dictionary<AclEnhancedConditionType, bool> conditionsUpdatedFeatures = new Dictionary<AclEnhancedConditionType, bool>();
      if (this.nodeToUpdateStatus != null)
      {
        IEnumerator enumerator = this.nodeToUpdateStatus.Keys.GetEnumerator();
        while (enumerator.MoveNext())
        {
          if ((bool) this.nodeToUpdateStatus[enumerator.Current])
          {
            TreeNode current = (TreeNode) enumerator.Current;
            AclEnhancedConditionType feature = this.NodeToFeature(current);
            if (!isUserUpdate)
            {
              if (current.Checked)
                conditionsUpdatedFeatures.Add(feature, true);
              else
                conditionsUpdatedFeatures.Add(feature, false);
            }
            else if (current.ImageIndex == 0)
              conditionsUpdatedFeatures.Add(feature, false);
            else if (current.Checked)
              conditionsUpdatedFeatures.Add(feature, true);
            else
              conditionsUpdatedFeatures.Add(feature, false);
          }
        }
      }
      return conditionsUpdatedFeatures;
    }

    public override Hashtable GetUpdatedFeatures(bool isUserUpdate)
    {
      Hashtable updatedFeatures = new Hashtable();
      if (this.nodeToUpdateStatus != null)
      {
        IEnumerator enumerator = this.nodeToUpdateStatus.Keys.GetEnumerator();
        while (enumerator.MoveNext())
        {
          if ((bool) this.nodeToUpdateStatus[enumerator.Current])
          {
            TreeNode current = (TreeNode) enumerator.Current;
            AclEnhancedConditionType feature = this.NodeToFeature(current);
            if (!isUserUpdate)
            {
              if (current.Checked)
                updatedFeatures.Add((object) feature, (object) AclTriState.True);
              else
                updatedFeatures.Add((object) feature, (object) AclTriState.False);
            }
            else if (current.ImageIndex == 0)
              updatedFeatures.Add((object) feature, (object) AclTriState.Unspecified);
            else if (current.Checked)
              updatedFeatures.Add((object) feature, (object) AclTriState.True);
            else
              updatedFeatures.Add((object) feature, (object) AclTriState.False);
          }
        }
      }
      return updatedFeatures;
    }

    public override void BuildNodes(TreeView treeView)
    {
      bool flag = this.session.EncompassEdition == EncompassEdition.Banker;
      this.accessConditionsNode = new TreeNode("Access Conditions");
      this.addConditionsNode = new TreeNode("Add Conditions");
      if (flag)
      {
        this.createBlankConditionsNode = new TreeNode("Create Blank Conditions");
        this.addAutomatedConditionsNode = new TreeNode("Add Automated Conditions");
        this.importAUSFindingsNode = new TreeNode("Import AUS Findings (DU, LPA, FHA)");
        this.importLoanQualityFindingsNode = new TreeNode("Import Loan Quality Findings (Early Check)");
      }
      this.editConditionsNode = new TreeNode("Edit Conditions");
      this.changePriorToNode = new TreeNode("Change Prior To");
      this.internalDescriptionNode = new TreeNode("Internal Description");
      this.externalDescriptionNode = new TreeNode("External Description");
      this.printInternallyNode = new TreeNode("Print Internally");
      this.printExternallyNode = new TreeNode("Print Externally");
      this.addCommentsNode = new TreeNode("Add Comments");
      this.removeCommentsNode = new TreeNode("Remove Comments");
      this.markCommentsNode = new TreeNode("Mark Comments Internal/External");
      this.assignDocumentsNode = new TreeNode("Assign Documents");
      this.unassignDocumentsNode = new TreeNode("Unassign Documents");
      this.deleteConditionsNode = new TreeNode("Delete Conditions");
      if (flag && !this.bIsInvestorDelivery)
        this.addConditionsNode.Nodes.AddRange(new TreeNode[4]
        {
          this.createBlankConditionsNode,
          this.addAutomatedConditionsNode,
          this.importAUSFindingsNode,
          this.importLoanQualityFindingsNode
        });
      this.editConditionsNode.Nodes.AddRange(new TreeNode[5]
      {
        this.changePriorToNode,
        this.internalDescriptionNode,
        this.externalDescriptionNode,
        this.printInternallyNode,
        this.printExternallyNode
      });
      this.addCommentsNode.Nodes.AddRange(new TreeNode[2]
      {
        this.removeCommentsNode,
        this.markCommentsNode
      });
      this.assignDocumentsNode.Nodes.AddRange(new TreeNode[1]
      {
        this.unassignDocumentsNode
      });
      if (this.bIsInvestorDelivery)
      {
        this.importAllConditionsNode = new TreeNode("Import All Conditions");
        this.reviewAndImportConditionsNode = new TreeNode("Review and Import Conditions");
        this.deliveryConditionResponses = new TreeNode("Deliver Condition Responses");
        this.ViewConditionDeliveryStatus = new TreeNode("View Condition Delivery Status");
        this.addConditionsNode.Nodes.AddRange(new TreeNode[1]
        {
          this.createBlankConditionsNode
        });
        this.accessConditionsNode.Nodes.AddRange(new TreeNode[9]
        {
          this.addConditionsNode,
          this.importAllConditionsNode,
          this.reviewAndImportConditionsNode,
          this.editConditionsNode,
          this.addCommentsNode,
          this.assignDocumentsNode,
          this.deleteConditionsNode,
          this.deliveryConditionResponses,
          this.ViewConditionDeliveryStatus
        });
      }
      else
        this.accessConditionsNode.Nodes.AddRange(new TreeNode[5]
        {
          this.addConditionsNode,
          this.editConditionsNode,
          this.addCommentsNode,
          this.assignDocumentsNode,
          this.deleteConditionsNode
        });
      treeView.Nodes.AddRange(new TreeNode[1]
      {
        this.accessConditionsNode
      });
      treeView.ExpandAll();
      this.nodeToFeature = new Hashtable();
      this.nodeToFeature.Add((object) this.accessConditionsNode, (object) AclEnhancedConditionType.AccessConditions);
      this.nodeToFeature.Add((object) this.addConditionsNode, (object) AclEnhancedConditionType.AddConditions);
      if (flag && !this.bIsInvestorDelivery)
      {
        this.nodeToFeature.Add((object) this.createBlankConditionsNode, (object) AclEnhancedConditionType.CreateBlankCondition);
        this.nodeToFeature.Add((object) this.addAutomatedConditionsNode, (object) AclEnhancedConditionType.AddAutomatedConditions);
        this.nodeToFeature.Add((object) this.importAUSFindingsNode, (object) AclEnhancedConditionType.ImportAUSFindings);
        this.nodeToFeature.Add((object) this.importLoanQualityFindingsNode, (object) AclEnhancedConditionType.ImportLoanQualityFindings);
      }
      this.nodeToFeature.Add((object) this.editConditionsNode, (object) AclEnhancedConditionType.EditConditions);
      this.nodeToFeature.Add((object) this.changePriorToNode, (object) AclEnhancedConditionType.ChangePriorTo);
      this.nodeToFeature.Add((object) this.internalDescriptionNode, (object) AclEnhancedConditionType.InternalDescription);
      this.nodeToFeature.Add((object) this.externalDescriptionNode, (object) AclEnhancedConditionType.ExternalDescription);
      this.nodeToFeature.Add((object) this.printInternallyNode, (object) AclEnhancedConditionType.PrintInternally);
      this.nodeToFeature.Add((object) this.printExternallyNode, (object) AclEnhancedConditionType.PrintExternally);
      this.nodeToFeature.Add((object) this.addCommentsNode, (object) AclEnhancedConditionType.AddComments);
      this.nodeToFeature.Add((object) this.removeCommentsNode, (object) AclEnhancedConditionType.RemoveComments);
      this.nodeToFeature.Add((object) this.markCommentsNode, (object) AclEnhancedConditionType.MarkComments);
      this.nodeToFeature.Add((object) this.assignDocumentsNode, (object) AclEnhancedConditionType.AssignDocuments);
      this.nodeToFeature.Add((object) this.unassignDocumentsNode, (object) AclEnhancedConditionType.UnassignDocuments);
      this.nodeToFeature.Add((object) this.deleteConditionsNode, (object) AclEnhancedConditionType.DeleteConditions);
      this.featureToNodeTbl = new Hashtable();
      this.featureToNodeTbl.Add((object) AclEnhancedConditionType.AccessConditions, (object) this.accessConditionsNode);
      this.featureToNodeTbl.Add((object) AclEnhancedConditionType.AddConditions, (object) this.addConditionsNode);
      if (flag && !this.bIsInvestorDelivery)
      {
        this.featureToNodeTbl.Add((object) AclEnhancedConditionType.CreateBlankCondition, (object) this.createBlankConditionsNode);
        this.featureToNodeTbl.Add((object) AclEnhancedConditionType.AddAutomatedConditions, (object) this.addAutomatedConditionsNode);
        this.featureToNodeTbl.Add((object) AclEnhancedConditionType.ImportAUSFindings, (object) this.importAUSFindingsNode);
        this.featureToNodeTbl.Add((object) AclEnhancedConditionType.ImportLoanQualityFindings, (object) this.importLoanQualityFindingsNode);
      }
      this.featureToNodeTbl.Add((object) AclEnhancedConditionType.EditConditions, (object) this.editConditionsNode);
      this.featureToNodeTbl.Add((object) AclEnhancedConditionType.ChangePriorTo, (object) this.changePriorToNode);
      this.featureToNodeTbl.Add((object) AclEnhancedConditionType.InternalDescription, (object) this.internalDescriptionNode);
      this.featureToNodeTbl.Add((object) AclEnhancedConditionType.ExternalDescription, (object) this.externalDescriptionNode);
      this.featureToNodeTbl.Add((object) AclEnhancedConditionType.PrintInternally, (object) this.printInternallyNode);
      this.featureToNodeTbl.Add((object) AclEnhancedConditionType.PrintExternally, (object) this.printExternallyNode);
      this.featureToNodeTbl.Add((object) AclEnhancedConditionType.AddComments, (object) this.addCommentsNode);
      this.featureToNodeTbl.Add((object) AclEnhancedConditionType.RemoveComments, (object) this.removeCommentsNode);
      this.featureToNodeTbl.Add((object) AclEnhancedConditionType.MarkComments, (object) this.markCommentsNode);
      this.featureToNodeTbl.Add((object) AclEnhancedConditionType.AssignDocuments, (object) this.assignDocumentsNode);
      this.featureToNodeTbl.Add((object) AclEnhancedConditionType.UnassignDocuments, (object) this.unassignDocumentsNode);
      this.featureToNodeTbl.Add((object) AclEnhancedConditionType.DeleteConditions, (object) this.deleteConditionsNode);
      this.nodeToUpdateStatus = new Hashtable();
      this.nodeToUpdateStatus.Add((object) this.accessConditionsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.addConditionsNode, (object) false);
      if (flag && !this.bIsInvestorDelivery)
      {
        this.nodeToUpdateStatus.Add((object) this.createBlankConditionsNode, (object) false);
        this.nodeToUpdateStatus.Add((object) this.addAutomatedConditionsNode, (object) false);
        this.nodeToUpdateStatus.Add((object) this.importAUSFindingsNode, (object) false);
        this.nodeToUpdateStatus.Add((object) this.importLoanQualityFindingsNode, (object) false);
      }
      this.nodeToUpdateStatus.Add((object) this.editConditionsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.changePriorToNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.internalDescriptionNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.externalDescriptionNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.printInternallyNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.printExternallyNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.addCommentsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.removeCommentsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.markCommentsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.assignDocumentsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.unassignDocumentsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.deleteConditionsNode, (object) false);
      if (!this.bIsInvestorDelivery)
        return;
      this.nodeToFeature.Add((object) this.createBlankConditionsNode, (object) AclEnhancedConditionType.CreateBlankCondition);
      this.nodeToFeature.Add((object) this.reviewAndImportConditionsNode, (object) AclEnhancedConditionType.ReviewAndImportConditions);
      this.nodeToFeature.Add((object) this.importAllConditionsNode, (object) AclEnhancedConditionType.ImportAllConditions);
      this.nodeToFeature.Add((object) this.deliveryConditionResponses, (object) AclEnhancedConditionType.DeliveryConditionResponses);
      this.nodeToFeature.Add((object) this.ViewConditionDeliveryStatus, (object) AclEnhancedConditionType.ViewConditionDeliveryStatus);
      this.featureToNodeTbl.Add((object) AclEnhancedConditionType.CreateBlankCondition, (object) this.createBlankConditionsNode);
      this.featureToNodeTbl.Add((object) AclEnhancedConditionType.ReviewAndImportConditions, (object) this.reviewAndImportConditionsNode);
      this.featureToNodeTbl.Add((object) AclEnhancedConditionType.ImportAllConditions, (object) this.importAllConditionsNode);
      this.featureToNodeTbl.Add((object) AclEnhancedConditionType.DeliveryConditionResponses, (object) this.deliveryConditionResponses);
      this.featureToNodeTbl.Add((object) AclEnhancedConditionType.ViewConditionDeliveryStatus, (object) this.ViewConditionDeliveryStatus);
      this.nodeToUpdateStatus.Add((object) this.createBlankConditionsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.reviewAndImportConditionsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.importAllConditionsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.deliveryConditionResponses, (object) false);
      this.nodeToUpdateStatus.Add((object) this.ViewConditionDeliveryStatus, (object) false);
    }

    public AclEnhancedConditionType NodeToFeature(TreeNode node)
    {
      return (AclEnhancedConditionType) this.nodeToFeature[(object) node];
    }
  }
}
