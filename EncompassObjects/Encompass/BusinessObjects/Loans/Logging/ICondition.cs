// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.ICondition
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.Collections;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>Interface for EDMTransaction class.</summary>
  /// <exclude />
  [Guid("5F5BFF37-4A10-481f-88DE-49710135137A")]
  public interface ICondition
  {
    string ID { get; }

    LogEntryType EntryType { get; }

    Comments Comments { get; }

    LogAlerts RoleAlerts { get; }

    bool IsAlert { get; }

    string Title { get; set; }

    ConditionType ConditionType { get; }

    string Description { get; set; }

    string Source { get; set; }

    string AddedBy { get; }

    DateTime DateAdded { get; }

    BorrowerPair BorrowerPair { get; set; }

    ConditionStatus Status { get; }

    LogEntryList GetLinkedDocuments();

    string Details { get; set; }
  }
}
