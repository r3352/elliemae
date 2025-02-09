// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Automation.PipelineScreen
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;

#nullable disable
namespace EllieMae.Encompass.Automation
{
  /// <summary>Represents the Pipeline screen within Encompass.</summary>
  public class PipelineScreen : Screen
  {
    internal PipelineScreen()
      : base(EncompassScreen.Pipeline)
    {
    }

    /// <summary>Refreshes the current pipeline view.</summary>
    public void Refresh() => Session.Application.GetService<IPipeline>().RefreshPipeline();
  }
}
