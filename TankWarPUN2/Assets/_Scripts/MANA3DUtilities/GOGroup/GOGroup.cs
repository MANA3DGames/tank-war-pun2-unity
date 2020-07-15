using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace MANA3DGames
{
	/// <summary>
	/// A major class that encapsulates a group of GameObjects, for easy access and modification.
	/// </summary>
	public class GOGroup
	{
		#region [Variables]

		/// <summary>
		/// The root of this group.
		/// </summary>
		protected GameObject root;
		/// <summary>
		/// Gets a value indicating whether this instance is active.
		/// </summary>
		/// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
		public bool IsActive { get { return root.activeSelf; } }

		/// <summary>
		/// The dictionary of gameObjects in this group.
		/// </summary>
		protected Dictionary<string, GameObject> gameObjects;
		/// <summary>
		/// Gets the gameObjects.
		/// </summary>
		/// <value>The gameObjects.</value>
		public Dictionary<string, GameObject> Items { get { return gameObjects; } }

		#endregion

		#region [Constructors]

		/// <summary>
		/// Initializes a new instance of the <see cref="MANA3DGames.GOGroup"/> class.
		/// </summary>
		/// <param name="root">Root GameObject.</param>
		public GOGroup( GameObject root )
		{
			// Assaign given root to this instance root.
			this.root = root;
			// Create a new empty items dictionary.
			this.gameObjects = new Dictionary<string, GameObject>();

			// Check if this root has any children.
			if ( root.transform.childCount > 0 )
			{
				// Iterate through children and add each one of them to this dictionary.
				for ( int i = 0; i < root.transform.childCount; i++ )
				{
					// Add the name of the child as the key and the gameObject as the value.
					gameObjects.Add( root.transform.GetChild(i).name, 
							   root.transform.GetChild(i).gameObject );
				}
			}
		}

		/// <summary>
		/// Destroy this instance.
		/// </summary>
		public virtual void Destroy()
		{
			// Clear gameObjects dictionary.
			gameObjects.Clear();
			// Null it out.
			gameObjects = null;
			// Destroy root gameObject.
			GameObject.Destroy( root );
		}

		#endregion


        public void Remove( string name )
        {
            GameObject go;
            if ( gameObjects.TryGetValue( name, out go ) )
            {
                GameObject.Destroy( go );
                gameObjects.Remove(name);
            }
        }

        public virtual void RemoveAll()
        {
            foreach ( var item in gameObjects )
            {
                if ( item.Value ) 
                    GameObject.Destroy( item.Value );
            }
            // Clear gameObjects dictionary.
            gameObjects.Clear();
        }

        #region [Show/Hide Functions]

        /// <summary>
        /// Shows the root GameObject.
        /// </summary>
        /// <param name="show">If set to <c>true</c> show.</param>
        public void ShowRoot( bool show )
		{
			// Activate/Diactivate root gameObject. 
			root.SetActive( show );
		}

		/// <summary>
		/// Shows all gameObjects.
		/// </summary>
		/// <param name="show">If set to <c>true</c> show.</param>
		public void ShowAllChildren( bool show )
		{
			// Iterate through gameObjects dictionary and activate/deactivate its value (gameObject itself).
			foreach (var item in gameObjects)
				item.Value.SetActive( show );
		}

		/// <summary>
		/// Shows a gameObject.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="show">If set to <c>true</c> show.</param>
		public void ShowGameObject( string name, bool show )
		{
			// Try to find a gameObject with the given name in items dictionary.
			GameObject go = Get( name );
			if ( go )
				// Activate/Deactivate target gameObject.
				go.SetActive( show );
			else
				// Could not find the requested item.
				Debug.Log( name + " couldn't be found in " + root.name );
		}

		#endregion

		#region [Set Functions]

		/// <summary>
		/// Sets the position of a gameObject.
		/// </summary>
		/// <param name="name">GameObject's Name.</param>
		/// <param name="pos">Position.</param>
		/// <param name="show">If set to <c>true</c> show.</param>
		public void SetPosition( string name, Vector2 pos, bool show = false )
		{
			// Try to find a gameObject with the given name in gameObjects dictionary.
			GameObject go = Get( name );
			if ( go )
			{
				// Set the target gameObject position to the given position.
				go.GetComponent<Transform>().position = pos;
				// Activate/Deactivete target gameObject.
				go.SetActive( show );
			}
			else
				Debug.Log( name + " couldn't be found in " + root.name );
		}

		/// <summary>
		/// Sets the position x of a gameObject.
		/// </summary>
		/// <param name="name">GameObject's Name.</param>
		/// <param name="val">Value.</param>
		/// <param name="show">If set to <c>true</c> show.</param>
		public void SetPositionX( string name, float val, bool show = false )
		{
			// Try to find a gameObject with the given name in gameObjects dictionary.
			GameObject go = Get( name );
			if ( go )
			{
				// Get current position.
				var pos = go.GetComponent<Transform>().position;
				// Set x component value of the current position as the given value.
				go.GetComponent<Transform>().position = new Vector3( val, pos.y, pos.z );
				go.SetActive( show );
			}
			else
				Debug.Log( name + " couldn't be found in " + root.name );
		}

		/// <summary>
		/// Sets the position y of a gameObject.
		/// </summary>
		/// <param name="name">Target GameObject Name.</param>
		/// <param name="val">Value.</param>
		/// <param name="show">If set to <c>true</c> show.</param>
		public void SetPositionY( string name, float val, bool show = false )
		{
			// Try to find a gameObject with the given name in gameObjects dictionary.
			GameObject go = Get( name );
			if ( go )
			{
				// Get current position.
				var pos = go.GetComponent<Transform>().position;
				// Set y component value of the current position as the given value.
				go.GetComponent<Transform>().position = new Vector3( pos.x, val, pos.z );
				go.SetActive( show );
			}
			else
				Debug.Log( name + " couldn't be found in " + root.name );
		}

		/// <summary>
		/// Sets the position z of a gameObject.
		/// </summary>
		/// <param name="name">Given GameObject Name.</param>
		/// <param name="val">Value.</param>
		/// <param name="show">If set to <c>true</c> show.</param>
		public void SetPositionZ( string name, float val, bool show = false )
		{
			// Try to find a gameObject with the given name in gameObjects dictionary.
			GameObject go = Get( name );
			if ( go )
			{
				// Get current position.
				var pos = go.GetComponent<Transform>().position;
				// Set z component value of the current position as the given value.
				go.GetComponent<Transform>().position = new Vector3( pos.x, pos.y, val );
				go.SetActive( show );
			}
			else
				Debug.Log( name + " couldn't be found in " + root.name );
		}

		/// <summary>
		/// Sets the local position of a gameObject.
		/// </summary>
		/// <param name="name">Target GameObject's Name.</param>
		/// <param name="pos">Position.</param>
		/// <param name="show">If set to <c>true</c> show.</param>
		public void SetLocalPosition( string name, Vector2 pos, bool show = false )
		{
			// Try to find a gameObject with the given name in gameObjects dictionary.
			GameObject go = Get( name );
			if ( go )
			{
				// Set the target gameObject local position to the given local position.
				go.GetComponent<Transform>().localPosition = pos;
				// Activate/Deactivete target gameObject.
				go.SetActive( show );
			}
			else
				Debug.Log( name + " couldn't be found in " + root.name );
		}

		/// <summary>
		/// Sets the local position x of a gameObject.
		/// </summary>
		/// <param name="name">Target GameObject Name.</param>
		/// <param name="val">Value.</param>
		/// <param name="show">If set to <c>true</c> show.</param>
		public void SetLocalPositionX( string name, float val, bool show = false )
		{
			// Try to find a gameObject with the given name in gameObjects dictionary.
			GameObject component = null;
			if ( gameObjects.TryGetValue( name, out component ) )
			{
				// Get current local position.
				var pos = component.GetComponent<Transform>().localPosition;
				// Set x component value of the current local position as the given value.
				component.GetComponent<Transform>().localPosition = new Vector3( val, pos.y, pos.z );
				component.SetActive( show );
			}
			else
				Debug.Log( name + " couldn't be found in " + root.name );
		}

		/// <summary>
		/// Sets the local position y of a gameObject.
		/// </summary>
		/// <param name="name">Target GameObject Name.</param>
		/// <param name="val">Value.</param>
		/// <param name="show">If set to <c>true</c> show.</param>
		public void SetLocalPositionY( string name, float val, bool show = false )
		{
			// Try to find a gameObject with the given name in gameObjects dictionary.
			GameObject component = null;
			if ( gameObjects.TryGetValue( name, out component ) )
			{
				// Get current local position.
				var pos = component.GetComponent<Transform>().localPosition;
				// Set y component value of the current local position as the given value.
				component.GetComponent<Transform>().localPosition = new Vector3( pos.x, val, pos.z );
				component.SetActive( show );
			}
			else
				Debug.Log( name + " couldn't be found in " + root.name );
		}

		/// <summary>
		/// Sets the local position z of a gameObject.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="val">Value.</param>
		/// <param name="show">If set to <c>true</c> show.</param>
		public void SetLocalPositionZ( string name, float val, bool show = false )
		{
			// Try to find a gameObject with the given name in gameObjects dictionary.
			GameObject component = null;
			if ( gameObjects.TryGetValue( name, out component ) )
			{
				// Get current local position.
				var pos = component.GetComponent<Transform>().localPosition;
				// Set z component value of the current local position as the given value.
				component.GetComponent<Transform>().localPosition = new Vector3( pos.x, pos.y, val );
				component.SetActive( show );
			}
			else
				Debug.Log( name + " couldn't be found in " + root.name );
		}


		/// <summary>
		/// Sets all game objects rotation.
		/// </summary>
		/// <param name="rotation">Given Rotation.</param>
		public void SetAllGameObjectsRotation( Quaternion rotation )
		{
			// Iterate through all gameObject in gameObjects dictionary.
			foreach ( var go in gameObjects )
				go.Value.transform.rotation = rotation;
		}


		public void SetLocalRotationX( string name, float val )
		{
			// Try to find a gameObject with the given name in gameObjects dictionary.
			GameObject component = null;
			if ( gameObjects.TryGetValue( name, out component ) )
			{
				// Get current local rotation.
				var rotation = component.GetComponent<Transform>().localRotation;
				// Set z component value of the current local position as the given value.
				component.GetComponent<Transform>().localRotation = Quaternion.Euler( val, rotation.y, rotation.z );
			}
			else
				Debug.Log( name + " couldn't be found in " + root.name );
		}

		public void SetLocalRotationZ( string name, float val )
		{
			// Try to find a gameObject with the given name in gameObjects dictionary.
			GameObject component = null;
			if ( gameObjects.TryGetValue( name, out component ) )
			{
				// Get current local rotation.
				var rotation = component.GetComponent<Transform>().localRotation;
				// Set z component value of the current local position as the given value.
				component.GetComponent<Transform>().localRotation = Quaternion.Euler( rotation.x, rotation.y, val );
			}
			else
				Debug.Log( name + " couldn't be found in " + root.name );
		}


		/// <summary>
		/// Sets the mesh text.
		/// </summary>
		/// <param name="name">Target GameObject Name.</param>
		/// <param name="text">Text.</param>
		/// <param name="inner">If set to <c>true</c> inner.</param>
		public void SetMeshText( string name, string text, bool inner )
		{
			// Try to find a gameObject with the given name in gameObjects dictionary.
			GameObject component = null;
			if ( gameObjects.TryGetValue( name, out component ) )
			{
				// Check if the target is a child of founded gameObject.
				if ( inner )
					component.GetComponentInChildren<TMPro.TextMeshPro>().text = text;
				else
					component.GetComponent<TMPro.TextMeshPro>().text = text;
			}
			else
				Debug.Log( name + " couldn't be found in " + root.name );
		}

		#endregion

		#region [Get Functions]

		/// <summary>
		/// Gets the active state of a gameObject.
		/// </summary>
		/// <returns><c>true</c>, if active was gotten, <c>false</c> otherwise.</returns>
		/// <param name="name">Target GameObject's Name.</param>
		public bool GetActive( string name )
		{
			// Try to find a gameObject with the given name in gameObjects dictionary.
			GameObject component = null;
			if ( gameObjects.TryGetValue( name, out component ) )
				return component.activeSelf;
			return false;
		}

		/// <summary>
		/// Gets the root gameObject.
		/// </summary>
		/// <returns>The root.</returns>
		public GameObject GetRoot()
		{
			return root;
		}

		/// <summary>
		/// Get gameObject with the specified name.
		/// </summary>
		/// <param name="name">Name.</param>
		public GameObject Get( string name )
		{
			// Try to find a gameObject with the given name in gameObjects dictionary.
			GameObject go = null;
			gameObjects.TryGetValue( name, out go );
			return go;
		}

		/// <summary>
		/// Gets the transform component of the target gameObject.
		/// </summary>
		/// <returns>The transform.</returns>
		/// <param name="name">Name.</param>
		public Transform GetTransform( string name )
		{
			return Get( name ).transform;
		}

		/// <summary>
		/// Gets the children of a gameObject.
		/// </summary>
		/// <returns>The children.</returns>
		/// <param name="name">Name.</param>
		public GameObject[] GetChildren( string name )
		{
			GameObject[] children = null;
			GameObject go = null;
			if ( gameObjects.TryGetValue( name, out go ) )
			{
				var trans = go.transform;
				children = new GameObject[trans.childCount];
				for ( int i = 0; i < trans.childCount; i++ ) 
					children[i] = trans.GetChild( i ).gameObject;
			}
			return children;
		}

		/// <summary>
		/// Gets the children transforms.
		/// </summary>
		/// <returns>The children transforms.</returns>
		/// <param name="name">Name.</param>
		public Transform[] GetChildrenTransforms( string name )
		{
			Transform[] children = null;
			GameObject go = null;
			if ( gameObjects.TryGetValue( name, out go ) )
			{
				var trans = go.transform;
				children = new Transform[trans.childCount];
				for ( int i = 0; i < trans.childCount; i++ ) 
					children[i] = trans.GetChild( i );
			}
			return children;
		}


		public GameObject[] GetAllMenuItems()
		{
			var transform = root.transform;
			int count = transform.childCount;
			GameObject[] items = new GameObject[count];
			for ( int i = 0; i < count; i++ ) 
			{
				int temp = i;
				items[temp] = transform.GetChild( temp ).gameObject;
			}

			return items;
		}

		public Transform[] GetAllMenuItemsTransform()
		{
			var transform = root.transform;
			int count = transform.childCount;
			Transform[] items = new Transform[count];
			for ( int i = 0; i < count; i++ ) 
			{
				int temp = i;
				items[temp] = transform.GetChild( temp );
			}

			return items;
		}

		#endregion
	}
}