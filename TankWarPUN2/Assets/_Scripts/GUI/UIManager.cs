using UnityEngine;

namespace MANA3DGames
{
    public class UIManager
    {
        #region [Global Varaibles]

        AppManager cGameManager;

        UIMenu cRootUIMenu;
        UIMenu cLoginUIMenu;
        UIMenu cMainMenu;
        UIMenu cCreateRoomMenu;
        UIMenu cJoinRandomRoomMenu;
        UIMenu cRoomListMenu;
        UIMenu cInsideRoomMenu;

        Transform uRoomListContentTransform;
        GameObject uRoomListEntryPrefab;
        GameObject uPlayerListEntryPrefab;

        #endregion

        #region [Constructor]

        public UIManager( AppManager manager, GameObject uiRoot )
        {
            cGameManager = manager;

            cRootUIMenu =   new UIMenu( uiRoot );

            SetupLoginMenu();
            SetupMainMenu();
            SetupCreateRoomMenu();
            SetupJoinRandomRoomMenu();
            SetupRoomListMenu();
            SetupInsideRoomMenu();
        }

        #endregion

        #region [Debugging]

        public void SetConnectionStatusText( string text )
        {
            cRootUIMenu.SetText( "ConnectionStatus", text );
        }

        #endregion

        #region [Login Menu]

        private void SetupLoginMenu()
        {
            cLoginUIMenu =  new UIMenu( cRootUIMenu.Get( "LoginMenu" ) );

            cLoginUIMenu.AddListenerToButton( "LoginButton", ()=> {
                string playerName = cLoginUIMenu.GetInputFieldValue( "PlayerNameInput" );

                if ( !playerName.Equals("") )
                {
                    cGameManager.Login( playerName );
                }
                else
                {
                    Debug.LogError( "Player Name is invalid." );
                }
            } );
        }

        public void DisplayLoginMenu()
        {
            cLoginUIMenu.ShowRoot( true );
        }

        public void HideLoginMenu()
        {
            cLoginUIMenu.ShowRoot( false );
        }

        #endregion

        #region [Main Menu]

        private void SetupMainMenu()
        { 
            cMainMenu = new UIMenu( cRootUIMenu.Get( "MainMenu" ) );
            cMainMenu.AddListenerToButton( "CreateRoomButton", cGameManager.GoToCreateRoomMenu );
            cMainMenu.AddListenerToButton( "JoinRandomRoomButton", cGameManager.GoToJoinRandomRoom );
            cMainMenu.AddListenerToButton( "RoomListButton", cGameManager.GoToRoomListMenu );
        }

        public void DisplayMainMenu()
        {
            cMainMenu.ShowRoot( true );
        }

        public void HideMainMenu()
        {
            cMainMenu.ShowRoot( false );
        }

        #endregion

        #region [Create Room Menu]

        private void SetupCreateRoomMenu()
        {
            cCreateRoomMenu = new UIMenu( cRootUIMenu.Get( "CreateRoomMenu" ) );
            cCreateRoomMenu.AddListenerToButton( "CancelButton", cGameManager.GoToMainMenu );
            cCreateRoomMenu.AddListenerToButton( "CreateRoomButton", ()=> {
                string roomName = cCreateRoomMenu.GetInputFieldValue( "RoomNameInput" );
                roomName = ( roomName.Equals( string.Empty ) ) ? "Room " + Random.Range( 1000, 10000 ) : roomName;

                byte maxPlayers;
                byte.TryParse( cCreateRoomMenu.GetInputFieldValue( "MaxPlayersInput" ), out maxPlayers );
                maxPlayers = (byte)Mathf.Clamp( maxPlayers, 2, 8 );

                cGameManager.CreateRoom( roomName, maxPlayers );
            } );
        }

        public void DisplayCreateRoomMenu()
        {
            cCreateRoomMenu.ShowRoot( true );
        }

        public void HideCreateRoomMenu()
        {
            cCreateRoomMenu.ShowRoot( false );
        }

        #endregion

        #region [Join Random Room Menu]

        private void SetupJoinRandomRoomMenu()
        {
            cJoinRandomRoomMenu = new UIMenu( cRootUIMenu.Get( "JoinRandomRoomMenu" ) );
        }

        public void DisplayJoinRandomRoomMenu()
        {
            cJoinRandomRoomMenu.ShowRoot( true );
        }

        public void HideJoinRandomRoomMenu()
        {
            cJoinRandomRoomMenu.ShowRoot( false );
        }

        #endregion

        #region [Room List Menu]

