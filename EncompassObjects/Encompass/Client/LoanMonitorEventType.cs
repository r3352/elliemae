// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.LoanMonitorEventType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Client
{
  /// <summary>
  /// Enumeration of the loan event types supported by the <see cref="T:EllieMae.Encompass.Client.LoanMonitorEventArgs" />.
  /// </summary>
  [Guid("222417DA-234D-494a-968E-1C1FB9932D4F")]
  public enum LoanMonitorEventType
  {
    /// <summary>Loan has been opened by a user.</summary>
    Opened = 1,
    /// <summary>Loan has been locked by a user.</summary>
    Locked = 2,
    /// <summary>Loan has been unlocked by a user.</summary>
    Unlocked = 3,
    /// <summary>Loan has been saved by a user.</summary>
    Saved = 4,
    /// <summary>Loan has been imported by a user.</summary>
    Imported = 5,
    /// <summary>Loan has been exported by a user.</summary>
    Exported = 6,
    /// <summary>Loan permissions have been changed by a user.</summary>
    PermissionsChanged = 7,
    /// <summary>Loan has been closed by a user.</summary>
    Closed = 8,
    /// <summary>Loan has been created by a user.</summary>
    Created = 9,
    /// <summary>Loan has been moved by a user.</summary>
    Moved = 10, // 0x0000000A
    /// <summary>Loan has been deleted by a user.</summary>
    Deleted = 11, // 0x0000000B
  }
}
