// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.DataDocs.SubmissionStatus
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using Newtonsoft.Json;
using System;

#nullable disable
namespace EllieMae.EMLite.Common.DataDocs
{
  public class SubmissionStatus
  {
    [JsonProperty("id")]
    public string ReferenceID { get; set; }

    [JsonProperty("loanId")]
    public string LoanGuid { get; set; }

    [JsonProperty("loanNumber")]
    public string LoanNumber { get; set; }

    [JsonProperty("details")]
    public string LogDetailsString { get; set; }

    [JsonProperty("submissionId")]
    public string SubmissionIDString { get; set; }

    [JsonProperty("submissionDate")]
    public string SubmissionDateString { get; set; }

    [JsonProperty("submittedBy")]
    public string SubmittedBy { get; set; }

    [JsonProperty("createdDate")]
    public DateTime? CreateDate { get; set; }

    [JsonProperty("auditIndicatorRed")]
    public string AuditCountRedString { get; set; }

    [JsonProperty("auditIndicatorYellow")]
    public string AuditCountYellowString { get; set; }

    [JsonProperty("auditIndicatorGreen")]
    public string AuditCountGreenString { get; set; }

    [JsonProperty("providerName")]
    public string SubmittedTo { get; set; }

    [JsonProperty("orderId")]
    public string RecipientTransactionID { get; set; }

    [JsonProperty("createdBy")]
    public string CreatedByString { get; set; }

    public string CreatedUserName { get; set; }

    [JsonProperty("loanStatus")]
    public string StatusString { get; set; }

    [JsonProperty("statusDate")]
    public string StatusDateString { get; set; }

    [JsonProperty("submissionType")]
    public string SubmissionTypeString { get; set; }

    [JsonProperty("hasValidationError")]
    public bool HasValidationError { get; set; }

    [JsonProperty("validationError")]
    public string[] ValidationErrors { get; set; }

    public bool CanSubmit => this.Status == DeliveryStatus.Completed;

    public bool CanRemove
    {
      get => this.Status == DeliveryStatus.Error || this.Status == DeliveryStatus.Completed;
    }

    public string DeliveryStatusString => DataDocsConstants.DeliveryStatusToString(this.Status);

    public DeliveryStatus Status
    {
      get
      {
        try
        {
          return DataDocsConstants.StringToDeliveryStatusEnum(this.StatusString);
        }
        catch
        {
          return DeliveryStatus.None;
        }
      }
    }

    public DeliveryAction[] Actions
    {
      get
      {
        switch (this.Status)
        {
          case DeliveryStatus.Error:
            return new DeliveryAction[1]
            {
              DeliveryAction.ViewLog
            };
          case DeliveryStatus.Rejected:
            return new DeliveryAction[1]
            {
              DeliveryAction.ViewLog
            };
          default:
            return new DeliveryAction[1];
        }
      }
    }

    public string LogDetails
    {
      get => this.CheckForNullString(this.LogDetailsString) ? (string) null : this.LogDetailsString;
    }

    private bool CheckForNullString(string stringToTest)
    {
      return string.IsNullOrEmpty(stringToTest) || string.Compare(stringToTest, "null", true) == 0;
    }

    public string SubmissionID
    {
      get
      {
        return this.CheckForNullString(this.SubmissionIDString) ? (string) null : this.SubmissionIDString;
      }
    }

    public DateTime? SubmissionDate
    {
      get
      {
        if (this.CheckForNullString(this.SubmissionDateString))
          return new DateTime?();
        DateTime result;
        return DateTime.TryParse(this.SubmissionDateString, out result) ? new DateTime?(result.ToLocalTime()) : new DateTime?();
      }
    }

    public DateTime? StatusDate
    {
      get
      {
        DateTime result;
        if (!this.CheckForNullString(this.StatusDateString) && DateTime.TryParse(this.StatusDateString, out result))
          return new DateTime?(result.ToLocalTime());
        if (this.SubmissionDate.HasValue && this.SubmissionDate.HasValue)
          return new DateTime?(this.SubmissionDate.Value.ToLocalTime());
        return this.CreateDate.HasValue && this.CreateDate.HasValue ? new DateTime?(this.CreateDate.Value.ToLocalTime()) : new DateTime?();
      }
    }

    public string CreatedBy
    {
      get
      {
        try
        {
          return this.CreatedByString.Split('\\')[2];
        }
        catch
        {
          return (string) null;
        }
      }
    }

    public int AuditCountRed
    {
      get
      {
        int result;
        return int.TryParse(this.AuditCountRedString, out result) ? result : 0;
      }
      set => this.AuditCountRedString = Convert.ToString(value);
    }

    public int AuditCountYellow
    {
      get
      {
        int result;
        return int.TryParse(this.AuditCountYellowString, out result) ? result : 0;
      }
      set => this.AuditCountYellowString = Convert.ToString(value);
    }

    public int AuditCountGreen
    {
      get
      {
        int result;
        return int.TryParse(this.AuditCountGreenString, out result) ? result : 0;
      }
      set => this.AuditCountGreenString = Convert.ToString(value);
    }

    public string SubmissionType
    {
      get
      {
        return this.CheckForNullString(this.SubmissionTypeString) ? "Not Available" : this.SubmissionTypeString;
      }
    }
  }
}
