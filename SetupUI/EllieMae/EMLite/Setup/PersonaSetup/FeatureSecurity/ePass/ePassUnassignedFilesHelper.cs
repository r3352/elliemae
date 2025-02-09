// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PersonaSetup.FeatureSecurity.ePass.ePassUnassignedFilesHelper
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
  public class ePassUnassignedFilesHelper : FeatureSecurityHelperBase
  {
    private TreeNode browseAndAttachNode;
    private TreeNode scanAndAttachNode;
    private TreeNode attachEncompassFormsNode;
    private TreeNode editFileNode;
    private TreeNode deletePagePermanentlyNode;
    private TreeNode mergeFilesNode;
    private TreeNode splitFileUpNode;
    private TreeNode addNotesToFileNode;
    private TreeNode deleteNotesNode;
    private TreeNode deleteFilePermanentlyNode;
    private TreeNode autoAssignNode;
    private TreeNode suggestorNode;

    public ePassUnassignedFilesHelper(Sessions.Session session, string userId, Persona[] personas)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new UserSecurityHelper(this.session, userId, personas, AclCategory.Features, FeatureSets.LoanEFolderFeatures);
    }

    public ePassUnassignedFilesHelper(Sessions.Session session, int personaId)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new PersonaSecurityHelper(this.session, personaId, AclCategory.Features, FeatureSets.LoanEFolderFeatures);
    }

    public override void BuildNodes(TreeView treeView)
    {
      this.browseAndAttachNode = new TreeNode("Browse and Attach");
      this.scanAndAttachNode = new TreeNode("Scan and Attach");
      this.attachEncompassFormsNode = new TreeNode("Attach Encompass Forms");
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
      this.deleteFilePermanentlyNode = new TreeNode("Delete File Permanently");
      this.autoAssignNode = new TreeNode("Auto Assign");
      this.suggestorNode = new TreeNode("Suggestor");
      treeView.Nodes.AddRange(new TreeNode[9]
      {
        this.browseAndAttachNode,
        this.scanAndAttachNode,
        this.attachEncompassFormsNode,
        this.editFileNode,
        this.mergeFilesNode,
        this.splitFileUpNode,
        this.addNotesToFileNode,
        this.deleteFilePermanentlyNode,
        this.autoAssignNode
      });
      treeView.Nodes.AddRange(new TreeNode[1]
      {
        this.suggestorNode
      });
      this.nodeToFeature = new Hashtable();
      this.nodeToFeature.Add((object) this.browseAndAttachNode, (object) AclFeature.eFolder_UF_BrowseAndAttach);
      this.nodeToFeature.Add((object) this.scanAndAttachNode, (object) AclFeature.eFolder_UF_ScanAndAttach);
      this.nodeToFeature.Add((object) this.attachEncompassFormsNode, (object) AclFeature.eFolder_UF_AttachEncompassForms);
      this.nodeToFeature.Add((object) this.editFileNode, (object) AclFeature.eFolder_UF_EditFile);
      this.nodeToFeature.Add((object) this.deletePagePermanentlyNode, (object) AclFeature.eFolder_UF_EF_DeletePagePermanently);
      this.nodeToFeature.Add((object) this.mergeFilesNode, (object) AclFeature.eFolder_UF_MergeFiles);
      this.nodeToFeature.Add((object) this.splitFileUpNode, (object) AclFeature.eFolder_UF_SplitFileUp);
      this.nodeToFeature.Add((object) this.deleteNotesNode, (object) AclFeature.eFolder_UF_AN_DeleteNotes);
      this.nodeToFeature.Add((object) this.addNotesToFileNode, (object) AclFeature.eFolder_UF_AddNotesToFile);
      this.nodeToFeature.Add((object) this.deleteFilePermanentlyNode, (object) AclFeature.eFolder_UF_DeleteFilePermanently);
      this.nodeToFeature.Add((object) this.autoAssignNode, (object) AclFeature.eFolder_UF_AutoAssign);
      this.nodeToFeature.Add((object) this.suggestorNode, (object) AclFeature.eFolder_UF_Suggestor);
      this.featureToNodeTbl = new Hashtable(FeatureSets.Features.Length);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_UF_BrowseAndAttach, (object) this.browseAndAttachNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_UF_ScanAndAttach, (object) this.scanAndAttachNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_UF_AttachEncompassForms, (object) this.attachEncompassFormsNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_UF_EditFile, (object) this.editFileNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_UF_EF_DeletePagePermanently, (object) this.deletePagePermanentlyNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_UF_MergeFiles, (object) this.mergeFilesNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_UF_SplitFileUp, (object) this.splitFileUpNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_UF_AN_DeleteNotes, (object) this.deleteNotesNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_UF_AddNotesToFile, (object) this.addNotesToFileNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_UF_DeleteFilePermanently, (object) this.deleteFilePermanentlyNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_UF_AutoAssign, (object) this.autoAssignNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_UF_Suggestor, (object) this.suggestorNode);
      this.nodeToUpdateStatus = new Hashtable();
      this.nodeToUpdateStatus.Add((object) this.browseAndAttachNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.scanAndAttachNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.attachEncompassFormsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.editFileNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.deletePagePermanentlyNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.mergeFilesNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.splitFileUpNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.deleteNotesNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.addNotesToFileNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.deleteFilePermanentlyNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.autoAssignNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.suggestorNode, (object) false);
      this.securityHelper.SetTables(this.featureToNodeTbl, this.nodeToFeature, this.dependentNodes, this.nodeToUpdateStatus);
      treeView.ExpandAll();
    }
  }
}
