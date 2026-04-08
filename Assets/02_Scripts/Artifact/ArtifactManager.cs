public class ArtifactManager
{
	public ArtifactInfo GetInfo(ArtifactId artifactId)
	{
		return artifactId switch
		{
			ArtifactId.AmmoNecklace => new ArtifactInfo(ArtifactId.AmmoNecklace, "탄약 목걸이"),
			ArtifactId.OvercooledLens => new ArtifactInfo(ArtifactId.OvercooledLens, "과냉각 렌즈"),
			ArtifactId.EntangledVines => new ArtifactInfo(ArtifactId.EntangledVines, "얽혀진 덩굴"),
			_ => null,
		};
	}
}

public class ArtifactInfo
{
	public ArtifactId Id { get; }
	public string KoreanName { get; }

	public ArtifactInfo(ArtifactId id, string koreanName)
	{
		Id = id;
		KoreanName = koreanName;
	}
}
