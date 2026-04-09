public static class ArtifactManager
{
	public static ArtifactInfo GetInfo(ArtifactId artifactId)											// 유물의 ID를 받아서 해당 유물의 정보를 반환하는 메서드
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

public class ArtifactInfo			// 유물 정보 클래스
{
	public ArtifactId Id { get; }
	public string KoreanName { get; }

	public ArtifactInfo(ArtifactId id, string koreanName)
	{
		Id = id;
		KoreanName = koreanName;
	}
}
