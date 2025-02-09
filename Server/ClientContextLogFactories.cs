// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ClientContextLogFactories
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class ClientContextLogFactories
  {
    private static readonly Dictionary<Type, object> LogFactories = new Dictionary<Type, object>();

    public static void Clear() => ClientContextLogFactories.LogFactories.Clear();

    public static T GetLogFactory<T>() => (T) ClientContextLogFactories.LogFactories[typeof (T)];

    public static void AddLogFactory<T>(T factory)
    {
      ClientContextLogFactories.LogFactories[typeof (T)] = (object) factory;
    }
  }
}
