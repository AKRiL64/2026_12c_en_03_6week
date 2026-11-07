using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneActionDirector: MonoBehaviour
{
    private InputSystem_Actions input;
    
    [SerializeField] private List<CutsceneDialogueActionEntry> dialogueActionsList = new List<CutsceneDialogueActionEntry>();
    [SerializeField] private string nextScene;
    
    private Dictionary<int, List<CutsceneAction>> actions = new Dictionary<int, List<CutsceneAction>>();
    private int currentIndex = -1;
    private void Awake()
    {
        input = new InputSystem_Actions();
        input.Enable();
        
        foreach (CutsceneDialogueActionEntry entry in dialogueActionsList)
        {
            if (!actions.ContainsKey(entry.Index))
            {
                actions.Add(entry.Index, new List<CutsceneAction>());
            }
            actions[entry.Index].Add(entry.Action);
        }
    }

    private void Start()
    {
        NextStep();
    }
    
    private void OnEnable()
    {
        input?.Enable();
    }

    private void OnDisable()
    {
        input?.Disable();
    }

    private void OnDestroy()
    {
        input?.Dispose();
    }
    
    private void Update()
    {
        if (input.Player.Jump.WasReleasedThisFrame())
        {
            NextStep();
        }
    }

    private void NextStep()
    {
        if (actions.Count == 0)
        {
            Debug.LogWarning("No actions found");
            return;
        }

        currentIndex++;

        if (!actions.TryGetValue(currentIndex, out List<CutsceneAction> list) || list.Count == 0)
        {
            Debug.LogWarning("No actions found for index " + currentIndex);
            NextScene();
            return;
        }

        foreach (CutsceneAction cutsceneAction in list)
        {
            cutsceneAction.Execute();
        }
        
        //get the next index element from actions and call execute for every action in such list
        //if there is no actions set in that index or no such action whatsoever, warn and call (NextScene)
    }

    private void NextScene()
    {
        if (string.IsNullOrWhiteSpace(nextScene))
        {
            Debug.LogWarning("Next scene not specified");
            return;
        }
        
        SceneManager.LoadScene(nextScene);
        
        //load the scene under the specified name in the "nextscene" variable
    }
}
