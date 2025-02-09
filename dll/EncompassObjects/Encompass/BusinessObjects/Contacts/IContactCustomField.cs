// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.IContactCustomField
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  [Guid("FD37EF56-078E-42a3-9E05-4BF72A21FD10")]
  public interface IContactCustomField
  {
    [DispId(0)]
    string Value { get; set; }

    string Name { get; }

    CustomFieldType Type { get; }

    StringList PossibleValues { get; }
  }
}
