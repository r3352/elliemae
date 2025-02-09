// Decompiled with JetBrains decompiler
// Type: Elli.Data.UserTypes.NADecimalUserType
// Assembly: Elli.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5199CF45-D8E1-4436-8A49-245565D9CA6B
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Data.dll

using Elli.Domain.Mortgage;
using NHibernate;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Elli.Data.UserTypes
{
  public class NADecimalUserType : IUserType, IParameterizedType
  {
    private int precision = 2;
    private bool applyFieldFormat;
    private static readonly SqlType[] SQL_TYPES = new SqlType[1]
    {
      NHibernateUtil.String.SqlType
    };

    public SqlType[] SqlTypes => NADecimalUserType.SQL_TYPES;

    public Type ReturnedType => typeof (NA<Decimal>);

    public bool Equals(object x, object y)
    {
      if (x == y)
        return true;
      return x != null && y != null && x.Equals(y);
    }

    public object DeepCopy(object value) => value;

    public bool IsMutable => false;

    public object NullSafeGet(IDataReader dr, string[] names, object owner)
    {
      object strA = NHibernateUtil.String.NullSafeGet(dr, names[0]);
      if (strA == null)
        return (object) null;
      NA<Decimal> na1 = new NA<Decimal>();
      NA<Decimal> na2;
      try
      {
        if (string.Compare((string) strA, "na", true) == 0 || string.Compare((string) strA, "n/a", true) == 0)
        {
          na2 = (NA<Decimal>) "NA";
        }
        else
        {
          Decimal result;
          if (!Decimal.TryParse(strA.ToString(), out result))
            result = 0M;
          na2 = (NA<Decimal>) result;
        }
      }
      catch (Exception ex)
      {
        return (object) null;
      }
      return (object) na2;
    }

    public void NullSafeSet(IDbCommand cmd, object obj, int index)
    {
      if (obj == null)
      {
        ((IDataParameter) cmd.Parameters[index]).Value = (object) DBNull.Value;
      }
      else
      {
        NA<Decimal> na = (NA<Decimal>) obj;
        if (na.IsNA)
          ((IDataParameter) cmd.Parameters[index]).Value = (object) "NA";
        else if (na.HasValue)
        {
          if (this.applyFieldFormat)
            ((IDataParameter) cmd.Parameters[index]).Value = (object) na.Value.ToString(string.Format("N{0}", (object) this.precision));
          else
            ((IDataParameter) cmd.Parameters[index]).Value = (object) na.Value.ToString(string.Format("F{0}", (object) this.precision));
        }
        else
          ((IDataParameter) cmd.Parameters[index]).Value = (object) DBNull.Value;
      }
    }

    public object Assemble(object cached, object owner)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public object Disassemble(object value)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public int GetHashCode(object x)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public object Replace(object original, object target, object owner)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void SetParameterValues(IDictionary parameters)
    {
      if (parameters == null)
      {
        this.precision = 2;
        this.applyFieldFormat = false;
      }
      else
      {
        int? parameter1 = parameters[(object) "precision"] as int?;
        if (!parameter1.HasValue)
        {
          this.precision = 2;
        }
        else
        {
          try
          {
            this.precision = Convert.ToInt32((object) parameter1);
          }
          catch
          {
            this.precision = 2;
          }
        }
        bool? parameter2 = parameters[(object) "applyFieldFormat"] as bool?;
        if (!parameter2.HasValue)
        {
          this.applyFieldFormat = false;
        }
        else
        {
          try
          {
            this.applyFieldFormat = Convert.ToBoolean((object) parameter2);
          }
          catch
          {
            this.applyFieldFormat = false;
          }
        }
      }
    }

    public void SetParameterValues(IDictionary<string, string> parameters)
    {
      if (parameters == null)
      {
        this.precision = 2;
        this.applyFieldFormat = false;
      }
      else
      {
        if (!parameters.ContainsKey("precision"))
        {
          this.precision = 2;
        }
        else
        {
          string parameter = parameters["precision"];
          if (parameter == null)
          {
            this.precision = 2;
          }
          else
          {
            try
            {
              this.precision = Convert.ToInt32(parameter);
            }
            catch
            {
              this.precision = 2;
            }
          }
        }
        if (!parameters.ContainsKey("applyFieldFormat"))
        {
          this.applyFieldFormat = false;
        }
        else
        {
          string parameter = parameters["applyFieldFormat"];
          if (parameter == null)
          {
            this.applyFieldFormat = false;
          }
          else
          {
            try
            {
              this.applyFieldFormat = Convert.ToBoolean(parameter);
            }
            catch
            {
              this.applyFieldFormat = false;
            }
          }
        }
      }
    }
  }
}
