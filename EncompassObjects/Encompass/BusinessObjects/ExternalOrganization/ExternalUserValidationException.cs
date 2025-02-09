// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalUserValidationException
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  /// <summary>
  /// Exception indicating a failed validation of an externaluser account
  /// </summary>
  public class ExternalUserValidationException : ApplicationException
  {
    internal ExternalUserValidationException(UserViolationType type, string message)
      : base(message)
    {
      this.ExceptionType = type;
    }

    /// <summary>Gets or Sets the number of Login Attempts</summary>
    public int LoginAttempts { get; set; }

    /// <summary>Gets or Sets the the UserInfor Object</summary>
    public ExternalUserInfo UserInfo { get; set; }

    /// <summary>Gets or sets user violation type</summary>
    public UserViolationType ExceptionType { get; set; }
  }
}
