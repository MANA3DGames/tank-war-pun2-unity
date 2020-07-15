using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using TMPro;

namespace MANA3DGames
{
	public class UIMenu : GOGroup
    {
		#region [Constructors]

		public UIMenu( GameObject root ) : base( root )
        {
        }
			
		#endregion

		#region [Listeners Functions]

		public bool RemoveAllListenerOnButton( string componentName, bool inner = false  )
		{
			GameObject go;
			if ( gameObjects.TryGetValue( componentName, out go ) )
			{
				Button btn = inner ? go.GetComponentInChildren<Button>() : go.GetComponent<Button>();
				if ( btn )
				{
					btn.onClick.RemoveAllListeners();
					return true;
				}
			}

			return false;
		}


		public bool AddListenerToButton( string componentName, UnityEngine.Events.UnityAction action, bool inner = false )
        {
            GameObject go;
            if ( gameObjects.TryGetValue( componentName, out go ) )
            {
				Button btn = inner ? go.GetComponentInChildren<Button>() : go.GetComponent<Button>();
                if ( btn )
                {
                    btn.onClick.AddListener( action );
                    return true;
                }
            }

            return false;
        }

		public bool AddListenerToRepeatButton( string componentName, UnityEngine.Events.UnityAction downAction, UnityEngine.Events.UnityAction upAction )
		{
			AddEventTrigger( componentName, EventTriggerType.PointerDown, ( act )=> { downAction.Invoke(); } );
			return AddEventTrigger( componentName, EventTriggerType.PointerUp, ( act )=> { upAction.Invoke(); } );
		}


		public bool AddEventTrigger( string componentName, EventTriggerType type, UnityEngine.Events.UnityAction<BaseEventData> action )
		{
			GameObject go;
			if ( gameObjects.TryGetValue( componentName, out go ) )
				return AddEventTrigger( go, type, action );

			return false;
		}

		public static bool AddEventTrigger( GameObject go, EventTriggerType type, UnityEngine.Events.UnityAction<BaseEventData> action )
		{
			EventTrigger trigger = go.GetComponent<EventTrigger>();
			if ( !trigger )
				trigger = go.AddComponent<EventTrigger>();

			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = type;
			entry.callback.AddListener( action );
			//trigger.delegates.Add( entry );
			trigger.triggers.Add( entry );
			return true;
		}


		public bool AddListenerToSliderValueChanged( string componentName, UnityEngine.Events.UnityAction<float> action, bool inner = false )
		{
			GameObject go;
			if ( gameObjects.TryGetValue( componentName, out go ) )
			{
				Slider slider = inner ? go.GetComponentInChildren<Slider>() : go.GetComponent<Slider>();
				if ( slider )
				{
					slider.onValueChanged.AddListener( action );
					return true;
				}
			}

			return false;
		}

		#endregion

		#region [Set Functions]

//        public bool SetText( string componentName, string text, bool internalTxt = false )
//        {
//            GameObject go;
//            if ( gameObjects.TryGetValue( componentName, out go ) )
//            {
//                Text txt = internalTxt ? go.GetComponentInChildren<Text>() : go.GetComponent<Text>();
//                if ( txt )
//                {
//                    txt.text = text;
//                    return true;
//                }
//            }
//
//            return false;
//        }

		public bool SetText( string componentName, string text, bool internalTxt = false )
		{
			GameObject go;
			if ( gameObjects.TryGetValue( componentName, out go ) )
			{
				TextMeshProUGUI txt = internalTxt ? go.GetComponentInChildren<TextMeshProUGUI>() : go.GetComponent<TextMeshProUGUI>();
				if ( txt )
				{
					txt.text = text;
					return true;
				}
			}

			return false;
		}


