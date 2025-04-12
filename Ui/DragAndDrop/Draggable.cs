using System;
using Godot;
using Systems;
using Systems.Currency;
using TheContest.Projectiles;

public partial class Draggable : ColorRect
{
    public const string IS_EMPTY = "empty";
    [Export] internal string _projectileId = "YellowMagic";
    [Export] private TextureRect _textureRect;
    [Export] private Label _descriptionLabel;

    public string ProjectileId => _projectileId;
    public ProjectileSegmentData Data { get; private set; } 
    public bool IsDraggableSource = false;
    public bool IsPulseTarget = false;
    private bool _isDragging = false;
        
    private ProjectileLibrary _library => SystemLoader.GetSystem<ProjectileLibrary>();

    public void SetDescriptionLabel(Label label) => _descriptionLabel = label;
    
    public override void _Ready()
    {
        if (SystemLoader.IsSystemLoadComplete)
        {
            Initialize();
        }
        else
        {
            SystemLoader.OnSystemLoadComplete += Initialize;
        }
    }
    
    public void InitializeId(string id)
    {
        _projectileId = id;
        InitializeProjectileData();
    }

    private void Initialize()
    {
        InitializeProjectileData();
        MouseEntered += OnMouseEntered;
        MouseExited += OnMouseExited;
    }
    
    private void InitializeProjectileData()
    {
        if (_projectileId == IS_EMPTY)
        {
            Data = null;
            _textureRect.Visible = false;
            Color = Colors.Gray;
            return;
        }
        _textureRect.Visible = true;
        Data = _library.GetAnyResource(_projectileId);
        if(Data is null)
        {
            return;
        }

        var texture = Data.Icon;
        Color = Colors.White;
        _textureRect.Texture = texture;
    }

    private bool TryUpdateProjectileData(string newId)
    {
        if (newId == _projectileId)
        {
            return false;
        }
        
        _projectileId = newId;
        
        float updatedCost = Data?.Cost ?? 0;
        
        if (newId == IS_EMPTY)
        {
            _projectileId = newId;
            Data = null;
            _textureRect.Visible = false;
            Color = Colors.Gray;
            UpdateCash(updatedCost);
            return true;
        }
        
        var newCandidateData = _library.GetAnyResource(newId);
        if (!CanAfford(newCandidateData.Cost - updatedCost))
        {
            return false;
        }
        
        _projectileId = newId;
        _textureRect.Visible = true;
        Data = newCandidateData;
        
        if(Data is null)
        {
            return false;
        }

        var texture = Data.Icon;
        Color = Colors.White;
        _textureRect.Texture = texture;
        
        updatedCost -= Data?.Cost ?? 0;
        
        UpdateCash(updatedCost);
        return true;
    }
    
    private void UpdateCash(float amount)
    {
        var currencySystem = SystemLoader.GetSystem<CurrencySystem>();
        var currency = currencySystem.GetCurrency("cash");
        currency.UpdateCurrencyByDelta(amount);
    }

    private bool CanAfford(float amount)
    {
        var currencySystem = SystemLoader.GetSystem<CurrencySystem>();
        var currency = currencySystem.GetCurrency("cash");
        return currency.Balance >= amount;
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
                _descriptionLabel.Text = "";
            }
            _isDragging = false;
        }
    }

    private void SetIsEmpty()
    {
        TryUpdateProjectileData(IS_EMPTY);
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
            TryUpdateProjectileData(otherDraggable._projectileId);
        }
        if(!otherDraggable.IsDraggableSource)
        {
            otherDraggable.TryUpdateProjectileData(IS_EMPTY);
        }
        else
        {
            otherDraggable.Color = Colors.White; 
        }
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

    private void OnMouseEntered()
    {
        if (Data is null)
        {
            return;
        }
        
        _descriptionLabel.Text = Data.GetDescription();
    }

    private void OnMouseExited()
    {
        if (_isDragging)
        {
            return;
        }
        
        _descriptionLabel.Text = "";  
    }
    
}
 