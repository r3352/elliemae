// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.PiggybackSynchronization
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using System;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  internal class PiggybackSynchronization
  {
    private LoanData loan;

    public PiggybackSynchronization(LoanData loan) => this.loan = loan;

    public void SyncPiggyBackField(string id, FieldSource fieldSource, string val)
    {
      this.SyncPiggyBackField(id, fieldSource, val, (BorrowerPair) null);
    }

    public void SyncPiggyBackField(
      string id,
      FieldSource fieldSource,
      string val,
      BorrowerPair pair)
    {
      if (this.loan.LinkedData == null || Session.LoanDataMgr == null || Session.LoanDataMgr.SystemConfiguration == null || Session.LoanDataMgr.SystemConfiguration.PiggybackSyncFields == null || !Session.LoanDataMgr.SystemConfiguration.PiggybackSyncFields.IsSycnField(id))
        return;
      string empty = string.Empty;
      if (fieldSource == FieldSource.CurrentLoan)
      {
        if (this.loan != null && this.loan.LinkedData != null)
        {
          if (this.loan.LinkedData.IsLocked(id))
          {
            if (!this.loan.IsLocked(id))
              this.loan.AddLock(id);
          }
          else if (this.loan.IsLocked(id))
            this.loan.RemoveLock(id);
        }
        if ((pair == null ? this.loan.GetField(id) : this.loan.GetSimpleField(id, pair)) != val)
        {
          if (pair != null)
            this.loan.SetCurrentField(id, val, pair);
          else
            this.loan.SetCurrentField(id, val);
        }
        if (this.loan == null || this.loan.LinkedData == null)
          return;
        this.loan.SyncPiggyBackFiles((string[]) null, true, false, id, val, fieldSource != FieldSource.CurrentLoan);
      }
      else
      {
        if (this.loan == null || this.loan.LinkedData == null)
          throw new InvalidOperationException("Linked loan field cannot be set when no linked loan is available");
        if (this.loan.IsLocked(id))
        {
          if (!this.loan.LinkedData.IsLocked(id))
            this.loan.LinkedData.AddLock(id);
        }
        else if (this.loan.LinkedData.IsLocked(id))
          this.loan.LinkedData.RemoveLock(id);
        if ((pair == null ? this.loan.LinkedData.GetField(id) : this.loan.LinkedData.GetSimpleField(id, pair)) != val)
        {
          if (pair != null)
            this.loan.LinkedData.SetCurrentField(id, val, pair);
          else
            this.loan.LinkedData.SetCurrentField(id, val);
        }
        if (this.loan == null || this.loan.LinkedData == null)
          return;
        this.loan.SyncPiggyBackFiles((string[]) null, true, false, id, val, fieldSource != FieldSource.CurrentLoan);
      }
    }
  }
}
