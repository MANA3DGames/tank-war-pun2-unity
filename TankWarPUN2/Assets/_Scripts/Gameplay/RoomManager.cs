using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;


namespace MANA3DGames
{
    public class RoomManager : MonoBehaviourPunCallbacks
    {
        public Text InfoText;
        //public GameObject tankGO;
        //public GameObject tankBullet;
        //TankControl tankControl;
        //PlayerInput playerInput;


        private float gStartTime;

        private const string CountdownStartTime = "StartTime";




        #region [MonoBehaviour]

        public void Start()
        {
            InfoText.text = "Waiting for other players...";

            Hashtable props = new Hashtable
            {
                { PUNGameData.PLAYER_LOADED_LEVEL, true }
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties( props );







            //tankControl = new TankControl( tankGO, tankBullet );
            //playerInput = new PlayerInput( this );
        }

        //public void Update()
        //{
        //    if ( playerInput != null && tankControl.GetIsMine() )
        //        playerInput.Update( tankControl );
        //}

        //public void FixedUpdate()
        //{
        //    if ( playerInput != null && tankControl.GetIsMine() )
        //        playerInput.FixedUpdate( tankControl );
        //}

        #endregion


        #region PUN CALLBACKS

        public override void OnDisconnected( DisconnectCause cause )
        {
            SceneManager.LoadScene( "Lobby" );
        }

        public override void OnLeftRoom()
        {
            PhotonNetwork.Disconnect();
        }

        public override void OnMasterClientSwitched( Player newMasterClient )
        {
            if ( PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber )
            {
                //StartCoroutine( SpawnAsteroid() );
                Debug.Log( "Start Over Game Logic..." );
            }
        }

        public override void OnPlayerLeftRoom( Player otherPlayer )
        {
            CheckEndOfGame();
        }

        public override void OnPlayerPropertiesUpdate( Player target, Hashtable changedProps )
        {
            if (changedProps.ContainsKey( PUNGameData.PLAYER_LIVES ) )
            {
                CheckEndOfGame();
                return;
            }

            if ( !PhotonNetwork.IsMasterClient )
            {
                return;
            }

            if ( changedProps.ContainsKey( PUNGameData.PLAYER_LOADED_LEVEL ) )
            {
                if ( CheckAllPlayerLoadedLevel() )
                {
                    Hashtable props = new Hashtable
                        {
                            { CountdownStartTime, (float) PhotonNetwork.Time }
                        };
                    PhotonNetwork.CurrentRoom.SetCustomProperties( props );
                }
            }
        }

        public override void OnRoomPropertiesUpdate( Hashtable propertiesThatChanged )
        {
            object startTimeFromProps;

            if ( propertiesThatChanged.TryGetValue( CountdownStartTime, out startTimeFromProps ) )
            {
                gStartTime = (float)startTimeFromProps;
                StartCoroutine( StartGame() );
            }
        }

        #endregion


        #region [Gameplay Logic]

        private IEnumerator StartGame()
        {
            float Countdown = 5.0f;
            float countdown = Countdown;

            while ( countdown > 0.0f )
            {
                float timer = (float)PhotonNetwork.Time - gStartTime;
                countdown = Countdown - timer;

                InfoText.text = string.Format( "Game starts in {0} seconds", countdown.ToString( "n2" ) );
                yield return new WaitForEndOfFrame();
            }

            InfoText.text = string.Empty;


            float angularStart = ( 360.0f / PhotonNetwork.CurrentRoom.PlayerCount ) * PhotonNetwork.LocalPlayer.GetPlayerNumber();
            float x = 20.0f * Mathf.Sin( angularStart * Mathf.Deg2Rad );
            float z = 20.0f * Mathf.Cos( angularStart * Mathf.Deg2Rad );
            Vector3 position = new Vector3( x, 0.0f, z );
            Quaternion rotation = Quaternion.Euler( 0.0f, angularStart, 0.0f );

            object prefabName;
            if ( !PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue( PUNGameData.PLAYER_PREFAB_NAME, out prefabName ) )
            {
                prefabName = "Tanks/0";
            }

            PhotonNetwork.Instantiate( (string)prefabName, position, rotation, 0 );

            if ( PhotonNetwork.IsMasterClient )
            {
                //StartCoroutine( SpawnAsteroid() );
                Debug.Log( "Start Game Logic!!!" );
            }
        }

        //private IEnumerator SpawnAsteroid()
        //{
        //    while ( true )
        //    {
        //        yield return new WaitForSeconds( Random.Range( PUNGameData.ASTEROIDS_MIN_SPAWN_TIME, PUNGameData.ASTEROIDS_MAX_SPAWN_TIME ) );

        //        Vector2 direction = Random.insideUnitCircle;
        //        Vector3 position = Vector3.zero;

        //        if ( Mathf.Abs(direction.x) > Mathf.Abs( direction.y ) )
        //        {
        //            // Make it appear on the left/right side
        //            position = new Vector3( Mathf.Sign(direction.x) * Camera.main.orthographicSize * Camera.main.aspect, 0, direction.y * Camera.main.orthographicSize );
        //        }
        //        else
        //        {
        //            // Make it appear on the top/bottom
        //            position = new Vector3( direction.x * Camera.main.orthographicSize * Camera.main.aspect, 0, Mathf.Sign(direction.y) * Camera.main.orthographicSize );
        //        }

        //        // Offset slightly so we are not out of screen at creation time (as it would destroy the asteroid right away)
        //        position -= position.normalized * 0.1f;


        //        Vector3 force = -position.normalized * 1000.0f;
        //        Vector3 torque = Random.insideUnitSphere * Random.Range( 500.0f, 1500.0f );
        //        object[] instantiationData = {force, torque, true};

        //        PhotonNetwork.InstantiateSceneObject( "BigAsteroid", position, Quaternion.Euler( Random.value * 360.0f, Random.value * 360.0f, Random.value * 360.0f ), 0, instantiationData );
        //    }
        //}

        private bool CheckAllPlayerLoadedLevel()
        {
            foreach ( Player p in PhotonNetwork.PlayerList )
            {
                object playerLoadedLevel;

                if ( p.CustomProperties.TryGetValue( PUNGameData.PLAYER_LOADED_LEVEL, out playerLoadedLevel ) )
                {
                    if ( (bool) playerLoadedLevel )
                    {
                        continue;
                    }
                }

                return false;
            }

            return true;
        }

        private void CheckEndOfGame()
        {
            bool allDestroyed = true;

            foreach ( Player p in PhotonNetwork.PlayerList )
            {
                object lives;
                if ( p.CustomProperties.TryGetValue( PUNGameData.PLAYER_LIVES, out lives ) )
                {
                    if ((int) lives > 0)
                    {
                        allDestroyed = false;
                        break;
                    }
                }
            }

            if ( allDestroyed )
            {
                if ( PhotonNetwork.IsMasterClient )
                {
                    StopAllCoroutines();
                }

                string winner = "";
                int score = -1;

                foreach ( Player p in PhotonNetwork.PlayerList )
                {
                    if ( p.GetScore() > score )
                    {
                        winner = p.NickName;
                        score = p.GetScore();
                    }
                }

                StartCoroutine( EndOfGame( winner, score ) );
            }
        }

        private IEnumerator EndOfGame( string winner, int score )
        {
            float timer = 5.0f;

            while (timer > 0.0f)
            {
                InfoText.text = string.Format( "Player {0} won with {1} points.\n\n\nReturning to login screen in {2} seconds.", winner, score, timer.ToString( "n2" ) );

                yield return new WaitForEndOfFrame();

                timer -= Time.deltaTime;
            }

            PhotonNetwork.LeaveRoom();
        }

        #endregion
    }
}