// Decompiled with JetBrains decompiler
// Type: Elli.Common.ModelFields.ModelFieldPath
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

#nullable disable
namespace Elli.Common.ModelFields
{
  public class ModelFieldPath
  {
    private const string RootPath = "Loan�";
    private const string PathSeparator = ".�";
    private ModelFieldPath.ModelFieldPathFragment _currentPath;
    private ModelFieldPath.ModelFieldPathFragmentProperties _currentPathProperties;
    private string _collectionName;
    private static readonly Dictionary<ModelFieldPath.ModelFieldPathFragment, ModelFieldPath.ModelFieldPathFragmentProperties> _modelFragmentProperties = new Dictionary<ModelFieldPath.ModelFieldPathFragment, ModelFieldPath.ModelFieldPathFragmentProperties>();

    public static ModelFieldPath CreateWithFullPath(string path)
    {
      using (PerformanceMeter.Current.BeginOperation("ModelFieldPath.CreateWithFullPath"))
        return path == null || path.Length > "Loan".Length && path.StartsWith("Loan", StringComparison.OrdinalIgnoreCase) ? new ModelFieldPath(path) : throw new Exception(string.Format("Path '{0}' is invalid", (object) path));
    }

    private ModelFieldPath(string path)
    {
      this._currentPath = new ModelFieldPath.ModelFieldPathFragment(path, "Loan", 0);
      this.MoveToNextPath();
    }

    public string FieldName
    {
      get
      {
        if (!this.IsField)
          throw new Exception(string.Format("Current path is not a field name '{0}'", (object) this._currentPath));
        return this._currentPathProperties.ValueExpression;
      }
    }

    public string FullPath => this._currentPath.ModelPath;

    public string CurrentPath => this._currentPath.FragmentPath;

    public string CurrentPathExpression => this._currentPathProperties.ValueExpression;

    public string RemainingPath => this._currentPath.RemainingPath;

    public bool IsField => this.RemainingPath == string.Empty;

    public int Level => this._currentPath.Level;

    public bool IsCollectionExpression => this._currentPathProperties.IsCollection;

    public string CollectionExpression => this._collectionName;

    public void MoveToNextPath()
    {
      using (PerformanceMeter.Current.BeginOperation("ModelFieldPath.MoveToNextPath"))
      {
        this._currentPathProperties = ModelFieldPath.GetFragmentPropeties(this._currentPath);
        if (this._currentPathProperties.ValueExpression == null)
          throw new Exception(string.Format("Error getting current path expression. CurrentFullPath='{0}'", (object) this._currentPath));
        string currentPath = this.CurrentPath;
        this._currentPath = new ModelFieldPath.ModelFieldPathFragment(this._currentPath.ModelPath, string.Format("{0}{1}{2}", (object) this._currentPath.FragmentPath, (object) ".", (object) this._currentPathProperties.ValueExpression), this._currentPath.Level + 1);
        this._collectionName = this._currentPathProperties.IsCollection ? string.Format("{0}{1}{2}", (object) currentPath, (object) ".", (object) ModelFieldPathExpression.Parse(this.CurrentPathExpression).CollectionName) : (string) null;
      }
    }

    public void MemberPathFixup(string correctedMember)
    {
      this._currentPathProperties.ValueExpression = correctedMember;
    }

    private static string GetCollectionExpression(ModelFieldPath.ModelFieldPathFragment pathFragment)
    {
      using (PerformanceMeter.Current.BeginOperation("ModelFieldPath.GetCollectionExpression"))
      {
        int startIndex = pathFragment.FragmentPath.Length + 1;
        string str = pathFragment.ModelPath.Substring(startIndex);
        int num1 = str.IndexOf('[');
        int num2 = str.IndexOf('.');
        if (num1 <= 0 || num1 >= num2)
          return (string) null;
        int num3 = str.IndexOf("].", StringComparison.Ordinal);
        if (num3 <= 0)
          throw new Exception(string.Format("Collection Closing Token Not Found. ModelPath='{0}'", (object) pathFragment.ModelPath));
        int length = num3 + 1;
        return pathFragment.ModelPath.Substring(startIndex, length);
      }
    }

    private static string GetPropertyExpression(ModelFieldPath.ModelFieldPathFragment pathFragment)
    {
      using (PerformanceMeter.Current.BeginOperation("ModelFieldPath.GetPropertyExpression"))
      {
        Match match = new Regex(string.Format("^{0}{1}{2}", (object) pathFragment.FragmentPath.Replace("(", "\\(").Replace(")", "\\)").Replace("[", "\\[").Replace("]", "\\]").Replace("'", "\\'"), (object) ".", (object) "(?<expression>\\w+)")).Match(pathFragment.ModelPath);
        return match.Success ? match.Groups["expression"].Value : (string) null;
      }
    }

    private static ModelFieldPath.ModelFieldPathFragmentProperties GetFragmentPropeties(
      ModelFieldPath.ModelFieldPathFragment fragment)
    {
      ModelFieldPath.ModelFieldPathFragmentProperties fragmentPropeties1 = (ModelFieldPath.ModelFieldPathFragmentProperties) null;
      lock (ModelFieldPath._modelFragmentProperties)
      {
        if (ModelFieldPath._modelFragmentProperties.TryGetValue(fragment, out fragmentPropeties1))
          return fragmentPropeties1;
      }
      ModelFieldPath.ModelFieldPathFragmentProperties fragmentPropeties2 = new ModelFieldPath.ModelFieldPathFragmentProperties();
      fragmentPropeties2.ValueExpression = ModelFieldPath.GetCollectionExpression(fragment);
      if (fragmentPropeties2.ValueExpression != null)
      {
        fragmentPropeties2.IsCollection = true;
      }
      else
      {
        fragmentPropeties2.ValueExpression = ModelFieldPath.GetPropertyExpression(fragment);
        fragmentPropeties2.IsProperty = fragmentPropeties2.ValueExpression != null;
      }
      lock (ModelFieldPath._modelFragmentProperties)
        ModelFieldPath._modelFragmentProperties[fragment] = fragmentPropeties2;
      return fragmentPropeties2;
    }

    private class ModelFieldPathFragment
    {
      public ModelFieldPathFragment(string modelPath, string fragmentPath, int level)
      {
        this.ModelPath = modelPath;
        this.FragmentPath = fragmentPath;
        this.Level = level;
        if (modelPath.Length > fragmentPath.Length)
          this.RemainingPath = modelPath.Substring(fragmentPath.Length + 1, modelPath.Length - fragmentPath.Length - 1);
        else
          this.RemainingPath = string.Empty;
      }

      public string ModelPath { get; private set; }

      public string FragmentPath { get; private set; }

      public int Level { get; private set; }

      public string RemainingPath { get; private set; }

      public override int GetHashCode()
      {
        return StringComparer.OrdinalIgnoreCase.GetHashCode(this.ModelPath) ^ StringComparer.OrdinalIgnoreCase.GetHashCode(this.FragmentPath);
      }

      public override bool Equals(object obj)
      {
        return obj is ModelFieldPath.ModelFieldPathFragment fieldPathFragment && StringComparer.OrdinalIgnoreCase.Compare(this.ModelPath, fieldPathFragment.ModelPath) == 0 && StringComparer.OrdinalIgnoreCase.Compare(this.FragmentPath, fieldPathFragment.FragmentPath) == 0;
      }

      public override string ToString() => this.FragmentPath;
    }

    private class ModelFieldPathFragmentProperties
    {
      public bool IsCollection { get; set; }

      public bool IsProperty { get; set; }

      public string ValueExpression { get; set; }
    }
  }
}
