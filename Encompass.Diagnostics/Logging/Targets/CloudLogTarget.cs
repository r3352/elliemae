// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.Targets.CloudLogTarget
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Encompass.Diagnostics.Logging.Formatters;
using Encompass.Diagnostics.Logging.Schema;
using Encompass.Diagnostics.PII;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

#nullable disable
namespace Encompass.Diagnostics.Logging.Targets
{
  public class CloudLogTarget : ILogTarget, IDisposable
  {
    private readonly List<Log> _logs;
    private readonly string _endpointUrl;
    private readonly string _apiKey;
    private readonly string _userId;
    private readonly string _sessionId;
    private readonly HttpClient _client;
    private readonly Encoding UTF8NoBOM = (Encoding) new UTF8Encoding(false, true);

    public CloudLogTarget(string endpointUrl, string apiKey, string userId, string sessionId)
    {
      this._logs = new List<Log>();
      this._endpointUrl = endpointUrl;
      this._apiKey = apiKey;
      this._sessionId = sessionId;
      this._userId = userId;
      this._client = new HttpClient()
      {
        BaseAddress = new Uri(this._endpointUrl)
      };
    }

    public void Write(Log log)
    {
      lock (this)
        this._logs.Add(log);
    }

    public void Flush()
    {
      List<Log> logList;
      lock (this)
      {
        logList = new List<Log>((IEnumerable<Log>) this._logs);
        this._logs.Clear();
      }
      if (logList.Count < 1)
        return;
      LogBatch logBatch = new LogBatch()
      {
        ApplicationName = DiagUtility.GetAppName(),
        SessionId = this._sessionId,
        UserId = this._userId,
        Logs = logList
      };
      HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
      httpRequestMessage.RequestUri = new Uri(this._endpointUrl);
      httpRequestMessage.Method = HttpMethod.Post;
      httpRequestMessage.Headers.Add("x-api-key", this._apiKey);
      httpRequestMessage.Headers.Add("x-realm", DiagUtility.LoggerScopeProvider.GetCurrent().Instance);
      using (HttpRequestMessage request = httpRequestMessage)
      {
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        using (MemoryStream content = new MemoryStream())
        {
          using (StreamWriter sw = new StreamWriter((Stream) content, this.UTF8NoBOM, 1024, true))
            MaskUtilities.SerializeObject((TextWriter) sw, (object) logBatch, Formatting.None, JsonLogFormatter.Settings, logBatch.Logs.Any<Log>((Func<Log, bool>) (log => "SQLTrace".Equals(log.Logger, StringComparison.OrdinalIgnoreCase))));
          content.Seek(0L, SeekOrigin.Begin);
          using (StreamContent streamContent = new StreamContent((Stream) content))
          {
            request.Content = (HttpContent) streamContent;
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            using (HttpResponseMessage result = this._client.SendAsync(request).Result)
            {
              if (result.StatusCode == HttpStatusCode.OK && result.StatusCode == HttpStatusCode.Created)
                ;
            }
          }
        }
      }
    }

    public void Dispose() => this._client.Dispose();
  }
}
