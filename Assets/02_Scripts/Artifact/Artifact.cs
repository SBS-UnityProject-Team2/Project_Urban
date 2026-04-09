public abstract class Artifact
{
    public abstract ArtifactId Id { get; }
    public abstract string KoreanName { get; }

    public abstract void Init(Actor actor);
}

public enum ArtifactId
{
    AmmoNecklace,
    OvercooledLens,
    EntangledVines,
}