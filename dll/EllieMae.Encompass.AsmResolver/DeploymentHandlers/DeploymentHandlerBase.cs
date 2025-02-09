// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.DeploymentHandlers.DeploymentHandlerBase
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using EllieMae.Encompass.AsmResolver.Manifests;
using EllieMae.Encompass.AsmResolver.Utils;
using System.IO;
using System.Text;

#nullable disable
namespace EllieMae.Encompass.AsmResolver.DeploymentHandlers
{
  internal abstract class DeploymentHandlerBase
  {
    protected static readonly Encoding defaultEncoding = Encoding.ASCII;
    internal readonly string UrlSeparator = "\\";
    protected readonly string appSuiteName;
    protected readonly string executableName;
    internal readonly string Root;
    protected DeploymentManifest deployManifest;
    protected ApplicationManifest appManifest;

    internal DeploymentHandlerBase(string root, string appSuiteName, string executableName)
    {
      if (BasicUtils.IsHttpOrHttps(root))
        this.UrlSeparator = "/";
      this.Root = root;
      this.appSuiteName = appSuiteName;
      this.executableName = executableName;
    }

    internal abstract string AppFolderUrl { get; }

    internal abstract DeploymentManifest DeployManifest { get; set; }

    internal abstract ApplicationManifest AppManifest { get; set; }

    protected static string getFileText(string url, long blockSize, long size)
    {
      return DeploymentHandlerBase.getFileText(url, blockSize, size, DeploymentHandlerBase.defaultEncoding);
    }

    protected static string getFileText(string url, long blockSize, long size, Encoding encoding)
    {
      if (blockSize <= 0L)
        blockSize = (long) FileUtil.FileDownloadBlockSize;
      byte[] file = DeploymentHandlerBase.getFile(url, blockSize, size, (string) null);
      return encoding.GetString(file);
    }

    protected static byte[] getFile(string url)
    {
      return DeploymentHandlerBase.getFile(url, 0L, 0L, (string) null);
    }

    protected static byte[] getFile(string url, long blockSize)
    {
      return DeploymentHandlerBase.getFile(url, blockSize, 0L, (string) null);
    }

    private static string myUrlEncode(string url)
    {
      url = url.Replace("%", "%25");
      url = url.Replace("#", "%23");
      url = url.Replace('\\', '/');
      return url;
    }

    protected static byte[] getFile(
      string url,
      long blockSize,
      long size,
      string progressBarTitle)
    {
      return !BasicUtils.IsHttpOrHttps(url) ? File.ReadAllBytes(url) : WebUtil.GetFile(url, blockSize, size, progressBarTitle);
    }
  }
}
