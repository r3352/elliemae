// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.SkyDriveViewer
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.SkyDrive;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Server.SkyDrive;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects
{
  public class SkyDriveViewer
  {
    private static readonly string sw = Tracing.SwEFolder;
    private const string className = "SkyDriveViewer�";

    public static SkyDriveUrl GetSkyDriveViewerURL(string attachmentId, ISession Session)
    {
      Task<SkyDriveUrl> skyDriveUrlForGet = new SkyDriveRestClient((IClientContext) ClientContext.GetCurrent(), Session.SessionInfo.UserID).GetSkyDriveUrlForGet(attachmentId);
      Task.WaitAll((Task) skyDriveUrlForGet);
      SkyDriveUrl result = skyDriveUrlForGet.Result;
      Tracing.Log(SkyDriveViewer.sw, TraceLevel.Verbose, nameof (SkyDriveViewer), "Checking GetSkyDriveUrlForObject Response");
      return result != null ? result : throw new FileNotFoundException("Unable to generate pre-sign url for 'Attachment Title'", attachmentId);
    }
  }
}
