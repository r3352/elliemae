// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.Schema.RemotingClientLog
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using System.Runtime.Serialization;

#nullable disable
namespace Encompass.Diagnostics.Logging.Schema
{
  public class RemotingClientLog : Log
  {
    public RemotingClientLog(Log log)
      : base(log)
    {
    }

    protected RemotingClientLog(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    internal override bool SkipPreparationBeforeWrite => true;

    public override string GetLogMessage()
    {
      string logMessage = base.GetLogMessage();
      if (this.Error != null)
        logMessage = logMessage + " Ex: " + this.Error.ToString();
      return logMessage;
    }

    public override string GetSourceName()
    {
      return "Client message - " + this.InstanceIdOrDefault() + "|" + this.CustomerId + "|" + this.UserId + "|" + this.SessionId;
    }
  }
}
