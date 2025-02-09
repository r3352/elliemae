// Decompiled with JetBrains decompiler
// Type: Elli.SQE.CanonicalPath
// Assembly: Elli.SQE, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 49809150-F4C8-4F09-B50E-E491713B0B30
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.SQE.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Elli.SQE
{
  public class CanonicalPath
  {
    private const string Delimiter = "^";
    private readonly List<CanonicalPath.Fragment> _fragments = new List<CanonicalPath.Fragment>();
    private string _delimiter = "^";
    private readonly ICanonicalFragmentFormatter _formatter;
    public static readonly Type[] AllTypes = new Type[5]
    {
      typeof (CanonicalPath.Fragment),
      typeof (CanonicalPath.CollectionFragment),
      typeof (CanonicalPath.QualifierFragment),
      typeof (CanonicalPath.IndexFragment),
      typeof (CanonicalPath.PropertyFragment)
    };

    public CanonicalPath(ICanonicalFragmentFormatter formatter = null)
    {
      this._formatter = formatter;
    }

    public void SetDelimiter(string delimiter) => this._delimiter = delimiter;

    public void Combine(CanonicalPath.Fragment fragment) => this._fragments.Add(fragment);

    public ICanonicalFragmentFormatter Formatter => this._formatter;

    public List<CanonicalPath.Fragment> Fragments => this._fragments;

    public string First
    {
      get => this._fragments.First<CanonicalPath.Fragment>().ToString(this._formatter);
    }

    public string Last => this._fragments.Last<CanonicalPath.Fragment>().ToString(this._formatter);

    public bool HasAnyFragmentTypes(Type[] filterTypes)
    {
      return this._fragments.Any<CanonicalPath.Fragment>((Func<CanonicalPath.Fragment, bool>) (f => filterTypes != null && ((IEnumerable<Type>) filterTypes).Contains<Type>(f.GetType())));
    }

    public CanonicalPath Clone() => this.Clone(CanonicalPath.AllTypes);

    public CanonicalPath Clone(Type[] filterTypes, bool include = true)
    {
      CanonicalPath canonicalPath = new CanonicalPath(this._formatter);
      canonicalPath.SetDelimiter(this._delimiter);
      foreach (CanonicalPath.Fragment fragment in this._fragments)
      {
        if (((filterTypes == null ? 0 : (((IEnumerable<Type>) filterTypes).Contains<Type>(fragment.GetType()) ? 1 : 0)) & (include ? 1 : 0)) != 0)
          canonicalPath.Combine(fragment);
        if (filterTypes != null && !((IEnumerable<Type>) filterTypes).Contains<Type>(fragment.GetType()) && !include)
          canonicalPath.Combine(fragment);
        if (filterTypes == null && !include)
          canonicalPath.Combine(fragment);
      }
      return canonicalPath;
    }

    public string ToUnformattedString(bool isWithoutDelimiter = false)
    {
      return new string(this.ToString(this._fragments.Count, isWithoutDelimiter).Where<char>((Func<char, bool>) (c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '-')).ToArray<char>());
    }

    public string ToString(int upperFragmentIndex, bool isWithoutDelimiter = false)
    {
      string str = "";
      for (int index = 0; index < upperFragmentIndex; ++index)
        str = !(index == 0 | isWithoutDelimiter) ? str + this._delimiter + this._fragments[index].ToString(this._formatter) : str + this._fragments[index].ToString(this._formatter);
      return str;
    }

    public override string ToString() => this.ToString(this._fragments.Count);

    public Queue<CanonicalPath.Fragment> ToQueue()
    {
      Queue<CanonicalPath.Fragment> queue = new Queue<CanonicalPath.Fragment>();
      foreach (CanonicalPath.Fragment fragment in this.Fragments)
        queue.Enqueue(fragment);
      return queue;
    }

    public CanonicalPath TrimStart()
    {
      CanonicalPath canonicalPath = new CanonicalPath(this._formatter);
      canonicalPath.SetDelimiter(this._delimiter);
      for (int index = 0; index < this._fragments.Count; ++index)
      {
        if (index != 0)
          canonicalPath.Combine(this._fragments[index]);
      }
      return canonicalPath;
    }

    public abstract class Fragment
    {
      private readonly string _text;

      protected Fragment(string text) => this._text = text;

      public override string ToString() => this._text;

      public string ToString(ICanonicalFragmentFormatter formatter)
      {
        return formatter != null ? formatter.Format(this.ToString()) : this.ToString();
      }
    }

    public class EntityFragment(string entityName) : CanonicalPath.Fragment(entityName)
    {
    }

    public class CollectionFragment(string collectionName) : CanonicalPath.Fragment(collectionName)
    {
    }

    public class QualifierFragment(string qualifierName, string qualifierValue) : 
      CanonicalPath.Fragment(qualifierName + qualifierValue)
    {
    }

    public class IndexFragment(int index) : CanonicalPath.Fragment(index.ToString())
    {
    }

    public class PropertyFragment(string propertyName) : CanonicalPath.Fragment(propertyName)
    {
    }
  }
}
