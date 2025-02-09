// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Query.DataExpression
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Query
{
  [Serializable]
  public class DataExpression : IQueryTerm
  {
    private string expression;
    private IQueryTerm[] terms;
    private string name;

    public DataExpression(string name, string expression, params string[] fields)
    {
      this.name = name;
      this.expression = expression;
      this.terms = new IQueryTerm[fields.Length];
      for (int index = 0; index < fields.Length; ++index)
        this.terms[index] = QueryTerm.Parse(fields[index]);
    }

    public DataExpression(string name, string expression, params IQueryTerm[] terms)
    {
      this.name = name;
      this.expression = expression;
      this.terms = terms;
    }

    public string FieldName => this.name;

    public string Expression => this.expression;

    public IQueryTerm[] Terms => this.terms;

    public string ToString(ICriterionTranslator translator)
    {
      string str = this.expression;
      for (int index = 0; index < this.terms.Length; ++index)
        str = str.Replace("{" + (object) index + "}", this.terms[index].ToString(translator));
      return str;
    }

    public string ToString(ICriterionTranslator translator, bool withTableName)
    {
      string str = this.expression;
      for (int index = 0; index < this.terms.Length; ++index)
        str = str.Replace("{" + (object) index + "}", this.terms[index].ToString(translator));
      return str;
    }

    public string[] GetTableNames(ICriterionTranslator translator)
    {
      Dictionary<string, bool> dictionary = new Dictionary<string, bool>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      for (int index = 0; index < this.terms.Length; ++index)
      {
        foreach (string tableName in this.terms[index].GetTableNames(translator))
          dictionary[tableName] = true;
      }
      string[] array = new string[dictionary.Count];
      dictionary.Keys.CopyTo(array, 0);
      return array;
    }

    public string[] GetFieldNames(ICriterionTranslator translator)
    {
      Dictionary<string, bool> dictionary = new Dictionary<string, bool>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      for (int index = 0; index < this.terms.Length; ++index)
      {
        foreach (string fieldName in this.terms[index].GetFieldNames(translator))
          dictionary[fieldName] = true;
      }
      string[] array = new string[dictionary.Count];
      dictionary.Keys.CopyTo(array, 0);
      return array;
    }
  }
}
