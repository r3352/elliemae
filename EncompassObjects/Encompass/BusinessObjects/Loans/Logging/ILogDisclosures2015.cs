// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.ILogDisclosures2015
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>Interface for Disclosure class.</summary>
  /// <exclude />
  [Guid("B6797560-3C20-48DB-9517-86EC673A6F34")]
  public interface ILogDisclosures2015
  {
    int Count { get; }

    Disclosure2015 this[int index] { get; }

    Disclosure2015 GetMostRecentDisclosure();

    Disclosure2015 GetMostRecentStandardDisclosure(StandardDisclosure2015Type disclosureType);

    IEnumerator GetEnumerator();
  }
}
