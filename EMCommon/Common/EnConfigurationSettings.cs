// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.EnConfigurationSettings
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class EnConfigurationSettings
  {
    public const string AppSessionIDCommandLineParameter = "-sid";
    public const string AppLogSIDCommandLineFlag = "-logsid";
    public const string AppDebugCommandLineFlag = "-debug";
    public const string AppGCTraceCommandLineFlag = "-gctrace";
    private static string instanceName = "";
    private static EnAppSettings appSettings = (EnAppSettings) null;
    private static EnGlobalSettings globalSettings = (EnGlobalSettings) null;
    private static string appSessionID = (string) null;
    private static string[] appArguments = (string[]) null;

    private EnConfigurationSettings()
    {
    }

    static EnConfigurationSettings()
    {
      EnConfigurationSettings.appSessionID = Guid.NewGuid().ToString("N");
      string configurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
      EnConfigurationSettings.appSettings = !((configurationFile ?? "") != "") ? new EnAppSettings() : new EnAppSettings(configurationFile);
      EnConfigurationSettings.InstanceName = EnConfigurationSettings.appSettings["instance"] ?? "";
    }

    public static string InstanceName
    {
      get => EnConfigurationSettings.instanceName;
      set
      {
        EnConfigurationSettings.globalSettings = new EnGlobalSettings(value);
        EnConfigurationSettings.instanceName = value;
      }
    }

    public static EnAppSettings AppSettings => EnConfigurationSettings.appSettings;

    public static EnGlobalSettings GlobalSettings => EnConfigurationSettings.globalSettings;

    public static string ApplicationSessionID => EnConfigurationSettings.appSessionID;

    public static string GetApplicationPath()
    {
      string[] commandLineArgs = Environment.GetCommandLineArgs();
      return commandLineArgs.Length != 0 ? commandLineArgs[0] : throw new InvalidOperationException("The command line args are not available in this context");
    }

    public static string[] GetApplicationArguments()
    {
      return EnConfigurationSettings.appArguments == null ? Environment.GetCommandLineArgs() : (string[]) EnConfigurationSettings.appArguments.Clone();
    }

    public static bool ApplicationArgumentExists(string argName)
    {
      foreach (string applicationArgument in EnConfigurationSettings.GetApplicationArguments())
      {
        if (string.Compare(applicationArgument, argName, true) == 0)
          return true;
      }
      return false;
    }

    public static string GetApplicationArgumentValue(string argName)
    {
      string[] applicationArguments = EnConfigurationSettings.GetApplicationArguments();
      for (int index = 0; index < applicationArguments.Length - 1; ++index)
      {
        if (string.Compare(applicationArguments[index], argName, true) == 0)
          return applicationArguments[index + 1];
      }
      return (string) null;
    }

    public static void ApplyInstanceFromCommandLine(string[] args)
    {
      EnConfigurationSettings.appArguments = args;
      for (int index = 0; index < args.Length - 1; ++index)
      {
        if (args[index] == "-i")
          EnConfigurationSettings.InstanceName = args[index + 1];
        else if (string.Compare(args[index], "-sid", true) == 0)
          EnConfigurationSettings.appSessionID = args[index + 1];
      }
    }
  }
}
