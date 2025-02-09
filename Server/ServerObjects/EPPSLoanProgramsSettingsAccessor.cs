// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.EPPSLoanProgramsSettingsAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataAccess;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects
{
  public class EPPSLoanProgramsSettingsAccessor
  {
    private const string className = "EPPSLoanProgramsSettingsAccessor�";

    public static void SaveEPPSLoanProgramsSettings(List<EPPSLoanProgram> programs)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("EPPSLoanPrograms");
      dbQueryBuilder.DeleteFrom(table);
      foreach (EPPSLoanProgram program in programs)
        dbQueryBuilder.InsertInto(table, new DbValueList()
        {
          {
            "ProgramId",
            (object) program.ProgramID
          },
          {
            "ProgramName",
            (object) program.ProgramName
          }
        }, true, false);
      dbQueryBuilder.Execute();
    }

    public static List<EPPSLoanProgram> GetEPPSLoanProgramsSettings()
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select * from EPPSLoanPrograms");
      return EPPSLoanProgramsSettingsAccessor.dataSetToEPPSPrograms(dbQueryBuilder.ExecuteSetQuery());
    }

    private static List<EPPSLoanProgram> dataSetToEPPSPrograms(DataSet ds)
    {
      List<EPPSLoanProgram> eppsPrograms = new List<EPPSLoanProgram>();
      foreach (DataRow row in (InternalDataCollectionBase) ds.Tables[0].Rows)
        eppsPrograms.Add(new EPPSLoanProgram(SQL.DecodeString(row["ProgramId"]), SQL.DecodeString(row["ProgramName"])));
      return eppsPrograms;
    }
  }
}
