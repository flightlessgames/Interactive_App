=== How to Set Up a New Scene ===

Each Scene must have:

	1) SceneState

public enum mysceneState
{
	None = 0,
	State1 = 1,
	etc
}

	2) SceneMenuController

public static event Action<mysceneState> StateChanged = delegate { };
public static mysceneState State = None;

public void ChangeState(int newState)
{
	State = (mysceneState)newState;
	StateChanged.Invoke(State);
}

//optional
private void Start()
{
	ChangeState(1);
}

	3) SceneMenuUIController

void OnEnable()
{
	mysceneMenuController.StateChanged += OnStateChanged;
}

void OnDisable()
{
	mysceneMenuController.StateChanged -= OnStateChanged;
}

private void OnStateChanged(mysceneState state)
{
	switch(state)
	{
		/*
		This is where you add cases for each state of your scene
		*/
		default:
			break;
	}
}

=== ===

The Code feels redundant here, but it inherently requires small hard-coded changes like mysceneState enum being written in. I don't see a way to inherit these behaviours or change them modularly without rewriting them like this. If you can find a better solution go ahead, but don't blindly trouble-shoot, the core scenes are already set-up and there's no need to fix what's not broken.