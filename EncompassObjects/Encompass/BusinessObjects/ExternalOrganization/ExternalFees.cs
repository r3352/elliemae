// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalFees
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.Encompass.Collections;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  /// <summary>Represents a single External fees.</summary>
  public class ExternalFees : IExternalFees
  {
    private ExternalFeeManagement externalFee;

    internal ExternalFees(ExternalFeeManagement externalFee) => this.externalFee = externalFee;

    /// <summary>Constructor</summary>
    public ExternalFees() => this.externalFee = new ExternalFeeManagement();

    /// <summary>Gets the FeeManagementID of the Fee record</summary>
    public int FeeManagementID => this.externalFee.FeeManagementID;

    /// <summary>Gets the ExternalOrgID of the Fee record</summary>
    public int ExternalOrgID => this.externalFee.ExternalOrgID;

    /// <summary>Gets or sets the Name of the Fee record</summary>
    public string FeeName
    {
      get => this.externalFee.FeeName;
      set => this.externalFee.FeeName = value;
    }

    /// <summary>Gets or sets the Description of the Fee record</summary>
    public string Description
    {
      get => this.externalFee.Description;
      set => this.externalFee.Description = value;
    }

    /// <summary>Gets or sets the Code of the Fee record</summary>
    public string Code
    {
      get => this.externalFee.Code;
      set => this.externalFee.Code = value;
    }

    /// <summary>Gets or sets the Channel of the Fee record</summary>
    public ExternalOrganizationEntityType Channel
    {
      get
      {
        switch (this.externalFee.Channel)
        {
          case ExternalOriginatorEntityType.Broker:
            return ExternalOrganizationEntityType.Broker;
          case ExternalOriginatorEntityType.Correspondent:
            return ExternalOrganizationEntityType.Correspondent;
          case ExternalOriginatorEntityType.Both:
            return ExternalOrganizationEntityType.Both;
          default:
            return ExternalOrganizationEntityType.None;
        }
      }
      set
      {
        switch (value)
        {
          case ExternalOrganizationEntityType.Broker:
            this.externalFee.Channel = ExternalOriginatorEntityType.Broker;
            break;
          case ExternalOrganizationEntityType.Correspondent:
            this.externalFee.Channel = ExternalOriginatorEntityType.Correspondent;
            break;
          case ExternalOrganizationEntityType.Both:
            this.externalFee.Channel = ExternalOriginatorEntityType.Both;
            break;
          default:
            this.externalFee.Channel = ExternalOriginatorEntityType.None;
            break;
        }
      }
    }

    /// <summary>Gets or sets the Start Date of the Fee record</summary>
    public DateTime StartDate
    {
      get => this.externalFee.StartDate;
      set => this.externalFee.StartDate = value;
    }

    /// <summary>Gets or sets the End Date of the Fee record</summary>
    public DateTime EndDate
    {
      get => this.externalFee.EndDate;
      set => this.externalFee.EndDate = value;
    }

    /// <summary>Gets the condition of the Fee record</summary>
    public int Condition => 0;

    /// <summary>Gets the Advanced Code of the Fee record</summary>
    public string AdvancedCode => this.externalFee.AdvancedCode;

    /// <summary>Gets the Advanced Code Xml of the Fee record</summary>
    public string AdvancedCodeXml => this.externalFee.AdvancedCodeXml;

    /// <summary>Gets or sets the Fee Percent of the Fee record</summary>
    public double FeePercent
    {
      get => this.externalFee.FeePercent;
      set => this.externalFee.FeePercent = value;
    }

    /// <summary>Gets or sets the Fee Amount of the Fee record</summary>
    public double FeeAmount
    {
      get => this.externalFee.FeeAmount;
      set => this.externalFee.FeeAmount = value;
    }

    /// <summary>Gets or sets the Fee Based On of the Fee record</summary>
    public int FeeBasedOn
    {
      get => this.externalFee.FeeBasedOn;
      set => this.externalFee.FeeBasedOn = value;
    }

    /// <summary>Gets or sets the Created By of the Fee record</summary>
    public string CreatedBy
    {
      get => this.externalFee.CreatedBy;
      set => this.externalFee.CreatedBy = value;
    }

    /// <summary>Gets or sets the Creation Date of the Fee record</summary>
    public DateTime DateCreated
    {
      get => this.externalFee.DateCreated;
      set => this.externalFee.DateCreated = value;
    }

    /// <summary>Gets or sets the Updated By of the Fee record</summary>
    public string UpdatedBy
    {
      get => this.externalFee.UpdatedBy;
      set => this.externalFee.UpdatedBy = value;
    }

    /// <summary>Gets or sets the Date Updated of the Fee record</summary>
    public DateTime DateUpdated
    {
      get => this.externalFee.DateUpdated;
      set => this.externalFee.DateUpdated = value;
    }

    /// <summary>Gets the status of the Fee record</summary>
    public ExternalOriginatorFeeStatus Status
    {
      get
      {
        switch (this.externalFee.Status)
        {
          case ExternalOriginatorStatus.Pending:
            return ExternalOriginatorFeeStatus.Pending;
          case ExternalOriginatorStatus.Active:
            return ExternalOriginatorFeeStatus.Active;
          case ExternalOriginatorStatus.NotActive:
            return ExternalOriginatorFeeStatus.NotActive;
          case ExternalOriginatorStatus.Expired:
            return ExternalOriginatorFeeStatus.Expired;
          default:
            return ExternalOriginatorFeeStatus.Active;
        }
      }
    }

    internal static ExternalFeesList ToList(List<ExternalFeeManagement> fees)
    {
      ExternalFeesList list = new ExternalFeesList();
      for (int index = 0; index < fees.Count; ++index)
        list.Add(new ExternalFees(fees[index]));
      return list;
    }
  }
}
