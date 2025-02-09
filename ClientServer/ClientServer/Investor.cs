// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Investor
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class Investor : BinaryConvertible<Investor>
  {
    private string name = "";
    private Hashtable contactInformations = new Hashtable();
    private List<string> investorContactTypes = new List<string>();
    private int deliveryTimeFrame;
    private Decimal pairOffFee;
    private string typeOfPurchaser = string.Empty;

    public Investor() => this.InitContactInformationCollection();

    public Investor(Investor source) => this.CopyFrom(source);

    public Investor(XmlSerializationInfo info)
    {
      this.name = info.GetString(nameof (name));
      foreach (string name in Enum.GetNames(typeof (InvestorContactType)))
      {
        object retVal;
        ContactInformation contactInformation = info.TryGetValue(name, typeof (ContactInformation), out retVal) ? (ContactInformation) retVal : new ContactInformation();
        if (this.contactInformations.ContainsKey((object) name))
          this.contactInformations.Remove((object) name);
        this.contactInformations.Add((object) name, (object) contactInformation);
      }
      this.investorContactTypes.Clear();
      foreach (string key in (IEnumerable) this.contactInformations.Keys)
        this.investorContactTypes.Add(key);
      this.deliveryTimeFrame = info.GetInteger(nameof (deliveryTimeFrame));
      this.pairOffFee = info.GetDecimal(nameof (pairOffFee));
      try
      {
        this.typeOfPurchaser = info.GetString(nameof (typeOfPurchaser));
      }
      catch
      {
        this.typeOfPurchaser = "";
      }
    }

    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    public Hashtable ContactInformations
    {
      get => this.contactInformations;
      set => this.contactInformations = value;
    }

    public ContactInformation GetContactInformation(InvestorContactType investorContactType)
    {
      return this.GetContactInformation(Enum.GetName(typeof (InvestorContactType), (object) investorContactType));
    }

    public ContactInformation GetContactInformation(string contactType)
    {
      contactType = contactType.ToLower();
      return !this.contactInformations.ContainsKey((object) contactType) ? (ContactInformation) null : this.contactInformations[(object) contactType] as ContactInformation;
    }

    public void AddContactInformation(ContactInformation contactInformation, string contactType)
    {
      if (!this.contactInformations.ContainsKey((object) contactType))
        this.contactInformations.Add((object) contactType, (object) contactInformation);
      else
        this.contactInformations[(object) contactType] = (object) contactInformation;
    }

    public ContactInformation ShippingInformation
    {
      get => this.GetContactInformation(InvestorContactType.shipping);
      set
      {
        if (this.contactInformations.ContainsKey((object) InvestorContactType.shipping) || this.contactInformations.ContainsKey((object) "shipping"))
        {
          this.contactInformations[(object) InvestorContactType.shipping] = (object) value;
        }
        else
        {
          string shippingType = Enum.GetName(typeof (InvestorContactType), (object) InvestorContactType.shipping);
          this.contactInformations.Add((object) shippingType, (object) value);
          if (this.investorContactTypes.Any<string>((Func<string, bool>) (t => string.Equals(t, shippingType))))
            return;
          this.investorContactTypes.Add(shippingType);
        }
      }
    }

    public ContactInformation CustomerServiceInformation
    {
      get => this.GetContactInformation(InvestorContactType.servicing);
    }

    public ContactInformation ContactInformation
    {
      get => this.GetContactInformation(InvestorContactType.contact);
    }

    public ContactInformation TrailingDocumentsInformation
    {
      get => this.GetContactInformation(InvestorContactType.docs);
    }

    public int DeliveryTimeFrame
    {
      get => this.deliveryTimeFrame;
      set => this.deliveryTimeFrame = value;
    }

    public Decimal PairOffFee
    {
      get => this.pairOffFee;
      set => this.pairOffFee = value;
    }

    public string TypeOfPurchaser
    {
      get => this.typeOfPurchaser;
      set => this.typeOfPurchaser = value;
    }

    public void CopyFrom(Investor source)
    {
      this.name = source.ContactInformation.EntityName;
      this.contactInformations = source.contactInformations;
      this.investorContactTypes = source.investorContactTypes;
      this.deliveryTimeFrame = source.deliveryTimeFrame;
      this.pairOffFee = source.pairOffFee;
      this.typeOfPurchaser = source.typeOfPurchaser;
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("name", (object) this.name);
      foreach (string investorContactType in this.investorContactTypes)
        info.AddValue(investorContactType, (object) (this.contactInformations[(object) investorContactType] as ContactInformation));
      info.AddValue("deliveryTimeFrame", (object) this.deliveryTimeFrame);
      info.AddValue("pairOffFee", (object) this.pairOffFee);
      info.AddValue("typeOfPurchaser", (object) this.typeOfPurchaser);
    }

    public void Clear()
    {
      this.name = "";
      this.deliveryTimeFrame = 0;
      this.pairOffFee = 0M;
      this.typeOfPurchaser = "";
      this.InitContactInformationCollection();
    }

    private void InitContactInformationCollection()
    {
      this.investorContactTypes.Clear();
      this.contactInformations.Clear();
      foreach (string name in Enum.GetNames(typeof (InvestorContactType)))
      {
        this.investorContactTypes.Add(name);
        this.contactInformations.Add((object) name, (object) new ContactInformation());
      }
    }
  }
}
