using System.Collections.Generic;

public abstract class Artifact
{
    private static readonly List<Artifact> activeArtifacts = new();

    public abstract ArtifactId Id { get; }
    public abstract string KoreanName { get; }

    public abstract void Init(Actor actor);

    public virtual void Dispose(Actor actor)
    {
    }

    public static void InitOwnedArtifacts(Actor owner, IReadOnlyList<ArtifactId> ownedArtifacts)
    {
        activeArtifacts.Clear();

        if (owner == null || ownedArtifacts == null)
            return;

        for (int i = 0; i < ownedArtifacts.Count; i++)
        {
            Artifact artifact = Create(ownedArtifacts[i]);
            if (artifact == null)
                continue;

            artifact.Init(owner);
            activeArtifacts.Add(artifact);
        }
    }

    public static void DisposeAll(Actor owner)
    {
        for (int i = 0; i < activeArtifacts.Count; i++)
            activeArtifacts[i].Dispose(owner);

        activeArtifacts.Clear();
    }

    private static Artifact Create(ArtifactId artifactId)
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

public enum ArtifactId
{
    AmmoNecklace,
    OvercooledLens,
    EntangledVines,
}