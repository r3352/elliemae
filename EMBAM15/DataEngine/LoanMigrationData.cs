// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.LoanMigrationData
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Workflow;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class LoanMigrationData
  {
    public static readonly string[] LegacyPersonas = new string[3]
    {
      "LO",
      "LP",
      "CL"
    };
    public Hashtable RealWorldRoleMap;
    public MilestoneSetup MilestoneSetup;
    public List<Milestone> MilestoneList;
    public MilestoneTemplate DefaultTemplate;
    public Hashtable MilestoneDataMap;

    public LoanMigrationData(
      Hashtable realWorldRoleMap,
      MilestoneSetup msSetup,
      Hashtable milestoneDataMap)
    {
      this.RealWorldRoleMap = realWorldRoleMap;
      this.MilestoneSetup = msSetup;
      this.MilestoneDataMap = milestoneDataMap;
    }

    public LoanMigrationData(
      Hashtable realWorldRoleMap,
      List<Milestone> msList,
      MilestoneTemplate defaultTemplate,
      Hashtable milestoneDataMap)
    {
      this.RealWorldRoleMap = realWorldRoleMap;
      this.MilestoneList = msList;
      this.DefaultTemplate = defaultTemplate;
      this.MilestoneDataMap = milestoneDataMap;
    }

    public static Dictionary<string, string> GetInputFormNameMapping()
    {
      return new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase)
      {
        {
          "Additional Disclosures Info",
          "Additional Disclosures Information"
        },
        {
          "Additional Requests Info",
          "Additional Requests Information"
        },
        {
          "FHA Loan Transmittal",
          "HUD-92900LT FHA Loan Transmittal"
        },
        {
          "Freddie Mac Additional",
          "Freddie Mac Additional Data"
        },
        {
          "HUD56001 Prop Imp",
          "HUD-56001 Property Improvement"
        },
        {
          "HUD928005b Cond commitment",
          "HUD-928005b Conditional Commitment"
        },
        {
          "Request for copy of Tax",
          "Request for Copy of Tax Return"
        },
        {
          "VA260286 Loan Summary",
          "VA 26-0286 Loan Summary"
        },
        {
          "VA266393 Loan Analysis",
          "VA 26-6393 Loan Analysis"
        },
        {
          "VA268261A Veteran Status",
          "VA 26-8261A Veteran Status"
        },
        {
          "VA268923 Rate Reduction WS",
          "VA 26-8923 Rate Reduction WS"
        }
      };
    }

    [Serializable]
    public class RoleData
    {
      public int RoleID = -1;
      public string RoleName = "";
      public string RealWorldRoleType = "";

      public RoleData(int roleId, string roleName)
      {
        this.RoleID = roleId;
        this.RoleName = roleName;
      }
    }

    [Serializable]
    public class MilestoneData
    {
      public string MilestoneID = "";
      public LoanMigrationData.RoleData AssociatedRole;

      public MilestoneData()
      {
      }

      public MilestoneData(string msid, LoanMigrationData.RoleData role)
      {
        this.MilestoneID = msid;
        this.AssociatedRole = role;
      }
    }
  }
}
