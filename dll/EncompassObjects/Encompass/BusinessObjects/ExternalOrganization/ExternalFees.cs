// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalFees
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.Encompass.Collections;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  public class ExternalFees : IExternalFees
  {
    private ExternalFeeManagement externalFee;

    internal ExternalFees(ExternalFeeManagement externalFee) => this.externalFee = externalFee;

    public ExternalFees() => this.externalFee = new ExternalFeeManagement();

    public int FeeManagementID => this.externalFee.FeeManagementID;

    public int ExternalOrgID => this.externalFee.ExternalOrgID;

    public string FeeName
    {
      get => this.externalFee.FeeName;
      set => this.externalFee.FeeName = value;
    }

    public string Description
    {
      get => this.externalFee.Description;
      set => this.externalFee.Description = value;
    }

    public string Code
    {
      get => this.externalFee.Code;
      set => this.externalFee.Code = value;
    }

    public ExternalOrganizationEntityType Channel
    {
      get
      {
        switch (this.externalFee.Channel - 1)
        {
          case 0:
            return ExternalOrganizationEntityType.Broker;
          case 1:
            return ExternalOrganizationEntityType.Correspondent;
          case 2:
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
            this.externalFee.Channel = (ExternalOriginatorEntityType) 1;
            break;
          case ExternalOrganizationEntityType.Correspondent:
            this.externalFee.Channel = (ExternalOriginatorEntityType) 2;
            break;
          case ExternalOrganizationEntityType.Both:
            this.externalFee.Channel = (ExternalOriginatorEntityType) 3;
            break;
          default:
            this.externalFee.Channel = (ExternalOriginatorEntityType) 0;
            break;
        }
      }
    }

    public DateTime StartDate
    {
      get => this.externalFee.StartDate;
      set => this.externalFee.StartDate = value;
    }

    public DateTime EndDate
    {
      get => this.externalFee.EndDate;
      set => this.externalFee.EndDate = value;
    }

    public int Condition => 0;

    public string AdvancedCode => this.externalFee.AdvancedCode;

    public string AdvancedCodeXml => this.externalFee.AdvancedCodeXml;

    public double FeePercent
    {
      get => this.externalFee.FeePercent;
      set => this.externalFee.FeePercent = value;
    }

    public double FeeAmount
    {
      get => this.externalFee.FeeAmount;
      set => this.externalFee.FeeAmount = value;
    }

    public int FeeBasedOn
    {
      get => this.externalFee.FeeBasedOn;
      set => this.externalFee.FeeBasedOn = value;
    }

    public string CreatedBy
    {
      get => this.externalFee.CreatedBy;
      set => this.externalFee.CreatedBy = value;
    }

    public DateTime DateCreated
    {
      get => this.externalFee.DateCreated;
      set => this.externalFee.DateCreated = value;
    }

    public string UpdatedBy
    {
      get => this.externalFee.UpdatedBy;
      set => this.externalFee.UpdatedBy = value;
    }

    public DateTime DateUpdated
    {
      get => this.externalFee.DateUpdated;
      set => this.externalFee.DateUpdated = value;
    }

    public ExternalOriginatorFeeStatus Status
    {
      get
      {
        switch (this.externalFee.Status - 1)
        {
          case 0:
            return ExternalOriginatorFeeStatus.Pending;
          case 1:
            return ExternalOriginatorFeeStatus.Active;
          case 2:
            return ExternalOriginatorFeeStatus.NotActive;
          case 3:
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
