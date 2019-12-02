using Assets.Scripts.Project.Enums;
using strange.extensions.mediation.impl;
using System;
using UnityEngine;

namespace Assets.Scripts.Project.Manager.Cam
{
    public class CamManager : EventView
    {
        public RV_Grid Grid;

        public Vector3 offset;

        public float smoothTime = .5f;

        private Vector3 _movePoint;

        private Vector3 _moveVelocity;

        private float _zoomSize;

        public Camera _cam;

        private Bounds _bounds;

        private void LateUpdate()
        {
            FixCam();
        }

        internal void FixCam()
        {
            CalculateMoveAndZoom();

            Move();

            Zoom();
        }

        private void Zoom()
        {
            float orthographicSize = _cam.orthographicSize;

            //Debug.Log("--------------------");
            //Debug.Log(_bounds);
            //Debug.Log(_bounds.size);
            //Vector3 topRight = new Vector3(_bounds.center.x + _bounds.size.x, _bounds.center.y, 0f);
            //Vector3 topRightAsViewport = _cam.WorldToViewportPoint(topRight);
            ////Debug.Log(orthographicSize);

            //if (topRightAsViewport.x >= topRightAsViewport.y)
            //    orthographicSize = Mathf.Abs(_bounds.size.x) / _cam.aspect / 2f;
            //else
            //    orthographicSize = Mathf.Abs(_bounds.size.y) / 2f;

            //float newZoom = _zoomSize;// Mathf.Lerp(5, 20, _zoomSize);

            _cam.orthographicSize = _zoomSize + 1;

            //_cam.orthographicSize = Mathf.Clamp(Mathf.Lerp(_cam.orthographicSize, orthographicSize, Time.deltaTime * 20f), 8f, Mathf.Infinity);
        }

        private void Move()
        {
            Vector3 newPosition = _movePoint + offset;

            transform.position = newPosition;

            //transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref _moveVelocity, smoothTime);
        }

        /// //////////////////////////////////////////////////////////////<summary>
        /// /////////////////////// CalculateMoveAndZoom //////////////////////////
        /// </summary>/////////////////////////////////////////////////////////////
        private void CalculateMoveAndZoom()
        {
            if (Grid.HexList.Count == 1)
            {
                _movePoint = Grid.HexList[0].position;

                _zoomSize = 10;
            }
            else
            {
                _bounds = new Bounds(Grid.HexList[0].position, Vector3.zero);

                for (int i = 0; i < Grid.HexList.Count; i++)
                {
                    _bounds.Encapsulate(Grid.HexList[i].position);
                }
                _movePoint = _bounds.center;

                _zoomSize = _bounds.size.x;
            }
        }
    }
}
