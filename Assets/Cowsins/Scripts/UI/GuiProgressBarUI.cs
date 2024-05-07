using System.Globalization;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace Assets.ProgressBars.Scripts
{
    public class GuiProgressBarUI : MonoBehaviour
    {
	    public Slider Slider;
		private const float Epsilon = 0.003f;
        private const string OffsetProperty = "_Offset";
        private const string AnimOffsetProperty = "_AnimOffset";
		private const string MainTextureProperty = "_MainTex";
		private const string MaskTextureProperty = "_MaskTex";

		private Color[] _maskPixels;

		private float _animOffset;

		[SerializeField] 
		private float _value;
		public float Value {
			get
			{
				return _value;
			}
			set
			{
				_value = value;
				ValueUpdated();
			}
		}
		
		[SerializeField] 
		private TextureWrapMode _textureWrapMode;
		public TextureWrapMode TextureWrapMode {
			get
			{
				return _textureWrapMode;
			}
			set
			{
				_textureWrapMode = value;
			}
		}

		[SerializeField]
        private RectTransform _knob;
        public RectTransform Knob
        {
			get
			{
				return _knob;
			}
			set
			{
				_knob = value;
			}
		}

		[SerializeField] 
		private Text _textMesh;
        public Text TextMesh
        {
			get
			{
				return _textMesh;
			}
			set
			{
				_textMesh = value;
			}
		}

		[SerializeField] 
		private int _digitsAfterComma;
		public int DigitsAfterComma {
			get
			{
				return _digitsAfterComma;
			}
			set
			{
				_digitsAfterComma = value;
				ValueUpdated();
			}
		}

		[SerializeField] 
		private string _textSuffix;
		public string TextSuffix {
			get
			{
				return _textSuffix;
			}
			set
			{
				_textSuffix = value;
				ValueUpdated();
			}
		}

		[SerializeField] 
		private float _knobPositionOffset;
		public float KnobPositionOffset {
			get
			{
				return _knobPositionOffset;
			}
			set
			{
				_knobPositionOffset = value;
			}
		}

		[SerializeField] 
		private float _knobMinPercent;
		public float KnobMinPercent {
			get
			{
				return _knobMinPercent;
			}
			set
			{
				_knobMinPercent = value;
			}
		}

		[SerializeField] 
		private float _knobMaxPercent;
		public float KnobMaxPercent {
			get
			{
				return _knobMaxPercent;
			}
			set
			{
				_knobMaxPercent = value;
			}
		}

		[SerializeField] 
		private TextIndicationType _textIndication;
		public TextIndicationType TextIndication {
			get
			{
				return _textIndication;
			}
			set
			{
				_textIndication = value;
			}
		}

		[SerializeField] 
		private int _sectorsCount;
		public int SectorsCount {
			get
			{
				return _sectorsCount;
			}
			set
			{
				_sectorsCount = value;
			}
		}

        
        private void Start()
        {
	        Slider.maxValue = 1;
	        Slider.value = 0.15f;//图片本身问题，0.15之前是不显示的。
            GetComponent<Image>().material = Instantiate(GetComponent<Image>().material);
			var rendererSprite = GetComponent<Image>().sprite;
		}
        /// <summary>
		/// Sets the position of knob using current value offset
		/// depends on bar width
		/// </summary>
		private void SetKnobPositionByBarWidth ()
		{
			
		}

		/// <summary>
		/// sets the positon of knob using value offset
		/// depends on mask pixels data
		/// </summary>
		

		/// <summary>
		/// set current percent value and update knob position
		/// </summary>
		private void ValueUpdated()
		{
			SetPercent(_value);
		}
		
		/// <summary>
        /// update shader vars with current percent value
        /// </summary>
        /// <param name="value"></param>
        public void SetPercent(float value)
		{
			Slider.value = value;
		}

		/// <summary>
		/// Formats the number to displayed string
		/// </summary>
		/// <returns>The number.</returns>
		/// <param name="number">Number.</param>
		private string FormatNumber(float number)
		{
			var multiplier = _digitsAfterComma * 10;
			if (multiplier == 0)
				return "任务完成" + ((int)(number)) + "%" +_textSuffix;
			string result = ((int)(number * multiplier) / (float)multiplier).ToString(CultureInfo.InvariantCulture);
			if (result.IndexOf(".", StringComparison.Ordinal) == -1) {
				int i = _digitsAfterComma;
				string appendix = string.Empty;
				while(--i > -1)
				{
					appendix += "0" + _textSuffix;
				}
				result = string.Format("{0}.{1}", result, appendix);
			}
			return result;
		}
    }
}
