// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.IPrintEvent
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>Interface for GeneralEntry class.</summary>
  /// <exclude />
  [Guid("4858B270-3B79-4100-B573-93B48E125F5C")]
  public interface IPrintEvent
  {
    string ID { get; }

    object Date { get; }

    LogEntryType EntryType { get; }

    string Comments { get; set; }

    LogAlerts RoleAlerts { get; }

    bool IsAlert { get; }

    string PerformedBy { get; set; }

    string PerformedByUserName { get; set; }

    PrintAction PrintAction { get; set; }

    PrintDocuments Documents { get; }
  }
}
