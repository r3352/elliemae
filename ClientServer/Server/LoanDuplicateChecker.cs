// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.LoanDuplicateChecker
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.DataEngine;
using System;

#nullable disable
namespace EllieMae.EMLite.Server
{
  [Serializable]
  public class LoanDuplicateChecker
  {
    private Guid guid;
    private string firstName;
    private string lastName;
    private string workPhone;
    private string homePhone;
    private string mobilePhone;
    private string email;
    private string workEmail;
    private string ssn;
    private Address subjectProperty;
    private LoanTypeEnum loantype;
    private DateTime dateOpened;
    private string fileStarter;
    private string loanOfficer;
    private string loanFolder;
    private long loanAmount;
    private LoanStatusMap.LoanStatus loanStatus;

    public LoanDuplicateChecker(
      Guid guid,
      string firstName,
      string lastName,
      string workPhone,
      string homePhone,
      string mobilePhone,
      string email,
      string workEmail,
      string ssn,
      Address subjectProperty,
      LoanTypeEnum loantype,
      DateTime dateopened,
      string fileStarter,
      string loanOfficer,
      string loanFolder,
      long loanAmount,
      LoanStatusMap.LoanStatus loanStatus)
    {
      this.guid = guid;
      this.firstName = firstName;
      this.lastName = lastName;
      this.workPhone = workPhone;
      this.homePhone = homePhone;
      this.mobilePhone = mobilePhone;
      this.email = email;
      this.workEmail = workEmail;
      this.ssn = ssn;
      this.subjectProperty = subjectProperty;
      this.loantype = loantype;
      this.dateOpened = dateopened;
      this.fileStarter = fileStarter;
      this.loanOfficer = loanOfficer;
      this.loanFolder = loanFolder;
      this.loanAmount = loanAmount;
      this.loanStatus = loanStatus;
    }

    public Guid GUID => this.guid;

    public string FirstName => this.firstName;

    public string LastName => this.lastName;

    public string WorkPhone => this.workPhone;

    public string HomePhone => this.homePhone;

    public string MobilePhone => this.mobilePhone;

    public string Email => this.email;

    public string WorkEmail => this.workEmail;

    public string SSN => this.ssn;

    public Address SubjectProperty => this.subjectProperty;

    public LoanTypeEnum Loantype => this.loantype;

    public DateTime DateOpened => this.dateOpened;

    public string FileStarter => this.fileStarter;

    public string LoanOfficer => this.loanOfficer;

    public string LoanFolder => this.loanFolder;

    public long LoanAmount => this.loanAmount;

    public LoanStatusMap.LoanStatus LoanStatus => this.loanStatus;

    public enum CheckField
    {
      FirstName,
      LastName,
      WorkPhone,
      HomePhone,
      MobilePhone,
      Email,
      WorkEmail,
      SSN,
    }
  }
}
