// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.eDelivery.HelperMethods.LoggingHelper
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

#nullable disable
namespace EllieMae.EMLite.eFolder.eDelivery.HelperMethods
{
  public class LoggingHelper
  {
    private const string correleationHeader = "X-Correlation-ID";
    private static readonly string sw = Tracing.SwEFolder;

    public static async Task LogErrors(
      HttpResponseMessage response,
      string className,
      string message)
    {
      string correleationID = string.Empty;
      IEnumerable<string> values;
      if (response.Headers.TryGetValues("X-Correlation-ID", out values))
        correleationID = values.FirstOrDefault<string>();
      string str1 = await response.Content.ReadAsStringAsync();
      string str2 = response.StatusCode.ToString();
      string msg = message + Environment.NewLine + "Status code = " + str2 + ", X-Correlation-ID = " + correleationID + Environment.NewLine + str1;
      Tracing.Log(LoggingHelper.sw, TraceLevel.Error, className, msg);
    }
  }
}
