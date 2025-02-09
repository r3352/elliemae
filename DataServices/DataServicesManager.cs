// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataServices.DataServicesManager
// Assembly: DataServices, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 227B0203-DF45-468D-9C1B-FA6CED472E23
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataServices.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.DataServices
{
  public static class DataServicesManager
  {
    private const string className = "DataServicesManager";
    private static string sw = Tracing.SwEpass;

    public static void UpdateReport()
    {
      try
      {
        Tracing.Log(DataServicesManager.sw, TraceLevel.Verbose, nameof (DataServicesManager), "Initialize the data services assembly");
        Tracing.Log(DataServicesManager.sw, TraceLevel.Verbose, nameof (DataServicesManager), "Data validation passed");
        WaitCallback callBack = new WaitCallback(DataServicesManager.BeginUpdate);
        string guid = Session.LoanDataMgr.LoanData.GUID;
        Tracing.Log(DataServicesManager.sw, TraceLevel.Verbose, nameof (DataServicesManager), "Queue data services task for loanID: " + guid);
        Hashtable dataServicesData = Session.InitialDataServicesData;
        ThreadPool.QueueUserWorkItem(callBack, (object) dataServicesData);
      }
      catch (Exception ex)
      {
        Tracing.Log(DataServicesManager.sw, nameof (DataServicesManager), TraceLevel.Error, "UpdateReport: " + ex.Message);
      }
    }

    private static void BeginUpdate(object stateInfo)
    {
      LoanDataMgr loanDataMgr = (LoanDataMgr) null;
      try
      {
        Tracing.Log(DataServicesManager.sw, TraceLevel.Verbose, nameof (DataServicesManager), "Open the loan in read-only mode");
        Hashtable compareData = (Hashtable) stateInfo;
        loanDataMgr = LoanDataMgr.OpenLoan(Session.SessionObjects, string.Concat(compareData[(object) "LOANGUID"]), false, 1, false);
        Tracing.Log(DataServicesManager.sw, TraceLevel.Verbose, nameof (DataServicesManager), "Initialize the data services assembly, and perform data services.");
        IDataServices dataServices = DataServicesManager.initializeDataServices(loanDataMgr);
        if (!dataServices.DataValidation() || dataServices.Compare(compareData))
          return;
        dataServices.UpdateReport(dataServices.GetData());
      }
      catch (Exception ex)
      {
        Tracing.Log(DataServicesManager.sw, nameof (DataServicesManager), TraceLevel.Error, "Automated Data Services: " + ex.Message);
      }
      finally
      {
        loanDataMgr?.Close();
      }
    }

    private static IDataServices initializeDataServices(LoanDataMgr loanDataMgr)
    {
      string str = SystemSettings.EpassDataDir + "Encompass.DataServices.dll";
      string fullName1 = AssemblyName.GetAssemblyName(str).FullName;
      Tracing.Log(DataServicesManager.sw, TraceLevel.Verbose, nameof (DataServicesManager), "Get the data services assembly name: " + fullName1);
      Assembly assembly1 = (Assembly) null;
      foreach (Assembly assembly2 in AppDomain.CurrentDomain.GetAssemblies())
      {
        if (assembly2.FullName == fullName1)
          assembly1 = assembly2;
      }
      if (assembly1 == (Assembly) null)
      {
        FileStream fileStream = File.OpenRead(str);
        byte[] numArray = new byte[fileStream.Length];
        fileStream.Read(numArray, 0, numArray.Length);
        fileStream.Close();
        Tracing.Log(DataServicesManager.sw, TraceLevel.Verbose, nameof (DataServicesManager), "Load the data services assembly");
        assembly1 = Assembly.Load(numArray);
      }
      string fullName2 = typeof (IDataServices).FullName;
      string typeName = (string) null;
      foreach (Type type in assembly1.GetTypes())
      {
        if (type.GetInterface(fullName2) != (Type) null)
          typeName = type.FullName;
      }
      Tracing.Log(DataServicesManager.sw, TraceLevel.Verbose, nameof (DataServicesManager), "CreateInstance of the class that implemented the IDataServices interface");
      IDataServices instance = (IDataServices) assembly1.CreateInstance(typeName);
      instance.Bam = (IBam) new Bam(loanDataMgr);
      return instance;
    }

    public static Hashtable RetrieveReportData(LoanDataMgr loanDataMgr)
    {
      Hashtable hashtable = new Hashtable();
      try
      {
        Tracing.Log(DataServicesManager.sw, TraceLevel.Verbose, nameof (DataServicesManager), "Initialize the data services assembly");
        hashtable = DataServicesManager.initializeDataServices(loanDataMgr).GetData();
      }
      catch (Exception ex)
      {
        Tracing.Log(DataServicesManager.sw, nameof (DataServicesManager), TraceLevel.Error, "Data Services: " + ex.Message);
      }
      return hashtable;
    }
  }
}
