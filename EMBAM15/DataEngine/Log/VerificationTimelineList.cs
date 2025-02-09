// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.VerificationTimelineList
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class VerificationTimelineList
  {
    private List<VerificationTimelineLog> timelines;

    public VerificationTimelineList() => this.timelines = new List<VerificationTimelineLog>();

    public void AddTimeline(VerificationTimelineLog timeline) => this.timelines.Add(timeline);

    public void RemoveTimeline(string timelineGuid)
    {
      foreach (VerificationTimelineLog timeline in this.timelines)
      {
        if (string.Compare(timeline.Guid, timelineGuid, true) == 0)
        {
          this.timelines.Remove(timeline);
          break;
        }
      }
    }

    public int TimelineCount => this.timelines.Count;

    public VerificationTimelineLog GetTimelineAt(int i) => this.timelines[i];

    public void Sort()
    {
      for (int index1 = 0; index1 < this.timelines.Count - 1; ++index1)
      {
        for (int index2 = index1 + 1; index2 < this.timelines.Count; ++index2)
        {
          if (this.timelines[index1].Date < this.timelines[index2].Date)
          {
            VerificationTimelineLog timeline = this.timelines[index1];
            this.timelines[index1] = this.timelines[index2];
            this.timelines[index2] = timeline;
          }
        }
      }
    }
  }
}
