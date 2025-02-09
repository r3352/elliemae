// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.FormBuilderSecurityHelper
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
  public class FormBuilderSecurityHelper : FeatureSecurityHelperBase
  {
    protected TreeNode customInputFormBuilderNode;
    protected TreeNode diagnosticMode;
    private AclFeature[] features = new AclFeature[2]
    {
      AclFeature.SettingsTab_Company_CustomInputFormEditor,
      AclFeature.SettingsTab_Company_DiagnosticMode
    };

    public FormBuilderSecurityHelper(Sessions.Session session, string userId, Persona[] personas)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new UserSecurityHelper(this.session, userId, personas, AclCategory.Features, this.features);
    }

    public FormBuilderSecurityHelper(Sessions.Session session, int personaId)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new PersonaSecurityHelper(this.session, personaId, AclCategory.Features, this.features);
    }

    public override void BuildNodes(TreeView treeView)
    {
      treeView.ShowLines = treeView.ShowRootLines = false;
      this.customInputFormBuilderNode = new TreeNode("Access to Input Form Builder");
      this.diagnosticMode = new TreeNode("Diagnostic Mode");
      treeView.Nodes.AddRange(new TreeNode[1]
      {
        this.customInputFormBuilderNode
      });
      treeView.Nodes.AddRange(new TreeNode[1]
      {
        this.diagnosticMode
      });
      this.nodeToFeature = new Hashtable(FeatureSets.SettingsTabCompanyFeatures.Length);
      this.nodeToFeature.Add((object) this.customInputFormBuilderNode, (object) AclFeature.SettingsTab_Company_CustomInputFormEditor);
      this.nodeToFeature.Add((object) this.diagnosticMode, (object) AclFeature.SettingsTab_Company_DiagnosticMode);
      this.featureToNodeTbl = new Hashtable(FeatureSets.SettingsTabCompanyFeatures.Length);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Company_CustomInputFormEditor, (object) this.customInputFormBuilderNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Company_DiagnosticMode, (object) this.diagnosticMode);
      this.nodeToUpdateStatus = new Hashtable();
      this.nodeToUpdateStatus.Add((object) this.customInputFormBuilderNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.diagnosticMode, (object) false);
      this.securityHelper.SetTables(this.featureToNodeTbl, this.nodeToFeature, this.dependentNodes, this.nodeToUpdateStatus);
      treeView.ExpandAll();
    }
  }
}
