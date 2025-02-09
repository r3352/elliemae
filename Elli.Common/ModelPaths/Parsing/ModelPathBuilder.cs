// Decompiled with JetBrains decompiler
// Type: Elli.Common.ModelPaths.Parsing.ModelPathBuilder
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using System.Text;

#nullable disable
namespace Elli.Common.ModelPaths.Parsing
{
  public class ModelPathBuilder : IModelPathBuilder<ModelPath, ModelPathBuilder>
  {
    private FragmentToken fragment;
    private QualifierToken qualifier;
    private ModelPath tokens = new ModelPath();

    public ModelPathBuilder StartFragment(StringBuilder tokenValue)
    {
      this.fragment = new FragmentToken(tokenValue);
      return this;
    }

    public ModelPathBuilder SetFragmentIndex(FragmentIndexToken index)
    {
      this.fragment.SetIndex(index);
      return this;
    }

    public ModelPathBuilder SetFragmentIndex(
      StringBuilder tokenValue,
      FragmentIndexType fragmentIndexType)
    {
      this.fragment.SetIndex(new FragmentIndexToken(tokenValue, fragmentIndexType));
      return this;
    }

    public ModelPathBuilder StartQualifier(StringBuilder tokenValue)
    {
      this.qualifier = new QualifierToken(tokenValue);
      return this;
    }

    public ModelPathBuilder SetQualifierValue(
      StringBuilder tokenValue,
      QualifierValueType qualifierValueType)
    {
      this.qualifier.SetValue(tokenValue, qualifierValueType);
      return this;
    }

    public ModelPathBuilder AddFragment()
    {
      this.tokens.Add(this.fragment);
      this.fragment = (FragmentToken) null;
      return this;
    }

    public ModelPathBuilder AddQualifierToFragment()
    {
      this.fragment.AddQualifier(this.qualifier);
      this.qualifier = (QualifierToken) null;
      return this;
    }

    public ModelPath Build()
    {
      if (this.fragment != null)
      {
        this.tokens.Add(this.fragment);
        this.fragment = (FragmentToken) null;
      }
      return this.tokens;
    }
  }
}
