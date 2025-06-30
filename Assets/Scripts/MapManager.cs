using UnityEngine;
using UniRx;
using System.Collections;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    public int mapIndex;
    public AudioClip mapBackgroundMusic;
    public StartPoint startPoint;

    private List<CheckpointControl> checkpointControls = new List<CheckpointControl>();
    private Dictionary<int, GameObject> loadedCheckpoints = new Dictionary<int, GameObject>();

    private void Start()
    {
        RxManager.SaveCheckpoint.Subscribe(AddCheckpoint).AddTo(this);
        RxManager.PreloadCheckpoint.Subscribe(PreloadCheckpoint).AddTo(this);

        if (startPoint == null)
        {
            startPoint = GetComponentInChildren<StartPoint>();
        }

        if (mapBackgroundMusic != null)
        {
            RxManager.SetBGMusicMap.OnNext(mapBackgroundMusic);
        }
        else
        {
            RxManager.SetDefaultBGMusicMap.OnNext(true);
        }

        LoadCurrentCheckpoint();
    }

    public void setStartMap(int index)
    {
        mapIndex = index;
        checkpointControls.Clear();
        loadedCheckpoints.Clear();
        LoadCurrentCheckpoint();
    }

    public Vector3 GetPlayerPosition()
    {
        return checkpointControls.Count == 0
            ? startPoint.startPoint.position
            : checkpointControls[checkpointControls.Count - 1].pointSpawn.position;
    }

    public void reLoadCurrenCheckPoint()
    {
        int currentIndex = checkpointControls.Count;
        if (loadedCheckpoints.ContainsKey(currentIndex))
        {
            Destroy(loadedCheckpoints[currentIndex]);
            loadedCheckpoints.Remove(currentIndex);
        }

        LoadCurrentCheckpoint();
    }

    public void RestartMap()
    {
        foreach (var cp in loadedCheckpoints.Values)
        {
            Destroy(cp);
        }

        loadedCheckpoints.Clear();

        foreach (var cpCtrl in checkpointControls)
        {
            cpCtrl.ResetCheckPoint();
        }

        checkpointControls.Clear();

        LoadCurrentCheckpoint();
    }

    private void AddCheckpoint(CheckpointControl checkpoint)
    {
        if (!checkpointControls.Contains(checkpoint))
        {
            checkpointControls.Add(checkpoint);
        }
    }

    private void PreloadCheckpoint(CheckpointControl current)
    {
        int nextIndex = checkpointControls.Count + 1;
        if (!loadedCheckpoints.ContainsKey(nextIndex))
        {
            StartCoroutine(LoadCheckpointAsync(nextIndex));
        }
    }

    private string GetCheckpointPath(int index)
    {
        return GameplayManager.Instance.inEvent
            ? $"CheckPointEvent/Map {mapIndex}/Checkpoint{index}"
            : $"CheckPoint/Map {mapIndex}/Checkpoint{index}";
    }

    private void LoadCheckpointByIndex(int index)
    {
        if (!loadedCheckpoints.ContainsKey(index))
        {
            Object prefab = Resources.Load(GetCheckpointPath(index));
            if (prefab != null)
            {
                GameObject instance = Instantiate(prefab, transform) as GameObject;
                loadedCheckpoints[index] = instance;
            }
        }
    }

    private IEnumerator LoadCheckpointAsync(int index)
    {
        ResourceRequest request = Resources.LoadAsync(GetCheckpointPath(index));
        yield return request;

        if (request.asset != null && !loadedCheckpoints.ContainsKey(index))
        {
            GameObject obj = Instantiate(request.asset, transform) as GameObject;
            loadedCheckpoints[index] = obj;
        }
    }

    private void LoadCurrentCheckpoint()
    {
        LoadCheckpointByIndex(checkpointControls.Count);
    }
}
