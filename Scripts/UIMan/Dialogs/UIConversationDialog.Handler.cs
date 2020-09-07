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
using HegaCore.Commands.Data;

namespace HegaCore.UI
{
    public partial class UIConversationDialog : UIManDialog, IPointerClickHandler
    {
        public static void Show(string id, Action onShowCompleted = null, Action onHide = null, Action onHideCompleted = null)
        {
            UIMan.Instance.ShowDialog<UIConversationDialog>(id, onShowCompleted, onHide, onHideCompleted);
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
        private Action onShowCompleted;
        private Action onHide;
        private Action onHideCompleted;

        private ConversationRow conversation;
        private DialogueRow defaultDialogue;
        private DialogueRow dialogue;
        private DialogueRow badDialogue;
        private DialogueRow goodDialogue;
        private ChoiceRow defaultChoice;

        private bool areChoicesOnly = false;
        private bool showChoicesOnly = false;
        private bool canTrySkipNext = true;
        private SpeedType speedType = SpeedType.Normal;
        private bool isEnd = true;
        private bool isHiding = true;
        private bool daemon;

        private void FindAllCanvases()
            => this.canvases = GetComponentsInChildren<Canvas>();

        private void FindAllRaycasters()
            => this.raycasters = GetComponentsInChildren<GraphicRaycaster>();

        public override void OnShow(params object[] args)
        {
            this.isEnd = true;
            this.isHiding = true;

            base.OnShow(args);

            var index = 0;

            args.GetThenMoveNext(ref index, out this.conversationId)
                .GetThenMoveNext(ref index, out this.onShowCompleted)
                .GetThenMoveNext(ref index, out this.onHide)
                .GetThenMoveNext(ref index, out this.onHideCompleted);

            this.panelCover.OnHideComplete.AddListener(ShowFirstDialogue);
            this.contentTyper.PrintCompleted.AddListener(ContentTyper_OnPrintCompleted);

            ToggleCanvases(true);
            Initialize();

            this.panelBackground.Hide(true);
            this.panelConversation.Hide(true);
            this.panelCover.Show(true);
        }

        public override void OnShowComplete()
        {
            base.OnShowComplete();

            ResetUI().Forget();
            this.panelCover.Hide();
            this.onShowCompleted?.Invoke();
        }

        public override void OnHide()
        {
            this.onHide?.Invoke();

            this.isEnd = true;
            this.isHiding = true;

            ForceHideAllActors();
            base.OnHide();
        }

        public override void OnHideComplete()
        {
            ToggleCanvases(false);

            base.OnHideComplete();

            Deinitialize();
            this.onHideCompleted?.Invoke();
        }

        private void HideOnEnd()
        {
            if (this.isHiding)
                return;

            this.isHiding = true;
            BeginHide();
        }

        private void BeginHide()
        {
            var settings = UIActivity.Settings.Default.With(false, false, alphaOnShow: 0f);
            UIMan.Instance.ShowActivity(0.5f, 0.5f, settings, OnActivityShowComplete);
        }

        private void OnActivityShowComplete(UIActivity sender, params object[] args)
            => Hide();

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

            InitializeDaemon(this.daemon = Settings.DataContainer.DarkLord);

            this.conversation = Settings.Novel.GetConversation(this.conversationId);

            if (this.actors.Length != this.actorViews.Length)
                this.actors = new Actor[this.actorViews.Length];

            for (var i = 0; i < this.actors.Length; i++)
            {
                if (this.actors[i] == null)
                    this.actors[i] = new Actor();

                this.actors[i].Model = string.Empty;
                this.actors[i].Controller = null;
            }

            this.AvatarAtlas = Settings.AvatarAtlasName;
            this.IsTyping = true;

            this.areChoicesOnly = false;
            this.showChoicesOnly = false;
            this.canTrySkipNext = true;
            this.speedType = SpeedType.Normal;

            if (this.conversation.IsNullOrNone())
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

        private void InitializeDaemon(bool value)
        {
            foreach (var daemon in this.daemons)
            {
                daemon.SetActive(value);
            }
        }

        private void SetBackground(string name, float? duration = null)
            => this.panelBackground.Switch(name, duration: duration);

        private async UniTaskVoid ResetUI()
        {
            this.Choices.Clear();
            this.panelBackground.Show(true);

            Invoke(this.defaultDialogue.CommandsOnStart);
            Invoke(this.defaultDialogue.CommandsOnEnd);

            await UniTask.DelayFrame(2);

            this.panelConversation.Show(true);

            ApplyLayerToAllActors();
            ShowSpeaker();
            HighlightActors();
        }

        private void ShowFirstDialogue()
        {
            if (this.conversation.IsNullOrNone())
                return;

            this.isEnd = false;
            this.isHiding = false;

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

        private void Update()
        {
            if (this.isEnd && this.isHiding)
                return;

            if (this.CanvasGroup.interactable)
            {
                if (Input.GetKeyUp(KeyCode.Return) ||
                    Input.GetKeyUp(KeyCode.KeypadEnter) ||
                    Input.GetKeyUp(KeyCode.Space))
                {
                    TrySkipNextOrEnd();
                }
            }

            if (Input.GetKey(KeyCode.LeftControl) ||
                Input.GetKey(KeyCode.RightControl))
            {
                SpeedUp();
            }
            else
            {
                StopSpeedUp();
            }
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (this.isEnd && this.isHiding)
                return;

            if (!this.CanvasGroup.interactable)
                return;

            TrySkipNextOrEnd();
        }

        public void TrySkipNextOrEnd()
        {
            if (!this.isEnd)
                TrySkipNext();

            if (this.isEnd)
                HideOnEnd();
        }

        public void SkipNext()
        {
            if (this.contentTyper.IsTyping)
            {
                this.contentTyper.Skip();
                return;
            }

            if (this.dialogue.IsNullOrNone())
            {
                UnuLogger.LogError("Cannot skip null dialogue");
                return;
            }

            if (this.areChoicesOnly)
                return;

            if (this.dialogue.Choices.Count <= 1)
                Next();
        }

        private void TrySkipNext()
        {
            if (!this.canTrySkipNext)
                return;

            this.canTrySkipNext = false;

            SkipNext();
            LockTrySkipNext(0.1f).Forget();
        }

        private void SpeedUp()
        {
            if (this.isEnd && this.isHiding)
                return;

            if (this.speedType != SpeedType.Normal)
                return;

            var state = this.speedType;
            this.speedType = SpeedType.SpeedUp;

            if (this.isEnd)
            {
                if (state == SpeedType.Normal)
                    HideOnEnd();

                return;
            }

            if (this.contentTyper.IsTyping)
            {
                this.contentTyper.Pause();
                this.contentTyper.Resume(this.speedTyperConfig);
            }
            else
            {
                TrySkipNext();
            }
        }

        private void StopSpeedUp()
        {
            if (this.isEnd && this.isHiding)
                return;

            if (this.speedType == SpeedType.Normal)
                return;

            this.speedType = SpeedType.Normal;

            if (this.isEnd)
                return;

            if (this.contentTyper.IsTyping)
            {
                this.contentTyper.Pause();
                this.contentTyper.Resume();
            }
        }

        private async UniTaskVoid LockTrySkipNext(float seconds)
        {
            this.canTrySkipNext = false;

            await UniTask.Delay(TimeSpan.FromSeconds(seconds));

            this.canTrySkipNext = true;
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
                this.defaultChoice = ChoiceRow.None;
            else
                this.defaultChoice = this.dialogue.GetChoice(0);

            Invoke(this.dialogue.CommandsOnStart);

            if (this.defaultChoice.IsNullOrNone())
            {
                PrintNonDefaultDialogue(false);
            }
            else
            {
                PrintDefaultChoiceDialogue();
            }
        }

        private void ContentTyper_OnPrintCompleted()
        {
            this.IsTyping = false;
            PrintNonDefaultDialogue(true);

            if (this.speedType != SpeedType.Normal)
            {
                this.speedType = SpeedType.RenewSpeedUp;
                this.canTrySkipNext = true;
            }
        }

        private void PrintNonDefaultDialogue(bool invokeCommandsOnEnd)
        {
            if (invokeCommandsOnEnd)
                Invoke(this.dialogue.CommandsOnEnd);

            RefreshChoices(this.dialogue.Choices);
            TryShowNextDialogueImmediately();
        }

        private void PrintDefaultChoiceDialogue()
        {
            Clear();
            CheckNextDialogueAreChoicesOnly();

            var content = GetContent(this.defaultChoice);

            if (string.IsNullOrEmpty(content))
            {
                UnuLogger.LogWarning("Default choice content is empty");
                content = " ";
            }

            this.IsTyping = true;

            if (this.speedType != SpeedType.Normal)
            {
                this.contentTyper.TypeText(content, this.speedTyperConfig);
            }
            else
                this.contentTyper.TypeText(content);
        }

        private void Clear()
        {
            this.Choices.Clear();
            this.contentText.SetText(string.Empty);
        }

        private void RefreshChoices(IChoiceDictionary choices)
        {
            if (choices.Count <= 1)
                return;

            foreach (var choice in choices.Values)
            {
                if (choice.Id <= 0)
                    continue;

                this.Choices.Add(new DialogueChoiceViewModel {
                    Id = choice.Id,
                    Content = GetContent(choice),
                    OnSelect = OnSelectChoice
                });
            }
        }

        private void OnSelectChoice(int id)
        {
            var choice = this.dialogue.GetChoice(id);
            Invoke(this.dialogue.CommandsOnEnd);

            if (choice.IsNullOrNone())
            {
                UnuLogger.LogError($"Cannot find any choice by id={id} in the dialogue {this.dialogue.Id} of the conversation {this.conversation.Id}");
                return;
            }

            UnuLogger.Log($"Select: {id}");
            this.areChoicesOnly = false;
            NextDialogue(choice.GoTo);
        }

        private void NextDialogue(string id)
        {
            this.dialogue = this.conversation.GetDialogue(id);

            ShowDialogue(out var canShowActors);

            if (!canShowActors)
            {
                this.HasSpeakerName = false;
                return;
            }

            ShowSpeaker();
            HighlightActors();
            InvokeActors();
        }

        private void CheckNextDialogueAreChoicesOnly()
        {
            this.areChoicesOnly = false;
            this.showChoicesOnly = false;

            if (this.defaultChoice.IsNullOrNone())
                return;

            var nextDialogue = this.conversation.GetDialogue(this.defaultChoice.GoTo);

            if (nextDialogue.IsNullOrNone() || nextDialogue.IsEnd())
                return;

            this.areChoicesOnly = nextDialogue.Choices.Count > 1;
            this.showChoicesOnly = this.areChoicesOnly;
        }

        private void TryShowNextDialogueImmediately()
        {
            if (!this.areChoicesOnly)
            {
                if (this.speedType != SpeedType.Normal)
                    Next();

                return;
            }

            if (!this.showChoicesOnly)
                return;

            Next();
            this.showChoicesOnly = false;
        }

        private void Next()
        {
            if (this.defaultChoice.IsNullOrNone())
            {
                UnuLogger.LogError($"Cannot go next from dialogue id={this.dialogue.Id}");
                return;
            }

            NextDialogue(this.defaultChoice.GoTo);
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
                actor.Model = string.Empty;
            }
        }

        private void HighlightActors()
        {
            if (this.dialogue.IsNullOrNone())
                return;

            foreach (var item in this.dialogue.Highlight)
            {
                var isDim = item < 0;
                var index = Mathf.Abs(item) - 1;

                if (!this.actors.ValidateIndex(index))
                    continue;

                var actor = this.actors[index];

                if (actor == null || !actor.Controller)
                    continue;

                var color = isDim ? this.colorDim : this.colorHighlight;
                CubismManager.Instance.SetColor(actor.Model, color, Settings.ActorDuration.Color);
            }
        }

        private void InvokeActors()
        {
            if (this.dialogue.IsNullOrNone())
                return;

            const int length = 4;
            var actors = Array1Pool<string>.Get(length);
            var actions = Array1Pool<IActorCommandList>.Get(length);

            var x = this.dialogue;
            actors.Set(x.Actor1, x.Actor2, x.Actor3, x.Actor4);
            actions.Set(x.Actions1, x.Actions2, x.Actions3, x.Actions4);

            for (var i = 0; i < length; i++)
            {
                if (string.IsNullOrEmpty(actors[i]))
                    continue;

                Invoke(actions[i]);
            }

            Array1Pool<string>.Return(actors);
            Array1Pool<IActorCommandList>.Return(actions);
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

        private void Invoke(IReadOnlyList<Command> commands)
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

        public void UI_Button_EndConversation()
        {
            this.badDialogue = null;
            this.goodDialogue = null;

            var pointBadAdd = new PointBadAdd();
            var pointGoodAdd = new PointGoodAdd();

            bool Contains(ICommandList commands, string key)
            {
                foreach (var command in commands)
                {
                    if (!string.IsNullOrEmpty(command.Key) &&
                        command.Key.Equals(key))
                        return true;
                }

                return false;
            }

            foreach (var dialog in this.conversation.Dialogues.Values)
            {
                if (Contains(dialog.CommandsOnStart, pointBadAdd.Key) ||
                    Contains(dialog.CommandsOnEnd, pointBadAdd.Key))
                {
                    this.badDialogue = dialog;
                }
                else
                if (Contains(dialog.CommandsOnStart, pointGoodAdd.Key) ||
                    Contains(dialog.CommandsOnEnd, pointGoodAdd.Key))
                {
                    this.goodDialogue = dialog;
                }
            }

            if (this.badDialogue == null || this.goodDialogue == null)
            {
                EndConversationInternal();
            }
            else
            {
                ShowGoodBadPanel();
            }
        }

        private void ShowGoodBadPanel()
        {
        }

        public void UI_Button_EndBad()
        {
            if (this.badDialogue != null)
            {
                Invoke(this.badDialogue.CommandsOnStart);
                Invoke(this.badDialogue.CommandsOnEnd);
            }

            EndConversationInternal();
        }

        public void UI_Button_EndGood()
        {
            if (this.goodDialogue != null)
            {
                Invoke(this.goodDialogue.CommandsOnStart);
                Invoke(this.goodDialogue.CommandsOnEnd);
            }

            EndConversationInternal();
        }

        private void EndConversationInternal()
        {
            if (this.contentTyper.IsTyping)
                this.contentTyper.Skip();

            var endDialogue = this.conversation.GetDialogue(EndDialogueRow.Keyword);

            if (endDialogue == null)
            {
                UnuLogger.LogError("END dialogue is missing");
                return;
            }

            Invoke(endDialogue.CommandsOnStart);
            Invoke(endDialogue.CommandsOnEnd);
            this.isEnd = true;
            BeginHide();
        }

        private string GetActorId(int actorNumber)
        {
            switch (actorNumber)
            {
                case 1: return this.dialogue?.Actor1 ?? string.Empty;
                case 2: return this.dialogue?.Actor2 ?? string.Empty;
                case 3: return this.dialogue?.Actor3 ?? string.Empty;
                case 4: return this.dialogue?.Actor4 ?? string.Empty;
                default: return string.Empty;
            }
        }

        private ActorView GetActorView(int actorNumber)
        {
            var index = actorNumber - 1;

            if (this.actorViews.ValidateIndex(index))
                return this.actorViews[index];

            return null;
        }

        private Actor GetActor(int actorNumber)
        {
            var index = actorNumber - 1;

            if (this.actors.ValidateIndex(index))
                return this.actors[index];

            return null;
        }

        public void UI_Event_Actor_Show(int actorNumber)
        {
            var id = GetActorId(actorNumber);
            var character = Settings.Character.GetCharacter(id);
            var model = character?.P1 ?? string.Empty;

            if (string.IsNullOrEmpty(model))
                return;

            var view = GetActorView(actorNumber);
            var actor = GetActor(actorNumber);

            if (view == null || actor == null)
                return;

            actor.Model = model;
            actor.Controller = CubismManager.Instance.Show(model, view.Position.position, Settings.ActorDuration.Show);

            if (!this.isHiding)
                ApplyLayerToActor(actor, view);
        }

        public void UI_Event_Actor_Show_FromLeft(int actorNumber)
            => ShowActorMove(actorNumber, this.positionLeft);

        public void UI_Event_Actor_Show_FromRight(int actorNumber)
            => ShowActorMove(actorNumber, this.positionRight);

        private void ShowActorMove(int actorNumber, Transform fromPosition)
        {
            var id = GetActorId(actorNumber);
            var character = Settings.Character.GetCharacter(id);
            var model = character?.P1 ?? string.Empty;

            if (string.IsNullOrEmpty(model))
                return;

            var view = GetActorView(actorNumber);
            var actor = GetActor(actorNumber);

            if (view == null || actor == null)
                return;

            var from = fromPosition.position;
            var to = view.Position.position;

            actor.Model = model;
            actor.Controller = CubismManager.Instance.Show(model, from, to, Settings.ActorDuration.Show);

            if (!this.isHiding)
                ApplyLayerToActor(actor, view);
        }

        public void UI_Event_Actor_Hide(int actorNumber)
        {
            var actor = GetActor(actorNumber);

            if (actor == null || !actor.Controller)
                return;

            CubismManager.Instance.Hide(actor.Model, Settings.ActorDuration.Hide);

            actor.Model = string.Empty;
            actor.Controller = null;
        }

        public void UI_Event_Actor_Hide_ToLeft(int actorNumber)
            => HideActorMove(actorNumber, this.positionLeft);

        public void UI_Event_Actor_Hide_ToRight(int actorNumber)
            => HideActorMove(actorNumber, this.positionRight);

        private void HideActorMove(int actorNumber, Transform toPosition)
        {
            var actor = GetActor(actorNumber);

            if (actor == null || !actor.Controller)
                return;

            CubismManager.Instance.Hide(actor.Model, toPosition.position, Settings.ActorDuration.Hide);

            actor.Model = string.Empty;
            actor.Controller = null;
        }

        public void UI_Event_Actor_Hide_All()
        {
            for (var i = 0; i < this.actors.Length; i++)
            {
                var actor = this.actors[i];

                if (actor == null || !actor.Controller)
                    continue;

                CubismManager.Instance.Hide(actor.Model, Settings.ActorDuration.Hide);

                actor.Model = string.Empty;
                actor.Controller = null;
            }
        }

        public void UI_Event_Background_Set(string name)
            => SetBackground(name);

        public void UI_Event_Background_Change(string name)
            => SetBackground(name, Settings.BackgroundDurationChange);

        public void UI_Event_Background_Change(string name, float duration)
            => SetBackground(name, duration);

        [Serializable]
        private class ActorView
        {
            public Transform Position = null;
            public SingleOrderLayer Layer = default;
        }

        [Serializable]
        private class Actor
        {
            [SerializeField]
            private string model = string.Empty;

            public string Model
            {
                get => this.model;
                set => this.model = value ?? string.Empty;
            }

            public CubismController Controller = null;
        }

        private enum SpeedType
        {
            Normal, SpeedUp, RenewSpeedUp
        }
    }
}
