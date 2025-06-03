
bool[,] map = new bool[10, 10]
{
    { true,  true,  true,  true,  true,  true,  true,  true,  true,  true },
    { true,  false, false, false, true,  true,  true,  false, false, true },
    { true,  true,  true,  false, true,  false, true,  true,  false, true },
    { true,  false, true,  false, true,  false, true,  false, false, true },
    { true,  false, true,  true,  true,  false, true,  false,  true,  true },
    { true,  false, false, false, true, false, false, false, false, true },
    { true,  true,  true,  true,  true,  false, true,  true,  true,  true },
    { true,  false, false, false, true,  false, true,  false, false, true },
    { true,  true,  true,  false, true,  true,  true,  true, true,  true },
    { true,  true,  true,  true,  true,  true,  true,  false,  true,  true }
};

var start = new IntVector2(0, 0);
var end = new IntVector2(9, 9);

// 맵을 입력하고 경로 계산
var path = AStar.FindPath(start, end, map);

int height = map.GetLength(0);
int width = map.GetLength(1);

for (int y = 0; y < height; y++)
{
    for (int x = 0; x < width; x++)
    {
        // 그리드상 위치의 상태에 따라 렌더링
        if (path.Exists(node => node.Pos.X == x && node.Pos.Y == y))
            Console.Write(" + ");
        else if (!map[y, x])
            Console.Write("[@]");
        else
            Console.Write("   ");
    }
    Console.WriteLine();
}