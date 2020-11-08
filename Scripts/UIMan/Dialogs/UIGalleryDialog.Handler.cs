using System;
using System.Table;
using UnityEngine;
using UnityEngine.UI;
using VisualNovelData.Data;
using UnuGames;
using DG.Tweening;
using Cysharp.Threading.Tasks;

namespace HegaCore.UI
{
    using Database;

    public partial class UIGalleryDialog : UIManDialog
    {
        public static void Show(Action onShow = null, Action onShowCompleted = null, Action onHide = null, Action onHideCompleted = null)
        {
            UIMan.Instance.ShowDialog<UIGalleryDialog>(onShow, onShowCompleted, onHide, onHideCompleted);
        }

        public static void Hide()
        {
            UIMan.Instance.HideDialog<UIGalleryDialog>(true);
        }

        [SerializeField]
        private Panel fullPanel = null;

        [SerializeField]
        private Panel minimalPanel = null;

        [SerializeField]
        private RectTransform placeholderSize = null;

        [SerializeField]
        private CharacterListModule characterList = null;

        [SerializeField]
        private GalleryCharacterItemModule[] images = null;

        [SerializeField]
        private GalleryCharacterItemModule[] clips = null;

        [SerializeField]
        private SingleOrderLayer characterLayer = default;

        private Action onShow;
        private Action onShowCompleted;
        private Action onHide;
        private Action onHideCompleted;
        private bool manualHide;

        private BaseGameDataContainer dataContainer;
        private ReadCharacterData characterData;
        private ReadTable<CharacterEntry> table;
        private GalleryClipAction showClip;
        private bool isHidden;

        private CubismController characterModel;
        private float modelScale;
        private int currentCharacter;
        private int maxImageIndex;
        private int currentImageId;
        private int currentImageIndex;

        private void Awake()
        {
            this.characterList.OnSelect += SelectCharacter;
            this.minimalPanel.OnShowComplete.AddListener(UnlockInput);
            this.fullPanel.OnShowComplete.AddListener(UnlockInput);
        }

        public override void OnShow(params object[] args)
        {
            base.OnShow(args);

            var index = 0;

            args.GetThenMoveNext(ref index, out this.onShow);
            args.GetThenMoveNext(ref index, out this.onShowCompleted);
            args.GetThenMoveNext(ref index, out this.onHide);
            args.GetThenMoveNext(ref index, out this.onHideCompleted);

            this.onShow?.Invoke();

            var placeholderHeight = this.placeholderSize.rect.height;
            var fullHeight = UIMan.Instance.GetComponent<CanvasScaler>().referenceResolution.y;
            this.modelScale = placeholderHeight / fullHeight;
            this.isHidden = false;

            Initialize();
        }

        public override void OnShowComplete()
        {
            base.OnShowComplete();

            this.onShowCompleted?.Invoke();

            SelectCharacter(Settings.FirstCharacterId);
        }

        public override void OnHide()
        {
            base.OnHide();

            Deinitialize();
            this.onHide?.Invoke();
        }

        public override void OnHideComplete()
        {
            base.OnHideComplete();

            if (this.manualHide)
                this.onHideCompleted?.Invoke();

            this.isHidden = true;
        }

        private void Initialize()
        {
            this.dataContainer = Settings.DataContainer;
            this.characterData = CharacterDataset.Data;
            this.table = CharacterDataset.Table;
            this.showClip = Settings.ShowClip;

            this.characterList.Initialize(CharacterDataset.Map, this.dataContainer.CharacterImages);
            this.currentImageId = -1;
            this.currentImageIndex = -1;
        }

        private void Deinitialize()
        {
            this.characterList.Deinitialize();
            HideCharacterModel();
        }

        private void SelectCharacter(int characterId)
            => SelectCharacter(characterId, false);

        private void SelectCharacter(int characterId, bool fromMax)
        {
            HideCharacterModel();

            this.currentCharacter = characterId;
            this.characterList.SetSelected(this.currentCharacter);
            this.maxImageIndex = -1;

            for (var i = 0; i < this.images.Length; i++)
            {
                var image = this.images[i];
                image.Deinitialize();

                var id = new CharacterId(this.currentCharacter, image.Id);
                var unlocked = this.dataContainer.CharacterImages.Contains(id);
                image.Initialize(unlocked, SelectImage);

                if (unlocked && i > this.maxImageIndex)
                    this.maxImageIndex = i;
            }

            var progress = this.dataContainer.GetPlayerCharacterProgress(this.currentCharacter, true);
            var data = this.table.GetEntry(this.currentCharacter);

            foreach (var clip in this.clips)
            {
                clip.Deinitialize();

                var unlocked = false;
                var character = string.Empty;

                if (clip.Id == 1)
                {
                    unlocked = progress >= data.Milestone_1;
                    character = data.Thumbnail_1;
                }
                else
                {
                    unlocked = progress >= data.Milestone_2;
                    character = data.Thumbnail_2;
                }

                clip.Initialize(character, unlocked, Clip_OnClick);
            }

            if (this.maxImageIndex >= 0)
            {
                if (fromMax)
                    this.currentImageIndex = this.maxImageIndex;
                else
                {
                    if (this.currentImageIndex < 0 ||
                      this.currentImageIndex > this.maxImageIndex)
                        this.currentImageIndex = 0;
                }

                SelectImageIndex(this.currentImageIndex);
            }
            else
            {
                this.currentImageId = -1;
                HideCharacterModel();
            }
        }

