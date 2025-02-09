// Decompiled with JetBrains decompiler
// Type: Elli.Service.InputFormService
// Assembly: Elli.Service, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 23E1ED92-4ABE-46FE-97AA-A4B97F8619DF
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.dll

using Elli.Common.Data;
using Elli.Domain;
using Elli.Domain.InputForm;
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
  public class InputFormService : IInputFormService, IDisposable
  {
    private readonly IInputFormRepository _inputFormRepository;
    private readonly IInputFormAssetRepository _inputFormAssetRepository;
    private StorageSettings _storageSettings;
    private StorageMode _storageMode;
    private IUnitOfWork _unitOfWork;
    private readonly IMetricsFactory _metricsFactory;
    private IFormBuilderMetricsRecorder _metricsRecorder;

    public InputFormService(
      IInputFormRepository repository,
      IInputFormAssetRepository assetRepository,
      IMetricsFactory metricsFactory)
    {
      this._inputFormRepository = repository;
      this._inputFormAssetRepository = assetRepository;
      this._metricsFactory = metricsFactory;
    }

    internal void ApplyConfiguration(StorageSettings storageSettings, StorageMode storageMode)
    {
      this._storageSettings = storageSettings;
      this._storageMode = storageMode;
      this._unitOfWork = IocContainer.CreateUnitOfWork((IStorageSettings) this._storageSettings, this._storageMode);
    }

    private IFormBuilderMetricsRecorder MetricsRecorder
    {
      get
      {
        return this._metricsRecorder != null ? this._metricsRecorder : (this._metricsRecorder = this._metricsFactory.CreateFormBuilderMetricsRecorder(this._storageSettings.ClientId ?? string.Empty, this._storageSettings.InstanceId ?? string.Empty));
      }
    }

    public string CreateInputForm(object inputFormObject)
    {
      using (this.MetricsRecorder.RecordInputFormServiceTime(nameof (CreateInputForm)))
      {
        // ISSUE: reference to a compiler-generated field
        if (InputFormService.\u003C\u003Eo__11.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          InputFormService.\u003C\u003Eo__11.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (InputFormService)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, string> target = InputFormService.\u003C\u003Eo__11.\u003C\u003Ep__1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, string>> p1 = InputFormService.\u003C\u003Eo__11.\u003C\u003Ep__1;
        // ISSUE: reference to a compiler-generated field
        if (InputFormService.\u003C\u003Eo__11.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          InputFormService.\u003C\u003Eo__11.\u003C\u003Ep__0 = CallSite<Func<CallSite, IInputFormRepository, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, nameof (CreateInputForm), (IEnumerable<Type>) null, typeof (InputFormService), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj = InputFormService.\u003C\u003Eo__11.\u003C\u003Ep__0.Target((CallSite) InputFormService.\u003C\u003Eo__11.\u003C\u003Ep__0, this._inputFormRepository, inputFormObject);
        return target((CallSite) p1, obj);
      }
    }

    public void UpdateInputForm(string inputFormId, object inputFormObject)
    {
      using (this.MetricsRecorder.RecordInputFormServiceTime(nameof (UpdateInputForm)))
      {
        // ISSUE: reference to a compiler-generated field
        if (InputFormService.\u003C\u003Eo__12.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          InputFormService.\u003C\u003Eo__12.\u003C\u003Ep__0 = CallSite<Action<CallSite, IInputFormRepository, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, nameof (UpdateInputForm), (IEnumerable<Type>) null, typeof (InputFormService), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        InputFormService.\u003C\u003Eo__12.\u003C\u003Ep__0.Target((CallSite) InputFormService.\u003C\u003Eo__12.\u003C\u003Ep__0, this._inputFormRepository, inputFormId, inputFormObject);
      }
    }

    public object GetInputForm(string inputFormId)
    {
      using (this.MetricsRecorder.RecordInputFormServiceTime(nameof (GetInputForm)))
        return this._inputFormRepository.GetInputForm(inputFormId);
    }

    public object GetInputFormByName(string inputFormName)
    {
      using (this.MetricsRecorder.RecordInputFormServiceTime(nameof (GetInputFormByName)))
        return this._inputFormRepository.GetInputFormByName(inputFormName);
    }

    public void DeleteInputForm(string inputFormId)
    {
      using (this.MetricsRecorder.RecordInputFormServiceTime(nameof (DeleteInputForm)))
        this._inputFormRepository.DeleteInputForm(inputFormId);
    }

    public IList<object> GetInputForms()
    {
      using (this.MetricsRecorder.RecordInputFormServiceTime(nameof (GetInputForms)))
        return this._inputFormRepository.GetInputForms();
    }

    public IList<object> GetInputForms(string assetId)
    {
      using (this.MetricsRecorder.RecordInputFormServiceTime(nameof (GetInputForms)))
        return this._inputFormRepository.GetInputForms(assetId);
    }

    public string CreateInputFormAsset(object inputFormAssetObject)
    {
      using (this.MetricsRecorder.RecordInputFormServiceTime(nameof (CreateInputFormAsset)))
      {
        // ISSUE: reference to a compiler-generated field
        if (InputFormService.\u003C\u003Eo__18.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          InputFormService.\u003C\u003Eo__18.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (InputFormService)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, string> target = InputFormService.\u003C\u003Eo__18.\u003C\u003Ep__1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, string>> p1 = InputFormService.\u003C\u003Eo__18.\u003C\u003Ep__1;
        // ISSUE: reference to a compiler-generated field
        if (InputFormService.\u003C\u003Eo__18.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          InputFormService.\u003C\u003Eo__18.\u003C\u003Ep__0 = CallSite<Func<CallSite, IInputFormAssetRepository, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, nameof (CreateInputFormAsset), (IEnumerable<Type>) null, typeof (InputFormService), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj = InputFormService.\u003C\u003Eo__18.\u003C\u003Ep__0.Target((CallSite) InputFormService.\u003C\u003Eo__18.\u003C\u003Ep__0, this._inputFormAssetRepository, inputFormAssetObject);
        return target((CallSite) p1, obj);
      }
    }

    public object GetInputFormAsset(string assetId)
    {
      using (this.MetricsRecorder.RecordInputFormServiceTime(nameof (GetInputFormAsset)))
        return this._inputFormAssetRepository.GetInputFormAsset(assetId);
    }

    public void UpdateInputFormAsset(string assetId, object asset)
    {
      using (this.MetricsRecorder.RecordInputFormServiceTime(nameof (UpdateInputFormAsset)))
      {
        // ISSUE: reference to a compiler-generated field
        if (InputFormService.\u003C\u003Eo__20.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          InputFormService.\u003C\u003Eo__20.\u003C\u003Ep__0 = CallSite<Action<CallSite, IInputFormAssetRepository, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, nameof (UpdateInputFormAsset), (IEnumerable<Type>) null, typeof (InputFormService), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        InputFormService.\u003C\u003Eo__20.\u003C\u003Ep__0.Target((CallSite) InputFormService.\u003C\u003Eo__20.\u003C\u003Ep__0, this._inputFormAssetRepository, assetId, asset);
      }
    }

    public void DeleteInputFormAsset(string assetId)
    {
      using (this.MetricsRecorder.RecordInputFormServiceTime(nameof (DeleteInputFormAsset)))
        this._inputFormAssetRepository.DeleteInputFormAsset(assetId);
    }

    public IList<object> GetInputFormAssets()
    {
      using (this.MetricsRecorder.RecordInputFormServiceTime(nameof (GetInputFormAssets)))
        return this._inputFormAssetRepository.GetInputFormAssets();
    }

    public IList<object> GetInputFormAssetsByType(string type)
    {
      using (this.MetricsRecorder.RecordInputFormServiceTime(nameof (GetInputFormAssetsByType)))
        return this._inputFormAssetRepository.GetInputFormAssetsByType(type);
    }

    public IList<object> GetInputFormAssetsByInputForm(string inputFormId)
    {
      using (this.MetricsRecorder.RecordInputFormServiceTime(nameof (GetInputFormAssetsByInputForm)))
      {
        IList<string> assetIdsByInputForm = this._inputFormRepository.GetInputFormAssetIdsByInputForm(inputFormId);
        return assetIdsByInputForm.Count > 0 ? this._inputFormAssetRepository.GetInputFormAssetsByAssetIds(assetIdsByInputForm) : (IList<object>) new List<object>();
      }
    }

    public void Dispose()
    {
      GC.SuppressFinalize((object) this);
      this._unitOfWork.Dispose();
    }
  }
}
