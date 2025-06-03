
// 위치 계산의 용이성을 위해 int type vector 정의
public struct IntVector2
{
    public int X, Y;

    public IntVector2(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static IntVector2 operator +(IntVector2 a, IntVector2 b)
    {
        return new IntVector2(a.X + b.X, a.Y + b.Y);
    }
}

// 알고리즘의 수행에 필요한 노드 정의
public class Node
{
    public IntVector2 Pos;
    public double G, H;
    public double F => G + H;
    public Node Parent;

    public Node(IntVector2 pos)
    {
        Pos = pos;
    }
}

public class AStar
{
    // 직선 1칸 이동 코스트 정의
    static float ORTHOGONAL_DISTANCE = 1.0f;
    // 대각 1칸 이동 코스트 정의
    static float DIAGONAL_DISTANCE = 1.41421356f;

    static readonly IntVector2[] directions =
    {
        new IntVector2(1, 0),  // e
        new IntVector2(1, 1),  // ne
        new IntVector2(0, 1),  // n
        new IntVector2(-1, 1), // nw
        new IntVector2(-1, 0), // w
        new IntVector2(-1, -1),// sw
        new IntVector2(0, -1), // s
        new IntVector2(1, -1), // se
    };
    public static List<Node> FindPath(IntVector2 start, IntVector2 end, bool[,] map)
    {
        int width = map.GetLength(1);
        int height = map.GetLength(0);

        var open = new List<Node> { new Node(start) };
        var closed = new List<IntVector2>();

        while (open.Count > 0)
        {
            // open 리스트에서 가장 F값이 높은 노드 선택
            open.Sort((a, b) => a.F.CompareTo(b.F));
            var current = open[0];

            // 선택된 노드를 open 리스트에서 제거
            open.RemoveAt(0);

            // 종료지점에 도달했으면 이 노드로부터 경로를 구성하고 알고리즘 종료 
            if (current.Pos.X == end.X && current.Pos.Y == end.Y)
                return ReconstructPath(current);

            // 이전 루프에서 판단
            closed.Add(current.Pos);

            foreach (var dir in directions)
            {
                var neighborPos = current.Pos + dir;

                // 이 방향이 정의된 범위 이내인가?
                if (neighborPos.X < 0 || neighborPos.Y < 0 || neighborPos.X >= width || neighborPos.Y >= height)
                    continue;
                // 이 방향이 이동 가능한가?
                if (!map[neighborPos.Y, neighborPos.X])
                    continue;
                // 가려는 곳이 이미 close 리스트에 있는 지점인가?
                if (IsInList(closed, neighborPos))
                    continue;

                // 노드의 상대 위치에 따른 이동 코스트 합산
                double moveCost = (dir.X == 0 || dir.Y == 0) ? ORTHOGONAL_DISTANCE : DIAGONAL_DISTANCE;
                double tentativeG = current.G + moveCost;

                // openList 에 해당 방향의 노드가 이미 있는지
                var existing = FindNode(open, neighborPos);
                if (existing == null)
                {
                    // 없으면 휴리스틱을 채우고
                    var neighbor = new Node(neighborPos)
                    {
                        G = tentativeG,
                        H = GetEuclideanDistance(neighborPos, end), // 목표지점까지의 직선거리
                        Parent = current
                    };
                    // openList 에 삽입
                    open.Add(neighbor);
                }
                else if (tentativeG < existing.G)
                {
                    existing.G = tentativeG;
                    existing.Parent = current;
                }
            }
        }

        return null;
    }

    // 위치 객체 리스트에서 같은 위치를 포함하고 있는지 
    private static bool IsInList(List<IntVector2> list, IntVector2 pos)
    {
        return list.Exists(x => x.X == pos.X && x.Y == pos.Y);
    }

    // 노드 리스트에서 같은 위치의 노드를 찾기
    private static Node FindNode(List<Node> list, IntVector2 pos)
    {
        return list.Find(x => x.Pos.X == pos.X && x.Pos.Y == pos.Y);
    }

    // 직선거리 유틸리티
    private static double GetEuclideanDistance(IntVector2 a, IntVector2 b)
    {
        int dx = a.X - b.X;
        int dy = a.Y - b.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    // 노드의 Parent 참조 체인을 통해 경로 구성 ( 시작 -> 종료 순으로 순회 할 수 있도록 )
    private static List<Node> ReconstructPath(Node end)
    {
        var path = new List<Node>();
        while (end != null)
        {
            path.Add(end);
            end = end.Parent;
        }
        path.Reverse();
        return path;
    }
}
