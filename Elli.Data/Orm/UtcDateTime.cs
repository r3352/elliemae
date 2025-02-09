// Decompiled with JetBrains decompiler
// Type: Elli.Data.Orm.UtcDateTime
// Assembly: Elli.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5199CF45-D8E1-4436-8A49-245565D9CA6B
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Data.dll

using NHibernate;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using System;
using System.Data;

#nullable disable
namespace Elli.Data.Orm
{
  public class UtcDateTime : IUserType
  {
    public object Assemble(object cached, object owner) => this.DeepCopy(cached);

    public object DeepCopy(object value)
    {
      return value != null ? (object) new DateTime(((DateTime) value).Ticks, DateTimeKind.Utc) : (object) null;
    }

    public object Disassemble(object value) => this.DeepCopy(value);

    public bool Equals(object x, object y)
    {
      if (x == y)
        return true;
      return x != null && y != null && x.Equals(y);
    }

    public int GetHashCode(object x)
    {
      return x != null ? x.GetHashCode() : throw new ArgumentNullException(nameof (x));
    }

    public bool IsMutable => false;

    public object NullSafeGet(IDataReader rs, string[] names, object owner)
    {
      object obj = NHibernateUtil.DateTime.NullSafeGet(rs, names[0]);
      return obj != null ? (object) new DateTime(((DateTime) obj).Ticks, DateTimeKind.Utc) : (object) null;
    }

    public void NullSafeSet(IDbCommand cmd, object value, int index)
    {
      if (value == null)
      {
        NHibernateUtil.DateTime.NullSafeSet(cmd, (object) null, index);
      }
      else
      {
        if (((DateTime) value).Kind != DateTimeKind.Utc)
          throw new InvalidTimeZoneException("The domain model date must be a Utc date.");
        NHibernateUtil.DateTime.NullSafeSet(cmd, value, index);
      }
    }

    public object Replace(object original, object target, object owner) => original;

    public Type ReturnedType => typeof (DateTime);

    public SqlType[] SqlTypes
    {
      get => new SqlType[1]{ new SqlType(DbType.DateTime) };
    }
  }
}
