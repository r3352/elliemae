// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Automation.BusinessContactsScreen
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.BusinessObjects.Contacts;
using EllieMae.Encompass.Query;

#nullable disable
namespace EllieMae.Encompass.Automation
{
  /// <summary>
  /// Represents the Business Contacts screen within the Encompass application.
  /// </summary>
  public class BusinessContactsScreen : Screen
  {
    internal BusinessContactsScreen()
      : base(EncompassScreen.BusinessContacts)
    {
    }

    /// <summary>
    /// Applies a filter to the user list on the Borrower Contacts screen.
    /// </summary>
    /// <param name="cri">The criterion for the filter.</param>
    /// <param name="matchType">The match type for the filter</param>
    /// <param name="sortOrder">The column to sort by when the results are displayed in the ListView.</param>
    /// <param name="description">A description for this query to be displayed in the UI.</param>
    public void ApplyFilter(
      EllieMae.Encompass.Query.QueryCriterion cri,
      ContactLoanMatchType matchType,
      SortCriterion sortOrder,
      string description)
    {
      Session.Application.GetService<IBizContacts>().ShowContacts(cri.Unwrap(), (RelatedLoanMatchType) matchType, new SortField[1]
      {
        sortOrder.Unwrap()
      }, description);
    }
  }
}
