public class Role
{
    private int id;
    private string name = "";
    private RoleEnum roleEnum;

    public int Id { get { return this.id; } set { this.id = value; } }
    public string Name { get { return this.name; } set { this.name = value; } }
    public RoleEnum RoleEnum { get { return this.roleEnum; } set { this.roleEnum = value; } }   
}