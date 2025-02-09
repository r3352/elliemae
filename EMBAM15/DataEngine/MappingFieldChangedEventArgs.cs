// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.MappingFieldChangedEventArgs
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class MappingFieldChangedEventArgs : FieldChangedEventArgs
  {
    private bool supressNotification;

    public bool SuppressNotification
    {
      get => this.supressNotification;
      set => this.supressNotification = value;
    }

    internal MappingFieldChangedEventArgs(
      string fieldId,
      BorrowerPair pair,
      string priorValue,
      string newValue,
      bool supressNotifications)
      : base(fieldId, pair, priorValue, newValue)
    {
      this.supressNotification = supressNotifications;
    }
  }
}