        private void SetupRoomListMenu()
        {
            cRoomListMenu = new UIMenu( cRootUIMenu.Get( "RoomListMenu" ) );
            cRoomListMenu.AddListenerToButton( "BackButton", cGameManager.GoToMainMenu );


            uRoomListEntryPrefab = Resources.Load( "UI/RoomListEntryPrefab" ) as GameObject;

            uRoomListContentTransform = cRoomListMenu.GetTransform( "RoomListScrollView" ).Find( "Viewport" ).Find( "Content" );
        }

        public void DisplayRoomListMenu()
        {
            cRoomListMenu.ShowRoot( true );
        }

        public void HideRoomListMenu()
        {
            cRoomListMenu.ShowRoot( false );
        }


        public void UpdateRoomListView( PUNRoomData[] rooms )
        {
            ClearRoomListView();

            for ( int i = 0; i < rooms.Length; i++ )
            {
                GameObject entry = GameObject.Instantiate( uRoomListEntryPrefab );
                entry.transform.SetParent( uRoomListContentTransform );
                entry.transform.localScale = Vector3.one;

                int temp = i;
                UIMenu menu = new UIMenu( entry );
                menu.SetText( "RoomNameText", rooms[temp].Name );
                menu.SetText( "RoomPlayersText", (byte)rooms[temp].PlayerCount + " / " + rooms[temp].MaxPlayers );
                menu.AddListenerToButton( "JoinRoomButton", ()=> cGameManager.JoinRoom( rooms[temp].Name ) );
            }
        }
        public void ClearRoomListView()
        {
            Transform[] gameObjects = uRoomListContentTransform.GetComponentsInChildren<Transform>();
            for ( int i = 0; i < gameObjects.Length; i++ )
            {
                if ( gameObjects[i] == uRoomListContentTransform )
                    continue;

                GameObject.Destroy( gameObjects[i].gameObject );
            }
        }

        #endregion

        #region [Inside Room Menu]

        private void SetupInsideRoomMenu()
        {
            cInsideRoomMenu = new UIMenu( cRootUIMenu.Get( "InsideRoomMenu" ) );
            cInsideRoomMenu.AddListenerToButton( "LeaveGameButton", cGameManager.GoToMainMenu );
            cInsideRoomMenu.AddListenerToButton( "StartGameButton", cGameManager.GoToGameplay );

            uPlayerListEntryPrefab = Resources.Load( "UI/PlayerListEntryPrefab" ) as GameObject;
        }

        public void DisplayInsideRoomMenu()
        {
            cInsideRoomMenu.ShowRoot( true );
        }

        public void HideInsideRoomMenu()
        {
            cInsideRoomMenu.ShowRoot( false );
        }

        public void UpdateInsideRoomView( PUNPlayerInsideRoomData[] players, bool showStartGame )
        {
            ClearInsideRoomView();

            for ( int i = 0; i < players.Length; i++ )
                AddPlayerToInsideRoom( players[i], showStartGame );

            ShowStartGameButton( showStartGame );
        }

        public void AddPlayerToInsideRoom( PUNPlayerInsideRoomData player, bool showStartGame )
        {
            GameObject entry = GameObject.Instantiate( uPlayerListEntryPrefab );
            entry.transform.SetParent( cInsideRoomMenu.GetRootRect() );
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<PlayerListEntryMono>().Initialize( player.ActorNumber, player.NickName, player.IsPlayerReady );
            entry.name = player.ActorNumber.ToString();

            ShowStartGameButton( showStartGame );
        }

        public void OnPlayerPropertiesUpdate( int ActorNumber, bool isPlayerReady )
        {
            var entry = cInsideRoomMenu.GetRoot().transform.Find( ActorNumber.ToString() );
            if ( entry )
                entry.GetComponent<PlayerListEntryMono>().SetPlayerReady( isPlayerReady );
        }


        public void ClearInsideRoomView()
        {
            var children = cInsideRoomMenu.GetAllMenuItems();
            for ( int i = 0; i < children.Length; i++ )
            {
                if ( children[i].name.Equals( "LeaveGameButton" ) || children[i].name.Equals( "StartGameButton" ) )
                    continue;

                GameObject.Destroy( children[i] );
            }
        }
        public void RemovePlayerFromInsideRoom( string ActorNumber )
        {
            var go = cInsideRoomMenu.GetRoot().transform.Find( ActorNumber ).gameObject;
            if ( go )
                GameObject.Destroy( go );
        }

        public void ShowStartGameButton( bool show )
        {
            cInsideRoomMenu.ShowGameObject( "StartGameButton", show );
        }

        #endregion
    }
}
