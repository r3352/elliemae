// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.DDMDataTableExportLog
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class DDMDataTableExportLog
  {
    public const string FLD_DT_EXP_ID = "dataTableExportLogID�";
    public const string FLD_DT_NAME = "dataTableName�";
    public const string FLD_DT_DESC = "dataTableDesc�";
    public const string FLD_DT_TYPE = "dataTableType�";
    public const string FLD_EXP_DT = "exportedTime�";
    public const string FLD_EXP_USERID = "exportedByUserID�";
    public const string FLD_EXP_USER_FN = "exportedByFullName�";

    public string DataTableExportLogID { get; set; }

    public string DataTableName { get; set; }

    public string DataTableDescription { get; set; }

    public string DataTableType { get; set; }

    public DateTime ExportedTime { get; set; }

    public string ExportedByUserID { get; set; }

    public string ExportedByFullName { get; set; }

    public DDMDataTableExportLog(
      string dataTableExportLogID,
      string dataTableName,
      string dataTableDescription,
      string dataTableType,
      DateTime exportedTime,
      string exportedByUserID,
      string exportedByFullName)
    {
      this.DataTableExportLogID = dataTableExportLogID;
      this.DataTableName = dataTableName;
      this.DataTableDescription = dataTableDescription;
      this.DataTableType = dataTableType;
      this.ExportedTime = exportedTime;
      this.ExportedByUserID = exportedByUserID;
      this.ExportedByFullName = exportedByFullName;
    }

    public DDMDataTableExportLog(DataRow row)
    {
      this.DataTableExportLogID = Convert.ToString(row["dataTableExportLogID"]);
      this.DataTableName = Convert.ToString(row["dataTableName"]);
      this.DataTableDescription = Convert.ToString(row["dataTableDesc"]);
      this.DataTableType = Convert.ToString(row["dataTableType"]);
      this.ExportedTime = Convert.ToDateTime(row["exportedTime"]);
      this.ExportedByUserID = Convert.ToString(row["exportedByUserID"]);
      this.ExportedByFullName = Convert.ToString(row["exportedByFullName"]);
    }
  }
}
