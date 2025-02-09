// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.DbAccessManagerFactory
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using EllieMae.EMLite.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.DataAccess
{
  public class DbAccessManagerFactory : IDataAccessFactory<IDbAccessManager>
  {
    public IDbAccessManager CreateInstance(IDbContext dbContext = null)
    {
      if (dbContext == null)
        dbContext = DataAccessFramework.Runtime.CreateService<IDbContext>();
      if (dbContext == null)
        throw new ArgumentNullException("IDbContext in DbAccessManagerFactory cannot be null.");
      if (dbContext is PgDbContext)
        return (IDbAccessManager) new PgDbAccessManager(dbContext);
      UowDbContext uowDbContext = dbContext as UowDbContext;
      return (IDbAccessManager) new DbAccessManager(dbContext.ConnectionString, DbServerType.SqlServer);
    }
  }
}
