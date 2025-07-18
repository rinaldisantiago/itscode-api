public class Ban
{
    private int idBan;
    private string reason = "";
    private DateTime banDate;
    private DateTime? unbanDate;
    private int idPerson;

    public int IdBan { get { return this.idBan; } set { this.idBan = value; } }
    public string Reason { get { return this.reason; } set { this.reason = value; } }
    public DateTime BanDate { get { return this.banDate; } set { this.banDate = value; } }
    public DateTime? UnbanDate { get { return this.unbanDate; } set { this.unbanDate = value; } }
    public int IdPerson { get { return this.idPerson; } set { this.idPerson = value; } }
}