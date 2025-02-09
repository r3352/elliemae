// Decompiled with JetBrains decompiler
// Type: Elli.Data.UserTypes.XmlUserType
// Assembly: Elli.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5199CF45-D8E1-4436-8A49-245565D9CA6B
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Data.dll

using NHibernate;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using System;
using System.Data;
using System.Xml;

#nullable disable
namespace Elli.Data.UserTypes
{
  public class XmlUserType : IUserType
  {
    public SqlType[] SqlTypes { get; } = new SqlType[1]
    {
      NHibernateUtil.StringClob.SqlType
    };

    public Type ReturnedType { get; } = typeof (XmlElement);

    public bool IsMutable { get; }

    public object Assemble(object cached, object owner) => throw new NotSupportedException();

    public object DeepCopy(object value) => value;

    public object Disassemble(object value) => throw new NotSupportedException();

    public bool Equals(object x, object y) => x == y;

    public int GetHashCode(object x) => x == null ? 0 : x.GetHashCode();

    public object NullSafeGet(IDataReader rs, string[] names, object owner)
    {
      if (!(NHibernateUtil.String.NullSafeGet(rs, names[0]) is string xml) || string.IsNullOrWhiteSpace(xml))
        return (object) null;
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(xml);
      return (object) xmlDocument.DocumentElement;
    }

    public void NullSafeSet(IDbCommand cmd, object value, int index)
    {
      if (value is XmlElement xmlElement && xmlElement != null)
        ((IDataParameter) cmd.Parameters[index]).Value = (object) xmlElement.OuterXml;
      else
        ((IDataParameter) cmd.Parameters[index]).Value = (object) DBNull.Value;
    }

    public object Replace(object original, object target, object owner)
    {
      throw new NotSupportedException();
    }
  }
}
