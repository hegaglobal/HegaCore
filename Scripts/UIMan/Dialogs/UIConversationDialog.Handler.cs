using System;
using System.Collections.Generic;
using System.Collections.Pooling;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnuGames;
using UnuGames.MVVM;
using RedBlueGames.Tools.TextTyper;
using VisualNovelData.Data;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using HegaCore.Events.Commands;

namespace HegaCore.UI
{
    public partial class UIConversationDialog : UIManDialog
    {
        public static void Show(string id, Action onShow = null, Action onShowCompleted = null,
                                Action onHide = null, Action onHideCompleted = null)
        {
            UIMan.Instance.ShowDialog<UIConversationDialog>(id, onShow, onShowCompleted, onHide, onHideCompleted);
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
        private Button buttonEndConversation = null;

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

        [SerializeField, InlineProperty, BoxGroup("Initial Actor View"), HideLabel]
        private ActorView initialActorView = new ActorView();

        [SerializeField]
        private Transform positionLeft = null;

        [SerializeField]
        private Transform positionRight = null;

        [SerializeField]
        private Color colorHighlight = Color.white;

        [SerializeField]
        private Color colorDim = Color.white;

        [SerializeField, ShowIf("@UnityEngine.Application.isPlaying")]
        private Actor[] actors = new Actor[0];

        [SerializeField]
        private CommandIgnorableMapper commandIgnorableMapper = null;

        [SerializeField, PropertySpace(4)]
        private GameObject[] daemons = null;

        [UIManProperty]
        public ObservableList<DialogueChoiceViewModel> Choices { get; }
            = new ObservableList<DialogueChoiceViewModel>();

        [ShowIf("@UnityEngine.Application.isPlaying")]
        private IInitializable<UIConversationDialog>[] initializables;

        [ShowIf("@UnityEngine.Application.isPlaying")]
        private IDeinitializable<UIConversationDialog>[] deinitializables;

        private string conversationId;
        private Action onShow;
        private Action onShowCompleted;
        private Action onHide;
        private Action onHideCompleted;

        private ReadNovelData novelData;
        private ReadCharacterData characterData;

        private ConversationRow conversation;
        private DialogueRow defaultDialogue;
        private DialogueRow dialogue;
        private ChoiceRow defaultChoice;

        private bool isInitialized = false;
        private bool nextDialogueHasMultipleChoices = false;
        private bool showNextDialogueMultipleChoices = false;
        private bool canTrySkipNext = true;
        private bool canSkipToEnd = true;
        private SpeedType speedType = SpeedType.Normal;
        private bool isEnd = true;
        private bool isHiding = true;
        private bool daemon;
        private int choice;
        private float waitingSeconds;

        private void Awake()
        {
            this.initializables = GetComponentsInChildren<IInitializable<UIConversationDialog>>().OrEmpty();
            this.deinitializables = GetComponentsInChildren<IDeinitializable<UIConversationDialog>>().OrEmpty();
        }

        private void Update()
        {
            if (Settings.AutoUpdateCommandSystem)
                Commands.Update(GameTime.Provider.DeltaTime);
        }

        private void FindAllCanvases()
            => this.canvases = GetComponentsInChildren<Canvas>().OrEmpty();

        private void FindAllRaycasters()
            => this.raycasters = GetComponentsInChildren<GraphicRaycaster>().OrEmpty();

        public override void OnShow(params object[] args)
        {
            this.isEnd = true;
            this.isHiding = true;

            base.OnShow(args);

            var index = 0;

            args.GetThenMoveNext(ref index, out this.conversationId)
                .GetThenMoveNext(ref index, out this.onShow)
                .GetThenMoveNext(ref index, out this.onShowCompleted)
                .GetThenMoveNext(ref index, out this.onHide)
                .GetThenMoveNext(ref index, out this.onHideCompleted);

            this.panelCover.OnHideComplete.AddListener(ShowFirstDialogue);
            this.contentTyper.PrintCompleted.AddListener(ContentTyper_OnPrintCompleted);

            ToggleCanvases(true);
            Initialize();

            this.panelBackground.Hide(true);
            this.panelConversation.Hide(true);
            // if(showCoverOnAwake)
            //     this.panelCover.Show(true);

            this.onShow?.Invoke();
        }

        public override void OnShowComplete()
        {
            base.OnShowComplete();
            LateShowComplete().Forget();
        }

        private async UniTaskVoid LateShowComplete()
        {
            await ResetUIAsync();
            await UniTask.WaitUntil(() => this.isInitialized);
            
            //Code Cu cua Tung, goi hide panel => complete thi show first dialogue.
            //this.panelCover.Hide();
            //End Code Cu ========================================================
            
            this.onShowCompleted?.Invoke();
            RegisterCommands();
            
            //Code moi, Hide truc tiep. Muon show cover thi dung command.
            ShowFirstDialogue();
        }

        private void RegisterCommands()
        {
            Commands.RegisterSpeedUpCommand(ExecuteSpeedUp, DeactivateSpeedUp);
            Commands.RegisterSkipNextCommand(ExecuteSkipNextOrEnd, null);
            this.buttonEndConversation.interactable = true;
        }

        private void RemoveCommands()
        {
            Commands.RemoveCommands();
            this.buttonEndConversation.interactable = false;
        }

        public override void OnHide()
        {
            this.onHide?.Invoke();

            this.isEnd = true;
            this.isHiding = true;

            RemoveCommands();

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

            UnuLogger.Log("Begin close Conversation on END");

            this.isHiding = true;
            BeginHide();
        }

        private void BeginHide()
        {
            //Code Cu cua Tung, luon luôn show panel cover khi hide dialogue conv.
            // this.panelCover.OnShowComplete.AddListener(OnActivityShowComplete);
            // this.panelCover.Show();
            //End Code Cu ========================================================
            
            //Code moi, Hide truc tiep. Muon show cover thi dung command.
            Hide();
        }

        private void OnActivityShowComplete()
        {
            this.panelCover.OnShowComplete.RemoveListener(OnActivityShowComplete);

            UnuLogger.Log("Close Conversation");
            Hide();
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
            this.isInitialized = false;

            this.novelData = VisualNovelDataset.Novel;
            this.characterData = VisualNovelDataset.Character;

            UnuLogger.Log($"Conversation: {this.conversationId}");

            InitializeDaemon(this.daemon = Settings.DataContainer.Daemon);

            this.conversation = this.novelData.GetConversation(this.conversationId);

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

            this.nextDialogueHasMultipleChoices = false;
            this.showNextDialogueMultipleChoices = false;
            this.canTrySkipNext = true;
            this.speedType = SpeedType.Normal;
            this.choice = 0;
            this.canSkipToEnd = true;

            if (this.conversation.IsNullOrNone())
            {
                UnuLogger.LogError($"Cannot find any conversation by id={this.conversationId}");
                return;
            }

            this.defaultDialogue = GetDialogue(this.conversation.StartingDialogue);

            if (this.defaultDialogue.Choices.Count <= 0)
            {
                UnuLogger.LogWarning($"Dialogue {this.dialogue.Id} of conversation {this.conversation.Id} has no choice");
                return;
            }

            for (var i = 0; i < 4; i++)
            {
                this.Choices.Add(new DialogueChoiceViewModel());
            }

            var firstChoice = this.defaultDialogue.GetChoice(0);
            this.dialogue = GetDialogue(firstChoice.GoTo);

            if (this.initializables != null && this.initializables.Length > 0)
            {
                for (var i = 0; i < this.initializables.Length; i++)
                {
                    this.initializables[i]?.Initialize(this);
                }
            }

            LateInitialize().Forget();
        }

        private async UniTaskVoid LateInitialize()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(Settings.Durations.Show));

