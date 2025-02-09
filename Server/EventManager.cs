// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.EventManager
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class EventManager : IEventManager
  {
    private const string className = "EventManager�";
    private Hashtable eventListeners = new Hashtable();

    public void RegisterListener(Type eventType, IClientSession session)
    {
      lock (this)
      {
        if (!this.eventListeners.ContainsKey((object) eventType))
          this.addEventType(eventType);
        this.addEventListener(eventType, session);
      }
    }

    public void UnregisterListener(Type eventType, IClientSession session)
    {
      lock (this)
      {
        if (!this.eventListeners.ContainsKey((object) eventType))
          this.addEventType(eventType);
        this.removeEventListener(eventType, session);
      }
    }

    public void UnregisterListenerFromAll(IClientSession session)
    {
      lock (this)
      {
        foreach (DictionaryEntry eventListener in this.eventListeners)
        {
          ArrayList arrayList = (ArrayList) eventListener.Value;
          if (arrayList.Contains((object) session))
            arrayList.Remove((object) session);
        }
      }
    }

    public IClientSession[] GetListeners(Type eventType)
    {
      lock (this)
      {
        if (!this.eventListeners.ContainsKey((object) eventType))
          this.addEventType(eventType);
        return (IClientSession[]) ((ArrayList) this.eventListeners[(object) eventType]).ToArray(typeof (IClientSession));
      }
    }

    private void addEventType(Type eventType)
    {
      ArrayList arrayList1 = new ArrayList();
      foreach (DictionaryEntry eventListener in this.eventListeners)
      {
        if (((Type) eventListener.Key).IsAssignableFrom(eventType))
        {
          ArrayList arrayList2 = (ArrayList) eventListener.Value;
          for (int index = 0; index < arrayList2.Count; ++index)
          {
            if (!arrayList1.Contains(arrayList2[index]))
              arrayList1.Add(arrayList2[index]);
          }
        }
      }
      this.eventListeners.Add((object) eventType, (object) arrayList1);
    }

    private void addEventListener(Type eventType, IClientSession session)
    {
      foreach (DictionaryEntry eventListener in this.eventListeners)
      {
        if (eventType.IsAssignableFrom((Type) eventListener.Key))
        {
          ArrayList arrayList = (ArrayList) eventListener.Value;
          if (!arrayList.Contains((object) session))
            arrayList.Add((object) session);
        }
      }
    }

    private void removeEventListener(Type eventType, IClientSession session)
    {
      foreach (DictionaryEntry eventListener in this.eventListeners)
      {
        if (eventType.IsAssignableFrom((Type) eventListener.Key))
        {
          ArrayList arrayList = (ArrayList) eventListener.Value;
          if (arrayList.Contains((object) session))
            arrayList.Remove((object) session);
        }
      }
    }
  }
}
