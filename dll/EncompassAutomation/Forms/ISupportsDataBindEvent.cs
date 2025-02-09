// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.ISupportsDataBindEvent
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

#nullable disable
namespace EllieMae.Encompass.Forms
{
  public interface ISupportsDataBindEvent : ISupportsEvents
  {
    event DataBindEventHandler DataBind;

    event DataCommitEventHandler DataCommit;

    bool InvokeDataBind(ref string value);

    bool InvokeDataCommit(ref string value);
  }
}
