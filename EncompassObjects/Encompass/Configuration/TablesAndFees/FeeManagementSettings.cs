// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Configuration.TablesAndFees.FeeManagementSettings
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.BusinessEnums;
using EllieMae.Encompass.Client;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.Configuration.TablesAndFees
{
  /// <summary>Provides access to settings under Fee Management.</summary>
  public class FeeManagementSettings : SessionBoundObject
  {
    private FeeManagementRecords fees;
    private List<string> removeList = new List<string>();

    internal FeeManagementSettings(Session session)
      : base(session)
    {
      this.fees = new FeeManagementRecords(session);
    }

    /// <summary>Returns the collection of fees</summary>
    public FeeManagementRecords Fees => this.fees;
  }
}
