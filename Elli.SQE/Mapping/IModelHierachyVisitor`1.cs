// Decompiled with JetBrains decompiler
// Type: Elli.SQE.Mapping.IModelHierachyVisitor`1
// Assembly: Elli.SQE, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 49809150-F4C8-4F09-B50E-E491713B0B30
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.SQE.dll

#nullable disable
namespace Elli.SQE.Mapping
{
  public interface IModelHierachyVisitor<T> where T : ModelHierarchy<T>
  {
    object Visit(IModelEntityHierarchy<T> obj);

    object Visit(IModelCollectionHierarchy<T> obj);

    object Visit(IModelPropertyHierarchy<T> obj);
  }
}
