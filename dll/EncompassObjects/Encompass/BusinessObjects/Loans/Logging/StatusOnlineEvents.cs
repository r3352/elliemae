// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.StatusOnlineEvents
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class StatusOnlineEvents : IStatusOnlineEvents
  {
    private ArrayList items = new ArrayList();

    internal StatusOnlineEvents(IEnumerable rawItems)
    {
      foreach (string[] rawItem in rawItems)
        this.items.Add((object) new StatusOnlineEvent(rawItem[0], rawItem[1]));
    }

    public int Count => this.items.Count;

    public StatusOnlineEvent this[int index] => (StatusOnlineEvent) this.items[index];

    public IEnumerator GetEnumerator() => this.items.GetEnumerator();
  }
}
