// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.IStateLicense
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  /// <summary>Interface for StateLicense class.</summary>
  /// <exclude />
  [Guid("B7429063-6D69-44b1-8AB2-D6B1AE40B382")]
  public interface IStateLicense
  {
    string State { get; }

    string LicenseNumber { get; set; }

    bool Enabled { get; set; }

    object ExpirationDate { get; set; }

    void SetExpirationDate(object value);

    bool Selected { get; set; }

    bool Exempt { get; set; }

    object IssueDate { get; set; }

    object StartDate { get; set; }

    string LicenseStatus { get; set; }

    object StatusDate { get; set; }

    object LastChecked { get; set; }
  }
}
