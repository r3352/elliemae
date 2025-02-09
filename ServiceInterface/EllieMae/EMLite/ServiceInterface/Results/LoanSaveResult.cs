// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ServiceInterface.Results.LoanSaveResult
// Assembly: ServiceInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF0C2B89-A027-4FA0-9669-4D2AA36A4D74
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ServiceInterface.dll

using Elli.BusinessRules;
using Elli.Common;
using Elli.Domain.Mortgage;
using System;

#nullable disable
namespace EllieMae.EMLite.ServiceInterface.Results
{
  public class LoanSaveResult
  {
    public Loan Loan { get; set; }

    public EmList<FieldValidationError> FieldValidationErrors { get; set; }

    public MilestoneValidationError MilestoneValidationErrors { get; set; }

    public string LoanValidationErrorMessage { get; set; }

    public Guid EncompassId { get; set; }

    public string LoanNumber { get; set; }

    public bool HasError => !this.IsValid;

    public bool IsValid
    {
      get
      {
        return (this.FieldValidationErrors == null || this.FieldValidationErrors.Count == 0) && (this.MilestoneValidationErrors == null || this.MilestoneValidationErrors.RequiredFields.Count == 0 && this.MilestoneValidationErrors.RequiredDocs.Count == 0 && this.MilestoneValidationErrors.RequiredTasks.Count == 0 && string.IsNullOrEmpty(this.MilestoneValidationErrors.NextMilestoneUser)) && string.IsNullOrEmpty(this.LoanValidationErrorMessage);
      }
    }
  }
}
