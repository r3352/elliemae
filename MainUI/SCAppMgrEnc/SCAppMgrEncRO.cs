// Decompiled with JetBrains decompiler
// Type: SCAppMgrEnc.SCAppMgrEncRO
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using System;

#nullable disable
namespace SCAppMgrEnc
{
  public class SCAppMgrEncRO : MarshalByRefObject
  {
    public static readonly SCAppMgrEncRO RemotingInstance = new SCAppMgrEncRO();

    private SCAppMgrEncRO()
    {
    }

    public string Echo(string input) => input;

    public override object InitializeLifetimeService() => (object) null;
  }
}
