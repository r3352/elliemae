// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ConcurrentUpdateNotification
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class ConcurrentUpdateNotification : UserNotification
  {
    public ConcurrentUpdateNotification(
      string userid,
      string loanGuid,
      string correlationId,
      DateTime notificationDate)
      : base(userid, notificationDate)
    {
      this.UserId = userid;
      this.LoanGuid = loanGuid;
    }

    public string UserId { get; private set; }

    public string LoanGuid { get; private set; }

    public string CorrelationId { get; private set; }
  }
}
