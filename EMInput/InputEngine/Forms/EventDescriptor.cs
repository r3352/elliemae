// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.Forms.EventDescriptor
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using System;

#nullable disable
namespace EllieMae.EMLite.InputEngine.Forms
{
  public class EventDescriptor
  {
    private string controlId;
    private string eventType;
    private string eventCode;

    public EventDescriptor(string controlId, string eventType, string eventCode)
    {
      this.controlId = controlId ?? "";
      this.eventType = eventType ?? "";
      this.eventCode = eventCode ?? "";
      if (this.controlId == "")
        throw new ArgumentException("Invalid control ID specified");
      if (this.eventType == "")
        throw new ArgumentException("Invalid event type specified");
    }

    public string ControlID => this.controlId;

    public string EventType => this.eventType;

    public string EventSourceCode => this.eventCode;
  }
}
