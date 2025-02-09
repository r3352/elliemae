// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.DisclosureTrackingLogUtils
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class DisclosureTrackingLogUtils
  {
    private static string fileName = "DisclosureTrackingLog.txt";
    private static string openingRecord = "<Record>";
    private static string closingRecord = "</Record>";
    private static string openingEvent = "<Event>";
    private static string closingEvent = "</Event>";
    private const string className = "DisclosureTrackingLogUtils�";
    private static readonly string sw = Tracing.SwDataEngine;

    public static void WriteLog(
      LoanDataMgr loanDataMgr,
      string userID,
      string message,
      string guidString)
    {
      string str = "";
      try
      {
        byte[] bytes = Encoding.UTF8.GetBytes(DisclosureTrackingLogUtils.openingEvent + "<Guid>" + guidString + "</Guid><CreatedDTTM>" + DateTime.Now.ToUniversalTime().ToString("u") + "</CreatedDTTM><Message>" + message + "</Message>" + DisclosureTrackingLogUtils.closingEvent);
        int num = 0;
        while (num < 5)
        {
          ++num;
          try
          {
            if (loanDataMgr.GetSupportingData(DisclosureTrackingLogUtils.fileName) == null)
              loanDataMgr.SaveSupportingData(DisclosureTrackingLogUtils.fileName, new BinaryObject(bytes));
            else
              loanDataMgr.AppendSupportingData(DisclosureTrackingLogUtils.fileName, new BinaryObject(bytes));
            str = "";
            num = 6;
          }
          catch (Exception ex)
          {
            str = ex.Message;
          }
        }
      }
      catch (Exception ex)
      {
        str = ex.Message;
      }
      if (!(str != ""))
        return;
      Tracing.Log(DisclosureTrackingLogUtils.sw, nameof (DisclosureTrackingLogUtils), TraceLevel.Error, "Error writing to DisclosureTrackingLog.txt file: " + str + Environment.NewLine);
    }

    public static void WriteLog(
      LoanDataMgr loanDataMgr,
      string clientID,
      string userID,
      string guidString,
      string disclosureGuid,
      string packageID,
      string Message)
    {
      string str = "";
      string key = "DTtemplog_" + guidString + ".txt";
      try
      {
        Encoding utF8 = Encoding.UTF8;
        string[] strArray = new string[17];
        strArray[0] = DisclosureTrackingLogUtils.openingEvent;
        strArray[1] = "<Guid>";
        strArray[2] = guidString;
        strArray[3] = "</Guid><CreatedDTTM>";
        DateTime dateTime = DateTime.Now;
        dateTime = dateTime.ToUniversalTime();
        strArray[4] = dateTime.ToString("u");
        strArray[5] = "</CreatedDTTM><ClientID>";
        strArray[6] = clientID;
        strArray[7] = "</ClientID><DisclosureGuid>";
        strArray[8] = disclosureGuid;
        strArray[9] = "</DisclosureGuid><UserID>";
        strArray[10] = userID;
        strArray[11] = "</UserID><PackageID>";
        strArray[12] = packageID;
        strArray[13] = "</PackageID><Message>";
        strArray[14] = Message;
        strArray[15] = "</Message>";
        strArray[16] = DisclosureTrackingLogUtils.closingEvent;
        string s = string.Concat(strArray);
        byte[] bytes = utF8.GetBytes(s);
        int num = 0;
        while (num < 5)
        {
          ++num;
          try
          {
            if (loanDataMgr.GetSupportingData(key) == null)
              loanDataMgr.SaveSupportingData(key, new BinaryObject(bytes));
            else
              loanDataMgr.AppendSupportingData(key, new BinaryObject(bytes));
            str = "";
            num = 6;
          }
          catch (Exception ex)
          {
            str = ex.Message;
          }
        }
      }
      catch (Exception ex)
      {
        str = ex.Message;
      }
      if (!(str != ""))
        return;
      Tracing.Log(DisclosureTrackingLogUtils.sw, nameof (DisclosureTrackingLogUtils), TraceLevel.Error, "Error writing to " + key + " file: " + str + Environment.NewLine);
    }

    public static void WriteLog(
      Dictionary<string, string> dataList,
      LoanDataMgr loanDataMgr,
      string userID,
      Guid eDisclosurePDFGuid,
      bool isInteractive,
      string recordGuid)
    {
      if (loanDataMgr == null)
        return;
      string str1 = string.Empty;
      string str2 = "";
      try
      {
        DisclosureTrackingLogUtils.fillGenericData(loanDataMgr, userID, dataList);
        if (eDisclosurePDFGuid == Guid.Empty)
          dataList.Add("ErrorCreatingPDF", "Yes");
        foreach (string key in dataList.Keys)
          str1 = str1 + "<" + key + ">" + dataList[key] + "</" + key + ">";
        if (str1 != string.Empty)
        {
          Encoding utF8 = Encoding.UTF8;
          string[] strArray = new string[6]
          {
            DisclosureTrackingLogUtils.openingRecord,
            "<CreatedDTTM>",
            null,
            null,
            null,
            null
          };
          DateTime dateTime = DateTime.Now;
          dateTime = dateTime.ToUniversalTime();
          strArray[2] = dateTime.ToString("u");
          strArray[3] = "</CreatedDTTM>";
          strArray[4] = str1;
          strArray[5] = DisclosureTrackingLogUtils.closingRecord;
          string s = string.Concat(strArray);
          byte[] bytes = utF8.GetBytes(s);
          int num = 0;
          while (num < 5)
          {
            ++num;
            try
            {
              if (loanDataMgr.GetSupportingData(DisclosureTrackingLogUtils.fileName) == null)
                loanDataMgr.SaveSupportingData(DisclosureTrackingLogUtils.fileName, new BinaryObject(bytes));
              else
                loanDataMgr.AppendSupportingData(DisclosureTrackingLogUtils.fileName, new BinaryObject(bytes));
              str2 = "";
              num = 6;
            }
            catch (SecurityException ex)
            {
              num = 6;
              str2 = "";
            }
            catch (Exception ex)
            {
              str2 = ex.Message;
            }
          }
        }
      }
      catch (Exception ex)
      {
        str2 = str2 + Environment.NewLine + ex.Message + Environment.NewLine + "; Data to write:" + str1;
      }
      if (str2 != "")
        Tracing.Log(DisclosureTrackingLogUtils.sw, nameof (DisclosureTrackingLogUtils), TraceLevel.Error, "Error writing to DisclosureTrackingLog.txt file: " + str2);
      if (!isInteractive || !(str2 != "") && !(eDisclosurePDFGuid == Guid.Empty))
        return;
      int num1 = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "Error recording disclosure tracking record.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }

    public static void WriteLog(
      Dictionary<string, string> dataList,
      LoanDataMgr loanDataMgr,
      string userID,
      string pdfFilePath,
      string pdfFileName,
      string pdfErrorMsg,
      bool isInteractive,
      string recordGuid)
    {
      if (loanDataMgr == null)
        return;
      string str1 = string.Empty;
      string str2 = "";
      try
      {
        DisclosureTrackingLogUtils.fillGenericData(loanDataMgr, userID, dataList);
        if (pdfErrorMsg != string.Empty)
          dataList.Add("ErrorCreatingPDF", pdfErrorMsg);
        foreach (string key in dataList.Keys)
          str1 = str1 + "<" + key + ">" + dataList[key] + "</" + key + ">";
        if (str1 != string.Empty)
        {
          Encoding utF8 = Encoding.UTF8;
          string[] strArray = new string[6]
          {
            DisclosureTrackingLogUtils.openingRecord,
            "<CreatedDTTM>",
            null,
            null,
            null,
            null
          };
          DateTime dateTime = DateTime.Now;
          dateTime = dateTime.ToUniversalTime();
          strArray[2] = dateTime.ToString("u");
          strArray[3] = "</CreatedDTTM>";
          strArray[4] = str1;
          strArray[5] = DisclosureTrackingLogUtils.closingRecord;
          string s = string.Concat(strArray);
          byte[] bytes = utF8.GetBytes(s);
          int num = 0;
          while (num < 5)
          {
            ++num;
            try
            {
              if (loanDataMgr.GetSupportingData(DisclosureTrackingLogUtils.fileName) == null)
                loanDataMgr.SaveSupportingData(DisclosureTrackingLogUtils.fileName, new BinaryObject(bytes));
              else
                loanDataMgr.AppendSupportingData(DisclosureTrackingLogUtils.fileName, new BinaryObject(bytes));
              str2 = "";
              num = 6;
            }
            catch (SecurityException ex)
            {
              num = 6;
              str2 = "";
            }
            catch (Exception ex)
            {
              str2 = ex.Message;
            }
          }
        }
      }
      catch (Exception ex)
      {
        str2 = str2 + Environment.NewLine + ex.Message + Environment.NewLine + "; Data to write:" + str1;
      }
      if (str2 != "")
        Tracing.Log(DisclosureTrackingLogUtils.sw, nameof (DisclosureTrackingLogUtils), TraceLevel.Error, "Error writing to DisclosureTrackingLog.txt file: " + str2);
      if (!isInteractive || !(str2 != "") && !(pdfErrorMsg != ""))
        return;
      int num1 = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "Error recording disclosure tracking record.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }

    public static string GetLog(IDisclosureTrackingLog actualLog)
    {
      XmlElement element = new XmlDocument().CreateElement("LogData");
      if (actualLog is EnhancedDisclosureTracking2015Log)
        ((LogRecordBase) actualLog).ToXml(element);
      else
        ((LogRecordBase) actualLog).ToXml(element);
      return element.OuterXml;
    }

    private static void fillGenericData(
      LoanDataMgr dataMgr,
      string userID,
      Dictionary<string, string> logData)
    {
      try
      {
        logData.Add("UserID", userID);
        logData.Add("Version", "8.1");
        logData.Add("LoanInfo.Right", Enum.GetName(typeof (LoanInfo.Right), (object) dataMgr.GetEffectiveRight(userID)));
        logData.Add("ContentAccess", Enum.GetName(typeof (LoanContentAccess), (object) dataMgr.LoanData.ContentAccess));
        if (dataMgr.LoanData.ContentAccess != LoanContentAccess.FullAccess && dataMgr.LoanData.ContentAccess != LoanContentAccess.None)
          logData.Add("ModuleAccess", LoanAccess.GetAccessRightMessage(dataMgr.LoanData.ContentAccess));
        logData.Add("UseNewRESPA", dataMgr.LoanData.Use2010RESPA ? "Yes" : "No");
      }
      catch (Exception ex)
      {
        logData.Add("ErrorCreatingLog", ex.Message);
      }
    }

    public static bool ContainsLE(
      List<DisclosureTrackingFormItem> formList,
      bool iseDisclosed,
      DocumentTrackingSetup docSetup)
    {
      return DisclosureTrackingLogUtils.ContainsIndicator(formList, (iseDisclosed ? 1 : 0) != 0, docSetup, (IEnumerable<string>) DisclosureTrackingConsts.LEFormNames, new DisclosureTrackingFormItem.FormType[1]
      {
        DisclosureTrackingFormItem.FormType.eDisclosure
      });
    }

    public static bool ContainsCD(
      List<DisclosureTrackingFormItem> formList,
      bool iseDisclosed,
      DocumentTrackingSetup docSetup)
    {
      return DisclosureTrackingLogUtils.ContainsIndicator(formList, (iseDisclosed ? 1 : 0) != 0, docSetup, (IEnumerable<string>) DisclosureTrackingConsts.CDFormNames, new DisclosureTrackingFormItem.FormType[2]
      {
        DisclosureTrackingFormItem.FormType.eDisclosure,
        DisclosureTrackingFormItem.FormType.ClosingDocsOrder
      });
    }

    public static bool ContainsSSPL(
      List<DisclosureTrackingFormItem> formList,
      bool iseDisclosed,
      DocumentTrackingSetup docSetup,
      bool isNoFee)
    {
      return !isNoFee ? DisclosureTrackingLogUtils.ContainsIndicator(formList, (iseDisclosed ? 1 : 0) != 0, docSetup, (IEnumerable<string>) new string[1]
      {
        DisclosureTrackingConsts.SSPLFormNames[0]
      }, new DisclosureTrackingFormItem.FormType[1]
      {
        DisclosureTrackingFormItem.FormType.eDisclosure
      }) : DisclosureTrackingLogUtils.ContainsIndicator(formList, (iseDisclosed ? 1 : 0) != 0, docSetup, (IEnumerable<string>) new string[1]
      {
        DisclosureTrackingConsts.SSPLFormNames[1]
      }, new DisclosureTrackingFormItem.FormType[1]
      {
        DisclosureTrackingFormItem.FormType.eDisclosure
      });
    }

    public static bool ContainsSafeHarbor2015(
      List<DisclosureTrackingFormItem> formList,
      bool iseDisclosed,
      DocumentTrackingSetup docSetup)
    {
      return DisclosureTrackingLogUtils.ContainsIndicator(formList, (iseDisclosed ? 1 : 0) != 0, docSetup, (IEnumerable<string>) DisclosureTrackingConsts.SafeHarbor2015, new DisclosureTrackingFormItem.FormType[1]
      {
        DisclosureTrackingFormItem.FormType.eDisclosure
      });
    }

    private static bool ContainsIndicator(
      List<DisclosureTrackingFormItem> formList,
      bool iseDisclosed,
      DocumentTrackingSetup docSetup,
      IEnumerable<string> allowedFormNames,
      DisclosureTrackingFormItem.FormType[] formTypes)
    {
      if (formList != null)
      {
        foreach (DisclosureTrackingFormItem form in formList)
        {
          if (form != null)
          {
            if ((!iseDisclosed || ((IEnumerable<DisclosureTrackingFormItem.FormType>) formTypes).Contains<DisclosureTrackingFormItem.FormType>(form.OutputFormType)) && allowedFormNames.Contains<string>(form.FormName))
              return true;
            if (docSetup != null)
            {
              foreach (DocumentTemplate documentTemplate in docSetup)
              {
                if (documentTemplate != null && form.DocumentTemplateGuid == documentTemplate.Guid && allowedFormNames.Contains<string>(documentTemplate.Source))
                  return true;
              }
            }
          }
        }
      }
      return false;
    }

    public static DisclosureTrackingFormItem GetForm(
      string formName,
      DisclosureTrackingFormItem.FormSignatureType signatureType,
      DisclosureTrackingFormItem.FormType formType,
      DocumentTrackingSetup docSetup,
      DisclosureTrackingBase.DisclosedMethod method,
      out bool containsLE,
      out bool containsCD,
      out bool containsSSPL,
      out bool containsSSPLNoFee,
      out bool containsSafeHarbor2015)
    {
      DocumentTemplate byName = docSetup.GetByName(formName);
      if (formType == DisclosureTrackingFormItem.FormType.None)
        formType = byName != null ? (!(byName.SourceType == "Standard Form") ? (!(byName.SourceType == "Custom Form") ? (!(byName.SourceType == "Borrower Specific Custom Form") ? (!(byName.SourceType == "Needed") ? (method == DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder || method == DisclosureTrackingBase.DisclosedMethod.eClose ? DisclosureTrackingFormItem.FormType.ClosingDocsOrder : DisclosureTrackingFormItem.FormType.None) : DisclosureTrackingFormItem.FormType.Needed) : DisclosureTrackingFormItem.FormType.CustomForm) : DisclosureTrackingFormItem.FormType.CustomForm) : DisclosureTrackingFormItem.FormType.StandardForm) : DisclosureTrackingFormItem.FormType.eDisclosure;
      string documentTemplateGuid = byName != null ? byName.Guid : string.Empty;
      containsLE = DisclosureTrackingConsts.LEFormNames.Contains(formName) || byName != null && DisclosureTrackingConsts.LEFormNames.Contains(byName.Source);
      containsCD = DisclosureTrackingConsts.CDFormNames.Contains(formName) || byName != null && DisclosureTrackingConsts.CDFormNames.Contains(byName.Source);
      containsSSPL = DisclosureTrackingConsts.SSPLFormNames[0].Equals(formName) || byName != null && DisclosureTrackingConsts.SSPLFormNames[0].Equals(byName.Source);
      containsSSPLNoFee = DisclosureTrackingConsts.SSPLFormNames[1].Equals(formName) || byName != null && DisclosureTrackingConsts.SSPLFormNames[1].Equals(byName.Source);
      containsSafeHarbor2015 = DisclosureTrackingConsts.SafeHarbor2015.Contains(formName) || byName != null && DisclosureTrackingConsts.SafeHarbor2015.Contains(byName.Source);
      return new DisclosureTrackingFormItem(formName, formType, documentTemplateGuid, signatureType);
    }

    public static IList<DisclosureTrackingFormItem> GetDocuments(
      EnhancedDisclosureTracking2015Log.DisclosureContentType content)
    {
      List<DisclosureTrackingFormItem> documents = new List<DisclosureTrackingFormItem>();
      List<string> stringList = new List<string>();
      switch (content)
      {
        case EnhancedDisclosureTracking2015Log.DisclosureContentType.LE:
          stringList.AddRange((IEnumerable<string>) DisclosureTrackingConsts.LEFormNames);
          break;
        case EnhancedDisclosureTracking2015Log.DisclosureContentType.CD:
          stringList.AddRange((IEnumerable<string>) DisclosureTrackingConsts.CDFormNames);
          break;
        case EnhancedDisclosureTracking2015Log.DisclosureContentType.ServiceProviderList:
          stringList.Add(DisclosureTrackingConsts.SSPLFormNames[0]);
          break;
        case EnhancedDisclosureTracking2015Log.DisclosureContentType.ServiceProviderListNoFee:
          stringList.Add(DisclosureTrackingConsts.SSPLFormNames[1]);
          break;
        case EnhancedDisclosureTracking2015Log.DisclosureContentType.SafeHarbor:
          stringList.AddRange((IEnumerable<string>) DisclosureTrackingConsts.NewSafeHarborFormNames);
          break;
      }
      foreach (string formName in stringList)
        documents.Add(new DisclosureTrackingFormItem(formName, DisclosureTrackingFormItem.FormType.StandardForm));
      return (IList<DisclosureTrackingFormItem>) documents;
    }

    private static Dictionary<string, int> borrowerPairIDsDistribution(
      LoanData loanData,
      DisclosureTracking2015Log.DisclosureTrackingType type)
    {
      IDisclosureTracking2015Log[] idisclosureTracking2015Log = loanData.GetLogList().GetAllIDisclosureTracking2015Log(true);
      Dictionary<string, int> dictionary = new Dictionary<string, int>();
      foreach (IDisclosureTracking2015Log disclosureTracking2015Log in idisclosureTracking2015Log)
      {
        if (type == DisclosureTracking2015Log.DisclosureTrackingType.LE && disclosureTracking2015Log.DisclosedForLE || type == DisclosureTracking2015Log.DisclosureTrackingType.CD && disclosureTracking2015Log.DisclosedForCD)
        {
          string[] strArray;
          switch (disclosureTracking2015Log)
          {
            case DisclosureTracking2015Log _:
              strArray = new string[1]
              {
                disclosureTracking2015Log.BorrowerPairID
              };
              break;
            case EnhancedDisclosureTracking2015Log _:
              strArray = (disclosureTracking2015Log as EnhancedDisclosureTracking2015Log).DisclosureRecipients.Where<EnhancedDisclosureTracking2015Log.DisclosureRecipient>((Func<EnhancedDisclosureTracking2015Log.DisclosureRecipient, bool>) (dr => dr.Role == EnhancedDisclosureTracking2015Log.DisclosureRecipientType.Borrower)).Select<EnhancedDisclosureTracking2015Log.DisclosureRecipient, string>((Func<EnhancedDisclosureTracking2015Log.DisclosureRecipient, string>) (dr => (dr as EnhancedDisclosureTracking2015Log.BorrowerRecipient).BorrowerPairId)).ToArray<string>();
              break;
            default:
              continue;
          }
          foreach (string key in strArray)
          {
            if (dictionary.ContainsKey(key))
              dictionary[key]++;
            else
              dictionary.Add(key, 1);
          }
        }
      }
      return dictionary;
    }

    public static DisclosureTracking2015Log.DisclosureTypeEnum GetDisclosureTracking2015LogType(
      LoanData loanData,
      IDisclosureTracking2015Log log)
    {
      Dictionary<string, int> dictionary1 = DisclosureTrackingLogUtils.borrowerPairIDsDistribution(loanData, DisclosureTracking2015Log.DisclosureTrackingType.LE);
      Dictionary<string, int> dictionary2 = DisclosureTrackingLogUtils.borrowerPairIDsDistribution(loanData, DisclosureTracking2015Log.DisclosureTrackingType.CD);
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      string[] strArray = new string[0];
      switch (log)
      {
        case DisclosureTracking2015Log _:
          strArray = new string[1]
          {
            (log as DisclosureTracking2015Log).BorrowerPairID
          };
          break;
        case EnhancedDisclosureTracking2015Log _:
          strArray = (log as EnhancedDisclosureTracking2015Log).DisclosureRecipients.Where<EnhancedDisclosureTracking2015Log.DisclosureRecipient>((Func<EnhancedDisclosureTracking2015Log.DisclosureRecipient, bool>) (dr => dr.Role == EnhancedDisclosureTracking2015Log.DisclosureRecipientType.Borrower)).Select<EnhancedDisclosureTracking2015Log.DisclosureRecipient, string>((Func<EnhancedDisclosureTracking2015Log.DisclosureRecipient, string>) (dr => (dr as EnhancedDisclosureTracking2015Log.BorrowerRecipient).BorrowerPairId)).ToArray<string>();
          break;
      }
      foreach (string key in strArray)
      {
        if (log.DisclosedForCD)
        {
          if (loanData.GetLogList().GetLatestIDisclosureTracking2015Log(DisclosureTracking2015Log.DisclosureTrackingType.CD) != null && dictionary2.TryGetValue(key, out num1))
            ++num2;
          else
            ++num3;
        }
        else if (log.DisclosedForLE)
        {
          if (loanData.GetLogList().GetLatestIDisclosureTracking2015Log(DisclosureTracking2015Log.DisclosureTrackingType.LE) != null && dictionary1.TryGetValue(key, out num1))
            ++num2;
          else
            ++num3;
        }
      }
      DisclosureTracking2015Log.DisclosureTypeEnum tracking2015LogType = DisclosureTracking2015Log.DisclosureTypeEnum.None;
      if (num2 > 0)
        tracking2015LogType = DisclosureTracking2015Log.DisclosureTypeEnum.Revised;
      else if (num3 > 0)
        tracking2015LogType = DisclosureTracking2015Log.DisclosureTypeEnum.Initial;
      return tracking2015LogType;
    }

    public static void SetUseForUCDExport(IDisclosureTracking2015Log log, bool isSetForUCDExport)
    {
      if (isSetForUCDExport)
      {
        foreach (IDisclosureTracking2015Log disclosureTracking2015Log in log.LogLoanData.GetLogList().GetAllIDisclosureTracking2015Log(false, DisclosureTracking2015Log.DisclosureTrackingType.CD))
          disclosureTracking2015Log.UseForUCDExport = false;
      }
      log.UseForUCDExport = isSetForUCDExport;
    }
  }
}
