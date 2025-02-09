// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.BamObjects.DisclosureTrackingRecordBase
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using EllieMae.EMLite.ePass.BamEnums;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ePass.BamObjects
{
  public class DisclosureTrackingRecordBase
  {
    public DateTime DisclosedDate { get; set; }

    public virtual bool IsLocked { get; set; }

    public DateTime DateAdded { get; set; }

    public string DisclosedBy { get; set; }

    public string DisclosedByFullName { get; set; }

    public bool DisclosedForSafeHarbor { get; set; }

    public DateTime ReceivedDate { get; set; }

    public bool IsDisclosedReceivedDateLocked { get; set; }

    public DateTime DisclosureCreatedDTTM { get; set; }

    public DisclosedMethodEnum DisclosureMethod { get; set; }

    public bool Received { get; set; }

    public string BorrowerName { get; set; }

    public string CoBorrowerName { get; set; }

    public string PropertyAddress { get; set; }

    public string PropertyCity { get; set; }

    public string PropertyState { get; set; }

    public string PropertyZip { get; set; }

    public string LoanProgram { get; set; }

    public string LoanAmount { get; set; }

    public bool IsManuallyCreated { get; set; }

    public bool IsDisclosedByLocked { get; set; }

    public DateTime ApplicationDate { get; set; }

    public string BorrowerPairID { get; set; }

    public Dictionary<string, string> DisclosedFields { get; set; }
  }
}
