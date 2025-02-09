// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerTasks.RenewServerLicenseTaskHandler
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Server.Tasks;
using System;

#nullable disable
namespace EllieMae.EMLite.Server.ServerTasks
{
  public class RenewServerLicenseTaskHandler : ITaskHandler
  {
    private static readonly string ClassName = "RenewServerLicenseTask";

    public void ProcessTask(ServerTask taskInfo)
    {
      ClientContext current = ClientContext.GetCurrent();
      try
      {
        LicenseManager.RefreshServerLicense((IClientContext) current);
        current.TraceLog.WriteInfo(RenewServerLicenseTaskHandler.ClassName, string.Format("Completed::RenewServerLicenseTask processing Client Instance {0}", (object) current.InstanceName));
      }
      catch (Exception ex)
      {
        current.TraceLog.WriteError(RenewServerLicenseTaskHandler.ClassName, string.Format("Exception while renewing license for the Instance - {0} : Exception - {1}", (object) current.InstanceName, (object) ex.Message));
      }
      try
      {
        bool isBillingModelEnabled;
        LicenseManager.RefreshBillingCalculation((IClientContext) current, out isBillingModelEnabled);
        current.TraceLog.WriteInfo(RenewServerLicenseTaskHandler.ClassName, string.Format("Completed::RefreshBillingCalculation processing Client Instance - {0} , billingModeEnabled - {1}", (object) current.InstanceName, (object) isBillingModelEnabled));
      }
      catch (Exception ex)
      {
        current.TraceLog.WriteError(RenewServerLicenseTaskHandler.ClassName, string.Format("Exception while renewing billing cost license for the Instance - {0} : Exception - {1}", (object) current.InstanceName, (object) ex.Message));
      }
    }
  }
}
