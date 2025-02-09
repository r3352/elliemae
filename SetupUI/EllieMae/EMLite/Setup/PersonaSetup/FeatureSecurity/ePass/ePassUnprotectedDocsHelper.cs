// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PersonaSetup.FeatureSecurity.ePass.ePassUnprotectedDocsHelper
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System.Collections;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.PersonaSetup.FeatureSecurity.ePass
{
  public class ePassUnprotectedDocsHelper : FeatureSecurityHelperBase
  {
    private TreeNode editDocNode;
    private TreeNode editDocDetailNode;
    private TreeNode createNewDocNameNode;
    private TreeNode addCommentNode;
    private TreeNode deleteCommentNode;
    private TreeNode browseAndAttachNode;
    private TreeNode scanAndAttachNode;
    private TreeNode attachEncompassFormsNode;
    private TreeNode attachUnassignedFilesNode;
    private TreeNode moveFileUpDownNode;
    private TreeNode editFileNode;
    private TreeNode deletePagePermanentlyNode;
    private TreeNode mergeFilesNode;
    private TreeNode splitFileUpNode;
    private TreeNode addNotesToFileNode;
    private TreeNode deleteNotesNode;
    private TreeNode removeFileFromDocNode;
    private TreeNode markFileAsCurrentVersionNode;
    private TreeNode deleteDocsNode;
    private TreeNode markStatusASReviewedNode;

    public ePassUnprotectedDocsHelper(Sessions.Session session, string userId, Persona[] personas)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new UserSecurityHelper(this.session, userId, personas, AclCategory.Features, FeatureSets.LoanEFolderFeatures);
    }

    public ePassUnprotectedDocsHelper(Sessions.Session session, int personaId)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new PersonaSecurityHelper(this.session, personaId, AclCategory.Features, FeatureSets.LoanEFolderFeatures);
    }

    public override void BuildNodes(TreeView treeView)
    {
      this.editDocNode = new TreeNode("Edit Document");
      this.editDocDetailNode = new TreeNode("Edit Document Details");
      this.createNewDocNameNode = new TreeNode("Create New Document Name");
      this.editDocDetailNode.Nodes.AddRange(new TreeNode[1]
      {
        this.createNewDocNameNode
      });
      this.addCommentNode = new TreeNode("Add Comment");
      this.deleteCommentNode = new TreeNode("Delete Comment");
      this.browseAndAttachNode = new TreeNode("Browse and Attach");
      this.scanAndAttachNode = new TreeNode("Scan and Attach");
      this.attachEncompassFormsNode = new TreeNode("Attach Encompass Forms");
      this.attachUnassignedFilesNode = new TreeNode("Attach Unassigned Files");
      this.moveFileUpDownNode = new TreeNode("Move File Up/Down");
      this.editFileNode = new TreeNode("Edit File");
      this.deletePagePermanentlyNode = new TreeNode("Delete Page Permanently");
      this.editFileNode.Nodes.AddRange(new TreeNode[1]
      {
        this.deletePagePermanentlyNode
      });
      this.mergeFilesNode = new TreeNode("Merge Files");
      this.splitFileUpNode = new TreeNode("Split File");
      this.addNotesToFileNode = new TreeNode("Add Notes to File");
      this.deleteNotesNode = new TreeNode("Delete Notes");
      this.addNotesToFileNode.Nodes.AddRange(new TreeNode[1]
      {
        this.deleteNotesNode
      });
      this.removeFileFromDocNode = new TreeNode("Remove File from Document");
      this.markFileAsCurrentVersionNode = new TreeNode("Mark File As Current Version");
      this.markStatusASReviewedNode = new TreeNode("Mark Status As Reviewed");
      this.editDocNode.Nodes.AddRange(new TreeNode[15]
      {
        this.editDocDetailNode,
        this.addCommentNode,
        this.deleteCommentNode,
        this.browseAndAttachNode,
        this.scanAndAttachNode,
        this.attachEncompassFormsNode,
        this.attachUnassignedFilesNode,
        this.moveFileUpDownNode,
        this.editFileNode,
        this.mergeFilesNode,
        this.splitFileUpNode,
        this.addNotesToFileNode,
        this.removeFileFromDocNode,
        this.markFileAsCurrentVersionNode,
        this.markStatusASReviewedNode
      });
      this.deleteDocsNode = new TreeNode("Delete Document");
      treeView.Nodes.AddRange(new TreeNode[2]
      {
        this.editDocNode,
        this.deleteDocsNode
      });
      this.nodeToFeature = new Hashtable();
      this.nodeToFeature.Add((object) this.editDocNode, (object) AclFeature.eFolder_UD_EditDoc);
      this.nodeToFeature.Add((object) this.editDocDetailNode, (object) AclFeature.eFolder_UD_ED_EditDocDetails);
      this.nodeToFeature.Add((object) this.createNewDocNameNode, (object) AclFeature.eFolder_UD_ED_EDD_CreateNewDocName);
      this.nodeToFeature.Add((object) this.addCommentNode, (object) AclFeature.eFolder_UD_ED_AddComment);
      this.nodeToFeature.Add((object) this.deleteCommentNode, (object) AclFeature.eFolder_UD_ED_DeleteComment);
      this.nodeToFeature.Add((object) this.browseAndAttachNode, (object) AclFeature.eFolder_UD_ED_BrowseAndAttach);
      this.nodeToFeature.Add((object) this.scanAndAttachNode, (object) AclFeature.eFolder_UD_ED_ScanAndAttach);
      this.nodeToFeature.Add((object) this.attachEncompassFormsNode, (object) AclFeature.eFolder_UD_ED_AttachEncompassForms);
      this.nodeToFeature.Add((object) this.attachUnassignedFilesNode, (object) AclFeature.eFolder_UD_ED_AttachUnassignedFiles);
      this.nodeToFeature.Add((object) this.moveFileUpDownNode, (object) AclFeature.eFolder_UD_ED_MoveFileUpDown);
      this.nodeToFeature.Add((object) this.editFileNode, (object) AclFeature.eFolder_UD_ED_EditFile);
      this.nodeToFeature.Add((object) this.deletePagePermanentlyNode, (object) AclFeature.eFolder_UD_ED_EF_DeletePagePermanently);
      this.nodeToFeature.Add((object) this.mergeFilesNode, (object) AclFeature.eFolder_UD_ED_MergeFiles);
      this.nodeToFeature.Add((object) this.splitFileUpNode, (object) AclFeature.eFolder_UD_ED_SplitFileUp);
      this.nodeToFeature.Add((object) this.deleteNotesNode, (object) AclFeature.eFolder_UD_ED_AN_DeleteNotes);
      this.nodeToFeature.Add((object) this.addNotesToFileNode, (object) AclFeature.eFolder_UD_ED_AddNotesToFile);
      this.nodeToFeature.Add((object) this.removeFileFromDocNode, (object) AclFeature.eFolder_UD_ED_RemoveFileFromDoc);
      this.nodeToFeature.Add((object) this.markFileAsCurrentVersionNode, (object) AclFeature.eFolder_UD_ED_MarkFileAsCurrentVersion);
      this.nodeToFeature.Add((object) this.markStatusASReviewedNode, (object) AclFeature.eFolder_UD_ED_MarkStatusASReviewed);
      this.nodeToFeature.Add((object) this.deleteDocsNode, (object) AclFeature.eFolder_UD_DeleteDoc);
      this.featureToNodeTbl = new Hashtable(FeatureSets.Features.Length);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_UD_EditDoc, (object) this.editDocNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_UD_ED_EditDocDetails, (object) this.editDocDetailNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_UD_ED_EDD_CreateNewDocName, (object) this.createNewDocNameNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_UD_ED_AddComment, (object) this.addCommentNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_UD_ED_DeleteComment, (object) this.deleteCommentNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_UD_ED_BrowseAndAttach, (object) this.browseAndAttachNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_UD_ED_ScanAndAttach, (object) this.scanAndAttachNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_UD_ED_AttachEncompassForms, (object) this.attachEncompassFormsNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_UD_ED_AttachUnassignedFiles, (object) this.attachUnassignedFilesNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_UD_ED_MoveFileUpDown, (object) this.moveFileUpDownNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_UD_ED_EditFile, (object) this.editFileNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_UD_ED_EF_DeletePagePermanently, (object) this.deletePagePermanentlyNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_UD_ED_MergeFiles, (object) this.mergeFilesNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_UD_ED_SplitFileUp, (object) this.splitFileUpNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_UD_ED_AN_DeleteNotes, (object) this.deleteNotesNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_UD_ED_AddNotesToFile, (object) this.addNotesToFileNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_UD_ED_RemoveFileFromDoc, (object) this.removeFileFromDocNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_UD_ED_MarkFileAsCurrentVersion, (object) this.markFileAsCurrentVersionNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_UD_ED_MarkStatusASReviewed, (object) this.markStatusASReviewedNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_UD_DeleteDoc, (object) this.deleteDocsNode);
      this.nodeToUpdateStatus = new Hashtable();
      this.nodeToUpdateStatus.Add((object) this.editDocNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.editDocDetailNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.createNewDocNameNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.addCommentNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.deleteCommentNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.browseAndAttachNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.scanAndAttachNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.attachEncompassFormsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.attachUnassignedFilesNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.moveFileUpDownNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.editFileNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.deletePagePermanentlyNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.mergeFilesNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.splitFileUpNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.deleteNotesNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.addNotesToFileNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.removeFileFromDocNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.markFileAsCurrentVersionNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.deleteDocsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.markStatusASReviewedNode, (object) false);
      this.securityHelper.SetTables(this.featureToNodeTbl, this.nodeToFeature, this.dependentNodes, this.nodeToUpdateStatus);
      treeView.ExpandAll();
    }
  }
}