		public bool SetAlpha( string componentName, float alpha, bool internalImg = false, bool show = true )
		{
			GameObject go;
			if ( gameObjects.TryGetValue( componentName, out go ) )
			{
				if ( internalImg )
				{
					Graphic[] graphics = go.GetComponentsInChildren<Graphic>();

					if ( graphics != null )
					{
                        for (int i = 0; i < graphics.Length; i++)
                        {
                            if (graphics[i].gameObject == go  )
								continue;
							var color = graphics[i].color;
                            graphics[i].color = new Color( color.r, color.g, color.b, alpha );
							go.SetActive( show );
						}
						return true;
					}
				}
				else
				{
					Graphic graphic = go.GetComponent<Graphic>();

					if ( graphic )
					{
						var color = graphic.color;
						graphic.color = new Color( color.r, color.g, color.b, alpha );
						go.SetActive( show );
						return true;
					}
				}
			}

			return false;
		}

		public bool SetAlphaChildren( string componentName, float alpha, bool show = true )
		{
			GameObject go;
			if ( gameObjects.TryGetValue( componentName, out go ) )
			{
				Graphic[] graphics = go.GetComponentsInChildren<Graphic>();
				if ( graphics != null )
				{
					for ( int i = 0; i < graphics.Length; i++ ) 
					{
						if (graphics[i].gameObject == go  )
							continue;
						var color = graphics[i].color;
                        graphics[i].color = new Color( color.r, color.g, color.b, alpha );
						go.SetActive( show );
					}
					return true;
				}
			}

			return false;
		}

        public bool SetColor( string componentName, Color color, bool internalTxt = false )
        {
            GameObject go;
            if ( gameObjects.TryGetValue( componentName, out go ) )
            {
				if ( internalTxt )
				{
					Graphic[] graphics = go.GetComponentsInChildren<Graphic>();

					if ( graphics != null )
					{
                        for (int i = 0; i < graphics.Length; i++)
						{
							if (graphics[i].gameObject == go  )
								continue;
                            graphics[i].color = color;
						}
						return true;
					}
				}
				else
				{
					Graphic graphic = go.GetComponent<Graphic>();

					if ( graphic )
					{
						graphic.color = color;
						return true;
					}
				}

            }

            return false;
        }

        public bool SetInputFiledText( string componentName, string text, bool internalTxt = false )
        {
            GameObject go;
            if ( gameObjects.TryGetValue( componentName, out go ) )
            {
                InputField txt = internalTxt ? go.GetComponentInChildren<InputField>() : go.GetComponent<InputField>();
                if ( txt )
                {
                    txt.text = text;
                    return true;
                }
            }

            return false;
        }

		public bool SetImageSprite( string componentName, Sprite sprite, bool internalImg = false, bool activate = true, bool setNative = false )
		{
			GameObject go;

			if ( gameObjects.TryGetValue( componentName, out go ) )
			{
				Image img = go.GetComponent<Image>();
				if ( internalImg )
				{
					if ( go.transform.childCount > 0 )
						img = go.transform.GetChild(0).GetComponent<Image>();
				}

				if ( img )
				{
					img.sprite = sprite;

					if ( setNative )
						img.SetNativeSize();

					if ( activate )
						go.gameObject.SetActive( true );
					return true;
				}
			}

			return false;
		}

		public void SetImageNaitve( string componentName, bool internalImg = false )
		{
			GameObject go;

			if ( gameObjects.TryGetValue( componentName, out go ) )
			{
				Image img = go.GetComponent<Image>();
				if ( internalImg )
				{
					if ( go.transform.childCount > 0 )
						img = go.transform.GetChild(0).GetComponent<Image>();
				}

				if ( img )
					img.SetNativeSize();
			}
		}


		public void SetButtonInteractable( string name, bool val )
		{
			GameObject component = null;
			if ( gameObjects.TryGetValue( name, out component ) )
				component.GetComponent<Button>().interactable = val;
			else
				Debug.Log( name + " couldn't be found in " + root.name );
		}

