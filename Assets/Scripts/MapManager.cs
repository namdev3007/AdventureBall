using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class MapManager : MonoBehaviour
{
    public StartPoint startPoint;

    private int mapNumber;

    private List<CheckpointControl> listCheckPointControl;
    private Dictionary<int, GameObject> dictCheckPoint;

    public AudioClip audioBGMap;
    public Vector2 scaleSkeleton;

    private void Start()
    {
        RxManager.saveCheckpoint.Subscribe(__checkPointControl => AddCheckPointControl(__checkPointControl)).AddTo(this);
        RxManager.preloadCheckpoint.Subscribe(__checkPointControl => PreLoadNextCheckPoint(__checkPointControl)).AddTo(this);

        if (startPoint == null)
        {
            startPoint = GetComponentInChildren<StartPoint>();
        }

        if (audioBGMap)
        {
            RxManager.SetBGMusicInMap.OnNext(audioBGMap);
        }
        else
        {
            RxManager.SetBGMusicDefaultInMap.OnNext(true);
        }

        if (scaleSkeleton != null)
        {
            RxManager.setScaleAvatarBoss.OnNext(scaleSkeleton);
        }
    }

    public void AddCheckPointControl(CheckpointControl _checkPointComtrol)
    {
        if (!listCheckPointControl.Contains(_checkPointComtrol))
        {
            listCheckPointControl.Add(_checkPointComtrol);
        }
    }

    public Vector3 GetPlayerPosition()
    {
        if (listCheckPointControl.Count == 0)
        {
            return startPoint.startPoint.position;
        }
        else
        {
            return listCheckPointControl[listCheckPointControl.Count - 1].pointSpawn.position;
        }
    }

    public int GetCurrentCheckPoint()
    {
        return listCheckPointControl.Count;
    }

    public void setStartMap(int _map)
    {

        mapNumber = _map;
        dictCheckPoint = new Dictionary<int, GameObject>();
        listCheckPointControl = new List<CheckpointControl>();
        LoadCurrentCheckPoint();

    }

    void LoadCurrentCheckPoint()
    {
        LoadCheckPointIndex(listCheckPointControl.Count);
    }

    string GetPathCheckPoint(int index)
    {
        Debug.Log("Map: " + mapNumber);
        return string.Format("CheckPoint/Map {0}/Checkpoint{1}", mapNumber, index);
    }

    void LoadCheckPointIndex(int index)
    {
        if (dictCheckPoint.ContainsKey(index) == false)
        {
            UnityEngine.Object obj = Resources.Load(GetPathCheckPoint(index), typeof(GameObject));
            InstanCheckPoint(index, obj);
        }
    }

    void InstanCheckPoint(int index, UnityEngine.Object obj)
    {
        if (obj != null && !dictCheckPoint.ContainsKey(index))
        {
            GameObject objCheckPoint = (Instantiate(obj, transform) as GameObject);
            dictCheckPoint.Add(index, objCheckPoint);
        }
    }

    IEnumerator LoadAsyncCheckPointIndex(int index)
    {
        ResourceRequest request = Resources.LoadAsync(GetPathCheckPoint(index), typeof(GameObject));
        while (!request.isDone)
        {
            yield return null;
        }
        InstanCheckPoint(index, request.asset);
    }

    public void PreLoadNextCheckPoint(CheckpointControl _checkPointComtrol)
    {
        int index = listCheckPointControl.Count + 1;
        if (dictCheckPoint.ContainsKey(index) == false)
        {
            StartCoroutine(LoadAsyncCheckPointIndex(index));
        }
    }

    public void reLoadCurrenCheckPoint()
    {
        if (dictCheckPoint.ContainsKey(listCheckPointControl.Count))
        {
            Destroy(dictCheckPoint[listCheckPointControl.Count]);
            dictCheckPoint.Remove(listCheckPointControl.Count);
        }

        LoadCurrentCheckPoint();
    }

    public void ReStartMap()
    {
        foreach (var checkPoint in dictCheckPoint)
        {
            Destroy(checkPoint.Value);
        }
        dictCheckPoint.Clear();

        foreach (var _checkPointControl in listCheckPointControl)
        {
            _checkPointControl.ResetCheckPoint();
        }
        listCheckPointControl.Clear();

        LoadCurrentCheckPoint();
    }
}