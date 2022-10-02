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
            UnitManager.Instance.GenerateArmies();
            TreeManager.Instance.GenerateRandomTrees();

            Exit();
        }


        public override void Exit()
        {
            machine.GoToNextTurn(0);
        }
    }
}
