using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Autohand.Demo {

    [HelpURL("https://app.gitbook.com/s/5zKO0EvOjzUDeT2aiFk3/auto-hand/controller-input")]
    public class XRHandPlayerControllerLink_Network : NetworkBehaviour
    {
        public XRHandControllerLink moveController;
        public XRHandControllerLink turnController;

        [Header("Input")]
        public Common2DAxis moveAxis;
        public Common2DAxis turnAxis;


        private NetInput_XR accumulatedInput;
        bool resetInput;
        AutoHandPlayer player;
        void OnEnable()
        {
            player = GetComponent<AutoHandPlayer>();
        }
        void Update(){
            if(Object.HasInputAuthority)
            {
                player.Move(moveController.GetAxis2D(moveAxis));
                player.Turn(turnController.GetAxis2D(turnAxis).x);

            }
        }
        void FixedUpdate(){
            if(Object.HasInputAuthority)
            {
                player.Move(moveController.GetAxis2D(moveAxis));
            }
        }

    }
}