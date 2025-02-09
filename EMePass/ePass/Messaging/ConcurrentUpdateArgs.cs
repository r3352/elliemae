// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.Messaging.ConcurrentUpdateArgs
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ePass.Messaging
{
  public class ConcurrentUpdateArgs : EventArgs
  {
    public ConcurrentUpdateArgs(string loanGuid, DateTime timestamp, string correlationId)
    {
      this.LoanGuid = loanGuid;
      this.Timestamp = timestamp;
      this.CorrelationId = correlationId;
    }

    public string LoanGuid { get; private set; }

    public DateTime Timestamp { get; private set; }

    public string CorrelationId { get; private set; }
  }
}
