// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Exceptions.LicenseException
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common.Licensing;
using System;
using System.Runtime.Serialization;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Exceptions
{
  [Serializable]
  public class LicenseException : ServerException
  {
    private const uint innerHResult = 2147754246;
    private LicenseExceptionType errorType;
    private LicenseInfo license;

    public LicenseException(LicenseInfo license, LicenseExceptionType errorType, string message)
      : base(message)
    {
      this.license = license;
      this.errorType = errorType;
      this.HResult = this.HRESULT(2147754246U);
    }

    protected LicenseException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this.errorType = (LicenseExceptionType) info.GetValue("licenseError", typeof (LicenseExceptionType));
      this.license = (LicenseInfo) info.GetValue(nameof (license), typeof (LicenseInfo));
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("licenseError", (object) this.errorType);
      info.AddValue("license", (object) this.license);
    }

    public LicenseExceptionType Cause => this.errorType;

    public LicenseInfo License => this.license;
  }
}
