using System;
using UnityEngine;
using UnityEngine.UI;

namespace MANA3DGames
{
	public class CommonTween
	{
		#region [Enumeration]

		public enum Ease
		{
			None = 0,
			BackIn, BackOut, BackInOut = 4,
			BounceIn, BounceOut, BounceInOut = 7,
			CircIn, CircOut, CircInOut = 10, 
			CubicIn, CubicOut, CubicInOut = 13,
			ElasticIn, ElasticOut, ElasticInOut = 16,
			ExpoIn, ExpoOut, ExpoInOut = 19, 
			Linear = 20, 
			QuadIn, QuadOut, QuadInOut = 23,
			QuartIn, QuartOut, QuartInOut = 26,
			QuintIn, QuintOut, QuintInOut = 29,
			SineIn, SineOut, SineInOut = 32
		}

		#endregion

		#region [General Functions]

		private static LTDescr SetEase( LTDescr tween, Ease ease )
		{
			if ( (int)ease >= 20 )
			{
				switch ( ease ) 
				{
				case Ease.Linear:
					tween.setEaseLinear();
					break;

				case Ease.QuadIn:
					tween.setEaseInQuad();
					break;
				case Ease.QuadOut:
					tween.setEaseOutQuad();
					break;
				case Ease.QuadInOut:
					tween.setEaseInOutQuad();
					break;

				case Ease.QuartIn:
					tween.setEaseInQuart();
					break;
				case Ease.QuartOut:
					tween.setEaseOutQuart();
					break;
				case Ease.QuartInOut:
					tween.setEaseInOutQuart();
					break;

				case Ease.QuintIn:
					tween.setEaseInQuint();
					break;
				case Ease.QuintOut:
					tween.setEaseOutQuint();
					break;
				case Ease.QuintInOut:
					tween.setEaseInOutQuint();
					break;

				case Ease.SineIn:
					tween.setEaseInSine();
					break;
				case Ease.SineOut:
					tween.setEaseOutSine();
					break;
				case Ease.SineInOut:
					tween.setEaseInOutSine();
					break;
				}
			}
			else
			{
				switch ( ease ) 
				{
				case Ease.None:
					break;

				case Ease.BackIn:
					tween.setEaseInBack();
					break;
				case Ease.BackOut:
					tween.setEaseOutBack();
					break;
				case Ease.BackInOut:
					tween.setEaseInOutBack();
					break;

				case Ease.BounceIn:
					tween.setEaseInBounce();
					break;
				case Ease.BounceOut:
					tween.setEaseOutBounce();
					break;
				case Ease.BounceInOut:
					tween.setEaseInOutBounce();
					break;

				case Ease.CircIn:
					tween.setEaseInCirc();
					break;
				case Ease.CircOut:
					tween.setEaseOutCirc();
					break;
				case Ease.CircInOut:
					tween.setEaseInOutCirc();
					break;

				case Ease.CubicIn:
					tween.setEaseInCubic();
					break;
				case Ease.CubicOut:
					tween.setEaseOutCubic();
					break;
				case Ease.CubicInOut:
					tween.setEaseInOutCubic();
					break;

				case Ease.ElasticIn:
					tween.setEaseInElastic();
					break;
				case Ease.ElasticOut:
					tween.setEaseOutElastic();
					break;
				case Ease.ElasticInOut:
					tween.setEaseInOutElastic();
					break;

				case Ease.ExpoIn:
					tween.setEaseInExpo();
					break;
				case Ease.ExpoOut:
					tween.setEaseOutExpo();
					break;
				case Ease.ExpoInOut:
					tween.setEaseInOutExpo();
					break;
				}
			}
			return tween;
		}

		private static LTDescr SetLastParameters( LTDescr tween, float delay, Action onCompleteAction, Ease ease )
		{
			tween.setDelay( delay );
			tween.setOnComplete( onCompleteAction );
			tween = SetEase( tween, ease );
			return tween;
		}

		#endregion

		#region [Alpha]

		public static LTDescr Alpha( GameObject go, float from, float to, float time, float delay = 0.0f, Action onCompleteAction = null, Ease ease = Ease.None, bool show = true, bool dontTweenChildren = false )
		{
			var tween = LeanTween.alpha( go, to, time );
			tween = SetLastParameters( tween, delay, onCompleteAction, ease );
			go.SetActive( show );
			return tween;
		}
			

