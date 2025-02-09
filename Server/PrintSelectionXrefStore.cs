// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.PrintSelectionXrefStore
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;

#nullable disable
namespace EllieMae.EMLite.Server
{
  internal class PrintSelectionXrefStore
  {
    private const string CUSTOMLETTERS = "CustomLetters�";
    private const string FORMGROUP = "FormGroup�";

    public static void DeleteCustomLetterXRefs(FileSystemEntry letterEntry)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete from PAS_PreselectFormsXRef where (XRef like '" + SQL.Escape(letterEntry.ToString()) + "') And (FormType = 'CustomLetters')");
      dbQueryBuilder.AppendLine("delete from BBR_PrintFormRuleXRef where (XRef like '" + SQL.Escape(letterEntry.ToString()) + "') And (FormType = 'CustomLetters')");
      dbQueryBuilder.AppendLine("delete from ConditionalLetterXRef where (XRef like '" + SQL.Escape(letterEntry.ToString()) + "')");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void MoveCustomLetterXRefs(FileSystemEntry source, FileSystemEntry target)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      if (source.Type == FileSystemEntry.Types.File)
      {
        dbQueryBuilder.AppendLine("update PAS_PreselectFormsXRef set XRef = " + SQL.Encode((object) target.ToString()) + " where (XRef like '" + SQL.Escape(source.ToString()) + "') And (FormType = 'CustomLetters')");
        dbQueryBuilder.AppendLine("update BBR_PrintFormRuleXRef set XRef = " + SQL.Encode((object) target.ToString()) + " where (XRef like '" + SQL.Escape(source.ToString()) + "') And (FormType = 'CustomLetters')");
        dbQueryBuilder.AppendLine("update ConditionalLetterXRef set XRef = " + SQL.Encode((object) target.ToString()) + " where (XRef like '" + SQL.Escape(source.ToString()) + "')");
      }
      else
      {
        dbQueryBuilder.Declare("@sourceLen", "int");
        dbQueryBuilder.SelectVar("@sourceLen", (object) source.ToString().Length);
        dbQueryBuilder.AppendLine("update PAS_PreselectFormsXRef set XRef = (" + SQL.Encode((object) target.ToString()) + " + substring(XRef, @sourceLen + 1, Len(XRef) - @sourceLen))  where (XRef like '" + SQL.Escape(source.ToString()) + "%') And (FormType = 'CustomLetters')");
        dbQueryBuilder.AppendLine("update BBR_PrintFormRuleXRef set XRef = (" + SQL.Encode((object) target.ToString()) + " + substring(XRef, @sourceLen + 1, Len(XRef) - @sourceLen))  where (XRef like '" + SQL.Escape(source.ToString()) + "%') And (FormType = 'CustomLetters')");
        dbQueryBuilder.AppendLine("update ConditionalLetterXRef set XRef = (" + SQL.Encode((object) target.ToString()) + " + substring(XRef, @sourceLen + 1, Len(XRef) - @sourceLen))  where (XRef like '" + SQL.Escape(source.ToString()) + "%')");
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void DeleteFormGroupXRefs(FileSystemEntry letterEntry)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete from PAS_PreselectFormsXRef where (XRef like '" + SQL.Escape(letterEntry.ToString()) + "') And (FormType = 'FormGroup')");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void MoveFormGroupXRefs(FileSystemEntry source, FileSystemEntry target)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      if (source.Type == FileSystemEntry.Types.File)
      {
        dbQueryBuilder.AppendLine("update PAS_PreselectFormsXRef set XRef = " + SQL.Encode((object) target.ToString()) + " where (XRef like '" + SQL.Escape(source.ToString()) + "') And (FormType = 'FormGroup')");
      }
      else
      {
        dbQueryBuilder.Declare("@sourceLen", "int");
        dbQueryBuilder.SelectVar("@sourceLen", (object) source.ToString().Length);
        dbQueryBuilder.AppendLine("update PAS_PreselectFormsXRef set XRef = (" + SQL.Encode((object) target.ToString()) + " + substring(XRef, @sourceLen + 1, Len(XRef) - @sourceLen))  where (XRef like '" + SQL.Escape(source.ToString()) + "%') And (FormType = 'FormGroup')");
      }
      dbQueryBuilder.ExecuteNonQuery();
    }
  }
}
