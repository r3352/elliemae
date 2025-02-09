// Decompiled with JetBrains decompiler
// Type: Elli.Service.IInputFormService
// Assembly: Elli.Service, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 23E1ED92-4ABE-46FE-97AA-A4B97F8619DF
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.dll

using System.Collections.Generic;

#nullable disable
namespace Elli.Service
{
  public interface IInputFormService
  {
    string CreateInputForm(object inputFormObject);

    void UpdateInputForm(string inputFormId, object inputFormObject);

    object GetInputForm(string inputFormId);

    object GetInputFormByName(string inputFormName);

    void DeleteInputForm(string inputFormId);

    IList<object> GetInputForms();

    IList<object> GetInputForms(string assetId);

    string CreateInputFormAsset(object inputFormAssetObject);

    object GetInputFormAsset(string assetId);

    void UpdateInputFormAsset(string assetId, object asset);

    void DeleteInputFormAsset(string assetId);

    IList<object> GetInputFormAssets();

    IList<object> GetInputFormAssetsByType(string type);

    IList<object> GetInputFormAssetsByInputForm(string inputFormId);
  }
}
