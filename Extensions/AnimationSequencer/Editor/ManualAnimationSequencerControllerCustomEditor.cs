#if ANIMATION_SEQUENCER

using System;
using System.Collections.Generic;
using BrunoMikoski.AnimationSequencer;
using DG.DOTweenEditor;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditorInternal;
using UnityEngine;

namespace HegaCore.Editor
{
    [CustomEditor(typeof(ManualAnimationSequencerController))]
    public sealed class ManualAnimationSequencerControllerCustomEditor : UnityEditor.Editor
    {
        private ReorderableList reorderableList;

        private ManualAnimationSequencerController sequencerController;
        private bool isPreviewPlaying;
        private double lastFrameTime;
        private float frameDelta;
        private ManualAnimationSequencerController[] activeSequencers = new ManualAnimationSequencerController[0];

        private static AnimationStepAdvancedDropdown cachedAnimationStepsDropdown;
        private static AnimationStepAdvancedDropdown AnimationStepAdvancedDropdown
        {
            get
            {
                if (cachedAnimationStepsDropdown == null)
                    cachedAnimationStepsDropdown = new AnimationStepAdvancedDropdown(new AdvancedDropdownState());
                return cachedAnimationStepsDropdown;
            }
        }

        private void OnEnable()
        {
            this.sequencerController = this.target as ManualAnimationSequencerController;
            this.reorderableList = new ReorderableList(this.serializedObject, this.serializedObject.FindProperty("animationSteps"), true, true, true, true);
            this.reorderableList.drawElementCallback += OnDrawAnimationStep;
            this.reorderableList.elementHeightCallback += GetAnimationStepHeight;
            this.reorderableList.onAddDropdownCallback += OnClickToAddNew;
            this.reorderableList.onRemoveCallback += OnClickToRemove;
            this.reorderableList.onReorderCallback += OnListOrderChanged;
            this.reorderableList.drawHeaderCallback += OnDrawerHeader;
            CalculateTotalAnimationTime();
            Repaint();
        }

        private void OnDisable()
        {
            this.reorderableList.drawElementCallback -= OnDrawAnimationStep;
            this.reorderableList.elementHeightCallback -= GetAnimationStepHeight;
            this.reorderableList.onAddDropdownCallback -= OnClickToAddNew;
            this.reorderableList.onRemoveCallback -= OnClickToRemove;
            this.reorderableList.onReorderCallback -= OnListOrderChanged;
            this.reorderableList.drawHeaderCallback -= OnDrawerHeader;
            StopPreview();
        }



