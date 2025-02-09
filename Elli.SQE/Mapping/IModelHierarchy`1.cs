// Decompiled with JetBrains decompiler
// Type: Elli.SQE.Mapping.IModelHierarchy`1
// Assembly: Elli.SQE, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 49809150-F4C8-4F09-B50E-E491713B0B30
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.SQE.dll

using Elli.Common.ModelFields;
using System.Collections.Generic;

#nullable disable
namespace Elli.SQE.Mapping
{
  public interface IModelHierarchy<T>
  {
    string Key { get; }

    bool IsRoot { get; }

    List<T> Children { get; set; }

    ModelFieldPath Path { get; }

    bool ExcludeFromMapping { get; }
  }
}
