// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.PriceGroup
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.Client;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  /// <summary>Represents a single External price group setting</summary>
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

    /// <summary>Gets StatusName</summary>
    public string StatusName => this.statusName;

    /// <summary>Gets StatusCode</summary>
    public string StatusCode => this.statusCode;

    /// <summary>Return Status Name</summary>
    /// <returns>Status Name</returns>
    public override string ToString() => this.statusName;

    /// <summary>Gets ID</summary>
    public int ID => this.settingId;
  }
}
