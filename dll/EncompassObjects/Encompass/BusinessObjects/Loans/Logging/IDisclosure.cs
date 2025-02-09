// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.IDisclosure
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  [Guid("A5284F0B-EE92-41ab-8EBE-62FDB22DBA7E")]
  public interface IDisclosure
  {
    string ID { get; }

    DateTime Date { get; }

    LogEntryType EntryType { get; }

    string Comments { get; set; }

    LogAlerts RoleAlerts { get; }

    bool IsAlert { get; }

    bool EnabledForCompliance { get; set; }

    StandardDisclosureType DisclosureType { get; }

    string DisclosedBy { get; }

    DateTime DateAdded { get; }

    DeliveryMethod DeliveryMethod { get; }

    object ReceivedDate { get; set; }

    DisclosureFields Fields { get; }

    DisclosedDocuments Documents { get; }
  }
}
