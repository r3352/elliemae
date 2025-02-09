// Decompiled with JetBrains decompiler
// Type: Elli.Service.IocContainer
// Assembly: Elli.Service, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 23E1ED92-4ABE-46FE-97AA-A4B97F8619DF
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.dll

using Elli.Common.Data;
using Elli.Data.MongoDB;
using Elli.Data.Orm;
using Elli.Data.Repositories;
using Elli.Domain;
using Elli.Domain.APR;
using Elli.Domain.InputForm;
using Elli.Domain.LoanBatch;
using Elli.Domain.Mortgage;
using Elli.Domain.Security;
using Elli.Domain.Settings;
using Elli.Domain.Site;
using Elli.Metrics;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Settings;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Configuration;

#nullable disable
namespace Elli.Service
{
  public static class IocContainer
  {
    private static readonly Container Container;
    private static bool _registered;
    private static StorageMode _storageMode;
    private static readonly object LockObject = new object();

    static IocContainer() => IocContainer.Container = new Container();

    public static void Register()
    {
      if (IocContainer._registered)
        return;
      lock (IocContainer.Container)
        IocContainer._registered = true;
      if (bool.TryParse(ConfigurationManager.AppSettings["Elli.Service.SkipContainerConfig"], out bool _))
        return;
      IocContainer.Container.Register<ILoanRepository>((Func<ILoanRepository>) (() =>
      {
        switch (IocContainer._storageMode)
        {
          case StorageMode.FileSystemOnly:
          case StorageMode.BothFileSystemMaster:
          case StorageMode.BothDatabaseMaster:
          case StorageMode.DatabaseOnly:
            return (ILoanRepository) IocContainer.Container.GetInstance<Elli.Data.Repositories.LoanRepository>();
          case StorageMode.MongoOnly:
          case StorageMode.MongoFileSystemMaster:
          case StorageMode.MongoMaster:
            return (ILoanRepository) IocContainer.Container.GetInstance<Elli.Data.MongoDB.LoanRepository>();
          default:
            throw new InvalidOperationException("Unsupported StorageMode type");
        }
      }));
      IocContainer.Container.Register<IAuthenticationRepository, AuthenticationRepository>();
      IocContainer.Container.Register<IDraftLoanRepository>((Func<IDraftLoanRepository>) (() =>
      {
        switch (IocContainer._storageMode)
        {
          case StorageMode.FileSystemOnly:
          case StorageMode.BothFileSystemMaster:
          case StorageMode.BothDatabaseMaster:
          case StorageMode.DatabaseOnly:
            return (IDraftLoanRepository) IocContainer.Container.GetInstance<Elli.Data.Repositories.DraftLoanRepository>();
          case StorageMode.MongoOnly:
          case StorageMode.MongoFileSystemMaster:
          case StorageMode.MongoMaster:
            return (IDraftLoanRepository) IocContainer.Container.GetInstance<Elli.Data.MongoDB.DraftLoanRepository>();
          default:
            throw new InvalidOperationException("Unsupported StorageMode type");
        }
      }));
      IocContainer.Container.Register<IInputFormRepository>((Func<IInputFormRepository>) (() => (IInputFormRepository) IocContainer.Container.GetInstance<InputFormRepository>()));
      IocContainer.Container.Register<IInputFormAssetRepository>((Func<IInputFormAssetRepository>) (() => (IInputFormAssetRepository) IocContainer.Container.GetInstance<InputFormAssetRepository>()));
      IocContainer.Container.Register<ISiteSettingRepository>((Func<ISiteSettingRepository>) (() => (ISiteSettingRepository) IocContainer.Container.GetInstance<SiteSettingRepository>()));
      IocContainer.Container.Register<IOrderRatesRepository>((Func<IOrderRatesRepository>) (() => (IOrderRatesRepository) IocContainer.Container.GetInstance<OrderRatesRepository>()));
      IocContainer.Container.Register<ISiteRepository>((Func<ISiteRepository>) (() => (ISiteRepository) IocContainer.Container.GetInstance<SiteRepository>()));
      IocContainer.Container.Register<ITPORecoverCodeSettingRepository>((Func<ITPORecoverCodeSettingRepository>) (() => (ITPORecoverCodeSettingRepository) IocContainer.Container.GetInstance<TPORecoverCodeSettingRepository>()));
      IocContainer.Container.Register<ILoanBatchRepository>((Func<ILoanBatchRepository>) (() => (ILoanBatchRepository) IocContainer.Container.GetInstance<LoanBatchRepository>()));
      IocContainer.Container.Register<IUnitOfWork>((Func<IUnitOfWork>) (() =>
      {
        switch (IocContainer._storageMode)
        {
          case StorageMode.FileSystemOnly:
          case StorageMode.BothFileSystemMaster:
          case StorageMode.BothDatabaseMaster:
          case StorageMode.DatabaseOnly:
            return (IUnitOfWork) IocContainer.Container.GetInstance<UnitOfWork>();
          case StorageMode.MongoOnly:
          case StorageMode.MongoFileSystemMaster:
          case StorageMode.MongoMaster:
            return (IUnitOfWork) IocContainer.Container.GetInstance<MongoUnitOfWork>();
          default:
            throw new InvalidOperationException("Unsupported StorageMode type");
        }
      }));
      IocContainer.Container.Register<IPipelineRepository, PipelineRepository>();
      IocContainer.Container.Register<IMetricsFactory, MetricsFactory>();
      IocContainer.Container.Register<IMortgageService, MortgageService>();
      IocContainer.Container.Register<ISecurityService, SecurityService>();
      IocContainer.Container.Register<IInputFormService, InputFormService>();
      IocContainer.Container.Register<ISiteSettingService, SiteSettingService>();
      IocContainer.Container.Register<IPipelineService, PipelineService>();
      IocContainer.Container.Register<ISiteService, SiteService>();
      IocContainer.Container.Register<ILoanBatchService, LoanBatchService>();
      IocContainer.Container.Register<IOrderRatesService, OrderRatesService>();
    }

