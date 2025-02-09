// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.IEncLoanData
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public interface IEncLoanData
  {
    bool IsFieldReadOnly(string fieldID);

    LoanContentAccess ContentAccess { get; set; }

    void SetField(string id, string val);

    void SetFieldFromCal(string id, string val);

    void SetCurrentField(string id, string val);

    void SetCurrentField(string id, string val, bool customCalcsOnly);

    void SetCurrentFieldFromCal(string id, string val);

    string GetField(string id);

    string GetField(string id, int borIndex);
  }
}
