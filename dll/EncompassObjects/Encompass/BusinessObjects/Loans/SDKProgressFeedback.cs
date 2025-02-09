// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.SDKProgressFeedback
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using System;
using System.Runtime.Remoting;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  internal class SDKProgressFeedback : MarshalByRefObject, IServerProgressFeedback, IDisposable
  {
    private int currentIdx;
    private string[] currentResult;
    private ReassignResult[] results;

    public SDKProgressFeedback(int loanCount) => this.results = new ReassignResult[loanCount];

    public ReassignResult[] Results => this.results;

    public string CurrentGUID { get; set; }

    public string CurrentLoanNumber { get; set; }

    public int MaxValue
    {
      get => -1;
      set
      {
      }
    }

    public bool Cancel
    {
      get => false;
      set
      {
      }
    }

    public string Status
    {
      get => string.Empty;
      set
      {
        this.results[this.currentIdx].Succeeded = value.Equals("Sucsess") || value.Equals("Success");
      }
    }

    public string Details
    {
      get => string.Empty;
      set
      {
        this.currentResult = value.Split('_');
        this.currentIdx = int.Parse(this.currentResult[0]);
        this.results[this.currentIdx] = new ReassignResult()
        {
          GUID = this.CurrentGUID,
          LoanNumber = this.CurrentLoanNumber,
          Message = this.currentResult[1]
        };
      }
    }

    public bool Increment(int count) => true;

    public int Value
    {
      get => -1;
      set
      {
      }
    }

    public bool ResetCounter(int maxValue) => true;

    public bool SetFeedback(string status, string details, int value) => true;

    public void Dispose()
    {
      try
      {
        RemotingServices.Disconnect((MarshalByRefObject) this);
      }
      catch
      {
      }
    }
  }
}
