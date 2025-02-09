// Decompiled with JetBrains decompiler
// Type: ClosingMarket.WebServices.UserGroup
// Assembly: ClosingMarket.WebServices, Version=1.0.2749.29102, Culture=neutral, PublicKeyToken=null
// MVID: 510652A0-EF36-486C-9EB6-CECE9FC11560
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClosingMarket.WebServices.dll

using System;
using System.Xml.Serialization;

#nullable disable
namespace ClosingMarket.WebServices
{
  [SoapType("UserGroup", "http://www.closingmarket.com")]
  [Flags]
  public enum UserGroup
  {
    None = 0,
    EnterpriseAdministrator = 1,
    CMSupervisor = 2,
    User = CMSupervisor | EnterpriseAdministrator, // 0x00000003
    CMSupport = 4,
    CMBilling = CMSupport | EnterpriseAdministrator, // 0x00000005
    CMSupportGroup = CMSupport | CMSupervisor, // 0x00000006
    CMBillingGroup = CMSupportGroup | EnterpriseAdministrator, // 0x00000007
    CMUserGroup = 8,
    EnterpriseAdministratorGroup = CMUserGroup | EnterpriseAdministrator, // 0x00000009
  }
}
