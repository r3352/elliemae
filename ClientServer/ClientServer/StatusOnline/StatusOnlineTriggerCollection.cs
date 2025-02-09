// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.StatusOnline.StatusOnlineTriggerCollection
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Serialization;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ClientServer.StatusOnline
{
  [Serializable]
  public class StatusOnlineTriggerCollection : CollectionBase, IXmlSerializable
  {
    public StatusOnlineTriggerCollection()
    {
    }

    public StatusOnlineTriggerCollection(XmlSerializationInfo info)
    {
      foreach (string name in info)
        this.Add((StatusOnlineTrigger) info.GetValue(name, typeof (StatusOnlineTrigger)));
    }

    public int Add(StatusOnlineTrigger trigger)
    {
      int index = this.List.IndexOf((object) trigger);
      if (index < 0)
        index = this.List.Add((object) trigger);
      else
        this.List[index] = (object) trigger;
      return index;
    }

    public StatusOnlineTrigger this[int index] => this.GetByIndex(index);

    public StatusOnlineTrigger this[string guid] => this.GetByID(guid);

    public StatusOnlineTrigger GetByIndex(int index) => (StatusOnlineTrigger) this.List[index];

    public StatusOnlineTrigger GetByID(string guid)
    {
      foreach (StatusOnlineTrigger byId in (IEnumerable) this.List)
      {
        if (byId.Guid == guid)
          return byId;
      }
      return (StatusOnlineTrigger) null;
    }

    public StatusOnlineTrigger[] GetUnpublishedTriggers()
    {
      System.Collections.Generic.List<StatusOnlineTrigger> statusOnlineTriggerList = new System.Collections.Generic.List<StatusOnlineTrigger>();
      foreach (StatusOnlineTrigger statusOnlineTrigger in (IEnumerable) this.List)
      {
        if (statusOnlineTrigger.DatePublished == DateTime.MinValue)
          statusOnlineTriggerList.Add(statusOnlineTrigger);
      }
      return statusOnlineTriggerList.ToArray();
    }

    public StatusOnlineTrigger[] GetPublishedTriggers()
    {
      System.Collections.Generic.List<StatusOnlineTrigger> statusOnlineTriggerList = new System.Collections.Generic.List<StatusOnlineTrigger>();
      foreach (StatusOnlineTrigger statusOnlineTrigger in (IEnumerable) this.List)
      {
        if (statusOnlineTrigger.DatePublished != DateTime.MinValue)
          statusOnlineTriggerList.Add(statusOnlineTrigger);
      }
      return statusOnlineTriggerList.ToArray();
    }

    public bool Contains(StatusOnlineTrigger trigger) => this.List.Contains((object) trigger);

    public void Remove(StatusOnlineTrigger trigger) => this.List.Remove((object) trigger);

    public StatusOnlineTrigger[] ToArray()
    {
      return (StatusOnlineTrigger[]) new ArrayList((ICollection) this.List).ToArray(typeof (StatusOnlineTrigger));
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      for (int index = 0; index < this.Count; ++index)
        info.AddValue(index.ToString(), (object) this[index]);
    }
  }
}
