// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.CorrespondentMasterViewModel
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class CorrespondentMasterViewModel : IPropertyDictionary
  {
    private Hashtable data = new Hashtable();

    public CorrespondentMasterViewModelID GetID()
    {
      return new CorrespondentMasterViewModelID()
      {
        CorrespondentMasterID = this.CorrespondentMasterID,
        CorrespondentMasterDeliveryMethodID = this.CorrespondentMasterDeliveryMethodID
      };
    }

    public int CorrespondentMasterID
    {
      get
      {
        return this.getField("CorrespondentMaster.CorrespondentMasterID") == null ? -1 : (int) this.getField("CorrespondentMaster.CorrespondentMasterID");
      }
      set => this.setField("CorrespondentMaster.CorrespondentMasterID", (object) value);
    }

    public int? CorrespondentMasterDeliveryMethodID
    {
      get
      {
        return this.getField("CorrespondentMasterDeliveryMethod.CorrespondentMasterDeliveryMethodID") == null ? new int?(-1) : new int?((int) this.getField("CorrespondentMasterDeliveryMethod.CorrespondentMasterDeliveryMethodID"));
      }
      set
      {
        this.setField("CorrespondentMasterDeliveryMethod.CorrespondentMasterDeliveryMethodID", (object) value);
      }
    }

    public string ContractNumber
    {
      get
      {
        return this.getField("CorrespondentMaster.ContractNumber") == null ? "" : (string) this.getField("CorrespondentMaster.ContractNumber");
      }
      set => this.setField("CorrespondentMaster.ContractNumber", (object) value);
    }

    public int Status
    {
      get
      {
        return this.getField("CorrespondentMaster.Status") == null ? 0 : (int) this.getField("CorrespondentMaster.Status");
      }
      set => this.setField("CorrespondentMaster.Status", (object) value);
    }

    public MasterCommitmentStatus MasterContractStatus => (MasterCommitmentStatus) this.Status;

    public Decimal CommitmentAmount
    {
      get
      {
        return this.getField("CorrespondentMaster.CommitmentAmount") == null ? 0M : (Decimal) this.getField("CorrespondentMaster.CommitmentAmount");
      }
      set => this.setField("CorrespondentMaster.CommitmentAmount", (object) value);
    }

    public DateTime MasterEffectiveDate
    {
      get
      {
        return this.getField("CorrespondentMaster.MasterEffectiveDate") == null ? DateTime.MinValue : (DateTime) this.getField("CorrespondentMaster.MasterEffectiveDate");
      }
      set => this.setField("CorrespondentMaster.MasterEffectiveDate", (object) value);
    }

    public DateTime MasterExpirationDate
    {
      get
      {
        return this.getField("CorrespondentMaster.MasterExpirationDate") == null ? DateTime.MinValue : (DateTime) this.getField("CorrespondentMaster.MasterExpirationDate");
      }
      set => this.setField("CorrespondentMaster.MasterExpirationDate", (object) value);
    }

    public string CompanyName
    {
      get
      {
        return this.getField("CorrespondentMaster.CompanyName") == null ? "" : (string) this.getField("CorrespondentMaster.CompanyName");
      }
      set => this.setField("CorrespondentMaster.CompanyName", (object) value);
    }

    public string TpoId
    {
      get
      {
        return this.getField("CorrespondentMaster.ExternalID") == null ? "" : (string) this.getField("CorrespondentMaster.ExternalID");
      }
      set => this.setField("CorrespondentMaster.ExternalID", (object) value);
    }

    public string OrganizationId
    {
      get
      {
        return this.getField("CorrespondentMaster.OrganizationID") == null ? "" : (string) this.getField("CorrespondentMaster.OrganizationID");
      }
      set => this.setField("CorrespondentMaster.OrganizationID", (object) value);
    }

    public string CommitmentType
    {
      get
      {
        return this.getField("CorrespondentMaster.CommitmentType") == null ? "" : (string) this.getField("CorrespondentMaster.CommitmentType");
      }
      set => this.setField("CorrespondentMaster.CommitmentType", (object) value);
    }

    public string GUID
    {
      get
      {
        return this.getField("CorrespondentMaster.GUID") == null ? "" : (string) this.getField("CorrespondentMaster.GUID");
      }
      set => this.setField("CorrespondentMaster.GUID", (object) value);
    }

    public int ExternalOriginatorManagementID
    {
      get
      {
        return this.getField("CorrespondentMaster.ExternalOriginatorManagementID") == null ? -1 : (int) this.getField("CorrespondentMaster.ExternalOriginatorManagementID");
      }
      set => this.setField("CorrespondentMaster.ExternalOriginatorManagementID", (object) value);
    }

    public virtual string DeliveryType
    {
      get
      {
        return this.getField("CorrespondentMasterDeliveryMethod.DeliveryType") == null ? "" : (string) this.getField("CorrespondentMasterDeliveryMethod.DeliveryType");
      }
      set => this.setField("CorrespondentMasterDeliveryMethod.DeliveryType", (object) value);
    }

    public virtual int DeliveryDays
    {
      get
      {
        return this.getField("CorrespondentMasterDeliveryMethod.DeliveryDays") == null ? 0 : (int) this.getField("CorrespondentMasterDeliveryMethod.DeliveryDays");
      }
      set => this.setField("CorrespondentMasterDeliveryMethod.DeliveryDays", (object) value);
    }

    public virtual string RateSheet
    {
      get
      {
        return this.getField("CorrespondentMaster.RateSheet") == null ? "" : (string) this.getField("CorrespondentMaster.RateSheet");
      }
      set => this.setField("CorrespondentMaster.RateSheet", (object) value);
    }

    public virtual DateTime EffectiveDate
    {
      get
      {
        return this.getField("CorrespondentMasterDeliveryMethod.EffectiveDate") == null ? DateTime.MinValue : (DateTime) this.getField("CorrespondentMasterDeliveryMethod.EffectiveDate");
      }
      set => this.setField("CorrespondentMasterDeliveryMethod.EffectiveDate", (object) value);
    }

    public virtual DateTime ExpirationDate
    {
      get
      {
        return this.getField("CorrespondentMasterDeliveryMethod.ExpirationDate") == null ? DateTime.MinValue : (DateTime) this.getField("CorrespondentMasterDeliveryMethod.ExpirationDate");
      }
      set => this.setField("CorrespondentMasterDeliveryMethod.ExpirationDate", (object) value);
    }

    public virtual Decimal Tolerance
    {
      get
      {
        return this.getField("CorrespondentMasterDeliveryMethod.Tolerance") == null ? 0M : (Decimal) this.getField("CorrespondentMasterDeliveryMethod.Tolerance");
      }
      set => this.setField("CorrespondentMasterDeliveryMethod.Tolerance", (object) value);
    }

    public Decimal AvailableAmount
    {
      get
      {
        return this.getField("CorrespondentMaster.AvailableAmount") == null ? 0M : (Decimal) this.getField("CorrespondentMaster.AvailableAmount");
      }
      set => this.setField("CorrespondentMaster.AvailableAmount", (object) value);
    }

    public override string ToString() => this.ContractNumber;

    protected object getField(string fieldName)
    {
      if (this.data.ContainsKey((object) fieldName))
        return this.data[(object) fieldName];
      if (this.data.ContainsKey((object) fieldName.Replace(" ", "")))
        return this.data[(object) fieldName.Replace(" ", "")];
      if (this.data.ContainsKey((object) ("CorrespondentMaster." + fieldName)))
        return this.data[(object) ("CorrespondentMaster." + fieldName)];
      return this.data.ContainsKey((object) ("CorrrespondentMasterDeliveryMethod." + fieldName)) ? this.data[(object) ("CorrrespondentMasterDeliveryMethod." + fieldName)] : (object) null;
    }

    protected void setField(string fieldName, object value)
    {
      if (this.data.ContainsKey((object) fieldName))
        this.data[(object) fieldName] = value;
      else if (this.data.ContainsKey((object) ("CorrespondentMaster." + fieldName)))
        this.data[(object) ("CorrespondentMaster." + fieldName)] = value;
      else if (this.data.ContainsKey((object) ("CorrrespondentMasterDeliveryMethod." + fieldName)))
        this.data[(object) ("CorrrespondentMasterDeliveryMethod." + fieldName)] = value;
      else
        this.data.Add((object) fieldName, value);
    }

    public object this[string propertyName]
    {
      get => this.getField(propertyName);
      set => throw new NotImplementedException();
    }
  }
}
