using System;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactManager : Singleton<ArtifactManager>
{
	[SerializeField] private List<ArtifactInfo> infos;

	private readonly Dictionary<ArtifactId, ArtifactInfo> infoMap = new();

	private void Start()
	{
		foreach (ArtifactInfo info in infos)
			infoMap[info.id] = info;
	}

	public ArtifactInfo GetInfo(ArtifactId artifactId)											// 유물의 ID를 받아서 해당 유물의 정보를 반환하는 메서드
	{
		return infoMap[artifactId];
	}
}

[Serializable]
public class ArtifactInfo			// 유물 정보 클래스
{
	public ArtifactId id;
	public Sprite image;
	public string name;
	public string desc;
}
