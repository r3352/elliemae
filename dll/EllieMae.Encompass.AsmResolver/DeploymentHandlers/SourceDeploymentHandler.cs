// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.DeploymentHandlers.SourceDeploymentHandler
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using EllieMae.Encompass.AsmResolver.Manifests;
using EllieMae.Encompass.AsmResolver.Utils;
using System;
using System.Diagnostics;
using System.IO;

#nullable disable
namespace EllieMae.Encompass.AsmResolver.DeploymentHandlers
{
  internal class SourceDeploymentHandler : DeploymentHandlerBase
  {
    private DeploymentHandler deployHandler;
    private bool appManifestLoadedFromCache;

    internal SourceDeploymentHandler(
      DeploymentHandler deployHandler,
      string root,
      string appSuiteName,
      string executableName)
      : base(root, appSuiteName, executableName)
    {
      this.deployHandler = deployHandler;
    }

    internal override string AppFolderUrl
    {
      get
      {
        return this.DeployManifest == null ? (string) null : this.Root + this.UrlSeparator + this.DeployManifest.GetPubLocAppFolder(this.executableName);
      }
    }

    private string parseHttpEquivRefresh(string httpResponse)
    {
      string httpEquivRefresh = (string) null;
      int startIndex = httpResponse.IndexOf("HTTP-EQUIV=\"refresh\"", StringComparison.CurrentCultureIgnoreCase);
      if (startIndex >= 0)
      {
        string str1 = httpResponse.Substring(startIndex);
        int num = str1.IndexOf("URL=", StringComparison.CurrentCultureIgnoreCase);
        if (num >= 0)
        {
          string str2 = str1.Substring(num + 4);
          int length = str2.IndexOf("\"");
          if (length > 0)
            httpEquivRefresh = str2.Substring(0, length);
        }
      }
      return httpEquivRefresh;
    }

    internal override DeploymentManifest DeployManifest
    {
      get
      {
        if (this.deployManifest == null)
        {
          int num = 2;
          for (int index = 0; index <= num; ++index)
          {
            string fileText = DeploymentHandlerBase.getFileText(this.Root + this.UrlSeparator + this.appSuiteName + ResolverConsts.DeployManifestExt, 51200L, 0L);
            try
            {
              this.deployManifest = new DeploymentManifest(fileText);
              break;
            }
            catch (Exception ex)
            {
              try
              {
                if (index != 0)
                {
                  if (index != num)
                    goto label_10;
                }
                if (BasicUtils.GetRegistryDebugLevel((string) null) >= 4)
                {
                  AssemblyResolver.Instance.WriteToEventLog("Getting deployment manifest with response: " + fileText, index < num ? EventLogEntryType.Warning : EventLogEntryType.Error);
                  AssemblyResolver.Instance.WriteToEventLog("Getting deployment manifest with error: " + ex.Message, index < num ? EventLogEntryType.Warning : EventLogEntryType.Error);
                }
              }
              catch
              {
              }
label_10:
              if (index >= num)
                throw ex;
              try
              {
                string httpEquivRefresh = this.parseHttpEquivRefresh(fileText);
                string str = httpEquivRefresh != null ? DeploymentHandlerBase.getFileText(httpEquivRefresh, 51200L, 0L) : throw ex;
                AssemblyResolver.Instance.WriteToEventLog("Getting response from " + httpEquivRefresh + ":\r\n" + str, EventLogEntryType.Information);
              }
              catch
              {
              }
            }
          }
        }
        return this.deployManifest;
      }
      set => throw new Exception("Cannot change source deployment manifest");
    }

    internal bool AppManifestLoadedFromCache => this.appManifestLoadedFromCache;

    internal override ApplicationManifest AppManifest
    {
      get
      {
        if (this.appManifest == null)
        {
          if (this.deployHandler == null)
          {
            int num1 = (int) AssemblyResolver.DisplayOrLogMessage(EventLogEntryType.Error, "SourceDeploymentHandler.AppManifest: null deployHandler", "Encompass");
          }
          else if (this.deployHandler.DeployManifest == null)
          {
            int num2 = (int) AssemblyResolver.DisplayOrLogMessage(EventLogEntryType.Error, "SourceDeploymentHandler.AppManifest: null deployHandler.DeployManifest", "Encompass");
          }
          else if (this.deployHandler.DeployManifest.AsmIdVersion == (Version) null)
          {
            int num3 = (int) AssemblyResolver.DisplayOrLogMessage(EventLogEntryType.Error, "SourceDeploymentHandler.AppManifest: null deployHandler.DeployManifest.AsmIdVersion", "Encompass");
          }
          else if (this.DeployManifest == null)
          {
            int num4 = (int) AssemblyResolver.DisplayOrLogMessage(EventLogEntryType.Error, "SourceDeploymentHandler.AppManifest: null DeployManifest", "Encompass");
          }
          Version asmIdVersion = this.deployHandler.DeployManifest.AsmIdVersion;
          string str = (string) null;
          try
          {
            str = Path.Combine(DeployUtils.ConstructUacHashFolder(this.deployHandler.AppCompanyName, AssemblyResolver.AppStartupPath), this.appSuiteName + "\\" + this.executableName + ResolverConsts.AppManifestExt);
          }
          catch (Exception ex)
          {
            int num5 = (int) AssemblyResolver.DisplayOrLogMessage(EventLogEntryType.Error, "SourceDeploymentHandler.AppManifest: error getting uac app manifest file path: " + ex.Message, "Encompass");
          }
          if (File.Exists(str))
          {
            this.appManifest = new ApplicationManifest(SystemUtils.MutexFileReadAllText(str));
            if (asmIdVersion == this.appManifest.AsmIdVersion)
            {
              this.appManifestLoadedFromCache = true;
              return this.appManifest;
            }
            this.appManifest = (ApplicationManifest) null;
          }
          this.appManifestLoadedFromCache = false;
          AsmFileInfo asmFileInfo = this.DeployManifest.GetAsmFileInfo(this.executableName);
          if (asmFileInfo == null)
          {
            int num6 = (int) AssemblyResolver.DisplayOrLogMessage(EventLogEntryType.Error, "SourceDeploymentHandler.AppManifest: null asmFileInfo", "Encompass");
          }
          long size = asmFileInfo.Size;
          this.appManifest = new ApplicationManifest(DeploymentHandlerBase.getFileText(this.AppFolderUrl + this.UrlSeparator + this.executableName + ResolverConsts.AppManifestExt, size, size));
        }
        return this.appManifest;
      }
      set => throw new Exception("Cannot change source application manifest");
    }
  }
}
