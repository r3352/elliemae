// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.DeploymentHandlers.CertificateHelper
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;

#nullable disable
namespace EllieMae.Encompass.AsmResolver.DeploymentHandlers
{
  public class CertificateHelper
  {
    private static X509Certificate2 GetDigitalCertificate(string filename)
    {
      X509Certificate2 digitalCertificate = (X509Certificate2) null;
      IntPtr zero1 = IntPtr.Zero;
      IntPtr zero2 = IntPtr.Zero;
      IntPtr zero3 = IntPtr.Zero;
      int dwExpectedContentTypeFlags = 1792;
      if (!WinCrypt.CryptQueryObject(1, Marshal.StringToHGlobalUni(filename), dwExpectedContentTypeFlags, 14, 0, out int _, out int _, out int _, ref zero1, ref zero2, ref zero3))
        throw new Win32Exception(Marshal.GetLastWin32Error());
      int pcbData = 0;
      if (!WinCrypt.CryptMsgGetParam(zero2, 29, 0, IntPtr.Zero, ref pcbData))
        throw new Win32Exception(Marshal.GetLastWin32Error());
      byte[] numArray = new byte[pcbData];
      if (!WinCrypt.CryptMsgGetParam(zero2, 29, 0, numArray, ref pcbData))
        throw new Win32Exception(Marshal.GetLastWin32Error());
      SignedCms signedCms = new SignedCms();
      signedCms.Decode(numArray);
      if (signedCms.SignerInfos.Count > 0)
      {
        SignerInfo signerInfo = signedCms.SignerInfos[0];
        if (signerInfo.Certificate != null)
          digitalCertificate = signerInfo.Certificate;
      }
      return digitalCertificate;
    }

    public static bool isCertExpired(string assemblyFilePath)
    {
      if (assemblyFilePath == string.Empty)
        throw new Exception("Null assembly file info while trying to compare the assembly versions. The assembly file info may not be present in the application manifest.");
      try
      {
        if (File.Exists(assemblyFilePath))
        {
          X509Certificate2 digitalCertificate = CertificateHelper.GetDigitalCertificate(assemblyFilePath);
          if (digitalCertificate != null && !digitalCertificate.Subject.ToLower().Contains("ice mortgage technology"))
            return true;
        }
        return false;
      }
      catch
      {
        throw;
      }
    }
  }
}
