// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.AttachmentCategory
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.Client;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  /// <summary>
  /// Represents a single External attachment category setting
  /// </summary>
  public class AttachmentCategory : SessionBoundObject, IAttachmentCategory
  {
    private string statusName;
    private int settingId;

    internal AttachmentCategory(Session session, string statusName, int settingId)
      : base(session)
    {
      this.settingId = settingId;
      this.statusName = statusName;
    }

    /// <summary>Gets StatusName</summary>
    public string StatusName => this.statusName;

    /// <summary>Gets ID</summary>
    public int ID => this.settingId;

    /// <summary>Return Status Name</summary>
    /// <returns>Status Name</returns>
    public override string ToString() => this.statusName;
  }
}
