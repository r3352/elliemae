// Decompiled with JetBrains decompiler
// Type: ClosingMarket.WebServices.TitleCompanyData
// Assembly: ClosingMarket.WebServices, Version=1.0.2749.29102, Culture=neutral, PublicKeyToken=null
// MVID: 510652A0-EF36-486C-9EB6-CECE9FC11560
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClosingMarket.WebServices.dll

using System.Xml.Serialization;

#nullable disable
namespace ClosingMarket.WebServices
{
  [SoapType("TitleCompanyData", "http://www.closingmarket.com")]
  public class TitleCompanyData : SerializableObject
  {
    public string TitleCompanyID;
    public string Name;
    public string Address;
    public string City;
    public string State;
    public string Zip;
    public string ContactName;
    public string Phone;
    public string Fax;
    public string CounterSig;
  }
}
