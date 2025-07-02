using System.Collections.Generic;
using System.Linq;
using Godot;
using Systems;
using TheContest.Projectiles;

public partial class ProjectileLibrary : BaseSystem
{
	[Export] private NeuroPulseFactory _factory;
	[Export] private string _trajectoryPath = "res://Projectiles/Segments/TrajectorySegment";
	[Export] private string _collisionPath = "res://Projectiles/Segments/CollisionSegment";
	private Godot.Collections.Dictionary<string, ProjectileSegmentData> _trajectories;
	private Godot.Collections.Dictionary<string, ProjectileSegmentData> _collisions;
	private HashSet<string> _unlockedSegments = new();
	
	[Export] public NeuroPulse PlayerPulse { get; private set; }
	
	public NeuroPulseFactory Factory => _factory;
	
	public override void Initialize()
	{
		_trajectories = LoadData(_trajectoryPath);
		_collisions = LoadData(_collisionPath);
		_factory.Initialize(this);
	}

	public override void OnGameplayEnd()
	{
		//..
	}

	public void SetPlayerPulse(NeuroPulse playerPulse)
	{
		PlayerPulse?.QueueFree();
		PlayerPulse = playerPulse;
		AddChild(PlayerPulse);
	}

	private Godot.Collections.Dictionary<string, ProjectileSegmentData> LoadData(string path)
	{
		var dictionary = new Godot.Collections.Dictionary<string, ProjectileSegmentData>();
		var directory = DirAccess.Open(path);
		directory.ListDirBegin();
		while (true)
		{
			string fileName = directory.GetNext();
			if (fileName == "")
			{
				break;
			}
			if(fileName.EndsWith(".tres"))
			{
				var resource = ResourceLoader.Load<ProjectileSegmentData>(path + "/" + fileName);
				if (!resource.IncludeInLibrary)
				{
					GD.Print($"[{GetType().Name}] Trajectory {resource.Id} marked for exclusion from library.");
					continue;
				}
				dictionary.Add(resource.Id, resource);
				if (resource.StartUnlocked)
				{
					_unlockedSegments.Add(resource.Id);
				}
				
				GD.Print($"[{GetType().Name}] Loaded trajectory: " + fileName);
			}
		}
		return dictionary;
	}

	public ProjectileSegmentData GetTrajectoryResource(string trajectoryName)
	{
		if (!_trajectories.ContainsKey(trajectoryName))
		{
			GD.Print($"[{GetType().Name}] Resource not found: {trajectoryName}");
			return null;
		}
		return _trajectories[trajectoryName];
	}
	
	public ProjectileSegmentData GetCollisionResource(string collisionName)
	{
		if (!_collisions.ContainsKey(collisionName))
		{
			GD.Print($"[{GetType().Name}] Resource not found: {collisionName}");
			return null;
		}
		return _collisions[collisionName];
	}

	public ProjectileSegmentData GetAnyResource(string projectileName)
	{
		var data = GetTrajectoryResource(projectileName);
		if (data != null)
		{
			return data;
		}
		return GetCollisionResource(projectileName);
	}
	
	
	public ProjectileSegmentData GetUnlockedResource(string projectileName)
	{
		var data = GetTrajectoryResource(projectileName);
		if (data != null)
		{
			return data;
		}
		return GetCollisionResource(projectileName);
	}
	
	public void UnlockPulseId(string id) => _unlockedSegments.Add(id);
	
	public ICollection<string> GetTrajectoryIds() => _trajectories.Keys;
	public ICollection<string> GetCollisionIds() => _collisions.Keys;
	
	public IEnumerable<string> GetAllPulseIds() => GetTrajectoryIds().Concat(GetCollisionIds());

	public List<string> GetAllUnlockableIds()
	{
		var unlockableIds = new List<string>();
		foreach (var data in _trajectories.Values)
		{
			if (data.Unlockable && !_unlockedSegments.Contains(data.Id))
			{
				unlockableIds.Add(data.Id);
			}
		}
		
		foreach (var data in _collisions.Values)
		{
			if (data.Unlockable && !_unlockedSegments.Contains(data.Id))
			{
				unlockableIds.Add(data.Id);
			}
		}

		return unlockableIds;
	}
	public List<string> GetAllUnlockedPulseIds() => _unlockedSegments.ToList();
	
	

}
