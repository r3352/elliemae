// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.OutlookServices
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using AddinExpress.Outlook;
using Outlook;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class OutlookServices : IDisposable
  {
    private const string className = "OutlookServices";
    protected static string sw = Tracing.SwContact;
    private SecurityManager secManager = new SecurityManager();
    private _Application outlookApp;
    private bool exitOnDispose = true;

    public OutlookServices()
    {
      Type typeFromProgId = Type.GetTypeFromProgID("Outlook.Application", false);
      if (typeFromProgId == (Type) null)
        throw new System.Exception("Missing or invalid Outlook registry keys");
      string str = "Getting null Outlook instance";
      try
      {
        this.outlookApp = Marshal.GetActiveObject("Outlook.Application") as _Application;
        if (this.outlookApp != null)
          this.exitOnDispose = false;
      }
      catch (System.Exception ex)
      {
        str = ex.Message;
      }
      try
      {
        if (this.outlookApp == null)
          this.outlookApp = Activator.CreateInstance(typeFromProgId) as _Application;
      }
      catch (System.Exception ex)
      {
        throw new System.Exception("Failed to start Microsoft Outlook: \r\n\r\n" + str + "\r\n\r\n" + ex.Message, ex);
      }
      try
      {
        this.secManager.ConnectTo((object) this.outlookApp);
        this.secManager.DisableOOMWarnings = true;
      }
      catch (System.Exception ex)
      {
        Tracing.Log(OutlookServices.sw, nameof (OutlookServices), TraceLevel.Error, "Error connecting to Outlook application from Outlook Security Manager: " + ex.Message);
      }
    }

    public _Application ApplicationObject => this.outlookApp;

    public void Dispose()
    {
      if (this.outlookApp == null)
        return;
      try
      {
        this.secManager.DisableOOMWarnings = false;
      }
      catch (System.Exception ex)
      {
        Tracing.Log(OutlookServices.sw, nameof (OutlookServices), TraceLevel.Error, "Error resetting Outlook Security Manager: " + ex.Message);
      }
      try
      {
        if (this.exitOnDispose)
          this.outlookApp.Quit();
      }
      catch (System.Exception ex)
      {
        Tracing.Log(OutlookServices.sw, nameof (OutlookServices), TraceLevel.Error, "Error quiting Outlook application: " + ex.Message);
      }
      Marshal.ReleaseComObject((object) this.outlookApp);
      this.outlookApp = (_Application) null;
    }
  }
}
