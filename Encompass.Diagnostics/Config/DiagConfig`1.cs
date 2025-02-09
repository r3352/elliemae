// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Config.DiagConfig`1
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace Encompass.Diagnostics.Config
{
  public class DiagConfig<TConfig> where TConfig : ConfigDataSection
  {
    public const string ConfigFileExtension = ".DiagnosticsConfig.json";
    public static DiagConfig<TConfig> Instance = new DiagConfig<TConfig>();
    private readonly string _configFilePath;
    private readonly FileSystemWatcher _fsw;
    private readonly List<IDiagConfigChangeHandler<TConfig>> _changeHandlers = new List<IDiagConfigChangeHandler<TConfig>>();
    private readonly ConcurrentDictionary<string, object> _globals = new ConcurrentDictionary<string, object>();

    public TConfig ConfigData { get; private set; }

    private DiagConfig()
    {
      string str = DiagUtility.GetAppName() + ".DiagnosticsConfig.json";
      string directoryName = Path.GetDirectoryName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
      this._configFilePath = Path.Combine(directoryName, str);
      try
      {
        this.ConfigData = this.ReadData();
      }
      catch
      {
        this.ConfigData = ConfigDataSection.NewInstance<TConfig>();
      }
      try
      {
        FileSystemEventHandler systemEventHandler = (FileSystemEventHandler) ((sender, e) => this.ReloadConfig());
        this._fsw = new FileSystemWatcher(directoryName, str)
        {
          NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.CreationTime,
          IncludeSubdirectories = false,
          EnableRaisingEvents = true
        };
        this._fsw.Created += systemEventHandler;
        this._fsw.Changed += systemEventHandler;
        this._fsw.Deleted += systemEventHandler;
      }
      catch
      {
      }
    }

    private TConfig ReadData()
    {
      JsonSerializerSettings settings = new JsonSerializerSettings();
      settings.Converters.Add((JsonConverter) new DictionaryConverter());
      settings.Converters.Add((JsonConverter) new LogLevelFilterConverter());
      JsonSerializer jsonSerializer = JsonSerializer.CreateDefault(settings);
      using (FileStream fileStream = new FileStream(this._configFilePath, FileMode.Open))
      {
        using (StreamReader reader1 = new StreamReader((Stream) fileStream))
        {
          using (JsonTextReader reader2 = new JsonTextReader((TextReader) reader1))
          {
            TConfig target = ConfigDataSection.NewInstance<TConfig>();
            jsonSerializer.Populate((JsonReader) reader2, (object) target);
            return target;
          }
        }
      }
    }

    public IDiagConfigChangeHandler<TConfig> AddHandler(IDiagConfigChangeHandler<TConfig> handler)
    {
      lock (this)
      {
        this._changeHandlers.Add(handler);
        if (handler.PerformsGlobalSetup)
          handler.GlobalSetup(this.ConfigData, false);
        if (handler.PerformsSetup)
          handler.Setup(this.ConfigData, false);
        return handler;
      }
    }

    public bool RemoveHandler(IDiagConfigChangeHandler<TConfig> handler)
    {
      lock (this)
      {
        if (!this._changeHandlers.Remove(handler))
          return false;
        if (handler.PerformsCleanup)
          handler.Cleanup(this.ConfigData);
        if (handler.PerformsGlobalCleanup)
          handler.GlobalCleanup(this.ConfigData);
        handler.Dispose();
        return true;
      }
    }

    public void ReloadConfig()
    {
      lock (this)
      {
        try
        {
          TConfig oldData = this.ConfigData;
          int num = 0;
          while (num < 3)
          {
            try
            {
              Thread.Sleep(500);
              this.ConfigData = this.ReadData();
              break;
            }
            catch (IOException ex)
            {
              ++num;
              if (num == 3)
              {
                this.ConfigData = ConfigDataSection.NewInstance<TConfig>();
                break;
              }
            }
            catch
            {
              this.ConfigData = ConfigDataSection.NewInstance<TConfig>();
              break;
            }
          }
          Parallel.ForEach<IDiagConfigChangeHandler<TConfig>>(this._changeHandlers.Where<IDiagConfigChangeHandler<TConfig>>((Func<IDiagConfigChangeHandler<TConfig>, bool>) (handler => handler.PerformsCleanup)), (Action<IDiagConfigChangeHandler<TConfig>>) (handler => handler.Cleanup(oldData)));
          Parallel.ForEach<IDiagConfigChangeHandler<TConfig>>(this._changeHandlers.Where<IDiagConfigChangeHandler<TConfig>>((Func<IDiagConfigChangeHandler<TConfig>, bool>) (handler => handler.PerformsGlobalCleanup)), (Action<IDiagConfigChangeHandler<TConfig>>) (handler => handler.GlobalCleanup(oldData)));
          Parallel.ForEach<IDiagConfigChangeHandler<TConfig>>(this._changeHandlers.Where<IDiagConfigChangeHandler<TConfig>>((Func<IDiagConfigChangeHandler<TConfig>, bool>) (handler => handler.PerformsGlobalSetup)), (Action<IDiagConfigChangeHandler<TConfig>>) (handler => handler.GlobalSetup(this.ConfigData, true)));
          Parallel.ForEach<IDiagConfigChangeHandler<TConfig>>(this._changeHandlers.Where<IDiagConfigChangeHandler<TConfig>>((Func<IDiagConfigChangeHandler<TConfig>, bool>) (handler => handler.PerformsSetup)), (Action<IDiagConfigChangeHandler<TConfig>>) (handler => handler.Setup(this.ConfigData, true)));
        }
        catch
        {
        }
      }
    }

    public void SetGlobalData<T>(string key, T data)
    {
      this._globals.AddOrUpdate(key, (object) data, (Func<string, object, object>) ((i, v) => (object) (T) data));
    }

    public void RemoveGlobalData(string key) => this._globals.TryRemove(key, out object _);

    public T GetGlobalData<T>(string key)
    {
      object obj1;
      return this._globals.TryGetValue(key, out obj1) && obj1 is T obj2 ? obj2 : default (T);
    }
  }
}
