// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.IServerProgressFeedback2
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface IServerProgressFeedback2
  {
    bool Cancel { get; set; }

    int GetMaxValue(int pbIdx);

    void SetMaxValue(int pbIdx, int value);

    string GetStatus(int pbIdx);

    void SetStatus(int pbIdx, string value);

    string GetDetails(int pbIdx);

    void SetDetails(int pbIdx, string value);

    bool Increment(int pbIdx, int count);

    int GetValue(int pbIdx);

    void SetValue(int pbIdx, int value);

    bool ResetCounter(int pbIdx, int maxValue);

    bool SetFeedback(int pbIdx, string status, string details, int value);

    int NumberOfThreads { get; }
  }
}
