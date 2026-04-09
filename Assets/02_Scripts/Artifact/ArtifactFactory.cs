public static class ArtifactFactory
{
    public static Artifact Create(ArtifactId artifactId)
    {
        return artifactId switch
        {
            ArtifactId.AmmoNecklace => new AmmoNecklace(),
            ArtifactId.OvercooledLens => new OvercooledLens(),
            ArtifactId.EntangledVines => new EntangledVines(),
            _ => null,
        };
    }
}