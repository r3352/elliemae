// Decompiled with JetBrains decompiler
// Type: Elli.Service.ISiteService
// Assembly: Elli.Service, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 23E1ED92-4ABE-46FE-97AA-A4B97F8619DF
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Elli.Service
{
  public interface ISiteService
  {
    bool CreateAdminRule(string siteId, Guid ruleId, string AdminRuleData);

    bool UpdateAdminRule(string siteId, Guid ruleId, string AdminRuleData);

    string GetAdminRule(string siteId, Guid ruleId);

    List<string> GetAdminRules(string siteId);

    bool DeleteAdminRule(string siteId, Guid ruleId);

    bool UpdateAdminRuleOrder(string siteId, List<Guid> AdminRuleOrderIds);

    List<string> GetAdminRuleOrder(string siteId);
  }
}
