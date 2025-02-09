// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.DataSource.IDataEntity
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using System.Collections.Generic;

#nullable disable
namespace Elli.CalculationEngine.Core.DataSource
{
  public interface IDataEntity : IDataElement
  {
    IDataSource GetDataSource();

    EntityDescriptor GetEntityType();

    string GetEntityName();

    IEnumerable<string> GetFieldList();

    IDataField GetField(string fieldId);

    void SetFieldValue(string fieldId, object value);

    object GetFieldValue(string fieldId);

    IDataEntity GetEntityByUniqueName(string entityName);

    IDataEntity GetRelatedEntity(string relationship);

    IEnumerable<IDataEntity> GetRelatedEntities(string relationship, EntityDescriptor descriptor);

    bool IsEntityInRelationship(IDataEntity relatedEntity, string relationship);

    object InvokeMethod(string functionName, params object[] parameters);
  }
}
