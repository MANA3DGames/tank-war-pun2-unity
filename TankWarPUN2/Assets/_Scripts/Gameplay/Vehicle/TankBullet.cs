using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;

namespace MANA3DGames
{
    public class TankBullet : MonoBehaviour
    {
        public Player Owner { get; private set; }
        public float Damage { get; private set; }

        public void Start()
        {
            Destroy( gameObject, 3.0f );
        }


        public void OnTriggerEnter( Collider other )
        {
            GameObject go = other.gameObject;

            if ( go.CompareTag( "Player" ) )
            {
                PhotonView otherView = go.GetComponent<PhotonView>();
                if ( Owner != otherView.Controller )
                {
                    // +1 to the Owner of the bullet.
                    Owner.AddScore( 1 );
                    otherView.RPC( "ApplyDamage", RpcTarget.AllViaServer, Damage );
                }
            }
            else if ( go.CompareTag( "Solid" ) )
            {

            }

            // Destroy bullet's gameObject.
            Destroy( gameObject );
        }

        public void InitializeBullet( Player owner, Vector3 originalDirection, float bulletSpeed, float damage, float lag )
        {
            Owner = owner;
            Damage = damage;

            transform.forward = originalDirection;

            Rigidbody rbody = GetComponent<Rigidbody>();
            rbody.velocity = originalDirection * bulletSpeed;
            rbody.position += rbody.velocity * lag;
        }
    }
}