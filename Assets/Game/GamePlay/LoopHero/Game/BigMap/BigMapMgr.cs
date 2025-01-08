using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityToolkit;
using Random = System.Random;

namespace Game.LoopHero
{
    public class BigMapMgr : LoopHeroModuleMgr<BigMapMgr>
    {
        public Tilemap tileMap;
        public Tile drawTile;
        public Tile bgTile;

        int mapHeight = 12;
        int mapWidth = 18;

        int mapStartX = -9;
        int mapStartY = -6;

        List<int> OptionOfPath = new List<int> { 18,20,22,24};

        private Vector2Int start = new Vector2Int(0, 5);
        private Vector2Int end = new Vector2Int(0, 4);

        private int pathBound = 6;// 在中心6*6区域生成

        private void OnEnable()
        {
            GetComponent<CinemachineCamera>().enabled = true;
            InitTile();
            InitMap();
        }

        private void OnDisable()
        {
            GetComponent<CinemachineCamera>().enabled = false;
        }

        private void InitTile()
        {
          //  drawTile = assetd
        }

        private void InitMap()
        {
            // 填入地图
            for (int i = 0;i < mapHeight; i++)
            {
                for(int j = 0;j < mapWidth; j++)
                {
                    tileMap.SetTile(new Vector3Int(j+mapStartX, i+mapStartY, 0), bgTile);
                }
            }

            // 填入路径
            Random random = new Random();
            var length  = OptionOfPath[ random.Next(OptionOfPath.Count)];
            InitPath(length);
        }


        // 生成（0，5）为起点-》，第一步必定是（1，5），并且以（0，4为终点）的，行动范围必定在（0，0）*（5，5）以内的定长轨道
        // 而且不走回头路，不走并行路
        private void InitPath(int pathLength)
        {
            List<Vector2Int> path = InitPathData(pathLength);
            if (path == null) return;
            foreach(var pos in path)
            {
                tileMap.SetTile(new Vector3Int(pos.x -pathBound/2, pos.y- pathLength/2, 0), drawTile);
            }
            
        }

        List<Vector2Int> InitPathData(int pathLength)
        {
            List<Vector2Int> path = new List<Vector2Int> { start };
            HashSet<Vector2Int> visited = new HashSet<Vector2Int> { start };

            // Start the path with the first move to (1, 5)
            path.Add(start + new Vector2Int(1,0));
            visited.Add(start + new Vector2Int(1, 0));
            path.Add(start + new Vector2Int(2, 0));
            visited.Add(start + new Vector2Int(2, 0)); // 第二步也不可能是向下，这样会和回去的路连通

            List<Vector2Int> finalPath = FindRandomPath(start + new Vector2Int(2, 0), end, path, visited, pathLength);

            return finalPath;
        }




        private List<Vector2Int> FindRandomPath(Vector2Int current, Vector2Int end, List<Vector2Int> path, HashSet<Vector2Int> visited, int length)
        {
            if (path.Count == length )
            {
                if (current == end)
                {
                    return new List<Vector2Int>(path);
                }
                else
                {
                    return null;
                }
            }

            List<Vector2Int> moves = new List<Vector2Int>
        {
            new Vector2Int(current.x + 1, current.y),
            new Vector2Int(current.x - 1, current.y),
            new Vector2Int(current.x, current.y + 1),
            new Vector2Int(current.x, current.y - 1)
        };

            Shuffle(moves);

            foreach (var move in moves)
            {
                if (IsValidMove(path ,move, visited))
                {
                    path.Add(move);
                    visited.Add(move);

                    List<Vector2Int> result = FindRandomPath(move, end, path, visited, length);
                    if (result != null)
                    {
                        return result;
                    }

                    path.RemoveAt(path.Count - 1);
                    visited.Remove(move);
                }
            }

            return null;
        }

        private bool IsValidMove(List<Vector2Int> path,Vector2Int position, HashSet<Vector2Int> visited)
        {
            var inBox =  position.x >= 0 && position.x < pathBound && position.y >= 0 && position.y < pathBound && !visited.Contains(position);
            if (path.Count > 3 && position != end)
            {
               
                var checkConnected = true;
                for (var i = 0;i< path.Count -1 ;i++)
                {
                    if (AreAdjacent(position, path[i]))
                    {
                        return false ;
                    }
                    
                }
                
                return checkConnected && inBox;
            }
            else
            {
                return inBox;
            }
            
        }

        private void Shuffle<T>(List<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                Random random = new Random();
                int randomIndex = random.Next(i, list.Count);
                T temp = list[i];
                list[i] = list[randomIndex];
                list[randomIndex] = temp;
            }
        }

        private bool AreAdjacent(Vector2Int point1, Vector2Int point2)
        {
            int dx = Mathf.Abs(point1.x - point2.x);
            int dy = Mathf.Abs(point1.y - point2.y);

            return (dx == 1 && dy == 0) || (dx == 0 && dy == 1);
        }

        public override void OnUpdate()
        {
            
        }
    }
}