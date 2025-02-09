// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Dashboard.DashboardDefaultView
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Dashboard
{
  [Serializable]
  public class DashboardDefaultView : IXmlSerializable
  {
    private Dictionary<string, string> mapping;

    public DashboardDefaultView() => this.mapping = new Dictionary<string, string>();

    public DashboardDefaultView(Dictionary<string, string> mapping) => this.mapping = mapping;

    public DashboardDefaultView(XmlSerializationInfo info)
    {
      this.mapping = new Dictionary<string, string>();
      IEnumerator enumerator = info.GetEnumerator();
      while (enumerator.MoveNext())
        this.mapping.Add(string.Concat(enumerator.Current), string.Concat(info.GetValue(string.Concat(enumerator.Current), typeof (string))));
    }

    public void AddMapping(string guid, string snapshotPath)
    {
      if (this.mapping.ContainsKey(guid))
        this.mapping[guid] = snapshotPath;
      else
        this.mapping.Add(guid, snapshotPath);
    }

    public bool Contains(string guid) => this.mapping.ContainsKey(guid);

    public string GetMappedSnapshotPath(string guid) => this.mapping[guid];

    public void Remove(string guid)
    {
      if (!this.Contains(guid))
        return;
      this.mapping.Remove(guid);
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      foreach (string key in this.mapping.Keys)
        info.AddValue(key, (object) this.mapping[key]);
    }
  }
}
