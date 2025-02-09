// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.BizContactsSecurityHelper
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
  public class BizContactsSecurityHelper : FeatureSecurityHelperBase
  {
    protected TreeNode accessToBizCntNode;
    protected TreeNode newBizCntNode;
    protected TreeNode copyBizCntNode;
    protected TreeNode deleteBizCntNode;
    protected TreeNode mailMergeBizCntNode;
    protected TreeNode importBizCntNode;
    protected TreeNode exportBizCntNode;
    protected TreeNode printBizCntNode;

    public BizContactsSecurityHelper(Sessions.Session session, string userId, Persona[] personas)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new UserSecurityHelper(this.session, userId, personas, AclCategory.Features, FeatureSets.BizContacts);
    }

    public BizContactsSecurityHelper(Sessions.Session session, int personaId)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new PersonaSecurityHelper(this.session, personaId, AclCategory.Features, FeatureSets.BizContacts);
    }

    public override void BuildNodes(TreeView treeView)
    {
      this.newBizCntNode = new TreeNode("Create New Contacts");
      this.copyBizCntNode = new TreeNode("Duplicate Contacts");
      this.deleteBizCntNode = new TreeNode("Delete Contacts");
      this.mailMergeBizCntNode = new TreeNode("Mail Merge");
      this.importBizCntNode = new TreeNode("Import Contacts");
      this.exportBizCntNode = new TreeNode("Export Contacts");
      this.printBizCntNode = new TreeNode("Print");
      this.accessToBizCntNode = new TreeNode("Access to Business Contacts");
      this.accessToBizCntNode.Nodes.AddRange(new TreeNode[7]
      {
        this.newBizCntNode,
        this.copyBizCntNode,
        this.deleteBizCntNode,
        this.mailMergeBizCntNode,
        this.importBizCntNode,
        this.exportBizCntNode,
        this.printBizCntNode
      });
      treeView.Nodes.AddRange(new TreeNode[1]
      {
        this.accessToBizCntNode
      });
      treeView.ExpandAll();
      this.nodeToFeature = new Hashtable();
      this.nodeToFeature.Add((object) this.accessToBizCntNode, (object) AclFeature.Cnt_Biz_Access);
      this.nodeToFeature.Add((object) this.newBizCntNode, (object) AclFeature.Cnt_Biz_CreateNew);
      this.nodeToFeature.Add((object) this.copyBizCntNode, (object) AclFeature.Cnt_Biz_Copy);
      this.nodeToFeature.Add((object) this.deleteBizCntNode, (object) AclFeature.Cnt_Biz_Delete);
      this.nodeToFeature.Add((object) this.mailMergeBizCntNode, (object) AclFeature.Cnt_Biz_MailMerge);
      this.nodeToFeature.Add((object) this.importBizCntNode, (object) AclFeature.Cnt_Biz_Import);
      this.nodeToFeature.Add((object) this.exportBizCntNode, (object) AclFeature.Cnt_Biz_Export);
      this.nodeToFeature.Add((object) this.printBizCntNode, (object) AclFeature.Cnt_Biz_Print);
      this.featureToNodeTbl = new Hashtable(FeatureSets.Contacts.Length);
      this.featureToNodeTbl.Add((object) AclFeature.Cnt_Biz_Access, (object) this.accessToBizCntNode);
      this.featureToNodeTbl.Add((object) AclFeature.Cnt_Biz_CreateNew, (object) this.newBizCntNode);
      this.featureToNodeTbl.Add((object) AclFeature.Cnt_Biz_Copy, (object) this.copyBizCntNode);
      this.featureToNodeTbl.Add((object) AclFeature.Cnt_Biz_Delete, (object) this.deleteBizCntNode);
      this.featureToNodeTbl.Add((object) AclFeature.Cnt_Biz_MailMerge, (object) this.mailMergeBizCntNode);
      this.featureToNodeTbl.Add((object) AclFeature.Cnt_Biz_Import, (object) this.importBizCntNode);
      this.featureToNodeTbl.Add((object) AclFeature.Cnt_Biz_Export, (object) this.exportBizCntNode);
      this.featureToNodeTbl.Add((object) AclFeature.Cnt_Biz_Print, (object) this.printBizCntNode);
      this.nodeToUpdateStatus = new Hashtable();
      this.nodeToUpdateStatus.Add((object) this.accessToBizCntNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.newBizCntNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.copyBizCntNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.deleteBizCntNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.mailMergeBizCntNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.importBizCntNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.exportBizCntNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.printBizCntNode, (object) false);
      this.securityHelper.SetTables(this.featureToNodeTbl, this.nodeToFeature, this.dependentNodes, this.nodeToUpdateStatus);
    }
  }
}
