// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.ILogDisclosures
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.BusinessObjects.Users;
using System;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>Interface for Disclosure class.</summary>
  /// <exclude />
  [Guid("75B40246-F2A2-4e96-A71D-D55ABA05B343")]
  public interface ILogDisclosures
  {
    int Count { get; }

    Disclosure this[int index] { get; }

    Disclosure Add(DateTime disclosureDate, StandardDisclosureType disclosureType);

    Disclosure AddForUser(
      DateTime disclosureDate,
      StandardDisclosureType disclosureType,
      User disclosuedByUser);

    Disclosure GetMostRecentDisclosure();

    Disclosure GetMostRecentStandardDisclosure(StandardDisclosureType disclosureType);

    IEnumerator GetEnumerator();
  }
}
