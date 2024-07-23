using System;
using Godot;

public partial class Root : Node2D
{
	const int CELLSCALE = 6;
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
		var viewSize = GetViewport().GetVisibleRect().Size;
		Vector2 gridSize = new Vector2(viewSize.X / CELLSCALE, viewSize.Y / CELLSCALE);
		var screenrect = new Rect2(new Vector2(0, 0), new Vector2(viewSize.X, viewSize.Y));
		DrawRect(screenrect, inActiveColor);

		var workingContainer = new Cell[(int)gridSize.X, (int)gridSize.Y];

		for (int x = 0; x < gridSize.X; x++)
		{
			for (int y = 0; y < gridSize.Y; y++)
			{
				var ws = workingContainer[x, y] = new Cell
				{
					Active = mainContainer[x, y].Active,
					Age = mainContainer[x, y].Age,
				};

				// Check adjacent counts
				var adjacentCount = 0;
				for (int cellx = x - 1; cellx <= x + 1; cellx++)
				{
					for (int celly = y - 1; celly <= y + 1; celly++)
					{
						// Ignore own cell
						if (!(cellx == x && celly == y))
						{
							// Establish if edge
							int cx = cellx < 0 ? (int)gridSize.X - 1 : cellx >= (int)gridSize.X ? 0 : cellx;
							int cy = celly < 0 ? (int)gridSize.Y - 1 : celly >= (int)gridSize.Y ? 0 : celly;

							adjacentCount += mainContainer[cx, cy].Active ? 1 : 0;
						}
					}
				}

				if (mainContainer[x, y].Active && (adjacentCount == 2 || adjacentCount == 3))
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
				float ageCol = ws.Age / 100;
				var outputCol = ws.Active ? new Color(seedColActive, ageCol, ageCol) : new Color(seedColInActive, ageCol, ageCol);
				DrawRect(rect, outputCol);
			}
		}

		mainContainer = workingContainer;
	}

	public override void _Ready()
	{
		var viewSize = GetViewport().GetVisibleRect().Size;
		Vector2 gridSize = new Vector2(viewSize.X / CELLSCALE, viewSize.Y / CELLSCALE);
		mainContainer = new Cell[(int)gridSize.X, (int)gridSize.Y];
		Random rnd1 = new Random();
		for (int x = 0; x < gridSize.X; x++)
		{
			for (int y = 0; y < gridSize.Y; y++)
			{
				mainContainer[x, y] = new Cell { Active = rnd1.Next() % 2 == 1 };
			}
		}
	}
	
	public override void _Process(double delta)
	{
		Framecount++;
		if (FRAMESKIP == 0)
			QueueRedraw();
		else if (Framecount % FRAMESKIP == 0)
			QueueRedraw();
	}
}
