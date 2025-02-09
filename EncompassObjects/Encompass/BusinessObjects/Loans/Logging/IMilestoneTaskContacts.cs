// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.IMilestoneTaskContacts
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.BusinessObjects.Contacts;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>Interface for MilestoneTaskContacts class.</summary>
  /// <exclude />
  [Guid("935A2658-13B0-434c-87B3-5E08588E4B46")]
  public interface IMilestoneTaskContacts
  {
    int Count { get; }

    MilestoneTaskContact this[int index] { get; }

    MilestoneTaskContact Add(string contactName, string contactEmail, string contactPhone);

    MilestoneTaskContact Add(BizContact contactToLink);

    void Remove(MilestoneTaskContact contactToRemove);

    void Clear();

    IEnumerator GetEnumerator();
  }
}
