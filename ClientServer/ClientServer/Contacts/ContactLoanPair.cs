// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Contacts.ContactLoanPair
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.DataEngine.Log;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Contacts
{
  [Serializable]
  public class ContactLoanPair
  {
    public readonly string LoanGuid;
    public readonly string ContactGuid;
    public readonly int ContactID;
    public readonly ContactType ContactType;
    public readonly CRMRoleType RoleType;
    public readonly int BorrowerPair;

    public ContactLoanPair(
      string loanGuid,
      string contactGuid,
      int contactId,
      ContactType contactType,
      CRMRoleType roleType,
      int borrowerPair)
    {
      this.LoanGuid = loanGuid;
      this.ContactGuid = contactGuid;
      this.ContactID = contactId;
      this.ContactType = contactType;
      this.RoleType = roleType;
      this.BorrowerPair = borrowerPair;
    }
  }
}
