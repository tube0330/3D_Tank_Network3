using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{
    static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }
        set
        {
            if (instance == null)
                instance = value;
            else if (instance != value)
                Destroy(value.gameObject);
        }
    }
    [SerializeField] private List<Transform> spawnList;
    [SerializeField] private GameObject apachePrefab;
    public bool isGameOver = false;
    public Text txt_curPlayer;
    public Text txt_logMessage;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        apachePrefab = Resources.Load<GameObject>("Apache");
        CreateTank();
        PhotonNetwork.IsMessageQueueRunning = true;
    }
    
    void Start()
    {
        var spawnPoint = GameObject.Find("SpawnPoints").gameObject;
        if (spawnPoint != null)
            spawnPoint.GetComponentsInChildren<Transform>(spawnList);

        spawnList.RemoveAt(0);
        //if (spawnList.Count > 0)

        string msg = "\n<color=#00ff00>[" + PhotonNetwork.NickName + "]Connected</color>";
        photonView.RPC(nameof(LogMessage), RpcTarget.AllBuffered, msg);

        if(spawnList.Count > 0 && PhotonNetwork.IsMasterClient) 
        InvokeRepeating("CreateApache", 0.01f, 3.0f);
    }

    void Update()
    {
        GetConnectPlayerCount();
    }

    public void CreateTank()
    {
        float pos = Random.Range(-50f, 50f);
        PhotonNetwork.Instantiate("Tank", new Vector3(pos, 0f, pos), Quaternion.identity, 0);
    }
    
    void CreateApache()
    {
        if (isGameOver) return;

        int count = (int)GameObject.FindGameObjectsWithTag("APACHE").Length;

        if (count < 10)
        {
            int idx = Random.Range(0, spawnList.Count);
            PhotonNetwork.InstantiateRoomObject("Apache", spawnList[idx].position, spawnList[idx].rotation, 0, null);
        }
    }
    
    [PunRPC]
    public void ApplyPlayerCountUpdate()
    {
        Room curRoom = PhotonNetwork.CurrentRoom;
        txt_curPlayer.text = curRoom.PlayerCount.ToString() + " / " + curRoom.MaxPlayers.ToString();
    }

    [PunRPC]
    void GetConnectPlayerCount()
    {
        if (PhotonNetwork.IsMasterClient)
            photonView.RPC(nameof(ApplyPlayerCountUpdate), RpcTarget.All);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GetConnectPlayerCount();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        GetConnectPlayerCount();
    }

    public void OnClickExitRoom()   //룸 나가기 버튼 클릭 이벤트에 연결될 함수
    {
        string msg = "\n<color=#ff0000>[" + PhotonNetwork.NickName + "]DisConnected</color>";
        photonView.RPC(nameof(LogMessage), RpcTarget.AllBuffered, msg);

        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadSceneAsync("Lobby");
    }

    [PunRPC]
    void LogMessage(string message)
    {
        txt_logMessage.text += message;
    }
}