		private static RectTransform GetRectAlpha( GameObject go, float from )
		{
			var graphic = go.GetComponent<Graphic>();
			if ( !graphic )
				return null;
			var rect = go.GetComponent<RectTransform>();
			var color = graphic.color;
			graphic.color = new Color( color.r, color.g, color.b, from );
			return rect;
		}

		public static LTDescr UIImageAlpha( UIMenu menu, string name, float from, float to, float time, float delay = 0.0f, Action onCompleteAction = null, Ease ease = Ease.None, bool show = true  )
		{
			return UIImageAlpha( menu.Get( name ), from, to, time, delay, onCompleteAction, ease, show );
		}
		
		public static LTDescr UIImageAlpha( GameObject go, float from, float to, float time, float delay = 0.0f, Action onCompleteAction = null, Ease ease = Ease.None, bool show = true, bool dontTweenChildren = false )
		{
			var rect = GetRectAlpha( go, from );
			var tween = LeanTween.alpha( rect, to, time );
			tween = SetLastParameters( tween, delay, onCompleteAction, ease );
			go.SetActive( show );
			return tween;
		}

		public static LTDescr UITextAlpha( GameObject go, float from, float to, float time, float delay = 0.0f, Action onCompleteAction = null, Ease ease = Ease.None, bool show = true  )
		{
			var rect = GetRectAlpha( go, from );
			var tween = LeanTween.alphaText( rect, to, time );
			tween = SetLastParameters( tween, delay, onCompleteAction, ease );
			go.SetActive( show );
			return tween;
		}

		public static void UIImageTextAlpha( UIMenu menu, string name, float from, float to, float time, float delay = 0.0f, Action onCompleteAction = null, Ease ease = Ease.None, bool show = true )
		{
			UIImageAlpha( menu.Get( name ), from, to, time, delay, onCompleteAction, ease, show );
			UITextAlpha( menu.Get( name ).GetComponentInChildren<TMPro.TextMeshProUGUI>().gameObject, from, to, time, delay, null, ease, show );
		}

		#endregion

		#region [Move]

		public static LTDescr MoveX( GOGroup group, string name, float from, float to, float time, float delay = 0.0f, Action onCompleteAction = null, Ease ease = Ease.None  )
		{
			group.SetLocalPositionX( name, from, true );
			var tween = LeanTween.moveX( group.Get( name ), to, time );
			tween = SetLastParameters( tween, delay, onCompleteAction, ease );
			return tween;
		}
		public static LTDescr MoveX( GameObject go, float from, float to, float time, float delay = 0.0f, Action onCompleteAction = null, Ease ease = Ease.None  )
		{
			var rect = go.GetComponent<Transform>();
			rect.localPosition = new Vector3( from, rect.localPosition.y, rect.localPosition.z );
			var tween = LeanTween.moveX( go, to, time );
			tween = SetLastParameters( tween, delay, onCompleteAction, ease );
			return tween;
		}

		public static LTDescr MoveY( GOGroup group, string name, float from, float to, float time, float delay = 0.0f, Action onCompleteAction = null, Ease ease = Ease.None  )
		{
			group.SetLocalPositionY( name, from, true );
			var tween = LeanTween.moveY( group.Get( name ), to, time );
			tween = SetLastParameters( tween, delay, onCompleteAction, ease );
			return tween;
		}
		public static LTDescr MoveY( GameObject go, float from, float to, float time, float delay = 0.0f, Action onCompleteAction = null, Ease ease = Ease.None  )
		{
			var rect = go.GetComponent<Transform>();
			rect.localPosition = new Vector3( rect.localPosition.x, from, rect.localPosition.z );
			var tween = LeanTween.moveY( go, to, time );
			tween = SetLastParameters( tween, delay, onCompleteAction, ease );
			return tween;
		}


		public static LTDescr UIMoveX( UIMenu menu, string name, float from, float to, float time, float delay = 0.0f, Action onCompleteAction = null, Ease ease = Ease.None  )
		{
			menu.SetAnchoredPositionX( name, from, true );
			var tween = LeanTween.moveX( menu.GetRect( name ), to, time );
			tween = SetLastParameters( tween, delay, onCompleteAction, ease );
			return tween;
		}
		public static LTDescr UIMoveX( GameObject go, float from, float to, float time, float delay = 0.0f, Action onCompleteAction = null, Ease ease = Ease.None  )
		{
			var rect = go.GetComponent<RectTransform>();
			rect.anchoredPosition = new Vector2( from, rect.anchoredPosition.y );
			var tween = LeanTween.moveX( rect, to, time );
			tween = SetLastParameters( tween, delay, onCompleteAction, ease );
			return tween;
		}

