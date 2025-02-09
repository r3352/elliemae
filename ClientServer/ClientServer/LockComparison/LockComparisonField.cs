// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.LockComparison.LockComparisonField
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.LockComparison
{
  [Serializable]
  public class LockComparisonField
  {
    public string LoanFieldId { get; set; }

    public string LockRequestFieldId { get; set; }

    public override bool Equals(object obj)
    {
      if (obj == null || !(obj is LockComparisonField))
        return false;
      LockComparisonField lockComparisonField = (LockComparisonField) obj;
      return string.Compare(this.LoanFieldId, lockComparisonField.LoanFieldId, StringComparison.InvariantCultureIgnoreCase) == 0 && string.Compare(this.LockRequestFieldId, lockComparisonField.LockRequestFieldId, StringComparison.InvariantCultureIgnoreCase) == 0;
    }

    public override int GetHashCode() => (this.LoanFieldId, this.LockRequestFieldId).GetHashCode();
  }
}
