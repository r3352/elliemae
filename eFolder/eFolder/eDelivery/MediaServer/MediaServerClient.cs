// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.eDelivery.MediaServer.MediaServerClient
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

#nullable disable
namespace EllieMae.EMLite.eFolder.eDelivery.MediaServer
{
  public class MediaServerClient
  {
    private const string className = "MediaServiceClient";
    private static readonly string sw = Tracing.SwEFolder;
    private Sessions.Session session;
    private int VaultBufferSize;
    private static HttpClient client = new HttpClient();

    public MediaServerClient()
      : this(Session.DefaultInstance)
    {
      this.VaultBufferSize = 6553590;
    }

    public MediaServerClient(Sessions.Session session) => this.session = session;

    public async Task<string> SaveVaultFile(
      string saveVaultFileUrl,
      string fileName,
      string filePath)
    {
      string str1 = (string) null;
      string msg1 = "Calling SaveVaultFile";
      string str2;
      try
      {
        Tracing.Log(MediaServerClient.sw, TraceLevel.Verbose, "MediaServiceClient", msg1);
        using (FileStream stream = new FileStream(filePath, FileMode.Open))
        {
          using (StreamContent fileContent = new StreamContent((Stream) stream, this.VaultBufferSize))
          {
            using (MultipartFormDataContent content = new MultipartFormDataContent())
            {
              content.Add((HttpContent) fileContent, "file1", fileName + "." + Path.GetExtension(filePath));
              using (HttpRequestMessage request = new HttpRequestMessage())
              {
                request.Method = HttpMethod.Post;
                request.Content = (HttpContent) content;
                request.RequestUri = new Uri(saveVaultFileUrl);
                HttpResponseMessage response = (HttpResponseMessage) null;
                response = await MediaServerClient.client.SendAsync(request);
                MediaUploadResult mediaUploadResult = JsonConvert.DeserializeObject<MediaUploadResult>(await response.Content.ReadAsStringAsync());
                if (mediaUploadResult == null || mediaUploadResult.Files == null || !response.IsSuccessStatusCode)
                  throw new MediaServerException(JsonConvert.DeserializeObject<MediaServerError>(await response.Content.ReadAsStringAsync()), response.StatusCode);
                str1 = mediaUploadResult.Files[0].FileId;
                response = (HttpResponseMessage) null;
              }
            }
          }
        }
        str2 = str1;
      }
      catch (Exception ex)
      {
        string msg2 = "Error in Vault service call to SaveVaultFile. Ex: " + (object) ex;
        Tracing.Log(MediaServerClient.sw, TraceLevel.Error, "MediaServiceClient", msg2);
        throw ex;
      }
      return str2;
    }
  }
}
