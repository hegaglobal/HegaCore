using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Rendering;
using Cysharp.Threading.Tasks;
using HegaCore.UI;
using Sirenix.OdinInspector;
using Live2D.Cubism.Framework.MouthMovement;
using Live2D.Cubism.Framework.Raycasting;
using UnityEngine.Serialization;

namespace HegaCore
{
    public sealed class CubismController : MonoBehaviour
    {
        [SerializeField] private bool useScaleOne = true;

        [SerializeField] private Animator animator = null;

        [SerializeField] private CubismRenderController cubismRenderer = null;

        [SerializeField] private SpriteRenderer spriteRenderer = null;

        [SerializeField] private bool setOrderOnAwake = false;

        [BoxGroup("Order & Layer")] [SerializeField]
        private bool withLayer = false;

        [BoxGroup("Order & Layer")] [SerializeField, HideIf(nameof(withLayer))]
        private int orderInLayer = default;

        [BoxGroup("Order & Layer")] [SerializeField, ShowIf(nameof(withLayer)), InlineProperty]
        private SingleOrderLayer orderAndLayer = default;

        public Animator Animator => this.animator;

        public float TempAlpha { get; set; }

        public Vector3 LocalScale { get; private set; }

        public string Id; //{ get; private set; }

        public bool IsActive => this.gameObject && this.gameObject.activeSelf;

        private bool hasIdAnim;
        private bool hasBodyAnim;
        private bool hasEmoAnim;
        private Color color;
        
        [FormerlySerializedAs("_live2DCharInteract")] [BoxGroup("Interact")]
        public Live2DCharInteract live2DCharInteract;
        [BoxGroup("Interact")] 
        public int curClothesID;
        // [BoxGroup("Interact")]
        // public List<int> allowClothesID;


        [BoxGroup("Lip syns")] public CubismMouthController MouthController;
        [BoxGroup("Lip syns")] public CubismAudioMouthInput AudioMouthInput;

        [BoxGroup("Custom Parameters")] [ShowInInspector, ReadOnly]
        private List<CubismParameterCustomControl> parameterControls = new List<CubismParameterCustomControl>();
        
