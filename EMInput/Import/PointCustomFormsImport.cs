// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Import.PointCustomFormsImport
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.CustomLetters;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

#nullable disable
namespace EllieMae.EMLite.Import
{
  public class PointCustomFormsImport : IDisposable
  {
    private const string className = "PointCustomFormsImport";
    private static readonly string sw = Tracing.SwImportExport;
    private BinaryObject letter;
    private bool isPublic;
    private string folder = string.Empty;
    private static object oFalse = (object) false;
    private static object oTrue = (object) true;
    private static object oMissing = (object) Missing.Value;
    private WordHandler wordHandler;

    public int Import(string file, string folder, bool isPublic)
    {
      ReportLog.AddLog("Import logs for custom form: " + file);
      int logCount = ReportLog.GetLogCount();
      this.wordHandler = new WordHandler();
      this.isPublic = isPublic;
      this.folder = folder;
      this.wordHandler.OpenAndSetDoc(file);
      this.wordHandler.ReplacePointFields();
      string extension = Path.GetExtension(file);
      string str1 = file.Replace(extension, ".doc");
      string str2 = str1.Substring(str1.LastIndexOf("\\") + 1);
      try
      {
        this.wordHandler.SaveAs(SystemUtil.CombinePath(SystemSettings.LocalCustomLetterDir, str2));
        this.wordHandler.ShutDown();
        this.letter = new BinaryObject(SystemUtil.CombinePath(SystemSettings.LocalCustomLetterDir, str2));
        Session.ConfigurationManager.SaveCustomLetter(CustomLetterType.Generic, new FileSystemEntry(SystemUtil.CombinePath(this.folder, str2), FileSystemEntry.Types.File, isPublic ? (string) null : Session.UserID), this.letter);
        File.Delete(SystemUtil.CombinePath(SystemSettings.LocalCustomLetterDir, str2));
      }
      catch (Exception ex)
      {
        Tracing.Log(PointCustomFormsImport.sw, TraceLevel.Error, nameof (PointCustomFormsImport), "Encompass client cannot save custom letter. Exception:" + ex.Message + "r\n");
        return 0;
      }
      if (ReportLog.GetLogCount() == logCount)
        ReportLog.RemoveLastLog();
      return 1;
    }

    public void Dispose()
    {
      if (this.letter == null)
        return;
      this.letter.Dispose();
      this.letter = (BinaryObject) null;
    }
  }
}
