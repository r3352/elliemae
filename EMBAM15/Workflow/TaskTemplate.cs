// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Workflow.TaskTemplate
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using Newtonsoft.Json;

#nullable disable
namespace EllieMae.EMLite.Workflow
{
  public class TaskTemplate
  {
    public TaskTemplate()
    {
    }

    public TaskTemplate(string typeID, string name, string taskGroupTemplateId)
    {
      this.TypeID = typeID;
      this.Name = name;
      this.TaskGroupTemplateId = taskGroupTemplateId;
    }

    [JsonProperty("type")]
    public virtual string TypeID { get; set; }

    public virtual string Name { get; set; }

    public virtual string TaskGroupTemplateId { get; set; }
  }
}
