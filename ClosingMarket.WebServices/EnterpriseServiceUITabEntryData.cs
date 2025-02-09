// Decompiled with JetBrains decompiler
// Type: ClosingMarket.WebServices.EnterpriseServiceUITabEntryData
// Assembly: ClosingMarket.WebServices, Version=1.0.2749.29102, Culture=neutral, PublicKeyToken=null
// MVID: 510652A0-EF36-486C-9EB6-CECE9FC11560
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClosingMarket.WebServices.dll

using System.Xml.Serialization;

#nullable disable
namespace ClosingMarket.WebServices
{
  [SoapInclude(typeof (EnterpriseServiceUITabEntryChildData))]
  [SoapType("EnterpriseServiceUITabEntryData", "http://www.closingmarket.com")]
  public class EnterpriseServiceUITabEntryData : SerializableObject
  {
    public int EnterpriseServiceUITabEntryID;
    public int EnterpriseServiceUITabID;
    public string EntryName;
    public string DisplayText;
    public ServiceUIControlType ControlType;
    public ServiceUIReturnType ReturnType;
    public DataValueType DefaultDataValueType;
    public DataValueField DefaultDataValueField;
    public string DefaultConstantValue;
    public bool ReadOnly;
    public bool Required;
    public bool Visible;
    public int OrderPosition;
  }
}
