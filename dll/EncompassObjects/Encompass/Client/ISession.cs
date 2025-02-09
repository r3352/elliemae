// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.ISession
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Users;
using EllieMae.Encompass.Configuration;
using EllieMae.Encompass.Reporting;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Client
{
  [Guid("FB7A5828-1517-4f63-BD98-7A72749CD35B")]
  public interface ISession
  {
    void Start(string serverUri, string loginId, string password);

    void StartOffline(string loginId, string password);

    void End();

    EllieMae.Encompass.BusinessObjects.Loans.Loans Loans { get; }

    EllieMae.Encompass.BusinessObjects.Contacts.Contacts Contacts { get; }

    EllieMae.Encompass.BusinessObjects.Users.Users Users { get; }

    string UserID { get; }

    string ID { get; }

    bool IsConnected { get; }

    EllieMae.Encompass.BusinessObjects.Calendar.Calendar Calendar { get; }

    Organizations Organizations { get; }

    EncompassEdition EncompassEdition { get; }

    string ClientID { get; }

    string SystemID { get; }

    ServerEvents ServerEvents { get; }

    string ServerID { get; }

    User GetCurrentUser();

    DataExchange DataExchange { get; }

    Reports Reports { get; }

    string ServerURI { get; }

    SystemSettings SystemSettings { get; }

    DateTime GetServerTime();
  }
}
