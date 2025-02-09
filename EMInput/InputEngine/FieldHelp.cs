// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.FieldHelp
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.AsmResolver;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.XPath;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class FieldHelp
  {
    private const string className = "FieldHelp";
    private static readonly string sw = Tracing.SwInputEngine;
    private static readonly string fieldHelpDownloadUrl = "http://help.icemortgagetechnology.com/encompass/FieldHelp.xml";
    private static Hashtable helpCache = CollectionsUtil.CreateCaseInsensitiveHashtable();
    private static string downloadedFieldHelpPath = (string) null;
    private static System.Threading.Timer downloadTimer = (System.Threading.Timer) null;

    static FieldHelp()
    {
      try
      {
        FieldHelp.downloadedFieldHelpPath = Path.Combine(SystemSettings.UpdatesDir, "FieldHelp.xml");
        string resourceFileFullPath = AssemblyResolver.GetResourceFileFullPath(Path.Combine(SystemSettings.DocDirRelPath, "FieldHelp.xml"), SystemSettings.LocalAppDir);
        bool flag = false;
        if (FieldHelp.compareModificationDates(FieldHelp.downloadedFieldHelpPath, resourceFileFullPath) < 0)
          flag = true;
        if (!flag)
        {
          if (FieldHelp.readFieldHelp(FieldHelp.downloadedFieldHelpPath))
            goto label_7;
        }
        FieldHelp.readFieldHelp(resourceFileFullPath);
      }
      catch
      {
      }
label_7:
      try
      {
        FieldHelp.downloadTimer = new System.Threading.Timer(new TimerCallback(FieldHelp.downloadFieldHelp), (object) null, TimeSpan.FromMinutes(1.0), TimeSpan.FromMilliseconds(-1.0));
      }
      catch (Exception ex)
      {
        Tracing.Log(FieldHelp.sw, nameof (FieldHelp), TraceLevel.Error, "Failed to start FieldHelp download timer: " + (object) ex);
      }
    }

    public static string GetText(string helpKey)
    {
      return string.Concat(FieldHelp.helpCache[(object) helpKey]);
    }

    private static bool readFieldHelp(string filePath)
    {
      try
      {
        if (!System.IO.File.Exists(filePath))
          return false;
        FieldHelp.helpCache.Clear();
        using (StreamReader streamReader = new StreamReader(filePath, Encoding.ASCII))
        {
          XPathNodeIterator xpathNodeIterator = new XPathDocument((TextReader) streamReader).CreateNavigator().Select("/FieldHelpList/FieldHelp");
          while (xpathNodeIterator.MoveNext())
            FieldHelp.helpCache[(object) xpathNodeIterator.Current.GetAttribute("helpKey", "")] = (object) xpathNodeIterator.Current.Value;
        }
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(FieldHelp.sw, nameof (FieldHelp), TraceLevel.Error, "Error loading field help from file '" + filePath + "': " + (object) ex);
        return false;
      }
    }

    private static void downloadFieldHelp(object notUsed)
    {
      try
      {
        string str = new WebClient()
        {
          Proxy = {
            Credentials = CredentialCache.DefaultCredentials
          }
        }.DownloadString(FieldHelp.fieldHelpDownloadUrl);
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(str ?? "");
        xmlDocument.Save(FieldHelp.downloadedFieldHelpPath);
      }
      catch (Exception ex)
      {
        Tracing.Log(FieldHelp.sw, nameof (FieldHelp), TraceLevel.Warning, "Failed to update FieldHelp XML file: " + (object) ex);
      }
    }

    private static int compareModificationDates(string path1, string path2)
    {
      FileInfo fileInfo1 = new FileInfo(path1);
      FileInfo fileInfo2 = new FileInfo(path2);
      if (!fileInfo1.Exists && !fileInfo2.Exists)
        return 0;
      if (!fileInfo1.Exists)
        return -1;
      return !fileInfo2.Exists ? 1 : (int) (fileInfo1.LastWriteTime - fileInfo2.LastWriteTime).TotalSeconds;
    }
  }
}
