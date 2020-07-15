using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun.UtilityScripts;

namespace MANA3DGames
{
    public class PUNManager : MonoBehaviourPunCallbacks
    {
        #region [Global Varaibles]

        private AppManager cGameManager;

        private Dictionary<string, RoomInfo> cachedRoomList;

        #endregion


        #region [MonoBehaviours]

        private void Awake()
        {
            cGameManager = GetComponent<AppManager>();

            PhotonNetwork.AutomaticallySyncScene = true;

            cachedRoomList = new Dictionary<string, RoomInfo>();
        }

        #endregion


        #region [PhotonNetwork]

        public void Connect( string playerName )
        {
            PhotonNetwork.LocalPlayer.NickName = playerName;
            PhotonNetwork.ConnectUsingSettings();
        }

        public void JoinRoom( string roomName )
        {
            LeaveLobby();
            PhotonNetwork.JoinRoom( roomName );
        }

        public void JoinRandomRoom()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        public void JoinLobby()
        {
            if ( !PhotonNetwork.InLobby )
                PhotonNetwork.JoinLobby();
        }

        public void LeaveLobby()
        {
            if ( PhotonNetwork.InLobby )
                PhotonNetwork.LeaveLobby();
        }

        public void CreateRoom( string roomName, byte maxPlayers )
        {
            RoomOptions options = new RoomOptions { MaxPlayers = maxPlayers };
            PhotonNetwork.CreateRoom( roomName, options, null );
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void LoadScene( string sceneName )
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel( sceneName );
        }

        public string GetConnectionStatus()
        {
            return "  Connection Status:  " + PhotonNetwork.NetworkClientState;
        }

        public bool CheckPlayersReady()
        {
            if ( !PhotonNetwork.IsMasterClient )
                return false;

            foreach ( Player p in PhotonNetwork.PlayerList )
            {
                object isPlayerReady;
                if ( p.CustomProperties.TryGetValue( PUNGameData.PLAYER_READY, out isPlayerReady ) )
                {
                    if ( !(bool) isPlayerReady )
                        return false;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        #endregion


        #region [RoomList/Lobby]

        public void SetPrefab( string prefabName )
        {
            PhotonNetwork.LocalPlayer.SetCustomProperties( new Hashtable { { PUNGameData.PLAYER_PREFAB_NAME, prefabName } } );
        }

        private void UpdateCachedRoomList( List<RoomInfo> roomList )
        {
            foreach ( RoomInfo info in roomList )
            {
                // Remove room from cached room list if it got closed, became invisible or was marked as removed
                if ( !info.IsOpen || !info.IsVisible || info.RemovedFromList )
                {
                    if ( cachedRoomList.ContainsKey( info.Name ) )
                        cachedRoomList.Remove( info.Name );

                    continue;
                }

                // Update cached room info
                if ( cachedRoomList.ContainsKey( info.Name ) )
                    cachedRoomList[info.Name] = info;
                // Add new room info to cache
                else
                    cachedRoomList.Add( info.Name, info );
            }
        }

        #endregion


        #region [PUN CALLBACKS]

        public override void OnConnectedToMaster()
        {
            cGameManager.GoToMainMenu();
        }

        public override void OnRoomListUpdate( List<RoomInfo> roomList )
        {
            UpdateCachedRoomList( roomList );

            PUNRoomData[] rooms = new PUNRoomData[roomList.Count];
            for ( int i = 0; i < roomList.Count; i++ )
                rooms[i] = new PUNRoomData( roomList[i].Name, roomList[i].PlayerCount, roomList[i].MaxPlayers );

            cGameManager.UpdateRoomListView( rooms );
        }

        public override void OnLeftLobby()
        {
            cachedRoomList.Clear();
            cGameManager.ClearRoomListView();
        }

        public override void OnCreateRoomFailed( short returnCode, string message )
        {
            cGameManager.GoToMainMenu();
        }

        public override void OnJoinRoomFailed( short returnCode, string message )
        {
            cGameManager.GoToMainMenu();
        }

        public override void OnJoinRandomFailed( short returnCode, string message )
        {
            cGameManager.GoToMainMenu();
        }

        public override void OnJoinedRoom()
        {
            PUNPlayerInsideRoomData[] players = new PUNPlayerInsideRoomData[PhotonNetwork.PlayerList.Length];
            for ( int i = 0; i < PhotonNetwork.PlayerList.Length; i++ )
            {
                players[i] = new PUNPlayerInsideRoomData( PhotonNetwork.PlayerList[i].ActorNumber, PhotonNetwork.PlayerList[i].NickName );

                object isPlayerReady;
                if ( PhotonNetwork.PlayerList[i].CustomProperties.TryGetValue( PUNGameData.PLAYER_READY, out isPlayerReady ) )
                    players[i].IsPlayerReady = (bool)isPlayerReady;
            }

            Hashtable props = new Hashtable
            {
                { PUNGameData.PLAYER_LOADED_LEVEL, false }
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties( props );


            cGameManager.UpdateInsideRoomView( players, CheckPlayersReady() );
            cGameManager.GoToInsideRoomMenu();
        }

        public override void OnLeftRoom()
        {
            cGameManager.ClearInsideRoomView();
            cGameManager.GoToMainMenu();
        }

        public override void OnPlayerEnteredRoom( Player newPlayer )
        {
            var player = new PUNPlayerInsideRoomData( newPlayer.ActorNumber, newPlayer.NickName );
            cGameManager.AddPlayerToInsideRoom( player, CheckPlayersReady() );
        }

        public override void OnPlayerLeftRoom( Player otherPlayer )
        {
            cGameManager.RemovePlayerFromInsideRoom( otherPlayer.ActorNumber.ToString() );
        }

        public override void OnMasterClientSwitched( Player newMasterClient )
        {
            if ( PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber )
                cGameManager.CheckToShowStartGameButton();
        }

        public override void OnPlayerPropertiesUpdate( Player target, Hashtable changedProps )
        {
            object isPlayerReady;
            if ( changedProps.TryGetValue( PUNGameData.PLAYER_READY, out isPlayerReady ) )
                cGameManager.OnPlayerPropertiesUpdate( target.ActorNumber, (bool)isPlayerReady );
        }

        #endregion
    }
}
