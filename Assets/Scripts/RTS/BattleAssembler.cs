﻿using RTS.Inputs.Mouse;
using RTS.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS
{
    public class BattleAssembler : MonoBehaviour
    {
        [System.Serializable]
        public class Settings
        {
            public MouseBattleInput.Settings mouseInput;
            public BattleSceneManager.Settings sceneManager;
            public AI.AIStrategy strategy;

            public GameObject BaseUnitPrefab;
        }


        public Settings settings;
        public Team playerTeam;
        public Team opposingTeam;
        public BattleUI battleUI;

        PlayerCommandsController playerCommands;
        MouseBattleInput mouseInput;
        Battle battleManager;
        BattleSceneManager sceneManager;



        void Awake()
        {
            RTS.World.UnitBehavior.UnitAssembler.UnitPrefab = settings.BaseUnitPrefab;

            var playerCommandsObj = new GameObject("PlayerCommandsController");
            playerCommandsObj.transform.parent = this.transform;
            playerCommands = playerCommandsObj.AddComponent<PlayerCommandsController>();
            playerCommands.Team = playerTeam;

            battleManager = new Battle(playerTeam, opposingTeam);

            var mouseInputObj = new GameObject("MouseInputManager");
            mouseInputObj.transform.parent = playerCommands.transform;
            mouseInput = mouseInputObj.AddComponent<MouseBattleInput>();
            mouseInput.settings = settings.mouseInput;
            mouseInput.controller = playerCommands;

            sceneManager = InstancingUtils.CreateWithPreemptiveExecution<BattleSceneManager>((manager) =>
            {
                manager.battle = battleManager;
                manager.battleUI = battleUI;
                manager.playerCommands = playerCommands;
                manager.settings = settings.sceneManager;
            });
            sceneManager.transform.parent = transform;

            var squadAIHandler = InstancingUtils.CreateWithPreemptiveExecution<SquadAIHandler>((handler) =>
            {
                handler.strategy = settings.strategy;
            });
            squadAIHandler.transform.parent = transform;

        }
    }
}