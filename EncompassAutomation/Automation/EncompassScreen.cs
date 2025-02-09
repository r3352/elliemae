// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Automation.EncompassScreen
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

#nullable disable
namespace EllieMae.Encompass.Automation
{
  /// <summary>
  /// Enumeration defining the top-level screens available in Encompass.
  /// </summary>
  public enum EncompassScreen
  {
    /// <summary>Unspecified screen</summary>
    Unknown = 0,
    /// <summary>The loan Pipeline screen</summary>
    Pipeline = 1,
    /// <summary>The News screen</summary>
    News = 2,
    /// <summary>The Dashboard screen</summary>
    Dashboard = 3,
    /// <summary>The Reports screen</summary>
    Reports = 4,
    /// <summary>The Loan Editor screen</summary>
    Loans = 10, // 0x0000000A
    /// <summary>The ePASS screen</summary>
    ePASS = 11, // 0x0000000B
    /// <summary>The Borrower Contacts screen</summary>
    BorrowerContacts = 20, // 0x00000014
    /// <summary>The Business Contacts screen</summary>
    BusinessContacts = 21, // 0x00000015
    /// <summary>The Calendar screen</summary>
    Calendar = 22, // 0x00000016
    /// <summary>The Task List screen</summary>
    Tasks = 24, // 0x00000018
    /// <summary>The Campaign Management screen</summary>
    Campaigns = 25, // 0x00000019
    /// <summary>The Trade Management screen</summary>
    Trades = 26, // 0x0000001A
  }
}
