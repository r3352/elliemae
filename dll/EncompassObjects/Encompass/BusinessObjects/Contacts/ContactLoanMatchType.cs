// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.ContactLoanMatchType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  [Guid("650CB8D7-8643-4a4b-B5B9-EA43B8305AE5")]
  public enum ContactLoanMatchType
  {
    None,
    AnyCompleted,
    LastCompleted,
    AnyOriginated,
    LastOriginated,
  }
}