		public void SetAllButtonsInteractable( bool val )
		{
			foreach ( var item in gameObjects )
			{
				var btn = item.Value.GetComponent<Button>();
				if ( btn )
					btn.interactable = val;
			}
		}
			

		public void SetImageFillVertical( string name, Image.OriginVertical origin )
		{
			Image img = Get( name ).GetComponent<Image>();
			img.type = Image.Type.Filled;
			img.fillMethod = Image.FillMethod.Vertical;
			img.fillOrigin = (int)origin;
		}
		public void SetImageFillHorizontal( string name, Image.OriginHorizontal origin )
		{
			Image img = Get( name ).GetComponent<Image>();
			img.type = Image.Type.Filled;
			img.fillMethod = Image.FillMethod.Horizontal;
			img.fillOrigin = (int)origin;
		}

		public void SetImageFillValue( string name, float val )
		{
			Get( name ).GetComponent<Image>().fillAmount = val;
		}

		public void SetSliderValue( string name, float val )
		{
			Get( name ).GetComponent<Slider>().value = val;
		}


		public void SetRectScale( string name, Vector2 scale, bool show = true )
		{
			GameObject component = null;
			if ( gameObjects.TryGetValue( name, out component ) )
			{
				component.GetComponent<RectTransform>().localScale = scale;
				component.SetActive( show );
			}
			else
				Debug.Log( name + " couldn't be found in " + root.name );
		}

		public void SetRectScaleX( string name, float val, bool show = true )
		{
			GameObject component = null;
			if ( gameObjects.TryGetValue( name, out component ) )
			{
				var scale = component.GetComponent<RectTransform>().localScale;
				component.GetComponent<RectTransform>().localScale = new Vector3( val, scale.y, scale.z );
				component.SetActive( show );
			}
			else
				Debug.Log( name + " couldn't be found in " + root.name );
		}

		public void SetRectScaleY( string name, float val, bool show = true )
		{
			GameObject component = null;
			if ( gameObjects.TryGetValue( name, out component ) )
			{
				var scale = component.GetComponent<RectTransform>().localScale;
				component.GetComponent<RectTransform>().localScale = new Vector3( scale.x, val, scale.z );
				component.SetActive( show );
			}
			else
				Debug.Log( name + " couldn't be found in " + root.name );
		}



		public void SetAnchoredPosition( string name, Vector2 pos, bool show = false )
		{
			GameObject component = null;
			if ( gameObjects.TryGetValue( name, out component ) )
			{
				component.GetComponent<RectTransform>().anchoredPosition = pos;
				component.SetActive( show );
			}
			else
				Debug.Log( name + " couldn't be found in " + root.name );
		}

		public void SetAnchoredPositionX( string name, float x, bool show = false )
		{
			GameObject component = null;
			if ( gameObjects.TryGetValue( name, out component ) )
			{
				var y = component.GetComponent<RectTransform> ().anchoredPosition.y;
				component.GetComponent<RectTransform>().anchoredPosition = new Vector2( x, y );
				component.SetActive( show );
			}
			else
				Debug.Log( name + " couldn't be found in " + root.name );
		}

		public void SetAnchoredPositionY( string name, float y, bool show = false )
		{
			GameObject component = null;
			if ( gameObjects.TryGetValue( name, out component ) )
			{
				var x = component.GetComponent<RectTransform> ().anchoredPosition.x;
				component.GetComponent<RectTransform>().anchoredPosition = new Vector2( x, y );
				component.SetActive( show );
			}
			else
				Debug.Log( name + " couldn't be found in " + root.name );
		}


		public void SetItemChildrenActiveState( string name, bool active )
		{
			var children = GetChildren( name );
			for ( int i = 0; i < children.Length; i++ )
                children[i].SetActive( active );
		}

		#endregion

		#region [Get Functions]

