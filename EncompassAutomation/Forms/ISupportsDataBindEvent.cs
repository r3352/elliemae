// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.ISupportsDataBindEvent
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>
  /// Interface signaling that the control supports the DataBind event
  /// </summary>
  /// <exclude />
  public interface ISupportsDataBindEvent : ISupportsEvents
  {
    /// <summary>The DataBind event, used by FieldControls to bind data from the underlying
    /// loan into the control.</summary>
    event DataBindEventHandler DataBind;

    /// <summary>The DataCommit event, used by FieldControls to persist data from control
    /// into the underlying loan.</summary>
    event DataCommitEventHandler DataCommit;

    /// <summary>This method is intended for internal use within Encompass only.</summary>
    bool InvokeDataBind(ref string value);

    /// <summary>This method is intended for internal use within Encompass only.</summary>
    bool InvokeDataCommit(ref string value);
  }
}
