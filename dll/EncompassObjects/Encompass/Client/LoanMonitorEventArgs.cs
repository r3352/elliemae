// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.LoanMonitorEventArgs
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer.Events;
using EllieMae.Encompass.BusinessObjects.Loans;
using System;

#nullable disable
namespace EllieMae.Encompass.Client
{
  public class LoanMonitorEventArgs : EventArgs, ILoanMonitorEventArgs
  {
    private LoanEvent evnt;
    private LoanIdentity loanId;
    private SessionInformation sessionInfo;

    internal LoanMonitorEventArgs(LoanEvent evnt)
    {
      this.evnt = evnt;
      this.loanId = new LoanIdentity(this.evnt.LoanIdentity);
      this.sessionInfo = new SessionInformation(((SessionMonitorEvent) this.evnt).Session);
    }

    public LoanMonitorEventType EventType => (LoanMonitorEventType) this.evnt.EventType;

    public SessionInformation SessionInformation => this.sessionInfo;

    public LoanIdentity LoanIdentity => this.loanId;
  }
}
