// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.DDMDataTableBpmManager
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class DDMDataTableBpmManager : ManagerBase
  {
    private IBpmManager ddmDataTableMgr;

    internal DDMDataTableBpmManager(Sessions.Session session)
      : base(session, ClientSessionCacheID.BpmDDMDataTables)
    {
      this.ddmDataTableMgr = this.session.SessionObjects.BpmManager;
    }

    internal static DDMDataTableBpmManager Instance
    {
      get => Session.DefaultInstance.DDMDataTableBpmManager;
    }

    internal static void RefreshInstance()
    {
      if (Session.DefaultInstance == null)
        return;
      Session.DefaultInstance.ResetDDMDataTableBpmManager();
    }

    public int UpsertDDMDataTable(
      DDMDataTable ddmDataTable,
      bool importMode = false,
      bool forceToPrimaryDb = false)
    {
      return ddmDataTable.Id >= 0 ? this.ddmDataTableMgr.UpdateDDMDataTable(ddmDataTable, forceToPrimaryDb) : this.ddmDataTableMgr.CreateDDMDataTable(ddmDataTable, importMode, forceToPrimaryDb);
    }

    public DDMDataTable[] GetAllDDMDataTable(bool forceToPrimaryDb = false)
    {
      return this.ddmDataTableMgr.GetAllDDMDataTables(forceToPrimaryDb);
    }

    public DDMDataTable[] GetAllDDMDataTablesWithFieldValues(bool forceToPrimaryDb = false)
    {
      return this.ddmDataTableMgr.GetAllDDMDataTablesWithFieldValues(forceToPrimaryDb);
    }

    public void DeleteDDMDataTable(DDMDataTable ddmDataTable)
    {
      this.ddmDataTableMgr.DeleteDDMDataTable(ddmDataTable);
    }

    public bool DDMDataTableExists(string tableName, bool forceToPrimaryDb = false)
    {
      return this.ddmDataTableMgr.DDMDataTableExists(tableName, forceToPrimaryDb);
    }

    public DDMDataTable GetDDMDataTableAndFieldValues(int dataTableId, bool forceToPrimaryDb = false)
    {
      return this.ddmDataTableMgr.GetDDMDataTableAndFieldValues(dataTableId, forceToPrimaryDb);
    }

    public DDMDataTable GetDDMDataTableAndFieldValuesByName(
      string dataTableName,
      bool forceToPrimaryDb = false)
    {
      return this.ddmDataTableMgr.GetDDMDataTableAndFieldValuesByName(dataTableName, forceToPrimaryDb);
    }

    public bool IsTableUsedByFeeOrFieldRules(DDMDataTable dt, bool forceToPrimaryDb = false)
    {
      return this.ddmDataTableMgr.IsTableUsedByFeeOrFieldRules(dt, forceToPrimaryDb);
    }

    public void ResetDataTableFeeRuleFieldRuleValue(DDMDataTable dt)
    {
      this.ddmDataTableMgr.ResetDataTableFeeRuleFieldRuleValue(dt);
    }

    public DDMDataTableExportLog GetDataTableExportLog(
      string dataTableExportLogID,
      bool forceToPrimaryDb = false)
    {
      return this.ddmDataTableMgr.GetDataTableExportLog(dataTableExportLogID, forceToPrimaryDb);
    }

    public void SaveDataTableExportLog(DDMDataTableExportLog dataTableExportLog)
    {
      this.ddmDataTableMgr.SaveDataTableExportLog(dataTableExportLog);
    }

    public List<DDMDataTableReference> GetDDMDataTableReferences(string dataTableName)
    {
      return this.ddmDataTableMgr.GetDDMDataTableReferences(dataTableName);
    }
  }
}
