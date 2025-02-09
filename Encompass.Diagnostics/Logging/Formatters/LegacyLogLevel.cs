// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.Formatters.LegacyLogLevel
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using System;

#nullable disable
namespace Encompass.Diagnostics.Logging.Formatters
{
  [Serializable]
  public enum LegacyLogLevel
  {
    None = 0,
    Error = 2,
    Warning = 4,
    Information = 8,
    Verbose = 16, // 0x00000010
  }
}
