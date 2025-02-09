// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.Browser.IBrowserParams
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ePass.Browser
{
  public interface IBrowserParams
  {
    [Obsolete]
    string[] CustomBrowserPreference { get; set; }

    string Scope { get; set; }

    Func<string, string> EncPostMessageHandler { get; set; }

    Action UnloadHandler { get; set; }
  }
}
