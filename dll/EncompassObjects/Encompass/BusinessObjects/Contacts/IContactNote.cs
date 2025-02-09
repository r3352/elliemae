// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.IContactNote
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  [Guid("F4D1FBC7-09A1-4efa-898F-A9D3A20F30CF")]
  public interface IContactNote
  {
    int ID { get; }

    string Subject { get; set; }

    DateTime Timestamp { get; set; }

    string Details { get; set; }

    void Commit();

    void Refresh();
  }
}
