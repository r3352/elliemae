// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.ContactTypeNameProvider
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.Common;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class ContactTypeNameProvider : CustomEnumNameProvider
  {
    private static Hashtable valueToNameMap = new Hashtable();

    static ContactTypeNameProvider()
    {
      ContactTypeNameProvider.valueToNameMap.Add((object) ContactType.BizPartner, (object) "Business");
      ContactTypeNameProvider.valueToNameMap.Add((object) ContactType.Borrower, (object) "Borrower");
    }

    public ContactTypeNameProvider()
      : base(typeof (ContactType), ContactTypeNameProvider.valueToNameMap)
    {
    }
  }
}
