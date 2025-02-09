// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.IPipelineData
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  [Guid("1C2039BE-DEBC-42a5-A7DF-4B8545662BB5")]
  public interface IPipelineData
  {
    object this[string fname] { get; }

    LoanIdentity LoanIdentity { get; }

    PipelineAlerts Alerts { get; }

    LoanLock CurrentLock { get; }

    LoanAccessRights GetAccessRights();

    StringList GetFieldNames();
  }
}
