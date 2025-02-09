// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Err
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.Common.Diagnostics;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Events;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public sealed class Err : IErr
  {
    public static readonly Err Instance = new Err();
    private static readonly EventLog appEventLog;
    private static Err.SysAlertDescriptor[] alertDescriptors;

    private Err()
    {
    }

    static Err()
    {
      EllieMae.EMLite.DataAccess.ServerGlobals.Err = (IErr) Err.Instance;
      Logger.IsDebug = EllieMae.EMLite.DataAccess.ServerGlobals.Debug;
      Err.alertDescriptors = Err.readSystemAlertDescriptors();
      try
      {
        Err.appEventLog = new EventLog();
        Err.appEventLog.BeginInit();
        Err.appEventLog.Log = "Application";
        Err.appEventLog.Source = "Encompass Server";
        Err.appEventLog.EndInit();
      }
      catch (Exception ex)
      {
        throw new Exception("Failed to initialize Application Event log: " + ex.Message, ex);
      }
    }

    public static void Initialize()
    {
    }

    public void RaiseI(string category, ServerDataException ex)
    {
      Err.Raise(category, (ServerException) ex);
    }

    public static void Raise(string className, ServerException ex)
    {
      Err.Raise(TraceLevel.Error, className, ex);
    }

    public static void Raise(IClientContext context, string className, ServerException ex)
    {
      Err.Raise(context, TraceLevel.Error, className, ex);
    }

    public static void Raise(TraceLevel level, string className, ServerException ex)
    {
      Err.Raise(ClientContext.GetCurrent(false), level, className, ex);
    }

    public static void Raise(
      IClientContext context,
      TraceLevel level,
      string className,
      ServerException ex)
    {
      ex.Source = className;
      ex.TraceLevel = level;
      if (context == null)
        ServerGlobals.TraceLog.WriteException(level, className, (Exception) ex);
      else
        context.TraceLog.WriteException(level, className, (Exception) ex);
      Err.SysAlertDescriptor sysAlertDescriptor = Err.getSysAlertDescriptor(ex);
      if (sysAlertDescriptor != null)
        Err.WriteSystemAlert(sysAlertDescriptor, ex);
      if (ex.SessionInfo != null && level == TraceLevel.Error)
        EncompassServer.RaiseEvent(context, (ServerEvent) new ExceptionEvent((Exception) ex));
      throw ex;
    }

    public static void Reraise(string className, Exception ex)
    {
      Err.Reraise(ClientContext.GetCurrent(false), className, ex);
    }

    public static void Reraise(IClientContext context, string className, Exception ex)
    {
      if (ex is ServerException)
        throw ex;
      Err.Raise(context, className, new ServerException("Unexpected server error: " + ex.Message, ex));
    }

    public static void Reraise(string className, Exception ex, SessionInfo info)
    {
      if (ex is ServerException)
      {
        ServerException serverException = (ServerException) ex;
        serverException.SessionInfo = serverException.SessionInfo == null ? info : throw ex;
        if (serverException.TraceLevel == TraceLevel.Error)
          EncompassServer.RaiseEvent(ClientContext.GetCurrent(false), (ServerEvent) new ExceptionEvent(ex));
      }
      else
        Err.Raise(className, new ServerException("Unexpected server error: " + ex.Message, ex, info));
    }

    internal static void WriteSystemAlert(Err.SysAlertDescriptor desc, ServerException ex)
    {
      IClientContext current = ClientContext.GetCurrent(false);
      string source = current != null ? (!(current.InstanceName == "") ? current.InstanceName + " Instance" : "Default Instance") : "Core Service";
      Err.WriteApplicationEvent("Error: " + desc.Description + Environment.NewLine + "Source: " + source + Environment.NewLine + "Exception Info:" + Environment.NewLine + ex.ToString(), desc.EntryType, desc.EventID, (EventLogEntryCategory) desc.Category);
      EncompassServer.AddSystemAlert(new SystemAlert(desc.EventID, (SystemAlertCategory) desc.Category, source, desc.Description));
    }

    internal static void WriteApplicationEvent(
      string message,
      EventLogEntryType eventType,
      int eventID,
      EventLogEntryCategory eventCategory)
    {
      if (Err.appEventLog == null)
        return;
      try
      {
        Err.appEventLog.WriteEntry(message, eventType, eventID, (short) eventCategory);
      }
      catch
      {
      }
    }

    private static Err.SysAlertDescriptor getSysAlertDescriptor(ServerException ex)
    {
      string str = ex.ToString();
      if (Err.alertDescriptors != null)
      {
        foreach (Err.SysAlertDescriptor alertDescriptor in Err.alertDescriptors)
        {
          if (str.Contains(alertDescriptor.ErrorText))
            return alertDescriptor;
        }
      }
      return (Err.SysAlertDescriptor) null;
    }

    private static Err.SysAlertDescriptor[] readSystemAlertDescriptors()
    {
      try
      {
        string filename = Path.Combine(EnConfigurationSettings.GlobalSettings.EncompassProgramDirectory, "documents\\SystemAlerts.xml");
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(filename);
        List<Err.SysAlertDescriptor> sysAlertDescriptorList = new List<Err.SysAlertDescriptor>();
        foreach (XmlElement selectNode in xmlDocument.SelectNodes("//SysAlert"))
          sysAlertDescriptorList.Add(new Err.SysAlertDescriptor(selectNode));
        return sysAlertDescriptorList.ToArray();
      }
      catch
      {
        return new Err.SysAlertDescriptor[0];
      }
    }

    internal class SysAlertDescriptor
    {
      public readonly string Description;
      public readonly string ErrorText;
      public readonly EventLogEntryType EntryType;
      public readonly int EventID;
      public readonly short Category;

      public SysAlertDescriptor(XmlElement e)
      {
        XmlElement xmlElement1 = (XmlElement) e.SelectSingleNode(nameof (ErrorText));
        if (xmlElement1 != null)
          this.ErrorText = xmlElement1.InnerText;
        XmlElement xmlElement2 = (XmlElement) e.SelectSingleNode(nameof (Description));
        if (xmlElement2 != null)
          this.Description = xmlElement2.InnerText;
        try
        {
          this.EntryType = (EventLogEntryType) Enum.Parse(typeof (EventLogEntryType), e.GetAttribute("Type"), true);
        }
        catch
        {
          this.EntryType = EventLogEntryType.Error;
        }
        this.EventID = Utils.ParseInt((object) e.GetAttribute(nameof (EventID)), 0);
        this.Category = (short) Utils.ParseInt((object) e.GetAttribute(nameof (Category)), 0);
      }
    }
  }
}
