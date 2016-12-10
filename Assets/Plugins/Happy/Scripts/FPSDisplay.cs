/*
 *
 *	Happy Framework
 *	by Toni Steyskal, 2016-2017
 * 
 */

using UnityEngine;
using UnityEngine.UI;
using System;

namespace Happy
{
	[DisallowMultipleComponent]
	public class FPSDisplay : MonoSingleton<FPSDisplay>
	{
		[Serializable]
		private struct FPSColor
		{
			public Color color;
			public int minimumFPS;
		}

		[Tooltip ("Number of frames taken for calculating average FPS.")]
		[Range (1, 60)]
		public int FrameRange = 60;
		public Text HighestFPSText;
		public Text AverageFPSText;
		public Text LowestFPSText;

		[SerializeField]
		private FPSColor[] _FPSColoring;

		[Header ("Read-Only")]
		[SerializeField]
		private int _highestFPS;
		[SerializeField]
		private int _averageFPS;
		[SerializeField]
		private int _lowestFPS;

		private int[] _fpsBuffer;
		private int _fpsBufferIndex;

		// Avoids triggering the garbage collector
		private readonly string[] _stringsFrom00To99 = {
			"00 FPS", "01 FPS", "02 FPS", "03 FPS", "04 FPS", "05 FPS", "06 FPS", "07 FPS", "08 FPS", "09 FPS",
			"10 FPS", "11 FPS", "12 FPS", "13 FPS", "14 FPS", "15 FPS", "16 FPS", "17 FPS", "18 FPS", "19 FPS",
			"20 FPS", "21 FPS", "22 FPS", "23 FPS", "24 FPS", "25 FPS", "26 FPS", "27 FPS", "28 FPS", "29 FPS",
			"30 FPS", "31 FPS", "32 FPS", "33 FPS", "34 FPS", "35 FPS", "36 FPS", "37 FPS", "38 FPS", "39 FPS",
			"40 FPS", "41 FPS", "42 FPS", "43 FPS", "44 FPS", "45 FPS", "46 FPS", "47 FPS", "48 FPS", "49 FPS",
			"50 FPS", "51 FPS", "52 FPS", "53 FPS", "54 FPS", "55 FPS", "56 FPS", "57 FPS", "58 FPS", "59 FPS",
			"60 FPS", "61 FPS", "62 FPS", "63 FPS", "64 FPS", "65 FPS", "66 FPS", "67 FPS", "68 FPS", "69 FPS",
			"70 FPS", "71 FPS", "72 FPS", "73 FPS", "74 FPS", "75 FPS", "76 FPS", "77 FPS", "78 FPS", "79 FPS",
			"80 FPS", "81 FPS", "82 FPS", "83 FPS", "84 FPS", "85 FPS", "86 FPS", "87 FPS", "88 FPS", "89 FPS",
			"90 FPS", "91 FPS", "92 FPS", "93 FPS", "94 FPS", "95 FPS", "96 FPS", "97 FPS", "98 FPS", "99 FPS"
		};

		void Awake()
		{
			if (Instance != this)
				Destroy (gameObject);
		}

		void Update ()
		{
			if (_fpsBuffer == null || _fpsBuffer.Length != FrameRange)
				InitializeBuffer ();

			UpdateBuffer ();
			CalculateFPS ();
			UpdateUI ();
		}

		private void InitializeBuffer ()
		{
			_fpsBuffer = new int[FrameRange];
			_fpsBufferIndex = 0;
		}

		private void UpdateBuffer ()
		{
			_fpsBuffer [_fpsBufferIndex++] = (int)(1f / Time.unscaledDeltaTime);

			if (_fpsBufferIndex >= FrameRange)
				_fpsBufferIndex = 0;
		}

		private void CalculateFPS ()
		{
			int sum = 0;
			int highestFPS = 0;
			int lowestFPS = int.MaxValue;

			for (int i = 0; i < FrameRange; i++)
			{
				int fps = _fpsBuffer [i];
				sum += fps;

				if (fps > highestFPS)
					highestFPS = fps;
	
				if (fps < lowestFPS)
					lowestFPS = fps;
			}

			_highestFPS = highestFPS;
			_averageFPS = sum / FrameRange;
			_lowestFPS = lowestFPS;
		}

		private void DisplayText (Text textObject, int fps)
		{
			textObject.text = _stringsFrom00To99 [Mathf.Clamp (fps, 0, 99)];

			for (int i = 0; i < _FPSColoring.Length; i++)
			{
				if (fps >= _FPSColoring [i].minimumFPS)
				{
					textObject.color = _FPSColoring [i].color;
					break;
				}
			}
		}

		private void UpdateUI ()
		{
			DisplayText (HighestFPSText, _highestFPS);
			DisplayText (AverageFPSText, _averageFPS);
			DisplayText	(LowestFPSText, _lowestFPS);
		}
	}
}
