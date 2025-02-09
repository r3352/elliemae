// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.MsMergeTable
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.DataAccess
{
  public static class MsMergeTable
  {
    private static readonly List<DbColumnType> s_assumeChanged = new List<DbColumnType>()
    {
      DbColumnType.Text,
      DbColumnType.Binary
    };

    public static void AppendMergeTable(DbQueryBuilder sql, MergeTable table, bool useDelete)
    {
      List<MergeColumn> columns = table.Columns;
      IEnumerable<MergeColumn> source1 = columns.Where<MergeColumn>((Func<MergeColumn, bool>) (c => c.Intent == MergeIntent.PrimaryKey));
      IEnumerable<MergeColumn> source2 = columns.Where<MergeColumn>((Func<MergeColumn, bool>) (c => c.Intent == MergeIntent.UpdateOnly || c.Intent == MergeIntent.Upsert));
      IEnumerable<MergeColumn> source3 = columns.Where<MergeColumn>((Func<MergeColumn, bool>) (c => c.Intent == MergeIntent.InsertOnly || c.Intent == MergeIntent.Upsert));
      bool flag1 = source2.Any<MergeColumn>();
      bool flag2 = source3.Any<MergeColumn>();
      string str1 = MsMergeTable.SqlCsv(columns.Select<MergeColumn, string>((Func<MergeColumn, string>) (c => MsMergeTable.ColumnName(c) + " " + MsMergeTable.ColumnTypeAndLength(c) + " NULL")));
      string str2 = MsMergeTable.SqlCsv(columns.Select<MergeColumn, string>((Func<MergeColumn, string>) (c => MsMergeTable.ColumnName(c))));
      string str3 = MsMergeTable.SqlCsv(source1.Select<MergeColumn, string>(new Func<MergeColumn, string>(setOrEquals)));
      string str4 = flag1 ? string.Join(" OR ", source2.Select<MergeColumn, string>(new Func<MergeColumn, string>(inequality))) : "1=1";
      string str5 = MsMergeTable.SqlCsv(source2.Select<MergeColumn, string>(new Func<MergeColumn, string>(setOrEquals)));
      string str6 = MsMergeTable.SqlCsv(columns.Select<MergeColumn, string>((Func<MergeColumn, string>) (c => "s." + MsMergeTable.ColumnName(c))));
      string str7 = string.Join(",\n", new List<string>()
      {
        "Insert",
        "Update",
        "Delete"
      }.Select<string, string>((Func<string, string>) (a => "    ISNULL(SUM(CASE WHEN [action]='" + a + "' THEN 1 ELSE 0 END),0) AS " + a + "Count")));
      string name = table.Name;
      string str8 = "#temp_" + name;
      string str9 = "@mergeCounts_" + name;
      sql.AppendLine("CREATE TABLE " + str8 + " (" + str1 + ")");
      if (table.Rows.Count > 0)
      {
        sql.AppendLine("INSERT INTO  " + str8 + " (" + str2 + ") ");
        sql.AppendLine("SELECT " + str2 + " FROM ( VALUES");
        MsMergeTable.AppendJoinedLines(sql, ",", table.Rows.Select<List<object>, string>((Func<List<object>, string>) (row => "    (" + string.Join(", ", row.Select<object, string>((Func<object, string>) (c => SQL.Encode(c)))) + ")")));
        sql.AppendLine(") AS r (" + str2 + ")");
      }
      sql.AppendLine("DECLARE " + str9 + " TABLE (action VARCHAR(20))");
      sql.AppendLine("MERGE " + name + " AS t USING " + str8 + " AS s ON (" + str3 + ")");
      if (flag1)
        sql.AppendLine("WHEN MATCHED AND (" + str4 + ") THEN UPDATE SET " + str5);
      if (flag2)
        sql.AppendLine("WHEN NOT MATCHED BY TARGET THEN INSERT (" + str2 + ") VALUES (" + str6 + ")");
      if (useDelete)
        sql.AppendLine("WHEN NOT MATCHED BY SOURCE THEN DELETE");
      sql.AppendLine("OUTPUT $action INTO " + str9 + ";");
      sql.AppendLine("SELECT\n" + str7 + "\nFROM " + str9);

      static string compare(MergeColumn c, string op)
      {
        return "t." + MsMergeTable.ColumnName(c) + " " + op + " s." + MsMergeTable.ColumnName(c);
      }

      string setOrEquals(MergeColumn c) => compare(c, "=");

      string inequality(MergeColumn c)
      {
        return !MsMergeTable.s_assumeChanged.Contains(c.DataType) ? compare(c, "<>") : "1=1";
      }
    }

    private static string SqlCsv(IEnumerable<string> toJoin) => string.Join(", ", toJoin);

    private static string ColumnName(MergeColumn c) => "[" + c.Name + "]";

    private static string ColumnTypeAndLength(MergeColumn c)
    {
      return "[" + c.DataType.ToString().ToUpper() + "] " + (c.MaxLength > 0 ? string.Format("({0})", (object) c.MaxLength) : (c.MaxLength == -1 ? "(MAX)" : ""));
    }

    private static void AppendJoinedLines(
      DbQueryBuilder sql,
      string separator,
      IEnumerable<string> lines)
    {
      IEnumerator<string> enumerator = lines.GetEnumerator();
      string text = (string) null;
      while (enumerator.MoveNext())
      {
        if (text != null)
          sql.AppendLine(text + separator);
        text = enumerator.Current;
      }
      if (text == null)
        return;
      sql.AppendLine(text);
    }
  }
}
