// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.RateLockFields
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public static class RateLockFields
  {
    public static readonly FieldDefinitionCollection All = new FieldDefinitionCollection();

    static RateLockFields()
    {
      RateLockFields.All.Add((FieldDefinition) new RateLockDaysToExpireField());
      RateLockFields.All.Add((FieldDefinition) new RateLockStatusField());
      RateLockFields.All.Add((FieldDefinition) new RateLockOtherField("REQUESTCOUNT", "Count of the number of lock requests", FieldFormat.INTEGER));
      RateLockFields.All.Add((FieldDefinition) new RateLockOtherField("BUYLOCKCOUNT", "Count of the number of buy side locks", FieldFormat.INTEGER));
      RateLockFields.All.Add((FieldDefinition) new RateLockOtherField("SELLLOCKCOUNT", "Count of the number of sell side locks", FieldFormat.INTEGER));
      RateLockFields.All.Add((FieldDefinition) new RateLockOtherField("CONFIRMATIONCOUNT", "Count of the number of lock confirmations", FieldFormat.INTEGER));
      RateLockFields.All.Add((FieldDefinition) new RateLockOtherField("DENIALCOUNT", "Count of the number of lock denials", FieldFormat.INTEGER));
      RateLockFields.All.Add((FieldDefinition) new RateLockCurrentStatusField());
      RateLockFields.All.Add((FieldDefinition) new RateLockOtherField("CURRENTDATETIME", "Date and Time of current status occurred", FieldFormat.STRING));
      RateLockFields.All.Add((FieldDefinition) new RateLockOtherField("DENIALCOMMENTS", "Denial Comments (Most Recent)", FieldFormat.STRING));
      RateLockFields.All.Add((FieldDefinition) new RateLockOtherField("DENIALCOMMENTS1", "Denial Comments (Previous 1)", FieldFormat.STRING));
      RateLockFields.All.Add((FieldDefinition) new RateLockOtherField("DENIALCOMMENTS2", "Denial Comments (Previous 2)", FieldFormat.STRING));
      RateLockFields.All.Add((FieldDefinition) new RateLockActiveRequestField());
      RateLockFields.All.Add((FieldDefinition) new RateLockPendingRequestField());
      RateLockFields.All.Add((FieldDefinition) new RateLockAndRequestStatusField());
      RateLockFields.All.Add((FieldDefinition) new RateLockRequestStatusField());
      RateLockFields.All.Add((FieldDefinition) new RateLockLastActionTimeField());
      RateLockFields.All.Add((FieldDefinition) new RateLockRequestTypeField());
      RateLockFields.All.Add((FieldDefinition) new RateLockPendingExtensionRequestField());
      RateLockFields.All.Add((FieldDefinition) new RateLockPendingCancellationRequestField());
      RateLockFields.All.Add((FieldDefinition) new RateLockIsCancelledField());
      RateLockFields.All.Add((FieldDefinition) new RateLockPendingRelockRequestField());
      foreach (string snapshotField in LockRequestLog.SnapshotFields)
      {
        FieldDefinition baseField = StandardFields.All[snapshotField];
        if (baseField != null)
        {
          RateLockField field = new RateLockField(baseField);
          if (!RateLockFields.All.Contains(field.FieldID))
          {
            RateLockFields.All.Add((FieldDefinition) field);
            RateLockFields.All.Add((FieldDefinition) new RateLockField(baseField, RateLockField.RateLockOrder.Previous));
            RateLockFields.All.Add((FieldDefinition) new RateLockField(baseField, RateLockField.RateLockOrder.Previous2));
            RateLockFields.All.Add((FieldDefinition) new RateLockField(baseField, "Rate Request - " + baseField.Description + " (Most Recent)", RateLockField.RateLockOrder.MostRecentRequest));
            RateLockFields.All.Add((FieldDefinition) new RateLockField(baseField, "Rate Lock Request - " + baseField.Description + " (Most Recent)", RateLockField.RateLockOrder.MostRecentLockRequest));
            RateLockFields.All.Add((FieldDefinition) new RateLockField(baseField, "Rate Lock Request - " + baseField.Description + " (Previous 1)", RateLockField.RateLockOrder.PreviousLockRequest));
            RateLockFields.All.Add((FieldDefinition) new RateLockField(baseField, "Rate Lock Request - " + baseField.Description + " (Previous 2)", RateLockField.RateLockOrder.Previous2LockRequest));
            RateLockFields.All.Add((FieldDefinition) new RateLockField(baseField, "Rate Request - " + baseField.Description + " (Mostly Recent Confirmed)", RateLockField.RateLockOrder.MostRecentlyConfirmed));
            RateLockFields.All.Add((FieldDefinition) new RateLockField(baseField, "Rate Lock Denial - " + baseField.Description + " (Most Recent)", RateLockField.RateLockOrder.MostRecentDenied));
            RateLockFields.All.Add((FieldDefinition) new RateLockField(baseField, "Rate Lock Denial - " + baseField.Description + " (Previous 1)", RateLockField.RateLockOrder.PreviousDenied));
            RateLockFields.All.Add((FieldDefinition) new RateLockField(baseField, "Rate Lock Denial - " + baseField.Description + " (Previous 2)", RateLockField.RateLockOrder.Previous2Denied));
          }
        }
      }
      foreach (string requestField in LockRequestLog.RequestFields)
      {
        FieldDefinition baseField = StandardFields.All[requestField];
        if (baseField != null)
        {
          RateLockField field = new RateLockField(baseField);
          if (!RateLockFields.All.Contains(field.FieldID))
          {
            RateLockFields.All.Add((FieldDefinition) field);
            RateLockFields.All.Add((FieldDefinition) new RateLockField(baseField, RateLockField.RateLockOrder.Previous));
            RateLockFields.All.Add((FieldDefinition) new RateLockField(baseField, RateLockField.RateLockOrder.Previous2));
            RateLockFields.All.Add((FieldDefinition) new RateLockField(baseField, "Rate Request - " + baseField.Description + " (Most Recent)", RateLockField.RateLockOrder.MostRecentRequest));
            RateLockFields.All.Add((FieldDefinition) new RateLockField(baseField, "Rate Lock Request - " + baseField.Description + " (Most Recent)", RateLockField.RateLockOrder.MostRecentLockRequest));
            RateLockFields.All.Add((FieldDefinition) new RateLockField(baseField, "Rate Lock Request - " + baseField.Description + " (Previous 1)", RateLockField.RateLockOrder.PreviousLockRequest));
            RateLockFields.All.Add((FieldDefinition) new RateLockField(baseField, "Rate Lock Request - " + baseField.Description + " (Previous 2)", RateLockField.RateLockOrder.Previous2LockRequest));
            RateLockFields.All.Add((FieldDefinition) new RateLockField(baseField, "Rate Request - " + baseField.Description + " (Most Recently Confirmed)", RateLockField.RateLockOrder.MostRecentlyConfirmed));
            RateLockFields.All.Add((FieldDefinition) new RateLockField(baseField, "Rate Lock Denial - " + baseField.Description + " (Most Recent)", RateLockField.RateLockOrder.MostRecentDenied));
            RateLockFields.All.Add((FieldDefinition) new RateLockField(baseField, "Rate Lock Denial - " + baseField.Description + " (Previous 1)", RateLockField.RateLockOrder.PreviousDenied));
            RateLockFields.All.Add((FieldDefinition) new RateLockField(baseField, "Rate Lock Denial - " + baseField.Description + " (Previous 2)", RateLockField.RateLockOrder.Previous2Denied));
          }
        }
      }
      RateLockFields.All.Add((FieldDefinition) new RateLockStatisticField("REQUESTEDBY", "Rate Requested By (Most Recent)", FieldFormat.STRING));
      RateLockFields.All.Add((FieldDefinition) new RateLockStatisticField("REQUESTEDBY", "Rate Requested By (Previous 1)", RateLockStatisticField.RateLockOrder.Previous, FieldFormat.STRING));
      RateLockFields.All.Add((FieldDefinition) new RateLockStatisticField("REQUESTEDBY", "Rate Requested By (Previous 2)", RateLockStatisticField.RateLockOrder.Previous2, FieldFormat.STRING));
      RateLockFields.All.Add((FieldDefinition) new RateLockStatisticField("CONFIRMEDREQUESTEDBY", "Rate Requested By (Most Recent in Confirmed Log)", FieldFormat.STRING));
      RateLockFields.All.Add((FieldDefinition) new RateLockStatisticField("CONFIRMEDREQUESTEDBY", "Rate Requested By (Previous 1 in Confirmed Log)", RateLockStatisticField.RateLockOrder.Previous, FieldFormat.STRING));
      RateLockFields.All.Add((FieldDefinition) new RateLockStatisticField("CONFIRMEDREQUESTEDBY", "Rate Requested By (Previous 2 in Confirmed Log)", RateLockStatisticField.RateLockOrder.Previous2, FieldFormat.STRING));
      RateLockFields.All.Add((FieldDefinition) new RateLockStatisticField("DURATION", "Fulfillment Duration (Most Recent)", FieldFormat.INTEGER));
      RateLockFields.All.Add((FieldDefinition) new RateLockStatisticField("DURATION", "Fulfillment Duration (Previous 1)", RateLockStatisticField.RateLockOrder.Previous, FieldFormat.INTEGER));
      RateLockFields.All.Add((FieldDefinition) new RateLockStatisticField("DURATION", "Fulfillment Duration (Previous 2)", RateLockStatisticField.RateLockOrder.Previous2, FieldFormat.INTEGER));
      RateLockFields.All.Add((FieldDefinition) new RateLockStatisticField("AVERAGEDURATION", "Average Fulfillments Duration (Minutes)", FieldFormat.DECIMAL_2));
      RateLockFields.All.Add((FieldDefinition) new RateLockStatisticField("FULFILLEDDATETIME", "Fulfilled Date Time (Most Recent)", FieldFormat.DATETIME));
      RateLockFields.All.Add((FieldDefinition) new RateLockStatisticField("FULFILLEDDATETIME", "Fulfilled Date Time (Previous 1)", RateLockStatisticField.RateLockOrder.Previous, FieldFormat.DATETIME));
      RateLockFields.All.Add((FieldDefinition) new RateLockStatisticField("FULFILLEDDATETIME", "Fulfilled Date Time (Previous 2)", RateLockStatisticField.RateLockOrder.Previous2, FieldFormat.DATETIME));
      RateLockFields.All.Add((FieldDefinition) new RateLockStatisticField("CANCELLEDDATETIME", "Cancelled Date Time (Most Recent)", FieldFormat.DATETIME));
    }

    internal static void AddField(VirtualField field)
    {
      if (RateLockFields.All.Contains(field.FieldID))
        return;
      RateLockFields.All.Add((FieldDefinition) field);
    }
  }
}
