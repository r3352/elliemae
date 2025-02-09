// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.AttachmentList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Loans;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  public class AttachmentList : ListBase, IAttachmentList
  {
    public AttachmentList()
      : base(typeof (Attachment))
    {
    }

    public AttachmentList(IList source)
      : base(typeof (Attachment), source)
    {
    }

    public Attachment this[int index]
    {
      get => (Attachment) this.List[index];
      set => this.List[index] = (object) value;
    }

    public void Add(Attachment value) => this.List.Add((object) value);

    public bool Contains(Attachment value) => this.List.Contains((object) value);

    public int IndexOf(Attachment value) => this.List.IndexOf((object) value);

    public void Insert(int index, Attachment value) => this.List.Insert(index, (object) value);

    public void Remove(Attachment value) => this.List.Remove((object) value);

    public Attachment[] ToArray()
    {
      Attachment[] array = new Attachment[this.List.Count];
      this.List.CopyTo((Array) array, 0);
      return array;
    }
  }
}
