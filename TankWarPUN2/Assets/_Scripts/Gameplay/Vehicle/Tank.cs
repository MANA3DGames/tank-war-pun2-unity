using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace MANA3DGames
{
    public class Tank : Vehicle
    {
        private Animator uTerretAnimator;


        protected override void Start()
        {
            base.Start();

            uTerretAnimator = transform.Find( "Terret" ).Find( "Tank_tower" ).GetComponent<Animator>();

            gTurretRotateSpeed = 3.0f;
            gChassisRotateSpeed = 2.0f;
            gMovementSpeed = 200.0f;

            gBulletSpeed = 40.0f;
            gBulletDamage = 20.0f;
    }

        [PunRPC]
        public override void Fire( PhotonMessageInfo info )
        {
            uTerretAnimator.StopPlayback();
            uTerretAnimator.Play( "Shoot", 0, 0.0f );

            base.Fire( info );
        }
    }
}
