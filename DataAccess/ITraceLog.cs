// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.ITraceLog
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using System;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.DataAccess
{
  public interface ITraceLog
  {
    void WriteErrorI(string category, string message);

    void WriteWarningI(string category, string message);

    void WriteInfoI(string category, string message);

    void WriteDebugI(string category, string message);

    void WriteVerboseI(string category, string message);

    void WriteSqlTraceI(string category, DateTime start);

    void WriteExceptionI(string category, Exception ex);

    TraceLevel TraceLevelI { get; set; }
  }
}
