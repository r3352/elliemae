// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ServerProcessInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using System;
using System.Net;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class ServerProcessInfo
  {
    private string hostName;
    private string ipAddress;
    private DateTime startTime;
    private DateTime currentTime;
    private OSVersion osVersion;
    private SystemUtil.MemoryInfo memoryInfo;
    private SystemUtil.ProcessorInfo processorInfo;
    private int processorCount;
    private long workingSet;
    private ServerInstanceInfo[] instances;
    private Version fieldListVersion;
    private Version serverVersion;
    private string serverChangeSetNumber;

    public ServerProcessInfo(
      DateTime startTime,
      ServerInstanceInfo[] instances,
      Version fieldListVersion,
      Version serverVersion)
    {
      IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
      this.hostName = Environment.MachineName;
      this.ipAddress = hostEntry.AddressList[0].ToString();
      this.startTime = startTime;
      this.currentTime = DateTime.Now;
      this.osVersion = SystemUtil.OS;
      this.memoryInfo = SystemUtil.GetMemoryInformation();
      this.processorInfo = SystemUtil.GetProcessorInformation();
      this.workingSet = Environment.WorkingSet;
      this.processorCount = Environment.ProcessorCount;
      this.instances = instances;
      this.fieldListVersion = fieldListVersion;
      this.serverVersion = serverVersion;
      this.serverChangeSetNumber = SystemUtil.GetServerChangesetNumber();
    }

    public string Hostname => this.hostName;

    public string IPAddress => this.ipAddress;

    public OSVersion OSVersion => this.osVersion;

    public DateTime StartTime => this.startTime;

    public DateTime CurrentTime => this.currentTime;

    public SystemUtil.MemoryInfo MemoryInfo => this.memoryInfo;

    public long WorkingSet => this.workingSet;

    public SystemUtil.ProcessorInfo ProcessorInfo => this.processorInfo;

    public int ProcessorCount => this.processorCount;

    public Version EncompassFieldListVersion => this.fieldListVersion;

    public Version EncompassServerVersion => this.serverVersion;

    public ServerInstanceInfo[] Instances => this.instances;

    public int GetEnabledInstanceCount()
    {
      int enabledInstanceCount = 0;
      foreach (ServerInstanceInfo instance in this.instances)
      {
        if (instance.Enabled)
          ++enabledInstanceCount;
      }
      return enabledInstanceCount;
    }

    public int GetActiveInstanceCount()
    {
      int activeInstanceCount = 0;
      foreach (ServerInstanceInfo instance in this.instances)
      {
        if (instance.SessionCount > 0)
          ++activeInstanceCount;
      }
      return activeInstanceCount;
    }

    public int GetTotalSessionCount()
    {
      int totalSessionCount = 0;
      foreach (ServerInstanceInfo instance in this.instances)
        totalSessionCount += instance.SessionCount;
      return totalSessionCount;
    }

    public int GetTotalSessionObjectCount()
    {
      int sessionObjectCount = 0;
      foreach (ServerInstanceInfo instance in this.instances)
        sessionObjectCount += instance.SessionObjectCount;
      return sessionObjectCount;
    }

    public string GetServerChangeSetNumber() => this.serverChangeSetNumber;
  }
}
