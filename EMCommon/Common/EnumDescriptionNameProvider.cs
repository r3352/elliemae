// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.EnumDescriptionNameProvider
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class EnumDescriptionNameProvider : IEnumNameProvider
  {
    private Type enumType;

    public EnumDescriptionNameProvider(Type enumType) => this.enumType = enumType;

    public virtual string GetName(object value)
    {
      return Enum.GetName(this.enumType, value) == null ? (string) null : EnumUtil.GetEnumDescription((Enum) value);
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
      return EnumUtil.GetEnumValueFromDescription(name, this.enumType);
    }
  }
}
