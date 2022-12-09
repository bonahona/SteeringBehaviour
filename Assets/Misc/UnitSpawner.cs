using Fyrvall.SteeringBehaviour;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [Header("Input")]
    public KeyCode SpawnKey = KeyCode.Q;
    public KeyCode DespawnKey = KeyCode.E;

    [Header("Spawn")]
    public SteeringAgent SteeringAgentPrefab;
    public Transform SpawnPosition;

    private List<SteeringAgent> SteeringAgents = new List<SteeringAgent>();

    // Start is called before the first frame update
    void Start()
    {
        SteeringAgents = GameObject.FindObjectsOfType<SteeringAgent>().Where(a => a.UseAgent).ToList();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(SpawnKey)) {
            SpawnNewAgent();
        }

        if(Input.GetKeyDown(DespawnKey)) {
            DespawnAllAgents();
        }
    }

    private void SpawnNewAgent()
    {
        if(SteeringAgentPrefab == null || SpawnPosition == null) {
            return;
        }

        var steeringAgent = GameObject.Instantiate(SteeringAgentPrefab, SpawnPosition.position, Quaternion.identity);
        SteeringAgents.Add(steeringAgent);
    }

    private void DespawnAllAgents()
    {
        foreach (var agent in SteeringAgents) {
            GameObject.Destroy(agent.gameObject);
        }

        SteeringAgents.Clear();
    }
}
