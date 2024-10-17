using DataBase;
using UnityEngine;

namespace Map
{
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject[] maps;
        [SerializeField] private GameObject[] objects;
        [SerializeField] private Sprite[] backgroundTiles;

        [SerializeField] private int startX;
        [SerializeField] private int startY;

        private GameObject[,] _blocks;
        private Block[,] _blockData;

        public static MapGenerator Instance;

        private void Awake() => Instance = this;

        private void Start()
        {
            Instantiate(maps[DB.Access.gameData.ChosenPlanet], maps[DB.Access.gameData.ChosenPlanet].transform.position, Quaternion.identity);
            
            _blocks = new GameObject[101, 80];
            _blockData = new Block[101, 80];

            for (int x = startX; x < _blocks.GetLength(0) + startX; x++)
            {
                for (int y = startY; (y - startY) * -1 < _blocks.GetLength(1); y--)
                {
                    if (IsBorderCoordinates(x - startX, (y - startY) * -1))
                    {
                        _blocks[x - startX, (y - startY) * -1] = Instantiate(objects[^1], new Vector3(x, y), Quaternion.identity);
                    }
                    else
                    {
                        _blocks[x - startX, (y - startY) * -1] = Instantiate(objects[ChooseRarityOfBlock(ChooseBlock(x - startX, (y - startY) * -1), (y - startY) * -1)], new Vector3(x, y), Quaternion.identity);
                        _blockData[x - startX, (y - startY) * -1] = _blocks[x - startX, (y - startY) * -1].GetComponent<Block>();
                    }
                }
            }

            SetNeighbourBlocks();
        }

        private void SetNeighbourBlocks()    
        {
            for (int x = 1; x < _blocks.GetLength(0) - 1; x++)
            {
                for (int y = 0; y < _blocks.GetLength(1) - 1; y++)
                {
                    if (_blocks[x, y].TryGetComponent<Block>(out var blockData)) blockData.BlockDisable();

                    if (y == 1 || (y == 0 && IsXOnShelterElevator(x + startX)))
                    {
                        if (y == 0 && IsXOnShelterElevator(x + startX)) blockData.BlockEnable();

                        if ((x == 1 && y == 1) || (x + startX == -2 && y == 0))
                        {
                            blockData.blockNeighbourCache.Add(_blockData[x + 1, y]);
                            blockData.blockNeighbourCache.Add(_blockData[x, y + 1]);
                        }
                        else if ((x == _blocks.GetLength(0) - 1 && y == 1) || (x + startX == 2 && y == 0))
                        {
                            blockData.blockNeighbourCache.Add(_blockData[x - 1, y]);
                            blockData.blockNeighbourCache.Add(_blockData[x, y + 1]);
                        }
                        else if (y == 1) SetDefaultNeighbour(blockData, x, y);
                        else
                        {
                            blockData.blockNeighbourCache.Add(_blockData[x + 1, y]);
                            blockData.blockNeighbourCache.Add(_blockData[x - 1, y]);
                            blockData.blockNeighbourCache.Add(_blockData[x, y + 1]);
                        }
                    }
                    else if (y == _blocks.GetLength(1) - 1) 
                    {
                        if (x == 1)
                        {
                            blockData.blockNeighbourCache.Add(_blockData[x + 1, y]);
                            blockData.blockNeighbourCache.Add(_blockData[x, y - 1]);
                        }
                        else if (x == _blocks.GetLength(0) - 1)
                        {
                            blockData.blockNeighbourCache.Add(_blockData[x - 1, y]);
                            blockData.blockNeighbourCache.Add(_blockData[x, y - 1]);
                        }
                        else
                        {
                            blockData.blockNeighbourCache.Add(_blockData[x + 1, y]);
                            blockData.blockNeighbourCache.Add(_blockData[x - 1, y]);
                            blockData.blockNeighbourCache.Add(_blockData[x, y - 1]);
                        }
                    }
                    else if (x == 1 && y > 0)
                    {
                        blockData.blockNeighbourCache.Add(_blockData[x, y + 1]);
                        blockData.blockNeighbourCache.Add(_blockData[x + 1, y]);
                        blockData.blockNeighbourCache.Add(_blockData[x, y - 1]);
                    }
                    else if (x == _blocks.GetLength(0) - 1 && y > 0)
                    {
                        blockData.blockNeighbourCache.Add(_blockData[x, y + 1]);
                        blockData.blockNeighbourCache.Add(_blockData[x - 1, y]);
                        blockData.blockNeighbourCache.Add(_blockData[x, y - 1]);
                    }
                    else if (y > 0)
                    {
                        SetDefaultNeighbour(blockData, x, y);
                    }
                }
            }
        }

        private void SetDefaultNeighbour(Block blockData, int x, int y)
        {
            blockData.blockNeighbourCache.Add(_blockData[x, y + 1]);
            blockData.blockNeighbourCache.Add(_blockData[x, y - 1]);
            blockData.blockNeighbourCache.Add(_blockData[x + 1, y]);
            blockData.blockNeighbourCache.Add(_blockData[x - 1, y]);
        }

        public Sprite GetBackground() => backgroundTiles[DB.Access.gameData.ChosenPlanet];

