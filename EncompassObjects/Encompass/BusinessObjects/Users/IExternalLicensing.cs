// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.IExternalLicensing
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Collections.Generic;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  /// <summary>
  /// Interface for IExternalLicensing class to support External Licensing
  /// </summary>
  /// <exclude />
  [Guid("6ACA232E-3AD4-4A38-B176-6FA86C8397D4")]
  public interface IExternalLicensing
  {
    bool UseParentInfo { get; set; }

    string LenderType { get; set; }

    string HomeState { get; set; }

    bool StatutoryElectionInMaryland { get; set; }

    bool StatutoryElectionInKansas { get; set; }

    bool UseCustomLenderProfile { get; set; }

    ATRSmallCreditors ATRSmallCreditor { get; set; }

    string ATRSmallCreditorToString();

    ATRExemptCreditors ATRExemptCreditor { get; set; }

    string ATRExemptCreditorToString();

    int AllowLoansWithIssues { get; set; }

    string MsgUploadNonApprovedLoans { get; set; }

    List<StateLicenseExtType> StateLicenseExtTypes { get; }

    void AddStateLicenseExtType(StateLicenseExtType stateLicenseExtType);
  }
}
