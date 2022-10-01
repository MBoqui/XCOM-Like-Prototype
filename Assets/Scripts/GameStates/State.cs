using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameStates
{
    public abstract class State
    {
        protected StateMachine machine;


        public State(StateMachine machine)
        {
            this.machine = machine;
        }


        public virtual void Enter()
        {

        }


        public virtual void Exit()
        {

        }


        public virtual void Execute()
        {

        }
    }
}
