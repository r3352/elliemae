// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AclGroupResourceSetHelper
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AclGroupResourceSetHelper : IResourceSetHelper
  {
    private Sessions.Session session;

    public AclGroupResourceSetHelper(Sessions.Session session) => this.session = session;

    public void saveResourceSet(
      object resourceSet,
      TreeNodeCollection nodes,
      Dictionary<string, object> updateList)
    {
      this.session.AclGroupManager.UpdateMembersInGroup(((AclGroup) resourceSet).ID, (string[]) updateList["resetUsers"], (int[]) updateList["resetOrgs"], (string[]) updateList["addUsers"], (int[]) updateList["addOrgs"], (int[]) updateList["addInclusiveOrgs"]);
    }

    private void saveCheckedNodeToGroup(TreeNode node, ArrayList orgIds, ArrayList userIds)
    {
      if (node.ImageIndex == 0)
        orgIds.Add((object) new OrgInGroup(((OrgInfo) node.Tag).Oid, true, ((OrgInfo) node.Tag).OrgName));
      else if (node.ImageIndex == 1)
        orgIds.Add((object) new OrgInGroup(((OrgInfo) node.Tag).Oid, false, ((OrgInfo) node.Tag).OrgName));
      else if (node.ImageIndex == 2)
        userIds.Add((object) ((UserInfo) node.Tag).Userid);
      foreach (TreeNode node1 in node.Nodes)
        this.saveCheckedNodeToGroup(node1, orgIds, userIds);
    }
  }
}
