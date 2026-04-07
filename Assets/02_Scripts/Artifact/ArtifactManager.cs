using System.Collections.Generic;
using UnityEngine;

public class ArtifactManager
{
	private readonly List<ArtifactId> ownedArtifacts = new();
	private ArtifactController artifactController;

	public IReadOnlyList<ArtifactId> OwnedArtifacts => ownedArtifacts;

	public void AddArtifact(ArtifactId artifactId)
	{
		if (ownedArtifacts.Contains(artifactId))
			return;

		ownedArtifacts.Add(artifactId);
	}

	public bool HasArtifact(ArtifactId artifactId)
	{
		return ownedArtifacts.Contains(artifactId);
	}

	public void RemoveArtifact(ArtifactId artifactId)
	{
		ownedArtifacts.Remove(artifactId);
	}

	public void Init(Battle battle)
	{
		artifactController = new ArtifactController(ownedArtifacts);
		artifactController.Initialize(battle);
	}

	public void Dispose()
	{
		artifactController.Dispose();
	}
}

public class ArtifactController
{
	private readonly IReadOnlyList<ArtifactId> ownedArtifacts;

	public ArtifactController(IReadOnlyList<ArtifactId> ownedArtifacts)
	{
		this.ownedArtifacts = ownedArtifacts;
	}

	public void Initialize(Battle battle)
	{
		for (int i = 0; i < ownedArtifacts.Count; i++)
		{
			Debug.Log($"[ArtifactController] Owned Artifact: {ownedArtifacts[i]}");
		}
	}

	public void Dispose()
	{
	}
}
