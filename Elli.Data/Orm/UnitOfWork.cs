// Decompiled with JetBrains decompiler
// Type: Elli.Data.Orm.UnitOfWork
// Assembly: Elli.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5199CF45-D8E1-4436-8A49-245565D9CA6B
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Data.dll

using Elli.Common.Data;
using Elli.Domain;
using EllieMae.EMLite.Common.Settings;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

#nullable disable
namespace Elli.Data.Orm
{
  public sealed class UnitOfWork : IUnitOfWork, IDisposable
  {
    private static ISessionFactory _currentSessionFactory;

    public static ISession GetCurrentSession()
    {
      return UnitOfWork.CurrentSessionFactory.GetCurrentSession();
    }

    public static IStatelessSession OpenStatelessSession()
    {
      return UnitOfWork.CurrentSessionFactory.OpenStatelessSession();
    }

    public void Begin()
    {
      string connectionString = ConfigurationManager.ConnectionStrings["EncompassTestConnection"] == null ? (string) null : ConfigurationManager.ConnectionStrings["EncompassTestConnection"].ConnectionString;
      this.Begin((IStorageSettings) new StorageSettings()
      {
        SqlConnectionString = connectionString
      });
    }

    public void Begin(IStorageSettings storageSettings)
    {
      string connectionString = storageSettings.SqlConnectionString;
      lock (typeof (UnitOfWork))
      {
        if (UnitOfWork.CurrentSessionFactory == null)
          UnitOfWork._currentSessionFactory = UnitOfWork.Initialize(connectionString);
      }
      if (CurrentSessionContext.HasBind(UnitOfWork.CurrentSessionFactory))
        return;
      SqlConnection sqlConnection = new SqlConnection(connectionString);
      sqlConnection.Open();
      CurrentSessionContext.Bind(UnitOfWork.CurrentSessionFactory.OpenSession((IDbConnection) sqlConnection));
    }

    public void Commit()
    {
      ISessionFactory currentSessionFactory = UnitOfWork.CurrentSessionFactory;
      if (currentSessionFactory == null || !CurrentSessionContext.HasBind(currentSessionFactory))
        return;
      ISession currentSession = currentSessionFactory.GetCurrentSession();
      CurrentSessionContext.Unbind(currentSessionFactory);
      ((IDisposable) currentSession).Dispose();
    }

    public void Dispose()
    {
      GC.SuppressFinalize((object) this);
      this.Commit();
    }

    private static ISessionFactory Initialize(string connectionString)
    {
      NHibernate.Cfg.Configuration configuration = new NHibernate.Cfg.Configuration();
      try
      {
        configuration.Configure();
      }
      catch (HibernateConfigException ex)
      {
      }
      configuration.SetProperty("dialect", "NHibernate.Dialect.MsSql2005Dialect");
      configuration.SetProperty("connection.connection_string", connectionString);
      if (string.IsNullOrEmpty(configuration.GetProperty("adonet.batch_size")))
        configuration.SetProperty("adonet.batch_size", "50");
      if (string.IsNullOrEmpty(configuration.GetProperty("current_session_context_class")))
        configuration.SetProperty("current_session_context_class", "call");
      configuration.AddAssembly("Elli.Data");
      return configuration.BuildSessionFactory();
    }

    private static ISessionFactory CurrentSessionFactory => UnitOfWork._currentSessionFactory;
  }
}
