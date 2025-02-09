// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Diagnostics.CustomLogFilter
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using Encompass.Diagnostics;
using Encompass.Diagnostics.Logging.Filters;
using Encompass.Diagnostics.Logging.Schema;

#nullable disable
namespace EllieMae.EMLite.Common.Diagnostics
{
  public class CustomLogFilter : ILogFilter
  {
    private readonly ILogFilter _innerLogFilter;

    public static event CustomLogFilter.IsActiveForLogHandler IsActiveForLog;

    public CustomLogFilter(ILogFilter innerLogFilter)
    {
      this._innerLogFilter = ArgumentChecks.IsNotNull<ILogFilter>(innerLogFilter, nameof (innerLogFilter));
    }

    public bool IsActiveFor(Log log)
    {
      CustomLogFilter.IsActiveForLogHandler isActiveForLog = CustomLogFilter.IsActiveForLog;
      return isActiveForLog == null ? this._innerLogFilter.IsActiveFor(log) : isActiveForLog(log);
    }

    public delegate bool IsActiveForLogHandler(Log log);
  }
}
