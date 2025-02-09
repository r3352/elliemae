// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.ILoanMonitorEventArgs
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.BusinessObjects.Loans;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Client
{
  /// <summary>Interface for the ConnectionEventArgs class</summary>
  /// <exclude />
  [Guid("25473C47-76AE-4d51-8022-F4E4FDBDE71D")]
  public interface ILoanMonitorEventArgs
  {
    LoanMonitorEventType EventType { get; }

    SessionInformation SessionInformation { get; }

    LoanIdentity LoanIdentity { get; }
  }
}
