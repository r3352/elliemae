// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.ITaskTemplates
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  [Guid("92E7F436-C571-42c2-9A81-90916F7E8A85")]
  public interface ITaskTemplates
  {
    int Count { get; }

    TaskTemplate this[int index] { get; }

    TaskTemplate GetTemplateByID(string templateId);

    TaskTemplate GetTemplateByName(string taskName);

    IEnumerator GetEnumerator();
  }
}
