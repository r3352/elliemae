// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.IUnderwritingConditionTemplates
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  [Guid("E09A74C1-1E15-46dc-875E-73E0C7ADD659")]
  public interface IUnderwritingConditionTemplates
  {
    int Count { get; }

    UnderwritingConditionTemplate this[int index] { get; }

    UnderwritingConditionTemplate GetTemplateByID(string templateId);

    UnderwritingConditionTemplate GetTemplateByTitle(string title);

    IEnumerator GetEnumerator();
  }
}
