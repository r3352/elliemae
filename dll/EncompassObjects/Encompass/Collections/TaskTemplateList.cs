// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.TaskTemplateList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Loans.Templates;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  public class TaskTemplateList : ListBase, ITaskTemplateList
  {
    public TaskTemplateList()
      : base(typeof (TaskTemplate))
    {
    }

    public TaskTemplateList(IList source)
      : base(typeof (TaskTemplate), source)
    {
    }

    public TaskTemplate this[int index]
    {
      get => (TaskTemplate) this.List[index];
      set => this.List[index] = (object) value;
    }

    public void Add(TaskTemplate value) => this.List.Add((object) value);

    public bool Contains(TaskTemplate value) => this.List.Contains((object) value);

    public int IndexOf(TaskTemplate value) => this.List.IndexOf((object) value);

    public void Insert(int index, TaskTemplate value) => this.List.Insert(index, (object) value);

    public void Remove(TaskTemplate value) => this.List.Remove((object) value);

    public TaskTemplate[] ToArray()
    {
      TaskTemplate[] array = new TaskTemplate[this.List.Count];
      this.List.CopyTo((Array) array, 0);
      return array;
    }
  }
}
