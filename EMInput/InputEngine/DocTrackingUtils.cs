// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.DocTrackingUtils
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class DocTrackingUtils
  {
    public static readonly string FieldPrefix = "CT.";
    public static readonly string InitialRequest_NextFollowupDate = "Warning - the document type shows that it has been received. Do you wish to override the receipt information and setup this document for Follow-up Requests?";
    public static readonly string InitialRequest_AddRequest = "Warning - This document has already been received. Do you wish to continue and override the prior request information?";
    public static readonly string InitialRequest_AddReceipt = "Warning - This document has already been received. Do you wish to continue and override the prior receipt information?";
    public static readonly string ShippingStatus_Override = "Warning - This document has already been shipped. Do you wish to continue and override the prior shipment information?";
    public static readonly string ShippingStatus_AddShipment_Override = "Warning. The selected document type already has a Shipping Date. Do you wish to override the prior Shipping information?";
    public static readonly string ReturnRequest_Override = "Warning - The document type shows that it has been received. Do you wish to override the receipt information and setup this document for Follow-up Requests?";
    public static readonly string ReturnReceipt_Override = "Warning - This document has already been received. Do you wish to continue and override the prior receipt information?";
    public static readonly string ReturnReceipt_AddReceipt_Override = "Warning - This document has already been received. Do you wish to continue and override the prior receipt information?";
    public static readonly string Email_Validation = "Email Address format is invalid.";
    public static readonly string Phone_Validation = "Phone Number format is invalid.";
    public static readonly string Final_Title_Policy = "Final Title Policy";
    public static readonly string DOT_Mortgage = "DOT/Mortgage";
    public static readonly string Executed_Note = "Executed Note";
    public static readonly string Action_Initial_Requested = "Initial Requested";
    public static readonly string Action_Initial_Received = "Initial Received";
    public static readonly string Action_Initial_ReceivedShipment = "Initial Received";
    public static readonly string Action_Sched_Shipment = "Sched. Shipment";
    public static readonly string Action_Shipped = "Shipped";
    public static readonly string Action_Return_Requested = "Return Requested";
    public static readonly string Action_Return_Received = "Return Received";
    public static readonly string Validate_Error_message = "Please correct data entry fields and resubmit.";
    public static readonly string DOTShippingStatusPrefix = DocTrackingUtils.FieldPrefix + "DOT.ShippingStatus.";
    public static readonly string FTPShippingStatusPrefix = DocTrackingUtils.FieldPrefix + "FTP.ShippingStatus.";
    public static readonly string CommentFiled = DocTrackingUtils.FieldPrefix + "Comment.AllComments";
    public static Hashtable ActionType = new Hashtable();

    public static bool IsCalculate { get; set; }

    public static DocumentTrackingManagement DocTrackingManagementForm { get; set; }

    public static Hashtable DocTrackingSettings { get; set; }

    public static IDictionary PoliciesSetting { get; set; }

    public static Sessions.Session Session { get; set; }

    public static bool IsImport { get; set; }

    static DocTrackingUtils()
    {
      ActionObject actionObject1 = new ActionObject()
      {
        Action = DocTrackingActionCd.ActionInitialRequested,
        Name = DocTrackingUtils.Action_Initial_Requested,
        IsBold = false,
        PopupClassName = "EllieMae.EMLite.InputEngine.AddInitialRequest"
      };
      DocTrackingUtils.ActionType[(object) DocTrackingUtils.Action_Initial_Requested] = (object) actionObject1;
      ActionObject actionObject2 = new ActionObject()
      {
        Action = DocTrackingActionCd.ActionInitialReceived,
        Name = DocTrackingUtils.Action_Initial_Received,
        IsBold = true,
        PopupClassName = "EllieMae.EMLite.InputEngine.AddInitialReceipt"
      };
      DocTrackingUtils.ActionType[(object) DocTrackingUtils.Action_Initial_Received] = (object) actionObject2;
      ActionObject actionObject3 = new ActionObject()
      {
        Action = DocTrackingActionCd.ActionInitialReceivedShipment,
        Name = DocTrackingUtils.Action_Initial_ReceivedShipment,
        IsBold = true,
        PopupClassName = "EllieMae.EMLite.InputEngine.AddInitialReceipt"
      };
      DocTrackingUtils.ActionType[(object) DocTrackingUtils.Action_Initial_ReceivedShipment] = (object) actionObject3;
      ActionObject actionObject4 = new ActionObject()
      {
        Action = DocTrackingActionCd.ActionSchedShipment,
        Name = DocTrackingUtils.Action_Sched_Shipment,
        IsBold = true,
        PopupClassName = "EllieMae.EMLite.InputEngine.AddInitialReceipt"
      };
      DocTrackingUtils.ActionType[(object) DocTrackingUtils.Action_Sched_Shipment] = (object) actionObject4;
      ActionObject actionObject5 = new ActionObject()
      {
        Action = DocTrackingActionCd.ActionShipped,
        Name = DocTrackingUtils.Action_Shipped,
        IsBold = true,
        PopupClassName = "EllieMae.EMLite.InputEngine.AddShipment"
      };
      DocTrackingUtils.ActionType[(object) DocTrackingUtils.Action_Shipped] = (object) actionObject5;
      ActionObject actionObject6 = new ActionObject()
      {
        Action = DocTrackingActionCd.ActionReturnRequested,
        Name = DocTrackingUtils.Action_Return_Requested,
        IsBold = false,
        PopupClassName = "EllieMae.EMLite.InputEngine.DocTracking.AddReturnRequest"
      };
      DocTrackingUtils.ActionType[(object) DocTrackingUtils.Action_Return_Requested] = (object) actionObject6;
      ActionObject actionObject7 = new ActionObject()
      {
        Action = DocTrackingActionCd.ActionReturnReceived,
        Name = DocTrackingUtils.Action_Return_Received,
        IsBold = true,
        PopupClassName = "EllieMae.EMLite.InputEngine.AddReturnReceipt"
      };
      DocTrackingUtils.ActionType[(object) DocTrackingUtils.Action_Return_Received] = (object) actionObject7;
    }

    public static void HilightFields(Control control, bool isHilight)
    {
      if (isHilight)
        control.BackColor = ColorTranslator.FromHtml("#FCDCDF");
      else if (control is CheckBox)
        control.BackColor = SystemColors.Control;
      else
        control.BackColor = SystemColors.Window;
    }

    public static string GetPrefix(
      DocTrackingType docTrackingType,
      DocTrackingRequestType requtestType)
    {
      switch (docTrackingType)
      {
        case DocTrackingType.FTP:
          return string.Format(DocTrackingUtils.FieldPrefix + "FTP.{0}.", (object) requtestType.ToString());
        case DocTrackingType.EN:
          return string.Format(DocTrackingUtils.FieldPrefix + "EN.{0}.", (object) requtestType.ToString());
        default:
          return string.Format(DocTrackingUtils.FieldPrefix + "DOT.{0}.", (object) requtestType.ToString());
      }
    }

    public static string GetFieldId(string prefix, Control control) => prefix + control.Tag;

    public static bool ValidateEmail(TextBox control, Control parent)
    {
      bool flag = true;
      string emailAddresses = control.Text.Trim();
      if (!string.IsNullOrEmpty(emailAddresses) && !Utils.ValidateEmail(emailAddresses))
      {
        int num = (int) Utils.Dialog((IWin32Window) parent, DocTrackingUtils.Email_Validation, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        flag = false;
      }
      return flag;
    }

    public static bool ValidatePhone(TextBox control, Control parent)
    {
      string orgval = control.Text.Trim();
      FieldFormat fieldFormat;
      FieldFormat dataFormat = fieldFormat = FieldFormat.PHONE;
      bool needsUpdate = false;
      string str = Utils.FormatInput(orgval, dataFormat, ref needsUpdate);
      if (needsUpdate)
      {
        control.Text = str;
        control.SelectionStart = str.Length;
      }
      return needsUpdate;
    }

    public static BusinessCalendar GetCalendar()
    {
      LockExpCalendarSetting expCalendarSetting = (LockExpCalendarSetting) DocTrackingUtils.PoliciesSetting[(object) "Policies.LockExpCalendar"];
      BusinessCalendar calendar = (BusinessCalendar) null;
      switch (expCalendarSetting)
      {
        case LockExpCalendarSetting.PostalCalendar:
          calendar = DocTrackingUtils.Session.SessionObjects.GetBusinessCalendar(CalendarType.Postal);
          break;
        case LockExpCalendarSetting.BusinessCalendar:
          calendar = DocTrackingUtils.Session.SessionObjects.GetBusinessCalendar(CalendarType.Business);
          break;
        case LockExpCalendarSetting.CustomCalendar:
          calendar = DocTrackingUtils.Session.SessionObjects.GetBusinessCalendar(CalendarType.Custom);
          break;
      }
      return calendar;
    }

    public static DateTime GetPreviousClosestBusinessDay(DateTime date)
    {
      BusinessCalendar calendar = DocTrackingUtils.GetCalendar();
      if (calendar != null)
        date = calendar.GetPreviousClosestBusinessDayNoWeekend(date);
      else if (date.DayOfWeek == DayOfWeek.Sunday)
        date = date.AddDays(-2.0);
      else if (date.DayOfWeek == DayOfWeek.Saturday)
        date = date.AddDays(-1.0);
      return date;
    }

    public static string BuildAddress(string[] address)
    {
      bool flag = true;
      foreach (string str in address)
      {
        if (str != string.Empty)
        {
          flag = false;
          break;
        }
      }
      if (flag)
        return "";
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string str in address)
      {
        if (!string.IsNullOrEmpty(str))
          stringBuilder.Append(str).Append(", ");
      }
      if (stringBuilder.Length > 0)
        stringBuilder.Length -= 2;
      return stringBuilder.ToString();
    }

    public static void SaveComments(DocumentTrackingComment comment)
    {
      List<DocumentTrackingComment> toComments = DocTrackingUtils.ParseToComments(DocTrackingUtils.Session.LoanData.GetField(DocTrackingUtils.CommentFiled));
      toComments.Add(comment);
      DocTrackingUtils.Session.LoanData.SetField(DocTrackingUtils.CommentFiled, DocTrackingUtils.StringifyFromComments((object) toComments));
    }

    public static List<DocumentTrackingComment> GetComments()
    {
      return DocTrackingUtils.ParseToComments(DocTrackingUtils.Session.LoanData.GetField(DocTrackingUtils.CommentFiled));
    }

    private static string StringifyFromComments(object jsonObject)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        new DataContractJsonSerializer(jsonObject.GetType()).WriteObject((Stream) memoryStream, jsonObject);
        return Encoding.UTF8.GetString(memoryStream.ToArray());
      }
    }

    private static List<DocumentTrackingComment> ParseToComments(string jsonString)
    {
      if (string.IsNullOrEmpty(jsonString))
        return new List<DocumentTrackingComment>();
      using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
        return (List<DocumentTrackingComment>) new DataContractJsonSerializer(typeof (List<DocumentTrackingComment>)).ReadObject((Stream) memoryStream);
    }

    public static DateTime GetNotePurchaseDt()
    {
      LoanData loanData = DocTrackingUtils.Session.LoanData;
      Hashtable settingsFromCache = DocTrackingUtils.Session.SessionObjects.GetCompanySettingsFromCache("NMLS");
      DateTime notePurchaseDt = DateTime.MinValue;
      LoanChannelNameProvider channelNameProvider = new LoanChannelNameProvider();
      if (loanData.GetField("2626") == LoanChannel.Correspondent.ToString())
        notePurchaseDt = Utils.ParseDate((object) loanData.GetField("3567"));
      else if (loanData.GetField("2626") == channelNameProvider.GetName((object) LoanChannel.BankedRetail) || loanData.GetField("2626") == channelNameProvider.GetName((object) LoanChannel.BankedWholesale))
        notePurchaseDt = Utils.ParseDate((object) loanData.GetField("L770"));
      else if (loanData.GetField("2626") == string.Empty && settingsFromCache != null && settingsFromCache.Count > 0 && settingsFromCache[(object) "ChannelMap/"] != null && (settingsFromCache[(object) "ChannelMap/"].ToString() == channelNameProvider.GetName((object) LoanChannel.BankedRetail) || settingsFromCache[(object) "ChannelMap/"].ToString() == channelNameProvider.GetName((object) LoanChannel.BankedWholesale)))
        notePurchaseDt = Utils.ParseDate((object) loanData.GetField("L770"));
      return notePurchaseDt;
    }

    public static DateTime CalculateNextFollowupDate(
      DateTime initRequestDay,
      int days,
      BusinessCalendar bc)
    {
      DateTime date = initRequestDay.AddDays((double) days);
      if (date.DayOfWeek == DayOfWeek.Saturday)
        date = date.AddDays(-1.0);
      else if (date.DayOfWeek == DayOfWeek.Sunday)
        date = date.AddDays(-2.0);
      if (bc != null)
        date = bc.GetNextClosestBusinessDay(date);
      return date;
    }
  }
}
