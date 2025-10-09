using UnityEditor.EditorTools;
using UnityEngine;

public class RandomTerrian : MonoBehaviour
{
    
    public GameObject[] terrainPrefabs; // 拖入你的PP_Lake, PP_Meadow等
    [Header("总区域块个数")]
    public int gridWidthNumber = 10;
    public int gridHeightNumber = 10;
    [Header("block offset")]
    [Tooltip("每个块占据尺寸，根据你的地形块实际大小调整")]
    public float tileSize = 10f; 
    
    public bool randomGenerate = true;
    
    void Start()
    {
        GenerateTerrain();
    }
    
    void GenerateTerrain()
    {
        for(int x = 0; x < gridWidthNumber; x++)
        {
            for(int z = 0; z < gridHeightNumber; z++)
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