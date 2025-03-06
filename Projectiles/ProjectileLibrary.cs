using Godot;
using Godot.Collections;

public partial class ProjectileLibrary : Node
{
	[Export] private string _trajectoryPath = "res://Projectiles/ProjectileTrajectory";
	[Export] private string _collisionPath = "res://Projectiles/ProjectileCollision";
	private Dictionary<string, PackedScene> _trajectories;
	private Dictionary<string, PackedScene> _collisions;
	
	public override void _Ready()
	{
		_trajectories = LoadPackedSceneData(_trajectoryPath);
		_collisions = LoadPackedSceneData(_collisionPath);
	}

	private Dictionary<string, PackedScene> LoadPackedSceneData(string path)
	{
		var dictionary = new Dictionary<string, PackedScene>();
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
}
