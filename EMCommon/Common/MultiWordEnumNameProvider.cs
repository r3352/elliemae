// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.MultiWordEnumNameProvider
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class MultiWordEnumNameProvider : IEnumNameProvider
  {
    private Type enumType;

    public MultiWordEnumNameProvider(Type enumType) => this.enumType = enumType;

    public virtual string GetName(object value)
    {
      string name1 = Enum.GetName(this.enumType, value);
      if (name1 == null)
        return (string) null;
      string name2 = "";
      char ch;
      for (int index = 0; index < name1.Length; ++index)
      {
        if (index == 0)
        {
          string str1 = name2;
          ch = name1[index];
          string str2 = ch.ToString();
          name2 = str1 + str2;
        }
        else if (char.IsUpper(name1[index]))
        {
          string str3 = name2;
          ch = name1[index];
          string str4 = ch.ToString();
          name2 = str3 + " " + str4;
        }
        else
        {
          string str5 = name2;
          ch = name1[index];
          string str6 = ch.ToString();
          name2 = str5 + str6;
        }
      }
      return name2;
    }

    public virtual string[] GetNames()
    {
      Array values = Enum.GetValues(this.enumType);
      string[] names = new string[values.Length];
      for (int index = 0; index < values.Length; ++index)
        names[index] = this.GetName(values.GetValue(index));
      return names;
    }

    public virtual object GetValue(string name)
    {
      return Enum.Parse(this.enumType, name.Replace(" ", ""), true);
    }
  }
}
