// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Collections.HashtableEventArgs
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Collections
{
  [Serializable]
  public class HashtableEventArgs : EventArgs
  {
    private object key;
    private object value;
    private bool keepChanges = true;

    public HashtableEventArgs(object key, object value)
    {
      this.key = key;
      this.value = value;
    }

    public bool KeepChanges
    {
      get => this.keepChanges;
      set => this.keepChanges = value;
    }

    public object Key => this.key;

    public object Value => this.value;
  }
}
