using Godot;
using System;
using System.Diagnostics;
using Godot.Collections;

public partial class EnemyWaveSpawner : Node2D
{
    [Export] private CollisionShape2D _area;
    [Export] private Timer _timer;
    [Export] private PackedScene _enemyPrefab;
    [Export] private int _maxEnemies = 6;
    
    private Array<Enemy> _enemies = new ();
    private RandomNumberGenerator _rng = new RandomNumberGenerator();

    public override void _Ready()
    {
        _timer.Timeout += OnSpawnTimer;
        OnSpawnTimer();
    }

    private void OnSpawnTimer()
    {
        if (_enemies.Count >= _maxEnemies)
        {
            return;
        }
        Vector2 spawnPoint = GetRandomPointInArea();
        var newEnemy = _enemyPrefab.Instantiate<Enemy>();
        _area.AddChild(newEnemy);
        newEnemy.Position = spawnPoint;
        newEnemy.OnDeath += OnDeath;
        _enemies.Add(newEnemy);
    }

    private void OnDeath(Enemy deadEnemy)
    {
        _enemies.Remove(deadEnemy);
    }

    private Vector2 GetRandomPointInArea()
    {
        var rect = _area.Shape.GetRect();
        var x = _rng.RandfRange(rect.Position.X, rect.End.X);
        var y = _rng.RandfRange(rect.Position.Y, rect.End.Y);
        return new Vector2(x, y);
    }
}
