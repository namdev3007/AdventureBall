using UniRx;
using UnityEngine;

public static class RxManager
{
    public static readonly Subject<CheckpointControl> SaveCheckpoint = new Subject<CheckpointControl>();
    public static readonly Subject<CheckpointControl> PreloadCheckpoint = new Subject<CheckpointControl>();

    public static readonly Subject<AudioClip> SetBGMusicMap = new Subject<AudioClip>();
    public static readonly Subject<bool> SetDefaultBGMusicMap = new Subject<bool>();

    public static readonly Subject<Vector2> SetScaleAvatarBoss = new Subject<Vector2>();
    public static Subject<CheckpointControl> preloadCheckpoint = new Subject<CheckpointControl>();
    public static Subject<CheckpointControl> saveCheckpoint = new Subject<CheckpointControl>();
    public static Subject<AudioClip> playAudioE = new Subject<AudioClip>();
    public static Subject<bool> victory = new Subject<bool>();
    public static Subject<bool> playerSpawn = new Subject<bool>();
    public static Subject<bool> enemyAttackPlayer = new Subject<bool>();
    public static Subject<int> enemyAttackPlayerMore = new Subject<int>();
    public static Subject<bool> playerJumping = new Subject<bool>();
    public static Subject<float> playerOnGround = new Subject<float>();
    public static Subject<bool> playerFallInWatter = new Subject<bool>();
    public static Subject<bool> playerDie = new Subject<bool>();
}
