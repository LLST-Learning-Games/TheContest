using System.Collections.Generic;
using System.Linq;
using Godot;
using Systems;
using TheContest.Projectiles;

public partial class ProjectileLibrary : BaseSystem
{
	[Export] private string _trajectoryPath = "res://Projectiles/Segments/TrajectorySegment";
	[Export] private string _collisionPath = "res://Projectiles/Segments/CollisionSegment";
	private Godot.Collections.Dictionary<string, ProjectileSegmentData> _trajectories;
	private Godot.Collections.Dictionary<string, ProjectileSegmentData> _collisions;
	
	public override void Initialize()
	{
		_trajectories = LoadData(_trajectoryPath);
		_collisions = LoadData(_collisionPath);
	}

	public override void OnGameplayEnd()
	{
		//..
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
				dictionary.Add(resource.Id, resource);
				GD.Print("Loaded trajectory: " + fileName);
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

	public ProjectileSegmentData GetResource(string projectileName)
	{
		var data = GetTrajectoryResource(projectileName);
		if (data != null)
		{
			return data;
		}
		return GetCollisionResource(projectileName);
	}
	
	public ICollection<string> GetTrajectoryIds() => _trajectories.Keys;
	public ICollection<string> GetCollisionIds() => _collisions.Keys;
	
	public IEnumerable<string> GetAllPulseIds() => GetTrajectoryIds().Concat(GetCollisionIds());

}
