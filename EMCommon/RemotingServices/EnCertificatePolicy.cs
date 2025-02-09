// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.EnCertificatePolicy
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.Common;
using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class EnCertificatePolicy
  {
    public static readonly EnCertificatePolicy DefaultPolicy = new EnCertificatePolicy();
    private static bool initialized = false;

    private EnCertificatePolicy()
    {
    }

    public static void SetDefaultPolicy()
    {
      lock (typeof (EnCertificatePolicy))
      {
        if (EnCertificatePolicy.initialized)
          return;
        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(EnCertificatePolicy.validateCertificate);
        EnCertificatePolicy.initialized = true;
      }
    }

    private static bool validateCertificate(
      object url,
      X509Certificate cert,
      X509Chain chain,
      SslPolicyErrors policyErrors)
    {
      DateTime notAfter = new X509Certificate2(cert).NotAfter;
      int policyErrorsEnum = (int) policyErrors;
      bool flag = EnCertificatePolicy.checkCondition(notAfter, policyErrorsEnum);
      string absoluteUri = url.ToString();
      try
      {
        absoluteUri = ((HttpWebRequest) url).Address.AbsoluteUri;
      }
      catch
      {
      }
      if (absoluteUri.Contains("Diagnostics.asmx"))
        flag = false;
      if (flag)
      {
        PerformanceMeter performanceMeter = new PerformanceMeter("SSL certificate", "Record faulty SSL", true, false, false, 57, nameof (validateCertificate), "D:\\ws\\24.3.0.0\\EmLite\\Common\\RemotingServices\\EnCertificatePolicy.cs");
        performanceMeter.SslPublication = true;
        performanceMeter.Start();
        performanceMeter.AddVariable("Url", (object) absoluteUri);
        performanceMeter.AddVariable("ExpirationDate", (object) notAfter);
        performanceMeter.AddVariable("Ssl Policy Errors", (object) policyErrorsEnum);
        performanceMeter.Stop();
      }
      return true;
    }

    public static bool checkCondition(DateTime expirationDate, int policyErrorsEnum)
    {
      bool flag = false;
      if (expirationDate < DateTime.Now)
        flag = true;
      if (policyErrorsEnum != 0)
        flag = true;
      return flag;
    }
  }
}
