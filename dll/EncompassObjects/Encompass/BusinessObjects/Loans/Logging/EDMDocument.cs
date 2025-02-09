// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.EDMDocument
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class EDMDocument : IEDMDocument
  {
    private string title;

    internal EDMDocument(string title) => this.title = title;

    public string Title => this.title;
  }
}
