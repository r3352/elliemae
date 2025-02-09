// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.DocumentCategory
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.Client;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  /// <summary>DocumentCategory Class</summary>
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

    /// <summary>Gets StatusName</summary>
    public string CategoryName => this.categoryName;

    /// <summary>Returns Category Name</summary>
    /// <returns></returns>
    public override string ToString() => this.categoryName;

    /// <summary>Gets ID</summary>
    public int ID => this.settingId;
  }
}
