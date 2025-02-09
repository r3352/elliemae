// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.IStateLicenseExtType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  /// <summary>
  /// Interface for IStateLicenseExtType class to support External State Licensing Types
  /// </summary>
  /// <exclude />
  [Guid("EE179522-C3BD-417F-83CE-3479D2CD8375")]
  public interface IStateLicenseExtType
  {
    string StateAbbrevation { get; set; }

    string LicenseType { get; set; }

    bool Selected { get; set; }

    bool Exempt { get; set; }

    string LicenseNo { get; set; }

    DateTime IssueDate { get; set; }

    DateTime StartDate { get; set; }

    DateTime EndDate { get; set; }

    string LicenseStatus { get; set; }

    DateTime StatusDate { get; set; }

    bool Approved { get; set; }

    DateTime LastChecked { get; set; }
  }
}
