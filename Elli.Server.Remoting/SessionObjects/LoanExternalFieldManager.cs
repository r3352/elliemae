// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.LoanExternalFieldManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Interfaces;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Server.ServerObjects;
using System;
using System.Collections.Generic;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class LoanExternalFieldManager : SessionBoundObject, ILoanExternalFieldManager
  {
    private const string className = "LoanExternalFieldManager";

    public LoanExternalFieldManager Initialize(ISession session)
    {
      this.InitializeInternal(session, nameof (LoanExternalFieldManager).ToLower());
      return this;
    }

    public virtual List<LoanExternalFieldConfig> GetAllLoanExternalFieldsDefination()
    {
      this.onApiCalled(nameof (LoanExternalFieldManager), nameof (GetAllLoanExternalFieldsDefination), new object[1]
      {
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.Pipeline)
      });
      try
      {
        return LoanExternalFieldsAccessor.GetAllLoanExternalFieldsDefination();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanExternalFieldManager), ex, this.Session.SessionInfo);
        return (List<LoanExternalFieldConfig>) null;
      }
    }

    public virtual string GetFolderID(string Guid)
    {
      this.onApiCalled(nameof (LoanExternalFieldManager), nameof (GetFolderID), new object[1]
      {
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.Pipeline)
      });
      try
      {
        return LoanExternalFieldsAccessor.GetFolderID(Guid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanExternalFieldManager), ex, this.Session.SessionInfo);
        return (string) null;
      }
    }
  }
}
