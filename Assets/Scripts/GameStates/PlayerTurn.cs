using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grid = GridSystem.Grid;
using GridSystem;

namespace GameStates
{
    public class PlayerTurn : State
    {
        int playerIndex;

        GridAgent selectedUnit;
        List<Vector2Int> path;

        public PlayerTurn(int playerIndex, StateMachine machine) : base(machine)
        {
            this.playerIndex = playerIndex;
        }


        public override void Enter()
        {

        }


        public override void Exit()
        {
            machine.GoToNextTurn(playerIndex);
        }


        public override void Execute()
        {
            Vector3 worldMousePosition = Boqui.Utils.GetMouseWorldPosition();

            Vector2Int? targetlocation = machine.grid.GetGridPosition(worldMousePosition);

            if (targetlocation == null) return;

            if (Input.GetMouseButtonDown(0))
            {
                GridAgent targetGridAgent = machine.grid.GetGridElement((Vector2Int)targetlocation).GetGridAgent();

                if (targetGridAgent != null) //select units
                {
                    selectedUnit = targetGridAgent;
                }

                TryMove((Vector2Int)targetlocation);
            }
        }

        void TryMove(Vector2Int targetlocation)
        {
            if (selectedUnit == null) return;

            path = machine.grid.FindPath(selectedUnit.gridPosition, targetlocation);

            if (path != null) //move
            {
                selectedUnit.SetMovePath(path);
            }
        }
    }
}
