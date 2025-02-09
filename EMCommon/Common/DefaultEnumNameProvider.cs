// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.DefaultEnumNameProvider
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class DefaultEnumNameProvider : IEnumNameProvider
  {
    private Type enumType;

    public DefaultEnumNameProvider(Type enumType) => this.enumType = enumType;

    public virtual string GetName(object value) => Enum.GetName(this.enumType, value);

    public virtual string[] GetNames() => Enum.GetNames(this.enumType);

    public virtual object GetValue(string name) => Enum.Parse(this.enumType, name, true);
  }
}
