// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.IContacts
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessEnums;
using EllieMae.Encompass.Collections;
using EllieMae.Encompass.Query;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  [Guid("41D56AD9-735D-4b88-BDDE-D57B26A758F5")]
  public interface IContacts
  {
    Contact Open(int contactId, ContactType type);

    Contact CreateNew(ContactType type);

    void Delete(int contactId, ContactType type);

    ContactList Query(
      QueryCriterion criterion,
      ContactLoanMatchType loanMatchType,
      ContactType type);

    ContactList GetAll(ContactType type);

    BizCategories BizCategories { get; }

    ContactCursor OpenCursor(SortCriterionList sortCriteria, ContactType type);

    ContactCursor QueryCursor(
      QueryCriterion criterion,
      ContactLoanMatchType loanMatchType,
      SortCriterionList sortCriteria,
      ContactType type);
  }
}
