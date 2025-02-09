// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.PipelineData
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public class PipelineData : SessionBoundObject, IPipelineData
  {
    private PipelineInfo pinfo;
    private LoanIdentity loanId;
    private PipelineAlerts alerts;
    private LoanLock lockInfo;

    internal PipelineData(Session session, PipelineInfo pinfo)
      : base(session)
    {
      this.pinfo = pinfo;
      this.loanId = new LoanIdentity(pinfo.Identity);
    }

    public object this[string fieldName] => this.pinfo.Info[(object) fieldName];

    public StringList GetFieldNames()
    {
      return new StringList((IList) new ArrayList(this.pinfo.Info.Keys));
    }

    public LoanIdentity LoanIdentity => this.loanId;

    public PipelineAlerts Alerts
    {
      get
      {
        if (this.alerts == null)
          this.alerts = new PipelineAlerts(this.Session, this.pinfo);
        return this.alerts;
      }
    }

    public LoanLock CurrentLock
    {
      get
      {
        if (this.pinfo.LockInfo == null || !this.pinfo.LockInfo.IsLocked)
          return (LoanLock) null;
        return this.lockInfo == null ? (this.lockInfo = new LoanLock(this.pinfo.LockInfo)) : this.lockInfo;
      }
    }

    public LoanAccessRights GetAccessRights()
    {
      ILoan iloan = ((ILoanManager) this.Session.Unwrap().GetObject("LoanManager")).OpenLoan(this.pinfo.GUID);
      if (iloan == null)
        return LoanAccessRights.None;
      try
      {
        LoanInfo.Right rights = iloan.GetRights();
        switch ((int) rights)
        {
          case 0:
            return LoanAccessRights.None;
          case 1:
            return LoanAccessRights.ReadWrite;
          case 2:
            throw new Exception("Invalid value returned for loan rights");
          case 3:
            return LoanAccessRights.Full;
          default:
            if (rights == 8)
              return LoanAccessRights.ReadOnly;
            goto case 2;
        }
      }
      finally
      {
        iloan.Close();
      }
    }

    internal static PipelineDataList ToList(Session session, PipelineInfo[] infos)
    {
      PipelineDataList list = new PipelineDataList();
      for (int index = 0; index < infos.Length; ++index)
        list.Add(new PipelineData(session, infos[index]));
      return list;
    }
  }
}
