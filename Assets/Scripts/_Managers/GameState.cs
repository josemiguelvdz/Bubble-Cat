public enum Checkpoint {spawnpoint, checkpoint1, checkpoint2, checkpoint3, checkpoint5} // Se encarga de guardar el checkpoint actual
                                                                                        // en el que te encuentres

public static class GameState
{
    public static Checkpoint currentCheckpoint = Checkpoint.spawnpoint;

    public static int currentSoap = 0;

}
