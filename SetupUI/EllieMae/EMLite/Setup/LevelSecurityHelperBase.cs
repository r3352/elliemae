// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LevelSecurityHelperBase
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using System;
using System.Collections;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public abstract class LevelSecurityHelperBase : ILevelSecurityHelper
  {
    protected Hashtable featureToNodeTbl;
    protected Hashtable nodeToFeature;
    protected Hashtable nodeToUpdateStatus;
    private Hashtable specialDepTreeNodes;
    private Hashtable specialDepTreeNodesImg;
    protected ArrayList dependentNodes;

    public abstract void SetPermission(AclFeature feature, bool access);

    public abstract void SetPermission(AclFeature feature, AclTriState accessState);

    public abstract Hashtable GetPersonaPermissions();

    public abstract void SetNodeImageIndex(TreeNode node, int index);

    public abstract void SetNodeStates();

    public virtual void SetPersonaId(int personaId) => throw new NotImplementedException();

    public virtual void setSpecialDepTreeNodes(Hashtable data) => this.specialDepTreeNodes = data;

    public virtual Hashtable getSpecialDepTreeNodes()
    {
      return this.specialDepTreeNodes == null ? new Hashtable() : this.specialDepTreeNodes;
    }

    public virtual void setSpecialDepTreeNodesImg(Hashtable specialDepTreeNodes)
    {
      this.specialDepTreeNodesImg = specialDepTreeNodes;
    }

    public virtual Hashtable getSpecialDepTreeNodesImg()
    {
      return this.specialDepTreeNodesImg == null ? new Hashtable() : this.specialDepTreeNodesImg;
    }

    public virtual void SetTables(
      Hashtable featureToNodeTbl,
      Hashtable nodeToFeatureTbl,
      ArrayList dependentNodes,
      Hashtable nodeToUpdateStatus)
    {
      this.featureToNodeTbl = featureToNodeTbl;
      this.nodeToFeature = nodeToFeatureTbl;
      this.dependentNodes = dependentNodes;
      this.nodeToUpdateStatus = nodeToUpdateStatus;
    }

    public virtual void SetTables(
      Hashtable featureToNodeTbl,
      Hashtable nodeToFeatureTbl,
      ArrayList dependentNodes)
    {
      this.featureToNodeTbl = featureToNodeTbl;
      this.nodeToFeature = nodeToFeatureTbl;
      this.dependentNodes = dependentNodes;
    }
  }
}
