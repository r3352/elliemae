// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.WebServices.DataServices2
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;

#nullable disable
namespace EllieMae.EMLite.WebServices
{
  [GeneratedCode("wsdl", "2.0.50727.42")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [WebServiceBinding(Name = "DataServices2Soap", Namespace = "http://dataservices.elliemaeservices.com/")]
  public class DataServices2 : SoapHttpClientProtocol
  {
    private SendOrPostCallback UploadLoanDataOperationCompleted;
    private SendOrPostCallback UploadNMLSReportOperationCompleted;
    private SendOrPostCallback LogExportServicesOperationCompleted;
    private SendOrPostCallback LogULDDExportOperationCompleted;

    public DataServices2(string dataServices2Url)
    {
      if (string.IsNullOrWhiteSpace(dataServices2Url) || !Uri.IsWellFormedUriString(dataServices2Url, UriKind.Absolute))
        this.Url = "https://dataservices.elliemaeservices.com/dataservices2.asmx";
      else
        this.Url = dataServices2Url;
    }

    public DataServices2()
    {
      this.Url = "https://dataservices.elliemaeservices.com/dataservices2.asmx";
    }

    public event UploadLoanDataCompletedEventHandler UploadLoanDataCompleted;

    public event UploadNMLSReportCompletedEventHandler UploadNMLSReportCompleted;

    public event LogExportServicesCompletedEventHandler LogExportServicesCompleted;

    public event LogULDDExportCompletedEventHandler LogULDDExportCompleted;

    [SoapDocumentMethod("http://dataservices.elliemaeservices.com/UploadLoanData", RequestNamespace = "http://dataservices.elliemaeservices.com/", ResponseNamespace = "http://dataservices.elliemaeservices.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public void UploadLoanData(Loan2 loan)
    {
      this.Invoke(nameof (UploadLoanData), new object[1]
      {
        (object) loan
      });
    }

    public IAsyncResult BeginUploadLoanData(Loan2 loan, AsyncCallback callback, object asyncState)
    {
      return this.BeginInvoke("UploadLoanData", new object[1]
      {
        (object) loan
      }, callback, asyncState);
    }

    public void EndUploadLoanData(IAsyncResult asyncResult) => this.EndInvoke(asyncResult);

    public void UploadLoanDataAsync(Loan2 loan) => this.UploadLoanDataAsync(loan, (object) null);

    public void UploadLoanDataAsync(Loan2 loan, object userState)
    {
      if (this.UploadLoanDataOperationCompleted == null)
        this.UploadLoanDataOperationCompleted = new SendOrPostCallback(this.OnUploadLoanDataOperationCompleted);
      this.InvokeAsync("UploadLoanData", new object[1]
      {
        (object) loan
      }, this.UploadLoanDataOperationCompleted, userState);
    }

    private void OnUploadLoanDataOperationCompleted(object arg)
    {
      if (this.UploadLoanDataCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.UploadLoanDataCompleted((object) this, new AsyncCompletedEventArgs(completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://dataservices.elliemaeservices.com/UploadNMLSReport", RequestNamespace = "http://dataservices.elliemaeservices.com/", ResponseNamespace = "http://dataservices.elliemaeservices.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public void UploadNMLSReport(
      string clientID,
      string reportFormat,
      string[] userStates,
      string[] loanFolders,
      string reportXml)
    {
      this.Invoke(nameof (UploadNMLSReport), new object[5]
      {
        (object) clientID,
        (object) reportFormat,
        (object) userStates,
        (object) loanFolders,
        (object) reportXml
      });
    }

    public IAsyncResult BeginUploadNMLSReport(
      string clientID,
      string reportFormat,
      string[] userStates,
      string[] loanFolders,
      string reportXml,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("UploadNMLSReport", new object[5]
      {
        (object) clientID,
        (object) reportFormat,
        (object) userStates,
        (object) loanFolders,
        (object) reportXml
      }, callback, asyncState);
    }

    public void EndUploadNMLSReport(IAsyncResult asyncResult) => this.EndInvoke(asyncResult);

    public void UploadNMLSReportAsync(
      string clientID,
      string reportFormat,
      string[] userStates,
      string[] loanFolders,
      string reportXml)
    {
      this.UploadNMLSReportAsync(clientID, reportFormat, userStates, loanFolders, reportXml, (object) null);
    }

    public void UploadNMLSReportAsync(
      string clientID,
      string reportFormat,
      string[] userStates,
      string[] loanFolders,
      string reportXml,
      object userState)
    {
      if (this.UploadNMLSReportOperationCompleted == null)
        this.UploadNMLSReportOperationCompleted = new SendOrPostCallback(this.OnUploadNMLSReportOperationCompleted);
      this.InvokeAsync("UploadNMLSReport", new object[5]
      {
        (object) clientID,
        (object) reportFormat,
        (object) userStates,
        (object) loanFolders,
        (object) reportXml
      }, this.UploadNMLSReportOperationCompleted, userState);
    }

    private void OnUploadNMLSReportOperationCompleted(object arg)
    {
      if (this.UploadNMLSReportCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.UploadNMLSReportCompleted((object) this, new AsyncCompletedEventArgs(completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://dataservices.elliemaeservices.com/LogExportServices", RequestNamespace = "http://dataservices.elliemaeservices.com/", ResponseNamespace = "http://dataservices.elliemaeservices.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public void LogExportServices(
      string clientID,
      string userID,
      string exportType,
      string[] loanGuids,
      string misc)
    {
      this.Invoke(nameof (LogExportServices), new object[5]
      {
        (object) clientID,
        (object) userID,
        (object) exportType,
        (object) loanGuids,
        (object) misc
      });
    }

    public IAsyncResult BeginLogExportServices(
      string clientID,
      string userID,
      string exportType,
      string[] loanGuids,
      string misc,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("LogExportServices", new object[5]
      {
        (object) clientID,
        (object) userID,
        (object) exportType,
        (object) loanGuids,
        (object) misc
      }, callback, asyncState);
    }

    public void EndLogExportServices(IAsyncResult asyncResult) => this.EndInvoke(asyncResult);

    public void LogExportServicesAsync(
      string clientID,
      string userID,
      string exportType,
      string[] loanGuids,
      string misc)
    {
      this.LogExportServicesAsync(clientID, userID, exportType, loanGuids, misc, (object) null);
    }

    public void LogExportServicesAsync(
      string clientID,
      string userID,
      string exportType,
      string[] loanGuids,
      string misc,
      object userState)
    {
      if (this.LogExportServicesOperationCompleted == null)
        this.LogExportServicesOperationCompleted = new SendOrPostCallback(this.OnLogExportServicesOperationCompleted);
      this.InvokeAsync("LogExportServices", new object[5]
      {
        (object) clientID,
        (object) userID,
        (object) exportType,
        (object) loanGuids,
        (object) misc
      }, this.LogExportServicesOperationCompleted, userState);
    }

    private void OnLogExportServicesOperationCompleted(object arg)
    {
      if (this.LogExportServicesCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.LogExportServicesCompleted((object) this, new AsyncCompletedEventArgs(completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://dataservices.elliemaeservices.com/LogULDDExport", RequestNamespace = "http://dataservices.elliemaeservices.com/", ResponseNamespace = "http://dataservices.elliemaeservices.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public void LogULDDExport(
      string clientID,
      string userID,
      string exportType,
      string[] loanGuids)
    {
      this.Invoke(nameof (LogULDDExport), new object[4]
      {
        (object) clientID,
        (object) userID,
        (object) exportType,
        (object) loanGuids
      });
    }

    public IAsyncResult BeginLogULDDExport(
      string clientID,
      string userID,
      string exportType,
      string[] loanGuids,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("LogULDDExport", new object[4]
      {
        (object) clientID,
        (object) userID,
        (object) exportType,
        (object) loanGuids
      }, callback, asyncState);
    }

    public void EndLogULDDExport(IAsyncResult asyncResult) => this.EndInvoke(asyncResult);

    public void LogULDDExportAsync(
      string clientID,
      string userID,
      string exportType,
      string[] loanGuids)
    {
      this.LogULDDExportAsync(clientID, userID, exportType, loanGuids, (object) null);
    }

    public void LogULDDExportAsync(
      string clientID,
      string userID,
      string exportType,
      string[] loanGuids,
      object userState)
    {
      if (this.LogULDDExportOperationCompleted == null)
        this.LogULDDExportOperationCompleted = new SendOrPostCallback(this.OnLogULDDExportOperationCompleted);
      this.InvokeAsync("LogULDDExport", new object[4]
      {
        (object) clientID,
        (object) userID,
        (object) exportType,
        (object) loanGuids
      }, this.LogULDDExportOperationCompleted, userState);
    }

    private void OnLogULDDExportOperationCompleted(object arg)
    {
      if (this.LogULDDExportCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.LogULDDExportCompleted((object) this, new AsyncCompletedEventArgs(completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    public new void CancelAsync(object userState) => base.CancelAsync(userState);
  }
}
