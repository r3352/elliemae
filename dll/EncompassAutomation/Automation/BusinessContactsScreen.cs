// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Automation.BusinessContactsScreen
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.BusinessObjects.Contacts;
using EllieMae.Encompass.Query;

#nullable disable
namespace EllieMae.Encompass.Automation
{
  public class BusinessContactsScreen : Screen
  {
    internal BusinessContactsScreen()
      : base(EncompassScreen.BusinessContacts)
    {
    }

    public void ApplyFilter(
      QueryCriterion cri,
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
