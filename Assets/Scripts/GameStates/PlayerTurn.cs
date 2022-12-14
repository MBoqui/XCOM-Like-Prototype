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
        int pathCost;

        float hitChance;

        Action action;

        public PlayerTurn(int playerIndex, StateMachine machine) : base(machine)
        {
            this.playerIndex = playerIndex;
        }


        //State Methods
        public override void Enter()
        {
            PlayerTurnMenu.Instance.Enable(playerIndex);
            UnitManager.Instance.RefreshUnits(playerIndex);

            Vector2Int cameraPosition = UnitManager.Instance.GetPlayerFristUnitPosition(playerIndex);
            CameraController.Instance.JumpTo(new Vector3(cameraPosition.x, 0, cameraPosition.y));
        }


        public override void Exit()
        {
            if (IsUnitBusy()) return;
            PlayerTurnMenu.Instance.Disable();
            machine.GoToNextTurn(playerIndex);
        }


        public override void Execute()
        {
            if (IsUnitBusy()) return;
            if (EventSystem.current.IsPointerOverGameObject()) return;

            Vector3 worldMousePosition = Boqui.Utils.GetMouseWorldPosition();

            Vector2Int? targetlocation = machine.grid.GetGridPosition(worldMousePosition);

            action = ComputeAction(targetlocation);

            ShowTips();

            HandleCommand();
        }


        //private Methods
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
                        hitChance = selectedUnit.CalculateHitChance(targetUnit);
                        return Action.Attack;
                    }
                }

                (pathCost, path) = machine.grid.FindPath(selectedUnit.gridPosition, (Vector2Int)targetlocation);
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
            if (targetUnit == null) hitChance = -1;
            PlayerTurnMenu.Instance.SetInfo(selectedUnit, pathCost, hitChance);
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

            selectedUnit.SetMovePath(pathCost, path);
        }


        bool IsUnitBusy()
        {
            if (selectedUnit != null)
            {
                if (selectedUnit.IsBusy()) return true;
            }

            return false;
        }


        //Enums
        public enum Action {
            None,
            Select,
            Move,
            Attack
        }
    }
}