        private void OnDrawerHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "Animation Steps", EditorStyles.foldoutHeader);
        }

        private void AddNewAnimationStepOfType(Type targetAnimationType)
        {
            SerializedProperty animationStepsProperty = this.reorderableList.serializedProperty;
            var targetIndex = animationStepsProperty.arraySize;
            animationStepsProperty.InsertArrayElementAtIndex(targetIndex);
            SerializedProperty arrayElementAtIndex = animationStepsProperty.GetArrayElementAtIndex(targetIndex);
            var managedReferenceValue = Activator.CreateInstance(targetAnimationType);
            arrayElementAtIndex.managedReferenceValue = managedReferenceValue;

            //TODO copy from last step would be better here.
            SerializedProperty targetSerializedProperty = arrayElementAtIndex.FindPropertyRelative("target");
            if (targetSerializedProperty != null)
                targetSerializedProperty.objectReferenceValue = (this.serializedObject.targetObject as ManualAnimationSequencerController)?.gameObject;

            this.serializedObject.ApplyModifiedProperties();
        }

        private void OnClickToRemove(ReorderableList list)
        {
            SerializedProperty element = this.reorderableList.serializedProperty.GetArrayElementAtIndex(list.index);
            SerializedPropertyExtensions.ClearPropertyCache(element.propertyPath);
            this.reorderableList.serializedProperty.DeleteArrayElementAtIndex(list.index);
            this.reorderableList.serializedProperty.serializedObject.ApplyModifiedProperties();
        }

        private void OnListOrderChanged(ReorderableList list)
        {
            SerializedPropertyExtensions.ClearPropertyCache(list.serializedProperty.propertyPath);
            list.serializedProperty.serializedObject.ApplyModifiedProperties();
        }

        private void OnClickToAddNew(Rect buttonRect, ReorderableList list)
        {
            AnimationStepAdvancedDropdown.Show(buttonRect, OnNewAnimationStepTypeSelected);
        }

        private void OnNewAnimationStepTypeSelected(AnimationStepAdvancedDropdownItem animationStepAdvancedDropdownItem)
        {
            AddNewAnimationStepOfType(animationStepAdvancedDropdownItem.AnimationStepType);
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            DrawBoxedArea("Settings", DrawSettings);
            DrawBoxedArea("Preview", DrawPreviewControls);
            var wasGUIEnabled = GUI.enabled;
            if (this.sequencerController.IsPlaying)
                GUI.enabled = false;

            this.reorderableList.DoLayoutList();
            GUI.enabled = wasGUIEnabled;
            if (EditorGUI.EndChangeCheck())
                CalculateTotalAnimationTime();
        }

        private void DrawSettings()
        {
            SerializedProperty initializationModeSerializedProperty = this.serializedObject.FindProperty("initializeMode");
            using (var changedCheck = new EditorGUI.ChangeCheckScope())
            {
                EditorGUILayout.PropertyField(initializationModeSerializedProperty);
                if (changedCheck.changed)
                    this.serializedObject.ApplyModifiedProperties();
            }
        }

        private void CalculateTotalAnimationTime()
        {
            SerializedProperty durationSerializedProperty = this.serializedObject.FindProperty("duration");
            if (durationSerializedProperty == null)
                return;

            float sequenceDuration = 0;
            for (var i = 0; i < this.sequencerController.AnimationSteps.Length; i++)
            {
                AnimationStepBase animationStep = this.sequencerController.AnimationSteps[i];
                sequenceDuration += animationStep.Delay + animationStep.Duration;
            }

            if (!Mathf.Approximately(sequenceDuration, durationSerializedProperty.floatValue))
            {
                durationSerializedProperty.floatValue = sequenceDuration;
                this.serializedObject.ApplyModifiedProperties();
            }
        }

        private void DrawPreviewControls()
        {
            if (!this.isPreviewPlaying)
            {
                GameObject sequencerGameObject = this.sequencerController.gameObject;
                EditorGUI.BeginDisabledGroup(!sequencerGameObject.activeSelf || !sequencerGameObject.activeInHierarchy);
                if (GUILayout.Button("Play"))
                {
                    Play();
                }

                EditorGUI.EndDisabledGroup();
            }
            else
            {
                if (GUILayout.Button("Complete"))
                {
                    Complete();
                }

                if (GUILayout.Button("Stop"))
                {
                    StopPreview();
                }
            }
        }

        private void Complete()
        {
            this.sequencerController.Complete();
        }

        private void Play()
        {
            if (!Application.isPlaying)
            {
                EditorApplication.update += EditorUpdate;
                DOTweenEditorPreview.Start();
                FindRelatedAnimationControllers();
                this.sequencerController.OnSequenceFinishedPlayingEvent += StopPreview;
                this.lastFrameTime = EditorApplication.timeSinceStartup;
                this.isPreviewPlaying = true;
            }

            this.sequencerController.PrepareForPlay(true);
            this.sequencerController.Play();
        }

        private void FindRelatedAnimationControllers()
        {
            if (Application.isPlaying)
                return;

            var sequencers = new List<ManualAnimationSequencerController>
            {
                this.sequencerController
            };
            for (var i = 0; i < this.sequencerController.AnimationSteps.Length; i++)
            {
                AnimationStepBase sequencerControllerAnimationStep = this.sequencerController.AnimationSteps[i];
                if (sequencerControllerAnimationStep is PlayManualSequenceAnimationStep playSequenceAnimationStep)
                    sequencers.Add(playSequenceAnimationStep.Target);
            }

            this.activeSequencers = sequencers.ToArray();
        }

        private void StopPreview()
        {
            if (!Application.isPlaying)
            {
                EditorApplication.update -= EditorUpdate;
                DOTweenEditorPreview.Stop(true);
            }

            this.sequencerController.OnSequenceFinishedPlayingEvent -= StopPreview;
            for (var i = 0; i < this.activeSequencers.Length; i++)
            {
                ManualAnimationSequencerController animationSequencerController = this.activeSequencers[i];
                if (animationSequencerController == null)
                    continue;

                animationSequencerController.Stop();
            }

            this.isPreviewPlaying = false;
            Repaint();
        }

        private void EditorUpdate()
        {
            UpdatePreview();
        }

        private void UpdatePreview()
        {
            if (!this.isPreviewPlaying)
                return;

            this.frameDelta = (float)(EditorApplication.timeSinceStartup - this.lastFrameTime);
            this.lastFrameTime = EditorApplication.timeSinceStartup;

            for (var i = 0; i < this.activeSequencers.Length; i++)
            {
                ManualAnimationSequencerController animationSequencerController = this.activeSequencers[i];
                if (animationSequencerController == null)
                    continue;

                animationSequencerController.UpdateStep(this.frameDelta);
            }
        }

        private void DrawBoxedArea(string title, Action additionalInspectorGUI)
        {
            using (new EditorGUILayout.VerticalScope("FrameBox"))
            {
                Rect rect = EditorGUILayout.GetControlRect();
                rect.x -= 4;
                rect.width += 8;
                rect.y -= 4;
                var foldoutHeader = new GUIStyle(EditorStyles.foldoutHeader);

                EditorGUI.LabelField(rect, title, foldoutHeader);
                EditorGUILayout.Space();
                additionalInspectorGUI.Invoke();
            }
        }

        private void OnDrawAnimationStep(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty element = this.reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            SerializedProperty flowTypeSerializedProperty = element.FindPropertyRelative("flowType");

            if (element.TryGetTargetObjectOfProperty(out AnimationStepBase animationStepBase))
            {
                if (animationStepBase.IsPlaying)
                {
                    this.reorderableList.index = index;
                }
            }

            var flowType = (FlowType)flowTypeSerializedProperty.enumValueIndex;

            var baseIdentLevel = EditorGUI.indentLevel;

            var guiContent = new GUIContent(element.displayName);
            if (animationStepBase != null)
                guiContent = new GUIContent(animationStepBase.GetDisplayNameForEditor(index + 1));

            if (flowType == FlowType.Join)
                EditorGUI.indentLevel = baseIdentLevel + 1;

            rect.height = EditorGUIUtility.singleLineHeight;
            rect.x += 10;
            rect.width -= 20;

            EditorGUI.PropertyField(
                rect,
                element,
                guiContent,
                false
            );

            EditorGUI.indentLevel = baseIdentLevel;
        }

        private float GetAnimationStepHeight(int index)
        {
            SerializedProperty element = this.reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            return element.GetPropertyDrawerHeight();

        }
    }
}


#endif