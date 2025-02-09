// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Configuration.TablesAndFees.TablesFeesSettings
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.Client;

#nullable disable
namespace EllieMae.Encompass.Configuration.TablesAndFees
{
  public class TablesFeesSettings : SessionBoundObject
  {
    private FeeManagementSettings feeManagement;

    internal TablesFeesSettings(Session session)
      : base(session)
    {
      this.feeManagement = new FeeManagementSettings(session);
    }

    public FeeManagementSettings FeeManagement => this.feeManagement;
  }
}