            this.isInitialized = true;
        }

        private void Deinitialize()
        {
            this.isInitialized = false;
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

            if (this.deinitializables != null && this.deinitializables.Length > 0)
            {
                for (var i = 0; i < this.deinitializables.Length; i++)
                {
                    this.deinitializables[i]?.Deinitialize(this);
                }
            }
        }

        private void InitializeDaemon(bool value)
        {
            foreach (var daemon in this.daemons)
            {
                daemon.SetActive(value);
            }
        }

        private DialogueRow GetDialogue(string id, bool logging = true)
        {
            var dialogue = this.conversation.GetDialogue(id);

#if UNITY_EDITOR
            if (logging)
                UnuLogger.Log($"Get [row={dialogue.Row}] dialogue_id={dialogue.Id}");
#endif

            return dialogue;
        }

        private void SetBackground(string name, float? duration = null)
        => this.panelBackground.Switch(name, duration: duration);

        private async UniTask ResetUIAsync()
        {
            this.Choices.Clear();
            this.panelBackground.Show(true);

            Invoke(this.defaultDialogue.CommandsOnStart);
            Invoke(this.defaultDialogue.CommandsOnEnd);

            await UniTask.DelayFrame(2);

            this.panelConversation.Show(true);

            ApplyLayerToAllActors();
            ShowActors();
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
                ShowActors();
            }
            else
            {
                this.HasSpeakerName = false;
            }
        }

        private void ExecuteSkipNextOrEnd()
        {
            if (this.isEnd && this.isHiding)
                return;

            if (this.CanvasGroup.interactable)
            {
                TrySkipNextOrEnd();
            }
        }

        private void ExecuteSpeedUp()
        {
            if (this.isEnd && this.isHiding)
                return;

            SpeedUp();
        }

        private void DeactivateSpeedUp()
        {
            if (this.isEnd && this.isHiding)
                return;

            StopSpeedUp();
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
                Debug.Log("Skip Next Return: ====== Skip Typing ++++++++++++");
                return;
            }

            if (this.dialogue.IsNullOrNone())
            {
                UnuLogger.LogWarning("Cannot skip null dialogue");
                return;
            }
            DelaySkipNext().Forget();
        }

        private async UniTaskVoid DelaySkipNext()
        {
            await WaitSecondsAsync();

            if (this.nextDialogueHasMultipleChoices)
            {
                DeactivateSpeedUp();
                PrintNonDefaultDialogueNoInvokeOnEnd();
                return;
            }

            if (this.dialogue.Choices.Count <= 1)
            {
                Next();
            }
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
                Invoke(this.dialogue.CommandsOnEnd);
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

        private async UniTask WaitSecondsAsync()
        {
            if (this.waitingSeconds < 0f)
                this.waitingSeconds = 0f;

            if (Mathf.Approximately(this.waitingSeconds, 0f))
                return;

            await UniTask.Delay(TimeSpan.FromSeconds(this.waitingSeconds));
            this.waitingSeconds = 0f;
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
                UnuLogger.LogWarning("Cannot show null dialogue");
                return;
            }

            if (this.dialogue.IsEnd())
            {
                UnuLogger.Log($"Show dialogue: {this.dialogue.Id}");
                Invoke(this.dialogue.CommandsOnStart);
                Invoke(this.dialogue.CommandsOnEnd);

                RemoveCommands();
                ForceHideAllActors();
                HideOnEnd();

                return;
            }

            if (this.dialogue.Choices.Count <= 0)
            {
                UnuLogger.LogWarning($"Dialogue {this.dialogue.Id} of conversation {this.conversation.Id} has no choice");
                return;
            }

            UnuLogger.Log($"Show dialogue: {this.dialogue.Id}");

            canShowActors = true;

            this.defaultChoice = this.dialogue.Choices.Count > 1 ? ChoiceRow.None : this.dialogue.GetChoice(0);
            Clear();
            Invoke(this.dialogue.CommandsOnStart);
            DelayShowDialogue().Forget();
        }

        private async UniTaskVoid DelayShowDialogue()
        {
            await WaitSecondsAsync();

            if (this.defaultChoice.IsNullOrNone())
            {
                PrintNonDefaultDialogueNoInvokeOnEnd();
                return;
            }

            CheckNextDialogueHasMultipleChoices();
            PrintDefaultChoiceDialogue();
        }

        private void PrintNonDefaultDialogueNoInvokeOnEnd()
        {
            if (this.dialogue.Choices.Count > 1)
            {
                RemoveCommands();
            }

            RefreshChoices(this.dialogue.Choices);
            TryShowNextDialogueImmediately();
        }

        private void ContentTyper_OnPrintCompleted()
        {
            this.IsTyping = false;
            Invoke(this.dialogue.CommandsOnEnd);
            DelayPrintNonDefaultDialogue().Forget();
        }

        private async UniTaskVoid DelayPrintNonDefaultDialogue()
        {
            await WaitSecondsAsync();

            if (this.dialogue.Choices.Count > 1)
            {
                RemoveCommands();
            }

            RefreshChoices(this.dialogue.Choices);
            TryShowNextDialogueImmediately();

            if (this.speedType != SpeedType.Normal)
            {
                this.speedType = SpeedType.RenewSpeedUp;
                this.canTrySkipNext = true;
            }
        }

        private void PrintDefaultChoiceDialogue()
        {
            Clear();

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
            {
                this.contentTyper.TypeText(content);
            }
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

            DelayOnSelectChoice(choice).Forget();
        }

        private async UniTaskVoid DelayOnSelectChoice(ChoiceRow choice)
        {
            await WaitSecondsAsync();

            this.nextDialogueHasMultipleChoices = false;
            NextDialogue(choice.GoTo);

            LateRegisterCommands(2).Forget();
        }

        private async UniTaskVoid LateRegisterCommands(int frameCount)
        {
            await UniTask.DelayFrame(frameCount);

            RegisterCommands();
        }

        private void NextDialogue(string id)
        {
            this.dialogue = GetDialogue(id);

            ShowDialogue(out var canShowActors);

            if (this.dialogue.IsEnd())
                return;

            if (!canShowActors)
            {
                this.HasSpeakerName = false;
                return;
            }

            ShowActors();
        }

        private void CheckNextDialogueHasMultipleChoices()
        {
            this.nextDialogueHasMultipleChoices = false;
            this.showNextDialogueMultipleChoices = false;

            if (this.defaultChoice.IsNullOrNone())
                return;

            var nextDialogue = GetDialogue(this.defaultChoice.GoTo, false);

            if (nextDialogue.IsNullOrNone() || nextDialogue.IsEnd())
                return;

            this.nextDialogueHasMultipleChoices = nextDialogue.Choices.Count > 1;
            this.showNextDialogueMultipleChoices = this.nextDialogueHasMultipleChoices;
        }

        private void TryShowNextDialogueImmediately()
        {
            if (!this.nextDialogueHasMultipleChoices)
            {
                if (this.speedType != SpeedType.Normal)
                {
                    Next();
                }

                return;
            }

            if (!this.showNextDialogueMultipleChoices)
                return;

            Next();
            this.showNextDialogueMultipleChoices = false;
        }

        private void Next()
        {
            if (this.defaultChoice.IsNullOrNone())
            {
                UnuLogger.LogWarning($"Cannot go next from dialogue id={this.dialogue.Id}");
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

        private void ShowActors()
        {
            ShowSpeaker();
            InvokeActors();
            HighlightActors();
        }

        private void ShowSpeaker()
        {
            var id = this.dialogue?.Speaker ?? string.Empty;
            var data = this.characterData.GetCharacter(id);

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

        private void InvokeActors()
        {
            if (this.dialogue.IsNullOrNone())
                return;

            const int length = 4;
            var actors = Pool.Provider.Array1<string>(length);
            var actions = Pool.Provider.Array1<IActorCommandList>(length);

            var x = this.dialogue;
            actors.Set(x.Actor1, x.Actor2, x.Actor3, x.Actor4);
            actions.Set(x.Actions1, x.Actions2, x.Actions3, x.Actions4);

            for (var i = 0; i < length; i++)
            {
                if (string.IsNullOrEmpty(actors[i]))
                    continue;

                Invoke(actions[i]);
            }

            Pool.Provider.Return(actors);
            Pool.Provider.Return(actions);
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
                CubismManager.Instance.SetColor(actor.Model, color, Settings.Durations.Color);
            }
        }

        private string GetContent(ChoiceRow choice)
            => this.conversation.GetContent(choice.ContentId).GetLocalization(Settings.DataContainer.Settings.Language);

        private string GetContent(CharacterRow character)
            => this.characterData.GetContent(character.ContentId).GetLocalization(Settings.DataContainer.Settings.Language);

        private void Invoke(IReadOnlyList<Command> commands)
        {
            if (commands == null)
                return;

            var progress = Settings.DataContainer.Player.ProgressPoint;
            Settings.CommandSystem.Invoke(commands.AsSegment(), progress);
        }

        private void InvokeSkipEnd(IReadOnlyList<Command> commands)
        {
            if (commands == null)
                return;

            var progress = Settings.DataContainer.Player.ProgressPoint;

            for (var i = 0; i < commands.Count; i++)
            {
                var command = commands[i];

                if (command == null)
                    continue;

                if (this.commandIgnorableMapper.CanIgnore(command.Key))
                    continue;

                Settings.CommandSystem.Invoke(command, progress);
            }
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

        private ChoiceRow GetChoice(DialogueRow dialogue, int choice)
        {
            var countChoice = dialogue.Choices.Count;

            if (countChoice < 0)
                return ChoiceRow.None;

            if (countChoice == 1)
                return dialogue.GetChoice(0);

            if (choice < 1)
                choice = 1;

            return dialogue.GetChoice(choice);
        }

        private async UniTaskVoid EndConversation()
        {
            await UniTask.DelayFrame(0);

            DialogueRow dialogue;

            if (this.dialogue.IsNullOrNone())
            {
                dialogue = GetDialogue(this.conversation.StartingDialogue, false);
            }
            else
            {
                var choice = GetChoice(this.dialogue, this.choice);

                if (choice.IsNullOrNone())
                {
                    EndConversationCompleted();
                    return;
                }

                dialogue = GetDialogue(choice.GoTo, false);
            }

            var count = this.conversation.Dialogues.Count;
            var index = 0;

            while (!dialogue.IsNullOrNone() && !dialogue.IsEnd())
            {
                if (index >= count)
                    break;

                InvokeSkipEnd(dialogue.CommandsOnStart);
                InvokeSkipEnd(dialogue.CommandsOnEnd);

                var choice = GetChoice(dialogue, this.choice);

                if (choice.IsNullOrNone())
                    break;

                dialogue = GetDialogue(choice.GoTo, false);
                index += 1;
            }

            EndConversationCompleted();
        }

        private void EndConversationCompleted()
        {
            var endDialogue = GetDialogue(EndDialogueRow.Keyword, false);

            if (endDialogue == null)
            {
                UnuLogger.LogError("END dialogue is missing");
            }
            else
            {
                Invoke(endDialogue.CommandsOnStart);
                Invoke(endDialogue.CommandsOnEnd);
            }

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

        public void UI_Button_EndConversation()
        {
            if (!this.canSkipToEnd)
                return;

            this.isEnd = true;
            this.canSkipToEnd = false;
            this.contentTyper.PrintCompleted.RemoveAllListeners();

            if (this.contentTyper.IsTyping)
                this.contentTyper.Skip();

            EndConversation().Forget();
        }

        private void ShowActorMove(int actorNumber, Transform fromPosition)
        {
            var id = GetActorId(actorNumber);
            var character = this.characterData.GetCharacter(id);
            var model = character?.P1 ?? string.Empty;

            if (string.IsNullOrEmpty(model))
                return;

            var view = GetActorView(actorNumber);
            var actor = GetActor(actorNumber);

            if (view == null || actor == null ||
                string.Equals(actor.Model, model))
                return;

            var from = fromPosition.position;
            var to = view.Position.position;

            actor.Model = model;
            actor.Controller = CubismManager.Instance.Show(model, from, to, Settings.Durations.Show, view.Layer);
        }

        private void HideActorMove(int actorNumber, Transform toPosition)
        {
            var actor = GetActor(actorNumber);

            if (actor == null || !actor.Controller)
                return;

            CubismManager.Instance.Hide(actor.Model, toPosition.position, Settings.Durations.Hide);

            actor.Model = string.Empty;
            actor.Controller = null;
        }

        private async UniTaskVoid DelayWaitSeconds()
        {
            await WaitSecondsAsync();

            this.buttonEndConversation.interactable = true;
            RegisterCommands();
        }

        public void UI_Event_Choice_Set(int choice)
            => this.choice = choice;

        public void UI_Event_WaitSeconds(float seconds)
        {
            this.waitingSeconds = seconds;

            RemoveCommands();
            StopSpeedUp();
            DelayWaitSeconds().Forget();
        }

        public void UI_Event_Actor_Show(int actorNumber)
        {
            var id = GetActorId(actorNumber);
            var character = this.characterData.GetCharacter(id);
            var model = character?.P1 ?? string.Empty;

            if (string.IsNullOrEmpty(model))
                return;

            var view = GetActorView(actorNumber);
            var actor = GetActor(actorNumber);

            if (view == null || actor == null ||
                string.Equals(actor.Model, model))
                return;

            actor.Model = model;
            actor.Controller = CubismManager.Instance.Show(model, view.Position.position, Settings.Durations.Show, view.Layer);
        }

        public void UI_Event_Actor_Show_FromLeft(int actorNumber)
            => ShowActorMove(actorNumber, this.positionLeft);

        public void UI_Event_Actor_Show_FromRight(int actorNumber)
            => ShowActorMove(actorNumber, this.positionRight);

        public void UI_Event_Actor_Hide(int actorNumber)
        {
            var actor = GetActor(actorNumber);

            if (actor == null || !actor.Controller)
                return;

            CubismManager.Instance.Hide(actor.Model, Settings.Durations.Hide);

            actor.Model = string.Empty;
            actor.Controller = null;
        }

        public void UI_Event_Actor_Hide_ToLeft(int actorNumber)
            => HideActorMove(actorNumber, this.positionLeft);

        public void UI_Event_Actor_Hide_ToRight(int actorNumber)
            => HideActorMove(actorNumber, this.positionRight);

        public void UI_Event_Actor_Hide_All()
        {
            for (var i = 0; i < this.actors.Length; i++)
            {
                var actor = this.actors[i];

                if (actor == null || !actor.Controller)
                    continue;

                CubismManager.Instance.Hide(actor.Model, Settings.Durations.Hide);

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

        public void UI_Event_ShowActivity(float duration, float hideDuration)
        {
            UI_Event_WaitSeconds(duration);
            UIDefaultActivity.Show(duration,hideDuration);
        }
        
        public void UI_Event_ShowActivity(float duration)
        {
            UI_Event_WaitSeconds(duration);
            UIDefaultActivity.Show(duration);
        }

        public void UI_Event_HideActivity(float duration = 1f)
        {
            UIDefaultActivity.Hide();
        }

        public void UI_Event_Play_BGM(string bgm)
        {
            AudioManager.Instance.Player.PlayMusic(bgm);
        }

        public void UI_Event_Stop_BGM()
        {
            AudioManager.Instance.Player.StopMusic();
        }
    }
}
