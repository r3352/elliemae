// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Export.ExportClient
// Assembly: EMExport, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D06A4C35-7634-4F74-B132-8DD78784C14A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMExport.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

#nullable disable
namespace EllieMae.EMLite.Export
{
  public class ExportClient
  {
    private const string typeName = "ExportData";
    private static string traceSW = Tracing.SwImportExport;

    public string GetAccessibleClients(string format, LoanData loanData)
    {
      string exportAssemblyPath = ExportData.GetExportAssemblyPath(format);
      Tracing.Log(ExportClient.traceSW, "ExportData", TraceLevel.Verbose, "Export assembly file: " + exportAssemblyPath);
      if (!File.Exists(exportAssemblyPath))
        return string.Empty;
      string fullName1 = AssemblyName.GetAssemblyName(exportAssemblyPath).FullName;
      Assembly assembly1 = (Assembly) null;
      foreach (Assembly assembly2 in AppDomain.CurrentDomain.GetAssemblies())
      {
        if (assembly2.FullName == fullName1)
          assembly1 = assembly2;
      }
      if (assembly1 == (Assembly) null)
      {
        FileStream fileStream = File.OpenRead(exportAssemblyPath);
        byte[] numArray = new byte[fileStream.Length];
        fileStream.Read(numArray, 0, numArray.Length);
        fileStream.Close();
        assembly1 = Assembly.Load(numArray);
      }
      string fullName2 = typeof (IClient).FullName;
      string typeName = (string) null;
      foreach (Type type in assembly1.GetTypes())
      {
        if (type.GetInterface(fullName2) != (Type) null)
          typeName = type.FullName;
      }
      IClient instance = (IClient) assembly1.CreateInstance(typeName);
      instance.Bam = (IBam) new Bam(loanData);
      MethodInfo method = instance.GetType().GetMethod("GetAccessibleClientIDs");
      if (method == (MethodInfo) null)
        throw new NotSupportedException("The export format '" + format + "' is not supported.");
      return string.Concat(method.Invoke((object) instance, new object[1]
      {
        (object) VersionInformation.CurrentVersion.GetExtendedVersion(Session.EncompassEdition)
      }));
    }
  }
}
