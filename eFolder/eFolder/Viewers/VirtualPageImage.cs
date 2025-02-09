// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Viewers.VirtualPageImage
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer.eFolder;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace EllieMae.EMLite.eFolder.Viewers
{
  public class VirtualPageImage
  {
    private PageImage page;
    private VirtualPageAnnotation[] virtualAnnotationList;
    private Point location;
    private Size size;

    public VirtualPageImage(PageImage page)
    {
      this.page = page;
      List<VirtualPageAnnotation> virtualPageAnnotationList = new List<VirtualPageAnnotation>();
      foreach (PageAnnotation annotation in page.Annotations)
      {
        VirtualPageAnnotation virtualPageAnnotation = new VirtualPageAnnotation(annotation);
        virtualPageAnnotationList.Add(virtualPageAnnotation);
      }
      this.virtualAnnotationList = virtualPageAnnotationList.ToArray();
    }

    public PageImage Page => this.page;

    public VirtualPageAnnotation[] Annotations => this.virtualAnnotationList;

    public Rectangle Bounds => new Rectangle(this.location, this.size);

    public int Height
    {
      get => this.size.Height;
      set
      {
        this.size.Height = value;
        this.updateAnnotationSizes();
      }
    }

    public int Left
    {
      get => this.location.X;
      set
      {
        this.location.X = value;
        this.updateAnnotationLocations();
      }
    }

    public Point Location
    {
      get => this.location;
      set
      {
        this.location = value;
        this.updateAnnotationLocations();
      }
    }

    public Size Size
    {
      get => this.size;
      set
      {
        this.size = value;
        this.updateAnnotationSizes();
      }
    }

    public int Top
    {
      get => this.location.Y;
      set
      {
        this.location.Y = value;
        this.updateAnnotationLocations();
      }
    }

    public int Width
    {
      get => this.size.Width;
      set
      {
        this.size.Width = value;
        this.updateAnnotationSizes();
      }
    }

    private void updateAnnotationLocations()
    {
      float num1 = Convert.ToSingle(this.size.Width) / (float) this.page.Width;
      float num2 = Convert.ToSingle(this.size.Height) / (float) this.page.Height;
      foreach (VirtualPageAnnotation virtualAnnotation in this.virtualAnnotationList)
      {
        int x = this.location.X + Convert.ToInt32((float) virtualAnnotation.Annotation.Left * num1);
        int y = this.location.Y + Convert.ToInt32((float) virtualAnnotation.Annotation.Top * num2);
        virtualAnnotation.Location = new Point(x, y);
      }
    }

    private void updateAnnotationSizes()
    {
      float num1 = Convert.ToSingle(this.size.Width) / (float) this.page.Width;
      float num2 = Convert.ToSingle(this.size.Height) / (float) this.page.Height;
      foreach (VirtualPageAnnotation virtualAnnotation in this.virtualAnnotationList)
      {
        int int32_1 = Convert.ToInt32((float) virtualAnnotation.Annotation.Width * num1);
        int int32_2 = Convert.ToInt32((float) virtualAnnotation.Annotation.Height * num2);
        virtualAnnotation.Size = new Size(int32_1, int32_2);
      }
    }

    public VirtualPageAnnotation GetAnnotationAtPoint(int x, int y)
    {
      foreach (VirtualPageAnnotation virtualAnnotation in this.virtualAnnotationList)
      {
        if (virtualAnnotation.Bounds.Contains(x, y))
          return virtualAnnotation;
      }
      return (VirtualPageAnnotation) null;
    }
  }
}
