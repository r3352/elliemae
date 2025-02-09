// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.AuthenticationResult
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using EllieMae.Encompass.AsmResolver.MOTD;
using EllieMae.Encompass.AsmResolver.Utils;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Xml;

#nullable disable
namespace EllieMae.Encompass.AsmResolver
{
  public class AuthenticationResult
  {
    public readonly AuthResultCode ResultCode;
    public readonly string ResultDescription;
    public readonly NameValueCollection AppCmdLineArgs;
    public readonly NameValueCollection Attributes;
    public readonly MOTDSettings MotdSettings;

    public string InstallationURL
    {
      get
      {
        if (this.ResultCode != AuthResultCode.Success)
          return (string) null;
        string installationUrl = this.ResultDescription;
        if (this.Attributes != null)
        {
          if (this.Attributes["HTTPS"] == "1")
            installationUrl = installationUrl.Replace("http://", "https://");
          else if (this.Attributes["HTTPS"] == "0")
            installationUrl = installationUrl.Replace("https://", "http://");
        }
        return installationUrl;
      }
    }

    public AuthenticationResult(
      AuthResultCode resultCode,
      string resDesc,
      NameValueCollection appCmdLineArgs,
      NameValueCollection attributes,
      MOTDSettings motdSettings)
    {
      this.ResultCode = resultCode;
      this.ResultDescription = resDesc;
      this.AppCmdLineArgs = appCmdLineArgs;
      this.Attributes = attributes;
      if (this.ResultCode == AuthResultCode.Success && (this.ResultDescription ?? "").Trim() == "")
      {
        this.ResultCode = AuthResultCode.NullInstallationURL;
        this.ResultDescription = "Cannot get the installation URL; user may not be authorized to use the application.";
      }
      this.MotdSettings = motdSettings;
    }

    internal static AuthenticationResult ToAuthResult(string xml)
    {
      AuthResultCode resultCode = AuthResultCode.UnhandledException;
      string resDesc = (string) null;
      NameValueCollection appCmdLineArgs = (NameValueCollection) null;
      NameValueCollection attributes = (NameValueCollection) null;
      MOTDSettings motdSettings = (MOTDSettings) null;
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(xml);
      XmlNodeList childNodes1 = xmlDocument.DocumentElement.ChildNodes;
      if (childNodes1 != null && childNodes1.Count > 0)
      {
        foreach (XmlNode xmlNode in childNodes1)
        {
          if (xmlNode is XmlElement xmlElement)
          {
            switch (xmlElement.Name)
            {
              case "Result":
                resultCode = (AuthResultCode) Convert.ToInt32(xmlElement.GetAttribute("resultCode"));
                if (resultCode == AuthResultCode.Success)
                {
                  if ((xmlElement.InnerText ?? "").Trim() != "")
                  {
                    resDesc = XT.DSB64(xmlElement.InnerText, ResolverConsts.KB64);
                    continue;
                  }
                  resultCode = AuthResultCode.NullInstallationURL;
                  resDesc = "Cannot get the installation URL; user may not be authorized to use the application.";
                  continue;
                }
                resDesc = xmlElement.InnerText;
                continue;
              case "AppCmdLineArgs":
                appCmdLineArgs = new NameValueCollection();
                XmlNodeList childNodes2 = xmlElement.ChildNodes;
                if (childNodes2 != null && childNodes2.Count > 0)
                {
                  IEnumerator enumerator = childNodes2.GetEnumerator();
                  try
                  {
                    while (enumerator.MoveNext())
                    {
                      if ((XmlNode) enumerator.Current is XmlElement current && current.Name == "CmdLineArgs")
                        appCmdLineArgs.Add(current.GetAttribute("cmdName"), current.GetAttribute("cmdLineArgs"));
                    }
                    continue;
                  }
                  finally
                  {
                    if (enumerator is IDisposable disposable)
                      disposable.Dispose();
                  }
                }
                else
                  continue;
              case "Attributes":
                attributes = new NameValueCollection();
                XmlNodeList childNodes3 = xmlElement.ChildNodes;
                if (childNodes3 != null && childNodes3.Count > 0)
                {
                  IEnumerator enumerator = childNodes3.GetEnumerator();
                  try
                  {
                    while (enumerator.MoveNext())
                    {
                      if ((XmlNode) enumerator.Current is XmlElement current && current.Name == "Attribute")
                        attributes.Add(current.GetAttribute("name"), current.GetAttribute("value"));
                    }
                    continue;
                  }
                  finally
                  {
                    if (enumerator is IDisposable disposable)
                      disposable.Dispose();
                  }
                }
                else
                  continue;
              case "MOTD":
                motdSettings = new MOTDSettings(Convert.ToInt32(xmlElement.GetAttribute("messageID")), xmlElement.GetAttribute("title"), xmlElement.GetAttribute("description") ?? "", XT.DSB64(xmlElement.GetAttribute("messageURL"), ResolverConsts.KB64), Convert.ToInt32(xmlElement.GetAttribute("numberOfDisplays")), Convert.ToInt32(xmlElement.GetAttribute("displayInterval")), (MOTDSettings.DisplayIntervalUnit) Convert.ToInt32(xmlElement.GetAttribute("intervalUnit")), Convert.ToDateTime(xmlElement.GetAttribute("startTime")), Convert.ToDateTime(xmlElement.GetAttribute("endTime")), Convert.ToInt32(xmlElement.GetAttribute("winWidth")), Convert.ToInt32(xmlElement.GetAttribute("winHeight")));
                continue;
              default:
                continue;
            }
          }
        }
      }
      return new AuthenticationResult(resultCode, resDesc, appCmdLineArgs, attributes, motdSettings);
    }
  }
}
