// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.FeatureSecurityHelperBase
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public abstract class FeatureSecurityHelperBase : IFeatureSecurityHelper
  {
    protected Sessions.Session session;
    protected Hashtable featureToNodeTbl;
    protected Hashtable nodeToFeature;
    protected Hashtable nodeToUpdateStatus;
    protected ILevelSecurityHelper securityHelper;
    protected ArrayList dependentNodes = new ArrayList();

    public abstract void BuildNodes(TreeView treeView);

    protected FeatureSecurityHelperBase(Sessions.Session session) => this.session = session;

    public AclFeature NodeToFeature(TreeNode node)
    {
      return (AclFeature) this.nodeToFeature[(object) node];
    }

    public virtual void SetNodeUpdateStatus(TreeNode node, bool status)
    {
      if (this.nodeToUpdateStatus == null || !this.nodeToUpdateStatus.Contains((object) node))
        return;
      this.nodeToUpdateStatus[(object) node] = (object) status;
    }

    public virtual Dictionary<AclEnhancedConditionType, bool> GetEnhancedConditionsUpdatedFeatures(
      bool isUserUpdate)
    {
      return new Dictionary<AclEnhancedConditionType, bool>();
    }

    public virtual bool GetNodeUpdateStatus(TreeNode node)
    {
      bool nodeUpdateStatus = false;
      if (this.nodeToUpdateStatus != null && this.nodeToUpdateStatus.Contains((object) node))
        nodeUpdateStatus = (bool) this.nodeToUpdateStatus[(object) node];
      return nodeUpdateStatus;
    }

    public void ResetNodeUpdateStatus()
    {
      if (this.nodeToUpdateStatus == null)
        return;
      Hashtable hashtable = new Hashtable();
      foreach (object key in (IEnumerable) this.nodeToUpdateStatus.Keys)
        hashtable.Add(key, (object) false);
      this.nodeToUpdateStatus = hashtable;
    }

    public virtual Hashtable GetUpdatedFeatures(bool isUserUpdate)
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
            AclFeature feature = this.NodeToFeature(current);
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

    public virtual void UncheckAllDependentNodes()
    {
      for (int index = 0; index < this.dependentNodes.Count; ++index)
        ((TreeNode) this.dependentNodes[index]).Checked = false;
    }

    public virtual bool IsDependentOnChildren(TreeNode parentNode)
    {
      return this.dependentNodes.Contains((object) parentNode);
    }

    public virtual void SetPersonaId(int personaId)
    {
      if (this.securityHelper == null)
        return;
      this.securityHelper.SetPersonaId(personaId);
    }

    public virtual void SetPermission(AclFeature feature, bool access)
    {
      this.securityHelper.SetPermission(feature, access);
    }

    public virtual void SetPermission(AclFeature feature, AclTriState accessState)
    {
      this.securityHelper.SetPermission(feature, accessState);
    }

    public virtual Hashtable GetPersonaPermissions() => this.securityHelper.GetPersonaPermissions();

    public virtual void SetNodeImageIndex(TreeNode node, int index)
    {
      this.securityHelper.SetNodeImageIndex(node, index);
    }

    public virtual void SetNodeStates() => this.securityHelper.SetNodeStates();

    public virtual void setSpecialDepTreeNodes(Hashtable specialDepTreeNodes)
    {
      this.securityHelper.setSpecialDepTreeNodes(specialDepTreeNodes);
    }

    public virtual Hashtable getSpecialDepTreeNodes()
    {
      return this.securityHelper.getSpecialDepTreeNodes();
    }

    public virtual void setSpecialDepTreeNodesImg(Hashtable specialDepTreeNodes)
    {
      this.securityHelper.setSpecialDepTreeNodesImg(specialDepTreeNodes);
    }

    public virtual Hashtable getSpecialDepTreeNodesImg()
    {
      return this.securityHelper.getSpecialDepTreeNodesImg();
    }
  }
}
