// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ServerIdentity
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using System;
using System.Net;
using System.Runtime.Serialization;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class ServerIdentity : ISerializable
  {
    public static readonly ServerIdentity Offline = new ServerIdentity();
    private Uri uri;
    private string instanceName;

    public ServerIdentity(Uri uri, string instanceName)
    {
      this.uri = !(uri == (Uri) null) ? uri : throw new ArgumentNullException(nameof (uri));
      this.instanceName = instanceName ?? "";
    }

    public ServerIdentity(string instanceName)
    {
      this.uri = (Uri) null;
      this.instanceName = instanceName;
    }

    private ServerIdentity()
    {
      this.uri = (Uri) null;
      this.instanceName = EnConfigurationSettings.InstanceName;
    }

    private ServerIdentity(SerializationInfo info, StreamingContext context)
    {
      this.uri = new Uri(info.GetString(nameof (uri)));
      this.instanceName = info.GetString("instance");
    }

    public Uri Uri => this.uri;

    public string InstanceName
    {
      get => this.instanceName.ToUpper();
      set => this.instanceName = value;
    }

    public bool IsHttp => this.uri != (Uri) null && this.uri.Scheme.ToLower().StartsWith("ghttp");

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      info.AddValue("uri", (object) this.uri.AbsoluteUri);
      info.AddValue("instance", (object) this.instanceName);
    }

    public override int GetHashCode()
    {
      return this.uri == (Uri) null ? 0 : this.uri.GetHashCode() ^ this.instanceName.ToLower().GetHashCode();
    }

    public override bool Equals(object obj)
    {
      return obj is ServerIdentity serverIdentity && string.Compare(serverIdentity.Uri.AbsoluteUri, this.Uri.AbsoluteUri, true) == 0 && string.Compare(serverIdentity.instanceName, this.instanceName, true) == 0;
    }

    public bool IsSameServer(ServerIdentity sid, bool compareScheme)
    {
      if (sid == null)
        return false;
      if (this.Equals((object) sid))
        return true;
      if (compareScheme && string.Compare(this.Uri.Scheme, sid.Uri.Scheme, true) != 0 || string.Compare(this.Uri.Scheme, sid.Uri.Scheme, true) == 0 && string.Compare(this.Uri.AbsolutePath, sid.Uri.AbsolutePath, true) != 0 || string.Compare(this.instanceName, sid.instanceName, true) != 0)
        return false;
      IPAddress[] hostAddresses1 = Dns.GetHostAddresses(this.Uri.Host);
      IPAddress[] hostAddresses2 = Dns.GetHostAddresses(sid.Uri.Host);
      foreach (IPAddress ipAddress1 in hostAddresses1)
      {
        foreach (IPAddress ipAddress2 in hostAddresses2)
        {
          if (ipAddress1.Equals((object) ipAddress2))
            return true;
        }
      }
      return false;
    }

    public override string ToString() => this.ToString(true, false);

    public string ToString(bool withInstanceName, bool withDefaultVirtualRoot)
    {
      if (this.uri == (Uri) null)
        return "";
      if (this.uri.Scheme.ToLower().StartsWith("gtcp"))
      {
        string str = this.uri.Host;
        if (this.uri.Port != EnConfigurationSettings.GlobalSettings.DefaultServerPortNumber)
          str = str + ":" + (object) this.uri.Port;
        if (withInstanceName && this.instanceName != "")
          str = str + "$" + this.instanceName;
        return str;
      }
      if (!this.uri.Scheme.ToLower().StartsWith("ghttp"))
        return "";
      string str1 = this.uri.Scheme.Substring(1) + "://" + this.uri.Host;
      if (!this.uri.IsDefaultPort)
        str1 = str1 + ":" + (object) this.uri.Port;
      if (withDefaultVirtualRoot || this.uri.AbsolutePath.ToLower() != "/encompass" && this.uri.AbsolutePath.ToLower() != "/encompass/")
        str1 += this.uri.AbsolutePath;
      if (withInstanceName && this.instanceName != "")
        str1 = str1 + "$" + this.instanceName;
      return str1;
    }

    public static ServerIdentity Parse(string server)
    {
      string instanceName = "";
      string[] strArray = server.Split('$');
      if (strArray.Length > 2)
        throw new FormatException("The server name format is invalid.");
      if (strArray.Length == 2)
      {
        server = strArray[0];
        instanceName = strArray[1];
      }
      if (server.IndexOf("://") < 0)
        server = "tcp://" + server;
      try
      {
        server = "g" + server;
        if (server.ToLower().StartsWith("gtcp://") && server.Substring(5).IndexOf(":") < 0)
          server = server + ":" + (object) EnConfigurationSettings.GlobalSettings.DefaultServerPortNumber;
        Uri uri = new Uri(server);
        if (uri.Scheme.ToLower() != "gtcp" && uri.Scheme.ToLower() != "ghttp" && uri.Scheme.ToLower() != "ghttps")
          throw new FormatException("The server name format is invalid.");
        if (uri.Scheme.ToLower().StartsWith("ghttp") && uri.PathAndQuery == "/")
          uri = new Uri(uri, "/" + EnConfigurationSettings.GlobalSettings.IIsDefaultVirtualRootName + "/");
        if (!uri.AbsoluteUri.EndsWith("/"))
          uri = new Uri(uri.AbsoluteUri + "/");
        return new ServerIdentity(uri, instanceName);
      }
      catch
      {
        throw new FormatException("The server name format is invalid.");
      }
    }
  }
}
