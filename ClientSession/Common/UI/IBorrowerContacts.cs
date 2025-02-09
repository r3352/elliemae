// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.IBorrowerContacts
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public interface IBorrowerContacts
  {
    void ShowContacts(
      QueryCriterion filter,
      RelatedLoanMatchType matchType,
      SortField[] sortOrder,
      string description);

    void ShowContacts(FieldFilterList filter, SortField[] sortOrder);

    void Refresh();

    void CreateNew();

    void ShowLeadCenter();

    void ShowLeadMailbox();

    void ShowLeads();

    void NavigateCustomerLoyalty(string url);
  }
}
