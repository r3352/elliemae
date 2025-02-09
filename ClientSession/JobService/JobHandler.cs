// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.JobService.JobHandler
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.Client;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;
using System.Diagnostics;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.JobService
{
  public abstract class JobHandler
  {
    private const string className = "JobHandler";
    private static readonly string sw = "JobEngine";
    private int jobId = -1;
    private object jobParams;
    private object jobState;

    public event StateChangedEventHandler StateChanged;

    public JobHandler() => Tracing.Init(Path.Combine(SystemSettings.LogDir, "JobProcessor"), true);

    public int JobID => this.jobId;

    protected object Parameters => this.jobParams;

    protected object State => this.jobState;

    public void Execute(int jobId, string jobType, string jobData, string jobState)
    {
      if ((jobState ?? "") == "")
        Tracing.Log(JobHandler.sw, nameof (JobHandler), TraceLevel.Info, "Job '" + (object) jobId + "' of type '" + jobType + "' started...");
      else
        Tracing.Log(JobHandler.sw, nameof (JobHandler), TraceLevel.Info, "Job '" + (object) jobId + "' of type '" + jobType + "' resumed...");
      try
      {
        this.jobId = jobId;
        this.jobParams = this.ParseParameters(jobData);
        this.jobState = this.ParseState(jobState);
        Tracing.Log(JobHandler.sw, nameof (JobHandler), TraceLevel.Verbose, "Job parameters parsed successfully");
        if (this.jobParams is ConnectedJobParameters jobParams)
        {
          Tracing.Log(JobHandler.sw, nameof (JobHandler), TraceLevel.Info, "Connecting to server '" + jobParams.ServerUri + "' as user '" + jobParams.UserID + "'");
          Connection newConnection = new Connection();
          newConnection.Open(jobParams.ServerUri, jobParams.UserID, jobParams.Password, "JobProcessor", false);
          Session.SetConnection((IConnection) newConnection, jobParams.Password);
          Session.HideUI = true;
          Tracing.Log(JobHandler.sw, nameof (JobHandler), TraceLevel.Verbose, "Connection made successfully");
        }
        try
        {
          this.ProcessJob();
        }
        finally
        {
          Session.End();
        }
        Tracing.Log(JobHandler.sw, nameof (JobHandler), TraceLevel.Info, "Job '" + (object) jobId + "' completed successfully");
      }
      catch (Exception ex)
      {
        Tracing.Log(JobHandler.sw, nameof (JobHandler), TraceLevel.Error, "Job '" + (object) jobId + "' failed: " + (object) ex);
        throw;
      }
    }

    protected virtual object ParseState(string jobState)
    {
      return (jobState ?? "") == "" ? (object) new JobState() : new XmlSerializer().Deserialize(jobState, typeof (JobState));
    }

    protected virtual void NotifyStateChanged()
    {
      if (this.StateChanged == null)
        return;
      this.StateChanged(this.jobId, new XmlSerializer().Serialize(this.jobState));
    }

    protected abstract object ParseParameters(string jobData);

    protected abstract void ProcessJob();
  }
}
