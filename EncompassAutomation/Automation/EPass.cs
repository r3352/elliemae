// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Automation.EPass
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using System.Web;

#nullable disable
namespace EllieMae.Encompass.Automation
{
  /// <summary>Provides access to the ePASS Services display.</summary>
  public class EPass
  {
    internal EPass()
    {
    }

    /// <summary>
    /// Displays the available ePASS providers for the specified service type.
    /// </summary>
    /// <param name="serviceType">The service type to be displayed.</param>
    /// <remarks>The service type specified should match one of the service types
    /// listed in the "Services" pane of the Loan Editor, e.g. "Credit Report".</remarks>
    public void DisplayServices(string serviceType)
    {
      Session.Application.GetService<IEPass>().ProcessURL("_EPASS_SIGNATURE;EPASSAI;2;" + HttpUtility.UrlEncode(serviceType));
    }
  }
}
