[System.Serializable]
public class PlayerData
{
    public float[] position;
    public float time;

    public PlayerData(PlayerMovement playerMovement)
    {
        position = new float[3];
        position[0] = playerMovement.transform.position.x;
        position[1] = playerMovement.transform.position.y;
        position[2] = playerMovement.transform.position.z;
    }
}