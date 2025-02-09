// Decompiled with JetBrains decompiler
// Type: Elli.SQE.QueryableField
// Assembly: Elli.SQE, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 49809150-F4C8-4F09-B50E-E491713B0B30
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.SQE.dll

using Elli.Common.Fields;
using System;

#nullable disable
namespace Elli.SQE
{
  public class QueryableField
  {
    public QueryableField(EncompassField field, Type type)
    {
      this.Field = field;
      this.Type = type;
    }

    public string ID => this.Field.ID;

    public EncompassField Field { get; private set; }

    public int? Index { get; private set; }

    public bool IsRepeatable { get; private set; }

    public Type Type { get; private set; }

    public QueryableField SetIndex(int value)
    {
      this.Index = new int?(value);
      return this;
    }

    public QueryableField SetIsRepeatabe(bool value)
    {
      this.IsRepeatable = value;
      return this;
    }

    public IDef Convert(IDefConverter<QueryableField> converter) => converter.Convert(this);
  }
}
