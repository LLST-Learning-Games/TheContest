using Godot;
using System;
using Godot.Collections;

public partial class EnemyWaveSpawner : Node2D
{
    [Export] private CollisionShape2D _area;
    [Export] private Timer _timer;
    [Export] private PackedScene _enemyPrefab;
    
    private Array<Enemy> _enemies = new ();
    private RandomNumberGenerator _rng = new RandomNumberGenerator();

    public override void _Ready()
    {
        _timer.Timeout += OnSpawnTimer;
    }

    private void OnSpawnTimer()
    {
        Vector2 spawnPoint = GetRandomPointInArea();
        var newEnemy = _enemyPrefab.Instantiate<Enemy>();
        AddChild(newEnemy);
        newEnemy.Position = spawnPoint;
        _enemies.Add(newEnemy);
    }

    private Vector2 GetRandomPointInArea()
    {
        var rect = _area.Shape.GetRect();
        var x = _rng.RandfRange(rect.Position.X, rect.Position.X + rect.Size.X);
        var y = _rng.RandfRange(rect.Position.Y, rect.Position.Y + rect.Size.Y);
        return new Vector2(x, y);
    }
}
