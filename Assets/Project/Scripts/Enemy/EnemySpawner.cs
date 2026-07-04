using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private GameObject[] enemyPrefab;
    [SerializeField]
    private int enemyCount = 10;
    [SerializeField]
    private Transform parentTransform;
    [SerializeField]
    private EntityBase target;

    private Vector3 offset = new Vector3(0.5f, 0.5f, 0);
    private List<Vector3> possibleTiles = new List<Vector3>();

    public static List<EntityBase> Enemies { get; private set; } = new List<EntityBase>();
    private void Awake()
    {
        // Tilemap의 Bounds 재설정(맵을 수정할 때 Bounds가 변경되지 않는 문제 해결)
        tilemap.CompressBounds();
        // 타일맵의 모든 타일을 대상으로 적을 배치할 수 있는 타일을 계산
        CalculatePossibleTiles();

        // 임의의 타일에 적 10기 생성
        for(int i=0; i < enemyCount; i++)
        {
            int type = Random.Range(0, enemyPrefab.Length);
            int index = Random.Range(0, possibleTiles.Count);

            GameObject clone = Instantiate(enemyPrefab[type], possibleTiles[index], Quaternion.identity, transform);
            clone.GetComponent<EnemyBase>().Initialize(this, parentTransform);
            clone.GetComponent<EnemyFSM>().Setup(target);
            Enemies.Add(clone.GetComponent<EntityBase>());
        }
    }
    private void CalculatePossibleTiles()
    {
        // BoundsInt는 Unity에서 정수로 표현된 축 정령 경계 상자를 나타내며
        // 주로 타일맵과 같은 3D 공간에서의 위치와 크기를 정의하는데 사용
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        // 외곽 벽에 붙은 타일은 제거하고자
        // x, y의 시작값은 1, 끝값은 bounds.size.x - 1, bounds.size.y - 1
        for(int y = 1; y < bounds.size.y - 1; y++)
        {
            for (int x = 1; x < bounds.size.x - 1; x++)
            {
                TileBase tile = allTiles[y * bounds.size.x + x];

                if( tile != null )
                {
                    // localPosition은 타일맵의 시작 지점으로부터 x, y축을 더해서 저장
                    Vector3Int localPosition = bounds.position + new Vector3Int(x, y);
                    // 그렇게 얻은 타일의 위치를 TileMap의 CellToWorld를 통해 월드좌표로 바꿔준 뒤
                    // offset을 더해 오브젝트가 해당 좌표의 타일의 정중앙에 오도록 위치를 계산해줌
                    Vector3 position = tilemap.CellToWorld(localPosition) + offset;
                    position.z = 0;

                    // 계산된 position을 possibleTiles에 저장
                    possibleTiles.Add(position);
                }
            }
        }
    }
    public void Deactivate(EntityBase enemy)
    {
        Enemies.Remove(enemy);
        Destroy(enemy.gameObject);
    }
}
