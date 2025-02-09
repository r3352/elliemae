// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.WebServices.EpassTransactionService
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.DataEngine;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.WebServices
{
  public class EpassTransactionService
  {
    private const string className = "EpassTransactionService�";
    private static readonly string sw = Tracing.SwEpass;
    private const string URL = "https://www.epassbusinesscenter.com/contour/logtransaction.asp�";
    private const string SECONDARYURL = "https://core.elliemaeservices.com/contour/logtransaction.asp�";
    private LoanDataMgr loanMgr;

    public EpassTransactionService(LoanDataMgr loanMgr) => this.loanMgr = loanMgr;

    public void SendTransactionLog(string vendorKeyword, string transID)
    {
      this.SendTransactionLog(vendorKeyword, transID, string.Empty);
    }

    public void SendTransactionLog(string vendorKeyword, string transID, string miscellaneous)
    {
      this.SendTransactionLog(this.loanMgr == null ? "" : this.loanMgr.SessionObjects.CompanyInfo.ClientID, this.loanMgr == null ? "" : this.loanMgr.SessionObjects.UserID, vendorKeyword, transID, miscellaneous);
    }

    public void SendTransactionLog(
      string clientID,
      string userID,
      string vendorKeyword,
      string transID,
      string miscellaneous)
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml("<TransactionLog/>");
      XmlElement documentElement = xmlDocument.DocumentElement;
      XmlElement element1 = xmlDocument.CreateElement("URLSignature");
      XmlElement element2 = xmlDocument.CreateElement("TransactionID");
      XmlElement element3 = xmlDocument.CreateElement("LOSName");
      XmlElement element4 = xmlDocument.CreateElement("LOSVersion");
      XmlElement element5 = xmlDocument.CreateElement("ClientID");
      XmlElement element6 = xmlDocument.CreateElement("UserName");
      XmlElement element7 = xmlDocument.CreateElement("LoanFileName");
      XmlElement element8 = xmlDocument.CreateElement("LoanGUID");
      XmlElement element9 = xmlDocument.CreateElement("LoanNumber");
      XmlElement element10 = xmlDocument.CreateElement("BorrowerName");
      XmlElement element11 = xmlDocument.CreateElement("PropertyAddress");
      XmlElement element12 = xmlDocument.CreateElement("LoanAmount");
      XmlElement element13 = xmlDocument.CreateElement("NoteRate");
      XmlElement element14 = xmlDocument.CreateElement("MortgageType");
      XmlElement element15 = xmlDocument.CreateElement("LoanPurpose");
      XmlElement element16 = xmlDocument.CreateElement("AmortizationType");
      XmlElement element17 = xmlDocument.CreateElement("AppraisedAmount");
      XmlElement element18 = xmlDocument.CreateElement("LienPosition");
      XmlElement element19 = xmlDocument.CreateElement("Miscellaneous");
      documentElement.AppendChild((XmlNode) element1);
      documentElement.AppendChild((XmlNode) element2);
      documentElement.AppendChild((XmlNode) element3);
      documentElement.AppendChild((XmlNode) element4);
      documentElement.AppendChild((XmlNode) element5);
      documentElement.AppendChild((XmlNode) element6);
      documentElement.AppendChild((XmlNode) element7);
      documentElement.AppendChild((XmlNode) element8);
      documentElement.AppendChild((XmlNode) element9);
      documentElement.AppendChild((XmlNode) element10);
      documentElement.AppendChild((XmlNode) element11);
      documentElement.AppendChild((XmlNode) element12);
      documentElement.AppendChild((XmlNode) element13);
      documentElement.AppendChild((XmlNode) element14);
      documentElement.AppendChild((XmlNode) element15);
      documentElement.AppendChild((XmlNode) element16);
      documentElement.AppendChild((XmlNode) element17);
      documentElement.AppendChild((XmlNode) element18);
      documentElement.AppendChild((XmlNode) element19);
      element1.InnerText = vendorKeyword;
      element2.InnerText = transID;
      element3.InnerText = "Encompass";
      element4.InnerText = VersionInformation.CurrentVersion.Version.FullVersion;
      element5.InnerText = clientID;
      element6.InnerText = userID;
      if (this.loanMgr != null)
      {
        element7.InnerText = this.loanMgr.LoanName;
        element8.InnerText = this.loanMgr.LoanData.GetSimpleField("GUID");
        element9.InnerText = this.loanMgr.LoanData.GetSimpleField("364");
        element10.InnerText = this.loanMgr.LoanData.GetSimpleField("36") + " " + this.loanMgr.LoanData.GetSimpleField("37");
        element11.InnerText = this.loanMgr.LoanData.GetSimpleField("11") + ", " + this.loanMgr.LoanData.GetSimpleField("12") + ", " + this.loanMgr.LoanData.GetSimpleField("14") + "  " + this.loanMgr.LoanData.GetSimpleField("15");
        element12.InnerText = this.loanMgr.LoanData.GetSimpleField("2");
        element13.InnerText = this.loanMgr.LoanData.GetSimpleField("3");
        element14.InnerText = this.loanMgr.LoanData.GetSimpleField("1172");
        element15.InnerText = this.loanMgr.LoanData.GetSimpleField("19");
        element16.InnerText = this.loanMgr.LoanData.GetSimpleField("608");
        element17.InnerText = this.loanMgr.LoanData.GetSimpleField("356");
        element18.InnerText = this.loanMgr.LoanData.GetSimpleField("420");
      }
      element19.InnerText = miscellaneous;
      if (element14.InnerText == "FarmersHomeAdministration")
        element14.InnerText = "FmHA";
      switch (element15.InnerText)
      {
        case "ConstructionToPermanent":
          element15.InnerText = "Construction-Permanent";
          break;
        case "ConstructionOnly":
          element15.InnerText = "Construction";
          break;
      }
      switch (element16.InnerText)
      {
        case "AdjustableRate":
          element16.InnerText = "ARM";
          break;
        case "GraduatedPaymentMortgage":
          element16.InnerText = "GPM";
          break;
        case "OtherAmortizationType":
          element16.InnerText = "Other";
          break;
      }
      switch (element18.InnerText)
      {
        case "FirstLien":
          element18.InnerText = "1st";
          break;
        case "SecondLien":
          element18.InnerText = "2nd";
          break;
        default:
          element18.InnerText = string.Empty;
          break;
      }
      string outerXml = xmlDocument.OuterXml;
      int num1 = 2;
      int num2 = 1;
      bool flag = false;
      string requestUriString = "https://www.epassbusinesscenter.com/contour/logtransaction.asp";
      for (; !flag && num2 <= num1; ++num2)
      {
        if (num2 == 2)
        {
          requestUriString = "https://core.elliemaeservices.com/contour/logtransaction.asp";
          Tracing.Log(EpassTransactionService.sw, TraceLevel.Info, nameof (EpassTransactionService), "Calling HA URL");
        }
        flag = true;
        try
        {
          HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(requestUriString);
          httpWebRequest.KeepAlive = false;
          httpWebRequest.Method = "POST";
          httpWebRequest.ContentType = "application/x-www-form-urlencoded";
          httpWebRequest.Timeout = 5000;
          using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
          {
            streamWriter.Write(outerXml);
            streamWriter.Close();
          }
          using (HttpWebResponse response = (HttpWebResponse) httpWebRequest.GetResponse())
          {
            using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
            {
              streamReader.ReadToEnd();
              streamReader.Close();
            }
          }
        }
        catch (Exception ex)
        {
          Tracing.Log(EpassTransactionService.sw, TraceLevel.Error, nameof (EpassTransactionService), ex.ToString());
          flag = false;
          if (num2 == num1)
            throw new Exception("An error occurred when trying to log the transaction", ex);
        }
      }
    }
  }
}
