// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Licensing.LicenseException
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Licensing
{
  /// <summary>
  /// Exception used to notify clients that a licensing-related error has occurred.
  /// </summary>
  [ComVisible(false)]
  public class LicenseException : ApplicationException
  {
    /// <summary>Constructor to initialize</summary>
    /// <param name="message"></param>
    public LicenseException(string message)
      : this(message, (Exception) null)
    {
    }

    /// <summary>Constructor to initialize class variables</summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public LicenseException(string message, Exception innerException)
      : base(message, innerException)
    {
      this.HResult = -2147212799;
    }
  }
}
