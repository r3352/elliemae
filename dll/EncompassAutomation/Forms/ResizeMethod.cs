// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.ResizeMethod
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using System;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  [Flags]
  public enum ResizeMethod
  {
    None = 0,
    Width = 1,
    Height = 2,
    Both = Height | Width, // 0x00000003
  }
}
