// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.Forms.EventPrototype
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

#nullable disable
namespace EllieMae.EMLite.InputEngine.Forms
{
  public class EventPrototype
  {
    private System.Reflection.EventInfo eventInfo;

    public EventPrototype(System.Reflection.EventInfo eventInfo) => this.eventInfo = eventInfo;

    public string EventHandlerTypeName => this.eventInfo.EventHandlerType.FullName;

    public string EventArgsTypeName
    {
      get
      {
        return this.eventInfo.EventHandlerType.GetMethod("Invoke").GetParameters()[1].ParameterType.FullName;
      }
    }
  }
}
