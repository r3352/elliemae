// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerDiagConfigData
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Logging;
using Encompass.Diagnostics.Config;
using Encompass.Diagnostics.Logging;
using Encompass.Diagnostics.Logging.Formatters;
using Encompass.Diagnostics.Logging.Targets;
using System.Collections.Generic;
using System.Configuration;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class ServerDiagConfigData : ConfigDataSection
  {
    public ServerDiagConfigData.LogListenersConfigData LogListeners { get; } = ConfigDataSection.NewInstance<ServerDiagConfigData.LogListenersConfigData>();

    public class ClassicLogConfigData : ConfigDataSection
    {
      public virtual bool Enabled { get; set; } = true;

      public string Name { get; } = "ClassicLogListener";

      public virtual Dictionary<string, LogLevelFilter> LogLevels { get; set; } = Tracing.GetLogLevels("*");

      public virtual string DefaultLogLevelName { get; set; } = "*";

      public ServerDiagConfigData.ClassicLogConfigData.SingleFileTargetConfigData SingleFileTarget { get; } = ConfigDataSection.NewInstance<ServerDiagConfigData.ClassicLogConfigData.SingleFileTargetConfigData>();

      public ServerDiagConfigData.ClassicLogConfigData.MultiFileTargetConfigData MultiFileTarget { get; } = ConfigDataSection.NewInstance<ServerDiagConfigData.ClassicLogConfigData.MultiFileTargetConfigData>();

      public ServerDiagConfigData.ClassicLogConfigData.EventTargetConfigData EventTarget { get; } = ConfigDataSection.NewInstance<ServerDiagConfigData.ClassicLogConfigData.EventTargetConfigData>();

      private static LogFormat GetJsonLogFormat() => LogFormat.LegacyJson;

      private static string GetRootFileName()
      {
        string rootFileName = ConfigurationManager.AppSettings["AppNameForLog"];
        if (string.IsNullOrWhiteSpace(rootFileName))
          rootFileName = "EncompassServer";
        return rootFileName;
      }

      public class MultiFileTargetConfigData : ConfigDataSection
      {
        public string Name { get; } = "MultiFileTarget";

        public virtual bool Enabled { get; set; } = EnConfigurationSettings.GlobalSettings.ServerLogMode == ServerLogMode.LogPerInstance;

        public virtual string RootFileName { get; set; } = ServerDiagConfigData.ClassicLogConfigData.GetRootFileName();

        public virtual bool AppendServerNameToLogFileName { get; set; } = true;

        public virtual bool AppendRolloverTimestampToActiveLog { get; set; } = true;

        public virtual FileLogRolloverFrequency RolloverFrequency { get; set; } = EnConfigurationSettings.GlobalSettings.ServerLogRolloverFrequency;

        public virtual LogFormat Format { get; set; }
      }

      public class SingleFileTargetConfigData : ConfigDataSection
      {
        public string Name { get; } = "SingleFileTarget";

        public virtual bool Enabled { get; set; } = EnConfigurationSettings.GlobalSettings.ServerLogMode == ServerLogMode.SingleFile;

        public virtual string BaseDir { get; set; } = EnConfigurationSettings.GlobalSettings.AppBaseLogDirectory;

        public virtual string RootFileName { get; set; } = ServerDiagConfigData.ClassicLogConfigData.GetRootFileName();

        public virtual bool AppendServerNameToLogFileName { get; set; } = true;

        public virtual bool AppendRolloverTimestampToActiveLog { get; set; } = true;

        public virtual FileLogRolloverFrequency RolloverFrequency { get; set; } = EnConfigurationSettings.GlobalSettings.ServerLogRolloverFrequency;

        public virtual LogFormat Format { get; set; } = ServerDiagConfigData.ClassicLogConfigData.GetJsonLogFormat();
      }

      public class EventTargetConfigData : ConfigDataSection
      {
        public string Name { get; } = "EventTarget";

        public virtual bool Enabled { get; set; } = EncompassServerMode.Service == EncompassServer.ServerMode;

        public virtual LogFormat Format { get; set; } = ServerDiagConfigData.ClassicLogConfigData.GetJsonLogFormat();
      }
    }

    public class LogListenersConfigData : ConfigDataSection
    {
      public ServerDiagConfigData.ClassicLogConfigData ClassicLog { get; } = ConfigDataSection.NewInstance<ServerDiagConfigData.ClassicLogConfigData>();
    }
  }
}
