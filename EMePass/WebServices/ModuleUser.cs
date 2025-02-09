// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.WebServices.ModuleUser
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using System.Xml.Serialization;

#nullable disable
namespace EllieMae.EMLite.WebServices
{
  [XmlType(Namespace = "http://modules.elliemaeservices.com/jedservices/")]
  public class ModuleUser
  {
    public string UserID;
    public bool PersonalLicense;
    public bool Disabled;
  }
}
