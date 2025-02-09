// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.ContactAccessEnumNameProvider
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class ContactAccessEnumNameProvider : IEnumNameProvider
  {
    private Type enumType;

    public ContactAccessEnumNameProvider(Type enumType) => this.enumType = enumType;

    public virtual string GetName(object value)
    {
      string name = Enum.GetName(this.enumType, value);
      switch (name)
      {
        case null:
          return (string) null;
        case "Private":
          return "Personal";
        default:
          return name;
      }
    }

    public virtual string[] GetNames()
    {
      return new string[2]{ "Personal", "Public" };
    }

    public virtual object GetValue(string name)
    {
      if (name.Trim().ToLower() == "personal")
        name = "Private";
      return Enum.Parse(this.enumType, name.Replace(" ", ""), true);
    }
  }
}
