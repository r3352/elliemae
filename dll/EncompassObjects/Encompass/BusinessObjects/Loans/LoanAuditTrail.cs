// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LoanAuditTrail
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.Encompass.Collections;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public class LoanAuditTrail : ILoanAuditTrail
  {
    private Loan loan;

    internal LoanAuditTrail(Loan loan) => this.loan = loan;

    public AuditTrailEntryList GetHistory(string fieldId)
    {
      try
      {
        return AuditTrailEntry.ToList(this.loan.Session, this.loan.Session.SessionObjects.LoanManager.GetAuditRecords(this.loan.Guid, fieldId));
      }
      catch (ObjectNotFoundException ex)
      {
        throw new Exception("The field '" + fieldId + "' is not in the Audit Trail.");
      }
    }

    public AuditTrailEntryList GetHistory(string[] fieldIds)
    {
      try
      {
        return AuditTrailEntry.ToList(this.loan.Session, this.loan.Session.SessionObjects.LoanManager.GetAuditRecords(this.loan.Guid, fieldIds));
      }
      catch (ObjectNotFoundException ex)
      {
        throw new Exception("The fields are not in the Audit Trail. Please check the input");
      }
    }

    public AuditTrailEntry GetMostRecentEntry(string fieldId) => this.GetHistory(fieldId)[0];

    public AuditTrailEntryList GetMostRecentEntries()
    {
      Dictionary<string, AuditRecord> auditRecordsForLoan = this.loan.Session.SessionObjects.LoanManager.GetLastAuditRecordsForLoan(this.loan.Guid);
      AuditRecord[] auditRecordArray = new AuditRecord[auditRecordsForLoan.Count];
      auditRecordsForLoan.Values.CopyTo(auditRecordArray, 0);
      return AuditTrailEntry.ToList(this.loan.Session, auditRecordArray);
    }

    public StringList GetAuditFieldList()
    {
      LoanXDBField[] trailLoanXdbField = this.loan.Session.SessionObjects.LoanManager.GetAuditTrailLoanXDBField();
      StringList auditFieldList = new StringList();
      foreach (LoanXDBField loanXdbField in trailLoanXdbField)
        auditFieldList.Add(loanXdbField.FieldIDWithCoMortgagor);
      return auditFieldList;
    }
  }
}
