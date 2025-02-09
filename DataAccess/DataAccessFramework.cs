// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.DataAccessFramework
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataAccess
{
  public sealed class DataAccessFramework
  {
    private Dictionary<Type, object> _dataAccessFactories = new Dictionary<Type, object>();

    static DataAccessFramework() => DataAccessFramework.Runtime = new DataAccessFramework();

    public static DataAccessFramework Runtime { get; private set; }

    private DataAccessFramework()
    {
    }

    public bool Initialized { get; private set; }

    public void Initialize()
    {
      this.Initialized = !this.Initialized ? true : throw new Exception("The DataAccessFramework has already been initialized");
    }

    public DataAccessFramework Inject<T>(IDataAccessFactory<T> dataAccessFactory) where T : class
    {
      if (this.Initialized)
        throw new Exception("All services must be injected prior to calling Initialize");
      if (!this._dataAccessFactories.ContainsKey(typeof (T)))
        this._dataAccessFactories[typeof (T)] = (object) dataAccessFactory;
      return this;
    }

    public T CreateService<T>(IDbContext dbContext = null) where T : class
    {
      IDataAccessFactory<T> serviceFactory = this.GetServiceFactory<T>();
      return serviceFactory == null ? default (T) : serviceFactory.CreateInstance(dbContext);
    }

    public IDataAccessFactory<T> GetServiceFactory<T>() where T : class
    {
      object obj = (object) null;
      return !this._dataAccessFactories.TryGetValue(typeof (T), out obj) ? (IDataAccessFactory<T>) null : (IDataAccessFactory<T>) obj;
    }

    internal void Dispose()
    {
      this._dataAccessFactories.Clear();
      this.Initialized = false;
    }
  }
}