        public string GetText( string componentName, bool internalTxt = false )
        {
            GameObject go;
            if ( gameObjects.TryGetValue( componentName, out go ) )
            {
				TextMeshProUGUI txt = internalTxt ? go.GetComponentInChildren<TextMeshProUGUI>() : go.GetComponent<TextMeshProUGUI>();
                if ( txt )
                {
                    return txt.text;
                }
            }

            return string.Empty;
        }

		public string GetInputFieldValue( string componentName, bool internalTxt = false )
		{
			GameObject go;
			if ( gameObjects.TryGetValue( componentName, out go ) )
			{
				TMP_InputField txt = internalTxt ? go.GetComponentInChildren<TMP_InputField>() : go.GetComponent<TMP_InputField>();
				if ( txt )
				{
					return txt.text;
				}
			}

			return string.Empty;
		}

		public float GetSliderValue( string componentName, bool internalTxt = false )
		{
			GameObject go;
			if ( gameObjects.TryGetValue( componentName, out go ) )
			{
				Slider slider = internalTxt ? go.GetComponentInChildren<Slider>() : go.GetComponent<Slider>();
				if ( slider )
				{
					return slider.value;
				}
			}

			return 0.0f;
		}

		public Sprite GetImageSprite( string componentName, bool internalImg = false )
		{
			GameObject go;

			if ( gameObjects.TryGetValue( componentName, out go ) )
			{
				Image img = go.GetComponent<Image>();
				if ( internalImg )
				{
					if ( go.transform.childCount > 0 )
						img = go.transform.GetChild(0).GetComponent<Image>();
				}

				if ( img )
				{
					return img.sprite;
				}
			}

			return null;
		}


        public TextMeshProUGUI GetTextMeshProUGUI( string componentName, bool internalTxt = false )
        {
            GameObject go;
            if ( gameObjects.TryGetValue( componentName, out go ) )
            {
                TextMeshProUGUI txt = internalTxt ? go.GetComponentInChildren<TextMeshProUGUI>() : go.GetComponent<TextMeshProUGUI>();
                return txt;
            }

            return null;
        }


		public RectTransform GetRootRect()
		{
			return root.GetComponent<RectTransform>();
		}
		public float GetRootRectPositionX()
		{
			return GetRootRect().anchoredPosition.x;
		}
		public float GetRootRectPositionY()
		{
			return GetRootRect().anchoredPosition.y;
		}


		public RectTransform GetRect( string name )
		{
			GameObject component = null;
			gameObjects.TryGetValue( name, out component );
			return component.GetComponent<RectTransform>();
		}
		public float GetRectPositionX( string name )
		{
			GameObject component = null;
			gameObjects.TryGetValue( name, out component );
			return component.GetComponent<RectTransform>().anchoredPosition.x;
		}
		public float GetRectPositionY( string name )
		{
			GameObject component = null;
			gameObjects.TryGetValue( name, out component );
			return component.GetComponent<RectTransform>().anchoredPosition.y;
		}


		public float GetRectScaleX( string name )
		{
			GameObject component = null;
			if ( gameObjects.TryGetValue( name, out component ) )
			{
				return component.GetComponent<RectTransform>().localScale.x;
			}
			else
				Debug.Log( name + " couldn't be found in " + root.name );

			return 0.0f;
		}
		public float GetRectScaleY( string name )
		{
			GameObject component = null;
			if ( gameObjects.TryGetValue( name, out component ) )
			{
				return component.GetComponent<RectTransform>().localScale.y;
			}
			else
				Debug.Log( name + " couldn't be found in " + root.name );

			return 0.0f;
		}


		public Graphic GetGraphic( string name )
		{
			GameObject go;

			if ( gameObjects.TryGetValue( name, out go ) )
				return go.GetComponent<Graphic>();

			return null;
		}

		public Color GetGraphicColor( string name )
		{
			GameObject go;

			if ( gameObjects.TryGetValue( name, out go ) )
				return go.GetComponent<Graphic>().color;

			return Color.white;
		}

		#endregion
    }
}