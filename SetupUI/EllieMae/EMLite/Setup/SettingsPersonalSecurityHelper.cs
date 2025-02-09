// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SettingsPersonalSecurityHelper
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
  public class SettingsPersonalSecurityHelper : FeatureSecurityHelperBase
  {
    protected TreeNode assignmentOfRightsNode;
    protected TreeNode myProfileNode;
    protected TreeNode myProfileNameNode;
    protected TreeNode myProfileEmailNode;
    protected TreeNode myProfilePhotoNode;
    protected TreeNode myProfilePhoneNode;
    protected TreeNode myProfileCellNode;
    protected TreeNode myProfileFaxNode;
    protected TreeNode defaultFileContactNode;
    protected TreeNode dashboardTemplateNode;
    protected TreeNode dashboardViewTemplateNode;

    public SettingsPersonalSecurityHelper(
      Sessions.Session session,
      string userId,
      Persona[] personas)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new UserSecurityHelper(this.session, userId, personas, AclCategory.Features, FeatureSets.SettingsTabPersonalFeatures);
    }

    public SettingsPersonalSecurityHelper(Sessions.Session session, int personaId)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new PersonaSecurityHelper(this.session, personaId, AclCategory.Features, FeatureSets.SettingsTabPersonalFeatures);
    }

    public Hashtable SpecialDepTreeNodes
    {
      get => this.securityHelper.getSpecialDepTreeNodes();
      set => this.securityHelper.setSpecialDepTreeNodes(value);
    }

    public override void BuildNodes(TreeView treeView)
    {
      this.assignmentOfRightsNode = new TreeNode("Grant File Access");
      this.myProfileNode = new TreeNode("My Profile");
      this.myProfileNameNode = new TreeNode("Name");
      this.myProfileEmailNode = new TreeNode("Email");
      this.myProfilePhotoNode = new TreeNode("Public Profile");
      this.myProfilePhoneNode = new TreeNode("Phone #");
      this.myProfileCellNode = new TreeNode("Cell #");
      this.myProfileFaxNode = new TreeNode("Fax #");
      this.defaultFileContactNode = new TreeNode("Default File Contacts");
      this.myProfileNode.Nodes.AddRange(new TreeNode[6]
      {
        this.myProfileNameNode,
        this.myProfileEmailNode,
        this.myProfilePhoneNode,
        this.myProfileCellNode,
        this.myProfileFaxNode,
        this.myProfilePhotoNode
      });
      treeView.Nodes.AddRange(new TreeNode[3]
      {
        this.myProfileNode,
        this.defaultFileContactNode,
        this.assignmentOfRightsNode
      });
      this.dependentNodes.Add((object) this.myProfileNode);
      treeView.ExpandAll();
      this.nodeToFeature = new Hashtable();
      this.nodeToFeature.Add((object) this.assignmentOfRightsNode, (object) AclFeature.SettingsTab_Personal_AssignmentOfRights);
      this.nodeToFeature.Add((object) this.myProfileNameNode, (object) AclFeature.SettingsTab_Personal_MyProfileName);
      this.nodeToFeature.Add((object) this.myProfileEmailNode, (object) AclFeature.SettingsTab_Personal_MyProfileEmail);
      this.nodeToFeature.Add((object) this.myProfilePhotoNode, (object) AclFeature.SettingsTab_Personal_MyProfilePhoto);
      this.nodeToFeature.Add((object) this.myProfilePhoneNode, (object) AclFeature.SettingsTab_Personal_MyProfilePhone);
      this.nodeToFeature.Add((object) this.myProfileCellNode, (object) AclFeature.SettingsTab_Personal_MyProfileCell);
      this.nodeToFeature.Add((object) this.myProfileFaxNode, (object) AclFeature.SettingsTab_Personal_MyProfileFax);
      this.nodeToFeature.Add((object) this.defaultFileContactNode, (object) AclFeature.SettingsTab_Personal_DefaultFileContacts);
      this.featureToNodeTbl = new Hashtable(FeatureSets.SettingsTabPersonalFeatures.Length);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Personal_AssignmentOfRights, (object) this.assignmentOfRightsNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Personal_MyProfileName, (object) this.myProfileNameNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Personal_MyProfileEmail, (object) this.myProfileEmailNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Personal_MyProfilePhoto, (object) this.myProfilePhotoNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Personal_MyProfilePhone, (object) this.myProfilePhoneNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Personal_MyProfileCell, (object) this.myProfileCellNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Personal_MyProfileFax, (object) this.myProfileFaxNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Personal_DefaultFileContacts, (object) this.defaultFileContactNode);
      this.nodeToUpdateStatus = new Hashtable();
      this.nodeToUpdateStatus.Add((object) this.assignmentOfRightsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.myProfileNameNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.myProfileEmailNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.myProfilePhotoNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.myProfilePhoneNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.myProfileCellNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.myProfileFaxNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.defaultFileContactNode, (object) false);
      this.securityHelper.SetTables(this.featureToNodeTbl, this.nodeToFeature, this.dependentNodes, this.nodeToUpdateStatus);
    }
  }
}
