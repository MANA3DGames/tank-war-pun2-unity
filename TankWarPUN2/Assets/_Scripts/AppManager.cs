using UnityEngine;

namespace MANA3DGames
{
    public class AppManager : MonoBehaviour
    {
        #region [Global Variables]

        private enum State
        {
            LoginMenu,
            MainMenu,
            CreateRoomMenu,
            JoinningRandomRoom,
            RoomListMenu,
            InsideRoomMenu,
            RoomGameplay
        }

        private State gState;

        private UIManager cUIManager;
        private PUNManager cPUNManager;

        #endregion


        #region [MonoBehaviour]

        private void Start()
        {
            cPUNManager = GetComponent<PUNManager>();
            cUIManager = new UIManager( this, GameObject.Find( "UI_Root" ) );

            GoToLoginMenu();
        }

        private void Update()
        {
            cUIManager.SetConnectionStatusText( cPUNManager.GetConnectionStatus() );


            if ( gState != State.InsideRoomMenu )
                return;

            if ( Input.GetKeyDown( KeyCode.Alpha1 ) )
                cPUNManager.SetPrefab( "Tanks/0" );
            else if ( Input.GetKeyDown( KeyCode.Alpha2 ) )
                cPUNManager.SetPrefab( "Tanks/1" );
        }

        #endregion


        #region [PUN Functions]

        public void Login( string playerName )
        {
            cPUNManager.Connect( playerName );
            cUIManager.HideLoginMenu();
        }

        public void CreateRoom( string roomName, byte maxPlayers )
        {
            cUIManager.HideCreateRoomMenu();
            cPUNManager.CreateRoom( roomName, maxPlayers );
        }

        public void JoinRoom( string roomName )
        {
            cPUNManager.JoinRoom( roomName );
        }

        public void UpdateRoomListView( PUNRoomData[] rooms )
        {
            cUIManager.UpdateRoomListView( rooms );
        }
        public void ClearRoomListView()
        {
            cUIManager.ClearRoomListView();
        }

        public void UpdateInsideRoomView( PUNPlayerInsideRoomData[] players, bool showStartGame )
        {
            cUIManager.UpdateInsideRoomView( players, showStartGame );
        }
        public void ClearInsideRoomView()
        {
            cUIManager.ClearInsideRoomView();
        }

        public void AddPlayerToInsideRoom( PUNPlayerInsideRoomData player, bool showStartGame )
        {
            cUIManager.AddPlayerToInsideRoom( player, showStartGame );
            CheckToShowStartGameButton();
        }
        public void RemovePlayerFromInsideRoom( string ActorNumber )
        {
            cUIManager.RemovePlayerFromInsideRoom( ActorNumber );
            CheckToShowStartGameButton();
        }

        public void OnPlayerPropertiesUpdate( int ActorNumber, bool isPlayerReady )
        {
            cUIManager.OnPlayerPropertiesUpdate( ActorNumber, isPlayerReady );
            CheckToShowStartGameButton();
        }

        public void CheckToShowStartGameButton()
        {
            cUIManager.ShowStartGameButton( cPUNManager.CheckPlayersReady() );
        }

        #endregion
        
        
        #region [GoTo Functions]

        public void GoToLoginMenu()
        {
            cUIManager.DisplayLoginMenu();
            gState = State.LoginMenu;
        }

        public void GoToMainMenu()
        {
            switch ( gState )
            {
                case State.LoginMenu:
                    cUIManager.HideLoginMenu();
                    break;
                case State.CreateRoomMenu:
                    cUIManager.HideCreateRoomMenu();
                    break;
                case State.RoomListMenu:
                    cUIManager.HideRoomListMenu();
                    cPUNManager.LeaveLobby();
                    break;
                case State.InsideRoomMenu:
                    cUIManager.HideInsideRoomMenu();
                    cPUNManager.LeaveRoom();
                    break;
            }

            cUIManager.DisplayMainMenu();
            gState = State.MainMenu;
        }

        public void GoToCreateRoomMenu()
        {
            switch ( gState )
            {
                case State.MainMenu:
                    cUIManager.HideMainMenu();
                    break;
            }

            cUIManager.DisplayCreateRoomMenu();
            gState = State.CreateRoomMenu;
        }

        public void GoToJoinRandomRoom()
        {
            switch ( gState )
            {
                case State.MainMenu:
                    cUIManager.HideMainMenu();
                    break;
            }

            cUIManager.DisplayJoinRandomRoomMenu();
            cPUNManager.JoinRandomRoom();
            gState = State.JoinningRandomRoom;
        }

        public void GoToRoomListMenu()
        {
            switch ( gState )
            {
                case State.MainMenu:
                    cUIManager.HideMainMenu();
                    break;
            }

            cUIManager.DisplayRoomListMenu();
            cPUNManager.JoinLobby();
            gState = State.RoomListMenu;
        }

        public void GoToInsideRoomMenu()
        {
            switch ( gState )
            {
                case State.MainMenu:
                    cUIManager.HideMainMenu();
                    break;
                case State.RoomListMenu:
                    cUIManager.HideRoomListMenu();
                    break;
            }

            cUIManager.DisplayInsideRoomMenu();
            gState = State.InsideRoomMenu;
        }

        public void GoToGameplay()
        {
            switch ( gState )
            {
                case State.InsideRoomMenu:
                    cUIManager.HideInsideRoomMenu();
                    break;
            }

            cPUNManager.LoadScene( "Arena1" );
            gState = State.RoomGameplay;
        }

        #endregion
    }
}