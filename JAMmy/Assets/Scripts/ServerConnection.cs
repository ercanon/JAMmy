using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class ServerConnection : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField inputRoom;

    public void ConnectServer()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        //Load
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(inputRoom.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(inputRoom.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
    }
}
