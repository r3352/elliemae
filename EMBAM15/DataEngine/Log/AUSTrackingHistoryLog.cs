// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.AUSTrackingHistoryLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class AUSTrackingHistoryLog : LogRecordBase
  {
    private readonly Dictionary<string, string> _submissionTypeToStringMapping = new Dictionary<string, string>()
    {
      {
        "DU",
        "Fannie Mae"
      },
      {
        "LP",
        "Freddie Mac"
      },
      {
        "LCLA",
        "Freddie Mac"
      },
      {
        "LQA",
        "Freddie Mac"
      },
      {
        "Manual Underwriting",
        "Manual Underwriting"
      },
      {
        "Other",
        "Other"
      },
      {
        "EarlyCheck",
        "Fannie Mae"
      }
    };
    private readonly Dictionary<string, string> _submissionTypeMapping = new Dictionary<string, string>()
    {
      {
        "Fannie Mae",
        "DU"
      },
      {
        "Freddie Mac",
        "LP"
      },
      {
        "LCLA",
        "Freddie Mac"
      },
      {
        "LQA",
        "Freddie Mac"
      },
      {
        "Manual Underwriting",
        "Value"
      },
      {
        "EarlyCheck",
        "Early Check"
      }
    };
    private string historyID;
    private DataTemplate dataValues;
    public static string[,] AUSFieldMapIDs = new string[34, 2]
    {
      {
        "AUS.X11",
        "MORNET.X75"
      },
      {
        "AUS.X12",
        "MORNET.X76"
      },
      {
        "AUS.X13",
        "1540"
      },
      {
        "AUS.X14",
        "740"
      },
      {
        "AUS.X15",
        "742"
      },
      {
        "AUS.X16",
        "2"
      },
      {
        "AUS.X17",
        "136"
      },
      {
        "AUS.X18",
        "356"
      },
      {
        "AUS.X19",
        "3"
      },
      {
        "AUS.X20",
        "1172"
      },
      {
        "AUS.X21",
        "4"
      },
      {
        "AUS.X22",
        "608"
      },
      {
        "AUS.X23",
        "19"
      },
      {
        "AUS.X24",
        "299"
      },
      {
        "AUS.X25",
        "228"
      },
      {
        "AUS.X26",
        "229"
      },
      {
        "AUS.X27",
        "230"
      },
      {
        "AUS.X28",
        "1405"
      },
      {
        "AUS.X29",
        "232"
      },
      {
        "AUS.X30",
        "233"
      },
      {
        "AUS.X31",
        "234"
      },
      {
        "AUS.X32",
        "912"
      },
      {
        "AUS.X33",
        "350"
      },
      {
        "AUS.X34",
        "901"
      },
      {
        "AUS.X35",
        "904"
      },
      {
        "AUS.X36",
        "903"
      },
      {
        "AUS.X37",
        "902"
      },
      {
        "AUS.X38",
        "907+908"
      },
      {
        "AUS.X39",
        "906"
      },
      {
        "AUS.X40",
        "1171"
      },
      {
        "AUS.X41",
        "1389"
      },
      {
        "AUS.X42",
        "915"
      },
      {
        "AUS.X199",
        "4752"
      },
      {
        "AUS.X200",
        "URLA.X144"
      }
    };

    public AUSTrackingHistoryLog(string historyID)
    {
      this.historyID = historyID;
      this.dataValues = new DataTemplate();
      this.dataValues.UsedForGeneralDataInput = true;
    }

    public string HistoryID
    {
      get => this.historyID;
      set => this.historyID = value;
    }

    public DataTemplate DataValues
    {
      get => this.dataValues;
      set => this.dataValues = value;
    }

    public void CopyLoanToLog(LoanData loan, bool forDU)
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      bool flag = loan.GetField("1811") == "PrimaryResidence";
      for (int index = 0; index <= AUSTrackingHistoryLog.AUSFieldMapIDs.GetUpperBound(0); ++index)
      {
        string id1 = AUSTrackingHistoryLog.AUSFieldMapIDs[index, 1];
        if (!forDU)
        {
          switch (id1)
          {
            case "740":
              id1 = "CASASRN.X201";
              break;
            case "742":
              id1 = "CASASRN.X202";
              break;
          }
        }
        string val;
        if (id1.IndexOf("+") > -1)
        {
          double num = 0.0;
          string str = id1;
          char[] chArray = new char[1]{ '+' };
          foreach (string id2 in str.Split(chArray))
            num += Utils.ParseDouble((object) loan.GetField(id2));
          val = num.ToString("N2");
        }
        else
        {
          if (flag && id1 == "912")
            id1 = "1731";
          val = loan.GetField(id1);
        }
        if (forDU && id1 == "4752" && loan.GetField("4830") == "Y")
          val = "not applicable";
        this.dataValues.SetCurrentField(AUSTrackingHistoryLog.AUSFieldMapIDs[index, 0], val);
      }
    }

    public string GetField(string id) => this.dataValues.GetField(id);

    public void SetField(string id, string val) => this.dataValues.SetCurrentField(id, val);

    public string RecordType
    {
      get => this.dataValues.GetField("AUS.X999");
      set => this.dataValues.SetCurrentField("AUS.X999", value);
    }

    public string SubmissionDate
    {
      get => this.dataValues.GetField("AUS.X3");
      set => this.dataValues.SetCurrentField("AUS.X3", value);
    }

    public string SubmissionTime
    {
      get => this.dataValues.GetField("AUS.X173");
      set => this.dataValues.SetCurrentField("AUS.X173", value);
    }

    public string SubmissionDateTime
    {
      get => this.dataValues.GetField("AUS.X3") + " " + this.dataValues.GetField("AUS.X173");
    }

    public string SubmissionType
    {
      get => this.dataValues.GetField("AUS.X1");
      set
      {
        this.dataValues.SetCurrentField("AUS.X1", this._submissionTypeMapping[value] ?? string.Empty);
      }
    }

    public string SubmissionTypeToString
    {
      get
      {
        return string.IsNullOrEmpty(this.SubmissionType) ? "" : this._submissionTypeToStringMapping[this.SubmissionType] ?? string.Empty;
      }
    }

    public string Recommendation
    {
      get => this.dataValues.GetField("AUS.X6");
      set => this.dataValues.SetCurrentField("AUS.X6", value);
    }

    public string SubmittedBy
    {
      get => this.dataValues.GetField("AUS.X8");
      set => this.dataValues.SetCurrentField("AUS.X8", value);
    }

    public string AUSVersion
    {
      get => this.dataValues.GetField("AUS.X9");
      set => this.dataValues.SetCurrentField("AUS.X9", value);
    }

    public string SubmissionNumber
    {
      get => this.dataValues.GetField("AUS.X5");
      set => this.dataValues.SetCurrentField("AUS.X5", value);
    }

    public string CreatedOn { get; set; }
  }
}
