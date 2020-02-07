=== How to Set Up a New Scene ===

Each Scene must have:

	1) SceneState.cs

public enum mySceneState
{
	None = 0,
	State1 = 1,
	etc
}

	2) SceneMenuUIController.cs

public SceneMenuUIController : Monobehaviour {
void OnEnable()
{
	StateController.StateChanged += OnStateChanged;
}

void OnDisable()
{
	StateController.StateChanged -= OnStateChanged;
}

private void OnStateChanged(int state)
{
	mySceneState enumState = (mySceneState)state;
	
	switch(enumState)
	{
		/*
		This is where you add cases for each state of your scene
		*/
		default:
			break;
	}
}
}

=== ===

All LevelController gameObjects have the StateController.cs component, and all UI Root gameObjects have the SceneMenuUIController.cs component.
All actions are driven from the SceneMenuUIController.cs unique to each scene, as well as the unique mySceneState enum. 