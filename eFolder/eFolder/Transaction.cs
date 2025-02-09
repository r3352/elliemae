// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Transaction
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.eFolder.LoanCenter;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder
{
  public class Transaction
  {
    private const string className = "Transaction";
    private static readonly string sw = Tracing.SwEFolder;

    public static string GetEdition()
    {
      string edition = string.Empty;
      switch (Session.EncompassEdition)
      {
        case EncompassEdition.Broker:
          edition = "Broker";
          break;
        case EncompassEdition.Banker:
          edition = "Banker";
          break;
      }
      if (Session.SessionObjects.RuntimeEnvironment == RuntimeEnvironment.Hosted)
        edition = "Anywhere";
      return edition;
    }

    public static bool Log(LoanCenterMessage msg)
    {
      string actionType = (string) null;
      switch (msg.MsgType)
      {
        case LoanCenterMessageType.RequestDocuments:
          actionType = "Request";
          break;
        case LoanCenterMessageType.SendDocuments:
          actionType = "Send";
          break;
        case LoanCenterMessageType.InitialDisclosures:
          actionType = "Disclosures";
          break;
        case LoanCenterMessageType.SecureFormTransfer:
          return true;
        case LoanCenterMessageType.Consent:
          actionType = "Consent";
          break;
      }
      return Transaction.Log(msg.LoanDataMgr, actionType);
    }

    public static bool Log(LoanDataMgr loanDataMgr, string actionType)
    {
      LoanData loanData = loanDataMgr.LoanData;
      string edition = Transaction.GetEdition();
      string str1 = loanData.GetSimpleField("36") + " " + loanData.GetSimpleField("37");
      string str2;
      switch (loanData.GetSimpleField("420"))
      {
        case "FirstLien":
          str2 = "1";
          break;
        case "SecondLien":
          str2 = "2";
          break;
        default:
          str2 = string.Empty;
          break;
      }
      SortedList sortedList = new SortedList();
      sortedList.Add((object) "ClientID", (object) Session.CompanyInfo.ClientID);
      sortedList.Add((object) "UserID", (object) Session.UserID);
      sortedList.Add((object) "LoanID", (object) loanData.GetSimpleField("GUID"));
      sortedList.Add((object) "Edition", (object) edition);
      sortedList.Add((object) "BorrowerName", (object) str1);
      sortedList.Add((object) "PropertyAddress", (object) loanData.GetSimpleField("11"));
      sortedList.Add((object) "PropertyCity", (object) loanData.GetSimpleField("12"));
      sortedList.Add((object) "PropertyState", (object) loanData.GetSimpleField("14"));
      sortedList.Add((object) "PropertyZip", (object) loanData.GetSimpleField("15"));
      sortedList.Add((object) "AppraisedValue", (object) loanData.GetSimpleField("356"));
      sortedList.Add((object) "PurchasePrice", (object) loanData.GetSimpleField("136"));
      sortedList.Add((object) "LoanNumber", (object) loanData.GetSimpleField("364"));
      sortedList.Add((object) "LoanAmount", (object) loanData.GetSimpleField("2"));
      sortedList.Add((object) "InterestRate", (object) loanData.GetSimpleField("3"));
      sortedList.Add((object) "LoanType", (object) loanData.GetSimpleField("1172"));
      sortedList.Add((object) "AmortizationType", (object) loanData.GetSimpleField("608"));
      sortedList.Add((object) "LoanPurpose", (object) loanData.GetSimpleField("19"));
      sortedList.Add((object) "LienPosition", (object) str2);
      sortedList.Add((object) "ActionType", (object) actionType);
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string key in (IEnumerable) sortedList.Keys)
      {
        if (stringBuilder.Length != 0)
          stringBuilder.Append("&");
        string str3 = HttpUtility.UrlEncode(sortedList[(object) key].ToString());
        stringBuilder.Append(key + "=" + str3);
      }
      string str4 = Session.SessionObjects?.StartupInfo?.ServiceUrls?.LoanCenterLogTransactionUrl;
      if (string.IsNullOrWhiteSpace(str4) || !Uri.IsWellFormedUriString(str4, UriKind.Absolute))
        str4 = "https://loancenter.elliemae.com/efolder/logtransaction.aspx";
      bool flag = false;
      try
      {
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(str4);
        httpWebRequest.KeepAlive = false;
        httpWebRequest.Method = "POST";
        httpWebRequest.ContentType = "application/x-www-form-urlencoded";
        httpWebRequest.ContentLength = (long) stringBuilder.Length;
        using (Stream requestStream = httpWebRequest.GetRequestStream())
        {
          using (StreamWriter streamWriter = new StreamWriter(requestStream))
            streamWriter.Write(stringBuilder.ToString());
        }
        using (WebResponse response = httpWebRequest.GetResponse())
        {
          using (Stream responseStream = response.GetResponseStream())
          {
            using (StreamReader streamReader = new StreamReader(responseStream))
              streamReader.ReadToEnd();
          }
        }
        flag = true;
      }
      catch (Exception ex)
      {
        Tracing.Log(Transaction.sw, TraceLevel.Error, nameof (Transaction), ex.Message);
        int num = (int) Utils.Dialog((IWin32Window) null, "The following error occurred when trying to log the EDM transaction:\n\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      return flag;
    }
  }
}
