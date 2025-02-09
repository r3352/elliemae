// Decompiled with JetBrains decompiler
// Type: Elli.Common.ModelPaths.Parsing.IModelPath
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

#nullable disable
namespace Elli.Common.ModelPaths.Parsing
{
  public interface IModelPath
  {
    int GetIndexAgnosticModelHashCode();

    ApplicationLookupData GetApplicationLookupData(int requestedIndex);

    bool TryGetCustomFieldLookupData(out CustomFieldlookupData customFieldlookupData);

    int GetCollectionIndex(int requestedIndex);

    int GetPropertyHashCode();

    int GetPropertyHashCode(out string propertyName);
  }
}
