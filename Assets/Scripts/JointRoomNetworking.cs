using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class JointRoomNetworking : MonoBehaviourPunCallbacks
{
    // Control UI
    public TMP_InputField roomName;
    public TMP_InputField playerName;
    public Canvas Canvas_Create_Joint_Room;
    public TMP_Text TMP_Loading;
    
    const int maxRoomPlayer = 2;

    private void Awake()
    {
        DontDestroyOnLoad(this.transform);

        PhotonNetwork.SendRate = 30;
        PhotonNetwork.SerializationRate = 20;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Control UI
        TMP_Loading.gameObject.SetActive(true);
        Canvas_Create_Joint_Room.gameObject.SetActive(false);

        PhotonNetwork.ConnectUsingSettings();
    }

    #region Photon calls back
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
        Debug.Log("On Connected To Master!!!");
    }

    public override void OnJoinedLobby()
    {
        // Control UI
        TMP_Loading.gameObject.SetActive(false);
        Canvas_Create_Joint_Room.gameObject.SetActive(true);

        Debug.Log("On joined lobby!!!");
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        PhotonNetwork.LoadLevel("WinStreakHasEnded");
        //PhotonNetwork.LoadLevel("TestSpawnTower");
        Debug.Log("OnJoinedRoom");
    }

    private void OnDisConnectedFromPhoton()
    {

    }
    #endregion


    #region Control UI
    public void ButtonCreateRoom()
    {
        if(roomName.text.Length > 1 && playerName.text.Length > 1)
            PhotonNetwork.CreateRoom(
                roomName.text, 
                new Photon.Realtime.RoomOptions() { MaxPlayers = maxRoomPlayer },
                null);
    }

    public void ButtonJoinRoom()
    {
        if (roomName.text.Length > 1 && playerName.text.Length > 1)
            PhotonNetwork.JoinRoom(roomName.text);
    }
    #endregion

    
}
