// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DocEngine.DocEngineResponseStatus
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

#nullable disable
namespace EllieMae.EMLite.DocEngine
{
  public enum DocEngineResponseStatus
  {
    Error = -3, // 0xFFFFFFFD
    Exception = -2, // 0xFFFFFFFE
    Failure = -1, // 0xFFFFFFFF
    Unknown = 0,
    Success = 1,
    Warning = 2,
  }
}
