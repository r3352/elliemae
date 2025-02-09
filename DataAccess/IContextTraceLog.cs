// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.IContextTraceLog
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using System;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.DataAccess
{
  public interface IContextTraceLog
  {
    void Write(string message, string category);

    void Write(TraceLevel level, string className, string message);

    void WriteException(TraceLevel level, string className, Exception ex);
  }
}