		public static LTDescr UIMoveY( UIMenu menu, string name, float from, float to, float time, float delay = 0.0f, Action onCompleteAction = null, Ease ease = Ease.None  )
		{
			menu.SetAnchoredPositionY( name, from, true );
			var tween = LeanTween.moveY( menu.GetRect( name ), to, time );
			tween = SetLastParameters( tween, delay, onCompleteAction, ease );
			return tween;
		}
		public static LTDescr UIMoveY( GameObject go, float from, float to, float time, float delay = 0.0f, Action onCompleteAction = null, Ease ease = Ease.None  )
		{
			var rect = go.GetComponent<RectTransform>();
			rect.anchoredPosition = new Vector2( rect.anchoredPosition.x, from );
			var tween = LeanTween.moveY( rect, to, time );
			tween = SetLastParameters( tween, delay, onCompleteAction, ease );
			return tween;
		}

		#endregion

		#region [Rotate]

		public static LTDescr RotateX( GOGroup group, string name, float from, float to, float time, float delay = 0.0f, Action onCompleteAction = null, Ease ease = Ease.None )
		{
			return RotateX( group.Get( name ), from, to, time, delay, onCompleteAction, ease );
		}
		public static LTDescr RotateX( GameObject go, float from, float to, float time, float delay = 0.0f, Action onCompleteAction = null, Ease ease = Ease.None )
		{
			var trans = go.GetComponent<Transform>();

			var fromRotation = Quaternion.Euler( from, trans.localEulerAngles.y, trans.localEulerAngles.z );
			trans.localRotation = Quaternion.Lerp( trans.localRotation, fromRotation, 0.0f );

			var tween = LeanTween.rotateAroundLocal( go, Vector3.right, to, time );

			tween = SetLastParameters( tween, delay, onCompleteAction, ease );
			return tween;
		}
			
		public static LTDescr RotateY( GOGroup group, string name, float from, float to, float time, float delay = 0.0f, Action onCompleteAction = null, Ease ease = Ease.None )
		{
			return RotateY( group.Get( name ), from, to, time, delay, onCompleteAction, ease );
		}
		public static LTDescr RotateY( GameObject go, float from, float to, float time, float delay = 0.0f, Action onCompleteAction = null, Ease ease = Ease.None )
		{
			var trans = go.GetComponent<Transform>();

			var fromRotation = Quaternion.Euler( trans.localEulerAngles.x, from, trans.localEulerAngles.z );
			trans.localRotation = Quaternion.Lerp( trans.localRotation, fromRotation, 0.0f );

			var tween = LeanTween.rotateAroundLocal( go, Vector3.up, to, time );
			//tween.setFrom( from ); DO NOT ADD THIS!!!!!

			tween = SetLastParameters( tween, delay, onCompleteAction, ease );
			return tween;
		}

		public static LTDescr RotateZ( GOGroup group, string name, float from, float to, float time, float delay = 0.0f, Action onCompleteAction = null, Ease ease = Ease.None )
		{
			return RotateZ( group.Get( name ), from, to, time, delay, onCompleteAction, ease );
		}
		public static LTDescr RotateZ( GameObject go, float from, float to, float time, float delay = 0.0f, Action onCompleteAction = null, Ease ease = Ease.None )
		{
			var trans = go.GetComponent<Transform>();

			var fromRotation = Quaternion.Euler( trans.localEulerAngles.x, trans.localEulerAngles.y, from );
			trans.localRotation = Quaternion.Lerp( trans.localRotation, fromRotation, 0.0f );

			var tween = LeanTween.rotateAroundLocal( go, Vector3.forward, to, time );
			//tween.setFrom( from ); DO NOT ADD THIS!!!!!

			tween = SetLastParameters( tween, delay, onCompleteAction, ease );
			return tween;
		}


		public static LTDescr UIRotateZ( UIMenu menu, string name, float from, float to, float time, float delay = 0.0f, Action onCompleteAction = null, Ease ease = Ease.None )
		{
			return UIRotateZ( menu.Get( name ), from, to, time, delay, onCompleteAction, ease );
		}
		public static LTDescr UIRotateZ( GameObject go, float from, float to, float time, float delay = 0.0f, Action onCompleteAction = null, Ease ease = Ease.None )
		{
			var rect = go.GetComponent<RectTransform>();

			var fromRotation = Quaternion.Euler( rect.localEulerAngles.x, rect.localEulerAngles.y, from );
			rect.localRotation = Quaternion.Lerp( rect.localRotation, fromRotation, 0.0f );

			var tween = LeanTween.rotateAroundLocal( rect, Vector3.forward, to, time );
			//tween.setFrom( from ); DO NOT ADD THIS!!!!!

			tween = SetLastParameters( tween, delay, onCompleteAction, ease );
			return tween;
		}

