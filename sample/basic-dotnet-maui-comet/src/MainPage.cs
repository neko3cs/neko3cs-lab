namespace BasicDotnetMauiComet;

public class MainPage : View
{

    [State]
    private readonly CometRide _comet = new();

    [Body]
    private new View Body() => new VStack {
        new Text(()=> $"({_comet.Rides}) rides taken:{_comet.CometTrain}")
            .Frame(width:300)
            .LineBreakMode(LineBreakMode.CharacterWrap),

        new Button("Ride the Comet! ☄️", ()=>{
            _comet.Rides++;
        })
            .Frame(height:44)
            .Margin(8)
            .Color(Colors.White)
            .Background(Colors.Green)
        .RoundedBorder(color:Colors.Blue)
        .Shadow(Colors.Grey,4,2,2),
    };

    public class CometRide : BindingObject
    {
        public int Rides
        {
            get => GetProperty<int>();
            set => SetProperty(value);
        }

        public string CometTrain => "☄️".Repeat(Rides);
    }
}