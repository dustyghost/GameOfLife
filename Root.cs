using System;
using Godot;

public class Root : Node2D
{
    const int CELLSCALE = 8;
    const int BORDERSIZE = 1;
    const int FRAMESKIP = 4;
    Color inActiveColor = new Color(0, 0, 0);
    Cell[,] mainContainer;
    public int Framecount = 0;

	float seedColActive = 0.5f;
	float seedColInActive = 0.0f;
	float dyingRate = 0.01f;
	float livingRate = 1.00f;

    public override void _Draw()
    {
        var viewSize = GetViewport().Size;
        Vector2 gridSize = new Vector2(viewSize.x / CELLSCALE, viewSize.y / CELLSCALE);
        var screenrect = new Rect2(new Vector2(0, 0), new Vector2(viewSize.x, viewSize.x));
        DrawRect(screenrect, inActiveColor);

        var workingContainer = new Cell[(int)gridSize.x, (int)gridSize.y];

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                var ws = workingContainer[x, y] = new Cell
                {
                    Active = mainContainer[x, y].Active,
                    Age = mainContainer[x, y].Age,
                };

                //Check adjacent counts
                var adjacentCount = 0;
                for (int cellx = (x - 1); cellx <= (x + 1); cellx++)
                {
                    for (int celly = (y - 1); celly <= (y + 1); celly++)
                    {
                        //ignore own cell
                        if (!(cellx == x && celly == y))
                        {
                            //establish if edge
                            var cx = cellx < 0 ? gridSize.x - 1 : cellx > gridSize.x - 1 ? 0 : cellx;
                            var cy = celly < 0 ? gridSize.y - 1 : celly > gridSize.y - 1 ? 0 : celly;

                            adjacentCount += mainContainer[(int)cx, ((int)cy)].Active ? 1 : 0;
                        }
                    }
                }

                if ((mainContainer[x, y].Active && adjacentCount == 2 || adjacentCount == 3))
                {
                    ws.Active = true;
                    ws.Age = (ws.Age + livingRate) < 99 ? (ws.Age + livingRate) : 99;
                }
                else if (!mainContainer[x, y].Active && adjacentCount == 3)
                {
                    ws.Active = true;
                    ws.Age = 1;
                }
                else
                {
                    ws.Active = false;
                    ws.Age = ws.Age < 1 ? 1 : ws.Age - dyingRate;
                }

                var rect = new Rect2(new Vector2(x * CELLSCALE + BORDERSIZE, y * CELLSCALE + BORDERSIZE), new Vector2(CELLSCALE - BORDERSIZE, CELLSCALE - BORDERSIZE));
				float ageCol = (ws.Age / 100);
				var outputCol = ws.Active  ? new Color(seedColActive, ageCol, ageCol) : new Color(seedColInActive, ageCol, ageCol);
                DrawRect(rect, outputCol);
            }
        }

        mainContainer = workingContainer;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var viewSize = GetViewport().Size;
        Vector2 gridSize = new Vector2(viewSize.x / CELLSCALE, viewSize.y / CELLSCALE);
        mainContainer = new Cell[(int)gridSize.x, (int)gridSize.y];
        Random rnd1 = new Random();
        for (int x = 0; x < gridSize.x; x += 1)
        {
            for (int y = 0; y < gridSize.y; y += 1)
            {
                mainContainer[x, y] = new Cell { Active = (rnd1.Next() % 2) == 1 };
            }
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Framecount++;
		if(FRAMESKIP == 0)
			Update();
        else if (Framecount % FRAMESKIP == 0)
            Update();
    }
}