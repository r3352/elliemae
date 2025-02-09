// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.Postgres.PgQueryHelpers
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.DataAccess.Postgres
{
  public static class PgQueryHelpers
  {
    public const string PgVariablePrefix = "v_";
    private static IList<DbType> s_MaxLengthSupportedTypes = (IList<DbType>) new List<DbType>()
    {
      DbType.AnsiString,
      DbType.AnsiStringFixedLength,
      DbType.Binary,
      DbType.String,
      DbType.StringFixedLength,
      DbType.VarNumeric
    };
    private static Dictionary<DbType, string> s_pgDbTypeName = new Dictionary<DbType, string>()
    {
      {
        DbType.AnsiString,
        "varchar"
      },
      {
        DbType.AnsiStringFixedLength,
        "char"
      },
      {
        DbType.Binary,
        "binary"
      },
      {
        DbType.Boolean,
        "smallint"
      },
      {
        DbType.Byte,
        "tinyint"
      },
      {
        DbType.Int16,
        "smallint"
      },
      {
        DbType.Int32,
        "int"
      },
      {
        DbType.Int64,
        "bigint"
      },
      {
        DbType.Currency,
        "money"
      },
      {
        DbType.Date,
        "date"
      },
      {
        DbType.DateTime,
        "timestamp"
      },
      {
        DbType.DateTime2,
        "timestamp"
      },
      {
        DbType.DateTimeOffset,
        "interval"
      },
      {
        DbType.Decimal,
        "decimal"
      },
      {
        DbType.Double,
        "double"
      },
      {
        DbType.Guid,
        "uniqueidentifier"
      },
      {
        DbType.Object,
        "varbinary"
      },
      {
        DbType.SByte,
        "tinyint"
      },
      {
        DbType.Single,
        "real"
      },
      {
        DbType.String,
        "varchar"
      },
      {
        DbType.StringFixedLength,
        "char"
      },
      {
        DbType.Time,
        "time"
      },
      {
        DbType.UInt16,
        "smallint"
      },
      {
        DbType.UInt32,
        "int"
      },
      {
        DbType.UInt64,
        "bigint"
      },
      {
        DbType.Xml,
        "xml"
      }
    };
    private const string _newLine = "\r\n";

    public static string VarName(string name) => !name.StartsWith("v_") ? "v_" + name : name;

    public static string RootName(string name)
    {
      return !name.StartsWith("v_") ? name : name.Substring("v_".Length);
    }

    public static string DbTypeName(DbType dbType, int maxLength = 0)
    {
      string str;
      if (!PgQueryHelpers.s_pgDbTypeName.TryGetValue(dbType, out str))
        throw new ArgumentException("DbType name not mapped for the configured database type.");
      if (maxLength != 0 && PgQueryHelpers.SupportsMaxLength(dbType))
        str = str + "(" + (maxLength == -1 ? "MAX" : maxLength.ToString()) + ")";
      return str;
    }

    private static bool SupportsMaxLength(DbType dbType)
    {
      return PgQueryHelpers.s_MaxLengthSupportedTypes.Contains(dbType);
    }

    public static string QuoteNames(string unquoted)
    {
      StringBuilder stringBuilder = new StringBuilder(unquoted.Length);
      char[] charArray = unquoted.ToCharArray();
      int length = charArray.Length;
      int num = charArray.Length - 2;
      int index1 = 0;
      int startIndex = 0;
      char c = ' ';
      while (index1 < length)
      {
        while (index1 < length && (c = charArray[index1]) != '\'' && c != '[')
          ++index1;
        if (index1 < length)
        {
          if (c == '\'')
          {
            int index2;
            for (index2 = index1 + 1; index2 < length && (c = charArray[index2]) != '\''; ++index2)
            {
              if (index2 < num && charArray[index2 + 1] == '\'' && charArray[index2 + 2] == '\'')
                index2 += 2;
            }
            index1 = index2 + 1;
          }
          else
          {
            for (charArray[index1++] = '"'; index1 < length && (c = charArray[index1]) != ']'; ++index1)
            {
              if (char.IsUpper(c))
                charArray[index1] = char.ToLower(c);
            }
            if (index1 < length)
              charArray[index1++] = '"';
          }
        }
      }
      if (startIndex < length)
        stringBuilder.Append(charArray, startIndex, length - startIndex);
      return stringBuilder.ToString();
    }

    public static void Upsert(
      PgDbQueryBuilder idbqb,
      DbConstraint constraint,
      DbTableInfo tableInfo,
      DbValue pkColumnValue,
      DbValueList nonPkColumnValues)
    {
      PgQueryHelpers.Upsert(idbqb, constraint, tableInfo.Name, pkColumnValue, nonPkColumnValues);
    }

    public static void Upsert(
      PgDbQueryBuilder idbqb,
      DbConstraint constraint,
      string tableName,
      DbValue pkColumnValue,
      DbValueList nonPkColumnValues)
    {
      PgDbQueryBuilder idbqb1 = idbqb;
      int constraint1 = (int) constraint;
      string tableName1 = tableName;
      List<DbValue> pkColumnValues = new List<DbValue>();
      pkColumnValues.Add(pkColumnValue);
      List<DbValue> list = nonPkColumnValues.Cast<DbValue>().ToList<DbValue>();
      PgQueryHelpers.Upsert(idbqb1, (DbConstraint) constraint1, tableName1, (IEnumerable<DbValue>) pkColumnValues, (IEnumerable<DbValue>) list);
    }

    public static void Upsert(
      PgDbQueryBuilder idbqb,
      DbConstraint constraint,
      string tableName,
      DbValue pkColumnValue,
      DbValue nonPkColumnValues)
    {
      PgQueryHelpers.Upsert(idbqb, constraint, tableName, (IEnumerable<DbValue>) new List<DbValue>()
      {
        pkColumnValue
      }, (IEnumerable<DbValue>) new List<DbValue>()
      {
        nonPkColumnValues
      });
    }

    public static void Upsert(
      PgDbQueryBuilder idbqb,
      DbConstraint constraint,
      string tableName,
      DbValueList pkColumnValues,
      DbValue nonPkColumnValue)
    {
      PgQueryHelpers.Upsert(idbqb, constraint, tableName, (IEnumerable<DbValue>) pkColumnValues.Cast<DbValue>().ToList<DbValue>(), (IEnumerable<DbValue>) new List<DbValue>()
      {
        nonPkColumnValue
      });
    }

    public static void Upsert(
      PgDbQueryBuilder idbqb,
      DbConstraint constraint,
      string tableName,
      IEnumerable<DbValue> pkColumnValues,
      IEnumerable<DbValue> nonPkColumnValues)
    {
      PgQueryHelpers.Upsert(idbqb, constraint, tableName, pkColumnValues, nonPkColumnValues, nonPkColumnValues);
    }

    public static void Upsert(
      PgDbQueryBuilder idbqb,
      DbConstraint constraint,
      string tableName,
      IEnumerable<DbValue> pkColumnValues,
      IEnumerable<DbValue> nonPkColumnInsertValues,
      IEnumerable<DbValue> nonPkColumnUpdateValues)
    {
      string str1 = string.Join(", ", pkColumnValues.Select<DbValue, string>((System.Func<DbValue, string>) (dbv => dbv.ColumnName)));
      IEnumerable<DbValue> source = pkColumnValues.Union<DbValue>(nonPkColumnInsertValues);
      string str2 = string.Join(", ", source.Select<DbValue, string>((System.Func<DbValue, string>) (dbv => dbv.ColumnName)));
      string str3 = string.Join(", ", source.Select<DbValue, string>((System.Func<DbValue, string>) (dbv => dbv.Encode())));
      string str4 = nonPkColumnUpdateValues == null ? (string) null : string.Join(", ", nonPkColumnUpdateValues.Select<DbValue, string>((System.Func<DbValue, string>) (dbv => dbv.ColumnName + " = " + dbv.Encode())));
      if (constraint == DbConstraint.Use)
      {
        idbqb.AppendLine("INSERT INTO " + tableName + "\r\n    (" + str2 + ") VALUES\r\n    (" + str3 + ")\r\nON CONFLICT (" + str1 + ") DO " + (str4 == null ? "NOTHING" : "UPDATE SET\r\n    " + str4 + ";"));
      }
      else
      {
        string str5 = string.Join(" AND ", pkColumnValues.Select<DbValue, string>((System.Func<DbValue, string>) (dbv => dbv.ColumnName + " = " + dbv.Encode())));
        PgDbQueryBuilder pgDbQueryBuilder = idbqb;
        string[] strArray = new string[12];
        strArray[0] = "WITH upsert (";
        strArray[1] = str2;
        strArray[2] = ") AS\r\n    (VALUES(";
        strArray[3] = str3;
        strArray[4] = ")),\r\n    updated AS\r\n";
        string str6;
        if (str4 != null)
          str6 = "    (UPDATE " + tableName + " SET " + str4 + " WHERE " + str5 + " RETURNING 1)";
        else
          str6 = "    (SELECT 1 FROM " + tableName + " WHERE " + str5 + ")";
        strArray[5] = str6;
        strArray[6] = "\r\nINSERT INTO ";
        strArray[7] = tableName;
        strArray[8] = " (";
        strArray[9] = str2;
        strArray[10] = ")\r\n    SELECT ";
        strArray[11] = str2;
        string text = string.Concat(strArray) + "\r\n    FROM upsert\r\n    WHERE NOT EXISTS (SELECT * FROM updated);";
        pgDbQueryBuilder.AppendLine(text);
      }
    }

    public static void CreateTempTable(
      PgDbQueryBuilder idbqb,
      string tableName,
      IEnumerable<DbVariable> variables,
      System.Func<DbVariable, string> valueSelector)
    {
      idbqb.AppendLine("DROP TABLE IF EXISTS " + tableName + ";\r\nCREATE TEMPORARY TABLE " + tableName + "\r\n    (" + string.Join(", ", variables.Select<DbVariable, string>((System.Func<DbVariable, string>) (v => PgQueryHelpers.RootName(v.RootName) + " " + PgQueryHelpers.DbTypeName(v.DbType, v.MaxLength)))) + ");\r\n");
      if (valueSelector == null)
        return;
      idbqb.AppendLine("INSERT INTO " + tableName + "\r\n    SELECT " + string.Join(", ", variables.Select<DbVariable, string>(valueSelector)) + ";");
    }
  }
}
