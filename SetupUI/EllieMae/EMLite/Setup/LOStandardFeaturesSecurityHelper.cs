// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LOStandardFeaturesSecurityHelper
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
  public class LOStandardFeaturesSecurityHelper : FeatureSecurityHelperBase
  {
    protected TreeNode workflowTasksToolNode;
    protected TreeNode manageTasksNode;
    protected TreeNode viewAllTasksNode;
    protected TreeNode modifyAllTasksNode;
    protected TreeNode assignAllTasksNode;
    protected TreeNode changeDueDateNode;
    protected TreeNode changeDueDateMyTaskNode;
    protected TreeNode changeDueDateAllTaskNode;
    protected TreeNode updateTaskPriorityNode;
    protected TreeNode addCommentsForAllTasksNode;
    protected TreeNode deleteTasksNode;
    protected TreeNode deleteMyTasksNode;
    protected TreeNode deleteAllTasksNode;
    protected TreeNode addTasksNode;
    protected TreeNode addAllTasksNode;
    protected TreeNode addSubTasksNode;
    protected TreeNode addAdhocSubTasksNode;
    protected TreeNode addTaskPipelineNode;
    protected TreeNode addModifyQueueNode;
    protected TreeNode servicesNode;
    protected TreeNode allServicesNode;
    protected TreeNode underWritingCenterNode;
    protected TreeNode createManualEntryNode;
    protected TreeNode repWarrantTrackerNode;
    protected TreeNode makePrimaryNode;
    protected TreeNode miCenterNode;
    protected TreeNode importRatesNode;
    protected TreeNode orderDelegatedNode;
    protected TreeNode orderNonDelegatedNode;
    protected TreeNode orderContractUnnderwritingNode;
    protected TreeNode checkStatusNode;
    protected TreeNode resubmitOrderNode;
    protected TreeNode uploadDocumentsNode;
    protected TreeNode activateCoverageNode;
    protected TreeNode schedulerEventToolNode;
    protected TreeNode appraisalManagementTool;

    public LOStandardFeaturesSecurityHelper(
      Sessions.Session session,
      string userId,
      Persona[] personas)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new UserSecurityHelper(this.session, userId, personas, AclCategory.Features, FeatureSets.LOConnectStandardFeatures);
    }

    public LOStandardFeaturesSecurityHelper(Sessions.Session session, int personaId)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new PersonaSecurityHelper(this.session, personaId, AclCategory.Features, FeatureSets.LOConnectStandardFeatures);
    }

    public override void BuildNodes(TreeView treeView)
    {
      this.workflowTasksToolNode = new TreeNode("Workflow Tasks Tool");
      this.workflowTasksToolNode.Tag = (object) AclFeature.LOConnectTab_WorkflowTasksTool;
      this.manageTasksNode = new TreeNode("ManageTasks");
      this.viewAllTasksNode = new TreeNode("View All Task Groups/Tasks");
      this.modifyAllTasksNode = new TreeNode("Modify All Task Groups/Tasks");
      this.assignAllTasksNode = new TreeNode("Assign All Task Groups/Tasks");
      this.updateTaskPriorityNode = new TreeNode("Update Task Priority");
      this.addCommentsForAllTasksNode = new TreeNode("Add Comments for All Tasks");
      this.changeDueDateNode = new TreeNode("Change Due Date");
      this.changeDueDateMyTaskNode = new TreeNode("Change Due Date for My Tasks");
      this.changeDueDateAllTaskNode = new TreeNode("Change Due Date for All Tasks");
      this.deleteTasksNode = new TreeNode("Delete Tasks");
      this.deleteMyTasksNode = new TreeNode("Delete My Tasks");
      this.deleteAllTasksNode = new TreeNode("Delete All Tasks");
      this.deleteAllTasksNode.Tag = (object) AclFeature.LOConnectTab_DeleteAllTasks;
      this.addTasksNode = new TreeNode("Add Tasks manually");
      this.addAllTasksNode = new TreeNode("Add all Task Groups/Tasks");
      this.addSubTasksNode = new TreeNode("Add Sub-tasks");
      this.addAdhocSubTasksNode = new TreeNode("Add adhoc Sub-tasks");
      this.addTaskPipelineNode = new TreeNode("Task Pipeline");
      this.addModifyQueueNode = new TreeNode("Modify default Task Queue");
      this.servicesNode = new TreeNode("Services");
      this.allServicesNode = new TreeNode("All Services");
      this.allServicesNode.Tag = (object) AclFeature.LOConnectTab_AllServices;
      this.underWritingCenterNode = new TreeNode("Underwriting Center");
      this.underWritingCenterNode.Tag = (object) AclFeature.LOConnectTab_UnderwritingCenter;
      this.createManualEntryNode = new TreeNode("Create Manual Entry");
      this.repWarrantTrackerNode = new TreeNode("Rep & Warrant Tracker");
      this.makePrimaryNode = new TreeNode("Make Primary");
      this.miCenterNode = new TreeNode("MI Center");
      this.miCenterNode.Tag = (object) AclFeature.LOConnectTab_MiCenter;
      this.importRatesNode = new TreeNode("Import Rates");
      this.orderDelegatedNode = new TreeNode("Order Delegated MI");
      this.orderNonDelegatedNode = new TreeNode("Order Non Delegated MI");
      this.orderContractUnnderwritingNode = new TreeNode("Order Contract Underwriting with MI");
      this.checkStatusNode = new TreeNode("Check Status");
      this.resubmitOrderNode = new TreeNode("Resubmit Order");
      this.uploadDocumentsNode = new TreeNode("Upload Documents");
      this.activateCoverageNode = new TreeNode("Activate Coverage");
      this.schedulerEventToolNode = new TreeNode("Scheduler Event Tool");
      this.appraisalManagementTool = new TreeNode("Appraisal Management Tool");
      this.addTasksNode.Nodes.AddRange(new TreeNode[3]
      {
        this.addAllTasksNode,
        this.addSubTasksNode,
        this.addAdhocSubTasksNode
      });
      this.deleteTasksNode.Nodes.AddRange(new TreeNode[2]
      {
        this.deleteMyTasksNode,
        this.deleteAllTasksNode
      });
      this.modifyAllTasksNode.Nodes.AddRange(new TreeNode[1]
      {
        this.assignAllTasksNode
      });
      this.changeDueDateNode.Nodes.AddRange(new TreeNode[2]
      {
        this.changeDueDateMyTaskNode,
        this.changeDueDateAllTaskNode
      });
      this.addTaskPipelineNode.Nodes.AddRange(new TreeNode[1]
      {
        this.addModifyQueueNode
      });
      this.manageTasksNode.Nodes.AddRange(new TreeNode[7]
      {
        this.viewAllTasksNode,
        this.updateTaskPriorityNode,
        this.addCommentsForAllTasksNode,
        this.modifyAllTasksNode,
        this.changeDueDateNode,
        this.deleteTasksNode,
        this.addTasksNode
      });
      this.workflowTasksToolNode.Nodes.AddRange(new TreeNode[2]
      {
        this.manageTasksNode,
        this.addTaskPipelineNode
      });
      this.underWritingCenterNode.Nodes.AddRange(new TreeNode[3]
      {
        this.createManualEntryNode,
        this.repWarrantTrackerNode,
        this.makePrimaryNode
      });
      this.miCenterNode.Nodes.AddRange(new TreeNode[8]
      {
        this.importRatesNode,
        this.orderDelegatedNode,
        this.orderNonDelegatedNode,
        this.orderContractUnnderwritingNode,
        this.checkStatusNode,
        this.resubmitOrderNode,
        this.uploadDocumentsNode,
        this.activateCoverageNode
      });
      this.servicesNode.Nodes.AddRange(new TreeNode[3]
      {
        this.allServicesNode,
        this.underWritingCenterNode,
        this.miCenterNode
      });
      treeView.Nodes.AddRange(new TreeNode[4]
      {
        this.workflowTasksToolNode,
        this.servicesNode,
        this.schedulerEventToolNode,
        this.appraisalManagementTool
      });
      treeView.ExpandAll();
      this.nodeToFeature = new Hashtable();
      this.nodeToFeature.Add((object) this.workflowTasksToolNode, (object) AclFeature.LOConnectTab_WorkflowTasksTool);
      this.nodeToFeature.Add((object) this.manageTasksNode, (object) AclFeature.LOConnectTab_ManageTasks);
      this.nodeToFeature.Add((object) this.viewAllTasksNode, (object) AclFeature.LOConnectTab_ViewAllTasks);
      this.nodeToFeature.Add((object) this.modifyAllTasksNode, (object) AclFeature.LOConnectTab_ModifyAllTasks);
      this.nodeToFeature.Add((object) this.assignAllTasksNode, (object) AclFeature.LOConnectTab_AssignAllTasks);
      this.nodeToFeature.Add((object) this.changeDueDateNode, (object) AclFeature.LOConnectTab_ChangeDueDate);
      this.nodeToFeature.Add((object) this.changeDueDateMyTaskNode, (object) AclFeature.LOConnectTab_ChangeDueDateForMyTasks);
      this.nodeToFeature.Add((object) this.changeDueDateAllTaskNode, (object) AclFeature.LOConnectTab_ChangeDueDateForAllTasks);
      this.nodeToFeature.Add((object) this.updateTaskPriorityNode, (object) AclFeature.LOConnectTab_UpdateTaskPriority);
      this.nodeToFeature.Add((object) this.addCommentsForAllTasksNode, (object) AclFeature.LOConnectTab_AddCommentsForAllTasks);
      this.nodeToFeature.Add((object) this.deleteTasksNode, (object) AclFeature.LOConnectTab_DeleteTasks);
      this.nodeToFeature.Add((object) this.deleteMyTasksNode, (object) AclFeature.LOConnectTab_DeleteMyTasks);
      this.nodeToFeature.Add((object) this.deleteAllTasksNode, (object) AclFeature.LOConnectTab_DeleteAllTasks);
      this.nodeToFeature.Add((object) this.addTasksNode, (object) AclFeature.LOConnectTab_AddTasks);
      this.nodeToFeature.Add((object) this.addAllTasksNode, (object) AclFeature.LOConnectTab_AddAllTasks);
      this.nodeToFeature.Add((object) this.addSubTasksNode, (object) AclFeature.LOConnectTab_AddSubTasks);
      this.nodeToFeature.Add((object) this.addAdhocSubTasksNode, (object) AclFeature.LOConnectTab_AddAdhocSubTasks);
      this.nodeToFeature.Add((object) this.addTaskPipelineNode, (object) AclFeature.LOConnectTab_TaskPipeline);
      this.nodeToFeature.Add((object) this.addModifyQueueNode, (object) AclFeature.LOConnectTab_ModifyQueue);
      this.nodeToFeature.Add((object) this.servicesNode, (object) AclFeature.LOConnectTab_Services);
      this.nodeToFeature.Add((object) this.allServicesNode, (object) AclFeature.LOConnectTab_AllServices);
      this.nodeToFeature.Add((object) this.underWritingCenterNode, (object) AclFeature.LOConnectTab_UnderwritingCenter);
      this.nodeToFeature.Add((object) this.createManualEntryNode, (object) AclFeature.LOConnectTab_CreateManualEntry);
      this.nodeToFeature.Add((object) this.repWarrantTrackerNode, (object) AclFeature.LOConnectTab_RepWarrantTracker);
      this.nodeToFeature.Add((object) this.makePrimaryNode, (object) AclFeature.LOConnectTab_MakePrimary);
      this.nodeToFeature.Add((object) this.schedulerEventToolNode, (object) AclFeature.LOConnectTab_SchedulerEventTool);
      this.nodeToFeature.Add((object) this.miCenterNode, (object) AclFeature.LOConnectTab_MiCenter);
      this.nodeToFeature.Add((object) this.importRatesNode, (object) AclFeature.LOConnectTab_ImportRates);
      this.nodeToFeature.Add((object) this.orderDelegatedNode, (object) AclFeature.LOConnectTab_OrderDelegated);
      this.nodeToFeature.Add((object) this.orderNonDelegatedNode, (object) AclFeature.LOConnectTab_OrderNonDelegated);
      this.nodeToFeature.Add((object) this.orderContractUnnderwritingNode, (object) AclFeature.LOConnectTab_OrderContractUnderwriting);
      this.nodeToFeature.Add((object) this.checkStatusNode, (object) AclFeature.LOConnectTab_CheckStatus);
      this.nodeToFeature.Add((object) this.resubmitOrderNode, (object) AclFeature.LOConnectTab_ResubmitOrder);
      this.nodeToFeature.Add((object) this.uploadDocumentsNode, (object) AclFeature.LOConnectTab_UploadDocuments);
      this.nodeToFeature.Add((object) this.activateCoverageNode, (object) AclFeature.LOConnectTab_ActivateCoverage);
      this.nodeToFeature.Add((object) this.appraisalManagementTool, (object) AclFeature.LOConnectTab_AppraisalManagementTool);
      this.featureToNodeTbl = new Hashtable(FeatureSets.LOConnectStandardFeatures.Length);
      this.featureToNodeTbl.Add((object) AclFeature.LOConnectTab_WorkflowTasksTool, (object) this.workflowTasksToolNode);
      this.featureToNodeTbl.Add((object) AclFeature.LOConnectTab_ManageTasks, (object) this.manageTasksNode);
      this.featureToNodeTbl.Add((object) AclFeature.LOConnectTab_ViewAllTasks, (object) this.viewAllTasksNode);
      this.featureToNodeTbl.Add((object) AclFeature.LOConnectTab_ModifyAllTasks, (object) this.modifyAllTasksNode);
      this.featureToNodeTbl.Add((object) AclFeature.LOConnectTab_AssignAllTasks, (object) this.assignAllTasksNode);
      this.featureToNodeTbl.Add((object) AclFeature.LOConnectTab_ChangeDueDate, (object) this.changeDueDateNode);
      this.featureToNodeTbl.Add((object) AclFeature.LOConnectTab_ChangeDueDateForMyTasks, (object) this.changeDueDateMyTaskNode);
      this.featureToNodeTbl.Add((object) AclFeature.LOConnectTab_ChangeDueDateForAllTasks, (object) this.changeDueDateAllTaskNode);
      this.featureToNodeTbl.Add((object) AclFeature.LOConnectTab_UpdateTaskPriority, (object) this.updateTaskPriorityNode);
      this.featureToNodeTbl.Add((object) AclFeature.LOConnectTab_AddCommentsForAllTasks, (object) this.addCommentsForAllTasksNode);
      this.featureToNodeTbl.Add((object) AclFeature.LOConnectTab_DeleteTasks, (object) this.deleteTasksNode);
      this.featureToNodeTbl.Add((object) AclFeature.LOConnectTab_DeleteMyTasks, (object) this.deleteMyTasksNode);
      this.featureToNodeTbl.Add((object) AclFeature.LOConnectTab_DeleteAllTasks, (object) this.deleteAllTasksNode);
      this.featureToNodeTbl.Add((object) AclFeature.LOConnectTab_AddTasks, (object) this.addTasksNode);
      this.featureToNodeTbl.Add((object) AclFeature.LOConnectTab_AddAllTasks, (object) this.addAllTasksNode);
      this.featureToNodeTbl.Add((object) AclFeature.LOConnectTab_AddSubTasks, (object) this.addSubTasksNode);
      this.featureToNodeTbl.Add((object) AclFeature.LOConnectTab_AddAdhocSubTasks, (object) this.addAdhocSubTasksNode);
      this.featureToNodeTbl.Add((object) AclFeature.LOConnectTab_TaskPipeline, (object) this.addTaskPipelineNode);
      this.featureToNodeTbl.Add((object) AclFeature.LOConnectTab_ModifyQueue, (object) this.addModifyQueueNode);
      this.featureToNodeTbl.Add((object) AclFeature.LOConnectTab_Services, (object) this.servicesNode);
      this.featureToNodeTbl.Add((object) AclFeature.LOConnectTab_AllServices, (object) this.allServicesNode);
      this.featureToNodeTbl.Add((object) AclFeature.LOConnectTab_UnderwritingCenter, (object) this.underWritingCenterNode);
      this.featureToNodeTbl.Add((object) AclFeature.LOConnectTab_CreateManualEntry, (object) this.createManualEntryNode);
      this.featureToNodeTbl.Add((object) AclFeature.LOConnectTab_RepWarrantTracker, (object) this.repWarrantTrackerNode);
      this.featureToNodeTbl.Add((object) AclFeature.LOConnectTab_MakePrimary, (object) this.makePrimaryNode);
      this.featureToNodeTbl.Add((object) AclFeature.LOConnectTab_SchedulerEventTool, (object) this.schedulerEventToolNode);
      this.featureToNodeTbl.Add((object) AclFeature.LOConnectTab_MiCenter, (object) this.miCenterNode);
      this.featureToNodeTbl.Add((object) AclFeature.LOConnectTab_ImportRates, (object) this.importRatesNode);
      this.featureToNodeTbl.Add((object) AclFeature.LOConnectTab_OrderDelegated, (object) this.orderDelegatedNode);
      this.featureToNodeTbl.Add((object) AclFeature.LOConnectTab_OrderNonDelegated, (object) this.orderNonDelegatedNode);
      this.featureToNodeTbl.Add((object) AclFeature.LOConnectTab_OrderContractUnderwriting, (object) this.orderContractUnnderwritingNode);
      this.featureToNodeTbl.Add((object) AclFeature.LOConnectTab_CheckStatus, (object) this.checkStatusNode);
      this.featureToNodeTbl.Add((object) AclFeature.LOConnectTab_ResubmitOrder, (object) this.resubmitOrderNode);
      this.featureToNodeTbl.Add((object) AclFeature.LOConnectTab_UploadDocuments, (object) this.uploadDocumentsNode);
      this.featureToNodeTbl.Add((object) AclFeature.LOConnectTab_ActivateCoverage, (object) this.activateCoverageNode);
      this.featureToNodeTbl.Add((object) AclFeature.LOConnectTab_AppraisalManagementTool, (object) this.appraisalManagementTool);
      this.nodeToUpdateStatus = new Hashtable()
      {
        {
          (object) this.workflowTasksToolNode,
          (object) false
        },
        {
          (object) this.manageTasksNode,
          (object) false
        },
        {
          (object) this.viewAllTasksNode,
          (object) false
        },
        {
          (object) this.modifyAllTasksNode,
          (object) false
        },
        {
          (object) this.assignAllTasksNode,
          (object) false
        },
        {
          (object) this.changeDueDateNode,
          (object) false
        },
        {
          (object) this.changeDueDateMyTaskNode,
          (object) false
        },
        {
          (object) this.changeDueDateAllTaskNode,
          (object) false
        },
        {
          (object) this.updateTaskPriorityNode,
          (object) false
        },
        {
          (object) this.addCommentsForAllTasksNode,
          (object) false
        },
        {
          (object) this.deleteTasksNode,
          (object) false
        },
        {
          (object) this.deleteMyTasksNode,
          (object) false
        },
        {
          (object) this.deleteAllTasksNode,
          (object) false
        },
        {
          (object) this.addTasksNode,
          (object) false
        },
        {
          (object) this.addAllTasksNode,
          (object) false
        },
        {
          (object) this.addSubTasksNode,
          (object) false
        },
        {
          (object) this.addAdhocSubTasksNode,
          (object) false
        },
        {
          (object) this.addTaskPipelineNode,
          (object) false
        },
        {
          (object) this.addModifyQueueNode,
          (object) false
        },
        {
          (object) this.servicesNode,
          (object) false
        },
        {
          (object) this.allServicesNode,
          (object) false
        },
        {
          (object) this.underWritingCenterNode,
          (object) false
        },
        {
          (object) this.createManualEntryNode,
          (object) false
        },
        {
          (object) this.repWarrantTrackerNode,
          (object) false
        },
        {
          (object) this.makePrimaryNode,
          (object) false
        },
        {
          (object) this.schedulerEventToolNode,
          (object) false
        },
        {
          (object) this.miCenterNode,
          (object) false
        },
        {
          (object) this.importRatesNode,
          (object) false
        },
        {
          (object) this.orderDelegatedNode,
          (object) false
        },
        {
          (object) this.orderNonDelegatedNode,
          (object) false
        },
        {
          (object) this.orderContractUnnderwritingNode,
          (object) false
        },
        {
          (object) this.checkStatusNode,
          (object) false
        },
        {
          (object) this.resubmitOrderNode,
          (object) false
        },
        {
          (object) this.uploadDocumentsNode,
          (object) false
        },
        {
          (object) this.activateCoverageNode,
          (object) false
        },
        {
          (object) this.appraisalManagementTool,
          (object) false
        }
      };
      this.securityHelper.SetTables(this.featureToNodeTbl, this.nodeToFeature, this.dependentNodes, this.nodeToUpdateStatus);
    }
  }
}
