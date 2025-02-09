// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Automation.EPass
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using System.Web;

#nullable disable
namespace EllieMae.Encompass.Automation
{
  public class EPass
  {
    internal EPass()
    {
    }

    public void DisplayServices(string serviceType)
    {
      Session.Application.GetService<IEPass>().ProcessURL("_EPASS_SIGNATURE;EPASSAI;2;" + HttpUtility.UrlEncode(serviceType));
    }
  }
}
