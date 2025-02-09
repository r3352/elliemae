// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.DocumentCategory
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.Client;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  public class DocumentCategory : SessionBoundObject, IDocumentCategory
  {
    private string categoryName;
    private int settingId;

    internal DocumentCategory(Session session, string categoryName, int settingId)
      : base(session)
    {
      this.settingId = settingId;
      this.categoryName = categoryName;
    }

    public string CategoryName => this.categoryName;

    public override string ToString() => this.categoryName;

    public int ID => this.settingId;
  }
}
