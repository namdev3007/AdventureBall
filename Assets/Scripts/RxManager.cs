using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class RxManager
{
    public static Subject<bool> victory = new Subject<bool>();
    public static Subject<int> selectSkinId = new Subject<int>();
    public static Subject<bool> SaveCoinIfVictory = new Subject<bool>();
    public static Subject<bool> trapped = new Subject<bool>();
    public static Subject<CheckpointControl> saveCheckpoint = new Subject<CheckpointControl>();
    public static Subject<CheckpointControl> preloadCheckpoint = new Subject<CheckpointControl>();
    public static Subject<int> playerEatCoin = new Subject<int>();
    public static Subject<bool> enemyAttackPlayer = new Subject<bool>();
    public static Subject<int> enemyAttackPlayerMore = new Subject<int>();
    public static Subject<bool> openHomePanel = new Subject<bool>();
    public static Subject<bool> clickedPlayAgainonPause = new Subject<bool>();
    public static Subject<bool> openGameUI = new Subject<bool>();
    public static Subject<bool> showGameOverPanel = new Subject<bool>();
    public static Subject<bool> showRevivePanel = new Subject<bool>();
    public static Subject<bool> playerTapToCloseBtn = new Subject<bool>();
    public static Subject<bool> onClickPauseBtn = new Subject<bool>();
    public static Subject<int> updateMoney = new Subject<int>();
    public static Subject<int> addCoin = new Subject<int>();
    public static Subject<int> addHealth = new Subject<int>();
    public static Subject<bool> openShop = new Subject<bool>();
    public static Subject<int> selectTabInShop = new Subject<int>();
    public static Subject<bool> playerSpawn = new Subject<bool>();

    public static Subject<AudioClip> playAudioE = new Subject<AudioClip>();
    public static Subject<AudioClip> playAudioCoin = new Subject<AudioClip>();

    public static Subject<bool> playerJumping = new Subject<bool>();
    public static Subject<float> playerOnGround = new Subject<float>();
    public static Subject<bool> playerFallInWatter = new Subject<bool>();

    public static Subject<int> updateLive = new Subject<int>();
    public static Subject<int> updateHeal = new Subject<int>();

    public static Subject<System.Action> actionPanelBlack = new Subject<System.Action>();
    public static Subject<System.Action> loadingDone = new Subject<System.Action>();
    public static Subject<bool> audioSnapOnOff = new Subject<bool>();

    public static Subject<bool> playerDie = new Subject<bool>();

    public static Subject<bool> audioClick = new Subject<bool>();

    public static Subject<bool> setTextCoinVictory = new Subject<bool>();


    public static Subject<float> SetSizeMagnetRx = new Subject<float>();

    //Skin Panel In Shop
    public static Subject<int> ShowSkinImageDetail = new Subject<int>();

    //Play Anim Player
    public static Subject<bool> PlayAnimHappy = new Subject<bool>();

    public static Subject<string> ActionNotice = new Subject<string>();

    public static Subject<AudioClip> SetBGMusicInMap = new Subject<AudioClip>();
    public static Subject<bool> SetBGMusicDefaultInMap = new Subject<bool>();
    public static Subject<Vector2> setScaleAvatarBoss = new Subject<Vector2>();

    public static Subject<int> NewSkinPanel = new Subject<int>();
    public static Subject<bool> UpdateAvatarInGamePlayPanel = new Subject<bool>();
    public static Subject<int> TextLiveBonus = new Subject<int>();

    public static Subject<int> ShowTabShopSkin = new Subject<int>();

    public static Subject<bool> purchaseSpecialOffer = new Subject<bool>();

    public static Subject<bool> VictoryLevelEvent = new Subject<bool>();
    public static Subject<bool> LoseLevelEvent = new Subject<bool>();

    public static Subject<bool> ActiveChristmasEvent = new Subject<bool>();
    public static Subject<bool> DeactiveChristmasEvent = new Subject<bool>();


}