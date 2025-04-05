using Godot;
using System;
using Systems;

public partial class Draggable : ColorRect
{
    private const string IS_EMPTY = "empty";
    [Export] internal string _projectileId = "YellowMagic";
    [Export] private TextureRect _textureRect;
    
    private ProjectileLibrary _library => SystemLoader.GetSystem<ProjectileLibrary>();

    public override void _Ready()
    {
        if (SystemLoader.IsSystemLoadComplete)
        {
            LookupProjectileData();
        }
        else
        {
            SystemLoader.OnSystemLoadComplete += LookupProjectileData;
        }
    }
    
    public void SetId(string id)
    {
        _projectileId = id;
        LookupProjectileData();
    }

    private void LookupProjectileData()
    {
        if (_projectileId == IS_EMPTY)
        {
            _textureRect.Visible = false;
            Color = Colors.Gray;
            return;
        }
        _textureRect.Visible = true;
        var projectile = _library.GetTrajectoryResource(_projectileId);
        if(projectile is null)
        {
            return;
        }
        var texture = projectile.SpriteFrames.GetFrameTexture("idle", 0);
        _textureRect.Texture = texture;
    }

    public override Variant _GetDragData(Vector2 atPosition)
    {
        GD.Print("Draggable!");
        SetDragPreview(GetDragPreview());
        Color = Colors.Gray;
        Input.MouseMode = Input.MouseModeEnum.Hidden;
        return this;
    }

    public Control GetDragPreview()
    {
        return this.Duplicate() as Control;
        // var colorRect = new ColorRect();
        // colorRect.Color = this.Color;
        // colorRect.Size = this.Size;
        // return colorRect;
    }

    public override void _Notification(int what)
    {
        if (what == NotificationDragEnd)
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
            if(!IsDragSuccessful())
            {
                Color = Colors.White;
            }
            
        }
    }
    
    public override void _DropData(Vector2 atPosition, Variant data)
    {
        var otherDraggable = data.AsGodotObject() as Draggable;
        _projectileId = otherDraggable._projectileId;
        otherDraggable._projectileId = IS_EMPTY;
        otherDraggable.LookupProjectileData();
        Color = Colors.White;
        LookupProjectileData();
    }

    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
        return data.AsGodotObject() is Draggable;
    }
}
 