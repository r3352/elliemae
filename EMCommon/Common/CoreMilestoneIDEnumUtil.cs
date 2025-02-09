// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.CoreMilestoneIDEnumUtil
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class CoreMilestoneIDEnumUtil
  {
    private static Hashtable _NameToValue = new Hashtable();
    private static Hashtable _ValueToName;

    static CoreMilestoneIDEnumUtil()
    {
      CoreMilestoneIDEnumUtil._NameToValue.Add((object) "Started", (object) CoreMilestoneID.Started);
      CoreMilestoneIDEnumUtil._NameToValue.Add((object) "Processing", (object) CoreMilestoneID.Processing);
      CoreMilestoneIDEnumUtil._NameToValue.Add((object) "submittal", (object) CoreMilestoneID.Submitted);
      CoreMilestoneIDEnumUtil._NameToValue.Add((object) "Approval", (object) CoreMilestoneID.Approved);
      CoreMilestoneIDEnumUtil._NameToValue.Add((object) "Docs Signing", (object) CoreMilestoneID.DocSigned);
      CoreMilestoneIDEnumUtil._NameToValue.Add((object) "Funding", (object) CoreMilestoneID.Funded);
      CoreMilestoneIDEnumUtil._NameToValue.Add((object) "Completion", (object) CoreMilestoneID.Completed);
      CoreMilestoneIDEnumUtil._ValueToName = new Hashtable();
      CoreMilestoneIDEnumUtil._ValueToName.Add((object) CoreMilestoneID.Started, (object) "Started");
      CoreMilestoneIDEnumUtil._ValueToName.Add((object) CoreMilestoneID.Processing, (object) "Processing");
      CoreMilestoneIDEnumUtil._ValueToName.Add((object) CoreMilestoneID.Submitted, (object) "submittal");
      CoreMilestoneIDEnumUtil._ValueToName.Add((object) CoreMilestoneID.Approved, (object) "Approval");
      CoreMilestoneIDEnumUtil._ValueToName.Add((object) CoreMilestoneID.DocSigned, (object) "Docs Signing");
      CoreMilestoneIDEnumUtil._ValueToName.Add((object) CoreMilestoneID.Funded, (object) "Funding");
      CoreMilestoneIDEnumUtil._ValueToName.Add((object) CoreMilestoneID.Completed, (object) "Completion");
    }

    public static object[] GetMilestoneNames()
    {
      CoreMilestoneID[] values = (CoreMilestoneID[]) Enum.GetValues(typeof (CoreMilestoneID));
      object[] milestoneNames = new object[values.Length];
      for (int index = 0; index < values.Length; ++index)
        milestoneNames[index] = (object) CoreMilestoneIDEnumUtil.ValueToName(values[index]);
      return milestoneNames;
    }

    public static CoreMilestoneID[] GetValues()
    {
      return (CoreMilestoneID[]) Enum.GetValues(typeof (CoreMilestoneID));
    }

    public static string ValueToName(CoreMilestoneID val)
    {
      return (string) CoreMilestoneIDEnumUtil._ValueToName[(object) val];
    }

    public static bool isCoreMilestone(string name)
    {
      return CoreMilestoneIDEnumUtil._NameToValue.Contains((object) name);
    }

    public static bool IsCoreMilestoneID(string milestoneID)
    {
      if (!Utils.IsInt((object) milestoneID))
        return false;
      CoreMilestoneID key = (CoreMilestoneID) Enum.ToObject(typeof (CoreMilestoneID), int.Parse(milestoneID));
      return CoreMilestoneIDEnumUtil._ValueToName.ContainsKey((object) key);
    }

    public static CoreMilestoneID GetCoreMilestoneID(string milestoneID)
    {
      return Utils.IsInt((object) milestoneID) ? (CoreMilestoneID) Enum.ToObject(typeof (CoreMilestoneID), int.Parse(milestoneID)) : throw new Exception("Milestone ID: " + milestoneID + " is not a core milestone ID.");
    }

    public static string GetMilestoneID(CoreMilestoneID coreMilestoneID)
    {
      return ((int) coreMilestoneID).ToString();
    }

    public static string GetDisplayName(CoreMilestoneID msId)
    {
      return MilestoneUIConfig.GetSettings(CoreMilestoneIDEnumUtil.ValueToName(msId)).StageText;
    }

    public static CoreMilestoneID NameToValue(string name)
    {
      return (CoreMilestoneID) CoreMilestoneIDEnumUtil._NameToValue[(object) name];
    }

    public static MilestoneSettings GetMilestoneSettings(string coreMsId)
    {
      return CoreMilestoneIDEnumUtil.GetMilestoneSettings(CoreMilestoneIDEnumUtil.GetCoreMilestoneID(coreMsId));
    }

    public static MilestoneSettings GetMilestoneSettings(CoreMilestoneID coreMsId)
    {
      return MilestoneUIConfig.GetSettings(CoreMilestoneIDEnumUtil.ValueToName(coreMsId));
    }
  }
}
