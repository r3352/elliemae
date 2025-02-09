// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AsyncTaskManager.CampaignRefreshTask
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.AsyncTaskManager
{
  public class CampaignRefreshTask : AsyncTaskBase
  {
    private const string className = "CampaignRefreshTask";
    protected static string sw = Tracing.SwCommon;
    private int[] campaignIds = new int[0];

    public event CampaignRefreshStartedEventHandler CampaignRefreshStartedEvent;

    public event CampaignRefreshCompletedEventHandler CampaignRefreshCompletedEvent;

    public CampaignRefreshTask() => this.SupportsProgress = true;

    protected override void DoTask()
    {
      try
      {
        this.campaignIds = Session.CampaignManager.GetCampaignQueryListForUser(Session.UserID);
        if (this.campaignIds == null)
          return;
        for (int index = 0; index < this.campaignIds.Length; ++index)
        {
          Session.CampaignManager.RunCampaignQueries(this.campaignIds[index]);
          this.progress = index / this.campaignIds.Length;
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(CampaignRefreshTask.sw, TraceLevel.Error, nameof (CampaignRefreshTask), "Error: " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace);
      }
    }

    protected override void OnStarted()
    {
      if (this.CampaignRefreshStartedEvent == null)
        return;
      this.CampaignRefreshStartedEvent((object) this, EventArgs.Empty);
    }

    protected override void OnCompleted()
    {
      if (this.CampaignRefreshCompletedEvent == null)
        return;
      this.CampaignRefreshCompletedEvent((object) this, EventArgs.Empty);
    }
  }
}
