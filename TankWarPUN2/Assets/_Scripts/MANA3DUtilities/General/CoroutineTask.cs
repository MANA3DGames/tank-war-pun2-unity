using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MANA3DGames
{
	public class CoroutineTask
	{
		#region [Public Events]

		public event System.Action<bool> onCoroutineTaskComplete;

		#endregion

		#region [Global Variables]

		private bool gIsAlive;								    // Indicates that the coroutine is runnning.
		private bool gIsPaused;									// Indicates that the coroutine is paused.
		private bool gCoroutineTaskWasKilled;					// Indicates the the coroutine was stopped (terminated).

		private IEnumerator gCoroutine;							// Running coroutine.
		private Stack<CoroutineTask> gSubCoroutineTaskStack;	// Stack of all subcoroutine tasks.

		#endregion

		#region [Constructors]

		public CoroutineTask( IEnumerator coroutine ) : this( coroutine, true )
		{
			// ...
		}

		public CoroutineTask( IEnumerator coroutine, bool shouldStart )
		{
			// Set the current coroutine.
			gCoroutine = coroutine;

			// Check if it should start immediately.
			if ( shouldStart )
				Start();
		}

		#endregion

		#region [Execution]

		private IEnumerator ExecuteTask()
		{
			// Null out the first run through in the case we start paused.
			yield return null;

			// Execute the task as long as its status is running. 
			while ( gIsAlive )
			{
				// Check if the task is paused.
				if ( gIsPaused )
				{
					yield return null;
				}
				// The task is still running.
				else
				{
					// Run the next iteration and stop if we are done.
					if ( gCoroutine.MoveNext() )
					{
						yield return gCoroutine.Current;
					}
					else
					{
						// Run subCoroutine tasks if we have any.
						if ( gSubCoroutineTaskStack != null )
							yield return CoroutineCoordinator.Instance.StartCoroutine( ExecuteSubTask() );

						gIsAlive = false;
					}
				}
			}

			// Fire off a complete task event (action).
			if ( onCoroutineTaskComplete != null )
				onCoroutineTaskComplete( gCoroutineTaskWasKilled );
		}

		private IEnumerator ExecuteSubTask()
		{
			// Check if there is any sub-coroutines in the stack.
			if ( gSubCoroutineTaskStack != null && gSubCoroutineTaskStack.Count > 0 )
			{
				do
				{
					// Get first sub-task.
					CoroutineTask subTask = gSubCoroutineTaskStack.Pop();

					// Execute the sub task.
					yield return CoroutineCoordinator.Instance.StartCoroutine( subTask.StartAsCoroutine() );
				}
				// Keep executing the sub tasks as long as there is any.
				while( gSubCoroutineTaskStack.Count > 0 );
			}
		}

		#endregion

		#region [Sub-Task]

		public CoroutineTask CreateAndAddSubCoroutineTask( IEnumerator coroutine )
		{
			var j = new CoroutineTask( coroutine, false );
			AddSubCoroutineTask( j );
			return j;
		}

		public void AddSubCoroutineTask( CoroutineTask subCoroutineTask )
		{
			if ( gSubCoroutineTaskStack == null )
				gSubCoroutineTaskStack = new Stack<CoroutineTask>();
			gSubCoroutineTaskStack.Push( subCoroutineTask );
		}

		public void RemoveSubCoroutineTask( CoroutineTask subCoroutineTask )
		{
			if ( gSubCoroutineTaskStack.Contains( subCoroutineTask ) )
			{
				var subCoroutineTaskStack = new Stack<CoroutineTask>( gSubCoroutineTaskStack.Count - 1 );
				var allCurrentSubTasks = gSubCoroutineTaskStack.ToArray();
				System.Array.Reverse( allCurrentSubTasks );

				for ( var i = 0; i < allCurrentSubTasks.Length; i++ )
				{
					var j = allCurrentSubTasks[i];
					if ( j != subCoroutineTask )
						subCoroutineTaskStack.Push( j );
				}

				// assign the new stack
				gSubCoroutineTaskStack = subCoroutineTaskStack;
			}
		}

		#endregion

		#region [Start]

		public void Start()
		{
			gIsAlive = true;
			CoroutineCoordinator.Instance.StartCoroutine( ExecuteTask() );
		}

		public IEnumerator StartAsCoroutine()
		{
			gIsAlive = true;
			yield return CoroutineCoordinator.Instance.StartCoroutine( ExecuteTask() );
		}

		#endregion

		#region [Pause/UnPause]

		public void Pause()
		{
			gIsPaused = true;
		}

		public void UnPause()
		{
			gIsPaused = false;
		}

		#endregion

		#region [Kill]

		public void Kill()
		{
			gCoroutineTaskWasKilled = true;
			gIsAlive = false;
			gIsPaused = false;
		}

		public void Kill( float delayInSeconds ) 
		{
			var delay = (int)( delayInSeconds * 1000 );

			new System.Threading.Timer( obj => 
				{
					lock( this )
					{
						Kill();
					}
				}, null, delay, System.Threading.Timeout.Infinite );

		}
			
		#endregion


		#region [Static Functions]

		public static CoroutineTask Create( IEnumerator coroutine )
		{
			// Call a proper constructor.
			return new CoroutineTask( coroutine );
		}

		public static CoroutineTask Create( IEnumerator coroutine, bool shouldStart )
		{
			// Call a proper constructor.
			return new CoroutineTask( coroutine, shouldStart );
		}

		#endregion
	}

    public class CoroutineCoordinator : MonoBehaviour
    {
        #region [Properties]

		private static CoroutineCoordinator _instance = null;

       	/// <summary>
		/// Singleton pattern to have only one instance for the whole game.
       	/// </summary>
       	/// <value>The instance.</value>
        public static CoroutineCoordinator Instance
        {
            get
            {
                // Check if we don't have an instance of the class.
                if ( !_instance )
                {
                    // Check if an CoroutineCoordinator is already available in the scene.
                    _instance = FindObjectOfType(typeof(CoroutineCoordinator)) as CoroutineCoordinator;

                    // Create a new one if there is none.
                    if (!_instance)
                    {
                        // Create an enmpty gameobject.
                        GameObject obj = new GameObject("_CoroutineCoordinator");

                        // Add CoroutineCoordinator script to the empty gameobject.
                        _instance = obj.AddComponent<CoroutineCoordinator>();
                    }
                }

                // Return the instance.
                return _instance;
            }
        }

        #endregion

        #region MonoBehaviour

        void OnApplicationQuit()
        {
            // Release reference on exist.
            _instance = null;
        }

        #endregion
    }
}