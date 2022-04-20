using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public float numOfHumans;
    public float numOfZombies;
    public GameObject humanPrefab;
    public GameObject sharkPrefab;
    public GameObject playerPrefab;
    public List<Human> humans;
    public List<Shark> sharks;
    public Player player;
    public MeshRenderer floor;
    private Bounds worldBounds;

    public Bounds WorldBounds => worldBounds;

    public List<Obstacle> obstacles;

    // Start is called before the first frame update
    void Start()
    {
        humans = new List<Human>();
        sharks = new List<Shark>();
        SpawnAgents();
        worldBounds = floor.bounds;
    }

    // Update is called once per frame
    void Update()
    {
        for (int j = sharks.Count - 1; j >= 0; j--)
        {
            for (int i = humans.Count - 1; i >= 0; i--)
            {
                if(Collision(humans[i], sharks[j]))
                {
                    // Remove fish from scene and list
                    Destroy(humans[i].gameObject);

                    humans.RemoveAt(i);

                    // Update shark's score accordingly
                    sharks[j].Score++;

                    // Makes shark bigger
                    Vector3 sizeChange = new Vector3(0.5f, 0.5f, 0.5f);

                    sharks[j].transform.localScale = sharks[j].transform.localScale + sizeChange;

                    // Makes shark faster
                    sharks[j].maxSpeed += 0.5f;
                }
            }
        }

        for (int i = 0; i < humans.Count; i++)
        {
            if(Collision(humans[i], player))
            {
                // Remove fish from scene and list
                Destroy(humans[i].gameObject);

                humans.RemoveAt(i);

                player.Score++;

                // Makes shark bigger
                Vector3 sizeChange = new Vector3(0.5f, 0.5f, 0.5f);

                player.transform.localScale = player.transform.localScale + sizeChange;

                // Makes shark faster
                player.maxSpeed += 0.5f;
            }
        }

        if (humans.Count == 0)
        {
            // Sets the variables for the winner shark and max score
            float maxScore = 0;
            Shark bestSharkie = null;
            
            // Compares each shark's score to the current max score, if they have a higher score, they are set to the new best sharkie!!
            foreach (Shark shark in sharks)
            {
                if (shark.Score > maxScore)
                {
                    bestSharkie = shark;
                    maxScore = shark.Score;
                }
            }

            // If the player has the highest score they are best sharkie!!
            if (player.Score >= maxScore)
            {
                bestSharkie = player;
            }

            if (bestSharkie != player)
            {
                Debug.Log($"The winner and best sharkie is another sharkie with a score of {bestSharkie.Score}");
            }
            else
            {
                Debug.Log($"The winner and best sharkie is YOU with a score of {bestSharkie.Score}");
            }

            bestSharkie.transform.localScale = new Vector3(20f, 20f, 20f);

            // Every shark now wanders
            foreach (Shark shark in sharks)
            {
                shark.Wander();
            }
        }

    }

    public void SpawnAgents()
    {
        // Gets the min and max of the X and Y coords via the floor information
        float minX = (floor.transform.position.x - (floor.transform.localScale.x / 2)) * 10;
        float maxX = (floor.transform.position.x + (floor.transform.localScale.x / 2)) * 10;
        float minZ = (floor.transform.position.z - (floor.transform.localScale.z / 2)) * 10;
        float maxZ = (floor.transform.position.z + (floor.transform.localScale.z / 2)) * 10;

        for (int i = 0; i < numOfHumans; i++)
        {
            Human human = Instantiate(humanPrefab, new Vector3(Random.Range(minX, maxX), 0, Random.Range(minZ, maxZ)), Quaternion.identity).GetComponent<Human>();
            humans.Add(human);
        }

        for (int i = 0; i < numOfZombies; i++)
        {
            Shark zombie = Instantiate(sharkPrefab, new Vector3(Random.Range(minX, maxX), 0, Random.Range(minZ, maxZ)), Quaternion.identity).GetComponent<Shark>();

            sharks.Add(zombie);
        }

        Player playerShark = Instantiate(playerPrefab, new Vector3(Random.Range(minX, maxX), 0, Random.Range(minZ, maxZ)), Quaternion.identity).GetComponent<Player>();
        player = playerShark;
    }

    public bool Collision(Human human, Shark shark)
    {
        float humanMinX = human.gameObject.transform.position.x - (human.gameObject.transform.localScale.x / 2);
        float humanMaxX = human.gameObject.transform.position.x + (human.gameObject.transform.localScale.x / 2);
        float humanMinZ = human.gameObject.transform.position.z - (human.gameObject.transform.localScale.z / 2);
        float humanMaxZ = human.gameObject.transform.position.z + (human.gameObject.transform.localScale.z / 2);
        float sharkMinX = shark.gameObject.transform.position.x - (shark.gameObject.transform.localScale.x / 2);
        float sharkMaxX = shark.gameObject.transform.position.x + (shark.gameObject.transform.localScale.x / 2);
        float sharkMinZ = shark.gameObject.transform.position.z - (shark.gameObject.transform.localScale.z / 2);
        float sharkMaxZ = shark.gameObject.transform.position.z + (shark.gameObject.transform.localScale.z / 2);

        if (humanMinX < sharkMaxX && humanMaxX > sharkMinX && humanMaxZ > sharkMinZ && humanMinZ < sharkMaxZ)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
