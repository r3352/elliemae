// Decompiled with JetBrains decompiler
// Type: Elli.Web.Host.ModuleParameters
// Assembly: Elli.Web.Host, Version=19.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9006AF5-0CBA-41F3-A51F-BE05E37C7972
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Web.Host.dll

using System.Collections.Generic;

#nullable disable
namespace Elli.Web.Host
{
  public class ModuleParameters
  {
    public ModuleUser User { get; set; }

    public Dictionary<string, object> Parameters { get; set; }

    public ModuleParameters()
    {
    }

    public ModuleParameters(ModuleUser user, Dictionary<string, object> parameters)
    {
      this.User = user;
      this.Parameters = parameters;
    }
  }
}
