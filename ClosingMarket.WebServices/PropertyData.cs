// Decompiled with JetBrains decompiler
// Type: ClosingMarket.WebServices.PropertyData
// Assembly: ClosingMarket.WebServices, Version=1.0.2749.29102, Culture=neutral, PublicKeyToken=null
// MVID: 510652A0-EF36-486C-9EB6-CECE9FC11560
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClosingMarket.WebServices.dll

using System.Xml.Serialization;

#nullable disable
namespace ClosingMarket.WebServices
{
  [SoapType("PropertyData", "http://www.closingmarket.com")]
  public class PropertyData : UserFieldDataArray
  {
    public string PropertyID;
    public string Address;
    public string City;
    public string State;
    public string Zip;
    public string CountyName;
    public string CountyCode;
    public string SubDivName;
    public string SubDivPlat;
    public string SubDivFileDate;
    public string AbstractNumber;
    public string Acreage;
    public string TaxID;
    public string LegalDescription;
    public string Block;
    public string Lot;
    public string Range;
    public string Township;
    public string Qtr1;
    public string Qtr2;
    public string Qtr3;
    public string Unit;
    public string Tract;
    public string Book;
    public string Page;
    public string InstrNum;
    public string PlatName;
    public string ARBNum;
    public string OutLot;
    public string District;
    public string Section;
    public string Phase;
    public string Part;
    public string Parcel;
    public string Building;
    public string Municipality;
    public TaxData[] TaxInformation;
  }
}
