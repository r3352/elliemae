// Decompiled with JetBrains decompiler
// Type: ClosingMarket.WebServices.NameData
// Assembly: ClosingMarket.WebServices, Version=1.0.2749.29102, Culture=neutral, PublicKeyToken=null
// MVID: 510652A0-EF36-486C-9EB6-CECE9FC11560
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClosingMarket.WebServices.dll

using System.Xml.Serialization;

#nullable disable
namespace ClosingMarket.WebServices
{
  [SoapType("NameData", "http://www.closingmarket.com")]
  public class NameData : SerializableObject
  {
    public string First;
    public string MI;
    public string Last;
    public string Ext;
    public string Gender;
    public string TaxID;
    public string Email;
    public string FullName;
  }
}
