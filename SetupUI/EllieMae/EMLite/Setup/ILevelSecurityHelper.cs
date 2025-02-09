// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ILevelSecurityHelper
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using System.Collections;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public interface ILevelSecurityHelper
  {
    void SetPermission(AclFeature feature, bool access);

    void SetPermission(AclFeature feature, AclTriState accessState);

    Hashtable GetPersonaPermissions();

    void SetNodeStates();

    void SetNodeImageIndex(TreeNode node, int index);

    void SetTables(
      Hashtable featureToNodeTbl,
      Hashtable nodeToFeatureTbl,
      ArrayList dependentNodes);

    void SetTables(
      Hashtable featureToNodeTbl,
      Hashtable nodeToFeatureTbl,
      ArrayList dependentNodes,
      Hashtable nodeToUpdateStatus);

    void SetPersonaId(int personaId);

    void setSpecialDepTreeNodes(Hashtable specialDepTreeNodes);

    Hashtable getSpecialDepTreeNodes();

    void setSpecialDepTreeNodesImg(Hashtable specialDepTreeNodes);

    Hashtable getSpecialDepTreeNodesImg();
  }
}
