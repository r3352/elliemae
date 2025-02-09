// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.LoanCenter.LoanDisplayInfo
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using System;

#nullable disable
namespace EllieMae.EMLite.eFolder.LoanCenter
{
  public class LoanDisplayInfo
  {
    public string LoanNumber { get; set; }

    public Guid LoanGuid { get; set; }

    public string BorrowerName { get; set; }

    public Decimal LoanAmount { get; set; }
  }
}
