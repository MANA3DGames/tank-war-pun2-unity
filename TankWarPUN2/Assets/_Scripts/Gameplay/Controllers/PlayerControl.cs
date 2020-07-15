using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;


namespace MANA3DGames
{
    public class PlayerControl : MonoBehaviour
    {
        private GameObject  uGameObject;
        private Transform   uTransform;
        private Transform   uCamTransform;
        private Rigidbody   uRigidBody;
        private PhotonView  cPhotonView;
        private Vehicle     cVehicle;


        private void Start()
        {
            uGameObject = gameObject;
            uTransform  = uGameObject.transform;
            uRigidBody  = uGameObject.GetComponent<Rigidbody>();
            cVehicle    = uGameObject.GetComponent<Vehicle>();
            cPhotonView = uGameObject.GetComponent<PhotonView>();

            if ( cPhotonView.IsMine )
                uCamTransform = Camera.main.transform;
        }

        private void Update()
        {
            if ( !cPhotonView.IsMine )
                return;

            cVehicle.UpdateMe( GetMove(), GetLookTarget(), GetShoot() );
        }

        private void FixedUpdate()
        {
            if ( !cPhotonView.IsMine )
                return;

            cVehicle.FixedUpdateMe( GetMove() );
            UpdateCamera();
        }



        private Vector3 GetMove()
        {
            float h = Input.GetAxis( "Horizontal" );
            float v = Input.GetAxis( "Vertical" );
            return new Vector3( h, 0, v );
        }

        private Vector3 GetLookTarget()
        {
            Plane plane = new Plane( Vector3.up, Vector3.up * 2 );
            Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );

            Vector3 targetPos = Vector3.zero;
            float point = 2.0f;

            if ( plane.Raycast( ray, out point ) )
                targetPos = ray.GetPoint( point );

            return targetPos;
        }

        private bool GetShoot()
        {
            return Input.GetMouseButtonDown( 0 );
        }



        private void UpdateCamera()
        {
            Vector3 camTargetPos = uTransform.position;
            camTargetPos.x += uRigidBody.velocity.x * 4;
            camTargetPos.y = 45.0f;
            camTargetPos.z -= 35.0f;
            camTargetPos.z += ( uRigidBody.velocity.z < 0.0f ? uRigidBody.velocity.z * 7 : uRigidBody.velocity.z * 3 );
            uCamTransform.position = Vector3.Lerp( uCamTransform.position, camTargetPos, 0.5f * Time.deltaTime );
        }
    }
}