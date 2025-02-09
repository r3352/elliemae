// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.Admin.ConfigResource
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

#nullable disable
namespace Confluent.Kafka.Admin
{
  /// <summary>A class representing resources that have configs.</summary>
  public class ConfigResource
  {
    /// <summary>The resource type (required)</summary>
    public ResourceType Type { get; set; }

    /// <summary>The resource name (required)</summary>
    public string Name { get; set; }

    /// <summary>
    ///     Tests whether this ConfigResource instance is equal to the specified object.
    /// </summary>
    /// <param name="obj">The object to test.</param>
    /// <returns>
    ///     true if obj is a ConfigResource and all properties are equal. false otherwise.
    /// </returns>
    public override bool Equals(object obj)
    {
      if ((object) (obj as ConfigResource) == null)
        return false;
      ConfigResource configResource = (ConfigResource) obj;
      return configResource.Type == this.Type && configResource.Name == this.Name;
    }

    /// <summary>Returns a hash code for this ConfigResource.</summary>
    /// <returns>
    ///     An integer that specifies a hash value for this ConfigResource.
    /// </returns>
    public override int GetHashCode() => this.Type.GetHashCode() * 251 + this.Name.GetHashCode();

    /// <summary>
    ///     Tests whether ConfigResource instance a is equal to ConfigResource instance b.
    /// </summary>
    /// <param name="a">
    ///     The first ConfigResource instance to compare.
    /// </param>
    /// <param name="b">
    ///     The second ConfigResource instance to compare.
    /// </param>
    /// <returns>
    ///     true if ConfigResource instances a and b are equal. false otherwise.
    /// </returns>
    public static bool operator ==(ConfigResource a, ConfigResource b)
    {
      return (object) a == null ? (object) b == null : a.Equals((object) b);
    }

    /// <summary>
    ///     Tests whether ConfigResource instance a is not equal to ConfigResource instance b.
    /// </summary>
    /// <param name="a">
    ///     The first ConfigResource instance to compare.
    /// </param>
    /// <param name="b">
    ///     The second ConfigResource instance to compare.
    /// </param>
    /// <returns>
    ///     true if ConfigResource instances a and b are not equal. false otherwise.
    /// </returns>
    public static bool operator !=(ConfigResource a, ConfigResource b) => !(a == b);

    /// <summary>
    ///     Returns a string representation of the ConfigResource object.
    /// </summary>
    /// <returns>
    ///     A string representation of the ConfigResource object.
    /// </returns>
    public override string ToString()
    {
      return string.Format("[{0}] {1}", (object) this.Type, (object) this.Name);
    }
  }
}
