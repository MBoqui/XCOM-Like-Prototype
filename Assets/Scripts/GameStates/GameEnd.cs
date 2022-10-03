using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grid = GridSystem.Grid;
using GridSystem;

namespace GameStates
{
    public class GameEnd : State
    {
        int playerIndex;
        public GameEnd(StateMachine machine, int playerIndex) : base(machine)
        {
            this.playerIndex = playerIndex;
        }


        public override void Enter()
        {
            PlayerTurnMenu.Instance.Disable();
            GameEndScreen.Instance.Enable(playerIndex);
        }


        public override void Exit()
        {
            GameEndScreen.Instance.Disable();

            GameManager.Instance.NewGame();
        }
    }
}
