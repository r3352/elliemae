// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BizLayer.Core.BindableBase
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System;

#nullable disable
namespace EllieMae.EMLite.BizLayer.Core
{
  [Serializable]
  public abstract class BindableBase
  {
    [NonSerialized]
    private EventHandler _nonSerializableHandlers;
    private EventHandler _serializableHandlers;

    public event EventHandler IsDirtyChanged
    {
      add
      {
        if (value.Method.IsPublic && (value.Method.DeclaringType.IsSerializable || value.Method.IsStatic))
          this._serializableHandlers += value;
        else
          this._nonSerializableHandlers += value;
      }
      remove
      {
        if (value.Method.IsPublic && (value.Method.DeclaringType.IsSerializable || value.Method.IsStatic))
          this._serializableHandlers -= value;
        else
          this._nonSerializableHandlers -= value;
      }
    }

    protected virtual void OnIsDirtyChanged()
    {
      if (this._nonSerializableHandlers != null)
        this._nonSerializableHandlers((object) this, EventArgs.Empty);
      if (this._serializableHandlers == null)
        return;
      this._serializableHandlers((object) this, EventArgs.Empty);
    }
  }
}
