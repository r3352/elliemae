// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Configuration.TablesAndFees.FeeManagementSettings
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessEnums;
using EllieMae.Encompass.Client;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.Configuration.TablesAndFees
{
  public class FeeManagementSettings : SessionBoundObject
  {
    private FeeManagementRecords fees;
    private List<string> removeList = new List<string>();

    internal FeeManagementSettings(Session session)
      : base(session)
    {
      this.fees = new FeeManagementRecords(session);
    }

    public FeeManagementRecords Fees => this.fees;
  }
}
