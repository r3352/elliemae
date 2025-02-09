// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.ArgumentChecks
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using System;

#nullable disable
namespace Encompass.Diagnostics
{
  public static class ArgumentChecks
  {
    public static T IsNotNull<T>(T value, string parameterName)
    {
      return (object) value != null ? value : throw new ArgumentNullException(parameterName);
    }

    public static string IsNotNullOrEmpty(string value, string parameterName)
    {
      return !string.IsNullOrEmpty(value) ? value : throw new ArgumentNullException(parameterName);
    }
  }
}
