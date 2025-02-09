// Decompiled with JetBrains decompiler
// Type: Elli.Service.SiteSettingService
// Assembly: Elli.Service, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 23E1ED92-4ABE-46FE-97AA-A4B97F8619DF
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.dll

using Elli.Common.Data;
using Elli.Domain;
using Elli.Domain.Settings;
using Elli.Metrics;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Settings;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable disable
namespace Elli.Service
{
  public class SiteSettingService : ISiteSettingService, IDisposable
  {
    private readonly ISiteSettingRepository _siteSettingRepository;
    private StorageSettings _settings;
    private StorageMode _storageMode;
    private IUnitOfWork _unitOfWork;
    private readonly IMetricsFactory _metricsFactory;

    public SiteSettingService(ISiteSettingRepository repository, IMetricsFactory metricsFactory)
    {
      this._siteSettingRepository = repository;
      this._metricsFactory = metricsFactory;
    }

    internal void ApplyConfiguration(StorageSettings storageSettings, StorageMode storageMode)
    {
      this._settings = storageSettings;
      this._storageMode = storageMode;
      this._unitOfWork = IocContainer.CreateUnitOfWork((IStorageSettings) this._settings, this._storageMode);
    }

    public bool CreateSiteSetting(object siteSettingData)
    {
      // ISSUE: reference to a compiler-generated field
      if (SiteSettingService.\u003C\u003Eo__7.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SiteSettingService.\u003C\u003Eo__7.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (bool), typeof (SiteSettingService)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target = SiteSettingService.\u003C\u003Eo__7.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p1 = SiteSettingService.\u003C\u003Eo__7.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (SiteSettingService.\u003C\u003Eo__7.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SiteSettingService.\u003C\u003Eo__7.\u003C\u003Ep__0 = CallSite<Func<CallSite, ISiteSettingRepository, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, nameof (CreateSiteSetting), (IEnumerable<Type>) null, typeof (SiteSettingService), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj = SiteSettingService.\u003C\u003Eo__7.\u003C\u003Ep__0.Target((CallSite) SiteSettingService.\u003C\u003Eo__7.\u003C\u003Ep__0, this._siteSettingRepository, siteSettingData);
      return target((CallSite) p1, obj);
    }

    public bool UpdateSiteSetting(object siteSettingData)
    {
      // ISSUE: reference to a compiler-generated field
      if (SiteSettingService.\u003C\u003Eo__8.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SiteSettingService.\u003C\u003Eo__8.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (bool), typeof (SiteSettingService)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target = SiteSettingService.\u003C\u003Eo__8.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p1 = SiteSettingService.\u003C\u003Eo__8.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (SiteSettingService.\u003C\u003Eo__8.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SiteSettingService.\u003C\u003Eo__8.\u003C\u003Ep__0 = CallSite<Func<CallSite, ISiteSettingRepository, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, nameof (UpdateSiteSetting), (IEnumerable<Type>) null, typeof (SiteSettingService), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj = SiteSettingService.\u003C\u003Eo__8.\u003C\u003Ep__0.Target((CallSite) SiteSettingService.\u003C\u003Eo__8.\u003C\u003Ep__0, this._siteSettingRepository, siteSettingData);
      return target((CallSite) p1, obj);
    }

    public object GetSiteSetting(object siteSettingData, string guid = null)
    {
      // ISSUE: reference to a compiler-generated field
      if (SiteSettingService.\u003C\u003Eo__9.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SiteSettingService.\u003C\u003Eo__9.\u003C\u003Ep__0 = CallSite<Func<CallSite, ISiteSettingRepository, object, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, nameof (GetSiteSetting), (IEnumerable<Type>) null, typeof (SiteSettingService), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return SiteSettingService.\u003C\u003Eo__9.\u003C\u003Ep__0.Target((CallSite) SiteSettingService.\u003C\u003Eo__9.\u003C\u003Ep__0, this._siteSettingRepository, siteSettingData, guid);
    }

    public string GetSiteSettingLibraryId(string siteId)
    {
      return this._siteSettingRepository.GetSiteSettingLibraryId(siteId);
    }

    public IList<object> GetSiteSettings() => this._siteSettingRepository.GetSiteSettings();

    public bool DeleteSiteSetting(string siteId)
    {
      return this._siteSettingRepository.DeleteSiteSetting(siteId);
    }

    public bool DeleteSiteSetting(string siteId, string guid)
    {
      return this._siteSettingRepository.DeleteSiteSetting(siteId, guid);
    }

    public object GetSiteSettingByKey(string siteId, string Key)
    {
      return this._siteSettingRepository.GetSiteSettingByKey(siteId, Key);
    }

    public void Dispose()
    {
      GC.SuppressFinalize((object) this);
      this._unitOfWork.Dispose();
    }
  }
}
