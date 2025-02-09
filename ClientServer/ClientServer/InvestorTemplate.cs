// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.InvestorTemplate
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class InvestorTemplate : BinaryConvertible<InvestorTemplate>, ITemplateSetting
  {
    public const string BulkSaleProperty = "BulkSale�";
    public const string DeliveryTimeFrameProperty = "DeliveryTimeFrame�";
    public const string PairOffFeeProperty = "PairOffFee�";
    public const string TypeOfPurchaserProperty = "TypeOfPurchaser�";
    private string guid;
    private Investor companyInfo;
    private bool bulkSale = true;

    public InvestorTemplate()
      : this(System.Guid.NewGuid().ToString())
    {
    }

    public InvestorTemplate(string guid)
    {
      this.guid = guid;
      this.companyInfo = new Investor();
    }

    public InvestorTemplate(XmlSerializationInfo info)
    {
      this.guid = info.GetString(nameof (guid));
      this.companyInfo = (Investor) info.GetValue(nameof (companyInfo), typeof (Investor));
      this.bulkSale = info.GetBoolean(nameof (bulkSale));
    }

    private InvestorTemplate(InvestorTemplate source)
      : this()
    {
      this.companyInfo = new Investor(source.companyInfo);
      this.companyInfo.Name = "";
      this.bulkSale = source.bulkSale;
    }

    public string Guid => this.guid;

    public Investor CompanyInformation => this.companyInfo;

    public bool BulkSale
    {
      get => this.bulkSale;
      set => this.bulkSale = value;
    }

    public string TemplateName
    {
      get => this.companyInfo.Name;
      set => this.companyInfo.Name = value;
    }

    public string Description
    {
      get => "";
      set
      {
      }
    }

    public Hashtable GetProperties()
    {
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      insensitiveHashtable.Add((object) "Guid", (object) this.guid);
      insensitiveHashtable.Add((object) "BulkSale", this.bulkSale ? (object) "Yes" : (object) "No");
      insensitiveHashtable.Add((object) "DeliveryTimeFrame", (object) this.companyInfo.DeliveryTimeFrame);
      insensitiveHashtable.Add((object) "PairOffFee", (object) this.CompanyInformation.PairOffFee);
      insensitiveHashtable.Add((object) "TypeOfPurchaser", (object) this.CompanyInformation.TypeOfPurchaser);
      return insensitiveHashtable;
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("guid", (object) this.guid);
      info.AddValue("companyInfo", (object) this.companyInfo);
      info.AddValue("bulkSale", (object) this.bulkSale);
    }

    public ITemplateSetting Duplicate() => (ITemplateSetting) new InvestorTemplate(this);

    public static explicit operator InvestorTemplate(BinaryObject o)
    {
      return BinaryConvertible<InvestorTemplate>.Parse(o);
    }
  }
}
