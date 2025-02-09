// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.Formatters.JsonLogFormatter
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Encompass.Diagnostics.Logging.Schema;
using Encompass.Diagnostics.PII;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace Encompass.Diagnostics.Logging.Formatters
{
  public class JsonLogFormatter : ILogFormatter
  {
    public static JsonSerializerSettings Settings = new JsonSerializerSettings()
    {
      NullValueHandling = NullValueHandling.Ignore,
      DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ",
      DateTimeZoneHandling = DateTimeZoneHandling.Utc,
      ContractResolver = (IContractResolver) new LoggingContractResolver()
    };
    private readonly Formatting _formatting;

    public JsonLogFormatter()
      : this(Formatting.None)
    {
    }

    public JsonLogFormatter(Formatting formatting) => this._formatting = formatting;

    public LogFormat GetFormat()
    {
      return this._formatting == Formatting.None ? LogFormat.Json : LogFormat.PrettyJson;
    }

    public void WriteFormatted(Log log, TextWriter textWriter)
    {
      MaskUtilities.SerializeObject(textWriter, (object) log, this._formatting, JsonLogFormatter.Settings, "SQLTrace".Equals(log.Logger, StringComparison.OrdinalIgnoreCase));
    }

    public string FormatLog(Log log)
    {
      StringBuilder sb = new StringBuilder(256);
      using (StringWriter stringWriter = new StringWriter(sb))
        this.WriteFormatted(log, (TextWriter) stringWriter);
      return sb.ToString();
    }
  }
}
