// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.AUSTrackingHistoryList
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class AUSTrackingHistoryList
  {
    private List<AUSTrackingHistoryLog> trackingList;

    public AUSTrackingHistoryList() => this.trackingList = new List<AUSTrackingHistoryLog>();

    public void AddHistory(AUSTrackingHistoryLog rec) => this.trackingList.Add(rec);

    public void RemoveHistory(string historyID)
    {
      foreach (AUSTrackingHistoryLog tracking in this.trackingList)
      {
        if (string.Compare(tracking.HistoryID, historyID, true) == 0)
        {
          this.trackingList.Remove(tracking);
          break;
        }
      }
    }

    public int HistoryCount => this.trackingList.Count;

    public AUSTrackingHistoryLog GetHistoryAt(int i) => this.trackingList[i];

    public void Sort()
    {
      for (int index1 = 0; index1 < this.trackingList.Count - 1; ++index1)
      {
        for (int index2 = index1 + 1; index2 < this.trackingList.Count; ++index2)
        {
          if (this.trackingList[index1].Date < this.trackingList[index2].Date)
          {
            AUSTrackingHistoryLog tracking = this.trackingList[index1];
            this.trackingList[index1] = this.trackingList[index2];
            this.trackingList[index2] = tracking;
          }
        }
      }
    }
  }
}
