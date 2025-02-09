// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.MilestoneUIConfig
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System.Collections;
using System.Drawing;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class MilestoneUIConfig
  {
    private static Hashtable tbl = new Hashtable();

    private MilestoneUIConfig()
    {
    }

    static MilestoneUIConfig()
    {
      MilestoneUIConfig.tbl = new Hashtable();
      MilestoneUIConfig.tbl[(object) "Started"] = (object) new MilestoneSettings(Color.FromArgb(153, 153, 153), "File started", string.Empty, "Started");
      MilestoneUIConfig.tbl[(object) "Processing"] = (object) new MilestoneSettings(Color.FromArgb(166, 139, 34), "Sent to processing", "Send to processing", "Processing");
      MilestoneUIConfig.tbl[(object) "submittal"] = (object) new MilestoneSettings(Color.FromArgb(166, 93, 34), "Submitted", "Submittal", "Submittal");
      MilestoneUIConfig.tbl[(object) "Approval"] = (object) new MilestoneSettings(Color.FromArgb(65, 154, 32), "Approved", "Approval", "Approval");
      MilestoneUIConfig.tbl[(object) "Docs Signing"] = (object) new MilestoneSettings(Color.FromArgb(34, 114, 166), "Doc signed", "Doc signing", "Doc Signing");
      MilestoneUIConfig.tbl[(object) "Funding"] = (object) new MilestoneSettings(Color.FromArgb(153, 31, 153), "Funded", "Funding", "Funding");
      MilestoneUIConfig.tbl[(object) "Completion"] = (object) new MilestoneSettings(Color.Black, "Completed", "Completion", "Completion");
    }

    public static string GetMilestoneName(string displayName)
    {
      foreach (string key in (IEnumerable) MilestoneUIConfig.tbl.Keys)
      {
        if (((MilestoneSettings) MilestoneUIConfig.tbl[(object) key]).StageText == displayName)
          return key;
      }
      return displayName;
    }

    public static string GetDisplayName(string msName)
    {
      MilestoneSettings settings = MilestoneUIConfig.GetSettings(msName);
      return settings != null ? settings.StageText : msName;
    }

    public static MilestoneSettings GetSettings(string stage)
    {
      return (MilestoneSettings) MilestoneUIConfig.tbl[(object) stage];
    }

    public static MilestoneSettings[] GetAllSettings()
    {
      return (MilestoneSettings[]) new ArrayList()
      {
        MilestoneUIConfig.tbl[(object) "Started"],
        MilestoneUIConfig.tbl[(object) "Processing"],
        MilestoneUIConfig.tbl[(object) "submittal"],
        MilestoneUIConfig.tbl[(object) "Approval"],
        MilestoneUIConfig.tbl[(object) "Docs Signing"],
        MilestoneUIConfig.tbl[(object) "Funding"],
        MilestoneUIConfig.tbl[(object) "Completion"]
      }.ToArray(typeof (MilestoneSettings));
    }
  }
}
