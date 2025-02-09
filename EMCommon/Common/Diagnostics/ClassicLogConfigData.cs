// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Diagnostics.ClassicLogConfigData
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using Encompass.Diagnostics.Config;
using Encompass.Diagnostics.Logging;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Common.Diagnostics
{
  public class ClassicLogConfigData : ConfigDataSection
  {
    public string Name { get; } = "ClassicLogListener";

    public string TargetName { get; } = "ClassicLogTarget";

    public virtual bool Enabled { get; set; } = true;

    public string DefaultLogLevelName { get; } = "Server";

    public virtual Dictionary<string, LogLevelFilter> LogLevels { get; set; } = Tracing.GetLogLevels();
  }
}
