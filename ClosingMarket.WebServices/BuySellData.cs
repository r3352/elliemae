// Decompiled with JetBrains decompiler
// Type: ClosingMarket.WebServices.BuySellData
// Assembly: ClosingMarket.WebServices, Version=1.0.2749.29102, Culture=neutral, PublicKeyToken=null
// MVID: 510652A0-EF36-486C-9EB6-CECE9FC11560
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClosingMarket.WebServices.dll

using System.Xml.Serialization;

#nullable disable
namespace ClosingMarket.WebServices
{
  [SoapType("BuySellData", "http://www.closingmarket.com")]
  public class BuySellData : SerializableObject
  {
    public string BuySellID;
    public AgentData Attorney;
    public Ten99Data Ten99;
    public string FormalNameOR;
    public string FormalNameText;
    public string SignatureBlock;
    public string SortName;
    public string FormalName;
    public string BusinessName;
    public NameData Name1;
    public NameData Name2;
    public NameRelation Name2Name1Relation;
    public string CurrentAddress;
    public string CurrentCity;
    public string CurrentState;
    public string CurrentZip;
    public string ForwardingAddress;
    public string ForwardingCity;
    public string ForwardingState;
    public string ForwardingZip;
    public string HomePhone;
    public string BusinessPhone;
    public string Fax;
    public string Notes;
  }
}
