// Decompiled with JetBrains decompiler
// Type: Elli.Common.Extensions.ByteExtensions
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

#nullable disable
namespace Elli.Common.Extensions
{
  public static class ByteExtensions
  {
    public static bool IsNotNullOrZero(this byte? value) => !value.IsNullOrZero();

    public static bool IsNullOrZero(this byte? value)
    {
      if (!value.HasValue)
        return true;
      byte? nullable1 = value;
      int? nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      int num = 0;
      return nullable2.GetValueOrDefault() == num & nullable2.HasValue;
    }
  }
}
