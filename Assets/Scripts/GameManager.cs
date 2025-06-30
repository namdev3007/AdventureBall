using UnityEngine;
using UniRx;

public class GameplayManager : SingletonDontDestroy<GameplayManager>
{
    public static readonly int MAX_MAP = 100;

    public Transform parent;
    public PlayerController player;

    private MapManager currentMap;
    public bool inEvent = false;
    public int Map
    {
        get => PlayerPrefs.GetInt("MAP", 1);
        set
        {
            PlayerPrefs.SetInt("MAP", value);
            PlayerPrefs.Save();
        }
    }

    private void Start()
    {
        RxManager.victory.Subscribe(_ => DestroyCurrentMap()).AddTo(this);
        RxManager.playerSpawn.Subscribe(_ => RespawnPlayer()).AddTo(this);
    }

    public void LoadMap()
    {
        DestroyCurrentMap();

        string path = inEvent
            ? $"ChristmasMap/Map {Map}"
            : $"Map/Map {Map}";

        GameObject mapPrefab = Resources.Load<GameObject>(path);
        if (mapPrefab != null)
        {
            GameObject mapGO = Instantiate(mapPrefab, parent);
            currentMap = mapGO.GetComponent<MapManager>();
            currentMap.setStartMap(Map);
        }

        CreatePlayer();
    }

    public void DestroyCurrentMap()
    {
        if (currentMap != null)
        {
            Destroy(currentMap.gameObject);
            currentMap = null;
        }

        if (player != null)
        {
            player.gameObject.SetActive(false);
        }
    }

    public void CreatePlayer()
    {
        if (currentMap == null || player == null) return;

        Vector3 spawnPos = currentMap.GetPlayerPosition();
        player.transform.position = spawnPos;
        player.Reset();
        player.gameObject.SetActive(true);
    }

    public void RespawnPlayer()
    {
        if (currentMap == null || player == null) return;

        currentMap.reLoadCurrenCheckPoint();
        player.Reset();
        player.transform.position = currentMap.GetPlayerPosition();
        player.gameObject.SetActive(true);
    }
}


