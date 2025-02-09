// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Configuration.LoanPipelineCacheStoreConfiguration
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Configuration;
using System;
using System.Configuration;

#nullable disable
namespace EllieMae.EMLite.Server.Configuration
{
  public class LoanPipelineCacheStoreConfiguration : CacheStoreConfiguration
  {
    [ConfigurationProperty("LeaseTime", IsRequired = false, DefaultValue = 60)]
    public new int LeaseTime => Convert.ToInt32(this[nameof (LeaseTime)]);

    [ConfigurationProperty("EntryTTLMinutes", IsRequired = false, DefaultValue = 720)]
    public int EntryTTLMinutes => Convert.ToInt32(this[nameof (EntryTTLMinutes)]);

    [ConfigurationProperty("ChunkSize", IsRequired = false, DefaultValue = 10000)]
    public int ChunkSize => Convert.ToInt32(this[nameof (ChunkSize)]);

    [ConfigurationProperty("MaxCursorLimit", IsRequired = false, DefaultValue = 10)]
    public int MaxCursorLimit => Convert.ToInt32(this[nameof (MaxCursorLimit)]);

    [ConfigurationProperty("CursorIdleSeconds", IsRequired = false, DefaultValue = 300)]
    public int CursorIdleSeconds => Convert.ToInt32(this[nameof (CursorIdleSeconds)]);

    [ConfigurationProperty("Type", DefaultValue = "InProcess", IsRequired = false)]
    public CacheStoreSource Type
    {
      get
      {
        return (CacheStoreSource) Enum.Parse(typeof (CacheStoreSource), this[nameof (Type)].ToString());
      }
    }

    [ConfigurationProperty("ConnectionTimeoutSeconds", IsRequired = false, DefaultValue = 3)]
    public int ConnectionTimeoutSeconds => Convert.ToInt32(this[nameof (ConnectionTimeoutSeconds)]);
  }
}
