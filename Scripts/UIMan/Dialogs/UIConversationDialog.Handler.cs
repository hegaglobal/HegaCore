using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnuGames;
using UnuGames.MVVM;
using RedBlueGames.Tools.TextTyper;
using VisualNovelData.Data;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace HegaCore.UI
{
    public partial class UIConversationDialog : UIManDialog, IPointerClickHandler
    {
        public static void Show(string id, Action onHideCompleted)
        {
            UIMan.Instance.ShowDialog<UIConversationDialog>(id, onHideCompleted);
        }

        public static void Hide()
        {
            UIMan.Instance.HideDialog<UIConversationDialog>(true);
        }

        [SerializeField, InlineButton(nameof(FindAllCanvases), "Find")]
        private Canvas[] canvases = null;

        [SerializeField, InlineButton(nameof(FindAllRaycasters), "Find")]
        private GraphicRaycaster[] raycasters = null;

        [SerializeField]
        private Panel panelCover = null;

        [SerializeField]
        private ImageSwitcherModule panelBackground = null;

        [SerializeField]
        private Panel panelConversation = null;

        [SerializeField]
        private TMP_Text contentText = null;

        [SerializeField]
        private TextTyper contentTyper = null;

        [SerializeField]
        private TextTyperConfig speedTyperConfig = null;

        [SerializeField]
        private string avatarDefault = "";

        [SerializeField, PropertySpace(4)]
        private ActorView[] actorViews = new ActorView[0];

        [SerializeField]
        private Transform positionLeft = null;

        [SerializeField]
        private Transform positionRight = null;

        [SerializeField]
        private Color colorHighlight = Color.white;

        [SerializeField]
        private Color colorDim = Color.white;

        [SerializeField, PropertySpace(4)]
        private GameObject[] daemons = null;

        [SerializeField, ShowIf("@UnityEngine.Application.isPlaying")]
        private Actor[] actors = new Actor[0];

        [UIManProperty]
        public ObservableList<DialogueChoiceViewModel> Choices { get; }
            = new ObservableList<DialogueChoiceViewModel>();

        private string conversationId;
        private Action onHideCompleted;

        private ConversationRow conversation;
        private DialogueRow defaultDialogue;
        private DialogueRow dialogue;
        private DialogueRow badDialogue;
        private DialogueRow goodDialogue;
        private ChoiceRow defaultChoice;

        private void FindAllCanvases()
            => this.canvases = GetComponentsInChildren<Canvas>();

        private void FindAllRaycasters()
            => this.raycasters = GetComponentsInChildren<GraphicRaycaster>();

        public override void OnShow(params object[] args)
        {
            base.OnShow(args);

            var index = 0;

            args.GetThenMoveNext(ref index, out this.conversationId)
                .GetThenMoveNext(ref index, out this.onHideCompleted);

            //this.panelCover.OnHideComplete.AddListener(BeginShow);
            //this.contentTyper.PrintCompleted.AddListener(ContentTyper_OnPrintCompleted);

            ToggleCanvases(true);
            Initialize();

            this.panelBackground.Hide(true);
            this.panelConversation.Hide(true);
            this.panelCover.Show(true);
        }

        public override void OnShowComplete()
        {
            base.OnShowComplete();

            PrepareUI().Forget();

            this.Choices.Clear();
            this.panelCover.Hide();
        }

        public override void OnHide()
        {
            //this.isEnd = true;
            //this.isHiding = true;

            //HideAllActors();
            base.OnHide();
        }

        public override void OnHideComplete()
        {
            ToggleCanvases(false);

            base.OnHideComplete();

            //Deinitialize();
            this.onHideCompleted?.Invoke();
        }

        private void HideOnEnd()
        {
            //if (this.isHiding)
            //    return;

            //this.isHiding = true;

            //BlackScreen.Instance.Show(Hide, autoHideDuration: 0.5f);
        }

        private void ToggleCanvases(bool value)
        {
            foreach (var canvas in this.canvases)
            {
                canvas.enabled = value;
            }

            foreach (var raycaster in this.raycasters)
            {
                raycaster.enabled = value;
            }
        }

        private void Initialize()
        {
            UnuLogger.Log($"Conversation: {this.conversationId}");

            InitializeDarkLord(Settings.DataContainer.DarkLord);

            this.conversation = Settings.Novel.GetConversation(this.conversationId);

            if (this.actors.Length != this.actorViews.Length)
                this.actors = new Actor[this.actorViews.Length];

            foreach (var actor in this.actors)
            {
                actor.Id = string.Empty;
                actor.Controller = null;
            }

            //this.areChoicesOnly = false;
            //this.showChoicesOnly = false;
            //this.canTrySkipNext = true;
            //this.speedUpState = SpeedUpState.None;
            this.IsTyping = false;

            if (this.conversation == null)
            {
                UnuLogger.LogError($"Cannot find any conversation by id={this.conversationId}");
                return;
            }

            this.defaultDialogue = this.conversation.GetDialogue(this.conversation.StartingDialogue);

            if (this.defaultDialogue.Choices.Count <= 0)
            {
                UnuLogger.LogError($"Dialogue {this.dialogue.Id} of conversation {this.conversation.Id} has no choice");
                return;
            }

            for (var i = 0; i < 4; i++)
            {
                this.Choices.Add(new DialogueChoiceViewModel());
            }

            var firstChoice = this.defaultDialogue.GetChoice(0);
            this.dialogue = this.conversation.GetDialogue(firstChoice.GoTo);
        }

        private void Deinitialize()
        {
            this.SpeakerAvatar = string.Empty;
            this.AvatarAtlas = string.Empty;
            this.Choices.Clear();
            SetBackground(string.Empty);

            this.panelCover.OnHideComplete.RemoveAllListeners();
            this.contentTyper.PrintCompleted.RemoveAllListeners();
            this.contentText.maxVisibleCharacters = 0;
            this.contentText.text = string.Empty;

            this.panelCover.Hide(true);
            this.panelBackground.Hide(true);
            this.panelConversation.Hide(true);
        }

        private void InitializeDarkLord(bool value)
        {
            foreach (var daemon in this.daemons)
            {
                daemon.SetActive(value);
            }
        }

        public void SetBackground(string value)
            => this.panelBackground.Initialize(value);

        private async UniTaskVoid PrepareUI()
        {
            this.panelBackground.Show(true);

            Invoke(this.defaultDialogue.CommandsOnStart);
            Invoke(this.defaultDialogue.CommandsOnEnd);

            await UniTask.DelayFrame(2);

            this.panelConversation.Show(true);

            ApplyLayerToAllActors();
            ShowSpeaker();
            HighlightActors();
        }

        private void BeginShow()
        {
            if (this.conversation.IsNullOrNone())
                return;

            //this.isEnd = false;
            //this.isHiding = false;

            ShowDialogue(out var canShowActors);

            if (canShowActors)
            {
                ShowSpeaker();
                HighlightActors();
            }
            else
            {
                this.HasSpeakerName = false;
            }
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
        }

        private void ShowDialogue(out bool canShowActors)
        {
            canShowActors = false;

            if (this.dialogue.IsNullOrNone())
            {
                UnuLogger.LogError("Cannot show null dialogue");
                return;
            }

            if (this.dialogue.IsEnd())
            {
                UnuLogger.Log($"Show dialogue: {this.dialogue.Id}");

                Invoke(this.dialogue.CommandsOnStart);
                Invoke(this.dialogue.CommandsOnEnd);

                ForceHideAllActors();

                //this.isEnd = true;
                HideOnEnd();

                return;
            }

            if (this.dialogue.Choices.Count <= 0)
            {
                UnuLogger.LogError($"Dialogue {this.dialogue.Id} of conversation {this.conversation.Id} has no choice");
                return;
            }

            UnuLogger.Log($"Show dialogue: {this.dialogue.Id}");

            canShowActors = true;

            if (this.dialogue.Choices.Count > 1)
                this.defaultChoice = null;
            else
                this.defaultChoice = this.dialogue.GetChoice(0);

            Invoke(this.dialogue.CommandsOnStart);

            if (this.defaultChoice == null)
            {
                //PrintNonDefaultDialogue(false);
            }
            else
            {
                //PrintDefaultChoiceDialogue();
            }
        }

        private void ForceHideAllActors()
        {
            foreach (var actor in this.actors)
            {
                if (actor == null)
                    continue;

                if (actor.Controller)
                    actor.Controller.Hide();

                actor.Controller = null;
                actor.Id = string.Empty;
            }
        }

        private void HighlightActors()
        {
            var highlight = this.dialogue?.Highlight ?? DialogueRow.None.Highlight;

            foreach (var item in highlight)
            {
                var isDim = item < 0;
                var index = Mathf.Abs(item) - 1;

                if (this.actors.ValidateIndex(index))
                {
                    if (isDim)
                        Highlight(this.actors[index].Controller);
                    else
                        Dim(this.actors[index].Controller);
                }
            }

            void Highlight(CubismController model)
            {
                if (model && model.gameObject.activeSelf)
                    model.SetColor(this.colorHighlight);
            }

            void Dim(CubismController model)
            {
                if (model && model.gameObject.activeSelf)
                    model.SetColor(this.colorDim);
            }
        }

        private void ShowSpeaker()
        {
            var id = this.dialogue?.Speaker ?? string.Empty;
            var data = Settings.Character.GetCharacter(id);

            string avatar;
            string name;

            if (data.IsNullOrNone())
            {
                avatar = this.avatarDefault;
                name = string.Empty;
            }
            else
            {
                avatar = data.Avatar;
                name = GetContent(data);
            }

            this.SpeakerName = name;
            this.HasSpeakerName = !string.IsNullOrEmpty(name);
            this.SpeakerAvatar = avatar;
        }

        private string GetContent(ChoiceRow choice)
            => this.conversation.GetContent(choice.ContentId).GetLocalization(Settings.DataContainer.Settings.Language);

        private string GetContent(CharacterRow character)
            => Settings.Character.GetContent(character.ContentId).GetLocalization(Settings.DataContainer.Settings.Language);

        private void Invoke(ICommandList commands)
        {
            if (commands == null)
                return;

            var progress = Settings.DataContainer.BasePlayer.ProgressPoint;
            Settings.CommandSystem.Invoke(commands.AsSegment(), progress);
        }

        private void ApplyLayerToAllActors()
        {
            for (var i = 0; i < this.actors.Length; i++)
            {
                ApplyLayerToActor(this.actors[i], this.actorViews[i]);
            }
        }

        private void ApplyLayerToActor(Actor actor, ActorView view)
        {
            if (actor == null || view == null)
                return;

            if (actor.Controller)
                actor.Controller.SetLayer(view.Layer);
        }
        [Serializable]
        private class ActorView
        {
            public Transform Position;
            public SingleOrderLayer Layer;
        }

        [Serializable]
        private class Actor
        {
            public string Id;
            public CubismController Controller;
        }
    }
}