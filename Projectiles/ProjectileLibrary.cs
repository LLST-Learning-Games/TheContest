using System.Collections.Generic;
using Godot;
using TheContest.Projectiles;

public partial class ProjectileLibrary : Node
{
	[Export] private string _trajectoryPath = "res://Projectiles/Segments/TrajectorySegment";
	[Export] private string _collisionPath = "res://Projectiles/Segments/CollisionSegment";
	private Godot.Collections.Dictionary<string, Resource> _trajectories;
	private Godot.Collections.Dictionary<string, Resource> _collisions;
	
	public override void _Ready()
	{
		_trajectories = LoadData(_trajectoryPath);
		_collisions = LoadData(_collisionPath);
	}

	private Godot.Collections.Dictionary<string, Resource> LoadData(string path)
	{
		var dictionary = new Godot.Collections.Dictionary<string, Resource>();
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

	public Resource GetTrajectoryResource(string trajectoryName)
	{
		if (!_trajectories.ContainsKey(trajectoryName))
		{
			GD.Print($"[{GetType().Name}] Resource not found: {trajectoryName}");
			return null;
		}
		return _trajectories[trajectoryName];
	}
	
	public Resource GetCollisionResource(string collisionName)
	{
		if (!_collisions.ContainsKey(collisionName))
		{
			GD.Print($"[{GetType().Name}] Resource not found: {collisionName}");
			return null;
		}
		return _collisions[collisionName];
	}
	
	public ICollection<string> GetTrajectoryIds() => _trajectories.Keys;
	public ICollection<string> GetCollisionIds() => _collisions.Keys;
}
