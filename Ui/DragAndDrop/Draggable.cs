using Godot;
using System;
using Systems;

public partial class Draggable : ColorRect
{
    private const string IS_EMPTY = "empty";
    [Export] internal string _projectileId = "YellowMagic";
    [Export] private TextureRect _textureRect;

    public string ProjectileId => _projectileId;
    public bool IsDraggableSource = false;
    private bool _isDragging = false;
        
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
        var projectile = _library.GetAnyResource(_projectileId);
        if(projectile is null)
        {
            return;
        }

        var texture = projectile.Icon;
        Color = Colors.White;
        _textureRect.Texture = texture;
    }

    public override Variant _GetDragData(Vector2 atPosition)
    {
        if (_projectileId == IS_EMPTY)
        {
            return IS_EMPTY;
        }
        SetDragPreview(GetDragPreview());
        Color = Colors.Gray;
        Input.MouseMode = Input.MouseModeEnum.Hidden;
        _isDragging = true;
        return this;
    }

    public Control GetDragPreview()
    {
        var preview = Duplicate() as Control;
        preview.SetAsTopLevel(true);
        preview.ZIndex = 420;
        return preview;
    }


    public override void _Notification(int what)
    {
        if (what == NotificationDragEnd)
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
            if(_isDragging && !IsDragSuccessful())
            {
                if(IsDraggableSource)
                {
                    Color = Colors.White;
                }
                else
                {
                    SetIsEmpty();
                }
            }
            _isDragging = false;
        }
    }

    private void SetIsEmpty()
    {
        _projectileId = IS_EMPTY;
        LookupProjectileData();
    }
    
    public override void _DropData(Vector2 atPosition, Variant data)
    {
        var otherDraggable = data.AsGodotObject() as Draggable;
        if (otherDraggable is null)
        {
            return;
        }

        if (otherDraggable == this)
        {
            Color = Colors.White;
            return;
        }
        if(!IsDraggableSource && otherDraggable._projectileId != IS_EMPTY)
        {
            _projectileId = otherDraggable._projectileId;
        }
        if(!otherDraggable.IsDraggableSource)
        {
            otherDraggable._projectileId = IS_EMPTY;
        }
        otherDraggable.LookupProjectileData();
        LookupProjectileData();
    }

    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
        return data.AsGodotObject() is Draggable;
    }
    
    public override void _GuiInput(InputEvent @event)
    {
        if (!IsDraggableSource &&
            @event is InputEventMouseButton mouseButtonEvent &&
            mouseButtonEvent.Pressed &&
            mouseButtonEvent.ButtonIndex == MouseButton.Right)
        {
            SetIsEmpty();
        }
    }
    
}
 