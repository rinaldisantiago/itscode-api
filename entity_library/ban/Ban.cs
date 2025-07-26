public class Ban
{
    private int id;
    private string reason = "";
    private DateTime banDate;
    private DateTime? unbanDate;
    private User user;

    public int IdBan { get { return this.id; } set { this.id = value; } }
    public string Reason { get { return this.reason; } set { this.reason = value; } }
    public DateTime BanDate { get { return this.banDate; } set { this.banDate = value; } }
    public DateTime? UnbanDate { get { return this.unbanDate; } set { this.unbanDate = value; } }
    public User User { get { return this.user; } set { this.user = value; } }
}