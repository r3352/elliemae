// Decompiled with JetBrains decompiler
// Type: Elli.Common.Extensions.StringExtensions
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

#nullable disable
namespace Elli.Common.Extensions
{
  public static class StringExtensions
  {
    public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);

    public static bool IsNullOrWhiteSpace(this string value) => string.IsNullOrWhiteSpace(value);

    public static string FormatGuidToString(this string value)
    {
      if (string.IsNullOrWhiteSpace(value))
        return string.Empty;
      return value.Trim('{', '}');
    }
  }
}
