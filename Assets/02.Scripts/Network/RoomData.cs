using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class RoomData : MonoBehaviourPun
{
    [HideInInspector]
    public string roomName = string.Empty;
    [HideInInspector]
    public int connectPlayer = 0;
    [HideInInspector]
    public int maxPlayer = 0;
    public Text r_name;
    public Text curRoom;

    public void DisplayRoomData()
    {
        r_name.text = roomName;
        curRoom.text = "(" + connectPlayer.ToString() + "/" + maxPlayer.ToString() + ")";
    }
}
