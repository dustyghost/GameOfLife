using System;
using Godot;

public class Root : Node2D
{
    const int gridBoxSize = 4;
    const int boxBorderSize = 1;
    Color activeColor = new Color(1, 0, 0);
    Color inActiveColor = new Color(0, 0, 0);
    Vector2 _screenSize = new Vector2(600, 600);
    bool[,] mainContainer;

	public override void _Draw()
	{
        var screenrect = new Rect2(new Vector2(0, 0), new Vector2(_screenSize.x, _screenSize.y));
        DrawRect(screenrect, inActiveColor);

        for (int x = 0; x < _screenSize.x; x += gridBoxSize)
        {
            for (int y = 0; y < _screenSize.y; y += gridBoxSize)
            {
                var rect = new Rect2(new Vector2(x+boxBorderSize, y+boxBorderSize), new Vector2(gridBoxSize-(boxBorderSize*2), gridBoxSize-(boxBorderSize*2)));
                var outputCol = mainContainer[x, y] ? activeColor : inActiveColor;
                DrawRect(rect, activeColor);
            }
        }
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        mainContainer = new bool[(int)_screenSize.x,(int)_screenSize.y];
        mainContainer[20,20] = true;
        mainContainer[60,20] = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		Update();
	}
}
