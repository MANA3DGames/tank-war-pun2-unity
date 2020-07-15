using System.Collections;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace MANA3DGames
{
    public class Vehicle : MonoBehaviour
    {
        #region [Global Variables]

        public GameObject  BulletPrefab;

        private GameObject          uGameObject;
        private Transform           uTransform;
        private Transform           uTerretTransform;
        private Transform           uChassisTransform;
        private Transform           uBulletSpawnPointTransform;
        private Rigidbody           uRigidBody;
        private Collider            uCollider;
        private Renderer[]          uRenderers;
        private PhotonView          cPhotonView;

        private bool                gDoubleFire;
        private bool                gIsDestroyed;

        private float               gHealth;
        private float               gShootingTimer;

        protected float             gTurretRotateSpeed;
        protected float             gChassisRotateSpeed;
        protected float             gMovementSpeed;

        protected float             gBulletSpeed = 40.0f;
        protected float             gBulletDamage = 20.0f;

        #endregion


        #region [MonoBehaviour]

        protected virtual void Start()
        {
            uGameObject = gameObject;
            uTransform = uGameObject.transform;
            uTerretTransform = uTransform.Find( "Terret" );
            uBulletSpawnPointTransform = uTerretTransform.Find( "BulletSpawnPoint" );
            uChassisTransform = uTransform.Find( "Chassis" );
            cPhotonView = uGameObject.GetComponent<PhotonView>();
            uRigidBody = uGameObject.GetComponent<Rigidbody>();
            uCollider = uGameObject.GetComponent<Collider>();
            uRenderers  = uGameObject.GetComponentsInChildren<Renderer>();

            Reset();

            if ( cPhotonView.IsMine )
                uGameObject.AddComponent<PlayerControl>();
        }

        public virtual void UpdateMe( Vector3 move, Vector3 targetPos, bool shoot )
        {
            if ( gIsDestroyed )
                return;

            UpdateTurret( targetPos );
            UpdateChassis( move );
            UpdateShoot( shoot );
        }

        public virtual void FixedUpdateMe( Vector3 move )
        {
            if ( !cPhotonView.IsMine || gIsDestroyed )
                return;

            UpdateMove( move );
        }

        //public void OnCollisionEnter( Collision collision )
        //{
        //    if ( gIsDestroyed )
        //        return;

        //    var go = collision.gameObject;
        //    if ( go.CompareTag( "Bullet" ) )
        //    {
        //        if ( cPhotonView.IsMine )
        //        {
        //            TankBullet bullet = go.GetComponent<TankBullet>();
        //            if ( bullet.Owner != cPhotonView.Controller )
        //            {
        //                // +1 to the Owner of the bullet.
        //                bullet.Owner.AddScore( 1 );
        //                cPhotonView.RPC( "ApplyDamage", RpcTarget.AllViaServer, bullet.Damage );
        //            }

        //            //go.GetComponent<PhotonView>().RPC( "DestroySpaceship", RpcTarget.All );
        //            //DestroyAsteroidGlobally();
        //        }

        //        // Destroy bullet's gameObject.
        //        Destroy( go );
        //    }
        //}

        #endregion

        #region [Utilites Funcitons]

        public bool GetIsMine()
        {
            return cPhotonView.IsMine;
        }

        protected virtual void Reset()
        {
            gDoubleFire = false;
            gIsDestroyed = false;

            gHealth = 100.0f;
            gShootingTimer = 0.0f;

            //Destruction.Stop();
        }

        #endregion

        #region [Update Functions]

        protected virtual void UpdateMove( Vector3 move )
        {
            Vector3 moveDirection = move * gMovementSpeed * Time.deltaTime;
            uRigidBody.velocity = moveDirection;
        }

        protected virtual void UpdateChassis( Vector3 move )
        {
            if ( move.sqrMagnitude > 0.25f )
            {
                Quaternion desiredRot = Quaternion.LookRotation( move );
                uChassisTransform.rotation = Quaternion.Slerp( uChassisTransform.rotation, desiredRot, gChassisRotateSpeed * Time.deltaTime );
            }
        }

        protected virtual void UpdateTurret( Vector3 targetPos )
        {

            Vector3 direction = targetPos - uTransform.position;
            Quaternion newRotation = Quaternion.LookRotation( direction, Vector3.up );
            Vector3 euler = newRotation.eulerAngles;
            euler.x = 0.0f;
            euler.z = 0.0f;
            newRotation = Quaternion.Euler( euler );
            uTerretTransform.rotation = Quaternion.Slerp( uTerretTransform.rotation, newRotation, gTurretRotateSpeed * Time.deltaTime );
        }

        protected virtual void UpdateShoot( bool shoot )
        {
            if ( shoot && gShootingTimer <= 0.0 )
            {
                gShootingTimer = 0.2f;
                cPhotonView.RPC( "Fire", RpcTarget.AllViaServer );
            }

            if ( gShootingTimer > 0.0f )
            {
                gShootingTimer -= Time.deltaTime;
            }
        }

        #endregion

        #region [PunRPC]

        [PunRPC]
        public virtual void Fire( PhotonMessageInfo info )
        {
            float lag = (float)( PhotonNetwork.Time - info.SentServerTime );

            if ( gDoubleFire )
            {
                /** Use this if you want to fire two bullets at once **/
                //Vector3 baseX = rotation * Vector3.right;
                //Vector3 baseZ = rotation * Vector3.forward;

                //Vector3 offsetLeft = -1.5f * baseX - 0.5f * baseZ;
                //Vector3 offsetRight = 1.5f * baseX - 0.5f * baseZ;

                //bullet = Instantiate(BulletPrefab, rigidbody.position + offsetLeft, Quaternion.identity) as GameObject;
                //bullet.GetComponent<Bullet>().InitializeBullet(photonView.Owner, baseZ, Mathf.Abs(lag));
                //bullet = Instantiate(BulletPrefab, rigidbody.position + offsetRight, Quaternion.identity) as GameObject;
                //bullet.GetComponent<Bullet>().InitializeBullet(photonView.Owner, baseZ, Mathf.Abs(lag));
            }
            else
            {
                /** Use this if you want to fire one bullet at a time **/
                var bullet = Instantiate( BulletPrefab, uBulletSpawnPointTransform.position, uBulletSpawnPointTransform.rotation ) as GameObject;
                bullet.GetComponent<TankBullet>().InitializeBullet( cPhotonView.Owner, ( uBulletSpawnPointTransform.rotation * Vector3.forward ), gBulletSpeed, gBulletDamage, Mathf.Abs( lag ) );
            }
        }

        [PunRPC]
        public virtual void ApplyDamage( float damage )
        {
            gHealth -= damage;
            if ( gHealth <= 0.0f )
                cPhotonView.RPC( "DestroyMe", RpcTarget.AllViaServer );

            Debug.Log( "Health: " + gHealth );
        }

        [PunRPC]
        public virtual void DestroyMe()
        {
            if ( gIsDestroyed )
                return;

            uRigidBody.velocity = Vector3.zero;
            uRigidBody.angularVelocity = Vector3.zero;

            uCollider.enabled = false;

            for ( int i = 0; i < uRenderers.Length; i++ )
                uRenderers[i].enabled = false;

            gIsDestroyed = true;

            //Destruction.Play();

            if ( cPhotonView.IsMine )
            {
                object lives;
                if ( PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue( PUNGameData.PLAYER_LIVES, out lives ) )
                {
                    PhotonNetwork.LocalPlayer.SetCustomProperties( new Hashtable { { PUNGameData.PLAYER_LIVES, ( (int)lives <= 1 ) ? 0 : ( (int)lives - 1 ) } } );

                    if ( ( (int)lives ) > 1 )
                    {
                        StartCoroutine( "WaitForRespawn" );
                    }
                }
            }
        }

        private IEnumerator WaitForRespawn()
        {
            yield return new WaitForSeconds( PUNGameData.PLAYER_RESPAWN_TIME );
            cPhotonView.RPC( "RespawnSpaceship", RpcTarget.AllViaServer );
        }

        [PunRPC]
        public virtual void RespawnSpaceship()
        {
            uCollider.enabled = true;
            for ( int i = 0; i < uRenderers.Length; i++ )
                uRenderers[i].enabled = true;

            Reset();
        }

        #endregion
    }
}
