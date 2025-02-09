// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.IConversation
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  [Guid("63BD3082-2EB2-448e-BE2F-EDDC02CDD6A9")]
  public interface IConversation
  {
    string ID { get; }

    object Date { get; }

    LogEntryType EntryType { get; }

    string Comments { get; set; }

    LogAlerts RoleAlerts { get; }

    bool IsAlert { get; }

    string HeldWith { get; set; }

    string Company { get; set; }

    string HeldBy { get; }

    ConversationContactMethod ContactMethod { get; set; }

    string PhoneNumber { get; set; }

    string EmailAddress { get; set; }

    bool DisplayInLog { get; set; }

    string NewComments { get; set; }
  }
}
