// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Licensing.EncompassModuleNameProvider
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System.Collections;

#nullable disable
namespace EllieMae.EMLite.Common.Licensing
{
  public class EncompassModuleNameProvider : CustomEnumNameProvider
  {
    private static Hashtable valueToNameMap = new Hashtable();

    static EncompassModuleNameProvider()
    {
      EncompassModuleNameProvider.valueToNameMap.Add((object) EncompassModule.EDM, (object) "Electronic Document Management");
      EncompassModuleNameProvider.valueToNameMap.Add((object) EncompassModule.StatusOnline, (object) "Status Online");
      EncompassModuleNameProvider.valueToNameMap.Add((object) EncompassModule.Mavent, (object) "Compliance Reports");
      EncompassModuleNameProvider.valueToNameMap.Add((object) EncompassModule.Fulfillment, (object) "Fulfillment Service");
      EncompassModuleNameProvider.valueToNameMap.Add((object) EncompassModule.IRS4506T, (object) "4506T Service");
      EncompassModuleNameProvider.valueToNameMap.Add((object) EncompassModule.eClose, (object) "Digital Closing");
      EncompassModuleNameProvider.valueToNameMap.Add((object) EncompassModule.ConsumerConnect, (object) "ConsumerConnect");
    }

    public EncompassModuleNameProvider()
      : base(typeof (EncompassModule), EncompassModuleNameProvider.valueToNameMap)
    {
    }

    public string GetBaseModuleName(EncompassModule module) => this.GetName((object) module);
  }
}
