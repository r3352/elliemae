// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.IContactOpportunities
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  /// <summary>Interface for ContactOpportunities class.</summary>
  /// <exclude />
  [Guid("49E27995-7422-4870-87B2-5CA8303E8974")]
  public interface IContactOpportunities
  {
    ContactOpportunity this[int index] { get; }

    int Count { get; }

    ContactOpportunity Add();

    void Remove(ContactOpportunity note);

    IEnumerator GetEnumerator();

    void Refresh();
  }
}
