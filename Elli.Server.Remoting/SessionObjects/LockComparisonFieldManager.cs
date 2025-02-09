// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.LockComparisonFieldManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.LockComparison;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Server.ServerObjects;
using System;
using System.Collections.Generic;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class LockComparisonFieldManager : SessionBoundObject, ILockComparisonFieldManager
  {
    private const string className = "LockComparisonFieldManager";

    public LockComparisonFieldManager Initialize(ISession session)
    {
      this.InitializeInternal(session, nameof (LockComparisonFieldManager).ToLower());
      return this;
    }

    public virtual void UpdateLockComparionsFields(IList<LockComparisonField> lockComparisonFields)
    {
      this.onApiCalled(nameof (LockComparisonFieldManager), nameof (UpdateLockComparionsFields), Array.Empty<object>());
      try
      {
        LockComparisonFields.InsertLockComparisonFields(lockComparisonFields);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LockComparisonFieldManager), ex, this.Session.SessionInfo);
      }
    }
  }
}
