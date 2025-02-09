// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.ComponentModel.ToolboxControlAttribute
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using System;
using System.Drawing;
using System.IO;
using System.Reflection;

#nullable disable
namespace EllieMae.Encompass.ComponentModel
{
  [AttributeUsage(AttributeTargets.Class)]
  public class ToolboxControlAttribute : Attribute
  {
    private string displayName;
    private Image icon;

    public ToolboxControlAttribute(string displayName)
      : this(displayName, (string) null)
    {
    }

    public ToolboxControlAttribute(string displayName, string imgSource)
    {
      this.displayName = displayName;
      try
      {
        using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(this.GetType(), imgSource))
          this.icon = Image.FromStream(manifestResourceStream);
      }
      catch
      {
        this.icon = (Image) null;
      }
    }

    public string DisplayName => this.displayName;

    public Image ToolboxIcon => this.icon;
  }
}
