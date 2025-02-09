// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.PipelineData
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>Represents summary information for a single Loan.</summary>
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

    /// <summary>Retrieves the data for a specified summary field.</summary>
    /// <remarks>
    /// The set of summary field available to the user correponds to fields which
    /// can be used to query for a loan. However, when specifying the field name, the
    /// leading portion of the canonical name should be omitted.
    /// <p>For example, the canonical name for the field containing the borrower's
    /// last name is "Loan.BorrowerLastName". To access the last name in the
    /// PipelineData class, simply use the field name "BorrowerLastName".</p>
    /// </remarks>
    public object this[string fieldName] => this.pinfo.Info[(object) fieldName];

    /// <summary>
    /// Returns the names of the fields contained in the PipelineData.
    /// </summary>
    /// <returns>Returns a StringList containing the names of the fields in the PipelineData
    /// object. These field names can be used to retrieve field values from the data object.</returns>
    public StringList GetFieldNames()
    {
      return new StringList((IList) new ArrayList(this.pinfo.Info.Keys));
    }

    /// <summary>
    /// Gets the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.PipelineData.LoanIdentity" /> for the current loan.
    /// </summary>
    public LoanIdentity LoanIdentity => this.loanId;

    /// <summary>
    /// Gets the collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.PipelineAlert" /> objects for this
    /// loan.
    /// </summary>
    public PipelineAlerts Alerts
    {
      get
      {
        if (this.alerts == null)
          this.alerts = new PipelineAlerts(this.Session, this.pinfo);
        return this.alerts;
      }
    }

    /// <summary>
    /// Returns the information for the current lock held on the loan.
    /// </summary>
    /// <remarks>If no lock is currently held, this property will return <c>null</c>.</remarks>
    public LoanLock CurrentLock
    {
      get
      {
        if (this.pinfo.LockInfo == null || !this.pinfo.LockInfo.IsLocked)
          return (LoanLock) null;
        return this.lockInfo == null ? (this.lockInfo = new LoanLock(this.pinfo.LockInfo)) : this.lockInfo;
      }
    }

    /// <summary>
    /// Returns the current user's effective access rights to this loan.
    /// </summary>
    /// <returns>The rights to this loan.</returns>
    /// <remarks>This method requires a round trip to the Encompass Server and
    /// should be considered expensive to invoke.</remarks>
    public LoanAccessRights GetAccessRights()
    {
      EllieMae.EMLite.ClientServer.ILoan loan = ((ILoanManager) this.Session.Unwrap().GetObject("LoanManager")).OpenLoan(this.pinfo.GUID);
      if (loan == null)
        return LoanAccessRights.None;
      try
      {
        switch (loan.GetRights())
        {
          case LoanInfo.Right.NoRight:
            return LoanAccessRights.None;
          case LoanInfo.Right.Access:
            return LoanAccessRights.ReadWrite;
          case LoanInfo.Right.FullRight:
            return LoanAccessRights.Full;
          case LoanInfo.Right.Read:
            return LoanAccessRights.ReadOnly;
          default:
            throw new Exception("Invalid value returned for loan rights");
        }
      }
      finally
      {
        loan.Close();
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
