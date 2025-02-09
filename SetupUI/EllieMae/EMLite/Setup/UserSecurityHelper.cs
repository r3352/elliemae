// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.UserSecurityHelper
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class UserSecurityHelper : LevelSecurityHelperBase
  {
    private Sessions.Session session;
    private string userId;
    private Persona[] personas;
    private AclCategory featureCategory;
    private AclFeature[] features;
    private AclEnhancedConditionType[] enhancedConditionFeatures;

    public UserSecurityHelper(
      Sessions.Session session,
      string userId,
      Persona[] personas,
      AclCategory featureCategory,
      AclFeature[] features)
    {
      this.session = session;
      this.userId = userId;
      this.personas = personas;
      this.featureCategory = featureCategory;
      this.features = features;
    }

    public UserSecurityHelper(
      Sessions.Session session,
      string userId,
      Persona[] personas,
      AclEnhancedConditionType[] features)
    {
      this.session = session;
      this.userId = userId;
      this.personas = personas;
      this.enhancedConditionFeatures = features;
    }

    public override void SetPermission(AclFeature feature, bool access)
    {
      this.SetPermission(feature, access ? AclTriState.True : AclTriState.False);
    }

    public override void SetPermission(AclFeature feature, AclTriState accessState)
    {
      ((FeaturesAclManager) this.session.ACL.GetAclManager(this.featureCategory)).SetPermission(feature, this.userId, accessState);
    }

    public override Hashtable GetPersonaPermissions()
    {
      return ((FeaturesAclManager) this.session.ACL.GetAclManager(this.featureCategory)).GetPermissions(this.features, this.personas) ?? throw new ApplicationException("Persona feature rights from a call to GetPersonaPermissions is null.");
    }

    public override void SetNodeImageIndex(TreeNode node, int index)
    {
      node.ImageIndex = index;
      node.SelectedImageIndex = index;
      bool flag1 = false;
      if (node.Tag is AclFeature)
      {
        switch ((AclFeature) node.Tag)
        {
          case AclFeature.eFolder_Other_ManageAccessToDocs:
          case AclFeature.eFolder_AttachDoc:
          case AclFeature.eFolder_DeleteDoc:
          case AclFeature.eFolder_RequestDoc:
          case AclFeature.eFolder_SendDoc:
          case AclFeature.eFolder_AssignAs:
            flag1 = true;
            break;
        }
      }
      if (flag1 || node.Parent == null || node.Parent.SelectedImageIndex == index)
        return;
      bool flag2 = false;
      foreach (TreeNode node1 in node.Parent.Nodes)
      {
        if (node1.SelectedImageIndex != index)
        {
          flag2 = true;
          break;
        }
      }
      if (flag2)
        return;
      this.SetNodeImageIndex(node.Parent, index);
    }

    public override void SetNodeStates()
    {
      FeaturesAclManager aclManager = (FeaturesAclManager) this.session.ACL.GetAclManager(this.featureCategory);
      Hashtable permissions1 = aclManager.GetPermissions(this.features, this.userId);
      Hashtable permissions2 = aclManager.GetPermissions(this.features, this.personas);
      if (permissions1 == null)
        throw new ApplicationException("User feature rights from a call to GetPersonaPermissions is null.");
      if (permissions2 == null)
        throw new ApplicationException("Persona feature rights from a call to GetPersonaPermissions is null.");
      Hashtable hashtable = new Hashtable(this.features.Length);
      for (int index = 0; index < this.features.Length; ++index)
      {
        if (this.featureToNodeTbl.Contains((object) this.features[index]))
        {
          TreeNode node = (TreeNode) this.featureToNodeTbl[(object) this.features[index]];
          bool flag1;
          bool flag2;
          if (permissions1.Contains((object) this.features[index]))
          {
            AclTriState aclTriState = (AclTriState) permissions1[(object) this.features[index]];
            if (aclTriState == AclTriState.Unspecified)
            {
              flag1 = true;
              flag2 = (bool) permissions2[(object) this.features[index]];
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
            flag2 = (bool) permissions2[(object) this.features[index]];
          }
          if (flag1)
            this.SetNodeImageIndex(node, 0);
          else
            this.SetNodeImageIndex(node, 1);
          node.Checked = flag2;
        }
      }
      string str = "";
      for (int index = 0; index < this.dependentNodes.Count; ++index)
      {
        bool flag3 = false;
        bool flag4 = false;
        str = "";
        string text = ((TreeNode) this.dependentNodes[index]).Text;
        if (this.getSpecialDepTreeNodes().ContainsKey((object) text))
        {
          if ((bool) this.getSpecialDepTreeNodes()[(object) text])
            flag3 = true;
          if ((int) this.getSpecialDepTreeNodesImg()[(object) text] == 1)
            flag4 = true;
        }
        else
        {
          foreach (TreeNode node in ((TreeNode) this.dependentNodes[index]).Nodes)
          {
            if (node.Checked)
              flag3 = true;
            if (node.ImageIndex == 1)
              flag4 = true;
          }
        }
        TreeNode dependentNode = (TreeNode) this.dependentNodes[index];
        if (flag3)
          dependentNode.Checked = true;
        if (flag4)
          this.SetNodeImageIndex(dependentNode, 1);
        else
          this.SetNodeImageIndex(dependentNode, 0);
      }
    }
  }
}
