// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ServiceInterface.Services.ILocationLookupService
// Assembly: ServiceInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF0C2B89-A027-4FA0-9669-4D2AA36A4D74
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ServiceInterface.dll

using Elli.Domain.Mortgage;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ServiceInterface.Services
{
  public interface ILocationLookupService
  {
    IEnumerable<ZipCodeInfo> Get(string zip);

    string GetFIPS(string state, string county);

    string GetMSACode(string state, string county);
  }
}
