// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.LoanFolderRuleInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class LoanFolderRuleInfo
  {
    public readonly string LoanFolder;
    public readonly bool CanOriginateLoans;
    public readonly bool CanDuplicateLoansFrom;
    public readonly bool CanDuplicateLoansInto;
    public readonly bool CanImportLoans;
    public readonly bool CanPiggybackLoans;
    public readonly BizRule.LoanFolderMoveRuleOption MoveRuleOption;
    public readonly string MilestoneID;
    public readonly bool[] LoanStatusSettings;
    public readonly DateTime LastModTime = DateTime.MinValue;
    private bool useDefaultData;

    public LoanFolderRuleInfo(string loanFolder)
    {
      this.LoanFolder = loanFolder;
      this.CanImportLoans = true;
      this.CanDuplicateLoansInto = true;
      this.CanPiggybackLoans = true;
      this.useDefaultData = true;
      this.MoveRuleOption = BizRule.LoanFolderMoveRuleOption.None;
      this.MilestoneID = "1";
      this.LoanStatusSettings = new bool[9];
    }

    public LoanFolderRuleInfo(string loanFolder, bool optionSetting)
      : this(loanFolder, optionSetting, optionSetting, optionSetting, optionSetting, optionSetting)
    {
    }

    public LoanFolderRuleInfo(
      string loanFolder,
      bool canOriginateLoans,
      bool canDuplicateLoansFrom,
      bool canDuplicateLoansInto,
      bool canImportLoans,
      bool canPiggybackLoans)
      : this(loanFolder, canOriginateLoans, canDuplicateLoansFrom, canDuplicateLoansInto, canImportLoans, canPiggybackLoans, BizRule.LoanFolderMoveRuleOption.None, (string) null, new bool[9])
    {
    }

    public LoanFolderRuleInfo(
      string loanFolder,
      bool canOriginateLoans,
      bool canDuplicateLoansFrom,
      bool canDuplicateLoansInto,
      bool canImportLoans,
      bool canPiggybackLoans,
      BizRule.LoanFolderMoveRuleOption moveRuleOption,
      string milestoneID,
      bool[] loanStatusSettings)
    {
      this.LoanFolder = loanFolder;
      this.CanOriginateLoans = canOriginateLoans;
      this.CanDuplicateLoansFrom = canDuplicateLoansFrom;
      this.CanDuplicateLoansInto = canDuplicateLoansInto;
      this.CanImportLoans = canImportLoans;
      this.CanPiggybackLoans = canPiggybackLoans;
      this.MoveRuleOption = moveRuleOption;
      this.MilestoneID = milestoneID == null ? "1" : milestoneID;
      if (loanStatusSettings != null)
      {
        this.LoanStatusSettings = loanStatusSettings.Length == 9 ? loanStatusSettings : throw new Exception("loanStatuses length must be 9");
      }
      else
      {
        this.LoanStatusSettings = new bool[9];
        for (int index = 0; index < 9; ++index)
          this.LoanStatusSettings[index] = true;
      }
    }

    public LoanFolderRuleInfo(
      string loanFolder,
      bool canOriginateLoans,
      bool canDuplicateLoansFrom,
      bool canDuplicateLoansInto,
      bool canImportLoans,
      bool canPiggybackLoans,
      BizRule.LoanFolderMoveRuleOption moveRuleOption,
      string milestoneID,
      ArrayList loanStatusList)
      : this(loanFolder, canOriginateLoans, canDuplicateLoansFrom, canDuplicateLoansInto, canImportLoans, canPiggybackLoans, moveRuleOption, milestoneID, new bool[9])
    {
      for (int index = 0; index < 9; ++index)
        this.LoanStatusSettings[index] = loanStatusList == null || loanStatusList.Contains((object) (LoanStatusMap.LoanStatus) index);
    }

    public LoanFolderRuleInfo(
      string loanFolder,
      bool canOriginateLoans,
      bool canDuplicateLoansFrom,
      bool canDuplicateLoansInto,
      bool canImportLoans,
      bool canPiggybackLoans,
      BizRule.LoanFolderMoveRuleOption moveRuleOption,
      string milestoneID,
      Hashtable loanStatusHT)
      : this(loanFolder, canOriginateLoans, canDuplicateLoansFrom, canDuplicateLoansInto, canImportLoans, canPiggybackLoans, moveRuleOption, milestoneID, new bool[9])
    {
      for (int key = 0; key < 9; ++key)
      {
        object obj = (object) null;
        if (loanStatusHT != null)
          obj = loanStatusHT[(object) (LoanStatusMap.LoanStatus) key];
        this.LoanStatusSettings[key] = obj == null || (bool) obj;
      }
    }

    public LoanFolderRuleInfo(string loanFolder, DataRow row)
    {
      this.LoanStatusSettings = new bool[9];
      if (row == null)
      {
        this.LoanFolder = loanFolder;
        this.CanOriginateLoans = true;
        this.CanImportLoans = true;
        this.CanDuplicateLoansFrom = true;
        this.CanDuplicateLoansInto = true;
        this.CanPiggybackLoans = true;
        this.MoveRuleOption = BizRule.LoanFolderMoveRuleOption.None;
        this.MilestoneID = "";
        for (int index = 0; index < 9; ++index)
          this.LoanStatusSettings[index] = true;
      }
      else
      {
        this.LoanFolder = (string) row[nameof (loanFolder)];
        this.CanOriginateLoans = (byte) row["canOriginateLoans"] == (byte) 1;
        this.CanDuplicateLoansFrom = (byte) row["canDuplicateLoansFrom"] == (byte) 1;
        this.CanDuplicateLoansInto = (byte) row["canDuplicateLoansInto"] == (byte) 1;
        this.CanImportLoans = (byte) row["canImportLoans"] == (byte) 1;
        this.CanPiggybackLoans = (byte) row["canPiggybackLoans"] == (byte) 1;
        this.MoveRuleOption = (BizRule.LoanFolderMoveRuleOption) (byte) row["moveRuleOption"];
        this.MilestoneID = (string) row["milestoneID"];
        for (int index = 0; index < 9; ++index)
          this.LoanStatusSettings[index] = (byte) row[BizRule.MoveRuleLoanStatusDbColNames[index]] == (byte) 1;
        this.LastModTime = (DateTime) row["lastModTime"];
      }
    }

    [PgReady]
    public LoanFolderRuleInfo(string loanFolder, DataRow row, DbServerType dbServerType)
    {
      if (dbServerType == DbServerType.Postgres)
      {
        this.LoanStatusSettings = new bool[9];
        if (row == null)
        {
          this.LoanFolder = loanFolder;
          this.CanOriginateLoans = true;
          this.CanImportLoans = true;
          this.CanDuplicateLoansFrom = true;
          this.CanDuplicateLoansInto = true;
          this.CanPiggybackLoans = true;
          this.MoveRuleOption = BizRule.LoanFolderMoveRuleOption.None;
          this.MilestoneID = "";
          for (int index = 0; index < 9; ++index)
            this.LoanStatusSettings[index] = true;
        }
        else
        {
          this.LoanFolder = (string) row[nameof (loanFolder)];
          this.CanOriginateLoans = Convert.ToInt32(row["canOriginateLoans"]) == 1;
          this.CanDuplicateLoansFrom = Convert.ToInt32(row["canDuplicateLoansFrom"]) == 1;
          this.CanDuplicateLoansInto = Convert.ToInt32(row["canDuplicateLoansInto"]) == 1;
          this.CanImportLoans = Convert.ToInt32(row["canImportLoans"]) == 1;
          this.CanPiggybackLoans = Convert.ToInt32(row["canPiggybackLoans"]) == 1;
          this.MoveRuleOption = (BizRule.LoanFolderMoveRuleOption) Convert.ToInt32(row["moveRuleOption"]);
          this.MilestoneID = (string) row["milestoneID"];
          for (int index = 0; index < 9; ++index)
            this.LoanStatusSettings[index] = Convert.ToInt32(row[BizRule.MoveRuleLoanStatusDbColNames[index]]) == 1;
          this.LastModTime = (DateTime) row["lastModTime"];
        }
      }
      else
      {
        this.LoanStatusSettings = new bool[9];
        if (row == null)
        {
          this.LoanFolder = loanFolder;
          this.CanOriginateLoans = true;
          this.CanImportLoans = true;
          this.CanDuplicateLoansFrom = true;
          this.CanDuplicateLoansInto = true;
          this.CanPiggybackLoans = true;
          this.MoveRuleOption = BizRule.LoanFolderMoveRuleOption.None;
          this.MilestoneID = "";
          for (int index = 0; index < 9; ++index)
            this.LoanStatusSettings[index] = true;
        }
        else
        {
          this.LoanFolder = (string) row[nameof (loanFolder)];
          this.CanOriginateLoans = (byte) row["canOriginateLoans"] == (byte) 1;
          this.CanDuplicateLoansFrom = (byte) row["canDuplicateLoansFrom"] == (byte) 1;
          this.CanDuplicateLoansInto = (byte) row["canDuplicateLoansInto"] == (byte) 1;
          this.CanImportLoans = (byte) row["canImportLoans"] == (byte) 1;
          this.CanPiggybackLoans = (byte) row["canPiggybackLoans"] == (byte) 1;
          this.MoveRuleOption = (BizRule.LoanFolderMoveRuleOption) (byte) row["moveRuleOption"];
          this.MilestoneID = (string) row["milestoneID"];
          for (int index = 0; index < 9; ++index)
            this.LoanStatusSettings[index] = (byte) row[BizRule.MoveRuleLoanStatusDbColNames[index]] == (byte) 1;
          this.LastModTime = (DateTime) row["lastModTime"];
        }
      }
    }

    public bool GetLoanStatusSetting(LoanStatusMap.LoanStatus loanStatus)
    {
      return this.LoanStatusSettings[(int) loanStatus];
    }

    public bool IsActionAllowed(LoanFolderAction action)
    {
      switch (action)
      {
        case LoanFolderAction.Originate:
          return this.CanOriginateLoans;
        case LoanFolderAction.DuplicateFrom:
          return this.CanDuplicateLoansFrom;
        case LoanFolderAction.DuplicateInto:
          return this.CanDuplicateLoansInto;
        case LoanFolderAction.Import:
          return this.CanImportLoans;
        case LoanFolderAction.Piggyback:
          return this.CanPiggybackLoans;
        default:
          throw new ArgumentException("The value '" + (object) action + "' is not a valid LoanFolderAction type");
      }
    }

    public bool UseDefaultData => this.useDefaultData;
  }
}
