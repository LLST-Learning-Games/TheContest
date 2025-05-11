using Godot;
using Godot.Collections;

public partial class EnemyWaveSpawner : Node2D
{
    [Export] private CollisionShape2D _spawnArea;
    [Export] private Timer _timer;
    [Export] private PackedScene _enemyPrefab;
    [Export] private int _maxEnemies = 6;
    [Export] private float _spawnDistance = 500f;
    
    private Array<Enemy> _enemies = new ();
    private RandomNumberGenerator _rng = new RandomNumberGenerator();
    private Character _player;

    public override void _Ready()
    {
        _timer.Timeout += OnSpawnTimer;
        OnSpawnTimer();
        _player = GetCharacter();
    }

    private Character GetCharacter()
    {
        var character = GetTree().GetFirstNodeInGroup("Player");
        return character as Character;
    }

    private void OnSpawnTimer()
    {
        if (_enemies.Count >= _maxEnemies)
        {
            return;
        }

        _player ??= GetCharacter();
        if (IsInstanceValid(_player) && _player.GlobalPosition.DistanceTo(_spawnArea.GlobalPosition) < _spawnDistance)
        {
            return;
        }
        Vector2 spawnPoint = GetRandomPointInArea();
        var newEnemy = _enemyPrefab.Instantiate<Enemy>();
        _spawnArea.AddChild(newEnemy);
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
        var rect = _spawnArea.Shape.GetRect();
        var x = _rng.RandfRange(rect.Position.X, rect.End.X);
        var y = _rng.RandfRange(rect.Position.Y, rect.End.Y);
        return new Vector2(x, y);
    }
}