        private void HideCharacterModel()
        {
            if (this.characterModel)
                this.characterModel.PlayBodyAnimation(0);

            CubismManager.Instance.HideAll();
            this.characterModel = null;
        }

        private void SelectImage(int id)
        {
            var index = 0;

            for (var i = 0; i < this.images.Length; i++)
            {
                if (this.images[i].Id == id)
                {
                    index = i;
                    break;
                }
            }

            SelectImageIndex(index);
        }

        private void SelectImageIndex(int index)
        {
            if (index > this.maxImageIndex)
                return;

            this.currentImageIndex = index;
            this.currentImageId = this.images[this.currentImageIndex].Id;

            for (var i = 0; i < this.images.Length; i++)
            {
                this.images[i].SetSelected(index == i);
            }

            ShowCurrentImage();
        }

        private void ShowCurrentImage()
        {
            if (!this.table.TryGetEntry(this.currentCharacter, out var data))
                return;

            var character = this.characterData.GetCharacter(data.Name.ToLower());
            var model = character?.P1 ?? string.Empty;

            this.CharacterKey = data.NameKey;
            this.CharacterIconKey = data.IconKey;

            if (string.IsNullOrEmpty(model))
                return;

            Vector3 position;
            float scale;

            if (this.minimalPanel.IsHidden)
            {
                position = this.placeholderSize.position;
                scale = this.modelScale;
            }
            else
            {
                position = this.fullPanel.transform.position;
                scale = 1f;
            }

            if (!this.characterModel)
                this.characterModel = CubismManager.Instance.Show(model, position, orderLayer: this.characterLayer, scale: scale);

            if (!this.characterModel)
                return;

            this.characterModel.PlayBodyAnimation(this.currentImageId);
        }

        private void Clip_OnClick(int id)
        {
            this.showClip?.Invoke(new CharacterId(this.currentCharacter, id), MakeInvisible, MakeVisible);
        }

        private void MakeInvisible()
        {
            SetInvisible();
            LockInput();
            HideCharacterModel();
        }

        private void MakeVisible()
        {
            SetVisible();
            UnlockInput();
            ShowCurrentImage();
        }

        private void Transition(Vector3 position, float scale, float duration)
        {
            var scaleVec = this.characterModel.LocalScale * scale;
            var tween = DOTween.Sequence();
            tween.Insert(0f, this.characterModel.transform.DOMove(position, duration));
            tween.Insert(0f, this.characterModel.transform.DOScale(scaleVec, duration));
        }

        public void UI_Button_Close()
        {
            UIDefaultActivity.Show(HideGallery, true);
        }

        private async UniTask HideGallery()
        {
            HideCharacterModel();
            this.manualHide = true;
            Hide();

            await UniTask.WaitUntil(() => this.isHidden);
        }

        public void UI_Button_ShowFull()
        {
            LockInput();

            this.minimalPanel.Hide();
            this.fullPanel.Show();

            if (this.characterModel)
                Transition(this.placeholderSize.position, this.modelScale, this.fullPanel.ShowDuration);
        }

        public void UI_Button_ShowMinimal()
        {
            LockInput();

            this.fullPanel.Hide();
            this.minimalPanel.Show();

            if (this.characterModel)
                Transition(this.fullPanel.transform.position, 1f, this.fullPanel.ShowDuration);
        }

        public void UI_Button_Next()
        {
            this.currentImageIndex += 1;

            if (this.currentImageIndex > this.maxImageIndex)
            {
                if (this.maxImageIndex >= 0)
                    this.currentImageIndex = 0;
                else
                    this.currentImageIndex = -1;
            }

            SelectImageIndex(this.currentImageIndex);
        }

        public void UI_Button_Previous()
        {
            this.currentImageIndex -= 1;

            if (this.currentImageIndex < 0)
                this.currentImageIndex = this.maxImageIndex;

            SelectImageIndex(this.currentImageIndex);
        }

        public void UI_Button_Minimal_Next()
        {
            this.currentImageIndex += 1;

            if (this.currentImageIndex <= this.maxImageIndex)
            {
                SelectImageIndex(this.currentImageIndex);
                return;
            }

            if (this.maxImageIndex >= 0)
                this.currentImageIndex = 0;
            else
                this.currentImageIndex = -1;

            var characterIndex = this.characterList.CurrentIndex;
            var nextCharacterIndex = characterIndex + 1;
            var reset = true;

            for (var i = 0; i < this.characterList.Count; i++)
            {
                if (this.characterList.IsUnlock(i))
                {
                    if (i == nextCharacterIndex)
                    {
                        reset = false;
                        characterIndex = nextCharacterIndex;
                        break;
                    }
                }
            }

            if (reset)
                characterIndex = 0;

            SelectCharacter(this.characterList.GetCharacterId(characterIndex));
        }

        public void UI_Button_Minimal_Previous()
        {
            this.currentImageIndex -= 1;

            if (this.currentImageIndex >= 0)
            {
                SelectImageIndex(this.currentImageIndex);
                return;
            }

            this.currentImageIndex = this.maxImageIndex;

            var characterIndex = this.characterList.CurrentIndex - 1;

            if (characterIndex < 0)
                characterIndex = this.characterList.Count - 1;

            for (; characterIndex >= 0; characterIndex--)
            {
                if (this.characterList.IsUnlock(characterIndex))
                    break;
            }

            SelectCharacter(this.characterList.GetCharacterId(characterIndex), true);
        }
    }
}