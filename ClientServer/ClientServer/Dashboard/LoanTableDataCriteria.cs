// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Dashboard.LoanTableDataCriteria
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Query;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Dashboard
{
  [Serializable]
  public class LoanTableDataCriteria : DashboardDataCriteria
  {
    private int maxRows = 1000;
    private string timeFrameField = string.Empty;
    private List<string> fieldCriterionNames = new List<string>();
    private SortField[] sortFields;
    private DashboardTimeFrameType timeFrameType = DashboardTimeFrameType.None;

    public int MaxRows
    {
      get => this.maxRows;
      set => this.maxRows = value;
    }

    public string TimeFrameField
    {
      get => this.timeFrameField;
      set => this.timeFrameField = value;
    }

    public List<string> FieldCriterionNames
    {
      get => this.fieldCriterionNames;
      set => this.fieldCriterionNames = value;
    }

    public SortField[] SortFields
    {
      get => this.sortFields;
      set => this.sortFields = value;
    }

    public DashboardTimeFrameType TimeFrameType
    {
      get => this.timeFrameType;
      set => this.timeFrameType = value;
    }
  }
}
