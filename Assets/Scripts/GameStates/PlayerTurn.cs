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
            Vector3 worldMousePosition = Boqui.Utils.GetMouseWorldPosition(LayerMask.GetMask("Ground"));

            if (Input.GetMouseButtonDown(0))
            {
                TreeManager.Instance.TryAddTree(worldMousePosition);
            }

            if (Input.GetMouseButtonDown(1))
            {
                machine.grid.TryDestroyObject(worldMousePosition);
            }

            if (Input.GetMouseButtonDown(2))
            {
                Vector2Int? targetlocation = machine.grid.GetGridPosition(worldMousePosition);

                if (targetlocation == null) return;

                List<Vector2Int> path = machine.grid.FindPath(machine.tank.GetGridPosition(), (Vector2Int)targetlocation);

                if (path != null)
                {
                    machine.tank.SetMovePath(path);

                    for (int i = 0; i < path.Count - 1; i++)
                    {
                        Debug.DrawLine(new Vector3(path[i].x + 0.5f, 0, path[i].y + 0.5f), new Vector3(path[i + 1].x + 0.5f, 0, path[i + 1].y + 0.5f), Color.red, 5f);
                    }
                }
            }
        }
    }
}