    internal static IUnitOfWork CreateUnitOfWork(
      IStorageSettings storageSettings,
      StorageMode storageMode)
    {
      IUnitOfWork instance;
      lock (IocContainer.LockObject)
        instance = IocContainer.Container.GetInstance<IUnitOfWork>();
      instance?.Begin(storageSettings);
      return instance;
    }

    public static IMortgageService CreateMortgageService(
      IStorageSettings storageSettings,
      StorageMode storageMode)
    {
      lock (IocContainer.LockObject)
      {
        IocContainer._storageMode = storageMode;
        MortgageService instance = IocContainer.Container.GetInstance<MortgageService>();
        instance.ApplyConfiguration(storageSettings, storageMode);
        return (IMortgageService) instance;
      }
    }

    public static IMortgageDraftService CreateMortgageDraftService(
      IStorageSettings storageSettings,
      StorageMode storageMode)
    {
      lock (IocContainer.LockObject)
      {
        IocContainer._storageMode = storageMode;
        MortgageDraftService instance = IocContainer.Container.GetInstance<MortgageDraftService>();
        instance.ApplyConfiguration(storageSettings, storageMode);
        return (IMortgageDraftService) instance;
      }
    }

    [Obsolete("This method is obsolete and used by Data Replication project only, For all other purpose use another overloaded method")]
    public static IMortgageService CreateMortgageService(
      Dictionary<string, string> storageSettings,
      StorageMode storageMode)
    {
      StorageSettings storageSettings1 = new StorageSettings();
      if (storageSettings.Count > 0)
        storageSettings1.SqlConnectionString = storageSettings["SqlConnectionString"];
      lock (IocContainer.LockObject)
      {
        IocContainer._storageMode = storageMode;
        MortgageService instance = IocContainer.Container.GetInstance<MortgageService>();
        instance.ApplyConfiguration((IStorageSettings) storageSettings1, storageMode);
        return (IMortgageService) instance;
      }
    }

    public static ISecurityService CreateSecurityService()
    {
      return IocContainer.Container.GetInstance<ISecurityService>();
    }

    public static ISecurityService CreateSecurityService(
      IStorageSettings connectionSettings,
      StorageMode storageMode)
    {
      lock (IocContainer.LockObject)
      {
        IocContainer._storageMode = storageMode;
        SecurityService instance = IocContainer.Container.GetInstance<SecurityService>();
        instance.ApplyConfiguration(connectionSettings, storageMode);
        return (ISecurityService) instance;
      }
    }

    public static IInputFormService CreateInputFormService(
      StorageSettings storageSettings,
      StorageMode storageMode)
    {
      lock (IocContainer.LockObject)
      {
        IocContainer._storageMode = storageMode;
        InputFormService instance = IocContainer.Container.GetInstance<InputFormService>();
        instance.ApplyConfiguration(storageSettings, storageMode);
        return (IInputFormService) instance;
      }
    }

    public static ISiteSettingService CreateSiteSettingService(
      StorageSettings connectionSettings,
      StorageMode storageMode)
    {
      lock (IocContainer.LockObject)
      {
        IocContainer._storageMode = storageMode;
        SiteSettingService instance = IocContainer.Container.GetInstance<SiteSettingService>();
        instance.ApplyConfiguration(connectionSettings, storageMode);
        return (ISiteSettingService) instance;
      }
    }

    public static ISiteService CreateSiteService(
      StorageSettings connectionSettings,
      StorageMode storageMode)
    {
      lock (IocContainer.LockObject)
      {
        IocContainer._storageMode = storageMode;
        SiteService instance = IocContainer.Container.GetInstance<SiteService>();
        instance.ApplyConfiguration(connectionSettings, storageMode);
        return (ISiteService) instance;
      }
    }

    public static ILoanMetricsRecorder CreateLoanMetricsRecorder(IStorageSettings storageSettings)
    {
      return IocContainer.Container.GetInstance<MetricsFactory>().CreateLoanMetricsRecorder(storageSettings.ClientId, storageSettings.InstanceId);
    }

    public static ILoanMetricsRecorder CreateLoanMetricsRecorder(string clientId, string instanceId)
    {
      return IocContainer.Container.GetInstance<MetricsFactory>().CreateLoanMetricsRecorder(clientId, instanceId);
    }

    public static IMongoUtilityMetricsRecorder CreateMongoUtilityMetricsRecorder(
      IStorageSettings storageSettings)
    {
      return IocContainer.Container.GetInstance<MetricsFactory>().CreateMongoUtilityMetricsRecorder(storageSettings.ClientId, storageSettings.InstanceId);
    }

    public static ILoanBatchService CreateLoanBatchService(
      IStorageSettings storageSettings,
      StorageMode storageMode)
    {
      lock (IocContainer.LockObject)
      {
        IocContainer._storageMode = storageMode;
        LoanBatchService instance = IocContainer.Container.GetInstance<LoanBatchService>();
        instance.ApplyConfiguration(storageSettings, storageMode);
        return (ILoanBatchService) instance;
      }
    }

    public static IOrderRatesService SaveEPPSRates(
      StorageSettings connectionSettings,
      StorageMode storageMode)
    {
      lock (IocContainer.LockObject)
      {
        IocContainer._storageMode = storageMode;
        OrderRatesService instance = IocContainer.Container.GetInstance<OrderRatesService>();
        instance.ApplyConfiguration(connectionSettings, storageMode);
        return (IOrderRatesService) instance;
      }
    }
  }
}
