// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SpecialFeatureCodePermissions
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class SpecialFeatureCodePermissions
  {
    public bool CanView { get; set; }

    public bool CanAdd { get; set; }

    public bool CanEdit { get; set; }

    public bool CanDelete { get; set; }

    public bool CanActivate { get; set; }

    public SpecialFeatureCodePermissions(FeaturesAclManager aclManager)
    {
      this.CanView = this.CanAdd = this.CanEdit = this.CanDelete = this.CanActivate = aclManager.GetUserApplicationRight(AclFeature.SettingsTab_SpecialFeatureCodes);
    }
  }
}
