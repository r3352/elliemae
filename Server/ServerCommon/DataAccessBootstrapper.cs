// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerCommon.DataAccessBootstrapper
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.Server.ServerObjects;

#nullable disable
namespace EllieMae.EMLite.Server.ServerCommon
{
  public static class DataAccessBootstrapper
  {
    private static bool _initialized = false;
    private static readonly object _lock = new object();

    public static bool Initialized => DataAccessBootstrapper._initialized;

    public static void Initialize()
    {
      lock (DataAccessBootstrapper._lock)
      {
        if (DataAccessBootstrapper._initialized)
          return;
        DataAccessBootstrapper.InitDataAccessFramework();
        DataAccessBootstrapper._initialized = true;
      }
    }

    private static void InitDataAccessFramework()
    {
      DataAccessFramework.Runtime.Inject<IDbAccessManager>((IDataAccessFactory<IDbAccessManager>) new DbAccessManagerFactory()).Inject<ILoanDataAccessor>((IDataAccessFactory<ILoanDataAccessor>) new LoanDataAccessorFactory()).Inject<IAttachmentAccessor>((IDataAccessFactory<IAttachmentAccessor>) new AttachmentDataAccessorFactory()).Initialize();
    }
  }
}
