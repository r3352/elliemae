// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.DDMDataTableFieldValue
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class DDMDataTableFieldValue
  {
    public const string FLD_DTF_ID = "dataTableFieldID�";
    public const string FLD_DTF_DT_ID = "dataTableID�";
    public const string FLD_DTF_ROWID = "rowId�";
    public const string FLD_DTF_FIELDID = "fieldId�";
    public const string FLD_DTF_VALUES = "values�";
    public const string FLD_DTF_CRITERIA = "criteria�";
    public const string FLD_DTF_ISOUTPUT = "isOutput�";
    public const string FLD_DTF_COLUMNID = "columnId�";

    public int Id { get; set; }

    public int DataTableId { get; set; }

    public int RowId { get; set; }

    public int ColumnId { get; set; }

    public string FieldId { get; set; }

    public string Values { get; set; }

    public int Criteria { get; set; }

    public bool IsOutput { get; set; }

    public DDMDataTableFieldValue()
    {
    }

    public DDMDataTableFieldValue(
      int id,
      int datatableId,
      int rowId,
      int columnId,
      string fieldId,
      string values,
      int criteria,
      bool isOutput)
    {
      this.Id = id;
      this.DataTableId = datatableId;
      this.RowId = rowId;
      this.ColumnId = columnId;
      this.FieldId = fieldId;
      this.Values = values;
      this.Criteria = criteria;
      this.IsOutput = isOutput;
    }

    public DDMDataTableFieldValue(DataRow dr)
    {
      this.Id = (int) dr["dataTableFieldID"];
      this.DataTableId = (int) dr["dataTableID"];
      this.RowId = (int) dr["rowId"];
      this.ColumnId = (int) dr["columnId"];
      this.FieldId = (string) dr["fieldId"];
      this.Criteria = (int) dr["criteria"];
      this.IsOutput = (bool) dr["isOutput"];
      this.Values = (string) dr["values"];
    }
  }
}
