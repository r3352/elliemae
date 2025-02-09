// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.Schema.CustomLevelLog
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Encompass.Diagnostics.Logging.Schema
{
  [Serializable]
  public class CustomLevelLog : Log
  {
    private readonly string _customLevel;

    public CustomLevelLog(string level) => this._customLevel = level;

    protected CustomLevelLog(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this._customLevel = info.GetString(nameof (_customLevel));
    }

    public override string GetLogLevel() => this._customLevel;

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      info.AddValue("_customLevel", (object) this._customLevel);
      base.GetObjectData(info, context);
    }
  }
}
