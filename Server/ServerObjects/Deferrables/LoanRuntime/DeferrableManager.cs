// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Deferrables.LoanRuntime.DeferrableManager
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.ElliEnum;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using System.Configuration;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Deferrables.LoanRuntime
{
  public class DeferrableManager
  {
    public static DeferrableType GetDeferrableType(DeferrableProcessorRole role)
    {
      if (role == DeferrableProcessorRole.Subscriber || role == DeferrableProcessorRole.RelaySubscriber)
        return DeferrableType.RealTime;
      ClientContext current = ClientContext.GetCurrent();
      if (current == null)
        return DeferrableType.RealTime;
      DeferrableLoanProcessMode serverSetting = (DeferrableLoanProcessMode) current.Settings.GetServerSetting("Policies.DeferrableLoanProcessMode");
      switch (serverSetting)
      {
        case DeferrableLoanProcessMode.Deferred:
          return !DeferrableManager.IsEnvironmentDeferrableFriendly(DeferrableType.Deferred) ? DeferrableType.RealTime : DeferrableType.Deferred;
        case DeferrableLoanProcessMode.RealTime:
          return DeferrableType.RealTime;
        default:
          return EncompassServer.ServerMode == EncompassServerMode.Service ? (serverSetting == DeferrableLoanProcessMode.EncompassPlatformDeferred && DeferrableManager.IsEnvironmentDeferrableFriendly(DeferrableType.Deferred) ? DeferrableType.Deferred : DeferrableType.RealTime) : (serverSetting == DeferrableLoanProcessMode.EncompassDeferred && DeferrableManager.IsEnvironmentDeferrableFriendly(DeferrableType.Deferred) ? DeferrableType.Deferred : DeferrableType.RealTime);
      }
    }

    public static bool IsEmailTriggerDeferredEnabled(IClientContext context)
    {
      bool result = false;
      bool.TryParse(context.Settings.GetServerSetting("Policies.DisableDeferredEmailTrigger", false) as string, out result);
      return (DeferrableManager.IsActivityDeferrableAllowed(4) || DeferrableManager.IsActivityDeferrableAllowed((int) ushort.MaxValue)) && !result;
    }

    public static bool IsActivityDeferrableAllowed(int activityFlag)
    {
      ClientContext current = ClientContext.GetCurrent();
      string path = ConfigurationManager.AppSettings["DeferrableActivities.PolicyName"];
      if (string.IsNullOrEmpty(path))
        path = EncompassServer.ServerMode != EncompassServerMode.Service ? "Policies.DeferrableENCAllowedActivities" : "Policies.DeferrableEBSAllowedActivities";
      return ((int) current.Settings.GetServerSetting(path) & activityFlag) > 0;
    }

    public static bool IsEnvironmentDeferrableFriendly(DeferrableType deferrableType)
    {
      if (deferrableType == DeferrableType.RealTime)
        return true;
      switch ((StorageMode) ClientContext.GetCurrent().Settings.GetStorageSetting("DataStore.StorageMode"))
      {
        case StorageMode.DatabaseOnly:
        case StorageMode.MongoOnly:
          return false;
        default:
          return true;
      }
    }
  }
}
