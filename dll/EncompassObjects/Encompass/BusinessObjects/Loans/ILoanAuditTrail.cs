// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.ILoanAuditTrail
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  [Guid("AA405C97-9753-4a11-92A5-26C3B34D8E1D")]
  public interface ILoanAuditTrail
  {
    AuditTrailEntryList GetHistory(string fieldId);

    AuditTrailEntry GetMostRecentEntry(string fieldId);

    AuditTrailEntryList GetMostRecentEntries();

    StringList GetAuditFieldList();
  }
}
