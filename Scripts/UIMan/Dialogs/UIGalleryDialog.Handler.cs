using System;
using System.Table;
using UnityEngine;
using UnityEngine.UI;
using VisualNovelData.Data;
using UnuGames;
using DG.Tweening;

namespace HegaCore.UI
{
    using Database;

    public partial class UIGalleryDialog : UIManDialog
    {
        public static void Show(Action onShowCompleted = null, Action onHide = null, Action onHideCompleted = null)
        {
            UIMan.Instance.ShowDialog<UIGalleryDialog>(onShowCompleted, onHide, onHideCompleted);
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
        private SingleLayer characterLayer = default;

        [SerializeField]
        private int sortingOrder = 0;

        private Action onShowCompleted;
        private Action onHide;
        private Action onHideCompleted;
        private bool manualHide;

        private BaseGameDataContainer dataContainer;
        private ReadCharacterData characterData;
        private ReadTable<CharacterEntry> table;
        private GalleryClipAction showClip;

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

            var placeholderHeight = this.placeholderSize.rect.height;
            var fullHeight = UIMan.Instance.GetComponent<CanvasScaler>().referenceResolution.y;
            this.modelScale = placeholderHeight / fullHeight;

            this.onShowCompleted = null;
            this.onHide = null;
            this.onHideCompleted = null;
            this.manualHide = false;

            var index = 0;
            args.GetThenMoveNext(ref index, out this.onShowCompleted);
            args.GetThenMoveNext(ref index, out this.onHide);
            args.GetThenMoveNext(ref index, out this.onHideCompleted);

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
        }

        private void Initialize()
        {
            this.dataContainer = Settings.DataContainer;
            this.characterData = Settings.CharacterData;
            this.table = Settings.CharacterTable;
            this.showClip = Settings.ShowClip;

            this.characterList.Initialize(this.dataContainer, Settings.CharacterMap);
            this.currentImageId = -1;
            this.currentImageIndex = -1;
        }

        private void Deinitialize()
        {
            this.characterList.Deinitialize();
        }

        private void SelectCharacter(int characterId)
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

            var progress = this.dataContainer.GetPlayerCharacterProgress(this.currentCharacter);
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
                if (this.currentImageIndex < 0 ||
                    this.currentImageIndex > this.maxImageIndex)
                    this.currentImageIndex = 0;

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
                this.characterModel.Hide();

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
            var character = this.characterData.GetCharacter(this.currentCharacter.ToString().ToLower());
            var model = character?.P1 ?? string.Empty;

            if (string.IsNullOrEmpty(model))
                return;

            Vector3 position;
            float scale;

            if (this.fullPanel.IsHidden)
            {
                position = this.placeholderSize.position;
                scale = this.modelScale;
            }
            else
            {
                position = this.fullPanel.transform.position;
                scale = 1f;
            }

            this.characterModel = CubismManager.Instance.Show(model, position);

            if (!this.characterModel)
                return;

            this.characterModel.SetScale(scale);
            this.characterModel.SetLayer(this.characterLayer.value, this.sortingOrder);
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
            HideCharacterModel();
            UIDefaultActivity.Show(OnClose);
        }

        private void OnClose()
        {
            this.manualHide = true;
            Hide();
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

            var girlIndex = this.characterList.CurrentIndex + 1;

            if (girlIndex >= this.characterList.Count)
                girlIndex = 0;

            for (; girlIndex < this.characterList.Count; girlIndex++)
            {
                if (this.characterList.IsUnlock(girlIndex))
                    break;
            }

            SelectCharacter(this.characterList.GetCharacterId(girlIndex));
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

            var girlIndex = this.characterList.CurrentIndex - 1;

            if (girlIndex < 0)
                girlIndex = this.characterList.Count - 1;

            for (; girlIndex >= 0; girlIndex--)
            {
                if (this.characterList.IsUnlock(girlIndex))
                    break;
            }

            SelectCharacter(this.characterList.GetCharacterId(girlIndex));
        }
    }
}