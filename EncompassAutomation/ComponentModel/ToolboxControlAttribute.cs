// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.ComponentModel.ToolboxControlAttribute
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using System;
using System.Drawing;
using System.IO;
using System.Reflection;

#nullable disable
namespace EllieMae.Encompass.ComponentModel
{
  /// <summary>
  /// Describes the appearance of a control within the Form Builder toolbox.
  /// </summary>
  /// <exclude />
  [AttributeUsage(AttributeTargets.Class)]
  public class ToolboxControlAttribute : Attribute
  {
    private string displayName;
    private Image icon;

    /// <summary>Constructor for a ToolboxAttribute object.</summary>
    /// <param name="displayName">The display name of the associated control.</param>
    public ToolboxControlAttribute(string displayName)
      : this(displayName, (string) null)
    {
    }

    /// <summary>
    /// Constructor for a ToolboxAttribute object including an image source.
    /// </summary>
    /// <param name="displayName">The display name of the associated control.</param>
    /// <param name="imgSource">The name of the internal resource stream containing the image for the toolbox.</param>
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

    /// <summary>Gets the display name of the component.</summary>
    public string DisplayName => this.displayName;

    /// <summary>
    /// Gets the icon to be used in the toolbox for the component.
    /// </summary>
    public Image ToolboxIcon => this.icon;
  }
}
