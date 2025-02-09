// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.LoanDataAccessorFactory
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.Metrics;
using EllieMae.EMLite.DataAccess;
using System;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class LoanDataAccessorFactory : 
    DataAccessorFactoryBase,
    IDataAccessFactory<ILoanDataAccessor>
  {
    public ILoanDataAccessor CreateInstance(IDbContext dbContext = null)
    {
      if (dbContext == null)
        dbContext = this.CreateDbContext();
      ILoanAccessorMetricsRecorder accessorMetricsRecorder = this.CreateLoanAccessorMetricsRecorder();
      ILoanSerializationMatrixRecorder serializationMetricsRecorder = this.CreateLoanSerializationMetricsRecorder();
      if (dbContext is PgDbContext)
        return (ILoanDataAccessor) new LoanAccessor(dbContext, accessorMetricsRecorder, serializationMetricsRecorder);
      if (dbContext is UowDbContext)
        return (ILoanDataAccessor) new LoanMortgageDataAccessorWrapper(this.Settings);
      if (dbContext is MsDbContext)
        throw new NotImplementedException("MsDbContext is currently not supported by the LoanDataAccessorFactory");
      return (ILoanDataAccessor) null;
    }
  }
}
