using UnityEngine;
using UnityEngine.UI;

namespace MicroCombo
{
	public class InterpFadeUI : InterpFadeBase
	{
		// Visuals
		public CanvasGroup fadeGroup = null;
		public Graphic[] fadeGraphics = null;

		private bool _interactive = false;
		private bool _blocksRaycasts = false;


		protected override void _OnCache()
		{
			base._OnCache();
			if (fadeGroup != null)
			{
				_interactive = fadeGroup.interactable;
				_blocksRaycasts = fadeGroup.blocksRaycasts;
			}
		}

		protected override void _ApplyColor(Color color)
		{
			if (fadeGroup != null)
			{
				fadeGroup.alpha = color.a;

				bool enabled = color.a > 0;
				fadeGroup.interactable = enabled && _interactive;
				fadeGroup.blocksRaycasts = enabled && _blocksRaycasts;
			}

			for (int i = 0; i < fadeGraphics.Length; i++)
			{
				fadeGraphics[i].color = color;
			}
		}
	}
}