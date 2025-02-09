// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.IMainScreen
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Calendar;
using System.ComponentModel;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public interface IMainScreen : IWin32Window, IApplicationWindow, ISynchronizeInvoke
  {
    void ShowHelp(Control control);

    void ShowHelp(Control control, string helpTargetName);

    void ShowLeadCenter();

    void ShowCalendar(
      IWin32Window owner,
      string userID,
      CSMessage.AccessLevel accessLevel,
      bool accessUpdate);

    bool AllowCalendarSharing();

    void SwitchToOrgUserSetup(string userid);

    void SwitchToExternalOrgUserSetup(string userid);

    void NavigateHome(string url);

    void NavigateToContact(CategoryType contactType);

    void NavigateToContact(ContactInfo selectedContact);

    void AddNewBorrowerToContactManagerList(int contactID);

    void HandleCEMessage(CEMessage message);

    void RefreshCE();

    bool IsClientEnabledToExportFNMFRE { get; }

    bool IsUnderwriterSummaryAccessibleForBroker { get; }

    void DisplayTPOCompanySetting(ExternalOriginatorManagementData o);

    void NavigateToTradesTab(int tradeId);
  }
}
