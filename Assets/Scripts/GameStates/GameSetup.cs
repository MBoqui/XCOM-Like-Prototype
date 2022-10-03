using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grid = GridSystem.Grid;
using GridSystem;

namespace GameStates
{
    public class GameSetup : State
    {
        public GameSetup(StateMachine machine) : base(machine)
        {
            
        }


        public override void Enter()
        {
            GameSetupMenu.Instance.Enable();
        }


        public override void Exit()
        {
            GameSetupMenu.Instance.Disable();
            GameManager.Instance.SetupMatch();

            machine.GoToNextTurn(0);
        }
    }
}
