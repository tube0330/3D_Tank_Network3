using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PhotonInit : MonoBehaviourPunCallbacks
{
    readonly string gameVersion = "V1.0";
    public InputField userID;
    public InputField roomName;
    public GameObject r_item;
    public GameObject scrollContents;
    readonly string roomItemTag = "ROOMITEM";
    private bool isConnectedAndInLobby = false;

    void Awake()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
            roomName.text = "Room_" + Random.Range(0, 999).ToString("000");
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Server.");
        PhotonNetwork.JoinLobby(); // 로비에 참여
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby.");
        userID.text = GetUserId();
        isConnectedAndInLobby = true; // 로비에 참여 성공 시 플래그 설정
    }

    string GetUserId()
    {
        string userId = PlayerPrefs.GetString("USER_ID");
        if (string.IsNullOrEmpty(userId))
            userId = "USER_" + Random.Range(0, 999).ToString("000");
        return userId;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogWarning("Failed to join random room: " + message);
        PhotonNetwork.CreateRoom("MyRoom", new RoomOptions { MaxPlayers = 20 });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Successfully joined room.");
        StartCoroutine(LoadTankMainScene());
    }

    IEnumerator LoadTankMainScene()
    {
        PhotonNetwork.IsMessageQueueRunning = false;
        AsyncOperation ao = SceneManager.LoadSceneAsync("TankMainScene");
        yield return ao;
    }

    public void OnClickJoinRandomRoom()
    {
        if (isConnectedAndInLobby) // 로비에 참여한 상태인지 확인
        {
            PhotonNetwork.NickName = userID.text;
            PlayerPrefs.SetString("USER_ID", userID.text);
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            Debug.LogError("Cannot join room: Not connected or not in lobby.");
        }
    }

    public void OnClickCreateRoom()
    {
        string _roomName = roomName.text;

        if (string.IsNullOrEmpty(roomName.text))
            _roomName = "ROOM_" + Random.Range(0, 999).ToString("000");

        PhotonNetwork.NickName = userID.text;
        PlayerPrefs.SetString("USER_ID", userID.text);

        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 20;

        PhotonNetwork.CreateRoom(_roomName, ro, TypedLobby.Default);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(roomItemTag))
            Destroy(obj);

        foreach (RoomInfo r_info in roomList)
        {
            if (r_info.RemovedFromList) continue;

            GameObject room = Instantiate(r_item);
            room.transform.SetParent(scrollContents.transform, false);

            RoomData roomData = room.GetComponent<RoomData>();
            roomData.roomName = r_info.Name;
            roomData.connectPlayer = r_info.PlayerCount;
            roomData.maxPlayer = r_info.MaxPlayers;
            roomData.DisplayRoomData();

            Button button = roomData.GetComponent<Button>();
            button.onClick.AddListener(() => OnClickRoomItem(roomData.roomName));

            if (roomData.connectPlayer == 0)
                Destroy(room);
        }
    }

    void OnClickRoomItem(string roomName)
    {
        if (isConnectedAndInLobby) // 로비에 참여한 상태인지 확인
        {
            PhotonNetwork.NickName = userID.text;
            PlayerPrefs.SetString("USER_ID", userID.text);
            PhotonNetwork.JoinRoom(roomName);
        }
        else
        {
            Debug.LogError("Cannot join room: Not connected or not in lobby.");
        }
    }

    private void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.InRoom.ToString());
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Create Room Failed: " + message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Join Room Failed: " + message);
    }
}
