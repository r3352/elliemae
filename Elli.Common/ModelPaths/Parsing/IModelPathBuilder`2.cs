// Decompiled with JetBrains decompiler
// Type: Elli.Common.ModelPaths.Parsing.IModelPathBuilder`2
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using System.Text;

#nullable disable
namespace Elli.Common.ModelPaths.Parsing
{
  public interface IModelPathBuilder<TPath, TBuilder>
    where TPath : IModelPath
    where TBuilder : IModelPathBuilder<TPath, TBuilder>
  {
    TBuilder StartFragment(StringBuilder tokenValue);

    TBuilder SetFragmentIndex(FragmentIndexToken index);

    TBuilder SetFragmentIndex(StringBuilder tokenValue, FragmentIndexType fragmentIndexType);

    TBuilder StartQualifier(StringBuilder tokenValue);

    TBuilder SetQualifierValue(StringBuilder tokenValue, QualifierValueType qualifierValueType);

    TBuilder AddQualifierToFragment();

    TBuilder AddFragment();

    TPath Build();
  }
}
