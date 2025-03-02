using Godot;
using Godot.Collections;

public partial class ProjectileLibrary : Node
{
	[Export] private string _trajectoryPath = "res://Projectiles/ProjectileTrajectory";
	private Dictionary<string, PackedScene> _trajectories;
	
	public override void _Ready()
	{
		LoadTrajectories();
	}

	private void LoadTrajectories()
	{
		_trajectories = new Dictionary<string, PackedScene>();
		var directory = DirAccess.Open(_trajectoryPath);
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
				var trajectory = ResourceLoader.Load<PackedScene>(_trajectoryPath + "/" + fileName);
				_trajectories.Add(fileName[..^5], trajectory);
				GD.Print("Loaded trajectory: " + fileName);
			}

		}
	}

	public PackedScene GetTrajectoryScene(string trajectoryName)
	{
		return _trajectories[trajectoryName];
	}
}