		#endregion

		#region [Scale]

		public static LTDescr UIScale( UIMenu menu, string name, Vector2 from, Vector2 to, float time, float delay = 0.0f, Action onCompleteAction = null, Ease ease = Ease.None  )
		{
			menu.SetRectScale( name, from, true );
			var tween = LeanTween.scale( menu.GetRect( name ), to, time );
			tween = SetLastParameters( tween, delay, onCompleteAction, ease );
			return tween;
		}
		public static LTDescr UIScale( GameObject go, Vector2 from, Vector2 to, float time, float delay = 0.0f, Action onCompleteAction = null, Ease ease = Ease.None  )
		{
			var rect = go.GetComponent<RectTransform>();
			rect.localScale = from;
			go.SetActive( true );

			var tween = LeanTween.scale( rect, to, time );
			tween = SetLastParameters( tween, delay, onCompleteAction, ease );
			return tween;
		}

		public static LTDescr UIScaleX( UIMenu menu, string name, float from, float to, float time, float delay = 0.0f, Action onCompleteAction = null, Ease ease = Ease.None  )
		{
			menu.SetRectScaleX( name, from, true );
			var vecTo = new Vector2( to, menu.GetRectScaleY( name ) );
			var tween = LeanTween.scale( menu.GetRect( name ), vecTo, time );
			tween = SetLastParameters( tween, delay, onCompleteAction, ease );
			return tween;
		}
		public static LTDescr UIScaleX( GameObject go, float from, float to, float time, float delay = 0.0f, Action onCompleteAction = null, Ease ease = Ease.None  )
		{
			var scale = go.GetComponent<RectTransform>().localScale;
			go.GetComponent<RectTransform>().localScale = new Vector3( from, scale.y, scale.z );
			var vecTo = new Vector2( to, scale.y );
			var tween = LeanTween.scale( go, vecTo, time );
			tween = SetLastParameters( tween, delay, onCompleteAction, ease );
			return tween;
		}

		public static LTDescr UIScaleY( UIMenu menu, string name, float from, float to, float time, float delay = 0.0f, Action onCompleteAction = null, Ease ease = Ease.None  )
		{
			menu.SetRectScaleY( name, from, true );
			var vecTo = new Vector2( menu.GetRectScaleX( name ), to );
			var tween = LeanTween.scale( menu.GetRect( name ), vecTo, time );
			tween = SetLastParameters( tween, delay, onCompleteAction, ease );
			return tween;
		}
		public static LTDescr UIScaleY( GameObject go, float from, float to, float time, float delay = 0.0f, Action onCompleteAction = null, Ease ease = Ease.None  )
		{
			var scale = go.GetComponent<RectTransform>().localScale;
			go.GetComponent<RectTransform>().localScale = new Vector3( scale.x, from, scale.z );
			var vecTo = new Vector2( scale.x, to );
			var tween = LeanTween.scale( go, vecTo, time );
			tween = SetLastParameters( tween, delay, onCompleteAction, ease );
			return tween;
		}

		#endregion

		#region [Fill]

		public static LTDescr UIFill( UIMenu menu, string name, float from, float to, float time, float delay = 0.0f, Action onCompleteAction = null, Ease ease = Ease.None )
		{
			Image img = menu.Get( name ).GetComponent<Image>();
			img.fillAmount = from;
			System.Action<float> action = (val)=> { img.fillAmount = val; };

			var tween = LeanTween.value( img.gameObject, action, from, to, time );
			tween = SetLastParameters( tween, delay, onCompleteAction, ease );
			return tween;
		}

		public static LTDescr UIFill( GameObject go, float from, float to, float time, float delay = 0.0f, Action onCompleteAction = null, Ease ease = Ease.None )
		{
			Image img = go.GetComponent<Image>();
			img.fillAmount = from;
			System.Action<float> action = (val)=> { img.fillAmount = val; };

			var tween = LeanTween.value( img.gameObject, action, from, to, time );
			tween = SetLastParameters( tween, delay, onCompleteAction, ease );
			return tween;
		}

		#endregion

		#region [Cancel Tween]

		public static void Cancel( GameObject go, bool callOnComplete )
		{
			LeanTween.cancel( go, callOnComplete );
		}

		#endregion
	}
}