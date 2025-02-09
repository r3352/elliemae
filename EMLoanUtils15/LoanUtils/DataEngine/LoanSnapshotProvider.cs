// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.DataEngine.LoanSnapshotProvider
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using Elli.ElliEnum;
using Elli.Interface;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.DataEngine
{
  public class LoanSnapshotProvider : ILoanSnapshotProvider
  {
    private const string className = "LoanSnapshotProvider�";
    private static readonly string sw = Tracing.SwDataEngine;
    private LoanDataMgr loanDataMgr;

    public LoanSnapshotProvider(LoanDataMgr loanDataMgr) => this.loanDataMgr = loanDataMgr;

    public Dictionary<string, string> GetLoanSnapshot(
      LogSnapshotType type,
      Guid snapshotGuid,
      bool ucdExists)
    {
      return this.loanDataMgr.GetLoanSnapshot(type, snapshotGuid, ucdExists);
    }

    public Dictionary<string, Dictionary<string, string>> GetLoanSnapshots(
      LogSnapshotType type,
      Dictionary<string, bool> snapshotGuids)
    {
      return this.loanDataMgr.GetLoanSnapshots(type, snapshotGuids);
    }

    public void SaveSnapshot(LogSnapshotType type, Guid snapshotGuid, string snapshot)
    {
      bool flag = true;
      byte num = 0;
      while (flag)
      {
        if (num >= (byte) 3)
          break;
        try
        {
          string snapshotFileName = SnapshotObject.GetLoanSnapshotFileName(type, snapshotGuid.ToString());
          SnapshotObject data = new SnapshotObject()
          {
            ParentId = snapshotGuid,
            Type = type,
            Data = snapshot
          };
          if (this.loanDataMgr.GetSupportingSnapshotData(type, snapshotGuid, snapshotFileName) != null)
            throw new Exception("System does not allow to append to an existing snapshot - " + snapshotFileName + ".");
          this.loanDataMgr.SaveSupportingSnapshotData(type, snapshotGuid, snapshotFileName, data);
          flag = false;
        }
        catch (Exception ex)
        {
          if (type != LogSnapshotType.DisclosureTracking && type != LogSnapshotType.DisclosureTrackingUCD)
            throw ex;
          if (num == (byte) 2)
            RemoteLogger.Write(TraceLevel.Warning, string.Format("Error creating disclosure tracking snapshot for the loan guid {0}, clientID {1}, Snapshotguid {2}, UserID {3} and Exception{4}", (object) this.loanDataMgr.LoanData.GUID, (object) this.loanDataMgr.SessionObjects.CompanyInfo.ClientID, (object) snapshotGuid, (object) this.loanDataMgr.SessionObjects.UserID, (object) ex.Message));
          ++num;
        }
      }
    }

    public void SaveSnapshot(
      LogSnapshotType type,
      Guid snapshotGuid,
      string snapshot,
      bool isAlwaysSaveNew)
    {
      try
      {
        SnapshotObject data = new SnapshotObject()
        {
          ParentId = snapshotGuid,
          Type = type,
          Data = snapshot
        };
        if (isAlwaysSaveNew)
        {
          string snapshotFileName = SnapshotObject.GetLoanSnapshotFileName(type, snapshotGuid.ToString());
          this.loanDataMgr.SaveSupportingSnapshotData(type, snapshotGuid, snapshotFileName, data);
        }
        else
          this.SaveSnapshot(type, snapshotGuid, snapshot);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
  }
}
