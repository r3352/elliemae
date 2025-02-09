// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Diagnostics.DiagnosticSubmissionProcess
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.WebServices;
using Microsoft.Web.Services2.Attachments;
using Microsoft.Web.Services2.Dime;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Diagnostics
{
  public class DiagnosticSubmissionProcess
  {
    private DiagnosticSession session;
    private Exception runtimeException;
    private string packageId;
    private IProgressFeedback feedback;

    public DiagnosticSubmissionProcess(DiagnosticSession session, Exception runtimeException)
    {
      this.session = session;
      this.runtimeException = runtimeException;
    }

    public bool Execute()
    {
      DialogResult dialogResult = DialogResult.None;
      while (dialogResult != DialogResult.OK)
      {
        using (DiagnosticSubmissionForm diagnosticSubmissionForm = new DiagnosticSubmissionForm(this.session))
        {
          dialogResult = diagnosticSubmissionForm.ShowDialog();
          if (dialogResult != DialogResult.OK)
            return false;
        }
        using (ProgressDialog progressDialog = new ProgressDialog("Encompass Diagnostics", new AsynchronousProcess(this.uploadDiagnosticData), (object) this.runtimeException, true))
          dialogResult = progressDialog.ShowDialog();
      }
      using (DiagnosticSubmissionResultsForm submissionResultsForm = new DiagnosticSubmissionResultsForm(this.packageId))
      {
        int num = (int) submissionResultsForm.ShowDialog();
      }
      return true;
    }

    private DialogResult uploadDiagnosticData(object notUsed, IProgressFeedback feedback)
    {
      string path = (string) null;
      try
      {
        this.feedback = feedback;
        feedback.Status = "Writing diagnostic summary data...";
        this.generateSessionInfoFile();
        feedback.Status = "Packaging diagnostic data...";
        path = this.createDiagnosticPackage();
        if (feedback.Cancel)
          return DialogResult.Cancel;
        string variable1 = this.session.GetVariable("ClientID");
        string variable2 = this.session.GetVariable("UserID");
        string caseNumber = DiagnosticSession.CaseNumber;
        string variable3 = this.session.GetVariable("Message");
        feedback.Status = "Uploading diagnostic data...";
        DimeAttachment dimeAttachment = new DimeAttachment(".zip", TypeFormat.Unchanged, path);
        using (DiagnosticsService diagnosticsService = new DiagnosticsService(Session.StartupInfo?.ServiceUrls?.JedServicesUrl))
        {
          diagnosticsService.ChunkSent += new ChunkHandler(this.onDataChunkSent);
          diagnosticsService.RequestSoapContext.Attachments.Add((Attachment) dimeAttachment);
          try
          {
            this.packageId = diagnosticsService.PostDiagnostics(variable1, variable2, caseNumber, variable3);
          }
          catch (Exception ex)
          {
            if (ex.Message.Contains("aborted"))
              return DialogResult.Cancel;
          }
        }
        return DialogResult.OK;
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "An error has occured while attempting to submit the diagnostic data: " + (object) ex, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return DialogResult.Abort;
      }
      finally
      {
        if (path != null && File.Exists(path))
          File.Delete(path);
      }
    }

    private void onDataChunkSent(object sender, ChunkHandlerEventArgs e)
    {
      if (this.feedback.MaxValue != (int) e.TotalBytes)
        this.feedback.MaxValue = (int) e.TotalBytes;
      this.feedback.Value = (int) e.TransferredBytes;
      if (!this.feedback.Cancel)
        return;
      e.Abort = true;
    }

    private string createDiagnosticPackage()
    {
      string diagnosticPackage = Path.Combine(Path.GetTempPath(), "diag-" + this.session.SessionID + ".zip");
      try
      {
        FileCompressor.Instance.ZipDirectory(this.session.GetSessionLogDir(), diagnosticPackage);
      }
      catch (Exception ex)
      {
        if (File.Exists(diagnosticPackage))
          File.Delete(diagnosticPackage);
        throw new Exception("Failed to create diagnostics ZIP file", ex);
      }
      return diagnosticPackage;
    }

    private void generateSessionInfoFile()
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml("<Session />");
      xmlDocument.DocumentElement.SetAttribute("CaseNumber", DiagnosticSession.CaseNumber);
      xmlDocument.DocumentElement.SetAttribute("StartTime", this.session.StartTime.ToString("MM/dd/yyyy h:mm:ss tt"));
      xmlDocument.DocumentElement.SetAttribute("EndTime", this.session.EndTime.ToString("MM/dd/yyyy h:mm:ss tt"));
      XmlElement xmlElement1 = (XmlElement) xmlDocument.DocumentElement.AppendChild((XmlNode) xmlDocument.CreateElement("System"));
      xmlElement1.SetAttribute("OSVersion", SystemUtil.OS.ToString());
      xmlElement1.SetAttribute("MachineName", Environment.MachineName);
      xmlElement1.SetAttribute("UserName", Environment.UserName);
      xmlElement1.SetAttribute("ProcessorCount", Environment.ProcessorCount.ToString());
      XmlElement xmlElement2 = (XmlElement) xmlElement1.AppendChild((XmlNode) xmlDocument.CreateElement("Memory"));
      SystemUtil.MemoryInfo memoryInformation = SystemUtil.GetMemoryInformation();
      xmlElement2.SetAttribute("Total", memoryInformation.TotalPhysicalMemory.ToString());
      xmlElement2.SetAttribute("Available", memoryInformation.AvailablePhysicalMemory.ToString());
      XmlElement xmlElement3 = (XmlElement) xmlElement1.AppendChild((XmlNode) xmlDocument.CreateElement("Processor"));
      SystemUtil.ProcessorInfo processorInformation = SystemUtil.GetProcessorInformation();
      xmlElement3.SetAttribute("ID", processorInformation.ProcessorID);
      xmlElement3.SetAttribute("Speed", processorInformation.ProcessorSpeed);
      xmlElement3.SetAttribute("Count", Environment.ProcessorCount.ToString());
      string commandLineArg = Environment.GetCommandLineArgs()[0];
      XmlElement xmlElement4 = (XmlElement) xmlDocument.DocumentElement.AppendChild((XmlNode) xmlDocument.CreateElement("Application"));
      xmlElement4.SetAttribute("Name", Path.GetFileNameWithoutExtension(commandLineArg));
      xmlElement4.SetAttribute("Version", VersionInformation.CurrentVersion.DisplayVersion);
      xmlElement4.SetAttribute("FileVersion", FileVersionInfo.GetVersionInfo(commandLineArg).FileVersion);
      xmlElement4.SetAttribute("AsmVersion", Assembly.GetEntryAssembly().GetName().Version.ToString());
      XmlElement xmlElement5 = (XmlElement) xmlDocument.DocumentElement.AppendChild((XmlNode) xmlDocument.CreateElement("Software"));
      XmlElement acroNode = (XmlElement) xmlElement5.AppendChild((XmlNode) xmlDocument.CreateElement("Acrobat"));
      XmlElement officeNode = (XmlElement) xmlElement5.AppendChild((XmlNode) xmlDocument.CreateElement("Office"));
      try
      {
        DiagnosticSubmissionProcess.populateAcrobatData(acroNode);
      }
      catch
      {
      }
      try
      {
        DiagnosticSubmissionProcess.populateOfficeData(officeNode);
      }
      catch
      {
      }
      XmlElement xmlElement6 = (XmlElement) xmlDocument.DocumentElement.AppendChild((XmlNode) xmlDocument.CreateElement("Variables"));
      foreach (string variableName in this.session.GetVariableNames())
        xmlElement6.AppendChild((XmlNode) xmlDocument.CreateElement(variableName)).InnerText = this.session.GetVariable(variableName);
      if (this.runtimeException != null)
        xmlDocument.DocumentElement.AppendChild((XmlNode) xmlDocument.CreateElement("Exception")).InnerText = this.runtimeException.ToString();
      string filename = Path.Combine(this.session.GetSessionLogDir(), "SessionInfo.xml");
      xmlDocument.Save(filename);
    }

    private static void populateAcrobatData(XmlElement acroNode)
    {
      string str1 = "";
      string str2 = "";
      using (RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey("AcroPDF.PDF\\CLSID", false))
      {
        if (registryKey != null)
          str1 = string.Concat(registryKey.GetValue(""));
      }
      if (str1 != "")
      {
        using (RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey("CLSID\\" + str1 + "\\InprocServer32", false))
        {
          if (registryKey != null)
            str2 = string.Concat(registryKey.GetValue(""));
        }
      }
      try
      {
        if (str2 != "")
        {
          if (File.Exists(str2))
          {
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(str2);
            acroNode.SetAttribute("ActiveXVersion", versionInfo.FileVersion);
          }
        }
      }
      catch
      {
      }
      using (RegistryKey registryKey1 = Registry.LocalMachine.OpenSubKey("Software\\Adobe\\Acrobat Reader", false))
      {
        if (registryKey1 != null)
        {
          foreach (string subKeyName in registryKey1.GetSubKeyNames())
          {
            using (RegistryKey registryKey2 = registryKey1.OpenSubKey(subKeyName + "\\InstallPath", false))
            {
              if (registryKey2 != null)
              {
                string path1 = string.Concat(registryKey2.GetValue(""));
                if (path1 != "")
                {
                  string str3 = Path.Combine(path1, "AcroRd32.exe");
                  if (!File.Exists(str3))
                    str3 = Path.Combine(path1, "AcroRd64.exe");
                  if (File.Exists(str3))
                    ((XmlElement) acroNode.AppendChild((XmlNode) acroNode.OwnerDocument.CreateElement("Reader"))).SetAttribute("Version", FileVersionInfo.GetVersionInfo(str3).FileVersion);
                }
              }
            }
          }
        }
      }
      using (RegistryKey registryKey3 = Registry.LocalMachine.OpenSubKey("Software\\Adobe\\Adobe Acrobat", false))
      {
        if (registryKey3 == null)
          return;
        foreach (string subKeyName in registryKey3.GetSubKeyNames())
        {
          using (RegistryKey registryKey4 = registryKey3.OpenSubKey(subKeyName + "\\InstallPath", false))
          {
            if (registryKey4 != null)
            {
              string path1 = string.Concat(registryKey4.GetValue(""));
              if (path1 != "")
              {
                string str4 = Path.Combine(path1, "Acrobat.exe");
                if (!File.Exists(str4))
                  str4 = Path.Combine(path1, "Acrobat32.exe");
                if (!File.Exists(str4))
                  str4 = Path.Combine(path1, "Acrobat64.exe");
                if (File.Exists(str4))
                  ((XmlElement) acroNode.AppendChild((XmlNode) acroNode.OwnerDocument.CreateElement("Full"))).SetAttribute("Version", FileVersionInfo.GetVersionInfo(str4).FileVersion);
              }
            }
          }
        }
      }
    }

    private static void populateOfficeData(XmlElement officeNode)
    {
      string str = "";
      using (RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey("Word.Application\\CurVer", false))
      {
        if (registryKey == null)
          return;
        str = string.Concat(registryKey.GetValue(""));
      }
      if (str.EndsWith(".8"))
        str = "97";
      else if (str.EndsWith(".9"))
        str = "2000";
      else if (str.EndsWith(".10"))
        str = "XP/2002";
      else if (str.EndsWith(".11"))
        str = "2003";
      else if (str.EndsWith(".12"))
        str = "2007";
      else if (str.EndsWith(".14"))
        str = "2010";
      officeNode.SetAttribute("Version", str);
    }
  }
}
