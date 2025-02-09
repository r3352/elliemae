// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientCommon.Amyuni
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.Common;
using Microsoft.Win32;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientCommon
{
  public class Amyuni
  {
    public static string DotNetCreatorVersion = (string) null;
    public static readonly string DotNetCreator40LicenseKey = "07EFCDAB01000100DD65CD4954C566886AF6B1DE4307235C7393CC8A0AFA00B96C13ACAFB8D17A5B0B4B30C7D898002D89D3FA3B17D68180426FCBE2CF586ABB7D6F423A";
    public static readonly string ConverterDriverName = "Encompass";
    public static readonly string ConverterCompanyName = "Ellie Mae, Inc.";
    private static string pdfConverterVersion = (string) null;
    private static string activationCode = (string) null;
    private static string pdfCreatorVersion = (string) null;
    private static string creatorCompanyName = (string) null;
    private static string creatorLicenseKey = (string) null;

    private Amyuni()
    {
    }

    public static string PdfConverterVersion
    {
      get
      {
        if (Amyuni.pdfConverterVersion == null)
        {
          using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Control\\Print\\Printers\\Encompass"))
          {
            if (registryKey != null)
            {
              string str = ((string) registryKey.GetValue("Printer Driver") ?? "").Trim();
              int num = str.LastIndexOf(' ');
              Amyuni.pdfConverterVersion = num >= 0 ? str.Substring(num + 1) : throw new Exception("Cannot find Amyuni PDF Converter version");
            }
          }
        }
        return Amyuni.pdfConverterVersion;
      }
    }

    public static string ActivationCode
    {
      get
      {
        if (Amyuni.activationCode == null)
        {
          string appSetting = EnConfigurationSettings.AppSettings["AmyuniActivationCode"];
          if (appSetting != null && appSetting != "")
          {
            Amyuni.activationCode = appSetting;
          }
          else
          {
            switch (Amyuni.PdfConverterVersion)
            {
              case "2.10":
                Amyuni.activationCode = "07EFCDAB01000100DD65CD498AD566880BF6B1DE47CB24D58B88CC8A0AFA00B96C13A7AFD1BC16326F6B7CA7BD4B2064EFB0D03F17FAB180427BC3A3A6211FD5254F127EEDB8749F4301314438DA38A6300FDF043FBECD97FA19AEAEE811DC690C50437420";
                break;
              case "3.00":
              case "300":
                Amyuni.activationCode = "07EFCDAB01000100DD65CD4927C566886BF6B1DE438724557B91CC8A0AFA00B96C13ACAFB8D17A5B0B4B32C7D898012D81DBFE3817D6B089426FC3EACB3B6ABB4C0C423AAB";
                break;
              case "2.51":
                Amyuni.activationCode = "07EFCDAB01000100DD65CD4940C566886BF6B1DE438724557B89CC8A0AFA00B96C13ACAFB8D17A5B0B4B32C7D898012D81DBFE3917D6B0BB426FC3EACB3B6ABB4C0C423AAB";
                break;
              case "400":
                Amyuni.activationCode = "07EFCDAB01000100DD65CD4954C566886AF6B1DE4307235C7393CC8A0AFA00B96C13ACAFB8D17A5B0B4B30C7D898002D89D3FA3B17D68180426FCBE2CF586ABB7D6F423A";
                break;
              default:
                throw new Exception("Amyuni PDF Converter version " + Amyuni.PdfConverterVersion + " not supported");
            }
          }
        }
        return Amyuni.activationCode;
      }
    }

    public static string PdfCreatorVersion
    {
      get
      {
        if (Amyuni.pdfCreatorVersion == null)
        {
          using (RegistryKey registryKey1 = Registry.ClassesRoot.OpenSubKey("CLSID\\{347DD8E0-4959-47d7-BFF4-71B3BD6464AB}"))
          {
            if (registryKey1 != null)
            {
              Amyuni.pdfCreatorVersion = "4.0";
            }
            else
            {
              using (RegistryKey registryKey2 = Registry.ClassesRoot.OpenSubKey("CLSID\\{02CA005A-5208-4455-910A-8F916C878AEA}"))
              {
                if (registryKey2 != null)
                {
                  Amyuni.pdfCreatorVersion = "3.0";
                }
                else
                {
                  using (RegistryKey registryKey3 = Registry.ClassesRoot.OpenSubKey("CLSID\\{02CA0058-5208-4455-910A-8F916C878AEA}"))
                  {
                    if (registryKey3 == null)
                      throw new Exception("Cannot find Amyuni PDF Creator version");
                    Amyuni.pdfCreatorVersion = "1.0";
                  }
                }
              }
            }
          }
        }
        return Amyuni.pdfCreatorVersion;
      }
    }

    public static string CreatorCompanyName
    {
      get
      {
        if (Amyuni.creatorCompanyName == null)
        {
          switch (Amyuni.PdfCreatorVersion)
          {
            case "4.0":
            case "3.0":
              Amyuni.creatorCompanyName = "Ellie Mae, Inc.";
              break;
            case "1.0":
              Amyuni.creatorCompanyName = "EllieMae Inc.";
              break;
            default:
              throw new Exception("Amyuni PDF Creator version " + Amyuni.PdfCreatorVersion + " not supported");
          }
        }
        return Amyuni.creatorCompanyName;
      }
    }

    public static string CreatorLicenseKey
    {
      get
      {
        if (Amyuni.creatorLicenseKey == null)
        {
          string appSetting = EnConfigurationSettings.AppSettings["AmyuniLicenseKey"];
          if (appSetting != null && appSetting != "")
            Amyuni.creatorLicenseKey = appSetting;
          else if (Amyuni.pdfCreatorVersion == "4.0")
          {
            Amyuni.creatorLicenseKey = "07EFCDAB01000100DD65CD491FC5668861F6B1DE631733DC7391CC8A0AFA00B96C13A9AF97D17AA40B4B33CED804002D81B0FE3B17DEB084426FC3EBCB586A";
          }
          else
          {
            switch (Amyuni.PdfCreatorVersion)
            {
              case "3.0":
                Amyuni.creatorLicenseKey = "07EFCDAB01000100DD65CD491CC5668861F6B1DE631733DC7391CC8A0AFA00B96C13A9AF97D17AA40B4B31CED804002D81B0FE3B17DEB083426FC3EBCB586A";
                break;
              case "1.0":
                Amyuni.creatorLicenseKey = "07EFCDAB010001006A84D753B4D5672E686423316B8936978B2F517AB9059A1FC1448005383FD1B2967FDA8F4180120BBD3074D5";
                break;
            }
          }
        }
        return Amyuni.creatorLicenseKey;
      }
    }
  }
}
