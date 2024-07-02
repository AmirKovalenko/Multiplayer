using System;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MultiplayerGameManager : MonoBehaviourPun
{
    private const string PlayerPrefabName = "Prefabs\\Player Prefab";

    [Header("Spawn Points")]
    [SerializeField] private bool randomizeSpawnPoint;

    [SerializeField] private SpawnPoint[] randomSpawnPoints;

    [SerializeField] private SpawnPoint defaultSpawnPoint;

    [Header("UI")]
    [SerializeField] private Button readyButton;

    private const string CLIENT_IS_READY_RPC = nameof(ClientIsReady);
    private const string SET_SPAWN_POINT_RPC = nameof(SetSpawnPoint);
    private const string GAME_STARTED_RPC = nameof(GameStarted);

    private PlayerMovement myPlayerMovement;
    private int playersReady = 0;


    public void SendReadyToMasterClient()
    {
        photonView.RPC(CLIENT_IS_READY_RPC, RpcTarget.MasterClient);
        readyButton.interactable = false;
    }

    private void Start()
    {
        //Method A
        // SpawnPoint targetSpawnPoint;
        // if (randomizeSpawnPoint)
        //     targetSpawnPoint = GetRandomSpawnPoint();
        // else
        //     targetSpawnPoint = defaultSpawnPoint;
        //
        // SpawnPlayer(targetSpawnPoint);

    }

    SpawnPoint GetRandomSpawnPoint()
    {
        List<SpawnPoint> availableSpawnPoints = new List<SpawnPoint>();

        foreach (var spawnPoint in randomSpawnPoints)
        {
            if (!spawnPoint.IsTaken)
            {
                availableSpawnPoints.Add(spawnPoint);
            }
        }

        if (availableSpawnPoints.Count == 0)
        {
            Debug.LogError("All spawn points are taken!");
        }

        int index = Random.Range(0, availableSpawnPoints.Count);
        return availableSpawnPoints[index];
    }

    void SpawnPlayer(SpawnPoint targetSpawnPoint)
    {
        targetSpawnPoint.Take();
        GameObject playerGO = PhotonNetwork.Instantiate(PlayerPrefabName,
            targetSpawnPoint.transform.position, targetSpawnPoint.transform.rotation);
        myPlayerMovement = playerGO.GetComponent<PlayerMovement>();
    }

    //Method A
    // [PunRPC]
    // private void ClientIsReady()
    // {
    //     Debug.Log("A Client is ready!");
    // }

    //Method B
    [PunRPC]
    private void ClientIsReady(PhotonMessageInfo messageInfo)
    {
        Debug.Log(messageInfo.Sender + " Is ready");
        SpawnPoint randomSpawnPoint = GetRandomSpawnPoint();
        randomSpawnPoint.Take();

        messageInfo.photonView.RPC(SET_SPAWN_POINT_RPC, messageInfo.Sender, randomSpawnPoint.ID);

        playersReady++;
        if (playersReady >= PhotonNetwork.CurrentRoom.PlayerCount)
        {
            photonView.RPC(GAME_STARTED_RPC, RpcTarget.All);
        }
    }

    // this will be invoked on the client
    [PunRPC]
    private void SetSpawnPoint(int spawnPointID)
    {
        foreach (var spawnPoint in randomSpawnPoints)
        {
            if (spawnPoint.ID == spawnPointID)
            {
                SpawnPlayer(spawnPoint);
                break;
            }
        }
    }

    [PunRPC]
    private void GameStarted()
    {
        myPlayerMovement.enabled = true;
        Debug.Log("Master client has the game started.");
    }


}