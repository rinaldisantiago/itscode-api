public sealed class Singleton
{
    private static Singleton _instance;


    private int _connectedUsers = 0;

    private Singleton() {}

    public static Singleton GetInstance()
    {
        if (_instance == null)
        {

            _instance = new Singleton();

        }
        return _instance;
    }

    public int GetCount() => _connectedUsers;

    public void AddUser() => Interlocked.Increment(ref _connectedUsers);

    public void RemoveUser()
    {
        if (_connectedUsers > 0)
            Interlocked.Decrement(ref _connectedUsers);
    }
}
