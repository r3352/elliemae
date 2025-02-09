// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.IHtmlEmailMessage
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  [Guid("BB0FEBEB-4CDB-446c-B785-8C3F5BB8A8A7")]
  public interface IHtmlEmailMessage
  {
    string ID { get; }

    DateTime Date { get; }

    LogEntryType EntryType { get; }

    string Comments { get; set; }

    LogAlerts RoleAlerts { get; }

    bool IsAlert { get; }

    string Description { get; }

    string Creator { get; }

    string Sender { get; }

    string Recipient { get; }

    string Subject { get; }

    string Body { get; }
  }
}
