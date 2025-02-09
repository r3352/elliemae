// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.DataSource.DataSourceWrapperFactory
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using Elli.CalculationEngine.Core.CalculationLibrary;
using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace Elli.CalculationEngine.Core.DataSource
{
  public class DataSourceWrapperFactory
  {
    private Assembly _engineAssembly;

    public DataSourceWrapperFactory(Assembly engineAssembly)
    {
      this._engineAssembly = engineAssembly;
    }

    public DataSourceWrapper CreateDataSourceWrapper(
      IDataSource source,
      CalculationSet calculationSet,
      Dictionary<Tuple<string, string>, TransientField> initialTransients = null)
    {
      DataSourceWrapper dataSourceWrapper;
      if (source != null && source is DataSourceWrapper)
        dataSourceWrapper = (DataSourceWrapper) source;
      else
        dataSourceWrapper = (DataSourceWrapper) Activator.CreateInstance(this._engineAssembly.GetType(string.Format("Elli.CalculationEngine.{0}EntityWrapper.{1}EntityWrapper", (object) calculationSet.Name.Replace(" ", ""), (object) source.GetEntityType().EntityType)), (object) source, (object) calculationSet, (object) initialTransients);
      return dataSourceWrapper;
    }
  }
}
