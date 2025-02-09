// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.EmptyControl
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using System;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  internal class EmptyControl : Control
  {
    internal EmptyControl()
      : base("")
    {
    }

    public override string ControlID
    {
      get => base.ControlID;
      set => throw new InvalidOperationException("Control ID cannot be changed");
    }

    internal override string RenderHTML()
    {
      throw new InvalidOperationException("Control cannot be inserted into a form");
    }
  }
}
