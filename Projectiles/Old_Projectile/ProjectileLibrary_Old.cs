using System.Collections.Generic;
using Godot;

public partial class ProjectileLibrary_Old : Node
{
	[Export] private string _trajectoryPath = "res://Projectiles/Old_Projectile/ProjectileTrajectory";
	[Export] private string _collisionPath = "res://Projectiles/Old_Projectile/ProjectileCollision";
	private Godot.Collections.Dictionary<string, PackedScene> _trajectories;
	private Godot.Collections.Dictionary<string, PackedScene> _collisions;
	
	public override void _Ready()
	{
		_trajectories = LoadPackedSceneData(_trajectoryPath);
		_collisions = LoadPackedSceneData(_collisionPath);
	}

	private Godot.Collections.Dictionary<string, PackedScene> LoadPackedSceneData(string path)
	{
		var dictionary = new Godot.Collections.Dictionary<string, PackedScene>();
		var directory = DirAccess.Open(path);
		directory.ListDirBegin();
		while (true)
		{
			string fileName = directory.GetNext();
			if (fileName == "")
			{
				break;
			}
			if(fileName.EndsWith(".tscn"))
			{
				var trajectory = ResourceLoader.Load<PackedScene>(path + "/" + fileName);
				dictionary.Add(fileName[..^5], trajectory);
				GD.Print("Loaded trajectory: " + fileName);
			}
		}
		return dictionary;
	}

	public PackedScene GetTrajectoryScene(string trajectoryName)
	{
		return _trajectories[trajectoryName];
	}
	
	public PackedScene GetCollisionScene(string collisionName)
	{
		return _collisions[collisionName];
	}
	
	public ICollection<string> GetTrajectoryIds() => _trajectories.Keys;
	public ICollection<string> GetCollisionIds() => _collisions.Keys;
}
