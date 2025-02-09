// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Client.ServerResolver
// Assembly: Client, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: AD6D6217-37E4-4BE3-B44A-5E3BA190AF3A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Client.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.IO;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Client
{
  public class ServerResolver
  {
    private static Hashtable serverMap = new Hashtable();

    static ServerResolver()
    {
      string path = Path.Combine(SystemSettings.LocalSettingsDir, "ServerRedirect.config");
      if (!File.Exists(path))
        return;
      using (StreamReader streamReader = new StreamReader(path, Encoding.ASCII))
      {
        string str;
        while ((str = streamReader.ReadLine()) != null)
        {
          if (!str.StartsWith("#"))
          {
            int length = str.IndexOf(":");
            if (length > 0)
            {
              ServerIdentity key;
              try
              {
                key = ServerIdentity.Parse(str.Substring(0, length).Trim());
              }
              catch
              {
                continue;
              }
              string[] strArray = str.Substring(length + 1).Split(',', ';');
              ArrayList arrayList = new ArrayList();
              for (int index = 0; index < strArray.Length; ++index)
              {
                try
                {
                  arrayList.Add((object) ServerIdentity.Parse(strArray[index].Trim()));
                }
                catch
                {
                }
              }
              if (ServerResolver.serverMap.ContainsKey((object) key))
                throw new Exception(key.ToString() + ": duplicate server name in ServerRedirect.config");
              ServerResolver.serverMap.Add((object) key, (object) (ServerIdentity[]) arrayList.ToArray(typeof (ServerIdentity)));
            }
          }
        }
      }
    }

    public static ServerIdentity[] Resolve(ServerIdentity sid)
    {
      ServerIdentity[] server1 = (ServerIdentity[]) ServerResolver.serverMap[(object) sid];
      if (server1 != null && server1.Length != 0)
        return server1;
      ServerIdentity[] server2 = (ServerIdentity[]) ServerResolver.serverMap[(object) new ServerIdentity(sid.Uri, "*")];
      if (server2 != null && server2.Length != 0)
      {
        for (int index = 0; index < server2.Length; ++index)
          server2[index] = new ServerIdentity(server2[index].Uri, sid.InstanceName);
        return server2;
      }
      return new ServerIdentity[1]{ sid };
    }
  }
}
