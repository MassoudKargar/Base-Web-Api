namespace Base.Domain.Generics;
public class Controller
{
    public Controller()
    {
        SPName = String.Empty;
        Caption = String.Empty;
    }
    public Guid Id { get; set; }
    public DateTime InsDate { get; set; }
    public string SPName { get; set; }
    public string Caption { get; set; }
}
