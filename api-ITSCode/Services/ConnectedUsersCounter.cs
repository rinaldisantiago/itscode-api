public sealed class ConnectedUsersCounter
{
    private static readonly Lazy<ConnectedUsersCounter> _instance =
        new Lazy<ConnectedUsersCounter>(() => new ConnectedUsersCounter());

    private int _connectedUsers = 0;

    // Propiedad para obtener la instancia global
    public static ConnectedUsersCounter Instance => _instance.Value;

    // Constructor privado para evitar instanciación externa
    private ConnectedUsersCounter() {}

    public int GetCount()
    {
        return _connectedUsers;
    }

    public void AddUser()
    {
        Interlocked.Increment(ref _connectedUsers);
    }

    public void RemoveUser()
    {
        // Evitamos números negativos
        if (_connectedUsers > 0)
            Interlocked.Decrement(ref _connectedUsers);
    }
}
