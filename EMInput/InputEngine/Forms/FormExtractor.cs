// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.Forms.FormExtractor
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.AsmResolver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.InputEngine.Forms
{
  public class FormExtractor
  {
    private static readonly Regex srcRegex = new Regex("src=\"(?<src>[^\"]+)\"", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
    private static Dictionary<string, string> emfrmPaths = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);

    public static string ExtractForm(string formId, string emfrmPath)
    {
      FormExtractor.emfrmPaths[formId] = emfrmPath;
      return FormExtractor.ExtractForm(emfrmPath, FormExtractor.getFormExtractionFolder(formId), false);
    }

    public static string ExtractForm(string emfrmPath, string extractionPath, bool forceExtract)
    {
      bool flag = forceExtract;
      if (!Directory.Exists(extractionPath))
        flag = true;
      else if (Directory.GetLastWriteTime(extractionPath) < File.GetLastWriteTime(emfrmPath))
        flag = true;
      if (flag)
        FormExtractor.extractFormToFolder(emfrmPath, extractionPath);
      string htmlForm = FormExtractor.findHTMLForm(extractionPath);
      if (flag)
        FormExtractor.resolveFormMacros(htmlForm);
      return htmlForm;
    }

    public static string GetFormSourcePath(string formId)
    {
      return FormExtractor.emfrmPaths.ContainsKey(formId) ? FormExtractor.emfrmPaths[formId] : (string) null;
    }

    private static void resolveFormMacros(string formPath)
    {
      string str1 = (string) null;
      using (StreamReader streamReader = new StreamReader(formPath, Encoding.ASCII))
        str1 = streamReader.ReadToEnd();
      string input = str1.Replace("$(ENFORMDIR)", AssemblyResolver.GetResourceFileFolderPath(SystemSettings.FormRelDir));
      string str2 = FormExtractor.srcRegex.Replace(input, new MatchEvaluator(FormExtractor.evalSrcAttribute));
      using (StreamWriter streamWriter = new StreamWriter(formPath, false, Encoding.ASCII))
        streamWriter.Write(str2);
    }

    private static string evalSrcAttribute(Match m)
    {
      return m.Groups["src"].Value.IndexOf(":") >= 0 ? m.Value.Remove(m.Groups["src"].Index - m.Index, m.Groups["src"].Length) : m.Value;
    }

    private static void extractFormToFolder(string formPath, string extractionPath)
    {
      FileCompressor.Instance.Unzip(formPath, extractionPath);
      for (int index = 10; index >= 0; --index)
      {
        try
        {
          Directory.SetLastWriteTime(extractionPath, File.GetLastWriteTime(formPath));
          break;
        }
        catch (Exception ex)
        {
          if (index == 0)
            throw ex;
          Thread.Sleep(100);
        }
      }
    }

    private static string findHTMLForm(string path)
    {
      string[] files1 = Directory.GetFiles(path, "*.htm");
      if (files1.Length != 0)
        return files1[0];
      string[] files2 = Directory.GetFiles(path, "*.html");
      return files2.Length != 0 ? files2[0] : throw new Exception("Form does not contain an HTML file");
    }

    private static string getFormExtractionFolder(string formId)
    {
      return Path.Combine(SystemSettings.TempFolderRoot, "Forms\\" + FileSystem.EncodeFilename(formId, false));
    }
  }
}
