// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.IContactCustomFields
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  /// <summary>Interface for ContactCustomFields class.</summary>
  /// <exclude />
  [Guid("AAD7B7C6-09ED-408c-98EB-ECE6AD6CEBC3")]
  public interface IContactCustomFields
  {
    ContactCustomField this[string fname] { get; }

    int Count { get; }

    IEnumerator GetEnumerator();
  }
}
