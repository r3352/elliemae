// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.ReportFieldClientExtension
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public static class ReportFieldClientExtension
  {
    public static Sessions.Session session = Session.DefaultInstance;

    public static object ToDisplayElement(
      this BizPartnerReportFieldDef thisObject,
      string criteriaName,
      IPropertyDictionary data,
      EventHandler clickEvent)
    {
      switch (thisObject.DisplayType)
      {
        case FieldDisplayType.BizContactGroup:
          return (object) new ContactGroupsLink(criteriaName, data, EllieMae.EMLite.ContactUI.ContactType.BizPartner);
        case FieldDisplayType.PublicContactGroup:
          return (object) new ContactGroupsLink(criteriaName, data, EllieMae.EMLite.ContactUI.ContactType.PublicBiz);
        default:
          return ReportFieldClientExtension.ReportFieldClientHelper.ToDisplayElement((ContactReportFieldDef) thisObject, criteriaName, data, clickEvent, typeof (ContactReportFieldDef));
      }
    }

    public static object ToDisplayElement(
      this BorrowerReportFieldDef thisObject,
      string criteriaName,
      IPropertyDictionary data,
      EventHandler clickEvent)
    {
      return thisObject.DisplayType == FieldDisplayType.ContactGroup ? (object) new ContactGroupsLink(criteriaName, data, EllieMae.EMLite.ContactUI.ContactType.Borrower) : ReportFieldClientExtension.ReportFieldClientHelper.ToDisplayElement((ContactReportFieldDef) thisObject, criteriaName, data, clickEvent, typeof (ContactReportFieldDef));
    }

    public static object ToDisplayElement(
      this ContactReportFieldDef thisObject,
      string criteriaName,
      IPropertyDictionary data,
      EventHandler clickEvent)
    {
      return ReportFieldClientExtension.ReportFieldClientHelper.ToDisplayElement(thisObject, criteriaName, data, clickEvent);
    }

    public static object ToDisplayElement(
      this CorrespondentMasterReportFieldDef thisObject,
      string criteriaName,
      IPropertyDictionary data,
      EventHandler clickEvent)
    {
      return thisObject.DisplayType == FieldDisplayType.Progress ? (object) new ProgressElement(Utils.ParseDecimal(data[criteriaName], 0M), true) : (object) thisObject.ToDisplayValue(string.Concat(data[criteriaName]));
    }

    public static object ToDisplayElement(
      this CorrespondentTradeReportFieldDef thisObject,
      string criteriaName,
      IPropertyDictionary dataList,
      EventHandler clickEvent)
    {
      return thisObject.ToDisplayElement(criteriaName, dataList[criteriaName], clickEvent);
    }

    public static object ToDisplayElement(
      this GSECommitmentReportFieldDef thisObject,
      string criteriaName,
      IPropertyDictionary dataList,
      EventHandler clickEvent)
    {
      return thisObject.ToDisplayElement(criteriaName, dataList[criteriaName], clickEvent);
    }

    public static object ToDisplayElement(
      this GSECommitmentReportFieldDef thisObject,
      string criteriaName,
      object data,
      EventHandler clickEvent)
    {
      switch (thisObject.DisplayType)
      {
        case FieldDisplayType.Progress:
          return (object) new ProgressElement(Utils.ParseDecimal(data, 0M), true);
        case FieldDisplayType.LockLabel:
          return Utils.ParseBoolean(data) ? (object) new LockLabel() : (object) "";
        default:
          return (object) thisObject.ToDisplayValue(string.Concat(data));
      }
    }

    public static object ToDisplayElement(
      this CorrespondentTradeReportFieldDef thisObject,
      string criteriaName,
      object data,
      EventHandler clickEvent)
    {
      switch (thisObject.DisplayType)
      {
        case FieldDisplayType.Alert:
          return Utils.ParseInt(data, 0) > 0 ? (object) new AlertMessageLabel(AlertMessageLabel.AlertMessageStyle.Alert, "!") : (object) "";
        case FieldDisplayType.Progress:
          return (object) new ProgressElement(Utils.ParseDecimal(data, 0M), true);
        case FieldDisplayType.LockLabel:
          return Utils.ParseBoolean(data) ? (object) new LockLabel() : (object) "";
        default:
          return (object) thisObject.ToDisplayValue(string.Concat(data));
      }
    }

    public static object ToDisplayElement(
      this LoanReportFieldDef thisObject,
      string fieldName,
      PipelineInfo pinfo,
      Control parentControl)
    {
      PipelineElementData data = new PipelineElementData(fieldName, pinfo);
      switch (thisObject.DisplayType)
      {
        case FieldDisplayType.Borrower:
          return (object) new BorrowerLink(parentControl, data);
        case FieldDisplayType.Property:
          return (object) new PropertyLink(parentControl, data);
        case FieldDisplayType.Alert:
          return (object) new PipelineAlertLink(parentControl, data);
        case FieldDisplayType.Message:
          return (object) new PipelineMessageLink(parentControl, data);
        case FieldDisplayType.User:
          return (object) new UserLink(parentControl, data);
        case FieldDisplayType.Milestone:
          return (object) new MilestoneLabel(((MilestoneTemplatesBpmManager) Session.DefaultInstance.BPM.GetBpmManager(BpmCategory.Milestones)).GetMilestoneByID(string.Concat(data.PipelineInfo.GetField("CurrentMilestoneID")), string.Concat(data.PipelineInfo.GetField("CurrentMilestoneName"))), data);
        case FieldDisplayType.CoreMilestone:
          return (object) new MilestoneLabel(((MilestoneTemplatesBpmManager) Session.DefaultInstance.BPM.GetBpmManager(BpmCategory.Milestones)).GetMilestoneByID(string.Concat(data.PipelineInfo.GetField("CurrentMilestoneID")), string.Concat(data.PipelineInfo.GetField("CurrentMilestoneName"))), data);
        case FieldDisplayType.RateLock:
          return (object) new RateLockLink(parentControl, data);
        case FieldDisplayType.LoanFolder:
          return (object) new LoanFolderLabel(data);
        case FieldDisplayType.LoanAssociate:
          return (object) new LoanAssociateLink(parentControl, data);
        case FieldDisplayType.RateLockAndRequest:
          return (object) new RateLockLink(parentControl, data);
        case FieldDisplayType.NextMilestone:
          return (object) new MilestoneLabel(((MilestoneTemplatesBpmManager) Session.DefaultInstance.BPM.GetBpmManager(BpmCategory.Milestones)).GetMilestoneByID(string.Concat(data.PipelineInfo.GetField("Loan.NextMilestoneID")), string.Concat(data.PipelineInfo.GetField("Loan.NextMilestoneName"))), data);
        case FieldDisplayType.LinkedGUID:
          return pinfo.LinkGuid != "" ? (object) new LinkedGUIDLink(parentControl, pinfo, data) : (object) "";
        case FieldDisplayType.LockValidationStatus:
          return (object) new LockValidationStatusLink(parentControl, data);
        default:
          return (object) thisObject.ToDisplayValue(data.ToString());
      }
    }

    public static object ToDisplayElement(
      this MasterContractReportFieldDef thisObject,
      string criteriaName,
      IPropertyDictionary data,
      EventHandler clickEvent)
    {
      if (thisObject.CriterionFieldName.ToLower() == "mastercontracts.term")
        return data[criteriaName];
      return thisObject.DisplayType == FieldDisplayType.Progress ? (object) new ProgressElement(Utils.ParseDecimal(data[criteriaName], 0M), true) : (object) thisObject.ToDisplayValue(string.Concat(data[criteriaName]));
    }

    public static object ToDisplayElement(
      this TradeReportFieldDef thisObject,
      string criteriaName,
      IPropertyDictionary dataList,
      EventHandler clickEvent)
    {
      return thisObject.ToDisplayElement(criteriaName, dataList[criteriaName], clickEvent);
    }

    public static object ToDisplayElement(
      this TradeReportFieldDef thisObject,
      string criteriaName,
      object data,
      EventHandler clickEvent)
    {
      switch (thisObject.DisplayType)
      {
        case FieldDisplayType.Alert:
          return Utils.ParseInt(data, 0) > 0 ? (object) new AlertMessageLabel(AlertMessageLabel.AlertMessageStyle.Alert, "!") : (object) "";
        case FieldDisplayType.Progress:
          return (object) new ProgressElement(Utils.ParseDecimal(data, 0M), true);
        case FieldDisplayType.LockLabel:
          return Utils.ParseBoolean(data) ? (object) new LockLabel() : (object) "";
        default:
          return (object) thisObject.ToDisplayValue(string.Concat(data));
      }
    }

    public static TableLayout.Column ToTableLayoutColumn(this ReportFieldDef thisObject)
    {
      return thisObject.ToTableLayoutColumn(new ReportFieldCommonExtension.GetFieldDefWidth(ReportFieldClientExtension.GetDefaultColumnWidth));
    }

    public static TableLayout.Column ToTableLayoutColumn(this ContactReportFieldDef thisObject)
    {
      return ReportFieldCommonExtension.ToTableLayoutColumn(thisObject, new ReportFieldCommonExtension.GetFieldDefWidth(ReportFieldClientExtension.GetDefaultColumnWidth));
    }

    public static TableLayout.Column ToTableLayoutColumn(this BizPartnerReportFieldDef thisObject)
    {
      return ReportFieldCommonExtension.ToTableLayoutColumn(thisObject, new ReportFieldCommonExtension.GetFieldDefWidth(ReportFieldClientExtension.GetDefaultColumnWidth));
    }

    public static TableLayout.Column ToTableLayoutColumn(this BorrowerReportFieldDef thisObject)
    {
      return ReportFieldCommonExtension.ToTableLayoutColumn(thisObject, new ReportFieldCommonExtension.GetFieldDefWidth(ReportFieldClientExtension.GetDefaultColumnWidth));
    }

    public static TableLayout.Column ToTableLayoutColumn(this LoanReportFieldDef thisObject)
    {
      return ReportFieldCommonExtension.ToTableLayoutColumn(thisObject, new ReportFieldCommonExtension.GetFieldDefWidth(ReportFieldClientExtension.GetDefaultColumnWidth));
    }

    public static TableLayout.Column ToTableLayoutColumn(
      this MasterContractReportFieldDef thisObject)
    {
      return ReportFieldCommonExtension.ToTableLayoutColumn(thisObject, new ReportFieldCommonExtension.GetFieldDefWidth(ReportFieldClientExtension.GetDefaultColumnWidth));
    }

    public static TableLayout.Column ToTableLayoutColumn(
      this CorrespondentMasterReportFieldDef thisObject)
    {
      return ReportFieldCommonExtension.ToTableLayoutColumn(thisObject, new ReportFieldCommonExtension.GetFieldDefWidth(ReportFieldClientExtension.GetDefaultColumnWidth));
    }

    public static TableLayout.Column ToTableLayoutColumn(
      this CorrespondentTradeReportFieldDef thisObject)
    {
      return ReportFieldCommonExtension.ToTableLayoutColumn(thisObject, new ReportFieldCommonExtension.GetFieldDefWidth(ReportFieldClientExtension.GetDefaultColumnWidth));
    }

    public static TableLayout.Column ToTableLayoutColumn(this TradeReportFieldDef thisObject)
    {
      return ReportFieldCommonExtension.ToTableLayoutColumn(thisObject, new ReportFieldCommonExtension.GetFieldDefWidth(ReportFieldClientExtension.GetDefaultColumnWidth));
    }

    public static int GetMaximumOptionWidth(ReportFieldDef thisObject)
    {
      if (thisObject.FieldDefinition.Options.Count == 0 || ReportFieldClientExtension.session.MainForm == null)
        return 0;
      int maximumOptionWidth = 0;
      using (Graphics graphics = ReportFieldClientExtension.session.MainForm.CreateGraphics())
      {
        foreach (FieldOption option in thisObject.FieldDefinition.Options)
        {
          int num = (int) graphics.MeasureString(option.Text, ReportFieldClientExtension.session.MainForm.Font).Width + 1;
          if (num > maximumOptionWidth)
            maximumOptionWidth = num;
        }
      }
      return maximumOptionWidth;
    }

    public static int GetDefaultColumnWidth(this ReportFieldDef thisObject)
    {
      return ReportFieldClientExtension.ReportFieldClientHelper.GetDefaultColumnWidth(thisObject);
    }

    public static int GetDefaultColumnWidth(this ContactReportFieldDef thisObject)
    {
      switch (thisObject.DisplayType)
      {
        case FieldDisplayType.Borrower:
          return 180;
        case FieldDisplayType.Alert:
          return 40;
        case FieldDisplayType.RateLock:
          return 60;
        default:
          return ReportFieldClientExtension.ReportFieldClientHelper.GetDefaultColumnWidth((ReportFieldDef) thisObject, typeof (ReportFieldDef));
      }
    }

    public static int GetDefaultColumnWidth(this LoanReportFieldDef thisObject)
    {
      switch (thisObject.DisplayType)
      {
        case FieldDisplayType.Borrower:
          return 180;
        case FieldDisplayType.Alert:
          return 40;
        case FieldDisplayType.Message:
          return 40;
        case FieldDisplayType.Milestone:
        case FieldDisplayType.CoreMilestone:
          return 110;
        case FieldDisplayType.RateLock:
          return 60;
        default:
          return ReportFieldClientExtension.ReportFieldClientHelper.GetDefaultColumnWidth((ReportFieldDef) thisObject, typeof (ReportFieldDef));
      }
    }

    private class ReportFieldClientHelper
    {
      public static object ToDisplayElement(
        ContactReportFieldDef thisObject,
        string criteriaName,
        IPropertyDictionary data,
        EventHandler clickEvent,
        System.Type type = null)
      {
        if ((System.Type) null == type)
          type = thisObject.GetType();
        if (typeof (BizPartnerReportFieldDef) == type)
          return ReportFieldClientExtension.ToDisplayElement(thisObject as BizPartnerReportFieldDef, criteriaName, data, clickEvent);
        if (typeof (BorrowerReportFieldDef) == type)
          return ReportFieldClientExtension.ToDisplayElement(thisObject as BorrowerReportFieldDef, criteriaName, data, clickEvent);
        switch (thisObject.DisplayType)
        {
          case FieldDisplayType.ContactEmail:
            return (object) new ContactEmailLink(criteriaName, data, clickEvent);
          case FieldDisplayType.ContactGroup:
            return (object) new ContactGroupsLink(criteriaName, data, EllieMae.EMLite.ContactUI.ContactType.Borrower);
          case FieldDisplayType.BizContactGroup:
            return (object) new ContactGroupsLink(criteriaName, data, EllieMae.EMLite.ContactUI.ContactType.BizPartner);
          case FieldDisplayType.PublicContactGroup:
            return (object) new ContactGroupsLink(criteriaName, data, EllieMae.EMLite.ContactUI.ContactType.PublicBiz);
          case FieldDisplayType.ContactPhone:
            return (object) new ContactPhoneLink(criteriaName, data, clickEvent, PhoneImageLink.PhoneType.LandPhone);
          case FieldDisplayType.ContactCellPhone:
            return (object) new ContactPhoneLink(criteriaName, data, clickEvent, PhoneImageLink.PhoneType.CellPhone);
          case FieldDisplayType.ContactFax:
            return (object) new ContactPhoneLink(criteriaName, data, clickEvent, PhoneImageLink.PhoneType.Fax);
          default:
            return (object) thisObject.ToDisplayValue(string.Concat(data[criteriaName]));
        }
      }

      public static int GetDefaultColumnWidth(ReportFieldDef thisObject, System.Type type = null)
      {
        if ((System.Type) null == type)
          type = thisObject.GetType();
        if (typeof (ContactReportFieldDef) == type)
          return ReportFieldClientExtension.GetDefaultColumnWidth(thisObject as ContactReportFieldDef);
        if (typeof (LoanReportFieldDef) == type)
          return ReportFieldClientExtension.GetDefaultColumnWidth(thisObject as LoanReportFieldDef);
        int maximumOptionWidth = ReportFieldClientExtension.GetMaximumOptionWidth(thisObject);
        if (maximumOptionWidth > 0)
          return Math.Min(maximumOptionWidth + 30, 200);
        switch (thisObject.FieldType)
        {
          case EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsNumeric:
            return 100;
          case EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsDate:
            return 80;
          case EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsMonthDay:
            return 60;
          case EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsYesNo:
            return 40;
          case EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsCheckbox:
            return 40;
          case EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsDateTime:
            return 80;
          default:
            return 180;
        }
      }
    }
  }
}
