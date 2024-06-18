[System.Serializable]
public class PlayerData
{
    public float[] position;
    public float elepsedTime;

    public PlayerData(PlayerMovement playerMovement)
    {
        this.elepsedTime = playerMovement.elepsedTime;

        position = new float[3];
        position[0] = playerMovement.transform.position.x;
        position[1] = playerMovement.transform.position.y;
        position[2] = playerMovement.transform.position.z;
    }
}