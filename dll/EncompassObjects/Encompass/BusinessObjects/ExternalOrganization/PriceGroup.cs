// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.PriceGroup
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.Client;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  public class PriceGroup : SessionBoundObject, IPriceGroup
  {
    private string statusName;
    private string statusCode;
    private int settingId;

    internal PriceGroup(Session session, string statusName, string statusCode, int settingId)
      : base(session)
    {
      this.settingId = settingId;
      this.statusName = statusName;
      this.statusCode = statusCode;
    }

    public string StatusName => this.statusName;

    public string StatusCode => this.statusCode;

    public override string ToString() => this.statusName;

    public int ID => this.settingId;
  }
}
