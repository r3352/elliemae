// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.BitmaskEnumNameProvider
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class BitmaskEnumNameProvider : IEnumNameProvider
  {
    private Type enumType;
    private IEnumNameProvider baseProvider;

    public BitmaskEnumNameProvider(Type enumType, IEnumNameProvider baseProvider)
    {
      this.enumType = enumType;
      this.baseProvider = baseProvider;
    }

    public BitmaskEnumNameProvider(Type enumType)
      : this(enumType, (IEnumNameProvider) new DefaultEnumNameProvider(enumType))
    {
    }

    public IEnumNameProvider BaseProvider => this.baseProvider;

    public virtual string GetName(object value)
    {
      string name = this.baseProvider.GetName(value);
      if (name != null)
        return name;
      StringBuilder stringBuilder = new StringBuilder();
      int num = (int) value;
      int[] values = (int[]) Enum.GetValues(this.enumType);
      for (int index = 0; index < values.Length; ++index)
      {
        if (values[index] != 0 && (num & values[index]) == values[index])
          stringBuilder.Append((stringBuilder.Length == 0 ? "" : ", ") + this.baseProvider.GetName((object) values[index]));
      }
      return stringBuilder.ToString();
    }

    public virtual string[] GetNames() => this.baseProvider.GetNames();

    public virtual object GetValue(string name)
    {
      int num = 0;
      string str1 = name;
      char[] chArray = new char[1]{ ',' };
      foreach (string str2 in str1.Split(chArray))
        num |= (int) this.baseProvider.GetValue(str2.Trim());
      return Enum.ToObject(this.enumType, num);
    }
  }
}
