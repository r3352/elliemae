// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Viewers.VirtualPageAnnotation
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer.eFolder;
using System;
using System.Drawing;

#nullable disable
namespace EllieMae.EMLite.eFolder.Viewers
{
  public class VirtualPageAnnotation
  {
    private PageAnnotation annotation;
    private string tooltip;
    private Point location;
    private Size size;

    public VirtualPageAnnotation(PageAnnotation annotation)
    {
      this.annotation = annotation;
      this.tooltip = annotation.AddedBy + " - " + annotation.Date.ToString() + Environment.NewLine + annotation.Visibility.ToString() + Environment.NewLine + Environment.NewLine + annotation.Text;
    }

    public PageAnnotation Annotation => this.annotation;

    public Rectangle Bounds => new Rectangle(this.location, this.size);

    public int Height
    {
      get => this.size.Height;
      set => this.size.Height = value;
    }

    public int Left
    {
      get => this.location.X;
      set => this.location.X = value;
    }

    public Point Location
    {
      get => this.location;
      set => this.location = value;
    }

    public Size Size
    {
      get => this.size;
      set => this.size = value;
    }

    public string ToolTip => this.tooltip;

    public int Top
    {
      get => this.location.Y;
      set => this.location.Y = value;
    }

    public int Width
    {
      get => this.size.Width;
      set => this.size.Width = value;
    }
  }
}
