// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PersonaSecurityHelper
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class PersonaSecurityHelper : LevelSecurityHelperBase
  {
    private int personaId;
    private AclCategory featureCategory;
    private AclFeature[] features;
    private AclEnhancedConditionType[] enhancedConditionFeatures;
    private Sessions.Session session;

    public PersonaSecurityHelper(
      Sessions.Session session,
      int personaId,
      AclCategory featureCategory,
      AclFeature[] features)
    {
      this.session = session;
      this.personaId = personaId;
      this.featureCategory = featureCategory;
      this.features = features;
    }

    public PersonaSecurityHelper(
      Sessions.Session session,
      int personaId,
      AclEnhancedConditionType[] features)
    {
      this.session = session;
      this.personaId = personaId;
      this.enhancedConditionFeatures = features;
    }

    public override void SetPersonaId(int personaId) => this.personaId = personaId;

    public override void SetPermission(AclFeature feature, AclTriState access)
    {
      ((FeaturesAclManager) this.session.ACL.GetAclManager(this.featureCategory)).SetPermission(feature, this.personaId, access);
    }

    public override void SetPermission(AclFeature feature, bool access)
    {
      this.SetPermission(feature, access ? AclTriState.True : AclTriState.False);
    }

    public override Hashtable GetPersonaPermissions()
    {
      return ((FeaturesAclManager) this.session.ACL.GetAclManager(this.featureCategory)).GetPermissions(this.features, this.personaId);
    }

    public override void SetNodeImageIndex(TreeNode node, int index)
    {
    }

    public override void SetNodeStates()
    {
      Hashtable personaPermissions = this.GetPersonaPermissions();
      if (personaPermissions == null)
        throw new ApplicationException("Feature rights from a call to GetPersonaPermissions is null.");
      for (int index = 0; index < this.features.Length; ++index)
      {
        if (this.featureToNodeTbl.Contains((object) this.features[index]))
          ((TreeNode) this.featureToNodeTbl[(object) this.features[index]]).Checked = (bool) personaPermissions[(object) this.features[index]];
      }
      string str = "";
      for (int index = 0; index < this.dependentNodes.Count; ++index)
      {
        bool flag = false;
        str = "";
        string text = ((TreeNode) this.dependentNodes[index]).Text;
        if (this.getSpecialDepTreeNodes().ContainsKey((object) text))
        {
          if ((bool) this.getSpecialDepTreeNodes()[(object) text])
            ((TreeNode) this.dependentNodes[index]).Checked = true;
        }
        else
        {
          foreach (TreeNode node in ((TreeNode) this.dependentNodes[index]).Nodes)
          {
            if (node.Checked)
              flag = true;
          }
          if (flag)
            ((TreeNode) this.dependentNodes[index]).Checked = true;
        }
      }
    }
  }
}
