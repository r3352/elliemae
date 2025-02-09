// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Automation.PipelineScreen
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;

#nullable disable
namespace EllieMae.Encompass.Automation
{
  public class PipelineScreen : Screen
  {
    internal PipelineScreen()
      : base(EncompassScreen.Pipeline)
    {
    }

    public void Refresh() => Session.Application.GetService<IPipeline>().RefreshPipeline();
  }
}
