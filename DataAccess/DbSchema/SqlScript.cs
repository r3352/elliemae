// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.DbSchema.SqlScript
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.DataAccess.DbSchema
{
  [Serializable]
  public class SqlScript : List<string>
  {
    public SqlScript(TextReader sqlScript)
      : this(sqlScript.ReadToEnd())
    {
    }

    public SqlScript(string sqlScript)
    {
      this.AddRange((IEnumerable<string>) SqlScript.parseSqlText(sqlScript));
    }

    public void WriteTo(IScriptWriter writer)
    {
      foreach (string text in (List<string>) this)
        writer.WriteTransaction(text);
    }

    private static List<string> parseSqlText(string sqlText)
    {
      List<string> sqlText1 = new List<string>();
      using (StringReader stringReader = new StringReader(sqlText))
      {
        string str1 = "";
        string str2;
        while ((str2 = stringReader.ReadLine()) != null)
        {
          if (str2.Trim().ToLower() == "go")
          {
            if (str1 != "")
              sqlText1.Add(str1);
            str1 = "";
          }
          else if (str2.Trim() != "")
            str1 = str1 + str2 + Environment.NewLine;
        }
        if (str1 != "")
          sqlText1.Add(str1);
      }
      return sqlText1;
    }

    public override string ToString()
    {
      return string.Join(Environment.NewLine + "GO" + Environment.NewLine + Environment.NewLine, this.ToArray());
    }
  }
}
