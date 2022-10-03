using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GridSystem;

namespace GameStates
{
    public class PlayerTurn : State
    {
        int playerIndex;

        Tank selectedUnit;
        Tank targetUnit;
        List<Vector2Int> path;
        Action action;

        public PlayerTurn(int playerIndex, StateMachine machine) : base(machine)
        {
            this.playerIndex = playerIndex;
        }


        public override void Enter()
        {
            PlayerTurnMenu.Instance.Enable();
        }


        public override void Exit()
        {
            PlayerTurnMenu.Instance.Disable();
            machine.GoToNextTurn(playerIndex);
        }


        public override void Execute()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            Vector3 worldMousePosition = Boqui.Utils.GetMouseWorldPosition();

            Vector2Int? targetlocation = machine.grid.GetGridPosition(worldMousePosition);

            action = ComputeAction(targetlocation);

            ShowTips();

            HandleCommand();
        }


        Action ComputeAction(Vector2Int? targetlocation)
        {
            if (targetlocation == null) return Action.None;

            targetUnit = GetUnitAt((Vector2Int)targetlocation);

            if (targetUnit != null)
            {
                if (targetUnit.playerIndex == playerIndex) return Action.Select;
            }

            if (selectedUnit != null)
            {
                if (targetUnit != null)
                {
                    if (targetUnit.playerIndex != playerIndex)
                    {
                        selectedUnit.CalculateHitChance(targetUnit);
                        return Action.Attack;
                    }
                }

                path = machine.grid.FindPath(selectedUnit.gridPosition, (Vector2Int)targetlocation);
                return Action.Move;
            }

            return Action.None;
        }

        Tank GetUnitAt(Vector2Int targetlocation)
        {
            GridElement element = machine.grid.GetGridElement(targetlocation);
            if (element == null) return null;

            GridAgent agent = element.GetGridAgent();
            if (agent == null) return null;

            return agent.GetComponent<Tank>();
        }


        void ShowTips()
        {
            //show hit chance, path, ap spent on action
        }

        void HandleCommand()
        {
            if (!Input.GetMouseButtonDown(0)) return;

            switch (action){
                case Action.Select:
                    selectedUnit = targetUnit;
                    break;
                case Action.Move:
                    TryMove();
                    break;
                case Action.Attack:
                    selectedUnit.Attack(targetUnit);
                    break;
            }
        }

        void TryMove()
        {
            if (selectedUnit == null) return;
            if (path == null) return;

            selectedUnit.SetMovePath(path);
        }

        enum Action {
            None,
            Select,
            Move,
            Attack
        }
    }
}
