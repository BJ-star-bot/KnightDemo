using UnityEngine;

public class RandomTerrian : MonoBehaviour
{
    
    public GameObject[] terrainPrefabs; // 拖入你的PP_Lake, PP_Meadow等
    [Header("Area size")]
    public int gridWidth = 10;
    public int gridHeight = 10;
    [Header("block offset")]
    public float tileSize = 10f; // 根据你的地形块实际大小调整
    
    public bool randomGenerate = true;
    
    void Start()
    {
        GenerateTerrain();
    }
    
    void GenerateTerrain()
    {
        for(int x = 0; x < gridWidth; x++)
        {
            for(int z = 0; z < gridHeight; z++)
            {
                Vector3 position = new Vector3(x * tileSize, 0, z * tileSize);
                
                GameObject prefabToSpawn;
                if(randomGenerate)
                {
                    // 随机选择地形块
                    prefabToSpawn = terrainPrefabs[Random.Range(0, terrainPrefabs.Length)];
                }
                else
                {
                    // 使用第一个地形块
                    prefabToSpawn = terrainPrefabs[0];
                }
                Quaternion rot = Quaternion.Euler(0, 90 * Random.Range(0, 4), 0);
                Instantiate(prefabToSpawn, position, rot, this.transform);
            }
        }
    }
}