// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.IPostClosingConditionTemplates
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  [Guid("ED0D386A-B33A-4d69-BD16-B8118A11F5FD")]
  public interface IPostClosingConditionTemplates
  {
    int Count { get; }

    PostClosingConditionTemplate this[int index] { get; }

    PostClosingConditionTemplate GetTemplateByID(string templateId);

    PostClosingConditionTemplate GetTemplateByTitle(string title);

    IEnumerator GetEnumerator();
  }
}
