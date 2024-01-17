using System;
using System.Collections.Generic;
using HegaCore;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace HegaCore
{



    public class DataManager : SingletonBehaviour<DataManager>
    {
        private Dictionary<string, WorkoutData> workoutDict;
        private Dictionary<string, WorkoutTextData> workoutTextDict;
        private Dictionary<string, WorkoutLiteData> workoutLiteDict;

        private Dictionary<Type, Dictionary<string, Type>> DataDict;

        protected override void OnAwake()
        {
            base.OnAwake();
            DataDict = new Dictionary<Type, Dictionary<string, Type>>();
            //LoadLevelsTable();
            workoutDict = new Dictionary<string, WorkoutData>();
            workoutTextDict = new Dictionary<string, WorkoutTextData>();
            workoutLiteDict = new Dictionary<string, WorkoutLiteData>();
        }

        #region R18

        public void GetWorkoutLiteData(string workoutLiteID, Action<WorkoutLiteData> callback)
        {
            if (workoutLiteDict.ContainsKey(workoutLiteID))
            {
                callback(workoutLiteDict[workoutLiteID]);
                return;
            }

            //Load Work Out Lite Data by Addressable
            AddressablesManager.LoadAsset<TextAsset>(workoutLiteID, ((s, asset) =>
            {
                WorkoutLiteData newWorkoutLite = new WorkoutLiteData();
                newWorkoutLite.Load(asset);
                workoutLiteDict.Add(workoutLiteID, newWorkoutLite);
                callback(newWorkoutLite);
            }));
        }

        public void GetWorkoutData(string workoutID, Action<WorkoutData, WorkoutTextData> callback)
        {
            if (workoutDict.ContainsKey(workoutID) && workoutTextDict.ContainsKey(workoutID))
            {
                callback(workoutDict[workoutID], workoutTextDict[workoutID]);
                return;
            }

            //Load Work Out Data by Addressable
            AddressablesManager.LoadAsset<TextAsset>(workoutID, ((s, asset) =>
            {
                WorkoutData newWorkout = new WorkoutData();
                newWorkout.Load(asset);
                workoutDict.Add(workoutID, newWorkout);

                GetWorkoutTextData(workoutID, data => { callback(newWorkout, data); });
            }));
        }

        private void GetWorkoutTextData(string workoutID, Action<WorkoutTextData> callback)
        {
            if (workoutTextDict.ContainsKey(workoutID))
            {
                callback(workoutTextDict[workoutID]);
                return;
            }

            //Load Work Out Text Data by Addressable
            AddressablesManager.LoadAsset<TextAsset>(workoutID + "_Text", ((s, asset) =>
            {
                WorkoutTextData newWorkoutText = new WorkoutTextData();
                newWorkoutText.Load(asset);
                workoutTextDict.Add(workoutID, newWorkoutText);
                callback(newWorkoutText);
            }));
        }

        #endregion

        public Camera live2DCamera;

        /// <summary>
        /// R DLC
        /// </summary>
        public bool DarkLord;// { get; private set; }

        /// <summary>
        /// ACHIEVEMENT
        /// </summary>
        public bool OverLord;// { get; private set; }

        /// <summary>
        /// DEV CHEAT
        /// </summary>
        public bool Daemon;// { get; private set; }

        public static GameDataContainer DataContainer { get; private set; }
        public static GameSettings GameSettings { get; private set; }

        public string CurCharID => $"Char{DataContainer.Player.CurCharIndex + 1}Stand";

        public void SetConfig(bool dl, bool ol, bool dm, GameSettings setting, GameDataContainer container)
        {
            DarkLord = dl;
            OverLord = ol;
            Daemon = dm;
            GameSettings = setting;
            DataContainer = container;
        }
    }
}