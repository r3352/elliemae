// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Milestone
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System.Drawing;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public sealed class Milestone
  {
    public const string Started = "Started";
    public const string Processing = "Processing";
    public const string Submitted = "submittal";
    public const string Approved = "Approval";
    public const string DocSigned = "Docs Signing";
    public const string Funded = "Funding";
    public const string Completed = "Completion";
    public const string Rejected = "Rejected";
    public const string Suspended = "Suspended";
    public static Color WarningColor = Color.FromArgb(204, 0, 0);
    public static string[] Stages = new string[7]
    {
      nameof (Started),
      nameof (Processing),
      "submittal",
      "Approval",
      "Docs Signing",
      "Funding",
      "Completion"
    };

    public static string GetCoreMilestoneID(string stage)
    {
      for (int index = 0; index < Milestone.Stages.Length; ++index)
      {
        if (stage == Milestone.Stages[index])
          return string.Concat((object) (index + 1));
      }
      return (string) null;
    }

    public static bool IsCoreMilestone(string ms)
    {
      bool flag = false;
      foreach (string stage in Milestone.Stages)
      {
        if (stage == ms)
        {
          flag = true;
          break;
        }
      }
      return flag;
    }
  }
}
