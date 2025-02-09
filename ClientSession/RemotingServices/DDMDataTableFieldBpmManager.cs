// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.DDMDataTableFieldBpmManager
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class DDMDataTableFieldBpmManager : ManagerBase
  {
    private IBpmManager ddmDataTableFieldMgr;

    internal DDMDataTableFieldBpmManager(Sessions.Session session)
      : base(session, ClientSessionCacheID.BpmDDMDataTableFields)
    {
      this.ddmDataTableFieldMgr = this.session.SessionObjects.BpmManager;
    }

    internal static DDMDataTableFieldBpmManager Instance
    {
      get => Session.DefaultInstance.DDMDataTableFieldBpmManager;
    }

    internal static void RefreshInstance()
    {
      if (Session.DefaultInstance == null)
        return;
      Session.DefaultInstance.ResetDDMDataTableFieldBpmManager();
    }

    public int UpsertDDMDataTableFieldValue(DDMDataTableFieldValue ddmDataTableFieldValue)
    {
      return ddmDataTableFieldValue.Id >= 0 ? this.ddmDataTableFieldMgr.UpdateDDMDataTableField(ddmDataTableFieldValue) : this.ddmDataTableFieldMgr.CreateDDMDataTableField(ddmDataTableFieldValue);
    }

    public void DeleteDDMDataTableField(DDMDataTableFieldValue ddmDataTableField)
    {
      this.ddmDataTableFieldMgr.DeleteDDMDataTableField(ddmDataTableField);
    }

    public int[] AtomicDataTableChange(
      List<DDMDataTableFieldValue> batchCreationList,
      List<DDMDataTableFieldValue> batchUpdateList,
      List<DDMDataTableFieldValue> batchDeletionList)
    {
      return this.ddmDataTableFieldMgr.AtomicDataTableChange(batchCreationList, batchUpdateList, batchDeletionList);
    }
  }
}
