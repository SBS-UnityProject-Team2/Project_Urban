using System.Collections.Generic;

public class ArtifactController
{
    private  List<ArtifactId> ownedArtifacts = new();    // 플레이어가 획득한 유물 리스트

    public List<ArtifactId> List => ownedArtifacts;      // 플레이어가 획득한 유물 리스트 반환하는 메서드
        

    public void AddArtifact(ArtifactId artifactId)               // 플레이어가 유물을 획득할때 호출하는 메서드(이벤트 보상)
    {
        if (ownedArtifacts.Contains(artifactId))
            return;

        ownedArtifacts.Add(artifactId);
    }

    public bool HasArtifact(ArtifactId artifactId)               // 플레이어가 특정 유물을 보유하고있는지 확인용 메서드
    {
        return ownedArtifacts.Contains(artifactId);
    }

    public void RemoveArtifact(ArtifactId artifactId)            // 플레이어가 유물을 제거할때 호출하는 메서드
    {
        ownedArtifacts.Remove(artifactId);
    }
}
