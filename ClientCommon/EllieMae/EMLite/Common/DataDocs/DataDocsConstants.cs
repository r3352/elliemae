// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.DataDocs.DataDocsConstants
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

#nullable disable
namespace EllieMae.EMLite.Common.DataDocs
{
  public static class DataDocsConstants
  {
    public static int MAX_SUBMISSIONS_PER_PAGE = 50;

    public static string[] GetDeliveryStatusList()
    {
      List<string> stringList = new List<string>();
      foreach (MemberInfo member in typeof (DeliveryStatus).GetMembers())
      {
        object[] customAttributes = member.GetCustomAttributes(typeof (DescriptionAttribute), false);
        if (customAttributes.Length != 0)
        {
          string description = ((DescriptionAttribute) customAttributes[0]).Description;
          stringList.Add(description);
        }
      }
      return stringList.ToArray();
    }

    public static string[] GetDeliveryStatusList(DeliveryStatus[] statuses)
    {
      string[] deliveryStatusList = new string[statuses.Length];
      for (int index = 0; index < statuses.Length; ++index)
        deliveryStatusList[index] = DataDocsConstants.DeliveryStatusToString(statuses[index]);
      return deliveryStatusList;
    }

    public static DeliveryStatus[] GetActiveDeliveryStatusList()
    {
      return new DeliveryStatus[3]
      {
        DeliveryStatus.InProgress,
        DeliveryStatus.Completed,
        DeliveryStatus.Error
      };
    }

    public static DeliveryStatus[] GetSubmittedDeliveryStatusList()
    {
      return new DeliveryStatus[4]
      {
        DeliveryStatus.Submitted,
        DeliveryStatus.Accepted,
        DeliveryStatus.Cancelled,
        DeliveryStatus.Rejected
      };
    }

    public static DeliveryStatus[] GetAllDeliveryStatusList()
    {
      return new DeliveryStatus[8]
      {
        DeliveryStatus.InProgress,
        DeliveryStatus.Completed,
        DeliveryStatus.Error,
        DeliveryStatus.Submitted,
        DeliveryStatus.Accepted,
        DeliveryStatus.Cancelled,
        DeliveryStatus.Rejected,
        DeliveryStatus.Delivered
      };
    }

    public static DeliveryStatus StringToDeliveryStatusEnum(string enumString)
    {
      switch (enumString.ToUpper())
      {
        case "":
          return DeliveryStatus.None;
        case "ACCEPTED":
          return DeliveryStatus.Accepted;
        case "CANCELLED":
          return DeliveryStatus.Cancelled;
        case "COMPLETED":
          return DeliveryStatus.Completed;
        case "DELIVERED":
          return DeliveryStatus.Delivered;
        case "ERROR":
          return DeliveryStatus.Error;
        case "IN PROGRESS":
        case "INPROGRESS":
          return DeliveryStatus.InProgress;
        case "REJECTED":
          return DeliveryStatus.Rejected;
        case "SUBMITTED":
          return DeliveryStatus.Submitted;
        default:
          throw new Exception("Wrong enum string: " + enumString);
      }
    }

    public static SubmissionType[] GetAllSubmissionTypesList()
    {
      return new SubmissionType[4]
      {
        SubmissionType.NotAvailable,
        SubmissionType.Default,
        SubmissionType.Mandatory,
        SubmissionType.BestEffort
      };
    }

    public static DeliveryAction StringToDeliveryActionsEnum(string enumString)
    {
      switch (enumString.ToUpper())
      {
        case "VIEW AUDIT REPORT":
          return DeliveryAction.ViewAuditReport;
        case "SUBMIT":
          return DeliveryAction.Submit;
        case "REMOVE":
          return DeliveryAction.Remove;
        case "VIEW LOG":
          return DeliveryAction.ViewLog;
        default:
          throw new Exception("Wrong enum string: " + enumString);
      }
    }

    public static string DeliveryActionsToString(DeliveryAction action)
    {
      foreach (MemberInfo member in typeof (DeliveryAction).GetMembers())
      {
        if (member.Name == action.ToString())
        {
          object[] customAttributes = member.GetCustomAttributes(typeof (DescriptionAttribute), false);
          if (customAttributes.Length != 0)
            return ((DescriptionAttribute) customAttributes[0]).Description;
        }
      }
      return string.Empty;
    }

    public static string DeliveryStatusToString(DeliveryStatus status)
    {
      foreach (MemberInfo member in typeof (DeliveryStatus).GetMembers())
      {
        if (member.Name == status.ToString())
        {
          object[] customAttributes = member.GetCustomAttributes(typeof (DescriptionAttribute), false);
          if (customAttributes.Length != 0)
            return ((DescriptionAttribute) customAttributes[0]).Description;
        }
      }
      return string.Empty;
    }

    public static string SubmissionTypeToString(SubmissionType status)
    {
      foreach (MemberInfo member in typeof (SubmissionType).GetMembers())
      {
        if (member.Name == status.ToString())
        {
          object[] customAttributes = member.GetCustomAttributes(typeof (DescriptionAttribute), false);
          if (customAttributes.Length != 0)
            return ((DescriptionAttribute) customAttributes[0]).Description;
        }
      }
      return string.Empty;
    }
  }
}