        [Button(ButtonSizes.Large)]
        private void GetRef()
        {
            this.animator = GetComponentInChildren<Animator>();
            this.cubismRenderer = GetComponentInChildren<CubismRenderController>();
            this.spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        private void Awake()
        {
            GetRef();
            curClothesID = -1;
            this.LocalScale = this.useScaleOne ? Vector3.one : this.transform.localScale;

            if (!this.setOrderOnAwake)
                return;

            if (this.withLayer)
            {
                SetSortingOrder(this.orderAndLayer.Order);
                SetLayer(this.orderAndLayer.Layer);
            }
            else
            {
                SetSortingOrder(this.orderInLayer);
            }
        }

        public void Initialize(string id, bool hasIdAnim, bool hasBodyAnim, bool hasEmoAnim)
        {
            this.Id = id;
            this.hasIdAnim = hasIdAnim;
            this.hasBodyAnim = hasBodyAnim;
            this.hasEmoAnim = hasEmoAnim;
        }

        public void Hide()
        {
            SetLayer(0);

            this.color = Color.white.With(a: 0f);
            SetColor(in this.color);

            this.transform.localScale = this.LocalScale;

            LateHide().Forget();
        }

        private async UniTaskVoid LateHide()
        {
            await UniTask.DelayFrame(2);

            if (this.gameObject.activeSelf)
                this.gameObject.SetActive(false);
        }

        public void Hide(in Vector3 position)
        {
            this.transform.position = position;
            Hide();
        }

        public void Set(in Vector3 position)
        {
            this.transform.position = position;
        }

        public void Set(in Vector3 position, in Color color)
        {
            this.transform.position = position;
            SetColor(color);
        }

        public void Show(float alpha = 1f)
        {
            if (!this.gameObject.activeSelf)
                this.gameObject.SetActive(true);

            SetAlpha(alpha);
        }

        public void Show(in Color color)
        {
            if (!this.gameObject.activeSelf)
                this.gameObject.SetActive(true);

            SetColor(in color);
        }

        public void Show(in Vector3 position, float alpha = 1f)
        {
            this.transform.position = position;
            Show(alpha);
        }

        public void Show(in Vector3 position, in Color color)
        {
            this.transform.position = position;
            Show(color);
        }

        public void SetScale(float value)
            => this.transform.localScale = this.LocalScale * value;

        public void SetLayer(int layer, int sortingOrder = 0)
        {
            if (this.cubismRenderer)
            {
                foreach (var renderer in this.cubismRenderer.Renderers)
                {
                    renderer.gameObject.layer = layer;
                }

                this.cubismRenderer.SortingOrder = sortingOrder;
            }

            if (this.spriteRenderer)
            {
                this.spriteRenderer.gameObject.layer = layer;
                this.spriteRenderer.sortingOrder = sortingOrder;
            }
        }

        public void SetSortingOrder(int sortingOrder)
        {
            if (this.cubismRenderer)
            {
                this.cubismRenderer.SortingOrder = sortingOrder;
            }

            if (this.spriteRenderer)
            {
                this.spriteRenderer.sortingOrder = sortingOrder;
            }
        }

        public void SetLayer(in SingleOrderLayer orderLayer)
        {
            if (this.cubismRenderer)
            {
                foreach (var renderer in this.cubismRenderer.Renderers)
                {
                    renderer.gameObject.layer = orderLayer.Layer.value;
                }

                this.cubismRenderer.SortingOrder = orderLayer.Order;
            }

            if (this.spriteRenderer)
            {
                this.spriteRenderer.gameObject.layer = orderLayer.Layer.value;
                this.spriteRenderer.sortingOrder = orderLayer.Order;
            }
        }

        public void PlayAnimation(int id)
        {
            if (this.animator && this.hasIdAnim)
                this.animator.SetInteger(Params.ID, id);
        }

        public void PlayBodyAnimation(int id)
        {
            if (this.animator && this.hasBodyAnim)
                this.animator.SetInteger(Params.Body, id);
        }

        public void PlayEmoAnimation(int id)
        {
            if (this.animator && this.hasEmoAnim)
                this.animator.SetInteger(Params.Emo, id);
        }

        public Color GetColor()
            => this.color;

        public void SetColor(in Color value)
        {
            this.color = value;

            if (this.cubismRenderer)
            {
                foreach (var renderer in this.cubismRenderer.Renderers)
                {
                    renderer.Color = value;
                }
            }

            if (this.spriteRenderer)
            {
                this.spriteRenderer.color = value;
            }
        }

        public void SetColor(Color value)
            => SetColor(in value);

        public float GetAlpha()
            => this.TempAlpha;

        public void SetAlpha(float value)
        {
            this.TempAlpha = Mathf.Clamp(value, 0f, 1f);

            if (this.cubismRenderer)
            {
                foreach (var renderer in this.cubismRenderer.Renderers)
                {
                    var color = renderer.Color.With(a: this.TempAlpha);
                    renderer.Color = color;
                }
            }

            if (this.spriteRenderer)
            {
                var color = this.spriteRenderer.color.With(a: this.TempAlpha);
                this.spriteRenderer.color = color;
            }
        }

        private static class Params
        {
            public const string ID = nameof(ID);
            public const string Body = nameof(Body);
            public const string Emo = nameof(Emo);
        }

        #region Lip syns

        public void InitLipsyns()
        {
            if (AudioMouthInput != null)
                AudioMouthInput.AudioInput = AudioManager.Instance.VoiceSource;
        }

        public void EnableLipsyns(bool enable)
        {
            UnuLogger.Log("Enable Lipsyns : " + enable);
            AudioMouthInput.enabled = enable;
            MouthController.enabled = enable;
        }

        public void SelectMouth(int mouthTypeValue)
        {
            //MouthController.currentMouthTypeValue = mouthTypeValue;
        }

        public void EnableControlMouthType(bool enable)
        {
            //MouthController.overrideMouthType = enable;
        }

        #endregion

        #region Control Parameters

        public void AddParameterCustomControl(CubismParameterCustomControl customControl)
        {
            parameterControls.Add(customControl);
        }

        public void BlendParamToValue(string paramName, float value, float duration = 0f, float delay = 0f)
        {
            var custom = GetParameterControl(paramName);
            custom?.BlendToValue(value, duration, delay);
        }

        private CubismParameterCustomControl GetParameterControl(string parameterName)
        {
            if (!string.IsNullOrEmpty(parameterName))
            {
                for (int i = 0; i < parameterControls.Count; i++)
                {
                    if (string.Equals(parameterName, parameterControls[i].name) ||
                        string.Equals(parameterName, parameterControls[i].customParamName))
                    {
                        return parameterControls[i];
                    }
                }
            }

            Debug.Log($"NO PARAM NAME: {parameterName}");
            return null;
        }

        #endregion

        #region Clothes

        public void ChangeClothes(int clothesID)
        {
            if (curClothesID == clothesID)
            {
                return;
            }

            curClothesID = clothesID;
            BlendParamToValue("Var", clothesID);
        }


        #endregion

        #region Interact

        public bool StartInteract(CubismRaycastHit[] hits)
        {
            return live2DCharInteract != null && live2DCharInteract.StartInteract(hits);
        }

        public void UpdateInteractDrag(Vector2 delta)
        {
            live2DCharInteract?.UpdateInteractDrag(delta);
        }

        public void EndInteract()
        {
            live2DCharInteract?.EndInteract();
        }

        public Dictionary<string, float> GetInteractPartValues()
        {
            if (live2DCharInteract != null)
            {
                return live2DCharInteract.GetInteractPartValues();
            }

            return null;
        }

        public void LoadInteractPartValues()
        {
            //Debug.Log("LoadInteractPartValues: "  + curClothesID);
            if (DataManager.Instance.DarkLord)// && allowClothesID.Contains(curClothesID))
            {
                var userCharacterData = DataManager.DataContainer.Player.GetUserCharacter(Id);
                live2DCharInteract?.LoadInteractPartValues(userCharacterData.interactValues);
            }
            else
                live2DCharInteract?.ResetInteractValue();
        }
        
        #endregion
    }
}