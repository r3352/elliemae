// Decompiled with JetBrains decompiler
// Type: ClosingMarket.WebServices.AgentData
// Assembly: ClosingMarket.WebServices, Version=1.0.2749.29102, Culture=neutral, PublicKeyToken=null
// MVID: 510652A0-EF36-486C-9EB6-CECE9FC11560
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClosingMarket.WebServices.dll

using System.Xml.Serialization;

#nullable disable
namespace ClosingMarket.WebServices
{
  [SoapType("AgentData", "http://www.closingmarket.com")]
  public class AgentData : SerializableObject
  {
    public string Name;
    public string Address;
    public string City;
    public string State;
    public string Zip;
    public string Phone;
    public string Fax;
    public string ContactFirst;
    public string ContactLast;
    public string ContactFull;
    public string Email;
    public string WebSite;
    public string AgentType;
    public string AgentContext;
    public string FriendlyName;
    public string Notes;
  }
}
