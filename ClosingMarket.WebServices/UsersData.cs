// Decompiled with JetBrains decompiler
// Type: ClosingMarket.WebServices.UsersData
// Assembly: ClosingMarket.WebServices, Version=1.0.2749.29102, Culture=neutral, PublicKeyToken=null
// MVID: 510652A0-EF36-486C-9EB6-CECE9FC11560
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClosingMarket.WebServices.dll

using System.Xml.Serialization;

#nullable disable
namespace ClosingMarket.WebServices
{
  [SoapType("UsersData", "http://www.closingmarket.com")]
  public class UsersData : SerializableObject
  {
    public int UsersID;
    public string EMailAddress;
    [SoapElement(DataType = "base64Binary")]
    public byte[] Password;
    public int EnterpriseID;
    public UserGroup UserGroup;
    public UserType UserType;
    public string FirstName;
    public string LastName;
    public string PhoneNumber;
    public string Fax;
    public ContactMethod ContactMethod;
    public bool PrimaryContact;
    public bool ViewAllOrders;
  }
}
