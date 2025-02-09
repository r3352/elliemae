// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.ILogTrackedDocuments
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Loans.Templates;
using EllieMae.Encompass.Collections;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  [Guid("97F66EAF-CE93-4096-897E-217523DD3BDE")]
  public interface ILogTrackedDocuments
  {
    int Count { get; }

    TrackedDocument this[int index] { get; }

    TrackedDocument Add(string docTitle, string msName);

    void Remove(TrackedDocument docEntry);

    LogEntryList GetDocumentsForMilestone(string msName);

    IEnumerator GetEnumerator();

    LogEntryList GetDocumentsByTitle(string documentTitle);

    TrackedDocument AddFromTemplate(DocumentTemplate template, string milestoneName);
  }
}
