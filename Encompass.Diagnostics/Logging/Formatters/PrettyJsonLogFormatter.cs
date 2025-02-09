// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.Formatters.PrettyJsonLogFormatter
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Newtonsoft.Json;

#nullable disable
namespace Encompass.Diagnostics.Logging.Formatters
{
  public class PrettyJsonLogFormatter : JsonLogFormatter, ILogFormatter
  {
    public PrettyJsonLogFormatter()
      : base(Formatting.Indented)
    {
    }
  }
}
