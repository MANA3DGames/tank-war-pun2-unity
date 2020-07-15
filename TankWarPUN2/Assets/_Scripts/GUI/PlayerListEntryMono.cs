// ------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerListEntry.cs" company="Exit Games GmbH">
//   Part of: Asteroid Demo,
// </copyright>
// <summary>
//  Player List Entry
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Photon.Pun;
using TMPro;

namespace MANA3DGames
{
    public class PlayerListEntryMono : MonoBehaviour
    {
        [Header("UI References")]
        public TextMeshProUGUI PlayerNameText;

        public Image PlayerColorImage;
        public Button PlayerReadyButton;
        public Image PlayerReadyImage;

        private int ownerId;
        private bool isPlayerReady;

        #region UNITY

        public void OnEnable()
        {
            PlayerNumbering.OnPlayerNumberingChanged += OnPlayerNumberingChanged;
        }

        public void Start()
        {
            if ( PhotonNetwork.LocalPlayer.ActorNumber != ownerId )
            {
                PlayerReadyButton.gameObject.SetActive( false );
            }
            else
            {
                Hashtable initialProps = new Hashtable() { { PUNGameData.PLAYER_READY, isPlayerReady }, { PUNGameData.PLAYER_LIVES, PUNGameData.PLAYER_MAX_LIVES } };
                PhotonNetwork.LocalPlayer.SetCustomProperties( initialProps );
                PhotonNetwork.LocalPlayer.SetScore( 0 );

                PlayerReadyButton.onClick.AddListener( () =>
                {
                    isPlayerReady = !isPlayerReady;
                    SetPlayerReady(isPlayerReady);

                    Hashtable props = new Hashtable() { { PUNGameData.PLAYER_READY, isPlayerReady } };
                    PhotonNetwork.LocalPlayer.SetCustomProperties( props );

                    if ( PhotonNetwork.IsMasterClient )
                    {
                        FindObjectOfType<AppManager>().CheckToShowStartGameButton();
                    }
                } );
            }
        }

        public void OnDisable()
        {
            PlayerNumbering.OnPlayerNumberingChanged -= OnPlayerNumberingChanged;
        }

        #endregion

        public void Initialize( int playerId, string playerName, bool isPlayerReady )
        {
            ownerId = playerId;
            PlayerNameText.text = playerName;
            SetPlayerReady( isPlayerReady );
        }

        private void OnPlayerNumberingChanged()
        {
            foreach ( Player p in PhotonNetwork.PlayerList )
            {
                if ( p.ActorNumber == ownerId )
                {
                    PlayerColorImage.color = PUNGameData.GetColor( p.GetPlayerNumber() );
                }
            }
        }

        public void SetPlayerReady( bool playerReady )
        {
            PlayerReadyButton.GetComponentInChildren<Text>().text = playerReady ? "Ready!" : "Ready?";
            PlayerReadyImage.enabled = playerReady;
        }
    }
}