        private bool IsBorderCoordinates(int x, int y) 
        {
            if (x == 0 || y == 0 && !IsXOnShelterElevator(x + startX) || x == 100 || y == 79) return true;

            return false;
        }

        private bool IsXOnShelterElevator(int x) 
        {
            if (x == -2 || x == -1 || x == 0 || x == 1 || x == 2) return true;

            return false;
        }

        private int ChooseRarityOfBlock(int blockType, int y) 
        {
            if (blockType == 0) 
            {
                if (Random.Range(0, 40) == 0) return 3;
                if (y >= 3 && Random.Range(0, 100) == 0) return 6;
                if (y >= 5 && Random.Range(0, 250) == 0) return 9;
            }
            else if (blockType == 1)
            {
                if (Random.Range(0, 35) == 0) return 4;
                if (Random.Range(0, 80) == 0) return 7;
                if (Random.Range(0, 200) == 0) return 10;

                if (Random.Range(0, 70) == 0) return 15;
            }
            else if (blockType == 2)
            {
                if (Random.Range(0, 20) == 0) return 5;
                if (Random.Range(0, 60) == 0) return 8;
                if (Random.Range(0, 150) == 0) return 11;

                if (Random.Range(0, 50) == 0) return 16;
            }

            return blockType;
        } 

        private int ChooseBlock(int x, int y)
        {
            if (y <= 5) 
            {
                return 0;
            }

            if (y <= 15 || (y > 50 && y <= 55) || (y > 60 && y <= 62))
            {
                if (x <= 3 || (x > 51 && x <= 70)) return Random.Range(0, 2);
                if (x <= 17 || (x > 25 && x <= 37))
                {
                    if (Random.Range(0, 2) == 0)
                        return 0;
                    return Random.Range(0, 2);
                }
                if (x <= 20 || (x > 70 && x <= 100))
                {
                    if (Random.Range(0, 50) == 0)
                        return 12;
                    if (Random.Range(0, 2) == 0)
                        return 0;
                    return Random.Range(0, 3);
                }
                if (x <= 25)
                {
                    if (Random.Range(0, 30) == 0)
                        return 12;
                    return Random.Range(0, 2);
                }

                if (x <= 51)
                {
                    if (Random.Range(0, 30) == 0)
                        return 12;
                    return Random.Range(0, 2);
                }
            }
            else if (y <= 30)
            {
                if (x <= 3 || (x > 51 && x <= 70))
                {
                    if (Random.Range(0, 20) == 0)
                        return 13;
                    return Random.Range(0, 2);
                }

                if (x <= 17 || (x > 25 && x <= 37))
                {
                    if (Random.Range(0, 3) == 0)
                        return 1;
                    return Random.Range(0, 3);
                }
                if (x <= 20)
                {
                    if (Random.Range(0, 3) == 0)
                        return 0;
                    return Random.Range(0, 3);
                }
                if (x <= 25 || (x > 70 && x <= 100)) return Random.Range(0, 2);
                if (x <= 51) return Random.Range(0, 2);
            }
            else if (y <= 48 || (y > 62 && y <= 65))
            {
                if (x <= 3 || (x > 51 && x <= 70))
                {
                    if (Random.Range(0, 20) == 0)
                        return 13;
                    return Random.Range(0, 2);
                }

                if (x <= 17 || (x > 25 && x <= 37))
                {
                    if (Random.Range(0, 15) == 0)
                        return 12;
                    return Random.Range(1, 3);
                }
                if (x <= 20)
                {
                    if (Random.Range(0, 20) == 0)
                        return 13;
                    if (Random.Range(0, 4) == 0)
                        return Random.Range(1, 3);
                    return Random.Range(0, 3);
                }
                if (x <= 25 || (x > 70 && x <= 100))
                {
                    if (Random.Range(0, 20) == 0)
                        return 14;
                    return Random.Range(0, 3);
                }

                if (x <= 51)
                {
                    if (Random.Range(0, 5) == 0)
                        return 13;
                    return Random.Range(0, 3);
                }
            }
            else if (y <= 75) 
            {
                if (x <= 3 || (x > 51 && x <= 70))
                {
                    if (Random.Range(0, 15) == 0)
                        return 14;
                    return Random.Range(1, 3);
                }

                if (x <= 17 || (x > 25 && x <= 37))
                {
                    if (Random.Range(0, 5) == 0)
                        return 13;
                    if (Random.Range(0, 10) == 0)
                        return Random.Range(1, 3);
                    return Random.Range(0, 3);
                }
                if (x <= 20 || (x > 70 && x <= 100))
                {
                    if (Random.Range(0, 10) == 0)
                        return 14;
                    if (Random.Range(0, 3) == 0)
                        return Random.Range(1, 3);
                    return Random.Range(0, 3);
                }
                if (x <= 25)
                {
                    if (Random.Range(0, 15) == 0)
                        return 13;
                    return Random.Range(1, 3);
                }

                if (x <= 51)
                {
                    if (Random.Range(0, 15) == 0)
                        return 12;
                    return Random.Range(1, 3);
                }
            }
        

            return 2;
        }
    }
}
