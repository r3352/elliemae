// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.DefaultDistributedMutexPolicy
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

#nullable disable
namespace Elli.MessageQueues
{
  [ExcludeFromCodeCoverage]
  public class DefaultDistributedMutexPolicy : IDistributedMutexPolicy
  {
    public string GetMutexRootFolderPath() => Path.GetTempPath();

    public TimeSpan GetMutexTimeout() => TimeSpan.FromSeconds(30.0);
  }
}
